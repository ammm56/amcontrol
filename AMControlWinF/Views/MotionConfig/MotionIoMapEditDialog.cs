using AM.Core.Context;
using AM.Model.Entity.Motion.Topology;
using AMControlWinF.Tools;
using AntdUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AMControlWinF.Views.MotionConfig
{
    /// <summary>
    /// IO 映射新增/编辑对话框。
    /// </summary>
    public partial class MotionIoMapEditDialog : AntdUI.Window
    {
        private static readonly IoTypeItem[] IoTypes = new[]
        {
            new IoTypeItem("DI", "DI（数字输入）"),
            new IoTypeItem("DO", "DO（数字输出）")
        };

        private readonly bool _isAdd;
        private readonly MotionIoMapEntity _sourceEntity;
        private readonly List<MotionCardEntity> _availableCards;

        public MotionIoMapEditDialog(MotionIoMapEntity entity, bool isAdd, List<MotionCardEntity> availableCards)
        {
            InitializeComponent();

            _sourceEntity = entity ?? new MotionIoMapEntity();
            _isAdd = isAdd;
            _availableCards = availableCards ?? new List<MotionCardEntity>();

            InitializeIoTypeDropdown();
            InitializeCardDropdown();
            BindEvents();
            ApplyThemeFromConfig();
            ApplyMode();
            LoadEntity();
        }

        public MotionIoMapEntity ResultEntity { get; private set; }

        private void InitializeIoTypeDropdown()
        {
            dropdownIoType.Items.Clear();
            dropdownIoType.Items.AddRange(IoTypes.Select(x => (object)x.DisplayName).ToArray());

            if (IoTypes.Length > 0)
            {
                dropdownIoType.SelectedValue = IoTypes[0].DisplayName;
            }
        }

        private void InitializeCardDropdown()
        {
            dropdownCardId.Items.Clear();
            dropdownCardId.Items.AddRange(_availableCards.Select(x => (object)GetCardDisplayText(x)).ToArray());

            if (_availableCards.Count > 0)
            {
                dropdownCardId.SelectedValue = GetCardDisplayText(_availableCards[0]);
            }
        }

        private void BindEvents()
        {
            dropdownCardId.SelectedValueChanged += DropdownCardId_SelectedValueChanged;
            buttonOk.Click += ButtonOk_Click;
            buttonCancel.Click += ButtonCancel_Click;
            Shown += MotionIoMapEditDialog_Shown;

            KeyPreview = true;
            KeyDown += MotionIoMapEditDialog_KeyDown;
        }

        private void MotionIoMapEditDialog_Shown(object sender, EventArgs e)
        {
            if (_isAdd)
            {
                inputLogicalBit.Focus();
                inputLogicalBit.SelectAll();
            }
            else
            {
                inputName.Focus();
                inputName.SelectAll();
            }
        }

        private void ApplyMode()
        {
            Text = _isAdd ? "新增 IO 映射" : "编辑 IO 映射";
            labelDialogTitle.Text = _isAdd ? "新增 IO 映射" : "编辑 IO 映射";
            labelDialogDescription.Text = _isAdd
                ? "填写 IO 映射配置"
                : "修改 IO 映射配置";

            buttonOk.Text = "保存";
        }

        private void LoadEntity()
        {
            var selectedIoType = IoTypes.FirstOrDefault(x =>
                string.Equals(x.Value, _sourceEntity.IoType ?? "DI", StringComparison.OrdinalIgnoreCase));

            dropdownIoType.SelectedValue = selectedIoType == null
                ? IoTypes[0].DisplayName
                : selectedIoType.DisplayName;

            dropdownIoType.Enabled = _isAdd;

            inputLogicalBit.Text = _sourceEntity.LogicalBit.ToString();
            inputLogicalBit.Enabled = _isAdd;

            inputName.Text = _sourceEntity.Name ?? string.Empty;

            SelectCardById(_sourceEntity.CardId);
            RefreshCoreOptions(_sourceEntity.Core);

            inputHardwareBit.Text = _sourceEntity.HardwareBit.ToString();
            checkIsExtModule.Checked = _sourceEntity.IsExtModule;
            checkIsEnabled.Checked = _sourceEntity.IsEnabled;
            inputSortOrder.Text = _sourceEntity.SortOrder.ToString();
            inputRemark.Text = _sourceEntity.Remark ?? string.Empty;
        }

        private void DropdownCardId_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            var currentCore = GetSelectedShort(dropdownCore, 1);
            RefreshCoreOptions(currentCore);
        }

        private void RefreshCoreOptions(short preferCore)
        {
            var card = GetSelectedCard();

            var coreMax = card != null && card.CoreNumber > 0
                ? card.CoreNumber
                : 1;

            if (coreMax <= 0)
                coreMax = 1;

            var coreOptions = BuildShortRange(1, coreMax);

            dropdownCore.Items.Clear();
            dropdownCore.Items.AddRange(coreOptions.Select(x => (object)x.ToString()).ToArray());
            dropdownCore.SelectedValue = coreOptions.Contains(preferCore)
                ? preferCore.ToString()
                : coreOptions[0].ToString();
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            MotionIoMapEntity entity;
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

        private void MotionIoMapEditDialog_KeyDown(object sender, KeyEventArgs e)
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

        private bool TryBuildEntity(out MotionIoMapEntity entity)
        {
            entity = null;

            var selectedCard = GetSelectedCard();
            if (selectedCard == null)
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "请选择所属控制卡。");
                dropdownCardId.Focus();
                return false;
            }

            var selectedIoTypeText = dropdownIoType.SelectedValue == null
                ? string.Empty
                : dropdownIoType.SelectedValue.ToString();

            var ioType = IoTypes.FirstOrDefault(x =>
                string.Equals(x.DisplayName, selectedIoTypeText, StringComparison.OrdinalIgnoreCase));

            if (ioType == null)
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "请选择 IO 类型。");
                dropdownIoType.Focus();
                return false;
            }

            short logicalBit;
            if (!short.TryParse((inputLogicalBit.Text ?? string.Empty).Trim(), out logicalBit) || logicalBit <= 0)
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "逻辑位号必须为大于 0 的整数。");
                inputLogicalBit.Focus();
                return false;
            }

            var name = (inputName.Text ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "名称不能为空。");
                inputName.Focus();
                return false;
            }

            var core = GetSelectedShort(dropdownCore, 1);

            short hardwareBit;
            if (!short.TryParse((inputHardwareBit.Text ?? string.Empty).Trim(), out hardwareBit) || hardwareBit < 0)
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "硬件位号必须为非负整数。");
                inputHardwareBit.Focus();
                return false;
            }

            int sortOrder;
            if (!int.TryParse((inputSortOrder.Text ?? string.Empty).Trim(), out sortOrder) || sortOrder < 0)
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "排序号必须为非负整数。");
                inputSortOrder.Focus();
                return false;
            }

            entity = new MotionIoMapEntity
            {
                Id = _sourceEntity.Id,
                CardId = selectedCard.CardId,
                IoType = ioType.Value,
                LogicalBit = logicalBit,
                Name = name,
                Core = core,
                IsExtModule = checkIsExtModule.Checked,
                HardwareBit = hardwareBit,
                IsEnabled = checkIsEnabled.Checked,
                SortOrder = sortOrder,
                Remark = NormalizeNullable(inputRemark.Text)
            };

            return true;
        }

        private MotionCardEntity GetSelectedCard()
        {
            var text = dropdownCardId.SelectedValue == null
                ? string.Empty
                : dropdownCardId.SelectedValue.ToString();

            return _availableCards.FirstOrDefault(x =>
                string.Equals(GetCardDisplayText(x), text, StringComparison.OrdinalIgnoreCase));
        }

        private void SelectCardById(short cardId)
        {
            var card = _availableCards.FirstOrDefault(x => x.CardId == cardId);
            if (card != null)
            {
                dropdownCardId.SelectedValue = GetCardDisplayText(card);
                return;
            }

            if (_availableCards.Count > 0)
            {
                dropdownCardId.SelectedValue = GetCardDisplayText(_availableCards[0]);
            }
        }

        private static string GetCardDisplayText(MotionCardEntity card)
        {
            if (card == null)
                return string.Empty;

            var name = string.IsNullOrWhiteSpace(card.DisplayName)
                ? (string.IsNullOrWhiteSpace(card.Name) ? "控制卡" : card.Name)
                : card.DisplayName;

            return "#" + card.CardId + "  " + name;
        }

        private static short GetSelectedShort(Select select, short defaultValue)
        {
            if (select == null || select.SelectedValue == null)
                return defaultValue;

            short result;
            return short.TryParse(select.SelectedValue.ToString(), out result)
                ? result
                : defaultValue;
        }

        private static List<short> BuildShortRange(int from, int to)
        {
            var list = new List<short>();

            for (var i = from; i <= to; i++)
            {
                list.Add((short)i);
            }

            return list;
        }

        private static string NormalizeNullable(string value)
        {
            var text = value == null ? string.Empty : value.Trim();
            return string.IsNullOrWhiteSpace(text) ? null : text;
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

        private sealed class IoTypeItem
        {
            public IoTypeItem(string value, string displayName)
            {
                Value = value;
                DisplayName = displayName;
            }

            public string Value { get; private set; }

            public string DisplayName { get; private set; }
        }
    }
}