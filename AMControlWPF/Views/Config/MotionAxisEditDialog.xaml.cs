using AM.Model.Entity.Motion.Topology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AMControlWPF.Views.Config
{
    public partial class MotionAxisEditDialog : HandyControl.Controls.GlowWindow
    {
        private readonly bool _isAdd;
        private readonly int _originalId;
        private readonly DateTime _originalCreateTime;
        private readonly IReadOnlyList<MotionCardEntity> _availableCards;

        /// <summary>对话框确认后的结果实体，由调用方读取并写回 ViewModel。</summary>
        public MotionAxisEntity ResultEntity { get; private set; }

        public MotionAxisEditDialog(MotionAxisEntity entity, bool isAdd,
            IReadOnlyList<MotionCardEntity> availableCards = null)
        {
            InitializeComponent();
            _isAdd = isAdd;
            _originalId = entity.Id;
            _originalCreateTime = entity.CreateTime;
            _availableCards = availableCards ?? new List<MotionCardEntity>();

            TextBlockTitle.Text = isAdd ? "新增轴拓扑" : "编辑轴拓扑";

            TextBoxLogicalAxis.Text = entity.LogicalAxis.ToString();
            TextBoxLogicalAxis.IsReadOnly = !isAdd;
            TextBoxLogicalAxis.Opacity = isAdd ? 1.0 : 0.6;

            TextBoxName.Text = entity.Name ?? string.Empty;
            TextBoxDisplayName.Text = entity.DisplayName ?? string.Empty;
            SelectComboBoxByTag(ComboBoxAxisCategory, entity.AxisCategory ?? "Linear");

            // 填充控制卡列表 → 选中当前卡 → 联动刷新三个物理映射下拉
            ComboBoxCardId.ItemsSource = _availableCards;
            SelectCardById(entity.CardId);
            RefreshPhysicalMappingOptions(entity.AxisId, entity.PhysicalCore, entity.PhysicalAxis);

            CheckBoxIsEnabled.IsChecked = entity.IsEnabled;
            TextBoxSortOrder.Text = entity.SortOrder.ToString();
            TextBoxDescription.Text = entity.Description ?? string.Empty;
            TextBoxRemark.Text = entity.Remark ?? string.Empty;
        }

        // ── 控制卡选择变更 ────────────────────────────────────────────────

        private void ComboBoxCardId_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 保留当前已选值，切换卡后按新卡范围重新约束
            var currentAxisId  = GetSelectedShort(ComboBoxAxisId,      0);
            var currentCore    = GetSelectedShort(ComboBoxPhysicalCore, 1);
            var currentPhysAxis = GetSelectedShort(ComboBoxPhysicalAxis, 0);

            RefreshPhysicalMappingOptions(currentAxisId, currentCore, currentPhysAxis);
        }

        /// <summary>
        /// 根据当前所选控制卡的轴数（AxisCountNumber）和内核数（CoreNumber）
        /// 统一刷新三个物理映射下拉框的可选项，并尽量保留用户已选值。
        /// </summary>
        private void RefreshPhysicalMappingOptions(short preferAxisId, short preferCore, short preferPhysAxis)
        {
            var card = ComboBoxCardId.SelectedItem as MotionCardEntity;

            // 轴数：无卡时默认 32 个选项
            int axisMax  = card != null && card.AxisCountNumber > 0 ? card.AxisCountNumber : 32;
            // 内核数：1-based，无卡时默认 1 个
            int coreMax  = card != null && card.CoreNumber > 0 ? card.CoreNumber : 1;

            // AxisId：0 .. axisMax-1
            var axisOptions = BuildShortRange(0, axisMax - 1);
            ComboBoxAxisId.ItemsSource = axisOptions;
            ComboBoxAxisId.SelectedIndex = (preferAxisId >= 0 && preferAxisId < axisMax) ? preferAxisId : 0;

            // PhysicalCore：1 .. coreMax（1-based）
            var coreOptions = BuildShortRange(1, coreMax);
            ComboBoxPhysicalCore.ItemsSource = coreOptions;
            var coreIdx = coreOptions.IndexOf(preferCore);
            ComboBoxPhysicalCore.SelectedIndex = coreIdx >= 0 ? coreIdx : 0;

            // PhysicalAxis：0 .. axisMax-1（与 AxisId 范围相同）
            var physAxisOptions = BuildShortRange(0, axisMax - 1);
            ComboBoxPhysicalAxis.ItemsSource = physAxisOptions;
            ComboBoxPhysicalAxis.SelectedIndex = (preferPhysAxis >= 0 && preferPhysAxis < axisMax) ? preferPhysAxis : 0;
        }

        // ── 保存 ─────────────────────────────────────────────────────────

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            TextBlockError.Visibility = Visibility.Collapsed;
            TextBlockError.Text = string.Empty;

            short logicalAxis;
            if (!short.TryParse(TextBoxLogicalAxis.Text.Trim(), out logicalAxis) || logicalAxis < 0)
            {
                ShowError("逻辑轴号必须为 0 ~ 32767 的整数。");
                return;
            }

            var name = (TextBoxName.Text ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(name))
            {
                ShowError("内部名称不能为空。");
                return;
            }

            var axisCategory = GetComboBoxTagString(ComboBoxAxisCategory, "Linear");

            var selectedCard = ComboBoxCardId.SelectedItem as MotionCardEntity;
            if (selectedCard == null)
            {
                ShowError("请选择所属控制卡。若列表为空，请先在控制卡管理页面添加控制卡。");
                return;
            }

            // 三个物理映射字段均来自下拉框，范围已在填充时约束，直接读取
            var axisId      = GetSelectedShort(ComboBoxAxisId,       0);
            var physCore    = GetSelectedShort(ComboBoxPhysicalCore,  1);
            var physAxis    = GetSelectedShort(ComboBoxPhysicalAxis,  0);

            int sortOrder;
            if (!int.TryParse(TextBoxSortOrder.Text.Trim(), out sortOrder) || sortOrder < 0)
            {
                ShowError("排序号必须为非负整数。");
                return;
            }

            ResultEntity = new MotionAxisEntity
            {
                Id = _originalId,
                CardId = selectedCard.CardId,
                AxisId = axisId,
                LogicalAxis = logicalAxis,
                Name = name,
                DisplayName = (TextBoxDisplayName.Text ?? string.Empty).Trim(),
                AxisCategory = axisCategory,
                PhysicalCore = physCore,
                PhysicalAxis = physAxis,
                IsEnabled = CheckBoxIsEnabled.IsChecked == true,
                SortOrder = sortOrder,
                Description = (TextBoxDescription.Text ?? string.Empty).Trim(),
                Remark = (TextBoxRemark.Text ?? string.Empty).Trim(),
                CreateTime = _originalCreateTime,
                UpdateTime = DateTime.Now
            };

            DialogResult = true;
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        // ── 辅助方法 ──────────────────────────────────────────────────────

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

        /// <summary>读取 ComboBox 当前选中的 short 值，未选时返回 defaultValue。</summary>
        private static short GetSelectedShort(ComboBox comboBox, short defaultValue)
        {
            return comboBox.SelectedItem is short s ? s : defaultValue;
        }

        /// <summary>
        /// 生成从 from 到 to（含两端）的 short 列表。
        /// 例：BuildShortRange(0, 3) → [0, 1, 2, 3]
        ///     BuildShortRange(1, 2) → [1, 2]
        /// </summary>
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