namespace AMControlWinF.Views.Auth
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelRoot = new System.Windows.Forms.TableLayoutPanel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelLoginName = new System.Windows.Forms.Label();
            this.textBoxLoginName = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.labelStatusTitle = new System.Windows.Forms.Label();
            this.labelStatusValue = new System.Windows.Forms.Label();
            this.labelErrorTitle = new System.Windows.Forms.Label();
            this.labelErrorValue = new System.Windows.Forms.Label();
            this.panelButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonLogin = new System.Windows.Forms.Button();
            this.panelRoot.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.ColumnCount = 2;
            this.panelRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.panelRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelRoot.Controls.Add(this.labelTitle, 0, 0);
            this.panelRoot.Controls.Add(this.labelLoginName, 0, 1);
            this.panelRoot.Controls.Add(this.textBoxLoginName, 1, 1);
            this.panelRoot.Controls.Add(this.labelPassword, 0, 2);
            this.panelRoot.Controls.Add(this.textBoxPassword, 1, 2);
            this.panelRoot.Controls.Add(this.labelStatusTitle, 0, 3);
            this.panelRoot.Controls.Add(this.labelStatusValue, 1, 3);
            this.panelRoot.Controls.Add(this.labelErrorTitle, 0, 4);
            this.panelRoot.Controls.Add(this.labelErrorValue, 1, 4);
            this.panelRoot.Controls.Add(this.panelButtons, 0, 5);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(12, 12);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.RowCount = 6;
            this.panelRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.panelRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.panelRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.panelRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.panelRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.panelRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelRoot.Size = new System.Drawing.Size(456, 254);
            this.panelRoot.TabIndex = 0;
            // 
            // labelTitle
            // 
            this.panelRoot.SetColumnSpan(this.labelTitle, 2);
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(3, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(450, 48);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "AMControl 登录";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelLoginName
            // 
            this.labelLoginName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLoginName.Location = new System.Drawing.Point(3, 48);
            this.labelLoginName.Name = "labelLoginName";
            this.labelLoginName.Size = new System.Drawing.Size(94, 40);
            this.labelLoginName.TabIndex = 1;
            this.labelLoginName.Text = "登录名";
            this.labelLoginName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxLoginName
            // 
            this.textBoxLoginName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLoginName.Location = new System.Drawing.Point(103, 58);
            this.textBoxLoginName.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.textBoxLoginName.Name = "textBoxLoginName";
            this.textBoxLoginName.Size = new System.Drawing.Size(350, 21);
            this.textBoxLoginName.TabIndex = 0;
            // 
            // labelPassword
            // 
            this.labelPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPassword.Location = new System.Drawing.Point(3, 88);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(94, 40);
            this.labelPassword.TabIndex = 3;
            this.labelPassword.Text = "密码";
            this.labelPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPassword.Location = new System.Drawing.Point(103, 98);
            this.textBoxPassword.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(350, 21);
            this.textBoxPassword.TabIndex = 1;
            this.textBoxPassword.UseSystemPasswordChar = true;
            // 
            // labelStatusTitle
            // 
            this.labelStatusTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelStatusTitle.Location = new System.Drawing.Point(3, 128);
            this.labelStatusTitle.Name = "labelStatusTitle";
            this.labelStatusTitle.Size = new System.Drawing.Size(94, 34);
            this.labelStatusTitle.TabIndex = 5;
            this.labelStatusTitle.Text = "状态";
            this.labelStatusTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelStatusValue
            // 
            this.labelStatusValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelStatusValue.Location = new System.Drawing.Point(103, 128);
            this.labelStatusValue.Name = "labelStatusValue";
            this.labelStatusValue.Size = new System.Drawing.Size(350, 34);
            this.labelStatusValue.TabIndex = 6;
            this.labelStatusValue.Text = "请输入登录信息";
            this.labelStatusValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelErrorTitle
            // 
            this.labelErrorTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelErrorTitle.Location = new System.Drawing.Point(3, 162);
            this.labelErrorTitle.Name = "labelErrorTitle";
            this.labelErrorTitle.Size = new System.Drawing.Size(94, 44);
            this.labelErrorTitle.TabIndex = 7;
            this.labelErrorTitle.Text = "错误";
            this.labelErrorTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelErrorValue
            // 
            this.labelErrorValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelErrorValue.ForeColor = System.Drawing.Color.Firebrick;
            this.labelErrorValue.Location = new System.Drawing.Point(103, 162);
            this.labelErrorValue.Name = "labelErrorValue";
            this.labelErrorValue.Size = new System.Drawing.Size(350, 44);
            this.labelErrorValue.TabIndex = 8;
            this.labelErrorValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelButtons
            // 
            this.panelRoot.SetColumnSpan(this.panelButtons, 2);
            this.panelButtons.Controls.Add(this.buttonCancel);
            this.panelButtons.Controls.Add(this.buttonLogin);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.panelButtons.Location = new System.Drawing.Point(3, 209);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(450, 42);
            this.panelButtons.TabIndex = 9;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(372, 3);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 30);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonLogin
            // 
            this.buttonLogin.Location = new System.Drawing.Point(291, 3);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(75, 30);
            this.buttonLogin.TabIndex = 2;
            this.buttonLogin.Text = "登录";
            this.buttonLogin.UseVisualStyleBackColor = true;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 278);
            this.Controls.Add(this.panelRoot);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.Padding = new System.Windows.Forms.Padding(12);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "登录";
            this.panelRoot.ResumeLayout(false);
            this.panelRoot.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel panelRoot;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelLoginName;
        private System.Windows.Forms.TextBox textBoxLoginName;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label labelStatusTitle;
        private System.Windows.Forms.Label labelStatusValue;
        private System.Windows.Forms.Label labelErrorTitle;
        private System.Windows.Forms.Label labelErrorValue;
        private System.Windows.Forms.FlowLayoutPanel panelButtons;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonLogin;
    }
}