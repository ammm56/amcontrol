using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Model.Common
{
    /// <summary>
    /// 统一返回结果（无数据）
    /// </summary>
    public class Result
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 结果码
        /// 0 通常表示成功，非 0 表示失败或警告
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public ResultSource Source { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; set; } = DateTime.Now;

        protected Result()
        {
            Time = DateTime.Now;
            Message = string.Empty;
            Source = ResultSource.Unknown;
        }

        protected Result(bool success, int code, string message, ResultSource source)
        {
            Success = success;
            Code = code;
            Message = message ?? string.Empty;
            Source = source;
            Time = DateTime.Now;
        }

        public static Result Ok(string message = "OK", ResultSource source = ResultSource.Unknown)
        {
            return new Result(true, 0, message, source);
        }

        public static Result Fail(int code, string message, ResultSource source = ResultSource.Unknown)
        {
            return new Result(false, code, message, source);
        }
    }

    /// <summary>
    /// 统一返回结果（带数据）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result<T> : Result
    {
        /// <summary>
        /// 单项数据
        /// </summary>
        public T Item { get; set; }

        /// <summary>
        /// 数据集：允许外部读，不允许随意写
        /// </summary>
        public IReadOnlyList<T> Items { get; protected set; } = new List<T>();

        protected Result(): base()
        {
            Items = new List<T>();
        }

        public Result(bool success, int code, string message, ResultSource source): base(success, code, message, source)
        {
            Items = new List<T>();
        }

        public static Result<T> OkItem(T item, string message = "OK", ResultSource source = ResultSource.Unknown)
        {
            return new Result<T>
            {
                Success = true,
                Code = 0,
                Message = message ?? string.Empty,
                Source = source,
                Time = DateTime.Now,
                Item = item,
                Items = new List<T>()
            };
        }

        public static Result<T> OkList(List<T> items, string message = "OK", ResultSource source = ResultSource.Unknown)
        {
            return new Result<T>
            {
                Success = true,
                Code = 0,
                Message = message ?? string.Empty,
                Source = source,
                Time = DateTime.Now,
                Items = items ?? new List<T>()
            };
        }

        /// <summary>
        /// 简化了调用者的代码，不需要先将 IEnumerable 转成 List 就能直接使用
        /// </summary>
        /// <param name="items"></param>
        /// <param name="message"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Result<T> OkList(IEnumerable<T> items, string message = "OK", ResultSource source = ResultSource.Unknown)
        {
            return new Result<T>
            {
                Success = true,
                Code = 0,
                Message = message ?? string.Empty,
                Source = source,
                Time = DateTime.Now,
                Items = items == null ? new List<T>() : items.ToList()
            };
        }

        public static new Result<T> Fail(int code, string message, ResultSource source = ResultSource.Unknown)
        {
            return new Result<T>
            {
                Success = false,
                Code = code,
                Message = message ?? string.Empty,
                Source = source,
                Time = DateTime.Now,
                Items = new List<T>()
            };
        }
    }

}
