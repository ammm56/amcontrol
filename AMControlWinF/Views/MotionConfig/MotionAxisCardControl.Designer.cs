namespace AMControlWinF.Views.MotionConfig
{
    partial class MotionAxisCardControl
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
            this.panelBottom = new AntdUI.Panel();
            this.flowBottomRight = new AntdUI.FlowPanel();
            this.labelValueStatus = new AntdUI.Label();
            this.flowBottomLeft = new AntdUI.FlowPanel();
            this.labelValuePhysicalAxis = new AntdUI.Label();
            this.labelValuePhysicalCore = new AntdUI.Label();
            this.panelBody = new AntdUI.Panel();
            this.labelValueLogicalAxis = new AntdUI.Label();
            this.buttonDetail = new AntdUI.Button();
            this.labelSubTitle = new AntdUI.Label();
            this.labelTitle = new AntdUI.Label();
            this.panelHeader = new AntdUI.Panel();
            this.flowHeaderRight = new AntdUI.FlowPanel();
            this.buttonEdit = new AntdUI.Button();
            this.buttonDelete = new AntdUI.Button();
            this.panelHeaderLeft = new AntdUI.Panel();
            this.buttonCategoryTag = new AntdUI.Button();
            this.panelCard.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.flowBottomRight.SuspendLayout();
            this.flowBottomLeft.SuspendLayout();
            this.panelBody.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.flowHeaderRight.SuspendLayout();
            this.panelHeaderLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelCard
            // 
            this.panelCard.BackColor = System.Drawing.Color.Transparent;
            this.panelCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(229)))), ((int)(((byte)(235)))));
            this.panelCard.Controls.Add(this.panelBottom);
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
            this.panelCard.ShadowOpacityAnimation = true;
            this.panelCard.Size = new System.Drawing.Size(245, 170);
            this.panelCard.TabIndex = 0;
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.flowBottomRight);
            this.panelBottom.Controls.Add(this.flowBottomLeft);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(16, 114);
            this.panelBottom.Margin = new System.Windows.Forms.Padding(0);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.panelBottom.Radius = 0;
            this.panelBottom.Size = new System.Drawing.Size(213, 40);
            this.panelBottom.TabIndex = 2;
            this.panelBottom.Text = "panelBottom";
            // 
            // flowBottomRight
            // 
            this.flowBottomRight.Align = AntdUI.TAlignFlow.Right;
            this.flowBottomRight.Controls.Add(this.labelValueStatus);
            this.flowBottomRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowBottomRight.Location = new System.Drawing.Point(135, 8);
            this.flowBottomRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowBottomRight.Name = "flowBottomRight";
            this.flowBottomRight.Size = new System.Drawing.Size(78, 32);
            this.flowBottomRight.TabIndex = 1;
            this.flowBottomRight.Text = "flowBottomRight";
            // 
            // labelValueStatus
            // 
            this.labelValueStatus.Location = new System.Drawing.Point(14, 0);
            this.labelValueStatus.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueStatus.Name = "labelValueStatus";
            this.labelValueStatus.Size = new System.Drawing.Size(64, 32);
            this.labelValueStatus.TabIndex = 0;
            this.labelValueStatus.Text = "● 启用";
            this.labelValueStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // flowBottomLeft
            // 
            this.flowBottomLeft.Controls.Add(this.labelValuePhysicalAxis);
            this.flowBottomLeft.Controls.Add(this.labelValuePhysicalCore);
            this.flowBottomLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowBottomLeft.Gap = 6;
            this.flowBottomLeft.Location = new System.Drawing.Point(0, 8);
            this.flowBottomLeft.Margin = new System.Windows.Forms.Padding(0);
            this.flowBottomLeft.Name = "flowBottomLeft";
            this.flowBottomLeft.Size = new System.Drawing.Size(128, 32);
            this.flowBottomLeft.TabIndex = 0;
            this.flowBottomLeft.Text = "flowBottomLeft";
            // 
            // labelValuePhysicalAxis
            // 
            this.labelValuePhysicalAxis.Location = new System.Drawing.Point(50, 0);
            this.labelValuePhysicalAxis.Margin = new System.Windows.Forms.Padding(0);
            this.labelValuePhysicalAxis.Name = "labelValuePhysicalAxis";
            this.labelValuePhysicalAxis.Size = new System.Drawing.Size(55, 32);
            this.labelValuePhysicalAxis.TabIndex = 1;
            this.labelValuePhysicalAxis.Text = "轴: A1";
            // 
            // labelValuePhysicalCore
            // 
            this.labelValuePhysicalCore.Location = new System.Drawing.Point(0, 0);
            this.labelValuePhysicalCore.Margin = new System.Windows.Forms.Padding(0);
            this.labelValuePhysicalCore.Name = "labelValuePhysicalCore";
            this.labelValuePhysicalCore.Size = new System.Drawing.Size(44, 32);
            this.labelValuePhysicalCore.TabIndex = 1;
            this.labelValuePhysicalCore.Text = "核: C1";
            // 
            // panelBody
            // 
            this.panelBody.Controls.Add(this.labelValueLogicalAxis);
            this.panelBody.Controls.Add(this.buttonDetail);
            this.panelBody.Controls.Add(this.labelSubTitle);
            this.panelBody.Controls.Add(this.labelTitle);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(16, 52);
            this.panelBody.Margin = new System.Windows.Forms.Padding(0);
            this.panelBody.Name = "panelBody";
            this.panelBody.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.panelBody.Radius = 0;
            this.panelBody.Size = new System.Drawing.Size(213, 102);
            this.panelBody.TabIndex = 1;
            // 
            // labelValueLogicalAxis
            // 
            this.labelValueLogicalAxis.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelValueLogicalAxis.ForeColor = System.Drawing.Color.Gray;
            this.labelValueLogicalAxis.Location = new System.Drawing.Point(149, 40);
            this.labelValueLogicalAxis.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueLogicalAxis.Name = "labelValueLogicalAxis";
            this.labelValueLogicalAxis.Padding = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.labelValueLogicalAxis.Size = new System.Drawing.Size(64, 24);
            this.labelValueLogicalAxis.TabIndex = 2;
            this.labelValueLogicalAxis.Text = "L101";
            this.labelValueLogicalAxis.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonDetail
            // 
            this.buttonDetail.IconSvg = "ProfileOutlined";
            this.buttonDetail.Location = new System.Drawing.Point(154, 6);
            this.buttonDetail.Margin = new System.Windows.Forms.Padding(0);
            this.buttonDetail.Name = "buttonDetail";
            this.buttonDetail.Radius = 8;
            this.buttonDetail.Size = new System.Drawing.Size(64, 32);
            this.buttonDetail.TabIndex = 1;
            this.buttonDetail.Text = "详情";
            this.buttonDetail.WaveSize = 0;
            // 
            // labelSubTitle
            // 
            this.labelSubTitle.ForeColor = System.Drawing.Color.Gray;
            this.labelSubTitle.Location = new System.Drawing.Point(0, 34);
            this.labelSubTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelSubTitle.Name = "labelSubTitle";
            this.labelSubTitle.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.labelSubTitle.Size = new System.Drawing.Size(167, 24);
            this.labelSubTitle.TabIndex = 1;
            this.labelSubTitle.Text = "Axis-101";
            // 
            // labelTitle
            // 
            this.labelTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(0, 6);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.labelTitle.Size = new System.Drawing.Size(167, 28);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "X轴";
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.flowHeaderRight);
            this.panelHeader.Controls.Add(this.panelHeaderLeft);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(16, 16);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Radius = 0;
            this.panelHeader.Size = new System.Drawing.Size(213, 36);
            this.panelHeader.TabIndex = 0;
            // 
            // flowHeaderRight
            // 
            this.flowHeaderRight.Align = AntdUI.TAlignFlow.RightCenter;
            this.flowHeaderRight.Controls.Add(this.buttonEdit);
            this.flowHeaderRight.Controls.Add(this.buttonDelete);
            this.flowHeaderRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowHeaderRight.Gap = 6;
            this.flowHeaderRight.Location = new System.Drawing.Point(67, 0);
            this.flowHeaderRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowHeaderRight.Name = "flowHeaderRight";
            this.flowHeaderRight.Size = new System.Drawing.Size(146, 36);
            this.flowHeaderRight.TabIndex = 1;
            this.flowHeaderRight.Text = "flowHeaderRight";
            // 
            // buttonEdit
            // 
            this.buttonEdit.IconSvg = "EditOutlined";
            this.buttonEdit.Location = new System.Drawing.Point(82, 0);
            this.buttonEdit.Margin = new System.Windows.Forms.Padding(0);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Radius = 8;
            this.buttonEdit.Size = new System.Drawing.Size(64, 32);
            this.buttonEdit.TabIndex = 0;
            this.buttonEdit.Text = "编辑";
            this.buttonEdit.WaveSize = 0;
            // 
            // buttonDelete
            // 
            this.buttonDelete.IconSvg = "DeleteOutlined";
            this.buttonDelete.Location = new System.Drawing.Point(12, 0);
            this.buttonDelete.Margin = new System.Windows.Forms.Padding(0);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Radius = 8;
            this.buttonDelete.Size = new System.Drawing.Size(64, 32);
            this.buttonDelete.TabIndex = 2;
            this.buttonDelete.Text = "删除";
            this.buttonDelete.WaveSize = 0;
            // 
            // panelHeaderLeft
            // 
            this.panelHeaderLeft.Controls.Add(this.buttonCategoryTag);
            this.panelHeaderLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelHeaderLeft.Location = new System.Drawing.Point(0, 0);
            this.panelHeaderLeft.Margin = new System.Windows.Forms.Padding(0);
            this.panelHeaderLeft.Name = "panelHeaderLeft";
            this.panelHeaderLeft.Radius = 0;
            this.panelHeaderLeft.Size = new System.Drawing.Size(96, 36);
            this.panelHeaderLeft.TabIndex = 0;
            // 
            // buttonCategoryTag
            // 
            this.buttonCategoryTag.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.buttonCategoryTag.Location = new System.Drawing.Point(0, 6);
            this.buttonCategoryTag.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCategoryTag.Name = "buttonCategoryTag";
            this.buttonCategoryTag.Radius = 8;
            this.buttonCategoryTag.Size = new System.Drawing.Size(72, 24);
            this.buttonCategoryTag.TabIndex = 0;
            this.buttonCategoryTag.Text = "直线轴";
            this.buttonCategoryTag.WaveSize = 0;
            // 
            // MotionAxisCardControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelCard);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MotionAxisCardControl";
            this.Size = new System.Drawing.Size(245, 170);
            this.panelCard.ResumeLayout(false);
            this.panelBottom.ResumeLayout(false);
            this.flowBottomRight.ResumeLayout(false);
            this.flowBottomLeft.ResumeLayout(false);
            this.panelBody.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.flowHeaderRight.ResumeLayout(false);
            this.panelHeaderLeft.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AntdUI.Panel panelCard;
        private AntdUI.Panel panelHeader;
        private AntdUI.FlowPanel flowHeaderRight;
        private AntdUI.Button buttonEdit;
        private AntdUI.Button buttonDetail;
        private AntdUI.Button buttonDelete;
        private AntdUI.Panel panelHeaderLeft;
        private AntdUI.Button buttonCategoryTag;
        private AntdUI.Panel panelBody;
        private AntdUI.Label labelTitle;
        private AntdUI.Label labelSubTitle;
        private AntdUI.Label labelValueLogicalAxis;
        private AntdUI.Panel panelBottom;
        private AntdUI.FlowPanel flowBottomLeft;
        private AntdUI.Label labelValuePhysicalCore;
        private AntdUI.FlowPanel flowBottomRight;
        private AntdUI.Label labelValueStatus;
        private AntdUI.Label labelValuePhysicalAxis;
    }
}