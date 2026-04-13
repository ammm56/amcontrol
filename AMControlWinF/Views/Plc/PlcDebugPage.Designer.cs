namespace AMControlWinF.Views.Plc
{
    partial class PlcDebugPage
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

        #region 组件设计器生成的代码

        private void InitializeComponent()
        {
            this.panelRoot = new AntdUI.Panel();
            this.panelContentCard = new AntdUI.Panel();
            this.gridContent = new AntdUI.GridPanel();
            this.panelHistoryCard = new AntdUI.Panel();
            this.tableResults = new AntdUI.Table();
            this.panelHistoryHeader = new AntdUI.Panel();
            this.flowHistoryHeaderLeft = new AntdUI.FlowPanel();
            this.labelHistorySummary = new AntdUI.Label();
            this.labelHistoryTitle = new AntdUI.Label();
            this.panelDebugOpsCard = new AntdUI.Panel();
            this.panelOpsScroll = new System.Windows.Forms.Panel();
            this.panelDirectDebugCard = new AntdUI.Panel();
            this.flowDirectBottomRight = new AntdUI.FlowPanel();
            this.buttonWriteAddress = new AntdUI.Button();
            this.buttonTestReadAddress = new AntdUI.Button();
            this.checkDirectWriteConfirmed = new AntdUI.Checkbox();
            this.inputDirectWriteValue = new AntdUI.Input();
            this.labelDirectWriteValue = new AntdUI.Label();
            this.inputDirectLength = new AntdUI.Input();
            this.labelDirectLength = new AntdUI.Label();
            this.selectDirectDataType = new AntdUI.Select();
            this.labelDirectDataType = new AntdUI.Label();
            this.inputDirectAddress = new AntdUI.Input();
            this.labelDirectAddress = new AntdUI.Label();
            this.labelDirectWriteTitle = new AntdUI.Label();
            this.panelPointDebugCard = new AntdUI.Panel();
            this.flowPointBottomRight = new AntdUI.FlowPanel();
            this.buttonWritePoint = new AntdUI.Button();
            this.buttonTestReadPoint = new AntdUI.Button();
            this.inputPointWriteValue = new AntdUI.Input();
            this.labelPointWriteValue = new AntdUI.Label();
            this.inputPointLength = new AntdUI.Input();
            this.labelPointLength = new AntdUI.Label();
            this.inputPointDataType = new AntdUI.Input();
            this.labelPointDataType = new AntdUI.Label();
            this.inputPointAddress = new AntdUI.Input();
            this.labelPointAddress = new AntdUI.Label();
            this.selectPoint = new AntdUI.Select();
            this.labelPointSelect = new AntdUI.Label();
            this.labelPointOpTitle = new AntdUI.Label();
            this.panelSpacer = new AntdUI.Panel();
            this.panelToolbar = new AntdUI.Panel();
            this.flowToolbarLeft = new AntdUI.FlowPanel();
            this.labelRuntimeSummary = new AntdUI.Label();
            this.selectPlcGlobal = new AntdUI.Select();
            this.flowToolbarRight = new AntdUI.FlowPanel();
            this.buttonRefresh = new AntdUI.Button();
            this.panelRoot.SuspendLayout();
            this.panelContentCard.SuspendLayout();
            this.gridContent.SuspendLayout();
            this.panelHistoryCard.SuspendLayout();
            this.panelHistoryHeader.SuspendLayout();
            this.flowHistoryHeaderLeft.SuspendLayout();
            this.panelDebugOpsCard.SuspendLayout();
            this.panelOpsScroll.SuspendLayout();
            this.panelDirectDebugCard.SuspendLayout();
            this.flowDirectBottomRight.SuspendLayout();
            this.panelPointDebugCard.SuspendLayout();
            this.flowPointBottomRight.SuspendLayout();
            this.panelToolbar.SuspendLayout();
            this.flowToolbarLeft.SuspendLayout();
            this.flowToolbarRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.panelContentCard);
            this.panelRoot.Controls.Add(this.panelSpacer);
            this.panelRoot.Controls.Add(this.panelToolbar);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Margin = new System.Windows.Forms.Padding(0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(8);
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(1100, 680);
            this.panelRoot.TabIndex = 0;
            // 
            // panelContentCard
            // 
            this.panelContentCard.BackColor = System.Drawing.Color.Transparent;
            this.panelContentCard.Controls.Add(this.gridContent);
            this.panelContentCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContentCard.Location = new System.Drawing.Point(8, 62);
            this.panelContentCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelContentCard.Name = "panelContentCard";
            this.panelContentCard.Radius = 0;
            this.panelContentCard.ShadowOpacity = 0F;
            this.panelContentCard.ShadowOpacityHover = 0F;
            this.panelContentCard.Size = new System.Drawing.Size(1084, 610);
            this.panelContentCard.TabIndex = 2;
            // 
            // gridContent
            // 
            this.gridContent.Controls.Add(this.panelHistoryCard);
            this.gridContent.Controls.Add(this.panelDebugOpsCard);
            this.gridContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridContent.Location = new System.Drawing.Point(0, 0);
            this.gridContent.Margin = new System.Windows.Forms.Padding(0);
            this.gridContent.Name = "gridContent";
            this.gridContent.Size = new System.Drawing.Size(1084, 610);
            this.gridContent.Span = "350 100%";
            this.gridContent.TabIndex = 0;
            // 
            // panelHistoryCard
            // 
            this.panelHistoryCard.BackColor = System.Drawing.Color.Transparent;
            this.panelHistoryCard.Controls.Add(this.tableResults);
            this.panelHistoryCard.Controls.Add(this.panelHistoryHeader);
            this.panelHistoryCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHistoryCard.Location = new System.Drawing.Point(350, 0);
            this.panelHistoryCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelHistoryCard.Name = "panelHistoryCard";
            this.panelHistoryCard.Padding = new System.Windows.Forms.Padding(8);
            this.panelHistoryCard.Radius = 12;
            this.panelHistoryCard.Shadow = 4;
            this.panelHistoryCard.ShadowOpacity = 0.15F;
            this.panelHistoryCard.Size = new System.Drawing.Size(734, 610);
            this.panelHistoryCard.TabIndex = 1;
            // 
            // tableResults
            // 
            this.tableResults.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tableResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableResults.EmptyHeader = true;
            this.tableResults.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.tableResults.Gap = 8;
            this.tableResults.Gaps = new System.Drawing.Size(8, 8);
            this.tableResults.Location = new System.Drawing.Point(12, 56);
            this.tableResults.Margin = new System.Windows.Forms.Padding(0);
            this.tableResults.Name = "tableResults";
            this.tableResults.ShowTip = false;
            this.tableResults.Size = new System.Drawing.Size(710, 542);
            this.tableResults.TabIndex = 1;
            this.tableResults.Text = "tableResults";
            // 
            // panelHistoryHeader
            // 
            this.panelHistoryHeader.Controls.Add(this.flowHistoryHeaderLeft);
            this.panelHistoryHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHistoryHeader.Location = new System.Drawing.Point(12, 12);
            this.panelHistoryHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelHistoryHeader.Name = "panelHistoryHeader";
            this.panelHistoryHeader.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.panelHistoryHeader.Radius = 0;
            this.panelHistoryHeader.Size = new System.Drawing.Size(710, 44);
            this.panelHistoryHeader.TabIndex = 0;
            // 
            // flowHistoryHeaderLeft
            // 
            this.flowHistoryHeaderLeft.Controls.Add(this.labelHistorySummary);
            this.flowHistoryHeaderLeft.Controls.Add(this.labelHistoryTitle);
            this.flowHistoryHeaderLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowHistoryHeaderLeft.Gap = 8;
            this.flowHistoryHeaderLeft.Location = new System.Drawing.Point(0, 0);
            this.flowHistoryHeaderLeft.Margin = new System.Windows.Forms.Padding(0);
            this.flowHistoryHeaderLeft.Name = "flowHistoryHeaderLeft";
            this.flowHistoryHeaderLeft.Size = new System.Drawing.Size(360, 36);
            this.flowHistoryHeaderLeft.TabIndex = 0;
            // 
            // labelHistorySummary
            // 
            this.labelHistorySummary.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelHistorySummary.Location = new System.Drawing.Point(88, 0);
            this.labelHistorySummary.Margin = new System.Windows.Forms.Padding(0);
            this.labelHistorySummary.Name = "labelHistorySummary";
            this.labelHistorySummary.Size = new System.Drawing.Size(272, 36);
            this.labelHistorySummary.TabIndex = 1;
            this.labelHistorySummary.Text = "最近 20 条执行结果";
            // 
            // labelHistoryTitle
            // 
            this.labelHistoryTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.labelHistoryTitle.Location = new System.Drawing.Point(0, 0);
            this.labelHistoryTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelHistoryTitle.Name = "labelHistoryTitle";
            this.labelHistoryTitle.Size = new System.Drawing.Size(80, 36);
            this.labelHistoryTitle.TabIndex = 0;
            this.labelHistoryTitle.Text = "执行记录";
            // 
            // panelDebugOpsCard
            // 
            this.panelDebugOpsCard.BackColor = System.Drawing.Color.Transparent;
            this.panelDebugOpsCard.Controls.Add(this.panelOpsScroll);
            this.panelDebugOpsCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDebugOpsCard.Location = new System.Drawing.Point(0, 0);
            this.panelDebugOpsCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelDebugOpsCard.Name = "panelDebugOpsCard";
            this.panelDebugOpsCard.Radius = 0;
            this.panelDebugOpsCard.ShadowOpacity = 0F;
            this.panelDebugOpsCard.ShadowOpacityHover = 0F;
            this.panelDebugOpsCard.Size = new System.Drawing.Size(350, 610);
            this.panelDebugOpsCard.TabIndex = 0;
            // 
            // panelOpsScroll
            // 
            this.panelOpsScroll.AutoScroll = true;
            this.panelOpsScroll.Controls.Add(this.panelDirectDebugCard);
            this.panelOpsScroll.Controls.Add(this.panelPointDebugCard);
            this.panelOpsScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOpsScroll.Location = new System.Drawing.Point(0, 0);
            this.panelOpsScroll.Margin = new System.Windows.Forms.Padding(0);
            this.panelOpsScroll.Name = "panelOpsScroll";
            this.panelOpsScroll.Size = new System.Drawing.Size(350, 610);
            this.panelOpsScroll.TabIndex = 0;
            // 
            // panelDirectDebugCard
            // 
            this.panelDirectDebugCard.Controls.Add(this.flowDirectBottomRight);
            this.panelDirectDebugCard.Controls.Add(this.checkDirectWriteConfirmed);
            this.panelDirectDebugCard.Controls.Add(this.inputDirectWriteValue);
            this.panelDirectDebugCard.Controls.Add(this.labelDirectWriteValue);
            this.panelDirectDebugCard.Controls.Add(this.inputDirectLength);
            this.panelDirectDebugCard.Controls.Add(this.labelDirectLength);
            this.panelDirectDebugCard.Controls.Add(this.selectDirectDataType);
            this.panelDirectDebugCard.Controls.Add(this.labelDirectDataType);
            this.panelDirectDebugCard.Controls.Add(this.inputDirectAddress);
            this.panelDirectDebugCard.Controls.Add(this.labelDirectAddress);
            this.panelDirectDebugCard.Controls.Add(this.labelDirectWriteTitle);
            this.panelDirectDebugCard.Location = new System.Drawing.Point(0, 292);
            this.panelDirectDebugCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelDirectDebugCard.Name = "panelDirectDebugCard";
            this.panelDirectDebugCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelDirectDebugCard.Radius = 12;
            this.panelDirectDebugCard.Shadow = 4;
            this.panelDirectDebugCard.ShadowOpacity = 0.15F;
            this.panelDirectDebugCard.Size = new System.Drawing.Size(350, 240);
            this.panelDirectDebugCard.TabIndex = 1;
            // 
            // flowDirectBottomRight
            // 
            this.flowDirectBottomRight.Align = AntdUI.TAlignFlow.RightCenter;
            this.flowDirectBottomRight.Controls.Add(this.buttonWriteAddress);
            this.flowDirectBottomRight.Controls.Add(this.buttonTestReadAddress);
            this.flowDirectBottomRight.Location = new System.Drawing.Point(126, 178);
            this.flowDirectBottomRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowDirectBottomRight.Name = "flowDirectBottomRight";
            this.flowDirectBottomRight.Size = new System.Drawing.Size(208, 36);
            this.flowDirectBottomRight.TabIndex = 10;
            // 
            // buttonWriteAddress
            // 
            this.buttonWriteAddress.IconSvg = "EditOutlined";
            this.buttonWriteAddress.Location = new System.Drawing.Point(113, 0);
            this.buttonWriteAddress.Margin = new System.Windows.Forms.Padding(0);
            this.buttonWriteAddress.Name = "buttonWriteAddress";
            this.buttonWriteAddress.Radius = 8;
            this.buttonWriteAddress.Size = new System.Drawing.Size(95, 36);
            this.buttonWriteAddress.TabIndex = 1;
            this.buttonWriteAddress.Text = "写入";
            this.buttonWriteAddress.Type = AntdUI.TTypeMini.Primary;
            this.buttonWriteAddress.WaveSize = 0;
            // 
            // buttonTestReadAddress
            // 
            this.buttonTestReadAddress.IconSvg = "SearchOutlined";
            this.buttonTestReadAddress.Location = new System.Drawing.Point(18, 0);
            this.buttonTestReadAddress.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTestReadAddress.Name = "buttonTestReadAddress";
            this.buttonTestReadAddress.Radius = 8;
            this.buttonTestReadAddress.Size = new System.Drawing.Size(95, 36);
            this.buttonTestReadAddress.TabIndex = 0;
            this.buttonTestReadAddress.Text = "读取";
            this.buttonTestReadAddress.WaveSize = 0;
            // 
            // checkDirectWriteConfirmed
            // 
            this.checkDirectWriteConfirmed.Location = new System.Drawing.Point(16, 184);
            this.checkDirectWriteConfirmed.Margin = new System.Windows.Forms.Padding(0);
            this.checkDirectWriteConfirmed.Name = "checkDirectWriteConfirmed";
            this.checkDirectWriteConfirmed.Size = new System.Drawing.Size(124, 24);
            this.checkDirectWriteConfirmed.TabIndex = 9;
            this.checkDirectWriteConfirmed.Text = "风险写入确认";
            // 
            // inputDirectWriteValue
            // 
            this.inputDirectWriteValue.Location = new System.Drawing.Point(96, 128);
            this.inputDirectWriteValue.Margin = new System.Windows.Forms.Padding(0);
            this.inputDirectWriteValue.Name = "inputDirectWriteValue";
            this.inputDirectWriteValue.Size = new System.Drawing.Size(238, 32);
            this.inputDirectWriteValue.TabIndex = 8;
            this.inputDirectWriteValue.WaveSize = 0;
            // 
            // labelDirectWriteValue
            // 
            this.labelDirectWriteValue.ForeColor = System.Drawing.Color.Gray;
            this.labelDirectWriteValue.Location = new System.Drawing.Point(16, 128);
            this.labelDirectWriteValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelDirectWriteValue.Name = "labelDirectWriteValue";
            this.labelDirectWriteValue.Size = new System.Drawing.Size(72, 32);
            this.labelDirectWriteValue.TabIndex = 7;
            this.labelDirectWriteValue.Text = "写入值";
            this.labelDirectWriteValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // inputDirectLength
            // 
            this.inputDirectLength.Location = new System.Drawing.Point(245, 88);
            this.inputDirectLength.Margin = new System.Windows.Forms.Padding(0);
            this.inputDirectLength.Name = "inputDirectLength";
            this.inputDirectLength.Size = new System.Drawing.Size(89, 32);
            this.inputDirectLength.TabIndex = 6;
            this.inputDirectLength.WaveSize = 0;
            // 
            // labelDirectLength
            // 
            this.labelDirectLength.ForeColor = System.Drawing.Color.Gray;
            this.labelDirectLength.Location = new System.Drawing.Point(189, 88);
            this.labelDirectLength.Margin = new System.Windows.Forms.Padding(0);
            this.labelDirectLength.Name = "labelDirectLength";
            this.labelDirectLength.Size = new System.Drawing.Size(48, 32);
            this.labelDirectLength.TabIndex = 5;
            this.labelDirectLength.Text = "长度";
            // 
            // selectDirectDataType
            // 
            this.selectDirectDataType.Location = new System.Drawing.Point(96, 88);
            this.selectDirectDataType.Margin = new System.Windows.Forms.Padding(0);
            this.selectDirectDataType.Name = "selectDirectDataType";
            this.selectDirectDataType.Size = new System.Drawing.Size(80, 32);
            this.selectDirectDataType.TabIndex = 4;
            this.selectDirectDataType.WaveSize = 0;
            // 
            // labelDirectDataType
            // 
            this.labelDirectDataType.ForeColor = System.Drawing.Color.Gray;
            this.labelDirectDataType.Location = new System.Drawing.Point(16, 88);
            this.labelDirectDataType.Margin = new System.Windows.Forms.Padding(0);
            this.labelDirectDataType.Name = "labelDirectDataType";
            this.labelDirectDataType.Size = new System.Drawing.Size(72, 32);
            this.labelDirectDataType.TabIndex = 3;
            this.labelDirectDataType.Text = "数据类型";
            this.labelDirectDataType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // inputDirectAddress
            // 
            this.inputDirectAddress.Location = new System.Drawing.Point(96, 48);
            this.inputDirectAddress.Margin = new System.Windows.Forms.Padding(0);
            this.inputDirectAddress.Name = "inputDirectAddress";
            this.inputDirectAddress.Size = new System.Drawing.Size(238, 32);
            this.inputDirectAddress.TabIndex = 2;
            this.inputDirectAddress.WaveSize = 0;
            // 
            // labelDirectAddress
            // 
            this.labelDirectAddress.ForeColor = System.Drawing.Color.Gray;
            this.labelDirectAddress.Location = new System.Drawing.Point(16, 48);
            this.labelDirectAddress.Margin = new System.Windows.Forms.Padding(0);
            this.labelDirectAddress.Name = "labelDirectAddress";
            this.labelDirectAddress.Size = new System.Drawing.Size(72, 32);
            this.labelDirectAddress.TabIndex = 1;
            this.labelDirectAddress.Text = "地址";
            this.labelDirectAddress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelDirectWriteTitle
            // 
            this.labelDirectWriteTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelDirectWriteTitle.Location = new System.Drawing.Point(16, 16);
            this.labelDirectWriteTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelDirectWriteTitle.Name = "labelDirectWriteTitle";
            this.labelDirectWriteTitle.Size = new System.Drawing.Size(300, 24);
            this.labelDirectWriteTitle.TabIndex = 0;
            this.labelDirectWriteTitle.Text = "按地址读写";
            // 
            // panelPointDebugCard
            // 
            this.panelPointDebugCard.Controls.Add(this.flowPointBottomRight);
            this.panelPointDebugCard.Controls.Add(this.inputPointWriteValue);
            this.panelPointDebugCard.Controls.Add(this.labelPointWriteValue);
            this.panelPointDebugCard.Controls.Add(this.inputPointLength);
            this.panelPointDebugCard.Controls.Add(this.labelPointLength);
            this.panelPointDebugCard.Controls.Add(this.inputPointDataType);
            this.panelPointDebugCard.Controls.Add(this.labelPointDataType);
            this.panelPointDebugCard.Controls.Add(this.inputPointAddress);
            this.panelPointDebugCard.Controls.Add(this.labelPointAddress);
            this.panelPointDebugCard.Controls.Add(this.selectPoint);
            this.panelPointDebugCard.Controls.Add(this.labelPointSelect);
            this.panelPointDebugCard.Controls.Add(this.labelPointOpTitle);
            this.panelPointDebugCard.Location = new System.Drawing.Point(0, 0);
            this.panelPointDebugCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelPointDebugCard.Name = "panelPointDebugCard";
            this.panelPointDebugCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelPointDebugCard.Radius = 12;
            this.panelPointDebugCard.Shadow = 4;
            this.panelPointDebugCard.ShadowOpacity = 0.15F;
            this.panelPointDebugCard.Size = new System.Drawing.Size(350, 276);
            this.panelPointDebugCard.TabIndex = 0;
            // 
            // flowPointBottomRight
            // 
            this.flowPointBottomRight.Align = AntdUI.TAlignFlow.RightCenter;
            this.flowPointBottomRight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowPointBottomRight.Controls.Add(this.buttonWritePoint);
            this.flowPointBottomRight.Controls.Add(this.buttonTestReadPoint);
            this.flowPointBottomRight.Gap = 8;
            this.flowPointBottomRight.Location = new System.Drawing.Point(126, 218);
            this.flowPointBottomRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowPointBottomRight.Name = "flowPointBottomRight";
            this.flowPointBottomRight.Size = new System.Drawing.Size(208, 36);
            this.flowPointBottomRight.TabIndex = 11;
            // 
            // buttonWritePoint
            // 
            this.buttonWritePoint.IconSvg = "EditOutlined";
            this.buttonWritePoint.Location = new System.Drawing.Point(113, 0);
            this.buttonWritePoint.Margin = new System.Windows.Forms.Padding(0);
            this.buttonWritePoint.Name = "buttonWritePoint";
            this.buttonWritePoint.Radius = 8;
            this.buttonWritePoint.Size = new System.Drawing.Size(95, 36);
            this.buttonWritePoint.TabIndex = 1;
            this.buttonWritePoint.Text = "写入点位";
            this.buttonWritePoint.Type = AntdUI.TTypeMini.Primary;
            this.buttonWritePoint.WaveSize = 0;
            // 
            // buttonTestReadPoint
            // 
            this.buttonTestReadPoint.IconSvg = "SearchOutlined";
            this.buttonTestReadPoint.Location = new System.Drawing.Point(10, 0);
            this.buttonTestReadPoint.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTestReadPoint.Name = "buttonTestReadPoint";
            this.buttonTestReadPoint.Radius = 8;
            this.buttonTestReadPoint.Size = new System.Drawing.Size(95, 36);
            this.buttonTestReadPoint.TabIndex = 0;
            this.buttonTestReadPoint.Text = "读取点位";
            this.buttonTestReadPoint.WaveSize = 0;
            // 
            // inputPointWriteValue
            // 
            this.inputPointWriteValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inputPointWriteValue.Location = new System.Drawing.Point(96, 168);
            this.inputPointWriteValue.Margin = new System.Windows.Forms.Padding(0);
            this.inputPointWriteValue.Name = "inputPointWriteValue";
            this.inputPointWriteValue.Size = new System.Drawing.Size(238, 32);
            this.inputPointWriteValue.TabIndex = 10;
            this.inputPointWriteValue.WaveSize = 0;
            // 
            // labelPointWriteValue
            // 
            this.labelPointWriteValue.ForeColor = System.Drawing.Color.Gray;
            this.labelPointWriteValue.Location = new System.Drawing.Point(16, 168);
            this.labelPointWriteValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelPointWriteValue.Name = "labelPointWriteValue";
            this.labelPointWriteValue.Size = new System.Drawing.Size(72, 32);
            this.labelPointWriteValue.TabIndex = 9;
            this.labelPointWriteValue.Text = "写入值";
            this.labelPointWriteValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // inputPointLength
            // 
            this.inputPointLength.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inputPointLength.Location = new System.Drawing.Point(245, 128);
            this.inputPointLength.Margin = new System.Windows.Forms.Padding(0);
            this.inputPointLength.Name = "inputPointLength";
            this.inputPointLength.Size = new System.Drawing.Size(89, 32);
            this.inputPointLength.TabIndex = 8;
            this.inputPointLength.WaveSize = 0;
            // 
            // labelPointLength
            // 
            this.labelPointLength.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPointLength.ForeColor = System.Drawing.Color.Gray;
            this.labelPointLength.Location = new System.Drawing.Point(189, 128);
            this.labelPointLength.Margin = new System.Windows.Forms.Padding(0);
            this.labelPointLength.Name = "labelPointLength";
            this.labelPointLength.Size = new System.Drawing.Size(48, 32);
            this.labelPointLength.TabIndex = 7;
            this.labelPointLength.Text = "长度";
            // 
            // inputPointDataType
            // 
            this.inputPointDataType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inputPointDataType.Location = new System.Drawing.Point(96, 128);
            this.inputPointDataType.Margin = new System.Windows.Forms.Padding(0);
            this.inputPointDataType.Name = "inputPointDataType";
            this.inputPointDataType.Size = new System.Drawing.Size(80, 32);
            this.inputPointDataType.TabIndex = 6;
            this.inputPointDataType.WaveSize = 0;
            // 
            // labelPointDataType
            // 
            this.labelPointDataType.ForeColor = System.Drawing.Color.Gray;
            this.labelPointDataType.Location = new System.Drawing.Point(16, 128);
            this.labelPointDataType.Margin = new System.Windows.Forms.Padding(0);
            this.labelPointDataType.Name = "labelPointDataType";
            this.labelPointDataType.Size = new System.Drawing.Size(72, 32);
            this.labelPointDataType.TabIndex = 5;
            this.labelPointDataType.Text = "数据类型";
            this.labelPointDataType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // inputPointAddress
            // 
            this.inputPointAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inputPointAddress.Location = new System.Drawing.Point(96, 88);
            this.inputPointAddress.Margin = new System.Windows.Forms.Padding(0);
            this.inputPointAddress.Name = "inputPointAddress";
            this.inputPointAddress.Size = new System.Drawing.Size(238, 32);
            this.inputPointAddress.TabIndex = 4;
            this.inputPointAddress.WaveSize = 0;
            // 
            // labelPointAddress
            // 
            this.labelPointAddress.ForeColor = System.Drawing.Color.Gray;
            this.labelPointAddress.Location = new System.Drawing.Point(16, 88);
            this.labelPointAddress.Margin = new System.Windows.Forms.Padding(0);
            this.labelPointAddress.Name = "labelPointAddress";
            this.labelPointAddress.Size = new System.Drawing.Size(72, 32);
            this.labelPointAddress.TabIndex = 3;
            this.labelPointAddress.Text = "地址";
            this.labelPointAddress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // selectPoint
            // 
            this.selectPoint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectPoint.Location = new System.Drawing.Point(96, 48);
            this.selectPoint.Margin = new System.Windows.Forms.Padding(0);
            this.selectPoint.Name = "selectPoint";
            this.selectPoint.Size = new System.Drawing.Size(238, 32);
            this.selectPoint.TabIndex = 2;
            this.selectPoint.WaveSize = 0;
            // 
            // labelPointSelect
            // 
            this.labelPointSelect.ForeColor = System.Drawing.Color.Gray;
            this.labelPointSelect.Location = new System.Drawing.Point(16, 48);
            this.labelPointSelect.Margin = new System.Windows.Forms.Padding(0);
            this.labelPointSelect.Name = "labelPointSelect";
            this.labelPointSelect.Size = new System.Drawing.Size(72, 32);
            this.labelPointSelect.TabIndex = 1;
            this.labelPointSelect.Text = "配置点位";
            this.labelPointSelect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelPointOpTitle
            // 
            this.labelPointOpTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelPointOpTitle.Location = new System.Drawing.Point(16, 16);
            this.labelPointOpTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelPointOpTitle.Name = "labelPointOpTitle";
            this.labelPointOpTitle.Size = new System.Drawing.Size(300, 24);
            this.labelPointOpTitle.TabIndex = 0;
            this.labelPointOpTitle.Text = "按配置点位调试";
            // 
            // panelSpacer
            // 
            this.panelSpacer.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSpacer.Location = new System.Drawing.Point(8, 52);
            this.panelSpacer.Margin = new System.Windows.Forms.Padding(0);
            this.panelSpacer.Name = "panelSpacer";
            this.panelSpacer.Radius = 0;
            this.panelSpacer.Size = new System.Drawing.Size(1084, 10);
            this.panelSpacer.TabIndex = 1;
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
            this.panelToolbar.Size = new System.Drawing.Size(1084, 44);
            this.panelToolbar.TabIndex = 0;
            // 
            // flowToolbarLeft
            // 
            this.flowToolbarLeft.Controls.Add(this.labelRuntimeSummary);
            this.flowToolbarLeft.Controls.Add(this.selectPlcGlobal);
            this.flowToolbarLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowToolbarLeft.Gap = 8;
            this.flowToolbarLeft.Location = new System.Drawing.Point(4, 4);
            this.flowToolbarLeft.Margin = new System.Windows.Forms.Padding(0);
            this.flowToolbarLeft.Name = "flowToolbarLeft";
            this.flowToolbarLeft.Size = new System.Drawing.Size(625, 36);
            this.flowToolbarLeft.TabIndex = 0;
            this.flowToolbarLeft.Text = "flowToolbarLeft";
            // 
            // labelRuntimeSummary
            // 
            this.labelRuntimeSummary.Location = new System.Drawing.Point(228, 0);
            this.labelRuntimeSummary.Margin = new System.Windows.Forms.Padding(0);
            this.labelRuntimeSummary.Name = "labelRuntimeSummary";
            this.labelRuntimeSummary.Size = new System.Drawing.Size(260, 40);
            this.labelRuntimeSummary.TabIndex = 1;
            this.labelRuntimeSummary.Text = "状态未知";
            // 
            // selectPlcGlobal
            // 
            this.selectPlcGlobal.Location = new System.Drawing.Point(0, 4);
            this.selectPlcGlobal.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.selectPlcGlobal.Name = "selectPlcGlobal";
            this.selectPlcGlobal.Size = new System.Drawing.Size(220, 32);
            this.selectPlcGlobal.TabIndex = 0;
            this.selectPlcGlobal.WaveSize = 0;
            // 
            // flowToolbarRight
            // 
            this.flowToolbarRight.Align = AntdUI.TAlignFlow.RightCenter;
            this.flowToolbarRight.Controls.Add(this.buttonRefresh);
            this.flowToolbarRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowToolbarRight.Gap = 8;
            this.flowToolbarRight.Location = new System.Drawing.Point(928, 4);
            this.flowToolbarRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowToolbarRight.Name = "flowToolbarRight";
            this.flowToolbarRight.Size = new System.Drawing.Size(152, 36);
            this.flowToolbarRight.TabIndex = 1;
            this.flowToolbarRight.Text = "flowToolbarRight";
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.IconSvg = "ReloadOutlined";
            this.buttonRefresh.Location = new System.Drawing.Point(56, 0);
            this.buttonRefresh.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Radius = 8;
            this.buttonRefresh.Size = new System.Drawing.Size(96, 36);
            this.buttonRefresh.TabIndex = 0;
            this.buttonRefresh.Text = "刷新";
            this.buttonRefresh.WaveSize = 0;
            // 
            // PlcDebugPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "PlcDebugPage";
            this.Size = new System.Drawing.Size(1100, 680);
            this.panelRoot.ResumeLayout(false);
            this.panelContentCard.ResumeLayout(false);
            this.gridContent.ResumeLayout(false);
            this.panelHistoryCard.ResumeLayout(false);
            this.panelHistoryHeader.ResumeLayout(false);
            this.flowHistoryHeaderLeft.ResumeLayout(false);
            this.panelDebugOpsCard.ResumeLayout(false);
            this.panelOpsScroll.ResumeLayout(false);
            this.panelDirectDebugCard.ResumeLayout(false);
            this.flowDirectBottomRight.ResumeLayout(false);
            this.panelPointDebugCard.ResumeLayout(false);
            this.flowPointBottomRight.ResumeLayout(false);
            this.panelToolbar.ResumeLayout(false);
            this.flowToolbarLeft.ResumeLayout(false);
            this.flowToolbarRight.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.Panel panelRoot;
        private AntdUI.Panel panelContentCard;
        private AntdUI.GridPanel gridContent;
        private AntdUI.Panel panelHistoryCard;
        private AntdUI.Table tableResults;
        private AntdUI.Panel panelHistoryHeader;
        private AntdUI.FlowPanel flowHistoryHeaderLeft;
        private AntdUI.Label labelHistorySummary;
        private AntdUI.Label labelHistoryTitle;
        private AntdUI.Panel panelDebugOpsCard;
        private System.Windows.Forms.Panel panelOpsScroll;
        private AntdUI.Panel panelDirectDebugCard;
        private AntdUI.FlowPanel flowDirectBottomRight;
        private AntdUI.Button buttonWriteAddress;
        private AntdUI.Button buttonTestReadAddress;
        private AntdUI.Checkbox checkDirectWriteConfirmed;
        private AntdUI.Input inputDirectWriteValue;
        private AntdUI.Label labelDirectWriteValue;
        private AntdUI.Input inputDirectLength;
        private AntdUI.Label labelDirectLength;
        private AntdUI.Select selectDirectDataType;
        private AntdUI.Label labelDirectDataType;
        private AntdUI.Input inputDirectAddress;
        private AntdUI.Label labelDirectAddress;
        private AntdUI.Label labelDirectWriteTitle;
        private AntdUI.Panel panelPointDebugCard;
        private AntdUI.FlowPanel flowPointBottomRight;
        private AntdUI.Button buttonWritePoint;
        private AntdUI.Button buttonTestReadPoint;
        private AntdUI.Input inputPointWriteValue;
        private AntdUI.Label labelPointWriteValue;
        private AntdUI.Input inputPointLength;
        private AntdUI.Label labelPointLength;
        private AntdUI.Input inputPointDataType;
        private AntdUI.Label labelPointDataType;
        private AntdUI.Input inputPointAddress;
        private AntdUI.Label labelPointAddress;
        private AntdUI.Select selectPoint;
        private AntdUI.Label labelPointSelect;
        private AntdUI.Label labelPointOpTitle;
        private AntdUI.Panel panelSpacer;
        private AntdUI.Panel panelToolbar;
        private AntdUI.FlowPanel flowToolbarLeft;
        private AntdUI.Select selectPlcGlobal;
        private AntdUI.Label labelRuntimeSummary;
        private AntdUI.FlowPanel flowToolbarRight;
        private AntdUI.Button buttonRefresh;
    }
}