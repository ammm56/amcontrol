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
            this.panelRoot = new AntdUI.Panel();
            this.panelTableCard = new AntdUI.Panel();
            this.tableUsers = new AntdUI.Table();
            this.panelActionCard = new AntdUI.Panel();
            this.buttonRefresh = new AntdUI.Button();
            this.buttonResetPassword = new AntdUI.Button();
            this.buttonToggleEnabled = new AntdUI.Button();
            this.buttonEdit = new AntdUI.Button();
            this.buttonAdd = new AntdUI.Button();
            this.panelSearchCard = new AntdUI.Panel();
            this.inputKeyword = new AntdUI.Input();
            this.labelKeyword = new AntdUI.Label();
            this.buttonClear = new AntdUI.Button();
            this.buttonSearch = new AntdUI.Button();
            this.panelSummary = new AntdUI.Panel();
            this.labelDisabledTitle = new AntdUI.Label();
            this.labelDisabledValue = new AntdUI.Label();
            this.panelDisabledCard = new AntdUI.Panel();
            this.labelEnabledTitle = new AntdUI.Label();
            this.labelEnabledValue = new AntdUI.Label();
            this.panelEnabledCard = new AntdUI.Panel();
            this.labelTotalTitle = new AntdUI.Label();
            this.labelTotalValue = new AntdUI.Label();
            this.panelTotalCard = new AntdUI.Panel();
            this.buttonPermission = new AntdUI.Button();
            this.panelRoot.SuspendLayout();
            this.panelTableCard.SuspendLayout();
            this.panelActionCard.SuspendLayout();
            this.panelSearchCard.SuspendLayout();
            this.panelSummary.SuspendLayout();
            this.panelDisabledCard.SuspendLayout();
            this.panelEnabledCard.SuspendLayout();
            this.panelTotalCard.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Back = System.Drawing.Color.Transparent;
            this.panelRoot.Controls.Add(this.panelTableCard);
            this.panelRoot.Controls.Add(this.panelSummary);
            this.panelRoot.Controls.Add(this.panelActionCard);
            this.panelRoot.Controls.Add(this.panelSearchCard);
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
            this.panelTableCard.Location = new System.Drawing.Point(0, 182);
            this.panelTableCard.Name = "panelTableCard";
            this.panelTableCard.Padding = new System.Windows.Forms.Padding(10);
            this.panelTableCard.Radius = 14;
            this.panelTableCard.Shadow = 4;
            this.panelTableCard.Size = new System.Drawing.Size(832, 434);
            this.panelTableCard.TabIndex = 3;
            // 
            // tableUsers
            // 
            this.tableUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableUsers.Gap = 12;
            this.tableUsers.Location = new System.Drawing.Point(14, 14);
            this.tableUsers.Name = "tableUsers";
            this.tableUsers.Size = new System.Drawing.Size(804, 406);
            this.tableUsers.TabIndex = 0;
            // 
            // panelActionCard
            // 
            this.panelActionCard.Controls.Add(this.buttonRefresh);
            this.panelActionCard.Controls.Add(this.buttonPermission);
            this.panelActionCard.Controls.Add(this.buttonResetPassword);
            this.panelActionCard.Controls.Add(this.buttonToggleEnabled);
            this.panelActionCard.Controls.Add(this.buttonEdit);
            this.panelActionCard.Controls.Add(this.buttonAdd);
            this.panelActionCard.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelActionCard.Location = new System.Drawing.Point(0, 58);
            this.panelActionCard.Name = "panelActionCard";
            this.panelActionCard.Padding = new System.Windows.Forms.Padding(4);
            this.panelActionCard.Radius = 14;
            this.panelActionCard.Shadow = 4;
            this.panelActionCard.Size = new System.Drawing.Size(832, 52);
            this.panelActionCard.TabIndex = 1;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonRefresh.Location = new System.Drawing.Point(238, 8);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(90, 36);
            this.buttonRefresh.TabIndex = 5;
            this.buttonRefresh.Text = "刷新";
            this.buttonRefresh.WaveSize = 0;
            // 
            // buttonResetPassword
            // 
            this.buttonResetPassword.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonResetPassword.Location = new System.Drawing.Point(430, 8);
            this.buttonResetPassword.Name = "buttonResetPassword";
            this.buttonResetPassword.Size = new System.Drawing.Size(102, 36);
            this.buttonResetPassword.TabIndex = 3;
            this.buttonResetPassword.Text = "重置密码";
            this.buttonResetPassword.WaveSize = 0;
            // 
            // buttonToggleEnabled
            // 
            this.buttonToggleEnabled.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonToggleEnabled.Location = new System.Drawing.Point(532, 8);
            this.buttonToggleEnabled.Name = "buttonToggleEnabled";
            this.buttonToggleEnabled.Size = new System.Drawing.Size(108, 36);
            this.buttonToggleEnabled.TabIndex = 2;
            this.buttonToggleEnabled.Text = "启用用户";
            this.buttonToggleEnabled.WaveSize = 0;
            // 
            // buttonEdit
            // 
            this.buttonEdit.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonEdit.Location = new System.Drawing.Point(640, 8);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(102, 36);
            this.buttonEdit.TabIndex = 1;
            this.buttonEdit.Text = "编辑用户";
            this.buttonEdit.WaveSize = 0;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonAdd.Location = new System.Drawing.Point(742, 8);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(82, 36);
            this.buttonAdd.TabIndex = 0;
            this.buttonAdd.Text = "新增用户";
            this.buttonAdd.Type = AntdUI.TTypeMini.Primary;
            this.buttonAdd.WaveSize = 0;
            // 
            // panelSearchCard
            // 
            this.panelSearchCard.Controls.Add(this.inputKeyword);
            this.panelSearchCard.Controls.Add(this.labelKeyword);
            this.panelSearchCard.Controls.Add(this.buttonClear);
            this.panelSearchCard.Controls.Add(this.buttonSearch);
            this.panelSearchCard.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearchCard.Location = new System.Drawing.Point(0, 0);
            this.panelSearchCard.Name = "panelSearchCard";
            this.panelSearchCard.Padding = new System.Windows.Forms.Padding(4);
            this.panelSearchCard.Radius = 14;
            this.panelSearchCard.Shadow = 4;
            this.panelSearchCard.Size = new System.Drawing.Size(832, 58);
            this.panelSearchCard.TabIndex = 0;
            // 
            // inputKeyword
            // 
            this.inputKeyword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inputKeyword.Location = new System.Drawing.Point(52, 8);
            this.inputKeyword.Name = "inputKeyword";
            this.inputKeyword.Size = new System.Drawing.Size(592, 42);
            this.inputKeyword.TabIndex = 3;
            // 
            // labelKeyword
            // 
            this.labelKeyword.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelKeyword.Location = new System.Drawing.Point(8, 8);
            this.labelKeyword.Name = "labelKeyword";
            this.labelKeyword.Size = new System.Drawing.Size(44, 42);
            this.labelKeyword.TabIndex = 2;
            this.labelKeyword.Text = "搜索";
            // 
            // buttonClear
            // 
            this.buttonClear.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonClear.Location = new System.Drawing.Point(644, 8);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(90, 42);
            this.buttonClear.TabIndex = 1;
            this.buttonClear.Text = "清空";
            this.buttonClear.WaveSize = 0;
            // 
            // buttonSearch
            // 
            this.buttonSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonSearch.Location = new System.Drawing.Point(734, 8);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(90, 42);
            this.buttonSearch.TabIndex = 0;
            this.buttonSearch.Text = "查询";
            this.buttonSearch.Type = AntdUI.TTypeMini.Primary;
            this.buttonSearch.WaveSize = 0;
            // 
            // panelSummary
            // 
            this.panelSummary.Back = System.Drawing.Color.Transparent;
            this.panelSummary.Controls.Add(this.panelDisabledCard);
            this.panelSummary.Controls.Add(this.panelEnabledCard);
            this.panelSummary.Controls.Add(this.panelTotalCard);
            this.panelSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSummary.Location = new System.Drawing.Point(0, 110);
            this.panelSummary.Name = "panelSummary";
            this.panelSummary.Padding = new System.Windows.Forms.Padding(4);
            this.panelSummary.Radius = 0;
            this.panelSummary.Size = new System.Drawing.Size(832, 72);
            this.panelSummary.TabIndex = 2;
            // 
            // labelDisabledTitle
            // 
            this.labelDisabledTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDisabledTitle.Location = new System.Drawing.Point(7, 7);
            this.labelDisabledTitle.Name = "labelDisabledTitle";
            this.labelDisabledTitle.Size = new System.Drawing.Size(126, 14);
            this.labelDisabledTitle.TabIndex = 0;
            this.labelDisabledTitle.Text = "已禁用";
            // 
            // labelDisabledValue
            // 
            this.labelDisabledValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDisabledValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 16F, System.Drawing.FontStyle.Bold);
            this.labelDisabledValue.Location = new System.Drawing.Point(7, 21);
            this.labelDisabledValue.Name = "labelDisabledValue";
            this.labelDisabledValue.Size = new System.Drawing.Size(126, 36);
            this.labelDisabledValue.TabIndex = 1;
            this.labelDisabledValue.Text = "0";
            // 
            // panelDisabledCard
            // 
            this.panelDisabledCard.Controls.Add(this.labelDisabledValue);
            this.panelDisabledCard.Controls.Add(this.labelDisabledTitle);
            this.panelDisabledCard.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelDisabledCard.Location = new System.Drawing.Point(284, 4);
            this.panelDisabledCard.Name = "panelDisabledCard";
            this.panelDisabledCard.Padding = new System.Windows.Forms.Padding(4);
            this.panelDisabledCard.Radius = 12;
            this.panelDisabledCard.Shadow = 3;
            this.panelDisabledCard.Size = new System.Drawing.Size(140, 64);
            this.panelDisabledCard.TabIndex = 2;
            // 
            // labelEnabledTitle
            // 
            this.labelEnabledTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelEnabledTitle.Location = new System.Drawing.Point(7, 7);
            this.labelEnabledTitle.Name = "labelEnabledTitle";
            this.labelEnabledTitle.Size = new System.Drawing.Size(126, 14);
            this.labelEnabledTitle.TabIndex = 0;
            this.labelEnabledTitle.Text = "已启用";
            // 
            // labelEnabledValue
            // 
            this.labelEnabledValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEnabledValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 16F, System.Drawing.FontStyle.Bold);
            this.labelEnabledValue.Location = new System.Drawing.Point(7, 21);
            this.labelEnabledValue.Name = "labelEnabledValue";
            this.labelEnabledValue.Size = new System.Drawing.Size(126, 36);
            this.labelEnabledValue.TabIndex = 1;
            this.labelEnabledValue.Text = "0";
            // 
            // panelEnabledCard
            // 
            this.panelEnabledCard.Controls.Add(this.labelEnabledValue);
            this.panelEnabledCard.Controls.Add(this.labelEnabledTitle);
            this.panelEnabledCard.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelEnabledCard.Location = new System.Drawing.Point(144, 4);
            this.panelEnabledCard.Name = "panelEnabledCard";
            this.panelEnabledCard.Padding = new System.Windows.Forms.Padding(4);
            this.panelEnabledCard.Radius = 12;
            this.panelEnabledCard.Shadow = 3;
            this.panelEnabledCard.Size = new System.Drawing.Size(140, 64);
            this.panelEnabledCard.TabIndex = 1;
            // 
            // labelTotalTitle
            // 
            this.labelTotalTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTotalTitle.Location = new System.Drawing.Point(7, 7);
            this.labelTotalTitle.Name = "labelTotalTitle";
            this.labelTotalTitle.Size = new System.Drawing.Size(126, 14);
            this.labelTotalTitle.TabIndex = 0;
            this.labelTotalTitle.Text = "用户总数";
            // 
            // labelTotalValue
            // 
            this.labelTotalValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTotalValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 16F, System.Drawing.FontStyle.Bold);
            this.labelTotalValue.Location = new System.Drawing.Point(7, 21);
            this.labelTotalValue.Name = "labelTotalValue";
            this.labelTotalValue.Size = new System.Drawing.Size(126, 36);
            this.labelTotalValue.TabIndex = 1;
            this.labelTotalValue.Text = "0";
            // 
            // panelTotalCard
            // 
            this.panelTotalCard.Controls.Add(this.labelTotalValue);
            this.panelTotalCard.Controls.Add(this.labelTotalTitle);
            this.panelTotalCard.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelTotalCard.Location = new System.Drawing.Point(4, 4);
            this.panelTotalCard.Name = "panelTotalCard";
            this.panelTotalCard.Padding = new System.Windows.Forms.Padding(4);
            this.panelTotalCard.Radius = 12;
            this.panelTotalCard.Shadow = 3;
            this.panelTotalCard.Size = new System.Drawing.Size(140, 64);
            this.panelTotalCard.TabIndex = 0;
            // 
            // buttonPermission
            // 
            this.buttonPermission.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonPermission.Location = new System.Drawing.Point(328, 8);
            this.buttonPermission.Name = "buttonPermission";
            this.buttonPermission.Size = new System.Drawing.Size(102, 36);
            this.buttonPermission.TabIndex = 4;
            this.buttonPermission.Text = "页面权限";
            this.buttonPermission.WaveSize = 0;
            // 
            // UserManagementPage
            // 
            this.Controls.Add(this.panelRoot);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.Name = "UserManagementPage";
            this.Size = new System.Drawing.Size(832, 616);
            this.panelRoot.ResumeLayout(false);
            this.panelTableCard.ResumeLayout(false);
            this.panelActionCard.ResumeLayout(false);
            this.panelSearchCard.ResumeLayout(false);
            this.panelSummary.ResumeLayout(false);
            this.panelDisabledCard.ResumeLayout(false);
            this.panelEnabledCard.ResumeLayout(false);
            this.panelTotalCard.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AntdUI.Panel panelRoot;
        private AntdUI.Panel panelSearchCard;
        private AntdUI.Button buttonSearch;
        private AntdUI.Button buttonClear;
        private AntdUI.Input inputKeyword;
        private AntdUI.Label labelKeyword;
        private AntdUI.Panel panelActionCard;
        private AntdUI.Button buttonAdd;
        private AntdUI.Button buttonEdit;
        private AntdUI.Button buttonToggleEnabled;
        private AntdUI.Button buttonResetPassword;
        private AntdUI.Button buttonRefresh;
        private AntdUI.Panel panelTableCard;
        private AntdUI.Table tableUsers;
        private AntdUI.Panel panelSummary;
        private AntdUI.Panel panelDisabledCard;
        private AntdUI.Label labelDisabledValue;
        private AntdUI.Label labelDisabledTitle;
        private AntdUI.Panel panelEnabledCard;
        private AntdUI.Label labelEnabledValue;
        private AntdUI.Label labelEnabledTitle;
        private AntdUI.Panel panelTotalCard;
        private AntdUI.Label labelTotalValue;
        private AntdUI.Label labelTotalTitle;
        private AntdUI.Button buttonPermission;
    }
}