using AM.Model.Common;
using System.Collections.Generic;

namespace AM.Core.Reporter
{
    /// <summary>
    /// 错误目录接口。
    /// 提供错误码到错误描述对象的映射查询能力。
    /// </summary>
    public interface IErrorCatalog
    {
        /// <summary>
        /// 根据错误码获取错误描述。
        /// 若不存在则返回默认未知错误描述对象。
        /// </summary>
        /// <param name="code">错误码。</param>
        /// <returns>错误描述对象。</returns>
        ErrorDescriptor Get(int code);

        /// <summary>
        /// 尝试根据错误码获取错误描述。
        /// </summary>
        /// <param name="code">错误码。</param>
        /// <param name="descriptor">输出错误描述对象。</param>
        /// <returns>是否找到。</returns>
        bool TryGet(int code, out ErrorDescriptor descriptor);

        /// <summary>
        /// 获取全部错误描述。
        /// </summary>
        /// <returns>错误描述集合。</returns>
        IReadOnlyList<ErrorDescriptor> GetAll();

        /// <summary>
        /// 重新加载错误目录。
        /// </summary>
        void Reload();
    }
}