using AM.Model.Model;
using AM.Model.MotionCard;
using AM.Model.MotionCard.Actuator;
using AM.Model.Plc;
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
        /// PLC 运行时配置聚合。
        /// 运行时由数据库装载，不再写入 config.json。
        /// </summary>
        [JsonIgnore]
        public PlcConfig PlcConfig { get; set; } = new PlcConfig();

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

        /// <summary>
        /// 是否启用使用信息上报。
        /// 默认 true。
        /// </summary>
        public bool EnableUsageReport { get; set; } = true;

        /// <summary>
        /// 统一后端服务根地址。
        /// 授权申请、设备注册、心跳、结构化上报和使用信息上报统一走此地址。
        /// </summary>
        public string BackendServiceUrl { get; set; } = "http://localhost:5095";

        /// <summary>
        /// 使用信息上传周期，单位 ms。
        /// 默认 60000。
        /// </summary>
        public int UsageUploadIntervalMs { get; set; } = 60000;

        /// <summary>
        /// 使用信息单批上传数量。
        /// 默认 100。
        /// </summary>
        public int UsageUploadBatchSize { get; set; } = 100;

        /// <summary>
        /// 客户端唯一标识。
        /// 首次运行后由客户端身份服务生成并回写。
        /// </summary>
        public string ClientId { get; set; } = string.Empty;

        /// <summary>
        /// 设备编码。
        /// 仅用于软件实例识别，不包含生产数据。
        /// </summary>
        public string MachineCode { get; set; } = string.Empty;

        /// <summary>
        /// 设备名称。
        /// 默认可为空，运行时可回退到 Environment.MachineName。
        /// </summary>
        public string MachineName { get; set; } = string.Empty;

        /// <summary>
        /// 是否启用设备侧 License 运行时校验。
        /// 默认 true。
        /// </summary>
        public bool EnableDeviceLicense { get; set; } = true;

        /// <summary>
        /// 设备心跳周期，单位 ms。
        /// 默认 60000。
        /// </summary>
        public int DeviceHeartbeatIntervalMs { get; set; } = 60000;

        /// <summary>
        /// 设备平台注册标识。
        /// 注册成功后由设备注册客户端回写并持久化。
        /// </summary>
        public string DeviceId { get; set; } = string.Empty;

        /// <summary>
        /// 最近一次注册或刷新后保存的设备 token。
        /// 当前阶段先持久化到 config.json，后续如有需要再调整存储位置。
        /// </summary>
        public string DeviceToken { get; set; } = string.Empty;
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