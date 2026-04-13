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

                _model.SetSelectedPlc(selectPlcGlobal.SelectedValue == null ? null : selectPlcGlobal.SelectedValue.ToString());
                RefreshView();
            };

            selectPoint.SelectedIndexChanged += (s, e) =>
            {
                if (_isBindingView)
                {
                    return;
                }

                _model.SetSelectedPoint(selectPoint.SelectedValue == null ? null : selectPoint.SelectedValue.ToString());
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
            };

            inputDirectLength.TextChanged += (s, e) =>
            {
                if (_isBindingView)
                {
                    return;
                }

                _model.DirectLength = ParseLength(inputDirectLength.Text);
            };

            inputDirectWriteValue.TextChanged += (s, e) =>
            {
                if (_isBindingView)
                {
                    return;
                }

                _model.DirectWriteValueText = inputDirectWriteValue.Text;
            };

            checkDirectWriteConfirmed.CheckedChanged += (s, e) =>
            {
                if (_isBindingView)
                {
                    return;
                }

                _model.DirectWriteConfirmed = checkDirectWriteConfirmed.Checked;
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

                inputPointAddress.Text = _model.ConfigPointAddress;
                inputPointDataType.Text = _model.ConfigPointDataType;
                inputPointLength.Text = _model.ConfigPointLength.ToString();
                inputPointWriteValue.Text = _model.ConfigWriteValueText;

                inputDirectAddress.Text = _model.DirectAddress;
                inputDirectLength.Text = _model.DirectLength.ToString();
                inputDirectWriteValue.Text = _model.DirectWriteValueText;
                checkDirectWriteConfirmed.Checked = _model.DirectWriteConfirmed;

                BindResult();
            }
            finally
            {
                _isBindingView = false;
            }
        }

        private void BindResult()
        {
            PlcDebugPageModel.DebugResultItem result = _model.LastResult;

            labelResultActionValue.Text = result == null || string.IsNullOrWhiteSpace(result.ActionName) ? "—" : result.ActionName;
            labelResultTargetValue.Text = result == null ? "—" : BuildResultTargetText(result);
            labelResultTypeValue.Text = result == null || string.IsNullOrWhiteSpace(result.DataType) ? "—" : result.DataType;
            labelResultValueValue.Text = result == null || string.IsNullOrWhiteSpace(result.ValueText) ? "—" : result.ValueText;
            labelResultQualityValue.Text = result == null || string.IsNullOrWhiteSpace(result.Quality) ? "—" : result.Quality;
            labelResultTimeValue.Text = result == null ? "—" : result.TimeText;
            labelResultMessageValue.Text = result == null || string.IsNullOrWhiteSpace(result.Message) ? "—" : result.Message;
        }

        private static string BuildResultTargetText(PlcDebugPageModel.DebugResultItem result)
        {
            if (result == null)
            {
                return "—";
            }

            if (!string.IsNullOrWhiteSpace(result.PointName))
            {
                return string.Format(
                    "{0} / {1}",
                    string.IsNullOrWhiteSpace(result.PlcName) ? "-" : result.PlcName,
                    result.PointName);
            }

            if (!string.IsNullOrWhiteSpace(result.Address))
            {
                return string.Format(
                    "{0} / {1}",
                    string.IsNullOrWhiteSpace(result.PlcName) ? "-" : result.PlcName,
                    result.Address);
            }

            return string.IsNullOrWhiteSpace(result.PlcName) ? "—" : result.PlcName;
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

            buttonRefresh.Enabled = !isBusy;
            buttonTestReadPoint.Enabled = !isBusy;
            buttonWritePoint.Enabled = !isBusy;
            buttonTestReadAddress.Enabled = !isBusy;
            buttonWriteAddress.Enabled = !isBusy;

            selectPlcGlobal.Enabled = !isBusy;
            selectPoint.Enabled = !isBusy;
            selectDirectDataType.Enabled = !isBusy;
            inputPointWriteValue.Enabled = !isBusy;
            inputDirectAddress.Enabled = !isBusy;
            inputDirectLength.Enabled = !isBusy;
            inputDirectWriteValue.Enabled = !isBusy;
            checkDirectWriteConfirmed.Enabled = !isBusy;
        }
    }
}