using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AM.Core.Context;
using AM.Tools;
using Newtonsoft.Json;

namespace AMControl
{
    public partial class CfgInfo : Form
    {
        public CfgInfo()
        {
            InitializeComponent();
        }

        private void CfgInfo_Load(object sender, EventArgs e)
        {
            string cfgstr = JsonConvert.SerializeObject(ConfigContext.Instance.Config,Newtonsoft.Json.Formatting.Indented);

            lab_cfginfo.Text = cfgstr;


        }
    }
}
