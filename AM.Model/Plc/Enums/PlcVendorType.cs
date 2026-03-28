namespace AM.Model.Plc.Enums
{
    /// <summary>
    /// PLC 厂商类型。
    /// </summary>
    public enum PlcVendorType
    {
        Unknown = 0,
        Siemens = 1,
        Mitsubishi = 2,
        Omron = 3,
        Keyence = 4,
        Panasonic = 5,
        Delta = 6,
        ModbusGeneric = 7,
        Custom = 99
    }
}