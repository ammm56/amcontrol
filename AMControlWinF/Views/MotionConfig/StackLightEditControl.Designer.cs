namespace AMControlWinF.Views.MotionConfig
{
    partial class StackLightEditControl
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
            this.checkAllowMultiSegmentOn = new AntdUI.Checkbox();
            this.checkEnableBuzzerOnAlarm = new AntdUI.Checkbox();
            this.checkEnableBuzzerOnWarning = new AntdUI.Checkbox();
            this.dropdownBuzzerOutputBit = new AntdUI.Select();
            this.labelBuzzerOutputBit = new AntdUI.Label();
            this.dropdownBlueOutputBit = new AntdUI.Select();
            this.labelBlueOutputBit = new AntdUI.Label();
            this.dropdownGreenOutputBit = new AntdUI.Select();
            this.labelGreenOutputBit = new AntdUI.Label();
            this.labelSectionRuntime = new AntdUI.Label();
            this.stackSectionMapping = new AntdUI.StackPanel();
            this.dropdownYellowOutputBit = new AntdUI.Select();
            this.labelYellowOutputBit = new AntdUI.Label();
            this.dropdownRedOutputBit = new AntdUI.Select();
            this.labelRedOutputBit = new AntdUI.Label();
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
            this.stackSectionRemark.Location = new System.Drawing.Point(579, 0);
            this.stackSectionRemark.Margin = new System.Windows.Forms.Padding(0);
            this.stackSectionRemark.Name = "stackSectionRemark";
            this.stackSectionRemark.Padding = new System.Windows.Forms.Padding(4);
            this.stackSectionRemark.Size = new System.Drawing.Size(249, 431);
            this.stackSectionRemark.TabIndex = 3;
            this.stackSectionRemark.Text = "stackSectionRemark";
            this.stackSectionRemark.Vertical = true;
            // 
            // inputRemark
            // 
            this.inputRemark.Location = new System.Drawing.Point(4, 306);
            this.inputRemark.Margin = new System.Windows.Forms.Padding(0);
            this.inputRemark.Multiline = true;
            this.inputRemark.Name = "inputRemark";
            this.inputRemark.PlaceholderText = "请输入备注";
            this.inputRemark.Size = new System.Drawing.Size(241, 88);
            this.inputRemark.TabIndex = 8;
            this.inputRemark.WaveSize = 0;
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(4, 280);
            this.labelRemark.Margin = new System.Windows.Forms.Padding(0);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(241, 22);
            this.labelRemark.TabIndex = 7;
            this.labelRemark.Text = "备注";
            // 
            // inputDescription
            // 
            this.inputDescription.Location = new System.Drawing.Point(4, 186);
            this.inputDescription.Margin = new System.Windows.Forms.Padding(0);
            this.inputDescription.Multiline = true;
            this.inputDescription.Name = "inputDescription";
            this.inputDescription.PlaceholderText = "请输入描述";
            this.inputDescription.Size = new System.Drawing.Size(241, 90);
            this.inputDescription.TabIndex = 6;
            this.inputDescription.WaveSize = 0;
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(4, 160);
            this.labelDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(241, 22);
            this.labelDescription.TabIndex = 5;
            this.labelDescription.Text = "描述";
            // 
            // checkIsEnabled
            // 
            this.checkIsEnabled.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkIsEnabled.Checked = true;
            this.checkIsEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkIsEnabled.Location = new System.Drawing.Point(4, 128);
            this.checkIsEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.checkIsEnabled.Name = "checkIsEnabled";
            this.checkIsEnabled.Size = new System.Drawing.Size(88, 28);
            this.checkIsEnabled.TabIndex = 4;
            this.checkIsEnabled.Text = "启用对象";
            // 
            // labelEnabled
            // 
            this.labelEnabled.Location = new System.Drawing.Point(4, 102);
            this.labelEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.labelEnabled.Name = "labelEnabled";
            this.labelEnabled.Size = new System.Drawing.Size(241, 22);
            this.labelEnabled.TabIndex = 3;
            this.labelEnabled.Text = "启用状态";
            // 
            // inputSortOrder
            // 
            this.inputSortOrder.Location = new System.Drawing.Point(4, 66);
            this.inputSortOrder.Margin = new System.Windows.Forms.Padding(0);
            this.inputSortOrder.Name = "inputSortOrder";
            this.inputSortOrder.PlaceholderText = "请输入排序号";
            this.inputSortOrder.Size = new System.Drawing.Size(241, 32);
            this.inputSortOrder.TabIndex = 2;
            this.inputSortOrder.WaveSize = 0;
            // 
            // labelSortOrder
            // 
            this.labelSortOrder.Location = new System.Drawing.Point(4, 40);
            this.labelSortOrder.Margin = new System.Windows.Forms.Padding(0);
            this.labelSortOrder.Name = "labelSortOrder";
            this.labelSortOrder.Size = new System.Drawing.Size(241, 22);
            this.labelSortOrder.TabIndex = 1;
            this.labelSortOrder.Text = "排序号";
            // 
            // labelSectionRemark
            // 
            this.labelSectionRemark.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionRemark.Location = new System.Drawing.Point(4, 4);
            this.labelSectionRemark.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionRemark.Name = "labelSectionRemark";
            this.labelSectionRemark.Size = new System.Drawing.Size(241, 26);
            this.labelSectionRemark.TabIndex = 0;
            this.labelSectionRemark.Text = "状态与备注";
            // 
            // stackSectionRuntime
            // 
            this.stackSectionRuntime.AutoScroll = true;
            this.stackSectionRuntime.Controls.Add(this.checkAllowMultiSegmentOn);
            this.stackSectionRuntime.Controls.Add(this.checkEnableBuzzerOnAlarm);
            this.stackSectionRuntime.Controls.Add(this.checkEnableBuzzerOnWarning);
            this.stackSectionRuntime.Controls.Add(this.dropdownBuzzerOutputBit);
            this.stackSectionRuntime.Controls.Add(this.labelBuzzerOutputBit);
            this.stackSectionRuntime.Controls.Add(this.dropdownBlueOutputBit);
            this.stackSectionRuntime.Controls.Add(this.labelBlueOutputBit);
            this.stackSectionRuntime.Controls.Add(this.dropdownGreenOutputBit);
            this.stackSectionRuntime.Controls.Add(this.labelGreenOutputBit);
            this.stackSectionRuntime.Controls.Add(this.labelSectionRuntime);
            this.stackSectionRuntime.Gap = 4;
            this.stackSectionRuntime.Location = new System.Drawing.Point(414, 0);
            this.stackSectionRuntime.Margin = new System.Windows.Forms.Padding(0);
            this.stackSectionRuntime.Name = "stackSectionRuntime";
            this.stackSectionRuntime.Padding = new System.Windows.Forms.Padding(4);
            this.stackSectionRuntime.Size = new System.Drawing.Size(165, 431);
            this.stackSectionRuntime.TabIndex = 2;
            this.stackSectionRuntime.Text = "stackSectionRuntime";
            this.stackSectionRuntime.Vertical = true;
            // 
            // checkAllowMultiSegmentOn
            // 
            this.checkAllowMultiSegmentOn.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkAllowMultiSegmentOn.Location = new System.Drawing.Point(4, 254);
            this.checkAllowMultiSegmentOn.Margin = new System.Windows.Forms.Padding(0);
            this.checkAllowMultiSegmentOn.Name = "checkAllowMultiSegmentOn";
            this.checkAllowMultiSegmentOn.Size = new System.Drawing.Size(94, 28);
            this.checkAllowMultiSegmentOn.TabIndex = 9;
            this.checkAllowMultiSegmentOn.Text = "允许多段同亮";
            // 
            // checkEnableBuzzerOnAlarm
            // 
            this.checkEnableBuzzerOnAlarm.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkEnableBuzzerOnAlarm.Location = new System.Drawing.Point(4, 222);
            this.checkEnableBuzzerOnAlarm.Margin = new System.Windows.Forms.Padding(0);
            this.checkEnableBuzzerOnAlarm.Name = "checkEnableBuzzerOnAlarm";
            this.checkEnableBuzzerOnAlarm.Size = new System.Drawing.Size(94, 28);
            this.checkEnableBuzzerOnAlarm.TabIndex = 8;
            this.checkEnableBuzzerOnAlarm.Text = "报警状态蜂鸣";
            // 
            // checkEnableBuzzerOnWarning
            // 
            this.checkEnableBuzzerOnWarning.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkEnableBuzzerOnWarning.Location = new System.Drawing.Point(4, 190);
            this.checkEnableBuzzerOnWarning.Margin = new System.Windows.Forms.Padding(0);
            this.checkEnableBuzzerOnWarning.Name = "checkEnableBuzzerOnWarning";
            this.checkEnableBuzzerOnWarning.Size = new System.Drawing.Size(94, 28);
            this.checkEnableBuzzerOnWarning.TabIndex = 7;
            this.checkEnableBuzzerOnWarning.Text = "警告状态蜂鸣";
            // 
            // dropdownBuzzerOutputBit
            // 
            this.dropdownBuzzerOutputBit.Location = new System.Drawing.Point(4, 154);
            this.dropdownBuzzerOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownBuzzerOutputBit.Name = "dropdownBuzzerOutputBit";
            this.dropdownBuzzerOutputBit.Size = new System.Drawing.Size(157, 32);
            this.dropdownBuzzerOutputBit.TabIndex = 6;
            this.dropdownBuzzerOutputBit.WaveSize = 0;
            // 
            // labelBuzzerOutputBit
            // 
            this.labelBuzzerOutputBit.Location = new System.Drawing.Point(4, 128);
            this.labelBuzzerOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelBuzzerOutputBit.Name = "labelBuzzerOutputBit";
            this.labelBuzzerOutputBit.Size = new System.Drawing.Size(157, 22);
            this.labelBuzzerOutputBit.TabIndex = 5;
            this.labelBuzzerOutputBit.Text = "蜂鸣器输出位(DO)";
            // 
            // dropdownBlueOutputBit
            // 
            this.dropdownBlueOutputBit.Location = new System.Drawing.Point(4, 92);
            this.dropdownBlueOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownBlueOutputBit.Name = "dropdownBlueOutputBit";
            this.dropdownBlueOutputBit.Size = new System.Drawing.Size(157, 32);
            this.dropdownBlueOutputBit.TabIndex = 4;
            this.dropdownBlueOutputBit.WaveSize = 0;
            // 
            // labelBlueOutputBit
            // 
            this.labelBlueOutputBit.Location = new System.Drawing.Point(4, 66);
            this.labelBlueOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelBlueOutputBit.Name = "labelBlueOutputBit";
            this.labelBlueOutputBit.Size = new System.Drawing.Size(157, 22);
            this.labelBlueOutputBit.TabIndex = 3;
            this.labelBlueOutputBit.Text = "蓝灯输出位(DO)";
            // 
            // dropdownGreenOutputBit
            // 
            this.dropdownGreenOutputBit.Location = new System.Drawing.Point(4, 30);
            this.dropdownGreenOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownGreenOutputBit.Name = "dropdownGreenOutputBit";
            this.dropdownGreenOutputBit.Size = new System.Drawing.Size(157, 32);
            this.dropdownGreenOutputBit.TabIndex = 2;
            this.dropdownGreenOutputBit.WaveSize = 0;
            // 
            // labelGreenOutputBit
            // 
            this.labelGreenOutputBit.Location = new System.Drawing.Point(4, 4);
            this.labelGreenOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelGreenOutputBit.Name = "labelGreenOutputBit";
            this.labelGreenOutputBit.Size = new System.Drawing.Size(157, 22);
            this.labelGreenOutputBit.TabIndex = 1;
            this.labelGreenOutputBit.Text = "绿灯输出位(DO)";
            // 
            // labelSectionRuntime
            // 
            this.labelSectionRuntime.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionRuntime.Location = new System.Drawing.Point(4, 286);
            this.labelSectionRuntime.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionRuntime.Name = "labelSectionRuntime";
            this.labelSectionRuntime.Size = new System.Drawing.Size(157, 24);
            this.labelSectionRuntime.TabIndex = 10;
            this.labelSectionRuntime.Text = "联动参数";
            // 
            // stackSectionMapping
            // 
            this.stackSectionMapping.AutoScroll = true;
            this.stackSectionMapping.Controls.Add(this.dropdownYellowOutputBit);
            this.stackSectionMapping.Controls.Add(this.labelYellowOutputBit);
            this.stackSectionMapping.Controls.Add(this.dropdownRedOutputBit);
            this.stackSectionMapping.Controls.Add(this.labelRedOutputBit);
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
            // dropdownYellowOutputBit
            // 
            this.dropdownYellowOutputBit.Location = new System.Drawing.Point(4, 120);
            this.dropdownYellowOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownYellowOutputBit.Name = "dropdownYellowOutputBit";
            this.dropdownYellowOutputBit.Size = new System.Drawing.Size(158, 32);
            this.dropdownYellowOutputBit.TabIndex = 4;
            this.dropdownYellowOutputBit.WaveSize = 0;
            // 
            // labelYellowOutputBit
            // 
            this.labelYellowOutputBit.Location = new System.Drawing.Point(4, 94);
            this.labelYellowOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelYellowOutputBit.Name = "labelYellowOutputBit";
            this.labelYellowOutputBit.Size = new System.Drawing.Size(158, 22);
            this.labelYellowOutputBit.TabIndex = 3;
            this.labelYellowOutputBit.Text = "黄灯输出位(DO)";
            // 
            // dropdownRedOutputBit
            // 
            this.dropdownRedOutputBit.Location = new System.Drawing.Point(4, 58);
            this.dropdownRedOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownRedOutputBit.Name = "dropdownRedOutputBit";
            this.dropdownRedOutputBit.Size = new System.Drawing.Size(158, 32);
            this.dropdownRedOutputBit.TabIndex = 2;
            this.dropdownRedOutputBit.WaveSize = 0;
            // 
            // labelRedOutputBit
            // 
            this.labelRedOutputBit.Location = new System.Drawing.Point(4, 32);
            this.labelRedOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelRedOutputBit.Name = "labelRedOutputBit";
            this.labelRedOutputBit.Size = new System.Drawing.Size(158, 22);
            this.labelRedOutputBit.TabIndex = 1;
            this.labelRedOutputBit.Text = "红灯输出位(DO)";
            // 
            // labelSectionMapping
            // 
            this.labelSectionMapping.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionMapping.Location = new System.Drawing.Point(4, 4);
            this.labelSectionMapping.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionMapping.Name = "labelSectionMapping";
            this.labelSectionMapping.Size = new System.Drawing.Size(158, 24);
            this.labelSectionMapping.TabIndex = 0;
            this.labelSectionMapping.Text = "灯色映射";
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
            // StackLightEditControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelContent);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "StackLightEditControl";
            this.Size = new System.Drawing.Size(836, 431);
            this.panelContent.ResumeLayout(false);
            this.gridMainSections.ResumeLayout(false);
            this.stackSectionRemark.ResumeLayout(false);
            this.stackSectionRuntime.ResumeLayout(false);
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
        private AntdUI.Label labelRedOutputBit;
        private AntdUI.Select dropdownRedOutputBit;
        private AntdUI.Label labelYellowOutputBit;
        private AntdUI.Select dropdownYellowOutputBit;
        private AntdUI.StackPanel stackSectionRuntime;
        private AntdUI.Label labelSectionRuntime;
        private AntdUI.Label labelGreenOutputBit;
        private AntdUI.Select dropdownGreenOutputBit;
        private AntdUI.Label labelBlueOutputBit;
        private AntdUI.Select dropdownBlueOutputBit;
        private AntdUI.Label labelBuzzerOutputBit;
        private AntdUI.Select dropdownBuzzerOutputBit;
        private AntdUI.Checkbox checkEnableBuzzerOnWarning;
        private AntdUI.Checkbox checkEnableBuzzerOnAlarm;
        private AntdUI.Checkbox checkAllowMultiSegmentOn;
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