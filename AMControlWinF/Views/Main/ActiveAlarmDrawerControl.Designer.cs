namespace AMControlWinF.Views.Main
{
    partial class ActiveAlarmDrawerControl
    {
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 释放资源。
        /// </summary>
        /// <param name="disposing">是否释放托管资源。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器生成代码。
        /// </summary>
        private void InitializeComponent()
        {
            this.panelRoot = new AntdUI.Panel();
            this.panelFooter = new AntdUI.Panel();
            this.labelPageInfo = new AntdUI.Label();
            this.flowFooterRight = new AntdUI.FlowPanel();
            this.buttonNextPage = new AntdUI.Button();
            this.buttonPrevPage = new AntdUI.Button();
            this.labelPageRange = new AntdUI.Label();
            this.panelContent = new AntdUI.Panel();
            this.tableLayoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.panelDetailCard = new AntdUI.Panel();
            this.panelDetailScroll = new AntdUI.Panel();
            this.panelSuggestionCard = new AntdUI.Panel();
            this.labelSuggestionTitle = new AntdUI.Label();
            this.labelDetailSuggestion = new AntdUI.Label();
            this.panelDescriptionCard = new AntdUI.Panel();
            this.labelDescriptionTitle = new AntdUI.Label();
            this.labelDetailDescription = new AntdUI.Label();
            this.panelMetaCard = new AntdUI.Panel();
            this.labelMetaTitle = new AntdUI.Label();
            this.labelDetailMeta = new AntdUI.Label();
            this.panelMessageCard = new AntdUI.Panel();
            this.labelMessageTitle = new AntdUI.Label();
            this.labelDetailMessage = new AntdUI.Label();
            this.labelDetailHeader = new AntdUI.Label();
            this.panelListCard = new AntdUI.Panel();
            this.tableAlarms = new AntdUI.Table();
            this.panelSummary = new AntdUI.Panel();
            this.flowSummaryRight = new AntdUI.FlowPanel();
            this.buttonClearAll = new AntdUI.Button();
            this.buttonClearCurrent = new AntdUI.Button();
            this.buttonRefresh = new AntdUI.Button();
            this.labelSummaryHint = new AntdUI.Label();
            this.labelAlarmCount = new AntdUI.Label();
            this.labelSummaryTitle = new AntdUI.Label();
            this.panelRoot.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.flowFooterRight.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.tableLayoutMain.SuspendLayout();
            this.panelDetailCard.SuspendLayout();
            this.panelDetailScroll.SuspendLayout();
            this.panelSuggestionCard.SuspendLayout();
            this.panelDescriptionCard.SuspendLayout();
            this.panelMetaCard.SuspendLayout();
            this.panelMessageCard.SuspendLayout();
            this.panelListCard.SuspendLayout();
            this.panelSummary.SuspendLayout();
            this.flowSummaryRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.panelContent);
            this.panelRoot.Controls.Add(this.panelFooter);
            this.panelRoot.Controls.Add(this.panelSummary);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Margin = new System.Windows.Forms.Padding(0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(8);
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(760, 620);
            this.panelRoot.TabIndex = 0;
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.labelPageInfo);
            this.panelFooter.Controls.Add(this.flowFooterRight);
            this.panelFooter.Controls.Add(this.labelPageRange);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(8, 564);
            this.panelFooter.Margin = new System.Windows.Forms.Padding(0);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Padding = new System.Windows.Forms.Padding(8);
            this.panelFooter.Radius = 12;
            this.panelFooter.Shadow = 4;
            this.panelFooter.Size = new System.Drawing.Size(744, 48);
            this.panelFooter.TabIndex = 2;
            // 
            // labelPageInfo
            // 
            this.labelPageInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPageInfo.Location = new System.Drawing.Point(94, 12);
            this.labelPageInfo.Margin = new System.Windows.Forms.Padding(0);
            this.labelPageInfo.Name = "labelPageInfo";
            this.labelPageInfo.Size = new System.Drawing.Size(458, 24);
            this.labelPageInfo.TabIndex = 1;
            this.labelPageInfo.Text = "第 0 / 0 页";
            this.labelPageInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flowFooterRight
            // 
            this.flowFooterRight.Controls.Add(this.buttonNextPage);
            this.flowFooterRight.Controls.Add(this.buttonPrevPage);
            this.flowFooterRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowFooterRight.Gap = 8;
            this.flowFooterRight.Location = new System.Drawing.Point(552, 12);
            this.flowFooterRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowFooterRight.Name = "flowFooterRight";
            this.flowFooterRight.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.flowFooterRight.Size = new System.Drawing.Size(184, 24);
            this.flowFooterRight.TabIndex = 2;
            this.flowFooterRight.Text = "flowFooterRight";
            // 
            // buttonNextPage
            // 
            this.buttonNextPage.Location = new System.Drawing.Point(88, 0);
            this.buttonNextPage.Margin = new System.Windows.Forms.Padding(0);
            this.buttonNextPage.Name = "buttonNextPage";
            this.buttonNextPage.Radius = 8;
            this.buttonNextPage.Size = new System.Drawing.Size(88, 24);
            this.buttonNextPage.TabIndex = 0;
            this.buttonNextPage.Text = "下一页";
            this.buttonNextPage.WaveSize = 0;
            // 
            // buttonPrevPage
            // 
            this.buttonPrevPage.Location = new System.Drawing.Point(0, 0);
            this.buttonPrevPage.Margin = new System.Windows.Forms.Padding(0);
            this.buttonPrevPage.Name = "buttonPrevPage";
            this.buttonPrevPage.Radius = 8;
            this.buttonPrevPage.Size = new System.Drawing.Size(80, 24);
            this.buttonPrevPage.TabIndex = 1;
            this.buttonPrevPage.Text = "上一页";
            this.buttonPrevPage.WaveSize = 0;
            // 
            // labelPageRange
            // 
            this.labelPageRange.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelPageRange.Location = new System.Drawing.Point(12, 12);
            this.labelPageRange.Margin = new System.Windows.Forms.Padding(0);
            this.labelPageRange.Name = "labelPageRange";
            this.labelPageRange.Size = new System.Drawing.Size(82, 24);
            this.labelPageRange.TabIndex = 0;
            this.labelPageRange.Text = "0 / 0";
            this.labelPageRange.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelContent
            // 
            this.panelContent.Controls.Add(this.tableLayoutMain);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(8, 60);
            this.panelContent.Margin = new System.Windows.Forms.Padding(0);
            this.panelContent.Name = "panelContent";
            this.panelContent.Radius = 0;
            this.panelContent.ShadowOpacity = 0F;
            this.panelContent.ShadowOpacityHover = 0F;
            this.panelContent.Size = new System.Drawing.Size(744, 504);
            this.panelContent.TabIndex = 1;
            // 
            // tableLayoutMain
            // 
            this.tableLayoutMain.ColumnCount = 2;
            this.tableLayoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 220F));
            this.tableLayoutMain.Controls.Add(this.panelDetailCard, 1, 0);
            this.tableLayoutMain.Controls.Add(this.panelListCard, 0, 0);
            this.tableLayoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutMain.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutMain.Name = "tableLayoutMain";
            this.tableLayoutMain.RowCount = 1;
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutMain.Size = new System.Drawing.Size(744, 504);
            this.tableLayoutMain.TabIndex = 0;
            // 
            // panelDetailCard
            // 
            this.panelDetailCard.Controls.Add(this.panelDetailScroll);
            this.panelDetailCard.Controls.Add(this.labelDetailHeader);
            this.panelDetailCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetailCard.Location = new System.Drawing.Point(524, 0);
            this.panelDetailCard.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.panelDetailCard.Name = "panelDetailCard";
            this.panelDetailCard.Padding = new System.Windows.Forms.Padding(8);
            this.panelDetailCard.Radius = 12;
            this.panelDetailCard.Shadow = 4;
            this.panelDetailCard.Size = new System.Drawing.Size(220, 504);
            this.panelDetailCard.TabIndex = 1;
            // 
            // panelDetailScroll
            // 
            this.panelDetailScroll.AutoScroll = true;
            this.panelDetailScroll.Controls.Add(this.panelSuggestionCard);
            this.panelDetailScroll.Controls.Add(this.panelDescriptionCard);
            this.panelDetailScroll.Controls.Add(this.panelMetaCard);
            this.panelDetailScroll.Controls.Add(this.panelMessageCard);
            this.panelDetailScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetailScroll.Location = new System.Drawing.Point(12, 48);
            this.panelDetailScroll.Margin = new System.Windows.Forms.Padding(0);
            this.panelDetailScroll.Name = "panelDetailScroll";
            this.panelDetailScroll.Radius = 0;
            this.panelDetailScroll.ShadowOpacity = 0F;
            this.panelDetailScroll.ShadowOpacityHover = 0F;
            this.panelDetailScroll.Size = new System.Drawing.Size(196, 444);
            this.panelDetailScroll.TabIndex = 1;
            // 
            // panelSuggestionCard
            // 
            this.panelSuggestionCard.Controls.Add(this.labelDetailSuggestion);
            this.panelSuggestionCard.Controls.Add(this.labelSuggestionTitle);
            this.panelSuggestionCard.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSuggestionCard.Location = new System.Drawing.Point(0, 314);
            this.panelSuggestionCard.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.panelSuggestionCard.Name = "panelSuggestionCard";
            this.panelSuggestionCard.Padding = new System.Windows.Forms.Padding(10);
            this.panelSuggestionCard.Radius = 10;
            this.panelSuggestionCard.ShadowOpacity = 0F;
            this.panelSuggestionCard.ShadowOpacityHover = 0F;
            this.panelSuggestionCard.Size = new System.Drawing.Size(196, 96);
            this.panelSuggestionCard.TabIndex = 3;
            // 
            // labelSuggestionTitle
            // 
            this.labelSuggestionTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelSuggestionTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSuggestionTitle.Location = new System.Drawing.Point(14, 14);
            this.labelSuggestionTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelSuggestionTitle.Name = "labelSuggestionTitle";
            this.labelSuggestionTitle.Size = new System.Drawing.Size(168, 22);
            this.labelSuggestionTitle.TabIndex = 0;
            this.labelSuggestionTitle.Text = "处理建议";
            // 
            // labelDetailSuggestion
            // 
            this.labelDetailSuggestion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDetailSuggestion.Location = new System.Drawing.Point(14, 36);
            this.labelDetailSuggestion.Margin = new System.Windows.Forms.Padding(0);
            this.labelDetailSuggestion.Name = "labelDetailSuggestion";
            this.labelDetailSuggestion.Size = new System.Drawing.Size(168, 46);
            this.labelDetailSuggestion.TabIndex = 1;
            this.labelDetailSuggestion.Text = "-";
            // 
            // panelDescriptionCard
            // 
            this.panelDescriptionCard.Controls.Add(this.labelDetailDescription);
            this.panelDescriptionCard.Controls.Add(this.labelDescriptionTitle);
            this.panelDescriptionCard.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDescriptionCard.Location = new System.Drawing.Point(0, 218);
            this.panelDescriptionCard.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.panelDescriptionCard.Name = "panelDescriptionCard";
            this.panelDescriptionCard.Padding = new System.Windows.Forms.Padding(10);
            this.panelDescriptionCard.Radius = 10;
            this.panelDescriptionCard.ShadowOpacity = 0F;
            this.panelDescriptionCard.ShadowOpacityHover = 0F;
            this.panelDescriptionCard.Size = new System.Drawing.Size(196, 96);
            this.panelDescriptionCard.TabIndex = 2;
            // 
            // labelDescriptionTitle
            // 
            this.labelDescriptionTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDescriptionTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelDescriptionTitle.Location = new System.Drawing.Point(14, 14);
            this.labelDescriptionTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelDescriptionTitle.Name = "labelDescriptionTitle";
            this.labelDescriptionTitle.Size = new System.Drawing.Size(168, 22);
            this.labelDescriptionTitle.TabIndex = 0;
            this.labelDescriptionTitle.Text = "详细说明";
            // 
            // labelDetailDescription
            // 
            this.labelDetailDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDetailDescription.Location = new System.Drawing.Point(14, 36);
            this.labelDetailDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDetailDescription.Name = "labelDetailDescription";
            this.labelDetailDescription.Size = new System.Drawing.Size(168, 46);
            this.labelDetailDescription.TabIndex = 1;
            this.labelDetailDescription.Text = "-";
            // 
            // panelMetaCard
            // 
            this.panelMetaCard.Controls.Add(this.labelDetailMeta);
            this.panelMetaCard.Controls.Add(this.labelMetaTitle);
            this.panelMetaCard.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelMetaCard.Location = new System.Drawing.Point(0, 108);
            this.panelMetaCard.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.panelMetaCard.Name = "panelMetaCard";
            this.panelMetaCard.Padding = new System.Windows.Forms.Padding(10);
            this.panelMetaCard.Radius = 10;
            this.panelMetaCard.ShadowOpacity = 0F;
            this.panelMetaCard.ShadowOpacityHover = 0F;
            this.panelMetaCard.Size = new System.Drawing.Size(196, 110);
            this.panelMetaCard.TabIndex = 1;
            // 
            // labelMetaTitle
            // 
            this.labelMetaTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelMetaTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelMetaTitle.Location = new System.Drawing.Point(14, 14);
            this.labelMetaTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelMetaTitle.Name = "labelMetaTitle";
            this.labelMetaTitle.Size = new System.Drawing.Size(168, 22);
            this.labelMetaTitle.TabIndex = 0;
            this.labelMetaTitle.Text = "报警详情";
            // 
            // labelDetailMeta
            // 
            this.labelDetailMeta.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDetailMeta.Location = new System.Drawing.Point(14, 36);
            this.labelDetailMeta.Margin = new System.Windows.Forms.Padding(0);
            this.labelDetailMeta.Name = "labelDetailMeta";
            this.labelDetailMeta.Size = new System.Drawing.Size(168, 60);
            this.labelDetailMeta.TabIndex = 1;
            this.labelDetailMeta.Text = "-";
            // 
            // panelMessageCard
            // 
            this.panelMessageCard.Controls.Add(this.labelDetailMessage);
            this.panelMessageCard.Controls.Add(this.labelMessageTitle);
            this.panelMessageCard.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelMessageCard.Location = new System.Drawing.Point(0, 0);
            this.panelMessageCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelMessageCard.Name = "panelMessageCard";
            this.panelMessageCard.Padding = new System.Windows.Forms.Padding(10);
            this.panelMessageCard.Radius = 10;
            this.panelMessageCard.ShadowOpacity = 0F;
            this.panelMessageCard.ShadowOpacityHover = 0F;
            this.panelMessageCard.Size = new System.Drawing.Size(196, 108);
            this.panelMessageCard.TabIndex = 0;
            // 
            // labelMessageTitle
            // 
            this.labelMessageTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelMessageTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelMessageTitle.Location = new System.Drawing.Point(14, 14);
            this.labelMessageTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelMessageTitle.Name = "labelMessageTitle";
            this.labelMessageTitle.Size = new System.Drawing.Size(168, 22);
            this.labelMessageTitle.TabIndex = 0;
            this.labelMessageTitle.Text = "消息";
            // 
            // labelDetailMessage
            // 
            this.labelDetailMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDetailMessage.Location = new System.Drawing.Point(14, 36);
            this.labelDetailMessage.Margin = new System.Windows.Forms.Padding(0);
            this.labelDetailMessage.Name = "labelDetailMessage";
            this.labelDetailMessage.Size = new System.Drawing.Size(168, 58);
            this.labelDetailMessage.TabIndex = 1;
            this.labelDetailMessage.Text = "-";
            // 
            // labelDetailHeader
            // 
            this.labelDetailHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDetailHeader.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.labelDetailHeader.Location = new System.Drawing.Point(12, 12);
            this.labelDetailHeader.Margin = new System.Windows.Forms.Padding(0);
            this.labelDetailHeader.Name = "labelDetailHeader";
            this.labelDetailHeader.Size = new System.Drawing.Size(196, 36);
            this.labelDetailHeader.TabIndex = 0;
            this.labelDetailHeader.Text = "报警详情";
            // 
            // panelListCard
            // 
            this.panelListCard.Controls.Add(this.tableAlarms);
            this.panelListCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelListCard.Location = new System.Drawing.Point(0, 0);
            this.panelListCard.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.panelListCard.Name = "panelListCard";
            this.panelListCard.Padding = new System.Windows.Forms.Padding(8);
            this.panelListCard.Radius = 12;
            this.panelListCard.Shadow = 4;
            this.panelListCard.Size = new System.Drawing.Size(524, 504);
            this.panelListCard.TabIndex = 0;
            // 
            // tableAlarms
            // 
            this.tableAlarms.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tableAlarms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableAlarms.EmptyHeader = true;
            this.tableAlarms.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.tableAlarms.Gap = 8;
            this.tableAlarms.Gaps = new System.Drawing.Size(8, 8);
            this.tableAlarms.Location = new System.Drawing.Point(12, 12);
            this.tableAlarms.Margin = new System.Windows.Forms.Padding(0);
            this.tableAlarms.Name = "tableAlarms";
            this.tableAlarms.ShowTip = false;
            this.tableAlarms.Size = new System.Drawing.Size(500, 480);
            this.tableAlarms.TabIndex = 0;
            this.tableAlarms.Text = "tableAlarms";
            // 
            // panelSummary
            // 
            this.panelSummary.Controls.Add(this.flowSummaryRight);
            this.panelSummary.Controls.Add(this.labelSummaryHint);
            this.panelSummary.Controls.Add(this.labelAlarmCount);
            this.panelSummary.Controls.Add(this.labelSummaryTitle);
            this.panelSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSummary.Location = new System.Drawing.Point(8, 8);
            this.panelSummary.Margin = new System.Windows.Forms.Padding(0);
            this.panelSummary.Name = "panelSummary";
            this.panelSummary.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.panelSummary.Radius = 12;
            this.panelSummary.Shadow = 4;
            this.panelSummary.Size = new System.Drawing.Size(744, 52);
            this.panelSummary.TabIndex = 0;
            // 
            // flowSummaryRight
            // 
            this.flowSummaryRight.Controls.Add(this.buttonClearAll);
            this.flowSummaryRight.Controls.Add(this.buttonClearCurrent);
            this.flowSummaryRight.Controls.Add(this.buttonRefresh);
            this.flowSummaryRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowSummaryRight.Gap = 8;
            this.flowSummaryRight.Location = new System.Drawing.Point(450, 12);
            this.flowSummaryRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowSummaryRight.Name = "flowSummaryRight";
            this.flowSummaryRight.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.flowSummaryRight.Size = new System.Drawing.Size(282, 28);
            this.flowSummaryRight.TabIndex = 3;
            this.flowSummaryRight.Text = "flowSummaryRight";
            // 
            // buttonClearAll
            // 
            this.buttonClearAll.Location = new System.Drawing.Point(188, 0);
            this.buttonClearAll.Margin = new System.Windows.Forms.Padding(0);
            this.buttonClearAll.Name = "buttonClearAll";
            this.buttonClearAll.Radius = 8;
            this.buttonClearAll.Size = new System.Drawing.Size(94, 28);
            this.buttonClearAll.TabIndex = 0;
            this.buttonClearAll.Text = "全部清除";
            this.buttonClearAll.WaveSize = 0;
            // 
            // buttonClearCurrent
            // 
            this.buttonClearCurrent.Location = new System.Drawing.Point(94, 0);
            this.buttonClearCurrent.Margin = new System.Windows.Forms.Padding(0);
            this.buttonClearCurrent.Name = "buttonClearCurrent";
            this.buttonClearCurrent.Radius = 8;
            this.buttonClearCurrent.Size = new System.Drawing.Size(86, 28);
            this.buttonClearCurrent.TabIndex = 1;
            this.buttonClearCurrent.Text = "清除当前";
            this.buttonClearCurrent.WaveSize = 0;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(0, 0);
            this.buttonRefresh.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Radius = 8;
            this.buttonRefresh.Size = new System.Drawing.Size(86, 28);
            this.buttonRefresh.TabIndex = 2;
            this.buttonRefresh.Text = "刷新";
            this.buttonRefresh.WaveSize = 0;
            // 
            // labelSummaryHint
            // 
            this.labelSummaryHint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSummaryHint.Location = new System.Drawing.Point(138, 12);
            this.labelSummaryHint.Margin = new System.Windows.Forms.Padding(0);
            this.labelSummaryHint.Name = "labelSummaryHint";
            this.labelSummaryHint.Size = new System.Drawing.Size(594, 28);
            this.labelSummaryHint.TabIndex = 2;
            this.labelSummaryHint.Text = "显示当前未清除报警。";
            this.labelSummaryHint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelAlarmCount
            // 
            this.labelAlarmCount.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelAlarmCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold);
            this.labelAlarmCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.labelAlarmCount.Location = new System.Drawing.Point(92, 12);
            this.labelAlarmCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelAlarmCount.Name = "labelAlarmCount";
            this.labelAlarmCount.Size = new System.Drawing.Size(46, 28);
            this.labelAlarmCount.TabIndex = 1;
            this.labelAlarmCount.Text = "0";
            this.labelAlarmCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelSummaryTitle
            // 
            this.labelSummaryTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelSummaryTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelSummaryTitle.Location = new System.Drawing.Point(16, 12);
            this.labelSummaryTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelSummaryTitle.Name = "labelSummaryTitle";
            this.labelSummaryTitle.Size = new System.Drawing.Size(76, 28);
            this.labelSummaryTitle.TabIndex = 0;
            this.labelSummaryTitle.Text = "活动报警";
            this.labelSummaryTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ActiveAlarmDrawerControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ActiveAlarmDrawerControl";
            this.Size = new System.Drawing.Size(760, 620);
            this.panelRoot.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            this.flowFooterRight.ResumeLayout(false);
            this.panelContent.ResumeLayout(false);
            this.tableLayoutMain.ResumeLayout(false);
            this.panelDetailCard.ResumeLayout(false);
            this.panelDetailScroll.ResumeLayout(false);
            this.panelSuggestionCard.ResumeLayout(false);
            this.panelDescriptionCard.ResumeLayout(false);
            this.panelMetaCard.ResumeLayout(false);
            this.panelMessageCard.ResumeLayout(false);
            this.panelListCard.ResumeLayout(false);
            this.panelSummary.ResumeLayout(false);
            this.flowSummaryRight.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private AntdUI.Panel panelRoot;
        private AntdUI.Panel panelSummary;
        private AntdUI.Label labelSummaryTitle;
        private AntdUI.Label labelAlarmCount;
        private AntdUI.Label labelSummaryHint;
        private AntdUI.FlowPanel flowSummaryRight;
        private AntdUI.Button buttonRefresh;
        private AntdUI.Button buttonClearCurrent;
        private AntdUI.Button buttonClearAll;
        private AntdUI.Panel panelContent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutMain;
        private AntdUI.Panel panelListCard;
        private AntdUI.Table tableAlarms;
        private AntdUI.Panel panelDetailCard;
        private AntdUI.Label labelDetailHeader;
        private AntdUI.Panel panelDetailScroll;
        private AntdUI.Panel panelMessageCard;
        private AntdUI.Label labelMessageTitle;
        private AntdUI.Label labelDetailMessage;
        private AntdUI.Panel panelMetaCard;
        private AntdUI.Label labelMetaTitle;
        private AntdUI.Label labelDetailMeta;
        private AntdUI.Panel panelDescriptionCard;
        private AntdUI.Label labelDescriptionTitle;
        private AntdUI.Label labelDetailDescription;
        private AntdUI.Panel panelSuggestionCard;
        private AntdUI.Label labelSuggestionTitle;
        private AntdUI.Label labelDetailSuggestion;
        private AntdUI.Panel panelFooter;
        private AntdUI.Label labelPageRange;
        private AntdUI.Label labelPageInfo;
        private AntdUI.FlowPanel flowFooterRight;
        private AntdUI.Button buttonPrevPage;
        private AntdUI.Button buttonNextPage;
    }
}