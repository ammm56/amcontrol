using AM.Model.Entity.Motion;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Globalization;

namespace AM.ViewModel.ViewModels.Config
{
    public class ConfigAxisArgViewModel : ObservableObject
    {
        private string _editValue;
        private bool _isDirty;

        public int Id { get; set; }
        public short LogicalAxis { get; set; }
        public string ParamName { get; set; }
        public string ParamDisplayName { get; set; }
        public string ParamGroup { get; set; }
        public string ParamValueType { get; set; }
        public double ParamDefaultValue { get; set; }
        public double ParamMaxValue { get; set; }
        public double ParamMinValue { get; set; }
        public string Unit { get; set; }
        public string Description { get; set; }
        public string ValueDescription { get; set; }
        public string VendorScope { get; set; }
        public string Remark { get; set; }
        public string AxisDisplayName { get; set; }

        /// <summary>所属分组的显示名称，由 ViewModel 加载时注入。</summary>
        public string GroupDisplayName { get; set; }

        public string OriginalEditValue { get; private set; }

        public string EditValue
        {
            get { return _editValue; }
            set
            {
                if (SetProperty(ref _editValue, value))
                    IsDirty = !string.Equals(value, OriginalEditValue, StringComparison.Ordinal);
            }
        }

        public bool IsDirty
        {
            get { return _isDirty; }
            private set { SetProperty(ref _isDirty, value); }
        }

        public string ParamDefaultValueText { get { return FormatValue(ParamDefaultValue); } }

        public string RangeText
        {
            get
            {
                if (string.Equals(ParamValueType, "Bool", StringComparison.OrdinalIgnoreCase)) return "0 / 1";
                if (ParamMinValue == 0D && ParamMaxValue == 0D) return "—";
                return FormatValue(ParamMinValue) + " ~ " + FormatValue(ParamMaxValue);
            }
        }

        public string TypeLabel
        {
            get
            {
                var t = ParamValueType ?? "Double";
                if (string.Equals(t, "Bool", StringComparison.OrdinalIgnoreCase)) return "布尔";
                if (string.Equals(t, "Int16", StringComparison.OrdinalIgnoreCase)) return "整数";
                if (string.Equals(t, "Int32", StringComparison.OrdinalIgnoreCase)) return "整数";
                if (string.Equals(t, "Enum", StringComparison.OrdinalIgnoreCase)) return "枚举";
                return "小数";
            }
        }

        /// <summary>单位显示值，Unit 为空时返回"未定义"。</summary>
        public string UnitDisplay { get { return string.IsNullOrWhiteSpace(Unit) ? "未定义" : Unit; } }

        public static ConfigAxisArgViewModel FromEntity(MotionAxisConfigEntity entity)
        {
            if (entity == null) return null;

            var vm = new ConfigAxisArgViewModel
            {
                Id = entity.Id,
                LogicalAxis = entity.LogicalAxis,
                ParamName = entity.ParamName,
                ParamDisplayName = entity.ParamDisplayName ?? entity.ParamName,
                ParamGroup = entity.ParamGroup ?? "Motion",
                ParamValueType = entity.ParamValueType ?? "Double",
                ParamDefaultValue = entity.ParamDefaultValue,
                ParamMaxValue = entity.ParamMaxValue,
                ParamMinValue = entity.ParamMinValue,
                Unit = entity.Unit ?? string.Empty,
                Description = entity.Description ?? string.Empty,
                ValueDescription = entity.ValueDescription ?? string.Empty,
                VendorScope = entity.VendorScope ?? "All",
                Remark = entity.Remark ?? string.Empty,
                AxisDisplayName = entity.AxisDisplayName ?? string.Empty,
            };

            vm._editValue = vm.FormatValue(entity.ParamSetValue);
            vm.OriginalEditValue = vm._editValue;
            vm._isDirty = false;
            return vm;
        }

        public MotionAxisConfigEntity ToEntity()
        {
            double setValue;
            TryGetSetValue(out setValue);

            return new MotionAxisConfigEntity
            {
                Id = Id,
                LogicalAxis = LogicalAxis,
                ParamName = ParamName,
                ParamDisplayName = ParamDisplayName,
                ParamGroup = ParamGroup,
                ParamValueType = ParamValueType,
                ParamSetValue = setValue,
                ParamDefaultValue = ParamDefaultValue,
                ParamMaxValue = ParamMaxValue,
                ParamMinValue = ParamMinValue,
                Unit = Unit,
                Description = Description,
                ValueDescription = ValueDescription,
                VendorScope = VendorScope,
                Remark = Remark,
                AxisDisplayName = AxisDisplayName,
            };
        }

        public bool TryGetSetValue(out double value)
        {
            var text = (_editValue ?? string.Empty).Trim();
            if (string.Equals(text, "true", StringComparison.OrdinalIgnoreCase)) { value = 1.0; return true; }
            if (string.Equals(text, "false", StringComparison.OrdinalIgnoreCase)) { value = 0.0; return true; }
            return double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out value);
        }

        public bool ValidateInputValue(string inputText, out double parsedValue, out string errorMessage)
        {
            parsedValue = 0;
            errorMessage = string.Empty;

            var text = (inputText ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(text)) { errorMessage = "参数值不能为空"; return false; }

            if (string.Equals(text, "true", StringComparison.OrdinalIgnoreCase)) text = "1";
            if (string.Equals(text, "false", StringComparison.OrdinalIgnoreCase)) text = "0";

            double v;
            if (!double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out v))
            { errorMessage = "请输入有效的数值"; return false; }

            var t = ParamValueType ?? "Double";
            if (string.Equals(t, "Bool", StringComparison.OrdinalIgnoreCase) && v != 0 && v != 1)
            { errorMessage = "布尔参数只能输入 0 或 1"; return false; }

            if (string.Equals(t, "Int16", StringComparison.OrdinalIgnoreCase)
                || string.Equals(t, "Int32", StringComparison.OrdinalIgnoreCase)
                || string.Equals(t, "Enum", StringComparison.OrdinalIgnoreCase))
            {
                if (Math.Abs(v - Math.Round(v)) > 1e-9)
                { errorMessage = "整数参数不能包含小数部分"; return false; }
            }

            if (ParamMinValue < ParamMaxValue && (v < ParamMinValue || v > ParamMaxValue))
            {
                errorMessage = string.Format("超出允许范围 [{0} ~ {1}]",
                    FormatValue(ParamMinValue), FormatValue(ParamMaxValue));
                return false;
            }

            parsedValue = v;
            return true;
        }

        public void CommitSave()
        {
            OriginalEditValue = EditValue;
            IsDirty = false;
        }

        public void ResetToDefault()
        {
            EditValue = FormatValue(ParamDefaultValue);
        }

        internal string FormatValue(double value)
        {
            var t = ParamValueType ?? "Double";
            if (string.Equals(t, "Bool", StringComparison.OrdinalIgnoreCase))
                return value >= 0.5 ? "1" : "0";
            if (string.Equals(t, "Int16", StringComparison.OrdinalIgnoreCase)
                || string.Equals(t, "Int32", StringComparison.OrdinalIgnoreCase)
                || string.Equals(t, "Enum", StringComparison.OrdinalIgnoreCase))
                return ((long)Math.Round(value)).ToString();
            return value.ToString("G6", CultureInfo.InvariantCulture);
        }
    }
}