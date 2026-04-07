namespace AMControlWinF.Views.MotionConfig
{
    partial class VacuumEditControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelContent = new AntdUI.Panel();
            this.gridMainSections = new AntdUI.GridPanel();
            this.stackSectionRemark = new AntdUI.StackPanel();
            this.inputRemark = new AntdUI.Input();
            this.labelRemark = new AntdUI.Label();
            this.inputDescription = new AntdUI.Input();
            this.labelDescription = new AntdUI.Label();
            this.checkIsEnabled = new AntdUI.Checkbox();
            this.labelEnabled = new AntdUI.Label();
            this.inputSortOrder = new AntdUI.Input();
            this.labelSortOrder = new AntdUI.Label();
            this.labelSectionRemark = new AntdUI.Label();
            this.stackSectionRuntime = new AntdUI.StackPanel();
            this.inputReleaseTimeoutMs = new AntdUI.Input();
            this.labelReleaseTimeoutMs = new AntdUI.Label();
            this.inputVacuumBuildTimeoutMs = new AntdUI.Input();
            this.labelVacuumBuildTimeoutMs = new AntdUI.Label();
            this.checkKeepVacuumOnAfterDetected = new AntdUI.Checkbox();
            this.checkUseWorkpieceCheck = new AntdUI.Checkbox();
            this.checkUseFeedbackCheck = new AntdUI.Checkbox();
            this.dropdownWorkpiecePresentBit = new AntdUI.Select();
            this.labelWorkpiecePresentBit = new AntdUI.Label();
            this.dropdownReleaseFeedbackBit = new AntdUI.Select();
            this.labelReleaseFeedbackBit = new AntdUI.Label();
            this.labelSectionRuntime = new AntdUI.Label();
            this.stackSectionMapping = new AntdUI.StackPanel();
            this.dropdownVacuumFeedbackBit = new AntdUI.Select();
            this.labelVacuumFeedbackBit = new AntdUI.Label();
            this.dropdownBlowOffOutputBit = new AntdUI.Select();
            this.labelBlowOffOutputBit = new AntdUI.Label();
            this.dropdownVacuumOnOutputBit = new AntdUI.Select();
            this.labelVacuumOnOutputBit = new AntdUI.Label();
            this.labelSectionMapping = new AntdUI.Label();
            this.stackSectionBasic = new AntdUI.StackPanel();
            this.inputDisplayName = new AntdUI.Input();
            this.labelDisplayName = new AntdUI.Label();
            this.inputName = new AntdUI.Input();
            this.labelName = new AntdUI.Label();
            this.labelSectionBasic = new AntdUI.Label();
            this.panelContent.SuspendLayout();
            this.gridMainSections.SuspendLayout();
            this.stackSectionRemark.SuspendLayout();
            this.stackSectionRuntime.SuspendLayout();
            this.stackSectionMapping.SuspendLayout();
            this.stackSectionBasic.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelContent
            // 
            this.panelContent.Controls.Add(this.gridMainSections);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 0);
            this.panelContent.Margin = new System.Windows.Forms.Padding(0);
            this.panelContent.Name = "panelContent";
            this.panelContent.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.panelContent.Radius = 0;
            this.panelContent.Size = new System.Drawing.Size(836, 431);
            this.panelContent.TabIndex = 0;
            // 
            // gridMainSections
            // 
            this.gridMainSections.Controls.Add(this.stackSectionRemark);
            this.gridMainSections.Controls.Add(this.stackSectionRuntime);
            this.gridMainSections.Controls.Add(this.stackSectionMapping);
            this.gridMainSections.Controls.Add(this.stackSectionBasic);
            this.gridMainSections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridMainSections.Location = new System.Drawing.Point(4, 0);
            this.gridMainSections.Margin = new System.Windows.Forms.Padding(0);
            this.gridMainSections.Name = "gridMainSections";
            this.gridMainSections.Size = new System.Drawing.Size(828, 431);
            this.gridMainSections.Span = "30% 20% 20% 30%";
            this.gridMainSections.TabIndex = 0;
            this.gridMainSections.Text = "gridMainSections";
            // 
            // stackSectionRemark
            // 
            this.stackSectionRemark.AutoScroll = true;
            this.stackSectionRemark.Controls.Add(this.inputRemark);
            this.stackSectionRemark.Controls.Add(this.labelRemark);
            this.stackSectionRemark.Controls.Add(this.inputDescription);
            this.stackSectionRemark.Controls.Add(this.labelDescription);
            this.stackSectionRemark.Controls.Add(this.checkIsEnabled);
            this.stackSectionRemark.Controls.Add(this.labelEnabled);
            this.stackSectionRemark.Controls.Add(this.inputSortOrder);
            this.stackSectionRemark.Controls.Add(this.labelSortOrder);
            this.stackSectionRemark.Controls.Add(this.labelSectionRemark);
            this.stackSectionRemark.Gap = 4;
            this.stackSectionRemark.Location = new System.Drawing.Point(580, 0);
            this.stackSectionRemark.Margin = new System.Windows.Forms.Padding(0);
            this.stackSectionRemark.Name = "stackSectionRemark";
            this.stackSectionRemark.Padding = new System.Windows.Forms.Padding(4);
            this.stackSectionRemark.Size = new System.Drawing.Size(248, 431);
            this.stackSectionRemark.TabIndex = 3;
            this.stackSectionRemark.Text = "stackSectionRemark";
            this.stackSectionRemark.Vertical = true;
            // 
            // inputRemark
            // 
            this.inputRemark.Location = new System.Drawing.Point(4, 275);
            this.inputRemark.Margin = new System.Windows.Forms.Padding(0);
            this.inputRemark.Multiline = true;
            this.inputRemark.Name = "inputRemark";
            this.inputRemark.PlaceholderText = "请输入备注";
            this.inputRemark.Size = new System.Drawing.Size(240, 65);
            this.inputRemark.TabIndex = 8;
            this.inputRemark.WaveSize = 0;
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(4, 249);
            this.labelRemark.Margin = new System.Windows.Forms.Padding(0);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(240, 22);
            this.labelRemark.TabIndex = 7;
            this.labelRemark.Text = "备注";
            // 
            // inputDescription
            // 
            this.inputDescription.Location = new System.Drawing.Point(4, 180);
            this.inputDescription.Margin = new System.Windows.Forms.Padding(0);
            this.inputDescription.Multiline = true;
            this.inputDescription.Name = "inputDescription";
            this.inputDescription.PlaceholderText = "请输入描述";
            this.inputDescription.Size = new System.Drawing.Size(240, 65);
            this.inputDescription.TabIndex = 6;
            this.inputDescription.WaveSize = 0;
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(4, 154);
            this.labelDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(240, 22);
            this.labelDescription.TabIndex = 5;
            this.labelDescription.Text = "描述";
            // 
            // checkIsEnabled
            // 
            this.checkIsEnabled.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkIsEnabled.Checked = true;
            this.checkIsEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkIsEnabled.Location = new System.Drawing.Point(4, 122);
            this.checkIsEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.checkIsEnabled.Name = "checkIsEnabled";
            this.checkIsEnabled.Size = new System.Drawing.Size(76, 28);
            this.checkIsEnabled.TabIndex = 4;
            this.checkIsEnabled.Text = "启用对象";
            // 
            // labelEnabled
            // 
            this.labelEnabled.Location = new System.Drawing.Point(4, 96);
            this.labelEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.labelEnabled.Name = "labelEnabled";
            this.labelEnabled.Size = new System.Drawing.Size(240, 22);
            this.labelEnabled.TabIndex = 3;
            this.labelEnabled.Text = "启用状态";
            // 
            // inputSortOrder
            // 
            this.inputSortOrder.Location = new System.Drawing.Point(4, 60);
            this.inputSortOrder.Margin = new System.Windows.Forms.Padding(0);
            this.inputSortOrder.Name = "inputSortOrder";
            this.inputSortOrder.PlaceholderText = "请输入排序号";
            this.inputSortOrder.Size = new System.Drawing.Size(240, 32);
            this.inputSortOrder.TabIndex = 2;
            this.inputSortOrder.WaveSize = 0;
            // 
            // labelSortOrder
            // 
            this.labelSortOrder.Location = new System.Drawing.Point(4, 34);
            this.labelSortOrder.Margin = new System.Windows.Forms.Padding(0);
            this.labelSortOrder.Name = "labelSortOrder";
            this.labelSortOrder.Size = new System.Drawing.Size(240, 22);
            this.labelSortOrder.TabIndex = 1;
            this.labelSortOrder.Text = "排序号";
            // 
            // labelSectionRemark
            // 
            this.labelSectionRemark.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionRemark.Location = new System.Drawing.Point(4, 4);
            this.labelSectionRemark.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionRemark.Name = "labelSectionRemark";
            this.labelSectionRemark.Size = new System.Drawing.Size(240, 26);
            this.labelSectionRemark.TabIndex = 0;
            this.labelSectionRemark.Text = "状态与备注";
            // 
            // stackSectionRuntime
            // 
            this.stackSectionRuntime.AutoScroll = true;
            this.stackSectionRuntime.Controls.Add(this.inputReleaseTimeoutMs);
            this.stackSectionRuntime.Controls.Add(this.labelReleaseTimeoutMs);
            this.stackSectionRuntime.Controls.Add(this.inputVacuumBuildTimeoutMs);
            this.stackSectionRuntime.Controls.Add(this.labelVacuumBuildTimeoutMs);
            this.stackSectionRuntime.Controls.Add(this.checkKeepVacuumOnAfterDetected);
            this.stackSectionRuntime.Controls.Add(this.checkUseWorkpieceCheck);
            this.stackSectionRuntime.Controls.Add(this.checkUseFeedbackCheck);
            this.stackSectionRuntime.Controls.Add(this.dropdownWorkpiecePresentBit);
            this.stackSectionRuntime.Controls.Add(this.labelWorkpiecePresentBit);
            this.stackSectionRuntime.Controls.Add(this.dropdownReleaseFeedbackBit);
            this.stackSectionRuntime.Controls.Add(this.labelReleaseFeedbackBit);
            this.stackSectionRuntime.Controls.Add(this.labelSectionRuntime);
            this.stackSectionRuntime.Gap = 4;
            this.stackSectionRuntime.Location = new System.Drawing.Point(414, 0);
            this.stackSectionRuntime.Margin = new System.Windows.Forms.Padding(0);
            this.stackSectionRuntime.Name = "stackSectionRuntime";
            this.stackSectionRuntime.Padding = new System.Windows.Forms.Padding(4);
            this.stackSectionRuntime.Size = new System.Drawing.Size(166, 431);
            this.stackSectionRuntime.TabIndex = 2;
            this.stackSectionRuntime.Text = "stackSectionRuntime";
            this.stackSectionRuntime.Vertical = true;
            // 
            // inputReleaseTimeoutMs
            // 
            this.inputReleaseTimeoutMs.Location = new System.Drawing.Point(4, 340);
            this.inputReleaseTimeoutMs.Margin = new System.Windows.Forms.Padding(0);
            this.inputReleaseTimeoutMs.Name = "inputReleaseTimeoutMs";
            this.inputReleaseTimeoutMs.PlaceholderText = "请输入释放超时";
            this.inputReleaseTimeoutMs.Size = new System.Drawing.Size(158, 32);
            this.inputReleaseTimeoutMs.TabIndex = 10;
            this.inputReleaseTimeoutMs.WaveSize = 0;
            // 
            // labelReleaseTimeoutMs
            // 
            this.labelReleaseTimeoutMs.Location = new System.Drawing.Point(4, 314);
            this.labelReleaseTimeoutMs.Margin = new System.Windows.Forms.Padding(0);
            this.labelReleaseTimeoutMs.Name = "labelReleaseTimeoutMs";
            this.labelReleaseTimeoutMs.Size = new System.Drawing.Size(158, 22);
            this.labelReleaseTimeoutMs.TabIndex = 9;
            this.labelReleaseTimeoutMs.Text = "释放超时(ms)";
            // 
            // inputVacuumBuildTimeoutMs
            // 
            this.inputVacuumBuildTimeoutMs.Location = new System.Drawing.Point(4, 278);
            this.inputVacuumBuildTimeoutMs.Margin = new System.Windows.Forms.Padding(0);
            this.inputVacuumBuildTimeoutMs.Name = "inputVacuumBuildTimeoutMs";
            this.inputVacuumBuildTimeoutMs.PlaceholderText = "请输入建压超时";
            this.inputVacuumBuildTimeoutMs.Size = new System.Drawing.Size(158, 32);
            this.inputVacuumBuildTimeoutMs.TabIndex = 8;
            this.inputVacuumBuildTimeoutMs.WaveSize = 0;
            // 
            // labelVacuumBuildTimeoutMs
            // 
            this.labelVacuumBuildTimeoutMs.Location = new System.Drawing.Point(4, 252);
            this.labelVacuumBuildTimeoutMs.Margin = new System.Windows.Forms.Padding(0);
            this.labelVacuumBuildTimeoutMs.Name = "labelVacuumBuildTimeoutMs";
            this.labelVacuumBuildTimeoutMs.Size = new System.Drawing.Size(158, 22);
            this.labelVacuumBuildTimeoutMs.TabIndex = 7;
            this.labelVacuumBuildTimeoutMs.Text = "建压超时(ms)";
            // 
            // checkKeepVacuumOnAfterDetected
            // 
            this.checkKeepVacuumOnAfterDetected.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkKeepVacuumOnAfterDetected.Location = new System.Drawing.Point(4, 220);
            this.checkKeepVacuumOnAfterDetected.Margin = new System.Windows.Forms.Padding(0);
            this.checkKeepVacuumOnAfterDetected.Name = "checkKeepVacuumOnAfterDetected";
            this.checkKeepVacuumOnAfterDetected.Size = new System.Drawing.Size(148, 28);
            this.checkKeepVacuumOnAfterDetected.TabIndex = 6;
            this.checkKeepVacuumOnAfterDetected.Text = "检测到工件后保持真空";
            // 
            // checkUseWorkpieceCheck
            // 
            this.checkUseWorkpieceCheck.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkUseWorkpieceCheck.Location = new System.Drawing.Point(4, 188);
            this.checkUseWorkpieceCheck.Margin = new System.Windows.Forms.Padding(0);
            this.checkUseWorkpieceCheck.Name = "checkUseWorkpieceCheck";
            this.checkUseWorkpieceCheck.Size = new System.Drawing.Size(124, 28);
            this.checkUseWorkpieceCheck.TabIndex = 5;
            this.checkUseWorkpieceCheck.Text = "启用工件检测校验";
            // 
            // checkUseFeedbackCheck
            // 
            this.checkUseFeedbackCheck.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkUseFeedbackCheck.Location = new System.Drawing.Point(4, 156);
            this.checkUseFeedbackCheck.Margin = new System.Windows.Forms.Padding(0);
            this.checkUseFeedbackCheck.Name = "checkUseFeedbackCheck";
            this.checkUseFeedbackCheck.Size = new System.Drawing.Size(100, 28);
            this.checkUseFeedbackCheck.TabIndex = 4;
            this.checkUseFeedbackCheck.Text = "启用反馈校验";
            // 
            // dropdownWorkpiecePresentBit
            // 
            this.dropdownWorkpiecePresentBit.Location = new System.Drawing.Point(4, 120);
            this.dropdownWorkpiecePresentBit.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownWorkpiecePresentBit.Name = "dropdownWorkpiecePresentBit";
            this.dropdownWorkpiecePresentBit.Size = new System.Drawing.Size(158, 32);
            this.dropdownWorkpiecePresentBit.TabIndex = 3;
            this.dropdownWorkpiecePresentBit.WaveSize = 0;
            // 
            // labelWorkpiecePresentBit
            // 
            this.labelWorkpiecePresentBit.Location = new System.Drawing.Point(4, 94);
            this.labelWorkpiecePresentBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelWorkpiecePresentBit.Name = "labelWorkpiecePresentBit";
            this.labelWorkpiecePresentBit.Size = new System.Drawing.Size(158, 22);
            this.labelWorkpiecePresentBit.TabIndex = 2;
            this.labelWorkpiecePresentBit.Text = "工件检测位(DI)";
            // 
            // dropdownReleaseFeedbackBit
            // 
            this.dropdownReleaseFeedbackBit.Location = new System.Drawing.Point(4, 58);
            this.dropdownReleaseFeedbackBit.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownReleaseFeedbackBit.Name = "dropdownReleaseFeedbackBit";
            this.dropdownReleaseFeedbackBit.Size = new System.Drawing.Size(158, 32);
            this.dropdownReleaseFeedbackBit.TabIndex = 1;
            this.dropdownReleaseFeedbackBit.WaveSize = 0;
            // 
            // labelReleaseFeedbackBit
            // 
            this.labelReleaseFeedbackBit.Location = new System.Drawing.Point(4, 32);
            this.labelReleaseFeedbackBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelReleaseFeedbackBit.Name = "labelReleaseFeedbackBit";
            this.labelReleaseFeedbackBit.Size = new System.Drawing.Size(158, 22);
            this.labelReleaseFeedbackBit.TabIndex = 0;
            this.labelReleaseFeedbackBit.Text = "释放反馈位(DI)";
            // 
            // labelSectionRuntime
            // 
            this.labelSectionRuntime.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionRuntime.Location = new System.Drawing.Point(4, 4);
            this.labelSectionRuntime.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionRuntime.Name = "labelSectionRuntime";
            this.labelSectionRuntime.Size = new System.Drawing.Size(158, 24);
            this.labelSectionRuntime.TabIndex = 11;
            this.labelSectionRuntime.Text = "控制参数";
            // 
            // stackSectionMapping
            // 
            this.stackSectionMapping.AutoScroll = true;
            this.stackSectionMapping.Controls.Add(this.dropdownVacuumFeedbackBit);
            this.stackSectionMapping.Controls.Add(this.labelVacuumFeedbackBit);
            this.stackSectionMapping.Controls.Add(this.dropdownBlowOffOutputBit);
            this.stackSectionMapping.Controls.Add(this.labelBlowOffOutputBit);
            this.stackSectionMapping.Controls.Add(this.dropdownVacuumOnOutputBit);
            this.stackSectionMapping.Controls.Add(this.labelVacuumOnOutputBit);
            this.stackSectionMapping.Controls.Add(this.labelSectionMapping);
            this.stackSectionMapping.Gap = 4;
            this.stackSectionMapping.Location = new System.Drawing.Point(248, 0);
            this.stackSectionMapping.Margin = new System.Windows.Forms.Padding(0);
            this.stackSectionMapping.Name = "stackSectionMapping";
            this.stackSectionMapping.Padding = new System.Windows.Forms.Padding(4);
            this.stackSectionMapping.Size = new System.Drawing.Size(166, 431);
            this.stackSectionMapping.TabIndex = 1;
            this.stackSectionMapping.Text = "stackSectionMapping";
            this.stackSectionMapping.Vertical = true;
            // 
            // dropdownVacuumFeedbackBit
            // 
            this.dropdownVacuumFeedbackBit.Location = new System.Drawing.Point(4, 182);
            this.dropdownVacuumFeedbackBit.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownVacuumFeedbackBit.Name = "dropdownVacuumFeedbackBit";
            this.dropdownVacuumFeedbackBit.Size = new System.Drawing.Size(158, 32);
            this.dropdownVacuumFeedbackBit.TabIndex = 6;
            this.dropdownVacuumFeedbackBit.WaveSize = 0;
            // 
            // labelVacuumFeedbackBit
            // 
            this.labelVacuumFeedbackBit.Location = new System.Drawing.Point(4, 156);
            this.labelVacuumFeedbackBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelVacuumFeedbackBit.Name = "labelVacuumFeedbackBit";
            this.labelVacuumFeedbackBit.Size = new System.Drawing.Size(158, 22);
            this.labelVacuumFeedbackBit.TabIndex = 5;
            this.labelVacuumFeedbackBit.Text = "真空反馈位(DI)";
            // 
            // dropdownBlowOffOutputBit
            // 
            this.dropdownBlowOffOutputBit.Location = new System.Drawing.Point(4, 120);
            this.dropdownBlowOffOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownBlowOffOutputBit.Name = "dropdownBlowOffOutputBit";
            this.dropdownBlowOffOutputBit.Size = new System.Drawing.Size(158, 32);
            this.dropdownBlowOffOutputBit.TabIndex = 4;
            this.dropdownBlowOffOutputBit.WaveSize = 0;
            // 
            // labelBlowOffOutputBit
            // 
            this.labelBlowOffOutputBit.Location = new System.Drawing.Point(4, 94);
            this.labelBlowOffOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelBlowOffOutputBit.Name = "labelBlowOffOutputBit";
            this.labelBlowOffOutputBit.Size = new System.Drawing.Size(158, 22);
            this.labelBlowOffOutputBit.TabIndex = 3;
            this.labelBlowOffOutputBit.Text = "破真空输出位(DO)";
            // 
            // dropdownVacuumOnOutputBit
            // 
            this.dropdownVacuumOnOutputBit.Location = new System.Drawing.Point(4, 58);
            this.dropdownVacuumOnOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownVacuumOnOutputBit.Name = "dropdownVacuumOnOutputBit";
            this.dropdownVacuumOnOutputBit.Size = new System.Drawing.Size(158, 32);
            this.dropdownVacuumOnOutputBit.TabIndex = 2;
            this.dropdownVacuumOnOutputBit.WaveSize = 0;
            // 
            // labelVacuumOnOutputBit
            // 
            this.labelVacuumOnOutputBit.Location = new System.Drawing.Point(4, 32);
            this.labelVacuumOnOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelVacuumOnOutputBit.Name = "labelVacuumOnOutputBit";
            this.labelVacuumOnOutputBit.Size = new System.Drawing.Size(158, 22);
            this.labelVacuumOnOutputBit.TabIndex = 1;
            this.labelVacuumOnOutputBit.Text = "吸真空输出位(DO)";
            // 
            // labelSectionMapping
            // 
            this.labelSectionMapping.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionMapping.Location = new System.Drawing.Point(4, 4);
            this.labelSectionMapping.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionMapping.Name = "labelSectionMapping";
            this.labelSectionMapping.Size = new System.Drawing.Size(158, 24);
            this.labelSectionMapping.TabIndex = 0;
            this.labelSectionMapping.Text = "IO映射";
            // 
            // stackSectionBasic
            // 
            this.stackSectionBasic.AutoScroll = true;
            this.stackSectionBasic.Controls.Add(this.inputDisplayName);
            this.stackSectionBasic.Controls.Add(this.labelDisplayName);
            this.stackSectionBasic.Controls.Add(this.inputName);
            this.stackSectionBasic.Controls.Add(this.labelName);
            this.stackSectionBasic.Controls.Add(this.labelSectionBasic);
            this.stackSectionBasic.Gap = 4;
            this.stackSectionBasic.Location = new System.Drawing.Point(0, 0);
            this.stackSectionBasic.Margin = new System.Windows.Forms.Padding(0);
            this.stackSectionBasic.Name = "stackSectionBasic";
            this.stackSectionBasic.Padding = new System.Windows.Forms.Padding(4);
            this.stackSectionBasic.Size = new System.Drawing.Size(248, 431);
            this.stackSectionBasic.TabIndex = 0;
            this.stackSectionBasic.Text = "stackSectionBasic";
            this.stackSectionBasic.Vertical = true;
            // 
            // inputDisplayName
            // 
            this.inputDisplayName.Location = new System.Drawing.Point(4, 120);
            this.inputDisplayName.Margin = new System.Windows.Forms.Padding(0);
            this.inputDisplayName.Name = "inputDisplayName";
            this.inputDisplayName.PlaceholderText = "请输入显示名称";
            this.inputDisplayName.Size = new System.Drawing.Size(240, 32);
            this.inputDisplayName.TabIndex = 4;
            this.inputDisplayName.WaveSize = 0;
            // 
            // labelDisplayName
            // 
            this.labelDisplayName.Location = new System.Drawing.Point(4, 94);
            this.labelDisplayName.Margin = new System.Windows.Forms.Padding(0);
            this.labelDisplayName.Name = "labelDisplayName";
            this.labelDisplayName.Size = new System.Drawing.Size(240, 22);
            this.labelDisplayName.TabIndex = 3;
            this.labelDisplayName.Text = "显示名称";
            // 
            // inputName
            // 
            this.inputName.Location = new System.Drawing.Point(4, 58);
            this.inputName.Margin = new System.Windows.Forms.Padding(0);
            this.inputName.Name = "inputName";
            this.inputName.PlaceholderText = "请输入对象名称";
            this.inputName.Size = new System.Drawing.Size(240, 32);
            this.inputName.TabIndex = 2;
            this.inputName.WaveSize = 0;
            // 
            // labelName
            // 
            this.labelName.Location = new System.Drawing.Point(4, 32);
            this.labelName.Margin = new System.Windows.Forms.Padding(0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(240, 22);
            this.labelName.TabIndex = 1;
            this.labelName.Text = "对象名称";
            // 
            // labelSectionBasic
            // 
            this.labelSectionBasic.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionBasic.Location = new System.Drawing.Point(4, 4);
            this.labelSectionBasic.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionBasic.Name = "labelSectionBasic";
            this.labelSectionBasic.Size = new System.Drawing.Size(240, 24);
            this.labelSectionBasic.TabIndex = 0;
            this.labelSectionBasic.Text = "基础信息";
            // 
            // VacuumEditControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelContent);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "VacuumEditControl";
            this.Size = new System.Drawing.Size(836, 431);
            this.panelContent.ResumeLayout(false);
            this.gridMainSections.ResumeLayout(false);
            this.stackSectionRemark.ResumeLayout(false);
            this.stackSectionRemark.PerformLayout();
            this.stackSectionRuntime.ResumeLayout(false);
            this.stackSectionRuntime.PerformLayout();
            this.stackSectionMapping.ResumeLayout(false);
            this.stackSectionBasic.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AntdUI.Panel panelContent;
        private AntdUI.GridPanel gridMainSections;
        private AntdUI.StackPanel stackSectionBasic;
        private AntdUI.Label labelSectionBasic;
        private AntdUI.Label labelName;
        private AntdUI.Input inputName;
        private AntdUI.Label labelDisplayName;
        private AntdUI.Input inputDisplayName;
        private AntdUI.StackPanel stackSectionMapping;
        private AntdUI.Label labelSectionMapping;
        private AntdUI.Label labelVacuumOnOutputBit;
        private AntdUI.Select dropdownVacuumOnOutputBit;
        private AntdUI.Label labelBlowOffOutputBit;
        private AntdUI.Select dropdownBlowOffOutputBit;
        private AntdUI.Label labelVacuumFeedbackBit;
        private AntdUI.Select dropdownVacuumFeedbackBit;
        private AntdUI.StackPanel stackSectionRuntime;
        private AntdUI.Label labelSectionRuntime;
        private AntdUI.Label labelReleaseFeedbackBit;
        private AntdUI.Select dropdownReleaseFeedbackBit;
        private AntdUI.Label labelWorkpiecePresentBit;
        private AntdUI.Select dropdownWorkpiecePresentBit;
        private AntdUI.Checkbox checkUseFeedbackCheck;
        private AntdUI.Checkbox checkUseWorkpieceCheck;
        private AntdUI.Checkbox checkKeepVacuumOnAfterDetected;
        private AntdUI.Label labelVacuumBuildTimeoutMs;
        private AntdUI.Input inputVacuumBuildTimeoutMs;
        private AntdUI.Label labelReleaseTimeoutMs;
        private AntdUI.Input inputReleaseTimeoutMs;
        private AntdUI.StackPanel stackSectionRemark;
        private AntdUI.Label labelSectionRemark;
        private AntdUI.Label labelSortOrder;
        private AntdUI.Input inputSortOrder;
        private AntdUI.Label labelEnabled;
        private AntdUI.Checkbox checkIsEnabled;
        private AntdUI.Label labelDescription;
        private AntdUI.Input inputDescription;
        private AntdUI.Label labelRemark;
        private AntdUI.Input inputRemark;
    }
}