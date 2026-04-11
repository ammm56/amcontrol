namespace AMControlWinF.Views.SysConfig
{
    partial class PlcStationEditDialog
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
            this.inputSortOrder = new AntdUI.Input();
            this.labelSortOrder = new AntdUI.Label();
            this.checkEnabled = new AntdUI.Checkbox();
            this.labelEnabled = new AntdUI.Label();
            this.labelSectionRemark = new AntdUI.Label();
            this.stackSectionRuntime = new AntdUI.StackPanel();
            this.inputScanIntervalMs = new AntdUI.Input();
            this.labelScanIntervalMs = new AntdUI.Label();
            this.inputReconnectIntervalMs = new AntdUI.Input();
            this.labelReconnectIntervalMs = new AntdUI.Label();
            this.inputTimeoutMs = new AntdUI.Input();
            this.labelTimeoutMs = new AntdUI.Label();
            this.inputSlot = new AntdUI.Input();
            this.labelSlot = new AntdUI.Label();
            this.inputRack = new AntdUI.Input();
            this.labelRack = new AntdUI.Label();
            this.labelSectionRuntime = new AntdUI.Label();
            this.stackSectionAddress = new AntdUI.StackPanel();
            this.inputPcNo = new AntdUI.Input();
            this.labelPcNo = new AntdUI.Label();
            this.inputNetworkNo = new AntdUI.Input();
            this.labelNetworkNo = new AntdUI.Label();
            this.inputStationNo = new AntdUI.Input();
            this.labelStationNo = new AntdUI.Label();
            this.panelRowStopBits = new AntdUI.Panel();
            this.dropdownStopBits = new AntdUI.Select();
            this.labelStopBits = new AntdUI.Label();
            this.panelRowParity = new AntdUI.Panel();
            this.dropdownParity = new AntdUI.Select();
            this.labelParity = new AntdUI.Label();
            this.panelRowDataBits = new AntdUI.Panel();
            this.inputDataBits = new AntdUI.Input();
            this.labelDataBits = new AntdUI.Label();
            this.panelRowBaudRate = new AntdUI.Panel();
            this.inputBaudRate = new AntdUI.Input();
            this.labelBaudRate = new AntdUI.Label();
            this.panelRowComPort = new AntdUI.Panel();
            this.inputComPort = new AntdUI.Input();
            this.labelComPort = new AntdUI.Label();
            this.panelRowPort = new AntdUI.Panel();
            this.inputPort = new AntdUI.Input();
            this.labelPort = new AntdUI.Label();
            this.panelRowIpAddress = new AntdUI.Panel();
            this.inputIpAddress = new AntdUI.Input();
            this.labelIpAddress = new AntdUI.Label();
            this.labelSectionAddress = new AntdUI.Label();
            this.stackSectionBasic = new AntdUI.StackPanel();
            this.panelRowProtocolType = new AntdUI.Panel();
            this.dropdownProtocolType = new AntdUI.Select();
            this.labelProtocolType = new AntdUI.Label();
            this.panelRowConnectionType = new AntdUI.Panel();
            this.dropdownConnectionType = new AntdUI.Select();
            this.labelConnectionType = new AntdUI.Label();
            this.inputModel = new AntdUI.Input();
            this.labelModel = new AntdUI.Label();
            this.inputVendor = new AntdUI.Input();
            this.labelVendor = new AntdUI.Label();
            this.inputDisplayName = new AntdUI.Input();
            this.labelDisplayName = new AntdUI.Label();
            this.inputName = new AntdUI.Input();
            this.labelName = new AntdUI.Label();
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
            this.textureBackgroundDialog.SuspendLayout();
            this.panelShell.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.gridMainSections.SuspendLayout();
            this.stackSectionRemark.SuspendLayout();
            this.stackSectionRuntime.SuspendLayout();
            this.stackSectionAddress.SuspendLayout();
            this.panelRowStopBits.SuspendLayout();
            this.panelRowParity.SuspendLayout();
            this.panelRowDataBits.SuspendLayout();
            this.panelRowBaudRate.SuspendLayout();
            this.panelRowComPort.SuspendLayout();
            this.panelRowPort.SuspendLayout();
            this.panelRowIpAddress.SuspendLayout();
            this.stackSectionBasic.SuspendLayout();
            this.panelRowProtocolType.SuspendLayout();
            this.panelRowConnectionType.SuspendLayout();
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
            this.textureBackgroundDialog.Size = new System.Drawing.Size(1080, 660);
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
            this.panelShell.Size = new System.Drawing.Size(1080, 660);
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
            this.panelContent.Size = new System.Drawing.Size(1024, 463);
            this.panelContent.TabIndex = 1;
            // 
            // gridMainSections
            // 
            this.gridMainSections.Controls.Add(this.stackSectionRemark);
            this.gridMainSections.Controls.Add(this.stackSectionRuntime);
            this.gridMainSections.Controls.Add(this.stackSectionAddress);
            this.gridMainSections.Controls.Add(this.stackSectionBasic);
            this.gridMainSections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridMainSections.Location = new System.Drawing.Point(4, 0);
            this.gridMainSections.Margin = new System.Windows.Forms.Padding(0);
            this.gridMainSections.Name = "gridMainSections";
            this.gridMainSections.Size = new System.Drawing.Size(1016, 463);
            this.gridMainSections.Span = "25% 25% 25% 25%";
            this.gridMainSections.TabIndex = 0;
            this.gridMainSections.Text = "gridMainSections";
            // 
            // stackSectionRemark
            // 
            this.stackSectionRemark.Controls.Add(this.inputRemark);
            this.stackSectionRemark.Controls.Add(this.labelRemark);
            this.stackSectionRemark.Controls.Add(this.inputDescription);
            this.stackSectionRemark.Controls.Add(this.labelDescription);
            this.stackSectionRemark.Controls.Add(this.inputSortOrder);
            this.stackSectionRemark.Controls.Add(this.labelSortOrder);
            this.stackSectionRemark.Controls.Add(this.checkEnabled);
            this.stackSectionRemark.Controls.Add(this.labelEnabled);
            this.stackSectionRemark.Controls.Add(this.labelSectionRemark);
            this.stackSectionRemark.Gap = 4;
            this.stackSectionRemark.Location = new System.Drawing.Point(762, 0);
            this.stackSectionRemark.Margin = new System.Windows.Forms.Padding(0);
            this.stackSectionRemark.Name = "stackSectionRemark";
            this.stackSectionRemark.Padding = new System.Windows.Forms.Padding(4);
            this.stackSectionRemark.Size = new System.Drawing.Size(254, 463);
            this.stackSectionRemark.TabIndex = 3;
            this.stackSectionRemark.Text = "stackSectionRemark";
            this.stackSectionRemark.Vertical = true;
            // 
            // inputRemark
            // 
            this.inputRemark.Location = new System.Drawing.Point(4, 336);
            this.inputRemark.Margin = new System.Windows.Forms.Padding(0);
            this.inputRemark.Multiline = true;
            this.inputRemark.Name = "inputRemark";
            this.inputRemark.PlaceholderText = "请输入备注";
            this.inputRemark.Size = new System.Drawing.Size(246, 90);
            this.inputRemark.TabIndex = 8;
            this.inputRemark.WaveSize = 0;
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(4, 310);
            this.labelRemark.Margin = new System.Windows.Forms.Padding(0);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(246, 22);
            this.labelRemark.TabIndex = 7;
            this.labelRemark.Text = "备注";
            // 
            // inputDescription
            // 
            this.inputDescription.Location = new System.Drawing.Point(4, 216);
            this.inputDescription.Margin = new System.Windows.Forms.Padding(0);
            this.inputDescription.Multiline = true;
            this.inputDescription.Name = "inputDescription";
            this.inputDescription.PlaceholderText = "请输入描述";
            this.inputDescription.Size = new System.Drawing.Size(246, 90);
            this.inputDescription.TabIndex = 6;
            this.inputDescription.WaveSize = 0;
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(4, 190);
            this.labelDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(246, 22);
            this.labelDescription.TabIndex = 5;
            this.labelDescription.Text = "描述";
            // 
            // inputSortOrder
            // 
            this.inputSortOrder.Location = new System.Drawing.Point(4, 154);
            this.inputSortOrder.Margin = new System.Windows.Forms.Padding(0);
            this.inputSortOrder.Name = "inputSortOrder";
            this.inputSortOrder.PlaceholderText = "请输入排序号";
            this.inputSortOrder.Size = new System.Drawing.Size(246, 32);
            this.inputSortOrder.TabIndex = 4;
            this.inputSortOrder.WaveSize = 0;
            // 
            // labelSortOrder
            // 
            this.labelSortOrder.Location = new System.Drawing.Point(4, 128);
            this.labelSortOrder.Margin = new System.Windows.Forms.Padding(0);
            this.labelSortOrder.Name = "labelSortOrder";
            this.labelSortOrder.Size = new System.Drawing.Size(246, 22);
            this.labelSortOrder.TabIndex = 3;
            this.labelSortOrder.Text = "排序号";
            // 
            // checkEnabled
            // 
            this.checkEnabled.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkEnabled.Checked = true;
            this.checkEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkEnabled.Location = new System.Drawing.Point(4, 90);
            this.checkEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.checkEnabled.Name = "checkEnabled";
            this.checkEnabled.Size = new System.Drawing.Size(77, 34);
            this.checkEnabled.TabIndex = 2;
            this.checkEnabled.Text = "启用站";
            // 
            // labelEnabled
            // 
            this.labelEnabled.Location = new System.Drawing.Point(4, 64);
            this.labelEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.labelEnabled.Name = "labelEnabled";
            this.labelEnabled.Size = new System.Drawing.Size(246, 22);
            this.labelEnabled.TabIndex = 1;
            this.labelEnabled.Text = "启用状态";
            // 
            // labelSectionRemark
            // 
            this.labelSectionRemark.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionRemark.Location = new System.Drawing.Point(4, 4);
            this.labelSectionRemark.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionRemark.Name = "labelSectionRemark";
            this.labelSectionRemark.Size = new System.Drawing.Size(246, 26);
            this.labelSectionRemark.TabIndex = 0;
            this.labelSectionRemark.Text = "运行与说明";
            // 
            // stackSectionRuntime
            // 
            this.stackSectionRuntime.Controls.Add(this.inputScanIntervalMs);
            this.stackSectionRuntime.Controls.Add(this.labelScanIntervalMs);
            this.stackSectionRuntime.Controls.Add(this.inputReconnectIntervalMs);
            this.stackSectionRuntime.Controls.Add(this.labelReconnectIntervalMs);
            this.stackSectionRuntime.Controls.Add(this.inputTimeoutMs);
            this.stackSectionRuntime.Controls.Add(this.labelTimeoutMs);
            this.stackSectionRuntime.Controls.Add(this.inputSlot);
            this.stackSectionRuntime.Controls.Add(this.labelSlot);
            this.stackSectionRuntime.Controls.Add(this.inputRack);
            this.stackSectionRuntime.Controls.Add(this.labelRack);
            this.stackSectionRuntime.Controls.Add(this.labelSectionRuntime);
            this.stackSectionRuntime.Gap = 4;
            this.stackSectionRuntime.Location = new System.Drawing.Point(508, 0);
            this.stackSectionRuntime.Margin = new System.Windows.Forms.Padding(0);
            this.stackSectionRuntime.Name = "stackSectionRuntime";
            this.stackSectionRuntime.Padding = new System.Windows.Forms.Padding(4);
            this.stackSectionRuntime.Size = new System.Drawing.Size(254, 463);
            this.stackSectionRuntime.TabIndex = 2;
            this.stackSectionRuntime.Text = "stackSectionRuntime";
            this.stackSectionRuntime.Vertical = true;
            // 
            // inputScanIntervalMs
            // 
            this.inputScanIntervalMs.Location = new System.Drawing.Point(4, 278);
            this.inputScanIntervalMs.Margin = new System.Windows.Forms.Padding(0);
            this.inputScanIntervalMs.Name = "inputScanIntervalMs";
            this.inputScanIntervalMs.PlaceholderText = "请输入扫描周期";
            this.inputScanIntervalMs.Size = new System.Drawing.Size(246, 32);
            this.inputScanIntervalMs.TabIndex = 10;
            this.inputScanIntervalMs.WaveSize = 0;
            // 
            // labelScanIntervalMs
            // 
            this.labelScanIntervalMs.Location = new System.Drawing.Point(4, 252);
            this.labelScanIntervalMs.Margin = new System.Windows.Forms.Padding(0);
            this.labelScanIntervalMs.Name = "labelScanIntervalMs";
            this.labelScanIntervalMs.Size = new System.Drawing.Size(246, 22);
            this.labelScanIntervalMs.TabIndex = 9;
            this.labelScanIntervalMs.Text = "扫描周期(ms)";
            // 
            // inputReconnectIntervalMs
            // 
            this.inputReconnectIntervalMs.Location = new System.Drawing.Point(4, 216);
            this.inputReconnectIntervalMs.Margin = new System.Windows.Forms.Padding(0);
            this.inputReconnectIntervalMs.Name = "inputReconnectIntervalMs";
            this.inputReconnectIntervalMs.PlaceholderText = "请输入重连周期";
            this.inputReconnectIntervalMs.Size = new System.Drawing.Size(246, 32);
            this.inputReconnectIntervalMs.TabIndex = 8;
            this.inputReconnectIntervalMs.WaveSize = 0;
            // 
            // labelReconnectIntervalMs
            // 
            this.labelReconnectIntervalMs.Location = new System.Drawing.Point(4, 190);
            this.labelReconnectIntervalMs.Margin = new System.Windows.Forms.Padding(0);
            this.labelReconnectIntervalMs.Name = "labelReconnectIntervalMs";
            this.labelReconnectIntervalMs.Size = new System.Drawing.Size(246, 22);
            this.labelReconnectIntervalMs.TabIndex = 7;
            this.labelReconnectIntervalMs.Text = "重连周期(ms)";
            // 
            // inputTimeoutMs
            // 
            this.inputTimeoutMs.Location = new System.Drawing.Point(4, 154);
            this.inputTimeoutMs.Margin = new System.Windows.Forms.Padding(0);
            this.inputTimeoutMs.Name = "inputTimeoutMs";
            this.inputTimeoutMs.PlaceholderText = "请输入通讯超时";
            this.inputTimeoutMs.Size = new System.Drawing.Size(246, 32);
            this.inputTimeoutMs.TabIndex = 6;
            this.inputTimeoutMs.WaveSize = 0;
            // 
            // labelTimeoutMs
            // 
            this.labelTimeoutMs.Location = new System.Drawing.Point(4, 128);
            this.labelTimeoutMs.Margin = new System.Windows.Forms.Padding(0);
            this.labelTimeoutMs.Name = "labelTimeoutMs";
            this.labelTimeoutMs.Size = new System.Drawing.Size(246, 22);
            this.labelTimeoutMs.TabIndex = 5;
            this.labelTimeoutMs.Text = "通讯超时(ms)";
            // 
            // inputSlot
            // 
            this.inputSlot.Location = new System.Drawing.Point(4, 92);
            this.inputSlot.Margin = new System.Windows.Forms.Padding(0);
            this.inputSlot.Name = "inputSlot";
            this.inputSlot.PlaceholderText = "请输入插槽号";
            this.inputSlot.Size = new System.Drawing.Size(246, 32);
            this.inputSlot.TabIndex = 4;
            this.inputSlot.WaveSize = 0;
            // 
            // labelSlot
            // 
            this.labelSlot.Location = new System.Drawing.Point(4, 66);
            this.labelSlot.Margin = new System.Windows.Forms.Padding(0);
            this.labelSlot.Name = "labelSlot";
            this.labelSlot.Size = new System.Drawing.Size(246, 22);
            this.labelSlot.TabIndex = 3;
            this.labelSlot.Text = "插槽号";
            // 
            // inputRack
            // 
            this.inputRack.Location = new System.Drawing.Point(4, 34);
            this.inputRack.Margin = new System.Windows.Forms.Padding(0);
            this.inputRack.Name = "inputRack";
            this.inputRack.PlaceholderText = "请输入机架号";
            this.inputRack.Size = new System.Drawing.Size(246, 32);
            this.inputRack.TabIndex = 2;
            this.inputRack.WaveSize = 0;
            // 
            // labelRack
            // 
            this.labelRack.Location = new System.Drawing.Point(4, 4);
            this.labelRack.Margin = new System.Windows.Forms.Padding(0);
            this.labelRack.Name = "labelRack";
            this.labelRack.Size = new System.Drawing.Size(246, 22);
            this.labelRack.TabIndex = 1;
            this.labelRack.Text = "机架号";
            // 
            // labelSectionRuntime
            // 
            this.labelSectionRuntime.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionRuntime.Location = new System.Drawing.Point(4, 4);
            this.labelSectionRuntime.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionRuntime.Name = "labelSectionRuntime";
            this.labelSectionRuntime.Size = new System.Drawing.Size(246, 26);
            this.labelSectionRuntime.TabIndex = 0;
            this.labelSectionRuntime.Text = "运行参数";
            // 
            // stackSectionAddress
            // 
            this.stackSectionAddress.Controls.Add(this.inputPcNo);
            this.stackSectionAddress.Controls.Add(this.labelPcNo);
            this.stackSectionAddress.Controls.Add(this.inputNetworkNo);
            this.stackSectionAddress.Controls.Add(this.labelNetworkNo);
            this.stackSectionAddress.Controls.Add(this.inputStationNo);
            this.stackSectionAddress.Controls.Add(this.labelStationNo);
            this.stackSectionAddress.Controls.Add(this.panelRowStopBits);
            this.stackSectionAddress.Controls.Add(this.panelRowParity);
            this.stackSectionAddress.Controls.Add(this.panelRowDataBits);
            this.stackSectionAddress.Controls.Add(this.panelRowBaudRate);
            this.stackSectionAddress.Controls.Add(this.panelRowComPort);
            this.stackSectionAddress.Controls.Add(this.panelRowPort);
            this.stackSectionAddress.Controls.Add(this.panelRowIpAddress);
            this.stackSectionAddress.Controls.Add(this.labelSectionAddress);
            this.stackSectionAddress.Gap = 4;
            this.stackSectionAddress.Location = new System.Drawing.Point(254, 0);
            this.stackSectionAddress.Margin = new System.Windows.Forms.Padding(0);
            this.stackSectionAddress.Name = "stackSectionAddress";
            this.stackSectionAddress.Padding = new System.Windows.Forms.Padding(4);
            this.stackSectionAddress.Size = new System.Drawing.Size(254, 463);
            this.stackSectionAddress.TabIndex = 1;
            this.stackSectionAddress.Text = "stackSectionAddress";
            this.stackSectionAddress.Vertical = true;
            // 
            // inputPcNo
            // 
            this.inputPcNo.Location = new System.Drawing.Point(4, 420);
            this.inputPcNo.Margin = new System.Windows.Forms.Padding(0);
            this.inputPcNo.Name = "inputPcNo";
            this.inputPcNo.PlaceholderText = "请输入PC号";
            this.inputPcNo.Size = new System.Drawing.Size(246, 32);
            this.inputPcNo.TabIndex = 13;
            this.inputPcNo.WaveSize = 0;
            // 
            // labelPcNo
            // 
            this.labelPcNo.Location = new System.Drawing.Point(4, 394);
            this.labelPcNo.Margin = new System.Windows.Forms.Padding(0);
            this.labelPcNo.Name = "labelPcNo";
            this.labelPcNo.Size = new System.Drawing.Size(246, 22);
            this.labelPcNo.TabIndex = 12;
            this.labelPcNo.Text = "PC号";
            // 
            // inputNetworkNo
            // 
            this.inputNetworkNo.Location = new System.Drawing.Point(4, 358);
            this.inputNetworkNo.Margin = new System.Windows.Forms.Padding(0);
            this.inputNetworkNo.Name = "inputNetworkNo";
            this.inputNetworkNo.PlaceholderText = "请输入网络号";
            this.inputNetworkNo.Size = new System.Drawing.Size(246, 32);
            this.inputNetworkNo.TabIndex = 11;
            this.inputNetworkNo.WaveSize = 0;
            // 
            // labelNetworkNo
            // 
            this.labelNetworkNo.Location = new System.Drawing.Point(4, 332);
            this.labelNetworkNo.Margin = new System.Windows.Forms.Padding(0);
            this.labelNetworkNo.Name = "labelNetworkNo";
            this.labelNetworkNo.Size = new System.Drawing.Size(246, 22);
            this.labelNetworkNo.TabIndex = 10;
            this.labelNetworkNo.Text = "网络号";
            // 
            // inputStationNo
            // 
            this.inputStationNo.Location = new System.Drawing.Point(4, 296);
            this.inputStationNo.Margin = new System.Windows.Forms.Padding(0);
            this.inputStationNo.Name = "inputStationNo";
            this.inputStationNo.PlaceholderText = "请输入站号";
            this.inputStationNo.Size = new System.Drawing.Size(246, 32);
            this.inputStationNo.TabIndex = 9;
            this.inputStationNo.WaveSize = 0;
            // 
            // labelStationNo
            // 
            this.labelStationNo.Location = new System.Drawing.Point(4, 270);
            this.labelStationNo.Margin = new System.Windows.Forms.Padding(0);
            this.labelStationNo.Name = "labelStationNo";
            this.labelStationNo.Size = new System.Drawing.Size(246, 22);
            this.labelStationNo.TabIndex = 8;
            this.labelStationNo.Text = "站号";
            // 
            // panelRowStopBits
            // 
            this.panelRowStopBits.Controls.Add(this.dropdownStopBits);
            this.panelRowStopBits.Controls.Add(this.labelStopBits);
            this.panelRowStopBits.Location = new System.Drawing.Point(4, 214);
            this.panelRowStopBits.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowStopBits.Name = "panelRowStopBits";
            this.panelRowStopBits.Radius = 0;
            this.panelRowStopBits.Size = new System.Drawing.Size(246, 52);
            this.panelRowStopBits.TabIndex = 7;
            // 
            // dropdownStopBits
            // 
            this.dropdownStopBits.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dropdownStopBits.Location = new System.Drawing.Point(0, 20);
            this.dropdownStopBits.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownStopBits.Name = "dropdownStopBits";
            this.dropdownStopBits.Size = new System.Drawing.Size(246, 32);
            this.dropdownStopBits.TabIndex = 1;
            this.dropdownStopBits.WaveSize = 0;
            // 
            // labelStopBits
            // 
            this.labelStopBits.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelStopBits.Location = new System.Drawing.Point(0, 0);
            this.labelStopBits.Margin = new System.Windows.Forms.Padding(0);
            this.labelStopBits.Name = "labelStopBits";
            this.labelStopBits.Size = new System.Drawing.Size(246, 22);
            this.labelStopBits.TabIndex = 0;
            this.labelStopBits.Text = "停止位";
            // 
            // panelRowParity
            // 
            this.panelRowParity.Controls.Add(this.dropdownParity);
            this.panelRowParity.Controls.Add(this.labelParity);
            this.panelRowParity.Location = new System.Drawing.Point(4, 158);
            this.panelRowParity.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowParity.Name = "panelRowParity";
            this.panelRowParity.Radius = 0;
            this.panelRowParity.Size = new System.Drawing.Size(246, 52);
            this.panelRowParity.TabIndex = 6;
            // 
            // dropdownParity
            // 
            this.dropdownParity.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dropdownParity.Location = new System.Drawing.Point(0, 20);
            this.dropdownParity.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownParity.Name = "dropdownParity";
            this.dropdownParity.Size = new System.Drawing.Size(246, 32);
            this.dropdownParity.TabIndex = 1;
            this.dropdownParity.WaveSize = 0;
            // 
            // labelParity
            // 
            this.labelParity.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelParity.Location = new System.Drawing.Point(0, 0);
            this.labelParity.Margin = new System.Windows.Forms.Padding(0);
            this.labelParity.Name = "labelParity";
            this.labelParity.Size = new System.Drawing.Size(246, 22);
            this.labelParity.TabIndex = 0;
            this.labelParity.Text = "校验位";
            // 
            // panelRowDataBits
            // 
            this.panelRowDataBits.Controls.Add(this.inputDataBits);
            this.panelRowDataBits.Controls.Add(this.labelDataBits);
            this.panelRowDataBits.Location = new System.Drawing.Point(4, 102);
            this.panelRowDataBits.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowDataBits.Name = "panelRowDataBits";
            this.panelRowDataBits.Radius = 0;
            this.panelRowDataBits.Size = new System.Drawing.Size(246, 52);
            this.panelRowDataBits.TabIndex = 5;
            // 
            // inputDataBits
            // 
            this.inputDataBits.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputDataBits.Location = new System.Drawing.Point(0, 20);
            this.inputDataBits.Margin = new System.Windows.Forms.Padding(0);
            this.inputDataBits.Name = "inputDataBits";
            this.inputDataBits.PlaceholderText = "请输入数据位";
            this.inputDataBits.Size = new System.Drawing.Size(246, 32);
            this.inputDataBits.TabIndex = 1;
            this.inputDataBits.WaveSize = 0;
            // 
            // labelDataBits
            // 
            this.labelDataBits.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDataBits.Location = new System.Drawing.Point(0, 0);
            this.labelDataBits.Margin = new System.Windows.Forms.Padding(0);
            this.labelDataBits.Name = "labelDataBits";
            this.labelDataBits.Size = new System.Drawing.Size(246, 22);
            this.labelDataBits.TabIndex = 0;
            this.labelDataBits.Text = "数据位";
            // 
            // panelRowBaudRate
            // 
            this.panelRowBaudRate.Controls.Add(this.inputBaudRate);
            this.panelRowBaudRate.Controls.Add(this.labelBaudRate);
            this.panelRowBaudRate.Location = new System.Drawing.Point(4, 46);
            this.panelRowBaudRate.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowBaudRate.Name = "panelRowBaudRate";
            this.panelRowBaudRate.Radius = 0;
            this.panelRowBaudRate.Size = new System.Drawing.Size(246, 52);
            this.panelRowBaudRate.TabIndex = 4;
            // 
            // inputBaudRate
            // 
            this.inputBaudRate.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputBaudRate.Location = new System.Drawing.Point(0, 20);
            this.inputBaudRate.Margin = new System.Windows.Forms.Padding(0);
            this.inputBaudRate.Name = "inputBaudRate";
            this.inputBaudRate.PlaceholderText = "请输入波特率";
            this.inputBaudRate.Size = new System.Drawing.Size(246, 32);
            this.inputBaudRate.TabIndex = 1;
            this.inputBaudRate.WaveSize = 0;
            // 
            // labelBaudRate
            // 
            this.labelBaudRate.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelBaudRate.Location = new System.Drawing.Point(0, 0);
            this.labelBaudRate.Margin = new System.Windows.Forms.Padding(0);
            this.labelBaudRate.Name = "labelBaudRate";
            this.labelBaudRate.Size = new System.Drawing.Size(246, 22);
            this.labelBaudRate.TabIndex = 0;
            this.labelBaudRate.Text = "波特率";
            // 
            // panelRowComPort
            // 
            this.panelRowComPort.Controls.Add(this.inputComPort);
            this.panelRowComPort.Controls.Add(this.labelComPort);
            this.panelRowComPort.Location = new System.Drawing.Point(4, 270);
            this.panelRowComPort.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowComPort.Name = "panelRowComPort";
            this.panelRowComPort.Radius = 0;
            this.panelRowComPort.Size = new System.Drawing.Size(246, 52);
            this.panelRowComPort.TabIndex = 3;
            // 
            // inputComPort
            // 
            this.inputComPort.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputComPort.Location = new System.Drawing.Point(0, 20);
            this.inputComPort.Margin = new System.Windows.Forms.Padding(0);
            this.inputComPort.Name = "inputComPort";
            this.inputComPort.PlaceholderText = "请输入串口号";
            this.inputComPort.Size = new System.Drawing.Size(246, 32);
            this.inputComPort.TabIndex = 1;
            this.inputComPort.WaveSize = 0;
            // 
            // labelComPort
            // 
            this.labelComPort.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelComPort.Location = new System.Drawing.Point(0, 0);
            this.labelComPort.Margin = new System.Windows.Forms.Padding(0);
            this.labelComPort.Name = "labelComPort";
            this.labelComPort.Size = new System.Drawing.Size(246, 22);
            this.labelComPort.TabIndex = 0;
            this.labelComPort.Text = "串口号";
            // 
            // panelRowPort
            // 
            this.panelRowPort.Controls.Add(this.inputPort);
            this.panelRowPort.Controls.Add(this.labelPort);
            this.panelRowPort.Location = new System.Drawing.Point(4, 214);
            this.panelRowPort.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowPort.Name = "panelRowPort";
            this.panelRowPort.Radius = 0;
            this.panelRowPort.Size = new System.Drawing.Size(246, 52);
            this.panelRowPort.TabIndex = 2;
            // 
            // inputPort
            // 
            this.inputPort.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputPort.Location = new System.Drawing.Point(0, 20);
            this.inputPort.Margin = new System.Windows.Forms.Padding(0);
            this.inputPort.Name = "inputPort";
            this.inputPort.PlaceholderText = "请输入端口";
            this.inputPort.Size = new System.Drawing.Size(246, 32);
            this.inputPort.TabIndex = 1;
            this.inputPort.WaveSize = 0;
            // 
            // labelPort
            // 
            this.labelPort.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelPort.Location = new System.Drawing.Point(0, 0);
            this.labelPort.Margin = new System.Windows.Forms.Padding(0);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(246, 22);
            this.labelPort.TabIndex = 0;
            this.labelPort.Text = "端口";
            // 
            // panelRowIpAddress
            // 
            this.panelRowIpAddress.Controls.Add(this.inputIpAddress);
            this.panelRowIpAddress.Controls.Add(this.labelIpAddress);
            this.panelRowIpAddress.Location = new System.Drawing.Point(4, 158);
            this.panelRowIpAddress.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowIpAddress.Name = "panelRowIpAddress";
            this.panelRowIpAddress.Radius = 0;
            this.panelRowIpAddress.Size = new System.Drawing.Size(246, 52);
            this.panelRowIpAddress.TabIndex = 1;
            // 
            // inputIpAddress
            // 
            this.inputIpAddress.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputIpAddress.Location = new System.Drawing.Point(0, 20);
            this.inputIpAddress.Margin = new System.Windows.Forms.Padding(0);
            this.inputIpAddress.Name = "inputIpAddress";
            this.inputIpAddress.PlaceholderText = "请输入IP地址";
            this.inputIpAddress.Size = new System.Drawing.Size(246, 32);
            this.inputIpAddress.TabIndex = 1;
            this.inputIpAddress.WaveSize = 0;
            // 
            // labelIpAddress
            // 
            this.labelIpAddress.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelIpAddress.Location = new System.Drawing.Point(0, 0);
            this.labelIpAddress.Margin = new System.Windows.Forms.Padding(0);
            this.labelIpAddress.Name = "labelIpAddress";
            this.labelIpAddress.Size = new System.Drawing.Size(246, 22);
            this.labelIpAddress.TabIndex = 0;
            this.labelIpAddress.Text = "IP地址";
            // 
            // labelSectionAddress
            // 
            this.labelSectionAddress.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionAddress.Location = new System.Drawing.Point(4, 4);
            this.labelSectionAddress.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionAddress.Name = "labelSectionAddress";
            this.labelSectionAddress.Size = new System.Drawing.Size(246, 26);
            this.labelSectionAddress.TabIndex = 0;
            this.labelSectionAddress.Text = "通讯参数";
            // 
            // stackSectionBasic
            // 
            this.stackSectionBasic.Controls.Add(this.panelRowProtocolType);
            this.stackSectionBasic.Controls.Add(this.panelRowConnectionType);
            this.stackSectionBasic.Controls.Add(this.inputModel);
            this.stackSectionBasic.Controls.Add(this.labelModel);
            this.stackSectionBasic.Controls.Add(this.inputVendor);
            this.stackSectionBasic.Controls.Add(this.labelVendor);
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
            this.stackSectionBasic.Size = new System.Drawing.Size(254, 463);
            this.stackSectionBasic.TabIndex = 0;
            this.stackSectionBasic.Text = "stackSectionBasic";
            this.stackSectionBasic.Vertical = true;
            // 
            // panelRowProtocolType
            // 
            this.panelRowProtocolType.Controls.Add(this.dropdownProtocolType);
            this.panelRowProtocolType.Controls.Add(this.labelProtocolType);
            this.panelRowProtocolType.Location = new System.Drawing.Point(4, 282);
            this.panelRowProtocolType.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowProtocolType.Name = "panelRowProtocolType";
            this.panelRowProtocolType.Radius = 0;
            this.panelRowProtocolType.Size = new System.Drawing.Size(246, 52);
            this.panelRowProtocolType.TabIndex = 10;
            // 
            // dropdownProtocolType
            // 
            this.dropdownProtocolType.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dropdownProtocolType.Location = new System.Drawing.Point(0, 20);
            this.dropdownProtocolType.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownProtocolType.Name = "dropdownProtocolType";
            this.dropdownProtocolType.Size = new System.Drawing.Size(246, 32);
            this.dropdownProtocolType.TabIndex = 1;
            this.dropdownProtocolType.WaveSize = 0;
            // 
            // labelProtocolType
            // 
            this.labelProtocolType.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelProtocolType.Location = new System.Drawing.Point(0, 0);
            this.labelProtocolType.Margin = new System.Windows.Forms.Padding(0);
            this.labelProtocolType.Name = "labelProtocolType";
            this.labelProtocolType.Size = new System.Drawing.Size(246, 22);
            this.labelProtocolType.TabIndex = 0;
            this.labelProtocolType.Text = "协议类型";
            // 
            // panelRowConnectionType
            // 
            this.panelRowConnectionType.Controls.Add(this.dropdownConnectionType);
            this.panelRowConnectionType.Controls.Add(this.labelConnectionType);
            this.panelRowConnectionType.Location = new System.Drawing.Point(4, 226);
            this.panelRowConnectionType.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowConnectionType.Name = "panelRowConnectionType";
            this.panelRowConnectionType.Radius = 0;
            this.panelRowConnectionType.Size = new System.Drawing.Size(246, 52);
            this.panelRowConnectionType.TabIndex = 9;
            // 
            // dropdownConnectionType
            // 
            this.dropdownConnectionType.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dropdownConnectionType.Location = new System.Drawing.Point(0, 20);
            this.dropdownConnectionType.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownConnectionType.Name = "dropdownConnectionType";
            this.dropdownConnectionType.Size = new System.Drawing.Size(246, 32);
            this.dropdownConnectionType.TabIndex = 1;
            this.dropdownConnectionType.WaveSize = 0;
            // 
            // labelConnectionType
            // 
            this.labelConnectionType.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelConnectionType.Location = new System.Drawing.Point(0, 0);
            this.labelConnectionType.Margin = new System.Windows.Forms.Padding(0);
            this.labelConnectionType.Name = "labelConnectionType";
            this.labelConnectionType.Size = new System.Drawing.Size(246, 22);
            this.labelConnectionType.TabIndex = 0;
            this.labelConnectionType.Text = "连接方式";
            // 
            // inputModel
            // 
            this.inputModel.Location = new System.Drawing.Point(4, 190);
            this.inputModel.Margin = new System.Windows.Forms.Padding(0);
            this.inputModel.Name = "inputModel";
            this.inputModel.PlaceholderText = "请输入型号";
            this.inputModel.Size = new System.Drawing.Size(246, 32);
            this.inputModel.TabIndex = 8;
            this.inputModel.WaveSize = 0;
            // 
            // labelModel
            // 
            this.labelModel.Location = new System.Drawing.Point(4, 164);
            this.labelModel.Margin = new System.Windows.Forms.Padding(0);
            this.labelModel.Name = "labelModel";
            this.labelModel.Size = new System.Drawing.Size(246, 22);
            this.labelModel.TabIndex = 7;
            this.labelModel.Text = "型号";
            // 
            // inputVendor
            // 
            this.inputVendor.Location = new System.Drawing.Point(4, 128);
            this.inputVendor.Margin = new System.Windows.Forms.Padding(0);
            this.inputVendor.Name = "inputVendor";
            this.inputVendor.PlaceholderText = "请输入厂商";
            this.inputVendor.Size = new System.Drawing.Size(246, 32);
            this.inputVendor.TabIndex = 6;
            this.inputVendor.WaveSize = 0;
            // 
            // labelVendor
            // 
            this.labelVendor.Location = new System.Drawing.Point(4, 102);
            this.labelVendor.Margin = new System.Windows.Forms.Padding(0);
            this.labelVendor.Name = "labelVendor";
            this.labelVendor.Size = new System.Drawing.Size(246, 22);
            this.labelVendor.TabIndex = 5;
            this.labelVendor.Text = "厂商";
            // 
            // inputDisplayName
            // 
            this.inputDisplayName.Location = new System.Drawing.Point(4, 66);
            this.inputDisplayName.Margin = new System.Windows.Forms.Padding(0);
            this.inputDisplayName.Name = "inputDisplayName";
            this.inputDisplayName.PlaceholderText = "请输入显示名";
            this.inputDisplayName.Size = new System.Drawing.Size(246, 32);
            this.inputDisplayName.TabIndex = 4;
            this.inputDisplayName.WaveSize = 0;
            // 
            // labelDisplayName
            // 
            this.labelDisplayName.Location = new System.Drawing.Point(4, 40);
            this.labelDisplayName.Margin = new System.Windows.Forms.Padding(0);
            this.labelDisplayName.Name = "labelDisplayName";
            this.labelDisplayName.Size = new System.Drawing.Size(246, 22);
            this.labelDisplayName.TabIndex = 3;
            this.labelDisplayName.Text = "显示名";
            // 
            // inputName
            // 
            this.inputName.Location = new System.Drawing.Point(4, 4);
            this.inputName.Margin = new System.Windows.Forms.Padding(0);
            this.inputName.Name = "inputName";
            this.inputName.PlaceholderText = "请输入 PLC 名称";
            this.inputName.Size = new System.Drawing.Size(246, 32);
            this.inputName.TabIndex = 2;
            this.inputName.WaveSize = 0;
            // 
            // labelName
            // 
            this.labelName.Location = new System.Drawing.Point(4, 4);
            this.labelName.Margin = new System.Windows.Forms.Padding(0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(246, 22);
            this.labelName.TabIndex = 1;
            this.labelName.Text = "名称";
            // 
            // labelSectionBasic
            // 
            this.labelSectionBasic.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionBasic.Location = new System.Drawing.Point(4, 4);
            this.labelSectionBasic.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionBasic.Name = "labelSectionBasic";
            this.labelSectionBasic.Size = new System.Drawing.Size(246, 26);
            this.labelSectionBasic.TabIndex = 0;
            this.labelSectionBasic.Text = "基础信息";
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.flowFooterButtons);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(28, 547);
            this.panelFooter.Margin = new System.Windows.Forms.Padding(0);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Padding = new System.Windows.Forms.Padding(4, 10, 4, 0);
            this.panelFooter.Radius = 0;
            this.panelFooter.Size = new System.Drawing.Size(1024, 85);
            this.panelFooter.TabIndex = 2;
            // 
            // flowFooterButtons
            // 
            this.flowFooterButtons.Controls.Add(this.buttonOk);
            this.flowFooterButtons.Controls.Add(this.buttonCancel);
            this.flowFooterButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowFooterButtons.Gap = 10;
            this.flowFooterButtons.Location = new System.Drawing.Point(776, 10);
            this.flowFooterButtons.Margin = new System.Windows.Forms.Padding(0);
            this.flowFooterButtons.Name = "flowFooterButtons";
            this.flowFooterButtons.Size = new System.Drawing.Size(244, 75);
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
            this.panelHeader.Size = new System.Drawing.Size(1024, 56);
            this.panelHeader.TabIndex = 0;
            // 
            // flowHeaderRight
            // 
            this.flowHeaderRight.Controls.Add(this.labelDialogDescription);
            this.flowHeaderRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowHeaderRight.Location = new System.Drawing.Point(640, 0);
            this.flowHeaderRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowHeaderRight.Name = "flowHeaderRight";
            this.flowHeaderRight.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.flowHeaderRight.Size = new System.Drawing.Size(380, 48);
            this.flowHeaderRight.TabIndex = 1;
            this.flowHeaderRight.Text = "flowHeaderRight";
            // 
            // labelDialogDescription
            // 
            this.labelDialogDescription.Location = new System.Drawing.Point(0, 0);
            this.labelDialogDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDialogDescription.Name = "labelDialogDescription";
            this.labelDialogDescription.Size = new System.Drawing.Size(380, 48);
            this.labelDialogDescription.TabIndex = 0;
            this.labelDialogDescription.Text = "配置 PLC 站的协议、连接参数与运行参数。";
            this.labelDialogDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // flowHeaderLeft
            // 
            this.flowHeaderLeft.Controls.Add(this.labelDialogTitle);
            this.flowHeaderLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowHeaderLeft.Location = new System.Drawing.Point(4, 0);
            this.flowHeaderLeft.Margin = new System.Windows.Forms.Padding(0);
            this.flowHeaderLeft.Name = "flowHeaderLeft";
            this.flowHeaderLeft.Size = new System.Drawing.Size(240, 48);
            this.flowHeaderLeft.TabIndex = 0;
            this.flowHeaderLeft.Text = "flowHeaderLeft";
            // 
            // labelDialogTitle
            // 
            this.labelDialogTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelDialogTitle.Location = new System.Drawing.Point(0, 0);
            this.labelDialogTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelDialogTitle.Name = "labelDialogTitle";
            this.labelDialogTitle.Size = new System.Drawing.Size(240, 48);
            this.labelDialogTitle.TabIndex = 0;
            this.labelDialogTitle.Text = "新增 PLC 站";
            // 
            // PlcStationEditDialog
            // 
            this.ClientSize = new System.Drawing.Size(1080, 660);
            this.Controls.Add(this.textureBackgroundDialog);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PlcStationEditDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PLC 站";
            this.textureBackgroundDialog.ResumeLayout(false);
            this.panelShell.ResumeLayout(false);
            this.panelContent.ResumeLayout(false);
            this.gridMainSections.ResumeLayout(false);
            this.stackSectionRemark.ResumeLayout(false);
            this.stackSectionRuntime.ResumeLayout(false);
            this.stackSectionAddress.ResumeLayout(false);
            this.panelRowStopBits.ResumeLayout(false);
            this.panelRowParity.ResumeLayout(false);
            this.panelRowDataBits.ResumeLayout(false);
            this.panelRowBaudRate.ResumeLayout(false);
            this.panelRowComPort.ResumeLayout(false);
            this.panelRowPort.ResumeLayout(false);
            this.panelRowIpAddress.ResumeLayout(false);
            this.stackSectionBasic.ResumeLayout(false);
            this.panelRowProtocolType.ResumeLayout(false);
            this.panelRowConnectionType.ResumeLayout(false);
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
        private AntdUI.Label labelSectionBasic;
        private AntdUI.Input inputName;
        private AntdUI.Label labelName;
        private AntdUI.Input inputDisplayName;
        private AntdUI.Label labelDisplayName;
        private AntdUI.Input inputVendor;
        private AntdUI.Label labelVendor;
        private AntdUI.Input inputModel;
        private AntdUI.Label labelModel;
        private AntdUI.Panel panelRowConnectionType;
        private AntdUI.Select dropdownConnectionType;
        private AntdUI.Label labelConnectionType;
        private AntdUI.Panel panelRowProtocolType;
        private AntdUI.Select dropdownProtocolType;
        private AntdUI.Label labelProtocolType;
        private AntdUI.StackPanel stackSectionAddress;
        private AntdUI.Label labelSectionAddress;
        private AntdUI.Panel panelRowIpAddress;
        private AntdUI.Input inputIpAddress;
        private AntdUI.Label labelIpAddress;
        private AntdUI.Panel panelRowPort;
        private AntdUI.Input inputPort;
        private AntdUI.Label labelPort;
        private AntdUI.Panel panelRowComPort;
        private AntdUI.Input inputComPort;
        private AntdUI.Label labelComPort;
        private AntdUI.Panel panelRowBaudRate;
        private AntdUI.Input inputBaudRate;
        private AntdUI.Label labelBaudRate;
        private AntdUI.Panel panelRowDataBits;
        private AntdUI.Input inputDataBits;
        private AntdUI.Label labelDataBits;
        private AntdUI.Panel panelRowParity;
        private AntdUI.Select dropdownParity;
        private AntdUI.Label labelParity;
        private AntdUI.Panel panelRowStopBits;
        private AntdUI.Select dropdownStopBits;
        private AntdUI.Label labelStopBits;
        private AntdUI.Input inputStationNo;
        private AntdUI.Label labelStationNo;
        private AntdUI.Input inputNetworkNo;
        private AntdUI.Label labelNetworkNo;
        private AntdUI.Input inputPcNo;
        private AntdUI.Label labelPcNo;
        private AntdUI.StackPanel stackSectionRuntime;
        private AntdUI.Label labelSectionRuntime;
        private AntdUI.Input inputRack;
        private AntdUI.Label labelRack;
        private AntdUI.Input inputSlot;
        private AntdUI.Label labelSlot;
        private AntdUI.Input inputTimeoutMs;
        private AntdUI.Label labelTimeoutMs;
        private AntdUI.Input inputReconnectIntervalMs;
        private AntdUI.Label labelReconnectIntervalMs;
        private AntdUI.Input inputScanIntervalMs;
        private AntdUI.Label labelScanIntervalMs;
        private AntdUI.StackPanel stackSectionRemark;
        private AntdUI.Label labelSectionRemark;
        private AntdUI.Label labelEnabled;
        private AntdUI.Checkbox checkEnabled;
        private AntdUI.Input inputSortOrder;
        private AntdUI.Label labelSortOrder;
        private AntdUI.Input inputDescription;
        private AntdUI.Label labelDescription;
        private AntdUI.Input inputRemark;
        private AntdUI.Label labelRemark;
        private AntdUI.Panel panelFooter;
        private AntdUI.FlowPanel flowFooterButtons;
        private AntdUI.Button buttonCancel;
        private AntdUI.Button buttonOk;
    }
}