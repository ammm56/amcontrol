namespace AMControlWinF.Views.AlarmLog
{
    partial class AlarmHistoryPage
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
            this.tableHistory = new AntdUI.Table();
            this.panelTableFooter = new AntdUI.Panel();
            this.paginationHistory = new AntdUI.Pagination();
            this.flowStats = new AntdUI.FlowPanel();
            this.panelClearedCard = new AntdUI.Panel();
            this.labelClearedCount = new AntdUI.Label();
            this.labelClearedTitle = new AntdUI.Label();
            this.panelUnclearedCard = new AntdUI.Panel();
            this.labelUnclearedCount = new AntdUI.Label();
            this.labelUnclearedTitle = new AntdUI.Label();
            this.panelCriticalCard = new AntdUI.Panel();
            this.labelCriticalCount = new AntdUI.Label();
            this.labelCriticalTitle = new AntdUI.Label();
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
            this.buttonUncleared = new AntdUI.Button();
            this.buttonWarning = new AntdUI.Button();
            this.buttonAlarm = new AntdUI.Button();
            this.buttonCritical = new AntdUI.Button();
            this.buttonAll = new AntdUI.Button();
            this.panelRoot.SuspendLayout();
            this.panelTableCard.SuspendLayout();
            this.panelTableFooter.SuspendLayout();
            this.flowStats.SuspendLayout();
            this.panelClearedCard.SuspendLayout();
            this.panelUnclearedCard.SuspendLayout();
            this.panelCriticalCard.SuspendLayout();
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
            this.panelTableCard.Controls.Add(this.tableHistory);
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
            // tableHistory
            // 
            this.tableHistory.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tableHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableHistory.EmptyHeader = true;
            this.tableHistory.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.tableHistory.Gap = 8;
            this.tableHistory.Gaps = new System.Drawing.Size(8, 8);
            this.tableHistory.Location = new System.Drawing.Point(12, 12);
            this.tableHistory.Margin = new System.Windows.Forms.Padding(0);
            this.tableHistory.Name = "tableHistory";
            this.tableHistory.ShowTip = false;
            this.tableHistory.Size = new System.Drawing.Size(810, 458);
            this.tableHistory.TabIndex = 0;
            this.tableHistory.Text = "tableHistory";
            // 
            // panelTableFooter
            // 
            this.panelTableFooter.Controls.Add(this.paginationHistory);
            this.panelTableFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelTableFooter.Location = new System.Drawing.Point(12, 470);
            this.panelTableFooter.Margin = new System.Windows.Forms.Padding(0);
            this.panelTableFooter.Name = "panelTableFooter";
            this.panelTableFooter.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.panelTableFooter.Radius = 0;
            this.panelTableFooter.Size = new System.Drawing.Size(810, 50);
            this.panelTableFooter.TabIndex = 1;
            // 
            // paginationHistory
            // 
            this.paginationHistory.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.paginationHistory.Location = new System.Drawing.Point(0, 8);
            this.paginationHistory.Margin = new System.Windows.Forms.Padding(0);
            this.paginationHistory.Name = "paginationHistory";
            this.paginationHistory.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.paginationHistory.Size = new System.Drawing.Size(810, 42);
            this.paginationHistory.SizeChangerWidth = 72;
            this.paginationHistory.TabIndex = 0;
            this.paginationHistory.Text = "paginationHistory";
            // 
            // flowStats
            // 
            this.flowStats.Controls.Add(this.panelClearedCard);
            this.flowStats.Controls.Add(this.panelUnclearedCard);
            this.flowStats.Controls.Add(this.panelCriticalCard);
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
            // panelClearedCard
            // 
            this.panelClearedCard.BackColor = System.Drawing.Color.Transparent;
            this.panelClearedCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(229)))), ((int)(((byte)(235)))));
            this.panelClearedCard.Controls.Add(this.labelClearedCount);
            this.panelClearedCard.Controls.Add(this.labelClearedTitle);
            this.panelClearedCard.Location = new System.Drawing.Point(568, 6);
            this.panelClearedCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelClearedCard.Name = "panelClearedCard";
            this.panelClearedCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelClearedCard.Radius = 12;
            this.panelClearedCard.Shadow = 4;
            this.panelClearedCard.ShadowOpacity = 0.2F;
            this.panelClearedCard.ShadowOpacityAnimation = true;
            this.panelClearedCard.Size = new System.Drawing.Size(180, 72);
            this.panelClearedCard.TabIndex = 3;
            // 
            // labelClearedCount
            // 
            this.labelClearedCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelClearedCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelClearedCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(196)))), ((int)(((byte)(26)))));
            this.labelClearedCount.Location = new System.Drawing.Point(104, 16);
            this.labelClearedCount.Name = "labelClearedCount";
            this.labelClearedCount.Size = new System.Drawing.Size(60, 40);
            this.labelClearedCount.TabIndex = 1;
            this.labelClearedCount.Text = "0";
            // 
            // labelClearedTitle
            // 
            this.labelClearedTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelClearedTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelClearedTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(196)))), ((int)(((byte)(26)))));
            this.labelClearedTitle.Location = new System.Drawing.Point(16, 16);
            this.labelClearedTitle.Name = "labelClearedTitle";
            this.labelClearedTitle.Size = new System.Drawing.Size(72, 40);
            this.labelClearedTitle.TabIndex = 0;
            this.labelClearedTitle.Text = "已清除";
            // 
            // panelUnclearedCard
            // 
            this.panelUnclearedCard.BackColor = System.Drawing.Color.Transparent;
            this.panelUnclearedCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(229)))), ((int)(((byte)(235)))));
            this.panelUnclearedCard.Controls.Add(this.labelUnclearedCount);
            this.panelUnclearedCard.Controls.Add(this.labelUnclearedTitle);
            this.panelUnclearedCard.Location = new System.Drawing.Point(380, 6);
            this.panelUnclearedCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelUnclearedCard.Name = "panelUnclearedCard";
            this.panelUnclearedCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelUnclearedCard.Radius = 12;
            this.panelUnclearedCard.Shadow = 4;
            this.panelUnclearedCard.ShadowOpacity = 0.2F;
            this.panelUnclearedCard.ShadowOpacityAnimation = true;
            this.panelUnclearedCard.Size = new System.Drawing.Size(180, 72);
            this.panelUnclearedCard.TabIndex = 2;
            // 
            // labelUnclearedCount
            // 
            this.labelUnclearedCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelUnclearedCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelUnclearedCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.labelUnclearedCount.Location = new System.Drawing.Point(104, 16);
            this.labelUnclearedCount.Name = "labelUnclearedCount";
            this.labelUnclearedCount.Size = new System.Drawing.Size(60, 40);
            this.labelUnclearedCount.TabIndex = 1;
            this.labelUnclearedCount.Text = "0";
            // 
            // labelUnclearedTitle
            // 
            this.labelUnclearedTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelUnclearedTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelUnclearedTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.labelUnclearedTitle.Location = new System.Drawing.Point(16, 16);
            this.labelUnclearedTitle.Name = "labelUnclearedTitle";
            this.labelUnclearedTitle.Size = new System.Drawing.Size(72, 40);
            this.labelUnclearedTitle.TabIndex = 0;
            this.labelUnclearedTitle.Text = "未清除";
            // 
            // panelCriticalCard
            // 
            this.panelCriticalCard.BackColor = System.Drawing.Color.Transparent;
            this.panelCriticalCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(229)))), ((int)(((byte)(235)))));
            this.panelCriticalCard.Controls.Add(this.labelCriticalCount);
            this.panelCriticalCard.Controls.Add(this.labelCriticalTitle);
            this.panelCriticalCard.Location = new System.Drawing.Point(192, 6);
            this.panelCriticalCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelCriticalCard.Name = "panelCriticalCard";
            this.panelCriticalCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelCriticalCard.Radius = 12;
            this.panelCriticalCard.Shadow = 4;
            this.panelCriticalCard.ShadowOpacity = 0.2F;
            this.panelCriticalCard.ShadowOpacityAnimation = true;
            this.panelCriticalCard.Size = new System.Drawing.Size(180, 72);
            this.panelCriticalCard.TabIndex = 1;
            // 
            // labelCriticalCount
            // 
            this.labelCriticalCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelCriticalCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelCriticalCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(140)))), ((int)(((byte)(0)))));
            this.labelCriticalCount.Location = new System.Drawing.Point(104, 16);
            this.labelCriticalCount.Name = "labelCriticalCount";
            this.labelCriticalCount.Size = new System.Drawing.Size(60, 40);
            this.labelCriticalCount.TabIndex = 1;
            this.labelCriticalCount.Text = "0";
            // 
            // labelCriticalTitle
            // 
            this.labelCriticalTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelCriticalTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelCriticalTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(140)))), ((int)(((byte)(0)))));
            this.labelCriticalTitle.Location = new System.Drawing.Point(16, 16);
            this.labelCriticalTitle.Name = "labelCriticalTitle";
            this.labelCriticalTitle.Size = new System.Drawing.Size(72, 40);
            this.labelCriticalTitle.TabIndex = 0;
            this.labelCriticalTitle.Text = "严重报警";
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
            this.labelTotalTitle.Text = "报警总数";
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
            this.flowActionsLeft.Size = new System.Drawing.Size(435, 36);
            this.flowActionsLeft.TabIndex = 0;
            this.flowActionsLeft.Text = "flowActionsLeft";
            // 
            // buttonQuery
            // 
            this.buttonQuery.Location = new System.Drawing.Point(364, 0);
            this.buttonQuery.Margin = new System.Windows.Forms.Padding(0);
            this.buttonQuery.Name = "buttonQuery";
            this.buttonQuery.Radius = 8;
            this.buttonQuery.Size = new System.Drawing.Size(60, 36);
            this.buttonQuery.TabIndex = 3;
            this.buttonQuery.Text = "查询";
            this.buttonQuery.WaveSize = 0;
            // 
            // pickerEnd
            // 
            this.pickerEnd.Location = new System.Drawing.Point(236, 0);
            this.pickerEnd.Margin = new System.Windows.Forms.Padding(0);
            this.pickerEnd.Name = "pickerEnd";
            this.pickerEnd.Size = new System.Drawing.Size(120, 36);
            this.pickerEnd.TabIndex = 2;
            this.pickerEnd.WaveSize = 0;
            // 
            // pickerStart
            // 
            this.pickerStart.Location = new System.Drawing.Point(108, 0);
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
            this.inputSearch.PlaceholderText = "搜索代码 / 来源 / 消息";
            this.inputSearch.Size = new System.Drawing.Size(100, 36);
            this.inputSearch.TabIndex = 0;
            this.inputSearch.WaveSize = 0;
            // 
            // flowActionsRight
            // 
            this.flowActionsRight.Align = AntdUI.TAlignFlow.Right;
            this.flowActionsRight.Controls.Add(this.buttonUncleared);
            this.flowActionsRight.Controls.Add(this.buttonWarning);
            this.flowActionsRight.Controls.Add(this.buttonAlarm);
            this.flowActionsRight.Controls.Add(this.buttonCritical);
            this.flowActionsRight.Controls.Add(this.buttonAll);
            this.flowActionsRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowActionsRight.Gap = 8;
            this.flowActionsRight.Location = new System.Drawing.Point(469, 4);
            this.flowActionsRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowActionsRight.Name = "flowActionsRight";
            this.flowActionsRight.Size = new System.Drawing.Size(361, 36);
            this.flowActionsRight.TabIndex = 1;
            this.flowActionsRight.Text = "flowActionsRight";
            // 
            // buttonUncleared
            // 
            this.buttonUncleared.Location = new System.Drawing.Point(296, 0);
            this.buttonUncleared.Margin = new System.Windows.Forms.Padding(0);
            this.buttonUncleared.Name = "buttonUncleared";
            this.buttonUncleared.Radius = 8;
            this.buttonUncleared.Size = new System.Drawing.Size(65, 36);
            this.buttonUncleared.TabIndex = 0;
            this.buttonUncleared.Text = "未清除";
            this.buttonUncleared.WaveSize = 0;
            // 
            // buttonWarning
            // 
            this.buttonWarning.Location = new System.Drawing.Point(228, 0);
            this.buttonWarning.Margin = new System.Windows.Forms.Padding(0);
            this.buttonWarning.Name = "buttonWarning";
            this.buttonWarning.Radius = 8;
            this.buttonWarning.Size = new System.Drawing.Size(60, 36);
            this.buttonWarning.TabIndex = 2;
            this.buttonWarning.Text = "警告";
            this.buttonWarning.WaveSize = 0;
            // 
            // buttonAlarm
            // 
            this.buttonAlarm.Location = new System.Drawing.Point(160, 0);
            this.buttonAlarm.Margin = new System.Windows.Forms.Padding(0);
            this.buttonAlarm.Name = "buttonAlarm";
            this.buttonAlarm.Radius = 8;
            this.buttonAlarm.Size = new System.Drawing.Size(60, 36);
            this.buttonAlarm.TabIndex = 3;
            this.buttonAlarm.Text = "报警";
            this.buttonAlarm.WaveSize = 0;
            // 
            // buttonCritical
            // 
            this.buttonCritical.Location = new System.Drawing.Point(92, 0);
            this.buttonCritical.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCritical.Name = "buttonCritical";
            this.buttonCritical.Radius = 8;
            this.buttonCritical.Size = new System.Drawing.Size(60, 36);
            this.buttonCritical.TabIndex = 4;
            this.buttonCritical.Text = "严重";
            this.buttonCritical.WaveSize = 0;
            // 
            // buttonAll
            // 
            this.buttonAll.Location = new System.Drawing.Point(12, 0);
            this.buttonAll.Margin = new System.Windows.Forms.Padding(0);
            this.buttonAll.Name = "buttonAll";
            this.buttonAll.Radius = 8;
            this.buttonAll.Size = new System.Drawing.Size(72, 36);
            this.buttonAll.TabIndex = 5;
            this.buttonAll.Text = "全部报警";
            this.buttonAll.Type = AntdUI.TTypeMini.Primary;
            this.buttonAll.WaveSize = 0;
            // 
            // AlarmHistoryPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "AlarmHistoryPage";
            this.Size = new System.Drawing.Size(850, 680);
            this.panelRoot.ResumeLayout(false);
            this.panelTableCard.ResumeLayout(false);
            this.panelTableFooter.ResumeLayout(false);
            this.flowStats.ResumeLayout(false);
            this.panelClearedCard.ResumeLayout(false);
            this.panelUnclearedCard.ResumeLayout(false);
            this.panelCriticalCard.ResumeLayout(false);
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
        private AntdUI.Button buttonAll;
        private AntdUI.Button buttonCritical;
        private AntdUI.Button buttonAlarm;
        private AntdUI.Button buttonWarning;
        private AntdUI.Button buttonUncleared;
        private AntdUI.FlowPanel flowStats;
        private AntdUI.Panel panelTotalCard;
        private AntdUI.Label labelTotalCount;
        private AntdUI.Label labelTotalTitle;
        private AntdUI.Panel panelCriticalCard;
        private AntdUI.Label labelCriticalCount;
        private AntdUI.Label labelCriticalTitle;
        private AntdUI.Panel panelUnclearedCard;
        private AntdUI.Label labelUnclearedCount;
        private AntdUI.Label labelUnclearedTitle;
        private AntdUI.Panel panelClearedCard;
        private AntdUI.Label labelClearedCount;
        private AntdUI.Label labelClearedTitle;
        private AntdUI.Panel panelTableCard;
        private AntdUI.Table tableHistory;
        private AntdUI.Panel panelTableFooter;
        private AntdUI.Pagination paginationHistory;
    }
}