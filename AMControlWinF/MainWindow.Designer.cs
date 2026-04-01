namespace AMControlWinF
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.titlebar = new AntdUI.PageHeader();
            this.dropdownTranslate = new AntdUI.Dropdown();
            this.buttonColorMode = new AntdUI.Button();
            this.textureBackgroundMain = new AMControlWinF.Views.Main.TextureBackgroundControl();
            this.gridMainHost = new AntdUI.GridPanel();
            this.panelStatusCard = new AntdUI.Panel();
            this.labelStatusValue = new AntdUI.Label();
            this.panelLeftCard = new AntdUI.Panel();
            this.menuPrimary = new AntdUI.Menu();
            this.panelAvatarHost = new AntdUI.Panel();
            this.userAvatarMenuControl = new AMControlWinF.Views.Main.UserAvatarMenuControl();
            this.panelSecondaryNavCard = new AntdUI.Panel();
            this.menuSecondary = new AntdUI.Menu();
            this.panelSecondaryHeader = new AntdUI.Panel();
            this.labelPrimaryTitleValue = new AntdUI.Label();
            this.panelWorkCard = new AntdUI.Panel();
            this.panelContent = new AntdUI.Panel();
            this.titlebar.SuspendLayout();
            this.textureBackgroundMain.SuspendLayout();
            this.gridMainHost.SuspendLayout();
            this.panelStatusCard.SuspendLayout();
            this.panelLeftCard.SuspendLayout();
            this.panelAvatarHost.SuspendLayout();
            this.panelSecondaryNavCard.SuspendLayout();
            this.panelSecondaryHeader.SuspendLayout();
            this.panelWorkCard.SuspendLayout();
            this.SuspendLayout();
            // 
            // titlebar
            // 
            this.titlebar.Controls.Add(this.dropdownTranslate);
            this.titlebar.Controls.Add(this.buttonColorMode);
            this.titlebar.DividerShow = true;
            this.titlebar.Dock = System.Windows.Forms.DockStyle.Top;
            this.titlebar.Location = new System.Drawing.Point(0, 0);
            this.titlebar.Name = "titlebar";
            this.titlebar.ShowButton = true;
            this.titlebar.ShowIcon = true;
            this.titlebar.Size = new System.Drawing.Size(1200, 48);
            this.titlebar.SubText = "v0.0.1";
            this.titlebar.TabIndex = 0;
            this.titlebar.Text = "AM运动控制";
            // 
            // dropdownTranslate
            // 
            this.dropdownTranslate.Dock = System.Windows.Forms.DockStyle.Right;
            this.dropdownTranslate.Ghost = true;
            this.dropdownTranslate.IconSvg = "TranslationOutlined";
            this.dropdownTranslate.Items.AddRange(new object[] {
            "简体中文",
            "English"});
            this.dropdownTranslate.Location = new System.Drawing.Point(956, 0);
            this.dropdownTranslate.Name = "dropdownTranslate";
            this.dropdownTranslate.Placement = AntdUI.TAlignFrom.BR;
            this.dropdownTranslate.Size = new System.Drawing.Size(50, 48);
            this.dropdownTranslate.TabIndex = 1;
            this.dropdownTranslate.Trigger = AntdUI.Trigger.Hover;
            this.dropdownTranslate.WaveSize = 0;
            // 
            // buttonColorMode
            // 
            this.buttonColorMode.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonColorMode.Ghost = true;
            this.buttonColorMode.IconSvg = "SunOutlined";
            this.buttonColorMode.Location = new System.Drawing.Point(1006, 0);
            this.buttonColorMode.Name = "buttonColorMode";
            this.buttonColorMode.Radius = 0;
            this.buttonColorMode.Size = new System.Drawing.Size(50, 48);
            this.buttonColorMode.TabIndex = 0;
            this.buttonColorMode.ToggleIconSvg = "MoonOutlined";
            this.buttonColorMode.WaveSize = 0;
            // 
            // textureBackgroundMain
            // 
            this.textureBackgroundMain.Controls.Add(this.gridMainHost);
            this.textureBackgroundMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textureBackgroundMain.Location = new System.Drawing.Point(0, 48);
            this.textureBackgroundMain.Name = "textureBackgroundMain";
            this.textureBackgroundMain.Size = new System.Drawing.Size(1200, 752);
            this.textureBackgroundMain.TabIndex = 1;
            // 
            // gridMainHost
            // 
            this.gridMainHost.Back = System.Drawing.Color.Transparent;
            this.gridMainHost.BackColor = System.Drawing.Color.Transparent;
            this.gridMainHost.Controls.Add(this.panelStatusCard);
            this.gridMainHost.Controls.Add(this.panelLeftCard);
            this.gridMainHost.Controls.Add(this.panelSecondaryNavCard);
            this.gridMainHost.Controls.Add(this.panelWorkCard);
            this.gridMainHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridMainHost.Location = new System.Drawing.Point(0, 0);
            this.gridMainHost.Name = "gridMainHost";
            this.gridMainHost.Padding = new System.Windows.Forms.Padding(6);
            this.gridMainHost.Size = new System.Drawing.Size(1200, 752);
            this.gridMainHost.Span = "100% 230 96 ;100%-100% 40";
            this.gridMainHost.TabIndex = 0;
            // 
            // panelStatusCard
            // 
            this.panelStatusCard.BackColor = System.Drawing.Color.Transparent;
            this.panelStatusCard.Controls.Add(this.labelStatusValue);
            this.panelStatusCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStatusCard.Location = new System.Drawing.Point(6, 706);
            this.panelStatusCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelStatusCard.Name = "panelStatusCard";
            this.panelStatusCard.Padding = new System.Windows.Forms.Padding(14, 0, 14, 0);
            this.panelStatusCard.Radius = 12;
            this.panelStatusCard.Shadow = 4;
            this.panelStatusCard.Size = new System.Drawing.Size(1188, 40);
            this.panelStatusCard.TabIndex = 3;
            // 
            // labelStatusValue
            // 
            this.labelStatusValue.BackColor = System.Drawing.Color.Transparent;
            this.labelStatusValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelStatusValue.Location = new System.Drawing.Point(18, 4);
            this.labelStatusValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelStatusValue.Name = "labelStatusValue";
            this.labelStatusValue.Size = new System.Drawing.Size(1152, 32);
            this.labelStatusValue.TabIndex = 0;
            this.labelStatusValue.Text = "应用已启动，等待操作。";
            // 
            // panelLeftCard
            // 
            this.panelLeftCard.BackColor = System.Drawing.Color.Transparent;
            this.panelLeftCard.Controls.Add(this.menuPrimary);
            this.panelLeftCard.Controls.Add(this.panelAvatarHost);
            this.panelLeftCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLeftCard.Location = new System.Drawing.Point(1098, 6);
            this.panelLeftCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelLeftCard.Name = "panelLeftCard";
            this.panelLeftCard.Padding = new System.Windows.Forms.Padding(8);
            this.panelLeftCard.Radius = 16;
            this.panelLeftCard.Shadow = 8;
            this.panelLeftCard.Size = new System.Drawing.Size(96, 700);
            this.panelLeftCard.TabIndex = 2;
            // 
            // menuPrimary
            // 
            this.menuPrimary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuPrimary.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.menuPrimary.Indent = true;
            this.menuPrimary.Location = new System.Drawing.Point(16, 16);
            this.menuPrimary.Margin = new System.Windows.Forms.Padding(0);
            this.menuPrimary.Name = "menuPrimary";
            this.menuPrimary.Padding = new System.Windows.Forms.Padding(5);
            this.menuPrimary.Size = new System.Drawing.Size(64, 626);
            this.menuPrimary.TabIndex = 0;
            this.menuPrimary.Unique = true;
            // 
            // panelAvatarHost
            // 
            this.panelAvatarHost.Controls.Add(this.userAvatarMenuControl);
            this.panelAvatarHost.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelAvatarHost.Location = new System.Drawing.Point(16, 642);
            this.panelAvatarHost.Margin = new System.Windows.Forms.Padding(0);
            this.panelAvatarHost.Name = "panelAvatarHost";
            this.panelAvatarHost.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.panelAvatarHost.Radius = 0;
            this.panelAvatarHost.Size = new System.Drawing.Size(64, 42);
            this.panelAvatarHost.TabIndex = 1;
            // 
            // userAvatarMenuControl
            // 
            this.userAvatarMenuControl.BackColor = System.Drawing.Color.Transparent;
            this.userAvatarMenuControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.userAvatarMenuControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userAvatarMenuControl.Location = new System.Drawing.Point(0, 2);
            this.userAvatarMenuControl.Margin = new System.Windows.Forms.Padding(0);
            this.userAvatarMenuControl.Name = "userAvatarMenuControl";
            this.userAvatarMenuControl.RoleDisplayName = "用户";
            this.userAvatarMenuControl.Size = new System.Drawing.Size(64, 38);
            this.userAvatarMenuControl.TabIndex = 0;
            this.userAvatarMenuControl.UserDisplayName = "未登录";
            // 
            // panelSecondaryNavCard
            // 
            this.panelSecondaryNavCard.BackColor = System.Drawing.Color.Transparent;
            this.panelSecondaryNavCard.Controls.Add(this.menuSecondary);
            this.panelSecondaryNavCard.Controls.Add(this.panelSecondaryHeader);
            this.panelSecondaryNavCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSecondaryNavCard.Location = new System.Drawing.Point(868, 6);
            this.panelSecondaryNavCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelSecondaryNavCard.Name = "panelSecondaryNavCard";
            this.panelSecondaryNavCard.Padding = new System.Windows.Forms.Padding(8);
            this.panelSecondaryNavCard.Radius = 16;
            this.panelSecondaryNavCard.Shadow = 8;
            this.panelSecondaryNavCard.Size = new System.Drawing.Size(230, 700);
            this.panelSecondaryNavCard.TabIndex = 1;
            // 
            // menuSecondary
            // 
            this.menuSecondary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuSecondary.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.menuSecondary.Indent = true;
            this.menuSecondary.Location = new System.Drawing.Point(16, 72);
            this.menuSecondary.Margin = new System.Windows.Forms.Padding(0);
            this.menuSecondary.Name = "menuSecondary";
            this.menuSecondary.Padding = new System.Windows.Forms.Padding(2);
            this.menuSecondary.Size = new System.Drawing.Size(198, 612);
            this.menuSecondary.TabIndex = 1;
            this.menuSecondary.Unique = true;
            // 
            // panelSecondaryHeader
            // 
            this.panelSecondaryHeader.Controls.Add(this.labelPrimaryTitleValue);
            this.panelSecondaryHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSecondaryHeader.Location = new System.Drawing.Point(16, 16);
            this.panelSecondaryHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelSecondaryHeader.Name = "panelSecondaryHeader";
            this.panelSecondaryHeader.Padding = new System.Windows.Forms.Padding(18, 0, 18, 0);
            this.panelSecondaryHeader.Radius = 0;
            this.panelSecondaryHeader.Size = new System.Drawing.Size(198, 56);
            this.panelSecondaryHeader.TabIndex = 0;
            // 
            // labelPrimaryTitleValue
            // 
            this.labelPrimaryTitleValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPrimaryTitleValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 16F, System.Drawing.FontStyle.Bold);
            this.labelPrimaryTitleValue.Location = new System.Drawing.Point(18, 0);
            this.labelPrimaryTitleValue.Name = "labelPrimaryTitleValue";
            this.labelPrimaryTitleValue.Size = new System.Drawing.Size(162, 56);
            this.labelPrimaryTitleValue.TabIndex = 0;
            this.labelPrimaryTitleValue.Text = "首页";
            // 
            // panelWorkCard
            // 
            this.panelWorkCard.BackColor = System.Drawing.Color.Transparent;
            this.panelWorkCard.Controls.Add(this.panelContent);
            this.panelWorkCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWorkCard.Location = new System.Drawing.Point(6, 6);
            this.panelWorkCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelWorkCard.Name = "panelWorkCard";
            this.panelWorkCard.Padding = new System.Windows.Forms.Padding(10);
            this.panelWorkCard.Radius = 16;
            this.panelWorkCard.Shadow = 8;
            this.panelWorkCard.Size = new System.Drawing.Size(862, 700);
            this.panelWorkCard.TabIndex = 0;
            // 
            // panelContent
            // 
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(18, 18);
            this.panelContent.Margin = new System.Windows.Forms.Padding(0);
            this.panelContent.Name = "panelContent";
            this.panelContent.Radius = 0;
            this.panelContent.Size = new System.Drawing.Size(826, 664);
            this.panelContent.TabIndex = 0;
            // 
            // MainWindow
            // 
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.ControlBox = false;
            this.Controls.Add(this.textureBackgroundMain);
            this.Controls.Add(this.titlebar);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1200, 800);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AMControl WinForms";
            this.titlebar.ResumeLayout(false);
            this.textureBackgroundMain.ResumeLayout(false);
            this.gridMainHost.ResumeLayout(false);
            this.panelStatusCard.ResumeLayout(false);
            this.panelLeftCard.ResumeLayout(false);
            this.panelAvatarHost.ResumeLayout(false);
            this.panelSecondaryNavCard.ResumeLayout(false);
            this.panelSecondaryHeader.ResumeLayout(false);
            this.panelWorkCard.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.PageHeader titlebar;
        private AntdUI.Dropdown dropdownTranslate;
        private AntdUI.Button buttonColorMode;
        private AMControlWinF.Views.Main.TextureBackgroundControl textureBackgroundMain;
        private AntdUI.GridPanel gridMainHost;
        private AntdUI.Panel panelStatusCard;
        private AntdUI.Label labelStatusValue;
        private AntdUI.Panel panelWorkCard;
        private AntdUI.Panel panelContent;
        private AntdUI.Panel panelSecondaryNavCard;
        private AntdUI.Menu menuSecondary;
        private AntdUI.Panel panelSecondaryHeader;
        private AntdUI.Label labelPrimaryTitleValue;
        private AntdUI.Panel panelLeftCard;
        private AntdUI.Menu menuPrimary;
        private AntdUI.Panel panelAvatarHost;
        private AMControlWinF.Views.Main.UserAvatarMenuControl userAvatarMenuControl;
    }
}