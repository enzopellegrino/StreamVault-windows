using StreamVault.Models;
using System.Management;

namespace StreamVault.Services;

/// <summary>
/// Service for detecting and managing screen capture capabilities
/// </summary>
public class ScreenCaptureService
{
    private readonly LoggingService _logger;

    public ScreenCaptureService(LoggingService logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Gets a list of all available monitors/displays
    /// </summary>
    /// <returns>List of available monitors</returns>
    public List<MonitorInfo> GetAvailableMonitors()
    {
        var monitors = new List<MonitorInfo>();

        try
        {
            // Get monitors from Windows Forms Screen class
            var screens = Screen.AllScreens;
            
            for (int i = 0; i < screens.Length; i++)
            {
                var screen = screens[i];
                var monitor = new MonitorInfo
                {
                    DeviceName = screen.DeviceName,
                    FriendlyName = GetFriendlyMonitorName(screen.DeviceName, i),
                    Index = i,
                    Width = screen.Bounds.Width,
                    Height = screen.Bounds.Height,
                    Left = screen.Bounds.Left,
                    Top = screen.Bounds.Top,
                    IsPrimary = screen.Primary
                };

                monitors.Add(monitor);
                _logger.Log($"Detected monitor: {monitor}");
            }

            if (monitors.Count == 0)
            {
                _logger.LogWarning("No monitors detected, adding default primary monitor");
                // Fallback: add a default monitor if none detected
                monitors.Add(new MonitorInfo
                {
                    DeviceName = "\\\\.\\DISPLAY1",
                    FriendlyName = "Primary Display",
                    Width = 1920,
                    Height = 1080,
                    Left = 0,
                    Top = 0,
                    IsPrimary = true
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error detecting monitors: {ex.Message}", ex);
            throw new InvalidOperationException("Failed to detect available monitors", ex);
        }

        return monitors.OrderBy(m => m.IsPrimary ? 0 : 1).ToList();
    }

    /// <summary>
    /// Gets a friendly name for a monitor device
    /// </summary>
    private string GetFriendlyMonitorName(string deviceName, int index)
    {
        try
        {
            // Try to get friendly name from WMI
            using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DesktopMonitor");
            var monitors = searcher.Get().Cast<ManagementObject>().ToList();
            
            if (index < monitors.Count)
            {
                var monitor = monitors[index];
                var name = monitor["Name"]?.ToString();
                if (!string.IsNullOrEmpty(name) && name != "Default Monitor")
                {
                    return name;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Could not get friendly name for {deviceName}: {ex.Message}");
        }

        // Fallback to generic name
        return $"Display {index + 1}";
    }

    /// <summary>
    /// Validates if a monitor is available for capture
    /// </summary>
    public bool IsMonitorAvailable(MonitorInfo monitor)
    {
        try
        {
            var availableMonitors = GetAvailableMonitors();
            return availableMonitors.Any(m => m.DeviceName == monitor.DeviceName);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error validating monitor availability: {ex.Message}", ex);
            return false;
        }
    }
}
