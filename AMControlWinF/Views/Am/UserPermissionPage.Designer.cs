namespace AMControlWinF.Views.Am
{
    partial class UserPermissionPage
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelRoot = new AntdUI.Panel();
            this.panelPermissionHost = new AntdUI.Panel();
            this.flowPermissionCards = new AntdUI.FlowPanel();
            this.labelModuleTitle = new AntdUI.Label();
            this.panelModuleBar = new AntdUI.Panel();
            this.flowModules = new AntdUI.FlowPanel();
            this.panelToolbar = new AntdUI.Panel();
            this.flowToolbarRight = new AntdUI.FlowPanel();
            this.buttonSave = new AntdUI.Button();
            this.buttonRestoreDefault = new AntdUI.Button();
            this.buttonClear = new AntdUI.Button();
            this.buttonSelectAll = new AntdUI.Button();
            this.flowToolbarLeft = new AntdUI.FlowPanel();
            this.labelCurrentUser = new AntdUI.Label();
            this.buttonSelectUser = new AntdUI.Button();
            this.panelRoot.SuspendLayout();
            this.panelPermissionHost.SuspendLayout();
            this.panelModuleBar.SuspendLayout();
            this.panelToolbar.SuspendLayout();
            this.flowToolbarRight.SuspendLayout();
            this.flowToolbarLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.panelPermissionHost);
            this.panelRoot.Controls.Add(this.panelModuleBar);
            this.panelRoot.Controls.Add(this.panelToolbar);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(8);
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(850, 680);
            this.panelRoot.TabIndex = 0;
            // 
            // panelPermissionHost
            // 
            this.panelPermissionHost.BackColor = System.Drawing.Color.Transparent;
            this.panelPermissionHost.Controls.Add(this.flowPermissionCards);
            this.panelPermissionHost.Controls.Add(this.labelModuleTitle);
            this.panelPermissionHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPermissionHost.Location = new System.Drawing.Point(8, 122);
            this.panelPermissionHost.Margin = new System.Windows.Forms.Padding(0);
            this.panelPermissionHost.Name = "panelPermissionHost";
            this.panelPermissionHost.Padding = new System.Windows.Forms.Padding(8);
            this.panelPermissionHost.Radius = 12;
            this.panelPermissionHost.Shadow = 4;
            this.panelPermissionHost.Size = new System.Drawing.Size(834, 550);
            this.panelPermissionHost.TabIndex = 2;
            // 
            // flowPermissionCards
            // 
            this.flowPermissionCards.Align = AntdUI.TAlignFlow.Left;
            this.flowPermissionCards.AutoScroll = true;
            this.flowPermissionCards.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowPermissionCards.Gap = 8;
            this.flowPermissionCards.Location = new System.Drawing.Point(12, 48);
            this.flowPermissionCards.Margin = new System.Windows.Forms.Padding(0);
            this.flowPermissionCards.Name = "flowPermissionCards";
            this.flowPermissionCards.Size = new System.Drawing.Size(810, 490);
            this.flowPermissionCards.TabIndex = 1;
            this.flowPermissionCards.Text = "flowPermissionCards";
            // 
            // labelModuleTitle
            // 
            this.labelModuleTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelModuleTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.labelModuleTitle.Location = new System.Drawing.Point(12, 12);
            this.labelModuleTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelModuleTitle.Name = "labelModuleTitle";
            this.labelModuleTitle.Size = new System.Drawing.Size(810, 36);
            this.labelModuleTitle.TabIndex = 0;
            this.labelModuleTitle.Text = "页面权限";
            // 
            // panelModuleBar
            // 
            this.panelModuleBar.BackColor = System.Drawing.Color.Transparent;
            this.panelModuleBar.Controls.Add(this.flowModules);
            this.panelModuleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelModuleBar.Location = new System.Drawing.Point(8, 52);
            this.panelModuleBar.Margin = new System.Windows.Forms.Padding(0);
            this.panelModuleBar.Name = "panelModuleBar";
            this.panelModuleBar.Radius = 0;
            this.panelModuleBar.Size = new System.Drawing.Size(834, 70);
            this.panelModuleBar.TabIndex = 1;
            // 
            // flowModules
            // 
            this.flowModules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowModules.Gap = 8;
            this.flowModules.Location = new System.Drawing.Point(0, 0);
            this.flowModules.Margin = new System.Windows.Forms.Padding(0);
            this.flowModules.Name = "flowModules";
            this.flowModules.Padding = new System.Windows.Forms.Padding(4, 18, 4, 6);
            this.flowModules.Size = new System.Drawing.Size(834, 70);
            this.flowModules.TabIndex = 0;
            this.flowModules.Text = "flowModules";
            // 
            // panelToolbar
            // 
            this.panelToolbar.BackColor = System.Drawing.Color.Transparent;
            this.panelToolbar.Controls.Add(this.flowToolbarRight);
            this.panelToolbar.Controls.Add(this.flowToolbarLeft);
            this.panelToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelToolbar.Location = new System.Drawing.Point(8, 8);
            this.panelToolbar.Margin = new System.Windows.Forms.Padding(0);
            this.panelToolbar.Name = "panelToolbar";
            this.panelToolbar.Padding = new System.Windows.Forms.Padding(4);
            this.panelToolbar.Radius = 0;
            this.panelToolbar.Size = new System.Drawing.Size(834, 44);
            this.panelToolbar.TabIndex = 0;
            // 
            // flowToolbarRight
            // 
            this.flowToolbarRight.Controls.Add(this.buttonSave);
            this.flowToolbarRight.Controls.Add(this.buttonRestoreDefault);
            this.flowToolbarRight.Controls.Add(this.buttonClear);
            this.flowToolbarRight.Controls.Add(this.buttonSelectAll);
            this.flowToolbarRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowToolbarRight.Gap = 8;
            this.flowToolbarRight.Location = new System.Drawing.Point(391, 4);
            this.flowToolbarRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowToolbarRight.Name = "flowToolbarRight";
            this.flowToolbarRight.Size = new System.Drawing.Size(439, 36);
            this.flowToolbarRight.TabIndex = 1;
            this.flowToolbarRight.Text = "flowToolbarRight";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(332, 0);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Radius = 8;
            this.buttonSave.Size = new System.Drawing.Size(100, 36);
            this.buttonSave.TabIndex = 0;
            this.buttonSave.Text = "保存";
            this.buttonSave.Type = AntdUI.TTypeMini.Primary;
            this.buttonSave.WaveSize = 0;
            // 
            // buttonRestoreDefault
            // 
            this.buttonRestoreDefault.Location = new System.Drawing.Point(216, 0);
            this.buttonRestoreDefault.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRestoreDefault.Name = "buttonRestoreDefault";
            this.buttonRestoreDefault.Radius = 8;
            this.buttonRestoreDefault.Size = new System.Drawing.Size(108, 36);
            this.buttonRestoreDefault.TabIndex = 1;
            this.buttonRestoreDefault.Text = "恢复默认权限";
            this.buttonRestoreDefault.WaveSize = 0;
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(108, 0);
            this.buttonClear.Margin = new System.Windows.Forms.Padding(0);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Radius = 8;
            this.buttonClear.Size = new System.Drawing.Size(100, 36);
            this.buttonClear.TabIndex = 2;
            this.buttonClear.Text = "清空";
            this.buttonClear.WaveSize = 0;
            // 
            // buttonSelectAll
            // 
            this.buttonSelectAll.Location = new System.Drawing.Point(0, 0);
            this.buttonSelectAll.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Radius = 8;
            this.buttonSelectAll.Size = new System.Drawing.Size(100, 36);
            this.buttonSelectAll.TabIndex = 3;
            this.buttonSelectAll.Text = "全选";
            this.buttonSelectAll.WaveSize = 0;
            // 
            // flowToolbarLeft
            // 
            this.flowToolbarLeft.Controls.Add(this.labelCurrentUser);
            this.flowToolbarLeft.Controls.Add(this.buttonSelectUser);
            this.flowToolbarLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowToolbarLeft.Gap = 8;
            this.flowToolbarLeft.Location = new System.Drawing.Point(4, 4);
            this.flowToolbarLeft.Margin = new System.Windows.Forms.Padding(0);
            this.flowToolbarLeft.Name = "flowToolbarLeft";
            this.flowToolbarLeft.Size = new System.Drawing.Size(344, 36);
            this.flowToolbarLeft.TabIndex = 0;
            this.flowToolbarLeft.Text = "flowToolbarLeft";
            // 
            // labelCurrentUser
            // 
            this.labelCurrentUser.Location = new System.Drawing.Point(108, 0);
            this.labelCurrentUser.Margin = new System.Windows.Forms.Padding(0);
            this.labelCurrentUser.Name = "labelCurrentUser";
            this.labelCurrentUser.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.labelCurrentUser.Size = new System.Drawing.Size(184, 36);
            this.labelCurrentUser.TabIndex = 1;
            this.labelCurrentUser.Text = "当前用户：未选择";
            // 
            // buttonSelectUser
            // 
            this.buttonSelectUser.IconSvg = "UserOutlined";
            this.buttonSelectUser.Location = new System.Drawing.Point(0, 0);
            this.buttonSelectUser.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSelectUser.Name = "buttonSelectUser";
            this.buttonSelectUser.Radius = 8;
            this.buttonSelectUser.Size = new System.Drawing.Size(100, 36);
            this.buttonSelectUser.TabIndex = 0;
            this.buttonSelectUser.Text = "选择用户";
            this.buttonSelectUser.Type = AntdUI.TTypeMini.Primary;
            this.buttonSelectUser.WaveSize = 0;
            // 
            // UserPermissionPage
            // 
            this.Controls.Add(this.panelRoot);
            this.Name = "UserPermissionPage";
            this.Size = new System.Drawing.Size(850, 680);
            this.panelRoot.ResumeLayout(false);
            this.panelPermissionHost.ResumeLayout(false);
            this.panelModuleBar.ResumeLayout(false);
            this.panelToolbar.ResumeLayout(false);
            this.flowToolbarRight.ResumeLayout(false);
            this.flowToolbarLeft.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AntdUI.Panel panelRoot;
        private AntdUI.Panel panelToolbar;
        private AntdUI.FlowPanel flowToolbarLeft;
        private AntdUI.Button buttonSelectUser;
        private AntdUI.Label labelCurrentUser;
        private AntdUI.FlowPanel flowToolbarRight;
        private AntdUI.Button buttonSave;
        private AntdUI.Button buttonRestoreDefault;
        private AntdUI.Button buttonClear;
        private AntdUI.Button buttonSelectAll;
        private AntdUI.Panel panelModuleBar;
        private AntdUI.FlowPanel flowModules;
        private AntdUI.Panel panelPermissionHost;
        private AntdUI.Label labelModuleTitle;
        private AntdUI.FlowPanel flowPermissionCards;
    }
}