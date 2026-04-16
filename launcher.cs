using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using VSCodePortableCommon;

namespace VSCodePortableLauncher
{
    partial class UpgradeForm : Form
    {
    [System.Runtime.InteropServices.DllImport("user32.dll")]
    static extern int ReleaseCapture();
    [System.Runtime.InteropServices.DllImport("user32.dll")]
    static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
    private const int WM_NCLBUTTONDOWN = 0xA1;
    private const int HTCAPTION = 0x2;
    private const int WS_BORDER = 0x00800000;

        // Constants
        private const int FORM_WIDTH = 800;
        private const int FORM_HEIGHT = 720;
        private const int TABLE_HEIGHT = 270;
        private const int LOG_HEIGHT = 230;

        // VSCode extensions (must match installer)
        public static readonly string[] VSCODE_EXTENSIONS = new string[]
        {
            "teabyii.ayu",
            "zhuangtongfa.material-theme",
            "ms-python.python",
            "ms-toolsai.jupyter",
            "ms-vscode-remote.remote-ssh",
            "KevinRose.vsc-python-indent",
            "usernamehw.errorlens",
            "Gerrnperl.outline-map"
        };

        // Win32 API for extracting icon from exe
        [System.Runtime.InteropServices.DllImport("shell32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern IntPtr ExtractIcon(IntPtr hInst, string lpszExeFileName, int nIconIndex);
        private Button upgradeButton;
        private Button cancelButton;

        private string pvsDir;
        private string pvsInfo;

        private Dictionary<string, string> versions;
        private bool upgradeCompleted;
        private bool upgradeStarted;
        private Stopwatch upgradeTimer;
        private DateTime upgradeStartTime;
        private List<string> pendingUpgrades;
        private System.Windows.Forms.Timer elapsedTimer;

        public UpgradeForm(string installDir, List<string> upgrades)
        {
            pvsDir = installDir;
            pvsInfo = Path.Combine(pvsDir, "pvs.info");

            upgradeCompleted = false;
            upgradeStarted = false;
            versions = new Dictionary<string, string>();
            pendingUpgrades = upgrades;

            // Optimize network performance
            System.Net.ServicePointManager.DefaultConnectionLimit = 100;
            System.Net.ServicePointManager.Expect100Continue = false;

            InitializeComponent();

            // Set form icon (extract from launcher.exe's win32 icon resource)
            try
            {
                string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                IntPtr hIcon = ExtractIcon(IntPtr.Zero, exePath, 0);
                if (hIcon != IntPtr.Zero)
                {
                    Icon = Icon.FromHandle(hIcon);
                }
            }
            catch { /* Ignore icon errors */ }

            headerPanel.MouseDown += HeaderPanel_MouseDown;
            headerTitleLabel.MouseDown += HeaderPanel_MouseDown;
            headerSubLabel.MouseDown += HeaderPanel_MouseDown;
            
            // Initialize table with pending upgrades
            InitializePendingUpgradesTable();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= WS_BORDER;
                return cp;
            }
        }

        private void HeaderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void InitializePendingUpgradesTable()
        {
            var componentMap = new Dictionary<string, string>
            {
                { "upgrade_pwsh7", "PowerShell 7" },
                { "upgrade_ohmyposh", "Oh My Posh" },
                { "upgrade_term_icons", "Terminal-Icons" },
                { "upgrade_psfzf", "PSFzf" },
                { "upgrade_modern_unix", "modern-unix-win" },
                { "upgrade_vscode", "VSCode" }
            };

            var componentsToShow = pendingUpgrades
                .Where(flag => componentMap.ContainsKey(flag))
                .Select(flag => componentMap[flag])
                .ToList();

            InitializeUpgradeTable(componentsToShow);
        }

        private void UpgradeButton_Click(object sender, EventArgs e)
        {
            if (upgradeStarted)
            {
                // Upgrade completed, close form
                Close();
            }
            else
            {
                // Start upgrade
                upgradeStarted = true;
                upgradeButton.Enabled = false;
                upgradeButton.Text = "Exit";
                upgradeButton.BackColor = Color.FromArgb(200, 200, 200);
                upgradeButton.ForeColor = Color.White;
                upgradeButton.FlatStyle = FlatStyle.Flat;
                upgradeButton.FlatAppearance.BorderColor = Color.FromArgb(150, 150, 150);
                upgradeButton.Cursor = Cursors.WaitCursor;
                cancelButton.Enabled = false;

                Task.Run(async () =>
                {
                    await PerformUpgrade(pendingUpgrades);
                });
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (!upgradeStarted)
            {
                // Just close form, VSCode will be launched by RunLauncher
                Close();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (upgradeStarted && !upgradeCompleted && e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                MessageBox.Show("Please wait until the upgrade process is complete.",
                    "Upgrade in Progress",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            base.OnFormClosing(e);
        }

        private async Task PerformUpgrade(List<string> pendingUpgrades)
        {
            try
            {
                CommonHelper.EnableTls12();

                if (!File.Exists(pvsInfo))
                {
                    ShowError("pvs.info file not found.");
                    return;
                }

                // Terminate Code.exe if pwsh7 or vscode upgrade is needed
                bool hasPwsh7Upgrade = pendingUpgrades.Contains("upgrade_pwsh7");
                bool hasModuleUpgrade = pendingUpgrades.Any(f => f == "upgrade_ohmyposh" || f == "upgrade_term_icons" || f == "upgrade_psfzf" || f == "upgrade_modern_unix");
                bool hasVSCodeUpgrade = pendingUpgrades.Contains("upgrade_vscode");

                if (hasPwsh7Upgrade || hasVSCodeUpgrade)
                {
                    LogActivity("Checking for running Code.exe processes...");
                    string codeExePath = Path.Combine(pvsDir, "Code.exe");

                    var codeProcesses = Process.GetProcesses()
                        .Where(p =>
                        {
                            try
                            {
                                if (p.MainModule != null && p.MainModule.FileName != null)
                                    return p.MainModule.FileName.Equals(codeExePath, StringComparison.OrdinalIgnoreCase);
                                return false;
                            }
                            catch { return false; }
                        })
                        .ToList();

                    if (codeProcesses.Any())
                    {
                        LogActivity("Found " + codeProcesses.Count + " Code.exe process(es). Terminating...");
                        foreach (var proc in codeProcesses)
                        {
                            try
                            {
                                proc.Kill();
                                proc.WaitForExit(5000);
                            }
                            catch { }
                        }
                        await Task.Delay(1000); // Wait for cleanup
                        LogActivity("Code.exe processes terminated.");
                    }
                }

                versions = ReadVersionsFromInfo();

                upgradeTimer = Stopwatch.StartNew();
                upgradeStartTime = DateTime.Now;
                LogActivity("=== Upgrade started at " + upgradeStartTime.ToString("HH:mm:ss") + " ===");

                // Start elapsed time timer (update every second)
                StartElapsedTimer();

                var allResults = new List<KeyValuePair<string, string>>();

                // Phase 1: If pwsh7 needs upgrade AND modules need upgrade, do pwsh7 first
                if (hasPwsh7Upgrade && hasModuleUpgrade)
                {
                    LogActivity("PowerShell 7: Upgrading first (required for modules)...");

                    // Run pwsh7 and vscode in parallel if both need upgrade
                    if (hasVSCodeUpgrade)
                    {
                        var phase1Tasks = new List<Task<KeyValuePair<string, string>>>
                        {
                            UpgradePowerShell7Async(),
                            UpgradeVSCodeAsync()
                        };
                        var phase1Results = await Task.WhenAll(phase1Tasks);
                        allResults.AddRange(phase1Results.Where(r => !string.IsNullOrEmpty(r.Value)));
                    }
                    else
                    {
                        var pwsh7Result = await UpgradePowerShell7Async();
                        if (!string.IsNullOrEmpty(pwsh7Result.Value))
                            allResults.Add(pwsh7Result);
                    }

                    // Wait for PowerShell to be ready (optimized polling)
                    string pwshExe = Path.Combine(pvsDir, "data", "lib", "pwsh", "pwsh.exe");
                    int retries = 0;
                    while (!File.Exists(pwshExe) && retries < 20)
                    {
                        await Task.Delay(200); // Check every 200ms
                        retries++;
                    }
                }

                // Phase 2: Run remaining upgrades in parallel
                var parallelTasks = new List<Task<KeyValuePair<string, string>>>();

                // Add pwsh7 if it needs upgrade but no modules need upgrade
                if (hasPwsh7Upgrade && !hasModuleUpgrade)
                    parallelTasks.Add(UpgradePowerShell7Async());

                if (pendingUpgrades.Contains("upgrade_ohmyposh"))
                    parallelTasks.Add(UpgradeOhMyPoshAsync());

                if (pendingUpgrades.Contains("upgrade_term_icons"))
                    parallelTasks.Add(UpgradeModuleAsync("Terminal-Icons", "TERMINAL_ICONS_VERSION"));

                if (pendingUpgrades.Contains("upgrade_psfzf"))
                    parallelTasks.Add(UpgradeModuleAsync("PSFzf", "PSFZF_VERSION"));

                if (pendingUpgrades.Contains("upgrade_modern_unix"))
                    parallelTasks.Add(UpgradeModuleAsync("modern-unix-win", "MODERN_UNIX_WIN_VERSION"));

                // Add vscode if not already upgraded in phase 1
                if (hasVSCodeUpgrade && !(hasPwsh7Upgrade && hasModuleUpgrade))
                    parallelTasks.Add(UpgradeVSCodeAsync());

                if (parallelTasks.Any())
                {
                    var parallelResults = await Task.WhenAll(parallelTasks);
                    allResults.AddRange(parallelResults.Where(r => !string.IsNullOrEmpty(r.Value)));
                }

                UpdateVersionsInInfo(allResults);
                DeleteUpgradeFlags();

                // Save activity log to upgrade.log
                SaveUpgradeLog();

                LogActivity("Upgrade completed successfully!");

                if (upgradeTimer != null)
                {
                    upgradeTimer.Stop();
                    var elapsed = upgradeTimer.Elapsed;
                    string elapsedStr = string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}",
                        elapsed.Hours, elapsed.Minutes, elapsed.Seconds, elapsed.Milliseconds);
                    LogActivity("=== Upgrade completed at " + DateTime.Now.ToString("HH:mm:ss") + " ===");
                    LogActivity("*** Total upgrade time: " + elapsedStr + " ***");
                }

                // Stop elapsed timer
                StopElapsedTimer();

                upgradeCompleted = true;
                Invoke(new Action(() =>
                {
                    upgradeButton.Enabled = true;
                    upgradeButton.BackColor = Color.FromArgb(0, 120, 215);
                    upgradeButton.ForeColor = Color.White;
                    upgradeButton.FlatStyle = FlatStyle.Standard;
                    upgradeButton.Cursor = Cursors.Default;
                }));
            }
            catch (Exception ex)
            {
                ShowError("Upgrade error: " + ex.Message);
                LogActivity("Upgrade error: " + ex.Message);
                
                // Stop elapsed timer
                StopElapsedTimer();
                
                upgradeCompleted = true;
                Invoke(new Action(() =>
                {
                    upgradeButton.Enabled = true;
                    upgradeButton.BackColor = Color.FromArgb(0, 120, 215);
                    upgradeButton.ForeColor = Color.White;
                    upgradeButton.FlatStyle = FlatStyle.Standard;
                    upgradeButton.Cursor = Cursors.Default;
                }));
            }
        }

        private void InitializeUpgradeTable(List<string> components)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => InitializeUpgradeTable(components)));
                return;
            }

            // Enable only components that need upgrade
            var componentMap = new Dictionary<string, Tuple<Label, Label, ProgressBar>>
            {
                { "PowerShell 7", Tuple.Create(pwsh7NameLabel, pwsh7StatusLabel, pwsh7ProgressBar) },
                { "Oh My Posh", Tuple.Create(ohmyposhNameLabel, ohmyposhStatusLabel, ohmyposhProgressBar) },
                { "Terminal-Icons", Tuple.Create(termIconsNameLabel, termIconsStatusLabel, termIconsProgressBar) },
                { "PSFzf", Tuple.Create(psfzfNameLabel, psfzfStatusLabel, psfzfProgressBar) },
                { "modern-unix-win", Tuple.Create(modernUnixNameLabel, modernUnixStatusLabel, modernUnixProgressBar) },
                { "VSCode", Tuple.Create(vscodeNameLabel, vscodeStatusLabel, vscodeProgressBar) }
            };

            foreach (var kvp in componentMap)
            {
                bool shouldEnable = components.Contains(kvp.Key);
                kvp.Value.Item1.Enabled = shouldEnable; // Name label
                kvp.Value.Item2.Enabled = shouldEnable; // Status label
                kvp.Value.Item3.Enabled = shouldEnable; // Progress bar
                
                if (shouldEnable)
                {
                    // Keep original component name for enabled rows
                    kvp.Value.Item1.Text = kvp.Key;
                    kvp.Value.Item2.Text = "Waiting";
                    kvp.Value.Item2.ForeColor = Color.Gray;
                    kvp.Value.Item3.Value = 0;
                }
                else
                {
                    // Replace component name with "-" for disabled rows
                    kvp.Value.Item1.Text = "-";
                }
            }
        }

        private void LogActivity(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => LogActivity(message)));
                return;
            }

            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            activityLog.AppendText(string.Format("[{0}] {1}\r\n", timestamp, message));
            activityLog.SelectionStart = activityLog.Text.Length;
            activityLog.ScrollToCaret();
        }

        private void UpdateComponentStatus(string componentName, string status, int progress)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateComponentStatus(componentName, status, progress)));
                return;
            }

            // Find and update the corresponding controls
            Label statusLabel = null;
            ProgressBar progressBar = null;

            switch (componentName)
            {
                case "PowerShell 7":
                    statusLabel = pwsh7StatusLabel;
                    progressBar = pwsh7ProgressBar;
                    break;
                case "Oh My Posh":
                    statusLabel = ohmyposhStatusLabel;
                    progressBar = ohmyposhProgressBar;
                    break;
                case "Terminal-Icons":
                    statusLabel = termIconsStatusLabel;
                    progressBar = termIconsProgressBar;
                    break;
                case "PSFzf":
                    statusLabel = psfzfStatusLabel;
                    progressBar = psfzfProgressBar;
                    break;
                case "modern-unix-win":
                    statusLabel = modernUnixStatusLabel;
                    progressBar = modernUnixProgressBar;
                    break;
                case "VSCode":
                    statusLabel = vscodeStatusLabel;
                    progressBar = vscodeProgressBar;
                    break;
                default:
                    return;
            }

            if (statusLabel != null && progressBar != null)
            {
                statusLabel.Text = status;
                progressBar.Value = Math.Min(progress, 100);

                switch (status)
                {
                    case "Waiting":
                        statusLabel.ForeColor = Color.Gray;
                        break;
                    case "Downloading":
                        statusLabel.ForeColor = Color.Blue;
                        break;
                    case "Installing":
                        statusLabel.ForeColor = Color.DarkOrange;
                        break;
                    case "Completed":
                        statusLabel.ForeColor = Color.Green;
                        break;
                    default:
                        statusLabel.ForeColor = Color.Red;
                        break;
                }
            }
        }

        private void StartElapsedTimer()
        {
            if (elapsedTimer != null)
            {
                elapsedTimer.Stop();
                elapsedTimer.Dispose();
            }

            elapsedTimer = new System.Windows.Forms.Timer();
            elapsedTimer.Interval = 1000; // Update every 1 second
            elapsedTimer.Tick += (s, e) => UpdateElapsedTimeDisplay();
            elapsedTimer.Start();
        }

        private void StopElapsedTimer()
        {
            if (elapsedTimer != null)
            {
                elapsedTimer.Stop();
                elapsedTimer.Dispose();
                elapsedTimer = null;
            }
        }

        private void UpdateElapsedTimeDisplay()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateElapsedTimeDisplay));
                return;
            }

            if (upgradeTimer != null && upgradeTimer.IsRunning && elapsedTimeLabel != null)
            {
                var elapsed = upgradeTimer.Elapsed;
                string elapsedStr = string.Format("{0:D2}:{1:D2}:{2:D2}",
                    elapsed.Hours, elapsed.Minutes, elapsed.Seconds);
                elapsedTimeLabel.Text = "Elapsed: " + elapsedStr;
                elapsedTimeLabel.Visible = true;
            }
        }

        private async Task<KeyValuePair<string, string>> UpgradePowerShell7Async()
        {
            const string component = "PowerShell 7";
            try
            {
                string tempDir = Path.Combine(Path.GetTempPath(), "vscode-pvs-upgrade");

                var installer = new PowerShellInstaller(
                    pvsDir,
                    (name, status, progress) => UpdateComponentStatus(component, ConvertStatus(status), progress),
                    (message) => LogActivity(message)
                );

                var result = await installer.InstallAsync(tempDir, true);

                if (!string.IsNullOrEmpty(result.Value))
                {
                    UpdateComponentStatus(component, "Completed", 100);
                    LogActivity("PowerShell 7: Upgrade completed - version " + result.Value);
                    return result;
                }
                else
                {
                    throw new Exception("PowerShell 7 installation returned empty version");
                }
            }
            catch (Exception ex)
            {
                UpdateComponentStatus(component, "Error", 0);
                LogActivity("PowerShell 7: Error - " + ex.Message);
                return new KeyValuePair<string, string>();
            }
        }

        private async Task<KeyValuePair<string, string>> UpgradeOhMyPoshAsync()
        {
            const string component = "Oh My Posh";
            try
            {
                string tempDir = Path.Combine(Path.GetTempPath(), "vscode-pvs-upgrade");

                var installer = new OhMyPoshInstaller(
                    pvsDir,
                    (name, status, progress) => UpdateComponentStatus(component, ConvertStatus(status), progress),
                    (message) => LogActivity(message)
                );

                var result = await installer.InstallAsync(tempDir);

                if (!string.IsNullOrEmpty(result.Value))
                {
                    UpdateComponentStatus(component, "Completed", 100);
                    LogActivity("Oh My Posh: Upgrade completed - version " + result.Value);
                    return result;
                }
                else
                {
                    throw new Exception("Oh My Posh installation returned empty version");
                }
            }
            catch (Exception ex)
            {
                UpdateComponentStatus(component, "Error", 0);
                LogActivity("Oh My Posh: Error - " + ex.Message);
                return new KeyValuePair<string, string>();
            }
        }

        private async Task<KeyValuePair<string, string>> UpgradeModuleAsync(string moduleName, string versionKey)
        {
            try
            {
                var installer = new ModuleInstaller(
                    pvsDir,
                    (name, status, progress) => UpdateComponentStatus(moduleName, ConvertStatus(status), progress),
                    (message) => LogActivity(message)
                );

                var result = await installer.InstallModuleAsync(moduleName, versionKey);

                if (!string.IsNullOrEmpty(result.Value))
                {
                    UpdateComponentStatus(moduleName, "Completed", 100);
                    LogActivity(moduleName + ": Upgrade completed - version " + result.Value);
                    return result;
                }
                else
                {
                    throw new Exception(moduleName + " installation returned empty version");
                }
            }
            catch (Exception ex)
            {
                UpdateComponentStatus(moduleName, "Error", 0);
                LogActivity(moduleName + ": Error - " + ex.Message);
                return new KeyValuePair<string, string>();
            }
        }

        private async Task<KeyValuePair<string, string>> UpgradeVSCodeAsync()
        {
            const string component = "VSCode";
            try
            {
                var installer = new VSCodeInstaller(
                    pvsDir,
                    (name, status, progress) => UpdateComponentStatus(component, ConvertStatus(status), progress),
                    (message) => LogActivity(message)
                );

                var result = await installer.UpgradeAsync(Path.Combine(Path.GetTempPath(), "vscode-pvs-upgrade"));

                if (!string.IsNullOrEmpty(result.Value))
                {
                    UpdateComponentStatus(component, "Completed", 100);
                    LogActivity("VSCode: Upgrade completed - version " + result.Value);
                    return result;
                }
                else
                {
                    throw new Exception("VSCode installation returned empty version");
                }
            }
            catch (Exception ex)
            {
                UpdateComponentStatus(component, "Error", 0);
                LogActivity("VSCode: Error - " + ex.Message);
                return new KeyValuePair<string, string>();
            }
        }

        private string ConvertStatus(string status)
        {
            if (status.Contains("Checking") || status.Contains("Requesting"))
                return "Downloading";
            else if (status.Contains("Downloading") || status.Contains("download"))
                return "Downloading";
            else if (status.Contains("Extracting") || status.Contains("extract") ||
                     status.Contains("Installing") || status.Contains("Configuring") ||
                     status.Contains("Creating") || status.Contains("Running") ||
                     status.Contains("Verifying"))
                return "Installing";
            else if (status.StartsWith("✓") || status.Contains("Completed"))
                return "Completed";
            else if (status.StartsWith("⚠") || status.Contains("Error"))
                return "Error";
            else if (status.Contains("Waiting"))
                return "Waiting";
            return status;
        }

        private void DeleteUpgradeFlags()
        {
            var flagFiles = new[]
            {
                Path.Combine(pvsDir, "upgrade_pwsh7"),
                Path.Combine(pvsDir, "upgrade_ohmyposh"),
                Path.Combine(pvsDir, "upgrade_term_icons"),
                Path.Combine(pvsDir, "upgrade_psfzf"),
                Path.Combine(pvsDir, "upgrade_modern_unix"),
                Path.Combine(pvsDir, "upgrade_vscode")
            };

            foreach (var flag in flagFiles)
            {
                if (File.Exists(flag))
                {
                    File.Delete(flag);
                }
            }
        }

        private Dictionary<string, string> ReadVersionsFromInfo()
        {
            var result = new Dictionary<string, string>();
            foreach (var line in File.ReadAllLines(pvsInfo))
            {
                int idx = line.IndexOf('=');
                if (idx > 0)
                {
                    result[line.Substring(0, idx)] = line.Substring(idx + 1);
                }
            }
            return result;
        }

        private void UpdateVersionsInInfo(List<KeyValuePair<string, string>> updates)
        {
            var lines = File.ReadAllLines(pvsInfo).ToList();

            foreach (var update in updates)
            {
                bool found = false;
                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i].StartsWith(update.Key + "="))
                    {
                        lines[i] = update.Key + "=" + update.Value;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    lines.Add(update.Key + "=" + update.Value);
                }
            }

            File.WriteAllLines(pvsInfo, lines);
        }

        private void ShowError(string message)
        {
            Invoke(new Action(() =>
            {
                MessageBox.Show(this, message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }));
        }

        private void SaveUpgradeLog()
        {
            try
            {
                string logPath = Path.Combine(pvsDir, "pvs.log");
                string logHeader = "\r\n" + new string('=', 80) + "\r\n";
                logHeader += "Upgrade Session: " + upgradeStartTime.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n";
                logHeader += new string('=', 80) + "\r\n";

                // Append to existing log file or create new one
                File.AppendAllText(logPath, logHeader + activityLog.Text + "\r\n", Encoding.UTF8);
                LogActivity("Upgrade log saved to: " + logPath);
            }
            catch (Exception ex)
            {
                LogActivity("Failed to save upgrade log: " + ex.Message);
            }
        }
    }
}
