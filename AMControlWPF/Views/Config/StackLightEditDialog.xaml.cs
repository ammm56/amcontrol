using AM.Model.Entity.Motion.Actuator;
using System.Windows;
using System.Windows.Controls;

namespace AMControlWPF.Views.Config
{
    public partial class StackLightEditDialog : HandyControl.Controls.GlowWindow
    {
        private readonly int _originalId;

        public StackLightConfigEntity ResultEntity { get; private set; }

        public StackLightEditDialog(StackLightConfigEntity entity, bool isAdd)
        {
            InitializeComponent();

            var e = entity ?? new StackLightConfigEntity { IsEnabled = true };

            _originalId = isAdd ? 0 : e.Id;
            TextBlockTitle.Text = isAdd ? "新增灯塔" : "编辑灯塔";

            TextBoxName.Text = e.Name ?? string.Empty;
            TextBoxName.IsReadOnly = !isAdd;
            TextBoxName.Opacity = isAdd ? 1.0 : 0.6;

            TextBoxDisplayName.Text = e.DisplayName ?? string.Empty;

            TextBoxRedOutputBit.Text = e.RedOutputBit.HasValue ? e.RedOutputBit.Value.ToString() : string.Empty;
            TextBoxYellowOutputBit.Text = e.YellowOutputBit.HasValue ? e.YellowOutputBit.Value.ToString() : string.Empty;
            TextBoxGreenOutputBit.Text = e.GreenOutputBit.HasValue ? e.GreenOutputBit.Value.ToString() : string.Empty;
            TextBoxBlueOutputBit.Text = e.BlueOutputBit.HasValue ? e.BlueOutputBit.Value.ToString() : string.Empty;
            TextBoxBuzzerOutputBit.Text = e.BuzzerOutputBit.HasValue ? e.BuzzerOutputBit.Value.ToString() : string.Empty;

            CheckBoxEnableBuzzerOnWarning.IsChecked = e.EnableBuzzerOnWarning;
            CheckBoxEnableBuzzerOnAlarm.IsChecked = e.EnableBuzzerOnAlarm;
            CheckBoxAllowMultiSegmentOn.IsChecked = e.AllowMultiSegmentOn;

            TextBoxSortOrder.Text = e.SortOrder.ToString();
            CheckBoxIsEnabled.IsChecked = e.IsEnabled;

            TextBoxDescription.Text = e.Description ?? string.Empty;
            TextBoxRemark.Text = e.Remark ?? string.Empty;
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

            int sortOrder;
            int.TryParse(TextBoxSortOrder.Text.Trim(), out sortOrder);

            ResultEntity = new StackLightConfigEntity
            {
                Id = _originalId,
                Name = name,
                DisplayName = (TextBoxDisplayName.Text ?? string.Empty).Trim(),
                RedOutputBit = ParseNullableShort(TextBoxRedOutputBit.Text),
                YellowOutputBit = ParseNullableShort(TextBoxYellowOutputBit.Text),
                GreenOutputBit = ParseNullableShort(TextBoxGreenOutputBit.Text),
                BlueOutputBit = ParseNullableShort(TextBoxBlueOutputBit.Text),
                BuzzerOutputBit = ParseNullableShort(TextBoxBuzzerOutputBit.Text),
                EnableBuzzerOnWarning = CheckBoxEnableBuzzerOnWarning.IsChecked == true,
                EnableBuzzerOnAlarm = CheckBoxEnableBuzzerOnAlarm.IsChecked == true,
                AllowMultiSegmentOn = CheckBoxAllowMultiSegmentOn.IsChecked == true,
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

        private static short? ParseNullableShort(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;
            short val;
            return short.TryParse(text.Trim(), out val) ? val : (short?)null;
        }
    }
}