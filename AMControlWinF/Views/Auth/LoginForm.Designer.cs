namespace AMControlWinF.Views.Auth
{
    partial class LoginForm
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

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.textureBackgroundLogin = new AMControlWinF.Views.Main.TextureBackgroundControl();
            this.panelShell = new AntdUI.Panel();
            this.panelLogin = new AntdUI.Panel();
            this.labelStatusValue = new AntdUI.Label();
            this.buttonLogin = new AntdUI.Button();
            this.buttonCancel = new AntdUI.Button();
            this.labelErrorValue = new AntdUI.Label();
            this.textBoxPassword = new AntdUI.Input();
            this.buttonPasswordIcon = new AntdUI.Button();
            this.labelPassword = new AntdUI.Label();
            this.textBoxLoginName = new AntdUI.Input();
            this.buttonLoginNameIcon = new AntdUI.Button();
            this.labelLoginName = new AntdUI.Label();
            this.labelLoginTitle = new AntdUI.Label();
            this.panelIntro = new AntdUI.Panel();
            this.labelIntroBottom = new AntdUI.Label();
            this.panelIntroCard = new AntdUI.Panel();
            this.labelIntroAdmin = new AntdUI.Label();
            this.labelIntroEngineer = new AntdUI.Label();
            this.labelIntroOperator = new AntdUI.Label();
            this.labelIntroCardTitle = new AntdUI.Label();
            this.labelIntroSubtitle = new AntdUI.Label();
            this.labelIntroTitle = new AntdUI.Label();
            this.textureBackgroundLogin.SuspendLayout();
            this.panelShell.SuspendLayout();
            this.panelLogin.SuspendLayout();
            this.panelIntro.SuspendLayout();
            this.panelIntroCard.SuspendLayout();
            this.SuspendLayout();
            // 
            // textureBackgroundLogin
            // 
            this.textureBackgroundLogin.Controls.Add(this.panelShell);
            this.textureBackgroundLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textureBackgroundLogin.Location = new System.Drawing.Point(0, 0);
            this.textureBackgroundLogin.Margin = new System.Windows.Forms.Padding(0);
            this.textureBackgroundLogin.Name = "textureBackgroundLogin";
            this.textureBackgroundLogin.Size = new System.Drawing.Size(880, 520);
            this.textureBackgroundLogin.TabIndex = 0;
            // 
            // panelShell
            // 
            this.panelShell.BackColor = System.Drawing.Color.Transparent;
            this.panelShell.Controls.Add(this.panelLogin);
            this.panelShell.Controls.Add(this.panelIntro);
            this.panelShell.Location = new System.Drawing.Point(24, 24);
            this.panelShell.Margin = new System.Windows.Forms.Padding(0);
            this.panelShell.Name = "panelShell";
            this.panelShell.Padding = new System.Windows.Forms.Padding(12);
            this.panelShell.Radius = 16;
            this.panelShell.Shadow = 8;
            this.panelShell.Size = new System.Drawing.Size(832, 472);
            this.panelShell.TabIndex = 0;
            // 
            // panelLogin
            // 
            this.panelLogin.Controls.Add(this.labelStatusValue);
            this.panelLogin.Controls.Add(this.buttonLogin);
            this.panelLogin.Controls.Add(this.buttonCancel);
            this.panelLogin.Controls.Add(this.labelErrorValue);
            this.panelLogin.Controls.Add(this.textBoxPassword);
            this.panelLogin.Controls.Add(this.buttonPasswordIcon);
            this.panelLogin.Controls.Add(this.labelPassword);
            this.panelLogin.Controls.Add(this.textBoxLoginName);
            this.panelLogin.Controls.Add(this.buttonLoginNameIcon);
            this.panelLogin.Controls.Add(this.labelLoginName);
            this.panelLogin.Controls.Add(this.labelLoginTitle);
            this.panelLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLogin.Location = new System.Drawing.Point(449, 24);
            this.panelLogin.Margin = new System.Windows.Forms.Padding(0);
            this.panelLogin.Name = "panelLogin";
            this.panelLogin.Radius = 0;
            this.panelLogin.Size = new System.Drawing.Size(359, 424);
            this.panelLogin.TabIndex = 1;
            // 
            // labelStatusValue
            // 
            this.labelStatusValue.BackColor = System.Drawing.Color.Transparent;
            this.labelStatusValue.Location = new System.Drawing.Point(36, 322);
            this.labelStatusValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelStatusValue.Name = "labelStatusValue";
            this.labelStatusValue.Size = new System.Drawing.Size(323, 24);
            this.labelStatusValue.TabIndex = 10;
            this.labelStatusValue.Text = "请输入登录信息";
            // 
            // buttonLogin
            // 
            this.buttonLogin.IconSvg = "LoginOutlined";
            this.buttonLogin.Location = new System.Drawing.Point(223, 266);
            this.buttonLogin.Margin = new System.Windows.Forms.Padding(0);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Radius = 8;
            this.buttonLogin.Size = new System.Drawing.Size(136, 40);
            this.buttonLogin.TabIndex = 3;
            this.buttonLogin.Text = "登录";
            this.buttonLogin.Type = AntdUI.TTypeMini.Primary;
            this.buttonLogin.WaveSize = 0;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(36, 266);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Radius = 8;
            this.buttonCancel.Size = new System.Drawing.Size(136, 40);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.WaveSize = 0;
            // 
            // labelErrorValue
            // 
            this.labelErrorValue.BackColor = System.Drawing.Color.Transparent;
            this.labelErrorValue.Location = new System.Drawing.Point(36, 208);
            this.labelErrorValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelErrorValue.Name = "labelErrorValue";
            this.labelErrorValue.Size = new System.Drawing.Size(323, 36);
            this.labelErrorValue.TabIndex = 9;
            this.labelErrorValue.Text = "";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(84, 152);
            this.textBoxPassword.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '●';
            this.textBoxPassword.Size = new System.Drawing.Size(275, 38);
            this.textBoxPassword.TabIndex = 1;
            this.textBoxPassword.Text = "am123";
            // 
            // buttonPasswordIcon
            // 
            this.buttonPasswordIcon.Ghost = true;
            this.buttonPasswordIcon.IconSvg = "LockOutlined";
            this.buttonPasswordIcon.Location = new System.Drawing.Point(36, 152);
            this.buttonPasswordIcon.Margin = new System.Windows.Forms.Padding(0);
            this.buttonPasswordIcon.Name = "buttonPasswordIcon";
            this.buttonPasswordIcon.Radius = 8;
            this.buttonPasswordIcon.Size = new System.Drawing.Size(40, 38);
            this.buttonPasswordIcon.TabIndex = 8;
            this.buttonPasswordIcon.WaveSize = 0;
            // 
            // labelPassword
            // 
            this.labelPassword.BackColor = System.Drawing.Color.Transparent;
            this.labelPassword.Location = new System.Drawing.Point(36, 122);
            this.labelPassword.Margin = new System.Windows.Forms.Padding(0);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(72, 22);
            this.labelPassword.TabIndex = 7;
            this.labelPassword.Text = "密码";
            // 
            // textBoxLoginName
            // 
            this.textBoxLoginName.Location = new System.Drawing.Point(84, 84);
            this.textBoxLoginName.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxLoginName.Name = "textBoxLoginName";
            this.textBoxLoginName.Size = new System.Drawing.Size(275, 38);
            this.textBoxLoginName.TabIndex = 0;
            this.textBoxLoginName.Text = "am";
            // 
            // buttonLoginNameIcon
            // 
            this.buttonLoginNameIcon.Ghost = true;
            this.buttonLoginNameIcon.IconSvg = "UserOutlined";
            this.buttonLoginNameIcon.Location = new System.Drawing.Point(36, 84);
            this.buttonLoginNameIcon.Margin = new System.Windows.Forms.Padding(0);
            this.buttonLoginNameIcon.Name = "buttonLoginNameIcon";
            this.buttonLoginNameIcon.Radius = 8;
            this.buttonLoginNameIcon.Size = new System.Drawing.Size(40, 38);
            this.buttonLoginNameIcon.TabIndex = 5;
            this.buttonLoginNameIcon.WaveSize = 0;
            // 
            // labelLoginName
            // 
            this.labelLoginName.BackColor = System.Drawing.Color.Transparent;
            this.labelLoginName.Location = new System.Drawing.Point(36, 54);
            this.labelLoginName.Margin = new System.Windows.Forms.Padding(0);
            this.labelLoginName.Name = "labelLoginName";
            this.labelLoginName.Size = new System.Drawing.Size(72, 22);
            this.labelLoginName.TabIndex = 4;
            this.labelLoginName.Text = "登录名";
            // 
            // labelLoginTitle
            // 
            this.labelLoginTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelLoginTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelLoginTitle.Location = new System.Drawing.Point(36, 24);
            this.labelLoginTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelLoginTitle.Name = "labelLoginTitle";
            this.labelLoginTitle.Size = new System.Drawing.Size(120, 36);
            this.labelLoginTitle.TabIndex = 0;
            this.labelLoginTitle.Text = "登录";
            // 
            // panelIntro
            // 
            this.panelIntro.Controls.Add(this.labelIntroBottom);
            this.panelIntro.Controls.Add(this.panelIntroCard);
            this.panelIntro.Controls.Add(this.labelIntroSubtitle);
            this.panelIntro.Controls.Add(this.labelIntroTitle);
            this.panelIntro.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelIntro.Location = new System.Drawing.Point(24, 24);
            this.panelIntro.Margin = new System.Windows.Forms.Padding(0);
            this.panelIntro.Name = "panelIntro";
            this.panelIntro.Radius = 0;
            this.panelIntro.Size = new System.Drawing.Size(425, 424);
            this.panelIntro.TabIndex = 0;
            // 
            // labelIntroBottom
            // 
            this.labelIntroBottom.BackColor = System.Drawing.Color.Transparent;
            this.labelIntroBottom.Location = new System.Drawing.Point(28, 394);
            this.labelIntroBottom.Margin = new System.Windows.Forms.Padding(0);
            this.labelIntroBottom.Name = "labelIntroBottom";
            this.labelIntroBottom.Size = new System.Drawing.Size(220, 22);
            this.labelIntroBottom.TabIndex = 3;
            this.labelIntroBottom.Text = "请使用有效账户登录系统";
            // 
            // panelIntroCard
            // 
            this.panelIntroCard.Controls.Add(this.labelIntroAdmin);
            this.panelIntroCard.Controls.Add(this.labelIntroEngineer);
            this.panelIntroCard.Controls.Add(this.labelIntroOperator);
            this.panelIntroCard.Controls.Add(this.labelIntroCardTitle);
            this.panelIntroCard.Location = new System.Drawing.Point(28, 154);
            this.panelIntroCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelIntroCard.Name = "panelIntroCard";
            this.panelIntroCard.Padding = new System.Windows.Forms.Padding(16);
            this.panelIntroCard.Radius = 12;
            this.panelIntroCard.Size = new System.Drawing.Size(348, 168);
            this.panelIntroCard.TabIndex = 2;
            // 
            // labelIntroAdmin
            // 
            this.labelIntroAdmin.BackColor = System.Drawing.Color.Transparent;
            this.labelIntroAdmin.Location = new System.Drawing.Point(16, 116);
            this.labelIntroAdmin.Margin = new System.Windows.Forms.Padding(0);
            this.labelIntroAdmin.Name = "labelIntroAdmin";
            this.labelIntroAdmin.Size = new System.Drawing.Size(300, 22);
            this.labelIntroAdmin.TabIndex = 3;
            this.labelIntroAdmin.Text = "• 管理员：增加配置与系统管理能力";
            // 
            // labelIntroEngineer
            // 
            this.labelIntroEngineer.BackColor = System.Drawing.Color.Transparent;
            this.labelIntroEngineer.Location = new System.Drawing.Point(16, 88);
            this.labelIntroEngineer.Margin = new System.Windows.Forms.Padding(0);
            this.labelIntroEngineer.Name = "labelIntroEngineer";
            this.labelIntroEngineer.Size = new System.Drawing.Size(300, 22);
            this.labelIntroEngineer.TabIndex = 2;
            this.labelIntroEngineer.Text = "• 工程师：增加视觉、PLC、工程调试";
            // 
            // labelIntroOperator
            // 
            this.labelIntroOperator.BackColor = System.Drawing.Color.Transparent;
            this.labelIntroOperator.Location = new System.Drawing.Point(16, 60);
            this.labelIntroOperator.Margin = new System.Windows.Forms.Padding(0);
            this.labelIntroOperator.Name = "labelIntroOperator";
            this.labelIntroOperator.Size = new System.Drawing.Size(300, 22);
            this.labelIntroOperator.TabIndex = 1;
            this.labelIntroOperator.Text = "• 操作员：生产、运动、IO、报警日志";
            // 
            // labelIntroCardTitle
            // 
            this.labelIntroCardTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelIntroCardTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelIntroCardTitle.Location = new System.Drawing.Point(16, 16);
            this.labelIntroCardTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelIntroCardTitle.Name = "labelIntroCardTitle";
            this.labelIntroCardTitle.Size = new System.Drawing.Size(316, 28);
            this.labelIntroCardTitle.TabIndex = 0;
            this.labelIntroCardTitle.Text = "登录后将根据用户角色显示不同工作区";
            // 
            // labelIntroSubtitle
            // 
            this.labelIntroSubtitle.BackColor = System.Drawing.Color.Transparent;
            this.labelIntroSubtitle.Location = new System.Drawing.Point(28, 92);
            this.labelIntroSubtitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelIntroSubtitle.Name = "labelIntroSubtitle";
            this.labelIntroSubtitle.Size = new System.Drawing.Size(300, 24);
            this.labelIntroSubtitle.TabIndex = 1;
            this.labelIntroSubtitle.Text = "设备控制 · 用户登录 · 权限访问";
            // 
            // labelIntroTitle
            // 
            this.labelIntroTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelIntroTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 24F, System.Drawing.FontStyle.Bold);
            this.labelIntroTitle.Location = new System.Drawing.Point(28, 48);
            this.labelIntroTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelIntroTitle.Name = "labelIntroTitle";
            this.labelIntroTitle.Size = new System.Drawing.Size(260, 40);
            this.labelIntroTitle.TabIndex = 0;
            this.labelIntroTitle.Text = "AM运动控制";
            // 
            // LoginForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(880, 520);
            this.ControlBox = false;
            this.Controls.Add(this.textureBackgroundLogin);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MaximumSize = new System.Drawing.Size(880, 520);
            this.MinimumSize = new System.Drawing.Size(880, 520);
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "用户登录";
            this.textureBackgroundLogin.ResumeLayout(false);
            this.panelShell.ResumeLayout(false);
            this.panelLogin.ResumeLayout(false);
            this.panelIntro.ResumeLayout(false);
            this.panelIntroCard.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AMControlWinF.Views.Main.TextureBackgroundControl textureBackgroundLogin;
        private AntdUI.Panel panelShell;
        private AntdUI.Panel panelIntro;
        private AntdUI.Label labelIntroTitle;
        private AntdUI.Label labelIntroSubtitle;
        private AntdUI.Panel panelIntroCard;
        private AntdUI.Label labelIntroCardTitle;
        private AntdUI.Label labelIntroOperator;
        private AntdUI.Label labelIntroEngineer;
        private AntdUI.Label labelIntroAdmin;
        private AntdUI.Label labelIntroBottom;
        private AntdUI.Panel panelLogin;
        private AntdUI.Label labelLoginTitle;
        private AntdUI.Label labelLoginName;
        private AntdUI.Button buttonLoginNameIcon;
        private AntdUI.Input textBoxLoginName;
        private AntdUI.Label labelPassword;
        private AntdUI.Button buttonPasswordIcon;
        private AntdUI.Input textBoxPassword;
        private AntdUI.Label labelErrorValue;
        private AntdUI.Button buttonCancel;
        private AntdUI.Button buttonLogin;
        private AntdUI.Label labelStatusValue;
    }
}