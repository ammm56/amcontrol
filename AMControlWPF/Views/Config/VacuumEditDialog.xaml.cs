using AM.Model.Entity.Motion.Actuator;
using AMControlWPF.Helpers;
using System.Windows;

namespace AMControlWPF.Views.Config
{
    public partial class VacuumEditDialog : HandyControl.Controls.GlowWindow
    {
        private readonly int _originalId;

        public VacuumConfigEntity ResultEntity { get; private set; }

        public VacuumEditDialog(VacuumConfigEntity entity, bool isAdd)
        {
            InitializeComponent();

            var e = entity ?? new VacuumConfigEntity
            {
                IsEnabled = true,
                VacuumBuildTimeoutMs = 3000,
                ReleaseTimeoutMs     = 1000
            };

            _originalId = isAdd ? 0 : e.Id;
            TextBlockTitle.Text = isAdd ? "新增真空" : "编辑真空";

            TextBoxName.Text = e.Name ?? string.Empty;
            TextBoxName.IsReadOnly = !isAdd;
            TextBoxName.Opacity    = isAdd ? 1.0 : 0.6;

            TextBoxDisplayName.Text = e.DisplayName ?? string.Empty;

            // 加载 IO 映射并绑定下拉列表
            var allIo      = IoDialogHelper.LoadAll();
            var doRequired = IoDialogHelper.BuildItems(allIo, "DO", nullable: false);
            var doOptional = IoDialogHelper.BuildItems(allIo, "DO", nullable: true);
            var diOptional = IoDialogHelper.BuildItems(allIo, "DI", nullable: true);

            IoDialogHelper.Apply(CbVacuumOnOutputBit,  doRequired, e.VacuumOnOutputBit > 0 ? (short?)e.VacuumOnOutputBit : null);
            IoDialogHelper.Apply(CbBlowOffOutputBit,   doOptional, e.BlowOffOutputBit);
            IoDialogHelper.Apply(CbVacuumFeedbackBit,  diOptional, e.VacuumFeedbackBit);
            IoDialogHelper.Apply(CbReleaseFeedbackBit, diOptional, e.ReleaseFeedbackBit);
            IoDialogHelper.Apply(CbWorkpiecePresentBit, diOptional, e.WorkpiecePresentBit);

            CheckBoxUseFeedbackCheck.IsChecked          = e.UseFeedbackCheck;
            CheckBoxUseWorkpieceCheck.IsChecked         = e.UseWorkpieceCheck;
            CheckBoxKeepVacuumOnAfterDetected.IsChecked = e.KeepVacuumOnAfterDetected;

            TextBoxVacuumBuildTimeoutMs.Text = e.VacuumBuildTimeoutMs.ToString();
            TextBoxReleaseTimeoutMs.Text     = e.ReleaseTimeoutMs.ToString();

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

            var vacuumOnBit = IoDialogHelper.GetValue(CbVacuumOnOutputBit);
            if (!vacuumOnBit.HasValue)
            {
                ShowError("请选择吸真空输出位 (DO)。");
                return;
            }

            int buildTimeout; int.TryParse(TextBoxVacuumBuildTimeoutMs.Text.Trim(), out buildTimeout);
            int releaseTimeout; int.TryParse(TextBoxReleaseTimeoutMs.Text.Trim(),   out releaseTimeout);
            int sortOrder;    int.TryParse(TextBoxSortOrder.Text.Trim(),            out sortOrder);

            ResultEntity = new VacuumConfigEntity
            {
                Id                       = _originalId,
                Name                     = name,
                DisplayName              = (TextBoxDisplayName.Text ?? string.Empty).Trim(),
                VacuumOnOutputBit        = vacuumOnBit.Value,
                BlowOffOutputBit         = IoDialogHelper.GetValue(CbBlowOffOutputBit),
                VacuumFeedbackBit        = IoDialogHelper.GetValue(CbVacuumFeedbackBit),
                ReleaseFeedbackBit       = IoDialogHelper.GetValue(CbReleaseFeedbackBit),
                WorkpiecePresentBit      = IoDialogHelper.GetValue(CbWorkpiecePresentBit),
                UseFeedbackCheck         = CheckBoxUseFeedbackCheck.IsChecked          == true,
                UseWorkpieceCheck        = CheckBoxUseWorkpieceCheck.IsChecked         == true,
                KeepVacuumOnAfterDetected = CheckBoxKeepVacuumOnAfterDetected.IsChecked == true,
                VacuumBuildTimeoutMs     = buildTimeout,
                ReleaseTimeoutMs         = releaseTimeout,
                IsEnabled                = CheckBoxIsEnabled.IsChecked == true,
                SortOrder                = sortOrder,
                Description              = (TextBoxDescription.Text ?? string.Empty).Trim(),
                Remark                   = (TextBoxRemark.Text      ?? string.Empty).Trim()
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