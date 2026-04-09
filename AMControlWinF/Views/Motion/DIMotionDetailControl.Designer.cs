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
            this.panelDetail = new AntdUI.Panel();
            this.panelScroll = new System.Windows.Forms.Panel();
            this.labelRemarkValue = new AntdUI.Label();
            this.labelRemarkTitle = new AntdUI.Label();
            this.labelDescriptionValue = new AntdUI.Label();
            this.labelDescriptionTitle = new AntdUI.Label();
            this.panelTagLink = new AntdUI.Panel();
            this.labelTagLinkValue = new AntdUI.Label();
            this.labelTagLinkKey = new AntdUI.Label();
            this.panelTagLastUpdate = new AntdUI.Panel();
            this.labelTagLastUpdateValue = new AntdUI.Label();
            this.labelTagLastUpdateKey = new AntdUI.Label();
            this.panelTagState = new AntdUI.Panel();
            this.labelTagStateValue = new AntdUI.Label();
            this.labelTagStateKey = new AntdUI.Label();
            this.panelTagHardware = new AntdUI.Panel();
            this.labelTagHardwareValue = new AntdUI.Label();
            this.labelTagHardwareKey = new AntdUI.Label();
            this.panelTagCard = new AntdUI.Panel();
            this.labelTagCardValue = new AntdUI.Label();
            this.labelTagCardKey = new AntdUI.Label();
            this.panelTagCategory = new AntdUI.Panel();
            this.labelTagCategoryValue = new AntdUI.Label();
            this.labelTagCategoryKey = new AntdUI.Label();
            this.panelTagLogic = new AntdUI.Panel();
            this.labelTagLogicValue = new AntdUI.Label();
            this.labelTagLogicKey = new AntdUI.Label();
            this.panelHeader = new AntdUI.Panel();
            this.labelSubTitle = new AntdUI.Label();
            this.labelTitle = new AntdUI.Label();
            this.panelEmpty = new AntdUI.Panel();
            this.labelEmpty = new AntdUI.Label();
            this.panelRoot.SuspendLayout();
            this.panelDetail.SuspendLayout();
            this.panelScroll.SuspendLayout();
            this.panelTagLink.SuspendLayout();
            this.panelTagLastUpdate.SuspendLayout();
            this.panelTagState.SuspendLayout();
            this.panelTagHardware.SuspendLayout();
            this.panelTagCard.SuspendLayout();
            this.panelTagCategory.SuspendLayout();
            this.panelTagLogic.SuspendLayout();
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
            this.panelRoot.Size = new System.Drawing.Size(260, 520);
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
            this.panelDetail.Size = new System.Drawing.Size(260, 520);
            this.panelDetail.TabIndex = 1;
            // 
            // panelScroll
            // 
            this.panelScroll.Controls.Add(this.labelRemarkValue);
            this.panelScroll.Controls.Add(this.labelRemarkTitle);
            this.panelScroll.Controls.Add(this.labelDescriptionValue);
            this.panelScroll.Controls.Add(this.labelDescriptionTitle);
            this.panelScroll.Controls.Add(this.panelTagLink);
            this.panelScroll.Controls.Add(this.panelTagLastUpdate);
            this.panelScroll.Controls.Add(this.panelTagState);
            this.panelScroll.Controls.Add(this.panelTagHardware);
            this.panelScroll.Controls.Add(this.panelTagCard);
            this.panelScroll.Controls.Add(this.panelTagCategory);
            this.panelScroll.Controls.Add(this.panelTagLogic);
            this.panelScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelScroll.Location = new System.Drawing.Point(0, 60);
            this.panelScroll.Margin = new System.Windows.Forms.Padding(0);
            this.panelScroll.Name = "panelScroll";
            this.panelScroll.Padding = new System.Windows.Forms.Padding(14, 12, 14, 12);
            this.panelScroll.Size = new System.Drawing.Size(260, 460);
            this.panelScroll.TabIndex = 1;
            // 
            // labelRemarkValue
            // 
            this.labelRemarkValue.Location = new System.Drawing.Point(14, 310);
            this.labelRemarkValue.Name = "labelRemarkValue";
            this.labelRemarkValue.Size = new System.Drawing.Size(220, 48);
            this.labelRemarkValue.TabIndex = 11;
            this.labelRemarkValue.Text = "—";
            this.labelRemarkValue.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            // 
            // labelRemarkTitle
            // 
            this.labelRemarkTitle.ForeColor = System.Drawing.Color.Gray;
            this.labelRemarkTitle.Location = new System.Drawing.Point(14, 284);
            this.labelRemarkTitle.Name = "labelRemarkTitle";
            this.labelRemarkTitle.Size = new System.Drawing.Size(220, 20);
            this.labelRemarkTitle.TabIndex = 10;
            this.labelRemarkTitle.Text = "备注";
            // 
            // labelDescriptionValue
            // 
            this.labelDescriptionValue.Location = new System.Drawing.Point(14, 241);
            this.labelDescriptionValue.Name = "labelDescriptionValue";
            this.labelDescriptionValue.Size = new System.Drawing.Size(220, 48);
            this.labelDescriptionValue.TabIndex = 9;
            this.labelDescriptionValue.Text = "—";
            this.labelDescriptionValue.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            // 
            // labelDescriptionTitle
            // 
            this.labelDescriptionTitle.ForeColor = System.Drawing.Color.Gray;
            this.labelDescriptionTitle.Location = new System.Drawing.Point(14, 215);
            this.labelDescriptionTitle.Name = "labelDescriptionTitle";
            this.labelDescriptionTitle.Size = new System.Drawing.Size(220, 20);
            this.labelDescriptionTitle.TabIndex = 8;
            this.labelDescriptionTitle.Text = "说明";
            // 
            // panelTagLink
            // 
            this.panelTagLink.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTagLink.Controls.Add(this.labelTagLinkValue);
            this.panelTagLink.Controls.Add(this.labelTagLinkKey);
            this.panelTagLink.Location = new System.Drawing.Point(8, 182);
            this.panelTagLink.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagLink.Name = "panelTagLink";
            this.panelTagLink.Radius = 0;
            this.panelTagLink.Size = new System.Drawing.Size(252, 24);
            this.panelTagLink.TabIndex = 6;
            // 
            // labelTagLinkValue
            // 
            this.labelTagLinkValue.AutoEllipsis = true;
            this.labelTagLinkValue.Location = new System.Drawing.Point(70, 0);
            this.labelTagLinkValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagLinkValue.Name = "labelTagLinkValue";
            this.labelTagLinkValue.Size = new System.Drawing.Size(174, 24);
            this.labelTagLinkValue.TabIndex = 1;
            this.labelTagLinkValue.Text = "—";
            // 
            // labelTagLinkKey
            // 
            this.labelTagLinkKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagLinkKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagLinkKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagLinkKey.Name = "labelTagLinkKey";
            this.labelTagLinkKey.Size = new System.Drawing.Size(64, 24);
            this.labelTagLinkKey.TabIndex = 0;
            this.labelTagLinkKey.Text = "使用对象";
            this.labelTagLinkKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagLastUpdate
            // 
            this.panelTagLastUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTagLastUpdate.Controls.Add(this.labelTagLastUpdateValue);
            this.panelTagLastUpdate.Controls.Add(this.labelTagLastUpdateKey);
            this.panelTagLastUpdate.Location = new System.Drawing.Point(8, 152);
            this.panelTagLastUpdate.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagLastUpdate.Name = "panelTagLastUpdate";
            this.panelTagLastUpdate.Radius = 0;
            this.panelTagLastUpdate.Size = new System.Drawing.Size(252, 24);
            this.panelTagLastUpdate.TabIndex = 5;
            // 
            // labelTagLastUpdateValue
            // 
            this.labelTagLastUpdateValue.AutoEllipsis = true;
            this.labelTagLastUpdateValue.Location = new System.Drawing.Point(70, 0);
            this.labelTagLastUpdateValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagLastUpdateValue.Name = "labelTagLastUpdateValue";
            this.labelTagLastUpdateValue.Size = new System.Drawing.Size(174, 24);
            this.labelTagLastUpdateValue.TabIndex = 1;
            this.labelTagLastUpdateValue.Text = "—";
            // 
            // labelTagLastUpdateKey
            // 
            this.labelTagLastUpdateKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagLastUpdateKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagLastUpdateKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagLastUpdateKey.Name = "labelTagLastUpdateKey";
            this.labelTagLastUpdateKey.Size = new System.Drawing.Size(64, 24);
            this.labelTagLastUpdateKey.TabIndex = 0;
            this.labelTagLastUpdateKey.Text = "最后触发";
            this.labelTagLastUpdateKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagState
            // 
            this.panelTagState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTagState.Controls.Add(this.labelTagStateValue);
            this.panelTagState.Controls.Add(this.labelTagStateKey);
            this.panelTagState.Location = new System.Drawing.Point(8, 122);
            this.panelTagState.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagState.Name = "panelTagState";
            this.panelTagState.Radius = 0;
            this.panelTagState.Size = new System.Drawing.Size(252, 24);
            this.panelTagState.TabIndex = 4;
            // 
            // labelTagStateValue
            // 
            this.labelTagStateValue.AutoEllipsis = true;
            this.labelTagStateValue.Location = new System.Drawing.Point(70, 0);
            this.labelTagStateValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagStateValue.Name = "labelTagStateValue";
            this.labelTagStateValue.Size = new System.Drawing.Size(174, 24);
            this.labelTagStateValue.TabIndex = 1;
            this.labelTagStateValue.Text = "—";
            // 
            // labelTagStateKey
            // 
            this.labelTagStateKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagStateKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagStateKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagStateKey.Name = "labelTagStateKey";
            this.labelTagStateKey.Size = new System.Drawing.Size(64, 24);
            this.labelTagStateKey.TabIndex = 0;
            this.labelTagStateKey.Text = "当前状态";
            this.labelTagStateKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagHardware
            // 
            this.panelTagHardware.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTagHardware.Controls.Add(this.labelTagHardwareValue);
            this.panelTagHardware.Controls.Add(this.labelTagHardwareKey);
            this.panelTagHardware.Location = new System.Drawing.Point(8, 92);
            this.panelTagHardware.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagHardware.Name = "panelTagHardware";
            this.panelTagHardware.Radius = 0;
            this.panelTagHardware.Size = new System.Drawing.Size(252, 24);
            this.panelTagHardware.TabIndex = 3;
            // 
            // labelTagHardwareValue
            // 
            this.labelTagHardwareValue.AutoEllipsis = true;
            this.labelTagHardwareValue.Location = new System.Drawing.Point(70, 0);
            this.labelTagHardwareValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagHardwareValue.Name = "labelTagHardwareValue";
            this.labelTagHardwareValue.Size = new System.Drawing.Size(174, 24);
            this.labelTagHardwareValue.TabIndex = 1;
            this.labelTagHardwareValue.Text = "—";
            // 
            // labelTagHardwareKey
            // 
            this.labelTagHardwareKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagHardwareKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagHardwareKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagHardwareKey.Name = "labelTagHardwareKey";
            this.labelTagHardwareKey.Size = new System.Drawing.Size(64, 24);
            this.labelTagHardwareKey.TabIndex = 0;
            this.labelTagHardwareKey.Text = "硬件地址";
            this.labelTagHardwareKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagCard
            // 
            this.panelTagCard.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTagCard.Controls.Add(this.labelTagCardValue);
            this.panelTagCard.Controls.Add(this.labelTagCardKey);
            this.panelTagCard.Location = new System.Drawing.Point(8, 62);
            this.panelTagCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagCard.Name = "panelTagCard";
            this.panelTagCard.Radius = 0;
            this.panelTagCard.Size = new System.Drawing.Size(252, 24);
            this.panelTagCard.TabIndex = 2;
            // 
            // labelTagCardValue
            // 
            this.labelTagCardValue.AutoEllipsis = true;
            this.labelTagCardValue.Location = new System.Drawing.Point(70, 0);
            this.labelTagCardValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagCardValue.Name = "labelTagCardValue";
            this.labelTagCardValue.Size = new System.Drawing.Size(174, 24);
            this.labelTagCardValue.TabIndex = 1;
            this.labelTagCardValue.Text = "—";
            // 
            // labelTagCardKey
            // 
            this.labelTagCardKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagCardKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagCardKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagCardKey.Name = "labelTagCardKey";
            this.labelTagCardKey.Size = new System.Drawing.Size(64, 24);
            this.labelTagCardKey.TabIndex = 0;
            this.labelTagCardKey.Text = "控制卡";
            this.labelTagCardKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagCategory
            // 
            this.panelTagCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTagCategory.Controls.Add(this.labelTagCategoryValue);
            this.panelTagCategory.Controls.Add(this.labelTagCategoryKey);
            this.panelTagCategory.Location = new System.Drawing.Point(8, 32);
            this.panelTagCategory.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagCategory.Name = "panelTagCategory";
            this.panelTagCategory.Radius = 0;
            this.panelTagCategory.Size = new System.Drawing.Size(252, 24);
            this.panelTagCategory.TabIndex = 1;
            // 
            // labelTagCategoryValue
            // 
            this.labelTagCategoryValue.AutoEllipsis = true;
            this.labelTagCategoryValue.Location = new System.Drawing.Point(70, 0);
            this.labelTagCategoryValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagCategoryValue.Name = "labelTagCategoryValue";
            this.labelTagCategoryValue.Size = new System.Drawing.Size(174, 24);
            this.labelTagCategoryValue.TabIndex = 1;
            this.labelTagCategoryValue.Text = "—";
            // 
            // labelTagCategoryKey
            // 
            this.labelTagCategoryKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagCategoryKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagCategoryKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagCategoryKey.Name = "labelTagCategoryKey";
            this.labelTagCategoryKey.Size = new System.Drawing.Size(64, 24);
            this.labelTagCategoryKey.TabIndex = 0;
            this.labelTagCategoryKey.Text = "分类";
            this.labelTagCategoryKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagLogic
            // 
            this.panelTagLogic.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTagLogic.Controls.Add(this.labelTagLogicValue);
            this.panelTagLogic.Controls.Add(this.labelTagLogicKey);
            this.panelTagLogic.Location = new System.Drawing.Point(8, 2);
            this.panelTagLogic.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagLogic.Name = "panelTagLogic";
            this.panelTagLogic.Radius = 0;
            this.panelTagLogic.Size = new System.Drawing.Size(252, 24);
            this.panelTagLogic.TabIndex = 0;
            // 
            // labelTagLogicValue
            // 
            this.labelTagLogicValue.AutoEllipsis = true;
            this.labelTagLogicValue.Location = new System.Drawing.Point(70, 0);
            this.labelTagLogicValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagLogicValue.Name = "labelTagLogicValue";
            this.labelTagLogicValue.Size = new System.Drawing.Size(174, 24);
            this.labelTagLogicValue.TabIndex = 1;
            this.labelTagLogicValue.Text = "—";
            // 
            // labelTagLogicKey
            // 
            this.labelTagLogicKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagLogicKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagLogicKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagLogicKey.Name = "labelTagLogicKey";
            this.labelTagLogicKey.ShadowOpacity = 0F;
            this.labelTagLogicKey.Size = new System.Drawing.Size(64, 24);
            this.labelTagLogicKey.TabIndex = 0;
            this.labelTagLogicKey.Text = "逻辑位";
            this.labelTagLogicKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.panelHeader.Size = new System.Drawing.Size(260, 60);
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
            this.labelSubTitle.Size = new System.Drawing.Size(244, 22);
            this.labelSubTitle.TabIndex = 1;
            this.labelSubTitle.Text = "—";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoEllipsis = true;
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(8, 8);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(244, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "—";
            // 
            // panelEmpty
            // 
            this.panelEmpty.Controls.Add(this.labelEmpty);
            this.panelEmpty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEmpty.Location = new System.Drawing.Point(0, 0);
            this.panelEmpty.Margin = new System.Windows.Forms.Padding(0);
            this.panelEmpty.Name = "panelEmpty";
            this.panelEmpty.Radius = 0;
            this.panelEmpty.Size = new System.Drawing.Size(260, 520);
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
            this.labelEmpty.Size = new System.Drawing.Size(260, 520);
            this.labelEmpty.TabIndex = 0;
            this.labelEmpty.Text = "请选择左侧 DI 卡片查看详情";
            this.labelEmpty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DIMotionDetailControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "DIMotionDetailControl";
            this.Size = new System.Drawing.Size(260, 520);
            this.panelRoot.ResumeLayout(false);
            this.panelDetail.ResumeLayout(false);
            this.panelScroll.ResumeLayout(false);
            this.panelTagLink.ResumeLayout(false);
            this.panelTagLastUpdate.ResumeLayout(false);
            this.panelTagState.ResumeLayout(false);
            this.panelTagHardware.ResumeLayout(false);
            this.panelTagCard.ResumeLayout(false);
            this.panelTagCategory.ResumeLayout(false);
            this.panelTagLogic.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.panelEmpty.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AntdUI.Panel panelRoot;
        private AntdUI.Panel panelEmpty;
        private AntdUI.Label labelEmpty;
        private AntdUI.Panel panelDetail;
        private AntdUI.Panel panelHeader;
        private AntdUI.Label labelTitle;
        private AntdUI.Label labelSubTitle;
        private System.Windows.Forms.Panel panelScroll;
        private AntdUI.Panel panelTagLogic;
        private AntdUI.Label labelTagLogicKey;
        private AntdUI.Label labelTagLogicValue;
        private AntdUI.Panel panelTagCategory;
        private AntdUI.Label labelTagCategoryKey;
        private AntdUI.Label labelTagCategoryValue;
        private AntdUI.Panel panelTagCard;
        private AntdUI.Label labelTagCardKey;
        private AntdUI.Label labelTagCardValue;
        private AntdUI.Panel panelTagHardware;
        private AntdUI.Label labelTagHardwareKey;
        private AntdUI.Label labelTagHardwareValue;
        private AntdUI.Panel panelTagState;
        private AntdUI.Label labelTagStateKey;
        private AntdUI.Label labelTagStateValue;
        private AntdUI.Panel panelTagLastUpdate;
        private AntdUI.Label labelTagLastUpdateKey;
        private AntdUI.Label labelTagLastUpdateValue;
        private AntdUI.Panel panelTagLink;
        private AntdUI.Label labelTagLinkKey;
        private AntdUI.Label labelTagLinkValue;
        private AntdUI.Label labelDescriptionTitle;
        private AntdUI.Label labelDescriptionValue;
        private AntdUI.Label labelRemarkTitle;
        private AntdUI.Label labelRemarkValue;
    }
}