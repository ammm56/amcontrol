namespace AMControlWinF.Views.Motion
{
    partial class MotionActuatorDetailControl
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
            this.labelDetail = new AntdUI.Label();
            this.labelTitle = new AntdUI.Label();
            this.panelRoot.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.labelDetail);
            this.panelRoot.Controls.Add(this.labelTitle);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Margin = new System.Windows.Forms.Padding(0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(12);
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(320, 320);
            this.panelRoot.TabIndex = 0;
            // 
            // labelDetail
            // 
            this.labelDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDetail.Location = new System.Drawing.Point(16, 40);
            this.labelDetail.Margin = new System.Windows.Forms.Padding(0);
            this.labelDetail.Name = "labelDetail";
            this.labelDetail.Size = new System.Drawing.Size(288, 264);
            this.labelDetail.TabIndex = 1;
            this.labelDetail.Text = "请先在左侧选择一个执行器对象。";
            // 
            // labelTitle
            // 
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(16, 16);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(288, 24);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "未选择执行器";
            // 
            // MotionActuatorDetailControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MotionActuatorDetailControl";
            this.Size = new System.Drawing.Size(320, 320);
            this.panelRoot.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private AntdUI.Panel panelRoot;
        private AntdUI.Label labelTitle;
        private AntdUI.Label labelDetail;
    }
}