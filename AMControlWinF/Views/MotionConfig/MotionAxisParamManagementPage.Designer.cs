namespace AMControlWinF.Views.MotionConfig
{
    partial class MotionAxisParamManagementPage
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
            this.labelCardsTitle = new AntdUI.Label();
            this.panelGroupFilters = new AntdUI.Panel();
            this.flowGroups = new AntdUI.FlowPanel();
            this.buttonGroupSafety = new AntdUI.Button();
            this.buttonGroupTiming = new AntdUI.Button();
            this.buttonGroupSoftLimit = new AntdUI.Button();
            this.buttonGroupHome = new AntdUI.Button();
            this.buttonGroupMotion = new AntdUI.Button();
            this.buttonGroupScale = new AntdUI.Button();
            this.buttonGroupHardware = new AntdUI.Button();
            this.panelToolbar = new AntdUI.Panel();
            this.flowToolbarRight = new AntdUI.FlowPanel();
            this.buttonAddParam = new AntdUI.Button();
            this.buttonEditParam = new AntdUI.Button();
            this.buttonDeleteParam = new AntdUI.Button();
            this.flowToolbarLeft = new AntdUI.FlowPanel();
            this.labelSelectedAxis = new AntdUI.Label();
            this.buttonSelectAxis = new AntdUI.Button();
            this.panelRoot.SuspendLayout();
            this.panelCardsHost.SuspendLayout();
            this.panelGroupFilters.SuspendLayout();
            this.flowGroups.SuspendLayout();
            this.panelToolbar.SuspendLayout();
            this.flowToolbarRight.SuspendLayout();
            this.flowToolbarLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.panelCardsHost);
            this.panelRoot.Controls.Add(this.panelGroupFilters);
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
            this.panelCardsHost.Controls.Add(this.labelCardsTitle);
            this.panelCardsHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCardsHost.Location = new System.Drawing.Point(8, 122);
            this.panelCardsHost.Margin = new System.Windows.Forms.Padding(0);
            this.panelCardsHost.Name = "panelCardsHost";
            this.panelCardsHost.Padding = new System.Windows.Forms.Padding(8);
            this.panelCardsHost.Radius = 12;
            this.panelCardsHost.Shadow = 4;
            this.panelCardsHost.Size = new System.Drawing.Size(834, 550);
            this.panelCardsHost.TabIndex = 2;
            // 
            // flowCards
            // 
            this.flowCards.AutoScroll = true;
            this.flowCards.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowCards.Gap = 2;
            this.flowCards.Location = new System.Drawing.Point(12, 48);
            this.flowCards.Margin = new System.Windows.Forms.Padding(0);
            this.flowCards.Name = "flowCards";
            this.flowCards.Size = new System.Drawing.Size(810, 490);
            this.flowCards.TabIndex = 1;
            this.flowCards.Text = "flowCards";
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
            this.labelCardsTitle.Text = "参数卡片";
            this.labelCardsTitle.Visible = false;
            // 
            // panelGroupFilters
            // 
            this.panelGroupFilters.BackColor = System.Drawing.Color.Transparent;
            this.panelGroupFilters.Controls.Add(this.flowGroups);
            this.panelGroupFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelGroupFilters.Location = new System.Drawing.Point(8, 52);
            this.panelGroupFilters.Margin = new System.Windows.Forms.Padding(0);
            this.panelGroupFilters.Name = "panelGroupFilters";
            this.panelGroupFilters.Radius = 0;
            this.panelGroupFilters.Size = new System.Drawing.Size(834, 70);
            this.panelGroupFilters.TabIndex = 1;
            // 
            // flowGroups
            // 
            this.flowGroups.Controls.Add(this.buttonGroupSafety);
            this.flowGroups.Controls.Add(this.buttonGroupTiming);
            this.flowGroups.Controls.Add(this.buttonGroupSoftLimit);
            this.flowGroups.Controls.Add(this.buttonGroupHome);
            this.flowGroups.Controls.Add(this.buttonGroupMotion);
            this.flowGroups.Controls.Add(this.buttonGroupScale);
            this.flowGroups.Controls.Add(this.buttonGroupHardware);
            this.flowGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowGroups.Gap = 8;
            this.flowGroups.Location = new System.Drawing.Point(0, 0);
            this.flowGroups.Margin = new System.Windows.Forms.Padding(0);
            this.flowGroups.Name = "flowGroups";
            this.flowGroups.Padding = new System.Windows.Forms.Padding(4, 18, 4, 6);
            this.flowGroups.Size = new System.Drawing.Size(834, 70);
            this.flowGroups.TabIndex = 0;
            this.flowGroups.Text = "flowGroups";
            // 
            // buttonGroupSafety
            // 
            this.buttonGroupSafety.Location = new System.Drawing.Point(580, 18);
            this.buttonGroupSafety.Margin = new System.Windows.Forms.Padding(0);
            this.buttonGroupSafety.Name = "buttonGroupSafety";
            this.buttonGroupSafety.Radius = 8;
            this.buttonGroupSafety.Size = new System.Drawing.Size(88, 40);
            this.buttonGroupSafety.TabIndex = 6;
            this.buttonGroupSafety.Text = "安全联动";
            this.buttonGroupSafety.WaveSize = 0;
            // 
            // buttonGroupTiming
            // 
            this.buttonGroupTiming.Location = new System.Drawing.Point(484, 18);
            this.buttonGroupTiming.Margin = new System.Windows.Forms.Padding(0);
            this.buttonGroupTiming.Name = "buttonGroupTiming";
            this.buttonGroupTiming.Radius = 8;
            this.buttonGroupTiming.Size = new System.Drawing.Size(88, 40);
            this.buttonGroupTiming.TabIndex = 5;
            this.buttonGroupTiming.Text = "使能时序";
            this.buttonGroupTiming.WaveSize = 0;
            // 
            // buttonGroupSoftLimit
            // 
            this.buttonGroupSoftLimit.Location = new System.Drawing.Point(388, 18);
            this.buttonGroupSoftLimit.Margin = new System.Windows.Forms.Padding(0);
            this.buttonGroupSoftLimit.Name = "buttonGroupSoftLimit";
            this.buttonGroupSoftLimit.Radius = 8;
            this.buttonGroupSoftLimit.Size = new System.Drawing.Size(88, 40);
            this.buttonGroupSoftLimit.TabIndex = 4;
            this.buttonGroupSoftLimit.Text = "软件限位";
            this.buttonGroupSoftLimit.WaveSize = 0;
            // 
            // buttonGroupHome
            // 
            this.buttonGroupHome.Location = new System.Drawing.Point(292, 18);
            this.buttonGroupHome.Margin = new System.Windows.Forms.Padding(0);
            this.buttonGroupHome.Name = "buttonGroupHome";
            this.buttonGroupHome.Radius = 8;
            this.buttonGroupHome.Size = new System.Drawing.Size(88, 40);
            this.buttonGroupHome.TabIndex = 3;
            this.buttonGroupHome.Text = "回零参数";
            this.buttonGroupHome.WaveSize = 0;
            // 
            // buttonGroupMotion
            // 
            this.buttonGroupMotion.Location = new System.Drawing.Point(196, 18);
            this.buttonGroupMotion.Margin = new System.Windows.Forms.Padding(0);
            this.buttonGroupMotion.Name = "buttonGroupMotion";
            this.buttonGroupMotion.Radius = 8;
            this.buttonGroupMotion.Size = new System.Drawing.Size(88, 40);
            this.buttonGroupMotion.TabIndex = 2;
            this.buttonGroupMotion.Text = "运动参数";
            this.buttonGroupMotion.Type = AntdUI.TTypeMini.Primary;
            this.buttonGroupMotion.WaveSize = 0;
            // 
            // buttonGroupScale
            // 
            this.buttonGroupScale.Location = new System.Drawing.Point(100, 18);
            this.buttonGroupScale.Margin = new System.Windows.Forms.Padding(0);
            this.buttonGroupScale.Name = "buttonGroupScale";
            this.buttonGroupScale.Radius = 8;
            this.buttonGroupScale.Size = new System.Drawing.Size(88, 40);
            this.buttonGroupScale.TabIndex = 1;
            this.buttonGroupScale.Text = "单位换算";
            this.buttonGroupScale.WaveSize = 0;
            // 
            // buttonGroupHardware
            // 
            this.buttonGroupHardware.Location = new System.Drawing.Point(4, 18);
            this.buttonGroupHardware.Margin = new System.Windows.Forms.Padding(0);
            this.buttonGroupHardware.Name = "buttonGroupHardware";
            this.buttonGroupHardware.Radius = 8;
            this.buttonGroupHardware.Size = new System.Drawing.Size(88, 40);
            this.buttonGroupHardware.TabIndex = 0;
            this.buttonGroupHardware.Text = "硬件信号";
            this.buttonGroupHardware.WaveSize = 0;
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
            this.flowToolbarRight.Controls.Add(this.buttonAddParam);
            this.flowToolbarRight.Controls.Add(this.buttonEditParam);
            this.flowToolbarRight.Controls.Add(this.buttonDeleteParam);
            this.flowToolbarRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowToolbarRight.Gap = 8;
            this.flowToolbarRight.Location = new System.Drawing.Point(450, 4);
            this.flowToolbarRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowToolbarRight.Name = "flowToolbarRight";
            this.flowToolbarRight.Size = new System.Drawing.Size(380, 36);
            this.flowToolbarRight.TabIndex = 1;
            this.flowToolbarRight.Text = "flowToolbarRight";
            // 
            // buttonAddParam
            // 
            this.buttonAddParam.IconSvg = "PlusOutlined";
            this.buttonAddParam.Location = new System.Drawing.Point(256, 0);
            this.buttonAddParam.Margin = new System.Windows.Forms.Padding(0);
            this.buttonAddParam.Name = "buttonAddParam";
            this.buttonAddParam.Radius = 8;
            this.buttonAddParam.Size = new System.Drawing.Size(120, 36);
            this.buttonAddParam.TabIndex = 0;
            this.buttonAddParam.Text = "新增参数";
            this.buttonAddParam.Type = AntdUI.TTypeMini.Primary;
            this.buttonAddParam.WaveSize = 0;
            // 
            // buttonEditParam
            // 
            this.buttonEditParam.IconSvg = "EditOutlined";
            this.buttonEditParam.Location = new System.Drawing.Point(128, 0);
            this.buttonEditParam.Margin = new System.Windows.Forms.Padding(0);
            this.buttonEditParam.Name = "buttonEditParam";
            this.buttonEditParam.Radius = 8;
            this.buttonEditParam.Size = new System.Drawing.Size(120, 36);
            this.buttonEditParam.TabIndex = 1;
            this.buttonEditParam.Text = "编辑参数";
            this.buttonEditParam.WaveSize = 0;
            // 
            // buttonDeleteParam
            // 
            this.buttonDeleteParam.IconSvg = "DeleteOutlined";
            this.buttonDeleteParam.Location = new System.Drawing.Point(0, 0);
            this.buttonDeleteParam.Margin = new System.Windows.Forms.Padding(0);
            this.buttonDeleteParam.Name = "buttonDeleteParam";
            this.buttonDeleteParam.Radius = 8;
            this.buttonDeleteParam.Size = new System.Drawing.Size(120, 36);
            this.buttonDeleteParam.TabIndex = 2;
            this.buttonDeleteParam.Text = "删除参数";
            this.buttonDeleteParam.WaveSize = 0;
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
            this.flowToolbarLeft.Size = new System.Drawing.Size(420, 36);
            this.flowToolbarLeft.TabIndex = 0;
            this.flowToolbarLeft.Text = "flowToolbarLeft";
            // 
            // labelSelectedAxis
            // 
            this.labelSelectedAxis.Location = new System.Drawing.Point(140, 0);
            this.labelSelectedAxis.Margin = new System.Windows.Forms.Padding(0);
            this.labelSelectedAxis.Name = "labelSelectedAxis";
            this.labelSelectedAxis.Size = new System.Drawing.Size(280, 36);
            this.labelSelectedAxis.TabIndex = 1;
            this.labelSelectedAxis.Text = "当前：未选择轴";
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
            // MotionAxisParamManagementPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MotionAxisParamManagementPage";
            this.Size = new System.Drawing.Size(850, 680);
            this.panelRoot.ResumeLayout(false);
            this.panelCardsHost.ResumeLayout(false);
            this.panelGroupFilters.ResumeLayout(false);
            this.flowGroups.ResumeLayout(false);
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
        private AntdUI.Button buttonDeleteParam;
        private AntdUI.Button buttonEditParam;
        private AntdUI.Button buttonAddParam;
        private AntdUI.Panel panelGroupFilters;
        private AntdUI.FlowPanel flowGroups;
        private AntdUI.Button buttonGroupHardware;
        private AntdUI.Button buttonGroupScale;
        private AntdUI.Button buttonGroupMotion;
        private AntdUI.Button buttonGroupHome;
        private AntdUI.Button buttonGroupSoftLimit;
        private AntdUI.Button buttonGroupTiming;
        private AntdUI.Button buttonGroupSafety;
        private AntdUI.Panel panelCardsHost;
        private AntdUI.Label labelCardsTitle;
        private AntdUI.FlowPanel flowCards;
    }
}