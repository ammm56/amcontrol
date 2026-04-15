namespace AMControlWinF.Views.Assembly
{
    partial class AssemblyWiringPage
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

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.panelRoot = new AntdUI.Panel();
            this.panelTableCard = new AntdUI.Panel();
            this.tableWiring = new AntdUI.Table();
            this.panelTableHeader = new AntdUI.Panel();
            this.labelTableSummary = new AntdUI.Label();
            this.labelTableTitle = new AntdUI.Label();
            this.panelDebugCard = new AntdUI.Panel();
            this.gridDebugLayout = new AntdUI.GridPanel();
            this.panelDebugActions = new AntdUI.Panel();
            this.panelDebugButtons = new AntdUI.Panel();
            this.buttonCancelVerified = new AntdUI.Button();
            this.buttonMarkVerified = new AntdUI.Button();
            this.buttonPulseDo = new AntdUI.Button();
            this.buttonDoOff = new AntdUI.Button();
            this.buttonDoOn = new AntdUI.Button();
            this.buttonReadDi = new AntdUI.Button();
            this.panelPulseRow = new AntdUI.Panel();
            this.inputPulseWidth = new AntdUI.Input();
            this.labelPulseWidth = new AntdUI.Label();
            this.panelDebugLeft = new AntdUI.Panel();
            this.panelDebugInfo = new AntdUI.Panel();
            this.labelSelectedRuntimeValue = new AntdUI.Label();
            this.labelSelectedRuntimeTitle = new AntdUI.Label();
            this.labelSelectedWiringValue = new AntdUI.Label();
            this.labelSelectedWiringTitle = new AntdUI.Label();
            this.labelSelectedHardwareValue = new AntdUI.Label();
            this.labelSelectedHardwareTitle = new AntdUI.Label();
            this.labelSelectedNameValue = new AntdUI.Label();
            this.labelSelectedNameTitle = new AntdUI.Label();
            this.panelDebugHeader = new AntdUI.Panel();
            this.labelDebugHint = new AntdUI.Label();
            this.labelDebugTitle = new AntdUI.Label();
            this.flowStats = new AntdUI.FlowPanel();
            this.panelUnverifiedCard = new AntdUI.Panel();
            this.labelUnverifiedCount = new AntdUI.Label();
            this.labelUnverifiedTitle = new AntdUI.Label();
            this.panelVerifiedCard = new AntdUI.Panel();
            this.labelVerifiedCount = new AntdUI.Label();
            this.labelVerifiedTitle = new AntdUI.Label();
            this.panelTotalCard = new AntdUI.Panel();
            this.labelTotalCount = new AntdUI.Label();
            this.labelTotalTitle = new AntdUI.Label();
            this.panelToolbar = new AntdUI.Panel();
            this.flowToolbarLeft = new AntdUI.FlowPanel();
            this.inputSearch = new AntdUI.Input();
            this.buttonFilterDO = new AntdUI.Button();
            this.buttonFilterDI = new AntdUI.Button();
            this.buttonFilterAll = new AntdUI.Button();
            this.labelSelectedCard = new AntdUI.Label();
            this.buttonSelectCard = new AntdUI.Button();
            this.flowToolbarRight = new AntdUI.FlowPanel();
            this.buttonExportCsv = new AntdUI.Button();
            this.buttonRefresh = new AntdUI.Button();
            this.checkboxOnlyUnverified = new AntdUI.Checkbox();
            this.panelRoot.SuspendLayout();
            this.panelTableCard.SuspendLayout();
            this.panelTableHeader.SuspendLayout();
            this.panelDebugCard.SuspendLayout();
            this.gridDebugLayout.SuspendLayout();
            this.panelDebugActions.SuspendLayout();
            this.panelDebugButtons.SuspendLayout();
            this.panelPulseRow.SuspendLayout();
            this.panelDebugLeft.SuspendLayout();
            this.panelDebugInfo.SuspendLayout();
            this.panelDebugHeader.SuspendLayout();
            this.flowStats.SuspendLayout();
            this.panelUnverifiedCard.SuspendLayout();
            this.panelVerifiedCard.SuspendLayout();
            this.panelTotalCard.SuspendLayout();
            this.panelToolbar.SuspendLayout();
            this.flowToolbarLeft.SuspendLayout();
            this.flowToolbarRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.panelTableCard);
            this.panelRoot.Controls.Add(this.panelDebugCard);
            this.panelRoot.Controls.Add(this.flowStats);
            this.panelRoot.Controls.Add(this.panelToolbar);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Margin = new System.Windows.Forms.Padding(0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(8);
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(1100, 720);
            this.panelRoot.TabIndex = 0;
            // 
            // panelTableCard
            // 
            this.panelTableCard.BackColor = System.Drawing.Color.Transparent;
            this.panelTableCard.Controls.Add(this.tableWiring);
            this.panelTableCard.Controls.Add(this.panelTableHeader);
            this.panelTableCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTableCard.Location = new System.Drawing.Point(8, 140);
            this.panelTableCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelTableCard.Name = "panelTableCard";
            this.panelTableCard.Padding = new System.Windows.Forms.Padding(8);
            this.panelTableCard.Radius = 12;
            this.panelTableCard.Shadow = 4;
            this.panelTableCard.ShadowOpacity = 0.15F;
            this.panelTableCard.Size = new System.Drawing.Size(1084, 384);
            this.panelTableCard.TabIndex = 2;
            // 
            // tableWiring
            // 
            this.tableWiring.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tableWiring.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableWiring.EmptyHeader = true;
            this.tableWiring.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.tableWiring.Gap = 8;
            this.tableWiring.Gaps = new System.Drawing.Size(8, 8);
            this.tableWiring.Location = new System.Drawing.Point(12, 56);
            this.tableWiring.Margin = new System.Windows.Forms.Padding(0);
            this.tableWiring.Name = "tableWiring";
            this.tableWiring.ShowTip = false;
            this.tableWiring.Size = new System.Drawing.Size(1060, 316);
            this.tableWiring.TabIndex = 1;
            this.tableWiring.Text = "tableWiring";
            // 
            // panelTableHeader
            // 
            this.panelTableHeader.Controls.Add(this.labelTableSummary);
            this.panelTableHeader.Controls.Add(this.labelTableTitle);
            this.panelTableHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTableHeader.Location = new System.Drawing.Point(12, 12);
            this.panelTableHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelTableHeader.Name = "panelTableHeader";
            this.panelTableHeader.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.panelTableHeader.Radius = 0;
            this.panelTableHeader.Size = new System.Drawing.Size(1060, 44);
            this.panelTableHeader.TabIndex = 0;
            // 
            // labelTableSummary
            // 
            this.labelTableSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTableSummary.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelTableSummary.Location = new System.Drawing.Point(104, 0);
            this.labelTableSummary.Margin = new System.Windows.Forms.Padding(0);
            this.labelTableSummary.Name = "labelTableSummary";
            this.labelTableSummary.Size = new System.Drawing.Size(956, 36);
            this.labelTableSummary.TabIndex = 1;
            this.labelTableSummary.Text = "共 0 项，已核对 0 项，未核对 0 项";
            // 
            // labelTableTitle
            // 
            this.labelTableTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelTableTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.labelTableTitle.Location = new System.Drawing.Point(0, 0);
            this.labelTableTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTableTitle.Name = "labelTableTitle";
            this.labelTableTitle.Size = new System.Drawing.Size(104, 36);
            this.labelTableTitle.TabIndex = 0;
            this.labelTableTitle.Text = "接线总表";
            // 
            // panelDebugCard
            // 
            this.panelDebugCard.BackColor = System.Drawing.Color.Transparent;
            this.panelDebugCard.Controls.Add(this.gridDebugLayout);
            this.panelDebugCard.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelDebugCard.Location = new System.Drawing.Point(8, 524);
            this.panelDebugCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelDebugCard.Name = "panelDebugCard";
            this.panelDebugCard.Radius = 12;
            this.panelDebugCard.Shadow = 4;
            this.panelDebugCard.ShadowOpacity = 0.15F;
            this.panelDebugCard.Size = new System.Drawing.Size(1084, 188);
            this.panelDebugCard.TabIndex = 3;
            // 
            // gridDebugLayout
            // 
            this.gridDebugLayout.Controls.Add(this.panelDebugActions);
            this.gridDebugLayout.Controls.Add(this.panelDebugLeft);
            this.gridDebugLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDebugLayout.Gap = 12;
            this.gridDebugLayout.Location = new System.Drawing.Point(4, 4);
            this.gridDebugLayout.Margin = new System.Windows.Forms.Padding(0);
            this.gridDebugLayout.Name = "gridDebugLayout";
            this.gridDebugLayout.Size = new System.Drawing.Size(1076, 180);
            this.gridDebugLayout.Span = "100% 300";
            this.gridDebugLayout.TabIndex = 0;
            this.gridDebugLayout.Text = "gridDebugLayout";
            // 
            // panelDebugActions
            // 
            this.panelDebugActions.Controls.Add(this.panelDebugButtons);
            this.panelDebugActions.Controls.Add(this.panelPulseRow);
            this.panelDebugActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDebugActions.Location = new System.Drawing.Point(788, 12);
            this.panelDebugActions.Margin = new System.Windows.Forms.Padding(0);
            this.panelDebugActions.Name = "panelDebugActions";
            this.panelDebugActions.Radius = 0;
            this.panelDebugActions.Size = new System.Drawing.Size(276, 156);
            this.panelDebugActions.TabIndex = 1;
            // 
            // panelDebugButtons
            // 
            this.panelDebugButtons.Controls.Add(this.buttonCancelVerified);
            this.panelDebugButtons.Controls.Add(this.buttonMarkVerified);
            this.panelDebugButtons.Controls.Add(this.buttonPulseDo);
            this.panelDebugButtons.Controls.Add(this.buttonDoOff);
            this.panelDebugButtons.Controls.Add(this.buttonDoOn);
            this.panelDebugButtons.Controls.Add(this.buttonReadDi);
            this.panelDebugButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDebugButtons.Location = new System.Drawing.Point(0, 36);
            this.panelDebugButtons.Margin = new System.Windows.Forms.Padding(0);
            this.panelDebugButtons.Name = "panelDebugButtons";
            this.panelDebugButtons.Radius = 0;
            this.panelDebugButtons.Size = new System.Drawing.Size(276, 120);
            this.panelDebugButtons.TabIndex = 1;
            // 
            // buttonCancelVerified
            // 
            this.buttonCancelVerified.IconSvg = "CloseCircleOutlined";
            this.buttonCancelVerified.Location = new System.Drawing.Point(145, 84);
            this.buttonCancelVerified.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCancelVerified.Name = "buttonCancelVerified";
            this.buttonCancelVerified.Radius = 8;
            this.buttonCancelVerified.Size = new System.Drawing.Size(120, 36);
            this.buttonCancelVerified.TabIndex = 5;
            this.buttonCancelVerified.Text = "取消核对";
            this.buttonCancelVerified.WaveSize = 0;
            // 
            // buttonMarkVerified
            // 
            this.buttonMarkVerified.IconSvg = "CheckCircleOutlined";
            this.buttonMarkVerified.Location = new System.Drawing.Point(15, 84);
            this.buttonMarkVerified.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMarkVerified.Name = "buttonMarkVerified";
            this.buttonMarkVerified.Radius = 8;
            this.buttonMarkVerified.Size = new System.Drawing.Size(120, 36);
            this.buttonMarkVerified.TabIndex = 4;
            this.buttonMarkVerified.Text = "标记已核对";
            this.buttonMarkVerified.Type = AntdUI.TTypeMini.Primary;
            this.buttonMarkVerified.WaveSize = 0;
            // 
            // buttonPulseDo
            // 
            this.buttonPulseDo.IconSvg = "ThunderboltOutlined";
            this.buttonPulseDo.Location = new System.Drawing.Point(145, 4);
            this.buttonPulseDo.Margin = new System.Windows.Forms.Padding(0);
            this.buttonPulseDo.Name = "buttonPulseDo";
            this.buttonPulseDo.Radius = 8;
            this.buttonPulseDo.Size = new System.Drawing.Size(120, 36);
            this.buttonPulseDo.TabIndex = 3;
            this.buttonPulseDo.Text = "脉冲输出";
            this.buttonPulseDo.WaveSize = 0;
            // 
            // buttonDoOff
            // 
            this.buttonDoOff.IconSvg = "PoweroffOutlined";
            this.buttonDoOff.Location = new System.Drawing.Point(145, 44);
            this.buttonDoOff.Margin = new System.Windows.Forms.Padding(0);
            this.buttonDoOff.Name = "buttonDoOff";
            this.buttonDoOff.Radius = 8;
            this.buttonDoOff.Size = new System.Drawing.Size(120, 36);
            this.buttonDoOff.TabIndex = 1;
            this.buttonDoOff.Text = "DO 关闭";
            this.buttonDoOff.WaveSize = 0;
            // 
            // buttonDoOn
            // 
            this.buttonDoOn.IconSvg = "PlayCircleOutlined";
            this.buttonDoOn.Location = new System.Drawing.Point(15, 44);
            this.buttonDoOn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonDoOn.Name = "buttonDoOn";
            this.buttonDoOn.Radius = 8;
            this.buttonDoOn.Size = new System.Drawing.Size(120, 36);
            this.buttonDoOn.TabIndex = 0;
            this.buttonDoOn.Text = "DO 打开";
            this.buttonDoOn.Type = AntdUI.TTypeMini.Primary;
            this.buttonDoOn.WaveSize = 0;
            // 
            // buttonReadDi
            // 
            this.buttonReadDi.IconSvg = "SearchOutlined";
            this.buttonReadDi.Location = new System.Drawing.Point(15, 4);
            this.buttonReadDi.Margin = new System.Windows.Forms.Padding(0);
            this.buttonReadDi.Name = "buttonReadDi";
            this.buttonReadDi.Radius = 8;
            this.buttonReadDi.Size = new System.Drawing.Size(120, 36);
            this.buttonReadDi.TabIndex = 0;
            this.buttonReadDi.Text = "读取 DI";
            this.buttonReadDi.WaveSize = 0;
            // 
            // panelPulseRow
            // 
            this.panelPulseRow.Controls.Add(this.inputPulseWidth);
            this.panelPulseRow.Controls.Add(this.labelPulseWidth);
            this.panelPulseRow.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPulseRow.Location = new System.Drawing.Point(0, 0);
            this.panelPulseRow.Margin = new System.Windows.Forms.Padding(0);
            this.panelPulseRow.Name = "panelPulseRow";
            this.panelPulseRow.Radius = 0;
            this.panelPulseRow.Size = new System.Drawing.Size(276, 36);
            this.panelPulseRow.TabIndex = 0;
            // 
            // inputPulseWidth
            // 
            this.inputPulseWidth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inputPulseWidth.Location = new System.Drawing.Point(92, 0);
            this.inputPulseWidth.Margin = new System.Windows.Forms.Padding(0);
            this.inputPulseWidth.Name = "inputPulseWidth";
            this.inputPulseWidth.Size = new System.Drawing.Size(184, 36);
            this.inputPulseWidth.TabIndex = 1;
            this.inputPulseWidth.Text = "300";
            this.inputPulseWidth.WaveSize = 0;
            // 
            // labelPulseWidth
            // 
            this.labelPulseWidth.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelPulseWidth.ForeColor = System.Drawing.Color.Gray;
            this.labelPulseWidth.Location = new System.Drawing.Point(0, 0);
            this.labelPulseWidth.Margin = new System.Windows.Forms.Padding(0);
            this.labelPulseWidth.Name = "labelPulseWidth";
            this.labelPulseWidth.Size = new System.Drawing.Size(92, 36);
            this.labelPulseWidth.TabIndex = 0;
            this.labelPulseWidth.Text = "脉冲宽度（ms）";
            // 
            // panelDebugLeft
            // 
            this.panelDebugLeft.Controls.Add(this.panelDebugInfo);
            this.panelDebugLeft.Controls.Add(this.panelDebugHeader);
            this.panelDebugLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDebugLeft.Location = new System.Drawing.Point(12, 12);
            this.panelDebugLeft.Margin = new System.Windows.Forms.Padding(0);
            this.panelDebugLeft.Name = "panelDebugLeft";
            this.panelDebugLeft.Radius = 0;
            this.panelDebugLeft.Size = new System.Drawing.Size(752, 156);
            this.panelDebugLeft.TabIndex = 0;
            // 
            // panelDebugInfo
            // 
            this.panelDebugInfo.Controls.Add(this.labelSelectedRuntimeValue);
            this.panelDebugInfo.Controls.Add(this.labelSelectedRuntimeTitle);
            this.panelDebugInfo.Controls.Add(this.labelSelectedWiringValue);
            this.panelDebugInfo.Controls.Add(this.labelSelectedWiringTitle);
            this.panelDebugInfo.Controls.Add(this.labelSelectedHardwareValue);
            this.panelDebugInfo.Controls.Add(this.labelSelectedHardwareTitle);
            this.panelDebugInfo.Controls.Add(this.labelSelectedNameValue);
            this.panelDebugInfo.Controls.Add(this.labelSelectedNameTitle);
            this.panelDebugInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDebugInfo.Location = new System.Drawing.Point(0, 36);
            this.panelDebugInfo.Margin = new System.Windows.Forms.Padding(0);
            this.panelDebugInfo.Name = "panelDebugInfo";
            this.panelDebugInfo.Padding = new System.Windows.Forms.Padding(0, 8, 12, 0);
            this.panelDebugInfo.Radius = 0;
            this.panelDebugInfo.Size = new System.Drawing.Size(752, 120);
            this.panelDebugInfo.TabIndex = 1;
            // 
            // labelSelectedRuntimeValue
            // 
            this.labelSelectedRuntimeValue.Location = new System.Drawing.Point(96, 84);
            this.labelSelectedRuntimeValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelSelectedRuntimeValue.Name = "labelSelectedRuntimeValue";
            this.labelSelectedRuntimeValue.Size = new System.Drawing.Size(600, 24);
            this.labelSelectedRuntimeValue.TabIndex = 7;
            this.labelSelectedRuntimeValue.Text = "-";
            // 
            // labelSelectedRuntimeTitle
            // 
            this.labelSelectedRuntimeTitle.ForeColor = System.Drawing.Color.Gray;
            this.labelSelectedRuntimeTitle.Location = new System.Drawing.Point(0, 84);
            this.labelSelectedRuntimeTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelSelectedRuntimeTitle.Name = "labelSelectedRuntimeTitle";
            this.labelSelectedRuntimeTitle.Size = new System.Drawing.Size(88, 24);
            this.labelSelectedRuntimeTitle.TabIndex = 6;
            this.labelSelectedRuntimeTitle.Text = "运行状态";
            // 
            // labelSelectedWiringValue
            // 
            this.labelSelectedWiringValue.Location = new System.Drawing.Point(96, 56);
            this.labelSelectedWiringValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelSelectedWiringValue.Name = "labelSelectedWiringValue";
            this.labelSelectedWiringValue.Size = new System.Drawing.Size(600, 24);
            this.labelSelectedWiringValue.TabIndex = 5;
            this.labelSelectedWiringValue.Text = "-";
            // 
            // labelSelectedWiringTitle
            // 
            this.labelSelectedWiringTitle.ForeColor = System.Drawing.Color.Gray;
            this.labelSelectedWiringTitle.Location = new System.Drawing.Point(0, 56);
            this.labelSelectedWiringTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelSelectedWiringTitle.Name = "labelSelectedWiringTitle";
            this.labelSelectedWiringTitle.Size = new System.Drawing.Size(88, 24);
            this.labelSelectedWiringTitle.TabIndex = 4;
            this.labelSelectedWiringTitle.Text = "接线信息";
            // 
            // labelSelectedHardwareValue
            // 
            this.labelSelectedHardwareValue.Location = new System.Drawing.Point(96, 28);
            this.labelSelectedHardwareValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelSelectedHardwareValue.Name = "labelSelectedHardwareValue";
            this.labelSelectedHardwareValue.Size = new System.Drawing.Size(600, 24);
            this.labelSelectedHardwareValue.TabIndex = 3;
            this.labelSelectedHardwareValue.Text = "-";
            // 
            // labelSelectedHardwareTitle
            // 
            this.labelSelectedHardwareTitle.ForeColor = System.Drawing.Color.Gray;
            this.labelSelectedHardwareTitle.Location = new System.Drawing.Point(0, 28);
            this.labelSelectedHardwareTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelSelectedHardwareTitle.Name = "labelSelectedHardwareTitle";
            this.labelSelectedHardwareTitle.Size = new System.Drawing.Size(88, 24);
            this.labelSelectedHardwareTitle.TabIndex = 2;
            this.labelSelectedHardwareTitle.Text = "硬件位置";
            // 
            // labelSelectedNameValue
            // 
            this.labelSelectedNameValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.labelSelectedNameValue.Location = new System.Drawing.Point(96, 0);
            this.labelSelectedNameValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelSelectedNameValue.Name = "labelSelectedNameValue";
            this.labelSelectedNameValue.Size = new System.Drawing.Size(600, 24);
            this.labelSelectedNameValue.TabIndex = 1;
            this.labelSelectedNameValue.Text = "未选择点位";
            // 
            // labelSelectedNameTitle
            // 
            this.labelSelectedNameTitle.ForeColor = System.Drawing.Color.Gray;
            this.labelSelectedNameTitle.Location = new System.Drawing.Point(0, 0);
            this.labelSelectedNameTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelSelectedNameTitle.Name = "labelSelectedNameTitle";
            this.labelSelectedNameTitle.Size = new System.Drawing.Size(88, 24);
            this.labelSelectedNameTitle.TabIndex = 0;
            this.labelSelectedNameTitle.Text = "当前选中";
            // 
            // panelDebugHeader
            // 
            this.panelDebugHeader.Controls.Add(this.labelDebugHint);
            this.panelDebugHeader.Controls.Add(this.labelDebugTitle);
            this.panelDebugHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDebugHeader.Location = new System.Drawing.Point(0, 0);
            this.panelDebugHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelDebugHeader.Name = "panelDebugHeader";
            this.panelDebugHeader.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.panelDebugHeader.Radius = 0;
            this.panelDebugHeader.Size = new System.Drawing.Size(752, 36);
            this.panelDebugHeader.TabIndex = 0;
            // 
            // labelDebugHint
            // 
            this.labelDebugHint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDebugHint.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelDebugHint.ForeColor = System.Drawing.Color.Gray;
            this.labelDebugHint.Location = new System.Drawing.Point(96, 0);
            this.labelDebugHint.Margin = new System.Windows.Forms.Padding(0);
            this.labelDebugHint.Name = "labelDebugHint";
            this.labelDebugHint.Size = new System.Drawing.Size(656, 28);
            this.labelDebugHint.TabIndex = 1;
            this.labelDebugHint.Text = "请选择表格中的点位后执行检查；本页仅用于接线对照和单点 IO 检查。";
            // 
            // labelDebugTitle
            // 
            this.labelDebugTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelDebugTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.labelDebugTitle.Location = new System.Drawing.Point(0, 0);
            this.labelDebugTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelDebugTitle.Name = "labelDebugTitle";
            this.labelDebugTitle.Size = new System.Drawing.Size(96, 28);
            this.labelDebugTitle.TabIndex = 0;
            this.labelDebugTitle.Text = "底部调试区";
            // 
            // flowStats
            // 
            this.flowStats.Controls.Add(this.panelUnverifiedCard);
            this.flowStats.Controls.Add(this.panelVerifiedCard);
            this.flowStats.Controls.Add(this.panelTotalCard);
            this.flowStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowStats.Gap = 8;
            this.flowStats.Location = new System.Drawing.Point(8, 52);
            this.flowStats.Margin = new System.Windows.Forms.Padding(0);
            this.flowStats.Name = "flowStats";
            this.flowStats.Padding = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.flowStats.Size = new System.Drawing.Size(1084, 88);
            this.flowStats.TabIndex = 1;
            this.flowStats.Text = "flowStats";
            // 
            // panelUnverifiedCard
            // 
            this.panelUnverifiedCard.BackColor = System.Drawing.Color.Transparent;
            this.panelUnverifiedCard.Controls.Add(this.labelUnverifiedCount);
            this.panelUnverifiedCard.Controls.Add(this.labelUnverifiedTitle);
            this.panelUnverifiedCard.Location = new System.Drawing.Point(324, 6);
            this.panelUnverifiedCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelUnverifiedCard.Name = "panelUnverifiedCard";
            this.panelUnverifiedCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelUnverifiedCard.Radius = 12;
            this.panelUnverifiedCard.Shadow = 4;
            this.panelUnverifiedCard.ShadowOpacity = 0.2F;
            this.panelUnverifiedCard.Size = new System.Drawing.Size(152, 72);
            this.panelUnverifiedCard.TabIndex = 2;
            // 
            // labelUnverifiedCount
            // 
            this.labelUnverifiedCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelUnverifiedCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelUnverifiedCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(173)))), ((int)(((byte)(20)))));
            this.labelUnverifiedCount.Location = new System.Drawing.Point(80, 16);
            this.labelUnverifiedCount.Name = "labelUnverifiedCount";
            this.labelUnverifiedCount.Size = new System.Drawing.Size(56, 40);
            this.labelUnverifiedCount.TabIndex = 1;
            this.labelUnverifiedCount.Text = "0";
            // 
            // labelUnverifiedTitle
            // 
            this.labelUnverifiedTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelUnverifiedTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(173)))), ((int)(((byte)(20)))));
            this.labelUnverifiedTitle.Location = new System.Drawing.Point(16, 16);
            this.labelUnverifiedTitle.Name = "labelUnverifiedTitle";
            this.labelUnverifiedTitle.Size = new System.Drawing.Size(72, 40);
            this.labelUnverifiedTitle.TabIndex = 0;
            this.labelUnverifiedTitle.Text = "未核对";
            // 
            // panelVerifiedCard
            // 
            this.panelVerifiedCard.BackColor = System.Drawing.Color.Transparent;
            this.panelVerifiedCard.Controls.Add(this.labelVerifiedCount);
            this.panelVerifiedCard.Controls.Add(this.labelVerifiedTitle);
            this.panelVerifiedCard.Location = new System.Drawing.Point(164, 6);
            this.panelVerifiedCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelVerifiedCard.Name = "panelVerifiedCard";
            this.panelVerifiedCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelVerifiedCard.Radius = 12;
            this.panelVerifiedCard.Shadow = 4;
            this.panelVerifiedCard.ShadowOpacity = 0.2F;
            this.panelVerifiedCard.Size = new System.Drawing.Size(152, 72);
            this.panelVerifiedCard.TabIndex = 1;
            // 
            // labelVerifiedCount
            // 
            this.labelVerifiedCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelVerifiedCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelVerifiedCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(119)))), ((int)(((byte)(255)))));
            this.labelVerifiedCount.Location = new System.Drawing.Point(80, 16);
            this.labelVerifiedCount.Name = "labelVerifiedCount";
            this.labelVerifiedCount.Size = new System.Drawing.Size(56, 40);
            this.labelVerifiedCount.TabIndex = 1;
            this.labelVerifiedCount.Text = "0";
            // 
            // labelVerifiedTitle
            // 
            this.labelVerifiedTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelVerifiedTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(119)))), ((int)(((byte)(255)))));
            this.labelVerifiedTitle.Location = new System.Drawing.Point(16, 16);
            this.labelVerifiedTitle.Name = "labelVerifiedTitle";
            this.labelVerifiedTitle.Size = new System.Drawing.Size(72, 40);
            this.labelVerifiedTitle.TabIndex = 0;
            this.labelVerifiedTitle.Text = "已核对";
            // 
            // panelTotalCard
            // 
            this.panelTotalCard.BackColor = System.Drawing.Color.Transparent;
            this.panelTotalCard.Controls.Add(this.labelTotalCount);
            this.panelTotalCard.Controls.Add(this.labelTotalTitle);
            this.panelTotalCard.Location = new System.Drawing.Point(4, 6);
            this.panelTotalCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelTotalCard.Name = "panelTotalCard";
            this.panelTotalCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelTotalCard.Radius = 12;
            this.panelTotalCard.Shadow = 4;
            this.panelTotalCard.ShadowOpacity = 0.2F;
            this.panelTotalCard.Size = new System.Drawing.Size(152, 72);
            this.panelTotalCard.TabIndex = 0;
            // 
            // labelTotalCount
            // 
            this.labelTotalCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelTotalCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelTotalCount.Location = new System.Drawing.Point(80, 16);
            this.labelTotalCount.Name = "labelTotalCount";
            this.labelTotalCount.Size = new System.Drawing.Size(56, 40);
            this.labelTotalCount.TabIndex = 1;
            this.labelTotalCount.Text = "0";
            // 
            // labelTotalTitle
            // 
            this.labelTotalTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelTotalTitle.Location = new System.Drawing.Point(16, 16);
            this.labelTotalTitle.Name = "labelTotalTitle";
            this.labelTotalTitle.Size = new System.Drawing.Size(72, 40);
            this.labelTotalTitle.TabIndex = 0;
            this.labelTotalTitle.Text = "点位总数";
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
            this.flowToolbarLeft.Controls.Add(this.inputSearch);
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
            this.flowToolbarLeft.Size = new System.Drawing.Size(532, 36);
            this.flowToolbarLeft.TabIndex = 0;
            this.flowToolbarLeft.Text = "flowToolbarLeft";
            // 
            // inputSearch
            // 
            this.inputSearch.Location = new System.Drawing.Point(398, 0);
            this.inputSearch.Margin = new System.Windows.Forms.Padding(0);
            this.inputSearch.Name = "inputSearch";
            this.inputSearch.PlaceholderText = "搜索点位 / 设备 / 线号";
            this.inputSearch.Size = new System.Drawing.Size(110, 36);
            this.inputSearch.TabIndex = 5;
            this.inputSearch.WaveSize = 0;
            // 
            // buttonFilterDO
            // 
            this.buttonFilterDO.Location = new System.Drawing.Point(334, 0);
            this.buttonFilterDO.Margin = new System.Windows.Forms.Padding(0);
            this.buttonFilterDO.Name = "buttonFilterDO";
            this.buttonFilterDO.Radius = 8;
            this.buttonFilterDO.Size = new System.Drawing.Size(56, 36);
            this.buttonFilterDO.TabIndex = 4;
            this.buttonFilterDO.Text = "DO";
            this.buttonFilterDO.WaveSize = 0;
            // 
            // buttonFilterDI
            // 
            this.buttonFilterDI.Location = new System.Drawing.Point(270, 0);
            this.buttonFilterDI.Margin = new System.Windows.Forms.Padding(0);
            this.buttonFilterDI.Name = "buttonFilterDI";
            this.buttonFilterDI.Radius = 8;
            this.buttonFilterDI.Size = new System.Drawing.Size(56, 36);
            this.buttonFilterDI.TabIndex = 3;
            this.buttonFilterDI.Text = "DI";
            this.buttonFilterDI.WaveSize = 0;
            // 
            // buttonFilterAll
            // 
            this.buttonFilterAll.Location = new System.Drawing.Point(206, 0);
            this.buttonFilterAll.Margin = new System.Windows.Forms.Padding(0);
            this.buttonFilterAll.Name = "buttonFilterAll";
            this.buttonFilterAll.Radius = 8;
            this.buttonFilterAll.Size = new System.Drawing.Size(56, 36);
            this.buttonFilterAll.TabIndex = 2;
            this.buttonFilterAll.Text = "全部";
            this.buttonFilterAll.Type = AntdUI.TTypeMini.Primary;
            this.buttonFilterAll.WaveSize = 0;
            // 
            // labelSelectedCard
            // 
            this.labelSelectedCard.Location = new System.Drawing.Point(83, 0);
            this.labelSelectedCard.Margin = new System.Windows.Forms.Padding(0);
            this.labelSelectedCard.Name = "labelSelectedCard";
            this.labelSelectedCard.Size = new System.Drawing.Size(115, 36);
            this.labelSelectedCard.TabIndex = 1;
            this.labelSelectedCard.Text = "全部控制卡";
            // 
            // buttonSelectCard
            // 
            this.buttonSelectCard.IconSvg = "CreditCardOutlined";
            this.buttonSelectCard.Location = new System.Drawing.Point(0, 0);
            this.buttonSelectCard.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSelectCard.Name = "buttonSelectCard";
            this.buttonSelectCard.Radius = 8;
            this.buttonSelectCard.Size = new System.Drawing.Size(75, 36);
            this.buttonSelectCard.TabIndex = 0;
            this.buttonSelectCard.Text = "控制卡";
            this.buttonSelectCard.Type = AntdUI.TTypeMini.Primary;
            this.buttonSelectCard.WaveSize = 0;
            // 
            // flowToolbarRight
            // 
            this.flowToolbarRight.Align = AntdUI.TAlignFlow.RightCenter;
            this.flowToolbarRight.Controls.Add(this.buttonExportCsv);
            this.flowToolbarRight.Controls.Add(this.buttonRefresh);
            this.flowToolbarRight.Controls.Add(this.checkboxOnlyUnverified);
            this.flowToolbarRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowToolbarRight.Gap = 8;
            this.flowToolbarRight.Location = new System.Drawing.Point(808, 4);
            this.flowToolbarRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowToolbarRight.Name = "flowToolbarRight";
            this.flowToolbarRight.Size = new System.Drawing.Size(272, 36);
            this.flowToolbarRight.TabIndex = 1;
            this.flowToolbarRight.Text = "flowToolbarRight";
            // 
            // buttonExportCsv
            // 
            this.buttonExportCsv.IconSvg = "DownloadOutlined";
            this.buttonExportCsv.Location = new System.Drawing.Point(182, 0);
            this.buttonExportCsv.Margin = new System.Windows.Forms.Padding(0);
            this.buttonExportCsv.Name = "buttonExportCsv";
            this.buttonExportCsv.Radius = 8;
            this.buttonExportCsv.Size = new System.Drawing.Size(90, 36);
            this.buttonExportCsv.TabIndex = 3;
            this.buttonExportCsv.Text = "导出 CSV";
            this.buttonExportCsv.WaveSize = 0;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.IconSvg = "ReloadOutlined";
            this.buttonRefresh.Location = new System.Drawing.Point(109, 0);
            this.buttonRefresh.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Radius = 8;
            this.buttonRefresh.Size = new System.Drawing.Size(65, 36);
            this.buttonRefresh.TabIndex = 2;
            this.buttonRefresh.Text = "刷新";
            this.buttonRefresh.WaveSize = 0;
            // 
            // checkboxOnlyUnverified
            // 
            this.checkboxOnlyUnverified.Location = new System.Drawing.Point(29, 0);
            this.checkboxOnlyUnverified.Margin = new System.Windows.Forms.Padding(0);
            this.checkboxOnlyUnverified.Name = "checkboxOnlyUnverified";
            this.checkboxOnlyUnverified.Size = new System.Drawing.Size(72, 36);
            this.checkboxOnlyUnverified.TabIndex = 0;
            this.checkboxOnlyUnverified.Text = "未核对";
            // 
            // AssemblyWiringPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "AssemblyWiringPage";
            this.Size = new System.Drawing.Size(1100, 720);
            this.panelRoot.ResumeLayout(false);
            this.panelTableCard.ResumeLayout(false);
            this.panelTableHeader.ResumeLayout(false);
            this.panelDebugCard.ResumeLayout(false);
            this.gridDebugLayout.ResumeLayout(false);
            this.panelDebugActions.ResumeLayout(false);
            this.panelDebugButtons.ResumeLayout(false);
            this.panelPulseRow.ResumeLayout(false);
            this.panelDebugLeft.ResumeLayout(false);
            this.panelDebugInfo.ResumeLayout(false);
            this.panelDebugHeader.ResumeLayout(false);
            this.flowStats.ResumeLayout(false);
            this.panelUnverifiedCard.ResumeLayout(false);
            this.panelVerifiedCard.ResumeLayout(false);
            this.panelTotalCard.ResumeLayout(false);
            this.panelToolbar.ResumeLayout(false);
            this.flowToolbarLeft.ResumeLayout(false);
            this.flowToolbarRight.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.Panel panelRoot;
        private AntdUI.Panel panelToolbar;
        private AntdUI.FlowPanel flowToolbarLeft;
        private AntdUI.Button buttonSelectCard;
        private AntdUI.Label labelSelectedCard;
        private AntdUI.Button buttonFilterAll;
        private AntdUI.Button buttonFilterDI;
        private AntdUI.Button buttonFilterDO;
        private AntdUI.Input inputSearch;
        private AntdUI.FlowPanel flowToolbarRight;
        private AntdUI.Checkbox checkboxOnlyUnverified;
        private AntdUI.Button buttonExportCsv;
        private AntdUI.Button buttonRefresh;
        private AntdUI.FlowPanel flowStats;
        private AntdUI.Panel panelTotalCard;
        private AntdUI.Label labelTotalTitle;
        private AntdUI.Label labelTotalCount;
        private AntdUI.Panel panelVerifiedCard;
        private AntdUI.Label labelVerifiedCount;
        private AntdUI.Label labelVerifiedTitle;
        private AntdUI.Panel panelUnverifiedCard;
        private AntdUI.Label labelUnverifiedCount;
        private AntdUI.Label labelUnverifiedTitle;
        private AntdUI.Panel panelTableCard;
        private AntdUI.Panel panelTableHeader;
        private AntdUI.Label labelTableTitle;
        private AntdUI.Label labelTableSummary;
        private AntdUI.Table tableWiring;
        private AntdUI.Panel panelDebugCard;
        private AntdUI.GridPanel gridDebugLayout;
        private AntdUI.Panel panelDebugLeft;
        private AntdUI.Panel panelDebugHeader;
        private AntdUI.Label labelDebugTitle;
        private AntdUI.Label labelDebugHint;
        private AntdUI.Panel panelDebugInfo;
        private AntdUI.Label labelSelectedNameTitle;
        private AntdUI.Label labelSelectedNameValue;
        private AntdUI.Label labelSelectedHardwareTitle;
        private AntdUI.Label labelSelectedHardwareValue;
        private AntdUI.Label labelSelectedWiringTitle;
        private AntdUI.Label labelSelectedWiringValue;
        private AntdUI.Label labelSelectedRuntimeTitle;
        private AntdUI.Label labelSelectedRuntimeValue;
        private AntdUI.Panel panelDebugActions;
        private AntdUI.Label labelPulseWidth;
        private AntdUI.Input inputPulseWidth;
        private AntdUI.Panel panelDebugButtons;
        private AntdUI.Panel panelPulseRow;
        private AntdUI.Button buttonCancelVerified;
        private AntdUI.Button buttonMarkVerified;
        private AntdUI.Button buttonReadDi;
        private AntdUI.Button buttonDoOn;
        private AntdUI.Button buttonDoOff;
        private AntdUI.Button buttonPulseDo;
    }
}
