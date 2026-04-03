namespace AMControlWinF.Views.MotionConfig
{
    partial class MotionCardEditDialog
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
            this.inputDescription = new AntdUI.Input();
            this.labelDescription = new AntdUI.Label();
            this.labelSectionRemark = new AntdUI.Label();
            this.stackSectionInitRight = new AntdUI.StackPanel();
            this.inputSortOrder = new AntdUI.Input();
            this.labelSortOrder = new AntdUI.Label();
            this.checkIsEnabled = new AntdUI.Checkbox();
            this.labelStateFlags = new AntdUI.Label();
            this.labelSectionState = new AntdUI.Label();
            this.inputInitOrder = new AntdUI.Input();
            this.labelInitOrder = new AntdUI.Label();
            this.inputAxisCount = new AntdUI.Input();
            this.labelAxisCount = new AntdUI.Label();
            this.stackSectionInitLeft = new AntdUI.StackPanel();
            this.checkUseExtModule = new AntdUI.Checkbox();
            this.labelInitFlags = new AntdUI.Label();
            this.inputOpenConfig = new AntdUI.Input();
            this.labelOpenConfig = new AntdUI.Label();
            this.inputModeParam = new AntdUI.Input();
            this.labelModeParam = new AntdUI.Label();
            this.inputCoreNumber = new AntdUI.Input();
            this.labelCoreNumber = new AntdUI.Label();
            this.labelSectionInit = new AntdUI.Label();
            this.stackSectionBasic = new AntdUI.StackPanel();
            this.inputDriverKey = new AntdUI.Input();
            this.labelDriverKey = new AntdUI.Label();
            this.inputDisplayName = new AntdUI.Input();
            this.labelDisplayName = new AntdUI.Label();
            this.inputName = new AntdUI.Input();
            this.labelName = new AntdUI.Label();
            this.dropdownCardType = new AntdUI.Select();
            this.labelCardType = new AntdUI.Label();
            this.inputCardId = new AntdUI.Input();
            this.labelCardId = new AntdUI.Label();
            this.labelSectionBasic = new AntdUI.Label();
            this.panelFooter = new AntdUI.Panel();
            this.flowFooterButtons = new AntdUI.FlowPanel();
            this.buttonOk = new AntdUI.Button();
            this.buttonCancel = new AntdUI.Button();
            this.panelHeader = new AntdUI.Panel();
            this.flowHeaderRight = new AntdUI.FlowPanel();
            this.labelDialogDescription = new AntdUI.Label();
            this.flowHeaderLeft = new AntdUI.FlowPanel();
            this.labelDialogTitle = new AntdUI.Label();
            this.labelSectionInit_right = new AntdUI.Label();
            this.textureBackgroundDialog.SuspendLayout();
            this.panelShell.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.gridMainSections.SuspendLayout();
            this.stackSectionRemark.SuspendLayout();
            this.stackSectionInitRight.SuspendLayout();
            this.stackSectionInitLeft.SuspendLayout();
            this.stackSectionBasic.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.flowFooterButtons.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.flowHeaderRight.SuspendLayout();
            this.flowHeaderLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // textureBackgroundDialog
            // 
            this.textureBackgroundDialog.Controls.Add(this.panelShell);
            this.textureBackgroundDialog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textureBackgroundDialog.Location = new System.Drawing.Point(0, 0);
            this.textureBackgroundDialog.Margin = new System.Windows.Forms.Padding(0);
            this.textureBackgroundDialog.Name = "textureBackgroundDialog";
            this.textureBackgroundDialog.Size = new System.Drawing.Size(900, 600);
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
            this.panelShell.ShadowOpacityAnimation = true;
            this.panelShell.Size = new System.Drawing.Size(900, 600);
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
            this.panelContent.Size = new System.Drawing.Size(844, 431);
            this.panelContent.TabIndex = 1;
            // 
            // gridMainSections
            // 
            this.gridMainSections.Controls.Add(this.stackSectionRemark);
            this.gridMainSections.Controls.Add(this.stackSectionInitRight);
            this.gridMainSections.Controls.Add(this.stackSectionInitLeft);
            this.gridMainSections.Controls.Add(this.stackSectionBasic);
            this.gridMainSections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridMainSections.Location = new System.Drawing.Point(4, 0);
            this.gridMainSections.Margin = new System.Windows.Forms.Padding(0);
            this.gridMainSections.Name = "gridMainSections";
            this.gridMainSections.Size = new System.Drawing.Size(836, 431);
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
            this.stackSectionRemark.Controls.Add(this.labelSectionRemark);
            this.stackSectionRemark.Gap = 4;
            this.stackSectionRemark.Location = new System.Drawing.Point(585, 0);
            this.stackSectionRemark.Margin = new System.Windows.Forms.Padding(0);
            this.stackSectionRemark.Name = "stackSectionRemark";
            this.stackSectionRemark.Padding = new System.Windows.Forms.Padding(4);
            this.stackSectionRemark.Size = new System.Drawing.Size(251, 431);
            this.stackSectionRemark.TabIndex = 3;
            this.stackSectionRemark.Text = "stackSectionRemark";
            this.stackSectionRemark.Vertical = true;
            // 
            // inputRemark
            // 
            this.inputRemark.Location = new System.Drawing.Point(4, 150);
            this.inputRemark.Margin = new System.Windows.Forms.Padding(0);
            this.inputRemark.Multiline = true;
            this.inputRemark.Name = "inputRemark";
            this.inputRemark.PlaceholderText = "请输入备注";
            this.inputRemark.Size = new System.Drawing.Size(243, 60);
            this.inputRemark.TabIndex = 4;
            this.inputRemark.WaveSize = 0;
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(4, 124);
            this.labelRemark.Margin = new System.Windows.Forms.Padding(0);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(243, 22);
            this.labelRemark.TabIndex = 3;
            this.labelRemark.Text = "备注";
            // 
            // inputDescription
            // 
            this.inputDescription.Location = new System.Drawing.Point(4, 60);
            this.inputDescription.Margin = new System.Windows.Forms.Padding(0);
            this.inputDescription.Multiline = true;
            this.inputDescription.Name = "inputDescription";
            this.inputDescription.PlaceholderText = "请输入描述";
            this.inputDescription.Size = new System.Drawing.Size(243, 60);
            this.inputDescription.TabIndex = 2;
            this.inputDescription.WaveSize = 0;
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(4, 34);
            this.labelDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(243, 22);
            this.labelDescription.TabIndex = 1;
            this.labelDescription.Text = "描述";
            // 
            // labelSectionRemark
            // 
            this.labelSectionRemark.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionRemark.Location = new System.Drawing.Point(4, 4);
            this.labelSectionRemark.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionRemark.Name = "labelSectionRemark";
            this.labelSectionRemark.Size = new System.Drawing.Size(243, 26);
            this.labelSectionRemark.TabIndex = 0;
            this.labelSectionRemark.Text = "说明与备注";
            // 
            // stackSectionInitRight
            // 
            this.stackSectionInitRight.AutoScroll = true;
            this.stackSectionInitRight.Controls.Add(this.inputSortOrder);
            this.stackSectionInitRight.Controls.Add(this.labelSortOrder);
            this.stackSectionInitRight.Controls.Add(this.checkIsEnabled);
            this.stackSectionInitRight.Controls.Add(this.labelStateFlags);
            this.stackSectionInitRight.Controls.Add(this.labelSectionState);
            this.stackSectionInitRight.Controls.Add(this.inputInitOrder);
            this.stackSectionInitRight.Controls.Add(this.labelInitOrder);
            this.stackSectionInitRight.Controls.Add(this.inputAxisCount);
            this.stackSectionInitRight.Controls.Add(this.labelAxisCount);
            this.stackSectionInitRight.Controls.Add(this.labelSectionInit_right);
            this.stackSectionInitRight.Gap = 4;
            this.stackSectionInitRight.Location = new System.Drawing.Point(418, 0);
            this.stackSectionInitRight.Margin = new System.Windows.Forms.Padding(0);
            this.stackSectionInitRight.Name = "stackSectionInitRight";
            this.stackSectionInitRight.Padding = new System.Windows.Forms.Padding(4);
            this.stackSectionInitRight.Size = new System.Drawing.Size(167, 431);
            this.stackSectionInitRight.TabIndex = 2;
            this.stackSectionInitRight.Text = "stackSectionInitRight";
            this.stackSectionInitRight.Vertical = true;
            // 
            // inputSortOrder
            // 
            this.inputSortOrder.Location = new System.Drawing.Point(4, 278);
            this.inputSortOrder.Margin = new System.Windows.Forms.Padding(0);
            this.inputSortOrder.Name = "inputSortOrder";
            this.inputSortOrder.PlaceholderText = "请输入排序号";
            this.inputSortOrder.Size = new System.Drawing.Size(159, 32);
            this.inputSortOrder.TabIndex = 8;
            this.inputSortOrder.WaveSize = 0;
            // 
            // labelSortOrder
            // 
            this.labelSortOrder.Location = new System.Drawing.Point(4, 252);
            this.labelSortOrder.Margin = new System.Windows.Forms.Padding(0);
            this.labelSortOrder.Name = "labelSortOrder";
            this.labelSortOrder.Size = new System.Drawing.Size(159, 22);
            this.labelSortOrder.TabIndex = 7;
            this.labelSortOrder.Text = "排序号";
            // 
            // checkIsEnabled
            // 
            this.checkIsEnabled.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkIsEnabled.Checked = true;
            this.checkIsEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkIsEnabled.Location = new System.Drawing.Point(4, 214);
            this.checkIsEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.checkIsEnabled.Name = "checkIsEnabled";
            this.checkIsEnabled.Size = new System.Drawing.Size(114, 34);
            this.checkIsEnabled.TabIndex = 6;
            this.checkIsEnabled.Text = "启用此控制卡";
            // 
            // labelStateFlags
            // 
            this.labelStateFlags.Location = new System.Drawing.Point(4, 188);
            this.labelStateFlags.Margin = new System.Windows.Forms.Padding(0);
            this.labelStateFlags.Name = "labelStateFlags";
            this.labelStateFlags.Size = new System.Drawing.Size(159, 22);
            this.labelStateFlags.TabIndex = 5;
            this.labelStateFlags.Text = "启用状态";
            // 
            // labelSectionState
            // 
            this.labelSectionState.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionState.Location = new System.Drawing.Point(4, 158);
            this.labelSectionState.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionState.Name = "labelSectionState";
            this.labelSectionState.Size = new System.Drawing.Size(159, 26);
            this.labelSectionState.TabIndex = 4;
            this.labelSectionState.Text = "状态与排序";
            // 
            // inputInitOrder
            // 
            this.inputInitOrder.Location = new System.Drawing.Point(4, 122);
            this.inputInitOrder.Margin = new System.Windows.Forms.Padding(0);
            this.inputInitOrder.Name = "inputInitOrder";
            this.inputInitOrder.PlaceholderText = "请输入初始化顺序";
            this.inputInitOrder.Size = new System.Drawing.Size(159, 32);
            this.inputInitOrder.TabIndex = 3;
            this.inputInitOrder.WaveSize = 0;
            // 
            // labelInitOrder
            // 
            this.labelInitOrder.Location = new System.Drawing.Point(4, 96);
            this.labelInitOrder.Margin = new System.Windows.Forms.Padding(0);
            this.labelInitOrder.Name = "labelInitOrder";
            this.labelInitOrder.Size = new System.Drawing.Size(159, 22);
            this.labelInitOrder.TabIndex = 2;
            this.labelInitOrder.Text = "初始化顺序";
            // 
            // inputAxisCount
            // 
            this.inputAxisCount.Location = new System.Drawing.Point(4, 60);
            this.inputAxisCount.Margin = new System.Windows.Forms.Padding(0);
            this.inputAxisCount.Name = "inputAxisCount";
            this.inputAxisCount.PlaceholderText = "请输入支持轴总数";
            this.inputAxisCount.Size = new System.Drawing.Size(159, 32);
            this.inputAxisCount.TabIndex = 1;
            this.inputAxisCount.WaveSize = 0;
            // 
            // labelAxisCount
            // 
            this.labelAxisCount.Location = new System.Drawing.Point(4, 4);
            this.labelAxisCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelAxisCount.Name = "labelAxisCount";
            this.labelAxisCount.Size = new System.Drawing.Size(159, 22);
            this.labelAxisCount.TabIndex = 0;
            this.labelAxisCount.Text = "支持轴总数";
            // 
            // stackSectionInitLeft
            // 
            this.stackSectionInitLeft.AutoScroll = true;
            this.stackSectionInitLeft.Controls.Add(this.checkUseExtModule);
            this.stackSectionInitLeft.Controls.Add(this.labelInitFlags);
            this.stackSectionInitLeft.Controls.Add(this.inputOpenConfig);
            this.stackSectionInitLeft.Controls.Add(this.labelOpenConfig);
            this.stackSectionInitLeft.Controls.Add(this.inputModeParam);
            this.stackSectionInitLeft.Controls.Add(this.labelModeParam);
            this.stackSectionInitLeft.Controls.Add(this.inputCoreNumber);
            this.stackSectionInitLeft.Controls.Add(this.labelCoreNumber);
            this.stackSectionInitLeft.Controls.Add(this.labelSectionInit);
            this.stackSectionInitLeft.Gap = 4;
            this.stackSectionInitLeft.Location = new System.Drawing.Point(251, 0);
            this.stackSectionInitLeft.Margin = new System.Windows.Forms.Padding(0);
            this.stackSectionInitLeft.Name = "stackSectionInitLeft";
            this.stackSectionInitLeft.Padding = new System.Windows.Forms.Padding(4);
            this.stackSectionInitLeft.Size = new System.Drawing.Size(167, 431);
            this.stackSectionInitLeft.TabIndex = 1;
            this.stackSectionInitLeft.Text = "stackSectionInitLeft";
            this.stackSectionInitLeft.Vertical = true;
            // 
            // checkUseExtModule
            // 
            this.checkUseExtModule.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkUseExtModule.Location = new System.Drawing.Point(4, 246);
            this.checkUseExtModule.Margin = new System.Windows.Forms.Padding(0);
            this.checkUseExtModule.Name = "checkUseExtModule";
            this.checkUseExtModule.Size = new System.Drawing.Size(137, 34);
            this.checkUseExtModule.TabIndex = 8;
            this.checkUseExtModule.Text = "启用扩展 IO 模块";
            // 
            // labelInitFlags
            // 
            this.labelInitFlags.Location = new System.Drawing.Point(4, 220);
            this.labelInitFlags.Margin = new System.Windows.Forms.Padding(0);
            this.labelInitFlags.Name = "labelInitFlags";
            this.labelInitFlags.Size = new System.Drawing.Size(159, 22);
            this.labelInitFlags.TabIndex = 7;
            this.labelInitFlags.Text = "扩展模块";
            // 
            // inputOpenConfig
            // 
            this.inputOpenConfig.Location = new System.Drawing.Point(4, 184);
            this.inputOpenConfig.Margin = new System.Windows.Forms.Padding(0);
            this.inputOpenConfig.Name = "inputOpenConfig";
            this.inputOpenConfig.PlaceholderText = "请输入初始参数（可选）";
            this.inputOpenConfig.Size = new System.Drawing.Size(159, 32);
            this.inputOpenConfig.TabIndex = 6;
            this.inputOpenConfig.WaveSize = 0;
            // 
            // labelOpenConfig
            // 
            this.labelOpenConfig.Location = new System.Drawing.Point(4, 158);
            this.labelOpenConfig.Margin = new System.Windows.Forms.Padding(0);
            this.labelOpenConfig.Name = "labelOpenConfig";
            this.labelOpenConfig.Size = new System.Drawing.Size(159, 22);
            this.labelOpenConfig.TabIndex = 5;
            this.labelOpenConfig.Text = "初始参数（OpenConfig）";
            // 
            // inputModeParam
            // 
            this.inputModeParam.Location = new System.Drawing.Point(4, 122);
            this.inputModeParam.Margin = new System.Windows.Forms.Padding(0);
            this.inputModeParam.Name = "inputModeParam";
            this.inputModeParam.PlaceholderText = "请输入打开模式参数";
            this.inputModeParam.Size = new System.Drawing.Size(159, 32);
            this.inputModeParam.TabIndex = 4;
            this.inputModeParam.WaveSize = 0;
            // 
            // labelModeParam
            // 
            this.labelModeParam.Location = new System.Drawing.Point(4, 96);
            this.labelModeParam.Margin = new System.Windows.Forms.Padding(0);
            this.labelModeParam.Name = "labelModeParam";
            this.labelModeParam.Size = new System.Drawing.Size(159, 22);
            this.labelModeParam.TabIndex = 3;
            this.labelModeParam.Text = "打开模式参数";
            // 
            // inputCoreNumber
            // 
            this.inputCoreNumber.Location = new System.Drawing.Point(4, 60);
            this.inputCoreNumber.Margin = new System.Windows.Forms.Padding(0);
            this.inputCoreNumber.Name = "inputCoreNumber";
            this.inputCoreNumber.PlaceholderText = "请输入内核数量";
            this.inputCoreNumber.Size = new System.Drawing.Size(159, 32);
            this.inputCoreNumber.TabIndex = 2;
            this.inputCoreNumber.WaveSize = 0;
            // 
            // labelCoreNumber
            // 
            this.labelCoreNumber.Location = new System.Drawing.Point(4, 34);
            this.labelCoreNumber.Margin = new System.Windows.Forms.Padding(0);
            this.labelCoreNumber.Name = "labelCoreNumber";
            this.labelCoreNumber.Size = new System.Drawing.Size(159, 22);
            this.labelCoreNumber.TabIndex = 1;
            this.labelCoreNumber.Text = "内核数量";
            // 
            // labelSectionInit
            // 
            this.labelSectionInit.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionInit.Location = new System.Drawing.Point(4, 4);
            this.labelSectionInit.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionInit.Name = "labelSectionInit";
            this.labelSectionInit.Size = new System.Drawing.Size(159, 26);
            this.labelSectionInit.TabIndex = 0;
            this.labelSectionInit.Text = "初始化参数（左侧）";
            // 
            // stackSectionBasic
            // 
            this.stackSectionBasic.AutoScroll = true;
            this.stackSectionBasic.Controls.Add(this.inputDriverKey);
            this.stackSectionBasic.Controls.Add(this.labelDriverKey);
            this.stackSectionBasic.Controls.Add(this.inputDisplayName);
            this.stackSectionBasic.Controls.Add(this.labelDisplayName);
            this.stackSectionBasic.Controls.Add(this.inputName);
            this.stackSectionBasic.Controls.Add(this.labelName);
            this.stackSectionBasic.Controls.Add(this.dropdownCardType);
            this.stackSectionBasic.Controls.Add(this.labelCardType);
            this.stackSectionBasic.Controls.Add(this.inputCardId);
            this.stackSectionBasic.Controls.Add(this.labelCardId);
            this.stackSectionBasic.Controls.Add(this.labelSectionBasic);
            this.stackSectionBasic.Gap = 4;
            this.stackSectionBasic.Location = new System.Drawing.Point(0, 0);
            this.stackSectionBasic.Margin = new System.Windows.Forms.Padding(0);
            this.stackSectionBasic.Name = "stackSectionBasic";
            this.stackSectionBasic.Padding = new System.Windows.Forms.Padding(4);
            this.stackSectionBasic.Size = new System.Drawing.Size(251, 431);
            this.stackSectionBasic.TabIndex = 0;
            this.stackSectionBasic.Text = "stackSectionBasic";
            this.stackSectionBasic.Vertical = true;
            // 
            // inputDriverKey
            // 
            this.inputDriverKey.Location = new System.Drawing.Point(4, 308);
            this.inputDriverKey.Margin = new System.Windows.Forms.Padding(0);
            this.inputDriverKey.Name = "inputDriverKey";
            this.inputDriverKey.PlaceholderText = "请输入驱动识别键（DriverKey）";
            this.inputDriverKey.Size = new System.Drawing.Size(243, 32);
            this.inputDriverKey.TabIndex = 10;
            this.inputDriverKey.WaveSize = 0;
            // 
            // labelDriverKey
            // 
            this.labelDriverKey.Location = new System.Drawing.Point(4, 282);
            this.labelDriverKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelDriverKey.Name = "labelDriverKey";
            this.labelDriverKey.Size = new System.Drawing.Size(243, 22);
            this.labelDriverKey.TabIndex = 9;
            this.labelDriverKey.Text = "驱动识别键（DriverKey）";
            // 
            // inputDisplayName
            // 
            this.inputDisplayName.Location = new System.Drawing.Point(4, 246);
            this.inputDisplayName.Margin = new System.Windows.Forms.Padding(0);
            this.inputDisplayName.Name = "inputDisplayName";
            this.inputDisplayName.PlaceholderText = "请输入显示名称";
            this.inputDisplayName.Size = new System.Drawing.Size(243, 32);
            this.inputDisplayName.TabIndex = 8;
            this.inputDisplayName.WaveSize = 0;
            // 
            // labelDisplayName
            // 
            this.labelDisplayName.Location = new System.Drawing.Point(4, 220);
            this.labelDisplayName.Margin = new System.Windows.Forms.Padding(0);
            this.labelDisplayName.Name = "labelDisplayName";
            this.labelDisplayName.Size = new System.Drawing.Size(243, 22);
            this.labelDisplayName.TabIndex = 7;
            this.labelDisplayName.Text = "显示名称（界面展示用）";
            // 
            // inputName
            // 
            this.inputName.Location = new System.Drawing.Point(4, 184);
            this.inputName.Margin = new System.Windows.Forms.Padding(0);
            this.inputName.Name = "inputName";
            this.inputName.PlaceholderText = "请输入内部名称";
            this.inputName.Size = new System.Drawing.Size(243, 32);
            this.inputName.TabIndex = 6;
            this.inputName.WaveSize = 0;
            // 
            // labelName
            // 
            this.labelName.Location = new System.Drawing.Point(4, 158);
            this.labelName.Margin = new System.Windows.Forms.Padding(0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(243, 22);
            this.labelName.TabIndex = 5;
            this.labelName.Text = "内部名称 *";
            // 
            // dropdownCardType
            // 
            this.dropdownCardType.Location = new System.Drawing.Point(4, 122);
            this.dropdownCardType.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownCardType.Name = "dropdownCardType";
            this.dropdownCardType.Size = new System.Drawing.Size(243, 32);
            this.dropdownCardType.TabIndex = 4;
            this.dropdownCardType.WaveSize = 0;
            // 
            // labelCardType
            // 
            this.labelCardType.Location = new System.Drawing.Point(4, 96);
            this.labelCardType.Margin = new System.Windows.Forms.Padding(0);
            this.labelCardType.Name = "labelCardType";
            this.labelCardType.Size = new System.Drawing.Size(243, 22);
            this.labelCardType.TabIndex = 3;
            this.labelCardType.Text = "控制卡类型";
            // 
            // inputCardId
            // 
            this.inputCardId.Location = new System.Drawing.Point(4, 60);
            this.inputCardId.Margin = new System.Windows.Forms.Padding(0);
            this.inputCardId.Name = "inputCardId";
            this.inputCardId.PlaceholderText = "请输入硬件卡号（Card Id）";
            this.inputCardId.Size = new System.Drawing.Size(243, 32);
            this.inputCardId.TabIndex = 2;
            this.inputCardId.WaveSize = 0;
            // 
            // labelCardId
            // 
            this.labelCardId.Location = new System.Drawing.Point(4, 34);
            this.labelCardId.Margin = new System.Windows.Forms.Padding(0);
            this.labelCardId.Name = "labelCardId";
            this.labelCardId.Size = new System.Drawing.Size(243, 22);
            this.labelCardId.TabIndex = 1;
            this.labelCardId.Text = "硬件卡号（Card Id）";
            // 
            // labelSectionBasic
            // 
            this.labelSectionBasic.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionBasic.Location = new System.Drawing.Point(4, 4);
            this.labelSectionBasic.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionBasic.Name = "labelSectionBasic";
            this.labelSectionBasic.Size = new System.Drawing.Size(243, 26);
            this.labelSectionBasic.TabIndex = 0;
            this.labelSectionBasic.Text = "基础标识";
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.flowFooterButtons);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(28, 515);
            this.panelFooter.Margin = new System.Windows.Forms.Padding(0);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Padding = new System.Windows.Forms.Padding(4, 10, 4, 0);
            this.panelFooter.Radius = 0;
            this.panelFooter.Size = new System.Drawing.Size(844, 57);
            this.panelFooter.TabIndex = 2;
            // 
            // flowFooterButtons
            // 
            this.flowFooterButtons.Controls.Add(this.buttonOk);
            this.flowFooterButtons.Controls.Add(this.buttonCancel);
            this.flowFooterButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowFooterButtons.Gap = 10;
            this.flowFooterButtons.Location = new System.Drawing.Point(596, 10);
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
            this.buttonOk.TabIndex = 1;
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
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.WaveSize = 0;
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.flowHeaderRight);
            this.panelHeader.Controls.Add(this.flowHeaderLeft);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(28, 28);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Padding = new System.Windows.Forms.Padding(4, 0, 4, 8);
            this.panelHeader.Radius = 0;
            this.panelHeader.Size = new System.Drawing.Size(844, 56);
            this.panelHeader.TabIndex = 0;
            // 
            // flowHeaderRight
            // 
            this.flowHeaderRight.Controls.Add(this.labelDialogDescription);
            this.flowHeaderRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowHeaderRight.Location = new System.Drawing.Point(593, 0);
            this.flowHeaderRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowHeaderRight.Name = "flowHeaderRight";
            this.flowHeaderRight.Size = new System.Drawing.Size(247, 48);
            this.flowHeaderRight.TabIndex = 1;
            this.flowHeaderRight.Text = "flowHeaderRight";
            // 
            // labelDialogDescription
            // 
            this.labelDialogDescription.Location = new System.Drawing.Point(0, 0);
            this.labelDialogDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDialogDescription.Name = "labelDialogDescription";
            this.labelDialogDescription.Size = new System.Drawing.Size(247, 48);
            this.labelDialogDescription.TabIndex = 0;
            this.labelDialogDescription.Text = "填写控制卡信息。";
            this.labelDialogDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // flowHeaderLeft
            // 
            this.flowHeaderLeft.Controls.Add(this.labelDialogTitle);
            this.flowHeaderLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowHeaderLeft.Location = new System.Drawing.Point(4, 0);
            this.flowHeaderLeft.Margin = new System.Windows.Forms.Padding(0);
            this.flowHeaderLeft.Name = "flowHeaderLeft";
            this.flowHeaderLeft.Size = new System.Drawing.Size(220, 48);
            this.flowHeaderLeft.TabIndex = 0;
            this.flowHeaderLeft.Text = "flowHeaderLeft";
            // 
            // labelDialogTitle
            // 
            this.labelDialogTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelDialogTitle.Location = new System.Drawing.Point(0, 0);
            this.labelDialogTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelDialogTitle.Name = "labelDialogTitle";
            this.labelDialogTitle.Size = new System.Drawing.Size(220, 48);
            this.labelDialogTitle.TabIndex = 0;
            this.labelDialogTitle.Text = "新增控制卡";
            // 
            // labelSectionInit_right
            // 
            this.labelSectionInit_right.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionInit_right.Location = new System.Drawing.Point(4, 30);
            this.labelSectionInit_right.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionInit_right.Name = "labelSectionInit_right";
            this.labelSectionInit_right.Size = new System.Drawing.Size(159, 26);
            this.labelSectionInit_right.TabIndex = 0;
            this.labelSectionInit_right.Text = "初始化参数（右侧）";
            // 
            // MotionCardEditDialog
            // 
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.textureBackgroundDialog);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MotionCardEditDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "控制卡编辑";
            this.textureBackgroundDialog.ResumeLayout(false);
            this.panelShell.ResumeLayout(false);
            this.panelContent.ResumeLayout(false);
            this.gridMainSections.ResumeLayout(false);
            this.stackSectionRemark.ResumeLayout(false);
            this.stackSectionInitRight.ResumeLayout(false);
            this.stackSectionInitRight.PerformLayout();
            this.stackSectionInitLeft.ResumeLayout(false);
            this.stackSectionInitLeft.PerformLayout();
            this.stackSectionBasic.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            this.flowFooterButtons.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.flowHeaderRight.ResumeLayout(false);
            this.flowHeaderLeft.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AMControlWinF.Views.Main.TextureBackgroundControl textureBackgroundDialog;
        private AntdUI.Panel panelShell;
        private AntdUI.Panel panelHeader;
        private AntdUI.FlowPanel flowHeaderLeft;
        private AntdUI.Label labelDialogTitle;
        private AntdUI.FlowPanel flowHeaderRight;
        private AntdUI.Label labelDialogDescription;
        private AntdUI.Panel panelContent;

        private AntdUI.GridPanel gridMainSections;
        private AntdUI.StackPanel stackSectionBasic;
        private AntdUI.StackPanel stackSectionInitLeft;
        private AntdUI.StackPanel stackSectionInitRight;
        private AntdUI.StackPanel stackSectionRemark;

        private AntdUI.Label labelSectionBasic;
        private AntdUI.Label labelCardId;
        private AntdUI.Input inputCardId;
        private AntdUI.Label labelCardType;
        private AntdUI.Select dropdownCardType;
        private AntdUI.Label labelName;
        private AntdUI.Input inputName;
        private AntdUI.Label labelDisplayName;
        private AntdUI.Input inputDisplayName;
        private AntdUI.Label labelDriverKey;
        private AntdUI.Input inputDriverKey;

        private AntdUI.Label labelSectionInit;
        private AntdUI.Label labelCoreNumber;
        private AntdUI.Input inputCoreNumber;
        private AntdUI.Label labelModeParam;
        private AntdUI.Input inputModeParam;
        private AntdUI.Label labelOpenConfig;
        private AntdUI.Input inputOpenConfig;
        private AntdUI.Label labelInitFlags;
        private AntdUI.Checkbox checkUseExtModule;

        private AntdUI.Label labelAxisCount;
        private AntdUI.Input inputAxisCount;
        private AntdUI.Label labelInitOrder;
        private AntdUI.Input inputInitOrder;
        private AntdUI.Label labelSectionState;
        private AntdUI.Label labelStateFlags;
        private AntdUI.Checkbox checkIsEnabled;
        private AntdUI.Label labelSortOrder;
        private AntdUI.Input inputSortOrder;

        private AntdUI.Label labelSectionRemark;
        private AntdUI.Label labelDescription;
        private AntdUI.Input inputDescription;
        private AntdUI.Label labelRemark;
        private AntdUI.Input inputRemark;

        private AntdUI.Panel panelFooter;
        private AntdUI.FlowPanel flowFooterButtons;
        private AntdUI.Button buttonOk;
        private AntdUI.Button buttonCancel;
        private AntdUI.Label labelSectionInit_right;
    }
}