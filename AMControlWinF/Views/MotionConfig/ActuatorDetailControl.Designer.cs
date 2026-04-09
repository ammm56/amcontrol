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
            this.panelRemark = new AntdUI.Panel();
            this.flowRemark = new AntdUI.FlowPanel();
            this.dividerSectionRemark = new AntdUI.Divider();
            this.panelBasic = new AntdUI.Panel();
            this.flowBasic = new AntdUI.FlowPanel();
            this.dividerSectionBasic = new AntdUI.Divider();
            this.panelMapping = new AntdUI.Panel();
            this.flowMapping = new AntdUI.FlowPanel();
            this.dividerSectionMapping = new AntdUI.Divider();
            this.panelState = new AntdUI.Panel();
            this.flowState = new AntdUI.FlowPanel();
            this.dividerSectionState = new AntdUI.Divider();
            this.panelRoot.SuspendLayout();
            this.gridDetails.SuspendLayout();
            this.panelRemark.SuspendLayout();
            this.panelBasic.SuspendLayout();
            this.panelMapping.SuspendLayout();
            this.panelState.SuspendLayout();
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
            this.panelRoot.Size = new System.Drawing.Size(550, 620);
            this.panelRoot.TabIndex = 0;
            this.panelRoot.Text = "panelRoot";
            // 
            // gridDetails
            // 
            this.gridDetails.Controls.Add(this.panelRemark);
            this.gridDetails.Controls.Add(this.panelBasic);
            this.gridDetails.Controls.Add(this.panelMapping);
            this.gridDetails.Controls.Add(this.panelState);
            this.gridDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDetails.Location = new System.Drawing.Point(0, 0);
            this.gridDetails.Margin = new System.Windows.Forms.Padding(0);
            this.gridDetails.Name = "gridDetails";
            this.gridDetails.Size = new System.Drawing.Size(550, 620);
            this.gridDetails.Span = "50% 50%;50% 50%;-70%";
            this.gridDetails.TabIndex = 0;
            this.gridDetails.Text = "gridDetails";
            // 
            // panelRemark
            // 
            this.panelRemark.Controls.Add(this.flowRemark);
            this.panelRemark.Controls.Add(this.dividerSectionRemark);
            this.panelRemark.Location = new System.Drawing.Point(275, 434);
            this.panelRemark.Margin = new System.Windows.Forms.Padding(0);
            this.panelRemark.Name = "panelRemark";
            this.panelRemark.Padding = new System.Windows.Forms.Padding(4);
            this.panelRemark.Radius = 0;
            this.panelRemark.Size = new System.Drawing.Size(275, 186);
            this.panelRemark.TabIndex = 3;
            this.panelRemark.Text = "panelRemark";
            // 
            // flowRemark
            // 
            this.flowRemark.AutoScroll = true;
            this.flowRemark.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowRemark.Location = new System.Drawing.Point(4, 27);
            this.flowRemark.Margin = new System.Windows.Forms.Padding(0);
            this.flowRemark.Name = "flowRemark";
            this.flowRemark.Size = new System.Drawing.Size(267, 155);
            this.flowRemark.TabIndex = 1;
            this.flowRemark.Text = "flowRemark";
            // 
            // dividerSectionRemark
            // 
            this.dividerSectionRemark.Dock = System.Windows.Forms.DockStyle.Top;
            this.dividerSectionRemark.Location = new System.Drawing.Point(4, 4);
            this.dividerSectionRemark.Margin = new System.Windows.Forms.Padding(0);
            this.dividerSectionRemark.Name = "dividerSectionRemark";
            this.dividerSectionRemark.Size = new System.Drawing.Size(267, 23);
            this.dividerSectionRemark.TabIndex = 0;
            this.dividerSectionRemark.Text = "说明与备注";
            // 
            // panelBasic
            // 
            this.panelBasic.Controls.Add(this.flowBasic);
            this.panelBasic.Controls.Add(this.dividerSectionBasic);
            this.panelBasic.Location = new System.Drawing.Point(0, 434);
            this.panelBasic.Margin = new System.Windows.Forms.Padding(0);
            this.panelBasic.Name = "panelBasic";
            this.panelBasic.Padding = new System.Windows.Forms.Padding(4);
            this.panelBasic.Radius = 0;
            this.panelBasic.Size = new System.Drawing.Size(275, 186);
            this.panelBasic.TabIndex = 0;
            this.panelBasic.Text = "panelBasic";
            // 
            // flowBasic
            // 
            this.flowBasic.AutoScroll = true;
            this.flowBasic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowBasic.Location = new System.Drawing.Point(4, 27);
            this.flowBasic.Margin = new System.Windows.Forms.Padding(0);
            this.flowBasic.Name = "flowBasic";
            this.flowBasic.Size = new System.Drawing.Size(267, 155);
            this.flowBasic.TabIndex = 1;
            this.flowBasic.Text = "flowBasic";
            // 
            // dividerSectionBasic
            // 
            this.dividerSectionBasic.Dock = System.Windows.Forms.DockStyle.Top;
            this.dividerSectionBasic.Location = new System.Drawing.Point(4, 4);
            this.dividerSectionBasic.Margin = new System.Windows.Forms.Padding(0);
            this.dividerSectionBasic.Name = "dividerSectionBasic";
            this.dividerSectionBasic.Size = new System.Drawing.Size(267, 23);
            this.dividerSectionBasic.TabIndex = 0;
            this.dividerSectionBasic.Text = "基础信息";
            // 
            // panelMapping
            // 
            this.panelMapping.Controls.Add(this.flowMapping);
            this.panelMapping.Controls.Add(this.dividerSectionMapping);
            this.panelMapping.Location = new System.Drawing.Point(275, 0);
            this.panelMapping.Margin = new System.Windows.Forms.Padding(0);
            this.panelMapping.Name = "panelMapping";
            this.panelMapping.Padding = new System.Windows.Forms.Padding(4);
            this.panelMapping.Radius = 0;
            this.panelMapping.Size = new System.Drawing.Size(275, 434);
            this.panelMapping.TabIndex = 1;
            this.panelMapping.Text = "panelMapping";
            // 
            // flowMapping
            // 
            this.flowMapping.AutoScroll = true;
            this.flowMapping.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowMapping.Location = new System.Drawing.Point(4, 27);
            this.flowMapping.Margin = new System.Windows.Forms.Padding(0);
            this.flowMapping.Name = "flowMapping";
            this.flowMapping.Size = new System.Drawing.Size(267, 403);
            this.flowMapping.TabIndex = 1;
            this.flowMapping.Text = "flowMapping";
            // 
            // dividerSectionMapping
            // 
            this.dividerSectionMapping.Dock = System.Windows.Forms.DockStyle.Top;
            this.dividerSectionMapping.Location = new System.Drawing.Point(4, 4);
            this.dividerSectionMapping.Margin = new System.Windows.Forms.Padding(0);
            this.dividerSectionMapping.Name = "dividerSectionMapping";
            this.dividerSectionMapping.Size = new System.Drawing.Size(267, 23);
            this.dividerSectionMapping.TabIndex = 0;
            this.dividerSectionMapping.Text = "IO与映射";
            // 
            // panelState
            // 
            this.panelState.Controls.Add(this.flowState);
            this.panelState.Controls.Add(this.dividerSectionState);
            this.panelState.Location = new System.Drawing.Point(0, 0);
            this.panelState.Margin = new System.Windows.Forms.Padding(0);
            this.panelState.Name = "panelState";
            this.panelState.Padding = new System.Windows.Forms.Padding(4);
            this.panelState.Radius = 0;
            this.panelState.Size = new System.Drawing.Size(275, 434);
            this.panelState.TabIndex = 2;
            this.panelState.Text = "panelState";
            // 
            // flowState
            // 
            this.flowState.AutoScroll = true;
            this.flowState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowState.Location = new System.Drawing.Point(4, 27);
            this.flowState.Margin = new System.Windows.Forms.Padding(0);
            this.flowState.Name = "flowState";
            this.flowState.Size = new System.Drawing.Size(267, 403);
            this.flowState.TabIndex = 1;
            this.flowState.Text = "flowState";
            // 
            // dividerSectionState
            // 
            this.dividerSectionState.Dock = System.Windows.Forms.DockStyle.Top;
            this.dividerSectionState.Location = new System.Drawing.Point(4, 4);
            this.dividerSectionState.Margin = new System.Windows.Forms.Padding(0);
            this.dividerSectionState.Name = "dividerSectionState";
            this.dividerSectionState.Size = new System.Drawing.Size(267, 23);
            this.dividerSectionState.TabIndex = 0;
            this.dividerSectionState.Text = "状态与参数";
            // 
            // ActuatorDetailControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ActuatorDetailControl";
            this.Size = new System.Drawing.Size(550, 620);
            this.panelRoot.ResumeLayout(false);
            this.gridDetails.ResumeLayout(false);
            this.panelRemark.ResumeLayout(false);
            this.panelBasic.ResumeLayout(false);
            this.panelMapping.ResumeLayout(false);
            this.panelState.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AntdUI.Panel panelRoot;
        private AntdUI.GridPanel gridDetails;
        private AntdUI.Panel panelBasic;
        private AntdUI.Divider dividerSectionBasic;
        private AntdUI.FlowPanel flowBasic;
        private AntdUI.Panel panelMapping;
        private AntdUI.Divider dividerSectionMapping;
        private AntdUI.FlowPanel flowMapping;
        private AntdUI.Panel panelState;
        private AntdUI.Divider dividerSectionState;
        private AntdUI.FlowPanel flowState;
        private AntdUI.Panel panelRemark;
        private AntdUI.Divider dividerSectionRemark;
        private AntdUI.FlowPanel flowRemark;
    }
}