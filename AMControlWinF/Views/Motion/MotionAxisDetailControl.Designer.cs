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
            this.inputMoveDistance = new AntdUI.Input();
            this.labelMoveDistanceTitle = new AntdUI.Label();
            this.inputTargetPosition = new AntdUI.Input();
            this.labelTargetPositionTitle = new AntdUI.Label();
            this.inputVelocity = new AntdUI.Input();
            this.labelVelocityTitle = new AntdUI.Label();
            this.labelParamHint = new AntdUI.Label();
            this.panelTagAxisJogVel = new AntdUI.Panel();
            this.labelTagAxisJogVelValue = new AntdUI.Label();
            this.labelTagAxisJogVelKey = new AntdUI.Label();
            this.panelTagAxisDefaultVel = new AntdUI.Panel();
            this.labelTagAxisDefaultVelValue = new AntdUI.Label();
            this.labelTagAxisDefaultVelKey = new AntdUI.Label();
            this.panelTagAxisEncoderMm = new AntdUI.Panel();
            this.labelTagAxisEncoderMmValue = new AntdUI.Label();
            this.labelTagAxisEncoderMmKey = new AntdUI.Label();
            this.panelTagAxisCommandMm = new AntdUI.Panel();
            this.labelTagAxisCommandMmValue = new AntdUI.Label();
            this.labelTagAxisCommandMmKey = new AntdUI.Label();
            this.panelTagAxisLimit = new AntdUI.Panel();
            this.labelTagAxisLimitValue = new AntdUI.Label();
            this.labelTagAxisLimitKey = new AntdUI.Label();
            this.panelTagAxisDone = new AntdUI.Panel();
            this.labelTagAxisDoneValue = new AntdUI.Label();
            this.labelTagAxisDoneKey = new AntdUI.Label();
            this.panelTagAxisHome = new AntdUI.Panel();
            this.labelTagAxisHomeValue = new AntdUI.Label();
            this.labelTagAxisHomeKey = new AntdUI.Label();
            this.panelTagAxisEnable = new AntdUI.Panel();
            this.labelTagAxisEnableValue = new AntdUI.Label();
            this.labelTagAxisEnableKey = new AntdUI.Label();
            this.panelTagAxisState = new AntdUI.Panel();
            this.labelTagAxisStateValue = new AntdUI.Label();
            this.labelTagAxisStateKey = new AntdUI.Label();
            this.panelTagAxisPhysical = new AntdUI.Panel();
            this.labelTagAxisPhysicalValue = new AntdUI.Label();
            this.labelTagAxisPhysicalKey = new AntdUI.Label();
            this.panelTagAxisType = new AntdUI.Panel();
            this.labelTagAxisTypeValue = new AntdUI.Label();
            this.labelTagAxisTypeKey = new AntdUI.Label();
            this.panelTagAxisLogic = new AntdUI.Panel();
            this.labelTagAxisLogicValue = new AntdUI.Label();
            this.labelTagAxisLogicKey = new AntdUI.Label();
            this.panelHeader = new AntdUI.Panel();
            this.labelSubTitle = new AntdUI.Label();
            this.labelTitle = new AntdUI.Label();
            this.panelEmpty = new AntdUI.Panel();
            this.labelEmpty = new AntdUI.Label();
            this.panelRoot.SuspendLayout();
            this.panelDetail.SuspendLayout();
            this.panelScroll.SuspendLayout();
            this.panelTagAxisJogVel.SuspendLayout();
            this.panelTagAxisDefaultVel.SuspendLayout();
            this.panelTagAxisEncoderMm.SuspendLayout();
            this.panelTagAxisCommandMm.SuspendLayout();
            this.panelTagAxisLimit.SuspendLayout();
            this.panelTagAxisDone.SuspendLayout();
            this.panelTagAxisHome.SuspendLayout();
            this.panelTagAxisEnable.SuspendLayout();
            this.panelTagAxisState.SuspendLayout();
            this.panelTagAxisPhysical.SuspendLayout();
            this.panelTagAxisType.SuspendLayout();
            this.panelTagAxisLogic.SuspendLayout();
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
            this.panelRoot.Size = new System.Drawing.Size(280, 520);
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
            this.panelDetail.Size = new System.Drawing.Size(280, 520);
            this.panelDetail.TabIndex = 1;
            this.panelDetail.Visible = false;
            // 
            // panelScroll
            // 
            this.panelScroll.AutoScroll = true;
            this.panelScroll.Controls.Add(this.inputMoveDistance);
            this.panelScroll.Controls.Add(this.labelMoveDistanceTitle);
            this.panelScroll.Controls.Add(this.inputTargetPosition);
            this.panelScroll.Controls.Add(this.labelTargetPositionTitle);
            this.panelScroll.Controls.Add(this.inputVelocity);
            this.panelScroll.Controls.Add(this.labelVelocityTitle);
            this.panelScroll.Controls.Add(this.labelParamHint);
            this.panelScroll.Controls.Add(this.panelTagAxisJogVel);
            this.panelScroll.Controls.Add(this.panelTagAxisDefaultVel);
            this.panelScroll.Controls.Add(this.panelTagAxisEncoderMm);
            this.panelScroll.Controls.Add(this.panelTagAxisCommandMm);
            this.panelScroll.Controls.Add(this.panelTagAxisLimit);
            this.panelScroll.Controls.Add(this.panelTagAxisDone);
            this.panelScroll.Controls.Add(this.panelTagAxisHome);
            this.panelScroll.Controls.Add(this.panelTagAxisEnable);
            this.panelScroll.Controls.Add(this.panelTagAxisState);
            this.panelScroll.Controls.Add(this.panelTagAxisPhysical);
            this.panelScroll.Controls.Add(this.panelTagAxisType);
            this.panelScroll.Controls.Add(this.panelTagAxisLogic);
            this.panelScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelScroll.Location = new System.Drawing.Point(0, 60);
            this.panelScroll.Margin = new System.Windows.Forms.Padding(0);
            this.panelScroll.Name = "panelScroll";
            this.panelScroll.Padding = new System.Windows.Forms.Padding(14, 12, 14, 12);
            this.panelScroll.Size = new System.Drawing.Size(280, 460);
            this.panelScroll.TabIndex = 1;
            // 
            // inputMoveDistance
            // 
            this.inputMoveDistance.Location = new System.Drawing.Point(8, 470);
            this.inputMoveDistance.Margin = new System.Windows.Forms.Padding(0);
            this.inputMoveDistance.Name = "inputMoveDistance";
            this.inputMoveDistance.Size = new System.Drawing.Size(252, 36);
            this.inputMoveDistance.TabIndex = 18;
            this.inputMoveDistance.Text = "10";
            this.inputMoveDistance.WaveSize = 0;
            // 
            // labelMoveDistanceTitle
            // 
            this.labelMoveDistanceTitle.ForeColor = System.Drawing.Color.Gray;
            this.labelMoveDistanceTitle.Location = new System.Drawing.Point(8, 446);
            this.labelMoveDistanceTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelMoveDistanceTitle.Name = "labelMoveDistanceTitle";
            this.labelMoveDistanceTitle.Size = new System.Drawing.Size(252, 20);
            this.labelMoveDistanceTitle.TabIndex = 17;
            this.labelMoveDistanceTitle.Text = "相对距离(mm)";
            // 
            // inputTargetPosition
            // 
            this.inputTargetPosition.Location = new System.Drawing.Point(8, 396);
            this.inputTargetPosition.Margin = new System.Windows.Forms.Padding(0);
            this.inputTargetPosition.Name = "inputTargetPosition";
            this.inputTargetPosition.Size = new System.Drawing.Size(252, 36);
            this.inputTargetPosition.TabIndex = 16;
            this.inputTargetPosition.Text = "0";
            this.inputTargetPosition.WaveSize = 0;
            // 
            // labelTargetPositionTitle
            // 
            this.labelTargetPositionTitle.ForeColor = System.Drawing.Color.Gray;
            this.labelTargetPositionTitle.Location = new System.Drawing.Point(8, 372);
            this.labelTargetPositionTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTargetPositionTitle.Name = "labelTargetPositionTitle";
            this.labelTargetPositionTitle.Size = new System.Drawing.Size(252, 20);
            this.labelTargetPositionTitle.TabIndex = 15;
            this.labelTargetPositionTitle.Text = "目标位置(mm)";
            // 
            // inputVelocity
            // 
            this.inputVelocity.Location = new System.Drawing.Point(8, 322);
            this.inputVelocity.Margin = new System.Windows.Forms.Padding(0);
            this.inputVelocity.Name = "inputVelocity";
            this.inputVelocity.Size = new System.Drawing.Size(252, 36);
            this.inputVelocity.TabIndex = 14;
            this.inputVelocity.Text = "10";
            this.inputVelocity.WaveSize = 0;
            // 
            // labelVelocityTitle
            // 
            this.labelVelocityTitle.ForeColor = System.Drawing.Color.Gray;
            this.labelVelocityTitle.Location = new System.Drawing.Point(8, 298);
            this.labelVelocityTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelVelocityTitle.Name = "labelVelocityTitle";
            this.labelVelocityTitle.Size = new System.Drawing.Size(252, 20);
            this.labelVelocityTitle.TabIndex = 13;
            this.labelVelocityTitle.Text = "速度(mm/s)";
            // 
            // labelParamHint
            // 
            this.labelParamHint.ForeColor = System.Drawing.Color.Gray;
            this.labelParamHint.Location = new System.Drawing.Point(8, 266);
            this.labelParamHint.Margin = new System.Windows.Forms.Padding(0);
            this.labelParamHint.Name = "labelParamHint";
            this.labelParamHint.Size = new System.Drawing.Size(252, 28);
            this.labelParamHint.TabIndex = 12;
            this.labelParamHint.Text = "参数区供应用速度、绝对定位、相对定位和点动动作使用";
            // 
            // panelTagAxisJogVel
            // 
            this.panelTagAxisJogVel.Controls.Add(this.labelTagAxisJogVelValue);
            this.panelTagAxisJogVel.Controls.Add(this.labelTagAxisJogVelKey);
            this.panelTagAxisJogVel.Location = new System.Drawing.Point(8, 234);
            this.panelTagAxisJogVel.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagAxisJogVel.Name = "panelTagAxisJogVel";
            this.panelTagAxisJogVel.Radius = 0;
            this.panelTagAxisJogVel.Size = new System.Drawing.Size(252, 24);
            this.panelTagAxisJogVel.TabIndex = 11;
            // 
            // labelTagAxisJogVelValue
            // 
            this.labelTagAxisJogVelValue.AutoEllipsis = true;
            this.labelTagAxisJogVelValue.Location = new System.Drawing.Point(94, 0);
            this.labelTagAxisJogVelValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagAxisJogVelValue.Name = "labelTagAxisJogVelValue";
            this.labelTagAxisJogVelValue.Size = new System.Drawing.Size(150, 24);
            this.labelTagAxisJogVelValue.TabIndex = 1;
            this.labelTagAxisJogVelValue.Text = "—";
            // 
            // labelTagAxisJogVelKey
            // 
            this.labelTagAxisJogVelKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagAxisJogVelKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagAxisJogVelKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagAxisJogVelKey.Name = "labelTagAxisJogVelKey";
            this.labelTagAxisJogVelKey.Size = new System.Drawing.Size(88, 24);
            this.labelTagAxisJogVelKey.TabIndex = 0;
            this.labelTagAxisJogVelKey.Text = "点动速度";
            this.labelTagAxisJogVelKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagAxisDefaultVel
            // 
            this.panelTagAxisDefaultVel.Controls.Add(this.labelTagAxisDefaultVelValue);
            this.panelTagAxisDefaultVel.Controls.Add(this.labelTagAxisDefaultVelKey);
            this.panelTagAxisDefaultVel.Location = new System.Drawing.Point(8, 204);
            this.panelTagAxisDefaultVel.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagAxisDefaultVel.Name = "panelTagAxisDefaultVel";
            this.panelTagAxisDefaultVel.Radius = 0;
            this.panelTagAxisDefaultVel.Size = new System.Drawing.Size(252, 24);
            this.panelTagAxisDefaultVel.TabIndex = 10;
            // 
            // labelTagAxisDefaultVelValue
            // 
            this.labelTagAxisDefaultVelValue.AutoEllipsis = true;
            this.labelTagAxisDefaultVelValue.Location = new System.Drawing.Point(94, 0);
            this.labelTagAxisDefaultVelValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagAxisDefaultVelValue.Name = "labelTagAxisDefaultVelValue";
            this.labelTagAxisDefaultVelValue.Size = new System.Drawing.Size(150, 24);
            this.labelTagAxisDefaultVelValue.TabIndex = 1;
            this.labelTagAxisDefaultVelValue.Text = "—";
            // 
            // labelTagAxisDefaultVelKey
            // 
            this.labelTagAxisDefaultVelKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagAxisDefaultVelKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagAxisDefaultVelKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagAxisDefaultVelKey.Name = "labelTagAxisDefaultVelKey";
            this.labelTagAxisDefaultVelKey.Size = new System.Drawing.Size(88, 24);
            this.labelTagAxisDefaultVelKey.TabIndex = 0;
            this.labelTagAxisDefaultVelKey.Text = "默认速度";
            this.labelTagAxisDefaultVelKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagAxisEncoderMm
            // 
            this.panelTagAxisEncoderMm.Controls.Add(this.labelTagAxisEncoderMmValue);
            this.panelTagAxisEncoderMm.Controls.Add(this.labelTagAxisEncoderMmKey);
            this.panelTagAxisEncoderMm.Location = new System.Drawing.Point(8, 174);
            this.panelTagAxisEncoderMm.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagAxisEncoderMm.Name = "panelTagAxisEncoderMm";
            this.panelTagAxisEncoderMm.Radius = 0;
            this.panelTagAxisEncoderMm.Size = new System.Drawing.Size(252, 24);
            this.panelTagAxisEncoderMm.TabIndex = 9;
            // 
            // labelTagAxisEncoderMmValue
            // 
            this.labelTagAxisEncoderMmValue.AutoEllipsis = true;
            this.labelTagAxisEncoderMmValue.Location = new System.Drawing.Point(94, 0);
            this.labelTagAxisEncoderMmValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagAxisEncoderMmValue.Name = "labelTagAxisEncoderMmValue";
            this.labelTagAxisEncoderMmValue.Size = new System.Drawing.Size(150, 24);
            this.labelTagAxisEncoderMmValue.TabIndex = 1;
            this.labelTagAxisEncoderMmValue.Text = "—";
            // 
            // labelTagAxisEncoderMmKey
            // 
            this.labelTagAxisEncoderMmKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagAxisEncoderMmKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagAxisEncoderMmKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagAxisEncoderMmKey.Name = "labelTagAxisEncoderMmKey";
            this.labelTagAxisEncoderMmKey.Size = new System.Drawing.Size(88, 24);
            this.labelTagAxisEncoderMmKey.TabIndex = 0;
            this.labelTagAxisEncoderMmKey.Text = "编码器位置";
            this.labelTagAxisEncoderMmKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagAxisCommandMm
            // 
            this.panelTagAxisCommandMm.Controls.Add(this.labelTagAxisCommandMmValue);
            this.panelTagAxisCommandMm.Controls.Add(this.labelTagAxisCommandMmKey);
            this.panelTagAxisCommandMm.Location = new System.Drawing.Point(8, 144);
            this.panelTagAxisCommandMm.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagAxisCommandMm.Name = "panelTagAxisCommandMm";
            this.panelTagAxisCommandMm.Radius = 0;
            this.panelTagAxisCommandMm.Size = new System.Drawing.Size(252, 24);
            this.panelTagAxisCommandMm.TabIndex = 8;
            // 
            // labelTagAxisCommandMmValue
            // 
            this.labelTagAxisCommandMmValue.AutoEllipsis = true;
            this.labelTagAxisCommandMmValue.Location = new System.Drawing.Point(94, 0);
            this.labelTagAxisCommandMmValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagAxisCommandMmValue.Name = "labelTagAxisCommandMmValue";
            this.labelTagAxisCommandMmValue.Size = new System.Drawing.Size(150, 24);
            this.labelTagAxisCommandMmValue.TabIndex = 1;
            this.labelTagAxisCommandMmValue.Text = "—";
            // 
            // labelTagAxisCommandMmKey
            // 
            this.labelTagAxisCommandMmKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagAxisCommandMmKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagAxisCommandMmKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagAxisCommandMmKey.Name = "labelTagAxisCommandMmKey";
            this.labelTagAxisCommandMmKey.Size = new System.Drawing.Size(88, 24);
            this.labelTagAxisCommandMmKey.TabIndex = 0;
            this.labelTagAxisCommandMmKey.Text = "指令位置";
            this.labelTagAxisCommandMmKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagAxisLimit
            // 
            this.panelTagAxisLimit.Controls.Add(this.labelTagAxisLimitValue);
            this.panelTagAxisLimit.Controls.Add(this.labelTagAxisLimitKey);
            this.panelTagAxisLimit.Location = new System.Drawing.Point(8, 114);
            this.panelTagAxisLimit.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagAxisLimit.Name = "panelTagAxisLimit";
            this.panelTagAxisLimit.Radius = 0;
            this.panelTagAxisLimit.Size = new System.Drawing.Size(252, 24);
            this.panelTagAxisLimit.TabIndex = 7;
            // 
            // labelTagAxisLimitValue
            // 
            this.labelTagAxisLimitValue.AutoEllipsis = true;
            this.labelTagAxisLimitValue.Location = new System.Drawing.Point(94, 0);
            this.labelTagAxisLimitValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagAxisLimitValue.Name = "labelTagAxisLimitValue";
            this.labelTagAxisLimitValue.Size = new System.Drawing.Size(150, 24);
            this.labelTagAxisLimitValue.TabIndex = 1;
            this.labelTagAxisLimitValue.Text = "—";
            // 
            // labelTagAxisLimitKey
            // 
            this.labelTagAxisLimitKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagAxisLimitKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagAxisLimitKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagAxisLimitKey.Name = "labelTagAxisLimitKey";
            this.labelTagAxisLimitKey.Size = new System.Drawing.Size(88, 24);
            this.labelTagAxisLimitKey.TabIndex = 0;
            this.labelTagAxisLimitKey.Text = "限位状态";
            this.labelTagAxisLimitKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagAxisDone
            // 
            this.panelTagAxisDone.Controls.Add(this.labelTagAxisDoneValue);
            this.panelTagAxisDone.Controls.Add(this.labelTagAxisDoneKey);
            this.panelTagAxisDone.Location = new System.Drawing.Point(8, 84);
            this.panelTagAxisDone.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagAxisDone.Name = "panelTagAxisDone";
            this.panelTagAxisDone.Radius = 0;
            this.panelTagAxisDone.Size = new System.Drawing.Size(252, 24);
            this.panelTagAxisDone.TabIndex = 6;
            // 
            // labelTagAxisDoneValue
            // 
            this.labelTagAxisDoneValue.AutoEllipsis = true;
            this.labelTagAxisDoneValue.Location = new System.Drawing.Point(94, 0);
            this.labelTagAxisDoneValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagAxisDoneValue.Name = "labelTagAxisDoneValue";
            this.labelTagAxisDoneValue.Size = new System.Drawing.Size(150, 24);
            this.labelTagAxisDoneValue.TabIndex = 1;
            this.labelTagAxisDoneValue.Text = "—";
            // 
            // labelTagAxisDoneKey
            // 
            this.labelTagAxisDoneKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagAxisDoneKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagAxisDoneKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagAxisDoneKey.Name = "labelTagAxisDoneKey";
            this.labelTagAxisDoneKey.Size = new System.Drawing.Size(88, 24);
            this.labelTagAxisDoneKey.TabIndex = 0;
            this.labelTagAxisDoneKey.Text = "到位状态";
            this.labelTagAxisDoneKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagAxisHome
            // 
            this.panelTagAxisHome.Controls.Add(this.labelTagAxisHomeValue);
            this.panelTagAxisHome.Controls.Add(this.labelTagAxisHomeKey);
            this.panelTagAxisHome.Location = new System.Drawing.Point(8, 54);
            this.panelTagAxisHome.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagAxisHome.Name = "panelTagAxisHome";
            this.panelTagAxisHome.Radius = 0;
            this.panelTagAxisHome.Size = new System.Drawing.Size(252, 24);
            this.panelTagAxisHome.TabIndex = 5;
            // 
            // labelTagAxisHomeValue
            // 
            this.labelTagAxisHomeValue.AutoEllipsis = true;
            this.labelTagAxisHomeValue.Location = new System.Drawing.Point(94, 0);
            this.labelTagAxisHomeValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagAxisHomeValue.Name = "labelTagAxisHomeValue";
            this.labelTagAxisHomeValue.Size = new System.Drawing.Size(150, 24);
            this.labelTagAxisHomeValue.TabIndex = 1;
            this.labelTagAxisHomeValue.Text = "—";
            // 
            // labelTagAxisHomeKey
            // 
            this.labelTagAxisHomeKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagAxisHomeKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagAxisHomeKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagAxisHomeKey.Name = "labelTagAxisHomeKey";
            this.labelTagAxisHomeKey.Size = new System.Drawing.Size(88, 24);
            this.labelTagAxisHomeKey.TabIndex = 0;
            this.labelTagAxisHomeKey.Text = "原点状态";
            this.labelTagAxisHomeKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagAxisEnable
            // 
            this.panelTagAxisEnable.Controls.Add(this.labelTagAxisEnableValue);
            this.panelTagAxisEnable.Controls.Add(this.labelTagAxisEnableKey);
            this.panelTagAxisEnable.Location = new System.Drawing.Point(8, 144);
            this.panelTagAxisEnable.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagAxisEnable.Name = "panelTagAxisEnable";
            this.panelTagAxisEnable.Radius = 0;
            this.panelTagAxisEnable.Size = new System.Drawing.Size(252, 24);
            this.panelTagAxisEnable.TabIndex = 4;
            // 
            // labelTagAxisEnableValue
            // 
            this.labelTagAxisEnableValue.AutoEllipsis = true;
            this.labelTagAxisEnableValue.Location = new System.Drawing.Point(94, 0);
            this.labelTagAxisEnableValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagAxisEnableValue.Name = "labelTagAxisEnableValue";
            this.labelTagAxisEnableValue.Size = new System.Drawing.Size(150, 24);
            this.labelTagAxisEnableValue.TabIndex = 1;
            this.labelTagAxisEnableValue.Text = "—";
            // 
            // labelTagAxisEnableKey
            // 
            this.labelTagAxisEnableKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagAxisEnableKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagAxisEnableKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagAxisEnableKey.Name = "labelTagAxisEnableKey";
            this.labelTagAxisEnableKey.Size = new System.Drawing.Size(88, 24);
            this.labelTagAxisEnableKey.TabIndex = 0;
            this.labelTagAxisEnableKey.Text = "使能状态";
            this.labelTagAxisEnableKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagAxisState
            // 
            this.panelTagAxisState.Controls.Add(this.labelTagAxisStateValue);
            this.panelTagAxisState.Controls.Add(this.labelTagAxisStateKey);
            this.panelTagAxisState.Location = new System.Drawing.Point(8, 114);
            this.panelTagAxisState.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagAxisState.Name = "panelTagAxisState";
            this.panelTagAxisState.Radius = 0;
            this.panelTagAxisState.Size = new System.Drawing.Size(252, 24);
            this.panelTagAxisState.TabIndex = 3;
            // 
            // labelTagAxisStateValue
            // 
            this.labelTagAxisStateValue.AutoEllipsis = true;
            this.labelTagAxisStateValue.Location = new System.Drawing.Point(94, 0);
            this.labelTagAxisStateValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagAxisStateValue.Name = "labelTagAxisStateValue";
            this.labelTagAxisStateValue.Size = new System.Drawing.Size(150, 24);
            this.labelTagAxisStateValue.TabIndex = 1;
            this.labelTagAxisStateValue.Text = "—";
            // 
            // labelTagAxisStateKey
            // 
            this.labelTagAxisStateKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagAxisStateKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagAxisStateKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagAxisStateKey.Name = "labelTagAxisStateKey";
            this.labelTagAxisStateKey.Size = new System.Drawing.Size(88, 24);
            this.labelTagAxisStateKey.TabIndex = 0;
            this.labelTagAxisStateKey.Text = "当前状态";
            this.labelTagAxisStateKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagAxisPhysical
            // 
            this.panelTagAxisPhysical.Controls.Add(this.labelTagAxisPhysicalValue);
            this.panelTagAxisPhysical.Controls.Add(this.labelTagAxisPhysicalKey);
            this.panelTagAxisPhysical.Location = new System.Drawing.Point(8, 84);
            this.panelTagAxisPhysical.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagAxisPhysical.Name = "panelTagAxisPhysical";
            this.panelTagAxisPhysical.Radius = 0;
            this.panelTagAxisPhysical.Size = new System.Drawing.Size(252, 24);
            this.panelTagAxisPhysical.TabIndex = 2;
            // 
            // labelTagAxisPhysicalValue
            // 
            this.labelTagAxisPhysicalValue.AutoEllipsis = true;
            this.labelTagAxisPhysicalValue.Location = new System.Drawing.Point(94, 0);
            this.labelTagAxisPhysicalValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagAxisPhysicalValue.Name = "labelTagAxisPhysicalValue";
            this.labelTagAxisPhysicalValue.Size = new System.Drawing.Size(150, 24);
            this.labelTagAxisPhysicalValue.TabIndex = 1;
            this.labelTagAxisPhysicalValue.Text = "—";
            // 
            // labelTagAxisPhysicalKey
            // 
            this.labelTagAxisPhysicalKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagAxisPhysicalKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagAxisPhysicalKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagAxisPhysicalKey.Name = "labelTagAxisPhysicalKey";
            this.labelTagAxisPhysicalKey.Size = new System.Drawing.Size(88, 24);
            this.labelTagAxisPhysicalKey.TabIndex = 0;
            this.labelTagAxisPhysicalKey.Text = "物理映射";
            this.labelTagAxisPhysicalKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagAxisType
            // 
            this.panelTagAxisType.Controls.Add(this.labelTagAxisTypeValue);
            this.panelTagAxisType.Controls.Add(this.labelTagAxisTypeKey);
            this.panelTagAxisType.Location = new System.Drawing.Point(8, 54);
            this.panelTagAxisType.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagAxisType.Name = "panelTagAxisType";
            this.panelTagAxisType.Radius = 0;
            this.panelTagAxisType.Size = new System.Drawing.Size(252, 24);
            this.panelTagAxisType.TabIndex = 1;
            // 
            // labelTagAxisTypeValue
            // 
            this.labelTagAxisTypeValue.AutoEllipsis = true;
            this.labelTagAxisTypeValue.Location = new System.Drawing.Point(94, 0);
            this.labelTagAxisTypeValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagAxisTypeValue.Name = "labelTagAxisTypeValue";
            this.labelTagAxisTypeValue.Size = new System.Drawing.Size(150, 24);
            this.labelTagAxisTypeValue.TabIndex = 1;
            this.labelTagAxisTypeValue.Text = "—";
            // 
            // labelTagAxisTypeKey
            // 
            this.labelTagAxisTypeKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagAxisTypeKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagAxisTypeKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagAxisTypeKey.Name = "labelTagAxisTypeKey";
            this.labelTagAxisTypeKey.Size = new System.Drawing.Size(88, 24);
            this.labelTagAxisTypeKey.TabIndex = 0;
            this.labelTagAxisTypeKey.Text = "轴类型";
            this.labelTagAxisTypeKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagAxisLogic
            // 
            this.panelTagAxisLogic.Controls.Add(this.labelTagAxisLogicValue);
            this.panelTagAxisLogic.Controls.Add(this.labelTagAxisLogicKey);
            this.panelTagAxisLogic.Location = new System.Drawing.Point(8, 24);
            this.panelTagAxisLogic.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagAxisLogic.Name = "panelTagAxisLogic";
            this.panelTagAxisLogic.Radius = 0;
            this.panelTagAxisLogic.Size = new System.Drawing.Size(252, 24);
            this.panelTagAxisLogic.TabIndex = 0;
            // 
            // labelTagAxisLogicValue
            // 
            this.labelTagAxisLogicValue.AutoEllipsis = true;
            this.labelTagAxisLogicValue.Location = new System.Drawing.Point(94, 0);
            this.labelTagAxisLogicValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagAxisLogicValue.Name = "labelTagAxisLogicValue";
            this.labelTagAxisLogicValue.Size = new System.Drawing.Size(150, 24);
            this.labelTagAxisLogicValue.TabIndex = 1;
            this.labelTagAxisLogicValue.Text = "—";
            // 
            // labelTagAxisLogicKey
            // 
            this.labelTagAxisLogicKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagAxisLogicKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagAxisLogicKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagAxisLogicKey.Name = "labelTagAxisLogicKey";
            this.labelTagAxisLogicKey.Size = new System.Drawing.Size(88, 24);
            this.labelTagAxisLogicKey.TabIndex = 0;
            this.labelTagAxisLogicKey.Text = "逻辑轴";
            this.labelTagAxisLogicKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.panelHeader.Size = new System.Drawing.Size(280, 60);
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
            this.labelSubTitle.Size = new System.Drawing.Size(264, 22);
            this.labelSubTitle.TabIndex = 1;
            this.labelSubTitle.Text = "当前轴：未选择";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoEllipsis = true;
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(8, 8);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(264, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "轴实时监视";
            // 
            // panelEmpty
            // 
            this.panelEmpty.Controls.Add(this.labelEmpty);
            this.panelEmpty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEmpty.Location = new System.Drawing.Point(0, 0);
            this.panelEmpty.Margin = new System.Windows.Forms.Padding(0);
            this.panelEmpty.Name = "panelEmpty";
            this.panelEmpty.Radius = 0;
            this.panelEmpty.Size = new System.Drawing.Size(280, 520);
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
            this.labelEmpty.Size = new System.Drawing.Size(280, 520);
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
            this.Size = new System.Drawing.Size(280, 520);
            this.panelRoot.ResumeLayout(false);
            this.panelDetail.ResumeLayout(false);
            this.panelScroll.ResumeLayout(false);
            this.panelTagAxisJogVel.ResumeLayout(false);
            this.panelTagAxisDefaultVel.ResumeLayout(false);
            this.panelTagAxisEncoderMm.ResumeLayout(false);
            this.panelTagAxisCommandMm.ResumeLayout(false);
            this.panelTagAxisLimit.ResumeLayout(false);
            this.panelTagAxisDone.ResumeLayout(false);
            this.panelTagAxisHome.ResumeLayout(false);
            this.panelTagAxisEnable.ResumeLayout(false);
            this.panelTagAxisState.ResumeLayout(false);
            this.panelTagAxisPhysical.ResumeLayout(false);
            this.panelTagAxisType.ResumeLayout(false);
            this.panelTagAxisLogic.ResumeLayout(false);
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
        private AntdUI.Panel panelTagAxisLogic;
        private AntdUI.Label labelTagAxisLogicKey;
        private AntdUI.Label labelTagAxisLogicValue;
        private AntdUI.Panel panelTagAxisType;
        private AntdUI.Label labelTagAxisTypeKey;
        private AntdUI.Label labelTagAxisTypeValue;
        private AntdUI.Panel panelTagAxisPhysical;
        private AntdUI.Label labelTagAxisPhysicalKey;
        private AntdUI.Label labelTagAxisPhysicalValue;
        private AntdUI.Panel panelTagAxisState;
        private AntdUI.Label labelTagAxisStateKey;
        private AntdUI.Label labelTagAxisStateValue;
        private AntdUI.Panel panelTagAxisEnable;
        private AntdUI.Label labelTagAxisEnableKey;
        private AntdUI.Label labelTagAxisEnableValue;
        private AntdUI.Panel panelTagAxisHome;
        private AntdUI.Label labelTagAxisHomeKey;
        private AntdUI.Label labelTagAxisHomeValue;
        private AntdUI.Panel panelTagAxisDone;
        private AntdUI.Label labelTagAxisDoneKey;
        private AntdUI.Label labelTagAxisDoneValue;
        private AntdUI.Panel panelTagAxisLimit;
        private AntdUI.Label labelTagAxisLimitKey;
        private AntdUI.Label labelTagAxisLimitValue;
        private AntdUI.Panel panelTagAxisCommandMm;
        private AntdUI.Label labelTagAxisCommandMmKey;
        private AntdUI.Label labelTagAxisCommandMmValue;
        private AntdUI.Panel panelTagAxisEncoderMm;
        private AntdUI.Label labelTagAxisEncoderMmKey;
        private AntdUI.Label labelTagAxisEncoderMmValue;
        private AntdUI.Panel panelTagAxisDefaultVel;
        private AntdUI.Label labelTagAxisDefaultVelKey;
        private AntdUI.Label labelTagAxisDefaultVelValue;
        private AntdUI.Panel panelTagAxisJogVel;
        private AntdUI.Label labelTagAxisJogVelKey;
        private AntdUI.Label labelTagAxisJogVelValue;
        private AntdUI.Label labelParamHint;
        private AntdUI.Label labelVelocityTitle;
        private AntdUI.Input inputVelocity;
        private AntdUI.Label labelTargetPositionTitle;
        private AntdUI.Input inputTargetPosition;
        private AntdUI.Label labelMoveDistanceTitle;
        private AntdUI.Input inputMoveDistance;
    }
}
