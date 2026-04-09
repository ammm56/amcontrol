using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Core.Messaging
{
    /// <summary>
    /// 线程安全、支持泛型消息、基于弱引用的消息总线实现。
    /// </summary>
    public class MessageBus: IMessageBus
    {
        private readonly object _syncRoot = new object();

        /// <summary>
        /// 按消息类型保存订阅信息。
        /// key: 消息类型
        /// value: 该类型消息的所有订阅
        /// </summary>
        private readonly Dictionary<Type, List<ISubscription>> _subscriptions = new Dictionary<Type, List<ISubscription>>();

        #region IMessageBus

        public void Publish(SystemMessage message)
        {
            Publish<SystemMessage>(message);
        }

        public void Subscribe(object recipient, Action<SystemMessage> handler)
        {
            Subscribe<SystemMessage>(recipient, handler);
        }

        public void Unsubscribe(object recipient)
        {
            if (recipient == null)
            {
                return;
            }

            lock (_syncRoot)
            {
                var messageTypes = _subscriptions.Keys.ToList();
                foreach (var messageType in messageTypes)
                {
                    RemoveRecipientSubscriptions_NoLock(messageType, recipient);
                }

                RemoveEmptyMessageTypes_NoLock();
            }
        }

        #endregion

        #region IGenericMessageBus

        public void Publish<TMessage>(TMessage message) where TMessage : class
        {
            if (message == null)
            {
                return;
            }

            List<ISubscription> snapshot;

            lock (_syncRoot)
            {
                CleanupDeadSubscriptions_NoLock();

                var messageType = typeof(TMessage);
                if (!_subscriptions.TryGetValue(messageType, out var list) || list.Count == 0)
                {
                    return;
                }

                snapshot = list.ToList();
            }

            List<ISubscription> invalidSubscriptions = null;

            foreach (var subscription in snapshot)
            {
                if (!subscription.IsAlive)
                {
                    if (invalidSubscriptions == null)
                    {
                        invalidSubscriptions = new List<ISubscription>();
                    }

                    invalidSubscriptions.Add(subscription);
                    continue;
                }

                try
                {
                    subscription.Invoke(message);
                }
                catch
                {
                    // 这里可以按项目需要接入日志系统。
                    // 为了避免某个订阅者异常影响其他订阅者，吞掉异常继续分发。
                    // 如果希望异常向上抛出，也可以改掉这段逻辑。
                }
            }

            if (invalidSubscriptions != null && invalidSubscriptions.Count > 0)
            {
                lock (_syncRoot)
                {
                    var messageType = typeof(TMessage);
                    if (_subscriptions.TryGetValue(messageType, out var list))
                    {
                        foreach (var invalid in invalidSubscriptions)
                        {
                            list.Remove(invalid);
                        }

                        if (list.Count == 0)
                        {
                            _subscriptions.Remove(messageType);
                        }
                    }
                }
            }
        }

        public void Subscribe<TMessage>(object recipient, Action<TMessage> handler) where TMessage : class
        {
            if (recipient == null)
            {
                throw new ArgumentNullException(nameof(recipient));
            }

            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            var messageType = typeof(TMessage);
            var subscription = new Subscription<TMessage>(recipient, handler);

            lock (_syncRoot)
            {
                CleanupDeadSubscriptions_NoLock();

                if (!_subscriptions.TryGetValue(messageType, out var list))
                {
                    list = new List<ISubscription>();
                    _subscriptions[messageType] = list;
                }

                // 如果希望同一个 recipient 对同一消息类型只能订阅一次，可先移除旧订阅
                list.RemoveAll(s => !s.IsAlive || s.MatchesRecipient(recipient));
                list.Add(subscription);
            }
        }

        public void Unsubscribe<TMessage>(object recipient) where TMessage : class
        {
            if (recipient == null)
            {
                return;
            }

            lock (_syncRoot)
            {
                var messageType = typeof(TMessage);
                RemoveRecipientSubscriptions_NoLock(messageType, recipient);
                RemoveEmptyMessageTypes_NoLock();
            }
        }

        #endregion

        #region Private Helpers

        private void RemoveRecipientSubscriptions_NoLock(Type messageType, object recipient)
        {
            if (!_subscriptions.TryGetValue(messageType, out var list))
            {
                return;
            }

            list.RemoveAll(s => !s.IsAlive || s.MatchesRecipient(recipient));

            if (list.Count == 0)
            {
                _subscriptions.Remove(messageType);
            }
        }

        private void CleanupDeadSubscriptions_NoLock()
        {
            var messageTypes = _subscriptions.Keys.ToList();

            foreach (var messageType in messageTypes)
            {
                var list = _subscriptions[messageType];
                list.RemoveAll(s => !s.IsAlive);

                if (list.Count == 0)
                {
                    _subscriptions.Remove(messageType);
                }
            }
        }

        private void RemoveEmptyMessageTypes_NoLock()
        {
            var emptyTypes = _subscriptions
                .Where(pair => pair.Value == null || pair.Value.Count == 0)
                .Select(pair => pair.Key)
                .ToList();

            foreach (var type in emptyTypes)
            {
                _subscriptions.Remove(type);
            }
        }

        #endregion

        #region Subscription Infrastructure

        private interface ISubscription
        {
            bool IsAlive { get; }

            bool MatchesRecipient(object recipient);

            void Invoke(object message);
        }

        private sealed class Subscription<TMessage> : ISubscription
            where TMessage : class
        {
            private readonly WeakReference _recipientReference;
            private readonly Action<TMessage> _handler;

            public Subscription(object recipient, Action<TMessage> handler)
            {
                _recipientReference = new WeakReference(recipient);
                _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            }

            public bool IsAlive
            {
                get { return _recipientReference.IsAlive && _recipientReference.Target != null; }
            }

            public bool MatchesRecipient(object recipient)
            {
                if (recipient == null)
                {
                    return false;
                }

                var target = _recipientReference.Target;
                if (target == null)
                {
                    return false;
                }

                return ReferenceEquals(target, recipient);
            }

            public void Invoke(object message)
            {
                if (!IsAlive)
                {
                    return;
                }

                _handler((TMessage)message);
            }
        }

        #endregion
    }
}
