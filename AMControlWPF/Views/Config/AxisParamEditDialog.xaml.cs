using AM.ViewModel.ViewModels.Config;
using System.Windows;

namespace AMControlWPF.Views.Config
{
    public partial class AxisParamEditDialog : HandyControl.Controls.GlowWindow
    {
        private readonly ConfigAxisArgViewModel _param;

        public AxisParamEditDialog(ConfigAxisArgViewModel param)
        {
            InitializeComponent();
            _param = param;
            DataContext = param;

            TextBlockCurrentValue.Text = param.EditValue;
            TextBoxNewValue.Text = param.EditValue;

            Loaded += (s, e) =>
            {
                TextBoxNewValue.Focus();
                TextBoxNewValue.SelectAll();
            };
        }

        private void ButtonConfirm_OnClick(object sender, RoutedEventArgs e)
        {
            HideError();

            double parsedValue;
            string errorMessage;

            if (!_param.ValidateInputValue(TextBoxNewValue.Text, out parsedValue, out errorMessage))
            {
                ShowError(errorMessage);
                return;
            }

            _param.EditValue = TextBoxNewValue.Text.Trim();
            DialogResult = true;
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void ShowError(string message)
        {
            TextBlockError.Text = message;
            BorderError.Visibility = Visibility.Visible;
        }

        private void HideError()
        {
            TextBlockError.Text = string.Empty;
            BorderError.Visibility = Visibility.Collapsed;
        }
    }
}