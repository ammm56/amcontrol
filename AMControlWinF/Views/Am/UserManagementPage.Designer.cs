namespace AMControlWinF.Views.Am
{
    partial class UserManagementPage
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserManagementPage));
            this.panelRoot = new AntdUI.Panel();
            this.panelTableCard = new AntdUI.Panel();
            this.tableUsers = new AntdUI.Table();
            this.panelSummary = new AntdUI.Panel();
            this.summaryLayout = new System.Windows.Forms.TableLayoutPanel();
            this.panelTotalCard = new AntdUI.Panel();
            this.labelTotalValue = new AntdUI.Label();
            this.labelTotalTitle = new AntdUI.Label();
            this.panelEnabledCard = new AntdUI.Panel();
            this.labelEnabledValue = new AntdUI.Label();
            this.labelEnabledTitle = new AntdUI.Label();
            this.panelDisabledCard = new AntdUI.Panel();
            this.labelDisabledValue = new AntdUI.Label();
            this.labelDisabledTitle = new AntdUI.Label();
            this.panelActionCard = new AntdUI.Panel();
            this.buttonRefresh = new AntdUI.Button();
            this.buttonPermission = new AntdUI.Button();
            this.buttonResetPassword = new AntdUI.Button();
            this.buttonToggleEnabled = new AntdUI.Button();
            this.buttonEdit = new AntdUI.Button();
            this.buttonAdd = new AntdUI.Button();
            this.panelSearchHost = new AntdUI.Panel();
            this.inputKeyword = new AntdUI.Input();
            this.buttonSearch = new AntdUI.Button();
            this.panelRoot.SuspendLayout();
            this.panelTableCard.SuspendLayout();
            this.panelSummary.SuspendLayout();
            this.summaryLayout.SuspendLayout();
            this.panelTotalCard.SuspendLayout();
            this.panelEnabledCard.SuspendLayout();
            this.panelDisabledCard.SuspendLayout();
            this.panelActionCard.SuspendLayout();
            this.panelSearchHost.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Back = System.Drawing.Color.Transparent;
            this.panelRoot.Controls.Add(this.panelTableCard);
            this.panelRoot.Controls.Add(this.panelSummary);
            this.panelRoot.Controls.Add(this.panelActionCard);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(832, 616);
            this.panelRoot.TabIndex = 0;
            // 
            // panelTableCard
            // 
            this.panelTableCard.Controls.Add(this.tableUsers);
            this.panelTableCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTableCard.Location = new System.Drawing.Point(0, 136);
            this.panelTableCard.Name = "panelTableCard";
            this.panelTableCard.Padding = new System.Windows.Forms.Padding(10);
            this.panelTableCard.Radius = 14;
            this.panelTableCard.Shadow = 4;
            this.panelTableCard.Size = new System.Drawing.Size(832, 480);
            this.panelTableCard.TabIndex = 2;
            // 
            // tableUsers
            // 
            this.tableUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableUsers.Gap = 12;
            this.tableUsers.Location = new System.Drawing.Point(14, 14);
            this.tableUsers.Name = "tableUsers";
            this.tableUsers.Size = new System.Drawing.Size(804, 452);
            this.tableUsers.TabIndex = 0;
            // 
            // panelSummary
            // 
            this.panelSummary.Back = System.Drawing.Color.Transparent;
            this.panelSummary.Controls.Add(this.summaryLayout);
            this.panelSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSummary.Location = new System.Drawing.Point(0, 62);
            this.panelSummary.Name = "panelSummary";
            this.panelSummary.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.panelSummary.Radius = 0;
            this.panelSummary.Size = new System.Drawing.Size(832, 74);
            this.panelSummary.TabIndex = 1;
            // 
            // summaryLayout
            // 
            this.summaryLayout.BackColor = System.Drawing.Color.Transparent;
            this.summaryLayout.ColumnCount = 3;
            this.summaryLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.summaryLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.summaryLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.summaryLayout.Controls.Add(this.panelTotalCard, 0, 0);
            this.summaryLayout.Controls.Add(this.panelEnabledCard, 1, 0);
            this.summaryLayout.Controls.Add(this.panelDisabledCard, 2, 0);
            this.summaryLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.summaryLayout.Location = new System.Drawing.Point(0, 8);
            this.summaryLayout.Name = "summaryLayout";
            this.summaryLayout.RowCount = 1;
            this.summaryLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.summaryLayout.Size = new System.Drawing.Size(832, 58);
            this.summaryLayout.TabIndex = 0;
            // 
            // panelTotalCard
            // 
            this.panelTotalCard.Controls.Add(this.labelTotalValue);
            this.panelTotalCard.Controls.Add(this.labelTotalTitle);
            this.panelTotalCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTotalCard.Location = new System.Drawing.Point(0, 0);
            this.panelTotalCard.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.panelTotalCard.Name = "panelTotalCard";
            this.panelTotalCard.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.panelTotalCard.Radius = 12;
            this.panelTotalCard.Shadow = 3;
            this.panelTotalCard.Size = new System.Drawing.Size(271, 58);
            this.panelTotalCard.TabIndex = 0;
            // 
            // labelTotalValue
            // 
            this.labelTotalValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTotalValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 16F, System.Drawing.FontStyle.Bold);
            this.labelTotalValue.Location = new System.Drawing.Point(15, 25);
            this.labelTotalValue.Name = "labelTotalValue";
            this.labelTotalValue.Size = new System.Drawing.Size(241, 22);
            this.labelTotalValue.TabIndex = 1;
            this.labelTotalValue.Text = "0";
            // 
            // labelTotalTitle
            // 
            this.labelTotalTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTotalTitle.Location = new System.Drawing.Point(15, 11);
            this.labelTotalTitle.Name = "labelTotalTitle";
            this.labelTotalTitle.Size = new System.Drawing.Size(241, 14);
            this.labelTotalTitle.TabIndex = 0;
            this.labelTotalTitle.Text = "用户总数";
            // 
            // panelEnabledCard
            // 
            this.panelEnabledCard.Controls.Add(this.labelEnabledValue);
            this.panelEnabledCard.Controls.Add(this.labelEnabledTitle);
            this.panelEnabledCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEnabledCard.Location = new System.Drawing.Point(277, 0);
            this.panelEnabledCard.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.panelEnabledCard.Name = "panelEnabledCard";
            this.panelEnabledCard.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.panelEnabledCard.Radius = 12;
            this.panelEnabledCard.Shadow = 3;
            this.panelEnabledCard.Size = new System.Drawing.Size(271, 58);
            this.panelEnabledCard.TabIndex = 1;
            // 
            // labelEnabledValue
            // 
            this.labelEnabledValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEnabledValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 16F, System.Drawing.FontStyle.Bold);
            this.labelEnabledValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(160)))), ((int)(((byte)(67)))));
            this.labelEnabledValue.Location = new System.Drawing.Point(15, 25);
            this.labelEnabledValue.Name = "labelEnabledValue";
            this.labelEnabledValue.Size = new System.Drawing.Size(241, 22);
            this.labelEnabledValue.TabIndex = 1;
            this.labelEnabledValue.Text = "0";
            // 
            // labelEnabledTitle
            // 
            this.labelEnabledTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelEnabledTitle.Location = new System.Drawing.Point(15, 11);
            this.labelEnabledTitle.Name = "labelEnabledTitle";
            this.labelEnabledTitle.Size = new System.Drawing.Size(241, 14);
            this.labelEnabledTitle.TabIndex = 0;
            this.labelEnabledTitle.Text = "启用用户";
            // 
            // panelDisabledCard
            // 
            this.panelDisabledCard.Controls.Add(this.labelDisabledValue);
            this.panelDisabledCard.Controls.Add(this.labelDisabledTitle);
            this.panelDisabledCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDisabledCard.Location = new System.Drawing.Point(554, 0);
            this.panelDisabledCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelDisabledCard.Name = "panelDisabledCard";
            this.panelDisabledCard.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.panelDisabledCard.Radius = 12;
            this.panelDisabledCard.Shadow = 3;
            this.panelDisabledCard.Size = new System.Drawing.Size(278, 58);
            this.panelDisabledCard.TabIndex = 2;
            // 
            // labelDisabledValue
            // 
            this.labelDisabledValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDisabledValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 16F, System.Drawing.FontStyle.Bold);
            this.labelDisabledValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.labelDisabledValue.Location = new System.Drawing.Point(15, 25);
            this.labelDisabledValue.Name = "labelDisabledValue";
            this.labelDisabledValue.Size = new System.Drawing.Size(248, 22);
            this.labelDisabledValue.TabIndex = 1;
            this.labelDisabledValue.Text = "0";
            // 
            // labelDisabledTitle
            // 
            this.labelDisabledTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDisabledTitle.Location = new System.Drawing.Point(15, 11);
            this.labelDisabledTitle.Name = "labelDisabledTitle";
            this.labelDisabledTitle.Size = new System.Drawing.Size(248, 14);
            this.labelDisabledTitle.TabIndex = 0;
            this.labelDisabledTitle.Text = "禁用用户";
            // 
            // panelActionCard
            // 
            this.panelActionCard.Controls.Add(this.buttonRefresh);
            this.panelActionCard.Controls.Add(this.buttonPermission);
            this.panelActionCard.Controls.Add(this.buttonResetPassword);
            this.panelActionCard.Controls.Add(this.buttonToggleEnabled);
            this.panelActionCard.Controls.Add(this.buttonEdit);
            this.panelActionCard.Controls.Add(this.buttonAdd);
            this.panelActionCard.Controls.Add(this.panelSearchHost);
            this.panelActionCard.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelActionCard.Location = new System.Drawing.Point(0, 0);
            this.panelActionCard.Name = "panelActionCard";
            this.panelActionCard.Padding = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.panelActionCard.Radius = 14;
            this.panelActionCard.Shadow = 4;
            this.panelActionCard.Size = new System.Drawing.Size(832, 62);
            this.panelActionCard.TabIndex = 0;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonRefresh.Location = new System.Drawing.Point(244, 14);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(78, 34);
            this.buttonRefresh.TabIndex = 6;
            this.buttonRefresh.Text = "刷新";
            this.buttonRefresh.WaveSize = 0;
            // 
            // buttonPermission
            // 
            this.buttonPermission.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonPermission.Location = new System.Drawing.Point(322, 14);
            this.buttonPermission.Name = "buttonPermission";
            this.buttonPermission.Size = new System.Drawing.Size(102, 34);
            this.buttonPermission.TabIndex = 5;
            this.buttonPermission.Text = "页面权限";
            this.buttonPermission.WaveSize = 0;
            // 
            // buttonResetPassword
            // 
            this.buttonResetPassword.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonResetPassword.Location = new System.Drawing.Point(424, 14);
            this.buttonResetPassword.Name = "buttonResetPassword";
            this.buttonResetPassword.Size = new System.Drawing.Size(102, 34);
            this.buttonResetPassword.TabIndex = 4;
            this.buttonResetPassword.Text = "重置密码";
            this.buttonResetPassword.WaveSize = 0;
            // 
            // buttonToggleEnabled
            // 
            this.buttonToggleEnabled.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonToggleEnabled.Location = new System.Drawing.Point(526, 14);
            this.buttonToggleEnabled.Name = "buttonToggleEnabled";
            this.buttonToggleEnabled.Size = new System.Drawing.Size(102, 34);
            this.buttonToggleEnabled.TabIndex = 3;
            this.buttonToggleEnabled.Text = "启用用户";
            this.buttonToggleEnabled.WaveSize = 0;
            // 
            // buttonEdit
            // 
            this.buttonEdit.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonEdit.Location = new System.Drawing.Point(628, 14);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(94, 34);
            this.buttonEdit.TabIndex = 2;
            this.buttonEdit.Text = "编辑用户";
            this.buttonEdit.WaveSize = 0;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonAdd.Location = new System.Drawing.Point(722, 14);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(94, 34);
            this.buttonAdd.TabIndex = 1;
            this.buttonAdd.Text = "新增用户";
            this.buttonAdd.Type = AntdUI.TTypeMini.Primary;
            this.buttonAdd.WaveSize = 0;
            // 
            // panelSearchHost
            // 
            this.panelSearchHost.Back = System.Drawing.Color.Transparent;
            this.panelSearchHost.Controls.Add(this.inputKeyword);
            this.panelSearchHost.Controls.Add(this.buttonSearch);
            this.panelSearchHost.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSearchHost.Location = new System.Drawing.Point(16, 14);
            this.panelSearchHost.Name = "panelSearchHost";
            this.panelSearchHost.Radius = 0;
            this.panelSearchHost.Size = new System.Drawing.Size(224, 34);
            this.panelSearchHost.TabIndex = 0;
            // 
            // inputKeyword
            // 
            this.inputKeyword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inputKeyword.Location = new System.Drawing.Point(0, 0);
            this.inputKeyword.Name = "inputKeyword";
            this.inputKeyword.Size = new System.Drawing.Size(152, 34);
            this.inputKeyword.TabIndex = 2;
            this.inputKeyword.WaveSize = 0;
            // 
            // buttonSearch
            // 
            this.buttonSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonSearch.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonSearch.IconSvg = resources.GetString("buttonSearch.IconSvg");
            this.buttonSearch.Location = new System.Drawing.Point(152, 0);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(72, 34);
            this.buttonSearch.TabIndex = 0;
            this.buttonSearch.Type = AntdUI.TTypeMini.Primary;
            this.buttonSearch.WaveSize = 0;
            // 
            // UserManagementPage
            // 
            this.Controls.Add(this.panelRoot);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.Name = "UserManagementPage";
            this.Size = new System.Drawing.Size(832, 616);
            this.panelRoot.ResumeLayout(false);
            this.panelTableCard.ResumeLayout(false);
            this.panelSummary.ResumeLayout(false);
            this.summaryLayout.ResumeLayout(false);
            this.panelTotalCard.ResumeLayout(false);
            this.panelEnabledCard.ResumeLayout(false);
            this.panelDisabledCard.ResumeLayout(false);
            this.panelActionCard.ResumeLayout(false);
            this.panelSearchHost.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AntdUI.Panel panelRoot;
        private AntdUI.Panel panelActionCard;
        private AntdUI.Panel panelSearchHost;
        private AntdUI.Input inputKeyword;
        private AntdUI.Button buttonSearch;
        private AntdUI.Button buttonAdd;
        private AntdUI.Button buttonEdit;
        private AntdUI.Button buttonToggleEnabled;
        private AntdUI.Button buttonResetPassword;
        private AntdUI.Button buttonPermission;
        private AntdUI.Button buttonRefresh;
        private AntdUI.Panel panelSummary;
        private System.Windows.Forms.TableLayoutPanel summaryLayout;
        private AntdUI.Panel panelTotalCard;
        private AntdUI.Label labelTotalValue;
        private AntdUI.Label labelTotalTitle;
        private AntdUI.Panel panelEnabledCard;
        private AntdUI.Label labelEnabledValue;
        private AntdUI.Label labelEnabledTitle;
        private AntdUI.Panel panelDisabledCard;
        private AntdUI.Label labelDisabledValue;
        private AntdUI.Label labelDisabledTitle;
        private AntdUI.Panel panelTableCard;
        private AntdUI.Table tableUsers;
    }
}