using AM.Core.Context;
using AM.DBService.Services.Motion.Actuator;
using AM.Model.Common;
using AM.Model.MotionCard.Actuator;
using AM.PageModel.Common;
using AM.PageModel.Motion.Actuator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AM.PageModel.Motion
{
    /// <summary>
    /// WinForms 执行器控制页页面模型。
    ///
    /// 【第二阶段收口定位】
    /// 本类继续保持“页面协调模型”定位，不做完整 MVVM。
    /// 在第一阶段完成模型拆分后，第二阶段开始收短调用链，
    /// 让页面层从“参与状态组装”收敛为“只负责事件和 Bind”。
    ///
    /// 【当前职责】
    /// 1. 从 MachineContext 构建执行器静态快照；
    /// 2. 从 RuntimeContext / 执行器服务刷新运行态；
    /// 3. 维护搜索、类型筛选、当前选中；
    /// 4. 维护左侧列表显示对象；
    /// 5. 直接维护右侧详情显示对象；
    /// 6. 直接维护右侧动作面板显示对象；
    /// 7. 提供动作校验与执行入口。
    ///
    /// 【与页面层关系】
    /// 页面层不再调用 BuildActionPanelState(...) 临时组装动作区状态，
    /// 而是只读取：
    /// - PageItems
    /// - SelectedItemKey
    /// - SelectedDetail
    /// - SelectedActionPanel
    ///
    /// 页面层只做：
    /// - 事件接线
    /// - 更新选项
    /// - 调模型动作
    /// - Bind 到控件
    ///
    /// 这样更符合 WinForms 简单、直观、事件驱动的实现方式。
    ///
    /// 【与自动任务层关系】
    /// 本类仅服务于“页面手动控制与监控”，
    /// 不负责后台自动流程、状态机、步骤编排。
    /// 后续自动任务层应独立建设，不与页面显示模型混合。
    /// </summary>
    public class MotionActuatorPageModel : BindableBase
    {
        private readonly CylinderService _cylinderService;
        private readonly VacuumService _vacuumService;
        private readonly GripperService _gripperService;
        private readonly StackLightService _stackLightService;

        private readonly MotionActuatorFilter _filter;
        private readonly MotionActuatorDisplayBuilder _displayBuilder;

        /// <summary>
        /// 所有执行器原始快照。
        /// 包含当前页面需要的静态配置与运行态数据。
        /// </summary>
        private List<MotionActuatorSnapshot> _allSnapshots;

        /// <summary>
        /// 当前筛选后的原始快照集合。
        /// 选择逻辑、动作逻辑都基于它。
        /// </summary>
        private List<MotionActuatorSnapshot> _filteredSnapshots;

        /// <summary>
        /// 当前筛选后的左侧列表显示对象集合。
        /// 只提供给左侧列表控件使用。
        /// </summary>
        private List<MotionActuatorListItem> _pageItems;

        /// <summary>
        /// 当前选中的原始快照。
        /// 只在 PageModel 内部使用，不向 UI 层暴露。
        /// </summary>
        private MotionActuatorSnapshot _selectedSnapshot;

        /// <summary>
        /// 当前选中对象对应的详情显示数据。
        /// </summary>
        private MotionActuatorDetailData _selectedDetail;

        /// <summary>
        /// 当前选中对象对应的动作面板显示数据。
        /// </summary>
        private MotionActuatorActionPanelState _selectedActionPanel;

        /// <summary>
        /// 当前动作面板选项：等待反馈。
        /// 页面层变更选项时同步到这里，由模型统一重算动作面板状态。
        /// </summary>
        private bool _waitFeedback;

        /// <summary>
        /// 当前动作面板选项：等待工件检测。
        /// </summary>
        private bool _waitWorkpiece;

        /// <summary>
        /// 当前动作面板选项：灯塔附带蜂鸣。
        /// </summary>
        private bool _stackLightWithBuzzer;

        private int _totalCount;
        private int _cylinderCount;
        private int _vacuumCount;
        private int _gripperCount;
        private int _stackLightCount;

        public MotionActuatorPageModel()
        {
            _cylinderService = new CylinderService();
            _vacuumService = new VacuumService();
            _gripperService = new GripperService();
            _stackLightService = new StackLightService();

            _filter = new MotionActuatorFilter();
            _displayBuilder = new MotionActuatorDisplayBuilder();

            _allSnapshots = new List<MotionActuatorSnapshot>();
            _filteredSnapshots = new List<MotionActuatorSnapshot>();
            _pageItems = new List<MotionActuatorListItem>();

            _waitFeedback = true;
            _waitWorkpiece = false;
            _stackLightWithBuzzer = false;

            _selectedDetail = MotionActuatorDetailData.CreateEmpty();
            _selectedActionPanel = MotionActuatorActionPanelState.CreateDefault();
        }

        /// <summary>
        /// 当前左侧卡片数据。
        /// 执行器控制页不分页，直接交给 VirtualPanel。
        /// </summary>
        public IList<MotionActuatorListItem> PageItems
        {
            get { return _pageItems; }
        }

        /// <summary>
        /// 当前选中项唯一键。
        /// 页面与列表控件只需要通过该键保持选中状态，
        /// 不需要直接接触原始快照对象。
        /// </summary>
        public string SelectedItemKey
        {
            get { return SelectedSnapshot == null ? null : SelectedSnapshot.ItemKey; }
        }

        /// <summary>
        /// 当前右侧详情区显示数据。
        /// 页面或详情控件可直接 Bind。
        /// </summary>
        public MotionActuatorDetailData SelectedDetail
        {
            get { return _selectedDetail; }
            private set { SetProperty(ref _selectedDetail, value); }
        }

        /// <summary>
        /// 当前右侧动作面板显示数据。
        /// 页面或动作面板控件可直接 Bind。
        /// </summary>
        public MotionActuatorActionPanelState SelectedActionPanel
        {
            get { return _selectedActionPanel; }
            private set { SetProperty(ref _selectedActionPanel, value); }
        }

        /// <summary>
        /// 当前搜索关键字。
        /// </summary>
        public string SearchText
        {
            get { return _filter.SearchText; }
        }

        /// <summary>
        /// 当前类型筛选。
        /// All / Cylinder / Vacuum / Gripper / StackLight
        /// </summary>
        public string TypeFilter
        {
            get { return _filter.TypeFilter; }
        }

        /// <summary>
        /// 当前显示列表总数。
        /// </summary>
        public int TotalCount
        {
            get { return _totalCount; }
            private set { SetProperty(ref _totalCount, value); }
        }

        public int CylinderCount
        {
            get { return _cylinderCount; }
            private set { SetProperty(ref _cylinderCount, value); }
        }

        public int VacuumCount
        {
            get { return _vacuumCount; }
            private set { SetProperty(ref _vacuumCount, value); }
        }

        public int GripperCount
        {
            get { return _gripperCount; }
            private set { SetProperty(ref _gripperCount, value); }
        }

        public int StackLightCount
        {
            get { return _stackLightCount; }
            private set { SetProperty(ref _stackLightCount, value); }
        }

        /// <summary>
        /// 首次加载。
        /// </summary>
        public async Task<Result> LoadAsync()
        {
            return await ReloadAsync();
        }

        /// <summary>
        /// 定时刷新运行态。
        /// </summary>
        public async Task<Result> RefreshAsync()
        {
            return await ReloadAsync();
        }

        /// <summary>
        /// 设置搜索关键字。
        /// 页面内搜索不重新查库，只重建筛选结果。
        /// </summary>
        public void SetSearchText(string searchText)
        {
            searchText = searchText ?? string.Empty;
            if (string.Equals(_filter.SearchText, searchText, StringComparison.Ordinal))
                return;

            _filter.SearchText = searchText;
            OnPropertyChanged(nameof(SearchText));

            ApplyFilterAndSelection();
        }

        /// <summary>
        /// 设置类型筛选。
        /// </summary>
        public void SetTypeFilter(string typeFilter)
        {
            typeFilter = string.IsNullOrWhiteSpace(typeFilter) ? "All" : typeFilter;
            if (string.Equals(_filter.TypeFilter, typeFilter, StringComparison.OrdinalIgnoreCase))
                return;

            _filter.TypeFilter = typeFilter;
            OnPropertyChanged(nameof(TypeFilter));

            ApplyFilterAndSelection();
        }

        /// <summary>
        /// 选中指定执行器对象。
        /// 左侧列表点击后通过 ItemKey 选中对应原始快照。
        /// </summary>
        public void SelectItem(string itemKey)
        {
            if (string.IsNullOrWhiteSpace(itemKey))
                return;

            var selected = _filteredSnapshots.FirstOrDefault(x =>
                string.Equals(x.ItemKey, itemKey, StringComparison.OrdinalIgnoreCase));

            if (selected == null)
                return;

            SelectedSnapshot = selected;
            RefreshSelectionViewData();
        }

        /// <summary>
        /// 更新动作面板选项。
        /// 页面层只负责把控件当前选项同步进来，
        /// 后续动作区状态重算由 PageModel 统一处理。
        /// </summary>
        public void UpdateActionPanelOptions(
            bool waitFeedback,
            bool waitWorkpiece,
            bool stackLightWithBuzzer)
        {
            if (_waitFeedback == waitFeedback
                && _waitWorkpiece == waitWorkpiece
                && _stackLightWithBuzzer == stackLightWithBuzzer)
            {
                return;
            }

            _waitFeedback = waitFeedback;
            _waitWorkpiece = waitWorkpiece;
            _stackLightWithBuzzer = stackLightWithBuzzer;

            RefreshSelectionViewData();
        }

        /// <summary>
        /// 基于当前缓存选项校验主动作。
        /// 页面层可直接调用，不需要再传选项。
        /// </summary>
        public Result ValidatePrimaryAction()
        {
            return ValidatePrimaryAction(_waitFeedback, _waitWorkpiece);
        }

        /// <summary>
        /// 基于当前缓存选项校验副动作。
        /// </summary>
        public Result ValidateSecondaryAction()
        {
            return ValidateSecondaryAction(_waitFeedback);
        }

        /// <summary>
        /// 基于当前缓存选项校验灯塔目标状态。
        /// </summary>
        public Result ValidateStackLightState(string stateKey)
        {
            return ValidateStackLightState(stateKey, _stackLightWithBuzzer);
        }

        /// <summary>
        /// 基于当前缓存选项执行主动作。
        /// 页面层无需再传等待反馈/工件检测选项。
        /// </summary>
        public Task<Result> ExecutePrimaryActionAsync()
        {
            return ExecutePrimaryActionAsync(_waitFeedback, _waitWorkpiece);
        }

        /// <summary>
        /// 基于当前缓存选项执行副动作。
        /// </summary>
        public Task<Result> ExecuteSecondaryActionAsync()
        {
            return ExecuteSecondaryActionAsync(_waitFeedback);
        }

        /// <summary>
        /// 基于当前缓存选项切换灯塔状态。
        /// </summary>
        public Task<Result> SetStackLightStateAsync(string stateKey)
        {
            return SetStackLightStateAsync(stateKey, _stackLightWithBuzzer);
        }

        /// <summary>
        /// 校验当前主动作是否允许执行。
        /// 该重载保留在 PageModel 内部，便于动作区状态构建时按指定选项重算。
        /// </summary>
        public Result ValidatePrimaryAction(bool waitFeedback, bool waitWorkpiece)
        {
            if (SelectedSnapshot == null)
                return Result.Fail(-2301, "请先选择执行器对象", ResultSource.Motion);

            switch (SelectedSnapshot.ActuatorType)
            {
                case "Cylinder":
                    if (SelectedSnapshot.PrimaryState == true && SelectedSnapshot.SecondaryState != true)
                        return Result.Fail(-2302, "当前气缸已在伸出到位状态，无需重复伸出", ResultSource.Motion);

                    if (waitFeedback)
                    {
                        if (!SelectedSnapshot.UseFeedbackCheck)
                            return Result.Fail(-2303, "当前气缸未启用反馈校验，请取消“等待反馈”或调整配置", ResultSource.Motion);

                        if (!SelectedSnapshot.PrimaryFeedbackBit.HasValue)
                            return Result.Fail(-2304, "当前气缸未配置伸出反馈位，无法等待伸出到位", ResultSource.Motion);
                    }

                    return Result.Ok("允许执行", ResultSource.Motion);

                case "Vacuum":
                    if (SelectedSnapshot.PrimaryState == true && (!waitWorkpiece || SelectedSnapshot.WorkpieceState != false))
                        return Result.Fail(-2305, "当前真空已建立，无需重复吸真空", ResultSource.Motion);

                    if (waitFeedback)
                    {
                        if (!SelectedSnapshot.UseFeedbackCheck)
                            return Result.Fail(-2306, "当前真空未启用反馈校验，请取消“等待反馈”或调整配置", ResultSource.Motion);

                        if (!SelectedSnapshot.PrimaryFeedbackBit.HasValue)
                            return Result.Fail(-2307, "当前真空未配置建压反馈位，无法等待真空建立", ResultSource.Motion);
                    }

                    if (waitWorkpiece)
                    {
                        if (!SelectedSnapshot.UseWorkpieceCheck)
                            return Result.Fail(-2308, "当前真空未启用工件检测校验，请取消“等待工件检测”或调整配置", ResultSource.Motion);

                        if (!SelectedSnapshot.WorkpieceBit.HasValue)
                            return Result.Fail(-2309, "当前真空未配置工件检测位，无法等待工件检测", ResultSource.Motion);
                    }

                    return Result.Ok("允许执行", ResultSource.Motion);

                case "Gripper":
                    if (SelectedSnapshot.PrimaryState == true && (!waitWorkpiece || SelectedSnapshot.WorkpieceState != false))
                        return Result.Fail(-2310, "当前夹爪已在夹紧到位状态，无需重复夹紧", ResultSource.Motion);

                    if (waitFeedback)
                    {
                        if (!SelectedSnapshot.UseFeedbackCheck)
                            return Result.Fail(-2311, "当前夹爪未启用反馈校验，请取消“等待反馈”或调整配置", ResultSource.Motion);

                        if (!SelectedSnapshot.PrimaryFeedbackBit.HasValue)
                            return Result.Fail(-2312, "当前夹爪未配置夹紧反馈位，无法等待夹紧到位", ResultSource.Motion);
                    }

                    if (waitWorkpiece)
                    {
                        if (!SelectedSnapshot.UseWorkpieceCheck)
                            return Result.Fail(-2313, "当前夹爪未启用工件检测校验，请取消“等待工件检测”或调整配置", ResultSource.Motion);

                        if (!SelectedSnapshot.WorkpieceBit.HasValue)
                            return Result.Fail(-2314, "当前夹爪未配置工件检测位，无法等待工件检测", ResultSource.Motion);
                    }

                    return Result.Ok("允许执行", ResultSource.Motion);

                default:
                    return Result.Fail(-2315, "当前对象不支持主动作", ResultSource.Motion);
            }
        }

        /// <summary>
        /// 校验当前副动作是否允许执行。
        /// </summary>
        public Result ValidateSecondaryAction(bool waitFeedback)
        {
            if (SelectedSnapshot == null)
                return Result.Fail(-2320, "请先选择执行器对象", ResultSource.Motion);

            switch (SelectedSnapshot.ActuatorType)
            {
                case "Cylinder":
                    if (SelectedSnapshot.SecondaryState == true && SelectedSnapshot.PrimaryState != true)
                        return Result.Fail(-2321, "当前气缸已在缩回到位状态，无需重复缩回", ResultSource.Motion);

                    if (waitFeedback)
                    {
                        if (!SelectedSnapshot.UseFeedbackCheck)
                            return Result.Fail(-2322, "当前气缸未启用反馈校验，请取消“等待反馈”或调整配置", ResultSource.Motion);

                        if (!SelectedSnapshot.SecondaryFeedbackBit.HasValue)
                            return Result.Fail(-2323, "当前气缸未配置缩回反馈位，无法等待缩回到位", ResultSource.Motion);
                    }

                    return Result.Ok("允许执行", ResultSource.Motion);

                case "Vacuum":
                    if (SelectedSnapshot.SecondaryState == true || SelectedSnapshot.PrimaryState == false)
                        return Result.Fail(-2324, "当前真空已处于释放状态，无需重复关闭真空", ResultSource.Motion);

                    if (waitFeedback)
                    {
                        if (!SelectedSnapshot.UseFeedbackCheck)
                            return Result.Fail(-2325, "当前真空未启用反馈校验，请取消“等待反馈”或调整配置", ResultSource.Motion);

                        if (!SelectedSnapshot.SecondaryFeedbackBit.HasValue && !SelectedSnapshot.PrimaryFeedbackBit.HasValue)
                            return Result.Fail(-2326, "当前真空未配置释放反馈位或建压反馈位，无法等待真空释放", ResultSource.Motion);
                    }

                    return Result.Ok("允许执行", ResultSource.Motion);

                case "Gripper":
                    if (SelectedSnapshot.SecondaryState == true && SelectedSnapshot.PrimaryState != true)
                        return Result.Fail(-2327, "当前夹爪已在打开到位状态，无需重复打开", ResultSource.Motion);

                    if (waitFeedback)
                    {
                        if (!SelectedSnapshot.UseFeedbackCheck)
                            return Result.Fail(-2328, "当前夹爪未启用反馈校验，请取消“等待反馈”或调整配置", ResultSource.Motion);

                        if (!SelectedSnapshot.SecondaryFeedbackBit.HasValue)
                            return Result.Fail(-2329, "当前夹爪未配置打开反馈位，无法等待打开到位", ResultSource.Motion);
                    }

                    return Result.Ok("允许执行", ResultSource.Motion);

                default:
                    return Result.Fail(-2330, "当前对象不支持副动作", ResultSource.Motion);
            }
        }

        /// <summary>
        /// 校验灯塔目标状态是否允许切换。
        /// </summary>
        public Result ValidateStackLightState(string stateKey, bool stackLightWithBuzzer)
        {
            if (SelectedSnapshot == null
                || !string.Equals(SelectedSnapshot.ActuatorType, "StackLight", StringComparison.OrdinalIgnoreCase))
            {
                return Result.Fail(-2335, "请先选择灯塔对象", ResultSource.Motion);
            }

            if (!SelectedSnapshot.HasAnyStackLightOutput)
                return Result.Fail(-2336, "当前灯塔未配置任何输出位，无法执行状态切换", ResultSource.Motion);

            StackLightState targetState;
            if (!TryParseStackLightState(stateKey, out targetState))
                return Result.Fail(-2337, "不支持的灯塔状态: " + stateKey, ResultSource.Motion);

            if (IsStackLightAlreadyInTargetState(SelectedSnapshot, targetState, stackLightWithBuzzer))
                return Result.Fail(-2338, "当前灯塔已处于目标状态，无需重复切换", ResultSource.Motion);

            return Result.Ok("允许执行", ResultSource.Motion);
        }

        /// <summary>
        /// 执行当前主动作。
        /// 实际执行统一走第三层执行器服务。
        /// </summary>
        public async Task<Result> ExecutePrimaryActionAsync(bool waitFeedback, bool waitWorkpiece)
        {
            var snapshot = SelectedSnapshot;
            if (snapshot == null)
                return Result.Fail(-2340, "请先选择执行器对象", ResultSource.Motion);

            var validate = ValidatePrimaryAction(waitFeedback, waitWorkpiece);
            if (!validate.Success)
            {
                ApplyActionResult(snapshot, validate);
                return validate;
            }

            Result result;
            switch (snapshot.ActuatorType)
            {
                case "Cylinder":
                    result = await Task.Run(() => _cylinderService.Extend(snapshot.Name, waitFeedback));
                    break;

                case "Vacuum":
                    result = await Task.Run(() => _vacuumService.VacuumOn(snapshot.Name, waitFeedback, waitWorkpiece));
                    break;

                case "Gripper":
                    result = await Task.Run(() => _gripperService.Close(snapshot.Name, waitFeedback, waitWorkpiece));
                    break;

                default:
                    result = Result.Fail(-2341, "当前对象不支持主动作", ResultSource.Motion);
                    break;
            }

            ApplyActionResult(snapshot, result);
            return result;
        }

        /// <summary>
        /// 执行当前副动作。
        /// </summary>
        public async Task<Result> ExecuteSecondaryActionAsync(bool waitFeedback)
        {
            var snapshot = SelectedSnapshot;
            if (snapshot == null)
                return Result.Fail(-2342, "请先选择执行器对象", ResultSource.Motion);

            var validate = ValidateSecondaryAction(waitFeedback);
            if (!validate.Success)
            {
                ApplyActionResult(snapshot, validate);
                return validate;
            }

            Result result;
            switch (snapshot.ActuatorType)
            {
                case "Cylinder":
                    result = await Task.Run(() => _cylinderService.Retract(snapshot.Name, waitFeedback));
                    break;

                case "Vacuum":
                    result = await Task.Run(() => _vacuumService.VacuumOff(snapshot.Name, waitFeedback));
                    break;

                case "Gripper":
                    result = await Task.Run(() => _gripperService.Open(snapshot.Name, waitFeedback));
                    break;

                default:
                    result = Result.Fail(-2343, "当前对象不支持副动作", ResultSource.Motion);
                    break;
            }

            ApplyActionResult(snapshot, result);
            return result;
        }

        /// <summary>
        /// 切换灯塔状态。
        /// </summary>
        public async Task<Result> SetStackLightStateAsync(string stateKey, bool stackLightWithBuzzer)
        {
            var snapshot = SelectedSnapshot;
            if (snapshot == null)
                return Result.Fail(-2344, "请先选择灯塔对象", ResultSource.Motion);

            var validate = ValidateStackLightState(stateKey, stackLightWithBuzzer);
            if (!validate.Success)
            {
                ApplyActionResult(snapshot, validate);
                return validate;
            }

            Result result;
            switch (stateKey)
            {
                case "Off":
                    result = await Task.Run(() => _stackLightService.TurnOff(snapshot.Name));
                    break;

                case "Idle":
                    result = await Task.Run(() => _stackLightService.SetIdle(snapshot.Name));
                    break;

                case "Running":
                    result = await Task.Run(() => _stackLightService.SetRunning(snapshot.Name));
                    break;

                case "Warning":
                    result = await Task.Run(() => _stackLightService.SetWarning(snapshot.Name, stackLightWithBuzzer));
                    break;

                case "Alarm":
                    result = await Task.Run(() => _stackLightService.SetAlarm(snapshot.Name, stackLightWithBuzzer));
                    break;

                default:
                    result = Result.Fail(-2345, "不支持的灯塔状态", ResultSource.Motion);
                    break;
            }

            ApplyActionResult(snapshot, result);
            return result;
        }

        /// <summary>
        /// 首次加载与定时刷新共用的数据刷新入口。
        /// </summary>
        private async Task<Result> ReloadAsync()
        {
            return await Task.Run(() =>
            {
                var previousSelectedKey = SelectedItemKey;

                _allSnapshots = BuildAllSnapshotsFromMachineContext();
                RefreshRuntimeStateCore(_allSnapshots);
                ApplyFilterAndSelection(previousSelectedKey);

                return Result.Ok("执行器控制页加载成功");
            });
        }

        /// <summary>
        /// 刷新当前选中对象对应的派生显示数据。
        /// 第二阶段开始统一由这里维护：
        /// - SelectedDetail
        /// - SelectedActionPanel
        /// - SelectedItemKey 对应通知
        /// </summary>
        private void RefreshSelectionViewData()
        {
            SelectedDetail = _displayBuilder.BuildDetailData(SelectedSnapshot);
            SelectedActionPanel = BuildActionPanelStateCore();

            OnPropertyChanged(nameof(SelectedItemKey));
        }

        /// <summary>
        /// 构建右侧动作面板状态。
        /// 第二阶段开始，动作面板状态由模型内部统一维护，
        /// 页面层不再直接参与组装。
        /// </summary>
        private MotionActuatorActionPanelState BuildActionPanelStateCore()
        {
            var state = MotionActuatorActionPanelState.CreateDefault();

            state.WaitFeedback = _waitFeedback;
            state.WaitWorkpiece = _waitWorkpiece;
            state.StackLightWithBuzzer = _stackLightWithBuzzer;

            if (SelectedSnapshot == null)
                return state;

            state.TitleText = SelectedSnapshot.TypeDisplay + " / " + SelectedSnapshot.DisplayTitle;
            state.SubTitleText = SelectedSnapshot.Name;

            var isStackLight = string.Equals(SelectedSnapshot.ActuatorType, "StackLight", StringComparison.OrdinalIgnoreCase);
            var canUseWorkpiece =
                string.Equals(SelectedSnapshot.ActuatorType, "Vacuum", StringComparison.OrdinalIgnoreCase)
                || string.Equals(SelectedSnapshot.ActuatorType, "Gripper", StringComparison.OrdinalIgnoreCase);

            state.ShowWaitFeedback = !isStackLight;
            state.ShowWaitWorkpiece = canUseWorkpiece;
            state.ShowStackLightWithBuzzer = isStackLight;

            if (isStackLight)
            {
                BuildStackLightButtons(state, _stackLightWithBuzzer);
                return state;
            }

            BuildNormalActionButtons(state, _waitFeedback, _waitWorkpiece);
            return state;
        }

        /// <summary>
        /// 从 MachineContext 构建执行器静态对象列表。
        /// 第一阶段仍保留部分显示字段在 Snapshot 中，后续如有必要再继续收口。
        /// </summary>
        private List<MotionActuatorSnapshot> BuildAllSnapshotsFromMachineContext()
        {
            var machine = MachineContext.Instance;
            var list = new List<MotionActuatorSnapshot>();

            if (machine.Cylinders != null)
            {
                foreach (var item in machine.Cylinders.Values
                    .Where(x => x != null && x.IsEnabled)
                    .OrderBy(x => x.SortOrder)
                    .ThenBy(x => x.Name))
                {
                    list.Add(new MotionActuatorSnapshot
                    {
                        ActuatorType = "Cylinder",
                        TypeDisplay = "气缸",
                        Name = item.Name,
                        DisplayName = item.DisplayName,
                        IsEnabled = item.IsEnabled,
                        SortOrder = item.SortOrder,
                        Description = item.Description,
                        Remark = item.Remark,
                        ControlModeText = ResolveDriveModeText(item.DriveMode),
                        PrimaryOutputBit = item.ExtendOutputBit,
                        SecondaryOutputBit = item.RetractOutputBit,
                        PrimaryFeedbackBit = item.ExtendFeedbackBit,
                        SecondaryFeedbackBit = item.RetractFeedbackBit,
                        PrimaryOutputText = FormatIoText("伸出输出", "DO", item.ExtendOutputBit),
                        SecondaryOutputText = FormatIoText("缩回输出", "DO", item.RetractOutputBit),
                        PrimaryFeedbackText = FormatIoText("伸出反馈", "DI", item.ExtendFeedbackBit),
                        SecondaryFeedbackText = FormatIoText("缩回反馈", "DI", item.RetractFeedbackBit),
                        WorkpieceText = "工件检测：—",
                        TimeoutText = "伸/缩超时：" + item.ExtendTimeoutMs + " / " + item.RetractTimeoutMs + " ms",
                        CardLine1Text = "模式：" + ResolveDriveModeText(item.DriveMode),
                        CardLine2Text = "DO：伸 " + item.ExtendOutputBit + " / 缩 " + FormatNullableBit(item.RetractOutputBit),
                        PrimaryActionText = "伸出",
                        SecondaryActionText = "缩回",
                        HasSecondaryAction = true,
                        UseFeedbackCheck = item.UseFeedbackCheck,
                        UseWorkpieceCheck = false,
                        LastActionMessage = "最近操作：未执行",
                        LastActionLevel = "Secondary"
                    });
                }
            }

            if (machine.Vacuums != null)
            {
                foreach (var item in machine.Vacuums.Values
                    .Where(x => x != null && x.IsEnabled)
                    .OrderBy(x => x.SortOrder)
                    .ThenBy(x => x.Name))
                {
                    list.Add(new MotionActuatorSnapshot
                    {
                        ActuatorType = "Vacuum",
                        TypeDisplay = "真空",
                        Name = item.Name,
                        DisplayName = item.DisplayName,
                        IsEnabled = item.IsEnabled,
                        SortOrder = item.SortOrder,
                        Description = item.Description,
                        Remark = item.Remark,
                        ControlModeText = item.KeepVacuumOnAfterDetected ? "吸附保持" : "普通释放",
                        PrimaryOutputBit = item.VacuumOnOutputBit,
                        SecondaryOutputBit = item.BlowOffOutputBit,
                        PrimaryFeedbackBit = item.VacuumFeedbackBit,
                        SecondaryFeedbackBit = item.ReleaseFeedbackBit,
                        WorkpieceBit = item.WorkpiecePresentBit,
                        PrimaryOutputText = FormatIoText("吸真空输出", "DO", item.VacuumOnOutputBit),
                        SecondaryOutputText = FormatIoText("破真空输出", "DO", item.BlowOffOutputBit),
                        PrimaryFeedbackText = FormatIoText("建压反馈", "DI", item.VacuumFeedbackBit),
                        SecondaryFeedbackText = FormatIoText("释放反馈", "DI", item.ReleaseFeedbackBit),
                        WorkpieceText = FormatIoText("工件检测", "DI", item.WorkpiecePresentBit),
                        TimeoutText = "建压/释放超时：" + item.VacuumBuildTimeoutMs + " / " + item.ReleaseTimeoutMs + " ms",
                        CardLine1Text = "模式：" + (item.KeepVacuumOnAfterDetected ? "保持吸附" : "允许释放"),
                        CardLine2Text = "DO：吸 " + item.VacuumOnOutputBit + " / 破 " + FormatNullableBit(item.BlowOffOutputBit),
                        PrimaryActionText = "吸真空",
                        SecondaryActionText = "关闭真空",
                        HasSecondaryAction = true,
                        UseFeedbackCheck = item.UseFeedbackCheck,
                        UseWorkpieceCheck = item.UseWorkpieceCheck,
                        LastActionMessage = "最近操作：未执行",
                        LastActionLevel = "Secondary"
                    });
                }
            }

            if (machine.Grippers != null)
            {
                foreach (var item in machine.Grippers.Values
                    .Where(x => x != null && x.IsEnabled)
                    .OrderBy(x => x.SortOrder)
                    .ThenBy(x => x.Name))
                {
                    list.Add(new MotionActuatorSnapshot
                    {
                        ActuatorType = "Gripper",
                        TypeDisplay = "夹爪",
                        Name = item.Name,
                        DisplayName = item.DisplayName,
                        IsEnabled = item.IsEnabled,
                        SortOrder = item.SortOrder,
                        Description = item.Description,
                        Remark = item.Remark,
                        ControlModeText = ResolveDriveModeText(item.DriveMode),
                        PrimaryOutputBit = item.CloseOutputBit,
                        SecondaryOutputBit = item.OpenOutputBit,
                        PrimaryFeedbackBit = item.CloseFeedbackBit,
                        SecondaryFeedbackBit = item.OpenFeedbackBit,
                        WorkpieceBit = item.WorkpiecePresentBit,
                        PrimaryOutputText = FormatIoText("夹紧输出", "DO", item.CloseOutputBit),
                        SecondaryOutputText = FormatIoText("打开输出", "DO", item.OpenOutputBit),
                        PrimaryFeedbackText = FormatIoText("夹紧反馈", "DI", item.CloseFeedbackBit),
                        SecondaryFeedbackText = FormatIoText("打开反馈", "DI", item.OpenFeedbackBit),
                        WorkpieceText = FormatIoText("工件检测", "DI", item.WorkpiecePresentBit),
                        TimeoutText = "夹/开超时：" + item.CloseTimeoutMs + " / " + item.OpenTimeoutMs + " ms",
                        CardLine1Text = "模式：" + ResolveDriveModeText(item.DriveMode),
                        CardLine2Text = "DO：夹 " + item.CloseOutputBit + " / 开 " + FormatNullableBit(item.OpenOutputBit),
                        PrimaryActionText = "夹紧",
                        SecondaryActionText = "打开",
                        HasSecondaryAction = true,
                        UseFeedbackCheck = item.UseFeedbackCheck,
                        UseWorkpieceCheck = item.UseWorkpieceCheck,
                        LastActionMessage = "最近操作：未执行",
                        LastActionLevel = "Secondary"
                    });
                }
            }

            if (machine.StackLights != null)
            {
                foreach (var item in machine.StackLights.Values
                    .Where(x => x != null && x.IsEnabled)
                    .OrderBy(x => x.SortOrder)
                    .ThenBy(x => x.Name))
                {
                    list.Add(new MotionActuatorSnapshot
                    {
                        ActuatorType = "StackLight",
                        TypeDisplay = "灯塔",
                        Name = item.Name,
                        DisplayName = item.DisplayName,
                        IsEnabled = item.IsEnabled,
                        SortOrder = item.SortOrder,
                        Description = item.Description,
                        Remark = item.Remark,
                        ControlModeText = item.AllowMultiSegmentOn ? "允许多段同亮" : "单段控制",
                        RedOutputBit = item.RedOutputBit,
                        YellowOutputBit = item.YellowOutputBit,
                        GreenOutputBit = item.GreenOutputBit,
                        BlueOutputBit = item.BlueOutputBit,
                        BuzzerOutputBit = item.BuzzerOutputBit,
                        PrimaryOutputText = FormatIoText("红灯输出", "DO", item.RedOutputBit),
                        SecondaryOutputText = FormatIoText("黄灯输出", "DO", item.YellowOutputBit),
                        PrimaryFeedbackText = FormatIoText("绿灯输出", "DO", item.GreenOutputBit),
                        SecondaryFeedbackText = FormatIoText("蓝灯输出", "DO", item.BlueOutputBit),
                        WorkpieceText = FormatIoText("蜂鸣器输出", "DO", item.BuzzerOutputBit),
                        TimeoutText = "蜂鸣联动：警告=" + BoolToChinese(item.EnableBuzzerOnWarning)
                            + " / 报警=" + BoolToChinese(item.EnableBuzzerOnAlarm),
                        CardLine1Text = "红/黄：" + FormatNullableBit(item.RedOutputBit) + " / " + FormatNullableBit(item.YellowOutputBit),
                        CardLine2Text = "绿/蓝/鸣：" + FormatNullableBit(item.GreenOutputBit) + " / "
                            + FormatNullableBit(item.BlueOutputBit) + " / " + FormatNullableBit(item.BuzzerOutputBit),
                        PrimaryActionText = string.Empty,
                        SecondaryActionText = string.Empty,
                        HasSecondaryAction = false,
                        UseFeedbackCheck = false,
                        UseWorkpieceCheck = false,
                        LastActionMessage = "最近操作：未执行",
                        LastActionLevel = "Secondary"
                    });
                }
            }

            return list
                .OrderBy(x => GetActuatorTypeSort(x.ActuatorType))
                .ThenBy(x => x.SortOrder)
                .ThenBy(x => x.Name)
                .ToList();
        }

        /// <summary>
        /// 刷新所有执行器运行态。
        /// 这一层仍然直接写入 Snapshot，
        /// 第二阶段不继续拆出独立 RuntimeAssembler，避免过度设计。
        /// </summary>
        private void RefreshRuntimeStateCore(IEnumerable<MotionActuatorSnapshot> snapshots)
        {
            foreach (var snapshot in snapshots)
            {
                if (snapshot == null)
                    continue;

                switch (snapshot.ActuatorType)
                {
                    case "Cylinder":
                        RefreshCylinderState(snapshot);
                        break;

                    case "Vacuum":
                        RefreshVacuumState(snapshot);
                        break;

                    case "Gripper":
                        RefreshGripperState(snapshot);
                        break;

                    case "StackLight":
                        RefreshStackLightState(snapshot);
                        break;
                }
            }
        }

        private void RefreshCylinderState(MotionActuatorSnapshot snapshot)
        {
            var isExtended = TryReadBoolResult(() => _cylinderService.IsExtended(snapshot.Name));
            var isRetracted = TryReadBoolResult(() => _cylinderService.IsRetracted(snapshot.Name));

            snapshot.PrimaryState = isExtended;
            snapshot.SecondaryState = isRetracted;
            snapshot.WorkpieceState = null;

            if (!snapshot.PrimaryFeedbackBit.HasValue && !snapshot.SecondaryFeedbackBit.HasValue)
            {
                snapshot.StateText = "未配反馈";
                snapshot.StateLevel = "Warning";
                snapshot.DetailText = "未配置伸出/缩回反馈位";
                snapshot.FooterText = "反馈：— / —";
                snapshot.HasFault = false;
            }
            else if (isExtended == true && isRetracted == true)
            {
                snapshot.StateText = "反馈冲突";
                snapshot.StateLevel = "Danger";
                snapshot.DetailText = "伸出与缩回反馈同时为到位";
                snapshot.FooterText = "伸出=Y / 缩回=Y";
                snapshot.HasFault = true;
            }
            else if (isExtended == true)
            {
                snapshot.StateText = "伸出到位";
                snapshot.StateLevel = "Success";
                snapshot.DetailText = "气缸当前处于伸出端";
                snapshot.FooterText = "伸出=Y / 缩回=" + BoolToShortText(isRetracted);
                snapshot.HasFault = false;
            }
            else if (isRetracted == true)
            {
                snapshot.StateText = "缩回到位";
                snapshot.StateLevel = "Primary";
                snapshot.DetailText = "气缸当前处于缩回端";
                snapshot.FooterText = "伸出=" + BoolToShortText(isExtended) + " / 缩回=Y";
                snapshot.HasFault = false;
            }
            else if (isExtended == false && isRetracted == false)
            {
                snapshot.StateText = "双未到位";
                snapshot.StateLevel = "Warning";
                snapshot.DetailText = "伸出与缩回反馈均未到位";
                snapshot.FooterText = "伸出=N / 缩回=N";
                snapshot.HasFault = false;
            }
            else
            {
                snapshot.StateText = "状态未知";
                snapshot.StateLevel = "Secondary";
                snapshot.DetailText = "当前反馈不足以判断气缸端位";
                snapshot.FooterText = "伸出=" + BoolToShortText(isExtended) + " / 缩回=" + BoolToShortText(isRetracted);
                snapshot.HasFault = false;
            }

            snapshot.RuntimeUpdateTimeText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void RefreshVacuumState(MotionActuatorSnapshot snapshot)
        {
            var isBuilt = TryReadBoolResult(() => _vacuumService.IsVacuumBuilt(snapshot.Name));
            var isReleased = TryReadBoolResult(() => _vacuumService.IsReleased(snapshot.Name));
            var hasWorkpiece = TryReadBoolResult(() => _vacuumService.HasWorkpiece(snapshot.Name));

            snapshot.PrimaryState = isBuilt;
            snapshot.SecondaryState = isReleased;
            snapshot.WorkpieceState = hasWorkpiece;

            if (!snapshot.PrimaryFeedbackBit.HasValue && !snapshot.SecondaryFeedbackBit.HasValue)
            {
                snapshot.StateText = "未配反馈";
                snapshot.StateLevel = "Warning";
                snapshot.DetailText = "未配置建压/释放反馈位";
                snapshot.FooterText = "工件=" + BoolToShortText(hasWorkpiece);
                snapshot.HasFault = false;
            }
            else if (isBuilt == true && isReleased == true)
            {
                snapshot.StateText = "反馈冲突";
                snapshot.StateLevel = "Danger";
                snapshot.DetailText = "建压与释放反馈同时成立";
                snapshot.FooterText = "建压=Y / 释放=Y";
                snapshot.HasFault = true;
            }
            else if (isBuilt == true && hasWorkpiece == true)
            {
                snapshot.StateText = "已吸附";
                snapshot.StateLevel = "Success";
                snapshot.DetailText = "真空已建立，且已检测到工件";
                snapshot.FooterText = "建压=Y / 工件=Y";
                snapshot.HasFault = false;
            }
            else if (isBuilt == true)
            {
                snapshot.StateText = "真空建立";
                snapshot.StateLevel = "Success";
                snapshot.DetailText = "真空已建立";
                snapshot.FooterText = "建压=Y / 工件=" + BoolToShortText(hasWorkpiece);
                snapshot.HasFault = false;
            }
            else if (isReleased == true)
            {
                snapshot.StateText = "已释放";
                snapshot.StateLevel = "Primary";
                snapshot.DetailText = "真空当前处于释放状态";
                snapshot.FooterText = "释放=Y / 工件=" + BoolToShortText(hasWorkpiece);
                snapshot.HasFault = false;
            }
            else if (isBuilt == false && isReleased == false)
            {
                snapshot.StateText = "未建压";
                snapshot.StateLevel = "Secondary";
                snapshot.DetailText = "当前既未检测到建压，也未检测到释放到位";
                snapshot.FooterText = "建压=N / 释放=N";
                snapshot.HasFault = false;
            }
            else
            {
                snapshot.StateText = "状态未知";
                snapshot.StateLevel = "Secondary";
                snapshot.DetailText = "当前反馈不足以判断真空状态";
                snapshot.FooterText = "工件=" + BoolToShortText(hasWorkpiece);
                snapshot.HasFault = false;
            }

            snapshot.RuntimeUpdateTimeText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void RefreshGripperState(MotionActuatorSnapshot snapshot)
        {
            var isClosed = TryReadBoolResult(() => _gripperService.IsClosed(snapshot.Name));
            var isOpened = TryReadBoolResult(() => _gripperService.IsOpened(snapshot.Name));
            var hasWorkpiece = TryReadBoolResult(() => _gripperService.HasWorkpiece(snapshot.Name));

            snapshot.PrimaryState = isClosed;
            snapshot.SecondaryState = isOpened;
            snapshot.WorkpieceState = hasWorkpiece;

            if (!snapshot.PrimaryFeedbackBit.HasValue && !snapshot.SecondaryFeedbackBit.HasValue)
            {
                snapshot.StateText = "未配反馈";
                snapshot.StateLevel = "Warning";
                snapshot.DetailText = "未配置夹紧/打开反馈位";
                snapshot.FooterText = "工件=" + BoolToShortText(hasWorkpiece);
                snapshot.HasFault = false;
            }
            else if (isClosed == true && isOpened == true)
            {
                snapshot.StateText = "反馈冲突";
                snapshot.StateLevel = "Danger";
                snapshot.DetailText = "夹紧与打开反馈同时成立";
                snapshot.FooterText = "夹紧=Y / 打开=Y";
                snapshot.HasFault = true;
            }
            else if (isClosed == true && hasWorkpiece == true)
            {
                snapshot.StateText = "夹紧有料";
                snapshot.StateLevel = "Success";
                snapshot.DetailText = "夹爪夹紧到位，且已检测到工件";
                snapshot.FooterText = "夹紧=Y / 工件=Y";
                snapshot.HasFault = false;
            }
            else if (isClosed == true)
            {
                snapshot.StateText = "夹紧到位";
                snapshot.StateLevel = "Success";
                snapshot.DetailText = "夹爪已夹紧到位";
                snapshot.FooterText = "夹紧=Y / 工件=" + BoolToShortText(hasWorkpiece);
                snapshot.HasFault = false;
            }
            else if (isOpened == true)
            {
                snapshot.StateText = "打开到位";
                snapshot.StateLevel = "Primary";
                snapshot.DetailText = "夹爪已打开到位";
                snapshot.FooterText = "打开=Y / 工件=" + BoolToShortText(hasWorkpiece);
                snapshot.HasFault = false;
            }
            else if (isClosed == false && isOpened == false)
            {
                snapshot.StateText = "双未到位";
                snapshot.StateLevel = "Secondary";
                snapshot.DetailText = "夹紧与打开反馈均未到位";
                snapshot.FooterText = "夹紧=N / 打开=N";
                snapshot.HasFault = false;
            }
            else
            {
                snapshot.StateText = "状态未知";
                snapshot.StateLevel = "Secondary";
                snapshot.DetailText = "当前反馈不足以判断夹爪状态";
                snapshot.FooterText = "工件=" + BoolToShortText(hasWorkpiece);
                snapshot.HasFault = false;
            }

            snapshot.RuntimeUpdateTimeText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void RefreshStackLightState(MotionActuatorSnapshot snapshot)
        {
            snapshot.RedOn = ReadDoState(snapshot.RedOutputBit);
            snapshot.YellowOn = ReadDoState(snapshot.YellowOutputBit);
            snapshot.GreenOn = ReadDoState(snapshot.GreenOutputBit);
            snapshot.BlueOn = ReadDoState(snapshot.BlueOutputBit);
            snapshot.BuzzerOn = ReadDoState(snapshot.BuzzerOutputBit);

            var onSegments = new List<string>();
            if (snapshot.RedOn == true) onSegments.Add("红");
            if (snapshot.YellowOn == true) onSegments.Add("黄");
            if (snapshot.GreenOn == true) onSegments.Add("绿");
            if (snapshot.BlueOn == true) onSegments.Add("蓝");

            if (!snapshot.HasAnyStackLightOutput)
            {
                snapshot.StateText = "未配输出";
                snapshot.StateLevel = "Warning";
                snapshot.DetailText = "未配置任何灯塔输出位";
                snapshot.FooterText = "亮段：—";
                snapshot.HasFault = false;
            }
            else if (onSegments.Count == 0 && snapshot.BuzzerOn != true)
            {
                snapshot.StateText = "全灭";
                snapshot.StateLevel = "Secondary";
                snapshot.DetailText = "当前所有灯段均关闭";
                snapshot.FooterText = "亮段：无 / 蜂鸣=N";
                snapshot.HasFault = false;
            }
            else if (onSegments.Count == 1)
            {
                snapshot.StateText = onSegments[0] + (snapshot.BuzzerOn == true ? "+蜂鸣" : "灯");
                snapshot.StateLevel = snapshot.RedOn == true
                    ? "Danger"
                    : (snapshot.YellowOn == true ? "Warning" : "Success");
                if (snapshot.BlueOn == true)
                    snapshot.StateLevel = "Primary";

                snapshot.DetailText = "红=" + BoolToShortText(snapshot.RedOn)
                    + " / 黄=" + BoolToShortText(snapshot.YellowOn)
                    + " / 绿=" + BoolToShortText(snapshot.GreenOn)
                    + " / 蓝=" + BoolToShortText(snapshot.BlueOn)
                    + " / 鸣=" + BoolToShortText(snapshot.BuzzerOn);
                snapshot.FooterText = "亮段：" + onSegments[0] + " / 蜂鸣=" + BoolToShortText(snapshot.BuzzerOn);
                snapshot.HasFault = false;
            }
            else
            {
                snapshot.StateText = snapshot.BuzzerOn == true ? "多段+蜂鸣" : "多段点亮";
                snapshot.StateLevel = snapshot.RedOn == true
                    ? "Danger"
                    : (snapshot.YellowOn == true ? "Warning" : "Primary");
                snapshot.DetailText = "红=" + BoolToShortText(snapshot.RedOn)
                    + " / 黄=" + BoolToShortText(snapshot.YellowOn)
                    + " / 绿=" + BoolToShortText(snapshot.GreenOn)
                    + " / 蓝=" + BoolToShortText(snapshot.BlueOn)
                    + " / 鸣=" + BoolToShortText(snapshot.BuzzerOn);
                snapshot.FooterText = "亮段：" + string.Join("/", onSegments) + " / 蜂鸣=" + BoolToShortText(snapshot.BuzzerOn);
                snapshot.HasFault = false;
            }

            snapshot.RuntimeUpdateTimeText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 应用筛选并尽量恢复选中项。
        /// </summary>
        private void ApplyFilterAndSelection()
        {
            var previousSelectedKey = SelectedItemKey;
            ApplyFilterAndSelection(previousSelectedKey);
        }

        private void ApplyFilterAndSelection(string previousSelectedKey)
        {
            _filter.Normalize();

            _filteredSnapshots = _allSnapshots
                .Where(x => _filter.IsMatch(x))
                .OrderBy(x => GetActuatorTypeSort(x.ActuatorType))
                .ThenBy(x => x.SortOrder)
                .ThenBy(x => x.Name)
                .ToList();

            _pageItems = _filteredSnapshots
                .Select(x => _displayBuilder.BuildListItem(x))
                .Where(x => x != null)
                .ToList();

            UpdateSummary(_filteredSnapshots);

            MotionActuatorSnapshot selected = null;
            if (!string.IsNullOrWhiteSpace(previousSelectedKey))
            {
                selected = _filteredSnapshots.FirstOrDefault(x =>
                    string.Equals(x.ItemKey, previousSelectedKey, StringComparison.OrdinalIgnoreCase));
            }

            if (selected == null && _filteredSnapshots.Count > 0)
                selected = _filteredSnapshots[0];

            SelectedSnapshot = selected;
            RefreshSelectionViewData();

            OnPropertyChanged(nameof(PageItems));
        }

        /// <summary>
        /// 构建普通执行器主/副按钮状态。
        /// 第二阶段开始由模型直接生成最终动作面板状态，
        /// 页面层不再参与按钮状态组装。
        /// </summary>
        private void BuildNormalActionButtons(
            MotionActuatorActionPanelState state,
            bool waitFeedback,
            bool waitWorkpiece)
        {
            var primaryValidate = ValidatePrimaryAction(waitFeedback, waitWorkpiece);
            var secondaryValidate = ValidateSecondaryAction(waitFeedback);

            state.PrimaryButton.Text = ResolvePrimaryActionButtonText(SelectedSnapshot, waitWorkpiece);
            state.PrimaryButton.Visible = true;
            state.PrimaryButton.Enabled = primaryValidate.Success;
            state.PrimaryButton.Type = ResolvePrimaryButtonType(SelectedSnapshot);

            state.SecondaryButton.Text = ResolveSecondaryActionButtonText(SelectedSnapshot);
            state.SecondaryButton.Visible = SelectedSnapshot != null && SelectedSnapshot.HasSecondaryAction;
            state.SecondaryButton.Enabled = SelectedSnapshot != null
                && SelectedSnapshot.HasSecondaryAction
                && secondaryValidate.Success;
            state.SecondaryButton.Type = ResolveSecondaryButtonType(SelectedSnapshot);

            state.OffButton.Visible = false;
            state.IdleButton.Visible = false;
            state.RunningButton.Visible = false;
            state.WarningButton.Visible = false;
            state.AlarmButton.Visible = false;
        }

        /// <summary>
        /// 构建灯塔按钮组状态。
        /// 灯塔场景必须显式隐藏普通主/副动作按钮，
        /// 避免第三行按钮区同时出现两套按钮。
        /// </summary>
        private void BuildStackLightButtons(
            MotionActuatorActionPanelState state,
            bool stackLightWithBuzzer)
        {
            var isOffCurrent = IsStackLightCurrentState("Off", stackLightWithBuzzer);
            var isIdleCurrent = IsStackLightCurrentState("Idle", stackLightWithBuzzer);
            var isRunningCurrent = IsStackLightCurrentState("Running", stackLightWithBuzzer);
            var isWarningCurrent = IsStackLightCurrentState("Warning", stackLightWithBuzzer);
            var isAlarmCurrent = IsStackLightCurrentState("Alarm", stackLightWithBuzzer);

            state.PrimaryButton.Visible = false;
            state.PrimaryButton.Enabled = false;

            state.SecondaryButton.Visible = false;
            state.SecondaryButton.Enabled = false;

            state.OffButton.Text = isOffCurrent ? "熄灭（当前）" : "熄灭";
            state.OffButton.Visible = true;
            state.OffButton.Enabled = ValidateStackLightState("Off", stackLightWithBuzzer).Success;
            state.OffButton.Type = "Default";

            state.IdleButton.Text = isIdleCurrent ? "空闲（当前）" : "空闲";
            state.IdleButton.Visible = true;
            state.IdleButton.Enabled = ValidateStackLightState("Idle", stackLightWithBuzzer).Success;
            state.IdleButton.Type = isIdleCurrent ? "Success" : "Primary";

            state.RunningButton.Text = isRunningCurrent ? "运行（当前）" : "运行";
            state.RunningButton.Visible = true;
            state.RunningButton.Enabled = ValidateStackLightState("Running", stackLightWithBuzzer).Success;
            state.RunningButton.Type = isRunningCurrent ? "Success" : "Primary";

            state.WarningButton.Text = isWarningCurrent ? "警告（当前）" : "警告";
            state.WarningButton.Visible = true;
            state.WarningButton.Enabled = ValidateStackLightState("Warning", stackLightWithBuzzer).Success;
            state.WarningButton.Type = "Warn";

            state.AlarmButton.Text = isAlarmCurrent ? "报警（当前）" : "报警";
            state.AlarmButton.Visible = true;
            state.AlarmButton.Enabled = ValidateStackLightState("Alarm", stackLightWithBuzzer).Success;
            state.AlarmButton.Type = "Error";
        }

        private void UpdateSummary(IList<MotionActuatorSnapshot> list)
        {
            var source = list ?? new List<MotionActuatorSnapshot>();

            TotalCount = source.Count;
            CylinderCount = source.Count(x => x.ActuatorType == "Cylinder");
            VacuumCount = source.Count(x => x.ActuatorType == "Vacuum");
            GripperCount = source.Count(x => x.ActuatorType == "Gripper");
            StackLightCount = source.Count(x => x.ActuatorType == "StackLight");
        }

        /// <summary>
        /// 当前灯塔是否处于指定状态。
        /// </summary>
        private bool IsStackLightCurrentState(string stateKey, bool stackLightWithBuzzer)
        {
            if (SelectedSnapshot == null
                || !string.Equals(SelectedSnapshot.ActuatorType, "StackLight", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            StackLightState state;
            if (!TryParseStackLightState(stateKey, out state))
                return false;

            return IsStackLightAlreadyInTargetState(SelectedSnapshot, state, stackLightWithBuzzer);
        }

        private static bool TryParseStackLightState(string stateKey, out StackLightState state)
        {
            state = StackLightState.Off;

            switch (stateKey)
            {
                case "Off":
                    state = StackLightState.Off;
                    return true;

                case "Idle":
                    state = StackLightState.Idle;
                    return true;

                case "Running":
                    state = StackLightState.Running;
                    return true;

                case "Warning":
                    state = StackLightState.Warning;
                    return true;

                case "Alarm":
                    state = StackLightState.Alarm;
                    return true;

                default:
                    return false;
            }
        }

        private static bool IsStackLightAlreadyInTargetState(
            MotionActuatorSnapshot snapshot,
            StackLightState targetState,
            bool withBuzzer)
        {
            if (snapshot == null)
                return false;

            switch (targetState)
            {
                case StackLightState.Off:
                    return snapshot.RedOn != true
                        && snapshot.YellowOn != true
                        && snapshot.GreenOn != true
                        && snapshot.BlueOn != true
                        && snapshot.BuzzerOn != true;

                case StackLightState.Idle:
                    return snapshot.BlueOn == true
                        && snapshot.RedOn != true
                        && snapshot.YellowOn != true
                        && snapshot.GreenOn != true
                        && snapshot.BuzzerOn != true;

                case StackLightState.Running:
                    return snapshot.GreenOn == true
                        && snapshot.RedOn != true
                        && snapshot.YellowOn != true
                        && snapshot.BlueOn != true
                        && snapshot.BuzzerOn != true;

                case StackLightState.Warning:
                    return snapshot.YellowOn == true
                        && snapshot.RedOn != true
                        && snapshot.GreenOn != true
                        && snapshot.BlueOn != true
                        && (withBuzzer ? snapshot.BuzzerOn == true : true);

                case StackLightState.Alarm:
                    return snapshot.RedOn == true
                        && snapshot.YellowOn != true
                        && snapshot.GreenOn != true
                        && snapshot.BlueOn != true
                        && (withBuzzer ? snapshot.BuzzerOn == true : true);

                default:
                    return false;
            }
        }

        private static string ResolvePrimaryActionButtonText(MotionActuatorSnapshot snapshot, bool waitWorkpiece)
        {
            if (snapshot == null)
                return "主操作";

            switch (snapshot.ActuatorType)
            {
                case "Cylinder":
                    return snapshot.PrimaryState == true && snapshot.SecondaryState != true
                        ? "伸出（已到位）"
                        : "伸出";

                case "Vacuum":
                    if (snapshot.PrimaryState == true && (!waitWorkpiece || snapshot.WorkpieceState != false))
                        return "吸真空（已建立）";

                    return waitWorkpiece ? "吸真空+检测" : "吸真空";

                case "Gripper":
                    if (snapshot.PrimaryState == true && (!waitWorkpiece || snapshot.WorkpieceState != false))
                        return "夹紧（已到位）";

                    return waitWorkpiece ? "夹紧+检测" : "夹紧";

                default:
                    return "主操作";
            }
        }

        private static string ResolveSecondaryActionButtonText(MotionActuatorSnapshot snapshot)
        {
            if (snapshot == null)
                return "副操作";

            switch (snapshot.ActuatorType)
            {
                case "Cylinder":
                    return snapshot.SecondaryState == true && snapshot.PrimaryState != true
                        ? "缩回（已到位）"
                        : "缩回";

                case "Vacuum":
                    return snapshot.SecondaryState == true || snapshot.PrimaryState == false
                        ? "关闭真空（已释放）"
                        : "关闭真空";

                case "Gripper":
                    return snapshot.SecondaryState == true && snapshot.PrimaryState != true
                        ? "打开（已到位）"
                        : "打开";

                default:
                    return "副操作";
            }
        }

        private static string ResolvePrimaryButtonType(MotionActuatorSnapshot snapshot)
        {
            if (snapshot == null)
                return "Primary";

            if (string.Equals(snapshot.ActuatorType, "Gripper", StringComparison.OrdinalIgnoreCase))
                return "Error";

            return "Primary";
        }

        private static string ResolveSecondaryButtonType(MotionActuatorSnapshot snapshot)
        {
            if (snapshot == null)
                return "Default";

            if (string.Equals(snapshot.ActuatorType, "Vacuum", StringComparison.OrdinalIgnoreCase))
                return "Warn";

            return "Success";
        }

        private static int GetActuatorTypeSort(string actuatorType)
        {
            switch (actuatorType)
            {
                case "Cylinder": return 10;
                case "Vacuum": return 20;
                case "Gripper": return 30;
                case "StackLight": return 40;
                default: return 100;
            }
        }

        private static string ResolveDriveModeText(string driveMode)
        {
            if (string.Equals(driveMode, "Double", StringComparison.OrdinalIgnoreCase))
                return "双线圈";

            if (string.Equals(driveMode, "Single", StringComparison.OrdinalIgnoreCase))
                return "单线圈";

            return string.IsNullOrWhiteSpace(driveMode) ? "—" : driveMode;
        }

        private static string FormatIoText(string label, string ioType, short bit)
        {
            return label + "：" + ioType + " L#" + bit;
        }

        private static string FormatIoText(string label, string ioType, short? bit)
        {
            return label + "：" + (bit.HasValue ? ioType + " L#" + bit.Value : "—");
        }

        private static string FormatNullableBit(short? bit)
        {
            return bit.HasValue ? bit.Value.ToString() : "—";
        }

        private static string BoolToChinese(bool value)
        {
            return value ? "是" : "否";
        }

        private static string BoolToShortText(bool? value)
        {
            if (!value.HasValue)
                return "—";

            return value.Value ? "Y" : "N";
        }

        private static bool? TryReadBoolResult(Func<Result<bool>> func)
        {
            var result = func();
            if (!result.Success)
                return null;

            return result.Item;
        }

        private static bool? ReadDoState(short? logicalBit)
        {
            if (!logicalBit.HasValue)
                return null;

            bool value;
            if (!RuntimeContext.Instance.MotionIo.TryGetDO(logicalBit.Value, out value))
                return null;

            return value;
        }

        private void ApplyActionResult(MotionActuatorSnapshot snapshot, Result result)
        {
            if (snapshot == null || result == null)
                return;

            snapshot.LastActionMessage = BuildLayeredActionMessage(result);
            snapshot.LastActionLevel = result.Success ? "Success" : "Danger";
            snapshot.HasFault = !result.Success;

            if (SelectedSnapshot != null
                && string.Equals(SelectedSnapshot.ItemKey, snapshot.ItemKey, StringComparison.OrdinalIgnoreCase))
            {
                RefreshSelectionViewData();
            }

            OnPropertyChanged(nameof(PageItems));
        }

        private static string BuildLayeredActionMessage(Result result)
        {
            if (result == null)
                return "执行失败：未返回结果";

            if (result.Success)
                return "操作成功：" + result.Message;

            return "操作失败：" + result.Message;
        }

        /// <summary>
        /// 当前选中的原始快照。
        /// 只在 PageModel 内部使用，不对 UI 层公开。
        /// </summary>
        private MotionActuatorSnapshot SelectedSnapshot
        {
            get { return _selectedSnapshot; }
            set { SetProperty(ref _selectedSnapshot, value); }
        }
    }
}