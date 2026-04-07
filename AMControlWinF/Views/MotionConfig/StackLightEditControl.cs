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
    public partial class StackLightEditControl : UserControl
    {
        private StackLightConfigEntity _sourceEntity;
        private bool _isAdd;
        private List<IoOptionItem> _doOptionalOptions;

        public StackLightEditControl()
        {
            InitializeComponent();

            _sourceEntity = new StackLightConfigEntity();
            _doOptionalOptions = new List<IoOptionItem>();
        }

        public void Bind(StackLightConfigEntity entity, bool isAdd, IList<MotionIoMapEntity> allIoItems)
        {
            _sourceEntity = entity ?? new StackLightConfigEntity
            {
                IsEnabled = true
            };
            _isAdd = isAdd;

            LoadIoOptions(allIoItems ?? new List<MotionIoMapEntity>());
            LoadEntity();
        }

        public bool TryBuildEntity(out StackLightConfigEntity entity)
        {
            entity = null;

            var name = (inputName.Text ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "对象名称不能为空。");
                inputName.Focus();
                return false;
            }

            int sortOrder;
            if (!TryParseNonNegativeInt(inputSortOrder.Text, out sortOrder))
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "排序号必须为非负整数。");
                inputSortOrder.Focus();
                return false;
            }

            entity = new StackLightConfigEntity
            {
                Id = _sourceEntity.Id,
                Name = name,
                DisplayName = NormalizeNullable(inputDisplayName.Text),
                RedOutputBit = GetSelectedIoBit(dropdownRedOutputBit),
                YellowOutputBit = GetSelectedIoBit(dropdownYellowOutputBit),
                GreenOutputBit = GetSelectedIoBit(dropdownGreenOutputBit),
                BlueOutputBit = GetSelectedIoBit(dropdownBlueOutputBit),
                BuzzerOutputBit = GetSelectedIoBit(dropdownBuzzerOutputBit),
                EnableBuzzerOnWarning = checkEnableBuzzerOnWarning.Checked,
                EnableBuzzerOnAlarm = checkEnableBuzzerOnAlarm.Checked,
                AllowMultiSegmentOn = checkAllowMultiSegmentOn.Checked,
                IsEnabled = checkIsEnabled.Checked,
                SortOrder = sortOrder,
                Description = NormalizeNullable(inputDescription.Text),
                Remark = NormalizeNullable(inputRemark.Text)
            };

            return true;
        }

        private void LoadIoOptions(IList<MotionIoMapEntity> allIoItems)
        {
            _doOptionalOptions = BuildIoOptions(allIoItems, "DO", true);

            ApplySelectOptions(dropdownRedOutputBit, _doOptionalOptions, null);
            ApplySelectOptions(dropdownYellowOutputBit, _doOptionalOptions, null);
            ApplySelectOptions(dropdownGreenOutputBit, _doOptionalOptions, null);
            ApplySelectOptions(dropdownBlueOutputBit, _doOptionalOptions, null);
            ApplySelectOptions(dropdownBuzzerOutputBit, _doOptionalOptions, null);
        }

        private void LoadEntity()
        {
            inputName.Text = _sourceEntity.Name ?? string.Empty;
            inputName.Enabled = _isAdd;
            inputDisplayName.Text = _sourceEntity.DisplayName ?? string.Empty;

            ApplySelectOptions(dropdownRedOutputBit, _doOptionalOptions, _sourceEntity.RedOutputBit);
            ApplySelectOptions(dropdownYellowOutputBit, _doOptionalOptions, _sourceEntity.YellowOutputBit);
            ApplySelectOptions(dropdownGreenOutputBit, _doOptionalOptions, _sourceEntity.GreenOutputBit);
            ApplySelectOptions(dropdownBlueOutputBit, _doOptionalOptions, _sourceEntity.BlueOutputBit);
            ApplySelectOptions(dropdownBuzzerOutputBit, _doOptionalOptions, _sourceEntity.BuzzerOutputBit);

            checkEnableBuzzerOnWarning.Checked = _sourceEntity.EnableBuzzerOnWarning;
            checkEnableBuzzerOnAlarm.Checked = _sourceEntity.EnableBuzzerOnAlarm;
            checkAllowMultiSegmentOn.Checked = _sourceEntity.AllowMultiSegmentOn;
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