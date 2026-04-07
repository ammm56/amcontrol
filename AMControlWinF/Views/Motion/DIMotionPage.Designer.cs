namespace AMControlWinF.Views.Motion
{
    partial class DIMotionPage
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
            this.panelContentCard = new AntdUI.Panel();
            this.gridContent = new AntdUI.GridPanel();
            this.panelFooter = new AntdUI.Panel();
            this.paginationInputs = new AntdUI.Pagination();
            this.labelPageSummary = new AntdUI.Label();
            this.panelDetailCard = new AntdUI.Panel();
            this.diMotionDetailControl = new AMControlWinF.Views.Motion.DIMotionDetailControl();
            this.panelListCard = new AntdUI.Panel();
            this.diMotionVirtualListControl = new AMControlWinF.Views.Motion.DIMotionVirtualListControl();
            this.flowStats = new AntdUI.FlowPanel();
            this.panelScanCard = new AntdUI.Panel();
            this.labelScanValue = new AntdUI.Label();
            this.labelScanTitle = new AntdUI.Label();
            this.panelActiveCard = new AntdUI.Panel();
            this.labelActiveCount = new AntdUI.Label();
            this.labelActiveTitle = new AntdUI.Label();
            this.panelTotalCard = new AntdUI.Panel();
            this.labelTotalCount = new AntdUI.Label();
            this.labelTotalTitle = new AntdUI.Label();
            this.panelToolbar = new AntdUI.Panel();
            this.flowToolbarRight = new AntdUI.FlowPanel();
            this.inputSearch = new AntdUI.Input();
            this.flowToolbarLeft = new AntdUI.FlowPanel();
            this.labelSelectedCard = new AntdUI.Label();
            this.buttonSelectCard = new AntdUI.Button();
            this.panelRoot.SuspendLayout();
            this.panelContentCard.SuspendLayout();
            this.gridContent.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.panelDetailCard.SuspendLayout();
            this.panelListCard.SuspendLayout();
            this.flowStats.SuspendLayout();
            this.panelScanCard.SuspendLayout();
            this.panelActiveCard.SuspendLayout();
            this.panelTotalCard.SuspendLayout();
            this.panelToolbar.SuspendLayout();
            this.flowToolbarRight.SuspendLayout();
            this.flowToolbarLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.panelContentCard);
            this.panelRoot.Controls.Add(this.flowStats);
            this.panelRoot.Controls.Add(this.panelToolbar);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(8);
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(850, 680);
            this.panelRoot.TabIndex = 0;
            // 
            // panelContentCard
            // 
            this.panelContentCard.BackColor = System.Drawing.Color.Transparent;
            this.panelContentCard.Controls.Add(this.gridContent);
            this.panelContentCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContentCard.Location = new System.Drawing.Point(8, 140);
            this.panelContentCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelContentCard.Name = "panelContentCard";
            this.panelContentCard.Padding = new System.Windows.Forms.Padding(8);
            this.panelContentCard.Radius = 12;
            this.panelContentCard.Shadow = 4;
            this.panelContentCard.ShadowOpacity = 0F;
            this.panelContentCard.ShadowOpacityHover = 0F;
            this.panelContentCard.Size = new System.Drawing.Size(834, 532);
            this.panelContentCard.TabIndex = 2;
            // 
            // gridContent
            // 
            this.gridContent.Controls.Add(this.panelFooter);
            this.gridContent.Controls.Add(this.panelDetailCard);
            this.gridContent.Controls.Add(this.panelListCard);
            this.gridContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridContent.Location = new System.Drawing.Point(12, 12);
            this.gridContent.Margin = new System.Windows.Forms.Padding(0);
            this.gridContent.Name = "gridContent";
            this.gridContent.Size = new System.Drawing.Size(810, 508);
            this.gridContent.Span = "62% 38%;100%-100% 52";
            this.gridContent.TabIndex = 0;
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.paginationInputs);
            this.panelFooter.Controls.Add(this.labelPageSummary);
            this.panelFooter.Location = new System.Drawing.Point(0, 456);
            this.panelFooter.Margin = new System.Windows.Forms.Padding(0);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.panelFooter.Radius = 0;
            this.panelFooter.Size = new System.Drawing.Size(810, 52);
            this.panelFooter.TabIndex = 2;
            // 
            // paginationInputs
            // 
            this.paginationInputs.Dock = System.Windows.Forms.DockStyle.Right;
            this.paginationInputs.Location = new System.Drawing.Point(426, 8);
            this.paginationInputs.Margin = new System.Windows.Forms.Padding(0);
            this.paginationInputs.Name = "paginationInputs";
            this.paginationInputs.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.paginationInputs.Size = new System.Drawing.Size(384, 44);
            this.paginationInputs.SizeChangerWidth = 72;
            this.paginationInputs.TabIndex = 1;
            // 
            // labelPageSummary
            // 
            this.labelPageSummary.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelPageSummary.Location = new System.Drawing.Point(0, 8);
            this.labelPageSummary.Margin = new System.Windows.Forms.Padding(0);
            this.labelPageSummary.Name = "labelPageSummary";
            this.labelPageSummary.Size = new System.Drawing.Size(260, 44);
            this.labelPageSummary.TabIndex = 0;
            this.labelPageSummary.Text = "共 0 项";
            // 
            // panelDetailCard
            // 
            this.panelDetailCard.BackColor = System.Drawing.Color.Transparent;
            this.panelDetailCard.Controls.Add(this.diMotionDetailControl);
            this.panelDetailCard.Location = new System.Drawing.Point(502, 0);
            this.panelDetailCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelDetailCard.Name = "panelDetailCard";
            this.panelDetailCard.Padding = new System.Windows.Forms.Padding(8);
            this.panelDetailCard.Radius = 12;
            this.panelDetailCard.Shadow = 4;
            this.panelDetailCard.Size = new System.Drawing.Size(308, 456);
            this.panelDetailCard.TabIndex = 1;
            // 
            // diMotionDetailControl
            // 
            this.diMotionDetailControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diMotionDetailControl.Location = new System.Drawing.Point(12, 12);
            this.diMotionDetailControl.Margin = new System.Windows.Forms.Padding(0);
            this.diMotionDetailControl.Name = "diMotionDetailControl";
            this.diMotionDetailControl.Size = new System.Drawing.Size(284, 432);
            this.diMotionDetailControl.TabIndex = 0;
            // 
            // panelListCard
            // 
            this.panelListCard.BackColor = System.Drawing.Color.Transparent;
            this.panelListCard.Controls.Add(this.diMotionVirtualListControl);
            this.panelListCard.Location = new System.Drawing.Point(0, 0);
            this.panelListCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelListCard.Name = "panelListCard";
            this.panelListCard.Padding = new System.Windows.Forms.Padding(8);
            this.panelListCard.Radius = 12;
            this.panelListCard.Shadow = 4;
            this.panelListCard.Size = new System.Drawing.Size(502, 456);
            this.panelListCard.TabIndex = 0;
            // 
            // diMotionVirtualListControl
            // 
            this.diMotionVirtualListControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diMotionVirtualListControl.Location = new System.Drawing.Point(12, 12);
            this.diMotionVirtualListControl.Margin = new System.Windows.Forms.Padding(0);
            this.diMotionVirtualListControl.Name = "diMotionVirtualListControl";
            this.diMotionVirtualListControl.Size = new System.Drawing.Size(478, 432);
            this.diMotionVirtualListControl.TabIndex = 0;
            // 
            // flowStats
            // 
            this.flowStats.Controls.Add(this.panelScanCard);
            this.flowStats.Controls.Add(this.panelActiveCard);
            this.flowStats.Controls.Add(this.panelTotalCard);
            this.flowStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowStats.Gap = 8;
            this.flowStats.Location = new System.Drawing.Point(8, 52);
            this.flowStats.Margin = new System.Windows.Forms.Padding(0);
            this.flowStats.Name = "flowStats";
            this.flowStats.Padding = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.flowStats.Size = new System.Drawing.Size(834, 88);
            this.flowStats.TabIndex = 1;
            // 
            // panelScanCard
            // 
            this.panelScanCard.BackColor = System.Drawing.Color.Transparent;
            this.panelScanCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(229)))), ((int)(((byte)(235)))));
            this.panelScanCard.Controls.Add(this.labelScanValue);
            this.panelScanCard.Controls.Add(this.labelScanTitle);
            this.panelScanCard.Location = new System.Drawing.Point(380, 6);
            this.panelScanCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelScanCard.Name = "panelScanCard";
            this.panelScanCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelScanCard.Radius = 12;
            this.panelScanCard.Shadow = 4;
            this.panelScanCard.ShadowOpacity = 0.2F;
            this.panelScanCard.ShadowOpacityAnimation = true;
            this.panelScanCard.Size = new System.Drawing.Size(180, 72);
            this.panelScanCard.TabIndex = 2;
            // 
            // labelScanValue
            // 
            this.labelScanValue.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelScanValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.labelScanValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(119)))), ((int)(((byte)(255)))));
            this.labelScanValue.Location = new System.Drawing.Point(66, 16);
            this.labelScanValue.Name = "labelScanValue";
            this.labelScanValue.Size = new System.Drawing.Size(98, 40);
            this.labelScanValue.TabIndex = 1;
            this.labelScanValue.Text = "未启动";
            this.labelScanValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelScanTitle
            // 
            this.labelScanTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelScanTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelScanTitle.Location = new System.Drawing.Point(16, 16);
            this.labelScanTitle.Name = "labelScanTitle";
            this.labelScanTitle.Size = new System.Drawing.Size(64, 40);
            this.labelScanTitle.TabIndex = 0;
            this.labelScanTitle.Text = "扫描状态";
            // 
            // panelActiveCard
            // 
            this.panelActiveCard.BackColor = System.Drawing.Color.Transparent;
            this.panelActiveCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(229)))), ((int)(((byte)(235)))));
            this.panelActiveCard.Controls.Add(this.labelActiveCount);
            this.panelActiveCard.Controls.Add(this.labelActiveTitle);
            this.panelActiveCard.Location = new System.Drawing.Point(192, 6);
            this.panelActiveCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelActiveCard.Name = "panelActiveCard";
            this.panelActiveCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelActiveCard.Radius = 12;
            this.panelActiveCard.Shadow = 4;
            this.panelActiveCard.ShadowOpacity = 0.2F;
            this.panelActiveCard.ShadowOpacityAnimation = true;
            this.panelActiveCard.Size = new System.Drawing.Size(180, 72);
            this.panelActiveCard.TabIndex = 1;
            // 
            // labelActiveCount
            // 
            this.labelActiveCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelActiveCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelActiveCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(196)))), ((int)(((byte)(26)))));
            this.labelActiveCount.Location = new System.Drawing.Point(94, 16);
            this.labelActiveCount.Name = "labelActiveCount";
            this.labelActiveCount.Size = new System.Drawing.Size(70, 40);
            this.labelActiveCount.TabIndex = 1;
            this.labelActiveCount.Text = "0";
            // 
            // labelActiveTitle
            // 
            this.labelActiveTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelActiveTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelActiveTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(196)))), ((int)(((byte)(26)))));
            this.labelActiveTitle.Location = new System.Drawing.Point(16, 16);
            this.labelActiveTitle.Name = "labelActiveTitle";
            this.labelActiveTitle.Size = new System.Drawing.Size(72, 40);
            this.labelActiveTitle.TabIndex = 0;
            this.labelActiveTitle.Text = "当前激活";
            // 
            // panelTotalCard
            // 
            this.panelTotalCard.BackColor = System.Drawing.Color.Transparent;
            this.panelTotalCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(229)))), ((int)(((byte)(235)))));
            this.panelTotalCard.Controls.Add(this.labelTotalCount);
            this.panelTotalCard.Controls.Add(this.labelTotalTitle);
            this.panelTotalCard.Location = new System.Drawing.Point(4, 6);
            this.panelTotalCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelTotalCard.Name = "panelTotalCard";
            this.panelTotalCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelTotalCard.Radius = 12;
            this.panelTotalCard.Shadow = 4;
            this.panelTotalCard.ShadowOpacity = 0.2F;
            this.panelTotalCard.ShadowOpacityAnimation = true;
            this.panelTotalCard.Size = new System.Drawing.Size(180, 72);
            this.panelTotalCard.TabIndex = 0;
            // 
            // labelTotalCount
            // 
            this.labelTotalCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelTotalCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelTotalCount.Location = new System.Drawing.Point(104, 16);
            this.labelTotalCount.Name = "labelTotalCount";
            this.labelTotalCount.Size = new System.Drawing.Size(60, 40);
            this.labelTotalCount.TabIndex = 1;
            this.labelTotalCount.Text = "0";
            // 
            // labelTotalTitle
            // 
            this.labelTotalTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelTotalTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelTotalTitle.Location = new System.Drawing.Point(16, 16);
            this.labelTotalTitle.Name = "labelTotalTitle";
            this.labelTotalTitle.Size = new System.Drawing.Size(72, 40);
            this.labelTotalTitle.TabIndex = 0;
            this.labelTotalTitle.Text = "输入总数";
            // 
            // panelToolbar
            // 
            this.panelToolbar.Controls.Add(this.flowToolbarRight);
            this.panelToolbar.Controls.Add(this.flowToolbarLeft);
            this.panelToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelToolbar.Location = new System.Drawing.Point(8, 8);
            this.panelToolbar.Margin = new System.Windows.Forms.Padding(0);
            this.panelToolbar.Name = "panelToolbar";
            this.panelToolbar.Padding = new System.Windows.Forms.Padding(4);
            this.panelToolbar.Radius = 0;
            this.panelToolbar.Size = new System.Drawing.Size(834, 44);
            this.panelToolbar.TabIndex = 0;
            // 
            // flowToolbarRight
            // 
            this.flowToolbarRight.Controls.Add(this.inputSearch);
            this.flowToolbarRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowToolbarRight.Gap = 8;
            this.flowToolbarRight.Location = new System.Drawing.Point(594, 4);
            this.flowToolbarRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowToolbarRight.Name = "flowToolbarRight";
            this.flowToolbarRight.Size = new System.Drawing.Size(236, 36);
            this.flowToolbarRight.TabIndex = 1;
            // 
            // inputSearch
            // 
            this.inputSearch.Location = new System.Drawing.Point(0, 0);
            this.inputSearch.Margin = new System.Windows.Forms.Padding(0);
            this.inputSearch.Name = "inputSearch";
            this.inputSearch.PlaceholderText = "搜索名称 / 逻辑位 / 硬件位";
            this.inputSearch.Size = new System.Drawing.Size(236, 36);
            this.inputSearch.TabIndex = 0;
            this.inputSearch.WaveSize = 0;
            // 
            // flowToolbarLeft
            // 
            this.flowToolbarLeft.Controls.Add(this.labelSelectedCard);
            this.flowToolbarLeft.Controls.Add(this.buttonSelectCard);
            this.flowToolbarLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowToolbarLeft.Gap = 8;
            this.flowToolbarLeft.Location = new System.Drawing.Point(4, 4);
            this.flowToolbarLeft.Margin = new System.Windows.Forms.Padding(0);
            this.flowToolbarLeft.Name = "flowToolbarLeft";
            this.flowToolbarLeft.Size = new System.Drawing.Size(420, 36);
            this.flowToolbarLeft.TabIndex = 0;
            // 
            // labelSelectedCard
            // 
            this.labelSelectedCard.Location = new System.Drawing.Point(140, 0);
            this.labelSelectedCard.Margin = new System.Windows.Forms.Padding(0);
            this.labelSelectedCard.Name = "labelSelectedCard";
            this.labelSelectedCard.Size = new System.Drawing.Size(280, 36);
            this.labelSelectedCard.TabIndex = 1;
            this.labelSelectedCard.Text = "当前：全部控制卡";
            // 
            // buttonSelectCard
            // 
            this.buttonSelectCard.IconSvg = "AppstoreOutlined";
            this.buttonSelectCard.Location = new System.Drawing.Point(0, 0);
            this.buttonSelectCard.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSelectCard.Name = "buttonSelectCard";
            this.buttonSelectCard.Radius = 8;
            this.buttonSelectCard.Size = new System.Drawing.Size(132, 36);
            this.buttonSelectCard.TabIndex = 0;
            this.buttonSelectCard.Text = "选择控制卡";
            this.buttonSelectCard.Type = AntdUI.TTypeMini.Primary;
            this.buttonSelectCard.WaveSize = 0;
            // 
            // DIMotionPage
            // 
            this.Controls.Add(this.panelRoot);
            this.Name = "DIMotionPage";
            this.Size = new System.Drawing.Size(850, 680);
            this.panelRoot.ResumeLayout(false);
            this.panelContentCard.ResumeLayout(false);
            this.gridContent.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            this.panelDetailCard.ResumeLayout(false);
            this.panelListCard.ResumeLayout(false);
            this.flowStats.ResumeLayout(false);
            this.panelScanCard.ResumeLayout(false);
            this.panelActiveCard.ResumeLayout(false);
            this.panelTotalCard.ResumeLayout(false);
            this.panelToolbar.ResumeLayout(false);
            this.flowToolbarRight.ResumeLayout(false);
            this.flowToolbarLeft.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AntdUI.Panel panelRoot;
        private AntdUI.Panel panelToolbar;
        private AntdUI.FlowPanel flowToolbarLeft;
        private AntdUI.Button buttonSelectCard;
        private AntdUI.Label labelSelectedCard;
        private AntdUI.FlowPanel flowToolbarRight;
        private AntdUI.Input inputSearch;
        private AntdUI.FlowPanel flowStats;
        private AntdUI.Panel panelTotalCard;
        private AntdUI.Label labelTotalCount;
        private AntdUI.Label labelTotalTitle;
        private AntdUI.Panel panelActiveCard;
        private AntdUI.Label labelActiveCount;
        private AntdUI.Label labelActiveTitle;
        private AntdUI.Panel panelScanCard;
        private AntdUI.Label labelScanValue;
        private AntdUI.Label labelScanTitle;
        private AntdUI.Panel panelContentCard;
        private AntdUI.GridPanel gridContent;
        private AntdUI.Panel panelListCard;
        private AntdUI.Panel panelDetailCard;
        private DIMotionVirtualListControl diMotionVirtualListControl;
        private DIMotionDetailControl diMotionDetailControl;
        private AntdUI.Panel panelFooter;
        private AntdUI.Label labelPageSummary;
        private AntdUI.Pagination paginationInputs;
    }
}