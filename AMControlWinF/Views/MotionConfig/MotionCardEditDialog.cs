using AM.Core.Context;
using AM.Model.Entity.Motion.Topology;
using AMControlWinF.Tools;
using AntdUI;
using System;
using System.Linq;
using System.Windows.Forms;

namespace AMControlWinF.Views.MotionConfig
{
    /// <summary>
    /// 控制卡新增/编辑对话框。
    /// </summary>
    public partial class MotionCardEditDialog : AntdUI.Window
    {
        private static readonly CardTypeItem[] CardTypes = new[]
        {
            new CardTypeItem(10, "GOOGO（固高）"),
            new CardTypeItem(20, "LEISAI（雷赛）"),
            new CardTypeItem(90, "VIRTUAL（虚拟）"),
            new CardTypeItem(99, "OTHER（其他）")
        };

        private readonly bool _isAdd;
        private readonly MotionCardEntity _sourceEntity;

        public MotionCardEditDialog(MotionCardEntity entity, bool isAdd)
        {
            InitializeComponent();

            _sourceEntity = entity ?? new MotionCardEntity();
            _isAdd = isAdd;

            InitializeCardTypeDropdown();
            BindEvents();
            ApplyThemeFromConfig();
            ApplyMode();
            LoadEntity();
        }

        public MotionCardEntity ResultEntity { get; private set; }

        private void InitializeCardTypeDropdown()
        {
            dropdownCardType.Items.Clear();
            dropdownCardType.Items.AddRange(CardTypes.Select(x => (object)x.DisplayName).ToArray());

            if (CardTypes.Length > 0)
            {
                dropdownCardType.SelectedValue = CardTypes[0].DisplayName;
            }
        }

        private void BindEvents()
        {
            buttonOk.Click += ButtonOk_Click;
            buttonCancel.Click += ButtonCancel_Click;
            Shown += MotionCardEditDialog_Shown;

            KeyPreview = true;
            KeyDown += MotionCardEditDialog_KeyDown;
        }

        private void MotionCardEditDialog_Shown(object sender, EventArgs e)
        {
            if (_isAdd)
            {
                inputCardId.Focus();
                inputCardId.SelectAll();
            }
            else
            {
                inputName.Focus();
                inputName.SelectAll();
            }
        }

        private void ApplyMode()
        {
            Text = _isAdd ? "新增控制卡" : "编辑控制卡";
            labelDialogTitle.Text = _isAdd ? "新增控制卡" : "编辑控制卡";
            labelDialogDescription.Text = _isAdd
                ? "填写控制卡信息"
                : "修改控制卡信息";

            //Size = new System.Drawing.Size(640, 760);
            buttonOk.Text = "保存";
        }

        private void LoadEntity()
        {
            inputCardId.Text = _sourceEntity.CardId.ToString();
            inputCardId.Enabled = _isAdd;

            var selectedType = CardTypes.FirstOrDefault(x => x.Value == _sourceEntity.CardType);
            dropdownCardType.SelectedValue = selectedType == null
                ? CardTypes[0].DisplayName
                : selectedType.DisplayName;

            inputName.Text = _sourceEntity.Name ?? string.Empty;
            inputDisplayName.Text = _sourceEntity.DisplayName ?? string.Empty;
            inputDriverKey.Text = _sourceEntity.DriverKey ?? string.Empty;
            inputCoreNumber.Text = _sourceEntity.CoreNumber <= 0 ? "2" : _sourceEntity.CoreNumber.ToString();
            inputAxisCount.Text = _sourceEntity.AxisCountNumber < 0 ? "0" : _sourceEntity.AxisCountNumber.ToString();
            inputModeParam.Text = _sourceEntity.ModeParam.ToString();
            inputInitOrder.Text = _sourceEntity.InitOrder.ToString();
            inputSortOrder.Text = _sourceEntity.SortOrder.ToString();
            inputOpenConfig.Text = _sourceEntity.OpenConfig ?? string.Empty;
            inputDescription.Text = _sourceEntity.Description ?? string.Empty;
            inputRemark.Text = _sourceEntity.Remark ?? string.Empty;
            checkUseExtModule.Checked = _sourceEntity.UseExtModule;
            checkIsEnabled.Checked = _sourceEntity.IsEnabled;
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            MotionCardEntity entity;
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

        private void MotionCardEditDialog_KeyDown(object sender, KeyEventArgs e)
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

        private bool TryBuildEntity(out MotionCardEntity entity)
        {
            entity = null;

            short cardId;
            if (!short.TryParse((inputCardId.Text ?? string.Empty).Trim(), out cardId) || cardId < 0)
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "硬件卡号（Card Id）必须为非负整数。");
                inputCardId.Focus();
                return false;
            }

            var name = (inputName.Text ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "内部名称不能为空。");
                inputName.Focus();
                return false;
            }

            int coreNumber;
            if (!int.TryParse((inputCoreNumber.Text ?? string.Empty).Trim(), out coreNumber) || coreNumber <= 0)
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "内核数量必须大于 0。");
                inputCoreNumber.Focus();
                return false;
            }

            short axisCount;
            if (!short.TryParse((inputAxisCount.Text ?? string.Empty).Trim(), out axisCount) || axisCount < 0)
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "支持轴总数必须为非负整数。");
                inputAxisCount.Focus();
                return false;
            }

            short modeParam;
            if (!short.TryParse((inputModeParam.Text ?? string.Empty).Trim(), out modeParam))
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "打开模式参数格式无效，请输入整数。");
                inputModeParam.Focus();
                return false;
            }

            int initOrder;
            if (!int.TryParse((inputInitOrder.Text ?? string.Empty).Trim(), out initOrder) || initOrder < 0)
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "初始化顺序必须为非负整数。");
                inputInitOrder.Focus();
                return false;
            }

            int sortOrder;
            if (!int.TryParse((inputSortOrder.Text ?? string.Empty).Trim(), out sortOrder) || sortOrder < 0)
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "排序号必须为非负整数。");
                inputSortOrder.Focus();
                return false;
            }

            var selectedText = dropdownCardType.SelectedValue == null
                ? string.Empty
                : dropdownCardType.SelectedValue.ToString();

            var cardType = CardTypes.FirstOrDefault(x =>
                string.Equals(x.DisplayName, selectedText, StringComparison.OrdinalIgnoreCase));

            entity = new MotionCardEntity
            {
                Id = _sourceEntity.Id,
                CardId = cardId,
                CardType = cardType == null ? 90 : cardType.Value,
                Name = name,
                DisplayName = NormalizeNullable(inputDisplayName.Text),
                DriverKey = NormalizeNullable(inputDriverKey.Text),
                CoreNumber = coreNumber,
                AxisCountNumber = axisCount,
                ModeParam = modeParam,
                InitOrder = initOrder,
                SortOrder = sortOrder,
                OpenConfig = NormalizeNullable(inputOpenConfig.Text),
                UseExtModule = checkUseExtModule.Checked,
                IsEnabled = checkIsEnabled.Checked,
                Description = NormalizeNullable(inputDescription.Text),
                Remark = NormalizeNullable(inputRemark.Text),
                CreateTime = _sourceEntity.CreateTime == default(DateTime) ? DateTime.Now : _sourceEntity.CreateTime,
                UpdateTime = DateTime.Now
            };

            return true;
        }

        private static string NormalizeNullable(string value)
        {
            var text = value == null ? string.Empty : value.Trim();
            return string.IsNullOrWhiteSpace(text) ? null : text;
        }

        private void ApplyThemeFromConfig()
        {
            var theme = ConfigContext.Instance.Config.Setting.Theme;
            var isDarkMode = IsDarkTheme(theme);

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

        private static bool IsDarkTheme(string theme)
        {
            if (string.IsNullOrWhiteSpace(theme))
            {
                return false;
            }

            return string.Equals(theme, "SkinDark", StringComparison.OrdinalIgnoreCase)
                || string.Equals(theme, "Dark", StringComparison.OrdinalIgnoreCase);
        }

        private sealed class CardTypeItem
        {
            public CardTypeItem(int value, string displayName)
            {
                Value = value;
                DisplayName = displayName;
            }

            public int Value { get; private set; }

            public string DisplayName { get; private set; }
        }
    }
}