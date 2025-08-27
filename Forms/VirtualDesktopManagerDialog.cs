using System.Diagnostics;
using StreamVault.Models;
using StreamVault.Services;
using static StreamVault.Services.VirtualDesktopDriverService;

namespace StreamVault.Forms;

public partial class VirtualDesktopManagerDialog : Form
{
    private readonly LoggingService _logger;
    private readonly VirtualDesktopDriverService _driverService;
    private List<VirtualDesktopInfo> _virtualDesktops;
    private DriverStatus? _driverStatus;

    public List<VirtualDesktopInfo> VirtualDesktops => _virtualDesktops;

    public VirtualDesktopManagerDialog(LoggingService logger)
    {
        _logger = logger;
        _driverService = new VirtualDesktopDriverService(_logger);
        _virtualDesktops = new List<VirtualDesktopInfo>();
        
        InitializeComponent();
        LoadDriverStatus();
        LoadVirtualDesktops();
    }

    private async void LoadDriverStatus()
    {
        try
        {
            buttonInstallDriver.Enabled = false;
            buttonInstallDriver.Text = "Checking...";
            
            _driverStatus = await _driverService.GetDriverStatusAsync();
            
            // Update UI based on status
            UpdateDriverStatusUI();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error loading driver status: {ex.Message}", ex);
            labelDriverStatus.Text = "Error checking driver status";
            labelDriverStatus.ForeColor = Color.Red;
        }
    }

    private void UpdateDriverStatusUI()
    {
        if (_driverStatus == null) return;

        // Administrator status
        if (_driverStatus.IsAdministrator)
        {
            labelAdminStatus.Text = "✅ Running as Administrator";
            labelAdminStatus.ForeColor = Color.Green;
        }
        else
        {
            labelAdminStatus.Text = "⚠️ Not running as Administrator";
            labelAdminStatus.ForeColor = Color.Orange;
        }

        // Driver status
        if (_driverStatus.IddDriverInstalled)
        {
            labelDriverStatus.Text = "✅ IDD Virtual Display Driver Installed";
            labelDriverStatus.ForeColor = Color.Green;
            buttonInstallDriver.Text = "Reinstall Driver";
            buttonCreateDesktop.Enabled = true;
        }
        else
        {
            labelDriverStatus.Text = "❌ Virtual Display Driver Not Installed";
            labelDriverStatus.ForeColor = Color.Red;
            buttonInstallDriver.Text = "Install Driver";
            buttonCreateDesktop.Enabled = false;
        }

        buttonInstallDriver.Enabled = true;
        
        // Drivers path
        labelDriversPath.Text = $"Drivers: {_driverStatus.DriversPath}";
    }

    private async void LoadVirtualDesktops()
    {
        try
        {
            _virtualDesktops = await _driverService.GetVirtualDesktopsAsync();
            
            listBoxDesktops.Items.Clear();
            foreach (var desktop in _virtualDesktops)
            {
                listBoxDesktops.Items.Add(desktop);
            }
            
            UpdateButtons();
            labelDesktopCount.Text = $"Virtual Desktops: {_virtualDesktops.Count}";
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error loading virtual desktops: {ex.Message}", ex);
        }
    }

    private void UpdateButtons()
    {
        var hasSelection = listBoxDesktops.SelectedIndex >= 0;
        buttonRemoveDesktop.Enabled = hasSelection;
        buttonTestDesktop.Enabled = hasSelection;
    }

    private async void ButtonInstallDriver_Click(object sender, EventArgs e)
    {
        try
        {
            if (!_driverStatus?.IsAdministrator == true)
            {
                var result = MessageBox.Show(
                    "Installing virtual display drivers requires administrator privileges.\n\n" +
                    "Click OK to restart the application as administrator, or Cancel to continue without driver installation.",
                    "Administrator Required",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.OK)
                {
                    RestartAsAdministrator();
                    return;
                }
                else
                {
                    return;
                }
            }

            buttonInstallDriver.Enabled = false;
            buttonInstallDriver.Text = "Installing...";

            var success = await _driverService.InstallIddDriverAsync();
            
            if (success)
            {
                MessageBox.Show(
                    "Virtual display driver installed successfully!\n\n" +
                    "You can now create virtual desktops for streaming.",
                    "Installation Complete",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                
                LoadDriverStatus();
            }
            else
            {
                MessageBox.Show(
                    "Driver installation failed. Please check the logs for more details.\n\n" +
                    "Make sure you're running as administrator and try again.",
                    "Installation Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                
                buttonInstallDriver.Enabled = true;
                buttonInstallDriver.Text = "Install Driver";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error installing driver: {ex.Message}", ex);
            MessageBox.Show($"Error during installation: {ex.Message}", "Error", 
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
            
            buttonInstallDriver.Enabled = true;
            buttonInstallDriver.Text = "Install Driver";
        }
    }

    private void RestartAsAdministrator()
    {
        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = Application.ExecutablePath,
                UseShellExecute = true,
                Verb = "runas" // Run as administrator
            };

            Process.Start(startInfo);
            Application.Exit();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error restarting as administrator: {ex.Message}", ex);
            MessageBox.Show("Could not restart as administrator. Please manually run the application as administrator.", 
                           "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void ButtonCreateDesktop_Click(object sender, EventArgs e)
    {
        try
        {
            var width = (int)numericWidth.Value;
            var height = (int)numericHeight.Value;
            var name = textBoxDesktopName.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                name = $"Virtual Desktop {_virtualDesktops.Count + 1}";
            }

            buttonCreateDesktop.Enabled = false;
            buttonCreateDesktop.Text = "Creating...";

            var desktop = await _driverService.CreateVirtualDesktopAsync(width, height, name);
            
            if (desktop != null)
            {
                _virtualDesktops.Add(desktop);
                LoadVirtualDesktops();
                
                // Select the new desktop
                listBoxDesktops.SelectedItem = desktop;
                
                MessageBox.Show($"Virtual desktop '{desktop.Name}' created successfully!", 
                               "Desktop Created", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Clear form
                textBoxDesktopName.Clear();
            }
            else
            {
                MessageBox.Show("Failed to create virtual desktop. Check the logs for details.", 
                               "Creation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating virtual desktop: {ex.Message}", ex);
            MessageBox.Show($"Error creating desktop: {ex.Message}", "Error", 
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            buttonCreateDesktop.Enabled = true;
            buttonCreateDesktop.Text = "Create Desktop";
        }
    }

    private async void ButtonRemoveDesktop_Click(object sender, EventArgs e)
    {
        if (listBoxDesktops.SelectedItem is not VirtualDesktopInfo selectedDesktop) return;

        var result = MessageBox.Show(
            $"Are you sure you want to remove the virtual desktop '{selectedDesktop.Name}'?",
            "Confirm Removal",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            try
            {
                var success = await _driverService.RemoveVirtualDesktopAsync(selectedDesktop.Id);
                
                if (success)
                {
                    _virtualDesktops.Remove(selectedDesktop);
                    LoadVirtualDesktops();
                    
                    MessageBox.Show("Virtual desktop removed successfully!", 
                                   "Desktop Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to remove virtual desktop.", 
                                   "Removal Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error removing virtual desktop: {ex.Message}", ex);
                MessageBox.Show($"Error removing desktop: {ex.Message}", "Error", 
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void ButtonTestDesktop_Click(object sender, EventArgs e)
    {
        if (listBoxDesktops.SelectedItem is not VirtualDesktopInfo selectedDesktop) return;

        try
        {
            // Test the virtual desktop by trying to capture it
            MessageBox.Show(
                $"Virtual Desktop: {selectedDesktop.DisplayInfo}\n" +
                $"ID: {selectedDesktop.Id}\n" +
                $"Driver: {selectedDesktop.DriverType}\n" +
                $"Created: {selectedDesktop.CreatedDate:yyyy-MM-dd HH:mm:ss}\n" +
                $"Status: {(selectedDesktop.IsActive ? "Active" : "Inactive")}",
                "Desktop Information",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error testing virtual desktop: {ex.Message}", ex);
            MessageBox.Show($"Error testing desktop: {ex.Message}", "Error", 
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ListBoxDesktops_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateButtons();
        
        if (listBoxDesktops.SelectedItem is VirtualDesktopInfo selectedDesktop)
        {
            // Update info panel
            labelSelectedInfo.Text = $"Selected: {selectedDesktop.DisplayInfo}";
        }
        else
        {
            labelSelectedInfo.Text = "No desktop selected";
        }
    }

    private void ButtonClose_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.OK;
        Close();
    }

    private void ButtonRefresh_Click(object sender, EventArgs e)
    {
        LoadDriverStatus();
        LoadVirtualDesktops();
    }
}
