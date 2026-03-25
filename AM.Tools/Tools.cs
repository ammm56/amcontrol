using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Common;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace AM.Tools
{
    public class Tools
    {
        // 工具类 reporter 调用：成功不上报，警告/错误仅写日志，不推送 UI 消息
        private static void LogWarn(string message, Exception ex = null)
        {
            if (ex == null)
                SystemContext.Instance.Reporter?.Info("Tools", message, null, null, ReportChannels.Log);
            else
                SystemContext.Instance.Reporter?.Error("Tools", ex, message, null, null);
        }

        private static void LogError(string message, Exception ex = null)
        {
            if (ex == null)
                SystemContext.Instance.Reporter?.Error("Tools", message, (string)null);
            else
                SystemContext.Instance.Reporter?.Error("Tools", ex, message, (string)null);
        }

        // ── 时间/ID 工具（保持不变）───────────────────────────────────────

        public static string Guid(int len = 12)
        {
            return System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, len);
        }

        public static string Now() { return DateTime.Now.ToString("yyyyMMddHHmmss"); }
        public static string NowCommon() { return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); }
        public static string NowFormat() { return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"); }
        public static decimal NowNum() { return Convert.ToDecimal(DateTime.Now.ToString("yyyyMMddHHmmss")); }

        // ── 配置读取 ──────────────────────────────────────────────────────

        /// <summary>
        /// 读取配置文件，失败时自动尝试备份文件。
        /// 成功返回 Result&lt;T&gt;，NotifyMode=Silent（配置读取静默，不推送 UI）。
        /// </summary>
        public static Result<T> ReadConfig<T>(string configname)
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string configFolder = Path.Combine(baseDir, "Configuration");
                string filePath = Path.Combine(configFolder, configname);

                if (File.Exists(filePath))
                {
                    var config = JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath, Encoding.UTF8));
                    return Result<T>.OkItem(config).WithNotifyMode(ResultNotifyMode.Silent);
                }

                string bakName = Path.GetFileNameWithoutExtension(configname) + "_bak.json";
                string filePathBak = Path.Combine(configFolder, bakName);

                if (File.Exists(filePathBak))
                {
                    LogWarn("主配置不存在，已回退到备份配置：" + configname);
                    var config = JsonConvert.DeserializeObject<T>(File.ReadAllText(filePathBak, Encoding.UTF8));
                    return Result<T>.OkItem(config).WithNotifyMode(ResultNotifyMode.Silent);
                }

                LogWarn("配置文件不存在：" + configname);
                return Result<T>.Fail(-1, "配置文件不存在：" + configname);
            }
            catch (Exception ex)
            {
                LogError("ReadConfig failed: " + configname, ex);
                return Result<T>.Fail(-1, "读取配置文件失败：" + configname);
            }
        }

        // ── 配置保存 ──────────────────────────────────────────────────────

        /// <summary>
        /// 保存配置文件（含备份）。
        /// </summary>
        public static Result SaveConfig<T>(string configname, T config)
        {
            try
            {
                string configDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configuration");
                string filePath = Path.Combine(configDir, configname);
                string filePathBak = Path.Combine(configDir,
                    Path.GetFileNameWithoutExtension(configname) + "_bak.json");

                if (!Directory.Exists(configDir))
                    Directory.CreateDirectory(configDir);

                if (File.Exists(filePath))
                {
                    if (File.Exists(filePathBak)) File.Delete(filePathBak);
                    File.Move(filePath, filePathBak);
                }

                File.WriteAllText(filePath,
                    JsonConvert.SerializeObject(config, Formatting.Indented), Encoding.UTF8);

                return Result.Ok("SaveConfig OK").WithNotifyMode(ResultNotifyMode.Silent);
            }
            catch (Exception ex)
            {
                LogError("SaveConfig failed: " + configname, ex);
                return Result.Fail(-1, "保存配置文件失败：" + configname);
            }
        }

        /// <summary>
        /// 保存 SettingArgsConfig 配置文件。
        /// </summary>
        public static Result SaveSettingArgsConfig(SettingArgsConfig settingArgsConfig)
        {
            return SaveConfig("settingargsconfig.json", settingArgsConfig);
        }
    }
}
