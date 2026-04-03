using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AMControlWinF.Tools
{
    /// <summary>
    /// 动态控件安全移除与延迟释放帮助类。
    /// 用于规避 AntdUI.Panel 在消息循环中访问已释放对象导致的 ObjectDisposedException。
    /// </summary>
    public static class ControlDisposeHelper
    {
        /// <summary>
        /// 安全清空容器中的全部子控件。
        /// 先从父容器移除，再延迟 Dispose。
        /// </summary>
        public static void ClearControlsSafely(Control parent)
        {
            if (parent == null)
                return;

            var controls = parent.Controls.OfType<Control>().ToList();
            if (controls.Count == 0)
                return;

            RemoveControlsSafely(parent, controls, parent);
        }

        /// <summary>
        /// 安全移除指定控件集合。
        /// </summary>
        public static void RemoveControlsSafely(Control parent, IList<Control> controls, Control invoker)
        {
            if (parent == null || controls == null || controls.Count == 0)
                return;

            var removedControls = new List<Control>();

            foreach (var control in controls)
            {
                if (control == null)
                    continue;

                if (parent.Controls.Contains(control))
                {
                    parent.Controls.Remove(control);
                    removedControls.Add(control);
                }
            }

            DisposeControlsDeferred(invoker ?? parent, removedControls);
        }

        /// <summary>
        /// 延迟释放控件，尽量避开当前消息循环。
        /// </summary>
        public static void DisposeControlsDeferred(Control invoker, IList<Control> controls)
        {
            if (controls == null || controls.Count == 0)
                return;

            if (invoker != null && !invoker.IsDisposed && invoker.IsHandleCreated)
            {
                try
                {
                    invoker.BeginInvoke(new Action(() => DisposeControlsImmediately(controls)));
                    return;
                }
                catch
                {
                    return;
                }
            }

            var fallbackForm = Application.OpenForms
                .Cast<Form>()
                .FirstOrDefault(f => f != null && !f.IsDisposed && f.IsHandleCreated);

            if (fallbackForm != null)
            {
                try
                {
                    fallbackForm.BeginInvoke(new Action(() => DisposeControlsImmediately(controls)));
                    return;
                }
                catch
                {
                    return;
                }
            }

            DisposeControlsImmediately(controls);
        }

        /// <summary>
        /// 立即释放控件。仅在无法延迟派发时兜底使用。
        /// </summary>
        public static void DisposeControlsImmediately(IList<Control> controls)
        {
            if (controls == null || controls.Count == 0)
                return;

            foreach (var control in controls)
            {
                try
                {
                    if (control != null && !control.IsDisposed)
                        control.Dispose();
                }
                catch
                {
                    return;
                }
            }
        }
    }
}