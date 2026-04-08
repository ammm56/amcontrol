using AM.Core.Context;
using AM.DBService.Services.Motion.Actuator;
using AM.Model.Common;
using AM.Model.MotionCard.Actuator;
using AM.PageModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AM.PageModel.Motion
{
    /// <summary>
    /// WinForms 执行器控制页页面模型。
    ///
    /// 职责：
    /// 1. 从 MachineContext 读取执行器静态结构；
    /// 2. 从 RuntimeContext / 执行器服务读取运行态；
    /// 3. 维护类型筛选、关键字筛选；
    /// 4. 维护左侧卡片集合与当前选中项；
    /// 5. 提供右侧控制面板所需的联动校验与实际执行入口。
    /// </summary>
    public class MotionActuatorPageModel : BindableBase
    {
        private readonly CylinderService _cylinderService;
        private readonly VacuumService _vacuumService;
        private readonly GripperService _gripperService;
        private readonly StackLightService _stackLightService;

        private List<MotionActuatorViewItem> _allItems;
        private List<MotionActuatorViewItem> _filteredItems;

        private string _searchText;
        private string _typeFilter;
        private MotionActuatorViewItem _selectedItem;

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

            _allItems = new List<MotionActuatorViewItem>();
            _filteredItems = new List<MotionActuatorViewItem>();

            _searchText = string.Empty;
            _typeFilter = "All";
        }

        /// <summary>
        /// 当前左侧卡片数据。
        /// 执行器控制页不分页，直接交给 VirtualPanel。
        /// </summary>
        public IList<MotionActuatorViewItem> PageItems
        {
            get { return _filteredItems; }
        }

        /// <summary>
        /// 当前选中的执行器对象。
        /// </summary>
        public MotionActuatorViewItem SelectedItem
        {
            get { return _selectedItem; }
            private set { SetProperty(ref _selectedItem, value); }
        }

        /// <summary>
        /// 当前搜索关键字。
        /// </summary>
        public string SearchText
        {
            get { return _searchText; }
        }

        /// <summary>
        /// 当前类型筛选。
        /// All / Cylinder / Vacuum / Gripper / StackLight
        /// </summary>
        public string TypeFilter
        {
            get { return _typeFilter; }
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
            if (string.Equals(_searchText, searchText, StringComparison.Ordinal))
                return;

            _searchText = searchText;
            OnPropertyChanged(nameof(SearchText));

            ApplyFilterAndSelection();
        }

        /// <summary>
        /// 设置类型筛选。
        /// </summary>
        public void SetTypeFilter(string typeFilter)
        {
            typeFilter = string.IsNullOrWhiteSpace(typeFilter) ? "All" : typeFilter;
            if (string.Equals(_typeFilter, typeFilter, StringComparison.OrdinalIgnoreCase))
                return;

            _typeFilter = typeFilter;
            OnPropertyChanged(nameof(TypeFilter));

            ApplyFilterAndSelection();
        }

        /// <summary>
        /// 选中指定执行器对象。
        /// </summary>
        public void SelectItem(string itemKey)
        {
            if (string.IsNullOrWhiteSpace(itemKey))
                return;

            var selected = _filteredItems.FirstOrDefault(x =>
                string.Equals(x.ItemKey, itemKey, StringComparison.OrdinalIgnoreCase));

            if (selected == null)
                return;

            SelectedItem = selected;
        }

        /// <summary>
        /// 首次加载与定时刷新共用的数据刷新入口。
        /// </summary>
        private async Task<Result> ReloadAsync()
        {
            return await Task.Run(() =>
            {
                var previousSelectedKey = SelectedItem == null ? null : SelectedItem.ItemKey;

                _allItems = BuildAllItemsFromMachineContext();
                RefreshRuntimeStateCore(_allItems);
                ApplyFilterAndSelection(previousSelectedKey);

                return Result.Ok("执行器控制页加载成功");
            });
        }

        /// <summary>
        /// 构建右侧控制面板状态。
        /// 页面层只负责把该状态应用到控件，不在 UI 层重复写联动判断。
        /// </summary>
        public MotionActuatorActionPanelState BuildActionPanelState(
            bool waitFeedback,
            bool waitWorkpiece,
            bool stackLightWithBuzzer)
        {
            var state = new MotionActuatorActionPanelState();

            if (SelectedItem == null)
            {
                state.TitleText = "当前对象：未选择";
                state.SubTitleText = "—";
                state.HintText = "请先在左侧选择一个执行器对象。";

                state.ShowNormalActions = true;
                state.ShowStackLightActions = false;

                state.PrimaryButtonText = "主操作";
                state.SecondaryButtonText = "副操作";
                state.PrimaryButtonEnabled = false;
                state.SecondaryButtonEnabled = false;

                state.ShowWaitFeedback = true;
                state.ShowWaitWorkpiece = false;
                state.ShowStackLightWithBuzzer = false;

                return state;
            }

            state.TitleText = SelectedItem.TypeDisplay + " / " + SelectedItem.DisplayTitle;
            state.SubTitleText = SelectedItem.Name;

            if (string.Equals(SelectedItem.ActuatorType, "StackLight", StringComparison.OrdinalIgnoreCase))
            {
                state.ShowNormalActions = false;
                state.ShowStackLightActions = true;

                state.ShowWaitFeedback = false;
                state.ShowWaitWorkpiece = false;
                state.ShowStackLightWithBuzzer = true;

                state.StackLightWithBuzzer = stackLightWithBuzzer;

                state.OffButtonText = IsStackLightCurrentState("Off", stackLightWithBuzzer) ? "熄灭（当前）" : "熄灭";
                state.IdleButtonText = IsStackLightCurrentState("Idle", stackLightWithBuzzer) ? "空闲" : "空闲";
                state.RunningButtonText = IsStackLightCurrentState("Running", stackLightWithBuzzer) ? "运行" : "运行";
                state.WarningButtonText = IsStackLightCurrentState("Warning", stackLightWithBuzzer) ? "警告（当前）" : "警告";
                state.AlarmButtonText = IsStackLightCurrentState("Alarm", stackLightWithBuzzer) ? "报警（当前）" : "报警";

                state.IsOffCurrent = IsStackLightCurrentState("Off", stackLightWithBuzzer);
                state.IsIdleCurrent = IsStackLightCurrentState("Idle", stackLightWithBuzzer);
                state.IsRunningCurrent = IsStackLightCurrentState("Running", stackLightWithBuzzer);
                state.IsWarningCurrent = IsStackLightCurrentState("Warning", stackLightWithBuzzer);
                state.IsAlarmCurrent = IsStackLightCurrentState("Alarm", stackLightWithBuzzer);

                state.OffButtonEnabled = ValidateStackLightState("Off", stackLightWithBuzzer).Success;
                state.IdleButtonEnabled = ValidateStackLightState("Idle", stackLightWithBuzzer).Success;
                state.RunningButtonEnabled = ValidateStackLightState("Running", stackLightWithBuzzer).Success;
                state.WarningButtonEnabled = ValidateStackLightState("Warning", stackLightWithBuzzer).Success;
                state.AlarmButtonEnabled = ValidateStackLightState("Alarm", stackLightWithBuzzer).Success;

                state.HintText = SelectedItem.DetailText;
                if (stackLightWithBuzzer)
                    state.HintText += "；当前已开启蜂鸣联动。";

                return state;
            }

            state.ShowNormalActions = true;
            state.ShowStackLightActions = false;

            state.ShowWaitFeedback = true;
            state.ShowWaitWorkpiece =
                string.Equals(SelectedItem.ActuatorType, "Vacuum", StringComparison.OrdinalIgnoreCase)
                || string.Equals(SelectedItem.ActuatorType, "Gripper", StringComparison.OrdinalIgnoreCase);
            state.ShowStackLightWithBuzzer = false;

            state.WaitFeedback = waitFeedback;
            state.WaitWorkpiece = waitWorkpiece;

            var primaryValidate = ValidatePrimaryAction(waitFeedback, waitWorkpiece);
            var secondaryValidate = ValidateSecondaryAction(waitFeedback);

            state.PrimaryButtonText = ResolvePrimaryActionButtonText(SelectedItem, waitWorkpiece);
            state.SecondaryButtonText = ResolveSecondaryActionButtonText(SelectedItem);

            state.PrimaryButtonEnabled = primaryValidate.Success;
            state.SecondaryButtonEnabled = SelectedItem.HasSecondaryAction && secondaryValidate.Success;

            if (!primaryValidate.Success && SelectedItem.HasSecondaryAction && !secondaryValidate.Success)
            {
                state.HintText = "主操作：" + primaryValidate.Message + "；副操作：" + secondaryValidate.Message;
            }
            else if (!primaryValidate.Success)
            {
                state.HintText = "主操作：" + primaryValidate.Message;
            }
            else if (SelectedItem.HasSecondaryAction && !secondaryValidate.Success)
            {
                state.HintText = "副操作：" + secondaryValidate.Message;
            }
            else
            {
                state.HintText = SelectedItem.DetailText;
            }

            return state;
        }

        /// <summary>
        /// 校验当前主动作是否允许执行。
        /// </summary>
        public Result ValidatePrimaryAction(bool waitFeedback, bool waitWorkpiece)
        {
            if (SelectedItem == null)
                return Result.Fail(-2301, "请先选择执行器对象", ResultSource.Motion);

            switch (SelectedItem.ActuatorType)
            {
                case "Cylinder":
                    if (SelectedItem.PrimaryState == true && SelectedItem.SecondaryState != true)
                        return Result.Fail(-2302, "当前气缸已在伸出到位状态，无需重复伸出", ResultSource.Motion);

                    if (waitFeedback)
                    {
                        if (!SelectedItem.UseFeedbackCheck)
                            return Result.Fail(-2303, "当前气缸未启用反馈校验，请取消“等待反馈”或调整配置", ResultSource.Motion);

                        if (!SelectedItem.PrimaryFeedbackBit.HasValue)
                            return Result.Fail(-2304, "当前气缸未配置伸出反馈位，无法等待伸出到位", ResultSource.Motion);
                    }

                    return Result.Ok("允许执行", ResultSource.Motion);

                case "Vacuum":
                    if (SelectedItem.PrimaryState == true && (!waitWorkpiece || SelectedItem.WorkpieceState != false))
                        return Result.Fail(-2305, "当前真空已建立，无需重复吸真空", ResultSource.Motion);

                    if (waitFeedback)
                    {
                        if (!SelectedItem.UseFeedbackCheck)
                            return Result.Fail(-2306, "当前真空未启用反馈校验，请取消“等待反馈”或调整配置", ResultSource.Motion);

                        if (!SelectedItem.PrimaryFeedbackBit.HasValue)
                            return Result.Fail(-2307, "当前真空未配置建压反馈位，无法等待真空建立", ResultSource.Motion);
                    }

                    if (waitWorkpiece)
                    {
                        if (!SelectedItem.UseWorkpieceCheck)
                            return Result.Fail(-2308, "当前真空未启用工件检测校验，请取消“等待工件检测”或调整配置", ResultSource.Motion);

                        if (!SelectedItem.WorkpieceBit.HasValue)
                            return Result.Fail(-2309, "当前真空未配置工件检测位，无法等待工件检测", ResultSource.Motion);
                    }

                    return Result.Ok("允许执行", ResultSource.Motion);

                case "Gripper":
                    if (SelectedItem.PrimaryState == true && (!waitWorkpiece || SelectedItem.WorkpieceState != false))
                        return Result.Fail(-2310, "当前夹爪已在夹紧到位状态，无需重复夹紧", ResultSource.Motion);

                    if (waitFeedback)
                    {
                        if (!SelectedItem.UseFeedbackCheck)
                            return Result.Fail(-2311, "当前夹爪未启用反馈校验，请取消“等待反馈”或调整配置", ResultSource.Motion);

                        if (!SelectedItem.PrimaryFeedbackBit.HasValue)
                            return Result.Fail(-2312, "当前夹爪未配置夹紧反馈位，无法等待夹紧到位", ResultSource.Motion);
                    }

                    if (waitWorkpiece)
                    {
                        if (!SelectedItem.UseWorkpieceCheck)
                            return Result.Fail(-2313, "当前夹爪未启用工件检测校验，请取消“等待工件检测”或调整配置", ResultSource.Motion);

                        if (!SelectedItem.WorkpieceBit.HasValue)
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
            if (SelectedItem == null)
                return Result.Fail(-2320, "请先选择执行器对象", ResultSource.Motion);

            switch (SelectedItem.ActuatorType)
            {
                case "Cylinder":
                    if (SelectedItem.SecondaryState == true && SelectedItem.PrimaryState != true)
                        return Result.Fail(-2321, "当前气缸已在缩回到位状态，无需重复缩回", ResultSource.Motion);

                    if (waitFeedback)
                    {
                        if (!SelectedItem.UseFeedbackCheck)
                            return Result.Fail(-2322, "当前气缸未启用反馈校验，请取消“等待反馈”或调整配置", ResultSource.Motion);

                        if (!SelectedItem.SecondaryFeedbackBit.HasValue)
                            return Result.Fail(-2323, "当前气缸未配置缩回反馈位，无法等待缩回到位", ResultSource.Motion);
                    }

                    return Result.Ok("允许执行", ResultSource.Motion);

                case "Vacuum":
                    if (SelectedItem.SecondaryState == true || SelectedItem.PrimaryState == false)
                        return Result.Fail(-2324, "当前真空已处于释放状态，无需重复关闭真空", ResultSource.Motion);

                    if (waitFeedback)
                    {
                        if (!SelectedItem.UseFeedbackCheck)
                            return Result.Fail(-2325, "当前真空未启用反馈校验，请取消“等待反馈”或调整配置", ResultSource.Motion);

                        if (!SelectedItem.SecondaryFeedbackBit.HasValue && !SelectedItem.PrimaryFeedbackBit.HasValue)
                            return Result.Fail(-2326, "当前真空未配置释放反馈位或建压反馈位，无法等待真空释放", ResultSource.Motion);
                    }

                    return Result.Ok("允许执行", ResultSource.Motion);

                case "Gripper":
                    if (SelectedItem.SecondaryState == true && SelectedItem.PrimaryState != true)
                        return Result.Fail(-2327, "当前夹爪已在打开到位状态，无需重复打开", ResultSource.Motion);

                    if (waitFeedback)
                    {
                        if (!SelectedItem.UseFeedbackCheck)
                            return Result.Fail(-2328, "当前夹爪未启用反馈校验，请取消“等待反馈”或调整配置", ResultSource.Motion);

                        if (!SelectedItem.SecondaryFeedbackBit.HasValue)
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
            if (SelectedItem == null || !string.Equals(SelectedItem.ActuatorType, "StackLight", StringComparison.OrdinalIgnoreCase))
                return Result.Fail(-2335, "请先选择灯塔对象", ResultSource.Motion);

            if (!SelectedItem.HasAnyStackLightOutput)
                return Result.Fail(-2336, "当前灯塔未配置任何输出位，无法执行状态切换", ResultSource.Motion);

            StackLightState targetState;
            if (!TryParseStackLightState(stateKey, out targetState))
                return Result.Fail(-2337, "不支持的灯塔状态: " + stateKey, ResultSource.Motion);

            if (IsStackLightAlreadyInTargetState(SelectedItem, targetState, stackLightWithBuzzer))
                return Result.Fail(-2338, "当前灯塔已处于目标状态，无需重复切换", ResultSource.Motion);

            return Result.Ok("允许执行", ResultSource.Motion);
        }

        /// <summary>
        /// 执行当前主动作。
        /// 实际执行仍统一走第三层执行器服务。
        /// </summary>
        public async Task<Result> ExecutePrimaryActionAsync(bool waitFeedback, bool waitWorkpiece)
        {
            var item = SelectedItem;
            if (item == null)
                return Result.Fail(-2340, "请先选择执行器对象", ResultSource.Motion);

            var validate = ValidatePrimaryAction(waitFeedback, waitWorkpiece);
            if (!validate.Success)
            {
                ApplyActionResult(item, validate);
                return validate;
            }

            Result result;
            switch (item.ActuatorType)
            {
                case "Cylinder":
                    result = await Task.Run(() => _cylinderService.Extend(item.Name, waitFeedback));
                    break;

                case "Vacuum":
                    result = await Task.Run(() => _vacuumService.VacuumOn(item.Name, waitFeedback, waitWorkpiece));
                    break;

                case "Gripper":
                    result = await Task.Run(() => _gripperService.Close(item.Name, waitFeedback, waitWorkpiece));
                    break;

                default:
                    result = Result.Fail(-2341, "当前对象不支持主动作", ResultSource.Motion);
                    break;
            }

            ApplyActionResult(item, result);
            return result;
        }

        /// <summary>
        /// 执行当前副动作。
        /// </summary>
        public async Task<Result> ExecuteSecondaryActionAsync(bool waitFeedback)
        {
            var item = SelectedItem;
            if (item == null)
                return Result.Fail(-2342, "请先选择执行器对象", ResultSource.Motion);

            var validate = ValidateSecondaryAction(waitFeedback);
            if (!validate.Success)
            {
                ApplyActionResult(item, validate);
                return validate;
            }

            Result result;
            switch (item.ActuatorType)
            {
                case "Cylinder":
                    result = await Task.Run(() => _cylinderService.Retract(item.Name, waitFeedback));
                    break;

                case "Vacuum":
                    result = await Task.Run(() => _vacuumService.VacuumOff(item.Name, waitFeedback));
                    break;

                case "Gripper":
                    result = await Task.Run(() => _gripperService.Open(item.Name, waitFeedback));
                    break;

                default:
                    result = Result.Fail(-2343, "当前对象不支持副动作", ResultSource.Motion);
                    break;
            }

            ApplyActionResult(item, result);
            return result;
        }

        /// <summary>
        /// 切换灯塔状态。
        /// </summary>
        public async Task<Result> SetStackLightStateAsync(string stateKey, bool stackLightWithBuzzer)
        {
            var item = SelectedItem;
            if (item == null)
                return Result.Fail(-2344, "请先选择灯塔对象", ResultSource.Motion);

            var validate = ValidateStackLightState(stateKey, stackLightWithBuzzer);
            if (!validate.Success)
            {
                ApplyActionResult(item, validate);
                return validate;
            }

            Result result;
            switch (stateKey)
            {
                case "Off":
                    result = await Task.Run(() => _stackLightService.TurnOff(item.Name));
                    break;

                case "Idle":
                    result = await Task.Run(() => _stackLightService.SetIdle(item.Name));
                    break;

                case "Running":
                    result = await Task.Run(() => _stackLightService.SetRunning(item.Name));
                    break;

                case "Warning":
                    result = await Task.Run(() => _stackLightService.SetWarning(item.Name, stackLightWithBuzzer));
                    break;

                case "Alarm":
                    result = await Task.Run(() => _stackLightService.SetAlarm(item.Name, stackLightWithBuzzer));
                    break;

                default:
                    result = Result.Fail(-2345, "不支持的灯塔状态", ResultSource.Motion);
                    break;
            }

            ApplyActionResult(item, result);
            return result;
        }

        /// <summary>
        /// 从 MachineContext 构建执行器静态对象列表。
        /// 这一层只负责结构，不负责运行态。
        /// </summary>
        private List<MotionActuatorViewItem> BuildAllItemsFromMachineContext()
        {
            var machine = MachineContext.Instance;
            var list = new List<MotionActuatorViewItem>();

            if (machine.Cylinders != null)
            {
                foreach (var item in machine.Cylinders.Values
                    .Where(x => x != null && x.IsEnabled)
                    .OrderBy(x => x.SortOrder)
                    .ThenBy(x => x.Name))
                {
                    list.Add(new MotionActuatorViewItem
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
                    list.Add(new MotionActuatorViewItem
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
                    list.Add(new MotionActuatorViewItem
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
                    list.Add(new MotionActuatorViewItem
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
        /// </summary>
        private void RefreshRuntimeStateCore(IEnumerable<MotionActuatorViewItem> items)
        {
            foreach (var item in items)
            {
                if (item == null)
                    continue;

                switch (item.ActuatorType)
                {
                    case "Cylinder":
                        RefreshCylinderState(item);
                        break;

                    case "Vacuum":
                        RefreshVacuumState(item);
                        break;

                    case "Gripper":
                        RefreshGripperState(item);
                        break;

                    case "StackLight":
                        RefreshStackLightState(item);
                        break;
                }
            }
        }

        private void RefreshCylinderState(MotionActuatorViewItem item)
        {
            var isExtended = TryReadBoolResult(() => _cylinderService.IsExtended(item.Name));
            var isRetracted = TryReadBoolResult(() => _cylinderService.IsRetracted(item.Name));

            item.PrimaryState = isExtended;
            item.SecondaryState = isRetracted;
            item.WorkpieceState = null;

            if (!item.PrimaryFeedbackBit.HasValue && !item.SecondaryFeedbackBit.HasValue)
            {
                item.StateText = "未配反馈";
                item.StateLevel = "Warning";
                item.DetailText = "未配置伸出/缩回反馈位";
                item.FooterText = "反馈：— / —";
                item.HasFault = false;
            }
            else if (isExtended == true && isRetracted == true)
            {
                item.StateText = "反馈冲突";
                item.StateLevel = "Danger";
                item.DetailText = "伸出与缩回反馈同时为到位";
                item.FooterText = "伸出=Y / 缩回=Y";
                item.HasFault = true;
            }
            else if (isExtended == true)
            {
                item.StateText = "伸出到位";
                item.StateLevel = "Success";
                item.DetailText = "气缸当前处于伸出端";
                item.FooterText = "伸出=Y / 缩回=" + BoolToShortText(isRetracted);
                item.HasFault = false;
            }
            else if (isRetracted == true)
            {
                item.StateText = "缩回到位";
                item.StateLevel = "Primary";
                item.DetailText = "气缸当前处于缩回端";
                item.FooterText = "伸出=" + BoolToShortText(isExtended) + " / 缩回=Y";
                item.HasFault = false;
            }
            else if (isExtended == false && isRetracted == false)
            {
                item.StateText = "双未到位";
                item.StateLevel = "Warning";
                item.DetailText = "伸出与缩回反馈均未到位";
                item.FooterText = "伸出=N / 缩回=N";
                item.HasFault = false;
            }
            else
            {
                item.StateText = "状态未知";
                item.StateLevel = "Secondary";
                item.DetailText = "当前反馈不足以判断气缸端位";
                item.FooterText = "伸出=" + BoolToShortText(isExtended) + " / 缩回=" + BoolToShortText(isRetracted);
                item.HasFault = false;
            }

            item.RuntimeUpdateTimeText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void RefreshVacuumState(MotionActuatorViewItem item)
        {
            var isBuilt = TryReadBoolResult(() => _vacuumService.IsVacuumBuilt(item.Name));
            var isReleased = TryReadBoolResult(() => _vacuumService.IsReleased(item.Name));
            var hasWorkpiece = TryReadBoolResult(() => _vacuumService.HasWorkpiece(item.Name));

            item.PrimaryState = isBuilt;
            item.SecondaryState = isReleased;
            item.WorkpieceState = hasWorkpiece;

            if (!item.PrimaryFeedbackBit.HasValue && !item.SecondaryFeedbackBit.HasValue)
            {
                item.StateText = "未配反馈";
                item.StateLevel = "Warning";
                item.DetailText = "未配置建压/释放反馈位";
                item.FooterText = "工件=" + BoolToShortText(hasWorkpiece);
                item.HasFault = false;
            }
            else if (isBuilt == true && isReleased == true)
            {
                item.StateText = "反馈冲突";
                item.StateLevel = "Danger";
                item.DetailText = "建压与释放反馈同时成立";
                item.FooterText = "建压=Y / 释放=Y";
                item.HasFault = true;
            }
            else if (isBuilt == true && hasWorkpiece == true)
            {
                item.StateText = "已吸附";
                item.StateLevel = "Success";
                item.DetailText = "真空已建立，且已检测到工件";
                item.FooterText = "建压=Y / 工件=Y";
                item.HasFault = false;
            }
            else if (isBuilt == true)
            {
                item.StateText = "真空建立";
                item.StateLevel = "Success";
                item.DetailText = "真空已建立";
                item.FooterText = "建压=Y / 工件=" + BoolToShortText(hasWorkpiece);
                item.HasFault = false;
            }
            else if (isReleased == true)
            {
                item.StateText = "已释放";
                item.StateLevel = "Primary";
                item.DetailText = "真空当前处于释放状态";
                item.FooterText = "释放=Y / 工件=" + BoolToShortText(hasWorkpiece);
                item.HasFault = false;
            }
            else if (isBuilt == false && isReleased == false)
            {
                item.StateText = "未建压";
                item.StateLevel = "Secondary";
                item.DetailText = "当前既未检测到建压，也未检测到释放到位";
                item.FooterText = "建压=N / 释放=N";
                item.HasFault = false;
            }
            else
            {
                item.StateText = "状态未知";
                item.StateLevel = "Secondary";
                item.DetailText = "当前反馈不足以判断真空状态";
                item.FooterText = "工件=" + BoolToShortText(hasWorkpiece);
                item.HasFault = false;
            }

            item.RuntimeUpdateTimeText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void RefreshGripperState(MotionActuatorViewItem item)
        {
            var isClosed = TryReadBoolResult(() => _gripperService.IsClosed(item.Name));
            var isOpened = TryReadBoolResult(() => _gripperService.IsOpened(item.Name));
            var hasWorkpiece = TryReadBoolResult(() => _gripperService.HasWorkpiece(item.Name));

            item.PrimaryState = isClosed;
            item.SecondaryState = isOpened;
            item.WorkpieceState = hasWorkpiece;

            if (!item.PrimaryFeedbackBit.HasValue && !item.SecondaryFeedbackBit.HasValue)
            {
                item.StateText = "未配反馈";
                item.StateLevel = "Warning";
                item.DetailText = "未配置夹紧/打开反馈位";
                item.FooterText = "工件=" + BoolToShortText(hasWorkpiece);
                item.HasFault = false;
            }
            else if (isClosed == true && isOpened == true)
            {
                item.StateText = "反馈冲突";
                item.StateLevel = "Danger";
                item.DetailText = "夹紧与打开反馈同时成立";
                item.FooterText = "夹紧=Y / 打开=Y";
                item.HasFault = true;
            }
            else if (isClosed == true && hasWorkpiece == true)
            {
                item.StateText = "夹紧有料";
                item.StateLevel = "Success";
                item.DetailText = "夹爪夹紧到位，且已检测到工件";
                item.FooterText = "夹紧=Y / 工件=Y";
                item.HasFault = false;
            }
            else if (isClosed == true)
            {
                item.StateText = "夹紧到位";
                item.StateLevel = "Success";
                item.DetailText = "夹爪已夹紧到位";
                item.FooterText = "夹紧=Y / 工件=" + BoolToShortText(hasWorkpiece);
                item.HasFault = false;
            }
            else if (isOpened == true)
            {
                item.StateText = "打开到位";
                item.StateLevel = "Primary";
                item.DetailText = "夹爪已打开到位";
                item.FooterText = "打开=Y / 工件=" + BoolToShortText(hasWorkpiece);
                item.HasFault = false;
            }
            else if (isClosed == false && isOpened == false)
            {
                item.StateText = "双未到位";
                item.StateLevel = "Secondary";
                item.DetailText = "夹紧与打开反馈均未到位";
                item.FooterText = "夹紧=N / 打开=N";
                item.HasFault = false;
            }
            else
            {
                item.StateText = "状态未知";
                item.StateLevel = "Secondary";
                item.DetailText = "当前反馈不足以判断夹爪状态";
                item.FooterText = "工件=" + BoolToShortText(hasWorkpiece);
                item.HasFault = false;
            }

            item.RuntimeUpdateTimeText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void RefreshStackLightState(MotionActuatorViewItem item)
        {
            item.RedOn = ReadDoState(item.RedOutputBit);
            item.YellowOn = ReadDoState(item.YellowOutputBit);
            item.GreenOn = ReadDoState(item.GreenOutputBit);
            item.BlueOn = ReadDoState(item.BlueOutputBit);
            item.BuzzerOn = ReadDoState(item.BuzzerOutputBit);

            var onSegments = new List<string>();
            if (item.RedOn == true) onSegments.Add("红");
            if (item.YellowOn == true) onSegments.Add("黄");
            if (item.GreenOn == true) onSegments.Add("绿");
            if (item.BlueOn == true) onSegments.Add("蓝");

            if (!item.HasAnyStackLightOutput)
            {
                item.StateText = "未配输出";
                item.StateLevel = "Warning";
                item.DetailText = "未配置任何灯塔输出位";
                item.FooterText = "亮段：—";
                item.HasFault = false;
            }
            else if (onSegments.Count == 0 && item.BuzzerOn != true)
            {
                item.StateText = "全灭";
                item.StateLevel = "Secondary";
                item.DetailText = "当前所有灯段均关闭";
                item.FooterText = "亮段：无 / 蜂鸣=N";
                item.HasFault = false;
            }
            else if (onSegments.Count == 1)
            {
                item.StateText = onSegments[0] + (item.BuzzerOn == true ? "+蜂鸣" : "灯");
                item.StateLevel = item.RedOn == true
                    ? "Danger"
                    : (item.YellowOn == true ? "Warning" : "Success");
                item.DetailText = "红=" + BoolToShortText(item.RedOn)
                    + " / 黄=" + BoolToShortText(item.YellowOn)
                    + " / 绿=" + BoolToShortText(item.GreenOn)
                    + " / 蓝=" + BoolToShortText(item.BlueOn)
                    + " / 鸣=" + BoolToShortText(item.BuzzerOn);
                item.FooterText = "亮段：" + onSegments[0] + " / 蜂鸣=" + BoolToShortText(item.BuzzerOn);
                item.HasFault = false;
            }
            else
            {
                item.StateText = item.BuzzerOn == true ? "多段+蜂鸣" : "多段点亮";
                item.StateLevel = item.RedOn == true
                    ? "Danger"
                    : (item.YellowOn == true ? "Warning" : "Primary");
                item.DetailText = "红=" + BoolToShortText(item.RedOn)
                    + " / 黄=" + BoolToShortText(item.YellowOn)
                    + " / 绿=" + BoolToShortText(item.GreenOn)
                    + " / 蓝=" + BoolToShortText(item.BlueOn)
                    + " / 鸣=" + BoolToShortText(item.BuzzerOn);
                item.FooterText = "亮段：" + string.Join("/", onSegments) + " / 蜂鸣=" + BoolToShortText(item.BuzzerOn);
                item.HasFault = false;
            }

            item.RuntimeUpdateTimeText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 应用筛选并尽量恢复选中项。
        /// </summary>
        private void ApplyFilterAndSelection()
        {
            var previousSelectedKey = SelectedItem == null ? null : SelectedItem.ItemKey;
            ApplyFilterAndSelection(previousSelectedKey);
        }

        private void ApplyFilterAndSelection(string previousSelectedKey)
        {
            IEnumerable<MotionActuatorViewItem> query = _allItems;

            if (!string.Equals(_typeFilter, "All", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(x => string.Equals(x.ActuatorType, _typeFilter, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(_searchText))
            {
                var keyword = _searchText.Trim().ToLowerInvariant();
                query = query.Where(x =>
                    (x.Name ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                    (x.DisplayName ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                    (x.TypeDisplay ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                    (x.StateText ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                    (x.CardLine1Text ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                    (x.CardLine2Text ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                    (x.LastActionMessage ?? string.Empty).ToLowerInvariant().Contains(keyword));
            }

            _filteredItems = query
                .OrderBy(x => GetActuatorTypeSort(x.ActuatorType))
                .ThenBy(x => x.SortOrder)
                .ThenBy(x => x.Name)
                .ToList();

            UpdateSummary(_filteredItems);

            MotionActuatorViewItem selected = null;
            if (!string.IsNullOrWhiteSpace(previousSelectedKey))
            {
                selected = _filteredItems.FirstOrDefault(x =>
                    string.Equals(x.ItemKey, previousSelectedKey, StringComparison.OrdinalIgnoreCase));
            }

            if (selected == null && _filteredItems.Count > 0)
                selected = _filteredItems[0];

            SelectedItem = selected;
            RaiseUiChanged();
        }

        private void UpdateSummary(IList<MotionActuatorViewItem> list)
        {
            var source = list ?? new List<MotionActuatorViewItem>();

            TotalCount = source.Count;
            CylinderCount = source.Count(x => x.ActuatorType == "Cylinder");
            VacuumCount = source.Count(x => x.ActuatorType == "Vacuum");
            GripperCount = source.Count(x => x.ActuatorType == "Gripper");
            StackLightCount = source.Count(x => x.ActuatorType == "StackLight");
        }

        private void RaiseUiChanged()
        {
            OnPropertyChanged(nameof(PageItems));
            OnPropertyChanged(nameof(SelectedItem));
        }

        private static string ResolvePrimaryActionButtonText(MotionActuatorViewItem item, bool waitWorkpiece)
        {
            if (item == null)
                return "主操作";

            switch (item.ActuatorType)
            {
                case "Cylinder":
                    return item.PrimaryState == true && item.SecondaryState != true
                        ? "伸出（已到位）"
                        : "伸出";

                case "Vacuum":
                    if (item.PrimaryState == true && (!waitWorkpiece || item.WorkpieceState != false))
                        return "吸真空（已建立）";

                    return waitWorkpiece ? "吸真空+检测" : "吸真空";

                case "Gripper":
                    if (item.PrimaryState == true && (!waitWorkpiece || item.WorkpieceState != false))
                        return "夹紧（已到位）";

                    return waitWorkpiece ? "夹紧+检测" : "夹紧";

                default:
                    return "主操作";
            }
        }

        private static string ResolveSecondaryActionButtonText(MotionActuatorViewItem item)
        {
            if (item == null)
                return "副操作";

            switch (item.ActuatorType)
            {
                case "Cylinder":
                    return item.SecondaryState == true && item.PrimaryState != true
                        ? "缩回（已到位）"
                        : "缩回";

                case "Vacuum":
                    return item.SecondaryState == true || item.PrimaryState == false
                        ? "关闭真空（已释放）"
                        : "关闭真空";

                case "Gripper":
                    return item.SecondaryState == true && item.PrimaryState != true
                        ? "打开（已到位）"
                        : "打开";

                default:
                    return "副操作";
            }
        }

        private bool IsStackLightCurrentState(string stateKey, bool stackLightWithBuzzer)
        {
            if (SelectedItem == null || !string.Equals(SelectedItem.ActuatorType, "StackLight", StringComparison.OrdinalIgnoreCase))
                return false;

            StackLightState state;
            if (!TryParseStackLightState(stateKey, out state))
                return false;

            return IsStackLightAlreadyInTargetState(SelectedItem, state, stackLightWithBuzzer);
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
            MotionActuatorViewItem item,
            StackLightState targetState,
            bool withBuzzer)
        {
            if (item == null)
                return false;

            switch (targetState)
            {
                case StackLightState.Off:
                    return item.RedOn != true
                        && item.YellowOn != true
                        && item.GreenOn != true
                        && item.BlueOn != true
                        && item.BuzzerOn != true;

                case StackLightState.Idle:
                case StackLightState.Running:
                    return item.GreenOn == true
                        && item.RedOn != true
                        && item.YellowOn != true
                        && item.BlueOn != true
                        && item.BuzzerOn != true;

                case StackLightState.Warning:
                    return item.YellowOn == true
                        && item.RedOn != true
                        && item.GreenOn != true
                        && item.BlueOn != true
                        && (withBuzzer ? item.BuzzerOn == true : true);

                case StackLightState.Alarm:
                    return item.RedOn == true
                        && item.YellowOn != true
                        && item.GreenOn != true
                        && item.BlueOn != true
                        && (withBuzzer ? item.BuzzerOn == true : true);

                default:
                    return false;
            }
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

        private void ApplyActionResult(MotionActuatorViewItem item, Result result)
        {
            if (item == null || result == null)
                return;

            item.LastActionMessage = BuildLayeredActionMessage(result);
            item.LastActionLevel = result.Success ? "Success" : "Danger";
            item.HasFault = !result.Success;
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
        /// 执行器卡片显示项。
        /// 保持字段命名尽量接近 WPF 版本，方便后续迁移动作逻辑。
        /// </summary>
        public sealed class MotionActuatorViewItem
        {
            public string ActuatorType { get; set; }
            public string TypeDisplay { get; set; }
            public string Name { get; set; }
            public string DisplayName { get; set; }
            public bool IsEnabled { get; set; }
            public int SortOrder { get; set; }
            public string Description { get; set; }
            public string Remark { get; set; }
            public string ControlModeText { get; set; }

            public short? PrimaryOutputBit { get; set; }
            public short? SecondaryOutputBit { get; set; }
            public short? PrimaryFeedbackBit { get; set; }
            public short? SecondaryFeedbackBit { get; set; }
            public short? WorkpieceBit { get; set; }

            public short? RedOutputBit { get; set; }
            public short? YellowOutputBit { get; set; }
            public short? GreenOutputBit { get; set; }
            public short? BlueOutputBit { get; set; }
            public short? BuzzerOutputBit { get; set; }

            public bool? PrimaryState { get; set; }
            public bool? SecondaryState { get; set; }
            public bool? WorkpieceState { get; set; }

            public bool? RedOn { get; set; }
            public bool? YellowOn { get; set; }
            public bool? GreenOn { get; set; }
            public bool? BlueOn { get; set; }
            public bool? BuzzerOn { get; set; }

            public string PrimaryOutputText { get; set; }
            public string SecondaryOutputText { get; set; }
            public string PrimaryFeedbackText { get; set; }
            public string SecondaryFeedbackText { get; set; }
            public string WorkpieceText { get; set; }
            public string TimeoutText { get; set; }

            public string CardLine1Text { get; set; }
            public string CardLine2Text { get; set; }

            public string PrimaryActionText { get; set; }
            public string SecondaryActionText { get; set; }
            public bool HasSecondaryAction { get; set; }
            public bool UseFeedbackCheck { get; set; }
            public bool UseWorkpieceCheck { get; set; }

            public string StateText { get; set; }
            public string StateLevel { get; set; }
            public string DetailText { get; set; }
            public string FooterText { get; set; }
            public string RuntimeUpdateTimeText { get; set; }
            public string LastActionMessage { get; set; }
            public string LastActionLevel { get; set; }
            public bool HasFault { get; set; }

            public bool HasAnyStackLightOutput
            {
                get
                {
                    return RedOutputBit.HasValue
                        || YellowOutputBit.HasValue
                        || GreenOutputBit.HasValue
                        || BlueOutputBit.HasValue
                        || BuzzerOutputBit.HasValue;
                }
            }

            public string DisplayTitle
            {
                get
                {
                    if (!string.IsNullOrWhiteSpace(DisplayName))
                        return DisplayName;

                    return string.IsNullOrWhiteSpace(Name) ? "未命名对象" : Name;
                }
            }

            public string ItemKey
            {
                get { return (ActuatorType ?? string.Empty) + "|" + (Name ?? string.Empty); }
            }
        }

        /// <summary>
        /// 右侧控制面板状态。
        /// 该对象不依赖具体 UI 控件类型，只表达控件当前应展示什么。
        /// </summary>
        public sealed class MotionActuatorActionPanelState
        {
            public string TitleText { get; set; }
            public string SubTitleText { get; set; }
            public string HintText { get; set; }

            public bool ShowNormalActions { get; set; }
            public bool ShowStackLightActions { get; set; }

            public bool ShowWaitFeedback { get; set; }
            public bool ShowWaitWorkpiece { get; set; }
            public bool ShowStackLightWithBuzzer { get; set; }

            public bool WaitFeedback { get; set; }
            public bool WaitWorkpiece { get; set; }
            public bool StackLightWithBuzzer { get; set; }

            public string PrimaryButtonText { get; set; }
            public bool PrimaryButtonEnabled { get; set; }

            public string SecondaryButtonText { get; set; }
            public bool SecondaryButtonEnabled { get; set; }

            public string OffButtonText { get; set; }
            public string IdleButtonText { get; set; }
            public string RunningButtonText { get; set; }
            public string WarningButtonText { get; set; }
            public string AlarmButtonText { get; set; }

            public bool OffButtonEnabled { get; set; }
            public bool IdleButtonEnabled { get; set; }
            public bool RunningButtonEnabled { get; set; }
            public bool WarningButtonEnabled { get; set; }
            public bool AlarmButtonEnabled { get; set; }

            public bool IsOffCurrent { get; set; }
            public bool IsIdleCurrent { get; set; }
            public bool IsRunningCurrent { get; set; }
            public bool IsWarningCurrent { get; set; }
            public bool IsAlarmCurrent { get; set; }
        }
    }
}