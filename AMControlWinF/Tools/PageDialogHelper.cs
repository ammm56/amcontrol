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