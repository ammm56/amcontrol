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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserAvatarPopoverCard));
            this.avatarPopupUser = new AntdUI.Avatar();
            this.labelUserDisplayName = new AntdUI.Label();
            this.labelRoleDisplayName = new AntdUI.Label();
            this.buttonSwitchUser = new AntdUI.Button();
            this.buttonChangePassword = new AntdUI.Button();
            this.buttonLogout = new AntdUI.Button();
            this.SuspendLayout();
            // 
            // avatarPopupUser
            // 
            this.avatarPopupUser.ImageSvg = resources.GetString("avatarPopupUser.ImageSvg");
            this.avatarPopupUser.Location = new System.Drawing.Point(16, 14);
            this.avatarPopupUser.Margin = new System.Windows.Forms.Padding(0);
            this.avatarPopupUser.Name = "avatarPopupUser";
            this.avatarPopupUser.Padding = new System.Windows.Forms.Padding(4);
            this.avatarPopupUser.Radius = 21;
            this.avatarPopupUser.Size = new System.Drawing.Size(42, 42);
            this.avatarPopupUser.TabIndex = 0;
            this.avatarPopupUser.Text = "A";
            // 
            // labelUserDisplayName
            // 
            this.labelUserDisplayName.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.labelUserDisplayName.Location = new System.Drawing.Point(68, 14);
            this.labelUserDisplayName.Margin = new System.Windows.Forms.Padding(0);
            this.labelUserDisplayName.Name = "labelUserDisplayName";
            this.labelUserDisplayName.Size = new System.Drawing.Size(180, 22);
            this.labelUserDisplayName.TabIndex = 1;
            this.labelUserDisplayName.Text = "当前用户";
            // 
            // labelRoleDisplayName
            // 
            this.labelRoleDisplayName.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelRoleDisplayName.Location = new System.Drawing.Point(68, 40);
            this.labelRoleDisplayName.Margin = new System.Windows.Forms.Padding(0);
            this.labelRoleDisplayName.Name = "labelRoleDisplayName";
            this.labelRoleDisplayName.Size = new System.Drawing.Size(180, 18);
            this.labelRoleDisplayName.TabIndex = 2;
            this.labelRoleDisplayName.Text = "角色";
            // 
            // buttonSwitchUser
            // 
            this.buttonSwitchUser.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonSwitchUser.Location = new System.Drawing.Point(16, 74);
            this.buttonSwitchUser.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSwitchUser.Name = "buttonSwitchUser";
            this.buttonSwitchUser.Radius = 10;
            this.buttonSwitchUser.Size = new System.Drawing.Size(110, 36);
            this.buttonSwitchUser.TabIndex = 3;
            this.buttonSwitchUser.Text = "切换用户";
            this.buttonSwitchUser.Type = AntdUI.TTypeMini.Primary;
            this.buttonSwitchUser.WaveSize = 0;
            // 
            // buttonChangePassword
            // 
            this.buttonChangePassword.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonChangePassword.Location = new System.Drawing.Point(134, 74);
            this.buttonChangePassword.Margin = new System.Windows.Forms.Padding(0);
            this.buttonChangePassword.Name = "buttonChangePassword";
            this.buttonChangePassword.Radius = 10;
            this.buttonChangePassword.Size = new System.Drawing.Size(110, 36);
            this.buttonChangePassword.TabIndex = 4;
            this.buttonChangePassword.Text = "修改密码";
            this.buttonChangePassword.Type = AntdUI.TTypeMini.Primary;
            this.buttonChangePassword.WaveSize = 0;
            // 
            // buttonLogout
            // 
            this.buttonLogout.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonLogout.Location = new System.Drawing.Point(16, 120);
            this.buttonLogout.Margin = new System.Windows.Forms.Padding(0);
            this.buttonLogout.Name = "buttonLogout";
            this.buttonLogout.Radius = 10;
            this.buttonLogout.Size = new System.Drawing.Size(110, 36);
            this.buttonLogout.TabIndex = 5;
            this.buttonLogout.Text = "退出登录";
            this.buttonLogout.Type = AntdUI.TTypeMini.Error;
            this.buttonLogout.WaveSize = 0;
            // 
            // UserAvatarPopoverCard
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.buttonLogout);
            this.Controls.Add(this.buttonChangePassword);
            this.Controls.Add(this.buttonSwitchUser);
            this.Controls.Add(this.labelRoleDisplayName);
            this.Controls.Add(this.labelUserDisplayName);
            this.Controls.Add(this.avatarPopupUser);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "UserAvatarPopoverCard";
            this.Size = new System.Drawing.Size(260, 180);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.Avatar avatarPopupUser;
        private AntdUI.Label labelUserDisplayName;
        private AntdUI.Label labelRoleDisplayName;
        private AntdUI.Button buttonSwitchUser;
        private AntdUI.Button buttonChangePassword;
        private AntdUI.Button buttonLogout;
    }
}