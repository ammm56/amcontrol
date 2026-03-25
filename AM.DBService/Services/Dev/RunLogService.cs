using AM.Model.Common;
using AM.Model.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AM.DBService.Services.Dev
{
    /// <summary>
    /// 运行日志文件读取服务（基于 NLog 输出文件）。
    /// 日志格式：{longdate}|{level:uppercase=true}|{logger}|{message}
    /// 使用 FileShare.ReadWrite 避免与 NLog 写入时发生文件锁冲突。
    /// </summary>
    public class RunLogService
    {
        private readonly string _logDirectory;

        /// <summary>日志文件元信息。</summary>
        public class LogFileInfo
        {
            /// <summary>文件完整路径。</summary>
            public string FilePath { get; set; }

            /// <summary>UI 显示名称（文件名）。</summary>
            public string DisplayName { get; set; }
        }

        public RunLogService()
        {
            _logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
        }

        /// <summary>
        /// 获取日志目录下所有 *.log 文件，按修改时间倒序排列（最新在最前）。
        /// </summary>
        public List<LogFileInfo> GetAvailableLogFiles()
        {
            var result = new List<LogFileInfo>();
            try
            {
                if (!Directory.Exists(_logDirectory))
                {
                    return result;
                }

                var files = Directory.GetFiles(_logDirectory, "*.log", SearchOption.TopDirectoryOnly);
                foreach (var file in files.OrderByDescending(f => File.GetLastWriteTime(f)))
                {
                    result.Add(new LogFileInfo
                    {
                        FilePath = file,
                        DisplayName = Path.GetFileName(file)
                    });
                }
            }
            catch { }

            return result;
        }

        /// <summary>
        /// 读取指定日志文件，解析为日志条目列表（最新行在最前）。
        /// </summary>
        /// <param name="filePath">日志文件完整路径。</param>
        /// <param name="maxLines">最大读取行数，默认 5000。</param>
        public Result<LogModel> ReadFile(string filePath, int maxLines = 5000)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                {
                    return Result<LogModel>.Fail(-1, "日志文件不存在: " + filePath);
                }

                var allLines = new List<string>();
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = new StreamReader(stream))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        allLines.Add(line);
                    }
                }

                // 取最后 maxLines 行，倒序使最新在最前
                var linesToProcess = allLines.Count > maxLines
                    ? allLines.GetRange(allLines.Count - maxLines, maxLines)
                    : allLines;

                linesToProcess.Reverse();

                var entries = new List<LogModel>();
                for (int i = 0; i < linesToProcess.Count; i++)
                {
                    var entry = ParseLine(linesToProcess[i]);
                    entry.LineNumber = allLines.Count - i;
                    entries.Add(entry);
                }

                return Result<LogModel>.OkList(entries, "日志文件读取成功")
                    .WithNotifyMode(ResultNotifyMode.Silent);
            }
            catch (Exception ex)
            {
                return Result<LogModel>.Fail(-1, "日志文件读取失败: " + ex.Message);
            }
        }

        private LogModel ParseLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                return new LogModel { RawLine = line ?? string.Empty, IsParsed = false };
            }

            var parts = line.Split(new[] { '|' }, 4);
            if (parts.Length < 4)
            {
                return new LogModel { RawLine = line, Message = line, IsParsed = false };
            }

            DateTime time;
            var parsed = DateTime.TryParse(parts[0].Trim(), out time);

            return new LogModel
            {
                Time = parsed ? time : (DateTime?)null,
                Level = parts[1].Trim(),
                Logger = parts[2].Trim(),
                Message = parts[3].Trim(),
                RawLine = line,
                IsParsed = parsed
            };
        }
    }
}