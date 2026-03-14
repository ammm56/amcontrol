using AM.Model.Common;
using System.Collections.Generic;

namespace AM.Core.Reporter
{
    /// <summary>
    /// 错误目录接口，提供错误码到错误描述的映射查询功能，支持获取单个错误描述、批量获取所有错误描述，以及重新加载错误数据。
    /// </summary>
    public interface IErrorCatalog
    {
        ErrorDescriptor Get(int code);

        bool TryGet(int code, out ErrorDescriptor descriptor);

        IReadOnlyList<ErrorDescriptor> GetAll();

        void Reload();
    }
}