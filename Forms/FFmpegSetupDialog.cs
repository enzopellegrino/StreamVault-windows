using StreamVault.Services;

namespace StreamVault.Forms;

public partial class FFmpegSetupDialog : Form
{
    private readonly FFmpegDownloadService _downloadService;
    private readonly LoggingService _logger;
    private bool _isDownloading = false;

    public bool FFmpegInstalled { get; private set; } = false;

    public FFmpegSetupDialog()
    {
        InitializeComponent();
        _logger = new LoggingService();
        _downloadService = new FFmpegDownloadService(_logger);
        
        InitializeUI();
        CheckCurrentStatus();
    }

    private void InitializeUI()
    {
        this.Text = "FFmpeg Setup - StreamVault";
        this.Size = new Size(500, 350);
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        
        UpdateUI();
    }

    private void CheckCurrentStatus()
    {
        var status = _downloadService.GetInstallationStatus();
        
        labelStatus.Text = status.Message;
        labelStatus.ForeColor = status.IsInstalled ? Color.Green : Color.Red;
        
        if (status.IsInstalled)
        {
            labelPath.Text = $"Path: {status.Path}";
            buttonDownload.Text = "FFmpeg Ready";
            buttonDownload.Enabled = false;
            buttonDownload.BackColor = Color.LightGreen;
            FFmpegInstalled = true;
        }
        else
        {
            labelPath.Text = "FFmpeg not found";
            buttonDownload.Text = "Download FFmpeg";
            buttonDownload.Enabled = true;
            buttonDownload.BackColor = Color.LightBlue;
            FFmpegInstalled = false;
        }
        
        UpdateUI();
    }

    private async void ButtonDownload_Click(object sender, EventArgs e)
    {
        if (_isDownloading)
            return;

        try
        {
            _isDownloading = true;
            buttonDownload.Enabled = false;
            buttonManual.Enabled = false;
            buttonCancel.Text = "Please Wait...";
            progressBar.Visible = true;
            
            var progress = new Progress<string>(message =>
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() => UpdateProgress(message)));
                }
                else
                {
                    UpdateProgress(message);
                }
            });

            var success = await _downloadService.DownloadAndInstallFFmpegAsync(progress);
            
            if (success)
            {
                labelStatus.Text = "FFmpeg installed successfully!";
                labelStatus.ForeColor = Color.Green;
                FFmpegInstalled = true;
                
                buttonDownload.Text = "FFmpeg Ready";
                buttonDownload.BackColor = Color.LightGreen;
                buttonCancel.Text = "Continue";
                
                MessageBox.Show("FFmpeg has been downloaded and installed successfully!\n\nYou can now start streaming.", 
                               "Installation Complete", 
                               MessageBoxButtons.OK, 
                               MessageBoxIcon.Information);
            }
            else
            {
                labelStatus.Text = "FFmpeg installation failed. Try manual installation.";
                labelStatus.ForeColor = Color.Red;
                
                MessageBox.Show("Automatic installation failed. Please try manual installation using the 'Manual Install' button.", 
                               "Installation Failed", 
                               MessageBoxButtons.OK, 
                               MessageBoxIcon.Warning);
            }
        }
        catch (Exception ex)
        {
            labelStatus.Text = $"Error: {ex.Message}";
            labelStatus.ForeColor = Color.Red;
            
            MessageBox.Show($"Installation error: {ex.Message}", 
                           "Error", 
                           MessageBoxButtons.OK, 
                           MessageBoxIcon.Error);
        }
        finally
        {
            _isDownloading = false;
            buttonDownload.Enabled = !FFmpegInstalled;
            buttonManual.Enabled = true;
            buttonCancel.Text = FFmpegInstalled ? "Continue" : "Cancel";
            progressBar.Visible = false;
            
            CheckCurrentStatus();
        }
    }

    private void UpdateProgress(string message)
    {
        labelProgress.Text = message;
        progressBar.Style = ProgressBarStyle.Marquee;
    }

    private void ButtonManual_Click(object sender, EventArgs e)
    {
        var result = MessageBox.Show(
            "Manual Installation Options:\n\n" +
            "1. Download from FFmpeg website (will open browser)\n" +
            "2. Install with Chocolatey: choco install ffmpeg\n" +
            "3. Install with Scoop: scoop install ffmpeg\n" +
            "4. Copy ffmpeg.exe to application directory\n\n" +
            "Do you want to open the FFmpeg download page?",
            "Manual Installation",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Information);

        if (result == DialogResult.Yes)
        {
            _downloadService.OpenFFmpegWebsite();
        }
    }

    private void ButtonRefresh_Click(object sender, EventArgs e)
    {
        CheckCurrentStatus();
    }

    private void ButtonCancel_Click(object sender, EventArgs e)
    {
        if (_isDownloading)
        {
            MessageBox.Show("Please wait for the download to complete.", 
                           "Download in Progress", 
                           MessageBoxButtons.OK, 
                           MessageBoxIcon.Information);
            return;
        }

        this.DialogResult = FFmpegInstalled ? DialogResult.OK : DialogResult.Cancel;
        this.Close();
    }

    private void UpdateUI()
    {
        buttonCancel.DialogResult = DialogResult.Cancel;
        this.AcceptButton = FFmpegInstalled ? buttonCancel : buttonDownload;
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (_isDownloading)
        {
            e.Cancel = true;
            MessageBox.Show("Please wait for the download to complete.", 
                           "Download in Progress", 
                           MessageBoxButtons.OK, 
                           MessageBoxIcon.Information);
            return;
        }

        _downloadService?.Dispose();
        base.OnFormClosing(e);
    }
}
