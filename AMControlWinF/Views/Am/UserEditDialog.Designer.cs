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
            this.panelBody = new AntdUI.Panel();
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
            this.buttonCancel = new AntdUI.Button();
            this.buttonOk = new AntdUI.Button();
            this.panelBody.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBody
            // 
            this.panelBody.Controls.Add(this.inputRemark);
            this.panelBody.Controls.Add(this.labelRemark);
            this.panelBody.Controls.Add(this.checkEnabled);
            this.panelBody.Controls.Add(this.labelEnabled);
            this.panelBody.Controls.Add(this.inputPassword);
            this.panelBody.Controls.Add(this.labelPassword);
            this.panelBody.Controls.Add(this.dropdownRole);
            this.panelBody.Controls.Add(this.labelRole);
            this.panelBody.Controls.Add(this.inputUserName);
            this.panelBody.Controls.Add(this.labelUserName);
            this.panelBody.Controls.Add(this.inputLoginName);
            this.panelBody.Controls.Add(this.labelLoginName);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(0, 0);
            this.panelBody.Name = "panelBody";
            this.panelBody.Padding = new System.Windows.Forms.Padding(24, 18, 24, 8);
            this.panelBody.Radius = 0;
            this.panelBody.Size = new System.Drawing.Size(420, 374);
            this.panelBody.TabIndex = 0;
            // 
            // inputRemark
            // 
            this.inputRemark.Location = new System.Drawing.Point(24, 296);
            this.inputRemark.Name = "inputRemark";
            this.inputRemark.PlaceholderText = "备注（可选）";
            this.inputRemark.Size = new System.Drawing.Size(372, 36);
            this.inputRemark.TabIndex = 11;
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(24, 272);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(80, 24);
            this.labelRemark.TabIndex = 10;
            this.labelRemark.Text = "备注";
            // 
            // checkEnabled
            // 
            this.checkEnabled.AutoSize = true;
            this.checkEnabled.Checked = true;
            this.checkEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkEnabled.Location = new System.Drawing.Point(84, 241);
            this.checkEnabled.Name = "checkEnabled";
            this.checkEnabled.Size = new System.Drawing.Size(59, 24);
            this.checkEnabled.TabIndex = 9;
            this.checkEnabled.Text = "启用";
            this.checkEnabled.UseVisualStyleBackColor = true;
            // 
            // labelEnabled
            // 
            this.labelEnabled.Location = new System.Drawing.Point(24, 240);
            this.labelEnabled.Name = "labelEnabled";
            this.labelEnabled.Size = new System.Drawing.Size(54, 24);
            this.labelEnabled.TabIndex = 8;
            this.labelEnabled.Text = "状态";
            // 
            // inputPassword
            // 
            this.inputPassword.Location = new System.Drawing.Point(24, 194);
            this.inputPassword.Name = "inputPassword";
            this.inputPassword.PasswordChar = '●';
            this.inputPassword.PlaceholderText = "请输入初始密码";
            this.inputPassword.Size = new System.Drawing.Size(372, 36);
            this.inputPassword.TabIndex = 7;
            // 
            // labelPassword
            // 
            this.labelPassword.Location = new System.Drawing.Point(24, 170);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(80, 24);
            this.labelPassword.TabIndex = 6;
            this.labelPassword.Text = "初始密码";
            // 
            // dropdownRole
            // 
            this.dropdownRole.Location = new System.Drawing.Point(24, 130);
            this.dropdownRole.Name = "dropdownRole";
            this.dropdownRole.Size = new System.Drawing.Size(372, 34);
            this.dropdownRole.TabIndex = 5;
            this.dropdownRole.Trigger = AntdUI.Trigger.Click;
            // 
            // labelRole
            // 
            this.labelRole.Location = new System.Drawing.Point(24, 106);
            this.labelRole.Name = "labelRole";
            this.labelRole.Size = new System.Drawing.Size(80, 24);
            this.labelRole.TabIndex = 4;
            this.labelRole.Text = "角色";
            // 
            // inputUserName
            // 
            this.inputUserName.Location = new System.Drawing.Point(24, 66);
            this.inputUserName.Name = "inputUserName";
            this.inputUserName.PlaceholderText = "请输入用户名";
            this.inputUserName.Size = new System.Drawing.Size(372, 36);
            this.inputUserName.TabIndex = 3;
            // 
            // labelUserName
            // 
            this.labelUserName.Location = new System.Drawing.Point(24, 42);
            this.labelUserName.Name = "labelUserName";
            this.labelUserName.Size = new System.Drawing.Size(80, 24);
            this.labelUserName.TabIndex = 2;
            this.labelUserName.Text = "用户名";
            // 
            // inputLoginName
            // 
            this.inputLoginName.Location = new System.Drawing.Point(24, 18);
            this.inputLoginName.Name = "inputLoginName";
            this.inputLoginName.PlaceholderText = "请输入登录名";
            this.inputLoginName.Size = new System.Drawing.Size(372, 36);
            this.inputLoginName.TabIndex = 1;
            // 
            // labelLoginName
            // 
            this.labelLoginName.Location = new System.Drawing.Point(24, -6);
            this.labelLoginName.Name = "labelLoginName";
            this.labelLoginName.Size = new System.Drawing.Size(80, 24);
            this.labelLoginName.TabIndex = 0;
            this.labelLoginName.Text = "登录名";
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.buttonCancel);
            this.panelFooter.Controls.Add(this.buttonOk);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(0, 374);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Padding = new System.Windows.Forms.Padding(0, 8, 24, 16);
            this.panelFooter.Radius = 0;
            this.panelFooter.Size = new System.Drawing.Size(420, 56);
            this.panelFooter.TabIndex = 1;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(320, 8);
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
            this.buttonOk.Location = new System.Drawing.Point(236, 8);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Radius = 8;
            this.buttonOk.Size = new System.Drawing.Size(76, 32);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "确定";
            this.buttonOk.Type = AntdUI.TTypeMini.Primary;
            this.buttonOk.WaveSize = 0;
            // 
            // UserEditDialog
            // 
            this.ClientSize = new System.Drawing.Size(420, 430);
            this.Controls.Add(this.panelBody);
            this.Controls.Add(this.panelFooter);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserEditDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "用户信息";
            this.panelBody.ResumeLayout(false);
            this.panelBody.PerformLayout();
            this.panelFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.Panel panelBody;
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
        private AntdUI.Button buttonCancel;
        private AntdUI.Button buttonOk;
    }
}