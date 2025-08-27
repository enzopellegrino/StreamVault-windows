using StreamVault.Models;
using StreamVault.Services;

namespace StreamVault.Forms;

public partial class SrtServerEditDialog : Form
{
    private readonly LoggingService _logger;
    private SrtServerInfo _server;

    public SrtServerInfo Server => _server;

    public SrtServerEditDialog(SrtServerInfo server, LoggingService logger)
    {
        _logger = logger;
        _server = new SrtServerInfo
        {
            Id = server.Id,
            Name = server.Name,
            Host = server.Host,
            Port = server.Port,
            StreamKey = server.StreamKey,
            Description = server.Description,
            IsActive = server.IsActive,
            CreatedDate = server.CreatedDate,
            LastUsed = server.LastUsed
        };
        
        InitializeComponent();
        LoadServerData();
    }

    private void LoadServerData()
    {
        textBoxName.Text = _server.Name;
        textBoxHost.Text = _server.Host;
        numericPort.Value = _server.Port;
        textBoxStreamKey.Text = _server.StreamKey;
        textBoxDescription.Text = _server.Description;
        checkBoxActive.Checked = _server.IsActive;
        
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

    private void TextBox_TextChanged(object sender, EventArgs e)
    {
        UpdateSrtUrl();
    }

    private void NumericPort_ValueChanged(object sender, EventArgs e)
    {
        UpdateSrtUrl();
    }

    private void ButtonOK_Click(object sender, EventArgs e)
    {
        if (!ValidateInput())
        {
            return;
        }

        // Update server with form data
        _server.Name = textBoxName.Text.Trim();
        _server.Host = textBoxHost.Text.Trim();
        _server.Port = (int)numericPort.Value;
        _server.StreamKey = textBoxStreamKey.Text.Trim();
        _server.Description = textBoxDescription.Text.Trim();
        _server.IsActive = checkBoxActive.Checked;

        DialogResult = DialogResult.OK;
        Close();
    }

    private bool ValidateInput()
    {
        // Validate name
        if (string.IsNullOrWhiteSpace(textBoxName.Text))
        {
            MessageBox.Show("Please enter a server name.", "Validation Error", 
                           MessageBoxButtons.OK, MessageBoxIcon.Warning);
            textBoxName.Focus();
            return false;
        }

        // Validate host
        if (string.IsNullOrWhiteSpace(textBoxHost.Text))
        {
            MessageBox.Show("Please enter a host address.", "Validation Error", 
                           MessageBoxButtons.OK, MessageBoxIcon.Warning);
            textBoxHost.Focus();
            return false;
        }

        // Validate port
        if (numericPort.Value < 1 || numericPort.Value > 65535)
        {
            MessageBox.Show("Port must be between 1 and 65535.", "Validation Error", 
                           MessageBoxButtons.OK, MessageBoxIcon.Warning);
            numericPort.Focus();
            return false;
        }

        return true;
    }

    private void ButtonCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
