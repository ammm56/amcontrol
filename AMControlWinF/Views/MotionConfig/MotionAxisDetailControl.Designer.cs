namespace AMControlWinF.Views.MotionConfig
{
    partial class MotionAxisDetailControl
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
            this.stackMapping = new AntdUI.StackPanel();
            this.labelValuePhysicalAxis = new AntdUI.Label();
            this.labelPhysicalAxis = new AntdUI.Label();
            this.labelValuePhysicalCore = new AntdUI.Label();
            this.labelPhysicalCore = new AntdUI.Label();
            this.labelValueAxisId = new AntdUI.Label();
            this.labelAxisId = new AntdUI.Label();
            this.dividerSectionMapping = new AntdUI.Divider();
            this.stackBasic = new AntdUI.StackPanel();
            this.labelValueCard = new AntdUI.Label();
            this.labelCard = new AntdUI.Label();
            this.labelValueName = new AntdUI.Label();
            this.labelName = new AntdUI.Label();
            this.labelValueDisplayName = new AntdUI.Label();
            this.labelDisplayName = new AntdUI.Label();
            this.labelValueAxisCategory = new AntdUI.Label();
            this.labelAxisCategory = new AntdUI.Label();
            this.labelValueLogicalAxis = new AntdUI.Label();
            this.labelLogicalAxis = new AntdUI.Label();
            this.dividerSectionBasic = new AntdUI.Divider();
            this.panelRoot.SuspendLayout();
            this.gridDetails.SuspendLayout();
            this.stackRemark.SuspendLayout();
            this.stackState.SuspendLayout();
            this.stackMapping.SuspendLayout();
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
            this.panelRoot.Size = new System.Drawing.Size(600, 520);
            this.panelRoot.TabIndex = 0;
            this.panelRoot.Text = "panelRoot";
            // 
            // gridDetails
            // 
            this.gridDetails.Controls.Add(this.stackRemark);
            this.gridDetails.Controls.Add(this.stackState);
            this.gridDetails.Controls.Add(this.stackMapping);
            this.gridDetails.Controls.Add(this.stackBasic);
            this.gridDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDetails.Location = new System.Drawing.Point(0, 0);
            this.gridDetails.Margin = new System.Windows.Forms.Padding(0);
            this.gridDetails.Name = "gridDetails";
            this.gridDetails.Size = new System.Drawing.Size(600, 520);
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
            this.stackRemark.Location = new System.Drawing.Point(300, 286);
            this.stackRemark.Margin = new System.Windows.Forms.Padding(0);
            this.stackRemark.Name = "stackRemark";
            this.stackRemark.Padding = new System.Windows.Forms.Padding(4);
            this.stackRemark.Size = new System.Drawing.Size(300, 234);
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
            this.dividerSectionRemark.TabIndex = 0;
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
            this.stackState.Location = new System.Drawing.Point(0, 286);
            this.stackState.Margin = new System.Windows.Forms.Padding(0);
            this.stackState.Name = "stackState";
            this.stackState.Padding = new System.Windows.Forms.Padding(4);
            this.stackState.Size = new System.Drawing.Size(300, 234);
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
            this.dividerSectionState.TabIndex = 0;
            this.dividerSectionState.Text = "状态与排序";
            // 
            // stackMapping
            // 
            this.stackMapping.AutoScroll = true;
            this.stackMapping.Controls.Add(this.labelValuePhysicalAxis);
            this.stackMapping.Controls.Add(this.labelPhysicalAxis);
            this.stackMapping.Controls.Add(this.labelValuePhysicalCore);
            this.stackMapping.Controls.Add(this.labelPhysicalCore);
            this.stackMapping.Controls.Add(this.labelValueAxisId);
            this.stackMapping.Controls.Add(this.labelAxisId);
            this.stackMapping.Controls.Add(this.dividerSectionMapping);
            this.stackMapping.Location = new System.Drawing.Point(300, 0);
            this.stackMapping.Margin = new System.Windows.Forms.Padding(0);
            this.stackMapping.Name = "stackMapping";
            this.stackMapping.Padding = new System.Windows.Forms.Padding(4);
            this.stackMapping.Size = new System.Drawing.Size(300, 286);
            this.stackMapping.TabIndex = 1;
            this.stackMapping.Text = "stackMapping";
            this.stackMapping.Vertical = true;
            // 
            // labelValuePhysicalAxis
            // 
            this.labelValuePhysicalAxis.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValuePhysicalAxis.Location = new System.Drawing.Point(4, 147);
            this.labelValuePhysicalAxis.Margin = new System.Windows.Forms.Padding(0);
            this.labelValuePhysicalAxis.Name = "labelValuePhysicalAxis";
            this.labelValuePhysicalAxis.Size = new System.Drawing.Size(292, 24);
            this.labelValuePhysicalAxis.TabIndex = 6;
            this.labelValuePhysicalAxis.Text = "-";
            // 
            // labelPhysicalAxis
            // 
            this.labelPhysicalAxis.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelPhysicalAxis.Location = new System.Drawing.Point(4, 125);
            this.labelPhysicalAxis.Margin = new System.Windows.Forms.Padding(0);
            this.labelPhysicalAxis.Name = "labelPhysicalAxis";
            this.labelPhysicalAxis.Size = new System.Drawing.Size(292, 22);
            this.labelPhysicalAxis.TabIndex = 5;
            this.labelPhysicalAxis.Text = "物理轴号";
            // 
            // labelValuePhysicalCore
            // 
            this.labelValuePhysicalCore.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValuePhysicalCore.Location = new System.Drawing.Point(4, 101);
            this.labelValuePhysicalCore.Margin = new System.Windows.Forms.Padding(0);
            this.labelValuePhysicalCore.Name = "labelValuePhysicalCore";
            this.labelValuePhysicalCore.Size = new System.Drawing.Size(292, 24);
            this.labelValuePhysicalCore.TabIndex = 4;
            this.labelValuePhysicalCore.Text = "-";
            // 
            // labelPhysicalCore
            // 
            this.labelPhysicalCore.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelPhysicalCore.Location = new System.Drawing.Point(4, 79);
            this.labelPhysicalCore.Margin = new System.Windows.Forms.Padding(0);
            this.labelPhysicalCore.Name = "labelPhysicalCore";
            this.labelPhysicalCore.Size = new System.Drawing.Size(292, 22);
            this.labelPhysicalCore.TabIndex = 3;
            this.labelPhysicalCore.Text = "物理内核号";
            // 
            // labelValueAxisId
            // 
            this.labelValueAxisId.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueAxisId.Location = new System.Drawing.Point(4, 55);
            this.labelValueAxisId.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueAxisId.Name = "labelValueAxisId";
            this.labelValueAxisId.Size = new System.Drawing.Size(292, 24);
            this.labelValueAxisId.TabIndex = 2;
            this.labelValueAxisId.Text = "-";
            // 
            // labelAxisId
            // 
            this.labelAxisId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelAxisId.Location = new System.Drawing.Point(4, 33);
            this.labelAxisId.Margin = new System.Windows.Forms.Padding(0);
            this.labelAxisId.Name = "labelAxisId";
            this.labelAxisId.Size = new System.Drawing.Size(292, 22);
            this.labelAxisId.TabIndex = 1;
            this.labelAxisId.Text = "卡内轴号（AxisId）";
            // 
            // dividerSectionMapping
            // 
            this.dividerSectionMapping.Location = new System.Drawing.Point(7, 7);
            this.dividerSectionMapping.Name = "dividerSectionMapping";
            this.dividerSectionMapping.Size = new System.Drawing.Size(286, 23);
            this.dividerSectionMapping.TabIndex = 0;
            this.dividerSectionMapping.Text = "归属与映射";
            // 
            // stackBasic
            // 
            this.stackBasic.AutoScroll = true;
            this.stackBasic.Controls.Add(this.labelValueCard);
            this.stackBasic.Controls.Add(this.labelCard);
            this.stackBasic.Controls.Add(this.labelValueName);
            this.stackBasic.Controls.Add(this.labelName);
            this.stackBasic.Controls.Add(this.labelValueDisplayName);
            this.stackBasic.Controls.Add(this.labelDisplayName);
            this.stackBasic.Controls.Add(this.labelValueAxisCategory);
            this.stackBasic.Controls.Add(this.labelAxisCategory);
            this.stackBasic.Controls.Add(this.labelValueLogicalAxis);
            this.stackBasic.Controls.Add(this.labelLogicalAxis);
            this.stackBasic.Controls.Add(this.dividerSectionBasic);
            this.stackBasic.Location = new System.Drawing.Point(0, 0);
            this.stackBasic.Margin = new System.Windows.Forms.Padding(0);
            this.stackBasic.Name = "stackBasic";
            this.stackBasic.Padding = new System.Windows.Forms.Padding(4);
            this.stackBasic.Size = new System.Drawing.Size(300, 286);
            this.stackBasic.TabIndex = 0;
            this.stackBasic.Text = "stackBasic";
            this.stackBasic.Vertical = true;
            // 
            // labelValueCard
            // 
            this.labelValueCard.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueCard.Location = new System.Drawing.Point(4, 239);
            this.labelValueCard.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueCard.Name = "labelValueCard";
            this.labelValueCard.Size = new System.Drawing.Size(292, 24);
            this.labelValueCard.TabIndex = 10;
            this.labelValueCard.Text = "-";
            // 
            // labelCard
            // 
            this.labelCard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelCard.Location = new System.Drawing.Point(4, 217);
            this.labelCard.Margin = new System.Windows.Forms.Padding(0);
            this.labelCard.Name = "labelCard";
            this.labelCard.Size = new System.Drawing.Size(292, 22);
            this.labelCard.TabIndex = 9;
            this.labelCard.Text = "所属控制卡";
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
            // labelValueAxisCategory
            // 
            this.labelValueAxisCategory.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueAxisCategory.Location = new System.Drawing.Point(4, 101);
            this.labelValueAxisCategory.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueAxisCategory.Name = "labelValueAxisCategory";
            this.labelValueAxisCategory.Size = new System.Drawing.Size(292, 24);
            this.labelValueAxisCategory.TabIndex = 4;
            this.labelValueAxisCategory.Text = "-";
            // 
            // labelAxisCategory
            // 
            this.labelAxisCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelAxisCategory.Location = new System.Drawing.Point(4, 79);
            this.labelAxisCategory.Margin = new System.Windows.Forms.Padding(0);
            this.labelAxisCategory.Name = "labelAxisCategory";
            this.labelAxisCategory.Size = new System.Drawing.Size(292, 22);
            this.labelAxisCategory.TabIndex = 3;
            this.labelAxisCategory.Text = "轴分类";
            // 
            // labelValueLogicalAxis
            // 
            this.labelValueLogicalAxis.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueLogicalAxis.Location = new System.Drawing.Point(4, 55);
            this.labelValueLogicalAxis.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueLogicalAxis.Name = "labelValueLogicalAxis";
            this.labelValueLogicalAxis.Size = new System.Drawing.Size(292, 24);
            this.labelValueLogicalAxis.TabIndex = 2;
            this.labelValueLogicalAxis.Text = "-";
            // 
            // labelLogicalAxis
            // 
            this.labelLogicalAxis.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelLogicalAxis.Location = new System.Drawing.Point(4, 33);
            this.labelLogicalAxis.Margin = new System.Windows.Forms.Padding(0);
            this.labelLogicalAxis.Name = "labelLogicalAxis";
            this.labelLogicalAxis.Size = new System.Drawing.Size(292, 22);
            this.labelLogicalAxis.TabIndex = 1;
            this.labelLogicalAxis.Text = "逻辑轴号";
            // 
            // dividerSectionBasic
            // 
            this.dividerSectionBasic.Location = new System.Drawing.Point(7, 7);
            this.dividerSectionBasic.Name = "dividerSectionBasic";
            this.dividerSectionBasic.Size = new System.Drawing.Size(286, 23);
            this.dividerSectionBasic.TabIndex = 0;
            this.dividerSectionBasic.Text = "基础标识";
            // 
            // MotionAxisDetailControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MotionAxisDetailControl";
            this.Size = new System.Drawing.Size(600, 520);
            this.panelRoot.ResumeLayout(false);
            this.gridDetails.ResumeLayout(false);
            this.stackRemark.ResumeLayout(false);
            this.stackState.ResumeLayout(false);
            this.stackMapping.ResumeLayout(false);
            this.stackBasic.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AntdUI.Panel panelRoot;
        private AntdUI.GridPanel gridDetails;
        private AntdUI.StackPanel stackBasic;
        private AntdUI.StackPanel stackMapping;
        private AntdUI.StackPanel stackState;
        private AntdUI.StackPanel stackRemark;

        private AntdUI.Divider dividerSectionBasic;
        private AntdUI.Divider dividerSectionMapping;
        private AntdUI.Divider dividerSectionState;
        private AntdUI.Divider dividerSectionRemark;

        private AntdUI.Label labelLogicalAxis;
        private AntdUI.Label labelValueLogicalAxis;
        private AntdUI.Label labelAxisCategory;
        private AntdUI.Label labelValueAxisCategory;
        private AntdUI.Label labelDisplayName;
        private AntdUI.Label labelValueDisplayName;
        private AntdUI.Label labelName;
        private AntdUI.Label labelValueName;
        private AntdUI.Label labelCard;
        private AntdUI.Label labelValueCard;

        private AntdUI.Label labelAxisId;
        private AntdUI.Label labelValueAxisId;
        private AntdUI.Label labelPhysicalCore;
        private AntdUI.Label labelValuePhysicalCore;
        private AntdUI.Label labelPhysicalAxis;
        private AntdUI.Label labelValuePhysicalAxis;

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
    }
}