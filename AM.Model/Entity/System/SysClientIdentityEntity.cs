using SqlSugar;
using System;

namespace AM.Model.Entity.System
{
    /// <summary>
    /// 客户端本地身份信息表。
    /// 用于保存软件实例级标识，不涉及生产与工艺数据。
    /// </summary>
    [SugarTable("sys_client_identity")]
    public class SysClientIdentityEntity
    {
        /// <summary>
        /// 自增主键。
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 客户端唯一标识。
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 应用编码。
        /// </summary>
        public string AppCode { get; set; }

        /// <summary>
        /// 设备编码。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string MachineCode { get; set; }

        /// <summary>
        /// 设备名称。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string MachineName { get; set; }

        /// <summary>
        /// 创建时间。
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间。
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}