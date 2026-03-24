using AM.Model.Entity.Motion.Actuator;
using System;
using System.Windows;
using System.Windows.Controls;

namespace AMControlWPF.Views.Config
{
    public partial class GripperEditDialog : HandyControl.Controls.GlowWindow
    {
        private readonly int _originalId;

        public GripperConfigEntity ResultEntity { get; private set; }

        public GripperEditDialog(GripperConfigEntity entity, bool isAdd)
        {
            InitializeComponent();

            var e = entity ?? new GripperConfigEntity
            {
                IsEnabled = true,
                DriveMode = "Double",
                CloseTimeoutMs = 3000,
                OpenTimeoutMs = 3000
            };

            _originalId = isAdd ? 0 : e.Id;
            TextBlockTitle.Text = isAdd ? "新增夹爪" : "编辑夹爪";

            TextBoxName.Text = e.Name ?? string.Empty;
            TextBoxName.IsReadOnly = !isAdd;
            TextBoxName.Opacity = isAdd ? 1.0 : 0.6;

            TextBoxDisplayName.Text = e.DisplayName ?? string.Empty;
            SelectComboBoxByTag(ComboBoxDriveMode, e.DriveMode ?? "Double");

            TextBoxCloseOutputBit.Text = e.CloseOutputBit.ToString();
            TextBoxOpenOutputBit.Text = e.OpenOutputBit.HasValue ? e.OpenOutputBit.Value.ToString() : string.Empty;
            TextBoxCloseFeedbackBit.Text = e.CloseFeedbackBit.HasValue ? e.CloseFeedbackBit.Value.ToString() : string.Empty;
            TextBoxOpenFeedbackBit.Text = e.OpenFeedbackBit.HasValue ? e.OpenFeedbackBit.Value.ToString() : string.Empty;
            TextBoxWorkpiecePresentBit.Text = e.WorkpiecePresentBit.HasValue ? e.WorkpiecePresentBit.Value.ToString() : string.Empty;

            CheckBoxUseFeedbackCheck.IsChecked = e.UseFeedbackCheck;
            CheckBoxUseWorkpieceCheck.IsChecked = e.UseWorkpieceCheck;

            TextBoxCloseTimeoutMs.Text = e.CloseTimeoutMs.ToString();
            TextBoxOpenTimeoutMs.Text = e.OpenTimeoutMs.ToString();

            CheckBoxAllowBothOff.IsChecked = e.AllowBothOff;
            CheckBoxAllowBothOn.IsChecked = e.AllowBothOn;

            TextBoxSortOrder.Text = e.SortOrder.ToString();
            CheckBoxIsEnabled.IsChecked = e.IsEnabled;

            TextBoxDescription.Text = e.Description ?? string.Empty;
            TextBoxRemark.Text = e.Remark ?? string.Empty;

            UpdateOpenBitState();
        }

        private void ComboBoxDriveMode_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateOpenBitState();
        }

        private void UpdateOpenBitState()
        {
            if (TextBoxOpenOutputBit == null) return;

            var isSingle = GetComboBoxTagString(ComboBoxDriveMode, "Double") == "Single";
            TextBoxOpenOutputBit.IsEnabled = !isSingle;
            TextBoxOpenOutputBit.Opacity = isSingle ? 0.45 : 1.0;
            if (isSingle)
                TextBoxOpenOutputBit.Text = string.Empty;
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

            short closeBit;
            if (!short.TryParse(TextBoxCloseOutputBit.Text.Trim(), out closeBit) || closeBit <= 0)
            {
                ShowError("夹紧输出位必须为正整数（DO 逻辑位号）。");
                return;
            }

            var driveMode = GetComboBoxTagString(ComboBoxDriveMode, "Double");
            var openBit = ParseNullableShort(TextBoxOpenOutputBit.Text);

            if (string.Equals(driveMode, "Double", StringComparison.OrdinalIgnoreCase) && !openBit.HasValue)
            {
                ShowError("双线圈夹爪必须填写打开输出位。");
                return;
            }

            int closeTimeout;
            int.TryParse(TextBoxCloseTimeoutMs.Text.Trim(), out closeTimeout);

            int openTimeout;
            int.TryParse(TextBoxOpenTimeoutMs.Text.Trim(), out openTimeout);

            int sortOrder;
            int.TryParse(TextBoxSortOrder.Text.Trim(), out sortOrder);

            ResultEntity = new GripperConfigEntity
            {
                Id = _originalId,
                Name = name,
                DisplayName = (TextBoxDisplayName.Text ?? string.Empty).Trim(),
                DriveMode = driveMode,
                CloseOutputBit = closeBit,
                OpenOutputBit = openBit,
                CloseFeedbackBit = ParseNullableShort(TextBoxCloseFeedbackBit.Text),
                OpenFeedbackBit = ParseNullableShort(TextBoxOpenFeedbackBit.Text),
                WorkpiecePresentBit = ParseNullableShort(TextBoxWorkpiecePresentBit.Text),
                UseFeedbackCheck = CheckBoxUseFeedbackCheck.IsChecked == true,
                UseWorkpieceCheck = CheckBoxUseWorkpieceCheck.IsChecked == true,
                CloseTimeoutMs = closeTimeout,
                OpenTimeoutMs = openTimeout,
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