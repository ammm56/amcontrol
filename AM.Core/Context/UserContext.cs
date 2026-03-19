using AM.Model.Entity.Auth;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AM.Core.Context
{
    /// <summary>
    /// 当前登录用户上下文。
    /// </summary>
    public sealed class UserContext
    {
        /// <summary>
        /// 全局唯一实例。
        /// </summary>
        public static UserContext Instance { get; } = new UserContext();

        /// <summary>
        /// 私有构造。
        /// </summary>
        private UserContext()
        {
            CurrentRoles = new List<SysRoleEntity>();
            CurrentPageKeys = new List<string>();
        }

        /// <summary>
        /// 当前登录用户。
        /// </summary>
        public SysUserEntity CurrentUser { get; private set; }

        /// <summary>
        /// 当前用户角色集合。
        /// </summary>
        public IReadOnlyList<SysRoleEntity> CurrentRoles { get; private set; }

        /// <summary>
        /// 当前用户页面权限标识集合。
        /// </summary>
        public IReadOnlyList<string> CurrentPageKeys { get; private set; }

        /// <summary>
        /// 是否已登录。
        /// </summary>
        public bool IsLoggedIn
        {
            get { return CurrentUser != null; }
        }

        /// <summary>
        /// 当前登录名。
        /// </summary>
        public string LoginName
        {
            get { return CurrentUser == null ? string.Empty : CurrentUser.LoginName; }
        }

        /// <summary>
        /// 当前显示名。
        /// </summary>
        public string UserName
        {
            get { return CurrentUser == null ? string.Empty : CurrentUser.UserName; }
        }

        /// <summary>
        /// 是否管理员。
        /// </summary>
        public bool IsAdmin
        {
            get { return CurrentUser != null && CurrentUser.IsAdmin; }
        }

        /// <summary>
        /// 是否使用自定义页面权限。
        /// </summary>
        public bool UseCustomPagePermission
        {
            get { return CurrentUser != null && CurrentUser.UseCustomPagePermission; }
        }

        /// <summary>
        /// 登录写入上下文。
        /// </summary>
        public void SignIn(SysUserEntity user, IEnumerable<SysRoleEntity> roles, IEnumerable<string> pageKeys)
        {
            CurrentUser = user;
            CurrentRoles = roles == null ? new List<SysRoleEntity>() : roles.ToList();
            CurrentPageKeys = pageKeys == null
                ? new List<string>()
                : pageKeys
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();
        }

        /// <summary>
        /// 退出登录并清空上下文。
        /// </summary>
        public void SignOut()
        {
            CurrentUser = null;
            CurrentRoles = new List<SysRoleEntity>();
            CurrentPageKeys = new List<string>();
        }

        /// <summary>
        /// 是否拥有指定角色编码。
        /// </summary>
        public bool HasRole(string roleCode)
        {
            if (string.IsNullOrWhiteSpace(roleCode) || CurrentRoles == null)
            {
                return false;
            }

            return CurrentRoles.Any(r => string.Equals(r.RoleCode, roleCode, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 是否拥有指定页面权限。
        /// </summary>
        public bool HasPagePermission(string pageKey)
        {
            if (string.IsNullOrWhiteSpace(pageKey) || CurrentPageKeys == null)
            {
                return false;
            }

            return CurrentPageKeys.Any(x => string.Equals(x, pageKey, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 刷新页面权限
        /// </summary>
        public void RefreshPagePermissions(IEnumerable<string> pageKeys, bool useCustomPagePermission)
        {
            CurrentPageKeys = pageKeys == null
                ? new List<string>()
                : pageKeys
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

            if (CurrentUser != null)
            {
                CurrentUser.UseCustomPagePermission = useCustomPagePermission;
            }
        }
    }
}