using AM.Model.MotionCard;
using System;

namespace AM.Model.Interfaces.MotionCard
{
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

        MotionResult LastResult { get; }

    }
}