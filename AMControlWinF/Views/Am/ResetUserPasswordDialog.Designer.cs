namespace AMControlWinF.Views.Am
{
    partial class ResetUserPasswordDialog
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
            this.inputConfirmPassword = new AntdUI.Input();
            this.labelConfirmPassword = new AntdUI.Label();
            this.inputNewPassword = new AntdUI.Input();
            this.labelNewPassword = new AntdUI.Label();
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
            this.textureBackgroundDialog.Size = new System.Drawing.Size(760, 460);
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
            this.panelShell.Size = new System.Drawing.Size(712, 412);
            this.panelShell.TabIndex = 0;
            // 
            // panelBody
            // 
            this.panelBody.Controls.Add(this.panelScrollHost);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(20, 96);
            this.panelBody.Margin = new System.Windows.Forms.Padding(0);
            this.panelBody.Name = "panelBody";
            this.panelBody.Radius = 0;
            this.panelBody.Size = new System.Drawing.Size(672, 200);
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
            this.panelScrollHost.Size = new System.Drawing.Size(672, 200);
            this.panelScrollHost.TabIndex = 0;
            // 
            // panelFormBody
            // 
            this.panelFormBody.Controls.Add(this.inputConfirmPassword);
            this.panelFormBody.Controls.Add(this.labelConfirmPassword);
            this.panelFormBody.Controls.Add(this.inputNewPassword);
            this.panelFormBody.Controls.Add(this.labelNewPassword);
            this.panelFormBody.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFormBody.Location = new System.Drawing.Point(0, 0);
            this.panelFormBody.Margin = new System.Windows.Forms.Padding(0);
            this.panelFormBody.Name = "panelFormBody";
            this.panelFormBody.Padding = new System.Windows.Forms.Padding(0);
            this.panelFormBody.Radius = 0;
            this.panelFormBody.Size = new System.Drawing.Size(655, 172);
            this.panelFormBody.TabIndex = 0;
            // 
            // inputConfirmPassword
            // 
            this.inputConfirmPassword.Location = new System.Drawing.Point(24, 122);
            this.inputConfirmPassword.Name = "inputConfirmPassword";
            this.inputConfirmPassword.PasswordChar = '●';
            this.inputConfirmPassword.PlaceholderText = "请再次输入新密码";
            this.inputConfirmPassword.Size = new System.Drawing.Size(560, 40);
            this.inputConfirmPassword.TabIndex = 3;
            // 
            // labelConfirmPassword
            // 
            this.labelConfirmPassword.BackColor = System.Drawing.Color.Transparent;
            this.labelConfirmPassword.Location = new System.Drawing.Point(24, 94);
            this.labelConfirmPassword.Margin = new System.Windows.Forms.Padding(0);
            this.labelConfirmPassword.Name = "labelConfirmPassword";
            this.labelConfirmPassword.Size = new System.Drawing.Size(96, 22);
            this.labelConfirmPassword.TabIndex = 2;
            this.labelConfirmPassword.Text = "确认密码";
            // 
            // inputNewPassword
            // 
            this.inputNewPassword.Location = new System.Drawing.Point(24, 40);
            this.inputNewPassword.Name = "inputNewPassword";
            this.inputNewPassword.PasswordChar = '●';
            this.inputNewPassword.PlaceholderText = "请输入新密码";
            this.inputNewPassword.Size = new System.Drawing.Size(560, 40);
            this.inputNewPassword.TabIndex = 1;
            // 
            // labelNewPassword
            // 
            this.labelNewPassword.BackColor = System.Drawing.Color.Transparent;
            this.labelNewPassword.Location = new System.Drawing.Point(24, 12);
            this.labelNewPassword.Margin = new System.Windows.Forms.Padding(0);
            this.labelNewPassword.Name = "labelNewPassword";
            this.labelNewPassword.Size = new System.Drawing.Size(96, 22);
            this.labelNewPassword.TabIndex = 0;
            this.labelNewPassword.Text = "新密码";
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.flowFooterButtons);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(20, 296);
            this.panelFooter.Margin = new System.Windows.Forms.Padding(0);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Padding = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.panelFooter.Radius = 0;
            this.panelFooter.Size = new System.Drawing.Size(672, 96);
            this.panelFooter.TabIndex = 2;
            // 
            // flowFooterButtons
            // 
            this.flowFooterButtons.Controls.Add(this.buttonCancel);
            this.flowFooterButtons.Controls.Add(this.buttonOk);
            this.flowFooterButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowFooterButtons.Gap = 10;
            this.flowFooterButtons.Location = new System.Drawing.Point(402, 12);
            this.flowFooterButtons.Margin = new System.Windows.Forms.Padding(0);
            this.flowFooterButtons.Name = "flowFooterButtons";
            this.flowFooterButtons.Size = new System.Drawing.Size(270, 84);
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
            this.buttonOk.Text = "确认";
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
            this.panelHeader.Size = new System.Drawing.Size(672, 76);
            this.panelHeader.TabIndex = 0;
            // 
            // labelDialogDescription
            // 
            this.labelDialogDescription.BackColor = System.Drawing.Color.Transparent;
            this.labelDialogDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDialogDescription.Location = new System.Drawing.Point(4, 40);
            this.labelDialogDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDialogDescription.Name = "labelDialogDescription";
            this.labelDialogDescription.Size = new System.Drawing.Size(664, 24);
            this.labelDialogDescription.TabIndex = 1;
            this.labelDialogDescription.Text = "请输入两次一致的新密码。";
            // 
            // labelDialogTitle
            // 
            this.labelDialogTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelDialogTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDialogTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelDialogTitle.Location = new System.Drawing.Point(4, 0);
            this.labelDialogTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelDialogTitle.Name = "labelDialogTitle";
            this.labelDialogTitle.Size = new System.Drawing.Size(664, 40);
            this.labelDialogTitle.TabIndex = 0;
            this.labelDialogTitle.Text = "重置用户密码";
            // 
            // ResetUserPasswordDialog
            // 
            this.ClientSize = new System.Drawing.Size(760, 460);
            this.Controls.Add(this.textureBackgroundDialog);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ResetUserPasswordDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "重置用户密码";
            this.textureBackgroundDialog.ResumeLayout(false);
            this.panelShell.ResumeLayout(false);
            this.panelBody.ResumeLayout(false);
            this.panelScrollHost.ResumeLayout(false);
            this.panelFormBody.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            this.flowFooterButtons.ResumeLayout(false);
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
        private AntdUI.Input inputConfirmPassword;
        private AntdUI.Label labelConfirmPassword;
        private AntdUI.Input inputNewPassword;
        private AntdUI.Label labelNewPassword;
        private AntdUI.Panel panelFooter;
        private AntdUI.FlowPanel flowFooterButtons;
        private AntdUI.Button buttonCancel;
        private AntdUI.Button buttonOk;
    }
}