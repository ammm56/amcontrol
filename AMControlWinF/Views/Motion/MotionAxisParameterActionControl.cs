using AM.PageModel.Motion;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AMControlWinF.Views.Motion
{
    /// <summary>
    /// 单个参数动作卡片控件。
    ///
    /// 设计目的：
    /// 1. 一个控件只表示一个参数动作，例如“应用速度”“绝对定位”“相对移动”；
    /// 2. 控件内部只包含：左上角分类徽标、参数输入框、右下角动作按钮；
    /// 3. 按钮文案直接显示实际动作名，而不是“确认”；
    /// 4. 控件本身不负责业务校验与动作执行，只负责展示和抛出点击事件；
    /// 5. 是否可执行由页面层传入的动作项决定，控件只根据状态更新外观。
    /// </summary>
    public partial class MotionAxisParameterActionControl : UserControl
    {
        /// <summary>
        /// 当前绑定的动作项。
        /// 来自 MotionAxisPageModel，用于承载当前动作是否可执行等运行态信息。
        /// </summary>
        private MotionAxisPageModel.MotionAxisActionViewItem _item;

        /// <summary>
        /// 配置阶段传入的强调色类型。
        /// 当运行态动作项未提供强调色时，回退使用此值。
        /// </summary>
        private string _configuredAccentType = "Primary";

        /// <summary>
        /// 按钮文案。
        /// 这里明确保存“动作名”，例如：
        /// - 应用速度
        /// - 绝对定位
        /// - 相对移动
        ///
        /// 注意：
        /// 按钮始终显示动作名，不再切换成“确认”“不可用”等文案，
        /// 避免用户只能看到按钮状态却不知道当前卡片对应哪个动作。
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
        /// 配置当前卡片的静态信息。
        ///
        /// 说明：
        /// - actionKey：动作标识
        /// - badgeText：左上角小徽标文本，如“参数”“定位”
        /// - buttonText：右下角按钮文案，直接使用实际动作名
        /// - placeholderText：输入框占位提示
        /// - accentType：强调色类型
        ///
        /// 该方法一般在页面初始化时调用一次。
        /// </summary>
        public void Configure(
            string actionKey,
            string badgeText,
            string buttonText,
            string placeholderText,
            string accentType)
        {
            ActionKey = actionKey ?? string.Empty;
            labelBadge.Text = string.IsNullOrWhiteSpace(badgeText) ? "-" : badgeText;
            inputValue.PlaceholderText = placeholderText ?? string.Empty;

            _configuredAccentType = string.IsNullOrWhiteSpace(accentType)
                ? "Primary"
                : accentType;

            _configuredButtonText = string.IsNullOrWhiteSpace(buttonText)
                ? "执行"
                : buttonText;

            // 初始配置完成后先以“不可执行”视觉呈现。
            // 真正的可执行状态由 BindItem(...) 在运行时刷新。
            ApplyAppearance(false, _configuredAccentType);
        }

        /// <summary>
        /// 绑定运行态动作项。
        ///
        /// 该方法只负责：
        /// 1. 保存动作项；
        /// 2. 根据动作项刷新可执行状态；
        /// 3. 若动作项带有更实时的分类/显示名，则同步更新展示。
        ///
        /// 不负责：
        /// - 解析输入值
        /// - 直接执行业务动作
        /// </summary>
        public void BindItem(MotionAxisPageModel.MotionAxisActionViewItem item)
        {
            _item = item;

            // 参数动作卡片是页面固定结构的一部分，不应因为搜索或暂时未绑定而隐藏。
            // 因此这里不再把 Visible 绑定到 item 是否为空，而是始终保持控件可见。
            if (item == null)
            {
                ApplyAppearance(false, _configuredAccentType);
                return;
            }

            if (!string.IsNullOrWhiteSpace(item.ActionKey))
                ActionKey = item.ActionKey;

            // 左上角徽标优先展示当前动作项的分类文本，
            // 这样当页面模型调整分类文案时，UI 可自动同步。
            if (!string.IsNullOrWhiteSpace(item.CategoryText))
                labelBadge.Text = item.CategoryText;

            // 按钮文案优先使用动作项显示名。
            // 这样可以确保页面模型与卡片文案保持一致。
            if (!string.IsNullOrWhiteSpace(item.DisplayText))
                _configuredButtonText = item.DisplayText;

            ApplyAppearance(item.CanExecute, item.AccentType);
        }

        /// <summary>
        /// 绑定控件内部事件。
        /// 当前只有一个动作按钮点击事件。
        /// </summary>
        private void BindEvents()
        {
            buttonConfirm.Click += ButtonConfirm_Click;
        }

        /// <summary>
        /// 动作按钮点击处理。
        ///
        /// 规则：
        /// - 未绑定动作项时忽略；
        /// - 当前动作不可执行时忽略；
        /// - 真正的动作执行由页面层统一处理，控件仅抛出 ExecuteRequested 事件。
        /// </summary>
        private void ButtonConfirm_Click(object sender, EventArgs e)
        {
            if (_item == null || !_item.CanExecute)
                return;

            var handler = ExecuteRequested;
            if (handler != null)
            {
                handler(this, new MotionAxisParameterActionExecuteRequestedEventArgs(ActionKey));
            }
        }

        /// <summary>
        /// 根据当前可执行状态与强调色刷新界面外观。
        ///
        /// 设计约束：
        /// 1. 左上角徽标颜色随动作类型变化；
        /// 2. 右下角按钮文案始终显示动作名；
        /// 3. 禁用态只通过 Enabled 呈现，不修改按钮文案；
        /// 4. 不在这里混入业务含义，仅做纯视觉更新。
        /// </summary>
        private void ApplyAppearance(bool canExecute, string accentType)
        {
            var accent = ResolveAccentColor(
                string.IsNullOrWhiteSpace(accentType) ? _configuredAccentType : accentType);

            labelBadge.BackColor = accent;

            buttonConfirm.Enabled = canExecute;
            buttonConfirm.Text = string.IsNullOrWhiteSpace(_configuredButtonText)
                ? "执行"
                : _configuredButtonText;
        }

        /// <summary>
        /// 将动作强调类型转换为实际颜色。
        /// 与上方简单动作卡片保持同一套颜色语义：
        /// - Danger：危险/急停
        /// - Warning：警告/回零等
        /// - Success：成功/参数应用
        /// - Default：普通灰色
        /// - Primary：主色蓝
        /// </summary>
        private static Color ResolveAccentColor(string accentType)
        {
            if (string.Equals(accentType, "Danger", StringComparison.OrdinalIgnoreCase))
                return Color.FromArgb(245, 108, 108);

            if (string.Equals(accentType, "Warning", StringComparison.OrdinalIgnoreCase))
                return Color.FromArgb(250, 140, 22);

            if (string.Equals(accentType, "Success", StringComparison.OrdinalIgnoreCase))
                return Color.FromArgb(82, 196, 26);

            if (string.Equals(accentType, "Default", StringComparison.OrdinalIgnoreCase))
                return Color.FromArgb(96, 125, 139);

            return Color.FromArgb(22, 119, 255);
        }

        /// <summary>
        /// 参数动作执行请求事件。
        /// 页面层订阅此事件后，统一读取各参数卡片输入值并调用页面模型执行动作。
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