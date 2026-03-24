using AM.Model.Entity.Motion.Actuator;
using System;
using System.Windows;
using System.Windows.Controls;

namespace AMControlWPF.Views.Config
{
    public partial class CylinderEditDialog : HandyControl.Controls.GlowWindow
    {
        private readonly int _originalId;

        public CylinderConfigEntity ResultEntity { get; private set; }

        public CylinderEditDialog(CylinderConfigEntity entity, bool isAdd)
        {
            InitializeComponent();
            _originalId = entity.Id;

            TextBlockTitle.Text = isAdd ? "新增气缸" : "编辑气缸";

            TextBoxName.Text = entity.Name ?? string.Empty;
            TextBoxName.IsReadOnly = !isAdd;
            TextBoxName.Opacity = isAdd ? 1.0 : 0.6;

            TextBoxDisplayName.Text = entity.DisplayName ?? string.Empty;
            SelectComboBoxByTag(ComboBoxDriveMode, entity.DriveMode ?? "Double");

            TextBoxExtendOutputBit.Text = entity.ExtendOutputBit > 0 ? entity.ExtendOutputBit.ToString() : string.Empty;
            TextBoxRetractOutputBit.Text = entity.RetractOutputBit.HasValue ? entity.RetractOutputBit.Value.ToString() : string.Empty;
            TextBoxExtendFeedbackBit.Text = entity.ExtendFeedbackBit.HasValue ? entity.ExtendFeedbackBit.Value.ToString() : string.Empty;
            TextBoxRetractFeedbackBit.Text = entity.RetractFeedbackBit.HasValue ? entity.RetractFeedbackBit.Value.ToString() : string.Empty;

            CheckBoxUseFeedbackCheck.IsChecked = entity.UseFeedbackCheck;
            TextBoxExtendTimeoutMs.Text = entity.ExtendTimeoutMs.ToString();
            TextBoxRetractTimeoutMs.Text = entity.RetractTimeoutMs.ToString();

            CheckBoxAllowBothOff.IsChecked = entity.AllowBothOff;
            CheckBoxAllowBothOn.IsChecked = entity.AllowBothOn;

            TextBoxSortOrder.Text = entity.SortOrder.ToString();
            CheckBoxIsEnabled.IsChecked = entity.IsEnabled;

            TextBoxDescription.Text = entity.Description ?? string.Empty;
            TextBoxRemark.Text = entity.Remark ?? string.Empty;

            UpdateRetractBitState();
        }

        private void ComboBoxDriveMode_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateRetractBitState();
        }

        private void UpdateRetractBitState()
        {
            if (TextBoxRetractOutputBit == null) return;

            var isSingle = GetComboBoxTagString(ComboBoxDriveMode, "Double") == "Single";
            TextBoxRetractOutputBit.IsEnabled = !isSingle;
            TextBoxRetractOutputBit.Opacity = isSingle ? 0.45 : 1.0;
            if (isSingle)
                TextBoxRetractOutputBit.Text = string.Empty;
        }

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            TextBlockError.Visibility = Visibility.Collapsed;

            var name = (TextBoxName.Text ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(name))
            {
                ShowError("内部名称不能为空。");
                return;
            }

            short extendBit;
            if (!short.TryParse(TextBoxExtendOutputBit.Text.Trim(), out extendBit) || extendBit <= 0)
            {
                ShowError("伸出输出位必须为正整数（DO 逻辑位号）。");
                return;
            }

            var driveMode = GetComboBoxTagString(ComboBoxDriveMode, "Double");
            var retractBit = ParseNullableShort(TextBoxRetractOutputBit.Text);

            if (string.Equals(driveMode, "Double", StringComparison.OrdinalIgnoreCase) && !retractBit.HasValue)
            {
                ShowError("双线圈气缸必须填写缩回输出位。");
                return;
            }

            int extendTimeout;
            int.TryParse(TextBoxExtendTimeoutMs.Text.Trim(), out extendTimeout);

            int retractTimeout;
            int.TryParse(TextBoxRetractTimeoutMs.Text.Trim(), out retractTimeout);

            int sortOrder;
            int.TryParse(TextBoxSortOrder.Text.Trim(), out sortOrder);

            ResultEntity = new CylinderConfigEntity
            {
                Id = _originalId,
                Name = name,
                DisplayName = (TextBoxDisplayName.Text ?? string.Empty).Trim(),
                DriveMode = driveMode,
                ExtendOutputBit = extendBit,
                RetractOutputBit = retractBit,
                ExtendFeedbackBit = ParseNullableShort(TextBoxExtendFeedbackBit.Text),
                RetractFeedbackBit = ParseNullableShort(TextBoxRetractFeedbackBit.Text),
                UseFeedbackCheck = CheckBoxUseFeedbackCheck.IsChecked == true,
                ExtendTimeoutMs = extendTimeout,
                RetractTimeoutMs = retractTimeout,
                AllowBothOff = CheckBoxAllowBothOff.IsChecked == true,
                AllowBothOn = CheckBoxAllowBothOn.IsChecked == true,
                IsEnabled = CheckBoxIsEnabled.IsChecked == true,
                SortOrder = sortOrder,
                Description = (TextBoxDescription.Text ?? string.Empty).Trim(),
                Remark = (TextBoxRemark.Text ?? string.Empty).Trim()
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
                comboBox.SelectedIndex = 0;
        }

        private static string GetComboBoxTagString(ComboBox comboBox, string defaultValue)
        {
            var item = comboBox.SelectedItem as ComboBoxItem;
            return item?.Tag?.ToString() ?? defaultValue;
        }

        private static short? ParseNullableShort(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;
            short val;
            return short.TryParse(text.Trim(), out val) ? val : (short?)null;
        }
    }
}