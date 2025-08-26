using StreamVault.Models;

namespace StreamVault.Forms;

public partial class VirtualMonitorDialog : Form
{
    private VirtualMonitorConfig _config = new();

    public VirtualMonitorDialog()
    {
        InitializeComponent();
        LoadPresets();
        SetDefaultValues();
    }

    private void LoadPresets()
    {
        comboBoxPresets.Items.Clear();
        foreach (var preset in VirtualMonitorConfig.Presets)
        {
            comboBoxPresets.Items.Add($"{preset.Name} ({preset.Width}x{preset.Height})");
        }
        comboBoxPresets.SelectedIndex = 1; // Full HD by default
    }

    private void SetDefaultValues()
    {
        textBoxName.Text = "Virtual Monitor 1";
        numericWidth.Value = 1920;
        numericHeight.Value = 1080;
        numericRefreshRate.Value = 60;
    }

    private void ComboBoxPresets_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (comboBoxPresets.SelectedIndex >= 0 && comboBoxPresets.SelectedIndex < VirtualMonitorConfig.Presets.Count)
        {
            var preset = VirtualMonitorConfig.Presets[comboBoxPresets.SelectedIndex];
            
            if (preset.Width > 0 && preset.Height > 0) // Not "Custom"
            {
                numericWidth.Value = preset.Width;
                numericHeight.Value = preset.Height;
                numericWidth.Enabled = false;
                numericHeight.Enabled = false;
            }
            else // Custom
            {
                numericWidth.Enabled = true;
                numericHeight.Enabled = true;
            }
        }
    }

    private void ButtonOK_Click(object sender, EventArgs e)
    {
        if (ValidateInput())
        {
            _config.Name = textBoxName.Text.Trim();
            _config.Width = (int)numericWidth.Value;
            _config.Height = (int)numericHeight.Value;
            _config.RefreshRate = (int)numericRefreshRate.Value;
            
            DialogResult = DialogResult.OK;
            Close();
        }
    }

    private void ButtonCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private bool ValidateInput()
    {
        if (string.IsNullOrWhiteSpace(textBoxName.Text))
        {
            MessageBox.Show("Please enter a monitor name.", "Validation Error", 
                           MessageBoxButtons.OK, MessageBoxIcon.Warning);
            textBoxName.Focus();
            return false;
        }

        if (numericWidth.Value < 640 || numericHeight.Value < 480)
        {
            MessageBox.Show("Minimum resolution is 640x480.", "Validation Error", 
                           MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        return true;
    }

    public VirtualMonitorConfig GetConfiguration()
    {
        return _config;
    }
}
