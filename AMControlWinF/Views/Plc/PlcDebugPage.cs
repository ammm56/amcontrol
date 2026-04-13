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
                    Width = "60"
                },
                new Column("TargetText", "目标", ColumnAlign.Left)
                {
                    Width = "90"
                },
                new Column("DataTypeText", "类型", ColumnAlign.Left)
                {
                    Width = "100"
                },
                new Column("ValueText", "值", ColumnAlign.Left)
                {
                    Width = "100"
                },
                new Column("ActionName", "操作", ColumnAlign.Left)
                {
                    Width = "120"
                },
                new Column("TargetModeText", "模式", ColumnAlign.Center)
                {
                    Width = "90"
                },
                new Column("Quality", "质量", ColumnAlign.Center)
                {
                    Width = "80"
                },
                new Column("PlcName", "PLC", ColumnAlign.Left)
                {
                    Width = "150"
                },
                new Column("MessageText", "消息", ColumnAlign.Left)
                {
                    Width = "220"
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
            _resultTableRows = new AntList<ResultTableRow>(
                _model.ResultHistory.Select(x => new ResultTableRow(x)).ToList());

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

        private sealed class ResultTableRow
        {
            public ResultTableRow(PlcDebugPageModel.DebugResultItem item)
            {
                Item = item ?? new PlcDebugPageModel.DebugResultItem();
            }

            public PlcDebugPageModel.DebugResultItem Item { get; private set; }

            public string TimeText
            {
                get { return Item.TimeText; }
            }

            public string ResultTag
            {
                get { return Item.Success ? "成功" : "失败"; }
            }

            public string ActionName
            {
                get { return string.IsNullOrWhiteSpace(Item.ActionName) ? "-" : Item.ActionName; }
            }

            public string TargetModeText
            {
                get
                {
                    if (string.Equals(Item.TargetMode, "ConfigPoint", StringComparison.OrdinalIgnoreCase))
                    {
                        return "配置点位";
                    }

                    if (string.Equals(Item.TargetMode, "DirectAddress", StringComparison.OrdinalIgnoreCase))
                    {
                        return "直接地址";
                    }

                    return "-";
                }
            }

            public string PlcName
            {
                get { return string.IsNullOrWhiteSpace(Item.PlcName) ? "-" : Item.PlcName; }
            }

            public string TargetText
            {
                get
                {
                    if (!string.IsNullOrWhiteSpace(Item.PointDisplayName))
                    {
                        return Item.PointDisplayName;
                    }

                    if (!string.IsNullOrWhiteSpace(Item.PointName))
                    {
                        return Item.PointName;
                    }

                    return string.IsNullOrWhiteSpace(Item.Address) ? "-" : Item.Address;
                }
            }

            public string DataTypeText
            {
                get
                {
                    string type = string.IsNullOrWhiteSpace(Item.DataType) ? "-" : Item.DataType;
                    string length = Item.Length > 0 ? Item.Length.ToString() : "-";
                    string access = string.IsNullOrWhiteSpace(Item.AccessModeText) ? "-" : Item.AccessModeText;
                    return string.Format("{0}/{1}/{2}", type, length, access);
                }
            }

            public string ValueText
            {
                get { return string.IsNullOrWhiteSpace(Item.ValueText) ? "-" : Item.ValueText; }
            }

            public string Quality
            {
                get { return string.IsNullOrWhiteSpace(Item.Quality) ? "-" : Item.Quality; }
            }

            public string MessageText
            {
                get { return string.IsNullOrWhiteSpace(Item.Message) ? "-" : Item.Message; }
            }
        }
    }
}