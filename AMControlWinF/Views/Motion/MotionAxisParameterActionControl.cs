using AM.PageModel.Motion.Axis;
using AntdUI;
using System;
using System.Windows.Forms;

namespace AMControlWinF.Views.Motion
{
    /// <summary>
    /// 单个参数动作卡片控件。
    /// 一个控件只表示一个参数动作，例如：应用速度、绝对定位、相对移动。
    ///
    /// 【当前职责】
    /// 1. 承载参数输入框和确认按钮；
    /// 2. 绑定页面模型给出的动作显示状态；
    /// 3. 用户点击后抛出参数动作执行请求，不直接执行业务逻辑。
    ///
    /// 【层级关系】
    /// - 上游：MotionAxisPage、MotionAxisPageModel；
    /// - 当前层：WinForms 参数动作卡片控件；
    /// - 下游：参数动作执行请求事件。
    /// </summary>
    public partial class MotionAxisParameterActionControl : UserControl
    {
        private MotionAxisActionViewItem _item;
        private TTypeMini _badgeType = TTypeMini.Primary;
        private string _buttonText = string.Empty;

        #region 构造与属性

        public MotionAxisParameterActionControl()
        {
            InitializeComponent();
            buttonConfirm.Click += ButtonConfirm_Click;
        }

        /// <summary>
        /// 当前卡片对应的动作键。
        /// </summary>
        public MotionAxisActionKey ActionKey { get; private set; }

        /// <summary>
        /// 当前输入框文本。
        /// 页面层统一从这里读取参数。
        /// </summary>
        public string InputText
        {
            get { return inputValue.Text; }
            set { inputValue.Text = value ?? string.Empty; }
        }

        #endregion

        #region 初始化与绑定

        /// <summary>
        /// 初始化卡片静态信息。
        /// 推荐由页面直接传入动作枚举。
        /// </summary>
        public void Configure(
            MotionAxisActionKey actionKey,
            string badgeText,
            string buttonText,
            string placeholderText,
            string accentType)
        {
            ActionKey = actionKey;
            buttonBadge.Text = string.IsNullOrWhiteSpace(badgeText) ? "-" : badgeText;
            inputValue.PlaceholderText = placeholderText ?? string.Empty;

            _buttonText = string.IsNullOrWhiteSpace(buttonText) ? "执行" : buttonText;
            _badgeType = ResolveBadgeType(accentType);

            ApplyAppearance(false, _badgeType);
        }

        /// <summary>
        /// 绑定运行态动作项，只刷新状态，不做业务执行。
        /// </summary>
        public void BindItem(MotionAxisActionViewItem item)
        {
            _item = item;

            if (item == null)
            {
                ApplyAppearance(false, _badgeType);
                return;
            }

            ActionKey = item.ActionKey;

            if (!string.IsNullOrWhiteSpace(item.CategoryText))
                buttonBadge.Text = item.CategoryText;

            if (!string.IsNullOrWhiteSpace(item.DisplayText))
                _buttonText = item.DisplayText;

            ApplyAppearance(item.CanExecute, ResolveBadgeType(item.AccentType));
        }

        #endregion

        #region 控件事件

        private void ButtonConfirm_Click(object sender, EventArgs e)
        {
            if (_item == null || !_item.CanExecute)
                return;

            ExecuteRequested?.Invoke(
                this,
                new MotionAxisParameterActionExecuteRequestedEventArgs(ActionKey));
        }

        /// <summary>
        /// 只刷新界面状态：
        /// - 徽标颜色通过 Type 控制；
        /// - 按钮始终显示动作名；
        /// - 禁用态只通过 Enabled 表达。
        /// </summary>
        private void ApplyAppearance(bool canExecute, TTypeMini badgeType)
        {
            buttonBadge.Type = badgeType;

            buttonConfirm.Enabled = canExecute;
            buttonConfirm.Text = string.IsNullOrWhiteSpace(_buttonText)
                ? "执行"
                : _buttonText;
        }

        /// <summary>
        /// 动作强调类型映射到 AntdUI 内置按钮类型。
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

        #endregion

        #region 事件与参数

        public event EventHandler<MotionAxisParameterActionExecuteRequestedEventArgs> ExecuteRequested;

        public sealed class MotionAxisParameterActionExecuteRequestedEventArgs : EventArgs
        {
            public MotionAxisParameterActionExecuteRequestedEventArgs(MotionAxisActionKey actionKey)
            {
                ActionKey = actionKey;
            }

            public MotionAxisActionKey ActionKey { get; private set; }
        }

        #endregion
    }
}