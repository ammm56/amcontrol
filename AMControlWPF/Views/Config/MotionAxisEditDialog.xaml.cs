using AM.Model.Entity.Motion.Topology;
using System;
using System.Windows;
using System.Windows.Controls;

namespace AMControlWPF.Views.Config
{
    public partial class MotionAxisEditDialog : HandyControl.Controls.GlowWindow
    {
        private readonly bool _isAdd;
        private readonly int _originalId;
        private readonly DateTime _originalCreateTime;

        /// <summary>对话框确认后的结果实体，由调用方读取并写回 ViewModel。</summary>
        public MotionAxisEntity ResultEntity { get; private set; }

        public MotionAxisEditDialog(MotionAxisEntity entity, bool isAdd)
        {
            InitializeComponent();
            _isAdd = isAdd;
            _originalId = entity.Id;
            _originalCreateTime = entity.CreateTime;

            TextBlockTitle.Text = isAdd ? "新增轴拓扑" : "编辑轴拓扑";

            TextBoxLogicalAxis.Text = entity.LogicalAxis.ToString();
            TextBoxLogicalAxis.IsReadOnly = !isAdd;
            TextBoxLogicalAxis.Opacity = isAdd ? 1.0 : 0.6;

            TextBoxName.Text = entity.Name ?? string.Empty;
            TextBoxDisplayName.Text = entity.DisplayName ?? string.Empty;
            SelectComboBoxByTag(ComboBoxAxisCategory, entity.AxisCategory ?? "Linear");

            TextBoxCardId.Text = entity.CardId.ToString();
            TextBoxAxisId.Text = entity.AxisId.ToString();
            TextBoxPhysicalCore.Text = entity.PhysicalCore.ToString();
            TextBoxPhysicalAxis.Text = entity.PhysicalAxis.ToString();

            CheckBoxIsEnabled.IsChecked = entity.IsEnabled;
            TextBoxSortOrder.Text = entity.SortOrder.ToString();

            TextBoxDescription.Text = entity.Description ?? string.Empty;
            TextBoxRemark.Text = entity.Remark ?? string.Empty;
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

            var name = TextBoxName.Text == null ? string.Empty : TextBoxName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                ShowError("内部名称不能为空。");
                return;
            }

            var axisCategory = GetComboBoxTagString(ComboBoxAxisCategory, "Linear");

            short cardId;
            if (!short.TryParse(TextBoxCardId.Text.Trim(), out cardId) || cardId < 0)
            {
                ShowError("所属控制卡号必须为非负整数。");
                return;
            }

            short axisId;
            if (!short.TryParse(TextBoxAxisId.Text.Trim(), out axisId) || axisId < 0)
            {
                ShowError("卡内轴编号必须为非负整数。");
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
                CardId = cardId,
                AxisId = axisId,
                LogicalAxis = logicalAxis,
                Name = name,
                DisplayName = TextBoxDisplayName.Text == null ? null : TextBoxDisplayName.Text.Trim(),
                AxisCategory = axisCategory,
                PhysicalCore = physicalCore,
                PhysicalAxis = physicalAxis,
                IsEnabled = CheckBoxIsEnabled.IsChecked == true,
                SortOrder = sortOrder,
                Description = TextBoxDescription.Text == null ? null : TextBoxDescription.Text.Trim(),
                Remark = TextBoxRemark.Text == null ? null : TextBoxRemark.Text.Trim(),
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