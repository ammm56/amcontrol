namespace AMControlWinF.Views.MotionConfig
{
    partial class MotionCardDetailDialog
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
            this.textureBackgroundDialog = new AMControlWinF.Views.Main.TextureBackgroundControl();
            this.panelShell = new AntdUI.Panel();
            this.panelContentHost = new AntdUI.Panel();
            this.motionCardDetailControl = new AMControlWinF.Views.MotionConfig.MotionCardDetailControl();
            this.panelHeader = new AntdUI.Panel();
            this.labelDialogDescription = new AntdUI.Label();
            this.labelDialogTitle = new AntdUI.Label();
            this.panelFooter = new AntdUI.Panel();
            this.flowFooterButtons = new AntdUI.FlowPanel();
            this.buttonCancel = new AntdUI.Button();
            this.textureBackgroundDialog.SuspendLayout();
            this.panelShell.SuspendLayout();
            this.panelContentHost.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.flowFooterButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // textureBackgroundDialog
            // 
            this.textureBackgroundDialog.Controls.Add(this.panelShell);
            this.textureBackgroundDialog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textureBackgroundDialog.Location = new System.Drawing.Point(0, 0);
            this.textureBackgroundDialog.Margin = new System.Windows.Forms.Padding(0);
            this.textureBackgroundDialog.Name = "textureBackgroundDialog";
            this.textureBackgroundDialog.Size = new System.Drawing.Size(800, 650);
            this.textureBackgroundDialog.TabIndex = 0;
            // 
            // panelShell
            // 
            this.panelShell.BackColor = System.Drawing.Color.Transparent;
            this.panelShell.Controls.Add(this.panelContentHost);
            this.panelShell.Controls.Add(this.panelHeader);
            this.panelShell.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelShell.Location = new System.Drawing.Point(0, 0);
            this.panelShell.Margin = new System.Windows.Forms.Padding(0);
            this.panelShell.Name = "panelShell";
            this.panelShell.Padding = new System.Windows.Forms.Padding(12);
            this.panelShell.Radius = 16;
            this.panelShell.Shadow = 16;
            this.panelShell.ShadowOpacity = 0.2F;
            this.panelShell.Size = new System.Drawing.Size(800, 650);
            this.panelShell.TabIndex = 0;
            // 
            // panelContentHost
            // 
            this.panelContentHost.Controls.Add(this.panelFooter);
            this.panelContentHost.Controls.Add(this.motionCardDetailControl);
            this.panelContentHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContentHost.Location = new System.Drawing.Point(28, 84);
            this.panelContentHost.Margin = new System.Windows.Forms.Padding(0);
            this.panelContentHost.Name = "panelContentHost";
            this.panelContentHost.Radius = 0;
            this.panelContentHost.Size = new System.Drawing.Size(744, 538);
            this.panelContentHost.TabIndex = 1;
            // 
            // motionCardDetailControl
            // 
            this.motionCardDetailControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.motionCardDetailControl.Location = new System.Drawing.Point(0, 0);
            this.motionCardDetailControl.Margin = new System.Windows.Forms.Padding(0);
            this.motionCardDetailControl.Name = "motionCardDetailControl";
            this.motionCardDetailControl.Size = new System.Drawing.Size(744, 538);
            this.motionCardDetailControl.TabIndex = 0;
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.labelDialogDescription);
            this.panelHeader.Controls.Add(this.labelDialogTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(28, 28);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Padding = new System.Windows.Forms.Padding(4, 0, 4, 8);
            this.panelHeader.Radius = 0;
            this.panelHeader.Size = new System.Drawing.Size(744, 56);
            this.panelHeader.TabIndex = 0;
            // 
            // labelDialogDescription
            // 
            this.labelDialogDescription.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelDialogDescription.Location = new System.Drawing.Point(395, 0);
            this.labelDialogDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDialogDescription.Name = "labelDialogDescription";
            this.labelDialogDescription.Size = new System.Drawing.Size(345, 48);
            this.labelDialogDescription.TabIndex = 1;
            this.labelDialogDescription.Text = "查看控制卡详细信息";
            this.labelDialogDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelDialogTitle
            // 
            this.labelDialogTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelDialogTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelDialogTitle.Location = new System.Drawing.Point(4, 0);
            this.labelDialogTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelDialogTitle.Name = "labelDialogTitle";
            this.labelDialogTitle.Size = new System.Drawing.Size(344, 48);
            this.labelDialogTitle.TabIndex = 0;
            this.labelDialogTitle.Text = "控制卡详情";
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.flowFooterButtons);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(0, 482);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(744, 56);
            this.panelFooter.TabIndex = 1;
            this.panelFooter.Text = "panel1";
            // 
            // flowFooterButtons
            // 
            this.flowFooterButtons.Align = AntdUI.TAlignFlow.Right;
            this.flowFooterButtons.Controls.Add(this.buttonCancel);
            this.flowFooterButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowFooterButtons.Location = new System.Drawing.Point(485, 0);
            this.flowFooterButtons.Name = "flowFooterButtons";
            this.flowFooterButtons.Size = new System.Drawing.Size(259, 56);
            this.flowFooterButtons.TabIndex = 0;
            this.flowFooterButtons.Text = "flowPanel1";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(143, 0);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Radius = 8;
            this.buttonCancel.Size = new System.Drawing.Size(116, 38);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.WaveSize = 0;
            // 
            // MotionCardDetailDialog
            // 
            this.ClientSize = new System.Drawing.Size(800, 650);
            this.Controls.Add(this.textureBackgroundDialog);
            this.Name = "MotionCardDetailDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "控制卡详情";
            this.TopMost = true;
            this.textureBackgroundDialog.ResumeLayout(false);
            this.panelShell.ResumeLayout(false);
            this.panelContentHost.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            this.flowFooterButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AMControlWinF.Views.Main.TextureBackgroundControl textureBackgroundDialog;
        private AntdUI.Panel panelShell;
        private AntdUI.Panel panelHeader;
        private AntdUI.Label labelDialogTitle;
        private AntdUI.Label labelDialogDescription;
        private AntdUI.Panel panelContentHost;
        private MotionCardDetailControl motionCardDetailControl;
        private AntdUI.Panel panelFooter;
        private AntdUI.FlowPanel flowFooterButtons;
        private AntdUI.Button buttonCancel;
    }
}