using AM.DBService.Services.Motion.App;
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
    /// 轴运行参数配置页面。
    /// </summary>
    public sealed partial class MotionAxisParamManagementPage : UserControl
    {
        private readonly MotionAxisParamManagementPageModel _model;
        private readonly MachineConfigReloadService _machineConfigReloadService;

        private bool _isFirstLoad;
        private bool _isBusy;
        private string _selectedParamName;

        public MotionAxisParamManagementPage()
        {
            InitializeComponent();

            _model = new MotionAxisParamManagementPageModel();
            _machineConfigReloadService = new MachineConfigReloadService();

            BindEvents();
            UpdateActionButtons();
            UpdateGroupButtons();
            UpdateSelectionUi();
        }

        private void BindEvents()
        {
            Load += MotionAxisParamManagementPage_Load;

            buttonSelectAxis.Click += ButtonSelectAxis_Click;
            buttonAddParam.Click += ButtonAddParam_Click;
            buttonEditParam.Click += ButtonEditParam_Click;
            buttonDeleteParam.Click += ButtonDeleteParam_Click;

            buttonGroupHardware.Click += async (s, e) => await ChangeGroupAsync(MotionAxisParamManagementPageModel.GroupHardware);
            buttonGroupScale.Click += async (s, e) => await ChangeGroupAsync(MotionAxisParamManagementPageModel.GroupScale);
            buttonGroupMotion.Click += async (s, e) => await ChangeGroupAsync(MotionAxisParamManagementPageModel.GroupMotion);
            buttonGroupHome.Click += async (s, e) => await ChangeGroupAsync(MotionAxisParamManagementPageModel.GroupHome);
            buttonGroupSoftLimit.Click += async (s, e) => await ChangeGroupAsync(MotionAxisParamManagementPageModel.GroupSoftLimit);
            buttonGroupTiming.Click += async (s, e) => await ChangeGroupAsync(MotionAxisParamManagementPageModel.GroupTiming);
            buttonGroupSafety.Click += async (s, e) => await ChangeGroupAsync(MotionAxisParamManagementPageModel.GroupSafety);
        }

        private async void MotionAxisParamManagementPage_Load(object sender, EventArgs e)
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
                await ReloadCoreAsync();
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private async Task ReloadCoreAsync()
        {
            await _model.LoadAsync();
            NormalizeSelectedParam();
            UpdateSelectionUi();
            UpdateGroupButtons();
            BuildCards();
        }

        /// <summary>
        /// 轴参数变更后立即热重载设备配置。
        /// 这样轴参数覆盖结果能立刻同步到运行中的 MotionCardConfig.AxisConfigs。
        /// </summary>
        private async Task<bool> ReloadMachineConfigAsync()
        {
            var result = await Task.Run(() => _machineConfigReloadService.ReloadAndRebuild());
            return result.Success;
        }

        private void SetBusyState(bool isBusy)
        {
            _isBusy = isBusy;
            UpdateActionButtons();
        }

        private void UpdateActionButtons()
        {
            var hasAxis = _model.IsAxisSelected;
            var hasParam = GetSelectedParam() != null;

            buttonSelectAxis.Enabled = !_isBusy;
            buttonAddParam.Enabled = !_isBusy && hasAxis;
            buttonEditParam.Enabled = !_isBusy && hasAxis && hasParam;
            buttonDeleteParam.Enabled = !_isBusy && hasAxis && hasParam;

            buttonGroupHardware.Enabled = !_isBusy && hasAxis;
            buttonGroupScale.Enabled = !_isBusy && hasAxis;
            buttonGroupMotion.Enabled = !_isBusy && hasAxis;
            buttonGroupHome.Enabled = !_isBusy && hasAxis;
            buttonGroupSoftLimit.Enabled = !_isBusy && hasAxis;
            buttonGroupTiming.Enabled = !_isBusy && hasAxis;
            buttonGroupSafety.Enabled = !_isBusy && hasAxis;
        }

        private void UpdateSelectionUi()
        {
            labelSelectedAxis.Text = _model.SelectedAxisText;
            UpdateActionButtons();
        }

        private void UpdateGroupButtons()
        {
            buttonGroupHardware.Type = string.Equals(_model.SelectedGroupKey, MotionAxisParamManagementPageModel.GroupHardware, StringComparison.OrdinalIgnoreCase)
                ? AntdUI.TTypeMini.Primary
                : AntdUI.TTypeMini.Default;

            buttonGroupScale.Type = string.Equals(_model.SelectedGroupKey, MotionAxisParamManagementPageModel.GroupScale, StringComparison.OrdinalIgnoreCase)
                ? AntdUI.TTypeMini.Primary
                : AntdUI.TTypeMini.Default;

            buttonGroupMotion.Type = string.Equals(_model.SelectedGroupKey, MotionAxisParamManagementPageModel.GroupMotion, StringComparison.OrdinalIgnoreCase)
                ? AntdUI.TTypeMini.Primary
                : AntdUI.TTypeMini.Default;

            buttonGroupHome.Type = string.Equals(_model.SelectedGroupKey, MotionAxisParamManagementPageModel.GroupHome, StringComparison.OrdinalIgnoreCase)
                ? AntdUI.TTypeMini.Primary
                : AntdUI.TTypeMini.Default;

            buttonGroupSoftLimit.Type = string.Equals(_model.SelectedGroupKey, MotionAxisParamManagementPageModel.GroupSoftLimit, StringComparison.OrdinalIgnoreCase)
                ? AntdUI.TTypeMini.Primary
                : AntdUI.TTypeMini.Default;

            buttonGroupTiming.Type = string.Equals(_model.SelectedGroupKey, MotionAxisParamManagementPageModel.GroupTiming, StringComparison.OrdinalIgnoreCase)
                ? AntdUI.TTypeMini.Primary
                : AntdUI.TTypeMini.Default;

            buttonGroupSafety.Type = string.Equals(_model.SelectedGroupKey, MotionAxisParamManagementPageModel.GroupSafety, StringComparison.OrdinalIgnoreCase)
                ? AntdUI.TTypeMini.Primary
                : AntdUI.TTypeMini.Default;
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

            UpdateActionButtons();
        }

        private AntdUI.Panel CreateCardWrapper(MotionAxisParamManagementPageModel.AxisParamViewItem item)
        {
            var wrapper = new AntdUI.Panel();
            var card = new MotionAxisParamCardControl();
            var clickHandler = new EventHandler((s, e) => SelectParam(item.ParamName));

            wrapper.Size = new Size(140, 95);
            wrapper.Radius = 12;
            wrapper.Tag = item.ParamName ?? string.Empty;

            card.Dock = DockStyle.Fill;
            card.Margin = new Padding(0);
            card.Bind(item);
            card.DetailRequested += (s, e) => ShowDetail(s as Control ?? card, item);

            BindClickRecursive(wrapper, clickHandler);
            BindClickRecursive(card, clickHandler);

            wrapper.Controls.Add(card);
            ApplyCardSelectionStyle(wrapper, string.Equals(_selectedParamName, item.ParamName, StringComparison.OrdinalIgnoreCase));
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

        private void ApplyCardSelectionStyle(AntdUI.Panel wrapper, bool selected)
        {
            if (wrapper == null)
                return;

            wrapper.BorderWidth = selected ? 2F : 0F;
            wrapper.BorderColor = selected
                ? Color.FromArgb(22, 119, 255)
                : Color.FromArgb(225, 229, 235);
            wrapper.Shadow = selected ? 6 : 0;
        }

        private void SelectParam(string paramName)
        {
            _selectedParamName = paramName;
            foreach (var wrapper in flowCards.Controls.OfType<AntdUI.Panel>())
            {
                var selected = string.Equals(wrapper.Tag as string, _selectedParamName, StringComparison.OrdinalIgnoreCase);
                ApplyCardSelectionStyle(wrapper, selected);
            }

            UpdateActionButtons();
        }

        private MotionAxisParamManagementPageModel.AxisParamViewItem GetSelectedParam()
        {
            if (string.IsNullOrWhiteSpace(_selectedParamName))
                return null;

            return _model.Items.FirstOrDefault(x => string.Equals(x.ParamName, _selectedParamName, StringComparison.OrdinalIgnoreCase));
        }

        private void NormalizeSelectedParam()
        {
            if (string.IsNullOrWhiteSpace(_selectedParamName))
                return;

            if (_model.Items.All(x => !string.Equals(x.ParamName, _selectedParamName, StringComparison.OrdinalIgnoreCase)))
            {
                _selectedParamName = null;
            }
        }

        private async void ButtonSelectAxis_Click(object sender, EventArgs e)
        {
            if (_isBusy)
                return;

            using (var dialog = new MotionAxisSelectDialog(_model.SelectedLogicalAxis))
            {
                if (dialog.ShowDialog(FindForm()) != DialogResult.OK)
                    return;

                SetBusyState(true);
                try
                {
                    var result = await _model.SelectAxisAsync(dialog.SelectedLogicalAxis);
                    if (!result.Success)
                        return;

                    _selectedParamName = null;
                    UpdateSelectionUi();
                    UpdateGroupButtons();
                    BuildCards();
                }
                finally
                {
                    SetBusyState(false);
                }
            }
        }

        private async Task ChangeGroupAsync(string groupKey)
        {
            if (_isBusy || !_model.IsAxisSelected)
                return;

            _model.SelectedGroupKey = groupKey;
            _selectedParamName = null;
            UpdateGroupButtons();
            BuildCards();
            await Task.CompletedTask;
        }

        private async void ButtonAddParam_Click(object sender, EventArgs e)
        {
            if (_isBusy)
                return;

            if (!_model.IsAxisSelected)
            {
                PageDialogHelper.ShowWarn(this, "新增参数", "请先选择轴。");
                return;
            }

            var entity = _model.CreateDefaultEntity();

            using (var dialog = new MotionAxisParamEditDialog(entity, true))
            {
                if (dialog.ShowDialog(FindForm()) != DialogResult.OK)
                    return;

                SetBusyState(true);
                try
                {
                    var result = await _model.SaveAsync(dialog.ResultEntity);
                    if (!result.Success)
                        return;

                    if (!await ReloadMachineConfigAsync())
                        return;

                    _selectedParamName = dialog.ResultEntity.ParamName;
                    await ReloadCoreAsync();
                }
                finally
                {
                    SetBusyState(false);
                }
            }
        }

        private async void ButtonEditParam_Click(object sender, EventArgs e)
        {
            if (_isBusy)
                return;

            var param = GetSelectedParam();
            if (param == null)
            {
                PageDialogHelper.ShowWarn(this, "编辑参数", "请先选择参数卡片。");
                return;
            }

            using (var dialog = new MotionAxisParamEditDialog(param.ToEntity(), false))
            {
                if (dialog.ShowDialog(FindForm()) != DialogResult.OK)
                    return;

                SetBusyState(true);
                try
                {
                    var result = await _model.SaveAsync(dialog.ResultEntity);
                    if (!result.Success)
                        return;

                    if (!await ReloadMachineConfigAsync())
                        return;

                    _selectedParamName = dialog.ResultEntity.ParamName;
                    await ReloadCoreAsync();
                }
                finally
                {
                    SetBusyState(false);
                }
            }
        }

        private async void ButtonDeleteParam_Click(object sender, EventArgs e)
        {
            if (_isBusy)
                return;

            var param = GetSelectedParam();
            if (param == null)
            {
                PageDialogHelper.ShowWarn(this, "删除参数", "请先选择参数卡片。");
                return;
            }

            var ok = PageDialogHelper.Confirm(
                this,
                "删除参数",
                "确定删除参数 " + param.ParamDisplayName + "（" + param.ParamName + "）吗？");

            if (!ok)
                return;

            SetBusyState(true);
            try
            {
                var result = await _model.DeleteAsync(param.ParamName);
                if (!result.Success)
                    return;

                if (!await ReloadMachineConfigAsync())
                    return;

                await ReloadCoreAsync();
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private void ShowDetail(Control anchorControl, MotionAxisParamManagementPageModel.AxisParamViewItem item)
        {
            if (anchorControl == null || item == null)
                return;

            var detail = new MotionAxisParamDetailControl();
            detail.Bind(item);

            PageDialogHelper.ShowDetailPopover(this, anchorControl, detail, new Size(560, 460));
        }
    }
}