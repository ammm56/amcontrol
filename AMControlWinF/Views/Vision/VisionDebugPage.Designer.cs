namespace AMControlWinF.Views.Vision
{
    partial class VisionDebugPage
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
            this.panelContent = new AntdUI.Panel();
            this.gridContent = new AntdUI.GridPanel();
            this.panelRightHost = new AntdUI.Panel();
            this.panelResultCard = new AntdUI.Panel();
            this.textResponseJson = new System.Windows.Forms.TextBox();
            this.panelResultHeader = new AntdUI.Panel();
            this.labelResultSummary = new AntdUI.Label();
            this.labelResultTitle = new AntdUI.Label();
            this.panelPreviewArea = new AntdUI.Panel();
            this.gridPreview = new AntdUI.GridPanel();
            this.panelInputPreviewCard = new AntdUI.Panel();
            this.inputImagePreview = new AMControlWinF.Views.Common.CameraImagePreviewControl();
            this.panelLivePreviewCard = new AntdUI.Panel();
            this.cameraLivePreview = new AMControlWinF.Views.Common.CameraImagePreviewControl();
            this.panelLivePreviewFooter = new AntdUI.Panel();
            this.flowCameraButtons = new AntdUI.FlowPanel();
            this.buttonCameraSettings = new AntdUI.Button();
            this.buttonGrabInput = new AntdUI.Button();
            this.buttonTogglePreview = new AntdUI.Button();
            this.buttonOpenCamera = new AntdUI.Button();
            this.panelOperationsCard = new AntdUI.Panel();
            this.panelOperationScroll = new System.Windows.Forms.Panel();
            this.flowOperations = new System.Windows.Forms.FlowLayoutPanel();
            this.panelOperationsHeader = new AntdUI.Panel();
            this.buttonStopContinuous = new AntdUI.Button();
            this.labelOperationsTitle = new AntdUI.Label();
            this.flowStats = new AntdUI.FlowPanel();
            this.panelLastElapsedCard = new AntdUI.Panel();
            this.labelLastElapsedValue = new AntdUI.Label();
            this.labelLastElapsedTitle = new AntdUI.Label();
            this.panelModelDeploymentCard = new AntdUI.Panel();
            this.labelModelDeploymentCount = new AntdUI.Label();
            this.labelModelDeploymentTitle = new AntdUI.Label();
            this.panelTriggerCard = new AntdUI.Panel();
            this.labelTriggerCount = new AntdUI.Label();
            this.labelTriggerTitle = new AntdUI.Label();
            this.panelRuntimeCard = new AntdUI.Panel();
            this.labelRuntimeCount = new AntdUI.Label();
            this.labelRuntimeTitle = new AntdUI.Label();
            this.panelOpenedCameraCard = new AntdUI.Panel();
            this.labelOpenedCameraCount = new AntdUI.Label();
            this.labelOpenedCameraTitle = new AntdUI.Label();
            this.panelToolbar = new AntdUI.Panel();
            this.flowToolbarRight = new AntdUI.FlowPanel();
            this.buttonRefreshConfig = new AntdUI.Button();
            this.flowToolbarLeft = new AntdUI.FlowPanel();
            this.labelStatus = new AntdUI.Label();
            this.selectExecutionMode = new AntdUI.Select();
            this.selectTrigger = new AntdUI.Select();
            this.selectRuntime = new AntdUI.Select();
            this.selectModelDeployment = new AntdUI.Select();
            this.selectCamera = new AntdUI.Select();
            this.labelTitle = new AntdUI.Label();
            this.panelRoot.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.gridContent.SuspendLayout();
            this.panelRightHost.SuspendLayout();
            this.panelResultCard.SuspendLayout();
            this.panelResultHeader.SuspendLayout();
            this.panelPreviewArea.SuspendLayout();
            this.gridPreview.SuspendLayout();
            this.panelInputPreviewCard.SuspendLayout();
            this.panelLivePreviewCard.SuspendLayout();
            this.panelLivePreviewFooter.SuspendLayout();
            this.flowCameraButtons.SuspendLayout();
            this.panelOperationsCard.SuspendLayout();
            this.panelOperationScroll.SuspendLayout();
            this.panelOperationsHeader.SuspendLayout();
            this.flowStats.SuspendLayout();
            this.panelLastElapsedCard.SuspendLayout();
            this.panelModelDeploymentCard.SuspendLayout();
            this.panelTriggerCard.SuspendLayout();
            this.panelRuntimeCard.SuspendLayout();
            this.panelOpenedCameraCard.SuspendLayout();
            this.panelToolbar.SuspendLayout();
            this.flowToolbarRight.SuspendLayout();
            this.flowToolbarLeft.SuspendLayout();
            this.SuspendLayout();
            //
            // panelRoot
            //
            this.panelRoot.Controls.Add(this.panelContent);
            this.panelRoot.Controls.Add(this.flowStats);
            this.panelRoot.Controls.Add(this.panelToolbar);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Margin = new System.Windows.Forms.Padding(0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(8);
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(900, 680);
            this.panelRoot.TabIndex = 0;
            //
            // panelContent
            //
            this.panelContent.BackColor = System.Drawing.Color.Transparent;
            this.panelContent.Controls.Add(this.gridContent);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(8, 134);
            this.panelContent.Margin = new System.Windows.Forms.Padding(0);
            this.panelContent.Name = "panelContent";
            this.panelContent.Radius = 0;
            this.panelContent.ShadowOpacity = 0F;
            this.panelContent.ShadowOpacityHover = 0F;
            this.panelContent.Size = new System.Drawing.Size(884, 538);
            this.panelContent.TabIndex = 2;
            //
            // gridContent
            //
            this.gridContent.Controls.Add(this.panelRightHost);
            this.gridContent.Controls.Add(this.panelOperationsCard);
            this.gridContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridContent.Location = new System.Drawing.Point(0, 0);
            this.gridContent.Margin = new System.Windows.Forms.Padding(0);
            this.gridContent.Name = "gridContent";
            this.gridContent.Size = new System.Drawing.Size(884, 538);
            this.gridContent.Span = "340 100%";
            this.gridContent.TabIndex = 0;
            //
            // panelRightHost
            //
            this.panelRightHost.BackColor = System.Drawing.Color.Transparent;
            this.panelRightHost.Controls.Add(this.panelResultCard);
            this.panelRightHost.Controls.Add(this.panelPreviewArea);
            this.panelRightHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRightHost.Location = new System.Drawing.Point(340, 0);
            this.panelRightHost.Margin = new System.Windows.Forms.Padding(0);
            this.panelRightHost.Name = "panelRightHost";
            this.panelRightHost.Radius = 0;
            this.panelRightHost.ShadowOpacity = 0F;
            this.panelRightHost.ShadowOpacityHover = 0F;
            this.panelRightHost.Size = new System.Drawing.Size(544, 538);
            this.panelRightHost.TabIndex = 1;
            //
            // panelResultCard
            //
            this.panelResultCard.BackColor = System.Drawing.Color.Transparent;
            this.panelResultCard.Controls.Add(this.textResponseJson);
            this.panelResultCard.Controls.Add(this.panelResultHeader);
            this.panelResultCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelResultCard.Location = new System.Drawing.Point(0, 292);
            this.panelResultCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelResultCard.Name = "panelResultCard";
            this.panelResultCard.Padding = new System.Windows.Forms.Padding(8);
            this.panelResultCard.Radius = 12;
            this.panelResultCard.Shadow = 4;
            this.panelResultCard.ShadowOpacity = 0.15F;
            this.panelResultCard.Size = new System.Drawing.Size(544, 246);
            this.panelResultCard.TabIndex = 2;
            //
            // textResponseJson
            //
            this.textResponseJson.BackColor = System.Drawing.Color.White;
            this.textResponseJson.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textResponseJson.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textResponseJson.Font = new System.Drawing.Font("Consolas", 9F);
            this.textResponseJson.Location = new System.Drawing.Point(12, 52);
            this.textResponseJson.Margin = new System.Windows.Forms.Padding(0);
            this.textResponseJson.Multiline = true;
            this.textResponseJson.Name = "textResponseJson";
            this.textResponseJson.ReadOnly = true;
            this.textResponseJson.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textResponseJson.Size = new System.Drawing.Size(520, 182);
            this.textResponseJson.TabIndex = 1;
            this.textResponseJson.WordWrap = false;
            //
            // panelResultHeader
            //
            this.panelResultHeader.Controls.Add(this.labelResultSummary);
            this.panelResultHeader.Controls.Add(this.labelResultTitle);
            this.panelResultHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelResultHeader.Location = new System.Drawing.Point(12, 12);
            this.panelResultHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelResultHeader.Name = "panelResultHeader";
            this.panelResultHeader.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.panelResultHeader.Radius = 0;
            this.panelResultHeader.Size = new System.Drawing.Size(520, 40);
            this.panelResultHeader.TabIndex = 0;
            //
            // labelResultSummary
            //
            this.labelResultSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelResultSummary.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelResultSummary.Location = new System.Drawing.Point(100, 0);
            this.labelResultSummary.Margin = new System.Windows.Forms.Padding(0);
            this.labelResultSummary.Name = "labelResultSummary";
            this.labelResultSummary.Size = new System.Drawing.Size(420, 32);
            this.labelResultSummary.TabIndex = 1;
            this.labelResultSummary.Text = "等待调用";
            //
            // labelResultTitle
            //
            this.labelResultTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelResultTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.labelResultTitle.Location = new System.Drawing.Point(0, 0);
            this.labelResultTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelResultTitle.Name = "labelResultTitle";
            this.labelResultTitle.Size = new System.Drawing.Size(100, 32);
            this.labelResultTitle.TabIndex = 0;
            this.labelResultTitle.Text = "原始 JSON";
            //
            // panelPreviewArea
            //
            this.panelPreviewArea.BackColor = System.Drawing.Color.Transparent;
            this.panelPreviewArea.Controls.Add(this.gridPreview);
            this.panelPreviewArea.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPreviewArea.Location = new System.Drawing.Point(0, 0);
            this.panelPreviewArea.Margin = new System.Windows.Forms.Padding(0);
            this.panelPreviewArea.Name = "panelPreviewArea";
            this.panelPreviewArea.Radius = 0;
            this.panelPreviewArea.ShadowOpacity = 0F;
            this.panelPreviewArea.ShadowOpacityHover = 0F;
            this.panelPreviewArea.Size = new System.Drawing.Size(544, 292);
            this.panelPreviewArea.TabIndex = 0;
            //
            // gridPreview
            //
            this.gridPreview.Controls.Add(this.panelInputPreviewCard);
            this.gridPreview.Controls.Add(this.panelLivePreviewCard);
            this.gridPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridPreview.Location = new System.Drawing.Point(0, 0);
            this.gridPreview.Margin = new System.Windows.Forms.Padding(0);
            this.gridPreview.Name = "gridPreview";
            this.gridPreview.Size = new System.Drawing.Size(544, 292);
            this.gridPreview.Span = "50% 50%";
            this.gridPreview.TabIndex = 0;
            //
            // panelInputPreviewCard
            //
            this.panelInputPreviewCard.BackColor = System.Drawing.Color.Transparent;
            this.panelInputPreviewCard.Controls.Add(this.inputImagePreview);
            this.panelInputPreviewCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelInputPreviewCard.Location = new System.Drawing.Point(272, 0);
            this.panelInputPreviewCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelInputPreviewCard.Name = "panelInputPreviewCard";
            this.panelInputPreviewCard.Padding = new System.Windows.Forms.Padding(8);
            this.panelInputPreviewCard.Radius = 12;
            this.panelInputPreviewCard.Shadow = 4;
            this.panelInputPreviewCard.ShadowOpacity = 0.15F;
            this.panelInputPreviewCard.Size = new System.Drawing.Size(272, 292);
            this.panelInputPreviewCard.TabIndex = 1;
            //
            // inputImagePreview
            //
            this.inputImagePreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inputImagePreview.Location = new System.Drawing.Point(12, 12);
            this.inputImagePreview.Margin = new System.Windows.Forms.Padding(0);
            this.inputImagePreview.Name = "inputImagePreview";
            this.inputImagePreview.Size = new System.Drawing.Size(248, 268);
            this.inputImagePreview.TabIndex = 0;
            //
            // panelLivePreviewCard
            //
            this.panelLivePreviewCard.BackColor = System.Drawing.Color.Transparent;
            this.panelLivePreviewCard.Controls.Add(this.cameraLivePreview);
            this.panelLivePreviewCard.Controls.Add(this.panelLivePreviewFooter);
            this.panelLivePreviewCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLivePreviewCard.Location = new System.Drawing.Point(0, 0);
            this.panelLivePreviewCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelLivePreviewCard.Name = "panelLivePreviewCard";
            this.panelLivePreviewCard.Padding = new System.Windows.Forms.Padding(8);
            this.panelLivePreviewCard.Radius = 12;
            this.panelLivePreviewCard.Shadow = 4;
            this.panelLivePreviewCard.ShadowOpacity = 0.15F;
            this.panelLivePreviewCard.Size = new System.Drawing.Size(272, 292);
            this.panelLivePreviewCard.TabIndex = 0;
            //
            // cameraLivePreview
            //
            this.cameraLivePreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cameraLivePreview.Location = new System.Drawing.Point(12, 12);
            this.cameraLivePreview.Margin = new System.Windows.Forms.Padding(0);
            this.cameraLivePreview.Name = "cameraLivePreview";
            this.cameraLivePreview.Size = new System.Drawing.Size(248, 216);
            this.cameraLivePreview.TabIndex = 0;
            //
            // panelLivePreviewFooter
            //
            this.panelLivePreviewFooter.Controls.Add(this.flowCameraButtons);
            this.panelLivePreviewFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelLivePreviewFooter.Location = new System.Drawing.Point(12, 228);
            this.panelLivePreviewFooter.Margin = new System.Windows.Forms.Padding(0);
            this.panelLivePreviewFooter.Name = "panelLivePreviewFooter";
            this.panelLivePreviewFooter.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.panelLivePreviewFooter.Radius = 0;
            this.panelLivePreviewFooter.Size = new System.Drawing.Size(248, 52);
            this.panelLivePreviewFooter.TabIndex = 1;
            //
            // flowCameraButtons
            //
            this.flowCameraButtons.Controls.Add(this.buttonCameraSettings);
            this.flowCameraButtons.Controls.Add(this.buttonGrabInput);
            this.flowCameraButtons.Controls.Add(this.buttonTogglePreview);
            this.flowCameraButtons.Controls.Add(this.buttonOpenCamera);
            this.flowCameraButtons.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowCameraButtons.Gap = 8;
            this.flowCameraButtons.Location = new System.Drawing.Point(0, 8);
            this.flowCameraButtons.Margin = new System.Windows.Forms.Padding(0);
            this.flowCameraButtons.Name = "flowCameraButtons";
            this.flowCameraButtons.Size = new System.Drawing.Size(368, 44);
            this.flowCameraButtons.TabIndex = 0;
            //
            // buttonCameraSettings
            //
            this.buttonCameraSettings.IconSvg = "SettingOutlined";
            this.buttonCameraSettings.Location = new System.Drawing.Point(280, 0);
            this.buttonCameraSettings.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCameraSettings.Name = "buttonCameraSettings";
            this.buttonCameraSettings.Radius = 8;
            this.buttonCameraSettings.Size = new System.Drawing.Size(78, 36);
            this.buttonCameraSettings.TabIndex = 3;
            this.buttonCameraSettings.Text = "设置";
            this.buttonCameraSettings.WaveSize = 0;
            //
            // buttonGrabInput
            //
            this.buttonGrabInput.IconSvg = "CameraOutlined";
            this.buttonGrabInput.Location = new System.Drawing.Point(194, 0);
            this.buttonGrabInput.Margin = new System.Windows.Forms.Padding(0);
            this.buttonGrabInput.Name = "buttonGrabInput";
            this.buttonGrabInput.Radius = 8;
            this.buttonGrabInput.Size = new System.Drawing.Size(78, 36);
            this.buttonGrabInput.TabIndex = 2;
            this.buttonGrabInput.Text = "取图";
            this.buttonGrabInput.WaveSize = 0;
            //
            // buttonTogglePreview
            //
            this.buttonTogglePreview.IconSvg = "VideoCameraOutlined";
            this.buttonTogglePreview.Location = new System.Drawing.Point(100, 0);
            this.buttonTogglePreview.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTogglePreview.Name = "buttonTogglePreview";
            this.buttonTogglePreview.Radius = 8;
            this.buttonTogglePreview.Size = new System.Drawing.Size(86, 36);
            this.buttonTogglePreview.TabIndex = 1;
            this.buttonTogglePreview.Text = "开始";
            this.buttonTogglePreview.Type = AntdUI.TTypeMini.Primary;
            this.buttonTogglePreview.WaveSize = 0;
            //
            // buttonOpenCamera
            //
            this.buttonOpenCamera.IconSvg = "PlayCircleOutlined";
            this.buttonOpenCamera.Location = new System.Drawing.Point(0, 0);
            this.buttonOpenCamera.Margin = new System.Windows.Forms.Padding(0);
            this.buttonOpenCamera.Name = "buttonOpenCamera";
            this.buttonOpenCamera.Radius = 8;
            this.buttonOpenCamera.Size = new System.Drawing.Size(92, 36);
            this.buttonOpenCamera.TabIndex = 0;
            this.buttonOpenCamera.Text = "打开";
            this.buttonOpenCamera.Type = AntdUI.TTypeMini.Primary;
            this.buttonOpenCamera.WaveSize = 0;
            //
            // panelOperationsCard
            //
            this.panelOperationsCard.BackColor = System.Drawing.Color.Transparent;
            this.panelOperationsCard.Controls.Add(this.panelOperationScroll);
            this.panelOperationsCard.Controls.Add(this.panelOperationsHeader);
            this.panelOperationsCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOperationsCard.Location = new System.Drawing.Point(0, 0);
            this.panelOperationsCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelOperationsCard.Name = "panelOperationsCard";
            this.panelOperationsCard.Padding = new System.Windows.Forms.Padding(8);
            this.panelOperationsCard.Radius = 12;
            this.panelOperationsCard.Shadow = 4;
            this.panelOperationsCard.ShadowOpacity = 0.15F;
            this.panelOperationsCard.Size = new System.Drawing.Size(340, 538);
            this.panelOperationsCard.TabIndex = 0;
            //
            // panelOperationScroll
            //
            this.panelOperationScroll.AutoScroll = true;
            this.panelOperationScroll.Controls.Add(this.flowOperations);
            this.panelOperationScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOperationScroll.Location = new System.Drawing.Point(12, 56);
            this.panelOperationScroll.Margin = new System.Windows.Forms.Padding(0);
            this.panelOperationScroll.Name = "panelOperationScroll";
            this.panelOperationScroll.Size = new System.Drawing.Size(316, 470);
            this.panelOperationScroll.TabIndex = 1;
            //
            // flowOperations
            //
            this.flowOperations.AutoSize = true;
            this.flowOperations.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowOperations.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowOperations.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowOperations.Location = new System.Drawing.Point(0, 0);
            this.flowOperations.Margin = new System.Windows.Forms.Padding(0);
            this.flowOperations.Name = "flowOperations";
            this.flowOperations.Size = new System.Drawing.Size(316, 0);
            this.flowOperations.TabIndex = 0;
            this.flowOperations.WrapContents = false;
            //
            // panelOperationsHeader
            //
            this.panelOperationsHeader.Controls.Add(this.buttonStopContinuous);
            this.panelOperationsHeader.Controls.Add(this.labelOperationsTitle);
            this.panelOperationsHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelOperationsHeader.Location = new System.Drawing.Point(12, 12);
            this.panelOperationsHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelOperationsHeader.Name = "panelOperationsHeader";
            this.panelOperationsHeader.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.panelOperationsHeader.Radius = 0;
            this.panelOperationsHeader.Size = new System.Drawing.Size(316, 44);
            this.panelOperationsHeader.TabIndex = 0;
            //
            // buttonStopContinuous
            //
            this.buttonStopContinuous.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonStopContinuous.IconSvg = "PauseCircleOutlined";
            this.buttonStopContinuous.Location = new System.Drawing.Point(214, 0);
            this.buttonStopContinuous.Margin = new System.Windows.Forms.Padding(0);
            this.buttonStopContinuous.Name = "buttonStopContinuous";
            this.buttonStopContinuous.Radius = 8;
            this.buttonStopContinuous.Size = new System.Drawing.Size(102, 36);
            this.buttonStopContinuous.TabIndex = 1;
            this.buttonStopContinuous.Text = "停止连续";
            this.buttonStopContinuous.WaveSize = 0;
            //
            // labelOperationsTitle
            //
            this.labelOperationsTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelOperationsTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.labelOperationsTitle.Location = new System.Drawing.Point(0, 0);
            this.labelOperationsTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelOperationsTitle.Name = "labelOperationsTitle";
            this.labelOperationsTitle.Size = new System.Drawing.Size(116, 36);
            this.labelOperationsTitle.TabIndex = 0;
            this.labelOperationsTitle.Text = "SDK 方法";
            //
            // flowStats
            //
            this.flowStats.Controls.Add(this.panelLastElapsedCard);
            this.flowStats.Controls.Add(this.panelModelDeploymentCard);
            this.flowStats.Controls.Add(this.panelTriggerCard);
            this.flowStats.Controls.Add(this.panelRuntimeCard);
            this.flowStats.Controls.Add(this.panelOpenedCameraCard);
            this.flowStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowStats.Gap = 6;
            this.flowStats.Location = new System.Drawing.Point(8, 52);
            this.flowStats.Margin = new System.Windows.Forms.Padding(0);
            this.flowStats.Name = "flowStats";
            this.flowStats.Padding = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.flowStats.Size = new System.Drawing.Size(884, 82);
            this.flowStats.TabIndex = 1;
            //
            // panelLastElapsedCard
            //
            this.panelLastElapsedCard.Controls.Add(this.labelLastElapsedValue);
            this.panelLastElapsedCard.Controls.Add(this.labelLastElapsedTitle);
            this.panelLastElapsedCard.Location = new System.Drawing.Point(588, 6);
            this.panelLastElapsedCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelLastElapsedCard.Name = "panelLastElapsedCard";
            this.panelLastElapsedCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelLastElapsedCard.Radius = 10;
            this.panelLastElapsedCard.Shadow = 4;
            this.panelLastElapsedCard.ShadowOpacity = 0.12F;
            this.panelLastElapsedCard.ShadowOpacityAnimation = true;
            this.panelLastElapsedCard.Size = new System.Drawing.Size(140, 66);
            this.panelLastElapsedCard.TabIndex = 4;
            //
            // labelLastElapsedValue
            //
            this.labelLastElapsedValue.BackColor = System.Drawing.Color.Transparent;
            this.labelLastElapsedValue.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelLastElapsedValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 16F, System.Drawing.FontStyle.Bold);
            this.labelLastElapsedValue.Location = new System.Drawing.Point(58, 16);
            this.labelLastElapsedValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelLastElapsedValue.Name = "labelLastElapsedValue";
            this.labelLastElapsedValue.Size = new System.Drawing.Size(66, 34);
            this.labelLastElapsedValue.TabIndex = 1;
            this.labelLastElapsedValue.Text = "--";
            this.labelLastElapsedValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // labelLastElapsedTitle
            //
            this.labelLastElapsedTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelLastElapsedTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelLastElapsedTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelLastElapsedTitle.Location = new System.Drawing.Point(16, 16);
            this.labelLastElapsedTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelLastElapsedTitle.Name = "labelLastElapsedTitle";
            this.labelLastElapsedTitle.Size = new System.Drawing.Size(37, 34);
            this.labelLastElapsedTitle.TabIndex = 0;
            this.labelLastElapsedTitle.Text = "用时";
            //
            // panelModelDeploymentCard
            //
            this.panelModelDeploymentCard.Controls.Add(this.labelModelDeploymentCount);
            this.panelModelDeploymentCard.Controls.Add(this.labelModelDeploymentTitle);
            this.panelModelDeploymentCard.Location = new System.Drawing.Point(442, 6);
            this.panelModelDeploymentCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelModelDeploymentCard.Name = "panelModelDeploymentCard";
            this.panelModelDeploymentCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelModelDeploymentCard.Radius = 10;
            this.panelModelDeploymentCard.Shadow = 4;
            this.panelModelDeploymentCard.ShadowOpacity = 0.12F;
            this.panelModelDeploymentCard.ShadowOpacityAnimation = true;
            this.panelModelDeploymentCard.Size = new System.Drawing.Size(140, 66);
            this.panelModelDeploymentCard.TabIndex = 3;
            //
            // labelModelDeploymentCount
            //
            this.labelModelDeploymentCount.BackColor = System.Drawing.Color.Transparent;
            this.labelModelDeploymentCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelModelDeploymentCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 16F, System.Drawing.FontStyle.Bold);
            this.labelModelDeploymentCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(85)))), ((int)(((byte)(255)))));
            this.labelModelDeploymentCount.Location = new System.Drawing.Point(74, 16);
            this.labelModelDeploymentCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelModelDeploymentCount.Name = "labelModelDeploymentCount";
            this.labelModelDeploymentCount.Size = new System.Drawing.Size(50, 34);
            this.labelModelDeploymentCount.TabIndex = 1;
            this.labelModelDeploymentCount.Text = "0";
            this.labelModelDeploymentCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // labelModelDeploymentTitle
            //
            this.labelModelDeploymentTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelModelDeploymentTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelModelDeploymentTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelModelDeploymentTitle.Location = new System.Drawing.Point(16, 16);
            this.labelModelDeploymentTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelModelDeploymentTitle.Name = "labelModelDeploymentTitle";
            this.labelModelDeploymentTitle.Size = new System.Drawing.Size(58, 34);
            this.labelModelDeploymentTitle.TabIndex = 0;
            this.labelModelDeploymentTitle.Text = "模型部署";
            //
            // panelTriggerCard
            //
            this.panelTriggerCard.Controls.Add(this.labelTriggerCount);
            this.panelTriggerCard.Controls.Add(this.labelTriggerTitle);
            this.panelTriggerCard.Location = new System.Drawing.Point(296, 6);
            this.panelTriggerCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelTriggerCard.Name = "panelTriggerCard";
            this.panelTriggerCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelTriggerCard.Radius = 10;
            this.panelTriggerCard.Shadow = 4;
            this.panelTriggerCard.ShadowOpacity = 0.12F;
            this.panelTriggerCard.ShadowOpacityAnimation = true;
            this.panelTriggerCard.Size = new System.Drawing.Size(140, 66);
            this.panelTriggerCard.TabIndex = 2;
            //
            // labelTriggerCount
            //
            this.labelTriggerCount.BackColor = System.Drawing.Color.Transparent;
            this.labelTriggerCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelTriggerCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 16F, System.Drawing.FontStyle.Bold);
            this.labelTriggerCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(180)))), ((int)(((byte)(60)))));
            this.labelTriggerCount.Location = new System.Drawing.Point(74, 16);
            this.labelTriggerCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelTriggerCount.Name = "labelTriggerCount";
            this.labelTriggerCount.Size = new System.Drawing.Size(50, 34);
            this.labelTriggerCount.TabIndex = 1;
            this.labelTriggerCount.Text = "0";
            this.labelTriggerCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // labelTriggerTitle
            //
            this.labelTriggerTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelTriggerTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelTriggerTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelTriggerTitle.Location = new System.Drawing.Point(16, 16);
            this.labelTriggerTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTriggerTitle.Name = "labelTriggerTitle";
            this.labelTriggerTitle.Size = new System.Drawing.Size(72, 34);
            this.labelTriggerTitle.TabIndex = 0;
            this.labelTriggerTitle.Text = "Trigger";
            //
            // panelRuntimeCard
            //
            this.panelRuntimeCard.Controls.Add(this.labelRuntimeCount);
            this.panelRuntimeCard.Controls.Add(this.labelRuntimeTitle);
            this.panelRuntimeCard.Location = new System.Drawing.Point(150, 6);
            this.panelRuntimeCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelRuntimeCard.Name = "panelRuntimeCard";
            this.panelRuntimeCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelRuntimeCard.Radius = 10;
            this.panelRuntimeCard.Shadow = 4;
            this.panelRuntimeCard.ShadowOpacity = 0.12F;
            this.panelRuntimeCard.ShadowOpacityAnimation = true;
            this.panelRuntimeCard.Size = new System.Drawing.Size(140, 66);
            this.panelRuntimeCard.TabIndex = 1;
            //
            // labelRuntimeCount
            //
            this.labelRuntimeCount.BackColor = System.Drawing.Color.Transparent;
            this.labelRuntimeCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelRuntimeCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 16F, System.Drawing.FontStyle.Bold);
            this.labelRuntimeCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(118)))), ((int)(((byte)(255)))));
            this.labelRuntimeCount.Location = new System.Drawing.Point(74, 16);
            this.labelRuntimeCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelRuntimeCount.Name = "labelRuntimeCount";
            this.labelRuntimeCount.Size = new System.Drawing.Size(50, 34);
            this.labelRuntimeCount.TabIndex = 1;
            this.labelRuntimeCount.Text = "0";
            this.labelRuntimeCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // labelRuntimeTitle
            //
            this.labelRuntimeTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelRuntimeTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelRuntimeTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelRuntimeTitle.Location = new System.Drawing.Point(16, 16);
            this.labelRuntimeTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelRuntimeTitle.Name = "labelRuntimeTitle";
            this.labelRuntimeTitle.Size = new System.Drawing.Size(72, 34);
            this.labelRuntimeTitle.TabIndex = 0;
            this.labelRuntimeTitle.Text = "Runtime";
            //
            // panelOpenedCameraCard
            //
            this.panelOpenedCameraCard.Controls.Add(this.labelOpenedCameraCount);
            this.panelOpenedCameraCard.Controls.Add(this.labelOpenedCameraTitle);
            this.panelOpenedCameraCard.Location = new System.Drawing.Point(4, 6);
            this.panelOpenedCameraCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelOpenedCameraCard.Name = "panelOpenedCameraCard";
            this.panelOpenedCameraCard.Padding = new System.Windows.Forms.Padding(12);
            this.panelOpenedCameraCard.Radius = 10;
            this.panelOpenedCameraCard.Shadow = 4;
            this.panelOpenedCameraCard.ShadowOpacity = 0.12F;
            this.panelOpenedCameraCard.ShadowOpacityAnimation = true;
            this.panelOpenedCameraCard.Size = new System.Drawing.Size(140, 66);
            this.panelOpenedCameraCard.TabIndex = 0;
            //
            // labelOpenedCameraCount
            //
            this.labelOpenedCameraCount.BackColor = System.Drawing.Color.Transparent;
            this.labelOpenedCameraCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelOpenedCameraCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 16F, System.Drawing.FontStyle.Bold);
            this.labelOpenedCameraCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(180)))), ((int)(((byte)(60)))));
            this.labelOpenedCameraCount.Location = new System.Drawing.Point(74, 16);
            this.labelOpenedCameraCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelOpenedCameraCount.Name = "labelOpenedCameraCount";
            this.labelOpenedCameraCount.Size = new System.Drawing.Size(50, 34);
            this.labelOpenedCameraCount.TabIndex = 1;
            this.labelOpenedCameraCount.Text = "0";
            this.labelOpenedCameraCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // labelOpenedCameraTitle
            //
            this.labelOpenedCameraTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelOpenedCameraTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelOpenedCameraTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelOpenedCameraTitle.Location = new System.Drawing.Point(16, 16);
            this.labelOpenedCameraTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelOpenedCameraTitle.Name = "labelOpenedCameraTitle";
            this.labelOpenedCameraTitle.Size = new System.Drawing.Size(72, 34);
            this.labelOpenedCameraTitle.TabIndex = 0;
            this.labelOpenedCameraTitle.Text = "已连接相机";
            //
            // panelToolbar
            //
            this.panelToolbar.Controls.Add(this.flowToolbarRight);
            this.panelToolbar.Controls.Add(this.flowToolbarLeft);
            this.panelToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelToolbar.Location = new System.Drawing.Point(8, 8);
            this.panelToolbar.Margin = new System.Windows.Forms.Padding(0);
            this.panelToolbar.Name = "panelToolbar";
            this.panelToolbar.Padding = new System.Windows.Forms.Padding(4);
            this.panelToolbar.Radius = 0;
            this.panelToolbar.Size = new System.Drawing.Size(884, 44);
            this.panelToolbar.TabIndex = 0;
            //
            // flowToolbarRight
            //
            this.flowToolbarRight.Align = AntdUI.TAlignFlow.RightCenter;
            this.flowToolbarRight.Controls.Add(this.buttonRefreshConfig);
            this.flowToolbarRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowToolbarRight.Gap = 8;
            this.flowToolbarRight.Location = new System.Drawing.Point(768, 4);
            this.flowToolbarRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowToolbarRight.Name = "flowToolbarRight";
            this.flowToolbarRight.Size = new System.Drawing.Size(112, 36);
            this.flowToolbarRight.TabIndex = 1;
            //
            // buttonRefreshConfig
            //
            this.buttonRefreshConfig.IconSvg = "ReloadOutlined";
            this.buttonRefreshConfig.Location = new System.Drawing.Point(24, 0);
            this.buttonRefreshConfig.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRefreshConfig.Name = "buttonRefreshConfig";
            this.buttonRefreshConfig.Radius = 8;
            this.buttonRefreshConfig.Size = new System.Drawing.Size(88, 36);
            this.buttonRefreshConfig.TabIndex = 0;
            this.buttonRefreshConfig.Text = "刷新配置";
            this.buttonRefreshConfig.Type = AntdUI.TTypeMini.Primary;
            this.buttonRefreshConfig.WaveSize = 0;
            //
            // flowToolbarLeft
            //
            this.flowToolbarLeft.Controls.Add(this.labelStatus);
            this.flowToolbarLeft.Controls.Add(this.selectExecutionMode);
            this.flowToolbarLeft.Controls.Add(this.selectTrigger);
            this.flowToolbarLeft.Controls.Add(this.selectRuntime);
            this.flowToolbarLeft.Controls.Add(this.selectModelDeployment);
            this.flowToolbarLeft.Controls.Add(this.selectCamera);
            this.flowToolbarLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowToolbarLeft.Gap = 8;
            this.flowToolbarLeft.Location = new System.Drawing.Point(4, 4);
            this.flowToolbarLeft.Margin = new System.Windows.Forms.Padding(0);
            this.flowToolbarLeft.Name = "flowToolbarLeft";
            this.flowToolbarLeft.Size = new System.Drawing.Size(876, 36);
            this.flowToolbarLeft.TabIndex = 0;
            //
            // labelStatus
            //
            this.labelStatus.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.labelStatus.Location = new System.Drawing.Point(730, 0);
            this.labelStatus.Margin = new System.Windows.Forms.Padding(0);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(124, 36);
            this.labelStatus.TabIndex = 5;
            this.labelStatus.Text = "等待加载";
            this.labelStatus.Visible = false;
            //
            // selectExecutionMode
            //
            this.selectExecutionMode.Location = new System.Drawing.Point(632, 0);
            this.selectExecutionMode.Margin = new System.Windows.Forms.Padding(0);
            this.selectExecutionMode.Name = "selectExecutionMode";
            this.selectExecutionMode.PlaceholderText = "调用方式";
            this.selectExecutionMode.Size = new System.Drawing.Size(90, 36);
            this.selectExecutionMode.TabIndex = 5;
            this.selectExecutionMode.WaveSize = 0;
            //
            // selectTrigger
            //
            this.selectTrigger.Location = new System.Drawing.Point(479, 0);
            this.selectTrigger.Margin = new System.Windows.Forms.Padding(0);
            this.selectTrigger.Name = "selectTrigger";
            this.selectTrigger.PlaceholderText = "Trigger Key";
            this.selectTrigger.Size = new System.Drawing.Size(145, 36);
            this.selectTrigger.TabIndex = 4;
            this.selectTrigger.WaveSize = 0;
            //
            // selectRuntime
            //
            this.selectRuntime.Location = new System.Drawing.Point(326, 0);
            this.selectRuntime.Margin = new System.Windows.Forms.Padding(0);
            this.selectRuntime.Name = "selectRuntime";
            this.selectRuntime.PlaceholderText = "Runtime Key";
            this.selectRuntime.Size = new System.Drawing.Size(145, 36);
            this.selectRuntime.TabIndex = 3;
            this.selectRuntime.WaveSize = 0;
            //
            // selectModelDeployment
            //
            this.selectModelDeployment.Location = new System.Drawing.Point(148, 0);
            this.selectModelDeployment.Margin = new System.Windows.Forms.Padding(0);
            this.selectModelDeployment.Name = "selectModelDeployment";
            this.selectModelDeployment.PlaceholderText = "Model Deployment";
            this.selectModelDeployment.Size = new System.Drawing.Size(170, 36);
            this.selectModelDeployment.TabIndex = 2;
            this.selectModelDeployment.WaveSize = 0;
            //
            // selectCamera
            //
            this.selectCamera.Location = new System.Drawing.Point(0, 0);
            this.selectCamera.Margin = new System.Windows.Forms.Padding(0);
            this.selectCamera.Name = "selectCamera";
            this.selectCamera.PlaceholderText = "选择相机";
            this.selectCamera.Size = new System.Drawing.Size(140, 36);
            this.selectCamera.TabIndex = 1;
            this.selectCamera.WaveSize = 0;
            //
            // labelTitle
            //
            this.labelTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(0, 0);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(84, 36);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "视觉调试";
            //
            // VisionDebugPage
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "VisionDebugPage";
            this.Size = new System.Drawing.Size(900, 680);
            this.panelRoot.ResumeLayout(false);
            this.panelContent.ResumeLayout(false);
            this.gridContent.ResumeLayout(false);
            this.panelRightHost.ResumeLayout(false);
            this.panelResultCard.ResumeLayout(false);
            this.panelResultCard.PerformLayout();
            this.panelResultHeader.ResumeLayout(false);
            this.panelPreviewArea.ResumeLayout(false);
            this.gridPreview.ResumeLayout(false);
            this.panelInputPreviewCard.ResumeLayout(false);
            this.panelLivePreviewCard.ResumeLayout(false);
            this.panelLivePreviewFooter.ResumeLayout(false);
            this.flowCameraButtons.ResumeLayout(false);
            this.panelOperationsCard.ResumeLayout(false);
            this.panelOperationScroll.ResumeLayout(false);
            this.panelOperationScroll.PerformLayout();
            this.panelOperationsHeader.ResumeLayout(false);
            this.flowStats.ResumeLayout(false);
            this.panelLastElapsedCard.ResumeLayout(false);
            this.panelModelDeploymentCard.ResumeLayout(false);
            this.panelTriggerCard.ResumeLayout(false);
            this.panelRuntimeCard.ResumeLayout(false);
            this.panelOpenedCameraCard.ResumeLayout(false);
            this.panelToolbar.ResumeLayout(false);
            this.flowToolbarRight.ResumeLayout(false);
            this.flowToolbarLeft.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AntdUI.Panel panelRoot;
        private AntdUI.Panel panelContent;
        private AntdUI.GridPanel gridContent;
        private AntdUI.Panel panelRightHost;
        private AntdUI.Panel panelOperationsCard;
        private System.Windows.Forms.Panel panelOperationScroll;
        private System.Windows.Forms.FlowLayoutPanel flowOperations;
        private AntdUI.Panel panelOperationsHeader;
        private AntdUI.Button buttonStopContinuous;
        private AntdUI.Label labelOperationsTitle;
        private AntdUI.Panel panelPreviewArea;
        private AntdUI.GridPanel gridPreview;
        private AntdUI.Panel panelInputPreviewCard;
        private Common.CameraImagePreviewControl inputImagePreview;
        private AntdUI.Panel panelLivePreviewCard;
        private Common.CameraImagePreviewControl cameraLivePreview;
        private AntdUI.Panel panelLivePreviewFooter;
        private AntdUI.FlowPanel flowCameraButtons;
        private AntdUI.Button buttonCameraSettings;
        private AntdUI.Button buttonGrabInput;
        private AntdUI.Button buttonTogglePreview;
        private AntdUI.Button buttonOpenCamera;
        private AntdUI.Panel panelResultCard;
        private System.Windows.Forms.TextBox textResponseJson;
        private AntdUI.Panel panelResultHeader;
        private AntdUI.Label labelResultSummary;
        private AntdUI.Label labelResultTitle;
        private AntdUI.FlowPanel flowStats;
        private AntdUI.Panel panelLastElapsedCard;
        private AntdUI.Label labelLastElapsedValue;
        private AntdUI.Label labelLastElapsedTitle;
        private AntdUI.Panel panelModelDeploymentCard;
        private AntdUI.Label labelModelDeploymentCount;
        private AntdUI.Label labelModelDeploymentTitle;
        private AntdUI.Panel panelTriggerCard;
        private AntdUI.Label labelTriggerCount;
        private AntdUI.Label labelTriggerTitle;
        private AntdUI.Panel panelRuntimeCard;
        private AntdUI.Label labelRuntimeCount;
        private AntdUI.Label labelRuntimeTitle;
        private AntdUI.Panel panelOpenedCameraCard;
        private AntdUI.Label labelOpenedCameraCount;
        private AntdUI.Label labelOpenedCameraTitle;
        private AntdUI.Panel panelToolbar;
        private AntdUI.FlowPanel flowToolbarRight;
        private AntdUI.Button buttonRefreshConfig;
        private AntdUI.FlowPanel flowToolbarLeft;
        private AntdUI.Label labelStatus;
        private AntdUI.Select selectExecutionMode;
        private AntdUI.Select selectModelDeployment;
        private AntdUI.Select selectTrigger;
        private AntdUI.Select selectRuntime;
        private AntdUI.Select selectCamera;
        private AntdUI.Label labelTitle;
    }
}
