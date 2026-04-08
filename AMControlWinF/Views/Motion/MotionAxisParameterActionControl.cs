using AM.PageModel.Motion;
using AntdUI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AMControlWinF.Views.Motion
{
    /// <summary>
    /// 单个参数动作卡片。
    /// 只负责一张卡片：徽标 + 标题 + 输入框 + 确认按钮。
    /// </summary>
    public partial class MotionAxisParameterActionControl : UserControl
    {
        private MotionAxisPageModel.MotionAxisActionViewItem _item;
        private string _configuredAccentType = "Primary";

        public MotionAxisParameterActionControl()
        {
            InitializeComponent();
            BindEvents();
        }

        /// <summary>
        /// 当前卡片绑定的动作键。
        /// </summary>
        public string ActionKey { get; private set; }

        /// <summary>
        /// 当前输入值。
        /// </summary>
        public string InputText
        {
            get { return inputValue.Text; }
            set { inputValue.Text = value ?? string.Empty; }
        }

        /// <summary>
        /// 配置卡片的静态展示信息。
        /// </summary>
        public void Configure(
            string actionKey,
            string badgeText,
            string titleText,
            string placeholderText,
            string accentType)
        {
            ActionKey = actionKey ?? string.Empty;
            labelBadge.Text = string.IsNullOrWhiteSpace(badgeText) ? "-" : badgeText;
            //labelTitle.Text = string.IsNullOrWhiteSpace(titleText) ? "-" : titleText;
            inputValue.PlaceholderText = placeholderText ?? string.Empty;
            _configuredAccentType = string.IsNullOrWhiteSpace(accentType) ? "Primary" : accentType;

            ApplyAppearance(false, _configuredAccentType);
        }

        /// <summary>
        /// 绑定运行态动作项。
        /// 用于刷新当前卡片是否可执行。
        /// </summary>
        public void BindItem(MotionAxisPageModel.MotionAxisActionViewItem item)
        {
            _item = item;
            Visible = item != null;

            if (item == null)
                return;

            if (!string.IsNullOrWhiteSpace(item.ActionKey))
                ActionKey = item.ActionKey;

            if (!string.IsNullOrWhiteSpace(item.CategoryText))
                labelBadge.Text = item.CategoryText;

            if (!string.IsNullOrWhiteSpace(item.DisplayText))
                //labelTitle.Text = item.DisplayText;

            ApplyAppearance(item.CanExecute, item.AccentType);
        }

        private void BindEvents()
        {
            buttonConfirm.Click += ButtonConfirm_Click;
        }

        private void ButtonConfirm_Click(object sender, EventArgs e)
        {
            if (_item == null || !_item.CanExecute)
                return;

            var handler = ExecuteRequested;
            if (handler != null)
                handler(this, new MotionAxisParameterActionExecuteRequestedEventArgs(ActionKey));
        }

        private void ApplyAppearance(bool canExecute, string accentType)
        {
            var accent = ResolveAccentColor(string.IsNullOrWhiteSpace(accentType) ? _configuredAccentType : accentType);

            labelBadge.BackColor = accent;
            //labelTitle.ForeColor = canExecute
            //    ? Color.FromArgb(38, 38, 38)
            //    : Color.FromArgb(160, 160, 160);

            buttonConfirm.Enabled = canExecute;
            buttonConfirm.Text = canExecute ? "确认" : "不可用";
        }

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

        public event EventHandler<MotionAxisParameterActionExecuteRequestedEventArgs> ExecuteRequested;

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