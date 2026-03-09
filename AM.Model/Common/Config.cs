using AM.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AM.Model.MotionCard;

namespace AM.Model.Common
{
    /// <summary>
    /// 配置
    /// </summary>
    public class Config:ObservableObject
    {

        public Setting Setting { get; set; } = new Setting();

        
        private DB _sqlite;
        /// <summary>
        /// 传统语言层面写法
        /// </summary>
        //public DB Sqlite
        //{
        //    get => _sqlite;
        //    set { _sqlite = value; RaisePropertyChanged(); }
        //}
        /// <summary>
        /// // 一行写法：自动判断值是否变化、自动赋值、自动发通知
        /// </summary>
        public DB Sqlite { get => _sqlite; set => SetProperty(ref _sqlite, value); }

        public MotionCardConfig MotionCardConfig { get; set; } = new MotionCardConfig();

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
    public class DB:ObservableObject
    {
        private string _connection;
        public string Connection { get => _connection;set=>SetProperty(ref _connection, value);  }

        private bool _enabled;
        public bool Enabled { get => _enabled; set => SetProperty(ref _enabled, value); }
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
            // 忽略大小写（Newtonsoft 默认就是不区分大小写的）
            // 如果需要显式设置：
            // MetadataPropertyHandling = MetadataPropertyHandling.Default,

            // 忽略空值
            NullValueHandling = NullValueHandling.Ignore,

            // 对应 IgnoreReadOnlyProperties
            // ContractResolver = new DefaultContractResolver { IgnoreSerializableAttribute = true }
        };

        /// <summary>
        /// 系统信息
        /// </summary>
        public MOSInfo OSInfo { get; set; } = new MOSInfo();
    }
}
