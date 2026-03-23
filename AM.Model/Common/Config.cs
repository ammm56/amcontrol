using AM.Model.Model;
using AM.Model.MotionCard;
using AM.Model.MotionCard.Actuator;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AM.Model.Common
{
    /// <summary>
    /// 配置
    /// </summary>
    public partial class Config
    {
        public Setting Setting { get; set; } = new Setting();

        public DB Sqlite { get; set; } = new DB();

        /// <summary>
        /// 运动控制完整配置。
        /// 运行时由数据库装载，不再写入 config.json。
        /// </summary>
        [JsonIgnore]
        public List<MotionCardConfig> MotionCardsConfig { get; set; } = new List<MotionCardConfig>();

        /// <summary>
        /// IO 扫描服务配置。
        /// 持久化到 config.json。
        /// </summary>
        public IoScanConfig IoScanConfig { get; set; } = new IoScanConfig();

        /// <summary>
        /// 第三层对象运行时配置聚合。
        /// 运行时由数据库装载，不再写入 config.json。
        /// </summary>
        [JsonIgnore]
        public ActuatorConfig ActuatorConfig { get; set; } = new ActuatorConfig();

        /// <summary>
        /// 运行时配置
        /// 序列化时不保存
        /// [JsonIgnore] 在 Newtonsoft 中依然通用
        /// </summary>
        [JsonIgnore]
        public RuntimeConfig RuntimeConfig { get; set; } = new RuntimeConfig();
    }

    /// <summary>
    /// 一般设置
    /// </summary>
    public class Setting
    {
        /// <summary>
        /// 当前语言
        /// </summary>
        public string Language { get; set; } = "zh-CN";

        /// <summary>
        /// 当前主题
        /// HandyControl 皮肤资源名：SkinDefault / SkinDark
        /// </summary>
        public string Theme { get; set; } = "SkinDefault";

        /// <summary>
        /// 是否启用控制台打印
        /// 控制台所有的打印输出
        /// </summary>
        public bool ConsolePrint { get; set; } = false;
        /// <summary>
        /// 是否启用普通信息打印
        /// </summary>
        public bool CommonInfoPrint { get; set; } = false;
        /// <summary>
        /// 是否启用调试信息打印
        /// </summary>
        public bool DebugInfoPrint { get; set; } = false;
        /// <summary>
        /// 是否启用控制器信息打印
        /// </summary>
        public bool ControllerInfoPrint { get; set; } = false;
        /// <summary>
        /// 数据库操作信息打印
        /// </summary>
        public bool DBPrint { get; set; } = false;
        /// <summary>
        /// 异常信息打印
        /// </summary>
        public bool EXPrint { get; set; } = false;
        /// <summary>
        /// 是否启用数据库执行命令记录显示
        /// </summary>
        public bool DBLogExec { get; set; } = false;
    }

    /// <summary>
    /// 数据库连接
    /// </summary>
    public class DB
    {
        public string Connection { get; set; } = "Data Source=am.db;Version=3;";

        public bool Enabled { get; set; } = false;
    }

    /// <summary>
    /// IO 扫描服务启动配置。
    /// 持久化到 config.json，控制扫描行为。
    /// </summary>
    public class IoScanConfig
    {
        /// <summary>
        /// 应用启动后是否自动开始 IO 扫描。
        /// 默认 false：需在设备就绪后手动启动。
        /// 调试/生产环境可设为 true 实现开机自动扫描。
        /// </summary>
        public bool AutoStart { get; set; } = false;

        /// <summary>
        /// 扫描周期，单位 ms。
        /// 最小值为 10ms，默认 50ms。
        /// </summary>
        public int ScanIntervalMs { get; set; } = 50;
    }

    /// <summary>
    /// 运行时配置
    /// 序列化时不保存
    /// </summary>
    public class RuntimeConfig
    {
        /// <summary>
        /// Newtonsoft.Json 的序列化配置类是 JsonSerializerSettings
        /// </summary>
        public JsonSerializerSettings CoreSerialOption { get; set; } = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
        };

        /// <summary>
        /// 系统信息
        /// </summary>
        public MOSInfo OSInfo { get; set; } = new MOSInfo();
    }
}