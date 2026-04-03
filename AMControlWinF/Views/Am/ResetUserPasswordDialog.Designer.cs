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
            this.panelContent = new AntdUI.Panel();
            this.stackFormRows = new AntdUI.StackPanel();
            this.panelRowNewPassword = new AntdUI.Panel();
            this.inputNewPassword = new AntdUI.Input();
            this.labelNewPassword = new AntdUI.Label();
            this.panelRowConfirmPassword = new AntdUI.Panel();
            this.inputConfirmPassword = new AntdUI.Input();
            this.labelConfirmPassword = new AntdUI.Label();
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
            this.panelRowNewPassword.SuspendLayout();
            this.panelRowConfirmPassword.SuspendLayout();
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
            this.textureBackgroundDialog.Size = new System.Drawing.Size(560, 460);
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
            this.panelShell.Size = new System.Drawing.Size(560, 460);
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
            this.panelContent.Size = new System.Drawing.Size(504, 281);
            this.panelContent.TabIndex = 1;
            // 
            // stackFormRows
            // 
            this.stackFormRows.AutoScroll = true;
            this.stackFormRows.Controls.Add(this.panelRowNewPassword);
            this.stackFormRows.Controls.Add(this.panelRowConfirmPassword);
            this.stackFormRows.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stackFormRows.Gap = 6;
            this.stackFormRows.Location = new System.Drawing.Point(4, 0);
            this.stackFormRows.Margin = new System.Windows.Forms.Padding(0);
            this.stackFormRows.Name = "stackFormRows";
            this.stackFormRows.Size = new System.Drawing.Size(496, 281);
            this.stackFormRows.TabIndex = 0;
            this.stackFormRows.Text = "stackFormRows";
            this.stackFormRows.Vertical = true;
            // 
            // panelRowNewPassword
            // 
            this.panelRowNewPassword.Controls.Add(this.inputNewPassword);
            this.panelRowNewPassword.Controls.Add(this.labelNewPassword);
            this.panelRowNewPassword.Location = new System.Drawing.Point(0, 66);
            this.panelRowNewPassword.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowNewPassword.Name = "panelRowNewPassword";
            this.panelRowNewPassword.Radius = 0;
            this.panelRowNewPassword.Size = new System.Drawing.Size(496, 60);
            this.panelRowNewPassword.TabIndex = 0;
            // 
            // inputNewPassword
            // 
            this.inputNewPassword.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputNewPassword.Location = new System.Drawing.Point(0, 24);
            this.inputNewPassword.Margin = new System.Windows.Forms.Padding(0);
            this.inputNewPassword.Name = "inputNewPassword";
            this.inputNewPassword.PasswordChar = '●';
            this.inputNewPassword.PlaceholderText = "请输入新密码";
            this.inputNewPassword.Size = new System.Drawing.Size(496, 36);
            this.inputNewPassword.TabIndex = 1;
            this.inputNewPassword.WaveSize = 0;
            // 
            // labelNewPassword
            // 
            this.labelNewPassword.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelNewPassword.Location = new System.Drawing.Point(0, 0);
            this.labelNewPassword.Margin = new System.Windows.Forms.Padding(0);
            this.labelNewPassword.Name = "labelNewPassword";
            this.labelNewPassword.Size = new System.Drawing.Size(496, 22);
            this.labelNewPassword.TabIndex = 0;
            this.labelNewPassword.Text = "新密码";
            // 
            // panelRowConfirmPassword
            // 
            this.panelRowConfirmPassword.Controls.Add(this.inputConfirmPassword);
            this.panelRowConfirmPassword.Controls.Add(this.labelConfirmPassword);
            this.panelRowConfirmPassword.Location = new System.Drawing.Point(0, 0);
            this.panelRowConfirmPassword.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowConfirmPassword.Name = "panelRowConfirmPassword";
            this.panelRowConfirmPassword.Radius = 0;
            this.panelRowConfirmPassword.Size = new System.Drawing.Size(496, 60);
            this.panelRowConfirmPassword.TabIndex = 1;
            // 
            // inputConfirmPassword
            // 
            this.inputConfirmPassword.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputConfirmPassword.Location = new System.Drawing.Point(0, 24);
            this.inputConfirmPassword.Margin = new System.Windows.Forms.Padding(0);
            this.inputConfirmPassword.Name = "inputConfirmPassword";
            this.inputConfirmPassword.PasswordChar = '●';
            this.inputConfirmPassword.PlaceholderText = "请再次输入新密码";
            this.inputConfirmPassword.Size = new System.Drawing.Size(496, 36);
            this.inputConfirmPassword.TabIndex = 1;
            this.inputConfirmPassword.WaveSize = 0;
            // 
            // labelConfirmPassword
            // 
            this.labelConfirmPassword.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelConfirmPassword.Location = new System.Drawing.Point(0, 0);
            this.labelConfirmPassword.Margin = new System.Windows.Forms.Padding(0);
            this.labelConfirmPassword.Name = "labelConfirmPassword";
            this.labelConfirmPassword.Size = new System.Drawing.Size(496, 22);
            this.labelConfirmPassword.TabIndex = 0;
            this.labelConfirmPassword.Text = "确认密码";
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.flowFooterButtons);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(28, 365);
            this.panelFooter.Margin = new System.Windows.Forms.Padding(0);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Padding = new System.Windows.Forms.Padding(4, 10, 4, 0);
            this.panelFooter.Radius = 0;
            this.panelFooter.Size = new System.Drawing.Size(504, 67);
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
            this.flowFooterButtons.Size = new System.Drawing.Size(244, 57);
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
            this.buttonOk.Text = "保存";
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
            this.flowHeaderRight.Location = new System.Drawing.Point(211, 0);
            this.flowHeaderRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowHeaderRight.Name = "flowHeaderRight";
            this.flowHeaderRight.Size = new System.Drawing.Size(289, 48);
            this.flowHeaderRight.TabIndex = 1;
            this.flowHeaderRight.Text = "flowHeaderRight";
            // 
            // labelDialogDescription
            // 
            this.labelDialogDescription.Location = new System.Drawing.Point(0, 0);
            this.labelDialogDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDialogDescription.Name = "labelDialogDescription";
            this.labelDialogDescription.Size = new System.Drawing.Size(287, 48);
            this.labelDialogDescription.TabIndex = 0;
            this.labelDialogDescription.Text = "目标用户：- / -";
            this.labelDialogDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // flowHeaderLeft
            // 
            this.flowHeaderLeft.Controls.Add(this.labelDialogTitle);
            this.flowHeaderLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowHeaderLeft.Location = new System.Drawing.Point(4, 0);
            this.flowHeaderLeft.Margin = new System.Windows.Forms.Padding(0);
            this.flowHeaderLeft.Name = "flowHeaderLeft";
            this.flowHeaderLeft.Size = new System.Drawing.Size(220, 48);
            this.flowHeaderLeft.TabIndex = 0;
            this.flowHeaderLeft.Text = "flowHeaderLeft";
            // 
            // labelDialogTitle
            // 
            this.labelDialogTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelDialogTitle.Location = new System.Drawing.Point(0, 0);
            this.labelDialogTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelDialogTitle.Name = "labelDialogTitle";
            this.labelDialogTitle.Size = new System.Drawing.Size(220, 38);
            this.labelDialogTitle.TabIndex = 0;
            this.labelDialogTitle.Text = "重置用户密码";
            // 
            // ResetUserPasswordDialog
            // 
            this.ClientSize = new System.Drawing.Size(560, 460);
            this.Controls.Add(this.textureBackgroundDialog);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ResetUserPasswordDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "重置用户密码";
            this.textureBackgroundDialog.ResumeLayout(false);
            this.panelShell.ResumeLayout(false);
            this.panelContent.ResumeLayout(false);
            this.stackFormRows.ResumeLayout(false);
            this.panelRowNewPassword.ResumeLayout(false);
            this.panelRowConfirmPassword.ResumeLayout(false);
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
        private AntdUI.Panel panelRowNewPassword;
        private AntdUI.Input inputNewPassword;
        private AntdUI.Label labelNewPassword;
        private AntdUI.Panel panelRowConfirmPassword;
        private AntdUI.Input inputConfirmPassword;
        private AntdUI.Label labelConfirmPassword;
        private AntdUI.Panel panelFooter;
        private AntdUI.FlowPanel flowFooterButtons;
        private AntdUI.Button buttonCancel;
        private AntdUI.Button buttonOk;
    }
}