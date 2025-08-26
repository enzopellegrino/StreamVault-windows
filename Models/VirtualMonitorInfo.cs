using System.Drawing;

namespace StreamVault.Models;

/// <summary>
/// Represents a virtual monitor/display device
/// </summary>
public class VirtualMonitorInfo
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int SourceMonitorIndex { get; set; }
    public Size Resolution { get; set; }
    public Rectangle Bounds { get; set; }
    public bool IsPrimary { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
    public string Description => $"{Name} ({Resolution.Width}x{Resolution.Height})";

    /// <summary>
    /// Converts virtual monitor to regular MonitorInfo for streaming
    /// </summary>
    public MonitorInfo ToMonitorInfo()
    {
        return new MonitorInfo
        {
            DeviceName = $"VIRTUAL_{Id}",
            FriendlyName = $"{Name} (Virtual {Resolution.Width}x{Resolution.Height})",
            Index = SourceMonitorIndex,
            Width = Resolution.Width,
            Height = Resolution.Height,
            Left = Bounds.Left,
            Top = Bounds.Top,
            IsPrimary = IsPrimary
        };
    }

    public override string ToString()
    {
        var status = IsActive ? "Active" : "Inactive";
        return $"{Name} - {Resolution.Width}x{Resolution.Height} (Virtual, {status})";
    }
}
