using ProtocolLib.CommonLib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolLib.CommonLib.Model.Language
{
    internal class en : Ilanguage
    {

        public string successtext { get; set; } = "成功";
        public string unknownerror { get; set; } = "未知错误";
        public string connecttime { get; set; } = "连接超时";
        public string connectex { get; set; } = "连接异常";
        public string remoteconnectclose { get; set; } = "远程连接关闭";
        public string ipaddresserror { get; set; } = "IP地址错误";
        public string notsupportedfunction { get; set; } = "不支持功能";
        public string waitdatatimeout { get; set; } = "等待指定数据超时";

        public string lognone { get; set; } = "None";
        public string logdebug { get; set; } = "Debug";
        public string loginfo { get; set; } = "Info";
        public string logwarn { get; set; } = "Warn";
        public string logerror { get; set; } = "Error";
        public string logfatal { get; set; } = "Fatal";
    }
}
