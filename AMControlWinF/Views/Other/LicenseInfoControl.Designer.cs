namespace AMControlWinF.Views.Other
{
    partial class LicenseInfoControl
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
            this.gridMain = new AntdUI.GridPanel();
            this.panelRightCard = new AntdUI.Panel();
            this.labelScopeValue = new AntdUI.Label();
            this.labelScopeTitle = new AntdUI.Label();
            this.labelTimeValue = new AntdUI.Label();
            this.labelTimeTitle = new AntdUI.Label();
            this.labelResultValue = new AntdUI.Label();
            this.labelRightTitle = new AntdUI.Label();
            this.panelLeftCard = new AntdUI.Panel();
            this.panelActionButtons = new AntdUI.Panel();
            this.flowActionButtons = new AntdUI.FlowPanel();
            this.buttonApplyLicense = new AntdUI.Button();
            this.buttonExportRequest = new AntdUI.Button();
            this.labelEnvironmentValue = new AntdUI.Label();
            this.labelEnvironmentTitle = new AntdUI.Label();
            this.labelHardwareValue = new AntdUI.Label();
            this.labelHardwareTitle = new AntdUI.Label();
            this.labelSoftwareValue = new AntdUI.Label();
            this.labelSoftwareTitle = new AntdUI.Label();
            this.labelLeftTitle = new AntdUI.Label();
            this.panelRoot.SuspendLayout();
            this.gridMain.SuspendLayout();
            this.panelRightCard.SuspendLayout();
            this.panelLeftCard.SuspendLayout();
            this.panelActionButtons.SuspendLayout();
            this.flowActionButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.gridMain);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(8);
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(980, 580);
            this.panelRoot.TabIndex = 0;
            // 
            // gridMain
            // 
            this.gridMain.Controls.Add(this.panelRightCard);
            this.gridMain.Controls.Add(this.panelLeftCard);
            this.gridMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridMain.Location = new System.Drawing.Point(8, 8);
            this.gridMain.Name = "gridMain";
            this.gridMain.Size = new System.Drawing.Size(964, 564);
            this.gridMain.Span = "47% 53%";
            this.gridMain.TabIndex = 0;
            // 
            // panelRightCard
            // 
            this.panelRightCard.Controls.Add(this.labelScopeValue);
            this.panelRightCard.Controls.Add(this.labelScopeTitle);
            this.panelRightCard.Controls.Add(this.labelTimeValue);
            this.panelRightCard.Controls.Add(this.labelTimeTitle);
            this.panelRightCard.Controls.Add(this.labelResultValue);
            this.panelRightCard.Controls.Add(this.labelRightTitle);
            this.panelRightCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRightCard.Location = new System.Drawing.Point(453, 0);
            this.panelRightCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelRightCard.Name = "panelRightCard";
            this.panelRightCard.Padding = new System.Windows.Forms.Padding(16);
            this.panelRightCard.Radius = 12;
            this.panelRightCard.Shadow = 4;
            this.panelRightCard.Size = new System.Drawing.Size(511, 564);
            this.panelRightCard.TabIndex = 1;
            // 
            // labelScopeValue
            // 
            this.labelScopeValue.BackColor = System.Drawing.Color.Transparent;
            this.labelScopeValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelScopeValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelScopeValue.Location = new System.Drawing.Point(20, 354);
            this.labelScopeValue.Name = "labelScopeValue";
            this.labelScopeValue.Size = new System.Drawing.Size(471, 190);
            this.labelScopeValue.TabIndex = 6;
            this.labelScopeValue.Text = "-";
            // 
            // labelScopeTitle
            // 
            this.labelScopeTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelScopeTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelScopeTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.labelScopeTitle.Location = new System.Drawing.Point(20, 330);
            this.labelScopeTitle.Name = "labelScopeTitle";
            this.labelScopeTitle.Size = new System.Drawing.Size(471, 24);
            this.labelScopeTitle.TabIndex = 5;
            this.labelScopeTitle.Text = "授权范围";
            // 
            // labelTimeValue
            // 
            this.labelTimeValue.BackColor = System.Drawing.Color.Transparent;
            this.labelTimeValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTimeValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelTimeValue.Location = new System.Drawing.Point(20, 220);
            this.labelTimeValue.Name = "labelTimeValue";
            this.labelTimeValue.Size = new System.Drawing.Size(471, 110);
            this.labelTimeValue.TabIndex = 4;
            this.labelTimeValue.Text = "-";
            // 
            // labelTimeTitle
            // 
            this.labelTimeTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelTimeTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTimeTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.labelTimeTitle.Location = new System.Drawing.Point(20, 196);
            this.labelTimeTitle.Name = "labelTimeTitle";
            this.labelTimeTitle.Size = new System.Drawing.Size(471, 24);
            this.labelTimeTitle.TabIndex = 3;
            this.labelTimeTitle.Text = "时间期限";
            // 
            // labelResultValue
            // 
            this.labelResultValue.BackColor = System.Drawing.Color.Transparent;
            this.labelResultValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelResultValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelResultValue.Location = new System.Drawing.Point(20, 44);
            this.labelResultValue.Name = "labelResultValue";
            this.labelResultValue.Size = new System.Drawing.Size(471, 152);
            this.labelResultValue.TabIndex = 2;
            this.labelResultValue.Text = "-";
            // 
            // labelRightTitle
            // 
            this.labelRightTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelRightTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelRightTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold);
            this.labelRightTitle.Location = new System.Drawing.Point(20, 20);
            this.labelRightTitle.Name = "labelRightTitle";
            this.labelRightTitle.Size = new System.Drawing.Size(471, 24);
            this.labelRightTitle.TabIndex = 0;
            this.labelRightTitle.Text = "授权结果";
            // 
            // panelLeftCard
            // 
            this.panelLeftCard.Controls.Add(this.panelActionButtons);
            this.panelLeftCard.Controls.Add(this.labelEnvironmentValue);
            this.panelLeftCard.Controls.Add(this.labelEnvironmentTitle);
            this.panelLeftCard.Controls.Add(this.labelHardwareValue);
            this.panelLeftCard.Controls.Add(this.labelHardwareTitle);
            this.panelLeftCard.Controls.Add(this.labelSoftwareValue);
            this.panelLeftCard.Controls.Add(this.labelSoftwareTitle);
            this.panelLeftCard.Controls.Add(this.labelLeftTitle);
            this.panelLeftCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLeftCard.Location = new System.Drawing.Point(0, 0);
            this.panelLeftCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelLeftCard.Name = "panelLeftCard";
            this.panelLeftCard.Padding = new System.Windows.Forms.Padding(16);
            this.panelLeftCard.Radius = 12;
            this.panelLeftCard.Shadow = 4;
            this.panelLeftCard.Size = new System.Drawing.Size(453, 564);
            this.panelLeftCard.TabIndex = 0;
            // 
            // panelActionButtons
            // 
            this.panelActionButtons.Controls.Add(this.flowActionButtons);
            this.panelActionButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelActionButtons.Location = new System.Drawing.Point(20, 498);
            this.panelActionButtons.Name = "panelActionButtons";
            this.panelActionButtons.Radius = 0;
            this.panelActionButtons.Size = new System.Drawing.Size(413, 46);
            this.panelActionButtons.TabIndex = 10;
            // 
            // flowActionButtons
            // 
            this.flowActionButtons.Align = AntdUI.TAlignFlow.Right;
            this.flowActionButtons.Controls.Add(this.buttonApplyLicense);
            this.flowActionButtons.Controls.Add(this.buttonExportRequest);
            this.flowActionButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowActionButtons.Gap = 10;
            this.flowActionButtons.Location = new System.Drawing.Point(0, 0);
            this.flowActionButtons.Name = "flowActionButtons";
            this.flowActionButtons.Size = new System.Drawing.Size(413, 46);
            this.flowActionButtons.TabIndex = 0;
            // 
            // buttonApplyLicense
            // 
            this.buttonApplyLicense.Location = new System.Drawing.Point(274, 3);
            this.buttonApplyLicense.Name = "buttonApplyLicense";
            this.buttonApplyLicense.Radius = 8;
            this.buttonApplyLicense.Size = new System.Drawing.Size(136, 38);
            this.buttonApplyLicense.TabIndex = 0;
            this.buttonApplyLicense.Text = "在线激活";
            this.buttonApplyLicense.Type = AntdUI.TTypeMini.Primary;
            this.buttonApplyLicense.WaveSize = 0;
            // 
            // buttonExportRequest
            // 
            this.buttonExportRequest.Location = new System.Drawing.Point(110, 3);
            this.buttonExportRequest.Name = "buttonExportRequest";
            this.buttonExportRequest.Radius = 8;
            this.buttonExportRequest.Size = new System.Drawing.Size(148, 38);
            this.buttonExportRequest.TabIndex = 1;
            this.buttonExportRequest.Text = "导出申请信息";
            this.buttonExportRequest.WaveSize = 0;
            // 
            // labelEnvironmentValue
            // 
            this.labelEnvironmentValue.BackColor = System.Drawing.Color.Transparent;
            this.labelEnvironmentValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelEnvironmentValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelEnvironmentValue.Location = new System.Drawing.Point(20, 307);
            this.labelEnvironmentValue.Name = "labelEnvironmentValue";
            this.labelEnvironmentValue.Size = new System.Drawing.Size(413, 132);
            this.labelEnvironmentValue.TabIndex = 7;
            this.labelEnvironmentValue.Text = "-";
            // 
            // labelEnvironmentTitle
            // 
            this.labelEnvironmentTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelEnvironmentTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelEnvironmentTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.labelEnvironmentTitle.Location = new System.Drawing.Point(20, 283);
            this.labelEnvironmentTitle.Name = "labelEnvironmentTitle";
            this.labelEnvironmentTitle.Size = new System.Drawing.Size(413, 24);
            this.labelEnvironmentTitle.TabIndex = 6;
            this.labelEnvironmentTitle.Text = "授权方式";
            // 
            // labelHardwareValue
            // 
            this.labelHardwareValue.BackColor = System.Drawing.Color.Transparent;
            this.labelHardwareValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelHardwareValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelHardwareValue.Location = new System.Drawing.Point(20, 174);
            this.labelHardwareValue.Name = "labelHardwareValue";
            this.labelHardwareValue.Size = new System.Drawing.Size(413, 109);
            this.labelHardwareValue.TabIndex = 5;
            this.labelHardwareValue.Text = "-";
            // 
            // labelHardwareTitle
            // 
            this.labelHardwareTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelHardwareTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelHardwareTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.labelHardwareTitle.Location = new System.Drawing.Point(20, 150);
            this.labelHardwareTitle.Name = "labelHardwareTitle";
            this.labelHardwareTitle.Size = new System.Drawing.Size(413, 24);
            this.labelHardwareTitle.TabIndex = 4;
            this.labelHardwareTitle.Text = "设备绑定";
            // 
            // labelSoftwareValue
            // 
            this.labelSoftwareValue.BackColor = System.Drawing.Color.Transparent;
            this.labelSoftwareValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelSoftwareValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelSoftwareValue.Location = new System.Drawing.Point(20, 68);
            this.labelSoftwareValue.Name = "labelSoftwareValue";
            this.labelSoftwareValue.Size = new System.Drawing.Size(413, 82);
            this.labelSoftwareValue.TabIndex = 3;
            this.labelSoftwareValue.Text = "-";
            // 
            // labelSoftwareTitle
            // 
            this.labelSoftwareTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelSoftwareTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelSoftwareTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.labelSoftwareTitle.Location = new System.Drawing.Point(20, 44);
            this.labelSoftwareTitle.Name = "labelSoftwareTitle";
            this.labelSoftwareTitle.Size = new System.Drawing.Size(413, 24);
            this.labelSoftwareTitle.TabIndex = 2;
            this.labelSoftwareTitle.Text = "软件身份";
            // 
            // labelLeftTitle
            // 
            this.labelLeftTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelLeftTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelLeftTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold);
            this.labelLeftTitle.Location = new System.Drawing.Point(20, 20);
            this.labelLeftTitle.Name = "labelLeftTitle";
            this.labelLeftTitle.Size = new System.Drawing.Size(413, 24);
            this.labelLeftTitle.TabIndex = 0;
            this.labelLeftTitle.Text = "授权申请";
            // 
            // LicenseInfoControl
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panelRoot);
            this.Name = "LicenseInfoControl";
            this.Size = new System.Drawing.Size(980, 580);
            this.panelRoot.ResumeLayout(false);
            this.gridMain.ResumeLayout(false);
            this.panelRightCard.ResumeLayout(false);
            this.panelLeftCard.ResumeLayout(false);
            this.panelActionButtons.ResumeLayout(false);
            this.flowActionButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AntdUI.Panel panelRoot;
        private AntdUI.GridPanel gridMain;
        private AntdUI.Panel panelRightCard;
        private AntdUI.Label labelScopeValue;
        private AntdUI.Label labelScopeTitle;
        private AntdUI.Label labelTimeValue;
        private AntdUI.Label labelTimeTitle;
        private AntdUI.Label labelResultValue;
        private AntdUI.Label labelRightTitle;
        private AntdUI.Panel panelLeftCard;
        private AntdUI.Panel panelActionButtons;
        private AntdUI.FlowPanel flowActionButtons;
        private AntdUI.Button buttonApplyLicense;
        private AntdUI.Button buttonExportRequest;
        private AntdUI.Label labelEnvironmentTitle;
        private AntdUI.Label labelEnvironmentValue;
        private AntdUI.Label labelHardwareValue;
        private AntdUI.Label labelHardwareTitle;
        private AntdUI.Label labelSoftwareValue;
        private AntdUI.Label labelSoftwareTitle;
        private AntdUI.Label labelLeftTitle;
    }
}
