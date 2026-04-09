using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolLib.CommonLib.Interface
{
    /// <summary>
    /// 语言
    /// </summary>
    public interface Ilanguage
    {
        #region 一般信息

        string successtext { get; set; }
        string unknownerror { get; set; }
        string connecttime { get; set; }
        string connectex { get; set; }
        string remoteconnectclose { get; set; }
        string ipaddresserror { get; set; }
        string notsupportedfunction { get; set; }
        string waitdatatimeout { get; set; }

        #endregion

        #region 日志

        string lognone { get; set; }
        string logdebug { get; set; }
        string loginfo { get; set; }
        string logwarn { get; set; }
        string logerror { get; set; }
        string logfatal { get; set; }

        #endregion
    }
}
