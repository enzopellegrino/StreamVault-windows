using StreamVault.Models;

namespace StreamVault.Services;

/// <summary>
/// Service for managing multiple simultaneous FFmpeg streams
/// </summary>
public class MultiStreamService
{
    private readonly LoggingService _logger;
    private readonly Dictionary<Guid, FFmpegService> _activeStreams = new();
    private readonly ChromeManagementService _chromeService;

    public MultiStreamService(LoggingService logger)
    {
        _logger = logger;
        _chromeService = new ChromeManagementService(logger);
    }

    /// <summary>
    /// Event fired when a stream starts
    /// </summary>
    public event EventHandler<StreamSession>? StreamStarted;

    /// <summary>
    /// Event fired when a stream stops
    /// </summary>
    public event EventHandler<StreamSession>? StreamStopped;

    /// <summary>
    /// Event fired when stream status changes
    /// </summary>
    public event EventHandler<(StreamSession Session, string Status)>? StreamStatusChanged;

    /// <summary>
    /// Start streaming on all monitors
    /// </summary>
    public async Task<bool> StartAllStreamsAsync(MultiStreamConfig config)
    {
        try
        {
            _logger.Log($"Starting multi-stream setup with {config.StreamSessions.Count} monitors");

            // Step 1: Launch Chrome on all monitors if enabled
            if (config.AutoStartChrome)
            {
                var monitors = config.StreamSessions.Select(s => s.Monitor).ToList();
                var chromeSuccess = await _chromeService.LaunchChromeOnAllMonitorsAsync(monitors, config.DefaultChromeUrl);
                
                if (!chromeSuccess)
                {
                    _logger.LogWarning("Some Chrome instances failed to launch, but continuing with streaming");
                }
                
                // Wait for Chrome to fully load
                await Task.Delay(3000);
            }

            // Step 2: Start streaming for each monitor
            var allSuccess = true;
            foreach (var session in config.StreamSessions)
            {
                var success = await StartStreamSessionAsync(session);
                if (!success)
                {
                    allSuccess = false;
                }
            }

            _logger.Log($"Multi-stream startup completed. Success: {allSuccess}");
            return allSuccess;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error starting multi-stream: {ex.Message}", ex);
            return false;
        }
    }

    /// <summary>
    /// Start streaming for a specific session
    /// </summary>
    public async Task<bool> StartStreamSessionAsync(StreamSession session)
    {
        try
        {
            if (_activeStreams.ContainsKey(session.Id))
            {
                _logger.LogWarning($"Stream already active for {session.Monitor.DeviceName}");
                return false;
            }

            _logger.Log($"Starting stream: {session}");

            // Create FFmpeg service for this stream
            var ffmpegService = new FFmpegService(_logger);
            
            // Subscribe to FFmpeg events
            ffmpegService.ProcessStarted += (sender, e) => OnStreamStarted(session);
            ffmpegService.ProcessStopped += (sender, e) => OnStreamStopped(session);
            ffmpegService.DataReceived += (sender, data) => OnStreamDataReceived(session, data);

            // Start the stream
            var success = await ffmpegService.StartScreenCaptureAsync(
                session.Monitor.DeviceName,
                session.SrtUrl,
                session.Fps,
                session.Bitrate
            );

            if (success)
            {
                _activeStreams[session.Id] = ffmpegService;
                session.IsActive = true;
                session.StartTime = DateTime.Now;
                session.Status = "Streaming";
                
                StreamStarted?.Invoke(this, session);
                _logger.Log($"Stream started successfully: {session.Monitor.DeviceName}");
                return true;
            }
            else
            {
                session.Status = "Failed to start";
                _logger.LogError($"Failed to start stream: {session.Monitor.DeviceName}");
                return false;
            }
        }
        catch (Exception ex)
        {
            session.Status = $"Error: {ex.Message}";
            _logger.LogError($"Error starting stream {session.Monitor.DeviceName}: {ex.Message}", ex);
            return false;
        }
    }

    /// <summary>
    /// Stop streaming for a specific session
    /// </summary>
    public async Task<bool> StopStreamSessionAsync(StreamSession session)
    {
        try
        {
            if (!_activeStreams.TryGetValue(session.Id, out var ffmpegService))
            {
                _logger.LogWarning($"No active stream found for {session.Monitor.DeviceName}");
                return true;
            }

            _logger.Log($"Stopping stream: {session.Monitor.DeviceName}");

            var success = await ffmpegService.StopAsync();
            
            _activeStreams.Remove(session.Id);
            session.IsActive = false;
            session.Status = success ? "Stopped" : "Stop failed";
            
            await ffmpegService.DisposeAsync();
            
            StreamStopped?.Invoke(this, session);
            _logger.Log($"Stream stopped: {session.Monitor.DeviceName}");
            
            return success;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error stopping stream {session.Monitor.DeviceName}: {ex.Message}", ex);
            return false;
        }
    }

    /// <summary>
    /// Stop all active streams
    /// </summary>
    public async Task<bool> StopAllStreamsAsync(List<StreamSession> sessions)
    {
        try
        {
            _logger.Log("Stopping all streams...");

            var stopTasks = sessions.Where(s => s.IsActive)
                                  .Select(s => StopStreamSessionAsync(s))
                                  .ToArray();

            var results = await Task.WhenAll(stopTasks);
            var allSuccess = results.All(r => r);

            // Close all Chrome instances
            await _chromeService.CloseAllChromeInstancesAsync();

            _logger.Log($"All streams stopped. Success: {allSuccess}");
            return allSuccess;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error stopping all streams: {ex.Message}", ex);
            return false;
        }
    }

    /// <summary>
    /// Get active stream count
    /// </summary>
    public int GetActiveStreamCount()
    {
        return _activeStreams.Count;
    }

    /// <summary>
    /// Get session status
    /// </summary>
    public string GetSessionStatus(StreamSession session)
    {
        if (_activeStreams.ContainsKey(session.Id))
        {
            var ffmpeg = _activeStreams[session.Id];
            return ffmpeg.IsRunning ? "Streaming" : "Starting...";
        }
        return session.Status;
    }

    /// <summary>
    /// Generate SRT URLs for all monitors
    /// </summary>
    public static List<StreamSession> GenerateStreamSessions(List<MonitorInfo> monitors, string baseHost = "127.0.0.1", int basePort = 9999)
    {
        var sessions = new List<StreamSession>();
        
        for (int i = 0; i < monitors.Count; i++)
        {
            var session = new StreamSession
            {
                Monitor = monitors[i],
                SrtUrl = $"srt://{baseHost}:{basePort + i}",
                Fps = 30,
                Bitrate = 2000
            };
            
            sessions.Add(session);
        }
        
        return sessions;
    }

    /// <summary>
    /// Check if Chrome is available
    /// </summary>
    public bool IsChromeAvailable()
    {
        return _chromeService.IsChromeAvailable();
    }

    /// <summary>
    /// Get Chrome version
    /// </summary>
    public async Task<string> GetChromeVersionAsync()
    {
        return await _chromeService.GetChromeVersionAsync();
    }

    private void OnStreamStarted(StreamSession session)
    {
        session.Status = "Streaming";
        StreamStatusChanged?.Invoke(this, (session, "Streaming"));
    }

    private void OnStreamStopped(StreamSession session)
    {
        session.Status = "Stopped";
        StreamStatusChanged?.Invoke(this, (session, "Stopped"));
    }

    private void OnStreamDataReceived(StreamSession session, string data)
    {
        // Update session with streaming progress if needed
        if (data.Contains("frame="))
        {
            session.Status = "Streaming (active)";
        }
    }

    /// <summary>
    /// Dispose resources
    /// </summary>
    public async Task DisposeAsync()
    {
        // Stop all active streams
        var sessions = _activeStreams.Keys.Select(id => 
            new StreamSession { Id = id }).ToList();
        
        foreach (var session in sessions)
        {
            await StopStreamSessionAsync(session);
        }

        await _chromeService.DisposeAsync();
    }
}
