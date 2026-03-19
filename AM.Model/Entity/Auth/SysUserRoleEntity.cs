using SqlSugar;
using System;

namespace AM.Model.Entity.Auth
{
    /// <summary>
    /// 用户角色关联。
    /// </summary>
    [SugarTable("sys_user_role")]
    public class SysUserRoleEntity
    {
        /// <summary>
        /// 自增主键。
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 用户 ID。
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 角色 ID。
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 创建时间。
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}