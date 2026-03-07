using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM.Model.Common;

namespace AM.Tools
{
    /// <summary>
    /// 配置文件单例
    /// </summary>
    public sealed class ConfigSingle
    {
        private static readonly ConfigSingle configSingle = null;
        private ConfigSingle() { }
        static ConfigSingle()
        {
            var temp = Tools.ReadConfig<Config>("config.json");
            configSingle = new ConfigSingle
            {
                Config = temp.Item1 ? temp.Item2 : new Config()
            };
            OSInfo.GetOSInfo();
        }
        public static ConfigSingle Instance
        {
            get
            {
                return configSingle;
            }
        }
        public Config Config;
    }
}
