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
    /// 轴拓扑配置页面。
    /// 被 MainWindow 页面缓存复用：
    /// - 不在离开页面时释放 ViewModel；
    /// - 首次加载使用布尔标记控制，避免重复初始化。
    /// </summary>
    public sealed partial class MotionAxisManagementPage : UserControl
    {
        private readonly MotionAxisManagementPageModel _model;
        private bool _isFirstLoad;
        private bool _isBusy;

        public MotionAxisManagementPage()
        {
            InitializeComponent();

            _model = new MotionAxisManagementPageModel();

            BindEvents();
            UpdateActionButtons();
        }

        private void BindEvents()
        {
            Load += MotionAxisManagementPage_Load;

            buttonRefresh.Click += async (s, e) => await ReloadAsync();
            buttonSelectCard.Click += ButtonSelectCard_Click;
            buttonAddAxis.Click += async (s, e) => await AddAxisAsync();
        }

        private async void MotionAxisManagementPage_Load(object sender, EventArgs e)
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
                labelSelectedCard.Text = _model.SelectedCardText;
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
            buttonAddAxis.Enabled = !_isBusy;
            buttonSelectCard.Enabled = !_isBusy;
        }

        private void BuildCards()
        {
            flowCards.SuspendLayout();
            try
            {
                ControlDisposeHelper.ClearControlsSafely(flowCards);

                foreach (var item in _model.Items)
                {
                    var card = new MotionAxisCardControl();
                    card.Bind(item);
                    card.Margin = new Padding(0);
                    card.EditRequested += async (s, e) => await EditAxisAsync(card.AxisItem);
                    card.DeleteRequested += async (s, e) => await DeleteAxisAsync(card.AxisItem);
                    card.DetailRequested += (s, e) => ShowDetail(s as Control ?? card, card.AxisItem);
                    flowCards.Controls.Add(card);
                }
            }
            finally
            {
                flowCards.ResumeLayout();
            }
        }

        private async void ButtonSelectCard_Click(object sender, EventArgs e)
        {
            if (_isBusy)
                return;

            using (var dialog = new MotionCardSelectDialog(_model.SelectedCardId))
            {
                if (dialog.ShowDialog(FindForm()) != DialogResult.OK)
                    return;

                _model.SelectedCardId = dialog.SelectedCardId;

                await ReloadAsync();
            }
        }

        private async Task AddAxisAsync()
        {
            if (_isBusy)
                return;

            if (!_model.Cards.Any())
            {
                PageDialogHelper.ShowWarn(this, "新增轴", "当前没有可用控制卡，请先新增控制卡。");
                return;
            }

            var entity = _model.CreateDefaultEntity();
            using (var dialog = new MotionAxisEditDialog(entity, true, _model.Cards.ToList()))
            {
                if (dialog.ShowDialog(FindForm()) != DialogResult.OK)
                    return;

                var result = await _model.SaveAsync(dialog.ResultEntity);
                if (!result.Success)
                    return;

                await ReloadAsync();
            }
        }

        private async Task EditAxisAsync(MotionAxisManagementPageModel.MotionAxisViewItem item)
        {
            if (_isBusy || item == null)
                return;

            using (var dialog = new MotionAxisEditDialog(item.ToEntity(), false, _model.Cards.ToList()))
            {
                if (dialog.ShowDialog(FindForm()) != DialogResult.OK)
                    return;

                var result = await _model.SaveAsync(dialog.ResultEntity);
                if (!result.Success)
                    return;

                await ReloadAsync();
            }
        }

        private async Task DeleteAxisAsync(MotionAxisManagementPageModel.MotionAxisViewItem item)
        {
            if (_isBusy || item == null)
                return;

            var ok = PageDialogHelper.Confirm(
                this,
                "删除轴拓扑",
                "确定删除逻辑轴 " + item.LogicalAxis + "（" + item.DisplayName + "）吗？");

            if (!ok)
                return;

            var result = await _model.DeleteAsync(item.LogicalAxis);
            if (!result.Success)
                return;

            await ReloadAsync();
        }

        private void ShowDetail(Control anchorControl, MotionAxisManagementPageModel.MotionAxisViewItem item)
        {
            if (anchorControl == null || item == null)
                return;

            var detail = new MotionAxisDetailControl();
            detail.Bind(item);

            PageDialogHelper.ShowDetailPopover(this, anchorControl, detail, new Size(500, 460));

        }
    }
}