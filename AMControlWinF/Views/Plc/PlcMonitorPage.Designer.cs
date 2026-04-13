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
            this.plcPointVirtualListControl = new AMControlWinF.Views.Plc.PlcPointVirtualListControl();
            this.flowStats = new AntdUI.FlowPanel();
            this.panelErrorPointCard = new AntdUI.Panel();
            this.labelErrorPointCount = new AntdUI.Label();
            this.labelErrorPointTitle = new AntdUI.Label();
            this.panelUnreadablePointCard = new AntdUI.Panel();
            this.labelUnreadablePointCount = new AntdUI.Label();
            this.labelUnreadablePointTitle = new AntdUI.Label();
            this.panelReadablePointCard = new AntdUI.Panel();
            this.labelReadablePointCount = new AntdUI.Label();
            this.labelReadablePointTitle = new AntdUI.Label();
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
            this.panelPointListCard.SuspendLayout();
            this.flowStats.SuspendLayout();
            this.panelErrorPointCard.SuspendLayout();
            this.panelUnreadablePointCard.SuspendLayout();
            this.panelReadablePointCard.SuspendLayout();
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
            // plcPointVirtualListControl
            // 
            this.plcPointVirtualListControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plcPointVirtualListControl.Location = new System.Drawing.Point(0, 0);
            this.plcPointVirtualListControl.Margin = new System.Windows.Forms.Padding(0);
            this.plcPointVirtualListControl.Name = "plcPointVirtualListControl";
            this.plcPointVirtualListControl.Size = new System.Drawing.Size(834, 532);
            this.plcPointVirtualListControl.TabIndex = 0;
            // 
            // flowStats
            // 
            this.flowStats.Controls.Add(this.panelErrorPointCard);
            this.flowStats.Controls.Add(this.panelUnreadablePointCard);
            this.flowStats.Controls.Add(this.panelReadablePointCard);
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
            // panelUnreadablePointCard
            // 
            this.panelUnreadablePointCard.Controls.Add(this.labelUnreadablePointCount);
            this.panelUnreadablePointCard.Controls.Add(this.labelUnreadablePointTitle);
            this.panelUnreadablePointCard.Location = new System.Drawing.Point(380, 6);
            this.panelUnreadablePointCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelUnreadablePointCard.Name = "panelUnreadablePointCard";
            this.panelUnreadablePointCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelUnreadablePointCard.Radius = 12;
            this.panelUnreadablePointCard.Shadow = 4;
            this.panelUnreadablePointCard.ShadowOpacityAnimation = true;
            this.panelUnreadablePointCard.Size = new System.Drawing.Size(180, 72);
            this.panelUnreadablePointCard.TabIndex = 2;
            // 
            // labelUnreadablePointCount
            // 
            this.labelUnreadablePointCount.BackColor = System.Drawing.Color.Transparent;
            this.labelUnreadablePointCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelUnreadablePointCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelUnreadablePointCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.labelUnreadablePointCount.Location = new System.Drawing.Point(104, 16);
            this.labelUnreadablePointCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelUnreadablePointCount.Name = "labelUnreadablePointCount";
            this.labelUnreadablePointCount.Size = new System.Drawing.Size(60, 40);
            this.labelUnreadablePointCount.TabIndex = 1;
            this.labelUnreadablePointCount.Text = "0";
            // 
            // labelUnreadablePointTitle
            // 
            this.labelUnreadablePointTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelUnreadablePointTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelUnreadablePointTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.labelUnreadablePointTitle.Location = new System.Drawing.Point(16, 16);
            this.labelUnreadablePointTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelUnreadablePointTitle.Name = "labelUnreadablePointTitle";
            this.labelUnreadablePointTitle.Size = new System.Drawing.Size(72, 40);
            this.labelUnreadablePointTitle.TabIndex = 0;
            this.labelUnreadablePointTitle.Text = "不可读";
            // 
            // panelReadablePointCard
            // 
            this.panelReadablePointCard.Controls.Add(this.labelReadablePointCount);
            this.panelReadablePointCard.Controls.Add(this.labelReadablePointTitle);
            this.panelReadablePointCard.Location = new System.Drawing.Point(192, 6);
            this.panelReadablePointCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelReadablePointCard.Name = "panelReadablePointCard";
            this.panelReadablePointCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelReadablePointCard.Radius = 12;
            this.panelReadablePointCard.Shadow = 4;
            this.panelReadablePointCard.ShadowOpacityAnimation = true;
            this.panelReadablePointCard.Size = new System.Drawing.Size(180, 72);
            this.panelReadablePointCard.TabIndex = 1;
            // 
            // labelReadablePointCount
            // 
            this.labelReadablePointCount.BackColor = System.Drawing.Color.Transparent;
            this.labelReadablePointCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelReadablePointCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelReadablePointCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(196)))), ((int)(((byte)(26)))));
            this.labelReadablePointCount.Location = new System.Drawing.Point(104, 16);
            this.labelReadablePointCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelReadablePointCount.Name = "labelReadablePointCount";
            this.labelReadablePointCount.Size = new System.Drawing.Size(60, 40);
            this.labelReadablePointCount.TabIndex = 1;
            this.labelReadablePointCount.Text = "0";
            // 
            // labelReadablePointTitle
            // 
            this.labelReadablePointTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelReadablePointTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelReadablePointTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(196)))), ((int)(((byte)(26)))));
            this.labelReadablePointTitle.Location = new System.Drawing.Point(16, 16);
            this.labelReadablePointTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelReadablePointTitle.Name = "labelReadablePointTitle";
            this.labelReadablePointTitle.Size = new System.Drawing.Size(72, 40);
            this.labelReadablePointTitle.TabIndex = 0;
            this.labelReadablePointTitle.Text = "可读";
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
            this.flowToolbarLeft.Size = new System.Drawing.Size(602, 36);
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
            this.panelPointListCard.ResumeLayout(false);
            this.flowStats.ResumeLayout(false);
            this.panelErrorPointCard.ResumeLayout(false);
            this.panelUnreadablePointCard.ResumeLayout(false);
            this.panelReadablePointCard.ResumeLayout(false);
            this.panelTotalPointCard.ResumeLayout(false);
            this.panelToolbar.ResumeLayout(false);
            this.flowToolbarLeft.ResumeLayout(false);
            this.flowToolbarRight.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AntdUI.Panel panelRoot;
        private AntdUI.Panel panelContentCard;
        private AntdUI.Panel panelPointListCard;
        private AntdUI.FlowPanel flowStats;
        private AntdUI.Panel panelErrorPointCard;
        private AntdUI.Label labelErrorPointCount;
        private AntdUI.Label labelErrorPointTitle;
        private AntdUI.Panel panelUnreadablePointCard;
        private AntdUI.Label labelUnreadablePointCount;
        private AntdUI.Label labelUnreadablePointTitle;
        private AntdUI.Panel panelReadablePointCard;
        private AntdUI.Label labelReadablePointCount;
        private AntdUI.Label labelReadablePointTitle;
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
    }
}