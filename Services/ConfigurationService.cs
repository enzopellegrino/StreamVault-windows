using System.Text.Json;
using StreamVault.Models;

namespace StreamVault.Services;

public class ConfigurationService
{
    private readonly string _configFilePath;
    private readonly LoggingService _logger;

    public ConfigurationService(LoggingService logger)
    {
        _logger = logger;
        
        // Use the application directory instead of AppData
        var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var configDirectory = Path.Combine(appDirectory, "Config");
        Directory.CreateDirectory(configDirectory);
        _configFilePath = Path.Combine(configDirectory, "config.json");
        
        _logger.Log($"Configuration will be stored in: {_configFilePath}");
    }

    public async Task<MultiStreamConfig> LoadConfigurationAsync()
    {
        try
        {
            if (!File.Exists(_configFilePath))
            {
                _logger.Log("Configuration file not found, creating default configuration");
                return CreateDefaultConfiguration();
            }

            var jsonContent = await File.ReadAllTextAsync(_configFilePath);
            var config = JsonSerializer.Deserialize<MultiStreamConfig>(jsonContent);
            
            if (config == null)
            {
                _logger.LogWarning("Failed to deserialize configuration, using defaults");
                return CreateDefaultConfiguration();
            }

            _logger.Log($"Configuration loaded from {_configFilePath}");
            return config;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error loading configuration: {ex.Message}", ex);
            return CreateDefaultConfiguration();
        }
    }

    public async Task SaveConfigurationAsync(MultiStreamConfig config)
    {
        try
        {
            // Ensure directory exists
            var directory = Path.GetDirectoryName(_configFilePath);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                _logger.Log($"Created configuration directory: {directory}");
            }

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var jsonContent = JsonSerializer.Serialize(config, options);
            
            // Log some details about what we're saving
            _logger.Log($"Saving configuration with {config.StreamSessions.Count} stream sessions and {config.VirtualMonitors.Count} virtual monitors");
            _logger.Log($"Configuration file: {_configFilePath}");
            
            await File.WriteAllTextAsync(_configFilePath, jsonContent);
            
            _logger.Log($"Configuration saved successfully to {_configFilePath}");
            
            // Verify the file was written
            if (File.Exists(_configFilePath))
            {
                var fileInfo = new FileInfo(_configFilePath);
                _logger.Log($"Configuration file size: {fileInfo.Length} bytes");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error saving configuration: {ex.Message}", ex);
            throw; // Re-throw so the caller knows it failed
        }
    }

    private MultiStreamConfig CreateDefaultConfiguration()
    {
        var defaultServers = new List<SrtServerInfo>
        {
            SrtServerInfo.CreateDefault(),
            SrtServerInfo.CreateOBSPreset(),
            SrtServerInfo.CreateVLCPreset()
        };

        return new MultiStreamConfig
        {
            SrtHost = "127.0.0.1",
            BaseSrtPort = 9999,
            DefaultChromeUrl = "https://www.google.com",
            AutoStartChrome = true,
            StreamSessions = new List<StreamSession>(),
            SrtServers = defaultServers,
            SelectedSrtServerId = defaultServers.First().Id
        };
    }

    public string GetConfigurationPath() => _configFilePath;
}
