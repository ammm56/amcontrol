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
            this.panelSectionRemark = new AntdUI.Panel();
            this.panelRowRemark = new AntdUI.Panel();
            this.inputRemark = new AntdUI.Input();
            this.labelRemark = new AntdUI.Label();
            this.panelRowDescription = new AntdUI.Panel();
            this.inputDescription = new AntdUI.Input();
            this.labelDescription = new AntdUI.Label();
            this.labelSectionRemark = new AntdUI.Label();
            this.panelSectionState = new AntdUI.Panel();
            this.panelStateRow1 = new AntdUI.Panel();
            this.panelStateRight = new AntdUI.Panel();
            this.inputSortOrder = new AntdUI.Input();
            this.labelSortOrder = new AntdUI.Label();
            this.panelStateLeft = new AntdUI.Panel();
            this.flowStateFlags = new AntdUI.FlowPanel();
            this.checkIsEnabled = new AntdUI.Checkbox();
            this.labelStateFlags = new AntdUI.Label();
            this.labelSectionState = new AntdUI.Label();
            this.panelSectionInit = new AntdUI.Panel();
            this.panelRowUseExtModule = new AntdUI.Panel();
            this.flowInitFlags = new AntdUI.FlowPanel();
            this.checkUseExtModule = new AntdUI.Checkbox();
            this.labelInitFlags = new AntdUI.Label();
            this.panelRowOpenConfig = new AntdUI.Panel();
            this.inputOpenConfig = new AntdUI.Input();
            this.labelOpenConfig = new AntdUI.Label();
            this.panelInitRow2 = new AntdUI.Panel();
            this.panelInitRow2Right = new AntdUI.Panel();
            this.inputInitOrder = new AntdUI.Input();
            this.labelInitOrder = new AntdUI.Label();
            this.panelInitRow2Left = new AntdUI.Panel();
            this.inputModeParam = new AntdUI.Input();
            this.labelModeParam = new AntdUI.Label();
            this.panelInitRow1 = new AntdUI.Panel();
            this.panelInitRow1Right = new AntdUI.Panel();
            this.inputAxisCount = new AntdUI.Input();
            this.labelAxisCount = new AntdUI.Label();
            this.panelInitRow1Left = new AntdUI.Panel();
            this.inputCoreNumber = new AntdUI.Input();
            this.labelCoreNumber = new AntdUI.Label();
            this.labelSectionInit = new AntdUI.Label();
            this.panelSectionBasic = new AntdUI.Panel();
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
            this.stackFormRows.SuspendLayout();
            this.panelSectionRemark.SuspendLayout();
            this.panelRowRemark.SuspendLayout();
            this.panelRowDescription.SuspendLayout();
            this.panelSectionState.SuspendLayout();
            this.panelStateRow1.SuspendLayout();
            this.panelStateRight.SuspendLayout();
            this.panelStateLeft.SuspendLayout();
            this.flowStateFlags.SuspendLayout();
            this.panelSectionInit.SuspendLayout();
            this.panelRowUseExtModule.SuspendLayout();
            this.flowInitFlags.SuspendLayout();
            this.panelRowOpenConfig.SuspendLayout();
            this.panelInitRow2.SuspendLayout();
            this.panelInitRow2Right.SuspendLayout();
            this.panelInitRow2Left.SuspendLayout();
            this.panelInitRow1.SuspendLayout();
            this.panelInitRow1Right.SuspendLayout();
            this.panelInitRow1Left.SuspendLayout();
            this.panelSectionBasic.SuspendLayout();
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
            this.textureBackgroundDialog.Size = new System.Drawing.Size(520, 650);
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
            this.panelShell.Size = new System.Drawing.Size(520, 650);
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
            this.panelContent.Size = new System.Drawing.Size(464, 481);
            this.panelContent.TabIndex = 1;
            // 
            // stackFormRows
            // 
            this.stackFormRows.AutoScroll = true;
            this.stackFormRows.Controls.Add(this.panelSectionRemark);
            this.stackFormRows.Controls.Add(this.panelSectionState);
            this.stackFormRows.Controls.Add(this.panelSectionInit);
            this.stackFormRows.Controls.Add(this.panelSectionBasic);
            this.stackFormRows.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stackFormRows.Gap = 8;
            this.stackFormRows.Location = new System.Drawing.Point(4, 0);
            this.stackFormRows.Margin = new System.Windows.Forms.Padding(0);
            this.stackFormRows.Name = "stackFormRows";
            this.stackFormRows.Size = new System.Drawing.Size(456, 481);
            this.stackFormRows.TabIndex = 0;
            this.stackFormRows.Text = "stackFormRows";
            this.stackFormRows.Vertical = true;
            // 
            // panelSectionRemark
            // 
            this.panelSectionRemark.Controls.Add(this.panelRowRemark);
            this.panelSectionRemark.Controls.Add(this.panelRowDescription);
            this.panelSectionRemark.Controls.Add(this.labelSectionRemark);
            this.panelSectionRemark.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSectionRemark.Location = new System.Drawing.Point(0, 656);
            this.panelSectionRemark.Margin = new System.Windows.Forms.Padding(0);
            this.panelSectionRemark.Name = "panelSectionRemark";
            this.panelSectionRemark.Radius = 0;
            this.panelSectionRemark.Size = new System.Drawing.Size(456, 187);
            this.panelSectionRemark.TabIndex = 3;
            // 
            // panelRowRemark
            // 
            this.panelRowRemark.Controls.Add(this.inputRemark);
            this.panelRowRemark.Controls.Add(this.labelRemark);
            this.panelRowRemark.Location = new System.Drawing.Point(0, 112);
            this.panelRowRemark.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowRemark.Name = "panelRowRemark";
            this.panelRowRemark.Radius = 0;
            this.panelRowRemark.Size = new System.Drawing.Size(576, 75);
            this.panelRowRemark.TabIndex = 2;
            // 
            // inputRemark
            // 
            this.inputRemark.Dock = System.Windows.Forms.DockStyle.Top;
            this.inputRemark.Location = new System.Drawing.Point(0, 22);
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
            this.panelRowDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelRowDescription.Location = new System.Drawing.Point(0, 22);
            this.panelRowDescription.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowDescription.Name = "panelRowDescription";
            this.panelRowDescription.Radius = 0;
            this.panelRowDescription.Size = new System.Drawing.Size(456, 75);
            this.panelRowDescription.TabIndex = 1;
            // 
            // inputDescription
            // 
            this.inputDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.inputDescription.Location = new System.Drawing.Point(0, 22);
            this.inputDescription.Margin = new System.Windows.Forms.Padding(0);
            this.inputDescription.Multiline = true;
            this.inputDescription.Name = "inputDescription";
            this.inputDescription.PlaceholderText = "请输入描述";
            this.inputDescription.Size = new System.Drawing.Size(456, 50);
            this.inputDescription.TabIndex = 1;
            this.inputDescription.WaveSize = 0;
            // 
            // labelDescription
            // 
            this.labelDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDescription.Location = new System.Drawing.Point(0, 0);
            this.labelDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(456, 22);
            this.labelDescription.TabIndex = 0;
            this.labelDescription.Text = "描述";
            // 
            // labelSectionRemark
            // 
            this.labelSectionRemark.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelSectionRemark.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionRemark.Location = new System.Drawing.Point(0, 0);
            this.labelSectionRemark.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionRemark.Name = "labelSectionRemark";
            this.labelSectionRemark.Size = new System.Drawing.Size(456, 22);
            this.labelSectionRemark.TabIndex = 0;
            this.labelSectionRemark.Text = "说明与备注";
            // 
            // panelSectionState
            // 
            this.panelSectionState.Controls.Add(this.panelStateRow1);
            this.panelSectionState.Controls.Add(this.labelSectionState);
            this.panelSectionState.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSectionState.Location = new System.Drawing.Point(0, 568);
            this.panelSectionState.Margin = new System.Windows.Forms.Padding(0);
            this.panelSectionState.Name = "panelSectionState";
            this.panelSectionState.Radius = 0;
            this.panelSectionState.Size = new System.Drawing.Size(456, 80);
            this.panelSectionState.TabIndex = 2;
            // 
            // panelStateRow1
            // 
            this.panelStateRow1.Controls.Add(this.panelStateRight);
            this.panelStateRow1.Controls.Add(this.panelStateLeft);
            this.panelStateRow1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelStateRow1.Location = new System.Drawing.Point(0, 22);
            this.panelStateRow1.Margin = new System.Windows.Forms.Padding(0);
            this.panelStateRow1.Name = "panelStateRow1";
            this.panelStateRow1.Radius = 0;
            this.panelStateRow1.Size = new System.Drawing.Size(456, 55);
            this.panelStateRow1.TabIndex = 1;
            // 
            // panelStateRight
            // 
            this.panelStateRight.Controls.Add(this.inputSortOrder);
            this.panelStateRight.Controls.Add(this.labelSortOrder);
            this.panelStateRight.Location = new System.Drawing.Point(292, 0);
            this.panelStateRight.Margin = new System.Windows.Forms.Padding(0);
            this.panelStateRight.Name = "panelStateRight";
            this.panelStateRight.Radius = 0;
            this.panelStateRight.Size = new System.Drawing.Size(284, 55);
            this.panelStateRight.TabIndex = 1;
            // 
            // inputSortOrder
            // 
            this.inputSortOrder.Dock = System.Windows.Forms.DockStyle.Top;
            this.inputSortOrder.Location = new System.Drawing.Point(0, 22);
            this.inputSortOrder.Margin = new System.Windows.Forms.Padding(0);
            this.inputSortOrder.Name = "inputSortOrder";
            this.inputSortOrder.PlaceholderText = "请输入排序号";
            this.inputSortOrder.Size = new System.Drawing.Size(284, 32);
            this.inputSortOrder.TabIndex = 1;
            this.inputSortOrder.WaveSize = 0;
            // 
            // labelSortOrder
            // 
            this.labelSortOrder.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelSortOrder.Location = new System.Drawing.Point(0, 0);
            this.labelSortOrder.Margin = new System.Windows.Forms.Padding(0);
            this.labelSortOrder.Name = "labelSortOrder";
            this.labelSortOrder.Size = new System.Drawing.Size(284, 22);
            this.labelSortOrder.TabIndex = 0;
            this.labelSortOrder.Text = "排序号";
            // 
            // panelStateLeft
            // 
            this.panelStateLeft.Controls.Add(this.flowStateFlags);
            this.panelStateLeft.Controls.Add(this.labelStateFlags);
            this.panelStateLeft.Location = new System.Drawing.Point(0, 0);
            this.panelStateLeft.Margin = new System.Windows.Forms.Padding(0);
            this.panelStateLeft.Name = "panelStateLeft";
            this.panelStateLeft.Radius = 0;
            this.panelStateLeft.Size = new System.Drawing.Size(284, 55);
            this.panelStateLeft.TabIndex = 0;
            // 
            // flowStateFlags
            // 
            this.flowStateFlags.Controls.Add(this.checkIsEnabled);
            this.flowStateFlags.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowStateFlags.Gap = 8;
            this.flowStateFlags.Location = new System.Drawing.Point(0, 22);
            this.flowStateFlags.Margin = new System.Windows.Forms.Padding(0);
            this.flowStateFlags.Name = "flowStateFlags";
            this.flowStateFlags.Size = new System.Drawing.Size(284, 33);
            this.flowStateFlags.TabIndex = 1;
            this.flowStateFlags.Text = "flowStateFlags";
            // 
            // checkIsEnabled
            // 
            this.checkIsEnabled.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkIsEnabled.Checked = true;
            this.checkIsEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkIsEnabled.Location = new System.Drawing.Point(0, 0);
            this.checkIsEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.checkIsEnabled.Name = "checkIsEnabled";
            this.checkIsEnabled.Size = new System.Drawing.Size(114, 34);
            this.checkIsEnabled.TabIndex = 0;
            this.checkIsEnabled.Text = "启用此控制卡";
            // 
            // labelStateFlags
            // 
            this.labelStateFlags.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelStateFlags.Location = new System.Drawing.Point(0, 0);
            this.labelStateFlags.Margin = new System.Windows.Forms.Padding(0);
            this.labelStateFlags.Name = "labelStateFlags";
            this.labelStateFlags.Size = new System.Drawing.Size(284, 22);
            this.labelStateFlags.TabIndex = 0;
            this.labelStateFlags.Text = "启用状态";
            // 
            // labelSectionState
            // 
            this.labelSectionState.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelSectionState.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionState.Location = new System.Drawing.Point(0, 0);
            this.labelSectionState.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionState.Name = "labelSectionState";
            this.labelSectionState.Size = new System.Drawing.Size(456, 22);
            this.labelSectionState.TabIndex = 0;
            this.labelSectionState.Text = "状态与排序";
            // 
            // panelSectionInit
            // 
            this.panelSectionInit.Controls.Add(this.panelRowUseExtModule);
            this.panelSectionInit.Controls.Add(this.panelRowOpenConfig);
            this.panelSectionInit.Controls.Add(this.panelInitRow2);
            this.panelSectionInit.Controls.Add(this.panelInitRow1);
            this.panelSectionInit.Controls.Add(this.labelSectionInit);
            this.panelSectionInit.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelSectionInit.Location = new System.Drawing.Point(0, 313);
            this.panelSectionInit.Margin = new System.Windows.Forms.Padding(0);
            this.panelSectionInit.Name = "panelSectionInit";
            this.panelSectionInit.Radius = 0;
            this.panelSectionInit.Size = new System.Drawing.Size(456, 247);
            this.panelSectionInit.TabIndex = 1;
            // 
            // panelRowUseExtModule
            // 
            this.panelRowUseExtModule.Controls.Add(this.flowInitFlags);
            this.panelRowUseExtModule.Controls.Add(this.labelInitFlags);
            this.panelRowUseExtModule.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelRowUseExtModule.Location = new System.Drawing.Point(0, 187);
            this.panelRowUseExtModule.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowUseExtModule.Name = "panelRowUseExtModule";
            this.panelRowUseExtModule.Radius = 0;
            this.panelRowUseExtModule.Size = new System.Drawing.Size(456, 55);
            this.panelRowUseExtModule.TabIndex = 4;
            // 
            // flowInitFlags
            // 
            this.flowInitFlags.Controls.Add(this.checkUseExtModule);
            this.flowInitFlags.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowInitFlags.Gap = 8;
            this.flowInitFlags.Location = new System.Drawing.Point(0, 22);
            this.flowInitFlags.Margin = new System.Windows.Forms.Padding(0);
            this.flowInitFlags.Name = "flowInitFlags";
            this.flowInitFlags.Size = new System.Drawing.Size(456, 33);
            this.flowInitFlags.TabIndex = 1;
            this.flowInitFlags.Text = "flowInitFlags";
            // 
            // checkUseExtModule
            // 
            this.checkUseExtModule.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkUseExtModule.Location = new System.Drawing.Point(0, 0);
            this.checkUseExtModule.Margin = new System.Windows.Forms.Padding(0);
            this.checkUseExtModule.Name = "checkUseExtModule";
            this.checkUseExtModule.Size = new System.Drawing.Size(137, 34);
            this.checkUseExtModule.TabIndex = 0;
            this.checkUseExtModule.Text = "启用扩展 IO 模块";
            // 
            // labelInitFlags
            // 
            this.labelInitFlags.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelInitFlags.Location = new System.Drawing.Point(0, 0);
            this.labelInitFlags.Margin = new System.Windows.Forms.Padding(0);
            this.labelInitFlags.Name = "labelInitFlags";
            this.labelInitFlags.Size = new System.Drawing.Size(456, 22);
            this.labelInitFlags.TabIndex = 0;
            this.labelInitFlags.Text = "扩展模块";
            // 
            // panelRowOpenConfig
            // 
            this.panelRowOpenConfig.Controls.Add(this.inputOpenConfig);
            this.panelRowOpenConfig.Controls.Add(this.labelOpenConfig);
            this.panelRowOpenConfig.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelRowOpenConfig.Location = new System.Drawing.Point(0, 132);
            this.panelRowOpenConfig.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowOpenConfig.Name = "panelRowOpenConfig";
            this.panelRowOpenConfig.Radius = 0;
            this.panelRowOpenConfig.Size = new System.Drawing.Size(456, 55);
            this.panelRowOpenConfig.TabIndex = 3;
            // 
            // inputOpenConfig
            // 
            this.inputOpenConfig.Dock = System.Windows.Forms.DockStyle.Top;
            this.inputOpenConfig.Location = new System.Drawing.Point(0, 22);
            this.inputOpenConfig.Margin = new System.Windows.Forms.Padding(0);
            this.inputOpenConfig.Name = "inputOpenConfig";
            this.inputOpenConfig.PlaceholderText = "请输入初始参数（可选）";
            this.inputOpenConfig.Size = new System.Drawing.Size(456, 32);
            this.inputOpenConfig.TabIndex = 1;
            this.inputOpenConfig.WaveSize = 0;
            // 
            // labelOpenConfig
            // 
            this.labelOpenConfig.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelOpenConfig.Location = new System.Drawing.Point(0, 0);
            this.labelOpenConfig.Margin = new System.Windows.Forms.Padding(0);
            this.labelOpenConfig.Name = "labelOpenConfig";
            this.labelOpenConfig.Size = new System.Drawing.Size(456, 22);
            this.labelOpenConfig.TabIndex = 0;
            this.labelOpenConfig.Text = "初始参数（OpenConfig）";
            // 
            // panelInitRow2
            // 
            this.panelInitRow2.Controls.Add(this.panelInitRow2Right);
            this.panelInitRow2.Controls.Add(this.panelInitRow2Left);
            this.panelInitRow2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelInitRow2.Location = new System.Drawing.Point(0, 77);
            this.panelInitRow2.Margin = new System.Windows.Forms.Padding(0);
            this.panelInitRow2.Name = "panelInitRow2";
            this.panelInitRow2.Radius = 0;
            this.panelInitRow2.Size = new System.Drawing.Size(456, 55);
            this.panelInitRow2.TabIndex = 2;
            // 
            // panelInitRow2Right
            // 
            this.panelInitRow2Right.Controls.Add(this.inputInitOrder);
            this.panelInitRow2Right.Controls.Add(this.labelInitOrder);
            this.panelInitRow2Right.Location = new System.Drawing.Point(292, 0);
            this.panelInitRow2Right.Margin = new System.Windows.Forms.Padding(0);
            this.panelInitRow2Right.Name = "panelInitRow2Right";
            this.panelInitRow2Right.Radius = 0;
            this.panelInitRow2Right.Size = new System.Drawing.Size(284, 55);
            this.panelInitRow2Right.TabIndex = 1;
            // 
            // inputInitOrder
            // 
            this.inputInitOrder.Dock = System.Windows.Forms.DockStyle.Top;
            this.inputInitOrder.Location = new System.Drawing.Point(0, 22);
            this.inputInitOrder.Margin = new System.Windows.Forms.Padding(0);
            this.inputInitOrder.Name = "inputInitOrder";
            this.inputInitOrder.PlaceholderText = "请输入初始化顺序";
            this.inputInitOrder.Size = new System.Drawing.Size(284, 32);
            this.inputInitOrder.TabIndex = 1;
            this.inputInitOrder.WaveSize = 0;
            // 
            // labelInitOrder
            // 
            this.labelInitOrder.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelInitOrder.Location = new System.Drawing.Point(0, 0);
            this.labelInitOrder.Margin = new System.Windows.Forms.Padding(0);
            this.labelInitOrder.Name = "labelInitOrder";
            this.labelInitOrder.Size = new System.Drawing.Size(284, 22);
            this.labelInitOrder.TabIndex = 0;
            this.labelInitOrder.Text = "初始化顺序";
            // 
            // panelInitRow2Left
            // 
            this.panelInitRow2Left.Controls.Add(this.inputModeParam);
            this.panelInitRow2Left.Controls.Add(this.labelModeParam);
            this.panelInitRow2Left.Location = new System.Drawing.Point(0, 0);
            this.panelInitRow2Left.Margin = new System.Windows.Forms.Padding(0);
            this.panelInitRow2Left.Name = "panelInitRow2Left";
            this.panelInitRow2Left.Radius = 0;
            this.panelInitRow2Left.Size = new System.Drawing.Size(284, 55);
            this.panelInitRow2Left.TabIndex = 0;
            // 
            // inputModeParam
            // 
            this.inputModeParam.Dock = System.Windows.Forms.DockStyle.Top;
            this.inputModeParam.Location = new System.Drawing.Point(0, 22);
            this.inputModeParam.Margin = new System.Windows.Forms.Padding(0);
            this.inputModeParam.Name = "inputModeParam";
            this.inputModeParam.PlaceholderText = "请输入打开模式参数";
            this.inputModeParam.Size = new System.Drawing.Size(284, 32);
            this.inputModeParam.TabIndex = 1;
            this.inputModeParam.WaveSize = 0;
            // 
            // labelModeParam
            // 
            this.labelModeParam.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelModeParam.Location = new System.Drawing.Point(0, 0);
            this.labelModeParam.Margin = new System.Windows.Forms.Padding(0);
            this.labelModeParam.Name = "labelModeParam";
            this.labelModeParam.Size = new System.Drawing.Size(284, 22);
            this.labelModeParam.TabIndex = 0;
            this.labelModeParam.Text = "打开模式参数";
            // 
            // panelInitRow1
            // 
            this.panelInitRow1.Controls.Add(this.panelInitRow1Right);
            this.panelInitRow1.Controls.Add(this.panelInitRow1Left);
            this.panelInitRow1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelInitRow1.Location = new System.Drawing.Point(0, 22);
            this.panelInitRow1.Margin = new System.Windows.Forms.Padding(0);
            this.panelInitRow1.Name = "panelInitRow1";
            this.panelInitRow1.Radius = 0;
            this.panelInitRow1.Size = new System.Drawing.Size(456, 55);
            this.panelInitRow1.TabIndex = 1;
            // 
            // panelInitRow1Right
            // 
            this.panelInitRow1Right.Controls.Add(this.inputAxisCount);
            this.panelInitRow1Right.Controls.Add(this.labelAxisCount);
            this.panelInitRow1Right.Location = new System.Drawing.Point(292, 0);
            this.panelInitRow1Right.Margin = new System.Windows.Forms.Padding(0);
            this.panelInitRow1Right.Name = "panelInitRow1Right";
            this.panelInitRow1Right.Radius = 0;
            this.panelInitRow1Right.Size = new System.Drawing.Size(284, 55);
            this.panelInitRow1Right.TabIndex = 1;
            // 
            // inputAxisCount
            // 
            this.inputAxisCount.Dock = System.Windows.Forms.DockStyle.Top;
            this.inputAxisCount.Location = new System.Drawing.Point(0, 22);
            this.inputAxisCount.Margin = new System.Windows.Forms.Padding(0);
            this.inputAxisCount.Name = "inputAxisCount";
            this.inputAxisCount.PlaceholderText = "请输入支持轴总数";
            this.inputAxisCount.Size = new System.Drawing.Size(284, 32);
            this.inputAxisCount.TabIndex = 1;
            this.inputAxisCount.WaveSize = 0;
            // 
            // labelAxisCount
            // 
            this.labelAxisCount.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelAxisCount.Location = new System.Drawing.Point(0, 0);
            this.labelAxisCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelAxisCount.Name = "labelAxisCount";
            this.labelAxisCount.Size = new System.Drawing.Size(284, 22);
            this.labelAxisCount.TabIndex = 0;
            this.labelAxisCount.Text = "支持轴总数";
            // 
            // panelInitRow1Left
            // 
            this.panelInitRow1Left.Controls.Add(this.inputCoreNumber);
            this.panelInitRow1Left.Controls.Add(this.labelCoreNumber);
            this.panelInitRow1Left.Location = new System.Drawing.Point(0, 0);
            this.panelInitRow1Left.Margin = new System.Windows.Forms.Padding(0);
            this.panelInitRow1Left.Name = "panelInitRow1Left";
            this.panelInitRow1Left.Radius = 0;
            this.panelInitRow1Left.Size = new System.Drawing.Size(284, 55);
            this.panelInitRow1Left.TabIndex = 0;
            // 
            // inputCoreNumber
            // 
            this.inputCoreNumber.Dock = System.Windows.Forms.DockStyle.Top;
            this.inputCoreNumber.Location = new System.Drawing.Point(0, 22);
            this.inputCoreNumber.Margin = new System.Windows.Forms.Padding(0);
            this.inputCoreNumber.Name = "inputCoreNumber";
            this.inputCoreNumber.PlaceholderText = "请输入内核数量";
            this.inputCoreNumber.Size = new System.Drawing.Size(284, 32);
            this.inputCoreNumber.TabIndex = 1;
            this.inputCoreNumber.WaveSize = 0;
            // 
            // labelCoreNumber
            // 
            this.labelCoreNumber.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelCoreNumber.Location = new System.Drawing.Point(0, 0);
            this.labelCoreNumber.Margin = new System.Windows.Forms.Padding(0);
            this.labelCoreNumber.Name = "labelCoreNumber";
            this.labelCoreNumber.Size = new System.Drawing.Size(284, 22);
            this.labelCoreNumber.TabIndex = 0;
            this.labelCoreNumber.Text = "内核数量";
            // 
            // labelSectionInit
            // 
            this.labelSectionInit.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelSectionInit.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionInit.Location = new System.Drawing.Point(0, 0);
            this.labelSectionInit.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionInit.Name = "labelSectionInit";
            this.labelSectionInit.Size = new System.Drawing.Size(456, 22);
            this.labelSectionInit.TabIndex = 0;
            this.labelSectionInit.Text = "初始化参数";
            // 
            // panelSectionBasic
            // 
            this.panelSectionBasic.Controls.Add(this.panelRowDriverKey);
            this.panelSectionBasic.Controls.Add(this.panelRowDisplayName);
            this.panelSectionBasic.Controls.Add(this.panelRowName);
            this.panelSectionBasic.Controls.Add(this.panelRowCardType);
            this.panelSectionBasic.Controls.Add(this.panelRowCardId);
            this.panelSectionBasic.Controls.Add(this.labelSectionBasic);
            this.panelSectionBasic.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSectionBasic.Location = new System.Drawing.Point(0, 0);
            this.panelSectionBasic.Margin = new System.Windows.Forms.Padding(0);
            this.panelSectionBasic.Name = "panelSectionBasic";
            this.panelSectionBasic.Radius = 0;
            this.panelSectionBasic.Size = new System.Drawing.Size(456, 305);
            this.panelSectionBasic.TabIndex = 0;
            // 
            // panelRowDriverKey
            // 
            this.panelRowDriverKey.Controls.Add(this.inputDriverKey);
            this.panelRowDriverKey.Controls.Add(this.labelDriverKey);
            this.panelRowDriverKey.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelRowDriverKey.Location = new System.Drawing.Point(0, 242);
            this.panelRowDriverKey.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowDriverKey.Name = "panelRowDriverKey";
            this.panelRowDriverKey.Radius = 0;
            this.panelRowDriverKey.Size = new System.Drawing.Size(456, 55);
            this.panelRowDriverKey.TabIndex = 5;
            // 
            // inputDriverKey
            // 
            this.inputDriverKey.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputDriverKey.Location = new System.Drawing.Point(0, 23);
            this.inputDriverKey.Margin = new System.Windows.Forms.Padding(0);
            this.inputDriverKey.Name = "inputDriverKey";
            this.inputDriverKey.PlaceholderText = "请输入驱动识别键（DriverKey）";
            this.inputDriverKey.Size = new System.Drawing.Size(456, 32);
            this.inputDriverKey.TabIndex = 1;
            this.inputDriverKey.WaveSize = 0;
            // 
            // labelDriverKey
            // 
            this.labelDriverKey.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDriverKey.Location = new System.Drawing.Point(0, 0);
            this.labelDriverKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelDriverKey.Name = "labelDriverKey";
            this.labelDriverKey.Size = new System.Drawing.Size(456, 22);
            this.labelDriverKey.TabIndex = 0;
            this.labelDriverKey.Text = "驱动识别键（DriverKey）";
            // 
            // panelRowDisplayName
            // 
            this.panelRowDisplayName.Controls.Add(this.inputDisplayName);
            this.panelRowDisplayName.Controls.Add(this.labelDisplayName);
            this.panelRowDisplayName.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelRowDisplayName.Location = new System.Drawing.Point(0, 187);
            this.panelRowDisplayName.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowDisplayName.Name = "panelRowDisplayName";
            this.panelRowDisplayName.Radius = 0;
            this.panelRowDisplayName.Size = new System.Drawing.Size(456, 55);
            this.panelRowDisplayName.TabIndex = 4;
            // 
            // inputDisplayName
            // 
            this.inputDisplayName.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputDisplayName.Location = new System.Drawing.Point(0, 23);
            this.inputDisplayName.Margin = new System.Windows.Forms.Padding(0);
            this.inputDisplayName.Name = "inputDisplayName";
            this.inputDisplayName.PlaceholderText = "请输入显示名称";
            this.inputDisplayName.Size = new System.Drawing.Size(456, 32);
            this.inputDisplayName.TabIndex = 1;
            this.inputDisplayName.WaveSize = 0;
            // 
            // labelDisplayName
            // 
            this.labelDisplayName.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDisplayName.Location = new System.Drawing.Point(0, 0);
            this.labelDisplayName.Margin = new System.Windows.Forms.Padding(0);
            this.labelDisplayName.Name = "labelDisplayName";
            this.labelDisplayName.Size = new System.Drawing.Size(456, 22);
            this.labelDisplayName.TabIndex = 0;
            this.labelDisplayName.Text = "显示名称（界面展示用）";
            // 
            // panelRowName
            // 
            this.panelRowName.Controls.Add(this.inputName);
            this.panelRowName.Controls.Add(this.labelName);
            this.panelRowName.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelRowName.Location = new System.Drawing.Point(0, 132);
            this.panelRowName.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowName.Name = "panelRowName";
            this.panelRowName.Radius = 0;
            this.panelRowName.Size = new System.Drawing.Size(456, 55);
            this.panelRowName.TabIndex = 3;
            // 
            // inputName
            // 
            this.inputName.Dock = System.Windows.Forms.DockStyle.Top;
            this.inputName.Location = new System.Drawing.Point(0, 22);
            this.inputName.Margin = new System.Windows.Forms.Padding(0);
            this.inputName.Name = "inputName";
            this.inputName.PlaceholderText = "请输入内部名称";
            this.inputName.Size = new System.Drawing.Size(456, 32);
            this.inputName.TabIndex = 1;
            this.inputName.WaveSize = 0;
            // 
            // labelName
            // 
            this.labelName.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelName.Location = new System.Drawing.Point(0, 0);
            this.labelName.Margin = new System.Windows.Forms.Padding(0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(456, 22);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "内部名称 *";
            // 
            // panelRowCardType
            // 
            this.panelRowCardType.Controls.Add(this.dropdownCardType);
            this.panelRowCardType.Controls.Add(this.labelCardType);
            this.panelRowCardType.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelRowCardType.Location = new System.Drawing.Point(0, 77);
            this.panelRowCardType.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowCardType.Name = "panelRowCardType";
            this.panelRowCardType.Radius = 0;
            this.panelRowCardType.Size = new System.Drawing.Size(456, 55);
            this.panelRowCardType.TabIndex = 2;
            // 
            // dropdownCardType
            // 
            this.dropdownCardType.Dock = System.Windows.Forms.DockStyle.Top;
            this.dropdownCardType.Location = new System.Drawing.Point(0, 22);
            this.dropdownCardType.Margin = new System.Windows.Forms.Padding(0);
            this.dropdownCardType.Name = "dropdownCardType";
            this.dropdownCardType.Size = new System.Drawing.Size(456, 32);
            this.dropdownCardType.TabIndex = 1;
            this.dropdownCardType.WaveSize = 0;
            // 
            // labelCardType
            // 
            this.labelCardType.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelCardType.Location = new System.Drawing.Point(0, 0);
            this.labelCardType.Margin = new System.Windows.Forms.Padding(0);
            this.labelCardType.Name = "labelCardType";
            this.labelCardType.Size = new System.Drawing.Size(456, 22);
            this.labelCardType.TabIndex = 0;
            this.labelCardType.Text = "控制卡类型";
            // 
            // panelRowCardId
            // 
            this.panelRowCardId.Controls.Add(this.inputCardId);
            this.panelRowCardId.Controls.Add(this.labelCardId);
            this.panelRowCardId.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelRowCardId.Location = new System.Drawing.Point(0, 22);
            this.panelRowCardId.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowCardId.Name = "panelRowCardId";
            this.panelRowCardId.Radius = 0;
            this.panelRowCardId.Size = new System.Drawing.Size(456, 55);
            this.panelRowCardId.TabIndex = 1;
            // 
            // inputCardId
            // 
            this.inputCardId.Dock = System.Windows.Forms.DockStyle.Top;
            this.inputCardId.Location = new System.Drawing.Point(0, 22);
            this.inputCardId.Margin = new System.Windows.Forms.Padding(0);
            this.inputCardId.Name = "inputCardId";
            this.inputCardId.PlaceholderText = "请输入硬件卡号（Card Id）";
            this.inputCardId.Size = new System.Drawing.Size(456, 32);
            this.inputCardId.TabIndex = 1;
            this.inputCardId.WaveSize = 0;
            // 
            // labelCardId
            // 
            this.labelCardId.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelCardId.Location = new System.Drawing.Point(0, 0);
            this.labelCardId.Margin = new System.Windows.Forms.Padding(0);
            this.labelCardId.Name = "labelCardId";
            this.labelCardId.Size = new System.Drawing.Size(456, 22);
            this.labelCardId.TabIndex = 0;
            this.labelCardId.Text = "硬件卡号（CardId）";
            // 
            // labelSectionBasic
            // 
            this.labelSectionBasic.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelSectionBasic.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionBasic.Location = new System.Drawing.Point(0, 0);
            this.labelSectionBasic.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionBasic.Name = "labelSectionBasic";
            this.labelSectionBasic.Size = new System.Drawing.Size(456, 22);
            this.labelSectionBasic.TabIndex = 0;
            this.labelSectionBasic.Text = "基础标识";
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.flowFooterButtons);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(28, 565);
            this.panelFooter.Margin = new System.Windows.Forms.Padding(0);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Padding = new System.Windows.Forms.Padding(4, 10, 4, 0);
            this.panelFooter.Radius = 0;
            this.panelFooter.Size = new System.Drawing.Size(464, 57);
            this.panelFooter.TabIndex = 2;
            // 
            // flowFooterButtons
            // 
            this.flowFooterButtons.Controls.Add(this.buttonOk);
            this.flowFooterButtons.Controls.Add(this.buttonCancel);
            this.flowFooterButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowFooterButtons.Gap = 10;
            this.flowFooterButtons.Location = new System.Drawing.Point(216, 10);
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
            this.panelHeader.Size = new System.Drawing.Size(464, 56);
            this.panelHeader.TabIndex = 0;
            // 
            // flowHeaderRight
            // 
            this.flowHeaderRight.Controls.Add(this.labelDialogDescription);
            this.flowHeaderRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowHeaderRight.Location = new System.Drawing.Point(191, 0);
            this.flowHeaderRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowHeaderRight.Name = "flowHeaderRight";
            this.flowHeaderRight.Size = new System.Drawing.Size(269, 48);
            this.flowHeaderRight.TabIndex = 1;
            this.flowHeaderRight.Text = "flowHeaderRight";
            // 
            // labelDialogDescription
            // 
            this.labelDialogDescription.Location = new System.Drawing.Point(0, 0);
            this.labelDialogDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDialogDescription.Name = "labelDialogDescription";
            this.labelDialogDescription.Size = new System.Drawing.Size(267, 48);
            this.labelDialogDescription.TabIndex = 0;
            this.labelDialogDescription.Text = "填写控制卡信息";
            this.labelDialogDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // flowHeaderLeft
            // 
            this.flowHeaderLeft.Controls.Add(this.labelDialogTitle);
            this.flowHeaderLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowHeaderLeft.Location = new System.Drawing.Point(4, 0);
            this.flowHeaderLeft.Margin = new System.Windows.Forms.Padding(0);
            this.flowHeaderLeft.Name = "flowHeaderLeft";
            this.flowHeaderLeft.Size = new System.Drawing.Size(187, 48);
            this.flowHeaderLeft.TabIndex = 0;
            this.flowHeaderLeft.Text = "flowHeaderLeft";
            // 
            // labelDialogTitle
            // 
            this.labelDialogTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelDialogTitle.Location = new System.Drawing.Point(0, 0);
            this.labelDialogTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelDialogTitle.Name = "labelDialogTitle";
            this.labelDialogTitle.Size = new System.Drawing.Size(187, 48);
            this.labelDialogTitle.TabIndex = 0;
            this.labelDialogTitle.Text = "新增控制卡";
            // 
            // MotionCardEditDialog
            // 
            this.ClientSize = new System.Drawing.Size(520, 650);
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
            this.panelSectionRemark.ResumeLayout(false);
            this.panelRowRemark.ResumeLayout(false);
            this.panelRowDescription.ResumeLayout(false);
            this.panelSectionState.ResumeLayout(false);
            this.panelStateRow1.ResumeLayout(false);
            this.panelStateRight.ResumeLayout(false);
            this.panelStateLeft.ResumeLayout(false);
            this.flowStateFlags.ResumeLayout(false);
            this.flowStateFlags.PerformLayout();
            this.panelSectionInit.ResumeLayout(false);
            this.panelRowUseExtModule.ResumeLayout(false);
            this.flowInitFlags.ResumeLayout(false);
            this.flowInitFlags.PerformLayout();
            this.panelRowOpenConfig.ResumeLayout(false);
            this.panelInitRow2.ResumeLayout(false);
            this.panelInitRow2Right.ResumeLayout(false);
            this.panelInitRow2Left.ResumeLayout(false);
            this.panelInitRow1.ResumeLayout(false);
            this.panelInitRow1Right.ResumeLayout(false);
            this.panelInitRow1Left.ResumeLayout(false);
            this.panelSectionBasic.ResumeLayout(false);
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
        private AntdUI.Panel panelSectionBasic;
        private AntdUI.Label labelSectionBasic;
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
        private AntdUI.Panel panelSectionInit;
        private AntdUI.Label labelSectionInit;
        private AntdUI.Panel panelInitRow1;
        private AntdUI.Panel panelInitRow1Left;
        private AntdUI.Input inputCoreNumber;
        private AntdUI.Label labelCoreNumber;
        private AntdUI.Panel panelInitRow1Right;
        private AntdUI.Input inputAxisCount;
        private AntdUI.Label labelAxisCount;
        private AntdUI.Panel panelInitRow2;
        private AntdUI.Panel panelInitRow2Left;
        private AntdUI.Input inputModeParam;
        private AntdUI.Label labelModeParam;
        private AntdUI.Panel panelInitRow2Right;
        private AntdUI.Input inputInitOrder;
        private AntdUI.Label labelInitOrder;
        private AntdUI.Panel panelRowOpenConfig;
        private AntdUI.Input inputOpenConfig;
        private AntdUI.Label labelOpenConfig;
        private AntdUI.Panel panelRowUseExtModule;
        private AntdUI.FlowPanel flowInitFlags;
        private AntdUI.Checkbox checkUseExtModule;
        private AntdUI.Label labelInitFlags;
        private AntdUI.Panel panelSectionState;
        private AntdUI.Label labelSectionState;
        private AntdUI.Panel panelStateRow1;
        private AntdUI.Panel panelStateLeft;
        private AntdUI.FlowPanel flowStateFlags;
        private AntdUI.Checkbox checkIsEnabled;
        private AntdUI.Label labelStateFlags;
        private AntdUI.Panel panelStateRight;
        private AntdUI.Input inputSortOrder;
        private AntdUI.Label labelSortOrder;
        private AntdUI.Panel panelSectionRemark;
        private AntdUI.Label labelSectionRemark;
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