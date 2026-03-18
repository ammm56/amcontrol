using SqlSugar;
using System;

namespace AM.Model.Entity.Auth
{
    /// <summary>
    /// 页面权限目录。
    /// </summary>
    [SugarTable("sys_page_permission")]
    public class SysPagePermission
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        public string ModuleKey { get; set; }

        public string ModuleName { get; set; }

        public string PageKey { get; set; }

        public string DisplayName { get; set; }

        [SugarColumn(IsNullable = true)]
        public string Description { get; set; }

        /// <summary>
        /// 默认角色编码，逗号分隔。
        /// 例如：Operator,Engineer,Am
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string DefaultRoleCodes { get; set; }

        [SugarColumn(IsNullable = true)]
        public string RecommendedRoles { get; set; }

        [SugarColumn(IsNullable = true)]
        public string RiskLevel { get; set; }

        public int SortOrder { get; set; }

        public bool IsEnabled { get; set; }

        public DateTime CreateTime { get; set; }
    }
}