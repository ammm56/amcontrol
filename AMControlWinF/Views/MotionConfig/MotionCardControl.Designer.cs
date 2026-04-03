namespace AMControlWinF.Views.MotionConfig
{
    partial class MotionCardControl
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
            this.buttonStatusTag = new AntdUI.Button();
            this.flowBottomLeft = new AntdUI.FlowPanel();
            this.labelValueExtModule = new AntdUI.Label();
            this.labelValueAxisCount = new AntdUI.Label();
            this.labelValueCore = new AntdUI.Label();
            this.panelBody = new AntdUI.Panel();
            this.labelValueCardId = new AntdUI.Label();
            this.labelSubTitle = new AntdUI.Label();
            this.labelTitle = new AntdUI.Label();
            this.panelHeader = new AntdUI.Panel();
            this.flowHeaderRight = new AntdUI.FlowPanel();
            this.buttonEdit = new AntdUI.Button();
            this.buttonDetail = new AntdUI.Button();
            this.buttonDelete = new AntdUI.Button();
            this.panelHeaderLeft = new AntdUI.Panel();
            this.buttonTypeTag = new AntdUI.Button();
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
            this.panelCard.Size = new System.Drawing.Size(311, 201);
            this.panelCard.TabIndex = 0;
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.flowBottomRight);
            this.panelBottom.Controls.Add(this.flowBottomLeft);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(16, 153);
            this.panelBottom.Margin = new System.Windows.Forms.Padding(0);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.panelBottom.Radius = 0;
            this.panelBottom.Size = new System.Drawing.Size(279, 32);
            this.panelBottom.TabIndex = 2;
            this.panelBottom.Text = "panelBottom";
            // 
            // flowBottomRight
            // 
            this.flowBottomRight.Align = AntdUI.TAlignFlow.Right;
            this.flowBottomRight.Controls.Add(this.labelValueStatus);
            this.flowBottomRight.Controls.Add(this.buttonStatusTag);
            this.flowBottomRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowBottomRight.Gap = 6;
            this.flowBottomRight.Location = new System.Drawing.Point(167, 8);
            this.flowBottomRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowBottomRight.Name = "flowBottomRight";
            this.flowBottomRight.Size = new System.Drawing.Size(112, 24);
            this.flowBottomRight.TabIndex = 1;
            this.flowBottomRight.Text = "flowBottomRight";
            // 
            // labelValueStatus
            // 
            this.labelValueStatus.Location = new System.Drawing.Point(52, 0);
            this.labelValueStatus.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueStatus.Name = "labelValueStatus";
            this.labelValueStatus.Size = new System.Drawing.Size(60, 24);
            this.labelValueStatus.TabIndex = 1;
            this.labelValueStatus.Text = "● 启用";
            // 
            // buttonStatusTag
            // 
            this.buttonStatusTag.Location = new System.Drawing.Point(0, 0);
            this.buttonStatusTag.Margin = new System.Windows.Forms.Padding(0);
            this.buttonStatusTag.Name = "buttonStatusTag";
            this.buttonStatusTag.Radius = 8;
            this.buttonStatusTag.Size = new System.Drawing.Size(46, 24);
            this.buttonStatusTag.TabIndex = 0;
            this.buttonStatusTag.Text = "状态";
            this.buttonStatusTag.Type = AntdUI.TTypeMini.Success;
            this.buttonStatusTag.Visible = false;
            this.buttonStatusTag.WaveSize = 0;
            // 
            // flowBottomLeft
            // 
            this.flowBottomLeft.Controls.Add(this.labelValueExtModule);
            this.flowBottomLeft.Controls.Add(this.labelValueAxisCount);
            this.flowBottomLeft.Controls.Add(this.labelValueCore);
            this.flowBottomLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowBottomLeft.Gap = 8;
            this.flowBottomLeft.Location = new System.Drawing.Point(0, 8);
            this.flowBottomLeft.Margin = new System.Windows.Forms.Padding(0);
            this.flowBottomLeft.Name = "flowBottomLeft";
            this.flowBottomLeft.Size = new System.Drawing.Size(220, 24);
            this.flowBottomLeft.TabIndex = 0;
            this.flowBottomLeft.Text = "flowBottomLeft";
            // 
            // labelValueExtModule
            // 
            this.labelValueExtModule.Location = new System.Drawing.Point(106, 0);
            this.labelValueExtModule.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueExtModule.Name = "labelValueExtModule";
            this.labelValueExtModule.Size = new System.Drawing.Size(55, 24);
            this.labelValueExtModule.TabIndex = 2;
            this.labelValueExtModule.Text = "扩展: 是";
            // 
            // labelValueAxisCount
            // 
            this.labelValueAxisCount.Location = new System.Drawing.Point(53, 0);
            this.labelValueAxisCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueAxisCount.Name = "labelValueAxisCount";
            this.labelValueAxisCount.Size = new System.Drawing.Size(45, 24);
            this.labelValueAxisCount.TabIndex = 1;
            this.labelValueAxisCount.Text = "轴: 16";
            // 
            // labelValueCore
            // 
            this.labelValueCore.Location = new System.Drawing.Point(0, 0);
            this.labelValueCore.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueCore.Name = "labelValueCore";
            this.labelValueCore.Size = new System.Drawing.Size(45, 24);
            this.labelValueCore.TabIndex = 0;
            this.labelValueCore.Text = "核: 2";
            // 
            // panelBody
            // 
            this.panelBody.Controls.Add(this.labelValueCardId);
            this.panelBody.Controls.Add(this.labelSubTitle);
            this.panelBody.Controls.Add(this.labelTitle);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(16, 52);
            this.panelBody.Margin = new System.Windows.Forms.Padding(0);
            this.panelBody.Name = "panelBody";
            this.panelBody.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.panelBody.Radius = 0;
            this.panelBody.Size = new System.Drawing.Size(279, 133);
            this.panelBody.TabIndex = 1;
            // 
            // labelValueCardId
            // 
            this.labelValueCardId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelValueCardId.ForeColor = System.Drawing.Color.Gray;
            this.labelValueCardId.Location = new System.Drawing.Point(237, 10);
            this.labelValueCardId.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueCardId.Name = "labelValueCardId";
            this.labelValueCardId.Padding = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.labelValueCardId.Size = new System.Drawing.Size(42, 24);
            this.labelValueCardId.TabIndex = 2;
            this.labelValueCardId.Text = "#0";
            this.labelValueCardId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelSubTitle
            // 
            this.labelSubTitle.ForeColor = System.Drawing.Color.Gray;
            this.labelSubTitle.Location = new System.Drawing.Point(0, 34);
            this.labelSubTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelSubTitle.Name = "labelSubTitle";
            this.labelSubTitle.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.labelSubTitle.Size = new System.Drawing.Size(173, 24);
            this.labelSubTitle.TabIndex = 0;
            this.labelSubTitle.Text = "VirtualCard-0";
            // 
            // labelTitle
            // 
            this.labelTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(0, 6);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.labelTitle.Size = new System.Drawing.Size(173, 28);
            this.labelTitle.TabIndex = 1;
            this.labelTitle.Text = "默认虚拟卡";
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
            this.panelHeader.Size = new System.Drawing.Size(279, 36);
            this.panelHeader.TabIndex = 0;
            // 
            // flowHeaderRight
            // 
            this.flowHeaderRight.Controls.Add(this.buttonEdit);
            this.flowHeaderRight.Controls.Add(this.buttonDetail);
            this.flowHeaderRight.Controls.Add(this.buttonDelete);
            this.flowHeaderRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowHeaderRight.Gap = 6;
            this.flowHeaderRight.Location = new System.Drawing.Point(75, 0);
            this.flowHeaderRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowHeaderRight.Name = "flowHeaderRight";
            this.flowHeaderRight.Size = new System.Drawing.Size(204, 36);
            this.flowHeaderRight.TabIndex = 1;
            this.flowHeaderRight.Text = "flowHeaderRight";
            // 
            // buttonEdit
            // 
            this.buttonEdit.IconSvg = "EditOutlined";
            this.buttonEdit.Location = new System.Drawing.Point(140, 0);
            this.buttonEdit.Margin = new System.Windows.Forms.Padding(0);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Radius = 8;
            this.buttonEdit.Size = new System.Drawing.Size(64, 32);
            this.buttonEdit.TabIndex = 0;
            this.buttonEdit.Text = "编辑";
            this.buttonEdit.Type = AntdUI.TTypeMini.Primary;
            this.buttonEdit.WaveSize = 0;
            // 
            // buttonDetail
            // 
            this.buttonDetail.IconSvg = "ProfileOutlined";
            this.buttonDetail.Location = new System.Drawing.Point(70, 0);
            this.buttonDetail.Margin = new System.Windows.Forms.Padding(0);
            this.buttonDetail.Name = "buttonDetail";
            this.buttonDetail.Radius = 8;
            this.buttonDetail.Size = new System.Drawing.Size(64, 32);
            this.buttonDetail.TabIndex = 1;
            this.buttonDetail.Text = "详情";
            this.buttonDetail.WaveSize = 0;
            // 
            // buttonDelete
            // 
            this.buttonDelete.IconSvg = "DeleteOutlined";
            this.buttonDelete.Location = new System.Drawing.Point(0, 0);
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
            this.panelHeaderLeft.Controls.Add(this.buttonTypeTag);
            this.panelHeaderLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelHeaderLeft.Location = new System.Drawing.Point(0, 0);
            this.panelHeaderLeft.Margin = new System.Windows.Forms.Padding(0);
            this.panelHeaderLeft.Name = "panelHeaderLeft";
            this.panelHeaderLeft.Radius = 0;
            this.panelHeaderLeft.Size = new System.Drawing.Size(83, 36);
            this.panelHeaderLeft.TabIndex = 0;
            // 
            // buttonTypeTag
            // 
            this.buttonTypeTag.Font = new System.Drawing.Font("宋体", 9F);
            this.buttonTypeTag.Location = new System.Drawing.Point(0, 6);
            this.buttonTypeTag.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTypeTag.Name = "buttonTypeTag";
            this.buttonTypeTag.Radius = 8;
            this.buttonTypeTag.Size = new System.Drawing.Size(72, 24);
            this.buttonTypeTag.TabIndex = 0;
            this.buttonTypeTag.Text = "VIRTUAL";
            this.buttonTypeTag.Type = AntdUI.TTypeMini.Primary;
            this.buttonTypeTag.WaveSize = 0;
            // 
            // MotionCardControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelCard);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MotionCardControl";
            this.Size = new System.Drawing.Size(311, 201);
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
        private AntdUI.Button buttonTypeTag;
        private AntdUI.Label labelTitle;
        private AntdUI.Label labelValueCardId;
        private AntdUI.Panel panelBody;
        private AntdUI.Label labelSubTitle;
        private AntdUI.Panel panelBottom;
        private AntdUI.FlowPanel flowBottomLeft;
        private AntdUI.Label labelValueCore;
        private AntdUI.Label labelValueAxisCount;
        private AntdUI.Label labelValueExtModule;
        private AntdUI.FlowPanel flowBottomRight;
        private AntdUI.Button buttonStatusTag;
        private AntdUI.Label labelValueStatus;
    }
}