using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Model.Entity
{
    /// <summary>
    /// 轴参数配置实体类
    /// </summary>
    [SugarTable("configaxisarg")]
    public partial class ConfigAxisArg
    {
        public ConfigAxisArg()
        {
            // 设置默认值
            this.ParamSetVal = 0;
            this.ParamDefaultVal = 0;
            this.ParamMaxVal = 0;
            this.ParamMinVal = 0;
            this.ParamStatus1 = 0;
            this.ParamStatus2 = 0;
            this.ParamStatus3 = 0;
        }

        /// <summary>
        /// Desc: 自增主键 ID
        /// </summary>           
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// Desc: 轴编号
        /// </summary>           
        public int Axis { get; set; }

        /// <summary>
        /// Desc: 参数英文名称
        /// </summary>           
        public string ParamName { get; set; }

        /// <summary>
        /// Desc: 参数中文名称
        /// </summary>           
        public string ParamName_Cn { get; set; }

        /// <summary>
        /// Desc: 参数当前设置值
        /// </summary>           
        public int ParamSetVal { get; set; }

        /// <summary>
        /// Desc: 参数默认值
        /// </summary>           
        public int ParamDefaultVal { get; set; }

        /// <summary>
        /// Desc: 参数最大值
        /// </summary>           
        public int ParamMaxVal { get; set; }

        /// <summary>
        /// Desc: 参数最小值
        /// </summary>           
        public int ParamMinVal { get; set; }

        /// <summary>
        /// Desc: 参数状态1 (布尔开关或状态码)
        /// </summary>           
        public int ParamStatus1 { get; set; }

        /// <summary>
        /// Desc: 参数状态2
        /// </summary>           
        public int ParamStatus2 { get; set; }

        /// <summary>
        /// Desc: 参数状态3
        /// </summary>           
        public int ParamStatus3 { get; set; }

        /// <summary>
        /// Desc: 描述
        /// </summary>           
        [SugarColumn(IsNullable = true)]
        public string Description { get; set; }

        /// <summary>
        /// Desc: 备注
        /// </summary>           
        [SugarColumn(IsNullable = true)]
        public string Remark { get; set; }

        /// <summary>
        /// Desc: 轴中文名称
        /// </summary>           
        [SugarColumn(IsNullable = true)]
        public string Axis_Cn { get; set; }

        /// <summary>
        /// Desc: 保留字段1-5
        /// </summary>           
        [SugarColumn(IsNullable = true)]
        public string Reserve1 { get; set; }

        [SugarColumn(IsNullable = true)]
        public string Reserve2 { get; set; }

        [SugarColumn(IsNullable = true)]
        public string Reserve3 { get; set; }

        [SugarColumn(IsNullable = true)]
        public string Reserve4 { get; set; }

        [SugarColumn(IsNullable = true)]
        public string Reserve5 { get; set; }
    }

}
