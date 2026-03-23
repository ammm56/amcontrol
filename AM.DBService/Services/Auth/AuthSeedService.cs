using AM.Core.Context;
using AM.Core.Reporter;
using AM.DBService.DBase;
using AM.Model.DB;
using AM.Model.Entity.Auth;
using SqlSugar;
using System;
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
                EnsurePagePermissions(db);
            }
            catch (Exception ex)
            {
                _reporter?.Error("AuthSeed", ex, "认证种子初始化失败");
            }
        }

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

        private void EnsurePagePermissions(SqlSugarClient db)
        {
            var defaults = AuthPermissionCatalog.CreateDefaultPagePermissions();
            var exists = db.Queryable<SysPagePermissionEntity>().ToList();

            var existsMap = exists
                .Where(x => !string.IsNullOrWhiteSpace(x.PageKey))
                .GroupBy(x => x.PageKey, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(x => x.Key, x => x.First(), StringComparer.OrdinalIgnoreCase);

            var insertedCount = 0;
            var updatedCount = 0;

            foreach (var item in defaults)
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

            _reporter?.Info(
                "AuthSeed",
                "页面权限目录同步完成，新增: " + insertedCount + "，更新: " + updatedCount);
        }
    }
}