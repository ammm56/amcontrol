namespace AM.Model.Plc.Enums
{
    /// <summary>
    /// PLC 通讯协议类型。
    /// </summary>
    public enum PlcProtocolType
    {
        Unknown = 0,
        ModbusTcp = 1,
        ModbusRtu = 2,
        S7 = 3,
        Mc3E = 4,
        Mc1E = 5,
        FinsTcp = 6,
        FinsUdp = 7,
        HostLink = 8,
        Custom = 99
    }
}