using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolLib.CommonLib.Model
{
    /// <summary>
    /// 操作返回信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class M_Return<T>
    {
        public M_Return() { }
        public M_Return(bool status,T result,string descmsg)
        {
            Status = status;
            Result = result;
            DescMsg = descmsg;  
        }

        /// <summary>
        /// 操作成功或失败
        /// </summary>
        public bool Status { get; set; } = false;
        /// <summary>
        /// 操作的返回结果
        /// </summary>
        public T Result { get; set; }

        /// <summary>
        /// 返回的描述消息
        /// </summary>
        public string DescMsg { get; set; } = "";

        public static M_Return<T> OK()
        {
            return new M_Return<T>
            {
                Status = true,
                Result = default(T),
                DescMsg = "ok"
            };
        }
        public static M_Return<T> OK(T result)
        {
            return new M_Return<T>
            {
                Status = true,
                Result = result,
                DescMsg = "ok"
            };
        }
        public static M_Return<T> OK(string msg)
        {
            return new M_Return<T>
            {
                Status = true,
                Result = default(T),
                DescMsg = msg
            };
        }
        public static M_Return<T> OK(string msg,T result)
        {
            return new M_Return<T>
            {
                Status = true,
                Result = result,
                DescMsg = msg
            };
        }
        public static M_Return<T> Error()
        {
            return new M_Return<T>
            {
                Status = false,
                Result = default(T),
                DescMsg = "err"
            };
        }
        public static M_Return<T> Error(string msg)
        {
            return new M_Return<T>
            {
                Status = false,
                Result = default(T),
                DescMsg = msg
            };
        }
        public static M_Return<T> Error(string msg,T result)
        {
            return new M_Return<T>
            {
                Status = false,
                Result = result,
                DescMsg = msg
            };
        }

        //public override string ToString()
        //{
        //    return JsonConvert.SerializeObject(this);
        //}
    }
}
