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
            this.stackRoot = new AntdUI.StackPanel();
            this.inputRemark = new AntdUI.Input();
            this.labelRemark = new AntdUI.Label();
            this.inputDescription = new AntdUI.Input();
            this.labelDescription = new AntdUI.Label();
            this.checkIsEnabled = new AntdUI.Checkbox();
            this.inputSortOrder = new AntdUI.Input();
            this.labelSortOrder = new AntdUI.Label();
            this.checkAllowBothOn = new AntdUI.Checkbox();
            this.checkAllowBothOff = new AntdUI.Checkbox();
            this.inputRetractTimeoutMs = new AntdUI.Input();
            this.labelRetractTimeoutMs = new AntdUI.Label();
            this.inputExtendTimeoutMs = new AntdUI.Input();
            this.labelExtendTimeoutMs = new AntdUI.Label();
            this.checkUseFeedbackCheck = new AntdUI.Checkbox();
            this.dropdownRetractFeedbackBit = new AntdUI.Select();
            this.labelRetractFeedbackBit = new AntdUI.Label();
            this.dropdownExtendFeedbackBit = new AntdUI.Select();
            this.labelExtendFeedbackBit = new AntdUI.Label();
            this.dropdownRetractOutputBit = new AntdUI.Select();
            this.labelRetractOutputBit = new AntdUI.Label();
            this.dropdownExtendOutputBit = new AntdUI.Select();
            this.labelExtendOutputBit = new AntdUI.Label();
            this.dropdownDriveMode = new AntdUI.Select();
            this.labelDriveMode = new AntdUI.Label();
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
            this.stackRoot.Controls.Add(this.checkAllowBothOn);
            this.stackRoot.Controls.Add(this.checkAllowBothOff);
            this.stackRoot.Controls.Add(this.inputRetractTimeoutMs);
            this.stackRoot.Controls.Add(this.labelRetractTimeoutMs);
            this.stackRoot.Controls.Add(this.inputExtendTimeoutMs);
            this.stackRoot.Controls.Add(this.labelExtendTimeoutMs);
            this.stackRoot.Controls.Add(this.checkUseFeedbackCheck);
            this.stackRoot.Controls.Add(this.dropdownRetractFeedbackBit);
            this.stackRoot.Controls.Add(this.labelRetractFeedbackBit);
            this.stackRoot.Controls.Add(this.dropdownExtendFeedbackBit);
            this.stackRoot.Controls.Add(this.labelExtendFeedbackBit);
            this.stackRoot.Controls.Add(this.dropdownRetractOutputBit);
            this.stackRoot.Controls.Add(this.labelRetractOutputBit);
            this.stackRoot.Controls.Add(this.dropdownExtendOutputBit);
            this.stackRoot.Controls.Add(this.labelExtendOutputBit);
            this.stackRoot.Controls.Add(this.dropdownDriveMode);
            this.stackRoot.Controls.Add(this.labelDriveMode);
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
            this.inputRemark.Location = new System.Drawing.Point(8, 700);
            this.inputRemark.Margin = new System.Windows.Forms.Padding(0);
            this.inputRemark.Multiline = true;
            this.inputRemark.Name = "inputRemark";
            this.inputRemark.PlaceholderText = "请输入备注";
            this.inputRemark.Size = new System.Drawing.Size(820, 78);
            this.inputRemark.TabIndex = 27;
            this.inputRemark.WaveSize = 0;
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(8, 674);
            this.labelRemark.Margin = new System.Windows.Forms.Padding(0);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(820, 22);
            this.labelRemark.TabIndex = 26;
            this.labelRemark.Text = "备注";
            // 
            // inputDescription
            // 
            this.inputDescription.Location = new System.Drawing.Point(8, 592);
            this.inputDescription.Margin = new System.Windows.Forms.Padding(0);
            this.inputDescription.Multiline = true;
            this.inputDescription.Name = "inputDescription";
            this.inputDescription.PlaceholderText = "请输入描述";
            this.inputDescription.Size = new System.Drawing.Size(820, 78);
            this.inputDescription.TabIndex = 25;
            this.inputDescription.WaveSize = 0;
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(8, 566);
            this.labelDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(820, 22);
            this.labelDescription.TabIndex = 24;
            this.labelDescription.Text = "描述";
            // 
            // checkIsEnabled
            // 
            this.checkIsEnabled.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkIsEnabled.Checked = true;
            this.checkIsEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkIsEnabled.Location = new System.Drawing.Point(8, 534);
            this.checkIsEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.checkIsEnabled.Name = "checkIsEnabled";
            this.checkIsEnabled.Size = new System.Drawing.Size(88, 28);
            this.checkIsEnabled.TabIndex = 23;
            this.checkIsEnabled.Text = "启用对象";
            // 
            // inputSortOrder
            // 
            this.inputSortOrder.Location = new System.Drawing.Point(8, 498);
            this.inputSortOrder.Margin = new System.Windows.Forms.Padding(0);
            this.inputSortOrder.Name = "inputSortOrder";
            this.inputSortOrder.PlaceholderText = "请输入排序号";
            this.inputSortOrder.Size = new System.Drawing.Size(820, 32);
            this.inputSortOrder.TabIndex = 22;
            this.inputSortOrder.WaveSize = 0;
            // 
            // labelSortOrder
            // 
            this.labelSortOrder.Location = new System.Drawing.Point(8, 472);
            this.labelSortOrder.Margin = new System.Windows.Forms.Padding(0);
            this.labelSortOrder.Name = "labelSortOrder";
            this.labelSortOrder.Size = new System.Drawing.Size(820, 22);
            this.labelSortOrder.TabIndex = 21;
            this.labelSortOrder.Text = "排序号";
            // 
            // checkAllowBothOn
            // 
            this.checkAllowBothOn.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkAllowBothOn.Location = new System.Drawing.Point(8, 440);
            this.checkAllowBothOn.Margin = new System.Windows.Forms.Padding(0);
            this.checkAllowBothOn.Name = "checkAllowBothOn";
            this.checkAllowBothOn.Size = new System.Drawing.Size(88, 28);
            this.checkAllowBothOn.TabIndex = 20;
            this.checkAllowBothOn.Text = "允许双ON";
            // 
            // checkAllowBothOff
            // 
            this.checkAllowBothOff.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkAllowBothOff.Location = new System.Drawing.Point(8, 408);
            this.checkAllowBothOff.Margin = new System.Windows.Forms.Padding(0);
            this.checkAllowBothOff.Name = "checkAllowBothOff";
            this.checkAllowBothOff.Size = new System.Drawing.Size(84, 28);
            this.checkAllowBothOff.TabIndex = 19;
            this.checkAllowBothOff.Text = "允许双OFF";
            // 
            // inputRetractTimeoutMs
            // 
            this.inputRetractTimeoutMs.Location = new System.Drawing.Point(8, 372);
            this.inputRetractTimeoutMs.Margin = new System.Windows.Forms.Padding(0);
            this.inputRetractTimeoutMs.Name = "inputRetractTimeoutMs";
            this.inputRetractTimeoutMs.PlaceholderText = "请输入缩回超时";
            this.inputRetractTimeoutMs.Size = new System.Drawing.Size(820, 32);
            this.inputRetractTimeoutMs.TabIndex = 18;
            this.inputRetractTimeoutMs.WaveSize = 0;
            // 
            // labelRetractTimeoutMs
            // 
            this.labelRetractTimeoutMs.Location = new System.Drawing.Point(8, 346);
            this.labelRetractTimeoutMs.Margin = new System.Windows.Forms.Padding(0);
            this.labelRetractTimeoutMs.Name = "labelRetractTimeoutMs";
            this.labelRetractTimeoutMs.Size = new System.Drawing.Size(820, 22);
            this.labelRetractTimeoutMs.TabIndex = 17;
            this.labelRetractTimeoutMs.Text = "缩回超时(ms)";
            // 
            // inputExtendTimeoutMs
            // 
            this.inputExtendTimeoutMs.Location = new System.Drawing.Point(8, 310);
            this.inputExtendTimeoutMs.Margin = new System.Windows.Forms.Padding(0);
            this.inputExtendTimeoutMs.Name = "inputExtendTimeoutMs";
            this.inputExtendTimeoutMs.PlaceholderText = "请输入伸出超时";
            this.inputExtendTimeoutMs.Size = new System.Drawing.Size(820, 32);
            this.inputExtendTimeoutMs.TabIndex = 16;
            this.inputExtendTimeoutMs.WaveSize = 0;
            // 
            // labelExtendTimeoutMs
            // 
            this.labelExtendTimeoutMs.Location = new System.Drawing.Point(8, 284);
            this.labelExtendTimeoutMs.Margin = new System.Windows.Forms.Padding(0);
            this.labelExtendTimeoutMs.Name = "labelExtendTimeoutMs";
            this.labelExtendTimeoutMs.Size = new System.Drawing.Size(820, 22);
            this.labelExtendTimeoutMs.TabIndex = 15;
            this.labelExtendTimeoutMs.Text = "伸出超时(ms)";
            // 
            // checkUseFeedbackCheck
            // 
            this.checkUseFeedbackCheck.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkUseFeedbackCheck.Location = new System.Drawing.Point(8, 252);
            this.checkUseFeedbackCheck.Margin = new System.Windows.Forms.Padding(0);
            this.checkUseFeedbackCheck.Name = "checkUseFeedbackCheck";
            this.checkUseFeedbackCheck.Size = new System.Drawing.Size(98, 28);
            this.checkUseFeedbackCheck.TabIndex = 14;
            this.checkUseFeedbackCheck.Text = "启用反馈校验";
            // 
            // dropdownRetractFeedbackBit
            // 
            this.dropdownRetractFeedbackBit.Location = new System.Drawing.Point(8, 216);
            this.dropdownRetractFeedbackBit.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownRetractFeedbackBit.Name = "dropdownRetractFeedbackBit";
            this.dropdownRetractFeedbackBit.Size = new System.Drawing.Size(820, 32);
            this.dropdownRetractFeedbackBit.TabIndex = 13;
            this.dropdownRetractFeedbackBit.WaveSize = 0;
            // 
            // labelRetractFeedbackBit
            // 
            this.labelRetractFeedbackBit.Location = new System.Drawing.Point(8, 190);
            this.labelRetractFeedbackBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelRetractFeedbackBit.Name = "labelRetractFeedbackBit";
            this.labelRetractFeedbackBit.Size = new System.Drawing.Size(820, 22);
            this.labelRetractFeedbackBit.TabIndex = 12;
            this.labelRetractFeedbackBit.Text = "缩回反馈位";
            // 
            // dropdownExtendFeedbackBit
            // 
            this.dropdownExtendFeedbackBit.Location = new System.Drawing.Point(8, 154);
            this.dropdownExtendFeedbackBit.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownExtendFeedbackBit.Name = "dropdownExtendFeedbackBit";
            this.dropdownExtendFeedbackBit.Size = new System.Drawing.Size(820, 32);
            this.dropdownExtendFeedbackBit.TabIndex = 11;
            this.dropdownExtendFeedbackBit.WaveSize = 0;
            // 
            // labelExtendFeedbackBit
            // 
            this.labelExtendFeedbackBit.Location = new System.Drawing.Point(8, 128);
            this.labelExtendFeedbackBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelExtendFeedbackBit.Name = "labelExtendFeedbackBit";
            this.labelExtendFeedbackBit.Size = new System.Drawing.Size(820, 22);
            this.labelExtendFeedbackBit.TabIndex = 10;
            this.labelExtendFeedbackBit.Text = "伸出反馈位";
            // 
            // dropdownRetractOutputBit
            // 
            this.dropdownRetractOutputBit.Location = new System.Drawing.Point(8, 92);
            this.dropdownRetractOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownRetractOutputBit.Name = "dropdownRetractOutputBit";
            this.dropdownRetractOutputBit.Size = new System.Drawing.Size(820, 32);
            this.dropdownRetractOutputBit.TabIndex = 9;
            this.dropdownRetractOutputBit.WaveSize = 0;
            // 
            // labelRetractOutputBit
            // 
            this.labelRetractOutputBit.Location = new System.Drawing.Point(8, 66);
            this.labelRetractOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelRetractOutputBit.Name = "labelRetractOutputBit";
            this.labelRetractOutputBit.Size = new System.Drawing.Size(820, 22);
            this.labelRetractOutputBit.TabIndex = 8;
            this.labelRetractOutputBit.Text = "缩回输出位";
            // 
            // dropdownExtendOutputBit
            // 
            this.dropdownExtendOutputBit.Location = new System.Drawing.Point(8, 30);
            this.dropdownExtendOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownExtendOutputBit.Name = "dropdownExtendOutputBit";
            this.dropdownExtendOutputBit.Size = new System.Drawing.Size(820, 32);
            this.dropdownExtendOutputBit.TabIndex = 7;
            this.dropdownExtendOutputBit.WaveSize = 0;
            // 
            // labelExtendOutputBit
            // 
            this.labelExtendOutputBit.Location = new System.Drawing.Point(8, 4);
            this.labelExtendOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelExtendOutputBit.Name = "labelExtendOutputBit";
            this.labelExtendOutputBit.Size = new System.Drawing.Size(820, 22);
            this.labelExtendOutputBit.TabIndex = 6;
            this.labelExtendOutputBit.Text = "伸出输出位";
            // 
            // dropdownDriveMode
            // 
            this.dropdownDriveMode.Location = new System.Drawing.Point(8, 216);
            this.dropdownDriveMode.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownDriveMode.Name = "dropdownDriveMode";
            this.dropdownDriveMode.Size = new System.Drawing.Size(820, 32);
            this.dropdownDriveMode.TabIndex = 5;
            this.dropdownDriveMode.WaveSize = 0;
            // 
            // labelDriveMode
            // 
            this.labelDriveMode.Location = new System.Drawing.Point(8, 190);
            this.labelDriveMode.Margin = new System.Windows.Forms.Padding(0);
            this.labelDriveMode.Name = "labelDriveMode";
            this.labelDriveMode.Size = new System.Drawing.Size(820, 22);
            this.labelDriveMode.TabIndex = 4;
            this.labelDriveMode.Text = "驱动模式";
            // 
            // inputDisplayName
            // 
            this.inputDisplayName.Location = new System.Drawing.Point(8, 154);
            this.inputDisplayName.Margin = new System.Windows.Forms.Padding(0);
            this.inputDisplayName.Name = "inputDisplayName";
            this.inputDisplayName.PlaceholderText = "请输入显示名称";
            this.inputDisplayName.Size = new System.Drawing.Size(820, 32);
            this.inputDisplayName.TabIndex = 3;
            this.inputDisplayName.WaveSize = 0;
            // 
            // labelDisplayName
            // 
            this.labelDisplayName.Location = new System.Drawing.Point(8, 128);
            this.labelDisplayName.Margin = new System.Windows.Forms.Padding(0);
            this.labelDisplayName.Name = "labelDisplayName";
            this.labelDisplayName.Size = new System.Drawing.Size(820, 22);
            this.labelDisplayName.TabIndex = 2;
            this.labelDisplayName.Text = "显示名称";
            // 
            // inputName
            // 
            this.inputName.Location = new System.Drawing.Point(8, 92);
            this.inputName.Margin = new System.Windows.Forms.Padding(0);
            this.inputName.Name = "inputName";
            this.inputName.PlaceholderText = "请输入对象名称";
            this.inputName.Size = new System.Drawing.Size(820, 32);
            this.inputName.TabIndex = 1;
            this.inputName.WaveSize = 0;
            // 
            // labelName
            // 
            this.labelName.Location = new System.Drawing.Point(8, 66);
            this.labelName.Margin = new System.Windows.Forms.Padding(0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(820, 22);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "对象名称";
            // 
            // CylinderEditControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.stackRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "CylinderEditControl";
            this.Size = new System.Drawing.Size(836, 452);
            this.stackRoot.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private AntdUI.StackPanel stackRoot;
        private AntdUI.Label labelName;
        private AntdUI.Input inputName;
        private AntdUI.Label labelDisplayName;
        private AntdUI.Input inputDisplayName;
        private AntdUI.Label labelDriveMode;
        private AntdUI.Select dropdownDriveMode;
        private AntdUI.Label labelExtendOutputBit;
        private AntdUI.Select dropdownExtendOutputBit;
        private AntdUI.Label labelRetractOutputBit;
        private AntdUI.Select dropdownRetractOutputBit;
        private AntdUI.Label labelExtendFeedbackBit;
        private AntdUI.Select dropdownExtendFeedbackBit;
        private AntdUI.Label labelRetractFeedbackBit;
        private AntdUI.Select dropdownRetractFeedbackBit;
        private AntdUI.Checkbox checkUseFeedbackCheck;
        private AntdUI.Label labelExtendTimeoutMs;
        private AntdUI.Input inputExtendTimeoutMs;
        private AntdUI.Label labelRetractTimeoutMs;
        private AntdUI.Input inputRetractTimeoutMs;
        private AntdUI.Checkbox checkAllowBothOff;
        private AntdUI.Checkbox checkAllowBothOn;
        private AntdUI.Label labelSortOrder;
        private AntdUI.Input inputSortOrder;
        private AntdUI.Checkbox checkIsEnabled;
        private AntdUI.Label labelDescription;
        private AntdUI.Input inputDescription;
        private AntdUI.Label labelRemark;
        private AntdUI.Input inputRemark;
    }
}