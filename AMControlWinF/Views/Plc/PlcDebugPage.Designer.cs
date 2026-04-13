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
            this.panelResultCard = new AntdUI.Panel();
            this.panelResultBody = new System.Windows.Forms.Panel();
            this.labelResultMessageValue = new AntdUI.Label();
            this.labelResultMessageTitle = new AntdUI.Label();
            this.panelResultTime = new AntdUI.Panel();
            this.labelResultTimeValue = new AntdUI.Label();
            this.labelResultTimeKey = new AntdUI.Label();
            this.panelResultQuality = new AntdUI.Panel();
            this.labelResultQualityValue = new AntdUI.Label();
            this.labelResultQualityKey = new AntdUI.Label();
            this.panelResultValue = new AntdUI.Panel();
            this.labelResultValueValue = new AntdUI.Label();
            this.labelResultValueKey = new AntdUI.Label();
            this.panelResultType = new AntdUI.Panel();
            this.labelResultTypeValue = new AntdUI.Label();
            this.labelResultTypeKey = new AntdUI.Label();
            this.panelResultTarget = new AntdUI.Panel();
            this.labelResultTargetValue = new AntdUI.Label();
            this.labelResultTargetKey = new AntdUI.Label();
            this.panelResultAction = new AntdUI.Panel();
            this.labelResultActionValue = new AntdUI.Label();
            this.labelResultActionKey = new AntdUI.Label();
            this.labelResultTitle = new AntdUI.Label();
            this.panelOpsCard = new AntdUI.Panel();
            this.panelOpsScroll = new System.Windows.Forms.Panel();
            this.panelDirectWriteCard = new AntdUI.Panel();
            this.checkDirectWriteConfirmed = new AntdUI.Checkbox();
            this.buttonWriteAddress = new AntdUI.Button();
            this.inputDirectWriteValue = new AntdUI.Input();
            this.labelDirectWriteValue = new AntdUI.Label();
            this.inputDirectLength = new AntdUI.Input();
            this.labelDirectLength = new AntdUI.Label();
            this.selectDirectDataType = new AntdUI.Select();
            this.labelDirectDataType = new AntdUI.Label();
            this.inputDirectAddress = new AntdUI.Input();
            this.labelDirectAddress = new AntdUI.Label();
            this.labelDirectWriteTitle = new AntdUI.Label();
            this.panelDirectReadCard = new AntdUI.Panel();
            this.buttonTestReadAddress = new AntdUI.Button();
            this.labelDirectReadTip = new AntdUI.Label();
            this.labelDirectReadTitle = new AntdUI.Label();
            this.panelPointOpCard = new AntdUI.Panel();
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
            this.panelToolbar = new AntdUI.Panel();
            this.flowToolbarLeft = new AntdUI.FlowPanel();
            this.labelRuntimeSummary = new AntdUI.Label();
            this.selectPlcGlobal = new AntdUI.Select();
            this.flowToolbarRight = new AntdUI.FlowPanel();
            this.buttonRefresh = new AntdUI.Button();
            this.panelRoot.SuspendLayout();
            this.panelContentCard.SuspendLayout();
            this.gridContent.SuspendLayout();
            this.panelResultCard.SuspendLayout();
            this.panelResultBody.SuspendLayout();
            this.panelResultTime.SuspendLayout();
            this.panelResultQuality.SuspendLayout();
            this.panelResultValue.SuspendLayout();
            this.panelResultType.SuspendLayout();
            this.panelResultTarget.SuspendLayout();
            this.panelResultAction.SuspendLayout();
            this.panelOpsCard.SuspendLayout();
            this.panelOpsScroll.SuspendLayout();
            this.panelDirectWriteCard.SuspendLayout();
            this.panelDirectReadCard.SuspendLayout();
            this.panelPointOpCard.SuspendLayout();
            this.panelToolbar.SuspendLayout();
            this.flowToolbarLeft.SuspendLayout();
            this.flowToolbarRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.panelContentCard);
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
            this.panelContentCard.Location = new System.Drawing.Point(8, 52);
            this.panelContentCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelContentCard.Name = "panelContentCard";
            this.panelContentCard.Radius = 0;
            this.panelContentCard.ShadowOpacity = 0F;
            this.panelContentCard.ShadowOpacityHover = 0F;
            this.panelContentCard.Size = new System.Drawing.Size(1084, 620);
            this.panelContentCard.TabIndex = 1;
            // 
            // gridContent
            // 
            this.gridContent.Controls.Add(this.panelResultCard);
            this.gridContent.Controls.Add(this.panelOpsCard);
            this.gridContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridContent.Location = new System.Drawing.Point(0, 0);
            this.gridContent.Margin = new System.Windows.Forms.Padding(0);
            this.gridContent.Name = "gridContent";
            this.gridContent.Size = new System.Drawing.Size(1084, 620);
            this.gridContent.Span = "58% 42%";
            this.gridContent.TabIndex = 0;
            // 
            // panelResultCard
            // 
            this.panelResultCard.BackColor = System.Drawing.Color.Transparent;
            this.panelResultCard.Controls.Add(this.panelResultBody);
            this.panelResultCard.Controls.Add(this.labelResultTitle);
            this.panelResultCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelResultCard.Location = new System.Drawing.Point(628, 0);
            this.panelResultCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelResultCard.Name = "panelResultCard";
            this.panelResultCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelResultCard.Radius = 12;
            this.panelResultCard.Shadow = 4;
            this.panelResultCard.ShadowOpacity = 0.15F;
            this.panelResultCard.Size = new System.Drawing.Size(456, 620);
            this.panelResultCard.TabIndex = 1;
            // 
            // panelResultBody
            // 
            this.panelResultBody.Controls.Add(this.labelResultMessageValue);
            this.panelResultBody.Controls.Add(this.labelResultMessageTitle);
            this.panelResultBody.Controls.Add(this.panelResultTime);
            this.panelResultBody.Controls.Add(this.panelResultQuality);
            this.panelResultBody.Controls.Add(this.panelResultValue);
            this.panelResultBody.Controls.Add(this.panelResultType);
            this.panelResultBody.Controls.Add(this.panelResultTarget);
            this.panelResultBody.Controls.Add(this.panelResultAction);
            this.panelResultBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelResultBody.Location = new System.Drawing.Point(16, 52);
            this.panelResultBody.Margin = new System.Windows.Forms.Padding(0);
            this.panelResultBody.Name = "panelResultBody";
            this.panelResultBody.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.panelResultBody.Size = new System.Drawing.Size(424, 552);
            this.panelResultBody.TabIndex = 1;
            // 
            // labelResultMessageValue
            // 
            this.labelResultMessageValue.Location = new System.Drawing.Point(8, 188);
            this.labelResultMessageValue.Name = "labelResultMessageValue";
            this.labelResultMessageValue.Size = new System.Drawing.Size(408, 180);
            this.labelResultMessageValue.TabIndex = 7;
            this.labelResultMessageValue.Text = "—";
            this.labelResultMessageValue.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            // 
            // labelResultMessageTitle
            // 
            this.labelResultMessageTitle.ForeColor = System.Drawing.Color.Gray;
            this.labelResultMessageTitle.Location = new System.Drawing.Point(8, 164);
            this.labelResultMessageTitle.Name = "labelResultMessageTitle";
            this.labelResultMessageTitle.Size = new System.Drawing.Size(408, 20);
            this.labelResultMessageTitle.TabIndex = 6;
            this.labelResultMessageTitle.Text = "结果消息";
            // 
            // panelResultTime
            // 
            this.panelResultTime.Controls.Add(this.labelResultTimeValue);
            this.panelResultTime.Controls.Add(this.labelResultTimeKey);
            this.panelResultTime.Location = new System.Drawing.Point(0, 132);
            this.panelResultTime.Margin = new System.Windows.Forms.Padding(0);
            this.panelResultTime.Name = "panelResultTime";
            this.panelResultTime.Radius = 0;
            this.panelResultTime.Size = new System.Drawing.Size(424, 24);
            this.panelResultTime.TabIndex = 5;
            // 
            // labelResultTimeValue
            // 
            this.labelResultTimeValue.Location = new System.Drawing.Point(88, 0);
            this.labelResultTimeValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelResultTimeValue.Name = "labelResultTimeValue";
            this.labelResultTimeValue.Size = new System.Drawing.Size(328, 24);
            this.labelResultTimeValue.TabIndex = 1;
            this.labelResultTimeValue.Text = "—";
            // 
            // labelResultTimeKey
            // 
            this.labelResultTimeKey.ForeColor = System.Drawing.Color.Gray;
            this.labelResultTimeKey.Location = new System.Drawing.Point(0, 0);
            this.labelResultTimeKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelResultTimeKey.Name = "labelResultTimeKey";
            this.labelResultTimeKey.Size = new System.Drawing.Size(80, 24);
            this.labelResultTimeKey.TabIndex = 0;
            this.labelResultTimeKey.Text = "执行时间";
            // 
            // panelResultQuality
            // 
            this.panelResultQuality.Controls.Add(this.labelResultQualityValue);
            this.panelResultQuality.Controls.Add(this.labelResultQualityKey);
            this.panelResultQuality.Location = new System.Drawing.Point(0, 104);
            this.panelResultQuality.Margin = new System.Windows.Forms.Padding(0);
            this.panelResultQuality.Name = "panelResultQuality";
            this.panelResultQuality.Radius = 0;
            this.panelResultQuality.Size = new System.Drawing.Size(424, 24);
            this.panelResultQuality.TabIndex = 4;
            // 
            // labelResultQualityValue
            // 
            this.labelResultQualityValue.Location = new System.Drawing.Point(88, 0);
            this.labelResultQualityValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelResultQualityValue.Name = "labelResultQualityValue";
            this.labelResultQualityValue.Size = new System.Drawing.Size(328, 24);
            this.labelResultQualityValue.TabIndex = 1;
            this.labelResultQualityValue.Text = "—";
            // 
            // labelResultQualityKey
            // 
            this.labelResultQualityKey.ForeColor = System.Drawing.Color.Gray;
            this.labelResultQualityKey.Location = new System.Drawing.Point(0, 0);
            this.labelResultQualityKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelResultQualityKey.Name = "labelResultQualityKey";
            this.labelResultQualityKey.Size = new System.Drawing.Size(80, 24);
            this.labelResultQualityKey.TabIndex = 0;
            this.labelResultQualityKey.Text = "质量";
            // 
            // panelResultValue
            // 
            this.panelResultValue.Controls.Add(this.labelResultValueValue);
            this.panelResultValue.Controls.Add(this.labelResultValueKey);
            this.panelResultValue.Location = new System.Drawing.Point(0, 76);
            this.panelResultValue.Margin = new System.Windows.Forms.Padding(0);
            this.panelResultValue.Name = "panelResultValue";
            this.panelResultValue.Radius = 0;
            this.panelResultValue.Size = new System.Drawing.Size(424, 24);
            this.panelResultValue.TabIndex = 3;
            // 
            // labelResultValueValue
            // 
            this.labelResultValueValue.Location = new System.Drawing.Point(88, 0);
            this.labelResultValueValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelResultValueValue.Name = "labelResultValueValue";
            this.labelResultValueValue.Size = new System.Drawing.Size(328, 24);
            this.labelResultValueValue.TabIndex = 1;
            this.labelResultValueValue.Text = "—";
            // 
            // labelResultValueKey
            // 
            this.labelResultValueKey.ForeColor = System.Drawing.Color.Gray;
            this.labelResultValueKey.Location = new System.Drawing.Point(0, 0);
            this.labelResultValueKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelResultValueKey.Name = "labelResultValueKey";
            this.labelResultValueKey.Size = new System.Drawing.Size(80, 24);
            this.labelResultValueKey.TabIndex = 0;
            this.labelResultValueKey.Text = "值";
            // 
            // panelResultType
            // 
            this.panelResultType.Controls.Add(this.labelResultTypeValue);
            this.panelResultType.Controls.Add(this.labelResultTypeKey);
            this.panelResultType.Location = new System.Drawing.Point(0, 48);
            this.panelResultType.Margin = new System.Windows.Forms.Padding(0);
            this.panelResultType.Name = "panelResultType";
            this.panelResultType.Radius = 0;
            this.panelResultType.Size = new System.Drawing.Size(424, 24);
            this.panelResultType.TabIndex = 2;
            // 
            // labelResultTypeValue
            // 
            this.labelResultTypeValue.Location = new System.Drawing.Point(88, 0);
            this.labelResultTypeValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelResultTypeValue.Name = "labelResultTypeValue";
            this.labelResultTypeValue.Size = new System.Drawing.Size(328, 24);
            this.labelResultTypeValue.TabIndex = 1;
            this.labelResultTypeValue.Text = "—";
            // 
            // labelResultTypeKey
            // 
            this.labelResultTypeKey.ForeColor = System.Drawing.Color.Gray;
            this.labelResultTypeKey.Location = new System.Drawing.Point(0, 0);
            this.labelResultTypeKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelResultTypeKey.Name = "labelResultTypeKey";
            this.labelResultTypeKey.Size = new System.Drawing.Size(80, 24);
            this.labelResultTypeKey.TabIndex = 0;
            this.labelResultTypeKey.Text = "数据类型";
            // 
            // panelResultTarget
            // 
            this.panelResultTarget.Controls.Add(this.labelResultTargetValue);
            this.panelResultTarget.Controls.Add(this.labelResultTargetKey);
            this.panelResultTarget.Location = new System.Drawing.Point(0, 20);
            this.panelResultTarget.Margin = new System.Windows.Forms.Padding(0);
            this.panelResultTarget.Name = "panelResultTarget";
            this.panelResultTarget.Radius = 0;
            this.panelResultTarget.Size = new System.Drawing.Size(424, 24);
            this.panelResultTarget.TabIndex = 1;
            // 
            // labelResultTargetValue
            // 
            this.labelResultTargetValue.Location = new System.Drawing.Point(88, 0);
            this.labelResultTargetValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelResultTargetValue.Name = "labelResultTargetValue";
            this.labelResultTargetValue.Size = new System.Drawing.Size(328, 24);
            this.labelResultTargetValue.TabIndex = 1;
            this.labelResultTargetValue.Text = "—";
            // 
            // labelResultTargetKey
            // 
            this.labelResultTargetKey.ForeColor = System.Drawing.Color.Gray;
            this.labelResultTargetKey.Location = new System.Drawing.Point(0, 0);
            this.labelResultTargetKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelResultTargetKey.Name = "labelResultTargetKey";
            this.labelResultTargetKey.Size = new System.Drawing.Size(80, 24);
            this.labelResultTargetKey.TabIndex = 0;
            this.labelResultTargetKey.Text = "目标";
            // 
            // panelResultAction
            // 
            this.panelResultAction.Controls.Add(this.labelResultActionValue);
            this.panelResultAction.Controls.Add(this.labelResultActionKey);
            this.panelResultAction.Location = new System.Drawing.Point(0, -8);
            this.panelResultAction.Margin = new System.Windows.Forms.Padding(0);
            this.panelResultAction.Name = "panelResultAction";
            this.panelResultAction.Radius = 0;
            this.panelResultAction.Size = new System.Drawing.Size(424, 24);
            this.panelResultAction.TabIndex = 0;
            // 
            // labelResultActionValue
            // 
            this.labelResultActionValue.Location = new System.Drawing.Point(88, 0);
            this.labelResultActionValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelResultActionValue.Name = "labelResultActionValue";
            this.labelResultActionValue.Size = new System.Drawing.Size(328, 24);
            this.labelResultActionValue.TabIndex = 1;
            this.labelResultActionValue.Text = "—";
            // 
            // labelResultActionKey
            // 
            this.labelResultActionKey.ForeColor = System.Drawing.Color.Gray;
            this.labelResultActionKey.Location = new System.Drawing.Point(0, 0);
            this.labelResultActionKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelResultActionKey.Name = "labelResultActionKey";
            this.labelResultActionKey.Size = new System.Drawing.Size(80, 24);
            this.labelResultActionKey.TabIndex = 0;
            this.labelResultActionKey.Text = "操作";
            // 
            // labelResultTitle
            // 
            this.labelResultTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelResultTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.labelResultTitle.Location = new System.Drawing.Point(16, 16);
            this.labelResultTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelResultTitle.Name = "labelResultTitle";
            this.labelResultTitle.Size = new System.Drawing.Size(424, 36);
            this.labelResultTitle.TabIndex = 0;
            this.labelResultTitle.Text = "执行结果";
            this.labelResultTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelOpsCard
            // 
            this.panelOpsCard.BackColor = System.Drawing.Color.Transparent;
            this.panelOpsCard.Controls.Add(this.panelOpsScroll);
            this.panelOpsCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOpsCard.Location = new System.Drawing.Point(0, 0);
            this.panelOpsCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelOpsCard.Name = "panelOpsCard";
            this.panelOpsCard.Radius = 0;
            this.panelOpsCard.ShadowOpacity = 0F;
            this.panelOpsCard.ShadowOpacityHover = 0F;
            this.panelOpsCard.Size = new System.Drawing.Size(628, 620);
            this.panelOpsCard.TabIndex = 0;
            // 
            // panelOpsScroll
            // 
            this.panelOpsScroll.AutoScroll = true;
            this.panelOpsScroll.Controls.Add(this.panelDirectWriteCard);
            this.panelOpsScroll.Controls.Add(this.panelDirectReadCard);
            this.panelOpsScroll.Controls.Add(this.panelPointOpCard);
            this.panelOpsScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOpsScroll.Location = new System.Drawing.Point(0, 0);
            this.panelOpsScroll.Margin = new System.Windows.Forms.Padding(0);
            this.panelOpsScroll.Name = "panelOpsScroll";
            this.panelOpsScroll.Padding = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.panelOpsScroll.Size = new System.Drawing.Size(628, 620);
            this.panelOpsScroll.TabIndex = 0;
            // 
            // panelDirectWriteCard
            // 
            this.panelDirectWriteCard.Controls.Add(this.checkDirectWriteConfirmed);
            this.panelDirectWriteCard.Controls.Add(this.buttonWriteAddress);
            this.panelDirectWriteCard.Controls.Add(this.inputDirectWriteValue);
            this.panelDirectWriteCard.Controls.Add(this.labelDirectWriteValue);
            this.panelDirectWriteCard.Controls.Add(this.inputDirectLength);
            this.panelDirectWriteCard.Controls.Add(this.labelDirectLength);
            this.panelDirectWriteCard.Controls.Add(this.selectDirectDataType);
            this.panelDirectWriteCard.Controls.Add(this.labelDirectDataType);
            this.panelDirectWriteCard.Controls.Add(this.inputDirectAddress);
            this.panelDirectWriteCard.Controls.Add(this.labelDirectAddress);
            this.panelDirectWriteCard.Controls.Add(this.labelDirectWriteTitle);
            this.panelDirectWriteCard.Location = new System.Drawing.Point(0, 376);
            this.panelDirectWriteCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelDirectWriteCard.Name = "panelDirectWriteCard";
            this.panelDirectWriteCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelDirectWriteCard.Radius = 12;
            this.panelDirectWriteCard.Shadow = 4;
            this.panelDirectWriteCard.ShadowOpacity = 0.15F;
            this.panelDirectWriteCard.Size = new System.Drawing.Size(600, 236);
            this.panelDirectWriteCard.TabIndex = 2;
            // 
            // checkDirectWriteConfirmed
            // 
            this.checkDirectWriteConfirmed.Location = new System.Drawing.Point(16, 184);
            this.checkDirectWriteConfirmed.Margin = new System.Windows.Forms.Padding(0);
            this.checkDirectWriteConfirmed.Name = "checkDirectWriteConfirmed";
            this.checkDirectWriteConfirmed.Size = new System.Drawing.Size(220, 24);
            this.checkDirectWriteConfirmed.TabIndex = 9;
            this.checkDirectWriteConfirmed.Text = "已确认这是高风险写入操作";
            // 
            // buttonWriteAddress
            // 
            this.buttonWriteAddress.IconSvg = "EditOutlined";
            this.buttonWriteAddress.Location = new System.Drawing.Point(488, 176);
            this.buttonWriteAddress.Margin = new System.Windows.Forms.Padding(0);
            this.buttonWriteAddress.Name = "buttonWriteAddress";
            this.buttonWriteAddress.Radius = 8;
            this.buttonWriteAddress.Size = new System.Drawing.Size(96, 36);
            this.buttonWriteAddress.TabIndex = 10;
            this.buttonWriteAddress.Text = "执行写入";
            this.buttonWriteAddress.Type = AntdUI.TTypeMini.Primary;
            this.buttonWriteAddress.WaveSize = 0;
            // 
            // inputDirectWriteValue
            // 
            this.inputDirectWriteValue.Location = new System.Drawing.Point(96, 136);
            this.inputDirectWriteValue.Margin = new System.Windows.Forms.Padding(0);
            this.inputDirectWriteValue.Name = "inputDirectWriteValue";
            this.inputDirectWriteValue.Size = new System.Drawing.Size(488, 32);
            this.inputDirectWriteValue.TabIndex = 8;
            this.inputDirectWriteValue.WaveSize = 0;
            // 
            // labelDirectWriteValue
            // 
            this.labelDirectWriteValue.ForeColor = System.Drawing.Color.Gray;
            this.labelDirectWriteValue.Location = new System.Drawing.Point(16, 136);
            this.labelDirectWriteValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelDirectWriteValue.Name = "labelDirectWriteValue";
            this.labelDirectWriteValue.Size = new System.Drawing.Size(72, 32);
            this.labelDirectWriteValue.TabIndex = 7;
            this.labelDirectWriteValue.Text = "写入值";
            this.labelDirectWriteValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // inputDirectLength
            // 
            this.inputDirectLength.Location = new System.Drawing.Point(424, 88);
            this.inputDirectLength.Margin = new System.Windows.Forms.Padding(0);
            this.inputDirectLength.Name = "inputDirectLength";
            this.inputDirectLength.Size = new System.Drawing.Size(160, 32);
            this.inputDirectLength.TabIndex = 6;
            this.inputDirectLength.WaveSize = 0;
            // 
            // labelDirectLength
            // 
            this.labelDirectLength.ForeColor = System.Drawing.Color.Gray;
            this.labelDirectLength.Location = new System.Drawing.Point(360, 88);
            this.labelDirectLength.Margin = new System.Windows.Forms.Padding(0);
            this.labelDirectLength.Name = "labelDirectLength";
            this.labelDirectLength.Size = new System.Drawing.Size(56, 32);
            this.labelDirectLength.TabIndex = 5;
            this.labelDirectLength.Text = "长度";
            this.labelDirectLength.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // selectDirectDataType
            // 
            this.selectDirectDataType.Location = new System.Drawing.Point(96, 88);
            this.selectDirectDataType.Margin = new System.Windows.Forms.Padding(0);
            this.selectDirectDataType.Name = "selectDirectDataType";
            this.selectDirectDataType.Size = new System.Drawing.Size(240, 32);
            this.selectDirectDataType.TabIndex = 4;
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
            this.labelDirectDataType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // inputDirectAddress
            // 
            this.inputDirectAddress.Location = new System.Drawing.Point(96, 40);
            this.inputDirectAddress.Margin = new System.Windows.Forms.Padding(0);
            this.inputDirectAddress.Name = "inputDirectAddress";
            this.inputDirectAddress.Size = new System.Drawing.Size(488, 32);
            this.inputDirectAddress.TabIndex = 2;
            this.inputDirectAddress.WaveSize = 0;
            // 
            // labelDirectAddress
            // 
            this.labelDirectAddress.ForeColor = System.Drawing.Color.Gray;
            this.labelDirectAddress.Location = new System.Drawing.Point(16, 40);
            this.labelDirectAddress.Margin = new System.Windows.Forms.Padding(0);
            this.labelDirectAddress.Name = "labelDirectAddress";
            this.labelDirectAddress.Size = new System.Drawing.Size(72, 32);
            this.labelDirectAddress.TabIndex = 1;
            this.labelDirectAddress.Text = "地址";
            this.labelDirectAddress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelDirectWriteTitle
            // 
            this.labelDirectWriteTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelDirectWriteTitle.Location = new System.Drawing.Point(16, 8);
            this.labelDirectWriteTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelDirectWriteTitle.Name = "labelDirectWriteTitle";
            this.labelDirectWriteTitle.Size = new System.Drawing.Size(200, 24);
            this.labelDirectWriteTitle.TabIndex = 0;
            this.labelDirectWriteTitle.Text = "按直接地址写入";
            // 
            // panelDirectReadCard
            // 
            this.panelDirectReadCard.Controls.Add(this.buttonTestReadAddress);
            this.panelDirectReadCard.Controls.Add(this.labelDirectReadTip);
            this.panelDirectReadCard.Controls.Add(this.labelDirectReadTitle);
            this.panelDirectReadCard.Location = new System.Drawing.Point(0, 276);
            this.panelDirectReadCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelDirectReadCard.Name = "panelDirectReadCard";
            this.panelDirectReadCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelDirectReadCard.Radius = 12;
            this.panelDirectReadCard.Shadow = 4;
            this.panelDirectReadCard.ShadowOpacity = 0.15F;
            this.panelDirectReadCard.Size = new System.Drawing.Size(600, 84);
            this.panelDirectReadCard.TabIndex = 1;
            // 
            // buttonTestReadAddress
            // 
            this.buttonTestReadAddress.IconSvg = "SearchOutlined";
            this.buttonTestReadAddress.Location = new System.Drawing.Point(488, 24);
            this.buttonTestReadAddress.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTestReadAddress.Name = "buttonTestReadAddress";
            this.buttonTestReadAddress.Radius = 8;
            this.buttonTestReadAddress.Size = new System.Drawing.Size(96, 36);
            this.buttonTestReadAddress.TabIndex = 2;
            this.buttonTestReadAddress.Text = "测试读取";
            this.buttonTestReadAddress.WaveSize = 0;
            // 
            // labelDirectReadTip
            // 
            this.labelDirectReadTip.ForeColor = System.Drawing.Color.Gray;
            this.labelDirectReadTip.Location = new System.Drawing.Point(16, 44);
            this.labelDirectReadTip.Margin = new System.Windows.Forms.Padding(0);
            this.labelDirectReadTip.Name = "labelDirectReadTip";
            this.labelDirectReadTip.Size = new System.Drawing.Size(320, 20);
            this.labelDirectReadTip.TabIndex = 1;
            this.labelDirectReadTip.Text = "复用下方地址、数据类型、长度输入项。";
            // 
            // labelDirectReadTitle
            // 
            this.labelDirectReadTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelDirectReadTitle.Location = new System.Drawing.Point(16, 16);
            this.labelDirectReadTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelDirectReadTitle.Name = "labelDirectReadTitle";
            this.labelDirectReadTitle.Size = new System.Drawing.Size(200, 24);
            this.labelDirectReadTitle.TabIndex = 0;
            this.labelDirectReadTitle.Text = "按直接地址读取";
            // 
            // panelPointOpCard
            // 
            this.panelPointOpCard.Controls.Add(this.buttonWritePoint);
            this.panelPointOpCard.Controls.Add(this.buttonTestReadPoint);
            this.panelPointOpCard.Controls.Add(this.inputPointWriteValue);
            this.panelPointOpCard.Controls.Add(this.labelPointWriteValue);
            this.panelPointOpCard.Controls.Add(this.inputPointLength);
            this.panelPointOpCard.Controls.Add(this.labelPointLength);
            this.panelPointOpCard.Controls.Add(this.inputPointDataType);
            this.panelPointOpCard.Controls.Add(this.labelPointDataType);
            this.panelPointOpCard.Controls.Add(this.inputPointAddress);
            this.panelPointOpCard.Controls.Add(this.labelPointAddress);
            this.panelPointOpCard.Controls.Add(this.selectPoint);
            this.panelPointOpCard.Controls.Add(this.labelPointSelect);
            this.panelPointOpCard.Controls.Add(this.labelPointOpTitle);
            this.panelPointOpCard.Location = new System.Drawing.Point(0, 0);
            this.panelPointOpCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelPointOpCard.Name = "panelPointOpCard";
            this.panelPointOpCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelPointOpCard.Radius = 12;
            this.panelPointOpCard.Shadow = 4;
            this.panelPointOpCard.ShadowOpacity = 0.15F;
            this.panelPointOpCard.Size = new System.Drawing.Size(600, 260);
            this.panelPointOpCard.TabIndex = 0;
            // 
            // buttonWritePoint
            // 
            this.buttonWritePoint.IconSvg = "EditOutlined";
            this.buttonWritePoint.Location = new System.Drawing.Point(488, 208);
            this.buttonWritePoint.Margin = new System.Windows.Forms.Padding(0);
            this.buttonWritePoint.Name = "buttonWritePoint";
            this.buttonWritePoint.Radius = 8;
            this.buttonWritePoint.Size = new System.Drawing.Size(96, 36);
            this.buttonWritePoint.TabIndex = 12;
            this.buttonWritePoint.Text = "写入点位";
            this.buttonWritePoint.Type = AntdUI.TTypeMini.Primary;
            this.buttonWritePoint.WaveSize = 0;
            // 
            // buttonTestReadPoint
            // 
            this.buttonTestReadPoint.IconSvg = "SearchOutlined";
            this.buttonTestReadPoint.Location = new System.Drawing.Point(384, 208);
            this.buttonTestReadPoint.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTestReadPoint.Name = "buttonTestReadPoint";
            this.buttonTestReadPoint.Radius = 8;
            this.buttonTestReadPoint.Size = new System.Drawing.Size(96, 36);
            this.buttonTestReadPoint.TabIndex = 11;
            this.buttonTestReadPoint.Text = "测试读取";
            this.buttonTestReadPoint.WaveSize = 0;
            // 
            // inputPointWriteValue
            // 
            this.inputPointWriteValue.Location = new System.Drawing.Point(96, 160);
            this.inputPointWriteValue.Margin = new System.Windows.Forms.Padding(0);
            this.inputPointWriteValue.Name = "inputPointWriteValue";
            this.inputPointWriteValue.Size = new System.Drawing.Size(488, 32);
            this.inputPointWriteValue.TabIndex = 10;
            this.inputPointWriteValue.WaveSize = 0;
            // 
            // labelPointWriteValue
            // 
            this.labelPointWriteValue.ForeColor = System.Drawing.Color.Gray;
            this.labelPointWriteValue.Location = new System.Drawing.Point(16, 160);
            this.labelPointWriteValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelPointWriteValue.Name = "labelPointWriteValue";
            this.labelPointWriteValue.Size = new System.Drawing.Size(72, 32);
            this.labelPointWriteValue.TabIndex = 9;
            this.labelPointWriteValue.Text = "写入值";
            this.labelPointWriteValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // inputPointLength
            // 
            this.inputPointLength.Location = new System.Drawing.Point(424, 112);
            this.inputPointLength.Margin = new System.Windows.Forms.Padding(0);
            this.inputPointLength.Name = "inputPointLength";
            this.inputPointLength.Size = new System.Drawing.Size(160, 32);
            this.inputPointLength.TabIndex = 8;
            this.inputPointLength.WaveSize = 0;
            // 
            // labelPointLength
            // 
            this.labelPointLength.ForeColor = System.Drawing.Color.Gray;
            this.labelPointLength.Location = new System.Drawing.Point(360, 112);
            this.labelPointLength.Margin = new System.Windows.Forms.Padding(0);
            this.labelPointLength.Name = "labelPointLength";
            this.labelPointLength.Size = new System.Drawing.Size(56, 32);
            this.labelPointLength.TabIndex = 7;
            this.labelPointLength.Text = "长度";
            this.labelPointLength.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // inputPointDataType
            // 
            this.inputPointDataType.Location = new System.Drawing.Point(96, 112);
            this.inputPointDataType.Margin = new System.Windows.Forms.Padding(0);
            this.inputPointDataType.Name = "inputPointDataType";
            this.inputPointDataType.Size = new System.Drawing.Size(240, 32);
            this.inputPointDataType.TabIndex = 6;
            this.inputPointDataType.WaveSize = 0;
            // 
            // labelPointDataType
            // 
            this.labelPointDataType.ForeColor = System.Drawing.Color.Gray;
            this.labelPointDataType.Location = new System.Drawing.Point(16, 112);
            this.labelPointDataType.Margin = new System.Windows.Forms.Padding(0);
            this.labelPointDataType.Name = "labelPointDataType";
            this.labelPointDataType.Size = new System.Drawing.Size(72, 32);
            this.labelPointDataType.TabIndex = 5;
            this.labelPointDataType.Text = "数据类型";
            this.labelPointDataType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // inputPointAddress
            // 
            this.inputPointAddress.Location = new System.Drawing.Point(96, 64);
            this.inputPointAddress.Margin = new System.Windows.Forms.Padding(0);
            this.inputPointAddress.Name = "inputPointAddress";
            this.inputPointAddress.Size = new System.Drawing.Size(488, 32);
            this.inputPointAddress.TabIndex = 4;
            this.inputPointAddress.WaveSize = 0;
            // 
            // labelPointAddress
            // 
            this.labelPointAddress.ForeColor = System.Drawing.Color.Gray;
            this.labelPointAddress.Location = new System.Drawing.Point(16, 64);
            this.labelPointAddress.Margin = new System.Windows.Forms.Padding(0);
            this.labelPointAddress.Name = "labelPointAddress";
            this.labelPointAddress.Size = new System.Drawing.Size(72, 32);
            this.labelPointAddress.TabIndex = 3;
            this.labelPointAddress.Text = "地址";
            this.labelPointAddress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // selectPoint
            // 
            this.selectPoint.Location = new System.Drawing.Point(96, 24);
            this.selectPoint.Margin = new System.Windows.Forms.Padding(0);
            this.selectPoint.Name = "selectPoint";
            this.selectPoint.Size = new System.Drawing.Size(488, 32);
            this.selectPoint.TabIndex = 2;
            // 
            // labelPointSelect
            // 
            this.labelPointSelect.ForeColor = System.Drawing.Color.Gray;
            this.labelPointSelect.Location = new System.Drawing.Point(16, 24);
            this.labelPointSelect.Margin = new System.Windows.Forms.Padding(0);
            this.labelPointSelect.Name = "labelPointSelect";
            this.labelPointSelect.Size = new System.Drawing.Size(72, 32);
            this.labelPointSelect.TabIndex = 1;
            this.labelPointSelect.Text = "配置点位";
            this.labelPointSelect.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelPointOpTitle
            // 
            this.labelPointOpTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelPointOpTitle.Location = new System.Drawing.Point(16, -8);
            this.labelPointOpTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelPointOpTitle.Name = "labelPointOpTitle";
            this.labelPointOpTitle.Size = new System.Drawing.Size(200, 24);
            this.labelPointOpTitle.TabIndex = 0;
            this.labelPointOpTitle.Text = "按配置点位调试";
            // 
            // panelToolbar
            // 
            this.panelToolbar.Controls.Add(this.flowToolbarLeft);
            this.panelToolbar.Controls.Add(this.flowToolbarRight);
            this.panelToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelToolbar.Location = new System.Drawing.Point(8, 8);
            this.panelToolbar.Margin = new System.Windows.Forms.Padding(0);
            this.panelToolbar.Name = "panelToolbar";
            this.panelToolbar.Padding = new System.Windows.Forms.Padding(4, 4, 4, 0);
            this.panelToolbar.Radius = 0;
            this.panelToolbar.Size = new System.Drawing.Size(1084, 44);
            this.panelToolbar.TabIndex = 0;
            // 
            // flowToolbarLeft
            // 
            this.flowToolbarLeft.Controls.Add(this.labelRuntimeSummary);
            this.flowToolbarLeft.Controls.Add(this.selectPlcGlobal);
            this.flowToolbarLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowToolbarLeft.Gap = 8;
            this.flowToolbarLeft.Location = new System.Drawing.Point(4, 4);
            this.flowToolbarLeft.Margin = new System.Windows.Forms.Padding(0);
            this.flowToolbarLeft.Name = "flowToolbarLeft";
            this.flowToolbarLeft.Size = new System.Drawing.Size(924, 40);
            this.flowToolbarLeft.TabIndex = 0;
            this.flowToolbarLeft.Text = "flowToolbarLeft";
            // 
            // labelRuntimeSummary
            // 
            this.labelRuntimeSummary.Location = new System.Drawing.Point(4, 0);
            this.labelRuntimeSummary.Margin = new System.Windows.Forms.Padding(0);
            this.labelRuntimeSummary.Name = "labelRuntimeSummary";
            this.labelRuntimeSummary.Size = new System.Drawing.Size(200, 40);
            this.labelRuntimeSummary.TabIndex = 0;
            this.labelRuntimeSummary.Text = "状态未知";
            this.labelRuntimeSummary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // selectPlcGlobal
            // 
            this.selectPlcGlobal.Location = new System.Drawing.Point(212, 4);
            this.selectPlcGlobal.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.selectPlcGlobal.Name = "selectPlcGlobal";
            this.selectPlcGlobal.Size = new System.Drawing.Size(220, 32);
            this.selectPlcGlobal.TabIndex = 1;
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
            this.flowToolbarRight.Size = new System.Drawing.Size(152, 40);
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
            this.buttonRefresh.Type = AntdUI.TTypeMini.Primary;
            this.buttonRefresh.WaveSize = 0;
            // 
            // PlcDebugPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "PlcDebugPage";
            this.Size = new System.Drawing.Size(1100, 680);
            this.panelRoot.ResumeLayout(false);
            this.panelContentCard.ResumeLayout(false);
            this.gridContent.ResumeLayout(false);
            this.panelResultCard.ResumeLayout(false);
            this.panelResultBody.ResumeLayout(false);
            this.panelResultTime.ResumeLayout(false);
            this.panelResultQuality.ResumeLayout(false);
            this.panelResultValue.ResumeLayout(false);
            this.panelResultType.ResumeLayout(false);
            this.panelResultTarget.ResumeLayout(false);
            this.panelResultAction.ResumeLayout(false);
            this.panelOpsCard.ResumeLayout(false);
            this.panelOpsScroll.ResumeLayout(false);
            this.panelDirectWriteCard.ResumeLayout(false);
            this.panelDirectReadCard.ResumeLayout(false);
            this.panelPointOpCard.ResumeLayout(false);
            this.panelToolbar.ResumeLayout(false);
            this.flowToolbarLeft.ResumeLayout(false);
            this.flowToolbarRight.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private AntdUI.Panel panelRoot;
        private AntdUI.Panel panelContentCard;
        private AntdUI.GridPanel gridContent;
        private AntdUI.Panel panelResultCard;
        private System.Windows.Forms.Panel panelResultBody;
        private AntdUI.Label labelResultMessageValue;
        private AntdUI.Label labelResultMessageTitle;
        private AntdUI.Panel panelResultTime;
        private AntdUI.Label labelResultTimeValue;
        private AntdUI.Label labelResultTimeKey;
        private AntdUI.Panel panelResultQuality;
        private AntdUI.Label labelResultQualityValue;
        private AntdUI.Label labelResultQualityKey;
        private AntdUI.Panel panelResultValue;
        private AntdUI.Label labelResultValueValue;
        private AntdUI.Label labelResultValueKey;
        private AntdUI.Panel panelResultType;
        private AntdUI.Label labelResultTypeValue;
        private AntdUI.Label labelResultTypeKey;
        private AntdUI.Panel panelResultTarget;
        private AntdUI.Label labelResultTargetValue;
        private AntdUI.Label labelResultTargetKey;
        private AntdUI.Panel panelResultAction;
        private AntdUI.Label labelResultActionValue;
        private AntdUI.Label labelResultActionKey;
        private AntdUI.Label labelResultTitle;
        private AntdUI.Panel panelOpsCard;
        private System.Windows.Forms.Panel panelOpsScroll;
        private AntdUI.Panel panelDirectWriteCard;
        private AntdUI.Checkbox checkDirectWriteConfirmed;
        private AntdUI.Button buttonWriteAddress;
        private AntdUI.Input inputDirectWriteValue;
        private AntdUI.Label labelDirectWriteValue;
        private AntdUI.Input inputDirectLength;
        private AntdUI.Label labelDirectLength;
        private AntdUI.Select selectDirectDataType;
        private AntdUI.Label labelDirectDataType;
        private AntdUI.Input inputDirectAddress;
        private AntdUI.Label labelDirectAddress;
        private AntdUI.Label labelDirectWriteTitle;
        private AntdUI.Panel panelDirectReadCard;
        private AntdUI.Button buttonTestReadAddress;
        private AntdUI.Label labelDirectReadTip;
        private AntdUI.Label labelDirectReadTitle;
        private AntdUI.Panel panelPointOpCard;
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
        private AntdUI.Panel panelToolbar;
        private AntdUI.FlowPanel flowToolbarLeft;
        private AntdUI.Label labelRuntimeSummary;
        private AntdUI.Select selectPlcGlobal;
        private AntdUI.FlowPanel flowToolbarRight;
        private AntdUI.Button buttonRefresh;
    }
}