using System;

namespace AM.Model.Runtime
{
    /// <summary>
    /// PLC 点位运行时快照。
    /// </summary>
    public class PlcPointRuntimeSnapshot
    {
        public string PlcName { get; set; }

        public string PointName { get; set; }

        public string DisplayName { get; set; }

        public string GroupName { get; set; }

        public string AddressText { get; set; }

        public string AreaType { get; set; }

        public string DataType { get; set; }

        public string ValueText { get; set; }

        public string RawValue { get; set; }

        public string Quality { get; set; }

        public DateTime UpdateTime { get; set; }

        public bool IsConnected { get; set; }

        public bool HasError { get; set; }

        public string ErrorMessage { get; set; }

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

        public PlcPointRuntimeSnapshot Clone()
        {
            return new PlcPointRuntimeSnapshot
            {
                PlcName = PlcName,
                PointName = PointName,
                DisplayName = DisplayName,
                GroupName = GroupName,
                AddressText = AddressText,
                AreaType = AreaType,
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