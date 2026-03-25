using AM.Model.Entity.Motion.Actuator;
using AMControlWPF.Helpers;
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

            var e = entity ?? new CylinderConfigEntity
            {
                IsEnabled = true,
                DriveMode = "Double",
                ExtendTimeoutMs = 3000,
                RetractTimeoutMs = 3000
            };

            _originalId = isAdd ? 0 : e.Id;
            TextBlockTitle.Text = isAdd ? "新增气缸" : "编辑气缸";

            TextBoxName.Text = e.Name ?? string.Empty;
            TextBoxName.IsReadOnly = !isAdd;
            TextBoxName.Opacity = isAdd ? 1.0 : 0.6;

            TextBoxDisplayName.Text = e.DisplayName ?? string.Empty;
            SelectComboBoxByTag(ComboBoxDriveMode, e.DriveMode ?? "Double");

            // 加载 IO 映射并绑定下拉列表
            var allIo      = IoDialogHelper.LoadAll();
            var doRequired = IoDialogHelper.BuildItems(allIo, "DO", nullable: false);
            var doOptional = IoDialogHelper.BuildItems(allIo, "DO", nullable: true);
            var diOptional = IoDialogHelper.BuildItems(allIo, "DI", nullable: true);

            IoDialogHelper.Apply(CbExtendOutputBit,  doRequired, e.ExtendOutputBit > 0 ? (short?)e.ExtendOutputBit : null);
            IoDialogHelper.Apply(CbRetractOutputBit, doOptional, e.RetractOutputBit);
            IoDialogHelper.Apply(CbExtendFeedbackBit,  diOptional, e.ExtendFeedbackBit);
            IoDialogHelper.Apply(CbRetractFeedbackBit, diOptional, e.RetractFeedbackBit);

            CheckBoxUseFeedbackCheck.IsChecked = e.UseFeedbackCheck;
            TextBoxExtendTimeoutMs.Text  = e.ExtendTimeoutMs.ToString();
            TextBoxRetractTimeoutMs.Text = e.RetractTimeoutMs.ToString();

            CheckBoxAllowBothOff.IsChecked = e.AllowBothOff;
            CheckBoxAllowBothOn.IsChecked  = e.AllowBothOn;

            TextBoxSortOrder.Text          = e.SortOrder.ToString();
            CheckBoxIsEnabled.IsChecked    = e.IsEnabled;
            TextBoxDescription.Text        = e.Description ?? string.Empty;
            TextBoxRemark.Text             = e.Remark      ?? string.Empty;

            UpdateRetractBitState();
        }

        private void ComboBoxDriveMode_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateRetractBitState();
        }

        private void UpdateRetractBitState()
        {
            if (CbRetractOutputBit == null) return;
            var isSingle = GetComboBoxTagString(ComboBoxDriveMode, "Double") == "Single";
            CbRetractOutputBit.IsEnabled = !isSingle;
            CbRetractOutputBit.Opacity   = isSingle ? 0.45 : 1.0;
            if (isSingle && CbRetractOutputBit.Items.Count > 0)
                CbRetractOutputBit.SelectedIndex = 0; // 选中「不配置」
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

            var extendBit = IoDialogHelper.GetValue(CbExtendOutputBit);
            if (!extendBit.HasValue)
            {
                ShowError("请选择伸出输出位 (DO)。");
                return;
            }

            var driveMode  = GetComboBoxTagString(ComboBoxDriveMode, "Double");
            var retractBit = IoDialogHelper.GetValue(CbRetractOutputBit);

            if (string.Equals(driveMode, "Double", StringComparison.OrdinalIgnoreCase) && !retractBit.HasValue)
            {
                ShowError("双线圈气缸必须选择缩回输出位。");
                return;
            }

            int extendTimeout;  int.TryParse(TextBoxExtendTimeoutMs.Text.Trim(),  out extendTimeout);
            int retractTimeout; int.TryParse(TextBoxRetractTimeoutMs.Text.Trim(), out retractTimeout);
            int sortOrder;      int.TryParse(TextBoxSortOrder.Text.Trim(),        out sortOrder);

            ResultEntity = new CylinderConfigEntity
            {
                Id               = _originalId,
                Name             = name,
                DisplayName      = (TextBoxDisplayName.Text ?? string.Empty).Trim(),
                DriveMode        = driveMode,
                ExtendOutputBit  = extendBit.Value,
                RetractOutputBit = retractBit,
                ExtendFeedbackBit  = IoDialogHelper.GetValue(CbExtendFeedbackBit),
                RetractFeedbackBit = IoDialogHelper.GetValue(CbRetractFeedbackBit),
                UseFeedbackCheck = CheckBoxUseFeedbackCheck.IsChecked == true,
                ExtendTimeoutMs  = extendTimeout,
                RetractTimeoutMs = retractTimeout,
                AllowBothOff     = CheckBoxAllowBothOff.IsChecked == true,
                AllowBothOn      = CheckBoxAllowBothOn.IsChecked  == true,
                IsEnabled        = CheckBoxIsEnabled.IsChecked    == true,
                SortOrder        = sortOrder,
                Description      = (TextBoxDescription.Text ?? string.Empty).Trim(),
                Remark           = (TextBoxRemark.Text      ?? string.Empty).Trim()
            };

            DialogResult = true;
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e) => DialogResult = false;

        private void ShowError(string message)
        {
            TextBlockError.Text       = message;
            TextBlockError.Visibility = Visibility.Visible;
        }

        private static void SelectComboBoxByTag(ComboBox cb, string tag)
        {
            foreach (ComboBoxItem item in cb.Items)
            {
                if (item.Tag != null && item.Tag.ToString() == tag)
                {
                    cb.SelectedItem = item;
                    return;
                }
            }
            if (cb.Items.Count > 0) cb.SelectedIndex = 0;
        }

        private static string GetComboBoxTagString(ComboBox cb, string defaultValue)
        {
            var item = cb.SelectedItem as ComboBoxItem;
            return item?.Tag?.ToString() ?? defaultValue;
        }
    }
}