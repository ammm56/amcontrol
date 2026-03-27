using AM.Core.Context;
using AM.DBService.Services.Motion.Actuator;
using AM.Model.Common;
using AM.Model.MotionCard.Actuator;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AM.ViewModel.ViewModels.Motion
{
    /// <summary>
    /// 执行器控制页视图模型。
    /// 从 MachineContext 读取已注册的执行器对象，订阅 MotionIo.SnapshotChanged 驱动状态刷新。
    /// 控制命令通过各类执行器服务异步执行，结果展示到 StatusText。
    /// 页面由 MainWindow 缓存复用，不在 Unloaded 中释放订阅。
    /// </summary>
    public class ActuatorControlViewModel : ObservableObject
    {
        private readonly CylinderService _cylinderService;
        private readonly GripperService _gripperService;
        private readonly VacuumService _vacuumService;
        private readonly StackLightService _stackLightService;
        private readonly SynchronizationContext _uiContext;
        private readonly List<ActuatorControlItem> _allItems;

        private ActuatorControlItem _selectedItem;
        private string _typeFilter;
        private string _statusText;
        private bool _isBusy;

        public ActuatorControlViewModel()
        {
            _cylinderService = new CylinderService();
            _gripperService = new GripperService();
            _vacuumService = new VacuumService();
            _stackLightService = new StackLightService();
            _uiContext = SynchronizationContext.Current ?? new SynchronizationContext();
            _allItems = new List<ActuatorControlItem>();

            Items = new ObservableCollection<ActuatorControlItem>();

            _typeFilter = "All";
            _statusText = "请等待执行器列表加载";

            RefreshCommand = new AsyncRelayCommand(RefreshAsync);
            SetTypeFilterCommand = new RelayCommand<string>(SetTypeFilter);

            // 气缸命令
            CylinderExtendCommand = new AsyncRelayCommand(CylinderExtendAsync, CanOperateCylinder);
            CylinderRetractCommand = new AsyncRelayCommand(CylinderRetractAsync, CanOperateCylinder);

            // 夹爪命令
            GripperCloseCommand = new AsyncRelayCommand(GripperCloseAsync, CanOperateGripper);
            GripperOpenCommand = new AsyncRelayCommand(GripperOpenAsync, CanOperateGripper);

            // 真空命令
            VacuumOnCommand = new AsyncRelayCommand(VacuumOnAsync, CanOperateVacuum);
            VacuumOffCommand = new AsyncRelayCommand(VacuumOffAsync, CanOperateVacuum);

            // 灯塔命令
            StackLightIdleCommand = new AsyncRelayCommand(StackLightIdleAsync, CanOperateStackLight);
            StackLightRunningCommand = new AsyncRelayCommand(StackLightRunningAsync, CanOperateStackLight);
            StackLightWarningCommand = new AsyncRelayCommand(StackLightWarningAsync, CanOperateStackLight);
            StackLightAlarmCommand = new AsyncRelayCommand(StackLightAlarmAsync, CanOperateStackLight);
            StackLightOffCommand = new AsyncRelayCommand(StackLightOffAsync, CanOperateStackLight);

            // 订阅 IO 快照变更，驱动执行器状态刷新
            RuntimeContext.Instance.MotionIo.SnapshotChanged += MotionIo_SnapshotChanged;
        }

        public ObservableCollection<ActuatorControlItem> Items { get; private set; }

        public IAsyncRelayCommand RefreshCommand { get; private set; }

        public IRelayCommand<string> SetTypeFilterCommand { get; private set; }

        // 气缸控制命令
        public IAsyncRelayCommand CylinderExtendCommand { get; private set; }
        public IAsyncRelayCommand CylinderRetractCommand { get; private set; }

        // 夹爪控制命令
        public IAsyncRelayCommand GripperCloseCommand { get; private set; }
        public IAsyncRelayCommand GripperOpenCommand { get; private set; }

        // 真空控制命令
        public IAsyncRelayCommand VacuumOnCommand { get; private set; }
        public IAsyncRelayCommand VacuumOffCommand { get; private set; }

        // 灯塔控制命令
        public IAsyncRelayCommand StackLightIdleCommand { get; private set; }
        public IAsyncRelayCommand StackLightRunningCommand { get; private set; }
        public IAsyncRelayCommand StackLightWarningCommand { get; private set; }
        public IAsyncRelayCommand StackLightAlarmCommand { get; private set; }
        public IAsyncRelayCommand StackLightOffCommand { get; private set; }

        public ActuatorControlItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (SetProperty(ref _selectedItem, value))
                {
                    NotifyCommandState();
                }
            }
        }

        public string TypeFilter
        {
            get { return _typeFilter; }
            set
            {
                if (SetProperty(ref _typeFilter, value))
                {
                    ApplyTypeFilter();
                }
            }
        }

        public string StatusText
        {
            get { return _statusText; }
            set { SetProperty(ref _statusText, value); }
        }

        public async Task LoadAsync()
        {
            await RefreshAsync();
        }

        private async Task RefreshAsync()
        {
            if (_isBusy)
            {
                return;
            }

            _isBusy = true;

            try
            {
                var previousName = SelectedItem == null ? null : SelectedItem.Name;

                _allItems.Clear();

                // 加载气缸
                var cylResult = await Task.Run(() => _cylinderService.QueryAll());
                if (cylResult.Success && cylResult.Items != null)
                {
                    foreach (var c in cylResult.Items)
                    {
                        _allItems.Add(new ActuatorControlItem(c));
                    }
                }

                // 加载夹爪
                var gripResult = await Task.Run(() => _gripperService.QueryAll());
                if (gripResult.Success && gripResult.Items != null)
                {
                    foreach (var g in gripResult.Items)
                    {
                        _allItems.Add(new ActuatorControlItem(g));
                    }
                }

                // 加载真空
                var vacResult = await Task.Run(() => _vacuumService.QueryAll());
                if (vacResult.Success && vacResult.Items != null)
                {
                    foreach (var v in vacResult.Items)
                    {
                        _allItems.Add(new ActuatorControlItem(v));
                    }
                }

                // 加载灯塔
                var slResult = await Task.Run(() => _stackLightService.QueryAll());
                if (slResult.Success && slResult.Items != null)
                {
                    foreach (var s in slResult.Items)
                    {
                        _allItems.Add(new ActuatorControlItem(s));
                    }
                }

                // 刷新运行态状态
                RefreshRuntimeState(_allItems);

                ApplyTypeFilter();

                // 尝试恢复之前的选中项
                if (previousName != null)
                {
                    SelectedItem = Items.FirstOrDefault(x => x.Name == previousName)
                        ?? (Items.Count > 0 ? Items[0] : null);
                }
                else
                {
                    SelectedItem = Items.Count > 0 ? Items[0] : null;
                }

                StatusText = string.Format("执行器已加载：气缸 {0} / 夹爪 {1} / 真空 {2} / 灯塔 {3}",
                    _allItems.Count(x => x.ActuatorType == "Cylinder"),
                    _allItems.Count(x => x.ActuatorType == "Gripper"),
                    _allItems.Count(x => x.ActuatorType == "Vacuum"),
                    _allItems.Count(x => x.ActuatorType == "StackLight"));
            }
            finally
            {
                _isBusy = false;
            }
        }

        /// <summary>
        /// IO 快照变更事件处理：在 UI 线程刷新执行器运行态状态。
        /// </summary>
        private void MotionIo_SnapshotChanged()
        {
            _uiContext.Post(_ => RefreshRuntimeState(_allItems), null);
        }

        /// <summary>
        /// 读取 RuntimeContext.MotionIo 更新执行器状态标志。
        /// </summary>
        private void RefreshRuntimeState(IList<ActuatorControlItem> items)
        {
            if (items == null || items.Count == 0)
            {
                return;
            }

            var ioRuntime = RuntimeContext.Instance.MotionIo;

            foreach (var item in items)
            {
                bool val;

                if (item.ActuatorType == "Cylinder")
                {
                    var cyl = item.CylinderConfig;
                    if (cyl != null)
                    {
                        if (cyl.ExtendFeedbackBit.HasValue && ioRuntime.TryGetDI(cyl.ExtendFeedbackBit.Value, out val))
                        {
                            item.IsStateA = val;
                        }
                        else if (ioRuntime.TryGetDO(cyl.ExtendOutputBit, out val))
                        {
                            item.IsStateA = val;
                        }

                        if (cyl.RetractFeedbackBit.HasValue && ioRuntime.TryGetDI(cyl.RetractFeedbackBit.Value, out val))
                        {
                            item.IsStateB = val;
                        }
                        else if (cyl.RetractOutputBit.HasValue && ioRuntime.TryGetDO(cyl.RetractOutputBit.Value, out val))
                        {
                            item.IsStateB = val;
                        }
                    }
                }
                else if (item.ActuatorType == "Gripper")
                {
                    var grip = item.GripperConfig;
                    if (grip != null)
                    {
                        if (grip.CloseFeedbackBit.HasValue && ioRuntime.TryGetDI(grip.CloseFeedbackBit.Value, out val))
                        {
                            item.IsStateA = val;
                        }
                        else if (ioRuntime.TryGetDO(grip.CloseOutputBit, out val))
                        {
                            item.IsStateA = val;
                        }

                        if (grip.OpenFeedbackBit.HasValue && ioRuntime.TryGetDI(grip.OpenFeedbackBit.Value, out val))
                        {
                            item.IsStateB = val;
                        }
                        else if (grip.OpenOutputBit.HasValue && ioRuntime.TryGetDO(grip.OpenOutputBit.Value, out val))
                        {
                            item.IsStateB = val;
                        }

                        if (grip.WorkpiecePresentBit.HasValue && ioRuntime.TryGetDI(grip.WorkpiecePresentBit.Value, out val))
                        {
                            item.HasWorkpiece = val;
                        }
                    }
                }
                else if (item.ActuatorType == "Vacuum")
                {
                    var vac = item.VacuumConfig;
                    if (vac != null)
                    {
                        if (vac.VacuumFeedbackBit.HasValue && ioRuntime.TryGetDI(vac.VacuumFeedbackBit.Value, out val))
                        {
                            item.IsStateA = val;
                        }
                        else if (ioRuntime.TryGetDO(vac.VacuumOnOutputBit, out val))
                        {
                            item.IsStateA = val;
                        }

                        if (vac.WorkpiecePresentBit.HasValue && ioRuntime.TryGetDI(vac.WorkpiecePresentBit.Value, out val))
                        {
                            item.HasWorkpiece = val;
                        }
                    }
                }
                else if (item.ActuatorType == "StackLight")
                {
                    var sl = item.StackLightConfig;
                    if (sl != null)
                    {
                        bool red = false, yellow = false, green = false;
                        if (sl.RedOutputBit.HasValue) ioRuntime.TryGetDO(sl.RedOutputBit.Value, out red);
                        if (sl.YellowOutputBit.HasValue) ioRuntime.TryGetDO(sl.YellowOutputBit.Value, out yellow);
                        if (sl.GreenOutputBit.HasValue) ioRuntime.TryGetDO(sl.GreenOutputBit.Value, out green);

                        item.IsStateA = red;
                        item.IsStateB = yellow;
                        item.IsStateC = green;
                        item.StackLightStateText = GetStackLightStateText(red, yellow, green);
                    }
                }
            }
        }

        private static string GetStackLightStateText(bool red, bool yellow, bool green)
        {
            if (red && !yellow && !green)
            {
                return "报警（红灯）";
            }

            if (!red && yellow && !green)
            {
                return "警告（黄灯）";
            }

            if (!red && !yellow && green)
            {
                return "绿灯亮";
            }

            if (!red && !yellow && !green)
            {
                return "熄灭";
            }

            return "组合状态";
        }

        private void SetTypeFilter(string typeFilter)
        {
            TypeFilter = typeFilter ?? "All";
        }

        private void ApplyTypeFilter()
        {
            var previousName = SelectedItem == null ? null : SelectedItem.Name;
            var filter = _typeFilter ?? "All";

            var filtered = filter == "All"
                ? _allItems.ToList()
                : _allItems.Where(x => x.ActuatorType == filter).ToList();

            Items.Clear();
            foreach (var item in filtered)
            {
                Items.Add(item);
            }

            if (previousName != null)
            {
                SelectedItem = Items.FirstOrDefault(x => x.Name == previousName)
                    ?? (Items.Count > 0 ? Items[0] : null);
            }
            else
            {
                SelectedItem = Items.Count > 0 ? Items[0] : null;
            }
        }

        // ─── 气缸控制 ───────────────────────────────────────

        private bool CanOperateCylinder()
        {
            return !_isBusy && SelectedItem != null && SelectedItem.ActuatorType == "Cylinder";
        }

        private async Task CylinderExtendAsync()
        {
            var item = SelectedItem;
            if (item == null || item.ActuatorType != "Cylinder")
            {
                return;
            }

            StatusText = string.Format("正在伸出气缸 [{0}]...", item.DisplayName);
            _isBusy = true;
            NotifyCommandState();

            try
            {
                var result = await Task.Run(() => _cylinderService.Extend(item.Name, false));
                StatusText = result.Success
                    ? string.Format("气缸 [{0}] 已发送伸出指令", item.DisplayName)
                    : string.Format("气缸 [{0}] 伸出失败：{1}", item.DisplayName, result.Message);
            }
            finally
            {
                _isBusy = false;
                NotifyCommandState();
            }
        }

        private async Task CylinderRetractAsync()
        {
            var item = SelectedItem;
            if (item == null || item.ActuatorType != "Cylinder")
            {
                return;
            }

            StatusText = string.Format("正在缩回气缸 [{0}]...", item.DisplayName);
            _isBusy = true;
            NotifyCommandState();

            try
            {
                var result = await Task.Run(() => _cylinderService.Retract(item.Name, false));
                StatusText = result.Success
                    ? string.Format("气缸 [{0}] 已发送缩回指令", item.DisplayName)
                    : string.Format("气缸 [{0}] 缩回失败：{1}", item.DisplayName, result.Message);
            }
            finally
            {
                _isBusy = false;
                NotifyCommandState();
            }
        }

        // ─── 夹爪控制 ───────────────────────────────────────

        private bool CanOperateGripper()
        {
            return !_isBusy && SelectedItem != null && SelectedItem.ActuatorType == "Gripper";
        }

        private async Task GripperCloseAsync()
        {
            var item = SelectedItem;
            if (item == null || item.ActuatorType != "Gripper")
            {
                return;
            }

            StatusText = string.Format("正在夹紧夹爪 [{0}]...", item.DisplayName);
            _isBusy = true;
            NotifyCommandState();

            try
            {
                var result = await Task.Run(() => _gripperService.Close(item.Name, false));
                StatusText = result.Success
                    ? string.Format("夹爪 [{0}] 已发送夹紧指令", item.DisplayName)
                    : string.Format("夹爪 [{0}] 夹紧失败：{1}", item.DisplayName, result.Message);
            }
            finally
            {
                _isBusy = false;
                NotifyCommandState();
            }
        }

        private async Task GripperOpenAsync()
        {
            var item = SelectedItem;
            if (item == null || item.ActuatorType != "Gripper")
            {
                return;
            }

            StatusText = string.Format("正在打开夹爪 [{0}]...", item.DisplayName);
            _isBusy = true;
            NotifyCommandState();

            try
            {
                var result = await Task.Run(() => _gripperService.Open(item.Name, false));
                StatusText = result.Success
                    ? string.Format("夹爪 [{0}] 已发送打开指令", item.DisplayName)
                    : string.Format("夹爪 [{0}] 打开失败：{1}", item.DisplayName, result.Message);
            }
            finally
            {
                _isBusy = false;
                NotifyCommandState();
            }
        }

        // ─── 真空控制 ───────────────────────────────────────

        private bool CanOperateVacuum()
        {
            return !_isBusy && SelectedItem != null && SelectedItem.ActuatorType == "Vacuum";
        }

        private async Task VacuumOnAsync()
        {
            var item = SelectedItem;
            if (item == null || item.ActuatorType != "Vacuum")
            {
                return;
            }

            StatusText = string.Format("正在打开真空 [{0}]...", item.DisplayName);
            _isBusy = true;
            NotifyCommandState();

            try
            {
                var result = await Task.Run(() => _vacuumService.VacuumOn(item.Name, false));
                StatusText = result.Success
                    ? string.Format("真空 [{0}] 已发送吸真空指令", item.DisplayName)
                    : string.Format("真空 [{0}] 打开失败：{1}", item.DisplayName, result.Message);
            }
            finally
            {
                _isBusy = false;
                NotifyCommandState();
            }
        }

        private async Task VacuumOffAsync()
        {
            var item = SelectedItem;
            if (item == null || item.ActuatorType != "Vacuum")
            {
                return;
            }

            StatusText = string.Format("正在关闭真空 [{0}]...", item.DisplayName);
            _isBusy = true;
            NotifyCommandState();

            try
            {
                var result = await Task.Run(() => _vacuumService.VacuumOff(item.Name, false));
                StatusText = result.Success
                    ? string.Format("真空 [{0}] 已发送破真空指令", item.DisplayName)
                    : string.Format("真空 [{0}] 关闭失败：{1}", item.DisplayName, result.Message);
            }
            finally
            {
                _isBusy = false;
                NotifyCommandState();
            }
        }

        // ─── 灯塔控制 ───────────────────────────────────────

        private bool CanOperateStackLight()
        {
            return !_isBusy && SelectedItem != null && SelectedItem.ActuatorType == "StackLight";
        }

        private async Task StackLightIdleAsync()
        {
            await SetStackLightStateAsync("空闲", s => _stackLightService.SetIdle(s));
        }

        private async Task StackLightRunningAsync()
        {
            await SetStackLightStateAsync("运行", s => _stackLightService.SetRunning(s));
        }

        private async Task StackLightWarningAsync()
        {
            await SetStackLightStateAsync("警告", s => _stackLightService.SetWarning(s));
        }

        private async Task StackLightAlarmAsync()
        {
            await SetStackLightStateAsync("报警", s => _stackLightService.SetAlarm(s));
        }

        private async Task StackLightOffAsync()
        {
            await SetStackLightStateAsync("熄灭", s => _stackLightService.TurnOff(s));
        }

        private async Task SetStackLightStateAsync(string stateName, Func<string, Result> action)
        {
            var item = SelectedItem;
            if (item == null || item.ActuatorType != "StackLight")
            {
                return;
            }

            StatusText = string.Format("正在设置灯塔 [{0}] 为{1}状态...", item.DisplayName, stateName);
            _isBusy = true;
            NotifyCommandState();

            try
            {
                var result = await Task.Run(() => action(item.Name));
                StatusText = result.Success
                    ? string.Format("灯塔 [{0}] 已切换为{1}状态", item.DisplayName, stateName)
                    : string.Format("灯塔 [{0}] 切换失败：{1}", item.DisplayName, result.Message);
            }
            finally
            {
                _isBusy = false;
                NotifyCommandState();
            }
        }

        private void NotifyCommandState()
        {
            CylinderExtendCommand.NotifyCanExecuteChanged();
            CylinderRetractCommand.NotifyCanExecuteChanged();
            GripperCloseCommand.NotifyCanExecuteChanged();
            GripperOpenCommand.NotifyCanExecuteChanged();
            VacuumOnCommand.NotifyCanExecuteChanged();
            VacuumOffCommand.NotifyCanExecuteChanged();
            StackLightIdleCommand.NotifyCanExecuteChanged();
            StackLightRunningCommand.NotifyCanExecuteChanged();
            StackLightWarningCommand.NotifyCanExecuteChanged();
            StackLightAlarmCommand.NotifyCanExecuteChanged();
            StackLightOffCommand.NotifyCanExecuteChanged();
        }
    }

    /// <summary>
    /// 执行器控制列表项。封装各类执行器对象的通用显示信息与实时状态。
    /// </summary>
    public class ActuatorControlItem : ObservableObject
    {
        private bool _isStateA;
        private bool _isStateB;
        private bool _isStateC;
        private bool _hasWorkpiece;
        private string _stackLightStateText;

        public ActuatorControlItem(CylinderConfig config)
        {
            ActuatorType = "Cylinder";
            TypeDisplay = "气缸";
            Name = config.Name;
            DisplayName = string.IsNullOrWhiteSpace(config.DisplayName) ? config.Name : config.DisplayName;
            IsEnabled = config.IsEnabled;
            SortOrder = config.SortOrder;
            Description = config.Description;
            Remark = config.Remark;
            StateALabel = "伸出";
            StateBLabel = "缩回";
            CylinderConfig = config;
        }

        public ActuatorControlItem(GripperConfig config)
        {
            ActuatorType = "Gripper";
            TypeDisplay = "夹爪";
            Name = config.Name;
            DisplayName = string.IsNullOrWhiteSpace(config.DisplayName) ? config.Name : config.DisplayName;
            IsEnabled = config.IsEnabled;
            SortOrder = config.SortOrder;
            Description = config.Description;
            Remark = config.Remark;
            StateALabel = "夹紧";
            StateBLabel = "打开";
            GripperConfig = config;
        }

        public ActuatorControlItem(VacuumConfig config)
        {
            ActuatorType = "Vacuum";
            TypeDisplay = "真空";
            Name = config.Name;
            DisplayName = string.IsNullOrWhiteSpace(config.DisplayName) ? config.Name : config.DisplayName;
            IsEnabled = config.IsEnabled;
            SortOrder = config.SortOrder;
            Description = config.Description;
            Remark = config.Remark;
            StateALabel = "真空";
            StateBLabel = "工件";
            VacuumConfig = config;
        }

        public ActuatorControlItem(StackLightConfig config)
        {
            ActuatorType = "StackLight";
            TypeDisplay = "灯塔";
            Name = config.Name;
            DisplayName = string.IsNullOrWhiteSpace(config.DisplayName) ? config.Name : config.DisplayName;
            IsEnabled = config.IsEnabled;
            SortOrder = config.SortOrder;
            Description = config.Description;
            Remark = config.Remark;
            StateALabel = "红";
            StateBLabel = "黄";
            StateCLabel = "绿";
            StackLightConfig = config;
            _stackLightStateText = "—";
        }

        /// <summary>执行器类型标识：Cylinder / Gripper / Vacuum / StackLight。</summary>
        public string ActuatorType { get; }

        /// <summary>类型中文显示名。</summary>
        public string TypeDisplay { get; }

        /// <summary>对象名称（唯一标识）。</summary>
        public string Name { get; }

        /// <summary>显示名称。</summary>
        public string DisplayName { get; }

        /// <summary>是否启用。</summary>
        public bool IsEnabled { get; }

        /// <summary>排序号。</summary>
        public int SortOrder { get; }

        /// <summary>说明。</summary>
        public string Description { get; }

        /// <summary>备注。</summary>
        public string Remark { get; }

        /// <summary>状态 A 标签（气缸=伸出/夹爪=夹紧/真空=真空/灯塔=红）。</summary>
        public string StateALabel { get; }

        /// <summary>状态 B 标签（气缸=缩回/夹爪=打开/真空=工件/灯塔=黄）。</summary>
        public string StateBLabel { get; }

        /// <summary>状态 C 标签（灯塔=绿）。</summary>
        public string StateCLabel { get; }

        /// <summary>气缸配置引用（仅 Cylinder 类型）。</summary>
        public CylinderConfig CylinderConfig { get; }

        /// <summary>夹爪配置引用（仅 Gripper 类型）。</summary>
        public GripperConfig GripperConfig { get; }

        /// <summary>真空配置引用（仅 Vacuum 类型）。</summary>
        public VacuumConfig VacuumConfig { get; }

        /// <summary>灯塔配置引用（仅 StackLight 类型）。</summary>
        public StackLightConfig StackLightConfig { get; }

        /// <summary>状态 A（伸出/夹紧/真空建立/红灯）。</summary>
        public bool IsStateA
        {
            get { return _isStateA; }
            set
            {
                if (SetProperty(ref _isStateA, value))
                {
                    OnPropertyChanged(nameof(StateText));
                }
            }
        }

        /// <summary>状态 B（缩回/打开/工件存在/黄灯）。</summary>
        public bool IsStateB
        {
            get { return _isStateB; }
            set
            {
                if (SetProperty(ref _isStateB, value))
                {
                    OnPropertyChanged(nameof(StateText));
                }
            }
        }

        /// <summary>状态 C（绿灯，仅灯塔使用）。</summary>
        public bool IsStateC
        {
            get { return _isStateC; }
            set
            {
                if (SetProperty(ref _isStateC, value))
                {
                    OnPropertyChanged(nameof(StateText));
                }
            }
        }

        /// <summary>工件存在（夹爪/真空使用）。</summary>
        public bool HasWorkpiece
        {
            get { return _hasWorkpiece; }
            set { SetProperty(ref _hasWorkpiece, value); }
        }

        /// <summary>灯塔综合状态文本。</summary>
        public string StackLightStateText
        {
            get { return _stackLightStateText; }
            set { SetProperty(ref _stackLightStateText, value); }
        }

        /// <summary>气缸/夹爪/真空综合状态文本。</summary>
        public string StateText
        {
            get
            {
                if (ActuatorType == "StackLight")
                {
                    return _stackLightStateText;
                }

                if (ActuatorType == "Cylinder")
                {
                    if (_isStateA && !_isStateB)
                    {
                        return "已伸出";
                    }

                    if (!_isStateA && _isStateB)
                    {
                        return "已缩回";
                    }

                    if (!_isStateA && !_isStateB)
                    {
                        return "中间态";
                    }

                    return "双到位";
                }

                if (ActuatorType == "Gripper")
                {
                    if (_isStateA && !_isStateB)
                    {
                        return "已夹紧";
                    }

                    if (!_isStateA && _isStateB)
                    {
                        return "已打开";
                    }

                    if (!_isStateA && !_isStateB)
                    {
                        return "中间态";
                    }

                    return "异常";
                }

                if (ActuatorType == "Vacuum")
                {
                    return _isStateA ? "真空建立" : "真空释放";
                }

                return "—";
            }
        }
    }
}
