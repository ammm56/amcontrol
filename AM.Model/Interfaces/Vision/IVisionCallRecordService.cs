using AM.Model.Common;
using AM.Model.Entity.Vision;
using System;

namespace AM.Model.Interfaces.Vision
{
    /// <summary>
    /// 视觉 SDK 调用记录服务接口。
    /// </summary>
    public interface IVisionCallRecordService
    {
        Result Save(VisionCallRecordEntity entity);

        Result<VisionCallRecordEntity> QueryPage(
            int pageIndex,
            int pageSize,
            string cameraCode = null,
            string callMode = null,
            bool? isSuccess = null,
            DateTime? startTime = null,
            DateTime? endTime = null);
    }
}
