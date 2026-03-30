using AM.PageModel.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMControlWinF.Views.Auth
{
    public partial class LoginForm : Form
    {
        private readonly LoginPageModel _model;
        private readonly BindingSource _bindingSource;

        public LoginForm()
        {
            InitializeComponent();

            _model = new LoginPageModel();
            _bindingSource = new BindingSource();

            InitializeBindings();
            BindEvents();
        }

        private void InitializeBindings()
        {
            _bindingSource.DataSource = _model;

            textBoxLoginName.DataBindings.Add("Text", _bindingSource, "LoginName", false, DataSourceUpdateMode.OnPropertyChanged);
            labelStatusValue.DataBindings.Add("Text", _bindingSource, "StatusText", false, DataSourceUpdateMode.OnPropertyChanged);
            labelErrorValue.DataBindings.Add("Text", _bindingSource, "ErrorMessage", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void BindEvents()
        {
            buttonLogin.Click += async (sender, e) => await LoginAsync();
            buttonCancel.Click += (sender, e) => Close();

            textBoxPassword.TextChanged += (sender, e) =>
            {
                _model.Password = textBoxPassword.Text;
            };

            Shown += LoginForm_Shown;
            FormClosing += LoginForm_FormClosing;
            AcceptButton = buttonLogin;
            CancelButton = buttonCancel;
        }

        private void LoginForm_Shown(object sender, EventArgs e)
        {
            textBoxLoginName.Focus();
            textBoxLoginName.SelectAll();
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != DialogResult.OK)
            {
                DialogResult = DialogResult.Cancel;
            }
        }

        private async Task LoginAsync()
        {
            SetBusyState(true);

            try
            {
                var success = await _model.LoginAsync();
                if (!success)
                {
                    return;
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private void SetBusyState(bool isBusy)
        {
            textBoxLoginName.Enabled = !isBusy;
            textBoxPassword.Enabled = !isBusy;
            buttonLogin.Enabled = !isBusy;
            buttonCancel.Enabled = !isBusy;
        }












    }
}
