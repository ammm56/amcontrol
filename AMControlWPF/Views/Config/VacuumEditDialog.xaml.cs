using AM.Model.Entity.Motion.Actuator;
using System.Windows;
using System.Windows.Controls;

namespace AMControlWPF.Views.Config
{
    public partial class VacuumEditDialog : HandyControl.Controls.GlowWindow
    {
        private readonly int _originalId;

        public VacuumConfigEntity ResultEntity { get; private set; }

        public VacuumEditDialog(VacuumConfigEntity entity, bool isAdd)
        {
            InitializeComponent();
            _originalId = entity.Id;

            TextBlockTitle.Text = isAdd ? "新增真空" : "编辑真空";

            TextBoxName.Text = entity.Name ?? string.Empty;
            TextBoxName.IsReadOnly = !isAdd;
            TextBoxName.Opacity = isAdd ? 1.0 : 0.6;

            TextBoxDisplayName.Text = entity.DisplayName ?? string.Empty;

            TextBoxVacuumOnOutputBit.Text = entity.VacuumOnOutputBit > 0 ? entity.VacuumOnOutputBit.ToString() : string.Empty;
            TextBoxBlowOffOutputBit.Text = entity.BlowOffOutputBit.HasValue ? entity.BlowOffOutputBit.Value.ToString() : string.Empty;
            TextBoxVacuumFeedbackBit.Text = entity.VacuumFeedbackBit.HasValue ? entity.VacuumFeedbackBit.Value.ToString() : string.Empty;
            TextBoxReleaseFeedbackBit.Text = entity.ReleaseFeedbackBit.HasValue ? entity.ReleaseFeedbackBit.Value.ToString() : string.Empty;
            TextBoxWorkpiecePresentBit.Text = entity.WorkpiecePresentBit.HasValue ? entity.WorkpiecePresentBit.Value.ToString() : string.Empty;

            CheckBoxUseFeedbackCheck.IsChecked = entity.UseFeedbackCheck;
            CheckBoxUseWorkpieceCheck.IsChecked = entity.UseWorkpieceCheck;
            CheckBoxKeepVacuumOnAfterDetected.IsChecked = entity.KeepVacuumOnAfterDetected;

            TextBoxVacuumBuildTimeoutMs.Text = entity.VacuumBuildTimeoutMs.ToString();
            TextBoxReleaseTimeoutMs.Text = entity.ReleaseTimeoutMs.ToString();

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

            short vacuumOnBit;
            if (!short.TryParse(TextBoxVacuumOnOutputBit.Text.Trim(), out vacuumOnBit) || vacuumOnBit <= 0)
            {
                ShowError("吸真空输出位必须为正整数（DO 逻辑位号）。");
                return;
            }

            int buildTimeout;
            int.TryParse(TextBoxVacuumBuildTimeoutMs.Text.Trim(), out buildTimeout);

            int releaseTimeout;
            int.TryParse(TextBoxReleaseTimeoutMs.Text.Trim(), out releaseTimeout);

            int sortOrder;
            int.TryParse(TextBoxSortOrder.Text.Trim(), out sortOrder);

            ResultEntity = new VacuumConfigEntity
            {
                Id = _originalId,
                Name = name,
                DisplayName = (TextBoxDisplayName.Text ?? string.Empty).Trim(),
                VacuumOnOutputBit = vacuumOnBit,
                BlowOffOutputBit = ParseNullableShort(TextBoxBlowOffOutputBit.Text),
                VacuumFeedbackBit = ParseNullableShort(TextBoxVacuumFeedbackBit.Text),
                ReleaseFeedbackBit = ParseNullableShort(TextBoxReleaseFeedbackBit.Text),
                WorkpiecePresentBit = ParseNullableShort(TextBoxWorkpiecePresentBit.Text),
                UseFeedbackCheck = CheckBoxUseFeedbackCheck.IsChecked == true,
                UseWorkpieceCheck = CheckBoxUseWorkpieceCheck.IsChecked == true,
                KeepVacuumOnAfterDetected = CheckBoxKeepVacuumOnAfterDetected.IsChecked == true,
                VacuumBuildTimeoutMs = buildTimeout,
                ReleaseTimeoutMs = releaseTimeout,
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