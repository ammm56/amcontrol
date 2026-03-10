using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Model.Common
{
    /// <summary>
    /// 返回信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Response<T> : ObservableObject
    {
        /// <summary>
        /// 数据项
        /// 选择的单项数据
        /// </summary>
        private T _item;
        public T item
        {
            get => _item;
            set => SetProperty(ref _item, value); // 使用 SetProperty 触发通知
        }

        /// <summary>
        /// 数据集
        /// </summary>
        public List<T> data {  get; set; }
    }

    /// <summary>
    /// 数据库返回信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DBResponse<T>:Response<T>
    {
        /// <summary>
        /// 状态
        /// </summary>
        public bool status { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string message { get; set; }

    }
}
