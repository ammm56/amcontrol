namespace AMControlWinF.Views.Motion
{
    partial class MotionActuatorVirtualListControl
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
            this.virtualPanelItems = new AntdUI.VirtualPanel();
            this.panelRoot.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.virtualPanelItems);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Margin = new System.Windows.Forms.Padding(0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(4);
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(320, 200);
            this.panelRoot.TabIndex = 0;
            // 
            // virtualPanelItems
            // 
            this.virtualPanelItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.virtualPanelItems.Empty = true;
            this.virtualPanelItems.EmptyText = "暂无执行器对象";
            this.virtualPanelItems.Gap = 8;
            this.virtualPanelItems.Location = new System.Drawing.Point(4, 4);
            this.virtualPanelItems.Margin = new System.Windows.Forms.Padding(0);
            this.virtualPanelItems.Name = "virtualPanelItems";
            this.virtualPanelItems.Padding = new System.Windows.Forms.Padding(4);
            this.virtualPanelItems.Radius = 12;
            this.virtualPanelItems.Shadow = 6;
            this.virtualPanelItems.ShadowOpacity = 0.12F;
            this.virtualPanelItems.ShadowOpacityAnimation = true;
            this.virtualPanelItems.ShadowOpacityHover = 0.22F;
            this.virtualPanelItems.Size = new System.Drawing.Size(312, 192);
            this.virtualPanelItems.TabIndex = 0;
            // 
            // MotionActuatorVirtualListControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MotionActuatorVirtualListControl";
            this.Size = new System.Drawing.Size(320, 200);
            this.panelRoot.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private AntdUI.Panel panelRoot;
        private AntdUI.VirtualPanel virtualPanelItems;
    }
}