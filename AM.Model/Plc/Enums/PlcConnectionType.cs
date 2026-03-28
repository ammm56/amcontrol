namespace AM.Model.Plc.Enums
{
    /// <summary>
    /// PLC 连接方式。
    /// </summary>
    public enum PlcConnectionType
    {
        Unknown = 0,
        Tcp = 1,
        Serial = 2,
        Usb = 3,
        Virtual = 4
    }
}