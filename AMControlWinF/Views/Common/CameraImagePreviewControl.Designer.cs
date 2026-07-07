namespace AMControlWinF.Views.Common
{
    partial class CameraImagePreviewControl
    {
        private System.ComponentModel.IContainer components = null;

        private void InitializeComponent()
        {
            this.panelRoot = new AntdUI.Panel();
            this.panelViewport = new System.Windows.Forms.Panel();
            this.picturePreview = new System.Windows.Forms.PictureBox();
            this.panelHeader = new AntdUI.Panel();
            this.labelSummary = new AntdUI.Label();
            this.labelTitle = new AntdUI.Label();
            this.panelRoot.SuspendLayout();
            this.panelViewport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picturePreview)).BeginInit();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.panelViewport);
            this.panelRoot.Controls.Add(this.panelHeader);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Margin = new System.Windows.Forms.Padding(0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(0);
            this.panelRoot.Radius = 0;
            this.panelRoot.ShadowOpacity = 0F;
            this.panelRoot.ShadowOpacityHover = 0F;
            this.panelRoot.Size = new System.Drawing.Size(520, 340);
            this.panelRoot.TabIndex = 0;
            // 
            // panelViewport
            // 
            this.panelViewport.BackColor = System.Drawing.Color.FromArgb(22, 25, 32);
            this.panelViewport.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelViewport.Controls.Add(this.picturePreview);
            this.panelViewport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelViewport.Location = new System.Drawing.Point(0, 44);
            this.panelViewport.Margin = new System.Windows.Forms.Padding(0);
            this.panelViewport.Name = "panelViewport";
            this.panelViewport.Size = new System.Drawing.Size(520, 296);
            this.panelViewport.TabIndex = 1;
            // 
            // picturePreview
            // 
            this.picturePreview.BackColor = System.Drawing.Color.FromArgb(22, 25, 32);
            this.picturePreview.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.picturePreview.Location = new System.Drawing.Point(0, 0);
            this.picturePreview.Margin = new System.Windows.Forms.Padding(0);
            this.picturePreview.Name = "picturePreview";
            this.picturePreview.Size = new System.Drawing.Size(320, 180);
            this.picturePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picturePreview.TabIndex = 0;
            this.picturePreview.TabStop = false;
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.labelSummary);
            this.panelHeader.Controls.Add(this.labelTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.panelHeader.Radius = 0;
            this.panelHeader.ShadowOpacity = 0F;
            this.panelHeader.ShadowOpacityHover = 0F;
            this.panelHeader.Size = new System.Drawing.Size(520, 44);
            this.panelHeader.TabIndex = 0;
            // 
            // labelSummary
            // 
            this.labelSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSummary.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelSummary.Location = new System.Drawing.Point(100, 0);
            this.labelSummary.Margin = new System.Windows.Forms.Padding(0);
            this.labelSummary.Name = "labelSummary";
            this.labelSummary.Size = new System.Drawing.Size(420, 36);
            this.labelSummary.TabIndex = 1;
            this.labelSummary.Text = "暂无图像";
            // 
            // labelTitle
            // 
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(0, 0);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(100, 36);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "实时预览";
            // 
            // CameraImagePreviewControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "CameraImagePreviewControl";
            this.Size = new System.Drawing.Size(520, 340);
            this.panelRoot.ResumeLayout(false);
            this.panelViewport.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picturePreview)).EndInit();
            this.panelHeader.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AntdUI.Panel panelRoot;
        private System.Windows.Forms.Panel panelViewport;
        private System.Windows.Forms.PictureBox picturePreview;
        private AntdUI.Panel panelHeader;
        private AntdUI.Label labelSummary;
        private AntdUI.Label labelTitle;
    }
}
