namespace AMControlWinF.Views.Motion
{
    partial class MotionMonitorDetailControl
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
            this.panelTagJogVel = new AntdUI.Panel();
            this.labelTagJogVelValue = new AntdUI.Label();
            this.labelTagJogVelKey = new AntdUI.Label();
            this.panelTagDefaultVel = new AntdUI.Panel();
            this.labelTagDefaultVelValue = new AntdUI.Label();
            this.labelTagDefaultVelKey = new AntdUI.Label();
            this.panelTagEncPulse = new AntdUI.Panel();
            this.labelTagEncPulseValue = new AntdUI.Label();
            this.labelTagEncPulseKey = new AntdUI.Label();
            this.panelTagCmdPulse = new AntdUI.Panel();
            this.labelTagCmdPulseValue = new AntdUI.Label();
            this.labelTagCmdPulseKey = new AntdUI.Label();
            this.panelTagErrorMm = new AntdUI.Panel();
            this.labelTagErrorMmValue = new AntdUI.Label();
            this.labelTagErrorMmKey = new AntdUI.Label();
            this.panelTagEncMm = new AntdUI.Panel();
            this.labelTagEncMmValue = new AntdUI.Label();
            this.labelTagEncMmKey = new AntdUI.Label();
            this.panelTagCmdMm = new AntdUI.Panel();
            this.labelTagCmdMmValue = new AntdUI.Label();
            this.labelTagCmdMmKey = new AntdUI.Label();
            this.panelTagLimit = new AntdUI.Panel();
            this.labelTagLimitValue = new AntdUI.Label();
            this.labelTagLimitKey = new AntdUI.Label();
            this.panelTagDone = new AntdUI.Panel();
            this.labelTagDoneValue = new AntdUI.Label();
            this.labelTagDoneKey = new AntdUI.Label();
            this.panelTagHome = new AntdUI.Panel();
            this.labelTagHomeValue = new AntdUI.Label();
            this.labelTagHomeKey = new AntdUI.Label();
            this.panelTagEnable = new AntdUI.Panel();
            this.labelTagEnableValue = new AntdUI.Label();
            this.labelTagEnableKey = new AntdUI.Label();
            this.panelTagState = new AntdUI.Panel();
            this.labelTagStateValue = new AntdUI.Label();
            this.labelTagStateKey = new AntdUI.Label();
            this.panelTagPhysical = new AntdUI.Panel();
            this.labelTagPhysicalValue = new AntdUI.Label();
            this.labelTagPhysicalKey = new AntdUI.Label();
            this.panelTagAxisType = new AntdUI.Panel();
            this.labelTagAxisTypeValue = new AntdUI.Label();
            this.labelTagAxisTypeKey = new AntdUI.Label();
            this.panelTagLogicalAxis = new AntdUI.Panel();
            this.labelTagLogicalAxisValue = new AntdUI.Label();
            this.labelTagLogicalAxisKey = new AntdUI.Label();
            this.panelHeader = new AntdUI.Panel();
            this.labelSubTitle = new AntdUI.Label();
            this.labelTitle = new AntdUI.Label();
            this.panelEmpty = new AntdUI.Panel();
            this.labelEmpty = new AntdUI.Label();
            this.labelTagLastUpdateValue = new AntdUI.Label();
            this.labelTagLastUpdateKey = new AntdUI.Label();
            this.panelTagLastUpdate = new AntdUI.Panel();
            this.panelRoot.SuspendLayout();
            this.panelDetail.SuspendLayout();
            this.panelScroll.SuspendLayout();
            this.panelTagJogVel.SuspendLayout();
            this.panelTagDefaultVel.SuspendLayout();
            this.panelTagEncPulse.SuspendLayout();
            this.panelTagCmdPulse.SuspendLayout();
            this.panelTagErrorMm.SuspendLayout();
            this.panelTagEncMm.SuspendLayout();
            this.panelTagCmdMm.SuspendLayout();
            this.panelTagLimit.SuspendLayout();
            this.panelTagDone.SuspendLayout();
            this.panelTagHome.SuspendLayout();
            this.panelTagEnable.SuspendLayout();
            this.panelTagState.SuspendLayout();
            this.panelTagPhysical.SuspendLayout();
            this.panelTagAxisType.SuspendLayout();
            this.panelTagLogicalAxis.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.panelEmpty.SuspendLayout();
            this.panelTagLastUpdate.SuspendLayout();
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
            this.panelRoot.Size = new System.Drawing.Size(250, 550);
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
            this.panelDetail.Size = new System.Drawing.Size(250, 550);
            this.panelDetail.TabIndex = 1;
            this.panelDetail.Visible = false;
            // 
            // panelScroll
            // 
            this.panelScroll.Controls.Add(this.panelTagLastUpdate);
            this.panelScroll.Controls.Add(this.panelTagJogVel);
            this.panelScroll.Controls.Add(this.panelTagDefaultVel);
            this.panelScroll.Controls.Add(this.panelTagEncPulse);
            this.panelScroll.Controls.Add(this.panelTagCmdPulse);
            this.panelScroll.Controls.Add(this.panelTagErrorMm);
            this.panelScroll.Controls.Add(this.panelTagEncMm);
            this.panelScroll.Controls.Add(this.panelTagCmdMm);
            this.panelScroll.Controls.Add(this.panelTagLimit);
            this.panelScroll.Controls.Add(this.panelTagDone);
            this.panelScroll.Controls.Add(this.panelTagHome);
            this.panelScroll.Controls.Add(this.panelTagEnable);
            this.panelScroll.Controls.Add(this.panelTagState);
            this.panelScroll.Controls.Add(this.panelTagPhysical);
            this.panelScroll.Controls.Add(this.panelTagAxisType);
            this.panelScroll.Controls.Add(this.panelTagLogicalAxis);
            this.panelScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelScroll.Location = new System.Drawing.Point(0, 60);
            this.panelScroll.Margin = new System.Windows.Forms.Padding(0);
            this.panelScroll.Name = "panelScroll";
            this.panelScroll.Padding = new System.Windows.Forms.Padding(14, 12, 14, 12);
            this.panelScroll.Size = new System.Drawing.Size(250, 490);
            this.panelScroll.TabIndex = 1;
            // 
            // panelTagJogVel
            // 
            this.panelTagJogVel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTagJogVel.Controls.Add(this.labelTagJogVelValue);
            this.panelTagJogVel.Controls.Add(this.labelTagJogVelKey);
            this.panelTagJogVel.Location = new System.Drawing.Point(8, 338);
            this.panelTagJogVel.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagJogVel.Name = "panelTagJogVel";
            this.panelTagJogVel.Radius = 0;
            this.panelTagJogVel.Size = new System.Drawing.Size(234, 24);
            this.panelTagJogVel.TabIndex = 14;
            // 
            // labelTagJogVelValue
            // 
            this.labelTagJogVelValue.AutoEllipsis = true;
            this.labelTagJogVelValue.Location = new System.Drawing.Point(94, 0);
            this.labelTagJogVelValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagJogVelValue.Name = "labelTagJogVelValue";
            this.labelTagJogVelValue.Size = new System.Drawing.Size(140, 22);
            this.labelTagJogVelValue.TabIndex = 1;
            this.labelTagJogVelValue.Text = "—";
            // 
            // labelTagJogVelKey
            // 
            this.labelTagJogVelKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagJogVelKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagJogVelKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagJogVelKey.Name = "labelTagJogVelKey";
            this.labelTagJogVelKey.Size = new System.Drawing.Size(88, 22);
            this.labelTagJogVelKey.TabIndex = 0;
            this.labelTagJogVelKey.Text = "点动速度";
            this.labelTagJogVelKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagDefaultVel
            // 
            this.panelTagDefaultVel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTagDefaultVel.Controls.Add(this.labelTagDefaultVelValue);
            this.panelTagDefaultVel.Controls.Add(this.labelTagDefaultVelKey);
            this.panelTagDefaultVel.Location = new System.Drawing.Point(8, 314);
            this.panelTagDefaultVel.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagDefaultVel.Name = "panelTagDefaultVel";
            this.panelTagDefaultVel.Radius = 0;
            this.panelTagDefaultVel.Size = new System.Drawing.Size(234, 24);
            this.panelTagDefaultVel.TabIndex = 13;
            // 
            // labelTagDefaultVelValue
            // 
            this.labelTagDefaultVelValue.AutoEllipsis = true;
            this.labelTagDefaultVelValue.Location = new System.Drawing.Point(94, 0);
            this.labelTagDefaultVelValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagDefaultVelValue.Name = "labelTagDefaultVelValue";
            this.labelTagDefaultVelValue.Size = new System.Drawing.Size(140, 22);
            this.labelTagDefaultVelValue.TabIndex = 1;
            this.labelTagDefaultVelValue.Text = "—";
            // 
            // labelTagDefaultVelKey
            // 
            this.labelTagDefaultVelKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagDefaultVelKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagDefaultVelKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagDefaultVelKey.Name = "labelTagDefaultVelKey";
            this.labelTagDefaultVelKey.Size = new System.Drawing.Size(88, 22);
            this.labelTagDefaultVelKey.TabIndex = 0;
            this.labelTagDefaultVelKey.Text = "默认速度";
            this.labelTagDefaultVelKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagEncPulse
            // 
            this.panelTagEncPulse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTagEncPulse.Controls.Add(this.labelTagEncPulseValue);
            this.panelTagEncPulse.Controls.Add(this.labelTagEncPulseKey);
            this.panelTagEncPulse.Location = new System.Drawing.Point(8, 290);
            this.panelTagEncPulse.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagEncPulse.Name = "panelTagEncPulse";
            this.panelTagEncPulse.Radius = 0;
            this.panelTagEncPulse.Size = new System.Drawing.Size(234, 24);
            this.panelTagEncPulse.TabIndex = 12;
            // 
            // labelTagEncPulseValue
            // 
            this.labelTagEncPulseValue.AutoEllipsis = true;
            this.labelTagEncPulseValue.Location = new System.Drawing.Point(94, 0);
            this.labelTagEncPulseValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagEncPulseValue.Name = "labelTagEncPulseValue";
            this.labelTagEncPulseValue.Size = new System.Drawing.Size(140, 22);
            this.labelTagEncPulseValue.TabIndex = 1;
            this.labelTagEncPulseValue.Text = "—";
            // 
            // labelTagEncPulseKey
            // 
            this.labelTagEncPulseKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagEncPulseKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagEncPulseKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagEncPulseKey.Name = "labelTagEncPulseKey";
            this.labelTagEncPulseKey.Size = new System.Drawing.Size(88, 22);
            this.labelTagEncPulseKey.TabIndex = 0;
            this.labelTagEncPulseKey.Text = "编码器脉冲";
            this.labelTagEncPulseKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagCmdPulse
            // 
            this.panelTagCmdPulse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTagCmdPulse.Controls.Add(this.labelTagCmdPulseValue);
            this.panelTagCmdPulse.Controls.Add(this.labelTagCmdPulseKey);
            this.panelTagCmdPulse.Location = new System.Drawing.Point(8, 266);
            this.panelTagCmdPulse.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagCmdPulse.Name = "panelTagCmdPulse";
            this.panelTagCmdPulse.Radius = 0;
            this.panelTagCmdPulse.Size = new System.Drawing.Size(234, 24);
            this.panelTagCmdPulse.TabIndex = 11;
            // 
            // labelTagCmdPulseValue
            // 
            this.labelTagCmdPulseValue.AutoEllipsis = true;
            this.labelTagCmdPulseValue.Location = new System.Drawing.Point(94, 0);
            this.labelTagCmdPulseValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagCmdPulseValue.Name = "labelTagCmdPulseValue";
            this.labelTagCmdPulseValue.Size = new System.Drawing.Size(140, 22);
            this.labelTagCmdPulseValue.TabIndex = 1;
            this.labelTagCmdPulseValue.Text = "—";
            // 
            // labelTagCmdPulseKey
            // 
            this.labelTagCmdPulseKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagCmdPulseKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagCmdPulseKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagCmdPulseKey.Name = "labelTagCmdPulseKey";
            this.labelTagCmdPulseKey.Size = new System.Drawing.Size(88, 22);
            this.labelTagCmdPulseKey.TabIndex = 0;
            this.labelTagCmdPulseKey.Text = "指令脉冲";
            this.labelTagCmdPulseKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagErrorMm
            // 
            this.panelTagErrorMm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTagErrorMm.Controls.Add(this.labelTagErrorMmValue);
            this.panelTagErrorMm.Controls.Add(this.labelTagErrorMmKey);
            this.panelTagErrorMm.Location = new System.Drawing.Point(8, 242);
            this.panelTagErrorMm.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagErrorMm.Name = "panelTagErrorMm";
            this.panelTagErrorMm.Radius = 0;
            this.panelTagErrorMm.Size = new System.Drawing.Size(234, 24);
            this.panelTagErrorMm.TabIndex = 10;
            // 
            // labelTagErrorMmValue
            // 
            this.labelTagErrorMmValue.AutoEllipsis = true;
            this.labelTagErrorMmValue.Location = new System.Drawing.Point(94, 0);
            this.labelTagErrorMmValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagErrorMmValue.Name = "labelTagErrorMmValue";
            this.labelTagErrorMmValue.Size = new System.Drawing.Size(140, 22);
            this.labelTagErrorMmValue.TabIndex = 1;
            this.labelTagErrorMmValue.Text = "—";
            // 
            // labelTagErrorMmKey
            // 
            this.labelTagErrorMmKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagErrorMmKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagErrorMmKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagErrorMmKey.Name = "labelTagErrorMmKey";
            this.labelTagErrorMmKey.Size = new System.Drawing.Size(88, 22);
            this.labelTagErrorMmKey.TabIndex = 0;
            this.labelTagErrorMmKey.Text = "位置误差";
            this.labelTagErrorMmKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagEncMm
            // 
            this.panelTagEncMm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTagEncMm.Controls.Add(this.labelTagEncMmValue);
            this.panelTagEncMm.Controls.Add(this.labelTagEncMmKey);
            this.panelTagEncMm.Location = new System.Drawing.Point(8, 218);
            this.panelTagEncMm.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagEncMm.Name = "panelTagEncMm";
            this.panelTagEncMm.Radius = 0;
            this.panelTagEncMm.Size = new System.Drawing.Size(234, 24);
            this.panelTagEncMm.TabIndex = 9;
            // 
            // labelTagEncMmValue
            // 
            this.labelTagEncMmValue.AutoEllipsis = true;
            this.labelTagEncMmValue.Location = new System.Drawing.Point(94, 0);
            this.labelTagEncMmValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagEncMmValue.Name = "labelTagEncMmValue";
            this.labelTagEncMmValue.Size = new System.Drawing.Size(140, 22);
            this.labelTagEncMmValue.TabIndex = 1;
            this.labelTagEncMmValue.Text = "—";
            // 
            // labelTagEncMmKey
            // 
            this.labelTagEncMmKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagEncMmKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagEncMmKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagEncMmKey.Name = "labelTagEncMmKey";
            this.labelTagEncMmKey.Size = new System.Drawing.Size(88, 22);
            this.labelTagEncMmKey.TabIndex = 0;
            this.labelTagEncMmKey.Text = "编码器位置";
            this.labelTagEncMmKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagCmdMm
            // 
            this.panelTagCmdMm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTagCmdMm.Controls.Add(this.labelTagCmdMmValue);
            this.panelTagCmdMm.Controls.Add(this.labelTagCmdMmKey);
            this.panelTagCmdMm.Location = new System.Drawing.Point(8, 194);
            this.panelTagCmdMm.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagCmdMm.Name = "panelTagCmdMm";
            this.panelTagCmdMm.Radius = 0;
            this.panelTagCmdMm.Size = new System.Drawing.Size(234, 24);
            this.panelTagCmdMm.TabIndex = 8;
            // 
            // labelTagCmdMmValue
            // 
            this.labelTagCmdMmValue.AutoEllipsis = true;
            this.labelTagCmdMmValue.Location = new System.Drawing.Point(94, 0);
            this.labelTagCmdMmValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagCmdMmValue.Name = "labelTagCmdMmValue";
            this.labelTagCmdMmValue.Size = new System.Drawing.Size(140, 22);
            this.labelTagCmdMmValue.TabIndex = 1;
            this.labelTagCmdMmValue.Text = "—";
            // 
            // labelTagCmdMmKey
            // 
            this.labelTagCmdMmKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagCmdMmKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagCmdMmKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagCmdMmKey.Name = "labelTagCmdMmKey";
            this.labelTagCmdMmKey.Size = new System.Drawing.Size(88, 22);
            this.labelTagCmdMmKey.TabIndex = 0;
            this.labelTagCmdMmKey.Text = "指令位置";
            this.labelTagCmdMmKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagLimit
            // 
            this.panelTagLimit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTagLimit.Controls.Add(this.labelTagLimitValue);
            this.panelTagLimit.Controls.Add(this.labelTagLimitKey);
            this.panelTagLimit.Location = new System.Drawing.Point(8, 170);
            this.panelTagLimit.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagLimit.Name = "panelTagLimit";
            this.panelTagLimit.Radius = 0;
            this.panelTagLimit.Size = new System.Drawing.Size(234, 24);
            this.panelTagLimit.TabIndex = 7;
            // 
            // labelTagLimitValue
            // 
            this.labelTagLimitValue.AutoEllipsis = true;
            this.labelTagLimitValue.Location = new System.Drawing.Point(94, 0);
            this.labelTagLimitValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagLimitValue.Name = "labelTagLimitValue";
            this.labelTagLimitValue.Size = new System.Drawing.Size(140, 22);
            this.labelTagLimitValue.TabIndex = 1;
            this.labelTagLimitValue.Text = "—";
            // 
            // labelTagLimitKey
            // 
            this.labelTagLimitKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagLimitKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagLimitKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagLimitKey.Name = "labelTagLimitKey";
            this.labelTagLimitKey.Size = new System.Drawing.Size(88, 22);
            this.labelTagLimitKey.TabIndex = 0;
            this.labelTagLimitKey.Text = "限位状态";
            this.labelTagLimitKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagDone
            // 
            this.panelTagDone.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTagDone.Controls.Add(this.labelTagDoneValue);
            this.panelTagDone.Controls.Add(this.labelTagDoneKey);
            this.panelTagDone.Location = new System.Drawing.Point(8, 146);
            this.panelTagDone.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagDone.Name = "panelTagDone";
            this.panelTagDone.Radius = 0;
            this.panelTagDone.Size = new System.Drawing.Size(234, 24);
            this.panelTagDone.TabIndex = 6;
            // 
            // labelTagDoneValue
            // 
            this.labelTagDoneValue.AutoEllipsis = true;
            this.labelTagDoneValue.Location = new System.Drawing.Point(94, 0);
            this.labelTagDoneValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagDoneValue.Name = "labelTagDoneValue";
            this.labelTagDoneValue.Size = new System.Drawing.Size(140, 22);
            this.labelTagDoneValue.TabIndex = 1;
            this.labelTagDoneValue.Text = "—";
            // 
            // labelTagDoneKey
            // 
            this.labelTagDoneKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagDoneKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagDoneKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagDoneKey.Name = "labelTagDoneKey";
            this.labelTagDoneKey.Size = new System.Drawing.Size(88, 22);
            this.labelTagDoneKey.TabIndex = 0;
            this.labelTagDoneKey.Text = "到位状态";
            this.labelTagDoneKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagHome
            // 
            this.panelTagHome.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTagHome.Controls.Add(this.labelTagHomeValue);
            this.panelTagHome.Controls.Add(this.labelTagHomeKey);
            this.panelTagHome.Location = new System.Drawing.Point(8, 122);
            this.panelTagHome.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagHome.Name = "panelTagHome";
            this.panelTagHome.Radius = 0;
            this.panelTagHome.Size = new System.Drawing.Size(234, 24);
            this.panelTagHome.TabIndex = 5;
            // 
            // labelTagHomeValue
            // 
            this.labelTagHomeValue.AutoEllipsis = true;
            this.labelTagHomeValue.Location = new System.Drawing.Point(94, 0);
            this.labelTagHomeValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagHomeValue.Name = "labelTagHomeValue";
            this.labelTagHomeValue.Size = new System.Drawing.Size(140, 22);
            this.labelTagHomeValue.TabIndex = 1;
            this.labelTagHomeValue.Text = "—";
            // 
            // labelTagHomeKey
            // 
            this.labelTagHomeKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagHomeKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagHomeKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagHomeKey.Name = "labelTagHomeKey";
            this.labelTagHomeKey.Size = new System.Drawing.Size(88, 22);
            this.labelTagHomeKey.TabIndex = 0;
            this.labelTagHomeKey.Text = "原点状态";
            this.labelTagHomeKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagEnable
            // 
            this.panelTagEnable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTagEnable.Controls.Add(this.labelTagEnableValue);
            this.panelTagEnable.Controls.Add(this.labelTagEnableKey);
            this.panelTagEnable.Location = new System.Drawing.Point(8, 98);
            this.panelTagEnable.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagEnable.Name = "panelTagEnable";
            this.panelTagEnable.Radius = 0;
            this.panelTagEnable.Size = new System.Drawing.Size(234, 24);
            this.panelTagEnable.TabIndex = 4;
            // 
            // labelTagEnableValue
            // 
            this.labelTagEnableValue.AutoEllipsis = true;
            this.labelTagEnableValue.Location = new System.Drawing.Point(94, 0);
            this.labelTagEnableValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagEnableValue.Name = "labelTagEnableValue";
            this.labelTagEnableValue.Size = new System.Drawing.Size(140, 22);
            this.labelTagEnableValue.TabIndex = 1;
            this.labelTagEnableValue.Text = "—";
            // 
            // labelTagEnableKey
            // 
            this.labelTagEnableKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagEnableKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagEnableKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagEnableKey.Name = "labelTagEnableKey";
            this.labelTagEnableKey.Size = new System.Drawing.Size(88, 22);
            this.labelTagEnableKey.TabIndex = 0;
            this.labelTagEnableKey.Text = "使能状态";
            this.labelTagEnableKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagState
            // 
            this.panelTagState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTagState.Controls.Add(this.labelTagStateValue);
            this.panelTagState.Controls.Add(this.labelTagStateKey);
            this.panelTagState.Location = new System.Drawing.Point(8, 74);
            this.panelTagState.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagState.Name = "panelTagState";
            this.panelTagState.Radius = 0;
            this.panelTagState.Size = new System.Drawing.Size(234, 24);
            this.panelTagState.TabIndex = 3;
            // 
            // labelTagStateValue
            // 
            this.labelTagStateValue.AutoEllipsis = true;
            this.labelTagStateValue.Location = new System.Drawing.Point(94, 0);
            this.labelTagStateValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagStateValue.Name = "labelTagStateValue";
            this.labelTagStateValue.Size = new System.Drawing.Size(140, 22);
            this.labelTagStateValue.TabIndex = 1;
            this.labelTagStateValue.Text = "—";
            // 
            // labelTagStateKey
            // 
            this.labelTagStateKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagStateKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagStateKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagStateKey.Name = "labelTagStateKey";
            this.labelTagStateKey.Size = new System.Drawing.Size(88, 22);
            this.labelTagStateKey.TabIndex = 0;
            this.labelTagStateKey.Text = "当前状态";
            this.labelTagStateKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagPhysical
            // 
            this.panelTagPhysical.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTagPhysical.Controls.Add(this.labelTagPhysicalValue);
            this.panelTagPhysical.Controls.Add(this.labelTagPhysicalKey);
            this.panelTagPhysical.Location = new System.Drawing.Point(8, 50);
            this.panelTagPhysical.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagPhysical.Name = "panelTagPhysical";
            this.panelTagPhysical.Radius = 0;
            this.panelTagPhysical.Size = new System.Drawing.Size(234, 24);
            this.panelTagPhysical.TabIndex = 2;
            // 
            // labelTagPhysicalValue
            // 
            this.labelTagPhysicalValue.AutoEllipsis = true;
            this.labelTagPhysicalValue.Location = new System.Drawing.Point(94, 0);
            this.labelTagPhysicalValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagPhysicalValue.Name = "labelTagPhysicalValue";
            this.labelTagPhysicalValue.Size = new System.Drawing.Size(140, 22);
            this.labelTagPhysicalValue.TabIndex = 1;
            this.labelTagPhysicalValue.Text = "—";
            // 
            // labelTagPhysicalKey
            // 
            this.labelTagPhysicalKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagPhysicalKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagPhysicalKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagPhysicalKey.Name = "labelTagPhysicalKey";
            this.labelTagPhysicalKey.Size = new System.Drawing.Size(88, 22);
            this.labelTagPhysicalKey.TabIndex = 0;
            this.labelTagPhysicalKey.Text = "物理映射";
            this.labelTagPhysicalKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagAxisType
            // 
            this.panelTagAxisType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTagAxisType.Controls.Add(this.labelTagAxisTypeValue);
            this.panelTagAxisType.Controls.Add(this.labelTagAxisTypeKey);
            this.panelTagAxisType.Location = new System.Drawing.Point(8, 26);
            this.panelTagAxisType.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagAxisType.Name = "panelTagAxisType";
            this.panelTagAxisType.Radius = 0;
            this.panelTagAxisType.Size = new System.Drawing.Size(234, 24);
            this.panelTagAxisType.TabIndex = 1;
            // 
            // labelTagAxisTypeValue
            // 
            this.labelTagAxisTypeValue.AutoEllipsis = true;
            this.labelTagAxisTypeValue.Location = new System.Drawing.Point(94, 0);
            this.labelTagAxisTypeValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagAxisTypeValue.Name = "labelTagAxisTypeValue";
            this.labelTagAxisTypeValue.Size = new System.Drawing.Size(140, 22);
            this.labelTagAxisTypeValue.TabIndex = 1;
            this.labelTagAxisTypeValue.Text = "—";
            // 
            // labelTagAxisTypeKey
            // 
            this.labelTagAxisTypeKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagAxisTypeKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagAxisTypeKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagAxisTypeKey.Name = "labelTagAxisTypeKey";
            this.labelTagAxisTypeKey.Size = new System.Drawing.Size(88, 22);
            this.labelTagAxisTypeKey.TabIndex = 0;
            this.labelTagAxisTypeKey.Text = "轴类型";
            this.labelTagAxisTypeKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagLogicalAxis
            // 
            this.panelTagLogicalAxis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTagLogicalAxis.Controls.Add(this.labelTagLogicalAxisValue);
            this.panelTagLogicalAxis.Controls.Add(this.labelTagLogicalAxisKey);
            this.panelTagLogicalAxis.Location = new System.Drawing.Point(8, 2);
            this.panelTagLogicalAxis.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagLogicalAxis.Name = "panelTagLogicalAxis";
            this.panelTagLogicalAxis.Radius = 0;
            this.panelTagLogicalAxis.ShadowOpacity = 0F;
            this.panelTagLogicalAxis.ShadowOpacityHover = 0F;
            this.panelTagLogicalAxis.Size = new System.Drawing.Size(234, 24);
            this.panelTagLogicalAxis.TabIndex = 0;
            // 
            // labelTagLogicalAxisValue
            // 
            this.labelTagLogicalAxisValue.AutoEllipsis = true;
            this.labelTagLogicalAxisValue.Location = new System.Drawing.Point(94, 0);
            this.labelTagLogicalAxisValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagLogicalAxisValue.Name = "labelTagLogicalAxisValue";
            this.labelTagLogicalAxisValue.Size = new System.Drawing.Size(134, 22);
            this.labelTagLogicalAxisValue.TabIndex = 1;
            this.labelTagLogicalAxisValue.Text = "—";
            // 
            // labelTagLogicalAxisKey
            // 
            this.labelTagLogicalAxisKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagLogicalAxisKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagLogicalAxisKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagLogicalAxisKey.Name = "labelTagLogicalAxisKey";
            this.labelTagLogicalAxisKey.ShadowOpacity = 0F;
            this.labelTagLogicalAxisKey.Size = new System.Drawing.Size(88, 22);
            this.labelTagLogicalAxisKey.TabIndex = 0;
            this.labelTagLogicalAxisKey.Text = "逻辑轴";
            this.labelTagLogicalAxisKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.panelHeader.Size = new System.Drawing.Size(250, 60);
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
            this.labelSubTitle.Size = new System.Drawing.Size(234, 22);
            this.labelSubTitle.TabIndex = 1;
            this.labelSubTitle.Text = "—";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoEllipsis = true;
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(8, 8);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(234, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "—";
            // 
            // panelEmpty
            // 
            this.panelEmpty.Controls.Add(this.labelEmpty);
            this.panelEmpty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEmpty.Location = new System.Drawing.Point(0, 0);
            this.panelEmpty.Margin = new System.Windows.Forms.Padding(0);
            this.panelEmpty.Name = "panelEmpty";
            this.panelEmpty.Radius = 0;
            this.panelEmpty.Size = new System.Drawing.Size(250, 550);
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
            this.labelEmpty.Size = new System.Drawing.Size(250, 550);
            this.labelEmpty.TabIndex = 0;
            this.labelEmpty.Text = "请选择左侧轴卡片查看详情";
            this.labelEmpty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTagLastUpdateValue
            // 
            this.labelTagLastUpdateValue.AutoEllipsis = true;
            this.labelTagLastUpdateValue.Location = new System.Drawing.Point(94, 0);
            this.labelTagLastUpdateValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagLastUpdateValue.Name = "labelTagLastUpdateValue";
            this.labelTagLastUpdateValue.Size = new System.Drawing.Size(140, 22);
            this.labelTagLastUpdateValue.TabIndex = 1;
            this.labelTagLastUpdateValue.Text = "—";
            // 
            // labelTagLastUpdateKey
            // 
            this.labelTagLastUpdateKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagLastUpdateKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagLastUpdateKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagLastUpdateKey.Name = "labelTagLastUpdateKey";
            this.labelTagLastUpdateKey.Size = new System.Drawing.Size(88, 22);
            this.labelTagLastUpdateKey.TabIndex = 0;
            this.labelTagLastUpdateKey.Text = "最后更新";
            this.labelTagLastUpdateKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagLastUpdate
            // 
            this.panelTagLastUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTagLastUpdate.Controls.Add(this.labelTagLastUpdateValue);
            this.panelTagLastUpdate.Controls.Add(this.labelTagLastUpdateKey);
            this.panelTagLastUpdate.Location = new System.Drawing.Point(8, 362);
            this.panelTagLastUpdate.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagLastUpdate.Name = "panelTagLastUpdate";
            this.panelTagLastUpdate.Radius = 0;
            this.panelTagLastUpdate.Size = new System.Drawing.Size(234, 24);
            this.panelTagLastUpdate.TabIndex = 15;
            // 
            // MotionMonitorDetailControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MotionMonitorDetailControl";
            this.Size = new System.Drawing.Size(250, 550);
            this.panelRoot.ResumeLayout(false);
            this.panelDetail.ResumeLayout(false);
            this.panelScroll.ResumeLayout(false);
            this.panelTagJogVel.ResumeLayout(false);
            this.panelTagDefaultVel.ResumeLayout(false);
            this.panelTagEncPulse.ResumeLayout(false);
            this.panelTagCmdPulse.ResumeLayout(false);
            this.panelTagErrorMm.ResumeLayout(false);
            this.panelTagEncMm.ResumeLayout(false);
            this.panelTagCmdMm.ResumeLayout(false);
            this.panelTagLimit.ResumeLayout(false);
            this.panelTagDone.ResumeLayout(false);
            this.panelTagHome.ResumeLayout(false);
            this.panelTagEnable.ResumeLayout(false);
            this.panelTagState.ResumeLayout(false);
            this.panelTagPhysical.ResumeLayout(false);
            this.panelTagAxisType.ResumeLayout(false);
            this.panelTagLogicalAxis.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.panelEmpty.ResumeLayout(false);
            this.panelTagLastUpdate.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AntdUI.Panel panelRoot;
        private AntdUI.Panel panelDetail;
        private System.Windows.Forms.Panel panelScroll;
        private AntdUI.Panel panelTagLogicalAxis;
        private AntdUI.Label labelTagLogicalAxisKey;
        private AntdUI.Label labelTagLogicalAxisValue;
        private AntdUI.Panel panelTagAxisType;
        private AntdUI.Label labelTagAxisTypeKey;
        private AntdUI.Label labelTagAxisTypeValue;
        private AntdUI.Panel panelTagPhysical;
        private AntdUI.Label labelTagPhysicalKey;
        private AntdUI.Label labelTagPhysicalValue;
        private AntdUI.Panel panelTagState;
        private AntdUI.Label labelTagStateKey;
        private AntdUI.Label labelTagStateValue;
        private AntdUI.Panel panelTagEnable;
        private AntdUI.Label labelTagEnableKey;
        private AntdUI.Label labelTagEnableValue;
        private AntdUI.Panel panelTagHome;
        private AntdUI.Label labelTagHomeKey;
        private AntdUI.Label labelTagHomeValue;
        private AntdUI.Panel panelTagDone;
        private AntdUI.Label labelTagDoneKey;
        private AntdUI.Label labelTagDoneValue;
        private AntdUI.Panel panelTagLimit;
        private AntdUI.Label labelTagLimitKey;
        private AntdUI.Label labelTagLimitValue;
        private AntdUI.Panel panelTagCmdMm;
        private AntdUI.Label labelTagCmdMmKey;
        private AntdUI.Label labelTagCmdMmValue;
        private AntdUI.Panel panelTagEncMm;
        private AntdUI.Label labelTagEncMmKey;
        private AntdUI.Label labelTagEncMmValue;
        private AntdUI.Panel panelTagErrorMm;
        private AntdUI.Label labelTagErrorMmKey;
        private AntdUI.Label labelTagErrorMmValue;
        private AntdUI.Panel panelTagCmdPulse;
        private AntdUI.Label labelTagCmdPulseKey;
        private AntdUI.Label labelTagCmdPulseValue;
        private AntdUI.Panel panelTagEncPulse;
        private AntdUI.Label labelTagEncPulseKey;
        private AntdUI.Label labelTagEncPulseValue;
        private AntdUI.Panel panelTagDefaultVel;
        private AntdUI.Label labelTagDefaultVelKey;
        private AntdUI.Label labelTagDefaultVelValue;
        private AntdUI.Panel panelTagJogVel;
        private AntdUI.Label labelTagJogVelKey;
        private AntdUI.Label labelTagJogVelValue;
        private AntdUI.Panel panelHeader;
        private AntdUI.Label labelTitle;
        private AntdUI.Label labelSubTitle;
        private AntdUI.Panel panelEmpty;
        private AntdUI.Label labelEmpty;
        private AntdUI.Panel panelTagLastUpdate;
        private AntdUI.Label labelTagLastUpdateValue;
        private AntdUI.Label labelTagLastUpdateKey;
    }
}