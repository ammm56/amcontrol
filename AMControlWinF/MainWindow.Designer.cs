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
            this.textureBackgroundMain = new AMControlWinF.Views.Main.TextureBackgroundControl();
            this.layoutShell = new System.Windows.Forms.TableLayoutPanel();
            this.panelLeftCard = new AntdUI.Panel();
            this.layoutLeft = new System.Windows.Forms.TableLayoutPanel();
            this.menuPrimary = new AntdUI.Menu();
            this.panelAvatarHost = new AntdUI.Panel();
            this.userAvatarMenuControl = new AMControlWinF.Views.Main.UserAvatarMenuControl();
            this.panelSecondaryNavCard = new AntdUI.Panel();
            this.menuSecondary = new AntdUI.Menu();
            this.panelWorkCard = new AntdUI.Panel();
            this.layoutWorkCard = new System.Windows.Forms.TableLayoutPanel();
            this.panelWorkHeader = new AntdUI.Panel();
            this.layoutHeader = new System.Windows.Forms.TableLayoutPanel();
            this.labelPrimaryTitleValue = new AntdUI.Label();
            this.labelPageTitleValue = new AntdUI.Label();
            this.labelPageDescriptionValue = new AntdUI.Label();
            this.panelContent = new AntdUI.Panel();
            this.panelStatusHost = new System.Windows.Forms.Panel();
            this.panelStatusCard = new AntdUI.Panel();
            this.labelStatusValue = new AntdUI.Label();
            this.labelStatusCaption = new AntdUI.Label();
            this.titlebar.SuspendLayout();
            this.textureBackgroundMain.SuspendLayout();
            this.layoutShell.SuspendLayout();
            this.panelLeftCard.SuspendLayout();
            this.layoutLeft.SuspendLayout();
            this.panelAvatarHost.SuspendLayout();
            this.panelSecondaryNavCard.SuspendLayout();
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
            this.titlebar.Size = new System.Drawing.Size(1280, 48);
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
            this.dropdownTranslate.Location = new System.Drawing.Point(1000, 0);
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
            this.buttonColorMode.Location = new System.Drawing.Point(1050, 0);
            this.buttonColorMode.Name = "buttonColorMode";
            this.buttonColorMode.Radius = 0;
            this.buttonColorMode.Size = new System.Drawing.Size(50, 48);
            this.buttonColorMode.TabIndex = 0;
            this.buttonColorMode.ToggleIconSvg = "MoonOutlined";
            this.buttonColorMode.WaveSize = 0;
            // 
            // textureBackgroundMain
            // 
            this.textureBackgroundMain.Controls.Add(this.layoutShell);
            this.textureBackgroundMain.Controls.Add(this.panelStatusHost);
            this.textureBackgroundMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textureBackgroundMain.Location = new System.Drawing.Point(0, 48);
            this.textureBackgroundMain.Name = "textureBackgroundMain";
            this.textureBackgroundMain.Size = new System.Drawing.Size(1280, 752);
            this.textureBackgroundMain.TabIndex = 1;
            // 
            // layoutShell
            // 
            this.layoutShell.BackColor = System.Drawing.Color.Transparent;
            this.layoutShell.ColumnCount = 3;
            this.layoutShell.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 92F));
            this.layoutShell.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 248F));
            this.layoutShell.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutShell.Controls.Add(this.panelLeftCard, 0, 0);
            this.layoutShell.Controls.Add(this.panelSecondaryNavCard, 1, 0);
            this.layoutShell.Controls.Add(this.panelWorkCard, 2, 0);
            this.layoutShell.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutShell.Location = new System.Drawing.Point(0, 0);
            this.layoutShell.Margin = new System.Windows.Forms.Padding(0);
            this.layoutShell.Name = "layoutShell";
            this.layoutShell.Padding = new System.Windows.Forms.Padding(14, 14, 14, 8);
            this.layoutShell.RowCount = 1;
            this.layoutShell.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutShell.Size = new System.Drawing.Size(1280, 696);
            this.layoutShell.TabIndex = 0;
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
            this.panelLeftCard.Size = new System.Drawing.Size(86, 668);
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
            this.layoutLeft.Location = new System.Drawing.Point(7, 7);
            this.layoutLeft.Margin = new System.Windows.Forms.Padding(0);
            this.layoutLeft.Name = "layoutLeft";
            this.layoutLeft.RowCount = 2;
            this.layoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 84F));
            this.layoutLeft.Size = new System.Drawing.Size(72, 654);
            this.layoutLeft.TabIndex = 0;
            // 
            // menuPrimary
            // 
            this.menuPrimary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuPrimary.Indent = true;
            this.menuPrimary.Location = new System.Drawing.Point(6, 10);
            this.menuPrimary.Margin = new System.Windows.Forms.Padding(6, 10, 6, 0);
            this.menuPrimary.Name = "menuPrimary";
            this.menuPrimary.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.menuPrimary.Size = new System.Drawing.Size(60, 560);
            this.menuPrimary.TabIndex = 0;
            this.menuPrimary.Unique = true;
            // 
            // panelAvatarHost
            // 
            this.panelAvatarHost.Back = System.Drawing.Color.Transparent;
            this.panelAvatarHost.Controls.Add(this.userAvatarMenuControl);
            this.panelAvatarHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAvatarHost.Location = new System.Drawing.Point(0, 570);
            this.panelAvatarHost.Margin = new System.Windows.Forms.Padding(0);
            this.panelAvatarHost.Name = "panelAvatarHost";
            this.panelAvatarHost.Radius = 0;
            this.panelAvatarHost.Size = new System.Drawing.Size(72, 84);
            this.panelAvatarHost.TabIndex = 1;
            // 
            // userAvatarMenuControl
            // 
            this.userAvatarMenuControl.BackColor = System.Drawing.Color.Transparent;
            this.userAvatarMenuControl.Location = new System.Drawing.Point(7, 6);
            this.userAvatarMenuControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userAvatarMenuControl.Name = "userAvatarMenuControl";
            this.userAvatarMenuControl.RoleDisplayName = "用户";
            this.userAvatarMenuControl.Size = new System.Drawing.Size(72, 72);
            this.userAvatarMenuControl.TabIndex = 0;
            this.userAvatarMenuControl.UserDisplayName = "未登录";
            // 
            // panelSecondaryNavCard
            // 
            this.panelSecondaryNavCard.Back = System.Drawing.Color.White;
            this.panelSecondaryNavCard.Controls.Add(this.menuSecondary);
            this.panelSecondaryNavCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSecondaryNavCard.Location = new System.Drawing.Point(109, 17);
            this.panelSecondaryNavCard.Name = "panelSecondaryNavCard";
            this.panelSecondaryNavCard.Padding = new System.Windows.Forms.Padding(10);
            this.panelSecondaryNavCard.Radius = 14;
            this.panelSecondaryNavCard.Shadow = 6;
            this.panelSecondaryNavCard.Size = new System.Drawing.Size(242, 668);
            this.panelSecondaryNavCard.TabIndex = 1;
            // 
            // menuSecondary
            // 
            this.menuSecondary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuSecondary.Indent = true;
            this.menuSecondary.Location = new System.Drawing.Point(17, 17);
            this.menuSecondary.Name = "menuSecondary";
            this.menuSecondary.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.menuSecondary.Size = new System.Drawing.Size(208, 634);
            this.menuSecondary.TabIndex = 0;
            this.menuSecondary.Unique = true;
            // 
            // panelWorkCard
            // 
            this.panelWorkCard.Back = System.Drawing.Color.White;
            this.panelWorkCard.Controls.Add(this.layoutWorkCard);
            this.panelWorkCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWorkCard.Location = new System.Drawing.Point(357, 17);
            this.panelWorkCard.Name = "panelWorkCard";
            this.panelWorkCard.Radius = 14;
            this.panelWorkCard.Shadow = 6;
            this.panelWorkCard.Size = new System.Drawing.Size(906, 668);
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
            this.layoutWorkCard.Location = new System.Drawing.Point(7, 7);
            this.layoutWorkCard.Margin = new System.Windows.Forms.Padding(0);
            this.layoutWorkCard.Name = "layoutWorkCard";
            this.layoutWorkCard.RowCount = 2;
            this.layoutWorkCard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 82F));
            this.layoutWorkCard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutWorkCard.Size = new System.Drawing.Size(892, 654);
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
            this.panelWorkHeader.Padding = new System.Windows.Forms.Padding(18, 16, 18, 12);
            this.panelWorkHeader.Radius = 0;
            this.panelWorkHeader.Size = new System.Drawing.Size(892, 82);
            this.panelWorkHeader.TabIndex = 0;
            // 
            // layoutHeader
            // 
            this.layoutHeader.BackColor = System.Drawing.Color.Transparent;
            this.layoutHeader.ColumnCount = 2;
            this.layoutHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 360F));
            this.layoutHeader.Controls.Add(this.labelPrimaryTitleValue, 0, 0);
            this.layoutHeader.Controls.Add(this.labelPageTitleValue, 0, 1);
            this.layoutHeader.Controls.Add(this.labelPageDescriptionValue, 1, 0);
            this.layoutHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutHeader.Location = new System.Drawing.Point(18, 16);
            this.layoutHeader.Margin = new System.Windows.Forms.Padding(0);
            this.layoutHeader.Name = "layoutHeader";
            this.layoutHeader.RowCount = 2;
            this.layoutHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.layoutHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.layoutHeader.Size = new System.Drawing.Size(856, 54);
            this.layoutHeader.TabIndex = 0;
            // 
            // labelPrimaryTitleValue
            // 
            this.labelPrimaryTitleValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPrimaryTitleValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 13F, System.Drawing.FontStyle.Bold);
            this.labelPrimaryTitleValue.Location = new System.Drawing.Point(3, 3);
            this.labelPrimaryTitleValue.Name = "labelPrimaryTitleValue";
            this.labelPrimaryTitleValue.Size = new System.Drawing.Size(490, 18);
            this.labelPrimaryTitleValue.TabIndex = 0;
            this.labelPrimaryTitleValue.Text = "-";
            // 
            // labelPageTitleValue
            // 
            this.labelPageTitleValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPageTitleValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 16F, System.Drawing.FontStyle.Bold);
            this.labelPageTitleValue.Location = new System.Drawing.Point(3, 27);
            this.labelPageTitleValue.Name = "labelPageTitleValue";
            this.labelPageTitleValue.Size = new System.Drawing.Size(490, 28);
            this.labelPageTitleValue.TabIndex = 1;
            this.labelPageTitleValue.Text = "-";
            // 
            // labelPageDescriptionValue
            // 
            this.labelPageDescriptionValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPageDescriptionValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.5F);
            this.labelPageDescriptionValue.Location = new System.Drawing.Point(499, 3);
            this.labelPageDescriptionValue.Name = "labelPageDescriptionValue";
            this.layoutHeader.SetRowSpan(this.labelPageDescriptionValue, 2);
            this.labelPageDescriptionValue.Size = new System.Drawing.Size(354, 52);
            this.labelPageDescriptionValue.TabIndex = 2;
            this.labelPageDescriptionValue.Text = "-";
            this.labelPageDescriptionValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panelContent
            // 
            this.panelContent.Back = System.Drawing.Color.Transparent;
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 82);
            this.panelContent.Margin = new System.Windows.Forms.Padding(0);
            this.panelContent.Name = "panelContent";
            this.panelContent.Padding = new System.Windows.Forms.Padding(18, 0, 18, 18);
            this.panelContent.Radius = 0;
            this.panelContent.Size = new System.Drawing.Size(892, 572);
            this.panelContent.TabIndex = 1;
            // 
            // panelStatusHost
            // 
            this.panelStatusHost.BackColor = System.Drawing.Color.Transparent;
            this.panelStatusHost.Controls.Add(this.panelStatusCard);
            this.panelStatusHost.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelStatusHost.Location = new System.Drawing.Point(0, 696);
            this.panelStatusHost.Margin = new System.Windows.Forms.Padding(0);
            this.panelStatusHost.Name = "panelStatusHost";
            this.panelStatusHost.Padding = new System.Windows.Forms.Padding(14, 0, 14, 10);
            this.panelStatusHost.Size = new System.Drawing.Size(1280, 56);
            this.panelStatusHost.TabIndex = 1;
            // 
            // panelStatusCard
            // 
            this.panelStatusCard.Back = System.Drawing.Color.White;
            this.panelStatusCard.Controls.Add(this.labelStatusValue);
            this.panelStatusCard.Controls.Add(this.labelStatusCaption);
            this.panelStatusCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStatusCard.Location = new System.Drawing.Point(14, 0);
            this.panelStatusCard.Name = "panelStatusCard";
            this.panelStatusCard.Padding = new System.Windows.Forms.Padding(14, 0, 14, 0);
            this.panelStatusCard.Radius = 12;
            this.panelStatusCard.Shadow = 4;
            this.panelStatusCard.Size = new System.Drawing.Size(1252, 46);
            this.panelStatusCard.TabIndex = 0;
            // 
            // labelStatusValue
            // 
            this.labelStatusValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelStatusValue.Location = new System.Drawing.Point(107, 5);
            this.labelStatusValue.Name = "labelStatusValue";
            this.labelStatusValue.Size = new System.Drawing.Size(1126, 36);
            this.labelStatusValue.TabIndex = 1;
            this.labelStatusValue.Text = "-";
            // 
            // labelStatusCaption
            // 
            this.labelStatusCaption.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelStatusCaption.Location = new System.Drawing.Point(19, 5);
            this.labelStatusCaption.Name = "labelStatusCaption";
            this.labelStatusCaption.Size = new System.Drawing.Size(88, 36);
            this.labelStatusCaption.TabIndex = 0;
            this.labelStatusCaption.Text = "系统状态：";
            // 
            // MainWindow
            // 
            this.ClientSize = new System.Drawing.Size(1280, 800);
            this.ControlBox = false;
            this.Controls.Add(this.textureBackgroundMain);
            this.Controls.Add(this.titlebar);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.MinimumSize = new System.Drawing.Size(1280, 800);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AMControl WinForms";
            this.titlebar.ResumeLayout(false);
            this.textureBackgroundMain.ResumeLayout(false);
            this.layoutShell.ResumeLayout(false);
            this.panelLeftCard.ResumeLayout(false);
            this.layoutLeft.ResumeLayout(false);
            this.panelAvatarHost.ResumeLayout(false);
            this.panelSecondaryNavCard.ResumeLayout(false);
            this.panelWorkCard.ResumeLayout(false);
            this.layoutWorkCard.ResumeLayout(false);
            this.panelWorkHeader.ResumeLayout(false);
            this.layoutHeader.ResumeLayout(false);
            this.panelStatusHost.ResumeLayout(false);
            this.panelStatusCard.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.PageHeader titlebar;
        private AntdUI.Dropdown dropdownTranslate;
        private AntdUI.Button buttonColorMode;
        private AMControlWinF.Views.Main.TextureBackgroundControl textureBackgroundMain;
        private System.Windows.Forms.TableLayoutPanel layoutShell;
        private AntdUI.Panel panelLeftCard;
        private System.Windows.Forms.TableLayoutPanel layoutLeft;
        private AntdUI.Menu menuPrimary;
        private AntdUI.Panel panelAvatarHost;
        private AMControlWinF.Views.Main.UserAvatarMenuControl userAvatarMenuControl;
        private AntdUI.Panel panelSecondaryNavCard;
        private AntdUI.Menu menuSecondary;
        private AntdUI.Panel panelWorkCard;
        private System.Windows.Forms.TableLayoutPanel layoutWorkCard;
        private AntdUI.Panel panelWorkHeader;
        private System.Windows.Forms.TableLayoutPanel layoutHeader;
        private AntdUI.Label labelPrimaryTitleValue;
        private AntdUI.Label labelPageTitleValue;
        private AntdUI.Label labelPageDescriptionValue;
        private AntdUI.Panel panelContent;
        private System.Windows.Forms.Panel panelStatusHost;
        private AntdUI.Panel panelStatusCard;
        private AntdUI.Label labelStatusCaption;
        private AntdUI.Label labelStatusValue;
    }
}