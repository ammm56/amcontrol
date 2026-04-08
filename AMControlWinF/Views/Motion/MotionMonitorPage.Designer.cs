namespace AMControlWinF.Views.Motion
{
    partial class MotionMonitorPage
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
            this.paginationAxes = new AntdUI.Pagination();
            this.labelPageSummary = new AntdUI.Label();
            this.panelDetailCard = new AntdUI.Panel();
            this.motionMonitorDetailControl = new AMControlWinF.Views.Motion.MotionMonitorDetailControl();
            this.panelListCard = new AntdUI.Panel();
            this.motionMonitorVirtualListControl = new AMControlWinF.Views.Motion.MotionMonitorVirtualListControl();
            this.flowStats = new AntdUI.FlowPanel();
            this.panelScanCard = new AntdUI.Panel();
            this.labelScanValue = new AntdUI.Label();
            this.labelScanTitle = new AntdUI.Label();
            this.panelReadyCard = new AntdUI.Panel();
            this.labelReadyCount = new AntdUI.Label();
            this.labelReadyTitle = new AntdUI.Label();
            this.panelMovingCard = new AntdUI.Panel();
            this.labelMovingCount = new AntdUI.Label();
            this.labelMovingTitle = new AntdUI.Label();
            this.panelAlarmCard = new AntdUI.Panel();
            this.labelAlarmCount = new AntdUI.Label();
            this.labelAlarmTitle = new AntdUI.Label();
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
            this.panelReadyCard.SuspendLayout();
            this.panelMovingCard.SuspendLayout();
            this.panelAlarmCard.SuspendLayout();
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
            this.panelRoot.Size = new System.Drawing.Size(980, 680);
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
            this.panelContentCard.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.panelContentCard.Radius = 12;
            this.panelContentCard.Shadow = 4;
            this.panelContentCard.ShadowOpacity = 0F;
            this.panelContentCard.ShadowOpacityHover = 0F;
            this.panelContentCard.Size = new System.Drawing.Size(964, 532);
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
            this.gridContent.Size = new System.Drawing.Size(940, 524);
            this.gridContent.Span = "100% 250";
            this.gridContent.TabIndex = 0;
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.paginationAxes);
            this.panelFooter.Controls.Add(this.labelPageSummary);
            this.panelFooter.Location = new System.Drawing.Point(0, 480);
            this.panelFooter.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Radius = 0;
            this.panelFooter.Size = new System.Drawing.Size(940, 44);
            this.panelFooter.TabIndex = 2;
            // 
            // paginationAxes
            // 
            this.paginationAxes.Dock = System.Windows.Forms.DockStyle.Right;
            this.paginationAxes.Location = new System.Drawing.Point(379, 0);
            this.paginationAxes.Margin = new System.Windows.Forms.Padding(0);
            this.paginationAxes.Name = "paginationAxes";
            this.paginationAxes.PageSize = 24;
            this.paginationAxes.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.paginationAxes.ShowSizeChanger = true;
            this.paginationAxes.Size = new System.Drawing.Size(561, 44);
            this.paginationAxes.SizeChangerWidth = 72;
            this.paginationAxes.TabIndex = 1;
            // 
            // labelPageSummary
            // 
            this.labelPageSummary.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelPageSummary.Location = new System.Drawing.Point(0, 0);
            this.labelPageSummary.Margin = new System.Windows.Forms.Padding(0);
            this.labelPageSummary.Name = "labelPageSummary";
            this.labelPageSummary.Size = new System.Drawing.Size(260, 44);
            this.labelPageSummary.TabIndex = 0;
            this.labelPageSummary.Text = "共 0 项";
            // 
            // panelDetailCard
            // 
            this.panelDetailCard.BackColor = System.Drawing.Color.Transparent;
            this.panelDetailCard.Controls.Add(this.motionMonitorDetailControl);
            this.panelDetailCard.Location = new System.Drawing.Point(690, 0);
            this.panelDetailCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelDetailCard.Name = "panelDetailCard";
            this.panelDetailCard.Padding = new System.Windows.Forms.Padding(4);
            this.panelDetailCard.Radius = 12;
            this.panelDetailCard.Shadow = 4;
            this.panelDetailCard.Size = new System.Drawing.Size(250, 472);
            this.panelDetailCard.TabIndex = 1;
            // 
            // motionMonitorDetailControl
            // 
            this.motionMonitorDetailControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.motionMonitorDetailControl.Location = new System.Drawing.Point(8, 8);
            this.motionMonitorDetailControl.Margin = new System.Windows.Forms.Padding(0);
            this.motionMonitorDetailControl.Name = "motionMonitorDetailControl";
            this.motionMonitorDetailControl.Size = new System.Drawing.Size(234, 456);
            this.motionMonitorDetailControl.TabIndex = 0;
            // 
            // panelListCard
            // 
            this.panelListCard.BackColor = System.Drawing.Color.Transparent;
            this.panelListCard.Controls.Add(this.motionMonitorVirtualListControl);
            this.panelListCard.Location = new System.Drawing.Point(0, 0);
            this.panelListCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelListCard.Name = "panelListCard";
            this.panelListCard.Radius = 0;
            this.panelListCard.ShadowOpacity = 0F;
            this.panelListCard.ShadowOpacityHover = 0F;
            this.panelListCard.Size = new System.Drawing.Size(690, 472);
            this.panelListCard.TabIndex = 0;
            // 
            // motionMonitorVirtualListControl
            // 
            this.motionMonitorVirtualListControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.motionMonitorVirtualListControl.Location = new System.Drawing.Point(0, 0);
            this.motionMonitorVirtualListControl.Margin = new System.Windows.Forms.Padding(0);
            this.motionMonitorVirtualListControl.Name = "motionMonitorVirtualListControl";
            this.motionMonitorVirtualListControl.Size = new System.Drawing.Size(690, 472);
            this.motionMonitorVirtualListControl.TabIndex = 0;
            // 
            // flowStats
            // 
            this.flowStats.Controls.Add(this.panelScanCard);
            this.flowStats.Controls.Add(this.panelReadyCard);
            this.flowStats.Controls.Add(this.panelMovingCard);
            this.flowStats.Controls.Add(this.panelAlarmCard);
            this.flowStats.Controls.Add(this.panelTotalCard);
            this.flowStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowStats.Gap = 8;
            this.flowStats.Location = new System.Drawing.Point(8, 52);
            this.flowStats.Margin = new System.Windows.Forms.Padding(0);
            this.flowStats.Name = "flowStats";
            this.flowStats.Padding = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.flowStats.Size = new System.Drawing.Size(964, 88);
            this.flowStats.TabIndex = 1;
            // 
            // panelScanCard
            // 
            this.panelScanCard.BackColor = System.Drawing.Color.Transparent;
            this.panelScanCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(229)))), ((int)(((byte)(235)))));
            this.panelScanCard.Controls.Add(this.labelScanValue);
            this.panelScanCard.Controls.Add(this.labelScanTitle);
            this.panelScanCard.Location = new System.Drawing.Point(636, 6);
            this.panelScanCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelScanCard.Name = "panelScanCard";
            this.panelScanCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelScanCard.Radius = 12;
            this.panelScanCard.Shadow = 4;
            this.panelScanCard.ShadowOpacity = 0.2F;
            this.panelScanCard.ShadowOpacityAnimation = true;
            this.panelScanCard.Size = new System.Drawing.Size(150, 72);
            this.panelScanCard.TabIndex = 4;
            // 
            // labelScanValue
            // 
            this.labelScanValue.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelScanValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.labelScanValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(119)))), ((int)(((byte)(255)))));
            this.labelScanValue.Location = new System.Drawing.Point(73, 16);
            this.labelScanValue.Name = "labelScanValue";
            this.labelScanValue.Size = new System.Drawing.Size(61, 40);
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
            // panelReadyCard
            // 
            this.panelReadyCard.BackColor = System.Drawing.Color.Transparent;
            this.panelReadyCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(229)))), ((int)(((byte)(235)))));
            this.panelReadyCard.Controls.Add(this.labelReadyCount);
            this.panelReadyCard.Controls.Add(this.labelReadyTitle);
            this.panelReadyCard.Location = new System.Drawing.Point(478, 6);
            this.panelReadyCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelReadyCard.Name = "panelReadyCard";
            this.panelReadyCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelReadyCard.Radius = 12;
            this.panelReadyCard.Shadow = 4;
            this.panelReadyCard.ShadowOpacity = 0.2F;
            this.panelReadyCard.ShadowOpacityAnimation = true;
            this.panelReadyCard.Size = new System.Drawing.Size(150, 72);
            this.panelReadyCard.TabIndex = 3;
            // 
            // labelReadyCount
            // 
            this.labelReadyCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelReadyCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelReadyCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(196)))), ((int)(((byte)(26)))));
            this.labelReadyCount.Location = new System.Drawing.Point(94, 16);
            this.labelReadyCount.Name = "labelReadyCount";
            this.labelReadyCount.Size = new System.Drawing.Size(40, 40);
            this.labelReadyCount.TabIndex = 1;
            this.labelReadyCount.Text = "0";
            // 
            // labelReadyTitle
            // 
            this.labelReadyTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelReadyTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelReadyTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(196)))), ((int)(((byte)(26)))));
            this.labelReadyTitle.Location = new System.Drawing.Point(16, 16);
            this.labelReadyTitle.Name = "labelReadyTitle";
            this.labelReadyTitle.Size = new System.Drawing.Size(72, 40);
            this.labelReadyTitle.TabIndex = 0;
            this.labelReadyTitle.Text = "就绪轴";
            // 
            // panelMovingCard
            // 
            this.panelMovingCard.BackColor = System.Drawing.Color.Transparent;
            this.panelMovingCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(229)))), ((int)(((byte)(235)))));
            this.panelMovingCard.Controls.Add(this.labelMovingCount);
            this.panelMovingCard.Controls.Add(this.labelMovingTitle);
            this.panelMovingCard.Location = new System.Drawing.Point(320, 6);
            this.panelMovingCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelMovingCard.Name = "panelMovingCard";
            this.panelMovingCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelMovingCard.Radius = 12;
            this.panelMovingCard.Shadow = 4;
            this.panelMovingCard.ShadowOpacity = 0.2F;
            this.panelMovingCard.ShadowOpacityAnimation = true;
            this.panelMovingCard.Size = new System.Drawing.Size(150, 72);
            this.panelMovingCard.TabIndex = 2;
            // 
            // labelMovingCount
            // 
            this.labelMovingCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelMovingCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelMovingCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(119)))), ((int)(((byte)(255)))));
            this.labelMovingCount.Location = new System.Drawing.Point(94, 16);
            this.labelMovingCount.Name = "labelMovingCount";
            this.labelMovingCount.Size = new System.Drawing.Size(40, 40);
            this.labelMovingCount.TabIndex = 1;
            this.labelMovingCount.Text = "0";
            // 
            // labelMovingTitle
            // 
            this.labelMovingTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelMovingTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelMovingTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(119)))), ((int)(((byte)(255)))));
            this.labelMovingTitle.Location = new System.Drawing.Point(16, 16);
            this.labelMovingTitle.Name = "labelMovingTitle";
            this.labelMovingTitle.Size = new System.Drawing.Size(72, 40);
            this.labelMovingTitle.TabIndex = 0;
            this.labelMovingTitle.Text = "运动中";
            // 
            // panelAlarmCard
            // 
            this.panelAlarmCard.BackColor = System.Drawing.Color.Transparent;
            this.panelAlarmCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(229)))), ((int)(((byte)(235)))));
            this.panelAlarmCard.Controls.Add(this.labelAlarmCount);
            this.panelAlarmCard.Controls.Add(this.labelAlarmTitle);
            this.panelAlarmCard.Location = new System.Drawing.Point(162, 6);
            this.panelAlarmCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelAlarmCard.Name = "panelAlarmCard";
            this.panelAlarmCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelAlarmCard.Radius = 12;
            this.panelAlarmCard.Shadow = 4;
            this.panelAlarmCard.ShadowOpacity = 0.2F;
            this.panelAlarmCard.ShadowOpacityAnimation = true;
            this.panelAlarmCard.Size = new System.Drawing.Size(150, 72);
            this.panelAlarmCard.TabIndex = 1;
            // 
            // labelAlarmCount
            // 
            this.labelAlarmCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelAlarmCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelAlarmCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.labelAlarmCount.Location = new System.Drawing.Point(94, 16);
            this.labelAlarmCount.Name = "labelAlarmCount";
            this.labelAlarmCount.Size = new System.Drawing.Size(40, 40);
            this.labelAlarmCount.TabIndex = 1;
            this.labelAlarmCount.Text = "0";
            // 
            // labelAlarmTitle
            // 
            this.labelAlarmTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelAlarmTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelAlarmTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.labelAlarmTitle.Location = new System.Drawing.Point(16, 16);
            this.labelAlarmTitle.Name = "labelAlarmTitle";
            this.labelAlarmTitle.Size = new System.Drawing.Size(72, 40);
            this.labelAlarmTitle.TabIndex = 0;
            this.labelAlarmTitle.Text = "报警轴";
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
            this.panelTotalCard.Size = new System.Drawing.Size(150, 72);
            this.panelTotalCard.TabIndex = 0;
            // 
            // labelTotalCount
            // 
            this.labelTotalCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelTotalCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelTotalCount.Location = new System.Drawing.Point(94, 16);
            this.labelTotalCount.Name = "labelTotalCount";
            this.labelTotalCount.Size = new System.Drawing.Size(40, 40);
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
            this.labelTotalTitle.Text = "当前卡轴数";
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
            this.inputSearch.PlaceholderText = "搜索轴名称 / 逻辑轴 / 类型";
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
            this.flowToolbarLeft.Size = new System.Drawing.Size(520, 36);
            this.flowToolbarLeft.TabIndex = 0;
            // 
            // labelSelectedCard
            // 
            this.labelSelectedCard.Location = new System.Drawing.Point(140, 0);
            this.labelSelectedCard.Margin = new System.Windows.Forms.Padding(0);
            this.labelSelectedCard.Name = "labelSelectedCard";
            this.labelSelectedCard.Size = new System.Drawing.Size(380, 36);
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
            // MotionMonitorPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MotionMonitorPage";
            this.Size = new System.Drawing.Size(980, 680);
            this.panelRoot.ResumeLayout(false);
            this.panelContentCard.ResumeLayout(false);
            this.gridContent.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            this.panelDetailCard.ResumeLayout(false);
            this.panelListCard.ResumeLayout(false);
            this.flowStats.ResumeLayout(false);
            this.panelScanCard.ResumeLayout(false);
            this.panelReadyCard.ResumeLayout(false);
            this.panelMovingCard.ResumeLayout(false);
            this.panelAlarmCard.ResumeLayout(false);
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
        private AntdUI.Panel panelAlarmCard;
        private AntdUI.Label labelAlarmCount;
        private AntdUI.Label labelAlarmTitle;
        private AntdUI.Panel panelMovingCard;
        private AntdUI.Label labelMovingCount;
        private AntdUI.Label labelMovingTitle;
        private AntdUI.Panel panelReadyCard;
        private AntdUI.Label labelReadyCount;
        private AntdUI.Label labelReadyTitle;
        private AntdUI.Panel panelScanCard;
        private AntdUI.Label labelScanValue;
        private AntdUI.Label labelScanTitle;
        private AntdUI.Panel panelContentCard;
        private AntdUI.GridPanel gridContent;
        private AntdUI.Panel panelListCard;
        private AntdUI.Panel panelDetailCard;
        private MotionMonitorVirtualListControl motionMonitorVirtualListControl;
        private MotionMonitorDetailControl motionMonitorDetailControl;
        private AntdUI.Panel panelFooter;
        private AntdUI.Label labelPageSummary;
        private AntdUI.Pagination paginationAxes;
    }
}