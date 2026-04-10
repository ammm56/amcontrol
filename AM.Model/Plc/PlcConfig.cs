using System.Collections.Generic;

namespace AM.Model.Plc
{
    /// <summary>
    /// PLC 运行时配置聚合。
    /// 由数据库装配后挂入 ConfigContext。
    /// </summary>
    public class PlcConfig
    {
        /// <summary>
        /// PLC 站配置集合。
        /// </summary>
        public List<PlcStationConfig> Stations { get; set; } = new List<PlcStationConfig>();

        /// <summary>
        /// PLC 点位配置集合。
        /// </summary>
        public List<PlcPointConfig> Points { get; set; } = new List<PlcPointConfig>();
    }
}