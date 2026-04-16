namespace VSCodePortableLauncher
{
    partial class UpgradeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            
            // Clean up elapsed timer
            if (elapsedTimer != null)
            {
                elapsedTimer.Stop();
                elapsedTimer.Dispose();
                elapsedTimer = null;
            }
            
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.headerPanel = new System.Windows.Forms.Panel();
            this.headerTitleLabel = new System.Windows.Forms.Label();
            this.headerSubLabel = new System.Windows.Forms.Label();
            this.upgradeButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.elapsedTimeLabel = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.componentTable = new System.Windows.Forms.TableLayoutPanel();
            this.pwsh7NameLabel = new System.Windows.Forms.Label();
            this.pwsh7StatusLabel = new System.Windows.Forms.Label();
            this.pwsh7ProgressBar = new System.Windows.Forms.ProgressBar();
            this.ohmyposhNameLabel = new System.Windows.Forms.Label();
            this.ohmyposhStatusLabel = new System.Windows.Forms.Label();
            this.ohmyposhProgressBar = new System.Windows.Forms.ProgressBar();
            this.termIconsNameLabel = new System.Windows.Forms.Label();
            this.termIconsStatusLabel = new System.Windows.Forms.Label();
            this.termIconsProgressBar = new System.Windows.Forms.ProgressBar();
            this.psfzfNameLabel = new System.Windows.Forms.Label();
            this.psfzfStatusLabel = new System.Windows.Forms.Label();
            this.psfzfProgressBar = new System.Windows.Forms.ProgressBar();
            this.modernUnixNameLabel = new System.Windows.Forms.Label();
            this.modernUnixStatusLabel = new System.Windows.Forms.Label();
            this.modernUnixProgressBar = new System.Windows.Forms.ProgressBar();
            this.vscodeNameLabel = new System.Windows.Forms.Label();
            this.vscodeStatusLabel = new System.Windows.Forms.Label();
            this.vscodeProgressBar = new System.Windows.Forms.ProgressBar();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.activityLog = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.componentTable.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // upgradeButton
            // 
            this.upgradeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(212)))));
            this.upgradeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.upgradeButton.FlatAppearance.BorderSize = 0;
            this.upgradeButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(16, 110, 190);
            this.upgradeButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(0, 90, 158);
            this.upgradeButton.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.upgradeButton.ForeColor = System.Drawing.Color.White;
            this.upgradeButton.Location = new System.Drawing.Point(478, 328);
            this.upgradeButton.Name = "upgradeButton";
            this.upgradeButton.Size = new System.Drawing.Size(110, 34);
            this.upgradeButton.TabIndex = 6;
            this.upgradeButton.Text = "Upgrade";
            this.upgradeButton.UseVisualStyleBackColor = false;
            this.upgradeButton.Click += new System.EventHandler(this.UpgradeButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.White;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(210, 210, 210);
            this.cancelButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            this.cancelButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(225, 225, 225);
            this.cancelButton.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cancelButton.ForeColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.cancelButton.Location = new System.Drawing.Point(594, 328);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(100, 34);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tabControl1.Location = new System.Drawing.Point(8, 59);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(681, 262);
            this.tabControl1.TabIndex = 14;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.elapsedTimeLabel);
            this.tabPage1.Controls.Add(this.statusLabel);
            this.tabPage1.Controls.Add(this.componentTable);
            this.tabPage1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(673, 234);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Available";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // elapsedTimeLabel
            // 
            this.elapsedTimeLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.elapsedTimeLabel.ForeColor = System.Drawing.Color.Gray;
            this.elapsedTimeLabel.Location = new System.Drawing.Point(552, 208);
            this.elapsedTimeLabel.Name = "elapsedTimeLabel";
            this.elapsedTimeLabel.Size = new System.Drawing.Size(109, 20);
            this.elapsedTimeLabel.TabIndex = 15;
            this.elapsedTimeLabel.Text = "Elapsed: 00:00:00";
            this.elapsedTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.elapsedTimeLabel.Visible = false;
            // 
            // statusLabel
            // 
            this.statusLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.statusLabel.ForeColor = System.Drawing.Color.Gray;
            this.statusLabel.Location = new System.Drawing.Point(11, 208);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(521, 20);
            this.statusLabel.TabIndex = 14;
            // 
            // componentTable
            // 
            this.componentTable.ColumnCount = 3;
            this.componentTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.30435F));
            this.componentTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.22018F));
            this.componentTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 69.57187F));
            this.componentTable.Controls.Add(this.pwsh7NameLabel, 0, 0);
            this.componentTable.Controls.Add(this.pwsh7StatusLabel, 1, 0);
            this.componentTable.Controls.Add(this.pwsh7ProgressBar, 2, 0);
            this.componentTable.Controls.Add(this.ohmyposhNameLabel, 0, 1);
            this.componentTable.Controls.Add(this.ohmyposhStatusLabel, 1, 1);
            this.componentTable.Controls.Add(this.ohmyposhProgressBar, 2, 1);
            this.componentTable.Controls.Add(this.termIconsNameLabel, 0, 2);
            this.componentTable.Controls.Add(this.termIconsStatusLabel, 1, 2);
            this.componentTable.Controls.Add(this.termIconsProgressBar, 2, 2);
            this.componentTable.Controls.Add(this.psfzfNameLabel, 0, 3);
            this.componentTable.Controls.Add(this.psfzfStatusLabel, 1, 3);
            this.componentTable.Controls.Add(this.psfzfProgressBar, 2, 3);
            this.componentTable.Controls.Add(this.modernUnixNameLabel, 0, 4);
            this.componentTable.Controls.Add(this.modernUnixStatusLabel, 1, 4);
            this.componentTable.Controls.Add(this.modernUnixProgressBar, 2, 4);
            this.componentTable.Controls.Add(this.vscodeNameLabel, 0, 5);
            this.componentTable.Controls.Add(this.vscodeStatusLabel, 1, 5);
            this.componentTable.Controls.Add(this.vscodeProgressBar, 2, 5);
            this.componentTable.Location = new System.Drawing.Point(8, 12);
            this.componentTable.Name = "componentTable";
            this.componentTable.RowCount = 6;
            this.componentTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.componentTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.componentTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.componentTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.componentTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.componentTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.componentTable.Size = new System.Drawing.Size(654, 188);
            this.componentTable.TabIndex = 12;
            // 
            // pwsh7NameLabel
            // 
            this.pwsh7NameLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.pwsh7NameLabel.AutoSize = true;
            this.pwsh7NameLabel.Enabled = false;
            this.pwsh7NameLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.pwsh7NameLabel.Location = new System.Drawing.Point(3, 9);
            this.pwsh7NameLabel.Name = "pwsh7NameLabel";
            this.pwsh7NameLabel.Size = new System.Drawing.Size(68, 13);
            this.pwsh7NameLabel.TabIndex = 0;
            this.pwsh7NameLabel.Text = "PowerShell 7";
            // 
            // pwsh7StatusLabel
            // 
            this.pwsh7StatusLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.pwsh7StatusLabel.AutoSize = true;
            this.pwsh7StatusLabel.Enabled = false;
            this.pwsh7StatusLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.pwsh7StatusLabel.ForeColor = System.Drawing.Color.Gray;
            this.pwsh7StatusLabel.Location = new System.Drawing.Point(109, 9);
            this.pwsh7StatusLabel.Name = "pwsh7StatusLabel";
            this.pwsh7StatusLabel.Size = new System.Drawing.Size(14, 13);
            this.pwsh7StatusLabel.TabIndex = 1;
            this.pwsh7StatusLabel.Text = "-";
            // 
            // pwsh7ProgressBar
            // 
            this.pwsh7ProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pwsh7ProgressBar.Enabled = false;
            this.pwsh7ProgressBar.Location = new System.Drawing.Point(201, 11);
            this.pwsh7ProgressBar.Name = "pwsh7ProgressBar";
            this.pwsh7ProgressBar.Size = new System.Drawing.Size(450, 8);
            this.pwsh7ProgressBar.TabIndex = 2;
            // 
            // ohmyposhNameLabel
            // 
            this.ohmyposhNameLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ohmyposhNameLabel.AutoSize = true;
            this.ohmyposhNameLabel.Enabled = false;
            this.ohmyposhNameLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.ohmyposhNameLabel.Location = new System.Drawing.Point(3, 40);
            this.ohmyposhNameLabel.Name = "ohmyposhNameLabel";
            this.ohmyposhNameLabel.Size = new System.Drawing.Size(65, 13);
            this.ohmyposhNameLabel.TabIndex = 3;
            this.ohmyposhNameLabel.Text = "Oh My Posh";
            // 
            // ohmyposhStatusLabel
            // 
            this.ohmyposhStatusLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ohmyposhStatusLabel.AutoSize = true;
            this.ohmyposhStatusLabel.Enabled = false;
            this.ohmyposhStatusLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.ohmyposhStatusLabel.ForeColor = System.Drawing.Color.Gray;
            this.ohmyposhStatusLabel.Location = new System.Drawing.Point(109, 40);
            this.ohmyposhStatusLabel.Name = "ohmyposhStatusLabel";
            this.ohmyposhStatusLabel.Size = new System.Drawing.Size(14, 13);
            this.ohmyposhStatusLabel.TabIndex = 4;
            this.ohmyposhStatusLabel.Text = "-";
            // 
            // ohmyposhProgressBar
            // 
            this.ohmyposhProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ohmyposhProgressBar.Enabled = false;
            this.ohmyposhProgressBar.Location = new System.Drawing.Point(201, 42);
            this.ohmyposhProgressBar.Name = "ohmyposhProgressBar";
            this.ohmyposhProgressBar.Size = new System.Drawing.Size(450, 8);
            this.ohmyposhProgressBar.TabIndex = 5;
            // 
            // termIconsNameLabel
            // 
            this.termIconsNameLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.termIconsNameLabel.AutoSize = true;
            this.termIconsNameLabel.Enabled = false;
            this.termIconsNameLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.termIconsNameLabel.Location = new System.Drawing.Point(3, 71);
            this.termIconsNameLabel.Name = "termIconsNameLabel";
            this.termIconsNameLabel.Size = new System.Drawing.Size(81, 13);
            this.termIconsNameLabel.TabIndex = 6;
            this.termIconsNameLabel.Text = "Terminal-Icons";
            // 
            // termIconsStatusLabel
            // 
            this.termIconsStatusLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.termIconsStatusLabel.AutoSize = true;
            this.termIconsStatusLabel.Enabled = false;
            this.termIconsStatusLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.termIconsStatusLabel.ForeColor = System.Drawing.Color.Gray;
            this.termIconsStatusLabel.Location = new System.Drawing.Point(109, 71);
            this.termIconsStatusLabel.Name = "termIconsStatusLabel";
            this.termIconsStatusLabel.Size = new System.Drawing.Size(14, 13);
            this.termIconsStatusLabel.TabIndex = 7;
            this.termIconsStatusLabel.Text = "-";
            // 
            // termIconsProgressBar
            // 
            this.termIconsProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.termIconsProgressBar.Enabled = false;
            this.termIconsProgressBar.Location = new System.Drawing.Point(201, 73);
            this.termIconsProgressBar.Name = "termIconsProgressBar";
            this.termIconsProgressBar.Size = new System.Drawing.Size(450, 8);
            this.termIconsProgressBar.TabIndex = 8;
            // 
            // psfzfNameLabel
            // 
            this.psfzfNameLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.psfzfNameLabel.AutoSize = true;
            this.psfzfNameLabel.Enabled = false;
            this.psfzfNameLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.psfzfNameLabel.Location = new System.Drawing.Point(3, 102);
            this.psfzfNameLabel.Name = "psfzfNameLabel";
            this.psfzfNameLabel.Size = new System.Drawing.Size(37, 13);
            this.psfzfNameLabel.TabIndex = 9;
            this.psfzfNameLabel.Text = "PSFzf";
            // 
            // psfzfStatusLabel
            // 
            this.psfzfStatusLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.psfzfStatusLabel.AutoSize = true;
            this.psfzfStatusLabel.Enabled = false;
            this.psfzfStatusLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.psfzfStatusLabel.ForeColor = System.Drawing.Color.Gray;
            this.psfzfStatusLabel.Location = new System.Drawing.Point(109, 102);
            this.psfzfStatusLabel.Name = "psfzfStatusLabel";
            this.psfzfStatusLabel.Size = new System.Drawing.Size(14, 13);
            this.psfzfStatusLabel.TabIndex = 10;
            this.psfzfStatusLabel.Text = "-";
            // 
            // psfzfProgressBar
            // 
            this.psfzfProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.psfzfProgressBar.Enabled = false;
            this.psfzfProgressBar.Location = new System.Drawing.Point(201, 104);
            this.psfzfProgressBar.Name = "psfzfProgressBar";
            this.psfzfProgressBar.Size = new System.Drawing.Size(450, 8);
            this.psfzfProgressBar.TabIndex = 11;
            // 
            // modernUnixNameLabel
            // 
            this.modernUnixNameLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.modernUnixNameLabel.AutoSize = true;
            this.modernUnixNameLabel.Enabled = false;
            this.modernUnixNameLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.modernUnixNameLabel.Location = new System.Drawing.Point(3, 133);
            this.modernUnixNameLabel.Name = "modernUnixNameLabel";
            this.modernUnixNameLabel.Size = new System.Drawing.Size(93, 13);
            this.modernUnixNameLabel.TabIndex = 12;
            this.modernUnixNameLabel.Text = "modern-unix-win";
            // 
            // modernUnixStatusLabel
            // 
            this.modernUnixStatusLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.modernUnixStatusLabel.AutoSize = true;
            this.modernUnixStatusLabel.Enabled = false;
            this.modernUnixStatusLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.modernUnixStatusLabel.ForeColor = System.Drawing.Color.Gray;
            this.modernUnixStatusLabel.Location = new System.Drawing.Point(109, 133);
            this.modernUnixStatusLabel.Name = "modernUnixStatusLabel";
            this.modernUnixStatusLabel.Size = new System.Drawing.Size(14, 13);
            this.modernUnixStatusLabel.TabIndex = 13;
            this.modernUnixStatusLabel.Text = "-";
            // 
            // modernUnixProgressBar
            // 
            this.modernUnixProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.modernUnixProgressBar.Enabled = false;
            this.modernUnixProgressBar.Location = new System.Drawing.Point(201, 135);
            this.modernUnixProgressBar.Name = "modernUnixProgressBar";
            this.modernUnixProgressBar.Size = new System.Drawing.Size(450, 8);
            this.modernUnixProgressBar.TabIndex = 14;
            // 
            // vscodeNameLabel
            // 
            this.vscodeNameLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.vscodeNameLabel.AutoSize = true;
            this.vscodeNameLabel.Enabled = false;
            this.vscodeNameLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.vscodeNameLabel.Location = new System.Drawing.Point(3, 165);
            this.vscodeNameLabel.Name = "vscodeNameLabel";
            this.vscodeNameLabel.Size = new System.Drawing.Size(45, 13);
            this.vscodeNameLabel.TabIndex = 15;
            this.vscodeNameLabel.Text = "VSCode";
            // 
            // vscodeStatusLabel
            // 
            this.vscodeStatusLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.vscodeStatusLabel.AutoSize = true;
            this.vscodeStatusLabel.Enabled = false;
            this.vscodeStatusLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.vscodeStatusLabel.ForeColor = System.Drawing.Color.Gray;
            this.vscodeStatusLabel.Location = new System.Drawing.Point(109, 165);
            this.vscodeStatusLabel.Name = "vscodeStatusLabel";
            this.vscodeStatusLabel.Size = new System.Drawing.Size(14, 13);
            this.vscodeStatusLabel.TabIndex = 16;
            this.vscodeStatusLabel.Text = "-";
            // 
            // vscodeProgressBar
            // 
            this.vscodeProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.vscodeProgressBar.Enabled = false;
            this.vscodeProgressBar.Location = new System.Drawing.Point(201, 167);
            this.vscodeProgressBar.Name = "vscodeProgressBar";
            this.vscodeProgressBar.Size = new System.Drawing.Size(450, 8);
            this.vscodeProgressBar.TabIndex = 17;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.activityLog);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(673, 234);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Log";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // activityLog
            // 
            this.activityLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.activityLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.activityLog.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.activityLog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.activityLog.Location = new System.Drawing.Point(9, 10);
            this.activityLog.Multiline = true;
            this.activityLog.Name = "activityLog";
            this.activityLog.ReadOnly = true;
            this.activityLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.activityLog.Size = new System.Drawing.Size(654, 213);
            this.activityLog.TabIndex = 11;
            this.activityLog.TabStop = false;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label1.ForeColor = System.Drawing.Color.SteelBlue;
            this.label1.Location = new System.Drawing.Point(9, 335);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 19);
            this.label1.TabIndex = 20;
            this.label1.Text = "PlanX Lab · Atticle";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(0, 88, 160);
            this.headerPanel.Controls.Add(this.headerSubLabel);
            this.headerPanel.Controls.Add(this.headerTitleLabel);
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(694, 64);
            this.headerPanel.TabIndex = 30;
            // 
            // headerTitleLabel
            // 
            this.headerTitleLabel.AutoSize = true;
            this.headerTitleLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.headerTitleLabel.ForeColor = System.Drawing.Color.White;
            this.headerTitleLabel.Location = new System.Drawing.Point(20, 10);
            this.headerTitleLabel.Name = "headerTitleLabel";
            this.headerTitleLabel.Text = "DevEnv Setup";
            // 
            // headerSubLabel
            // 
            this.headerSubLabel.AutoSize = true;
            this.headerSubLabel.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.headerSubLabel.ForeColor = System.Drawing.Color.FromArgb(176, 210, 240);
            this.headerSubLabel.Location = new System.Drawing.Point(22, 38);
            this.headerSubLabel.Name = "headerSubLabel";
            this.headerSubLabel.Text = "Portable Development Environment Updater";
            // 
            // UpgradeForm
            //
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(694, 370);
            this.Controls.Add(this.headerPanel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.upgradeButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "UpgradeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DevEnv Setup";
            this.TopMost = true;
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.componentTable.ResumeLayout(false);
            this.componentTable.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label elapsedTimeLabel;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.TableLayoutPanel componentTable;
        private System.Windows.Forms.Label pwsh7NameLabel;
        private System.Windows.Forms.Label pwsh7StatusLabel;
        private System.Windows.Forms.ProgressBar pwsh7ProgressBar;
        private System.Windows.Forms.Label ohmyposhNameLabel;
        private System.Windows.Forms.Label ohmyposhStatusLabel;
        private System.Windows.Forms.ProgressBar ohmyposhProgressBar;
        private System.Windows.Forms.Label termIconsNameLabel;
        private System.Windows.Forms.Label termIconsStatusLabel;
        private System.Windows.Forms.ProgressBar termIconsProgressBar;
        private System.Windows.Forms.Label psfzfNameLabel;
        private System.Windows.Forms.Label psfzfStatusLabel;
        private System.Windows.Forms.ProgressBar psfzfProgressBar;
        private System.Windows.Forms.Label modernUnixNameLabel;
        private System.Windows.Forms.Label modernUnixStatusLabel;
        private System.Windows.Forms.ProgressBar modernUnixProgressBar;
        private System.Windows.Forms.Label vscodeNameLabel;
        private System.Windows.Forms.Label vscodeStatusLabel;
        private System.Windows.Forms.ProgressBar vscodeProgressBar;
        private System.Windows.Forms.TextBox activityLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label headerTitleLabel;
        private System.Windows.Forms.Label headerSubLabel;
    }
}
