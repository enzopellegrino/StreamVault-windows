namespace StreamVault.Models;

/// <summary>
/// Represents a virtual monitor/display device
/// </summary>
public class VirtualMonitorInfo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public int Width { get; set; }
    public int Height { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public string DriverType { get; set; } = "Unknown";

    /// <summary>
    /// Converts virtual monitor to regular MonitorInfo for streaming
    /// </summary>
    public MonitorInfo ToMonitorInfo()
    {
        return new MonitorInfo
        {
            DeviceName = $"VIRTUAL_{Id:N}",
            FriendlyName = $"{Name} (Virtual {Width}x{Height})",
            Width = Width,
            Height = Height,
            Left = 0, // Virtual monitors typically start at origin
            Top = 0,
            IsPrimary = false // Virtual monitors are never primary
        };
    }

    public override string ToString()
    {
        var status = IsActive ? "Active" : "Inactive";
        return $"{Name} - {Width}x{Height} (Virtual, {status})";
    }

    /// <summary>
    /// Gets a description with creation details
    /// </summary>
    public string GetDetailedDescription()
    {
        return $"{Name} | {Width}x{Height} | Created: {CreatedAt:yyyy-MM-dd HH:mm} | Driver: {DriverType}";
    }
}

/// <summary>
/// Configuration for creating virtual monitors
/// </summary>
public class VirtualMonitorConfig
{
    public string Name { get; set; } = "Virtual Monitor";
    public int Width { get; set; } = 1920;
    public int Height { get; set; } = 1080;
    public int RefreshRate { get; set; } = 60;
    public string PreferredDriver { get; set; } = "Auto";

    /// <summary>
    /// Predefined resolution presets
    /// </summary>
    public static readonly List<(string Name, int Width, int Height)> Presets = new()
    {
        ("HD Ready", 1366, 768),
        ("Full HD", 1920, 1080),
        ("QHD", 2560, 1440),
        ("4K UHD", 3840, 2160),
        ("Custom", 0, 0)
    };

    /// <summary>
    /// Validates the configuration
    /// </summary>
    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(Name) &&
               Width > 0 && Width <= 7680 &&  // Max 8K width
               Height > 0 && Height <= 4320 && // Max 8K height
               RefreshRate > 0 && RefreshRate <= 240;
    }

    public override string ToString()
    {
        return $"{Name} - {Width}x{Height}@{RefreshRate}Hz";
    }
}
