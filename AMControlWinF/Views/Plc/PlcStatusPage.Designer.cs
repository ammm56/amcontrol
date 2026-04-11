namespace AMControlWinF.Views.Plc
{
    partial class PlcStatusPage
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
            this.panelStationDetailCard = new AntdUI.Panel();
            this.panelStationListCard = new AntdUI.Panel();
            this.flowStats = new AntdUI.FlowPanel();
            this.panelErrorStationCard = new AntdUI.Panel();
            this.labelErrorStationCount = new AntdUI.Label();
            this.labelErrorStationTitle = new AntdUI.Label();
            this.panelOfflineStationCard = new AntdUI.Panel();
            this.labelOfflineStationCount = new AntdUI.Label();
            this.labelOfflineStationTitle = new AntdUI.Label();
            this.panelOnlineStationCard = new AntdUI.Panel();
            this.labelOnlineStationCount = new AntdUI.Label();
            this.labelOnlineStationTitle = new AntdUI.Label();
            this.panelStationTotalCard = new AntdUI.Panel();
            this.labelStationTotalCount = new AntdUI.Label();
            this.labelStationTotalTitle = new AntdUI.Label();
            this.panelToolbar = new AntdUI.Panel();
            this.flowToolbarLeft = new AntdUI.FlowPanel();
            this.labelRuntimeSummary = new AntdUI.Label();
            this.inputSearch = new AntdUI.Input();
            this.flowToolbarRight = new AntdUI.FlowPanel();
            this.buttonStopScan = new AntdUI.Button();
            this.buttonStartScan = new AntdUI.Button();
            this.buttonScanOnce = new AntdUI.Button();
            this.buttonRefresh = new AntdUI.Button();
            this.plcStatusDetailControl = new AMControlWinF.Views.Plc.PlcStatusDetailControl();
            this.plcStatusVirtualListControl = new AMControlWinF.Views.Plc.PlcStatusVirtualListControl();
            this.panelRoot.SuspendLayout();
            this.panelContentCard.SuspendLayout();
            this.gridContent.SuspendLayout();
            this.panelStationDetailCard.SuspendLayout();
            this.panelStationListCard.SuspendLayout();
            this.flowStats.SuspendLayout();
            this.panelErrorStationCard.SuspendLayout();
            this.panelOfflineStationCard.SuspendLayout();
            this.panelOnlineStationCard.SuspendLayout();
            this.panelStationTotalCard.SuspendLayout();
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
            this.gridContent.Controls.Add(this.panelStationDetailCard);
            this.gridContent.Controls.Add(this.panelStationListCard);
            this.gridContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridContent.Location = new System.Drawing.Point(0, 0);
            this.gridContent.Margin = new System.Windows.Forms.Padding(0);
            this.gridContent.Name = "gridContent";
            this.gridContent.Size = new System.Drawing.Size(834, 532);
            this.gridContent.Span = "100% 250";
            this.gridContent.TabIndex = 0;
            // 
            // panelStationDetailCard
            // 
            this.panelStationDetailCard.BackColor = System.Drawing.Color.Transparent;
            this.panelStationDetailCard.Controls.Add(this.plcStatusDetailControl);
            this.panelStationDetailCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStationDetailCard.Location = new System.Drawing.Point(584, 0);
            this.panelStationDetailCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelStationDetailCard.Name = "panelStationDetailCard";
            this.panelStationDetailCard.Padding = new System.Windows.Forms.Padding(8);
            this.panelStationDetailCard.Radius = 12;
            this.panelStationDetailCard.Shadow = 4;
            this.panelStationDetailCard.ShadowOpacity = 0.15F;
            this.panelStationDetailCard.Size = new System.Drawing.Size(250, 532);
            this.panelStationDetailCard.TabIndex = 1;
            // 
            // panelStationListCard
            // 
            this.panelStationListCard.BackColor = System.Drawing.Color.Transparent;
            this.panelStationListCard.Controls.Add(this.plcStatusVirtualListControl);
            this.panelStationListCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStationListCard.Location = new System.Drawing.Point(0, 0);
            this.panelStationListCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelStationListCard.Name = "panelStationListCard";
            this.panelStationListCard.Radius = 0;
            this.panelStationListCard.ShadowOpacity = 0F;
            this.panelStationListCard.ShadowOpacityHover = 0F;
            this.panelStationListCard.Size = new System.Drawing.Size(584, 532);
            this.panelStationListCard.TabIndex = 0;
            // 
            // flowStats
            // 
            this.flowStats.Controls.Add(this.panelErrorStationCard);
            this.flowStats.Controls.Add(this.panelOfflineStationCard);
            this.flowStats.Controls.Add(this.panelOnlineStationCard);
            this.flowStats.Controls.Add(this.panelStationTotalCard);
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
            // panelErrorStationCard
            // 
            this.panelErrorStationCard.Controls.Add(this.labelErrorStationCount);
            this.panelErrorStationCard.Controls.Add(this.labelErrorStationTitle);
            this.panelErrorStationCard.Location = new System.Drawing.Point(568, 6);
            this.panelErrorStationCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelErrorStationCard.Name = "panelErrorStationCard";
            this.panelErrorStationCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelErrorStationCard.Radius = 12;
            this.panelErrorStationCard.Shadow = 4;
            this.panelErrorStationCard.ShadowOpacityAnimation = true;
            this.panelErrorStationCard.Size = new System.Drawing.Size(180, 72);
            this.panelErrorStationCard.TabIndex = 3;
            // 
            // labelErrorStationCount
            // 
            this.labelErrorStationCount.BackColor = System.Drawing.Color.Transparent;
            this.labelErrorStationCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelErrorStationCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelErrorStationCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(79)))));
            this.labelErrorStationCount.Location = new System.Drawing.Point(104, 16);
            this.labelErrorStationCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelErrorStationCount.Name = "labelErrorStationCount";
            this.labelErrorStationCount.Size = new System.Drawing.Size(60, 40);
            this.labelErrorStationCount.TabIndex = 1;
            this.labelErrorStationCount.Text = "0";
            // 
            // labelErrorStationTitle
            // 
            this.labelErrorStationTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelErrorStationTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelErrorStationTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(79)))));
            this.labelErrorStationTitle.Location = new System.Drawing.Point(16, 16);
            this.labelErrorStationTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelErrorStationTitle.Name = "labelErrorStationTitle";
            this.labelErrorStationTitle.Size = new System.Drawing.Size(72, 40);
            this.labelErrorStationTitle.TabIndex = 0;
            this.labelErrorStationTitle.Text = "异常";
            // 
            // panelOfflineStationCard
            // 
            this.panelOfflineStationCard.Controls.Add(this.labelOfflineStationCount);
            this.panelOfflineStationCard.Controls.Add(this.labelOfflineStationTitle);
            this.panelOfflineStationCard.Location = new System.Drawing.Point(380, 6);
            this.panelOfflineStationCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelOfflineStationCard.Name = "panelOfflineStationCard";
            this.panelOfflineStationCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelOfflineStationCard.Radius = 12;
            this.panelOfflineStationCard.Shadow = 4;
            this.panelOfflineStationCard.ShadowOpacityAnimation = true;
            this.panelOfflineStationCard.Size = new System.Drawing.Size(180, 72);
            this.panelOfflineStationCard.TabIndex = 2;
            // 
            // labelOfflineStationCount
            // 
            this.labelOfflineStationCount.BackColor = System.Drawing.Color.Transparent;
            this.labelOfflineStationCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelOfflineStationCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelOfflineStationCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.labelOfflineStationCount.Location = new System.Drawing.Point(104, 16);
            this.labelOfflineStationCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelOfflineStationCount.Name = "labelOfflineStationCount";
            this.labelOfflineStationCount.Size = new System.Drawing.Size(60, 40);
            this.labelOfflineStationCount.TabIndex = 1;
            this.labelOfflineStationCount.Text = "0";
            // 
            // labelOfflineStationTitle
            // 
            this.labelOfflineStationTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelOfflineStationTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelOfflineStationTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.labelOfflineStationTitle.Location = new System.Drawing.Point(16, 16);
            this.labelOfflineStationTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelOfflineStationTitle.Name = "labelOfflineStationTitle";
            this.labelOfflineStationTitle.Size = new System.Drawing.Size(72, 40);
            this.labelOfflineStationTitle.TabIndex = 0;
            this.labelOfflineStationTitle.Text = "离线";
            // 
            // panelOnlineStationCard
            // 
            this.panelOnlineStationCard.Controls.Add(this.labelOnlineStationCount);
            this.panelOnlineStationCard.Controls.Add(this.labelOnlineStationTitle);
            this.panelOnlineStationCard.Location = new System.Drawing.Point(192, 6);
            this.panelOnlineStationCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelOnlineStationCard.Name = "panelOnlineStationCard";
            this.panelOnlineStationCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelOnlineStationCard.Radius = 12;
            this.panelOnlineStationCard.Shadow = 4;
            this.panelOnlineStationCard.ShadowOpacityAnimation = true;
            this.panelOnlineStationCard.Size = new System.Drawing.Size(180, 72);
            this.panelOnlineStationCard.TabIndex = 1;
            // 
            // labelOnlineStationCount
            // 
            this.labelOnlineStationCount.BackColor = System.Drawing.Color.Transparent;
            this.labelOnlineStationCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelOnlineStationCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelOnlineStationCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(196)))), ((int)(((byte)(26)))));
            this.labelOnlineStationCount.Location = new System.Drawing.Point(104, 16);
            this.labelOnlineStationCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelOnlineStationCount.Name = "labelOnlineStationCount";
            this.labelOnlineStationCount.Size = new System.Drawing.Size(60, 40);
            this.labelOnlineStationCount.TabIndex = 1;
            this.labelOnlineStationCount.Text = "0";
            // 
            // labelOnlineStationTitle
            // 
            this.labelOnlineStationTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelOnlineStationTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelOnlineStationTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(196)))), ((int)(((byte)(26)))));
            this.labelOnlineStationTitle.Location = new System.Drawing.Point(16, 16);
            this.labelOnlineStationTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelOnlineStationTitle.Name = "labelOnlineStationTitle";
            this.labelOnlineStationTitle.Size = new System.Drawing.Size(72, 40);
            this.labelOnlineStationTitle.TabIndex = 0;
            this.labelOnlineStationTitle.Text = "在线";
            // 
            // panelStationTotalCard
            // 
            this.panelStationTotalCard.Controls.Add(this.labelStationTotalCount);
            this.panelStationTotalCard.Controls.Add(this.labelStationTotalTitle);
            this.panelStationTotalCard.Location = new System.Drawing.Point(4, 6);
            this.panelStationTotalCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelStationTotalCard.Name = "panelStationTotalCard";
            this.panelStationTotalCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelStationTotalCard.Radius = 12;
            this.panelStationTotalCard.Shadow = 4;
            this.panelStationTotalCard.ShadowOpacityAnimation = true;
            this.panelStationTotalCard.Size = new System.Drawing.Size(180, 72);
            this.panelStationTotalCard.TabIndex = 0;
            // 
            // labelStationTotalCount
            // 
            this.labelStationTotalCount.BackColor = System.Drawing.Color.Transparent;
            this.labelStationTotalCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelStationTotalCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelStationTotalCount.Location = new System.Drawing.Point(104, 16);
            this.labelStationTotalCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelStationTotalCount.Name = "labelStationTotalCount";
            this.labelStationTotalCount.Size = new System.Drawing.Size(60, 40);
            this.labelStationTotalCount.TabIndex = 1;
            this.labelStationTotalCount.Text = "0";
            // 
            // labelStationTotalTitle
            // 
            this.labelStationTotalTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelStationTotalTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelStationTotalTitle.Location = new System.Drawing.Point(16, 16);
            this.labelStationTotalTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelStationTotalTitle.Name = "labelStationTotalTitle";
            this.labelStationTotalTitle.Size = new System.Drawing.Size(72, 40);
            this.labelStationTotalTitle.TabIndex = 0;
            this.labelStationTotalTitle.Text = "总站数";
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
            this.flowToolbarLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowToolbarLeft.Gap = 8;
            this.flowToolbarLeft.Location = new System.Drawing.Point(4, 4);
            this.flowToolbarLeft.Margin = new System.Windows.Forms.Padding(0);
            this.flowToolbarLeft.Name = "flowToolbarLeft";
            this.flowToolbarLeft.Size = new System.Drawing.Size(380, 36);
            this.flowToolbarLeft.TabIndex = 0;
            // 
            // labelRuntimeSummary
            // 
            this.labelRuntimeSummary.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelRuntimeSummary.Location = new System.Drawing.Point(190, 0);
            this.labelRuntimeSummary.Margin = new System.Windows.Forms.Padding(0);
            this.labelRuntimeSummary.Name = "labelRuntimeSummary";
            this.labelRuntimeSummary.Size = new System.Drawing.Size(189, 36);
            this.labelRuntimeSummary.TabIndex = 0;
            this.labelRuntimeSummary.Text = "状态未知";
            // 
            // inputSearch
            // 
            this.inputSearch.Location = new System.Drawing.Point(0, 0);
            this.inputSearch.Margin = new System.Windows.Forms.Padding(0);
            this.inputSearch.Name = "inputSearch";
            this.inputSearch.PlaceholderText = "搜索站名 / 显示名 / 协议 / 连接 / 端点 / 错误";
            this.inputSearch.Size = new System.Drawing.Size(182, 36);
            this.inputSearch.TabIndex = 1;
            this.inputSearch.WaveSize = 0;
            // 
            // flowToolbarRight
            // 
            this.flowToolbarRight.Align = AntdUI.TAlignFlow.RightCenter;
            this.flowToolbarRight.Controls.Add(this.buttonStopScan);
            this.flowToolbarRight.Controls.Add(this.buttonStartScan);
            this.flowToolbarRight.Controls.Add(this.buttonScanOnce);
            this.flowToolbarRight.Controls.Add(this.buttonRefresh);
            this.flowToolbarRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowToolbarRight.Gap = 8;
            this.flowToolbarRight.Location = new System.Drawing.Point(396, 4);
            this.flowToolbarRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowToolbarRight.Name = "flowToolbarRight";
            this.flowToolbarRight.Size = new System.Drawing.Size(434, 36);
            this.flowToolbarRight.TabIndex = 1;
            // 
            // buttonStopScan
            // 
            this.buttonStopScan.Location = new System.Drawing.Point(338, 0);
            this.buttonStopScan.Margin = new System.Windows.Forms.Padding(0);
            this.buttonStopScan.Name = "buttonStopScan";
            this.buttonStopScan.Radius = 8;
            this.buttonStopScan.Size = new System.Drawing.Size(96, 36);
            this.buttonStopScan.TabIndex = 3;
            this.buttonStopScan.Text = "停止扫描";
            this.buttonStopScan.Type = AntdUI.TTypeMini.Error;
            this.buttonStopScan.WaveSize = 0;
            // 
            // buttonStartScan
            // 
            this.buttonStartScan.Location = new System.Drawing.Point(234, 0);
            this.buttonStartScan.Margin = new System.Windows.Forms.Padding(0);
            this.buttonStartScan.Name = "buttonStartScan";
            this.buttonStartScan.Radius = 8;
            this.buttonStartScan.Size = new System.Drawing.Size(96, 36);
            this.buttonStartScan.TabIndex = 2;
            this.buttonStartScan.Text = "启动扫描";
            this.buttonStartScan.Type = AntdUI.TTypeMini.Primary;
            this.buttonStartScan.WaveSize = 0;
            // 
            // buttonScanOnce
            // 
            this.buttonScanOnce.Location = new System.Drawing.Point(124, 0);
            this.buttonScanOnce.Margin = new System.Windows.Forms.Padding(0);
            this.buttonScanOnce.Name = "buttonScanOnce";
            this.buttonScanOnce.Radius = 8;
            this.buttonScanOnce.Size = new System.Drawing.Size(102, 36);
            this.buttonScanOnce.TabIndex = 1;
            this.buttonScanOnce.Text = "单轮扫描";
            this.buttonScanOnce.WaveSize = 0;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(16, 0);
            this.buttonRefresh.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Radius = 8;
            this.buttonRefresh.Size = new System.Drawing.Size(100, 36);
            this.buttonRefresh.TabIndex = 0;
            this.buttonRefresh.Text = "刷新";
            this.buttonRefresh.WaveSize = 0;
            // 
            // plcStatusDetailControl
            // 
            this.plcStatusDetailControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plcStatusDetailControl.Location = new System.Drawing.Point(12, 12);
            this.plcStatusDetailControl.Margin = new System.Windows.Forms.Padding(0);
            this.plcStatusDetailControl.Name = "plcStatusDetailControl";
            this.plcStatusDetailControl.Size = new System.Drawing.Size(226, 508);
            this.plcStatusDetailControl.TabIndex = 1;
            // 
            // plcStatusVirtualListControl
            // 
            this.plcStatusVirtualListControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plcStatusVirtualListControl.Location = new System.Drawing.Point(0, 0);
            this.plcStatusVirtualListControl.Margin = new System.Windows.Forms.Padding(0);
            this.plcStatusVirtualListControl.Name = "plcStatusVirtualListControl";
            this.plcStatusVirtualListControl.Size = new System.Drawing.Size(584, 532);
            this.plcStatusVirtualListControl.TabIndex = 1;
            // 
            // PlcStatusPage
            // 
            this.Controls.Add(this.panelRoot);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.Name = "PlcStatusPage";
            this.Size = new System.Drawing.Size(850, 680);
            this.panelRoot.ResumeLayout(false);
            this.panelContentCard.ResumeLayout(false);
            this.gridContent.ResumeLayout(false);
            this.panelStationDetailCard.ResumeLayout(false);
            this.panelStationListCard.ResumeLayout(false);
            this.flowStats.ResumeLayout(false);
            this.panelErrorStationCard.ResumeLayout(false);
            this.panelOfflineStationCard.ResumeLayout(false);
            this.panelOnlineStationCard.ResumeLayout(false);
            this.panelStationTotalCard.ResumeLayout(false);
            this.panelToolbar.ResumeLayout(false);
            this.flowToolbarLeft.ResumeLayout(false);
            this.flowToolbarRight.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AntdUI.Panel panelRoot;
        private AntdUI.Panel panelToolbar;
        private AntdUI.FlowPanel flowToolbarLeft;
        private AntdUI.Label labelRuntimeSummary;
        private AntdUI.Input inputSearch;
        private AntdUI.FlowPanel flowToolbarRight;
        private AntdUI.Button buttonRefresh;
        private AntdUI.Button buttonScanOnce;
        private AntdUI.Button buttonStartScan;
        private AntdUI.Button buttonStopScan;
        private AntdUI.FlowPanel flowStats;
        private AntdUI.Panel panelStationTotalCard;
        private AntdUI.Label labelStationTotalCount;
        private AntdUI.Label labelStationTotalTitle;
        private AntdUI.Panel panelOnlineStationCard;
        private AntdUI.Label labelOnlineStationCount;
        private AntdUI.Label labelOnlineStationTitle;
        private AntdUI.Panel panelOfflineStationCard;
        private AntdUI.Label labelOfflineStationCount;
        private AntdUI.Label labelOfflineStationTitle;
        private AntdUI.Panel panelErrorStationCard;
        private AntdUI.Label labelErrorStationCount;
        private AntdUI.Label labelErrorStationTitle;
        private AntdUI.Panel panelContentCard;
        private AntdUI.GridPanel gridContent;
        private AntdUI.Panel panelStationListCard;
        private AMControlWinF.Views.Plc.PlcStatusVirtualListControl plcStatusVirtualListControl;
        private AntdUI.Panel panelStationDetailCard;
        private AMControlWinF.Views.Plc.PlcStatusDetailControl plcStatusDetailControl;
    }
}