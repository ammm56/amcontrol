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

        // 单位换算
        InvalidK = -1101,
        PulseOverflow = -1102,
        InvalidConvertInput = -1103
    }
}