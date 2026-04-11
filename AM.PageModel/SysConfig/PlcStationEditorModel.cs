using AM.Model.Common;
using AM.Model.Entity.Plc;
using AM.PageModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AM.PageModel.SysConfig
{
    /// <summary>
    /// PLC 站编辑模型。
    /// 负责页面/对话框编辑态字段承载、默认值填充、输入校验与实体构建。
    /// </summary>
    public class PlcStationEditorModel : BindableBase
    {
        private int _id;
        private string _name;
        private string _displayName;
        private string _vendor;
        private string _model;
        private string _connectionType;
        private string _protocolType;
        private string _ipAddress;
        private string _portText;
        private string _comPort;
        private string _baudRateText;
        private string _dataBitsText;
        private string _parity;
        private string _stopBits;
        private string _stationNoText;
        private string _networkNoText;
        private string _pcNoText;
        private string _rackText;
        private string _slotText;
        private string _timeoutMsText;
        private string _reconnectIntervalMsText;
        private string _scanIntervalMsText;
        private bool _isEnabled;
        private string _sortOrderText;
        private string _description;
        private string _remark;

        public PlcStationEditorModel()
        {
            ResetForCreate();
        }

        /// <summary>
        /// 连接方式建议值。
        /// </summary>
        public static IReadOnlyList<string> ConnectionTypes
        {
            get
            {
                return new[]
                {
                    "Tcp",
                    "Serial",
                    "Virtual"
                };
            }
        }

        /// <summary>
        /// 协议类型建议值。
        /// </summary>
        public static IReadOnlyList<string> ProtocolTypes
        {
            get
            {
                return new[]
                {
                    "ModbusTcp",
                    "ModbusRtu",
                    "S7Tcp"
                };
            }
        }

        /// <summary>
        /// 串口校验位建议值。
        /// </summary>
        public static IReadOnlyList<string> ParityOptions
        {
            get
            {
                return new[]
                {
                    "None",
                    "Odd",
                    "Even"
                };
            }
        }

        /// <summary>
        /// 串口停止位建议值。
        /// </summary>
        public static IReadOnlyList<string> StopBitsOptions
        {
            get
            {
                return new[]
                {
                    "1",
                    "1.5",
                    "2"
                };
            }
        }

        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value ?? string.Empty); }
        }

        public string DisplayName
        {
            get { return _displayName; }
            set { SetProperty(ref _displayName, value ?? string.Empty); }
        }

        public string Vendor
        {
            get { return _vendor; }
            set { SetProperty(ref _vendor, value ?? string.Empty); }
        }

        public string Model
        {
            get { return _model; }
            set { SetProperty(ref _model, value ?? string.Empty); }
        }

        public string ConnectionType
        {
            get { return _connectionType; }
            set { SetProperty(ref _connectionType, value ?? string.Empty); }
        }

        public string ProtocolType
        {
            get { return _protocolType; }
            set { SetProperty(ref _protocolType, value ?? string.Empty); }
        }

        public string IpAddress
        {
            get { return _ipAddress; }
            set { SetProperty(ref _ipAddress, value ?? string.Empty); }
        }

        public string PortText
        {
            get { return _portText; }
            set { SetProperty(ref _portText, value ?? string.Empty); }
        }

        public string ComPort
        {
            get { return _comPort; }
            set { SetProperty(ref _comPort, value ?? string.Empty); }
        }

        public string BaudRateText
        {
            get { return _baudRateText; }
            set { SetProperty(ref _baudRateText, value ?? string.Empty); }
        }

        public string DataBitsText
        {
            get { return _dataBitsText; }
            set { SetProperty(ref _dataBitsText, value ?? string.Empty); }
        }

        public string Parity
        {
            get { return _parity; }
            set { SetProperty(ref _parity, value ?? string.Empty); }
        }

        public string StopBits
        {
            get { return _stopBits; }
            set { SetProperty(ref _stopBits, value ?? string.Empty); }
        }

        public string StationNoText
        {
            get { return _stationNoText; }
            set { SetProperty(ref _stationNoText, value ?? string.Empty); }
        }

        public string NetworkNoText
        {
            get { return _networkNoText; }
            set { SetProperty(ref _networkNoText, value ?? string.Empty); }
        }

        public string PcNoText
        {
            get { return _pcNoText; }
            set { SetProperty(ref _pcNoText, value ?? string.Empty); }
        }

        public string RackText
        {
            get { return _rackText; }
            set { SetProperty(ref _rackText, value ?? string.Empty); }
        }

        public string SlotText
        {
            get { return _slotText; }
            set { SetProperty(ref _slotText, value ?? string.Empty); }
        }

        public string TimeoutMsText
        {
            get { return _timeoutMsText; }
            set { SetProperty(ref _timeoutMsText, value ?? string.Empty); }
        }

        public string ReconnectIntervalMsText
        {
            get { return _reconnectIntervalMsText; }
            set { SetProperty(ref _reconnectIntervalMsText, value ?? string.Empty); }
        }

        public string ScanIntervalMsText
        {
            get { return _scanIntervalMsText; }
            set { SetProperty(ref _scanIntervalMsText, value ?? string.Empty); }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { SetProperty(ref _isEnabled, value); }
        }

        public string SortOrderText
        {
            get { return _sortOrderText; }
            set { SetProperty(ref _sortOrderText, value ?? string.Empty); }
        }

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value ?? string.Empty); }
        }

        public string Remark
        {
            get { return _remark; }
            set { SetProperty(ref _remark, value ?? string.Empty); }
        }

        public bool IsTcpConnection
        {
            get
            {
                return string.Equals(ConnectionType, "Tcp", StringComparison.OrdinalIgnoreCase);
            }
        }

        public bool IsSerialConnection
        {
            get
            {
                return string.Equals(ConnectionType, "Serial", StringComparison.OrdinalIgnoreCase);
            }
        }

        /// <summary>
        /// 新增时重置为默认值。
        /// </summary>
        public void ResetForCreate()
        {
            Id = 0;
            Name = string.Empty;
            DisplayName = string.Empty;
            Vendor = "ModbusGeneric";
            Model = string.Empty;
            ConnectionType = "Tcp";
            ProtocolType = "ModbusTcp";
            IpAddress = "127.0.0.1";
            PortText = "502";
            ComPort = string.Empty;
            BaudRateText = "9600";
            DataBitsText = "8";
            Parity = "None";
            StopBits = "1";
            StationNoText = "1";
            NetworkNoText = string.Empty;
            PcNoText = string.Empty;
            RackText = string.Empty;
            SlotText = string.Empty;
            TimeoutMsText = "1000";
            ReconnectIntervalMsText = "2000";
            ScanIntervalMsText = "200";
            IsEnabled = true;
            SortOrderText = "1";
            Description = string.Empty;
            Remark = string.Empty;
        }

        /// <summary>
        /// 从实体装载到编辑模型。
        /// </summary>
        public void LoadFrom(PlcStationConfigEntity entity)
        {
            if (entity == null)
            {
                ResetForCreate();
                return;
            }

            Id = entity.Id;
            Name = entity.Name ?? string.Empty;
            DisplayName = entity.DisplayName ?? string.Empty;
            Vendor = entity.Vendor ?? string.Empty;
            Model = entity.Model ?? string.Empty;
            ConnectionType = entity.ConnectionType ?? string.Empty;
            ProtocolType = entity.ProtocolType ?? string.Empty;
            IpAddress = entity.IpAddress ?? string.Empty;
            PortText = entity.Port.HasValue ? entity.Port.Value.ToString() : string.Empty;
            ComPort = entity.ComPort ?? string.Empty;
            BaudRateText = entity.BaudRate.HasValue ? entity.BaudRate.Value.ToString() : string.Empty;
            DataBitsText = entity.DataBits.HasValue ? entity.DataBits.Value.ToString() : string.Empty;
            Parity = entity.Parity ?? string.Empty;
            StopBits = entity.StopBits ?? string.Empty;
            StationNoText = entity.StationNo.HasValue ? entity.StationNo.Value.ToString() : string.Empty;
            NetworkNoText = entity.NetworkNo.HasValue ? entity.NetworkNo.Value.ToString() : string.Empty;
            PcNoText = entity.PcNo.HasValue ? entity.PcNo.Value.ToString() : string.Empty;
            RackText = entity.Rack.HasValue ? entity.Rack.Value.ToString() : string.Empty;
            SlotText = entity.Slot.HasValue ? entity.Slot.Value.ToString() : string.Empty;
            TimeoutMsText = entity.TimeoutMs.ToString();
            ReconnectIntervalMsText = entity.ReconnectIntervalMs.ToString();
            ScanIntervalMsText = entity.ScanIntervalMs.ToString();
            IsEnabled = entity.IsEnabled;
            SortOrderText = entity.SortOrder.ToString();
            Description = entity.Description ?? string.Empty;
            Remark = entity.Remark ?? string.Empty;
        }

        /// <summary>
        /// 构建保存实体。
        /// </summary>
        public Result<PlcStationConfigEntity> BuildEntity()
        {
            Name = NormalizeText(Name);
            DisplayName = NormalizeText(DisplayName);
            Vendor = NormalizeText(Vendor);
            Model = NormalizeText(Model);
            ConnectionType = NormalizeText(ConnectionType);
            ProtocolType = NormalizeText(ProtocolType);
            IpAddress = NormalizeText(IpAddress);
            ComPort = NormalizeText(ComPort);
            Parity = NormalizeText(Parity);
            StopBits = NormalizeText(StopBits);
            Description = NormalizeText(Description);
            Remark = NormalizeText(Remark);

            if (string.IsNullOrWhiteSpace(Name))
            {
                return Result<PlcStationConfigEntity>.Fail(-4101, "PLC 名称不能为空", ResultSource.UI);
            }

            if (string.IsNullOrWhiteSpace(ConnectionType))
            {
                return Result<PlcStationConfigEntity>.Fail(-4102, "连接方式不能为空", ResultSource.UI);
            }

            if (string.IsNullOrWhiteSpace(ProtocolType))
            {
                return Result<PlcStationConfigEntity>.Fail(-4103, "协议类型不能为空", ResultSource.UI);
            }

            int timeoutMs;
            if (!TryParsePositiveInt(TimeoutMsText, out timeoutMs))
            {
                return Result<PlcStationConfigEntity>.Fail(-4104, "通讯超时必须为大于 0 的整数", ResultSource.UI);
            }

            int reconnectIntervalMs;
            if (!TryParsePositiveInt(ReconnectIntervalMsText, out reconnectIntervalMs))
            {
                return Result<PlcStationConfigEntity>.Fail(-4105, "重连周期必须为大于 0 的整数", ResultSource.UI);
            }

            int scanIntervalMs;
            if (!TryParsePositiveInt(ScanIntervalMsText, out scanIntervalMs))
            {
                return Result<PlcStationConfigEntity>.Fail(-4106, "扫描周期必须为大于 0 的整数", ResultSource.UI);
            }

            int sortOrder;
            if (!TryParseNonNegativeInt(SortOrderText, out sortOrder))
            {
                return Result<PlcStationConfigEntity>.Fail(-4107, "排序号必须为大于等于 0 的整数", ResultSource.UI);
            }

            int? port = null;
            int? baudRate = null;
            int? dataBits = null;
            short? stationNo = null;
            short? networkNo = null;
            short? pcNo = null;
            short? rack = null;
            short? slot = null;

            Result parseResult = ParseNullableInt(PortText, "端口", out port);
            if (!parseResult.Success)
            {
                return Result<PlcStationConfigEntity>.Fail(parseResult.Code, parseResult.Message, parseResult.Source);
            }

            parseResult = ParseNullableInt(BaudRateText, "波特率", out baudRate);
            if (!parseResult.Success)
            {
                return Result<PlcStationConfigEntity>.Fail(parseResult.Code, parseResult.Message, parseResult.Source);
            }

            parseResult = ParseNullableInt(DataBitsText, "数据位", out dataBits);
            if (!parseResult.Success)
            {
                return Result<PlcStationConfigEntity>.Fail(parseResult.Code, parseResult.Message, parseResult.Source);
            }

            parseResult = ParseNullableShort(StationNoText, "站号", out stationNo);
            if (!parseResult.Success)
            {
                return Result<PlcStationConfigEntity>.Fail(parseResult.Code, parseResult.Message, parseResult.Source);
            }

            parseResult = ParseNullableShort(NetworkNoText, "网络号", out networkNo);
            if (!parseResult.Success)
            {
                return Result<PlcStationConfigEntity>.Fail(parseResult.Code, parseResult.Message, parseResult.Source);
            }

            parseResult = ParseNullableShort(PcNoText, "PC号", out pcNo);
            if (!parseResult.Success)
            {
                return Result<PlcStationConfigEntity>.Fail(parseResult.Code, parseResult.Message, parseResult.Source);
            }

            parseResult = ParseNullableShort(RackText, "机架号", out rack);
            if (!parseResult.Success)
            {
                return Result<PlcStationConfigEntity>.Fail(parseResult.Code, parseResult.Message, parseResult.Source);
            }

            parseResult = ParseNullableShort(SlotText, "插槽号", out slot);
            if (!parseResult.Success)
            {
                return Result<PlcStationConfigEntity>.Fail(parseResult.Code, parseResult.Message, parseResult.Source);
            }

            if (string.Equals(ConnectionType, "Tcp", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(IpAddress))
                {
                    return Result<PlcStationConfigEntity>.Fail(-4108, "TCP 连接方式下 IP 地址不能为空", ResultSource.UI);
                }

                if (!port.HasValue || port.Value <= 0)
                {
                    return Result<PlcStationConfigEntity>.Fail(-4109, "TCP 连接方式下端口必须大于 0", ResultSource.UI);
                }
            }

            if (string.Equals(ConnectionType, "Serial", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(ComPort))
                {
                    return Result<PlcStationConfigEntity>.Fail(-4110, "串口连接方式下串口号不能为空", ResultSource.UI);
                }

                if (!baudRate.HasValue || baudRate.Value <= 0)
                {
                    return Result<PlcStationConfigEntity>.Fail(-4111, "串口连接方式下波特率必须大于 0", ResultSource.UI);
                }
            }

            if (string.IsNullOrWhiteSpace(DisplayName))
            {
                DisplayName = Name;
            }

            var entity = new PlcStationConfigEntity
            {
                Id = Id,
                Name = Name,
                DisplayName = DisplayName,
                Vendor = Vendor,
                Model = Model,
                ConnectionType = ConnectionType,
                ProtocolType = ProtocolType,
                IpAddress = IpAddress,
                Port = port,
                ComPort = ComPort,
                BaudRate = baudRate,
                DataBits = dataBits,
                Parity = Parity,
                StopBits = StopBits,
                StationNo = stationNo,
                NetworkNo = networkNo,
                PcNo = pcNo,
                Rack = rack,
                Slot = slot,
                TimeoutMs = timeoutMs,
                ReconnectIntervalMs = reconnectIntervalMs,
                ScanIntervalMs = scanIntervalMs,
                IsEnabled = IsEnabled,
                SortOrder = sortOrder,
                Description = Description,
                Remark = Remark,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };

            return Result<PlcStationConfigEntity>.OkItem(entity, "PLC 站编辑模型构建成功", ResultSource.UI)
                .WithNotifyMode(ResultNotifyMode.Silent);
        }

        private static string NormalizeText(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
        }

        private static bool TryParsePositiveInt(string text, out int value)
        {
            if (int.TryParse((text ?? string.Empty).Trim(), out value))
            {
                return value > 0;
            }

            return false;
        }

        private static bool TryParseNonNegativeInt(string text, out int value)
        {
            if (int.TryParse((text ?? string.Empty).Trim(), out value))
            {
                return value >= 0;
            }

            return false;
        }

        private static Result ParseNullableInt(string text, string displayName, out int? value)
        {
            value = null;

            if (string.IsNullOrWhiteSpace(text))
            {
                return Result.Ok("允许为空", ResultSource.UI).WithNotifyMode(ResultNotifyMode.Silent);
            }

            int parsed;
            if (!int.TryParse(text.Trim(), out parsed))
            {
                return Result.Fail(-4112, displayName + "必须为整数", ResultSource.UI);
            }

            value = parsed;
            return Result.Ok("解析成功", ResultSource.UI).WithNotifyMode(ResultNotifyMode.Silent);
        }

        private static Result ParseNullableShort(string text, string displayName, out short? value)
        {
            value = null;

            if (string.IsNullOrWhiteSpace(text))
            {
                return Result.Ok("允许为空", ResultSource.UI).WithNotifyMode(ResultNotifyMode.Silent);
            }

            short parsed;
            if (!short.TryParse(text.Trim(), out parsed))
            {
                return Result.Fail(-4113, displayName + "必须为整数", ResultSource.UI);
            }

            value = parsed;
            return Result.Ok("解析成功", ResultSource.UI).WithNotifyMode(ResultNotifyMode.Silent);
        }
    }
}