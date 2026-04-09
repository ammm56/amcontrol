using ProtocolLib.CommonLib.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolLib.CommonLib.Model.Language
{
    public class M_Language
    {
        public M_Language()
        {
            if (CultureInfo.CurrentCulture.ToString().ToLower().StartsWith("zh"))
            {
                SetZH();
            }
            else
            {
                SetEN();
            }
        }

        public Ilanguage Language = new zh();
        public void SetZH() => Language = new zh();
        public void SetEN() => Language = new en();

    }
}
