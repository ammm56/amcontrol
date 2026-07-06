using AM.Model.Common;
using AM.Model.Vision;
using System.Threading;
using System.Threading.Tasks;

namespace AM.Model.Interfaces.Vision
{
    /// <summary>
    /// 本项目视觉调用服务。
    /// </summary>
    public interface IVisionInvokeService
    {
        Task<Result<VisionInvokeResult>> InvokeZeroMqImageBytesAsync(
            string triggerSourceName,
            byte[] imageBytes,
            string mediaType = "image/jpeg",
            CancellationToken cancellationToken = default(CancellationToken));

        Task<Result<VisionInvokeResult>> InvokeRuntimeAppResultWithImageBytesAsync(
            string runtimeName,
            byte[] imageBytes,
            string mediaType = "image/jpeg",
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
