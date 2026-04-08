namespace AMControlWinF.Views.Motion
{
    partial class MotionAxisDetailControl
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
            this.panelDetail = new AntdUI.Panel();
            this.panelScroll = new System.Windows.Forms.Panel();
            this.labelDescriptionText = new AntdUI.Label();
            this.labelDescriptionTitle = new AntdUI.Label();
            this.panelTagJogVel = new AntdUI.Panel();
            this.labelTagJogVelValue = new AntdUI.Label();
            this.labelTagJogVelKey = new AntdUI.Label();
            this.panelTagDefaultVel = new AntdUI.Panel();
            this.labelTagDefaultVelValue = new AntdUI.Label();
            this.labelTagDefaultVelKey = new AntdUI.Label();
            this.panelTagErrorMm = new AntdUI.Panel();
            this.labelTagErrorMmValue = new AntdUI.Label();
            this.labelTagErrorMmKey = new AntdUI.Label();
            this.panelTagEncMm = new AntdUI.Panel();
            this.labelTagEncMmValue = new AntdUI.Label();
            this.labelTagEncMmKey = new AntdUI.Label();
            this.panelTagCmdMm = new AntdUI.Panel();
            this.labelTagCmdMmValue = new AntdUI.Label();
            this.labelTagCmdMmKey = new AntdUI.Label();
            this.panelTagDone = new AntdUI.Panel();
            this.labelTagDoneValue = new AntdUI.Label();
            this.labelTagDoneKey = new AntdUI.Label();
            this.panelTagHome = new AntdUI.Panel();
            this.labelTagHomeValue = new AntdUI.Label();
            this.labelTagHomeKey = new AntdUI.Label();
            this.panelTagEnable = new AntdUI.Panel();
            this.labelTagEnableValue = new AntdUI.Label();
            this.labelTagEnableKey = new AntdUI.Label();
            this.panelTagLimit = new AntdUI.Panel();
            this.labelTagLimitValue = new AntdUI.Label();
            this.labelTagLimitKey = new AntdUI.Label();
            this.panelTagInterlock = new AntdUI.Panel();
            this.labelTagInterlockValue = new AntdUI.Label();
            this.labelTagInterlockKey = new AntdUI.Label();
            this.panelTagState = new AntdUI.Panel();
            this.labelTagStateValue = new AntdUI.Label();
            this.labelTagStateKey = new AntdUI.Label();
            this.panelTagUpdate = new AntdUI.Panel();
            this.labelTagUpdateValue = new AntdUI.Label();
            this.labelTagUpdateKey = new AntdUI.Label();
            this.panelTagScan = new AntdUI.Panel();
            this.labelTagScanValue = new AntdUI.Label();
            this.labelTagScanKey = new AntdUI.Label();
            this.panelTagCard = new AntdUI.Panel();
            this.labelTagCardValue = new AntdUI.Label();
            this.labelTagCardKey = new AntdUI.Label();
            this.panelHeader = new AntdUI.Panel();
            this.labelSubTitle = new AntdUI.Label();
            this.labelTitle = new AntdUI.Label();
            this.panelEmpty = new AntdUI.Panel();
            this.labelEmpty = new AntdUI.Label();
            this.panelRoot.SuspendLayout();
            this.panelDetail.SuspendLayout();
            this.panelScroll.SuspendLayout();
            this.panelTagJogVel.SuspendLayout();
            this.panelTagDefaultVel.SuspendLayout();
            this.panelTagErrorMm.SuspendLayout();
            this.panelTagEncMm.SuspendLayout();
            this.panelTagCmdMm.SuspendLayout();
            this.panelTagDone.SuspendLayout();
            this.panelTagHome.SuspendLayout();
            this.panelTagEnable.SuspendLayout();
            this.panelTagLimit.SuspendLayout();
            this.panelTagInterlock.SuspendLayout();
            this.panelTagState.SuspendLayout();
            this.panelTagUpdate.SuspendLayout();
            this.panelTagScan.SuspendLayout();
            this.panelTagCard.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.panelEmpty.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.panelDetail);
            this.panelRoot.Controls.Add(this.panelEmpty);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Margin = new System.Windows.Forms.Padding(0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(272, 608);
            this.panelRoot.TabIndex = 0;
            // 
            // panelDetail
            // 
            this.panelDetail.Controls.Add(this.panelScroll);
            this.panelDetail.Controls.Add(this.panelHeader);
            this.panelDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetail.Location = new System.Drawing.Point(0, 0);
            this.panelDetail.Margin = new System.Windows.Forms.Padding(0);
            this.panelDetail.Name = "panelDetail";
            this.panelDetail.Radius = 0;
            this.panelDetail.Size = new System.Drawing.Size(272, 608);
            this.panelDetail.TabIndex = 1;
            this.panelDetail.Visible = false;
            // 
            // panelScroll
            // 
            this.panelScroll.Controls.Add(this.labelDescriptionText);
            this.panelScroll.Controls.Add(this.labelDescriptionTitle);
            this.panelScroll.Controls.Add(this.panelTagJogVel);
            this.panelScroll.Controls.Add(this.panelTagDefaultVel);
            this.panelScroll.Controls.Add(this.panelTagErrorMm);
            this.panelScroll.Controls.Add(this.panelTagEncMm);
            this.panelScroll.Controls.Add(this.panelTagCmdMm);
            this.panelScroll.Controls.Add(this.panelTagDone);
            this.panelScroll.Controls.Add(this.panelTagHome);
            this.panelScroll.Controls.Add(this.panelTagEnable);
            this.panelScroll.Controls.Add(this.panelTagLimit);
            this.panelScroll.Controls.Add(this.panelTagInterlock);
            this.panelScroll.Controls.Add(this.panelTagState);
            this.panelScroll.Controls.Add(this.panelTagUpdate);
            this.panelScroll.Controls.Add(this.panelTagScan);
            this.panelScroll.Controls.Add(this.panelTagCard);
            this.panelScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelScroll.Location = new System.Drawing.Point(0, 60);
            this.panelScroll.Margin = new System.Windows.Forms.Padding(0);
            this.panelScroll.Name = "panelScroll";
            this.panelScroll.Padding = new System.Windows.Forms.Padding(14, 12, 14, 12);
            this.panelScroll.Size = new System.Drawing.Size(272, 548);
            this.panelScroll.TabIndex = 1;
            // 
            // labelDescriptionText
            // 
            this.labelDescriptionText.Location = new System.Drawing.Point(8, 446);
            this.labelDescriptionText.Margin = new System.Windows.Forms.Padding(0);
            this.labelDescriptionText.Name = "labelDescriptionText";
            this.labelDescriptionText.Size = new System.Drawing.Size(252, 44);
            this.labelDescriptionText.TabIndex = 15;
            this.labelDescriptionText.Text = "当前页面用于单轴手动控制与运行状态确认。";
            // 
            // labelDescriptionTitle
            // 
            this.labelDescriptionTitle.ForeColor = System.Drawing.Color.Gray;
            this.labelDescriptionTitle.Location = new System.Drawing.Point(8, 420);
            this.labelDescriptionTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelDescriptionTitle.Name = "labelDescriptionTitle";
            this.labelDescriptionTitle.Size = new System.Drawing.Size(252, 22);
            this.labelDescriptionTitle.TabIndex = 14;
            this.labelDescriptionTitle.Text = "说明";
            // 
            // panelTagJogVel
            // 
            this.panelTagJogVel.Controls.Add(this.labelTagJogVelValue);
            this.panelTagJogVel.Controls.Add(this.labelTagJogVelKey);
            this.panelTagJogVel.Location = new System.Drawing.Point(8, 392);
            this.panelTagJogVel.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagJogVel.Name = "panelTagJogVel";
            this.panelTagJogVel.Radius = 0;
            this.panelTagJogVel.Size = new System.Drawing.Size(252, 24);
            this.panelTagJogVel.TabIndex = 13;
            // 
            // labelTagJogVelValue
            // 
            this.labelTagJogVelValue.AutoEllipsis = true;
            this.labelTagJogVelValue.Location = new System.Drawing.Point(108, 0);
            this.labelTagJogVelValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagJogVelValue.Name = "labelTagJogVelValue";
            this.labelTagJogVelValue.Size = new System.Drawing.Size(144, 22);
            this.labelTagJogVelValue.TabIndex = 1;
            this.labelTagJogVelValue.Text = "—";
            // 
            // labelTagJogVelKey
            // 
            this.labelTagJogVelKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagJogVelKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagJogVelKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagJogVelKey.Name = "labelTagJogVelKey";
            this.labelTagJogVelKey.Size = new System.Drawing.Size(102, 22);
            this.labelTagJogVelKey.TabIndex = 0;
            this.labelTagJogVelKey.Text = "Jog速度(mm/s)";
            this.labelTagJogVelKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagDefaultVel
            // 
            this.panelTagDefaultVel.Controls.Add(this.labelTagDefaultVelValue);
            this.panelTagDefaultVel.Controls.Add(this.labelTagDefaultVelKey);
            this.panelTagDefaultVel.Location = new System.Drawing.Point(8, 368);
            this.panelTagDefaultVel.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagDefaultVel.Name = "panelTagDefaultVel";
            this.panelTagDefaultVel.Radius = 0;
            this.panelTagDefaultVel.Size = new System.Drawing.Size(252, 24);
            this.panelTagDefaultVel.TabIndex = 12;
            // 
            // labelTagDefaultVelValue
            // 
            this.labelTagDefaultVelValue.AutoEllipsis = true;
            this.labelTagDefaultVelValue.Location = new System.Drawing.Point(108, 0);
            this.labelTagDefaultVelValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagDefaultVelValue.Name = "labelTagDefaultVelValue";
            this.labelTagDefaultVelValue.Size = new System.Drawing.Size(144, 22);
            this.labelTagDefaultVelValue.TabIndex = 1;
            this.labelTagDefaultVelValue.Text = "—";
            // 
            // labelTagDefaultVelKey
            // 
            this.labelTagDefaultVelKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagDefaultVelKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagDefaultVelKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagDefaultVelKey.Name = "labelTagDefaultVelKey";
            this.labelTagDefaultVelKey.Size = new System.Drawing.Size(102, 22);
            this.labelTagDefaultVelKey.TabIndex = 0;
            this.labelTagDefaultVelKey.Text = "默认速度(mm/s)";
            this.labelTagDefaultVelKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagErrorMm
            // 
            this.panelTagErrorMm.Controls.Add(this.labelTagErrorMmValue);
            this.panelTagErrorMm.Controls.Add(this.labelTagErrorMmKey);
            this.panelTagErrorMm.Location = new System.Drawing.Point(8, 344);
            this.panelTagErrorMm.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagErrorMm.Name = "panelTagErrorMm";
            this.panelTagErrorMm.Radius = 0;
            this.panelTagErrorMm.Size = new System.Drawing.Size(252, 24);
            this.panelTagErrorMm.TabIndex = 11;
            // 
            // labelTagErrorMmValue
            // 
            this.labelTagErrorMmValue.AutoEllipsis = true;
            this.labelTagErrorMmValue.Location = new System.Drawing.Point(108, 0);
            this.labelTagErrorMmValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagErrorMmValue.Name = "labelTagErrorMmValue";
            this.labelTagErrorMmValue.Size = new System.Drawing.Size(144, 22);
            this.labelTagErrorMmValue.TabIndex = 1;
            this.labelTagErrorMmValue.Text = "—";
            // 
            // labelTagErrorMmKey
            // 
            this.labelTagErrorMmKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagErrorMmKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagErrorMmKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagErrorMmKey.Name = "labelTagErrorMmKey";
            this.labelTagErrorMmKey.Size = new System.Drawing.Size(102, 22);
            this.labelTagErrorMmKey.TabIndex = 0;
            this.labelTagErrorMmKey.Text = "位置误差(mm)";
            this.labelTagErrorMmKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagEncMm
            // 
            this.panelTagEncMm.Controls.Add(this.labelTagEncMmValue);
            this.panelTagEncMm.Controls.Add(this.labelTagEncMmKey);
            this.panelTagEncMm.Location = new System.Drawing.Point(8, 320);
            this.panelTagEncMm.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagEncMm.Name = "panelTagEncMm";
            this.panelTagEncMm.Radius = 0;
            this.panelTagEncMm.Size = new System.Drawing.Size(252, 24);
            this.panelTagEncMm.TabIndex = 10;
            // 
            // labelTagEncMmValue
            // 
            this.labelTagEncMmValue.AutoEllipsis = true;
            this.labelTagEncMmValue.Location = new System.Drawing.Point(108, 0);
            this.labelTagEncMmValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagEncMmValue.Name = "labelTagEncMmValue";
            this.labelTagEncMmValue.Size = new System.Drawing.Size(144, 22);
            this.labelTagEncMmValue.TabIndex = 1;
            this.labelTagEncMmValue.Text = "—";
            // 
            // labelTagEncMmKey
            // 
            this.labelTagEncMmKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagEncMmKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagEncMmKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagEncMmKey.Name = "labelTagEncMmKey";
            this.labelTagEncMmKey.Size = new System.Drawing.Size(102, 22);
            this.labelTagEncMmKey.TabIndex = 0;
            this.labelTagEncMmKey.Text = "编码器位置(mm)";
            this.labelTagEncMmKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagCmdMm
            // 
            this.panelTagCmdMm.Controls.Add(this.labelTagCmdMmValue);
            this.panelTagCmdMm.Controls.Add(this.labelTagCmdMmKey);
            this.panelTagCmdMm.Location = new System.Drawing.Point(8, 296);
            this.panelTagCmdMm.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagCmdMm.Name = "panelTagCmdMm";
            this.panelTagCmdMm.Radius = 0;
            this.panelTagCmdMm.Size = new System.Drawing.Size(252, 24);
            this.panelTagCmdMm.TabIndex = 9;
            // 
            // labelTagCmdMmValue
            // 
            this.labelTagCmdMmValue.AutoEllipsis = true;
            this.labelTagCmdMmValue.Location = new System.Drawing.Point(108, 0);
            this.labelTagCmdMmValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagCmdMmValue.Name = "labelTagCmdMmValue";
            this.labelTagCmdMmValue.Size = new System.Drawing.Size(144, 22);
            this.labelTagCmdMmValue.TabIndex = 1;
            this.labelTagCmdMmValue.Text = "—";
            // 
            // labelTagCmdMmKey
            // 
            this.labelTagCmdMmKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagCmdMmKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagCmdMmKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagCmdMmKey.Name = "labelTagCmdMmKey";
            this.labelTagCmdMmKey.Size = new System.Drawing.Size(102, 22);
            this.labelTagCmdMmKey.TabIndex = 0;
            this.labelTagCmdMmKey.Text = "规划位置(mm)";
            this.labelTagCmdMmKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagDone
            // 
            this.panelTagDone.Controls.Add(this.labelTagDoneValue);
            this.panelTagDone.Controls.Add(this.labelTagDoneKey);
            this.panelTagDone.Location = new System.Drawing.Point(8, 272);
            this.panelTagDone.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagDone.Name = "panelTagDone";
            this.panelTagDone.Radius = 0;
            this.panelTagDone.Size = new System.Drawing.Size(252, 24);
            this.panelTagDone.TabIndex = 8;
            // 
            // labelTagDoneValue
            // 
            this.labelTagDoneValue.AutoEllipsis = true;
            this.labelTagDoneValue.Location = new System.Drawing.Point(108, 0);
            this.labelTagDoneValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagDoneValue.Name = "labelTagDoneValue";
            this.labelTagDoneValue.Size = new System.Drawing.Size(144, 22);
            this.labelTagDoneValue.TabIndex = 1;
            this.labelTagDoneValue.Text = "—";
            // 
            // labelTagDoneKey
            // 
            this.labelTagDoneKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagDoneKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagDoneKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagDoneKey.Name = "labelTagDoneKey";
            this.labelTagDoneKey.Size = new System.Drawing.Size(102, 22);
            this.labelTagDoneKey.TabIndex = 0;
            this.labelTagDoneKey.Text = "到位状态";
            this.labelTagDoneKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagHome
            // 
            this.panelTagHome.Controls.Add(this.labelTagHomeValue);
            this.panelTagHome.Controls.Add(this.labelTagHomeKey);
            this.panelTagHome.Location = new System.Drawing.Point(8, 248);
            this.panelTagHome.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagHome.Name = "panelTagHome";
            this.panelTagHome.Radius = 0;
            this.panelTagHome.Size = new System.Drawing.Size(252, 24);
            this.panelTagHome.TabIndex = 7;
            // 
            // labelTagHomeValue
            // 
            this.labelTagHomeValue.AutoEllipsis = true;
            this.labelTagHomeValue.Location = new System.Drawing.Point(108, 0);
            this.labelTagHomeValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagHomeValue.Name = "labelTagHomeValue";
            this.labelTagHomeValue.Size = new System.Drawing.Size(144, 22);
            this.labelTagHomeValue.TabIndex = 1;
            this.labelTagHomeValue.Text = "—";
            // 
            // labelTagHomeKey
            // 
            this.labelTagHomeKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagHomeKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagHomeKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagHomeKey.Name = "labelTagHomeKey";
            this.labelTagHomeKey.Size = new System.Drawing.Size(102, 22);
            this.labelTagHomeKey.TabIndex = 0;
            this.labelTagHomeKey.Text = "原点状态";
            this.labelTagHomeKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagEnable
            // 
            this.panelTagEnable.Controls.Add(this.labelTagEnableValue);
            this.panelTagEnable.Controls.Add(this.labelTagEnableKey);
            this.panelTagEnable.Location = new System.Drawing.Point(8, 224);
            this.panelTagEnable.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagEnable.Name = "panelTagEnable";
            this.panelTagEnable.Radius = 0;
            this.panelTagEnable.Size = new System.Drawing.Size(252, 24);
            this.panelTagEnable.TabIndex = 6;
            // 
            // labelTagEnableValue
            // 
            this.labelTagEnableValue.AutoEllipsis = true;
            this.labelTagEnableValue.Location = new System.Drawing.Point(108, 0);
            this.labelTagEnableValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagEnableValue.Name = "labelTagEnableValue";
            this.labelTagEnableValue.Size = new System.Drawing.Size(144, 22);
            this.labelTagEnableValue.TabIndex = 1;
            this.labelTagEnableValue.Text = "—";
            // 
            // labelTagEnableKey
            // 
            this.labelTagEnableKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagEnableKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagEnableKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagEnableKey.Name = "labelTagEnableKey";
            this.labelTagEnableKey.Size = new System.Drawing.Size(102, 22);
            this.labelTagEnableKey.TabIndex = 0;
            this.labelTagEnableKey.Text = "使能状态";
            this.labelTagEnableKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagLimit
            // 
            this.panelTagLimit.Controls.Add(this.labelTagLimitValue);
            this.panelTagLimit.Controls.Add(this.labelTagLimitKey);
            this.panelTagLimit.Location = new System.Drawing.Point(8, 200);
            this.panelTagLimit.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagLimit.Name = "panelTagLimit";
            this.panelTagLimit.Radius = 0;
            this.panelTagLimit.Size = new System.Drawing.Size(252, 24);
            this.panelTagLimit.TabIndex = 5;
            // 
            // labelTagLimitValue
            // 
            this.labelTagLimitValue.AutoEllipsis = true;
            this.labelTagLimitValue.Location = new System.Drawing.Point(108, 0);
            this.labelTagLimitValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagLimitValue.Name = "labelTagLimitValue";
            this.labelTagLimitValue.Size = new System.Drawing.Size(144, 22);
            this.labelTagLimitValue.TabIndex = 1;
            this.labelTagLimitValue.Text = "—";
            // 
            // labelTagLimitKey
            // 
            this.labelTagLimitKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagLimitKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagLimitKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagLimitKey.Name = "labelTagLimitKey";
            this.labelTagLimitKey.Size = new System.Drawing.Size(102, 22);
            this.labelTagLimitKey.TabIndex = 0;
            this.labelTagLimitKey.Text = "限位状态";
            this.labelTagLimitKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagInterlock
            // 
            this.panelTagInterlock.Controls.Add(this.labelTagInterlockValue);
            this.panelTagInterlock.Controls.Add(this.labelTagInterlockKey);
            this.panelTagInterlock.Location = new System.Drawing.Point(8, 160);
            this.panelTagInterlock.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagInterlock.Name = "panelTagInterlock";
            this.panelTagInterlock.Radius = 0;
            this.panelTagInterlock.Size = new System.Drawing.Size(252, 40);
            this.panelTagInterlock.TabIndex = 4;
            // 
            // labelTagInterlockValue
            // 
            this.labelTagInterlockValue.AutoEllipsis = true;
            this.labelTagInterlockValue.Location = new System.Drawing.Point(108, 0);
            this.labelTagInterlockValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagInterlockValue.Name = "labelTagInterlockValue";
            this.labelTagInterlockValue.Size = new System.Drawing.Size(144, 40);
            this.labelTagInterlockValue.TabIndex = 1;
            this.labelTagInterlockValue.Text = "—";
            // 
            // labelTagInterlockKey
            // 
            this.labelTagInterlockKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagInterlockKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagInterlockKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagInterlockKey.Name = "labelTagInterlockKey";
            this.labelTagInterlockKey.Size = new System.Drawing.Size(102, 22);
            this.labelTagInterlockKey.TabIndex = 0;
            this.labelTagInterlockKey.Text = "联锁说明";
            this.labelTagInterlockKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagState
            // 
            this.panelTagState.Controls.Add(this.labelTagStateValue);
            this.panelTagState.Controls.Add(this.labelTagStateKey);
            this.panelTagState.Location = new System.Drawing.Point(8, 136);
            this.panelTagState.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagState.Name = "panelTagState";
            this.panelTagState.Radius = 0;
            this.panelTagState.Size = new System.Drawing.Size(252, 24);
            this.panelTagState.TabIndex = 3;
            // 
            // labelTagStateValue
            // 
            this.labelTagStateValue.AutoEllipsis = true;
            this.labelTagStateValue.Location = new System.Drawing.Point(108, 0);
            this.labelTagStateValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagStateValue.Name = "labelTagStateValue";
            this.labelTagStateValue.Size = new System.Drawing.Size(144, 22);
            this.labelTagStateValue.TabIndex = 1;
            this.labelTagStateValue.Text = "—";
            // 
            // labelTagStateKey
            // 
            this.labelTagStateKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagStateKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagStateKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagStateKey.Name = "labelTagStateKey";
            this.labelTagStateKey.Size = new System.Drawing.Size(102, 22);
            this.labelTagStateKey.TabIndex = 0;
            this.labelTagStateKey.Text = "当前状态";
            this.labelTagStateKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagUpdate
            // 
            this.panelTagUpdate.Controls.Add(this.labelTagUpdateValue);
            this.panelTagUpdate.Controls.Add(this.labelTagUpdateKey);
            this.panelTagUpdate.Location = new System.Drawing.Point(8, 112);
            this.panelTagUpdate.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagUpdate.Name = "panelTagUpdate";
            this.panelTagUpdate.Radius = 0;
            this.panelTagUpdate.Size = new System.Drawing.Size(252, 24);
            this.panelTagUpdate.TabIndex = 2;
            // 
            // labelTagUpdateValue
            // 
            this.labelTagUpdateValue.AutoEllipsis = true;
            this.labelTagUpdateValue.Location = new System.Drawing.Point(108, 0);
            this.labelTagUpdateValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagUpdateValue.Name = "labelTagUpdateValue";
            this.labelTagUpdateValue.Size = new System.Drawing.Size(144, 22);
            this.labelTagUpdateValue.TabIndex = 1;
            this.labelTagUpdateValue.Text = "—";
            // 
            // labelTagUpdateKey
            // 
            this.labelTagUpdateKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagUpdateKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagUpdateKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagUpdateKey.Name = "labelTagUpdateKey";
            this.labelTagUpdateKey.Size = new System.Drawing.Size(102, 22);
            this.labelTagUpdateKey.TabIndex = 0;
            this.labelTagUpdateKey.Text = "更新时间";
            this.labelTagUpdateKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagScan
            // 
            this.panelTagScan.Controls.Add(this.labelTagScanValue);
            this.panelTagScan.Controls.Add(this.labelTagScanKey);
            this.panelTagScan.Location = new System.Drawing.Point(8, 88);
            this.panelTagScan.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagScan.Name = "panelTagScan";
            this.panelTagScan.Radius = 0;
            this.panelTagScan.Size = new System.Drawing.Size(252, 24);
            this.panelTagScan.TabIndex = 1;
            // 
            // labelTagScanValue
            // 
            this.labelTagScanValue.AutoEllipsis = true;
            this.labelTagScanValue.Location = new System.Drawing.Point(108, 0);
            this.labelTagScanValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagScanValue.Name = "labelTagScanValue";
            this.labelTagScanValue.Size = new System.Drawing.Size(144, 22);
            this.labelTagScanValue.TabIndex = 1;
            this.labelTagScanValue.Text = "—";
            // 
            // labelTagScanKey
            // 
            this.labelTagScanKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagScanKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagScanKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagScanKey.Name = "labelTagScanKey";
            this.labelTagScanKey.Size = new System.Drawing.Size(102, 22);
            this.labelTagScanKey.TabIndex = 0;
            this.labelTagScanKey.Text = "采样状态";
            this.labelTagScanKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagCard
            // 
            this.panelTagCard.Controls.Add(this.labelTagCardValue);
            this.panelTagCard.Controls.Add(this.labelTagCardKey);
            this.panelTagCard.Location = new System.Drawing.Point(8, 64);
            this.panelTagCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagCard.Name = "panelTagCard";
            this.panelTagCard.Radius = 0;
            this.panelTagCard.Size = new System.Drawing.Size(252, 24);
            this.panelTagCard.TabIndex = 0;
            // 
            // labelTagCardValue
            // 
            this.labelTagCardValue.AutoEllipsis = true;
            this.labelTagCardValue.Location = new System.Drawing.Point(108, 0);
            this.labelTagCardValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagCardValue.Name = "labelTagCardValue";
            this.labelTagCardValue.Size = new System.Drawing.Size(144, 22);
            this.labelTagCardValue.TabIndex = 1;
            this.labelTagCardValue.Text = "—";
            // 
            // labelTagCardKey
            // 
            this.labelTagCardKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagCardKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagCardKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagCardKey.Name = "labelTagCardKey";
            this.labelTagCardKey.Size = new System.Drawing.Size(102, 22);
            this.labelTagCardKey.TabIndex = 0;
            this.labelTagCardKey.Text = "控制卡";
            this.labelTagCardKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.labelSubTitle);
            this.panelHeader.Controls.Add(this.labelTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Padding = new System.Windows.Forms.Padding(8);
            this.panelHeader.Radius = 0;
            this.panelHeader.Size = new System.Drawing.Size(272, 60);
            this.panelHeader.TabIndex = 0;
            // 
            // labelSubTitle
            // 
            this.labelSubTitle.AutoEllipsis = true;
            this.labelSubTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelSubTitle.ForeColor = System.Drawing.Color.Gray;
            this.labelSubTitle.Location = new System.Drawing.Point(8, 34);
            this.labelSubTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelSubTitle.Name = "labelSubTitle";
            this.labelSubTitle.Size = new System.Drawing.Size(256, 22);
            this.labelSubTitle.TabIndex = 1;
            this.labelSubTitle.Text = "卡#0  默认虚拟卡";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoEllipsis = true;
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(8, 8);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(256, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "当前轴：未选择";
            // 
            // panelEmpty
            // 
            this.panelEmpty.Controls.Add(this.labelEmpty);
            this.panelEmpty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEmpty.Location = new System.Drawing.Point(0, 0);
            this.panelEmpty.Margin = new System.Windows.Forms.Padding(0);
            this.panelEmpty.Name = "panelEmpty";
            this.panelEmpty.Radius = 0;
            this.panelEmpty.Size = new System.Drawing.Size(272, 608);
            this.panelEmpty.TabIndex = 0;
            // 
            // labelEmpty
            // 
            this.labelEmpty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEmpty.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.labelEmpty.ForeColor = System.Drawing.Color.Gray;
            this.labelEmpty.Location = new System.Drawing.Point(0, 0);
            this.labelEmpty.Margin = new System.Windows.Forms.Padding(0);
            this.labelEmpty.Name = "labelEmpty";
            this.labelEmpty.Size = new System.Drawing.Size(272, 608);
            this.labelEmpty.TabIndex = 0;
            this.labelEmpty.Text = "请先选择轴查看实时监视信息";
            this.labelEmpty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MotionAxisDetailControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MotionAxisDetailControl";
            this.Size = new System.Drawing.Size(272, 608);
            this.panelRoot.ResumeLayout(false);
            this.panelDetail.ResumeLayout(false);
            this.panelScroll.ResumeLayout(false);
            this.panelTagJogVel.ResumeLayout(false);
            this.panelTagDefaultVel.ResumeLayout(false);
            this.panelTagErrorMm.ResumeLayout(false);
            this.panelTagEncMm.ResumeLayout(false);
            this.panelTagCmdMm.ResumeLayout(false);
            this.panelTagDone.ResumeLayout(false);
            this.panelTagHome.ResumeLayout(false);
            this.panelTagEnable.ResumeLayout(false);
            this.panelTagLimit.ResumeLayout(false);
            this.panelTagInterlock.ResumeLayout(false);
            this.panelTagState.ResumeLayout(false);
            this.panelTagUpdate.ResumeLayout(false);
            this.panelTagScan.ResumeLayout(false);
            this.panelTagCard.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.panelEmpty.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private AntdUI.Panel panelRoot;
        private AntdUI.Panel panelDetail;
        private System.Windows.Forms.Panel panelScroll;
        private AntdUI.Panel panelHeader;
        private AntdUI.Label labelSubTitle;
        private AntdUI.Label labelTitle;
        private AntdUI.Panel panelEmpty;
        private AntdUI.Label labelEmpty;

        private AntdUI.Panel panelTagCard;
        private AntdUI.Label labelTagCardKey;
        private AntdUI.Label labelTagCardValue;
        private AntdUI.Panel panelTagScan;
        private AntdUI.Label labelTagScanKey;
        private AntdUI.Label labelTagScanValue;
        private AntdUI.Panel panelTagUpdate;
        private AntdUI.Label labelTagUpdateKey;
        private AntdUI.Label labelTagUpdateValue;
        private AntdUI.Panel panelTagState;
        private AntdUI.Label labelTagStateKey;
        private AntdUI.Label labelTagStateValue;
        private AntdUI.Panel panelTagInterlock;
        private AntdUI.Label labelTagInterlockKey;
        private AntdUI.Label labelTagInterlockValue;
        private AntdUI.Panel panelTagLimit;
        private AntdUI.Label labelTagLimitKey;
        private AntdUI.Label labelTagLimitValue;
        private AntdUI.Panel panelTagEnable;
        private AntdUI.Label labelTagEnableKey;
        private AntdUI.Label labelTagEnableValue;
        private AntdUI.Panel panelTagHome;
        private AntdUI.Label labelTagHomeKey;
        private AntdUI.Label labelTagHomeValue;
        private AntdUI.Panel panelTagDone;
        private AntdUI.Label labelTagDoneKey;
        private AntdUI.Label labelTagDoneValue;
        private AntdUI.Panel panelTagCmdMm;
        private AntdUI.Label labelTagCmdMmKey;
        private AntdUI.Label labelTagCmdMmValue;
        private AntdUI.Panel panelTagEncMm;
        private AntdUI.Label labelTagEncMmKey;
        private AntdUI.Label labelTagEncMmValue;
        private AntdUI.Panel panelTagErrorMm;
        private AntdUI.Label labelTagErrorMmKey;
        private AntdUI.Label labelTagErrorMmValue;
        private AntdUI.Panel panelTagDefaultVel;
        private AntdUI.Label labelTagDefaultVelKey;
        private AntdUI.Label labelTagDefaultVelValue;
        private AntdUI.Panel panelTagJogVel;
        private AntdUI.Label labelTagJogVelKey;
        private AntdUI.Label labelTagJogVelValue;
        private AntdUI.Label labelDescriptionTitle;
        private AntdUI.Label labelDescriptionText;
    }
}