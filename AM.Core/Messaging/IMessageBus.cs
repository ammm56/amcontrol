using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Core.Messaging
{
    /// <summary>
    /// 消息通知总线接口
    /// 职责：UI 实时提示、状态栏、消息中心、弹窗、模块间广播
    /// 特点：面向运行时交互、短生命周期、不负责长期状态
    /// </summary>
    public interface IMessageBus
    {
        void Publish(SystemMessage message);

        void Subscribe(object recipient, Action<SystemMessage> handler);
        void Unsubscribe(object recipient);
    }
}
