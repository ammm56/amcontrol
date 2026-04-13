namespace AMControlWinF.Views.Plc
{
    partial class PlcMonitorPage
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
            this.panelContentCard = new AntdUI.Panel();
            this.gridContent = new AntdUI.GridPanel();
            this.panelPointDetailCard = new AntdUI.Panel();
            this.plcPointDetailControl = new AMControlWinF.Views.Plc.PlcPointDetailControl();
            this.panelPointListCard = new AntdUI.Panel();
            this.plcPointVirtualListControl = new AMControlWinF.Views.Plc.PlcPointVirtualListControl();
            this.flowStats = new AntdUI.FlowPanel();
            this.panelErrorPointCard = new AntdUI.Panel();
            this.labelErrorPointCount = new AntdUI.Label();
            this.labelErrorPointTitle = new AntdUI.Label();
            this.panelOfflinePointCard = new AntdUI.Panel();
            this.labelOfflinePointCount = new AntdUI.Label();
            this.labelOfflinePointTitle = new AntdUI.Label();
            this.panelOnlinePointCard = new AntdUI.Panel();
            this.labelOnlinePointCount = new AntdUI.Label();
            this.labelOnlinePointTitle = new AntdUI.Label();
            this.panelTotalPointCard = new AntdUI.Panel();
            this.labelTotalPointCount = new AntdUI.Label();
            this.labelTotalPointTitle = new AntdUI.Label();
            this.panelToolbar = new AntdUI.Panel();
            this.flowToolbarLeft = new AntdUI.FlowPanel();
            this.labelRuntimeSummary = new AntdUI.Label();
            this.selectPlc = new AntdUI.Select();
            this.selectGroup = new AntdUI.Select();
            this.inputSearch = new AntdUI.Input();
            this.flowToolbarRight = new AntdUI.FlowPanel();
            this.buttonRefresh = new AntdUI.Button();
            this.panelRoot.SuspendLayout();
            this.panelContentCard.SuspendLayout();
            this.gridContent.SuspendLayout();
            this.panelPointDetailCard.SuspendLayout();
            this.panelPointListCard.SuspendLayout();
            this.flowStats.SuspendLayout();
            this.panelErrorPointCard.SuspendLayout();
            this.panelOfflinePointCard.SuspendLayout();
            this.panelOnlinePointCard.SuspendLayout();
            this.panelTotalPointCard.SuspendLayout();
            this.panelToolbar.SuspendLayout();
            this.flowToolbarLeft.SuspendLayout();
            this.flowToolbarRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.panelContentCard);
            this.panelRoot.Controls.Add(this.flowStats);
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
            // panelContentCard
            // 
            this.panelContentCard.BackColor = System.Drawing.Color.Transparent;
            this.panelContentCard.Controls.Add(this.gridContent);
            this.panelContentCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContentCard.Location = new System.Drawing.Point(8, 140);
            this.panelContentCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelContentCard.Name = "panelContentCard";
            this.panelContentCard.Radius = 0;
            this.panelContentCard.ShadowOpacity = 0F;
            this.panelContentCard.ShadowOpacityHover = 0F;
            this.panelContentCard.Size = new System.Drawing.Size(834, 532);
            this.panelContentCard.TabIndex = 2;
            // 
            // gridContent
            // 
            this.gridContent.Controls.Add(this.panelPointDetailCard);
            this.gridContent.Controls.Add(this.panelPointListCard);
            this.gridContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridContent.Location = new System.Drawing.Point(0, 0);
            this.gridContent.Margin = new System.Windows.Forms.Padding(0);
            this.gridContent.Name = "gridContent";
            this.gridContent.Size = new System.Drawing.Size(834, 532);
            this.gridContent.Span = "100% 250";
            this.gridContent.TabIndex = 0;
            // 
            // panelPointDetailCard
            // 
            this.panelPointDetailCard.BackColor = System.Drawing.Color.Transparent;
            this.panelPointDetailCard.Controls.Add(this.plcPointDetailControl);
            this.panelPointDetailCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPointDetailCard.Location = new System.Drawing.Point(584, 0);
            this.panelPointDetailCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelPointDetailCard.Name = "panelPointDetailCard";
            this.panelPointDetailCard.Padding = new System.Windows.Forms.Padding(8);
            this.panelPointDetailCard.Radius = 12;
            this.panelPointDetailCard.Shadow = 4;
            this.panelPointDetailCard.ShadowOpacity = 0.15F;
            this.panelPointDetailCard.Size = new System.Drawing.Size(250, 532);
            this.panelPointDetailCard.TabIndex = 1;
            // 
            // plcPointDetailControl
            // 
            this.plcPointDetailControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plcPointDetailControl.Location = new System.Drawing.Point(12, 12);
            this.plcPointDetailControl.Margin = new System.Windows.Forms.Padding(0);
            this.plcPointDetailControl.Name = "plcPointDetailControl";
            this.plcPointDetailControl.Size = new System.Drawing.Size(226, 508);
            this.plcPointDetailControl.TabIndex = 0;
            // 
            // panelPointListCard
            // 
            this.panelPointListCard.BackColor = System.Drawing.Color.Transparent;
            this.panelPointListCard.Controls.Add(this.plcPointVirtualListControl);
            this.panelPointListCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPointListCard.Location = new System.Drawing.Point(0, 0);
            this.panelPointListCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelPointListCard.Name = "panelPointListCard";
            this.panelPointListCard.Radius = 0;
            this.panelPointListCard.ShadowOpacity = 0F;
            this.panelPointListCard.ShadowOpacityHover = 0F;
            this.panelPointListCard.Size = new System.Drawing.Size(584, 532);
            this.panelPointListCard.TabIndex = 0;
            // 
            // plcPointVirtualListControl
            // 
            this.plcPointVirtualListControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plcPointVirtualListControl.Location = new System.Drawing.Point(0, 0);
            this.plcPointVirtualListControl.Margin = new System.Windows.Forms.Padding(0);
            this.plcPointVirtualListControl.Name = "plcPointVirtualListControl";
            this.plcPointVirtualListControl.Size = new System.Drawing.Size(584, 532);
            this.plcPointVirtualListControl.TabIndex = 0;
            // 
            // flowStats
            // 
            this.flowStats.Controls.Add(this.panelErrorPointCard);
            this.flowStats.Controls.Add(this.panelOfflinePointCard);
            this.flowStats.Controls.Add(this.panelOnlinePointCard);
            this.flowStats.Controls.Add(this.panelTotalPointCard);
            this.flowStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowStats.Gap = 8;
            this.flowStats.Location = new System.Drawing.Point(8, 52);
            this.flowStats.Margin = new System.Windows.Forms.Padding(0);
            this.flowStats.Name = "flowStats";
            this.flowStats.Padding = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.flowStats.Size = new System.Drawing.Size(834, 88);
            this.flowStats.TabIndex = 1;
            this.flowStats.Text = "flowStats";
            // 
            // panelErrorPointCard
            // 
            this.panelErrorPointCard.Controls.Add(this.labelErrorPointCount);
            this.panelErrorPointCard.Controls.Add(this.labelErrorPointTitle);
            this.panelErrorPointCard.Location = new System.Drawing.Point(568, 6);
            this.panelErrorPointCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelErrorPointCard.Name = "panelErrorPointCard";
            this.panelErrorPointCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelErrorPointCard.Radius = 12;
            this.panelErrorPointCard.Shadow = 4;
            this.panelErrorPointCard.ShadowOpacityAnimation = true;
            this.panelErrorPointCard.Size = new System.Drawing.Size(180, 72);
            this.panelErrorPointCard.TabIndex = 3;
            // 
            // labelErrorPointCount
            // 
            this.labelErrorPointCount.BackColor = System.Drawing.Color.Transparent;
            this.labelErrorPointCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelErrorPointCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelErrorPointCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(79)))));
            this.labelErrorPointCount.Location = new System.Drawing.Point(104, 16);
            this.labelErrorPointCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelErrorPointCount.Name = "labelErrorPointCount";
            this.labelErrorPointCount.Size = new System.Drawing.Size(60, 40);
            this.labelErrorPointCount.TabIndex = 1;
            this.labelErrorPointCount.Text = "0";
            // 
            // labelErrorPointTitle
            // 
            this.labelErrorPointTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelErrorPointTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelErrorPointTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(79)))));
            this.labelErrorPointTitle.Location = new System.Drawing.Point(16, 16);
            this.labelErrorPointTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelErrorPointTitle.Name = "labelErrorPointTitle";
            this.labelErrorPointTitle.Size = new System.Drawing.Size(72, 40);
            this.labelErrorPointTitle.TabIndex = 0;
            this.labelErrorPointTitle.Text = "异常";
            // 
            // panelOfflinePointCard
            // 
            this.panelOfflinePointCard.Controls.Add(this.labelOfflinePointCount);
            this.panelOfflinePointCard.Controls.Add(this.labelOfflinePointTitle);
            this.panelOfflinePointCard.Location = new System.Drawing.Point(380, 6);
            this.panelOfflinePointCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelOfflinePointCard.Name = "panelOfflinePointCard";
            this.panelOfflinePointCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelOfflinePointCard.Radius = 12;
            this.panelOfflinePointCard.Shadow = 4;
            this.panelOfflinePointCard.ShadowOpacityAnimation = true;
            this.panelOfflinePointCard.Size = new System.Drawing.Size(180, 72);
            this.panelOfflinePointCard.TabIndex = 2;
            // 
            // labelOfflinePointCount
            // 
            this.labelOfflinePointCount.BackColor = System.Drawing.Color.Transparent;
            this.labelOfflinePointCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelOfflinePointCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelOfflinePointCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.labelOfflinePointCount.Location = new System.Drawing.Point(104, 16);
            this.labelOfflinePointCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelOfflinePointCount.Name = "labelOfflinePointCount";
            this.labelOfflinePointCount.Size = new System.Drawing.Size(60, 40);
            this.labelOfflinePointCount.TabIndex = 1;
            this.labelOfflinePointCount.Text = "0";
            // 
            // labelOfflinePointTitle
            // 
            this.labelOfflinePointTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelOfflinePointTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelOfflinePointTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.labelOfflinePointTitle.Location = new System.Drawing.Point(16, 16);
            this.labelOfflinePointTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelOfflinePointTitle.Name = "labelOfflinePointTitle";
            this.labelOfflinePointTitle.Size = new System.Drawing.Size(72, 40);
            this.labelOfflinePointTitle.TabIndex = 0;
            this.labelOfflinePointTitle.Text = "离线";
            // 
            // panelOnlinePointCard
            // 
            this.panelOnlinePointCard.Controls.Add(this.labelOnlinePointCount);
            this.panelOnlinePointCard.Controls.Add(this.labelOnlinePointTitle);
            this.panelOnlinePointCard.Location = new System.Drawing.Point(192, 6);
            this.panelOnlinePointCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelOnlinePointCard.Name = "panelOnlinePointCard";
            this.panelOnlinePointCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelOnlinePointCard.Radius = 12;
            this.panelOnlinePointCard.Shadow = 4;
            this.panelOnlinePointCard.ShadowOpacityAnimation = true;
            this.panelOnlinePointCard.Size = new System.Drawing.Size(180, 72);
            this.panelOnlinePointCard.TabIndex = 1;
            // 
            // labelOnlinePointCount
            // 
            this.labelOnlinePointCount.BackColor = System.Drawing.Color.Transparent;
            this.labelOnlinePointCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelOnlinePointCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelOnlinePointCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(196)))), ((int)(((byte)(26)))));
            this.labelOnlinePointCount.Location = new System.Drawing.Point(104, 16);
            this.labelOnlinePointCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelOnlinePointCount.Name = "labelOnlinePointCount";
            this.labelOnlinePointCount.Size = new System.Drawing.Size(60, 40);
            this.labelOnlinePointCount.TabIndex = 1;
            this.labelOnlinePointCount.Text = "0";
            // 
            // labelOnlinePointTitle
            // 
            this.labelOnlinePointTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelOnlinePointTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelOnlinePointTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(196)))), ((int)(((byte)(26)))));
            this.labelOnlinePointTitle.Location = new System.Drawing.Point(16, 16);
            this.labelOnlinePointTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelOnlinePointTitle.Name = "labelOnlinePointTitle";
            this.labelOnlinePointTitle.Size = new System.Drawing.Size(72, 40);
            this.labelOnlinePointTitle.TabIndex = 0;
            this.labelOnlinePointTitle.Text = "在线";
            // 
            // panelTotalPointCard
            // 
            this.panelTotalPointCard.Controls.Add(this.labelTotalPointCount);
            this.panelTotalPointCard.Controls.Add(this.labelTotalPointTitle);
            this.panelTotalPointCard.Location = new System.Drawing.Point(4, 6);
            this.panelTotalPointCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelTotalPointCard.Name = "panelTotalPointCard";
            this.panelTotalPointCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelTotalPointCard.Radius = 12;
            this.panelTotalPointCard.Shadow = 4;
            this.panelTotalPointCard.ShadowOpacityAnimation = true;
            this.panelTotalPointCard.Size = new System.Drawing.Size(180, 72);
            this.panelTotalPointCard.TabIndex = 0;
            // 
            // labelTotalPointCount
            // 
            this.labelTotalPointCount.BackColor = System.Drawing.Color.Transparent;
            this.labelTotalPointCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelTotalPointCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelTotalPointCount.Location = new System.Drawing.Point(104, 16);
            this.labelTotalPointCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelTotalPointCount.Name = "labelTotalPointCount";
            this.labelTotalPointCount.Size = new System.Drawing.Size(60, 40);
            this.labelTotalPointCount.TabIndex = 1;
            this.labelTotalPointCount.Text = "0";
            // 
            // labelTotalPointTitle
            // 
            this.labelTotalPointTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelTotalPointTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelTotalPointTitle.Location = new System.Drawing.Point(16, 16);
            this.labelTotalPointTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTotalPointTitle.Name = "labelTotalPointTitle";
            this.labelTotalPointTitle.Size = new System.Drawing.Size(72, 40);
            this.labelTotalPointTitle.TabIndex = 0;
            this.labelTotalPointTitle.Text = "总点位";
            // 
            // panelToolbar
            // 
            this.panelToolbar.BackColor = System.Drawing.Color.Transparent;
            this.panelToolbar.Controls.Add(this.flowToolbarLeft);
            this.panelToolbar.Controls.Add(this.flowToolbarRight);
            this.panelToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelToolbar.Location = new System.Drawing.Point(8, 8);
            this.panelToolbar.Margin = new System.Windows.Forms.Padding(0);
            this.panelToolbar.Name = "panelToolbar";
            this.panelToolbar.Padding = new System.Windows.Forms.Padding(4);
            this.panelToolbar.Radius = 0;
            this.panelToolbar.ShadowOpacity = 0.15F;
            this.panelToolbar.Size = new System.Drawing.Size(834, 44);
            this.panelToolbar.TabIndex = 0;
            // 
            // flowToolbarLeft
            // 
            this.flowToolbarLeft.Controls.Add(this.labelRuntimeSummary);
            this.flowToolbarLeft.Controls.Add(this.inputSearch);
            this.flowToolbarLeft.Controls.Add(this.selectGroup);
            this.flowToolbarLeft.Controls.Add(this.selectPlc);
            this.flowToolbarLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowToolbarLeft.Gap = 8;
            this.flowToolbarLeft.Location = new System.Drawing.Point(4, 4);
            this.flowToolbarLeft.Margin = new System.Windows.Forms.Padding(0);
            this.flowToolbarLeft.Name = "flowToolbarLeft";
            this.flowToolbarLeft.Size = new System.Drawing.Size(604, 36);
            this.flowToolbarLeft.TabIndex = 0;
            this.flowToolbarLeft.Text = "flowToolbarLeft";
            // 
            // labelRuntimeSummary
            // 
            this.labelRuntimeSummary.Location = new System.Drawing.Point(414, 0);
            this.labelRuntimeSummary.Margin = new System.Windows.Forms.Padding(0);
            this.labelRuntimeSummary.Name = "labelRuntimeSummary";
            this.labelRuntimeSummary.Size = new System.Drawing.Size(180, 40);
            this.labelRuntimeSummary.TabIndex = 0;
            this.labelRuntimeSummary.Text = "状态未知";
            // 
            // selectPlc
            // 
            this.selectPlc.Location = new System.Drawing.Point(286, 4);
            this.selectPlc.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.selectPlc.Name = "selectPlc";
            this.selectPlc.Radius = 0;
            this.selectPlc.Size = new System.Drawing.Size(120, 32);
            this.selectPlc.TabIndex = 1;
            this.selectPlc.WaveSize = 0;
            // 
            // selectGroup
            // 
            this.selectGroup.Location = new System.Drawing.Point(158, 4);
            this.selectGroup.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.selectGroup.Name = "selectGroup";
            this.selectGroup.Radius = 0;
            this.selectGroup.Size = new System.Drawing.Size(120, 32);
            this.selectGroup.TabIndex = 2;
            this.selectGroup.WaveSize = 0;
            // 
            // inputSearch
            // 
            this.inputSearch.Location = new System.Drawing.Point(0, 4);
            this.inputSearch.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.inputSearch.Name = "inputSearch";
            this.inputSearch.PlaceholderText = "搜索点位名称 / 分组 / 地址 / 值";
            this.inputSearch.Size = new System.Drawing.Size(150, 32);
            this.inputSearch.TabIndex = 3;
            this.inputSearch.WaveSize = 0;
            // 
            // flowToolbarRight
            // 
            this.flowToolbarRight.Align = AntdUI.TAlignFlow.RightCenter;
            this.flowToolbarRight.Controls.Add(this.buttonRefresh);
            this.flowToolbarRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowToolbarRight.Gap = 8;
            this.flowToolbarRight.Location = new System.Drawing.Point(730, 4);
            this.flowToolbarRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowToolbarRight.Name = "flowToolbarRight";
            this.flowToolbarRight.Size = new System.Drawing.Size(100, 36);
            this.flowToolbarRight.TabIndex = 1;
            this.flowToolbarRight.Text = "flowToolbarRight";
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.IconSvg = "ReloadOutlined";
            this.buttonRefresh.Location = new System.Drawing.Point(4, 0);
            this.buttonRefresh.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Radius = 8;
            this.buttonRefresh.Size = new System.Drawing.Size(96, 36);
            this.buttonRefresh.TabIndex = 0;
            this.buttonRefresh.Text = "刷新";
            this.buttonRefresh.WaveSize = 0;
            // 
            // PlcMonitorPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "PlcMonitorPage";
            this.Size = new System.Drawing.Size(850, 680);
            this.panelRoot.ResumeLayout(false);
            this.panelContentCard.ResumeLayout(false);
            this.gridContent.ResumeLayout(false);
            this.panelPointDetailCard.ResumeLayout(false);
            this.panelPointListCard.ResumeLayout(false);
            this.flowStats.ResumeLayout(false);
            this.panelErrorPointCard.ResumeLayout(false);
            this.panelOfflinePointCard.ResumeLayout(false);
            this.panelOnlinePointCard.ResumeLayout(false);
            this.panelTotalPointCard.ResumeLayout(false);
            this.panelToolbar.ResumeLayout(false);
            this.flowToolbarLeft.ResumeLayout(false);
            this.flowToolbarRight.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AntdUI.Panel panelRoot;
        private AntdUI.Panel panelContentCard;
        private AntdUI.GridPanel gridContent;
        private AntdUI.Panel panelPointDetailCard;
        private AntdUI.Panel panelPointListCard;
        private AntdUI.FlowPanel flowStats;
        private AntdUI.Panel panelErrorPointCard;
        private AntdUI.Label labelErrorPointCount;
        private AntdUI.Label labelErrorPointTitle;
        private AntdUI.Panel panelOfflinePointCard;
        private AntdUI.Label labelOfflinePointCount;
        private AntdUI.Label labelOfflinePointTitle;
        private AntdUI.Panel panelOnlinePointCard;
        private AntdUI.Label labelOnlinePointCount;
        private AntdUI.Label labelOnlinePointTitle;
        private AntdUI.Panel panelTotalPointCard;
        private AntdUI.Label labelTotalPointCount;
        private AntdUI.Label labelTotalPointTitle;
        private AntdUI.Panel panelToolbar;
        private AntdUI.FlowPanel flowToolbarLeft;
        private AntdUI.Label labelRuntimeSummary;
        private AntdUI.Select selectPlc;
        private AntdUI.Select selectGroup;
        private AntdUI.Input inputSearch;
        private AntdUI.FlowPanel flowToolbarRight;
        private AntdUI.Button buttonRefresh;
        private PlcPointVirtualListControl plcPointVirtualListControl;
        private PlcPointDetailControl plcPointDetailControl;
    }
}