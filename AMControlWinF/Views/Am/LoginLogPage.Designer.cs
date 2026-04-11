namespace AMControlWinF.Views.Am
{
    partial class LoginLogPage
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.panelRoot = new AntdUI.Panel();
            this.panelTableCard = new AntdUI.Panel();
            this.tableLogs = new AntdUI.Table();
            this.panelTableFooter = new AntdUI.Panel();
            this.paginationLogs = new AntdUI.Pagination();
            this.flowStats = new AntdUI.FlowPanel();
            this.panelFailedCard = new AntdUI.Panel();
            this.labelFailedCount = new AntdUI.Label();
            this.labelFailedTitle = new AntdUI.Label();
            this.panelSuccessCard = new AntdUI.Panel();
            this.labelSuccessCount = new AntdUI.Label();
            this.labelSuccessTitle = new AntdUI.Label();
            this.panelTotalCard = new AntdUI.Panel();
            this.labelTotalCount = new AntdUI.Label();
            this.labelTotalTitle = new AntdUI.Label();
            this.panelToolbar = new AntdUI.Panel();
            this.flowActionsLeft = new AntdUI.FlowPanel();
            this.buttonQuery = new AntdUI.Button();
            this.pickerEnd = new AntdUI.DatePicker();
            this.pickerStart = new AntdUI.DatePicker();
            this.inputSearch = new AntdUI.Input();
            this.flowActionsRight = new AntdUI.FlowPanel();
            this.buttonToday = new AntdUI.Button();
            this.buttonFailed = new AntdUI.Button();
            this.buttonSuccess = new AntdUI.Button();
            this.buttonAll = new AntdUI.Button();
            this.panelRoot.SuspendLayout();
            this.panelTableCard.SuspendLayout();
            this.panelTableFooter.SuspendLayout();
            this.flowStats.SuspendLayout();
            this.panelFailedCard.SuspendLayout();
            this.panelSuccessCard.SuspendLayout();
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
            this.panelTableCard.Controls.Add(this.tableLogs);
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
            // tableLogs
            // 
            this.tableLogs.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tableLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLogs.EmptyHeader = true;
            this.tableLogs.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.tableLogs.Gap = 8;
            this.tableLogs.Gaps = new System.Drawing.Size(8, 8);
            this.tableLogs.Location = new System.Drawing.Point(12, 12);
            this.tableLogs.Margin = new System.Windows.Forms.Padding(0);
            this.tableLogs.Name = "tableLogs";
            this.tableLogs.ShowTip = false;
            this.tableLogs.Size = new System.Drawing.Size(810, 458);
            this.tableLogs.TabIndex = 0;
            this.tableLogs.Text = "tableLogs";
            // 
            // panelTableFooter
            // 
            this.panelTableFooter.Controls.Add(this.paginationLogs);
            this.panelTableFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelTableFooter.Location = new System.Drawing.Point(12, 470);
            this.panelTableFooter.Margin = new System.Windows.Forms.Padding(0);
            this.panelTableFooter.Name = "panelTableFooter";
            this.panelTableFooter.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.panelTableFooter.Radius = 0;
            this.panelTableFooter.Size = new System.Drawing.Size(810, 50);
            this.panelTableFooter.TabIndex = 1;
            // 
            // paginationLogs
            // 
            this.paginationLogs.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.paginationLogs.Location = new System.Drawing.Point(0, 8);
            this.paginationLogs.Margin = new System.Windows.Forms.Padding(0);
            this.paginationLogs.Name = "paginationLogs";
            this.paginationLogs.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.paginationLogs.Size = new System.Drawing.Size(810, 42);
            this.paginationLogs.SizeChangerWidth = 72;
            this.paginationLogs.TabIndex = 0;
            this.paginationLogs.Text = "paginationLogs";
            // 
            // flowStats
            // 
            this.flowStats.Controls.Add(this.panelFailedCard);
            this.flowStats.Controls.Add(this.panelSuccessCard);
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
            // panelFailedCard
            // 
            this.panelFailedCard.BackColor = System.Drawing.Color.Transparent;
            this.panelFailedCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(229)))), ((int)(((byte)(235)))));
            this.panelFailedCard.Controls.Add(this.labelFailedCount);
            this.panelFailedCard.Controls.Add(this.labelFailedTitle);
            this.panelFailedCard.Location = new System.Drawing.Point(380, 6);
            this.panelFailedCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelFailedCard.Name = "panelFailedCard";
            this.panelFailedCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelFailedCard.Radius = 12;
            this.panelFailedCard.Shadow = 4;
            this.panelFailedCard.ShadowOpacity = 0.2F;
            this.panelFailedCard.ShadowOpacityAnimation = true;
            this.panelFailedCard.Size = new System.Drawing.Size(180, 72);
            this.panelFailedCard.TabIndex = 2;
            // 
            // labelFailedCount
            // 
            this.labelFailedCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelFailedCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelFailedCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.labelFailedCount.Location = new System.Drawing.Point(100, 16);
            this.labelFailedCount.Name = "labelFailedCount";
            this.labelFailedCount.Size = new System.Drawing.Size(64, 40);
            this.labelFailedCount.TabIndex = 1;
            this.labelFailedCount.Text = "0";
            // 
            // labelFailedTitle
            // 
            this.labelFailedTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelFailedTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelFailedTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.labelFailedTitle.Location = new System.Drawing.Point(16, 16);
            this.labelFailedTitle.Name = "labelFailedTitle";
            this.labelFailedTitle.Size = new System.Drawing.Size(78, 40);
            this.labelFailedTitle.TabIndex = 0;
            this.labelFailedTitle.Text = "登录失败";
            // 
            // panelSuccessCard
            // 
            this.panelSuccessCard.BackColor = System.Drawing.Color.Transparent;
            this.panelSuccessCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(229)))), ((int)(((byte)(235)))));
            this.panelSuccessCard.Controls.Add(this.labelSuccessCount);
            this.panelSuccessCard.Controls.Add(this.labelSuccessTitle);
            this.panelSuccessCard.Location = new System.Drawing.Point(192, 6);
            this.panelSuccessCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelSuccessCard.Name = "panelSuccessCard";
            this.panelSuccessCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelSuccessCard.Radius = 12;
            this.panelSuccessCard.Shadow = 4;
            this.panelSuccessCard.ShadowOpacity = 0.2F;
            this.panelSuccessCard.ShadowOpacityAnimation = true;
            this.panelSuccessCard.Size = new System.Drawing.Size(180, 72);
            this.panelSuccessCard.TabIndex = 1;
            // 
            // labelSuccessCount
            // 
            this.labelSuccessCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelSuccessCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelSuccessCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(196)))), ((int)(((byte)(26)))));
            this.labelSuccessCount.Location = new System.Drawing.Point(85, 16);
            this.labelSuccessCount.Name = "labelSuccessCount";
            this.labelSuccessCount.Size = new System.Drawing.Size(79, 40);
            this.labelSuccessCount.TabIndex = 1;
            this.labelSuccessCount.Text = "0";
            // 
            // labelSuccessTitle
            // 
            this.labelSuccessTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelSuccessTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelSuccessTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(196)))), ((int)(((byte)(26)))));
            this.labelSuccessTitle.Location = new System.Drawing.Point(16, 16);
            this.labelSuccessTitle.Name = "labelSuccessTitle";
            this.labelSuccessTitle.Size = new System.Drawing.Size(63, 40);
            this.labelSuccessTitle.TabIndex = 0;
            this.labelSuccessTitle.Text = "登录成功";
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
            this.flowActionsLeft.Controls.Add(this.buttonQuery);
            this.flowActionsLeft.Controls.Add(this.pickerEnd);
            this.flowActionsLeft.Controls.Add(this.pickerStart);
            this.flowActionsLeft.Controls.Add(this.inputSearch);
            this.flowActionsLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowActionsLeft.Gap = 8;
            this.flowActionsLeft.Location = new System.Drawing.Point(4, 4);
            this.flowActionsLeft.Name = "flowActionsLeft";
            this.flowActionsLeft.Size = new System.Drawing.Size(475, 36);
            this.flowActionsLeft.TabIndex = 0;
            this.flowActionsLeft.Text = "flowActionsLeft";
            // 
            // buttonQuery
            // 
            this.buttonQuery.Location = new System.Drawing.Point(384, 0);
            this.buttonQuery.Margin = new System.Windows.Forms.Padding(0);
            this.buttonQuery.Name = "buttonQuery";
            this.buttonQuery.Radius = 8;
            this.buttonQuery.Size = new System.Drawing.Size(80, 36);
            this.buttonQuery.TabIndex = 3;
            this.buttonQuery.Text = "查询";
            this.buttonQuery.WaveSize = 0;
            // 
            // pickerEnd
            // 
            this.pickerEnd.Location = new System.Drawing.Point(256, 0);
            this.pickerEnd.Margin = new System.Windows.Forms.Padding(0);
            this.pickerEnd.Name = "pickerEnd";
            this.pickerEnd.Size = new System.Drawing.Size(120, 36);
            this.pickerEnd.TabIndex = 2;
            this.pickerEnd.WaveSize = 0;
            // 
            // pickerStart
            // 
            this.pickerStart.Location = new System.Drawing.Point(128, 0);
            this.pickerStart.Margin = new System.Windows.Forms.Padding(0);
            this.pickerStart.Name = "pickerStart";
            this.pickerStart.Size = new System.Drawing.Size(120, 36);
            this.pickerStart.TabIndex = 1;
            this.pickerStart.WaveSize = 0;
            // 
            // inputSearch
            // 
            this.inputSearch.Location = new System.Drawing.Point(0, 0);
            this.inputSearch.Margin = new System.Windows.Forms.Padding(0);
            this.inputSearch.Name = "inputSearch";
            this.inputSearch.PlaceholderText = "搜索登录名";
            this.inputSearch.Size = new System.Drawing.Size(120, 36);
            this.inputSearch.TabIndex = 0;
            this.inputSearch.WaveSize = 0;
            // 
            // flowActionsRight
            // 
            this.flowActionsRight.Controls.Add(this.buttonToday);
            this.flowActionsRight.Controls.Add(this.buttonFailed);
            this.flowActionsRight.Controls.Add(this.buttonSuccess);
            this.flowActionsRight.Controls.Add(this.buttonAll);
            this.flowActionsRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowActionsRight.Gap = 8;
            this.flowActionsRight.Location = new System.Drawing.Point(503, 4);
            this.flowActionsRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowActionsRight.Name = "flowActionsRight";
            this.flowActionsRight.Size = new System.Drawing.Size(327, 36);
            this.flowActionsRight.TabIndex = 1;
            this.flowActionsRight.Text = "flowActionsRight";
            // 
            // buttonToday
            // 
            this.buttonToday.Location = new System.Drawing.Point(240, 0);
            this.buttonToday.Margin = new System.Windows.Forms.Padding(0);
            this.buttonToday.Name = "buttonToday";
            this.buttonToday.Radius = 8;
            this.buttonToday.Size = new System.Drawing.Size(72, 36);
            this.buttonToday.TabIndex = 0;
            this.buttonToday.Text = "今日日志";
            this.buttonToday.WaveSize = 0;
            // 
            // buttonFailed
            // 
            this.buttonFailed.Location = new System.Drawing.Point(160, 0);
            this.buttonFailed.Margin = new System.Windows.Forms.Padding(0);
            this.buttonFailed.Name = "buttonFailed";
            this.buttonFailed.Radius = 8;
            this.buttonFailed.Size = new System.Drawing.Size(72, 36);
            this.buttonFailed.TabIndex = 1;
            this.buttonFailed.Text = "失败日志";
            this.buttonFailed.WaveSize = 0;
            // 
            // buttonSuccess
            // 
            this.buttonSuccess.Location = new System.Drawing.Point(80, 0);
            this.buttonSuccess.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSuccess.Name = "buttonSuccess";
            this.buttonSuccess.Radius = 8;
            this.buttonSuccess.Size = new System.Drawing.Size(72, 36);
            this.buttonSuccess.TabIndex = 2;
            this.buttonSuccess.Text = "成功日志";
            this.buttonSuccess.WaveSize = 0;
            // 
            // buttonAll
            // 
            this.buttonAll.Location = new System.Drawing.Point(0, 0);
            this.buttonAll.Margin = new System.Windows.Forms.Padding(0);
            this.buttonAll.Name = "buttonAll";
            this.buttonAll.Radius = 8;
            this.buttonAll.Size = new System.Drawing.Size(72, 36);
            this.buttonAll.TabIndex = 3;
            this.buttonAll.Text = "全部日志";
            this.buttonAll.Type = AntdUI.TTypeMini.Primary;
            this.buttonAll.WaveSize = 0;
            // 
            // LoginLogPage
            // 
            this.Controls.Add(this.panelRoot);
            this.Name = "LoginLogPage";
            this.Size = new System.Drawing.Size(850, 680);
            this.panelRoot.ResumeLayout(false);
            this.panelTableCard.ResumeLayout(false);
            this.panelTableFooter.ResumeLayout(false);
            this.flowStats.ResumeLayout(false);
            this.panelFailedCard.ResumeLayout(false);
            this.panelSuccessCard.ResumeLayout(false);
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
        private AntdUI.Input inputSearch;
        private AntdUI.DatePicker pickerStart;
        private AntdUI.DatePicker pickerEnd;
        private AntdUI.Button buttonQuery;
        private AntdUI.FlowPanel flowActionsRight;
        private AntdUI.Button buttonToday;
        private AntdUI.Button buttonFailed;
        private AntdUI.Button buttonSuccess;
        private AntdUI.Button buttonAll;
        private AntdUI.FlowPanel flowStats;
        private AntdUI.Panel panelTotalCard;
        private AntdUI.Label labelTotalCount;
        private AntdUI.Label labelTotalTitle;
        private AntdUI.Panel panelSuccessCard;
        private AntdUI.Label labelSuccessCount;
        private AntdUI.Label labelSuccessTitle;
        private AntdUI.Panel panelFailedCard;
        private AntdUI.Label labelFailedCount;
        private AntdUI.Label labelFailedTitle;
        private AntdUI.Panel panelTableCard;
        private AntdUI.Table tableLogs;
        private AntdUI.Panel panelTableFooter;
        private AntdUI.Pagination paginationLogs;
    }
}