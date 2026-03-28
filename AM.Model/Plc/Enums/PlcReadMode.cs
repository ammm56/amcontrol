namespace AM.Model.Plc.Enums
{
    /// <summary>
    /// PLC 读取模式。
    /// </summary>
    public enum PlcReadMode
    {
        Unknown = 0,
        Single = 1,
        BatchByAddress = 2,
        BatchByDataType = 3,
        BatchByWordLength = 4,
        BatchByByteLength = 5
    }
}