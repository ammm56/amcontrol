using AM.Core.Context;
using AM.Model.Entity.Motion.Topology;
using AMControlWinF.Tools;
using AntdUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AMControlWinF.Views.MotionConfig
{
    /// <summary>
    /// 轴拓扑新增/编辑对话框。
    /// </summary>
    public partial class MotionAxisEditDialog : AntdUI.Window
    {
        private static readonly AxisCategoryItem[] AxisCategories = new[]
        {
            new AxisCategoryItem("Linear", "Linear（直线轴）"),
            new AxisCategoryItem("Rotary", "Rotary（旋转轴）"),
            new AxisCategoryItem("GantryMaster", "GantryMaster（龙门主轴）"),
            new AxisCategoryItem("GantrySlave", "GantrySlave（龙门从轴）"),
            new AxisCategoryItem("Virtual", "Virtual（虚拟轴）"),
            new AxisCategoryItem("Other", "Other（其他）")
        };

        private readonly bool _isAdd;
        private readonly MotionAxisEntity _sourceEntity;
        private readonly List<MotionCardEntity> _availableCards;

        public MotionAxisEditDialog(MotionAxisEntity entity, bool isAdd, List<MotionCardEntity> availableCards)
        {
            InitializeComponent();

            _sourceEntity = entity ?? new MotionAxisEntity();
            _isAdd = isAdd;
            _availableCards = availableCards ?? new List<MotionCardEntity>();

            InitializeAxisCategoryDropdown();
            InitializeCardDropdown();
            BindEvents();
            ApplyThemeFromConfig();
            ApplyMode();
            LoadEntity();
        }

        public MotionAxisEntity ResultEntity { get; private set; }

        private void InitializeAxisCategoryDropdown()
        {
            dropdownAxisCategory.Items.Clear();
            dropdownAxisCategory.Items.AddRange(AxisCategories.Select(x => (object)x.DisplayName).ToArray());

            if (AxisCategories.Length > 0)
            {
                dropdownAxisCategory.SelectedValue = AxisCategories[0].DisplayName;
            }
        }

        private void InitializeCardDropdown()
        {
            dropdownCardId.Items.Clear();
            dropdownCardId.Items.AddRange(_availableCards.Select(x => (object)GetCardDisplayText(x)).ToArray());

            if (_availableCards.Count > 0)
            {
                dropdownCardId.SelectedValue = GetCardDisplayText(_availableCards[0]);
            }
        }

        private void BindEvents()
        {
            dropdownCardId.SelectedValueChanged += DropdownCardId_SelectedValueChanged;
            buttonOk.Click += ButtonOk_Click;
            buttonCancel.Click += ButtonCancel_Click;
            Shown += MotionAxisEditDialog_Shown;

            KeyPreview = true;
            KeyDown += MotionAxisEditDialog_KeyDown;
        }

        private void MotionAxisEditDialog_Shown(object sender, EventArgs e)
        {
            if (_isAdd)
            {
                inputLogicalAxis.Focus();
                inputLogicalAxis.SelectAll();
            }
            else
            {
                inputName.Focus();
                inputName.SelectAll();
            }
        }

        private void ApplyMode()
        {
            Text = _isAdd ? "新增轴拓扑" : "编辑轴拓扑";
            labelDialogTitle.Text = _isAdd ? "新增轴拓扑" : "编辑轴拓扑";
            labelDialogDescription.Text = _isAdd
                ? "填写轴拓扑配置"
                : "修改轴拓扑配置";

            buttonOk.Text = "保存";
        }

        private void LoadEntity()
        {
            inputLogicalAxis.Text = _sourceEntity.LogicalAxis.ToString();
            inputLogicalAxis.Enabled = _isAdd;

            inputName.Text = _sourceEntity.Name ?? string.Empty;
            inputDisplayName.Text = _sourceEntity.DisplayName ?? string.Empty;

            var selectedCategory = AxisCategories.FirstOrDefault(x =>
                string.Equals(x.Value, _sourceEntity.AxisCategory ?? "Linear", StringComparison.OrdinalIgnoreCase));
            dropdownAxisCategory.SelectedValue = selectedCategory == null
                ? AxisCategories[0].DisplayName
                : selectedCategory.DisplayName;

            SelectCardById(_sourceEntity.CardId);
            RefreshPhysicalMappingOptions(_sourceEntity.AxisId, _sourceEntity.PhysicalCore, _sourceEntity.PhysicalAxis);

            inputSortOrder.Text = _sourceEntity.SortOrder.ToString();
            checkIsEnabled.Checked = _sourceEntity.IsEnabled;
            inputDescription.Text = _sourceEntity.Description ?? string.Empty;
            inputRemark.Text = _sourceEntity.Remark ?? string.Empty;
        }

        private void DropdownCardId_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            var currentAxisId = GetSelectedShort(dropdownAxisId, 0);
            var currentCore = GetSelectedShort(dropdownPhysicalCore, 1);
            var currentPhysicalAxis = GetSelectedShort(dropdownPhysicalAxis, 0);

            RefreshPhysicalMappingOptions(currentAxisId, currentCore, currentPhysicalAxis);
        }

        private void RefreshPhysicalMappingOptions(short preferAxisId, short preferCore, short preferPhysicalAxis)
        {
            var card = GetSelectedCard();

            var axisMax = card != null && card.AxisCountNumber > 0
                ? card.AxisCountNumber
                : (short)32;

            var coreMax = card != null && card.CoreNumber > 0
                ? card.CoreNumber
                : 1;

            if (axisMax <= 0)
                axisMax = 1;

            if (coreMax <= 0)
                coreMax = 1;

            var axisOptions = BuildShortRange(0, axisMax - 1);
            var coreOptions = BuildShortRange(1, coreMax);
            var physicalAxisOptions = BuildShortRange(0, axisMax - 1);

            dropdownAxisId.Items.Clear();
            dropdownAxisId.Items.AddRange(axisOptions.Select(x => (object)x.ToString()).ToArray());
            dropdownAxisId.SelectedValue = axisOptions.Contains(preferAxisId)
                ? preferAxisId.ToString()
                : axisOptions[0].ToString();

            dropdownPhysicalCore.Items.Clear();
            dropdownPhysicalCore.Items.AddRange(coreOptions.Select(x => (object)x.ToString()).ToArray());
            dropdownPhysicalCore.SelectedValue = coreOptions.Contains(preferCore)
                ? preferCore.ToString()
                : coreOptions[0].ToString();

            dropdownPhysicalAxis.Items.Clear();
            dropdownPhysicalAxis.Items.AddRange(physicalAxisOptions.Select(x => (object)x.ToString()).ToArray());
            dropdownPhysicalAxis.SelectedValue = physicalAxisOptions.Contains(preferPhysicalAxis)
                ? preferPhysicalAxis.ToString()
                : physicalAxisOptions[0].ToString();
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            MotionAxisEntity entity;
            if (!TryBuildEntity(out entity))
                return;

            ResultEntity = entity;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void MotionAxisEditDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            if (e.KeyCode == Keys.Enter && !(ActiveControl is TextBoxBase))
            {
                e.SuppressKeyPress = true;
                ButtonOk_Click(sender, EventArgs.Empty);
            }
        }

        private bool TryBuildEntity(out MotionAxisEntity entity)
        {
            entity = null;

            short logicalAxis;
            if (!short.TryParse((inputLogicalAxis.Text ?? string.Empty).Trim(), out logicalAxis) || logicalAxis <= 0)
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "逻辑轴号必须为大于 0 的整数。");
                inputLogicalAxis.Focus();
                return false;
            }

            var name = (inputName.Text ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "内部名称不能为空。");
                inputName.Focus();
                return false;
            }

            var selectedCard = GetSelectedCard();
            if (selectedCard == null)
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "请选择所属控制卡。");
                dropdownCardId.Focus();
                return false;
            }

            short axisId = GetSelectedShort(dropdownAxisId, 0);
            short physicalCore = GetSelectedShort(dropdownPhysicalCore, 1);
            short physicalAxis = GetSelectedShort(dropdownPhysicalAxis, 0);

            int sortOrder;
            if (!int.TryParse((inputSortOrder.Text ?? string.Empty).Trim(), out sortOrder) || sortOrder < 0)
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "排序号必须为非负整数。");
                inputSortOrder.Focus();
                return false;
            }

            var selectedAxisCategoryText = dropdownAxisCategory.SelectedValue == null
                ? string.Empty
                : dropdownAxisCategory.SelectedValue.ToString();

            var axisCategory = AxisCategories.FirstOrDefault(x =>
                string.Equals(x.DisplayName, selectedAxisCategoryText, StringComparison.OrdinalIgnoreCase));

            entity = new MotionAxisEntity
            {
                Id = _sourceEntity.Id,
                CardId = selectedCard.CardId,
                AxisId = axisId,
                LogicalAxis = logicalAxis,
                Name = name,
                DisplayName = NormalizeNullable(inputDisplayName.Text),
                AxisCategory = axisCategory == null ? "Linear" : axisCategory.Value,
                PhysicalCore = physicalCore,
                PhysicalAxis = physicalAxis,
                IsEnabled = checkIsEnabled.Checked,
                SortOrder = sortOrder,
                Description = NormalizeNullable(inputDescription.Text),
                Remark = NormalizeNullable(inputRemark.Text),
                CreateTime = _sourceEntity.CreateTime == default(DateTime) ? DateTime.Now : _sourceEntity.CreateTime,
                UpdateTime = DateTime.Now
            };

            return true;
        }

        private MotionCardEntity GetSelectedCard()
        {
            var text = dropdownCardId.SelectedValue == null
                ? string.Empty
                : dropdownCardId.SelectedValue.ToString();

            return _availableCards.FirstOrDefault(x =>
                string.Equals(GetCardDisplayText(x), text, StringComparison.OrdinalIgnoreCase));
        }

        private void SelectCardById(short cardId)
        {
            var card = _availableCards.FirstOrDefault(x => x.CardId == cardId);
            if (card != null)
            {
                dropdownCardId.SelectedValue = GetCardDisplayText(card);
                return;
            }

            if (_availableCards.Count > 0)
            {
                dropdownCardId.SelectedValue = GetCardDisplayText(_availableCards[0]);
            }
        }

        private static string GetCardDisplayText(MotionCardEntity card)
        {
            if (card == null)
                return string.Empty;

            var name = string.IsNullOrWhiteSpace(card.DisplayName)
                ? (string.IsNullOrWhiteSpace(card.Name) ? "控制卡" : card.Name)
                : card.DisplayName;

            return "#" + card.CardId + "  " + name;
        }

        private static short GetSelectedShort(Select select, short defaultValue)
        {
            if (select == null || select.SelectedValue == null)
                return defaultValue;

            short result;
            return short.TryParse(select.SelectedValue.ToString(), out result)
                ? result
                : defaultValue;
        }

        private static List<short> BuildShortRange(int from, int to)
        {
            var list = new List<short>();

            for (var i = from; i <= to; i++)
            {
                list.Add((short)i);
            }

            return list;
        }

        private static string NormalizeNullable(string value)
        {
            var text = value == null ? string.Empty : value.Trim();
            return string.IsNullOrWhiteSpace(text) ? null : text;
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

        private sealed class AxisCategoryItem
        {
            public AxisCategoryItem(string value, string displayName)
            {
                Value = value;
                DisplayName = displayName;
            }

            public string Value { get; private set; }

            public string DisplayName { get; private set; }
        }
    }
}