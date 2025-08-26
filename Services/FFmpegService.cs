using System.Diagnostics;
using System.Text;

namespace StreamVault.Services;

/// <summary>
/// Service for managing FFmpeg operations including screen capture and streaming
/// </summary>
public class FFmpegService
{
    private readonly LoggingService _logger;
    private Process? _ffmpegProcess;
    private readonly string _ffmpegPath;
    private bool _isRunning = false;

    public FFmpegService(LoggingService logger)
    {
        _logger = logger;
        _ffmpegPath = FindFFmpegExecutable();
        
        if (string.IsNullOrEmpty(_ffmpegPath))
        {
            _logger.LogError("FFmpeg not found! Please install FFmpeg or download it.");
        }
        else
        {
            _logger.Log($"FFmpeg found at: {_ffmpegPath}");
        }
    }

    /// <summary>
    /// Check if FFmpeg is available
    /// </summary>
    public bool IsFFmpegAvailable => !string.IsNullOrEmpty(_ffmpegPath) && File.Exists(_ffmpegPath);

    /// <summary>
    /// Event fired when FFmpeg process starts
    /// </summary>
    public event EventHandler? ProcessStarted;

    /// <summary>
    /// Event fired when FFmpeg process stops
    /// </summary>
    public event EventHandler? ProcessStopped;

    /// <summary>
    /// Event fired when FFmpeg outputs data
    /// </summary>
    public event EventHandler<string>? DataReceived;

    /// <summary>
    /// Check if FFmpeg is available and running
    /// </summary>
    public bool IsRunning => _isRunning && _ffmpegProcess != null && !_ffmpegProcess.HasExited;

    /// <summary>
    /// Start screen capture and streaming with FFmpeg
    /// </summary>
    public async Task<bool> StartScreenCaptureAsync(string monitorDevice, string srtUrl, int fps = 30, int bitrate = 2000)
    {
        try
        {
            if (IsRunning)
            {
                _logger.LogWarning("FFmpeg is already running");
                return false;
            }

            if (!IsFFmpegAvailable)
            {
                var message = "FFmpeg executable not found. Please install FFmpeg:\n\n" +
                             "Option 1: Download from https://ffmpeg.org/download.html\n" +
                             "Option 2: Install with Chocolatey: choco install ffmpeg\n" +
                             "Option 3: Install with Scoop: scoop install ffmpeg\n" +
                             "Option 4: Copy ffmpeg.exe to application directory\n\n" +
                             "After installation, restart the application.";
                             
                throw new InvalidOperationException(message);
            }

            var arguments = BuildFFmpegArguments(monitorDevice, srtUrl, fps, bitrate);
            
            _logger.Log($"Starting FFmpeg with arguments: {arguments}");

            _ffmpegProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _ffmpegPath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    StandardErrorEncoding = Encoding.UTF8,
                    StandardOutputEncoding = Encoding.UTF8
                },
                EnableRaisingEvents = true
            };

            _ffmpegProcess.OutputDataReceived += OnOutputDataReceived;
            _ffmpegProcess.ErrorDataReceived += OnErrorDataReceived;
            _ffmpegProcess.Exited += OnProcessExited;

            _ffmpegProcess.Start();
            _ffmpegProcess.BeginOutputReadLine();
            _ffmpegProcess.BeginErrorReadLine();

            _isRunning = true;
            ProcessStarted?.Invoke(this, EventArgs.Empty);
            
            _logger.Log("FFmpeg process started successfully");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to start FFmpeg: {ex.Message}", ex);
            return false;
        }
    }

    /// <summary>
    /// Stop the FFmpeg process
    /// </summary>
    public async Task<bool> StopAsync()
    {
        try
        {
            if (!IsRunning)
            {
                _logger.LogWarning("FFmpeg is not running");
                return true;
            }

            _logger.Log("Stopping FFmpeg process...");

            // Send 'q' command to FFmpeg for graceful shutdown
            if (_ffmpegProcess?.StandardInput != null && !_ffmpegProcess.StandardInput.BaseStream.CanWrite == false)
            {
                try
                {
                    await _ffmpegProcess.StandardInput.WriteLineAsync("q");
                    await _ffmpegProcess.StandardInput.FlushAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"Could not send quit command to FFmpeg: {ex.Message}");
                }
            }

            // Wait for graceful shutdown
            var cancelled = false;
            if (_ffmpegProcess != null)
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                try
                {
                    await _ffmpegProcess.WaitForExitAsync(cts.Token);
                    cancelled = true;
                }
                catch (OperationCanceledException)
                {
                    cancelled = false;
                }
            }

            // Force kill if necessary
            if (!cancelled && _ffmpegProcess != null && !_ffmpegProcess.HasExited)
            {
                _logger.LogWarning("Force killing FFmpeg process");
                _ffmpegProcess.Kill();
                await _ffmpegProcess.WaitForExitAsync();
            }

            _isRunning = false;
            ProcessStopped?.Invoke(this, EventArgs.Empty);
            
            _logger.Log("FFmpeg process stopped");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error stopping FFmpeg: {ex.Message}", ex);
            return false;
        }
    }

    /// <summary>
    /// Get FFmpeg version information
    /// </summary>
    public async Task<string> GetVersionAsync()
    {
        try
        {
            if (string.IsNullOrEmpty(_ffmpegPath))
            {
                return "FFmpeg not found";
            }

            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _ffmpegPath,
                    Arguments = "-version",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            process.Start();
            var output = await process.StandardOutput.ReadToEndAsync();
            await process.WaitForExitAsync();

            return output.Split('\n').FirstOrDefault() ?? "Unknown version";
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting FFmpeg version: {ex.Message}", ex);
            return "Error getting version";
        }
    }

    /// <summary>
    /// List available input devices (monitors, audio devices)
    /// </summary>
    public async Task<List<string>> GetInputDevicesAsync()
    {
        try
        {
            if (string.IsNullOrEmpty(_ffmpegPath))
            {
                return new List<string>();
            }

            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _ffmpegPath,
                    Arguments = "-f gdigrab -list_devices true -i dummy",
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            process.Start();
            var output = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();

            var devices = new List<string>();
            var lines = output.Split('\n');
            
            foreach (var line in lines)
            {
                if (line.Contains("DirectShow video devices:") || line.Contains("gdigrab"))
                {
                    // Parse device names from FFmpeg output
                    if (line.Contains("\""))
                    {
                        var start = line.IndexOf('"') + 1;
                        var end = line.LastIndexOf('"');
                        if (start < end)
                        {
                            devices.Add(line.Substring(start, end - start));
                        }
                    }
                }
            }

            return devices;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting input devices: {ex.Message}", ex);
            return new List<string>();
        }
    }

    /// <summary>
    /// Find FFmpeg executable in various locations
    /// </summary>
    private string FindFFmpegExecutable()
    {
        var possiblePaths = new[]
        {
            "ffmpeg.exe", // In PATH
            "ffmpeg", // Unix-style
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg.exe"), // App directory
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "ffmpeg.exe"), // App bin directory
            @"C:\ffmpeg\bin\ffmpeg.exe", // Common installation path
            @"C:\tools\ffmpeg\bin\ffmpeg.exe", // Chocolatey path
        };

        foreach (var path in possiblePaths)
        {
            try
            {
                if (File.Exists(path))
                {
                    _logger.Log($"Found FFmpeg at: {path}");
                    return path;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error checking path {path}: {ex.Message}");
            }
        }

        // Try using 'where' command on Windows to find FFmpeg in PATH
        try
        {
            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "where",
                    Arguments = "ffmpeg",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode == 0 && !string.IsNullOrWhiteSpace(output))
            {
                var path = output.Trim().Split('\n').FirstOrDefault();
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    _logger.Log($"Found FFmpeg in PATH: {path}");
                    return path;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error using 'where' command: {ex.Message}");
        }

        _logger.LogError("FFmpeg executable not found");
        return string.Empty;
    }

    /// <summary>
    /// Build FFmpeg command line arguments
    /// </summary>
    private string BuildFFmpegArguments(string monitorDevice, string srtUrl, int fps, int bitrate)
    {
        var args = new StringBuilder();

        // Input settings (screen capture)
        args.Append($"-f gdigrab ");
        args.Append($"-framerate {fps} ");
        args.Append($"-i \"{monitorDevice}\" ");

        // Video encoding settings
        args.Append($"-c:v libx264 ");
        args.Append($"-preset ultrafast ");
        args.Append($"-tune zerolatency ");
        args.Append($"-b:v {bitrate}k ");
        args.Append($"-maxrate {bitrate * 2}k ");
        args.Append($"-bufsize {bitrate * 4}k ");
        args.Append($"-pix_fmt yuv420p ");
        args.Append($"-g {fps * 2} "); // GOP size
        args.Append($"-keyint_min {fps} ");
        args.Append($"-sc_threshold 0 ");

        // SRT output settings
        args.Append($"-f mpegts ");
        args.Append($"\"{srtUrl}\" ");

        // Additional flags
        args.Append($"-y "); // Overwrite output

        var command = args.ToString().Trim();
        _logger.Log($"Built FFmpeg command: ffmpeg {command}");
        _logger.Log($"Monitor device: {monitorDevice}");
        _logger.Log($"SRT URL: {srtUrl}");
        _logger.Log($"Settings: FPS={fps}, Bitrate={bitrate}k");
        
        return command;
    }

    private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Data))
        {
            _logger.Log($"FFmpeg: {e.Data}");
            DataReceived?.Invoke(this, e.Data);
        }
    }

    private void OnErrorDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Data))
        {
            // FFmpeg outputs progress info to stderr, so not all stderr is error
            if (e.Data.Contains("error") || e.Data.Contains("Error"))
            {
                _logger.LogError($"FFmpeg Error: {e.Data}");
            }
            else
            {
                _logger.Log($"FFmpeg Info: {e.Data}");
            }
            DataReceived?.Invoke(this, e.Data);
        }
    }

    private void OnProcessExited(object? sender, EventArgs e)
    {
        _isRunning = false;
        ProcessStopped?.Invoke(this, EventArgs.Empty);
        _logger.Log("FFmpeg process exited");
    }

    /// <summary>
    /// Dispose of resources
    /// </summary>
    public async Task DisposeAsync()
    {
        if (IsRunning)
        {
            await StopAsync();
        }

        _ffmpegProcess?.Dispose();
    }
}
