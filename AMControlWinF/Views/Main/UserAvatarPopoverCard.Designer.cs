namespace AMControlWinF.Views.Main
{
    partial class UserAvatarPopoverCard
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panelRoot = new AntdUI.Panel();
            this.panelActions = new System.Windows.Forms.Panel();
            this.buttonLogout = new AntdUI.Button();
            this.buttonChangePassword = new AntdUI.Button();
            this.buttonSwitchUser = new AntdUI.Button();
            this.separatorHeader = new System.Windows.Forms.Panel();
            this.panelHeader = new AntdUI.Panel();
            this.labelRoleDisplayName = new AntdUI.Label();
            this.labelUserDisplayName = new AntdUI.Label();
            this.panelRoot.SuspendLayout();
            this.panelActions.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.BackColor = System.Drawing.Color.Transparent;
            this.panelRoot.Controls.Add(this.panelActions);
            this.panelRoot.Controls.Add(this.separatorHeader);
            this.panelRoot.Controls.Add(this.panelHeader);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Margin = new System.Windows.Forms.Padding(0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(6);
            this.panelRoot.Radius = 14;
            this.panelRoot.Shadow = 8;
            this.panelRoot.Size = new System.Drawing.Size(220, 220);
            this.panelRoot.TabIndex = 0;
            // 
            // panelActions
            // 
            this.panelActions.BackColor = System.Drawing.Color.Transparent;
            this.panelActions.Controls.Add(this.buttonLogout);
            this.panelActions.Controls.Add(this.buttonChangePassword);
            this.panelActions.Controls.Add(this.buttonSwitchUser);
            this.panelActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelActions.Location = new System.Drawing.Point(18, 75);
            this.panelActions.Margin = new System.Windows.Forms.Padding(0);
            this.panelActions.Name = "panelActions";
            this.panelActions.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.panelActions.Size = new System.Drawing.Size(184, 127);
            this.panelActions.TabIndex = 2;
            // 
            // buttonLogout
            // 
            this.buttonLogout.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonLogout.Ghost = true;
            this.buttonLogout.IconSvg = "LogoutOutlined";
            this.buttonLogout.Location = new System.Drawing.Point(0, 90);
            this.buttonLogout.Margin = new System.Windows.Forms.Padding(0);
            this.buttonLogout.Name = "buttonLogout";
            this.buttonLogout.Radius = 10;
            this.buttonLogout.Size = new System.Drawing.Size(184, 40);
            this.buttonLogout.TabIndex = 3;
            this.buttonLogout.Text = "退出登录";
            this.buttonLogout.WaveSize = 0;
            // 
            // buttonChangePassword
            // 
            this.buttonChangePassword.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonChangePassword.Ghost = true;
            this.buttonChangePassword.IconSvg = "LockOutlined";
            this.buttonChangePassword.Location = new System.Drawing.Point(0, 50);
            this.buttonChangePassword.Margin = new System.Windows.Forms.Padding(0);
            this.buttonChangePassword.Name = "buttonChangePassword";
            this.buttonChangePassword.Radius = 10;
            this.buttonChangePassword.Size = new System.Drawing.Size(184, 40);
            this.buttonChangePassword.TabIndex = 2;
            this.buttonChangePassword.Text = "修改密码";
            this.buttonChangePassword.WaveSize = 0;
            // 
            // buttonSwitchUser
            // 
            this.buttonSwitchUser.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonSwitchUser.Ghost = true;
            this.buttonSwitchUser.IconSvg = "SwapOutlined";
            this.buttonSwitchUser.Location = new System.Drawing.Point(0, 10);
            this.buttonSwitchUser.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSwitchUser.Name = "buttonSwitchUser";
            this.buttonSwitchUser.Radius = 10;
            this.buttonSwitchUser.Size = new System.Drawing.Size(184, 40);
            this.buttonSwitchUser.TabIndex = 1;
            this.buttonSwitchUser.Text = "切换用户";
            this.buttonSwitchUser.WaveSize = 0;
            // 
            // separatorHeader
            // 
            this.separatorHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.separatorHeader.Location = new System.Drawing.Point(18, 74);
            this.separatorHeader.Margin = new System.Windows.Forms.Padding(0);
            this.separatorHeader.Name = "separatorHeader";
            this.separatorHeader.Size = new System.Drawing.Size(184, 1);
            this.separatorHeader.TabIndex = 1;
            this.separatorHeader.Visible = false;
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.Transparent;
            this.panelHeader.Controls.Add(this.labelRoleDisplayName);
            this.panelHeader.Controls.Add(this.labelUserDisplayName);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(18, 18);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Padding = new System.Windows.Forms.Padding(0, 4, 0, 6);
            this.panelHeader.Radius = 0;
            this.panelHeader.Size = new System.Drawing.Size(184, 56);
            this.panelHeader.TabIndex = 0;
            // 
            // labelRoleDisplayName
            // 
            this.labelRoleDisplayName.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelRoleDisplayName.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelRoleDisplayName.Location = new System.Drawing.Point(0, 30);
            this.labelRoleDisplayName.Margin = new System.Windows.Forms.Padding(0);
            this.labelRoleDisplayName.Name = "labelRoleDisplayName";
            this.labelRoleDisplayName.Size = new System.Drawing.Size(184, 22);
            this.labelRoleDisplayName.TabIndex = 1;
            this.labelRoleDisplayName.Text = "用户";
            // 
            // labelUserDisplayName
            // 
            this.labelUserDisplayName.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelUserDisplayName.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold);
            this.labelUserDisplayName.Location = new System.Drawing.Point(0, 4);
            this.labelUserDisplayName.Margin = new System.Windows.Forms.Padding(0);
            this.labelUserDisplayName.Name = "labelUserDisplayName";
            this.labelUserDisplayName.Size = new System.Drawing.Size(184, 26);
            this.labelUserDisplayName.TabIndex = 0;
            this.labelUserDisplayName.Text = "未登录";
            // 
            // UserAvatarPopoverCard
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "UserAvatarPopoverCard";
            this.Size = new System.Drawing.Size(220, 220);
            this.panelRoot.ResumeLayout(false);
            this.panelActions.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.Panel panelRoot;
        private AntdUI.Panel panelHeader;
        private AntdUI.Label labelUserDisplayName;
        private AntdUI.Label labelRoleDisplayName;
        private System.Windows.Forms.Panel separatorHeader;
        private System.Windows.Forms.Panel panelActions;
        private AntdUI.Button buttonSwitchUser;
        private AntdUI.Button buttonChangePassword;
        private AntdUI.Button buttonLogout;
    }
}