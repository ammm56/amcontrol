namespace AM.Model.Plc.Enums
{
    /// <summary>
    /// PLC 地址区域类型。
    /// </summary>
    public enum PlcAreaType
    {
        Unknown = 0,
        Coil = 1,
        DiscreteInput = 2,
        InputRegister = 3,
        HoldingRegister = 4,
        X = 10,
        Y = 11,
        M = 12,
        D = 13,
        W = 14,
        R = 15,
        T = 16,
        C = 17,
        DB = 18,
        V = 19,
        Custom = 99
    }
}