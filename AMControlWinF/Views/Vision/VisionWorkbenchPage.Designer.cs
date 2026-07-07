namespace AMControlWinF.Views.Vision
{
    partial class VisionWorkbenchPage
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
            this.panelBrowserCard = new AntdUI.Panel();
            this.panelFallback = new AntdUI.Panel();
            this.panelFallbackContent = new AntdUI.Panel();
            this.buttonFallbackOpen = new AntdUI.Button();
            this.labelFallbackMessage = new AntdUI.Label();
            this.labelFallbackTitle = new AntdUI.Label();
            this.webViewWorkbench = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.panelToolbar = new AntdUI.Panel();
            this.flowToolbarRight = new AntdUI.FlowPanel();
            this.buttonOpenExternal = new AntdUI.Button();
            this.buttonRetry = new AntdUI.Button();
            this.buttonRefresh = new AntdUI.Button();
            this.flowToolbarLeft = new AntdUI.FlowPanel();
            this.labelStatus = new AntdUI.Label();
            this.labelTitle = new AntdUI.Label();
            this.panelRoot.SuspendLayout();
            this.panelBrowserCard.SuspendLayout();
            this.panelFallback.SuspendLayout();
            this.panelFallbackContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.webViewWorkbench)).BeginInit();
            this.panelToolbar.SuspendLayout();
            this.flowToolbarRight.SuspendLayout();
            this.flowToolbarLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.panelBrowserCard);
            this.panelRoot.Controls.Add(this.panelToolbar);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Margin = new System.Windows.Forms.Padding(0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(8, 8, 8, 0);
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(1150, 680);
            this.panelRoot.TabIndex = 0;
            // 
            // panelBrowserCard
            // 
            this.panelBrowserCard.BackColor = System.Drawing.Color.Transparent;
            this.panelBrowserCard.Controls.Add(this.panelFallback);
            this.panelBrowserCard.Controls.Add(this.webViewWorkbench);
            this.panelBrowserCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBrowserCard.Location = new System.Drawing.Point(8, 52);
            this.panelBrowserCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelBrowserCard.Name = "panelBrowserCard";
            this.panelBrowserCard.Radius = 12;
            this.panelBrowserCard.Shadow = 4;
            this.panelBrowserCard.ShadowOpacity = 0.15F;
            this.panelBrowserCard.Size = new System.Drawing.Size(1134, 628);
            this.panelBrowserCard.TabIndex = 1;
            // 
            // panelFallback
            // 
            this.panelFallback.Controls.Add(this.panelFallbackContent);
            this.panelFallback.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFallback.Location = new System.Drawing.Point(4, 4);
            this.panelFallback.Margin = new System.Windows.Forms.Padding(0);
            this.panelFallback.Name = "panelFallback";
            this.panelFallback.Radius = 0;
            this.panelFallback.Size = new System.Drawing.Size(1126, 620);
            this.panelFallback.TabIndex = 1;
            this.panelFallback.Visible = false;
            // 
            // panelFallbackContent
            // 
            this.panelFallbackContent.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelFallbackContent.Controls.Add(this.buttonFallbackOpen);
            this.panelFallbackContent.Controls.Add(this.labelFallbackMessage);
            this.panelFallbackContent.Controls.Add(this.labelFallbackTitle);
            this.panelFallbackContent.Location = new System.Drawing.Point(313, 209);
            this.panelFallbackContent.Margin = new System.Windows.Forms.Padding(0);
            this.panelFallbackContent.Name = "panelFallbackContent";
            this.panelFallbackContent.Padding = new System.Windows.Forms.Padding(16);
            this.panelFallbackContent.Radius = 12;
            this.panelFallbackContent.Shadow = 4;
            this.panelFallbackContent.ShadowOpacity = 0.12F;
            this.panelFallbackContent.Size = new System.Drawing.Size(500, 202);
            this.panelFallbackContent.TabIndex = 0;
            // 
            // buttonFallbackOpen
            // 
            this.buttonFallbackOpen.IconSvg = "ExportOutlined";
            this.buttonFallbackOpen.Location = new System.Drawing.Point(176, 140);
            this.buttonFallbackOpen.Margin = new System.Windows.Forms.Padding(0);
            this.buttonFallbackOpen.Name = "buttonFallbackOpen";
            this.buttonFallbackOpen.Radius = 8;
            this.buttonFallbackOpen.Size = new System.Drawing.Size(148, 36);
            this.buttonFallbackOpen.TabIndex = 2;
            this.buttonFallbackOpen.Text = "外部浏览器打开";
            this.buttonFallbackOpen.Type = AntdUI.TTypeMini.Primary;
            this.buttonFallbackOpen.WaveSize = 0;
            // 
            // labelFallbackMessage
            // 
            this.labelFallbackMessage.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelFallbackMessage.Location = new System.Drawing.Point(20, 64);
            this.labelFallbackMessage.Margin = new System.Windows.Forms.Padding(0);
            this.labelFallbackMessage.Name = "labelFallbackMessage";
            this.labelFallbackMessage.Size = new System.Drawing.Size(460, 62);
            this.labelFallbackMessage.TabIndex = 1;
            this.labelFallbackMessage.Text = "请确认 amvision 前端服务已启动。";
            this.labelFallbackMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelFallbackTitle
            // 
            this.labelFallbackTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Bold);
            this.labelFallbackTitle.Location = new System.Drawing.Point(20, 20);
            this.labelFallbackTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelFallbackTitle.Name = "labelFallbackTitle";
            this.labelFallbackTitle.Size = new System.Drawing.Size(460, 36);
            this.labelFallbackTitle.TabIndex = 0;
            this.labelFallbackTitle.Text = "视觉工作台不可用";
            this.labelFallbackTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // webViewWorkbench
            // 
            this.webViewWorkbench.AllowExternalDrop = true;
            this.webViewWorkbench.CreationProperties = null;
            this.webViewWorkbench.DefaultBackgroundColor = System.Drawing.Color.White;
            this.webViewWorkbench.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webViewWorkbench.Location = new System.Drawing.Point(4, 4);
            this.webViewWorkbench.Margin = new System.Windows.Forms.Padding(0);
            this.webViewWorkbench.Name = "webViewWorkbench";
            this.webViewWorkbench.Size = new System.Drawing.Size(1126, 620);
            this.webViewWorkbench.TabIndex = 0;
            this.webViewWorkbench.ZoomFactor = 1D;
            // 
            // panelToolbar
            // 
            this.panelToolbar.Controls.Add(this.flowToolbarRight);
            this.panelToolbar.Controls.Add(this.flowToolbarLeft);
            this.panelToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelToolbar.Location = new System.Drawing.Point(8, 8);
            this.panelToolbar.Margin = new System.Windows.Forms.Padding(0);
            this.panelToolbar.Name = "panelToolbar";
            this.panelToolbar.Padding = new System.Windows.Forms.Padding(4);
            this.panelToolbar.Radius = 0;
            this.panelToolbar.Size = new System.Drawing.Size(1134, 44);
            this.panelToolbar.TabIndex = 0;
            // 
            // flowToolbarRight
            // 
            this.flowToolbarRight.Align = AntdUI.TAlignFlow.RightCenter;
            this.flowToolbarRight.Controls.Add(this.buttonOpenExternal);
            this.flowToolbarRight.Controls.Add(this.buttonRetry);
            this.flowToolbarRight.Controls.Add(this.buttonRefresh);
            this.flowToolbarRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowToolbarRight.Gap = 8;
            this.flowToolbarRight.Location = new System.Drawing.Point(792, 4);
            this.flowToolbarRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowToolbarRight.Name = "flowToolbarRight";
            this.flowToolbarRight.Size = new System.Drawing.Size(338, 36);
            this.flowToolbarRight.TabIndex = 1;
            // 
            // buttonOpenExternal
            // 
            this.buttonOpenExternal.IconSvg = "ExportOutlined";
            this.buttonOpenExternal.Location = new System.Drawing.Point(194, 0);
            this.buttonOpenExternal.Margin = new System.Windows.Forms.Padding(0);
            this.buttonOpenExternal.Name = "buttonOpenExternal";
            this.buttonOpenExternal.Radius = 8;
            this.buttonOpenExternal.Size = new System.Drawing.Size(144, 36);
            this.buttonOpenExternal.TabIndex = 2;
            this.buttonOpenExternal.Text = "外部浏览器打开";
            this.buttonOpenExternal.WaveSize = 0;
            // 
            // buttonRetry
            // 
            this.buttonRetry.IconSvg = "SyncOutlined";
            this.buttonRetry.Location = new System.Drawing.Point(100, 0);
            this.buttonRetry.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRetry.Name = "buttonRetry";
            this.buttonRetry.Radius = 8;
            this.buttonRetry.Size = new System.Drawing.Size(86, 36);
            this.buttonRetry.TabIndex = 1;
            this.buttonRetry.Text = "重连";
            this.buttonRetry.WaveSize = 0;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.IconSvg = "ReloadOutlined";
            this.buttonRefresh.Location = new System.Drawing.Point(14, 0);
            this.buttonRefresh.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Radius = 8;
            this.buttonRefresh.Size = new System.Drawing.Size(78, 36);
            this.buttonRefresh.TabIndex = 0;
            this.buttonRefresh.Text = "刷新";
            this.buttonRefresh.Type = AntdUI.TTypeMini.Primary;
            this.buttonRefresh.WaveSize = 0;
            // 
            // flowToolbarLeft
            // 
            this.flowToolbarLeft.Align = AntdUI.TAlignFlow.Left;
            this.flowToolbarLeft.Controls.Add(this.labelStatus);
            this.flowToolbarLeft.Controls.Add(this.labelTitle);
            this.flowToolbarLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowToolbarLeft.Gap = 8;
            this.flowToolbarLeft.Location = new System.Drawing.Point(4, 4);
            this.flowToolbarLeft.Margin = new System.Windows.Forms.Padding(0);
            this.flowToolbarLeft.Name = "flowToolbarLeft";
            this.flowToolbarLeft.Size = new System.Drawing.Size(700, 36);
            this.flowToolbarLeft.TabIndex = 0;
            // 
            // labelStatus
            // 
            this.labelStatus.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelStatus.Location = new System.Drawing.Point(116, 0);
            this.labelStatus.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(580, 36);
            this.labelStatus.TabIndex = 1;
            this.labelStatus.Text = "视觉工作台待连接";
            // 
            // labelTitle
            // 
            this.labelTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(0, 0);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(100, 36);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "视觉工作台";
            // 
            // VisionWorkbenchPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "VisionWorkbenchPage";
            this.Size = new System.Drawing.Size(1150, 680);
            this.panelRoot.ResumeLayout(false);
            this.panelBrowserCard.ResumeLayout(false);
            this.panelFallback.ResumeLayout(false);
            this.panelFallbackContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.webViewWorkbench)).EndInit();
            this.panelToolbar.ResumeLayout(false);
            this.flowToolbarRight.ResumeLayout(false);
            this.flowToolbarLeft.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AntdUI.Panel panelRoot;
        private AntdUI.Panel panelToolbar;
        private AntdUI.FlowPanel flowToolbarLeft;
        private AntdUI.Label labelTitle;
        private AntdUI.Label labelStatus;
        private AntdUI.FlowPanel flowToolbarRight;
        private AntdUI.Button buttonRefresh;
        private AntdUI.Button buttonRetry;
        private AntdUI.Button buttonOpenExternal;
        private AntdUI.Panel panelBrowserCard;
        private Microsoft.Web.WebView2.WinForms.WebView2 webViewWorkbench;
        private AntdUI.Panel panelFallback;
        private AntdUI.Panel panelFallbackContent;
        private AntdUI.Label labelFallbackTitle;
        private AntdUI.Label labelFallbackMessage;
        private AntdUI.Button buttonFallbackOpen;
    }
}
