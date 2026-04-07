using AM.Model.Entity.Motion.Actuator;
using AM.Model.Entity.Motion.Topology;
using AMControlWinF.Tools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace AMControlWinF.Views.MotionConfig
{
    public partial class CylinderEditControl : UserControl
    {
        private static readonly DriveModeItem[] DriveModes = new[]
        {
            new DriveModeItem("Single", "Single（单线圈）"),
            new DriveModeItem("Double", "Double（双线圈）")
        };

        private CylinderConfigEntity _sourceEntity;
        private bool _isAdd;
        private List<IoOptionItem> _doRequiredOptions;
        private List<IoOptionItem> _doOptionalOptions;
        private List<IoOptionItem> _diOptionalOptions;

        public CylinderEditControl()
        {
            InitializeComponent();

            _sourceEntity = new CylinderConfigEntity();
            _doRequiredOptions = new List<IoOptionItem>();
            _doOptionalOptions = new List<IoOptionItem>();
            _diOptionalOptions = new List<IoOptionItem>();

            InitializeDriveModeDropdown();
            BindEvents();
        }

        public void Bind(CylinderConfigEntity entity, bool isAdd, IList<MotionIoMapEntity> allIoItems)
        {
            _sourceEntity = entity ?? new CylinderConfigEntity
            {
                IsEnabled = true,
                DriveMode = "Double",
                ExtendTimeoutMs = 3000,
                RetractTimeoutMs = 3000
            };
            _isAdd = isAdd;

            LoadIoOptions(allIoItems ?? new List<MotionIoMapEntity>());
            LoadEntity();
        }

        public bool TryBuildEntity(out CylinderConfigEntity entity)
        {
            entity = null;

            var name = (inputName.Text ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "对象名称不能为空。");
                inputName.Focus();
                return false;
            }

            short? extendBit = GetSelectedIoBit(dropdownExtendOutputBit);
            if (!extendBit.HasValue)
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "请选择伸出输出位。");
                dropdownExtendOutputBit.Focus();
                return false;
            }

            var driveMode = GetSelectedDriveMode();
            short? retractBit = GetSelectedIoBit(dropdownRetractOutputBit);
            if (string.Equals(driveMode, "Double", StringComparison.OrdinalIgnoreCase) && !retractBit.HasValue)
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "双线圈气缸必须配置缩回输出位。");
                dropdownRetractOutputBit.Focus();
                return false;
            }

            int extendTimeout;
            if (!TryParseNonNegativeInt(inputExtendTimeoutMs.Text, out extendTimeout))
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "伸出超时必须为非负整数。");
                inputExtendTimeoutMs.Focus();
                return false;
            }

            int retractTimeout;
            if (!TryParseNonNegativeInt(inputRetractTimeoutMs.Text, out retractTimeout))
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "缩回超时必须为非负整数。");
                inputRetractTimeoutMs.Focus();
                return false;
            }

            int sortOrder;
            if (!TryParseNonNegativeInt(inputSortOrder.Text, out sortOrder))
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "排序号必须为非负整数。");
                inputSortOrder.Focus();
                return false;
            }

            entity = new CylinderConfigEntity
            {
                Id = _sourceEntity.Id,
                Name = name,
                DisplayName = NormalizeNullable(inputDisplayName.Text),
                DriveMode = driveMode,
                ExtendOutputBit = extendBit.Value,
                RetractOutputBit = retractBit,
                ExtendFeedbackBit = GetSelectedIoBit(dropdownExtendFeedbackBit),
                RetractFeedbackBit = GetSelectedIoBit(dropdownRetractFeedbackBit),
                UseFeedbackCheck = checkUseFeedbackCheck.Checked,
                ExtendTimeoutMs = extendTimeout,
                RetractTimeoutMs = retractTimeout,
                AlarmCodeOnExtendTimeout = _sourceEntity.AlarmCodeOnExtendTimeout,
                AlarmCodeOnRetractTimeout = _sourceEntity.AlarmCodeOnRetractTimeout,
                AllowBothOff = checkAllowBothOff.Checked,
                AllowBothOn = checkAllowBothOn.Checked,
                IsEnabled = checkIsEnabled.Checked,
                SortOrder = sortOrder,
                Description = NormalizeNullable(inputDescription.Text),
                Remark = NormalizeNullable(inputRemark.Text)
            };

            return true;
        }

        private void InitializeDriveModeDropdown()
        {
            dropdownDriveMode.Items.Clear();
            dropdownDriveMode.Items.AddRange(DriveModes.Select(x => (object)x.DisplayName).ToArray());

            if (DriveModes.Length > 0)
            {
                dropdownDriveMode.SelectedValue = DriveModes[0].DisplayName;
            }
        }

        private void BindEvents()
        {
            dropdownDriveMode.SelectedValueChanged += DropdownDriveMode_SelectedValueChanged;
        }

        private void DropdownDriveMode_SelectedValueChanged(object sender, AntdUI.ObjectNEventArgs e)
        {
            UpdateRetractOutputState();
        }

        private void LoadIoOptions(IList<MotionIoMapEntity> allIoItems)
        {
            _doRequiredOptions = BuildIoOptions(allIoItems, "DO", false);
            _doOptionalOptions = BuildIoOptions(allIoItems, "DO", true);
            _diOptionalOptions = BuildIoOptions(allIoItems, "DI", true);

            ApplySelectOptions(dropdownExtendOutputBit, _doRequiredOptions, null);
            ApplySelectOptions(dropdownRetractOutputBit, _doOptionalOptions, null);
            ApplySelectOptions(dropdownExtendFeedbackBit, _diOptionalOptions, null);
            ApplySelectOptions(dropdownRetractFeedbackBit, _diOptionalOptions, null);
        }

        private void LoadEntity()
        {
            inputName.Text = _sourceEntity.Name ?? string.Empty;
            inputName.Enabled = _isAdd;
            inputDisplayName.Text = _sourceEntity.DisplayName ?? string.Empty;

            SelectDriveMode(_sourceEntity.DriveMode);
            ApplySelectOptions(dropdownExtendOutputBit, _doRequiredOptions, _sourceEntity.ExtendOutputBit > 0 ? (short?)_sourceEntity.ExtendOutputBit : null);
            ApplySelectOptions(dropdownRetractOutputBit, _doOptionalOptions, _sourceEntity.RetractOutputBit);
            ApplySelectOptions(dropdownExtendFeedbackBit, _diOptionalOptions, _sourceEntity.ExtendFeedbackBit);
            ApplySelectOptions(dropdownRetractFeedbackBit, _diOptionalOptions, _sourceEntity.RetractFeedbackBit);

            checkUseFeedbackCheck.Checked = _sourceEntity.UseFeedbackCheck;
            inputExtendTimeoutMs.Text = _sourceEntity.ExtendTimeoutMs.ToString(CultureInfo.InvariantCulture);
            inputRetractTimeoutMs.Text = _sourceEntity.RetractTimeoutMs.ToString(CultureInfo.InvariantCulture);
            checkAllowBothOff.Checked = _sourceEntity.AllowBothOff;
            checkAllowBothOn.Checked = _sourceEntity.AllowBothOn;
            inputSortOrder.Text = _sourceEntity.SortOrder.ToString(CultureInfo.InvariantCulture);
            checkIsEnabled.Checked = _sourceEntity.IsEnabled;
            inputDescription.Text = _sourceEntity.Description ?? string.Empty;
            inputRemark.Text = _sourceEntity.Remark ?? string.Empty;

            UpdateRetractOutputState();
        }

        private void UpdateRetractOutputState()
        {
            var isSingle = string.Equals(GetSelectedDriveMode(), "Single", StringComparison.OrdinalIgnoreCase);
            dropdownRetractOutputBit.Enabled = !isSingle;

            if (isSingle && _doOptionalOptions.Count > 0)
            {
                dropdownRetractOutputBit.SelectedValue = _doOptionalOptions[0].DisplayName;
            }
        }

        private string GetSelectedDriveMode()
        {
            var selectedText = dropdownDriveMode.SelectedValue == null ? string.Empty : dropdownDriveMode.SelectedValue.ToString();
            var selected = DriveModes.FirstOrDefault(x => string.Equals(x.DisplayName, selectedText, StringComparison.OrdinalIgnoreCase));
            return selected == null ? "Double" : selected.Value;
        }

        private void SelectDriveMode(string driveMode)
        {
            var selected = DriveModes.FirstOrDefault(x => string.Equals(x.Value, driveMode, StringComparison.OrdinalIgnoreCase));
            dropdownDriveMode.SelectedValue = selected == null ? DriveModes[0].DisplayName : selected.DisplayName;
        }

        private static List<IoOptionItem> BuildIoOptions(IList<MotionIoMapEntity> source, string ioType, bool nullable)
        {
            var list = new List<IoOptionItem>();
            if (nullable)
            {
                list.Add(new IoOptionItem(null, "不配置"));
            }

            foreach (var item in source.Where(x => string.Equals(x.IoType, ioType, StringComparison.OrdinalIgnoreCase)))
            {
                list.Add(new IoOptionItem(item.LogicalBit, "L" + item.LogicalBit + " · " + (string.IsNullOrWhiteSpace(item.Name) ? "-" : item.Name)));
            }

            return list;
        }

        private static void ApplySelectOptions(AntdUI.Select select, IList<IoOptionItem> options, short? selectedBit)
        {
            select.Items.Clear();
            select.Items.AddRange(options.Select(x => (object)x.DisplayName).ToArray());
            select.Tag = options;

            var selected = options.FirstOrDefault(x => x.Bit.HasValue && selectedBit.HasValue && x.Bit.Value == selectedBit.Value);
            if (selected != null)
            {
                select.SelectedValue = selected.DisplayName;
            }
            else if (options.Count > 0)
            {
                select.SelectedValue = options[0].DisplayName;
            }
        }

        private static short? GetSelectedIoBit(AntdUI.Select select)
        {
            var options = select.Tag as IList<IoOptionItem>;
            if (options == null)
                return null;

            var selectedText = select.SelectedValue == null ? string.Empty : select.SelectedValue.ToString();
            var selected = options.FirstOrDefault(x => string.Equals(x.DisplayName, selectedText, StringComparison.OrdinalIgnoreCase));
            return selected == null ? (short?)null : selected.Bit;
        }

        private static bool TryParseNonNegativeInt(string text, out int value)
        {
            value = 0;
            if (!int.TryParse((text ?? string.Empty).Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
                return false;

            return value >= 0;
        }

        private static string NormalizeNullable(string text)
        {
            var value = text == null ? string.Empty : text.Trim();
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }

        private sealed class DriveModeItem
        {
            public DriveModeItem(string value, string displayName)
            {
                Value = value;
                DisplayName = displayName;
            }

            public string Value { get; private set; }

            public string DisplayName { get; private set; }
        }

        private sealed class IoOptionItem
        {
            public IoOptionItem(short? bit, string displayName)
            {
                Bit = bit;
                DisplayName = displayName;
            }

            public short? Bit { get; private set; }

            public string DisplayName { get; private set; }
        }
    }
}