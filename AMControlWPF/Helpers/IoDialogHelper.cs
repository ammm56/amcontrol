using AM.DBService.Services.Motion.Topology;
using AM.Model.Entity.Motion.Topology;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace AMControlWPF.Helpers
{
    /// <summary>
    /// IO 下拉选项数据模型。
    /// </summary>
    public class IoComboItem
    {
        public short? Value { get; set; }
        public string Display { get; set; }
    }

    /// <summary>
    /// 执行器编辑对话框 IO 下拉辅助类，统一处理 IO 选项加载、绑定与读取。
    /// 依赖 System.Windows.Controls.ComboBox，必须保留在 AMControlWPF 层。
    /// </summary>
    internal static class IoDialogHelper
    {
        private const string NullDisplay = "── 不配置 ──";

        /// <summary>从数据库加载所有 IO 映射记录，失败时返回空列表。</summary>
        public static IReadOnlyList<MotionIoMapEntity> LoadAll()
        {
            try
            {
                var result = new MotionIoMapCrudService().QueryAll();
                return result.Success && result.Items != null
                    ? result.Items
                    : new List<MotionIoMapEntity>();
            }
            catch
            {
                return new List<MotionIoMapEntity>();
            }
        }

        /// <summary>
        /// 按 IoType 过滤并构建下拉项列表。
        /// </summary>
        /// <param name="all">全部 IO 映射记录。</param>
        /// <param name="ioType">"DO" 或 "DI"。</param>
        /// <param name="nullable">是否在首位插入「不配置」选项（用于可空字段）。</param>
        public static List<IoComboItem> BuildItems(IReadOnlyList<MotionIoMapEntity> all, string ioType, bool nullable)
        {
            var list = new List<IoComboItem>();
            if (nullable)
                list.Add(new IoComboItem { Value = null, Display = NullDisplay });

            foreach (var m in all.Where(x => x.IoType == ioType).OrderBy(x => x.LogicalBit))
            {
                list.Add(new IoComboItem
                {
                    Value = m.LogicalBit,
                    Display = string.Format("{0}#{1}  {2}", ioType, m.LogicalBit, m.Name)
                });
            }
            return list;
        }

        /// <summary>将下拉项绑定到 ComboBox 并按当前值预选。</summary>
        public static void Apply(ComboBox cb, List<IoComboItem> items, short? currentValue)
        {
            cb.ItemsSource = items;
            cb.DisplayMemberPath = "Display";
            SelectByValue(cb, currentValue);
        }

        /// <summary>按逻辑位号选中 ComboBox 中的项，找不到时选首项。</summary>
        public static void SelectByValue(ComboBox cb, short? value)
        {
            foreach (IoComboItem item in cb.Items)
            {
                if (item.Value == value)
                {
                    cb.SelectedItem = item;
                    return;
                }
            }
            if (cb.Items.Count > 0)
                cb.SelectedIndex = 0;
        }

        /// <summary>读取 ComboBox 当前选中的逻辑位号，「不配置」返回 null。</summary>
        public static short? GetValue(ComboBox cb)
        {
            return (cb.SelectedItem as IoComboItem)?.Value;
        }
    }
}