using NUnit.Framework;
using ProtocolLib.CommonLib.Interface;
using ProtocolLib.CommonLib.Model;
using ProtocolLib.CommonLib.Model.Net;
using ProtocolLib.ModbusTcp;
using System;

namespace AM.Tests.Protocols
{
    [TestFixture]
    public class ModbusTcpProtocolTests
    {
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

        [Test]
        public void Should_Create_ModbusTcp_Protocol_By_Reflection()
        {
            IProtocol protocol = CreateProtocol();
            Assert.That(protocol, Is.Not.Null);
        }

        [Test]
        [Explicit("需要本地存在可访问的 Modbus TCP 服务，默认使用 127.0.0.1:502")]
        public void Should_Init_And_Read_Write_Once()
        {
            IProtocol protocol = CreateProtocol();

            int setCfgResult = protocol.SetCFG(CreateConfig());
            Assert.That(setCfgResult, Is.EqualTo(0), "SetCFG 失败");

            int initResult = protocol.Init();
            Assert.That(initResult, Is.EqualTo(0), "Init 失败");

            M_Return<M_GatherData> writeResultStart = protocol.Set("40001", "uint16", 1);
            Assert.That(writeResultStart, Is.Not.Null);
            Assert.That(writeResultStart.Status, Is.True, writeResultStart.DescMsg);

            M_Return<M_GatherData> readResult = protocol.Get("40001", "uint16");
            Assert.That(readResult, Is.Not.Null);
            Assert.That(readResult.Status, Is.True, readResult.DescMsg);

            M_Return<M_GatherData> readResult2 = protocol.Get("40010", "uint16");
            Assert.That(readResult2, Is.Not.Null);
            Assert.That(readResult2.Status, Is.True, readResult2.DescMsg);

            M_TypedValue expectedValue = readResult.Result.TypedValue + readResult2.Result.TypedValue;
            M_Return<M_GatherData> writeResult = protocol.Set("40010", "uint16", expectedValue);
            Assert.That(writeResult, Is.Not.Null);
            Assert.That(writeResult.Status, Is.True, writeResult.DescMsg);

            M_Return<M_GatherData> readResultRes = protocol.Get("40010", "uint16");
            Assert.That(readResultRes, Is.Not.Null);
            Assert.That(readResultRes.Status, Is.True, readResultRes.DescMsg);
            Assert.That(readResultRes.Result.type, Is.EqualTo("uint16"));
            Assert.That(readResultRes.Result.value, Is.EqualTo(expectedValue.value), "写入值与读取值不匹配");

            M_Return<string> closeResult = protocol.CloseConnected();
            Assert.That(closeResult, Is.Not.Null);
            Assert.That(closeResult.Status, Is.True, closeResult.DescMsg);
        }
    }
}