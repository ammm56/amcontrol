namespace AMControlWinF.Views.Am
{
    partial class UserEditDialog
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
            this.textBoxRemark = new System.Windows.Forms.TextBox();
            this.checkBoxEnabled = new System.Windows.Forms.CheckBox();
            this.dropdownRole = new AntdUI.Dropdown();
            this.inputPassword = new AntdUI.Input();
            this.inputUserName = new AntdUI.Input();
            this.inputLoginName = new AntdUI.Input();
            this.labelRemark = new AntdUI.Label();
            this.labelEnabled = new AntdUI.Label();
            this.labelRole = new AntdUI.Label();
            this.labelPassword = new AntdUI.Label();
            this.labelUserName = new AntdUI.Label();
            this.labelLoginName = new AntdUI.Label();
            this.labelMessage = new AntdUI.Label();
            this.labelTitle = new AntdUI.Label();
            this.panelRoot.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.panelForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Back = System.Drawing.Color.White;
            this.panelRoot.Controls.Add(this.panelForm);
            this.panelRoot.Controls.Add(this.panelButtons);
            this.panelRoot.Controls.Add(this.labelMessage);
            this.panelRoot.Controls.Add(this.labelTitle);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(18);
            this.panelRoot.Radius = 16;
            this.panelRoot.Size = new System.Drawing.Size(704, 521);
            this.panelRoot.TabIndex = 0;
            // 
            // panelButtons
            // 
            this.panelButtons.Back = System.Drawing.Color.Transparent;
            this.panelButtons.Controls.Add(this.buttonSubmit);
            this.panelButtons.Controls.Add(this.buttonCancel);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(24, 449);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Radius = 0;
            this.panelButtons.Size = new System.Drawing.Size(656, 48);
            this.panelButtons.TabIndex = 2;
            // 
            // buttonSubmit
            // 
            this.buttonSubmit.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonSubmit.Location = new System.Drawing.Point(416, 0);
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
            this.buttonCancel.Location = new System.Drawing.Point(536, 0);
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
            this.panelForm.Controls.Add(this.textBoxRemark);
            this.panelForm.Controls.Add(this.checkBoxEnabled);
            this.panelForm.Controls.Add(this.dropdownRole);
            this.panelForm.Controls.Add(this.inputPassword);
            this.panelForm.Controls.Add(this.inputUserName);
            this.panelForm.Controls.Add(this.inputLoginName);
            this.panelForm.Controls.Add(this.labelRemark);
            this.panelForm.Controls.Add(this.labelEnabled);
            this.panelForm.Controls.Add(this.labelRole);
            this.panelForm.Controls.Add(this.labelPassword);
            this.panelForm.Controls.Add(this.labelUserName);
            this.panelForm.Controls.Add(this.labelLoginName);
            this.panelForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelForm.Location = new System.Drawing.Point(24, 62);
            this.panelForm.Name = "panelForm";
            this.panelForm.Radius = 0;
            this.panelForm.Size = new System.Drawing.Size(656, 435);
            this.panelForm.TabIndex = 1;
            // 
            // textBoxRemark
            // 
            this.textBoxRemark.Location = new System.Drawing.Point(124, 249);
            this.textBoxRemark.Multiline = true;
            this.textBoxRemark.Name = "textBoxRemark";
            this.textBoxRemark.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxRemark.Size = new System.Drawing.Size(500, 120);
            this.textBoxRemark.TabIndex = 11;
            // 
            // checkBoxEnabled
            // 
            this.checkBoxEnabled.AutoSize = true;
            this.checkBoxEnabled.Location = new System.Drawing.Point(124, 208);
            this.checkBoxEnabled.Name = "checkBoxEnabled";
            this.checkBoxEnabled.Size = new System.Drawing.Size(91, 24);
            this.checkBoxEnabled.TabIndex = 10;
            this.checkBoxEnabled.Text = "启用用户";
            this.checkBoxEnabled.UseVisualStyleBackColor = true;
            // 
            // dropdownRole
            // 
            this.dropdownRole.Location = new System.Drawing.Point(124, 154);
            this.dropdownRole.Name = "dropdownRole";
            this.dropdownRole.Placement = AntdUI.TAlignFrom.BL;
            this.dropdownRole.Size = new System.Drawing.Size(500, 38);
            this.dropdownRole.TabIndex = 9;
            // 
            // inputPassword
            // 
            this.inputPassword.Location = new System.Drawing.Point(124, 103);
            this.inputPassword.Name = "inputPassword";
            this.inputPassword.PasswordChar = '●';
            this.inputPassword.Size = new System.Drawing.Size(500, 38);
            this.inputPassword.TabIndex = 8;
            // 
            // inputUserName
            // 
            this.inputUserName.Location = new System.Drawing.Point(124, 52);
            this.inputUserName.Name = "inputUserName";
            this.inputUserName.Size = new System.Drawing.Size(500, 38);
            this.inputUserName.TabIndex = 7;
            // 
            // inputLoginName
            // 
            this.inputLoginName.Location = new System.Drawing.Point(124, 1);
            this.inputLoginName.Name = "inputLoginName";
            this.inputLoginName.Size = new System.Drawing.Size(500, 38);
            this.inputLoginName.TabIndex = 6;
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(0, 249);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(108, 30);
            this.labelRemark.TabIndex = 5;
            this.labelRemark.Text = "备注";
            // 
            // labelEnabled
            // 
            this.labelEnabled.Location = new System.Drawing.Point(0, 206);
            this.labelEnabled.Name = "labelEnabled";
            this.labelEnabled.Size = new System.Drawing.Size(108, 30);
            this.labelEnabled.TabIndex = 4;
            this.labelEnabled.Text = "状态";
            // 
            // labelRole
            // 
            this.labelRole.Location = new System.Drawing.Point(0, 156);
            this.labelRole.Name = "labelRole";
            this.labelRole.Size = new System.Drawing.Size(108, 30);
            this.labelRole.TabIndex = 3;
            this.labelRole.Text = "角色";
            // 
            // labelPassword
            // 
            this.labelPassword.Location = new System.Drawing.Point(0, 105);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(108, 30);
            this.labelPassword.TabIndex = 2;
            this.labelPassword.Text = "初始密码";
            // 
            // labelUserName
            // 
            this.labelUserName.Location = new System.Drawing.Point(0, 54);
            this.labelUserName.Name = "labelUserName";
            this.labelUserName.Size = new System.Drawing.Size(108, 30);
            this.labelUserName.TabIndex = 1;
            this.labelUserName.Text = "用户名";
            // 
            // labelLoginName
            // 
            this.labelLoginName.Location = new System.Drawing.Point(0, 3);
            this.labelLoginName.Name = "labelLoginName";
            this.labelLoginName.Size = new System.Drawing.Size(108, 30);
            this.labelLoginName.TabIndex = 0;
            this.labelLoginName.Text = "登录名";
            // 
            // labelMessage
            // 
            this.labelMessage.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelMessage.ForeColor = System.Drawing.Color.IndianRed;
            this.labelMessage.Location = new System.Drawing.Point(24, 34);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(656, 28);
            this.labelMessage.TabIndex = 3;
            // 
            // labelTitle
            // 
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 16F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(24, 24);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(656, 10);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "新增用户";
            // 
            // UserEditDialog
            // 
            this.ClientSize = new System.Drawing.Size(704, 521);
            this.ControlBox = false;
            this.Controls.Add(this.panelRoot);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.Name = "UserEditDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "用户";
            this.panelRoot.ResumeLayout(false);
            this.panelButtons.ResumeLayout(false);
            this.panelForm.ResumeLayout(false);
            this.panelForm.PerformLayout();
            this.ResumeLayout(false);

        }

        private AntdUI.Panel panelRoot;
        private AntdUI.Label labelTitle;
        private AntdUI.Label labelMessage;
        private AntdUI.Panel panelButtons;
        private AntdUI.Button buttonSubmit;
        private AntdUI.Button buttonCancel;
        private AntdUI.Panel panelForm;
        private AntdUI.Label labelLoginName;
        private AntdUI.Label labelUserName;
        private AntdUI.Label labelPassword;
        private AntdUI.Label labelRole;
        private AntdUI.Label labelEnabled;
        private AntdUI.Label labelRemark;
        private AntdUI.Input inputLoginName;
        private AntdUI.Input inputUserName;
        private AntdUI.Input inputPassword;
        private AntdUI.Dropdown dropdownRole;
        private System.Windows.Forms.CheckBox checkBoxEnabled;
        private System.Windows.Forms.TextBox textBoxRemark;
    }
}