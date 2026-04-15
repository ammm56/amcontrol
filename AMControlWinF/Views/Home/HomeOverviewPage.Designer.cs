namespace AMControlWinF.Views.Home
{
    partial class HomeOverviewPage
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        private void InitializeComponent()
        {
            this.panelRoot = new AntdUI.Panel();
            this.panelContentCard = new AntdUI.Panel();
            this.gridContent = new AntdUI.GridPanel();
            this.panelAreaD = new AntdUI.Panel();
            this.carouselAreaD = new AntdUI.Carousel();
            this.panelAreaC = new AntdUI.Panel();
            this.carouselAreaC = new AntdUI.Carousel();
            this.panelAreaB = new AntdUI.Panel();
            this.carouselAreaB = new AntdUI.Carousel();
            this.panelAreaA = new AntdUI.Panel();
            this.carouselAreaA = new AntdUI.Carousel();
            this.panelSpacer = new AntdUI.Panel();
            this.panelToolbar = new AntdUI.Panel();
            this.labelTime = new AntdUI.LabelTime();
            this.panelRoot.SuspendLayout();
            this.panelContentCard.SuspendLayout();
            this.gridContent.SuspendLayout();
            this.panelAreaD.SuspendLayout();
            this.panelAreaC.SuspendLayout();
            this.panelAreaB.SuspendLayout();
            this.panelAreaA.SuspendLayout();
            this.panelToolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.panelContentCard);
            this.panelRoot.Controls.Add(this.panelSpacer);
            this.panelRoot.Controls.Add(this.panelToolbar);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Margin = new System.Windows.Forms.Padding(0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(1100, 680);
            this.panelRoot.TabIndex = 0;
            // 
            // panelContentCard
            // 
            this.panelContentCard.BackColor = System.Drawing.Color.Transparent;
            this.panelContentCard.Controls.Add(this.gridContent);
            this.panelContentCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContentCard.Location = new System.Drawing.Point(0, 44);
            this.panelContentCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelContentCard.Name = "panelContentCard";
            this.panelContentCard.Radius = 0;
            this.panelContentCard.ShadowOpacity = 0F;
            this.panelContentCard.ShadowOpacityHover = 0F;
            this.panelContentCard.Size = new System.Drawing.Size(1100, 636);
            this.panelContentCard.TabIndex = 2;
            // 
            // gridContent
            // 
            this.gridContent.Controls.Add(this.panelAreaD);
            this.gridContent.Controls.Add(this.panelAreaC);
            this.gridContent.Controls.Add(this.panelAreaB);
            this.gridContent.Controls.Add(this.panelAreaA);
            this.gridContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridContent.Location = new System.Drawing.Point(0, 0);
            this.gridContent.Margin = new System.Windows.Forms.Padding(0);
            this.gridContent.Name = "gridContent";
            this.gridContent.Size = new System.Drawing.Size(1100, 636);
            this.gridContent.TabIndex = 0;
            // 
            // panelAreaD
            // 
            this.panelAreaD.BackColor = System.Drawing.Color.Transparent;
            this.panelAreaD.Controls.Add(this.carouselAreaD);
            this.panelAreaD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAreaD.Location = new System.Drawing.Point(550, 318);
            this.panelAreaD.Margin = new System.Windows.Forms.Padding(0);
            this.panelAreaD.Name = "panelAreaD";
            this.panelAreaD.Padding = new System.Windows.Forms.Padding(8);
            this.panelAreaD.Radius = 12;
            this.panelAreaD.Shadow = 4;
            this.panelAreaD.Size = new System.Drawing.Size(550, 318);
            this.panelAreaD.TabIndex = 3;
            // 
            // carouselAreaD
            // 
            this.carouselAreaD.Autodelay = 3;
            this.carouselAreaD.Autoplay = true;
            this.carouselAreaD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.carouselAreaD.DotPosition = AntdUI.TAlignMini.Bottom;
            this.carouselAreaD.Location = new System.Drawing.Point(12, 12);
            this.carouselAreaD.Margin = new System.Windows.Forms.Padding(0);
            this.carouselAreaD.Name = "carouselAreaD";
            this.carouselAreaD.Radius = 10;
            this.carouselAreaD.Size = new System.Drawing.Size(526, 294);
            this.carouselAreaD.TabIndex = 0;
            this.carouselAreaD.Text = "carouselAreaD";
            // 
            // panelAreaC
            // 
            this.panelAreaC.BackColor = System.Drawing.Color.Transparent;
            this.panelAreaC.Controls.Add(this.carouselAreaC);
            this.panelAreaC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAreaC.Location = new System.Drawing.Point(0, 318);
            this.panelAreaC.Margin = new System.Windows.Forms.Padding(0);
            this.panelAreaC.Name = "panelAreaC";
            this.panelAreaC.Padding = new System.Windows.Forms.Padding(8);
            this.panelAreaC.Radius = 12;
            this.panelAreaC.Shadow = 4;
            this.panelAreaC.Size = new System.Drawing.Size(550, 318);
            this.panelAreaC.TabIndex = 2;
            // 
            // carouselAreaC
            // 
            this.carouselAreaC.Autodelay = 3;
            this.carouselAreaC.Autoplay = true;
            this.carouselAreaC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.carouselAreaC.DotPosition = AntdUI.TAlignMini.Bottom;
            this.carouselAreaC.Location = new System.Drawing.Point(12, 12);
            this.carouselAreaC.Margin = new System.Windows.Forms.Padding(0);
            this.carouselAreaC.Name = "carouselAreaC";
            this.carouselAreaC.Radius = 10;
            this.carouselAreaC.Size = new System.Drawing.Size(526, 294);
            this.carouselAreaC.TabIndex = 0;
            this.carouselAreaC.Text = "carouselAreaC";
            // 
            // panelAreaB
            // 
            this.panelAreaB.BackColor = System.Drawing.Color.Transparent;
            this.panelAreaB.Controls.Add(this.carouselAreaB);
            this.panelAreaB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAreaB.Location = new System.Drawing.Point(550, 0);
            this.panelAreaB.Margin = new System.Windows.Forms.Padding(0);
            this.panelAreaB.Name = "panelAreaB";
            this.panelAreaB.Padding = new System.Windows.Forms.Padding(8);
            this.panelAreaB.Radius = 12;
            this.panelAreaB.Shadow = 4;
            this.panelAreaB.Size = new System.Drawing.Size(550, 318);
            this.panelAreaB.TabIndex = 1;
            // 
            // carouselAreaB
            // 
            this.carouselAreaB.Autodelay = 3;
            this.carouselAreaB.Autoplay = true;
            this.carouselAreaB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.carouselAreaB.DotPosition = AntdUI.TAlignMini.Bottom;
            this.carouselAreaB.Location = new System.Drawing.Point(12, 12);
            this.carouselAreaB.Margin = new System.Windows.Forms.Padding(0);
            this.carouselAreaB.Name = "carouselAreaB";
            this.carouselAreaB.Radius = 10;
            this.carouselAreaB.Size = new System.Drawing.Size(526, 294);
            this.carouselAreaB.TabIndex = 0;
            this.carouselAreaB.Text = "carouselAreaB";
            // 
            // panelAreaA
            // 
            this.panelAreaA.BackColor = System.Drawing.Color.Transparent;
            this.panelAreaA.Controls.Add(this.carouselAreaA);
            this.panelAreaA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAreaA.Location = new System.Drawing.Point(0, 0);
            this.panelAreaA.Margin = new System.Windows.Forms.Padding(0);
            this.panelAreaA.Name = "panelAreaA";
            this.panelAreaA.Padding = new System.Windows.Forms.Padding(8);
            this.panelAreaA.Radius = 12;
            this.panelAreaA.Shadow = 4;
            this.panelAreaA.Size = new System.Drawing.Size(550, 318);
            this.panelAreaA.TabIndex = 0;
            // 
            // carouselAreaA
            // 
            this.carouselAreaA.Autodelay = 3;
            this.carouselAreaA.Autoplay = true;
            this.carouselAreaA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.carouselAreaA.DotPosition = AntdUI.TAlignMini.Bottom;
            this.carouselAreaA.Location = new System.Drawing.Point(12, 12);
            this.carouselAreaA.Margin = new System.Windows.Forms.Padding(0);
            this.carouselAreaA.Name = "carouselAreaA";
            this.carouselAreaA.Radius = 10;
            this.carouselAreaA.Size = new System.Drawing.Size(526, 294);
            this.carouselAreaA.TabIndex = 0;
            this.carouselAreaA.Text = "carouselAreaA";
            // 
            // panelSpacer
            // 
            this.panelSpacer.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSpacer.Location = new System.Drawing.Point(0, 44);
            this.panelSpacer.Margin = new System.Windows.Forms.Padding(0);
            this.panelSpacer.Name = "panelSpacer";
            this.panelSpacer.Radius = 0;
            this.panelSpacer.Size = new System.Drawing.Size(1100, 0);
            this.panelSpacer.TabIndex = 1;
            this.panelSpacer.Visible = false;
            // 
            // panelToolbar
            // 
            this.panelToolbar.Controls.Add(this.labelTime);
            this.panelToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelToolbar.Location = new System.Drawing.Point(0, 0);
            this.panelToolbar.Margin = new System.Windows.Forms.Padding(0);
            this.panelToolbar.Name = "panelToolbar";
            this.panelToolbar.Padding = new System.Windows.Forms.Padding(4);
            this.panelToolbar.Radius = 0;
            this.panelToolbar.Size = new System.Drawing.Size(1100, 44);
            this.panelToolbar.TabIndex = 0;
            // 
            // labelTime
            // 
            this.labelTime.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelTime.Location = new System.Drawing.Point(4, 4);
            this.labelTime.Margin = new System.Windows.Forms.Padding(0);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(230, 36);
            this.labelTime.TabIndex = 1;
            this.labelTime.Text = "labelTime";
            // 
            // HomeOverviewPage
            // 
            this.Controls.Add(this.panelRoot);
            this.Name = "HomeOverviewPage";
            this.Size = new System.Drawing.Size(1100, 680);
            this.panelRoot.ResumeLayout(false);
            this.panelContentCard.ResumeLayout(false);
            this.gridContent.ResumeLayout(false);
            this.panelAreaD.ResumeLayout(false);
            this.panelAreaC.ResumeLayout(false);
            this.panelAreaB.ResumeLayout(false);
            this.panelAreaA.ResumeLayout(false);
            this.panelToolbar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.Panel panelRoot;
        private AntdUI.Panel panelToolbar;
        private AntdUI.LabelTime labelTime;
        private AntdUI.Panel panelSpacer;
        private AntdUI.Panel panelContentCard;
        private AntdUI.GridPanel gridContent;
        private AntdUI.Panel panelAreaA;
        private AntdUI.Carousel carouselAreaA;
        private AntdUI.Panel panelAreaB;
        private AntdUI.Carousel carouselAreaB;
        private AntdUI.Panel panelAreaC;
        private AntdUI.Carousel carouselAreaC;
        private AntdUI.Panel panelAreaD;
        private AntdUI.Carousel carouselAreaD;
    }
}