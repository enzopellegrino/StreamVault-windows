using StreamVault.Forms;
using StreamVault.Services;

namespace StreamVault;

static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // Configure the application for high DPI support on Windows 10.
        ApplicationConfiguration.Initialize();
        
        // Initialize the logging service
        var logger = new LoggingService();
        logger.Log("Application starting...");
        
        try
        {
            // Start directly with the Multi-Monitor Streaming interface
            Application.Run(new MultiStreamForm());
        }
        catch (Exception ex)
        {
            logger.LogError($"Unhandled exception: {ex.Message}", ex);
            MessageBox.Show($"An unexpected error occurred: {ex.Message}", 
                           "StreamVault Error", 
                           MessageBoxButtons.OK, 
                           MessageBoxIcon.Error);
        }
        finally
        {
            logger.Log("Application shutting down...");
        }
    }
}
