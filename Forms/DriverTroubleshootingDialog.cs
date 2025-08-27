using System.Diagnostics;
using StreamVault.Services;

namespace StreamVault.Forms;

public partial class DriverTroubleshootingDialog : Form
{
    private readonly LoggingService _logger;
    private readonly VirtualDesktopDriverService _driverService;

    public DriverTroubleshootingDialog(LoggingService logger, VirtualDesktopDriverService driverService)
    {
        _logger = logger;
        _driverService = driverService;
        InitializeComponent();
        LoadSystemInformation();
    }

    private void InitializeComponent()
    {
        this.Text = "Virtual Display Driver Troubleshooting";
        this.Size = new Size(600, 500);
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;

        // Main panel
        var mainPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 4,
            Padding = new Padding(10)
        };

        // System Info Group
        var systemGroup = new GroupBox
        {
            Text = "System Information",
            Height = 120,
            Dock = DockStyle.Fill
        };

        var systemPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 4,
            Padding = new Padding(5)
        };

        systemPanel.Controls.Add(new Label { Text = "Windows Version:", AutoSize = true }, 0, 0);
        systemPanel.Controls.Add(new Label { Name = "labelWindowsVersion", Text = "Checking...", AutoSize = true }, 1, 0);

        systemPanel.Controls.Add(new Label { Text = "Administrator:", AutoSize = true }, 0, 1);
        systemPanel.Controls.Add(new Label { Name = "labelAdminStatus", Text = "Checking...", AutoSize = true }, 1, 1);

        systemPanel.Controls.Add(new Label { Text = "Test Signing:", AutoSize = true }, 0, 2);
        systemPanel.Controls.Add(new Label { Name = "labelTestSigning", Text = "Checking...", AutoSize = true }, 1, 2);

        systemPanel.Controls.Add(new Label { Text = "Driver Status:", AutoSize = true }, 0, 3);
        systemPanel.Controls.Add(new Label { Name = "labelDriverStatus", Text = "Checking...", AutoSize = true }, 1, 3);

        systemGroup.Controls.Add(systemPanel);

        // Solutions Group
        var solutionsGroup = new GroupBox
        {
            Text = "Quick Solutions",
            Height = 200,
            Dock = DockStyle.Fill
        };

        var solutionsPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 5,
            Padding = new Padding(5)
        };

        solutionsPanel.Controls.Add(new Button
        {
            Text = "Enable Test Signing (Requires Restart)",
            Name = "buttonEnableTestSigning",
            Height = 30,
            Dock = DockStyle.Fill
        }, 0, 0);

        solutionsPanel.Controls.Add(new Button
        {
            Text = "Open Device Manager",
            Name = "buttonDeviceManager",
            Height = 30,
            Dock = DockStyle.Fill
        }, 0, 1);

        solutionsPanel.Controls.Add(new Button
        {
            Text = "Check Event Viewer for Errors",
            Name = "buttonEventViewer",
            Height = 30,
            Dock = DockStyle.Fill
        }, 0, 2);

        solutionsPanel.Controls.Add(new Button
        {
            Text = "Open Troubleshooting Guide",
            Name = "buttonTroubleshooting",
            Height = 30,
            Dock = DockStyle.Fill
        }, 0, 3);

        solutionsPanel.Controls.Add(new Button
        {
            Text = "Restart as Administrator",
            Name = "buttonRestartAdmin",
            Height = 30,
            Dock = DockStyle.Fill
        }, 0, 4);

        solutionsGroup.Controls.Add(solutionsPanel);

        // Log Group
        var logGroup = new GroupBox
        {
            Text = "Recent Errors",
            Height = 120,
            Dock = DockStyle.Fill
        };

        var textBoxLog = new TextBox
        {
            Name = "textBoxLog",
            Multiline = true,
            ScrollBars = ScrollBars.Vertical,
            ReadOnly = true,
            Dock = DockStyle.Fill,
            Font = new Font("Courier New", 8)
        };
        logGroup.Controls.Add(textBoxLog);

        // Button Panel
        var buttonPanel = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.RightToLeft,
            Height = 40,
            Dock = DockStyle.Fill
        };

        buttonPanel.Controls.Add(new Button
        {
            Text = "Close",
            DialogResult = DialogResult.OK,
            Width = 75
        });

        buttonPanel.Controls.Add(new Button
        {
            Text = "Refresh",
            Name = "buttonRefresh",
            Width = 75
        });

        // Add to main panel
        mainPanel.Controls.Add(systemGroup, 0, 0);
        mainPanel.Controls.Add(solutionsGroup, 0, 1);
        mainPanel.Controls.Add(logGroup, 0, 2);
        mainPanel.Controls.Add(buttonPanel, 0, 3);

        mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 120));
        mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 200));
        mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));

        this.Controls.Add(mainPanel);

        // Wire up events
        this.Controls.Find("buttonEnableTestSigning", true).FirstOrDefault()!.Click += ButtonEnableTestSigning_Click;
        this.Controls.Find("buttonDeviceManager", true).FirstOrDefault()!.Click += ButtonDeviceManager_Click;
        this.Controls.Find("buttonEventViewer", true).FirstOrDefault()!.Click += ButtonEventViewer_Click;
        this.Controls.Find("buttonTroubleshooting", true).FirstOrDefault()!.Click += ButtonTroubleshooting_Click;
        this.Controls.Find("buttonRestartAdmin", true).FirstOrDefault()!.Click += ButtonRestartAdmin_Click;
        this.Controls.Find("buttonRefresh", true).FirstOrDefault()!.Click += ButtonRefresh_Click;
    }

    private async void LoadSystemInformation()
    {
        try
        {
            // Windows Version
            var windowsVersion = Environment.OSVersion.VersionString;
            this.Controls.Find("labelWindowsVersion", true).FirstOrDefault()!.Text = windowsVersion;

            // Administrator Status
            var isAdmin = _driverService.IsRunningAsAdministrator();
            var adminLabel = this.Controls.Find("labelAdminStatus", true).FirstOrDefault()!;
            adminLabel.Text = isAdmin ? "✅ Yes" : "❌ No";
            adminLabel.ForeColor = isAdmin ? Color.Green : Color.Red;

            // Test Signing Status
            var testSigning = await CheckTestSigningAsync();
            var testLabel = this.Controls.Find("labelTestSigning", true).FirstOrDefault()!;
            testLabel.Text = testSigning ? "✅ Enabled" : "❌ Disabled";
            testLabel.ForeColor = testSigning ? Color.Green : Color.Red;

            // Driver Status
            var driverInstalled = await _driverService.IsIddDriverInstalledAsync();
            var driverLabel = this.Controls.Find("labelDriverStatus", true).FirstOrDefault()!;
            driverLabel.Text = driverInstalled ? "✅ Installed" : "❌ Not Installed";
            driverLabel.ForeColor = driverInstalled ? Color.Green : Color.Red;

            // Load recent errors
            await LoadRecentErrors();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error loading system information: {ex.Message}", ex);
        }
    }

    private async Task<bool> CheckTestSigningAsync()
    {
        try
        {
            var process = Process.Start(new ProcessStartInfo
            {
                FileName = "bcdedit",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            });

            if (process == null) return false;

            var output = await process.StandardOutput.ReadToEndAsync();
            await process.WaitForExitAsync();

            return output.Contains("testsigning") && output.Contains("Yes");
        }
        catch
        {
            return false;
        }
    }

    private async Task LoadRecentErrors()
    {
        try
        {
            var textBox = this.Controls.Find("textBoxLog", true).FirstOrDefault() as TextBox;
            if (textBox == null) return;

            textBox.Text = "Loading recent errors...";

            // Get recent driver-related errors from event log
            var process = Process.Start(new ProcessStartInfo
            {
                FileName = "powershell",
                Arguments = "-Command \"Get-WinEvent -FilterHashtable @{LogName='System'; Level=2,3; StartTime=(Get-Date).AddHours(-1)} | Where-Object {$_.Message -like '*driver*' -or $_.Message -like '*IDD*' -or $_.Message -like '*display*'} | Select-Object -First 10 | Format-List TimeCreated, LevelDisplayName, Message\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            });

            if (process != null)
            {
                var output = await process.StandardOutput.ReadToEndAsync();
                await process.WaitForExitAsync();

                textBox.Text = string.IsNullOrEmpty(output) ? "No recent driver errors found." : output;
            }
        }
        catch (Exception ex)
        {
            var textBox = this.Controls.Find("textBoxLog", true).FirstOrDefault() as TextBox;
            if (textBox != null)
            {
                textBox.Text = $"Error loading event log: {ex.Message}";
            }
        }
    }

    private async void ButtonEnableTestSigning_Click(object? sender, EventArgs e)
    {
        try
        {
            var result = MessageBox.Show(
                "This will enable test signing mode, which is required for unsigned drivers.\n\n" +
                "A computer restart will be required after this operation.\n\n" +
                "Do you want to continue?",
                "Enable Test Signing",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "bcdedit",
                    Arguments = "/set testsigning on",
                    UseShellExecute = true,
                    Verb = "runas"
                });

                if (process != null)
                {
                    await process.WaitForExitAsync();
                    MessageBox.Show(
                        "Test signing has been enabled. Please restart your computer for the changes to take effect.",
                        "Test Signing Enabled",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error enabling test signing: {ex.Message}", "Error", 
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ButtonDeviceManager_Click(object? sender, EventArgs e)
    {
        try
        {
            Process.Start("devmgmt.msc");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error opening Device Manager: {ex.Message}", "Error", 
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ButtonEventViewer_Click(object? sender, EventArgs e)
    {
        try
        {
            Process.Start("eventvwr.msc");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error opening Event Viewer: {ex.Message}", "Error", 
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ButtonTroubleshooting_Click(object? sender, EventArgs e)
    {
        try
        {
            var troubleshootingPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "drivers", "TROUBLESHOOTING.md");
            if (File.Exists(troubleshootingPath))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = troubleshootingPath,
                    UseShellExecute = true
                });
            }
            else
            {
                MessageBox.Show("Troubleshooting guide not found.", "File Not Found", 
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error opening troubleshooting guide: {ex.Message}", "Error", 
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ButtonRestartAdmin_Click(object? sender, EventArgs e)
    {
        try
        {
            var exePath = Process.GetCurrentProcess().MainModule?.FileName;
            if (exePath != null)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = exePath,
                    UseShellExecute = true,
                    Verb = "runas"
                });
                Application.Exit();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error restarting as administrator: {ex.Message}", "Error", 
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ButtonRefresh_Click(object? sender, EventArgs e)
    {
        LoadSystemInformation();
    }
}
