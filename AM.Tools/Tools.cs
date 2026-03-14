using AM.Core.Context;
using AM.Model.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace AM.Tools
{
    /// <summary>
    /// 工具
    /// 静态方法
    /// </summary>
    public class Tools
    {
        private static void ReportInfo(string message, string code = null)
        {
            SystemContext.Instance.Reporter?.Info("Tools", message, code);
        }

        private static void ReportWarn(string message, string code = null)
        {
            SystemContext.Instance.Reporter?.Warn("Tools", message, code);
        }

        private static void ReportError(string message, Exception ex = null, string code = null)
        {
            if (ex == null)
                SystemContext.Instance.Reporter?.Error("Tools", message, code);
            else
                SystemContext.Instance.Reporter?.Error("Tools", ex, message, code);
        }

        public static string Guid(int len = 12)
        {
            string res = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, len);
            return res;
        }

        public static string Now()
        {
            string res = DateTime.Now.ToString("yyyyMMddHHmmss");
            return res;
        }
        public static string NowCommon()
        {
            string res = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return res;
        }
        public static decimal NowNum()
        {
            return Convert.ToDecimal(DateTime.Now.ToString("yyyyMMddHHmmss"));
        }
        public static string NowFormat()
        {
            string res = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
            return res;
        }


        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configname"></param>
        /// <returns></returns>
        public static (bool, T) ReadConfig<T>(string configname)
        {
            try
            {
                // 1. 获取基础目录
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string configFolder = Path.Combine(baseDir, "Configuration");
                string filePath = Path.Combine(configFolder, configname);

                T config = default(T);

                // 2. 检查主配置文件是否存在
                if (File.Exists(filePath))
                {
                    // 使用 File.ReadAllText 替代 StreamReader 可以更安全地处理文件流锁定
                    string json = File.ReadAllText(filePath, Encoding.UTF8);
                    config = JsonConvert.DeserializeObject<T>(json);
                }
                else
                {
                    // 3. 尝试读取备份文件逻辑
                    // 使用 Path 库处理文件名，比 Substring 更健壮
                    string fileNameWithoutExt = Path.GetFileNameWithoutExtension(configname);
                    string confignamebak = fileNameWithoutExt + "_bak.json";
                    string filePathBak = Path.Combine(configFolder, confignamebak);

                    if (File.Exists(filePathBak))
                    {
                        string json = File.ReadAllText(filePathBak, Encoding.UTF8);
                        config = JsonConvert.DeserializeObject<T>(json);
                        ReportWarn("主配置不存在，已回退到备份配置：" + configname);
                    }
                    else
                    {
                        // 如果主文件和备份都不存在，返回失败
                        ReportWarn("配置文件不存在：" + configname);
                        return (false, default(T));
                    }
                }

                return (true, config);
            }
            catch (Exception ex)
            {
                ReportError("ReadConfig failed: " + configname, ex);
                return (false, default(T));
            }
        }

        /// <summary>
        /// 保存设置参数配置文件
        /// </summary>
        /// <param name="settingArgsConfig"></param>
        /// <returns></returns>
        public static (bool, string) SaveSettingArgsConfig(SettingArgsConfig settingArgsConfig)
        {
            try
            {
                // 1. 使用 AppDomain 获取根目录，并用 Path.Combine 安全拼接
                string configDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configuration");
                string filePath = Path.Combine(configDir, "settingargsconfig.json");
                string filePathBak = Path.Combine(configDir, "settingargsconfig_bak.json");

                // 2. 确保配置目录存在
                if (!Directory.Exists(configDir))
                {
                    Directory.CreateDirectory(configDir);
                }

                // 3. 备份逻辑优化
                if (File.Exists(filePath))
                {
                    // 如果备份文件已存在，先删除（File.Move 不支持覆盖）
                    if (File.Exists(filePathBak))
                    {
                        File.Delete(filePathBak);
                    }
                    // 移动当前文件到备份
                    File.Move(filePath, filePathBak);
                }

                // 4. 序列化并写入 (使用简化的 File.WriteAllText)
                // Formatting.Indented 对应 .NET Core 的 WriteIndented = true
                string jsonContent = JsonConvert.SerializeObject(settingArgsConfig, Newtonsoft.Json.Formatting.Indented);

                // WriteAllText 会自动创建文件、写入 UTF-8 并在完成后关闭连接
                File.WriteAllText(filePath, jsonContent, Encoding.UTF8);

                ReportInfo("SaveSettingArgsConfig success");
                return (true, "OK");
            }
            catch (Exception ex)
            {
                ReportError("SaveSettingArgsConfig failed", ex);
                return (false, $"Tools SaveSettingArgsConfig ex: {ex.Message}");
            }
        }

        
    }

    


}
