using AM.Core.Reporter;
using AM.Model.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AM.Tools.Reporter
{
    /// <summary>
    /// 基于 Configuration/errordescription.json 的错误目录实现
    /// </summary>
    public class JsonErrorCatalog : IErrorCatalog
    {
        private readonly string _filePath;
        private readonly object _lockObj = new object();
        private Dictionary<int, ErrorDescriptor> _map = new Dictionary<int, ErrorDescriptor>();

        public JsonErrorCatalog() : this(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configuration", "errordescription.json"))
        {
        }

        public JsonErrorCatalog(string filePath)
        {
            _filePath = filePath;
            Reload();
        }

        public ErrorDescriptor Get(int code)
        {
            ErrorDescriptor descriptor;
            if (TryGet(code, out descriptor))
                return descriptor;

            return new ErrorDescriptor
            {
                Code = code,
                Name = "Unknown",
                Message = "未知错误",
                Description = "未在错误目录中找到对应错误码说明。",
                Source = "System",
                Suggestion = "请补充 errordescription.json 或检查错误码来源。"
            };
        }

        public bool TryGet(int code, out ErrorDescriptor descriptor)
        {
            lock (_lockObj)
            {
                return _map.TryGetValue(code, out descriptor);
            }
        }

        public IReadOnlyList<ErrorDescriptor> GetAll()
        {
            lock (_lockObj)
            {
                return _map.Values
                    .OrderBy(p => p.Code)
                    .ToList();
            }
        }

        public void Reload()
        {
            lock (_lockObj)
            {
                _map = LoadMap(_filePath);
            }
        }

        private static Dictionary<int, ErrorDescriptor> LoadMap(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return new Dictionary<int, ErrorDescriptor>();
                }

                var json = File.ReadAllText(filePath, Encoding.UTF8);
                var list = JsonConvert.DeserializeObject<List<ErrorDescriptor>>(json) ?? new List<ErrorDescriptor>();

                return list
                    .Where(p => p != null)
                    .GroupBy(p => p.Code)
                    .ToDictionary(g => g.Key, g => g.Last());
            }
            catch
            {
                return new Dictionary<int, ErrorDescriptor>();
            }
        }
    }
}