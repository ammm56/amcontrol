namespace AMControlWinF.Views.Motion
{
    partial class MotionActuatorPage
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
            this.panelContent = new AntdUI.Panel();
            this.gridContent = new AntdUI.GridPanel();
            this.panelRight = new AntdUI.Panel();
            this.tableRight = new System.Windows.Forms.TableLayoutPanel();
            this.actuatorActionPanelControl = new AMControlWinF.Views.Motion.MotionActuatorActionPanelControl();
            this.actuatorDetailControl = new AMControlWinF.Views.Motion.MotionActuatorDetailControl();
            this.panelLeft = new AntdUI.Panel();
            this.actuatorVirtualListControl = new AMControlWinF.Views.Motion.MotionActuatorVirtualListControl();
            this.panelSummary = new AntdUI.Panel();
            this.panelCylinderSummary = new AntdUI.Panel();
            this.labelCylinderCount = new AntdUI.Label();
            this.labelCylinderTitle = new AntdUI.Label();
            this.panelVacuumSummary = new AntdUI.Panel();
            this.labelVacuumCount = new AntdUI.Label();
            this.labelVacuumTitle = new AntdUI.Label();
            this.panelGripperSummary = new AntdUI.Panel();
            this.labelGripperCount = new AntdUI.Label();
            this.labelGripperTitle = new AntdUI.Label();
            this.panelStackLightSummary = new AntdUI.Panel();
            this.labelStackLightCount = new AntdUI.Label();
            this.labelStackLightTitle = new AntdUI.Label();
            this.panelToolbar = new AntdUI.Panel();
            this.flowToolbarRight = new AntdUI.FlowPanel();
            this.inputSearch = new AntdUI.Input();
            this.flowToolbarLeft = new AntdUI.FlowPanel();
            this.buttonFilterStackLight = new AntdUI.Button();
            this.buttonFilterGripper = new AntdUI.Button();
            this.buttonFilterVacuum = new AntdUI.Button();
            this.buttonFilterCylinder = new AntdUI.Button();
            this.buttonFilterAll = new AntdUI.Button();
            this.tableSummary = new System.Windows.Forms.TableLayoutPanel();
            this.panelRoot.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.gridContent.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.tableRight.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.panelSummary.SuspendLayout();
            this.panelCylinderSummary.SuspendLayout();
            this.panelVacuumSummary.SuspendLayout();
            this.panelGripperSummary.SuspendLayout();
            this.panelStackLightSummary.SuspendLayout();
            this.panelToolbar.SuspendLayout();
            this.flowToolbarRight.SuspendLayout();
            this.flowToolbarLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.panelContent);
            this.panelRoot.Controls.Add(this.panelSummary);
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
            // panelContent
            // 
            this.panelContent.Controls.Add(this.gridContent);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(8, 128);
            this.panelContent.Margin = new System.Windows.Forms.Padding(0);
            this.panelContent.Name = "panelContent";
            this.panelContent.Radius = 0;
            this.panelContent.ShadowOpacity = 0F;
            this.panelContent.ShadowOpacityHover = 0F;
            this.panelContent.Size = new System.Drawing.Size(834, 544);
            this.panelContent.TabIndex = 2;
            // 
            // gridContent
            // 
            this.gridContent.Controls.Add(this.panelRight);
            this.gridContent.Controls.Add(this.panelLeft);
            this.gridContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridContent.Location = new System.Drawing.Point(0, 0);
            this.gridContent.Margin = new System.Windows.Forms.Padding(0);
            this.gridContent.Name = "gridContent";
            this.gridContent.Size = new System.Drawing.Size(834, 544);
            this.gridContent.Span = "100% 320";
            this.gridContent.TabIndex = 0;
            // 
            // panelRight
            // 
            this.panelRight.Controls.Add(this.tableRight);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRight.Location = new System.Drawing.Point(514, 0);
            this.panelRight.Margin = new System.Windows.Forms.Padding(0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Radius = 12;
            this.panelRight.Shadow = 4;
            this.panelRight.Size = new System.Drawing.Size(320, 544);
            this.panelRight.TabIndex = 1;
            // 
            // tableRight
            // 
            this.tableRight.ColumnCount = 1;
            this.tableRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableRight.Controls.Add(this.actuatorActionPanelControl, 0, 0);
            this.tableRight.Controls.Add(this.actuatorDetailControl, 0, 1);
            this.tableRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableRight.Location = new System.Drawing.Point(4, 4);
            this.tableRight.Margin = new System.Windows.Forms.Padding(0);
            this.tableRight.Name = "tableRight";
            this.tableRight.RowCount = 2;
            this.tableRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 196F));
            this.tableRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableRight.Size = new System.Drawing.Size(312, 536);
            this.tableRight.TabIndex = 0;
            // 
            // actuatorActionPanelControl
            // 
            this.actuatorActionPanelControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actuatorActionPanelControl.Location = new System.Drawing.Point(0, 0);
            this.actuatorActionPanelControl.Margin = new System.Windows.Forms.Padding(0);
            this.actuatorActionPanelControl.Name = "actuatorActionPanelControl";
            this.actuatorActionPanelControl.Size = new System.Drawing.Size(312, 196);
            this.actuatorActionPanelControl.TabIndex = 0;
            // 
            // actuatorDetailControl
            // 
            this.actuatorDetailControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actuatorDetailControl.Location = new System.Drawing.Point(0, 196);
            this.actuatorDetailControl.Margin = new System.Windows.Forms.Padding(0);
            this.actuatorDetailControl.Name = "actuatorDetailControl";
            this.actuatorDetailControl.Size = new System.Drawing.Size(312, 340);
            this.actuatorDetailControl.TabIndex = 1;
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.panelStackLightSummary);
            this.panelLeft.Controls.Add(this.panelGripperSummary);
            this.panelLeft.Controls.Add(this.panelVacuumSummary);
            this.panelLeft.Controls.Add(this.panelCylinderSummary);
            this.panelLeft.Controls.Add(this.actuatorVirtualListControl);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Margin = new System.Windows.Forms.Padding(0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Radius = 0;
            this.panelLeft.ShadowOpacity = 0F;
            this.panelLeft.ShadowOpacityHover = 0F;
            this.panelLeft.Size = new System.Drawing.Size(514, 544);
            this.panelLeft.TabIndex = 0;
            // 
            // actuatorVirtualListControl
            // 
            this.actuatorVirtualListControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actuatorVirtualListControl.Location = new System.Drawing.Point(0, 0);
            this.actuatorVirtualListControl.Margin = new System.Windows.Forms.Padding(0);
            this.actuatorVirtualListControl.Name = "actuatorVirtualListControl";
            this.actuatorVirtualListControl.Size = new System.Drawing.Size(514, 544);
            this.actuatorVirtualListControl.TabIndex = 0;
            // 
            // panelSummary
            // 
            this.panelSummary.Controls.Add(this.tableSummary);
            this.panelSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSummary.Location = new System.Drawing.Point(8, 52);
            this.panelSummary.Margin = new System.Windows.Forms.Padding(0);
            this.panelSummary.Name = "panelSummary";
            this.panelSummary.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.panelSummary.Radius = 0;
            this.panelSummary.Size = new System.Drawing.Size(834, 76);
            this.panelSummary.TabIndex = 1;
            // 
            // panelCylinderSummary
            // 
            this.panelCylinderSummary.Controls.Add(this.labelCylinderCount);
            this.panelCylinderSummary.Controls.Add(this.labelCylinderTitle);
            this.panelCylinderSummary.Location = new System.Drawing.Point(67, 52);
            this.panelCylinderSummary.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.panelCylinderSummary.Name = "panelCylinderSummary";
            this.panelCylinderSummary.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.panelCylinderSummary.Radius = 10;
            this.panelCylinderSummary.Size = new System.Drawing.Size(202, 60);
            this.panelCylinderSummary.TabIndex = 0;
            // 
            // labelCylinderCount
            // 
            this.labelCylinderCount.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelCylinderCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Bold);
            this.labelCylinderCount.Location = new System.Drawing.Point(12, 28);
            this.labelCylinderCount.Name = "labelCylinderCount";
            this.labelCylinderCount.Size = new System.Drawing.Size(178, 24);
            this.labelCylinderCount.TabIndex = 1;
            this.labelCylinderCount.Text = "0";
            // 
            // labelCylinderTitle
            // 
            this.labelCylinderTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelCylinderTitle.Location = new System.Drawing.Point(12, 8);
            this.labelCylinderTitle.Name = "labelCylinderTitle";
            this.labelCylinderTitle.Size = new System.Drawing.Size(178, 18);
            this.labelCylinderTitle.TabIndex = 0;
            this.labelCylinderTitle.Text = "气缸";
            // 
            // panelVacuumSummary
            // 
            this.panelVacuumSummary.Controls.Add(this.labelVacuumCount);
            this.panelVacuumSummary.Controls.Add(this.labelVacuumTitle);
            this.panelVacuumSummary.Location = new System.Drawing.Point(281, 80);
            this.panelVacuumSummary.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.panelVacuumSummary.Name = "panelVacuumSummary";
            this.panelVacuumSummary.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.panelVacuumSummary.Radius = 10;
            this.panelVacuumSummary.Size = new System.Drawing.Size(196, 60);
            this.panelVacuumSummary.TabIndex = 1;
            // 
            // labelVacuumCount
            // 
            this.labelVacuumCount.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelVacuumCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Bold);
            this.labelVacuumCount.Location = new System.Drawing.Point(12, 28);
            this.labelVacuumCount.Name = "labelVacuumCount";
            this.labelVacuumCount.Size = new System.Drawing.Size(172, 24);
            this.labelVacuumCount.TabIndex = 1;
            this.labelVacuumCount.Text = "0";
            // 
            // labelVacuumTitle
            // 
            this.labelVacuumTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelVacuumTitle.Location = new System.Drawing.Point(12, 8);
            this.labelVacuumTitle.Name = "labelVacuumTitle";
            this.labelVacuumTitle.Size = new System.Drawing.Size(172, 18);
            this.labelVacuumTitle.TabIndex = 0;
            this.labelVacuumTitle.Text = "真空";
            // 
            // panelGripperSummary
            // 
            this.panelGripperSummary.Controls.Add(this.labelGripperCount);
            this.panelGripperSummary.Controls.Add(this.labelGripperTitle);
            this.panelGripperSummary.Location = new System.Drawing.Point(281, 171);
            this.panelGripperSummary.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.panelGripperSummary.Name = "panelGripperSummary";
            this.panelGripperSummary.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.panelGripperSummary.Radius = 10;
            this.panelGripperSummary.Size = new System.Drawing.Size(196, 60);
            this.panelGripperSummary.TabIndex = 2;
            // 
            // labelGripperCount
            // 
            this.labelGripperCount.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelGripperCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Bold);
            this.labelGripperCount.Location = new System.Drawing.Point(12, 28);
            this.labelGripperCount.Name = "labelGripperCount";
            this.labelGripperCount.Size = new System.Drawing.Size(172, 24);
            this.labelGripperCount.TabIndex = 1;
            this.labelGripperCount.Text = "0";
            // 
            // labelGripperTitle
            // 
            this.labelGripperTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelGripperTitle.Location = new System.Drawing.Point(12, 8);
            this.labelGripperTitle.Name = "labelGripperTitle";
            this.labelGripperTitle.Size = new System.Drawing.Size(172, 18);
            this.labelGripperTitle.TabIndex = 0;
            this.labelGripperTitle.Text = "夹爪";
            // 
            // panelStackLightSummary
            // 
            this.panelStackLightSummary.Controls.Add(this.labelStackLightCount);
            this.panelStackLightSummary.Controls.Add(this.labelStackLightTitle);
            this.panelStackLightSummary.Location = new System.Drawing.Point(293, 293);
            this.panelStackLightSummary.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.panelStackLightSummary.Name = "panelStackLightSummary";
            this.panelStackLightSummary.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.panelStackLightSummary.Radius = 10;
            this.panelStackLightSummary.Size = new System.Drawing.Size(204, 60);
            this.panelStackLightSummary.TabIndex = 3;
            // 
            // labelStackLightCount
            // 
            this.labelStackLightCount.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelStackLightCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Bold);
            this.labelStackLightCount.Location = new System.Drawing.Point(12, 28);
            this.labelStackLightCount.Name = "labelStackLightCount";
            this.labelStackLightCount.Size = new System.Drawing.Size(180, 24);
            this.labelStackLightCount.TabIndex = 1;
            this.labelStackLightCount.Text = "0";
            // 
            // labelStackLightTitle
            // 
            this.labelStackLightTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelStackLightTitle.Location = new System.Drawing.Point(12, 8);
            this.labelStackLightTitle.Name = "labelStackLightTitle";
            this.labelStackLightTitle.Size = new System.Drawing.Size(180, 18);
            this.labelStackLightTitle.TabIndex = 0;
            this.labelStackLightTitle.Text = "灯塔";
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
            this.panelToolbar.Size = new System.Drawing.Size(834, 44);
            this.panelToolbar.TabIndex = 0;
            // 
            // flowToolbarRight
            // 
            this.flowToolbarRight.Controls.Add(this.inputSearch);
            this.flowToolbarRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowToolbarRight.Gap = 8;
            this.flowToolbarRight.Location = new System.Drawing.Point(598, 4);
            this.flowToolbarRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowToolbarRight.Name = "flowToolbarRight";
            this.flowToolbarRight.Size = new System.Drawing.Size(232, 36);
            this.flowToolbarRight.TabIndex = 1;
            // 
            // inputSearch
            // 
            this.inputSearch.Location = new System.Drawing.Point(0, 0);
            this.inputSearch.Margin = new System.Windows.Forms.Padding(0);
            this.inputSearch.Name = "inputSearch";
            this.inputSearch.PlaceholderText = "搜索名称 / 类型 / 状态 / 最近操作";
            this.inputSearch.Size = new System.Drawing.Size(232, 36);
            this.inputSearch.TabIndex = 0;
            this.inputSearch.WaveSize = 0;
            // 
            // flowToolbarLeft
            // 
            this.flowToolbarLeft.Controls.Add(this.buttonFilterStackLight);
            this.flowToolbarLeft.Controls.Add(this.buttonFilterGripper);
            this.flowToolbarLeft.Controls.Add(this.buttonFilterVacuum);
            this.flowToolbarLeft.Controls.Add(this.buttonFilterCylinder);
            this.flowToolbarLeft.Controls.Add(this.buttonFilterAll);
            this.flowToolbarLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowToolbarLeft.Gap = 8;
            this.flowToolbarLeft.Location = new System.Drawing.Point(4, 4);
            this.flowToolbarLeft.Margin = new System.Windows.Forms.Padding(0);
            this.flowToolbarLeft.Name = "flowToolbarLeft";
            this.flowToolbarLeft.Size = new System.Drawing.Size(520, 36);
            this.flowToolbarLeft.TabIndex = 0;
            // 
            // buttonFilterStackLight
            // 
            this.buttonFilterStackLight.Location = new System.Drawing.Point(320, 0);
            this.buttonFilterStackLight.Margin = new System.Windows.Forms.Padding(0);
            this.buttonFilterStackLight.Name = "buttonFilterStackLight";
            this.buttonFilterStackLight.Radius = 8;
            this.buttonFilterStackLight.Size = new System.Drawing.Size(72, 36);
            this.buttonFilterStackLight.TabIndex = 4;
            this.buttonFilterStackLight.Text = "灯塔";
            this.buttonFilterStackLight.WaveSize = 0;
            // 
            // buttonFilterGripper
            // 
            this.buttonFilterGripper.Location = new System.Drawing.Point(240, 0);
            this.buttonFilterGripper.Margin = new System.Windows.Forms.Padding(0);
            this.buttonFilterGripper.Name = "buttonFilterGripper";
            this.buttonFilterGripper.Radius = 8;
            this.buttonFilterGripper.Size = new System.Drawing.Size(72, 36);
            this.buttonFilterGripper.TabIndex = 3;
            this.buttonFilterGripper.Text = "夹爪";
            this.buttonFilterGripper.WaveSize = 0;
            // 
            // buttonFilterVacuum
            // 
            this.buttonFilterVacuum.Location = new System.Drawing.Point(160, 0);
            this.buttonFilterVacuum.Margin = new System.Windows.Forms.Padding(0);
            this.buttonFilterVacuum.Name = "buttonFilterVacuum";
            this.buttonFilterVacuum.Radius = 8;
            this.buttonFilterVacuum.Size = new System.Drawing.Size(72, 36);
            this.buttonFilterVacuum.TabIndex = 2;
            this.buttonFilterVacuum.Text = "真空";
            this.buttonFilterVacuum.WaveSize = 0;
            // 
            // buttonFilterCylinder
            // 
            this.buttonFilterCylinder.Location = new System.Drawing.Point(80, 0);
            this.buttonFilterCylinder.Margin = new System.Windows.Forms.Padding(0);
            this.buttonFilterCylinder.Name = "buttonFilterCylinder";
            this.buttonFilterCylinder.Radius = 8;
            this.buttonFilterCylinder.Size = new System.Drawing.Size(72, 36);
            this.buttonFilterCylinder.TabIndex = 1;
            this.buttonFilterCylinder.Text = "气缸";
            this.buttonFilterCylinder.WaveSize = 0;
            // 
            // buttonFilterAll
            // 
            this.buttonFilterAll.Location = new System.Drawing.Point(0, 0);
            this.buttonFilterAll.Margin = new System.Windows.Forms.Padding(0);
            this.buttonFilterAll.Name = "buttonFilterAll";
            this.buttonFilterAll.Radius = 8;
            this.buttonFilterAll.Size = new System.Drawing.Size(72, 36);
            this.buttonFilterAll.TabIndex = 0;
            this.buttonFilterAll.Text = "全部";
            this.buttonFilterAll.Type = AntdUI.TTypeMini.Primary;
            this.buttonFilterAll.WaveSize = 0;
            // 
            // tableSummary
            // 
            this.tableSummary.ColumnCount = 1;
            this.tableSummary.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableSummary.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableSummary.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableSummary.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableSummary.Location = new System.Drawing.Point(0, 8);
            this.tableSummary.Margin = new System.Windows.Forms.Padding(0);
            this.tableSummary.Name = "tableSummary";
            this.tableSummary.RowCount = 1;
            this.tableSummary.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableSummary.Size = new System.Drawing.Size(834, 60);
            this.tableSummary.TabIndex = 0;
            // 
            // MotionActuatorPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MotionActuatorPage";
            this.Size = new System.Drawing.Size(850, 680);
            this.panelRoot.ResumeLayout(false);
            this.panelContent.ResumeLayout(false);
            this.gridContent.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.tableRight.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            this.panelSummary.ResumeLayout(false);
            this.panelCylinderSummary.ResumeLayout(false);
            this.panelVacuumSummary.ResumeLayout(false);
            this.panelGripperSummary.ResumeLayout(false);
            this.panelStackLightSummary.ResumeLayout(false);
            this.panelToolbar.ResumeLayout(false);
            this.flowToolbarRight.ResumeLayout(false);
            this.flowToolbarLeft.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AntdUI.Panel panelRoot;
        private AntdUI.Panel panelToolbar;
        private AntdUI.FlowPanel flowToolbarLeft;
        private AntdUI.Button buttonFilterAll;
        private AntdUI.Button buttonFilterCylinder;
        private AntdUI.Button buttonFilterVacuum;
        private AntdUI.Button buttonFilterGripper;
        private AntdUI.Button buttonFilterStackLight;
        private AntdUI.FlowPanel flowToolbarRight;
        private AntdUI.Input inputSearch;
        private AntdUI.Panel panelSummary;
        private AntdUI.Panel panelCylinderSummary;
        private AntdUI.Label labelCylinderTitle;
        private AntdUI.Label labelCylinderCount;
        private AntdUI.Panel panelVacuumSummary;
        private AntdUI.Label labelVacuumTitle;
        private AntdUI.Label labelVacuumCount;
        private AntdUI.Panel panelGripperSummary;
        private AntdUI.Label labelGripperTitle;
        private AntdUI.Label labelGripperCount;
        private AntdUI.Panel panelStackLightSummary;
        private AntdUI.Label labelStackLightTitle;
        private AntdUI.Label labelStackLightCount;
        private AntdUI.Panel panelContent;
        private AntdUI.GridPanel gridContent;
        private AntdUI.Panel panelLeft;
        private MotionActuatorVirtualListControl actuatorVirtualListControl;
        private AntdUI.Panel panelRight;
        private System.Windows.Forms.TableLayoutPanel tableRight;
        private MotionActuatorActionPanelControl actuatorActionPanelControl;
        private MotionActuatorDetailControl actuatorDetailControl;
        private System.Windows.Forms.TableLayoutPanel tableSummary;
    }
}