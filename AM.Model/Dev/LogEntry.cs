using System;

namespace AM.Model.Dev
{
    /// <summary>
    /// 运行日志条目（从 NLog 日志文件逐行解析得到）。
    /// </summary>
    public class LogEntry
    {
        /// <summary>在文件中的行号（从 1 开始）。</summary>
        public int LineNumber { get; set; }

        /// <summary>日志时间（解析失败则为 null）。</summary>
        public DateTime? Time { get; set; }

        /// <summary>日志级别，如 INFO / WARN / ERROR / DEBUG / TRACE / FATAL。</summary>
        public string Level { get; set; }

        /// <summary>日志来源（Logger 名称）。</summary>
        public string Logger { get; set; }

        /// <summary>日志消息正文。</summary>
        public string Message { get; set; }

        /// <summary>原始行文本（解析失败时保留原始内容）。</summary>
        public string RawLine { get; set; }

        /// <summary>是否成功解析为结构化数据。</summary>
        public bool IsParsed { get; set; }

        /// <summary>格式化的时间文本，用于 UI 列显示。</summary>
        public string TimeText
        {
            get
            {
                return Time.HasValue
                    ? Time.Value.ToString("HH:mm:ss.fff")
                    : string.Empty;
            }
        }
    }
}