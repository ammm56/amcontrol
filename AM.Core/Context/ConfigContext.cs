using AM.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Core.Context
{
    public sealed class ConfigContext
    {
        public static ConfigContext Instance { get; } = new ConfigContext();

        private ConfigContext() { }

        public Config Config { get; private set; }

        public void Initialize(Config config)
        {
            Config = config;
        }
    }
}
