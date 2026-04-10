using NUnit.Framework;
using ProtocolLib.CommonLib.Interface;
using ProtocolLib.CommonLib.Model;
using ProtocolLib.CommonLib.Model.Net;
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
        }

        private static IProtocol CreateProtocol()
        {
            Type protocolType = typeof(ProtocolLib.S7Tcp.Protocol);
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
                protocoltype = "s71200tcp",
                ip = "127.0.0.1",
                port = 102
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

        private static void AssertValue(string type, string expected, string actual)
        {
            switch (type)
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
        public void Should_Init_And_Read_Write_Once()
        {
            IProtocol protocol = CreateAndInitProtocol();

            try
            {
                M_Return<M_GatherData> readResult = ReadSuccess(protocol, TestAddress.UInt16Read, "uint16");

                M_TypedValue expectedValue = readResult.Result.TypedValue;
                M_Return<M_GatherData> writeResult = WriteSuccess(protocol, TestAddress.UInt16Write, "uint16", expectedValue);
                Assert.That(writeResult, Is.Not.Null);

                M_Return<M_GatherData> readBackResult = ReadSuccess(protocol, TestAddress.UInt16Write, "uint16");
                Assert.That(readBackResult.Result.value, Is.EqualTo(expectedValue.value), "写入值与读取值不匹配");
            }
            finally
            {
                CloseProtocol(protocol);
            }
        }

        [Test]
        [Explicit("需要本地存在可访问的西门子 S7 服务，且 DB1.20[20] 可读写")]
        public void Should_Read_And_Write_Fixed_Length_String_By_Address_Length()
        {
            IProtocol protocol = CreateAndInitProtocol();

            try
            {
                const string value = "HELLO_S7_STRING";
                WriteSuccess(protocol, TestAddress.StringFixed20, "string", value);

                M_Return<M_GatherData> readResult = ReadSuccess(protocol, TestAddress.StringFixed20, "string");
                AssertValue("string", value, readResult.Result.value);

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