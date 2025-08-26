using StreamVault.Models;
using StreamVault.Services;
using StreamVault.Forms;

namespace StreamVault.Forms;

public partial class MultiStreamForm : Form
{
    private readonly MultiStreamService _multiStreamService;
    private readonly LoggingService _logger;
    private readonly ConfigurationService _configService;
    private MultiStreamConfig _config = new();
    private bool _isStreaming = false;
    private bool _isInitializing = true;  // Flag per evitare salvataggi durante l'inizializzazione
    private System.Windows.Forms.Timer _saveTimer;  // Timer per ritardare il salvataggio

    public MultiStreamForm()
    {
        InitializeComponent();
        _logger = new LoggingService();
        _configService = new ConfigurationService(_logger);
        _multiStreamService = new MultiStreamService(_logger);
        
        // Initialize save timer
        _saveTimer = new System.Windows.Forms.Timer();
        _saveTimer.Interval = 1000; // Save after 1 second of inactivity
        _saveTimer.Tick += async (s, e) => {
            _saveTimer.Stop();
            await SaveConfigurationAsync();
        };
        
        // Subscribe to events
        _multiStreamService.StreamStarted += OnStreamStarted;
        _multiStreamService.StreamStopped += OnStreamStopped;
        _multiStreamService.StreamStatusChanged += OnStreamStatusChanged;
        
        InitializeUI();
        LoadMonitorsAndGenerateConfig();
        
        // Subscribe to UI change events after initialization
        SubscribeToUIEvents();
        _isInitializing = false;  // Fine dell'inizializzazione
    }

    private async void InitializeUI()
    {
        this.Text = "StreamVault - Multi-Monitor Streaming";
        this.Size = new Size(1200, 800);  // Aumentato da 800x600 a 1200x800
        this.StartPosition = FormStartPosition.CenterScreen;
        this.MinimumSize = new Size(1000, 700);  // Dimensione minima
        
        // Set default values
        textBoxBaseHost.Text = "127.0.0.1";
        numericBasePort.Value = 9999;
        textBoxChromeUrl.Text = "https://www.google.com";
        checkBoxAutoChrome.Checked = true;
        
        // Load saved configuration
        await LoadConfigurationAsync();
        
        UpdateUI();
        
        // Check FFmpeg availability
        await CheckFFmpegAvailability();
        
        _isInitializing = false;  // Configurazione caricata, ora possiamo iniziare a salvare
    }

    private async Task CheckFFmpegAvailability()
    {
        try
        {
            var ffmpegService = new FFmpegService(_logger);
            if (!ffmpegService.IsFFmpegAvailable)
            {
                labelStatus.Text = "FFmpeg not found - Install required";
                labelStatus.ForeColor = Color.Red;
                
                var result = MessageBox.Show(
                    "FFmpeg is required for streaming but was not found.\n\n" +
                    "Would you like to install FFmpeg now?",
                    "FFmpeg Required",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);
                
                if (result == DialogResult.Yes)
                {
                    using var setupDialog = new FFmpegSetupDialog();
                    setupDialog.ShowDialog(this);
                }
            }
            else
            {
                var version = await ffmpegService.GetVersionAsync();
                toolStripStatusLabelChrome.Text = $"FFmpeg: {version.Split(' ').FirstOrDefault()}";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error checking FFmpeg: {ex.Message}", ex);
        }
    }

    private async void LoadMonitorsAndGenerateConfig()
    {
        try
        {
            // Get available monitors using screen capture service
            var screenService = new ScreenCaptureService(_logger);
            var monitors = screenService.GetAvailableMonitors();
            
            // Generate stream sessions for all monitors
            _config.StreamSessions = MultiStreamService.GenerateStreamSessions(
                monitors, 
                textBoxBaseHost.Text, 
                (int)numericBasePort.Value);
            
            _config.DefaultChromeUrl = textBoxChromeUrl.Text;
            _config.AutoStartChrome = checkBoxAutoChrome.Checked;
            
            // Update the DataGridView
            RefreshStreamGrid();
            
            // Update status
            var chromeAvailable = _multiStreamService.IsChromeAvailable();
            var chromeVersion = await _multiStreamService.GetChromeVersionAsync();
            
            labelStatus.Text = $"Ready - {monitors.Count} monitors detected, Chrome: {(chromeAvailable ? "Available" : "Not Found")}";
            toolStripStatusLabelChrome.Text = $"Chrome: {chromeVersion}";
            
            _logger.Log($"Loaded {monitors.Count} monitors for multi-streaming");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error loading monitors: {ex.Message}", ex);
            MessageBox.Show($"Error loading monitors: {ex.Message}", "Error", 
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void RefreshStreamGrid()
    {
        dataGridViewStreams.DataSource = null;
        dataGridViewStreams.DataSource = _config.StreamSessions;
        
        // Configure columns
        dataGridViewStreams.Columns["Id"].Visible = false;
        dataGridViewStreams.Columns["ChromeLaunched"].Visible = false;
        dataGridViewStreams.Columns["ChromeProcessId"].Visible = false;
        dataGridViewStreams.Columns["StartTime"].Visible = false;
        
        dataGridViewStreams.AutoResizeColumns();
    }

    private async void ButtonStartAll_Click(object sender, EventArgs e)
    {
        try
        {
            if (_isStreaming)
            {
                await StopAllStreaming();
            }
            else
            {
                await StartAllStreaming();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in start/stop operation: {ex.Message}", ex);
            MessageBox.Show($"Error: {ex.Message}", "Operation Failed", 
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task StartAllStreaming()
    {
        try
        {
            labelStatus.Text = "Starting multi-stream...";
            buttonStartAll.Enabled = false;
            
            // Update config with current UI values
            _config.SrtHost = textBoxBaseHost.Text;
            _config.BaseSrtPort = (int)numericBasePort.Value;
            _config.DefaultChromeUrl = textBoxChromeUrl.Text;
            _config.AutoStartChrome = checkBoxAutoChrome.Checked;
            
            // Regenerate SRT URLs
            for (int i = 0; i < _config.StreamSessions.Count; i++)
            {
                _config.StreamSessions[i].SrtUrl = $"srt://{_config.SrtHost}:{_config.BaseSrtPort + i}";
            }
            
            var success = await _multiStreamService.StartAllStreamsAsync(_config);
            
            if (success)
            {
                _isStreaming = true;
                buttonStartAll.Text = "Stop All Streams";
                buttonStartAll.BackColor = Color.IndianRed;
                labelStatus.Text = $"Streaming active on {_config.StreamSessions.Count} monitors";
                labelStatus.ForeColor = Color.Green;
            }
            else
            {
                labelStatus.Text = "Failed to start some streams";
                labelStatus.ForeColor = Color.Orange;
            }
            
            RefreshStreamGrid();
        }
        finally
        {
            buttonStartAll.Enabled = true;
        }
    }

    private async Task StopAllStreaming()
    {
        try
        {
            labelStatus.Text = "Stopping all streams...";
            buttonStartAll.Enabled = false;
            
            var success = await _multiStreamService.StopAllStreamsAsync(_config.StreamSessions);
            
            _isStreaming = false;
            buttonStartAll.Text = "Start All Streams";
            buttonStartAll.BackColor = Color.LightGreen;
            
            if (success)
            {
                labelStatus.Text = "All streams stopped";
                labelStatus.ForeColor = Color.Black;
            }
            else
            {
                labelStatus.Text = "Some streams failed to stop";
                labelStatus.ForeColor = Color.Orange;
            }
            
            RefreshStreamGrid();
        }
        finally
        {
            buttonStartAll.Enabled = true;
        }
    }

    private void ButtonRefreshMonitors_Click(object sender, EventArgs e)
    {
        LoadMonitorsAndGenerateConfig();
    }

    private void ButtonAddMonitor_Click(object sender, EventArgs e)
    {
        try
        {
            // Show virtual monitor setup dialog
            using var setupDialog = new VirtualMonitorSetupDialog();
            
            if (setupDialog.ShowDialog(this) == DialogResult.OK && setupDialog.VirtualMonitor != null)
            {
                var virtualMonitor = setupDialog.VirtualMonitor;
                
                // Add to virtual monitors list
                _config.VirtualMonitors.Add(virtualMonitor);
                
                // Create a new virtual monitor stream session
                var newSession = new StreamSession
                {
                    Id = Guid.NewGuid().ToString(),
                    VirtualMonitor = virtualMonitor,
                    MonitorId = virtualMonitor.Id,
                    MonitorName = virtualMonitor.Name,
                    SrtUrl = $"srt://{textBoxBaseHost.Text}:{(int)numericBasePort.Value + _config.StreamSessions.Count}",
                    Status = "Ready",
                    IsVirtual = true
                };

                _config.StreamSessions.Add(newSession);
                RefreshStreamGrid();
                
                _logger.Log($"Added virtual monitor: {virtualMonitor.Name} based on Monitor {virtualMonitor.SourceMonitorIndex + 1}");
                
                // Save configuration
                if (!_isInitializing)
                {
                    _saveTimer.Stop();
                    _saveTimer.Start();
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding virtual monitor: {ex.Message}", ex);
            MessageBox.Show($"Error adding virtual monitor: {ex.Message}", "Error", 
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ButtonGenerateUrls_Click(object sender, EventArgs e)
    {
        try
        {
            // Regenerate SRT URLs based on current settings
            var baseHost = textBoxBaseHost.Text;
            var basePort = (int)numericBasePort.Value;
            
            for (int i = 0; i < _config.StreamSessions.Count; i++)
            {
                _config.StreamSessions[i].SrtUrl = $"srt://{baseHost}:{basePort + i}";
            }
            
            RefreshStreamGrid();
            _logger.Log("SRT URLs regenerated");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error generating URLs: {ex.Message}", ex);
        }
    }

    private async void ButtonTestChrome_Click(object sender, EventArgs e)
    {
        try
        {
            buttonTestChrome.Enabled = false;
            labelStatus.Text = "Testing Chrome...";
            
            var chromeService = new ChromeManagementService(_logger);
            var available = chromeService.IsChromeAvailable();
            var version = await chromeService.GetChromeVersionAsync();
            
            if (available)
            {
                MessageBox.Show($"Chrome is available!\n\nVersion: {version}", 
                               "Chrome Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Chrome is not available. Please install Google Chrome.", 
                               "Chrome Test", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
            labelStatus.Text = $"Chrome test completed - {(available ? "Available" : "Not Found")}";
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error testing Chrome: {ex.Message}", ex);
            MessageBox.Show($"Error testing Chrome: {ex.Message}", 
                           "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            buttonTestChrome.Enabled = true;
        }
    }

    private void OnStreamStarted(object? sender, StreamSession session)
    {
        if (InvokeRequired)
        {
            Invoke(new Action(() => OnStreamStarted(sender, session)));
            return;
        }
        
        RefreshStreamGrid();
        toolStripStatusLabelStreams.Text = $"Active streams: {_multiStreamService.GetActiveStreamCount()}";
    }

    private void OnStreamStopped(object? sender, StreamSession session)
    {
        if (InvokeRequired)
        {
            Invoke(new Action(() => OnStreamStopped(sender, session)));
            return;
        }
        
        RefreshStreamGrid();
        toolStripStatusLabelStreams.Text = $"Active streams: {_multiStreamService.GetActiveStreamCount()}";
    }

    private void OnStreamStatusChanged(object? sender, (StreamSession Session, string Status) data)
    {
        if (InvokeRequired)
        {
            Invoke(new Action(() => OnStreamStatusChanged(sender, data)));
            return;
        }
        
        RefreshStreamGrid();
    }

    private void UpdateUI()
    {
        buttonStartAll.Enabled = !_isStreaming || _config.StreamSessions.Any(s => s.IsActive);
    }

    protected override async void OnFormClosing(FormClosingEventArgs e)
    {
        if (_isStreaming)
        {
            var result = MessageBox.Show(
                "Streaming is currently active. Do you want to stop all streams and close?",
                "Confirm Close",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                await _multiStreamService.StopAllStreamsAsync(_config.StreamSessions);
            }
            else
            {
                e.Cancel = true;
                return;
            }
        }
        
        // Save configuration before closing
        await SaveConfigurationAsync();
        
        // Dispose timer
        _saveTimer?.Stop();
        _saveTimer?.Dispose();
        
        try
        {
            await _multiStreamService.DisposeAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error during cleanup: {ex.Message}", ex);
        }
        
        base.OnFormClosing(e);
    }

    #region Configuration Management

    private async Task LoadConfigurationAsync()
    {
        try
        {
            _config = await _configService.LoadConfigurationAsync();
            
            // Apply loaded configuration to UI
            textBoxBaseHost.Text = _config.SrtHost;
            numericBasePort.Value = _config.BaseSrtPort;
            textBoxChromeUrl.Text = _config.DefaultChromeUrl;
            checkBoxAutoChrome.Checked = _config.AutoStartChrome;
            
            _logger.Log($"Configuration loaded successfully - Host: {_config.SrtHost}, Port: {_config.BaseSrtPort}, URL: {_config.DefaultChromeUrl}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error loading configuration: {ex.Message}", ex);
        }
    }

    private async Task SaveConfigurationAsync()
    {
        try
        {
            // Update config with current UI values
            _config.SrtHost = textBoxBaseHost.Text;
            _config.BaseSrtPort = (int)numericBasePort.Value;
            _config.DefaultChromeUrl = textBoxChromeUrl.Text;
            _config.AutoStartChrome = checkBoxAutoChrome.Checked;
            
            await _configService.SaveConfigurationAsync(_config);
            _logger.Log($"Configuration saved successfully - Host: {_config.SrtHost}, Port: {_config.BaseSrtPort}, URL: {_config.DefaultChromeUrl}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error saving configuration: {ex.Message}", ex);
        }
    }

    #endregion

    private void SubscribeToUIEvents()
    {
        textBoxBaseHost.TextChanged += OnConfigurationChanged;
        numericBasePort.ValueChanged += OnConfigurationChanged;
        textBoxChromeUrl.TextChanged += OnConfigurationChanged;
        checkBoxAutoChrome.CheckedChanged += OnConfigurationChanged;
    }

    private async void OnConfigurationChanged(object? sender, EventArgs e)
    {
        if (_isInitializing) return;  // Non salvare durante l'inizializzazione
        
        // Restart timer - salva solo dopo 1 secondo di inattività
        _saveTimer.Stop();
        _saveTimer.Start();
    }

    private async void ButtonSaveConfig_Click(object? sender, EventArgs e)
    {
        await SaveConfigurationAsync();
        MessageBox.Show($"Configuration saved to: {_configService.GetConfigurationPath()}", 
                       "Configuration Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void ButtonDebugFFmpeg_Click(object? sender, EventArgs e)
    {
        try
        {
            using var debugDialog = new FFmpegDebugDialog();
            debugDialog.ShowDialog(this);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error opening FFmpeg debug dialog: {ex.Message}", ex);
            MessageBox.Show($"Error opening debug dialog: {ex.Message}", 
                           "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
