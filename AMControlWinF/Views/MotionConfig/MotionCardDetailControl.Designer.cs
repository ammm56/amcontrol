namespace AMControlWinF.Views.MotionConfig
{
    partial class MotionCardDetailControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelRoot = new AntdUI.Panel();
            this.gridDetails = new AntdUI.GridPanel();
            this.stackRemark = new AntdUI.StackPanel();
            this.labelValueRemark = new AntdUI.Label();
            this.labelRemark = new AntdUI.Label();
            this.labelValueDescription = new AntdUI.Label();
            this.labelDescription = new AntdUI.Label();
            this.dividerSectionRemark = new AntdUI.Divider();
            this.stackState = new AntdUI.StackPanel();
            this.labelValueUpdateTime = new AntdUI.Label();
            this.labelUpdateTime = new AntdUI.Label();
            this.labelValueSortOrder = new AntdUI.Label();
            this.labelSortOrder = new AntdUI.Label();
            this.labelValueEnabled = new AntdUI.Label();
            this.labelEnabled = new AntdUI.Label();
            this.dividerSectionState = new AntdUI.Divider();
            this.stackInit = new AntdUI.StackPanel();
            this.labelValueUseExtModule = new AntdUI.Label();
            this.labelUseExtModule = new AntdUI.Label();
            this.labelValueOpenConfig = new AntdUI.Label();
            this.labelOpenConfig = new AntdUI.Label();
            this.labelValueInitOrder = new AntdUI.Label();
            this.labelInitOrder = new AntdUI.Label();
            this.labelValueModeParam = new AntdUI.Label();
            this.labelModeParam = new AntdUI.Label();
            this.labelValueAxisCount = new AntdUI.Label();
            this.labelAxisCount = new AntdUI.Label();
            this.labelValueCoreNumber = new AntdUI.Label();
            this.labelCoreNumber = new AntdUI.Label();
            this.dividerSectionInit = new AntdUI.Divider();
            this.stackBasic = new AntdUI.StackPanel();
            this.labelValueDriverKey = new AntdUI.Label();
            this.labelDriverKey = new AntdUI.Label();
            this.labelValueName = new AntdUI.Label();
            this.labelName = new AntdUI.Label();
            this.labelValueDisplayName = new AntdUI.Label();
            this.labelDisplayName = new AntdUI.Label();
            this.labelValueCardType = new AntdUI.Label();
            this.labelCardType = new AntdUI.Label();
            this.labelValueCardId = new AntdUI.Label();
            this.labelCardId = new AntdUI.Label();
            this.dividerSectionBasic = new AntdUI.Divider();
            this.panelRoot.SuspendLayout();
            this.gridDetails.SuspendLayout();
            this.stackRemark.SuspendLayout();
            this.stackState.SuspendLayout();
            this.stackInit.SuspendLayout();
            this.stackBasic.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.gridDetails);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Margin = new System.Windows.Forms.Padding(0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(600, 560);
            this.panelRoot.TabIndex = 0;
            this.panelRoot.Text = "panelRoot";
            // 
            // gridDetails
            // 
            this.gridDetails.Controls.Add(this.stackRemark);
            this.gridDetails.Controls.Add(this.stackState);
            this.gridDetails.Controls.Add(this.stackInit);
            this.gridDetails.Controls.Add(this.stackBasic);
            this.gridDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDetails.Location = new System.Drawing.Point(0, 0);
            this.gridDetails.Margin = new System.Windows.Forms.Padding(0);
            this.gridDetails.Name = "gridDetails";
            this.gridDetails.Size = new System.Drawing.Size(600, 560);
            this.gridDetails.Span = "50% 50%;50% 50%;-55%";
            this.gridDetails.TabIndex = 0;
            this.gridDetails.Text = "gridDetails";
            // 
            // stackRemark
            // 
            this.stackRemark.AutoScroll = true;
            this.stackRemark.Controls.Add(this.labelValueRemark);
            this.stackRemark.Controls.Add(this.labelRemark);
            this.stackRemark.Controls.Add(this.labelValueDescription);
            this.stackRemark.Controls.Add(this.labelDescription);
            this.stackRemark.Controls.Add(this.dividerSectionRemark);
            this.stackRemark.Location = new System.Drawing.Point(300, 308);
            this.stackRemark.Margin = new System.Windows.Forms.Padding(0);
            this.stackRemark.Name = "stackRemark";
            this.stackRemark.Padding = new System.Windows.Forms.Padding(4);
            this.stackRemark.Size = new System.Drawing.Size(300, 252);
            this.stackRemark.TabIndex = 3;
            this.stackRemark.Text = "stackRemark";
            this.stackRemark.Vertical = true;
            // 
            // labelValueRemark
            // 
            this.labelValueRemark.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueRemark.Location = new System.Drawing.Point(4, 131);
            this.labelValueRemark.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueRemark.Name = "labelValueRemark";
            this.labelValueRemark.Size = new System.Drawing.Size(292, 48);
            this.labelValueRemark.TabIndex = 4;
            this.labelValueRemark.Text = "-";
            this.labelValueRemark.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            // 
            // labelRemark
            // 
            this.labelRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelRemark.Location = new System.Drawing.Point(4, 109);
            this.labelRemark.Margin = new System.Windows.Forms.Padding(0);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(292, 22);
            this.labelRemark.TabIndex = 3;
            this.labelRemark.Text = "备注";
            // 
            // labelValueDescription
            // 
            this.labelValueDescription.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueDescription.Location = new System.Drawing.Point(4, 55);
            this.labelValueDescription.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.labelValueDescription.Name = "labelValueDescription";
            this.labelValueDescription.Size = new System.Drawing.Size(292, 48);
            this.labelValueDescription.TabIndex = 2;
            this.labelValueDescription.Text = "-";
            this.labelValueDescription.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            // 
            // labelDescription
            // 
            this.labelDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelDescription.Location = new System.Drawing.Point(4, 33);
            this.labelDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(292, 22);
            this.labelDescription.TabIndex = 1;
            this.labelDescription.Text = "描述";
            // 
            // dividerSectionRemark
            // 
            this.dividerSectionRemark.Location = new System.Drawing.Point(7, 7);
            this.dividerSectionRemark.Name = "dividerSectionRemark";
            this.dividerSectionRemark.Size = new System.Drawing.Size(286, 23);
            this.dividerSectionRemark.TabIndex = 6;
            this.dividerSectionRemark.Text = "说明与备注";
            // 
            // stackState
            // 
            this.stackState.AutoScroll = true;
            this.stackState.Controls.Add(this.labelValueUpdateTime);
            this.stackState.Controls.Add(this.labelUpdateTime);
            this.stackState.Controls.Add(this.labelValueSortOrder);
            this.stackState.Controls.Add(this.labelSortOrder);
            this.stackState.Controls.Add(this.labelValueEnabled);
            this.stackState.Controls.Add(this.labelEnabled);
            this.stackState.Controls.Add(this.dividerSectionState);
            this.stackState.Location = new System.Drawing.Point(0, 308);
            this.stackState.Margin = new System.Windows.Forms.Padding(0);
            this.stackState.Name = "stackState";
            this.stackState.Padding = new System.Windows.Forms.Padding(4);
            this.stackState.Size = new System.Drawing.Size(300, 252);
            this.stackState.TabIndex = 2;
            this.stackState.Text = "stackState";
            this.stackState.Vertical = true;
            // 
            // labelValueUpdateTime
            // 
            this.labelValueUpdateTime.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueUpdateTime.Location = new System.Drawing.Point(4, 147);
            this.labelValueUpdateTime.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.labelValueUpdateTime.Name = "labelValueUpdateTime";
            this.labelValueUpdateTime.Size = new System.Drawing.Size(292, 24);
            this.labelValueUpdateTime.TabIndex = 6;
            this.labelValueUpdateTime.Text = "-";
            // 
            // labelUpdateTime
            // 
            this.labelUpdateTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelUpdateTime.Location = new System.Drawing.Point(4, 125);
            this.labelUpdateTime.Margin = new System.Windows.Forms.Padding(0);
            this.labelUpdateTime.Name = "labelUpdateTime";
            this.labelUpdateTime.Size = new System.Drawing.Size(292, 22);
            this.labelUpdateTime.TabIndex = 5;
            this.labelUpdateTime.Text = "更新时间";
            // 
            // labelValueSortOrder
            // 
            this.labelValueSortOrder.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueSortOrder.Location = new System.Drawing.Point(4, 101);
            this.labelValueSortOrder.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueSortOrder.Name = "labelValueSortOrder";
            this.labelValueSortOrder.Size = new System.Drawing.Size(292, 24);
            this.labelValueSortOrder.TabIndex = 4;
            this.labelValueSortOrder.Text = "-";
            // 
            // labelSortOrder
            // 
            this.labelSortOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelSortOrder.Location = new System.Drawing.Point(4, 79);
            this.labelSortOrder.Margin = new System.Windows.Forms.Padding(0);
            this.labelSortOrder.Name = "labelSortOrder";
            this.labelSortOrder.Size = new System.Drawing.Size(292, 22);
            this.labelSortOrder.TabIndex = 3;
            this.labelSortOrder.Text = "排序号";
            // 
            // labelValueEnabled
            // 
            this.labelValueEnabled.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueEnabled.Location = new System.Drawing.Point(4, 55);
            this.labelValueEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueEnabled.Name = "labelValueEnabled";
            this.labelValueEnabled.Size = new System.Drawing.Size(292, 24);
            this.labelValueEnabled.TabIndex = 2;
            this.labelValueEnabled.Text = "-";
            // 
            // labelEnabled
            // 
            this.labelEnabled.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelEnabled.Location = new System.Drawing.Point(4, 33);
            this.labelEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.labelEnabled.Name = "labelEnabled";
            this.labelEnabled.Size = new System.Drawing.Size(292, 22);
            this.labelEnabled.TabIndex = 1;
            this.labelEnabled.Text = "启用状态";
            // 
            // dividerSectionState
            // 
            this.dividerSectionState.Location = new System.Drawing.Point(7, 7);
            this.dividerSectionState.Name = "dividerSectionState";
            this.dividerSectionState.Size = new System.Drawing.Size(286, 23);
            this.dividerSectionState.TabIndex = 8;
            this.dividerSectionState.Text = "状态与排序";
            // 
            // stackInit
            // 
            this.stackInit.AutoScroll = true;
            this.stackInit.Controls.Add(this.labelValueUseExtModule);
            this.stackInit.Controls.Add(this.labelUseExtModule);
            this.stackInit.Controls.Add(this.labelValueOpenConfig);
            this.stackInit.Controls.Add(this.labelOpenConfig);
            this.stackInit.Controls.Add(this.labelValueInitOrder);
            this.stackInit.Controls.Add(this.labelInitOrder);
            this.stackInit.Controls.Add(this.labelValueModeParam);
            this.stackInit.Controls.Add(this.labelModeParam);
            this.stackInit.Controls.Add(this.labelValueAxisCount);
            this.stackInit.Controls.Add(this.labelAxisCount);
            this.stackInit.Controls.Add(this.labelValueCoreNumber);
            this.stackInit.Controls.Add(this.labelCoreNumber);
            this.stackInit.Controls.Add(this.dividerSectionInit);
            this.stackInit.Location = new System.Drawing.Point(300, 0);
            this.stackInit.Margin = new System.Windows.Forms.Padding(0);
            this.stackInit.Name = "stackInit";
            this.stackInit.Padding = new System.Windows.Forms.Padding(4);
            this.stackInit.Size = new System.Drawing.Size(300, 308);
            this.stackInit.TabIndex = 1;
            this.stackInit.Text = "stackInit";
            this.stackInit.Vertical = true;
            // 
            // labelValueUseExtModule
            // 
            this.labelValueUseExtModule.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueUseExtModule.Location = new System.Drawing.Point(4, 285);
            this.labelValueUseExtModule.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueUseExtModule.Name = "labelValueUseExtModule";
            this.labelValueUseExtModule.Size = new System.Drawing.Size(292, 24);
            this.labelValueUseExtModule.TabIndex = 12;
            this.labelValueUseExtModule.Text = "-";
            // 
            // labelUseExtModule
            // 
            this.labelUseExtModule.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelUseExtModule.Location = new System.Drawing.Point(4, 263);
            this.labelUseExtModule.Margin = new System.Windows.Forms.Padding(0);
            this.labelUseExtModule.Name = "labelUseExtModule";
            this.labelUseExtModule.Size = new System.Drawing.Size(292, 22);
            this.labelUseExtModule.TabIndex = 11;
            this.labelUseExtModule.Text = "扩展模块";
            // 
            // labelValueOpenConfig
            // 
            this.labelValueOpenConfig.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueOpenConfig.Location = new System.Drawing.Point(4, 239);
            this.labelValueOpenConfig.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueOpenConfig.Name = "labelValueOpenConfig";
            this.labelValueOpenConfig.Size = new System.Drawing.Size(292, 24);
            this.labelValueOpenConfig.TabIndex = 10;
            this.labelValueOpenConfig.Text = "-";
            this.labelValueOpenConfig.Visible = false;
            // 
            // labelOpenConfig
            // 
            this.labelOpenConfig.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelOpenConfig.Location = new System.Drawing.Point(4, 217);
            this.labelOpenConfig.Margin = new System.Windows.Forms.Padding(0);
            this.labelOpenConfig.Name = "labelOpenConfig";
            this.labelOpenConfig.Size = new System.Drawing.Size(292, 22);
            this.labelOpenConfig.TabIndex = 9;
            this.labelOpenConfig.Text = "打开模式参数";
            this.labelOpenConfig.Visible = false;
            // 
            // labelValueInitOrder
            // 
            this.labelValueInitOrder.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueInitOrder.Location = new System.Drawing.Point(4, 193);
            this.labelValueInitOrder.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueInitOrder.Name = "labelValueInitOrder";
            this.labelValueInitOrder.Size = new System.Drawing.Size(292, 24);
            this.labelValueInitOrder.TabIndex = 8;
            this.labelValueInitOrder.Text = "-";
            // 
            // labelInitOrder
            // 
            this.labelInitOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelInitOrder.Location = new System.Drawing.Point(4, 171);
            this.labelInitOrder.Margin = new System.Windows.Forms.Padding(0);
            this.labelInitOrder.Name = "labelInitOrder";
            this.labelInitOrder.Size = new System.Drawing.Size(292, 22);
            this.labelInitOrder.TabIndex = 7;
            this.labelInitOrder.Text = "初始化顺序";
            // 
            // labelValueModeParam
            // 
            this.labelValueModeParam.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueModeParam.Location = new System.Drawing.Point(4, 147);
            this.labelValueModeParam.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueModeParam.Name = "labelValueModeParam";
            this.labelValueModeParam.Size = new System.Drawing.Size(292, 24);
            this.labelValueModeParam.TabIndex = 6;
            this.labelValueModeParam.Text = "-";
            // 
            // labelModeParam
            // 
            this.labelModeParam.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelModeParam.Location = new System.Drawing.Point(4, 125);
            this.labelModeParam.Margin = new System.Windows.Forms.Padding(0);
            this.labelModeParam.Name = "labelModeParam";
            this.labelModeParam.Size = new System.Drawing.Size(292, 22);
            this.labelModeParam.TabIndex = 5;
            this.labelModeParam.Text = "打开模式参数";
            // 
            // labelValueAxisCount
            // 
            this.labelValueAxisCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueAxisCount.Location = new System.Drawing.Point(4, 101);
            this.labelValueAxisCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueAxisCount.Name = "labelValueAxisCount";
            this.labelValueAxisCount.Size = new System.Drawing.Size(292, 24);
            this.labelValueAxisCount.TabIndex = 4;
            this.labelValueAxisCount.Text = "-";
            // 
            // labelAxisCount
            // 
            this.labelAxisCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelAxisCount.Location = new System.Drawing.Point(4, 79);
            this.labelAxisCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelAxisCount.Name = "labelAxisCount";
            this.labelAxisCount.Size = new System.Drawing.Size(292, 22);
            this.labelAxisCount.TabIndex = 3;
            this.labelAxisCount.Text = "支持轴总数";
            // 
            // labelValueCoreNumber
            // 
            this.labelValueCoreNumber.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueCoreNumber.Location = new System.Drawing.Point(4, 55);
            this.labelValueCoreNumber.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueCoreNumber.Name = "labelValueCoreNumber";
            this.labelValueCoreNumber.Size = new System.Drawing.Size(292, 24);
            this.labelValueCoreNumber.TabIndex = 2;
            this.labelValueCoreNumber.Text = "-";
            // 
            // labelCoreNumber
            // 
            this.labelCoreNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelCoreNumber.Location = new System.Drawing.Point(4, 33);
            this.labelCoreNumber.Margin = new System.Windows.Forms.Padding(0);
            this.labelCoreNumber.Name = "labelCoreNumber";
            this.labelCoreNumber.Size = new System.Drawing.Size(292, 22);
            this.labelCoreNumber.TabIndex = 1;
            this.labelCoreNumber.Text = "内核数量";
            // 
            // dividerSectionInit
            // 
            this.dividerSectionInit.Location = new System.Drawing.Point(7, 7);
            this.dividerSectionInit.Name = "dividerSectionInit";
            this.dividerSectionInit.Size = new System.Drawing.Size(286, 23);
            this.dividerSectionInit.TabIndex = 14;
            this.dividerSectionInit.Text = "初始化参数";
            // 
            // stackBasic
            // 
            this.stackBasic.AutoScroll = true;
            this.stackBasic.Controls.Add(this.labelValueDriverKey);
            this.stackBasic.Controls.Add(this.labelDriverKey);
            this.stackBasic.Controls.Add(this.labelValueName);
            this.stackBasic.Controls.Add(this.labelName);
            this.stackBasic.Controls.Add(this.labelValueDisplayName);
            this.stackBasic.Controls.Add(this.labelDisplayName);
            this.stackBasic.Controls.Add(this.labelValueCardType);
            this.stackBasic.Controls.Add(this.labelCardType);
            this.stackBasic.Controls.Add(this.labelValueCardId);
            this.stackBasic.Controls.Add(this.labelCardId);
            this.stackBasic.Controls.Add(this.dividerSectionBasic);
            this.stackBasic.Location = new System.Drawing.Point(0, 0);
            this.stackBasic.Margin = new System.Windows.Forms.Padding(0);
            this.stackBasic.Name = "stackBasic";
            this.stackBasic.Padding = new System.Windows.Forms.Padding(4);
            this.stackBasic.Size = new System.Drawing.Size(300, 308);
            this.stackBasic.TabIndex = 0;
            this.stackBasic.Text = "stackBasic";
            this.stackBasic.Vertical = true;
            // 
            // labelValueDriverKey
            // 
            this.labelValueDriverKey.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueDriverKey.Location = new System.Drawing.Point(4, 239);
            this.labelValueDriverKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueDriverKey.Name = "labelValueDriverKey";
            this.labelValueDriverKey.Size = new System.Drawing.Size(292, 24);
            this.labelValueDriverKey.TabIndex = 10;
            this.labelValueDriverKey.Text = "-";
            // 
            // labelDriverKey
            // 
            this.labelDriverKey.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelDriverKey.Location = new System.Drawing.Point(4, 217);
            this.labelDriverKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelDriverKey.Name = "labelDriverKey";
            this.labelDriverKey.Size = new System.Drawing.Size(292, 22);
            this.labelDriverKey.TabIndex = 9;
            this.labelDriverKey.Text = "驱动识别键";
            // 
            // labelValueName
            // 
            this.labelValueName.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueName.Location = new System.Drawing.Point(4, 193);
            this.labelValueName.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueName.Name = "labelValueName";
            this.labelValueName.Size = new System.Drawing.Size(292, 24);
            this.labelValueName.TabIndex = 8;
            this.labelValueName.Text = "-";
            // 
            // labelName
            // 
            this.labelName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelName.Location = new System.Drawing.Point(4, 171);
            this.labelName.Margin = new System.Windows.Forms.Padding(0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(292, 22);
            this.labelName.TabIndex = 7;
            this.labelName.Text = "内部名称";
            // 
            // labelValueDisplayName
            // 
            this.labelValueDisplayName.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueDisplayName.Location = new System.Drawing.Point(4, 147);
            this.labelValueDisplayName.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueDisplayName.Name = "labelValueDisplayName";
            this.labelValueDisplayName.Size = new System.Drawing.Size(292, 24);
            this.labelValueDisplayName.TabIndex = 6;
            this.labelValueDisplayName.Text = "-";
            // 
            // labelDisplayName
            // 
            this.labelDisplayName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelDisplayName.Location = new System.Drawing.Point(4, 125);
            this.labelDisplayName.Margin = new System.Windows.Forms.Padding(0);
            this.labelDisplayName.Name = "labelDisplayName";
            this.labelDisplayName.Size = new System.Drawing.Size(292, 22);
            this.labelDisplayName.TabIndex = 5;
            this.labelDisplayName.Text = "显示名称";
            // 
            // labelValueCardType
            // 
            this.labelValueCardType.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueCardType.Location = new System.Drawing.Point(4, 101);
            this.labelValueCardType.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueCardType.Name = "labelValueCardType";
            this.labelValueCardType.Size = new System.Drawing.Size(292, 24);
            this.labelValueCardType.TabIndex = 4;
            this.labelValueCardType.Text = "-";
            // 
            // labelCardType
            // 
            this.labelCardType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelCardType.Location = new System.Drawing.Point(4, 79);
            this.labelCardType.Margin = new System.Windows.Forms.Padding(0);
            this.labelCardType.Name = "labelCardType";
            this.labelCardType.Size = new System.Drawing.Size(292, 22);
            this.labelCardType.TabIndex = 3;
            this.labelCardType.Text = "控制卡类型";
            // 
            // labelValueCardId
            // 
            this.labelValueCardId.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueCardId.Location = new System.Drawing.Point(4, 55);
            this.labelValueCardId.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueCardId.Name = "labelValueCardId";
            this.labelValueCardId.Size = new System.Drawing.Size(292, 24);
            this.labelValueCardId.TabIndex = 2;
            this.labelValueCardId.Text = "-";
            // 
            // labelCardId
            // 
            this.labelCardId.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelCardId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelCardId.Location = new System.Drawing.Point(4, 33);
            this.labelCardId.Margin = new System.Windows.Forms.Padding(0);
            this.labelCardId.Name = "labelCardId";
            this.labelCardId.Size = new System.Drawing.Size(292, 22);
            this.labelCardId.TabIndex = 1;
            this.labelCardId.Text = "卡号（CardId）";
            // 
            // dividerSectionBasic
            // 
            this.dividerSectionBasic.Location = new System.Drawing.Point(7, 7);
            this.dividerSectionBasic.Name = "dividerSectionBasic";
            this.dividerSectionBasic.Size = new System.Drawing.Size(286, 23);
            this.dividerSectionBasic.TabIndex = 12;
            this.dividerSectionBasic.Text = "基础标识";
            // 
            // MotionCardDetailControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MotionCardDetailControl";
            this.Size = new System.Drawing.Size(600, 560);
            this.panelRoot.ResumeLayout(false);
            this.gridDetails.ResumeLayout(false);
            this.stackRemark.ResumeLayout(false);
            this.stackState.ResumeLayout(false);
            this.stackInit.ResumeLayout(false);
            this.stackBasic.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AntdUI.Panel panelRoot;
        private AntdUI.GridPanel gridDetails;
        private AntdUI.StackPanel stackBasic;
        private AntdUI.StackPanel stackInit;
        private AntdUI.StackPanel stackState;
        private AntdUI.StackPanel stackRemark;

        private AntdUI.Label labelCardId;
        private AntdUI.Label labelValueCardId;
        private AntdUI.Label labelCardType;
        private AntdUI.Label labelValueCardType;
        private AntdUI.Label labelDisplayName;
        private AntdUI.Label labelValueDisplayName;
        private AntdUI.Label labelName;
        private AntdUI.Label labelValueName;
        private AntdUI.Label labelDriverKey;
        private AntdUI.Label labelValueDriverKey;

        private AntdUI.Label labelCoreNumber;
        private AntdUI.Label labelValueCoreNumber;
        private AntdUI.Label labelAxisCount;
        private AntdUI.Label labelValueAxisCount;
        private AntdUI.Label labelModeParam;
        private AntdUI.Label labelValueModeParam;
        private AntdUI.Label labelInitOrder;
        private AntdUI.Label labelValueInitOrder;
        private AntdUI.Label labelOpenConfig;
        private AntdUI.Label labelValueOpenConfig;
        private AntdUI.Label labelUseExtModule;
        private AntdUI.Label labelValueUseExtModule;

        private AntdUI.Label labelEnabled;
        private AntdUI.Label labelValueEnabled;
        private AntdUI.Label labelSortOrder;
        private AntdUI.Label labelValueSortOrder;
        private AntdUI.Label labelUpdateTime;
        private AntdUI.Label labelValueUpdateTime;

        private AntdUI.Label labelDescription;
        private AntdUI.Label labelValueDescription;
        private AntdUI.Label labelRemark;
        private AntdUI.Label labelValueRemark;
        private AntdUI.Divider dividerSectionBasic;
        private AntdUI.Divider dividerSectionRemark;
        private AntdUI.Divider dividerSectionState;
        private AntdUI.Divider dividerSectionInit;
    }
}