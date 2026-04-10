using NUnit.Framework;
using ProtocolLib.CommonLib.Interface;
using ProtocolLib.CommonLib.Model;
using ProtocolLib.CommonLib.Model.Net;
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
            public const string Bool = "00000";

            public const string Int16 = "40000";
            public const string UInt16 = "40001";

            public const string Int32 = "40010";
            public const string UInt32 = "40012";

            public const string Int64 = "40020";
            public const string UInt64 = "40024";

            public const string Single = "40030";
            public const string Double = "40032";

            public const string StringFixed20 = "40040[20]";

            public const string CalcLeft = "40050";
            public const string CalcRight = "40051";
            public const string CalcResult = "40052";

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

        private static M_NetConfig CreateConfig()
        {
            return new M_NetConfig
            {
                equipmentid = "1",
                protocoltype = "modbustcp",
                ip = "127.0.0.1",
                port = 502
            };
        }

        private static IProtocol CreateAndInitProtocol()
        {
            IProtocol protocol = CreateProtocol();

            int setCfgResult = protocol.SetCFG(CreateConfig());
            Assert.That(setCfgResult, Is.EqualTo(0), "SetCFG 失败");

            int initResult = protocol.Init();
            Assert.That(initResult, Is.EqualTo(0), "Init 失败");

            return protocol;
        }

        private static void CloseProtocol(IProtocol protocol)
        {
            if (protocol == null) return;

            M_Return<string> closeResult = protocol.CloseConnected();
            Assert.That(closeResult, Is.Not.Null);
            Assert.That(closeResult.Status, Is.True, closeResult.DescMsg);
        }

        private static M_Return<M_GatherData> WriteSuccess(IProtocol protocol, string address, string type, object value)
        {
            M_Return<M_GatherData> result = protocol.Set(address, type, value);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.True, result.DescMsg);
            Assert.That(result.Result, Is.Not.Null);
            Assert.That(result.Result.type, Is.EqualTo(type));
            return result;
        }

        private static M_Return<M_GatherData> ReadSuccess(IProtocol protocol, string address, string type)
        {
            M_Return<M_GatherData> result = protocol.Get(address, type);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.True, result.DescMsg);
            Assert.That(result.Result, Is.Not.Null);
            Assert.That(result.Result.type, Is.EqualTo(type));
            return result;
        }

        private static void AssertRoundTrip(IProtocol protocol, string address, string type, object writeValue, string expectedReadValue)
        {
            WriteSuccess(protocol, address, type, writeValue);
            M_Return<M_GatherData> readResult = ReadSuccess(protocol, address, type);
            AssertValue(type, expectedReadValue, readResult.Result.value);
        }

        private static void AssertValue(string type, string expected, string actual)
        {
            switch (type)
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
                    Assert.Fail("未处理的数据类型断言: " + type);
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
        public void Should_Init_And_Close_ModbusTcp_Protocol()
        {
            IProtocol protocol = CreateAndInitProtocol();
            CloseProtocol(protocol);
        }

        [Test]
        [Explicit("需要本地存在可访问的 Modbus TCP 服务，且以下地址可读写：00000,40000-40060，字符串地址使用 40040[20]")]
        public void Should_Read_And_Write_All_Supported_DataTypes()
        {
            IProtocol protocol = CreateAndInitProtocol();

            try
            {
                AssertRoundTrip(protocol, TestAddress.Bool, "bool", true, "1");

                AssertRoundTrip(protocol, TestAddress.Int16, "int16", (short)-1234, "-1234");
                AssertRoundTrip(protocol, TestAddress.UInt16, "uint16", (ushort)1234, "1234");

                AssertRoundTrip(protocol, TestAddress.Int32, "int32", -123456, "-123456");
                AssertRoundTrip(protocol, TestAddress.UInt32, "uint32", 123456u, "123456");

                AssertRoundTrip(protocol, TestAddress.Int64, "int64", -1234567890L, "-1234567890");
                AssertRoundTrip(protocol, TestAddress.UInt64, "uint64", 1234567890UL, "1234567890");

                AssertRoundTrip(protocol, TestAddress.Single, "single", 12.5f, "12.5");
                AssertRoundTrip(protocol, TestAddress.Double, "double", 123.125d, "123.125");

                AssertRoundTrip(protocol, TestAddress.StringFixed20, "string", "AM_MODBUS", "AM_MODBUS");
            }
            finally
            {
                CloseProtocol(protocol);
            }
        }

        [Test]
        [Explicit("需要本地存在可访问的 Modbus TCP 服务，且 40050-40052 可读写")]
        public void Should_Support_TypedValue_Compare_And_Basic_Calculation()
        {
            IProtocol protocol = CreateAndInitProtocol();

            try
            {
                WriteSuccess(protocol, TestAddress.CalcLeft, "uint16", (ushort)8);
                WriteSuccess(protocol, TestAddress.CalcRight, "uint16", (ushort)2);

                M_Return<M_GatherData> leftRead = ReadSuccess(protocol, TestAddress.CalcLeft, "uint16");
                M_Return<M_GatherData> rightRead = ReadSuccess(protocol, TestAddress.CalcRight, "uint16");

                M_TypedValue left = leftRead.Result.TypedValue;
                M_TypedValue right = rightRead.Result.TypedValue;

                Assert.That(left > right, Is.True);
                Assert.That(left >= right, Is.True);
                Assert.That(left < right, Is.False);
                Assert.That(left <= right, Is.False);
                Assert.That(left == right, Is.False);
                Assert.That(left != right, Is.True);

                M_TypedValue addValue = left + right;
                M_TypedValue subValue = left - right;
                M_TypedValue mulValue = left * right;
                M_TypedValue divValue = left / right;

                Assert.That(addValue.type, Is.EqualTo("uint16"));
                Assert.That(addValue.value, Is.EqualTo("10"));
                Assert.That(subValue.value, Is.EqualTo("6"));
                Assert.That(mulValue.value, Is.EqualTo("16"));
                Assert.That(divValue.value, Is.EqualTo("4"));

                M_Return<M_GatherData> writeResult = WriteSuccess(protocol, TestAddress.CalcResult, "uint16", addValue);
                Assert.That(writeResult.Result.value, Is.EqualTo("10"));

                M_Return<M_GatherData> readBack = ReadSuccess(protocol, TestAddress.CalcResult, "uint16");
                Assert.That(readBack.Result.value, Is.EqualTo(addValue.value), "计算结果写入后回读不一致");
            }
            finally
            {
                CloseProtocol(protocol);
            }
        }

        [Test]
        [Explicit("需要本地存在可访问的 Modbus TCP 服务，且 40060 可读写")]
        public void Should_Reconnect_And_Continue_Read_Write()
        {
            IProtocol protocol = CreateAndInitProtocol();

            try
            {
                WriteSuccess(protocol, TestAddress.Reconnect, "uint16", (ushort)99);
                M_Return<M_GatherData> beforeReconnect = ReadSuccess(protocol, TestAddress.Reconnect, "uint16");
                Assert.That(beforeReconnect.Result.value, Is.EqualTo("99"));

                M_Return<string> reconnectResult = protocol.Reconnect();
                Assert.That(reconnectResult, Is.Not.Null);
                Assert.That(reconnectResult.Status, Is.True, reconnectResult.DescMsg);

                M_Return<M_GatherData> afterReconnectRead = ReadSuccess(protocol, TestAddress.Reconnect, "uint16");
                Assert.That(afterReconnectRead.Result.value, Is.EqualTo("99"));

                M_TypedValue increment = new M_TypedValue
                {
                    type = "uint16",
                    value = "1"
                };

                M_TypedValue expectedValue = afterReconnectRead.Result.TypedValue + increment;
                WriteSuccess(protocol, TestAddress.Reconnect, "uint16", expectedValue);

                M_Return<M_GatherData> finalRead = ReadSuccess(protocol, TestAddress.Reconnect, "uint16");
                Assert.That(finalRead.Result.value, Is.EqualTo(expectedValue.value));
            }
            finally
            {
                CloseProtocol(protocol);
            }
        }

        [Test]
        [Explicit("需要本地存在可访问的 Modbus TCP 服务，且 40040[20] 可读写")]
        public void Should_Read_And_Write_Fixed_Length_String_By_Address_Length()
        {
            IProtocol protocol = CreateAndInitProtocol();

            try
            {
                const string value = "HELLO_MODBUS_STRING";
                WriteSuccess(protocol, TestAddress.StringFixed20, "string", value);

                M_Return<M_GatherData> readResult = ReadSuccess(protocol, TestAddress.StringFixed20, "string");
                Assert.That(readResult.Result.value, Is.EqualTo(value));

                const string tooLongValue = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                WriteSuccess(protocol, TestAddress.StringFixed20, "string", tooLongValue);

                M_Return<M_GatherData> truncatedReadResult = ReadSuccess(protocol, TestAddress.StringFixed20, "string");
                Assert.That(truncatedReadResult.Result.value, Is.EqualTo("ABCDEFGHIJKLMNOPQRST"));
            }
            finally
            {
                CloseProtocol(protocol);
            }
        }
    }
}