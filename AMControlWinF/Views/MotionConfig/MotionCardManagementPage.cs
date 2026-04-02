using AM.PageModel.MotionConfig;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMControlWinF.Views.MotionConfig
{
    /// <summary>
    /// 控制卡配置页面。
    /// 被 MainWindow 页面缓存复用：
    /// - 不在离开页面时释放 ViewModel；
    /// - 首次加载使用布尔标记控制，避免重复初始化。
    /// </summary>
    public partial class MotionCardManagementPage : UserControl
    {
        private readonly MotionCardManagementPageModel _model;
        private bool _isFirstLoad;
        private bool _isBusy;

        public MotionCardManagementPage()
        {
            InitializeComponent();

            _model = new MotionCardManagementPageModel();

            BindEvents();
            UpdateActionButtons();
        }

        private void BindEvents()
        {
            Load += MotionCardManagementPage_Load;

            buttonRefresh.Click += async (s, e) => await ReloadAsync();
            buttonAddCard.Click += ButtonAddCard_Click;
        }

        private async void MotionCardManagementPage_Load(object sender, EventArgs e)
        {
            if (_isFirstLoad)
                return;

            _isFirstLoad = true;
            await ReloadAsync();
        }

        private async Task ReloadAsync()
        {
            if (_isBusy)
                return;

            SetBusyState(true);
            try
            {
                var result = await _model.LoadAsync();
                BuildCards();
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private void SetBusyState(bool isBusy)
        {
            _isBusy = isBusy;
            UpdateActionButtons();
        }

        private void UpdateActionButtons()
        {
            buttonRefresh.Enabled = !_isBusy;
            buttonAddCard.Enabled = !_isBusy;
        }

        private void BuildCards()
        {
            flowCards.SuspendLayout();
            try
            {
                flowCards.Controls.Clear();

                foreach (var item in _model.Cards)
                {
                    var card = new MotionCardControl();
                    card.Bind(item);
                    flowCards.Controls.Add(card);
                }
            }
            finally
            {
                flowCards.ResumeLayout();
            }
        }

        private void ButtonAddCard_Click(object sender, EventArgs e)
        {
            // 下一步接 MotionCardEditDialog。
        }
    }
}