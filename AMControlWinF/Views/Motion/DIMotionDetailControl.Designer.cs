namespace AMControlWinF.Views.Motion
{
    partial class DIMotionDetailControl
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
            this.panelRoot = new AntdUI.Panel();
            this.panelEmpty = new AntdUI.Panel();
            this.labelEmpty = new AntdUI.Label();
            this.gridDetails = new AntdUI.GridPanel();
            this.panelRemark = new AntdUI.Panel();
            this.flowRemark = new AntdUI.FlowPanel();
            this.dividerRemark = new AntdUI.Divider();
            this.panelRuntime = new AntdUI.Panel();
            this.flowRuntime = new AntdUI.FlowPanel();
            this.dividerRuntime = new AntdUI.Divider();
            this.panelMapping = new AntdUI.Panel();
            this.flowMapping = new AntdUI.FlowPanel();
            this.dividerMapping = new AntdUI.Divider();
            this.panelBasic = new AntdUI.Panel();
            this.flowBasic = new AntdUI.FlowPanel();
            this.dividerBasic = new AntdUI.Divider();
            this.panelRoot.SuspendLayout();
            this.panelEmpty.SuspendLayout();
            this.gridDetails.SuspendLayout();
            this.panelRemark.SuspendLayout();
            this.panelRuntime.SuspendLayout();
            this.panelMapping.SuspendLayout();
            this.panelBasic.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.gridDetails);
            this.panelRoot.Controls.Add(this.panelEmpty);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Margin = new System.Windows.Forms.Padding(0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(360, 520);
            this.panelRoot.TabIndex = 0;
            // 
            // panelEmpty
            // 
            this.panelEmpty.Controls.Add(this.labelEmpty);
            this.panelEmpty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEmpty.Location = new System.Drawing.Point(0, 0);
            this.panelEmpty.Margin = new System.Windows.Forms.Padding(0);
            this.panelEmpty.Name = "panelEmpty";
            this.panelEmpty.Radius = 0;
            this.panelEmpty.Size = new System.Drawing.Size(360, 520);
            this.panelEmpty.TabIndex = 0;
            // 
            // labelEmpty
            // 
            this.labelEmpty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEmpty.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.labelEmpty.ForeColor = System.Drawing.Color.Gray;
            this.labelEmpty.Location = new System.Drawing.Point(0, 0);
            this.labelEmpty.Margin = new System.Windows.Forms.Padding(0);
            this.labelEmpty.Name = "labelEmpty";
            this.labelEmpty.Size = new System.Drawing.Size(360, 520);
            this.labelEmpty.TabIndex = 0;
            this.labelEmpty.Text = "请选择左侧 DI 卡片查看详情";
            this.labelEmpty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gridDetails
            // 
            this.gridDetails.Controls.Add(this.panelRemark);
            this.gridDetails.Controls.Add(this.panelRuntime);
            this.gridDetails.Controls.Add(this.panelMapping);
            this.gridDetails.Controls.Add(this.panelBasic);
            this.gridDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDetails.Location = new System.Drawing.Point(0, 0);
            this.gridDetails.Margin = new System.Windows.Forms.Padding(0);
            this.gridDetails.Name = "gridDetails";
            this.gridDetails.Size = new System.Drawing.Size(360, 520);
            this.gridDetails.Span = "50% 50%;50% 50%;-60%";
            this.gridDetails.TabIndex = 1;
            // 
            // panelRemark
            // 
            this.panelRemark.Controls.Add(this.flowRemark);
            this.panelRemark.Controls.Add(this.dividerRemark);
            this.panelRemark.Location = new System.Drawing.Point(180, 286);
            this.panelRemark.Margin = new System.Windows.Forms.Padding(0);
            this.panelRemark.Name = "panelRemark";
            this.panelRemark.Padding = new System.Windows.Forms.Padding(4);
            this.panelRemark.Radius = 0;
            this.panelRemark.Size = new System.Drawing.Size(180, 234);
            this.panelRemark.TabIndex = 3;
            // 
            // flowRemark
            // 
            this.flowRemark.AutoScroll = true;
            this.flowRemark.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowRemark.Location = new System.Drawing.Point(4, 27);
            this.flowRemark.Margin = new System.Windows.Forms.Padding(0);
            this.flowRemark.Name = "flowRemark";
            this.flowRemark.Size = new System.Drawing.Size(172, 203);
            this.flowRemark.TabIndex = 1;
            // 
            // dividerRemark
            // 
            this.dividerRemark.Dock = System.Windows.Forms.DockStyle.Top;
            this.dividerRemark.Location = new System.Drawing.Point(4, 4);
            this.dividerRemark.Margin = new System.Windows.Forms.Padding(0);
            this.dividerRemark.Name = "dividerRemark";
            this.dividerRemark.Size = new System.Drawing.Size(172, 23);
            this.dividerRemark.TabIndex = 0;
            this.dividerRemark.Text = "说明与备注";
            // 
            // panelRuntime
            // 
            this.panelRuntime.Controls.Add(this.flowRuntime);
            this.panelRuntime.Controls.Add(this.dividerRuntime);
            this.panelRuntime.Location = new System.Drawing.Point(0, 286);
            this.panelRuntime.Margin = new System.Windows.Forms.Padding(0);
            this.panelRuntime.Name = "panelRuntime";
            this.panelRuntime.Padding = new System.Windows.Forms.Padding(4);
            this.panelRuntime.Radius = 0;
            this.panelRuntime.Size = new System.Drawing.Size(180, 234);
            this.panelRuntime.TabIndex = 2;
            // 
            // flowRuntime
            // 
            this.flowRuntime.AutoScroll = true;
            this.flowRuntime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowRuntime.Location = new System.Drawing.Point(4, 27);
            this.flowRuntime.Margin = new System.Windows.Forms.Padding(0);
            this.flowRuntime.Name = "flowRuntime";
            this.flowRuntime.Size = new System.Drawing.Size(172, 203);
            this.flowRuntime.TabIndex = 1;
            // 
            // dividerRuntime
            // 
            this.dividerRuntime.Dock = System.Windows.Forms.DockStyle.Top;
            this.dividerRuntime.Location = new System.Drawing.Point(4, 4);
            this.dividerRuntime.Margin = new System.Windows.Forms.Padding(0);
            this.dividerRuntime.Name = "dividerRuntime";
            this.dividerRuntime.Size = new System.Drawing.Size(172, 23);
            this.dividerRuntime.TabIndex = 0;
            this.dividerRuntime.Text = "运行信息";
            // 
            // panelMapping
            // 
            this.panelMapping.Controls.Add(this.flowMapping);
            this.panelMapping.Controls.Add(this.dividerMapping);
            this.panelMapping.Location = new System.Drawing.Point(180, 0);
            this.panelMapping.Margin = new System.Windows.Forms.Padding(0);
            this.panelMapping.Name = "panelMapping";
            this.panelMapping.Padding = new System.Windows.Forms.Padding(4);
            this.panelMapping.Radius = 0;
            this.panelMapping.Size = new System.Drawing.Size(180, 286);
            this.panelMapping.TabIndex = 1;
            // 
            // flowMapping
            // 
            this.flowMapping.AutoScroll = true;
            this.flowMapping.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowMapping.Location = new System.Drawing.Point(4, 27);
            this.flowMapping.Margin = new System.Windows.Forms.Padding(0);
            this.flowMapping.Name = "flowMapping";
            this.flowMapping.Size = new System.Drawing.Size(172, 255);
            this.flowMapping.TabIndex = 1;
            // 
            // dividerMapping
            // 
            this.dividerMapping.Dock = System.Windows.Forms.DockStyle.Top;
            this.dividerMapping.Location = new System.Drawing.Point(4, 4);
            this.dividerMapping.Margin = new System.Windows.Forms.Padding(0);
            this.dividerMapping.Name = "dividerMapping";
            this.dividerMapping.Size = new System.Drawing.Size(172, 23);
            this.dividerMapping.TabIndex = 0;
            this.dividerMapping.Text = "归属与映射";
            // 
            // panelBasic
            // 
            this.panelBasic.Controls.Add(this.flowBasic);
            this.panelBasic.Controls.Add(this.dividerBasic);
            this.panelBasic.Location = new System.Drawing.Point(0, 0);
            this.panelBasic.Margin = new System.Windows.Forms.Padding(0);
            this.panelBasic.Name = "panelBasic";
            this.panelBasic.Padding = new System.Windows.Forms.Padding(4);
            this.panelBasic.Radius = 0;
            this.panelBasic.Size = new System.Drawing.Size(180, 286);
            this.panelBasic.TabIndex = 0;
            // 
            // flowBasic
            // 
            this.flowBasic.AutoScroll = true;
            this.flowBasic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowBasic.Location = new System.Drawing.Point(4, 27);
            this.flowBasic.Margin = new System.Windows.Forms.Padding(0);
            this.flowBasic.Name = "flowBasic";
            this.flowBasic.Size = new System.Drawing.Size(172, 255);
            this.flowBasic.TabIndex = 1;
            // 
            // dividerBasic
            // 
            this.dividerBasic.Dock = System.Windows.Forms.DockStyle.Top;
            this.dividerBasic.Location = new System.Drawing.Point(4, 4);
            this.dividerBasic.Margin = new System.Windows.Forms.Padding(0);
            this.dividerBasic.Name = "dividerBasic";
            this.dividerBasic.Size = new System.Drawing.Size(172, 23);
            this.dividerBasic.TabIndex = 0;
            this.dividerBasic.Text = "基础标识";
            // 
            // DIMotionDetailControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "DIMotionDetailControl";
            this.Size = new System.Drawing.Size(360, 520);
            this.panelRoot.ResumeLayout(false);
            this.panelEmpty.ResumeLayout(false);
            this.gridDetails.ResumeLayout(false);
            this.panelRemark.ResumeLayout(false);
            this.panelRuntime.ResumeLayout(false);
            this.panelMapping.ResumeLayout(false);
            this.panelBasic.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AntdUI.Panel panelRoot;
        private AntdUI.Panel panelEmpty;
        private AntdUI.Label labelEmpty;
        private AntdUI.GridPanel gridDetails;
        private AntdUI.Panel panelBasic;
        private AntdUI.FlowPanel flowBasic;
        private AntdUI.Divider dividerBasic;
        private AntdUI.Panel panelMapping;
        private AntdUI.FlowPanel flowMapping;
        private AntdUI.Divider dividerMapping;
        private AntdUI.Panel panelRuntime;
        private AntdUI.FlowPanel flowRuntime;
        private AntdUI.Divider dividerRuntime;
        private AntdUI.Panel panelRemark;
        private AntdUI.FlowPanel flowRemark;
        private AntdUI.Divider dividerRemark;
    }
}