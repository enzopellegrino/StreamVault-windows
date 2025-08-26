using StreamVault.Models;
using StreamVault.Services;
using System.Diagnostics;
using System.Windows.Forms;

namespace StreamVault.Forms;

public partial class MainForm : Form
{
    private readonly ScreenCaptureService _screenCaptureService;
    private readonly StreamingService _streamingService;
    private readonly VirtualDisplayService _virtualDisplayService;
    private readonly FFmpegService _ffmpegService;
    private readonly LoggingService _logger;
    private readonly ConfigurationService _configService;
    private bool _isStreaming = false;

    public MainForm()
    {
        InitializeComponent();
        _logger = new LoggingService();
        _configService = new ConfigurationService(_logger);
        _screenCaptureService = new ScreenCaptureService(_logger);
        _streamingService = new StreamingService(_logger);
        _virtualDisplayService = new VirtualDisplayService(_logger);
        _ffmpegService = new FFmpegService(_logger);
        
        // Subscribe to FFmpeg events
        _ffmpegService.ProcessStarted += OnFFmpegStarted;
        _ffmpegService.ProcessStopped += OnFFmpegStopped;
        _ffmpegService.DataReceived += OnFFmpegDataReceived;
        
        InitializeUI();
        LoadAvailableMonitors();
    }

    private async void InitializeUI()
    {
        // Set default values
        numericFps.Value = 30;
        numericBitrate.Value = 2000;
        textBoxSrtUrl.Text = "srt://127.0.0.1:9999";
        
        // Set initial button state
        UpdateButtonStates();
        
        _logger.Log("UI initialized with default values");
        
        // Check FFmpeg availability
        await CheckFFmpegAvailability();
    }

    private async Task CheckFFmpegAvailability()
    {
        try
        {
            var version = await _ffmpegService.GetVersionAsync();
            if (version.Contains("ffmpeg") || version.Contains("FFmpeg"))
            {
                toolStripStatusLabel.Text = $"✅ FFmpeg Ready - {version.Split('\n')[0]}";
                _logger.Log($"FFmpeg detected: {version.Split('\n')[0]}");
            }
            else
            {
                toolStripStatusLabel.Text = "❌ FFmpeg not found - Please install FFmpeg";
                toolStripStatusLabel.ForeColor = Color.Red;
                _logger.LogWarning("FFmpeg not found in system");
                
                // Show warning message with download option
                var result = MessageBox.Show(
                    "🔧 FFmpeg non trovato nel sistema!\n\n" +
                    "StreamVault utilizza FFmpeg per la cattura schermo e streaming SRT.\n\n" +
                    "OPZIONI DI INSTALLAZIONE:\n\n" +
                    "✅ FACILE - Download Automatico:\n" +
                    "   Clicca 'Sì' per aprire la pagina di download FFmpeg\n\n" +
                    "⚙️ MANUALE - Installazione Veloce:\n" +
                    "   1. Copia ffmpeg.exe nella cartella dell'app\n" +
                    "   2. Riavvia StreamVault\n\n" +
                    "🔗 SISTEMA - PATH Environment:\n" +
                    "   1. Scarica FFmpeg da ffmpeg.org\n" +
                    "   2. Aggiungi al PATH di Windows\n" +
                    "   3. Riavvia StreamVault\n\n" +
                    "Vuoi aprire la pagina di download di FFmpeg?",
                    "FFmpeg Richiesto - StreamVault",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);
                
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        System.Diagnostics.Process.Start(new ProcessStartInfo
                        {
                            FileName = "https://www.gyan.dev/ffmpeg/builds/",
                            UseShellExecute = true
                        });
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error opening FFmpeg download page: {ex.Message}", ex);
                        MessageBox.Show("Impossibile aprire il browser. Vai manualmente a: https://www.gyan.dev/ffmpeg/builds/", 
                                       "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            toolStripStatusLabel.Text = "⚠️ Error checking FFmpeg";
            _logger.LogError($"Error checking FFmpeg: {ex.Message}", ex);
        }
    }

    private void LoadAvailableMonitors()
    {
        try
        {
            var monitors = _screenCaptureService.GetAvailableMonitors();
            var virtualMonitors = _virtualDisplayService.GetVirtualMonitors();
            
            comboBoxMonitor.Items.Clear();
            
            // Add physical monitors
            foreach (var monitor in monitors)
            {
                comboBoxMonitor.Items.Add(monitor);
            }
            
            // Add virtual monitors
            foreach (var virtualMonitor in virtualMonitors)
            {
                comboBoxMonitor.Items.Add(virtualMonitor.ToMonitorInfo());
            }
            
            if (comboBoxMonitor.Items.Count > 0)
            {
                comboBoxMonitor.SelectedIndex = 0;
            }
            
            var totalCount = monitors.Count + virtualMonitors.Count;
            _logger.Log($"Loaded {monitors.Count} physical and {virtualMonitors.Count} virtual monitors (total: {totalCount})");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to load monitors: {ex.Message}", ex);
            MessageBox.Show($"Error loading monitors: {ex.Message}", 
                           "Error", 
                           MessageBoxButtons.OK, 
                           MessageBoxIcon.Warning);
        }
    }

    private async void ButtonStartStop_Click(object sender, EventArgs e)
    {
        if (!_isStreaming)
        {
            await StartStreaming();
        }
        else
        {
            await StopStreaming();
        }
    }

    private async Task StartStreaming()
    {
        try
        {
            // Check FFmpeg availability first
            var ffmpegVersion = await _ffmpegService.GetVersionAsync();
            if (!ffmpegVersion.Contains("ffmpeg") && !ffmpegVersion.Contains("FFmpeg"))
            {
                MessageBox.Show(
                    "❌ FFmpeg non è disponibile!\n\n" +
                    "StreamVault richiede FFmpeg per funzionare.\n\n" +
                    "Installa FFmpeg:\n" +
                    "• Scarica da https://ffmpeg.org/download.html\n" +
                    "• Aggiungi al PATH di sistema\n" +
                    "• Oppure copia ffmpeg.exe nella directory dell'app\n\n" +
                    "Riavvia l'applicazione dopo l'installazione.",
                    "FFmpeg Richiesto - StreamVault",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            if (comboBoxMonitor.SelectedItem == null)
            {
                MessageBox.Show("Please select a monitor to capture.", 
                               "No Monitor Selected", 
                               MessageBoxButtons.OK, 
                               MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxSrtUrl.Text))
            {
                MessageBox.Show("Please enter an SRT URL.", 
                               "No SRT URL", 
                               MessageBoxButtons.OK, 
                               MessageBoxIcon.Warning);
                return;
            }

            var selectedMonitor = (MonitorInfo)comboBoxMonitor.SelectedItem;
            var srtUrl = textBoxSrtUrl.Text.Trim();
            var fps = (int)numericFps.Value;
            var bitrate = (int)numericBitrate.Value;

            _logger.Log($"Starting FFmpeg streaming: Monitor={selectedMonitor.DeviceName}, " +
                       $"FPS={fps}, Bitrate={bitrate}, SRT={srtUrl}");

            labelStatus.Text = "Starting streaming...";
            buttonStartStop.Enabled = false;

            // Use FFmpeg for streaming
            var success = await _ffmpegService.StartScreenCaptureAsync(
                selectedMonitor.DeviceName, 
                srtUrl, 
                fps, 
                bitrate);

            if (success)
            {
                _isStreaming = true;
                labelStatus.Text = "Streaming active (FFmpeg)";
                labelStatus.ForeColor = Color.Green;
                _logger.Log("FFmpeg streaming started successfully");
            }
            else
            {
                labelStatus.Text = "Failed to start FFmpeg";
                labelStatus.ForeColor = Color.Red;
                MessageBox.Show("Failed to start FFmpeg streaming. Please check if FFmpeg is installed.", 
                               "FFmpeg Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            UpdateButtonStates();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to start streaming: {ex.Message}", ex);
            labelStatus.Text = "Failed to start streaming";
            labelStatus.ForeColor = Color.Red;
            
            MessageBox.Show($"Failed to start streaming: {ex.Message}", 
                           "Streaming Error", 
                           MessageBoxButtons.OK, 
                           MessageBoxIcon.Error);
        }
        finally
        {
            buttonStartStop.Enabled = true;
        }
    }

    private async Task StopStreaming()
    {
        try
        {
            _logger.Log("Stopping FFmpeg streaming...");
            labelStatus.Text = "Stopping streaming...";
            buttonStartStop.Enabled = false;

            var success = await _ffmpegService.StopAsync();
            
            _isStreaming = false;
            
            if (success)
            {
                labelStatus.Text = "Streaming stopped";
                labelStatus.ForeColor = Color.Black;
                _logger.Log("FFmpeg streaming stopped successfully");
            }
            else
            {
                labelStatus.Text = "Error stopping stream";
                labelStatus.ForeColor = Color.Orange;
            }
            
            UpdateButtonStates();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to stop streaming: {ex.Message}", ex);
            MessageBox.Show($"Failed to stop streaming: {ex.Message}", 
                           "Streaming Error", 
                           MessageBoxButtons.OK, 
                           MessageBoxIcon.Error);
        }
        finally
        {
            buttonStartStop.Enabled = true;
        }
    }

    private void UpdateButtonStates()
    {
        buttonStartStop.Text = _isStreaming ? "🛑 Stop FFmpeg Stream" : "▶️ Start FFmpeg Stream";
        buttonStartStop.BackColor = _isStreaming ? Color.IndianRed : Color.LightGreen;
        
        // Disable controls during streaming
        comboBoxMonitor.Enabled = !_isStreaming;
        numericFps.Enabled = !_isStreaming;
        numericBitrate.Enabled = !_isStreaming;
        textBoxSrtUrl.Enabled = !_isStreaming;
    }

    private void ButtonRefreshMonitors_Click(object sender, EventArgs e)
    {
        LoadAvailableMonitors();
    }

    private async void ButtonCreateVirtualMonitor_Click(object sender, EventArgs e)
    {
        try
        {
            // Temporarily disable virtual monitor creation via MainForm
            MessageBox.Show("Virtual monitor creation is now available through the Multi-Stream form.", 
                           "Virtual Monitor", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating virtual monitor: {ex.Message}", ex);
            MessageBox.Show($"Failed to create virtual monitor: {ex.Message}", 
                           "Virtual Monitor Error", 
                           MessageBoxButtons.OK, 
                           MessageBoxIcon.Error);
        }
    }

    private async void ButtonRemoveVirtualMonitor_Click(object sender, EventArgs e)
    {
        try
        {
            // Temporarily disable virtual monitor removal via MainForm
            MessageBox.Show("Virtual monitor management is now available through the Multi-Stream form.", 
                           "Virtual Monitor", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error removing virtual monitor: {ex.Message}", ex);
            MessageBox.Show($"Failed to remove virtual monitor: {ex.Message}", 
                           "Virtual Monitor Error", 
                           MessageBoxButtons.OK, 
                           MessageBoxIcon.Error);
        }
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (_isStreaming)
        {
            var result = MessageBox.Show("Streaming is currently active. Do you want to stop it and exit?", 
                                       "Confirm Exit", 
                                       MessageBoxButtons.YesNo, 
                                       MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                Task.Run(async () => await StopStreaming()).Wait();
            }
            else
            {
                e.Cancel = true;
                return;
            }
        }
        
        // Cleanup virtual displays
        try
        {
            Task.Run(async () => await _virtualDisplayService.CleanupAsync()).Wait();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error during virtual display cleanup: {ex.Message}", ex);
        }
        
        // Cleanup FFmpeg
        try
        {
            Task.Run(async () => await _ffmpegService.DisposeAsync()).Wait();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error during FFmpeg cleanup: {ex.Message}", ex);
        }
        
        base.OnFormClosing(e);
    }

    #region FFmpeg Event Handlers

    private void OnFFmpegStarted(object? sender, EventArgs e)
    {
        if (InvokeRequired)
        {
            Invoke(new Action(() => OnFFmpegStarted(sender, e)));
            return;
        }
        
        labelStatus.Text = "FFmpeg started";
        labelStatus.ForeColor = Color.Green;
        _logger.Log("FFmpeg process started");
    }

    private void OnFFmpegStopped(object? sender, EventArgs e)
    {
        if (InvokeRequired)
        {
            Invoke(new Action(() => OnFFmpegStopped(sender, e)));
            return;
        }
        
        _isStreaming = false;
        labelStatus.Text = "FFmpeg stopped";
        labelStatus.ForeColor = Color.Black;
        UpdateButtonStates();
        _logger.Log("FFmpeg process stopped");
    }

    private void OnFFmpegDataReceived(object? sender, string data)
    {
        // Log FFmpeg output (can be used for progress monitoring)
        if (data.Contains("frame=") || data.Contains("fps="))
        {
            // This is progress information from FFmpeg
            if (InvokeRequired)
            {
                Invoke(new Action(() => {
                    toolStripStatusLabel.Text = $"FFmpeg: {data.Substring(0, Math.Min(50, data.Length))}...";
                }));
            }
            else
            {
                toolStripStatusLabel.Text = $"FFmpeg: {data.Substring(0, Math.Min(50, data.Length))}...";
            }
        }
    }

    private void ButtonMultiStream_Click(object sender, EventArgs e)
    {
        try
        {
            _logger.Log("Opening Multi-Monitor Streaming window");
            
            var multiStreamForm = new MultiStreamForm();
            multiStreamForm.Show();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error opening multi-stream window: {ex.Message}", ex);
            MessageBox.Show($"Error opening multi-stream window: {ex.Message}", 
                           "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    #endregion
}
