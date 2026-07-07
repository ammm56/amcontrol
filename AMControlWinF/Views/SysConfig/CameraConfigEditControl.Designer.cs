namespace AMControlWinF.Views.SysConfig
{
    partial class CameraConfigEditControl
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
            this.gridMainSections = new AntdUI.GridPanel();
            this.stackSectionRemark = new AntdUI.StackPanel();
            this.inputRemark = new AntdUI.Input();
            this.labelRemark = new AntdUI.Label();
            this.inputSaveImageDirectory = new AntdUI.Input();
            this.labelSaveImageDirectory = new AntdUI.Label();
            this.checkSaveImageEnabled = new AntdUI.Checkbox();
            this.labelSaveImageEnabled = new AntdUI.Label();
            this.labelSectionRemark = new AntdUI.Label();
            this.stackSectionEncode = new AntdUI.StackPanel();
            this.inputPreviewFps = new AntdUI.Input();
            this.labelPreviewFps = new AntdUI.Label();
            this.inputJpegQuality = new AntdUI.Input();
            this.labelJpegQuality = new AntdUI.Label();
            this.panelRowImageFormat = new AntdUI.Panel();
            this.selectImageFormat = new AntdUI.Select();
            this.labelImageFormat = new AntdUI.Label();
            this.inputGrabTimeoutMs = new AntdUI.Input();
            this.labelGrabTimeoutMs = new AntdUI.Label();
            this.labelSectionEncode = new AntdUI.Label();
            this.stackSectionCapture = new AntdUI.StackPanel();
            this.inputGain = new AntdUI.Input();
            this.labelGain = new AntdUI.Label();
            this.inputExposure = new AntdUI.Input();
            this.labelExposure = new AntdUI.Label();
            this.inputPixelFormat = new AntdUI.Input();
            this.labelPixelFormat = new AntdUI.Label();
            this.inputFps = new AntdUI.Input();
            this.labelFps = new AntdUI.Label();
            this.selectHeight = new AntdUI.Select();
            this.labelHeight = new AntdUI.Label();
            this.selectWidth = new AntdUI.Select();
            this.labelWidth = new AntdUI.Label();
            this.labelSectionCapture = new AntdUI.Label();
            this.stackSectionBasic = new AntdUI.StackPanel();
            this.inputFriendlyName = new AntdUI.Input();
            this.labelFriendlyName = new AntdUI.Label();
            this.inputDeviceIndex = new AntdUI.InputNumber();
            this.labelDeviceIndex = new AntdUI.Label();
            this.checkEnabled = new AntdUI.Checkbox();
            this.labelEnabled = new AntdUI.Label();
            this.panelRowDriverType = new AntdUI.Panel();
            this.selectDriverType = new AntdUI.Select();
            this.labelDriverType = new AntdUI.Label();
            this.inputCameraName = new AntdUI.Input();
            this.labelCameraName = new AntdUI.Label();
            this.inputCameraCode = new AntdUI.Input();
            this.labelCameraCode = new AntdUI.Label();
            this.labelSectionBasic = new AntdUI.Label();
            this.gridMainSections.SuspendLayout();
            this.stackSectionRemark.SuspendLayout();
            this.stackSectionEncode.SuspendLayout();
            this.panelRowImageFormat.SuspendLayout();
            this.stackSectionCapture.SuspendLayout();
            this.stackSectionBasic.SuspendLayout();
            this.panelRowDriverType.SuspendLayout();
            this.SuspendLayout();
            //
            // gridMainSections
            //
            this.gridMainSections.Controls.Add(this.stackSectionRemark);
            this.gridMainSections.Controls.Add(this.stackSectionEncode);
            this.gridMainSections.Controls.Add(this.stackSectionCapture);
            this.gridMainSections.Controls.Add(this.stackSectionBasic);
            this.gridMainSections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridMainSections.Location = new System.Drawing.Point(0, 0);
            this.gridMainSections.Margin = new System.Windows.Forms.Padding(0);
            this.gridMainSections.Name = "gridMainSections";
            this.gridMainSections.Size = new System.Drawing.Size(1016, 463);
            this.gridMainSections.Span = "25% 25% 25% 25%";
            this.gridMainSections.TabIndex = 0;
            this.gridMainSections.Text = "gridMainSections";
            //
            // stackSectionRemark
            //
            this.stackSectionRemark.Controls.Add(this.inputRemark);
            this.stackSectionRemark.Controls.Add(this.labelRemark);
            this.stackSectionRemark.Controls.Add(this.inputSaveImageDirectory);
            this.stackSectionRemark.Controls.Add(this.labelSaveImageDirectory);
            this.stackSectionRemark.Controls.Add(this.checkSaveImageEnabled);
            this.stackSectionRemark.Controls.Add(this.labelSaveImageEnabled);
            this.stackSectionRemark.Controls.Add(this.labelSectionRemark);
            this.stackSectionRemark.Gap = 4;
            this.stackSectionRemark.Location = new System.Drawing.Point(762, 0);
            this.stackSectionRemark.Margin = new System.Windows.Forms.Padding(0);
            this.stackSectionRemark.Name = "stackSectionRemark";
            this.stackSectionRemark.Padding = new System.Windows.Forms.Padding(4);
            this.stackSectionRemark.Size = new System.Drawing.Size(254, 463);
            this.stackSectionRemark.TabIndex = 3;
            this.stackSectionRemark.Text = "stackSectionRemark";
            this.stackSectionRemark.Vertical = true;
            //
            // inputRemark
            //
            this.inputRemark.Location = new System.Drawing.Point(4, 190);
            this.inputRemark.Margin = new System.Windows.Forms.Padding(0);
            this.inputRemark.Multiline = true;
            this.inputRemark.Name = "inputRemark";
            this.inputRemark.PlaceholderText = "请输入备注";
            this.inputRemark.Size = new System.Drawing.Size(246, 90);
            this.inputRemark.TabIndex = 6;
            this.inputRemark.WaveSize = 0;
            //
            // labelRemark
            //
            this.labelRemark.Location = new System.Drawing.Point(4, 164);
            this.labelRemark.Margin = new System.Windows.Forms.Padding(0);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(246, 22);
            this.labelRemark.TabIndex = 5;
            this.labelRemark.Text = "备注";
            //
            // inputSaveImageDirectory
            //
            this.inputSaveImageDirectory.Location = new System.Drawing.Point(4, 128);
            this.inputSaveImageDirectory.Margin = new System.Windows.Forms.Padding(0);
            this.inputSaveImageDirectory.Name = "inputSaveImageDirectory";
            this.inputSaveImageDirectory.PlaceholderText = "请输入图片保存目录";
            this.inputSaveImageDirectory.Size = new System.Drawing.Size(246, 32);
            this.inputSaveImageDirectory.TabIndex = 4;
            this.inputSaveImageDirectory.WaveSize = 0;
            //
            // labelSaveImageDirectory
            //
            this.labelSaveImageDirectory.Location = new System.Drawing.Point(4, 102);
            this.labelSaveImageDirectory.Margin = new System.Windows.Forms.Padding(0);
            this.labelSaveImageDirectory.Name = "labelSaveImageDirectory";
            this.labelSaveImageDirectory.Size = new System.Drawing.Size(246, 22);
            this.labelSaveImageDirectory.TabIndex = 3;
            this.labelSaveImageDirectory.Text = "图片保存目录";
            //
            // checkSaveImageEnabled
            //
            this.checkSaveImageEnabled.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkSaveImageEnabled.Location = new System.Drawing.Point(4, 64);
            this.checkSaveImageEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.checkSaveImageEnabled.Name = "checkSaveImageEnabled";
            this.checkSaveImageEnabled.Size = new System.Drawing.Size(86, 34);
            this.checkSaveImageEnabled.TabIndex = 2;
            this.checkSaveImageEnabled.Text = "取图保存";
            //
            // labelSaveImageEnabled
            //
            this.labelSaveImageEnabled.Location = new System.Drawing.Point(4, 38);
            this.labelSaveImageEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.labelSaveImageEnabled.Name = "labelSaveImageEnabled";
            this.labelSaveImageEnabled.Size = new System.Drawing.Size(246, 22);
            this.labelSaveImageEnabled.TabIndex = 1;
            this.labelSaveImageEnabled.Text = "取图保存";
            //
            // labelSectionRemark
            //
            this.labelSectionRemark.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionRemark.Location = new System.Drawing.Point(4, 4);
            this.labelSectionRemark.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionRemark.Name = "labelSectionRemark";
            this.labelSectionRemark.Size = new System.Drawing.Size(246, 26);
            this.labelSectionRemark.TabIndex = 0;
            this.labelSectionRemark.Text = "运行与说明";
            //
            // stackSectionEncode
            //
            this.stackSectionEncode.Controls.Add(this.inputPreviewFps);
            this.stackSectionEncode.Controls.Add(this.labelPreviewFps);
            this.stackSectionEncode.Controls.Add(this.inputJpegQuality);
            this.stackSectionEncode.Controls.Add(this.labelJpegQuality);
            this.stackSectionEncode.Controls.Add(this.panelRowImageFormat);
            this.stackSectionEncode.Controls.Add(this.inputGrabTimeoutMs);
            this.stackSectionEncode.Controls.Add(this.labelGrabTimeoutMs);
            this.stackSectionEncode.Controls.Add(this.labelSectionEncode);
            this.stackSectionEncode.Gap = 4;
            this.stackSectionEncode.Location = new System.Drawing.Point(508, 0);
            this.stackSectionEncode.Margin = new System.Windows.Forms.Padding(0);
            this.stackSectionEncode.Name = "stackSectionEncode";
            this.stackSectionEncode.Padding = new System.Windows.Forms.Padding(4);
            this.stackSectionEncode.Size = new System.Drawing.Size(254, 463);
            this.stackSectionEncode.TabIndex = 2;
            this.stackSectionEncode.Text = "stackSectionEncode";
            this.stackSectionEncode.Vertical = true;
            //
            // inputPreviewFps
            //
            this.inputPreviewFps.Location = new System.Drawing.Point(4, 222);
            this.inputPreviewFps.Margin = new System.Windows.Forms.Padding(0);
            this.inputPreviewFps.Name = "inputPreviewFps";
            this.inputPreviewFps.PlaceholderText = "请输入预览 FPS";
            this.inputPreviewFps.Size = new System.Drawing.Size(246, 32);
            this.inputPreviewFps.TabIndex = 7;
            this.inputPreviewFps.WaveSize = 0;
            //
            // labelPreviewFps
            //
            this.labelPreviewFps.Location = new System.Drawing.Point(4, 196);
            this.labelPreviewFps.Margin = new System.Windows.Forms.Padding(0);
            this.labelPreviewFps.Name = "labelPreviewFps";
            this.labelPreviewFps.Size = new System.Drawing.Size(246, 22);
            this.labelPreviewFps.TabIndex = 6;
            this.labelPreviewFps.Text = "预览FPS";
            //
            // inputJpegQuality
            //
            this.inputJpegQuality.Location = new System.Drawing.Point(4, 160);
            this.inputJpegQuality.Margin = new System.Windows.Forms.Padding(0);
            this.inputJpegQuality.Name = "inputJpegQuality";
            this.inputJpegQuality.PlaceholderText = "请输入保存/调用 JPEG 质量";
            this.inputJpegQuality.Size = new System.Drawing.Size(246, 32);
            this.inputJpegQuality.TabIndex = 5;
            this.inputJpegQuality.WaveSize = 0;
            //
            // labelJpegQuality
            //
            this.labelJpegQuality.Location = new System.Drawing.Point(4, 134);
            this.labelJpegQuality.Margin = new System.Windows.Forms.Padding(0);
            this.labelJpegQuality.Name = "labelJpegQuality";
            this.labelJpegQuality.Size = new System.Drawing.Size(246, 22);
            this.labelJpegQuality.TabIndex = 4;
            this.labelJpegQuality.Text = "保存/调用 JPEG 质量";
            //
            // panelRowImageFormat
            //
            this.panelRowImageFormat.Controls.Add(this.selectImageFormat);
            this.panelRowImageFormat.Controls.Add(this.labelImageFormat);
            this.panelRowImageFormat.Location = new System.Drawing.Point(4, 78);
            this.panelRowImageFormat.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowImageFormat.Name = "panelRowImageFormat";
            this.panelRowImageFormat.Radius = 0;
            this.panelRowImageFormat.Size = new System.Drawing.Size(246, 52);
            this.panelRowImageFormat.TabIndex = 3;
            //
            // selectImageFormat
            //
            this.selectImageFormat.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.selectImageFormat.Location = new System.Drawing.Point(0, 20);
            this.selectImageFormat.Margin = new System.Windows.Forms.Padding(0);
            this.selectImageFormat.Name = "selectImageFormat";
            this.selectImageFormat.Size = new System.Drawing.Size(246, 32);
            this.selectImageFormat.TabIndex = 1;
            this.selectImageFormat.WaveSize = 0;
            //
            // labelImageFormat
            //
            this.labelImageFormat.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelImageFormat.Location = new System.Drawing.Point(0, 0);
            this.labelImageFormat.Margin = new System.Windows.Forms.Padding(0);
            this.labelImageFormat.Name = "labelImageFormat";
            this.labelImageFormat.Size = new System.Drawing.Size(246, 22);
            this.labelImageFormat.TabIndex = 0;
            this.labelImageFormat.Text = "保存/调用图片格式";
            //
            // inputGrabTimeoutMs
            //
            this.inputGrabTimeoutMs.Location = new System.Drawing.Point(4, 42);
            this.inputGrabTimeoutMs.Margin = new System.Windows.Forms.Padding(0);
            this.inputGrabTimeoutMs.Name = "inputGrabTimeoutMs";
            this.inputGrabTimeoutMs.PlaceholderText = "请输入取图超时";
            this.inputGrabTimeoutMs.Size = new System.Drawing.Size(246, 32);
            this.inputGrabTimeoutMs.TabIndex = 2;
            this.inputGrabTimeoutMs.WaveSize = 0;
            //
            // labelGrabTimeoutMs
            //
            this.labelGrabTimeoutMs.Location = new System.Drawing.Point(4, 30);
            this.labelGrabTimeoutMs.Margin = new System.Windows.Forms.Padding(0);
            this.labelGrabTimeoutMs.Name = "labelGrabTimeoutMs";
            this.labelGrabTimeoutMs.Size = new System.Drawing.Size(246, 22);
            this.labelGrabTimeoutMs.TabIndex = 1;
            this.labelGrabTimeoutMs.Text = "取图超时(ms)";
            //
            // labelSectionEncode
            //
            this.labelSectionEncode.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionEncode.Location = new System.Drawing.Point(4, 4);
            this.labelSectionEncode.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionEncode.Name = "labelSectionEncode";
            this.labelSectionEncode.Size = new System.Drawing.Size(246, 26);
            this.labelSectionEncode.TabIndex = 0;
            this.labelSectionEncode.Text = "保存/视觉调用编码";
            //
            // stackSectionCapture
            //
            this.stackSectionCapture.Controls.Add(this.inputGain);
            this.stackSectionCapture.Controls.Add(this.labelGain);
            this.stackSectionCapture.Controls.Add(this.inputExposure);
            this.stackSectionCapture.Controls.Add(this.labelExposure);
            this.stackSectionCapture.Controls.Add(this.inputPixelFormat);
            this.stackSectionCapture.Controls.Add(this.labelPixelFormat);
            this.stackSectionCapture.Controls.Add(this.inputFps);
            this.stackSectionCapture.Controls.Add(this.labelFps);
            this.stackSectionCapture.Controls.Add(this.selectHeight);
            this.stackSectionCapture.Controls.Add(this.labelHeight);
            this.stackSectionCapture.Controls.Add(this.selectWidth);
            this.stackSectionCapture.Controls.Add(this.labelWidth);
            this.stackSectionCapture.Controls.Add(this.labelSectionCapture);
            this.stackSectionCapture.Gap = 4;
            this.stackSectionCapture.Location = new System.Drawing.Point(254, 0);
            this.stackSectionCapture.Margin = new System.Windows.Forms.Padding(0);
            this.stackSectionCapture.Name = "stackSectionCapture";
            this.stackSectionCapture.Padding = new System.Windows.Forms.Padding(4);
            this.stackSectionCapture.Size = new System.Drawing.Size(254, 463);
            this.stackSectionCapture.TabIndex = 1;
            this.stackSectionCapture.Text = "stackSectionCapture";
            this.stackSectionCapture.Vertical = true;
            //
            // inputGain
            //
            this.inputGain.Location = new System.Drawing.Point(4, 352);
            this.inputGain.Margin = new System.Windows.Forms.Padding(0);
            this.inputGain.Name = "inputGain";
            this.inputGain.PlaceholderText = "留空使用驱动默认值";
            this.inputGain.Size = new System.Drawing.Size(246, 32);
            this.inputGain.TabIndex = 12;
            this.inputGain.WaveSize = 0;
            //
            // labelGain
            //
            this.labelGain.Location = new System.Drawing.Point(4, 326);
            this.labelGain.Margin = new System.Windows.Forms.Padding(0);
            this.labelGain.Name = "labelGain";
            this.labelGain.Size = new System.Drawing.Size(246, 22);
            this.labelGain.TabIndex = 11;
            this.labelGain.Text = "增益";
            //
            // inputExposure
            //
            this.inputExposure.Location = new System.Drawing.Point(4, 290);
            this.inputExposure.Margin = new System.Windows.Forms.Padding(0);
            this.inputExposure.Name = "inputExposure";
            this.inputExposure.PlaceholderText = "留空使用驱动默认值";
            this.inputExposure.Size = new System.Drawing.Size(246, 32);
            this.inputExposure.TabIndex = 10;
            this.inputExposure.WaveSize = 0;
            //
            // labelExposure
            //
            this.labelExposure.Location = new System.Drawing.Point(4, 264);
            this.labelExposure.Margin = new System.Windows.Forms.Padding(0);
            this.labelExposure.Name = "labelExposure";
            this.labelExposure.Size = new System.Drawing.Size(246, 22);
            this.labelExposure.TabIndex = 9;
            this.labelExposure.Text = "曝光";
            //
            // inputPixelFormat
            //
            this.inputPixelFormat.Location = new System.Drawing.Point(4, 228);
            this.inputPixelFormat.Margin = new System.Windows.Forms.Padding(0);
            this.inputPixelFormat.Name = "inputPixelFormat";
            this.inputPixelFormat.PlaceholderText = "例如 MJPG";
            this.inputPixelFormat.Size = new System.Drawing.Size(246, 32);
            this.inputPixelFormat.TabIndex = 8;
            this.inputPixelFormat.WaveSize = 0;
            //
            // labelPixelFormat
            //
            this.labelPixelFormat.Location = new System.Drawing.Point(4, 202);
            this.labelPixelFormat.Margin = new System.Windows.Forms.Padding(0);
            this.labelPixelFormat.Name = "labelPixelFormat";
            this.labelPixelFormat.Size = new System.Drawing.Size(246, 22);
            this.labelPixelFormat.TabIndex = 7;
            this.labelPixelFormat.Text = "FourCC";
            //
            // inputFps
            //
            this.inputFps.Location = new System.Drawing.Point(4, 166);
            this.inputFps.Margin = new System.Windows.Forms.Padding(0);
            this.inputFps.Name = "inputFps";
            this.inputFps.PlaceholderText = "请输入 FPS";
            this.inputFps.Size = new System.Drawing.Size(246, 32);
            this.inputFps.TabIndex = 6;
            this.inputFps.WaveSize = 0;
            //
            // labelFps
            //
            this.labelFps.Location = new System.Drawing.Point(4, 140);
            this.labelFps.Margin = new System.Windows.Forms.Padding(0);
            this.labelFps.Name = "labelFps";
            this.labelFps.Size = new System.Drawing.Size(246, 22);
            this.labelFps.TabIndex = 5;
            this.labelFps.Text = "FPS";
            //
            // selectHeight
            //
            this.selectHeight.Location = new System.Drawing.Point(4, 104);
            this.selectHeight.Margin = new System.Windows.Forms.Padding(0);
            this.selectHeight.Name = "selectHeight";
            this.selectHeight.PlaceholderText = "请选择高度";
            this.selectHeight.Size = new System.Drawing.Size(246, 32);
            this.selectHeight.TabIndex = 4;
            this.selectHeight.WaveSize = 0;
            //
            // labelHeight
            //
            this.labelHeight.Location = new System.Drawing.Point(4, 78);
            this.labelHeight.Margin = new System.Windows.Forms.Padding(0);
            this.labelHeight.Name = "labelHeight";
            this.labelHeight.Size = new System.Drawing.Size(246, 22);
            this.labelHeight.TabIndex = 3;
            this.labelHeight.Text = "高度";
            //
            // selectWidth
            //
            this.selectWidth.Location = new System.Drawing.Point(4, 42);
            this.selectWidth.Margin = new System.Windows.Forms.Padding(0);
            this.selectWidth.Name = "selectWidth";
            this.selectWidth.PlaceholderText = "请选择宽度";
            this.selectWidth.Size = new System.Drawing.Size(246, 32);
            this.selectWidth.TabIndex = 2;
            this.selectWidth.WaveSize = 0;
            //
            // labelWidth
            //
            this.labelWidth.Location = new System.Drawing.Point(4, 30);
            this.labelWidth.Margin = new System.Windows.Forms.Padding(0);
            this.labelWidth.Name = "labelWidth";
            this.labelWidth.Size = new System.Drawing.Size(246, 22);
            this.labelWidth.TabIndex = 1;
            this.labelWidth.Text = "宽度";
            //
            // labelSectionCapture
            //
            this.labelSectionCapture.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionCapture.Location = new System.Drawing.Point(4, 4);
            this.labelSectionCapture.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionCapture.Name = "labelSectionCapture";
            this.labelSectionCapture.Size = new System.Drawing.Size(246, 26);
            this.labelSectionCapture.TabIndex = 0;
            this.labelSectionCapture.Text = "采集参数";
            //
            // stackSectionBasic
            //
            this.stackSectionBasic.Controls.Add(this.inputFriendlyName);
            this.stackSectionBasic.Controls.Add(this.labelFriendlyName);
            this.stackSectionBasic.Controls.Add(this.inputDeviceIndex);
            this.stackSectionBasic.Controls.Add(this.labelDeviceIndex);
            this.stackSectionBasic.Controls.Add(this.checkEnabled);
            this.stackSectionBasic.Controls.Add(this.labelEnabled);
            this.stackSectionBasic.Controls.Add(this.panelRowDriverType);
            this.stackSectionBasic.Controls.Add(this.inputCameraName);
            this.stackSectionBasic.Controls.Add(this.labelCameraName);
            this.stackSectionBasic.Controls.Add(this.inputCameraCode);
            this.stackSectionBasic.Controls.Add(this.labelCameraCode);
            this.stackSectionBasic.Controls.Add(this.labelSectionBasic);
            this.stackSectionBasic.Gap = 4;
            this.stackSectionBasic.Location = new System.Drawing.Point(0, 0);
            this.stackSectionBasic.Margin = new System.Windows.Forms.Padding(0);
            this.stackSectionBasic.Name = "stackSectionBasic";
            this.stackSectionBasic.Padding = new System.Windows.Forms.Padding(4);
            this.stackSectionBasic.Size = new System.Drawing.Size(254, 463);
            this.stackSectionBasic.TabIndex = 0;
            this.stackSectionBasic.Text = "stackSectionBasic";
            this.stackSectionBasic.Vertical = true;
            //
            // inputFriendlyName
            //
            this.inputFriendlyName.Location = new System.Drawing.Point(4, 282);
            this.inputFriendlyName.Margin = new System.Windows.Forms.Padding(0);
            this.inputFriendlyName.Name = "inputFriendlyName";
            this.inputFriendlyName.PlaceholderText = "请输入系统枚举名称";
            this.inputFriendlyName.Size = new System.Drawing.Size(246, 32);
            this.inputFriendlyName.TabIndex = 11;
            this.inputFriendlyName.WaveSize = 0;
            //
            // labelFriendlyName
            //
            this.labelFriendlyName.Location = new System.Drawing.Point(4, 256);
            this.labelFriendlyName.Margin = new System.Windows.Forms.Padding(0);
            this.labelFriendlyName.Name = "labelFriendlyName";
            this.labelFriendlyName.Size = new System.Drawing.Size(246, 22);
            this.labelFriendlyName.TabIndex = 10;
            this.labelFriendlyName.Text = "枚举名称";
            //
            // inputDeviceIndex
            //
            this.inputDeviceIndex.Location = new System.Drawing.Point(4, 220);
            this.inputDeviceIndex.Margin = new System.Windows.Forms.Padding(0);
            this.inputDeviceIndex.Name = "inputDeviceIndex";
            this.inputDeviceIndex.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.inputDeviceIndex.PlaceholderText = "设备索引从 0 开始";
            this.inputDeviceIndex.Size = new System.Drawing.Size(246, 32);
            this.inputDeviceIndex.TabIndex = 9;
            this.inputDeviceIndex.WaveSize = 0;
            //
            // labelDeviceIndex
            //
            this.labelDeviceIndex.Location = new System.Drawing.Point(4, 194);
            this.labelDeviceIndex.Margin = new System.Windows.Forms.Padding(0);
            this.labelDeviceIndex.Name = "labelDeviceIndex";
            this.labelDeviceIndex.Size = new System.Drawing.Size(246, 22);
            this.labelDeviceIndex.TabIndex = 8;
            this.labelDeviceIndex.Text = "设备索引";
            //
            // checkEnabled
            //
            this.checkEnabled.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.checkEnabled.Checked = true;
            this.checkEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkEnabled.Location = new System.Drawing.Point(4, 156);
            this.checkEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.checkEnabled.Name = "checkEnabled";
            this.checkEnabled.Size = new System.Drawing.Size(86, 34);
            this.checkEnabled.TabIndex = 7;
            this.checkEnabled.Text = "启用相机";
            //
            // labelEnabled
            //
            this.labelEnabled.Location = new System.Drawing.Point(4, 130);
            this.labelEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.labelEnabled.Name = "labelEnabled";
            this.labelEnabled.Size = new System.Drawing.Size(246, 22);
            this.labelEnabled.TabIndex = 6;
            this.labelEnabled.Text = "启用状态";
            //
            // panelRowDriverType
            //
            this.panelRowDriverType.Controls.Add(this.selectDriverType);
            this.panelRowDriverType.Controls.Add(this.labelDriverType);
            this.panelRowDriverType.Location = new System.Drawing.Point(4, 74);
            this.panelRowDriverType.Margin = new System.Windows.Forms.Padding(0);
            this.panelRowDriverType.Name = "panelRowDriverType";
            this.panelRowDriverType.Radius = 0;
            this.panelRowDriverType.Size = new System.Drawing.Size(246, 52);
            this.panelRowDriverType.TabIndex = 5;
            //
            // selectDriverType
            //
            this.selectDriverType.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.selectDriverType.Location = new System.Drawing.Point(0, 20);
            this.selectDriverType.Margin = new System.Windows.Forms.Padding(0);
            this.selectDriverType.Name = "selectDriverType";
            this.selectDriverType.Size = new System.Drawing.Size(246, 32);
            this.selectDriverType.TabIndex = 1;
            this.selectDriverType.WaveSize = 0;
            //
            // labelDriverType
            //
            this.labelDriverType.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDriverType.Location = new System.Drawing.Point(0, 0);
            this.labelDriverType.Margin = new System.Windows.Forms.Padding(0);
            this.labelDriverType.Name = "labelDriverType";
            this.labelDriverType.Size = new System.Drawing.Size(246, 22);
            this.labelDriverType.TabIndex = 0;
            this.labelDriverType.Text = "驱动名称";
            //
            // inputCameraName
            //
            this.inputCameraName.Location = new System.Drawing.Point(4, 38);
            this.inputCameraName.Margin = new System.Windows.Forms.Padding(0);
            this.inputCameraName.Name = "inputCameraName";
            this.inputCameraName.PlaceholderText = "请输入相机名称";
            this.inputCameraName.Size = new System.Drawing.Size(246, 32);
            this.inputCameraName.TabIndex = 4;
            this.inputCameraName.WaveSize = 0;
            //
            // labelCameraName
            //
            this.labelCameraName.Location = new System.Drawing.Point(4, 40);
            this.labelCameraName.Margin = new System.Windows.Forms.Padding(0);
            this.labelCameraName.Name = "labelCameraName";
            this.labelCameraName.Size = new System.Drawing.Size(246, 22);
            this.labelCameraName.TabIndex = 3;
            this.labelCameraName.Text = "名称";
            //
            // inputCameraCode
            //
            this.inputCameraCode.Location = new System.Drawing.Point(4, 4);
            this.inputCameraCode.Margin = new System.Windows.Forms.Padding(0);
            this.inputCameraCode.Name = "inputCameraCode";
            this.inputCameraCode.PlaceholderText = "请输入相机编码";
            this.inputCameraCode.Size = new System.Drawing.Size(246, 32);
            this.inputCameraCode.TabIndex = 2;
            this.inputCameraCode.WaveSize = 0;
            //
            // labelCameraCode
            //
            this.labelCameraCode.Location = new System.Drawing.Point(4, 4);
            this.labelCameraCode.Margin = new System.Windows.Forms.Padding(0);
            this.labelCameraCode.Name = "labelCameraCode";
            this.labelCameraCode.Size = new System.Drawing.Size(246, 22);
            this.labelCameraCode.TabIndex = 1;
            this.labelCameraCode.Text = "编码";
            //
            // labelSectionBasic
            //
            this.labelSectionBasic.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionBasic.Location = new System.Drawing.Point(4, 4);
            this.labelSectionBasic.Margin = new System.Windows.Forms.Padding(0);
            this.labelSectionBasic.Name = "labelSectionBasic";
            this.labelSectionBasic.Size = new System.Drawing.Size(246, 26);
            this.labelSectionBasic.TabIndex = 0;
            this.labelSectionBasic.Text = "基础信息";
            //
            // CameraConfigEditControl
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.gridMainSections);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "CameraConfigEditControl";
            this.Size = new System.Drawing.Size(1016, 463);
            this.gridMainSections.ResumeLayout(false);
            this.stackSectionRemark.ResumeLayout(false);
            this.stackSectionEncode.ResumeLayout(false);
            this.panelRowImageFormat.ResumeLayout(false);
            this.stackSectionCapture.ResumeLayout(false);
            this.stackSectionBasic.ResumeLayout(false);
            this.panelRowDriverType.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private AntdUI.GridPanel gridMainSections;
        private AntdUI.StackPanel stackSectionBasic;
        private AntdUI.Label labelSectionBasic;
        private AntdUI.Label labelCameraCode;
        private AntdUI.Input inputCameraCode;
        private AntdUI.Label labelCameraName;
        private AntdUI.Input inputCameraName;
        private AntdUI.Panel panelRowDriverType;
        private AntdUI.Label labelDriverType;
        private AntdUI.Select selectDriverType;
        private AntdUI.Label labelEnabled;
        private AntdUI.Checkbox checkEnabled;
        private AntdUI.Label labelDeviceIndex;
        private AntdUI.InputNumber inputDeviceIndex;
        private AntdUI.Label labelFriendlyName;
        private AntdUI.Input inputFriendlyName;
        private AntdUI.StackPanel stackSectionCapture;
        private AntdUI.Label labelSectionCapture;
        private AntdUI.Label labelWidth;
        private AntdUI.Select selectWidth;
        private AntdUI.Label labelHeight;
        private AntdUI.Select selectHeight;
        private AntdUI.Label labelFps;
        private AntdUI.Input inputFps;
        private AntdUI.Label labelPixelFormat;
        private AntdUI.Input inputPixelFormat;
        private AntdUI.Label labelExposure;
        private AntdUI.Input inputExposure;
        private AntdUI.Label labelGain;
        private AntdUI.Input inputGain;
        private AntdUI.StackPanel stackSectionEncode;
        private AntdUI.Label labelSectionEncode;
        private AntdUI.Label labelGrabTimeoutMs;
        private AntdUI.Input inputGrabTimeoutMs;
        private AntdUI.Panel panelRowImageFormat;
        private AntdUI.Label labelImageFormat;
        private AntdUI.Select selectImageFormat;
        private AntdUI.Label labelJpegQuality;
        private AntdUI.Input inputJpegQuality;
        private AntdUI.Label labelPreviewFps;
        private AntdUI.Input inputPreviewFps;
        private AntdUI.StackPanel stackSectionRemark;
        private AntdUI.Label labelSectionRemark;
        private AntdUI.Label labelSaveImageEnabled;
        private AntdUI.Checkbox checkSaveImageEnabled;
        private AntdUI.Label labelSaveImageDirectory;
        private AntdUI.Input inputSaveImageDirectory;
        private AntdUI.Label labelRemark;
        private AntdUI.Input inputRemark;
    }
}
