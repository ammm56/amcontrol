using SqlSugar;
using System;

namespace AM.Model.Entity.Auth
{
    /// <summary>
    /// 系统用户。
    /// </summary>
    [SugarTable("sys_user")]
    public class SysUserEntity
    {
        /// <summary>
        /// 自增主键。
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 登录名。
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 用户显示名。
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码哈希。
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// 密码盐。
        /// </summary>
        public string PasswordSalt { get; set; }

        /// <summary>
        /// 是否启用。
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 是否管理员。
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 是否启用自定义页面权限。
        /// False：按角色默认权限。
        /// True：按 sys_user_page_permission 作为最终页面权限。
        /// </summary>
        public bool UseCustomPagePermission { get; set; }

        /// <summary>
        /// 连续登录失败次数。
        /// </summary>
        public int FailedLoginCount { get; set; }

        /// <summary>
        /// 锁定截止时间。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? LockoutEndTime { get; set; }

        /// <summary>
        /// 最后登录时间。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 备注。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间。
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}