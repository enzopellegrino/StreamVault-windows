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
    private System.Windows.Forms.Timer _refreshTimer;  // Timer per aggiornare la UI

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
        
        // Initialize refresh timer for real-time updates
        _refreshTimer = new System.Windows.Forms.Timer();
        _refreshTimer.Interval = 1000; // Update every second
        _refreshTimer.Tick += (s, e) => {
            if (!_isInitializing && _config.StreamSessions.Any(session => session.IsActive))
            {
                UpdateGridDisplayData();
            }
        };
        _refreshTimer.Start();
        
        // Subscribe to events
        _multiStreamService.StreamStarted += OnStreamStarted;
        _multiStreamService.StreamStopped += OnStreamStopped;
        _multiStreamService.StreamStatusChanged += OnStreamStatusChanged;
        
        InitializeUI();
        LoadMonitorsAndGenerateConfig();
        
        // Subscribe to UI change events after initialization
        SubscribeToUIEvents();
        _isInitializing = false;  // Fine dell'inizializzazione
        
        // Load SRT servers
        LoadSrtServers();
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _refreshTimer?.Stop();
        _refreshTimer?.Dispose();
        _saveTimer?.Stop();
        _saveTimer?.Dispose();
        base.OnFormClosed(e);
    }

    private async void InitializeUI()
    {
        this.Text = "StreamVault - Screen Capture & Streaming";
        this.Size = new Size(1210, 720);  // Dimensioni finestra principale
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
        try
        {
            dataGridViewStreams.DataSource = null;
            
            // Setup custom DataGridView with action columns
            dataGridViewStreams.Columns.Clear();
            dataGridViewStreams.AutoGenerateColumns = false;
            
            // Add Monitor Name column
            var monitorColumn = new DataGridViewTextBoxColumn
            {
                Name = "MonitorName",
                HeaderText = "Monitor",
                DataPropertyName = "MonitorName",
                Width = 120,
                ReadOnly = true
            };
            dataGridViewStreams.Columns.Add(monitorColumn);
            
            // Add SRT URL column
            var srtColumn = new DataGridViewTextBoxColumn
            {
                Name = "SrtUrl",
                HeaderText = "SRT URL",
                DataPropertyName = "SrtUrl",
                Width = 180,
                ReadOnly = true
            };
            dataGridViewStreams.Columns.Add(srtColumn);
            
            // Add Status column with color coding
            var statusColumn = new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "Status",
                DataPropertyName = "Status",
                Width = 100,
                ReadOnly = true
            };
            dataGridViewStreams.Columns.Add(statusColumn);
            
            // Add Start/Stop Action column
            var actionColumn = new DataGridViewButtonColumn
            {
                Name = "Action",
                HeaderText = "Action",
                Text = "Start",
                UseColumnTextForButtonValue = false,
                Width = 80
            };
            dataGridViewStreams.Columns.Add(actionColumn);
            
            // Add Duration column
            var durationColumn = new DataGridViewTextBoxColumn
            {
                Name = "Duration",
                HeaderText = "Duration",
                Width = 80,
                ReadOnly = true
            };
            dataGridViewStreams.Columns.Add(durationColumn);
            
            // Add Bitrate column
            var bitrateColumn = new DataGridViewTextBoxColumn
            {
                Name = "Bitrate",
                HeaderText = "Bitrate (kbps)",
                DataPropertyName = "Bitrate",
                Width = 100,
                ReadOnly = true
            };
            dataGridViewStreams.Columns.Add(bitrateColumn);
            
            // Add FPS column
            var fpsColumn = new DataGridViewTextBoxColumn
            {
                Name = "Fps",
                HeaderText = "FPS",
                DataPropertyName = "Fps",
                Width = 60,
                ReadOnly = true
            };
            dataGridViewStreams.Columns.Add(fpsColumn);
            
            // Add Is Virtual column
            var virtualColumn = new DataGridViewCheckBoxColumn
            {
                Name = "IsVirtual",
                HeaderText = "Virtual",
                DataPropertyName = "IsVirtual",
                Width = 70,
                ReadOnly = true
            };
            dataGridViewStreams.Columns.Add(virtualColumn);
            
            // Add Chrome Status column
            var chromeColumn = new DataGridViewTextBoxColumn
            {
                Name = "ChromeStatus",
                HeaderText = "Chrome",
                Width = 80,
                ReadOnly = true
            };
            dataGridViewStreams.Columns.Add(chromeColumn);
            
            // Bind data
            dataGridViewStreams.DataSource = _config.StreamSessions;
            
            // Update action button text and duration
            UpdateGridDisplayData();
            
            // Handle button clicks
            dataGridViewStreams.CellClick -= DataGridViewStreams_CellClick;
            dataGridViewStreams.CellClick += DataGridViewStreams_CellClick;
            
            // Handle cell formatting for colors
            dataGridViewStreams.CellFormatting -= DataGridViewStreams_CellFormatting;
            dataGridViewStreams.CellFormatting += DataGridViewStreams_CellFormatting;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error refreshing stream grid: {ex.Message}", ex);
        }
    }

    private void UpdateGridDisplayData()
    {
        for (int i = 0; i < dataGridViewStreams.Rows.Count && i < _config.StreamSessions.Count; i++)
        {
            var session = _config.StreamSessions[i];
            var row = dataGridViewStreams.Rows[i];
            
            // Update action button text
            row.Cells["Action"].Value = session.IsActive ? "Stop" : "Start";
            
            // Update duration
            if (session.IsActive && session.StartTime != DateTime.MinValue)
            {
                var duration = DateTime.Now - session.StartTime;
                row.Cells["Duration"].Value = $"{duration.Hours:D2}:{duration.Minutes:D2}:{duration.Seconds:D2}";
            }
            else
            {
                row.Cells["Duration"].Value = "--:--:--";
            }
            
            // Update Chrome status
            row.Cells["ChromeStatus"].Value = session.ChromeLaunched ? "Active" : "Stopped";
        }
    }

    private void DataGridViewStreams_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
        {
            var columnName = dataGridViewStreams.Columns[e.ColumnIndex].Name;
            
            if (columnName == "Action" && e.RowIndex < _config.StreamSessions.Count)
            {
                var session = _config.StreamSessions[e.RowIndex];
                _ = Task.Run(async () =>
                {
                    try
                    {
                        if (session.IsActive)
                        {
                            await StopIndividualStream(session);
                        }
                        else
                        {
                            await StartIndividualStream(session);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error toggling individual stream: {ex.Message}", ex);
                        if (InvokeRequired)
                        {
                            Invoke(new Action(() => 
                            {
                                MessageBox.Show($"Error toggling stream: {ex.Message}", "Error", 
                                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }));
                        }
                    }
                });
            }
        }
    }

    private void DataGridViewStreams_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex >= 0 && e.RowIndex < _config.StreamSessions.Count)
        {
            var session = _config.StreamSessions[e.RowIndex];
            var columnName = dataGridViewStreams.Columns[e.ColumnIndex].Name;
            
            // Color code the status column
            if (columnName == "Status")
            {
                switch (session.Status.ToLower())
                {
                    case "streaming":
                    case "active":
                        e.CellStyle.BackColor = Color.LightGreen;
                        e.CellStyle.ForeColor = Color.DarkGreen;
                        break;
                    case "starting":
                    case "initializing":
                        e.CellStyle.BackColor = Color.LightYellow;
                        e.CellStyle.ForeColor = Color.DarkOrange;
                        break;
                    case "error":
                    case "failed":
                        e.CellStyle.BackColor = Color.LightCoral;
                        e.CellStyle.ForeColor = Color.DarkRed;
                        break;
                    case "stopping":
                        e.CellStyle.BackColor = Color.LightGray;
                        e.CellStyle.ForeColor = Color.Black;
                        break;
                    default: // Ready, Stopped, etc.
                        e.CellStyle.BackColor = Color.White;
                        e.CellStyle.ForeColor = Color.Black;
                        break;
                }
            }
            
            // Color code the Action button
            if (columnName == "Action")
            {
                if (session.IsActive)
                {
                    e.CellStyle.BackColor = Color.IndianRed;
                    e.CellStyle.ForeColor = Color.White;
                }
                else
                {
                    e.CellStyle.BackColor = Color.LightGreen;
                    e.CellStyle.ForeColor = Color.Black;
                }
            }
        }
    }

    private async Task StartIndividualStream(StreamSession session)
    {
        try
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => 
                {
                    session.Status = "Starting...";
                    RefreshStreamGrid();
                }));
            }
            
            var success = await _multiStreamService.StartStreamAsync(session);
            
            if (InvokeRequired)
            {
                Invoke(new Action(() => 
                {
                    session.Status = success ? "Streaming" : "Failed";
                    session.IsActive = success;
                    if (success) session.StartTime = DateTime.Now;
                    RefreshStreamGrid();
                    UpdateStatusLabels();
                }));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error starting individual stream: {ex.Message}", ex);
            if (InvokeRequired)
            {
                Invoke(new Action(() => 
                {
                    session.Status = "Error";
                    session.IsActive = false;
                    RefreshStreamGrid();
                }));
            }
            throw;
        }
    }

    private async Task StopIndividualStream(StreamSession session)
    {
        try
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => 
                {
                    session.Status = "Stopping...";
                    RefreshStreamGrid();
                }));
            }
            
            var success = await _multiStreamService.StopStreamAsync(session);
            
            if (InvokeRequired)
            {
                Invoke(new Action(() => 
                {
                    session.Status = success ? "Stopped" : "Error";
                    session.IsActive = false;
                    session.StartTime = DateTime.MinValue;
                    RefreshStreamGrid();
                    UpdateStatusLabels();
                }));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error stopping individual stream: {ex.Message}", ex);
            if (InvokeRequired)
            {
                Invoke(new Action(() => 
                {
                    session.Status = "Error";
                    RefreshStreamGrid();
                }));
            }
            throw;
        }
    }

    private void UpdateStatusLabels()
    {
        var activeCount = _config.StreamSessions.Count(s => s.IsActive);
        toolStripStatusLabelStreams.Text = $"Active streams: {activeCount}";
        
        if (activeCount > 0)
        {
            labelStatus.Text = $"Streaming: {activeCount} of {_config.StreamSessions.Count} monitors";
            labelStatus.ForeColor = Color.Green;
        }
        else if (_config.StreamSessions.Any(s => s.Status == "Failed" || s.Status == "Error"))
        {
            labelStatus.Text = "Some streams have errors";
            labelStatus.ForeColor = Color.Red;
        }
        else
        {
            labelStatus.Text = "Ready";
            labelStatus.ForeColor = Color.Black;
        }
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
        
        data.Session.Status = data.Status;
        RefreshStreamGrid();
        UpdateStatusLabels();
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
        comboBoxSrtServers.SelectedIndexChanged += OnConfigurationChanged;
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

    #region SRT Server Management

    private void LoadSrtServers()
    {
        try
        {
            comboBoxSrtServers.Items.Clear();
            
            // Ensure we have some default servers
            if (!_config.SrtServers.Any())
            {
                _config.SrtServers.AddRange(new[]
                {
                    SrtServerInfo.CreateDefault(),
                    SrtServerInfo.CreateOBSPreset(),
                    SrtServerInfo.CreateVLCPreset()
                });
                _config.SelectedSrtServerId = _config.SrtServers.First().Id;
            }
            
            // Add servers to combo box
            foreach (var server in _config.SrtServers.Where(s => s.IsActive))
            {
                comboBoxSrtServers.Items.Add(server);
            }
            
            // Select the current server
            if (!string.IsNullOrEmpty(_config.SelectedSrtServerId))
            {
                var selectedServer = _config.SrtServers.FirstOrDefault(s => s.Id == _config.SelectedSrtServerId);
                if (selectedServer != null)
                {
                    comboBoxSrtServers.SelectedItem = selectedServer;
                }
            }
            
            // If nothing selected, select the first one
            if (comboBoxSrtServers.SelectedIndex < 0 && comboBoxSrtServers.Items.Count > 0)
            {
                comboBoxSrtServers.SelectedIndex = 0;
            }
            
            _logger.Log($"Loaded {_config.SrtServers.Count} SRT servers, {comboBoxSrtServers.Items.Count} active");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error loading SRT servers: {ex.Message}", ex);
        }
    }

    private void ComboBoxSrtServers_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (comboBoxSrtServers.SelectedItem is SrtServerInfo selectedServer)
        {
            _config.SelectedSrtServerId = selectedServer.Id;
            selectedServer.LastUsed = DateTime.Now;
            
            // Update base host and port to match selected server
            textBoxBaseHost.Text = selectedServer.Host;
            numericBasePort.Value = selectedServer.Port;
            
            _logger.Log($"Selected SRT server: {selectedServer.DisplayText}");
            
            // Trigger configuration save
            if (!_isInitializing)
            {
                _saveTimer.Stop();
                _saveTimer.Start();
            }
        }
    }

    private void ButtonManageSrtServers_Click(object sender, EventArgs e)
    {
        try
        {
            using var dialog = new SrtServerManagerDialog(_config.SrtServers, _logger);
            
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                _config.SrtServers = new List<SrtServerInfo>(dialog.Servers);
                
                // Ensure selected server is still valid
                if (!_config.SrtServers.Any(s => s.Id == _config.SelectedSrtServerId))
                {
                    var firstActiveServer = _config.SrtServers.FirstOrDefault(s => s.IsActive);
                    _config.SelectedSrtServerId = firstActiveServer?.Id ?? string.Empty;
                }
                
                LoadSrtServers();
                
                // Save changes
                if (!_isInitializing)
                {
                    _saveTimer.Stop();
                    _saveTimer.Start();
                }
                
                _logger.Log($"SRT servers updated: {_config.SrtServers.Count} total servers");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error managing SRT servers: {ex.Message}", ex);
            MessageBox.Show($"Error managing SRT servers: {ex.Message}", 
                           "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private SrtServerInfo? GetSelectedSrtServer()
    {
        return comboBoxSrtServers.SelectedItem as SrtServerInfo;
    }

    private void ButtonVirtualDesktops_Click(object sender, EventArgs e)
    {
        try
        {
            using var dialog = new VirtualDesktopManagerDialog(_logger);
            dialog.ShowDialog(this);
            
            // Refresh monitors after managing virtual desktops
            LoadMonitorsAndGenerateConfig();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error opening virtual desktop manager: {ex.Message}", ex);
            MessageBox.Show($"Error opening virtual desktop manager: {ex.Message}", 
                           "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    #endregion
}
