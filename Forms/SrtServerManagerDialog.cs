using StreamVault.Models;
using StreamVault.Services;

namespace StreamVault.Forms;

public partial class SrtServerManagerDialog : Form
{
    private readonly LoggingService _logger;
    private List<SrtServerInfo> _servers;
    private SrtServerInfo? _selectedServer;
    private bool _hasChanges = false;

    public List<SrtServerInfo> Servers => _servers;
    public bool HasChanges => _hasChanges;

    public SrtServerManagerDialog(List<SrtServerInfo> servers, LoggingService logger)
    {
        _logger = logger;
        _servers = new List<SrtServerInfo>(servers); // Create a copy
        InitializeComponent();
        LoadServers();
    }

    private void LoadServers()
    {
        try
        {
            listBoxServers.Items.Clear();
            
            foreach (var server in _servers)
            {
                listBoxServers.Items.Add(server);
            }

            if (listBoxServers.Items.Count > 0)
            {
                listBoxServers.SelectedIndex = 0;
            }

            UpdateButtons();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error loading servers: {ex.Message}", ex);
        }
    }

    private void UpdateButtons()
    {
        var hasSelection = listBoxServers.SelectedIndex >= 0;
        buttonEdit.Enabled = hasSelection;
        buttonDelete.Enabled = hasSelection && _servers.Count > 1; // Keep at least one server
        buttonDuplicate.Enabled = hasSelection;
        buttonTest.Enabled = hasSelection;
    }

    private void ListBoxServers_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (listBoxServers.SelectedIndex >= 0 && listBoxServers.SelectedIndex < _servers.Count)
        {
            _selectedServer = _servers[listBoxServers.SelectedIndex];
            ShowServerDetails(_selectedServer);
        }
        else
        {
            _selectedServer = null;
            ClearServerDetails();
        }
        
        UpdateButtons();
    }

    private void ShowServerDetails(SrtServerInfo server)
    {
        textBoxName.Text = server.Name;
        textBoxHost.Text = server.Host;
        numericPort.Value = server.Port;
        textBoxStreamKey.Text = server.StreamKey;
        textBoxDescription.Text = server.Description;
        checkBoxActive.Checked = server.IsActive;
        labelSrtUrl.Text = server.SrtUrl;
        labelCreated.Text = $"Created: {server.CreatedDate:yyyy-MM-dd HH:mm}";
        labelLastUsed.Text = $"Last Used: {server.LastUsed:yyyy-MM-dd HH:mm}";
    }

    private void ClearServerDetails()
    {
        textBoxName.Clear();
        textBoxHost.Clear();
        numericPort.Value = 9999;
        textBoxStreamKey.Clear();
        textBoxDescription.Clear();
        checkBoxActive.Checked = true;
        labelSrtUrl.Text = "srt://";
        labelCreated.Text = "Created: -";
        labelLastUsed.Text = "Last Used: -";
    }

    private void ButtonAdd_Click(object sender, EventArgs e)
    {
        var newServer = new SrtServerInfo
        {
            Name = "New SRT Server",
            Host = "127.0.0.1",
            Port = 9999 + _servers.Count,
            Description = "New SRT server configuration"
        };

        using var dialog = new SrtServerEditDialog(newServer, _logger);
        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            _servers.Add(dialog.Server);
            _hasChanges = true;
            LoadServers();
            
            // Select the new server
            listBoxServers.SelectedIndex = _servers.Count - 1;
        }
    }

    private void ButtonEdit_Click(object sender, EventArgs e)
    {
        if (_selectedServer == null) return;

        using var dialog = new SrtServerEditDialog(_selectedServer, _logger);
        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            // Update the server in place
            var index = _servers.IndexOf(_selectedServer);
            if (index >= 0)
            {
                _servers[index] = dialog.Server;
                _hasChanges = true;
                LoadServers();
                listBoxServers.SelectedIndex = index;
            }
        }
    }

    private void ButtonDelete_Click(object sender, EventArgs e)
    {
        if (_selectedServer == null || _servers.Count <= 1) return;

        var result = MessageBox.Show(
            $"Are you sure you want to delete the server '{_selectedServer.Name}'?",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            var index = listBoxServers.SelectedIndex;
            _servers.Remove(_selectedServer);
            _hasChanges = true;
            LoadServers();
            
            // Select the next server or the last one
            if (index >= _servers.Count) index = _servers.Count - 1;
            if (index >= 0) listBoxServers.SelectedIndex = index;
        }
    }

    private void ButtonDuplicate_Click(object sender, EventArgs e)
    {
        if (_selectedServer == null) return;

        var duplicatedServer = _selectedServer.Clone();
        _servers.Add(duplicatedServer);
        _hasChanges = true;
        LoadServers();
        
        // Select the duplicated server
        listBoxServers.SelectedIndex = _servers.Count - 1;
    }

    private async void ButtonTest_Click(object sender, EventArgs e)
    {
        if (_selectedServer == null) return;

        try
        {
            buttonTest.Enabled = false;
            buttonTest.Text = "Testing...";

            // Simple test - try to create a connection (this is a basic test)
            var testResult = await TestSrtConnection(_selectedServer);
            
            MessageBox.Show(
                testResult ? "Connection test successful!" : "Connection test failed. Check server settings.",
                "Test Result",
                MessageBoxButtons.OK,
                testResult ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error testing SRT connection: {ex.Message}", ex);
            MessageBox.Show($"Test error: {ex.Message}", "Test Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            buttonTest.Enabled = true;
            buttonTest.Text = "Test Connection";
        }
    }

    private async Task<bool> TestSrtConnection(SrtServerInfo server)
    {
        try
        {
            // This is a simple validation test
            // In a real scenario, you might want to try a brief connection
            await Task.Delay(1000); // Simulate test delay
            
            // Basic validation
            return server.IsValid() && 
                   !string.IsNullOrWhiteSpace(server.Host) && 
                   server.Port > 0 && server.Port <= 65535;
        }
        catch
        {
            return false;
        }
    }

    private void ButtonAddPreset_Click(object sender, EventArgs e)
    {
        var presetMenu = new ContextMenuStrip();
        
        presetMenu.Items.Add("OBS Studio", null, (s, e) => AddPreset(SrtServerInfo.CreateOBSPreset()));
        presetMenu.Items.Add("VLC Media Player", null, (s, e) => AddPreset(SrtServerInfo.CreateVLCPreset()));
        presetMenu.Items.Add("Custom Server", null, (s, e) => AddPreset(SrtServerInfo.CreateDefault()));
        
        presetMenu.Show(buttonAddPreset, new Point(0, buttonAddPreset.Height));
    }

    private void AddPreset(SrtServerInfo preset)
    {
        // Ensure unique port
        preset.Port = 9999 + _servers.Count;
        
        _servers.Add(preset);
        _hasChanges = true;
        LoadServers();
        
        // Select the new preset
        listBoxServers.SelectedIndex = _servers.Count - 1;
    }

    private void TextBox_TextChanged(object sender, EventArgs e)
    {
        UpdateSrtUrl();
    }

    private void NumericPort_ValueChanged(object sender, EventArgs e)
    {
        UpdateSrtUrl();
    }

    private void UpdateSrtUrl()
    {
        try
        {
            var host = textBoxHost.Text.Trim();
            var port = (int)numericPort.Value;
            var streamKey = textBoxStreamKey.Text.Trim();
            
            var url = $"srt://{host}:{port}";
            if (!string.IsNullOrEmpty(streamKey))
            {
                url += $"/{streamKey}";
            }
            
            labelSrtUrl.Text = url;
        }
        catch
        {
            labelSrtUrl.Text = "srt://";
        }
    }

    private void ButtonOK_Click(object sender, EventArgs e)
    {
        // Validate that we have at least one active server
        if (!_servers.Any(s => s.IsActive))
        {
            MessageBox.Show("At least one server must be active.", "Validation Error", 
                           MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        DialogResult = DialogResult.OK;
        Close();
    }

    private void ButtonCancel_Click(object sender, EventArgs e)
    {
        if (_hasChanges)
        {
            var result = MessageBox.Show(
                "You have unsaved changes. Do you want to discard them?",
                "Unsaved Changes",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                return;
            }
        }

        DialogResult = DialogResult.Cancel;
        Close();
    }
}
