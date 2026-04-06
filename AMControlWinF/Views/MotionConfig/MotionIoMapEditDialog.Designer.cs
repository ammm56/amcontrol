namespace AMControlWinF.Views.MotionConfig
{
    partial class MotionIoMapEditDialog
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
            this.textureBackgroundDialog = new AMControlWinF.Views.Main.TextureBackgroundControl();
            this.panelShell = new AntdUI.Panel();
            this.panelContent = new AntdUI.Panel();
            this.gridMainSections = new AntdUI.GridPanel();
            this.stackSectionRemark = new AntdUI.StackPanel();
            this.inputRemark = new AntdUI.Input();
            this.labelRemark = new AntdUI.Label();
            this.labelSectionRemark = new AntdUI.Label();
            this.stackSectionRuntime = new AntdUI.StackPanel();
            this.inputSortOrder = new AntdUI.Input();
            this.labelSortOrder = new AntdUI.Label();
            this.checkIsEnabled = new AntdUI.Checkbox();
            this.labelEnabled = new AntdUI.Label();
            this.checkIsExtModule = new AntdUI.Checkbox();
            this.labelIsExtModule = new AntdUI.Label();
            this.labelSectionRuntime = new AntdUI.Label();
            this.stackSectionMapping = new AntdUI.StackPanel();
            this.inputHardwareBit = new AntdUI.Input();
            this.labelHardwareBit = new AntdUI.Label();
            this.dropdownCore = new AntdUI.Select();
            this.labelCore = new AntdUI.Label();
            this.dropdownCardId = new AntdUI.Select();
            this.labelCardId = new AntdUI.Label();
            this.labelSectionMapping = new AntdUI.Label();
            this.stackSectionBasic = new AntdUI.StackPanel();
            this.inputName = new AntdUI.Input();
            this.labelName = new AntdUI.Label();
            this.inputLogicalBit = new AntdUI.Input();
            this.labelLogicalBit = new AntdUI.Label();
            this.dropdownIoType = new AntdUI.Select();
            this.labelIoType = new AntdUI.Label();
            this.labelSectionBasic = new AntdUI.Label();
            this.panelFooter = new AntdUI.Panel();
            this.flowFooterButtons = new AntdUI.FlowPanel();
            this.buttonOk = new AntdUI.Button();
            this.buttonCancel = new AntdUI.Button();
            this.panelHeader = new AntdUI.Panel();
            this.labelDialogDescription = new AntdUI.Label();
            this.labelDialogTitle = new AntdUI.Label();
            this.textureBackgroundDialog.SuspendLayout();
            this.panelShell.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.gridMainSections.SuspendLayout();
            this.stackSectionRemark.SuspendLayout();
            this.stackSectionRuntime.SuspendLayout();
            this.stackSectionMapping.SuspendLayout();
            this.stackSectionBasic.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.flowFooterButtons.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // textureBackgroundDialog
            // 
            this.textureBackgroundDialog.Controls.Add(this.panelShell);
            this.textureBackgroundDialog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textureBackgroundDialog.Location = new System.Drawing.Point(0, 0);
            this.textureBackgroundDialog.Margin = new System.Windows.Forms.Padding(0);
            this.textureBackgroundDialog.Name = "textureBackgroundDialog";
            this.textureBackgroundDialog.Size = new System.Drawing.Size(860, 520);
            this.textureBackgroundDialog.TabIndex = 0;
            // 
            // panelShell
            // 
            this.panelShell.BackColor = System.Drawing.Color.Transparent;
            this.panelShell.Controls.Add(this.panelContent);
            this.panelShell.Controls.Add(this.panelFooter);
            this.panelShell.Controls.Add(this.panelHeader);
            this.panelShell.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelShell.Location = new System.Drawing.Point(0, 0);
            this.panelShell.Margin = new System.Windows.Forms.Padding(0);
            this.panelShell.Name = "panelShell";
            this.panelShell.Padding = new System.Windows.Forms.Padding(12);
            this.panelShell.Radius = 16;
            this.panelShell.Shadow = 16;
            this.panelShell.ShadowOpacity = 0.2F;
            this.panelShell.Size = new System.Drawing.Size(860, 520);
            this.panelShell.TabIndex = 0;
            // 
            // panelContent
            // 
            this.panelContent.Controls.Add(this.gridMainSections);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(28, 84);
            this.panelContent.Margin = new System.Windows.Forms.Padding(0);
            this.panelContent.Name = "panelContent";
            this.panelContent.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.panelContent.Radius = 0;
            this.panelContent.Size = new System.Drawing.Size(804, 351);
            this.panelContent.TabIndex = 1;
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
            this.gridMainSections.Size = new System.Drawing.Size(796, 351);
            this.gridMainSections.Span = "28% 24% 20% 28%";
            this.gridMainSections.TabIndex = 0;
            this.gridMainSections.Text = "gridMainSections";
            // 
            // stackSectionRemark
            // 
            this.stackSectionRemark.AutoScroll = true;
            this.stackSectionRemark.Controls.Add(this.inputRemark);
            this.stackSectionRemark.Controls.Add(this.labelRemark);
            this.stackSectionRemark.Controls.Add(this.labelSectionRemark);
            this.stackSectionRemark.Gap = 4;
            this.stackSectionRemark.Location = new System.Drawing.Point(573, 0);
            this.stackSectionRemark.Margin = new System.Windows.Forms.Padding(0);
            this.stackSectionRemark.Name = "stackSectionRemark";
            this.stackSectionRemark.Padding = new System.Windows.Forms.Padding(4);
            this.stackSectionRemark.Size = new System.Drawing.Size(223, 351);
            this.stackSectionRemark.TabIndex = 3;
            this.stackSectionRemark.Text = "stackSectionRemark";
            this.stackSectionRemark.Vertical = true;
            // 
            // inputRemark
            // 
            this.inputRemark.Location = new System.Drawing.Point(4, 60);
            this.inputRemark.Margin = new System.Windows.Forms.Padding(0);
            this.inputRemark.Multiline = true;
            this.inputRemark.Name = "inputRemark";
            this.inputRemark.PlaceholderText = "请输入备注";
            this.inputRemark.Size = new System.Drawing.Size(215, 80);
            this.inputRemark.TabIndex = 2;
            this.inputRemark.WaveSize = 0;
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(4, 34);
            this.labelRemark.Margin = new System.Windows.Forms.Padding(0);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(215, 22);
            this.labelRemark.TabIndex = 1;
            this.labelRemark.Text = "备注";
            // 
            // labelSectionRemark
            // 
            this.labelSectionRemark.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionRemark.Location = new System.Drawing.Point(4, 4);
            this.labelSectionRemark.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionRemark.Name = "labelSectionRemark";
            this.labelSectionRemark.Size = new System.Drawing.Size(215, 26);
            this.labelSectionRemark.TabIndex = 0;
            this.labelSectionRemark.Text = "备注信息";
            // 
            // stackSectionRuntime
            // 
            this.stackSectionRuntime.AutoScroll = true;
            this.stackSectionRuntime.Controls.Add(this.inputSortOrder);
            this.stackSectionRuntime.Controls.Add(this.labelSortOrder);
            this.stackSectionRuntime.Controls.Add(this.checkIsEnabled);
            this.stackSectionRuntime.Controls.Add(this.labelEnabled);
            this.stackSectionRuntime.Controls.Add(this.checkIsExtModule);
            this.stackSectionRuntime.Controls.Add(this.labelIsExtModule);
            this.stackSectionRuntime.Controls.Add(this.labelSectionRuntime);
            this.stackSectionRuntime.Gap = 4;
            this.stackSectionRuntime.Location = new System.Drawing.Point(414, 0);
            this.stackSectionRuntime.Margin = new System.Windows.Forms.Padding(0);
            this.stackSectionRuntime.Name = "stackSectionRuntime";
            this.stackSectionRuntime.Padding = new System.Windows.Forms.Padding(4);
            this.stackSectionRuntime.Size = new System.Drawing.Size(159, 351);
            this.stackSectionRuntime.TabIndex = 2;
            this.stackSectionRuntime.Text = "stackSectionRuntime";
            this.stackSectionRuntime.Vertical = true;
            // 
            // inputSortOrder
            // 
            this.inputSortOrder.Location = new System.Drawing.Point(4, 176);
            this.inputSortOrder.Margin = new System.Windows.Forms.Padding(0);
            this.inputSortOrder.Name = "inputSortOrder";
            this.inputSortOrder.PlaceholderText = "请输入排序号";
            this.inputSortOrder.Size = new System.Drawing.Size(151, 32);
            this.inputSortOrder.TabIndex = 6;
            this.inputSortOrder.WaveSize = 0;
            // 
            // labelSortOrder
            // 
            this.labelSortOrder.Location = new System.Drawing.Point(4, 150);
            this.labelSortOrder.Margin = new System.Windows.Forms.Padding(0);
            this.labelSortOrder.Name = "labelSortOrder";
            this.labelSortOrder.Size = new System.Drawing.Size(151, 22);
            this.labelSortOrder.TabIndex = 5;
            this.labelSortOrder.Text = "排序号";
            // 
            // checkIsEnabled
            // 
            this.checkIsEnabled.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkIsEnabled.Checked = true;
            this.checkIsEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkIsEnabled.Location = new System.Drawing.Point(4, 118);
            this.checkIsEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.checkIsEnabled.Name = "checkIsEnabled";
            this.checkIsEnabled.Size = new System.Drawing.Size(88, 28);
            this.checkIsEnabled.TabIndex = 4;
            this.checkIsEnabled.Text = "启用此点位";
            // 
            // labelEnabled
            // 
            this.labelEnabled.Location = new System.Drawing.Point(4, 92);
            this.labelEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.labelEnabled.Name = "labelEnabled";
            this.labelEnabled.Size = new System.Drawing.Size(151, 22);
            this.labelEnabled.TabIndex = 3;
            this.labelEnabled.Text = "启用状态";
            // 
            // checkIsExtModule
            // 
            this.checkIsExtModule.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkIsExtModule.Location = new System.Drawing.Point(4, 60);
            this.checkIsExtModule.Margin = new System.Windows.Forms.Padding(0);
            this.checkIsExtModule.Name = "checkIsExtModule";
            this.checkIsExtModule.Size = new System.Drawing.Size(94, 28);
            this.checkIsExtModule.TabIndex = 2;
            this.checkIsExtModule.Text = "扩展模块 IO";
            // 
            // labelIsExtModule
            // 
            this.labelIsExtModule.Location = new System.Drawing.Point(4, 34);
            this.labelIsExtModule.Margin = new System.Windows.Forms.Padding(0);
            this.labelIsExtModule.Name = "labelIsExtModule";
            this.labelIsExtModule.Size = new System.Drawing.Size(151, 22);
            this.labelIsExtModule.TabIndex = 1;
            this.labelIsExtModule.Text = "板载 / 扩展";
            // 
            // labelSectionRuntime
            // 
            this.labelSectionRuntime.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionRuntime.Location = new System.Drawing.Point(4, 4);
            this.labelSectionRuntime.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionRuntime.Name = "labelSectionRuntime";
            this.labelSectionRuntime.Size = new System.Drawing.Size(151, 26);
            this.labelSectionRuntime.TabIndex = 0;
            this.labelSectionRuntime.Text = "状态与属性";
            // 
            // stackSectionMapping
            // 
            this.stackSectionMapping.AutoScroll = true;
            this.stackSectionMapping.Controls.Add(this.inputHardwareBit);
            this.stackSectionMapping.Controls.Add(this.labelHardwareBit);
            this.stackSectionMapping.Controls.Add(this.dropdownCore);
            this.stackSectionMapping.Controls.Add(this.labelCore);
            this.stackSectionMapping.Controls.Add(this.dropdownCardId);
            this.stackSectionMapping.Controls.Add(this.labelCardId);
            this.stackSectionMapping.Controls.Add(this.labelSectionMapping);
            this.stackSectionMapping.Gap = 4;
            this.stackSectionMapping.Location = new System.Drawing.Point(223, 0);
            this.stackSectionMapping.Margin = new System.Windows.Forms.Padding(0);
            this.stackSectionMapping.Name = "stackSectionMapping";
            this.stackSectionMapping.Padding = new System.Windows.Forms.Padding(4);
            this.stackSectionMapping.Size = new System.Drawing.Size(191, 351);
            this.stackSectionMapping.TabIndex = 1;
            this.stackSectionMapping.Text = "stackSectionMapping";
            this.stackSectionMapping.Vertical = true;
            // 
            // inputHardwareBit
            // 
            this.inputHardwareBit.Location = new System.Drawing.Point(4, 184);
            this.inputHardwareBit.Margin = new System.Windows.Forms.Padding(0);
            this.inputHardwareBit.Name = "inputHardwareBit";
            this.inputHardwareBit.PlaceholderText = "请输入硬件位号";
            this.inputHardwareBit.Size = new System.Drawing.Size(183, 32);
            this.inputHardwareBit.TabIndex = 6;
            this.inputHardwareBit.WaveSize = 0;
            // 
            // labelHardwareBit
            // 
            this.labelHardwareBit.Location = new System.Drawing.Point(4, 158);
            this.labelHardwareBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelHardwareBit.Name = "labelHardwareBit";
            this.labelHardwareBit.Size = new System.Drawing.Size(183, 22);
            this.labelHardwareBit.TabIndex = 5;
            this.labelHardwareBit.Text = "硬件位号";
            // 
            // dropdownCore
            // 
            this.dropdownCore.Location = new System.Drawing.Point(4, 122);
            this.dropdownCore.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownCore.Name = "dropdownCore";
            this.dropdownCore.Size = new System.Drawing.Size(183, 32);
            this.dropdownCore.TabIndex = 4;
            this.dropdownCore.WaveSize = 0;
            // 
            // labelCore
            // 
            this.labelCore.Location = new System.Drawing.Point(4, 96);
            this.labelCore.Margin = new System.Windows.Forms.Padding(0);
            this.labelCore.Name = "labelCore";
            this.labelCore.Size = new System.Drawing.Size(183, 22);
            this.labelCore.TabIndex = 3;
            this.labelCore.Text = "所属内核";
            // 
            // dropdownCardId
            // 
            this.dropdownCardId.Location = new System.Drawing.Point(4, 60);
            this.dropdownCardId.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownCardId.Name = "dropdownCardId";
            this.dropdownCardId.Size = new System.Drawing.Size(183, 32);
            this.dropdownCardId.TabIndex = 2;
            this.dropdownCardId.WaveSize = 0;
            // 
            // labelCardId
            // 
            this.labelCardId.Location = new System.Drawing.Point(4, 34);
            this.labelCardId.Margin = new System.Windows.Forms.Padding(0);
            this.labelCardId.Name = "labelCardId";
            this.labelCardId.Size = new System.Drawing.Size(183, 22);
            this.labelCardId.TabIndex = 1;
            this.labelCardId.Text = "所属控制卡";
            // 
            // labelSectionMapping
            // 
            this.labelSectionMapping.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionMapping.Location = new System.Drawing.Point(4, 4);
            this.labelSectionMapping.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionMapping.Name = "labelSectionMapping";
            this.labelSectionMapping.Size = new System.Drawing.Size(183, 26);
            this.labelSectionMapping.TabIndex = 0;
            this.labelSectionMapping.Text = "归属与映射";
            // 
            // stackSectionBasic
            // 
            this.stackSectionBasic.AutoScroll = true;
            this.stackSectionBasic.Controls.Add(this.inputName);
            this.stackSectionBasic.Controls.Add(this.labelName);
            this.stackSectionBasic.Controls.Add(this.inputLogicalBit);
            this.stackSectionBasic.Controls.Add(this.labelLogicalBit);
            this.stackSectionBasic.Controls.Add(this.dropdownIoType);
            this.stackSectionBasic.Controls.Add(this.labelIoType);
            this.stackSectionBasic.Controls.Add(this.labelSectionBasic);
            this.stackSectionBasic.Gap = 4;
            this.stackSectionBasic.Location = new System.Drawing.Point(0, 0);
            this.stackSectionBasic.Margin = new System.Windows.Forms.Padding(0);
            this.stackSectionBasic.Name = "stackSectionBasic";
            this.stackSectionBasic.Padding = new System.Windows.Forms.Padding(4);
            this.stackSectionBasic.Size = new System.Drawing.Size(223, 351);
            this.stackSectionBasic.TabIndex = 0;
            this.stackSectionBasic.Text = "stackSectionBasic";
            this.stackSectionBasic.Vertical = true;
            // 
            // inputName
            // 
            this.inputName.Location = new System.Drawing.Point(4, 184);
            this.inputName.Margin = new System.Windows.Forms.Padding(0);
            this.inputName.Name = "inputName";
            this.inputName.PlaceholderText = "请输入名称";
            this.inputName.Size = new System.Drawing.Size(215, 32);
            this.inputName.TabIndex = 6;
            this.inputName.WaveSize = 0;
            // 
            // labelName
            // 
            this.labelName.Location = new System.Drawing.Point(4, 158);
            this.labelName.Margin = new System.Windows.Forms.Padding(0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(215, 22);
            this.labelName.TabIndex = 5;
            this.labelName.Text = "名称";
            // 
            // inputLogicalBit
            // 
            this.inputLogicalBit.Location = new System.Drawing.Point(4, 122);
            this.inputLogicalBit.Margin = new System.Windows.Forms.Padding(0);
            this.inputLogicalBit.Name = "inputLogicalBit";
            this.inputLogicalBit.PlaceholderText = "请输入逻辑位号";
            this.inputLogicalBit.Size = new System.Drawing.Size(215, 32);
            this.inputLogicalBit.TabIndex = 4;
            this.inputLogicalBit.WaveSize = 0;
            // 
            // labelLogicalBit
            // 
            this.labelLogicalBit.Location = new System.Drawing.Point(4, 96);
            this.labelLogicalBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelLogicalBit.Name = "labelLogicalBit";
            this.labelLogicalBit.Size = new System.Drawing.Size(215, 22);
            this.labelLogicalBit.TabIndex = 3;
            this.labelLogicalBit.Text = "逻辑位号";
            // 
            // dropdownIoType
            // 
            this.dropdownIoType.Location = new System.Drawing.Point(4, 60);
            this.dropdownIoType.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownIoType.Name = "dropdownIoType";
            this.dropdownIoType.Size = new System.Drawing.Size(215, 32);
            this.dropdownIoType.TabIndex = 2;
            this.dropdownIoType.WaveSize = 0;
            // 
            // labelIoType
            // 
            this.labelIoType.Location = new System.Drawing.Point(4, 34);
            this.labelIoType.Margin = new System.Windows.Forms.Padding(0);
            this.labelIoType.Name = "labelIoType";
            this.labelIoType.Size = new System.Drawing.Size(215, 22);
            this.labelIoType.TabIndex = 1;
            this.labelIoType.Text = "IO 类型";
            // 
            // labelSectionBasic
            // 
            this.labelSectionBasic.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionBasic.Location = new System.Drawing.Point(4, 4);
            this.labelSectionBasic.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionBasic.Name = "labelSectionBasic";
            this.labelSectionBasic.Size = new System.Drawing.Size(215, 26);
            this.labelSectionBasic.TabIndex = 0;
            this.labelSectionBasic.Text = "基础标识";
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.flowFooterButtons);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(28, 435);
            this.panelFooter.Margin = new System.Windows.Forms.Padding(0);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Padding = new System.Windows.Forms.Padding(4, 10, 4, 0);
            this.panelFooter.Radius = 0;
            this.panelFooter.Size = new System.Drawing.Size(804, 57);
            this.panelFooter.TabIndex = 2;
            // 
            // flowFooterButtons
            // 
            this.flowFooterButtons.Controls.Add(this.buttonOk);
            this.flowFooterButtons.Controls.Add(this.buttonCancel);
            this.flowFooterButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowFooterButtons.Gap = 10;
            this.flowFooterButtons.Location = new System.Drawing.Point(556, 10);
            this.flowFooterButtons.Margin = new System.Windows.Forms.Padding(0);
            this.flowFooterButtons.Name = "flowFooterButtons";
            this.flowFooterButtons.Size = new System.Drawing.Size(244, 47);
            this.flowFooterButtons.TabIndex = 0;
            this.flowFooterButtons.Text = "flowFooterButtons";
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(126, 0);
            this.buttonOk.Margin = new System.Windows.Forms.Padding(0);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Radius = 8;
            this.buttonOk.Size = new System.Drawing.Size(116, 38);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "保存";
            this.buttonOk.Type = AntdUI.TTypeMini.Primary;
            this.buttonOk.WaveSize = 0;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(0, 0);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Radius = 8;
            this.buttonCancel.Size = new System.Drawing.Size(116, 38);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.WaveSize = 0;
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.labelDialogDescription);
            this.panelHeader.Controls.Add(this.labelDialogTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(28, 28);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Padding = new System.Windows.Forms.Padding(4, 0, 4, 8);
            this.panelHeader.Radius = 0;
            this.panelHeader.Size = new System.Drawing.Size(804, 56);
            this.panelHeader.TabIndex = 0;
            // 
            // labelDialogDescription
            // 
            this.labelDialogDescription.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelDialogDescription.Location = new System.Drawing.Point(477, 0);
            this.labelDialogDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDialogDescription.Name = "labelDialogDescription";
            this.labelDialogDescription.Size = new System.Drawing.Size(323, 48);
            this.labelDialogDescription.TabIndex = 1;
            this.labelDialogDescription.Text = "填写 IO 映射配置";
            this.labelDialogDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelDialogTitle
            // 
            this.labelDialogTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelDialogTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelDialogTitle.Location = new System.Drawing.Point(4, 0);
            this.labelDialogTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelDialogTitle.Name = "labelDialogTitle";
            this.labelDialogTitle.Size = new System.Drawing.Size(352, 48);
            this.labelDialogTitle.TabIndex = 0;
            this.labelDialogTitle.Text = "新增 IO 映射";
            // 
            // MotionIoMapEditDialog
            // 
            this.ClientSize = new System.Drawing.Size(860, 520);
            this.Controls.Add(this.textureBackgroundDialog);
            this.Name = "MotionIoMapEditDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "IO 映射编辑";
            this.textureBackgroundDialog.ResumeLayout(false);
            this.panelShell.ResumeLayout(false);
            this.panelContent.ResumeLayout(false);
            this.gridMainSections.ResumeLayout(false);
            this.stackSectionRemark.ResumeLayout(false);
            this.stackSectionRuntime.ResumeLayout(false);
            this.stackSectionRuntime.PerformLayout();
            this.stackSectionMapping.ResumeLayout(false);
            this.stackSectionBasic.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            this.flowFooterButtons.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AMControlWinF.Views.Main.TextureBackgroundControl textureBackgroundDialog;
        private AntdUI.Panel panelShell;
        private AntdUI.Panel panelContent;
        private AntdUI.GridPanel gridMainSections;

        private AntdUI.StackPanel stackSectionBasic;
        private AntdUI.Label labelSectionBasic;
        private AntdUI.Label labelIoType;
        private AntdUI.Select dropdownIoType;
        private AntdUI.Label labelLogicalBit;
        private AntdUI.Input inputLogicalBit;
        private AntdUI.Label labelName;
        private AntdUI.Input inputName;

        private AntdUI.StackPanel stackSectionMapping;
        private AntdUI.Label labelSectionMapping;
        private AntdUI.Label labelCardId;
        private AntdUI.Select dropdownCardId;
        private AntdUI.Label labelCore;
        private AntdUI.Select dropdownCore;
        private AntdUI.Label labelHardwareBit;
        private AntdUI.Input inputHardwareBit;

        private AntdUI.StackPanel stackSectionRuntime;
        private AntdUI.Label labelSectionRuntime;
        private AntdUI.Label labelIsExtModule;
        private AntdUI.Checkbox checkIsExtModule;
        private AntdUI.Label labelEnabled;
        private AntdUI.Checkbox checkIsEnabled;
        private AntdUI.Label labelSortOrder;
        private AntdUI.Input inputSortOrder;

        private AntdUI.StackPanel stackSectionRemark;
        private AntdUI.Label labelSectionRemark;
        private AntdUI.Label labelRemark;
        private AntdUI.Input inputRemark;

        private AntdUI.Panel panelFooter;
        private AntdUI.FlowPanel flowFooterButtons;
        private AntdUI.Button buttonOk;
        private AntdUI.Button buttonCancel;

        private AntdUI.Panel panelHeader;
        private AntdUI.Label labelDialogDescription;
        private AntdUI.Label labelDialogTitle;
    }
}