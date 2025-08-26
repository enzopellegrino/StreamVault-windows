using StreamVault.Models;

namespace StreamVault.Forms;

public partial class VirtualMonitorListDialog : Form
{
    private readonly List<VirtualMonitorInfo> _virtualMonitors;
    private VirtualMonitorInfo? _selectedMonitor;

    public VirtualMonitorListDialog(List<VirtualMonitorInfo> virtualMonitors)
    {
        InitializeComponent();
        _virtualMonitors = virtualMonitors;
        LoadVirtualMonitors();
    }

    private void LoadVirtualMonitors()
    {
        listBoxMonitors.Items.Clear();
        
        foreach (var monitor in _virtualMonitors)
        {
            listBoxMonitors.Items.Add($"{monitor.Name} - {monitor.Resolution.Width}x{monitor.Resolution.Height} (Created: {monitor.CreatedAt:MM/dd HH:mm})");
        }

        if (listBoxMonitors.Items.Count > 0)
        {
            listBoxMonitors.SelectedIndex = 0;
        }

        buttonRemove.Enabled = listBoxMonitors.Items.Count > 0;
    }

    private void ListBoxMonitors_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (listBoxMonitors.SelectedIndex >= 0 && listBoxMonitors.SelectedIndex < _virtualMonitors.Count)
        {
            _selectedMonitor = _virtualMonitors[listBoxMonitors.SelectedIndex];
            
            // Update details
            var monitor = _selectedMonitor;
            labelDetails.Text = $"ID: {monitor.Id}\n" +
                               $"Size: {monitor.Resolution.Width}x{monitor.Resolution.Height}\n" +
                               $"Status: {(monitor.IsActive ? "Active" : "Inactive")}\n" +
                               $"Created: {monitor.CreatedAt:yyyy-MM-dd HH:mm:ss}";
            
            buttonRemove.Enabled = true;
        }
        else
        {
            _selectedMonitor = null;
            labelDetails.Text = "No monitor selected";
            buttonRemove.Enabled = false;
        }
    }

    private void ButtonRemove_Click(object sender, EventArgs e)
    {
        if (_selectedMonitor != null)
        {
            var result = MessageBox.Show($"Are you sure you want to remove virtual monitor '{_selectedMonitor.Name}'?",
                                       "Confirm Removal",
                                       MessageBoxButtons.YesNo,
                                       MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }

    private void ButtonCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    public VirtualMonitorInfo? GetSelectedMonitor()
    {
        return _selectedMonitor;
    }
}
