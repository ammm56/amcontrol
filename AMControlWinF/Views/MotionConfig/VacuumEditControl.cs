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
    public partial class VacuumEditControl : UserControl
    {
        private VacuumConfigEntity _sourceEntity;
        private bool _isAdd;
        private List<IoOptionItem> _doRequiredOptions;
        private List<IoOptionItem> _doOptionalOptions;
        private List<IoOptionItem> _diOptionalOptions;

        public VacuumEditControl()
        {
            InitializeComponent();

            _sourceEntity = new VacuumConfigEntity();
            _doRequiredOptions = new List<IoOptionItem>();
            _doOptionalOptions = new List<IoOptionItem>();
            _diOptionalOptions = new List<IoOptionItem>();
        }

        public void Bind(VacuumConfigEntity entity, bool isAdd, IList<MotionIoMapEntity> allIoItems)
        {
            _sourceEntity = entity ?? new VacuumConfigEntity
            {
                IsEnabled = true,
                VacuumBuildTimeoutMs = 3000,
                ReleaseTimeoutMs = 1000,
                KeepVacuumOnAfterDetected = true
            };
            _isAdd = isAdd;

            LoadIoOptions(allIoItems ?? new List<MotionIoMapEntity>());
            LoadEntity();
        }

        public bool TryBuildEntity(out VacuumConfigEntity entity)
        {
            entity = null;

            var name = (inputName.Text ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "对象名称不能为空。");
                inputName.Focus();
                return false;
            }

            short? vacuumBit = GetSelectedIoBit(dropdownVacuumOnOutputBit);
            if (!vacuumBit.HasValue)
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "请选择吸真空输出位。");
                dropdownVacuumOnOutputBit.Focus();
                return false;
            }

            int buildTimeout;
            if (!TryParseNonNegativeInt(inputVacuumBuildTimeoutMs.Text, out buildTimeout))
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "建压超时必须为非负整数。");
                inputVacuumBuildTimeoutMs.Focus();
                return false;
            }

            int releaseTimeout;
            if (!TryParseNonNegativeInt(inputReleaseTimeoutMs.Text, out releaseTimeout))
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "释放超时必须为非负整数。");
                inputReleaseTimeoutMs.Focus();
                return false;
            }

            int sortOrder;
            if (!TryParseNonNegativeInt(inputSortOrder.Text, out sortOrder))
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "排序号必须为非负整数。");
                inputSortOrder.Focus();
                return false;
            }

            entity = new VacuumConfigEntity
            {
                Id = _sourceEntity.Id,
                Name = name,
                DisplayName = NormalizeNullable(inputDisplayName.Text),
                VacuumOnOutputBit = vacuumBit.Value,
                BlowOffOutputBit = GetSelectedIoBit(dropdownBlowOffOutputBit),
                VacuumFeedbackBit = GetSelectedIoBit(dropdownVacuumFeedbackBit),
                ReleaseFeedbackBit = GetSelectedIoBit(dropdownReleaseFeedbackBit),
                WorkpiecePresentBit = GetSelectedIoBit(dropdownWorkpiecePresentBit),
                UseFeedbackCheck = checkUseFeedbackCheck.Checked,
                UseWorkpieceCheck = checkUseWorkpieceCheck.Checked,
                VacuumBuildTimeoutMs = buildTimeout,
                ReleaseTimeoutMs = releaseTimeout,
                AlarmCodeOnBuildTimeout = _sourceEntity.AlarmCodeOnBuildTimeout,
                AlarmCodeOnReleaseTimeout = _sourceEntity.AlarmCodeOnReleaseTimeout,
                AlarmCodeOnWorkpieceLost = _sourceEntity.AlarmCodeOnWorkpieceLost,
                KeepVacuumOnAfterDetected = checkKeepVacuumOnAfterDetected.Checked,
                IsEnabled = checkIsEnabled.Checked,
                SortOrder = sortOrder,
                Description = NormalizeNullable(inputDescription.Text),
                Remark = NormalizeNullable(inputRemark.Text)
            };

            return true;
        }

        private void LoadIoOptions(IList<MotionIoMapEntity> allIoItems)
        {
            _doRequiredOptions = BuildIoOptions(allIoItems, "DO", false);
            _doOptionalOptions = BuildIoOptions(allIoItems, "DO", true);
            _diOptionalOptions = BuildIoOptions(allIoItems, "DI", true);

            ApplySelectOptions(dropdownVacuumOnOutputBit, _doRequiredOptions, null);
            ApplySelectOptions(dropdownBlowOffOutputBit, _doOptionalOptions, null);
            ApplySelectOptions(dropdownVacuumFeedbackBit, _diOptionalOptions, null);
            ApplySelectOptions(dropdownReleaseFeedbackBit, _diOptionalOptions, null);
            ApplySelectOptions(dropdownWorkpiecePresentBit, _diOptionalOptions, null);
        }

        private void LoadEntity()
        {
            inputName.Text = _sourceEntity.Name ?? string.Empty;
            inputName.Enabled = _isAdd;
            inputDisplayName.Text = _sourceEntity.DisplayName ?? string.Empty;

            ApplySelectOptions(dropdownVacuumOnOutputBit, _doRequiredOptions, _sourceEntity.VacuumOnOutputBit > 0 ? (short?)_sourceEntity.VacuumOnOutputBit : null);
            ApplySelectOptions(dropdownBlowOffOutputBit, _doOptionalOptions, _sourceEntity.BlowOffOutputBit);
            ApplySelectOptions(dropdownVacuumFeedbackBit, _diOptionalOptions, _sourceEntity.VacuumFeedbackBit);
            ApplySelectOptions(dropdownReleaseFeedbackBit, _diOptionalOptions, _sourceEntity.ReleaseFeedbackBit);
            ApplySelectOptions(dropdownWorkpiecePresentBit, _diOptionalOptions, _sourceEntity.WorkpiecePresentBit);

            checkUseFeedbackCheck.Checked = _sourceEntity.UseFeedbackCheck;
            checkUseWorkpieceCheck.Checked = _sourceEntity.UseWorkpieceCheck;
            checkKeepVacuumOnAfterDetected.Checked = _sourceEntity.KeepVacuumOnAfterDetected;
            inputVacuumBuildTimeoutMs.Text = _sourceEntity.VacuumBuildTimeoutMs.ToString(CultureInfo.InvariantCulture);
            inputReleaseTimeoutMs.Text = _sourceEntity.ReleaseTimeoutMs.ToString(CultureInfo.InvariantCulture);
            inputSortOrder.Text = _sourceEntity.SortOrder.ToString(CultureInfo.InvariantCulture);
            checkIsEnabled.Checked = _sourceEntity.IsEnabled;
            inputDescription.Text = _sourceEntity.Description ?? string.Empty;
            inputRemark.Text = _sourceEntity.Remark ?? string.Empty;
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