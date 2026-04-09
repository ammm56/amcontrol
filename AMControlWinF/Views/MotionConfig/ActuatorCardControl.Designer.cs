namespace AMControlWinF.Views.MotionConfig
{
    partial class ActuatorCardControl
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
            this.panelCard = new AntdUI.Panel();
            this.panelBody = new AntdUI.Panel();
            this.labelStatus = new AntdUI.Label();
            this.labelName = new AntdUI.Label();
            this.labelTitle = new AntdUI.Label();
            this.panelHeader = new AntdUI.Panel();
            this.buttonDetail = new AntdUI.Button();
            this.buttonTypeTag = new AntdUI.Button();
            this.panelCard.SuspendLayout();
            this.panelBody.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelCard
            // 
            this.panelCard.BackColor = System.Drawing.Color.Transparent;
            this.panelCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(229)))), ((int)(((byte)(235)))));
            this.panelCard.Controls.Add(this.panelBody);
            this.panelCard.Controls.Add(this.panelHeader);
            this.panelCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCard.Location = new System.Drawing.Point(0, 0);
            this.panelCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelCard.Name = "panelCard";
            this.panelCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelCard.Radius = 12;
            this.panelCard.Shadow = 4;
            this.panelCard.ShadowOpacity = 0.2F;
            this.panelCard.Size = new System.Drawing.Size(180, 140);
            this.panelCard.TabIndex = 0;
            // 
            // panelBody
            // 
            this.panelBody.Controls.Add(this.labelStatus);
            this.panelBody.Controls.Add(this.labelName);
            this.panelBody.Controls.Add(this.labelTitle);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(16, 52);
            this.panelBody.Margin = new System.Windows.Forms.Padding(0);
            this.panelBody.Name = "panelBody";
            this.panelBody.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.panelBody.Radius = 0;
            this.panelBody.Size = new System.Drawing.Size(148, 72);
            this.panelBody.TabIndex = 1;
            // 
            // labelStatus
            // 
            this.labelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelStatus.Location = new System.Drawing.Point(97, 50);
            this.labelStatus.Margin = new System.Windows.Forms.Padding(0);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(51, 22);
            this.labelStatus.TabIndex = 5;
            this.labelStatus.Text = "● 启用";
            this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelName
            // 
            this.labelName.ForeColor = System.Drawing.Color.Gray;
            this.labelName.Location = new System.Drawing.Point(0, 25);
            this.labelName.Margin = new System.Windows.Forms.Padding(0);
            this.labelName.Name = "labelName";
            this.labelName.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.labelName.Size = new System.Drawing.Size(122, 22);
            this.labelName.TabIndex = 1;
            this.labelName.Text = "InternalName";
            // 
            // labelTitle
            // 
            this.labelTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(0, 1);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.labelTitle.Size = new System.Drawing.Size(148, 28);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "执行器标题";
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.buttonDetail);
            this.panelHeader.Controls.Add(this.buttonTypeTag);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(16, 16);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Radius = 0;
            this.panelHeader.Size = new System.Drawing.Size(148, 36);
            this.panelHeader.TabIndex = 0;
            // 
            // buttonDetail
            // 
            this.buttonDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDetail.IconSvg = "ProfileOutlined";
            this.buttonDetail.Location = new System.Drawing.Point(84, 0);
            this.buttonDetail.Margin = new System.Windows.Forms.Padding(0);
            this.buttonDetail.Name = "buttonDetail";
            this.buttonDetail.Radius = 8;
            this.buttonDetail.Size = new System.Drawing.Size(64, 32);
            this.buttonDetail.TabIndex = 0;
            this.buttonDetail.Text = "详情";
            this.buttonDetail.WaveSize = 0;
            // 
            // buttonTypeTag
            // 
            this.buttonTypeTag.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.buttonTypeTag.Location = new System.Drawing.Point(0, 0);
            this.buttonTypeTag.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTypeTag.Name = "buttonTypeTag";
            this.buttonTypeTag.Radius = 8;
            this.buttonTypeTag.Size = new System.Drawing.Size(64, 24);
            this.buttonTypeTag.TabIndex = 0;
            this.buttonTypeTag.Text = "气缸";
            this.buttonTypeTag.Type = AntdUI.TTypeMini.Primary;
            this.buttonTypeTag.WaveSize = 0;
            // 
            // ActuatorCardControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelCard);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ActuatorCardControl";
            this.Size = new System.Drawing.Size(180, 140);
            this.panelCard.ResumeLayout(false);
            this.panelBody.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AntdUI.Panel panelCard;
        private AntdUI.Panel panelHeader;
        private AntdUI.Button buttonTypeTag;
        private AntdUI.Button buttonDetail;
        private AntdUI.Panel panelBody;
        private AntdUI.Label labelTitle;
        private AntdUI.Label labelName;
        private AntdUI.Label labelStatus;
    }
}