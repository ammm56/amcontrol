namespace AMControlWinF.Views.MotionConfig
{
    partial class MotionIoMapManagementPage
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelRoot = new AntdUI.Panel();
            this.panelCardsHost = new AntdUI.Panel();
            this.flowCards = new AntdUI.FlowPanel();
            this.labelCardsTitle = new AntdUI.Label();
            this.panelPlaceholder = new AntdUI.Panel();
            this.panelToolbar = new AntdUI.Panel();
            this.flowToolbarRight = new AntdUI.FlowPanel();
            this.buttonAddIoMap = new AntdUI.Button();
            this.buttonRefresh = new AntdUI.Button();
            this.flowToolbarLeft = new AntdUI.FlowPanel();
            this.labelSelectedCard = new AntdUI.Label();
            this.buttonFilterDO = new AntdUI.Button();
            this.buttonFilterDI = new AntdUI.Button();
            this.buttonFilterAll = new AntdUI.Button();
            this.buttonSelectCard = new AntdUI.Button();
            this.panelRoot.SuspendLayout();
            this.panelCardsHost.SuspendLayout();
            this.panelToolbar.SuspendLayout();
            this.flowToolbarRight.SuspendLayout();
            this.flowToolbarLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.panelCardsHost);
            this.panelRoot.Controls.Add(this.panelPlaceholder);
            this.panelRoot.Controls.Add(this.panelToolbar);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Margin = new System.Windows.Forms.Padding(0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(8);
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(850, 680);
            this.panelRoot.TabIndex = 0;
            // 
            // panelCardsHost
            // 
            this.panelCardsHost.BackColor = System.Drawing.Color.Transparent;
            this.panelCardsHost.Controls.Add(this.flowCards);
            this.panelCardsHost.Controls.Add(this.labelCardsTitle);
            this.panelCardsHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCardsHost.Location = new System.Drawing.Point(8, 62);
            this.panelCardsHost.Margin = new System.Windows.Forms.Padding(0);
            this.panelCardsHost.Name = "panelCardsHost";
            this.panelCardsHost.Padding = new System.Windows.Forms.Padding(8);
            this.panelCardsHost.Radius = 12;
            this.panelCardsHost.Shadow = 4;
            this.panelCardsHost.Size = new System.Drawing.Size(834, 610);
            this.panelCardsHost.TabIndex = 2;
            // 
            // flowCards
            // 
            this.flowCards.AutoScroll = true;
            this.flowCards.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowCards.Gap = 12;
            this.flowCards.Location = new System.Drawing.Point(12, 48);
            this.flowCards.Margin = new System.Windows.Forms.Padding(0);
            this.flowCards.Name = "flowCards";
            this.flowCards.Size = new System.Drawing.Size(810, 550);
            this.flowCards.TabIndex = 1;
            this.flowCards.Text = "flowCards";
            // 
            // labelCardsTitle
            // 
            this.labelCardsTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelCardsTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.labelCardsTitle.Location = new System.Drawing.Point(12, 12);
            this.labelCardsTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelCardsTitle.Name = "labelCardsTitle";
            this.labelCardsTitle.Size = new System.Drawing.Size(810, 36);
            this.labelCardsTitle.TabIndex = 0;
            this.labelCardsTitle.Text = "IO 映射列表";
            this.labelCardsTitle.Visible = false;
            // 
            // panelPlaceholder
            // 
            this.panelPlaceholder.BackColor = System.Drawing.Color.Transparent;
            this.panelPlaceholder.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPlaceholder.Location = new System.Drawing.Point(8, 52);
            this.panelPlaceholder.Margin = new System.Windows.Forms.Padding(0);
            this.panelPlaceholder.Name = "panelPlaceholder";
            this.panelPlaceholder.Padding = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.panelPlaceholder.Radius = 12;
            this.panelPlaceholder.Shadow = 4;
            this.panelPlaceholder.Size = new System.Drawing.Size(834, 10);
            this.panelPlaceholder.TabIndex = 1;
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
            this.flowToolbarRight.Controls.Add(this.buttonAddIoMap);
            this.flowToolbarRight.Controls.Add(this.buttonRefresh);
            this.flowToolbarRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowToolbarRight.Gap = 8;
            this.flowToolbarRight.Location = new System.Drawing.Point(594, 4);
            this.flowToolbarRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowToolbarRight.Name = "flowToolbarRight";
            this.flowToolbarRight.Size = new System.Drawing.Size(236, 36);
            this.flowToolbarRight.TabIndex = 1;
            this.flowToolbarRight.Text = "flowToolbarRight";
            // 
            // buttonAddIoMap
            // 
            this.buttonAddIoMap.IconSvg = "PlusOutlined";
            this.buttonAddIoMap.Location = new System.Drawing.Point(108, 0);
            this.buttonAddIoMap.Margin = new System.Windows.Forms.Padding(0);
            this.buttonAddIoMap.Name = "buttonAddIoMap";
            this.buttonAddIoMap.Radius = 8;
            this.buttonAddIoMap.Size = new System.Drawing.Size(120, 36);
            this.buttonAddIoMap.TabIndex = 0;
            this.buttonAddIoMap.Text = "新增 IO";
            this.buttonAddIoMap.Type = AntdUI.TTypeMini.Primary;
            this.buttonAddIoMap.WaveSize = 0;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.IconSvg = "ReloadOutlined";
            this.buttonRefresh.Location = new System.Drawing.Point(0, 0);
            this.buttonRefresh.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Radius = 8;
            this.buttonRefresh.Size = new System.Drawing.Size(100, 36);
            this.buttonRefresh.TabIndex = 1;
            this.buttonRefresh.Text = "刷新";
            this.buttonRefresh.WaveSize = 0;
            // 
            // flowToolbarLeft
            // 
            
            this.flowToolbarLeft.Controls.Add(this.buttonFilterDO);
            this.flowToolbarLeft.Controls.Add(this.buttonFilterDI);
            this.flowToolbarLeft.Controls.Add(this.buttonFilterAll);
            this.flowToolbarLeft.Controls.Add(this.labelSelectedCard);
            this.flowToolbarLeft.Controls.Add(this.buttonSelectCard);
            this.flowToolbarLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowToolbarLeft.Gap = 8;
            this.flowToolbarLeft.Location = new System.Drawing.Point(4, 4);
            this.flowToolbarLeft.Margin = new System.Windows.Forms.Padding(0);
            this.flowToolbarLeft.Name = "flowToolbarLeft";
            this.flowToolbarLeft.Size = new System.Drawing.Size(560, 36);
            this.flowToolbarLeft.TabIndex = 0;
            this.flowToolbarLeft.Text = "flowToolbarLeft";
            // 
            // labelSelectedCard
            // 
            this.labelSelectedCard.Location = new System.Drawing.Point(412, 0);
            this.labelSelectedCard.Margin = new System.Windows.Forms.Padding(0);
            this.labelSelectedCard.Name = "labelSelectedCard";
            this.labelSelectedCard.Size = new System.Drawing.Size(148, 36);
            this.labelSelectedCard.TabIndex = 4;
            this.labelSelectedCard.Text = "当前：全部控制卡";
            // 
            // buttonFilterDO
            // 
            this.buttonFilterDO.Location = new System.Drawing.Point(324, 0);
            this.buttonFilterDO.Margin = new System.Windows.Forms.Padding(0);
            this.buttonFilterDO.Name = "buttonFilterDO";
            this.buttonFilterDO.Radius = 8;
            this.buttonFilterDO.Size = new System.Drawing.Size(80, 36);
            this.buttonFilterDO.TabIndex = 3;
            this.buttonFilterDO.Text = "DO";
            this.buttonFilterDO.WaveSize = 0;
            // 
            // buttonFilterDI
            // 
            this.buttonFilterDI.Location = new System.Drawing.Point(236, 0);
            this.buttonFilterDI.Margin = new System.Windows.Forms.Padding(0);
            this.buttonFilterDI.Name = "buttonFilterDI";
            this.buttonFilterDI.Radius = 8;
            this.buttonFilterDI.Size = new System.Drawing.Size(80, 36);
            this.buttonFilterDI.TabIndex = 2;
            this.buttonFilterDI.Text = "DI";
            this.buttonFilterDI.WaveSize = 0;
            // 
            // buttonFilterAll
            // 
            this.buttonFilterAll.Location = new System.Drawing.Point(148, 0);
            this.buttonFilterAll.Margin = new System.Windows.Forms.Padding(0);
            this.buttonFilterAll.Name = "buttonFilterAll";
            this.buttonFilterAll.Radius = 8;
            this.buttonFilterAll.Size = new System.Drawing.Size(80, 36);
            this.buttonFilterAll.TabIndex = 1;
            this.buttonFilterAll.Text = "全部";
            this.buttonFilterAll.Type = AntdUI.TTypeMini.Primary;
            this.buttonFilterAll.WaveSize = 0;
            // 
            // buttonSelectCard
            // 
            this.buttonSelectCard.IconSvg = "CreditCardOutlined";
            this.buttonSelectCard.Location = new System.Drawing.Point(0, 0);
            this.buttonSelectCard.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSelectCard.Name = "buttonSelectCard";
            this.buttonSelectCard.Radius = 8;
            this.buttonSelectCard.Size = new System.Drawing.Size(140, 36);
            this.buttonSelectCard.TabIndex = 0;
            this.buttonSelectCard.Text = "选择控制卡";
            this.buttonSelectCard.Type = AntdUI.TTypeMini.Primary;
            this.buttonSelectCard.WaveSize = 0;
            // 
            // MotionIoMapManagementPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MotionIoMapManagementPage";
            this.Size = new System.Drawing.Size(850, 680);
            this.panelRoot.ResumeLayout(false);
            this.panelCardsHost.ResumeLayout(false);
            this.panelToolbar.ResumeLayout(false);
            this.flowToolbarRight.ResumeLayout(false);
            this.flowToolbarLeft.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AntdUI.Panel panelRoot;
        private AntdUI.Panel panelToolbar;
        private AntdUI.FlowPanel flowToolbarLeft;
        private AntdUI.Button buttonSelectCard;
        private AntdUI.Button buttonFilterAll;
        private AntdUI.Button buttonFilterDI;
        private AntdUI.Button buttonFilterDO;
        private AntdUI.Label labelSelectedCard;
        private AntdUI.FlowPanel flowToolbarRight;
        private AntdUI.Button buttonRefresh;
        private AntdUI.Button buttonAddIoMap;
        private AntdUI.Panel panelPlaceholder;
        private AntdUI.Panel panelCardsHost;
        private AntdUI.Label labelCardsTitle;
        private AntdUI.FlowPanel flowCards;
    }
}