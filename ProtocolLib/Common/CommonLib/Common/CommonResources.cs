using ProtocolLib.CommonLib.Interface;
using ProtocolLib.CommonLib.Model.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolLib.CommonLib.Common
{
    public sealed class CommonResources
    {
        private static readonly CommonResources commonResources = null;
        private CommonResources() { }
        static CommonResources()
        {
            commonResources = new CommonResources()
            {
                Language = new M_Language()
            };
        }

        public static CommonResources Get
        {
            get
            {
                return commonResources;
            }
        }

        public M_Language Language;
    }
}
