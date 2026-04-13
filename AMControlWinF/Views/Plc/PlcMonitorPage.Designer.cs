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
            this.panelPointListCard = new AntdUI.Panel();
            this.flowStats = new AntdUI.FlowPanel();
            this.panelDisconnectedPointCard = new AntdUI.Panel();
            this.labelDisconnectedPointCount = new AntdUI.Label();
            this.labelDisconnectedPointTitle = new AntdUI.Label();
            this.panelReadErrorPointCard = new AntdUI.Panel();
            this.labelReadErrorPointCount = new AntdUI.Label();
            this.labelReadErrorPointTitle = new AntdUI.Label();
            this.panelNormalPointCard = new AntdUI.Panel();
            this.labelNormalPointCount = new AntdUI.Label();
            this.labelNormalPointTitle = new AntdUI.Label();
            this.panelMonitoredPointCard = new AntdUI.Panel();
            this.labelMonitoredPointCount = new AntdUI.Label();
            this.labelMonitoredPointTitle = new AntdUI.Label();
            this.panelToolbar = new AntdUI.Panel();
            this.flowToolbarLeft = new AntdUI.FlowPanel();
            this.labelRuntimeSummary = new AntdUI.Label();
            this.inputSearch = new AntdUI.Input();
            this.selectGroup = new AntdUI.Select();
            this.selectPlc = new AntdUI.Select();
            this.flowToolbarRight = new AntdUI.FlowPanel();
            this.buttonRefresh = new AntdUI.Button();
            this.plcPointVirtualListControl = new AMControlWinF.Views.Plc.PlcPointVirtualListControl();
            this.panelRoot.SuspendLayout();
            this.panelContentCard.SuspendLayout();
            this.panelPointListCard.SuspendLayout();
            this.flowStats.SuspendLayout();
            this.panelDisconnectedPointCard.SuspendLayout();
            this.panelReadErrorPointCard.SuspendLayout();
            this.panelNormalPointCard.SuspendLayout();
            this.panelMonitoredPointCard.SuspendLayout();
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
            this.panelContentCard.Controls.Add(this.panelPointListCard);
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
            this.panelPointListCard.Size = new System.Drawing.Size(834, 532);
            this.panelPointListCard.TabIndex = 0;
            // 
            // flowStats
            // 
            this.flowStats.Controls.Add(this.panelDisconnectedPointCard);
            this.flowStats.Controls.Add(this.panelReadErrorPointCard);
            this.flowStats.Controls.Add(this.panelNormalPointCard);
            this.flowStats.Controls.Add(this.panelMonitoredPointCard);
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
            // panelDisconnectedPointCard
            // 
            this.panelDisconnectedPointCard.Controls.Add(this.labelDisconnectedPointCount);
            this.panelDisconnectedPointCard.Controls.Add(this.labelDisconnectedPointTitle);
            this.panelDisconnectedPointCard.Location = new System.Drawing.Point(568, 6);
            this.panelDisconnectedPointCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelDisconnectedPointCard.Name = "panelDisconnectedPointCard";
            this.panelDisconnectedPointCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelDisconnectedPointCard.Radius = 12;
            this.panelDisconnectedPointCard.Shadow = 4;
            this.panelDisconnectedPointCard.ShadowOpacityAnimation = true;
            this.panelDisconnectedPointCard.Size = new System.Drawing.Size(180, 72);
            this.panelDisconnectedPointCard.TabIndex = 3;
            // 
            // labelDisconnectedPointCount
            // 
            this.labelDisconnectedPointCount.BackColor = System.Drawing.Color.Transparent;
            this.labelDisconnectedPointCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelDisconnectedPointCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelDisconnectedPointCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.labelDisconnectedPointCount.Location = new System.Drawing.Point(104, 16);
            this.labelDisconnectedPointCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelDisconnectedPointCount.Name = "labelDisconnectedPointCount";
            this.labelDisconnectedPointCount.Size = new System.Drawing.Size(60, 40);
            this.labelDisconnectedPointCount.TabIndex = 1;
            this.labelDisconnectedPointCount.Text = "0";
            // 
            // labelDisconnectedPointTitle
            // 
            this.labelDisconnectedPointTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelDisconnectedPointTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelDisconnectedPointTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.labelDisconnectedPointTitle.Location = new System.Drawing.Point(16, 16);
            this.labelDisconnectedPointTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelDisconnectedPointTitle.Name = "labelDisconnectedPointTitle";
            this.labelDisconnectedPointTitle.Size = new System.Drawing.Size(72, 40);
            this.labelDisconnectedPointTitle.TabIndex = 0;
            this.labelDisconnectedPointTitle.Text = "通讯断开";
            // 
            // panelReadErrorPointCard
            // 
            this.panelReadErrorPointCard.Controls.Add(this.labelReadErrorPointCount);
            this.panelReadErrorPointCard.Controls.Add(this.labelReadErrorPointTitle);
            this.panelReadErrorPointCard.Location = new System.Drawing.Point(380, 6);
            this.panelReadErrorPointCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelReadErrorPointCard.Name = "panelReadErrorPointCard";
            this.panelReadErrorPointCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelReadErrorPointCard.Radius = 12;
            this.panelReadErrorPointCard.Shadow = 4;
            this.panelReadErrorPointCard.ShadowOpacityAnimation = true;
            this.panelReadErrorPointCard.Size = new System.Drawing.Size(180, 72);
            this.panelReadErrorPointCard.TabIndex = 2;
            // 
            // labelReadErrorPointCount
            // 
            this.labelReadErrorPointCount.BackColor = System.Drawing.Color.Transparent;
            this.labelReadErrorPointCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelReadErrorPointCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelReadErrorPointCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(79)))));
            this.labelReadErrorPointCount.Location = new System.Drawing.Point(104, 16);
            this.labelReadErrorPointCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelReadErrorPointCount.Name = "labelReadErrorPointCount";
            this.labelReadErrorPointCount.Size = new System.Drawing.Size(60, 40);
            this.labelReadErrorPointCount.TabIndex = 1;
            this.labelReadErrorPointCount.Text = "0";
            // 
            // labelReadErrorPointTitle
            // 
            this.labelReadErrorPointTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelReadErrorPointTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelReadErrorPointTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(79)))));
            this.labelReadErrorPointTitle.Location = new System.Drawing.Point(16, 16);
            this.labelReadErrorPointTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelReadErrorPointTitle.Name = "labelReadErrorPointTitle";
            this.labelReadErrorPointTitle.Size = new System.Drawing.Size(72, 40);
            this.labelReadErrorPointTitle.TabIndex = 0;
            this.labelReadErrorPointTitle.Text = "读取错误";
            // 
            // panelNormalPointCard
            // 
            this.panelNormalPointCard.Controls.Add(this.labelNormalPointCount);
            this.panelNormalPointCard.Controls.Add(this.labelNormalPointTitle);
            this.panelNormalPointCard.Location = new System.Drawing.Point(192, 6);
            this.panelNormalPointCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelNormalPointCard.Name = "panelNormalPointCard";
            this.panelNormalPointCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelNormalPointCard.Radius = 12;
            this.panelNormalPointCard.Shadow = 4;
            this.panelNormalPointCard.ShadowOpacityAnimation = true;
            this.panelNormalPointCard.Size = new System.Drawing.Size(180, 72);
            this.panelNormalPointCard.TabIndex = 1;
            // 
            // labelNormalPointCount
            // 
            this.labelNormalPointCount.BackColor = System.Drawing.Color.Transparent;
            this.labelNormalPointCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelNormalPointCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelNormalPointCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(196)))), ((int)(((byte)(26)))));
            this.labelNormalPointCount.Location = new System.Drawing.Point(104, 16);
            this.labelNormalPointCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelNormalPointCount.Name = "labelNormalPointCount";
            this.labelNormalPointCount.Size = new System.Drawing.Size(60, 40);
            this.labelNormalPointCount.TabIndex = 1;
            this.labelNormalPointCount.Text = "0";
            // 
            // labelNormalPointTitle
            // 
            this.labelNormalPointTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelNormalPointTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelNormalPointTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(196)))), ((int)(((byte)(26)))));
            this.labelNormalPointTitle.Location = new System.Drawing.Point(16, 16);
            this.labelNormalPointTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelNormalPointTitle.Name = "labelNormalPointTitle";
            this.labelNormalPointTitle.Size = new System.Drawing.Size(72, 40);
            this.labelNormalPointTitle.TabIndex = 0;
            this.labelNormalPointTitle.Text = "正常";
            // 
            // panelMonitoredPointCard
            // 
            this.panelMonitoredPointCard.Controls.Add(this.labelMonitoredPointCount);
            this.panelMonitoredPointCard.Controls.Add(this.labelMonitoredPointTitle);
            this.panelMonitoredPointCard.Location = new System.Drawing.Point(4, 6);
            this.panelMonitoredPointCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelMonitoredPointCard.Name = "panelMonitoredPointCard";
            this.panelMonitoredPointCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelMonitoredPointCard.Radius = 12;
            this.panelMonitoredPointCard.Shadow = 4;
            this.panelMonitoredPointCard.ShadowOpacityAnimation = true;
            this.panelMonitoredPointCard.Size = new System.Drawing.Size(180, 72);
            this.panelMonitoredPointCard.TabIndex = 0;
            // 
            // labelMonitoredPointCount
            // 
            this.labelMonitoredPointCount.BackColor = System.Drawing.Color.Transparent;
            this.labelMonitoredPointCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelMonitoredPointCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelMonitoredPointCount.Location = new System.Drawing.Point(104, 16);
            this.labelMonitoredPointCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelMonitoredPointCount.Name = "labelMonitoredPointCount";
            this.labelMonitoredPointCount.Size = new System.Drawing.Size(60, 40);
            this.labelMonitoredPointCount.TabIndex = 1;
            this.labelMonitoredPointCount.Text = "0";
            // 
            // labelMonitoredPointTitle
            // 
            this.labelMonitoredPointTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelMonitoredPointTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelMonitoredPointTitle.Location = new System.Drawing.Point(16, 16);
            this.labelMonitoredPointTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelMonitoredPointTitle.Name = "labelMonitoredPointTitle";
            this.labelMonitoredPointTitle.Size = new System.Drawing.Size(72, 40);
            this.labelMonitoredPointTitle.TabIndex = 0;
            this.labelMonitoredPointTitle.Text = "监视点位";
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
            this.flowToolbarLeft.Size = new System.Drawing.Size(628, 36);
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
            // inputSearch
            // 
            this.inputSearch.Location = new System.Drawing.Point(256, 4);
            this.inputSearch.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.inputSearch.Name = "inputSearch";
            this.inputSearch.PlaceholderText = "搜索点位名称 / 分组 / 地址 / 值";
            this.inputSearch.Size = new System.Drawing.Size(150, 32);
            this.inputSearch.TabIndex = 3;
            this.inputSearch.WaveSize = 0;
            // 
            // selectGroup
            // 
            this.selectGroup.Location = new System.Drawing.Point(128, 4);
            this.selectGroup.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.selectGroup.Name = "selectGroup";
            this.selectGroup.Radius = 0;
            this.selectGroup.Size = new System.Drawing.Size(120, 32);
            this.selectGroup.TabIndex = 2;
            this.selectGroup.WaveSize = 0;
            // 
            // selectPlc
            // 
            this.selectPlc.Location = new System.Drawing.Point(0, 4);
            this.selectPlc.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.selectPlc.Name = "selectPlc";
            this.selectPlc.Radius = 0;
            this.selectPlc.Size = new System.Drawing.Size(120, 32);
            this.selectPlc.TabIndex = 1;
            this.selectPlc.WaveSize = 0;
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
            // plcPointVirtualListControl
            // 
            this.plcPointVirtualListControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plcPointVirtualListControl.Location = new System.Drawing.Point(0, 0);
            this.plcPointVirtualListControl.Margin = new System.Windows.Forms.Padding(0);
            this.plcPointVirtualListControl.Name = "plcPointVirtualListControl";
            this.plcPointVirtualListControl.Size = new System.Drawing.Size(834, 532);
            this.plcPointVirtualListControl.TabIndex = 0;
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
            this.panelPointListCard.ResumeLayout(false);
            this.flowStats.ResumeLayout(false);
            this.panelDisconnectedPointCard.ResumeLayout(false);
            this.panelReadErrorPointCard.ResumeLayout(false);
            this.panelNormalPointCard.ResumeLayout(false);
            this.panelMonitoredPointCard.ResumeLayout(false);
            this.panelToolbar.ResumeLayout(false);
            this.flowToolbarLeft.ResumeLayout(false);
            this.flowToolbarRight.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AntdUI.Panel panelRoot;
        private AntdUI.Panel panelContentCard;
        private AntdUI.Panel panelPointListCard;
        private PlcPointVirtualListControl plcPointVirtualListControl;
        private AntdUI.FlowPanel flowStats;
        private AntdUI.Panel panelDisconnectedPointCard;
        private AntdUI.Label labelDisconnectedPointCount;
        private AntdUI.Label labelDisconnectedPointTitle;
        private AntdUI.Panel panelReadErrorPointCard;
        private AntdUI.Label labelReadErrorPointCount;
        private AntdUI.Label labelReadErrorPointTitle;
        private AntdUI.Panel panelNormalPointCard;
        private AntdUI.Label labelNormalPointCount;
        private AntdUI.Label labelNormalPointTitle;
        private AntdUI.Panel panelMonitoredPointCard;
        private AntdUI.Label labelMonitoredPointCount;
        private AntdUI.Label labelMonitoredPointTitle;
        private AntdUI.Panel panelToolbar;
        private AntdUI.FlowPanel flowToolbarLeft;
        private AntdUI.Label labelRuntimeSummary;
        private AntdUI.Select selectPlc;
        private AntdUI.Select selectGroup;
        private AntdUI.Input inputSearch;
        private AntdUI.FlowPanel flowToolbarRight;
        private AntdUI.Button buttonRefresh;
    }
}