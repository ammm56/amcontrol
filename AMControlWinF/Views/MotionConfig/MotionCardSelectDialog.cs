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
    /// 控制卡选择对话框。
    /// </summary>
    public partial class MotionCardSelectDialog : AntdUI.Window
    {
        private readonly MotionCardManagementPageModel _model;
        private bool _isFirstLoad;
        private short? _selectedCardId;

        public MotionCardSelectDialog()
        {
            InitializeComponent();

            _model = new MotionCardManagementPageModel();

            BindEvents();
            ApplyThemeFromConfig();
            UpdateSelectionUi();
        }

        public short SelectedCardId { get; private set; }

        private void BindEvents()
        {
            Load += MotionCardSelectDialog_Load;
            buttonRefresh.Click += async (s, e) => await ReloadAsync();
            buttonAllCards.Click += ButtonAllCards_Click;
            buttonOk.Click += ButtonOk_Click;
            buttonCancel.Click += ButtonCancel_Click;

            KeyPreview = true;
            KeyDown += MotionCardSelectDialog_KeyDown;
        }

        private async void MotionCardSelectDialog_Load(object sender, EventArgs e)
        {
            if (_isFirstLoad)
                return;

            _isFirstLoad = true;
            await ReloadAsync();
        }

        private async Task ReloadAsync()
        {
            buttonRefresh.Enabled = false;
            buttonAllCards.Enabled = false;

            try
            {
                await _model.LoadAsync();
                BuildCards();
                UpdateSelectionUi();
            }
            finally
            {
                buttonRefresh.Enabled = true;
                buttonAllCards.Enabled = true;
            }
        }

        private void BuildCards()
        {
            flowCards.SuspendLayout();
            try
            {
                ControlDisposeHelper.ClearControlsSafely(flowCards);

                foreach (var item in _model.Cards)
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

        private AntdUI.Panel CreateCardWrapper(MotionCardManagementPageModel.MotionCardViewItem item)
        {
            var wrapper = new AntdUI.Panel();
            var card = new MotionCardControl();
            var clickHandler = new EventHandler((s, e) => SelectCard(item.CardId));

            wrapper.Margin = new Padding(0);
            wrapper.Padding = new Padding(2);
            wrapper.Size = new Size(316, 206);
            wrapper.Radius = 12;
            wrapper.BorderWidth = 1F;
            wrapper.Tag = item.CardId;

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

        private void SelectCard(short cardId)
        {
            _selectedCardId = cardId;
            UpdateSelectionUi();
        }

        private void ButtonAllCards_Click(object sender, EventArgs e)
        {
            _selectedCardId = null;
            UpdateSelectionUi();
        }

        private void UpdateSelectionUi()
        {
            buttonAllCards.Type = _selectedCardId.HasValue
                ? AntdUI.TTypeMini.Default
                : AntdUI.TTypeMini.Primary;

            if (_selectedCardId.HasValue)
            {
                var selected = _model.Cards.FirstOrDefault(x => x.CardId == _selectedCardId.Value);
                if (selected != null)
                {
                    var name = string.IsNullOrWhiteSpace(selected.DisplayName) ? selected.Name : selected.DisplayName;
                    labelCurrentSelection.Text = "当前选择：" + name + " (#" + selected.CardId + ")";
                }
                else
                {
                    labelCurrentSelection.Text = "当前选择：控制卡 #" + _selectedCardId.Value;
                }
            }
            else
            {
                labelCurrentSelection.Text = "当前选择：全部控制卡";
            }

            foreach (var wrapper in flowCards.Controls.OfType<AntdUI.Panel>())
            {
                var cardId = wrapper.Tag is short ? (short)wrapper.Tag : (short)0;
                var selected = _selectedCardId.HasValue && _selectedCardId.Value == cardId;

                wrapper.BorderWidth = selected ? 2F : 1F;
                wrapper.BorderColor = selected
                    ? Color.FromArgb(22, 119, 255)
                    : Color.FromArgb(225, 229, 235);
                wrapper.Shadow = selected ? 6 : 0;
            }
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            SelectedCardId = _selectedCardId ?? (short)0;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void MotionCardSelectDialog_KeyDown(object sender, KeyEventArgs e)
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