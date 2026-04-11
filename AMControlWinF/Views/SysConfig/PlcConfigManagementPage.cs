using AM.Model.Common;
using AM.Model.Entity.Plc;
using AM.PageModel.SysConfig;
using AMControlWinF.Tools;
using AntdUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMControlWinF.Views.SysConfig
{
    /// <summary>
    /// PLC 配置管理页面。
    /// 当前职责：
    /// 1. 站/点位列表与搜索；
    /// 2. 站选择驱动点位列表联动；
    /// 3. 配置重载、扫描启停、单轮扫描；
    /// 4. 打开独立新增/编辑对话框；
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
                    Width = "110",
                    Fixed = true
                },
                new Column("ProtocolType", "协议", ColumnAlign.Center)
                {
                    Width = "90"
                },
                new Column("ConnectionType", "连接", ColumnAlign.Center)
                {
                    Width = "80"
                },
                new Column("EndpointText", "端点", ColumnAlign.Left)
                {
                    Width = "150"
                },
                new Column("RuntimeTag", "运行态", ColumnAlign.Center)
                {
                    Width = "70",
                    Fixed = true
                },
                new Column("PointCountText", "点位数", ColumnAlign.Center)
                {
                    Width = "80"
                },
                new Column("Name", "名称", ColumnAlign.Left)
                {
                    Width = "150"
                }
            };

            tablePoints.Columns = new ColumnCollection()
            {
                new Column("DisplayName", "显示名", ColumnAlign.Left)
                {
                    Width = "110",
                    Fixed = true
                },
                new Column("AddressText", "地址", ColumnAlign.Left)
                {
                    Width = "90"
                },
                new Column("DataTypeText", "类型", ColumnAlign.Center)
                {
                    Width = "80"
                },
                new Column("LengthText", "长度", ColumnAlign.Center)
                {
                    Width = "50"
                },
                new Column("AccessModeTag", "访问", ColumnAlign.Center)
                {
                    Width = "80"
                },
                new Column("EnabledTag", "状态", ColumnAlign.Center)
                {
                    Width = "70"
                },
                new Column("Name", "名称", ColumnAlign.Left)
                {
                    Width = "120"
                },
                new Column("GroupText", "分组", ColumnAlign.Center)
                {
                    Width = "80"
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

            await EditStationAsync(null);
        }

        private async void ButtonEditStation_Click(object sender, EventArgs e)
        {
            if (_isBusy || _model.SelectedStation == null)
            {
                return;
            }

            await EditStationAsync(_model.SelectedStation);
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

            await EditPointAsync(null);
        }

        private async void ButtonEditPoint_Click(object sender, EventArgs e)
        {
            if (_isBusy || _model.SelectedPoint == null)
            {
                return;
            }

            await EditPointAsync(_model.SelectedPoint);
        }

        /// <summary>
        /// 对外刷新入口：
        /// - 负责防重入；
        /// - 负责统一设置忙碌状态；
        /// - 真正的刷新逻辑下沉到 ReloadCoreAsync。
        /// </summary>
        private async Task ReloadAsync(string preferredStationName, string preferredPointName)
        {
            if (_isBusy)
            {
                return;
            }

            SetBusyState(true);
            try
            {
                await ReloadCoreAsync(preferredStationName, preferredPointName);
            }
            finally
            {
                SetBusyState(false);
            }
        }

        /// <summary>
        /// 内部刷新核心逻辑：
        /// - 不再判断 _isBusy；
        /// - 供已经处于忙碌流程中的方法复用；
        /// - 避免保存后调用 ReloadAsync 被 _isBusy 拦截。
        /// </summary>
        private async Task ReloadCoreAsync(string preferredStationName, string preferredPointName)
        {
            var result = await _model.LoadAsync(preferredStationName, preferredPointName);
            if (!result.Success)
            {
                RefreshPage();
                return;
            }

            RefreshPage();
        }

        private async Task ExecuteAsync(Func<Result> action, string preferredStationName, string preferredPointName)
        {
            if (_isBusy || action == null)
            {
                return;
            }

            SetBusyState(true);
            try
            {
                var result = await Task.Run(action);
                if (!result.Success)
                {
                    return;
                }

                await ReloadCoreAsync(preferredStationName, preferredPointName);
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private async Task ReloadRuntimeConfigAsync()
        {
            await ExecuteAsync(
                () => _model.ReloadConfig(),
                _model.SelectedStationName,
                _model.SelectedPointName);
        }

        private async Task EditStationAsync(PlcConfigManagementPageModel.StationViewItem item)
        {
            using (var dialog = new PlcStationEditDialog())
            {
                dialog.IsCreateMode = item == null;
                dialog.SetEntity(item == null ? null : item.ToEntity());

                if (dialog.ShowDialog(FindForm()) != DialogResult.OK || dialog.ResultEntity == null)
                {
                    return;
                }

                SetBusyState(true);
                try
                {
                    var saveResult = await Task.Run(() => _model.SaveStation(dialog.ResultEntity));
                    if (!saveResult.Success)
                    {
                        return;
                    }

                    var reloadResult = await Task.Run(() => _model.ReloadConfig());
                    if (!reloadResult.Success)
                    {
                        return;
                    }

                    await ReloadCoreAsync(dialog.ResultEntity.Name, null);
                }
                finally
                {
                    SetBusyState(false);
                }
            }
        }

        private async Task EditPointAsync(PlcConfigManagementPageModel.PointViewItem item)
        {
            using (var dialog = new PlcPointEditDialog())
            {
                dialog.IsCreateMode = item == null;
                dialog.SetStationNames(BuildStationNameList(item == null ? null : item.PlcName));
                dialog.SetEntity(item == null ? BuildDefaultPointEntity() : item.ToEntity());

                if (dialog.ShowDialog(FindForm()) != DialogResult.OK || dialog.ResultEntity == null)
                {
                    return;
                }

                SetBusyState(true);
                try
                {
                    var saveResult = await Task.Run(() => _model.SavePoint(dialog.ResultEntity));
                    if (!saveResult.Success)
                    {
                        return;
                    }

                    var reloadResult = await Task.Run(() => _model.ReloadConfig());
                    if (!reloadResult.Success)
                    {
                        return;
                    }

                    await ReloadCoreAsync(dialog.ResultEntity.PlcName, dialog.ResultEntity.Name);
                }
                finally
                {
                    SetBusyState(false);
                }
            }
        }

        private PlcPointConfigEntity BuildDefaultPointEntity()
        {
            return new PlcPointConfigEntity
            {
                PlcName = _model.SelectedStation == null ? string.Empty : _model.SelectedStation.Name,
                Name = string.Empty,
                DisplayName = string.Empty,
                GroupName = "Default",
                Address = string.Empty,
                DataType = "bool",
                Length = 1,
                AccessMode = "ReadWrite",
                IsEnabled = true,
                SortOrder = 1,
                Description = string.Empty,
                Remark = string.Empty
            };
        }

        private IList<string> BuildStationNameList(string extraStationName)
        {
            var list = new List<string>();

            foreach (var item in _model.Stations)
            {
                if (item == null || string.IsNullOrWhiteSpace(item.Name))
                {
                    continue;
                }

                if (!list.Any(x => string.Equals(x, item.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    list.Add(item.Name);
                }
            }

            if (_model.SelectedStation != null && !string.IsNullOrWhiteSpace(_model.SelectedStation.Name))
            {
                if (!list.Any(x => string.Equals(x, _model.SelectedStation.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    list.Add(_model.SelectedStation.Name);
                }
            }

            if (!string.IsNullOrWhiteSpace(extraStationName))
            {
                if (!list.Any(x => string.Equals(x, extraStationName, StringComparison.OrdinalIgnoreCase)))
                {
                    list.Add(extraStationName);
                }
            }

            return list.OrderBy(x => x).ToList();
        }

        private async Task DeleteSelectedStationAsync()
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
                var deleteResult = await Task.Run(() => _model.DeleteStation(_model.SelectedStation.Name));
                if (!deleteResult.Success)
                {
                    return;
                }

                var reloadResult = await Task.Run(() => _model.ReloadConfig());
                if (!reloadResult.Success)
                {
                    return;
                }

                await ReloadCoreAsync(null, null);
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private async Task DeleteSelectedPointAsync()
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
                var deleteResult = await Task.Run(() => _model.DeletePoint(_model.SelectedPoint.PlcName, _model.SelectedPoint.Name));
                if (!deleteResult.Success)
                {
                    return;
                }

                var reloadResult = await Task.Run(() => _model.ReloadConfig());
                if (!reloadResult.Success)
                {
                    return;
                }

                await ReloadCoreAsync(stationName, null);
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