namespace AMControlWinF.Views.Main
{
    partial class PageLoadingMaskControl
    {
        private System.ComponentModel.IContainer components = null;


        #region 组件设计器生成的代码

        private void InitializeComponent()
        {
            this.panelRoot = new AntdUI.Panel();
            this.progressLoading = new AntdUI.Progress();
            this.panelRoot.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.progressLoading);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Margin = new System.Windows.Forms.Padding(0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(96, 0, 96, 0);
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(850, 680);
            this.panelRoot.TabIndex = 0;
            // 
            // progressLoading
            // 
            this.progressLoading.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.progressLoading.Location = new System.Drawing.Point(0, 300);
            this.progressLoading.Margin = new System.Windows.Forms.Padding(0);
            this.progressLoading.Name = "progressLoading";
            this.progressLoading.Size = new System.Drawing.Size(850, 20);
            this.progressLoading.TabIndex = 0;
            this.progressLoading.Text = "";
            this.progressLoading.TextUnit = "";
            // 
            // PageLoadingMaskControl
            // 
            this.Controls.Add(this.panelRoot);
            this.Name = "PageLoadingMaskControl";
            this.Size = new System.Drawing.Size(850, 680);
            this.panelRoot.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.Panel panelRoot;
        private AntdUI.Progress progressLoading;
    }
}