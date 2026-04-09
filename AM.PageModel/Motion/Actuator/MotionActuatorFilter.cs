using System;

namespace AM.PageModel.Motion.Actuator
{
    /// <summary>
    /// 执行器页面筛选条件对象。
    ///
    /// 【当前职责】
    /// 1. 收口页面筛选条件输入；
    /// 2. 避免页面模型中散落多个筛选字段；
    /// 3. 统一封装筛选规范化与匹配逻辑。
    ///
    /// 【层级关系】
    /// - 上游：MotionActuatorPageModel；
    /// - 下游：MotionActuatorSnapshot 过滤逻辑。
    /// </summary>
    public sealed class MotionActuatorFilter
    {
        /// <summary>
        /// 搜索关键字。
        /// </summary>
        public string SearchText { get; set; }

        /// <summary>
        /// 类型筛选。
        /// 约定：
        /// All / Cylinder / Vacuum / Gripper / StackLight
        /// </summary>
        public string TypeFilter { get; set; }

        public MotionActuatorFilter()
        {
            SearchText = string.Empty;
            TypeFilter = "All";
        }

        /// <summary>
        /// 规范化当前筛选条件。
        /// </summary>
        public void Normalize()
        {
            SearchText = SearchText ?? string.Empty;
            TypeFilter = string.IsNullOrWhiteSpace(TypeFilter) ? "All" : TypeFilter;
        }

        /// <summary>
        /// 判断某个原始快照是否满足当前筛选条件。
        /// 优先基于快照自身已有文本字段做匹配，不额外依赖显示映射层。
        /// </summary>
        public bool IsMatch(MotionActuatorSnapshot snapshot)
        {
            if (snapshot == null)
                return false;

            Normalize();

            if (!string.Equals(TypeFilter, "All", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(snapshot.ActuatorType, TypeFilter, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(SearchText))
                return true;

            var keyword = SearchText.Trim().ToLowerInvariant();

            return (snapshot.Name ?? string.Empty).ToLowerInvariant().Contains(keyword)
                || (snapshot.DisplayName ?? string.Empty).ToLowerInvariant().Contains(keyword)
                || (snapshot.TypeDisplay ?? string.Empty).ToLowerInvariant().Contains(keyword)
                || (snapshot.StateText ?? string.Empty).ToLowerInvariant().Contains(keyword)
                || (snapshot.CardLine1Text ?? string.Empty).ToLowerInvariant().Contains(keyword)
                || (snapshot.CardLine2Text ?? string.Empty).ToLowerInvariant().Contains(keyword)
                || (snapshot.LastActionMessage ?? string.Empty).ToLowerInvariant().Contains(keyword);
        }
    }
}