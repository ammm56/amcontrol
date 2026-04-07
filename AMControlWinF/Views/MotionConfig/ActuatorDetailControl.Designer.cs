namespace AMControlWinF.Views.MotionConfig
{
    partial class ActuatorDetailControl
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
            this.dividerSectionRemark = new AntdUI.Divider();
            this.stackState = new AntdUI.StackPanel();
            this.dividerSectionState = new AntdUI.Divider();
            this.stackMapping = new AntdUI.StackPanel();
            this.dividerSectionMapping = new AntdUI.Divider();
            this.stackBasic = new AntdUI.StackPanel();
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
            this.panelRoot.Size = new System.Drawing.Size(550, 560);
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
            this.gridDetails.Size = new System.Drawing.Size(550, 560);
            this.gridDetails.Span = "50% 50%;50% 50%;-55%";
            this.gridDetails.TabIndex = 0;
            this.gridDetails.Text = "gridDetails";
            // 
            // stackRemark
            // 
            this.stackRemark.AutoScroll = true;
            this.stackRemark.Controls.Add(this.dividerSectionRemark);
            this.stackRemark.Location = new System.Drawing.Point(275, 308);
            this.stackRemark.Margin = new System.Windows.Forms.Padding(0);
            this.stackRemark.Name = "stackRemark";
            this.stackRemark.Padding = new System.Windows.Forms.Padding(4);
            this.stackRemark.Size = new System.Drawing.Size(275, 252);
            this.stackRemark.TabIndex = 3;
            this.stackRemark.Text = "stackRemark";
            this.stackRemark.Vertical = true;
            // 
            // dividerSectionRemark
            // 
            this.dividerSectionRemark.Location = new System.Drawing.Point(7, 7);
            this.dividerSectionRemark.Name = "dividerSectionRemark";
            this.dividerSectionRemark.Size = new System.Drawing.Size(261, 23);
            this.dividerSectionRemark.TabIndex = 0;
            this.dividerSectionRemark.Text = "说明与备注";
            // 
            // stackState
            // 
            this.stackState.AutoScroll = true;
            this.stackState.Controls.Add(this.dividerSectionState);
            this.stackState.Location = new System.Drawing.Point(0, 308);
            this.stackState.Margin = new System.Windows.Forms.Padding(0);
            this.stackState.Name = "stackState";
            this.stackState.Padding = new System.Windows.Forms.Padding(4);
            this.stackState.Size = new System.Drawing.Size(275, 252);
            this.stackState.TabIndex = 2;
            this.stackState.Text = "stackState";
            this.stackState.Vertical = true;
            // 
            // dividerSectionState
            // 
            this.dividerSectionState.Location = new System.Drawing.Point(7, 7);
            this.dividerSectionState.Name = "dividerSectionState";
            this.dividerSectionState.Size = new System.Drawing.Size(261, 23);
            this.dividerSectionState.TabIndex = 0;
            this.dividerSectionState.Text = "状态与参数";
            // 
            // stackMapping
            // 
            this.stackMapping.AutoScroll = true;
            this.stackMapping.Controls.Add(this.dividerSectionMapping);
            this.stackMapping.Location = new System.Drawing.Point(275, 0);
            this.stackMapping.Margin = new System.Windows.Forms.Padding(0);
            this.stackMapping.Name = "stackMapping";
            this.stackMapping.Padding = new System.Windows.Forms.Padding(4);
            this.stackMapping.Size = new System.Drawing.Size(275, 308);
            this.stackMapping.TabIndex = 1;
            this.stackMapping.Text = "stackMapping";
            this.stackMapping.Vertical = true;
            // 
            // dividerSectionMapping
            // 
            this.dividerSectionMapping.Location = new System.Drawing.Point(7, 7);
            this.dividerSectionMapping.Name = "dividerSectionMapping";
            this.dividerSectionMapping.Size = new System.Drawing.Size(261, 23);
            this.dividerSectionMapping.TabIndex = 0;
            this.dividerSectionMapping.Text = "IO与映射";
            // 
            // stackBasic
            // 
            this.stackBasic.AutoScroll = true;
            this.stackBasic.Controls.Add(this.dividerSectionBasic);
            this.stackBasic.Location = new System.Drawing.Point(0, 0);
            this.stackBasic.Margin = new System.Windows.Forms.Padding(0);
            this.stackBasic.Name = "stackBasic";
            this.stackBasic.Padding = new System.Windows.Forms.Padding(4);
            this.stackBasic.Size = new System.Drawing.Size(275, 308);
            this.stackBasic.TabIndex = 0;
            this.stackBasic.Text = "stackBasic";
            this.stackBasic.Vertical = true;
            // 
            // dividerSectionBasic
            // 
            this.dividerSectionBasic.Location = new System.Drawing.Point(7, 7);
            this.dividerSectionBasic.Name = "dividerSectionBasic";
            this.dividerSectionBasic.Size = new System.Drawing.Size(261, 23);
            this.dividerSectionBasic.TabIndex = 0;
            this.dividerSectionBasic.Text = "基础信息";
            // 
            // ActuatorDetailControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ActuatorDetailControl";
            this.Size = new System.Drawing.Size(550, 560);
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
        private AntdUI.Divider dividerSectionBasic;
        private AntdUI.StackPanel stackMapping;
        private AntdUI.Divider dividerSectionMapping;
        private AntdUI.StackPanel stackState;
        private AntdUI.Divider dividerSectionState;
        private AntdUI.StackPanel stackRemark;
        private AntdUI.Divider dividerSectionRemark;
    }
}