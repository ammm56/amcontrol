namespace AMControlWinF.Views.MotionConfig
{
    partial class MotionCardDeleteConfirmDialog
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
            this.textureBackgroundDialog = new AMControlWinF.Views.Main.TextureBackgroundControl();
            this.panelShell = new AntdUI.Panel();
            this.panelContent = new AntdUI.Panel();
            this.stackRows = new AntdUI.StackPanel();
            this.panelRowHint = new AntdUI.Panel();
            this.labelHintValue = new AntdUI.Label();
            this.labelHintTitle = new AntdUI.Label();
            this.panelRowInternalName = new AntdUI.Panel();
            this.labelInternalNameValue = new AntdUI.Label();
            this.labelInternalNameTitle = new AntdUI.Label();
            this.panelRowDisplayName = new AntdUI.Panel();
            this.labelDisplayNameValue = new AntdUI.Label();
            this.labelDisplayNameTitle = new AntdUI.Label();
            this.panelRowCardId = new AntdUI.Panel();
            this.labelCardIdValue = new AntdUI.Label();
            this.labelCardIdTitle = new AntdUI.Label();
            this.panelFooter = new AntdUI.Panel();
            this.flowFooterButtons = new AntdUI.FlowPanel();
            this.buttonCancel = new AntdUI.Button();
            this.buttonOk = new AntdUI.Button();
            this.panelHeader = new AntdUI.Panel();
            this.flowHeaderRight = new AntdUI.FlowPanel();
            this.labelDialogDescription = new AntdUI.Label();
            this.flowHeaderLeft = new AntdUI.FlowPanel();
            this.labelDialogTitle = new AntdUI.Label();
            this.textureBackgroundDialog.SuspendLayout();
            this.panelShell.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.stackRows.SuspendLayout();
            this.panelRowHint.SuspendLayout();
            this.panelRowInternalName.SuspendLayout();
            this.panelRowDisplayName.SuspendLayout();
            this.panelRowCardId.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.flowFooterButtons.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.flowHeaderRight.SuspendLayout();
            this.flowHeaderLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // textureBackgroundDialog
            // 
            this.textureBackgroundDialog.Controls.Add(this.panelShell);
            this.textureBackgroundDialog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textureBackgroundDialog.Location = new System.Drawing.Point(0, 0);
            this.textureBackgroundDialog.Margin = new System.Windows.Forms.Padding(0);
            this.textureBackgroundDialog.Name = "textureBackgroundDialog";
            this.textureBackgroundDialog.Size = new System.Drawing.Size(560, 420);
            this.textureBackgroundDialog.TabIndex = 0;
            // 
            // panelShell
            // 
            this.panelShell.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelShell.BackColor = System.Drawing.Color.Transparent;
            this.panelShell.Controls.Add(this.panelContent);
            this.panelShell.Controls.Add(this.panelFooter);
            this.panelShell.Controls.Add(this.panelHeader);
            this.panelShell.Location = new System.Drawing.Point(24, 24);
            this.panelShell.Margin = new System.Windows.Forms.Padding(0);
            this.panelShell.Name = "panelShell";
            this.panelShell.Padding = new System.Windows.Forms.Padding(12);
            this.panelShell.Radius = 16;
            this.panelShell.Shadow = 8;
            this.panelShell.Size = new System.Drawing.Size(512, 372);
            this.panelShell.TabIndex = 0;
            // 
            // panelContent
            // 
            this.panelContent.Controls.Add(this.stackRows);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(20, 76);
            this.panelContent.Margin = new System.Windows.Forms.Padding(0);
            this.panelContent.Name = "panelContent";
            this.panelContent.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.panelContent.Radius = 0;
            this.panelContent.Size = new System.Drawing.Size(472, 219);
            this.panelContent.TabIndex = 1;
            // 
            // stackRows
            // 
            this.stackRows.Controls.Add(this.panelRowHint);
            this.stackRows.Controls.Add(this.panelRowInternalName);
            this.stackRows.Controls.Add(this.panelRowDisplayName);
            this.stackRows.Controls.Add(this.panelRowCardId);
            this.stackRows.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stackRows.Gap = 4;
            this.stackRows.Location = new System.Drawing.Point(4, 0);
            this.stackRows.Margin = new System.Windows.Forms.Padding(0);
            this.stackRows.Name = "stackRows";
            this.stackRows.Size = new System.Drawing.Size(464, 219);
            this.stackRows.TabIndex = 0;
            this.stackRows.Text = "stackRows";
            this.stackRows.Vertical = true;
            // 
            // panelRowHint
            // 
            this.panelRowHint.Controls.Add(this.labelHintValue);
            this.panelRowHint.Controls.Add(this.labelHintTitle);
            this.panelRowHint.Location = new System.Drawing.Point(0, 168);
            this.panelRowHint.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowHint.Name = "panelRowHint";
            this.panelRowHint.Radius = 0;
            this.panelRowHint.Size = new System.Drawing.Size(464, 48);
            this.panelRowHint.TabIndex = 3;
            // 
            // labelHintValue
            // 
            this.labelHintValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHintValue.Location = new System.Drawing.Point(0, 22);
            this.labelHintValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelHintValue.Name = "labelHintValue";
            this.labelHintValue.Size = new System.Drawing.Size(464, 26);
            this.labelHintValue.TabIndex = 1;
            this.labelHintValue.Text = "-";
            // 
            // labelHintTitle
            // 
            this.labelHintTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelHintTitle.Location = new System.Drawing.Point(0, 0);
            this.labelHintTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelHintTitle.Name = "labelHintTitle";
            this.labelHintTitle.Size = new System.Drawing.Size(464, 22);
            this.labelHintTitle.TabIndex = 0;
            this.labelHintTitle.Text = "操作提示";
            // 
            // panelRowInternalName
            // 
            this.panelRowInternalName.Controls.Add(this.labelInternalNameValue);
            this.panelRowInternalName.Controls.Add(this.labelInternalNameTitle);
            this.panelRowInternalName.Location = new System.Drawing.Point(0, 112);
            this.panelRowInternalName.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowInternalName.Name = "panelRowInternalName";
            this.panelRowInternalName.Radius = 0;
            this.panelRowInternalName.Size = new System.Drawing.Size(464, 52);
            this.panelRowInternalName.TabIndex = 2;
            // 
            // labelInternalNameValue
            // 
            this.labelInternalNameValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelInternalNameValue.Location = new System.Drawing.Point(0, 22);
            this.labelInternalNameValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelInternalNameValue.Name = "labelInternalNameValue";
            this.labelInternalNameValue.Size = new System.Drawing.Size(464, 30);
            this.labelInternalNameValue.TabIndex = 1;
            this.labelInternalNameValue.Text = "-";
            // 
            // labelInternalNameTitle
            // 
            this.labelInternalNameTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelInternalNameTitle.Location = new System.Drawing.Point(0, 0);
            this.labelInternalNameTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelInternalNameTitle.Name = "labelInternalNameTitle";
            this.labelInternalNameTitle.Size = new System.Drawing.Size(464, 22);
            this.labelInternalNameTitle.TabIndex = 0;
            this.labelInternalNameTitle.Text = "内部名称";
            // 
            // panelRowDisplayName
            // 
            this.panelRowDisplayName.Controls.Add(this.labelDisplayNameValue);
            this.panelRowDisplayName.Controls.Add(this.labelDisplayNameTitle);
            this.panelRowDisplayName.Location = new System.Drawing.Point(0, 56);
            this.panelRowDisplayName.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowDisplayName.Name = "panelRowDisplayName";
            this.panelRowDisplayName.Radius = 0;
            this.panelRowDisplayName.Size = new System.Drawing.Size(464, 52);
            this.panelRowDisplayName.TabIndex = 1;
            // 
            // labelDisplayNameValue
            // 
            this.labelDisplayNameValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDisplayNameValue.Location = new System.Drawing.Point(0, 22);
            this.labelDisplayNameValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelDisplayNameValue.Name = "labelDisplayNameValue";
            this.labelDisplayNameValue.Size = new System.Drawing.Size(464, 30);
            this.labelDisplayNameValue.TabIndex = 1;
            this.labelDisplayNameValue.Text = "-";
            // 
            // labelDisplayNameTitle
            // 
            this.labelDisplayNameTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDisplayNameTitle.Location = new System.Drawing.Point(0, 0);
            this.labelDisplayNameTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelDisplayNameTitle.Name = "labelDisplayNameTitle";
            this.labelDisplayNameTitle.Size = new System.Drawing.Size(464, 22);
            this.labelDisplayNameTitle.TabIndex = 0;
            this.labelDisplayNameTitle.Text = "显示名称";
            // 
            // panelRowCardId
            // 
            this.panelRowCardId.Controls.Add(this.labelCardIdValue);
            this.panelRowCardId.Controls.Add(this.labelCardIdTitle);
            this.panelRowCardId.Location = new System.Drawing.Point(0, 0);
            this.panelRowCardId.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowCardId.Name = "panelRowCardId";
            this.panelRowCardId.Radius = 0;
            this.panelRowCardId.Size = new System.Drawing.Size(464, 52);
            this.panelRowCardId.TabIndex = 0;
            // 
            // labelCardIdValue
            // 
            this.labelCardIdValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCardIdValue.Location = new System.Drawing.Point(0, 22);
            this.labelCardIdValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelCardIdValue.Name = "labelCardIdValue";
            this.labelCardIdValue.Size = new System.Drawing.Size(464, 30);
            this.labelCardIdValue.TabIndex = 1;
            this.labelCardIdValue.Text = "-";
            // 
            // labelCardIdTitle
            // 
            this.labelCardIdTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelCardIdTitle.Location = new System.Drawing.Point(0, 0);
            this.labelCardIdTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelCardIdTitle.Name = "labelCardIdTitle";
            this.labelCardIdTitle.Size = new System.Drawing.Size(464, 22);
            this.labelCardIdTitle.TabIndex = 0;
            this.labelCardIdTitle.Text = "卡号";
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.flowFooterButtons);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(20, 295);
            this.panelFooter.Margin = new System.Windows.Forms.Padding(0);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Padding = new System.Windows.Forms.Padding(4, 10, 4, 0);
            this.panelFooter.Radius = 0;
            this.panelFooter.Size = new System.Drawing.Size(472, 57);
            this.panelFooter.TabIndex = 2;
            // 
            // flowFooterButtons
            // 
            this.flowFooterButtons.Controls.Add(this.buttonOk);
            this.flowFooterButtons.Controls.Add(this.buttonCancel);
            this.flowFooterButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowFooterButtons.Gap = 10;
            this.flowFooterButtons.Location = new System.Drawing.Point(224, 10);
            this.flowFooterButtons.Margin = new System.Windows.Forms.Padding(0);
            this.flowFooterButtons.Name = "flowFooterButtons";
            this.flowFooterButtons.Size = new System.Drawing.Size(244, 47);
            this.flowFooterButtons.TabIndex = 0;
            this.flowFooterButtons.Text = "flowFooterButtons";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(126, 0);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Radius = 8;
            this.buttonCancel.Size = new System.Drawing.Size(116, 38);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.WaveSize = 0;
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(0, 0);
            this.buttonOk.Margin = new System.Windows.Forms.Padding(0);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Radius = 8;
            this.buttonOk.Size = new System.Drawing.Size(116, 38);
            this.buttonOk.TabIndex = 1;
            this.buttonOk.Text = "删除";
            this.buttonOk.Type = AntdUI.TTypeMini.Error;
            this.buttonOk.WaveSize = 0;
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.flowHeaderRight);
            this.panelHeader.Controls.Add(this.flowHeaderLeft);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(20, 20);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Padding = new System.Windows.Forms.Padding(4, 0, 4, 8);
            this.panelHeader.Radius = 0;
            this.panelHeader.Size = new System.Drawing.Size(472, 56);
            this.panelHeader.TabIndex = 0;
            // 
            // flowHeaderRight
            // 
            this.flowHeaderRight.Controls.Add(this.labelDialogDescription);
            this.flowHeaderRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowHeaderRight.Location = new System.Drawing.Point(188, 0);
            this.flowHeaderRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowHeaderRight.Name = "flowHeaderRight";
            this.flowHeaderRight.Size = new System.Drawing.Size(280, 48);
            this.flowHeaderRight.TabIndex = 1;
            this.flowHeaderRight.Text = "flowHeaderRight";
            // 
            // labelDialogDescription
            // 
            this.labelDialogDescription.Location = new System.Drawing.Point(0, 0);
            this.labelDialogDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDialogDescription.Name = "labelDialogDescription";
            this.labelDialogDescription.Size = new System.Drawing.Size(280, 48);
            this.labelDialogDescription.TabIndex = 0;
            this.labelDialogDescription.Text = "请确认是否继续执行当前操作。";
            this.labelDialogDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // flowHeaderLeft
            // 
            this.flowHeaderLeft.Controls.Add(this.labelDialogTitle);
            this.flowHeaderLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowHeaderLeft.Location = new System.Drawing.Point(4, 0);
            this.flowHeaderLeft.Margin = new System.Windows.Forms.Padding(0);
            this.flowHeaderLeft.Name = "flowHeaderLeft";
            this.flowHeaderLeft.Size = new System.Drawing.Size(220, 48);
            this.flowHeaderLeft.TabIndex = 0;
            this.flowHeaderLeft.Text = "flowHeaderLeft";
            // 
            // labelDialogTitle
            // 
            this.labelDialogTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelDialogTitle.Location = new System.Drawing.Point(0, 0);
            this.labelDialogTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelDialogTitle.Name = "labelDialogTitle";
            this.labelDialogTitle.Size = new System.Drawing.Size(220, 38);
            this.labelDialogTitle.TabIndex = 0;
            this.labelDialogTitle.Text = "删除控制卡";
            // 
            // MotionCardDeleteConfirmDialog
            // 
            this.ClientSize = new System.Drawing.Size(560, 420);
            this.Controls.Add(this.textureBackgroundDialog);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MotionCardDeleteConfirmDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "删除控制卡";
            this.textureBackgroundDialog.ResumeLayout(false);
            this.panelShell.ResumeLayout(false);
            this.panelContent.ResumeLayout(false);
            this.stackRows.ResumeLayout(false);
            this.panelRowHint.ResumeLayout(false);
            this.panelRowInternalName.ResumeLayout(false);
            this.panelRowDisplayName.ResumeLayout(false);
            this.panelRowCardId.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            this.flowFooterButtons.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.flowHeaderRight.ResumeLayout(false);
            this.flowHeaderLeft.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AMControlWinF.Views.Main.TextureBackgroundControl textureBackgroundDialog;
        private AntdUI.Panel panelShell;
        private AntdUI.Panel panelHeader;
        private AntdUI.FlowPanel flowHeaderLeft;
        private AntdUI.Label labelDialogTitle;
        private AntdUI.FlowPanel flowHeaderRight;
        private AntdUI.Label labelDialogDescription;
        private AntdUI.Panel panelContent;
        private AntdUI.StackPanel stackRows;
        private AntdUI.Panel panelRowCardId;
        private AntdUI.Label labelCardIdValue;
        private AntdUI.Label labelCardIdTitle;
        private AntdUI.Panel panelRowDisplayName;
        private AntdUI.Label labelDisplayNameValue;
        private AntdUI.Label labelDisplayNameTitle;
        private AntdUI.Panel panelRowInternalName;
        private AntdUI.Label labelInternalNameValue;
        private AntdUI.Label labelInternalNameTitle;
        private AntdUI.Panel panelRowHint;
        private AntdUI.Label labelHintValue;
        private AntdUI.Label labelHintTitle;
        private AntdUI.Panel panelFooter;
        private AntdUI.FlowPanel flowFooterButtons;
        private AntdUI.Button buttonCancel;
        private AntdUI.Button buttonOk;
    }
}