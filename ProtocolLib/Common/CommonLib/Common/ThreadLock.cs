using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ProtocolLib.CommonLib.Common
{
    /// <summary>
    /// 基元用户基元内核同步 线程同步锁
    /// </summary>
    public sealed class SimpleLock:IDisposable
    {
        /// <summary>
        /// 当前总的锁的进入次数
        /// </summary>
        private static long simpleLockCount = 0;
        /// <summary>
        /// 当前锁的等待的次数
        /// 此时已经开始竞争
        /// </summary>
        private static long simpleLockWaitCount = 0;

        /// <summary>
        /// 基元用户模式构造同步锁
        /// </summary>
        private int m_waiters = 0;
        /// <summary>
        /// 检测冗余调用
        /// </summary>
        private bool disposeValue = false;

        void Dispose(bool disposing)
        {
            if (!disposeValue)
            {
                if (disposing)
                {
                    //释放托管状态
                }
            }
            m_waiterLock.Value.Close();
            disposeValue = true;
        }
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// 基元内核模式构造同步锁
        /// </summary>
        private readonly Lazy<AutoResetEvent> m_waiterLock = new Lazy<AutoResetEvent>(() =>
        {
            return new AutoResetEvent(false);
        });

        /// <summary>
        /// 获取锁
        /// </summary>
        public void Enter()
        {
            Interlocked.Increment(ref simpleLockCount);
            //用户锁可以使用的时候，直接返回，第一次调用时发生
            if (Interlocked.Increment(ref m_waiters) == 1) return;
            //当发生锁竞争时，使用内核同步构造锁
            Interlocked.Increment(ref simpleLockWaitCount);

            m_waiterLock.Value.WaitOne();
        }

        /// <summary>
        /// 离开锁
        /// </summary>
        public void Leave()
        {
            Interlocked.Decrement(ref simpleLockCount);
            //没有可用的锁的时候
            if(Interlocked.Decrement(ref m_waiters) == 0) return;

            Interlocked.Decrement(ref simpleLockWaitCount);
            m_waiterLock.Value.Set();
        }

        /// <summary>
        /// 当前锁是否在等待中
        /// </summary>
        public bool IsWaitting => m_waiters != 0;
        /// <summary>
        /// 获取当前总的所有进入锁的数量
        /// </summary>
        public static long SimpleLockCount => simpleLockCount;
        /// <summary>
        /// 获取正在等待的锁的数量
        /// </summary>
        public static long SimpleLockWaitCount => simpleLockWaitCount;
    }
}
