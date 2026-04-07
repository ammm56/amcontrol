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
            this.stackRoot = new AntdUI.StackPanel();
            this.inputRemark = new AntdUI.Input();
            this.labelRemark = new AntdUI.Label();
            this.inputDescription = new AntdUI.Input();
            this.labelDescription = new AntdUI.Label();
            this.checkIsEnabled = new AntdUI.Checkbox();
            this.inputSortOrder = new AntdUI.Input();
            this.labelSortOrder = new AntdUI.Label();
            this.checkAllowMultiSegmentOn = new AntdUI.Checkbox();
            this.checkEnableBuzzerOnAlarm = new AntdUI.Checkbox();
            this.checkEnableBuzzerOnWarning = new AntdUI.Checkbox();
            this.dropdownBuzzerOutputBit = new AntdUI.Select();
            this.labelBuzzerOutputBit = new AntdUI.Label();
            this.dropdownBlueOutputBit = new AntdUI.Select();
            this.labelBlueOutputBit = new AntdUI.Label();
            this.dropdownGreenOutputBit = new AntdUI.Select();
            this.labelGreenOutputBit = new AntdUI.Label();
            this.dropdownYellowOutputBit = new AntdUI.Select();
            this.labelYellowOutputBit = new AntdUI.Label();
            this.dropdownRedOutputBit = new AntdUI.Select();
            this.labelRedOutputBit = new AntdUI.Label();
            this.inputDisplayName = new AntdUI.Input();
            this.labelDisplayName = new AntdUI.Label();
            this.inputName = new AntdUI.Input();
            this.labelName = new AntdUI.Label();
            this.stackRoot.SuspendLayout();
            this.SuspendLayout();
            // 
            // stackRoot
            // 
            this.stackRoot.AutoScroll = true;
            this.stackRoot.Controls.Add(this.inputRemark);
            this.stackRoot.Controls.Add(this.labelRemark);
            this.stackRoot.Controls.Add(this.inputDescription);
            this.stackRoot.Controls.Add(this.labelDescription);
            this.stackRoot.Controls.Add(this.checkIsEnabled);
            this.stackRoot.Controls.Add(this.inputSortOrder);
            this.stackRoot.Controls.Add(this.labelSortOrder);
            this.stackRoot.Controls.Add(this.checkAllowMultiSegmentOn);
            this.stackRoot.Controls.Add(this.checkEnableBuzzerOnAlarm);
            this.stackRoot.Controls.Add(this.checkEnableBuzzerOnWarning);
            this.stackRoot.Controls.Add(this.dropdownBuzzerOutputBit);
            this.stackRoot.Controls.Add(this.labelBuzzerOutputBit);
            this.stackRoot.Controls.Add(this.dropdownBlueOutputBit);
            this.stackRoot.Controls.Add(this.labelBlueOutputBit);
            this.stackRoot.Controls.Add(this.dropdownGreenOutputBit);
            this.stackRoot.Controls.Add(this.labelGreenOutputBit);
            this.stackRoot.Controls.Add(this.dropdownYellowOutputBit);
            this.stackRoot.Controls.Add(this.labelYellowOutputBit);
            this.stackRoot.Controls.Add(this.dropdownRedOutputBit);
            this.stackRoot.Controls.Add(this.labelRedOutputBit);
            this.stackRoot.Controls.Add(this.inputDisplayName);
            this.stackRoot.Controls.Add(this.labelDisplayName);
            this.stackRoot.Controls.Add(this.inputName);
            this.stackRoot.Controls.Add(this.labelName);
            this.stackRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stackRoot.Gap = 4;
            this.stackRoot.Location = new System.Drawing.Point(0, 0);
            this.stackRoot.Margin = new System.Windows.Forms.Padding(0);
            this.stackRoot.Name = "stackRoot";
            this.stackRoot.Padding = new System.Windows.Forms.Padding(8);
            this.stackRoot.Size = new System.Drawing.Size(836, 452);
            this.stackRoot.TabIndex = 0;
            this.stackRoot.Text = "stackRoot";
            this.stackRoot.Vertical = true;
            // 
            // inputRemark
            // 
            this.inputRemark.Location = new System.Drawing.Point(8, 576);
            this.inputRemark.Margin = new System.Windows.Forms.Padding(0);
            this.inputRemark.Multiline = true;
            this.inputRemark.Name = "inputRemark";
            this.inputRemark.PlaceholderText = "请输入备注";
            this.inputRemark.Size = new System.Drawing.Size(820, 78);
            this.inputRemark.TabIndex = 23;
            this.inputRemark.WaveSize = 0;
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(8, 550);
            this.labelRemark.Margin = new System.Windows.Forms.Padding(0);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(820, 22);
            this.labelRemark.TabIndex = 22;
            this.labelRemark.Text = "备注";
            // 
            // inputDescription
            // 
            this.inputDescription.Location = new System.Drawing.Point(8, 468);
            this.inputDescription.Margin = new System.Windows.Forms.Padding(0);
            this.inputDescription.Multiline = true;
            this.inputDescription.Name = "inputDescription";
            this.inputDescription.PlaceholderText = "请输入描述";
            this.inputDescription.Size = new System.Drawing.Size(820, 78);
            this.inputDescription.TabIndex = 21;
            this.inputDescription.WaveSize = 0;
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(8, 442);
            this.labelDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(820, 22);
            this.labelDescription.TabIndex = 20;
            this.labelDescription.Text = "描述";
            // 
            // checkIsEnabled
            // 
            this.checkIsEnabled.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkIsEnabled.Checked = true;
            this.checkIsEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkIsEnabled.Location = new System.Drawing.Point(8, 410);
            this.checkIsEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.checkIsEnabled.Name = "checkIsEnabled";
            this.checkIsEnabled.Size = new System.Drawing.Size(88, 28);
            this.checkIsEnabled.TabIndex = 19;
            this.checkIsEnabled.Text = "启用对象";
            // 
            // inputSortOrder
            // 
            this.inputSortOrder.Location = new System.Drawing.Point(8, 374);
            this.inputSortOrder.Margin = new System.Windows.Forms.Padding(0);
            this.inputSortOrder.Name = "inputSortOrder";
            this.inputSortOrder.PlaceholderText = "请输入排序号";
            this.inputSortOrder.Size = new System.Drawing.Size(820, 32);
            this.inputSortOrder.TabIndex = 18;
            this.inputSortOrder.WaveSize = 0;
            // 
            // labelSortOrder
            // 
            this.labelSortOrder.Location = new System.Drawing.Point(8, 348);
            this.labelSortOrder.Margin = new System.Windows.Forms.Padding(0);
            this.labelSortOrder.Name = "labelSortOrder";
            this.labelSortOrder.Size = new System.Drawing.Size(820, 22);
            this.labelSortOrder.TabIndex = 17;
            this.labelSortOrder.Text = "排序号";
            // 
            // checkAllowMultiSegmentOn
            // 
            this.checkAllowMultiSegmentOn.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkAllowMultiSegmentOn.Location = new System.Drawing.Point(8, 316);
            this.checkAllowMultiSegmentOn.Margin = new System.Windows.Forms.Padding(0);
            this.checkAllowMultiSegmentOn.Name = "checkAllowMultiSegmentOn";
            this.checkAllowMultiSegmentOn.Size = new System.Drawing.Size(94, 28);
            this.checkAllowMultiSegmentOn.TabIndex = 16;
            this.checkAllowMultiSegmentOn.Text = "允许多段同亮";
            // 
            // checkEnableBuzzerOnAlarm
            // 
            this.checkEnableBuzzerOnAlarm.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkEnableBuzzerOnAlarm.Location = new System.Drawing.Point(8, 284);
            this.checkEnableBuzzerOnAlarm.Margin = new System.Windows.Forms.Padding(0);
            this.checkEnableBuzzerOnAlarm.Name = "checkEnableBuzzerOnAlarm";
            this.checkEnableBuzzerOnAlarm.Size = new System.Drawing.Size(94, 28);
            this.checkEnableBuzzerOnAlarm.TabIndex = 15;
            this.checkEnableBuzzerOnAlarm.Text = "报警状态蜂鸣";
            // 
            // checkEnableBuzzerOnWarning
            // 
            this.checkEnableBuzzerOnWarning.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkEnableBuzzerOnWarning.Location = new System.Drawing.Point(8, 252);
            this.checkEnableBuzzerOnWarning.Margin = new System.Windows.Forms.Padding(0);
            this.checkEnableBuzzerOnWarning.Name = "checkEnableBuzzerOnWarning";
            this.checkEnableBuzzerOnWarning.Size = new System.Drawing.Size(94, 28);
            this.checkEnableBuzzerOnWarning.TabIndex = 14;
            this.checkEnableBuzzerOnWarning.Text = "警告状态蜂鸣";
            // 
            // dropdownBuzzerOutputBit
            // 
            this.dropdownBuzzerOutputBit.Location = new System.Drawing.Point(8, 216);
            this.dropdownBuzzerOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownBuzzerOutputBit.Name = "dropdownBuzzerOutputBit";
            this.dropdownBuzzerOutputBit.Size = new System.Drawing.Size(820, 32);
            this.dropdownBuzzerOutputBit.TabIndex = 13;
            this.dropdownBuzzerOutputBit.WaveSize = 0;
            // 
            // labelBuzzerOutputBit
            // 
            this.labelBuzzerOutputBit.Location = new System.Drawing.Point(8, 190);
            this.labelBuzzerOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelBuzzerOutputBit.Name = "labelBuzzerOutputBit";
            this.labelBuzzerOutputBit.Size = new System.Drawing.Size(820, 22);
            this.labelBuzzerOutputBit.TabIndex = 12;
            this.labelBuzzerOutputBit.Text = "蜂鸣器输出位";
            // 
            // dropdownBlueOutputBit
            // 
            this.dropdownBlueOutputBit.Location = new System.Drawing.Point(8, 154);
            this.dropdownBlueOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownBlueOutputBit.Name = "dropdownBlueOutputBit";
            this.dropdownBlueOutputBit.Size = new System.Drawing.Size(820, 32);
            this.dropdownBlueOutputBit.TabIndex = 11;
            this.dropdownBlueOutputBit.WaveSize = 0;
            // 
            // labelBlueOutputBit
            // 
            this.labelBlueOutputBit.Location = new System.Drawing.Point(8, 128);
            this.labelBlueOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelBlueOutputBit.Name = "labelBlueOutputBit";
            this.labelBlueOutputBit.Size = new System.Drawing.Size(820, 22);
            this.labelBlueOutputBit.TabIndex = 10;
            this.labelBlueOutputBit.Text = "蓝灯输出位";
            // 
            // dropdownGreenOutputBit
            // 
            this.dropdownGreenOutputBit.Location = new System.Drawing.Point(8, 92);
            this.dropdownGreenOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownGreenOutputBit.Name = "dropdownGreenOutputBit";
            this.dropdownGreenOutputBit.Size = new System.Drawing.Size(820, 32);
            this.dropdownGreenOutputBit.TabIndex = 9;
            this.dropdownGreenOutputBit.WaveSize = 0;
            // 
            // labelGreenOutputBit
            // 
            this.labelGreenOutputBit.Location = new System.Drawing.Point(8, 66);
            this.labelGreenOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelGreenOutputBit.Name = "labelGreenOutputBit";
            this.labelGreenOutputBit.Size = new System.Drawing.Size(820, 22);
            this.labelGreenOutputBit.TabIndex = 8;
            this.labelGreenOutputBit.Text = "绿灯输出位";
            // 
            // dropdownYellowOutputBit
            // 
            this.dropdownYellowOutputBit.Location = new System.Drawing.Point(8, 30);
            this.dropdownYellowOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownYellowOutputBit.Name = "dropdownYellowOutputBit";
            this.dropdownYellowOutputBit.Size = new System.Drawing.Size(820, 32);
            this.dropdownYellowOutputBit.TabIndex = 7;
            this.dropdownYellowOutputBit.WaveSize = 0;
            // 
            // labelYellowOutputBit
            // 
            this.labelYellowOutputBit.Location = new System.Drawing.Point(8, 4);
            this.labelYellowOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelYellowOutputBit.Name = "labelYellowOutputBit";
            this.labelYellowOutputBit.Size = new System.Drawing.Size(820, 22);
            this.labelYellowOutputBit.TabIndex = 6;
            this.labelYellowOutputBit.Text = "黄灯输出位";
            // 
            // dropdownRedOutputBit
            // 
            this.dropdownRedOutputBit.Location = new System.Drawing.Point(8, 154);
            this.dropdownRedOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownRedOutputBit.Name = "dropdownRedOutputBit";
            this.dropdownRedOutputBit.Size = new System.Drawing.Size(820, 32);
            this.dropdownRedOutputBit.TabIndex = 5;
            this.dropdownRedOutputBit.WaveSize = 0;
            // 
            // labelRedOutputBit
            // 
            this.labelRedOutputBit.Location = new System.Drawing.Point(8, 128);
            this.labelRedOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelRedOutputBit.Name = "labelRedOutputBit";
            this.labelRedOutputBit.Size = new System.Drawing.Size(820, 22);
            this.labelRedOutputBit.TabIndex = 4;
            this.labelRedOutputBit.Text = "红灯输出位";
            // 
            // inputDisplayName
            // 
            this.inputDisplayName.Location = new System.Drawing.Point(8, 92);
            this.inputDisplayName.Margin = new System.Windows.Forms.Padding(0);
            this.inputDisplayName.Name = "inputDisplayName";
            this.inputDisplayName.PlaceholderText = "请输入显示名称";
            this.inputDisplayName.Size = new System.Drawing.Size(820, 32);
            this.inputDisplayName.TabIndex = 3;
            this.inputDisplayName.WaveSize = 0;
            // 
            // labelDisplayName
            // 
            this.labelDisplayName.Location = new System.Drawing.Point(8, 66);
            this.labelDisplayName.Margin = new System.Windows.Forms.Padding(0);
            this.labelDisplayName.Name = "labelDisplayName";
            this.labelDisplayName.Size = new System.Drawing.Size(820, 22);
            this.labelDisplayName.TabIndex = 2;
            this.labelDisplayName.Text = "显示名称";
            // 
            // inputName
            // 
            this.inputName.Location = new System.Drawing.Point(8, 30);
            this.inputName.Margin = new System.Windows.Forms.Padding(0);
            this.inputName.Name = "inputName";
            this.inputName.PlaceholderText = "请输入对象名称";
            this.inputName.Size = new System.Drawing.Size(820, 32);
            this.inputName.TabIndex = 1;
            this.inputName.WaveSize = 0;
            // 
            // labelName
            // 
            this.labelName.Location = new System.Drawing.Point(8, 4);
            this.labelName.Margin = new System.Windows.Forms.Padding(0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(820, 22);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "对象名称";
            // 
            // StackLightEditControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.stackRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "StackLightEditControl";
            this.Size = new System.Drawing.Size(836, 452);
            this.stackRoot.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private AntdUI.StackPanel stackRoot;
        private AntdUI.Label labelName;
        private AntdUI.Input inputName;
        private AntdUI.Label labelDisplayName;
        private AntdUI.Input inputDisplayName;
        private AntdUI.Label labelRedOutputBit;
        private AntdUI.Select dropdownRedOutputBit;
        private AntdUI.Label labelYellowOutputBit;
        private AntdUI.Select dropdownYellowOutputBit;
        private AntdUI.Label labelGreenOutputBit;
        private AntdUI.Select dropdownGreenOutputBit;
        private AntdUI.Label labelBlueOutputBit;
        private AntdUI.Select dropdownBlueOutputBit;
        private AntdUI.Label labelBuzzerOutputBit;
        private AntdUI.Select dropdownBuzzerOutputBit;
        private AntdUI.Checkbox checkEnableBuzzerOnWarning;
        private AntdUI.Checkbox checkEnableBuzzerOnAlarm;
        private AntdUI.Checkbox checkAllowMultiSegmentOn;
        private AntdUI.Label labelSortOrder;
        private AntdUI.Input inputSortOrder;
        private AntdUI.Checkbox checkIsEnabled;
        private AntdUI.Label labelDescription;
        private AntdUI.Input inputDescription;
        private AntdUI.Label labelRemark;
        private AntdUI.Input inputRemark;
    }
}