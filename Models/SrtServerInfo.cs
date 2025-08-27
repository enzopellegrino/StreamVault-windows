using System.ComponentModel;

namespace StreamVault.Models;

/// <summary>
/// Represents an SRT server configuration
/// </summary>
public class SrtServerInfo
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string Host { get; set; } = "127.0.0.1";
    public int Port { get; set; } = 9999;
    public string StreamKey { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime LastUsed { get; set; } = DateTime.Now;

    /// <summary>
    /// Gets the complete SRT URL
    /// </summary>
    public string SrtUrl 
    { 
        get 
        {
            var url = $"srt://{Host}:{Port}";
            if (!string.IsNullOrEmpty(StreamKey))
            {
                url += $"/{StreamKey}";
            }
            return url;
        }
    }

    /// <summary>
    /// Gets display text for UI
    /// </summary>
    public string DisplayText => $"{Name} ({Host}:{Port})";

    /// <summary>
    /// Creates a default local SRT server
    /// </summary>
    public static SrtServerInfo CreateDefault()
    {
        return new SrtServerInfo
        {
            Name = "Local SRT Server",
            Host = "127.0.0.1",
            Port = 9999,
            Description = "Default local SRT server"
        };
    }

    /// <summary>
    /// Creates an OBS SRT server preset
    /// </summary>
    public static SrtServerInfo CreateOBSPreset()
    {
        return new SrtServerInfo
        {
            Name = "OBS Studio",
            Host = "127.0.0.1",
            Port = 9999,
            Description = "OBS Studio SRT input"
        };
    }

    /// <summary>
    /// Creates a VLC SRT server preset
    /// </summary>
    public static SrtServerInfo CreateVLCPreset()
    {
        return new SrtServerInfo
        {
            Name = "VLC Media Player",
            Host = "127.0.0.1",
            Port = 9998,
            Description = "VLC SRT stream receiver"
        };
    }

    public override string ToString()
    {
        return DisplayText;
    }

    /// <summary>
    /// Validates the server configuration
    /// </summary>
    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(Name) && 
               !string.IsNullOrWhiteSpace(Host) && 
               Port > 0 && Port <= 65535;
    }

    /// <summary>
    /// Creates a copy of this server info
    /// </summary>
    public SrtServerInfo Clone()
    {
        return new SrtServerInfo
        {
            Id = Guid.NewGuid().ToString(), // New ID for cloned server
            Name = $"{Name} (Copy)",
            Host = Host,
            Port = Port + 1, // Increment port to avoid conflicts
            StreamKey = StreamKey,
            Description = Description,
            IsActive = IsActive,
            CreatedDate = DateTime.Now
        };
    }
}
