namespace AM.Model.Common
{
    /// <summary>
    /// 错误描述信息
    /// </summary>
    public class ErrorDescriptor
    {
        public int Code { get; set; }

        public string Name { get; set; }

        public string Message { get; set; }

        public string Description { get; set; }

        public string Source { get; set; }

        public string Suggestion { get; set; }
    }
}