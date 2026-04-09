using System;

namespace AM.PageModel.Motion.Actuator
{
    /// <summary>
    /// 执行器页面筛选条件对象。
    ///
    /// 【层级定位】
    /// - 所在层：页面模型辅助对象；
    /// - 上游来源：MotionActuatorPageModel；
    /// - 下游去向：Snapshot / ListItem 过滤逻辑。
    ///
    /// 【职责】
    /// 1. 收口页面筛选条件；
    /// 2. 避免 PageModel 中长期散落多个筛选字段；
    /// 3. 让筛选逻辑的输入结构清晰稳定。
    ///
    /// 【当前阶段说明】
    /// 第一阶段只保留：
    /// - SearchText
    /// - TypeFilter
    ///
    /// 后续如果增加：
    /// - 是否仅显示故障对象
    /// - 是否仅显示启用对象
    /// - 是否按控制卡过滤
    /// 都可以继续加到该对象中，而不是继续摊在 PageModel 成员上。
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
        /// 这里优先基于 Snapshot 自身已有文本字段做匹配，
        /// 第一阶段不额外依赖 DisplayBuilder。
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