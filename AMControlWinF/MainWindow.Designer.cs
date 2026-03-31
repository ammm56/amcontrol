namespace AMControlWinF
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.titlebar = new AntdUI.PageHeader();
            this.dropdownTranslate = new AntdUI.Dropdown();
            this.buttonColorMode = new AntdUI.Button();
            this.textureBackgroundMain = new AMControlWinF.Views.Main.TextureBackgroundControl();
            this.layoutMainHost = new System.Windows.Forms.TableLayoutPanel();
            this.layoutShell = new System.Windows.Forms.TableLayoutPanel();
            this.panelLeftCard = new AntdUI.Panel();
            this.layoutLeft = new System.Windows.Forms.TableLayoutPanel();
            this.menuPrimary = new AntdUI.Menu();
            this.panelAvatarHost = new AntdUI.Panel();
            this.userAvatarMenuControl = new AMControlWinF.Views.Main.UserAvatarMenuControl();
            this.panelSecondaryNavCard = new AntdUI.Panel();
            this.layoutSecondary = new System.Windows.Forms.TableLayoutPanel();
            this.panelSecondaryHeader = new AntdUI.Panel();
            this.labelPrimaryTitleValue = new AntdUI.Label();
            this.menuSecondary = new AntdUI.Menu();
            this.panelWorkCard = new AntdUI.Panel();
            this.layoutWorkCard = new System.Windows.Forms.TableLayoutPanel();
            this.panelWorkHeader = new AntdUI.Panel();
            this.layoutHeader = new System.Windows.Forms.TableLayoutPanel();
            this.labelPageTitleValue = new AntdUI.Label();
            this.labelPageDescriptionValue = new AntdUI.Label();
            this.panelContent = new AntdUI.Panel();
            this.panelStatusHost = new System.Windows.Forms.Panel();
            this.panelStatusCard = new AntdUI.Panel();
            this.labelStatusCaption = new AntdUI.Label();
            this.titlebar.SuspendLayout();
            this.textureBackgroundMain.SuspendLayout();
            this.layoutMainHost.SuspendLayout();
            this.layoutShell.SuspendLayout();
            this.panelLeftCard.SuspendLayout();
            this.layoutLeft.SuspendLayout();
            this.panelAvatarHost.SuspendLayout();
            this.panelSecondaryNavCard.SuspendLayout();
            this.layoutSecondary.SuspendLayout();
            this.panelSecondaryHeader.SuspendLayout();
            this.panelWorkCard.SuspendLayout();
            this.layoutWorkCard.SuspendLayout();
            this.panelWorkHeader.SuspendLayout();
            this.layoutHeader.SuspendLayout();
            this.panelStatusHost.SuspendLayout();
            this.panelStatusCard.SuspendLayout();
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
            this.dropdownTranslate.Location = new System.Drawing.Point(920, 0);
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
            this.buttonColorMode.Location = new System.Drawing.Point(970, 0);
            this.buttonColorMode.Name = "buttonColorMode";
            this.buttonColorMode.Radius = 0;
            this.buttonColorMode.Size = new System.Drawing.Size(50, 48);
            this.buttonColorMode.TabIndex = 0;
            this.buttonColorMode.ToggleIconSvg = "MoonOutlined";
            this.buttonColorMode.WaveSize = 0;
            // 
            // textureBackgroundMain
            // 
            this.textureBackgroundMain.Controls.Add(this.layoutMainHost);
            this.textureBackgroundMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textureBackgroundMain.Location = new System.Drawing.Point(0, 48);
            this.textureBackgroundMain.Name = "textureBackgroundMain";
            this.textureBackgroundMain.Size = new System.Drawing.Size(1200, 752);
            this.textureBackgroundMain.TabIndex = 1;
            // 
            // layoutMainHost
            // 
            this.layoutMainHost.BackColor = System.Drawing.Color.Transparent;
            this.layoutMainHost.ColumnCount = 1;
            this.layoutMainHost.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMainHost.Controls.Add(this.layoutShell, 0, 0);
            this.layoutMainHost.Controls.Add(this.panelStatusHost, 0, 1);
            this.layoutMainHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMainHost.Location = new System.Drawing.Point(0, 0);
            this.layoutMainHost.Margin = new System.Windows.Forms.Padding(0);
            this.layoutMainHost.Name = "layoutMainHost";
            this.layoutMainHost.Padding = new System.Windows.Forms.Padding(12, 8, 8, 6);
            this.layoutMainHost.RowCount = 2;
            this.layoutMainHost.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMainHost.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.layoutMainHost.Size = new System.Drawing.Size(1200, 752);
            this.layoutMainHost.TabIndex = 0;
            // 
            // layoutShell
            // 
            this.layoutShell.BackColor = System.Drawing.Color.Transparent;
            this.layoutShell.ColumnCount = 3;
            this.layoutShell.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 86F));
            this.layoutShell.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 230F));
            this.layoutShell.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutShell.Controls.Add(this.panelLeftCard, 0, 0);
            this.layoutShell.Controls.Add(this.panelSecondaryNavCard, 1, 0);
            this.layoutShell.Controls.Add(this.panelWorkCard, 2, 0);
            this.layoutShell.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutShell.Location = new System.Drawing.Point(12, 8);
            this.layoutShell.Margin = new System.Windows.Forms.Padding(0);
            this.layoutShell.Name = "layoutShell";
            this.layoutShell.RowCount = 1;
            this.layoutShell.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutShell.Size = new System.Drawing.Size(1180, 698);
            this.layoutShell.TabIndex = 0;
            // 
            // panelLeftCard
            // 
            this.panelLeftCard.Back = System.Drawing.Color.White;
            this.panelLeftCard.Controls.Add(this.layoutLeft);
            this.panelLeftCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLeftCard.Location = new System.Drawing.Point(0, 0);
            this.panelLeftCard.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.panelLeftCard.Name = "panelLeftCard";
            this.panelLeftCard.Padding = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.panelLeftCard.Radius = 14;
            this.panelLeftCard.Shadow = 6;
            this.panelLeftCard.Size = new System.Drawing.Size(84, 698);
            this.panelLeftCard.TabIndex = 0;
            // 
            // layoutLeft
            // 
            this.layoutLeft.BackColor = System.Drawing.Color.Transparent;
            this.layoutLeft.ColumnCount = 1;
            this.layoutLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutLeft.Controls.Add(this.menuPrimary, 0, 0);
            this.layoutLeft.Controls.Add(this.panelAvatarHost, 0, 1);
            this.layoutLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutLeft.Location = new System.Drawing.Point(15, 16);
            this.layoutLeft.Margin = new System.Windows.Forms.Padding(0);
            this.layoutLeft.Name = "layoutLeft";
            this.layoutLeft.RowCount = 2;
            this.layoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutLeft.Size = new System.Drawing.Size(54, 666);
            this.layoutLeft.TabIndex = 0;
            // 
            // menuPrimary
            // 
            this.menuPrimary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuPrimary.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.menuPrimary.Indent = true;
            this.menuPrimary.Location = new System.Drawing.Point(0, 0);
            this.menuPrimary.Margin = new System.Windows.Forms.Padding(0);
            this.menuPrimary.Name = "menuPrimary";
            this.menuPrimary.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.menuPrimary.Size = new System.Drawing.Size(54, 598);
            this.menuPrimary.TabIndex = 0;
            this.menuPrimary.Unique = true;
            // 
            // panelAvatarHost
            // 
            this.panelAvatarHost.Back = System.Drawing.Color.Transparent;
            this.panelAvatarHost.Controls.Add(this.userAvatarMenuControl);
            this.panelAvatarHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAvatarHost.Location = new System.Drawing.Point(0, 606);
            this.panelAvatarHost.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.panelAvatarHost.Name = "panelAvatarHost";
            this.panelAvatarHost.Radius = 0;
            this.panelAvatarHost.Size = new System.Drawing.Size(54, 60);
            this.panelAvatarHost.TabIndex = 1;
            // 
            // userAvatarMenuControl
            // 
            this.userAvatarMenuControl.BackColor = System.Drawing.Color.Transparent;
            this.userAvatarMenuControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.userAvatarMenuControl.Location = new System.Drawing.Point(0, 0);
            this.userAvatarMenuControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userAvatarMenuControl.Name = "userAvatarMenuControl";
            this.userAvatarMenuControl.RoleDisplayName = "用户";
            this.userAvatarMenuControl.Size = new System.Drawing.Size(60, 60);
            this.userAvatarMenuControl.TabIndex = 0;
            this.userAvatarMenuControl.UserDisplayName = "未登录";
            // 
            // panelSecondaryNavCard
            // 
            this.panelSecondaryNavCard.Back = System.Drawing.Color.White;
            this.panelSecondaryNavCard.Controls.Add(this.layoutSecondary);
            this.panelSecondaryNavCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSecondaryNavCard.Location = new System.Drawing.Point(86, 0);
            this.panelSecondaryNavCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelSecondaryNavCard.Name = "panelSecondaryNavCard";
            this.panelSecondaryNavCard.Radius = 14;
            this.panelSecondaryNavCard.Shadow = 6;
            this.panelSecondaryNavCard.Size = new System.Drawing.Size(230, 698);
            this.panelSecondaryNavCard.TabIndex = 1;
            // 
            // layoutSecondary
            // 
            this.layoutSecondary.BackColor = System.Drawing.Color.Transparent;
            this.layoutSecondary.ColumnCount = 1;
            this.layoutSecondary.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutSecondary.Controls.Add(this.panelSecondaryHeader, 0, 0);
            this.layoutSecondary.Controls.Add(this.menuSecondary, 0, 1);
            this.layoutSecondary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutSecondary.Location = new System.Drawing.Point(7, 7);
            this.layoutSecondary.Margin = new System.Windows.Forms.Padding(0);
            this.layoutSecondary.Name = "layoutSecondary";
            this.layoutSecondary.RowCount = 2;
            this.layoutSecondary.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.layoutSecondary.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutSecondary.Size = new System.Drawing.Size(216, 684);
            this.layoutSecondary.TabIndex = 0;
            // 
            // panelSecondaryHeader
            // 
            this.panelSecondaryHeader.Back = System.Drawing.Color.Transparent;
            this.panelSecondaryHeader.Controls.Add(this.labelPrimaryTitleValue);
            this.panelSecondaryHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSecondaryHeader.Location = new System.Drawing.Point(0, 0);
            this.panelSecondaryHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelSecondaryHeader.Name = "panelSecondaryHeader";
            this.panelSecondaryHeader.Padding = new System.Windows.Forms.Padding(18, 0, 18, 0);
            this.panelSecondaryHeader.Radius = 0;
            this.panelSecondaryHeader.Size = new System.Drawing.Size(216, 56);
            this.panelSecondaryHeader.TabIndex = 0;
            // 
            // labelPrimaryTitleValue
            // 
            this.labelPrimaryTitleValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPrimaryTitleValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 16F, System.Drawing.FontStyle.Bold);
            this.labelPrimaryTitleValue.Location = new System.Drawing.Point(18, 0);
            this.labelPrimaryTitleValue.Name = "labelPrimaryTitleValue";
            this.labelPrimaryTitleValue.Size = new System.Drawing.Size(180, 56);
            this.labelPrimaryTitleValue.TabIndex = 0;
            this.labelPrimaryTitleValue.Text = "首页";
            // 
            // menuSecondary
            // 
            this.menuSecondary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuSecondary.Indent = true;
            this.menuSecondary.Location = new System.Drawing.Point(8, 64);
            this.menuSecondary.Margin = new System.Windows.Forms.Padding(8, 8, 8, 6);
            this.menuSecondary.Name = "menuSecondary";
            this.menuSecondary.Size = new System.Drawing.Size(200, 614);
            this.menuSecondary.TabIndex = 1;
            this.menuSecondary.Unique = true;
            // 
            // panelWorkCard
            // 
            this.panelWorkCard.Back = System.Drawing.Color.White;
            this.panelWorkCard.Controls.Add(this.layoutWorkCard);
            this.panelWorkCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWorkCard.Location = new System.Drawing.Point(316, 0);
            this.panelWorkCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelWorkCard.Name = "panelWorkCard";
            this.panelWorkCard.Radius = 16;
            this.panelWorkCard.Shadow = 8;
            this.panelWorkCard.Size = new System.Drawing.Size(864, 698);
            this.panelWorkCard.TabIndex = 2;
            // 
            // layoutWorkCard
            // 
            this.layoutWorkCard.BackColor = System.Drawing.Color.Transparent;
            this.layoutWorkCard.ColumnCount = 1;
            this.layoutWorkCard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutWorkCard.Controls.Add(this.panelWorkHeader, 0, 0);
            this.layoutWorkCard.Controls.Add(this.panelContent, 0, 1);
            this.layoutWorkCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutWorkCard.Location = new System.Drawing.Point(10, 10);
            this.layoutWorkCard.Margin = new System.Windows.Forms.Padding(0);
            this.layoutWorkCard.Name = "layoutWorkCard";
            this.layoutWorkCard.RowCount = 2;
            this.layoutWorkCard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.layoutWorkCard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutWorkCard.Size = new System.Drawing.Size(844, 678);
            this.layoutWorkCard.TabIndex = 0;
            // 
            // panelWorkHeader
            // 
            this.panelWorkHeader.Back = System.Drawing.Color.Transparent;
            this.panelWorkHeader.Controls.Add(this.layoutHeader);
            this.panelWorkHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWorkHeader.Location = new System.Drawing.Point(0, 0);
            this.panelWorkHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelWorkHeader.Name = "panelWorkHeader";
            this.panelWorkHeader.Padding = new System.Windows.Forms.Padding(16, 0, 16, 0);
            this.panelWorkHeader.Radius = 0;
            this.panelWorkHeader.Size = new System.Drawing.Size(844, 44);
            this.panelWorkHeader.TabIndex = 0;
            this.panelWorkHeader.Visible = false;
            // 
            // layoutHeader
            // 
            this.layoutHeader.BackColor = System.Drawing.Color.Transparent;
            this.layoutHeader.ColumnCount = 2;
            this.layoutHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 320F));
            this.layoutHeader.Controls.Add(this.labelPageTitleValue, 0, 0);
            this.layoutHeader.Controls.Add(this.labelPageDescriptionValue, 1, 0);
            this.layoutHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutHeader.Location = new System.Drawing.Point(16, 0);
            this.layoutHeader.Margin = new System.Windows.Forms.Padding(0);
            this.layoutHeader.Name = "layoutHeader";
            this.layoutHeader.RowCount = 1;
            this.layoutHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutHeader.Size = new System.Drawing.Size(812, 44);
            this.layoutHeader.TabIndex = 0;
            // 
            // labelPageTitleValue
            // 
            this.labelPageTitleValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPageTitleValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 15F, System.Drawing.FontStyle.Bold);
            this.labelPageTitleValue.Location = new System.Drawing.Point(3, 3);
            this.labelPageTitleValue.Name = "labelPageTitleValue";
            this.labelPageTitleValue.Size = new System.Drawing.Size(486, 38);
            this.labelPageTitleValue.TabIndex = 0;
            this.labelPageTitleValue.Text = "首页";
            // 
            // labelPageDescriptionValue
            // 
            this.labelPageDescriptionValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPageDescriptionValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.5F);
            this.labelPageDescriptionValue.Location = new System.Drawing.Point(495, 3);
            this.labelPageDescriptionValue.Name = "labelPageDescriptionValue";
            this.labelPageDescriptionValue.Size = new System.Drawing.Size(314, 38);
            this.labelPageDescriptionValue.TabIndex = 1;
            this.labelPageDescriptionValue.Text = "设备总览、生产摘要与快捷入口。";
            this.labelPageDescriptionValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelContent
            // 
            this.panelContent.Back = System.Drawing.Color.Transparent;
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(10, 54);
            this.panelContent.Margin = new System.Windows.Forms.Padding(10);
            this.panelContent.Name = "panelContent";
            this.panelContent.Radius = 0;
            this.panelContent.Size = new System.Drawing.Size(824, 614);
            this.panelContent.TabIndex = 1;
            // 
            // panelStatusHost
            // 
            this.panelStatusHost.BackColor = System.Drawing.Color.Transparent;
            this.panelStatusHost.Controls.Add(this.panelStatusCard);
            this.panelStatusHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStatusHost.Location = new System.Drawing.Point(12, 710);
            this.panelStatusHost.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.panelStatusHost.Name = "panelStatusHost";
            this.panelStatusHost.Size = new System.Drawing.Size(1180, 36);
            this.panelStatusHost.TabIndex = 1;
            // 
            // panelStatusCard
            // 
            this.panelStatusCard.Back = System.Drawing.Color.White;
            this.panelStatusCard.Controls.Add(this.labelStatusCaption);
            this.panelStatusCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStatusCard.Location = new System.Drawing.Point(0, 0);
            this.panelStatusCard.Name = "panelStatusCard";
            this.panelStatusCard.Padding = new System.Windows.Forms.Padding(14, 0, 14, 0);
            this.panelStatusCard.Radius = 12;
            this.panelStatusCard.Shadow = 4;
            this.panelStatusCard.Size = new System.Drawing.Size(1180, 36);
            this.panelStatusCard.TabIndex = 0;
            // 
            // labelStatusCaption
            // 
            this.labelStatusCaption.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelStatusCaption.Location = new System.Drawing.Point(19, 5);
            this.labelStatusCaption.Name = "labelStatusCaption";
            this.labelStatusCaption.Size = new System.Drawing.Size(88, 26);
            this.labelStatusCaption.TabIndex = 0;
            this.labelStatusCaption.Text = "系统状态：";
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
            this.layoutMainHost.ResumeLayout(false);
            this.layoutShell.ResumeLayout(false);
            this.panelLeftCard.ResumeLayout(false);
            this.layoutLeft.ResumeLayout(false);
            this.panelAvatarHost.ResumeLayout(false);
            this.panelSecondaryNavCard.ResumeLayout(false);
            this.layoutSecondary.ResumeLayout(false);
            this.panelSecondaryHeader.ResumeLayout(false);
            this.panelWorkCard.ResumeLayout(false);
            this.layoutWorkCard.ResumeLayout(false);
            this.panelWorkHeader.ResumeLayout(false);
            this.layoutHeader.ResumeLayout(false);
            this.panelStatusHost.ResumeLayout(false);
            this.panelStatusCard.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private AntdUI.Dropdown dropdownTranslate;
        private AntdUI.Button buttonColorMode;
        private AMControlWinF.Views.Main.TextureBackgroundControl textureBackgroundMain;
        private System.Windows.Forms.TableLayoutPanel layoutMainHost;
        private System.Windows.Forms.TableLayoutPanel layoutShell;
        private AntdUI.Panel panelLeftCard;
        private System.Windows.Forms.TableLayoutPanel layoutLeft;
        private AntdUI.Menu menuPrimary;
        private AntdUI.Panel panelAvatarHost;
        private AMControlWinF.Views.Main.UserAvatarMenuControl userAvatarMenuControl;
        private AntdUI.Panel panelSecondaryNavCard;
        private System.Windows.Forms.TableLayoutPanel layoutSecondary;
        private AntdUI.Panel panelSecondaryHeader;
        private AntdUI.Label labelPrimaryTitleValue;
        private AntdUI.Menu menuSecondary;
        private AntdUI.Panel panelWorkCard;
        private System.Windows.Forms.TableLayoutPanel layoutWorkCard;
        private AntdUI.Panel panelWorkHeader;
        private System.Windows.Forms.TableLayoutPanel layoutHeader;
        private AntdUI.Label labelPageTitleValue;
        private AntdUI.Label labelPageDescriptionValue;
        private AntdUI.Panel panelContent;
        private System.Windows.Forms.Panel panelStatusHost;
        private AntdUI.Panel panelStatusCard;
        private AntdUI.Label labelStatusCaption;
        private AntdUI.PageHeader titlebar;
    }
}