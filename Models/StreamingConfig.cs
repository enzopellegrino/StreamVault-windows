namespace StreamVault.Models;

/// <summary>
/// Configuration settings for streaming
/// </summary>
public class StreamingConfig
{
    public MonitorInfo Monitor { get; set; } = new();
    public string SrtUrl { get; set; } = string.Empty;
    public int Fps { get; set; } = 30;
    public int Bitrate { get; set; } = 2000;

    /// <summary>
    /// Validates the streaming configuration
    /// </summary>
    /// <returns>True if configuration is valid, false otherwise</returns>
    public bool IsValid()
    {
        return Monitor != null &&
               !string.IsNullOrWhiteSpace(SrtUrl) &&
               Fps > 0 && Fps <= 60 &&
               Bitrate > 0;
    }

    /// <summary>
    /// Gets a string representation of the configuration for logging
    /// </summary>
    public override string ToString()
    {
        return $"Monitor: {Monitor?.FriendlyName}, SRT: {SrtUrl}, FPS: {Fps}, Bitrate: {Bitrate}kbps";
    }
}
