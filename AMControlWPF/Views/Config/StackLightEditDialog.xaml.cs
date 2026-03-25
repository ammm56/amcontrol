using AM.Model.Entity.Motion.Actuator;
using AMControlWPF.Helpers;
using System.Windows;

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
            TextBoxName.Opacity    = isAdd ? 1.0 : 0.6;

            TextBoxDisplayName.Text = e.DisplayName ?? string.Empty;

            // 加载 IO 映射并绑定下拉列表（灯塔全为 DO 可空点位）
            var allIo      = IoDialogHelper.LoadAll();
            var doOptional = IoDialogHelper.BuildItems(allIo, "DO", nullable: true);

            IoDialogHelper.Apply(CbRedOutputBit,    doOptional, e.RedOutputBit);
            IoDialogHelper.Apply(CbYellowOutputBit, doOptional, e.YellowOutputBit);
            IoDialogHelper.Apply(CbGreenOutputBit,  doOptional, e.GreenOutputBit);
            IoDialogHelper.Apply(CbBlueOutputBit,   doOptional, e.BlueOutputBit);
            IoDialogHelper.Apply(CbBuzzerOutputBit, doOptional, e.BuzzerOutputBit);

            CheckBoxEnableBuzzerOnWarning.IsChecked = e.EnableBuzzerOnWarning;
            CheckBoxEnableBuzzerOnAlarm.IsChecked   = e.EnableBuzzerOnAlarm;
            CheckBoxAllowMultiSegmentOn.IsChecked   = e.AllowMultiSegmentOn;

            TextBoxSortOrder.Text       = e.SortOrder.ToString();
            CheckBoxIsEnabled.IsChecked = e.IsEnabled;
            TextBoxDescription.Text     = e.Description ?? string.Empty;
            TextBoxRemark.Text          = e.Remark      ?? string.Empty;
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

            int sortOrder; int.TryParse(TextBoxSortOrder.Text.Trim(), out sortOrder);

            ResultEntity = new StackLightConfigEntity
            {
                Id                    = _originalId,
                Name                  = name,
                DisplayName           = (TextBoxDisplayName.Text ?? string.Empty).Trim(),
                RedOutputBit          = IoDialogHelper.GetValue(CbRedOutputBit),
                YellowOutputBit       = IoDialogHelper.GetValue(CbYellowOutputBit),
                GreenOutputBit        = IoDialogHelper.GetValue(CbGreenOutputBit),
                BlueOutputBit         = IoDialogHelper.GetValue(CbBlueOutputBit),
                BuzzerOutputBit       = IoDialogHelper.GetValue(CbBuzzerOutputBit),
                EnableBuzzerOnWarning = CheckBoxEnableBuzzerOnWarning.IsChecked == true,
                EnableBuzzerOnAlarm   = CheckBoxEnableBuzzerOnAlarm.IsChecked   == true,
                AllowMultiSegmentOn   = CheckBoxAllowMultiSegmentOn.IsChecked   == true,
                IsEnabled             = CheckBoxIsEnabled.IsChecked == true,
                SortOrder             = sortOrder,
                Description           = (TextBoxDescription.Text ?? string.Empty).Trim(),
                Remark                = (TextBoxRemark.Text      ?? string.Empty).Trim()
            };

            DialogResult = true;
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e) => DialogResult = false;

        private void ShowError(string message)
        {
            TextBlockError.Text       = message;
            TextBlockError.Visibility = Visibility.Visible;
        }
    }
}