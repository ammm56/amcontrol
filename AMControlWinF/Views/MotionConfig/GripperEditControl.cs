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
    public partial class GripperEditControl : UserControl
    {
        private static readonly DriveModeItem[] DriveModes = new[]
        {
            new DriveModeItem("Single", "Single（单线圈）"),
            new DriveModeItem("Double", "Double（双线圈）")
        };

        private GripperConfigEntity _sourceEntity;
        private bool _isAdd;
        private List<IoOptionItem> _doRequiredOptions;
        private List<IoOptionItem> _doOptionalOptions;
        private List<IoOptionItem> _diOptionalOptions;

        public GripperEditControl()
        {
            InitializeComponent();

            _sourceEntity = new GripperConfigEntity();
            _doRequiredOptions = new List<IoOptionItem>();
            _doOptionalOptions = new List<IoOptionItem>();
            _diOptionalOptions = new List<IoOptionItem>();

            InitializeDriveModeDropdown();
            BindEvents();
        }

        public void Bind(GripperConfigEntity entity, bool isAdd, IList<MotionIoMapEntity> allIoItems)
        {
            SuspendLayout();
            try
            {
                _sourceEntity = entity ?? new GripperConfigEntity
                {
                    IsEnabled = true,
                    DriveMode = "Double",
                    CloseTimeoutMs = 3000,
                    OpenTimeoutMs = 3000
                };
                _isAdd = isAdd;

                LoadIoOptions(allIoItems ?? new List<MotionIoMapEntity>());
                LoadEntity();
            }
            finally
            {
                ResumeLayout();
            }
        }

        public bool TryBuildEntity(out GripperConfigEntity entity)
        {
            entity = null;

            var name = (inputName.Text ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "对象名称不能为空。");
                inputName.Focus();
                return false;
            }

            short? closeBit = GetSelectedIoBit(dropdownCloseOutputBit);
            if (!closeBit.HasValue)
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "请选择夹紧输出位。");
                dropdownCloseOutputBit.Focus();
                return false;
            }

            var driveMode = GetSelectedDriveMode();
            short? openBit = GetSelectedIoBit(dropdownOpenOutputBit);
            if (string.Equals(driveMode, "Double", StringComparison.OrdinalIgnoreCase) && !openBit.HasValue)
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "双线圈夹爪必须配置打开输出位。");
                dropdownOpenOutputBit.Focus();
                return false;
            }

            int closeTimeout;
            if (!TryParseNonNegativeInt(inputCloseTimeoutMs.Text, out closeTimeout))
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "夹紧超时必须为非负整数。");
                inputCloseTimeoutMs.Focus();
                return false;
            }

            int openTimeout;
            if (!TryParseNonNegativeInt(inputOpenTimeoutMs.Text, out openTimeout))
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "打开超时必须为非负整数。");
                inputOpenTimeoutMs.Focus();
                return false;
            }

            int sortOrder;
            if (!TryParseNonNegativeInt(inputSortOrder.Text, out sortOrder))
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "排序号必须为非负整数。");
                inputSortOrder.Focus();
                return false;
            }

            entity = new GripperConfigEntity
            {
                Id = _sourceEntity.Id,
                Name = name,
                DisplayName = NormalizeNullable(inputDisplayName.Text),
                DriveMode = driveMode,
                CloseOutputBit = closeBit.Value,
                OpenOutputBit = openBit,
                CloseFeedbackBit = GetSelectedIoBit(dropdownCloseFeedbackBit),
                OpenFeedbackBit = GetSelectedIoBit(dropdownOpenFeedbackBit),
                WorkpiecePresentBit = GetSelectedIoBit(dropdownWorkpiecePresentBit),
                UseFeedbackCheck = checkUseFeedbackCheck.Checked,
                UseWorkpieceCheck = checkUseWorkpieceCheck.Checked,
                CloseTimeoutMs = closeTimeout,
                OpenTimeoutMs = openTimeout,
                AlarmCodeOnCloseTimeout = _sourceEntity.AlarmCodeOnCloseTimeout,
                AlarmCodeOnOpenTimeout = _sourceEntity.AlarmCodeOnOpenTimeout,
                AlarmCodeOnWorkpieceLost = _sourceEntity.AlarmCodeOnWorkpieceLost,
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
            UpdateOpenOutputState();
        }

        private void LoadIoOptions(IList<MotionIoMapEntity> allIoItems)
        {
            _doRequiredOptions = BuildIoOptions(allIoItems, "DO", false);
            _doOptionalOptions = BuildIoOptions(allIoItems, "DO", true);
            _diOptionalOptions = BuildIoOptions(allIoItems, "DI", true);

            ApplySelectOptions(dropdownCloseOutputBit, _doRequiredOptions, null);
            ApplySelectOptions(dropdownOpenOutputBit, _doOptionalOptions, null);
            ApplySelectOptions(dropdownCloseFeedbackBit, _diOptionalOptions, null);
            ApplySelectOptions(dropdownOpenFeedbackBit, _diOptionalOptions, null);
            ApplySelectOptions(dropdownWorkpiecePresentBit, _diOptionalOptions, null);
        }

        private void LoadEntity()
        {
            inputName.Text = _sourceEntity.Name ?? string.Empty;
            inputName.Enabled = _isAdd;
            inputDisplayName.Text = _sourceEntity.DisplayName ?? string.Empty;

            SelectDriveMode(_sourceEntity.DriveMode);
            ApplySelectOptions(dropdownCloseOutputBit, _doRequiredOptions, _sourceEntity.CloseOutputBit > 0 ? (short?)_sourceEntity.CloseOutputBit : null);
            ApplySelectOptions(dropdownOpenOutputBit, _doOptionalOptions, _sourceEntity.OpenOutputBit);
            ApplySelectOptions(dropdownCloseFeedbackBit, _diOptionalOptions, _sourceEntity.CloseFeedbackBit);
            ApplySelectOptions(dropdownOpenFeedbackBit, _diOptionalOptions, _sourceEntity.OpenFeedbackBit);
            ApplySelectOptions(dropdownWorkpiecePresentBit, _diOptionalOptions, _sourceEntity.WorkpiecePresentBit);

            checkUseFeedbackCheck.Checked = _sourceEntity.UseFeedbackCheck;
            checkUseWorkpieceCheck.Checked = _sourceEntity.UseWorkpieceCheck;
            inputCloseTimeoutMs.Text = _sourceEntity.CloseTimeoutMs.ToString(CultureInfo.InvariantCulture);
            inputOpenTimeoutMs.Text = _sourceEntity.OpenTimeoutMs.ToString(CultureInfo.InvariantCulture);
            checkAllowBothOff.Checked = _sourceEntity.AllowBothOff;
            checkAllowBothOn.Checked = _sourceEntity.AllowBothOn;
            inputSortOrder.Text = _sourceEntity.SortOrder.ToString(CultureInfo.InvariantCulture);
            checkIsEnabled.Checked = _sourceEntity.IsEnabled;
            inputDescription.Text = _sourceEntity.Description ?? string.Empty;
            inputRemark.Text = _sourceEntity.Remark ?? string.Empty;

            UpdateOpenOutputState();
        }

        private void UpdateOpenOutputState()
        {
            var isSingle = string.Equals(GetSelectedDriveMode(), "Single", StringComparison.OrdinalIgnoreCase);
            dropdownOpenOutputBit.Enabled = !isSingle;

            if (isSingle && _doOptionalOptions.Count > 0)
            {
                dropdownOpenOutputBit.SelectedValue = _doOptionalOptions[0].DisplayName;
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

            foreach (var item in source
                .Where(x => string.Equals(x.IoType, ioType, StringComparison.OrdinalIgnoreCase))
                .OrderBy(x => x.SortOrder)
                .ThenBy(x => x.LogicalBit))
            {
                list.Add(new IoOptionItem(
                    item.LogicalBit,
                    "L" + item.LogicalBit + " · " + (string.IsNullOrWhiteSpace(item.Name) ? "-" : item.Name)));
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