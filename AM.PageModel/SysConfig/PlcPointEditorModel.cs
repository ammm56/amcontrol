using AM.Model.Common;
using AM.Model.Entity.Plc;
using AM.PageModel.Common;
using System;
using System.Collections.Generic;

namespace AM.PageModel.SysConfig
{
    /// <summary>
    /// PLC 点位编辑模型。
    /// 负责页面/对话框点位编辑态字段承载、默认值填充、输入校验与实体构建。
    /// </summary>
    public class PlcPointEditorModel : BindableBase
    {
        private int _id;
        private string _plcName;
        private string _name;
        private string _displayName;
        private string _groupName;
        private string _address;
        private string _dataType;
        private string _lengthText;
        private string _accessMode;
        private bool _isEnabled;
        private string _sortOrderText;
        private string _description;
        private string _remark;

        public PlcPointEditorModel()
        {
            ResetForCreate();
        }

        public static IReadOnlyList<string> DataTypes
        {
            get
            {
                return new[]
                {
                    "bool",
                    "uint8",
                    "int8",
                    "uint16",
                    "int16",
                    "uint32",
                    "int32",
                    "uint64",
                    "int64",
                    "float",
                    "double",
                    "string",
                    "bool[]",
                    "uint8[]",
                    "int8[]",
                    "uint16[]",
                    "int16[]",
                    "uint32[]",
                    "int32[]",
                    "uint64[]",
                    "int64[]",
                    "float[]",
                    "double[]"
                };
            }
        }

        public static IReadOnlyList<string> AccessModes
        {
            get
            {
                return new[]
                {
                    "ReadOnly",
                    "ReadWrite",
                    "WriteOnly"
                };
            }
        }

        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        public string PlcName
        {
            get { return _plcName; }
            set { SetProperty(ref _plcName, value ?? string.Empty); }
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value ?? string.Empty); }
        }

        public string DisplayName
        {
            get { return _displayName; }
            set { SetProperty(ref _displayName, value ?? string.Empty); }
        }

        public string GroupName
        {
            get { return _groupName; }
            set { SetProperty(ref _groupName, value ?? string.Empty); }
        }

        public string Address
        {
            get { return _address; }
            set { SetProperty(ref _address, value ?? string.Empty); }
        }

        public string DataType
        {
            get { return _dataType; }
            set { SetProperty(ref _dataType, value ?? string.Empty); }
        }

        public string LengthText
        {
            get { return _lengthText; }
            set { SetProperty(ref _lengthText, value ?? string.Empty); }
        }

        public string AccessMode
        {
            get { return _accessMode; }
            set { SetProperty(ref _accessMode, value ?? string.Empty); }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { SetProperty(ref _isEnabled, value); }
        }

        public string SortOrderText
        {
            get { return _sortOrderText; }
            set { SetProperty(ref _sortOrderText, value ?? string.Empty); }
        }

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value ?? string.Empty); }
        }

        public string Remark
        {
            get { return _remark; }
            set { SetProperty(ref _remark, value ?? string.Empty); }
        }

        public void ResetForCreate()
        {
            Id = 0;
            PlcName = string.Empty;
            Name = string.Empty;
            DisplayName = string.Empty;
            GroupName = "Default";
            Address = string.Empty;
            DataType = "bool";
            LengthText = "1";
            AccessMode = "ReadWrite";
            IsEnabled = true;
            SortOrderText = "1";
            Description = string.Empty;
            Remark = string.Empty;
        }

        public void LoadFrom(PlcPointConfigEntity entity)
        {
            if (entity == null)
            {
                ResetForCreate();
                return;
            }

            Id = entity.Id;
            PlcName = entity.PlcName ?? string.Empty;
            Name = entity.Name ?? string.Empty;
            DisplayName = entity.DisplayName ?? string.Empty;
            GroupName = entity.GroupName ?? string.Empty;
            Address = entity.Address ?? string.Empty;
            DataType = entity.DataType ?? string.Empty;
            LengthText = entity.Length.ToString();
            AccessMode = entity.AccessMode ?? string.Empty;
            IsEnabled = entity.IsEnabled;
            SortOrderText = entity.SortOrder.ToString();
            Description = entity.Description ?? string.Empty;
            Remark = entity.Remark ?? string.Empty;
        }

        public Result<PlcPointConfigEntity> BuildEntity()
        {
            PlcName = NormalizeText(PlcName);
            Name = NormalizeText(Name);
            DisplayName = NormalizeText(DisplayName);
            GroupName = NormalizeText(GroupName);
            Address = NormalizeText(Address);
            DataType = NormalizeDataType(DataType);
            AccessMode = NormalizeText(AccessMode);
            Description = NormalizeText(Description);
            Remark = NormalizeText(Remark);

            if (string.IsNullOrWhiteSpace(PlcName))
            {
                return Result<PlcPointConfigEntity>.Fail(-4201, "所属 PLC 名称不能为空", ResultSource.UI);
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                return Result<PlcPointConfigEntity>.Fail(-4202, "点位名称不能为空", ResultSource.UI);
            }

            if (string.IsNullOrWhiteSpace(Address))
            {
                return Result<PlcPointConfigEntity>.Fail(-4203, "点位地址不能为空", ResultSource.UI);
            }

            if (string.IsNullOrWhiteSpace(DataType))
            {
                return Result<PlcPointConfigEntity>.Fail(-4204, "数据类型不能为空", ResultSource.UI);
            }

            if (string.IsNullOrWhiteSpace(AccessMode))
            {
                return Result<PlcPointConfigEntity>.Fail(-4205, "访问模式不能为空", ResultSource.UI);
            }

            int length;
            if (!TryParsePositiveInt(LengthText, out length))
            {
                return Result<PlcPointConfigEntity>.Fail(-4206, "Length 必须为大于 0 的整数", ResultSource.UI);
            }

            int sortOrder;
            if (!TryParseNonNegativeInt(SortOrderText, out sortOrder))
            {
                return Result<PlcPointConfigEntity>.Fail(-4207, "排序号必须为大于等于 0 的整数", ResultSource.UI);
            }

            if (!IsSupportedDataType(DataType))
            {
                return Result<PlcPointConfigEntity>.Fail(-4208, "不支持的数据类型: " + DataType, ResultSource.UI);
            }

            if (!IsSupportedAccessMode(AccessMode))
            {
                return Result<PlcPointConfigEntity>.Fail(-4209, "不支持的访问模式: " + AccessMode, ResultSource.UI);
            }

            if (string.IsNullOrWhiteSpace(DisplayName))
            {
                DisplayName = Name;
            }

            var entity = new PlcPointConfigEntity
            {
                Id = Id,
                PlcName = PlcName,
                Name = Name,
                DisplayName = DisplayName,
                GroupName = GroupName,
                Address = Address,
                DataType = DataType,
                Length = length,
                AccessMode = AccessMode,
                IsEnabled = IsEnabled,
                SortOrder = sortOrder,
                Description = Description,
                Remark = Remark
            };

            return Result<PlcPointConfigEntity>.OkItem(entity, "PLC 点位编辑模型构建成功", ResultSource.UI)
                .WithNotifyMode(ResultNotifyMode.Silent);
        }

        private static string NormalizeText(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
        }

        private static string NormalizeDataType(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? string.Empty
                : value.Trim().Replace(" ", string.Empty).ToLowerInvariant();
        }

        private static bool TryParsePositiveInt(string text, out int value)
        {
            if (int.TryParse((text ?? string.Empty).Trim(), out value))
            {
                return value > 0;
            }

            return false;
        }

        private static bool TryParseNonNegativeInt(string text, out int value)
        {
            if (int.TryParse((text ?? string.Empty).Trim(), out value))
            {
                return value >= 0;
            }

            return false;
        }

        private static bool IsSupportedDataType(string dataType)
        {
            foreach (var item in DataTypes)
            {
                if (string.Equals(item, dataType, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsSupportedAccessMode(string accessMode)
        {
            foreach (var item in AccessModes)
            {
                if (string.Equals(item, accessMode, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}