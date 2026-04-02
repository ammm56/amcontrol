namespace AMControlWinF.Views.MotionConfig
{
    partial class MotionCardControl
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
            this.panelCard = new AntdUI.Panel();
            this.panelBottom = new AntdUI.Panel();
            this.flowBottomRight = new AntdUI.FlowPanel();
            this.buttonStatusTag = new AntdUI.Button();
            this.flowBottomLeft = new AntdUI.FlowPanel();
            this.labelValueAxisCount = new AntdUI.Label();
            this.labelAxisCount = new AntdUI.Label();
            this.labelValueCore = new AntdUI.Label();
            this.labelCore = new AntdUI.Label();
            this.panelBody = new AntdUI.Panel();
            this.panelRight = new AntdUI.Panel();
            this.labelDescription = new AntdUI.Label();
            this.labelDescriptionTitle = new AntdUI.Label();
            this.labelValueRemark = new AntdUI.Label();
            this.labelRemark = new AntdUI.Label();
            this.labelValueExtModule = new AntdUI.Label();
            this.labelExtModule = new AntdUI.Label();
            this.labelValueOpenConfig = new AntdUI.Label();
            this.labelOpenConfig = new AntdUI.Label();
            this.labelValueModeParam = new AntdUI.Label();
            this.labelModeParam = new AntdUI.Label();
            this.panelLeft = new AntdUI.Panel();
            this.labelValueUpdateTime = new AntdUI.Label();
            this.labelUpdateTime = new AntdUI.Label();
            this.labelValueStatus = new AntdUI.Label();
            this.labelStatus = new AntdUI.Label();
            this.labelValueDriverKey = new AntdUI.Label();
            this.labelDriverKey = new AntdUI.Label();
            this.labelValueName = new AntdUI.Label();
            this.labelName = new AntdUI.Label();
            this.labelValueDisplayName = new AntdUI.Label();
            this.labelDisplayName = new AntdUI.Label();
            this.labelValueCardId = new AntdUI.Label();
            this.labelCardId = new AntdUI.Label();
            this.panelHeader = new AntdUI.Panel();
            this.flowHeaderRight = new AntdUI.FlowPanel();
            this.buttonEdit = new AntdUI.Button();
            this.buttonDelete = new AntdUI.Button();
            this.flowHeaderLeft = new AntdUI.FlowPanel();
            this.labelTitle = new AntdUI.Label();
            this.buttonTypeTag = new AntdUI.Button();
            this.panelCard.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.flowBottomRight.SuspendLayout();
            this.flowBottomLeft.SuspendLayout();
            this.panelBody.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.flowHeaderRight.SuspendLayout();
            this.flowHeaderLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelCard
            // 
            this.panelCard.BackColor = System.Drawing.Color.Transparent;
            this.panelCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(229)))), ((int)(((byte)(235)))));
            this.panelCard.Controls.Add(this.panelBottom);
            this.panelCard.Controls.Add(this.panelBody);
            this.panelCard.Controls.Add(this.panelHeader);
            this.panelCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCard.Location = new System.Drawing.Point(0, 0);
            this.panelCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelCard.Name = "panelCard";
            this.panelCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelCard.Radius = 12;
            this.panelCard.Shadow = 4;
            this.panelCard.ShadowOpacity = 0.2F;
            this.panelCard.ShadowOpacityAnimation = true;
            this.panelCard.Size = new System.Drawing.Size(400, 360);
            this.panelCard.TabIndex = 0;
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.flowBottomRight);
            this.panelBottom.Controls.Add(this.flowBottomLeft);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(16, 300);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Padding = new System.Windows.Forms.Padding(4);
            this.panelBottom.Size = new System.Drawing.Size(368, 44);
            this.panelBottom.TabIndex = 2;
            this.panelBottom.Text = "panel1";
            // 
            // flowBottomRight
            // 
            this.flowBottomRight.Align = AntdUI.TAlignFlow.Right;
            this.flowBottomRight.Controls.Add(this.buttonStatusTag);
            this.flowBottomRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowBottomRight.Location = new System.Drawing.Point(264, 4);
            this.flowBottomRight.Name = "flowBottomRight";
            this.flowBottomRight.Size = new System.Drawing.Size(100, 36);
            this.flowBottomRight.TabIndex = 0;
            this.flowBottomRight.Text = "flowBottomRight";
            // 
            // buttonStatusTag
            // 
            this.buttonStatusTag.Location = new System.Drawing.Point(28, 6);
            this.buttonStatusTag.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.buttonStatusTag.Name = "buttonStatusTag";
            this.buttonStatusTag.Radius = 8;
            this.buttonStatusTag.Size = new System.Drawing.Size(72, 26);
            this.buttonStatusTag.TabIndex = 0;
            this.buttonStatusTag.Text = "已启用";
            this.buttonStatusTag.Type = AntdUI.TTypeMini.Success;
            this.buttonStatusTag.WaveSize = 0;
            // 
            // flowBottomLeft
            // 
            this.flowBottomLeft.Controls.Add(this.labelValueAxisCount);
            this.flowBottomLeft.Controls.Add(this.labelAxisCount);
            this.flowBottomLeft.Controls.Add(this.labelValueCore);
            this.flowBottomLeft.Controls.Add(this.labelCore);
            this.flowBottomLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowBottomLeft.Location = new System.Drawing.Point(4, 4);
            this.flowBottomLeft.Name = "flowBottomLeft";
            this.flowBottomLeft.Size = new System.Drawing.Size(200, 36);
            this.flowBottomLeft.TabIndex = 0;
            this.flowBottomLeft.Text = "flowBottomLeft";
            // 
            // labelValueAxisCount
            // 
            this.labelValueAxisCount.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelValueAxisCount.Location = new System.Drawing.Point(118, 0);
            this.labelValueAxisCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueAxisCount.Name = "labelValueAxisCount";
            this.labelValueAxisCount.Size = new System.Drawing.Size(38, 36);
            this.labelValueAxisCount.TabIndex = 3;
            this.labelValueAxisCount.Text = "-";
            // 
            // labelAxisCount
            // 
            this.labelAxisCount.Location = new System.Drawing.Point(78, 0);
            this.labelAxisCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelAxisCount.Name = "labelAxisCount";
            this.labelAxisCount.Size = new System.Drawing.Size(40, 36);
            this.labelAxisCount.TabIndex = 2;
            this.labelAxisCount.Text = "轴数：";
            // 
            // labelValueCore
            // 
            this.labelValueCore.Location = new System.Drawing.Point(40, 0);
            this.labelValueCore.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueCore.Name = "labelValueCore";
            this.labelValueCore.Size = new System.Drawing.Size(38, 36);
            this.labelValueCore.TabIndex = 1;
            this.labelValueCore.Text = "-";
            // 
            // labelCore
            // 
            this.labelCore.Location = new System.Drawing.Point(0, 0);
            this.labelCore.Margin = new System.Windows.Forms.Padding(0);
            this.labelCore.Name = "labelCore";
            this.labelCore.Size = new System.Drawing.Size(40, 36);
            this.labelCore.TabIndex = 0;
            this.labelCore.Text = "核数：";
            // 
            // panelBody
            // 
            this.panelBody.Controls.Add(this.panelRight);
            this.panelBody.Controls.Add(this.panelLeft);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(16, 60);
            this.panelBody.Margin = new System.Windows.Forms.Padding(0);
            this.panelBody.Name = "panelBody";
            this.panelBody.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.panelBody.Radius = 0;
            this.panelBody.Size = new System.Drawing.Size(368, 284);
            this.panelBody.TabIndex = 1;
            // 
            // panelRight
            // 
            this.panelRight.Controls.Add(this.labelDescription);
            this.panelRight.Controls.Add(this.labelDescriptionTitle);
            this.panelRight.Controls.Add(this.labelValueRemark);
            this.panelRight.Controls.Add(this.labelRemark);
            this.panelRight.Controls.Add(this.labelValueExtModule);
            this.panelRight.Controls.Add(this.labelExtModule);
            this.panelRight.Controls.Add(this.labelValueOpenConfig);
            this.panelRight.Controls.Add(this.labelOpenConfig);
            this.panelRight.Controls.Add(this.labelValueModeParam);
            this.panelRight.Controls.Add(this.labelModeParam);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRight.Location = new System.Drawing.Point(183, 4);
            this.panelRight.Margin = new System.Windows.Forms.Padding(0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.panelRight.Radius = 0;
            this.panelRight.Size = new System.Drawing.Size(185, 280);
            this.panelRight.TabIndex = 1;
            // 
            // labelDescription
            // 
            this.labelDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDescription.Location = new System.Drawing.Point(8, 206);
            this.labelDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(177, 50);
            this.labelDescription.TabIndex = 13;
            this.labelDescription.Text = "-";
            this.labelDescription.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            // 
            // labelDescriptionTitle
            // 
            this.labelDescriptionTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDescriptionTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelDescriptionTitle.Location = new System.Drawing.Point(8, 184);
            this.labelDescriptionTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelDescriptionTitle.Name = "labelDescriptionTitle";
            this.labelDescriptionTitle.Size = new System.Drawing.Size(177, 22);
            this.labelDescriptionTitle.TabIndex = 12;
            this.labelDescriptionTitle.Text = "描述";
            // 
            // labelValueRemark
            // 
            this.labelValueRemark.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelValueRemark.Location = new System.Drawing.Point(8, 160);
            this.labelValueRemark.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueRemark.Name = "labelValueRemark";
            this.labelValueRemark.Size = new System.Drawing.Size(177, 24);
            this.labelValueRemark.TabIndex = 11;
            this.labelValueRemark.Text = "-";
            // 
            // labelRemark
            // 
            this.labelRemark.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelRemark.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelRemark.Location = new System.Drawing.Point(8, 138);
            this.labelRemark.Margin = new System.Windows.Forms.Padding(0);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(177, 22);
            this.labelRemark.TabIndex = 10;
            this.labelRemark.Text = "备注";
            // 
            // labelValueExtModule
            // 
            this.labelValueExtModule.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelValueExtModule.Location = new System.Drawing.Point(8, 114);
            this.labelValueExtModule.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueExtModule.Name = "labelValueExtModule";
            this.labelValueExtModule.Size = new System.Drawing.Size(177, 24);
            this.labelValueExtModule.TabIndex = 9;
            this.labelValueExtModule.Text = "-";
            // 
            // labelExtModule
            // 
            this.labelExtModule.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelExtModule.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelExtModule.Location = new System.Drawing.Point(8, 92);
            this.labelExtModule.Margin = new System.Windows.Forms.Padding(0);
            this.labelExtModule.Name = "labelExtModule";
            this.labelExtModule.Size = new System.Drawing.Size(177, 22);
            this.labelExtModule.TabIndex = 8;
            this.labelExtModule.Text = "扩展模块";
            // 
            // labelValueOpenConfig
            // 
            this.labelValueOpenConfig.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelValueOpenConfig.Location = new System.Drawing.Point(8, 68);
            this.labelValueOpenConfig.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueOpenConfig.Name = "labelValueOpenConfig";
            this.labelValueOpenConfig.Size = new System.Drawing.Size(177, 24);
            this.labelValueOpenConfig.TabIndex = 7;
            this.labelValueOpenConfig.Text = "-";
            // 
            // labelOpenConfig
            // 
            this.labelOpenConfig.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelOpenConfig.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelOpenConfig.Location = new System.Drawing.Point(8, 46);
            this.labelOpenConfig.Margin = new System.Windows.Forms.Padding(0);
            this.labelOpenConfig.Name = "labelOpenConfig";
            this.labelOpenConfig.Size = new System.Drawing.Size(177, 22);
            this.labelOpenConfig.TabIndex = 6;
            this.labelOpenConfig.Text = "初始化参数";
            // 
            // labelValueModeParam
            // 
            this.labelValueModeParam.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelValueModeParam.Location = new System.Drawing.Point(8, 22);
            this.labelValueModeParam.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueModeParam.Name = "labelValueModeParam";
            this.labelValueModeParam.Size = new System.Drawing.Size(177, 24);
            this.labelValueModeParam.TabIndex = 5;
            this.labelValueModeParam.Text = "-";
            // 
            // labelModeParam
            // 
            this.labelModeParam.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelModeParam.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelModeParam.Location = new System.Drawing.Point(8, 0);
            this.labelModeParam.Margin = new System.Windows.Forms.Padding(0);
            this.labelModeParam.Name = "labelModeParam";
            this.labelModeParam.Size = new System.Drawing.Size(177, 22);
            this.labelModeParam.TabIndex = 4;
            this.labelModeParam.Text = "模式参数";
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.labelValueUpdateTime);
            this.panelLeft.Controls.Add(this.labelUpdateTime);
            this.panelLeft.Controls.Add(this.labelValueStatus);
            this.panelLeft.Controls.Add(this.labelStatus);
            this.panelLeft.Controls.Add(this.labelValueDriverKey);
            this.panelLeft.Controls.Add(this.labelDriverKey);
            this.panelLeft.Controls.Add(this.labelValueName);
            this.panelLeft.Controls.Add(this.labelName);
            this.panelLeft.Controls.Add(this.labelValueDisplayName);
            this.panelLeft.Controls.Add(this.labelDisplayName);
            this.panelLeft.Controls.Add(this.labelValueCardId);
            this.panelLeft.Controls.Add(this.labelCardId);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 4);
            this.panelLeft.Margin = new System.Windows.Forms.Padding(0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Padding = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.panelLeft.Radius = 0;
            this.panelLeft.Size = new System.Drawing.Size(185, 280);
            this.panelLeft.TabIndex = 0;
            // 
            // labelValueUpdateTime
            // 
            this.labelValueUpdateTime.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelValueUpdateTime.Location = new System.Drawing.Point(0, 208);
            this.labelValueUpdateTime.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueUpdateTime.Name = "labelValueUpdateTime";
            this.labelValueUpdateTime.Size = new System.Drawing.Size(177, 24);
            this.labelValueUpdateTime.TabIndex = 11;
            this.labelValueUpdateTime.Text = "-";
            // 
            // labelUpdateTime
            // 
            this.labelUpdateTime.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelUpdateTime.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelUpdateTime.Location = new System.Drawing.Point(0, 186);
            this.labelUpdateTime.Margin = new System.Windows.Forms.Padding(0);
            this.labelUpdateTime.Name = "labelUpdateTime";
            this.labelUpdateTime.Size = new System.Drawing.Size(177, 22);
            this.labelUpdateTime.TabIndex = 10;
            this.labelUpdateTime.Text = "更新时间";
            // 
            // labelValueStatus
            // 
            this.labelValueStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelValueStatus.Location = new System.Drawing.Point(0, 162);
            this.labelValueStatus.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueStatus.Name = "labelValueStatus";
            this.labelValueStatus.Size = new System.Drawing.Size(177, 24);
            this.labelValueStatus.TabIndex = 9;
            this.labelValueStatus.Text = "-";
            this.labelValueStatus.Visible = false;
            // 
            // labelStatus
            // 
            this.labelStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelStatus.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelStatus.Location = new System.Drawing.Point(0, 140);
            this.labelStatus.Margin = new System.Windows.Forms.Padding(0);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(177, 22);
            this.labelStatus.TabIndex = 8;
            this.labelStatus.Text = "状态";
            this.labelStatus.Visible = false;
            // 
            // labelValueDriverKey
            // 
            this.labelValueDriverKey.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelValueDriverKey.Location = new System.Drawing.Point(0, 116);
            this.labelValueDriverKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueDriverKey.Name = "labelValueDriverKey";
            this.labelValueDriverKey.Size = new System.Drawing.Size(177, 24);
            this.labelValueDriverKey.TabIndex = 7;
            this.labelValueDriverKey.Text = "-";
            // 
            // labelDriverKey
            // 
            this.labelDriverKey.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDriverKey.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelDriverKey.Location = new System.Drawing.Point(0, 94);
            this.labelDriverKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelDriverKey.Name = "labelDriverKey";
            this.labelDriverKey.Size = new System.Drawing.Size(177, 22);
            this.labelDriverKey.TabIndex = 6;
            this.labelDriverKey.Text = "内部名称";
            // 
            // labelValueName
            // 
            this.labelValueName.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelValueName.Location = new System.Drawing.Point(0, 70);
            this.labelValueName.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueName.Name = "labelValueName";
            this.labelValueName.Size = new System.Drawing.Size(177, 24);
            this.labelValueName.TabIndex = 5;
            this.labelValueName.Text = "-";
            // 
            // labelName
            // 
            this.labelName.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelName.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelName.Location = new System.Drawing.Point(0, 48);
            this.labelName.Margin = new System.Windows.Forms.Padding(0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(177, 22);
            this.labelName.TabIndex = 4;
            this.labelName.Text = "名称";
            // 
            // labelValueDisplayName
            // 
            this.labelValueDisplayName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelValueDisplayName.Location = new System.Drawing.Point(66, 162);
            this.labelValueDisplayName.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueDisplayName.Name = "labelValueDisplayName";
            this.labelValueDisplayName.Size = new System.Drawing.Size(96, 24);
            this.labelValueDisplayName.TabIndex = 3;
            this.labelValueDisplayName.Text = "-";
            this.labelValueDisplayName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelValueDisplayName.Visible = false;
            // 
            // labelDisplayName
            // 
            this.labelDisplayName.Location = new System.Drawing.Point(66, 136);
            this.labelDisplayName.Margin = new System.Windows.Forms.Padding(0);
            this.labelDisplayName.Name = "labelDisplayName";
            this.labelDisplayName.Size = new System.Drawing.Size(100, 24);
            this.labelDisplayName.TabIndex = 2;
            this.labelDisplayName.Text = "显示名称";
            this.labelDisplayName.Visible = false;
            // 
            // labelValueCardId
            // 
            this.labelValueCardId.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelValueCardId.Location = new System.Drawing.Point(0, 24);
            this.labelValueCardId.Margin = new System.Windows.Forms.Padding(0);
            this.labelValueCardId.Name = "labelValueCardId";
            this.labelValueCardId.Size = new System.Drawing.Size(177, 24);
            this.labelValueCardId.TabIndex = 1;
            this.labelValueCardId.Text = "-";
            // 
            // labelCardId
            // 
            this.labelCardId.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelCardId.Location = new System.Drawing.Point(0, 0);
            this.labelCardId.Margin = new System.Windows.Forms.Padding(0);
            this.labelCardId.Name = "labelCardId";
            this.labelCardId.Size = new System.Drawing.Size(177, 24);
            this.labelCardId.TabIndex = 0;
            this.labelCardId.Text = "卡号";
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.flowHeaderRight);
            this.panelHeader.Controls.Add(this.flowHeaderLeft);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(16, 16);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Padding = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.panelHeader.Radius = 0;
            this.panelHeader.Size = new System.Drawing.Size(368, 44);
            this.panelHeader.TabIndex = 0;
            // 
            // flowHeaderRight
            // 
            this.flowHeaderRight.Controls.Add(this.buttonEdit);
            this.flowHeaderRight.Controls.Add(this.buttonDelete);
            this.flowHeaderRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowHeaderRight.Gap = 8;
            this.flowHeaderRight.Location = new System.Drawing.Point(200, 4);
            this.flowHeaderRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowHeaderRight.Name = "flowHeaderRight";
            this.flowHeaderRight.Size = new System.Drawing.Size(168, 36);
            this.flowHeaderRight.TabIndex = 1;
            this.flowHeaderRight.Text = "flowHeaderRight";
            // 
            // buttonEdit
            // 
            this.buttonEdit.IconSvg = "EditOutlined";
            this.buttonEdit.Location = new System.Drawing.Point(88, 0);
            this.buttonEdit.Margin = new System.Windows.Forms.Padding(0);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Radius = 8;
            this.buttonEdit.Size = new System.Drawing.Size(80, 36);
            this.buttonEdit.TabIndex = 0;
            this.buttonEdit.Text = "编辑";
            this.buttonEdit.Type = AntdUI.TTypeMini.Primary;
            this.buttonEdit.WaveSize = 0;
            // 
            // buttonDelete
            // 
            this.buttonDelete.IconSvg = "DeleteOutlined";
            this.buttonDelete.Location = new System.Drawing.Point(0, 0);
            this.buttonDelete.Margin = new System.Windows.Forms.Padding(0);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Radius = 8;
            this.buttonDelete.Size = new System.Drawing.Size(80, 36);
            this.buttonDelete.TabIndex = 1;
            this.buttonDelete.Text = "删除";
            this.buttonDelete.WaveSize = 0;
            // 
            // flowHeaderLeft
            // 
            this.flowHeaderLeft.Controls.Add(this.labelTitle);
            this.flowHeaderLeft.Controls.Add(this.buttonTypeTag);
            this.flowHeaderLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowHeaderLeft.Gap = 6;
            this.flowHeaderLeft.Location = new System.Drawing.Point(0, 4);
            this.flowHeaderLeft.Margin = new System.Windows.Forms.Padding(0);
            this.flowHeaderLeft.Name = "flowHeaderLeft";
            this.flowHeaderLeft.Size = new System.Drawing.Size(198, 36);
            this.flowHeaderLeft.TabIndex = 0;
            this.flowHeaderLeft.Text = "flowHeaderLeft";
            // 
            // labelTitle
            // 
            this.labelTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(66, 0);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(120, 36);
            this.labelTitle.TabIndex = 2;
            this.labelTitle.Text = "默认虚拟卡";
            // 
            // buttonTypeTag
            // 
            this.buttonTypeTag.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonTypeTag.Location = new System.Drawing.Point(0, 6);
            this.buttonTypeTag.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.buttonTypeTag.Name = "buttonTypeTag";
            this.buttonTypeTag.Radius = 8;
            this.buttonTypeTag.Size = new System.Drawing.Size(60, 24);
            this.buttonTypeTag.TabIndex = 1;
            this.buttonTypeTag.Text = "VIRTUAL";
            this.buttonTypeTag.Type = AntdUI.TTypeMini.Primary;
            this.buttonTypeTag.WaveSize = 0;
            // 
            // MotionCardControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelCard);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MotionCardControl";
            this.Size = new System.Drawing.Size(400, 360);
            this.panelCard.ResumeLayout(false);
            this.panelBottom.ResumeLayout(false);
            this.flowBottomRight.ResumeLayout(false);
            this.flowBottomLeft.ResumeLayout(false);
            this.panelBody.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.flowHeaderRight.ResumeLayout(false);
            this.flowHeaderLeft.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AntdUI.Panel panelCard;
        private AntdUI.Panel panelHeader;
        private AntdUI.FlowPanel flowHeaderRight;
        private AntdUI.Button buttonEdit;
        private AntdUI.Button buttonDelete;
        private AntdUI.FlowPanel flowHeaderLeft;
        private AntdUI.Button buttonStatusTag;
        private AntdUI.Button buttonTypeTag;
        private AntdUI.Label labelTitle;
        private AntdUI.Panel panelBody;
        private AntdUI.Panel panelLeft;
        private AntdUI.Label labelCardId;
        private AntdUI.Label labelValueCardId;
        private AntdUI.Label labelDisplayName;
        private AntdUI.Label labelValueDisplayName;
        private AntdUI.Label labelName;
        private AntdUI.Label labelValueName;
        private AntdUI.Label labelDriverKey;
        private AntdUI.Label labelValueDriverKey;
        private AntdUI.Label labelStatus;
        private AntdUI.Label labelValueStatus;
        private AntdUI.Label labelUpdateTime;
        private AntdUI.Label labelValueUpdateTime;
        private AntdUI.Panel panelRight;
        private AntdUI.Label labelCore;
        private AntdUI.Label labelValueCore;
        private AntdUI.Label labelAxisCount;
        private AntdUI.Label labelValueAxisCount;
        private AntdUI.Label labelModeParam;
        private AntdUI.Label labelValueModeParam;
        private AntdUI.Label labelOpenConfig;
        private AntdUI.Label labelValueOpenConfig;
        private AntdUI.Label labelExtModule;
        private AntdUI.Label labelValueExtModule;
        private AntdUI.Label labelRemark;
        private AntdUI.Label labelValueRemark;
        private AntdUI.Label labelDescriptionTitle;
        private AntdUI.Label labelDescription;
        private AntdUI.Panel panelBottom;
        private AntdUI.FlowPanel flowBottomLeft;
        private AntdUI.FlowPanel flowBottomRight;
    }
}