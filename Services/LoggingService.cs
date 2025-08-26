namespace StreamVault.Services;

/// <summary>
/// Simple logging service for application events and errors
/// </summary>
public class LoggingService
{
    private readonly string _logFilePath;
    private readonly object _lockObject = new();

    public LoggingService()
    {
        var logDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StreamVault");
        Directory.CreateDirectory(logDirectory);
        _logFilePath = Path.Combine(logDirectory, $"StreamVault_{DateTime.Now:yyyyMMdd}.log");
    }

    /// <summary>
    /// Logs an informational message
    /// </summary>
    public void Log(string message)
    {
        WriteToLog("INFO", message);
    }

    /// <summary>
    /// Logs an error message with optional exception details
    /// </summary>
    public void LogError(string message, Exception? exception = null)
    {
        var fullMessage = exception != null 
            ? $"{message} | Exception: {exception}" 
            : message;
        
        WriteToLog("ERROR", fullMessage);
    }

    /// <summary>
    /// Logs a warning message
    /// </summary>
    public void LogWarning(string message)
    {
        WriteToLog("WARNING", message);
    }

    private void WriteToLog(string level, string message)
    {
        lock (_lockObject)
        {
            try
            {
                var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";
                File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
                
                // Also write to console for debugging
                Console.WriteLine(logEntry);
            }
            catch (Exception ex)
            {
                // If logging fails, write to console as fallback
                Console.WriteLine($"Logging failed: {ex.Message}");
                Console.WriteLine($"Original message: [{level}] {message}");
            }
        }
    }

    /// <summary>
    /// Gets the path to the current log file
    /// </summary>
    public string GetLogFilePath() => _logFilePath;
}
