using AM.Model.Common;
using AM.Model.MotionCard;
using AM.Model.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Model.Interfaces.MotionCard
{
    /// <summary>
    /// 控制卡聚合接口（组合各职责子接口）
    /// </summary>
    public interface IMotionCardService :
        IMotionCardDiagnostics,
        IMotionCardConfiguration,
        IMotionCardConnection,
        IMotionAxisMaintenance,
        IMotionAxisControl,
        IMotionAxisMotion,
        IMotionDigitalIO,
        IMotionAxisParameter,
        IMotionAxisState
    {
    }

    /// <summary>
    /// 诊断与报警事件
    /// </summary>
    public interface IMotionCardDiagnostics
    {
        /// <summary>
        /// 事件通知：卡号, 错误信息
        /// 改为到IMessageBus中统一实现
        /// </summary>
        //event Action<short, string> OnError;

    }

    /// <summary>
    /// 运行前配置加载
    /// </summary>
    public interface IMotionCardConfiguration
    {
        void LoadAxisConfig(List<AxisConfig> configs);
    }

    /// <summary>
    /// 控制卡连接与生命周期
    /// </summary>
    public interface IMotionCardConnection
    {
        Result Initialize(string configPath);
        Result Connect();
        Result Disconnect();
    }

    /// <summary>
    /// 轴维护接口（清状态/清零/硬件配置）
    /// </summary>
    public interface IMotionAxisMaintenance
    {
        Result ClearStatus(short logicalAxis);
        Result ClearAllAxisStatus();
        Result SetZeroPos(short logicalAxis);
        Result SetAllZeroPos();
        Result ConfigAxisHardware(AxisConfig cfg);
    }

    /// <summary>
    /// 轴基础控制
    /// </summary>
    public interface IMotionAxisControl
    {
        Result Enable(short logicalAxis, bool onOff);
        Result Stop(short logicalAxis, bool isEmergency = false);
        Result StopAll(bool isEmergency = false);
        Task<Result> HomeAsync(short logicalAxis);
        Result Home(short logicalAxis);
    }

    /// <summary>
    /// 轴运动控制
    /// </summary>
    public interface IMotionAxisMotion
    {
        Result MoveRelative(short logicalAxis, double distance, double velocity, double acc, double dec);
        Result MoveAbsolute(short logicalAxis, double position, double velocity, double acc, double dec);
        Result JogMove(short logicalAxis, int direction, double velocity);

        Result MoveRelativeMm(short logicalAxis, double distanceMm, double velMm);
        Result MoveAbsoluteMm(short logicalAxis, double positionMm, double velMm);
        Result JogMoveMm(short logicalAxis, bool direction, double velMm);
        Result JogStop(short logicalAxis);
    }

    /// <summary>
    /// 数字IO操作
    /// </summary>
    public interface IMotionDigitalIO
    {
        Result SetDO(short bit, bool status);
        Result<bool> GetDI(short bit);
        Result<bool> GetDO(short bit);
    }

    /// <summary>
    /// 轴参数设置
    /// </summary>
    public interface IMotionAxisParameter
    {
        Result SetVel(short logicalAxis, double vel);
        Result SetAcc(short logicalAxis, double acc);
        Result SetDec(short logicalAxis, double dec);
    }

    /// <summary>
    /// 轴状态读取
    /// </summary>
    public interface IMotionAxisState
    {
        Result<AxisStatus> GetAxisStatus(short logicalAxis);

        // 脉冲位置
        Result<double> GetCommandPosition(short logicalAxis);
        Result<double> GetEncoderPosition(short logicalAxis);

        // 毫米位置（与脉冲方法一一对应）
        Result<double> GetCommandPositionMm(short logicalAxis);
        Result<double> GetEncoderPositionMm(short logicalAxis);

        Result<bool> IsMoving(short logicalAxis);
    }
}
