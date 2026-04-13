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
    /// 当前版本采用“左操作 + 右结果”的工程调试布局：
    /// 1. 左侧：配置点位调试 + 直接地址调试；
    /// 2. 右侧：最近一次执行结果；
    /// 3. 调试历史已由 Model 承载，后续可继续扩展到界面。
    /// </summary>
    public partial class PlcDebugPage : UserControl
    {
        private readonly PlcDebugPageModel _model;

        private bool _isFirstLoad;
        private bool _isBusy;
        private bool _isBindingView;

        public PlcDebugPage()
        {
            InitializeComponent();
            _model = new PlcDebugPageModel();
            BindEvents();
            ApplyStaticViewState();
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

        /// <summary>
        /// 固定视图状态。
        /// 配置点位元信息仅作展示，不允许在调试页修改。
        /// </summary>
        private void ApplyStaticViewState()
        {
            inputPointAddress.Enabled = false;
            inputPointDataType.Enabled = false;
            inputPointLength.Enabled = false;
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

                BindConfigPointMeta();
                BindDirectAddressArea();
                BindResult();
                RefreshOperationState();
            }
            finally
            {
                _isBindingView = false;
            }
        }

        private void BindConfigPointMeta()
        {
            inputPointAddress.Text = _model.ConfigPointAddress;
            inputPointDataType.Text = BuildConfigPointDataTypeText();
            inputPointLength.Text = _model.ConfigPointLength.ToString();
            inputPointWriteValue.Text = _model.ConfigWriteValueText;

            // 当前 Designer 还没有单独的配置点提示标签，
            // 先把权限/启用状态压缩到标题中，避免页面信息缺失。
            labelPointOpTitle.Text = BuildPointOperationTitle();
        }

        private void BindDirectAddressArea()
        {
            inputDirectAddress.Text = _model.DirectAddress;
            inputDirectLength.Text = _model.DirectLength.ToString();
            inputDirectWriteValue.Text = _model.DirectWriteValueText;
            checkDirectWriteConfirmed.Checked = _model.DirectWriteConfirmed;

            // 直接地址读取区当前已有提示标签，可复用展示操作条件。
            labelDirectReadTip.Text = _model.DirectOperationHintText;
        }

        private void RefreshOperationState()
        {
            buttonTestReadPoint.Enabled = !_isBusy && _model.CanReadSelectedPoint;
            buttonWritePoint.Enabled = !_isBusy && _model.CanWriteSelectedPoint;

            buttonTestReadAddress.Enabled = !_isBusy && _model.CanReadDirectAddress;
            buttonWriteAddress.Enabled = !_isBusy && _model.CanWriteDirectAddress;

            // 仅禁止配置元信息编辑；写入值允许编辑。
            inputPointWriteValue.Enabled = !_isBusy && _model.CanWriteSelectedPoint;

            inputDirectAddress.Enabled = !_isBusy;
            inputDirectLength.Enabled = !_isBusy;
            selectDirectDataType.Enabled = !_isBusy;
            inputDirectWriteValue.Enabled = !_isBusy;
            checkDirectWriteConfirmed.Enabled = !_isBusy;

            selectPlcGlobal.Enabled = !_isBusy;
            selectPoint.Enabled = !_isBusy;
            buttonRefresh.Enabled = !_isBusy;

            labelDirectWriteTitle.Text = BuildDirectWriteTitle();
        }

        private void BindResult()
        {
            PlcDebugPageModel.DebugResultItem result = _model.LastResult;

            labelResultActionValue.Text = result == null || string.IsNullOrWhiteSpace(result.ActionName)
                ? "—"
                : result.ActionName;

            labelResultTargetValue.Text = result == null
                ? "—"
                : BuildResultTargetText(result);

            labelResultTypeValue.Text = result == null || string.IsNullOrWhiteSpace(result.DataType)
                ? "—"
                : BuildResultTypeText(result);

            labelResultValueValue.Text = result == null || string.IsNullOrWhiteSpace(result.ValueText)
                ? "—"
                : result.ValueText;

            labelResultQualityValue.Text = result == null || string.IsNullOrWhiteSpace(result.Quality)
                ? "—"
                : result.Quality;

            labelResultTimeValue.Text = result == null
                ? "—"
                : result.TimeText;

            labelResultMessageValue.Text = result == null
                ? "—"
                : BuildResultMessageText(result);

            ApplyResultVisualState(result);
        }

        private void ApplyResultVisualState(PlcDebugPageModel.DebugResultItem result)
        {
            if (result == null)
            {
                labelResultTitle.Text = "执行结果";
                labelResultTitle.ForeColor = System.Drawing.Color.Empty;
                return;
            }

            labelResultTitle.Text = result.Success ? "执行结果 · 成功" : "执行结果 · 失败";
            labelResultTitle.ForeColor = result.Success
                ? System.Drawing.Color.FromArgb(82, 196, 26)
                : System.Drawing.Color.FromArgb(255, 77, 79);
        }

        private string BuildPointOperationTitle()
        {
            string accessText = _model.ConfigPointAccessModeText;
            string enabledText = _model.ConfigPointEnabled ? "已启用" : "未启用";

            if (!_model.IsPointSelected)
            {
                return "按配置点位调试";
            }

            return string.Format(
                "按配置点位调试（{0} / {1}）",
                string.IsNullOrWhiteSpace(accessText) ? "-" : accessText,
                enabledText);
        }

        private string BuildConfigPointDataTypeText()
        {
            if (string.IsNullOrWhiteSpace(_model.ConfigPointDataType))
            {
                return "—";
            }

            string accessText = _model.ConfigPointAccessModeText;
            if (string.IsNullOrWhiteSpace(accessText) || accessText == "-")
            {
                return _model.ConfigPointDataType;
            }

            return string.Format("{0} / {1}", _model.ConfigPointDataType, accessText);
        }

        private string BuildDirectWriteTitle()
        {
            if (_model.CanWriteDirectAddress)
            {
                return "按直接地址写入（已确认）";
            }

            return "按直接地址写入";
        }

        private static string BuildResultTargetText(PlcDebugPageModel.DebugResultItem result)
        {
            if (result == null)
            {
                return "—";
            }

            string target = !string.IsNullOrWhiteSpace(result.PointDisplayName)
                ? result.PointDisplayName
                : (!string.IsNullOrWhiteSpace(result.PointName) ? result.PointName : result.Address);

            if (string.IsNullOrWhiteSpace(target))
            {
                target = "—";
            }

            if (!string.IsNullOrWhiteSpace(result.PlcName))
            {
                return string.Format("{0} / {1}", result.PlcName, target);
            }

            return target;
        }

        private static string BuildResultTypeText(PlcDebugPageModel.DebugResultItem result)
        {
            if (result == null)
            {
                return "—";
            }

            string dataType = string.IsNullOrWhiteSpace(result.DataType) ? "-" : result.DataType;
            string lengthText = result.Length > 0 ? result.Length.ToString() : "-";
            string accessText = string.IsNullOrWhiteSpace(result.AccessModeText) ? "-" : result.AccessModeText;

            return string.Format("{0} / Len={1} / {2}", dataType, lengthText, accessText);
        }

        private static string BuildResultMessageText(PlcDebugPageModel.DebugResultItem result)
        {
            if (result == null)
            {
                return "—";
            }

            List<string> lines = new List<string>();

            lines.Add(string.IsNullOrWhiteSpace(result.Message) ? "—" : result.Message);

            if (!string.IsNullOrWhiteSpace(result.RawValue))
            {
                lines.Add("原始值: " + result.RawValue);
            }

            if (!string.IsNullOrWhiteSpace(result.InputValueText))
            {
                lines.Add("输入值: " + result.InputValueText);
            }

            if (!string.IsNullOrWhiteSpace(result.TargetMode))
            {
                lines.Add("模式: " + result.TargetMode);
            }

            if (result.Confirmed)
            {
                lines.Add("已确认高风险写入");
            }

            return string.Join(Environment.NewLine, lines);
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
    }
}