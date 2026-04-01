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
            this.panelBody = new AntdUI.Panel();
            this.inputConfirmPassword = new AntdUI.Input();
            this.labelConfirmPassword = new AntdUI.Label();
            this.inputNewPassword = new AntdUI.Input();
            this.labelNewPassword = new AntdUI.Label();
            this.panelFooter = new AntdUI.Panel();
            this.buttonCancel = new AntdUI.Button();
            this.buttonOk = new AntdUI.Button();
            this.panelBody.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBody
            // 
            this.panelBody.Controls.Add(this.inputConfirmPassword);
            this.panelBody.Controls.Add(this.labelConfirmPassword);
            this.panelBody.Controls.Add(this.inputNewPassword);
            this.panelBody.Controls.Add(this.labelNewPassword);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(0, 0);
            this.panelBody.Name = "panelBody";
            this.panelBody.Padding = new System.Windows.Forms.Padding(24, 20, 24, 8);
            this.panelBody.Radius = 0;
            this.panelBody.Size = new System.Drawing.Size(400, 176);
            this.panelBody.TabIndex = 0;
            // 
            // inputConfirmPassword
            // 
            this.inputConfirmPassword.Location = new System.Drawing.Point(24, 110);
            this.inputConfirmPassword.Name = "inputConfirmPassword";
            this.inputConfirmPassword.PasswordChar = '●';
            this.inputConfirmPassword.PlaceholderText = "请再次输入新密码";
            this.inputConfirmPassword.Size = new System.Drawing.Size(352, 36);
            this.inputConfirmPassword.TabIndex = 3;
            // 
            // labelConfirmPassword
            // 
            this.labelConfirmPassword.Location = new System.Drawing.Point(24, 86);
            this.labelConfirmPassword.Name = "labelConfirmPassword";
            this.labelConfirmPassword.Size = new System.Drawing.Size(80, 24);
            this.labelConfirmPassword.TabIndex = 2;
            this.labelConfirmPassword.Text = "确认密码";
            // 
            // inputNewPassword
            // 
            this.inputNewPassword.Location = new System.Drawing.Point(24, 42);
            this.inputNewPassword.Name = "inputNewPassword";
            this.inputNewPassword.PasswordChar = '●';
            this.inputNewPassword.PlaceholderText = "请输入新密码";
            this.inputNewPassword.Size = new System.Drawing.Size(352, 36);
            this.inputNewPassword.TabIndex = 1;
            // 
            // labelNewPassword
            // 
            this.labelNewPassword.Location = new System.Drawing.Point(24, 18);
            this.labelNewPassword.Name = "labelNewPassword";
            this.labelNewPassword.Size = new System.Drawing.Size(80, 24);
            this.labelNewPassword.TabIndex = 0;
            this.labelNewPassword.Text = "新密码";
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.buttonCancel);
            this.panelFooter.Controls.Add(this.buttonOk);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(0, 176);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Padding = new System.Windows.Forms.Padding(0, 8, 24, 16);
            this.panelFooter.Radius = 0;
            this.panelFooter.Size = new System.Drawing.Size(400, 56);
            this.panelFooter.TabIndex = 1;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(300, 8);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Radius = 8;
            this.buttonCancel.Size = new System.Drawing.Size(76, 32);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.WaveSize = 0;
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.Location = new System.Drawing.Point(216, 8);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Radius = 8;
            this.buttonOk.Size = new System.Drawing.Size(76, 32);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "确定";
            this.buttonOk.Type = AntdUI.TTypeMini.Primary;
            this.buttonOk.WaveSize = 0;
            // 
            // ResetUserPasswordDialog
            // 
            this.ClientSize = new System.Drawing.Size(400, 232);
            this.Controls.Add(this.panelBody);
            this.Controls.Add(this.panelFooter);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ResetUserPasswordDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "重置用户密码";
            this.panelBody.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.Panel panelBody;
        private AntdUI.Input inputConfirmPassword;
        private AntdUI.Label labelConfirmPassword;
        private AntdUI.Input inputNewPassword;
        private AntdUI.Label labelNewPassword;
        private AntdUI.Panel panelFooter;
        private AntdUI.Button buttonCancel;
        private AntdUI.Button buttonOk;
    }
}