using AM.Model.Common;

namespace AM.Model.Interfaces.Motion.Assembly
{
    /// <summary>
    /// 装配接线工作台查询服务接口。
    /// 负责聚合软件 IO 定义、接线信息与运行时状态。
    /// </summary>
    public interface IAssemblyWiringQueryService
    {
        /// <summary>
        /// 查询全部接线工作台行数据。
        /// </summary>
        Result<AssemblyWiringRowModel> QueryAll();

        /// <summary>
        /// 按控制卡查询接线工作台行数据。
        /// </summary>
        /// <param name="cardId">控制卡硬件通道号。</param>
        Result<AssemblyWiringRowModel> QueryByCardId(short cardId);

        /// <summary>
        /// 按逻辑位与 IO 类型查询接线工作台行数据。
        /// </summary>
        /// <param name="logicalBit">逻辑位号。</param>
        /// <param name="ioType">IO 类型，仅支持 DI 或 DO。</param>
        Result<AssemblyWiringRowModel> QueryByLogicalBit(short logicalBit, string ioType);
    }

    /// <summary>
    /// 装配接线工作台表格行模型。
    /// 用于在单页中同时承载软件定义、接线定义与运行时状态。
    /// </summary>
    public class AssemblyWiringRowModel
    {
        public int IoMapId { get; set; }

        public string IoType { get; set; }

        public short LogicalBit { get; set; }

        public short CardId { get; set; }

        public string CardDisplayName { get; set; }

        public string SoftwareName { get; set; }

        public string DisplayName { get; set; }

        public string SignalCategory { get; set; }

        public string Description { get; set; }

        public short Core { get; set; }

        public bool IsExtModule { get; set; }

        public short HardwareBit { get; set; }

        public bool IsEnabled { get; set; }

        public string TerminalBlock { get; set; }

        public string TerminalNo { get; set; }

        public string ConnectorNo { get; set; }

        public string PinNo { get; set; }

        public string WireNo { get; set; }

        public string DeviceName { get; set; }

        public string DeviceModel { get; set; }

        public string DeviceTerminal { get; set; }

        public string CabinetArea { get; set; }

        public string SignalType { get; set; }

        public string ExpectedNormalState { get; set; }

        public string CheckMethod { get; set; }

        public string WiringRemark { get; set; }

        public bool IsVerified { get; set; }

        public string VerifiedBy { get; set; }

        public string CurrentValueText { get; set; }

        public string LastUpdateTimeText { get; set; }

        public string RuntimeStatusText { get; set; }

        public string WiringStatusText { get; set; }

        public bool CanManualOperate { get; set; }
    }
}
