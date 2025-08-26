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
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var appFolder = Path.Combine(appDataPath, "StreamVault");
        Directory.CreateDirectory(appFolder);
        _configFilePath = Path.Combine(appFolder, "config.json");
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
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var jsonContent = JsonSerializer.Serialize(config, options);
            await File.WriteAllTextAsync(_configFilePath, jsonContent);
            
            _logger.Log($"Configuration saved to {_configFilePath}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error saving configuration: {ex.Message}", ex);
        }
    }

    private MultiStreamConfig CreateDefaultConfiguration()
    {
        return new MultiStreamConfig
        {
            SrtHost = "127.0.0.1",
            BaseSrtPort = 9999,
            DefaultChromeUrl = "https://www.google.com",
            AutoStartChrome = true,
            StreamSessions = new List<StreamSession>()
        };
    }

    public string GetConfigurationPath() => _configFilePath;
}
