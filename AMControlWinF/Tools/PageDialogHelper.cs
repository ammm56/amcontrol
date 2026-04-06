using AntdUI;
using System.Drawing;
using System.Windows.Forms;

namespace AMControlWinF.Tools
{
    /// <summary>
    /// 页面级对话辅助。
    /// 仅用于当前页面/当前窗口内的临时交互式弹窗，不走系统消息通知链路。
    /// </summary>
    public static class PageDialogHelper
    {
        /// <summary>
        /// 页面信息提示，不显示底部按钮。
        /// </summary>
        public static DialogResult ShowInfo(Control owner, string title, string content)
        {
            return ShowMessage(owner, title, content, TType.Info);
        }

        /// <summary>
        /// 页面警告提示，不显示底部按钮。
        /// </summary>
        public static DialogResult ShowWarn(Control owner, string title, string content)
        {
            return ShowMessage(owner, title, content, TType.Warn);
        }

        /// <summary>
        /// 页面错误提示，不显示底部按钮。
        /// </summary>
        public static DialogResult ShowError(Control owner, string title, string content)
        {
            return ShowMessage(owner, title, content, TType.Error);
        }

        /// <summary>
        /// 弹出自定义详情面板。
        /// </summary>
        public static DialogResult ShowPanel(
            Control owner,
            string title,
            Control content,
            int width = 900)
        {
            title = string.IsNullOrWhiteSpace(title) ? "详情" : title;
            if (content == null)
                return DialogResult.None;

            content.Margin = new Padding(0);

            var window = ResolveOwnerWindow(owner);
            if (window == null)
            {
                return AntdUI.Modal.open(new AntdUI.Modal.Config(title, content)
                {
                    Width = width,
                    BtnHeight = 0,
                    CloseIcon = true,
                    Keyboard = true,
                    MaskClosable = true,
                    Draggable = true,
                    Padding = new Size(20, 16),
                    ContentPadding = new Size(0, 0),
                    UseIconPadding = false,
                    DefaultFocus = false
                });
            }

            return AntdUI.Modal.open(new AntdUI.Modal.Config(window, title, (object)content)
            {
                Width = width,
                BtnHeight = 0,
                CloseIcon = true,
                Keyboard = true,
                MaskClosable = true,
                Draggable = true,
                Padding = new Size(20, 16),
                ContentPadding = new Size(0, 0),
                UseIconPadding = false,
                DefaultFocus = false
            });
        }

        /// <summary>
        /// 统一弹出页面详情 Popover。
        /// 包含：内容尺寸设置、DPI 适配、方向计算、关闭释放。
        /// </summary>
        public static Form ShowDetailPopover(
            Control owner,
            Control anchorControl,
            Control content,
            Size contentSize)
        {
            return ShowPopoverPanel(owner, anchorControl, content, contentSize);
        }

        /// <summary>
        /// 统一弹出 Popover 面板。
        /// </summary>
        public static Form ShowPopoverPanel(
            Control owner,
            Control anchorControl,
            Control content,
            Size contentSize,
            int radius = 10,
            int gap = 6,
            bool focus = false)
        {
            if (anchorControl == null || content == null)
                return null;

            content.Margin = new Padding(0);
            content.Size = contentSize;
            content.MinimumSize = contentSize;

            var window = ResolveOwnerWindow(owner) ?? ResolveOwnerWindow(anchorControl);
            if (window != null)
            {
                window.AutoDpi(content);
            }

            var align = ResolvePopoverAlign(owner, anchorControl, content.Size);

            return AntdUI.Popover.open(new AntdUI.Popover.Config(anchorControl, content)
            {
                ArrowAlign = align,
                Radius = radius,
                Padding = new Size(16, 16),
                Gap = gap,
                Focus = focus,
                OnClosing = (s, e) =>
                {
                    if (content != null && !content.IsDisposed)
                        content.Dispose();
                }
            });
        }

        /// <summary>
        /// 页面确认框，返回是否点击确定。
        /// </summary>
        public static bool Confirm(
            Control owner,
            string title,
            string content,
            TType type = TType.Warn,
            string okText = "确定",
            string cancelText = "取消")
        {
            return ShowConfirm(owner, title, content, type, okText, cancelText) == DialogResult.OK;
        }

        /// <summary>
        /// 页面确认框，返回原始结果。
        /// </summary>
        public static DialogResult ConfirmResult(
            Control owner,
            string title,
            string content,
            TType type = TType.Warn,
            string okText = "确定",
            string cancelText = "取消")
        {
            return ShowConfirm(owner, title, content, type, okText, cancelText);
        }

        private static DialogResult ShowMessage(Control owner, string title, string content, TType type)
        {
            title = string.IsNullOrWhiteSpace(title) ? "提示" : title;
            content = content ?? string.Empty;

            var window = ResolveOwnerWindow(owner);
            if (window == null)
            {
                return MessageBox.Show(
                    content,
                    title,
                    MessageBoxButtons.OK,
                    ToMessageBoxIcon(type));
            }

            return AntdUI.Modal.open(new AntdUI.Modal.Config(window, title, content, type)
            {
                BtnHeight = 0,
                CloseIcon = true,
                Keyboard = true,
                MaskClosable = true,
                Draggable = true,
                Padding = new Size(24, 20)
            });
        }

        private static DialogResult ShowConfirm(
            Control owner,
            string title,
            string content,
            TType type,
            string okText,
            string cancelText)
        {
            title = string.IsNullOrWhiteSpace(title) ? "确认" : title;
            content = content ?? string.Empty;
            okText = string.IsNullOrWhiteSpace(okText) ? "确定" : okText;
            cancelText = string.IsNullOrWhiteSpace(cancelText) ? "取消" : cancelText;

            var window = ResolveOwnerWindow(owner);
            if (window == null)
            {
                return MessageBox.Show(
                    content,
                    title,
                    MessageBoxButtons.OKCancel,
                    ToMessageBoxIcon(type));
            }

            return AntdUI.Modal.open(new AntdUI.Modal.Config(window, title, content, type)
            {
                OkText = okText,
                CancelText = cancelText,
                CloseIcon = true,
                Keyboard = true,
                MaskClosable = false,
                Draggable = true,
                Padding = new Size(24, 20)
            });
        }

        /// <summary>
        /// 计算 Popover 最合适的箭头方向。
        /// 规则：优先落在可用空间更大的象限。
        /// </summary>
        private static TAlign ResolvePopoverAlign(Control owner, Control anchorControl, Size popupSize)
        {
            var host = ResolveHostForm(owner, anchorControl);
            if (host == null || anchorControl == null)
                return TAlign.LT;

            var anchorScreenRect = anchorControl.RectangleToScreen(anchorControl.ClientRectangle);
            var anchorRect = new Rectangle(
                host.PointToClient(anchorScreenRect.Location),
                anchorScreenRect.Size);

            var hostClientRect = host.ClientRectangle;

            var spaceLeft = anchorRect.Left;
            var spaceRight = hostClientRect.Width - anchorRect.Right;
            var spaceTop = anchorRect.Top;
            var spaceBottom = hostClientRect.Height - anchorRect.Bottom;

            var showOnRight = spaceRight >= popupSize.Width || spaceRight >= spaceLeft;
            var showBelow = spaceBottom >= popupSize.Height || spaceBottom >= spaceTop;

            if (showOnRight && showBelow)
                return TAlign.LT;

            if (showOnRight && !showBelow)
                return TAlign.LB;

            if (!showOnRight && showBelow)
                return TAlign.RT;

            return TAlign.RB;
        }

        private static Form ResolveHostForm(Control owner, Control anchorControl)
        {
            if (owner != null)
            {
                var ownerForm = owner.FindForm();
                if (ownerForm != null)
                    return ownerForm;
            }

            if (anchorControl != null)
                return anchorControl.FindForm();

            return null;
        }

        private static AntdUI.Window ResolveOwnerWindow(Control owner)
        {
            if (owner == null)
                return null;

            var window = owner as AntdUI.Window;
            if (window != null)
                return window;

            return owner.FindForm() as AntdUI.Window;
        }

        private static MessageBoxIcon ToMessageBoxIcon(TType type)
        {
            switch (type)
            {
                case TType.Error:
                    return MessageBoxIcon.Error;
                case TType.Warn:
                    return MessageBoxIcon.Warning;
                case TType.Success:
                case TType.Info:
                default:
                    return MessageBoxIcon.Information;
            }
        }
    }
}