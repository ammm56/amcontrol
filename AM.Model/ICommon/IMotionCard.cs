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
        void SetAxisParam(short axis, AxisParam param);

        // --- 连接与生命周期 ---
        bool Initialize(string configPath);
        short Connect(short cardId);
        short Disconnect(); // 通常内部记录 cardId

        // --- 轴基础控制 ---
        short Enable(short axis, bool onORoff);
        short Stop(short axis, bool emergency = false); // 增加急停选项
        Task<short> HomeAsync(short axis); // 异步回零
        short Home(short axis);

        // --- 运动指令 ---
        short MoveRelative(short axis, double distance, double velocity, double acc, double dec);
        short MoveAbsolute(short axis, double position, double velocity, double acc, double dec);
        /// <summary>
        /// Jog运动：direction 为 1(正) / -1(负) / 0(停止)
        /// </summary>
        short JogMove(short axis, int direction, double velocity);
        // 业务层使用 mm 单位的方法
        short MoveRelativeMm(short axis, double distanceMm, double velMm);
        short MoveAbsoluteMm(short axis, double positionMm, double velMm);

        // --- IO 操作 ---
        short SetDO(short bit, bool status);
        bool GetDI(short bit);
        bool GetDO(short bit);

        // --- 设置参数 ---
        short SetVel(short axis, double vel);
        short SetAcc(short axis, double acc);
        short SetDec(short axis, double dec);

        // --- 状态获取 ---
        /// <summary>
        /// 获取轴综合状态（报错、限位、到位等）
        /// </summary>
        AxisStatus GetAxisStatus(short axis);

        double GetCommandPosition(short axis); // 指令位置
        double GetEncoderPosition(short axis); // 编码器反馈位置
        double GetPositionMm(short axis); // 获取规划位置（mm）
        bool IsMoving(short axis);


    }
}
