using ProtocolLib.CommonLib.Common;
using ProtocolLib.CommonLib.Interface;
using ProtocolLib.CommonLib.Model;
using ProtocolLib.CommonLib.Net.NetworkBase;
using ProtocolLib.S7Tcp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolLib.S7Tcp.Core
{
    public class SiemensS7 : NetworkDeviceBase
    {
        private byte plc_rack = 0x00;
        private byte plc_slot = 0x00;
        private int pdu_length = 0;

        const byte pduStart = 0x28;            // CPU start
        const byte pduStop = 0x29;             // CPU stop
        const byte pduAlreadyStarted = 0x02;   // CPU already in run mode
        const byte pduAlreadyStopped = 0x07;   // CPU already in stop mode

        public SiemensS7(E_SiemensPLCS siemens)
        {
            Initialization(siemens, string.Empty);
        }
        public SiemensS7(E_SiemensPLCS siemens, string ipAddress)
        {
            Initialization(siemens, ipAddress);
        }
        public SiemensS7(E_SiemensPLCS siemens, string ipAddress, int port)
        {
            Initialization(siemens, ipAddress, port);
        }
        public void UpdateIPPortInfo(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }
        private void Initialization(E_SiemensPLCS siemens, string ipAddress, int port)
        {
            WordLength = 2;
            ip = ipAddress;
            this.port = port;
            CurrentPlc = siemens;
            byteTransform = new ReverseBytesTransform();

            switch (siemens)
            {
                case E_SiemensPLCS.S1200: plcHead1[21] = 0; break;
                case E_SiemensPLCS.S300: plcHead1[21] = 2; break;
                case E_SiemensPLCS.S400: plcHead1[21] = 3; plcHead1[17] = 0x00; break;
                case E_SiemensPLCS.S1500: plcHead1[21] = 0; break;
                case E_SiemensPLCS.S200Smart:
                    {
                        plcHead1 = plcHead1_200smart;
                        plcHead2 = plcHead2_200smart;
                        break;
                    }
                case E_SiemensPLCS.S200:
                    {
                        plcHead1 = plcHead1_200;
                        plcHead2 = plcHead2_200;
                        break;
                    }
                default: plcHead1[18] = 0; break;
            }
        }
        private void Initialization(E_SiemensPLCS siemens, string ipAddress)
        {
            WordLength = 2;
            ip = ipAddress;
            port = 102;
            CurrentPlc = siemens;
            byteTransform = new ReverseBytesTransform();

            switch (siemens)
            {
                case E_SiemensPLCS.S1200: plcHead1[21] = 0; break;
                case E_SiemensPLCS.S300: plcHead1[21] = 2; break;
                case E_SiemensPLCS.S400: plcHead1[21] = 3; plcHead1[17] = 0x00; break;
                case E_SiemensPLCS.S1500: plcHead1[21] = 0; break;
                case E_SiemensPLCS.S200Smart:
                    {
                        plcHead1 = plcHead1_200smart;
                        plcHead2 = plcHead2_200smart;
                        break;
                    }
                case E_SiemensPLCS.S200:
                    {
                        plcHead1 = plcHead1_200;
                        plcHead2 = plcHead2_200;
                        break;
                    }
                default: plcHead1[18] = 0; break;
            }
        }

        #region Constructor
        protected override INetMessage GetNewNetMessage() => new S7Message();

        /// <summary>
        /// PLC的槽号，针对S7-400的PLC设置的
        /// </summary>
        public byte Slot
        {
            get => plc_slot;
            set
            {
                plc_slot = value;
                plcHead1[21] = (byte)((this.plc_rack * 0x20) + this.plc_slot);
            }
        }
        /// <summary>
		/// PLC的机架号，针对S7-400的PLC设置的
		/// </summary>
		public byte Rack
        {
            get => plc_rack;
            set
            {
                this.plc_rack = value;
                plcHead1[21] = (byte)((this.plc_rack * 0x20) + this.plc_slot);
            }
        }
        /// <summary>
		/// 获取或设置当前PLC的连接方式，PG: 0x01，OP: 0x02，S7Basic: 0x03...0x10
		/// </summary>
		public byte ConnectionType
        {
            get => this.plcHead1[20];
            set
            {
                if (CurrentPlc == E_SiemensPLCS.S200 ||
                    CurrentPlc == E_SiemensPLCS.S200Smart)
                {

                }
                else
                {
                    this.plcHead1[20] = value;
                }
            }
        }
        /// <summary>
		/// 西门子相关的本地TSAP参数信息
		/// </summary>
		public int LocalTSAP
        {
            get => this.plcHead1[16] * 256 + this.plcHead1[17];
            set
            {
                if (CurrentPlc == E_SiemensPLCS.S200 ||
                    CurrentPlc == E_SiemensPLCS.S200Smart)
                {

                }
                else
                {
                    this.plcHead1[16] = BitConverter.GetBytes(value)[1];
                    this.plcHead1[17] = BitConverter.GetBytes(value)[0];
                }
            }
        }
        /// <summary>
		/// 获取当前西门子的PDU的长度信息，不同型号PLC的值会不一样
		/// </summary>
		public int PDULength => pdu_length;

        #endregion
        #region NetworkSecBase Override
        public override M_OperateResult<byte[]> ReadData(Socket socket, byte[] send, bool hasResponseData = true, bool usePackHeader = true)
        {
            while (true)
            {
                M_OperateResult<byte[]> read = base.ReadData(socket, send, hasResponseData, usePackHeader);
                if (!read.IsSuccess) return read;

                if ((read.Content[2] * 256 + read.Content[3]) != 0x07) return read;
            }
        }
        protected override M_OperateResult InitOnConnect(Socket socket)
        {
            // 第一次握手 -> First handshake
            M_OperateResult<byte[]> read_first = ReadData(socket, plcHead1);
            if (!read_first.IsSuccess) return read_first;

            // 第二次握手 -> Second handshake
            M_OperateResult<byte[]> read_second = ReadData(socket, plcHead2);
            if (!read_second.IsSuccess) return read_second;

            // 调整单次接收的pdu长度信息
            pdu_length = byteTransform.bytes2UInt16(read_second.Content.SelectLast(2), 0) - 28;
            if (pdu_length < 200) pdu_length = 200;

            // 返回成功的信号 -> Return a successful signal
            return M_OperateResult.CreateSuccessResult();
        }


        #endregion
        #region Read OrderNumber
        public M_OperateResult<string> ReadOrderNumber() => ToolBasic.GetSuccessResultFromOther(ReadData(plcOrderNumber), m => Encoding.ASCII.GetString(m, 71, 20));

        #endregion
        #region Start Stop
        private M_OperateResult CheckStartResult(byte[] content)
        {
            if (content.Length < 19) return new M_OperateResult("Receive error");

            if (content[19] != pduStart) return new M_OperateResult("Can not start PLC");
            else if (content[20] != pduAlreadyStarted) return new M_OperateResult("Can not start PLC");

            return M_OperateResult.CreateSuccessResult();
        }

        private M_OperateResult CheckStopResult(byte[] content)
        {
            if (content.Length < 19) return new M_OperateResult("Receive error");

            if (content[19] != pduStop) return new M_OperateResult("Can not stop PLC");
            else if (content[20] != pduAlreadyStopped) return new M_OperateResult("Can not stop PLC");

            return M_OperateResult.CreateSuccessResult();
        }
        /// <summary>
		/// 对PLC进行热启动，目前仅适用于200smart型号
		/// <returns>是否启动成功的结果对象</returns>
        public M_OperateResult HotStart() => ToolBasic.GetResultFromOther(ReadData(S7_HOT_START), CheckStartResult);
        /// <summary>
		/// 对PLC进行冷启动，目前仅适用于200smart型号
		/// <returns>是否启动成功的结果对象</returns>
        public M_OperateResult ColdStart() => ToolBasic.GetResultFromOther(ReadData(S7_COLD_START), CheckStartResult);
        /// <summary>
		/// 对PLC进行停止，目前仅适用于200smart型号
		/// <returns>是否启动成功的结果对象</returns>
        public M_OperateResult Stop() => ToolBasic.GetResultFromOther(ReadData(S7_STOP), CheckStopResult);

        #endregion
        #region Read Write Support
        /// <summary>
        /// 从PLC读取原始的字节数据，地址格式为I100，Q100，DB20.100，M100，长度参数以字节为单位
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public override M_OperateResult<byte[]> Read(string address, ushort length)
        {
            M_OperateResult<S7AddressData> addressResult = S7AddressData.ParseFrom(address, length);
            if (!addressResult.IsSuccess) return M_OperateResult.CreateFailedResult<byte[]>(addressResult);

            // 如果长度超过 pdu_length，分批次读取 -> If the length is more than pdu_length, read in batches
            List<byte> bytesContent = new List<byte>();
            ushort alreadyFinished = 0;
            while (alreadyFinished < length)
            {
                ushort readLength = (ushort)Math.Min(length - alreadyFinished, pdu_length);
                addressResult.Content.Length = readLength;
                M_OperateResult<byte[]> read = Read(new S7AddressData[] { addressResult.Content });
                if (!read.IsSuccess) return read;

                bytesContent.AddRange(read.Content);
                alreadyFinished += readLength;
                if (addressResult.Content.DataCode == 0x1F || addressResult.Content.DataCode == 0x1E)
                    addressResult.Content.AddressStart += readLength / 2;
                else
                    addressResult.Content.AddressStart += readLength * 8;
            }

            return M_OperateResult.CreateSuccessResult(bytesContent.ToArray());
        }
        /// <summary>
        /// 从PLC读取数据，地址格式为I100，Q100，DB20.100，M100，以位为单位
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
		private M_OperateResult<byte[]> ReadBitFromPLC(string address)
        {
            // 指令生成
            M_OperateResult<byte[]> command = BuildBitReadCommand(address);
            if (!command.IsSuccess) return M_OperateResult.CreateFailedResult<byte[]>(command);

            // 核心交互
            M_OperateResult<byte[]> read = ReadData(command.Content);
            if (!read.IsSuccess) return read;

            // 分析结果
            return AnalysisReadBit(read.Content);
        }
        /// <summary>
        /// 一次性从PLC获取所有的数据，按照先后顺序返回一个统一的Buffer，需要按照顺序处理，两个数组长度必须一致，数组长度无限制
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public M_OperateResult<byte[]> Read(string[] address, ushort[] length)
        {
            S7AddressData[] addressResult = new S7AddressData[address.Length];
            for (int i = 0; i < address.Length; i++)
            {
                M_OperateResult<S7AddressData> tmp = S7AddressData.ParseFrom(address[i], length[i]);
                if (!tmp.IsSuccess) return M_OperateResult.CreateFailedResult<byte[]>(tmp);

                addressResult[i] = tmp.Content;
            }

            return Read(addressResult);
        }
        /// <summary>
        /// 读取西门子的地址数据信息，支持任意个数的数据读取
        /// </summary>
        /// <param name="s7Addresses"></param>
        /// <returns></returns>
        public M_OperateResult<byte[]> Read(S7AddressData[] s7Addresses)
        {
            if (s7Addresses.Length > 19)
            {
                List<byte> bytes = new List<byte>();
                List<S7AddressData[]> groups = ToolBasic.ArraySplitByLength<S7AddressData>(s7Addresses, 19);
                for (int i = 0; i < groups.Count; i++)
                {
                    M_OperateResult<byte[]> read = Read(groups[i]);
                    if (!read.IsSuccess) return read;

                    bytes.AddRange(read.Content);
                }
                return M_OperateResult.CreateSuccessResult(bytes.ToArray());
            }
            else
            {
                return ReadS7AddressData(s7Addresses);
            }
        }
        /// <summary>
        /// 单次的读取，只能读取最多19个数组的长度
        /// </summary>
        /// <param name="s7Addresses"></param>
        /// <returns></returns>
        private M_OperateResult<byte[]> ReadS7AddressData(S7AddressData[] s7Addresses)
        {
            // 构建指令
            M_OperateResult<byte[]> command = BuildReadCommand(s7Addresses);
            if (!command.IsSuccess) return command;

            // 核心交互
            M_OperateResult<byte[]> read = ReadData(command.Content);
            if (!read.IsSuccess) return read;

            // 分析结果
            return AnalysisReadByte(s7Addresses, read.Content);
        }

        /// <summary>
        /// 基础的写入数据的操作支持
        /// </summary>
        /// <param name="entireValue"></param>
        /// <returns></returns>
        private M_OperateResult WriteBase(byte[] entireValue) => ToolBasic.GetResultFromOther(ReadData(entireValue), AnalysisWrite);

        public override M_OperateResult Write(string address, byte[] value)
        {
            M_OperateResult<S7AddressData> analysis = S7AddressData.ParseFrom(address);
            if (!analysis.IsSuccess) return M_OperateResult.CreateFailedResult<byte[]>(analysis);

            int length = value.Length;
            ushort alreadyFinished = 0;
            while (alreadyFinished < length)
            {
                ushort writeLength = (ushort)Math.Min(length - alreadyFinished, pdu_length);
                byte[] buffer = byteTransform.bytes2Byte(value, alreadyFinished, writeLength);

                M_OperateResult<byte[]> command = BuildWriteByteCommand(analysis, buffer);
                if (!command.IsSuccess) return command;

                M_OperateResult write = WriteBase(command.Content);
                if (!write.IsSuccess) return write;

                alreadyFinished += writeLength;
                analysis.Content.AddressStart += writeLength * 8;
            }

            return M_OperateResult.CreateSuccessResult();
        }


















        #endregion
        #region Read Write Bool
        /// <summary>
        /// 读取指定地址的bool数据，地址格式为I100，M100，Q100，DB20.100
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public override M_OperateResult<bool> ReadBool(string address) => ToolBasic.GetResultFromBytes(ReadBitFromPLC(address), m => m[0] != 0x00);
        /// <summary>
        /// 读取指定地址的bool数组，地址格式为I100，M100，Q100，DB20.100
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public override M_OperateResult<bool[]> ReadBool(string address, ushort length)
        {
            M_OperateResult<S7AddressData> analysis = S7AddressData.ParseFrom(address);
            if (!analysis.IsSuccess) return M_OperateResult.CreateFailedResult<bool[]>(analysis);

            ToolBasic.CalculateStartBitIndexAndLength(analysis.Content.AddressStart, length, out int newStart, out ushort byteLength, out int offset);
            analysis.Content.AddressStart = newStart;
            analysis.Content.Length = byteLength;

            M_OperateResult<byte[]> read = Read(new S7AddressData[] { analysis.Content });
            if (!read.IsSuccess) return M_OperateResult.CreateFailedResult<bool[]>(read);

            return M_OperateResult.CreateSuccessResult(read.Content.ToBoolArray().SelectMiddle(offset, length));
        }
        /// <summary>
        /// 写入PLC的一个位，例如"M100.6"，"I100.7"，"Q100.0"，"DB20.100.0"，如果只写了"M100"默认为"M100.0"
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override M_OperateResult Write(string address, bool value)
        {
            // 生成指令
            M_OperateResult<byte[]> command = BuildWriteBitCommand(address, value);
            if (!command.IsSuccess) return command;

            return WriteBase(command.Content);
        }
        /// <summary>
        /// 向PLC中写入bool数组，比如你写入M100,那么data[0]对应M100.0
        /// </summary>
        /// <param name="address"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public override M_OperateResult Write(string address, bool[] values) => Write(address, ToolBasic.BoolArrayToByte(values));


        #endregion
        #region Read Write Byte
        /// <summary>
        /// 读取指定地址的byte数据，地址格式I100，M100，Q100，DB20.100
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public M_OperateResult<byte> ReadByte(string address) => ToolBasic.GetResultFromArray(Read(address, 1));

        /// <summary>
        /// 向PLC中写入byte数据，返回值说明
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public M_OperateResult Write(string address, byte value) => Write(address, new byte[] { value });

        #endregion
        #region ReadWrite String

        public override M_OperateResult Write(string address, string value, Encoding encoding)
        {
            if (value == null) value = string.Empty;

            byte[] buffer = encoding.GetBytes(value);
            if (encoding == Encoding.Unicode) buffer = ToolBasic.BytesReverseByWord(buffer);

            if (CurrentPlc != E_SiemensPLCS.S200Smart)
            {
                // need read one time
                M_OperateResult<byte[]> readLength = Read(address, 2);
                if (!readLength.IsSuccess) return readLength;

                if (readLength.Content[0] == 255) return new M_OperateResult<string>("Value in plc is not string type");
                if (readLength.Content[0] == 0) readLength.Content[0] = 254; // allow to create new string
                if (value.Length > readLength.Content[0]) return new M_OperateResult<string>("String length is too long than plc defined");

                return Write(address, ToolBasic.SpliceArray(new byte[] { readLength.Content[0], (byte)value.Length }, buffer));
            }
            else
            {
                return Write(address, ToolBasic.SpliceArray(new byte[] { (byte)value.Length }, buffer));
            }
        }

        public M_OperateResult WriteWString(string address, string value) => Write(address, value, Encoding.Unicode);

        public override M_OperateResult<string> ReadString(string address, ushort length, Encoding encoding)
        {
            if (length == 0) return ReadString(address);
            return base.ReadString(address, length, encoding);
        }
        /// <summary>
        /// 读取西门子的地址的字符串信息
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public M_OperateResult<string> ReadString(string address)
        {
            if (CurrentPlc != E_SiemensPLCS.S200Smart)
            {
                var read = Read(address, 2);
                if (!read.IsSuccess) return M_OperateResult.CreateFailedResult<string>(read);

                if (read.Content[0] == 0 || read.Content[0] == 255) return new M_OperateResult<string>("Value in plc is not string type");

                var readString = Read(address, (ushort)(2 + read.Content[1]));
                if (!readString.IsSuccess) return M_OperateResult.CreateFailedResult<string>(readString);

                return M_OperateResult.CreateSuccessResult(Encoding.ASCII.GetString(readString.Content, 2, readString.Content.Length - 2));
            }
            else
            {
                var read = Read(address, 1);
                if (!read.IsSuccess) return M_OperateResult.CreateFailedResult<string>(read);

                var readString = Read(address, (ushort)(1 + read.Content[0]));
                if (!readString.IsSuccess) return M_OperateResult.CreateFailedResult<string>(readString);

                return M_OperateResult.CreateSuccessResult(Encoding.ASCII.GetString(readString.Content, 1, readString.Content.Length - 1));
            }
        }
        /// <summary>
        /// 读取西门子的地址的字符串信息
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public M_OperateResult<string> ReadWString(string address)
        {
            if (CurrentPlc != E_SiemensPLCS.S200Smart)
            {
                var read = Read(address, 2);
                if (!read.IsSuccess) return M_OperateResult.CreateFailedResult<string>(read);

                if (read.Content[0] == 0 || read.Content[0] == 255) return new M_OperateResult<string>("Value in plc is not string type");    // max string length can't be zero

                var readString = Read(address, (ushort)(2 + read.Content[1] * 2));
                if (!readString.IsSuccess) return M_OperateResult.CreateFailedResult<string>(readString);

                return M_OperateResult.CreateSuccessResult(Encoding.Unicode.GetString(ToolBasic.BytesReverseByWord(readString.Content.RemoveBegin(2))));
            }
            else
            {
                var read = Read(address, 1);
                if (!read.IsSuccess) return M_OperateResult.CreateFailedResult<string>(read);

                var readString = Read(address, (ushort)(1 + read.Content[0] * 2));
                if (!readString.IsSuccess) return M_OperateResult.CreateFailedResult<string>(readString);

                return M_OperateResult.CreateSuccessResult(Encoding.Unicode.GetString(readString.Content, 1, readString.Content.Length - 1));
            }
        }

        #endregion
        #region ReadWrite DateTime
        /// <summary>
        /// 从PLC中读取时间格式的数据
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public M_OperateResult<DateTime> ReadDateTime(string address) => ToolBasic.GetResultFromBytes(Read(address, 8), SiemensDateTime.FromByteArray);

        /// <summary>
        /// 向PLC中写入时间格式的数据
        /// </summary>
        /// <param name="address"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public M_OperateResult Write(string address, DateTime dateTime) => Write(address, SiemensDateTime.ToByteArray(dateTime));
















        #endregion
        #region Head Codes
        private byte[] plcHead1 = new byte[22]
        {
            0x03,0x00,0x00,0x16,0x11,0xE0,0x00,0x00,0x00,0x01,0x00,0xC0,0x01,0x0A,0xC1,0x02,
            0x01,0x02,0xC2,0x02,0x01,0x00
        };
        private byte[] plcHead2 = new byte[25]
        {
            0x03,0x00,0x00,0x19,0x02,0xF0,0x80,0x32,0x01,0x00,0x00,0x04,0x00,0x00,0x08,0x00,
            0x00,0xF0,0x00,0x00,0x01,0x00,0x01,0x01,0xE0
        };
        private byte[] plcOrderNumber = new byte[]
        {
            0x03,0x00,0x00,0x21,0x02,0xF0,0x80,0x32,0x07,0x00,0x00,0x00,0x01,0x00,0x08,0x00,
            0x08,0x00,0x01,0x12,0x04,0x11,0x44,0x01,0x00,0xFF,0x09,0x00,0x04,0x00,0x11,0x00,
            0x00
        };
        private E_SiemensPLCS CurrentPlc = E_SiemensPLCS.S1200;
        private byte[] plcHead1_200smart = new byte[22]
        {
            0x03,0x00,0x00,0x16,0x11,0xE0,0x00,0x00,0x00,0x01,0x00,0xC1,0x02,0x10,0x00,0xC2,
            0x02,0x03,0x00,0xC0,0x01,0x0A
        };
        private byte[] plcHead2_200smart = new byte[25]
        {
            0x03,0x00,0x00,0x19,0x02,0xF0,0x80,0x32,0x01,0x00,0x00,0xCC,0xC1,0x00,0x08,0x00,
            0x00,0xF0,0x00,0x00,0x01,0x00,0x01,0x03,0xC0
        };

        private byte[] plcHead1_200 = new byte[22]
        {
            0x03,0x00,0x00,0x16,0x11,0xE0,0x00,0x00,0x00,0x01,0x00,0xC1,0x02,0x4D,0x57,0xC2,
            0x02,0x4D,0x57,0xC0,0x01,0x09
        };
        private byte[] plcHead2_200 = new byte[25]
        {
            0x03,0x00,0x00,0x19,0x02,0xF0,0x80,0x32,0x01,0x00,0x00,0x00,0x00,0x00,0x08,0x00,
            0x00,0xF0,0x00,0x00,0x01,0x00,0x01,0x03,0xC0
        };

        byte[] S7_STOP = {
            0x03, 0x00, 0x00, 0x21, 0x02, 0xf0, 0x80, 0x32, 0x01, 0x00, 0x00, 0x0e, 0x00, 0x00, 0x10, 0x00,
            0x00, 0x29, 0x00, 0x00, 0x00, 0x00, 0x00, 0x09, 0x50, 0x5f, 0x50, 0x52, 0x4f, 0x47, 0x52, 0x41,
            0x4d
        };

        byte[] S7_HOT_START = {
            0x03, 0x00, 0x00, 0x25, 0x02, 0xf0, 0x80, 0x32, 0x01, 0x00, 0x00, 0x0c, 0x00, 0x00, 0x14, 0x00,
            0x00, 0x28, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xfd, 0x00, 0x00, 0x09, 0x50, 0x5f, 0x50, 0x52,
            0x4f, 0x47, 0x52, 0x41, 0x4d
        };

        byte[] S7_COLD_START = {
            0x03, 0x00, 0x00, 0x27, 0x02, 0xf0, 0x80, 0x32, 0x01, 0x00, 0x00, 0x0f, 0x00, 0x00, 0x16, 0x00,
            0x00, 0x28, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xfd, 0x00, 0x02, 0x43, 0x20, 0x09, 0x50, 0x5f,
            0x50, 0x52, 0x4f, 0x47, 0x52, 0x41, 0x4d
        };

        #endregion
        #region Build Command

        public static M_OperateResult<byte[]> BuildReadCommand(S7AddressData[] s7Addresses)
        {
            if (s7Addresses == null) throw new NullReferenceException("s7Addresses");
            if (s7Addresses.Length > 19) throw new Exception(CommonResources.Get.Language.Language.ipaddresserror);

            int readCount = s7Addresses.Length;
            byte[] _PLCCommand = new byte[19 + readCount * 12];
            // ======================================================================================
            _PLCCommand[0] = 0x03;                                                // 报文头 -> Head
            _PLCCommand[1] = 0x00;
            _PLCCommand[2] = (byte)(_PLCCommand.Length / 256);                    // 长度 -> Length
            _PLCCommand[3] = (byte)(_PLCCommand.Length % 256);
            _PLCCommand[4] = 0x02;                                                // 固定 -> Fixed
            _PLCCommand[5] = 0xF0;
            _PLCCommand[6] = 0x80;
            _PLCCommand[7] = 0x32;                                                // 协议标识 -> Protocol identification
            _PLCCommand[8] = 0x01;                                                // 命令：发 -> Command: Send
            _PLCCommand[9] = 0x00;                                                // redundancy identification (reserved): 0x0000;
            _PLCCommand[10] = 0x00;
            _PLCCommand[11] = 0x00;                                                // protocol data unit reference; it’s increased by request event;
            _PLCCommand[12] = 0x01;
            _PLCCommand[13] = (byte)((_PLCCommand.Length - 17) / 256);             // 参数命令数据总长度 -> Parameter command Data total length
            _PLCCommand[14] = (byte)((_PLCCommand.Length - 17) % 256);
            _PLCCommand[15] = 0x00;                                                // 读取内部数据时为00，读取CPU型号为Data数据长度 -> Read internal data is 00, read CPU model is data length
            _PLCCommand[16] = 0x00;
            // =====================================================================================
            _PLCCommand[17] = 0x04;                                                // 读写指令，04读，05写 -> Read-write instruction, 04 read, 05 Write
            _PLCCommand[18] = (byte)readCount;                                     // 读取数据块个数 -> Number of data blocks read

            for (int ii = 0; ii < readCount; ii++)
            {
                //===========================================================================================
                // 指定有效值类型 -> Specify a valid value type
                _PLCCommand[19 + ii * 12] = 0x12;
                // 接下来本次地址访问长度 -> The next time the address access length
                _PLCCommand[20 + ii * 12] = 0x0A;
                // 语法标记，ANY -> Syntax tag, any
                _PLCCommand[21 + ii * 12] = 0x10;
                // 按字为单位 -> by word
                if (s7Addresses[ii].DataCode == 0x1E || s7Addresses[ii].DataCode == 0x1F)
                {
                    _PLCCommand[22 + ii * 12] = s7Addresses[ii].DataCode;
                    // 访问数据的个数 -> Number of Access data
                    _PLCCommand[23 + ii * 12] = (byte)(s7Addresses[ii].Length / 2 / 256);
                    _PLCCommand[24 + ii * 12] = (byte)(s7Addresses[ii].Length / 2 % 256);
                }
                else
                {
                    if (s7Addresses[ii].DataCode == 0x06 | s7Addresses[ii].DataCode == 0x07)
                    {
                        // 访问数据的个数 -> Number of Access data
                        _PLCCommand[22 + ii * 12] = 0x04;
                        _PLCCommand[23 + ii * 12] = (byte)(s7Addresses[ii].Length / 2 / 256);
                        _PLCCommand[24 + ii * 12] = (byte)(s7Addresses[ii].Length / 2 % 256);
                    }
                    else
                    {
                        _PLCCommand[22 + ii * 12] = 0x02;
                        // 访问数据的个数 -> Number of Access data
                        _PLCCommand[23 + ii * 12] = (byte)(s7Addresses[ii].Length / 256);
                        _PLCCommand[24 + ii * 12] = (byte)(s7Addresses[ii].Length % 256);
                    }
                }
                // DB块编号，如果访问的是DB块的话 -> DB block number, if you are accessing a DB block
                _PLCCommand[25 + ii * 12] = (byte)(s7Addresses[ii].DbBlock / 256);
                _PLCCommand[26 + ii * 12] = (byte)(s7Addresses[ii].DbBlock % 256);
                // 访问数据类型 -> Accessing data types
                _PLCCommand[27 + ii * 12] = s7Addresses[ii].DataCode;
                // 偏移位置 -> Offset position
                _PLCCommand[28 + ii * 12] = (byte)(s7Addresses[ii].AddressStart / 256 / 256 % 256);
                _PLCCommand[29 + ii * 12] = (byte)(s7Addresses[ii].AddressStart / 256 % 256);
                _PLCCommand[30 + ii * 12] = (byte)(s7Addresses[ii].AddressStart % 256);
            }

            return M_OperateResult.CreateSuccessResult(_PLCCommand);
        }

        /// <summary>
        /// 生成一个位读取数据指令头的通用方法
        /// </summary>
        /// <param name="address">起始地址，例如M100.0，I0.1，Q0.1，DB2.100.2</param>
        /// <returns>包含结果对象的报文</returns>
        public static M_OperateResult<byte[]> BuildBitReadCommand(string address)
        {
            M_OperateResult<S7AddressData> analysis = S7AddressData.ParseFrom(address);
            if (!analysis.IsSuccess) return M_OperateResult.CreateFailedResult<byte[]>(analysis);

            byte[] _PLCCommand = new byte[31];
            _PLCCommand[0] = 0x03;
            _PLCCommand[1] = 0x00;
            // 长度 -> Length
            _PLCCommand[2] = (byte)(_PLCCommand.Length / 256);
            _PLCCommand[3] = (byte)(_PLCCommand.Length % 256);
            // 固定 -> Fixed
            _PLCCommand[4] = 0x02;
            _PLCCommand[5] = 0xF0;
            _PLCCommand[6] = 0x80;
            _PLCCommand[7] = 0x32;
            // 命令：发 -> command to send
            _PLCCommand[8] = 0x01;
            // 标识序列号
            _PLCCommand[9] = 0x00;
            _PLCCommand[10] = 0x00;
            _PLCCommand[11] = 0x00;
            _PLCCommand[12] = 0x01;
            // 命令数据总长度 -> Identification serial Number
            _PLCCommand[13] = (byte)((_PLCCommand.Length - 17) / 256);
            _PLCCommand[14] = (byte)((_PLCCommand.Length - 17) % 256);

            _PLCCommand[15] = 0x00;
            _PLCCommand[16] = 0x00;

            // 命令起始符 -> Command start character
            _PLCCommand[17] = 0x04;
            // 读取数据块个数 -> Number of data blocks read
            _PLCCommand[18] = 0x01;

            //===========================================================================================
            // 读取地址的前缀 -> Read the prefix of the address
            _PLCCommand[19] = 0x12;
            _PLCCommand[20] = 0x0A;
            _PLCCommand[21] = 0x10;
            // 读取的数据时位 -> Data read-time bit
            _PLCCommand[22] = 0x01;
            // 访问数据的个数 -> Number of Access data
            _PLCCommand[23] = 0x00;
            _PLCCommand[24] = 0x01;
            // DB块编号，如果访问的是DB块的话 -> DB block number, if you are accessing a DB block
            _PLCCommand[25] = (byte)(analysis.Content.DbBlock / 256);
            _PLCCommand[26] = (byte)(analysis.Content.DbBlock % 256);
            // 访问数据类型 -> Types of reading data
            _PLCCommand[27] = analysis.Content.DataCode;
            // 偏移位置 -> Offset position
            _PLCCommand[28] = (byte)(analysis.Content.AddressStart / 256 / 256 % 256);
            _PLCCommand[29] = (byte)(analysis.Content.AddressStart / 256 % 256);
            _PLCCommand[30] = (byte)(analysis.Content.AddressStart % 256);

            return M_OperateResult.CreateSuccessResult(_PLCCommand);
        }

        /// <summary>
        /// 生成一个写入字节数据的指令
        /// </summary>
        /// <param name="analysis">起始地址，示例M100,I100,Q100,DB1.100</param>
        /// <param name="data">原始的字节数据</param>
        /// <returns>包含结果对象的报文</returns>
        public static M_OperateResult<byte[]> BuildWriteByteCommand(M_OperateResult<S7AddressData> analysis, byte[] data)
        {
            byte[] _PLCCommand = new byte[35 + data.Length];
            _PLCCommand[0] = 0x03;
            _PLCCommand[1] = 0x00;
            // 长度 -> Length
            _PLCCommand[2] = (byte)((35 + data.Length) / 256);
            _PLCCommand[3] = (byte)((35 + data.Length) % 256);
            // 固定 -> Fixed
            _PLCCommand[4] = 0x02;
            _PLCCommand[5] = 0xF0;
            _PLCCommand[6] = 0x80;
            _PLCCommand[7] = 0x32;
            // 命令 发 -> command to send
            _PLCCommand[8] = 0x01;
            // 标识序列号 -> Identification serial Number
            _PLCCommand[9] = 0x00;
            _PLCCommand[10] = 0x00;
            _PLCCommand[11] = 0x00;
            _PLCCommand[12] = 0x01;
            // 固定 -> Fixed
            _PLCCommand[13] = 0x00;
            _PLCCommand[14] = 0x0E;
            // 写入长度+4 -> Write Length +4
            _PLCCommand[15] = (byte)((4 + data.Length) / 256);
            _PLCCommand[16] = (byte)((4 + data.Length) % 256);
            // 读写指令 -> Read and write instructions
            _PLCCommand[17] = 0x05;
            // 写入数据块个数 -> Number of data blocks written
            _PLCCommand[18] = 0x01;
            // 固定，返回数据长度 -> Fixed, return data length
            _PLCCommand[19] = 0x12;
            _PLCCommand[20] = 0x0A;
            _PLCCommand[21] = 0x10;
            if (analysis.Content.DataCode == 0x06 || analysis.Content.DataCode == 0x07)
            {
                // 写入方式，1是按位，2是按字 -> Write mode, 1 is bitwise, 2 is by byte, 4 is by word
                _PLCCommand[22] = 0x04;
                // 写入数据的个数 -> Number of Write Data
                _PLCCommand[23] = (byte)(data.Length / 2 / 256);
                _PLCCommand[24] = (byte)(data.Length / 2 % 256);
            }
            else
            {
                // 写入方式，1是按位，2是按字 -> Write mode, 1 is bitwise, 2 is by word
                _PLCCommand[22] = 0x02;
                // 写入数据的个数 -> Number of Write Data
                _PLCCommand[23] = (byte)(data.Length / 256);
                _PLCCommand[24] = (byte)(data.Length % 256);
            }
            // DB块编号，如果访问的是DB块的话 -> DB block number, if you are accessing a DB block
            _PLCCommand[25] = (byte)(analysis.Content.DbBlock / 256);
            _PLCCommand[26] = (byte)(analysis.Content.DbBlock % 256);
            // 写入数据的类型 -> Types of writing data
            _PLCCommand[27] = analysis.Content.DataCode;
            // 偏移位置 -> Offset position
            _PLCCommand[28] = (byte)(analysis.Content.AddressStart / 256 / 256 % 256); ;
            _PLCCommand[29] = (byte)(analysis.Content.AddressStart / 256 % 256);
            _PLCCommand[30] = (byte)(analysis.Content.AddressStart % 256);
            // 按字写入 -> Write by Word
            _PLCCommand[31] = 0x00;
            _PLCCommand[32] = 0x04;
            // 按位计算的长度 -> The length of the bitwise calculation
            _PLCCommand[33] = (byte)(data.Length * 8 / 256);
            _PLCCommand[34] = (byte)(data.Length * 8 % 256);

            data.CopyTo(_PLCCommand, 35);

            return M_OperateResult.CreateSuccessResult(_PLCCommand);
        }

        /// <summary>
        /// 生成一个写入位数据的指令
        /// </summary>
        /// <param name="address">起始地址，示例M100,I100,Q100,DB1.100</param>
        /// <param name="data">是否通断</param>
        /// <returns>包含结果对象的报文</returns>
        public static M_OperateResult<byte[]> BuildWriteBitCommand(string address, bool data)
        {
            M_OperateResult<S7AddressData> analysis = S7AddressData.ParseFrom(address);
            if (!analysis.IsSuccess) return M_OperateResult.CreateFailedResult<byte[]>(analysis);

            byte[] buffer = new byte[1];
            buffer[0] = data ? (byte)0x01 : (byte)0x00;

            byte[] _PLCCommand = new byte[35 + buffer.Length];
            _PLCCommand[0] = 0x03;
            _PLCCommand[1] = 0x00;
            // 长度 -> length
            _PLCCommand[2] = (byte)((35 + buffer.Length) / 256);
            _PLCCommand[3] = (byte)((35 + buffer.Length) % 256);
            // 固定 -> fixed
            _PLCCommand[4] = 0x02;
            _PLCCommand[5] = 0xF0;
            _PLCCommand[6] = 0x80;
            _PLCCommand[7] = 0x32;
            // 命令 发 -> command to send
            _PLCCommand[8] = 0x01;
            // 标识序列号 -> Identification serial Number
            _PLCCommand[9] = 0x00;
            _PLCCommand[10] = 0x00;
            _PLCCommand[11] = 0x00;
            _PLCCommand[12] = 0x01;
            // 固定 -> fixed
            _PLCCommand[13] = 0x00;
            _PLCCommand[14] = 0x0E;
            // 写入长度+4 -> Write Length +4
            _PLCCommand[15] = (byte)((4 + buffer.Length) / 256);
            _PLCCommand[16] = (byte)((4 + buffer.Length) % 256);
            // 命令起始符 -> Command start character
            _PLCCommand[17] = 0x05;
            // 写入数据块个数 -> Number of data blocks written
            _PLCCommand[18] = 0x01;
            _PLCCommand[19] = 0x12;
            _PLCCommand[20] = 0x0A;
            _PLCCommand[21] = 0x10;
            // 写入方式，1是按位，2是按字 -> Write mode, 1 is bitwise, 2 is by word
            _PLCCommand[22] = 0x01;
            // 写入数据的个数 -> Number of Write Data
            _PLCCommand[23] = (byte)(buffer.Length / 256);
            _PLCCommand[24] = (byte)(buffer.Length % 256);
            // DB块编号，如果访问的是DB块的话 -> DB block number, if you are accessing a DB block
            _PLCCommand[25] = (byte)(analysis.Content.DbBlock / 256);
            _PLCCommand[26] = (byte)(analysis.Content.DbBlock % 256);
            // 写入数据的类型 -> Types of writing data
            _PLCCommand[27] = analysis.Content.DataCode;
            // 偏移位置 -> Offset position
            _PLCCommand[28] = (byte)(analysis.Content.AddressStart / 256 / 256);
            _PLCCommand[29] = (byte)(analysis.Content.AddressStart / 256);
            _PLCCommand[30] = (byte)(analysis.Content.AddressStart % 256);
            // 按位写入 -> Bitwise Write
            if (analysis.Content.DataCode == 0x1C)
            {
                _PLCCommand[31] = 0x00;
                _PLCCommand[32] = 0x09;
            }
            else
            {
                _PLCCommand[31] = 0x00;
                _PLCCommand[32] = 0x03;
            }
            // 按位计算的长度 -> The length of the bitwise calculation
            _PLCCommand[33] = (byte)(buffer.Length / 256);
            _PLCCommand[34] = (byte)(buffer.Length % 256);

            buffer.CopyTo(_PLCCommand, 35);

            return M_OperateResult.CreateSuccessResult(_PLCCommand);
        }


        private static M_OperateResult<byte[]> AnalysisReadByte(S7AddressData[] s7Addresses, byte[] content)
        {
            // 分析结果 -> Analysis results
            int receiveCount = 0;
            for (int i = 0; i < s7Addresses.Length; i++)
            {
                if (s7Addresses[i].DataCode == 0x1F || s7Addresses[i].DataCode == 0x1E)
                    receiveCount += s7Addresses[i].Length * 2;
                else
                    receiveCount += s7Addresses[i].Length;
            }

            if (content.Length >= 21 && content[20] == s7Addresses.Length)
            {
                byte[] buffer = new byte[receiveCount];
                int kk = 0;
                int ll = 0;
                for (int ii = 21; ii < content.Length; ii++)
                {
                    if ((ii + 1) < content.Length)
                    {
                        if (content[ii] == 0xFF && content[ii + 1] == 0x04)
                        {
                            Array.Copy(content, ii + 4, buffer, ll, s7Addresses[kk].Length);
                            ii += s7Addresses[kk].Length + 3;
                            ll += s7Addresses[kk].Length;
                            kk++;
                        }
                        else if (content[ii] == 0xFF && content[ii + 1] == 0x09)
                        {
                            int count = content[ii + 2] * 256 + content[ii + 3];
                            if (count % 3 == 0)
                            {
                                for (int i = 0; i < count / 3; i++)
                                {
                                    Array.Copy(content, ii + 5 + 3 * i, buffer, ll, 2);
                                    ll += 2;
                                }
                            }
                            else
                            {
                                for (int i = 0; i < count / 5; i++)
                                {
                                    Array.Copy(content, ii + 7 + 5 * i, buffer, ll, 2);
                                    ll += 2;
                                }
                            }
                            ii += count + 4;
                            kk++;
                        }
                        else if (content[ii] == 0x05 &&
                            content[ii + 1] == 0x00)
                        {
                            return new M_OperateResult<byte[]>(content[ii], CommonResources.Get.Language.Language.ipaddresserror);
                        }
                        else if (content[ii] == 0x06 &&
                            content[ii + 1] == 0x00)
                        {
                            return new M_OperateResult<byte[]>(content[ii], CommonResources.Get.Language.Language.ipaddresserror);
                        }
                        else if (content[ii] == 0x0A &&
                            content[ii + 1] == 0x00)
                        {
                            return new M_OperateResult<byte[]>(content[ii], CommonResources.Get.Language.Language.ipaddresserror);
                        }
                    }
                }
                return M_OperateResult.CreateSuccessResult(buffer);
            }
            else
            {
                return new M_OperateResult<byte[]>(CommonResources.Get.Language.Language.ipaddresserror + " Msg:" + ToolBasic.Byte2HexString(content, ' '));
            }
        }

        private static M_OperateResult<byte[]> AnalysisReadBit(byte[] content)
        {
            int receiveCount = 1;
            if (content.Length >= 21 && content[20] == 1)
            {
                byte[] buffer = new byte[receiveCount];
                if (22 < content.Length)
                {
                    if (content[21] == 0xFF &&
                        content[22] == 0x03)
                    {
                        buffer[0] = content[25];
                    }
                }

                return M_OperateResult.CreateSuccessResult(buffer);
            }
            else
            {
                return new M_OperateResult<byte[]>(CommonResources.Get.Language.Language.ipaddresserror);
            }
        }

        private static M_OperateResult AnalysisWrite(byte[] content)
        {
            byte code = content[content.Length - 1];
            if (code != 0xFF)
                return new M_OperateResult(code, CommonResources.Get.Language.Language.ipaddresserror + code + " Msg:" + ToolBasic.Byte2HexString(content, ' '));
            else
                return M_OperateResult.CreateSuccessResult();
        }

        #endregion

        public override string ToString() => $"SiemensS7 {CurrentPlc}[{ip}:{port}]";

    }
}
