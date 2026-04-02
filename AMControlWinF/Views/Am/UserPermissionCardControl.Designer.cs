namespace AMControlWinF.Views.Am
{
    partial class UserPermissionCardControl
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

        #region 组件设计器生成的代码

        private void InitializeComponent()
        {
            this.panelCard = new AntdUI.Panel();
            this.panelBody = new AntdUI.Panel();
            this.labelRecommendedRoles = new AntdUI.Label();
            this.labelDescription = new AntdUI.Label();
            this.panelFooter = new AntdUI.Panel();
            this.flowFooter = new AntdUI.FlowPanel();
            this.buttonRisk = new AntdUI.Button();
            this.panelHeader = new AntdUI.Panel();
            this.labelTitle = new AntdUI.Label();
            this.checkPermission = new AntdUI.Checkbox();
            this.panelCard.SuspendLayout();
            this.panelBody.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.flowFooter.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelCard
            // 
            this.panelCard.BackColor = System.Drawing.Color.Transparent;
            this.panelCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(229)))), ((int)(((byte)(235)))));
            this.panelCard.BorderWidth = 1F;
            this.panelCard.Controls.Add(this.panelBody);
            this.panelCard.Controls.Add(this.panelFooter);
            this.panelCard.Controls.Add(this.panelHeader);
            this.panelCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCard.Location = new System.Drawing.Point(0, 0);
            this.panelCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelCard.Name = "panelCard";
            this.panelCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelCard.Radius = 12;
            this.panelCard.Shadow = 4;
            this.panelCard.ShadowOpacity = 0.2F;
            this.panelCard.ShadowOpacityAnimation = true;
            this.panelCard.Size = new System.Drawing.Size(228, 168);
            this.panelCard.TabIndex = 0;
            // 
            // panelBody
            // 
            this.panelBody.Controls.Add(this.labelRecommendedRoles);
            this.panelBody.Controls.Add(this.labelDescription);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(17, 45);
            this.panelBody.Margin = new System.Windows.Forms.Padding(0);
            this.panelBody.Name = "panelBody";
            this.panelBody.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.panelBody.Radius = 0;
            this.panelBody.Size = new System.Drawing.Size(194, 72);
            this.panelBody.TabIndex = 1;
            // 
            // labelRecommendedRoles
            // 
            this.labelRecommendedRoles.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelRecommendedRoles.Location = new System.Drawing.Point(0, 44);
            this.labelRecommendedRoles.Margin = new System.Windows.Forms.Padding(0);
            this.labelRecommendedRoles.Name = "labelRecommendedRoles";
            this.labelRecommendedRoles.Size = new System.Drawing.Size(194, 28);
            this.labelRecommendedRoles.TabIndex = 1;
            this.labelRecommendedRoles.Text = "建议角色：-";
            this.labelRecommendedRoles.Visible = false;
            // 
            // labelDescription
            // 
            this.labelDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDescription.Location = new System.Drawing.Point(0, 2);
            this.labelDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(194, 70);
            this.labelDescription.TabIndex = 0;
            this.labelDescription.Text = "暂无说明。";
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.flowFooter);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(17, 117);
            this.panelFooter.Margin = new System.Windows.Forms.Padding(0);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.panelFooter.Radius = 0;
            this.panelFooter.Size = new System.Drawing.Size(194, 34);
            this.panelFooter.TabIndex = 2;
            // 
            // flowFooter
            // 
            this.flowFooter.Controls.Add(this.buttonRisk);
            this.flowFooter.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowFooter.Location = new System.Drawing.Point(0, 6);
            this.flowFooter.Margin = new System.Windows.Forms.Padding(0);
            this.flowFooter.Name = "flowFooter";
            this.flowFooter.Size = new System.Drawing.Size(112, 28);
            this.flowFooter.TabIndex = 0;
            this.flowFooter.Text = "flowFooter";
            // 
            // buttonRisk
            // 
            this.buttonRisk.Location = new System.Drawing.Point(0, 0);
            this.buttonRisk.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRisk.Name = "buttonRisk";
            this.buttonRisk.Radius = 8;
            this.buttonRisk.Size = new System.Drawing.Size(92, 24);
            this.buttonRisk.TabIndex = 0;
            this.buttonRisk.Text = "权限：低";
            this.buttonRisk.Type = AntdUI.TTypeMini.Success;
            this.buttonRisk.WaveSize = 0;
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.labelTitle);
            this.panelHeader.Controls.Add(this.checkPermission);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(17, 17);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Radius = 0;
            this.panelHeader.Size = new System.Drawing.Size(194, 28);
            this.panelHeader.TabIndex = 0;
            // 
            // labelTitle
            // 
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(0, 0);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(166, 28);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "权限标题";
            // 
            // checkPermission
            // 
            this.checkPermission.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkPermission.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkPermission.Location = new System.Drawing.Point(166, 0);
            this.checkPermission.Margin = new System.Windows.Forms.Padding(0);
            this.checkPermission.Name = "checkPermission";
            this.checkPermission.Size = new System.Drawing.Size(28, 28);
            this.checkPermission.TabIndex = 1;
            this.checkPermission.Text = "";
            // 
            // UserPermissionCardControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelCard);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "UserPermissionCardControl";
            this.Size = new System.Drawing.Size(228, 168);
            this.panelCard.ResumeLayout(false);
            this.panelBody.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            this.flowFooter.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.Panel panelCard;
        private AntdUI.Panel panelHeader;
        private AntdUI.Label labelTitle;
        private AntdUI.Checkbox checkPermission;
        private AntdUI.Panel panelBody;
        private AntdUI.Label labelDescription;
        private AntdUI.Label labelRecommendedRoles;
        private AntdUI.Panel panelFooter;
        private AntdUI.FlowPanel flowFooter;
        private AntdUI.Button buttonRisk;
    }
}