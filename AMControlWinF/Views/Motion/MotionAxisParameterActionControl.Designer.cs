namespace AMControlWinF.Views.Motion
{
    partial class MotionAxisParameterActionControl
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
            this.panelApplyVelocityCard = new AntdUI.Panel();
            this.buttonApplyVelocity = new AntdUI.Button();
            this.inputVelocity = new AntdUI.Input();
            this.labelApplyVelocityTitle = new AntdUI.Label();
            this.labelApplyVelocityBadge = new AntdUI.Label();
            this.panelMoveAbsoluteCard = new AntdUI.Panel();
            this.buttonMoveAbsolute = new AntdUI.Button();
            this.inputTargetPosition = new AntdUI.Input();
            this.labelMoveAbsoluteTitle = new AntdUI.Label();
            this.labelMoveAbsoluteBadge = new AntdUI.Label();
            this.panelMoveRelativeCard = new AntdUI.Panel();
            this.buttonMoveRelative = new AntdUI.Button();
            this.inputMoveDistance = new AntdUI.Input();
            this.labelMoveRelativeTitle = new AntdUI.Label();
            this.labelMoveRelativeBadge = new AntdUI.Label();
            this.panelSpacer = new AntdUI.Panel();
            this.panelRoot.SuspendLayout();
            this.tableMain.SuspendLayout();
            this.panelApplyVelocityCard.SuspendLayout();
            this.panelMoveAbsoluteCard.SuspendLayout();
            this.panelMoveRelativeCard.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.tableMain);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Margin = new System.Windows.Forms.Padding(0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(4, 8, 4, 0);
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(320, 220);
            this.panelRoot.TabIndex = 0;
            // 
            // tableMain
            // 
            this.tableMain.ColumnCount = 2;
            this.tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableMain.Controls.Add(this.panelApplyVelocityCard, 0, 0);
            this.tableMain.Controls.Add(this.panelMoveAbsoluteCard, 1, 0);
            this.tableMain.Controls.Add(this.panelMoveRelativeCard, 0, 1);
            this.tableMain.Controls.Add(this.panelSpacer, 1, 1);
            this.tableMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableMain.Location = new System.Drawing.Point(8, 12);
            this.tableMain.Margin = new System.Windows.Forms.Padding(0);
            this.tableMain.Name = "tableMain";
            this.tableMain.RowCount = 2;
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableMain.Size = new System.Drawing.Size(304, 208);
            this.tableMain.TabIndex = 0;
            // 
            // panelApplyVelocityCard
            // 
            this.panelApplyVelocityCard.Controls.Add(this.buttonApplyVelocity);
            this.panelApplyVelocityCard.Controls.Add(this.inputVelocity);
            this.panelApplyVelocityCard.Controls.Add(this.labelApplyVelocityTitle);
            this.panelApplyVelocityCard.Controls.Add(this.labelApplyVelocityBadge);
            this.panelApplyVelocityCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelApplyVelocityCard.Location = new System.Drawing.Point(0, 0);
            this.panelApplyVelocityCard.Margin = new System.Windows.Forms.Padding(0, 0, 4, 4);
            this.panelApplyVelocityCard.Name = "panelApplyVelocityCard";
            this.panelApplyVelocityCard.Padding = new System.Windows.Forms.Padding(10, 10, 10, 8);
            this.panelApplyVelocityCard.Radius = 10;
            this.panelApplyVelocityCard.Shadow = 4;
            this.panelApplyVelocityCard.ShadowOpacity = 0.18F;
            this.panelApplyVelocityCard.Size = new System.Drawing.Size(148, 100);
            this.panelApplyVelocityCard.TabIndex = 0;
            // 
            // buttonApplyVelocity
            // 
            this.buttonApplyVelocity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonApplyVelocity.Location = new System.Drawing.Point(82, 64);
            this.buttonApplyVelocity.Margin = new System.Windows.Forms.Padding(0);
            this.buttonApplyVelocity.Name = "buttonApplyVelocity";
            this.buttonApplyVelocity.Radius = 6;
            this.buttonApplyVelocity.Size = new System.Drawing.Size(56, 28);
            this.buttonApplyVelocity.TabIndex = 3;
            this.buttonApplyVelocity.Text = "确认";
            this.buttonApplyVelocity.Type = AntdUI.TTypeMini.Primary;
            this.buttonApplyVelocity.WaveSize = 0;
            // 
            // inputVelocity
            // 
            this.inputVelocity.Location = new System.Drawing.Point(10, 34);
            this.inputVelocity.Margin = new System.Windows.Forms.Padding(0);
            this.inputVelocity.Name = "inputVelocity";
            this.inputVelocity.PlaceholderText = "速度";
            this.inputVelocity.Size = new System.Drawing.Size(128, 26);
            this.inputVelocity.TabIndex = 2;
            this.inputVelocity.Text = "10";
            this.inputVelocity.WaveSize = 0;
            // 
            // labelApplyVelocityTitle
            // 
            this.labelApplyVelocityTitle.Location = new System.Drawing.Point(10, 16);
            this.labelApplyVelocityTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelApplyVelocityTitle.Name = "labelApplyVelocityTitle";
            this.labelApplyVelocityTitle.Size = new System.Drawing.Size(128, 16);
            this.labelApplyVelocityTitle.TabIndex = 1;
            this.labelApplyVelocityTitle.Text = "应用速度";
            // 
            // labelApplyVelocityBadge
            // 
            this.labelApplyVelocityBadge.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(196)))), ((int)(((byte)(26)))));
            this.labelApplyVelocityBadge.ForeColor = System.Drawing.Color.White;
            this.labelApplyVelocityBadge.Location = new System.Drawing.Point(10, 0);
            this.labelApplyVelocityBadge.Margin = new System.Windows.Forms.Padding(0);
            this.labelApplyVelocityBadge.Name = "labelApplyVelocityBadge";
            this.labelApplyVelocityBadge.Size = new System.Drawing.Size(42, 18);
            this.labelApplyVelocityBadge.TabIndex = 0;
            this.labelApplyVelocityBadge.Text = "参数";
            this.labelApplyVelocityBadge.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelMoveAbsoluteCard
            // 
            this.panelMoveAbsoluteCard.Controls.Add(this.buttonMoveAbsolute);
            this.panelMoveAbsoluteCard.Controls.Add(this.inputTargetPosition);
            this.panelMoveAbsoluteCard.Controls.Add(this.labelMoveAbsoluteTitle);
            this.panelMoveAbsoluteCard.Controls.Add(this.labelMoveAbsoluteBadge);
            this.panelMoveAbsoluteCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMoveAbsoluteCard.Location = new System.Drawing.Point(156, 0);
            this.panelMoveAbsoluteCard.Margin = new System.Windows.Forms.Padding(4, 0, 0, 4);
            this.panelMoveAbsoluteCard.Name = "panelMoveAbsoluteCard";
            this.panelMoveAbsoluteCard.Padding = new System.Windows.Forms.Padding(10, 10, 10, 8);
            this.panelMoveAbsoluteCard.Radius = 10;
            this.panelMoveAbsoluteCard.Shadow = 4;
            this.panelMoveAbsoluteCard.ShadowOpacity = 0.18F;
            this.panelMoveAbsoluteCard.Size = new System.Drawing.Size(148, 100);
            this.panelMoveAbsoluteCard.TabIndex = 1;
            // 
            // buttonMoveAbsolute
            // 
            this.buttonMoveAbsolute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMoveAbsolute.Location = new System.Drawing.Point(82, 64);
            this.buttonMoveAbsolute.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMoveAbsolute.Name = "buttonMoveAbsolute";
            this.buttonMoveAbsolute.Radius = 6;
            this.buttonMoveAbsolute.Size = new System.Drawing.Size(56, 28);
            this.buttonMoveAbsolute.TabIndex = 3;
            this.buttonMoveAbsolute.Text = "确认";
            this.buttonMoveAbsolute.Type = AntdUI.TTypeMini.Primary;
            this.buttonMoveAbsolute.WaveSize = 0;
            // 
            // inputTargetPosition
            // 
            this.inputTargetPosition.Location = new System.Drawing.Point(10, 34);
            this.inputTargetPosition.Margin = new System.Windows.Forms.Padding(0);
            this.inputTargetPosition.Name = "inputTargetPosition";
            this.inputTargetPosition.PlaceholderText = "目标位置";
            this.inputTargetPosition.Size = new System.Drawing.Size(128, 26);
            this.inputTargetPosition.TabIndex = 2;
            this.inputTargetPosition.Text = "0";
            this.inputTargetPosition.WaveSize = 0;
            // 
            // labelMoveAbsoluteTitle
            // 
            this.labelMoveAbsoluteTitle.Location = new System.Drawing.Point(10, 16);
            this.labelMoveAbsoluteTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelMoveAbsoluteTitle.Name = "labelMoveAbsoluteTitle";
            this.labelMoveAbsoluteTitle.Size = new System.Drawing.Size(128, 16);
            this.labelMoveAbsoluteTitle.TabIndex = 1;
            this.labelMoveAbsoluteTitle.Text = "绝对定位";
            // 
            // labelMoveAbsoluteBadge
            // 
            this.labelMoveAbsoluteBadge.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(119)))), ((int)(((byte)(255)))));
            this.labelMoveAbsoluteBadge.ForeColor = System.Drawing.Color.White;
            this.labelMoveAbsoluteBadge.Location = new System.Drawing.Point(10, 0);
            this.labelMoveAbsoluteBadge.Margin = new System.Windows.Forms.Padding(0);
            this.labelMoveAbsoluteBadge.Name = "labelMoveAbsoluteBadge";
            this.labelMoveAbsoluteBadge.Size = new System.Drawing.Size(42, 18);
            this.labelMoveAbsoluteBadge.TabIndex = 0;
            this.labelMoveAbsoluteBadge.Text = "定位";
            this.labelMoveAbsoluteBadge.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelMoveRelativeCard
            // 
            this.panelMoveRelativeCard.Controls.Add(this.buttonMoveRelative);
            this.panelMoveRelativeCard.Controls.Add(this.inputMoveDistance);
            this.panelMoveRelativeCard.Controls.Add(this.labelMoveRelativeTitle);
            this.panelMoveRelativeCard.Controls.Add(this.labelMoveRelativeBadge);
            this.panelMoveRelativeCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMoveRelativeCard.Location = new System.Drawing.Point(0, 108);
            this.panelMoveRelativeCard.Margin = new System.Windows.Forms.Padding(0, 4, 4, 0);
            this.panelMoveRelativeCard.Name = "panelMoveRelativeCard";
            this.panelMoveRelativeCard.Padding = new System.Windows.Forms.Padding(10, 10, 10, 8);
            this.panelMoveRelativeCard.Radius = 10;
            this.panelMoveRelativeCard.Shadow = 4;
            this.panelMoveRelativeCard.ShadowOpacity = 0.18F;
            this.panelMoveRelativeCard.Size = new System.Drawing.Size(148, 100);
            this.panelMoveRelativeCard.TabIndex = 2;
            // 
            // buttonMoveRelative
            // 
            this.buttonMoveRelative.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMoveRelative.Location = new System.Drawing.Point(82, 64);
            this.buttonMoveRelative.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMoveRelative.Name = "buttonMoveRelative";
            this.buttonMoveRelative.Radius = 6;
            this.buttonMoveRelative.Size = new System.Drawing.Size(56, 28);
            this.buttonMoveRelative.TabIndex = 3;
            this.buttonMoveRelative.Text = "确认";
            this.buttonMoveRelative.Type = AntdUI.TTypeMini.Primary;
            this.buttonMoveRelative.WaveSize = 0;
            // 
            // inputMoveDistance
            // 
            this.inputMoveDistance.Location = new System.Drawing.Point(10, 34);
            this.inputMoveDistance.Margin = new System.Windows.Forms.Padding(0);
            this.inputMoveDistance.Name = "inputMoveDistance";
            this.inputMoveDistance.PlaceholderText = "相对距离";
            this.inputMoveDistance.Size = new System.Drawing.Size(128, 26);
            this.inputMoveDistance.TabIndex = 2;
            this.inputMoveDistance.Text = "10";
            this.inputMoveDistance.WaveSize = 0;
            // 
            // labelMoveRelativeTitle
            // 
            this.labelMoveRelativeTitle.Location = new System.Drawing.Point(10, 16);
            this.labelMoveRelativeTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelMoveRelativeTitle.Name = "labelMoveRelativeTitle";
            this.labelMoveRelativeTitle.Size = new System.Drawing.Size(128, 16);
            this.labelMoveRelativeTitle.TabIndex = 1;
            this.labelMoveRelativeTitle.Text = "相对移动";
            // 
            // labelMoveRelativeBadge
            // 
            this.labelMoveRelativeBadge.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(119)))), ((int)(((byte)(255)))));
            this.labelMoveRelativeBadge.ForeColor = System.Drawing.Color.White;
            this.labelMoveRelativeBadge.Location = new System.Drawing.Point(10, 0);
            this.labelMoveRelativeBadge.Margin = new System.Windows.Forms.Padding(0);
            this.labelMoveRelativeBadge.Name = "labelMoveRelativeBadge";
            this.labelMoveRelativeBadge.Size = new System.Drawing.Size(42, 18);
            this.labelMoveRelativeBadge.TabIndex = 0;
            this.labelMoveRelativeBadge.Text = "定位";
            this.labelMoveRelativeBadge.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelSpacer
            // 
            this.panelSpacer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSpacer.Location = new System.Drawing.Point(156, 108);
            this.panelSpacer.Margin = new System.Windows.Forms.Padding(4, 4, 0, 0);
            this.panelSpacer.Name = "panelSpacer";
            this.panelSpacer.Radius = 0;
            this.panelSpacer.Size = new System.Drawing.Size(148, 100);
            this.panelSpacer.TabIndex = 3;
            // 
            // MotionAxisParameterActionControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MotionAxisParameterActionControl";
            this.Size = new System.Drawing.Size(320, 220);
            this.panelRoot.ResumeLayout(false);
            this.tableMain.ResumeLayout(false);
            this.panelApplyVelocityCard.ResumeLayout(false);
            this.panelMoveAbsoluteCard.ResumeLayout(false);
            this.panelMoveRelativeCard.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private AntdUI.Panel panelRoot;
        private System.Windows.Forms.TableLayoutPanel tableMain;
        private AntdUI.Panel panelApplyVelocityCard;
        private AntdUI.Label labelApplyVelocityBadge;
        private AntdUI.Label labelApplyVelocityTitle;
        private AntdUI.Input inputVelocity;
        private AntdUI.Button buttonApplyVelocity;
        private AntdUI.Panel panelMoveAbsoluteCard;
        private AntdUI.Label labelMoveAbsoluteBadge;
        private AntdUI.Label labelMoveAbsoluteTitle;
        private AntdUI.Input inputTargetPosition;
        private AntdUI.Button buttonMoveAbsolute;
        private AntdUI.Panel panelMoveRelativeCard;
        private AntdUI.Label labelMoveRelativeBadge;
        private AntdUI.Label labelMoveRelativeTitle;
        private AntdUI.Input inputMoveDistance;
        private AntdUI.Button buttonMoveRelative;
        private AntdUI.Panel panelSpacer;
    }
}