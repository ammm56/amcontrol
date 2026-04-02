namespace AMControlWinF.Views.Am
{
    partial class UserManagementPage
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.panelRoot = new AntdUI.Panel();
            this.panelTableCard = new AntdUI.Panel();
            this.tableUsers = new AntdUI.Table();
            this.flowStats = new AntdUI.FlowPanel();
            this.panelDisabledCard = new AntdUI.Panel();
            this.labelDisabledCount = new AntdUI.Label();
            this.labelDisabledTitle = new AntdUI.Label();
            this.panelEnabledCard = new AntdUI.Panel();
            this.labelEnabledCount = new AntdUI.Label();
            this.labelEnabledTitle = new AntdUI.Label();
            this.panelTotalCard = new AntdUI.Panel();
            this.labelTotalCount = new AntdUI.Label();
            this.labelTotalTitle = new AntdUI.Label();
            this.panelToolbar = new AntdUI.Panel();
            this.flowActionsLeft = new AntdUI.FlowPanel();
            this.inputSearch = new AntdUI.Input();
            this.flowActionsRight = new AntdUI.FlowPanel();
            this.buttonAddUser = new AntdUI.Button();
            this.buttonEditUser = new AntdUI.Button();
            this.buttonDeleteUser = new AntdUI.Button();
            this.buttonToggleEnabled = new AntdUI.Button();
            this.panelRoot.SuspendLayout();
            this.panelTableCard.SuspendLayout();
            this.flowStats.SuspendLayout();
            this.panelDisabledCard.SuspendLayout();
            this.panelEnabledCard.SuspendLayout();
            this.panelTotalCard.SuspendLayout();
            this.panelToolbar.SuspendLayout();
            this.flowActionsLeft.SuspendLayout();
            this.flowActionsRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.panelTableCard);
            this.panelRoot.Controls.Add(this.flowStats);
            this.panelRoot.Controls.Add(this.panelToolbar);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(8);
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(826, 664);
            this.panelRoot.TabIndex = 0;
            // 
            // panelTableCard
            // 
            this.panelTableCard.BackColor = System.Drawing.Color.Transparent;
            this.panelTableCard.Controls.Add(this.tableUsers);
            this.panelTableCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTableCard.Location = new System.Drawing.Point(8, 140);
            this.panelTableCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelTableCard.Name = "panelTableCard";
            this.panelTableCard.Padding = new System.Windows.Forms.Padding(8);
            this.panelTableCard.Radius = 12;
            this.panelTableCard.Shadow = 4;
            this.panelTableCard.Size = new System.Drawing.Size(810, 516);
            this.panelTableCard.TabIndex = 2;
            // 
            // tableUsers
            // 
            this.tableUsers.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tableUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableUsers.EmptyHeader = true;
            this.tableUsers.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.tableUsers.Gap = 8;
            this.tableUsers.Gaps = new System.Drawing.Size(8, 8);
            this.tableUsers.Location = new System.Drawing.Point(12, 12);
            this.tableUsers.Margin = new System.Windows.Forms.Padding(0);
            this.tableUsers.Name = "tableUsers";
            this.tableUsers.ShowTip = false;
            this.tableUsers.Size = new System.Drawing.Size(786, 492);
            this.tableUsers.TabIndex = 0;
            this.tableUsers.Text = "tableUsers";
            // 
            // flowStats
            // 
            this.flowStats.Controls.Add(this.panelDisabledCard);
            this.flowStats.Controls.Add(this.panelEnabledCard);
            this.flowStats.Controls.Add(this.panelTotalCard);
            this.flowStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowStats.Gap = 8;
            this.flowStats.Location = new System.Drawing.Point(8, 52);
            this.flowStats.Margin = new System.Windows.Forms.Padding(0);
            this.flowStats.Name = "flowStats";
            this.flowStats.Padding = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.flowStats.Size = new System.Drawing.Size(810, 88);
            this.flowStats.TabIndex = 1;
            // 
            // panelDisabledCard
            // 
            this.panelDisabledCard.BackColor = System.Drawing.Color.Transparent;
            this.panelDisabledCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(229)))), ((int)(((byte)(235)))));
            this.panelDisabledCard.Controls.Add(this.labelDisabledCount);
            this.panelDisabledCard.Controls.Add(this.labelDisabledTitle);
            this.panelDisabledCard.Location = new System.Drawing.Point(380, 6);
            this.panelDisabledCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelDisabledCard.Name = "panelDisabledCard";
            this.panelDisabledCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelDisabledCard.Radius = 12;
            this.panelDisabledCard.Shadow = 4;
            this.panelDisabledCard.ShadowOpacity = 0.2F;
            this.panelDisabledCard.ShadowOpacityAnimation = true;
            this.panelDisabledCard.Size = new System.Drawing.Size(180, 72);
            this.panelDisabledCard.TabIndex = 2;
            // 
            // labelDisabledCount
            // 
            this.labelDisabledCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelDisabledCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelDisabledCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.labelDisabledCount.Location = new System.Drawing.Point(100, 16);
            this.labelDisabledCount.Name = "labelDisabledCount";
            this.labelDisabledCount.Size = new System.Drawing.Size(64, 40);
            this.labelDisabledCount.TabIndex = 1;
            this.labelDisabledCount.Text = "0";
            // 
            // labelDisabledTitle
            // 
            this.labelDisabledTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelDisabledTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelDisabledTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.labelDisabledTitle.Location = new System.Drawing.Point(16, 16);
            this.labelDisabledTitle.Name = "labelDisabledTitle";
            this.labelDisabledTitle.Size = new System.Drawing.Size(78, 40);
            this.labelDisabledTitle.TabIndex = 0;
            this.labelDisabledTitle.Text = "禁用用户";
            // 
            // panelEnabledCard
            // 
            this.panelEnabledCard.BackColor = System.Drawing.Color.Transparent;
            this.panelEnabledCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(229)))), ((int)(((byte)(235)))));
            this.panelEnabledCard.Controls.Add(this.labelEnabledCount);
            this.panelEnabledCard.Controls.Add(this.labelEnabledTitle);
            this.panelEnabledCard.Location = new System.Drawing.Point(192, 6);
            this.panelEnabledCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelEnabledCard.Name = "panelEnabledCard";
            this.panelEnabledCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelEnabledCard.Radius = 12;
            this.panelEnabledCard.Shadow = 4;
            this.panelEnabledCard.ShadowOpacity = 0.2F;
            this.panelEnabledCard.ShadowOpacityAnimation = true;
            this.panelEnabledCard.Size = new System.Drawing.Size(180, 72);
            this.panelEnabledCard.TabIndex = 1;
            // 
            // labelEnabledCount
            // 
            this.labelEnabledCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelEnabledCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelEnabledCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(196)))), ((int)(((byte)(26)))));
            this.labelEnabledCount.Location = new System.Drawing.Point(85, 16);
            this.labelEnabledCount.Name = "labelEnabledCount";
            this.labelEnabledCount.Size = new System.Drawing.Size(79, 40);
            this.labelEnabledCount.TabIndex = 1;
            this.labelEnabledCount.Text = "0";
            // 
            // labelEnabledTitle
            // 
            this.labelEnabledTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelEnabledTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelEnabledTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(196)))), ((int)(((byte)(26)))));
            this.labelEnabledTitle.Location = new System.Drawing.Point(16, 16);
            this.labelEnabledTitle.Name = "labelEnabledTitle";
            this.labelEnabledTitle.Size = new System.Drawing.Size(63, 40);
            this.labelEnabledTitle.TabIndex = 0;
            this.labelEnabledTitle.Text = "启用用户";
            // 
            // panelTotalCard
            // 
            this.panelTotalCard.BackColor = System.Drawing.Color.Transparent;
            this.panelTotalCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(229)))), ((int)(((byte)(235)))));
            this.panelTotalCard.Controls.Add(this.labelTotalCount);
            this.panelTotalCard.Controls.Add(this.labelTotalTitle);
            this.panelTotalCard.Location = new System.Drawing.Point(4, 6);
            this.panelTotalCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelTotalCard.Name = "panelTotalCard";
            this.panelTotalCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelTotalCard.Radius = 12;
            this.panelTotalCard.Shadow = 4;
            this.panelTotalCard.ShadowOpacity = 0.2F;
            this.panelTotalCard.ShadowOpacityAnimation = true;
            this.panelTotalCard.Size = new System.Drawing.Size(180, 72);
            this.panelTotalCard.TabIndex = 0;
            // 
            // labelTotalCount
            // 
            this.labelTotalCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelTotalCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelTotalCount.Location = new System.Drawing.Point(104, 16);
            this.labelTotalCount.Name = "labelTotalCount";
            this.labelTotalCount.Size = new System.Drawing.Size(60, 40);
            this.labelTotalCount.TabIndex = 1;
            this.labelTotalCount.Text = "0";
            // 
            // labelTotalTitle
            // 
            this.labelTotalTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelTotalTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelTotalTitle.Location = new System.Drawing.Point(16, 16);
            this.labelTotalTitle.Name = "labelTotalTitle";
            this.labelTotalTitle.Size = new System.Drawing.Size(72, 40);
            this.labelTotalTitle.TabIndex = 0;
            this.labelTotalTitle.Text = "用户总数";
            // 
            // panelToolbar
            // 
            this.panelToolbar.Controls.Add(this.flowActionsLeft);
            this.panelToolbar.Controls.Add(this.flowActionsRight);
            this.panelToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelToolbar.Location = new System.Drawing.Point(8, 8);
            this.panelToolbar.Margin = new System.Windows.Forms.Padding(0);
            this.panelToolbar.Name = "panelToolbar";
            this.panelToolbar.Padding = new System.Windows.Forms.Padding(4);
            this.panelToolbar.Radius = 0;
            this.panelToolbar.Size = new System.Drawing.Size(810, 44);
            this.panelToolbar.TabIndex = 0;
            // 
            // flowActionsLeft
            // 
            this.flowActionsLeft.Controls.Add(this.inputSearch);
            this.flowActionsLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowActionsLeft.Gap = 8;
            this.flowActionsLeft.Location = new System.Drawing.Point(4, 4);
            this.flowActionsLeft.Name = "flowActionsLeft";
            this.flowActionsLeft.Size = new System.Drawing.Size(345, 36);
            this.flowActionsLeft.TabIndex = 2;
            this.flowActionsLeft.Text = "flowPanel1";
            // 
            // inputSearch
            // 
            this.inputSearch.Location = new System.Drawing.Point(4, 0);
            this.inputSearch.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.inputSearch.Name = "inputSearch";
            this.inputSearch.PlaceholderText = "搜索登录名 / 用户名 / 角色 / 备注";
            this.inputSearch.Size = new System.Drawing.Size(280, 36);
            this.inputSearch.TabIndex = 0;
            this.inputSearch.WaveSize = 0;
            // 
            // flowActionsRight
            // 
            this.flowActionsRight.Controls.Add(this.buttonAddUser);
            this.flowActionsRight.Controls.Add(this.buttonEditUser);
            this.flowActionsRight.Controls.Add(this.buttonDeleteUser);
            this.flowActionsRight.Controls.Add(this.buttonToggleEnabled);
            this.flowActionsRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowActionsRight.Gap = 8;
            this.flowActionsRight.Location = new System.Drawing.Point(366, 4);
            this.flowActionsRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowActionsRight.Name = "flowActionsRight";
            this.flowActionsRight.Size = new System.Drawing.Size(440, 36);
            this.flowActionsRight.TabIndex = 1;
            // 
            // buttonAddUser
            // 
            this.buttonAddUser.IconSvg = "PlusOutlined";
            this.buttonAddUser.Location = new System.Drawing.Point(340, 0);
            this.buttonAddUser.Margin = new System.Windows.Forms.Padding(0);
            this.buttonAddUser.Name = "buttonAddUser";
            this.buttonAddUser.Radius = 8;
            this.buttonAddUser.Size = new System.Drawing.Size(100, 36);
            this.buttonAddUser.TabIndex = 3;
            this.buttonAddUser.Text = "新增用户";
            this.buttonAddUser.Type = AntdUI.TTypeMini.Primary;
            this.buttonAddUser.WaveSize = 0;
            // 
            // buttonEditUser
            // 
            this.buttonEditUser.Enabled = false;
            this.buttonEditUser.IconSvg = "EditOutlined";
            this.buttonEditUser.Location = new System.Drawing.Point(232, 0);
            this.buttonEditUser.Margin = new System.Windows.Forms.Padding(0);
            this.buttonEditUser.Name = "buttonEditUser";
            this.buttonEditUser.Radius = 8;
            this.buttonEditUser.Size = new System.Drawing.Size(100, 36);
            this.buttonEditUser.TabIndex = 0;
            this.buttonEditUser.Text = "编辑用户";
            this.buttonEditUser.WaveSize = 0;
            // 
            // buttonDeleteUser
            // 
            this.buttonDeleteUser.Enabled = false;
            this.buttonDeleteUser.IconSvg = "DeleteOutlined";
            this.buttonDeleteUser.Location = new System.Drawing.Point(124, 0);
            this.buttonDeleteUser.Margin = new System.Windows.Forms.Padding(0);
            this.buttonDeleteUser.Name = "buttonDeleteUser";
            this.buttonDeleteUser.Radius = 8;
            this.buttonDeleteUser.Size = new System.Drawing.Size(100, 36);
            this.buttonDeleteUser.TabIndex = 2;
            this.buttonDeleteUser.Text = "删除用户";
            this.buttonDeleteUser.WaveSize = 0;
            // 
            // buttonToggleEnabled
            // 
            this.buttonToggleEnabled.Enabled = false;
            this.buttonToggleEnabled.IconSvg = "SwapOutlined";
            this.buttonToggleEnabled.Location = new System.Drawing.Point(0, 0);
            this.buttonToggleEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.buttonToggleEnabled.Name = "buttonToggleEnabled";
            this.buttonToggleEnabled.Radius = 8;
            this.buttonToggleEnabled.Size = new System.Drawing.Size(116, 36);
            this.buttonToggleEnabled.TabIndex = 1;
            this.buttonToggleEnabled.Text = "启用/禁用";
            this.buttonToggleEnabled.WaveSize = 0;
            // 
            // UserManagementPage
            // 
            this.Controls.Add(this.panelRoot);
            this.Name = "UserManagementPage";
            this.Size = new System.Drawing.Size(826, 664);
            this.panelRoot.ResumeLayout(false);
            this.panelTableCard.ResumeLayout(false);
            this.flowStats.ResumeLayout(false);
            this.panelDisabledCard.ResumeLayout(false);
            this.panelEnabledCard.ResumeLayout(false);
            this.panelTotalCard.ResumeLayout(false);
            this.panelToolbar.ResumeLayout(false);
            this.flowActionsLeft.ResumeLayout(false);
            this.flowActionsRight.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.Panel panelRoot;
        private AntdUI.Panel panelToolbar;
        private AntdUI.FlowPanel flowActionsRight;
        private AntdUI.Input inputSearch;
        private AntdUI.Button buttonEditUser;
        private AntdUI.Button buttonToggleEnabled;
        private AntdUI.Button buttonDeleteUser;
        private AntdUI.Button buttonAddUser;
        private AntdUI.FlowPanel flowStats;
        private AntdUI.Panel panelTotalCard;
        private AntdUI.Label labelTotalCount;
        private AntdUI.Label labelTotalTitle;
        private AntdUI.Panel panelEnabledCard;
        private AntdUI.Label labelEnabledCount;
        private AntdUI.Label labelEnabledTitle;
        private AntdUI.Panel panelDisabledCard;
        private AntdUI.Label labelDisabledCount;
        private AntdUI.Label labelDisabledTitle;
        private AntdUI.Panel panelTableCard;
        private AntdUI.Table tableUsers;
        private AntdUI.FlowPanel flowActionsLeft;
    }
}