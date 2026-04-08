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
            this.panelDetailCard = new AntdUI.Panel();
            this.motionAxisDetailControl = new AMControlWinF.Views.Motion.MotionAxisDetailControl();
            this.panelListCard = new AntdUI.Panel();
            this.motionAxisVirtualListControl = new AMControlWinF.Views.Motion.MotionAxisVirtualListControl();
            this.panelBlank = new AntdUI.Panel();
            this.panelToolbar = new AntdUI.Panel();
            this.flowToolbarRight = new AntdUI.FlowPanel();
            this.inputSearch = new AntdUI.Input();
            this.flowToolbarLeft = new AntdUI.FlowPanel();
            this.buttonSelectAxis = new AntdUI.Button();
            this.labelSelectedAxis = new AntdUI.Label();
            this.panelRoot.SuspendLayout();
            this.panelContentCard.SuspendLayout();
            this.gridContent.SuspendLayout();
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
            this.panelRoot.Controls.Add(this.panelBlank);
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
            this.panelContentCard.Location = new System.Drawing.Point(8, 56);
            this.panelContentCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelContentCard.Name = "panelContentCard";
            this.panelContentCard.Padding = new System.Windows.Forms.Padding(0);
            this.panelContentCard.Radius = 0;
            this.panelContentCard.ShadowOpacity = 0F;
            this.panelContentCard.ShadowOpacityHover = 0F;
            this.panelContentCard.Size = new System.Drawing.Size(964, 616);
            this.panelContentCard.TabIndex = 2;
            // 
            // gridContent
            // 
            this.gridContent.Controls.Add(this.panelDetailCard);
            this.gridContent.Controls.Add(this.panelListCard);
            this.gridContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridContent.Location = new System.Drawing.Point(0, 0);
            this.gridContent.Margin = new System.Windows.Forms.Padding(0);
            this.gridContent.Name = "gridContent";
            this.gridContent.Size = new System.Drawing.Size(964, 616);
            this.gridContent.Span = "100% 280";
            this.gridContent.TabIndex = 0;
            // 
            // panelDetailCard
            // 
            this.panelDetailCard.BackColor = System.Drawing.Color.Transparent;
            this.panelDetailCard.Controls.Add(this.motionAxisDetailControl);
            this.panelDetailCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetailCard.Location = new System.Drawing.Point(684, 0);
            this.panelDetailCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelDetailCard.Name = "panelDetailCard";
            this.panelDetailCard.Padding = new System.Windows.Forms.Padding(0);
            this.panelDetailCard.Radius = 12;
            this.panelDetailCard.Shadow = 4;
            this.panelDetailCard.Size = new System.Drawing.Size(280, 616);
            this.panelDetailCard.TabIndex = 1;
            // 
            // motionAxisDetailControl
            // 
            this.motionAxisDetailControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.motionAxisDetailControl.Location = new System.Drawing.Point(4, 4);
            this.motionAxisDetailControl.Margin = new System.Windows.Forms.Padding(0);
            this.motionAxisDetailControl.Name = "motionAxisDetailControl";
            this.motionAxisDetailControl.Size = new System.Drawing.Size(272, 608);
            this.motionAxisDetailControl.TabIndex = 0;
            // 
            // panelListCard
            // 
            this.panelListCard.BackColor = System.Drawing.Color.Transparent;
            this.panelListCard.Controls.Add(this.motionAxisVirtualListControl);
            this.panelListCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelListCard.Location = new System.Drawing.Point(0, 0);
            this.panelListCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelListCard.Name = "panelListCard";
            this.panelListCard.Radius = 0;
            this.panelListCard.ShadowOpacity = 0F;
            this.panelListCard.ShadowOpacityHover = 0F;
            this.panelListCard.Size = new System.Drawing.Size(684, 616);
            this.panelListCard.TabIndex = 0;
            // 
            // motionAxisVirtualListControl
            // 
            this.motionAxisVirtualListControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.motionAxisVirtualListControl.Location = new System.Drawing.Point(0, 0);
            this.motionAxisVirtualListControl.Margin = new System.Windows.Forms.Padding(0);
            this.motionAxisVirtualListControl.Name = "motionAxisVirtualListControl";
            this.motionAxisVirtualListControl.Size = new System.Drawing.Size(684, 616);
            this.motionAxisVirtualListControl.TabIndex = 0;
            // 
            // panelBlank
            // 
            this.panelBlank.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelBlank.Location = new System.Drawing.Point(8, 52);
            this.panelBlank.Margin = new System.Windows.Forms.Padding(0);
            this.panelBlank.Name = "panelBlank";
            this.panelBlank.Radius = 0;
            this.panelBlank.Size = new System.Drawing.Size(964, 4);
            this.panelBlank.TabIndex = 1;
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
            this.inputSearch.PlaceholderText = "搜索动作名称 / 分类 / 联动原因";
            this.inputSearch.Size = new System.Drawing.Size(236, 36);
            this.inputSearch.TabIndex = 0;
            this.inputSearch.WaveSize = 0;
            // 
            // flowToolbarLeft
            // 
            this.flowToolbarLeft.Controls.Add(this.labelSelectedAxis);
            this.flowToolbarLeft.Controls.Add(this.buttonSelectAxis);
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
            this.buttonSelectAxis.Location = new System.Drawing.Point(0, 0);
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
            this.labelSelectedAxis.Location = new System.Drawing.Point(140, 0);
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
        private AntdUI.Panel panelBlank;
        private AntdUI.Panel panelContentCard;
        private AntdUI.GridPanel gridContent;
        private AntdUI.Panel panelListCard;
        private AntdUI.Panel panelDetailCard;
        private MotionAxisVirtualListControl motionAxisVirtualListControl;
        private MotionAxisDetailControl motionAxisDetailControl;
    }
}