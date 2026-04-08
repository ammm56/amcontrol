namespace AMControlWinF.Views.Motion
{
    partial class MotionAxisPage
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
            this.paginationActions = new AntdUI.Pagination();
            this.labelPageSummary = new AntdUI.Label();
            this.panelDetailCard = new AntdUI.Panel();
            this.motionAxisDetailControl = new AMControlWinF.Views.Motion.MotionAxisDetailControl();
            this.panelListCard = new AntdUI.Panel();
            this.motionAxisVirtualListControl = new AMControlWinF.Views.Motion.MotionAxisVirtualListControl();
            this.panelSummaryBlank = new AntdUI.Panel();
            this.panelToolbar = new AntdUI.Panel();
            this.flowToolbarRight = new AntdUI.FlowPanel();
            this.inputSearch = new AntdUI.Input();
            this.flowToolbarLeft = new AntdUI.FlowPanel();
            this.buttonSelectAxis = new AntdUI.Button();
            this.labelSelectedAxis = new AntdUI.Label();
            this.panelRoot.SuspendLayout();
            this.panelContentCard.SuspendLayout();
            this.gridContent.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.panelDetailCard.SuspendLayout();
            this.panelListCard.SuspendLayout();
            this.panelToolbar.SuspendLayout();
            this.flowToolbarRight.SuspendLayout();
            this.flowToolbarLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.panelContentCard);
            this.panelRoot.Controls.Add(this.panelSummaryBlank);
            this.panelRoot.Controls.Add(this.panelToolbar);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Margin = new System.Windows.Forms.Padding(0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(8);
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(980, 680);
            this.panelRoot.TabIndex = 0;
            // 
            // panelContentCard
            // 
            this.panelContentCard.BackColor = System.Drawing.Color.Transparent;
            this.panelContentCard.Controls.Add(this.gridContent);
            this.panelContentCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContentCard.Location = new System.Drawing.Point(8, 96);
            this.panelContentCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelContentCard.Name = "panelContentCard";
            this.panelContentCard.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.panelContentCard.Radius = 12;
            this.panelContentCard.Shadow = 4;
            this.panelContentCard.ShadowOpacity = 0F;
            this.panelContentCard.ShadowOpacityHover = 0F;
            this.panelContentCard.Size = new System.Drawing.Size(964, 576);
            this.panelContentCard.TabIndex = 2;
            // 
            // gridContent
            // 
            this.gridContent.Controls.Add(this.panelFooter);
            this.gridContent.Controls.Add(this.panelDetailCard);
            this.gridContent.Controls.Add(this.panelListCard);
            this.gridContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridContent.Location = new System.Drawing.Point(12, 4);
            this.gridContent.Margin = new System.Windows.Forms.Padding(0);
            this.gridContent.Name = "gridContent";
            this.gridContent.Size = new System.Drawing.Size(940, 568);
            this.gridContent.Span = "100% 250;100%-100% 52";
            this.gridContent.TabIndex = 0;
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.paginationActions);
            this.panelFooter.Controls.Add(this.labelPageSummary);
            this.panelFooter.Location = new System.Drawing.Point(0, 516);
            this.panelFooter.Margin = new System.Windows.Forms.Padding(0);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.panelFooter.Radius = 0;
            this.panelFooter.Size = new System.Drawing.Size(940, 52);
            this.panelFooter.TabIndex = 2;
            // 
            // paginationActions
            // 
            this.paginationActions.Dock = System.Windows.Forms.DockStyle.Right;
            this.paginationActions.Location = new System.Drawing.Point(379, 8);
            this.paginationActions.Margin = new System.Windows.Forms.Padding(0);
            this.paginationActions.Name = "paginationActions";
            this.paginationActions.PageSize = 12;
            this.paginationActions.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.paginationActions.ShowSizeChanger = true;
            this.paginationActions.Size = new System.Drawing.Size(561, 44);
            this.paginationActions.SizeChangerWidth = 72;
            this.paginationActions.TabIndex = 1;
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
            this.panelDetailCard.Controls.Add(this.motionAxisDetailControl);
            this.panelDetailCard.Location = new System.Drawing.Point(690, 0);
            this.panelDetailCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelDetailCard.Name = "panelDetailCard";
            this.panelDetailCard.Padding = new System.Windows.Forms.Padding(8);
            this.panelDetailCard.Radius = 12;
            this.panelDetailCard.Shadow = 4;
            this.panelDetailCard.Size = new System.Drawing.Size(250, 516);
            this.panelDetailCard.TabIndex = 1;
            // 
            // motionAxisDetailControl
            // 
            this.motionAxisDetailControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.motionAxisDetailControl.Location = new System.Drawing.Point(12, 12);
            this.motionAxisDetailControl.Margin = new System.Windows.Forms.Padding(0);
            this.motionAxisDetailControl.Name = "motionAxisDetailControl";
            this.motionAxisDetailControl.Size = new System.Drawing.Size(226, 492);
            this.motionAxisDetailControl.TabIndex = 0;
            // 
            // panelListCard
            // 
            this.panelListCard.BackColor = System.Drawing.Color.Transparent;
            this.panelListCard.Controls.Add(this.motionAxisVirtualListControl);
            this.panelListCard.Location = new System.Drawing.Point(0, 0);
            this.panelListCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelListCard.Name = "panelListCard";
            this.panelListCard.Radius = 0;
            this.panelListCard.ShadowOpacity = 0F;
            this.panelListCard.ShadowOpacityHover = 0F;
            this.panelListCard.Size = new System.Drawing.Size(690, 516);
            this.panelListCard.TabIndex = 0;
            // 
            // motionAxisVirtualListControl
            // 
            this.motionAxisVirtualListControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.motionAxisVirtualListControl.Location = new System.Drawing.Point(0, 0);
            this.motionAxisVirtualListControl.Margin = new System.Windows.Forms.Padding(0);
            this.motionAxisVirtualListControl.Name = "motionAxisVirtualListControl";
            this.motionAxisVirtualListControl.Size = new System.Drawing.Size(690, 516);
            this.motionAxisVirtualListControl.TabIndex = 0;
            // 
            // panelSummaryBlank
            // 
            this.panelSummaryBlank.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSummaryBlank.Location = new System.Drawing.Point(8, 52);
            this.panelSummaryBlank.Margin = new System.Windows.Forms.Padding(0);
            this.panelSummaryBlank.Name = "panelSummaryBlank";
            this.panelSummaryBlank.Padding = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.panelSummaryBlank.Radius = 0;
            this.panelSummaryBlank.Size = new System.Drawing.Size(964, 44);
            this.panelSummaryBlank.TabIndex = 1;
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
            this.panelToolbar.Size = new System.Drawing.Size(964, 44);
            this.panelToolbar.TabIndex = 0;
            // 
            // flowToolbarRight
            // 
            this.flowToolbarRight.Controls.Add(this.inputSearch);
            this.flowToolbarRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowToolbarRight.Gap = 8;
            this.flowToolbarRight.Location = new System.Drawing.Point(724, 4);
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
            this.inputSearch.PlaceholderText = "搜索动作名称 / 分组 / 参数";
            this.inputSearch.Size = new System.Drawing.Size(236, 36);
            this.inputSearch.TabIndex = 0;
            this.inputSearch.WaveSize = 0;
            // 
            // flowToolbarLeft
            // 
            this.flowToolbarLeft.Controls.Add(this.buttonSelectAxis);
            this.flowToolbarLeft.Controls.Add(this.labelSelectedAxis);
            this.flowToolbarLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowToolbarLeft.Gap = 8;
            this.flowToolbarLeft.Location = new System.Drawing.Point(4, 4);
            this.flowToolbarLeft.Margin = new System.Windows.Forms.Padding(0);
            this.flowToolbarLeft.Name = "flowToolbarLeft";
            this.flowToolbarLeft.Size = new System.Drawing.Size(520, 36);
            this.flowToolbarLeft.TabIndex = 0;
            // 
            // buttonSelectAxis
            // 
            this.buttonSelectAxis.IconSvg = "AimOutlined";
            this.buttonSelectAxis.Location = new System.Drawing.Point(388, 0);
            this.buttonSelectAxis.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSelectAxis.Name = "buttonSelectAxis";
            this.buttonSelectAxis.Radius = 8;
            this.buttonSelectAxis.Size = new System.Drawing.Size(132, 36);
            this.buttonSelectAxis.TabIndex = 0;
            this.buttonSelectAxis.Text = "选择轴";
            this.buttonSelectAxis.Type = AntdUI.TTypeMini.Primary;
            this.buttonSelectAxis.WaveSize = 0;
            // 
            // labelSelectedAxis
            // 
            this.labelSelectedAxis.Location = new System.Drawing.Point(0, 0);
            this.labelSelectedAxis.Margin = new System.Windows.Forms.Padding(0);
            this.labelSelectedAxis.Name = "labelSelectedAxis";
            this.labelSelectedAxis.Size = new System.Drawing.Size(380, 36);
            this.labelSelectedAxis.TabIndex = 1;
            this.labelSelectedAxis.Text = "当前：未选择轴";
            // 
            // MotionAxisPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MotionAxisPage";
            this.Size = new System.Drawing.Size(980, 680);
            this.panelRoot.ResumeLayout(false);
            this.panelContentCard.ResumeLayout(false);
            this.gridContent.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            this.panelDetailCard.ResumeLayout(false);
            this.panelListCard.ResumeLayout(false);
            this.panelToolbar.ResumeLayout(false);
            this.flowToolbarRight.ResumeLayout(false);
            this.flowToolbarLeft.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AntdUI.Panel panelRoot;
        private AntdUI.Panel panelToolbar;
        private AntdUI.FlowPanel flowToolbarLeft;
        private AntdUI.Button buttonSelectAxis;
        private AntdUI.Label labelSelectedAxis;
        private AntdUI.FlowPanel flowToolbarRight;
        private AntdUI.Input inputSearch;
        private AntdUI.Panel panelSummaryBlank;
        private AntdUI.Panel panelContentCard;
        private AntdUI.GridPanel gridContent;
        private AntdUI.Panel panelListCard;
        private AntdUI.Panel panelDetailCard;
        private MotionAxisVirtualListControl motionAxisVirtualListControl;
        private MotionAxisDetailControl motionAxisDetailControl;
        private AntdUI.Panel panelFooter;
        private AntdUI.Label labelPageSummary;
        private AntdUI.Pagination paginationActions;
    }
}