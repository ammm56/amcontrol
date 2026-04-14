using AM.DBService.Services.Motion.Assembly;
using AM.Model.Common;
using AM.Model.Interfaces.Motion.Assembly;
using AM.PageModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AM.PageModel.Assembly
{
    /// <summary>
    /// 装配接线与调试工作台页面模型。
    /// 当前阶段负责查询结果整理、筛选、统计与调试动作转发。
    /// </summary>
    public class AssemblyWiringPageModel : BindableBase
    {
        private readonly AssemblyWiringQueryService _queryService;
        private readonly AssemblyWiringDebugService _debugService;

        private List<AssemblyWiringRowViewItem> _allItems;
        private List<AssemblyWiringRowViewItem> _items;
        private AssemblyWiringRowViewItem _selectedItem;
        private string _searchText;
        private short? _selectedCardId;
        private string _selectedIoType;
        private bool _onlyUnverified;
        private bool _onlyIssues;
        private bool _debugModeEnabled;
        private int _totalCount;
        private int _verifiedCount;
        private int _unverifiedCount;
        private int _runtimeOkCount;
        private int _issueCount;
        private string _summaryText;

        public AssemblyWiringPageModel()
        {
            _queryService = new AssemblyWiringQueryService();
            _debugService = new AssemblyWiringDebugService();
            _allItems = new List<AssemblyWiringRowViewItem>();
            _items = new List<AssemblyWiringRowViewItem>();
            _searchText = string.Empty;
            _selectedIoType = "All";
            _summaryText = string.Empty;
        }

        /// <summary>
        /// 当前筛选后的行集合。
        /// </summary>
        public IList<AssemblyWiringRowViewItem> Items
        {
            get { return _items; }
        }

        /// <summary>
        /// 当前选中行。
        /// </summary>
        public AssemblyWiringRowViewItem SelectedItem
        {
            get { return _selectedItem; }
            private set { SetProperty(ref _selectedItem, value); }
        }

        /// <summary>
        /// 搜索关键字。
        /// </summary>
        public string SearchText
        {
            get { return _searchText; }
            private set { SetProperty(ref _searchText, value ?? string.Empty); }
        }

        /// <summary>
        /// 当前控制卡筛选。
        /// </summary>
        public short? SelectedCardId
        {
            get { return _selectedCardId; }
            private set { SetProperty(ref _selectedCardId, value); }
        }

        /// <summary>
        /// 当前控制卡筛选文本。
        /// </summary>
        public string SelectedCardText
        {
            get
            {
                if (!SelectedCardId.HasValue)
                {
                    return "当前：全部控制卡";
                }

                var selected = _allItems.FirstOrDefault(p => p.CardId == SelectedCardId.Value);
                if (selected == null)
                {
                    return "当前：控制卡 #" + SelectedCardId.Value;
                }

                return "当前：" + selected.CardDisplayName + " (#" + SelectedCardId.Value + ")";
            }
        }

        /// <summary>
        /// 当前 IO 类型筛选。
        /// </summary>
        public string SelectedIoType
        {
            get { return _selectedIoType; }
            private set { SetProperty(ref _selectedIoType, value ?? "All"); }
        }

        /// <summary>
        /// 是否仅显示未核对项。
        /// </summary>
        public bool OnlyUnverified
        {
            get { return _onlyUnverified; }
            private set { SetProperty(ref _onlyUnverified, value); }
        }

        /// <summary>
        /// 是否仅显示异常项。
        /// </summary>
        public bool OnlyIssues
        {
            get { return _onlyIssues; }
            private set { SetProperty(ref _onlyIssues, value); }
        }

        /// <summary>
        /// 是否开启调试模式。
        /// </summary>
        public bool DebugModeEnabled
        {
            get { return _debugModeEnabled; }
            private set { SetProperty(ref _debugModeEnabled, value); }
        }

        /// <summary>
        /// 当前总数。
        /// </summary>
        public int TotalCount
        {
            get { return _totalCount; }
            private set { SetProperty(ref _totalCount, value); }
        }

        /// <summary>
        /// 已核对数量。
        /// </summary>
        public int VerifiedCount
        {
            get { return _verifiedCount; }
            private set { SetProperty(ref _verifiedCount, value); }
        }

        /// <summary>
        /// 未核对数量。
        /// </summary>
        public int UnverifiedCount
        {
            get { return _unverifiedCount; }
            private set { SetProperty(ref _unverifiedCount, value); }
        }

        /// <summary>
        /// 运行正常数量。
        /// </summary>
        public int RuntimeOkCount
        {
            get { return _runtimeOkCount; }
            private set { SetProperty(ref _runtimeOkCount, value); }
        }

        /// <summary>
        /// 异常数量。
        /// </summary>
        public int IssueCount
        {
            get { return _issueCount; }
            private set { SetProperty(ref _issueCount, value); }
        }

        /// <summary>
        /// 摘要文本。
        /// </summary>
        public string SummaryText
        {
            get { return _summaryText; }
            private set { SetProperty(ref _summaryText, value ?? string.Empty); }
        }

        /// <summary>
        /// 首次加载数据。
        /// </summary>
        public async Task<Result> LoadAsync()
        {
            return await ReloadAsync();
        }

        /// <summary>
        /// 刷新当前数据。
        /// </summary>
        public async Task<Result> RefreshAsync()
        {
            return await ReloadAsync();
        }

        /// <summary>
        /// 设置搜索关键字。
        /// </summary>
        public void SetSearchText(string text)
        {
            SearchText = text;
            ApplyFilter();
        }

        /// <summary>
        /// 设置控制卡筛选。
        /// </summary>
        public void SetCardFilter(short? cardId)
        {
            SelectedCardId = cardId;
            ApplyFilter();
            OnPropertyChanged(nameof(SelectedCardText));
        }

        /// <summary>
        /// 设置 IO 类型筛选。
        /// </summary>
        public void SetIoTypeFilter(string ioType)
        {
            SelectedIoType = NormalizeIoTypeFilter(ioType);
            ApplyFilter();
        }

        /// <summary>
        /// 设置仅显示未核对项。
        /// </summary>
        public void SetOnlyUnverified(bool value)
        {
            OnlyUnverified = value;
            ApplyFilter();
        }

        /// <summary>
        /// 设置仅显示异常项。
        /// </summary>
        public void SetOnlyIssues(bool value)
        {
            OnlyIssues = value;
            ApplyFilter();
        }

        /// <summary>
        /// 设置调试模式。
        /// </summary>
        public void SetDebugModeEnabled(bool value)
        {
            DebugModeEnabled = value;
        }

        /// <summary>
        /// 选中指定行。
        /// </summary>
        public void SelectRow(int ioMapId)
        {
            SelectedItem = _items.FirstOrDefault(p => p.IoMapId == ioMapId);
        }

        /// <summary>
        /// 读取当前选中 DI。
        /// </summary>
        public Result<bool> ReadSelectedDi()
        {
            if (SelectedItem == null)
            {
                return Result<bool>.Fail(-1, "当前未选择任何行", ResultSource.UI);
            }

            return _debugService.ReadDi(SelectedItem.LogicalBit);
        }

        /// <summary>
        /// 设置当前选中 DO。
        /// </summary>
        public Result SetSelectedDo(bool value)
        {
            if (SelectedItem == null)
            {
                return Result.Fail(-1, "当前未选择任何行", ResultSource.UI);
            }

            return _debugService.SetDo(SelectedItem.LogicalBit, value);
        }

        /// <summary>
        /// 对当前选中 DO 执行脉冲输出。
        /// </summary>
        public Result PulseSelectedDo(int pulseMs)
        {
            if (SelectedItem == null)
            {
                return Result.Fail(-1, "当前未选择任何行", ResultSource.UI);
            }

            return _debugService.PulseDo(SelectedItem.LogicalBit, pulseMs);
        }

        /// <summary>
        /// 测试当前选中行关联的执行器。
        /// </summary>
        public Result TestSelectedActuator(string actionName)
        {
            if (SelectedItem == null)
            {
                return Result.Fail(-1, "当前未选择任何行", ResultSource.UI);
            }

            return _debugService.TestActuator(SelectedItem.RelatedActuatorName, actionName);
        }

        private async Task<Result> ReloadAsync()
        {
            return await Task.Run(() =>
            {
                var result = _queryService.QueryAll();
                if (!result.Success)
                {
                    _allItems = new List<AssemblyWiringRowViewItem>();
                    _items = new List<AssemblyWiringRowViewItem>();
                    SelectedItem = null;
                    RefreshStats();
                    OnPropertyChanged(nameof(Items));
                    return Result.Fail(result.Code, result.Message, result.Source);
                }

                _allItems = (result.Items ?? new List<AssemblyWiringRowModel>())
                    .Select(ToViewItem)
                    .ToList();

                ApplyFilter();
                OnPropertyChanged(nameof(SelectedCardText));
                return Result.Ok("装配接线工作台加载成功");
            });
        }

        private void ApplyFilter()
        {
            IEnumerable<AssemblyWiringRowViewItem> query = _allItems;

            if (SelectedCardId.HasValue)
            {
                query = query.Where(p => p.CardId == SelectedCardId.Value);
            }

            if (!string.Equals(SelectedIoType, "All", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(p => string.Equals(p.IoType, SelectedIoType, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                query = query.Where(p => ContainsText(p.SoftwareName, SearchText)
                    || ContainsText(p.DisplayName, SearchText)
                    || ContainsText(p.DeviceName, SearchText)
                    || ContainsText(p.WireNo, SearchText)
                    || ContainsText(p.TerminalBlock, SearchText)
                    || ContainsText(p.TerminalNo, SearchText));
            }

            if (OnlyUnverified)
            {
                query = query.Where(p => !p.IsVerified);
            }

            if (OnlyIssues)
            {
                query = query.Where(p => p.IsIssue);
            }

            _items = query
                .OrderBy(p => p.CardId)
                .ThenBy(p => p.IoType)
                .ThenBy(p => p.LogicalBit)
                .ToList();

            SelectedItem = ResolveSelectedItem();
            RefreshStats();
            OnPropertyChanged(nameof(Items));
        }

        private AssemblyWiringRowViewItem ResolveSelectedItem()
        {
            if (SelectedItem == null)
            {
                return _items.FirstOrDefault();
            }

            var selected = _items.FirstOrDefault(p => p.IoMapId == SelectedItem.IoMapId);
            return selected ?? _items.FirstOrDefault();
        }

        private void RefreshStats()
        {
            TotalCount = _items.Count;
            VerifiedCount = _items.Count(p => p.IsVerified);
            UnverifiedCount = _items.Count(p => !p.IsVerified);
            RuntimeOkCount = _items.Count(p => string.Equals(p.RuntimeStatusText, "已刷新", StringComparison.OrdinalIgnoreCase));
            IssueCount = _items.Count(p => p.IsIssue);
            SummaryText = "共 " + TotalCount + " 项，异常 " + IssueCount + " 项";
        }

        private static bool ContainsText(string source, string searchText)
        {
            return !string.IsNullOrWhiteSpace(source)
                && source.IndexOf(searchText ?? string.Empty, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static string NormalizeIoTypeFilter(string ioType)
        {
            if (string.IsNullOrWhiteSpace(ioType))
            {
                return "All";
            }

            if (string.Equals(ioType, "DI", StringComparison.OrdinalIgnoreCase))
            {
                return "DI";
            }

            if (string.Equals(ioType, "DO", StringComparison.OrdinalIgnoreCase))
            {
                return "DO";
            }

            return "All";
        }

        private static AssemblyWiringRowViewItem ToViewItem(AssemblyWiringRowModel model)
        {
            return new AssemblyWiringRowViewItem
            {
                IoMapId = model.IoMapId,
                IoType = model.IoType ?? string.Empty,
                LogicalBit = model.LogicalBit,
                CardId = model.CardId,
                CardDisplayName = model.CardDisplayName ?? string.Empty,
                SoftwareName = model.SoftwareName ?? string.Empty,
                DisplayName = model.DisplayName ?? string.Empty,
                SignalCategory = model.SignalCategory ?? string.Empty,
                Description = model.Description ?? string.Empty,
                Core = model.Core,
                IsExtModule = model.IsExtModule,
                HardwareBit = model.HardwareBit,
                IsEnabled = model.IsEnabled,
                TerminalBlock = model.TerminalBlock ?? string.Empty,
                TerminalNo = model.TerminalNo ?? string.Empty,
                ConnectorNo = model.ConnectorNo ?? string.Empty,
                PinNo = model.PinNo ?? string.Empty,
                WireNo = model.WireNo ?? string.Empty,
                DeviceName = model.DeviceName ?? string.Empty,
                DeviceTerminal = model.DeviceTerminal ?? string.Empty,
                CabinetArea = model.CabinetArea ?? string.Empty,
                SignalType = model.SignalType ?? string.Empty,
                IsVerified = model.IsVerified,
                VerifiedBy = model.VerifiedBy ?? string.Empty,
                CurrentValueText = model.CurrentValueText ?? string.Empty,
                LastUpdateTimeText = model.LastUpdateTimeText ?? string.Empty,
                RuntimeStatusText = model.RuntimeStatusText ?? string.Empty,
                WiringStatusText = model.WiringStatusText ?? string.Empty,
                RelatedActuatorName = model.RelatedActuatorName ?? string.Empty,
                RelatedActuatorType = model.RelatedActuatorType ?? string.Empty,
                CanManualOperate = model.CanManualOperate
            };
        }

        /// <summary>
        /// 装配接线工作台表格行视图模型。
        /// </summary>
        public sealed class AssemblyWiringRowViewItem
        {
            public int IoMapId { get; set; }

            public string IoType { get; set; }

            public short LogicalBit { get; set; }

            public short CardId { get; set; }

            public string CardDisplayName { get; set; }

            public string SoftwareName { get; set; }

            public string DisplayName { get; set; }

            public string SignalCategory { get; set; }

            public string Description { get; set; }

            public short Core { get; set; }

            public bool IsExtModule { get; set; }

            public short HardwareBit { get; set; }

            public bool IsEnabled { get; set; }

            public string TerminalBlock { get; set; }

            public string TerminalNo { get; set; }

            public string ConnectorNo { get; set; }

            public string PinNo { get; set; }

            public string WireNo { get; set; }

            public string DeviceName { get; set; }

            public string DeviceTerminal { get; set; }

            public string CabinetArea { get; set; }

            public string SignalType { get; set; }

            public bool IsVerified { get; set; }

            public string VerifiedBy { get; set; }

            public string CurrentValueText { get; set; }

            public string LastUpdateTimeText { get; set; }

            public string RuntimeStatusText { get; set; }

            public string WiringStatusText { get; set; }

            public string RelatedActuatorName { get; set; }

            public string RelatedActuatorType { get; set; }

            public bool CanManualOperate { get; set; }

            public bool IsIssue
            {
                get
                {
                    return string.Equals(WiringStatusText, "未定义", StringComparison.OrdinalIgnoreCase)
                        || string.Equals(WiringStatusText, "未核对", StringComparison.OrdinalIgnoreCase)
                        || !string.Equals(RuntimeStatusText, "已刷新", StringComparison.OrdinalIgnoreCase);
                }
            }
        }
    }
}
