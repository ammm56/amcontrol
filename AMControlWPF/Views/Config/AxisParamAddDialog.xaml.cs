using AM.Model.Entity.Motion;
using AM.ViewModel.ViewModels.Config;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace AMControlWPF.Views.Config
{
    public partial class AxisParamAddDialog : HandyControl.Controls.GlowWindow
    {
        private readonly AxisParamAxisItem _axis;

        /// <summary>用户确认新增后填充的实体，DialogResult=true 时有效。</summary>
        public MotionAxisConfigEntity ResultEntity { get; private set; }

        public AxisParamAddDialog(AxisParamAxisItem axis)
        {
            InitializeComponent();
            _axis = axis;

            var axisTitle = string.IsNullOrWhiteSpace(axis.DisplayName) ? axis.Name : axis.DisplayName;
            TextBlockAxisContext.Text = string.Format("— 轴 {0} · {1}", axis.LogicalAxis, axisTitle);

            Loaded += (s, e) => TextBoxParamName.Focus();
        }

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            HideError();

            // ── 参数名（代码名）校验 ──
            var paramName = (TextBoxParamName.Text ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(paramName))
            {
                ShowError("参数名不能为空，请输入英文代码名（如 MyCustomParam）");
                return;
            }
            if (!IsValidParamName(paramName))
            {
                ShowError("参数名只能包含英文字母、数字和下划线，且不能以数字开头");
                return;
            }

            // ── 显示名称校验 ──
            var displayName = (TextBoxDisplayName.Text ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(displayName))
            {
                ShowError("显示名称不能为空");
                return;
            }

            // ── 数值校验 ──
            double setValue;
            if (!TryParseValue(TextBoxSetValue.Text, out setValue))
            {
                ShowError("初始值格式不正确，请输入有效数值");
                return;
            }

            double defaultValue;
            if (!TryParseValue(TextBoxDefaultValue.Text, out defaultValue))
            {
                ShowError("默认值格式不正确，请输入有效数值");
                return;
            }

            double minValue;
            TryParseValue(TextBoxMinValue.Text, out minValue);

            double maxValue;
            TryParseValue(TextBoxMaxValue.Text, out maxValue);

            if (minValue != 0D && maxValue != 0D && minValue >= maxValue)
            {
                ShowError("最小值必须小于最大值（均填 0 表示不限范围）");
                return;
            }

            var groupItem = ComboBoxGroup.SelectedItem as ComboBoxItem;
            var typeItem = ComboBoxType.SelectedItem as ComboBoxItem;

            var paramGroup = groupItem != null && groupItem.Tag != null
                ? groupItem.Tag.ToString() : "Motion";
            var paramType = typeItem != null && typeItem.Tag != null
                ? typeItem.Tag.ToString() : "Double";

            var axisDisplay = string.IsNullOrWhiteSpace(_axis.DisplayName) ? _axis.Name : _axis.DisplayName;

            ResultEntity = new MotionAxisConfigEntity
            {
                Id = 0,
                LogicalAxis = _axis.LogicalAxis,
                AxisDisplayName = axisDisplay,
                ParamName = paramName,
                ParamDisplayName = displayName,
                ParamGroup = paramGroup,
                ParamValueType = paramType,
                ParamSetValue = setValue,
                ParamDefaultValue = defaultValue,
                ParamMinValue = minValue,
                ParamMaxValue = maxValue,
                Unit = (TextBoxUnit.Text ?? string.Empty).Trim(),
                Description = (TextBoxDescription.Text ?? string.Empty).Trim(),
            };

            DialogResult = true;
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        /// <summary>合法标识符：字母/下划线开头，仅含字母数字下划线。</summary>
        private static bool IsValidParamName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            if (char.IsDigit(name[0])) return false;
            foreach (var c in name)
            {
                if (!char.IsLetterOrDigit(c) && c != '_') return false;
            }
            return true;
        }

        private static bool TryParseValue(string text, out double value)
        {
            var t = (text ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(t)) { value = 0; return true; }
            if (string.Equals(t, "true", StringComparison.OrdinalIgnoreCase)) { value = 1; return true; }
            if (string.Equals(t, "false", StringComparison.OrdinalIgnoreCase)) { value = 0; return true; }
            return double.TryParse(t, NumberStyles.Any, CultureInfo.InvariantCulture, out value);
        }

        private void ShowError(string message)
        {
            TextBlockError.Text = message;
            BorderError.Visibility = Visibility.Visible;
        }

        private void HideError()
        {
            TextBlockError.Text = string.Empty;
            BorderError.Visibility = Visibility.Collapsed;
        }
    }
}