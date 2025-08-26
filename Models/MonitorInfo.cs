using System.Drawing;

namespace StreamVault.Models;

/// <summary>
/// Represents information about a monitor/display device
/// </summary>
public class MonitorInfo
{
    public string DeviceName { get; set; } = string.Empty;
    public string FriendlyName { get; set; } = string.Empty;
    public int Index { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int Left { get; set; }
    public int Top { get; set; }
    public bool IsPrimary { get; set; }

    /// <summary>
    /// Gets the resolution as a Size structure
    /// </summary>
    public Size Resolution => new Size(Width, Height);

    /// <summary>
    /// Gets the bounds rectangle for this monitor
    /// </summary>
    public Rectangle Bounds => new Rectangle(Left, Top, Width, Height);

    public override string ToString()
    {
        var primaryText = IsPrimary ? " (Primary)" : "";
        return $"{FriendlyName} - {Width}x{Height}{primaryText}";
    }

    public string GetDisplayBounds()
    {
        return $"{Width}x{Height}+{Left}+{Top}";
    }
}
