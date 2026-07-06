using AM.Model.Common;
using AM.Model.Entity.Device;

namespace AM.Model.Interfaces.Camera
{
    /// <summary>
    /// 本项目相机配置 CRUD 服务接口。
    /// </summary>
    public interface ICameraConfigCrudService
    {
        Result<CameraConfigEntity> QueryAll();

        Result<CameraConfigEntity> QueryByCode(string cameraCode);

        Result Save(CameraConfigEntity entity);

        Result DeleteByCode(string cameraCode);
    }
}
