namespace AMControlWinF.Views.Plc
{
    partial class PlcPointVirtualListControl
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
            this.virtualPanelPoints = new AntdUI.VirtualPanel();
            this.panelRoot.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.virtualPanelPoints);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Margin = new System.Windows.Forms.Padding(0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(4);
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(220, 120);
            this.panelRoot.TabIndex = 0;
            // 
            // virtualPanelPoints
            // 
            this.virtualPanelPoints.Dock = System.Windows.Forms.DockStyle.Fill;
            this.virtualPanelPoints.Empty = true;
            this.virtualPanelPoints.EmptyText = "暂无 PLC 点位";
            this.virtualPanelPoints.Gap = 0;
            this.virtualPanelPoints.Location = new System.Drawing.Point(4, 4);
            this.virtualPanelPoints.Margin = new System.Windows.Forms.Padding(0);
            this.virtualPanelPoints.Name = "virtualPanelPoints";
            this.virtualPanelPoints.Padding = new System.Windows.Forms.Padding(4);
            this.virtualPanelPoints.Radius = 12;
            this.virtualPanelPoints.Shadow = 6;
            this.virtualPanelPoints.ShadowOpacity = 0.12F;
            this.virtualPanelPoints.ShadowOpacityAnimation = true;
            this.virtualPanelPoints.ShadowOpacityHover = 0.22F;
            this.virtualPanelPoints.Size = new System.Drawing.Size(212, 112);
            this.virtualPanelPoints.TabIndex = 0;
            // 
            // PlcPointVirtualListControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "PlcPointVirtualListControl";
            this.Size = new System.Drawing.Size(220, 120);
            this.panelRoot.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private AntdUI.Panel panelRoot;
        private AntdUI.VirtualPanel virtualPanelPoints;
    }
}