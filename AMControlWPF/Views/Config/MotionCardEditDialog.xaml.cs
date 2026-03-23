using AM.Model.Entity.Motion.Topology;
using System;
using System.Windows;
using System.Windows.Controls;

namespace AMControlWPF.Views.Config
{
    public partial class MotionCardEditDialog : HandyControl.Controls.GlowWindow
    {
        private readonly bool _isAdd;

        /// <summary>对话框确认后的结果实体，由调用方读取并写回 ViewModel。</summary>
        public MotionCardEntity ResultEntity { get; private set; }

        public MotionCardEditDialog(MotionCardEntity entity, bool isAdd)
        {
            InitializeComponent();
            _isAdd = isAdd;

            TextBlockTitle.Text = isAdd ? "新增控制卡" : "编辑控制卡";

            TextBoxCardId.Text = entity.CardId.ToString();
            TextBoxCardId.IsReadOnly = !isAdd;
            TextBoxCardId.Opacity = isAdd ? 1.0 : 0.6;

            SelectComboBoxByTag(ComboBoxCardType, entity.CardType.ToString());
            TextBoxName.Text = entity.Name ?? string.Empty;
            TextBoxDisplayName.Text = entity.DisplayName ?? string.Empty;
            TextBoxDriverKey.Text = entity.DriverKey ?? string.Empty;

            TextBoxCoreNumber.Text = entity.CoreNumber.ToString();
            TextBoxAxisCountNumber.Text = entity.AxisCountNumber.ToString();
            TextBoxModeParam.Text = entity.ModeParam.ToString();
            TextBoxInitOrder.Text = entity.InitOrder.ToString();
            CheckBoxUseExtModule.IsChecked = entity.UseExtModule;

            CheckBoxIsEnabled.IsChecked = entity.IsEnabled;
            TextBoxSortOrder.Text = entity.SortOrder.ToString();

            TextBoxDescription.Text = entity.Description ?? string.Empty;
            TextBoxRemark.Text = entity.Remark ?? string.Empty;
        }

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            TextBlockError.Visibility = Visibility.Collapsed;
            TextBlockError.Text = string.Empty;

            // ── 校验 ─────────────────────────────────────────────────
            short cardId;
            if (!short.TryParse(TextBoxCardId.Text.Trim(), out cardId) || cardId < 0)
            {
                ShowError("硬件卡号必须为 0 ~ 32767 的整数。");
                return;
            }

            var name = TextBoxName.Text == null ? string.Empty : TextBoxName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                ShowError("内部名称不能为空。");
                return;
            }

            int cardType = GetComboBoxTagInt(ComboBoxCardType, 90);

            int coreNumber;
            if (!int.TryParse(TextBoxCoreNumber.Text.Trim(), out coreNumber) || coreNumber < 1)
            {
                ShowError("内核数量必须 ≥ 1。");
                return;
            }

            short axisCount;
            if (!short.TryParse(TextBoxAxisCountNumber.Text.Trim(), out axisCount) || axisCount < 0)
            {
                ShowError("支持轴总数必须为非负整数。");
                return;
            }

            short modeParam;
            if (!short.TryParse(TextBoxModeParam.Text.Trim(), out modeParam))
            {
                ShowError("打开模式参数格式无效，请输入整数。");
                return;
            }

            int initOrder;
            if (!int.TryParse(TextBoxInitOrder.Text.Trim(), out initOrder) || initOrder < 0)
            {
                ShowError("初始化顺序必须为非负整数。");
                return;
            }

            int sortOrder;
            if (!int.TryParse(TextBoxSortOrder.Text.Trim(), out sortOrder) || sortOrder < 0)
            {
                ShowError("排序号必须为非负整数。");
                return;
            }

            // ── 构建结果 ──────────────────────────────────────────────
            ResultEntity = new MotionCardEntity
            {
                CardId = cardId,
                CardType = cardType,
                Name = name,
                DisplayName = TextBoxDisplayName.Text == null ? null : TextBoxDisplayName.Text.Trim(),
                DriverKey = TextBoxDriverKey.Text == null ? null : TextBoxDriverKey.Text.Trim(),
                ModeParam = modeParam,
                OpenConfig = null,
                CoreNumber = coreNumber,
                AxisCountNumber = axisCount,
                UseExtModule = CheckBoxUseExtModule.IsChecked == true,
                InitOrder = initOrder,
                IsEnabled = CheckBoxIsEnabled.IsChecked == true,
                SortOrder = sortOrder,
                Description = TextBoxDescription.Text == null ? null : TextBoxDescription.Text.Trim(),
                Remark = TextBoxRemark.Text == null ? null : TextBoxRemark.Text.Trim(),
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };

            DialogResult = true;
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        // ── 辅助方法 ────────────────────────────────────────────────────

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

        private static int GetComboBoxTagInt(ComboBox comboBox, int defaultValue)
        {
            var item = comboBox.SelectedItem as ComboBoxItem;
            if (item == null || item.Tag == null)
            {
                return defaultValue;
            }

            int result;
            return int.TryParse(item.Tag.ToString(), out result) ? result : defaultValue;
        }
    }
}