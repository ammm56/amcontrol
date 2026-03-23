using AM.Model.Entity.Motion.Topology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AMControlWPF.Views.Config
{
    public partial class MotionIoMapEditDialog : HandyControl.Controls.GlowWindow
    {
        private readonly bool _isAdd;
        private readonly int _originalId;
        private readonly IReadOnlyList<MotionCardEntity> _availableCards;

        public MotionIoMapEntity ResultEntity { get; private set; }

        public MotionIoMapEditDialog(MotionIoMapEntity entity, bool isAdd,
            IReadOnlyList<MotionCardEntity> availableCards = null)
        {
            InitializeComponent();
            _isAdd = isAdd;
            _originalId = entity.Id;
            _availableCards = availableCards ?? new List<MotionCardEntity>();

            TextBlockTitle.Text = isAdd ? "新增 IO 映射" : "编辑 IO 映射";

            // 控制卡下拉
            ComboBoxCardId.ItemsSource = _availableCards;
            SelectCardById(entity.CardId);

            // IO 类型（新增时允许改，编辑时只读提示）
            SelectComboBoxByTag(ComboBoxIoType, entity.IoType ?? "DI");
            ComboBoxIoType.IsEnabled = isAdd;
            ComboBoxIoType.Opacity = isAdd ? 1.0 : 0.6;

            // 逻辑位（新增可改，编辑时只读，保持业务键不变）
            TextBoxLogicalBit.Text = entity.LogicalBit.ToString();
            TextBoxLogicalBit.IsReadOnly = !isAdd;
            TextBoxLogicalBit.Opacity = isAdd ? 1.0 : 0.6;

            TextBoxName.Text = entity.Name ?? string.Empty;

            // 内核下拉（依赖卡）
            UpdateCoreOptions(entity.Core);

            TextBoxHardwareBit.Text = entity.HardwareBit.ToString();
            CheckBoxIsExtModule.IsChecked = entity.IsExtModule;
            CheckBoxIsEnabled.IsChecked = entity.IsEnabled;
            TextBoxSortOrder.Text = entity.SortOrder.ToString();
            TextBoxRemark.Text = entity.Remark ?? string.Empty;
        }

        // ── 控制卡切换 → 刷新内核选项 ────────────────────────────────

        private void ComboBoxCardId_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentCore = GetSelectedShort(ComboBoxCore, 1);
            UpdateCoreOptions(currentCore);
        }

        private void UpdateCoreOptions(short preferCore)
        {
            var card = ComboBoxCardId.SelectedItem as MotionCardEntity;
            var coreMax = card != null && card.CoreNumber > 0 ? card.CoreNumber : 1;
            var options = BuildShortRange(1, coreMax); // 1-based

            ComboBoxCore.ItemsSource = options;

            var idx = options.IndexOf(preferCore);
            ComboBoxCore.SelectedIndex = idx >= 0 ? idx : 0;
        }

        // ── 保存 ─────────────────────────────────────────────────────

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            TextBlockError.Visibility = Visibility.Collapsed;

            var selectedCard = ComboBoxCardId.SelectedItem as MotionCardEntity;
            if (selectedCard == null)
            {
                ShowError("请选择所属控制卡。若列表为空，请先在控制卡管理页面添加控制卡。");
                return;
            }

            var ioType = GetComboBoxTagString(ComboBoxIoType, null);
            if (string.IsNullOrEmpty(ioType))
            {
                ShowError("请选择 IO 类型。");
                return;
            }

            short logicalBit;
            if (!short.TryParse(TextBoxLogicalBit.Text.Trim(), out logicalBit) || logicalBit <= 0)
            {
                ShowError("逻辑位号必须为大于 0 的整数。");
                return;
            }

            var name = (TextBoxName.Text ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(name))
            {
                ShowError("名称不能为空。");
                return;
            }

            var core = GetSelectedShort(ComboBoxCore, 1);

            short hardwareBit;
            if (!short.TryParse(TextBoxHardwareBit.Text.Trim(), out hardwareBit) || hardwareBit < 0)
            {
                ShowError("硬件位号必须为非负整数。");
                return;
            }

            int sortOrder;
            if (!int.TryParse(TextBoxSortOrder.Text.Trim(), out sortOrder) || sortOrder < 0)
            {
                ShowError("排序号必须为非负整数。");
                return;
            }

            ResultEntity = new MotionIoMapEntity
            {
                Id = _originalId,
                CardId = selectedCard.CardId,
                IoType = ioType,
                LogicalBit = logicalBit,
                Name = name,
                Core = core,
                IsExtModule = CheckBoxIsExtModule.IsChecked == true,
                HardwareBit = hardwareBit,
                IsEnabled = CheckBoxIsEnabled.IsChecked == true,
                SortOrder = sortOrder,
                Remark = (TextBoxRemark.Text ?? string.Empty).Trim()
            };

            DialogResult = true;
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        // ── 辅助 ─────────────────────────────────────────────────────

        private void SelectCardById(short cardId)
        {
            var card = _availableCards.FirstOrDefault(c => c.CardId == cardId);
            ComboBoxCardId.SelectedItem = card ?? (_availableCards.Count > 0 ? _availableCards[0] : null);
        }

        private void ShowError(string message)
        {
            TextBlockError.Text = message;
            TextBlockError.Visibility = Visibility.Visible;
        }

        private static short GetSelectedShort(ComboBox comboBox, short defaultValue)
        {
            return comboBox.SelectedItem is short s ? s : defaultValue;
        }

        private static List<short> BuildShortRange(int from, int to)
        {
            var list = new List<short>(Math.Max(to - from + 1, 0));
            for (var i = from; i <= to; i++)
            {
                list.Add((short)i);
            }
            return list;
        }

        private static void SelectComboBoxByTag(ComboBox comboBox, string tag)
        {
            foreach (ComboBoxItem item in comboBox.Items)
            {
                if (item.Tag != null && item.Tag.ToString() == tag)
                {
                    comboBox.SelectedItem = item;
                    return;
                }
            }
            if (comboBox.Items.Count > 0)
            {
                comboBox.SelectedIndex = 0;
            }
        }

        private static string GetComboBoxTagString(ComboBox comboBox, string defaultValue)
        {
            var item = comboBox.SelectedItem as ComboBoxItem;
            return item?.Tag?.ToString() ?? defaultValue;
        }
    }
}