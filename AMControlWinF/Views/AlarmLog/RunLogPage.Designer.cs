namespace AMControlWinF.Views.AlarmLog
{
    partial class RunLogPage
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

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.panelRoot = new AntdUI.Panel();
            this.panelTableCard = new AntdUI.Panel();
            this.tableRunLogs = new AntdUI.Table();
            this.panelTableFooter = new AntdUI.Panel();
            this.paginationRunLogs = new AntdUI.Pagination();
            this.flowStats = new AntdUI.FlowPanel();
            this.panelInfoCard = new AntdUI.Panel();
            this.labelInfoCount = new AntdUI.Label();
            this.labelInfoTitle = new AntdUI.Label();
            this.panelWarnCard = new AntdUI.Panel();
            this.labelWarnCount = new AntdUI.Label();
            this.labelWarnTitle = new AntdUI.Label();
            this.panelErrorCard = new AntdUI.Panel();
            this.labelErrorCount = new AntdUI.Label();
            this.labelErrorTitle = new AntdUI.Label();
            this.panelTotalCard = new AntdUI.Panel();
            this.labelTotalCount = new AntdUI.Label();
            this.labelTotalTitle = new AntdUI.Label();
            this.panelToolbar = new AntdUI.Panel();
            this.flowActionsLeft = new AntdUI.FlowPanel();
            this.buttonRefresh = new AntdUI.Button();
            this.buttonQuery = new AntdUI.Button();
            this.inputSearch = new AntdUI.Input();
            this.selectLogFile = new AntdUI.Select();
            this.flowActionsRight = new AntdUI.FlowPanel();
            this.buttonDebug = new AntdUI.Button();
            this.buttonInfo = new AntdUI.Button();
            this.buttonWarn = new AntdUI.Button();
            this.buttonError = new AntdUI.Button();
            this.buttonAll = new AntdUI.Button();
            this.panelRoot.SuspendLayout();
            this.panelTableCard.SuspendLayout();
            this.panelTableFooter.SuspendLayout();
            this.flowStats.SuspendLayout();
            this.panelInfoCard.SuspendLayout();
            this.panelWarnCard.SuspendLayout();
            this.panelErrorCard.SuspendLayout();
            this.panelTotalCard.SuspendLayout();
            this.panelToolbar.SuspendLayout();
            this.flowActionsLeft.SuspendLayout();
            this.flowActionsRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.panelTableCard);
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
            // panelTableCard
            // 
            this.panelTableCard.BackColor = System.Drawing.Color.Transparent;
            this.panelTableCard.Controls.Add(this.tableRunLogs);
            this.panelTableCard.Controls.Add(this.panelTableFooter);
            this.panelTableCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTableCard.Location = new System.Drawing.Point(8, 140);
            this.panelTableCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelTableCard.Name = "panelTableCard";
            this.panelTableCard.Padding = new System.Windows.Forms.Padding(8);
            this.panelTableCard.Radius = 12;
            this.panelTableCard.Shadow = 4;
            this.panelTableCard.ShadowOpacity = 0.15F;
            this.panelTableCard.Size = new System.Drawing.Size(834, 532);
            this.panelTableCard.TabIndex = 2;
            // 
            // tableRunLogs
            // 
            this.tableRunLogs.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tableRunLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableRunLogs.EmptyHeader = true;
            this.tableRunLogs.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.tableRunLogs.Gap = 8;
            this.tableRunLogs.Gaps = new System.Drawing.Size(8, 8);
            this.tableRunLogs.Location = new System.Drawing.Point(12, 12);
            this.tableRunLogs.Margin = new System.Windows.Forms.Padding(0);
            this.tableRunLogs.Name = "tableRunLogs";
            this.tableRunLogs.ShowTip = false;
            this.tableRunLogs.Size = new System.Drawing.Size(810, 458);
            this.tableRunLogs.TabIndex = 0;
            this.tableRunLogs.Text = "tableRunLogs";
            // 
            // panelTableFooter
            // 
            this.panelTableFooter.Controls.Add(this.paginationRunLogs);
            this.panelTableFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelTableFooter.Location = new System.Drawing.Point(12, 470);
            this.panelTableFooter.Margin = new System.Windows.Forms.Padding(0);
            this.panelTableFooter.Name = "panelTableFooter";
            this.panelTableFooter.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.panelTableFooter.Radius = 0;
            this.panelTableFooter.Size = new System.Drawing.Size(810, 50);
            this.panelTableFooter.TabIndex = 1;
            // 
            // paginationRunLogs
            // 
            this.paginationRunLogs.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.paginationRunLogs.Location = new System.Drawing.Point(0, 8);
            this.paginationRunLogs.Margin = new System.Windows.Forms.Padding(0);
            this.paginationRunLogs.Name = "paginationRunLogs";
            this.paginationRunLogs.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.paginationRunLogs.Size = new System.Drawing.Size(810, 42);
            this.paginationRunLogs.SizeChangerWidth = 72;
            this.paginationRunLogs.TabIndex = 0;
            this.paginationRunLogs.Text = "paginationRunLogs";
            // 
            // flowStats
            // 
            this.flowStats.Controls.Add(this.panelInfoCard);
            this.flowStats.Controls.Add(this.panelWarnCard);
            this.flowStats.Controls.Add(this.panelErrorCard);
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
            // panelInfoCard
            // 
            this.panelInfoCard.BackColor = System.Drawing.Color.Transparent;
            this.panelInfoCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(229)))), ((int)(((byte)(235)))));
            this.panelInfoCard.Controls.Add(this.labelInfoCount);
            this.panelInfoCard.Controls.Add(this.labelInfoTitle);
            this.panelInfoCard.Location = new System.Drawing.Point(568, 6);
            this.panelInfoCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelInfoCard.Name = "panelInfoCard";
            this.panelInfoCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelInfoCard.Radius = 12;
            this.panelInfoCard.Shadow = 4;
            this.panelInfoCard.ShadowOpacity = 0.2F;
            this.panelInfoCard.ShadowOpacityAnimation = true;
            this.panelInfoCard.Size = new System.Drawing.Size(180, 72);
            this.panelInfoCard.TabIndex = 3;
            // 
            // labelInfoCount
            // 
            this.labelInfoCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelInfoCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelInfoCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(196)))), ((int)(((byte)(26)))));
            this.labelInfoCount.Location = new System.Drawing.Point(104, 16);
            this.labelInfoCount.Name = "labelInfoCount";
            this.labelInfoCount.Size = new System.Drawing.Size(60, 40);
            this.labelInfoCount.TabIndex = 1;
            this.labelInfoCount.Text = "0";
            // 
            // labelInfoTitle
            // 
            this.labelInfoTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelInfoTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelInfoTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(196)))), ((int)(((byte)(26)))));
            this.labelInfoTitle.Location = new System.Drawing.Point(16, 16);
            this.labelInfoTitle.Name = "labelInfoTitle";
            this.labelInfoTitle.Size = new System.Drawing.Size(72, 40);
            this.labelInfoTitle.TabIndex = 0;
            this.labelInfoTitle.Text = "信息";
            // 
            // panelWarnCard
            // 
            this.panelWarnCard.BackColor = System.Drawing.Color.Transparent;
            this.panelWarnCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(229)))), ((int)(((byte)(235)))));
            this.panelWarnCard.Controls.Add(this.labelWarnCount);
            this.panelWarnCard.Controls.Add(this.labelWarnTitle);
            this.panelWarnCard.Location = new System.Drawing.Point(380, 6);
            this.panelWarnCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelWarnCard.Name = "panelWarnCard";
            this.panelWarnCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelWarnCard.Radius = 12;
            this.panelWarnCard.Shadow = 4;
            this.panelWarnCard.ShadowOpacity = 0.2F;
            this.panelWarnCard.ShadowOpacityAnimation = true;
            this.panelWarnCard.Size = new System.Drawing.Size(180, 72);
            this.panelWarnCard.TabIndex = 2;
            // 
            // labelWarnCount
            // 
            this.labelWarnCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelWarnCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelWarnCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(145)))), ((int)(((byte)(56)))));
            this.labelWarnCount.Location = new System.Drawing.Point(104, 16);
            this.labelWarnCount.Name = "labelWarnCount";
            this.labelWarnCount.Size = new System.Drawing.Size(60, 40);
            this.labelWarnCount.TabIndex = 1;
            this.labelWarnCount.Text = "0";
            // 
            // labelWarnTitle
            // 
            this.labelWarnTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelWarnTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelWarnTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(145)))), ((int)(((byte)(56)))));
            this.labelWarnTitle.Location = new System.Drawing.Point(16, 16);
            this.labelWarnTitle.Name = "labelWarnTitle";
            this.labelWarnTitle.Size = new System.Drawing.Size(72, 40);
            this.labelWarnTitle.TabIndex = 0;
            this.labelWarnTitle.Text = "警告";
            // 
            // panelErrorCard
            // 
            this.panelErrorCard.BackColor = System.Drawing.Color.Transparent;
            this.panelErrorCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(229)))), ((int)(((byte)(235)))));
            this.panelErrorCard.Controls.Add(this.labelErrorCount);
            this.panelErrorCard.Controls.Add(this.labelErrorTitle);
            this.panelErrorCard.Location = new System.Drawing.Point(192, 6);
            this.panelErrorCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelErrorCard.Name = "panelErrorCard";
            this.panelErrorCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelErrorCard.Radius = 12;
            this.panelErrorCard.Shadow = 4;
            this.panelErrorCard.ShadowOpacity = 0.2F;
            this.panelErrorCard.ShadowOpacityAnimation = true;
            this.panelErrorCard.Size = new System.Drawing.Size(180, 72);
            this.panelErrorCard.TabIndex = 1;
            // 
            // labelErrorCount
            // 
            this.labelErrorCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelErrorCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelErrorCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.labelErrorCount.Location = new System.Drawing.Point(104, 16);
            this.labelErrorCount.Name = "labelErrorCount";
            this.labelErrorCount.Size = new System.Drawing.Size(60, 40);
            this.labelErrorCount.TabIndex = 1;
            this.labelErrorCount.Text = "0";
            // 
            // labelErrorTitle
            // 
            this.labelErrorTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelErrorTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelErrorTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.labelErrorTitle.Location = new System.Drawing.Point(16, 16);
            this.labelErrorTitle.Name = "labelErrorTitle";
            this.labelErrorTitle.Size = new System.Drawing.Size(72, 40);
            this.labelErrorTitle.TabIndex = 0;
            this.labelErrorTitle.Text = "错误";
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
            this.labelTotalTitle.Text = "日志总数";
            // 
            // panelToolbar
            // 
            this.panelToolbar.Controls.Add(this.flowActionsLeft);
            this.panelToolbar.Controls.Add(this.flowActionsRight);
            this.panelToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelToolbar.Location = new System.Drawing.Point(8, 8);
            this.panelToolbar.Margin = new System.Windows.Forms.Padding(0);
            this.panelToolbar.Name = "panelToolbar";
            this.panelToolbar.Padding = new System.Windows.Forms.Padding(4);
            this.panelToolbar.Radius = 0;
            this.panelToolbar.Size = new System.Drawing.Size(834, 44);
            this.panelToolbar.TabIndex = 0;
            // 
            // flowActionsLeft
            // 
            this.flowActionsLeft.Controls.Add(this.buttonRefresh);
            this.flowActionsLeft.Controls.Add(this.buttonQuery);
            this.flowActionsLeft.Controls.Add(this.inputSearch);
            this.flowActionsLeft.Controls.Add(this.selectLogFile);
            this.flowActionsLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowActionsLeft.Gap = 8;
            this.flowActionsLeft.Location = new System.Drawing.Point(4, 4);
            this.flowActionsLeft.Name = "flowActionsLeft";
            this.flowActionsLeft.Size = new System.Drawing.Size(490, 36);
            this.flowActionsLeft.TabIndex = 0;
            this.flowActionsLeft.Text = "flowActionsLeft";
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(400, 0);
            this.buttonRefresh.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Radius = 8;
            this.buttonRefresh.Size = new System.Drawing.Size(80, 36);
            this.buttonRefresh.TabIndex = 3;
            this.buttonRefresh.Text = "刷新";
            this.buttonRefresh.WaveSize = 0;
            // 
            // buttonQuery
            // 
            this.buttonQuery.Location = new System.Drawing.Point(312, 0);
            this.buttonQuery.Margin = new System.Windows.Forms.Padding(0);
            this.buttonQuery.Name = "buttonQuery";
            this.buttonQuery.Radius = 8;
            this.buttonQuery.Size = new System.Drawing.Size(80, 36);
            this.buttonQuery.TabIndex = 2;
            this.buttonQuery.Text = "查询";
            this.buttonQuery.WaveSize = 0;
            // 
            // inputSearch
            // 
            this.inputSearch.Location = new System.Drawing.Point(160, 0);
            this.inputSearch.Margin = new System.Windows.Forms.Padding(0);
            this.inputSearch.Name = "inputSearch";
            this.inputSearch.PlaceholderText = "搜索来源 / 消息";
            this.inputSearch.Size = new System.Drawing.Size(144, 36);
            this.inputSearch.TabIndex = 1;
            this.inputSearch.WaveSize = 0;
            // 
            // selectLogFile
            // 
            this.selectLogFile.Location = new System.Drawing.Point(0, 0);
            this.selectLogFile.Margin = new System.Windows.Forms.Padding(0);
            this.selectLogFile.Name = "selectLogFile";
            this.selectLogFile.PlaceholderText = "选择日志文件";
            this.selectLogFile.Size = new System.Drawing.Size(152, 36);
            this.selectLogFile.TabIndex = 0;
            this.selectLogFile.WaveSize = 0;
            // 
            // flowActionsRight
            // 
            this.flowActionsRight.Controls.Add(this.buttonDebug);
            this.flowActionsRight.Controls.Add(this.buttonInfo);
            this.flowActionsRight.Controls.Add(this.buttonWarn);
            this.flowActionsRight.Controls.Add(this.buttonError);
            this.flowActionsRight.Controls.Add(this.buttonAll);
            this.flowActionsRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowActionsRight.Gap = 8;
            this.flowActionsRight.Location = new System.Drawing.Point(534, 4);
            this.flowActionsRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowActionsRight.Name = "flowActionsRight";
            this.flowActionsRight.Size = new System.Drawing.Size(296, 36);
            this.flowActionsRight.TabIndex = 1;
            this.flowActionsRight.Text = "flowActionsRight";
            // 
            // buttonDebug
            // 
            this.buttonDebug.Location = new System.Drawing.Point(240, 0);
            this.buttonDebug.Margin = new System.Windows.Forms.Padding(0);
            this.buttonDebug.Name = "buttonDebug";
            this.buttonDebug.Radius = 8;
            this.buttonDebug.Size = new System.Drawing.Size(56, 36);
            this.buttonDebug.TabIndex = 0;
            this.buttonDebug.Text = "调试";
            this.buttonDebug.WaveSize = 0;
            // 
            // buttonInfo
            // 
            this.buttonInfo.Location = new System.Drawing.Point(184, 0);
            this.buttonInfo.Margin = new System.Windows.Forms.Padding(0);
            this.buttonInfo.Name = "buttonInfo";
            this.buttonInfo.Radius = 8;
            this.buttonInfo.Size = new System.Drawing.Size(48, 36);
            this.buttonInfo.TabIndex = 1;
            this.buttonInfo.Text = "信息";
            this.buttonInfo.WaveSize = 0;
            // 
            // buttonWarn
            // 
            this.buttonWarn.Location = new System.Drawing.Point(128, 0);
            this.buttonWarn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonWarn.Name = "buttonWarn";
            this.buttonWarn.Radius = 8;
            this.buttonWarn.Size = new System.Drawing.Size(48, 36);
            this.buttonWarn.TabIndex = 2;
            this.buttonWarn.Text = "警告";
            this.buttonWarn.WaveSize = 0;
            // 
            // buttonError
            // 
            this.buttonError.Location = new System.Drawing.Point(64, 0);
            this.buttonError.Margin = new System.Windows.Forms.Padding(0);
            this.buttonError.Name = "buttonError";
            this.buttonError.Radius = 8;
            this.buttonError.Size = new System.Drawing.Size(56, 36);
            this.buttonError.TabIndex = 3;
            this.buttonError.Text = "错误";
            this.buttonError.WaveSize = 0;
            // 
            // buttonAll
            // 
            this.buttonAll.Location = new System.Drawing.Point(0, 0);
            this.buttonAll.Margin = new System.Windows.Forms.Padding(0);
            this.buttonAll.Name = "buttonAll";
            this.buttonAll.Radius = 8;
            this.buttonAll.Size = new System.Drawing.Size(56, 36);
            this.buttonAll.TabIndex = 4;
            this.buttonAll.Text = "全部";
            this.buttonAll.Type = AntdUI.TTypeMini.Primary;
            this.buttonAll.WaveSize = 0;
            // 
            // RunLogPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "RunLogPage";
            this.Size = new System.Drawing.Size(850, 680);
            this.panelRoot.ResumeLayout(false);
            this.panelTableCard.ResumeLayout(false);
            this.panelTableFooter.ResumeLayout(false);
            this.flowStats.ResumeLayout(false);
            this.panelInfoCard.ResumeLayout(false);
            this.panelWarnCard.ResumeLayout(false);
            this.panelErrorCard.ResumeLayout(false);
            this.panelTotalCard.ResumeLayout(false);
            this.panelToolbar.ResumeLayout(false);
            this.flowActionsLeft.ResumeLayout(false);
            this.flowActionsRight.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.Panel panelRoot;
        private AntdUI.Panel panelToolbar;
        private AntdUI.FlowPanel flowActionsLeft;
        private AntdUI.Select selectLogFile;
        private AntdUI.Input inputSearch;
        private AntdUI.Button buttonQuery;
        private AntdUI.Button buttonRefresh;
        private AntdUI.FlowPanel flowActionsRight;
        private AntdUI.Button buttonAll;
        private AntdUI.Button buttonError;
        private AntdUI.Button buttonWarn;
        private AntdUI.Button buttonInfo;
        private AntdUI.Button buttonDebug;
        private AntdUI.FlowPanel flowStats;
        private AntdUI.Panel panelTotalCard;
        private AntdUI.Label labelTotalCount;
        private AntdUI.Label labelTotalTitle;
        private AntdUI.Panel panelErrorCard;
        private AntdUI.Label labelErrorCount;
        private AntdUI.Label labelErrorTitle;
        private AntdUI.Panel panelWarnCard;
        private AntdUI.Label labelWarnCount;
        private AntdUI.Label labelWarnTitle;
        private AntdUI.Panel panelInfoCard;
        private AntdUI.Label labelInfoCount;
        private AntdUI.Label labelInfoTitle;
        private AntdUI.Panel panelTableCard;
        private AntdUI.Table tableRunLogs;
        private AntdUI.Panel panelTableFooter;
        private AntdUI.Pagination paginationRunLogs;
    }
}