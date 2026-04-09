namespace AMControlWinF.Views.MotionConfig
{
    partial class MotionAxisParamDetailControl
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
            this.labelValueVendor = new AntdUI.Label();
            this.labelVendor = new AntdUI.Label();
            this.labelValueValueDescription = new AntdUI.Label();
            this.labelValueDescriptionTitle = new AntdUI.Label();
            this.dividerSectionState = new AntdUI.Divider();
            this.stackMapping = new AntdUI.StackPanel();
            this.labelValueUnit = new AntdUI.Label();
            this.labelUnit = new AntdUI.Label();
            this.labelValueRange = new AntdUI.Label();
            this.labelRange = new AntdUI.Label();
            this.labelValueDefault = new AntdUI.Label();
            this.labelDefault = new AntdUI.Label();
            this.labelValueCurrent = new AntdUI.Label();
            this.labelCurrent = new AntdUI.Label();
            this.dividerSectionMapping = new AntdUI.Divider();
            this.stackBasic = new AntdUI.StackPanel();
            this.labelValueType = new AntdUI.Label();
            this.labelType = new AntdUI.Label();
            this.labelValueParamName = new AntdUI.Label();
            this.labelParamName = new AntdUI.Label();
            this.labelValueName = new AntdUI.Label();
            this.labelName = new AntdUI.Label();
            this.labelValueGroup = new AntdUI.Label();
            this.labelGroup = new AntdUI.Label();
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
            this.panelRoot.Size = new System.Drawing.Size(560, 460);
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
            this.gridDetails.Size = new System.Drawing.Size(560, 460);
            this.gridDetails.Span = "50% 50%;50% 50%;-50%";
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
            this.stackRemark.Location = new System.Drawing.Point(280, 253);
            this.stackRemark.Margin = new System.Windows.Forms.Padding(0);
            this.stackRemark.Name = "stackRemark";
            this.stackRemark.Padding = new System.Windows.Forms.Padding(4);
            this.stackRemark.Size = new System.Drawing.Size(280, 207);
            this.stackRemark.TabIndex = 3;
            this.stackRemark.Text = "stackRemark";
            this.stackRemark.Vertical = true;
            // 
            // labelValueRemark
            // 
            this.labelValueRemark.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueRemark.Location = new System.Drawing.Point(4, 147);
            this.labelValueRemark.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueRemark.Name = "labelValueRemark";
            this.labelValueRemark.Size = new System.Drawing.Size(272, 48);
            this.labelValueRemark.TabIndex = 4;
            this.labelValueRemark.Text = "-";
            this.labelValueRemark.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            // 
            // labelRemark
            // 
            this.labelRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelRemark.Location = new System.Drawing.Point(4, 125);
            this.labelRemark.Margin = new System.Windows.Forms.Padding(0);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(272, 22);
            this.labelRemark.TabIndex = 3;
            this.labelRemark.Text = "备注";
            // 
            // labelValueDescription
            // 
            this.labelValueDescription.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueDescription.Location = new System.Drawing.Point(4, 55);
            this.labelValueDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueDescription.Name = "labelValueDescription";
            this.labelValueDescription.Size = new System.Drawing.Size(272, 70);
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
            this.labelDescription.Size = new System.Drawing.Size(272, 22);
            this.labelDescription.TabIndex = 1;
            this.labelDescription.Text = "参数说明";
            // 
            // dividerSectionRemark
            // 
            this.dividerSectionRemark.Location = new System.Drawing.Point(7, 7);
            this.dividerSectionRemark.Name = "dividerSectionRemark";
            this.dividerSectionRemark.Size = new System.Drawing.Size(266, 23);
            this.dividerSectionRemark.TabIndex = 0;
            this.dividerSectionRemark.Text = "说明与备注";
            // 
            // stackState
            // 
            this.stackState.AutoScroll = true;
            this.stackState.Controls.Add(this.labelValueVendor);
            this.stackState.Controls.Add(this.labelVendor);
            this.stackState.Controls.Add(this.labelValueValueDescription);
            this.stackState.Controls.Add(this.labelValueDescriptionTitle);
            this.stackState.Controls.Add(this.dividerSectionState);
            this.stackState.Location = new System.Drawing.Point(0, 253);
            this.stackState.Margin = new System.Windows.Forms.Padding(0);
            this.stackState.Name = "stackState";
            this.stackState.Padding = new System.Windows.Forms.Padding(4);
            this.stackState.Size = new System.Drawing.Size(280, 207);
            this.stackState.TabIndex = 2;
            this.stackState.Text = "stackState";
            this.stackState.Vertical = true;
            // 
            // labelValueVendor
            // 
            this.labelValueVendor.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueVendor.Location = new System.Drawing.Point(4, 147);
            this.labelValueVendor.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueVendor.Name = "labelValueVendor";
            this.labelValueVendor.Size = new System.Drawing.Size(272, 24);
            this.labelValueVendor.TabIndex = 4;
            this.labelValueVendor.Text = "-";
            // 
            // labelVendor
            // 
            this.labelVendor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelVendor.Location = new System.Drawing.Point(4, 125);
            this.labelVendor.Margin = new System.Windows.Forms.Padding(0);
            this.labelVendor.Name = "labelVendor";
            this.labelVendor.Size = new System.Drawing.Size(272, 22);
            this.labelVendor.TabIndex = 3;
            this.labelVendor.Text = "厂商范围";
            // 
            // labelValueValueDescription
            // 
            this.labelValueValueDescription.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueValueDescription.Location = new System.Drawing.Point(4, 55);
            this.labelValueValueDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueValueDescription.Name = "labelValueValueDescription";
            this.labelValueValueDescription.Size = new System.Drawing.Size(272, 70);
            this.labelValueValueDescription.TabIndex = 2;
            this.labelValueValueDescription.Text = "-";
            this.labelValueValueDescription.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            // 
            // labelValueDescriptionTitle
            // 
            this.labelValueDescriptionTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelValueDescriptionTitle.Location = new System.Drawing.Point(4, 33);
            this.labelValueDescriptionTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueDescriptionTitle.Name = "labelValueDescriptionTitle";
            this.labelValueDescriptionTitle.Size = new System.Drawing.Size(272, 22);
            this.labelValueDescriptionTitle.TabIndex = 1;
            this.labelValueDescriptionTitle.Text = "取值说明";
            // 
            // dividerSectionState
            // 
            this.dividerSectionState.Location = new System.Drawing.Point(7, 7);
            this.dividerSectionState.Name = "dividerSectionState";
            this.dividerSectionState.Size = new System.Drawing.Size(266, 23);
            this.dividerSectionState.TabIndex = 0;
            this.dividerSectionState.Text = "约束信息";
            // 
            // stackMapping
            // 
            this.stackMapping.AutoScroll = true;
            this.stackMapping.Controls.Add(this.labelValueUnit);
            this.stackMapping.Controls.Add(this.labelUnit);
            this.stackMapping.Controls.Add(this.labelValueRange);
            this.stackMapping.Controls.Add(this.labelRange);
            this.stackMapping.Controls.Add(this.labelValueDefault);
            this.stackMapping.Controls.Add(this.labelDefault);
            this.stackMapping.Controls.Add(this.labelValueCurrent);
            this.stackMapping.Controls.Add(this.labelCurrent);
            this.stackMapping.Controls.Add(this.dividerSectionMapping);
            this.stackMapping.Location = new System.Drawing.Point(280, 0);
            this.stackMapping.Margin = new System.Windows.Forms.Padding(0);
            this.stackMapping.Name = "stackMapping";
            this.stackMapping.Padding = new System.Windows.Forms.Padding(4);
            this.stackMapping.Size = new System.Drawing.Size(280, 253);
            this.stackMapping.TabIndex = 1;
            this.stackMapping.Text = "stackMapping";
            this.stackMapping.Vertical = true;
            // 
            // labelValueUnit
            // 
            this.labelValueUnit.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueUnit.Location = new System.Drawing.Point(4, 193);
            this.labelValueUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueUnit.Name = "labelValueUnit";
            this.labelValueUnit.Size = new System.Drawing.Size(272, 24);
            this.labelValueUnit.TabIndex = 8;
            this.labelValueUnit.Text = "-";
            // 
            // labelUnit
            // 
            this.labelUnit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelUnit.Location = new System.Drawing.Point(4, 171);
            this.labelUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(272, 22);
            this.labelUnit.TabIndex = 7;
            this.labelUnit.Text = "参数单位";
            // 
            // labelValueRange
            // 
            this.labelValueRange.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueRange.Location = new System.Drawing.Point(4, 147);
            this.labelValueRange.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueRange.Name = "labelValueRange";
            this.labelValueRange.Size = new System.Drawing.Size(272, 24);
            this.labelValueRange.TabIndex = 6;
            this.labelValueRange.Text = "-";
            // 
            // labelRange
            // 
            this.labelRange.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelRange.Location = new System.Drawing.Point(4, 125);
            this.labelRange.Margin = new System.Windows.Forms.Padding(0);
            this.labelRange.Name = "labelRange";
            this.labelRange.Size = new System.Drawing.Size(272, 22);
            this.labelRange.TabIndex = 5;
            this.labelRange.Text = "取值范围";
            // 
            // labelValueDefault
            // 
            this.labelValueDefault.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueDefault.Location = new System.Drawing.Point(4, 101);
            this.labelValueDefault.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueDefault.Name = "labelValueDefault";
            this.labelValueDefault.Size = new System.Drawing.Size(272, 24);
            this.labelValueDefault.TabIndex = 4;
            this.labelValueDefault.Text = "-";
            // 
            // labelDefault
            // 
            this.labelDefault.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelDefault.Location = new System.Drawing.Point(4, 79);
            this.labelDefault.Margin = new System.Windows.Forms.Padding(0);
            this.labelDefault.Name = "labelDefault";
            this.labelDefault.Size = new System.Drawing.Size(272, 22);
            this.labelDefault.TabIndex = 3;
            this.labelDefault.Text = "默认值";
            // 
            // labelValueCurrent
            // 
            this.labelValueCurrent.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueCurrent.Location = new System.Drawing.Point(4, 55);
            this.labelValueCurrent.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueCurrent.Name = "labelValueCurrent";
            this.labelValueCurrent.Size = new System.Drawing.Size(272, 24);
            this.labelValueCurrent.TabIndex = 2;
            this.labelValueCurrent.Text = "-";
            // 
            // labelCurrent
            // 
            this.labelCurrent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelCurrent.Location = new System.Drawing.Point(4, 33);
            this.labelCurrent.Margin = new System.Windows.Forms.Padding(0);
            this.labelCurrent.Name = "labelCurrent";
            this.labelCurrent.Size = new System.Drawing.Size(272, 22);
            this.labelCurrent.TabIndex = 1;
            this.labelCurrent.Text = "当前值";
            // 
            // dividerSectionMapping
            // 
            this.dividerSectionMapping.Location = new System.Drawing.Point(7, 7);
            this.dividerSectionMapping.Name = "dividerSectionMapping";
            this.dividerSectionMapping.Size = new System.Drawing.Size(266, 23);
            this.dividerSectionMapping.TabIndex = 0;
            this.dividerSectionMapping.Text = "参数值";
            // 
            // stackBasic
            // 
            this.stackBasic.AutoScroll = true;
            this.stackBasic.Controls.Add(this.labelValueType);
            this.stackBasic.Controls.Add(this.labelType);
            this.stackBasic.Controls.Add(this.labelValueParamName);
            this.stackBasic.Controls.Add(this.labelParamName);
            this.stackBasic.Controls.Add(this.labelValueName);
            this.stackBasic.Controls.Add(this.labelName);
            this.stackBasic.Controls.Add(this.labelValueGroup);
            this.stackBasic.Controls.Add(this.labelGroup);
            this.stackBasic.Controls.Add(this.dividerSectionBasic);
            this.stackBasic.Location = new System.Drawing.Point(0, 0);
            this.stackBasic.Margin = new System.Windows.Forms.Padding(0);
            this.stackBasic.Name = "stackBasic";
            this.stackBasic.Padding = new System.Windows.Forms.Padding(4);
            this.stackBasic.Size = new System.Drawing.Size(280, 253);
            this.stackBasic.TabIndex = 0;
            this.stackBasic.Text = "stackBasic";
            this.stackBasic.Vertical = true;
            // 
            // labelValueType
            // 
            this.labelValueType.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueType.Location = new System.Drawing.Point(4, 193);
            this.labelValueType.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueType.Name = "labelValueType";
            this.labelValueType.Size = new System.Drawing.Size(272, 24);
            this.labelValueType.TabIndex = 8;
            this.labelValueType.Text = "-";
            // 
            // labelType
            // 
            this.labelType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelType.Location = new System.Drawing.Point(4, 171);
            this.labelType.Margin = new System.Windows.Forms.Padding(0);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(272, 22);
            this.labelType.TabIndex = 7;
            this.labelType.Text = "参数类型";
            // 
            // labelValueParamName
            // 
            this.labelValueParamName.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueParamName.Location = new System.Drawing.Point(4, 147);
            this.labelValueParamName.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueParamName.Name = "labelValueParamName";
            this.labelValueParamName.Size = new System.Drawing.Size(272, 24);
            this.labelValueParamName.TabIndex = 6;
            this.labelValueParamName.Text = "-";
            // 
            // labelParamName
            // 
            this.labelParamName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelParamName.Location = new System.Drawing.Point(4, 125);
            this.labelParamName.Margin = new System.Windows.Forms.Padding(0);
            this.labelParamName.Name = "labelParamName";
            this.labelParamName.Size = new System.Drawing.Size(272, 22);
            this.labelParamName.TabIndex = 5;
            this.labelParamName.Text = "参数名";
            // 
            // labelValueName
            // 
            this.labelValueName.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueName.Location = new System.Drawing.Point(4, 101);
            this.labelValueName.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueName.Name = "labelValueName";
            this.labelValueName.Size = new System.Drawing.Size(272, 24);
            this.labelValueName.TabIndex = 4;
            this.labelValueName.Text = "-";
            // 
            // labelName
            // 
            this.labelName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelName.Location = new System.Drawing.Point(4, 79);
            this.labelName.Margin = new System.Windows.Forms.Padding(0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(272, 22);
            this.labelName.TabIndex = 3;
            this.labelName.Text = "显示名称";
            // 
            // labelValueGroup
            // 
            this.labelValueGroup.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueGroup.Location = new System.Drawing.Point(4, 55);
            this.labelValueGroup.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueGroup.Name = "labelValueGroup";
            this.labelValueGroup.Size = new System.Drawing.Size(272, 24);
            this.labelValueGroup.TabIndex = 2;
            this.labelValueGroup.Text = "-";
            // 
            // labelGroup
            // 
            this.labelGroup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelGroup.Location = new System.Drawing.Point(4, 33);
            this.labelGroup.Margin = new System.Windows.Forms.Padding(0);
            this.labelGroup.Name = "labelGroup";
            this.labelGroup.Size = new System.Drawing.Size(272, 22);
            this.labelGroup.TabIndex = 1;
            this.labelGroup.Text = "参数分组";
            // 
            // dividerSectionBasic
            // 
            this.dividerSectionBasic.Location = new System.Drawing.Point(7, 7);
            this.dividerSectionBasic.Name = "dividerSectionBasic";
            this.dividerSectionBasic.Size = new System.Drawing.Size(266, 23);
            this.dividerSectionBasic.TabIndex = 0;
            this.dividerSectionBasic.Text = "基础信息";
            // 
            // MotionAxisParamDetailControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MotionAxisParamDetailControl";
            this.Size = new System.Drawing.Size(560, 460);
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
        private AntdUI.Label labelGroup;
        private AntdUI.Label labelValueGroup;
        private AntdUI.Label labelName;
        private AntdUI.Label labelValueName;
        private AntdUI.Label labelParamName;
        private AntdUI.Label labelValueParamName;
        private AntdUI.Label labelType;
        private AntdUI.Label labelValueType;
        private AntdUI.Label labelCurrent;
        private AntdUI.Label labelValueCurrent;
        private AntdUI.Label labelDefault;
        private AntdUI.Label labelValueDefault;
        private AntdUI.Label labelRange;
        private AntdUI.Label labelValueRange;
        private AntdUI.Label labelUnit;
        private AntdUI.Label labelValueUnit;
        private AntdUI.Label labelValueDescriptionTitle;
        private AntdUI.Label labelValueValueDescription;
        private AntdUI.Label labelVendor;
        private AntdUI.Label labelValueVendor;
        private AntdUI.Label labelDescription;
        private AntdUI.Label labelValueDescription;
        private AntdUI.Label labelRemark;
        private AntdUI.Label labelValueRemark;
    }
}