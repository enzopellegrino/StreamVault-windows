using StreamVault.Models;
using System.Diagnostics;

namespace StreamVault.Services;

/// <summary>
/// Service for managing FFmpeg-based streaming to SRT
/// </summary>
public class StreamingService
{
    private readonly LoggingService _logger;
    private Process? _ffmpegProcess;
    private bool _isStreaming = false;

    public StreamingService(LoggingService logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Starts streaming with the specified configuration
    /// </summary>
    public async Task StartStreamingAsync(StreamingConfig config)
    {
        if (_isStreaming)
        {
            throw new InvalidOperationException("Streaming is already active");
        }

        if (!config.IsValid())
        {
            throw new ArgumentException("Invalid streaming configuration");
        }

        try
        {
            _logger.Log($"Starting streaming with config: {config}");

            // Verify FFmpeg is available
            if (!await IsFFmpegAvailableAsync())
            {
                throw new FileNotFoundException("FFmpeg not found. Please ensure FFmpeg is installed and available in PATH.");
            }

            // Build FFmpeg command
            var ffmpegArgs = BuildFFmpegCommand(config);
            _logger.Log($"FFmpeg command: ffmpeg {ffmpegArgs}");

            // Start FFmpeg process
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = ffmpegArgs,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            _ffmpegProcess = Process.Start(processStartInfo);
            
            if (_ffmpegProcess == null)
            {
                throw new InvalidOperationException("Failed to start FFmpeg process");
            }

            // Set up output monitoring
            _ffmpegProcess.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    _logger.Log($"FFmpeg output: {e.Data}");
                }
            };

            _ffmpegProcess.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    _logger.Log($"FFmpeg: {e.Data}");
                }
            };

            _ffmpegProcess.BeginOutputReadLine();
            _ffmpegProcess.BeginErrorReadLine();

            // Give FFmpeg a moment to start
            await Task.Delay(2000);

            // Check if process is still running
            if (_ffmpegProcess.HasExited)
            {
                var exitCode = _ffmpegProcess.ExitCode;
                throw new InvalidOperationException($"FFmpeg exited immediately with code {exitCode}");
            }

            _isStreaming = true;
            _logger.Log("Streaming started successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to start streaming: {ex.Message}", ex);
            await CleanupProcess();
            throw;
        }
    }

    /// <summary>
    /// Stops the current streaming session
    /// </summary>
    public async Task StopStreamingAsync()
    {
        if (!_isStreaming)
        {
            return;
        }

        try
        {
            _logger.Log("Stopping streaming...");
            await CleanupProcess();
            _isStreaming = false;
            _logger.Log("Streaming stopped successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error stopping streaming: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Checks if streaming is currently active
    /// </summary>
    public bool IsStreaming => _isStreaming;

    /// <summary>
    /// Builds the FFmpeg command line arguments for screen capture and SRT streaming
    /// </summary>
    private string BuildFFmpegCommand(StreamingConfig config)
    {
        var monitor = config.Monitor;
        
        // Build input parameters for screen capture
        var inputParams = $"-f gdigrab -framerate {config.Fps} -offset_x {monitor.Left} -offset_y {monitor.Top} -video_size {monitor.Width}x{monitor.Height} -i desktop";
        
        // Build encoding parameters
        var videoCodec = "-c:v libx264";
        var preset = "-preset ultrafast";
        var tune = "-tune zerolatency";
        var pixelFormat = "-pix_fmt yuv420p";
        var bitrate = $"-b:v {config.Bitrate}k";
        var bufsize = $"-bufsize {config.Bitrate * 2}k";
        var maxrate = $"-maxrate {config.Bitrate}k";
        
        // SRT-specific parameters
        var format = "-f mpegts";
        var srtParams = "?pkt_size=1316&mode=caller";
        var output = $"\"{config.SrtUrl}{srtParams}\"";
        
        // Additional parameters for low latency
        var additionalParams = "-g 30 -keyint_min 30 -sc_threshold 0 -fflags +genpts";
        
        return $"{inputParams} {videoCodec} {preset} {tune} {pixelFormat} {bitrate} {bufsize} {maxrate} {additionalParams} {format} {output}";
    }

    /// <summary>
    /// Checks if FFmpeg is available on the system
    /// </summary>
    private async Task<bool> IsFFmpegAvailableAsync()
    {
        try
        {
            using var process = new Process();
            process.StartInfo.FileName = "ffmpeg";
            process.StartInfo.Arguments = "-version";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;

            process.Start();
            await process.WaitForExitAsync();
            
            return process.ExitCode == 0;
        }
        catch (Exception ex)
        {
            _logger.LogError($"FFmpeg availability check failed: {ex.Message}", ex);
            return false;
        }
    }

    /// <summary>
    /// Cleans up the FFmpeg process
    /// </summary>
    private async Task CleanupProcess()
    {
        if (_ffmpegProcess != null)
        {
            try
            {
                if (!_ffmpegProcess.HasExited)
                {
                    _ffmpegProcess.Kill();
                    await _ffmpegProcess.WaitForExitAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error terminating FFmpeg process: {ex.Message}");
            }
            finally
            {
                _ffmpegProcess.Dispose();
                _ffmpegProcess = null;
            }
        }
    }
}
