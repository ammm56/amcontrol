namespace AMControlWinF.Views.Am
{
    partial class ResetUserPasswordDialog
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelRoot = new AntdUI.Panel();
            this.panelButtons = new AntdUI.Panel();
            this.buttonSubmit = new AntdUI.Button();
            this.buttonCancel = new AntdUI.Button();
            this.panelForm = new AntdUI.Panel();
            this.inputConfirmPassword = new AntdUI.Input();
            this.inputNewPassword = new AntdUI.Input();
            this.labelConfirmPassword = new AntdUI.Label();
            this.labelNewPassword = new AntdUI.Label();
            this.labelLoginNameValue = new AntdUI.Label();
            this.labelLoginNameTitle = new AntdUI.Label();
            this.labelMessage = new AntdUI.Label();
            this.labelTitle = new AntdUI.Label();
            this.panelRoot.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.panelForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.panelForm);
            this.panelRoot.Controls.Add(this.panelButtons);
            this.panelRoot.Controls.Add(this.labelMessage);
            this.panelRoot.Controls.Add(this.labelTitle);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(18);
            this.panelRoot.Radius = 16;
            this.panelRoot.Size = new System.Drawing.Size(604, 281);
            this.panelRoot.TabIndex = 0;
            // 
            // panelButtons
            // 
            this.panelButtons.Back = System.Drawing.Color.Transparent;
            this.panelButtons.Controls.Add(this.buttonSubmit);
            this.panelButtons.Controls.Add(this.buttonCancel);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(24, 209);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Radius = 0;
            this.panelButtons.Size = new System.Drawing.Size(556, 48);
            this.panelButtons.TabIndex = 2;
            // 
            // buttonSubmit
            // 
            this.buttonSubmit.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonSubmit.Location = new System.Drawing.Point(316, 0);
            this.buttonSubmit.Name = "buttonSubmit";
            this.buttonSubmit.Size = new System.Drawing.Size(120, 48);
            this.buttonSubmit.TabIndex = 1;
            this.buttonSubmit.Text = "确定";
            this.buttonSubmit.Type = AntdUI.TTypeMini.Primary;
            this.buttonSubmit.WaveSize = 0;
            this.buttonSubmit.Click += new System.EventHandler(this.buttonSubmit_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonCancel.Location = new System.Drawing.Point(436, 0);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(120, 48);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.WaveSize = 0;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // panelForm
            // 
            this.panelForm.Back = System.Drawing.Color.Transparent;
            this.panelForm.Controls.Add(this.inputConfirmPassword);
            this.panelForm.Controls.Add(this.inputNewPassword);
            this.panelForm.Controls.Add(this.labelConfirmPassword);
            this.panelForm.Controls.Add(this.labelNewPassword);
            this.panelForm.Controls.Add(this.labelLoginNameValue);
            this.panelForm.Controls.Add(this.labelLoginNameTitle);
            this.panelForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelForm.Location = new System.Drawing.Point(24, 70);
            this.panelForm.Name = "panelForm";
            this.panelForm.Radius = 0;
            this.panelForm.Size = new System.Drawing.Size(556, 187);
            this.panelForm.TabIndex = 1;
            // 
            // inputConfirmPassword
            // 
            this.inputConfirmPassword.Location = new System.Drawing.Point(124, 109);
            this.inputConfirmPassword.Name = "inputConfirmPassword";
            this.inputConfirmPassword.PasswordChar = '●';
            this.inputConfirmPassword.Size = new System.Drawing.Size(400, 38);
            this.inputConfirmPassword.TabIndex = 5;
            // 
            // inputNewPassword
            // 
            this.inputNewPassword.Location = new System.Drawing.Point(124, 57);
            this.inputNewPassword.Name = "inputNewPassword";
            this.inputNewPassword.PasswordChar = '●';
            this.inputNewPassword.Size = new System.Drawing.Size(400, 38);
            this.inputNewPassword.TabIndex = 4;
            // 
            // labelConfirmPassword
            // 
            this.labelConfirmPassword.Location = new System.Drawing.Point(0, 113);
            this.labelConfirmPassword.Name = "labelConfirmPassword";
            this.labelConfirmPassword.Size = new System.Drawing.Size(108, 30);
            this.labelConfirmPassword.TabIndex = 3;
            this.labelConfirmPassword.Text = "确认密码";
            // 
            // labelNewPassword
            // 
            this.labelNewPassword.Location = new System.Drawing.Point(0, 61);
            this.labelNewPassword.Name = "labelNewPassword";
            this.labelNewPassword.Size = new System.Drawing.Size(108, 30);
            this.labelNewPassword.TabIndex = 2;
            this.labelNewPassword.Text = "新密码";
            // 
            // labelLoginNameValue
            // 
            this.labelLoginNameValue.Location = new System.Drawing.Point(124, 9);
            this.labelLoginNameValue.Name = "labelLoginNameValue";
            this.labelLoginNameValue.Size = new System.Drawing.Size(400, 30);
            this.labelLoginNameValue.TabIndex = 1;
            this.labelLoginNameValue.Text = "-";
            // 
            // labelLoginNameTitle
            // 
            this.labelLoginNameTitle.Location = new System.Drawing.Point(0, 9);
            this.labelLoginNameTitle.Name = "labelLoginNameTitle";
            this.labelLoginNameTitle.Size = new System.Drawing.Size(108, 30);
            this.labelLoginNameTitle.TabIndex = 0;
            this.labelLoginNameTitle.Text = "用户";
            // 
            // labelMessage
            // 
            this.labelMessage.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelMessage.ForeColor = System.Drawing.Color.IndianRed;
            this.labelMessage.Location = new System.Drawing.Point(24, 42);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(556, 28);
            this.labelMessage.TabIndex = 3;
            // 
            // labelTitle
            // 
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 16F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(24, 24);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(556, 18);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "重置密码";
            // 
            // ResetUserPasswordDialog
            // 
            this.ClientSize = new System.Drawing.Size(604, 281);
            this.ControlBox = false;
            this.Controls.Add(this.panelRoot);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.Name = "ResetUserPasswordDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "重置密码";
            this.panelRoot.ResumeLayout(false);
            this.panelButtons.ResumeLayout(false);
            this.panelForm.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AntdUI.Panel panelRoot;
        private AntdUI.Label labelTitle;
        private AntdUI.Label labelMessage;
        private AntdUI.Panel panelButtons;
        private AntdUI.Button buttonSubmit;
        private AntdUI.Button buttonCancel;
        private AntdUI.Panel panelForm;
        private AntdUI.Label labelLoginNameTitle;
        private AntdUI.Label labelLoginNameValue;
        private AntdUI.Label labelNewPassword;
        private AntdUI.Label labelConfirmPassword;
        private AntdUI.Input inputNewPassword;
        private AntdUI.Input inputConfirmPassword;
    }
}