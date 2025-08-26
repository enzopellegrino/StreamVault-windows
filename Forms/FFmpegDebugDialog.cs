using StreamVault.Services;
using System.Diagnostics;

namespace StreamVault.Forms;

public partial class FFmpegDebugDialog : Form
{
    private readonly FFmpegService _ffmpegService;
    private readonly LoggingService _logger;

    public FFmpegDebugDialog()
    {
        InitializeComponent();
        _logger = new LoggingService();
        _ffmpegService = new FFmpegService(_logger);
        
        // Subscribe to FFmpeg events for real-time output
        _ffmpegService.DataReceived += OnFFmpegDataReceived;
        
        LoadFFmpegInfo();
    }

    private async void LoadFFmpegInfo()
    {
        try
        {
            // Check FFmpeg availability
            if (_ffmpegService.IsFFmpegAvailable)
            {
                labelFFmpegStatus.Text = "Status: ✅ Available";
                labelFFmpegStatus.ForeColor = Color.Green;
                
                var version = await _ffmpegService.GetVersionAsync();
                labelFFmpegVersion.Text = $"Version: {version}";
                
                // Try to get FFmpeg path
                var ffmpegPath = GetFFmpegPath();
                labelFFmpegPath.Text = $"Path: {ffmpegPath}";
            }
            else
            {
                labelFFmpegStatus.Text = "Status: ❌ Not Available";
                labelFFmpegStatus.ForeColor = Color.Red;
                labelFFmpegVersion.Text = "Version: Not found";
                labelFFmpegPath.Text = "Path: Not found";
            }
        }
        catch (Exception ex)
        {
            AppendOutput($"Error loading FFmpeg info: {ex.Message}");
        }
    }

    private string GetFFmpegPath()
    {
        try
        {
            // Try multiple methods to find FFmpeg
            AppendOutput("=== Searching for FFmpeg ===");
            
            // Method 1: Try 'where' command
            AppendOutput("Method 1: Using 'where' command...");
            var process = new Process();
            process.StartInfo.FileName = "where";
            process.StartInfo.Arguments = "ffmpeg";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            
            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();
            process.WaitForExit();
            
            AppendOutput($"'where' exit code: {process.ExitCode}");
            if (!string.IsNullOrWhiteSpace(output))
            {
                AppendOutput($"'where' output: {output.Trim()}");
                return output.Trim().Split('\n')[0].Trim();
            }
            if (!string.IsNullOrWhiteSpace(error))
            {
                AppendOutput($"'where' error: {error.Trim()}");
            }
            
            // Method 2: Check common paths
            AppendOutput("Method 2: Checking common paths...");
            var commonPaths = new[]
            {
                @"C:\ffmpeg\bin\ffmpeg.exe",
                @"C:\Program Files\ffmpeg\bin\ffmpeg.exe",
                @"C:\Program Files (x86)\ffmpeg\bin\ffmpeg.exe",
                @"ffmpeg.exe" // Current directory
            };
            
            foreach (var path in commonPaths)
            {
                AppendOutput($"Checking: {path}");
                if (File.Exists(path))
                {
                    AppendOutput($"✅ Found at: {path}");
                    return path;
                }
            }
            
            // Method 3: Check PATH environment variable
            AppendOutput("Method 3: Checking PATH environment...");
            var pathEnv = Environment.GetEnvironmentVariable("PATH");
            if (pathEnv != null)
            {
                var paths = pathEnv.Split(';');
                foreach (var path in paths)
                {
                    if (!string.IsNullOrWhiteSpace(path))
                    {
                        var ffmpegPath = Path.Combine(path.Trim(), "ffmpeg.exe");
                        AppendOutput($"Checking PATH: {ffmpegPath}");
                        if (File.Exists(ffmpegPath))
                        {
                            AppendOutput($"✅ Found in PATH: {ffmpegPath}");
                            return ffmpegPath;
                        }
                    }
                }
            }
            
            AppendOutput("❌ FFmpeg not found in any location");
            return "Not found";
        }
        catch (Exception ex)
        {
            AppendOutput($"❌ Error searching for FFmpeg: {ex.Message}");
            return "Error during search";
        }
    }

    private async void ButtonTestCapture_Click(object? sender, EventArgs e)
    {
        AppendOutput("=== Testing Screen Capture ===");
        
        try
        {
            // Get primary monitor info
            var primaryScreen = Screen.PrimaryScreen;
            if (primaryScreen != null)
            {
                AppendOutput($"Primary screen: {primaryScreen.Bounds.Width}x{primaryScreen.Bounds.Height}");
                
                // Test simple capture command
                var testCommand = $"-f gdigrab -i desktop -t 5 -y test_capture.mp4";
                AppendOutput($"Test command: ffmpeg {testCommand}");
                
                var result = await RunFFmpegCommand(testCommand);
                AppendOutput($"Test result: {(result ? "✅ Success" : "❌ Failed")}");
            }
        }
        catch (Exception ex)
        {
            AppendOutput($"❌ Error: {ex.Message}");
        }
    }

    private async void ButtonTestSRT_Click(object? sender, EventArgs e)
    {
        AppendOutput("=== Testing SRT Stream ===");
        
        try
        {
            // Test SRT listener
            var testSrtCommand = $"-f gdigrab -i desktop -c:v libx264 -preset ultrafast -tune zerolatency -f mpegts srt://127.0.0.1:9999?mode=listener&latency=20";
            AppendOutput($"SRT test command: ffmpeg {testSrtCommand}");
            AppendOutput("Note: This will start a 10-second test stream to SRT://127.0.0.1:9999");
            
            var result = await RunFFmpegCommand(testSrtCommand, timeoutSeconds: 10);
            AppendOutput($"SRT test result: {(result ? "✅ Success" : "❌ Failed")}");
        }
        catch (Exception ex)
        {
            AppendOutput($"❌ Error: {ex.Message}");
        }
    }

    private async void ButtonShowDevices_Click(object? sender, EventArgs e)
    {
        AppendOutput("=== Available Devices ===");
        
        try
        {
            // List DirectShow devices
            AppendOutput("DirectShow video devices:");
            var videoDevicesCommand = "-list_devices true -f dshow -i dummy";
            await RunFFmpegCommand(videoDevicesCommand, timeoutSeconds: 5);
            
            AppendOutput("\nAvailable screens:");
            foreach (var screen in Screen.AllScreens)
            {
                AppendOutput($"- {screen.DeviceName}: {screen.Bounds.Width}x{screen.Bounds.Height} at {screen.Bounds.X},{screen.Bounds.Y}");
                AppendOutput($"  Primary: {screen.Primary}, Working Area: {screen.WorkingArea}");
            }
        }
        catch (Exception ex)
        {
            AppendOutput($"❌ Error: {ex.Message}");
        }
    }

    private async Task<bool> RunFFmpegCommand(string arguments, int timeoutSeconds = 30)
    {
        try
        {
            var process = new Process();
            process.StartInfo.FileName = "ffmpeg";
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            
            process.OutputDataReceived += (s, e) => {
                if (!string.IsNullOrEmpty(e.Data))
                    AppendOutput($"OUT: {e.Data}");
            };
            
            process.ErrorDataReceived += (s, e) => {
                if (!string.IsNullOrEmpty(e.Data))
                    AppendOutput($"ERR: {e.Data}");
            };
            
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            
            var completed = process.WaitForExit(timeoutSeconds * 1000);
            
            if (!completed)
            {
                AppendOutput($"⚠️ Command timed out after {timeoutSeconds} seconds, killing process...");
                process.Kill();
                return false;
            }
            
            AppendOutput($"Process exited with code: {process.ExitCode}");
            return process.ExitCode == 0;
        }
        catch (Exception ex)
        {
            AppendOutput($"❌ Process error: {ex.Message}");
            return false;
        }
    }

    private void OnFFmpegDataReceived(object? sender, string data)
    {
        AppendOutput($"FFmpeg: {data}");
    }

    private void AppendOutput(string message)
    {
        if (InvokeRequired)
        {
            Invoke(new Action<string>(AppendOutput), message);
            return;
        }
        
        textBoxOutput.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\r\n");
        textBoxOutput.ScrollToCaret();
    }

    private void ButtonClearOutput_Click(object? sender, EventArgs e)
    {
        textBoxOutput.Clear();
    }

    private void ButtonClose_Click(object? sender, EventArgs e)
    {
        Close();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
        }
        base.Dispose(disposing);
    }
}
