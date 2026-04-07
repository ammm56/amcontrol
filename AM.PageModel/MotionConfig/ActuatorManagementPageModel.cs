using AM.DBService.Services.Motion.Actuator;
using AM.Model.Common;
using AM.Model.Entity.Motion.Actuator;
using AM.PageModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AM.PageModel.MotionConfig
{
    /// <summary>
    /// 执行器配置页面模型。
    /// 统一承载气缸、真空、灯塔、夹爪四类执行器配置。
    /// </summary>
    public class ActuatorManagementPageModel : BindableBase
    {
        public const string TypeAll = "All";
        public const string TypeCylinder = "Cylinder";
        public const string TypeVacuum = "Vacuum";
        public const string TypeStackLight = "StackLight";
        public const string TypeGripper = "Gripper";

        private static readonly string[] TypeOrder = new[]
        {
            TypeCylinder,
            TypeVacuum,
            TypeStackLight,
            TypeGripper
        };

        private readonly MotionCylinderConfigCrudService _cylinderCrudService;
        private readonly MotionVacuumConfigCrudService _vacuumCrudService;
        private readonly MotionStackLightConfigCrudService _stackLightCrudService;
        private readonly MotionGripperConfigCrudService _gripperCrudService;

        private List<ActuatorCategoryItem> _categories;
        private List<ActuatorViewItem> _allItems;
        private List<ActuatorViewItem> _items;
        private string _selectedCategoryKey;

        public ActuatorManagementPageModel()
        {
            _cylinderCrudService = new MotionCylinderConfigCrudService();
            _vacuumCrudService = new MotionVacuumConfigCrudService();
            _stackLightCrudService = new MotionStackLightConfigCrudService();
            _gripperCrudService = new MotionGripperConfigCrudService();

            _categories = CreateDefaultCategories();
            _allItems = new List<ActuatorViewItem>();
            _items = new List<ActuatorViewItem>();
            _selectedCategoryKey = TypeAll;
        }

        public IList<ActuatorCategoryItem> Categories
        {
            get { return _categories; }
        }

        public IList<ActuatorViewItem> Items
        {
            get { return _items; }
        }

        public string SelectedCategoryKey
        {
            get { return _selectedCategoryKey; }
            set
            {
                var normalized = NormalizeTypeKey(value);
                if (string.Equals(_selectedCategoryKey, normalized, StringComparison.OrdinalIgnoreCase))
                    return;

                _selectedCategoryKey = normalized;
                RebuildFilteredItems();
                OnPropertyChanged(nameof(SelectedCategoryKey));
            }
        }

        public int TotalCount
        {
            get { return _allItems.Count; }
        }

        public string SummaryText
        {
            get
            {
                return "共 " + TotalCount + " 项";
            }
        }

        public async Task<Result> LoadAsync()
        {
            return await Task.Run(() =>
            {
                var cylindersResult = _cylinderCrudService.QueryAll();
                if (!cylindersResult.Success)
                    return Result.Fail(cylindersResult.Code, cylindersResult.Message, cylindersResult.Source);

                var vacuumsResult = _vacuumCrudService.QueryAll();
                if (!vacuumsResult.Success)
                    return Result.Fail(vacuumsResult.Code, vacuumsResult.Message, vacuumsResult.Source);

                var stackLightsResult = _stackLightCrudService.QueryAll();
                if (!stackLightsResult.Success)
                    return Result.Fail(stackLightsResult.Code, stackLightsResult.Message, stackLightsResult.Source);

                var grippersResult = _gripperCrudService.QueryAll();
                if (!grippersResult.Success)
                    return Result.Fail(grippersResult.Code, grippersResult.Message, grippersResult.Source);

                var unifiedItems = new List<ActuatorViewItem>();

                foreach (var item in (cylindersResult.Items ?? new List<CylinderConfigEntity>())
                    .OrderBy(x => x.SortOrder)
                    .ThenBy(x => x.DisplayName)
                    .ThenBy(x => x.Name))
                {
                    unifiedItems.Add(ToCylinderViewItem(item));
                }

                foreach (var item in (vacuumsResult.Items ?? new List<VacuumConfigEntity>())
                    .OrderBy(x => x.SortOrder)
                    .ThenBy(x => x.DisplayName)
                    .ThenBy(x => x.Name))
                {
                    unifiedItems.Add(ToVacuumViewItem(item));
                }

                foreach (var item in (stackLightsResult.Items ?? new List<StackLightConfigEntity>())
                    .OrderBy(x => x.SortOrder)
                    .ThenBy(x => x.DisplayName)
                    .ThenBy(x => x.Name))
                {
                    unifiedItems.Add(ToStackLightViewItem(item));
                }

                foreach (var item in (grippersResult.Items ?? new List<GripperConfigEntity>())
                    .OrderBy(x => x.SortOrder)
                    .ThenBy(x => x.DisplayName)
                    .ThenBy(x => x.Name))
                {
                    unifiedItems.Add(ToGripperViewItem(item));
                }

                _allItems = unifiedItems
                    .OrderBy(x => TypeOrderIndex(x.ActuatorType))
                    .ThenBy(x => x.SortOrder)
                    .ThenBy(x => x.DisplayName)
                    .ThenBy(x => x.Name)
                    .ToList();

                _categories = CreateDefaultCategories();
                RebuildFilteredItems();

                OnPropertyChanged(nameof(Categories));
                OnPropertyChanged(nameof(Items));
                OnPropertyChanged(nameof(TotalCount));
                OnPropertyChanged(nameof(SummaryText));

                return Result.Ok("执行器配置加载成功");
            });
        }

        public async Task<Result> SaveAsync(string actuatorType, object entity)
        {
            var normalizedType = NormalizeTypeKey(actuatorType);

            return await Task.Run(() =>
            {
                switch (normalizedType)
                {
                    case TypeCylinder:
                        return entity is CylinderConfigEntity
                            ? _cylinderCrudService.Save((CylinderConfigEntity)entity)
                            : Result.Fail(-1, "气缸参数无效", ResultSource.Unknown);

                    case TypeVacuum:
                        return entity is VacuumConfigEntity
                            ? _vacuumCrudService.Save((VacuumConfigEntity)entity)
                            : Result.Fail(-1, "真空参数无效", ResultSource.Unknown);

                    case TypeStackLight:
                        return entity is StackLightConfigEntity
                            ? _stackLightCrudService.Save((StackLightConfigEntity)entity)
                            : Result.Fail(-1, "灯塔参数无效", ResultSource.Unknown);

                    case TypeGripper:
                        return entity is GripperConfigEntity
                            ? _gripperCrudService.Save((GripperConfigEntity)entity)
                            : Result.Fail(-1, "夹爪参数无效", ResultSource.Unknown);

                    default:
                        return Result.Fail(-1, "执行器类型无效", ResultSource.Unknown);
                }
            });
        }

        public async Task<Result> DeleteAsync(ActuatorViewItem item)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.ActuatorType) || string.IsNullOrWhiteSpace(item.Name))
                return Result.Fail(-1, "删除目标无效", ResultSource.Unknown);

            var normalizedType = NormalizeTypeKey(item.ActuatorType);

            return await Task.Run(() =>
            {
                switch (normalizedType)
                {
                    case TypeCylinder:
                        return _cylinderCrudService.DeleteByName(item.Name);

                    case TypeVacuum:
                        return _vacuumCrudService.DeleteByName(item.Name);

                    case TypeStackLight:
                        return _stackLightCrudService.DeleteByName(item.Name);

                    case TypeGripper:
                        return _gripperCrudService.DeleteByName(item.Name);

                    default:
                        return Result.Fail(-1, "执行器类型无效", ResultSource.Unknown);
                }
            });
        }

        public object CreateDefaultEntity(string actuatorType)
        {
            var normalizedType = NormalizeTypeKey(actuatorType);
            if (string.Equals(normalizedType, TypeAll, StringComparison.OrdinalIgnoreCase))
                normalizedType = TypeCylinder;

            switch (normalizedType)
            {
                case TypeCylinder:
                    return new CylinderConfigEntity
                    {
                        Name = string.Empty,
                        DisplayName = string.Empty,
                        DriveMode = "Double",
                        ExtendOutputBit = 0,
                        RetractOutputBit = null,
                        ExtendFeedbackBit = null,
                        RetractFeedbackBit = null,
                        UseFeedbackCheck = false,
                        ExtendTimeoutMs = 1000,
                        RetractTimeoutMs = 1000,
                        AllowBothOff = false,
                        AllowBothOn = false,
                        IsEnabled = true,
                        SortOrder = _allItems.Count + 1,
                        Description = string.Empty,
                        Remark = string.Empty
                    };

                case TypeVacuum:
                    return new VacuumConfigEntity
                    {
                        Name = string.Empty,
                        DisplayName = string.Empty,
                        VacuumOnOutputBit = 0,
                        BlowOffOutputBit = null,
                        VacuumFeedbackBit = null,
                        ReleaseFeedbackBit = null,
                        WorkpiecePresentBit = null,
                        UseFeedbackCheck = false,
                        UseWorkpieceCheck = false,
                        VacuumBuildTimeoutMs = 1000,
                        ReleaseTimeoutMs = 1000,
                        KeepVacuumOnAfterDetected = true,
                        IsEnabled = true,
                        SortOrder = _allItems.Count + 1,
                        Description = string.Empty,
                        Remark = string.Empty
                    };

                case TypeStackLight:
                    return new StackLightConfigEntity
                    {
                        Name = string.Empty,
                        DisplayName = string.Empty,
                        RedOutputBit = null,
                        YellowOutputBit = null,
                        GreenOutputBit = null,
                        BlueOutputBit = null,
                        BuzzerOutputBit = null,
                        EnableBuzzerOnWarning = false,
                        EnableBuzzerOnAlarm = false,
                        AllowMultiSegmentOn = false,
                        IsEnabled = true,
                        SortOrder = _allItems.Count + 1,
                        Description = string.Empty,
                        Remark = string.Empty
                    };

                case TypeGripper:
                    return new GripperConfigEntity
                    {
                        Name = string.Empty,
                        DisplayName = string.Empty,
                        DriveMode = "Double",
                        CloseOutputBit = 0,
                        OpenOutputBit = null,
                        CloseFeedbackBit = null,
                        OpenFeedbackBit = null,
                        WorkpiecePresentBit = null,
                        UseFeedbackCheck = false,
                        UseWorkpieceCheck = false,
                        CloseTimeoutMs = 1000,
                        OpenTimeoutMs = 1000,
                        AllowBothOff = false,
                        AllowBothOn = false,
                        IsEnabled = true,
                        SortOrder = _allItems.Count + 1,
                        Description = string.Empty,
                        Remark = string.Empty
                    };

                default:
                    return null;
            }
        }

        private void RebuildFilteredItems()
        {
            IEnumerable<ActuatorViewItem> query = _allItems;

            if (!string.Equals(_selectedCategoryKey, TypeAll, StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(x => string.Equals(x.ActuatorType, _selectedCategoryKey, StringComparison.OrdinalIgnoreCase));
            }

            _items = query
                .OrderBy(x => TypeOrderIndex(x.ActuatorType))
                .ThenBy(x => x.SortOrder)
                .ThenBy(x => x.DisplayName)
                .ThenBy(x => x.Name)
                .ToList();

            OnPropertyChanged(nameof(Items));
            OnPropertyChanged(nameof(TotalCount));
            OnPropertyChanged(nameof(SummaryText));
        }

        private static List<ActuatorCategoryItem> CreateDefaultCategories()
        {
            return new List<ActuatorCategoryItem>
            {
                new ActuatorCategoryItem(TypeAll, "全部"),
                new ActuatorCategoryItem(TypeCylinder, "气缸"),
                new ActuatorCategoryItem(TypeVacuum, "真空"),
                new ActuatorCategoryItem(TypeStackLight, "灯塔"),
                new ActuatorCategoryItem(TypeGripper, "夹爪")
            };
        }

        private static string NormalizeTypeKey(string actuatorType)
        {
            if (string.IsNullOrWhiteSpace(actuatorType))
                return TypeAll;

            if (string.Equals(actuatorType, TypeAll, StringComparison.OrdinalIgnoreCase))
                return TypeAll;
            if (string.Equals(actuatorType, TypeCylinder, StringComparison.OrdinalIgnoreCase))
                return TypeCylinder;
            if (string.Equals(actuatorType, TypeVacuum, StringComparison.OrdinalIgnoreCase))
                return TypeVacuum;
            if (string.Equals(actuatorType, TypeStackLight, StringComparison.OrdinalIgnoreCase))
                return TypeStackLight;
            if (string.Equals(actuatorType, TypeGripper, StringComparison.OrdinalIgnoreCase))
                return TypeGripper;

            return actuatorType.Trim();
        }

        private static int TypeOrderIndex(string actuatorType)
        {
            var normalized = NormalizeTypeKey(actuatorType);
            for (var i = 0; i < TypeOrder.Length; i++)
            {
                if (string.Equals(TypeOrder[i], normalized, StringComparison.OrdinalIgnoreCase))
                    return i;
            }

            return TypeOrder.Length + 1;
        }

        private static string GetTypeDisplayName(string actuatorType)
        {
            switch (NormalizeTypeKey(actuatorType))
            {
                case TypeCylinder: return "气缸";
                case TypeVacuum: return "真空";
                case TypeStackLight: return "灯塔";
                case TypeGripper: return "夹爪";
                case TypeAll: return "全部";
                default: return actuatorType;
            }
        }

        private static ActuatorViewItem ToCylinderViewItem(CylinderConfigEntity entity)
        {
            return new ActuatorViewItem
            {
                Id = entity.Id,
                ActuatorType = TypeCylinder,
                TypeDisplayName = GetTypeDisplayName(TypeCylinder),
                Name = entity.Name ?? string.Empty,
                DisplayName = string.IsNullOrWhiteSpace(entity.DisplayName) ? (entity.Name ?? string.Empty) : entity.DisplayName,
                IsEnabled = entity.IsEnabled,
                SortOrder = entity.SortOrder,
                Description = entity.Description ?? string.Empty,
                Remark = entity.Remark ?? string.Empty,
                Summary1 = "驱动: " + (string.IsNullOrWhiteSpace(entity.DriveMode) ? "-" : entity.DriveMode),
                Summary2 = "伸出DO: " + entity.ExtendOutputBit,
                Summary3 = "缩回DO: " + (entity.RetractOutputBit.HasValue ? entity.RetractOutputBit.Value.ToString() : "-"),
                DetailLines = new List<ActuatorDetailLine>
                {
                    new ActuatorDetailLine("执行器类型", "气缸"),
                    new ActuatorDetailLine("对象名称", entity.Name),
                    new ActuatorDetailLine("显示名称", entity.DisplayName),
                    new ActuatorDetailLine("驱动模式", entity.DriveMode),
                    new ActuatorDetailLine("伸出输出位", entity.ExtendOutputBit.ToString()),
                    new ActuatorDetailLine("缩回输出位", entity.RetractOutputBit.HasValue ? entity.RetractOutputBit.Value.ToString() : "-"),
                    new ActuatorDetailLine("伸出反馈位", entity.ExtendFeedbackBit.HasValue ? entity.ExtendFeedbackBit.Value.ToString() : "-"),
                    new ActuatorDetailLine("缩回反馈位", entity.RetractFeedbackBit.HasValue ? entity.RetractFeedbackBit.Value.ToString() : "-"),
                    new ActuatorDetailLine("反馈校验", entity.UseFeedbackCheck ? "是" : "否"),
                    new ActuatorDetailLine("伸出超时(ms)", entity.ExtendTimeoutMs.ToString()),
                    new ActuatorDetailLine("缩回超时(ms)", entity.RetractTimeoutMs.ToString()),
                    new ActuatorDetailLine("允许双OFF", entity.AllowBothOff ? "是" : "否"),
                    new ActuatorDetailLine("允许双ON", entity.AllowBothOn ? "是" : "否"),
                    new ActuatorDetailLine("启用", entity.IsEnabled ? "是" : "否"),
                    new ActuatorDetailLine("排序号", entity.SortOrder.ToString()),
                    new ActuatorDetailLine("描述", string.IsNullOrWhiteSpace(entity.Description) ? "-" : entity.Description),
                    new ActuatorDetailLine("备注", string.IsNullOrWhiteSpace(entity.Remark) ? "-" : entity.Remark)
                },
                SourceEntity = entity
            };
        }

        private static ActuatorViewItem ToVacuumViewItem(VacuumConfigEntity entity)
        {
            return new ActuatorViewItem
            {
                Id = entity.Id,
                ActuatorType = TypeVacuum,
                TypeDisplayName = GetTypeDisplayName(TypeVacuum),
                Name = entity.Name ?? string.Empty,
                DisplayName = string.IsNullOrWhiteSpace(entity.DisplayName) ? (entity.Name ?? string.Empty) : entity.DisplayName,
                IsEnabled = entity.IsEnabled,
                SortOrder = entity.SortOrder,
                Description = entity.Description ?? string.Empty,
                Remark = entity.Remark ?? string.Empty,
                Summary1 = "吸DO: " + entity.VacuumOnOutputBit,
                Summary2 = "破真空DO: " + (entity.BlowOffOutputBit.HasValue ? entity.BlowOffOutputBit.Value.ToString() : "-"),
                Summary3 = "工件检测: " + (entity.UseWorkpieceCheck ? "是" : "否"),
                DetailLines = new List<ActuatorDetailLine>
                {
                    new ActuatorDetailLine("执行器类型", "真空"),
                    new ActuatorDetailLine("对象名称", entity.Name),
                    new ActuatorDetailLine("显示名称", entity.DisplayName),
                    new ActuatorDetailLine("吸真空输出位", entity.VacuumOnOutputBit.ToString()),
                    new ActuatorDetailLine("破真空输出位", entity.BlowOffOutputBit.HasValue ? entity.BlowOffOutputBit.Value.ToString() : "-"),
                    new ActuatorDetailLine("真空反馈位", entity.VacuumFeedbackBit.HasValue ? entity.VacuumFeedbackBit.Value.ToString() : "-"),
                    new ActuatorDetailLine("释放反馈位", entity.ReleaseFeedbackBit.HasValue ? entity.ReleaseFeedbackBit.Value.ToString() : "-"),
                    new ActuatorDetailLine("工件检测位", entity.WorkpiecePresentBit.HasValue ? entity.WorkpiecePresentBit.Value.ToString() : "-"),
                    new ActuatorDetailLine("反馈校验", entity.UseFeedbackCheck ? "是" : "否"),
                    new ActuatorDetailLine("工件检测校验", entity.UseWorkpieceCheck ? "是" : "否"),
                    new ActuatorDetailLine("建压超时(ms)", entity.VacuumBuildTimeoutMs.ToString()),
                    new ActuatorDetailLine("释放超时(ms)", entity.ReleaseTimeoutMs.ToString()),
                    new ActuatorDetailLine("检测后保持真空", entity.KeepVacuumOnAfterDetected ? "是" : "否"),
                    new ActuatorDetailLine("启用", entity.IsEnabled ? "是" : "否"),
                    new ActuatorDetailLine("排序号", entity.SortOrder.ToString()),
                    new ActuatorDetailLine("描述", string.IsNullOrWhiteSpace(entity.Description) ? "-" : entity.Description),
                    new ActuatorDetailLine("备注", string.IsNullOrWhiteSpace(entity.Remark) ? "-" : entity.Remark)
                },
                SourceEntity = entity
            };
        }

        private static ActuatorViewItem ToStackLightViewItem(StackLightConfigEntity entity)
        {
            return new ActuatorViewItem
            {
                Id = entity.Id,
                ActuatorType = TypeStackLight,
                TypeDisplayName = GetTypeDisplayName(TypeStackLight),
                Name = entity.Name ?? string.Empty,
                DisplayName = string.IsNullOrWhiteSpace(entity.DisplayName) ? (entity.Name ?? string.Empty) : entity.DisplayName,
                IsEnabled = entity.IsEnabled,
                SortOrder = entity.SortOrder,
                Description = entity.Description ?? string.Empty,
                Remark = entity.Remark ?? string.Empty,
                Summary1 = "红: " + (entity.RedOutputBit.HasValue ? entity.RedOutputBit.Value.ToString() : "-"),
                Summary2 = "黄: " + (entity.YellowOutputBit.HasValue ? entity.YellowOutputBit.Value.ToString() : "-"),
                Summary3 = "绿: " + (entity.GreenOutputBit.HasValue ? entity.GreenOutputBit.Value.ToString() : "-"),
                DetailLines = new List<ActuatorDetailLine>
                {
                    new ActuatorDetailLine("执行器类型", "灯塔"),
                    new ActuatorDetailLine("对象名称", entity.Name),
                    new ActuatorDetailLine("显示名称", entity.DisplayName),
                    new ActuatorDetailLine("红灯输出位", entity.RedOutputBit.HasValue ? entity.RedOutputBit.Value.ToString() : "-"),
                    new ActuatorDetailLine("黄灯输出位", entity.YellowOutputBit.HasValue ? entity.YellowOutputBit.Value.ToString() : "-"),
                    new ActuatorDetailLine("绿灯输出位", entity.GreenOutputBit.HasValue ? entity.GreenOutputBit.Value.ToString() : "-"),
                    new ActuatorDetailLine("蓝灯输出位", entity.BlueOutputBit.HasValue ? entity.BlueOutputBit.Value.ToString() : "-"),
                    new ActuatorDetailLine("蜂鸣器输出位", entity.BuzzerOutputBit.HasValue ? entity.BuzzerOutputBit.Value.ToString() : "-"),
                    new ActuatorDetailLine("警告带蜂鸣", entity.EnableBuzzerOnWarning ? "是" : "否"),
                    new ActuatorDetailLine("报警带蜂鸣", entity.EnableBuzzerOnAlarm ? "是" : "否"),
                    new ActuatorDetailLine("允许多段同亮", entity.AllowMultiSegmentOn ? "是" : "否"),
                    new ActuatorDetailLine("启用", entity.IsEnabled ? "是" : "否"),
                    new ActuatorDetailLine("排序号", entity.SortOrder.ToString()),
                    new ActuatorDetailLine("描述", string.IsNullOrWhiteSpace(entity.Description) ? "-" : entity.Description),
                    new ActuatorDetailLine("备注", string.IsNullOrWhiteSpace(entity.Remark) ? "-" : entity.Remark)
                },
                SourceEntity = entity
            };
        }

        private static ActuatorViewItem ToGripperViewItem(GripperConfigEntity entity)
        {
            return new ActuatorViewItem
            {
                Id = entity.Id,
                ActuatorType = TypeGripper,
                TypeDisplayName = GetTypeDisplayName(TypeGripper),
                Name = entity.Name ?? string.Empty,
                DisplayName = string.IsNullOrWhiteSpace(entity.DisplayName) ? (entity.Name ?? string.Empty) : entity.DisplayName,
                IsEnabled = entity.IsEnabled,
                SortOrder = entity.SortOrder,
                Description = entity.Description ?? string.Empty,
                Remark = entity.Remark ?? string.Empty,
                Summary1 = "驱动: " + (string.IsNullOrWhiteSpace(entity.DriveMode) ? "-" : entity.DriveMode),
                Summary2 = "夹紧DO: " + entity.CloseOutputBit,
                Summary3 = "打开DO: " + (entity.OpenOutputBit.HasValue ? entity.OpenOutputBit.Value.ToString() : "-"),
                DetailLines = new List<ActuatorDetailLine>
                {
                    new ActuatorDetailLine("执行器类型", "夹爪"),
                    new ActuatorDetailLine("对象名称", entity.Name),
                    new ActuatorDetailLine("显示名称", entity.DisplayName),
                    new ActuatorDetailLine("驱动模式", entity.DriveMode),
                    new ActuatorDetailLine("夹紧输出位", entity.CloseOutputBit.ToString()),
                    new ActuatorDetailLine("打开输出位", entity.OpenOutputBit.HasValue ? entity.OpenOutputBit.Value.ToString() : "-"),
                    new ActuatorDetailLine("夹紧反馈位", entity.CloseFeedbackBit.HasValue ? entity.CloseFeedbackBit.Value.ToString() : "-"),
                    new ActuatorDetailLine("打开反馈位", entity.OpenFeedbackBit.HasValue ? entity.OpenFeedbackBit.Value.ToString() : "-"),
                    new ActuatorDetailLine("工件检测位", entity.WorkpiecePresentBit.HasValue ? entity.WorkpiecePresentBit.Value.ToString() : "-"),
                    new ActuatorDetailLine("反馈校验", entity.UseFeedbackCheck ? "是" : "否"),
                    new ActuatorDetailLine("工件检测校验", entity.UseWorkpieceCheck ? "是" : "否"),
                    new ActuatorDetailLine("夹紧超时(ms)", entity.CloseTimeoutMs.ToString()),
                    new ActuatorDetailLine("打开超时(ms)", entity.OpenTimeoutMs.ToString()),
                    new ActuatorDetailLine("允许双OFF", entity.AllowBothOff ? "是" : "否"),
                    new ActuatorDetailLine("允许双ON", entity.AllowBothOn ? "是" : "否"),
                    new ActuatorDetailLine("启用", entity.IsEnabled ? "是" : "否"),
                    new ActuatorDetailLine("排序号", entity.SortOrder.ToString()),
                    new ActuatorDetailLine("描述", string.IsNullOrWhiteSpace(entity.Description) ? "-" : entity.Description),
                    new ActuatorDetailLine("备注", string.IsNullOrWhiteSpace(entity.Remark) ? "-" : entity.Remark)
                },
                SourceEntity = entity
            };
        }

        public sealed class ActuatorCategoryItem
        {
            public ActuatorCategoryItem(string key, string displayName)
            {
                Key = key;
                DisplayName = displayName;
            }

            public string Key { get; private set; }

            public string DisplayName { get; private set; }
        }

        public sealed class ActuatorDetailLine
        {
            public ActuatorDetailLine(string title, string value)
            {
                Title = title;
                Value = string.IsNullOrWhiteSpace(value) ? "-" : value;
            }

            public string Title { get; private set; }

            public string Value { get; private set; }
        }

        public sealed class ActuatorViewItem
        {
            public int Id { get; set; }

            public string ActuatorType { get; set; }

            public string TypeDisplayName { get; set; }

            public string Name { get; set; }

            public string DisplayName { get; set; }

            public bool IsEnabled { get; set; }

            public int SortOrder { get; set; }

            public string Description { get; set; }

            public string Remark { get; set; }

            public string Summary1 { get; set; }

            public string Summary2 { get; set; }

            public string Summary3 { get; set; }

            public IList<ActuatorDetailLine> DetailLines { get; set; }

            public object SourceEntity { get; set; }

            public string DisplayTitle
            {
                get
                {
                    return string.IsNullOrWhiteSpace(DisplayName) ? Name : DisplayName;
                }
            }

            public string StatusText
            {
                get { return IsEnabled ? "已启用" : "已禁用"; }
            }
        }
    }
}