namespace AM.Model.Plc.Enums
{
    /// <summary>
    /// PLC 点位质量状态。
    /// </summary>
    public enum PlcPointQuality
    {
        Unknown = 0,
        Good = 1,
        Stale = 2,
        Disconnected = 3,
        Error = 4
    }
}