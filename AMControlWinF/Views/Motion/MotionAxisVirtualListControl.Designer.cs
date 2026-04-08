namespace AMControlWinF.Views.Motion
{
    partial class MotionAxisVirtualListControl
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
            this.virtualPanelInputs = new AntdUI.VirtualPanel();
            this.panelRoot.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.virtualPanelInputs);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Margin = new System.Windows.Forms.Padding(0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(4);
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(240, 160);
            this.panelRoot.TabIndex = 0;
            // 
            // virtualPanelInputs
            // 
            this.virtualPanelInputs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.virtualPanelInputs.Empty = true;
            this.virtualPanelInputs.EmptyText = "暂无动作数据";
            this.virtualPanelInputs.Gap = 0;
            this.virtualPanelInputs.Location = new System.Drawing.Point(4, 4);
            this.virtualPanelInputs.Margin = new System.Windows.Forms.Padding(0);
            this.virtualPanelInputs.Name = "virtualPanelInputs";
            this.virtualPanelInputs.Padding = new System.Windows.Forms.Padding(4);
            this.virtualPanelInputs.Radius = 12;
            this.virtualPanelInputs.Shadow = 6;
            this.virtualPanelInputs.ShadowOpacity = 0.12F;
            this.virtualPanelInputs.ShadowOpacityAnimation = true;
            this.virtualPanelInputs.ShadowOpacityHover = 0.22F;
            this.virtualPanelInputs.Size = new System.Drawing.Size(232, 152);
            this.virtualPanelInputs.TabIndex = 0;
            // 
            // MotionAxisVirtualListControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MotionAxisVirtualListControl";
            this.Size = new System.Drawing.Size(240, 160);
            this.panelRoot.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private AntdUI.Panel panelRoot;
        private AntdUI.VirtualPanel virtualPanelInputs;
    }
}