namespace AMControlWinF.Views.MotionConfig
{
    partial class MotionAxisEditDialog
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
            this.inputSortOrder = new AntdUI.Input();
            this.labelSortOrder = new AntdUI.Label();
            this.checkIsEnabled = new AntdUI.Checkbox();
            this.labelEnabled = new AntdUI.Label();
            this.dropdownPhysicalAxis = new AntdUI.Select();
            this.labelPhysicalAxis = new AntdUI.Label();
            this.dropdownPhysicalCore = new AntdUI.Select();
            this.labelPhysicalCore = new AntdUI.Label();
            this.labelSectionRuntime = new AntdUI.Label();
            this.stackSectionMapping = new AntdUI.StackPanel();
            this.dropdownAxisId = new AntdUI.Select();
            this.labelAxisId = new AntdUI.Label();
            this.dropdownCardId = new AntdUI.Select();
            this.labelCardId = new AntdUI.Label();
            this.dropdownAxisCategory = new AntdUI.Select();
            this.labelAxisCategory = new AntdUI.Label();
            this.labelSectionMapping = new AntdUI.Label();
            this.stackSectionBasic = new AntdUI.StackPanel();
            this.inputDisplayName = new AntdUI.Input();
            this.labelDisplayName = new AntdUI.Label();
            this.inputName = new AntdUI.Input();
            this.labelName = new AntdUI.Label();
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
            this.gridMainSections.Controls.Add(this.stackSectionRuntime);
            this.gridMainSections.Controls.Add(this.stackSectionMapping);
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
            this.inputRemark.Size = new System.Drawing.Size(243, 70);
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
            // stackSectionRuntime
            // 
            this.stackSectionRuntime.AutoScroll = true;
            this.stackSectionRuntime.Controls.Add(this.inputSortOrder);
            this.stackSectionRuntime.Controls.Add(this.labelSortOrder);
            this.stackSectionRuntime.Controls.Add(this.checkIsEnabled);
            this.stackSectionRuntime.Controls.Add(this.labelEnabled);
            this.stackSectionRuntime.Controls.Add(this.dropdownPhysicalAxis);
            this.stackSectionRuntime.Controls.Add(this.labelPhysicalAxis);
            this.stackSectionRuntime.Controls.Add(this.dropdownPhysicalCore);
            this.stackSectionRuntime.Controls.Add(this.labelPhysicalCore);
            this.stackSectionRuntime.Controls.Add(this.labelSectionRuntime);
            this.stackSectionRuntime.Gap = 4;
            this.stackSectionRuntime.Location = new System.Drawing.Point(418, 0);
            this.stackSectionRuntime.Margin = new System.Windows.Forms.Padding(0);
            this.stackSectionRuntime.Name = "stackSectionRuntime";
            this.stackSectionRuntime.Padding = new System.Windows.Forms.Padding(4);
            this.stackSectionRuntime.Size = new System.Drawing.Size(167, 431);
            this.stackSectionRuntime.TabIndex = 2;
            this.stackSectionRuntime.Text = "stackSectionRuntime";
            this.stackSectionRuntime.Vertical = true;
            // 
            // inputSortOrder
            // 
            this.inputSortOrder.Location = new System.Drawing.Point(4, 242);
            this.inputSortOrder.Margin = new System.Windows.Forms.Padding(0);
            this.inputSortOrder.Name = "inputSortOrder";
            this.inputSortOrder.PlaceholderText = "请输入排序号";
            this.inputSortOrder.Size = new System.Drawing.Size(159, 32);
            this.inputSortOrder.TabIndex = 8;
            this.inputSortOrder.WaveSize = 0;
            // 
            // labelSortOrder
            // 
            this.labelSortOrder.Location = new System.Drawing.Point(4, 216);
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
            this.checkIsEnabled.Location = new System.Drawing.Point(4, 184);
            this.checkIsEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.checkIsEnabled.Name = "checkIsEnabled";
            this.checkIsEnabled.Size = new System.Drawing.Size(76, 28);
            this.checkIsEnabled.TabIndex = 6;
            this.checkIsEnabled.Text = "启用此轴";
            // 
            // labelEnabled
            // 
            this.labelEnabled.Location = new System.Drawing.Point(4, 158);
            this.labelEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.labelEnabled.Name = "labelEnabled";
            this.labelEnabled.Size = new System.Drawing.Size(159, 22);
            this.labelEnabled.TabIndex = 5;
            this.labelEnabled.Text = "启用状态";
            // 
            // dropdownPhysicalAxis
            // 
            this.dropdownPhysicalAxis.Location = new System.Drawing.Point(4, 122);
            this.dropdownPhysicalAxis.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownPhysicalAxis.Name = "dropdownPhysicalAxis";
            this.dropdownPhysicalAxis.Size = new System.Drawing.Size(159, 32);
            this.dropdownPhysicalAxis.TabIndex = 4;
            this.dropdownPhysicalAxis.WaveSize = 0;
            // 
            // labelPhysicalAxis
            // 
            this.labelPhysicalAxis.Location = new System.Drawing.Point(4, 96);
            this.labelPhysicalAxis.Margin = new System.Windows.Forms.Padding(0);
            this.labelPhysicalAxis.Name = "labelPhysicalAxis";
            this.labelPhysicalAxis.Size = new System.Drawing.Size(159, 22);
            this.labelPhysicalAxis.TabIndex = 3;
            this.labelPhysicalAxis.Text = "物理轴号";
            // 
            // dropdownPhysicalCore
            // 
            this.dropdownPhysicalCore.Location = new System.Drawing.Point(4, 60);
            this.dropdownPhysicalCore.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownPhysicalCore.Name = "dropdownPhysicalCore";
            this.dropdownPhysicalCore.Size = new System.Drawing.Size(159, 32);
            this.dropdownPhysicalCore.TabIndex = 2;
            this.dropdownPhysicalCore.WaveSize = 0;
            // 
            // labelPhysicalCore
            // 
            this.labelPhysicalCore.Location = new System.Drawing.Point(4, 34);
            this.labelPhysicalCore.Margin = new System.Windows.Forms.Padding(0);
            this.labelPhysicalCore.Name = "labelPhysicalCore";
            this.labelPhysicalCore.Size = new System.Drawing.Size(159, 22);
            this.labelPhysicalCore.TabIndex = 1;
            this.labelPhysicalCore.Text = "物理内核号";
            // 
            // labelSectionRuntime
            // 
            this.labelSectionRuntime.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionRuntime.Location = new System.Drawing.Point(4, 4);
            this.labelSectionRuntime.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionRuntime.Name = "labelSectionRuntime";
            this.labelSectionRuntime.Size = new System.Drawing.Size(159, 26);
            this.labelSectionRuntime.TabIndex = 0;
            this.labelSectionRuntime.Text = "运行与排序";
            // 
            // stackSectionMapping
            // 
            this.stackSectionMapping.AutoScroll = true;
            this.stackSectionMapping.Controls.Add(this.dropdownAxisId);
            this.stackSectionMapping.Controls.Add(this.labelAxisId);
            this.stackSectionMapping.Controls.Add(this.dropdownCardId);
            this.stackSectionMapping.Controls.Add(this.labelCardId);
            this.stackSectionMapping.Controls.Add(this.dropdownAxisCategory);
            this.stackSectionMapping.Controls.Add(this.labelAxisCategory);
            this.stackSectionMapping.Controls.Add(this.labelSectionMapping);
            this.stackSectionMapping.Gap = 4;
            this.stackSectionMapping.Location = new System.Drawing.Point(251, 0);
            this.stackSectionMapping.Margin = new System.Windows.Forms.Padding(0);
            this.stackSectionMapping.Name = "stackSectionMapping";
            this.stackSectionMapping.Padding = new System.Windows.Forms.Padding(4);
            this.stackSectionMapping.Size = new System.Drawing.Size(167, 431);
            this.stackSectionMapping.TabIndex = 1;
            this.stackSectionMapping.Text = "stackSectionMapping";
            this.stackSectionMapping.Vertical = true;
            // 
            // dropdownAxisId
            // 
            this.dropdownAxisId.Location = new System.Drawing.Point(4, 184);
            this.dropdownAxisId.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownAxisId.Name = "dropdownAxisId";
            this.dropdownAxisId.Size = new System.Drawing.Size(159, 32);
            this.dropdownAxisId.TabIndex = 6;
            this.dropdownAxisId.WaveSize = 0;
            // 
            // labelAxisId
            // 
            this.labelAxisId.Location = new System.Drawing.Point(4, 158);
            this.labelAxisId.Margin = new System.Windows.Forms.Padding(0);
            this.labelAxisId.Name = "labelAxisId";
            this.labelAxisId.Size = new System.Drawing.Size(159, 22);
            this.labelAxisId.TabIndex = 5;
            this.labelAxisId.Text = "卡内轴号（AxisId）";
            // 
            // dropdownCardId
            // 
            this.dropdownCardId.Location = new System.Drawing.Point(4, 122);
            this.dropdownCardId.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownCardId.Name = "dropdownCardId";
            this.dropdownCardId.Size = new System.Drawing.Size(159, 32);
            this.dropdownCardId.TabIndex = 4;
            this.dropdownCardId.WaveSize = 0;
            // 
            // labelCardId
            // 
            this.labelCardId.Location = new System.Drawing.Point(4, 96);
            this.labelCardId.Margin = new System.Windows.Forms.Padding(0);
            this.labelCardId.Name = "labelCardId";
            this.labelCardId.Size = new System.Drawing.Size(159, 22);
            this.labelCardId.TabIndex = 3;
            this.labelCardId.Text = "所属控制卡";
            // 
            // dropdownAxisCategory
            // 
            this.dropdownAxisCategory.Location = new System.Drawing.Point(4, 60);
            this.dropdownAxisCategory.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownAxisCategory.Name = "dropdownAxisCategory";
            this.dropdownAxisCategory.Size = new System.Drawing.Size(159, 32);
            this.dropdownAxisCategory.TabIndex = 2;
            this.dropdownAxisCategory.WaveSize = 0;
            // 
            // labelAxisCategory
            // 
            this.labelAxisCategory.Location = new System.Drawing.Point(4, 34);
            this.labelAxisCategory.Margin = new System.Windows.Forms.Padding(0);
            this.labelAxisCategory.Name = "labelAxisCategory";
            this.labelAxisCategory.Size = new System.Drawing.Size(159, 22);
            this.labelAxisCategory.TabIndex = 1;
            this.labelAxisCategory.Text = "轴分类";
            // 
            // labelSectionMapping
            // 
            this.labelSectionMapping.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionMapping.Location = new System.Drawing.Point(4, 4);
            this.labelSectionMapping.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionMapping.Name = "labelSectionMapping";
            this.labelSectionMapping.Size = new System.Drawing.Size(159, 26);
            this.labelSectionMapping.TabIndex = 0;
            this.labelSectionMapping.Text = "归属与映射";
            // 
            // stackSectionBasic
            // 
            this.stackSectionBasic.AutoScroll = true;
            this.stackSectionBasic.Controls.Add(this.inputDisplayName);
            this.stackSectionBasic.Controls.Add(this.labelDisplayName);
            this.stackSectionBasic.Controls.Add(this.inputName);
            this.stackSectionBasic.Controls.Add(this.labelName);
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
            // inputDisplayName
            // 
            this.inputDisplayName.Location = new System.Drawing.Point(4, 184);
            this.inputDisplayName.Margin = new System.Windows.Forms.Padding(0);
            this.inputDisplayName.Name = "inputDisplayName";
            this.inputDisplayName.PlaceholderText = "请输入显示名称";
            this.inputDisplayName.Size = new System.Drawing.Size(243, 32);
            this.inputDisplayName.TabIndex = 6;
            this.inputDisplayName.WaveSize = 0;
            // 
            // labelDisplayName
            // 
            this.labelDisplayName.Location = new System.Drawing.Point(4, 158);
            this.labelDisplayName.Margin = new System.Windows.Forms.Padding(0);
            this.labelDisplayName.Name = "labelDisplayName";
            this.labelDisplayName.Size = new System.Drawing.Size(243, 22);
            this.labelDisplayName.TabIndex = 5;
            this.labelDisplayName.Text = "显示名称";
            // 
            // inputName
            // 
            this.inputName.Location = new System.Drawing.Point(4, 122);
            this.inputName.Margin = new System.Windows.Forms.Padding(0);
            this.inputName.Name = "inputName";
            this.inputName.PlaceholderText = "请输入内部名称";
            this.inputName.Size = new System.Drawing.Size(243, 32);
            this.inputName.TabIndex = 4;
            this.inputName.WaveSize = 0;
            // 
            // labelName
            // 
            this.labelName.Location = new System.Drawing.Point(4, 96);
            this.labelName.Margin = new System.Windows.Forms.Padding(0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(243, 22);
            this.labelName.TabIndex = 3;
            this.labelName.Text = "内部名称";
            // 
            // inputLogicalAxis
            // 
            this.inputLogicalAxis.Location = new System.Drawing.Point(4, 60);
            this.inputLogicalAxis.Margin = new System.Windows.Forms.Padding(0);
            this.inputLogicalAxis.Name = "inputLogicalAxis";
            this.inputLogicalAxis.PlaceholderText = "请输入逻辑轴号";
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
            this.panelHeader.Size = new System.Drawing.Size(844, 56);
            this.panelHeader.TabIndex = 0;
            // 
            // labelDialogDescription
            // 
            this.labelDialogDescription.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelDialogDescription.Location = new System.Drawing.Point(529, 0);
            this.labelDialogDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDialogDescription.Name = "labelDialogDescription";
            this.labelDialogDescription.Size = new System.Drawing.Size(311, 48);
            this.labelDialogDescription.TabIndex = 1;
            this.labelDialogDescription.Text = "填写轴拓扑配置";
            this.labelDialogDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelDialogTitle
            // 
            this.labelDialogTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelDialogTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelDialogTitle.Location = new System.Drawing.Point(4, 0);
            this.labelDialogTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelDialogTitle.Name = "labelDialogTitle";
            this.labelDialogTitle.Size = new System.Drawing.Size(325, 48);
            this.labelDialogTitle.TabIndex = 0;
            this.labelDialogTitle.Text = "新增轴拓扑";
            // 
            // MotionAxisEditDialog
            // 
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.textureBackgroundDialog);
            this.Name = "MotionAxisEditDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "轴拓扑编辑";
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
        private AntdUI.Label labelLogicalAxis;
        private AntdUI.Input inputLogicalAxis;
        private AntdUI.Label labelName;
        private AntdUI.Input inputName;
        private AntdUI.Label labelDisplayName;
        private AntdUI.Input inputDisplayName;
        private AntdUI.StackPanel stackSectionMapping;
        private AntdUI.Label labelSectionMapping;
        private AntdUI.Label labelAxisCategory;
        private AntdUI.Select dropdownAxisCategory;
        private AntdUI.Label labelCardId;
        private AntdUI.Select dropdownCardId;
        private AntdUI.Label labelAxisId;
        private AntdUI.Select dropdownAxisId;
        private AntdUI.StackPanel stackSectionRuntime;
        private AntdUI.Label labelSectionRuntime;
        private AntdUI.Label labelPhysicalCore;
        private AntdUI.Select dropdownPhysicalCore;
        private AntdUI.Label labelPhysicalAxis;
        private AntdUI.Select dropdownPhysicalAxis;
        private AntdUI.Label labelEnabled;
        private AntdUI.Checkbox checkIsEnabled;
        private AntdUI.Label labelSortOrder;
        private AntdUI.Input inputSortOrder;
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