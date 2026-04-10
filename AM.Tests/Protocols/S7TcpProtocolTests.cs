using NUnit.Framework;
using ProtocolLib.CommonLib.Interface;
using ProtocolLib.CommonLib.Model;
using ProtocolLib.S7Tcp;
using System;
using System.Globalization;

namespace AM.Tests.Protocols
{
    [TestFixture]
    public class S7TcpProtocolTests
    {
        private static class TestAddress
        {
            public const string UInt16Read = "DB1.0";
            public const string UInt16Write = "DB1.2";
            public const string StringFixed20 = "DB1.20[20]";
            public const string Reconnect = "DB1.4";
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
                protocolType = "s71200tcp",
                connectionType = "tcp",
                ip = "127.0.0.1",
                port = 102,
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

        private static M_Return<M_PointData> WriteSuccess(IProtocol protocol, string address, string dataType, object value, int stringLength = 0, int arrayLength = 0)
        {
            M_Return<M_PointData> result = protocol.WritePoint(new M_PointWriteRequest
            {
                address = address,
                dataType = dataType,
                value = value,
                stringLength = stringLength,
                arrayLength = arrayLength
            });

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.True, result.DescMsg);
            Assert.That(result.Result, Is.Not.Null);
            Assert.That(result.Result.dataType, Is.EqualTo(dataType));
            return result;
        }

        private static M_Return<M_PointData> ReadSuccess(IProtocol protocol, string address, string dataType, int stringLength = 0, int arrayLength = 0)
        {
            M_Return<M_PointData> result = protocol.ReadPoint(new M_PointReadRequest
            {
                address = address,
                dataType = dataType,
                stringLength = stringLength,
                arrayLength = arrayLength
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
            int stringLength = 0,
            int arrayLength = 0)
        {
            WriteSuccess(protocol, address, dataType, writeValue, stringLength, arrayLength);
            M_Return<M_PointData> readResult = ReadSuccess(protocol, address, dataType, stringLength, arrayLength);
            AssertValue(dataType, expectedReadValue, readResult.Result.value);
        }

        private static void AssertValue(string dataType, string expected, string actual)
        {
            switch (dataType)
            {
                case "single":
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
                    Assert.That(actual, Is.EqualTo(expected));
                    break;
            }
        }

        [Test]
        public void Should_Create_S7Tcp_Protocol_By_Reflection()
        {
            IProtocol protocol = CreateProtocol();
            Assert.That(protocol, Is.Not.Null);
        }

        [Test]
        [Explicit("需要本地存在可访问的西门子 S7 服务，默认使用 127.0.0.1:102")]
        public void Should_Configure_Connect_And_Disconnect_S7Tcp_Protocol()
        {
            IProtocol protocol = CreateAndConnectProtocol();
            CloseProtocol(protocol);
        }

        [Test]
        [Explicit("需要本地存在可访问的西门子 S7 服务，且 DB1.0、DB1.2、DB1.20[20] 可读写")]
        public void Should_Read_And_Write_S7Tcp_Points()
        {
            IProtocol protocol = CreateAndConnectProtocol();

            try
            {
                M_Return<M_PointData> readResult = ReadSuccess(protocol, TestAddress.UInt16Read, "uint16");
                Assert.That(readResult.Result.value, Is.Not.Null);

                ushort writeValue;
                Assert.That(ushort.TryParse(readResult.Result.value, out writeValue), Is.True, "读取值无法转换为 uint16");

                AssertRoundTrip(protocol, TestAddress.UInt16Write, "uint16", writeValue, writeValue.ToString(CultureInfo.InvariantCulture));

                const string value = "HELLO_S7_STRING";
                AssertRoundTrip(protocol, TestAddress.StringFixed20, "string", value, value, 20, 0);

                const string tooLongValue = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                WriteSuccess(protocol, TestAddress.StringFixed20, "string", tooLongValue, 20, 0);

                M_Return<M_PointData> truncatedReadResult = ReadSuccess(protocol, TestAddress.StringFixed20, "string", 20, 0);
                Assert.That(truncatedReadResult.Result.value, Is.EqualTo("ABCDEFGHIJKLMNOPQRST"));
            }
            finally
            {
                CloseProtocol(protocol);
            }
        }

        [Test]
        [Explicit("需要本地存在可访问的西门子 S7 服务，且 DB1.4 可读写")]
        public void Should_Support_S7Tcp_Reconnect()
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