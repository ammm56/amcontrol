using AM.Core.Context;
using AM.Model.Entity.Motion;
using AMControlWinF.Tools;
using AntdUI;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace AMControlWinF.Views.MotionConfig
{
    /// <summary>
    /// 轴参数新增/编辑对话框。
    /// </summary>
    public partial class MotionAxisParamEditDialog : AntdUI.Window
    {
        private static readonly ParamGroupItem[] ParamGroups = new[]
        {
            new ParamGroupItem("Hardware", "硬件信号"),
            new ParamGroupItem("Scale", "单位换算"),
            new ParamGroupItem("Motion", "运动参数"),
            new ParamGroupItem("Home", "回零参数"),
            new ParamGroupItem("SoftLimit", "软件限位"),
            new ParamGroupItem("Timing", "使能时序"),
            new ParamGroupItem("Safety", "安全联动")
        };

        private static readonly ParamValueTypeItem[] ParamValueTypes = new[]
        {
            new ParamValueTypeItem("Bool", "Bool（布尔）"),
            new ParamValueTypeItem("Int16", "Int16（短整型）"),
            new ParamValueTypeItem("Int32", "Int32（整型）"),
            new ParamValueTypeItem("Double", "Double（小数）"),
            new ParamValueTypeItem("Enum", "Enum（枚举值）")
        };

        private readonly bool _isAdd;
        private readonly MotionAxisConfigEntity _sourceEntity;

        public MotionAxisParamEditDialog(MotionAxisConfigEntity entity, bool isAdd)
        {
            InitializeComponent();

            _sourceEntity = entity ?? new MotionAxisConfigEntity();
            _isAdd = isAdd;

            InitializeGroupDropdown();
            InitializeValueTypeDropdown();
            BindEvents();
            ApplyThemeFromConfig();
            ApplyMode();
            LoadEntity();
        }

        public MotionAxisConfigEntity ResultEntity { get; private set; }

        private void InitializeGroupDropdown()
        {
            dropdownParamGroup.Items.Clear();
            dropdownParamGroup.Items.AddRange(ParamGroups.Select(x => (object)x.DisplayName).ToArray());

            if (ParamGroups.Length > 0)
            {
                dropdownParamGroup.SelectedValue = ParamGroups[0].DisplayName;
            }
        }

        private void InitializeValueTypeDropdown()
        {
            dropdownParamValueType.Items.Clear();
            dropdownParamValueType.Items.AddRange(ParamValueTypes.Select(x => (object)x.DisplayName).ToArray());

            if (ParamValueTypes.Length > 0)
            {
                dropdownParamValueType.SelectedValue = ParamValueTypes[0].DisplayName;
            }
        }

        private void BindEvents()
        {
            buttonOk.Click += ButtonOk_Click;
            buttonCancel.Click += ButtonCancel_Click;
            Shown += MotionAxisParamEditDialog_Shown;

            KeyPreview = true;
            KeyDown += MotionAxisParamEditDialog_KeyDown;
        }

        private void MotionAxisParamEditDialog_Shown(object sender, EventArgs e)
        {
            if (_isAdd)
            {
                inputParamName.Focus();
                inputParamName.SelectAll();
            }
            else
            {
                inputParamDisplayName.Focus();
                inputParamDisplayName.SelectAll();
            }
        }

        private void ApplyMode()
        {
            Text = _isAdd ? "新增轴参数" : "编辑轴参数";
            labelDialogTitle.Text = _isAdd ? "新增轴参数" : "编辑轴参数";
            labelDialogDescription.Text = _isAdd
                ? "填写轴参数配置"
                : "修改轴参数配置";

            buttonOk.Text = "保存";
        }

        private void LoadEntity()
        {
            inputLogicalAxis.Text = _sourceEntity.LogicalAxis.ToString();
            inputLogicalAxis.Enabled = false;

            inputParamName.Text = _sourceEntity.ParamName ?? string.Empty;
            inputParamName.Enabled = _isAdd;

            inputParamDisplayName.Text = _sourceEntity.ParamDisplayName ?? string.Empty;

            var selectedGroup = ParamGroups.FirstOrDefault(x =>
                string.Equals(x.Value, _sourceEntity.ParamGroup ?? "Motion", StringComparison.OrdinalIgnoreCase));
            dropdownParamGroup.SelectedValue = selectedGroup == null
                ? ParamGroups[0].DisplayName
                : selectedGroup.DisplayName;

            var selectedValueType = ParamValueTypes.FirstOrDefault(x =>
                string.Equals(x.Value, _sourceEntity.ParamValueType ?? "Double", StringComparison.OrdinalIgnoreCase));
            dropdownParamValueType.SelectedValue = selectedValueType == null
                ? ParamValueTypes[0].DisplayName
                : selectedValueType.DisplayName;

            if (!_isAdd)
            {
                dropdownParamValueType.Enabled = false;
            }

            inputParamSetValue.Text = FormatValue(_sourceEntity.ParamSetValue, _sourceEntity.ParamValueType);
            inputParamDefaultValue.Text = FormatValue(_sourceEntity.ParamDefaultValue, _sourceEntity.ParamValueType);
            inputParamMinValue.Text = FormatValue(_sourceEntity.ParamMinValue, _sourceEntity.ParamValueType);
            inputParamMaxValue.Text = FormatValue(_sourceEntity.ParamMaxValue, _sourceEntity.ParamValueType);

            inputUnit.Text = _sourceEntity.Unit ?? string.Empty;
            inputVendorScope.Text = string.IsNullOrWhiteSpace(_sourceEntity.VendorScope) ? "All" : _sourceEntity.VendorScope;
            inputValueDescription.Text = _sourceEntity.ValueDescription ?? string.Empty;
            inputDescription.Text = _sourceEntity.Description ?? string.Empty;
            inputRemark.Text = _sourceEntity.Remark ?? string.Empty;
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            MotionAxisConfigEntity entity;
            if (!TryBuildEntity(out entity))
                return;

            ResultEntity = entity;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void MotionAxisParamEditDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            if (e.KeyCode == Keys.Enter && !(ActiveControl is TextBoxBase))
            {
                e.SuppressKeyPress = true;
                ButtonOk_Click(sender, EventArgs.Empty);
            }
        }

        private bool TryBuildEntity(out MotionAxisConfigEntity entity)
        {
            entity = null;

            short logicalAxis;
            if (!short.TryParse((inputLogicalAxis.Text ?? string.Empty).Trim(), out logicalAxis) || logicalAxis <= 0)
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "逻辑轴号无效。");
                inputLogicalAxis.Focus();
                return false;
            }

            var paramName = (inputParamName.Text ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(paramName))
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "参数名不能为空。");
                inputParamName.Focus();
                return false;
            }

            var paramDisplayName = (inputParamDisplayName.Text ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(paramDisplayName))
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "参数显示名称不能为空。");
                inputParamDisplayName.Focus();
                return false;
            }

            var selectedGroupText = dropdownParamGroup.SelectedValue == null
                ? string.Empty
                : dropdownParamGroup.SelectedValue.ToString();
            var group = ParamGroups.FirstOrDefault(x =>
                string.Equals(x.DisplayName, selectedGroupText, StringComparison.OrdinalIgnoreCase));
            if (group == null)
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "请选择参数分组。");
                dropdownParamGroup.Focus();
                return false;
            }

            var selectedTypeText = dropdownParamValueType.SelectedValue == null
                ? string.Empty
                : dropdownParamValueType.SelectedValue.ToString();
            var valueType = ParamValueTypes.FirstOrDefault(x =>
                string.Equals(x.DisplayName, selectedTypeText, StringComparison.OrdinalIgnoreCase));
            if (valueType == null)
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "请选择参数值类型。");
                dropdownParamValueType.Focus();
                return false;
            }

            double paramSetValue;
            string errorMessage;
            if (!TryParseByType(inputParamSetValue.Text, valueType.Value, out paramSetValue, out errorMessage))
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "当前值无效：" + errorMessage);
                inputParamSetValue.Focus();
                return false;
            }

            double paramDefaultValue;
            if (!TryParseByType(inputParamDefaultValue.Text, valueType.Value, out paramDefaultValue, out errorMessage))
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "默认值无效：" + errorMessage);
                inputParamDefaultValue.Focus();
                return false;
            }

            double paramMinValue;
            if (!TryParseDouble(inputParamMinValue.Text, out paramMinValue))
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "最小值必须为有效数值。");
                inputParamMinValue.Focus();
                return false;
            }

            double paramMaxValue;
            if (!TryParseDouble(inputParamMaxValue.Text, out paramMaxValue))
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "最大值必须为有效数值。");
                inputParamMaxValue.Focus();
                return false;
            }

            if (paramMinValue < paramMaxValue && (paramSetValue < paramMinValue || paramSetValue > paramMaxValue))
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "当前值超出最小值/最大值范围。");
                inputParamSetValue.Focus();
                return false;
            }

            if (paramMinValue < paramMaxValue && (paramDefaultValue < paramMinValue || paramDefaultValue > paramMaxValue))
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "默认值超出最小值/最大值范围。");
                inputParamDefaultValue.Focus();
                return false;
            }

            entity = new MotionAxisConfigEntity
            {
                Id = _sourceEntity.Id,
                LogicalAxis = logicalAxis,
                ParamName = paramName,
                ParamDisplayName = paramDisplayName,
                ParamGroup = group.Value,
                ParamValueType = valueType.Value,
                ParamSetValue = paramSetValue,
                ParamDefaultValue = paramDefaultValue,
                ParamMinValue = paramMinValue,
                ParamMaxValue = paramMaxValue,
                Unit = NormalizeNullable(inputUnit.Text),
                Description = NormalizeNullable(inputDescription.Text),
                ValueDescription = NormalizeNullable(inputValueDescription.Text),
                VendorScope = NormalizeVendorScope(inputVendorScope.Text),
                Remark = NormalizeNullable(inputRemark.Text),
                AxisDisplayName = _sourceEntity.AxisDisplayName
            };

            return true;
        }

        private static bool TryParseByType(string text, string valueType, out double value, out string errorMessage)
        {
            errorMessage = string.Empty;
            value = 0D;

            var raw = (text ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(raw))
            {
                errorMessage = "不能为空";
                return false;
            }

            if (string.Equals(valueType, "Bool", StringComparison.OrdinalIgnoreCase))
            {
                if (string.Equals(raw, "true", StringComparison.OrdinalIgnoreCase) || raw == "1")
                {
                    value = 1D;
                    return true;
                }

                if (string.Equals(raw, "false", StringComparison.OrdinalIgnoreCase) || raw == "0")
                {
                    value = 0D;
                    return true;
                }

                errorMessage = "布尔参数只能输入 0、1、true、false";
                return false;
            }

            if (!double.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
            {
                if (!double.TryParse(raw, NumberStyles.Any, CultureInfo.CurrentCulture, out value))
                {
                    errorMessage = "请输入有效数值";
                    return false;
                }
            }

            if (string.Equals(valueType, "Int16", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(valueType, "Int32", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(valueType, "Enum", StringComparison.OrdinalIgnoreCase))
            {
                if (Math.Abs(value - Math.Round(value)) > 1e-9)
                {
                    errorMessage = "该类型只允许整数";
                    return false;
                }
            }

            return true;
        }

        private static bool TryParseDouble(string text, out double value)
        {
            var raw = (text ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(raw))
            {
                value = 0D;
                return true;
            }

            if (double.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                return true;

            return double.TryParse(raw, NumberStyles.Any, CultureInfo.CurrentCulture, out value);
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

        private static string NormalizeNullable(string value)
        {
            var text = value == null ? string.Empty : value.Trim();
            return string.IsNullOrWhiteSpace(text) ? null : text;
        }

        private static string NormalizeVendorScope(string value)
        {
            var text = NormalizeNullable(value);
            return string.IsNullOrWhiteSpace(text) ? "All" : text;
        }

        private void ApplyThemeFromConfig()
        {
            var theme = ConfigContext.Instance.Config.Setting.Theme;
            var isDarkMode = !string.IsNullOrWhiteSpace(theme) &&
                             (string.Equals(theme, "SkinDark", StringComparison.OrdinalIgnoreCase) ||
                              string.Equals(theme, "Dark", StringComparison.OrdinalIgnoreCase));

            if (isDarkMode)
            {
                AntdUI.Config.IsDark = true;
            }
            else
            {
                AntdUI.Config.IsLight = true;
            }

            textureBackgroundDialog.SetTheme(isDarkMode);
        }

        private sealed class ParamGroupItem
        {
            public ParamGroupItem(string value, string displayName)
            {
                Value = value;
                DisplayName = displayName;
            }

            public string Value { get; private set; }

            public string DisplayName { get; private set; }
        }

        private sealed class ParamValueTypeItem
        {
            public ParamValueTypeItem(string value, string displayName)
            {
                Value = value;
                DisplayName = displayName;
            }

            public string Value { get; private set; }

            public string DisplayName { get; private set; }
        }
    }
}