namespace AMControlWinF.Views.SysConfig
{
    partial class PlcConfigManagementPage
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
            this.panelPointCard = new AntdUI.Panel();
            this.tablePoints = new AntdUI.Table();
            this.panelPointHeader = new AntdUI.Panel();
            this.flowPointActionsRight = new AntdUI.FlowPanel();
            this.buttonAddPoint = new AntdUI.Button();
            this.buttonEditPoint = new AntdUI.Button();
            this.buttonDeletePoint = new AntdUI.Button();
            this.flowPointActionsLeft = new AntdUI.FlowPanel();
            this.labelSelectedPoint = new AntdUI.Label();
            this.labelPointTitle = new AntdUI.Label();
            this.panelStationCard = new AntdUI.Panel();
            this.tableStations = new AntdUI.Table();
            this.panelStationHeader = new AntdUI.Panel();
            this.flowStationActionsRight = new AntdUI.FlowPanel();
            this.buttonAddStation = new AntdUI.Button();
            this.buttonEditStation = new AntdUI.Button();
            this.buttonDeleteStation = new AntdUI.Button();
            this.flowStationActionsLeft = new AntdUI.FlowPanel();
            this.labelSelectedStation = new AntdUI.Label();
            this.labelStationTitle = new AntdUI.Label();
            this.flowStats = new AntdUI.FlowPanel();
            this.panelCurrentStationPointCard = new AntdUI.Panel();
            this.labelCurrentStationPointCount = new AntdUI.Label();
            this.labelCurrentStationPointTitle = new AntdUI.Label();
            this.panelPointTotalCard = new AntdUI.Panel();
            this.labelPointTotalCount = new AntdUI.Label();
            this.labelPointTotalTitle = new AntdUI.Label();
            this.panelOnlineStationCard = new AntdUI.Panel();
            this.labelOnlineStationCount = new AntdUI.Label();
            this.labelOnlineStationTitle = new AntdUI.Label();
            this.panelStationTotalCard = new AntdUI.Panel();
            this.labelStationTotalCount = new AntdUI.Label();
            this.labelStationTotalTitle = new AntdUI.Label();
            this.panelToolbar = new AntdUI.Panel();
            this.flowToolbarLeft = new AntdUI.FlowPanel();
            this.labelRuntimeSummary = new AntdUI.Label();
            this.inputPointSearch = new AntdUI.Input();
            this.inputStationSearch = new AntdUI.Input();
            this.flowToolbarRight = new AntdUI.FlowPanel();
            this.buttonStopScan = new AntdUI.Button();
            this.buttonStartScan = new AntdUI.Button();
            this.buttonScanOnce = new AntdUI.Button();
            this.buttonReloadConfig = new AntdUI.Button();
            this.buttonRefresh = new AntdUI.Button();
            this.panelRoot.SuspendLayout();
            this.panelContentCard.SuspendLayout();
            this.gridContent.SuspendLayout();
            this.panelPointCard.SuspendLayout();
            this.panelPointHeader.SuspendLayout();
            this.flowPointActionsRight.SuspendLayout();
            this.flowPointActionsLeft.SuspendLayout();
            this.panelStationCard.SuspendLayout();
            this.panelStationHeader.SuspendLayout();
            this.flowStationActionsRight.SuspendLayout();
            this.flowStationActionsLeft.SuspendLayout();
            this.flowStats.SuspendLayout();
            this.panelCurrentStationPointCard.SuspendLayout();
            this.panelPointTotalCard.SuspendLayout();
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
            this.panelRoot.Size = new System.Drawing.Size(1150, 680);
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
            this.panelContentCard.Size = new System.Drawing.Size(1134, 532);
            this.panelContentCard.TabIndex = 2;
            // 
            // gridContent
            // 
            this.gridContent.Controls.Add(this.panelPointCard);
            this.gridContent.Controls.Add(this.panelStationCard);
            this.gridContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridContent.Location = new System.Drawing.Point(0, 0);
            this.gridContent.Margin = new System.Windows.Forms.Padding(0);
            this.gridContent.Name = "gridContent";
            this.gridContent.Size = new System.Drawing.Size(1134, 532);
            this.gridContent.Span = "40% 60%";
            this.gridContent.TabIndex = 0;
            // 
            // panelPointCard
            // 
            this.panelPointCard.BackColor = System.Drawing.Color.Transparent;
            this.panelPointCard.Controls.Add(this.tablePoints);
            this.panelPointCard.Controls.Add(this.panelPointHeader);
            this.panelPointCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPointCard.Location = new System.Drawing.Point(454, 0);
            this.panelPointCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelPointCard.Name = "panelPointCard";
            this.panelPointCard.Padding = new System.Windows.Forms.Padding(8);
            this.panelPointCard.Radius = 12;
            this.panelPointCard.Shadow = 4;
            this.panelPointCard.ShadowOpacity = 0.15F;
            this.panelPointCard.Size = new System.Drawing.Size(680, 532);
            this.panelPointCard.TabIndex = 1;
            // 
            // tablePoints
            // 
            this.tablePoints.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tablePoints.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePoints.EmptyHeader = true;
            this.tablePoints.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.tablePoints.Gap = 8;
            this.tablePoints.Gaps = new System.Drawing.Size(8, 8);
            this.tablePoints.Location = new System.Drawing.Point(12, 56);
            this.tablePoints.Margin = new System.Windows.Forms.Padding(0);
            this.tablePoints.Name = "tablePoints";
            this.tablePoints.ShowTip = false;
            this.tablePoints.Size = new System.Drawing.Size(656, 464);
            this.tablePoints.TabIndex = 1;
            this.tablePoints.Text = "tablePoints";
            // 
            // panelPointHeader
            // 
            this.panelPointHeader.Controls.Add(this.flowPointActionsRight);
            this.panelPointHeader.Controls.Add(this.flowPointActionsLeft);
            this.panelPointHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPointHeader.Location = new System.Drawing.Point(12, 12);
            this.panelPointHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelPointHeader.Name = "panelPointHeader";
            this.panelPointHeader.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.panelPointHeader.Radius = 0;
            this.panelPointHeader.Size = new System.Drawing.Size(656, 44);
            this.panelPointHeader.TabIndex = 0;
            // 
            // flowPointActionsRight
            // 
            this.flowPointActionsRight.Align = AntdUI.TAlignFlow.RightCenter;
            this.flowPointActionsRight.Controls.Add(this.buttonAddPoint);
            this.flowPointActionsRight.Controls.Add(this.buttonEditPoint);
            this.flowPointActionsRight.Controls.Add(this.buttonDeletePoint);
            this.flowPointActionsRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowPointActionsRight.Gap = 8;
            this.flowPointActionsRight.Location = new System.Drawing.Point(336, 0);
            this.flowPointActionsRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowPointActionsRight.Name = "flowPointActionsRight";
            this.flowPointActionsRight.Size = new System.Drawing.Size(320, 36);
            this.flowPointActionsRight.TabIndex = 1;
            // 
            // buttonAddPoint
            // 
            this.buttonAddPoint.IconSvg = "PlusOutlined";
            this.buttonAddPoint.Location = new System.Drawing.Point(239, 0);
            this.buttonAddPoint.Margin = new System.Windows.Forms.Padding(0);
            this.buttonAddPoint.Name = "buttonAddPoint";
            this.buttonAddPoint.Radius = 8;
            this.buttonAddPoint.Size = new System.Drawing.Size(81, 36);
            this.buttonAddPoint.TabIndex = 0;
            this.buttonAddPoint.Text = "新增";
            this.buttonAddPoint.Type = AntdUI.TTypeMini.Primary;
            this.buttonAddPoint.WaveSize = 0;
            // 
            // buttonEditPoint
            // 
            this.buttonEditPoint.IconSvg = "EditOutlined";
            this.buttonEditPoint.Location = new System.Drawing.Point(151, 0);
            this.buttonEditPoint.Margin = new System.Windows.Forms.Padding(0);
            this.buttonEditPoint.Name = "buttonEditPoint";
            this.buttonEditPoint.Radius = 8;
            this.buttonEditPoint.Size = new System.Drawing.Size(80, 36);
            this.buttonEditPoint.TabIndex = 1;
            this.buttonEditPoint.Text = "编辑";
            this.buttonEditPoint.WaveSize = 0;
            // 
            // buttonDeletePoint
            // 
            this.buttonDeletePoint.IconSvg = "DeleteOutlined";
            this.buttonDeletePoint.Location = new System.Drawing.Point(62, 0);
            this.buttonDeletePoint.Margin = new System.Windows.Forms.Padding(0);
            this.buttonDeletePoint.Name = "buttonDeletePoint";
            this.buttonDeletePoint.Radius = 8;
            this.buttonDeletePoint.Size = new System.Drawing.Size(81, 36);
            this.buttonDeletePoint.TabIndex = 2;
            this.buttonDeletePoint.Text = "删除";
            this.buttonDeletePoint.WaveSize = 0;
            // 
            // flowPointActionsLeft
            // 
            this.flowPointActionsLeft.Controls.Add(this.labelSelectedPoint);
            this.flowPointActionsLeft.Controls.Add(this.labelPointTitle);
            this.flowPointActionsLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowPointActionsLeft.Gap = 8;
            this.flowPointActionsLeft.Location = new System.Drawing.Point(0, 0);
            this.flowPointActionsLeft.Margin = new System.Windows.Forms.Padding(0);
            this.flowPointActionsLeft.Name = "flowPointActionsLeft";
            this.flowPointActionsLeft.Size = new System.Drawing.Size(252, 36);
            this.flowPointActionsLeft.TabIndex = 0;
            // 
            // labelSelectedPoint
            // 
            this.labelSelectedPoint.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelSelectedPoint.Location = new System.Drawing.Point(72, 0);
            this.labelSelectedPoint.Margin = new System.Windows.Forms.Padding(0);
            this.labelSelectedPoint.Name = "labelSelectedPoint";
            this.labelSelectedPoint.Size = new System.Drawing.Size(180, 36);
            this.labelSelectedPoint.TabIndex = 1;
            this.labelSelectedPoint.Text = "未选择";
            // 
            // labelPointTitle
            // 
            this.labelPointTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.labelPointTitle.Location = new System.Drawing.Point(0, 0);
            this.labelPointTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelPointTitle.Name = "labelPointTitle";
            this.labelPointTitle.Size = new System.Drawing.Size(64, 36);
            this.labelPointTitle.TabIndex = 0;
            this.labelPointTitle.Text = "点位配置";
            // 
            // panelStationCard
            // 
            this.panelStationCard.BackColor = System.Drawing.Color.Transparent;
            this.panelStationCard.Controls.Add(this.tableStations);
            this.panelStationCard.Controls.Add(this.panelStationHeader);
            this.panelStationCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStationCard.Location = new System.Drawing.Point(0, 0);
            this.panelStationCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelStationCard.Name = "panelStationCard";
            this.panelStationCard.Padding = new System.Windows.Forms.Padding(8);
            this.panelStationCard.Radius = 12;
            this.panelStationCard.Shadow = 4;
            this.panelStationCard.ShadowOpacity = 0.15F;
            this.panelStationCard.Size = new System.Drawing.Size(454, 532);
            this.panelStationCard.TabIndex = 0;
            // 
            // tableStations
            // 
            this.tableStations.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tableStations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableStations.EmptyHeader = true;
            this.tableStations.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.tableStations.Gap = 8;
            this.tableStations.Gaps = new System.Drawing.Size(8, 8);
            this.tableStations.Location = new System.Drawing.Point(12, 56);
            this.tableStations.Margin = new System.Windows.Forms.Padding(0);
            this.tableStations.Name = "tableStations";
            this.tableStations.ShowTip = false;
            this.tableStations.Size = new System.Drawing.Size(430, 464);
            this.tableStations.TabIndex = 1;
            this.tableStations.Text = "tableStations";
            // 
            // panelStationHeader
            // 
            this.panelStationHeader.Controls.Add(this.flowStationActionsRight);
            this.panelStationHeader.Controls.Add(this.flowStationActionsLeft);
            this.panelStationHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelStationHeader.Location = new System.Drawing.Point(12, 12);
            this.panelStationHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelStationHeader.Name = "panelStationHeader";
            this.panelStationHeader.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.panelStationHeader.Radius = 0;
            this.panelStationHeader.Size = new System.Drawing.Size(430, 44);
            this.panelStationHeader.TabIndex = 0;
            // 
            // flowStationActionsRight
            // 
            this.flowStationActionsRight.Align = AntdUI.TAlignFlow.RightCenter;
            this.flowStationActionsRight.Controls.Add(this.buttonAddStation);
            this.flowStationActionsRight.Controls.Add(this.buttonEditStation);
            this.flowStationActionsRight.Controls.Add(this.buttonDeleteStation);
            this.flowStationActionsRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowStationActionsRight.Gap = 8;
            this.flowStationActionsRight.Location = new System.Drawing.Point(107, 0);
            this.flowStationActionsRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowStationActionsRight.Name = "flowStationActionsRight";
            this.flowStationActionsRight.Size = new System.Drawing.Size(323, 36);
            this.flowStationActionsRight.TabIndex = 1;
            // 
            // buttonAddStation
            // 
            this.buttonAddStation.IconSvg = "PlusOutlined";
            this.buttonAddStation.Location = new System.Drawing.Point(243, 0);
            this.buttonAddStation.Margin = new System.Windows.Forms.Padding(0);
            this.buttonAddStation.Name = "buttonAddStation";
            this.buttonAddStation.Radius = 8;
            this.buttonAddStation.Size = new System.Drawing.Size(80, 36);
            this.buttonAddStation.TabIndex = 0;
            this.buttonAddStation.Text = "新增";
            this.buttonAddStation.Type = AntdUI.TTypeMini.Primary;
            this.buttonAddStation.WaveSize = 0;
            // 
            // buttonEditStation
            // 
            this.buttonEditStation.IconSvg = "EditOutlined";
            this.buttonEditStation.Location = new System.Drawing.Point(149, 0);
            this.buttonEditStation.Margin = new System.Windows.Forms.Padding(0);
            this.buttonEditStation.Name = "buttonEditStation";
            this.buttonEditStation.Radius = 8;
            this.buttonEditStation.Size = new System.Drawing.Size(86, 36);
            this.buttonEditStation.TabIndex = 1;
            this.buttonEditStation.Text = "编辑";
            this.buttonEditStation.WaveSize = 0;
            // 
            // buttonDeleteStation
            // 
            this.buttonDeleteStation.IconSvg = "DeleteOutlined";
            this.buttonDeleteStation.Location = new System.Drawing.Point(59, 0);
            this.buttonDeleteStation.Margin = new System.Windows.Forms.Padding(0);
            this.buttonDeleteStation.Name = "buttonDeleteStation";
            this.buttonDeleteStation.Radius = 8;
            this.buttonDeleteStation.Size = new System.Drawing.Size(82, 36);
            this.buttonDeleteStation.TabIndex = 2;
            this.buttonDeleteStation.Text = "删除";
            this.buttonDeleteStation.WaveSize = 0;
            // 
            // flowStationActionsLeft
            // 
            this.flowStationActionsLeft.Controls.Add(this.labelSelectedStation);
            this.flowStationActionsLeft.Controls.Add(this.labelStationTitle);
            this.flowStationActionsLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowStationActionsLeft.Gap = 8;
            this.flowStationActionsLeft.Location = new System.Drawing.Point(0, 0);
            this.flowStationActionsLeft.Margin = new System.Windows.Forms.Padding(0);
            this.flowStationActionsLeft.Name = "flowStationActionsLeft";
            this.flowStationActionsLeft.Size = new System.Drawing.Size(250, 36);
            this.flowStationActionsLeft.TabIndex = 0;
            // 
            // labelSelectedStation
            // 
            this.labelSelectedStation.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelSelectedStation.Location = new System.Drawing.Point(62, 0);
            this.labelSelectedStation.Margin = new System.Windows.Forms.Padding(0);
            this.labelSelectedStation.Name = "labelSelectedStation";
            this.labelSelectedStation.Size = new System.Drawing.Size(170, 36);
            this.labelSelectedStation.TabIndex = 1;
            this.labelSelectedStation.Text = "未选择 PLC";
            // 
            // labelStationTitle
            // 
            this.labelStationTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.labelStationTitle.Location = new System.Drawing.Point(0, 0);
            this.labelStationTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelStationTitle.Name = "labelStationTitle";
            this.labelStationTitle.Size = new System.Drawing.Size(54, 36);
            this.labelStationTitle.TabIndex = 0;
            this.labelStationTitle.Text = "站配置";
            // 
            // flowStats
            // 
            this.flowStats.Controls.Add(this.panelCurrentStationPointCard);
            this.flowStats.Controls.Add(this.panelPointTotalCard);
            this.flowStats.Controls.Add(this.panelOnlineStationCard);
            this.flowStats.Controls.Add(this.panelStationTotalCard);
            this.flowStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowStats.Gap = 8;
            this.flowStats.Location = new System.Drawing.Point(8, 52);
            this.flowStats.Margin = new System.Windows.Forms.Padding(0);
            this.flowStats.Name = "flowStats";
            this.flowStats.Padding = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.flowStats.Size = new System.Drawing.Size(1134, 88);
            this.flowStats.TabIndex = 1;
            // 
            // panelCurrentStationPointCard
            // 
            this.panelCurrentStationPointCard.BackColor = System.Drawing.Color.Transparent;
            this.panelCurrentStationPointCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(229)))), ((int)(((byte)(235)))));
            this.panelCurrentStationPointCard.Controls.Add(this.labelCurrentStationPointCount);
            this.panelCurrentStationPointCard.Controls.Add(this.labelCurrentStationPointTitle);
            this.panelCurrentStationPointCard.Location = new System.Drawing.Point(568, 6);
            this.panelCurrentStationPointCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelCurrentStationPointCard.Name = "panelCurrentStationPointCard";
            this.panelCurrentStationPointCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelCurrentStationPointCard.Radius = 12;
            this.panelCurrentStationPointCard.Shadow = 4;
            this.panelCurrentStationPointCard.ShadowOpacity = 0.2F;
            this.panelCurrentStationPointCard.ShadowOpacityAnimation = true;
            this.panelCurrentStationPointCard.Size = new System.Drawing.Size(180, 72);
            this.panelCurrentStationPointCard.TabIndex = 3;
            // 
            // labelCurrentStationPointCount
            // 
            this.labelCurrentStationPointCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelCurrentStationPointCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelCurrentStationPointCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(196)))), ((int)(((byte)(26)))));
            this.labelCurrentStationPointCount.Location = new System.Drawing.Point(92, 16);
            this.labelCurrentStationPointCount.Name = "labelCurrentStationPointCount";
            this.labelCurrentStationPointCount.Size = new System.Drawing.Size(72, 40);
            this.labelCurrentStationPointCount.TabIndex = 1;
            this.labelCurrentStationPointCount.Text = "0";
            // 
            // labelCurrentStationPointTitle
            // 
            this.labelCurrentStationPointTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelCurrentStationPointTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelCurrentStationPointTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(196)))), ((int)(((byte)(26)))));
            this.labelCurrentStationPointTitle.Location = new System.Drawing.Point(16, 16);
            this.labelCurrentStationPointTitle.Name = "labelCurrentStationPointTitle";
            this.labelCurrentStationPointTitle.Size = new System.Drawing.Size(70, 40);
            this.labelCurrentStationPointTitle.TabIndex = 0;
            this.labelCurrentStationPointTitle.Text = "当前站点位";
            // 
            // panelPointTotalCard
            // 
            this.panelPointTotalCard.BackColor = System.Drawing.Color.Transparent;
            this.panelPointTotalCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(229)))), ((int)(((byte)(235)))));
            this.panelPointTotalCard.Controls.Add(this.labelPointTotalCount);
            this.panelPointTotalCard.Controls.Add(this.labelPointTotalTitle);
            this.panelPointTotalCard.Location = new System.Drawing.Point(380, 6);
            this.panelPointTotalCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelPointTotalCard.Name = "panelPointTotalCard";
            this.panelPointTotalCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelPointTotalCard.Radius = 12;
            this.panelPointTotalCard.Shadow = 4;
            this.panelPointTotalCard.ShadowOpacity = 0.2F;
            this.panelPointTotalCard.ShadowOpacityAnimation = true;
            this.panelPointTotalCard.Size = new System.Drawing.Size(180, 72);
            this.panelPointTotalCard.TabIndex = 2;
            // 
            // labelPointTotalCount
            // 
            this.labelPointTotalCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelPointTotalCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelPointTotalCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(119)))), ((int)(((byte)(255)))));
            this.labelPointTotalCount.Location = new System.Drawing.Point(104, 16);
            this.labelPointTotalCount.Name = "labelPointTotalCount";
            this.labelPointTotalCount.Size = new System.Drawing.Size(60, 40);
            this.labelPointTotalCount.TabIndex = 1;
            this.labelPointTotalCount.Text = "0";
            // 
            // labelPointTotalTitle
            // 
            this.labelPointTotalTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelPointTotalTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelPointTotalTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(119)))), ((int)(((byte)(255)))));
            this.labelPointTotalTitle.Location = new System.Drawing.Point(16, 16);
            this.labelPointTotalTitle.Name = "labelPointTotalTitle";
            this.labelPointTotalTitle.Size = new System.Drawing.Size(72, 40);
            this.labelPointTotalTitle.TabIndex = 0;
            this.labelPointTotalTitle.Text = "点位总数";
            // 
            // panelOnlineStationCard
            // 
            this.panelOnlineStationCard.BackColor = System.Drawing.Color.Transparent;
            this.panelOnlineStationCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(229)))), ((int)(((byte)(235)))));
            this.panelOnlineStationCard.Controls.Add(this.labelOnlineStationCount);
            this.panelOnlineStationCard.Controls.Add(this.labelOnlineStationTitle);
            this.panelOnlineStationCard.Location = new System.Drawing.Point(192, 6);
            this.panelOnlineStationCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelOnlineStationCard.Name = "panelOnlineStationCard";
            this.panelOnlineStationCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelOnlineStationCard.Radius = 12;
            this.panelOnlineStationCard.Shadow = 4;
            this.panelOnlineStationCard.ShadowOpacity = 0.2F;
            this.panelOnlineStationCard.ShadowOpacityAnimation = true;
            this.panelOnlineStationCard.Size = new System.Drawing.Size(180, 72);
            this.panelOnlineStationCard.TabIndex = 1;
            // 
            // labelOnlineStationCount
            // 
            this.labelOnlineStationCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelOnlineStationCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelOnlineStationCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(196)))), ((int)(((byte)(26)))));
            this.labelOnlineStationCount.Location = new System.Drawing.Point(100, 16);
            this.labelOnlineStationCount.Name = "labelOnlineStationCount";
            this.labelOnlineStationCount.Size = new System.Drawing.Size(64, 40);
            this.labelOnlineStationCount.TabIndex = 1;
            this.labelOnlineStationCount.Text = "0";
            // 
            // labelOnlineStationTitle
            // 
            this.labelOnlineStationTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelOnlineStationTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelOnlineStationTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(196)))), ((int)(((byte)(26)))));
            this.labelOnlineStationTitle.Location = new System.Drawing.Point(16, 16);
            this.labelOnlineStationTitle.Name = "labelOnlineStationTitle";
            this.labelOnlineStationTitle.Size = new System.Drawing.Size(72, 40);
            this.labelOnlineStationTitle.TabIndex = 0;
            this.labelOnlineStationTitle.Text = "在线站数";
            // 
            // panelStationTotalCard
            // 
            this.panelStationTotalCard.BackColor = System.Drawing.Color.Transparent;
            this.panelStationTotalCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(229)))), ((int)(((byte)(235)))));
            this.panelStationTotalCard.Controls.Add(this.labelStationTotalCount);
            this.panelStationTotalCard.Controls.Add(this.labelStationTotalTitle);
            this.panelStationTotalCard.Location = new System.Drawing.Point(4, 6);
            this.panelStationTotalCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelStationTotalCard.Name = "panelStationTotalCard";
            this.panelStationTotalCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelStationTotalCard.Radius = 12;
            this.panelStationTotalCard.Shadow = 4;
            this.panelStationTotalCard.ShadowOpacity = 0.2F;
            this.panelStationTotalCard.ShadowOpacityAnimation = true;
            this.panelStationTotalCard.Size = new System.Drawing.Size(180, 72);
            this.panelStationTotalCard.TabIndex = 0;
            // 
            // labelStationTotalCount
            // 
            this.labelStationTotalCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelStationTotalCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelStationTotalCount.Location = new System.Drawing.Point(104, 16);
            this.labelStationTotalCount.Name = "labelStationTotalCount";
            this.labelStationTotalCount.Size = new System.Drawing.Size(60, 40);
            this.labelStationTotalCount.TabIndex = 1;
            this.labelStationTotalCount.Text = "0";
            // 
            // labelStationTotalTitle
            // 
            this.labelStationTotalTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelStationTotalTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelStationTotalTitle.Location = new System.Drawing.Point(16, 16);
            this.labelStationTotalTitle.Name = "labelStationTotalTitle";
            this.labelStationTotalTitle.Size = new System.Drawing.Size(72, 40);
            this.labelStationTotalTitle.TabIndex = 0;
            this.labelStationTotalTitle.Text = "PLC站数";
            // 
            // panelToolbar
            // 
            this.panelToolbar.Controls.Add(this.flowToolbarLeft);
            this.panelToolbar.Controls.Add(this.flowToolbarRight);
            this.panelToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelToolbar.Location = new System.Drawing.Point(8, 8);
            this.panelToolbar.Margin = new System.Windows.Forms.Padding(0);
            this.panelToolbar.Name = "panelToolbar";
            this.panelToolbar.Padding = new System.Windows.Forms.Padding(4);
            this.panelToolbar.Radius = 0;
            this.panelToolbar.Size = new System.Drawing.Size(1134, 44);
            this.panelToolbar.TabIndex = 0;
            // 
            // flowToolbarLeft
            // 
            this.flowToolbarLeft.Align = AntdUI.TAlignFlow.Left;
            this.flowToolbarLeft.Controls.Add(this.labelRuntimeSummary);
            this.flowToolbarLeft.Controls.Add(this.inputPointSearch);
            this.flowToolbarLeft.Controls.Add(this.inputStationSearch);
            this.flowToolbarLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowToolbarLeft.Gap = 8;
            this.flowToolbarLeft.Location = new System.Drawing.Point(4, 4);
            this.flowToolbarLeft.Margin = new System.Windows.Forms.Padding(0);
            this.flowToolbarLeft.Name = "flowToolbarLeft";
            this.flowToolbarLeft.Size = new System.Drawing.Size(313, 36);
            this.flowToolbarLeft.TabIndex = 0;
            // 
            // labelRuntimeSummary
            // 
            this.labelRuntimeSummary.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelRuntimeSummary.Location = new System.Drawing.Point(237, 0);
            this.labelRuntimeSummary.Margin = new System.Windows.Forms.Padding(0);
            this.labelRuntimeSummary.Name = "labelRuntimeSummary";
            this.labelRuntimeSummary.Size = new System.Drawing.Size(59, 36);
            this.labelRuntimeSummary.TabIndex = 2;
            this.labelRuntimeSummary.Text = "未加载";
            // 
            // inputPointSearch
            // 
            this.inputPointSearch.Location = new System.Drawing.Point(124, 0);
            this.inputPointSearch.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.inputPointSearch.Name = "inputPointSearch";
            this.inputPointSearch.PlaceholderText = "搜索点位名 / 分组 / 地址";
            this.inputPointSearch.Size = new System.Drawing.Size(105, 36);
            this.inputPointSearch.TabIndex = 1;
            this.inputPointSearch.WaveSize = 0;
            // 
            // inputStationSearch
            // 
            this.inputStationSearch.Location = new System.Drawing.Point(0, 0);
            this.inputStationSearch.Margin = new System.Windows.Forms.Padding(0);
            this.inputStationSearch.Name = "inputStationSearch";
            this.inputStationSearch.PlaceholderText = "搜索 PLC 名称 / 协议 / 端点";
            this.inputStationSearch.Size = new System.Drawing.Size(108, 36);
            this.inputStationSearch.TabIndex = 0;
            this.inputStationSearch.WaveSize = 0;
            // 
            // flowToolbarRight
            // 
            this.flowToolbarRight.Align = AntdUI.TAlignFlow.RightCenter;
            this.flowToolbarRight.Controls.Add(this.buttonStopScan);
            this.flowToolbarRight.Controls.Add(this.buttonStartScan);
            this.flowToolbarRight.Controls.Add(this.buttonScanOnce);
            this.flowToolbarRight.Controls.Add(this.buttonReloadConfig);
            this.flowToolbarRight.Controls.Add(this.buttonRefresh);
            this.flowToolbarRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowToolbarRight.Gap = 8;
            this.flowToolbarRight.Location = new System.Drawing.Point(590, 4);
            this.flowToolbarRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowToolbarRight.Name = "flowToolbarRight";
            this.flowToolbarRight.Size = new System.Drawing.Size(540, 36);
            this.flowToolbarRight.TabIndex = 1;
            // 
            // buttonStopScan
            // 
            this.buttonStopScan.IconSvg = "PauseCircleOutlined";
            this.buttonStopScan.Location = new System.Drawing.Point(443, 0);
            this.buttonStopScan.Margin = new System.Windows.Forms.Padding(0);
            this.buttonStopScan.Name = "buttonStopScan";
            this.buttonStopScan.Radius = 8;
            this.buttonStopScan.Size = new System.Drawing.Size(97, 36);
            this.buttonStopScan.TabIndex = 4;
            this.buttonStopScan.Text = "停止扫描";
            this.buttonStopScan.WaveSize = 0;
            // 
            // buttonStartScan
            // 
            this.buttonStartScan.IconSvg = "PlayCircleOutlined";
            this.buttonStartScan.Location = new System.Drawing.Point(339, 0);
            this.buttonStartScan.Margin = new System.Windows.Forms.Padding(0);
            this.buttonStartScan.Name = "buttonStartScan";
            this.buttonStartScan.Radius = 8;
            this.buttonStartScan.Size = new System.Drawing.Size(96, 36);
            this.buttonStartScan.TabIndex = 3;
            this.buttonStartScan.Text = "启动扫描";
            this.buttonStartScan.WaveSize = 0;
            // 
            // buttonScanOnce
            // 
            this.buttonScanOnce.IconSvg = "ThunderboltOutlined";
            this.buttonScanOnce.Location = new System.Drawing.Point(235, 0);
            this.buttonScanOnce.Margin = new System.Windows.Forms.Padding(0);
            this.buttonScanOnce.Name = "buttonScanOnce";
            this.buttonScanOnce.Radius = 8;
            this.buttonScanOnce.Size = new System.Drawing.Size(96, 36);
            this.buttonScanOnce.TabIndex = 2;
            this.buttonScanOnce.Text = "单轮扫描";
            this.buttonScanOnce.WaveSize = 0;
            // 
            // buttonReloadConfig
            // 
            this.buttonReloadConfig.IconSvg = "SyncOutlined";
            this.buttonReloadConfig.Location = new System.Drawing.Point(145, 0);
            this.buttonReloadConfig.Margin = new System.Windows.Forms.Padding(0);
            this.buttonReloadConfig.Name = "buttonReloadConfig";
            this.buttonReloadConfig.Radius = 8;
            this.buttonReloadConfig.Size = new System.Drawing.Size(82, 36);
            this.buttonReloadConfig.TabIndex = 1;
            this.buttonReloadConfig.Text = "重载";
            this.buttonReloadConfig.Type = AntdUI.TTypeMini.Primary;
            this.buttonReloadConfig.WaveSize = 0;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.IconSvg = "ReloadOutlined";
            this.buttonRefresh.Location = new System.Drawing.Point(59, 0);
            this.buttonRefresh.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Radius = 8;
            this.buttonRefresh.Size = new System.Drawing.Size(78, 36);
            this.buttonRefresh.TabIndex = 0;
            this.buttonRefresh.Text = "刷新";
            this.buttonRefresh.WaveSize = 0;
            // 
            // PlcConfigManagementPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "PlcConfigManagementPage";
            this.Size = new System.Drawing.Size(1150, 680);
            this.panelRoot.ResumeLayout(false);
            this.panelContentCard.ResumeLayout(false);
            this.gridContent.ResumeLayout(false);
            this.panelPointCard.ResumeLayout(false);
            this.panelPointHeader.ResumeLayout(false);
            this.flowPointActionsRight.ResumeLayout(false);
            this.flowPointActionsLeft.ResumeLayout(false);
            this.panelStationCard.ResumeLayout(false);
            this.panelStationHeader.ResumeLayout(false);
            this.flowStationActionsRight.ResumeLayout(false);
            this.flowStationActionsLeft.ResumeLayout(false);
            this.flowStats.ResumeLayout(false);
            this.panelCurrentStationPointCard.ResumeLayout(false);
            this.panelPointTotalCard.ResumeLayout(false);
            this.panelOnlineStationCard.ResumeLayout(false);
            this.panelStationTotalCard.ResumeLayout(false);
            this.panelToolbar.ResumeLayout(false);
            this.flowToolbarLeft.ResumeLayout(false);
            this.flowToolbarRight.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AntdUI.Panel panelRoot;
        private AntdUI.Panel panelToolbar;
        private AntdUI.FlowPanel flowToolbarRight;
        private AntdUI.Button buttonStopScan;
        private AntdUI.Button buttonStartScan;
        private AntdUI.Button buttonScanOnce;
        private AntdUI.Button buttonReloadConfig;
        private AntdUI.Button buttonRefresh;
        private AntdUI.FlowPanel flowToolbarLeft;
        private AntdUI.Label labelRuntimeSummary;
        private AntdUI.Input inputPointSearch;
        private AntdUI.Input inputStationSearch;
        private AntdUI.FlowPanel flowStats;
        private AntdUI.Panel panelCurrentStationPointCard;
        private AntdUI.Label labelCurrentStationPointCount;
        private AntdUI.Label labelCurrentStationPointTitle;
        private AntdUI.Panel panelPointTotalCard;
        private AntdUI.Label labelPointTotalCount;
        private AntdUI.Label labelPointTotalTitle;
        private AntdUI.Panel panelOnlineStationCard;
        private AntdUI.Label labelOnlineStationCount;
        private AntdUI.Label labelOnlineStationTitle;
        private AntdUI.Panel panelStationTotalCard;
        private AntdUI.Label labelStationTotalCount;
        private AntdUI.Label labelStationTotalTitle;
        private AntdUI.Panel panelContentCard;
        private AntdUI.GridPanel gridContent;
        private AntdUI.Panel panelPointCard;
        private AntdUI.Table tablePoints;
        private AntdUI.Panel panelPointHeader;
        private AntdUI.FlowPanel flowPointActionsRight;
        private AntdUI.Button buttonDeletePoint;
        private AntdUI.Button buttonEditPoint;
        private AntdUI.Button buttonAddPoint;
        private AntdUI.FlowPanel flowPointActionsLeft;
        private AntdUI.Label labelSelectedPoint;
        private AntdUI.Label labelPointTitle;
        private AntdUI.Panel panelStationCard;
        private AntdUI.Table tableStations;
        private AntdUI.Panel panelStationHeader;
        private AntdUI.FlowPanel flowStationActionsRight;
        private AntdUI.Button buttonDeleteStation;
        private AntdUI.Button buttonEditStation;
        private AntdUI.Button buttonAddStation;
        private AntdUI.FlowPanel flowStationActionsLeft;
        private AntdUI.Label labelSelectedStation;
        private AntdUI.Label labelStationTitle;
    }
}