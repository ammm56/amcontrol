using ProtocolLib.CommonLib.Interface;
using ProtocolLib.CommonLib.Model;
using ProtocolLib.CommonLib.Model.Net;
using ProtocolLib.S7Tcp.Common;
using ProtocolLib.S7Tcp.Core;
using ProtocolLib.S7Tcp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolLib.S7Tcp
{
    public class Protocol:IProtocol
    {
        /// <summary>
        /// 协议名
        /// </summary>
        public static readonly string ProtocolName = "s71200tcp";

        /// <summary>
        /// 协议
        /// </summary>
        private SiemensS7 _siementS7 = null;

        /// <summary>
        /// 连接
        /// </summary>
        private M_OperateResult _connect = null;

        /// <summary>
        /// 配置
        /// </summary>
        private M_ProtocolConfig _protocolConfig = null;

        /// <summary>
        /// 采集结果信息
        /// </summary>
        private List<M_GatherData> _gatherDatas = new List<M_GatherData>();

        private CollectionUtil _collectionUtil = new CollectionUtil();

        public Protocol() { }

        public int Init()
        {
            try
            {
                if (_siementS7 == null)
                {
                    _siementS7 = new SiemensS7(E_SiemensPLCS.S1200,_protocolConfig.ip, _protocolConfig.port);
                }

                _connect = _siementS7.Connection();
                if (!_connect.IsSuccess)
                {
                    Console.WriteLine($"连接错误 {_connect.Message}");
                    return 1;
                }

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"初始化错误，异常={ex.Message}");
                return -1;
            }
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="address"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public M_Return<M_GatherData> Get(string address, string type)
        {
            try
            {
                M_Return<M_GatherData> res = _collectionUtil.ReadData(_siementS7, "", address, type);
                if (!res.Status)
                {
                    return M_Return<M_GatherData>.Error(res.DescMsg);
                }

                return M_Return<M_GatherData>.OK(res.Result);
            }
            catch (Exception ex)
            {
                return M_Return<M_GatherData>.Error($"读取错误 {ex.Message}");
            }
        }
        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="address"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public M_Return<M_GatherData> Set(string address, string type, object value)
        {
            try
            {
                M_Return<M_GatherData> res = _collectionUtil.WriteData(_siementS7, address, value, type);
                if (!res.Status)
                {
                    return M_Return<M_GatherData>.Error(res.DescMsg);
                }

                return M_Return<M_GatherData>.OK(res.Result);
            }
            catch (Exception ex)
            {
                return M_Return<M_GatherData>.Error($"写入错误 {ex.Message}");
            }
        }

        /// <summary>
        /// 更新设置配置
        /// </summary>
        /// <param name="netconfig"></param>
        /// <returns></returns>
        public int SetCFG(M_NetConfig netconfig)
        {
            M_ProtocolConfig newconfig = new M_ProtocolConfig
            {
                equipmentid = netconfig.equipmentid,
                protocoltype = netconfig.protocoltype,
                ip = netconfig.ip,
                port = netconfig.port,
                byteorder = netconfig.byteorder,
                pointinfo = netconfig.pointinfo == null ? new List<Point>() : new List<Point>(netconfig.pointinfo)
            };

            if (_siementS7 != null && (newconfig.ip != _protocolConfig.ip || newconfig.port != _protocolConfig.port))
            {
                _protocolConfig = newconfig;
                _siementS7.UpdateIPPortInfo(_protocolConfig.ip, _protocolConfig.port);

                var reres = Reconnect();
                if (!reres.Status)
                {
                    return 1;
                }
            }
            _protocolConfig = newconfig;
            if (_siementS7 != null)
            {
                _siementS7.UpdateIPPortInfo(_protocolConfig.ip, _protocolConfig.port);
            }
            M_Return<List<Point>> res = _collectionUtil.DecodePoints4Rule(ref this._protocolConfig);

            if (res.Status) return 0;
            return 1;
        }

        /// <summary>
        /// 重连
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public M_Return<string> Reconnect()
        {
            try
            {
                var closeres = CloseConnected();
                if (closeres.Status)
                {
                    if (_siementS7 == null)
                    {
                        _siementS7 = new SiemensS7(E_SiemensPLCS.S1200,_protocolConfig.ip, _protocolConfig.port);
                    }
                    _connect = _siementS7.Connection();
                    if (!_connect.IsSuccess)
                    {
                        return M_Return<string>.Error("重新连接错误");
                    }
                    return M_Return<string>.OK("重新连接成功");
                }
                return M_Return<string>.Error(closeres.DescMsg);
            }
            catch (Exception ex)
            {
                return M_Return<string>.Error($"重新连接错误，异常={ex.Message}");
            }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <returns></returns>
        public M_Return<string> CloseConnected()
        {
            try
            {
                if (_connect != null && _connect.IsSuccess)
                {
                    M_OperateResult res = _siementS7.ConnectClose();
                    if (res.IsSuccess) return M_Return<string>.OK("关闭连接成功");
                    else return M_Return<string>.Error("关闭连接错误");
                }
                return M_Return<string>.OK("连接已关闭");
            }
            catch (Exception ex)
            {
                return M_Return<string>.Error($"关闭连接错误，异常={ex.Message}");
            }
        }





    }
}
