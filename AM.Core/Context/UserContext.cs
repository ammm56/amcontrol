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
            CurrentRoles = new List<SysRole>();
        }

        /// <summary>
        /// 当前登录用户。
        /// </summary>
        public SysUser CurrentUser { get; private set; }

        /// <summary>
        /// 当前用户角色集合。
        /// </summary>
        public IReadOnlyList<SysRole> CurrentRoles { get; private set; }

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
        /// 登录写入上下文。
        /// </summary>
        public void SignIn(SysUser user, IEnumerable<SysRole> roles)
        {
            CurrentUser = user;
            CurrentRoles = roles == null ? new List<SysRole>() : roles.ToList();
        }

        /// <summary>
        /// 退出登录并清空上下文。
        /// </summary>
        public void SignOut()
        {
            CurrentUser = null;
            CurrentRoles = new List<SysRole>();
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
    }
}