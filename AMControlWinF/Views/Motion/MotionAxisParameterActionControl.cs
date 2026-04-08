using AM.PageModel.Motion;
using AntdUI;
using System;
using System.Windows.Forms;

namespace AMControlWinF.Views.Motion
{
    /// <summary>
    /// 单个参数动作卡片控件。
    ///
    /// 设计说明：
    /// 1. 一个控件只表示一个参数动作，例如“应用速度”“绝对定位”“相对移动”；
    /// 2. 控件内部只包含：左上角分类徽标、参数输入框、右下角动作按钮；
    /// 3. 左上角徽标使用 AntdUI.Button，仅用于视觉表达，不承载点击语义；
    /// 4. 右下角按钮始终显示真实动作名，不再显示“确认”“不可用”；
    /// 5. 控件本身不执行业务逻辑，只负责展示状态并抛出执行事件。
    /// </summary>
    public partial class MotionAxisParameterActionControl : UserControl
    {
        /// <summary>
        /// 当前绑定的动作项。
        /// 页面层会周期性刷新该项，用于同步可执行状态。
        /// </summary>
        private MotionAxisPageModel.MotionAxisActionViewItem _item;

        /// <summary>
        /// 配置阶段传入的默认徽标样式。
        /// 当运行态动作项未提供强调类型时，回退使用该值。
        /// </summary>
        private TTypeMini _configuredBadgeType = TTypeMini.Primary;

        /// <summary>
        /// 右下角动作按钮文案。
        /// 这里固定保存真实动作名，例如：应用速度、绝对定位、相对移动。
        /// </summary>
        private string _configuredButtonText = string.Empty;

        public MotionAxisParameterActionControl()
        {
            InitializeComponent();
            BindEvents();
        }

        /// <summary>
        /// 当前卡片对应的动作键。
        /// 例如：ApplyVelocity / MoveAbsolute / MoveRelative
        /// 页面层执行动作时使用此键。
        /// </summary>
        public string ActionKey { get; private set; }

        /// <summary>
        /// 当前输入框中的文本值。
        /// 页面层统一从这里读取参数，不在控件内部做业务解析。
        /// </summary>
        public string InputText
        {
            get { return inputValue.Text; }
            set { inputValue.Text = value ?? string.Empty; }
        }

        /// <summary>
        /// 配置卡片的静态展示信息。
        ///
        /// 参数说明：
        /// - actionKey：动作键
        /// - badgeText：左上角分类文本，如“参数”“定位”
        /// - buttonText：右下角按钮文案，直接显示真实动作名
        /// - placeholderText：输入框占位提示
        /// - accentType：默认强调类型
        ///
        /// 调用时机：
        /// 页面初始化时调用一次，建立当前卡片的固定身份。
        /// </summary>
        public void Configure(
            string actionKey,
            string badgeText,
            string buttonText,
            string placeholderText,
            string accentType)
        {
            ActionKey = actionKey ?? string.Empty;
            buttonBadge.Text = string.IsNullOrWhiteSpace(badgeText) ? "-" : badgeText;
            inputValue.PlaceholderText = placeholderText ?? string.Empty;

            _configuredButtonText = string.IsNullOrWhiteSpace(buttonText)
                ? "执行"
                : buttonText;

            _configuredBadgeType = ResolveBadgeType(accentType);

            // 初始化时先按不可执行态渲染。
            ApplyAppearance(false, _configuredBadgeType);
        }

        /// <summary>
        /// 绑定运行态动作项。
        ///
        /// 该方法只负责：
        /// 1. 保存动作项；
        /// 2. 刷新当前卡片是否可执行；
        /// 3. 若动作项有更新后的分类或显示名，则同步到界面。
        ///
        /// 注意：
        /// 参数卡片是页面固定结构的一部分，不能因搜索或临时状态而隐藏。
        /// 因此 item 为 null 时，只刷新成不可执行态，不隐藏控件。
        /// </summary>
        public void BindItem(MotionAxisPageModel.MotionAxisActionViewItem item)
        {
            _item = item;

            if (item == null)
            {
                ApplyAppearance(false, _configuredBadgeType);
                return;
            }

            if (!string.IsNullOrWhiteSpace(item.ActionKey))
                ActionKey = item.ActionKey;

            if (!string.IsNullOrWhiteSpace(item.CategoryText))
                buttonBadge.Text = item.CategoryText;

            if (!string.IsNullOrWhiteSpace(item.DisplayText))
                _configuredButtonText = item.DisplayText;

            ApplyAppearance(item.CanExecute, ResolveBadgeType(item.AccentType));
        }

        /// <summary>
        /// 绑定控件内部事件。
        /// 当前仅监听右下角动作按钮点击事件。
        /// </summary>
        private void BindEvents()
        {
            buttonConfirm.Click += ButtonConfirm_Click;
        }

        /// <summary>
        /// 动作按钮点击处理。
        ///
        /// 规则：
        /// 1. 未绑定动作项时忽略；
        /// 2. 当前动作不可执行时忽略；
        /// 3. 真正的动作执行由页面层统一处理，本控件只抛出动作键。
        /// </summary>
        private void ButtonConfirm_Click(object sender, EventArgs e)
        {
            if (_item == null || !_item.CanExecute)
                return;

            var handler = ExecuteRequested;
            if (handler != null)
                handler(this, new MotionAxisParameterActionExecuteRequestedEventArgs(ActionKey));
        }

        /// <summary>
        /// 根据可执行状态与徽标类型刷新界面外观。
        ///
        /// 外观规则：
        /// 1. 左上角徽标通过 Type 呈现颜色语义，不再手工设置 BackColor；
        /// 2. 右下角动作按钮始终显示真实动作名；
        /// 3. 禁用态仅通过按钮 Enabled 呈现，不改变按钮文案；
        /// 4. 徽标按钮不参与点击逻辑，仅保留样式用途。
        /// </summary>
        private void ApplyAppearance(bool canExecute, TTypeMini badgeType)
        {
            buttonBadge.Type = badgeType;

            buttonConfirm.Enabled = canExecute;
            buttonConfirm.Text = string.IsNullOrWhiteSpace(_configuredButtonText)
                ? "执行"
                : _configuredButtonText;
        }

        /// <summary>
        /// 将动作强调类型转换为 AntdUI 内置按钮类型。
        /// 这样可以直接复用 AntdUI 的标准配色，避免额外维护颜色映射。
        /// </summary>
        private static TTypeMini ResolveBadgeType(string accentType)
        {
            if (string.Equals(accentType, "Danger", StringComparison.OrdinalIgnoreCase))
                return TTypeMini.Error;

            if (string.Equals(accentType, "Warning", StringComparison.OrdinalIgnoreCase))
                return TTypeMini.Warn;

            if (string.Equals(accentType, "Success", StringComparison.OrdinalIgnoreCase))
                return TTypeMini.Success;

            if (string.Equals(accentType, "Default", StringComparison.OrdinalIgnoreCase))
                return TTypeMini.Default;

            return TTypeMini.Primary;
        }

        /// <summary>
        /// 参数动作执行请求事件。
        /// 页面层订阅后，统一读取各卡片输入值并调用页面模型执行动作。
        /// </summary>
        public event EventHandler<MotionAxisParameterActionExecuteRequestedEventArgs> ExecuteRequested;

        /// <summary>
        /// 参数动作执行请求事件参数。
        /// 仅携带动作键，具体参数值由页面层从各卡片控件读取。
        /// </summary>
        public sealed class MotionAxisParameterActionExecuteRequestedEventArgs : EventArgs
        {
            public MotionAxisParameterActionExecuteRequestedEventArgs(string actionKey)
            {
                ActionKey = actionKey;
            }

            public string ActionKey { get; private set; }
        }
    }
}