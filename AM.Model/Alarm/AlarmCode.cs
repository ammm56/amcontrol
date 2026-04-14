namespace AM.Model.Alarm
{
    /// <summary>
    /// 系统报警码定义。
    /// 约定分段：
    /// 1000 段：PLC / 通信
    /// 2000 段：Motion / 轴 / IO / 后台采样
    /// 3000 段：视觉
    /// 4000 段：数据库 / 配置
    /// 9000 段：通用未知异常
    /// </summary>
    public enum AlarmCode
    {
        None = 0,

        #region PLC / 通信

        /// <summary>
        /// PLC 连接断开。
        /// </summary>
        PLCDisconnect = 1001,

        /// <summary>
        /// PLC 扫描服务启动失败。
        /// </summary>
        PlcScanWorkerStartFailed = 1002,

        /// <summary>
        /// PLC 扫描服务运行异常。
        /// </summary>
        PlcScanWorkerRuntimeFailed = 1003,

        #endregion

        #region Motion / 轴 / IO / 后台采样

        /// <summary>
        /// 伺服报警。
        /// </summary>
        AxisServoAlarm = 2001,

        /// <summary>
        /// 轴跟随误差超限。
        /// </summary>
        AxisFollowingError = 2002,

        /// <summary>
        /// IO 后台扫描失败，服务已自动停止，需手动重启。
        /// </summary>
        IoScanFailed = 2003,

        /// <summary>
        /// 控制卡驱动初始化失败。
        /// </summary>
        MotionCardInitializeFailed = 2004,

        /// <summary>
        /// 控制卡连接失败。
        /// </summary>
        MotionCardConnectFailed = 2005,

        /// <summary>
        /// 轴运行态采样服务启动失败。
        /// </summary>
        MotionAxisScanWorkerStartFailed = 2006,

        /// <summary>
        /// 轴运行态采样服务运行异常。
        /// </summary>
        MotionAxisScanWorkerRuntimeFailed = 2007,

        /// <summary>
        /// Motion IO 扫描服务启动失败。
        /// </summary>
        MotionIoScanWorkerStartFailed = 2008,

        /// <summary>
        /// Motion IO 扫描服务运行异常。
        /// </summary>
        MotionIoScanWorkerRuntimeFailed = 2009,

        #endregion

        #region 视觉

        CameraTimeout = 3001,

        CameraGrabFailed = 3002,

        #endregion

        #region 数据库 / 配置

        DatabaseError = 4001,

        #endregion

        #region 通用

        Unknown = 9999

        #endregion
    }
}