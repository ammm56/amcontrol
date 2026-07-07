namespace AMControlWinF.Views.SysConfig
{
    partial class CameraConfigManagementPage
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                DisposeRuntimeResources();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelRoot = new AntdUI.Panel();
            this.panelContentCard = new AntdUI.Panel();
            this.gridContent = new AntdUI.GridPanel();
            this.panelPreviewCard = new AntdUI.Panel();
            this.panelPreviewViewport = new System.Windows.Forms.Panel();
            this.flowPreviewZoom = new AntdUI.FlowPanel();
            this.buttonPreviewZoomOut = new AntdUI.Button();
            this.buttonPreviewZoomIn = new AntdUI.Button();
            this.picturePreview = new System.Windows.Forms.PictureBox();
            this.panelPreviewFooter = new AntdUI.Panel();
            this.flowPreviewButtons = new AntdUI.FlowPanel();
            this.buttonCameraSettings = new AntdUI.Button();
            this.buttonGrabFrame = new AntdUI.Button();
            this.buttonTogglePreview = new AntdUI.Button();
            this.buttonOpenCamera = new AntdUI.Button();
            this.panelPreviewHeader = new AntdUI.Panel();
            this.labelPreviewSummary = new AntdUI.Label();
            this.labelPreviewTitle = new AntdUI.Label();
            this.panelCameraCard = new AntdUI.Panel();
            this.tableCameras = new AntdUI.Table();
            this.panelCameraHeader = new AntdUI.Panel();
            this.flowCameraActionsRight = new AntdUI.FlowPanel();
            this.buttonAddCamera = new AntdUI.Button();
            this.buttonEditCamera = new AntdUI.Button();
            this.buttonDeleteCamera = new AntdUI.Button();
            this.flowCameraActionsLeft = new AntdUI.FlowPanel();
            this.labelSelectedCamera = new AntdUI.Label();
            this.labelCameraTitle = new AntdUI.Label();
            this.flowStats = new AntdUI.FlowPanel();
            this.panelConnectedCard = new AntdUI.Panel();
            this.labelConnectedCount = new AntdUI.Label();
            this.labelConnectedTitle = new AntdUI.Label();
            this.panelCameraTotalCard = new AntdUI.Panel();
            this.labelCameraTotalCount = new AntdUI.Label();
            this.labelCameraTotalTitle = new AntdUI.Label();
            this.panelToolbar = new AntdUI.Panel();
            this.flowToolbarLeft = new AntdUI.FlowPanel();
            this.labelRuntimeSummary = new AntdUI.Label();
            this.inputSearch = new AntdUI.Input();
            this.flowToolbarRight = new AntdUI.FlowPanel();
            this.buttonRefresh = new AntdUI.Button();
            this.panelRoot.SuspendLayout();
            this.panelContentCard.SuspendLayout();
            this.gridContent.SuspendLayout();
            this.panelPreviewCard.SuspendLayout();
            this.panelPreviewViewport.SuspendLayout();
            this.flowPreviewZoom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picturePreview)).BeginInit();
            this.panelPreviewFooter.SuspendLayout();
            this.flowPreviewButtons.SuspendLayout();
            this.panelPreviewHeader.SuspendLayout();
            this.panelCameraCard.SuspendLayout();
            this.panelCameraHeader.SuspendLayout();
            this.flowCameraActionsRight.SuspendLayout();
            this.flowCameraActionsLeft.SuspendLayout();
            this.flowStats.SuspendLayout();
            this.panelConnectedCard.SuspendLayout();
            this.panelCameraTotalCard.SuspendLayout();
            this.panelToolbar.SuspendLayout();
            this.flowToolbarLeft.SuspendLayout();
            this.flowToolbarRight.SuspendLayout();
            this.SuspendLayout();
            //
            // panelRoot
            //
            this.panelRoot.Controls.Add(this.panelContentCard);
            this.panelRoot.Controls.Add(this.flowStats);
            this.panelRoot.Controls.Add(this.panelToolbar);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Margin = new System.Windows.Forms.Padding(0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(8);
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(1150, 680);
            this.panelRoot.TabIndex = 0;
            //
            // panelContentCard
            //
            this.panelContentCard.BackColor = System.Drawing.Color.Transparent;
            this.panelContentCard.Controls.Add(this.gridContent);
            this.panelContentCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContentCard.Location = new System.Drawing.Point(8, 140);
            this.panelContentCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelContentCard.Name = "panelContentCard";
            this.panelContentCard.Radius = 0;
            this.panelContentCard.ShadowOpacity = 0F;
            this.panelContentCard.ShadowOpacityHover = 0F;
            this.panelContentCard.Size = new System.Drawing.Size(1134, 532);
            this.panelContentCard.TabIndex = 2;
            //
            // gridContent
            //
            this.gridContent.Controls.Add(this.panelPreviewCard);
            this.gridContent.Controls.Add(this.panelCameraCard);
            this.gridContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridContent.Location = new System.Drawing.Point(0, 0);
            this.gridContent.Margin = new System.Windows.Forms.Padding(0);
            this.gridContent.Name = "gridContent";
            this.gridContent.Size = new System.Drawing.Size(1134, 532);
            this.gridContent.Span = "40% 60%";
            this.gridContent.TabIndex = 0;
            //
            // panelPreviewCard
            //
            this.panelPreviewCard.BackColor = System.Drawing.Color.Transparent;
            this.panelPreviewCard.Controls.Add(this.panelPreviewViewport);
            this.panelPreviewCard.Controls.Add(this.panelPreviewFooter);
            this.panelPreviewCard.Controls.Add(this.panelPreviewHeader);
            this.panelPreviewCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPreviewCard.Location = new System.Drawing.Point(454, 0);
            this.panelPreviewCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelPreviewCard.Name = "panelPreviewCard";
            this.panelPreviewCard.Padding = new System.Windows.Forms.Padding(8);
            this.panelPreviewCard.Radius = 12;
            this.panelPreviewCard.Shadow = 4;
            this.panelPreviewCard.ShadowOpacity = 0.15F;
            this.panelPreviewCard.Size = new System.Drawing.Size(680, 532);
            this.panelPreviewCard.TabIndex = 1;
            //
            // panelPreviewViewport
            //
            this.panelPreviewViewport.BackColor = System.Drawing.Color.FromArgb(22, 25, 32);
            this.panelPreviewViewport.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelPreviewViewport.Controls.Add(this.flowPreviewZoom);
            this.panelPreviewViewport.Controls.Add(this.picturePreview);
            this.panelPreviewViewport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPreviewViewport.Location = new System.Drawing.Point(12, 56);
            this.panelPreviewViewport.Margin = new System.Windows.Forms.Padding(0);
            this.panelPreviewViewport.Name = "panelPreviewViewport";
            this.panelPreviewViewport.Size = new System.Drawing.Size(656, 412);
            this.panelPreviewViewport.TabIndex = 1;
            //
            // flowPreviewZoom
            //
            this.flowPreviewZoom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom)));
            this.flowPreviewZoom.Controls.Add(this.buttonPreviewZoomOut);
            this.flowPreviewZoom.Controls.Add(this.buttonPreviewZoomIn);
            this.flowPreviewZoom.Gap = 4;
            this.flowPreviewZoom.Location = new System.Drawing.Point(578, 366);
            this.flowPreviewZoom.Margin = new System.Windows.Forms.Padding(0);
            this.flowPreviewZoom.Name = "flowPreviewZoom";
            this.flowPreviewZoom.Size = new System.Drawing.Size(70, 36);
            this.flowPreviewZoom.TabIndex = 2;
            //
            // buttonPreviewZoomOut
            //
            this.buttonPreviewZoomOut.IconSvg = "MinusOutlined";
            this.buttonPreviewZoomOut.Location = new System.Drawing.Point(4, 0);
            this.buttonPreviewZoomOut.Margin = new System.Windows.Forms.Padding(0);
            this.buttonPreviewZoomOut.Name = "buttonPreviewZoomOut";
            this.buttonPreviewZoomOut.Radius = 8;
            this.buttonPreviewZoomOut.Size = new System.Drawing.Size(30, 30);
            this.buttonPreviewZoomOut.TabIndex = 0;
            this.buttonPreviewZoomOut.WaveSize = 0;
            //
            // buttonPreviewZoomIn
            //
            this.buttonPreviewZoomIn.IconSvg = "PlusOutlined";
            this.buttonPreviewZoomIn.Location = new System.Drawing.Point(38, 0);
            this.buttonPreviewZoomIn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonPreviewZoomIn.Name = "buttonPreviewZoomIn";
            this.buttonPreviewZoomIn.Radius = 8;
            this.buttonPreviewZoomIn.Size = new System.Drawing.Size(30, 30);
            this.buttonPreviewZoomIn.TabIndex = 1;
            this.buttonPreviewZoomIn.WaveSize = 0;
            //
            // picturePreview
            //
            this.picturePreview.BackColor = System.Drawing.Color.FromArgb(22, 25, 32);
            this.picturePreview.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.picturePreview.Location = new System.Drawing.Point(0, 0);
            this.picturePreview.Margin = new System.Windows.Forms.Padding(0);
            this.picturePreview.Name = "picturePreview";
            this.picturePreview.Size = new System.Drawing.Size(640, 360);
            this.picturePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picturePreview.TabIndex = 1;
            this.picturePreview.TabStop = false;
            //
            // panelPreviewFooter
            //
            this.panelPreviewFooter.Controls.Add(this.flowPreviewButtons);
            this.panelPreviewFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelPreviewFooter.Location = new System.Drawing.Point(12, 468);
            this.panelPreviewFooter.Margin = new System.Windows.Forms.Padding(0);
            this.panelPreviewFooter.Name = "panelPreviewFooter";
            this.panelPreviewFooter.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.panelPreviewFooter.Radius = 0;
            this.panelPreviewFooter.Size = new System.Drawing.Size(656, 52);
            this.panelPreviewFooter.TabIndex = 2;
            //
            // flowPreviewButtons
            //
            this.flowPreviewButtons.Controls.Add(this.buttonCameraSettings);
            this.flowPreviewButtons.Controls.Add(this.buttonGrabFrame);
            this.flowPreviewButtons.Controls.Add(this.buttonTogglePreview);
            this.flowPreviewButtons.Controls.Add(this.buttonOpenCamera);
            this.flowPreviewButtons.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowPreviewButtons.Gap = 8;
            this.flowPreviewButtons.Location = new System.Drawing.Point(0, 8);
            this.flowPreviewButtons.Margin = new System.Windows.Forms.Padding(0);
            this.flowPreviewButtons.Name = "flowPreviewButtons";
            this.flowPreviewButtons.Size = new System.Drawing.Size(420, 44);
            this.flowPreviewButtons.TabIndex = 0;
            //
            // buttonCameraSettings
            //
            this.buttonCameraSettings.IconSvg = "SettingOutlined";
            this.buttonCameraSettings.Location = new System.Drawing.Point(294, 0);
            this.buttonCameraSettings.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraSettings.Name = "buttonCameraSettings";
            this.buttonCameraSettings.Radius = 8;
            this.buttonCameraSettings.Size = new System.Drawing.Size(98, 36);
            this.buttonCameraSettings.TabIndex = 3;
            this.buttonCameraSettings.Text = "设置页";
            this.buttonCameraSettings.WaveSize = 0;
            //
            // buttonGrabFrame
            //
            this.buttonGrabFrame.IconSvg = "CameraOutlined";
            this.buttonGrabFrame.Location = new System.Drawing.Point(204, 0);
            this.buttonGrabFrame.Margin = new System.Windows.Forms.Padding(0);
            this.buttonGrabFrame.Name = "buttonGrabFrame";
            this.buttonGrabFrame.Radius = 8;
            this.buttonGrabFrame.Size = new System.Drawing.Size(82, 36);
            this.buttonGrabFrame.TabIndex = 2;
            this.buttonGrabFrame.Text = "取图";
            this.buttonGrabFrame.WaveSize = 0;
            //
            // buttonTogglePreview
            //
            this.buttonTogglePreview.IconSvg = "VideoCameraOutlined";
            this.buttonTogglePreview.Location = new System.Drawing.Point(104, 0);
            this.buttonTogglePreview.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTogglePreview.Name = "buttonTogglePreview";
            this.buttonTogglePreview.Radius = 8;
            this.buttonTogglePreview.Size = new System.Drawing.Size(92, 36);
            this.buttonTogglePreview.TabIndex = 1;
            this.buttonTogglePreview.Text = "开始";
            this.buttonTogglePreview.Type = AntdUI.TTypeMini.Primary;
            this.buttonTogglePreview.WaveSize = 0;
            //
            // buttonOpenCamera
            //
            this.buttonOpenCamera.IconSvg = "PlayCircleOutlined";
            this.buttonOpenCamera.Location = new System.Drawing.Point(4, 0);
            this.buttonOpenCamera.Margin = new System.Windows.Forms.Padding(0);
            this.buttonOpenCamera.Name = "buttonOpenCamera";
            this.buttonOpenCamera.Radius = 8;
            this.buttonOpenCamera.Size = new System.Drawing.Size(92, 36);
            this.buttonOpenCamera.TabIndex = 0;
            this.buttonOpenCamera.Text = "打开";
            this.buttonOpenCamera.Type = AntdUI.TTypeMini.Primary;
            this.buttonOpenCamera.WaveSize = 0;
            //
            // panelPreviewHeader
            //
            this.panelPreviewHeader.Controls.Add(this.labelPreviewSummary);
            this.panelPreviewHeader.Controls.Add(this.labelPreviewTitle);
            this.panelPreviewHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPreviewHeader.Location = new System.Drawing.Point(12, 12);
            this.panelPreviewHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelPreviewHeader.Name = "panelPreviewHeader";
            this.panelPreviewHeader.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.panelPreviewHeader.Radius = 0;
            this.panelPreviewHeader.Size = new System.Drawing.Size(656, 44);
            this.panelPreviewHeader.TabIndex = 0;
            //
            // labelPreviewSummary
            //
            this.labelPreviewSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPreviewSummary.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelPreviewSummary.Location = new System.Drawing.Point(100, 0);
            this.labelPreviewSummary.Margin = new System.Windows.Forms.Padding(0);
            this.labelPreviewSummary.Name = "labelPreviewSummary";
            this.labelPreviewSummary.Size = new System.Drawing.Size(556, 36);
            this.labelPreviewSummary.TabIndex = 1;
            this.labelPreviewSummary.Text = "未打开相机";
            //
            // labelPreviewTitle
            //
            this.labelPreviewTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelPreviewTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.labelPreviewTitle.Location = new System.Drawing.Point(0, 0);
            this.labelPreviewTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelPreviewTitle.Name = "labelPreviewTitle";
            this.labelPreviewTitle.Size = new System.Drawing.Size(100, 36);
            this.labelPreviewTitle.TabIndex = 0;
            this.labelPreviewTitle.Text = "实时预览";
            //
            // panelCameraCard
            //
            this.panelCameraCard.BackColor = System.Drawing.Color.Transparent;
            this.panelCameraCard.Controls.Add(this.tableCameras);
            this.panelCameraCard.Controls.Add(this.panelCameraHeader);
            this.panelCameraCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCameraCard.Location = new System.Drawing.Point(0, 0);
            this.panelCameraCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelCameraCard.Name = "panelCameraCard";
            this.panelCameraCard.Padding = new System.Windows.Forms.Padding(8);
            this.panelCameraCard.Radius = 12;
            this.panelCameraCard.Shadow = 4;
            this.panelCameraCard.ShadowOpacity = 0.15F;
            this.panelCameraCard.Size = new System.Drawing.Size(454, 532);
            this.panelCameraCard.TabIndex = 0;
            //
            // tableCameras
            //
            this.tableCameras.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tableCameras.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableCameras.EmptyHeader = true;
            this.tableCameras.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.tableCameras.Gap = 8;
            this.tableCameras.Gaps = new System.Drawing.Size(8, 8);
            this.tableCameras.Location = new System.Drawing.Point(12, 56);
            this.tableCameras.Margin = new System.Windows.Forms.Padding(0);
            this.tableCameras.Name = "tableCameras";
            this.tableCameras.ShowTip = false;
            this.tableCameras.Size = new System.Drawing.Size(430, 464);
            this.tableCameras.TabIndex = 1;
            //
            // panelCameraHeader
            //
            this.panelCameraHeader.Controls.Add(this.flowCameraActionsRight);
            this.panelCameraHeader.Controls.Add(this.flowCameraActionsLeft);
            this.panelCameraHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCameraHeader.Location = new System.Drawing.Point(12, 12);
            this.panelCameraHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelCameraHeader.Name = "panelCameraHeader";
            this.panelCameraHeader.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.panelCameraHeader.Radius = 0;
            this.panelCameraHeader.Size = new System.Drawing.Size(430, 44);
            this.panelCameraHeader.TabIndex = 0;
            //
            // flowCameraActionsRight
            //
            this.flowCameraActionsRight.Align = AntdUI.TAlignFlow.RightCenter;
            this.flowCameraActionsRight.Controls.Add(this.buttonAddCamera);
            this.flowCameraActionsRight.Controls.Add(this.buttonEditCamera);
            this.flowCameraActionsRight.Controls.Add(this.buttonDeleteCamera);
            this.flowCameraActionsRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowCameraActionsRight.Gap = 8;
            this.flowCameraActionsRight.Location = new System.Drawing.Point(106, 0);
            this.flowCameraActionsRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowCameraActionsRight.Name = "flowCameraActionsRight";
            this.flowCameraActionsRight.Size = new System.Drawing.Size(324, 36);
            this.flowCameraActionsRight.TabIndex = 1;
            //
            // buttonAddCamera
            //
            this.buttonAddCamera.IconSvg = "PlusOutlined";
            this.buttonAddCamera.Location = new System.Drawing.Point(236, 0);
            this.buttonAddCamera.Margin = new System.Windows.Forms.Padding(0);
            this.buttonAddCamera.Name = "buttonAddCamera";
            this.buttonAddCamera.Radius = 8;
            this.buttonAddCamera.Size = new System.Drawing.Size(88, 36);
            this.buttonAddCamera.TabIndex = 0;
            this.buttonAddCamera.Text = "新增";
            this.buttonAddCamera.Type = AntdUI.TTypeMini.Primary;
            this.buttonAddCamera.WaveSize = 0;
            //
            // buttonEditCamera
            //
            this.buttonEditCamera.IconSvg = "EditOutlined";
            this.buttonEditCamera.Location = new System.Drawing.Point(146, 0);
            this.buttonEditCamera.Margin = new System.Windows.Forms.Padding(0);
            this.buttonEditCamera.Name = "buttonEditCamera";
            this.buttonEditCamera.Radius = 8;
            this.buttonEditCamera.Size = new System.Drawing.Size(82, 36);
            this.buttonEditCamera.TabIndex = 1;
            this.buttonEditCamera.Text = "编辑";
            this.buttonEditCamera.WaveSize = 0;
            //
            // buttonDeleteCamera
            //
            this.buttonDeleteCamera.IconSvg = "DeleteOutlined";
            this.buttonDeleteCamera.Location = new System.Drawing.Point(56, 0);
            this.buttonDeleteCamera.Margin = new System.Windows.Forms.Padding(0);
            this.buttonDeleteCamera.Name = "buttonDeleteCamera";
            this.buttonDeleteCamera.Radius = 8;
            this.buttonDeleteCamera.Size = new System.Drawing.Size(82, 36);
            this.buttonDeleteCamera.TabIndex = 2;
            this.buttonDeleteCamera.Text = "删除";
            this.buttonDeleteCamera.WaveSize = 0;
            //
            // flowCameraActionsLeft
            //
            this.flowCameraActionsLeft.Controls.Add(this.labelSelectedCamera);
            this.flowCameraActionsLeft.Controls.Add(this.labelCameraTitle);
            this.flowCameraActionsLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowCameraActionsLeft.Gap = 8;
            this.flowCameraActionsLeft.Location = new System.Drawing.Point(0, 0);
            this.flowCameraActionsLeft.Margin = new System.Windows.Forms.Padding(0);
            this.flowCameraActionsLeft.Name = "flowCameraActionsLeft";
            this.flowCameraActionsLeft.Size = new System.Drawing.Size(250, 36);
            this.flowCameraActionsLeft.TabIndex = 0;
            //
            // labelSelectedCamera
            //
            this.labelSelectedCamera.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelSelectedCamera.Location = new System.Drawing.Point(84, 0);
            this.labelSelectedCamera.Margin = new System.Windows.Forms.Padding(0);
            this.labelSelectedCamera.Name = "labelSelectedCamera";
            this.labelSelectedCamera.Size = new System.Drawing.Size(166, 36);
            this.labelSelectedCamera.TabIndex = 1;
            this.labelSelectedCamera.Text = "未选择";
            //
            // labelCameraTitle
            //
            this.labelCameraTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.labelCameraTitle.Location = new System.Drawing.Point(0, 0);
            this.labelCameraTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelCameraTitle.Name = "labelCameraTitle";
            this.labelCameraTitle.Size = new System.Drawing.Size(76, 36);
            this.labelCameraTitle.TabIndex = 0;
            this.labelCameraTitle.Text = "相机配置";
            //
            // flowStats
            //
            this.flowStats.Controls.Add(this.panelConnectedCard);
            this.flowStats.Controls.Add(this.panelCameraTotalCard);
            this.flowStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowStats.Gap = 8;
            this.flowStats.Location = new System.Drawing.Point(8, 52);
            this.flowStats.Margin = new System.Windows.Forms.Padding(0);
            this.flowStats.Name = "flowStats";
            this.flowStats.Padding = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.flowStats.Size = new System.Drawing.Size(1134, 88);
            this.flowStats.TabIndex = 1;
            //
            // panelConnectedCard
            //
            this.panelConnectedCard.Controls.Add(this.labelConnectedCount);
            this.panelConnectedCard.Controls.Add(this.labelConnectedTitle);
            this.panelConnectedCard.Location = new System.Drawing.Point(186, 6);
            this.panelConnectedCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelConnectedCard.Name = "panelConnectedCard";
            this.panelConnectedCard.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.panelConnectedCard.Radius = 12;
            this.panelConnectedCard.Shadow = 4;
            this.panelConnectedCard.ShadowOpacity = 0.12F;
            this.panelConnectedCard.Size = new System.Drawing.Size(174, 64);
            this.panelConnectedCard.TabIndex = 1;
            //
            // labelConnectedCount
            //
            this.labelConnectedCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelConnectedCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelConnectedCount.ForeColor = System.Drawing.Color.FromArgb(26, 180, 48);
            this.labelConnectedCount.Location = new System.Drawing.Point(98, 8);
            this.labelConnectedCount.Name = "labelConnectedCount";
            this.labelConnectedCount.Size = new System.Drawing.Size(64, 48);
            this.labelConnectedCount.TabIndex = 1;
            this.labelConnectedCount.Text = "0";
            this.labelConnectedCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // labelConnectedTitle
            //
            this.labelConnectedTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelConnectedTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelConnectedTitle.Location = new System.Drawing.Point(12, 8);
            this.labelConnectedTitle.Name = "labelConnectedTitle";
            this.labelConnectedTitle.Size = new System.Drawing.Size(86, 48);
            this.labelConnectedTitle.TabIndex = 0;
            this.labelConnectedTitle.Text = "连接数";
            //
            // panelCameraTotalCard
            //
            this.panelCameraTotalCard.Controls.Add(this.labelCameraTotalCount);
            this.panelCameraTotalCard.Controls.Add(this.labelCameraTotalTitle);
            this.panelCameraTotalCard.Location = new System.Drawing.Point(4, 6);
            this.panelCameraTotalCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelCameraTotalCard.Name = "panelCameraTotalCard";
            this.panelCameraTotalCard.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.panelCameraTotalCard.Radius = 12;
            this.panelCameraTotalCard.Shadow = 4;
            this.panelCameraTotalCard.ShadowOpacity = 0.12F;
            this.panelCameraTotalCard.Size = new System.Drawing.Size(174, 64);
            this.panelCameraTotalCard.TabIndex = 0;
            //
            // labelCameraTotalCount
            //
            this.labelCameraTotalCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCameraTotalCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelCameraTotalCount.ForeColor = System.Drawing.Color.FromArgb(35, 35, 35);
            this.labelCameraTotalCount.Location = new System.Drawing.Point(98, 8);
            this.labelCameraTotalCount.Name = "labelCameraTotalCount";
            this.labelCameraTotalCount.Size = new System.Drawing.Size(64, 48);
            this.labelCameraTotalCount.TabIndex = 1;
            this.labelCameraTotalCount.Text = "0";
            this.labelCameraTotalCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // labelCameraTotalTitle
            //
            this.labelCameraTotalTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelCameraTotalTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelCameraTotalTitle.Location = new System.Drawing.Point(12, 8);
            this.labelCameraTotalTitle.Name = "labelCameraTotalTitle";
            this.labelCameraTotalTitle.Size = new System.Drawing.Size(86, 48);
            this.labelCameraTotalTitle.TabIndex = 0;
            this.labelCameraTotalTitle.Text = "相机数";
            //
            // panelToolbar
            //
            this.panelToolbar.Controls.Add(this.flowToolbarLeft);
            this.panelToolbar.Controls.Add(this.flowToolbarRight);
            this.panelToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelToolbar.Location = new System.Drawing.Point(8, 8);
            this.panelToolbar.Margin = new System.Windows.Forms.Padding(0);
            this.panelToolbar.Name = "panelToolbar";
            this.panelToolbar.Padding = new System.Windows.Forms.Padding(4);
            this.panelToolbar.Radius = 0;
            this.panelToolbar.Size = new System.Drawing.Size(1134, 44);
            this.panelToolbar.TabIndex = 0;
            //
            // flowToolbarLeft
            //
            this.flowToolbarLeft.Align = AntdUI.TAlignFlow.Left;
            this.flowToolbarLeft.Controls.Add(this.labelRuntimeSummary);
            this.flowToolbarLeft.Controls.Add(this.inputSearch);
            this.flowToolbarLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowToolbarLeft.Gap = 8;
            this.flowToolbarLeft.Location = new System.Drawing.Point(4, 4);
            this.flowToolbarLeft.Margin = new System.Windows.Forms.Padding(0);
            this.flowToolbarLeft.Name = "flowToolbarLeft";
            this.flowToolbarLeft.Size = new System.Drawing.Size(430, 36);
            this.flowToolbarLeft.TabIndex = 0;
            //
            // labelRuntimeSummary
            //
            this.labelRuntimeSummary.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelRuntimeSummary.Location = new System.Drawing.Point(236, 0);
            this.labelRuntimeSummary.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.labelRuntimeSummary.Name = "labelRuntimeSummary";
            this.labelRuntimeSummary.Size = new System.Drawing.Size(190, 36);
            this.labelRuntimeSummary.TabIndex = 1;
            this.labelRuntimeSummary.Text = "相机运行态：待加载";
            //
            // inputSearch
            //
            this.inputSearch.Location = new System.Drawing.Point(0, 0);
            this.inputSearch.Margin = new System.Windows.Forms.Padding(0);
            this.inputSearch.Name = "inputSearch";
            this.inputSearch.PlaceholderText = "搜索相机名称 / 编码";
            this.inputSearch.Size = new System.Drawing.Size(220, 36);
            this.inputSearch.TabIndex = 0;
            this.inputSearch.WaveSize = 0;
            //
            // flowToolbarRight
            //
            this.flowToolbarRight.Align = AntdUI.TAlignFlow.RightCenter;
            this.flowToolbarRight.Controls.Add(this.buttonRefresh);
            this.flowToolbarRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowToolbarRight.Gap = 8;
            this.flowToolbarRight.Location = new System.Drawing.Point(1030, 4);
            this.flowToolbarRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowToolbarRight.Name = "flowToolbarRight";
            this.flowToolbarRight.Size = new System.Drawing.Size(100, 36);
            this.flowToolbarRight.TabIndex = 1;
            //
            // buttonRefresh
            //
            this.buttonRefresh.IconSvg = "ReloadOutlined";
            this.buttonRefresh.Location = new System.Drawing.Point(22, 0);
            this.buttonRefresh.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Radius = 8;
            this.buttonRefresh.Size = new System.Drawing.Size(78, 36);
            this.buttonRefresh.TabIndex = 0;
            this.buttonRefresh.Text = "刷新";
            this.buttonRefresh.WaveSize = 0;
            //
            // CameraConfigManagementPage
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "CameraConfigManagementPage";
            this.Size = new System.Drawing.Size(1150, 680);
            this.panelRoot.ResumeLayout(false);
            this.panelContentCard.ResumeLayout(false);
            this.gridContent.ResumeLayout(false);
            this.panelPreviewCard.ResumeLayout(false);
            this.flowPreviewZoom.ResumeLayout(false);
            this.panelPreviewViewport.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picturePreview)).EndInit();
            this.panelPreviewFooter.ResumeLayout(false);
            this.flowPreviewButtons.ResumeLayout(false);
            this.panelPreviewHeader.ResumeLayout(false);
            this.panelCameraCard.ResumeLayout(false);
            this.panelCameraHeader.ResumeLayout(false);
            this.flowCameraActionsRight.ResumeLayout(false);
            this.flowCameraActionsLeft.ResumeLayout(false);
            this.flowStats.ResumeLayout(false);
            this.panelConnectedCard.ResumeLayout(false);
            this.panelCameraTotalCard.ResumeLayout(false);
            this.panelToolbar.ResumeLayout(false);
            this.flowToolbarLeft.ResumeLayout(false);
            this.flowToolbarRight.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private AntdUI.Panel panelRoot;
        private AntdUI.Panel panelContentCard;
        private AntdUI.GridPanel gridContent;
        private AntdUI.Panel panelPreviewCard;
        private System.Windows.Forms.Panel panelPreviewViewport;
        private AntdUI.FlowPanel flowPreviewZoom;
        private AntdUI.Button buttonPreviewZoomOut;
        private AntdUI.Button buttonPreviewZoomIn;
        private System.Windows.Forms.PictureBox picturePreview;
        private AntdUI.Panel panelPreviewFooter;
        private AntdUI.FlowPanel flowPreviewButtons;
        private AntdUI.Button buttonOpenCamera;
        private AntdUI.Button buttonTogglePreview;
        private AntdUI.Button buttonGrabFrame;
        private AntdUI.Button buttonCameraSettings;
        private AntdUI.Panel panelPreviewHeader;
        private AntdUI.Label labelPreviewSummary;
        private AntdUI.Label labelPreviewTitle;
        private AntdUI.Panel panelCameraCard;
        private AntdUI.Table tableCameras;
        private AntdUI.Panel panelCameraHeader;
        private AntdUI.FlowPanel flowCameraActionsRight;
        private AntdUI.Button buttonAddCamera;
        private AntdUI.Button buttonEditCamera;
        private AntdUI.Button buttonDeleteCamera;
        private AntdUI.FlowPanel flowCameraActionsLeft;
        private AntdUI.Label labelSelectedCamera;
        private AntdUI.Label labelCameraTitle;
        private AntdUI.FlowPanel flowStats;
        private AntdUI.Panel panelConnectedCard;
        private AntdUI.Label labelConnectedCount;
        private AntdUI.Label labelConnectedTitle;
        private AntdUI.Panel panelCameraTotalCard;
        private AntdUI.Label labelCameraTotalCount;
        private AntdUI.Label labelCameraTotalTitle;
        private AntdUI.Panel panelToolbar;
        private AntdUI.FlowPanel flowToolbarLeft;
        private AntdUI.Label labelRuntimeSummary;
        private AntdUI.Input inputSearch;
        private AntdUI.FlowPanel flowToolbarRight;
        private AntdUI.Button buttonRefresh;
    }
}
