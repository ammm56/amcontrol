namespace AMControlWinF.Views.MotionConfig
{
    partial class MotionAxisParamCardControl
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
            this.labelParamName = new AntdUI.Label();
            this.buttonDetail = new AntdUI.Button();
            this.labelValue = new AntdUI.Label();
            this.labelTitle = new AntdUI.Label();
            this.buttonTypeTag = new AntdUI.Button();
            this.panelCard.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelCard
            // 
            this.panelCard.BackColor = System.Drawing.Color.Transparent;
            this.panelCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(229)))), ((int)(((byte)(235)))));
            this.panelCard.Controls.Add(this.buttonDetail);
            this.panelCard.Controls.Add(this.labelParamName);
            this.panelCard.Controls.Add(this.buttonTypeTag);
            this.panelCard.Controls.Add(this.labelValue);
            this.panelCard.Controls.Add(this.labelTitle);
            this.panelCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCard.Location = new System.Drawing.Point(0, 0);
            this.panelCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelCard.Name = "panelCard";
            this.panelCard.Padding = new System.Windows.Forms.Padding(8);
            this.panelCard.Radius = 12;
            this.panelCard.Shadow = 4;
            this.panelCard.ShadowOpacity = 0.2F;
            this.panelCard.ShadowOpacityAnimation = true;
            this.panelCard.Size = new System.Drawing.Size(140, 85);
            this.panelCard.TabIndex = 0;
            // 
            // labelParamName
            // 
            this.labelParamName.ForeColor = System.Drawing.Color.Gray;
            this.labelParamName.Location = new System.Drawing.Point(11, 55);
            this.labelParamName.Margin = new System.Windows.Forms.Padding(0);
            this.labelParamName.Name = "labelParamName";
            this.labelParamName.Size = new System.Drawing.Size(121, 22);
            this.labelParamName.TabIndex = 0;
            this.labelParamName.Text = "ParamName";
            // 
            // buttonDetail
            // 
            this.buttonDetail.IconSvg = "ProfileOutlined";
            this.buttonDetail.Location = new System.Drawing.Point(69, 5);
            this.buttonDetail.Margin = new System.Windows.Forms.Padding(0);
            this.buttonDetail.Name = "buttonDetail";
            this.buttonDetail.Radius = 8;
            this.buttonDetail.Size = new System.Drawing.Size(64, 32);
            this.buttonDetail.TabIndex = 0;
            this.buttonDetail.Text = "详情";
            this.buttonDetail.WaveSize = 0;
            // 
            // labelValue
            // 
            this.labelValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Bold);
            this.labelValue.Location = new System.Drawing.Point(87, 29);
            this.labelValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelValue.Name = "labelValue";
            this.labelValue.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.labelValue.Size = new System.Drawing.Size(47, 34);
            this.labelValue.TabIndex = 1;
            this.labelValue.Text = "0";
            // 
            // labelTitle
            // 
            this.labelTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(6, 29);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.labelTitle.Size = new System.Drawing.Size(84, 28);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "默认速度";
            // 
            // buttonTypeTag
            // 
            this.buttonTypeTag.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.buttonTypeTag.Location = new System.Drawing.Point(8, 5);
            this.buttonTypeTag.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTypeTag.Name = "buttonTypeTag";
            this.buttonTypeTag.Radius = 8;
            this.buttonTypeTag.Size = new System.Drawing.Size(42, 24);
            this.buttonTypeTag.TabIndex = 0;
            this.buttonTypeTag.Text = "小数";
            this.buttonTypeTag.Visible = false;
            this.buttonTypeTag.WaveSize = 0;
            // 
            // MotionAxisParamCardControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelCard);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MotionAxisParamCardControl";
            this.Size = new System.Drawing.Size(140, 85);
            this.panelCard.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AntdUI.Panel panelCard;
        private AntdUI.Button buttonDetail;
        private AntdUI.Button buttonTypeTag;
        private AntdUI.Label labelTitle;
        private AntdUI.Label labelValue;
        private AntdUI.Label labelParamName;
    }
}