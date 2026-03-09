using AM.Model.MotionCard;
using AM.Model.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Model.ICommon
{
    /// <summary>
    /// 控制卡标准接口
    /// 所有控制卡的共性操作：初始化、点动（Jog）、绝对/相对运动、停止、状态读取。
    /// </summary>
    public interface IMotionCard
    {
        // 事件通知：卡号, 错误信息
        event Action<short, string> OnError;
        // 配置轴参数（用于单位转换）
        void SetAxisParam(short logicalAxis, AxisParam param);
        // 把配置给具体的卡实例
        void LoadAxisConfig(List<AxisConfig> configs);

        // --- 连接与生命周期 ---
        bool Initialize(string configPath);
        short Connect();
        short Disconnect();

        // --- 清除状态 ---
        short ClearStatus(short logicalAxis);
        short ClearAllAxisStatus();
        /// <summary>
        /// 将指定逻辑轴的位置清零
        /// </summary>
        short SetZeroPos(short logicalAxis);
        /// <summary>
        /// 将全卡所有轴的位置清零
        /// </summary>
        short SetAllZeroPos();

        // --- 手动配置轴硬件、IO ---
        short ConfigAxisHardware(AxisConfig cfg);

        // --- 轴基础控制 ---
        short Enable(short logicalAxis, bool onORoff);
        /// <summary>
        /// 停止指定逻辑轴
        /// 急停：直接物理抱死或停止脉冲输出，通常用于安全光幕触发或紧急红色按钮。
        /// 平滑停止：遵循在 AxisConfig 中设置的 Dec（减速度）。
        /// </summary>
        /// <param name="logicalAxis">逻辑轴号</param>
        /// <param name="isEmergency">是否急停(true:立即急停, false:立刻平停)</param>
        short Stop(short logicalAxis, bool isEmergency = false);
        /// <summary>
        /// 停止控制卡上的所有轴
        /// </summary>
        short StopAll(bool isEmergency = false);
        Task<short> HomeAsync(short logicalAxis); // 异步回零
        short Home(short logicalAxis);

        // --- 运动指令 ---
        short MoveRelative(short logicalAxis, double distance, double velocity, double acc, double dec);
        short MoveAbsolute(short logicalAxis, double position, double velocity, double acc, double dec);
        /// <summary>
        /// Jog运动：direction 为 1(正) / -1(负) / 0(停止)
        /// </summary>
        short JogMove(short logicalAxis, int direction, double velocity);
        // 业务层使用 mm 单位的方法
        short MoveRelativeMm(short logicalAxis, double distanceMm, double velMm);
        short MoveAbsoluteMm(short logicalAxis, double positionMm, double velMm);

        // --- IO 操作 ---
        short SetDO(short bit, bool status);
        bool GetDI(short bit);
        bool GetDO(short bit);

        // --- 设置参数 ---
        short SetVel(short logicalAxis, double vel);
        short SetAcc(short logicalAxis, double acc);
        short SetDec(short logicalAxis, double dec);

        // --- 状态获取 ---
        /// <summary>
        /// 获取轴综合状态（报错、限位、到位等）
        /// </summary>
        AxisStatus GetAxisStatus(short logicalAxis);

        double GetCommandPosition(short logicalAxis); // 指令位置
        double GetEncoderPosition(short logicalAxis); // 编码器反馈位置
        double GetPositionMm(short logicalAxis); // 获取规划位置（mm）
        bool IsMoving(short logicalAxis);


    }
}
