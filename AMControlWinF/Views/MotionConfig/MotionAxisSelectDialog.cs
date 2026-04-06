using AM.Core.Context;
using AM.PageModel.MotionConfig;
using AMControlWinF.Tools;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMControlWinF.Views.MotionConfig
{
    /// <summary>
    /// 轴选择对话框。
    /// </summary>
    public partial class MotionAxisSelectDialog : AntdUI.Window
    {
        private readonly MotionAxisManagementPageModel _model;
        private readonly short? _initialSelectedLogicalAxis;
        private bool _isFirstLoad;
        private short? _selectedLogicalAxis;

        public MotionAxisSelectDialog()
            : this(null)
        {
        }

        public MotionAxisSelectDialog(short? selectedLogicalAxis)
        {
            InitializeComponent();

            _model = new MotionAxisManagementPageModel();
            _initialSelectedLogicalAxis = selectedLogicalAxis;
            _selectedLogicalAxis = selectedLogicalAxis;

            BindEvents();
            ApplyThemeFromConfig();
            UpdateSelectionUi();
        }

        public short SelectedLogicalAxis { get; private set; }

        private void BindEvents()
        {
            Load += MotionAxisSelectDialog_Load;
            buttonRefresh.Click += async (s, e) => await ReloadAsync();
            buttonOk.Click += ButtonOk_Click;
            buttonCancel.Click += ButtonCancel_Click;

            KeyPreview = true;
            KeyDown += MotionAxisSelectDialog_KeyDown;
        }

        private async void MotionAxisSelectDialog_Load(object sender, EventArgs e)
        {
            if (_isFirstLoad)
                return;

            _isFirstLoad = true;
            await ReloadAsync();
        }

        private async Task ReloadAsync()
        {
            buttonRefresh.Enabled = false;
            try
            {
                await _model.LoadAsync();
                _model.ChangePage(1, 9999);

                if (!_selectedLogicalAxis.HasValue && _initialSelectedLogicalAxis.HasValue)
                {
                    _selectedLogicalAxis = _initialSelectedLogicalAxis;
                }

                BuildCards();
                UpdateSelectionUi();
            }
            finally
            {
                buttonRefresh.Enabled = true;
            }
        }

        private void BuildCards()
        {
            flowCards.SuspendLayout();
            try
            {
                ControlDisposeHelper.ClearControlsSafely(flowCards);

                foreach (var item in _model.Items)
                {
                    var wrapper = CreateCardWrapper(item);
                    flowCards.Controls.Add(wrapper);
                }
            }
            finally
            {
                flowCards.ResumeLayout();
            }
        }

        private AntdUI.Panel CreateCardWrapper(MotionAxisManagementPageModel.MotionAxisViewItem item)
        {
            var wrapper = new AntdUI.Panel();
            var card = new MotionAxisCardControl();
            var clickHandler = new EventHandler((s, e) => SelectAxis(item.LogicalAxis));

            wrapper.Margin = new Padding(0);
            wrapper.Padding = new Padding(2);
            wrapper.Size = new Size(250, 176);
            wrapper.Radius = 12;
            wrapper.BorderWidth = 1F;
            wrapper.Tag = item.LogicalAxis;

            card.Dock = DockStyle.Fill;
            card.Margin = new Padding(0);
            card.Bind(item);

            BindClickRecursive(wrapper, clickHandler);
            BindClickRecursive(card, clickHandler);

            wrapper.Controls.Add(card);
            return wrapper;
        }

        private void BindClickRecursive(Control control, EventHandler handler)
        {
            if (control == null || handler == null)
                return;

            control.Click += handler;

            foreach (Control child in control.Controls)
            {
                BindClickRecursive(child, handler);
            }
        }

        private void SelectAxis(short logicalAxis)
        {
            _selectedLogicalAxis = logicalAxis;
            UpdateSelectionUi();
        }

        private void UpdateSelectionUi()
        {
            if (_selectedLogicalAxis.HasValue)
            {
                var selected = _model.Items.FirstOrDefault(x => x.LogicalAxis == _selectedLogicalAxis.Value);
                if (selected != null)
                {
                    labelCurrentSelection.Text = "当前选择：" + selected.DisplayName + " (L" + selected.LogicalAxis + ")";
                }
                else
                {
                    labelCurrentSelection.Text = "当前选择：逻辑轴 L" + _selectedLogicalAxis.Value;
                }
            }
            else
            {
                labelCurrentSelection.Text = "当前选择：未选择轴";
            }

            buttonOk.Enabled = _selectedLogicalAxis.HasValue;

            foreach (var wrapper in flowCards.Controls.OfType<AntdUI.Panel>())
            {
                var logicalAxis = wrapper.Tag is short ? (short)wrapper.Tag : (short)0;
                var selected = _selectedLogicalAxis.HasValue && _selectedLogicalAxis.Value == logicalAxis;

                wrapper.BorderWidth = selected ? 2F : 0F;
                wrapper.BorderColor = selected
                    ? Color.FromArgb(22, 119, 255)
                    : Color.FromArgb(225, 229, 235);
                wrapper.Shadow = selected ? 6 : 0;
            }
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            if (!_selectedLogicalAxis.HasValue)
                return;

            SelectedLogicalAxis = _selectedLogicalAxis.Value;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void MotionAxisSelectDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void ApplyThemeFromConfig()
        {
            var theme = ConfigContext.Instance.Config.Setting.Theme;
            var isDarkMode = !string.IsNullOrWhiteSpace(theme) &&
                             (string.Equals(theme, "SkinDark", StringComparison.OrdinalIgnoreCase) ||
                              string.Equals(theme, "Dark", StringComparison.OrdinalIgnoreCase));

            if (isDarkMode)
            {
                AntdUI.Config.IsDark = true;
            }
            else
            {
                AntdUI.Config.IsLight = true;
            }

            textureBackgroundDialog.SetTheme(isDarkMode);
        }
    }
}