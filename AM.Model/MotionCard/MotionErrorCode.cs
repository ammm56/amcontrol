namespace AM.Model.MotionCard
{
    /// <summary>
    /// 运动控制统一错误码
    /// 约定：0 成功；负数失败
    /// </summary>
    public enum MotionErrorCode : short
    {
        Success = 0,

        // 通用
        Unknown = -1999,
        NotImplemented = -1998,

        // 配置/映射
        AxisMapNotFound = -1001,
        InvalidAxisConfig = -1002,
        InvalidIoBit = -1003,
        IoMapNotFound = -1004,
        HomeConfigInvalid = -1005,
        HomeFailed = -1006,
        HomeTimeout = -1007,

        /// <summary>
        /// Motion IO 运行时缓存已过期/不可用。
        /// 说明：
        /// 1. 不代表 IO 映射不存在；
        /// 2. 表示后台扫描缓存未及时更新、扫描停止或扫描停滞。
        /// </summary>
        IoRuntimeCacheStale = -1008,

        // 单位换算
        InvalidK = -1101,
        PulseOverflow = -1102,
        InvalidConvertInput = -1103,


        CardConnectFailed = -1201
    }
}