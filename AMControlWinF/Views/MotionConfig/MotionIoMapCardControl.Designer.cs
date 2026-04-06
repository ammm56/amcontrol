namespace AMControlWinF.Views.MotionConfig
{
    partial class MotionIoMapCardControl
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
            this.labelValueHardwareBit = new AntdUI.Label();
            this.labelValueCore = new AntdUI.Label();
            this.labelValueLogicalBit = new AntdUI.Label();
            this.panelBody = new AntdUI.Panel();
            this.labelValueExtModule = new AntdUI.Label();
            this.buttonDetail = new AntdUI.Button();
            this.labelTitle = new AntdUI.Label();
            this.panelHeader = new AntdUI.Panel();
            this.flowHeaderRight = new AntdUI.FlowPanel();
            this.buttonEdit = new AntdUI.Button();
            this.buttonDelete = new AntdUI.Button();
            this.panelHeaderLeft = new AntdUI.Panel();
            this.buttonIoTypeTag = new AntdUI.Button();
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
            this.panelCard.Size = new System.Drawing.Size(240, 160);
            this.panelCard.TabIndex = 0;
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.flowBottomRight);
            this.panelBottom.Controls.Add(this.flowBottomLeft);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(16, 104);
            this.panelBottom.Margin = new System.Windows.Forms.Padding(0);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.panelBottom.Radius = 0;
            this.panelBottom.Size = new System.Drawing.Size(208, 40);
            this.panelBottom.TabIndex = 2;
            this.panelBottom.Text = "panelBottom";
            // 
            // flowBottomRight
            // 
            this.flowBottomRight.Align = AntdUI.TAlignFlow.Right;
            this.flowBottomRight.Controls.Add(this.labelValueStatus);
            this.flowBottomRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowBottomRight.Location = new System.Drawing.Point(144, 8);
            this.flowBottomRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowBottomRight.Name = "flowBottomRight";
            this.flowBottomRight.Size = new System.Drawing.Size(64, 32);
            this.flowBottomRight.TabIndex = 1;
            this.flowBottomRight.Text = "flowBottomRight";
            // 
            // labelValueStatus
            // 
            this.labelValueStatus.Location = new System.Drawing.Point(0, 0);
            this.labelValueStatus.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueStatus.Name = "labelValueStatus";
            this.labelValueStatus.Size = new System.Drawing.Size(64, 32);
            this.labelValueStatus.TabIndex = 0;
            this.labelValueStatus.Text = "● 启用";
            this.labelValueStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // flowBottomLeft
            // 
            this.flowBottomLeft.Controls.Add(this.labelValueHardwareBit);
            this.flowBottomLeft.Controls.Add(this.labelValueCore);
            this.flowBottomLeft.Controls.Add(this.labelValueLogicalBit);
            this.flowBottomLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowBottomLeft.Gap = 6;
            this.flowBottomLeft.Location = new System.Drawing.Point(0, 8);
            this.flowBottomLeft.Margin = new System.Windows.Forms.Padding(0);
            this.flowBottomLeft.Name = "flowBottomLeft";
            this.flowBottomLeft.Size = new System.Drawing.Size(180, 32);
            this.flowBottomLeft.TabIndex = 0;
            this.flowBottomLeft.Text = "flowBottomLeft";
            // 
            // labelValueHardwareBit
            // 
            this.labelValueHardwareBit.Location = new System.Drawing.Point(89, 0);
            this.labelValueHardwareBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueHardwareBit.Name = "labelValueHardwareBit";
            this.labelValueHardwareBit.Size = new System.Drawing.Size(44, 32);
            this.labelValueHardwareBit.TabIndex = 1;
            this.labelValueHardwareBit.Text = "位号: 0";
            // 
            // labelValueCore
            // 
            this.labelValueCore.Location = new System.Drawing.Point(43, 0);
            this.labelValueCore.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueCore.Name = "labelValueCore";
            this.labelValueCore.Size = new System.Drawing.Size(40, 32);
            this.labelValueCore.TabIndex = 0;
            this.labelValueCore.Text = "核: 1";
            // 
            // labelValueLogicalBit
            // 
            this.labelValueLogicalBit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelValueLogicalBit.Location = new System.Drawing.Point(0, 0);
            this.labelValueLogicalBit.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueLogicalBit.Name = "labelValueLogicalBit";
            this.labelValueLogicalBit.Size = new System.Drawing.Size(37, 32);
            this.labelValueLogicalBit.TabIndex = 2;
            this.labelValueLogicalBit.Text = "L1";
            // 
            // panelBody
            // 
            this.panelBody.Controls.Add(this.labelValueExtModule);
            this.panelBody.Controls.Add(this.buttonDetail);
            this.panelBody.Controls.Add(this.labelTitle);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(16, 52);
            this.panelBody.Margin = new System.Windows.Forms.Padding(0);
            this.panelBody.Name = "panelBody";
            this.panelBody.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.panelBody.Radius = 0;
            this.panelBody.Size = new System.Drawing.Size(208, 92);
            this.panelBody.TabIndex = 1;
            // 
            // labelValueExtModule
            // 
            this.labelValueExtModule.ForeColor = System.Drawing.Color.Gray;
            this.labelValueExtModule.Location = new System.Drawing.Point(132, 34);
            this.labelValueExtModule.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueExtModule.Name = "labelValueExtModule";
            this.labelValueExtModule.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelValueExtModule.Size = new System.Drawing.Size(76, 24);
            this.labelValueExtModule.TabIndex = 3;
            this.labelValueExtModule.Text = "板载";
            this.labelValueExtModule.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonDetail
            // 
            this.buttonDetail.IconSvg = "ProfileOutlined";
            this.buttonDetail.Location = new System.Drawing.Point(144, 2);
            this.buttonDetail.Margin = new System.Windows.Forms.Padding(0);
            this.buttonDetail.Name = "buttonDetail";
            this.buttonDetail.Radius = 8;
            this.buttonDetail.Size = new System.Drawing.Size(64, 32);
            this.buttonDetail.TabIndex = 1;
            this.buttonDetail.Text = "详情";
            this.buttonDetail.WaveSize = 0;
            // 
            // labelTitle
            // 
            this.labelTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(0, 6);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.labelTitle.Size = new System.Drawing.Size(144, 46);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "DI-1";
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
            this.panelHeader.Size = new System.Drawing.Size(208, 36);
            this.panelHeader.TabIndex = 0;
            // 
            // flowHeaderRight
            // 
            this.flowHeaderRight.Align = AntdUI.TAlignFlow.RightCenter;
            this.flowHeaderRight.Controls.Add(this.buttonEdit);
            this.flowHeaderRight.Controls.Add(this.buttonDelete);
            this.flowHeaderRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowHeaderRight.Gap = 6;
            this.flowHeaderRight.Location = new System.Drawing.Point(72, 0);
            this.flowHeaderRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowHeaderRight.Name = "flowHeaderRight";
            this.flowHeaderRight.Size = new System.Drawing.Size(136, 36);
            this.flowHeaderRight.TabIndex = 1;
            this.flowHeaderRight.Text = "flowHeaderRight";
            // 
            // buttonEdit
            // 
            this.buttonEdit.IconSvg = "EditOutlined";
            this.buttonEdit.Location = new System.Drawing.Point(72, 0);
            this.buttonEdit.Margin = new System.Windows.Forms.Padding(0);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Radius = 8;
            this.buttonEdit.Size = new System.Drawing.Size(64, 32);
            this.buttonEdit.TabIndex = 0;
            this.buttonEdit.Text = "编辑";
            this.buttonEdit.Type = AntdUI.TTypeMini.Primary;
            this.buttonEdit.WaveSize = 0;
            // 
            // buttonDelete
            // 
            this.buttonDelete.IconSvg = "DeleteOutlined";
            this.buttonDelete.Location = new System.Drawing.Point(2, 0);
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
            this.panelHeaderLeft.Controls.Add(this.buttonIoTypeTag);
            this.panelHeaderLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelHeaderLeft.Location = new System.Drawing.Point(0, 0);
            this.panelHeaderLeft.Margin = new System.Windows.Forms.Padding(0);
            this.panelHeaderLeft.Name = "panelHeaderLeft";
            this.panelHeaderLeft.Radius = 0;
            this.panelHeaderLeft.Size = new System.Drawing.Size(72, 36);
            this.panelHeaderLeft.TabIndex = 0;
            // 
            // buttonIoTypeTag
            // 
            this.buttonIoTypeTag.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.buttonIoTypeTag.Location = new System.Drawing.Point(0, 6);
            this.buttonIoTypeTag.Margin = new System.Windows.Forms.Padding(0);
            this.buttonIoTypeTag.Name = "buttonIoTypeTag";
            this.buttonIoTypeTag.Radius = 8;
            this.buttonIoTypeTag.Size = new System.Drawing.Size(58, 24);
            this.buttonIoTypeTag.TabIndex = 0;
            this.buttonIoTypeTag.Text = "DI";
            this.buttonIoTypeTag.WaveSize = 0;
            // 
            // MotionIoMapCardControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelCard);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MotionIoMapCardControl";
            this.Size = new System.Drawing.Size(240, 160);
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
        private AntdUI.Button buttonIoTypeTag;
        private AntdUI.Panel panelBody;
        private AntdUI.Label labelTitle;
        private AntdUI.Label labelValueLogicalBit;
        private AntdUI.Label labelValueExtModule;
        private AntdUI.Panel panelBottom;
        private AntdUI.FlowPanel flowBottomLeft;
        private AntdUI.Label labelValueCore;
        private AntdUI.Label labelValueHardwareBit;
        private AntdUI.FlowPanel flowBottomRight;
        private AntdUI.Label labelValueStatus;
    }
}