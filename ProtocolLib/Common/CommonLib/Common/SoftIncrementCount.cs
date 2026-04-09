using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolLib.CommonLib.Common
{
    /// <summary>
    /// 不持久化序号自增类
    /// </summary>
    public sealed class SoftIncrementCount:IDisposable
    {
        public SoftIncrementCount(long max,long start = 0,int tick = 1)
        {
            this.start = start;
            this.current = start;
            this.max = max;

            this.simpleLock = new SimpleLock();
        }

        private long start = 0;
        private long current = 0;
        private long max = long.MaxValue;
        private SimpleLock simpleLock;
        /// <summary>
        /// 增加的单元
        /// 默认增1
        /// </summary>
        public int IncreaseTick { get; set; } = 1;

        /// <summary>
        /// 获取自增信息
        /// </summary>
        /// <returns></returns>
        public long GetCurrentValue()
        {
            long value = 0;
            simpleLock.Enter();

            value = current;
            current += IncreaseTick;
            if(current > max)
            {
                current = start;
            }
            else if(current < start)
            {
                current = max;
            }

            simpleLock.Leave();
            return value;
        }

        /// <summary>
        /// 重置当前序号的最大值
        /// </summary>
        /// <param name="max"></param>
        public void ResetMaxValue(long max)
        {
            simpleLock.Enter();

            if (max > start)
            {
                if (max < current) current = start;
                this.max = max;
            }

            simpleLock.Leave();
        }

        /// <summary>
        /// 重置当前序号的初始值
        /// </summary>
        /// <param name="start"></param>
        public void ResetStartValue(long start)
        {
            simpleLock.Enter();

            if(start < max)
            {
                if (current < start) current = start;
                this.start = start;
            }

            simpleLock.Leave();
        }

        /// <summary>
        /// 当前值重置为初始值
        /// </summary>
        public void ResetCurrentValue()
        {
            simpleLock.Enter();

            current = start;

            simpleLock.Leave();
        }

        /// <summary>
        /// 当前值重置为指定值
        /// </summary>
        /// <param name="value"></param>
        public void ResetCurrentValue(long value)
        {
            simpleLock.Enter();

            if(value > max)
            {
                current = max;
            }
            else if(value < start)
            {
                current = start;
            }
            else
            {
                current = value;
            }

            simpleLock.Leave();
        }



        public override string ToString()
        {
            return $"SoftIncrementCount current={current} max={max}";
        }

        /// <summary>
        /// 检测冗余调用
        /// </summary>
        private bool disposedValue = false;
        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    simpleLock.Dispose();
                }
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }

    }
}
