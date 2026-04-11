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
            this.checkEnabled = new AntdUI.Checkbox();
            this.labelEnabled = new AntdUI.Label();
            this.labelSectionRemark = new AntdUI.Label();
            this.stackSectionRule = new AntdUI.StackPanel();
            this.inputSortOrder = new AntdUI.Input();
            this.labelSortOrder = new AntdUI.Label();
            this.panelRowAccessMode = new AntdUI.Panel();
            this.dropdownAccessMode = new AntdUI.Select();
            this.labelAccessMode = new AntdUI.Label();
            this.inputLength = new AntdUI.Input();
            this.labelLength = new AntdUI.Label();
            this.panelRowDataType = new AntdUI.Panel();
            this.dropdownDataType = new AntdUI.Select();
            this.labelDataType = new AntdUI.Label();
            this.labelSectionRule = new AntdUI.Label();
            this.stackSectionAddress = new AntdUI.StackPanel();
            this.inputAddress = new AntdUI.Input();
            this.labelAddress = new AntdUI.Label();
            this.inputGroupName = new AntdUI.Input();
            this.labelGroupName = new AntdUI.Label();
            this.inputDisplayName = new AntdUI.Input();
            this.labelDisplayName = new AntdUI.Label();
            this.labelSectionAddress = new AntdUI.Label();
            this.stackSectionBasic = new AntdUI.StackPanel();
            this.inputName = new AntdUI.Input();
            this.labelName = new AntdUI.Label();
            this.panelRowPlcName = new AntdUI.Panel();
            this.dropdownPlcName = new AntdUI.Select();
            this.labelPlcName = new AntdUI.Label();
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
            this.stackSectionRule.SuspendLayout();
            this.panelRowAccessMode.SuspendLayout();
            this.panelRowDataType.SuspendLayout();
            this.stackSectionAddress.SuspendLayout();
            this.stackSectionBasic.SuspendLayout();
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
            this.textureBackgroundDialog.Size = new System.Drawing.Size(1000, 520);
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
            this.panelShell.Size = new System.Drawing.Size(1000, 520);
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
            this.panelContent.Size = new System.Drawing.Size(944, 348);
            this.panelContent.TabIndex = 1;
            // 
            // gridMainSections
            // 
            this.gridMainSections.Controls.Add(this.stackSectionRemark);
            this.gridMainSections.Controls.Add(this.stackSectionRule);
            this.gridMainSections.Controls.Add(this.stackSectionAddress);
            this.gridMainSections.Controls.Add(this.stackSectionBasic);
            this.gridMainSections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridMainSections.Location = new System.Drawing.Point(4, 0);
            this.gridMainSections.Margin = new System.Windows.Forms.Padding(0);
            this.gridMainSections.Name = "gridMainSections";
            this.gridMainSections.Size = new System.Drawing.Size(936, 348);
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
            this.stackSectionRemark.Controls.Add(this.checkEnabled);
            this.stackSectionRemark.Controls.Add(this.labelEnabled);
            this.stackSectionRemark.Controls.Add(this.labelSectionRemark);
            this.stackSectionRemark.Gap = 4;
            this.stackSectionRemark.Location = new System.Drawing.Point(702, 0);
            this.stackSectionRemark.Margin = new System.Windows.Forms.Padding(0);
            this.stackSectionRemark.Name = "stackSectionRemark";
            this.stackSectionRemark.Padding = new System.Windows.Forms.Padding(4);
            this.stackSectionRemark.Size = new System.Drawing.Size(234, 348);
            this.stackSectionRemark.TabIndex = 3;
            this.stackSectionRemark.Text = "stackSectionRemark";
            this.stackSectionRemark.Vertical = true;
            // 
            // inputRemark
            // 
            this.inputRemark.Location = new System.Drawing.Point(4, 242);
            this.inputRemark.Margin = new System.Windows.Forms.Padding(0);
            this.inputRemark.Multiline = true;
            this.inputRemark.Name = "inputRemark";
            this.inputRemark.PlaceholderText = "请输入备注";
            this.inputRemark.Size = new System.Drawing.Size(226, 78);
            this.inputRemark.TabIndex = 6;
            this.inputRemark.WaveSize = 0;
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(4, 216);
            this.labelRemark.Margin = new System.Windows.Forms.Padding(0);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(226, 22);
            this.labelRemark.TabIndex = 5;
            this.labelRemark.Text = "备注";
            // 
            // inputDescription
            // 
            this.inputDescription.Location = new System.Drawing.Point(4, 116);
            this.inputDescription.Margin = new System.Windows.Forms.Padding(0);
            this.inputDescription.Multiline = true;
            this.inputDescription.Name = "inputDescription";
            this.inputDescription.PlaceholderText = "请输入描述";
            this.inputDescription.Size = new System.Drawing.Size(226, 96);
            this.inputDescription.TabIndex = 4;
            this.inputDescription.WaveSize = 0;
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(4, 90);
            this.labelDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(226, 22);
            this.labelDescription.TabIndex = 3;
            this.labelDescription.Text = "描述";
            // 
            // checkEnabled
            // 
            this.checkEnabled.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkEnabled.Checked = true;
            this.checkEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkEnabled.Location = new System.Drawing.Point(4, 52);
            this.checkEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.checkEnabled.Name = "checkEnabled";
            this.checkEnabled.Size = new System.Drawing.Size(92, 34);
            this.checkEnabled.TabIndex = 2;
            this.checkEnabled.Text = "启用点位";
            // 
            // labelEnabled
            // 
            this.labelEnabled.Location = new System.Drawing.Point(4, 26);
            this.labelEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.labelEnabled.Name = "labelEnabled";
            this.labelEnabled.Size = new System.Drawing.Size(226, 22);
            this.labelEnabled.TabIndex = 1;
            this.labelEnabled.Text = "启用状态";
            // 
            // labelSectionRemark
            // 
            this.labelSectionRemark.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionRemark.Location = new System.Drawing.Point(4, 4);
            this.labelSectionRemark.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionRemark.Name = "labelSectionRemark";
            this.labelSectionRemark.Size = new System.Drawing.Size(226, 22);
            this.labelSectionRemark.TabIndex = 0;
            this.labelSectionRemark.Text = "说明与状态";
            // 
            // stackSectionRule
            // 
            this.stackSectionRule.Controls.Add(this.inputSortOrder);
            this.stackSectionRule.Controls.Add(this.labelSortOrder);
            this.stackSectionRule.Controls.Add(this.panelRowAccessMode);
            this.stackSectionRule.Controls.Add(this.inputLength);
            this.stackSectionRule.Controls.Add(this.labelLength);
            this.stackSectionRule.Controls.Add(this.panelRowDataType);
            this.stackSectionRule.Controls.Add(this.labelSectionRule);
            this.stackSectionRule.Gap = 4;
            this.stackSectionRule.Location = new System.Drawing.Point(468, 0);
            this.stackSectionRule.Margin = new System.Windows.Forms.Padding(0);
            this.stackSectionRule.Name = "stackSectionRule";
            this.stackSectionRule.Padding = new System.Windows.Forms.Padding(4);
            this.stackSectionRule.Size = new System.Drawing.Size(234, 348);
            this.stackSectionRule.TabIndex = 2;
            this.stackSectionRule.Text = "stackSectionRule";
            this.stackSectionRule.Vertical = true;
            // 
            // inputSortOrder
            // 
            this.inputSortOrder.Location = new System.Drawing.Point(4, 214);
            this.inputSortOrder.Margin = new System.Windows.Forms.Padding(0);
            this.inputSortOrder.Name = "inputSortOrder";
            this.inputSortOrder.PlaceholderText = "请输入排序号";
            this.inputSortOrder.Size = new System.Drawing.Size(226, 32);
            this.inputSortOrder.TabIndex = 6;
            this.inputSortOrder.WaveSize = 0;
            // 
            // labelSortOrder
            // 
            this.labelSortOrder.Location = new System.Drawing.Point(4, 188);
            this.labelSortOrder.Margin = new System.Windows.Forms.Padding(0);
            this.labelSortOrder.Name = "labelSortOrder";
            this.labelSortOrder.Size = new System.Drawing.Size(226, 22);
            this.labelSortOrder.TabIndex = 5;
            this.labelSortOrder.Text = "排序号";
            // 
            // panelRowAccessMode
            // 
            this.panelRowAccessMode.Controls.Add(this.dropdownAccessMode);
            this.panelRowAccessMode.Controls.Add(this.labelAccessMode);
            this.panelRowAccessMode.Location = new System.Drawing.Point(4, 132);
            this.panelRowAccessMode.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowAccessMode.Name = "panelRowAccessMode";
            this.panelRowAccessMode.Radius = 0;
            this.panelRowAccessMode.Size = new System.Drawing.Size(226, 52);
            this.panelRowAccessMode.TabIndex = 4;
            // 
            // dropdownAccessMode
            // 
            this.dropdownAccessMode.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dropdownAccessMode.Location = new System.Drawing.Point(0, 20);
            this.dropdownAccessMode.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownAccessMode.Name = "dropdownAccessMode";
            this.dropdownAccessMode.Size = new System.Drawing.Size(226, 32);
            this.dropdownAccessMode.TabIndex = 1;
            this.dropdownAccessMode.WaveSize = 0;
            // 
            // labelAccessMode
            // 
            this.labelAccessMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelAccessMode.Location = new System.Drawing.Point(0, 0);
            this.labelAccessMode.Margin = new System.Windows.Forms.Padding(0);
            this.labelAccessMode.Name = "labelAccessMode";
            this.labelAccessMode.Size = new System.Drawing.Size(226, 22);
            this.labelAccessMode.TabIndex = 0;
            this.labelAccessMode.Text = "访问模式";
            // 
            // inputLength
            // 
            this.inputLength.Location = new System.Drawing.Point(4, 96);
            this.inputLength.Margin = new System.Windows.Forms.Padding(0);
            this.inputLength.Name = "inputLength";
            this.inputLength.PlaceholderText = "请输入 Length";
            this.inputLength.Size = new System.Drawing.Size(226, 32);
            this.inputLength.TabIndex = 3;
            this.inputLength.WaveSize = 0;
            // 
            // labelLength
            // 
            this.labelLength.Location = new System.Drawing.Point(4, 70);
            this.labelLength.Margin = new System.Windows.Forms.Padding(0);
            this.labelLength.Name = "labelLength";
            this.labelLength.Size = new System.Drawing.Size(226, 22);
            this.labelLength.TabIndex = 2;
            this.labelLength.Text = "Length";
            // 
            // panelRowDataType
            // 
            this.panelRowDataType.Controls.Add(this.dropdownDataType);
            this.panelRowDataType.Controls.Add(this.labelDataType);
            this.panelRowDataType.Location = new System.Drawing.Point(4, 34);
            this.panelRowDataType.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowDataType.Name = "panelRowDataType";
            this.panelRowDataType.Radius = 0;
            this.panelRowDataType.Size = new System.Drawing.Size(226, 52);
            this.panelRowDataType.TabIndex = 1;
            // 
            // dropdownDataType
            // 
            this.dropdownDataType.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dropdownDataType.Location = new System.Drawing.Point(0, 20);
            this.dropdownDataType.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownDataType.Name = "dropdownDataType";
            this.dropdownDataType.Size = new System.Drawing.Size(226, 32);
            this.dropdownDataType.TabIndex = 1;
            this.dropdownDataType.WaveSize = 0;
            // 
            // labelDataType
            // 
            this.labelDataType.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDataType.Location = new System.Drawing.Point(0, 0);
            this.labelDataType.Margin = new System.Windows.Forms.Padding(0);
            this.labelDataType.Name = "labelDataType";
            this.labelDataType.Size = new System.Drawing.Size(226, 22);
            this.labelDataType.TabIndex = 0;
            this.labelDataType.Text = "数据类型";
            // 
            // labelSectionRule
            // 
            this.labelSectionRule.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionRule.Location = new System.Drawing.Point(4, 4);
            this.labelSectionRule.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionRule.Name = "labelSectionRule";
            this.labelSectionRule.Size = new System.Drawing.Size(226, 22);
            this.labelSectionRule.TabIndex = 0;
            this.labelSectionRule.Text = "访问规则";
            // 
            // stackSectionAddress
            // 
            this.stackSectionAddress.Controls.Add(this.inputAddress);
            this.stackSectionAddress.Controls.Add(this.labelAddress);
            this.stackSectionAddress.Controls.Add(this.inputGroupName);
            this.stackSectionAddress.Controls.Add(this.labelGroupName);
            this.stackSectionAddress.Controls.Add(this.inputDisplayName);
            this.stackSectionAddress.Controls.Add(this.labelDisplayName);
            this.stackSectionAddress.Controls.Add(this.labelSectionAddress);
            this.stackSectionAddress.Gap = 4;
            this.stackSectionAddress.Location = new System.Drawing.Point(234, 0);
            this.stackSectionAddress.Margin = new System.Windows.Forms.Padding(0);
            this.stackSectionAddress.Name = "stackSectionAddress";
            this.stackSectionAddress.Padding = new System.Windows.Forms.Padding(4);
            this.stackSectionAddress.Size = new System.Drawing.Size(234, 348);
            this.stackSectionAddress.TabIndex = 1;
            this.stackSectionAddress.Text = "stackSectionAddress";
            this.stackSectionAddress.Vertical = true;
            // 
            // inputAddress
            // 
            this.inputAddress.Location = new System.Drawing.Point(4, 158);
            this.inputAddress.Margin = new System.Windows.Forms.Padding(0);
            this.inputAddress.Name = "inputAddress";
            this.inputAddress.PlaceholderText = "请输入协议地址";
            this.inputAddress.Size = new System.Drawing.Size(226, 32);
            this.inputAddress.TabIndex = 6;
            this.inputAddress.WaveSize = 0;
            // 
            // labelAddress
            // 
            this.labelAddress.Location = new System.Drawing.Point(4, 132);
            this.labelAddress.Margin = new System.Windows.Forms.Padding(0);
            this.labelAddress.Name = "labelAddress";
            this.labelAddress.Size = new System.Drawing.Size(226, 22);
            this.labelAddress.TabIndex = 5;
            this.labelAddress.Text = "地址";
            // 
            // inputGroupName
            // 
            this.inputGroupName.Location = new System.Drawing.Point(4, 96);
            this.inputGroupName.Margin = new System.Windows.Forms.Padding(0);
            this.inputGroupName.Name = "inputGroupName";
            this.inputGroupName.PlaceholderText = "请输入分组";
            this.inputGroupName.Size = new System.Drawing.Size(226, 32);
            this.inputGroupName.TabIndex = 4;
            this.inputGroupName.WaveSize = 0;
            // 
            // labelGroupName
            // 
            this.labelGroupName.Location = new System.Drawing.Point(4, 70);
            this.labelGroupName.Margin = new System.Windows.Forms.Padding(0);
            this.labelGroupName.Name = "labelGroupName";
            this.labelGroupName.Size = new System.Drawing.Size(226, 22);
            this.labelGroupName.TabIndex = 3;
            this.labelGroupName.Text = "分组";
            // 
            // inputDisplayName
            // 
            this.inputDisplayName.Location = new System.Drawing.Point(4, 34);
            this.inputDisplayName.Margin = new System.Windows.Forms.Padding(0);
            this.inputDisplayName.Name = "inputDisplayName";
            this.inputDisplayName.PlaceholderText = "请输入显示名";
            this.inputDisplayName.Size = new System.Drawing.Size(226, 32);
            this.inputDisplayName.TabIndex = 2;
            this.inputDisplayName.WaveSize = 0;
            // 
            // labelDisplayName
            // 
            this.labelDisplayName.Location = new System.Drawing.Point(4, 8);
            this.labelDisplayName.Margin = new System.Windows.Forms.Padding(0);
            this.labelDisplayName.Name = "labelDisplayName";
            this.labelDisplayName.Size = new System.Drawing.Size(226, 22);
            this.labelDisplayName.TabIndex = 1;
            this.labelDisplayName.Text = "显示名";
            // 
            // labelSectionAddress
            // 
            this.labelSectionAddress.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionAddress.Location = new System.Drawing.Point(4, 4);
            this.labelSectionAddress.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionAddress.Name = "labelSectionAddress";
            this.labelSectionAddress.Size = new System.Drawing.Size(226, 22);
            this.labelSectionAddress.TabIndex = 0;
            this.labelSectionAddress.Text = "点位信息";
            // 
            // stackSectionBasic
            // 
            this.stackSectionBasic.Controls.Add(this.inputName);
            this.stackSectionBasic.Controls.Add(this.labelName);
            this.stackSectionBasic.Controls.Add(this.panelRowPlcName);
            this.stackSectionBasic.Controls.Add(this.labelSectionBasic);
            this.stackSectionBasic.Gap = 4;
            this.stackSectionBasic.Location = new System.Drawing.Point(0, 0);
            this.stackSectionBasic.Margin = new System.Windows.Forms.Padding(0);
            this.stackSectionBasic.Name = "stackSectionBasic";
            this.stackSectionBasic.Padding = new System.Windows.Forms.Padding(4);
            this.stackSectionBasic.Size = new System.Drawing.Size(234, 348);
            this.stackSectionBasic.TabIndex = 0;
            this.stackSectionBasic.Text = "stackSectionBasic";
            this.stackSectionBasic.Vertical = true;
            // 
            // inputName
            // 
            this.inputName.Location = new System.Drawing.Point(4, 96);
            this.inputName.Margin = new System.Windows.Forms.Padding(0);
            this.inputName.Name = "inputName";
            this.inputName.PlaceholderText = "请输入点位名称";
            this.inputName.Size = new System.Drawing.Size(226, 32);
            this.inputName.TabIndex = 3;
            this.inputName.WaveSize = 0;
            // 
            // labelName
            // 
            this.labelName.Location = new System.Drawing.Point(4, 70);
            this.labelName.Margin = new System.Windows.Forms.Padding(0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(226, 22);
            this.labelName.TabIndex = 2;
            this.labelName.Text = "名称";
            // 
            // panelRowPlcName
            // 
            this.panelRowPlcName.Controls.Add(this.dropdownPlcName);
            this.panelRowPlcName.Controls.Add(this.labelPlcName);
            this.panelRowPlcName.Location = new System.Drawing.Point(4, 34);
            this.panelRowPlcName.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowPlcName.Name = "panelRowPlcName";
            this.panelRowPlcName.Radius = 0;
            this.panelRowPlcName.Size = new System.Drawing.Size(226, 52);
            this.panelRowPlcName.TabIndex = 1;
            // 
            // dropdownPlcName
            // 
            this.dropdownPlcName.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dropdownPlcName.Location = new System.Drawing.Point(0, 20);
            this.dropdownPlcName.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownPlcName.Name = "dropdownPlcName";
            this.dropdownPlcName.Size = new System.Drawing.Size(226, 32);
            this.dropdownPlcName.TabIndex = 1;
            this.dropdownPlcName.WaveSize = 0;
            // 
            // labelPlcName
            // 
            this.labelPlcName.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelPlcName.Location = new System.Drawing.Point(0, 0);
            this.labelPlcName.Margin = new System.Windows.Forms.Padding(0);
            this.labelPlcName.Name = "labelPlcName";
            this.labelPlcName.Size = new System.Drawing.Size(226, 22);
            this.labelPlcName.TabIndex = 0;
            this.labelPlcName.Text = "所属PLC";
            // 
            // labelSectionBasic
            // 
            this.labelSectionBasic.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionBasic.Location = new System.Drawing.Point(4, 4);
            this.labelSectionBasic.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionBasic.Name = "labelSectionBasic";
            this.labelSectionBasic.Size = new System.Drawing.Size(226, 22);
            this.labelSectionBasic.TabIndex = 0;
            this.labelSectionBasic.Text = "基础信息";
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.flowFooterButtons);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(28, 432);
            this.panelFooter.Margin = new System.Windows.Forms.Padding(0);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Padding = new System.Windows.Forms.Padding(4, 10, 4, 0);
            this.panelFooter.Radius = 0;
            this.panelFooter.Size = new System.Drawing.Size(944, 60);
            this.panelFooter.TabIndex = 2;
            // 
            // flowFooterButtons
            // 
            this.flowFooterButtons.Controls.Add(this.buttonOk);
            this.flowFooterButtons.Controls.Add(this.buttonCancel);
            this.flowFooterButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowFooterButtons.Gap = 10;
            this.flowFooterButtons.Location = new System.Drawing.Point(696, 10);
            this.flowFooterButtons.Margin = new System.Windows.Forms.Padding(0);
            this.flowFooterButtons.Name = "flowFooterButtons";
            this.flowFooterButtons.Size = new System.Drawing.Size(244, 50);
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
            this.panelHeader.Size = new System.Drawing.Size(944, 56);
            this.panelHeader.TabIndex = 0;
            // 
            // flowHeaderRight
            // 
            this.flowHeaderRight.Controls.Add(this.labelDialogDescription);
            this.flowHeaderRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowHeaderRight.Location = new System.Drawing.Point(596, 0);
            this.flowHeaderRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowHeaderRight.Name = "flowHeaderRight";
            this.flowHeaderRight.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.flowHeaderRight.Size = new System.Drawing.Size(344, 48);
            this.flowHeaderRight.TabIndex = 1;
            this.flowHeaderRight.Text = "flowHeaderRight";
            // 
            // labelDialogDescription
            // 
            this.labelDialogDescription.Location = new System.Drawing.Point(0, 0);
            this.labelDialogDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDialogDescription.Name = "labelDialogDescription";
            this.labelDialogDescription.Size = new System.Drawing.Size(344, 48);
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
            this.labelDialogTitle.Text = "新增 PLC 点位";
            // 
            // PlcPointEditDialog
            // 
            this.ClientSize = new System.Drawing.Size(1000, 520);
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
            this.gridMainSections.ResumeLayout(false);
            this.stackSectionRemark.ResumeLayout(false);
            this.stackSectionRule.ResumeLayout(false);
            this.panelRowAccessMode.ResumeLayout(false);
            this.panelRowDataType.ResumeLayout(false);
            this.stackSectionAddress.ResumeLayout(false);
            this.stackSectionBasic.ResumeLayout(false);
            this.panelRowPlcName.ResumeLayout(false);
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
        private AntdUI.Panel panelRowPlcName;
        private AntdUI.Select dropdownPlcName;
        private AntdUI.Label labelPlcName;
        private AntdUI.Input inputName;
        private AntdUI.Label labelName;
        private AntdUI.StackPanel stackSectionAddress;
        private AntdUI.Label labelSectionAddress;
        private AntdUI.Input inputDisplayName;
        private AntdUI.Label labelDisplayName;
        private AntdUI.Input inputGroupName;
        private AntdUI.Label labelGroupName;
        private AntdUI.Input inputAddress;
        private AntdUI.Label labelAddress;
        private AntdUI.StackPanel stackSectionRule;
        private AntdUI.Label labelSectionRule;
        private AntdUI.Panel panelRowDataType;
        private AntdUI.Select dropdownDataType;
        private AntdUI.Label labelDataType;
        private AntdUI.Input inputLength;
        private AntdUI.Label labelLength;
        private AntdUI.Panel panelRowAccessMode;
        private AntdUI.Select dropdownAccessMode;
        private AntdUI.Label labelAccessMode;
        private AntdUI.Input inputSortOrder;
        private AntdUI.Label labelSortOrder;
        private AntdUI.StackPanel stackSectionRemark;
        private AntdUI.Label labelSectionRemark;
        private AntdUI.Label labelEnabled;
        private AntdUI.Checkbox checkEnabled;
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