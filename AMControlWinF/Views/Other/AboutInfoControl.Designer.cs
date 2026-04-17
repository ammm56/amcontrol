namespace AMControlWinF.Views.Other
{
    partial class AboutInfoControl
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
            this.gridMain = new AntdUI.GridPanel();
            this.panelRightCard = new AntdUI.Panel();
            this.tableLibraries = new AntdUI.Table();
            this.labelLibrariesTitle = new AntdUI.Label();
            this.panelLeftCard = new AntdUI.Panel();
            this.labelVersionValue = new AntdUI.Label();
            this.labelcopyright = new AntdUI.Label();
            this.labelunity = new AntdUI.Label();
            this.labelemail = new AntdUI.Label();
            this.labelAuthorValue = new AntdUI.Label();
            this.labelProgramName = new AntdUI.Label();
            this.panelRoot.SuspendLayout();
            this.gridMain.SuspendLayout();
            this.panelRightCard.SuspendLayout();
            this.panelLeftCard.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.gridMain);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Margin = new System.Windows.Forms.Padding(0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(8);
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(760, 500);
            this.panelRoot.TabIndex = 0;
            // 
            // gridMain
            // 
            this.gridMain.Controls.Add(this.panelRightCard);
            this.gridMain.Controls.Add(this.panelLeftCard);
            this.gridMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridMain.Location = new System.Drawing.Point(8, 8);
            this.gridMain.Margin = new System.Windows.Forms.Padding(0);
            this.gridMain.Name = "gridMain";
            this.gridMain.Size = new System.Drawing.Size(744, 484);
            this.gridMain.Span = "40% 60%";
            this.gridMain.TabIndex = 0;
            // 
            // panelRightCard
            // 
            this.panelRightCard.Controls.Add(this.tableLibraries);
            this.panelRightCard.Controls.Add(this.labelLibrariesTitle);
            this.panelRightCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRightCard.Location = new System.Drawing.Point(298, 0);
            this.panelRightCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelRightCard.Name = "panelRightCard";
            this.panelRightCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelRightCard.Radius = 12;
            this.panelRightCard.Shadow = 4;
            this.panelRightCard.Size = new System.Drawing.Size(446, 484);
            this.panelRightCard.TabIndex = 1;
            // 
            // tableLibraries
            // 
            this.tableLibraries.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tableLibraries.BackColor = System.Drawing.Color.Transparent;
            this.tableLibraries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLibraries.EmptyHeader = true;
            this.tableLibraries.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.tableLibraries.Gap = 8;
            this.tableLibraries.Gaps = new System.Drawing.Size(8, 8);
            this.tableLibraries.Location = new System.Drawing.Point(16, 52);
            this.tableLibraries.Margin = new System.Windows.Forms.Padding(0);
            this.tableLibraries.Name = "tableLibraries";
            this.tableLibraries.ShowTip = false;
            this.tableLibraries.Size = new System.Drawing.Size(414, 416);
            this.tableLibraries.TabIndex = 1;
            this.tableLibraries.Text = "tableLibraries";
            // 
            // labelLibrariesTitle
            // 
            this.labelLibrariesTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelLibrariesTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelLibrariesTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.labelLibrariesTitle.Location = new System.Drawing.Point(16, 16);
            this.labelLibrariesTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelLibrariesTitle.Name = "labelLibrariesTitle";
            this.labelLibrariesTitle.Size = new System.Drawing.Size(414, 36);
            this.labelLibrariesTitle.TabIndex = 0;
            this.labelLibrariesTitle.Text = "第三方库与协议";
            // 
            // panelLeftCard
            // 
            this.panelLeftCard.Controls.Add(this.labelVersionValue);
            this.panelLeftCard.Controls.Add(this.labelcopyright);
            this.panelLeftCard.Controls.Add(this.labelunity);
            this.panelLeftCard.Controls.Add(this.labelemail);
            this.panelLeftCard.Controls.Add(this.labelAuthorValue);
            this.panelLeftCard.Controls.Add(this.labelProgramName);
            this.panelLeftCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLeftCard.Location = new System.Drawing.Point(0, 0);
            this.panelLeftCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelLeftCard.Name = "panelLeftCard";
            this.panelLeftCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelLeftCard.Radius = 12;
            this.panelLeftCard.Shadow = 4;
            this.panelLeftCard.Size = new System.Drawing.Size(298, 484);
            this.panelLeftCard.TabIndex = 0;
            // 
            // labelVersionValue
            // 
            this.labelVersionValue.BackColor = System.Drawing.Color.Transparent;
            this.labelVersionValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelVersionValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.labelVersionValue.Location = new System.Drawing.Point(16, 168);
            this.labelVersionValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelVersionValue.Name = "labelVersionValue";
            this.labelVersionValue.Size = new System.Drawing.Size(266, 28);
            this.labelVersionValue.TabIndex = 10;
            this.labelVersionValue.Text = "版本：0.0.1";
            // 
            // labelcopyright
            // 
            this.labelcopyright.BackColor = System.Drawing.Color.Transparent;
            this.labelcopyright.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelcopyright.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.labelcopyright.Location = new System.Drawing.Point(16, 140);
            this.labelcopyright.Margin = new System.Windows.Forms.Padding(0);
            this.labelcopyright.Name = "labelcopyright";
            this.labelcopyright.Size = new System.Drawing.Size(266, 28);
            this.labelcopyright.TabIndex = 7;
            this.labelcopyright.Text = "版权所有：amm";
            this.labelcopyright.Visible = false;
            // 
            // labelunity
            // 
            this.labelunity.BackColor = System.Drawing.Color.Transparent;
            this.labelunity.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelunity.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.labelunity.Location = new System.Drawing.Point(16, 112);
            this.labelunity.Margin = new System.Windows.Forms.Padding(0);
            this.labelunity.Name = "labelunity";
            this.labelunity.Size = new System.Drawing.Size(266, 28);
            this.labelunity.TabIndex = 6;
            this.labelunity.Text = "单位：amm";
            this.labelunity.Visible = false;
            // 
            // labelemail
            // 
            this.labelemail.BackColor = System.Drawing.Color.Transparent;
            this.labelemail.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelemail.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.labelemail.Location = new System.Drawing.Point(16, 84);
            this.labelemail.Margin = new System.Windows.Forms.Padding(0);
            this.labelemail.Name = "labelemail";
            this.labelemail.Size = new System.Drawing.Size(266, 28);
            this.labelemail.TabIndex = 5;
            this.labelemail.Text = "邮箱：tang.am@foxmail.com";
            // 
            // labelAuthorValue
            // 
            this.labelAuthorValue.BackColor = System.Drawing.Color.Transparent;
            this.labelAuthorValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelAuthorValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.labelAuthorValue.Location = new System.Drawing.Point(16, 56);
            this.labelAuthorValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelAuthorValue.Name = "labelAuthorValue";
            this.labelAuthorValue.Size = new System.Drawing.Size(266, 28);
            this.labelAuthorValue.TabIndex = 3;
            this.labelAuthorValue.Text = "开发者：amm";
            // 
            // labelProgramName
            // 
            this.labelProgramName.BackColor = System.Drawing.Color.Transparent;
            this.labelProgramName.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelProgramName.Font = new System.Drawing.Font("Microsoft YaHei UI", 13F, System.Drawing.FontStyle.Bold);
            this.labelProgramName.Location = new System.Drawing.Point(16, 16);
            this.labelProgramName.Margin = new System.Windows.Forms.Padding(0);
            this.labelProgramName.Name = "labelProgramName";
            this.labelProgramName.Size = new System.Drawing.Size(266, 40);
            this.labelProgramName.TabIndex = 1;
            this.labelProgramName.Text = "AM运动控制程序";
            // 
            // AboutInfoControl
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panelRoot);
            this.Name = "AboutInfoControl";
            this.Size = new System.Drawing.Size(760, 500);
            this.panelRoot.ResumeLayout(false);
            this.gridMain.ResumeLayout(false);
            this.panelRightCard.ResumeLayout(false);
            this.panelLeftCard.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.Panel panelRoot;
        private AntdUI.GridPanel gridMain;
        private AntdUI.Panel panelLeftCard;
        private AntdUI.Label labelProgramName;
        private AntdUI.Label labelVersionValue;
        private AntdUI.Label labelAuthorValue;
        private AntdUI.Panel panelRightCard;
        private AntdUI.Label labelLibrariesTitle;
        private AntdUI.Table tableLibraries;
        private AntdUI.Label labelcopyright;
        private AntdUI.Label labelunity;
        private AntdUI.Label labelemail;
    }
}