namespace AMControlWinF.Views.MotionConfig
{
    partial class MotionAxisManagementPage
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
            this.panelRoot = new AntdUI.Panel();
            this.panelCardsHost = new AntdUI.Panel();
            this.flowCards = new AntdUI.FlowPanel();
            this.panelPagination = new AntdUI.Panel();
            this.paginationCards = new AntdUI.Pagination();
            this.labelPageSummary = new AntdUI.Label();
            this.labelCardsTitle = new AntdUI.Label();
            this.panelPlaceholder = new AntdUI.Panel();
            this.panelToolbar = new AntdUI.Panel();
            this.flowToolbarRight = new AntdUI.FlowPanel();
            this.buttonAddAxis = new AntdUI.Button();
            this.buttonRefresh = new AntdUI.Button();
            this.flowToolbarLeft = new AntdUI.FlowPanel();
            this.labelSelectedCard = new AntdUI.Label();
            this.buttonSelectCard = new AntdUI.Button();
            this.panelRoot.SuspendLayout();
            this.panelCardsHost.SuspendLayout();
            this.panelPagination.SuspendLayout();
            this.panelToolbar.SuspendLayout();
            this.flowToolbarRight.SuspendLayout();
            this.flowToolbarLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.panelCardsHost);
            this.panelRoot.Controls.Add(this.panelPlaceholder);
            this.panelRoot.Controls.Add(this.panelToolbar);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Margin = new System.Windows.Forms.Padding(0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(8);
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(850, 680);
            this.panelRoot.TabIndex = 0;
            // 
            // panelCardsHost
            // 
            this.panelCardsHost.BackColor = System.Drawing.Color.Transparent;
            this.panelCardsHost.Controls.Add(this.flowCards);
            this.panelCardsHost.Controls.Add(this.panelPagination);
            this.panelCardsHost.Controls.Add(this.labelCardsTitle);
            this.panelCardsHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCardsHost.Location = new System.Drawing.Point(8, 62);
            this.panelCardsHost.Margin = new System.Windows.Forms.Padding(0);
            this.panelCardsHost.Name = "panelCardsHost";
            this.panelCardsHost.Padding = new System.Windows.Forms.Padding(8);
            this.panelCardsHost.Radius = 12;
            this.panelCardsHost.Shadow = 4;
            this.panelCardsHost.Size = new System.Drawing.Size(834, 610);
            this.panelCardsHost.TabIndex = 2;
            // 
            // flowCards
            // 
            this.flowCards.AutoScroll = true;
            this.flowCards.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowCards.Gap = 12;
            this.flowCards.Location = new System.Drawing.Point(12, 48);
            this.flowCards.Margin = new System.Windows.Forms.Padding(0);
            this.flowCards.Name = "flowCards";
            this.flowCards.Size = new System.Drawing.Size(810, 502);
            this.flowCards.TabIndex = 1;
            this.flowCards.Text = "flowCards";
            // 
            // panelPagination
            // 
            this.panelPagination.Controls.Add(this.paginationCards);
            this.panelPagination.Controls.Add(this.labelPageSummary);
            this.panelPagination.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelPagination.Location = new System.Drawing.Point(12, 550);
            this.panelPagination.Margin = new System.Windows.Forms.Padding(0);
            this.panelPagination.Name = "panelPagination";
            this.panelPagination.Padding = new System.Windows.Forms.Padding(4, 8, 4, 0);
            this.panelPagination.Radius = 0;
            this.panelPagination.Size = new System.Drawing.Size(810, 48);
            this.panelPagination.TabIndex = 2;
            this.panelPagination.Text = "panelPagination";
            // 
            // paginationCards
            // 
            this.paginationCards.Dock = System.Windows.Forms.DockStyle.Right;
            this.paginationCards.Location = new System.Drawing.Point(244, 8);
            this.paginationCards.Margin = new System.Windows.Forms.Padding(0);
            this.paginationCards.Name = "paginationCards";
            this.paginationCards.PageSize = 9;
            this.paginationCards.PageSizeOptions = new int[] { 3, 6, 9, 12, 24, 36 };
            this.paginationCards.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.paginationCards.ShowSizeChanger = true;
            this.paginationCards.Size = new System.Drawing.Size(562, 40);
            this.paginationCards.TabIndex = 1;
            this.paginationCards.Text = "paginationCards";
            // 
            // labelPageSummary
            // 
            this.labelPageSummary.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelPageSummary.Location = new System.Drawing.Point(4, 8);
            this.labelPageSummary.Margin = new System.Windows.Forms.Padding(0);
            this.labelPageSummary.Name = "labelPageSummary";
            this.labelPageSummary.Size = new System.Drawing.Size(240, 40);
            this.labelPageSummary.TabIndex = 0;
            this.labelPageSummary.Text = "共 0 项";
            // 
            // labelCardsTitle
            // 
            this.labelCardsTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelCardsTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.labelCardsTitle.Location = new System.Drawing.Point(12, 12);
            this.labelCardsTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelCardsTitle.Name = "labelCardsTitle";
            this.labelCardsTitle.Size = new System.Drawing.Size(810, 36);
            this.labelCardsTitle.TabIndex = 0;
            this.labelCardsTitle.Text = "轴列表";
            this.labelCardsTitle.Visible = false;
            // 
            // panelPlaceholder
            // 
            this.panelPlaceholder.BackColor = System.Drawing.Color.Transparent;
            this.panelPlaceholder.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPlaceholder.Location = new System.Drawing.Point(8, 52);
            this.panelPlaceholder.Margin = new System.Windows.Forms.Padding(0);
            this.panelPlaceholder.Name = "panelPlaceholder";
            this.panelPlaceholder.Padding = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.panelPlaceholder.Radius = 12;
            this.panelPlaceholder.Shadow = 4;
            this.panelPlaceholder.Size = new System.Drawing.Size(834, 10);
            this.panelPlaceholder.TabIndex = 1;
            // 
            // panelToolbar
            // 
            this.panelToolbar.BackColor = System.Drawing.Color.Transparent;
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
            this.flowToolbarRight.Controls.Add(this.buttonAddAxis);
            this.flowToolbarRight.Controls.Add(this.buttonRefresh);
            this.flowToolbarRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowToolbarRight.Gap = 8;
            this.flowToolbarRight.Location = new System.Drawing.Point(594, 4);
            this.flowToolbarRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowToolbarRight.Name = "flowToolbarRight";
            this.flowToolbarRight.Size = new System.Drawing.Size(236, 36);
            this.flowToolbarRight.TabIndex = 1;
            this.flowToolbarRight.Text = "flowToolbarRight";
            // 
            // buttonAddAxis
            // 
            this.buttonAddAxis.IconSvg = "PlusOutlined";
            this.buttonAddAxis.Location = new System.Drawing.Point(108, 0);
            this.buttonAddAxis.Margin = new System.Windows.Forms.Padding(0);
            this.buttonAddAxis.Name = "buttonAddAxis";
            this.buttonAddAxis.Radius = 8;
            this.buttonAddAxis.Size = new System.Drawing.Size(120, 36);
            this.buttonAddAxis.TabIndex = 0;
            this.buttonAddAxis.Text = "新增轴";
            this.buttonAddAxis.Type = AntdUI.TTypeMini.Primary;
            this.buttonAddAxis.WaveSize = 0;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.IconSvg = "ReloadOutlined";
            this.buttonRefresh.Location = new System.Drawing.Point(0, 0);
            this.buttonRefresh.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Radius = 8;
            this.buttonRefresh.Size = new System.Drawing.Size(100, 36);
            this.buttonRefresh.TabIndex = 1;
            this.buttonRefresh.Text = "刷新";
            this.buttonRefresh.WaveSize = 0;
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
            this.flowToolbarLeft.Text = "flowToolbarLeft";
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
            this.buttonSelectCard.IconSvg = "CreditCardOutlined";
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
            // MotionAxisManagementPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MotionAxisManagementPage";
            this.Size = new System.Drawing.Size(850, 680);
            this.panelRoot.ResumeLayout(false);
            this.panelCardsHost.ResumeLayout(false);
            this.panelPagination.ResumeLayout(false);
            this.panelToolbar.ResumeLayout(false);
            this.flowToolbarRight.ResumeLayout(false);
            this.flowToolbarLeft.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AntdUI.Panel panelRoot;
        private AntdUI.Panel panelToolbar;
        private AntdUI.FlowPanel flowToolbarLeft;
        private AntdUI.FlowPanel flowToolbarRight;
        private AntdUI.Button buttonSelectCard;
        private AntdUI.Label labelSelectedCard;
        private AntdUI.Button buttonRefresh;
        private AntdUI.Button buttonAddAxis;
        private AntdUI.Panel panelPlaceholder;
        private AntdUI.Panel panelCardsHost;
        private AntdUI.Label labelCardsTitle;
        private AntdUI.FlowPanel flowCards;
        private AntdUI.Panel panelPagination;
        private AntdUI.Label labelPageSummary;
        private AntdUI.Pagination paginationCards;
    }
}