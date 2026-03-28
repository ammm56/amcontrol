namespace AM.Model.Motion
{
    /// <summary>
    /// Motion 页面控制卡筛选项。
    /// </summary>
    public class MotionCardFilterItem
    {
        public short CardId { get; set; }

        public string DisplayName { get; set; }

        public string DisplayText
        {
            get
            {
                return "卡#" + CardId + "  " + (string.IsNullOrWhiteSpace(DisplayName) ? "未命名控制卡" : DisplayName);
            }
        }
    }
}