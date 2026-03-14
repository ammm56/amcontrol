using System;

namespace AM.Core.Messaging
{
    /// <summary>
    /// 消息通知总线接口。
    /// 用于模块间发布与订阅系统消息。
    /// </summary>
    public interface IMessageBus
    {
        /// <summary>
        /// 发布一条系统消息。
        /// </summary>
        /// <param name="message">系统消息对象。</param>
        void Publish(SystemMessage message);

        /// <summary>
        /// 订阅系统消息。
        /// </summary>
        /// <param name="recipient">订阅者标识对象。</param>
        /// <param name="handler">消息处理方法。</param>
        void Subscribe(object recipient, Action<SystemMessage> handler);

        /// <summary>
        /// 取消指定订阅者的消息订阅。
        /// </summary>
        /// <param name="recipient">订阅者标识对象。</param>
        void Unsubscribe(object recipient);
    }
}
