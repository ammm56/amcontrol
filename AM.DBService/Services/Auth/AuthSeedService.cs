using AM.Core.Context;
using AM.Core.Reporter;
using AM.DBService.DBase;
using AM.Model.DB;
using AM.Model.Entity.Auth;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AM.DBService.Services.Auth
{
    /// <summary>
    /// 认证种子初始化服务。
    /// </summary>
    public class AuthSeedService
    {
        private readonly IAppReporter _reporter;
        private readonly DBContext _dbContext;

        public AuthSeedService() : this(SystemContext.Instance.Reporter)
        {
        }

        public AuthSeedService(IAppReporter reporter)
        {
            _reporter = reporter;
            _dbContext = new DBContext();
        }

        public void EnsureSeedData()
        {
            try
            {
                var db = _dbContext.GetClient(new MDBaseSet { DBType = "Sqlite" });

                EnsureTables(db);
                EnsureRoles(db);
                EnsureDefaultAdmin(db);
                // 使用内置目录作为兜底（首次运行或 NavigationCatalog 同步之前）
                EnsurePagePermissions(db);
            }
            catch (Exception ex)
            {
                _reporter?.Error("AuthSeed", ex, "认证种子初始化失败");
            }
        }

        /// <summary>
        /// 以外部目录（NavigationCatalog）为权威来源，全量同步权限目录到 DB。
        /// 新增条目会被插入，已有条目会被更新，DB 中多余的过期条目会被删除。
        /// 在 AppBootstrap 之后、LoginView 显示之前调用。
        /// </summary>
        public void SyncPagePermissions(List<SysPagePermissionEntity> catalog)
        {
            if (catalog == null || catalog.Count == 0)
            {
                _reporter?.Warn("AuthSeed", "页面权限目录为空，跳过同步");
                return;
            }

            try
            {
                var db = _dbContext.GetClient(new MDBaseSet { DBType = "Sqlite" });
                SyncPagePermissionsCore(db, catalog);
            }
            catch (Exception ex)
            {
                _reporter?.Error("AuthSeed", ex, "同步页面权限目录失败");
            }
        }

        // ─── 私有方法 ───

        private void EnsureTables(SqlSugarClient db)
        {
            db.CodeFirst.InitTables(
                typeof(SysUserEntity),
                typeof(SysRoleEntity),
                typeof(SysUserRoleEntity),
                typeof(SysLoginLogEntity),
                typeof(SysPagePermissionEntity),
                typeof(SysUserPagePermissionEntity));

            _reporter?.Info("AuthSeed", "认证与权限表初始化完成");
        }

        private void EnsureRoles(SqlSugarClient db)
        {
            EnsureRole(db, "Operator", "操作员");
            EnsureRole(db, "Engineer", "工程师");
            EnsureRole(db, "Am", "管理员");
        }

        private void EnsureRole(SqlSugarClient db, string roleCode, string roleName)
        {
            var exists = db.Queryable<SysRoleEntity>().Any(r => r.RoleCode == roleCode);
            if (exists)
            {
                return;
            }

            db.Insertable(new SysRoleEntity
            {
                RoleCode = roleCode,
                RoleName = roleName,
                Remark = "系统初始化角色",
                CreateTime = DateTime.Now
            }).ExecuteCommand();

            _reporter?.Info("AuthSeed", "已初始化角色：" + roleCode);
        }

        private void EnsureDefaultAdmin(SqlSugarClient db)
        {
            var adminUser = db.Queryable<SysUserEntity>().First(u => u.LoginName == "am");

            if (adminUser == null)
            {
                string salt;
                string hash;
                AuthService.CreatePasswordHash("am123", out salt, out hash);

                var user = new SysUserEntity
                {
                    LoginName = "am",
                    UserName = "系统管理员",
                    PasswordSalt = salt,
                    PasswordHash = hash,
                    IsEnabled = true,
                    IsAdmin = true,
                    UseCustomPagePermission = false,
                    FailedLoginCount = 0,
                    LockoutEndTime = null,
                    LastLoginTime = null,
                    Remark = "系统初始化默认管理员。首次部署后请尽快修改默认密码。",
                    CreateTime = DateTime.Now
                };

                db.Insertable(user).ExecuteCommand();
                adminUser = db.Queryable<SysUserEntity>().First(u => u.LoginName == "am");
                _reporter?.Warn("AuthSeed", "已初始化默认管理员：am / am123");
            }

            BindAdminRole(db, adminUser);
        }

        private void BindAdminRole(SqlSugarClient db, SysUserEntity adminUser)
        {
            if (adminUser == null)
            {
                return;
            }

            var adminRole = db.Queryable<SysRoleEntity>().First(r => r.RoleCode == "Am");
            if (adminRole == null)
            {
                _reporter?.Error("AuthSeed", "默认管理员角色不存在");
                return;
            }

            var roleBound = db.Queryable<SysUserRoleEntity>()
                .Any(x => x.UserId == adminUser.Id && x.RoleId == adminRole.Id);

            if (roleBound)
            {
                return;
            }

            db.Insertable(new SysUserRoleEntity
            {
                UserId = adminUser.Id,
                RoleId = adminRole.Id,
                CreateTime = DateTime.Now
            }).ExecuteCommand();

            _reporter?.Info("AuthSeed", "默认管理员角色绑定完成");
        }

        /// <summary>兜底：使用内置 AuthPermissionCatalog 做初次种子，避免首次启动权限表为空。</summary>
        private void EnsurePagePermissions(SqlSugarClient db)
        {
            // 若 DB 中已有条目，则跳过（SyncPagePermissions 会在稍后做精确全量同步）
            var count = db.Queryable<SysPagePermissionEntity>().Count();
            if (count > 0)
            {
                return;
            }

            var defaults = AuthPermissionCatalog.CreateDefaultPagePermissions();
            foreach (var item in defaults)
            {
                db.Insertable(item).ExecuteCommand();
            }

            _reporter?.Info("AuthSeed", "页面权限目录首次初始化，共 " + defaults.Count + " 条");
        }

        /// <summary>
        /// 全量同步核心：upsert + 删除 DB 中不在 catalog 里的过期条目。
        /// </summary>
        private void SyncPagePermissionsCore(SqlSugarClient db, List<SysPagePermissionEntity> catalog)
        {
            var exists = db.Queryable<SysPagePermissionEntity>().ToList();

            var existsMap = exists
                .Where(x => !string.IsNullOrWhiteSpace(x.PageKey))
                .GroupBy(x => x.PageKey, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(x => x.Key, x => x.First(), StringComparer.OrdinalIgnoreCase);

            var insertedCount = 0;
            var updatedCount = 0;

            foreach (var item in catalog)
            {
                SysPagePermissionEntity current;
                if (existsMap.TryGetValue(item.PageKey, out current))
                {
                    current.ModuleKey = item.ModuleKey;
                    current.ModuleName = item.ModuleName;
                    current.DisplayName = item.DisplayName;
                    current.Description = item.Description;
                    current.DefaultRoleCodes = item.DefaultRoleCodes;
                    current.RecommendedRoles = item.RecommendedRoles;
                    current.RiskLevel = item.RiskLevel;
                    current.SortOrder = item.SortOrder;
                    current.IsEnabled = item.IsEnabled;

                    if (current.CreateTime == default(DateTime))
                    {
                        current.CreateTime = item.CreateTime;
                    }

                    db.Updateable(current).ExecuteCommand();
                    updatedCount++;
                }
                else
                {
                    db.Insertable(item).ExecuteCommand();
                    insertedCount++;
                }
            }

            // 删除 DB 中有但 catalog 中已不存在的过期条目
            var catalogKeys = new HashSet<string>(
                catalog.Select(x => x.PageKey), StringComparer.OrdinalIgnoreCase);

            var deletedCount = 0;
            foreach (var stale in exists.Where(x => !catalogKeys.Contains(x.PageKey)).ToList())
            {
                db.Deleteable<SysPagePermissionEntity>()
                    .Where(x => x.Id == stale.Id)
                    .ExecuteCommand();
                deletedCount++;
                _reporter?.Info("AuthSeed", "删除过期权限目录项：" + stale.PageKey);
            }

            _reporter?.Info("AuthSeed",
                string.Format("权限目录同步完成，新增: {0}，更新: {1}，删除: {2}",
                    insertedCount, updatedCount, deletedCount));
        }
    }
}