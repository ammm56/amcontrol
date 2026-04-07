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
            this.stackRoot = new AntdUI.StackPanel();
            this.inputRemark = new AntdUI.Input();
            this.labelRemark = new AntdUI.Label();
            this.inputDescription = new AntdUI.Input();
            this.labelDescription = new AntdUI.Label();
            this.checkIsEnabled = new AntdUI.Checkbox();
            this.inputSortOrder = new AntdUI.Input();
            this.labelSortOrder = new AntdUI.Label();
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
            this.dropdownVacuumFeedbackBit = new AntdUI.Select();
            this.labelVacuumFeedbackBit = new AntdUI.Label();
            this.dropdownBlowOffOutputBit = new AntdUI.Select();
            this.labelBlowOffOutputBit = new AntdUI.Label();
            this.dropdownVacuumOnOutputBit = new AntdUI.Select();
            this.labelVacuumOnOutputBit = new AntdUI.Label();
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
            this.stackRoot.Controls.Add(this.inputReleaseTimeoutMs);
            this.stackRoot.Controls.Add(this.labelReleaseTimeoutMs);
            this.stackRoot.Controls.Add(this.inputVacuumBuildTimeoutMs);
            this.stackRoot.Controls.Add(this.labelVacuumBuildTimeoutMs);
            this.stackRoot.Controls.Add(this.checkKeepVacuumOnAfterDetected);
            this.stackRoot.Controls.Add(this.checkUseWorkpieceCheck);
            this.stackRoot.Controls.Add(this.checkUseFeedbackCheck);
            this.stackRoot.Controls.Add(this.dropdownWorkpiecePresentBit);
            this.stackRoot.Controls.Add(this.labelWorkpiecePresentBit);
            this.stackRoot.Controls.Add(this.dropdownReleaseFeedbackBit);
            this.stackRoot.Controls.Add(this.labelReleaseFeedbackBit);
            this.stackRoot.Controls.Add(this.dropdownVacuumFeedbackBit);
            this.stackRoot.Controls.Add(this.labelVacuumFeedbackBit);
            this.stackRoot.Controls.Add(this.dropdownBlowOffOutputBit);
            this.stackRoot.Controls.Add(this.labelBlowOffOutputBit);
            this.stackRoot.Controls.Add(this.dropdownVacuumOnOutputBit);
            this.stackRoot.Controls.Add(this.labelVacuumOnOutputBit);
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
            this.inputRemark.Location = new System.Drawing.Point(8, 638);
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
            this.labelRemark.Location = new System.Drawing.Point(8, 612);
            this.labelRemark.Margin = new System.Windows.Forms.Padding(0);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(820, 22);
            this.labelRemark.TabIndex = 26;
            this.labelRemark.Text = "备注";
            // 
            // inputDescription
            // 
            this.inputDescription.Location = new System.Drawing.Point(8, 530);
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
            this.labelDescription.Location = new System.Drawing.Point(8, 504);
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
            this.checkIsEnabled.Location = new System.Drawing.Point(8, 472);
            this.checkIsEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.checkIsEnabled.Name = "checkIsEnabled";
            this.checkIsEnabled.Size = new System.Drawing.Size(88, 28);
            this.checkIsEnabled.TabIndex = 23;
            this.checkIsEnabled.Text = "启用对象";
            // 
            // inputSortOrder
            // 
            this.inputSortOrder.Location = new System.Drawing.Point(8, 436);
            this.inputSortOrder.Margin = new System.Windows.Forms.Padding(0);
            this.inputSortOrder.Name = "inputSortOrder";
            this.inputSortOrder.PlaceholderText = "请输入排序号";
            this.inputSortOrder.Size = new System.Drawing.Size(820, 32);
            this.inputSortOrder.TabIndex = 22;
            this.inputSortOrder.WaveSize = 0;
            // 
            // labelSortOrder
            // 
            this.labelSortOrder.Location = new System.Drawing.Point(8, 410);
            this.labelSortOrder.Margin = new System.Windows.Forms.Padding(0);
            this.labelSortOrder.Name = "labelSortOrder";
            this.labelSortOrder.Size = new System.Drawing.Size(820, 22);
            this.labelSortOrder.TabIndex = 21;
            this.labelSortOrder.Text = "排序号";
            // 
            // inputReleaseTimeoutMs
            // 
            this.inputReleaseTimeoutMs.Location = new System.Drawing.Point(8, 374);
            this.inputReleaseTimeoutMs.Margin = new System.Windows.Forms.Padding(0);
            this.inputReleaseTimeoutMs.Name = "inputReleaseTimeoutMs";
            this.inputReleaseTimeoutMs.PlaceholderText = "请输入释放超时";
            this.inputReleaseTimeoutMs.Size = new System.Drawing.Size(820, 32);
            this.inputReleaseTimeoutMs.TabIndex = 20;
            this.inputReleaseTimeoutMs.WaveSize = 0;
            // 
            // labelReleaseTimeoutMs
            // 
            this.labelReleaseTimeoutMs.Location = new System.Drawing.Point(8, 348);
            this.labelReleaseTimeoutMs.Margin = new System.Windows.Forms.Padding(0);
            this.labelReleaseTimeoutMs.Name = "labelReleaseTimeoutMs";
            this.labelReleaseTimeoutMs.Size = new System.Drawing.Size(820, 22);
            this.labelReleaseTimeoutMs.TabIndex = 19;
            this.labelReleaseTimeoutMs.Text = "释放超时(ms)";
            // 
            // inputVacuumBuildTimeoutMs
            // 
            this.inputVacuumBuildTimeoutMs.Location = new System.Drawing.Point(8, 312);
            this.inputVacuumBuildTimeoutMs.Margin = new System.Windows.Forms.Padding(0);
            this.inputVacuumBuildTimeoutMs.Name = "inputVacuumBuildTimeoutMs";
            this.inputVacuumBuildTimeoutMs.PlaceholderText = "请输入建压超时";
            this.inputVacuumBuildTimeoutMs.Size = new System.Drawing.Size(820, 32);
            this.inputVacuumBuildTimeoutMs.TabIndex = 18;
            this.inputVacuumBuildTimeoutMs.WaveSize = 0;
            // 
            // labelVacuumBuildTimeoutMs
            // 
            this.labelVacuumBuildTimeoutMs.Location = new System.Drawing.Point(8, 286);
            this.labelVacuumBuildTimeoutMs.Margin = new System.Windows.Forms.Padding(0);
            this.labelVacuumBuildTimeoutMs.Name = "labelVacuumBuildTimeoutMs";
            this.labelVacuumBuildTimeoutMs.Size = new System.Drawing.Size(820, 22);
            this.labelVacuumBuildTimeoutMs.TabIndex = 17;
            this.labelVacuumBuildTimeoutMs.Text = "建压超时(ms)";
            // 
            // checkKeepVacuumOnAfterDetected
            // 
            this.checkKeepVacuumOnAfterDetected.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkKeepVacuumOnAfterDetected.Location = new System.Drawing.Point(8, 254);
            this.checkKeepVacuumOnAfterDetected.Margin = new System.Windows.Forms.Padding(0);
            this.checkKeepVacuumOnAfterDetected.Name = "checkKeepVacuumOnAfterDetected";
            this.checkKeepVacuumOnAfterDetected.Size = new System.Drawing.Size(120, 28);
            this.checkKeepVacuumOnAfterDetected.TabIndex = 16;
            this.checkKeepVacuumOnAfterDetected.Text = "检测到工件后保持真空";
            // 
            // checkUseWorkpieceCheck
            // 
            this.checkUseWorkpieceCheck.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkUseWorkpieceCheck.Location = new System.Drawing.Point(8, 222);
            this.checkUseWorkpieceCheck.Margin = new System.Windows.Forms.Padding(0);
            this.checkUseWorkpieceCheck.Name = "checkUseWorkpieceCheck";
            this.checkUseWorkpieceCheck.Size = new System.Drawing.Size(110, 28);
            this.checkUseWorkpieceCheck.TabIndex = 15;
            this.checkUseWorkpieceCheck.Text = "启用工件检测校验";
            // 
            // checkUseFeedbackCheck
            // 
            this.checkUseFeedbackCheck.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkUseFeedbackCheck.Location = new System.Drawing.Point(8, 190);
            this.checkUseFeedbackCheck.Margin = new System.Windows.Forms.Padding(0);
            this.checkUseFeedbackCheck.Name = "checkUseFeedbackCheck";
            this.checkUseFeedbackCheck.Size = new System.Drawing.Size(98, 28);
            this.checkUseFeedbackCheck.TabIndex = 14;
            this.checkUseFeedbackCheck.Text = "启用反馈校验";
            // 
            // dropdownWorkpiecePresentBit
            // 
            this.dropdownWorkpiecePresentBit.Location = new System.Drawing.Point(8, 154);
            this.dropdownWorkpiecePresentBit.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownWorkpiecePresentBit.Name = "dropdownWorkpiecePresentBit";
            this.dropdownWorkpiecePresentBit.Size = new System.Drawing.Size(820, 32);
            this.dropdownWorkpiecePresentBit.TabIndex = 13;
            this.dropdownWorkpiecePresentBit.WaveSize = 0;
            // 
            // labelWorkpiecePresentBit
            // 
            this.labelWorkpiecePresentBit.Location = new System.Drawing.Point(8, 128);
            this.labelWorkpiecePresentBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelWorkpiecePresentBit.Name = "labelWorkpiecePresentBit";
            this.labelWorkpiecePresentBit.Size = new System.Drawing.Size(820, 22);
            this.labelWorkpiecePresentBit.TabIndex = 12;
            this.labelWorkpiecePresentBit.Text = "工件检测位";
            // 
            // dropdownReleaseFeedbackBit
            // 
            this.dropdownReleaseFeedbackBit.Location = new System.Drawing.Point(8, 92);
            this.dropdownReleaseFeedbackBit.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownReleaseFeedbackBit.Name = "dropdownReleaseFeedbackBit";
            this.dropdownReleaseFeedbackBit.Size = new System.Drawing.Size(820, 32);
            this.dropdownReleaseFeedbackBit.TabIndex = 11;
            this.dropdownReleaseFeedbackBit.WaveSize = 0;
            // 
            // labelReleaseFeedbackBit
            // 
            this.labelReleaseFeedbackBit.Location = new System.Drawing.Point(8, 66);
            this.labelReleaseFeedbackBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelReleaseFeedbackBit.Name = "labelReleaseFeedbackBit";
            this.labelReleaseFeedbackBit.Size = new System.Drawing.Size(820, 22);
            this.labelReleaseFeedbackBit.TabIndex = 10;
            this.labelReleaseFeedbackBit.Text = "释放反馈位";
            // 
            // dropdownVacuumFeedbackBit
            // 
            this.dropdownVacuumFeedbackBit.Location = new System.Drawing.Point(8, 30);
            this.dropdownVacuumFeedbackBit.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownVacuumFeedbackBit.Name = "dropdownVacuumFeedbackBit";
            this.dropdownVacuumFeedbackBit.Size = new System.Drawing.Size(820, 32);
            this.dropdownVacuumFeedbackBit.TabIndex = 9;
            this.dropdownVacuumFeedbackBit.WaveSize = 0;
            // 
            // labelVacuumFeedbackBit
            // 
            this.labelVacuumFeedbackBit.Location = new System.Drawing.Point(8, 4);
            this.labelVacuumFeedbackBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelVacuumFeedbackBit.Name = "labelVacuumFeedbackBit";
            this.labelVacuumFeedbackBit.Size = new System.Drawing.Size(820, 22);
            this.labelVacuumFeedbackBit.TabIndex = 8;
            this.labelVacuumFeedbackBit.Text = "真空反馈位";
            // 
            // dropdownBlowOffOutputBit
            // 
            this.dropdownBlowOffOutputBit.Location = new System.Drawing.Point(8, 216);
            this.dropdownBlowOffOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownBlowOffOutputBit.Name = "dropdownBlowOffOutputBit";
            this.dropdownBlowOffOutputBit.Size = new System.Drawing.Size(820, 32);
            this.dropdownBlowOffOutputBit.TabIndex = 7;
            this.dropdownBlowOffOutputBit.WaveSize = 0;
            // 
            // labelBlowOffOutputBit
            // 
            this.labelBlowOffOutputBit.Location = new System.Drawing.Point(8, 190);
            this.labelBlowOffOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelBlowOffOutputBit.Name = "labelBlowOffOutputBit";
            this.labelBlowOffOutputBit.Size = new System.Drawing.Size(820, 22);
            this.labelBlowOffOutputBit.TabIndex = 6;
            this.labelBlowOffOutputBit.Text = "破真空输出位";
            // 
            // dropdownVacuumOnOutputBit
            // 
            this.dropdownVacuumOnOutputBit.Location = new System.Drawing.Point(8, 154);
            this.dropdownVacuumOnOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownVacuumOnOutputBit.Name = "dropdownVacuumOnOutputBit";
            this.dropdownVacuumOnOutputBit.Size = new System.Drawing.Size(820, 32);
            this.dropdownVacuumOnOutputBit.TabIndex = 5;
            this.dropdownVacuumOnOutputBit.WaveSize = 0;
            // 
            // labelVacuumOnOutputBit
            // 
            this.labelVacuumOnOutputBit.Location = new System.Drawing.Point(8, 128);
            this.labelVacuumOnOutputBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelVacuumOnOutputBit.Name = "labelVacuumOnOutputBit";
            this.labelVacuumOnOutputBit.Size = new System.Drawing.Size(820, 22);
            this.labelVacuumOnOutputBit.TabIndex = 4;
            this.labelVacuumOnOutputBit.Text = "吸真空输出位";
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
            // VacuumEditControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.stackRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "VacuumEditControl";
            this.Size = new System.Drawing.Size(836, 452);
            this.stackRoot.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private AntdUI.StackPanel stackRoot;
        private AntdUI.Label labelName;
        private AntdUI.Input inputName;
        private AntdUI.Label labelDisplayName;
        private AntdUI.Input inputDisplayName;
        private AntdUI.Label labelVacuumOnOutputBit;
        private AntdUI.Select dropdownVacuumOnOutputBit;
        private AntdUI.Label labelBlowOffOutputBit;
        private AntdUI.Select dropdownBlowOffOutputBit;
        private AntdUI.Label labelVacuumFeedbackBit;
        private AntdUI.Select dropdownVacuumFeedbackBit;
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
        private AntdUI.Label labelSortOrder;
        private AntdUI.Input inputSortOrder;
        private AntdUI.Checkbox checkIsEnabled;
        private AntdUI.Label labelDescription;
        private AntdUI.Input inputDescription;
        private AntdUI.Label labelRemark;
        private AntdUI.Input inputRemark;
    }
}