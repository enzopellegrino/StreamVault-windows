using System.Diagnostics;
using System.IO.Compression;
using System.Net.Http;

namespace StreamVault.Services;

/// <summary>
/// Service for downloading and managing FFmpeg installation
/// </summary>
public class FFmpegDownloadService
{
    private readonly LoggingService _logger;
    private readonly HttpClient _httpClient;
    
    // FFmpeg download URL for Windows x64
    private const string FFMPEG_DOWNLOAD_URL = "https://github.com/BtbN/FFmpeg-Builds/releases/download/latest/ffmpeg-master-latest-win64-gpl.zip";
    private const string FFMPEG_DOWNLOAD_FILENAME = "ffmpeg-windows.zip";

    public FFmpegDownloadService(LoggingService logger)
    {
        _logger = logger;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "StreamVault/1.0");
    }

    /// <summary>
    /// Download and install FFmpeg to application directory
    /// </summary>
    public async Task<bool> DownloadAndInstallFFmpegAsync(IProgress<string>? progress = null)
    {
        try
        {
            var appDir = AppDomain.CurrentDomain.BaseDirectory;
            var ffmpegDir = Path.Combine(appDir, "ffmpeg");
            var ffmpegExePath = Path.Combine(ffmpegDir, "ffmpeg.exe");
            
            // Check if already exists
            if (File.Exists(ffmpegExePath))
            {
                _logger.Log("FFmpeg already exists in application directory");
                return true;
            }

            progress?.Report("Starting FFmpeg download...");
            _logger.Log("Starting FFmpeg download...");

            // Create ffmpeg directory
            Directory.CreateDirectory(ffmpegDir);
            
            // Download FFmpeg
            var downloadPath = Path.Combine(appDir, FFMPEG_DOWNLOAD_FILENAME);
            var success = await DownloadFileAsync(FFMPEG_DOWNLOAD_URL, downloadPath, progress);
            
            if (!success)
            {
                return false;
            }

            progress?.Report("Extracting FFmpeg...");
            _logger.Log("Extracting FFmpeg archive...");

            // Extract FFmpeg
            await ExtractFFmpegAsync(downloadPath, ffmpegDir, progress);
            
            // Cleanup download file
            try
            {
                File.Delete(downloadPath);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Could not delete download file: {ex.Message}");
            }

            // Verify installation
            if (File.Exists(ffmpegExePath))
            {
                progress?.Report("FFmpeg installed successfully!");
                _logger.Log($"FFmpeg installed successfully at: {ffmpegExePath}");
                return true;
            }
            else
            {
                progress?.Report("FFmpeg installation failed");
                _logger.LogError("FFmpeg executable not found after extraction");
                return false;
            }
        }
        catch (Exception ex)
        {
            var errorMsg = $"Error downloading/installing FFmpeg: {ex.Message}";
            progress?.Report(errorMsg);
            _logger.LogError(errorMsg, ex);
            return false;
        }
    }

    /// <summary>
    /// Download file with progress reporting
    /// </summary>
    private async Task<bool> DownloadFileAsync(string url, string filePath, IProgress<string>? progress)
    {
        try
        {
            using var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            var totalBytes = response.Content.Headers.ContentLength;
            using var contentStream = await response.Content.ReadAsStreamAsync();
            using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);

            var buffer = new byte[8192];
            long totalBytesRead = 0;
            int bytesRead;

            while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                await fileStream.WriteAsync(buffer, 0, bytesRead);
                totalBytesRead += bytesRead;

                if (totalBytes.HasValue)
                {
                    var percentage = (double)totalBytesRead / totalBytes.Value * 100;
                    progress?.Report($"Downloading FFmpeg: {percentage:F1}% ({totalBytesRead / 1024 / 1024:F1} MB)");
                }
                else
                {
                    progress?.Report($"Downloading FFmpeg: {totalBytesRead / 1024 / 1024:F1} MB");
                }
            }

            _logger.Log($"Downloaded FFmpeg: {totalBytesRead / 1024 / 1024:F1} MB");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error downloading FFmpeg: {ex.Message}", ex);
            return false;
        }
    }

    /// <summary>
    /// Extract FFmpeg from downloaded archive
    /// </summary>
    private async Task ExtractFFmpegAsync(string archivePath, string extractPath, IProgress<string>? progress)
    {
        try
        {
            progress?.Report("Extracting FFmpeg archive...");
            
            using var archive = ZipFile.OpenRead(archivePath);
            
            // Find ffmpeg.exe in the archive
            var ffmpegEntry = archive.Entries.FirstOrDefault(e => e.Name.Equals("ffmpeg.exe", StringComparison.OrdinalIgnoreCase));
            
            if (ffmpegEntry == null)
            {
                throw new InvalidOperationException("ffmpeg.exe not found in downloaded archive");
            }

            var ffmpegPath = Path.Combine(extractPath, "ffmpeg.exe");
            ffmpegEntry.ExtractToFile(ffmpegPath, true);
            
            progress?.Report("FFmpeg extracted successfully");
            _logger.Log($"FFmpeg extracted to: {ffmpegPath}");
            
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error extracting FFmpeg: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Get FFmpeg installation status
    /// </summary>
    public FFmpegInstallationStatus GetInstallationStatus()
    {
        var appDir = AppDomain.CurrentDomain.BaseDirectory;
        var ffmpegPaths = new[]
        {
            Path.Combine(appDir, "ffmpeg", "ffmpeg.exe"),
            Path.Combine(appDir, "ffmpeg.exe"),
            "ffmpeg.exe" // In PATH
        };

        foreach (var path in ffmpegPaths)
        {
            if (File.Exists(path) || IsInPath("ffmpeg"))
            {
                return new FFmpegInstallationStatus
                {
                    IsInstalled = true,
                    Path = path,
                    Message = "FFmpeg is available"
                };
            }
        }

        return new FFmpegInstallationStatus
        {
            IsInstalled = false,
            Path = string.Empty,
            Message = "FFmpeg not found. Click 'Download FFmpeg' to install automatically."
        };
    }

    /// <summary>
    /// Check if command is available in PATH
    /// </summary>
    private bool IsInPath(string command)
    {
        try
        {
            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "where",
                    Arguments = command,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            process.Start();
            process.WaitForExit();
            return process.ExitCode == 0;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Open FFmpeg website for manual download
    /// </summary>
    public void OpenFFmpegWebsite()
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://ffmpeg.org/download.html",
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error opening FFmpeg website: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Dispose resources
    /// </summary>
    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}

/// <summary>
/// FFmpeg installation status
/// </summary>
public class FFmpegInstallationStatus
{
    public bool IsInstalled { get; set; }
    public string Path { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
