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
            this.panelBody = new AntdUI.Panel();
            this.panelScrollHost = new System.Windows.Forms.Panel();
            this.panelFormBody = new AntdUI.Panel();
            this.inputRemark = new AntdUI.Input();
            this.labelRemark = new AntdUI.Label();
            this.checkEnabled = new System.Windows.Forms.CheckBox();
            this.labelEnabled = new AntdUI.Label();
            this.inputPassword = new AntdUI.Input();
            this.labelPassword = new AntdUI.Label();
            this.dropdownRole = new AntdUI.Dropdown();
            this.labelRole = new AntdUI.Label();
            this.inputUserName = new AntdUI.Input();
            this.labelUserName = new AntdUI.Label();
            this.inputLoginName = new AntdUI.Input();
            this.labelLoginName = new AntdUI.Label();
            this.panelFooter = new AntdUI.Panel();
            this.flowFooterButtons = new AntdUI.FlowPanel();
            this.buttonCancel = new AntdUI.Button();
            this.buttonOk = new AntdUI.Button();
            this.panelHeader = new AntdUI.Panel();
            this.labelDialogDescription = new AntdUI.Label();
            this.labelDialogTitle = new AntdUI.Label();
            this.textureBackgroundDialog.SuspendLayout();
            this.panelShell.SuspendLayout();
            this.panelBody.SuspendLayout();
            this.panelScrollHost.SuspendLayout();
            this.panelFormBody.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.flowFooterButtons.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // textureBackgroundDialog
            // 
            this.textureBackgroundDialog.Controls.Add(this.panelShell);
            this.textureBackgroundDialog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textureBackgroundDialog.Location = new System.Drawing.Point(0, 0);
            this.textureBackgroundDialog.Margin = new System.Windows.Forms.Padding(0);
            this.textureBackgroundDialog.Name = "textureBackgroundDialog";
            this.textureBackgroundDialog.Size = new System.Drawing.Size(884, 601);
            this.textureBackgroundDialog.TabIndex = 0;
            // 
            // panelShell
            // 
            this.panelShell.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelShell.BackColor = System.Drawing.Color.Transparent;
            this.panelShell.Controls.Add(this.panelBody);
            this.panelShell.Controls.Add(this.panelFooter);
            this.panelShell.Controls.Add(this.panelHeader);
            this.panelShell.Location = new System.Drawing.Point(24, 24);
            this.panelShell.Margin = new System.Windows.Forms.Padding(0);
            this.panelShell.Name = "panelShell";
            this.panelShell.Padding = new System.Windows.Forms.Padding(12);
            this.panelShell.Radius = 16;
            this.panelShell.Shadow = 8;
            this.panelShell.Size = new System.Drawing.Size(836, 553);
            this.panelShell.TabIndex = 0;
            // 
            // panelBody
            // 
            this.panelBody.Controls.Add(this.panelScrollHost);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(20, 104);
            this.panelBody.Margin = new System.Windows.Forms.Padding(0);
            this.panelBody.Name = "panelBody";
            this.panelBody.Radius = 0;
            this.panelBody.Size = new System.Drawing.Size(796, 353);
            this.panelBody.TabIndex = 1;
            // 
            // panelScrollHost
            // 
            this.panelScrollHost.AutoScroll = true;
            this.panelScrollHost.Controls.Add(this.panelFormBody);
            this.panelScrollHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelScrollHost.Location = new System.Drawing.Point(0, 0);
            this.panelScrollHost.Margin = new System.Windows.Forms.Padding(0);
            this.panelScrollHost.Name = "panelScrollHost";
            this.panelScrollHost.Size = new System.Drawing.Size(796, 353);
            this.panelScrollHost.TabIndex = 0;
            // 
            // panelFormBody
            // 
            this.panelFormBody.Controls.Add(this.inputRemark);
            this.panelFormBody.Controls.Add(this.labelRemark);
            this.panelFormBody.Controls.Add(this.checkEnabled);
            this.panelFormBody.Controls.Add(this.labelEnabled);
            this.panelFormBody.Controls.Add(this.inputPassword);
            this.panelFormBody.Controls.Add(this.labelPassword);
            this.panelFormBody.Controls.Add(this.dropdownRole);
            this.panelFormBody.Controls.Add(this.labelRole);
            this.panelFormBody.Controls.Add(this.inputUserName);
            this.panelFormBody.Controls.Add(this.labelUserName);
            this.panelFormBody.Controls.Add(this.inputLoginName);
            this.panelFormBody.Controls.Add(this.labelLoginName);
            this.panelFormBody.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFormBody.Location = new System.Drawing.Point(0, 0);
            this.panelFormBody.Margin = new System.Windows.Forms.Padding(0);
            this.panelFormBody.Name = "panelFormBody";
            this.panelFormBody.Padding = new System.Windows.Forms.Padding(0);
            this.panelFormBody.Radius = 0;
            this.panelFormBody.Size = new System.Drawing.Size(779, 420);
            this.panelFormBody.TabIndex = 0;
            // 
            // inputRemark
            // 
            this.inputRemark.Location = new System.Drawing.Point(24, 326);
            this.inputRemark.Name = "inputRemark";
            this.inputRemark.PlaceholderText = "请输入备注（可选）";
            this.inputRemark.Size = new System.Drawing.Size(640, 88);
            this.inputRemark.TabIndex = 11;
            // 
            // labelRemark
            // 
            this.labelRemark.BackColor = System.Drawing.Color.Transparent;
            this.labelRemark.Location = new System.Drawing.Point(24, 300);
            this.labelRemark.Margin = new System.Windows.Forms.Padding(0);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(96, 22);
            this.labelRemark.TabIndex = 10;
            this.labelRemark.Text = "备注";
            // 
            // checkEnabled
            // 
            this.checkEnabled.AutoSize = true;
            this.checkEnabled.Checked = true;
            this.checkEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkEnabled.Location = new System.Drawing.Point(24, 258);
            this.checkEnabled.Name = "checkEnabled";
            this.checkEnabled.Size = new System.Drawing.Size(59, 24);
            this.checkEnabled.TabIndex = 9;
            this.checkEnabled.Text = "启用";
            this.checkEnabled.UseVisualStyleBackColor = true;
            // 
            // labelEnabled
            // 
            this.labelEnabled.BackColor = System.Drawing.Color.Transparent;
            this.labelEnabled.Location = new System.Drawing.Point(24, 230);
            this.labelEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.labelEnabled.Name = "labelEnabled";
            this.labelEnabled.Size = new System.Drawing.Size(96, 22);
            this.labelEnabled.TabIndex = 8;
            this.labelEnabled.Text = "状态";
            // 
            // inputPassword
            // 
            this.inputPassword.Location = new System.Drawing.Point(24, 190);
            this.inputPassword.Name = "inputPassword";
            this.inputPassword.PasswordChar = '●';
            this.inputPassword.PlaceholderText = "请输入初始密码";
            this.inputPassword.Size = new System.Drawing.Size(640, 40);
            this.inputPassword.TabIndex = 7;
            // 
            // labelPassword
            // 
            this.labelPassword.BackColor = System.Drawing.Color.Transparent;
            this.labelPassword.Location = new System.Drawing.Point(24, 162);
            this.labelPassword.Margin = new System.Windows.Forms.Padding(0);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(96, 22);
            this.labelPassword.TabIndex = 6;
            this.labelPassword.Text = "初始密码";
            // 
            // dropdownRole
            // 
            this.dropdownRole.Location = new System.Drawing.Point(24, 122);
            this.dropdownRole.Name = "dropdownRole";
            this.dropdownRole.Size = new System.Drawing.Size(640, 34);
            this.dropdownRole.TabIndex = 5;
            this.dropdownRole.Trigger = AntdUI.Trigger.Click;
            // 
            // labelRole
            // 
            this.labelRole.BackColor = System.Drawing.Color.Transparent;
            this.labelRole.Location = new System.Drawing.Point(24, 94);
            this.labelRole.Margin = new System.Windows.Forms.Padding(0);
            this.labelRole.Name = "labelRole";
            this.labelRole.Size = new System.Drawing.Size(96, 22);
            this.labelRole.TabIndex = 4;
            this.labelRole.Text = "角色";
            // 
            // inputUserName
            // 
            this.inputUserName.Location = new System.Drawing.Point(24, 54);
            this.inputUserName.Name = "inputUserName";
            this.inputUserName.PlaceholderText = "请输入用户名";
            this.inputUserName.Size = new System.Drawing.Size(640, 40);
            this.inputUserName.TabIndex = 3;
            // 
            // labelUserName
            // 
            this.labelUserName.BackColor = System.Drawing.Color.Transparent;
            this.labelUserName.Location = new System.Drawing.Point(24, 26);
            this.labelUserName.Margin = new System.Windows.Forms.Padding(0);
            this.labelUserName.Name = "labelUserName";
            this.labelUserName.Size = new System.Drawing.Size(96, 22);
            this.labelUserName.TabIndex = 2;
            this.labelUserName.Text = "用户名";
            // 
            // inputLoginName
            // 
            this.inputLoginName.Location = new System.Drawing.Point(24, -14);
            this.inputLoginName.Name = "inputLoginName";
            this.inputLoginName.PlaceholderText = "请输入登录名";
            this.inputLoginName.Size = new System.Drawing.Size(640, 40);
            this.inputLoginName.TabIndex = 1;
            // 
            // labelLoginName
            // 
            this.labelLoginName.BackColor = System.Drawing.Color.Transparent;
            this.labelLoginName.Location = new System.Drawing.Point(24, -42);
            this.labelLoginName.Margin = new System.Windows.Forms.Padding(0);
            this.labelLoginName.Name = "labelLoginName";
            this.labelLoginName.Size = new System.Drawing.Size(96, 22);
            this.labelLoginName.TabIndex = 0;
            this.labelLoginName.Text = "登录名";
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.flowFooterButtons);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(20, 457);
            this.panelFooter.Margin = new System.Windows.Forms.Padding(0);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Padding = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.panelFooter.Radius = 0;
            this.panelFooter.Size = new System.Drawing.Size(796, 76);
            this.panelFooter.TabIndex = 2;
            // 
            // flowFooterButtons
            // 
            this.flowFooterButtons.Controls.Add(this.buttonCancel);
            this.flowFooterButtons.Controls.Add(this.buttonOk);
            this.flowFooterButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowFooterButtons.Gap = 10;
            this.flowFooterButtons.Location = new System.Drawing.Point(526, 12);
            this.flowFooterButtons.Margin = new System.Windows.Forms.Padding(0);
            this.flowFooterButtons.Name = "flowFooterButtons";
            this.flowFooterButtons.Size = new System.Drawing.Size(270, 64);
            this.flowFooterButtons.TabIndex = 0;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(0, 0);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Radius = 8;
            this.buttonCancel.Size = new System.Drawing.Size(124, 40);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.WaveSize = 0;
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(134, 0);
            this.buttonOk.Margin = new System.Windows.Forms.Padding(0);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Radius = 8;
            this.buttonOk.Size = new System.Drawing.Size(124, 40);
            this.buttonOk.TabIndex = 1;
            this.buttonOk.Text = "确定";
            this.buttonOk.Type = AntdUI.TTypeMini.Primary;
            this.buttonOk.WaveSize = 0;
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.labelDialogDescription);
            this.panelHeader.Controls.Add(this.labelDialogTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(20, 20);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Padding = new System.Windows.Forms.Padding(4, 0, 4, 12);
            this.panelHeader.Radius = 0;
            this.panelHeader.Size = new System.Drawing.Size(796, 84);
            this.panelHeader.TabIndex = 0;
            // 
            // labelDialogDescription
            // 
            this.labelDialogDescription.BackColor = System.Drawing.Color.Transparent;
            this.labelDialogDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDialogDescription.Location = new System.Drawing.Point(4, 40);
            this.labelDialogDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDialogDescription.Name = "labelDialogDescription";
            this.labelDialogDescription.Size = new System.Drawing.Size(788, 28);
            this.labelDialogDescription.TabIndex = 1;
            this.labelDialogDescription.Text = "创建用户账号并设置角色、状态和备注信息。";
            // 
            // labelDialogTitle
            // 
            this.labelDialogTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelDialogTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDialogTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelDialogTitle.Location = new System.Drawing.Point(4, 0);
            this.labelDialogTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelDialogTitle.Name = "labelDialogTitle";
            this.labelDialogTitle.Size = new System.Drawing.Size(788, 40);
            this.labelDialogTitle.TabIndex = 0;
            this.labelDialogTitle.Text = "新增用户";
            // 
            // UserEditDialog
            // 
            this.ClientSize = new System.Drawing.Size(884, 601);
            this.Controls.Add(this.textureBackgroundDialog);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserEditDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "用户信息";
            this.textureBackgroundDialog.ResumeLayout(false);
            this.panelShell.ResumeLayout(false);
            this.panelBody.ResumeLayout(false);
            this.panelScrollHost.ResumeLayout(false);
            this.panelFormBody.ResumeLayout(false);
            this.panelFormBody.PerformLayout();
            this.panelFooter.ResumeLayout(false);
            this.flowFooterButtons.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AMControlWinF.Views.Main.TextureBackgroundControl textureBackgroundDialog;
        private AntdUI.Panel panelShell;
        private AntdUI.Panel panelHeader;
        private AntdUI.Label labelDialogDescription;
        private AntdUI.Label labelDialogTitle;
        private AntdUI.Panel panelBody;
        private System.Windows.Forms.Panel panelScrollHost;
        private AntdUI.Panel panelFormBody;
        private AntdUI.Input inputRemark;
        private AntdUI.Label labelRemark;
        private System.Windows.Forms.CheckBox checkEnabled;
        private AntdUI.Label labelEnabled;
        private AntdUI.Input inputPassword;
        private AntdUI.Label labelPassword;
        private AntdUI.Dropdown dropdownRole;
        private AntdUI.Label labelRole;
        private AntdUI.Input inputUserName;
        private AntdUI.Label labelUserName;
        private AntdUI.Input inputLoginName;
        private AntdUI.Label labelLoginName;
        private AntdUI.Panel panelFooter;
        private AntdUI.FlowPanel flowFooterButtons;
        private AntdUI.Button buttonCancel;
        private AntdUI.Button buttonOk;
    }
}