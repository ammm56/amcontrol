namespace AMControlWinF.Views.Plc
{
    partial class PlcStatusVirtualListControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelRoot = new AntdUI.Panel();
            this.virtualPanelStations = new AntdUI.VirtualPanel();
            this.panelRoot.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.virtualPanelStations);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Margin = new System.Windows.Forms.Padding(0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(4);
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(180, 100);
            this.panelRoot.TabIndex = 0;
            // 
            // virtualPanelStations
            // 
            this.virtualPanelStations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.virtualPanelStations.Empty = true;
            this.virtualPanelStations.EmptyText = "暂无 PLC 站";
            this.virtualPanelStations.Gap = 0;
            this.virtualPanelStations.Location = new System.Drawing.Point(4, 4);
            this.virtualPanelStations.Margin = new System.Windows.Forms.Padding(0);
            this.virtualPanelStations.Name = "virtualPanelStations";
            this.virtualPanelStations.Padding = new System.Windows.Forms.Padding(4);
            this.virtualPanelStations.Radius = 12;
            this.virtualPanelStations.Shadow = 6;
            this.virtualPanelStations.ShadowOpacity = 0.12F;
            this.virtualPanelStations.ShadowOpacityAnimation = true;
            this.virtualPanelStations.ShadowOpacityHover = 0.22F;
            this.virtualPanelStations.Size = new System.Drawing.Size(172, 92);
            this.virtualPanelStations.TabIndex = 0;
            // 
            // PlcStatusVirtualListControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "PlcStatusVirtualListControl";
            this.Size = new System.Drawing.Size(180, 100);
            this.panelRoot.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private AntdUI.Panel panelRoot;
        private AntdUI.VirtualPanel virtualPanelStations;
    }
}