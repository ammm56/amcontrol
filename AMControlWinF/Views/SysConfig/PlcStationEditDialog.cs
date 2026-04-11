using AM.Core.Context;
using AM.Model.Entity.Plc;
using AM.PageModel.SysConfig;
using AMControlWinF.Tools;
using AntdUI;
using System;
using System.Linq;
using System.Windows.Forms;

namespace AMControlWinF.Views.SysConfig
{
    /// <summary>
    /// PLC 站新增/编辑对话框。
    /// 风格与现有 UserEditDialog 保持一致：
    /// - 纹理背景
    /// - 中央大卡片
    /// - 顶部标题说明
    /// - 中间滚动表单区
    /// - 底部右对齐按钮栏
    /// </summary>
    public partial class PlcStationEditDialog : AntdUI.Window
    {
        private readonly PlcStationEditorModel _model;
        private bool _isCreateMode = true;

        public PlcStationEditDialog()
        {
            InitializeComponent();

            _model = new PlcStationEditorModel();

            InitializeDropdowns();
            BindEvents();
            ApplyThemeFromConfig();
            ApplyMode();
            ApplyConnectionMode();
        }

        /// <summary>
        /// 是否新增模式。
        /// </summary>
        public bool IsCreateMode
        {
            get { return _isCreateMode; }
            set
            {
                _isCreateMode = value;
                ApplyMode();
            }
        }

        /// <summary>
        /// 保存成功后的结果实体。
        /// </summary>
        public PlcStationConfigEntity ResultEntity { get; private set; }

        /// <summary>
        /// 装载待编辑实体；传 null 表示新增。
        /// </summary>
        public void SetEntity(PlcStationConfigEntity entity)
        {
            if (entity == null)
            {
                _model.ResetForCreate();
            }
            else
            {
                _model.LoadFrom(entity);
            }

            ApplyEditorToUi();
            ApplyConnectionMode();
        }

        private void InitializeDropdowns()
        {
            dropdownConnectionType.Items.Clear();
            dropdownConnectionType.Items.AddRange(PlcStationEditorModel.ConnectionTypes.Select(x => (object)x).ToArray());

            dropdownProtocolType.Items.Clear();
            dropdownProtocolType.Items.AddRange(PlcStationEditorModel.ProtocolTypes.Select(x => (object)x).ToArray());

            dropdownParity.Items.Clear();
            dropdownParity.Items.AddRange(PlcStationEditorModel.ParityOptions.Select(x => (object)x).ToArray());

            dropdownStopBits.Items.Clear();
            dropdownStopBits.Items.AddRange(PlcStationEditorModel.StopBitsOptions.Select(x => (object)x).ToArray());
        }

        private void BindEvents()
        {
            buttonOk.Click += ButtonOk_Click;
            buttonCancel.Click += ButtonCancel_Click;

            dropdownConnectionType.SelectedValueChanged += DropdownConnectionType_SelectedValueChanged;

            Shown += PlcStationEditDialog_Shown;

            KeyPreview = true;
            KeyDown += PlcStationEditDialog_KeyDown;
        }

        private void PlcStationEditDialog_Shown(object sender, EventArgs e)
        {
            inputName.Focus();
            inputName.SelectAll();
        }

        private void DropdownConnectionType_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            ApplyConnectionMode();
        }

        private void ApplyMode()
        {
            if (_isCreateMode)
            {
                Text = "新增 PLC 站";
                labelDialogTitle.Text = "新增 PLC 站";
                labelDialogDescription.Text = "创建 PLC 站连接对象并配置协议、连接参数与扫描周期。";
                Size = new System.Drawing.Size(680, 860);
                buttonOk.Text = "保存";
            }
            else
            {
                Text = "编辑 PLC 站";
                labelDialogTitle.Text = "编辑 PLC 站";
                labelDialogDescription.Text = "修改 PLC 站的连接参数、协议类型和运行配置。";
                Size = new System.Drawing.Size(680, 820);
                buttonOk.Text = "保存";
            }
        }

        private void ApplyEditorToUi()
        {
            inputName.Text = _model.Name;
            inputDisplayName.Text = _model.DisplayName;
            inputVendor.Text = _model.Vendor;
            inputModel.Text = _model.Model;

            SetSelectValue(dropdownConnectionType, _model.ConnectionType);
            SetSelectValue(dropdownProtocolType, _model.ProtocolType);

            inputIpAddress.Text = _model.IpAddress;
            inputPort.Text = _model.PortText;

            inputComPort.Text = _model.ComPort;
            inputBaudRate.Text = _model.BaudRateText;
            inputDataBits.Text = _model.DataBitsText;

            SetSelectValue(dropdownParity, _model.Parity);
            SetSelectValue(dropdownStopBits, _model.StopBits);

            inputStationNo.Text = _model.StationNoText;
            inputNetworkNo.Text = _model.NetworkNoText;
            inputPcNo.Text = _model.PcNoText;
            inputRack.Text = _model.RackText;
            inputSlot.Text = _model.SlotText;

            inputTimeoutMs.Text = _model.TimeoutMsText;
            inputReconnectIntervalMs.Text = _model.ReconnectIntervalMsText;
            inputScanIntervalMs.Text = _model.ScanIntervalMsText;
            inputSortOrder.Text = _model.SortOrderText;

            checkEnabled.Checked = _model.IsEnabled;
            inputDescription.Text = _model.Description;
            inputRemark.Text = _model.Remark;
        }

        private void SyncUiToModel()
        {
            _model.Name = inputName.Text;
            _model.DisplayName = inputDisplayName.Text;
            _model.Vendor = inputVendor.Text;
            _model.Model = inputModel.Text;

            _model.ConnectionType = GetSelectValue(dropdownConnectionType);
            _model.ProtocolType = GetSelectValue(dropdownProtocolType);

            _model.IpAddress = inputIpAddress.Text;
            _model.PortText = inputPort.Text;

            _model.ComPort = inputComPort.Text;
            _model.BaudRateText = inputBaudRate.Text;
            _model.DataBitsText = inputDataBits.Text;

            _model.Parity = GetSelectValue(dropdownParity);
            _model.StopBits = GetSelectValue(dropdownStopBits);

            _model.StationNoText = inputStationNo.Text;
            _model.NetworkNoText = inputNetworkNo.Text;
            _model.PcNoText = inputPcNo.Text;
            _model.RackText = inputRack.Text;
            _model.SlotText = inputSlot.Text;

            _model.TimeoutMsText = inputTimeoutMs.Text;
            _model.ReconnectIntervalMsText = inputReconnectIntervalMs.Text;
            _model.ScanIntervalMsText = inputScanIntervalMs.Text;
            _model.SortOrderText = inputSortOrder.Text;

            _model.IsEnabled = checkEnabled.Checked;
            _model.Description = inputDescription.Text;
            _model.Remark = inputRemark.Text;
        }

        private void ApplyConnectionMode()
        {
            var connectionType = GetSelectValue(dropdownConnectionType);

            bool isTcp = string.Equals(connectionType, "Tcp", StringComparison.OrdinalIgnoreCase);
            bool isSerial = string.Equals(connectionType, "Serial", StringComparison.OrdinalIgnoreCase);

            panelRowIpAddress.Visible = isTcp;
            panelRowPort.Visible = isTcp;

            panelRowComPort.Visible = isSerial;
            panelRowBaudRate.Visible = isSerial;
            panelRowDataBits.Visible = isSerial;
            panelRowParity.Visible = isSerial;
            panelRowStopBits.Visible = isSerial;
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            SyncUiToModel();

            var result = _model.BuildEntity();
            if (!result.Success || result.Item == null)
            {
                PageDialogHelper.ShowWarn(this, "输入提示", result.Message);
                return;
            }

            ResultEntity = result.Item;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void PlcStationEditDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            if (e.KeyCode == Keys.Enter && !(ActiveControl is TextBoxBase))
            {
                e.SuppressKeyPress = true;
                ButtonOk_Click(sender, EventArgs.Empty);
            }
        }

        private void ApplyThemeFromConfig()
        {
            var theme = ConfigContext.Instance.Config.Setting.Theme;
            var isDarkMode = IsDarkTheme(theme);

            if (isDarkMode)
            {
                AntdUI.Config.IsDark = true;
            }
            else
            {
                AntdUI.Config.IsLight = true;
            }

            textureBackgroundDialog.SetTheme(isDarkMode);
        }

        private static bool IsDarkTheme(string theme)
        {
            if (string.IsNullOrWhiteSpace(theme))
            {
                return false;
            }

            return string.Equals(theme, "SkinDark", StringComparison.OrdinalIgnoreCase)
                || string.Equals(theme, "Dark", StringComparison.OrdinalIgnoreCase);
        }

        private static void SetSelectValue(Select select, string value)
        {
            if (select == null)
            {
                return;
            }

            var text = string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();

            if (!string.IsNullOrWhiteSpace(text))
            {
                bool exists = select.Items.Cast<object>()
                    .Any(x => string.Equals(x == null ? string.Empty : x.ToString(), text, StringComparison.OrdinalIgnoreCase));

                if (!exists)
                {
                    select.Items.Add(text);
                }
            }

            select.SelectedValue = text;
        }

        private static string GetSelectValue(Select select)
        {
            if (select == null || select.SelectedValue == null)
            {
                return string.Empty;
            }

            return select.SelectedValue.ToString();
        }
    }
}