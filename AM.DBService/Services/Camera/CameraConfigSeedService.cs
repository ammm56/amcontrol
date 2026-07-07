using AM.Core.Context;
using AM.Core.Reporter;
using AM.DBService.DBase;
using AM.Model.Common;
using AM.Model.DB;
using AM.Model.Entity.Device;
using AM.Model.Vision;
using SqlSugar;
using System;

namespace AM.DBService.Services.Camera
{
    /// <summary>
    /// 相机配置种子服务。
    /// 仅负责初始化本项目相机表和默认 USB 相机参数。
    /// </summary>
    public class CameraConfigSeedService
    {
        private readonly IAppReporter _reporter;
        private readonly DBContext _dbContext;

        public CameraConfigSeedService()
            : this(SystemContext.Instance.Reporter)
        {
        }

        public CameraConfigSeedService(IAppReporter reporter)
        {
            _reporter = reporter;
            _dbContext = new DBContext();
        }

        public Result EnsureSeedData()
        {
            try
            {
                var db = _dbContext.GetClient(new MDBaseSet { DBType = "Sqlite" });
                CameraConfigCrudService.EnsureTables(db);

                if (db.Queryable<CameraConfigEntity>().Any())
                {
                    _reporter.Info("CameraConfigSeed", "相机配置种子已存在，跳过初始化");
                    return Result.Ok("相机配置种子已存在", ResultSource.Database);
                }

                db.Insertable(CreateDefaultUsbCamera()).ExecuteCommand();

                _reporter.Info("CameraConfigSeed", "默认 USB 相机配置种子初始化完成");
                return Result.Ok("默认 USB 相机配置种子初始化完成", ResultSource.Database);
            }
            catch (Exception ex)
            {
                _reporter.Error("CameraConfigSeed", ex, "默认 USB 相机配置种子初始化失败");
                return Result.Fail((int)DbErrorCode.SaveFailed, "默认 USB 相机配置种子初始化失败", ResultSource.Database);
            }
        }

        private static CameraConfigEntity CreateDefaultUsbCamera()
        {
            var now = DateTime.Now;

            return new CameraConfigEntity
            {
                CameraCode = "USB_CAMERA_0",
                CameraName = "默认USB相机",
                DriverType = CameraDriverType.Usb.ToString(),
                IsEnabled = true,
                DeviceIndex = 0,
                DevicePath = null,
                FriendlyName = "USB Camera 0",
                Width = 1920,
                Height = 1080,
                Fps = 30,
                PixelFormat = "MJPG",
                Exposure = null,
                Gain = null,
                GrabTimeoutMs = 3000,
                ImageFormat = CameraImageFormat.JPEG.ToString(),
                JpegQuality = 80,
                PreviewFps = 30,
                SaveImageEnabled = true,
                SaveImageDirectory = "Images\\Camera",
                Remark = "系统初始化默认 OpenCV DSHOW USB 相机配置",
                CreateTime = now,
                UpdateTime = now
            };
        }
    }
}
