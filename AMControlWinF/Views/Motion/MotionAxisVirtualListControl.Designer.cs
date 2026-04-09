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
            this.virtualPanelActions = new AntdUI.VirtualPanel();
            this.panelRoot.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.virtualPanelActions);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Margin = new System.Windows.Forms.Padding(0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(4);
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(240, 160);
            this.panelRoot.TabIndex = 0;
            // 
            // virtualPanelActions
            // 
            this.virtualPanelActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.virtualPanelActions.Empty = true;
            this.virtualPanelActions.EmptyText = "暂无动作数据";
            this.virtualPanelActions.Gap = 8;
            this.virtualPanelActions.Location = new System.Drawing.Point(4, 4);
            this.virtualPanelActions.Margin = new System.Windows.Forms.Padding(0);
            this.virtualPanelActions.Name = "virtualPanelActions";
            this.virtualPanelActions.Padding = new System.Windows.Forms.Padding(4);
            this.virtualPanelActions.Radius = 12;
            this.virtualPanelActions.Shadow = 6;
            this.virtualPanelActions.ShadowOpacity = 0.12F;
            this.virtualPanelActions.ShadowOpacityAnimation = true;
            this.virtualPanelActions.ShadowOpacityHover = 0.22F;
            this.virtualPanelActions.Size = new System.Drawing.Size(232, 152);
            this.virtualPanelActions.TabIndex = 0;
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
        private AntdUI.VirtualPanel virtualPanelActions;
    }
}