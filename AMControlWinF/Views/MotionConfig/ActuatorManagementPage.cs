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
            var clickHandler = new EventHandler((s, e) => SelectItem(item));

            wrapper.Margin = new Padding(0);
            wrapper.Padding = new Padding(2);
            wrapper.Size = new Size(248, 168);
            wrapper.Radius = 12;
            wrapper.BorderWidth = 0F;
            wrapper.Tag = BuildItemKey(item);

            var card = CreateCardPanel(item);
            card.Dock = DockStyle.Fill;

            BindClickRecursive(wrapper, clickHandler);
            BindClickRecursive(card, clickHandler);

            wrapper.Controls.Add(card);
            ApplyCardSelectionStyle(wrapper, string.Equals(_selectedItemKey, BuildItemKey(item), StringComparison.OrdinalIgnoreCase));
            return wrapper;
        }

        private AntdUI.Panel CreateCardPanel(ActuatorManagementPageModel.ActuatorViewItem item)
        {
            var panelCard = new AntdUI.Panel();
            panelCard.BackColor = Color.Transparent;
            panelCard.BorderColor = Color.FromArgb(225, 229, 235);
            panelCard.Padding = new Padding(12);
            panelCard.Radius = 12;
            panelCard.Shadow = 4;
            panelCard.ShadowOpacity = 0.2F;
            panelCard.ShadowOpacityAnimation = false;

            var panelHeader = new AntdUI.Panel();
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Height = 36;
            panelHeader.Radius = 0;

            var flowHeaderLeft = new AntdUI.FlowPanel();
            flowHeaderLeft.Dock = DockStyle.Left;
            flowHeaderLeft.Gap = 6;
            flowHeaderLeft.Size = new Size(88, 36);

            var buttonTypeTag = new AntdUI.Button();
            buttonTypeTag.Font = new Font("Microsoft YaHei UI", 9F);
            buttonTypeTag.Margin = new Padding(0);
            buttonTypeTag.Radius = 8;
            buttonTypeTag.Size = new Size(64, 24);
            buttonTypeTag.Text = item.TypeDisplayName;
            buttonTypeTag.Type = ResolveTypeButtonType(item.ActuatorType);
            buttonTypeTag.WaveSize = 0;

            flowHeaderLeft.Controls.Add(buttonTypeTag);

            var flowHeaderRight = new AntdUI.FlowPanel();
            flowHeaderRight.Align = TAlignFlow.RightCenter;
            flowHeaderRight.Dock = DockStyle.Right;
            flowHeaderRight.Gap = 6;
            flowHeaderRight.Size = new Size(72, 36);

            var buttonDetail = new AntdUI.Button();
            buttonDetail.IconSvg = "ProfileOutlined";
            buttonDetail.Margin = new Padding(0);
            buttonDetail.Radius = 8;
            buttonDetail.Size = new Size(64, 32);
            buttonDetail.Text = "详情";
            buttonDetail.WaveSize = 0;
            buttonDetail.Click += (s, e) => ShowDetail(buttonDetail, item);

            flowHeaderRight.Controls.Add(buttonDetail);

            panelHeader.Controls.Add(flowHeaderRight);
            panelHeader.Controls.Add(flowHeaderLeft);

            var panelBody = new AntdUI.Panel();
            panelBody.Dock = DockStyle.Fill;
            panelBody.Padding = new Padding(0, 2, 0, 0);
            panelBody.Radius = 0;

            var labelTitle = new AntdUI.Label();
            labelTitle.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold);
            labelTitle.Location = new Point(0, 6);
            labelTitle.Margin = new Padding(0);
            labelTitle.Padding = new Padding(6, 0, 0, 0);
            labelTitle.Size = new Size(210, 28);
            labelTitle.Text = item.DisplayTitle;

            var labelName = new AntdUI.Label();
            labelName.ForeColor = Color.Gray;
            labelName.Location = new Point(0, 38);
            labelName.Margin = new Padding(0);
            labelName.Padding = new Padding(6, 0, 0, 0);
            labelName.Size = new Size(210, 22);
            labelName.Text = string.IsNullOrWhiteSpace(item.Name) ? "-" : item.Name;

            var labelSummary1 = new AntdUI.Label();
            labelSummary1.Location = new Point(0, 66);
            labelSummary1.Margin = new Padding(0);
            labelSummary1.Padding = new Padding(6, 0, 0, 0);
            labelSummary1.Size = new Size(210, 22);
            labelSummary1.Text = string.IsNullOrWhiteSpace(item.Summary1) ? "-" : item.Summary1;

            var labelSummary2 = new AntdUI.Label();
            labelSummary2.Location = new Point(0, 88);
            labelSummary2.Margin = new Padding(0);
            labelSummary2.Padding = new Padding(6, 0, 0, 0);
            labelSummary2.Size = new Size(210, 22);
            labelSummary2.Text = string.IsNullOrWhiteSpace(item.Summary2) ? "-" : item.Summary2;

            var labelSummary3 = new AntdUI.Label();
            labelSummary3.Location = new Point(0, 110);
            labelSummary3.Margin = new Padding(0);
            labelSummary3.Padding = new Padding(6, 0, 0, 0);
            labelSummary3.Size = new Size(210, 22);
            labelSummary3.Text = string.IsNullOrWhiteSpace(item.Summary3) ? "-" : item.Summary3;

            var labelStatus = new AntdUI.Label();
            labelStatus.Location = new Point(132, 132);
            labelStatus.Margin = new Padding(0);
            labelStatus.Size = new Size(78, 22);
            labelStatus.Text = item.IsEnabled ? "● 启用" : "● 禁用";
            labelStatus.TextAlign = ContentAlignment.MiddleRight;
            labelStatus.ForeColor = item.IsEnabled
                ? Color.FromArgb(82, 196, 26)
                : Color.FromArgb(245, 34, 45);

            panelBody.Controls.Add(labelStatus);
            panelBody.Controls.Add(labelSummary3);
            panelBody.Controls.Add(labelSummary2);
            panelBody.Controls.Add(labelSummary1);
            panelBody.Controls.Add(labelName);
            panelBody.Controls.Add(labelTitle);

            panelCard.Controls.Add(panelBody);
            panelCard.Controls.Add(panelHeader);

            return panelCard;
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
                _selectedItemKey = null;
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

                    var savedName = GetEntityName(dialog.ResultEntity);
                    _selectedItemKey = dialog.ResultActuatorType + "|" + savedName;
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

                    var savedName = GetEntityName(dialog.ResultEntity);
                    _selectedItemKey = dialog.ResultActuatorType + "|" + savedName;
                    await ReloadAsync();
                }
                finally
                {
                    SetBusyState(false);
                }
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

        private static TTypeMini ResolveTypeButtonType(string actuatorType)
        {
            if (string.Equals(actuatorType, ActuatorManagementPageModel.TypeCylinder, StringComparison.OrdinalIgnoreCase))
                return TTypeMini.Primary;
            if (string.Equals(actuatorType, ActuatorManagementPageModel.TypeVacuum, StringComparison.OrdinalIgnoreCase))
                return TTypeMini.Success;
            if (string.Equals(actuatorType, ActuatorManagementPageModel.TypeStackLight, StringComparison.OrdinalIgnoreCase))
                return TTypeMini.Warn;
            if (string.Equals(actuatorType, ActuatorManagementPageModel.TypeGripper, StringComparison.OrdinalIgnoreCase))
                return TTypeMini.Error;

            return TTypeMini.Default;
        }

        private void ShowDetail(Control anchorControl, ActuatorManagementPageModel.ActuatorViewItem item)
        {
            if (anchorControl == null || item == null)
                return;

            var detail = CreateDetailControl(item);
            PageDialogHelper.ShowDetailPopover(this, anchorControl, detail, new Size(560, 460));
        }

        private Control CreateDetailControl(ActuatorManagementPageModel.ActuatorViewItem item)
        {
            var panelRoot = new AntdUI.Panel();
            panelRoot.Dock = DockStyle.Fill;
            panelRoot.Padding = new Padding(12);
            panelRoot.Radius = 0;

            var stack = new AntdUI.StackPanel();
            stack.Dock = DockStyle.Fill;
            stack.Padding = new Padding(4);
            stack.Vertical = true;

            var labelTitle = new AntdUI.Label();
            labelTitle.Font = new Font("Microsoft YaHei UI", 13F, FontStyle.Bold);
            labelTitle.Margin = new Padding(0);
            labelTitle.Size = new Size(520, 30);
            labelTitle.Text = item.DisplayTitle;

            var labelSubTitle = new AntdUI.Label();
            labelSubTitle.ForeColor = Color.Gray;
            labelSubTitle.Margin = new Padding(0);
            labelSubTitle.Size = new Size(520, 24);
            labelSubTitle.Text = item.TypeDisplayName + " · " + (item.IsEnabled ? "已启用" : "已禁用");

            var divider = new AntdUI.Divider();
            divider.Margin = new Padding(0);
            divider.Size = new Size(520, 22);
            divider.Text = "详细信息";

            stack.Controls.Add(labelTitle);
            stack.Controls.Add(labelSubTitle);
            stack.Controls.Add(divider);

            foreach (var line in item.DetailLines ?? Enumerable.Empty<ActuatorManagementPageModel.ActuatorDetailLine>())
            {
                var labelName = new AntdUI.Label();
                labelName.ForeColor = Color.Gray;
                labelName.Margin = new Padding(0);
                labelName.Size = new Size(520, 22);
                labelName.Text = line.Title;

                var labelValue = new AntdUI.Label();
                labelValue.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
                labelValue.Margin = new Padding(0);
                labelValue.Size = new Size(520, 24);
                labelValue.Text = string.IsNullOrWhiteSpace(line.Value) ? "-" : line.Value;

                stack.Controls.Add(labelName);
                stack.Controls.Add(labelValue);
            }

            panelRoot.Controls.Add(stack);
            return panelRoot;
        }
    }
}