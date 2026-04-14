// AMControlWinF\Views\Main\ActiveAlarmDrawerControl.Designer.cs
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
            this.panelContentCard = new AntdUI.Panel();
            this.tableLayoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.panelListCard = new AntdUI.Panel();
            this.tableAlarms = new AntdUI.Table();
            this.panelListHeader = new AntdUI.Panel();
            this.flowListHeaderLeft = new AntdUI.FlowPanel();
            this.labelListSummary = new AntdUI.Label();
            this.labelListTitle = new AntdUI.Label();
            this.panelDetailCard = new AntdUI.Panel();
            this.panelDetailBody = new AntdUI.Panel();
            this.labelDetailSuggestion = new AntdUI.Label();
            this.labelDetailSuggestionTitle = new AntdUI.Label();
            this.labelDetailDescription = new AntdUI.Label();
            this.labelDetailDescriptionTitle = new AntdUI.Label();
            this.labelDetailMeta = new AntdUI.Label();
            this.labelDetailMetaTitle = new AntdUI.Label();
            this.labelDetailMessage = new AntdUI.Label();
            this.labelDetailHeader = new AntdUI.Label();
            this.panelFooter = new AntdUI.Panel();
            this.paginationAlarms = new AntdUI.Pagination();
            this.panelToolbar = new AntdUI.Panel();
            this.flowToolbarRight = new AntdUI.FlowPanel();
            this.buttonClearAll = new AntdUI.Button();
            this.buttonClearCurrent = new AntdUI.Button();
            this.buttonRefresh = new AntdUI.Button();
            this.flowToolbarLeft = new AntdUI.FlowPanel();
            this.labelSummaryHint = new AntdUI.Label();
            this.labelAlarmCount = new AntdUI.Label();
            this.labelSummaryTitle = new AntdUI.Label();
            this.panelRoot.SuspendLayout();
            this.panelContentCard.SuspendLayout();
            this.tableLayoutMain.SuspendLayout();
            this.panelListCard.SuspendLayout();
            this.panelListHeader.SuspendLayout();
            this.flowListHeaderLeft.SuspendLayout();
            this.panelDetailCard.SuspendLayout();
            this.panelDetailBody.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.panelToolbar.SuspendLayout();
            this.flowToolbarRight.SuspendLayout();
            this.flowToolbarLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.panelContentCard);
            this.panelRoot.Controls.Add(this.panelFooter);
            this.panelRoot.Controls.Add(this.panelToolbar);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(8);
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(760, 620);
            this.panelRoot.TabIndex = 0;
            // 
            // panelContentCard
            // 
            this.panelContentCard.BackColor = System.Drawing.Color.Transparent;
            this.panelContentCard.Controls.Add(this.tableLayoutMain);
            this.panelContentCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContentCard.Location = new System.Drawing.Point(8, 52);
            this.panelContentCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelContentCard.Name = "panelContentCard";
            this.panelContentCard.Radius = 0;
            this.panelContentCard.ShadowOpacity = 0F;
            this.panelContentCard.ShadowOpacityHover = 0F;
            this.panelContentCard.Size = new System.Drawing.Size(744, 516);
            this.panelContentCard.TabIndex = 1;
            // 
            // tableLayoutMain
            // 
            this.tableLayoutMain.ColumnCount = 2;
            this.tableLayoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tableLayoutMain.Controls.Add(this.panelListCard, 0, 0);
            this.tableLayoutMain.Controls.Add(this.panelDetailCard, 1, 0);
            this.tableLayoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutMain.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutMain.Name = "tableLayoutMain";
            this.tableLayoutMain.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.tableLayoutMain.RowCount = 1;
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutMain.Size = new System.Drawing.Size(744, 516);
            this.tableLayoutMain.TabIndex = 0;
            // 
            // panelListCard
            // 
            this.panelListCard.BackColor = System.Drawing.Color.Transparent;
            this.panelListCard.Controls.Add(this.tableAlarms);
            this.panelListCard.Controls.Add(this.panelListHeader);
            this.panelListCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelListCard.Location = new System.Drawing.Point(0, 10);
            this.panelListCard.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.panelListCard.Name = "panelListCard";
            this.panelListCard.Padding = new System.Windows.Forms.Padding(8);
            this.panelListCard.Radius = 12;
            this.panelListCard.Shadow = 4;
            this.panelListCard.Size = new System.Drawing.Size(486, 506);
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
            this.tableAlarms.Location = new System.Drawing.Point(12, 56);
            this.tableAlarms.Margin = new System.Windows.Forms.Padding(0);
            this.tableAlarms.Name = "tableAlarms";
            this.tableAlarms.ShowTip = false;
            this.tableAlarms.Size = new System.Drawing.Size(462, 438);
            this.tableAlarms.TabIndex = 1;
            this.tableAlarms.Text = "tableAlarms";
            // 
            // panelListHeader
            // 
            this.panelListHeader.Controls.Add(this.flowListHeaderLeft);
            this.panelListHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelListHeader.Location = new System.Drawing.Point(12, 12);
            this.panelListHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelListHeader.Name = "panelListHeader";
            this.panelListHeader.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.panelListHeader.Radius = 0;
            this.panelListHeader.Size = new System.Drawing.Size(462, 44);
            this.panelListHeader.TabIndex = 0;
            // 
            // flowListHeaderLeft
            // 
            this.flowListHeaderLeft.Controls.Add(this.labelListSummary);
            this.flowListHeaderLeft.Controls.Add(this.labelListTitle);
            this.flowListHeaderLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowListHeaderLeft.Gap = 8;
            this.flowListHeaderLeft.Location = new System.Drawing.Point(0, 0);
            this.flowListHeaderLeft.Margin = new System.Windows.Forms.Padding(0);
            this.flowListHeaderLeft.Name = "flowListHeaderLeft";
            this.flowListHeaderLeft.Size = new System.Drawing.Size(320, 36);
            this.flowListHeaderLeft.TabIndex = 0;
            this.flowListHeaderLeft.Text = "flowListHeaderLeft";
            // 
            // labelListSummary
            // 
            this.labelListSummary.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelListSummary.Location = new System.Drawing.Point(72, 0);
            this.labelListSummary.Margin = new System.Windows.Forms.Padding(0);
            this.labelListSummary.Name = "labelListSummary";
            this.labelListSummary.Size = new System.Drawing.Size(248, 36);
            this.labelListSummary.TabIndex = 1;
            this.labelListSummary.Text = "当前活动报警列表";
            // 
            // labelListTitle
            // 
            this.labelListTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.labelListTitle.Location = new System.Drawing.Point(0, 0);
            this.labelListTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelListTitle.Name = "labelListTitle";
            this.labelListTitle.Size = new System.Drawing.Size(64, 36);
            this.labelListTitle.TabIndex = 0;
            this.labelListTitle.Text = "报警列表";
            // 
            // panelDetailCard
            // 
            this.panelDetailCard.BackColor = System.Drawing.Color.Transparent;
            this.panelDetailCard.Controls.Add(this.panelDetailBody);
            this.panelDetailCard.Controls.Add(this.labelDetailHeader);
            this.panelDetailCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetailCard.Location = new System.Drawing.Point(494, 10);
            this.panelDetailCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelDetailCard.Name = "panelDetailCard";
            this.panelDetailCard.Padding = new System.Windows.Forms.Padding(8);
            this.panelDetailCard.Radius = 12;
            this.panelDetailCard.Shadow = 4;
            this.panelDetailCard.Size = new System.Drawing.Size(250, 506);
            this.panelDetailCard.TabIndex = 1;
            // 
            // panelDetailBody
            // 
            this.panelDetailBody.Controls.Add(this.labelDetailSuggestion);
            this.panelDetailBody.Controls.Add(this.labelDetailSuggestionTitle);
            this.panelDetailBody.Controls.Add(this.labelDetailDescription);
            this.panelDetailBody.Controls.Add(this.labelDetailDescriptionTitle);
            this.panelDetailBody.Controls.Add(this.labelDetailMeta);
            this.panelDetailBody.Controls.Add(this.labelDetailMetaTitle);
            this.panelDetailBody.Controls.Add(this.labelDetailMessage);
            this.panelDetailBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetailBody.Location = new System.Drawing.Point(12, 48);
            this.panelDetailBody.Margin = new System.Windows.Forms.Padding(0);
            this.panelDetailBody.Name = "panelDetailBody";
            this.panelDetailBody.Padding = new System.Windows.Forms.Padding(4);
            this.panelDetailBody.Radius = 0;
            this.panelDetailBody.ShadowOpacity = 0F;
            this.panelDetailBody.ShadowOpacityHover = 0F;
            this.panelDetailBody.Size = new System.Drawing.Size(226, 446);
            this.panelDetailBody.TabIndex = 1;
            // 
            // labelDetailSuggestion
            // 
            this.labelDetailSuggestion.Location = new System.Drawing.Point(8, 308);
            this.labelDetailSuggestion.Margin = new System.Windows.Forms.Padding(0);
            this.labelDetailSuggestion.Name = "labelDetailSuggestion";
            this.labelDetailSuggestion.Size = new System.Drawing.Size(206, 92);
            this.labelDetailSuggestion.TabIndex = 6;
            this.labelDetailSuggestion.Text = "-";
            // 
            // labelDetailSuggestionTitle
            // 
            this.labelDetailSuggestionTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelDetailSuggestionTitle.Location = new System.Drawing.Point(8, 282);
            this.labelDetailSuggestionTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelDetailSuggestionTitle.Name = "labelDetailSuggestionTitle";
            this.labelDetailSuggestionTitle.Size = new System.Drawing.Size(206, 22);
            this.labelDetailSuggestionTitle.TabIndex = 5;
            this.labelDetailSuggestionTitle.Text = "处理建议";
            // 
            // labelDetailDescription
            // 
            this.labelDetailDescription.Location = new System.Drawing.Point(8, 190);
            this.labelDetailDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDetailDescription.Name = "labelDetailDescription";
            this.labelDetailDescription.Size = new System.Drawing.Size(206, 84);
            this.labelDetailDescription.TabIndex = 4;
            this.labelDetailDescription.Text = "-";
            // 
            // labelDetailDescriptionTitle
            // 
            this.labelDetailDescriptionTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelDetailDescriptionTitle.Location = new System.Drawing.Point(8, 164);
            this.labelDetailDescriptionTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelDetailDescriptionTitle.Name = "labelDetailDescriptionTitle";
            this.labelDetailDescriptionTitle.Size = new System.Drawing.Size(206, 22);
            this.labelDetailDescriptionTitle.TabIndex = 3;
            this.labelDetailDescriptionTitle.Text = "详细说明";
            // 
            // labelDetailMeta
            // 
            this.labelDetailMeta.Location = new System.Drawing.Point(8, 72);
            this.labelDetailMeta.Margin = new System.Windows.Forms.Padding(0);
            this.labelDetailMeta.Name = "labelDetailMeta";
            this.labelDetailMeta.Size = new System.Drawing.Size(206, 88);
            this.labelDetailMeta.TabIndex = 2;
            this.labelDetailMeta.Text = "报警代码：-\r\n报警等级：-\r\n报警来源：-\r\n控制卡号：-\r\n报警时间：-";
            // 
            // labelDetailMetaTitle
            // 
            this.labelDetailMetaTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelDetailMetaTitle.Location = new System.Drawing.Point(8, 46);
            this.labelDetailMetaTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelDetailMetaTitle.Name = "labelDetailMetaTitle";
            this.labelDetailMetaTitle.Size = new System.Drawing.Size(206, 22);
            this.labelDetailMetaTitle.TabIndex = 1;
            this.labelDetailMetaTitle.Text = "报警详情";
            // 
            // labelDetailMessage
            // 
            this.labelDetailMessage.Location = new System.Drawing.Point(8, 8);
            this.labelDetailMessage.Margin = new System.Windows.Forms.Padding(0);
            this.labelDetailMessage.Name = "labelDetailMessage";
            this.labelDetailMessage.Size = new System.Drawing.Size(206, 34);
            this.labelDetailMessage.TabIndex = 0;
            this.labelDetailMessage.Text = "-";
            // 
            // labelDetailHeader
            // 
            this.labelDetailHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDetailHeader.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.labelDetailHeader.Location = new System.Drawing.Point(12, 12);
            this.labelDetailHeader.Margin = new System.Windows.Forms.Padding(0);
            this.labelDetailHeader.Name = "labelDetailHeader";
            this.labelDetailHeader.Size = new System.Drawing.Size(226, 36);
            this.labelDetailHeader.TabIndex = 0;
            this.labelDetailHeader.Text = "报警详情";
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.paginationAlarms);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(8, 568);
            this.panelFooter.Margin = new System.Windows.Forms.Padding(0);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.panelFooter.Radius = 0;
            this.panelFooter.Size = new System.Drawing.Size(744, 44);
            this.panelFooter.TabIndex = 2;
            // 
            // paginationAlarms
            // 
            this.paginationAlarms.BackColor = System.Drawing.Color.Transparent;
            this.paginationAlarms.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.paginationAlarms.Location = new System.Drawing.Point(0, 2);
            this.paginationAlarms.Margin = new System.Windows.Forms.Padding(0);
            this.paginationAlarms.Name = "paginationAlarms";
            this.paginationAlarms.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.paginationAlarms.Size = new System.Drawing.Size(744, 42);
            this.paginationAlarms.SizeChangerWidth = 72;
            this.paginationAlarms.TabIndex = 0;
            this.paginationAlarms.Text = "paginationAlarms";
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
            this.panelToolbar.Size = new System.Drawing.Size(744, 44);
            this.panelToolbar.TabIndex = 0;
            // 
            // flowToolbarRight
            // 
            this.flowToolbarRight.Controls.Add(this.buttonClearAll);
            this.flowToolbarRight.Controls.Add(this.buttonClearCurrent);
            this.flowToolbarRight.Controls.Add(this.buttonRefresh);
            this.flowToolbarRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowToolbarRight.Gap = 8;
            this.flowToolbarRight.Location = new System.Drawing.Point(454, 4);
            this.flowToolbarRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowToolbarRight.Name = "flowToolbarRight";
            this.flowToolbarRight.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.flowToolbarRight.Size = new System.Drawing.Size(286, 36);
            this.flowToolbarRight.TabIndex = 1;
            this.flowToolbarRight.Text = "flowToolbarRight";
            // 
            // buttonClearAll
            // 
            this.buttonClearAll.Location = new System.Drawing.Point(192, 0);
            this.buttonClearAll.Margin = new System.Windows.Forms.Padding(0);
            this.buttonClearAll.Name = "buttonClearAll";
            this.buttonClearAll.Radius = 8;
            this.buttonClearAll.Size = new System.Drawing.Size(94, 36);
            this.buttonClearAll.TabIndex = 0;
            this.buttonClearAll.Text = "全部清除";
            this.buttonClearAll.Type = AntdUI.TTypeMini.Primary;
            this.buttonClearAll.WaveSize = 0;
            // 
            // buttonClearCurrent
            // 
            this.buttonClearCurrent.Location = new System.Drawing.Point(98, 0);
            this.buttonClearCurrent.Margin = new System.Windows.Forms.Padding(0);
            this.buttonClearCurrent.Name = "buttonClearCurrent";
            this.buttonClearCurrent.Radius = 8;
            this.buttonClearCurrent.Size = new System.Drawing.Size(86, 36);
            this.buttonClearCurrent.TabIndex = 1;
            this.buttonClearCurrent.Text = "清除当前";
            this.buttonClearCurrent.Type = AntdUI.TTypeMini.Primary;
            this.buttonClearCurrent.WaveSize = 0;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(0, 0);
            this.buttonRefresh.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Radius = 8;
            this.buttonRefresh.Size = new System.Drawing.Size(90, 36);
            this.buttonRefresh.TabIndex = 2;
            this.buttonRefresh.Text = "刷新";
            this.buttonRefresh.WaveSize = 0;
            // 
            // flowToolbarLeft
            // 
            this.flowToolbarLeft.Controls.Add(this.labelSummaryHint);
            this.flowToolbarLeft.Controls.Add(this.labelAlarmCount);
            this.flowToolbarLeft.Controls.Add(this.labelSummaryTitle);
            this.flowToolbarLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowToolbarLeft.Gap = 8;
            this.flowToolbarLeft.Location = new System.Drawing.Point(4, 4);
            this.flowToolbarLeft.Margin = new System.Windows.Forms.Padding(0);
            this.flowToolbarLeft.Name = "flowToolbarLeft";
            this.flowToolbarLeft.Size = new System.Drawing.Size(375, 36);
            this.flowToolbarLeft.TabIndex = 0;
            this.flowToolbarLeft.Text = "flowToolbarLeft";
            // 
            // labelSummaryHint
            // 
            this.labelSummaryHint.Location = new System.Drawing.Point(134, 0);
            this.labelSummaryHint.Margin = new System.Windows.Forms.Padding(0);
            this.labelSummaryHint.Name = "labelSummaryHint";
            this.labelSummaryHint.Size = new System.Drawing.Size(228, 36);
            this.labelSummaryHint.TabIndex = 2;
            this.labelSummaryHint.Text = "显示当前未清除报警。";
            // 
            // labelAlarmCount
            // 
            this.labelAlarmCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold);
            this.labelAlarmCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.labelAlarmCount.Location = new System.Drawing.Point(82, 0);
            this.labelAlarmCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelAlarmCount.Name = "labelAlarmCount";
            this.labelAlarmCount.Size = new System.Drawing.Size(44, 36);
            this.labelAlarmCount.TabIndex = 1;
            this.labelAlarmCount.Text = "0";
            // 
            // labelSummaryTitle
            // 
            this.labelSummaryTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelSummaryTitle.Location = new System.Drawing.Point(0, 0);
            this.labelSummaryTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelSummaryTitle.Name = "labelSummaryTitle";
            this.labelSummaryTitle.Size = new System.Drawing.Size(74, 36);
            this.labelSummaryTitle.TabIndex = 0;
            this.labelSummaryTitle.Text = "活动报警";
            // 
            // ActiveAlarmDrawerControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ActiveAlarmDrawerControl";
            this.Size = new System.Drawing.Size(760, 620);
            this.panelRoot.ResumeLayout(false);
            this.panelContentCard.ResumeLayout(false);
            this.tableLayoutMain.ResumeLayout(false);
            this.panelListCard.ResumeLayout(false);
            this.panelListHeader.ResumeLayout(false);
            this.flowListHeaderLeft.ResumeLayout(false);
            this.panelDetailCard.ResumeLayout(false);
            this.panelDetailBody.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            this.panelToolbar.ResumeLayout(false);
            this.flowToolbarRight.ResumeLayout(false);
            this.flowToolbarLeft.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.Panel panelRoot;
        private AntdUI.Panel panelToolbar;
        private AntdUI.FlowPanel flowToolbarLeft;
        private AntdUI.Label labelSummaryTitle;
        private AntdUI.Label labelAlarmCount;
        private AntdUI.Label labelSummaryHint;
        private AntdUI.FlowPanel flowToolbarRight;
        private AntdUI.Button buttonRefresh;
        private AntdUI.Button buttonClearCurrent;
        private AntdUI.Button buttonClearAll;
        private AntdUI.Panel panelContentCard;
        private System.Windows.Forms.TableLayoutPanel tableLayoutMain;
        private AntdUI.Panel panelListCard;
        private AntdUI.Panel panelListHeader;
        private AntdUI.FlowPanel flowListHeaderLeft;
        private AntdUI.Label labelListTitle;
        private AntdUI.Label labelListSummary;
        private AntdUI.Table tableAlarms;
        private AntdUI.Panel panelDetailCard;
        private AntdUI.Label labelDetailHeader;
        private AntdUI.Panel panelDetailBody;
        private AntdUI.Label labelDetailMessage;
        private AntdUI.Label labelDetailMetaTitle;
        private AntdUI.Label labelDetailMeta;
        private AntdUI.Label labelDetailDescriptionTitle;
        private AntdUI.Label labelDetailDescription;
        private AntdUI.Label labelDetailSuggestionTitle;
        private AntdUI.Label labelDetailSuggestion;
        private AntdUI.Panel panelFooter;
        private AntdUI.Pagination paginationAlarms;
    }
}