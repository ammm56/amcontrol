namespace AMControlWinF.Views.MotionConfig
{
    partial class MotionIoMapDetailControl
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
            this.dividerSectionRemark = new AntdUI.Divider();
            this.stackState = new AntdUI.StackPanel();
            this.labelValueSortOrder = new AntdUI.Label();
            this.labelSortOrder = new AntdUI.Label();
            this.labelValueEnabled = new AntdUI.Label();
            this.labelEnabled = new AntdUI.Label();
            this.dividerSectionState = new AntdUI.Divider();
            this.stackMapping = new AntdUI.StackPanel();
            this.labelValueExtModule = new AntdUI.Label();
            this.labelExtModule = new AntdUI.Label();
            this.labelValueHardwareBit = new AntdUI.Label();
            this.labelHardwareBit = new AntdUI.Label();
            this.labelValueCore = new AntdUI.Label();
            this.labelCore = new AntdUI.Label();
            this.dividerSectionMapping = new AntdUI.Divider();
            this.stackBasic = new AntdUI.StackPanel();
            this.labelValueCard = new AntdUI.Label();
            this.labelCard = new AntdUI.Label();
            this.labelValueLogicalBit = new AntdUI.Label();
            this.labelLogicalBit = new AntdUI.Label();
            this.labelValueName = new AntdUI.Label();
            this.labelName = new AntdUI.Label();
            this.labelValueIoType = new AntdUI.Label();
            this.labelIoType = new AntdUI.Label();
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
            this.panelRoot.Size = new System.Drawing.Size(500, 420);
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
            this.gridDetails.Size = new System.Drawing.Size(500, 420);
            this.gridDetails.Span = "50% 50%;50% 50%;-55%";
            this.gridDetails.TabIndex = 0;
            this.gridDetails.Text = "gridDetails";
            // 
            // stackRemark
            // 
            this.stackRemark.AutoScroll = true;
            this.stackRemark.Controls.Add(this.labelValueRemark);
            this.stackRemark.Controls.Add(this.labelRemark);
            this.stackRemark.Controls.Add(this.dividerSectionRemark);
            this.stackRemark.Location = new System.Drawing.Point(250, 231);
            this.stackRemark.Margin = new System.Windows.Forms.Padding(0);
            this.stackRemark.Name = "stackRemark";
            this.stackRemark.Padding = new System.Windows.Forms.Padding(4);
            this.stackRemark.Size = new System.Drawing.Size(250, 189);
            this.stackRemark.TabIndex = 3;
            this.stackRemark.Text = "stackRemark";
            this.stackRemark.Vertical = true;
            // 
            // labelValueRemark
            // 
            this.labelValueRemark.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueRemark.Location = new System.Drawing.Point(4, 55);
            this.labelValueRemark.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueRemark.Name = "labelValueRemark";
            this.labelValueRemark.Size = new System.Drawing.Size(242, 72);
            this.labelValueRemark.TabIndex = 2;
            this.labelValueRemark.Text = "-";
            this.labelValueRemark.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            // 
            // labelRemark
            // 
            this.labelRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelRemark.Location = new System.Drawing.Point(4, 33);
            this.labelRemark.Margin = new System.Windows.Forms.Padding(0);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(242, 22);
            this.labelRemark.TabIndex = 1;
            this.labelRemark.Text = "备注";
            // 
            // dividerSectionRemark
            // 
            this.dividerSectionRemark.Location = new System.Drawing.Point(7, 7);
            this.dividerSectionRemark.Name = "dividerSectionRemark";
            this.dividerSectionRemark.Size = new System.Drawing.Size(236, 23);
            this.dividerSectionRemark.TabIndex = 0;
            this.dividerSectionRemark.Text = "备注";
            // 
            // stackState
            // 
            this.stackState.AutoScroll = true;
            this.stackState.Controls.Add(this.labelValueSortOrder);
            this.stackState.Controls.Add(this.labelSortOrder);
            this.stackState.Controls.Add(this.labelValueEnabled);
            this.stackState.Controls.Add(this.labelEnabled);
            this.stackState.Controls.Add(this.dividerSectionState);
            this.stackState.Location = new System.Drawing.Point(0, 231);
            this.stackState.Margin = new System.Windows.Forms.Padding(0);
            this.stackState.Name = "stackState";
            this.stackState.Padding = new System.Windows.Forms.Padding(4);
            this.stackState.Size = new System.Drawing.Size(250, 189);
            this.stackState.TabIndex = 2;
            this.stackState.Text = "stackState";
            this.stackState.Vertical = true;
            // 
            // labelValueSortOrder
            // 
            this.labelValueSortOrder.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueSortOrder.Location = new System.Drawing.Point(4, 101);
            this.labelValueSortOrder.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueSortOrder.Name = "labelValueSortOrder";
            this.labelValueSortOrder.Size = new System.Drawing.Size(242, 24);
            this.labelValueSortOrder.TabIndex = 4;
            this.labelValueSortOrder.Text = "-";
            // 
            // labelSortOrder
            // 
            this.labelSortOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelSortOrder.Location = new System.Drawing.Point(4, 79);
            this.labelSortOrder.Margin = new System.Windows.Forms.Padding(0);
            this.labelSortOrder.Name = "labelSortOrder";
            this.labelSortOrder.Size = new System.Drawing.Size(242, 22);
            this.labelSortOrder.TabIndex = 3;
            this.labelSortOrder.Text = "排序号";
            // 
            // labelValueEnabled
            // 
            this.labelValueEnabled.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueEnabled.Location = new System.Drawing.Point(4, 55);
            this.labelValueEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueEnabled.Name = "labelValueEnabled";
            this.labelValueEnabled.Size = new System.Drawing.Size(242, 24);
            this.labelValueEnabled.TabIndex = 2;
            this.labelValueEnabled.Text = "-";
            // 
            // labelEnabled
            // 
            this.labelEnabled.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelEnabled.Location = new System.Drawing.Point(4, 33);
            this.labelEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.labelEnabled.Name = "labelEnabled";
            this.labelEnabled.Size = new System.Drawing.Size(242, 22);
            this.labelEnabled.TabIndex = 1;
            this.labelEnabled.Text = "启用状态";
            // 
            // dividerSectionState
            // 
            this.dividerSectionState.Location = new System.Drawing.Point(7, 7);
            this.dividerSectionState.Name = "dividerSectionState";
            this.dividerSectionState.Size = new System.Drawing.Size(236, 23);
            this.dividerSectionState.TabIndex = 0;
            this.dividerSectionState.Text = "状态与排序";
            // 
            // stackMapping
            // 
            this.stackMapping.AutoScroll = true;
            this.stackMapping.Controls.Add(this.labelValueExtModule);
            this.stackMapping.Controls.Add(this.labelExtModule);
            this.stackMapping.Controls.Add(this.labelValueHardwareBit);
            this.stackMapping.Controls.Add(this.labelHardwareBit);
            this.stackMapping.Controls.Add(this.labelValueCore);
            this.stackMapping.Controls.Add(this.labelCore);
            this.stackMapping.Controls.Add(this.dividerSectionMapping);
            this.stackMapping.Location = new System.Drawing.Point(250, 0);
            this.stackMapping.Margin = new System.Windows.Forms.Padding(0);
            this.stackMapping.Name = "stackMapping";
            this.stackMapping.Padding = new System.Windows.Forms.Padding(4);
            this.stackMapping.Size = new System.Drawing.Size(250, 231);
            this.stackMapping.TabIndex = 1;
            this.stackMapping.Text = "stackMapping";
            this.stackMapping.Vertical = true;
            // 
            // labelValueExtModule
            // 
            this.labelValueExtModule.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueExtModule.Location = new System.Drawing.Point(4, 147);
            this.labelValueExtModule.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueExtModule.Name = "labelValueExtModule";
            this.labelValueExtModule.Size = new System.Drawing.Size(242, 24);
            this.labelValueExtModule.TabIndex = 6;
            this.labelValueExtModule.Text = "-";
            // 
            // labelExtModule
            // 
            this.labelExtModule.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelExtModule.Location = new System.Drawing.Point(4, 125);
            this.labelExtModule.Margin = new System.Windows.Forms.Padding(0);
            this.labelExtModule.Name = "labelExtModule";
            this.labelExtModule.Size = new System.Drawing.Size(242, 22);
            this.labelExtModule.TabIndex = 5;
            this.labelExtModule.Text = "板载 / 扩展";
            // 
            // labelValueHardwareBit
            // 
            this.labelValueHardwareBit.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueHardwareBit.Location = new System.Drawing.Point(4, 101);
            this.labelValueHardwareBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueHardwareBit.Name = "labelValueHardwareBit";
            this.labelValueHardwareBit.Size = new System.Drawing.Size(242, 24);
            this.labelValueHardwareBit.TabIndex = 4;
            this.labelValueHardwareBit.Text = "-";
            // 
            // labelHardwareBit
            // 
            this.labelHardwareBit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelHardwareBit.Location = new System.Drawing.Point(4, 79);
            this.labelHardwareBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelHardwareBit.Name = "labelHardwareBit";
            this.labelHardwareBit.Size = new System.Drawing.Size(242, 22);
            this.labelHardwareBit.TabIndex = 3;
            this.labelHardwareBit.Text = "硬件位号";
            // 
            // labelValueCore
            // 
            this.labelValueCore.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueCore.Location = new System.Drawing.Point(4, 55);
            this.labelValueCore.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueCore.Name = "labelValueCore";
            this.labelValueCore.Size = new System.Drawing.Size(242, 24);
            this.labelValueCore.TabIndex = 2;
            this.labelValueCore.Text = "-";
            // 
            // labelCore
            // 
            this.labelCore.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelCore.Location = new System.Drawing.Point(4, 33);
            this.labelCore.Margin = new System.Windows.Forms.Padding(0);
            this.labelCore.Name = "labelCore";
            this.labelCore.Size = new System.Drawing.Size(242, 22);
            this.labelCore.TabIndex = 1;
            this.labelCore.Text = "所属内核";
            // 
            // dividerSectionMapping
            // 
            this.dividerSectionMapping.Location = new System.Drawing.Point(7, 7);
            this.dividerSectionMapping.Name = "dividerSectionMapping";
            this.dividerSectionMapping.Size = new System.Drawing.Size(236, 23);
            this.dividerSectionMapping.TabIndex = 0;
            this.dividerSectionMapping.Text = "归属与映射";
            // 
            // stackBasic
            // 
            this.stackBasic.AutoScroll = true;
            this.stackBasic.Controls.Add(this.labelValueCard);
            this.stackBasic.Controls.Add(this.labelCard);
            this.stackBasic.Controls.Add(this.labelValueLogicalBit);
            this.stackBasic.Controls.Add(this.labelLogicalBit);
            this.stackBasic.Controls.Add(this.labelValueName);
            this.stackBasic.Controls.Add(this.labelName);
            this.stackBasic.Controls.Add(this.labelValueIoType);
            this.stackBasic.Controls.Add(this.labelIoType);
            this.stackBasic.Controls.Add(this.dividerSectionBasic);
            this.stackBasic.Location = new System.Drawing.Point(0, 0);
            this.stackBasic.Margin = new System.Windows.Forms.Padding(0);
            this.stackBasic.Name = "stackBasic";
            this.stackBasic.Padding = new System.Windows.Forms.Padding(4);
            this.stackBasic.Size = new System.Drawing.Size(250, 231);
            this.stackBasic.TabIndex = 0;
            this.stackBasic.Text = "stackBasic";
            this.stackBasic.Vertical = true;
            // 
            // labelValueCard
            // 
            this.labelValueCard.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueCard.Location = new System.Drawing.Point(4, 193);
            this.labelValueCard.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueCard.Name = "labelValueCard";
            this.labelValueCard.Size = new System.Drawing.Size(242, 24);
            this.labelValueCard.TabIndex = 8;
            this.labelValueCard.Text = "-";
            // 
            // labelCard
            // 
            this.labelCard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelCard.Location = new System.Drawing.Point(4, 171);
            this.labelCard.Margin = new System.Windows.Forms.Padding(0);
            this.labelCard.Name = "labelCard";
            this.labelCard.Size = new System.Drawing.Size(242, 22);
            this.labelCard.TabIndex = 7;
            this.labelCard.Text = "所属控制卡";
            // 
            // labelValueLogicalBit
            // 
            this.labelValueLogicalBit.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueLogicalBit.Location = new System.Drawing.Point(4, 147);
            this.labelValueLogicalBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueLogicalBit.Name = "labelValueLogicalBit";
            this.labelValueLogicalBit.Size = new System.Drawing.Size(242, 24);
            this.labelValueLogicalBit.TabIndex = 6;
            this.labelValueLogicalBit.Text = "-";
            // 
            // labelLogicalBit
            // 
            this.labelLogicalBit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelLogicalBit.Location = new System.Drawing.Point(4, 125);
            this.labelLogicalBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelLogicalBit.Name = "labelLogicalBit";
            this.labelLogicalBit.Size = new System.Drawing.Size(242, 22);
            this.labelLogicalBit.TabIndex = 5;
            this.labelLogicalBit.Text = "逻辑位号";
            // 
            // labelValueName
            // 
            this.labelValueName.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueName.Location = new System.Drawing.Point(4, 101);
            this.labelValueName.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueName.Name = "labelValueName";
            this.labelValueName.Size = new System.Drawing.Size(242, 24);
            this.labelValueName.TabIndex = 4;
            this.labelValueName.Text = "-";
            // 
            // labelName
            // 
            this.labelName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelName.Location = new System.Drawing.Point(4, 79);
            this.labelName.Margin = new System.Windows.Forms.Padding(0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(242, 22);
            this.labelName.TabIndex = 3;
            this.labelName.Text = "名称";
            // 
            // labelValueIoType
            // 
            this.labelValueIoType.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelValueIoType.Location = new System.Drawing.Point(4, 55);
            this.labelValueIoType.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueIoType.Name = "labelValueIoType";
            this.labelValueIoType.Size = new System.Drawing.Size(242, 24);
            this.labelValueIoType.TabIndex = 10;
            this.labelValueIoType.Text = "-";
            // 
            // labelIoType
            // 
            this.labelIoType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.labelIoType.Location = new System.Drawing.Point(4, 33);
            this.labelIoType.Margin = new System.Windows.Forms.Padding(0);
            this.labelIoType.Name = "labelIoType";
            this.labelIoType.Size = new System.Drawing.Size(242, 22);
            this.labelIoType.TabIndex = 9;
            this.labelIoType.Text = "IO 类型";
            // 
            // dividerSectionBasic
            // 
            this.dividerSectionBasic.Location = new System.Drawing.Point(7, 7);
            this.dividerSectionBasic.Name = "dividerSectionBasic";
            this.dividerSectionBasic.Size = new System.Drawing.Size(236, 23);
            this.dividerSectionBasic.TabIndex = 0;
            this.dividerSectionBasic.Text = "基础标识";
            // 
            // MotionIoMapDetailControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MotionIoMapDetailControl";
            this.Size = new System.Drawing.Size(500, 420);
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

        private AntdUI.Label labelName;
        private AntdUI.Label labelValueName;
        private AntdUI.Label labelLogicalBit;
        private AntdUI.Label labelValueLogicalBit;
        private AntdUI.Label labelCard;
        private AntdUI.Label labelValueCard;
        private AntdUI.Label labelIoType;
        private AntdUI.Label labelValueIoType;

        private AntdUI.Label labelCore;
        private AntdUI.Label labelValueCore;
        private AntdUI.Label labelHardwareBit;
        private AntdUI.Label labelValueHardwareBit;
        private AntdUI.Label labelExtModule;
        private AntdUI.Label labelValueExtModule;

        private AntdUI.Label labelEnabled;
        private AntdUI.Label labelValueEnabled;
        private AntdUI.Label labelSortOrder;
        private AntdUI.Label labelValueSortOrder;

        private AntdUI.Label labelRemark;
        private AntdUI.Label labelValueRemark;
    }
}