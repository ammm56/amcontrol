namespace AMControlWinF.Views.Motion
{
    partial class MotionAxisDetailControl
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
            this.panelDetail = new AntdUI.Panel();
            this.panelScroll = new System.Windows.Forms.Panel();
            this.labelDetailText = new AntdUI.Label();
            this.panelHeader = new AntdUI.Panel();
            this.labelSubTitle = new AntdUI.Label();
            this.labelTitle = new AntdUI.Label();
            this.panelEmpty = new AntdUI.Panel();
            this.labelEmpty = new AntdUI.Label();
            this.panelRoot.SuspendLayout();
            this.panelDetail.SuspendLayout();
            this.panelScroll.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.panelEmpty.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.panelDetail);
            this.panelRoot.Controls.Add(this.panelEmpty);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Margin = new System.Windows.Forms.Padding(0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(280, 520);
            this.panelRoot.TabIndex = 0;
            // 
            // panelDetail
            // 
            this.panelDetail.Controls.Add(this.panelScroll);
            this.panelDetail.Controls.Add(this.panelHeader);
            this.panelDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetail.Location = new System.Drawing.Point(0, 0);
            this.panelDetail.Margin = new System.Windows.Forms.Padding(0);
            this.panelDetail.Name = "panelDetail";
            this.panelDetail.Radius = 0;
            this.panelDetail.Size = new System.Drawing.Size(280, 520);
            this.panelDetail.TabIndex = 1;
            // 
            // panelScroll
            // 
            this.panelScroll.Controls.Add(this.labelDetailText);
            this.panelScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelScroll.Location = new System.Drawing.Point(0, 60);
            this.panelScroll.Margin = new System.Windows.Forms.Padding(0);
            this.panelScroll.Name = "panelScroll";
            this.panelScroll.Padding = new System.Windows.Forms.Padding(14, 12, 14, 12);
            this.panelScroll.Size = new System.Drawing.Size(280, 460);
            this.panelScroll.TabIndex = 1;
            // 
            // labelDetailText
            // 
            this.labelDetailText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDetailText.ForeColor = System.Drawing.Color.Gray;
            this.labelDetailText.Location = new System.Drawing.Point(14, 12);
            this.labelDetailText.Margin = new System.Windows.Forms.Padding(0);
            this.labelDetailText.Name = "labelDetailText";
            this.labelDetailText.Size = new System.Drawing.Size(252, 436);
            this.labelDetailText.TabIndex = 0;
            this.labelDetailText.Text = "动作详情";
            this.labelDetailText.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.labelSubTitle);
            this.panelHeader.Controls.Add(this.labelTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Padding = new System.Windows.Forms.Padding(8);
            this.panelHeader.Radius = 0;
            this.panelHeader.Size = new System.Drawing.Size(280, 60);
            this.panelHeader.TabIndex = 0;
            // 
            // labelSubTitle
            // 
            this.labelSubTitle.AutoEllipsis = true;
            this.labelSubTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelSubTitle.ForeColor = System.Drawing.Color.Gray;
            this.labelSubTitle.Location = new System.Drawing.Point(8, 34);
            this.labelSubTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelSubTitle.Name = "labelSubTitle";
            this.labelSubTitle.Size = new System.Drawing.Size(264, 22);
            this.labelSubTitle.TabIndex = 1;
            this.labelSubTitle.Text = "当前轴与动作说明";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoEllipsis = true;
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(8, 8);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(264, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "轴动作详情";
            // 
            // panelEmpty
            // 
            this.panelEmpty.Controls.Add(this.labelEmpty);
            this.panelEmpty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEmpty.Location = new System.Drawing.Point(0, 0);
            this.panelEmpty.Margin = new System.Windows.Forms.Padding(0);
            this.panelEmpty.Name = "panelEmpty";
            this.panelEmpty.Radius = 0;
            this.panelEmpty.Size = new System.Drawing.Size(280, 520);
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
            this.labelEmpty.Size = new System.Drawing.Size(280, 520);
            this.labelEmpty.TabIndex = 0;
            this.labelEmpty.Text = "请选择一个轴动作卡片查看详情";
            this.labelEmpty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MotionAxisDetailControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MotionAxisDetailControl";
            this.Size = new System.Drawing.Size(280, 520);
            this.panelRoot.ResumeLayout(false);
            this.panelDetail.ResumeLayout(false);
            this.panelScroll.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.panelEmpty.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private AntdUI.Panel panelRoot;
        private AntdUI.Panel panelDetail;
        private System.Windows.Forms.Panel panelScroll;
        private AntdUI.Label labelDetailText;
        private AntdUI.Panel panelHeader;
        private AntdUI.Label labelSubTitle;
        private AntdUI.Label labelTitle;
        private AntdUI.Panel panelEmpty;
        private AntdUI.Label labelEmpty;
    }
}