using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
//using Newtonsoft.Json;
using System.Threading;

namespace ProtocolLib.CommonLib.Common
{
    /// <summary>
    /// 操作超时检查
    /// </summary>
    public class TimeOutCheck
    {
        static TimeOutCheck()
        {
            CreateTimeoutCheckThread();
        }
        public TimeOutCheck()
        {
            UniqueID     = Interlocked.Increment(ref TimeoutID);
            StartTime    = DateTime.Now;
            IsSuccessful = false;
            IsTimeout    = false;
        }

        /// <summary>
        /// 当前超时对象的唯一ID信息，每实例化一个TimeOutCheck对象，ID信息会增1
        /// </summary>
        public long UniqueID { get; set; }
        /// <summary>
        /// 操作开始的时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 操作是否成功
        /// 当操作完成时，设置为True，超时检测自动结束
        /// 如果一直为False,超时检测到超时，设置为True
        /// </summary>
        public bool IsSuccessful { get; set; }
        /// <summary>
        /// 延时时间，单位毫秒
        /// </summary>
        public int DelayTime { get; set; }
        /// <summary>
        /// 连接超时用的Socket
        /// 协议的socket
        /// </summary>
        //[JsonIgnore]
        public Socket WorkSocket { get; set; }
        /// <summary>
        /// 是否发生了超时的操作
        /// 调用方法异常结束时，对这个判断，是否因为发送了超时导致的异常
        /// </summary>
        public bool IsTimeout { get; set; }


        #region 超时检测部分
        private static long TimeoutID = 0;
        private static List<TimeOutCheck> WaitHandleTimeOutChecks = new List<TimeOutCheck>();
        private static object listLock = new object();
        /// <summary>
        /// 超时检测线程
        /// </summary>
        private static Thread threadCheckTimeOut;
        /// <summary>
        /// 线程唯一ID
        /// </summary>
        private static long threadUniqueID = 0;
        /// <summary>
        /// 线程的活跃时间
        /// </summary>
        private static DateTime threadActiveTime;
        /// <summary>
        /// 每不活跃60秒就增加一次计数，超过两次重启线程
        /// </summary>
        private static int activeDisableCount = 0;

        /// <summary>
        /// 获取到目前为止用的时间
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetConsumeTime()=>DateTime.Now - StartTime;

        /// <summary>
        /// 新增一个超时检测对象
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static TimeOutCheck HandleTimeOutCheck(Socket socket,int timeout)
        {
            TimeOutCheck timeOutCheck = new TimeOutCheck()
            {
                DelayTime = timeout,
                IsSuccessful = false,
                StartTime = DateTime.Now,
                WorkSocket = socket
            };
            if (timeout > 0) TimeOutCheckHandle(timeOutCheck);
            return timeOutCheck;
        }
        /// <summary>
        /// 新增一个超时检测对象
        /// 当操作完成时，手动标记对象的IsSuccessful为True
        /// </summary>
        /// <param name="timeOutCheck"></param>
        public static void TimeOutCheckHandle(TimeOutCheck timeOutCheck)
        {
            lock (listLock)
            {
                //检测线程是否活跃，超过60秒不活跃，尝试重启线程
                if((DateTime.Now - threadActiveTime).TotalSeconds > 60)
                {
                    threadActiveTime = DateTime.Now;
                    if(Interlocked.Increment(ref activeDisableCount) >= 2)
                    {
                        CreateTimeoutCheckThread();
                    }
                }
                WaitHandleTimeOutChecks.Add(timeOutCheck);
            }
        }
        /// <summary>
        /// 创建超时检测线程
        /// </summary>
        public static void CreateTimeoutCheckThread()
        {
            threadActiveTime = DateTime.Now;
            threadCheckTimeOut?.Abort();
            threadCheckTimeOut = new Thread(new ParameterizedThreadStart(CheckTimeOut));
            threadCheckTimeOut.IsBackground = true;
            threadCheckTimeOut.Priority = ThreadPriority.AboveNormal;
            threadCheckTimeOut.Start(Interlocked.Increment(ref threadUniqueID));
        }
        /// <summary>
        /// 超时检测方法
        /// 由一个单独线程运行，线程优先级高，超时信息可放这里处理
        /// </summary>
        /// <param name="obj"></param>
        private static void CheckTimeOut(object obj)
        {
            long threadID = (long)obj;
            while (true)
            {
                Thread.Sleep(100);
                if (threadID != threadUniqueID) break;
                threadActiveTime = DateTime.Now;
                activeDisableCount = 0;
                lock (listLock)
                {
                    for(int i = WaitHandleTimeOutChecks.Count - 1; i >= 0; i--)
                    {
                        TimeOutCheck timeOutCheck = WaitHandleTimeOutChecks[i];
                        if (timeOutCheck.IsSuccessful)
                        {
                            WaitHandleTimeOutChecks.RemoveAt(i);
                            continue;
                        }
                        if((DateTime.Now - timeOutCheck.StartTime).TotalMilliseconds > timeOutCheck.DelayTime)
                        {
                            //连接超时或验证超时
                            if (!timeOutCheck.IsSuccessful)
                            {
                                timeOutCheck.WorkSocket?.Close();
                                timeOutCheck.IsTimeout = true;
                            }
                            WaitHandleTimeOutChecks.RemoveAt(i);
                            continue;
                        }
                    }
                }
            }
        }

        #endregion

        #region 外部获取超时检测相关信息
        //特性AttributeUsage
        //获取检测超时对象个数

        //特性AttributeUsage
        //获取等待检测对象列表

        #endregion

    }
}
