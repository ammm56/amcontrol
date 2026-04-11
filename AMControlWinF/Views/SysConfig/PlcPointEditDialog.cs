using AM.Core.Context;
using AM.Model.Entity.Plc;
using AM.PageModel.SysConfig;
using AMControlWinF.Tools;
using AntdUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AMControlWinF.Views.SysConfig
{
    /// <summary>
    /// PLC 点位新增/编辑对话框。
    /// 采用运控配置统一风格：
    /// - 纹理背景
    /// - 顶部标题说明
    /// - 中间横向四列参数区
    /// - 底部固定按钮区
    /// </summary>
    public partial class PlcPointEditDialog : AntdUI.Window
    {
        private readonly PlcPointEditorModel _model;
        private bool _isCreateMode = true;

        public PlcPointEditDialog()
        {
            InitializeComponent();

            _model = new PlcPointEditorModel();

            InitializeDropdowns();
            BindEvents();
            ApplyThemeFromConfig();
            ApplyMode();
        }

        public bool IsCreateMode
        {
            get { return _isCreateMode; }
            set
            {
                _isCreateMode = value;
                ApplyMode();
            }
        }

        public PlcPointConfigEntity ResultEntity { get; private set; }

        public void SetStationNames(IList<string> stationNames)
        {
            var selected = GetSelectValue(dropdownPlcName);

            dropdownPlcName.Items.Clear();
            if (stationNames != null)
            {
                dropdownPlcName.Items.AddRange(
                    stationNames
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .Select(x => (object)x)
                        .ToArray());
            }

            SetSelectValue(dropdownPlcName, selected);
        }

        public void SetEntity(PlcPointConfigEntity entity)
        {
            if (entity == null)
            {
                _model.ResetForCreate();
            }
            else
            {
                _model.LoadFrom(entity);
            }

            ApplyModelToUi();
        }

        private void InitializeDropdowns()
        {
            dropdownDataType.Items.Clear();
            dropdownDataType.Items.AddRange(
                PlcPointEditorModel.DataTypes
                    .Select(x => (object)x)
                    .ToArray());

            dropdownAccessMode.Items.Clear();
            dropdownAccessMode.Items.AddRange(
                PlcPointEditorModel.AccessModes
                    .Select(x => (object)x)
                    .ToArray());
        }

        private void BindEvents()
        {
            buttonOk.Click += ButtonOk_Click;
            buttonCancel.Click += ButtonCancel_Click;

            Shown += PlcPointEditDialog_Shown;

            KeyPreview = true;
            KeyDown += PlcPointEditDialog_KeyDown;
        }

        private void PlcPointEditDialog_Shown(object sender, EventArgs e)
        {
            if (_isCreateMode)
            {
                dropdownPlcName.Focus();
            }
            else
            {
                inputName.Focus();
                inputName.SelectAll();
            }
        }

        private void ApplyMode()
        {
            Text = _isCreateMode ? "新增 PLC 点位" : "编辑 PLC 点位";
            labelDialogTitle.Text = _isCreateMode ? "新增 PLC 点位" : "编辑 PLC 点位";
            labelDialogDescription.Text = _isCreateMode
                ? "配置点位地址、数据类型、长度与访问模式。"
                : "修改点位地址、数据类型、长度与访问模式。";

            buttonOk.Text = "保存";
        }

        private void ApplyModelToUi()
        {
            SetSelectValue(dropdownPlcName, _model.PlcName);
            inputName.Text = _model.Name;
            inputDisplayName.Text = _model.DisplayName;
            inputGroupName.Text = _model.GroupName;
            inputAddress.Text = _model.Address;
            SetSelectValue(dropdownDataType, _model.DataType);
            inputLength.Text = _model.LengthText;
            SetSelectValue(dropdownAccessMode, _model.AccessMode);
            inputSortOrder.Text = _model.SortOrderText;
            checkEnabled.Checked = _model.IsEnabled;
            inputDescription.Text = _model.Description;
            inputRemark.Text = _model.Remark;
        }

        private void ApplyUiToModel()
        {
            _model.PlcName = GetSelectValue(dropdownPlcName);
            _model.Name = inputName.Text;
            _model.DisplayName = inputDisplayName.Text;
            _model.GroupName = inputGroupName.Text;
            _model.Address = inputAddress.Text;
            _model.DataType = GetSelectValue(dropdownDataType);
            _model.LengthText = inputLength.Text;
            _model.AccessMode = GetSelectValue(dropdownAccessMode);
            _model.SortOrderText = inputSortOrder.Text;
            _model.IsEnabled = checkEnabled.Checked;
            _model.Description = inputDescription.Text;
            _model.Remark = inputRemark.Text;
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            ApplyUiToModel();

            var result = _model.BuildEntity();
            if (!result.Success || result.Item == null)
            {
                PageDialogHelper.ShowWarn(this, "输入提示", result.Message);
                return;
            }

            ResultEntity = result.Item;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void PlcPointEditDialog_KeyDown(object sender, KeyEventArgs e)
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

        private void ApplyThemeFromConfig()
        {
            var theme = ConfigContext.Instance.Config.Setting.Theme;
            var isDarkMode = IsDarkTheme(theme);

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

        private static bool IsDarkTheme(string theme)
        {
            if (string.IsNullOrWhiteSpace(theme))
            {
                return false;
            }

            return string.Equals(theme, "SkinDark", StringComparison.OrdinalIgnoreCase)
                || string.Equals(theme, "Dark", StringComparison.OrdinalIgnoreCase);
        }

        private static void SetSelectValue(Select select, string value)
        {
            if (select == null)
            {
                return;
            }

            var text = string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();

            if (!string.IsNullOrWhiteSpace(text))
            {
                bool exists = select.Items.Cast<object>()
                    .Any(x => string.Equals(x == null ? string.Empty : x.ToString(), text, StringComparison.OrdinalIgnoreCase));

                if (!exists)
                {
                    select.Items.Add(text);
                }
            }

            select.SelectedValue = text;
        }

        private static string GetSelectValue(Select select)
        {
            if (select == null || select.SelectedValue == null)
            {
                return string.Empty;
            }

            return select.SelectedValue.ToString();
        }
    }
}