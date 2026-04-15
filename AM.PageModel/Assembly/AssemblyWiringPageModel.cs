using AM.Core.Context;
using AM.DBService.Services.Motion.Assembly;
using AM.DBService.Services.Motion.Topology;
using AM.Model.Common;
using AM.Model.Entity.Motion.Topology;
using AM.Model.Interfaces.Motion.Assembly;
using AM.PageModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private readonly MotionIoWiringCrudService _ioWiringCrudService;

        private List<AssemblyWiringRowViewItem> _allItems;
        private List<AssemblyWiringRowViewItem> _items;
        private AssemblyWiringRowViewItem _selectedItem;
        private string _searchText;
        private short? _selectedCardId;
        private string _selectedIoType;
        private bool _onlyUnverified;
        private int _totalCount;
        private int _verifiedCount;
        private int _unverifiedCount;
        private string _summaryText;

        public AssemblyWiringPageModel()
        {
            _queryService = new AssemblyWiringQueryService();
            _debugService = new AssemblyWiringDebugService();
            _ioWiringCrudService = new MotionIoWiringCrudService();
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
                    return "全部控制卡";
                }

                var selected = _allItems.FirstOrDefault(p => p.CardId == SelectedCardId.Value);
                if (selected == null)
                {
                    return "控制卡 #" + SelectedCardId.Value;
                }

                return selected.CardDisplayName + " (#" + SelectedCardId.Value + ")";
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
        /// 当前是否已选择点位。
        /// </summary>
        public bool HasSelection
        {
            get { return SelectedItem != null; }
        }

        /// <summary>
        /// 当前选中项是否为 DI 点。
        /// </summary>
        public bool IsSelectedDi
        {
            get
            {
                return HasSelection
                    && string.Equals(SelectedItem.IoType, "DI", StringComparison.OrdinalIgnoreCase);
            }
        }

        /// <summary>
        /// 当前选中项是否为 DO 点。
        /// </summary>
        public bool IsSelectedDo
        {
            get
            {
                return HasSelection
                    && string.Equals(SelectedItem.IoType, "DO", StringComparison.OrdinalIgnoreCase);
            }
        }

        /// <summary>
        /// 当前是否允许读取 DI。
        /// </summary>
        public bool CanReadSelectedDi
        {
            get
            {
                return IsSelectedDi;
            }
        }

        /// <summary>
        /// 当前是否允许手动控制 DO。
        /// </summary>
        public bool CanSetSelectedDo
        {
            get { return IsSelectedDo && SelectedItem.CanManualOperate; }
        }

        /// <summary>
        /// 当前是否允许标记已核对。
        /// </summary>
        public bool CanMarkSelectedVerified
        {
            get
            {
                return HasSelection
                    && !SelectedItem.IsVerified;
            }
        }

        /// <summary>
        /// 当前是否允许取消核对。
        /// </summary>
        public bool CanCancelSelectedVerified
        {
            get
            {
                return HasSelection
                    && SelectedItem.IsVerified;
            }
        }

        /// <summary>
        /// 当前检查模式文本。
        /// </summary>
        public string SelectedCheckModeText
        {
            get
            {
                if (!HasSelection)
                {
                    return "未选择点位";
                }

                if (IsSelectedDi)
                {
                    return "DI 状态读取";
                }

                return "DO 手动调试";
            }
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
        /// 选中指定行。
        /// </summary>
        public void SelectRow(int ioMapId)
        {
            SelectedItem = _items.FirstOrDefault(p => p.IoMapId == ioMapId);
            RaiseSelectionStateChanged();
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
        /// 标记当前选中点位已核对。
        /// </summary>
        public Result MarkSelectedVerified()
        {
            if (SelectedItem == null)
            {
                return Result.Fail(-1, "当前未选择任何行", ResultSource.UI);
            }

            var entityResult = BuildWiringEntityFromSelection();
            if (!entityResult.Success)
            {
                return entityResult;
            }

            entityResult.Item.IsVerified = true;
            entityResult.Item.VerifiedBy = GetCurrentVerifier();
            entityResult.Item.VerifiedTime = DateTime.Now;

            return _ioWiringCrudService.Save(entityResult.Item);
        }

        /// <summary>
        /// 取消当前选中点位核对状态。
        /// </summary>
        public Result CancelSelectedVerified()
        {
            if (SelectedItem == null)
            {
                return Result.Fail(-1, "当前未选择任何行", ResultSource.UI);
            }

            var entityResult = BuildWiringEntityFromSelection();
            if (!entityResult.Success)
            {
                return entityResult;
            }

            entityResult.Item.IsVerified = false;
            entityResult.Item.VerifiedBy = null;
            entityResult.Item.VerifiedTime = null;

            return _ioWiringCrudService.Save(entityResult.Item);
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
                    RaiseSelectionStateChanged();
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

            _items = query
                .OrderBy(p => p.CardId)
                .ThenBy(p => p.IoType)
                .ThenBy(p => p.LogicalBit)
                .ToList();

            SelectedItem = ResolveSelectedItem();
            RefreshStats();
            RaiseSelectionStateChanged();
            OnPropertyChanged(nameof(Items));
        }

        private void RaiseSelectionStateChanged()
        {
            OnPropertyChanged(nameof(HasSelection));
            OnPropertyChanged(nameof(IsSelectedDi));
            OnPropertyChanged(nameof(IsSelectedDo));
            OnPropertyChanged(nameof(CanReadSelectedDi));
            OnPropertyChanged(nameof(CanSetSelectedDo));
            OnPropertyChanged(nameof(CanMarkSelectedVerified));
            OnPropertyChanged(nameof(CanCancelSelectedVerified));
            OnPropertyChanged(nameof(SelectedCheckModeText));
        }

        private Result<MotionIoWiringEntity> BuildWiringEntityFromSelection()
        {
            var existing = _ioWiringCrudService.QueryByIoMapId(SelectedItem.IoMapId);
            if (existing.Success && existing.Item != null)
            {
                return Result<MotionIoWiringEntity>.OkItem(existing.Item, "IO接线信息查询成功", ResultSource.UI);
            }

            if (existing.Code != (int)DbErrorCode.NotFound)
            {
                return Result<MotionIoWiringEntity>.Fail(existing.Code, existing.Message, existing.Source);
            }

            return Result<MotionIoWiringEntity>.OkItem(new MotionIoWiringEntity
            {
                IoMapId = SelectedItem.IoMapId,
                CardId = SelectedItem.CardId,
                IoType = SelectedItem.IoType,
                LogicalBit = SelectedItem.LogicalBit,
                TerminalBlock = NullIfWhiteSpace(SelectedItem.TerminalBlock),
                TerminalNo = NullIfWhiteSpace(SelectedItem.TerminalNo),
                ConnectorNo = NullIfWhiteSpace(SelectedItem.ConnectorNo),
                PinNo = NullIfWhiteSpace(SelectedItem.PinNo),
                WireNo = NullIfWhiteSpace(SelectedItem.WireNo),
                DeviceName = NullIfWhiteSpace(SelectedItem.DeviceName),
                DeviceModel = NullIfWhiteSpace(SelectedItem.DeviceModel),
                DeviceTerminal = NullIfWhiteSpace(SelectedItem.DeviceTerminal),
                CabinetArea = NullIfWhiteSpace(SelectedItem.CabinetArea),
                SignalType = NullIfWhiteSpace(SelectedItem.SignalType),
                ExpectedNormalState = NullIfWhiteSpace(SelectedItem.ExpectedNormalState),
                CheckMethod = NullIfWhiteSpace(SelectedItem.CheckMethod),
                Remark = NullIfWhiteSpace(SelectedItem.WiringRemark)
            }, "IO接线信息初始化成功", ResultSource.UI);
        }

        private static string GetCurrentVerifier()
        {
            if (!string.IsNullOrWhiteSpace(UserContext.Instance.UserName))
            {
                return UserContext.Instance.UserName;
            }

            return string.IsNullOrWhiteSpace(UserContext.Instance.LoginName)
                ? "System"
                : UserContext.Instance.LoginName;
        }

        private static string NullIfWhiteSpace(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }

        /// <summary>
        /// 导出当前筛选结果为 CSV 文本。
        /// </summary>
        public string ExportCurrentItemsToCsv()
        {
            var builder = new StringBuilder();
            builder.AppendLine("类型,逻辑IO号,硬件位号,显示名,控制卡,板载扩展,端子排,端子号,插头号,针脚号,线号,对端设备,设备型号,对端端子,区域,信号类型,常态,点检方法,接线备注,是否核对,核对人,当前值,更新时间,运行状态,接线状态");

            foreach (var item in _items)
            {
                builder.AppendLine(string.Join(",", new[]
                {
                    EscapeCsv(item.IoType),
                    EscapeCsv(item.LogicalBit.ToString()),
                    EscapeCsv(item.HardwareBit.ToString()),
                    EscapeCsv(item.DisplayName),
                    EscapeCsv(item.CardDisplayName),
                    EscapeCsv(item.IsExtModule ? "扩展" : "板载"),
                    EscapeCsv(item.TerminalBlock),
                    EscapeCsv(item.TerminalNo),
                    EscapeCsv(item.ConnectorNo),
                    EscapeCsv(item.PinNo),
                    EscapeCsv(item.WireNo),
                    EscapeCsv(item.DeviceName),
                    EscapeCsv(item.DeviceModel),
                    EscapeCsv(item.DeviceTerminal),
                    EscapeCsv(item.CabinetArea),
                    EscapeCsv(item.SignalType),
                    EscapeCsv(item.ExpectedNormalState),
                    EscapeCsv(item.CheckMethod),
                    EscapeCsv(item.WiringRemark),
                    EscapeCsv(item.IsVerified ? "是" : "否"),
                    EscapeCsv(item.VerifiedBy),
                    EscapeCsv(item.CurrentValueText),
                    EscapeCsv(item.LastUpdateTimeText),
                    EscapeCsv(item.RuntimeStatusText),
                    EscapeCsv(item.WiringStatusText)
                }));
            }

            return builder.ToString();
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
            SummaryText = "共 " + TotalCount + " 项，已核对 " + VerifiedCount + " 项，未核对 " + UnverifiedCount + " 项";
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
                DeviceModel = model.DeviceModel ?? string.Empty,
                DeviceTerminal = model.DeviceTerminal ?? string.Empty,
                CabinetArea = model.CabinetArea ?? string.Empty,
                SignalType = model.SignalType ?? string.Empty,
                ExpectedNormalState = model.ExpectedNormalState ?? string.Empty,
                CheckMethod = model.CheckMethod ?? string.Empty,
                WiringRemark = model.WiringRemark ?? string.Empty,
                IsVerified = model.IsVerified,
                VerifiedBy = model.VerifiedBy ?? string.Empty,
                CurrentValueText = model.CurrentValueText ?? string.Empty,
                LastUpdateTimeText = model.LastUpdateTimeText ?? string.Empty,
                RuntimeStatusText = model.RuntimeStatusText ?? string.Empty,
                WiringStatusText = model.WiringStatusText ?? string.Empty,
                CanManualOperate = model.CanManualOperate
            };
        }

        private static string EscapeCsv(string value)
        {
            value = value ?? string.Empty;
            value = value.Replace("\r", " ").Replace("\n", " ");

            if (value.Contains("\"") || value.Contains(","))
            {
                return "\"" + value.Replace("\"", "\"\"") + "\"";
            }

            return value;
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

            public string DeviceModel { get; set; }

            public string DeviceTerminal { get; set; }

            public string CabinetArea { get; set; }

            public string SignalType { get; set; }

            public string ExpectedNormalState { get; set; }

            public string CheckMethod { get; set; }

            public string WiringRemark { get; set; }

            public bool IsVerified { get; set; }

            public string VerifiedBy { get; set; }

            public string CurrentValueText { get; set; }

            public string LastUpdateTimeText { get; set; }

            public string RuntimeStatusText { get; set; }

            public string WiringStatusText { get; set; }

            public bool CanManualOperate { get; set; }
        }
    }
}
