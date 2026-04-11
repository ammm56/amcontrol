namespace AMControlWinF.Views.MotionConfig
{
    partial class MotionAxisParamEditDialog
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
            this.stackSectionRuntime = new AntdUI.StackPanel();
            this.inputVendorScope = new AntdUI.Input();
            this.labelVendorScope = new AntdUI.Label();
            this.inputValueDescription = new AntdUI.Input();
            this.labelValueDescription = new AntdUI.Label();
            this.inputUnit = new AntdUI.Input();
            this.labelUnit = new AntdUI.Label();
            this.labelSectionRuntime = new AntdUI.Label();
            this.stackSectionValue = new AntdUI.StackPanel();
            this.inputParamMaxValue = new AntdUI.Input();
            this.labelParamMaxValue = new AntdUI.Label();
            this.inputParamMinValue = new AntdUI.Input();
            this.labelParamMinValue = new AntdUI.Label();
            this.inputParamDefaultValue = new AntdUI.Input();
            this.labelParamDefaultValue = new AntdUI.Label();
            this.inputParamSetValue = new AntdUI.Input();
            this.labelParamSetValue = new AntdUI.Label();
            this.labelSectionValue = new AntdUI.Label();
            this.stackSectionBasic = new AntdUI.StackPanel();
            this.dropdownParamValueType = new AntdUI.Select();
            this.labelParamValueType = new AntdUI.Label();
            this.dropdownParamGroup = new AntdUI.Select();
            this.labelParamGroup = new AntdUI.Label();
            this.inputParamDisplayName = new AntdUI.Input();
            this.labelParamDisplayName = new AntdUI.Label();
            this.inputParamName = new AntdUI.Input();
            this.labelParamName = new AntdUI.Label();
            this.inputLogicalAxis = new AntdUI.Input();
            this.labelLogicalAxis = new AntdUI.Label();
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
            this.stackSectionValue.SuspendLayout();
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
            this.textureBackgroundDialog.Size = new System.Drawing.Size(960, 600);
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
            this.panelShell.Size = new System.Drawing.Size(960, 600);
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
            this.panelContent.Size = new System.Drawing.Size(904, 431);
            this.panelContent.TabIndex = 1;
            // 
            // gridMainSections
            // 
            this.gridMainSections.Controls.Add(this.stackSectionRemark);
            this.gridMainSections.Controls.Add(this.stackSectionRuntime);
            this.gridMainSections.Controls.Add(this.stackSectionValue);
            this.gridMainSections.Controls.Add(this.stackSectionBasic);
            this.gridMainSections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridMainSections.Location = new System.Drawing.Point(4, 0);
            this.gridMainSections.Margin = new System.Windows.Forms.Padding(0);
            this.gridMainSections.Name = "gridMainSections";
            this.gridMainSections.Size = new System.Drawing.Size(896, 431);
            this.gridMainSections.Span = "28% 22% 22% 28%";
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
            this.stackSectionRemark.Location = new System.Drawing.Point(645, 0);
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
            this.inputRemark.Location = new System.Drawing.Point(4, 182);
            this.inputRemark.Margin = new System.Windows.Forms.Padding(0);
            this.inputRemark.Multiline = true;
            this.inputRemark.Name = "inputRemark";
            this.inputRemark.PlaceholderText = "请输入备注";
            this.inputRemark.Size = new System.Drawing.Size(243, 90);
            this.inputRemark.TabIndex = 4;
            this.inputRemark.WaveSize = 0;
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(4, 156);
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
            this.inputDescription.PlaceholderText = "请输入参数说明";
            this.inputDescription.Size = new System.Drawing.Size(243, 92);
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
            this.labelDescription.Text = "参数说明";
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
            // stackSectionRuntime
            // 
            this.stackSectionRuntime.AutoScroll = true;
            this.stackSectionRuntime.Controls.Add(this.inputVendorScope);
            this.stackSectionRuntime.Controls.Add(this.labelVendorScope);
            this.stackSectionRuntime.Controls.Add(this.inputValueDescription);
            this.stackSectionRuntime.Controls.Add(this.labelValueDescription);
            this.stackSectionRuntime.Controls.Add(this.inputUnit);
            this.stackSectionRuntime.Controls.Add(this.labelUnit);
            this.stackSectionRuntime.Controls.Add(this.labelSectionRuntime);
            this.stackSectionRuntime.Gap = 4;
            this.stackSectionRuntime.Location = new System.Drawing.Point(448, 0);
            this.stackSectionRuntime.Margin = new System.Windows.Forms.Padding(0);
            this.stackSectionRuntime.Name = "stackSectionRuntime";
            this.stackSectionRuntime.Padding = new System.Windows.Forms.Padding(4);
            this.stackSectionRuntime.Size = new System.Drawing.Size(197, 431);
            this.stackSectionRuntime.TabIndex = 2;
            this.stackSectionRuntime.Text = "stackSectionRuntime";
            this.stackSectionRuntime.Vertical = true;
            // 
            // inputVendorScope
            // 
            this.inputVendorScope.Location = new System.Drawing.Point(4, 244);
            this.inputVendorScope.Margin = new System.Windows.Forms.Padding(0);
            this.inputVendorScope.Name = "inputVendorScope";
            this.inputVendorScope.PlaceholderText = "如 All / GOOGO / LEISAI";
            this.inputVendorScope.Size = new System.Drawing.Size(189, 32);
            this.inputVendorScope.TabIndex = 6;
            this.inputVendorScope.WaveSize = 0;
            // 
            // labelVendorScope
            // 
            this.labelVendorScope.Location = new System.Drawing.Point(4, 218);
            this.labelVendorScope.Margin = new System.Windows.Forms.Padding(0);
            this.labelVendorScope.Name = "labelVendorScope";
            this.labelVendorScope.Size = new System.Drawing.Size(189, 22);
            this.labelVendorScope.TabIndex = 5;
            this.labelVendorScope.Text = "厂商范围";
            // 
            // inputValueDescription
            // 
            this.inputValueDescription.Location = new System.Drawing.Point(4, 122);
            this.inputValueDescription.Margin = new System.Windows.Forms.Padding(0);
            this.inputValueDescription.Multiline = true;
            this.inputValueDescription.Name = "inputValueDescription";
            this.inputValueDescription.PlaceholderText = "请输入取值说明";
            this.inputValueDescription.Size = new System.Drawing.Size(189, 92);
            this.inputValueDescription.TabIndex = 4;
            this.inputValueDescription.WaveSize = 0;
            // 
            // labelValueDescription
            // 
            this.labelValueDescription.Location = new System.Drawing.Point(4, 96);
            this.labelValueDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueDescription.Name = "labelValueDescription";
            this.labelValueDescription.Size = new System.Drawing.Size(189, 22);
            this.labelValueDescription.TabIndex = 3;
            this.labelValueDescription.Text = "取值说明";
            // 
            // inputUnit
            // 
            this.inputUnit.Location = new System.Drawing.Point(4, 60);
            this.inputUnit.Margin = new System.Windows.Forms.Padding(0);
            this.inputUnit.Name = "inputUnit";
            this.inputUnit.PlaceholderText = "如 mm / pulse / ms";
            this.inputUnit.Size = new System.Drawing.Size(189, 32);
            this.inputUnit.TabIndex = 2;
            this.inputUnit.WaveSize = 0;
            // 
            // labelUnit
            // 
            this.labelUnit.Location = new System.Drawing.Point(4, 34);
            this.labelUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(189, 22);
            this.labelUnit.TabIndex = 1;
            this.labelUnit.Text = "参数单位";
            // 
            // labelSectionRuntime
            // 
            this.labelSectionRuntime.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionRuntime.Location = new System.Drawing.Point(4, 4);
            this.labelSectionRuntime.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionRuntime.Name = "labelSectionRuntime";
            this.labelSectionRuntime.Size = new System.Drawing.Size(189, 26);
            this.labelSectionRuntime.TabIndex = 0;
            this.labelSectionRuntime.Text = "扩展信息";
            // 
            // stackSectionValue
            // 
            this.stackSectionValue.AutoScroll = true;
            this.stackSectionValue.Controls.Add(this.inputParamMaxValue);
            this.stackSectionValue.Controls.Add(this.labelParamMaxValue);
            this.stackSectionValue.Controls.Add(this.inputParamMinValue);
            this.stackSectionValue.Controls.Add(this.labelParamMinValue);
            this.stackSectionValue.Controls.Add(this.inputParamDefaultValue);
            this.stackSectionValue.Controls.Add(this.labelParamDefaultValue);
            this.stackSectionValue.Controls.Add(this.inputParamSetValue);
            this.stackSectionValue.Controls.Add(this.labelParamSetValue);
            this.stackSectionValue.Controls.Add(this.labelSectionValue);
            this.stackSectionValue.Gap = 4;
            this.stackSectionValue.Location = new System.Drawing.Point(251, 0);
            this.stackSectionValue.Margin = new System.Windows.Forms.Padding(0);
            this.stackSectionValue.Name = "stackSectionValue";
            this.stackSectionValue.Padding = new System.Windows.Forms.Padding(4);
            this.stackSectionValue.Size = new System.Drawing.Size(197, 431);
            this.stackSectionValue.TabIndex = 1;
            this.stackSectionValue.Text = "stackSectionValue";
            this.stackSectionValue.Vertical = true;
            // 
            // inputParamMaxValue
            // 
            this.inputParamMaxValue.Location = new System.Drawing.Point(4, 246);
            this.inputParamMaxValue.Margin = new System.Windows.Forms.Padding(0);
            this.inputParamMaxValue.Name = "inputParamMaxValue";
            this.inputParamMaxValue.PlaceholderText = "请输入最大值";
            this.inputParamMaxValue.Size = new System.Drawing.Size(189, 32);
            this.inputParamMaxValue.TabIndex = 8;
            this.inputParamMaxValue.WaveSize = 0;
            // 
            // labelParamMaxValue
            // 
            this.labelParamMaxValue.Location = new System.Drawing.Point(4, 220);
            this.labelParamMaxValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelParamMaxValue.Name = "labelParamMaxValue";
            this.labelParamMaxValue.Size = new System.Drawing.Size(189, 22);
            this.labelParamMaxValue.TabIndex = 7;
            this.labelParamMaxValue.Text = "最大值";
            // 
            // inputParamMinValue
            // 
            this.inputParamMinValue.Location = new System.Drawing.Point(4, 184);
            this.inputParamMinValue.Margin = new System.Windows.Forms.Padding(0);
            this.inputParamMinValue.Name = "inputParamMinValue";
            this.inputParamMinValue.PlaceholderText = "请输入最小值";
            this.inputParamMinValue.Size = new System.Drawing.Size(189, 32);
            this.inputParamMinValue.TabIndex = 6;
            this.inputParamMinValue.WaveSize = 0;
            // 
            // labelParamMinValue
            // 
            this.labelParamMinValue.Location = new System.Drawing.Point(4, 158);
            this.labelParamMinValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelParamMinValue.Name = "labelParamMinValue";
            this.labelParamMinValue.Size = new System.Drawing.Size(189, 22);
            this.labelParamMinValue.TabIndex = 5;
            this.labelParamMinValue.Text = "最小值";
            // 
            // inputParamDefaultValue
            // 
            this.inputParamDefaultValue.Location = new System.Drawing.Point(4, 122);
            this.inputParamDefaultValue.Margin = new System.Windows.Forms.Padding(0);
            this.inputParamDefaultValue.Name = "inputParamDefaultValue";
            this.inputParamDefaultValue.PlaceholderText = "请输入默认值";
            this.inputParamDefaultValue.Size = new System.Drawing.Size(189, 32);
            this.inputParamDefaultValue.TabIndex = 4;
            this.inputParamDefaultValue.WaveSize = 0;
            // 
            // labelParamDefaultValue
            // 
            this.labelParamDefaultValue.Location = new System.Drawing.Point(4, 96);
            this.labelParamDefaultValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelParamDefaultValue.Name = "labelParamDefaultValue";
            this.labelParamDefaultValue.Size = new System.Drawing.Size(189, 22);
            this.labelParamDefaultValue.TabIndex = 3;
            this.labelParamDefaultValue.Text = "默认值";
            // 
            // inputParamSetValue
            // 
            this.inputParamSetValue.Location = new System.Drawing.Point(4, 60);
            this.inputParamSetValue.Margin = new System.Windows.Forms.Padding(0);
            this.inputParamSetValue.Name = "inputParamSetValue";
            this.inputParamSetValue.PlaceholderText = "请输入当前值";
            this.inputParamSetValue.Size = new System.Drawing.Size(189, 32);
            this.inputParamSetValue.TabIndex = 2;
            this.inputParamSetValue.WaveSize = 0;
            // 
            // labelParamSetValue
            // 
            this.labelParamSetValue.Location = new System.Drawing.Point(4, 34);
            this.labelParamSetValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelParamSetValue.Name = "labelParamSetValue";
            this.labelParamSetValue.Size = new System.Drawing.Size(189, 22);
            this.labelParamSetValue.TabIndex = 1;
            this.labelParamSetValue.Text = "当前值";
            // 
            // labelSectionValue
            // 
            this.labelSectionValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionValue.Location = new System.Drawing.Point(4, 4);
            this.labelSectionValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionValue.Name = "labelSectionValue";
            this.labelSectionValue.Size = new System.Drawing.Size(189, 26);
            this.labelSectionValue.TabIndex = 0;
            this.labelSectionValue.Text = "参数值";
            // 
            // stackSectionBasic
            // 
            this.stackSectionBasic.AutoScroll = true;
            this.stackSectionBasic.Controls.Add(this.dropdownParamValueType);
            this.stackSectionBasic.Controls.Add(this.labelParamValueType);
            this.stackSectionBasic.Controls.Add(this.dropdownParamGroup);
            this.stackSectionBasic.Controls.Add(this.labelParamGroup);
            this.stackSectionBasic.Controls.Add(this.inputParamDisplayName);
            this.stackSectionBasic.Controls.Add(this.labelParamDisplayName);
            this.stackSectionBasic.Controls.Add(this.inputParamName);
            this.stackSectionBasic.Controls.Add(this.labelParamName);
            this.stackSectionBasic.Controls.Add(this.inputLogicalAxis);
            this.stackSectionBasic.Controls.Add(this.labelLogicalAxis);
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
            // dropdownParamValueType
            // 
            this.dropdownParamValueType.Location = new System.Drawing.Point(4, 308);
            this.dropdownParamValueType.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownParamValueType.Name = "dropdownParamValueType";
            this.dropdownParamValueType.Size = new System.Drawing.Size(243, 32);
            this.dropdownParamValueType.TabIndex = 10;
            this.dropdownParamValueType.WaveSize = 0;
            // 
            // labelParamValueType
            // 
            this.labelParamValueType.Location = new System.Drawing.Point(4, 282);
            this.labelParamValueType.Margin = new System.Windows.Forms.Padding(0);
            this.labelParamValueType.Name = "labelParamValueType";
            this.labelParamValueType.Size = new System.Drawing.Size(243, 22);
            this.labelParamValueType.TabIndex = 9;
            this.labelParamValueType.Text = "参数值类型";
            // 
            // dropdownParamGroup
            // 
            this.dropdownParamGroup.Location = new System.Drawing.Point(4, 246);
            this.dropdownParamGroup.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownParamGroup.Name = "dropdownParamGroup";
            this.dropdownParamGroup.Size = new System.Drawing.Size(243, 32);
            this.dropdownParamGroup.TabIndex = 8;
            this.dropdownParamGroup.WaveSize = 0;
            // 
            // labelParamGroup
            // 
            this.labelParamGroup.Location = new System.Drawing.Point(4, 220);
            this.labelParamGroup.Margin = new System.Windows.Forms.Padding(0);
            this.labelParamGroup.Name = "labelParamGroup";
            this.labelParamGroup.Size = new System.Drawing.Size(243, 22);
            this.labelParamGroup.TabIndex = 7;
            this.labelParamGroup.Text = "参数分组";
            // 
            // inputParamDisplayName
            // 
            this.inputParamDisplayName.Location = new System.Drawing.Point(4, 184);
            this.inputParamDisplayName.Margin = new System.Windows.Forms.Padding(0);
            this.inputParamDisplayName.Name = "inputParamDisplayName";
            this.inputParamDisplayName.PlaceholderText = "请输入参数显示名称";
            this.inputParamDisplayName.Size = new System.Drawing.Size(243, 32);
            this.inputParamDisplayName.TabIndex = 6;
            this.inputParamDisplayName.WaveSize = 0;
            // 
            // labelParamDisplayName
            // 
            this.labelParamDisplayName.Location = new System.Drawing.Point(4, 158);
            this.labelParamDisplayName.Margin = new System.Windows.Forms.Padding(0);
            this.labelParamDisplayName.Name = "labelParamDisplayName";
            this.labelParamDisplayName.Size = new System.Drawing.Size(243, 22);
            this.labelParamDisplayName.TabIndex = 5;
            this.labelParamDisplayName.Text = "参数显示名称";
            // 
            // inputParamName
            // 
            this.inputParamName.Location = new System.Drawing.Point(4, 122);
            this.inputParamName.Margin = new System.Windows.Forms.Padding(0);
            this.inputParamName.Name = "inputParamName";
            this.inputParamName.PlaceholderText = "请输入参数名";
            this.inputParamName.Size = new System.Drawing.Size(243, 32);
            this.inputParamName.TabIndex = 4;
            this.inputParamName.WaveSize = 0;
            // 
            // labelParamName
            // 
            this.labelParamName.Location = new System.Drawing.Point(4, 96);
            this.labelParamName.Margin = new System.Windows.Forms.Padding(0);
            this.labelParamName.Name = "labelParamName";
            this.labelParamName.Size = new System.Drawing.Size(243, 22);
            this.labelParamName.TabIndex = 3;
            this.labelParamName.Text = "参数名";
            // 
            // inputLogicalAxis
            // 
            this.inputLogicalAxis.Location = new System.Drawing.Point(4, 60);
            this.inputLogicalAxis.Margin = new System.Windows.Forms.Padding(0);
            this.inputLogicalAxis.Name = "inputLogicalAxis";
            this.inputLogicalAxis.PlaceholderText = "逻辑轴号";
            this.inputLogicalAxis.Size = new System.Drawing.Size(243, 32);
            this.inputLogicalAxis.TabIndex = 2;
            this.inputLogicalAxis.WaveSize = 0;
            // 
            // labelLogicalAxis
            // 
            this.labelLogicalAxis.Location = new System.Drawing.Point(4, 34);
            this.labelLogicalAxis.Margin = new System.Windows.Forms.Padding(0);
            this.labelLogicalAxis.Name = "labelLogicalAxis";
            this.labelLogicalAxis.Size = new System.Drawing.Size(243, 22);
            this.labelLogicalAxis.TabIndex = 1;
            this.labelLogicalAxis.Text = "逻辑轴号";
            // 
            // labelSectionBasic
            // 
            this.labelSectionBasic.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionBasic.Location = new System.Drawing.Point(4, 4);
            this.labelSectionBasic.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionBasic.Name = "labelSectionBasic";
            this.labelSectionBasic.Size = new System.Drawing.Size(243, 26);
            this.labelSectionBasic.TabIndex = 0;
            this.labelSectionBasic.Text = "基础信息";
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
            this.panelFooter.Size = new System.Drawing.Size(904, 57);
            this.panelFooter.TabIndex = 2;
            // 
            // flowFooterButtons
            // 
            this.flowFooterButtons.Controls.Add(this.buttonOk);
            this.flowFooterButtons.Controls.Add(this.buttonCancel);
            this.flowFooterButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowFooterButtons.Gap = 10;
            this.flowFooterButtons.Location = new System.Drawing.Point(656, 10);
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
            this.panelHeader.Size = new System.Drawing.Size(904, 56);
            this.panelHeader.TabIndex = 0;
            // 
            // labelDialogDescription
            // 
            this.labelDialogDescription.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelDialogDescription.Location = new System.Drawing.Point(462, 0);
            this.labelDialogDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDialogDescription.Name = "labelDialogDescription";
            this.labelDialogDescription.Size = new System.Drawing.Size(438, 48);
            this.labelDialogDescription.TabIndex = 1;
            this.labelDialogDescription.Text = "填写轴参数配置";
            this.labelDialogDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelDialogTitle
            // 
            this.labelDialogTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelDialogTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelDialogTitle.Location = new System.Drawing.Point(4, 0);
            this.labelDialogTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelDialogTitle.Name = "labelDialogTitle";
            this.labelDialogTitle.Size = new System.Drawing.Size(458, 48);
            this.labelDialogTitle.TabIndex = 0;
            this.labelDialogTitle.Text = "新增轴参数";
            // 
            // MotionAxisParamEditDialog
            // 
            this.ClientSize = new System.Drawing.Size(1200, 750);
            this.Controls.Add(this.textureBackgroundDialog);
            this.Name = "MotionAxisParamEditDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "轴参数编辑";
            this.textureBackgroundDialog.ResumeLayout(false);
            this.panelShell.ResumeLayout(false);
            this.panelContent.ResumeLayout(false);
            this.gridMainSections.ResumeLayout(false);
            this.stackSectionRemark.ResumeLayout(false);
            this.stackSectionRuntime.ResumeLayout(false);
            this.stackSectionValue.ResumeLayout(false);
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
        private AntdUI.Label labelLogicalAxis;
        private AntdUI.Input inputLogicalAxis;
        private AntdUI.Label labelParamName;
        private AntdUI.Input inputParamName;
        private AntdUI.Label labelParamDisplayName;
        private AntdUI.Input inputParamDisplayName;
        private AntdUI.Label labelParamGroup;
        private AntdUI.Select dropdownParamGroup;
        private AntdUI.Label labelParamValueType;
        private AntdUI.Select dropdownParamValueType;

        private AntdUI.StackPanel stackSectionValue;
        private AntdUI.Label labelSectionValue;
        private AntdUI.Label labelParamSetValue;
        private AntdUI.Input inputParamSetValue;
        private AntdUI.Label labelParamDefaultValue;
        private AntdUI.Input inputParamDefaultValue;
        private AntdUI.Label labelParamMinValue;
        private AntdUI.Input inputParamMinValue;
        private AntdUI.Label labelParamMaxValue;
        private AntdUI.Input inputParamMaxValue;

        private AntdUI.StackPanel stackSectionRuntime;
        private AntdUI.Label labelSectionRuntime;
        private AntdUI.Label labelUnit;
        private AntdUI.Input inputUnit;
        private AntdUI.Label labelValueDescription;
        private AntdUI.Input inputValueDescription;
        private AntdUI.Label labelVendorScope;
        private AntdUI.Input inputVendorScope;

        private AntdUI.StackPanel stackSectionRemark;
        private AntdUI.Label labelSectionRemark;
        private AntdUI.Label labelDescription;
        private AntdUI.Input inputDescription;
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