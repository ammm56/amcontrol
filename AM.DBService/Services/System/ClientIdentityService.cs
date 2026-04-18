using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.DBService.DBase;
using AM.Model.Common;
using AM.Model.Entity.System;
using System;
using System.Linq;

namespace AM.DBService.Services.System
{
    /// <summary>
    /// 客户端身份信息服务。
    /// 
    /// 职责：
    /// 1. 确保本地身份表存在；
    /// 2. 优先从 config.json 读取客户端身份；
    /// 3. 当 config.json 缺失身份信息时，回退到本地表；
    /// 4. 当本地表也不存在时，自动创建默认身份；
    /// 5. 将 ClientId / MachineCode / MachineName 回写到 config.json。
    /// </summary>
    public class ClientIdentityService : ServiceBase
    {
        private readonly DBCommon<SysClientIdentityEntity> _identityDb;

        private SysClientIdentityEntity _cachedIdentity;

        protected override string MessageSourceName
        {
            get { return "ClientIdentityService"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Database; }
        }

        public ClientIdentityService()
            : base()
        {
            _identityDb = new DBCommon<SysClientIdentityEntity>();
            EnsureTableSchema();
        }

        public ClientIdentityService(IAppReporter reporter)
            : base(reporter)
        {
            _identityDb = new DBCommon<SysClientIdentityEntity>();
            EnsureTableSchema();
        }

        /// <summary>
        /// 确保本地身份表存在。
        /// </summary>
        private void EnsureTableSchema()
        {
            try
            {
                _identityDb._sqlSugarClient.CodeFirst.InitTables<SysClientIdentityEntity>();
            }
            catch (Exception ex)
            {
                _reporter?.Error(MessageSourceName, ex, "初始化 sys_client_identity 表失败", -1, null, ReportChannels.Log);
            }
        }

        /// <summary>
        /// 获取当前客户端身份信息。
        /// 优先级：
        /// 1. 内存缓存；
        /// 2. config.json；
        /// 3. 本地表；
        /// 4. 自动创建默认身份。
        /// </summary>
        public Result<SysClientIdentityEntity> GetCurrent()
        {
            if (_cachedIdentity != null)
            {
                return OkSilent(_cachedIdentity, "读取客户端身份成功");
            }

            try
            {
                SysClientIdentityEntity configIdentity = BuildIdentityFromConfig();
                if (!string.IsNullOrWhiteSpace(configIdentity.ClientId))
                {
                    _cachedIdentity = configIdentity;
                    EnsureDbIdentityUpToDate(configIdentity);
                    return OkSilent(_cachedIdentity, "读取客户端身份成功");
                }

                Result<SysClientIdentityEntity> dbResult = QueryFirstFromDatabase();
                if (dbResult.Success && dbResult.Item != null)
                {
                    _cachedIdentity = dbResult.Item;
                    SaveIdentityToConfig(_cachedIdentity);
                    return OkSilent(_cachedIdentity, "读取客户端身份成功");
                }

                return CreateDefaultIdentity();
            }
            catch (Exception ex)
            {
                return Fail<SysClientIdentityEntity>(-1, "读取客户端身份异常", ReportChannels.Log, ex);
            }
        }

        /// <summary>
        /// 保存设备信息。
        /// </summary>
        public Result SaveMachineInfo(string machineCode, string machineName)
        {
            try
            {
                Result<SysClientIdentityEntity> currentResult = GetCurrent();
                if (!currentResult.Success || currentResult.Item == null)
                {
                    return Fail(currentResult.Code == 0 ? -1 : currentResult.Code, "获取客户端身份失败，无法保存设备信息");
                }

                SysClientIdentityEntity entity = currentResult.Item;
                entity.MachineCode = machineCode ?? string.Empty;
                entity.MachineName = string.IsNullOrWhiteSpace(machineName)
                    ? Environment.MachineName
                    : machineName.Trim();
                entity.UpdateTime = DateTime.Now;

                Result saveDbResult = SaveIdentityToDatabase(entity);
                if (!saveDbResult.Success)
                {
                    return saveDbResult;
                }

                Result saveConfigResult = SaveIdentityToConfig(entity);
                if (!saveConfigResult.Success)
                {
                    return saveConfigResult;
                }

                _cachedIdentity = entity;
                return OkSilent("保存客户端设备信息成功");
            }
            catch (Exception ex)
            {
                return Fail(-1, "保存客户端设备信息异常", ReportChannels.Log, ex);
            }
        }

        /// <summary>
        /// 从 config.json 构建客户端身份对象。
        /// </summary>
        private SysClientIdentityEntity BuildIdentityFromConfig()
        {
            Setting setting = ConfigContext.Instance.Config.Setting;

            return new SysClientIdentityEntity
            {
                ClientId = setting.ClientId ?? string.Empty,
                AppCode = BackendServiceConfigHelper.GetDesktopAppCode(),
                MachineCode = setting.MachineCode ?? string.Empty,
                MachineName = string.IsNullOrWhiteSpace(setting.MachineName)
                    ? Environment.MachineName
                    : setting.MachineName,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
        }

        /// <summary>
        /// 查询本地表中的首条客户端身份。
        /// </summary>
        private Result<SysClientIdentityEntity> QueryFirstFromDatabase()
        {
            try
            {
                Result<SysClientIdentityEntity> queryResult = _identityDb.QueryAll();
                if (!queryResult.Success)
                {
                    return Fail<SysClientIdentityEntity>(queryResult.Code, "查询客户端身份失败");
                }

                SysClientIdentityEntity entity = queryResult.Items == null
                    ? null
                    : queryResult.Items.FirstOrDefault();

                if (entity == null)
                {
                    return WarnSilent<SysClientIdentityEntity>(-1, "未找到客户端身份记录");
                }

                return OkSilent(entity, "查询客户端身份成功");
            }
            catch (Exception ex)
            {
                return Fail<SysClientIdentityEntity>(-1, "查询客户端身份异常", ReportChannels.Log, ex);
            }
        }

        /// <summary>
        /// 创建默认客户端身份。
        /// </summary>
        private Result<SysClientIdentityEntity> CreateDefaultIdentity()
        {
            try
            {
                SysClientIdentityEntity entity = new SysClientIdentityEntity
                {
                    ClientId = AM.Tools.Tools.Guid(24),
                    AppCode = BackendServiceConfigHelper.GetDesktopAppCode(),
                    MachineCode = string.Empty,
                    MachineName = Environment.MachineName,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };

                Result saveDbResult = SaveIdentityToDatabase(entity);
                if (!saveDbResult.Success)
                {
                    return Fail<SysClientIdentityEntity>(saveDbResult.Code, "创建默认客户端身份失败");
                }

                Result saveConfigResult = SaveIdentityToConfig(entity);
                if (!saveConfigResult.Success)
                {
                    return Fail<SysClientIdentityEntity>(saveConfigResult.Code, "回写默认客户端身份到配置失败");
                }

                _cachedIdentity = entity;
                return OkSilent(entity, "创建默认客户端身份成功");
            }
            catch (Exception ex)
            {
                return Fail<SysClientIdentityEntity>(-1, "创建默认客户端身份异常", ReportChannels.Log, ex);
            }
        }

        /// <summary>
        /// 保证数据库中的客户端身份与配置一致。
        /// config.json 已有 ClientId 时，若本地表为空则补写；
        /// 若本地表存在首条记录则更新其设备信息。
        /// </summary>
        private void EnsureDbIdentityUpToDate(SysClientIdentityEntity configIdentity)
        {
            try
            {
                Result<SysClientIdentityEntity> dbResult = QueryFirstFromDatabase();
                if (!dbResult.Success || dbResult.Item == null)
                {
                    SaveIdentityToDatabase(configIdentity);
                    return;
                }

                SysClientIdentityEntity dbEntity = dbResult.Item;
                dbEntity.ClientId = configIdentity.ClientId ?? string.Empty;
                dbEntity.AppCode = BackendServiceConfigHelper.GetDesktopAppCode();
                dbEntity.MachineCode = configIdentity.MachineCode ?? string.Empty;
                dbEntity.MachineName = configIdentity.MachineName ?? string.Empty;
                dbEntity.UpdateTime = DateTime.Now;

                SaveIdentityToDatabase(dbEntity);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 保存身份到本地数据库。
        /// 当前仅维护首条记录。
        /// </summary>
        private Result SaveIdentityToDatabase(SysClientIdentityEntity entity)
        {
            try
            {
                Result<SysClientIdentityEntity> dbResult = QueryFirstFromDatabase();
                if (dbResult.Success && dbResult.Item != null)
                {
                    SysClientIdentityEntity dbEntity = dbResult.Item;
                    dbEntity.ClientId = entity.ClientId ?? string.Empty;
                    dbEntity.AppCode = BackendServiceConfigHelper.GetDesktopAppCode();
                    dbEntity.MachineCode = entity.MachineCode ?? string.Empty;
                    dbEntity.MachineName = entity.MachineName ?? string.Empty;
                    dbEntity.UpdateTime = DateTime.Now;

                    Result editResult = _identityDb.Edit(dbEntity);
                    if (!editResult.Success)
                    {
                        return Fail(editResult.Code, "更新客户端身份失败");
                    }

                    entity.Id = dbEntity.Id;
                    return OkSilent("更新客户端身份成功");
                }

                entity.AppCode = BackendServiceConfigHelper.GetDesktopAppCode();
                entity.CreateTime = entity.CreateTime == default(DateTime) ? DateTime.Now : entity.CreateTime;
                entity.UpdateTime = DateTime.Now;

                Result addResult = _identityDb.Add(entity);
                if (!addResult.Success)
                {
                    return Fail(addResult.Code, "新增客户端身份失败");
                }

                return OkSilent("新增客户端身份成功");
            }
            catch (Exception ex)
            {
                return Fail(-1, "保存客户端身份异常", ReportChannels.Log, ex);
            }
        }

        /// <summary>
        /// 将身份回写到 config.json。
        /// </summary>
        private Result SaveIdentityToConfig(SysClientIdentityEntity entity)
        {
            try
            {
                Setting setting = ConfigContext.Instance.Config.Setting;
                setting.ClientId = entity.ClientId ?? string.Empty;
                setting.MachineCode = entity.MachineCode ?? string.Empty;
                setting.MachineName = entity.MachineName ?? string.Empty;

                Result saveResult = AM.Tools.Tools.SaveConfig("config.json", ConfigContext.Instance.Config);
                if (!saveResult.Success)
                {
                    return Fail(saveResult.Code, "保存客户端身份配置失败");
                }

                return OkSilent("保存客户端身份配置成功");
            }
            catch (Exception ex)
            {
                return Fail(-1, "保存客户端身份配置异常", ReportChannels.Log, ex);
            }
        }
    }
}