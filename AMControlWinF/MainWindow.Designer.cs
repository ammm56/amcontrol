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
            this.titlebar = new AntdUI.PageHeader();
            this.dropdownTranslate = new AntdUI.Dropdown();
            this.buttonColorMode = new AntdUI.Button();
            this.layoutRoot = new System.Windows.Forms.TableLayoutPanel();
            this.panelStage = new System.Windows.Forms.Panel();
            this.layoutShell = new System.Windows.Forms.TableLayoutPanel();
            this.panelLeftCard = new AntdUI.Panel();
            this.layoutLeft = new System.Windows.Forms.TableLayoutPanel();
            this.panelPrimaryNav = new AntdUI.Panel();
            this.labelPrimaryNavTitle = new AntdUI.Label();
            this.menuPrimary = new AntdUI.Menu();
            this.panelAvatarHost = new AntdUI.Panel();
            this.userAvatarMenuControl = new AMControlWinF.Views.Main.UserAvatarMenuControl();
            this.panelSecondaryNavCard = new AntdUI.Panel();
            this.labelSecondaryNavTitle = new AntdUI.Label();
            this.menuSecondary = new AntdUI.Menu();
            this.layoutWorkArea = new System.Windows.Forms.TableLayoutPanel();
            this.panelHeaderCard = new AntdUI.Panel();
            this.layoutHeader = new System.Windows.Forms.TableLayoutPanel();
            this.labelPrimaryTitleTitle = new AntdUI.Label();
            this.labelPrimaryTitleValue = new AntdUI.Label();
            this.labelPageTitleTitle = new AntdUI.Label();
            this.labelPageTitleValue = new AntdUI.Label();
            this.labelPageDescriptionTitle = new AntdUI.Label();
            this.labelPageDescriptionValue = new AntdUI.Label();
            this.panelContentCard = new AntdUI.Panel();
            this.panelContent = new AntdUI.Panel();
            this.textureBackgroundMain = new AMControlWinF.Views.Main.TextureBackgroundControl();
            this.panelStatusBar = new AntdUI.Panel();
            this.labelStatusCaption = new AntdUI.Label();
            this.labelStatusValue = new AntdUI.Label();
            this.titlebar.SuspendLayout();
            this.layoutRoot.SuspendLayout();
            this.panelStage.SuspendLayout();
            this.layoutShell.SuspendLayout();
            this.panelLeftCard.SuspendLayout();
            this.layoutLeft.SuspendLayout();
            this.panelPrimaryNav.SuspendLayout();
            this.panelAvatarHost.SuspendLayout();
            this.panelSecondaryNavCard.SuspendLayout();
            this.layoutWorkArea.SuspendLayout();
            this.panelHeaderCard.SuspendLayout();
            this.layoutHeader.SuspendLayout();
            this.panelContentCard.SuspendLayout();
            this.panelStatusBar.SuspendLayout();
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
            this.titlebar.Size = new System.Drawing.Size(1480, 48);
            this.titlebar.SubText = "工业设备控制";
            this.titlebar.TabIndex = 0;
            this.titlebar.Text = "AMControl WinForms";
            // 
            // dropdownTranslate
            // 
            this.dropdownTranslate.Dock = System.Windows.Forms.DockStyle.Right;
            this.dropdownTranslate.Ghost = true;
            this.dropdownTranslate.IconSvg = "TranslationOutlined";
            this.dropdownTranslate.Items.AddRange(new object[] {
            "简体中文",
            "English"});
            this.dropdownTranslate.Location = new System.Drawing.Point(1380, 0);
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
            this.buttonColorMode.Location = new System.Drawing.Point(1430, 0);
            this.buttonColorMode.Name = "buttonColorMode";
            this.buttonColorMode.Radius = 0;
            this.buttonColorMode.Size = new System.Drawing.Size(50, 48);
            this.buttonColorMode.TabIndex = 0;
            this.buttonColorMode.ToggleIconSvg = "MoonOutlined";
            this.buttonColorMode.WaveSize = 0;
            // 
            // layoutRoot
            // 
            this.layoutRoot.BackColor = System.Drawing.Color.Transparent;
            this.layoutRoot.ColumnCount = 1;
            this.layoutRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutRoot.Controls.Add(this.panelStage, 0, 0);
            this.layoutRoot.Controls.Add(this.panelStatusBar, 0, 1);
            this.layoutRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutRoot.Location = new System.Drawing.Point(0, 48);
            this.layoutRoot.Margin = new System.Windows.Forms.Padding(0);
            this.layoutRoot.Name = "layoutRoot";
            this.layoutRoot.RowCount = 2;
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.layoutRoot.Size = new System.Drawing.Size(1480, 852);
            this.layoutRoot.TabIndex = 1;
            // 
            // panelStage
            // 
            this.panelStage.Controls.Add(this.layoutShell);
            this.panelStage.Controls.Add(this.textureBackgroundMain);
            this.panelStage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStage.Location = new System.Drawing.Point(0, 0);
            this.panelStage.Margin = new System.Windows.Forms.Padding(0);
            this.panelStage.Name = "panelStage";
            this.panelStage.Size = new System.Drawing.Size(1480, 816);
            this.panelStage.TabIndex = 0;
            // 
            // layoutShell
            // 
            this.layoutShell.BackColor = System.Drawing.Color.Transparent;
            this.layoutShell.ColumnCount = 3;
            this.layoutShell.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.layoutShell.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.layoutShell.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutShell.Controls.Add(this.panelLeftCard, 0, 0);
            this.layoutShell.Controls.Add(this.panelSecondaryNavCard, 1, 0);
            this.layoutShell.Controls.Add(this.layoutWorkArea, 2, 0);
            this.layoutShell.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutShell.Location = new System.Drawing.Point(0, 0);
            this.layoutShell.Margin = new System.Windows.Forms.Padding(0);
            this.layoutShell.Name = "layoutShell";
            this.layoutShell.Padding = new System.Windows.Forms.Padding(14, 14, 14, 10);
            this.layoutShell.RowCount = 1;
            this.layoutShell.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutShell.Size = new System.Drawing.Size(1480, 816);
            this.layoutShell.TabIndex = 1;
            // 
            // panelLeftCard
            // 
            this.panelLeftCard.Back = System.Drawing.Color.White;
            this.panelLeftCard.Controls.Add(this.layoutLeft);
            this.panelLeftCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLeftCard.Location = new System.Drawing.Point(17, 17);
            this.panelLeftCard.Name = "panelLeftCard";
            this.panelLeftCard.Radius = 14;
            this.panelLeftCard.Shadow = 6;
            this.panelLeftCard.Size = new System.Drawing.Size(244, 786);
            this.panelLeftCard.TabIndex = 0;
            // 
            // layoutLeft
            // 
            this.layoutLeft.BackColor = System.Drawing.Color.Transparent;
            this.layoutLeft.ColumnCount = 1;
            this.layoutLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutLeft.Controls.Add(this.panelPrimaryNav, 0, 0);
            this.layoutLeft.Controls.Add(this.panelAvatarHost, 0, 1);
            this.layoutLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutLeft.Location = new System.Drawing.Point(0, 0);
            this.layoutLeft.Margin = new System.Windows.Forms.Padding(0);
            this.layoutLeft.Name = "layoutLeft";
            this.layoutLeft.RowCount = 2;
            this.layoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 94F));
            this.layoutLeft.Size = new System.Drawing.Size(244, 786);
            this.layoutLeft.TabIndex = 0;
            // 
            // panelPrimaryNav
            // 
            this.panelPrimaryNav.Back = System.Drawing.Color.Transparent;
            this.panelPrimaryNav.Controls.Add(this.menuPrimary);
            this.panelPrimaryNav.Controls.Add(this.labelPrimaryNavTitle);
            this.panelPrimaryNav.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPrimaryNav.Location = new System.Drawing.Point(0, 0);
            this.panelPrimaryNav.Margin = new System.Windows.Forms.Padding(0);
            this.panelPrimaryNav.Name = "panelPrimaryNav";
            this.panelPrimaryNav.Padding = new System.Windows.Forms.Padding(12, 14, 12, 8);
            this.panelPrimaryNav.Radius = 0;
            this.panelPrimaryNav.Size = new System.Drawing.Size(244, 692);
            this.panelPrimaryNav.TabIndex = 0;
            // 
            // labelPrimaryNavTitle
            // 
            this.labelPrimaryNavTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelPrimaryNavTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelPrimaryNavTitle.Location = new System.Drawing.Point(12, 14);
            this.labelPrimaryNavTitle.Name = "labelPrimaryNavTitle";
            this.labelPrimaryNavTitle.Size = new System.Drawing.Size(220, 30);
            this.labelPrimaryNavTitle.TabIndex = 0;
            this.labelPrimaryNavTitle.Text = "一级导航";
            // 
            // menuPrimary
            // 
            this.menuPrimary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuPrimary.Indent = true;
            this.menuPrimary.Location = new System.Drawing.Point(12, 44);
            this.menuPrimary.Name = "menuPrimary";
            this.menuPrimary.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.menuPrimary.Size = new System.Drawing.Size(220, 640);
            this.menuPrimary.TabIndex = 1;
            this.menuPrimary.Unique = true;
            // 
            // panelAvatarHost
            // 
            this.panelAvatarHost.Back = System.Drawing.Color.Transparent;
            this.panelAvatarHost.Controls.Add(this.userAvatarMenuControl);
            this.panelAvatarHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAvatarHost.Location = new System.Drawing.Point(0, 692);
            this.panelAvatarHost.Margin = new System.Windows.Forms.Padding(0);
            this.panelAvatarHost.Name = "panelAvatarHost";
            this.panelAvatarHost.Radius = 0;
            this.panelAvatarHost.Size = new System.Drawing.Size(244, 94);
            this.panelAvatarHost.TabIndex = 1;
            // 
            // userAvatarMenuControl
            // 
            this.userAvatarMenuControl.BackColor = System.Drawing.Color.Transparent;
            this.userAvatarMenuControl.Location = new System.Drawing.Point(86, 10);
            this.userAvatarMenuControl.Name = "userAvatarMenuControl";
            this.userAvatarMenuControl.Size = new System.Drawing.Size(72, 72);
            this.userAvatarMenuControl.TabIndex = 0;
            // 
            // panelSecondaryNavCard
            // 
            this.panelSecondaryNavCard.Back = System.Drawing.Color.White;
            this.panelSecondaryNavCard.Controls.Add(this.menuSecondary);
            this.panelSecondaryNavCard.Controls.Add(this.labelSecondaryNavTitle);
            this.panelSecondaryNavCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSecondaryNavCard.Location = new System.Drawing.Point(267, 17);
            this.panelSecondaryNavCard.Name = "panelSecondaryNavCard";
            this.panelSecondaryNavCard.Padding = new System.Windows.Forms.Padding(14, 14, 14, 8);
            this.panelSecondaryNavCard.Radius = 14;
            this.panelSecondaryNavCard.Shadow = 6;
            this.panelSecondaryNavCard.Size = new System.Drawing.Size(294, 786);
            this.panelSecondaryNavCard.TabIndex = 1;
            // 
            // labelSecondaryNavTitle
            // 
            this.labelSecondaryNavTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelSecondaryNavTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelSecondaryNavTitle.Location = new System.Drawing.Point(14, 14);
            this.labelSecondaryNavTitle.Name = "labelSecondaryNavTitle";
            this.labelSecondaryNavTitle.Size = new System.Drawing.Size(266, 30);
            this.labelSecondaryNavTitle.TabIndex = 0;
            this.labelSecondaryNavTitle.Text = "二级导航";
            // 
            // menuSecondary
            // 
            this.menuSecondary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuSecondary.Indent = true;
            this.menuSecondary.Location = new System.Drawing.Point(14, 44);
            this.menuSecondary.Name = "menuSecondary";
            this.menuSecondary.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.menuSecondary.Size = new System.Drawing.Size(266, 734);
            this.menuSecondary.TabIndex = 1;
            this.menuSecondary.Unique = true;
            // 
            // layoutWorkArea
            // 
            this.layoutWorkArea.BackColor = System.Drawing.Color.Transparent;
            this.layoutWorkArea.ColumnCount = 1;
            this.layoutWorkArea.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutWorkArea.Controls.Add(this.panelHeaderCard, 0, 0);
            this.layoutWorkArea.Controls.Add(this.panelContentCard, 0, 1);
            this.layoutWorkArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutWorkArea.Location = new System.Drawing.Point(567, 17);
            this.layoutWorkArea.Name = "layoutWorkArea";
            this.layoutWorkArea.RowCount = 2;
            this.layoutWorkArea.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 134F));
            this.layoutWorkArea.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutWorkArea.Size = new System.Drawing.Size(896, 786);
            this.layoutWorkArea.TabIndex = 2;
            // 
            // panelHeaderCard
            // 
            this.panelHeaderCard.Back = System.Drawing.Color.White;
            this.panelHeaderCard.Controls.Add(this.layoutHeader);
            this.panelHeaderCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHeaderCard.Location = new System.Drawing.Point(0, 0);
            this.panelHeaderCard.Margin = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.panelHeaderCard.Name = "panelHeaderCard";
            this.panelHeaderCard.Padding = new System.Windows.Forms.Padding(18, 16, 18, 16);
            this.panelHeaderCard.Radius = 14;
            this.panelHeaderCard.Shadow = 6;
            this.panelHeaderCard.Size = new System.Drawing.Size(896, 122);
            this.panelHeaderCard.TabIndex = 0;
            // 
            // layoutHeader
            // 
            this.layoutHeader.BackColor = System.Drawing.Color.Transparent;
            this.layoutHeader.ColumnCount = 2;
            this.layoutHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.layoutHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutHeader.Controls.Add(this.labelPrimaryTitleTitle, 0, 0);
            this.layoutHeader.Controls.Add(this.labelPrimaryTitleValue, 1, 0);
            this.layoutHeader.Controls.Add(this.labelPageTitleTitle, 0, 1);
            this.layoutHeader.Controls.Add(this.labelPageTitleValue, 1, 1);
            this.layoutHeader.Controls.Add(this.labelPageDescriptionTitle, 0, 2);
            this.layoutHeader.Controls.Add(this.labelPageDescriptionValue, 1, 2);
            this.layoutHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutHeader.Location = new System.Drawing.Point(18, 16);
            this.layoutHeader.Name = "layoutHeader";
            this.layoutHeader.RowCount = 3;
            this.layoutHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.layoutHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.layoutHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutHeader.Size = new System.Drawing.Size(860, 90);
            this.layoutHeader.TabIndex = 0;
            // 
            // labelPrimaryTitleTitle
            // 
            this.labelPrimaryTitleTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPrimaryTitleTitle.Location = new System.Drawing.Point(3, 0);
            this.labelPrimaryTitleTitle.Name = "labelPrimaryTitleTitle";
            this.labelPrimaryTitleTitle.Size = new System.Drawing.Size(104, 24);
            this.labelPrimaryTitleTitle.TabIndex = 0;
            this.labelPrimaryTitleTitle.Text = "当前模块";
            // 
            // labelPrimaryTitleValue
            // 
            this.labelPrimaryTitleValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPrimaryTitleValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelPrimaryTitleValue.Location = new System.Drawing.Point(113, 0);
            this.labelPrimaryTitleValue.Name = "labelPrimaryTitleValue";
            this.labelPrimaryTitleValue.Size = new System.Drawing.Size(744, 24);
            this.labelPrimaryTitleValue.TabIndex = 1;
            this.labelPrimaryTitleValue.Text = "-";
            // 
            // labelPageTitleTitle
            // 
            this.labelPageTitleTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPageTitleTitle.Location = new System.Drawing.Point(3, 24);
            this.labelPageTitleTitle.Name = "labelPageTitleTitle";
            this.labelPageTitleTitle.Size = new System.Drawing.Size(104, 34);
            this.labelPageTitleTitle.TabIndex = 2;
            this.labelPageTitleTitle.Text = "当前页面";
            // 
            // labelPageTitleValue
            // 
            this.labelPageTitleValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPageTitleValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold);
            this.labelPageTitleValue.Location = new System.Drawing.Point(113, 24);
            this.labelPageTitleValue.Name = "labelPageTitleValue";
            this.labelPageTitleValue.Size = new System.Drawing.Size(744, 34);
            this.labelPageTitleValue.TabIndex = 3;
            this.labelPageTitleValue.Text = "-";
            // 
            // labelPageDescriptionTitle
            // 
            this.labelPageDescriptionTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPageDescriptionTitle.Location = new System.Drawing.Point(3, 58);
            this.labelPageDescriptionTitle.Name = "labelPageDescriptionTitle";
            this.labelPageDescriptionTitle.Size = new System.Drawing.Size(104, 32);
            this.labelPageDescriptionTitle.TabIndex = 4;
            this.labelPageDescriptionTitle.Text = "页面说明";
            // 
            // labelPageDescriptionValue
            // 
            this.labelPageDescriptionValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPageDescriptionValue.Location = new System.Drawing.Point(113, 58);
            this.labelPageDescriptionValue.Name = "labelPageDescriptionValue";
            this.labelPageDescriptionValue.Size = new System.Drawing.Size(744, 32);
            this.labelPageDescriptionValue.TabIndex = 5;
            this.labelPageDescriptionValue.Text = "-";
            // 
            // panelContentCard
            // 
            this.panelContentCard.Back = System.Drawing.Color.White;
            this.panelContentCard.Controls.Add(this.panelContent);
            this.panelContentCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContentCard.Location = new System.Drawing.Point(0, 134);
            this.panelContentCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelContentCard.Name = "panelContentCard";
            this.panelContentCard.Padding = new System.Windows.Forms.Padding(0);
            this.panelContentCard.Radius = 14;
            this.panelContentCard.Shadow = 6;
            this.panelContentCard.Size = new System.Drawing.Size(896, 652);
            this.panelContentCard.TabIndex = 1;
            // 
            // panelContent
            // 
            this.panelContent.Back = System.Drawing.Color.White;
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 0);
            this.panelContent.Name = "panelContent";
            this.panelContent.Radius = 0;
            this.panelContent.Size = new System.Drawing.Size(896, 652);
            this.panelContent.TabIndex = 0;
            // 
            // textureBackgroundMain
            // 
            this.textureBackgroundMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textureBackgroundMain.Location = new System.Drawing.Point(0, 0);
            this.textureBackgroundMain.Name = "textureBackgroundMain";
            this.textureBackgroundMain.Size = new System.Drawing.Size(1480, 816);
            this.textureBackgroundMain.TabIndex = 0;
            // 
            // panelStatusBar
            // 
            this.panelStatusBar.Back = System.Drawing.Color.White;
            this.panelStatusBar.Controls.Add(this.labelStatusValue);
            this.panelStatusBar.Controls.Add(this.labelStatusCaption);
            this.panelStatusBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStatusBar.Location = new System.Drawing.Point(0, 816);
            this.panelStatusBar.Margin = new System.Windows.Forms.Padding(0);
            this.panelStatusBar.Name = "panelStatusBar";
            this.panelStatusBar.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.panelStatusBar.Radius = 0;
            this.panelStatusBar.Size = new System.Drawing.Size(1480, 36);
            this.panelStatusBar.TabIndex = 1;
            // 
            // labelStatusCaption
            // 
            this.labelStatusCaption.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelStatusCaption.Location = new System.Drawing.Point(12, 0);
            this.labelStatusCaption.Name = "labelStatusCaption";
            this.labelStatusCaption.Size = new System.Drawing.Size(88, 36);
            this.labelStatusCaption.TabIndex = 0;
            this.labelStatusCaption.Text = "系统状态：";
            // 
            // labelStatusValue
            // 
            this.labelStatusValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelStatusValue.Location = new System.Drawing.Point(100, 0);
            this.labelStatusValue.Name = "labelStatusValue";
            this.labelStatusValue.Size = new System.Drawing.Size(1368, 36);
            this.labelStatusValue.TabIndex = 1;
            this.labelStatusValue.Text = "-";
            // 
            // MainWindow
            // 
            this.ClientSize = new System.Drawing.Size(1480, 900);
            this.ControlBox = false;
            this.Controls.Add(this.layoutRoot);
            this.Controls.Add(this.titlebar);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.MinimumSize = new System.Drawing.Size(1320, 840);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AMControl WinForms";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.titlebar.ResumeLayout(false);
            this.layoutRoot.ResumeLayout(false);
            this.panelStage.ResumeLayout(false);
            this.layoutShell.ResumeLayout(false);
            this.panelLeftCard.ResumeLayout(false);
            this.layoutLeft.ResumeLayout(false);
            this.panelPrimaryNav.ResumeLayout(false);
            this.panelAvatarHost.ResumeLayout(false);
            this.panelSecondaryNavCard.ResumeLayout(false);
            this.layoutWorkArea.ResumeLayout(false);
            this.panelHeaderCard.ResumeLayout(false);
            this.layoutHeader.ResumeLayout(false);
            this.panelContentCard.ResumeLayout(false);
            this.panelStatusBar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.PageHeader titlebar;
        private AntdUI.Dropdown dropdownTranslate;
        private AntdUI.Button buttonColorMode;
        private System.Windows.Forms.TableLayoutPanel layoutRoot;
        private System.Windows.Forms.Panel panelStage;
        private AMControlWinF.Views.Main.TextureBackgroundControl textureBackgroundMain;
        private System.Windows.Forms.TableLayoutPanel layoutShell;
        private AntdUI.Panel panelLeftCard;
        private System.Windows.Forms.TableLayoutPanel layoutLeft;
        private AntdUI.Panel panelPrimaryNav;
        private AntdUI.Label labelPrimaryNavTitle;
        private AntdUI.Menu menuPrimary;
        private AntdUI.Panel panelAvatarHost;
        private AMControlWinF.Views.Main.UserAvatarMenuControl userAvatarMenuControl;
        private AntdUI.Panel panelSecondaryNavCard;
        private AntdUI.Label labelSecondaryNavTitle;
        private AntdUI.Menu menuSecondary;
        private System.Windows.Forms.TableLayoutPanel layoutWorkArea;
        private AntdUI.Panel panelHeaderCard;
        private System.Windows.Forms.TableLayoutPanel layoutHeader;
        private AntdUI.Label labelPrimaryTitleTitle;
        private AntdUI.Label labelPrimaryTitleValue;
        private AntdUI.Label labelPageTitleTitle;
        private AntdUI.Label labelPageTitleValue;
        private AntdUI.Label labelPageDescriptionTitle;
        private AntdUI.Label labelPageDescriptionValue;
        private AntdUI.Panel panelContentCard;
        private AntdUI.Panel panelContent;
        private AntdUI.Panel panelStatusBar;
        private AntdUI.Label labelStatusCaption;
        private AntdUI.Label labelStatusValue;
    }
}