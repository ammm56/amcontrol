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
            this.panelBody = new AntdUI.Panel();
            this.panelTableCard = new AntdUI.Panel();
            this.tableUsers = new AntdUI.Table();
            this.panelDetailCard = new AntdUI.Panel();
            this.panelDetailActions = new AntdUI.Panel();
            this.buttonPermission = new AntdUI.Button();
            this.buttonResetPassword = new AntdUI.Button();
            this.buttonToggleEnabled = new AntdUI.Button();
            this.buttonEdit = new AntdUI.Button();
            this.panelDetailInfo = new AntdUI.Panel();
            this.labelDetailRemarkValue = new AntdUI.Label();
            this.labelDetailRemarkTitle = new AntdUI.Label();
            this.labelDetailLastLoginValue = new AntdUI.Label();
            this.labelDetailLastLoginTitle = new AntdUI.Label();
            this.labelDetailStateValue = new AntdUI.Label();
            this.labelDetailStateTitle = new AntdUI.Label();
            this.labelDetailRoleValue = new AntdUI.Label();
            this.labelDetailRoleTitle = new AntdUI.Label();
            this.labelDetailLoginNameValue = new AntdUI.Label();
            this.labelDetailLoginNameTitle = new AntdUI.Label();
            this.labelDetailUserNameValue = new AntdUI.Label();
            this.labelDetailUserNameTitle = new AntdUI.Label();
            this.labelDetailTitle = new AntdUI.Label();
            this.panelSummary = new AntdUI.Panel();
            this.panelDisabledCard = new AntdUI.Panel();
            this.labelDisabledValue = new AntdUI.Label();
            this.labelDisabledTitle = new AntdUI.Label();
            this.panelEnabledCard = new AntdUI.Panel();
            this.labelEnabledValue = new AntdUI.Label();
            this.labelEnabledTitle = new AntdUI.Label();
            this.panelTotalCard = new AntdUI.Panel();
            this.labelTotalValue = new AntdUI.Label();
            this.labelTotalTitle = new AntdUI.Label();
            this.panelToolbar = new AntdUI.Panel();
            this.inputKeyword = new AntdUI.Input();
            this.labelKeyword = new AntdUI.Label();
            this.buttonClear = new AntdUI.Button();
            this.buttonSearch = new AntdUI.Button();
            this.buttonRefresh = new AntdUI.Button();
            this.buttonAdd = new AntdUI.Button();
            this.panelRoot.SuspendLayout();
            this.panelBody.SuspendLayout();
            this.panelTableCard.SuspendLayout();
            this.panelDetailCard.SuspendLayout();
            this.panelDetailActions.SuspendLayout();
            this.panelDetailInfo.SuspendLayout();
            this.panelSummary.SuspendLayout();
            this.panelDisabledCard.SuspendLayout();
            this.panelEnabledCard.SuspendLayout();
            this.panelTotalCard.SuspendLayout();
            this.panelToolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Back = System.Drawing.Color.Transparent;
            this.panelRoot.Controls.Add(this.panelBody);
            this.panelRoot.Controls.Add(this.panelSummary);
            this.panelRoot.Controls.Add(this.panelToolbar);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(832, 616);
            this.panelRoot.TabIndex = 0;
            // 
            // panelBody
            // 
            this.panelBody.Back = System.Drawing.Color.Transparent;
            this.panelBody.Controls.Add(this.panelTableCard);
            this.panelBody.Controls.Add(this.panelDetailCard);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(0, 140);
            this.panelBody.Name = "panelBody";
            this.panelBody.Radius = 0;
            this.panelBody.Size = new System.Drawing.Size(832, 476);
            this.panelBody.TabIndex = 2;
            // 
            // panelTableCard
            // 
            this.panelTableCard.Back = System.Drawing.Color.White;
            this.panelTableCard.Controls.Add(this.tableUsers);
            this.panelTableCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTableCard.Location = new System.Drawing.Point(0, 0);
            this.panelTableCard.Name = "panelTableCard";
            this.panelTableCard.Padding = new System.Windows.Forms.Padding(10);
            this.panelTableCard.Radius = 14;
            this.panelTableCard.Shadow = 4;
            this.panelTableCard.Size = new System.Drawing.Size(548, 476);
            this.panelTableCard.TabIndex = 0;
            // 
            // tableUsers
            // 
            this.tableUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableUsers.Location = new System.Drawing.Point(16, 16);
            this.tableUsers.Name = "tableUsers";
            this.tableUsers.Size = new System.Drawing.Size(516, 444);
            this.tableUsers.TabIndex = 0;
            // 
            // panelDetailCard
            // 
            this.panelDetailCard.Back = System.Drawing.Color.White;
            this.panelDetailCard.Controls.Add(this.panelDetailInfo);
            this.panelDetailCard.Controls.Add(this.panelDetailActions);
            this.panelDetailCard.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelDetailCard.Location = new System.Drawing.Point(548, 0);
            this.panelDetailCard.Name = "panelDetailCard";
            this.panelDetailCard.Padding = new System.Windows.Forms.Padding(10);
            this.panelDetailCard.Radius = 14;
            this.panelDetailCard.Shadow = 4;
            this.panelDetailCard.Size = new System.Drawing.Size(284, 476);
            this.panelDetailCard.TabIndex = 1;
            // 
            // panelDetailActions
            // 
            this.panelDetailActions.Back = System.Drawing.Color.Transparent;
            this.panelDetailActions.Controls.Add(this.buttonPermission);
            this.panelDetailActions.Controls.Add(this.buttonResetPassword);
            this.panelDetailActions.Controls.Add(this.buttonToggleEnabled);
            this.panelDetailActions.Controls.Add(this.buttonEdit);
            this.panelDetailActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelDetailActions.Location = new System.Drawing.Point(16, 324);
            this.panelDetailActions.Name = "panelDetailActions";
            this.panelDetailActions.Radius = 0;
            this.panelDetailActions.Size = new System.Drawing.Size(252, 136);
            this.panelDetailActions.TabIndex = 1;
            // 
            // buttonPermission
            // 
            this.buttonPermission.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonPermission.Location = new System.Drawing.Point(0, 102);
            this.buttonPermission.Name = "buttonPermission";
            this.buttonPermission.Size = new System.Drawing.Size(252, 34);
            this.buttonPermission.TabIndex = 3;
            this.buttonPermission.Text = "页面权限";
            this.buttonPermission.WaveSize = 0;
            // 
            // buttonResetPassword
            // 
            this.buttonResetPassword.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonResetPassword.Location = new System.Drawing.Point(0, 68);
            this.buttonResetPassword.Name = "buttonResetPassword";
            this.buttonResetPassword.Size = new System.Drawing.Size(252, 34);
            this.buttonResetPassword.TabIndex = 2;
            this.buttonResetPassword.Text = "重置密码";
            this.buttonResetPassword.WaveSize = 0;
            // 
            // buttonToggleEnabled
            // 
            this.buttonToggleEnabled.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonToggleEnabled.Location = new System.Drawing.Point(0, 34);
            this.buttonToggleEnabled.Name = "buttonToggleEnabled";
            this.buttonToggleEnabled.Size = new System.Drawing.Size(252, 34);
            this.buttonToggleEnabled.TabIndex = 1;
            this.buttonToggleEnabled.Text = "启用/禁用";
            this.buttonToggleEnabled.WaveSize = 0;
            // 
            // buttonEdit
            // 
            this.buttonEdit.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonEdit.Location = new System.Drawing.Point(0, 0);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(252, 34);
            this.buttonEdit.TabIndex = 0;
            this.buttonEdit.Text = "编辑用户";
            this.buttonEdit.Type = AntdUI.TTypeMini.Primary;
            this.buttonEdit.WaveSize = 0;
            // 
            // panelDetailInfo
            // 
            this.panelDetailInfo.Back = System.Drawing.Color.Transparent;
            this.panelDetailInfo.Controls.Add(this.labelDetailRemarkValue);
            this.panelDetailInfo.Controls.Add(this.labelDetailRemarkTitle);
            this.panelDetailInfo.Controls.Add(this.labelDetailLastLoginValue);
            this.panelDetailInfo.Controls.Add(this.labelDetailLastLoginTitle);
            this.panelDetailInfo.Controls.Add(this.labelDetailStateValue);
            this.panelDetailInfo.Controls.Add(this.labelDetailStateTitle);
            this.panelDetailInfo.Controls.Add(this.labelDetailRoleValue);
            this.panelDetailInfo.Controls.Add(this.labelDetailRoleTitle);
            this.panelDetailInfo.Controls.Add(this.labelDetailLoginNameValue);
            this.panelDetailInfo.Controls.Add(this.labelDetailLoginNameTitle);
            this.panelDetailInfo.Controls.Add(this.labelDetailUserNameValue);
            this.panelDetailInfo.Controls.Add(this.labelDetailUserNameTitle);
            this.panelDetailInfo.Controls.Add(this.labelDetailTitle);
            this.panelDetailInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetailInfo.Location = new System.Drawing.Point(16, 16);
            this.panelDetailInfo.Name = "panelDetailInfo";
            this.panelDetailInfo.Radius = 0;
            this.panelDetailInfo.Size = new System.Drawing.Size(252, 444);
            this.panelDetailInfo.TabIndex = 0;
            // 
            // labelDetailRemarkValue
            // 
            this.labelDetailRemarkValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDetailRemarkValue.Location = new System.Drawing.Point(0, 286);
            this.labelDetailRemarkValue.Name = "labelDetailRemarkValue";
            this.labelDetailRemarkValue.Size = new System.Drawing.Size(252, 56);
            this.labelDetailRemarkValue.TabIndex = 12;
            this.labelDetailRemarkValue.Text = "-";
            // 
            // labelDetailRemarkTitle
            // 
            this.labelDetailRemarkTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDetailRemarkTitle.ForeColor = System.Drawing.Color.DimGray;
            this.labelDetailRemarkTitle.Location = new System.Drawing.Point(0, 258);
            this.labelDetailRemarkTitle.Name = "labelDetailRemarkTitle";
            this.labelDetailRemarkTitle.Size = new System.Drawing.Size(252, 28);
            this.labelDetailRemarkTitle.TabIndex = 11;
            this.labelDetailRemarkTitle.Text = "备注";
            // 
            // labelDetailLastLoginValue
            // 
            this.labelDetailLastLoginValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDetailLastLoginValue.Location = new System.Drawing.Point(0, 230);
            this.labelDetailLastLoginValue.Name = "labelDetailLastLoginValue";
            this.labelDetailLastLoginValue.Size = new System.Drawing.Size(252, 28);
            this.labelDetailLastLoginValue.TabIndex = 10;
            this.labelDetailLastLoginValue.Text = "-";
            // 
            // labelDetailLastLoginTitle
            // 
            this.labelDetailLastLoginTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDetailLastLoginTitle.ForeColor = System.Drawing.Color.DimGray;
            this.labelDetailLastLoginTitle.Location = new System.Drawing.Point(0, 202);
            this.labelDetailLastLoginTitle.Name = "labelDetailLastLoginTitle";
            this.labelDetailLastLoginTitle.Size = new System.Drawing.Size(252, 28);
            this.labelDetailLastLoginTitle.TabIndex = 9;
            this.labelDetailLastLoginTitle.Text = "最后登录";
            // 
            // labelDetailStateValue
            // 
            this.labelDetailStateValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDetailStateValue.Location = new System.Drawing.Point(0, 174);
            this.labelDetailStateValue.Name = "labelDetailStateValue";
            this.labelDetailStateValue.Size = new System.Drawing.Size(252, 28);
            this.labelDetailStateValue.TabIndex = 8;
            this.labelDetailStateValue.Text = "-";
            // 
            // labelDetailStateTitle
            // 
            this.labelDetailStateTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDetailStateTitle.ForeColor = System.Drawing.Color.DimGray;
            this.labelDetailStateTitle.Location = new System.Drawing.Point(0, 146);
            this.labelDetailStateTitle.Name = "labelDetailStateTitle";
            this.labelDetailStateTitle.Size = new System.Drawing.Size(252, 28);
            this.labelDetailStateTitle.TabIndex = 7;
            this.labelDetailStateTitle.Text = "状态";
            // 
            // labelDetailRoleValue
            // 
            this.labelDetailRoleValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDetailRoleValue.Location = new System.Drawing.Point(0, 118);
            this.labelDetailRoleValue.Name = "labelDetailRoleValue";
            this.labelDetailRoleValue.Size = new System.Drawing.Size(252, 28);
            this.labelDetailRoleValue.TabIndex = 6;
            this.labelDetailRoleValue.Text = "-";
            // 
            // labelDetailRoleTitle
            // 
            this.labelDetailRoleTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDetailRoleTitle.ForeColor = System.Drawing.Color.DimGray;
            this.labelDetailRoleTitle.Location = new System.Drawing.Point(0, 90);
            this.labelDetailRoleTitle.Name = "labelDetailRoleTitle";
            this.labelDetailRoleTitle.Size = new System.Drawing.Size(252, 28);
            this.labelDetailRoleTitle.TabIndex = 5;
            this.labelDetailRoleTitle.Text = "角色";
            // 
            // labelDetailLoginNameValue
            // 
            this.labelDetailLoginNameValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDetailLoginNameValue.Location = new System.Drawing.Point(0, 62);
            this.labelDetailLoginNameValue.Name = "labelDetailLoginNameValue";
            this.labelDetailLoginNameValue.Size = new System.Drawing.Size(252, 28);
            this.labelDetailLoginNameValue.TabIndex = 4;
            this.labelDetailLoginNameValue.Text = "-";
            // 
            // labelDetailLoginNameTitle
            // 
            this.labelDetailLoginNameTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDetailLoginNameTitle.ForeColor = System.Drawing.Color.DimGray;
            this.labelDetailLoginNameTitle.Location = new System.Drawing.Point(0, 34);
            this.labelDetailLoginNameTitle.Name = "labelDetailLoginNameTitle";
            this.labelDetailLoginNameTitle.Size = new System.Drawing.Size(252, 28);
            this.labelDetailLoginNameTitle.TabIndex = 3;
            this.labelDetailLoginNameTitle.Text = "登录名";
            // 
            // labelDetailUserNameValue
            // 
            this.labelDetailUserNameValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDetailUserNameValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.labelDetailUserNameValue.Location = new System.Drawing.Point(0, 34);
            this.labelDetailUserNameValue.Name = "labelDetailUserNameValue";
            this.labelDetailUserNameValue.Size = new System.Drawing.Size(252, 28);
            this.labelDetailUserNameValue.TabIndex = 2;
            this.labelDetailUserNameValue.Text = "-";
            this.labelDetailUserNameValue.Visible = false;
            // 
            // labelDetailUserNameTitle
            // 
            this.labelDetailUserNameTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDetailUserNameTitle.ForeColor = System.Drawing.Color.DimGray;
            this.labelDetailUserNameTitle.Location = new System.Drawing.Point(0, 34);
            this.labelDetailUserNameTitle.Name = "labelDetailUserNameTitle";
            this.labelDetailUserNameTitle.Size = new System.Drawing.Size(252, 0);
            this.labelDetailUserNameTitle.TabIndex = 1;
            this.labelDetailUserNameTitle.Text = "用户名";
            this.labelDetailUserNameTitle.Visible = false;
            // 
            // labelDetailTitle
            // 
            this.labelDetailTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDetailTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Bold);
            this.labelDetailTitle.Location = new System.Drawing.Point(0, 0);
            this.labelDetailTitle.Name = "labelDetailTitle";
            this.labelDetailTitle.Size = new System.Drawing.Size(252, 34);
            this.labelDetailTitle.TabIndex = 0;
            this.labelDetailTitle.Text = "用户详情";
            // 
            // panelSummary
            // 
            this.panelSummary.Back = System.Drawing.Color.Transparent;
            this.panelSummary.Controls.Add(this.panelDisabledCard);
            this.panelSummary.Controls.Add(this.panelEnabledCard);
            this.panelSummary.Controls.Add(this.panelTotalCard);
            this.panelSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSummary.Location = new System.Drawing.Point(0, 68);
            this.panelSummary.Name = "panelSummary";
            this.panelSummary.Radius = 0;
            this.panelSummary.Size = new System.Drawing.Size(832, 72);
            this.panelSummary.TabIndex = 1;
            // 
            // panelDisabledCard
            // 
            this.panelDisabledCard.Back = System.Drawing.Color.White;
            this.panelDisabledCard.Controls.Add(this.labelDisabledValue);
            this.panelDisabledCard.Controls.Add(this.labelDisabledTitle);
            this.panelDisabledCard.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelDisabledCard.Location = new System.Drawing.Point(376, 0);
            this.panelDisabledCard.Name = "panelDisabledCard";
            this.panelDisabledCard.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.panelDisabledCard.Radius = 12;
            this.panelDisabledCard.Shadow = 3;
            this.panelDisabledCard.Size = new System.Drawing.Size(180, 72);
            this.panelDisabledCard.TabIndex = 2;
            // 
            // labelDisabledValue
            // 
            this.labelDisabledValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDisabledValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelDisabledValue.Location = new System.Drawing.Point(18, 30);
            this.labelDisabledValue.Name = "labelDisabledValue";
            this.labelDisabledValue.Size = new System.Drawing.Size(144, 26);
            this.labelDisabledValue.TabIndex = 1;
            this.labelDisabledValue.Text = "0";
            // 
            // labelDisabledTitle
            // 
            this.labelDisabledTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDisabledTitle.ForeColor = System.Drawing.Color.DimGray;
            this.labelDisabledTitle.Location = new System.Drawing.Point(18, 14);
            this.labelDisabledTitle.Name = "labelDisabledTitle";
            this.labelDisabledTitle.Size = new System.Drawing.Size(144, 16);
            this.labelDisabledTitle.TabIndex = 0;
            this.labelDisabledTitle.Text = "已禁用";
            // 
            // panelEnabledCard
            // 
            this.panelEnabledCard.Back = System.Drawing.Color.White;
            this.panelEnabledCard.Controls.Add(this.labelEnabledValue);
            this.panelEnabledCard.Controls.Add(this.labelEnabledTitle);
            this.panelEnabledCard.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelEnabledCard.Location = new System.Drawing.Point(188, 0);
            this.panelEnabledCard.Name = "panelEnabledCard";
            this.panelEnabledCard.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.panelEnabledCard.Radius = 12;
            this.panelEnabledCard.Shadow = 3;
            this.panelEnabledCard.Size = new System.Drawing.Size(188, 72);
            this.panelEnabledCard.TabIndex = 1;
            // 
            // labelEnabledValue
            // 
            this.labelEnabledValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEnabledValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelEnabledValue.Location = new System.Drawing.Point(18, 30);
            this.labelEnabledValue.Name = "labelEnabledValue";
            this.labelEnabledValue.Size = new System.Drawing.Size(152, 26);
            this.labelEnabledValue.TabIndex = 1;
            this.labelEnabledValue.Text = "0";
            // 
            // labelEnabledTitle
            // 
            this.labelEnabledTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelEnabledTitle.ForeColor = System.Drawing.Color.DimGray;
            this.labelEnabledTitle.Location = new System.Drawing.Point(18, 14);
            this.labelEnabledTitle.Name = "labelEnabledTitle";
            this.labelEnabledTitle.Size = new System.Drawing.Size(152, 16);
            this.labelEnabledTitle.TabIndex = 0;
            this.labelEnabledTitle.Text = "已启用";
            // 
            // panelTotalCard
            // 
            this.panelTotalCard.Back = System.Drawing.Color.White;
            this.panelTotalCard.Controls.Add(this.labelTotalValue);
            this.panelTotalCard.Controls.Add(this.labelTotalTitle);
            this.panelTotalCard.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelTotalCard.Location = new System.Drawing.Point(0, 0);
            this.panelTotalCard.Name = "panelTotalCard";
            this.panelTotalCard.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.panelTotalCard.Radius = 12;
            this.panelTotalCard.Shadow = 3;
            this.panelTotalCard.Size = new System.Drawing.Size(188, 72);
            this.panelTotalCard.TabIndex = 0;
            // 
            // labelTotalValue
            // 
            this.labelTotalValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTotalValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelTotalValue.Location = new System.Drawing.Point(18, 30);
            this.labelTotalValue.Name = "labelTotalValue";
            this.labelTotalValue.Size = new System.Drawing.Size(152, 26);
            this.labelTotalValue.TabIndex = 1;
            this.labelTotalValue.Text = "0";
            // 
            // labelTotalTitle
            // 
            this.labelTotalTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTotalTitle.ForeColor = System.Drawing.Color.DimGray;
            this.labelTotalTitle.Location = new System.Drawing.Point(18, 14);
            this.labelTotalTitle.Name = "labelTotalTitle";
            this.labelTotalTitle.Size = new System.Drawing.Size(152, 16);
            this.labelTotalTitle.TabIndex = 0;
            this.labelTotalTitle.Text = "用户总数";
            // 
            // panelToolbar
            // 
            this.panelToolbar.Back = System.Drawing.Color.White;
            this.panelToolbar.Controls.Add(this.inputKeyword);
            this.panelToolbar.Controls.Add(this.labelKeyword);
            this.panelToolbar.Controls.Add(this.buttonClear);
            this.panelToolbar.Controls.Add(this.buttonSearch);
            this.panelToolbar.Controls.Add(this.buttonRefresh);
            this.panelToolbar.Controls.Add(this.buttonAdd);
            this.panelToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelToolbar.Location = new System.Drawing.Point(0, 0);
            this.panelToolbar.Name = "panelToolbar";
            this.panelToolbar.Padding = new System.Windows.Forms.Padding(12);
            this.panelToolbar.Radius = 14;
            this.panelToolbar.Shadow = 4;
            this.panelToolbar.Size = new System.Drawing.Size(832, 68);
            this.panelToolbar.TabIndex = 0;
            // 
            // inputKeyword
            // 
            this.inputKeyword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inputKeyword.Location = new System.Drawing.Point(68, 18);
            this.inputKeyword.Name = "inputKeyword";
            this.inputKeyword.Size = new System.Drawing.Size(380, 32);
            this.inputKeyword.TabIndex = 5;
            // 
            // labelKeyword
            // 
            this.labelKeyword.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelKeyword.Location = new System.Drawing.Point(18, 18);
            this.labelKeyword.Name = "labelKeyword";
            this.labelKeyword.Size = new System.Drawing.Size(50, 32);
            this.labelKeyword.TabIndex = 4;
            this.labelKeyword.Text = "搜索";
            // 
            // buttonClear
            // 
            this.buttonClear.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonClear.Location = new System.Drawing.Point(448, 18);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(86, 32);
            this.buttonClear.TabIndex = 3;
            this.buttonClear.Text = "清空";
            this.buttonClear.WaveSize = 0;
            // 
            // buttonSearch
            // 
            this.buttonSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonSearch.Location = new System.Drawing.Point(534, 18);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(86, 32);
            this.buttonSearch.TabIndex = 2;
            this.buttonSearch.Text = "查询";
            this.buttonSearch.WaveSize = 0;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonRefresh.Location = new System.Drawing.Point(620, 18);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(92, 32);
            this.buttonRefresh.TabIndex = 1;
            this.buttonRefresh.Text = "刷新";
            this.buttonRefresh.WaveSize = 0;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonAdd.Location = new System.Drawing.Point(712, 18);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(102, 32);
            this.buttonAdd.TabIndex = 0;
            this.buttonAdd.Text = "新增用户";
            this.buttonAdd.Type = AntdUI.TTypeMini.Primary;
            this.buttonAdd.WaveSize = 0;
            // 
            // UserManagementPage
            // 
            this.Controls.Add(this.panelRoot);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.Name = "UserManagementPage";
            this.Size = new System.Drawing.Size(832, 616);
            this.panelRoot.ResumeLayout(false);
            this.panelBody.ResumeLayout(false);
            this.panelTableCard.ResumeLayout(false);
            this.panelDetailCard.ResumeLayout(false);
            this.panelDetailActions.ResumeLayout(false);
            this.panelDetailInfo.ResumeLayout(false);
            this.panelSummary.ResumeLayout(false);
            this.panelDisabledCard.ResumeLayout(false);
            this.panelEnabledCard.ResumeLayout(false);
            this.panelTotalCard.ResumeLayout(false);
            this.panelToolbar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AntdUI.Panel panelRoot;
        private AntdUI.Panel panelToolbar;
        private AntdUI.Button buttonAdd;
        private AntdUI.Button buttonRefresh;
        private AntdUI.Button buttonSearch;
        private AntdUI.Button buttonClear;
        private AntdUI.Input inputKeyword;
        private AntdUI.Label labelKeyword;
        private AntdUI.Panel panelSummary;
        private AntdUI.Panel panelTotalCard;
        private AntdUI.Label labelTotalTitle;
        private AntdUI.Label labelTotalValue;
        private AntdUI.Panel panelEnabledCard;
        private AntdUI.Label labelEnabledTitle;
        private AntdUI.Label labelEnabledValue;
        private AntdUI.Panel panelDisabledCard;
        private AntdUI.Label labelDisabledTitle;
        private AntdUI.Label labelDisabledValue;
        private AntdUI.Panel panelBody;
        private AntdUI.Panel panelTableCard;
        private AntdUI.Table tableUsers;
        private AntdUI.Panel panelDetailCard;
        private AntdUI.Panel panelDetailInfo;
        private AntdUI.Label labelDetailTitle;
        private AntdUI.Label labelDetailLoginNameTitle;
        private AntdUI.Label labelDetailLoginNameValue;
        private AntdUI.Label labelDetailRoleTitle;
        private AntdUI.Label labelDetailRoleValue;
        private AntdUI.Label labelDetailStateTitle;
        private AntdUI.Label labelDetailStateValue;
        private AntdUI.Label labelDetailLastLoginTitle;
        private AntdUI.Label labelDetailLastLoginValue;
        private AntdUI.Label labelDetailRemarkTitle;
        private AntdUI.Label labelDetailRemarkValue;
        private AntdUI.Panel panelDetailActions;
        private AntdUI.Button buttonEdit;
        private AntdUI.Button buttonToggleEnabled;
        private AntdUI.Button buttonResetPassword;
        private AntdUI.Button buttonPermission;
        private AntdUI.Label labelDetailUserNameTitle;
        private AntdUI.Label labelDetailUserNameValue;
    }
}