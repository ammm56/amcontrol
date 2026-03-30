using AM.Model.Common;
using AM.Model.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMControl
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btn_gsna_Click(object sender, EventArgs e)
        {
            GSNA gsna = new GSNA();
            gsna.Show();
        }

        private void btn_loadcfg_Click(object sender, EventArgs e)
        {
            CfgInfo cfginfo = new CfgInfo();
            cfginfo.Show();
        }

        private void btn_querydbaxistable_Click(object sender, EventArgs e)
        {
            AxisTableInfo axisTableInfo = new AxisTableInfo();
            axisTableInfo.Show();

        }
    }
}
