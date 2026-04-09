using ProtocolLib.CommonLib.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolLib.CommonLib.Model
{
    /// <summary>
    /// 操作结果
    /// </summary>
    public class M_OperateResult
    {
        public M_OperateResult() { }

        public M_OperateResult(string msg)
        {
            Message = msg;
        }

        public M_OperateResult(int err, string msg)
        {
            ErrorCode = err;
            Message = msg;
        }

        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 错误的信息
        /// </summary>
        public string Message { get; set; } = CommonResources.Get.Language.Language.unknownerror;

        /// <summary>
        /// 错误的代码
        /// </summary>
        public int ErrorCode { get; set; } = 1000;

        /// <summary>
        /// 从一个结果中复制错误代码和信息
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="result"></param>
        public void CopyErrorFromOther<TResult>(TResult result) where TResult : M_OperateResult
        {
            if (result != null)
            {
                ErrorCode = result.ErrorCode;
                Message = result.Message;
            }
        }

        /// <summary>
        /// 创建返回成功结果对象
        /// </summary>
        /// <returns></returns>
        public static M_OperateResult CreateSuccessResult()
        {
            return new M_OperateResult()
            {
                IsSuccess = true,
                ErrorCode = 0,
                Message = CommonResources.Get.Language.Language.successtext
            };
        }

        /// <summary>
        /// 创建返回成功结果对象,有一个T参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static M_OperateResult<T> CreateSuccessResult<T>(T value)
        {
            return new M_OperateResult<T>()
            {
                IsSuccess = true,
                ErrorCode = 0,
                Message = CommonResources.Get.Language.Language.successtext,
                Content = value
            };
        }

        /// <summary>
        /// 创建返回成功结果对象
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static M_OperateResult<T1, T2> CreateSuccessResult<T1, T2>(T1 value1, T2 value2)
        {
            return new M_OperateResult<T1, T2>()
            {
                IsSuccess = true,
                ErrorCode = 0,
                Message = CommonResources.Get.Language.Language.successtext,
                Content1 = value1,
                Content2 = value2
            };
        }

        /// <summary>
        /// 创建返回一个失败的结果对象，复制另一个结果对象的错误信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        public static M_OperateResult<T> CreateFailedResult<T>(M_OperateResult result)
        {
            return new M_OperateResult<T>()
            {
                ErrorCode = result.ErrorCode,
                Message = result.Message
            };
        }

        /// <summary>
        /// 创建返回一个失败的结果对象，复制另一个结果对象的错误信息
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        public static M_OperateResult<T1, T2> CreateFailedResult<T1, T2>(M_OperateResult result)
        {
            return new M_OperateResult<T1, T2>()
            {
                ErrorCode = result.ErrorCode,
                Message = result.Message
            };
        }
    }

    /// <summary>
    /// 泛型操作结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class M_OperateResult<T> : M_OperateResult
    {
        public M_OperateResult() : base() { }

        public M_OperateResult(string msg) : base(msg) { }

        public M_OperateResult(int err, string msg) : base(err, msg) { }

        public T Content { get; set; }

        /// <summary>
        /// 返回Check结果对象
        /// 成功返回对象本身，失败返回错误信息
        /// </summary>
        /// <param name="check">检查的委托方法</param>
        /// <param name="message">检查失败的错误信息</param>
        /// <returns></returns>
        public M_OperateResult<T> Check(Func<T, bool> check, string message = "数据内容检查失败")
        {
            if (!IsSuccess) return this;
            if (check(Content)) return this;
            return new M_OperateResult<T>(message);
        }

        /// <summary>
        /// 返回Check结果对象，可自定义数据检查
        /// </summary>
        /// <param name="check">检查的委托方法</param>
        /// <returns></returns>
        public M_OperateResult<T> Check(Func<T, M_OperateResult> check)
        {
            if (!IsSuccess) return this;
            M_OperateResult checkResult = check(Content);
            if (!checkResult.IsSuccess) return CreateFailedResult<T>(checkResult);

            return this;
        }

        /// <summary>
        /// 接下来要做的内容
        /// 成功返回接下的执行结果，失败返回当前对象
        /// </summary>
        /// <typeparam name="TResult">等待当前对象成功后执行的内容</typeparam>
        /// <param name="func">返回整个方法链最终的成功失败结果</param>
        /// <returns></returns>
        public M_OperateResult<TResult> Then<TResult>(Func<T, M_OperateResult<TResult>> func)
        {
            return IsSuccess ? func(Content) : CreateFailedResult<TResult>(this);
        }
    }

    /// <summary>
    /// 泛型操作结果
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public class M_OperateResult<T1, T2> : M_OperateResult
    {
        public M_OperateResult() : base() { }

        public M_OperateResult(string msg) : base(msg) { }

        public M_OperateResult(int err, string msg) : base(err, msg) { }

        public T1 Content1 { get; set; }

        public T2 Content2 { get; set; }

        /// <summary>
        /// 返回Check结果对象
        /// 成功返回对象本身，失败返回错误信息
        /// </summary>
        /// <param name="check">检查的委托方法</param>
        /// <param name="message">检查失败的错误信息</param>
        /// <returns></returns>
        public M_OperateResult<T1, T2> Check(Func<T1, T2, bool> check, string message = "数据内容检查失败")
        {
            if (!IsSuccess) return this;
            if (check(Content1, Content2)) return this;
            return new M_OperateResult<T1, T2>(message);
        }

        /// <summary>
        /// 返回Check结果对象，可自定义数据检查
        /// </summary>
        /// <param name="check">检查的委托方法</param>
        /// <returns></returns>
        public M_OperateResult<T1, T2> Check(Func<T1, T2, M_OperateResult> check)
        {
            if (!IsSuccess) return this;
            M_OperateResult checkResult = check(Content1, Content2);
            if (!checkResult.IsSuccess) return CreateFailedResult<T1, T2>(checkResult);

            return this;
        }

        /// <summary>
        /// 接下来要做的内容
        /// 成功返回接下的执行结果，失败返回当前对象
        /// </summary>
        /// <param name="func">返回整个方法链最终的成功失败结果</param>
        /// <returns></returns>
        public M_OperateResult Then(Func<T1, T2, M_OperateResult> func)
        {
            return IsSuccess ? func(Content1, Content2) : this;
        }

        public M_OperateResult<TResult> Then<TResult>(Func<T1, T2, M_OperateResult<TResult>> func)
        {
            return IsSuccess ? func(Content1, Content2) : CreateFailedResult<TResult>(this);
        }
    }
}