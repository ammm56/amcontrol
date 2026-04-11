namespace AMControlWinF.Views.SysConfig
{
    partial class PlcPointEditDialog
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

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
            this.panelRowEnabled = new AntdUI.Panel();
            this.flowEnabledHost = new AntdUI.FlowPanel();
            this.checkEnabled = new AntdUI.Checkbox();
            this.labelEnabled = new AntdUI.Label();
            this.panelRowSortOrder = new AntdUI.Panel();
            this.inputSortOrder = new AntdUI.Input();
            this.labelSortOrder = new AntdUI.Label();
            this.panelRowAccessMode = new AntdUI.Panel();
            this.dropdownAccessMode = new AntdUI.Select();
            this.labelAccessMode = new AntdUI.Label();
            this.panelRowLength = new AntdUI.Panel();
            this.inputLength = new AntdUI.Input();
            this.labelLength = new AntdUI.Label();
            this.panelRowDataType = new AntdUI.Panel();
            this.dropdownDataType = new AntdUI.Select();
            this.labelDataType = new AntdUI.Label();
            this.panelRowAddress = new AntdUI.Panel();
            this.inputAddress = new AntdUI.Input();
            this.labelAddress = new AntdUI.Label();
            this.panelRowGroupName = new AntdUI.Panel();
            this.inputGroupName = new AntdUI.Input();
            this.labelGroupName = new AntdUI.Label();
            this.panelRowDisplayName = new AntdUI.Panel();
            this.inputDisplayName = new AntdUI.Input();
            this.labelDisplayName = new AntdUI.Label();
            this.panelRowName = new AntdUI.Panel();
            this.inputName = new AntdUI.Input();
            this.labelName = new AntdUI.Label();
            this.panelRowPlcName = new AntdUI.Panel();
            this.dropdownPlcName = new AntdUI.Select();
            this.labelPlcName = new AntdUI.Label();
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
            this.panelRowEnabled.SuspendLayout();
            this.flowEnabledHost.SuspendLayout();
            this.panelRowSortOrder.SuspendLayout();
            this.panelRowAccessMode.SuspendLayout();
            this.panelRowLength.SuspendLayout();
            this.panelRowDataType.SuspendLayout();
            this.panelRowAddress.SuspendLayout();
            this.panelRowGroupName.SuspendLayout();
            this.panelRowDisplayName.SuspendLayout();
            this.panelRowName.SuspendLayout();
            this.panelRowPlcName.SuspendLayout();
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
            this.textureBackgroundDialog.Size = new System.Drawing.Size(620, 680);
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
            this.panelShell.ShadowOpacityAnimation = true;
            this.panelShell.ShadowOpacityHover = 0.2F;
            this.panelShell.Size = new System.Drawing.Size(620, 680);
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
            this.panelContent.Size = new System.Drawing.Size(564, 511);
            this.panelContent.TabIndex = 1;
            // 
            // stackFormRows
            // 
            this.stackFormRows.AutoScroll = true;
            this.stackFormRows.Controls.Add(this.panelRowRemark);
            this.stackFormRows.Controls.Add(this.panelRowDescription);
            this.stackFormRows.Controls.Add(this.panelRowEnabled);
            this.stackFormRows.Controls.Add(this.panelRowSortOrder);
            this.stackFormRows.Controls.Add(this.panelRowAccessMode);
            this.stackFormRows.Controls.Add(this.panelRowLength);
            this.stackFormRows.Controls.Add(this.panelRowDataType);
            this.stackFormRows.Controls.Add(this.panelRowAddress);
            this.stackFormRows.Controls.Add(this.panelRowGroupName);
            this.stackFormRows.Controls.Add(this.panelRowDisplayName);
            this.stackFormRows.Controls.Add(this.panelRowName);
            this.stackFormRows.Controls.Add(this.panelRowPlcName);
            this.stackFormRows.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stackFormRows.Gap = 4;
            this.stackFormRows.Location = new System.Drawing.Point(4, 0);
            this.stackFormRows.Margin = new System.Windows.Forms.Padding(0);
            this.stackFormRows.Name = "stackFormRows";
            this.stackFormRows.Size = new System.Drawing.Size(556, 511);
            this.stackFormRows.TabIndex = 0;
            this.stackFormRows.Text = "stackFormRows";
            this.stackFormRows.Vertical = true;
            // 
            // panelRowRemark
            // 
            this.panelRowRemark.Controls.Add(this.inputRemark);
            this.panelRowRemark.Controls.Add(this.labelRemark);
            this.panelRowRemark.Location = new System.Drawing.Point(0, 708);
            this.panelRowRemark.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowRemark.Name = "panelRowRemark";
            this.panelRowRemark.Radius = 0;
            this.panelRowRemark.Size = new System.Drawing.Size(556, 92);
            this.panelRowRemark.TabIndex = 11;
            // 
            // inputRemark
            // 
            this.inputRemark.AutoScroll = true;
            this.inputRemark.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputRemark.Location = new System.Drawing.Point(0, 26);
            this.inputRemark.Margin = new System.Windows.Forms.Padding(0);
            this.inputRemark.Multiline = true;
            this.inputRemark.Name = "inputRemark";
            this.inputRemark.PlaceholderText = "请输入备注（可选）";
            this.inputRemark.Size = new System.Drawing.Size(556, 66);
            this.inputRemark.TabIndex = 1;
            this.inputRemark.WaveSize = 0;
            // 
            // labelRemark
            // 
            this.labelRemark.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelRemark.Location = new System.Drawing.Point(0, 0);
            this.labelRemark.Margin = new System.Windows.Forms.Padding(0);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(556, 22);
            this.labelRemark.TabIndex = 0;
            this.labelRemark.Text = "备注";
            // 
            // panelRowDescription
            // 
            this.panelRowDescription.Controls.Add(this.inputDescription);
            this.panelRowDescription.Controls.Add(this.labelDescription);
            this.panelRowDescription.Location = new System.Drawing.Point(0, 612);
            this.panelRowDescription.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowDescription.Name = "panelRowDescription";
            this.panelRowDescription.Radius = 0;
            this.panelRowDescription.Size = new System.Drawing.Size(556, 92);
            this.panelRowDescription.TabIndex = 10;
            // 
            // inputDescription
            // 
            this.inputDescription.AutoScroll = true;
            this.inputDescription.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputDescription.Location = new System.Drawing.Point(0, 26);
            this.inputDescription.Margin = new System.Windows.Forms.Padding(0);
            this.inputDescription.Multiline = true;
            this.inputDescription.Name = "inputDescription";
            this.inputDescription.PlaceholderText = "请输入描述（可选）";
            this.inputDescription.Size = new System.Drawing.Size(556, 66);
            this.inputDescription.TabIndex = 1;
            this.inputDescription.WaveSize = 0;
            // 
            // labelDescription
            // 
            this.labelDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDescription.Location = new System.Drawing.Point(0, 0);
            this.labelDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(556, 22);
            this.labelDescription.TabIndex = 0;
            this.labelDescription.Text = "描述";
            // 
            // panelRowEnabled
            // 
            this.panelRowEnabled.Controls.Add(this.flowEnabledHost);
            this.panelRowEnabled.Controls.Add(this.labelEnabled);
            this.panelRowEnabled.Location = new System.Drawing.Point(0, 552);
            this.panelRowEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowEnabled.Name = "panelRowEnabled";
            this.panelRowEnabled.Radius = 0;
            this.panelRowEnabled.Size = new System.Drawing.Size(556, 56);
            this.panelRowEnabled.TabIndex = 9;
            // 
            // flowEnabledHost
            // 
            this.flowEnabledHost.Controls.Add(this.checkEnabled);
            this.flowEnabledHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowEnabledHost.Location = new System.Drawing.Point(0, 22);
            this.flowEnabledHost.Margin = new System.Windows.Forms.Padding(0);
            this.flowEnabledHost.Name = "flowEnabledHost";
            this.flowEnabledHost.Size = new System.Drawing.Size(556, 34);
            this.flowEnabledHost.TabIndex = 1;
            this.flowEnabledHost.Text = "flowEnabledHost";
            // 
            // checkEnabled
            // 
            this.checkEnabled.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkEnabled.Checked = true;
            this.checkEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkEnabled.Location = new System.Drawing.Point(0, 0);
            this.checkEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.checkEnabled.Name = "checkEnabled";
            this.checkEnabled.Size = new System.Drawing.Size(61, 34);
            this.checkEnabled.TabIndex = 0;
            this.checkEnabled.Text = "启用";
            // 
            // labelEnabled
            // 
            this.labelEnabled.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelEnabled.Location = new System.Drawing.Point(0, 0);
            this.labelEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.labelEnabled.Name = "labelEnabled";
            this.labelEnabled.Size = new System.Drawing.Size(556, 22);
            this.labelEnabled.TabIndex = 0;
            this.labelEnabled.Text = "启用状态";
            // 
            // panelRowSortOrder
            // 
            this.panelRowSortOrder.Controls.Add(this.inputSortOrder);
            this.panelRowSortOrder.Controls.Add(this.labelSortOrder);
            this.panelRowSortOrder.Location = new System.Drawing.Point(0, 492);
            this.panelRowSortOrder.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowSortOrder.Name = "panelRowSortOrder";
            this.panelRowSortOrder.Radius = 0;
            this.panelRowSortOrder.Size = new System.Drawing.Size(556, 56);
            this.panelRowSortOrder.TabIndex = 8;
            // 
            // inputSortOrder
            // 
            this.inputSortOrder.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputSortOrder.Location = new System.Drawing.Point(0, 20);
            this.inputSortOrder.Margin = new System.Windows.Forms.Padding(0);
            this.inputSortOrder.Name = "inputSortOrder";
            this.inputSortOrder.PlaceholderText = "请输入排序号";
            this.inputSortOrder.Size = new System.Drawing.Size(556, 36);
            this.inputSortOrder.TabIndex = 1;
            this.inputSortOrder.WaveSize = 0;
            // 
            // labelSortOrder
            // 
            this.labelSortOrder.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelSortOrder.Location = new System.Drawing.Point(0, 0);
            this.labelSortOrder.Margin = new System.Windows.Forms.Padding(0);
            this.labelSortOrder.Name = "labelSortOrder";
            this.labelSortOrder.Size = new System.Drawing.Size(556, 22);
            this.labelSortOrder.TabIndex = 0;
            this.labelSortOrder.Text = "排序号";
            // 
            // panelRowAccessMode
            // 
            this.panelRowAccessMode.Controls.Add(this.dropdownAccessMode);
            this.panelRowAccessMode.Controls.Add(this.labelAccessMode);
            this.panelRowAccessMode.Location = new System.Drawing.Point(0, 432);
            this.panelRowAccessMode.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowAccessMode.Name = "panelRowAccessMode";
            this.panelRowAccessMode.Radius = 0;
            this.panelRowAccessMode.Size = new System.Drawing.Size(556, 56);
            this.panelRowAccessMode.TabIndex = 7;
            // 
            // dropdownAccessMode
            // 
            this.dropdownAccessMode.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dropdownAccessMode.Location = new System.Drawing.Point(0, 20);
            this.dropdownAccessMode.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownAccessMode.Name = "dropdownAccessMode";
            this.dropdownAccessMode.Size = new System.Drawing.Size(556, 36);
            this.dropdownAccessMode.TabIndex = 1;
            this.dropdownAccessMode.WaveSize = 0;
            // 
            // labelAccessMode
            // 
            this.labelAccessMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelAccessMode.Location = new System.Drawing.Point(0, 0);
            this.labelAccessMode.Margin = new System.Windows.Forms.Padding(0);
            this.labelAccessMode.Name = "labelAccessMode";
            this.labelAccessMode.Size = new System.Drawing.Size(556, 22);
            this.labelAccessMode.TabIndex = 0;
            this.labelAccessMode.Text = "访问模式";
            // 
            // panelRowLength
            // 
            this.panelRowLength.Controls.Add(this.inputLength);
            this.panelRowLength.Controls.Add(this.labelLength);
            this.panelRowLength.Location = new System.Drawing.Point(0, 372);
            this.panelRowLength.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowLength.Name = "panelRowLength";
            this.panelRowLength.Radius = 0;
            this.panelRowLength.Size = new System.Drawing.Size(556, 56);
            this.panelRowLength.TabIndex = 6;
            // 
            // inputLength
            // 
            this.inputLength.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputLength.Location = new System.Drawing.Point(0, 20);
            this.inputLength.Margin = new System.Windows.Forms.Padding(0);
            this.inputLength.Name = "inputLength";
            this.inputLength.PlaceholderText = "请输入 Length";
            this.inputLength.Size = new System.Drawing.Size(556, 36);
            this.inputLength.TabIndex = 1;
            this.inputLength.WaveSize = 0;
            // 
            // labelLength
            // 
            this.labelLength.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelLength.Location = new System.Drawing.Point(0, 0);
            this.labelLength.Margin = new System.Windows.Forms.Padding(0);
            this.labelLength.Name = "labelLength";
            this.labelLength.Size = new System.Drawing.Size(556, 22);
            this.labelLength.TabIndex = 0;
            this.labelLength.Text = "Length";
            // 
            // panelRowDataType
            // 
            this.panelRowDataType.Controls.Add(this.dropdownDataType);
            this.panelRowDataType.Controls.Add(this.labelDataType);
            this.panelRowDataType.Location = new System.Drawing.Point(0, 312);
            this.panelRowDataType.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowDataType.Name = "panelRowDataType";
            this.panelRowDataType.Radius = 0;
            this.panelRowDataType.Size = new System.Drawing.Size(556, 56);
            this.panelRowDataType.TabIndex = 5;
            // 
            // dropdownDataType
            // 
            this.dropdownDataType.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dropdownDataType.Location = new System.Drawing.Point(0, 20);
            this.dropdownDataType.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownDataType.Name = "dropdownDataType";
            this.dropdownDataType.Size = new System.Drawing.Size(556, 36);
            this.dropdownDataType.TabIndex = 1;
            this.dropdownDataType.WaveSize = 0;
            // 
            // labelDataType
            // 
            this.labelDataType.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDataType.Location = new System.Drawing.Point(0, 0);
            this.labelDataType.Margin = new System.Windows.Forms.Padding(0);
            this.labelDataType.Name = "labelDataType";
            this.labelDataType.Size = new System.Drawing.Size(556, 22);
            this.labelDataType.TabIndex = 0;
            this.labelDataType.Text = "数据类型";
            // 
            // panelRowAddress
            // 
            this.panelRowAddress.Controls.Add(this.inputAddress);
            this.panelRowAddress.Controls.Add(this.labelAddress);
            this.panelRowAddress.Location = new System.Drawing.Point(0, 252);
            this.panelRowAddress.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowAddress.Name = "panelRowAddress";
            this.panelRowAddress.Radius = 0;
            this.panelRowAddress.Size = new System.Drawing.Size(556, 56);
            this.panelRowAddress.TabIndex = 4;
            // 
            // inputAddress
            // 
            this.inputAddress.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputAddress.Location = new System.Drawing.Point(0, 20);
            this.inputAddress.Margin = new System.Windows.Forms.Padding(0);
            this.inputAddress.Name = "inputAddress";
            this.inputAddress.PlaceholderText = "请输入协议地址";
            this.inputAddress.Size = new System.Drawing.Size(556, 36);
            this.inputAddress.TabIndex = 1;
            this.inputAddress.WaveSize = 0;
            // 
            // labelAddress
            // 
            this.labelAddress.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelAddress.Location = new System.Drawing.Point(0, 0);
            this.labelAddress.Margin = new System.Windows.Forms.Padding(0);
            this.labelAddress.Name = "labelAddress";
            this.labelAddress.Size = new System.Drawing.Size(556, 22);
            this.labelAddress.TabIndex = 0;
            this.labelAddress.Text = "地址";
            // 
            // panelRowGroupName
            // 
            this.panelRowGroupName.Controls.Add(this.inputGroupName);
            this.panelRowGroupName.Controls.Add(this.labelGroupName);
            this.panelRowGroupName.Location = new System.Drawing.Point(0, 192);
            this.panelRowGroupName.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowGroupName.Name = "panelRowGroupName";
            this.panelRowGroupName.Radius = 0;
            this.panelRowGroupName.Size = new System.Drawing.Size(556, 56);
            this.panelRowGroupName.TabIndex = 3;
            // 
            // inputGroupName
            // 
            this.inputGroupName.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputGroupName.Location = new System.Drawing.Point(0, 20);
            this.inputGroupName.Margin = new System.Windows.Forms.Padding(0);
            this.inputGroupName.Name = "inputGroupName";
            this.inputGroupName.PlaceholderText = "请输入分组（可选）";
            this.inputGroupName.Size = new System.Drawing.Size(556, 36);
            this.inputGroupName.TabIndex = 1;
            this.inputGroupName.WaveSize = 0;
            // 
            // labelGroupName
            // 
            this.labelGroupName.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelGroupName.Location = new System.Drawing.Point(0, 0);
            this.labelGroupName.Margin = new System.Windows.Forms.Padding(0);
            this.labelGroupName.Name = "labelGroupName";
            this.labelGroupName.Size = new System.Drawing.Size(556, 22);
            this.labelGroupName.TabIndex = 0;
            this.labelGroupName.Text = "分组";
            // 
            // panelRowDisplayName
            // 
            this.panelRowDisplayName.Controls.Add(this.inputDisplayName);
            this.panelRowDisplayName.Controls.Add(this.labelDisplayName);
            this.panelRowDisplayName.Location = new System.Drawing.Point(0, 132);
            this.panelRowDisplayName.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowDisplayName.Name = "panelRowDisplayName";
            this.panelRowDisplayName.Radius = 0;
            this.panelRowDisplayName.Size = new System.Drawing.Size(556, 56);
            this.panelRowDisplayName.TabIndex = 2;
            // 
            // inputDisplayName
            // 
            this.inputDisplayName.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputDisplayName.Location = new System.Drawing.Point(0, 20);
            this.inputDisplayName.Margin = new System.Windows.Forms.Padding(0);
            this.inputDisplayName.Name = "inputDisplayName";
            this.inputDisplayName.PlaceholderText = "请输入显示名";
            this.inputDisplayName.Size = new System.Drawing.Size(556, 36);
            this.inputDisplayName.TabIndex = 1;
            this.inputDisplayName.WaveSize = 0;
            // 
            // labelDisplayName
            // 
            this.labelDisplayName.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDisplayName.Location = new System.Drawing.Point(0, 0);
            this.labelDisplayName.Margin = new System.Windows.Forms.Padding(0);
            this.labelDisplayName.Name = "labelDisplayName";
            this.labelDisplayName.Size = new System.Drawing.Size(556, 22);
            this.labelDisplayName.TabIndex = 0;
            this.labelDisplayName.Text = "显示名";
            // 
            // panelRowName
            // 
            this.panelRowName.Controls.Add(this.inputName);
            this.panelRowName.Controls.Add(this.labelName);
            this.panelRowName.Location = new System.Drawing.Point(0, 72);
            this.panelRowName.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowName.Name = "panelRowName";
            this.panelRowName.Radius = 0;
            this.panelRowName.Size = new System.Drawing.Size(556, 56);
            this.panelRowName.TabIndex = 1;
            // 
            // inputName
            // 
            this.inputName.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputName.Location = new System.Drawing.Point(0, 20);
            this.inputName.Margin = new System.Windows.Forms.Padding(0);
            this.inputName.Name = "inputName";
            this.inputName.PlaceholderText = "请输入点位名称";
            this.inputName.Size = new System.Drawing.Size(556, 36);
            this.inputName.TabIndex = 1;
            this.inputName.WaveSize = 0;
            // 
            // labelName
            // 
            this.labelName.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelName.Location = new System.Drawing.Point(0, 0);
            this.labelName.Margin = new System.Windows.Forms.Padding(0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(556, 22);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "名称";
            // 
            // panelRowPlcName
            // 
            this.panelRowPlcName.Controls.Add(this.dropdownPlcName);
            this.panelRowPlcName.Controls.Add(this.labelPlcName);
            this.panelRowPlcName.Location = new System.Drawing.Point(0, 12);
            this.panelRowPlcName.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowPlcName.Name = "panelRowPlcName";
            this.panelRowPlcName.Radius = 0;
            this.panelRowPlcName.Size = new System.Drawing.Size(556, 56);
            this.panelRowPlcName.TabIndex = 0;
            // 
            // dropdownPlcName
            // 
            this.dropdownPlcName.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dropdownPlcName.Location = new System.Drawing.Point(0, 20);
            this.dropdownPlcName.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownPlcName.Name = "dropdownPlcName";
            this.dropdownPlcName.Size = new System.Drawing.Size(556, 36);
            this.dropdownPlcName.TabIndex = 1;
            this.dropdownPlcName.WaveSize = 0;
            // 
            // labelPlcName
            // 
            this.labelPlcName.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelPlcName.Location = new System.Drawing.Point(0, 0);
            this.labelPlcName.Margin = new System.Windows.Forms.Padding(0);
            this.labelPlcName.Name = "labelPlcName";
            this.labelPlcName.Size = new System.Drawing.Size(556, 22);
            this.labelPlcName.TabIndex = 0;
            this.labelPlcName.Text = "所属PLC";
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.flowFooterButtons);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(28, 595);
            this.panelFooter.Margin = new System.Windows.Forms.Padding(0);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Padding = new System.Windows.Forms.Padding(4, 10, 4, 0);
            this.panelFooter.Radius = 0;
            this.panelFooter.Size = new System.Drawing.Size(564, 57);
            this.panelFooter.TabIndex = 2;
            // 
            // flowFooterButtons
            // 
            this.flowFooterButtons.Controls.Add(this.buttonOk);
            this.flowFooterButtons.Controls.Add(this.buttonCancel);
            this.flowFooterButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowFooterButtons.Gap = 10;
            this.flowFooterButtons.Location = new System.Drawing.Point(316, 10);
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
            this.panelHeader.Size = new System.Drawing.Size(564, 56);
            this.panelHeader.TabIndex = 0;
            // 
            // flowHeaderRight
            // 
            this.flowHeaderRight.Controls.Add(this.labelDialogDescription);
            this.flowHeaderRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowHeaderRight.Location = new System.Drawing.Point(260, 0);
            this.flowHeaderRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowHeaderRight.Name = "flowHeaderRight";
            this.flowHeaderRight.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.flowHeaderRight.Size = new System.Drawing.Size(300, 48);
            this.flowHeaderRight.TabIndex = 1;
            this.flowHeaderRight.Text = "flowHeaderRight";
            // 
            // labelDialogDescription
            // 
            this.labelDialogDescription.Location = new System.Drawing.Point(0, 0);
            this.labelDialogDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDialogDescription.Name = "labelDialogDescription";
            this.labelDialogDescription.Size = new System.Drawing.Size(300, 48);
            this.labelDialogDescription.TabIndex = 0;
            this.labelDialogDescription.Text = "配置点位地址、数据类型、长度与访问模式。";
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
            this.labelDialogTitle.Text = "新增 PLC 点位";
            // 
            // PlcPointEditDialog
            // 
            this.ClientSize = new System.Drawing.Size(620, 680);
            this.Controls.Add(this.textureBackgroundDialog);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PlcPointEditDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PLC 点位";
            this.textureBackgroundDialog.ResumeLayout(false);
            this.panelShell.ResumeLayout(false);
            this.panelContent.ResumeLayout(false);
            this.stackFormRows.ResumeLayout(false);
            this.panelRowRemark.ResumeLayout(false);
            this.panelRowDescription.ResumeLayout(false);
            this.panelRowEnabled.ResumeLayout(false);
            this.flowEnabledHost.ResumeLayout(false);
            this.flowEnabledHost.PerformLayout();
            this.panelRowSortOrder.ResumeLayout(false);
            this.panelRowAccessMode.ResumeLayout(false);
            this.panelRowLength.ResumeLayout(false);
            this.panelRowDataType.ResumeLayout(false);
            this.panelRowAddress.ResumeLayout(false);
            this.panelRowGroupName.ResumeLayout(false);
            this.panelRowDisplayName.ResumeLayout(false);
            this.panelRowName.ResumeLayout(false);
            this.panelRowPlcName.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            this.flowFooterButtons.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.flowHeaderRight.ResumeLayout(false);
            this.flowHeaderLeft.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AMControlWinF.Views.Main.TextureBackgroundControl textureBackgroundDialog;
        private AntdUI.Panel panelShell;
        private AntdUI.Panel panelHeader;
        private AntdUI.FlowPanel flowHeaderLeft;
        private AntdUI.Label labelDialogTitle;
        private AntdUI.FlowPanel flowHeaderRight;
        private AntdUI.Label labelDialogDescription;
        private AntdUI.Panel panelContent;
        private AntdUI.StackPanel stackFormRows;
        private AntdUI.Panel panelRowPlcName;
        private AntdUI.Select dropdownPlcName;
        private AntdUI.Label labelPlcName;
        private AntdUI.Panel panelRowName;
        private AntdUI.Input inputName;
        private AntdUI.Label labelName;
        private AntdUI.Panel panelRowDisplayName;
        private AntdUI.Input inputDisplayName;
        private AntdUI.Label labelDisplayName;
        private AntdUI.Panel panelRowGroupName;
        private AntdUI.Input inputGroupName;
        private AntdUI.Label labelGroupName;
        private AntdUI.Panel panelRowAddress;
        private AntdUI.Input inputAddress;
        private AntdUI.Label labelAddress;
        private AntdUI.Panel panelRowDataType;
        private AntdUI.Select dropdownDataType;
        private AntdUI.Label labelDataType;
        private AntdUI.Panel panelRowLength;
        private AntdUI.Input inputLength;
        private AntdUI.Label labelLength;
        private AntdUI.Panel panelRowAccessMode;
        private AntdUI.Select dropdownAccessMode;
        private AntdUI.Label labelAccessMode;
        private AntdUI.Panel panelRowSortOrder;
        private AntdUI.Input inputSortOrder;
        private AntdUI.Label labelSortOrder;
        private AntdUI.Panel panelRowEnabled;
        private AntdUI.FlowPanel flowEnabledHost;
        private AntdUI.Checkbox checkEnabled;
        private AntdUI.Label labelEnabled;
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