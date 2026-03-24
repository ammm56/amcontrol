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
            _originalId = entity.Id;

            TextBlockTitle.Text = isAdd ? "新增灯塔" : "编辑灯塔";

            TextBoxName.Text = entity.Name ?? string.Empty;
            TextBoxName.IsReadOnly = !isAdd;
            TextBoxName.Opacity = isAdd ? 1.0 : 0.6;

            TextBoxDisplayName.Text = entity.DisplayName ?? string.Empty;

            TextBoxRedOutputBit.Text = entity.RedOutputBit.HasValue ? entity.RedOutputBit.Value.ToString() : string.Empty;
            TextBoxYellowOutputBit.Text = entity.YellowOutputBit.HasValue ? entity.YellowOutputBit.Value.ToString() : string.Empty;
            TextBoxGreenOutputBit.Text = entity.GreenOutputBit.HasValue ? entity.GreenOutputBit.Value.ToString() : string.Empty;
            TextBoxBlueOutputBit.Text = entity.BlueOutputBit.HasValue ? entity.BlueOutputBit.Value.ToString() : string.Empty;
            TextBoxBuzzerOutputBit.Text = entity.BuzzerOutputBit.HasValue ? entity.BuzzerOutputBit.Value.ToString() : string.Empty;

            CheckBoxEnableBuzzerOnWarning.IsChecked = entity.EnableBuzzerOnWarning;
            CheckBoxEnableBuzzerOnAlarm.IsChecked = entity.EnableBuzzerOnAlarm;
            CheckBoxAllowMultiSegmentOn.IsChecked = entity.AllowMultiSegmentOn;

            TextBoxSortOrder.Text = entity.SortOrder.ToString();
            CheckBoxIsEnabled.IsChecked = entity.IsEnabled;

            TextBoxDescription.Text = entity.Description ?? string.Empty;
            TextBoxRemark.Text = entity.Remark ?? string.Empty;
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