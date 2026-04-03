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
            this.stackFormRows = new AntdUI.StackPanel();
            this.panelRowRemark = new AntdUI.Panel();
            this.inputRemark = new AntdUI.Input();
            this.labelRemark = new AntdUI.Label();
            this.panelRowDescription = new AntdUI.Panel();
            this.inputDescription = new AntdUI.Input();
            this.labelDescription = new AntdUI.Label();
            this.panelRowOpenConfig = new AntdUI.Panel();
            this.inputOpenConfig = new AntdUI.Input();
            this.labelOpenConfig = new AntdUI.Label();
            this.panelRowFlags = new AntdUI.Panel();
            this.flowFlags = new AntdUI.FlowPanel();
            this.checkIsEnabled = new AntdUI.Checkbox();
            this.checkUseExtModule = new AntdUI.Checkbox();
            this.labelFlags = new AntdUI.Label();
            this.panelRowSortOrder = new AntdUI.Panel();
            this.inputSortOrder = new AntdUI.Input();
            this.labelSortOrder = new AntdUI.Label();
            this.panelRowInitOrder = new AntdUI.Panel();
            this.inputInitOrder = new AntdUI.Input();
            this.labelInitOrder = new AntdUI.Label();
            this.panelRowModeParam = new AntdUI.Panel();
            this.inputModeParam = new AntdUI.Input();
            this.labelModeParam = new AntdUI.Label();
            this.panelRowAxisCount = new AntdUI.Panel();
            this.inputAxisCount = new AntdUI.Input();
            this.labelAxisCount = new AntdUI.Label();
            this.panelRowCoreNumber = new AntdUI.Panel();
            this.inputCoreNumber = new AntdUI.Input();
            this.labelCoreNumber = new AntdUI.Label();
            this.panelRowDriverKey = new AntdUI.Panel();
            this.inputDriverKey = new AntdUI.Input();
            this.labelDriverKey = new AntdUI.Label();
            this.panelRowDisplayName = new AntdUI.Panel();
            this.inputDisplayName = new AntdUI.Input();
            this.labelDisplayName = new AntdUI.Label();
            this.panelRowName = new AntdUI.Panel();
            this.inputName = new AntdUI.Input();
            this.labelName = new AntdUI.Label();
            this.panelRowCardType = new AntdUI.Panel();
            this.dropdownCardType = new AntdUI.Select();
            this.labelCardType = new AntdUI.Label();
            this.panelRowCardId = new AntdUI.Panel();
            this.inputCardId = new AntdUI.Input();
            this.labelCardId = new AntdUI.Label();
            this.panelFooter = new AntdUI.Panel();
            this.flowFooterButtons = new AntdUI.FlowPanel();
            this.buttonOk = new AntdUI.Button();
            this.buttonCancel = new AntdUI.Button();
            this.panelHeader = new AntdUI.Panel();
            this.flowHeaderRight = new AntdUI.FlowPanel();
            this.labelDialogDescription = new AntdUI.Label();
            this.flowHeaderLeft = new AntdUI.FlowPanel();
            this.labelDialogTitle = new AntdUI.Label();
            this.textureBackgroundDialog.SuspendLayout();
            this.panelShell.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.stackFormRows.SuspendLayout();
            this.panelRowRemark.SuspendLayout();
            this.panelRowDescription.SuspendLayout();
            this.panelRowOpenConfig.SuspendLayout();
            this.panelRowFlags.SuspendLayout();
            this.flowFlags.SuspendLayout();
            this.panelRowSortOrder.SuspendLayout();
            this.panelRowInitOrder.SuspendLayout();
            this.panelRowModeParam.SuspendLayout();
            this.panelRowAxisCount.SuspendLayout();
            this.panelRowCoreNumber.SuspendLayout();
            this.panelRowDriverKey.SuspendLayout();
            this.panelRowDisplayName.SuspendLayout();
            this.panelRowName.SuspendLayout();
            this.panelRowCardType.SuspendLayout();
            this.panelRowCardId.SuspendLayout();
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
            this.textureBackgroundDialog.Size = new System.Drawing.Size(640, 760);
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
            this.panelShell.Size = new System.Drawing.Size(640, 760);
            this.panelShell.TabIndex = 0;
            // 
            // panelContent
            // 
            this.panelContent.Controls.Add(this.stackFormRows);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(28, 84);
            this.panelContent.Margin = new System.Windows.Forms.Padding(0);
            this.panelContent.Name = "panelContent";
            this.panelContent.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.panelContent.Radius = 0;
            this.panelContent.Size = new System.Drawing.Size(584, 591);
            this.panelContent.TabIndex = 1;
            // 
            // stackFormRows
            // 
            this.stackFormRows.AutoScroll = true;
            this.stackFormRows.Controls.Add(this.panelRowRemark);
            this.stackFormRows.Controls.Add(this.panelRowDescription);
            this.stackFormRows.Controls.Add(this.panelRowOpenConfig);
            this.stackFormRows.Controls.Add(this.panelRowFlags);
            this.stackFormRows.Controls.Add(this.panelRowSortOrder);
            this.stackFormRows.Controls.Add(this.panelRowInitOrder);
            this.stackFormRows.Controls.Add(this.panelRowModeParam);
            this.stackFormRows.Controls.Add(this.panelRowAxisCount);
            this.stackFormRows.Controls.Add(this.panelRowCoreNumber);
            this.stackFormRows.Controls.Add(this.panelRowDriverKey);
            this.stackFormRows.Controls.Add(this.panelRowDisplayName);
            this.stackFormRows.Controls.Add(this.panelRowName);
            this.stackFormRows.Controls.Add(this.panelRowCardType);
            this.stackFormRows.Controls.Add(this.panelRowCardId);
            this.stackFormRows.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stackFormRows.Gap = 4;
            this.stackFormRows.Location = new System.Drawing.Point(4, 0);
            this.stackFormRows.Margin = new System.Windows.Forms.Padding(0);
            this.stackFormRows.Name = "stackFormRows";
            this.stackFormRows.Size = new System.Drawing.Size(576, 591);
            this.stackFormRows.TabIndex = 0;
            this.stackFormRows.Text = "stackFormRows";
            this.stackFormRows.Vertical = true;
            // 
            // panelRowRemark
            // 
            this.panelRowRemark.Controls.Add(this.inputRemark);
            this.panelRowRemark.Controls.Add(this.labelRemark);
            this.panelRowRemark.Location = new System.Drawing.Point(0, 770);
            this.panelRowRemark.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowRemark.Name = "panelRowRemark";
            this.panelRowRemark.Radius = 0;
            this.panelRowRemark.Size = new System.Drawing.Size(576, 76);
            this.panelRowRemark.TabIndex = 13;
            // 
            // inputRemark
            // 
            this.inputRemark.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputRemark.Location = new System.Drawing.Point(0, 26);
            this.inputRemark.Margin = new System.Windows.Forms.Padding(0);
            this.inputRemark.Multiline = true;
            this.inputRemark.Name = "inputRemark";
            this.inputRemark.PlaceholderText = "请输入备注";
            this.inputRemark.Size = new System.Drawing.Size(576, 50);
            this.inputRemark.TabIndex = 1;
            this.inputRemark.WaveSize = 0;
            // 
            // labelRemark
            // 
            this.labelRemark.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelRemark.Location = new System.Drawing.Point(0, 0);
            this.labelRemark.Margin = new System.Windows.Forms.Padding(0);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(576, 22);
            this.labelRemark.TabIndex = 0;
            this.labelRemark.Text = "备注";
            // 
            // panelRowDescription
            // 
            this.panelRowDescription.Controls.Add(this.inputDescription);
            this.panelRowDescription.Controls.Add(this.labelDescription);
            this.panelRowDescription.Location = new System.Drawing.Point(0, 690);
            this.panelRowDescription.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowDescription.Name = "panelRowDescription";
            this.panelRowDescription.Radius = 0;
            this.panelRowDescription.Size = new System.Drawing.Size(576, 76);
            this.panelRowDescription.TabIndex = 12;
            // 
            // inputDescription
            // 
            this.inputDescription.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputDescription.Location = new System.Drawing.Point(0, 26);
            this.inputDescription.Margin = new System.Windows.Forms.Padding(0);
            this.inputDescription.Multiline = true;
            this.inputDescription.Name = "inputDescription";
            this.inputDescription.PlaceholderText = "请输入描述";
            this.inputDescription.Size = new System.Drawing.Size(576, 50);
            this.inputDescription.TabIndex = 1;
            this.inputDescription.WaveSize = 0;
            // 
            // labelDescription
            // 
            this.labelDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDescription.Location = new System.Drawing.Point(0, 0);
            this.labelDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(576, 22);
            this.labelDescription.TabIndex = 0;
            this.labelDescription.Text = "描述";
            // 
            // panelRowOpenConfig
            // 
            this.panelRowOpenConfig.Controls.Add(this.inputOpenConfig);
            this.panelRowOpenConfig.Controls.Add(this.labelOpenConfig);
            this.panelRowOpenConfig.Location = new System.Drawing.Point(0, 610);
            this.panelRowOpenConfig.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowOpenConfig.Name = "panelRowOpenConfig";
            this.panelRowOpenConfig.Radius = 0;
            this.panelRowOpenConfig.Size = new System.Drawing.Size(576, 76);
            this.panelRowOpenConfig.TabIndex = 11;
            // 
            // inputOpenConfig
            // 
            this.inputOpenConfig.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputOpenConfig.Location = new System.Drawing.Point(0, 26);
            this.inputOpenConfig.Margin = new System.Windows.Forms.Padding(0);
            this.inputOpenConfig.Multiline = true;
            this.inputOpenConfig.Name = "inputOpenConfig";
            this.inputOpenConfig.PlaceholderText = "请输入初始化参数";
            this.inputOpenConfig.Size = new System.Drawing.Size(576, 50);
            this.inputOpenConfig.TabIndex = 1;
            this.inputOpenConfig.WaveSize = 0;
            // 
            // labelOpenConfig
            // 
            this.labelOpenConfig.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelOpenConfig.Location = new System.Drawing.Point(0, 0);
            this.labelOpenConfig.Margin = new System.Windows.Forms.Padding(0);
            this.labelOpenConfig.Name = "labelOpenConfig";
            this.labelOpenConfig.Size = new System.Drawing.Size(576, 22);
            this.labelOpenConfig.TabIndex = 0;
            this.labelOpenConfig.Text = "初始化参数";
            // 
            // panelRowFlags
            // 
            this.panelRowFlags.Controls.Add(this.flowFlags);
            this.panelRowFlags.Controls.Add(this.labelFlags);
            this.panelRowFlags.Location = new System.Drawing.Point(0, 558);
            this.panelRowFlags.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowFlags.Name = "panelRowFlags";
            this.panelRowFlags.Radius = 0;
            this.panelRowFlags.Size = new System.Drawing.Size(576, 48);
            this.panelRowFlags.TabIndex = 10;
            // 
            // flowFlags
            // 
            this.flowFlags.Controls.Add(this.checkIsEnabled);
            this.flowFlags.Controls.Add(this.checkUseExtModule);
            this.flowFlags.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowFlags.Gap = 12;
            this.flowFlags.Location = new System.Drawing.Point(0, 22);
            this.flowFlags.Margin = new System.Windows.Forms.Padding(0);
            this.flowFlags.Name = "flowFlags";
            this.flowFlags.Size = new System.Drawing.Size(576, 26);
            this.flowFlags.TabIndex = 1;
            this.flowFlags.Text = "flowFlags";
            // 
            // checkIsEnabled
            // 
            this.checkIsEnabled.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkIsEnabled.Checked = true;
            this.checkIsEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkIsEnabled.Location = new System.Drawing.Point(100, 0);
            this.checkIsEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.checkIsEnabled.Name = "checkIsEnabled";
            this.checkIsEnabled.Size = new System.Drawing.Size(61, 34);
            this.checkIsEnabled.TabIndex = 0;
            this.checkIsEnabled.Text = "启用";
            // 
            // checkUseExtModule
            // 
            this.checkUseExtModule.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkUseExtModule.Location = new System.Drawing.Point(0, 0);
            this.checkUseExtModule.Margin = new System.Windows.Forms.Padding(0);
            this.checkUseExtModule.Name = "checkUseExtModule";
            this.checkUseExtModule.Size = new System.Drawing.Size(88, 34);
            this.checkUseExtModule.TabIndex = 1;
            this.checkUseExtModule.Text = "扩展模块";
            // 
            // labelFlags
            // 
            this.labelFlags.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelFlags.Location = new System.Drawing.Point(0, 0);
            this.labelFlags.Margin = new System.Windows.Forms.Padding(0);
            this.labelFlags.Name = "labelFlags";
            this.labelFlags.Size = new System.Drawing.Size(576, 22);
            this.labelFlags.TabIndex = 0;
            this.labelFlags.Text = "状态配置";
            // 
            // panelRowSortOrder
            // 
            this.panelRowSortOrder.Controls.Add(this.inputSortOrder);
            this.panelRowSortOrder.Controls.Add(this.labelSortOrder);
            this.panelRowSortOrder.Location = new System.Drawing.Point(0, 502);
            this.panelRowSortOrder.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowSortOrder.Name = "panelRowSortOrder";
            this.panelRowSortOrder.Radius = 0;
            this.panelRowSortOrder.Size = new System.Drawing.Size(576, 52);
            this.panelRowSortOrder.TabIndex = 9;
            // 
            // inputSortOrder
            // 
            this.inputSortOrder.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputSortOrder.Location = new System.Drawing.Point(0, 20);
            this.inputSortOrder.Margin = new System.Windows.Forms.Padding(0);
            this.inputSortOrder.Name = "inputSortOrder";
            this.inputSortOrder.PlaceholderText = "请输入排序号";
            this.inputSortOrder.Size = new System.Drawing.Size(576, 32);
            this.inputSortOrder.TabIndex = 1;
            this.inputSortOrder.WaveSize = 0;
            // 
            // labelSortOrder
            // 
            this.labelSortOrder.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelSortOrder.Location = new System.Drawing.Point(0, 0);
            this.labelSortOrder.Margin = new System.Windows.Forms.Padding(0);
            this.labelSortOrder.Name = "labelSortOrder";
            this.labelSortOrder.Size = new System.Drawing.Size(576, 22);
            this.labelSortOrder.TabIndex = 0;
            this.labelSortOrder.Text = "排序号";
            // 
            // panelRowInitOrder
            // 
            this.panelRowInitOrder.Controls.Add(this.inputInitOrder);
            this.panelRowInitOrder.Controls.Add(this.labelInitOrder);
            this.panelRowInitOrder.Location = new System.Drawing.Point(0, 446);
            this.panelRowInitOrder.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowInitOrder.Name = "panelRowInitOrder";
            this.panelRowInitOrder.Radius = 0;
            this.panelRowInitOrder.Size = new System.Drawing.Size(576, 52);
            this.panelRowInitOrder.TabIndex = 8;
            // 
            // inputInitOrder
            // 
            this.inputInitOrder.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputInitOrder.Location = new System.Drawing.Point(0, 20);
            this.inputInitOrder.Margin = new System.Windows.Forms.Padding(0);
            this.inputInitOrder.Name = "inputInitOrder";
            this.inputInitOrder.PlaceholderText = "请输入初始化顺序";
            this.inputInitOrder.Size = new System.Drawing.Size(576, 32);
            this.inputInitOrder.TabIndex = 1;
            this.inputInitOrder.WaveSize = 0;
            // 
            // labelInitOrder
            // 
            this.labelInitOrder.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelInitOrder.Location = new System.Drawing.Point(0, 0);
            this.labelInitOrder.Margin = new System.Windows.Forms.Padding(0);
            this.labelInitOrder.Name = "labelInitOrder";
            this.labelInitOrder.Size = new System.Drawing.Size(576, 22);
            this.labelInitOrder.TabIndex = 0;
            this.labelInitOrder.Text = "初始化顺序";
            // 
            // panelRowModeParam
            // 
            this.panelRowModeParam.Controls.Add(this.inputModeParam);
            this.panelRowModeParam.Controls.Add(this.labelModeParam);
            this.panelRowModeParam.Location = new System.Drawing.Point(0, 390);
            this.panelRowModeParam.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowModeParam.Name = "panelRowModeParam";
            this.panelRowModeParam.Radius = 0;
            this.panelRowModeParam.Size = new System.Drawing.Size(576, 52);
            this.panelRowModeParam.TabIndex = 7;
            // 
            // inputModeParam
            // 
            this.inputModeParam.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputModeParam.Location = new System.Drawing.Point(0, 20);
            this.inputModeParam.Margin = new System.Windows.Forms.Padding(0);
            this.inputModeParam.Name = "inputModeParam";
            this.inputModeParam.PlaceholderText = "请输入模式参数";
            this.inputModeParam.Size = new System.Drawing.Size(576, 32);
            this.inputModeParam.TabIndex = 1;
            this.inputModeParam.WaveSize = 0;
            // 
            // labelModeParam
            // 
            this.labelModeParam.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelModeParam.Location = new System.Drawing.Point(0, 0);
            this.labelModeParam.Margin = new System.Windows.Forms.Padding(0);
            this.labelModeParam.Name = "labelModeParam";
            this.labelModeParam.Size = new System.Drawing.Size(576, 22);
            this.labelModeParam.TabIndex = 0;
            this.labelModeParam.Text = "模式参数";
            // 
            // panelRowAxisCount
            // 
            this.panelRowAxisCount.Controls.Add(this.inputAxisCount);
            this.panelRowAxisCount.Controls.Add(this.labelAxisCount);
            this.panelRowAxisCount.Location = new System.Drawing.Point(0, 334);
            this.panelRowAxisCount.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowAxisCount.Name = "panelRowAxisCount";
            this.panelRowAxisCount.Radius = 0;
            this.panelRowAxisCount.Size = new System.Drawing.Size(576, 52);
            this.panelRowAxisCount.TabIndex = 6;
            // 
            // inputAxisCount
            // 
            this.inputAxisCount.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputAxisCount.Location = new System.Drawing.Point(0, 20);
            this.inputAxisCount.Margin = new System.Windows.Forms.Padding(0);
            this.inputAxisCount.Name = "inputAxisCount";
            this.inputAxisCount.PlaceholderText = "请输入轴数";
            this.inputAxisCount.Size = new System.Drawing.Size(576, 32);
            this.inputAxisCount.TabIndex = 1;
            this.inputAxisCount.WaveSize = 0;
            // 
            // labelAxisCount
            // 
            this.labelAxisCount.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelAxisCount.Location = new System.Drawing.Point(0, 0);
            this.labelAxisCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelAxisCount.Name = "labelAxisCount";
            this.labelAxisCount.Size = new System.Drawing.Size(576, 22);
            this.labelAxisCount.TabIndex = 0;
            this.labelAxisCount.Text = "轴数";
            // 
            // panelRowCoreNumber
            // 
            this.panelRowCoreNumber.Controls.Add(this.inputCoreNumber);
            this.panelRowCoreNumber.Controls.Add(this.labelCoreNumber);
            this.panelRowCoreNumber.Location = new System.Drawing.Point(0, 278);
            this.panelRowCoreNumber.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowCoreNumber.Name = "panelRowCoreNumber";
            this.panelRowCoreNumber.Radius = 0;
            this.panelRowCoreNumber.Size = new System.Drawing.Size(576, 52);
            this.panelRowCoreNumber.TabIndex = 5;
            // 
            // inputCoreNumber
            // 
            this.inputCoreNumber.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputCoreNumber.Location = new System.Drawing.Point(0, 20);
            this.inputCoreNumber.Margin = new System.Windows.Forms.Padding(0);
            this.inputCoreNumber.Name = "inputCoreNumber";
            this.inputCoreNumber.PlaceholderText = "请输入核数";
            this.inputCoreNumber.Size = new System.Drawing.Size(576, 32);
            this.inputCoreNumber.TabIndex = 1;
            this.inputCoreNumber.WaveSize = 0;
            // 
            // labelCoreNumber
            // 
            this.labelCoreNumber.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelCoreNumber.Location = new System.Drawing.Point(0, 0);
            this.labelCoreNumber.Margin = new System.Windows.Forms.Padding(0);
            this.labelCoreNumber.Name = "labelCoreNumber";
            this.labelCoreNumber.Size = new System.Drawing.Size(576, 22);
            this.labelCoreNumber.TabIndex = 0;
            this.labelCoreNumber.Text = "核数";
            // 
            // panelRowDriverKey
            // 
            this.panelRowDriverKey.Controls.Add(this.inputDriverKey);
            this.panelRowDriverKey.Controls.Add(this.labelDriverKey);
            this.panelRowDriverKey.Location = new System.Drawing.Point(0, 222);
            this.panelRowDriverKey.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowDriverKey.Name = "panelRowDriverKey";
            this.panelRowDriverKey.Radius = 0;
            this.panelRowDriverKey.Size = new System.Drawing.Size(576, 52);
            this.panelRowDriverKey.TabIndex = 4;
            // 
            // inputDriverKey
            // 
            this.inputDriverKey.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputDriverKey.Location = new System.Drawing.Point(0, 20);
            this.inputDriverKey.Margin = new System.Windows.Forms.Padding(0);
            this.inputDriverKey.Name = "inputDriverKey";
            this.inputDriverKey.PlaceholderText = "请输入驱动识别键";
            this.inputDriverKey.Size = new System.Drawing.Size(576, 32);
            this.inputDriverKey.TabIndex = 1;
            this.inputDriverKey.WaveSize = 0;
            // 
            // labelDriverKey
            // 
            this.labelDriverKey.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDriverKey.Location = new System.Drawing.Point(0, 0);
            this.labelDriverKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelDriverKey.Name = "labelDriverKey";
            this.labelDriverKey.Size = new System.Drawing.Size(576, 22);
            this.labelDriverKey.TabIndex = 0;
            this.labelDriverKey.Text = "驱动识别键";
            // 
            // panelRowDisplayName
            // 
            this.panelRowDisplayName.Controls.Add(this.inputDisplayName);
            this.panelRowDisplayName.Controls.Add(this.labelDisplayName);
            this.panelRowDisplayName.Location = new System.Drawing.Point(0, 166);
            this.panelRowDisplayName.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowDisplayName.Name = "panelRowDisplayName";
            this.panelRowDisplayName.Radius = 0;
            this.panelRowDisplayName.Size = new System.Drawing.Size(576, 52);
            this.panelRowDisplayName.TabIndex = 3;
            // 
            // inputDisplayName
            // 
            this.inputDisplayName.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputDisplayName.Location = new System.Drawing.Point(0, 20);
            this.inputDisplayName.Margin = new System.Windows.Forms.Padding(0);
            this.inputDisplayName.Name = "inputDisplayName";
            this.inputDisplayName.PlaceholderText = "请输入显示名称";
            this.inputDisplayName.Size = new System.Drawing.Size(576, 32);
            this.inputDisplayName.TabIndex = 1;
            this.inputDisplayName.WaveSize = 0;
            // 
            // labelDisplayName
            // 
            this.labelDisplayName.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDisplayName.Location = new System.Drawing.Point(0, 0);
            this.labelDisplayName.Margin = new System.Windows.Forms.Padding(0);
            this.labelDisplayName.Name = "labelDisplayName";
            this.labelDisplayName.Size = new System.Drawing.Size(576, 22);
            this.labelDisplayName.TabIndex = 0;
            this.labelDisplayName.Text = "显示名称";
            // 
            // panelRowName
            // 
            this.panelRowName.Controls.Add(this.inputName);
            this.panelRowName.Controls.Add(this.labelName);
            this.panelRowName.Location = new System.Drawing.Point(0, 110);
            this.panelRowName.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowName.Name = "panelRowName";
            this.panelRowName.Radius = 0;
            this.panelRowName.Size = new System.Drawing.Size(576, 52);
            this.panelRowName.TabIndex = 2;
            // 
            // inputName
            // 
            this.inputName.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputName.Location = new System.Drawing.Point(0, 20);
            this.inputName.Margin = new System.Windows.Forms.Padding(0);
            this.inputName.Name = "inputName";
            this.inputName.PlaceholderText = "请输入内部名称";
            this.inputName.Size = new System.Drawing.Size(576, 32);
            this.inputName.TabIndex = 1;
            this.inputName.WaveSize = 0;
            // 
            // labelName
            // 
            this.labelName.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelName.Location = new System.Drawing.Point(0, 0);
            this.labelName.Margin = new System.Windows.Forms.Padding(0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(576, 22);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "内部名称";
            // 
            // panelRowCardType
            // 
            this.panelRowCardType.Controls.Add(this.dropdownCardType);
            this.panelRowCardType.Controls.Add(this.labelCardType);
            this.panelRowCardType.Location = new System.Drawing.Point(0, 54);
            this.panelRowCardType.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowCardType.Name = "panelRowCardType";
            this.panelRowCardType.Radius = 0;
            this.panelRowCardType.Size = new System.Drawing.Size(576, 52);
            this.panelRowCardType.TabIndex = 1;
            // 
            // dropdownCardType
            // 
            this.dropdownCardType.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dropdownCardType.Location = new System.Drawing.Point(0, 20);
            this.dropdownCardType.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownCardType.Name = "dropdownCardType";
            this.dropdownCardType.Size = new System.Drawing.Size(576, 32);
            this.dropdownCardType.TabIndex = 1;
            this.dropdownCardType.WaveSize = 0;
            // 
            // labelCardType
            // 
            this.labelCardType.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelCardType.Location = new System.Drawing.Point(0, 0);
            this.labelCardType.Margin = new System.Windows.Forms.Padding(0);
            this.labelCardType.Name = "labelCardType";
            this.labelCardType.Size = new System.Drawing.Size(576, 22);
            this.labelCardType.TabIndex = 0;
            this.labelCardType.Text = "控制卡类型";
            // 
            // panelRowCardId
            // 
            this.panelRowCardId.Controls.Add(this.inputCardId);
            this.panelRowCardId.Controls.Add(this.labelCardId);
            this.panelRowCardId.Location = new System.Drawing.Point(0, 0);
            this.panelRowCardId.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowCardId.Name = "panelRowCardId";
            this.panelRowCardId.Radius = 0;
            this.panelRowCardId.Size = new System.Drawing.Size(576, 50);
            this.panelRowCardId.TabIndex = 0;
            // 
            // inputCardId
            // 
            this.inputCardId.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputCardId.Location = new System.Drawing.Point(0, 18);
            this.inputCardId.Margin = new System.Windows.Forms.Padding(0);
            this.inputCardId.Name = "inputCardId";
            this.inputCardId.PlaceholderText = "请输入控制卡卡号";
            this.inputCardId.Size = new System.Drawing.Size(576, 32);
            this.inputCardId.TabIndex = 1;
            this.inputCardId.WaveSize = 0;
            // 
            // labelCardId
            // 
            this.labelCardId.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelCardId.Location = new System.Drawing.Point(0, 0);
            this.labelCardId.Margin = new System.Windows.Forms.Padding(0);
            this.labelCardId.Name = "labelCardId";
            this.labelCardId.Size = new System.Drawing.Size(576, 22);
            this.labelCardId.TabIndex = 0;
            this.labelCardId.Text = "卡号";
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.flowFooterButtons);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(28, 675);
            this.panelFooter.Margin = new System.Windows.Forms.Padding(0);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Padding = new System.Windows.Forms.Padding(4, 10, 4, 0);
            this.panelFooter.Radius = 0;
            this.panelFooter.Size = new System.Drawing.Size(584, 57);
            this.panelFooter.TabIndex = 2;
            // 
            // flowFooterButtons
            // 
            this.flowFooterButtons.Controls.Add(this.buttonOk);
            this.flowFooterButtons.Controls.Add(this.buttonCancel);
            this.flowFooterButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowFooterButtons.Gap = 10;
            this.flowFooterButtons.Location = new System.Drawing.Point(336, 10);
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
            this.panelHeader.Size = new System.Drawing.Size(584, 56);
            this.panelHeader.TabIndex = 0;
            // 
            // flowHeaderRight
            // 
            this.flowHeaderRight.Controls.Add(this.labelDialogDescription);
            this.flowHeaderRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowHeaderRight.Location = new System.Drawing.Point(267, 0);
            this.flowHeaderRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowHeaderRight.Name = "flowHeaderRight";
            this.flowHeaderRight.Size = new System.Drawing.Size(313, 48);
            this.flowHeaderRight.TabIndex = 1;
            this.flowHeaderRight.Text = "flowHeaderRight";
            // 
            // labelDialogDescription
            // 
            this.labelDialogDescription.Location = new System.Drawing.Point(0, 0);
            this.labelDialogDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDialogDescription.Name = "labelDialogDescription";
            this.labelDialogDescription.Size = new System.Drawing.Size(311, 48);
            this.labelDialogDescription.TabIndex = 0;
            this.labelDialogDescription.Text = "填写控制卡基础信息、初始化参数和说明信息。";
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
            // MotionCardEditDialog
            // 
            this.ClientSize = new System.Drawing.Size(640, 760);
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
            this.stackFormRows.ResumeLayout(false);
            this.panelRowRemark.ResumeLayout(false);
            this.panelRowDescription.ResumeLayout(false);
            this.panelRowOpenConfig.ResumeLayout(false);
            this.panelRowFlags.ResumeLayout(false);
            this.flowFlags.ResumeLayout(false);
            this.flowFlags.PerformLayout();
            this.panelRowSortOrder.ResumeLayout(false);
            this.panelRowInitOrder.ResumeLayout(false);
            this.panelRowModeParam.ResumeLayout(false);
            this.panelRowAxisCount.ResumeLayout(false);
            this.panelRowCoreNumber.ResumeLayout(false);
            this.panelRowDriverKey.ResumeLayout(false);
            this.panelRowDisplayName.ResumeLayout(false);
            this.panelRowName.ResumeLayout(false);
            this.panelRowCardType.ResumeLayout(false);
            this.panelRowCardId.ResumeLayout(false);
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
        private AntdUI.StackPanel stackFormRows;
        private AntdUI.Panel panelRowCardId;
        private AntdUI.Input inputCardId;
        private AntdUI.Label labelCardId;
        private AntdUI.Panel panelRowCardType;
        private AntdUI.Select dropdownCardType;
        private AntdUI.Label labelCardType;
        private AntdUI.Panel panelRowName;
        private AntdUI.Input inputName;
        private AntdUI.Label labelName;
        private AntdUI.Panel panelRowDisplayName;
        private AntdUI.Input inputDisplayName;
        private AntdUI.Label labelDisplayName;
        private AntdUI.Panel panelRowDriverKey;
        private AntdUI.Input inputDriverKey;
        private AntdUI.Label labelDriverKey;
        private AntdUI.Panel panelRowCoreNumber;
        private AntdUI.Input inputCoreNumber;
        private AntdUI.Label labelCoreNumber;
        private AntdUI.Panel panelRowAxisCount;
        private AntdUI.Input inputAxisCount;
        private AntdUI.Label labelAxisCount;
        private AntdUI.Panel panelRowModeParam;
        private AntdUI.Input inputModeParam;
        private AntdUI.Label labelModeParam;
        private AntdUI.Panel panelRowInitOrder;
        private AntdUI.Input inputInitOrder;
        private AntdUI.Label labelInitOrder;
        private AntdUI.Panel panelRowSortOrder;
        private AntdUI.Input inputSortOrder;
        private AntdUI.Label labelSortOrder;
        private AntdUI.Panel panelRowFlags;
        private AntdUI.FlowPanel flowFlags;
        private AntdUI.Checkbox checkIsEnabled;
        private AntdUI.Checkbox checkUseExtModule;
        private AntdUI.Label labelFlags;
        private AntdUI.Panel panelRowOpenConfig;
        private AntdUI.Input inputOpenConfig;
        private AntdUI.Label labelOpenConfig;
        private AntdUI.Panel panelRowDescription;
        private AntdUI.Input inputDescription;
        private AntdUI.Label labelDescription;
        private AntdUI.Panel panelRowRemark;
        private AntdUI.Input inputRemark;
        private AntdUI.Label labelRemark;
        private AntdUI.Panel panelFooter;
        private AntdUI.FlowPanel flowFooterButtons;
        private AntdUI.Button buttonCancel;
        private AntdUI.Button buttonOk;
    }
}