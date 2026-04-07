namespace AMControlWinF.Views.MotionConfig
{
    partial class GripperEditControl
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
            this.inputOpenTimeoutMs = new AntdUI.Input();
            this.labelOpenTimeoutMs = new AntdUI.Label();
            this.inputCloseTimeoutMs = new AntdUI.Input();
            this.labelCloseTimeoutMs = new AntdUI.Label();
            this.checkUseWorkpieceCheck = new AntdUI.Checkbox();
            this.checkUseFeedbackCheck = new AntdUI.Checkbox();
            this.dropdownWorkpiecePresentBit = new AntdUI.Select();
            this.labelWorkpiecePresentBit = new AntdUI.Label();
            this.dropdownOpenFeedbackBit = new AntdUI.Select();
            this.labelOpenFeedbackBit = new AntdUI.Label();
            this.dropdownCloseFeedbackBit = new AntdUI.Select();
            this.labelCloseFeedbackBit = new AntdUI.Label();
            this.dropdownOpenOutputBit = new AntdUI.Select();
            this.labelOpenOutputBit = new AntdUI.Label();
            this.dropdownCloseOutputBit = new AntdUI.Select();
            this.labelCloseOutputBit = new AntdUI.Label();
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
            this.stackRoot.Controls.Add(this.inputOpenTimeoutMs);
            this.stackRoot.Controls.Add(this.labelOpenTimeoutMs);
            this.stackRoot.Controls.Add(this.inputCloseTimeoutMs);
            this.stackRoot.Controls.Add(this.labelCloseTimeoutMs);
            this.stackRoot.Controls.Add(this.checkUseWorkpieceCheck);
            this.stackRoot.Controls.Add(this.checkUseFeedbackCheck);
            this.stackRoot.Controls.Add(this.dropdownWorkpiecePresentBit);
            this.stackRoot.Controls.Add(this.labelWorkpiecePresentBit);
            this.stackRoot.Controls.Add(this.dropdownOpenFeedbackBit);
            this.stackRoot.Controls.Add(this.labelOpenFeedbackBit);
            this.stackRoot.Controls.Add(this.dropdownCloseFeedbackBit);
            this.stackRoot.Controls.Add(this.labelCloseFeedbackBit);
            this.stackRoot.Controls.Add(this.dropdownOpenOutputBit);
            this.stackRoot.Controls.Add(this.labelOpenOutputBit);
            this.stackRoot.Controls.Add(this.dropdownCloseOutputBit);
            this.stackRoot.Controls.Add(this.labelCloseOutputBit);
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
            this.inputRemark.TabIndex = 30;
            this.inputRemark.WaveSize = 0;
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(8, 674);
            this.labelRemark.Margin = new System.Windows.Forms.Padding(0);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(820, 22);
            this.labelRemark.TabIndex = 29;
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
            this.inputDescription.TabIndex = 28;
            this.inputDescription.WaveSize = 0;
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(8, 566);
            this.labelDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(820, 22);
            this.labelDescription.TabIndex = 27;
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
            this.checkIsEnabled.TabIndex = 26;
            this.checkIsEnabled.Text = "启用对象";
            // 
            // inputSortOrder
            // 
            this.inputSortOrder.Location = new System.Drawing.Point(8, 498);
            this.inputSortOrder.Margin = new System.Windows.Forms.Padding(0);
            this.inputSortOrder.Name = "inputSortOrder";
            this.inputSortOrder.PlaceholderText = "请输入排序号";
            this.inputSortOrder.Size = new System.Drawing.Size(820, 32);
            this.inputSortOrder.TabIndex = 25;
            this.inputSortOrder.WaveSize = 0;
            // 
            // labelSortOrder
            // 
            this.labelSortOrder.Location = new System.Drawing.Point(8, 472);
            this.labelSortOrder.Margin = new System.Windows.Forms.Padding(0);
            this.labelSortOrder.Name = "labelSortOrder";
            this.labelSortOrder.Size = new System.Drawing.Size(820, 22);
            this.labelSortOrder.TabIndex = 24;
            this.labelSortOrder.Text = "排序号";
            // 
            // checkAllowBothOn
            // 
            this.checkAllowBothOn.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkAllowBothOn.Location = new System.Drawing.Point(8, 440);
            this.checkAllowBothOn.Margin = new System.Windows.Forms.Padding(0);
            this.checkAllowBothOn.Name = "checkAllowBothOn";
            this.checkAllowBothOn.Size = new System.Drawing.Size(88, 28);
            this.checkAllowBothOn.TabIndex = 23;
            this.checkAllowBothOn.Text = "允许双ON";
            // 
            // checkAllowBothOff
            // 
            this.checkAllowBothOff.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkAllowBothOff.Location = new System.Drawing.Point(8, 408);
            this.checkAllowBothOff.Margin = new System.Windows.Forms.Padding(0);
            this.checkAllowBothOff.Name = "checkAllowBothOff";
            this.checkAllowBothOff.Size = new System.Drawing.Size(84, 28);
            this.checkAllowBothOff.TabIndex = 22;
            this.checkAllowBothOff.Text = "允许双OFF";
            // 
            // inputOpenTimeoutMs
            // 
            this.inputOpenTimeoutMs.Location = new System.Drawing.Point(8, 372);
            this.inputOpenTimeoutMs.Margin = new System.Windows.Forms.Padding(0);
            this.inputOpenTimeoutMs.Name = "inputOpenTimeoutMs";
            this.inputOpenTimeoutMs.PlaceholderText = "请输入打开超时";
            this.inputOpenTimeoutMs.Size = new System.Drawing.Size(820, 32);
            this.inputOpenTimeoutMs.TabIndex = 21;
            this.inputOpenTimeoutMs.WaveSize = 0;
            // 
            // labelOpenTimeoutMs
            // 
            this.labelOpenTimeoutMs.Location = new System.Drawing.Point(8, 346);
            this.labelOpenTimeoutMs.Margin = new System.Windows.Forms.Padding(0);
            this.labelOpenTimeoutMs.Name = "labelOpenTimeoutMs";
            this.labelOpenTimeoutMs.Size = new System.Drawing.Size(820, 22);
            this.labelOpenTimeoutMs.TabIndex = 20;
            this.labelOpenTimeoutMs.Text = "打开超时(ms)";
            // 
            // inputCloseTimeoutMs
            // 
            this.inputCloseTimeoutMs.Location = new System.Drawing.Point(8, 310);
            this.inputCloseTimeoutMs.Margin = new System.Windows.Forms.Padding(0);
            this.inputCloseTimeoutMs.Name = "inputCloseTimeoutMs";
            this.inputCloseTimeoutMs.PlaceholderText = "请输入夹紧超时";
            this.inputCloseTimeoutMs.Size = new System.Drawing.Size(820, 32);
            this.inputCloseTimeoutMs.TabIndex = 19;
            this.inputCloseTimeoutMs.WaveSize = 0;
            // 
            // labelCloseTimeoutMs
            // 
            this.labelCloseTimeoutMs.Location = new System.Drawing.Point(8, 284);
            this.labelCloseTimeoutMs.Margin = new System.Windows.Forms.Padding(0);
            this.labelCloseTimeoutMs.Name = "labelCloseTimeoutMs";
            this.labelCloseTimeoutMs.Size = new System.Drawing.Size(820, 22);
            this.labelCloseTimeoutMs.TabIndex = 18;
            this.labelCloseTimeoutMs.Text = "夹紧超时(ms)";
            // 
            // checkUseWorkpieceCheck
            // 
            this.checkUseWorkpieceCheck.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkUseWorkpieceCheck.Location = new System.Drawing.Point(8, 252);
            this.checkUseWorkpieceCheck.Margin = new System.Windows.Forms.Padding(0);
            this.checkUseWorkpieceCheck.Name = "checkUseWorkpieceCheck";
            this.checkUseWorkpieceCheck.Size = new System.Drawing.Size(110, 28);
            this.checkUseWorkpieceCheck.TabIndex = 17;
            this.checkUseWorkpieceCheck.Text = "启用工件检测校验";
            // 
            // checkUseFeedbackCheck
            // 
            this.checkUseFeedbackCheck.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkUseFeedbackCheck.Location = new System.Drawing.Point(8, 220);
            this.checkUseFeedbackCheck.Margin = new System.Windows.Forms.Padding(0);
            this.checkUseFeedbackCheck.Name = "checkUseFeedbackCheck";
            this.checkUseFeedbackCheck.Size = new System.Drawing.Size(98, 28);
            this.checkUseFeedbackCheck.TabIndex = 16;
            this.checkUseFeedbackCheck.Text = "启用反馈校验";
            // 
            // dropdownWorkpiecePresentBit
            // 
            this.dropdownWorkpiecePresentBit.Location = new System.Drawing.Point(8, 184);
            this.dropdownWorkpiecePresentBit.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownWorkpiecePresentBit.Name = "dropdownWorkpiecePresentBit";
            this.dropdownWorkpiecePresentBit.Size = new System.Drawing.Size(820, 32);
            this.dropdownWorkpiecePresentBit.TabIndex = 15;
            this.dropdownWorkpiecePresentBit.WaveSize = 0;
            // 
            // labelWorkpiecePresentBit
            // 
            this.labelWorkpiecePresentBit.Location = new System.Drawing.Point(8, 158);
            this.labelWorkpiecePresentBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelWorkpiecePresentBit.Name = "labelWorkpiecePresentBit";
            this.labelWorkpiecePresentBit.Size = new System.Drawing.Size(820, 22);
            this.labelWorkpiecePresentBit.TabIndex = 14;
            this.labelWorkpiecePresentBit.Text = "工件检测位";
            // 
            // dropdownOpenFeedbackBit
            // 
            this.dropdownOpenFeedbackBit.Location = new System.Drawing.Point(8, 122);
            this.dropdownOpenFeedbackBit.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownOpenFeedbackBit.Name = "dropdownOpenFeedbackBit";
            this.dropdownOpenFeedbackBit.Size = new System.Drawing.Size(820, 32);
            this.dropdownOpenFeedbackBit.TabIndex = 13;
            this.dropdownOpenFeedbackBit.WaveSize = 0;
            // 
            // labelOpenFeedbackBit
            // 
            this.labelOpenFeedbackBit.Location = new System.Drawing.Point(8, 96);
            this.labelOpenFeedbackBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelOpenFeedbackBit.Name = "labelOpenFeedbackBit";
            this.labelOpenFeedbackBit.Size = new System.Drawing.Size(820, 22);
            this.labelOpenFeedbackBit.TabIndex = 12;
            this.labelOpenFeedbackBit.Text = "打开反馈位";
            // 
            // dropdownCloseFeedbackBit
            // 
            this.dropdownCloseFeedbackBit.Location = new System.Drawing.Point(8, 60);
            this.dropdownCloseFeedbackBit.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownCloseFeedbackBit.Name = "dropdownCloseFeedbackBit";
            this.dropdownCloseFeedbackBit.Size = new System.Drawing.Size(820, 32);
            this.dropdownCloseFeedbackBit.TabIndex = 11;
            this.dropdownCloseFeedbackBit.WaveSize = 0;
            // 
            // labelCloseFeedbackBit
            // 
            this.labelCloseFeedbackBit.Location = new System.Drawing.Point(8, 34);
            this.labelCloseFeedbackBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelCloseFeedbackBit.Name = "labelCloseFeedbackBit";
            this.labelCloseFeedbackBit.Size = new System.Drawing.Size(820, 22);
            this.labelCloseFeedbackBit.TabIndex = 10;
            this.labelCloseFeedbackBit.Text = "夹紧反馈位";
            // 
            // dropdownOpenOutputBit
            // 
            this.dropdownOpenOutputBit.Location = new System.Drawing.Point(8, 246);
            this.dropdownOpenOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownOpenOutputBit.Name = "dropdownOpenOutputBit";
            this.dropdownOpenOutputBit.Size = new System.Drawing.Size(820, 32);
            this.dropdownOpenOutputBit.TabIndex = 9;
            this.dropdownOpenOutputBit.WaveSize = 0;
            // 
            // labelOpenOutputBit
            // 
            this.labelOpenOutputBit.Location = new System.Drawing.Point(8, 220);
            this.labelOpenOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelOpenOutputBit.Name = "labelOpenOutputBit";
            this.labelOpenOutputBit.Size = new System.Drawing.Size(820, 22);
            this.labelOpenOutputBit.TabIndex = 8;
            this.labelOpenOutputBit.Text = "打开输出位";
            // 
            // dropdownCloseOutputBit
            // 
            this.dropdownCloseOutputBit.Location = new System.Drawing.Point(8, 184);
            this.dropdownCloseOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownCloseOutputBit.Name = "dropdownCloseOutputBit";
            this.dropdownCloseOutputBit.Size = new System.Drawing.Size(820, 32);
            this.dropdownCloseOutputBit.TabIndex = 7;
            this.dropdownCloseOutputBit.WaveSize = 0;
            // 
            // labelCloseOutputBit
            // 
            this.labelCloseOutputBit.Location = new System.Drawing.Point(8, 158);
            this.labelCloseOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelCloseOutputBit.Name = "labelCloseOutputBit";
            this.labelCloseOutputBit.Size = new System.Drawing.Size(820, 22);
            this.labelCloseOutputBit.TabIndex = 6;
            this.labelCloseOutputBit.Text = "夹紧输出位";
            // 
            // dropdownDriveMode
            // 
            this.dropdownDriveMode.Location = new System.Drawing.Point(8, 122);
            this.dropdownDriveMode.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownDriveMode.Name = "dropdownDriveMode";
            this.dropdownDriveMode.Size = new System.Drawing.Size(820, 32);
            this.dropdownDriveMode.TabIndex = 5;
            this.dropdownDriveMode.WaveSize = 0;
            // 
            // labelDriveMode
            // 
            this.labelDriveMode.Location = new System.Drawing.Point(8, 96);
            this.labelDriveMode.Margin = new System.Windows.Forms.Padding(0);
            this.labelDriveMode.Name = "labelDriveMode";
            this.labelDriveMode.Size = new System.Drawing.Size(820, 22);
            this.labelDriveMode.TabIndex = 4;
            this.labelDriveMode.Text = "驱动模式";
            // 
            // inputDisplayName
            // 
            this.inputDisplayName.Location = new System.Drawing.Point(8, 60);
            this.inputDisplayName.Margin = new System.Windows.Forms.Padding(0);
            this.inputDisplayName.Name = "inputDisplayName";
            this.inputDisplayName.PlaceholderText = "请输入显示名称";
            this.inputDisplayName.Size = new System.Drawing.Size(820, 32);
            this.inputDisplayName.TabIndex = 3;
            this.inputDisplayName.WaveSize = 0;
            // 
            // labelDisplayName
            // 
            this.labelDisplayName.Location = new System.Drawing.Point(8, 34);
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
            // GripperEditControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.stackRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "GripperEditControl";
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
        private AntdUI.Label labelCloseOutputBit;
        private AntdUI.Select dropdownCloseOutputBit;
        private AntdUI.Label labelOpenOutputBit;
        private AntdUI.Select dropdownOpenOutputBit;
        private AntdUI.Label labelCloseFeedbackBit;
        private AntdUI.Select dropdownCloseFeedbackBit;
        private AntdUI.Label labelOpenFeedbackBit;
        private AntdUI.Select dropdownOpenFeedbackBit;
        private AntdUI.Label labelWorkpiecePresentBit;
        private AntdUI.Select dropdownWorkpiecePresentBit;
        private AntdUI.Checkbox checkUseFeedbackCheck;
        private AntdUI.Checkbox checkUseWorkpieceCheck;
        private AntdUI.Label labelCloseTimeoutMs;
        private AntdUI.Input inputCloseTimeoutMs;
        private AntdUI.Label labelOpenTimeoutMs;
        private AntdUI.Input inputOpenTimeoutMs;
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