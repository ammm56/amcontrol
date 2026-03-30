namespace AMControlWinF.Views.Main
{
    partial class UserAvatarMenuControl
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

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.panelRoot = new AntdUI.Panel();
            this.avatarCurrentUser = new AntdUI.Avatar();
            this.panelRoot.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Back = System.Drawing.Color.Transparent;
            this.panelRoot.Controls.Add(this.avatarCurrentUser);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(72, 72);
            this.panelRoot.TabIndex = 0;
            // 
            // avatarCurrentUser
            // 
            this.avatarCurrentUser.Cursor = System.Windows.Forms.Cursors.Hand;
            this.avatarCurrentUser.Location = new System.Drawing.Point(10, 10);
            this.avatarCurrentUser.Name = "avatarCurrentUser";
            this.avatarCurrentUser.Size = new System.Drawing.Size(52, 52);
            this.avatarCurrentUser.TabIndex = 0;
            this.avatarCurrentUser.Text = "A";
            // 
            // UserAvatarMenuControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panelRoot);
            this.Name = "UserAvatarMenuControl";
            this.Size = new System.Drawing.Size(72, 72);
            this.panelRoot.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.Panel panelRoot;
        private AntdUI.Avatar avatarCurrentUser;
    }
}