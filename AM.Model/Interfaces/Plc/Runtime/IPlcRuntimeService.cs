using AM.Model.Common;
using AM.Model.Runtime;

namespace AM.Model.Interfaces.Plc.Runtime
{
    /// <summary>
    /// PLC 运行时服务接口。
    /// 负责运行态启动、停止、状态查询与手动刷新。
    /// </summary>
    public interface IPlcRuntimeService
    {
        /// <summary>
        /// 启动 PLC 扫描服务。
        /// </summary>
        Result Start();

        /// <summary>
        /// 停止 PLC 扫描服务。
        /// </summary>
        Result Stop();

        /// <summary>
        /// 手动执行一轮扫描。
        /// </summary>
        Result ScanOnce();

        /// <summary>
        /// 查询指定 PLC 站运行态。
        /// </summary>
        Result<PlcStationRuntimeSnapshot> QueryStation(string plcName);

        /// <summary>
        /// 查询指定 PLC 点位运行态。
        /// </summary>
        Result<PlcPointRuntimeSnapshot> QueryPoint(string pointName);

        /// <summary>
        /// 查询全部 PLC 站运行态。
        /// </summary>
        Result<PlcStationRuntimeSnapshot> QueryAllStations();

        /// <summary>
        /// 查询全部 PLC 点位运行态。
        /// </summary>
        Result<PlcPointRuntimeSnapshot> QueryAllPoints();
    }
}
