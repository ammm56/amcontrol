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
            this.panelCard = new AntdUI.Panel();
            this.buttonConfirm = new AntdUI.Button();
            this.inputValue = new AntdUI.Input();
            this.labelBadge = new AntdUI.Label();
            this.panelRoot.SuspendLayout();
            this.panelCard.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.panelCard);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Margin = new System.Windows.Forms.Padding(0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(4);
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(220, 124);
            this.panelRoot.TabIndex = 0;
            // 
            // panelCard
            // 
            this.panelCard.Controls.Add(this.buttonConfirm);
            this.panelCard.Controls.Add(this.inputValue);
            this.panelCard.Controls.Add(this.labelBadge);
            this.panelCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCard.Location = new System.Drawing.Point(4, 4);
            this.panelCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelCard.Name = "panelCard";
            this.panelCard.Padding = new System.Windows.Forms.Padding(10, 10, 10, 8);
            this.panelCard.Radius = 10;
            this.panelCard.Shadow = 4;
            this.panelCard.ShadowOpacity = 0.18F;
            this.panelCard.Size = new System.Drawing.Size(212, 116);
            this.panelCard.TabIndex = 0;
            // 
            // buttonConfirm
            // 
            this.buttonConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConfirm.Location = new System.Drawing.Point(146, 77);
            this.buttonConfirm.Margin = new System.Windows.Forms.Padding(0);
            this.buttonConfirm.Name = "buttonConfirm";
            this.buttonConfirm.Size = new System.Drawing.Size(56, 28);
            this.buttonConfirm.TabIndex = 3;
            this.buttonConfirm.Text = "确认";
            this.buttonConfirm.Type = AntdUI.TTypeMini.Primary;
            this.buttonConfirm.WaveSize = 0;
            // 
            // inputValue
            // 
            this.inputValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inputValue.Location = new System.Drawing.Point(10, 42);
            this.inputValue.Margin = new System.Windows.Forms.Padding(0);
            this.inputValue.Name = "inputValue";
            this.inputValue.PlaceholderText = "请输入参数";
            this.inputValue.Size = new System.Drawing.Size(192, 30);
            this.inputValue.TabIndex = 2;
            this.inputValue.WaveSize = 0;
            // 
            // labelBadge
            // 
            this.labelBadge.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(119)))), ((int)(((byte)(255)))));
            this.labelBadge.ForeColor = System.Drawing.Color.White;
            this.labelBadge.Location = new System.Drawing.Point(14, 14);
            this.labelBadge.Margin = new System.Windows.Forms.Padding(0);
            this.labelBadge.Name = "labelBadge";
            this.labelBadge.Size = new System.Drawing.Size(42, 18);
            this.labelBadge.TabIndex = 0;
            this.labelBadge.Text = "参数";
            this.labelBadge.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MotionAxisParameterActionControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MotionAxisParameterActionControl";
            this.Size = new System.Drawing.Size(220, 124);
            this.panelRoot.ResumeLayout(false);
            this.panelCard.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AntdUI.Panel panelRoot;
        private AntdUI.Panel panelCard;
        private AntdUI.Label labelBadge;
        private AntdUI.Input inputValue;
        private AntdUI.Button buttonConfirm;
    }
}