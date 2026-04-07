namespace AMControlWinF.Views.MotionConfig
{
    partial class CylinderEditControl
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
            this.checkAllowBothOn = new AntdUI.Checkbox();
            this.checkAllowBothOff = new AntdUI.Checkbox();
            this.labelSectionRemark = new AntdUI.Label();
            this.stackSectionRuntime = new AntdUI.StackPanel();
            this.inputRetractTimeoutMs = new AntdUI.Input();
            this.labelRetractTimeoutMs = new AntdUI.Label();
            this.inputExtendTimeoutMs = new AntdUI.Input();
            this.labelExtendTimeoutMs = new AntdUI.Label();
            this.checkUseFeedbackCheck = new AntdUI.Checkbox();
            this.dropdownRetractFeedbackBit = new AntdUI.Select();
            this.labelRetractFeedbackBit = new AntdUI.Label();
            this.dropdownRetractOutputBit = new AntdUI.Select();
            this.labelRetractOutputBit = new AntdUI.Label();
            this.labelSectionRuntime = new AntdUI.Label();
            this.stackSectionMapping = new AntdUI.StackPanel();
            this.dropdownExtendFeedbackBit = new AntdUI.Select();
            this.labelExtendFeedbackBit = new AntdUI.Label();
            this.dropdownExtendOutputBit = new AntdUI.Select();
            this.labelExtendOutputBit = new AntdUI.Label();
            this.labelSectionMapping = new AntdUI.Label();
            this.stackSectionBasic = new AntdUI.StackPanel();
            this.dropdownDriveMode = new AntdUI.Select();
            this.labelDriveMode = new AntdUI.Label();
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
            this.stackSectionRemark.Controls.Add(this.checkAllowBothOn);
            this.stackSectionRemark.Controls.Add(this.checkAllowBothOff);
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
            this.inputRemark.Location = new System.Drawing.Point(4, 334);
            this.inputRemark.Margin = new System.Windows.Forms.Padding(0);
            this.inputRemark.Multiline = true;
            this.inputRemark.Name = "inputRemark";
            this.inputRemark.PlaceholderText = "请输入备注";
            this.inputRemark.Size = new System.Drawing.Size(240, 60);
            this.inputRemark.TabIndex = 10;
            this.inputRemark.WaveSize = 0;
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(4, 308);
            this.labelRemark.Margin = new System.Windows.Forms.Padding(0);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(240, 22);
            this.labelRemark.TabIndex = 9;
            this.labelRemark.Text = "备注";
            // 
            // inputDescription
            // 
            this.inputDescription.Location = new System.Drawing.Point(4, 244);
            this.inputDescription.Margin = new System.Windows.Forms.Padding(0);
            this.inputDescription.Multiline = true;
            this.inputDescription.Name = "inputDescription";
            this.inputDescription.PlaceholderText = "请输入描述";
            this.inputDescription.Size = new System.Drawing.Size(240, 60);
            this.inputDescription.TabIndex = 8;
            this.inputDescription.WaveSize = 0;
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(4, 218);
            this.labelDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(240, 22);
            this.labelDescription.TabIndex = 7;
            this.labelDescription.Text = "描述";
            // 
            // checkIsEnabled
            // 
            this.checkIsEnabled.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkIsEnabled.Checked = true;
            this.checkIsEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkIsEnabled.Location = new System.Drawing.Point(4, 186);
            this.checkIsEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.checkIsEnabled.Name = "checkIsEnabled";
            this.checkIsEnabled.Size = new System.Drawing.Size(76, 28);
            this.checkIsEnabled.TabIndex = 6;
            this.checkIsEnabled.Text = "启用对象";
            // 
            // labelEnabled
            // 
            this.labelEnabled.Location = new System.Drawing.Point(4, 160);
            this.labelEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.labelEnabled.Name = "labelEnabled";
            this.labelEnabled.Size = new System.Drawing.Size(240, 22);
            this.labelEnabled.TabIndex = 5;
            this.labelEnabled.Text = "启用状态";
            // 
            // inputSortOrder
            // 
            this.inputSortOrder.Location = new System.Drawing.Point(4, 124);
            this.inputSortOrder.Margin = new System.Windows.Forms.Padding(0);
            this.inputSortOrder.Name = "inputSortOrder";
            this.inputSortOrder.PlaceholderText = "请输入排序号";
            this.inputSortOrder.Size = new System.Drawing.Size(240, 32);
            this.inputSortOrder.TabIndex = 4;
            this.inputSortOrder.WaveSize = 0;
            // 
            // labelSortOrder
            // 
            this.labelSortOrder.Location = new System.Drawing.Point(4, 98);
            this.labelSortOrder.Margin = new System.Windows.Forms.Padding(0);
            this.labelSortOrder.Name = "labelSortOrder";
            this.labelSortOrder.Size = new System.Drawing.Size(240, 22);
            this.labelSortOrder.TabIndex = 3;
            this.labelSortOrder.Text = "排序号";
            // 
            // checkAllowBothOn
            // 
            this.checkAllowBothOn.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkAllowBothOn.Location = new System.Drawing.Point(4, 66);
            this.checkAllowBothOn.Margin = new System.Windows.Forms.Padding(0);
            this.checkAllowBothOn.Name = "checkAllowBothOn";
            this.checkAllowBothOn.Size = new System.Drawing.Size(226, 28);
            this.checkAllowBothOn.TabIndex = 2;
            this.checkAllowBothOn.Text = "允许双输出同时导通（AllowBothOn）";
            // 
            // checkAllowBothOff
            // 
            this.checkAllowBothOff.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkAllowBothOff.Location = new System.Drawing.Point(4, 34);
            this.checkAllowBothOff.Margin = new System.Windows.Forms.Padding(0);
            this.checkAllowBothOff.Name = "checkAllowBothOff";
            this.checkAllowBothOff.Size = new System.Drawing.Size(232, 28);
            this.checkAllowBothOff.TabIndex = 1;
            this.checkAllowBothOff.Text = "允许双输出同时关闭（AllowBothOff）";
            // 
            // labelSectionRemark
            // 
            this.labelSectionRemark.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionRemark.Location = new System.Drawing.Point(4, 4);
            this.labelSectionRemark.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionRemark.Name = "labelSectionRemark";
            this.labelSectionRemark.Size = new System.Drawing.Size(240, 26);
            this.labelSectionRemark.TabIndex = 0;
            this.labelSectionRemark.Text = "高级设置与备注";
            // 
            // stackSectionRuntime
            // 
            this.stackSectionRuntime.AutoScroll = true;
            this.stackSectionRuntime.Controls.Add(this.inputRetractTimeoutMs);
            this.stackSectionRuntime.Controls.Add(this.labelRetractTimeoutMs);
            this.stackSectionRuntime.Controls.Add(this.inputExtendTimeoutMs);
            this.stackSectionRuntime.Controls.Add(this.labelExtendTimeoutMs);
            this.stackSectionRuntime.Controls.Add(this.checkUseFeedbackCheck);
            this.stackSectionRuntime.Controls.Add(this.dropdownRetractFeedbackBit);
            this.stackSectionRuntime.Controls.Add(this.labelRetractFeedbackBit);
            this.stackSectionRuntime.Controls.Add(this.dropdownRetractOutputBit);
            this.stackSectionRuntime.Controls.Add(this.labelRetractOutputBit);
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
            // inputRetractTimeoutMs
            // 
            this.inputRetractTimeoutMs.Location = new System.Drawing.Point(4, 276);
            this.inputRetractTimeoutMs.Margin = new System.Windows.Forms.Padding(0);
            this.inputRetractTimeoutMs.Name = "inputRetractTimeoutMs";
            this.inputRetractTimeoutMs.PlaceholderText = "请输入缩回超时";
            this.inputRetractTimeoutMs.Size = new System.Drawing.Size(158, 32);
            this.inputRetractTimeoutMs.TabIndex = 9;
            this.inputRetractTimeoutMs.WaveSize = 0;
            // 
            // labelRetractTimeoutMs
            // 
            this.labelRetractTimeoutMs.Location = new System.Drawing.Point(4, 250);
            this.labelRetractTimeoutMs.Margin = new System.Windows.Forms.Padding(0);
            this.labelRetractTimeoutMs.Name = "labelRetractTimeoutMs";
            this.labelRetractTimeoutMs.Size = new System.Drawing.Size(158, 22);
            this.labelRetractTimeoutMs.TabIndex = 8;
            this.labelRetractTimeoutMs.Text = "缩回超时(ms)";
            // 
            // inputExtendTimeoutMs
            // 
            this.inputExtendTimeoutMs.Location = new System.Drawing.Point(4, 214);
            this.inputExtendTimeoutMs.Margin = new System.Windows.Forms.Padding(0);
            this.inputExtendTimeoutMs.Name = "inputExtendTimeoutMs";
            this.inputExtendTimeoutMs.PlaceholderText = "请输入伸出超时";
            this.inputExtendTimeoutMs.Size = new System.Drawing.Size(158, 32);
            this.inputExtendTimeoutMs.TabIndex = 7;
            this.inputExtendTimeoutMs.WaveSize = 0;
            // 
            // labelExtendTimeoutMs
            // 
            this.labelExtendTimeoutMs.Location = new System.Drawing.Point(4, 188);
            this.labelExtendTimeoutMs.Margin = new System.Windows.Forms.Padding(0);
            this.labelExtendTimeoutMs.Name = "labelExtendTimeoutMs";
            this.labelExtendTimeoutMs.Size = new System.Drawing.Size(158, 22);
            this.labelExtendTimeoutMs.TabIndex = 6;
            this.labelExtendTimeoutMs.Text = "伸出超时(ms)";
            // 
            // checkUseFeedbackCheck
            // 
            this.checkUseFeedbackCheck.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkUseFeedbackCheck.Location = new System.Drawing.Point(4, 156);
            this.checkUseFeedbackCheck.Margin = new System.Windows.Forms.Padding(0);
            this.checkUseFeedbackCheck.Name = "checkUseFeedbackCheck";
            this.checkUseFeedbackCheck.Size = new System.Drawing.Size(100, 28);
            this.checkUseFeedbackCheck.TabIndex = 5;
            this.checkUseFeedbackCheck.Text = "启用反馈校验";
            // 
            // dropdownRetractFeedbackBit
            // 
            this.dropdownRetractFeedbackBit.Location = new System.Drawing.Point(4, 120);
            this.dropdownRetractFeedbackBit.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownRetractFeedbackBit.Name = "dropdownRetractFeedbackBit";
            this.dropdownRetractFeedbackBit.Size = new System.Drawing.Size(158, 32);
            this.dropdownRetractFeedbackBit.TabIndex = 4;
            this.dropdownRetractFeedbackBit.WaveSize = 0;
            // 
            // labelRetractFeedbackBit
            // 
            this.labelRetractFeedbackBit.Location = new System.Drawing.Point(4, 94);
            this.labelRetractFeedbackBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelRetractFeedbackBit.Name = "labelRetractFeedbackBit";
            this.labelRetractFeedbackBit.Size = new System.Drawing.Size(158, 22);
            this.labelRetractFeedbackBit.TabIndex = 3;
            this.labelRetractFeedbackBit.Text = "缩回反馈位(DI)";
            // 
            // dropdownRetractOutputBit
            // 
            this.dropdownRetractOutputBit.Location = new System.Drawing.Point(4, 58);
            this.dropdownRetractOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownRetractOutputBit.Name = "dropdownRetractOutputBit";
            this.dropdownRetractOutputBit.Size = new System.Drawing.Size(158, 32);
            this.dropdownRetractOutputBit.TabIndex = 2;
            this.dropdownRetractOutputBit.WaveSize = 0;
            // 
            // labelRetractOutputBit
            // 
            this.labelRetractOutputBit.Location = new System.Drawing.Point(4, 32);
            this.labelRetractOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelRetractOutputBit.Name = "labelRetractOutputBit";
            this.labelRetractOutputBit.Size = new System.Drawing.Size(158, 22);
            this.labelRetractOutputBit.TabIndex = 1;
            this.labelRetractOutputBit.Text = "缩回输出位(DO)";
            // 
            // labelSectionRuntime
            // 
            this.labelSectionRuntime.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionRuntime.Location = new System.Drawing.Point(4, 4);
            this.labelSectionRuntime.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionRuntime.Name = "labelSectionRuntime";
            this.labelSectionRuntime.Size = new System.Drawing.Size(158, 24);
            this.labelSectionRuntime.TabIndex = 0;
            this.labelSectionRuntime.Text = "控制参数";
            // 
            // stackSectionMapping
            // 
            this.stackSectionMapping.AutoScroll = true;
            this.stackSectionMapping.Controls.Add(this.dropdownExtendFeedbackBit);
            this.stackSectionMapping.Controls.Add(this.labelExtendFeedbackBit);
            this.stackSectionMapping.Controls.Add(this.dropdownExtendOutputBit);
            this.stackSectionMapping.Controls.Add(this.labelExtendOutputBit);
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
            // dropdownExtendFeedbackBit
            // 
            this.dropdownExtendFeedbackBit.Location = new System.Drawing.Point(4, 120);
            this.dropdownExtendFeedbackBit.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownExtendFeedbackBit.Name = "dropdownExtendFeedbackBit";
            this.dropdownExtendFeedbackBit.Size = new System.Drawing.Size(158, 32);
            this.dropdownExtendFeedbackBit.TabIndex = 4;
            this.dropdownExtendFeedbackBit.WaveSize = 0;
            // 
            // labelExtendFeedbackBit
            // 
            this.labelExtendFeedbackBit.Location = new System.Drawing.Point(4, 94);
            this.labelExtendFeedbackBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelExtendFeedbackBit.Name = "labelExtendFeedbackBit";
            this.labelExtendFeedbackBit.Size = new System.Drawing.Size(158, 22);
            this.labelExtendFeedbackBit.TabIndex = 3;
            this.labelExtendFeedbackBit.Text = "伸出反馈位(DI)";
            // 
            // dropdownExtendOutputBit
            // 
            this.dropdownExtendOutputBit.Location = new System.Drawing.Point(4, 58);
            this.dropdownExtendOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownExtendOutputBit.Name = "dropdownExtendOutputBit";
            this.dropdownExtendOutputBit.Size = new System.Drawing.Size(158, 32);
            this.dropdownExtendOutputBit.TabIndex = 2;
            this.dropdownExtendOutputBit.WaveSize = 0;
            // 
            // labelExtendOutputBit
            // 
            this.labelExtendOutputBit.Location = new System.Drawing.Point(4, 32);
            this.labelExtendOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelExtendOutputBit.Name = "labelExtendOutputBit";
            this.labelExtendOutputBit.Size = new System.Drawing.Size(158, 22);
            this.labelExtendOutputBit.TabIndex = 1;
            this.labelExtendOutputBit.Text = "伸出输出位(DO)";
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
            this.stackSectionBasic.Controls.Add(this.dropdownDriveMode);
            this.stackSectionBasic.Controls.Add(this.labelDriveMode);
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
            // dropdownDriveMode
            // 
            this.dropdownDriveMode.Location = new System.Drawing.Point(4, 182);
            this.dropdownDriveMode.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownDriveMode.Name = "dropdownDriveMode";
            this.dropdownDriveMode.Size = new System.Drawing.Size(240, 32);
            this.dropdownDriveMode.TabIndex = 6;
            this.dropdownDriveMode.WaveSize = 0;
            // 
            // labelDriveMode
            // 
            this.labelDriveMode.Location = new System.Drawing.Point(4, 156);
            this.labelDriveMode.Margin = new System.Windows.Forms.Padding(0);
            this.labelDriveMode.Name = "labelDriveMode";
            this.labelDriveMode.Size = new System.Drawing.Size(240, 22);
            this.labelDriveMode.TabIndex = 5;
            this.labelDriveMode.Text = "驱动模式";
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
            // CylinderEditControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelContent);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "CylinderEditControl";
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
        private AntdUI.Label labelDriveMode;
        private AntdUI.Select dropdownDriveMode;
        private AntdUI.StackPanel stackSectionMapping;
        private AntdUI.Label labelSectionMapping;
        private AntdUI.Label labelExtendOutputBit;
        private AntdUI.Select dropdownExtendOutputBit;
        private AntdUI.Label labelExtendFeedbackBit;
        private AntdUI.Select dropdownExtendFeedbackBit;
        private AntdUI.StackPanel stackSectionRuntime;
        private AntdUI.Label labelSectionRuntime;
        private AntdUI.Label labelRetractOutputBit;
        private AntdUI.Select dropdownRetractOutputBit;
        private AntdUI.Label labelRetractFeedbackBit;
        private AntdUI.Select dropdownRetractFeedbackBit;
        private AntdUI.Checkbox checkUseFeedbackCheck;
        private AntdUI.Label labelExtendTimeoutMs;
        private AntdUI.Input inputExtendTimeoutMs;
        private AntdUI.Label labelRetractTimeoutMs;
        private AntdUI.Input inputRetractTimeoutMs;
        private AntdUI.StackPanel stackSectionRemark;
        private AntdUI.Label labelSectionRemark;
        private AntdUI.Checkbox checkAllowBothOff;
        private AntdUI.Checkbox checkAllowBothOn;
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