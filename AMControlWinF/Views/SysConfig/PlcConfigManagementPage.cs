using AM.Model.Common;
using AM.PageModel.SysConfig;
using AMControlWinF.Tools;
using AntdUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WinLabel = System.Windows.Forms.Label;

namespace AMControlWinF.Views.SysConfig
{
    /// <summary>
    /// PLC 配置管理页面。
    /// 第一版先完成：
    /// 1. 站/点位列表与搜索；
    /// 2. 站选择驱动点位列表联动；
    /// 3. 配置重载、扫描启停、单轮扫描；
    /// 4. 页面内 Modal 新增/编辑；
    /// 5. 删除与刷新。
    /// </summary>
    public partial class PlcConfigManagementPage : UserControl
    {
        private readonly PlcConfigManagementPageModel _model;

        private bool _isFirstLoad;
        private bool _isBusy;

        private AntList<StationTableRow> _stationTableRows;
        private AntList<PointTableRow> _pointTableRows;

        public PlcConfigManagementPage()
        {
            InitializeComponent();

            _model = new PlcConfigManagementPageModel();
            _stationTableRows = new AntList<StationTableRow>();
            _pointTableRows = new AntList<PointTableRow>();

            InitializeTables();
            BindEvents();
            RefreshActionButtons();
        }

        private void BindEvents()
        {
            Load += PlcConfigManagementPage_Load;

            inputStationSearch.TextChanged += InputStationSearch_TextChanged;
            inputPointSearch.TextChanged += InputPointSearch_TextChanged;

            tableStations.CellClick += TableStations_CellClick;
            tablePoints.CellClick += TablePoints_CellClick;

            buttonRefresh.Click += async (s, e) => await ReloadAsync(_model.SelectedStationName, _model.SelectedPointName);
            buttonReloadConfig.Click += async (s, e) => await ReloadRuntimeConfigAsync();
            buttonScanOnce.Click += async (s, e) => await ExecuteAsync(() => _model.ScanOnce(), _model.SelectedStationName, _model.SelectedPointName);
            buttonStartScan.Click += async (s, e) => await ExecuteAsync(() => _model.StartScan(), _model.SelectedStationName, _model.SelectedPointName);
            buttonStopScan.Click += async (s, e) => await ExecuteAsync(() => _model.StopScan(), _model.SelectedStationName, _model.SelectedPointName);

            buttonAddStation.Click += ButtonAddStation_Click;
            buttonEditStation.Click += ButtonEditStation_Click;
            buttonDeleteStation.Click += async (s, e) => await DeleteSelectedStationAsync();

            buttonAddPoint.Click += ButtonAddPoint_Click;
            buttonEditPoint.Click += ButtonEditPoint_Click;
            buttonDeletePoint.Click += async (s, e) => await DeleteSelectedPointAsync();
        }

        private void InitializeTables()
        {
            tableStations.Columns = new ColumnCollection()
            {
                new Column("DisplayName", "显示名", ColumnAlign.Left)
                {
                    Width = "160",
                    Fixed = true
                },
                new Column("Name", "名称", ColumnAlign.Left)
                {
                    Width = "150"
                },
                new Column("ProtocolType", "协议", ColumnAlign.Center)
                {
                    Width = "110"
                },
                new Column("ConnectionType", "连接", ColumnAlign.Center)
                {
                    Width = "100"
                },
                new Column("EndpointText", "端点", ColumnAlign.Left)
                {
                    Width = "170"
                },
                new Column("RuntimeTag", "运行态", ColumnAlign.Center)
                {
                    Width = "110"
                },
                new Column("PointCountText", "点位数", ColumnAlign.Center)
                {
                    Width = "80"
                }
            };

            tablePoints.Columns = new ColumnCollection()
            {
                new Column("DisplayName", "显示名", ColumnAlign.Left)
                {
                    Width = "150",
                    Fixed = true
                },
                new Column("Name", "名称", ColumnAlign.Left)
                {
                    Width = "150"
                },
                new Column("GroupText", "分组", ColumnAlign.Center)
                {
                    Width = "120"
                },
                new Column("AddressText", "地址", ColumnAlign.Left)
                {
                    Width = "140"
                },
                new Column("DataTypeText", "类型", ColumnAlign.Center)
                {
                    Width = "100"
                },
                new Column("LengthText", "长度", ColumnAlign.Center)
                {
                    Width = "70"
                },
                new Column("AccessModeTag", "访问", ColumnAlign.Center)
                {
                    Width = "110"
                },
                new Column("EnabledTag", "状态", ColumnAlign.Center)
                {
                    Width = "100"
                }
            };
        }

        private async void PlcConfigManagementPage_Load(object sender, EventArgs e)
        {
            if (_isFirstLoad)
            {
                return;
            }

            _isFirstLoad = true;
            await ReloadAsync(null, null);
        }

        private void InputStationSearch_TextChanged(object sender, EventArgs e)
        {
            _model.SetStationSearchText(inputStationSearch.Text);
            RefreshPage();
        }

        private void InputPointSearch_TextChanged(object sender, EventArgs e)
        {
            _model.SetPointSearchText(inputPointSearch.Text);
            RefreshPage();
        }

        private void TableStations_CellClick(object sender, TableClickEventArgs e)
        {
            var row = e == null ? null : e.Record as StationTableRow;
            if (row == null || row.Item == null)
            {
                return;
            }

            _model.SelectStationByName(row.Item.Name);
            RefreshPage();
        }

        private void TablePoints_CellClick(object sender, TableClickEventArgs e)
        {
            var row = e == null ? null : e.Record as PointTableRow;
            if (row == null || row.Item == null)
            {
                return;
            }

            _model.SelectPointByName(row.Item.Name);
            RefreshPage();
        }

        private async void ButtonAddStation_Click(object sender, EventArgs e)
        {
            if (_isBusy)
            {
                return;
            }

            ShowStationEditorDialog(null);
        }

        private async void ButtonEditStation_Click(object sender, EventArgs e)
        {
            if (_isBusy || _model.SelectedStation == null)
            {
                return;
            }

            ShowStationEditorDialog(_model.SelectedStation);
        }

        private async void ButtonAddPoint_Click(object sender, EventArgs e)
        {
            if (_isBusy)
            {
                return;
            }

            if (_model.SelectedStation == null)
            {
                PageDialogHelper.ShowWarn(this, "新增点位", "请先选择一个 PLC 站。");
                return;
            }

            ShowPointEditorDialog(null);
        }

        private async void ButtonEditPoint_Click(object sender, EventArgs e)
        {
            if (_isBusy || _model.SelectedPoint == null)
            {
                return;
            }

            ShowPointEditorDialog(_model.SelectedPoint);
        }

        private async System.Threading.Tasks.Task ReloadAsync(string preferredStationName, string preferredPointName)
        {
            if (_isBusy)
            {
                return;
            }

            SetBusyState(true);
            try
            {
                var result = await _model.LoadAsync(preferredStationName, preferredPointName);
                if (!result.Success)
                {
                    RefreshPage();
                    return;
                }

                RefreshPage();
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private async System.Threading.Tasks.Task ExecuteAsync(Func<Result> action, string preferredStationName, string preferredPointName)
        {
            if (_isBusy || action == null)
            {
                return;
            }

            SetBusyState(true);
            try
            {
                var result = await System.Threading.Tasks.Task.Run(action);
                if (!result.Success)
                {
                    return;
                }

                await ReloadAsync(preferredStationName, preferredPointName);
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private async System.Threading.Tasks.Task ReloadRuntimeConfigAsync()
        {
            await ExecuteAsync(
                () => _model.ReloadConfig(),
                _model.SelectedStationName,
                _model.SelectedPointName);
        }

        private async System.Threading.Tasks.Task DeleteSelectedStationAsync()
        {
            if (_isBusy || _model.SelectedStation == null)
            {
                return;
            }

            bool ok = PageDialogHelper.Confirm(
                this,
                "删除 PLC 站",
                "确定删除 PLC 站 " + _model.SelectedStation.DisplayTitle + " 吗？\r\n若该站下仍存在点位，删除将被拒绝。");

            if (!ok)
            {
                return;
            }

            SetBusyState(true);
            try
            {
                var deleteResult = await System.Threading.Tasks.Task.Run(() => _model.DeleteStation(_model.SelectedStation.Name));
                if (!deleteResult.Success)
                {
                    return;
                }

                var reloadResult = await System.Threading.Tasks.Task.Run(() => _model.ReloadConfig());
                if (!reloadResult.Success)
                {
                    return;
                }

                await ReloadAsync(null, null);
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private async System.Threading.Tasks.Task DeleteSelectedPointAsync()
        {
            if (_isBusy || _model.SelectedPoint == null)
            {
                return;
            }

            bool ok = PageDialogHelper.Confirm(
                this,
                "删除 PLC 点位",
                "确定删除 PLC 点位 " + _model.SelectedPoint.DisplayTitle + " 吗？");

            if (!ok)
            {
                return;
            }

            string stationName = _model.SelectedPoint.PlcName;

            SetBusyState(true);
            try
            {
                var deleteResult = await System.Threading.Tasks.Task.Run(() => _model.DeletePoint(_model.SelectedPoint.PlcName, _model.SelectedPoint.Name));
                if (!deleteResult.Success)
                {
                    return;
                }

                var reloadResult = await System.Threading.Tasks.Task.Run(() => _model.ReloadConfig());
                if (!reloadResult.Success)
                {
                    return;
                }

                await ReloadAsync(stationName, null);
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private void SetBusyState(bool isBusy)
        {
            _isBusy = isBusy;

            inputStationSearch.Enabled = !isBusy;
            inputPointSearch.Enabled = !isBusy;

            buttonRefresh.Enabled = !isBusy;
            buttonReloadConfig.Enabled = !isBusy;
            buttonScanOnce.Enabled = !isBusy;
            buttonStartScan.Enabled = !isBusy;
            buttonStopScan.Enabled = !isBusy;

            RefreshActionButtons();
        }

        private void RefreshActionButtons()
        {
            bool hasStation = _model.SelectedStation != null;
            bool hasPoint = _model.SelectedPoint != null;

            buttonAddStation.Enabled = !_isBusy;
            buttonEditStation.Enabled = !_isBusy && hasStation;
            buttonDeleteStation.Enabled = !_isBusy && hasStation;

            buttonAddPoint.Enabled = !_isBusy && hasStation;
            buttonEditPoint.Enabled = !_isBusy && hasPoint;
            buttonDeletePoint.Enabled = !_isBusy && hasPoint;

            labelSelectedStation.Text = _model.SelectedStationText;
            labelSelectedPoint.Text = _model.SelectedPointText;
        }

        private void RefreshPage()
        {
            labelStationTotalCount.Text = _model.TotalStationCount.ToString();
            labelOnlineStationCount.Text = _model.OnlineStationCount.ToString();
            labelPointTotalCount.Text = _model.TotalPointCount.ToString();
            labelCurrentStationPointCount.Text = _model.CurrentStationPointCount.ToString();
            labelRuntimeSummary.Text = _model.RuntimeSummaryText;

            RebindStationTable();
            RebindPointTable();
            RefreshActionButtons();
        }

        private void RebindStationTable()
        {
            _stationTableRows = new AntList<StationTableRow>();

            foreach (var item in _model.Stations)
            {
                _stationTableRows.Add(new StationTableRow
                {
                    Item = item,
                    Name = new CellText(item.Name ?? string.Empty),
                    DisplayName = new CellText(item.DisplayTitle),
                    ProtocolType = new CellText(string.IsNullOrWhiteSpace(item.ProtocolType) ? "-" : item.ProtocolType),
                    ConnectionType = new CellText(string.IsNullOrWhiteSpace(item.ConnectionType) ? "-" : item.ConnectionType),
                    EndpointText = new CellText(item.EndpointText),
                    RuntimeTag = new CellTag(
                        item.IsConnected ? "在线" : "离线",
                        item.IsConnected ? TTypeMini.Success : TTypeMini.Error),
                    PointCountText = new CellText(item.PointCount.ToString())
                });
            }

            tableStations.Binding(_stationTableRows);
        }

        private void RebindPointTable()
        {
            _pointTableRows = new AntList<PointTableRow>();

            foreach (var item in _model.Points)
            {
                _pointTableRows.Add(new PointTableRow
                {
                    Item = item,
                    Name = new CellText(item.Name ?? string.Empty),
                    DisplayName = new CellText(item.DisplayTitle),
                    GroupText = new CellText(string.IsNullOrWhiteSpace(item.GroupName) ? "-" : item.GroupName),
                    AddressText = new CellText(item.Address ?? string.Empty),
                    DataTypeText = new CellText(item.DataType ?? string.Empty),
                    LengthText = new CellText(item.Length.ToString()),
                    AccessModeTag = BuildAccessModeTag(item.AccessMode),
                    EnabledTag = new CellTag(item.IsEnabled ? "启用" : "禁用", item.IsEnabled ? TTypeMini.Success : TTypeMini.Error)
                });
            }

            tablePoints.Binding(_pointTableRows);
        }

        private static CellTag BuildAccessModeTag(string accessMode)
        {
            string normalized = string.IsNullOrWhiteSpace(accessMode) ? string.Empty : accessMode.Trim();

            switch (normalized)
            {
                case "ReadOnly":
                    return new CellTag("只读", TTypeMini.Default);
                case "WriteOnly":
                    return new CellTag("只写", TTypeMini.Warn);
                default:
                    return new CellTag("读写", TTypeMini.Primary);
            }
        }

        private void ShowStationEditorDialog(PlcConfigManagementPageModel.StationViewItem item)
        {
            var editorModel = new PlcStationEditorModel();
            if (item == null)
            {
                editorModel.ResetForCreate();
            }
            else
            {
                editorModel.LoadFrom(item.ToEntity());
            }

            Action applyValues;
            Control content = BuildStationEditorContent(editorModel, out applyValues);

            AntdUI.Modal.open(new AntdUI.Modal.Config(FindForm(), item == null ? "新增 PLC 站" : "编辑 PLC 站", (object)content)
            {
                Width = 920,
                OkText = "保存",
                CancelText = "取消",
                CloseIcon = true,
                Keyboard = true,
                MaskClosable = false,
                Draggable = true,
                DefaultFocus = false,
                OnOk = cfg =>
                {
                    applyValues();

                    var entityResult = editorModel.BuildEntity();
                    if (!entityResult.Success)
                    {
                        PageDialogHelper.ShowWarn(this, "PLC 站校验", entityResult.Message);
                        return false;
                    }

                    var saveResult = _model.SaveStation(entityResult.Item);
                    if (!saveResult.Success)
                    {
                        return false;
                    }

                    var reloadResult = _model.ReloadConfig();
                    if (!reloadResult.Success)
                    {
                        return false;
                    }

                    BeginInvoke(new Action(() => ReloadAfterSave(entityResult.Item.Name, null)));
                    return true;
                }
            });
        }

        private void ShowPointEditorDialog(PlcConfigManagementPageModel.PointViewItem item)
        {
            var editorModel = new PlcPointEditorModel();
            if (item == null)
            {
                editorModel.ResetForCreate();
                editorModel.PlcName = _model.SelectedStation == null ? string.Empty : _model.SelectedStation.Name;
            }
            else
            {
                editorModel.LoadFrom(item.ToEntity());
            }

            Action applyValues;
            Control content = BuildPointEditorContent(editorModel, _model.Stations.Select(x => x.Name).ToList(), out applyValues);

            AntdUI.Modal.open(new AntdUI.Modal.Config(FindForm(), item == null ? "新增 PLC 点位" : "编辑 PLC 点位", (object)content)
            {
                Width = 900,
                OkText = "保存",
                CancelText = "取消",
                CloseIcon = true,
                Keyboard = true,
                MaskClosable = false,
                Draggable = true,
                DefaultFocus = false,
                OnOk = cfg =>
                {
                    applyValues();

                    var entityResult = editorModel.BuildEntity();
                    if (!entityResult.Success)
                    {
                        PageDialogHelper.ShowWarn(this, "PLC 点位校验", entityResult.Message);
                        return false;
                    }

                    var saveResult = _model.SavePoint(entityResult.Item);
                    if (!saveResult.Success)
                    {
                        return false;
                    }

                    var reloadResult = _model.ReloadConfig();
                    if (!reloadResult.Success)
                    {
                        return false;
                    }

                    BeginInvoke(new Action(() => ReloadAfterSave(entityResult.Item.PlcName, entityResult.Item.Name)));
                    return true;
                }
            });
        }

        private async void ReloadAfterSave(string stationName, string pointName)
        {
            await ReloadAsync(stationName, pointName);
        }

        private Control BuildStationEditorContent(PlcStationEditorModel model, out Action applyValues)
        {
            var panel = new System.Windows.Forms.Panel();
            panel.Dock = DockStyle.Fill;
            panel.Padding = new Padding(0);

            var scrollHost = new System.Windows.Forms.Panel();
            scrollHost.Dock = DockStyle.Fill;
            scrollHost.AutoScroll = true;

            var layout = new TableLayoutPanel();
            layout.AutoSize = true;
            layout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            layout.ColumnCount = 4;
            layout.RowCount = 0;
            layout.Dock = DockStyle.Top;
            layout.Padding = new Padding(8);
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            var txtName = CreateEditorTextBox(model.Name);
            var txtDisplayName = CreateEditorTextBox(model.DisplayName);
            var txtVendor = CreateEditorTextBox(model.Vendor);
            var txtModel = CreateEditorTextBox(model.Model);
            var cmbConnectionType = CreateEditorComboBox(PlcStationEditorModel.ConnectionTypes, model.ConnectionType);
            var cmbProtocolType = CreateEditorComboBox(PlcStationEditorModel.ProtocolTypes, model.ProtocolType);
            var txtIp = CreateEditorTextBox(model.IpAddress);
            var txtPort = CreateEditorTextBox(model.PortText);
            var txtComPort = CreateEditorTextBox(model.ComPort);
            var txtBaudRate = CreateEditorTextBox(model.BaudRateText);
            var txtDataBits = CreateEditorTextBox(model.DataBitsText);
            var cmbParity = CreateEditorComboBox(PlcStationEditorModel.ParityOptions, model.Parity);
            var cmbStopBits = CreateEditorComboBox(PlcStationEditorModel.StopBitsOptions, model.StopBits);
            var txtStationNo = CreateEditorTextBox(model.StationNoText);
            var txtNetworkNo = CreateEditorTextBox(model.NetworkNoText);
            var txtPcNo = CreateEditorTextBox(model.PcNoText);
            var txtRack = CreateEditorTextBox(model.RackText);
            var txtSlot = CreateEditorTextBox(model.SlotText);
            var txtTimeoutMs = CreateEditorTextBox(model.TimeoutMsText);
            var txtReconnectMs = CreateEditorTextBox(model.ReconnectIntervalMsText);
            var txtScanMs = CreateEditorTextBox(model.ScanIntervalMsText);
            var txtSortOrder = CreateEditorTextBox(model.SortOrderText);
            var chkEnabled = CreateEditorCheckBox("启用该 PLC 站", model.IsEnabled);
            var txtDescription = CreateEditorMultiLineTextBox(model.Description);
            var txtRemark = CreateEditorMultiLineTextBox(model.Remark);

            AddEditorRow(layout, "名称", txtName, "显示名", txtDisplayName);
            AddEditorRow(layout, "厂商", txtVendor, "型号", txtModel);
            AddEditorRow(layout, "连接方式", cmbConnectionType, "协议类型", cmbProtocolType);
            AddEditorRow(layout, "IP地址", txtIp, "端口", txtPort);
            AddEditorRow(layout, "串口号", txtComPort, "波特率", txtBaudRate);
            AddEditorRow(layout, "数据位", txtDataBits, "校验位", cmbParity);
            AddEditorRow(layout, "停止位", cmbStopBits, "站号", txtStationNo);
            AddEditorRow(layout, "网络号", txtNetworkNo, "PC号", txtPcNo);
            AddEditorRow(layout, "机架号", txtRack, "插槽号", txtSlot);
            AddEditorRow(layout, "通讯超时", txtTimeoutMs, "重连周期", txtReconnectMs);
            AddEditorRow(layout, "扫描周期", txtScanMs, "排序号", txtSortOrder);
            AddSingleEditorRow(layout, "启用", chkEnabled, 3);
            AddMultiLineEditorRow(layout, "描述", txtDescription);
            AddMultiLineEditorRow(layout, "备注", txtRemark);

            scrollHost.Controls.Add(layout);
            panel.Controls.Add(scrollHost);

            applyValues = () =>
            {
                model.Name = txtName.Text;
                model.DisplayName = txtDisplayName.Text;
                model.Vendor = txtVendor.Text;
                model.Model = txtModel.Text;
                model.ConnectionType = cmbConnectionType.Text;
                model.ProtocolType = cmbProtocolType.Text;
                model.IpAddress = txtIp.Text;
                model.PortText = txtPort.Text;
                model.ComPort = txtComPort.Text;
                model.BaudRateText = txtBaudRate.Text;
                model.DataBitsText = txtDataBits.Text;
                model.Parity = cmbParity.Text;
                model.StopBits = cmbStopBits.Text;
                model.StationNoText = txtStationNo.Text;
                model.NetworkNoText = txtNetworkNo.Text;
                model.PcNoText = txtPcNo.Text;
                model.RackText = txtRack.Text;
                model.SlotText = txtSlot.Text;
                model.TimeoutMsText = txtTimeoutMs.Text;
                model.ReconnectIntervalMsText = txtReconnectMs.Text;
                model.ScanIntervalMsText = txtScanMs.Text;
                model.SortOrderText = txtSortOrder.Text;
                model.IsEnabled = chkEnabled.Checked;
                model.Description = txtDescription.Text;
                model.Remark = txtRemark.Text;
            };

            return panel;
        }

        private Control BuildPointEditorContent(PlcPointEditorModel model, IList<string> stationNames, out Action applyValues)
        {
            var panel = new System.Windows.Forms.Panel();
            panel.Dock = DockStyle.Fill;

            var scrollHost = new System.Windows.Forms.Panel();
            scrollHost.Dock = DockStyle.Fill;
            scrollHost.AutoScroll = true;

            var layout = new TableLayoutPanel();
            layout.AutoSize = true;
            layout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            layout.ColumnCount = 4;
            layout.RowCount = 0;
            layout.Dock = DockStyle.Top;
            layout.Padding = new Padding(8);
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            var cmbPlcName = CreateEditorComboBox(stationNames, model.PlcName);
            var txtName = CreateEditorTextBox(model.Name);
            var txtDisplayName = CreateEditorTextBox(model.DisplayName);
            var txtGroup = CreateEditorTextBox(model.GroupName);
            var txtAddress = CreateEditorTextBox(model.Address);
            var cmbDataType = CreateEditorComboBox(PlcPointEditorModel.DataTypes, model.DataType);
            var txtLength = CreateEditorTextBox(model.LengthText);
            var cmbAccessMode = CreateEditorComboBox(PlcPointEditorModel.AccessModes, model.AccessMode);
            var txtSortOrder = CreateEditorTextBox(model.SortOrderText);
            var chkEnabled = CreateEditorCheckBox("启用该点位", model.IsEnabled);
            var txtDescription = CreateEditorMultiLineTextBox(model.Description);
            var txtRemark = CreateEditorMultiLineTextBox(model.Remark);

            AddEditorRow(layout, "所属PLC", cmbPlcName, "名称", txtName);
            AddEditorRow(layout, "显示名", txtDisplayName, "分组", txtGroup);
            AddEditorRow(layout, "地址", txtAddress, "数据类型", cmbDataType);
            AddEditorRow(layout, "Length", txtLength, "访问模式", cmbAccessMode);
            AddEditorRow(layout, "排序号", txtSortOrder, "状态", chkEnabled);
            AddMultiLineEditorRow(layout, "描述", txtDescription);
            AddMultiLineEditorRow(layout, "备注", txtRemark);

            scrollHost.Controls.Add(layout);
            panel.Controls.Add(scrollHost);

            applyValues = () =>
            {
                model.PlcName = cmbPlcName.Text;
                model.Name = txtName.Text;
                model.DisplayName = txtDisplayName.Text;
                model.GroupName = txtGroup.Text;
                model.Address = txtAddress.Text;
                model.DataType = cmbDataType.Text;
                model.LengthText = txtLength.Text;
                model.AccessMode = cmbAccessMode.Text;
                model.SortOrderText = txtSortOrder.Text;
                model.IsEnabled = chkEnabled.Checked;
                model.Description = txtDescription.Text;
                model.Remark = txtRemark.Text;
            };

            return panel;
        }

        private static TextBox CreateEditorTextBox(string value)
        {
            return new TextBox
            {
                Text = value ?? string.Empty,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 4, 0, 4)
            };
        }

        private static TextBox CreateEditorMultiLineTextBox(string value)
        {
            return new TextBox
            {
                Text = value ?? string.Empty,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 4, 0, 4),
                Multiline = true,
                Height = 72,
                ScrollBars = ScrollBars.Vertical
            };
        }

        private static ComboBox CreateEditorComboBox(IEnumerable<string> items, string value)
        {
            var combo = new ComboBox
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 4, 0, 4),
                DropDownStyle = ComboBoxStyle.DropDown
            };

            if (items != null)
            {
                foreach (var item in items.Distinct(StringComparer.OrdinalIgnoreCase))
                {
                    combo.Items.Add(item);
                }
            }

            combo.Text = value ?? string.Empty;
            return combo;
        }

        private static CheckBox CreateEditorCheckBox(string text, bool isChecked)
        {
            return new CheckBox
            {
                Text = text ?? string.Empty,
                Checked = isChecked,
                Dock = DockStyle.Left,
                AutoSize = true,
                Margin = new Padding(0, 8, 0, 4)
            };
        }

        private static WinLabel CreateEditorLabel(string text)
        {
            return new WinLabel
            {
                Text = text ?? string.Empty,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = false,
                Margin = new Padding(0, 4, 8, 4),
                Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular)
            };
        }

        private static void AddEditorRow(TableLayoutPanel layout, string leftLabel, Control leftControl, string rightLabel, Control rightControl)
        {
            int rowIndex = layout.RowCount;
            layout.RowCount += 1;
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            layout.Controls.Add(CreateEditorLabel(leftLabel), 0, rowIndex);
            layout.Controls.Add(leftControl, 1, rowIndex);
            layout.Controls.Add(CreateEditorLabel(rightLabel), 2, rowIndex);
            layout.Controls.Add(rightControl, 3, rowIndex);
        }

        private static void AddSingleEditorRow(TableLayoutPanel layout, string label, Control control, int columnSpan)
        {
            int rowIndex = layout.RowCount;
            layout.RowCount += 1;
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            layout.Controls.Add(CreateEditorLabel(label), 0, rowIndex);
            layout.Controls.Add(control, 1, rowIndex);
            layout.SetColumnSpan(control, columnSpan);
        }

        private static void AddMultiLineEditorRow(TableLayoutPanel layout, string label, Control control)
        {
            int rowIndex = layout.RowCount;
            layout.RowCount += 1;
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            layout.Controls.Add(CreateEditorLabel(label), 0, rowIndex);
            layout.Controls.Add(control, 1, rowIndex);
            layout.SetColumnSpan(control, 3);
        }

        private sealed class StationTableRow
        {
            public PlcConfigManagementPageModel.StationViewItem Item { get; set; }

            public CellText Name { get; set; }

            public CellText DisplayName { get; set; }

            public CellText ProtocolType { get; set; }

            public CellText ConnectionType { get; set; }

            public CellText EndpointText { get; set; }

            public CellTag RuntimeTag { get; set; }

            public CellText PointCountText { get; set; }
        }

        private sealed class PointTableRow
        {
            public PlcConfigManagementPageModel.PointViewItem Item { get; set; }

            public CellText Name { get; set; }

            public CellText DisplayName { get; set; }

            public CellText GroupText { get; set; }

            public CellText AddressText { get; set; }

            public CellText DataTypeText { get; set; }

            public CellText LengthText { get; set; }

            public CellTag AccessModeTag { get; set; }

            public CellTag EnabledTag { get; set; }
        }
    }
}