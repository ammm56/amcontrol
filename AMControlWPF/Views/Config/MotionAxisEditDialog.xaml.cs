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

            // 填充控制卡下拉列表并选中当前卡
            PopulateCardComboBox();
            SelectCardById(entity.CardId);

            // 根据所选控制卡更新轴编号选项，并选中当前轴编号
            UpdateAxisIdOptions(entity.AxisId);

            TextBoxPhysicalCore.Text = entity.PhysicalCore.ToString();
            TextBoxPhysicalAxis.Text = entity.PhysicalAxis.ToString();

            CheckBoxIsEnabled.IsChecked = entity.IsEnabled;
            TextBoxSortOrder.Text = entity.SortOrder.ToString();

            TextBoxDescription.Text = entity.Description ?? string.Empty;
            TextBoxRemark.Text = entity.Remark ?? string.Empty;
        }

        private void PopulateCardComboBox()
        {
            ComboBoxCardId.ItemsSource = _availableCards;
        }

        private void SelectCardById(short cardId)
        {
            var card = _availableCards.FirstOrDefault(c => c.CardId == cardId);
            ComboBoxCardId.SelectedItem = card ?? (_availableCards.Count > 0 ? _availableCards[0] : null);
        }

        private void ComboBoxCardId_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 保留当前已选轴编号，切换卡后刷新可选范围
            short currentAxisId = 0;
            if (ComboBoxAxisId.SelectedItem is short s)
            {
                currentAxisId = s;
            }

            UpdateAxisIdOptions(currentAxisId);
        }

        private void UpdateAxisIdOptions(short preferAxisId = 0)
        {
            var selectedCard = ComboBoxCardId.SelectedItem as MotionCardEntity;
            var maxCount = selectedCard != null && selectedCard.AxisCountNumber > 0
                ? selectedCard.AxisCountNumber
                : 32;

            var options = new List<short>();
            for (short i = 0; i < maxCount; i++)
            {
                options.Add(i);
            }

            ComboBoxAxisId.ItemsSource = options;

            // 优先保持原轴编号，越界则选第 0 号
            if (preferAxisId >= 0 && preferAxisId < maxCount)
            {
                ComboBoxAxisId.SelectedIndex = preferAxisId;
            }
            else if (options.Count > 0)
            {
                ComboBoxAxisId.SelectedIndex = 0;
            }
        }

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            TextBlockError.Visibility = Visibility.Collapsed;
            TextBlockError.Text = string.Empty;

            // ── 校验 ────────────────────────────────────────────────────
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

            if (!(ComboBoxAxisId.SelectedItem is short axisId))
            {
                ShowError("请选择卡内轴编号。");
                return;
            }

            short physicalCore;
            if (!short.TryParse(TextBoxPhysicalCore.Text.Trim(), out physicalCore) || physicalCore < 1)
            {
                ShowError("物理内核号必须 ≥ 1。");
                return;
            }

            short physicalAxis;
            if (!short.TryParse(TextBoxPhysicalAxis.Text.Trim(), out physicalAxis) || physicalAxis < 0)
            {
                ShowError("物理轴号必须为非负整数。");
                return;
            }

            int sortOrder;
            if (!int.TryParse(TextBoxSortOrder.Text.Trim(), out sortOrder) || sortOrder < 0)
            {
                ShowError("排序号必须为非负整数。");
                return;
            }

            // ── 构建结果 ────────────────────────────────────────────────
            ResultEntity = new MotionAxisEntity
            {
                Id = _originalId,
                CardId = selectedCard.CardId,
                AxisId = axisId,
                LogicalAxis = logicalAxis,
                Name = name,
                DisplayName = (TextBoxDisplayName.Text ?? string.Empty).Trim(),
                AxisCategory = axisCategory,
                PhysicalCore = physicalCore,
                PhysicalAxis = physicalAxis,
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

        // ── 辅助方法 ─────────────────────────────────────────────────────

        private void ShowError(string message)
        {
            TextBlockError.Text = message;
            TextBlockError.Visibility = Visibility.Visible;
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
            if (item == null || item.Tag == null)
            {
                return defaultValue;
            }

            return item.Tag.ToString();
        }
    }
}