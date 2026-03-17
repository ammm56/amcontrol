using SqlSugar;
using System;

namespace AM.Model.Entity.Auth
{
    /// <summary>
    /// 登录日志。
    /// </summary>
    [SugarTable("sys_login_log")]
    public class SysLoginLog
    {
        /// <summary>
        /// 自增主键。
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 用户 ID。
        /// 登录失败时可为空。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? UserId { get; set; }

        /// <summary>
        /// 登录名。
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 是否成功。
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 登录结果消息。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Message { get; set; }

        /// <summary>
        /// 登录时间。
        /// </summary>
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// 客户端信息。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string ClientInfo { get; set; }
    }
}