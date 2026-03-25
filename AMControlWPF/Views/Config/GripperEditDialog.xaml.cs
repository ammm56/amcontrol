using AM.Model.Entity.Motion.Actuator;
using AMControlWPF.Helpers;
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

            // 加载 IO 映射并绑定下拉列表
            var allIo = IoDialogHelper.LoadAll();
            var doRequired = IoDialogHelper.BuildItems(allIo, "DO", nullable: false);
            var doOptional = IoDialogHelper.BuildItems(allIo, "DO", nullable: true);
            var diOptional = IoDialogHelper.BuildItems(allIo, "DI", nullable: true);

            IoDialogHelper.Apply(CbCloseOutputBit, doRequired, e.CloseOutputBit > 0 ? (short?)e.CloseOutputBit : null);
            IoDialogHelper.Apply(CbOpenOutputBit, doOptional, e.OpenOutputBit);
            IoDialogHelper.Apply(CbCloseFeedbackBit, diOptional, e.CloseFeedbackBit);
            IoDialogHelper.Apply(CbOpenFeedbackBit, diOptional, e.OpenFeedbackBit);
            IoDialogHelper.Apply(CbWorkpiecePresentBit, diOptional, e.WorkpiecePresentBit);

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
            if (CbOpenOutputBit == null) return;
            var isSingle = GetComboBoxTagString(ComboBoxDriveMode, "Double") == "Single";
            CbOpenOutputBit.IsEnabled = !isSingle;
            CbOpenOutputBit.Opacity = isSingle ? 0.45 : 1.0;
            if (isSingle && CbOpenOutputBit.Items.Count > 0)
                CbOpenOutputBit.SelectedIndex = 0; // 选中「不配置」
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

            var closeBit = IoDialogHelper.GetValue(CbCloseOutputBit);
            if (!closeBit.HasValue)
            {
                ShowError("请选择夹紧输出位 (DO)。");
                return;
            }

            var driveMode = GetComboBoxTagString(ComboBoxDriveMode, "Double");
            var openBit = IoDialogHelper.GetValue(CbOpenOutputBit);

            if (string.Equals(driveMode, "Double", StringComparison.OrdinalIgnoreCase) && !openBit.HasValue)
            {
                ShowError("双线圈夹爪必须选择打开输出位。");
                return;
            }

            int closeTimeout; int.TryParse(TextBoxCloseTimeoutMs.Text.Trim(), out closeTimeout);
            int openTimeout; int.TryParse(TextBoxOpenTimeoutMs.Text.Trim(), out openTimeout);
            int sortOrder; int.TryParse(TextBoxSortOrder.Text.Trim(), out sortOrder);

            ResultEntity = new GripperConfigEntity
            {
                Id = _originalId,
                Name = name,
                DisplayName = (TextBoxDisplayName.Text ?? string.Empty).Trim(),
                DriveMode = driveMode,
                CloseOutputBit = closeBit.Value,
                OpenOutputBit = openBit,
                CloseFeedbackBit = IoDialogHelper.GetValue(CbCloseFeedbackBit),
                OpenFeedbackBit = IoDialogHelper.GetValue(CbOpenFeedbackBit),
                WorkpiecePresentBit = IoDialogHelper.GetValue(CbWorkpiecePresentBit),
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

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e) => DialogResult = false;

        private void ShowError(string message)
        {
            TextBlockError.Text = message;
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