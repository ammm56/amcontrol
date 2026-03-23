namespace AM.Model.Alarm
{
    public enum AlarmCode
    {
        None = 0,

        PLCDisconnect = 1001,

        AxisServoAlarm = 2001,

        AxisFollowingError = 2002,

        /// <summary>
        /// IO 后台扫描失败，服务已自动停止，需手动重启。
        /// </summary>
        IoScanFailed = 2003,

        CameraTimeout = 3001,

        CameraGrabFailed = 3002,

        DatabaseError = 4001,

        Unknown = 9999
    }
}