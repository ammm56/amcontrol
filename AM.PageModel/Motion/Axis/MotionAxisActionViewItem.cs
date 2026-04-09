using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.PageModel.Motion.Axis
{
    /// <summary>
    /// 左侧动作卡片显示项。
    /// 卡片外观保持统一按钮式小卡片风格，只承载当前动作的执行状态。
    /// </summary>
    public sealed class MotionAxisActionViewItem
    {
        public string ActionKey { get; set; }
        public string DisplayText { get; set; }
        public string CategoryText { get; set; }
        public string AccentType { get; set; }
        public bool CanExecute { get; set; }
        public string DisabledReason { get; set; }
    }
}
