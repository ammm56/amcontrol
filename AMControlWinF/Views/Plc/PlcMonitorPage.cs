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
    /// PLC 点位监视页面。
    /// 布局采用三行结构：
    /// 1. 顶部工具栏；
    /// 2. 中部统计卡片；
    /// 3. 底部整行点位虚拟卡片列表。
    /// </summary>
    public partial class PlcMonitorPage : UserControl
    {
        private readonly PlcMonitorPageModel _model;
        private readonly Timer _refreshTimer;

        private bool _isFirstLoad;
        private bool _isBusy;
        private bool _isBindingFilters;

        public PlcMonitorPage()
        {
            InitializeComponent();

            _model = new PlcMonitorPageModel();
            _refreshTimer = new Timer();
            _refreshTimer.Interval = 500;

            BindEvents();

            Disposed += (s, e) =>
            {
                _refreshTimer.Stop();
                _refreshTimer.Dispose();
            };
        }

        private void BindEvents()
        {
            Load += PlcMonitorPage_Load;
            VisibleChanged += PlcMonitorPage_VisibleChanged;

            _refreshTimer.Tick += RefreshTimer_Tick;

            selectPlc.SelectedIndexChanged += (s, e) =>
            {
                if (_isBindingFilters)
                {
                    return;
                }

                _model.SetSelectedPlc(selectPlc.SelectedValue == null ? null : selectPlc.SelectedValue.ToString());
                RefreshView();
            };

            selectGroup.SelectedIndexChanged += (s, e) =>
            {
                if (_isBindingFilters)
                {
                    return;
                }

                _model.SetSelectedGroup(selectGroup.SelectedValue == null ? null : selectGroup.SelectedValue.ToString());
                RefreshView();
            };

            inputSearch.TextChanged += (s, e) =>
            {
                _model.SetSearchText(inputSearch.Text);
                RefreshView();
            };

            buttonRefresh.Click += async (s, e) => await ReloadAsync(false);

            // 保留选中事件，仅用于卡片选中高亮，不再驱动右侧详情区。
            plcPointVirtualListControl.ItemSelected += PlcPointVirtualListControl_ItemSelected;
        }

        private async void PlcMonitorPage_Load(object sender, EventArgs e)
        {
            if (_isFirstLoad)
            {
                return;
            }

            _isFirstLoad = true;
            await ReloadAsync(true);
            UpdateRefreshTimerState();
        }

        private void PlcMonitorPage_VisibleChanged(object sender, EventArgs e)
        {
            UpdateRefreshTimerState();
        }

        private async void RefreshTimer_Tick(object sender, EventArgs e)
        {
            if (!Visible || _isBusy || IsFilterEditing())
            {
                return;
            }

            await ReloadAsync(false);
        }

        private async Task ReloadAsync(bool useLoadMethod)
        {
            if (_isBusy)
            {
                return;
            }

            _isBusy = true;
            try
            {
                Result result = useLoadMethod
                    ? await _model.LoadAsync()
                    : await _model.RefreshAsync();

                RefreshView();

                if (!result.Success)
                {
                    return;
                }
            }
            finally
            {
                _isBusy = false;
            }
        }

        private void RefreshView()
        {
            labelRuntimeSummary.Text = _model.RuntimeSummaryText;
            labelTotalPointCount.Text = _model.TotalPointCount.ToString();
            labelReadablePointCount.Text = _model.ReadablePointCount.ToString();
            labelUnreadablePointCount.Text = _model.UnreadablePointCount.ToString();
            labelErrorPointCount.Text = _model.ErrorPointCount.ToString();

            BindFilters();
            RefreshPointList();
        }

        /// <summary>
        /// 绑定顶部筛选控件。
        /// 绑定期间屏蔽控件事件，避免刷新时再次进入筛选逻辑。
        /// </summary>
        private void BindFilters()
        {
            _isBindingFilters = true;
            try
            {
                BindSelectItems(
                    selectPlc,
                    "全部 PLC",
                    _model.PlcOptions,
                    _model.SelectedPlcName);

                BindSelectItems(
                    selectGroup,
                    "全部分组",
                    _model.GroupOptions,
                    _model.SelectedGroupName);

                if (!string.Equals(
                    inputSearch.Text ?? string.Empty,
                    _model.SearchText ?? string.Empty,
                    StringComparison.Ordinal))
                {
                    inputSearch.Text = _model.SearchText ?? string.Empty;
                }
            }
            finally
            {
                _isBindingFilters = false;
            }
        }

        private static void BindSelectItems(
            Select select,
            string allText,
            IList<string> items,
            string selectedValue)
        {
            if (select == null)
            {
                return;
            }

            string normalizedSelected = string.IsNullOrWhiteSpace(selectedValue)
                ? string.Empty
                : selectedValue.Trim();

            List<SelectItem> selectItems = new List<SelectItem>();
            selectItems.Add(new SelectItem(allText, string.Empty));

            if (items != null)
            {
                selectItems.AddRange(items
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(x => x)
                    .Select(x => new SelectItem(x, x))
                    .ToList());
            }

            select.Items.Clear();
            select.Items.AddRange(selectItems.ToArray());

            if (!string.Equals(
                select.SelectedValue == null ? string.Empty : select.SelectedValue.ToString(),
                normalizedSelected,
                StringComparison.OrdinalIgnoreCase))
            {
                select.SelectedValue = normalizedSelected;
            }
        }

        private void RefreshPointList()
        {
            plcPointVirtualListControl.BindItems(_model.Points, _model.SelectedPoint);
        }

        private void PlcPointVirtualListControl_ItemSelected(object sender, PlcPointVirtualListControl.PlcPointSelectedEventArgs e)
        {
            if (e == null)
            {
                return;
            }

            _model.SelectPointByName(e.PointName);
            RefreshPointList();
        }

        private void UpdateRefreshTimerState()
        {
            if (IsDisposed)
            {
                return;
            }

            if (_isFirstLoad && Visible)
            {
                _refreshTimer.Start();
            }
            else
            {
                _refreshTimer.Stop();
            }
        }

        private bool IsFilterEditing()
        {
            return inputSearch != null
                && inputSearch.Focused
                && inputSearch.Enabled
                && Visible;
        }
    }
}