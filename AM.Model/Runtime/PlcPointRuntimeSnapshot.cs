using System;

namespace AM.Model.Runtime
{
    /// <summary>
    /// PLC 点位运行时快照。
    /// 当前版本仅保留最小运行态字段，不再保留 AreaType。
    /// </summary>
    public class PlcPointRuntimeSnapshot
    {
        /// <summary>
        /// 所属 PLC 名称。
        /// </summary>
        public string PlcName { get; set; }

        /// <summary>
        /// 点位名称。
        /// </summary>
        public string PointName { get; set; }

        /// <summary>
        /// 点位显示名称。
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 点位分组名称。
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 点位地址显示文本。
        /// 当前直接使用完整协议地址。
        /// </summary>
        public string AddressText { get; set; }

        /// <summary>
        /// 点位数据类型。
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 点位显示值。
        /// </summary>
        public string ValueText { get; set; }

        /// <summary>
        /// 点位原始值文本。
        /// </summary>
        public string RawValue { get; set; }

        /// <summary>
        /// 质量标记。
        /// 例如：Good / Disconnected / Error。
        /// </summary>
        public string Quality { get; set; }

        /// <summary>
        /// 最近更新时间。
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 当前是否已连接。
        /// </summary>
        public bool IsConnected { get; set; }

        /// <summary>
        /// 当前是否处于错误状态。
        /// </summary>
        public bool HasError { get; set; }

        /// <summary>
        /// 错误信息。
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 点位显示标题。
        /// </summary>
        public string DisplayTitle
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(DisplayName))
                {
                    return DisplayName;
                }

                return string.IsNullOrWhiteSpace(PointName) ? "未命名点位" : PointName;
            }
        }

        /// <summary>
        /// 创建当前快照的副本。
        /// </summary>
        public PlcPointRuntimeSnapshot Clone()
        {
            return new PlcPointRuntimeSnapshot
            {
                PlcName = PlcName,
                PointName = PointName,
                DisplayName = DisplayName,
                GroupName = GroupName,
                AddressText = AddressText,
                DataType = DataType,
                ValueText = ValueText,
                RawValue = RawValue,
                Quality = Quality,
                UpdateTime = UpdateTime,
                IsConnected = IsConnected,
                HasError = HasError,
                ErrorMessage = ErrorMessage
            };
        }
    }
}