using NUnit.Framework;
using ProtocolLib.CommonLib.Interface;
using ProtocolLib.CommonLib.Model;
using ProtocolLib.CommonLib.Model.Net;
using System;

namespace AM.Tests.Protocols
{
    [TestFixture]
    public class S7TcpProtocolTests
    {
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
            IProtocol protocol = CreateProtocol();

            int setCfgResult = protocol.SetCFG(CreateConfig());
            Assert.That(setCfgResult, Is.EqualTo(0), "SetCFG 失败");

            int initResult = protocol.Init();
            Assert.That(initResult, Is.EqualTo(0), "Init 失败");

            M_Return<M_GatherData> readResult = protocol.Get("DB1.0", "uint16");
            Assert.That(readResult, Is.Not.Null);
            Assert.That(readResult.Status, Is.True, readResult.DescMsg);

            M_Return<M_GatherData> writeResult = protocol.Set("DB1.2", "uint16", readResult.Result.value);
            Assert.That(writeResult, Is.Not.Null);
            Assert.That(writeResult.Status, Is.True, writeResult.DescMsg);

            M_Return<string> closeResult = protocol.CloseConnected();
            Assert.That(closeResult, Is.Not.Null);
            Assert.That(closeResult.Status, Is.True, closeResult.DescMsg);
        }
    }
}