using StreamVault.Models;
using StreamVault.Services;
using System.Drawing.Imaging;

namespace StreamVault.Forms;

public partial class VirtualMonitorSetupDialog : Form
{
    private readonly LoggingService _logger;
    private readonly ScreenCaptureService _screenService;
    private List<MonitorInfo> _availableMonitors;
    private MonitorInfo? _selectedMonitor;
    
    public VirtualMonitorInfo? VirtualMonitor { get; private set; }

    public VirtualMonitorSetupDialog()
    {
        InitializeComponent();
        _logger = new LoggingService();
        _screenService = new ScreenCaptureService(_logger);
        _availableMonitors = new List<MonitorInfo>();
        
        LoadAvailableMonitors();
        SetDefaultValues();
    }

    private void LoadAvailableMonitors()
    {
        try
        {
            _availableMonitors = _screenService.GetAvailableMonitors();
            
            listBoxMonitors.Items.Clear();
            foreach (var monitor in _availableMonitors)
            {
                var displayText = $"Monitor {monitor.Index + 1}: {monitor.Resolution.Width}x{monitor.Resolution.Height}";
                if (monitor.IsPrimary)
                    displayText += " (Primary)";
                    
                listBoxMonitors.Items.Add(displayText);
            }
            
            // Select first monitor by default
            if (listBoxMonitors.Items.Count > 0)
            {
                listBoxMonitors.SelectedIndex = 0;
            }
            
            _logger.Log($"Loaded {_availableMonitors.Count} monitors for virtual monitor setup");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error loading monitors: {ex.Message}", ex);
            MessageBox.Show($"Error loading monitors: {ex.Message}", "Error", 
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void SetDefaultValues()
    {
        var virtualCount = 1; // This should be passed from the parent form
        textBoxMonitorName.Text = $"Virtual Monitor {virtualCount}";
    }

    private void ListBoxMonitors_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (listBoxMonitors.SelectedIndex >= 0 && listBoxMonitors.SelectedIndex < _availableMonitors.Count)
        {
            _selectedMonitor = _availableMonitors[listBoxMonitors.SelectedIndex];
            UpdateMonitorInfo();
            CapturePreview();
        }
    }

    private void UpdateMonitorInfo()
    {
        if (_selectedMonitor != null)
        {
            textBoxResolution.Text = $"{_selectedMonitor.Resolution.Width}x{_selectedMonitor.Resolution.Height}";
            checkBoxPrimary.Checked = _selectedMonitor.IsPrimary;
        }
    }

    private async void CapturePreview()
    {
        if (_selectedMonitor == null) return;

        try
        {
            buttonRefreshPreview.Enabled = false;
            buttonRefreshPreview.Text = "Capturing...";
            
            // Capture screenshot of selected monitor
            await Task.Run(() =>
            {
                try
                {
                    var bounds = _selectedMonitor.Bounds;
                    using var bitmap = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb);
                    using var graphics = Graphics.FromImage(bitmap);
                    
                    graphics.CopyFromScreen(bounds.Left, bounds.Top, 0, 0, bounds.Size, CopyPixelOperation.SourceCopy);
                    
                    // Update UI on main thread
                    this.Invoke(new Action(() =>
                    {
                        try
                        {
                            pictureBoxPreview.Image?.Dispose();
                            pictureBoxPreview.Image = new Bitmap(bitmap);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Error updating preview image: {ex.Message}", ex);
                        }
                    }));
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error capturing screen: {ex.Message}", ex);
                    this.Invoke(new Action(() =>
                    {
                        MessageBox.Show($"Error capturing screen preview: {ex.Message}", "Preview Error", 
                                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }));
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in capture preview: {ex.Message}", ex);
        }
        finally
        {
            buttonRefreshPreview.Enabled = true;
            buttonRefreshPreview.Text = "Refresh";
        }
    }

    private void ButtonRefreshPreview_Click(object sender, EventArgs e)
    {
        CapturePreview();
    }

    private void ButtonOK_Click(object sender, EventArgs e)
    {
        if (_selectedMonitor == null)
        {
            MessageBox.Show("Please select a monitor first.", "No Monitor Selected", 
                           MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(textBoxMonitorName.Text))
        {
            MessageBox.Show("Please enter a name for the virtual monitor.", "Name Required", 
                           MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Create virtual monitor info
        VirtualMonitor = new VirtualMonitorInfo
        {
            Id = Guid.NewGuid().ToString(),
            Name = textBoxMonitorName.Text.Trim(),
            SourceMonitorIndex = _selectedMonitor.Index,
            Resolution = _selectedMonitor.Resolution,
            Bounds = _selectedMonitor.Bounds,
            IsPrimary = checkBoxPrimary.Checked,
            CreatedAt = DateTime.Now
        };

        _logger.Log($"Created virtual monitor: {VirtualMonitor.Name} based on Monitor {_selectedMonitor.Index + 1}");
        
        DialogResult = DialogResult.OK;
        Close();
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        try
        {
            pictureBoxPreview.Image?.Dispose();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error disposing preview image: {ex.Message}", ex);
        }
        
        base.OnFormClosing(e);
    }
}
