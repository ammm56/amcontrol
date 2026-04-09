using AM.DBService.Services;
using AM.DBService.Services.Motion.Topology;
using AM.Model.Common;
using AM.Model.Entity.Motion;
using AM.Model.Entity.Motion.Topology;
using AM.PageModel.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AM.PageModel.MotionConfig
{
    /// <summary>
    /// 轴运行参数配置页面模型。
    /// </summary>
    public class MotionAxisParamManagementPageModel : BindableBase
    {
        public const string GroupAll = "All";
        public const string GroupHardware = "Hardware";
        public const string GroupScale = "Scale";
        public const string GroupMotion = "Motion";
        public const string GroupHome = "Home";
        public const string GroupSoftLimit = "SoftLimit";
        public const string GroupTiming = "Timing";
        public const string GroupSafety = "Safety";

        private static readonly string[] GroupOrder = new[]
        {
            GroupHardware,
            GroupScale,
            GroupMotion,
            GroupHome,
            GroupSoftLimit,
            GroupTiming,
            GroupSafety
        };

        private readonly MotionAxisConfigService _axisConfigService;
        private readonly MotionAxisCrudService _axisCrudService;

        private List<AxisSummaryItem> _axes;
        private List<AxisParamViewItem> _allItems;
        private List<AxisParamViewItem> _items;
        private List<AxisParamGroupItem> _groups;
        private short? _selectedLogicalAxis;
        private string _selectedGroupKey;

        public MotionAxisParamManagementPageModel()
        {
            _axisConfigService = new MotionAxisConfigService();
            _axisCrudService = new MotionAxisCrudService();

            _axes = new List<AxisSummaryItem>();
            _allItems = new List<AxisParamViewItem>();
            _items = new List<AxisParamViewItem>();
            _groups = new List<AxisParamGroupItem>();
            _selectedGroupKey = GroupHardware;
        }

        public IList<AxisSummaryItem> Axes
        {
            get { return _axes; }
        }

        public IList<AxisParamViewItem> Items
        {
            get { return _items; }
        }

        public IList<AxisParamGroupItem> Groups
        {
            get { return _groups; }
        }

        public short? SelectedLogicalAxis
        {
            get { return _selectedLogicalAxis; }
            set
            {
                if (_selectedLogicalAxis == value)
                    return;

                _selectedLogicalAxis = value;
                OnPropertyChanged(nameof(SelectedLogicalAxis));
                OnPropertyChanged(nameof(SelectedAxisText));
                OnPropertyChanged(nameof(IsAxisSelected));
            }
        }

        public string SelectedGroupKey
        {
            get { return _selectedGroupKey; }
            set
            {
                var normalized = NormalizeGroupKey(value);
                if (string.Equals(_selectedGroupKey, normalized, StringComparison.OrdinalIgnoreCase))
                    return;

                _selectedGroupKey = normalized;
                RebuildFilteredItems();
                OnPropertyChanged(nameof(SelectedGroupKey));
            }
        }

        public bool IsAxisSelected
        {
            get { return _selectedLogicalAxis.HasValue; }
        }

        public string SelectedAxisText
        {
            get
            {
                if (!_selectedLogicalAxis.HasValue)
                    return "当前：未选择轴";

                var axis = _axes.FirstOrDefault(x => x.LogicalAxis == _selectedLogicalAxis.Value);
                if (axis == null)
                    return "当前：逻辑轴 " + _selectedLogicalAxis.Value;

                return "当前：" + axis.DisplayText;
            }
        }

        public async Task<Result> LoadAsync()
        {
            return await Task.Run(() =>
            {
                var axisResult = _axisCrudService.QueryAll();
                if (!axisResult.Success)
                {
                    _axes = new List<AxisSummaryItem>();
                    _allItems = new List<AxisParamViewItem>();
                    _items = new List<AxisParamViewItem>();
                    RebuildGroups();

                    OnPropertyChanged(nameof(Axes));
                    OnPropertyChanged(nameof(Items));
                    OnPropertyChanged(nameof(Groups));
                    OnPropertyChanged(nameof(SelectedAxisText));
                    OnPropertyChanged(nameof(IsAxisSelected));

                    return Result.Fail(axisResult.Code, axisResult.Message, axisResult.Source);
                }

                _axes = (axisResult.Items ?? new List<MotionAxisEntity>())
                    .OrderBy(x => x.SortOrder)
                    .ThenBy(x => x.LogicalAxis)
                    .Select(ToAxisSummaryItem)
                    .ToList();

                if (_selectedLogicalAxis.HasValue && !_axes.Any(x => x.LogicalAxis == _selectedLogicalAxis.Value))
                {
                    _selectedLogicalAxis = null;
                }

                if (!_selectedLogicalAxis.HasValue && _axes.Count > 0)
                {
                    _selectedLogicalAxis = _axes[0].LogicalAxis;
                }

                if (!_selectedLogicalAxis.HasValue)
                {
                    _allItems = new List<AxisParamViewItem>();
                    _items = new List<AxisParamViewItem>();
                    RebuildGroups();

                    OnPropertyChanged(nameof(Axes));
                    OnPropertyChanged(nameof(Items));
                    OnPropertyChanged(nameof(Groups));
                    OnPropertyChanged(nameof(SelectedAxisText));
                    OnPropertyChanged(nameof(IsAxisSelected));

                    return Result.Ok("轴列表加载成功");
                }

                return LoadParamsCore();
            });
        }

        public async Task<Result> SelectAxisAsync(short logicalAxis)
        {
            SelectedLogicalAxis = logicalAxis;
            SelectedGroupKey = GroupHardware;

            return await Task.Run(() => LoadParamsCore());
        }

        public void ClearAxisSelection()
        {
            SelectedLogicalAxis = null;
            _allItems = new List<AxisParamViewItem>();
            _items = new List<AxisParamViewItem>();
            RebuildGroups();

            OnPropertyChanged(nameof(Items));
            OnPropertyChanged(nameof(Groups));
        }

        public async Task<Result> SaveAsync(MotionAxisConfigEntity entity)
        {
            if (entity == null)
                return Result.Fail(-1, "参数不能为空", ResultSource.Unknown);

            return await Task.Run(() => _axisConfigService.Save(entity));
        }

        public async Task<Result> DeleteAsync(string paramName)
        {
            if (!_selectedLogicalAxis.HasValue || string.IsNullOrWhiteSpace(paramName))
                return Result.Fail(-1, "删除参数无效", ResultSource.Unknown);

            return await Task.Run(() => _axisConfigService.Delete(_selectedLogicalAxis.Value, paramName));
        }

        public MotionAxisConfigEntity CreateDefaultEntity()
        {
            var logicalAxis = _selectedLogicalAxis ?? (short)0;
            var axis = _axes.FirstOrDefault(x => x.LogicalAxis == logicalAxis);

            return new MotionAxisConfigEntity
            {
                LogicalAxis = logicalAxis,
                ParamName = string.Empty,
                ParamDisplayName = string.Empty,
                ParamGroup = SelectedGroupKey == GroupAll ? GroupMotion : SelectedGroupKey,
                ParamValueType = "Double",
                ParamSetValue = 0D,
                ParamDefaultValue = 0D,
                ParamMaxValue = 0D,
                ParamMinValue = 0D,
                Unit = string.Empty,
                Description = string.Empty,
                ValueDescription = string.Empty,
                VendorScope = "All",
                Remark = string.Empty,
                AxisDisplayName = axis == null ? string.Empty : axis.AxisDisplayName
            };
        }

        private Result LoadParamsCore()
        {
            if (!_selectedLogicalAxis.HasValue)
            {
                _allItems = new List<AxisParamViewItem>();
                _items = new List<AxisParamViewItem>();
                RebuildGroups();

                OnPropertyChanged(nameof(Items));
                OnPropertyChanged(nameof(Groups));
                OnPropertyChanged(nameof(SelectedAxisText));
                OnPropertyChanged(nameof(IsAxisSelected));

                return Result.Ok("当前未选择轴");
            }

            var result = _axisConfigService.QueryByLogicalAxis(_selectedLogicalAxis.Value);
            if (!result.Success)
            {
                _allItems = new List<AxisParamViewItem>();
                _items = new List<AxisParamViewItem>();
                RebuildGroups();

                OnPropertyChanged(nameof(Axes));
                OnPropertyChanged(nameof(Items));
                OnPropertyChanged(nameof(Groups));
                OnPropertyChanged(nameof(SelectedAxisText));
                OnPropertyChanged(nameof(IsAxisSelected));

                return Result.Fail(result.Code, result.Message, result.Source);
            }

            _allItems = (result.Items ?? new List<MotionAxisConfigEntity>())
                .OrderBy(x => GroupOrderIndex(x.ParamGroup))
                .ThenBy(x => x.ParamDisplayName)
                .ThenBy(x => x.ParamName)
                .Select(ToViewItem)
                .ToList();

            RebuildGroups();
            RebuildFilteredItems();

            OnPropertyChanged(nameof(Axes));
            OnPropertyChanged(nameof(Items));
            OnPropertyChanged(nameof(Groups));
            OnPropertyChanged(nameof(SelectedAxisText));
            OnPropertyChanged(nameof(IsAxisSelected));

            return Result.Ok("轴参数加载成功");
        }

        private void RebuildGroups()
        {
            var list = new List<AxisParamGroupItem>();
            list.Add(new AxisParamGroupItem(GroupAll, "全部参数"));

            foreach (var key in GroupOrder)
            {
                if (_allItems.Any(x => string.Equals(x.ParamGroup, key, StringComparison.OrdinalIgnoreCase)))
                {
                    list.Add(new AxisParamGroupItem(key, GetGroupDisplayName(key)));
                }
            }

            var customKeys = _allItems
                .Select(x => x.ParamGroup)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Where(x => !GroupOrder.Any(g => string.Equals(g, x, StringComparison.OrdinalIgnoreCase)))
                .OrderBy(x => x)
                .ToList();

            foreach (var key in customKeys)
            {
                list.Add(new AxisParamGroupItem(key, GetGroupDisplayName(key)));
            }

            _groups = list;
        }

        private void RebuildFilteredItems()
        {
            IEnumerable<AxisParamViewItem> query = _allItems;

            if (!string.Equals(_selectedGroupKey, GroupAll, StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(x => string.Equals(x.ParamGroup, _selectedGroupKey, StringComparison.OrdinalIgnoreCase));
            }

            _items = query.ToList();

            OnPropertyChanged(nameof(Items));
            OnPropertyChanged(nameof(Groups));
        }

        private static AxisSummaryItem ToAxisSummaryItem(MotionAxisEntity entity)
        {
            var displayName = string.IsNullOrWhiteSpace(entity.DisplayName)
                ? (string.IsNullOrWhiteSpace(entity.Name) ? "轴-" + entity.LogicalAxis : entity.Name)
                : entity.DisplayName;

            return new AxisSummaryItem
            {
                LogicalAxis = entity.LogicalAxis,
                Name = entity.Name ?? string.Empty,
                DisplayName = displayName,
                CardId = entity.CardId,
                AxisDisplayName = displayName
            };
        }

        private static AxisParamViewItem ToViewItem(MotionAxisConfigEntity entity)
        {
            return new AxisParamViewItem
            {
                Id = entity.Id,
                LogicalAxis = entity.LogicalAxis,
                ParamName = entity.ParamName ?? string.Empty,
                ParamDisplayName = string.IsNullOrWhiteSpace(entity.ParamDisplayName) ? (entity.ParamName ?? string.Empty) : entity.ParamDisplayName,
                ParamGroup = NormalizeGroupKey(entity.ParamGroup),
                ParamValueType = string.IsNullOrWhiteSpace(entity.ParamValueType) ? "Double" : entity.ParamValueType,
                ParamSetValue = entity.ParamSetValue,
                ParamDefaultValue = entity.ParamDefaultValue,
                ParamMaxValue = entity.ParamMaxValue,
                ParamMinValue = entity.ParamMinValue,
                Unit = entity.Unit ?? string.Empty,
                Description = entity.Description ?? string.Empty,
                ValueDescription = entity.ValueDescription ?? string.Empty,
                VendorScope = string.IsNullOrWhiteSpace(entity.VendorScope) ? "All" : entity.VendorScope,
                Remark = entity.Remark ?? string.Empty,
                AxisDisplayName = entity.AxisDisplayName ?? string.Empty
            };
        }

        private static int GroupOrderIndex(string groupKey)
        {
            var normalized = NormalizeGroupKey(groupKey);
            for (var i = 0; i < GroupOrder.Length; i++)
            {
                if (string.Equals(GroupOrder[i], normalized, StringComparison.OrdinalIgnoreCase))
                    return i;
            }

            return GroupOrder.Length + 1;
        }

        public static string GetGroupDisplayName(string groupKey)
        {
            switch (NormalizeGroupKey(groupKey))
            {
                case GroupHardware: return "硬件信号";
                case GroupScale: return "单位换算";
                case GroupMotion: return "运动参数";
                case GroupHome: return "回零参数";
                case GroupSoftLimit: return "软件限位";
                case GroupTiming: return "使能时序";
                case GroupSafety: return "安全联动";
                case GroupAll: return "全部参数";
                default: return string.IsNullOrWhiteSpace(groupKey) ? "未分组" : groupKey;
            }
        }

        private static string NormalizeGroupKey(string groupKey)
        {
            if (string.IsNullOrWhiteSpace(groupKey))
                return GroupMotion;

            if (string.Equals(groupKey, GroupAll, StringComparison.OrdinalIgnoreCase))
                return GroupAll;
            if (string.Equals(groupKey, GroupHardware, StringComparison.OrdinalIgnoreCase))
                return GroupHardware;
            if (string.Equals(groupKey, GroupScale, StringComparison.OrdinalIgnoreCase))
                return GroupScale;
            if (string.Equals(groupKey, GroupMotion, StringComparison.OrdinalIgnoreCase))
                return GroupMotion;
            if (string.Equals(groupKey, GroupHome, StringComparison.OrdinalIgnoreCase))
                return GroupHome;
            if (string.Equals(groupKey, GroupSoftLimit, StringComparison.OrdinalIgnoreCase))
                return GroupSoftLimit;
            if (string.Equals(groupKey, GroupTiming, StringComparison.OrdinalIgnoreCase))
                return GroupTiming;
            if (string.Equals(groupKey, GroupSafety, StringComparison.OrdinalIgnoreCase))
                return GroupSafety;

            return groupKey.Trim();
        }

        public sealed class AxisSummaryItem
        {
            public short LogicalAxis { get; set; }

            public string Name { get; set; }

            public string DisplayName { get; set; }

            public short CardId { get; set; }

            public string AxisDisplayName { get; set; }

            public string DisplayText
            {
                get
                {
                    return "L" + LogicalAxis + " · " + (string.IsNullOrWhiteSpace(DisplayName) ? Name : DisplayName);
                }
            }
        }

        public sealed class AxisParamGroupItem
        {
            public AxisParamGroupItem(string key, string displayName)
            {
                Key = key;
                DisplayName = displayName;
            }

            public string Key { get; private set; }

            public string DisplayName { get; private set; }
        }

        public sealed class AxisParamViewItem
        {
            public int Id { get; set; }

            public short LogicalAxis { get; set; }

            public string ParamName { get; set; }

            public string ParamDisplayName { get; set; }

            public string ParamGroup { get; set; }

            public string ParamValueType { get; set; }

            public double ParamSetValue { get; set; }

            public double ParamDefaultValue { get; set; }

            public double ParamMaxValue { get; set; }

            public double ParamMinValue { get; set; }

            public string Unit { get; set; }

            public string Description { get; set; }

            public string ValueDescription { get; set; }

            public string VendorScope { get; set; }

            public string Remark { get; set; }

            public string AxisDisplayName { get; set; }

            public string GroupDisplayName
            {
                get { return GetGroupDisplayName(ParamGroup); }
            }

            public string ParamValueText
            {
                get { return FormatValue(ParamSetValue, ParamValueType); }
            }

            public string ParamDefaultValueText
            {
                get { return FormatValue(ParamDefaultValue, ParamValueType); }
            }

            public string UnitText
            {
                get { return string.IsNullOrWhiteSpace(Unit) ? "-" : Unit; }
            }

            public string ValueDescriptionText
            {
                get { return string.IsNullOrWhiteSpace(ValueDescription) ? "-" : ValueDescription; }
            }

            public string DescriptionText
            {
                get { return string.IsNullOrWhiteSpace(Description) ? "-" : Description; }
            }

            public string RemarkText
            {
                get { return string.IsNullOrWhiteSpace(Remark) ? "-" : Remark; }
            }

            public string RangeText
            {
                get
                {
                    if (string.Equals(ParamValueType, "Bool", StringComparison.OrdinalIgnoreCase))
                        return "0 / 1";

                    if (ParamMinValue == 0D && ParamMaxValue == 0D)
                        return "-";

                    return FormatValue(ParamMinValue, ParamValueType) + " ~ " + FormatValue(ParamMaxValue, ParamValueType);
                }
            }

            public string TypeLabel
            {
                get
                {
                    if (string.Equals(ParamValueType, "Bool", StringComparison.OrdinalIgnoreCase))
                        return "布尔";
                    if (string.Equals(ParamValueType, "Int16", StringComparison.OrdinalIgnoreCase))
                        return "整数";
                    if (string.Equals(ParamValueType, "Int32", StringComparison.OrdinalIgnoreCase))
                        return "整数";
                    if (string.Equals(ParamValueType, "Enum", StringComparison.OrdinalIgnoreCase))
                        return "枚举";

                    return "小数";
                }
            }

            public MotionAxisConfigEntity ToEntity()
            {
                return new MotionAxisConfigEntity
                {
                    Id = Id,
                    LogicalAxis = LogicalAxis,
                    ParamName = ParamName,
                    ParamDisplayName = ParamDisplayName,
                    ParamGroup = ParamGroup,
                    ParamValueType = ParamValueType,
                    ParamSetValue = ParamSetValue,
                    ParamDefaultValue = ParamDefaultValue,
                    ParamMaxValue = ParamMaxValue,
                    ParamMinValue = ParamMinValue,
                    Unit = Unit,
                    Description = Description,
                    ValueDescription = ValueDescription,
                    VendorScope = VendorScope,
                    Remark = Remark,
                    AxisDisplayName = AxisDisplayName
                };
            }

            private static string FormatValue(double value, string valueType)
            {
                if (string.Equals(valueType, "Bool", StringComparison.OrdinalIgnoreCase))
                    return value >= 0.5D ? "1" : "0";

                if (string.Equals(valueType, "Int16", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(valueType, "Int32", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(valueType, "Enum", StringComparison.OrdinalIgnoreCase))
                {
                    return ((long)Math.Round(value)).ToString(CultureInfo.InvariantCulture);
                }

                return value.ToString("G6", CultureInfo.InvariantCulture);
            }
        }
    }
}