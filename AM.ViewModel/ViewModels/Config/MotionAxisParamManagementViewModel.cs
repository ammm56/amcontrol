using AM.DBService.Services;
using AM.DBService.Services.Motion.Topology;
using AM.Model.Entity.Motion;
using AM.Model.Entity.Motion.Topology;
using AM.Model.Interfaces.DB;
using AM.Model.Interfaces.DB.Motion.Topology;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AM.ViewModel.ViewModels.Config
{
    /// <summary>
    /// 轴运行参数管理页面视图模型。
    /// 三列布局：左轴列表 / 中参数磁贴（分组过滤）/ 右参数详情 + 编辑操作。
    /// </summary>
    public class MotionAxisParamManagementViewModel : ObservableObject
    {
        private readonly IMotionAxisConfigService _axisConfigService;
        private readonly IMotionAxisCrudService _axisCrudService;

        // 内部平铺参数列表（所有分组合并，用于保存和过滤）
        private readonly List<ConfigAxisArgViewModel> _allParams = new List<ConfigAxisArgViewModel>();

        private AxisParamAxisItem _selectedAxis;
        private ConfigAxisArgViewModel _selectedParam;
        private string _selectedGroupFilter;
        private string _statusText;
        private bool _isBusy;

        public MotionAxisParamManagementViewModel()
            : this(new MotionAxisConfigService(), new MotionAxisCrudService())
        {
        }

        public MotionAxisParamManagementViewModel(
            IMotionAxisConfigService axisConfigService,
            IMotionAxisCrudService axisCrudService)
        {
            _axisConfigService = axisConfigService;
            _axisCrudService = axisCrudService;

            AxisItems = new ObservableCollection<AxisParamAxisItem>();
            GroupFilters = new ObservableCollection<string>();
            FilteredParams = new ObservableCollection<ConfigAxisArgViewModel>();
            _statusText = "请在左侧选择轴以查看运行参数";
            _selectedGroupFilter = "全部";

            RefreshCommand = new AsyncRelayCommand(LoadAxisListAsync, CanExecute);
            SaveCurrentAxisCommand = new AsyncRelayCommand(SaveCurrentAxisAsync, CanSave);
            ResetToDefaultCommand = new RelayCommand(ResetAllToDefault, CanSave);
            ResetSelectedParamToDefaultCommand = new RelayCommand(ResetSelectedParamToDefault, CanResetSelected);
        }

        // ── 集合 ──

        public ObservableCollection<AxisParamAxisItem> AxisItems { get; }

        /// <summary>分组过滤器列表："全部" + 各分组显示名。</summary>
        public ObservableCollection<string> GroupFilters { get; }

        /// <summary>当前过滤后展示在磁贴区的参数列表。</summary>
        public ObservableCollection<ConfigAxisArgViewModel> FilteredParams { get; }

        // ── 命令 ──

        public IAsyncRelayCommand RefreshCommand { get; }
        public IAsyncRelayCommand SaveCurrentAxisCommand { get; }
        public IRelayCommand ResetToDefaultCommand { get; }
        public IRelayCommand ResetSelectedParamToDefaultCommand { get; }

        // ── 属性 ──

        public AxisParamAxisItem SelectedAxis
        {
            get { return _selectedAxis; }
            set
            {
                if (SetProperty(ref _selectedAxis, value))
                {
                    OnPropertyChanged(nameof(IsAxisSelected));
                    SelectedParam = null;
                    SaveCurrentAxisCommand.NotifyCanExecuteChanged();
                    ResetToDefaultCommand.NotifyCanExecuteChanged();
                    _ = LoadParamsForAxisAsync();
                }
            }
        }

        public bool IsAxisSelected { get { return _selectedAxis != null; } }

        public ConfigAxisArgViewModel SelectedParam
        {
            get { return _selectedParam; }
            set
            {
                if (SetProperty(ref _selectedParam, value))
                {
                    OnPropertyChanged(nameof(IsParamSelected));
                    ResetSelectedParamToDefaultCommand.NotifyCanExecuteChanged();
                }
            }
        }

        public bool IsParamSelected { get { return _selectedParam != null; } }

        /// <summary>已成功加载参数（供过滤器栏显隐）。</summary>
        public bool IsParamsLoaded { get { return _allParams.Count > 0; } }

        public string SelectedGroupFilter
        {
            get { return _selectedGroupFilter; }
            set
            {
                if (SetProperty(ref _selectedGroupFilter, value))
                {
                    SelectedParam = null;
                    ApplyGroupFilter();
                }
            }
        }

        public string StatusText
        {
            get { return _statusText; }
            set { SetProperty(ref _statusText, value); }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (SetProperty(ref _isBusy, value))
                {
                    RefreshCommand.NotifyCanExecuteChanged();
                    SaveCurrentAxisCommand.NotifyCanExecuteChanged();
                    ResetToDefaultCommand.NotifyCanExecuteChanged();
                    ResetSelectedParamToDefaultCommand.NotifyCanExecuteChanged();
                }
            }
        }

        // ── 加载轴列表 ──

        public async Task LoadAxisListAsync()
        {
            IsBusy = true;
            StatusText = "加载轴列表...";

            try
            {
                var result = await Task.Run(() => _axisCrudService.QueryAll());
                var previousKey = _selectedAxis?.LogicalAxis;

                AxisItems.Clear();
                ClearParams();

                if (!result.Success)
                {
                    StatusText = "轴列表加载失败: " + result.Message;
                    return;
                }

                foreach (var axis in result.Items ?? Enumerable.Empty<MotionAxisEntity>())
                {
                    if (axis == null) continue;
                    AxisItems.Add(new AxisParamAxisItem
                    {
                        LogicalAxis = axis.LogicalAxis,
                        Name = axis.Name ?? string.Empty,
                        DisplayName = string.IsNullOrWhiteSpace(axis.DisplayName) ? axis.Name : axis.DisplayName,
                        CardId = axis.CardId,
                    });
                }

                StatusText = string.Format("已加载 {0} 条轴配置，请选择轴查看参数", AxisItems.Count);

                if (previousKey.HasValue)
                    SelectedAxis = AxisItems.FirstOrDefault(a => a.LogicalAxis == previousKey.Value);
            }
            finally
            {
                IsBusy = false;
            }
        }

        // ── 加载选中轴的参数 ──

        private async Task LoadParamsForAxisAsync()
        {
            ClearParams();
            if (_selectedAxis == null) return;

            IsBusy = true;
            StatusText = string.Format("加载轴 {0}（{1}）运行参数...",
                _selectedAxis.LogicalAxis, _selectedAxis.DisplayName);

            try
            {
                var logicalAxis = _selectedAxis.LogicalAxis;
                var result = await Task.Run(() => _axisConfigService.QueryByLogicalAxis(logicalAxis));

                if (!result.Success || result.Items == null || result.Items.Count == 0)
                {
                    StatusText = string.Format("轴 {0} 暂无运行参数，请先通过种子服务初始化默认参数", logicalAxis);
                    GroupFilters.Add("全部");
                    _selectedGroupFilter = "全部";
                    OnPropertyChanged(nameof(SelectedGroupFilter));
                    OnPropertyChanged(nameof(IsParamsLoaded));
                    return;
                }

                var groupOrder = new[] { "Hardware", "Scale", "Motion", "Home", "SoftLimit", "Timing", "Safety" };
                var grouped = result.Items
                    .Where(p => p != null)
                    .GroupBy(p => p.ParamGroup ?? "Motion")
                    .ToDictionary(g => g.Key, g => g.ToList());

                GroupFilters.Add("全部");

                // 按预定义顺序加载
                foreach (var groupKey in groupOrder)
                {
                    List<MotionAxisConfigEntity> entities;
                    if (!grouped.TryGetValue(groupKey, out entities) || entities.Count == 0) continue;

                    var displayName = GetGroupDisplayName(groupKey);
                    GroupFilters.Add(displayName);

                    foreach (var e in entities)
                    {
                        var vm = ConfigAxisArgViewModel.FromEntity(e);
                        if (vm == null) continue;
                        vm.GroupDisplayName = displayName;
                        _allParams.Add(vm);
                    }
                    grouped.Remove(groupKey);
                }

                // 扩展分组追加末尾
                foreach (var kvp in grouped)
                {
                    var displayName = kvp.Key;
                    if (!GroupFilters.Contains(displayName)) GroupFilters.Add(displayName);
                    foreach (var e in kvp.Value)
                    {
                        var vm = ConfigAxisArgViewModel.FromEntity(e);
                        if (vm == null) continue;
                        vm.GroupDisplayName = displayName;
                        _allParams.Add(vm);
                    }
                }

                // 重置过滤器并展示全部
                _selectedGroupFilter = "全部";
                OnPropertyChanged(nameof(SelectedGroupFilter));
                OnPropertyChanged(nameof(IsParamsLoaded));
                ApplyGroupFilter();

                StatusText = string.Format("已加载轴 {0} 共 {1} 条参数，分 {2} 组",
                    logicalAxis, _allParams.Count, GroupFilters.Count - 1);
            }
            finally
            {
                IsBusy = false;
            }
        }

        // ── 过滤器逻辑 ──

        private void ApplyGroupFilter()
        {
            FilteredParams.Clear();
            var filter = _selectedGroupFilter ?? "全部";

            var source = string.Equals(filter, "全部", System.StringComparison.Ordinal)
                ? _allParams
                : _allParams.Where(p => string.Equals(p.GroupDisplayName, filter, System.StringComparison.Ordinal));

            foreach (var p in source)
                FilteredParams.Add(p);
        }

        // ── 保存当前轴所有参数 ──

        private async Task SaveCurrentAxisAsync()
        {
            if (_selectedAxis == null || _allParams.Count == 0) return;

            var invalidParams = _allParams
                .Where(p => { double v; return !p.TryGetSetValue(out v); })
                .Select(p => p.ParamDisplayName)
                .ToList();

            if (invalidParams.Count > 0)
            {
                StatusText = "保存失败，以下参数值无效: " + string.Join("、", invalidParams);
                return;
            }

            IsBusy = true;
            StatusText = "保存中...";

            try
            {
                var entities = _allParams.Select(p => p.ToEntity()).ToList();
                var result = await Task.Run(() => _axisConfigService.SaveRange(entities));

                if (!result.Success)
                {
                    StatusText = "保存失败: " + result.Message;
                    return;
                }

                foreach (var p in _allParams) p.CommitSave();
                StatusText = string.Format("轴 {0} 参数保存成功，共 {1} 条",
                    _selectedAxis.LogicalAxis, entities.Count);
            }
            finally
            {
                IsBusy = false;
            }
        }

        // ── 新增参数（由 View 代码后置调用） ──

        public async Task AddParamAsync(MotionAxisConfigEntity entity)
        {
            if (entity == null || _selectedAxis == null) return;

            entity.LogicalAxis = _selectedAxis.LogicalAxis;
            entity.AxisDisplayName = string.IsNullOrWhiteSpace(_selectedAxis.DisplayName)
                ? _selectedAxis.Name : _selectedAxis.DisplayName;

            IsBusy = true;
            StatusText = string.Format("新增参数「{0}」...", entity.ParamDisplayName);

            try
            {
                var result = await Task.Run(() =>
                    _axisConfigService.SaveRange(new List<MotionAxisConfigEntity> { entity }));

                if (!result.Success)
                {
                    StatusText = "新增参数失败: " + result.Message;
                    return;
                }

                StatusText = string.Format("新增参数「{0}」成功，正在刷新...", entity.ParamDisplayName);
                await LoadParamsForAxisAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }

        // ── 恢复默认值 ──

        private void ResetAllToDefault()
        {
            foreach (var p in _allParams) p.ResetToDefault();
            StatusText = "已将所有参数恢复为默认值，请确认后点击「保存当前轴参数」使其生效";
        }

        private void ResetSelectedParamToDefault()
        {
            if (_selectedParam == null) return;
            _selectedParam.ResetToDefault();
            StatusText = string.Format("参数「{0}」已恢复为默认值 {1}，请保存生效",
                _selectedParam.ParamDisplayName, _selectedParam.EditValue);
        }

        // ── 辅助 ──

        private void ClearParams()
        {
            _allParams.Clear();
            FilteredParams.Clear();
            GroupFilters.Clear();
            SelectedParam = null;
            OnPropertyChanged(nameof(IsParamsLoaded));
        }

        private bool CanExecute() => !_isBusy;
        private bool CanSave() => !_isBusy && _selectedAxis != null;
        private bool CanResetSelected() => !_isBusy && _selectedParam != null;

        public static string GetGroupDisplayName(string groupName)
        {
            switch (groupName)
            {
                case "Hardware":  return "硬件信号";
                case "Scale":     return "单位换算";
                case "Motion":    return "运动参数";
                case "Home":      return "回零参数";
                case "SoftLimit": return "软件限位";
                case "Timing":    return "使能时序";
                case "Safety":    return "安全联动";
                default:          return groupName;
            }
        }
    }

    // ── 轴列表项 ──

    public class AxisParamAxisItem
    {
        public short LogicalAxis { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public short CardId { get; set; }

        public string SummaryText
        {
            get
            {
                var d = string.IsNullOrWhiteSpace(DisplayName) ? Name : DisplayName;
                return LogicalAxis + " · " + d;
            }
        }
    }
}