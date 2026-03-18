using AM.Core.Context;
using AM.Core.Reporter;
using AM.DBService.DBase;
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