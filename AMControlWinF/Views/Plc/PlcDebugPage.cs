using AM.Model.Common;
using AM.PageModel.Plc;
using AntdUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMControlWinF.Views.Plc
{
    /// <summary>
    /// PLC 调试页面。
    /// 三行布局：
    /// 1. 顶部工具栏；
    /// 2. 中间空占位；
    /// 3. 左侧调试操作区，右侧执行结果历史表。
    /// </summary>
    public partial class PlcDebugPage : UserControl
    {
        private readonly PlcDebugPageModel _model;

        private bool _isFirstLoad;
        private bool _isBusy;
        private bool _isBindingView;

        private AntList<ResultTableRow> _resultTableRows;

        public PlcDebugPage()
        {
            InitializeComponent();

            _model = new PlcDebugPageModel();
            _resultTableRows = new AntList<ResultTableRow>();

            InitializeTables();
            ApplyStaticViewState();
            BindEvents();
            RefreshOperationState();
        }

        private void InitializeTables()
        {
            tableResults.Columns = new ColumnCollection()
            {
                new Column("TimeText", "时间", ColumnAlign.Center)
                {
                    Width = "135",
                    Fixed = true
                },
                new Column("ResultTag", "结果", ColumnAlign.Center)
                {
                    Width = "65"
                },
                new Column("TargetText", "目标", ColumnAlign.Left)
                {
                    Width = "85"
                },
                new Column("DataTypeText", "类型", ColumnAlign.Left)
                {
                    Width = "95"
                },
                new Column("ValueText", "值", ColumnAlign.Left)
                {
                    Width = "100"
                },
                new Column("ActionName", "操作", ColumnAlign.Left)
                {
                    Width = "120"
                },
                new Column("TargetModeTag", "模式", ColumnAlign.Center)
                {
                    Width = "95"
                },
                new Column("QualityTag", "质量", ColumnAlign.Center)
                {
                    Width = "80"
                },
                new Column("PlcNameTag", "PLC站", ColumnAlign.Center)
                {
                    Width = "130"
                },
                new Column("MessageText", "消息", ColumnAlign.Left)
                {
                    Width = "260"
                }
            };
        }

        /// <summary>
        /// 配置点位元信息只读展示，不允许在调试页直接修改配置。
        /// </summary>
        private void ApplyStaticViewState()
        {
            inputPointAddress.Enabled = false;
            inputPointDataType.Enabled = false;
            inputPointLength.Enabled = false;
        }

        private void BindEvents()
        {
            Load += PlcDebugPage_Load;

            buttonRefresh.Click += async (s, e) => await ReloadAsync();

            selectPlcGlobal.SelectedIndexChanged += (s, e) =>
            {
                if (_isBindingView)
                {
                    return;
                }

                _model.SetSelectedPlc(selectPlcGlobal.SelectedValue == null
                    ? null
                    : selectPlcGlobal.SelectedValue.ToString());

                RefreshView();
            };

            selectPoint.SelectedIndexChanged += (s, e) =>
            {
                if (_isBindingView)
                {
                    return;
                }

                _model.SetSelectedPoint(selectPoint.SelectedValue == null
                    ? null
                    : selectPoint.SelectedValue.ToString());

                RefreshView();
            };

            selectDirectDataType.SelectedIndexChanged += (s, e) =>
            {
                if (_isBindingView)
                {
                    return;
                }

                _model.DirectDataType = selectDirectDataType.SelectedValue == null
                    ? string.Empty
                    : selectDirectDataType.SelectedValue.ToString();

                RefreshOperationState();
            };

            inputPointWriteValue.TextChanged += (s, e) =>
            {
                if (_isBindingView)
                {
                    return;
                }

                _model.ConfigWriteValueText = inputPointWriteValue.Text;
            };

            inputDirectAddress.TextChanged += (s, e) =>
            {
                if (_isBindingView)
                {
                    return;
                }

                _model.DirectAddress = inputDirectAddress.Text;
                RefreshOperationState();
            };

            inputDirectLength.TextChanged += (s, e) =>
            {
                if (_isBindingView)
                {
                    return;
                }

                _model.DirectLength = ParseLength(inputDirectLength.Text);
                RefreshOperationState();
            };

            inputDirectWriteValue.TextChanged += (s, e) =>
            {
                if (_isBindingView)
                {
                    return;
                }

                _model.DirectWriteValueText = inputDirectWriteValue.Text;
                RefreshOperationState();
            };

            checkDirectWriteConfirmed.CheckedChanged += (s, e) =>
            {
                if (_isBindingView)
                {
                    return;
                }

                _model.DirectWriteConfirmed = checkDirectWriteConfirmed.Checked;
                RefreshOperationState();
            };

            buttonTestReadPoint.Click += async (s, e) => await ExecuteAsync(() => _model.TestReadPoint());
            buttonWritePoint.Click += async (s, e) => await ExecuteAsync(() => _model.WritePoint());
            buttonTestReadAddress.Click += async (s, e) => await ExecuteAsync(() => _model.TestReadAddress());
            buttonWriteAddress.Click += async (s, e) => await ExecuteAsync(() => _model.WriteAddress());
        }

        private async void PlcDebugPage_Load(object sender, EventArgs e)
        {
            if (_isFirstLoad)
            {
                return;
            }

            _isFirstLoad = true;
            await ReloadAsync();
        }

        private async Task ReloadAsync()
        {
            if (_isBusy)
            {
                return;
            }

            SetBusyState(true);
            try
            {
                Result result = await _model.LoadAsync();
                RefreshView();

                if (!result.Success)
                {
                    return;
                }
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private async Task ExecuteAsync(Func<Result> action)
        {
            if (_isBusy || action == null)
            {
                return;
            }

            SetBusyState(true);
            try
            {
                Result result = await Task.Run(action);
                RefreshView();

                if (!result.Success)
                {
                    return;
                }
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private void RefreshView()
        {
            _isBindingView = true;
            try
            {
                labelRuntimeSummary.Text = _model.RuntimeSummaryText;
                labelHistorySummary.Text = string.Format("最近 {0} 条执行结果", _model.ResultHistoryCount);

                BindSelectItems(
                    selectPlcGlobal,
                    "请选择 PLC",
                    _model.PlcOptions,
                    _model.SelectedPlcName);

                BindSelectItems(
                    selectPoint,
                    "请选择配置点位",
                    _model.PointOptions.Select(x => x.DisplayTitle).ToList(),
                    _model.SelectedPointName,
                    _model.PointOptions.Select(x => (object)x.PointName).ToList());

                BindSelectItems(
                    selectDirectDataType,
                    "请选择数据类型",
                    _model.DirectDataTypeOptions,
                    _model.DirectDataType);

                BindConfigPointArea();
                BindDirectAddressArea();
                BindResultTable();
                RefreshOperationState();
            }
            finally
            {
                _isBindingView = false;
            }
        }

        private void BindConfigPointArea()
        {
            inputPointAddress.Text = _model.ConfigPointAddress;
            inputPointDataType.Text = BuildConfigPointDataTypeText();
            inputPointLength.Text = _model.ConfigPointLength.ToString();
            inputPointWriteValue.Text = _model.ConfigWriteValueText;
            labelPointOpTitle.Text = BuildPointOperationTitle();
        }

        private void BindDirectAddressArea()
        {
            inputDirectAddress.Text = _model.DirectAddress;
            inputDirectLength.Text = _model.DirectLength.ToString();
            inputDirectWriteValue.Text = _model.DirectWriteValueText;
            checkDirectWriteConfirmed.Checked = _model.DirectWriteConfirmed;
        }

        private void BindResultTable()
        {
            _resultTableRows = new AntList<ResultTableRow>();

            foreach (PlcDebugPageModel.DebugResultItem item in _model.ResultHistory)
            {
                _resultTableRows.Add(new ResultTableRow
                {
                    Item = item,
                    TimeText = new CellText(item == null ? "-" : item.TimeText),
                    ResultTag = BuildResultTag(item),
                    TargetText = new CellText(BuildTargetText(item)),
                    DataTypeText = new CellText(BuildDataTypeText(item)),
                    ValueText = new CellText(string.IsNullOrWhiteSpace(item == null ? null : item.ValueText) ? "-" : item.ValueText),
                    ActionName = new CellText(string.IsNullOrWhiteSpace(item == null ? null : item.ActionName) ? "-" : item.ActionName),
                    TargetModeTag = BuildTargetModeTag(item),
                    QualityTag = BuildQualityTag(item),
                    PlcNameTag = BuildPlcNameTag(item),
                    MessageText = new CellText(string.IsNullOrWhiteSpace(item == null ? null : item.Message) ? "-" : item.Message)
                });
            }

            tableResults.Binding(_resultTableRows);
        }

        private void RefreshOperationState()
        {
            buttonRefresh.Enabled = !_isBusy;

            selectPlcGlobal.Enabled = !_isBusy;
            selectPoint.Enabled = !_isBusy;

            buttonTestReadPoint.Enabled = !_isBusy && _model.CanReadSelectedPoint;
            buttonWritePoint.Enabled = !_isBusy && _model.CanWriteSelectedPoint;
            inputPointWriteValue.Enabled = !_isBusy && _model.CanWriteSelectedPoint;

            inputDirectAddress.Enabled = !_isBusy;
            selectDirectDataType.Enabled = !_isBusy;
            inputDirectLength.Enabled = !_isBusy;
            inputDirectWriteValue.Enabled = !_isBusy;
            checkDirectWriteConfirmed.Enabled = !_isBusy;

            buttonTestReadAddress.Enabled = !_isBusy && _model.CanReadDirectAddress;
            buttonWriteAddress.Enabled = !_isBusy && _model.CanWriteDirectAddress;
        }

        private string BuildPointOperationTitle()
        {
            if (!_model.IsPointSelected)
            {
                return "按配置点位调试";
            }

            return string.Format(
                "按配置点位调试（{0} / {1}）",
                string.IsNullOrWhiteSpace(_model.ConfigPointAccessModeText) ? "-" : _model.ConfigPointAccessModeText,
                _model.ConfigPointEnabled ? "已启用" : "未启用");
        }

        private string BuildConfigPointDataTypeText()
        {
            if (string.IsNullOrWhiteSpace(_model.ConfigPointDataType))
            {
                return "—";
            }

            return _model.ConfigPointDataType;
        }

        private static void BindSelectItems(
            Select select,
            string emptyText,
            IList<string> texts,
            string selectedValue)
        {
            BindSelectItems(
                select,
                emptyText,
                texts,
                selectedValue,
                texts == null ? new List<object>() : texts.Cast<object>().ToList());
        }

        private static void BindSelectItems(
            Select select,
            string emptyText,
            IList<string> texts,
            string selectedValue,
            IList<object> values)
        {
            if (select == null)
            {
                return;
            }

            List<SelectItem> items = new List<SelectItem>();
            items.Add(new SelectItem(emptyText, string.Empty));

            if (texts != null && values != null)
            {
                int count = Math.Min(texts.Count, values.Count);
                for (int i = 0; i < count; i++)
                {
                    if (string.IsNullOrWhiteSpace(texts[i]))
                    {
                        continue;
                    }

                    items.Add(new SelectItem(texts[i], values[i] ?? string.Empty));
                }
            }

            select.Items.Clear();
            select.Items.AddRange(items.ToArray());

            string normalized = string.IsNullOrWhiteSpace(selectedValue)
                ? string.Empty
                : selectedValue.Trim();

            if (!string.Equals(
                select.SelectedValue == null ? string.Empty : select.SelectedValue.ToString(),
                normalized,
                StringComparison.OrdinalIgnoreCase))
            {
                select.SelectedValue = normalized;
            }
        }

        private static int ParseLength(string text)
        {
            int value;
            return int.TryParse(text, out value) && value > 0 ? value : 1;
        }

        private void SetBusyState(bool isBusy)
        {
            _isBusy = isBusy;
            RefreshOperationState();
        }

        #region Table行构建和样式

        private static CellTag BuildResultTag(PlcDebugPageModel.DebugResultItem item)
        {
            bool success = item != null && item.Success;
            return new CellTag(
                success ? "成功" : "失败",
                success ? TTypeMini.Success : TTypeMini.Error);
        }

        private static string BuildTargetText(PlcDebugPageModel.DebugResultItem item)
        {
            if (item == null)
            {
                return "-";
            }

            if (!string.IsNullOrWhiteSpace(item.PointDisplayName))
            {
                return item.PointDisplayName;
            }

            if (!string.IsNullOrWhiteSpace(item.PointName))
            {
                return item.PointName;
            }

            if (!string.IsNullOrWhiteSpace(item.Address))
            {
                return item.Address;
            }

            return "-";
        }

        private static CellTag BuildTargetModeTag(PlcDebugPageModel.DebugResultItem item)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.TargetMode))
            {
                return new CellTag("-", TTypeMini.Default);
            }

            if (string.Equals(item.TargetMode, "ConfigPoint", StringComparison.OrdinalIgnoreCase))
            {
                return new CellTag("配置点位", TTypeMini.Primary);
            }

            if (string.Equals(item.TargetMode, "DirectAddress", StringComparison.OrdinalIgnoreCase))
            {
                return new CellTag("直接地址", TTypeMini.Warn);
            }

            return new CellTag(item.TargetMode, TTypeMini.Default);
        }

        private static string BuildDataTypeText(PlcDebugPageModel.DebugResultItem item)
        {
            if (item == null)
            {
                return "-";
            }

            string type = string.IsNullOrWhiteSpace(item.DataType) ? "-" : item.DataType;
            string length = item.Length > 0 ? item.Length.ToString() : "-";
            string access = string.IsNullOrWhiteSpace(item.AccessModeText) ? "-" : item.AccessModeText;

            return string.Format("{0}/{1}/{2}", type, length, access);
        }

        private static CellTag BuildPlcNameTag(PlcDebugPageModel.DebugResultItem item)
        {
            string plcName = item == null || string.IsNullOrWhiteSpace(item.PlcName)
                ? "-"
                : item.PlcName;

            return new CellTag(
                plcName,
                plcName == "-" ? TTypeMini.Default : TTypeMini.Primary);
        }

        private static CellTag BuildQualityTag(PlcDebugPageModel.DebugResultItem item)
        {
            string quality = item == null || string.IsNullOrWhiteSpace(item.Quality)
                ? "-"
                : item.Quality;

            if (string.Equals(quality, "Good", StringComparison.OrdinalIgnoreCase))
            {
                return new CellTag("Good", TTypeMini.Success);
            }

            if (string.Equals(quality, "Error", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(quality, "Bad", StringComparison.OrdinalIgnoreCase))
            {
                return new CellTag(quality, TTypeMini.Error);
            }

            return new CellTag(quality, TTypeMini.Default);
        }

        private sealed class ResultTableRow
        {
            public PlcDebugPageModel.DebugResultItem Item { get; set; }

            public CellText TimeText { get; set; }

            public CellTag ResultTag { get; set; }

            public CellText TargetText { get; set; }

            public CellText DataTypeText { get; set; }

            public CellText ValueText { get; set; }

            public CellText ActionName { get; set; }

            public CellTag TargetModeTag { get; set; }

            public CellTag QualityTag { get; set; }

            public CellTag PlcNameTag { get; set; }

            public CellText MessageText { get; set; }
        }

        #endregion
    }
}