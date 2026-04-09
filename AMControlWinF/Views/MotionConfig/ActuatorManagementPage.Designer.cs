namespace AMControlWinF.Views.MotionConfig
{
    partial class ActuatorManagementPage
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
            this.panelCardsHost = new AntdUI.Panel();
            this.flowCards = new AntdUI.FlowPanel();
            this.labelCardsTitle = new AntdUI.Label();
            this.panelCategoryBar = new AntdUI.Panel();
            this.flowCategories = new AntdUI.FlowPanel();
            this.buttonFilterGripper = new AntdUI.Button();
            this.buttonFilterStackLight = new AntdUI.Button();
            this.buttonFilterVacuum = new AntdUI.Button();
            this.buttonFilterCylinder = new AntdUI.Button();
            this.buttonFilterAll = new AntdUI.Button();
            this.panelToolbar = new AntdUI.Panel();
            this.flowToolbarRight = new AntdUI.FlowPanel();
            this.buttonAdd = new AntdUI.Button();
            this.buttonEdit = new AntdUI.Button();
            this.buttonDelete = new AntdUI.Button();
            this.panelToolbarLeftBlank = new AntdUI.Panel();
            this.panelRoot.SuspendLayout();
            this.panelCardsHost.SuspendLayout();
            this.panelCategoryBar.SuspendLayout();
            this.flowCategories.SuspendLayout();
            this.panelToolbar.SuspendLayout();
            this.flowToolbarRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.panelCardsHost);
            this.panelRoot.Controls.Add(this.panelCategoryBar);
            this.panelRoot.Controls.Add(this.panelToolbar);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Margin = new System.Windows.Forms.Padding(0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(8);
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(850, 680);
            this.panelRoot.TabIndex = 0;
            // 
            // panelCardsHost
            // 
            this.panelCardsHost.BackColor = System.Drawing.Color.Transparent;
            this.panelCardsHost.Controls.Add(this.flowCards);
            this.panelCardsHost.Controls.Add(this.labelCardsTitle);
            this.panelCardsHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCardsHost.Location = new System.Drawing.Point(8, 122);
            this.panelCardsHost.Margin = new System.Windows.Forms.Padding(0);
            this.panelCardsHost.Name = "panelCardsHost";
            this.panelCardsHost.Padding = new System.Windows.Forms.Padding(8);
            this.panelCardsHost.Radius = 12;
            this.panelCardsHost.Shadow = 4;
            this.panelCardsHost.Size = new System.Drawing.Size(834, 550);
            this.panelCardsHost.TabIndex = 2;
            // 
            // flowCards
            // 
            this.flowCards.AutoScroll = true;
            this.flowCards.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowCards.Gap = 12;
            this.flowCards.Location = new System.Drawing.Point(12, 48);
            this.flowCards.Margin = new System.Windows.Forms.Padding(0);
            this.flowCards.Name = "flowCards";
            this.flowCards.Size = new System.Drawing.Size(810, 490);
            this.flowCards.TabIndex = 1;
            this.flowCards.Text = "flowCards";
            // 
            // labelCardsTitle
            // 
            this.labelCardsTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelCardsTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.labelCardsTitle.Location = new System.Drawing.Point(12, 12);
            this.labelCardsTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelCardsTitle.Name = "labelCardsTitle";
            this.labelCardsTitle.Size = new System.Drawing.Size(810, 36);
            this.labelCardsTitle.TabIndex = 0;
            this.labelCardsTitle.Text = "执行器列表";
            this.labelCardsTitle.Visible = false;
            // 
            // panelCategoryBar
            // 
            this.panelCategoryBar.BackColor = System.Drawing.Color.Transparent;
            this.panelCategoryBar.Controls.Add(this.flowCategories);
            this.panelCategoryBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCategoryBar.Location = new System.Drawing.Point(8, 52);
            this.panelCategoryBar.Margin = new System.Windows.Forms.Padding(0);
            this.panelCategoryBar.Name = "panelCategoryBar";
            this.panelCategoryBar.Radius = 0;
            this.panelCategoryBar.Size = new System.Drawing.Size(834, 70);
            this.panelCategoryBar.TabIndex = 1;
            // 
            // flowCategories
            // 
            this.flowCategories.Controls.Add(this.buttonFilterGripper);
            this.flowCategories.Controls.Add(this.buttonFilterStackLight);
            this.flowCategories.Controls.Add(this.buttonFilterVacuum);
            this.flowCategories.Controls.Add(this.buttonFilterCylinder);
            this.flowCategories.Controls.Add(this.buttonFilterAll);
            this.flowCategories.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowCategories.Gap = 8;
            this.flowCategories.Location = new System.Drawing.Point(0, 0);
            this.flowCategories.Margin = new System.Windows.Forms.Padding(0);
            this.flowCategories.Name = "flowCategories";
            this.flowCategories.Padding = new System.Windows.Forms.Padding(4, 18, 4, 6);
            this.flowCategories.Size = new System.Drawing.Size(834, 70);
            this.flowCategories.TabIndex = 0;
            this.flowCategories.Text = "flowCategories";
            // 
            // buttonFilterGripper
            // 
            this.buttonFilterGripper.Location = new System.Drawing.Point(324, 18);
            this.buttonFilterGripper.Margin = new System.Windows.Forms.Padding(0);
            this.buttonFilterGripper.Name = "buttonFilterGripper";
            this.buttonFilterGripper.Radius = 8;
            this.buttonFilterGripper.Size = new System.Drawing.Size(72, 40);
            this.buttonFilterGripper.TabIndex = 4;
            this.buttonFilterGripper.Text = "夹爪";
            this.buttonFilterGripper.WaveSize = 0;
            // 
            // buttonFilterStackLight
            // 
            this.buttonFilterStackLight.Location = new System.Drawing.Point(244, 18);
            this.buttonFilterStackLight.Margin = new System.Windows.Forms.Padding(0);
            this.buttonFilterStackLight.Name = "buttonFilterStackLight";
            this.buttonFilterStackLight.Radius = 8;
            this.buttonFilterStackLight.Size = new System.Drawing.Size(72, 40);
            this.buttonFilterStackLight.TabIndex = 3;
            this.buttonFilterStackLight.Text = "灯塔";
            this.buttonFilterStackLight.WaveSize = 0;
            // 
            // buttonFilterVacuum
            // 
            this.buttonFilterVacuum.Location = new System.Drawing.Point(164, 18);
            this.buttonFilterVacuum.Margin = new System.Windows.Forms.Padding(0);
            this.buttonFilterVacuum.Name = "buttonFilterVacuum";
            this.buttonFilterVacuum.Radius = 8;
            this.buttonFilterVacuum.Size = new System.Drawing.Size(72, 40);
            this.buttonFilterVacuum.TabIndex = 2;
            this.buttonFilterVacuum.Text = "真空";
            this.buttonFilterVacuum.WaveSize = 0;
            // 
            // buttonFilterCylinder
            // 
            this.buttonFilterCylinder.Location = new System.Drawing.Point(84, 18);
            this.buttonFilterCylinder.Margin = new System.Windows.Forms.Padding(0);
            this.buttonFilterCylinder.Name = "buttonFilterCylinder";
            this.buttonFilterCylinder.Radius = 8;
            this.buttonFilterCylinder.Size = new System.Drawing.Size(72, 40);
            this.buttonFilterCylinder.TabIndex = 1;
            this.buttonFilterCylinder.Text = "气缸";
            this.buttonFilterCylinder.WaveSize = 0;
            // 
            // buttonFilterAll
            // 
            this.buttonFilterAll.Location = new System.Drawing.Point(4, 18);
            this.buttonFilterAll.Margin = new System.Windows.Forms.Padding(0);
            this.buttonFilterAll.Name = "buttonFilterAll";
            this.buttonFilterAll.Radius = 8;
            this.buttonFilterAll.Size = new System.Drawing.Size(72, 40);
            this.buttonFilterAll.TabIndex = 0;
            this.buttonFilterAll.Text = "全部";
            this.buttonFilterAll.Type = AntdUI.TTypeMini.Primary;
            this.buttonFilterAll.WaveSize = 0;
            // 
            // panelToolbar
            // 
            this.panelToolbar.BackColor = System.Drawing.Color.Transparent;
            this.panelToolbar.Controls.Add(this.flowToolbarRight);
            this.panelToolbar.Controls.Add(this.panelToolbarLeftBlank);
            this.panelToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelToolbar.Location = new System.Drawing.Point(8, 8);
            this.panelToolbar.Margin = new System.Windows.Forms.Padding(0);
            this.panelToolbar.Name = "panelToolbar";
            this.panelToolbar.Padding = new System.Windows.Forms.Padding(4);
            this.panelToolbar.Radius = 0;
            this.panelToolbar.Size = new System.Drawing.Size(834, 44);
            this.panelToolbar.TabIndex = 0;
            // 
            // flowToolbarRight
            // 
            this.flowToolbarRight.Controls.Add(this.buttonAdd);
            this.flowToolbarRight.Controls.Add(this.buttonEdit);
            this.flowToolbarRight.Controls.Add(this.buttonDelete);
            this.flowToolbarRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowToolbarRight.Gap = 8;
            this.flowToolbarRight.Location = new System.Drawing.Point(450, 4);
            this.flowToolbarRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowToolbarRight.Name = "flowToolbarRight";
            this.flowToolbarRight.Size = new System.Drawing.Size(380, 36);
            this.flowToolbarRight.TabIndex = 1;
            this.flowToolbarRight.Text = "flowToolbarRight";
            // 
            // buttonAdd
            // 
            this.buttonAdd.IconSvg = "PlusOutlined";
            this.buttonAdd.Location = new System.Drawing.Point(256, 0);
            this.buttonAdd.Margin = new System.Windows.Forms.Padding(0);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Radius = 8;
            this.buttonAdd.Size = new System.Drawing.Size(120, 36);
            this.buttonAdd.TabIndex = 0;
            this.buttonAdd.Text = "新增";
            this.buttonAdd.Type = AntdUI.TTypeMini.Primary;
            this.buttonAdd.WaveSize = 0;
            // 
            // buttonEdit
            // 
            this.buttonEdit.IconSvg = "EditOutlined";
            this.buttonEdit.Location = new System.Drawing.Point(128, 0);
            this.buttonEdit.Margin = new System.Windows.Forms.Padding(0);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Radius = 8;
            this.buttonEdit.Size = new System.Drawing.Size(120, 36);
            this.buttonEdit.TabIndex = 1;
            this.buttonEdit.Text = "编辑";
            this.buttonEdit.WaveSize = 0;
            // 
            // buttonDelete
            // 
            this.buttonDelete.IconSvg = "DeleteOutlined";
            this.buttonDelete.Location = new System.Drawing.Point(0, 0);
            this.buttonDelete.Margin = new System.Windows.Forms.Padding(0);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Radius = 8;
            this.buttonDelete.Size = new System.Drawing.Size(120, 36);
            this.buttonDelete.TabIndex = 2;
            this.buttonDelete.Text = "删除";
            this.buttonDelete.WaveSize = 0;
            // 
            // panelToolbarLeftBlank
            // 
            this.panelToolbarLeftBlank.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelToolbarLeftBlank.Location = new System.Drawing.Point(4, 4);
            this.panelToolbarLeftBlank.Margin = new System.Windows.Forms.Padding(0);
            this.panelToolbarLeftBlank.Name = "panelToolbarLeftBlank";
            this.panelToolbarLeftBlank.Radius = 0;
            this.panelToolbarLeftBlank.Size = new System.Drawing.Size(420, 36);
            this.panelToolbarLeftBlank.TabIndex = 0;
            this.panelToolbarLeftBlank.Text = "panelToolbarLeftBlank";
            // 
            // ActuatorManagementPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ActuatorManagementPage";
            this.Size = new System.Drawing.Size(850, 680);
            this.panelRoot.ResumeLayout(false);
            this.panelCardsHost.ResumeLayout(false);
            this.panelCategoryBar.ResumeLayout(false);
            this.flowCategories.ResumeLayout(false);
            this.panelToolbar.ResumeLayout(false);
            this.flowToolbarRight.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private AntdUI.Panel panelRoot;
        private AntdUI.Panel panelToolbar;
        private AntdUI.Panel panelToolbarLeftBlank;
        private AntdUI.FlowPanel flowToolbarRight;
        private AntdUI.Button buttonAdd;
        private AntdUI.Button buttonEdit;
        private AntdUI.Button buttonDelete;
        private AntdUI.Panel panelCategoryBar;
        private AntdUI.FlowPanel flowCategories;
        private AntdUI.Button buttonFilterAll;
        private AntdUI.Button buttonFilterCylinder;
        private AntdUI.Button buttonFilterVacuum;
        private AntdUI.Button buttonFilterStackLight;
        private AntdUI.Button buttonFilterGripper;
        private AntdUI.Panel panelCardsHost;
        private AntdUI.Label labelCardsTitle;
        private AntdUI.FlowPanel flowCards;
    }
}