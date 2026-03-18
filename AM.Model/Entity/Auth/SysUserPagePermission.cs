using SqlSugar;
using System;

namespace AM.Model.Entity.Auth
{
    /// <summary>
    /// 用户页面权限。
    /// </summary>
    [SugarTable("sys_user_page_permission")]
    public class SysUserPagePermission
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        public int UserId { get; set; }

        public string PageKey { get; set; }

        public DateTime CreateTime { get; set; }
    }
}