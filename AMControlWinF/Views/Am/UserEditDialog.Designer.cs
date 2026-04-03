namespace AMControlWinF.Views.Am
{
    partial class UserEditDialog
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.textureBackgroundDialog = new AMControlWinF.Views.Main.TextureBackgroundControl();
            this.panelShell = new AntdUI.Panel();
            this.panelContent = new AntdUI.Panel();
            this.stackFormRows = new AntdUI.StackPanel();
            this.panelRowRemark = new AntdUI.Panel();
            this.inputRemark = new AntdUI.Input();
            this.labelRemark = new AntdUI.Label();
            this.panelRowEnabled = new AntdUI.Panel();
            this.flowEnabledHost = new AntdUI.FlowPanel();
            this.checkEnabled = new AntdUI.Checkbox();
            this.labelEnabled = new AntdUI.Label();
            this.panelRowPassword = new AntdUI.Panel();
            this.inputPassword = new AntdUI.Input();
            this.labelPassword = new AntdUI.Label();
            this.panelRowRole = new AntdUI.Panel();
            this.dropdownRole = new AntdUI.Select();
            this.labelRole = new AntdUI.Label();
            this.panelRowUserName = new AntdUI.Panel();
            this.inputUserName = new AntdUI.Input();
            this.labelUserName = new AntdUI.Label();
            this.panelRowLoginName = new AntdUI.Panel();
            this.inputLoginName = new AntdUI.Input();
            this.labelLoginName = new AntdUI.Label();
            this.panelFooter = new AntdUI.Panel();
            this.flowFooterButtons = new AntdUI.FlowPanel();
            this.buttonOk = new AntdUI.Button();
            this.buttonCancel = new AntdUI.Button();
            this.panelHeader = new AntdUI.Panel();
            this.flowHeaderRight = new AntdUI.FlowPanel();
            this.labelDialogDescription = new AntdUI.Label();
            this.flowHeaderLeft = new AntdUI.FlowPanel();
            this.labelDialogTitle = new AntdUI.Label();
            this.textureBackgroundDialog.SuspendLayout();
            this.panelShell.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.stackFormRows.SuspendLayout();
            this.panelRowRemark.SuspendLayout();
            this.panelRowEnabled.SuspendLayout();
            this.flowEnabledHost.SuspendLayout();
            this.panelRowPassword.SuspendLayout();
            this.panelRowRole.SuspendLayout();
            this.panelRowUserName.SuspendLayout();
            this.panelRowLoginName.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.flowFooterButtons.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.flowHeaderRight.SuspendLayout();
            this.flowHeaderLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // textureBackgroundDialog
            // 
            this.textureBackgroundDialog.Controls.Add(this.panelShell);
            this.textureBackgroundDialog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textureBackgroundDialog.Location = new System.Drawing.Point(0, 0);
            this.textureBackgroundDialog.Margin = new System.Windows.Forms.Padding(0);
            this.textureBackgroundDialog.Name = "textureBackgroundDialog";
            this.textureBackgroundDialog.Size = new System.Drawing.Size(560, 600);
            this.textureBackgroundDialog.TabIndex = 0;
            // 
            // panelShell
            // 
            this.panelShell.BackColor = System.Drawing.Color.Transparent;
            this.panelShell.Controls.Add(this.panelContent);
            this.panelShell.Controls.Add(this.panelFooter);
            this.panelShell.Controls.Add(this.panelHeader);
            this.panelShell.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelShell.Location = new System.Drawing.Point(0, 0);
            this.panelShell.Margin = new System.Windows.Forms.Padding(0);
            this.panelShell.Name = "panelShell";
            this.panelShell.Padding = new System.Windows.Forms.Padding(12);
            this.panelShell.Radius = 16;
            this.panelShell.Shadow = 16;
            this.panelShell.ShadowOpacity = 0.2F;
            this.panelShell.ShadowOpacityAnimation = true;
            this.panelShell.Size = new System.Drawing.Size(560, 600);
            this.panelShell.TabIndex = 0;
            // 
            // panelContent
            // 
            this.panelContent.Controls.Add(this.stackFormRows);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(28, 84);
            this.panelContent.Margin = new System.Windows.Forms.Padding(0);
            this.panelContent.Name = "panelContent";
            this.panelContent.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.panelContent.Radius = 0;
            this.panelContent.Size = new System.Drawing.Size(504, 431);
            this.panelContent.TabIndex = 1;
            // 
            // stackFormRows
            // 
            this.stackFormRows.AutoScroll = true;
            this.stackFormRows.Controls.Add(this.panelRowRemark);
            this.stackFormRows.Controls.Add(this.panelRowEnabled);
            this.stackFormRows.Controls.Add(this.panelRowPassword);
            this.stackFormRows.Controls.Add(this.panelRowRole);
            this.stackFormRows.Controls.Add(this.panelRowUserName);
            this.stackFormRows.Controls.Add(this.panelRowLoginName);
            this.stackFormRows.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stackFormRows.Gap = 4;
            this.stackFormRows.Location = new System.Drawing.Point(4, 0);
            this.stackFormRows.Margin = new System.Windows.Forms.Padding(0);
            this.stackFormRows.Name = "stackFormRows";
            this.stackFormRows.Size = new System.Drawing.Size(496, 431);
            this.stackFormRows.TabIndex = 0;
            this.stackFormRows.Text = "stackFormRows";
            this.stackFormRows.Vertical = true;
            // 
            // panelRowRemark
            // 
            this.panelRowRemark.Controls.Add(this.inputRemark);
            this.panelRowRemark.Controls.Add(this.labelRemark);
            this.panelRowRemark.Location = new System.Drawing.Point(0, 300);
            this.panelRowRemark.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowRemark.Name = "panelRowRemark";
            this.panelRowRemark.Radius = 0;
            this.panelRowRemark.Size = new System.Drawing.Size(496, 76);
            this.panelRowRemark.TabIndex = 5;
            // 
            // inputRemark
            // 
            this.inputRemark.AutoScroll = true;
            this.inputRemark.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputRemark.Location = new System.Drawing.Point(0, 26);
            this.inputRemark.Margin = new System.Windows.Forms.Padding(0);
            this.inputRemark.Multiline = true;
            this.inputRemark.Name = "inputRemark";
            this.inputRemark.PlaceholderText = "请输入备注（可选）";
            this.inputRemark.Size = new System.Drawing.Size(496, 50);
            this.inputRemark.TabIndex = 1;
            this.inputRemark.WaveSize = 0;
            // 
            // labelRemark
            // 
            this.labelRemark.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelRemark.Location = new System.Drawing.Point(0, 0);
            this.labelRemark.Margin = new System.Windows.Forms.Padding(0);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(496, 22);
            this.labelRemark.TabIndex = 0;
            this.labelRemark.Text = "备注";
            // 
            // panelRowEnabled
            // 
            this.panelRowEnabled.Controls.Add(this.flowEnabledHost);
            this.panelRowEnabled.Controls.Add(this.labelEnabled);
            this.panelRowEnabled.Location = new System.Drawing.Point(0, 240);
            this.panelRowEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowEnabled.Name = "panelRowEnabled";
            this.panelRowEnabled.Radius = 0;
            this.panelRowEnabled.Size = new System.Drawing.Size(496, 56);
            this.panelRowEnabled.TabIndex = 4;
            // 
            // flowEnabledHost
            // 
            this.flowEnabledHost.Controls.Add(this.checkEnabled);
            this.flowEnabledHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowEnabledHost.Location = new System.Drawing.Point(0, 22);
            this.flowEnabledHost.Margin = new System.Windows.Forms.Padding(0);
            this.flowEnabledHost.Name = "flowEnabledHost";
            this.flowEnabledHost.Size = new System.Drawing.Size(496, 34);
            this.flowEnabledHost.TabIndex = 1;
            this.flowEnabledHost.Text = "flowEnabledHost";
            // 
            // checkEnabled
            // 
            this.checkEnabled.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkEnabled.Checked = true;
            this.checkEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkEnabled.Location = new System.Drawing.Point(0, 0);
            this.checkEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.checkEnabled.Name = "checkEnabled";
            this.checkEnabled.Size = new System.Drawing.Size(61, 34);
            this.checkEnabled.TabIndex = 0;
            this.checkEnabled.Text = "启用";
            // 
            // labelEnabled
            // 
            this.labelEnabled.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelEnabled.Location = new System.Drawing.Point(0, 0);
            this.labelEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.labelEnabled.Name = "labelEnabled";
            this.labelEnabled.Size = new System.Drawing.Size(496, 22);
            this.labelEnabled.TabIndex = 0;
            this.labelEnabled.Text = "启用状态";
            // 
            // panelRowPassword
            // 
            this.panelRowPassword.Controls.Add(this.inputPassword);
            this.panelRowPassword.Controls.Add(this.labelPassword);
            this.panelRowPassword.Location = new System.Drawing.Point(0, 180);
            this.panelRowPassword.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowPassword.Name = "panelRowPassword";
            this.panelRowPassword.Radius = 0;
            this.panelRowPassword.Size = new System.Drawing.Size(496, 56);
            this.panelRowPassword.TabIndex = 3;
            // 
            // inputPassword
            // 
            this.inputPassword.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputPassword.Location = new System.Drawing.Point(0, 20);
            this.inputPassword.Margin = new System.Windows.Forms.Padding(0);
            this.inputPassword.Name = "inputPassword";
            this.inputPassword.PasswordChar = '●';
            this.inputPassword.PlaceholderText = "请输入初始密码";
            this.inputPassword.Size = new System.Drawing.Size(496, 36);
            this.inputPassword.TabIndex = 1;
            this.inputPassword.WaveSize = 0;
            // 
            // labelPassword
            // 
            this.labelPassword.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelPassword.Location = new System.Drawing.Point(0, 0);
            this.labelPassword.Margin = new System.Windows.Forms.Padding(0);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(496, 22);
            this.labelPassword.TabIndex = 0;
            this.labelPassword.Text = "初始密码";
            // 
            // panelRowRole
            // 
            this.panelRowRole.Controls.Add(this.dropdownRole);
            this.panelRowRole.Controls.Add(this.labelRole);
            this.panelRowRole.Location = new System.Drawing.Point(0, 120);
            this.panelRowRole.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowRole.Name = "panelRowRole";
            this.panelRowRole.Radius = 0;
            this.panelRowRole.Size = new System.Drawing.Size(496, 56);
            this.panelRowRole.TabIndex = 2;
            // 
            // dropdownRole
            // 
            this.dropdownRole.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dropdownRole.Location = new System.Drawing.Point(0, 20);
            this.dropdownRole.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownRole.Name = "dropdownRole";
            this.dropdownRole.Size = new System.Drawing.Size(496, 36);
            this.dropdownRole.TabIndex = 1;
            this.dropdownRole.WaveSize = 0;
            // 
            // labelRole
            // 
            this.labelRole.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelRole.Location = new System.Drawing.Point(0, 0);
            this.labelRole.Margin = new System.Windows.Forms.Padding(0);
            this.labelRole.Name = "labelRole";
            this.labelRole.Size = new System.Drawing.Size(496, 22);
            this.labelRole.TabIndex = 0;
            this.labelRole.Text = "角色";
            // 
            // panelRowUserName
            // 
            this.panelRowUserName.Controls.Add(this.inputUserName);
            this.panelRowUserName.Controls.Add(this.labelUserName);
            this.panelRowUserName.Location = new System.Drawing.Point(0, 60);
            this.panelRowUserName.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowUserName.Name = "panelRowUserName";
            this.panelRowUserName.Radius = 0;
            this.panelRowUserName.Size = new System.Drawing.Size(496, 56);
            this.panelRowUserName.TabIndex = 1;
            // 
            // inputUserName
            // 
            this.inputUserName.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputUserName.Location = new System.Drawing.Point(0, 20);
            this.inputUserName.Margin = new System.Windows.Forms.Padding(0);
            this.inputUserName.Name = "inputUserName";
            this.inputUserName.PlaceholderText = "请输入用户名";
            this.inputUserName.Size = new System.Drawing.Size(496, 36);
            this.inputUserName.TabIndex = 1;
            this.inputUserName.WaveSize = 0;
            // 
            // labelUserName
            // 
            this.labelUserName.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelUserName.Location = new System.Drawing.Point(0, 0);
            this.labelUserName.Margin = new System.Windows.Forms.Padding(0);
            this.labelUserName.Name = "labelUserName";
            this.labelUserName.Size = new System.Drawing.Size(496, 22);
            this.labelUserName.TabIndex = 0;
            this.labelUserName.Text = "用户名";
            // 
            // panelRowLoginName
            // 
            this.panelRowLoginName.Controls.Add(this.inputLoginName);
            this.panelRowLoginName.Controls.Add(this.labelLoginName);
            this.panelRowLoginName.Location = new System.Drawing.Point(0, 0);
            this.panelRowLoginName.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowLoginName.Name = "panelRowLoginName";
            this.panelRowLoginName.Radius = 0;
            this.panelRowLoginName.Size = new System.Drawing.Size(496, 56);
            this.panelRowLoginName.TabIndex = 0;
            // 
            // inputLoginName
            // 
            this.inputLoginName.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputLoginName.Location = new System.Drawing.Point(0, 20);
            this.inputLoginName.Margin = new System.Windows.Forms.Padding(0);
            this.inputLoginName.Name = "inputLoginName";
            this.inputLoginName.PlaceholderText = "请输入登录名";
            this.inputLoginName.Size = new System.Drawing.Size(496, 36);
            this.inputLoginName.TabIndex = 1;
            this.inputLoginName.WaveSize = 0;
            // 
            // labelLoginName
            // 
            this.labelLoginName.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelLoginName.Location = new System.Drawing.Point(0, 0);
            this.labelLoginName.Margin = new System.Windows.Forms.Padding(0);
            this.labelLoginName.Name = "labelLoginName";
            this.labelLoginName.Size = new System.Drawing.Size(496, 22);
            this.labelLoginName.TabIndex = 0;
            this.labelLoginName.Text = "登录名";
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.flowFooterButtons);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(28, 515);
            this.panelFooter.Margin = new System.Windows.Forms.Padding(0);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Padding = new System.Windows.Forms.Padding(4, 10, 4, 0);
            this.panelFooter.Radius = 0;
            this.panelFooter.Size = new System.Drawing.Size(504, 57);
            this.panelFooter.TabIndex = 2;
            // 
            // flowFooterButtons
            // 
            this.flowFooterButtons.Controls.Add(this.buttonOk);
            this.flowFooterButtons.Controls.Add(this.buttonCancel);
            this.flowFooterButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowFooterButtons.Gap = 10;
            this.flowFooterButtons.Location = new System.Drawing.Point(256, 10);
            this.flowFooterButtons.Margin = new System.Windows.Forms.Padding(0);
            this.flowFooterButtons.Name = "flowFooterButtons";
            this.flowFooterButtons.Size = new System.Drawing.Size(244, 47);
            this.flowFooterButtons.TabIndex = 0;
            this.flowFooterButtons.Text = "flowFooterButtons";
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(126, 0);
            this.buttonOk.Margin = new System.Windows.Forms.Padding(0);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Radius = 8;
            this.buttonOk.Size = new System.Drawing.Size(116, 38);
            this.buttonOk.TabIndex = 1;
            this.buttonOk.Text = "确认";
            this.buttonOk.Type = AntdUI.TTypeMini.Primary;
            this.buttonOk.WaveSize = 0;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(0, 0);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Radius = 8;
            this.buttonCancel.Size = new System.Drawing.Size(116, 38);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.WaveSize = 0;
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.flowHeaderRight);
            this.panelHeader.Controls.Add(this.flowHeaderLeft);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(28, 28);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Padding = new System.Windows.Forms.Padding(4, 0, 4, 8);
            this.panelHeader.Radius = 0;
            this.panelHeader.Size = new System.Drawing.Size(504, 56);
            this.panelHeader.TabIndex = 0;
            // 
            // flowHeaderRight
            // 
            this.flowHeaderRight.Controls.Add(this.labelDialogDescription);
            this.flowHeaderRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowHeaderRight.Location = new System.Drawing.Point(220, 0);
            this.flowHeaderRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowHeaderRight.Name = "flowHeaderRight";
            this.flowHeaderRight.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.flowHeaderRight.Size = new System.Drawing.Size(280, 48);
            this.flowHeaderRight.TabIndex = 1;
            this.flowHeaderRight.Text = "flowHeaderRight";
            // 
            // labelDialogDescription
            // 
            this.labelDialogDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDialogDescription.Location = new System.Drawing.Point(0, 0);
            this.labelDialogDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDialogDescription.Name = "labelDialogDescription";
            this.labelDialogDescription.Size = new System.Drawing.Size(278, 48);
            this.labelDialogDescription.TabIndex = 0;
            this.labelDialogDescription.Text = "创建用户账号并设置角色、状态和备注信息。";
            this.labelDialogDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // flowHeaderLeft
            // 
            this.flowHeaderLeft.Controls.Add(this.labelDialogTitle);
            this.flowHeaderLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowHeaderLeft.Location = new System.Drawing.Point(4, 0);
            this.flowHeaderLeft.Margin = new System.Windows.Forms.Padding(0);
            this.flowHeaderLeft.Name = "flowHeaderLeft";
            this.flowHeaderLeft.Size = new System.Drawing.Size(173, 48);
            this.flowHeaderLeft.TabIndex = 0;
            this.flowHeaderLeft.Text = "flowHeaderLeft";
            // 
            // labelDialogTitle
            // 
            this.labelDialogTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelDialogTitle.Location = new System.Drawing.Point(0, 0);
            this.labelDialogTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelDialogTitle.Name = "labelDialogTitle";
            this.labelDialogTitle.Size = new System.Drawing.Size(173, 48);
            this.labelDialogTitle.TabIndex = 0;
            this.labelDialogTitle.Text = "新增用户";
            // 
            // UserEditDialog
            // 
            this.ClientSize = new System.Drawing.Size(560, 600);
            this.Controls.Add(this.textureBackgroundDialog);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserEditDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "用户信息";
            this.textureBackgroundDialog.ResumeLayout(false);
            this.panelShell.ResumeLayout(false);
            this.panelContent.ResumeLayout(false);
            this.stackFormRows.ResumeLayout(false);
            this.panelRowRemark.ResumeLayout(false);
            this.panelRowEnabled.ResumeLayout(false);
            this.flowEnabledHost.ResumeLayout(false);
            this.flowEnabledHost.PerformLayout();
            this.panelRowPassword.ResumeLayout(false);
            this.panelRowRole.ResumeLayout(false);
            this.panelRowUserName.ResumeLayout(false);
            this.panelRowLoginName.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            this.flowFooterButtons.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.flowHeaderRight.ResumeLayout(false);
            this.flowHeaderLeft.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AMControlWinF.Views.Main.TextureBackgroundControl textureBackgroundDialog;
        private AntdUI.Panel panelShell;
        private AntdUI.Panel panelHeader;
        private AntdUI.FlowPanel flowHeaderLeft;
        private AntdUI.Label labelDialogTitle;
        private AntdUI.FlowPanel flowHeaderRight;
        private AntdUI.Label labelDialogDescription;
        private AntdUI.Panel panelContent;
        private AntdUI.StackPanel stackFormRows;
        private AntdUI.Panel panelRowLoginName;
        private AntdUI.Input inputLoginName;
        private AntdUI.Label labelLoginName;
        private AntdUI.Panel panelRowUserName;
        private AntdUI.Input inputUserName;
        private AntdUI.Label labelUserName;
        private AntdUI.Panel panelRowRole;
        private AntdUI.Select dropdownRole;
        private AntdUI.Label labelRole;
        private AntdUI.Panel panelRowPassword;
        private AntdUI.Input inputPassword;
        private AntdUI.Label labelPassword;
        private AntdUI.Panel panelRowEnabled;
        private AntdUI.FlowPanel flowEnabledHost;
        private AntdUI.Checkbox checkEnabled;
        private AntdUI.Label labelEnabled;
        private AntdUI.Panel panelRowRemark;
        private AntdUI.Input inputRemark;
        private AntdUI.Label labelRemark;
        private AntdUI.Panel panelFooter;
        private AntdUI.FlowPanel flowFooterButtons;
        private AntdUI.Button buttonCancel;
        private AntdUI.Button buttonOk;
    }
}