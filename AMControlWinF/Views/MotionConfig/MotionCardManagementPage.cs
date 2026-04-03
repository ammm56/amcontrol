using AM.DBService.Services.Motion.Topology;
using AM.Model.Entity.Motion.Topology;
using AM.PageModel.MotionConfig;
using System;
using System.Linq;
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
        private readonly MotionCardCrudService _cardService;
        private bool _isFirstLoad;
        private bool _isBusy;

        public MotionCardManagementPage()
        {
            InitializeComponent();

            _model = new MotionCardManagementPageModel();
            _cardService = new MotionCardCrudService();

            BindEvents();
            UpdateActionButtons();
        }

        private void BindEvents()
        {
            Load += MotionCardManagementPage_Load;

            buttonRefresh.Click += async (s, e) => await ReloadAsync();
            buttonAddCard.Click += async (s, e) => await AddCardAsync();
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
                await _model.LoadAsync();
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
                    card.Margin = new Padding(0);
                    card.EditRequested += async (s, e) => await EditCardAsync(card.CardItem);
                    card.DeleteRequested += async (s, e) => await DeleteCardAsync(card.CardItem);
                    flowCards.Controls.Add(card);
                }
            }
            finally
            {
                flowCards.ResumeLayout();
            }
        }

        private async Task AddCardAsync()
        {
            if (_isBusy)
                return;

            var entity = CreateDefaultCardEntity();

            using (var dialog = new MotionCardEditDialog(entity, true))
            {
                if (dialog.ShowDialog(FindForm()) != DialogResult.OK)
                    return;

                var result = await Task.Run(() => _cardService.Save(dialog.ResultEntity));
                if (!result.Success)
                    return;

                await ReloadAsync();
            }
        }

        private async Task EditCardAsync(MotionCardManagementPageModel.MotionCardViewItem item)
        {
            if (_isBusy || item == null)
                return;

            var queryResult = await Task.Run(() => _cardService.QueryByCardId(item.CardId));
            if (!queryResult.Success || queryResult.Item == null)
                return;

            using (var dialog = new MotionCardEditDialog(queryResult.Item, false))
            {
                if (dialog.ShowDialog(FindForm()) != DialogResult.OK)
                    return;

                var saveResult = await Task.Run(() => _cardService.Save(dialog.ResultEntity));
                if (!saveResult.Success)
                    return;

                await ReloadAsync();
            }
        }

        private async Task DeleteCardAsync(MotionCardManagementPageModel.MotionCardViewItem item)
        {
            if (_isBusy || item == null)
                return;

            using (var dialog = new MotionCardDeleteConfirmDialog())
            {
                dialog.TargetCardId = item.CardId;
                dialog.TargetDisplayName = item.DisplayName;
                dialog.TargetName = item.Name;

                if (dialog.ShowDialog(FindForm()) != DialogResult.OK)
                    return;
            }

            var result = await Task.Run(() => _cardService.DeleteByCardId(item.CardId));
            if (!result.Success)
                return;

            await ReloadAsync();
        }

        private static MotionCardEntity CreateDefaultCardEntity()
        {
            return new MotionCardEntity
            {
                CardId = 0,
                CardType = 90,
                Name = string.Empty,
                DisplayName = string.Empty,
                DriverKey = "Virtual.Basic",
                ModeParam = 0,
                OpenConfig = string.Empty,
                CoreNumber = 2,
                AxisCountNumber = 16,
                UseExtModule = false,
                InitOrder = 1,
                IsEnabled = true,
                SortOrder = 1,
                Description = string.Empty,
                Remark = string.Empty,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
        }
    }
}