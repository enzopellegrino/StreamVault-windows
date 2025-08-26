using System.Diagnostics;
using System.Runtime.InteropServices;
using StreamVault.Models;

namespace StreamVault.Services;

/// <summary>
/// Service for managing Chrome instances across multiple monitors
/// </summary>
public class ChromeManagementService
{
    private readonly LoggingService _logger;
    private readonly List<Process> _chromeProcesses = new();
    private string _chromePath = string.Empty;

    // Windows API for window positioning
    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);
    
    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    private const uint SWP_NOZORDER = 0x0004;
    private const uint SWP_NOACTIVATE = 0x0010;
    private const int SW_MAXIMIZE = 3;

    public ChromeManagementService(LoggingService logger)
    {
        _logger = logger;
        _chromePath = FindChromeExecutable();
    }

    /// <summary>
    /// Launch Chrome on all available monitors
    /// </summary>
    public async Task<bool> LaunchChromeOnAllMonitorsAsync(List<MonitorInfo> monitors, string url = "https://www.google.com")
    {
        if (string.IsNullOrEmpty(_chromePath))
        {
            _logger.LogError("Chrome executable not found");
            return false;
        }

        var success = true;
        
        foreach (var monitor in monitors)
        {
            try
            {
                var launched = await LaunchChromeOnMonitorAsync(monitor, url);
                if (!launched)
                {
                    success = false;
                    _logger.LogError($"Failed to launch Chrome on monitor: {monitor.DeviceName}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error launching Chrome on {monitor.DeviceName}: {ex.Message}", ex);
                success = false;
            }
        }

        return success;
    }

    /// <summary>
    /// Launch Chrome on a specific monitor
    /// </summary>
    public async Task<bool> LaunchChromeOnMonitorAsync(MonitorInfo monitor, string url = "https://www.google.com")
    {
        try
        {
            _logger.Log($"Launching Chrome on monitor: {monitor.DeviceName}");

            var startInfo = new ProcessStartInfo
            {
                FileName = _chromePath,
                Arguments = $"--new-window --start-fullscreen --disable-web-security --disable-features=VizDisplayCompositor --kiosk \"{url}\"",
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Maximized
            };

            var process = Process.Start(startInfo);
            if (process == null)
            {
                return false;
            }

            _chromeProcesses.Add(process);

            // Wait for window to be created
            await Task.Delay(2000);

            // Position Chrome window on the target monitor
            await PositionChromeOnMonitorAsync(process, monitor);

            _logger.Log($"Chrome launched successfully on {monitor.DeviceName}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to launch Chrome: {ex.Message}", ex);
            return false;
        }
    }

    /// <summary>
    /// Position Chrome window on specific monitor
    /// </summary>
    private async Task PositionChromeOnMonitorAsync(Process chromeProcess, MonitorInfo monitor)
    {
        try
        {
            // Wait for Chrome to fully load
            await Task.Delay(3000);

            if (chromeProcess.MainWindowHandle != IntPtr.Zero)
            {
                // Move and resize window to fill the target monitor
                SetWindowPos(
                    chromeProcess.MainWindowHandle,
                    IntPtr.Zero,
                    monitor.Bounds.X,
                    monitor.Bounds.Y,
                    monitor.Bounds.Width,
                    monitor.Bounds.Height,
                    SWP_NOZORDER | SWP_NOACTIVATE
                );

                // Maximize on the monitor
                ShowWindow(chromeProcess.MainWindowHandle, SW_MAXIMIZE);

                _logger.Log($"Chrome positioned on monitor: {monitor.DeviceName} at {monitor.Bounds}");
            }
            else
            {
                _logger.LogWarning($"Could not get Chrome window handle for monitor: {monitor.DeviceName}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error positioning Chrome: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Close all Chrome instances
    /// </summary>
    public async Task CloseAllChromeInstancesAsync()
    {
        _logger.Log("Closing all Chrome instances...");

        foreach (var process in _chromeProcesses.ToList())
        {
            try
            {
                if (!process.HasExited)
                {
                    process.CloseMainWindow();
                    
                    // Wait for graceful close
                    if (!process.WaitForExit(5000))
                    {
                        process.Kill();
                    }
                }
                
                process.Dispose();
                _chromeProcesses.Remove(process);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error closing Chrome process: {ex.Message}", ex);
            }
        }

        _logger.Log("All Chrome instances closed");
    }

    /// <summary>
    /// Get Chrome processes count
    /// </summary>
    public int GetActiveChromeCount()
    {
        return _chromeProcesses.Count(p => !p.HasExited);
    }

    /// <summary>
    /// Find Chrome executable
    /// </summary>
    private string FindChromeExecutable()
    {
        var possiblePaths = new[]
        {
            @"C:\Program Files\Google\Chrome\Application\chrome.exe",
            @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe",
            @"C:\Users\" + Environment.UserName + @"\AppData\Local\Google\Chrome\Application\chrome.exe",
            @"C:\Program Files\BraveSoftware\Brave-Browser\Application\brave.exe",
            @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe"
        };

        foreach (var path in possiblePaths)
        {
            if (File.Exists(path))
            {
                _logger.Log($"Found browser at: {path}");
                return path;
            }
        }

        // Try to find in registry or PATH
        try
        {
            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "where",
                    Arguments = "chrome",
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
                var chromePath = output.Trim().Split('\n').FirstOrDefault();
                if (!string.IsNullOrEmpty(chromePath) && File.Exists(chromePath))
                {
                    _logger.Log($"Found Chrome in PATH: {chromePath}");
                    return chromePath;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error searching Chrome in PATH: {ex.Message}");
        }

        _logger.LogError("Chrome executable not found");
        return string.Empty;
    }

    /// <summary>
    /// Check if Chrome is available
    /// </summary>
    public bool IsChromeAvailable()
    {
        return !string.IsNullOrEmpty(_chromePath) && File.Exists(_chromePath);
    }

    /// <summary>
    /// Get Chrome version
    /// </summary>
    public async Task<string> GetChromeVersionAsync()
    {
        try
        {
            if (string.IsNullOrEmpty(_chromePath))
                return "Chrome not found";

            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _chromePath,
                    Arguments = "--version",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            process.Start();
            var output = await process.StandardOutput.ReadToEndAsync();
            await process.WaitForExitAsync();

            return output.Trim();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting Chrome version: {ex.Message}", ex);
            return "Version unknown";
        }
    }

    /// <summary>
    /// Dispose resources
    /// </summary>
    public async Task DisposeAsync()
    {
        await CloseAllChromeInstancesAsync();
    }
}
