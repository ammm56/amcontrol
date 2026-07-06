namespace AMControlWinF.Views.SysConfig
{
    partial class CameraConfigEditDialog
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
            this.textureBackgroundDialog = new AMControlWinF.Views.Main.TextureBackgroundControl();
            this.panelShell = new AntdUI.Panel();
            this.panelContent = new AntdUI.Panel();
            this.editor = new AMControlWinF.Views.SysConfig.CameraConfigEditControl();
            this.panelFooter = new AntdUI.Panel();
            this.flowFooterButtons = new AntdUI.FlowPanel();
            this.buttonOk = new AntdUI.Button();
            this.buttonCancel = new AntdUI.Button();
            this.panelHeader = new AntdUI.Panel();
            this.flowHeaderRight = new AntdUI.FlowPanel();
            this.labelDescription = new AntdUI.Label();
            this.flowHeaderLeft = new AntdUI.FlowPanel();
            this.labelTitle = new AntdUI.Label();
            this.textureBackgroundDialog.SuspendLayout();
            this.panelShell.SuspendLayout();
            this.panelContent.SuspendLayout();
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
            this.textureBackgroundDialog.Size = new System.Drawing.Size(1080, 660);
            this.textureBackgroundDialog.TabIndex = 0;
            //
            // panelShell
            //
            this.panelShell.BackColor = System.Drawing.Color.Transparent;
            this.panelShell.Controls.Add(this.panelContent);
            this.panelShell.Controls.Add(this.panelFooter);
            this.panelShell.Controls.Add(this.panelHeader);
            this.panelShell.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelShell.Location = new System.Drawing.Point(0, 0);
            this.panelShell.Margin = new System.Windows.Forms.Padding(0);
            this.panelShell.Name = "panelShell";
            this.panelShell.Padding = new System.Windows.Forms.Padding(12);
            this.panelShell.Radius = 16;
            this.panelShell.Shadow = 16;
            this.panelShell.ShadowOpacity = 0.2F;
            this.panelShell.ShadowOpacityAnimation = true;
            this.panelShell.Size = new System.Drawing.Size(1080, 660);
            this.panelShell.TabIndex = 0;
            //
            // panelContent
            //
            this.panelContent.Controls.Add(this.editor);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(28, 84);
            this.panelContent.Margin = new System.Windows.Forms.Padding(0);
            this.panelContent.Name = "panelContent";
            this.panelContent.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.panelContent.Radius = 0;
            this.panelContent.Size = new System.Drawing.Size(1024, 463);
            this.panelContent.TabIndex = 1;
            //
            // editor
            //
            this.editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editor.Location = new System.Drawing.Point(4, 0);
            this.editor.Margin = new System.Windows.Forms.Padding(0);
            this.editor.Name = "editor";
            this.editor.Size = new System.Drawing.Size(1016, 463);
            this.editor.TabIndex = 0;
            //
            // panelFooter
            //
            this.panelFooter.Controls.Add(this.flowFooterButtons);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(28, 547);
            this.panelFooter.Margin = new System.Windows.Forms.Padding(0);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Padding = new System.Windows.Forms.Padding(4, 10, 4, 0);
            this.panelFooter.Radius = 0;
            this.panelFooter.Size = new System.Drawing.Size(1024, 85);
            this.panelFooter.TabIndex = 2;
            //
            // flowFooterButtons
            //
            this.flowFooterButtons.Controls.Add(this.buttonOk);
            this.flowFooterButtons.Controls.Add(this.buttonCancel);
            this.flowFooterButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowFooterButtons.Gap = 10;
            this.flowFooterButtons.Location = new System.Drawing.Point(776, 10);
            this.flowFooterButtons.Margin = new System.Windows.Forms.Padding(0);
            this.flowFooterButtons.Name = "flowFooterButtons";
            this.flowFooterButtons.Size = new System.Drawing.Size(244, 75);
            this.flowFooterButtons.TabIndex = 0;
            this.flowFooterButtons.Text = "flowFooterButtons";
            //
            // buttonOk
            //
            this.buttonOk.Location = new System.Drawing.Point(126, 0);
            this.buttonOk.Margin = new System.Windows.Forms.Padding(0);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Radius = 8;
            this.buttonOk.Size = new System.Drawing.Size(116, 38);
            this.buttonOk.TabIndex = 1;
            this.buttonOk.Text = "保存";
            this.buttonOk.Type = AntdUI.TTypeMini.Primary;
            this.buttonOk.WaveSize = 0;
            //
            // buttonCancel
            //
            this.buttonCancel.Location = new System.Drawing.Point(0, 0);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Radius = 8;
            this.buttonCancel.Size = new System.Drawing.Size(116, 38);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.WaveSize = 0;
            //
            // panelHeader
            //
            this.panelHeader.Controls.Add(this.flowHeaderRight);
            this.panelHeader.Controls.Add(this.flowHeaderLeft);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(28, 28);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Padding = new System.Windows.Forms.Padding(4, 0, 4, 8);
            this.panelHeader.Radius = 0;
            this.panelHeader.Size = new System.Drawing.Size(1024, 56);
            this.panelHeader.TabIndex = 0;
            //
            // flowHeaderRight
            //
            this.flowHeaderRight.Controls.Add(this.labelDescription);
            this.flowHeaderRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowHeaderRight.Location = new System.Drawing.Point(640, 0);
            this.flowHeaderRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowHeaderRight.Name = "flowHeaderRight";
            this.flowHeaderRight.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.flowHeaderRight.Size = new System.Drawing.Size(380, 48);
            this.flowHeaderRight.TabIndex = 1;
            this.flowHeaderRight.Text = "flowHeaderRight";
            //
            // labelDescription
            //
            this.labelDescription.Location = new System.Drawing.Point(0, 0);
            this.labelDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(380, 48);
            this.labelDescription.TabIndex = 0;
            this.labelDescription.Text = "配置 amcontrol 本项目相机的采集、编码与预览参数。";
            this.labelDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // flowHeaderLeft
            //
            this.flowHeaderLeft.Controls.Add(this.labelTitle);
            this.flowHeaderLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowHeaderLeft.Location = new System.Drawing.Point(4, 0);
            this.flowHeaderLeft.Margin = new System.Windows.Forms.Padding(0);
            this.flowHeaderLeft.Name = "flowHeaderLeft";
            this.flowHeaderLeft.Size = new System.Drawing.Size(240, 48);
            this.flowHeaderLeft.TabIndex = 0;
            this.flowHeaderLeft.Text = "flowHeaderLeft";
            //
            // labelTitle
            //
            this.labelTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(0, 0);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(240, 48);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "新增相机";
            //
            // CameraConfigEditDialog
            //
            this.ClientSize = new System.Drawing.Size(1080, 660);
            this.Controls.Add(this.textureBackgroundDialog);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CameraConfigEditDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "相机配置";
            this.textureBackgroundDialog.ResumeLayout(false);
            this.panelShell.ResumeLayout(false);
            this.panelContent.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            this.flowFooterButtons.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.flowHeaderRight.ResumeLayout(false);
            this.flowHeaderLeft.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private AMControlWinF.Views.Main.TextureBackgroundControl textureBackgroundDialog;
        private AntdUI.Panel panelShell;
        private AntdUI.Panel panelContent;
        private CameraConfigEditControl editor;
        private AntdUI.Panel panelFooter;
        private AntdUI.FlowPanel flowFooterButtons;
        private AntdUI.Button buttonCancel;
        private AntdUI.Button buttonOk;
        private AntdUI.Panel panelHeader;
        private AntdUI.FlowPanel flowHeaderRight;
        private AntdUI.Label labelDescription;
        private AntdUI.FlowPanel flowHeaderLeft;
        private AntdUI.Label labelTitle;
    }
}
