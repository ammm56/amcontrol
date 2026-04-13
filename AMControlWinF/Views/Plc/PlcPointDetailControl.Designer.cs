namespace AMControlWinF.Views.Plc
{
    partial class PlcPointDetailControl
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
            this.panelTagUpdateTime = new AntdUI.Panel();
            this.labelTagUpdateTimeValue = new AntdUI.Label();
            this.labelTagUpdateTimeKey = new AntdUI.Label();
            this.panelTagConnection = new AntdUI.Panel();
            this.labelTagConnectionValue = new AntdUI.Label();
            this.labelTagConnectionKey = new AntdUI.Label();
            this.panelTagQuality = new AntdUI.Panel();
            this.labelTagQualityValue = new AntdUI.Label();
            this.labelTagQualityKey = new AntdUI.Label();
            this.panelTagRawValue = new AntdUI.Panel();
            this.labelTagRawValueValue = new AntdUI.Label();
            this.labelTagRawValueKey = new AntdUI.Label();
            this.panelTagValue = new AntdUI.Panel();
            this.labelTagValueValue = new AntdUI.Label();
            this.labelTagValueKey = new AntdUI.Label();
            this.panelTagDataType = new AntdUI.Panel();
            this.labelTagDataTypeValue = new AntdUI.Label();
            this.labelTagDataTypeKey = new AntdUI.Label();
            this.panelTagAddress = new AntdUI.Panel();
            this.labelTagAddressValue = new AntdUI.Label();
            this.labelTagAddressKey = new AntdUI.Label();
            this.panelTagGroup = new AntdUI.Panel();
            this.labelTagGroupValue = new AntdUI.Label();
            this.labelTagGroupKey = new AntdUI.Label();
            this.panelTagPlc = new AntdUI.Panel();
            this.labelTagPlcValue = new AntdUI.Label();
            this.labelTagPlcKey = new AntdUI.Label();
            this.panelHeader = new AntdUI.Panel();
            this.labelSubTitle = new AntdUI.Label();
            this.labelTitle = new AntdUI.Label();
            this.panelEmpty = new AntdUI.Panel();
            this.labelEmpty = new AntdUI.Label();
            this.panelRoot.SuspendLayout();
            this.panelDetail.SuspendLayout();
            this.panelScroll.SuspendLayout();
            this.panelTagUpdateTime.SuspendLayout();
            this.panelTagConnection.SuspendLayout();
            this.panelTagQuality.SuspendLayout();
            this.panelTagRawValue.SuspendLayout();
            this.panelTagValue.SuspendLayout();
            this.panelTagDataType.SuspendLayout();
            this.panelTagAddress.SuspendLayout();
            this.panelTagGroup.SuspendLayout();
            this.panelTagPlc.SuspendLayout();
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
            this.panelScroll.Controls.Add(this.panelTagUpdateTime);
            this.panelScroll.Controls.Add(this.panelTagConnection);
            this.panelScroll.Controls.Add(this.panelTagQuality);
            this.panelScroll.Controls.Add(this.panelTagRawValue);
            this.panelScroll.Controls.Add(this.panelTagValue);
            this.panelScroll.Controls.Add(this.panelTagDataType);
            this.panelScroll.Controls.Add(this.panelTagAddress);
            this.panelScroll.Controls.Add(this.panelTagGroup);
            this.panelScroll.Controls.Add(this.panelTagPlc);
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
            this.labelErrorValue.Location = new System.Drawing.Point(14, 270);
            this.labelErrorValue.Name = "labelErrorValue";
            this.labelErrorValue.Size = new System.Drawing.Size(244, 132);
            this.labelErrorValue.TabIndex = 10;
            this.labelErrorValue.Text = "—";
            this.labelErrorValue.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            // 
            // labelErrorTitle
            // 
            this.labelErrorTitle.ForeColor = System.Drawing.Color.Gray;
            this.labelErrorTitle.Location = new System.Drawing.Point(14, 246);
            this.labelErrorTitle.Name = "labelErrorTitle";
            this.labelErrorTitle.Size = new System.Drawing.Size(244, 20);
            this.labelErrorTitle.TabIndex = 9;
            this.labelErrorTitle.Text = "错误信息";
            // 
            // panelTagUpdateTime
            // 
            this.panelTagUpdateTime.Controls.Add(this.labelTagUpdateTimeValue);
            this.panelTagUpdateTime.Controls.Add(this.labelTagUpdateTimeKey);
            this.panelTagUpdateTime.Location = new System.Drawing.Point(8, 218);
            this.panelTagUpdateTime.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagUpdateTime.Name = "panelTagUpdateTime";
            this.panelTagUpdateTime.Radius = 0;
            this.panelTagUpdateTime.Size = new System.Drawing.Size(260, 24);
            this.panelTagUpdateTime.TabIndex = 8;
            // 
            // labelTagUpdateTimeValue
            // 
            this.labelTagUpdateTimeValue.Location = new System.Drawing.Point(78, 0);
            this.labelTagUpdateTimeValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagUpdateTimeValue.Name = "labelTagUpdateTimeValue";
            this.labelTagUpdateTimeValue.Size = new System.Drawing.Size(176, 24);
            this.labelTagUpdateTimeValue.TabIndex = 1;
            this.labelTagUpdateTimeValue.Text = "—";
            // 
            // labelTagUpdateTimeKey
            // 
            this.labelTagUpdateTimeKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagUpdateTimeKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagUpdateTimeKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagUpdateTimeKey.Name = "labelTagUpdateTimeKey";
            this.labelTagUpdateTimeKey.Size = new System.Drawing.Size(72, 24);
            this.labelTagUpdateTimeKey.TabIndex = 0;
            this.labelTagUpdateTimeKey.Text = "更新时间";
            // 
            // panelTagConnection
            // 
            this.panelTagConnection.Controls.Add(this.labelTagConnectionValue);
            this.panelTagConnection.Controls.Add(this.labelTagConnectionKey);
            this.panelTagConnection.Location = new System.Drawing.Point(8, 190);
            this.panelTagConnection.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagConnection.Name = "panelTagConnection";
            this.panelTagConnection.Radius = 0;
            this.panelTagConnection.Size = new System.Drawing.Size(260, 24);
            this.panelTagConnection.TabIndex = 7;
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
            this.labelTagConnectionKey.Text = "连接状态";
            // 
            // panelTagQuality
            // 
            this.panelTagQuality.Controls.Add(this.labelTagQualityValue);
            this.panelTagQuality.Controls.Add(this.labelTagQualityKey);
            this.panelTagQuality.Location = new System.Drawing.Point(8, 162);
            this.panelTagQuality.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagQuality.Name = "panelTagQuality";
            this.panelTagQuality.Radius = 0;
            this.panelTagQuality.Size = new System.Drawing.Size(260, 24);
            this.panelTagQuality.TabIndex = 6;
            // 
            // labelTagQualityValue
            // 
            this.labelTagQualityValue.Location = new System.Drawing.Point(78, 0);
            this.labelTagQualityValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagQualityValue.Name = "labelTagQualityValue";
            this.labelTagQualityValue.Size = new System.Drawing.Size(176, 24);
            this.labelTagQualityValue.TabIndex = 1;
            this.labelTagQualityValue.Text = "—";
            // 
            // labelTagQualityKey
            // 
            this.labelTagQualityKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagQualityKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagQualityKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagQualityKey.Name = "labelTagQualityKey";
            this.labelTagQualityKey.Size = new System.Drawing.Size(72, 24);
            this.labelTagQualityKey.TabIndex = 0;
            this.labelTagQualityKey.Text = "质量";
            // 
            // panelTagRawValue
            // 
            this.panelTagRawValue.Controls.Add(this.labelTagRawValueValue);
            this.panelTagRawValue.Controls.Add(this.labelTagRawValueKey);
            this.panelTagRawValue.Location = new System.Drawing.Point(8, 134);
            this.panelTagRawValue.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagRawValue.Name = "panelTagRawValue";
            this.panelTagRawValue.Radius = 0;
            this.panelTagRawValue.Size = new System.Drawing.Size(260, 24);
            this.panelTagRawValue.TabIndex = 5;
            // 
            // labelTagRawValueValue
            // 
            this.labelTagRawValueValue.Location = new System.Drawing.Point(78, 0);
            this.labelTagRawValueValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagRawValueValue.Name = "labelTagRawValueValue";
            this.labelTagRawValueValue.Size = new System.Drawing.Size(176, 24);
            this.labelTagRawValueValue.TabIndex = 1;
            this.labelTagRawValueValue.Text = "—";
            // 
            // labelTagRawValueKey
            // 
            this.labelTagRawValueKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagRawValueKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagRawValueKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagRawValueKey.Name = "labelTagRawValueKey";
            this.labelTagRawValueKey.Size = new System.Drawing.Size(72, 24);
            this.labelTagRawValueKey.TabIndex = 0;
            this.labelTagRawValueKey.Text = "原始值";
            // 
            // panelTagValue
            // 
            this.panelTagValue.Controls.Add(this.labelTagValueValue);
            this.panelTagValue.Controls.Add(this.labelTagValueKey);
            this.panelTagValue.Location = new System.Drawing.Point(8, 106);
            this.panelTagValue.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagValue.Name = "panelTagValue";
            this.panelTagValue.Radius = 0;
            this.panelTagValue.Size = new System.Drawing.Size(260, 24);
            this.panelTagValue.TabIndex = 4;
            // 
            // labelTagValueValue
            // 
            this.labelTagValueValue.Location = new System.Drawing.Point(78, 0);
            this.labelTagValueValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagValueValue.Name = "labelTagValueValue";
            this.labelTagValueValue.Size = new System.Drawing.Size(176, 24);
            this.labelTagValueValue.TabIndex = 1;
            this.labelTagValueValue.Text = "—";
            // 
            // labelTagValueKey
            // 
            this.labelTagValueKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagValueKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagValueKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagValueKey.Name = "labelTagValueKey";
            this.labelTagValueKey.Size = new System.Drawing.Size(72, 24);
            this.labelTagValueKey.TabIndex = 0;
            this.labelTagValueKey.Text = "当前值";
            // 
            // panelTagDataType
            // 
            this.panelTagDataType.Controls.Add(this.labelTagDataTypeValue);
            this.panelTagDataType.Controls.Add(this.labelTagDataTypeKey);
            this.panelTagDataType.Location = new System.Drawing.Point(8, 78);
            this.panelTagDataType.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagDataType.Name = "panelTagDataType";
            this.panelTagDataType.Radius = 0;
            this.panelTagDataType.Size = new System.Drawing.Size(260, 24);
            this.panelTagDataType.TabIndex = 3;
            // 
            // labelTagDataTypeValue
            // 
            this.labelTagDataTypeValue.Location = new System.Drawing.Point(78, 0);
            this.labelTagDataTypeValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagDataTypeValue.Name = "labelTagDataTypeValue";
            this.labelTagDataTypeValue.Size = new System.Drawing.Size(176, 24);
            this.labelTagDataTypeValue.TabIndex = 1;
            this.labelTagDataTypeValue.Text = "—";
            // 
            // labelTagDataTypeKey
            // 
            this.labelTagDataTypeKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagDataTypeKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagDataTypeKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagDataTypeKey.Name = "labelTagDataTypeKey";
            this.labelTagDataTypeKey.Size = new System.Drawing.Size(72, 24);
            this.labelTagDataTypeKey.TabIndex = 0;
            this.labelTagDataTypeKey.Text = "数据类型";
            // 
            // panelTagAddress
            // 
            this.panelTagAddress.Controls.Add(this.labelTagAddressValue);
            this.panelTagAddress.Controls.Add(this.labelTagAddressKey);
            this.panelTagAddress.Location = new System.Drawing.Point(8, 50);
            this.panelTagAddress.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagAddress.Name = "panelTagAddress";
            this.panelTagAddress.Radius = 0;
            this.panelTagAddress.Size = new System.Drawing.Size(260, 24);
            this.panelTagAddress.TabIndex = 2;
            // 
            // labelTagAddressValue
            // 
            this.labelTagAddressValue.Location = new System.Drawing.Point(78, 0);
            this.labelTagAddressValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagAddressValue.Name = "labelTagAddressValue";
            this.labelTagAddressValue.Size = new System.Drawing.Size(176, 24);
            this.labelTagAddressValue.TabIndex = 1;
            this.labelTagAddressValue.Text = "—";
            // 
            // labelTagAddressKey
            // 
            this.labelTagAddressKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagAddressKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagAddressKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagAddressKey.Name = "labelTagAddressKey";
            this.labelTagAddressKey.Size = new System.Drawing.Size(72, 24);
            this.labelTagAddressKey.TabIndex = 0;
            this.labelTagAddressKey.Text = "地址";
            // 
            // panelTagGroup
            // 
            this.panelTagGroup.Controls.Add(this.labelTagGroupValue);
            this.panelTagGroup.Controls.Add(this.labelTagGroupKey);
            this.panelTagGroup.Location = new System.Drawing.Point(8, 22);
            this.panelTagGroup.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagGroup.Name = "panelTagGroup";
            this.panelTagGroup.Radius = 0;
            this.panelTagGroup.Size = new System.Drawing.Size(260, 24);
            this.panelTagGroup.TabIndex = 1;
            // 
            // labelTagGroupValue
            // 
            this.labelTagGroupValue.Location = new System.Drawing.Point(78, 0);
            this.labelTagGroupValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagGroupValue.Name = "labelTagGroupValue";
            this.labelTagGroupValue.Size = new System.Drawing.Size(176, 24);
            this.labelTagGroupValue.TabIndex = 1;
            this.labelTagGroupValue.Text = "—";
            // 
            // labelTagGroupKey
            // 
            this.labelTagGroupKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagGroupKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagGroupKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagGroupKey.Name = "labelTagGroupKey";
            this.labelTagGroupKey.Size = new System.Drawing.Size(72, 24);
            this.labelTagGroupKey.TabIndex = 0;
            this.labelTagGroupKey.Text = "分组";
            // 
            // panelTagPlc
            // 
            this.panelTagPlc.Controls.Add(this.labelTagPlcValue);
            this.panelTagPlc.Controls.Add(this.labelTagPlcKey);
            this.panelTagPlc.Location = new System.Drawing.Point(8, -6);
            this.panelTagPlc.Margin = new System.Windows.Forms.Padding(0);
            this.panelTagPlc.Name = "panelTagPlc";
            this.panelTagPlc.Radius = 0;
            this.panelTagPlc.Size = new System.Drawing.Size(260, 24);
            this.panelTagPlc.TabIndex = 0;
            // 
            // labelTagPlcValue
            // 
            this.labelTagPlcValue.Location = new System.Drawing.Point(78, 0);
            this.labelTagPlcValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagPlcValue.Name = "labelTagPlcValue";
            this.labelTagPlcValue.Size = new System.Drawing.Size(176, 24);
            this.labelTagPlcValue.TabIndex = 1;
            this.labelTagPlcValue.Text = "—";
            // 
            // labelTagPlcKey
            // 
            this.labelTagPlcKey.ForeColor = System.Drawing.Color.Gray;
            this.labelTagPlcKey.Location = new System.Drawing.Point(0, 0);
            this.labelTagPlcKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelTagPlcKey.Name = "labelTagPlcKey";
            this.labelTagPlcKey.Size = new System.Drawing.Size(72, 24);
            this.labelTagPlcKey.TabIndex = 0;
            this.labelTagPlcKey.Text = "PLC";
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.labelSubTitle);
            this.panelHeader.Controls.Add(this.labelTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Padding = new System.Windows.Forms.Padding(14, 12, 14, 8);
            this.panelHeader.Radius = 0;
            this.panelHeader.Size = new System.Drawing.Size(280, 60);
            this.panelHeader.TabIndex = 0;
            // 
            // labelSubTitle
            // 
            this.labelSubTitle.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelSubTitle.ForeColor = System.Drawing.Color.Gray;
            this.labelSubTitle.Location = new System.Drawing.Point(18, 28);
            this.labelSubTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelSubTitle.Name = "labelSubTitle";
            this.labelSubTitle.Size = new System.Drawing.Size(244, 12);
            this.labelSubTitle.TabIndex = 1;
            this.labelSubTitle.Text = "—";
            this.labelSubTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTitle
            // 
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(18, 16);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(244, 24);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "未选择点位";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.labelEmpty.ForeColor = System.Drawing.Color.Gray;
            this.labelEmpty.Location = new System.Drawing.Point(0, 0);
            this.labelEmpty.Margin = new System.Windows.Forms.Padding(0);
            this.labelEmpty.Name = "labelEmpty";
            this.labelEmpty.Size = new System.Drawing.Size(280, 540);
            this.labelEmpty.TabIndex = 0;
            this.labelEmpty.Text = "请选择一个 PLC 点位";
            this.labelEmpty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PlcPointDetailControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "PlcPointDetailControl";
            this.Size = new System.Drawing.Size(280, 540);
            this.panelRoot.ResumeLayout(false);
            this.panelDetail.ResumeLayout(false);
            this.panelScroll.ResumeLayout(false);
            this.panelTagUpdateTime.ResumeLayout(false);
            this.panelTagConnection.ResumeLayout(false);
            this.panelTagQuality.ResumeLayout(false);
            this.panelTagRawValue.ResumeLayout(false);
            this.panelTagValue.ResumeLayout(false);
            this.panelTagDataType.ResumeLayout(false);
            this.panelTagAddress.ResumeLayout(false);
            this.panelTagGroup.ResumeLayout(false);
            this.panelTagPlc.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.panelEmpty.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private AntdUI.Panel panelRoot;
        private AntdUI.Panel panelDetail;
        private System.Windows.Forms.Panel panelScroll;
        private AntdUI.Label labelErrorValue;
        private AntdUI.Label labelErrorTitle;
        private AntdUI.Panel panelTagUpdateTime;
        private AntdUI.Label labelTagUpdateTimeValue;
        private AntdUI.Label labelTagUpdateTimeKey;
        private AntdUI.Panel panelTagConnection;
        private AntdUI.Label labelTagConnectionValue;
        private AntdUI.Label labelTagConnectionKey;
        private AntdUI.Panel panelTagQuality;
        private AntdUI.Label labelTagQualityValue;
        private AntdUI.Label labelTagQualityKey;
        private AntdUI.Panel panelTagRawValue;
        private AntdUI.Label labelTagRawValueValue;
        private AntdUI.Label labelTagRawValueKey;
        private AntdUI.Panel panelTagValue;
        private AntdUI.Label labelTagValueValue;
        private AntdUI.Label labelTagValueKey;
        private AntdUI.Panel panelTagDataType;
        private AntdUI.Label labelTagDataTypeValue;
        private AntdUI.Label labelTagDataTypeKey;
        private AntdUI.Panel panelTagAddress;
        private AntdUI.Label labelTagAddressValue;
        private AntdUI.Label labelTagAddressKey;
        private AntdUI.Panel panelTagGroup;
        private AntdUI.Label labelTagGroupValue;
        private AntdUI.Label labelTagGroupKey;
        private AntdUI.Panel panelTagPlc;
        private AntdUI.Label labelTagPlcValue;
        private AntdUI.Label labelTagPlcKey;
        private AntdUI.Panel panelHeader;
        private AntdUI.Label labelSubTitle;
        private AntdUI.Label labelTitle;
        private AntdUI.Panel panelEmpty;
        private AntdUI.Label labelEmpty;
    }
}