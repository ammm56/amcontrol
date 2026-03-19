using AM.Core.Context;
using AM.Core.Reporter;
using AM.DBService.DBase;
using AM.Model.Auth;
using AM.Model.Common;
using AM.Model.Entity.Auth;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;

namespace AM.DBService.Services.Auth
{
    /// <summary>
    /// 认证服务。
    /// </summary>
    public class AuthService
    {
        private const int PasswordIterations = 10000;
        private const int PasswordByteSize = 32;

        private readonly DBCommon<SysUserEntity> _userDb;
        private readonly DBCommon<SysRoleEntity> _roleDb;
        private readonly DBCommon<SysUserRoleEntity> _userRoleDb;
        private readonly DBCommon<SysLoginLogEntity> _loginLogDb;
        private readonly DBCommon<SysPagePermissionEntity> _pagePermissionDb;
        private readonly DBCommon<SysUserPagePermissionEntity> _userPagePermissionDb;
        private readonly IAppReporter _reporter;

        public AuthService() : this(SystemContext.Instance.Reporter)
        {
        }

        public AuthService(IAppReporter reporter)
        {
            _userDb = new DBCommon<SysUserEntity>();
            _roleDb = new DBCommon<SysRoleEntity>();
            _userRoleDb = new DBCommon<SysUserRoleEntity>();
            _loginLogDb = new DBCommon<SysLoginLogEntity>();
            _pagePermissionDb = new DBCommon<SysPagePermissionEntity>();
            _userPagePermissionDb = new DBCommon<SysUserPagePermissionEntity>();
            _reporter = reporter;
        }

        public Result<SysUserEntity> Login(string loginName, string password, string clientInfo = null)
        {
            if (string.IsNullOrWhiteSpace(loginName))
            {
                return Result<SysUserEntity>.Fail(-1, "登录名不能为空", ResultSource.Unknown);
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return Result<SysUserEntity>.Fail(-1, "密码不能为空", ResultSource.Unknown);
            }

            var userQuery = _userDb.QueryAll();
            if (!userQuery.Success)
            {
                _reporter?.Error("AuthService", "查询用户失败", userQuery.Code);
                return Result<SysUserEntity>.Fail(userQuery.Code, "查询用户失败", ResultSource.Database);
            }

            var user = userQuery.Items.FirstOrDefault(u =>
                string.Equals(u.LoginName, loginName, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                SaveLoginLog(null, loginName, false, "用户不存在", clientInfo);
                _reporter?.Warn("AuthService", "登录失败：用户不存在");
                return Result<SysUserEntity>.Fail(-2, "用户不存在", ResultSource.Unknown);
            }

            if (!user.IsEnabled)
            {
                SaveLoginLog(user.Id, loginName, false, "用户已禁用", clientInfo);
                _reporter?.Warn("AuthService", "登录失败：用户已禁用");
                return Result<SysUserEntity>.Fail(-3, "用户已禁用", ResultSource.Unknown);
            }

            if (!VerifyPassword(password, user.PasswordSalt, user.PasswordHash))
            {
                SaveLoginLog(user.Id, loginName, false, "密码错误", clientInfo);
                _reporter?.Warn("AuthService", "登录失败：密码错误");
                return Result<SysUserEntity>.Fail(-4, "密码错误", ResultSource.Unknown);
            }

            var rolesResult = GetRolesByUserId(user.Id);
            if (!rolesResult.Success)
            {
                SaveLoginLog(user.Id, loginName, false, "查询角色失败", clientInfo);
                return Result<SysUserEntity>.Fail(rolesResult.Code, "查询角色失败", ResultSource.Database);
            }

            var permissionResult = GetUserPagePermissions(user.Id);
            if (!permissionResult.Success)
            {
                SaveLoginLog(user.Id, loginName, false, "查询页面权限失败", clientInfo);
                return Result<SysUserEntity>.Fail(permissionResult.Code, "查询页面权限失败", ResultSource.Database);
            }

            user.LastLoginTime = DateTime.Now;
            var editResult = _userDb.Edit(user);
            if (!editResult.Success)
            {
                _reporter?.Warn("AuthService", "更新最后登录时间失败", editResult.Code);
            }

            UserContext.Instance.SignIn(user, rolesResult.Items, permissionResult.Items);

            SaveLoginLog(user.Id, loginName, true, "登录成功", clientInfo);
            _reporter?.Info("AuthService", "登录成功");

            return Result<SysUserEntity>.OkItem(user, "登录成功", ResultSource.Unknown);
        }

        public Result Logout()
        {
            var loginName = UserContext.Instance.LoginName;
            UserContext.Instance.SignOut();
            _reporter?.Info("AuthService", "退出登录：" + loginName);
            return Result.Ok("退出成功", ResultSource.Unknown);
        }

        public Result DeleteUser(int userId)
        {
            if (userId <= 0)
            {
                return Result.Fail(-80, "用户标识无效", ResultSource.Unknown);
            }

            if (UserContext.Instance.IsLoggedIn &&
                UserContext.Instance.CurrentUser != null &&
                UserContext.Instance.CurrentUser.Id == userId)
            {
                return Result.Fail(-81, "不能删除当前登录用户", ResultSource.Unknown);
            }

            var userQuery = _userDb.QueryAll();
            if (!userQuery.Success)
            {
                _reporter?.Error("AuthService", "删除用户时查询用户失败", userQuery.Code);
                return Result.Fail(userQuery.Code, "查询用户失败", ResultSource.Database);
            }

            var user = userQuery.Items.FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                return Result.Fail(-82, "用户不存在", ResultSource.Unknown);
            }

            try
            {
                var db = _userDb._sqlSugarClient;
                db.Ado.BeginTran();

                db.Deleteable<SysUserPagePermissionEntity>()
                    .Where(x => x.UserId == userId)
                    .ExecuteCommand();

                db.Deleteable<SysUserRoleEntity>()
                    .Where(x => x.UserId == userId)
                    .ExecuteCommand();

                db.Deleteable<SysUserEntity>()
                    .Where(x => x.Id == userId)
                    .ExecuteCommand();

                db.Ado.CommitTran();

                _reporter?.Info("AuthService", "删除用户成功：" + user.LoginName);
                return Result.Ok("删除用户成功", ResultSource.Database);
            }
            catch (Exception ex)
            {
                _userDb._sqlSugarClient.Ado.RollbackTran();
                _reporter?.Error("AuthService", ex, "删除用户失败");
                return Result.Fail(-83, "删除用户失败", ResultSource.Database);
            }
        }

        public Result<SysRoleEntity> GetRolesByUserId(int userId)
        {
            var userRoleQuery = _userRoleDb.QueryAll();
            if (!userRoleQuery.Success)
            {
                _reporter?.Error("AuthService", "查询用户角色关联失败", userRoleQuery.Code);
                return Result<SysRoleEntity>.Fail(userRoleQuery.Code, "查询用户角色关联失败", ResultSource.Database);
            }

            var roleQuery = _roleDb.QueryAll();
            if (!roleQuery.Success)
            {
                _reporter?.Error("AuthService", "查询角色失败", roleQuery.Code);
                return Result<SysRoleEntity>.Fail(roleQuery.Code, "查询角色失败", ResultSource.Database);
            }

            var roleIds = userRoleQuery.Items
                .Where(x => x.UserId == userId)
                .Select(x => x.RoleId)
                .Distinct()
                .ToList();

            var roles = roleQuery.Items
                .Where(r => roleIds.Contains(r.Id))
                .ToList();

            return Result<SysRoleEntity>.OkList(roles, "查询角色成功", ResultSource.Database);
        }

        public Result<SysPagePermissionEntity> GetPagePermissions()
        {
            var queryResult = _pagePermissionDb.QueryAll();
            if (!queryResult.Success)
            {
                _reporter?.Error("AuthService", "查询页面权限目录失败", queryResult.Code);
                return Result<SysPagePermissionEntity>.Fail(queryResult.Code, "查询页面权限目录失败", ResultSource.Database);
            }

            var list = queryResult.Items
                .Where(x => x.IsEnabled)
                .OrderBy(x => x.ModuleKey)
                .ThenBy(x => x.SortOrder)
                .ThenBy(x => x.DisplayName)
                .ToList();

            return Result<SysPagePermissionEntity>.OkList(list, "查询页面权限目录成功", ResultSource.Database);
        }

        /// <summary>
        /// 获取用户最终有效页面权限。
        /// UseCustomPagePermission=False 时，按角色默认权限生成。
        /// UseCustomPagePermission=True 时，按用户自定义页面权限读取。
        /// </summary>
        public Result<string> GetUserPagePermissions(int userId)
        {
            if (userId <= 0)
            {
                return Result<string>.Fail(-60, "用户标识无效", ResultSource.Unknown);
            }

            var userQuery = _userDb.QueryAll();
            if (!userQuery.Success)
            {
                _reporter?.Error("AuthService", "查询用户失败", userQuery.Code);
                return Result<string>.Fail(userQuery.Code, "查询用户失败", ResultSource.Database);
            }

            var user = userQuery.Items.FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                return Result<string>.Fail(-61, "用户不存在", ResultSource.Unknown);
            }

            var pagePermissionResult = GetPagePermissions();
            if (!pagePermissionResult.Success)
            {
                return Result<string>.Fail(pagePermissionResult.Code, pagePermissionResult.Message, pagePermissionResult.Source);
            }

            if (!user.UseCustomPagePermission)
            {
                var rolesResult = GetRolesByUserId(userId);
                if (!rolesResult.Success)
                {
                    return Result<string>.Fail(rolesResult.Code, rolesResult.Message, rolesResult.Source);
                }

                var roleCodes = new HashSet<string>(
                    rolesResult.Items.Select(x => x.RoleCode),
                    StringComparer.OrdinalIgnoreCase);

                var defaultKeys = pagePermissionResult.Items
                    .Where(x => MatchAnyRole(x.DefaultRoleCodes, roleCodes))
                    .Select(x => x.PageKey)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                return Result<string>.OkList(defaultKeys, "查询用户默认页面权限成功", ResultSource.Database);
            }

            var userPermissionQuery = _userPagePermissionDb.QueryAll();
            if (!userPermissionQuery.Success)
            {
                _reporter?.Error("AuthService", "查询用户自定义页面权限失败", userPermissionQuery.Code);
                return Result<string>.Fail(userPermissionQuery.Code, "查询用户自定义页面权限失败", ResultSource.Database);
            }

            var validKeys = new HashSet<string>(
                pagePermissionResult.Items.Select(x => x.PageKey),
                StringComparer.OrdinalIgnoreCase);

            var customKeys = userPermissionQuery.Items
                .Where(x => x.UserId == userId && !string.IsNullOrWhiteSpace(x.PageKey))
                .Select(x => x.PageKey)
                .Where(validKeys.Contains)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            return Result<string>.OkList(customKeys, "查询用户自定义页面权限成功", ResultSource.Database);
        }

        public Result SaveUserPagePermissions(int userId, IEnumerable<string> pageKeys)
        {
            if (userId <= 0)
            {
                return Result.Fail(-62, "用户标识无效", ResultSource.Unknown);
            }

            var userQuery = _userDb.QueryAll();
            if (!userQuery.Success)
            {
                _reporter?.Error("AuthService", "保存权限时查询用户失败", userQuery.Code);
                return Result.Fail(userQuery.Code, "查询用户失败", ResultSource.Database);
            }

            var user = userQuery.Items.FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                return Result.Fail(-63, "用户不存在", ResultSource.Unknown);
            }

            var pagePermissionResult = GetPagePermissions();
            if (!pagePermissionResult.Success)
            {
                return Result.Fail(pagePermissionResult.Code, pagePermissionResult.Message, pagePermissionResult.Source);
            }

            var validPageKeys = new HashSet<string>(
                pagePermissionResult.Items.Select(x => x.PageKey),
                StringComparer.OrdinalIgnoreCase);

            var targetPageKeys = (pageKeys ?? new string[0])
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Where(validPageKeys.Contains)
                .ToList();

            var oldQuery = _userPagePermissionDb.QueryAll();
            if (!oldQuery.Success)
            {
                _reporter?.Error("AuthService", "查询旧用户页面权限失败", oldQuery.Code);
                return Result.Fail(oldQuery.Code, "查询旧用户页面权限失败", ResultSource.Database);
            }

            var oldItems = oldQuery.Items.Where(x => x.UserId == userId).ToList();
            foreach (var item in oldItems)
            {
                var deleteResult = _userPagePermissionDb.Delete(item);
                if (!deleteResult.Success)
                {
                    _reporter?.Error("AuthService", "删除旧页面权限失败", deleteResult.Code);
                    return Result.Fail(deleteResult.Code, "删除旧页面权限失败", ResultSource.Database);
                }
            }

            foreach (var pageKey in targetPageKeys)
            {
                var addResult = _userPagePermissionDb.Add(new SysUserPagePermissionEntity
                {
                    UserId = userId,
                    PageKey = pageKey,
                    CreateTime = DateTime.Now
                });

                if (!addResult.Success)
                {
                    _reporter?.Error("AuthService", "保存页面权限失败", addResult.Code);
                    return Result.Fail(addResult.Code, "保存页面权限失败", ResultSource.Database);
                }
            }

            user.UseCustomPagePermission = true;
            var editUserResult = _userDb.Edit(user);
            if (!editUserResult.Success)
            {
                _reporter?.Error("AuthService", "更新用户权限模式失败", editUserResult.Code);
                return Result.Fail(editUserResult.Code, "更新用户权限模式失败", ResultSource.Database);
            }

            _reporter?.Info("AuthService", "保存用户页面权限成功：" + user.LoginName);
            return Result.Ok("保存页面权限成功", ResultSource.Database);
        }

        public Result<UserSummary> GetUserSummaries()
        {
            var userQuery = _userDb.QueryAll();
            if (!userQuery.Success)
            {
                _reporter?.Error("AuthService", "查询用户列表失败", userQuery.Code);
                return Result<UserSummary>.Fail(userQuery.Code, "查询用户列表失败", ResultSource.Database);
            }

            var roleQuery = _roleDb.QueryAll();
            if (!roleQuery.Success)
            {
                _reporter?.Error("AuthService", "查询角色列表失败", roleQuery.Code);
                return Result<UserSummary>.Fail(roleQuery.Code, "查询角色列表失败", ResultSource.Database);
            }

            var userRoleQuery = _userRoleDb.QueryAll();
            if (!userRoleQuery.Success)
            {
                _reporter?.Error("AuthService", "查询用户角色关联失败", userRoleQuery.Code);
                return Result<UserSummary>.Fail(userRoleQuery.Code, "查询用户角色关联失败", ResultSource.Database);
            }

            var roleMap = roleQuery.Items.ToDictionary(x => x.Id, x => x);
            var userRoleMap = userRoleQuery.Items
                .GroupBy(x => x.UserId)
                .ToDictionary(x => x.Key, x => x.FirstOrDefault());

            var list = userQuery.Items
                .Select(user =>
                {
                    SysUserRoleEntity userRole;
                    SysRoleEntity role = null;

                    if (userRoleMap.TryGetValue(user.Id, out userRole) && userRole != null)
                    {
                        SysRoleEntity tempRole;
                        if (roleMap.TryGetValue(userRole.RoleId, out tempRole))
                        {
                            role = tempRole;
                        }
                    }

                    return new UserSummary
                    {
                        Id = user.Id,
                        LoginName = user.LoginName,
                        UserName = user.UserName,
                        RoleCode = role == null ? string.Empty : role.RoleCode,
                        RoleName = role == null ? string.Empty : role.RoleName,
                        IsEnabled = user.IsEnabled,
                        LastLoginTime = user.LastLoginTime,
                        Remark = user.Remark
                    };
                })
                .OrderByDescending(x => x.IsEnabled)
                .ThenBy(x => x.LoginName)
                .ToList();

            return Result<UserSummary>.OkList(list, "查询用户摘要成功", ResultSource.Database);
        }

        /// <summary>
        /// 新增用户。
        /// </summary>
        public Result CreateUser(string loginName, string userName, string roleCode, string password, bool isEnabled, string remark)
        {
            if (string.IsNullOrWhiteSpace(loginName))
            {
                return Result.Fail(-20, "登录名不能为空", ResultSource.Unknown);
            }

            if (string.IsNullOrWhiteSpace(userName))
            {
                return Result.Fail(-21, "用户名不能为空", ResultSource.Unknown);
            }

            if (string.IsNullOrWhiteSpace(roleCode))
            {
                return Result.Fail(-22, "角色不能为空", ResultSource.Unknown);
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return Result.Fail(-23, "初始密码不能为空", ResultSource.Unknown);
            }

            var userQuery = _userDb.QueryAll();
            if (!userQuery.Success)
            {
                _reporter?.Error("AuthService", "新增用户时查询用户失败", userQuery.Code);
                return Result.Fail(userQuery.Code, "查询用户失败", ResultSource.Database);
            }

            var exists = userQuery.Items.Any(u => string.Equals(u.LoginName, loginName, StringComparison.OrdinalIgnoreCase));
            if (exists)
            {
                return Result.Fail(-25, "登录名已存在", ResultSource.Unknown);
            }

            var roleQuery = _roleDb.QueryAll();
            if (!roleQuery.Success)
            {
                _reporter?.Error("AuthService", "新增用户时查询角色失败", roleQuery.Code);
                return Result.Fail(roleQuery.Code, "查询角色失败", ResultSource.Database);
            }

            var role = roleQuery.Items.FirstOrDefault(r => string.Equals(r.RoleCode, roleCode, StringComparison.OrdinalIgnoreCase));
            if (role == null)
            {
                return Result.Fail(-26, "角色不存在", ResultSource.Unknown);
            }

            string salt;
            string hash;
            CreatePasswordHash(password, out salt, out hash);

            var user = new SysUserEntity
            {
                LoginName = loginName.Trim(),
                UserName = userName.Trim(),
                PasswordSalt = salt,
                PasswordHash = hash,
                IsEnabled = isEnabled,
                IsAdmin = string.Equals(roleCode, "Am", StringComparison.OrdinalIgnoreCase),
                UseCustomPagePermission = false,
                FailedLoginCount = 0,
                LockoutEndTime = null,
                LastLoginTime = null,
                Remark = string.IsNullOrWhiteSpace(remark) ? null : remark.Trim(),
                CreateTime = DateTime.Now
            };

            var addUserResult = _userDb.Add(user);
            if (!addUserResult.Success)
            {
                _reporter?.Error("AuthService", "新增用户保存失败", addUserResult.Code);
                return Result.Fail(addUserResult.Code, "新增用户保存失败", ResultSource.Database);
            }

            var reloadUserQuery = _userDb.QueryAll();
            if (!reloadUserQuery.Success)
            {
                _reporter?.Error("AuthService", "新增用户后重新查询失败", reloadUserQuery.Code);
                return Result.Fail(reloadUserQuery.Code, "新增成功，但重新查询用户失败", ResultSource.Database);
            }

            var savedUser = reloadUserQuery.Items
                .FirstOrDefault(u => string.Equals(u.LoginName, loginName, StringComparison.OrdinalIgnoreCase));

            if (savedUser == null)
            {
                return Result.Fail(-27, "新增成功，但无法获取用户主键", ResultSource.Database);
            }

            var addUserRoleResult = _userRoleDb.Add(new SysUserRoleEntity
            {
                UserId = savedUser.Id,
                RoleId = role.Id,
                CreateTime = DateTime.Now
            });

            if (!addUserRoleResult.Success)
            {
                _reporter?.Error("AuthService", "新增用户角色关联失败", addUserRoleResult.Code);
                return Result.Fail(addUserRoleResult.Code, "新增用户成功，但角色关联保存失败", ResultSource.Database);
            }

            _reporter?.Info("AuthService", "新增用户成功：" + loginName);
            return Result.Ok("新增用户成功", ResultSource.Database);
        }

        /// <summary>
        /// 更新用户基本信息。
        /// </summary>
        public Result UpdateUser(int userId, string userName, string roleCode, bool isEnabled, string remark)
        {
            if (userId <= 0)
            {
                return Result.Fail(-30, "用户标识无效", ResultSource.Unknown);
            }

            if (string.IsNullOrWhiteSpace(userName))
            {
                return Result.Fail(-31, "用户名不能为空", ResultSource.Unknown);
            }

            if (string.IsNullOrWhiteSpace(roleCode))
            {
                return Result.Fail(-32, "角色不能为空", ResultSource.Unknown);
            }

            var userQuery = _userDb.QueryAll();
            if (!userQuery.Success)
            {
                _reporter?.Error("AuthService", "编辑用户时查询用户失败", userQuery.Code);
                return Result.Fail(userQuery.Code, "查询用户失败", ResultSource.Database);
            }

            var user = userQuery.Items.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return Result.Fail(-33, "用户不存在", ResultSource.Unknown);
            }

            var roleQuery = _roleDb.QueryAll();
            if (!roleQuery.Success)
            {
                _reporter?.Error("AuthService", "编辑用户时查询角色失败", roleQuery.Code);
                return Result.Fail(roleQuery.Code, "查询角色失败", ResultSource.Database);
            }

            var role = roleQuery.Items.FirstOrDefault(r => string.Equals(r.RoleCode, roleCode, StringComparison.OrdinalIgnoreCase));
            if (role == null)
            {
                return Result.Fail(-34, "角色不存在", ResultSource.Unknown);
            }

            var userRoleQuery = _userRoleDb.QueryAll();
            if (!userRoleQuery.Success)
            {
                _reporter?.Error("AuthService", "编辑用户时查询用户角色关联失败", userRoleQuery.Code);
                return Result.Fail(userRoleQuery.Code, "查询用户角色关联失败", ResultSource.Database);
            }

            var userRole = userRoleQuery.Items.FirstOrDefault(x => x.UserId == userId);
            var roleChanged = userRole == null || userRole.RoleId != role.Id;

            user.UserName = userName.Trim();
            user.IsEnabled = isEnabled;
            user.IsAdmin = string.Equals(roleCode, "Am", StringComparison.OrdinalIgnoreCase);
            user.Remark = string.IsNullOrWhiteSpace(remark) ? null : remark.Trim();

            if (roleChanged)
            {
                user.UseCustomPagePermission = false;
            }

            var editUserResult = _userDb.Edit(user);
            if (!editUserResult.Success)
            {
                _reporter?.Error("AuthService", "编辑用户保存失败", editUserResult.Code);
                return Result.Fail(editUserResult.Code, "编辑用户保存失败", ResultSource.Database);
            }

            Result saveUserRoleResult;
            if (userRole == null)
            {
                saveUserRoleResult = _userRoleDb.Add(new SysUserRoleEntity
                {
                    UserId = userId,
                    RoleId = role.Id,
                    CreateTime = DateTime.Now
                });
            }
            else
            {
                userRole.RoleId = role.Id;
                saveUserRoleResult = _userRoleDb.Edit(userRole);
            }

            if (!saveUserRoleResult.Success)
            {
                _reporter?.Error("AuthService", "编辑用户角色关联保存失败", saveUserRoleResult.Code);
                return Result.Fail(saveUserRoleResult.Code, "编辑用户成功，但角色关联保存失败", ResultSource.Database);
            }

            if (roleChanged)
            {
                var clearPermissionResult = ClearUserCustomPagePermissions(userId);
                if (!clearPermissionResult.Success)
                {
                    return clearPermissionResult;
                }
            }

            RefreshCurrentUserContextIfNeeded(userId);

            _reporter?.Info(
                "AuthService",
                roleChanged
                    ? "编辑用户成功，角色已变更并恢复为新角色默认页面权限：" + user.LoginName
                    : "编辑用户成功：" + user.LoginName);

            return Result.Ok("编辑用户成功", ResultSource.Database);
        }

        /// <summary>
        /// 管理员重置指定用户密码。
        /// </summary>
        public Result ResetUserPassword(int userId, string newPassword)
        {
            if (userId <= 0)
            {
                return Result.Fail(-40, "用户标识无效", ResultSource.Unknown);
            }

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                return Result.Fail(-41, "新密码不能为空", ResultSource.Unknown);
            }

            var userQuery = _userDb.QueryAll();
            if (!userQuery.Success)
            {
                _reporter?.Error("AuthService", "重置密码时查询用户失败", userQuery.Code);
                return Result.Fail(userQuery.Code, "查询用户失败", ResultSource.Database);
            }

            var user = userQuery.Items.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return Result.Fail(-43, "用户不存在", ResultSource.Unknown);
            }

            string salt;
            string hash;
            CreatePasswordHash(newPassword, out salt, out hash);

            user.PasswordSalt = salt;
            user.PasswordHash = hash;
            user.FailedLoginCount = 0;
            user.LockoutEndTime = null;

            var editResult = _userDb.Edit(user);
            if (!editResult.Success)
            {
                _reporter?.Error("AuthService", "重置密码保存失败", editResult.Code);
                return Result.Fail(editResult.Code, "重置密码保存失败", ResultSource.Database);
            }

            _reporter?.Info("AuthService", "重置用户密码成功：" + user.LoginName);
            return Result.Ok("重置密码成功", ResultSource.Database);
        }

        /// <summary>
        /// 设置用户启用状态。
        /// </summary>
        public Result SetUserEnabled(int userId, bool isEnabled)
        {
            if (userId <= 0)
            {
                return Result.Fail(-50, "用户标识无效", ResultSource.Unknown);
            }

            var userQuery = _userDb.QueryAll();
            if (!userQuery.Success)
            {
                _reporter?.Error("AuthService", "设置启用状态时查询用户失败", userQuery.Code);
                return Result.Fail(userQuery.Code, "查询用户失败", ResultSource.Database);
            }

            var user = userQuery.Items.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return Result.Fail(-51, "用户不存在", ResultSource.Unknown);
            }

            user.IsEnabled = isEnabled;

            if (!isEnabled)
            {
                user.FailedLoginCount = 0;
                user.LockoutEndTime = null;
            }

            var editResult = _userDb.Edit(user);
            if (!editResult.Success)
            {
                _reporter?.Error("AuthService", "设置用户启用状态失败", editResult.Code);
                return Result.Fail(editResult.Code, "设置用户启用状态失败", ResultSource.Database);
            }

            _reporter?.Info("AuthService", (isEnabled ? "启用用户成功：" : "禁用用户成功：") + user.LoginName);
            return Result.Ok(isEnabled ? "用户已启用" : "用户已禁用", ResultSource.Database);
        }

        private static bool MatchAnyRole(string defaultRoleCodes, HashSet<string> roleCodes)
        {
            if (string.IsNullOrWhiteSpace(defaultRoleCodes) || roleCodes == null || roleCodes.Count == 0)
            {
                return false;
            }

            var parts = defaultRoleCodes
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim());

            return parts.Any(roleCodes.Contains);
        }

        public Result ChangeCurrentUserPassword(string oldPassword, string newPassword, string confirmPassword)
        {
            if (!UserContext.Instance.IsLoggedIn || UserContext.Instance.CurrentUser == null)
            {
                return Result.Fail(-10, "当前没有已登录用户", ResultSource.Unknown);
            }

            if (string.IsNullOrWhiteSpace(oldPassword))
            {
                return Result.Fail(-11, "旧密码不能为空", ResultSource.Unknown);
            }

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                return Result.Fail(-12, "新密码不能为空", ResultSource.Unknown);
            }

            if (string.IsNullOrWhiteSpace(confirmPassword))
            {
                return Result.Fail(-13, "确认密码不能为空", ResultSource.Unknown);
            }

            if (!string.Equals(newPassword, confirmPassword, StringComparison.Ordinal))
            {
                return Result.Fail(-14, "两次输入的新密码不一致", ResultSource.Unknown);
            }

            var loginName = UserContext.Instance.LoginName;
            var userQuery = _userDb.QueryAll();
            if (!userQuery.Success)
            {
                _reporter?.Error("AuthService", "修改密码时查询用户失败", userQuery.Code);
                return Result.Fail(userQuery.Code, "查询用户失败", ResultSource.Database);
            }

            var user = userQuery.Items.FirstOrDefault(u =>
                string.Equals(u.LoginName, loginName, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                return Result.Fail(-16, "当前用户不存在", ResultSource.Unknown);
            }

            if (!VerifyPassword(oldPassword, user.PasswordSalt, user.PasswordHash))
            {
                return Result.Fail(-17, "旧密码错误", ResultSource.Unknown);
            }

            string salt;
            string hash;
            CreatePasswordHash(newPassword, out salt, out hash);

            user.PasswordSalt = salt;
            user.PasswordHash = hash;

            var editResult = _userDb.Edit(user);
            if (!editResult.Success)
            {
                _reporter?.Error("AuthService", "修改密码保存失败", editResult.Code);
                return Result.Fail(editResult.Code, "修改密码保存失败", ResultSource.Database);
            }

            if (UserContext.Instance.CurrentUser != null)
            {
                UserContext.Instance.CurrentUser.PasswordSalt = salt;
                UserContext.Instance.CurrentUser.PasswordHash = hash;
            }

            _reporter?.Info("AuthService", "当前用户修改密码成功");
            return Result.Ok("密码修改成功", ResultSource.Unknown);
        }

        public Result<LoginLogSummary> GetLoginLogSummaries(
            string loginNameKeyword,
            bool? isSuccess,
            DateTime? startDate,
            DateTime? endDate,
            int pageIndex,
            int pageSize,
            out int totalCount,
            out int successCount,
            out int failedCount)
        {
            totalCount = 0;
            successCount = 0;
            failedCount = 0;

            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }

            if (pageSize <= 0)
            {
                pageSize = 100;
            }

            try
            {
                totalCount = BuildLoginLogQuery(loginNameKeyword, isSuccess, startDate, endDate).Count();
                successCount = BuildLoginLogQuery(loginNameKeyword, isSuccess, startDate, endDate)
                    .Where(x => x.IsSuccess)
                    .Count();
                failedCount = BuildLoginLogQuery(loginNameKeyword, isSuccess, startDate, endDate)
                    .Where(x => !x.IsSuccess)
                    .Count();

                var pageItems = BuildLoginLogQuery(loginNameKeyword, isSuccess, startDate, endDate)
                    .OrderBy(x => x.LoginTime, OrderByType.Desc)
                    .ToPageList(pageIndex, pageSize)
                    .Select(x => new LoginLogSummary
                    {
                        Id = x.Id,
                        UserId = x.UserId,
                        LoginName = x.LoginName,
                        IsSuccess = x.IsSuccess,
                        Message = x.Message,
                        LoginTime = x.LoginTime,
                        AppVersion = x.AppVersion
                    })
                    .ToList();

                return Result<LoginLogSummary>.OkList(pageItems, "查询登录日志成功", ResultSource.Database);
            }
            catch (Exception ex)
            {
                _reporter?.Error("AuthService", ex, "查询登录日志失败");
                return Result<LoginLogSummary>.Fail(-70, "查询登录日志失败", ResultSource.Database);
            }
        }

        private ISugarQueryable<SysLoginLogEntity> BuildLoginLogQuery(
            string loginNameKeyword,
            bool? isSuccess,
            DateTime? startDate,
            DateTime? endDate)
        {
            var query = _loginLogDb._sqlSugarClient.Queryable<SysLoginLogEntity>();

            if (!string.IsNullOrWhiteSpace(loginNameKeyword))
            {
                var keyword = loginNameKeyword.Trim();
                query = query.Where(x => x.LoginName.Contains(keyword));
            }

            if (isSuccess.HasValue)
            {
                query = query.Where(x => x.IsSuccess == isSuccess.Value);
            }

            if (startDate.HasValue)
            {
                var begin = startDate.Value.Date;
                query = query.Where(x => x.LoginTime >= begin);
            }

            if (endDate.HasValue)
            {
                var end = endDate.Value.Date.AddDays(1);
                query = query.Where(x => x.LoginTime < end);
            }

            return query;
        }

        private Result ClearUserCustomPagePermissions(int userId)
        {
            try
            {
                _userPagePermissionDb._sqlSugarClient.Deleteable<SysUserPagePermissionEntity>()
                    .Where(x => x.UserId == userId)
                    .ExecuteCommand();

                return Result.Ok("清除用户自定义页面权限成功", ResultSource.Database);
            }
            catch (Exception ex)
            {
                _reporter?.Error("AuthService", ex, "清除用户自定义页面权限失败");
                return Result.Fail(-84, "清除用户自定义页面权限失败", ResultSource.Database);
            }
        }

        private void RefreshCurrentUserContextIfNeeded(int userId)
        {
            if (!UserContext.Instance.IsLoggedIn ||
                UserContext.Instance.CurrentUser == null ||
                UserContext.Instance.CurrentUser.Id != userId)
            {
                return;
            }

            var userQuery = _userDb.QueryAll();
            if (!userQuery.Success)
            {
                return;
            }

            var currentUser = userQuery.Items.FirstOrDefault(x => x.Id == userId);
            if (currentUser == null)
            {
                return;
            }

            var roleResult = GetRolesByUserId(userId);
            if (!roleResult.Success)
            {
                return;
            }

            var permissionResult = GetUserPagePermissions(userId);
            if (!permissionResult.Success)
            {
                return;
            }

            UserContext.Instance.SignIn(currentUser, roleResult.Items, permissionResult.Items);
        }

        public Result RestoreDefaultPagePermissions(int userId)
        {
            if (userId <= 0)
            {
                return Result.Fail(-71, "用户标识无效", ResultSource.Unknown);
            }

            var userQuery = _userDb.QueryAll();
            if (!userQuery.Success)
            {
                _reporter?.Error("AuthService", "恢复默认权限时查询用户失败", userQuery.Code);
                return Result.Fail(userQuery.Code, "查询用户失败", ResultSource.Database);
            }

            var user = userQuery.Items.FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                return Result.Fail(-72, "用户不存在", ResultSource.Unknown);
            }

            try
            {
                _userPagePermissionDb._sqlSugarClient.Deleteable<SysUserPagePermissionEntity>()
                    .Where(x => x.UserId == userId)
                    .ExecuteCommand();

                user.UseCustomPagePermission = false;
                var editResult = _userDb.Edit(user);
                if (!editResult.Success)
                {
                    _reporter?.Error("AuthService", "更新用户权限模式失败", editResult.Code);
                    return Result.Fail(editResult.Code, "更新用户权限模式失败", ResultSource.Database);
                }

                _reporter?.Info("AuthService", "恢复默认页面权限成功：" + user.LoginName);
                return Result.Ok("已恢复角色默认页面权限", ResultSource.Database);
            }
            catch (Exception ex)
            {
                _reporter?.Error("AuthService", ex, "恢复默认页面权限失败");
                return Result.Fail(-73, "恢复默认页面权限失败", ResultSource.Database);
            }
        }

        public static void CreatePasswordHash(string password, out string salt, out string hash)
        {
            if (password == null)
            {
                password = string.Empty;
            }

            var saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }

            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, PasswordIterations))
            {
                var hashBytes = deriveBytes.GetBytes(PasswordByteSize);
                salt = Convert.ToBase64String(saltBytes);
                hash = Convert.ToBase64String(hashBytes);
            }
        }

        private static bool VerifyPassword(string password, string salt, string hash)
        {
            if (string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(salt) ||
                string.IsNullOrWhiteSpace(hash))
            {
                return false;
            }

            byte[] saltBytes;
            byte[] expectedHashBytes;

            try
            {
                saltBytes = Convert.FromBase64String(salt);
                expectedHashBytes = Convert.FromBase64String(hash);
            }
            catch
            {
                return false;
            }

            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, PasswordIterations))
            {
                var actualHashBytes = deriveBytes.GetBytes(PasswordByteSize);
                return actualHashBytes.SequenceEqual(expectedHashBytes);
            }
        }

        private void SaveLoginLog(int? userId, string loginName, bool isSuccess, string message, string clientInfo)
        {
            var entity = new SysLoginLogEntity
            {
                UserId = userId,
                LoginName = loginName ?? string.Empty,
                IsSuccess = isSuccess,
                Message = message ?? string.Empty,
                LoginTime = DateTime.Now,
                AppVersion = GetApplicationVersion()
            };

            var result = _loginLogDb.Add(entity);
            if (!result.Success)
            {
                _reporter?.Warn("AuthService", "登录日志写入失败", result.Code);
            }
        }

        private static string GetApplicationVersion()
        {
            try
            {
                var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
                var version = assembly.GetName().Version;
                if (version == null)
                {
                    return "-";
                }

                return version.ToString();
            }
            catch
            {
                return "-";
            }
        }
    }
}