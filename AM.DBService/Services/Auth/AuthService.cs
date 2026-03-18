using AM.Core.Context;
using AM.Core.Reporter;
using AM.DBService.DBase;
using AM.Model.Auth;
using AM.Model.Common;
using AM.Model.Entity.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace AM.DBService.Services.Auth
{
    /// <summary>
    /// 认证服务。
    /// 第一版采用 用户/角色/用户角色 模型完成登录闭环。
    /// </summary>
    public class AuthService
    {
        private const int PasswordIterations = 10000;
        private const int PasswordByteSize = 32;

        private readonly DBCommon<SysUser> _userDb;
        private readonly DBCommon<SysRole> _roleDb;
        private readonly DBCommon<SysUserRole> _userRoleDb;
        private readonly DBCommon<SysLoginLog> _loginLogDb;
        private readonly IAppReporter _reporter;

        public AuthService() : this(SystemContext.Instance.Reporter)
        {
        }

        public AuthService(IAppReporter reporter)
        {
            _userDb = new DBCommon<SysUser>();
            _roleDb = new DBCommon<SysRole>();
            _userRoleDb = new DBCommon<SysUserRole>();
            _loginLogDb = new DBCommon<SysLoginLog>();
            _reporter = reporter;
        }

        /// <summary>
        /// 登录。
        /// </summary>
        public Result<SysUser> Login(string loginName, string password, string clientInfo = null)
        {
            if (string.IsNullOrWhiteSpace(loginName))
            {
                return Result<SysUser>.Fail(-1, "登录名不能为空", ResultSource.Unknown);
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return Result<SysUser>.Fail(-1, "密码不能为空", ResultSource.Unknown);
            }

            var userQuery = _userDb.QueryAll();
            if (!userQuery.Success)
            {
                _reporter?.Error("AuthService", "查询用户失败", userQuery.Code);
                return Result<SysUser>.Fail(userQuery.Code, "查询用户失败", ResultSource.Database);
            }

            var user = userQuery.Items.FirstOrDefault(u =>
                string.Equals(u.LoginName, loginName, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                SaveLoginLog(null, loginName, false, "用户不存在", clientInfo);
                _reporter?.Warn("AuthService", "登录失败：用户不存在");
                return Result<SysUser>.Fail(-2, "用户不存在", ResultSource.Unknown);
            }

            if (!user.IsEnabled)
            {
                SaveLoginLog(user.Id, loginName, false, "用户已禁用", clientInfo);
                _reporter?.Warn("AuthService", "登录失败：用户已禁用");
                return Result<SysUser>.Fail(-3, "用户已禁用", ResultSource.Unknown);
            }

            if (!VerifyPassword(password, user.PasswordSalt, user.PasswordHash))
            {
                SaveLoginLog(user.Id, loginName, false, "密码错误", clientInfo);
                _reporter?.Warn("AuthService", "登录失败：密码错误");
                return Result<SysUser>.Fail(-4, "密码错误", ResultSource.Unknown);
            }

            var rolesResult = GetRolesByUserId(user.Id);
            if (!rolesResult.Success)
            {
                SaveLoginLog(user.Id, loginName, false, "查询角色失败", clientInfo);
                return Result<SysUser>.Fail(rolesResult.Code, "查询角色失败", ResultSource.Database);
            }

            user.LastLoginTime = DateTime.Now;
            var editResult = _userDb.Edit(user);
            if (!editResult.Success)
            {
                _reporter?.Warn("AuthService", "更新最后登录时间失败", editResult.Code);
            }

            UserContext.Instance.SignIn(user, rolesResult.Items);

            SaveLoginLog(user.Id, loginName, true, "登录成功", clientInfo);
            _reporter?.Info("AuthService", "登录成功");

            return Result<SysUser>.OkItem(user, "登录成功", ResultSource.Unknown);
        }

        /// <summary>
        /// 退出登录。
        /// </summary>
        public Result Logout()
        {
            var loginName = UserContext.Instance.LoginName;
            UserContext.Instance.SignOut();
            _reporter?.Info("AuthService", "退出登录：" + loginName);
            return Result.Ok("退出成功", ResultSource.Unknown);
        }

        /// <summary>
        /// 获取指定用户角色。
        /// </summary>
        public Result<SysRole> GetRolesByUserId(int userId)
        {
            var userRoleQuery = _userRoleDb.QueryAll();
            if (!userRoleQuery.Success)
            {
                _reporter?.Error("AuthService", "查询用户角色关联失败", userRoleQuery.Code);
                return Result<SysRole>.Fail(userRoleQuery.Code, "查询用户角色关联失败", ResultSource.Database);
            }

            var roleQuery = _roleDb.QueryAll();
            if (!roleQuery.Success)
            {
                _reporter?.Error("AuthService", "查询角色失败", roleQuery.Code);
                return Result<SysRole>.Fail(roleQuery.Code, "查询角色失败", ResultSource.Database);
            }

            var roleIds = userRoleQuery.Items
                .Where(x => x.UserId == userId)
                .Select(x => x.RoleId)
                .Distinct()
                .ToList();

            var roles = roleQuery.Items
                .Where(r => roleIds.Contains(r.Id))
                .ToList();

            return Result<SysRole>.OkList(roles, "查询角色成功", ResultSource.Database);
        }

        /// <summary>
        /// 获得用户列表，用户管理页用。
        /// </summary>
        /// <returns></returns>
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
                    SysUserRole userRole;
                    SysRole role = null;

                    if (userRoleMap.TryGetValue(user.Id, out userRole) && userRole != null)
                    {
                        SysRole tempRole;
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

            if (password.Length < 6)
            {
                return Result.Fail(-24, "初始密码长度不能少于 6 位", ResultSource.Unknown);
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

            var user = new SysUser
            {
                LoginName = loginName.Trim(),
                UserName = userName.Trim(),
                PasswordSalt = salt,
                PasswordHash = hash,
                IsEnabled = isEnabled,
                IsAdmin = string.Equals(roleCode, "Am", StringComparison.OrdinalIgnoreCase),
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

            var addUserRoleResult = _userRoleDb.Add(new SysUserRole
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

            user.UserName = userName.Trim();
            user.IsEnabled = isEnabled;
            user.IsAdmin = string.Equals(roleCode, "Am", StringComparison.OrdinalIgnoreCase);
            user.Remark = string.IsNullOrWhiteSpace(remark) ? null : remark.Trim();

            var editUserResult = _userDb.Edit(user);
            if (!editUserResult.Success)
            {
                _reporter?.Error("AuthService", "编辑用户保存失败", editUserResult.Code);
                return Result.Fail(editUserResult.Code, "编辑用户保存失败", ResultSource.Database);
            }

            var userRoleQuery = _userRoleDb.QueryAll();
            if (!userRoleQuery.Success)
            {
                _reporter?.Error("AuthService", "编辑用户时查询用户角色关联失败", userRoleQuery.Code);
                return Result.Fail(userRoleQuery.Code, "查询用户角色关联失败", ResultSource.Database);
            }

            var userRole = userRoleQuery.Items.FirstOrDefault(x => x.UserId == userId);
            Result saveUserRoleResult;

            if (userRole == null)
            {
                saveUserRoleResult = _userRoleDb.Add(new SysUserRole
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

            _reporter?.Info("AuthService", "编辑用户成功：" + user.LoginName);
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

            if (newPassword.Length < 6)
            {
                return Result.Fail(-42, "新密码长度不能少于 6 位", ResultSource.Unknown);
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
        /// 修改当前登录用户的密码。
        /// </summary>
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

            if (newPassword.Length < 6)
            {
                return Result.Fail(-15, "新密码长度不能少于 6 位", ResultSource.Unknown);
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

        /// <summary>
        /// 创建密码哈希。
        /// 用于初始化管理员账号或新增用户。
        /// </summary>
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
            var entity = new SysLoginLog
            {
                UserId = userId,
                LoginName = loginName ?? string.Empty,
                IsSuccess = isSuccess,
                Message = message ?? string.Empty,
                LoginTime = DateTime.Now,
                ClientInfo = clientInfo
            };

            var result = _loginLogDb.Add(entity);
            if (!result.Success)
            {
                _reporter?.Warn("AuthService", "登录日志写入失败", result.Code);
            }
        }
    }
}