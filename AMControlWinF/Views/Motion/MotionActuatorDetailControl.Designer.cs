namespace AMControlWinF.Views.Motion
{
    partial class MotionActuatorDetailControl
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
            this.tableMain = new System.Windows.Forms.TableLayoutPanel();
            this.labelTitle = new AntdUI.Label();
            this.labelSubTitle = new AntdUI.Label();
            this.tableInfo = new System.Windows.Forms.TableLayoutPanel();
            this.labelStateKey = new AntdUI.Label();
            this.labelStateValue = new AntdUI.Label();
            this.labelModeKey = new AntdUI.Label();
            this.labelModeValue = new AntdUI.Label();
            this.labelPrimaryOutputKey = new AntdUI.Label();
            this.labelPrimaryOutputValue = new AntdUI.Label();
            this.labelSecondaryOutputKey = new AntdUI.Label();
            this.labelSecondaryOutputValue = new AntdUI.Label();
            this.labelPrimaryFeedbackKey = new AntdUI.Label();
            this.labelPrimaryFeedbackValue = new AntdUI.Label();
            this.labelSecondaryFeedbackKey = new AntdUI.Label();
            this.labelSecondaryFeedbackValue = new AntdUI.Label();
            this.labelWorkpieceKey = new AntdUI.Label();
            this.labelWorkpieceValue = new AntdUI.Label();
            this.labelTimeoutKey = new AntdUI.Label();
            this.labelTimeoutValue = new AntdUI.Label();
            this.labelSummaryKey = new AntdUI.Label();
            this.labelSummaryValue = new AntdUI.Label();
            this.labelUpdateTimeKey = new AntdUI.Label();
            this.labelUpdateTimeValue = new AntdUI.Label();
            this.labelLastActionKey = new AntdUI.Label();
            this.labelLastActionValue = new AntdUI.Label();
            this.panelRoot.SuspendLayout();
            this.tableMain.SuspendLayout();
            this.tableInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.tableMain);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Margin = new System.Windows.Forms.Padding(0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(12);
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(320, 320);
            this.panelRoot.TabIndex = 0;
            // 
            // tableMain
            // 
            this.tableMain.ColumnCount = 1;
            this.tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableMain.Controls.Add(this.labelTitle, 0, 0);
            this.tableMain.Controls.Add(this.labelSubTitle, 0, 1);
            this.tableMain.Controls.Add(this.tableInfo, 0, 2);
            this.tableMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableMain.Location = new System.Drawing.Point(16, 16);
            this.tableMain.Margin = new System.Windows.Forms.Padding(0);
            this.tableMain.Name = "tableMain";
            this.tableMain.RowCount = 3;
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableMain.Size = new System.Drawing.Size(288, 288);
            this.tableMain.TabIndex = 0;
            // 
            // labelTitle
            // 
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(0, 0);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(288, 24);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "未选择执行器";
            // 
            // labelSubTitle
            // 
            this.labelSubTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSubTitle.Location = new System.Drawing.Point(0, 24);
            this.labelSubTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelSubTitle.Name = "labelSubTitle";
            this.labelSubTitle.Size = new System.Drawing.Size(288, 22);
            this.labelSubTitle.TabIndex = 1;
            this.labelSubTitle.Text = "请先在左侧选择一个执行器对象。";
            // 
            // tableInfo
            // 
            this.tableInfo.ColumnCount = 2;
            this.tableInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 78F));
            this.tableInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableInfo.Controls.Add(this.labelStateKey, 0, 0);
            this.tableInfo.Controls.Add(this.labelStateValue, 1, 0);
            this.tableInfo.Controls.Add(this.labelModeKey, 0, 1);
            this.tableInfo.Controls.Add(this.labelModeValue, 1, 1);
            this.tableInfo.Controls.Add(this.labelPrimaryOutputKey, 0, 2);
            this.tableInfo.Controls.Add(this.labelPrimaryOutputValue, 1, 2);
            this.tableInfo.Controls.Add(this.labelSecondaryOutputKey, 0, 3);
            this.tableInfo.Controls.Add(this.labelSecondaryOutputValue, 1, 3);
            this.tableInfo.Controls.Add(this.labelPrimaryFeedbackKey, 0, 4);
            this.tableInfo.Controls.Add(this.labelPrimaryFeedbackValue, 1, 4);
            this.tableInfo.Controls.Add(this.labelSecondaryFeedbackKey, 0, 5);
            this.tableInfo.Controls.Add(this.labelSecondaryFeedbackValue, 1, 5);
            this.tableInfo.Controls.Add(this.labelWorkpieceKey, 0, 6);
            this.tableInfo.Controls.Add(this.labelWorkpieceValue, 1, 6);
            this.tableInfo.Controls.Add(this.labelTimeoutKey, 0, 7);
            this.tableInfo.Controls.Add(this.labelTimeoutValue, 1, 7);
            this.tableInfo.Controls.Add(this.labelSummaryKey, 0, 8);
            this.tableInfo.Controls.Add(this.labelSummaryValue, 1, 8);
            this.tableInfo.Controls.Add(this.labelUpdateTimeKey, 0, 9);
            this.tableInfo.Controls.Add(this.labelUpdateTimeValue, 1, 9);
            this.tableInfo.Controls.Add(this.labelLastActionKey, 0, 10);
            this.tableInfo.Controls.Add(this.labelLastActionValue, 1, 10);
            this.tableInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableInfo.Location = new System.Drawing.Point(0, 46);
            this.tableInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tableInfo.Name = "tableInfo";
            this.tableInfo.RowCount = 11;
            this.tableInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableInfo.Size = new System.Drawing.Size(288, 242);
            this.tableInfo.TabIndex = 2;
            // 
            // labelStateKey
            // 
            this.labelStateKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelStateKey.Location = new System.Drawing.Point(0, 0);
            this.labelStateKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelStateKey.Name = "labelStateKey";
            this.labelStateKey.Size = new System.Drawing.Size(78, 22);
            this.labelStateKey.TabIndex = 0;
            this.labelStateKey.Text = "状态";
            // 
            // labelStateValue
            // 
            this.labelStateValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelStateValue.Location = new System.Drawing.Point(78, 0);
            this.labelStateValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelStateValue.Name = "labelStateValue";
            this.labelStateValue.Size = new System.Drawing.Size(210, 22);
            this.labelStateValue.TabIndex = 1;
            this.labelStateValue.Text = "—";
            // 
            // labelModeKey
            // 
            this.labelModeKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelModeKey.Location = new System.Drawing.Point(0, 22);
            this.labelModeKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelModeKey.Name = "labelModeKey";
            this.labelModeKey.Size = new System.Drawing.Size(78, 22);
            this.labelModeKey.TabIndex = 2;
            this.labelModeKey.Text = "模式";
            // 
            // labelModeValue
            // 
            this.labelModeValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelModeValue.Location = new System.Drawing.Point(78, 22);
            this.labelModeValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelModeValue.Name = "labelModeValue";
            this.labelModeValue.Size = new System.Drawing.Size(210, 22);
            this.labelModeValue.TabIndex = 3;
            this.labelModeValue.Text = "—";
            // 
            // labelPrimaryOutputKey
            // 
            this.labelPrimaryOutputKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPrimaryOutputKey.Location = new System.Drawing.Point(0, 44);
            this.labelPrimaryOutputKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelPrimaryOutputKey.Name = "labelPrimaryOutputKey";
            this.labelPrimaryOutputKey.Size = new System.Drawing.Size(78, 22);
            this.labelPrimaryOutputKey.TabIndex = 4;
            this.labelPrimaryOutputKey.Text = "主输出";
            // 
            // labelPrimaryOutputValue
            // 
            this.labelPrimaryOutputValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPrimaryOutputValue.Location = new System.Drawing.Point(78, 44);
            this.labelPrimaryOutputValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelPrimaryOutputValue.Name = "labelPrimaryOutputValue";
            this.labelPrimaryOutputValue.Size = new System.Drawing.Size(210, 22);
            this.labelPrimaryOutputValue.TabIndex = 5;
            this.labelPrimaryOutputValue.Text = "—";
            // 
            // labelSecondaryOutputKey
            // 
            this.labelSecondaryOutputKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSecondaryOutputKey.Location = new System.Drawing.Point(0, 66);
            this.labelSecondaryOutputKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelSecondaryOutputKey.Name = "labelSecondaryOutputKey";
            this.labelSecondaryOutputKey.Size = new System.Drawing.Size(78, 22);
            this.labelSecondaryOutputKey.TabIndex = 6;
            this.labelSecondaryOutputKey.Text = "副输出";
            // 
            // labelSecondaryOutputValue
            // 
            this.labelSecondaryOutputValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSecondaryOutputValue.Location = new System.Drawing.Point(78, 66);
            this.labelSecondaryOutputValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelSecondaryOutputValue.Name = "labelSecondaryOutputValue";
            this.labelSecondaryOutputValue.Size = new System.Drawing.Size(210, 22);
            this.labelSecondaryOutputValue.TabIndex = 7;
            this.labelSecondaryOutputValue.Text = "—";
            // 
            // labelPrimaryFeedbackKey
            // 
            this.labelPrimaryFeedbackKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPrimaryFeedbackKey.Location = new System.Drawing.Point(0, 88);
            this.labelPrimaryFeedbackKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelPrimaryFeedbackKey.Name = "labelPrimaryFeedbackKey";
            this.labelPrimaryFeedbackKey.Size = new System.Drawing.Size(78, 22);
            this.labelPrimaryFeedbackKey.TabIndex = 8;
            this.labelPrimaryFeedbackKey.Text = "主反馈";
            // 
            // labelPrimaryFeedbackValue
            // 
            this.labelPrimaryFeedbackValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPrimaryFeedbackValue.Location = new System.Drawing.Point(78, 88);
            this.labelPrimaryFeedbackValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelPrimaryFeedbackValue.Name = "labelPrimaryFeedbackValue";
            this.labelPrimaryFeedbackValue.Size = new System.Drawing.Size(210, 22);
            this.labelPrimaryFeedbackValue.TabIndex = 9;
            this.labelPrimaryFeedbackValue.Text = "—";
            // 
            // labelSecondaryFeedbackKey
            // 
            this.labelSecondaryFeedbackKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSecondaryFeedbackKey.Location = new System.Drawing.Point(0, 110);
            this.labelSecondaryFeedbackKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelSecondaryFeedbackKey.Name = "labelSecondaryFeedbackKey";
            this.labelSecondaryFeedbackKey.Size = new System.Drawing.Size(78, 22);
            this.labelSecondaryFeedbackKey.TabIndex = 10;
            this.labelSecondaryFeedbackKey.Text = "副反馈";
            // 
            // labelSecondaryFeedbackValue
            // 
            this.labelSecondaryFeedbackValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSecondaryFeedbackValue.Location = new System.Drawing.Point(78, 110);
            this.labelSecondaryFeedbackValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelSecondaryFeedbackValue.Name = "labelSecondaryFeedbackValue";
            this.labelSecondaryFeedbackValue.Size = new System.Drawing.Size(210, 22);
            this.labelSecondaryFeedbackValue.TabIndex = 11;
            this.labelSecondaryFeedbackValue.Text = "—";
            // 
            // labelWorkpieceKey
            // 
            this.labelWorkpieceKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelWorkpieceKey.Location = new System.Drawing.Point(0, 132);
            this.labelWorkpieceKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelWorkpieceKey.Name = "labelWorkpieceKey";
            this.labelWorkpieceKey.Size = new System.Drawing.Size(78, 22);
            this.labelWorkpieceKey.TabIndex = 12;
            this.labelWorkpieceKey.Text = "工件检测";
            // 
            // labelWorkpieceValue
            // 
            this.labelWorkpieceValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelWorkpieceValue.Location = new System.Drawing.Point(78, 132);
            this.labelWorkpieceValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelWorkpieceValue.Name = "labelWorkpieceValue";
            this.labelWorkpieceValue.Size = new System.Drawing.Size(210, 22);
            this.labelWorkpieceValue.TabIndex = 13;
            this.labelWorkpieceValue.Text = "—";
            // 
            // labelTimeoutKey
            // 
            this.labelTimeoutKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTimeoutKey.Location = new System.Drawing.Point(0, 154);
            this.labelTimeoutKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTimeoutKey.Name = "labelTimeoutKey";
            this.labelTimeoutKey.Size = new System.Drawing.Size(78, 22);
            this.labelTimeoutKey.TabIndex = 14;
            this.labelTimeoutKey.Text = "超时配置";
            // 
            // labelTimeoutValue
            // 
            this.labelTimeoutValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTimeoutValue.Location = new System.Drawing.Point(78, 154);
            this.labelTimeoutValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTimeoutValue.Name = "labelTimeoutValue";
            this.labelTimeoutValue.Size = new System.Drawing.Size(210, 22);
            this.labelTimeoutValue.TabIndex = 15;
            this.labelTimeoutValue.Text = "—";
            // 
            // labelSummaryKey
            // 
            this.labelSummaryKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSummaryKey.Location = new System.Drawing.Point(0, 176);
            this.labelSummaryKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelSummaryKey.Name = "labelSummaryKey";
            this.labelSummaryKey.Size = new System.Drawing.Size(78, 22);
            this.labelSummaryKey.TabIndex = 16;
            this.labelSummaryKey.Text = "运行摘要";
            // 
            // labelSummaryValue
            // 
            this.labelSummaryValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSummaryValue.Location = new System.Drawing.Point(78, 176);
            this.labelSummaryValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelSummaryValue.Name = "labelSummaryValue";
            this.labelSummaryValue.Size = new System.Drawing.Size(210, 22);
            this.labelSummaryValue.TabIndex = 17;
            this.labelSummaryValue.Text = "—";
            // 
            // labelUpdateTimeKey
            // 
            this.labelUpdateTimeKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelUpdateTimeKey.Location = new System.Drawing.Point(0, 198);
            this.labelUpdateTimeKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelUpdateTimeKey.Name = "labelUpdateTimeKey";
            this.labelUpdateTimeKey.Size = new System.Drawing.Size(78, 22);
            this.labelUpdateTimeKey.TabIndex = 18;
            this.labelUpdateTimeKey.Text = "更新时间";
            // 
            // labelUpdateTimeValue
            // 
            this.labelUpdateTimeValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelUpdateTimeValue.Location = new System.Drawing.Point(78, 198);
            this.labelUpdateTimeValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelUpdateTimeValue.Name = "labelUpdateTimeValue";
            this.labelUpdateTimeValue.Size = new System.Drawing.Size(210, 22);
            this.labelUpdateTimeValue.TabIndex = 19;
            this.labelUpdateTimeValue.Text = "—";
            // 
            // labelLastActionKey
            // 
            this.labelLastActionKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLastActionKey.Location = new System.Drawing.Point(0, 220);
            this.labelLastActionKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelLastActionKey.Name = "labelLastActionKey";
            this.labelLastActionKey.Size = new System.Drawing.Size(78, 22);
            this.labelLastActionKey.TabIndex = 20;
            this.labelLastActionKey.Text = "最近操作";
            // 
            // labelLastActionValue
            // 
            this.labelLastActionValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLastActionValue.Location = new System.Drawing.Point(78, 220);
            this.labelLastActionValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelLastActionValue.Name = "labelLastActionValue";
            this.labelLastActionValue.Size = new System.Drawing.Size(210, 22);
            this.labelLastActionValue.TabIndex = 21;
            this.labelLastActionValue.Text = "—";
            // 
            // MotionActuatorDetailControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MotionActuatorDetailControl";
            this.Size = new System.Drawing.Size(320, 320);
            this.panelRoot.ResumeLayout(false);
            this.tableMain.ResumeLayout(false);
            this.tableInfo.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private AntdUI.Panel panelRoot;
        private System.Windows.Forms.TableLayoutPanel tableMain;
        private AntdUI.Label labelTitle;
        private AntdUI.Label labelSubTitle;
        private System.Windows.Forms.TableLayoutPanel tableInfo;
        private AntdUI.Label labelStateKey;
        private AntdUI.Label labelStateValue;
        private AntdUI.Label labelModeKey;
        private AntdUI.Label labelModeValue;
        private AntdUI.Label labelPrimaryOutputKey;
        private AntdUI.Label labelPrimaryOutputValue;
        private AntdUI.Label labelSecondaryOutputKey;
        private AntdUI.Label labelSecondaryOutputValue;
        private AntdUI.Label labelPrimaryFeedbackKey;
        private AntdUI.Label labelPrimaryFeedbackValue;
        private AntdUI.Label labelSecondaryFeedbackKey;
        private AntdUI.Label labelSecondaryFeedbackValue;
        private AntdUI.Label labelWorkpieceKey;
        private AntdUI.Label labelWorkpieceValue;
        private AntdUI.Label labelTimeoutKey;
        private AntdUI.Label labelTimeoutValue;
        private AntdUI.Label labelSummaryKey;
        private AntdUI.Label labelSummaryValue;
        private AntdUI.Label labelUpdateTimeKey;
        private AntdUI.Label labelUpdateTimeValue;
        private AntdUI.Label labelLastActionKey;
        private AntdUI.Label labelLastActionValue;
    }
}