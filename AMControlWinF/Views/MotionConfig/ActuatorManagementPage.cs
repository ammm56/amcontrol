using AM.Model.Entity.Motion.Actuator;
using AM.PageModel.MotionConfig;
using AMControlWinF.Tools;
using AntdUI;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMControlWinF.Views.MotionConfig
{
    /// <summary>
    /// 执行器配置页面。
    /// </summary>
    public sealed partial class ActuatorManagementPage : UserControl
    {
        private readonly ActuatorManagementPageModel _model;
        private bool _isFirstLoad;
        private bool _isBusy;
        private string _selectedItemKey;

        public ActuatorManagementPage()
        {
            InitializeComponent();

            _model = new ActuatorManagementPageModel();

            BindEvents();
            UpdateActionButtons();
            UpdateCategoryButtons();
        }

        private void BindEvents()
        {
            Load += ActuatorManagementPage_Load;

            buttonAdd.Click += ButtonAdd_Click;
            buttonEdit.Click += ButtonEdit_Click;
            buttonDelete.Click += ButtonDelete_Click;

            buttonFilterAll.Click += (s, e) => ChangeCategory(ActuatorManagementPageModel.TypeAll);
            buttonFilterCylinder.Click += (s, e) => ChangeCategory(ActuatorManagementPageModel.TypeCylinder);
            buttonFilterVacuum.Click += (s, e) => ChangeCategory(ActuatorManagementPageModel.TypeVacuum);
            buttonFilterStackLight.Click += (s, e) => ChangeCategory(ActuatorManagementPageModel.TypeStackLight);
            buttonFilterGripper.Click += (s, e) => ChangeCategory(ActuatorManagementPageModel.TypeGripper);
        }

        private async void ActuatorManagementPage_Load(object sender, EventArgs e)
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
                NormalizeSelectedItem();
                UpdateCategoryButtons();
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
            var hasSelectedItem = GetSelectedItem() != null;

            buttonAdd.Enabled = !_isBusy;
            buttonEdit.Enabled = !_isBusy && hasSelectedItem;
            buttonDelete.Enabled = !_isBusy && hasSelectedItem;

            buttonFilterAll.Enabled = !_isBusy;
            buttonFilterCylinder.Enabled = !_isBusy;
            buttonFilterVacuum.Enabled = !_isBusy;
            buttonFilterStackLight.Enabled = !_isBusy;
            buttonFilterGripper.Enabled = !_isBusy;
        }

        private void UpdateCategoryButtons()
        {
            buttonFilterAll.Type = IsCategorySelected(ActuatorManagementPageModel.TypeAll)
                ? TTypeMini.Primary
                : TTypeMini.Default;

            buttonFilterCylinder.Type = IsCategorySelected(ActuatorManagementPageModel.TypeCylinder)
                ? TTypeMini.Primary
                : TTypeMini.Default;

            buttonFilterVacuum.Type = IsCategorySelected(ActuatorManagementPageModel.TypeVacuum)
                ? TTypeMini.Primary
                : TTypeMini.Default;

            buttonFilterStackLight.Type = IsCategorySelected(ActuatorManagementPageModel.TypeStackLight)
                ? TTypeMini.Primary
                : TTypeMini.Default;

            buttonFilterGripper.Type = IsCategorySelected(ActuatorManagementPageModel.TypeGripper)
                ? TTypeMini.Primary
                : TTypeMini.Default;
        }

        private bool IsCategorySelected(string categoryKey)
        {
            return string.Equals(_model.SelectedCategoryKey, categoryKey, StringComparison.OrdinalIgnoreCase);
        }

        private void ChangeCategory(string categoryKey)
        {
            if (_isBusy)
                return;

            _model.SelectedCategoryKey = categoryKey;
            NormalizeSelectedItem();
            UpdateCategoryButtons();
            BuildCards();
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

        private AntdUI.Panel CreateCardWrapper(ActuatorManagementPageModel.ActuatorViewItem item)
        {
            var wrapper = new AntdUI.Panel();
            var card = new ActuatorCardControl();
            var clickHandler = new EventHandler((s, e) => SelectItem(item));

            wrapper.Margin = new Padding(0);
            wrapper.Padding = new Padding(2);
            wrapper.Size = new Size(180, 140);
            wrapper.Radius = 12;
            wrapper.BorderWidth = 0F;
            wrapper.Tag = BuildItemKey(item);

            card.Dock = DockStyle.Fill;
            card.Margin = new Padding(0);
            card.Bind(item);
            card.DetailRequested += (s, e) => ShowDetail(s as Control ?? card, item);

            BindClickRecursive(wrapper, clickHandler);
            BindClickRecursive(card, clickHandler);

            wrapper.Controls.Add(card);
            ApplyCardSelectionStyle(wrapper, string.Equals(_selectedItemKey, BuildItemKey(item), StringComparison.OrdinalIgnoreCase));
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

        private void SelectItem(ActuatorManagementPageModel.ActuatorViewItem item)
        {
            _selectedItemKey = BuildItemKey(item);

            foreach (var wrapper in flowCards.Controls.OfType<AntdUI.Panel>())
            {
                var selected = string.Equals(wrapper.Tag as string, _selectedItemKey, StringComparison.OrdinalIgnoreCase);
                ApplyCardSelectionStyle(wrapper, selected);
            }

            UpdateActionButtons();
        }

        private void NormalizeSelectedItem()
        {
            if (string.IsNullOrWhiteSpace(_selectedItemKey))
                return;

            if (_model.Items.All(x => !string.Equals(BuildItemKey(x), _selectedItemKey, StringComparison.OrdinalIgnoreCase)))
            {
                _selectedItemKey = null;
            }
        }

        private ActuatorManagementPageModel.ActuatorViewItem GetSelectedItem()
        {
            if (string.IsNullOrWhiteSpace(_selectedItemKey))
                return null;

            return _model.Items.FirstOrDefault(x => string.Equals(BuildItemKey(x), _selectedItemKey, StringComparison.OrdinalIgnoreCase));
        }

        private static string BuildItemKey(ActuatorManagementPageModel.ActuatorViewItem item)
        {
            if (item == null)
                return string.Empty;

            return (item.ActuatorType ?? string.Empty) + "|" + (item.Name ?? string.Empty);
        }

        private async void ButtonAdd_Click(object sender, EventArgs e)
        {
            if (_isBusy)
                return;

            var actuatorType = ResolveAddType();
            var entity = _model.CreateDefaultEntity(actuatorType);
            if (entity == null)
            {
                PageDialogHelper.ShowWarn(this, "新增执行器", "当前执行器类型无效。");
                return;
            }

            using (var dialog = new ActuatorEditDialog(actuatorType, entity, true))
            {
                if (dialog.ShowDialog(FindForm()) != DialogResult.OK)
                    return;

                SetBusyState(true);
                try
                {
                    var result = await _model.SaveAsync(dialog.ResultActuatorType, dialog.ResultEntity);
                    if (!result.Success)
                        return;

                    _selectedItemKey = dialog.ResultActuatorType + "|" + GetEntityName(dialog.ResultEntity);
                    _model.SelectedCategoryKey = dialog.ResultActuatorType;
                    await ReloadAsync();
                }
                finally
                {
                    SetBusyState(false);
                }
            }
        }

        private async void ButtonEdit_Click(object sender, EventArgs e)
        {
            if (_isBusy)
                return;

            var item = GetSelectedItem();
            if (item == null)
            {
                PageDialogHelper.ShowWarn(this, "编辑执行器", "请先选择执行器卡片。");
                return;
            }

            using (var dialog = new ActuatorEditDialog(item.ActuatorType, item.SourceEntity, false))
            {
                if (dialog.ShowDialog(FindForm()) != DialogResult.OK)
                    return;

                SetBusyState(true);
                try
                {
                    var result = await _model.SaveAsync(dialog.ResultActuatorType, dialog.ResultEntity);
                    if (!result.Success)
                        return;

                    _selectedItemKey = dialog.ResultActuatorType + "|" + GetEntityName(dialog.ResultEntity);
                    await ReloadAsync();
                }
                finally
                {
                    SetBusyState(false);
                }
            }
        }

        private async void ButtonDelete_Click(object sender, EventArgs e)
        {
            if (_isBusy)
                return;

            var item = GetSelectedItem();
            if (item == null)
            {
                PageDialogHelper.ShowWarn(this, "删除执行器", "请先选择执行器卡片。");
                return;
            }

            var ok = PageDialogHelper.Confirm(
                this,
                "删除执行器",
                "确定删除 " + item.TypeDisplayName + "「" + item.DisplayTitle + "」吗？");

            if (!ok)
                return;

            SetBusyState(true);
            try
            {
                var result = await _model.DeleteAsync(item);
                if (!result.Success)
                    return;

                _selectedItemKey = null;
                await ReloadAsync();
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private static string GetEntityName(object entity)
        {
            if (entity is CylinderConfigEntity)
                return ((CylinderConfigEntity)entity).Name ?? string.Empty;

            if (entity is VacuumConfigEntity)
                return ((VacuumConfigEntity)entity).Name ?? string.Empty;

            if (entity is StackLightConfigEntity)
                return ((StackLightConfigEntity)entity).Name ?? string.Empty;

            if (entity is GripperConfigEntity)
                return ((GripperConfigEntity)entity).Name ?? string.Empty;

            return string.Empty;
        }

        private string ResolveAddType()
        {
            if (string.Equals(_model.SelectedCategoryKey, ActuatorManagementPageModel.TypeAll, StringComparison.OrdinalIgnoreCase))
                return ActuatorManagementPageModel.TypeCylinder;

            return _model.SelectedCategoryKey;
        }

        private void ShowDetail(Control anchorControl, ActuatorManagementPageModel.ActuatorViewItem item)
        {
            if (anchorControl == null || item == null)
                return;

            var detail = new ActuatorDetailControl();
            detail.Bind(item);

            PageDialogHelper.ShowDetailPopover(this, anchorControl, detail, new Size(560, 560));
        }
    }
}