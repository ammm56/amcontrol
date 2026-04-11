namespace AMControlWinF.Views.Plc
{
    partial class PlcStatusDetailControl
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
            this.panelRoot = new AntdUI.Panel();
            this.panelDetail = new AntdUI.Panel();
            this.panelScroll = new System.Windows.Forms.Panel();
            this.labelErrorValue = new AntdUI.Label();
            this.labelErrorTitle = new AntdUI.Label();
            this.panelTagFailed = new AntdUI.Panel();
            this.labelTagFailedValue = new AntdUI.Label();
            this.labelTagFailedKey = new AntdUI.Label();
            this.panelTagSuccess = new AntdUI.Panel();
            this.labelTagSuccessValue = new AntdUI.Label();
            this.labelTagSuccessKey = new AntdUI.Label();
            this.panelTagAverageWrite = new AntdUI.Panel();
            this.labelTagAverageWriteValue = new AntdUI.Label();
            this.labelTagAverageWriteKey = new AntdUI.Label();
            this.panelTagAverageRead = new AntdUI.Panel();
            this.labelTagAverageReadValue = new AntdUI.Label();
            this.labelTagAverageReadKey = new AntdUI.Label();
            this.panelTagLastConnect = new AntdUI.Panel();
            this.labelTagLastConnectValue = new AntdUI.Label();
            this.labelTagLastConnectKey = new AntdUI.Label();
            this.panelTagLastScan = new AntdUI.Panel();
            this.labelTagLastScanValue = new AntdUI.Label();
            this.labelTagLastScanKey = new AntdUI.Label();
            this.panelTagScan = new AntdUI.Panel();
            this.labelTagScanValue = new AntdUI.Label();
            this.labelTagScanKey = new AntdUI.Label();
            this.panelTagRuntime = new AntdUI.Panel();
            this.labelTagRuntimeValue = new AntdUI.Label();
            this.labelTagRuntimeKey = new AntdUI.Label();
            this.panelTagConfig = new AntdUI.Panel();
            this.labelTagConfigValue = new AntdUI.Label();
            this.labelTagConfigKey = new AntdUI.Label();
            this.panelTagEndpoint = new AntdUI.Panel();
            this.labelTagEndpointValue = new AntdUI.Label();
            this.labelTagEndpointKey = new AntdUI.Label();
            this.panelTagConnection = new AntdUI.Panel();
            this.labelTagConnectionValue = new AntdUI.Label();
            this.labelTagConnectionKey = new AntdUI.Label();
            this.panelTagProtocol = new AntdUI.Panel();
            this.labelTagProtocolValue = new AntdUI.Label();
            this.labelTagProtocolKey = new AntdUI.Label();
            this.panelHeader = new AntdUI.Panel();
            this.labelSubTitle = new AntdUI.Label();
            this.labelTitle = new AntdUI.Label();
            this.panelEmpty = new AntdUI.Panel();
            this.labelEmpty = new AntdUI.Label();
            this.panelRoot.SuspendLayout();
            this.panelDetail.SuspendLayout();
            this.panelScroll.SuspendLayout();
            this.panelTagFailed.SuspendLayout();
            this.panelTagSuccess.SuspendLayout();
            this.panelTagAverageWrite.SuspendLayout();
            this.panelTagAverageRead.SuspendLayout();
            this.panelTagLastConnect.SuspendLayout();
            this.panelTagLastScan.SuspendLayout();
            this.panelTagScan.SuspendLayout();
            this.panelTagRuntime.SuspendLayout();
            this.panelTagConfig.SuspendLayout();
            this.panelTagEndpoint.SuspendLayout();
            this.panelTagConnection.SuspendLayout();
            this.panelTagProtocol.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.panelEmpty.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.panelDetail);
            this.panelRoot.Controls.Add(this.panelEmpty);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Margin = new System.Windows.Forms.Padding(0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(280, 540);
            this.panelRoot.TabIndex = 0;
            // 
            // panelDetail
            // 
            this.panelDetail.Controls.Add(this.panelScroll);
            this.panelDetail.Controls.Add(this.panelHeader);
            this.panelDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetail.Location = new System.Drawing.Point(0, 0);
            this.panelDetail.Margin = new System.Windows.Forms.Padding(0);
            this.panelDetail.Name = "panelDetail";
            this.panelDetail.Radius = 0;
            this.panelDetail.Size = new System.Drawing.Size(280, 540);
            this.panelDetail.TabIndex = 1;
            // 
            // panelScroll
            // 
            this.panelScroll.Controls.Add(this.labelErrorValue);
            this.panelScroll.Controls.Add(this.labelErrorTitle);
            this.panelScroll.Controls.Add(this.panelTagFailed);
            this.panelScroll.Controls.Add(this.panelTagSuccess);
            this.panelScroll.Controls.Add(this.panelTagAverageWrite);
            this.panelScroll.Controls.Add(this.panelTagAverageRead);
            this.panelScroll.Controls.Add(this.panelTagLastConnect);
            this.panelScroll.Controls.Add(this.panelTagLastScan);
            this.panelScroll.Controls.Add(this.panelTagScan);
            this.panelScroll.Controls.Add(this.panelTagRuntime);
            this.panelScroll.Controls.Add(this.panelTagConfig);
            this.panelScroll.Controls.Add(this.panelTagEndpoint);
            this.panelScroll.Controls.Add(this.panelTagConnection);
            this.panelScroll.Controls.Add(this.panelTagProtocol);
            this.panelScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelScroll.Location = new System.Drawing.Point(0, 60);
            this.panelScroll.Margin = new System.Windows.Forms.Padding(0);
            this.panelScroll.Name = "panelScroll";
            this.panelScroll.Padding = new System.Windows.Forms.Padding(14, 12, 14, 12);
            this.panelScroll.Size = new System.Drawing.Size(280, 480);
            this.panelScroll.TabIndex = 1;
            // 
            // labelErrorValue
            // 
            this.labelErrorValue.Location = new System.Drawing.Point(14, 358);
            this.labelErrorValue.Name = "labelErrorValue";
            this.labelErrorValue.Size = new System.Drawing.Size(244, 93);
            this.labelErrorValue.TabIndex = 13;
            this.labelErrorValue.Text = "—";
            this.labelErrorValue.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            // 
            // labelErrorTitle
            // 
            this.labelErrorTitle.ForeColor = System.Drawing.Color.Gray;
            this.labelErrorTitle.Location = new System.Drawing.Point(14, 334);
            this.labelErrorTitle.Name = "labelErrorTitle";
            this.labelErrorTitle.Size = new System.Drawing.Size(244, 20);
            this.labelErrorTitle.TabIndex = 12;
            this.labelErrorTitle.Text = "最近错误";
            // 
            // panelTagFailed
            // 
            this.panelTagFailed.Controls.Add(this.labelTagFailedValue);
            this.panelTagFailed.Controls.Add(this.labelTagFailedKey);
            this.panelTagFailed.Location = new System.Drawing.Point(8, 310);
            this.panelTagFailed.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagFailed.Name = "panelTagFailed";
            this.panelTagFailed.Radius = 0;
            this.panelTagFailed.Size = new System.Drawing.Size(260, 24);
            this.panelTagFailed.TabIndex = 11;
            // 
            // labelTagFailedValue
            // 
            this.labelTagFailedValue.Location = new System.Drawing.Point(78, 0);
            this.labelTagFailedValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagFailedValue.Name = "labelTagFailedValue";
            this.labelTagFailedValue.Size = new System.Drawing.Size(176, 24);
            this.labelTagFailedValue.TabIndex = 1;
            this.labelTagFailedValue.Text = "—";
            // 
            // labelTagFailedKey
            // 
            this.labelTagFailedKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagFailedKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagFailedKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagFailedKey.Name = "labelTagFailedKey";
            this.labelTagFailedKey.Size = new System.Drawing.Size(72, 24);
            this.labelTagFailedKey.TabIndex = 0;
            this.labelTagFailedKey.Text = "失败轮次";
            this.labelTagFailedKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagSuccess
            // 
            this.panelTagSuccess.Controls.Add(this.labelTagSuccessValue);
            this.panelTagSuccess.Controls.Add(this.labelTagSuccessKey);
            this.panelTagSuccess.Location = new System.Drawing.Point(8, 282);
            this.panelTagSuccess.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagSuccess.Name = "panelTagSuccess";
            this.panelTagSuccess.Radius = 0;
            this.panelTagSuccess.Size = new System.Drawing.Size(260, 24);
            this.panelTagSuccess.TabIndex = 10;
            // 
            // labelTagSuccessValue
            // 
            this.labelTagSuccessValue.Location = new System.Drawing.Point(78, 0);
            this.labelTagSuccessValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagSuccessValue.Name = "labelTagSuccessValue";
            this.labelTagSuccessValue.Size = new System.Drawing.Size(176, 24);
            this.labelTagSuccessValue.TabIndex = 1;
            this.labelTagSuccessValue.Text = "—";
            // 
            // labelTagSuccessKey
            // 
            this.labelTagSuccessKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagSuccessKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagSuccessKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagSuccessKey.Name = "labelTagSuccessKey";
            this.labelTagSuccessKey.Size = new System.Drawing.Size(72, 24);
            this.labelTagSuccessKey.TabIndex = 0;
            this.labelTagSuccessKey.Text = "成功轮次";
            this.labelTagSuccessKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagAverageWrite
            // 
            this.panelTagAverageWrite.Controls.Add(this.labelTagAverageWriteValue);
            this.panelTagAverageWrite.Controls.Add(this.labelTagAverageWriteKey);
            this.panelTagAverageWrite.Location = new System.Drawing.Point(8, 254);
            this.panelTagAverageWrite.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagAverageWrite.Name = "panelTagAverageWrite";
            this.panelTagAverageWrite.Radius = 0;
            this.panelTagAverageWrite.Size = new System.Drawing.Size(260, 24);
            this.panelTagAverageWrite.TabIndex = 9;
            // 
            // labelTagAverageWriteValue
            // 
            this.labelTagAverageWriteValue.Location = new System.Drawing.Point(78, 0);
            this.labelTagAverageWriteValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagAverageWriteValue.Name = "labelTagAverageWriteValue";
            this.labelTagAverageWriteValue.Size = new System.Drawing.Size(176, 24);
            this.labelTagAverageWriteValue.TabIndex = 1;
            this.labelTagAverageWriteValue.Text = "—";
            // 
            // labelTagAverageWriteKey
            // 
            this.labelTagAverageWriteKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagAverageWriteKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagAverageWriteKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagAverageWriteKey.Name = "labelTagAverageWriteKey";
            this.labelTagAverageWriteKey.Size = new System.Drawing.Size(72, 24);
            this.labelTagAverageWriteKey.TabIndex = 0;
            this.labelTagAverageWriteKey.Text = "平均写入";
            this.labelTagAverageWriteKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagAverageRead
            // 
            this.panelTagAverageRead.Controls.Add(this.labelTagAverageReadValue);
            this.panelTagAverageRead.Controls.Add(this.labelTagAverageReadKey);
            this.panelTagAverageRead.Location = new System.Drawing.Point(8, 226);
            this.panelTagAverageRead.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagAverageRead.Name = "panelTagAverageRead";
            this.panelTagAverageRead.Radius = 0;
            this.panelTagAverageRead.Size = new System.Drawing.Size(260, 24);
            this.panelTagAverageRead.TabIndex = 8;
            // 
            // labelTagAverageReadValue
            // 
            this.labelTagAverageReadValue.Location = new System.Drawing.Point(78, 0);
            this.labelTagAverageReadValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagAverageReadValue.Name = "labelTagAverageReadValue";
            this.labelTagAverageReadValue.Size = new System.Drawing.Size(176, 24);
            this.labelTagAverageReadValue.TabIndex = 1;
            this.labelTagAverageReadValue.Text = "—";
            // 
            // labelTagAverageReadKey
            // 
            this.labelTagAverageReadKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagAverageReadKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagAverageReadKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagAverageReadKey.Name = "labelTagAverageReadKey";
            this.labelTagAverageReadKey.Size = new System.Drawing.Size(72, 24);
            this.labelTagAverageReadKey.TabIndex = 0;
            this.labelTagAverageReadKey.Text = "扫描均值";
            this.labelTagAverageReadKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagLastConnect
            // 
            this.panelTagLastConnect.Controls.Add(this.labelTagLastConnectValue);
            this.panelTagLastConnect.Controls.Add(this.labelTagLastConnectKey);
            this.panelTagLastConnect.Location = new System.Drawing.Point(8, 198);
            this.panelTagLastConnect.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagLastConnect.Name = "panelTagLastConnect";
            this.panelTagLastConnect.Radius = 0;
            this.panelTagLastConnect.Size = new System.Drawing.Size(260, 24);
            this.panelTagLastConnect.TabIndex = 7;
            // 
            // labelTagLastConnectValue
            // 
            this.labelTagLastConnectValue.Location = new System.Drawing.Point(78, 0);
            this.labelTagLastConnectValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagLastConnectValue.Name = "labelTagLastConnectValue";
            this.labelTagLastConnectValue.Size = new System.Drawing.Size(176, 24);
            this.labelTagLastConnectValue.TabIndex = 1;
            this.labelTagLastConnectValue.Text = "—";
            // 
            // labelTagLastConnectKey
            // 
            this.labelTagLastConnectKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagLastConnectKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagLastConnectKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagLastConnectKey.Name = "labelTagLastConnectKey";
            this.labelTagLastConnectKey.Size = new System.Drawing.Size(72, 24);
            this.labelTagLastConnectKey.TabIndex = 0;
            this.labelTagLastConnectKey.Text = "连接时间";
            this.labelTagLastConnectKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagLastScan
            // 
            this.panelTagLastScan.Controls.Add(this.labelTagLastScanValue);
            this.panelTagLastScan.Controls.Add(this.labelTagLastScanKey);
            this.panelTagLastScan.Location = new System.Drawing.Point(8, 170);
            this.panelTagLastScan.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagLastScan.Name = "panelTagLastScan";
            this.panelTagLastScan.Radius = 0;
            this.panelTagLastScan.Size = new System.Drawing.Size(260, 24);
            this.panelTagLastScan.TabIndex = 6;
            // 
            // labelTagLastScanValue
            // 
            this.labelTagLastScanValue.Location = new System.Drawing.Point(78, 0);
            this.labelTagLastScanValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagLastScanValue.Name = "labelTagLastScanValue";
            this.labelTagLastScanValue.Size = new System.Drawing.Size(176, 24);
            this.labelTagLastScanValue.TabIndex = 1;
            this.labelTagLastScanValue.Text = "—";
            // 
            // labelTagLastScanKey
            // 
            this.labelTagLastScanKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagLastScanKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagLastScanKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagLastScanKey.Name = "labelTagLastScanKey";
            this.labelTagLastScanKey.Size = new System.Drawing.Size(72, 24);
            this.labelTagLastScanKey.TabIndex = 0;
            this.labelTagLastScanKey.Text = "扫描开始时间";
            this.labelTagLastScanKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagScan
            // 
            this.panelTagScan.Controls.Add(this.labelTagScanValue);
            this.panelTagScan.Controls.Add(this.labelTagScanKey);
            this.panelTagScan.Location = new System.Drawing.Point(8, 142);
            this.panelTagScan.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagScan.Name = "panelTagScan";
            this.panelTagScan.Radius = 0;
            this.panelTagScan.Size = new System.Drawing.Size(260, 24);
            this.panelTagScan.TabIndex = 5;
            // 
            // labelTagScanValue
            // 
            this.labelTagScanValue.Location = new System.Drawing.Point(78, 0);
            this.labelTagScanValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagScanValue.Name = "labelTagScanValue";
            this.labelTagScanValue.Size = new System.Drawing.Size(176, 24);
            this.labelTagScanValue.TabIndex = 1;
            this.labelTagScanValue.Text = "—";
            // 
            // labelTagScanKey
            // 
            this.labelTagScanKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagScanKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagScanKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagScanKey.Name = "labelTagScanKey";
            this.labelTagScanKey.Size = new System.Drawing.Size(72, 24);
            this.labelTagScanKey.TabIndex = 0;
            this.labelTagScanKey.Text = "扫描";
            this.labelTagScanKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagRuntime
            // 
            this.panelTagRuntime.Controls.Add(this.labelTagRuntimeValue);
            this.panelTagRuntime.Controls.Add(this.labelTagRuntimeKey);
            this.panelTagRuntime.Location = new System.Drawing.Point(8, 114);
            this.panelTagRuntime.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagRuntime.Name = "panelTagRuntime";
            this.panelTagRuntime.Radius = 0;
            this.panelTagRuntime.Size = new System.Drawing.Size(260, 24);
            this.panelTagRuntime.TabIndex = 4;
            // 
            // labelTagRuntimeValue
            // 
            this.labelTagRuntimeValue.Location = new System.Drawing.Point(78, 0);
            this.labelTagRuntimeValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagRuntimeValue.Name = "labelTagRuntimeValue";
            this.labelTagRuntimeValue.Size = new System.Drawing.Size(176, 24);
            this.labelTagRuntimeValue.TabIndex = 1;
            this.labelTagRuntimeValue.Text = "—";
            // 
            // labelTagRuntimeKey
            // 
            this.labelTagRuntimeKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagRuntimeKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagRuntimeKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagRuntimeKey.Name = "labelTagRuntimeKey";
            this.labelTagRuntimeKey.Size = new System.Drawing.Size(72, 24);
            this.labelTagRuntimeKey.TabIndex = 0;
            this.labelTagRuntimeKey.Text = "通讯";
            this.labelTagRuntimeKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagConfig
            // 
            this.panelTagConfig.Controls.Add(this.labelTagConfigValue);
            this.panelTagConfig.Controls.Add(this.labelTagConfigKey);
            this.panelTagConfig.Location = new System.Drawing.Point(8, 86);
            this.panelTagConfig.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagConfig.Name = "panelTagConfig";
            this.panelTagConfig.Radius = 0;
            this.panelTagConfig.Size = new System.Drawing.Size(260, 24);
            this.panelTagConfig.TabIndex = 3;
            // 
            // labelTagConfigValue
            // 
            this.labelTagConfigValue.Location = new System.Drawing.Point(78, 0);
            this.labelTagConfigValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagConfigValue.Name = "labelTagConfigValue";
            this.labelTagConfigValue.Size = new System.Drawing.Size(176, 24);
            this.labelTagConfigValue.TabIndex = 1;
            this.labelTagConfigValue.Text = "—";
            // 
            // labelTagConfigKey
            // 
            this.labelTagConfigKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagConfigKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagConfigKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagConfigKey.Name = "labelTagConfigKey";
            this.labelTagConfigKey.Size = new System.Drawing.Size(72, 24);
            this.labelTagConfigKey.TabIndex = 0;
            this.labelTagConfigKey.Text = "配置";
            this.labelTagConfigKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagEndpoint
            // 
            this.panelTagEndpoint.Controls.Add(this.labelTagEndpointValue);
            this.panelTagEndpoint.Controls.Add(this.labelTagEndpointKey);
            this.panelTagEndpoint.Location = new System.Drawing.Point(8, 58);
            this.panelTagEndpoint.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagEndpoint.Name = "panelTagEndpoint";
            this.panelTagEndpoint.Radius = 0;
            this.panelTagEndpoint.Size = new System.Drawing.Size(260, 24);
            this.panelTagEndpoint.TabIndex = 2;
            // 
            // labelTagEndpointValue
            // 
            this.labelTagEndpointValue.Location = new System.Drawing.Point(78, 0);
            this.labelTagEndpointValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagEndpointValue.Name = "labelTagEndpointValue";
            this.labelTagEndpointValue.Size = new System.Drawing.Size(176, 24);
            this.labelTagEndpointValue.TabIndex = 1;
            this.labelTagEndpointValue.Text = "—";
            // 
            // labelTagEndpointKey
            // 
            this.labelTagEndpointKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagEndpointKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagEndpointKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagEndpointKey.Name = "labelTagEndpointKey";
            this.labelTagEndpointKey.Size = new System.Drawing.Size(72, 24);
            this.labelTagEndpointKey.TabIndex = 0;
            this.labelTagEndpointKey.Text = "端点";
            this.labelTagEndpointKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagConnection
            // 
            this.panelTagConnection.Controls.Add(this.labelTagConnectionValue);
            this.panelTagConnection.Controls.Add(this.labelTagConnectionKey);
            this.panelTagConnection.Location = new System.Drawing.Point(8, 30);
            this.panelTagConnection.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagConnection.Name = "panelTagConnection";
            this.panelTagConnection.Radius = 0;
            this.panelTagConnection.Size = new System.Drawing.Size(260, 24);
            this.panelTagConnection.TabIndex = 1;
            // 
            // labelTagConnectionValue
            // 
            this.labelTagConnectionValue.Location = new System.Drawing.Point(78, 0);
            this.labelTagConnectionValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagConnectionValue.Name = "labelTagConnectionValue";
            this.labelTagConnectionValue.Size = new System.Drawing.Size(176, 24);
            this.labelTagConnectionValue.TabIndex = 1;
            this.labelTagConnectionValue.Text = "—";
            // 
            // labelTagConnectionKey
            // 
            this.labelTagConnectionKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagConnectionKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagConnectionKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagConnectionKey.Name = "labelTagConnectionKey";
            this.labelTagConnectionKey.Size = new System.Drawing.Size(72, 24);
            this.labelTagConnectionKey.TabIndex = 0;
            this.labelTagConnectionKey.Text = "连接";
            this.labelTagConnectionKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTagProtocol
            // 
            this.panelTagProtocol.Controls.Add(this.labelTagProtocolValue);
            this.panelTagProtocol.Controls.Add(this.labelTagProtocolKey);
            this.panelTagProtocol.Location = new System.Drawing.Point(8, 2);
            this.panelTagProtocol.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagProtocol.Name = "panelTagProtocol";
            this.panelTagProtocol.Radius = 0;
            this.panelTagProtocol.Size = new System.Drawing.Size(260, 24);
            this.panelTagProtocol.TabIndex = 0;
            // 
            // labelTagProtocolValue
            // 
            this.labelTagProtocolValue.Location = new System.Drawing.Point(78, 0);
            this.labelTagProtocolValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagProtocolValue.Name = "labelTagProtocolValue";
            this.labelTagProtocolValue.Size = new System.Drawing.Size(176, 24);
            this.labelTagProtocolValue.TabIndex = 1;
            this.labelTagProtocolValue.Text = "—";
            // 
            // labelTagProtocolKey
            // 
            this.labelTagProtocolKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagProtocolKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagProtocolKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagProtocolKey.Name = "labelTagProtocolKey";
            this.labelTagProtocolKey.Size = new System.Drawing.Size(72, 24);
            this.labelTagProtocolKey.TabIndex = 0;
            this.labelTagProtocolKey.Text = "协议";
            this.labelTagProtocolKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.labelSubTitle);
            this.panelHeader.Controls.Add(this.labelTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.panelHeader.Radius = 0;
            this.panelHeader.Size = new System.Drawing.Size(280, 60);
            this.panelHeader.TabIndex = 0;
            // 
            // labelSubTitle
            // 
            this.labelSubTitle.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelSubTitle.ForeColor = System.Drawing.Color.Gray;
            this.labelSubTitle.Location = new System.Drawing.Point(12, 32);
            this.labelSubTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelSubTitle.Name = "labelSubTitle";
            this.labelSubTitle.Size = new System.Drawing.Size(256, 20);
            this.labelSubTitle.TabIndex = 1;
            this.labelSubTitle.Text = "—";
            // 
            // labelTitle
            // 
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(12, 8);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(256, 24);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "未选择 PLC";
            // 
            // panelEmpty
            // 
            this.panelEmpty.Controls.Add(this.labelEmpty);
            this.panelEmpty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEmpty.Location = new System.Drawing.Point(0, 0);
            this.panelEmpty.Margin = new System.Windows.Forms.Padding(0);
            this.panelEmpty.Name = "panelEmpty";
            this.panelEmpty.Radius = 0;
            this.panelEmpty.Size = new System.Drawing.Size(280, 540);
            this.panelEmpty.TabIndex = 0;
            // 
            // labelEmpty
            // 
            this.labelEmpty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEmpty.Location = new System.Drawing.Point(0, 0);
            this.labelEmpty.Margin = new System.Windows.Forms.Padding(0);
            this.labelEmpty.Name = "labelEmpty";
            this.labelEmpty.Size = new System.Drawing.Size(280, 540);
            this.labelEmpty.TabIndex = 0;
            this.labelEmpty.Text = "请在左侧选择一个 PLC 站";
            this.labelEmpty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PlcStatusDetailControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "PlcStatusDetailControl";
            this.Size = new System.Drawing.Size(280, 540);
            this.panelRoot.ResumeLayout(false);
            this.panelDetail.ResumeLayout(false);
            this.panelScroll.ResumeLayout(false);
            this.panelTagFailed.ResumeLayout(false);
            this.panelTagSuccess.ResumeLayout(false);
            this.panelTagAverageWrite.ResumeLayout(false);
            this.panelTagAverageRead.ResumeLayout(false);
            this.panelTagLastConnect.ResumeLayout(false);
            this.panelTagLastScan.ResumeLayout(false);
            this.panelTagScan.ResumeLayout(false);
            this.panelTagRuntime.ResumeLayout(false);
            this.panelTagConfig.ResumeLayout(false);
            this.panelTagEndpoint.ResumeLayout(false);
            this.panelTagConnection.ResumeLayout(false);
            this.panelTagProtocol.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.panelEmpty.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AntdUI.Panel panelRoot;
        private AntdUI.Panel panelDetail;
        private System.Windows.Forms.Panel panelScroll;
        private AntdUI.Panel panelHeader;
        private AntdUI.Label labelTitle;
        private AntdUI.Label labelSubTitle;
        private AntdUI.Panel panelEmpty;
        private AntdUI.Label labelEmpty;
        private AntdUI.Panel panelTagProtocol;
        private AntdUI.Label labelTagProtocolValue;
        private AntdUI.Label labelTagProtocolKey;
        private AntdUI.Panel panelTagConnection;
        private AntdUI.Label labelTagConnectionValue;
        private AntdUI.Label labelTagConnectionKey;
        private AntdUI.Panel panelTagEndpoint;
        private AntdUI.Label labelTagEndpointValue;
        private AntdUI.Label labelTagEndpointKey;
        private AntdUI.Panel panelTagConfig;
        private AntdUI.Label labelTagConfigValue;
        private AntdUI.Label labelTagConfigKey;
        private AntdUI.Panel panelTagRuntime;
        private AntdUI.Label labelTagRuntimeValue;
        private AntdUI.Label labelTagRuntimeKey;
        private AntdUI.Panel panelTagScan;
        private AntdUI.Label labelTagScanValue;
        private AntdUI.Label labelTagScanKey;
        private AntdUI.Panel panelTagLastScan;
        private AntdUI.Label labelTagLastScanValue;
        private AntdUI.Label labelTagLastScanKey;
        private AntdUI.Panel panelTagLastConnect;
        private AntdUI.Label labelTagLastConnectValue;
        private AntdUI.Label labelTagLastConnectKey;
        private AntdUI.Panel panelTagAverageRead;
        private AntdUI.Label labelTagAverageReadValue;
        private AntdUI.Label labelTagAverageReadKey;
        private AntdUI.Panel panelTagAverageWrite;
        private AntdUI.Label labelTagAverageWriteValue;
        private AntdUI.Label labelTagAverageWriteKey;
        private AntdUI.Panel panelTagSuccess;
        private AntdUI.Label labelTagSuccessValue;
        private AntdUI.Label labelTagSuccessKey;
        private AntdUI.Panel panelTagFailed;
        private AntdUI.Label labelTagFailedValue;
        private AntdUI.Label labelTagFailedKey;
        private AntdUI.Label labelErrorTitle;
        private AntdUI.Label labelErrorValue;
    }
}