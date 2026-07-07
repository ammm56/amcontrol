using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.DBService.DBase;
using AM.Model.Common;
using AM.Model.DB;
using AM.Model.Entity.Device;
using AM.Model.Interfaces.Camera;
using AM.Model.Vision;
using SqlSugar;
using System;
using System.Linq;

namespace AM.DBService.Services.Camera
{
    /// <summary>
    /// 相机配置 CRUD 服务。
    /// 对应数据库表：device_camera_config。
    /// </summary>
    public class CameraConfigCrudService : ServiceBase, ICameraConfigCrudService
    {
        private readonly DBContext _dbContext;

        protected override string MessageSourceName
        {
            get { return "CameraConfigCrud"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Database; }
        }

        public CameraConfigCrudService()
            : this(SystemContext.Instance.Reporter)
        {
        }

        public CameraConfigCrudService(IAppReporter reporter)
            : base(reporter)
        {
            _dbContext = new DBContext();
        }

        public Result<CameraConfigEntity> QueryAll()
        {
            try
            {
                var db = CreateDb();
                EnsureTables(db);

                var items = db.Queryable<CameraConfigEntity>()
                    .ToList()
                    .OrderBy(x => x.Id)
                    .ToList();

                return OkListLogOnly(items, "相机配置查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<CameraConfigEntity>(ex, (int)DbErrorCode.QueryFailed, "相机配置查询失败");
            }
        }

        public Result<CameraConfigEntity> QueryByCode(string cameraCode)
        {
            try
            {
                var normalizedCode = NormalizeCode(cameraCode);
                if (string.IsNullOrWhiteSpace(normalizedCode))
                {
                    return Fail<CameraConfigEntity>((int)DbErrorCode.InvalidArgument, "相机编码不能为空");
                }

                var db = CreateDb();
                EnsureTables(db);

                var item = db.Queryable<CameraConfigEntity>()
                    .First(x => x.CameraCode == normalizedCode);

                if (item == null)
                {
                    return Warn<CameraConfigEntity>((int)DbErrorCode.NotFound, "未找到对应相机配置");
                }

                return OkLogOnly(item, "相机配置查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<CameraConfigEntity>(ex, (int)DbErrorCode.QueryFailed, "按编码查询相机配置失败");
            }
        }

        public Result Save(CameraConfigEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "相机配置不能为空");
                }

                Normalize(entity);

                var validateResult = Validate(entity);
                if (!validateResult.Success)
                {
                    return validateResult;
                }

                var db = CreateDb();
                EnsureTables(db);

                var exists = db.Queryable<CameraConfigEntity>()
                    .First(x => x.CameraCode == entity.CameraCode && x.Id != entity.Id);

                if (exists != null)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "相机编码已存在: " + entity.CameraCode);
                }

                var now = DateTime.Now;
                if (entity.Id <= 0 && entity.CreateTime == default(DateTime))
                {
                    entity.CreateTime = now;
                }

                entity.UpdateTime = now;

                if (entity.Id > 0)
                {
                    db.Updateable(entity).ExecuteCommand();
                }
                else
                {
                    db.Insertable(entity).ExecuteCommand();
                }

                return Ok("相机配置保存成功");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.SaveFailed, "相机配置保存失败");
            }
        }

        public Result DeleteByCode(string cameraCode)
        {
            try
            {
                var normalizedCode = NormalizeCode(cameraCode);
                if (string.IsNullOrWhiteSpace(normalizedCode))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "相机编码不能为空");
                }

                var db = CreateDb();
                EnsureTables(db);

                var exists = db.Queryable<CameraConfigEntity>()
                    .Any(x => x.CameraCode == normalizedCode);

                if (!exists)
                {
                    return Warn((int)DbErrorCode.NotFound, "未找到要删除的相机配置");
                }

                db.Deleteable<CameraConfigEntity>()
                    .Where(x => x.CameraCode == normalizedCode)
                    .ExecuteCommand();

                return Ok("相机配置删除成功");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.DeleteFailed, "相机配置删除失败");
            }
        }

        internal static void EnsureTables(SqlSugarClient db)
        {
            db.CodeFirst.InitTables(typeof(CameraConfigEntity));

            db.Ado.ExecuteCommand(
                "CREATE UNIQUE INDEX IF NOT EXISTS ux_device_camera_config_code ON device_camera_config(CameraCode)");
        }

        private SqlSugarClient CreateDb()
        {
            return _dbContext.GetClient(new MDBaseSet { DBType = "Sqlite" });
        }

        private static void Normalize(CameraConfigEntity entity)
        {
            entity.CameraCode = NormalizeCode(entity.CameraCode);
            entity.CameraName = NormalizeNullableText(entity.CameraName);
            entity.DriverType = NormalizeNullableText(entity.DriverType);
            entity.DevicePath = NormalizeNullableText(entity.DevicePath);
            entity.FriendlyName = NormalizeNullableText(entity.FriendlyName);
            entity.PixelFormat = NormalizeNullableText(entity.PixelFormat);
            entity.ImageFormat = NormalizeImageFormat(entity.ImageFormat);
            entity.SaveImageDirectory = NormalizeNullableText(entity.SaveImageDirectory);
            entity.Remark = NormalizeNullableText(entity.Remark);

            if (string.IsNullOrWhiteSpace(entity.CameraName) && !string.IsNullOrWhiteSpace(entity.CameraCode))
            {
                entity.CameraName = entity.CameraCode;
            }

            if (string.IsNullOrWhiteSpace(entity.DriverType))
            {
                entity.DriverType = CameraDriverType.Usb.ToString();
            }

            if (string.IsNullOrWhiteSpace(entity.ImageFormat))
            {
                entity.ImageFormat = CameraImageFormat.JPEG.ToString();
            }

            if (string.IsNullOrWhiteSpace(entity.PixelFormat))
            {
                entity.PixelFormat = "MJPG";
            }
            else if (string.Equals(entity.PixelFormat, "MJPEG", StringComparison.OrdinalIgnoreCase))
            {
                entity.PixelFormat = "MJPG";
            }

            if (entity.Width <= 0)
            {
                entity.Width = 1920;
            }

            if (entity.Height <= 0)
            {
                entity.Height = 1080;
            }

            if (entity.Fps <= 0)
            {
                entity.Fps = 30;
            }

            if (entity.GrabTimeoutMs <= 0)
            {
                entity.GrabTimeoutMs = 3000;
            }

            if (entity.JpegQuality <= 0 || entity.JpegQuality > 100)
            {
                entity.JpegQuality = 80;
            }

            if (entity.PreviewFps <= 0)
            {
                entity.PreviewFps = entity.Fps <= 0 ? 30 : entity.Fps;
            }

            if (entity.Fps > 0 && entity.PreviewFps > entity.Fps)
            {
                entity.PreviewFps = entity.Fps;
            }

            if (string.IsNullOrWhiteSpace(entity.SaveImageDirectory))
            {
                entity.SaveImageDirectory = "Images\\Camera";
            }
        }

        private Result Validate(CameraConfigEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.CameraCode))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "相机编码不能为空");
            }

            if (string.IsNullOrWhiteSpace(entity.DriverType))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "相机驱动类型不能为空");
            }

            if (entity.Width <= 0 || entity.Height <= 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "相机分辨率必须大于 0");
            }

            if (entity.Fps <= 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "相机 FPS 必须大于 0");
            }

            if (entity.GrabTimeoutMs <= 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "取图超时必须大于 0");
            }

            if (entity.JpegQuality < 1 || entity.JpegQuality > 100)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "JPEG 质量必须在 1 到 100 之间");
            }

            return OkSilent("相机配置校验通过");
        }

        private static string NormalizeCode(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        private static string NormalizeNullableText(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        private static string NormalizeImageFormat(string value)
        {
            var text = NormalizeNullableText(value);
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            return text.ToUpperInvariant();
        }
    }
}
