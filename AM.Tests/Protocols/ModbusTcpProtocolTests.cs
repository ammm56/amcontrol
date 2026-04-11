using NUnit.Framework;
using ProtocolLib.CommonLib.Interface;
using ProtocolLib.CommonLib.Model;
using ProtocolLib.ModbusTcp;
using System;
using System.Globalization;

namespace AM.Tests.Protocols
{
    [TestFixture]
    public class ModbusTcpProtocolTests
    {
        private static class TestAddress
        {
            public const string Bool = "00001";
            public const string Int16 = "40000";
            public const string UInt16 = "40001";
            public const string Int32 = "40010";
            public const string UInt32 = "40012";
            public const string Int64 = "40020";
            public const string UInt64 = "40024";
            public const string Float = "40030";
            public const string Double = "40032";
            public const string StringFixed20 = "40040[20]";
            public const string Reconnect = "40060";
        }

        private static IProtocol CreateProtocol()
        {
            Type protocolType = typeof(Protocol);
            object instance = Activator.CreateInstance(protocolType);

            Assert.That(instance, Is.Not.Null, "Protocol 实例创建失败");

            IProtocol protocol = instance as IProtocol;
            Assert.That(protocol, Is.Not.Null, "Protocol 未实现 IProtocol");

            return protocol;
        }

        private static M_ProtocolOptions CreateConfig()
        {
            return new M_ProtocolOptions
            {
                protocolType = "modbustcp",
                connectionType = "tcp",
                ip = "127.0.0.1",
                port = 502,
                stationNo = 1,
                timeoutMs = 1000
            };
        }

        private static IProtocol CreateAndConnectProtocol()
        {
            IProtocol protocol = CreateProtocol();

            M_Return<bool> configureResult = protocol.Configure(CreateConfig());
            Assert.That(configureResult, Is.Not.Null);
            Assert.That(configureResult.Status, Is.True, configureResult.DescMsg);

            M_Return<bool> connectResult = protocol.Connect();
            Assert.That(connectResult, Is.Not.Null);
            Assert.That(connectResult.Status, Is.True, connectResult.DescMsg);

            return protocol;
        }

        private static void CloseProtocol(IProtocol protocol)
        {
            if (protocol == null)
            {
                return;
            }

            M_Return<bool> closeResult = protocol.Disconnect();
            Assert.That(closeResult, Is.Not.Null);
            Assert.That(closeResult.Status, Is.True, closeResult.DescMsg);
        }

        private static M_Return<M_PointData> WriteSuccess(IProtocol protocol, string address, string dataType, object value, int length = 0)
        {
            M_Return<M_PointData> result = protocol.WritePoint(new M_PointWriteRequest
            {
                address = address,
                dataType = dataType,
                value = value,
                length = length
            });

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.True, result.DescMsg);
            Assert.That(result.Result, Is.Not.Null);
            Assert.That(result.Result.dataType, Is.EqualTo(dataType));
            return result;
        }

        private static M_Return<M_PointData> ReadSuccess(IProtocol protocol, string address, string dataType, int length = 0)
        {
            M_Return<M_PointData> result = protocol.ReadPoint(new M_PointReadRequest
            {
                address = address,
                dataType = dataType,
                length = length
            });

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.True, result.DescMsg);
            Assert.That(result.Result, Is.Not.Null);
            Assert.That(result.Result.dataType, Is.EqualTo(dataType));
            return result;
        }

        private static void AssertRoundTrip(
            IProtocol protocol,
            string address,
            string dataType,
            object writeValue,
            string expectedReadValue,
            int length = 0)
        {
            WriteSuccess(protocol, address, dataType, writeValue, length);
            M_Return<M_PointData> readResult = ReadSuccess(protocol, address, dataType, length);
            AssertValue(dataType, expectedReadValue, readResult.Result.value);
        }

        private static void AssertValue(string dataType, string expected, string actual)
        {
            switch (dataType)
            {
                case "bool":
                case "string":
                case "int16":
                case "uint16":
                case "int32":
                case "uint32":
                case "int64":
                case "uint64":
                    Assert.That(actual, Is.EqualTo(expected));
                    break;

                case "float":
                    Assert.That(
                        Math.Abs(float.Parse(actual, CultureInfo.InvariantCulture) - float.Parse(expected, CultureInfo.InvariantCulture)),
                        Is.LessThan(0.0001f));
                    break;

                case "double":
                    Assert.That(
                        Math.Abs(double.Parse(actual, CultureInfo.InvariantCulture) - double.Parse(expected, CultureInfo.InvariantCulture)),
                        Is.LessThan(0.0000001d));
                    break;

                default:
                    Assert.Fail("未处理的数据类型断言: " + dataType);
                    break;
            }
        }

        [Test]
        public void Should_Create_ModbusTcp_Protocol_By_Reflection()
        {
            IProtocol protocol = CreateProtocol();
            Assert.That(protocol, Is.Not.Null);
        }

        [Test]
        [Explicit("需要本地存在可访问的 Modbus TCP 服务，默认使用 127.0.0.1:502")]
        public void Should_Configure_Connect_And_Disconnect_ModbusTcp_Protocol()
        {
            IProtocol protocol = CreateAndConnectProtocol();
            CloseProtocol(protocol);
        }

        [Test]
        [Explicit("需要本地存在可访问的 Modbus TCP 服务，且以下地址可读写：00001,40000-40060，字符串地址使用 40040[20]")]
        public void Should_Read_And_Write_All_Supported_DataTypes()
        {
            IProtocol protocol = CreateAndConnectProtocol();

            try
            {
                AssertRoundTrip(protocol, TestAddress.Bool, "bool", true, "1");
                AssertRoundTrip(protocol, TestAddress.Int16, "int16", (short)-1234, "-1234");
                AssertRoundTrip(protocol, TestAddress.UInt16, "uint16", (ushort)1234, "1234");
                AssertRoundTrip(protocol, TestAddress.Int32, "int32", -123456, "-123456");
                AssertRoundTrip(protocol, TestAddress.UInt32, "uint32", 123456u, "123456");
                AssertRoundTrip(protocol, TestAddress.Int64, "int64", -1234567890L, "-1234567890");
                AssertRoundTrip(protocol, TestAddress.UInt64, "uint64", 1234567890UL, "1234567890");
                AssertRoundTrip(protocol, TestAddress.Float, "float", 12.5f, "12.5");
                AssertRoundTrip(protocol, TestAddress.Double, "double", 123.125d, "123.125");
                AssertRoundTrip(protocol, TestAddress.StringFixed20, "string", "AM_MODBUS", "AM_MODBUS", 20);
            }
            finally
            {
                CloseProtocol(protocol);
            }
        }

        [Test]
        [Explicit("需要本地存在可访问的 Modbus TCP 服务，且 40060 可读写")]
        public void Should_Support_Reconnect()
        {
            IProtocol protocol = CreateAndConnectProtocol();

            try
            {
                M_Return<M_PointData> writeResult = WriteSuccess(protocol, TestAddress.Reconnect, "uint16", (ushort)88);
                Assert.That(writeResult.Result.value, Is.Not.Null);

                M_Return<bool> reconnectResult = protocol.Reconnect();
                Assert.That(reconnectResult, Is.Not.Null);
                Assert.That(reconnectResult.Status, Is.True, reconnectResult.DescMsg);

                M_Return<M_PointData> readResult = ReadSuccess(protocol, TestAddress.Reconnect, "uint16");
                Assert.That(readResult.Result.value, Is.EqualTo("88"));
            }
            finally
            {
                CloseProtocol(protocol);
            }
        }
    }
}