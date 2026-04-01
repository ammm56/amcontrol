using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.PageModel.Main
{
    /// <summary>
    /// MainWindow 关闭时的退出意图。
    /// 供 Program.cs 主循环判断后续流程。
    /// </summary>
    public enum MainWindowExitReason
    {
        /// <summary>正常关闭（关闭按钮、Alt+F4）→ 退出程序。</summary>
        Exit = 0,
        /// <summary>切换用户 → 已在模态 LoginForm 中完成认证，直接重建主窗体。</summary>
        SwitchUser = 1,
        /// <summary>退出登录 → 需清除登录态并重新弹出登录窗。</summary>
        Logout = 2
    }
}
