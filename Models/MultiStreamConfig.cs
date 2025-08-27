namespace StreamVault.Models;

/// <summary>
/// Configuration for multi-monitor streaming setup
/// </summary>
public class MultiStreamConfig
{
    public List<StreamSession> StreamSessions { get; set; } = new();
    public List<VirtualMonitorInfo> VirtualMonitors { get; set; } = new();
    public List<SrtServerInfo> SrtServers { get; set; } = new();
    public string SelectedSrtServerId { get; set; } = string.Empty;
    public bool AutoStartChrome { get; set; } = true;
    public string ChromePath { get; set; } = string.Empty;
    public string DefaultChromeUrl { get; set; } = "https://www.google.com";
    public int BaseSrtPort { get; set; } = 9999;
    public string SrtHost { get; set; } = "127.0.0.1";
}

/// <summary>
/// Individual streaming session for a specific monitor
/// </summary>
public class StreamSession
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public MonitorInfo? Monitor { get; set; } = null;
    public VirtualMonitorInfo? VirtualMonitor { get; set; } = null;
    public string MonitorId { get; set; } = string.Empty;
    public string MonitorName { get; set; } = string.Empty;
    public string SrtUrl { get; set; } = string.Empty;
    public int Fps { get; set; } = 30;
    public int Bitrate { get; set; } = 2000;
    public bool IsActive { get; set; } = false;
    public bool IsVirtual { get; set; } = false;
    public bool ChromeLaunched { get; set; } = false;
    public int ChromeProcessId { get; set; } = 0;
    public DateTime StartTime { get; set; }
    public string Status { get; set; } = "Ready";
    
    /// <summary>
    /// Gets the effective monitor info (physical or virtual)
    /// </summary>
    public MonitorInfo? GetEffectiveMonitor()
    {
        return IsVirtual ? VirtualMonitor?.ToMonitorInfo() : Monitor;
    }
    
    public override string ToString()
    {
        var displayName = IsVirtual ? VirtualMonitor?.Name ?? MonitorName : Monitor?.DeviceName ?? MonitorName;
        return $"{displayName} → {SrtUrl} ({Status})";
    }
}
