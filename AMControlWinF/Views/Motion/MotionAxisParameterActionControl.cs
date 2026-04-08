using AM.PageModel.Motion;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AMControlWinF.Views.Motion
{
    /// <summary>
    /// 单轴控制参数动作区。
    ///
    /// 职责：
    /// 1. 只承载参数动作卡片；
    /// 2. 提供输入框 + 确认按钮；
    /// 3. 与简单动作虚拟卡片列表分离，保持结构清晰；
    /// 4. 页面层从本控件读取参数并执行动作。
    /// </summary>
    public partial class MotionAxisParameterActionControl : UserControl
    {
        private MotionAxisPageModel.MotionAxisActionViewItem _applyVelocityItem;
        private MotionAxisPageModel.MotionAxisActionViewItem _moveAbsoluteItem;
        private MotionAxisPageModel.MotionAxisActionViewItem _moveRelativeItem;
        private short? _lastLogicalAxis;

        public MotionAxisParameterActionControl()
        {
            InitializeComponent();
            BindEvents();
        }

        public event EventHandler<MotionAxisParameterActionExecuteRequestedEventArgs> ActionExecuteRequested;

        public string VelocityText
        {
            get { return inputVelocity.Text; }
        }

        public string TargetPositionText
        {
            get { return inputTargetPosition.Text; }
        }

        public string MoveDistanceText
        {
            get { return inputMoveDistance.Text; }
        }

        /// <summary>
        /// 绑定参数动作卡片状态。
        /// </summary>
        public void BindItems(IList<MotionAxisPageModel.MotionAxisActionViewItem> items)
        {
            var sourceItems = items ?? new List<MotionAxisPageModel.MotionAxisActionViewItem>();

            _applyVelocityItem = sourceItems.FirstOrDefault(x => IsAction(x, "ApplyVelocity"));
            _moveAbsoluteItem = sourceItems.FirstOrDefault(x => IsAction(x, "MoveAbsolute"));
            _moveRelativeItem = sourceItems.FirstOrDefault(x => IsAction(x, "MoveRelative"));

            BindCard(
                panelApplyVelocityCard,
                labelApplyVelocityBadge,
                labelApplyVelocityTitle,
                buttonApplyVelocity,
                _applyVelocityItem,
                "参数",
                "应用速度");

            BindCard(
                panelMoveAbsoluteCard,
                labelMoveAbsoluteBadge,
                labelMoveAbsoluteTitle,
                buttonMoveAbsolute,
                _moveAbsoluteItem,
                "定位",
                "绝对定位");

            BindCard(
                panelMoveRelativeCard,
                labelMoveRelativeBadge,
                labelMoveRelativeTitle,
                buttonMoveRelative,
                _moveRelativeItem,
                "定位",
                "相对移动");
        }

        /// <summary>
        /// 当前轴变化时刷新默认参数。
        /// 定时刷新不覆盖用户输入。
        /// </summary>
        public void BindSelectedAxis(MotionAxisPageModel.MotionAxisSelectedViewItem axisItem)
        {
            var logicalAxis = axisItem == null ? (short?)null : axisItem.LogicalAxis;
            if (_lastLogicalAxis == logicalAxis)
                return;

            _lastLogicalAxis = logicalAxis;

            if (axisItem == null)
            {
                inputVelocity.Text = "10";
                inputTargetPosition.Text = "0";
                inputMoveDistance.Text = "10";
                return;
            }

            if (axisItem.JogVelocityMm > 0D)
                inputVelocity.Text = axisItem.JogVelocityMm.ToString("0.###");
            else if (axisItem.DefaultVelocityMm > 0D)
                inputVelocity.Text = axisItem.DefaultVelocityMm.ToString("0.###");
            else
                inputVelocity.Text = "10";

            inputTargetPosition.Text = axisItem.CommandPositionMm.ToString("0.###");
            inputMoveDistance.Text = "10";
        }

        private void BindEvents()
        {
            buttonApplyVelocity.Click += (s, e) => RaiseExecuteRequested("ApplyVelocity");
            buttonMoveAbsolute.Click += (s, e) => RaiseExecuteRequested("MoveAbsolute");
            buttonMoveRelative.Click += (s, e) => RaiseExecuteRequested("MoveRelative");
        }

        private void RaiseExecuteRequested(string actionKey)
        {
            var handler = ActionExecuteRequested;
            if (handler != null)
                handler(this, new MotionAxisParameterActionExecuteRequestedEventArgs(actionKey));
        }

        private static void BindCard(
            AntdUI.Panel cardPanel,
            AntdUI.Label badgeLabel,
            AntdUI.Label titleLabel,
            AntdUI.Button actionButton,
            MotionAxisPageModel.MotionAxisActionViewItem item,
            string badgeText,
            string titleText)
        {
            if (cardPanel == null || badgeLabel == null || titleLabel == null || actionButton == null)
                return;

            cardPanel.Visible = item != null;
            if (item == null)
                return;

            badgeLabel.Text = badgeText;
            titleLabel.Text = titleText;

            actionButton.Enabled = item.CanExecute;
            actionButton.Text = item.CanExecute ? "确认" : "不可用";
            titleLabel.ForeColor = item.CanExecute
                ? Color.FromArgb(38, 38, 38)
                : Color.FromArgb(160, 160, 160);

            badgeLabel.BackColor = ResolveAccentColor(item.AccentType);
        }

        private static bool IsAction(MotionAxisPageModel.MotionAxisActionViewItem item, string actionKey)
        {
            return item != null &&
                   string.Equals(item.ActionKey, actionKey, StringComparison.OrdinalIgnoreCase);
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