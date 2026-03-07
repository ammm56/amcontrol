using AM.DB.Tables;
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
    public partial class AxisTableInfo : Form
    {
        public AxisTableInfo()
        {
            InitializeComponent();
        }

        private void AxisTableInfo_Load(object sender, EventArgs e)
        {
            DBResponse<List<ConfigAxisArg>> res = new DBResponse<List<ConfigAxisArg>>
            {
                data = new DBTable<ConfigAxisArg>().Query()
            };
            res.message = JsonConvert.SerializeObject(res.data, Newtonsoft.Json.Formatting.Indented);

            dataGridView1.DataSource = res.data;
            // 格式化：隐藏 ID 等字段
            dataGridView1.Columns["Id"].Visible = false;
            dataGridView1.Columns["axis"].HeaderText = "轴编号";
            dataGridView1.Columns["paramname"].HeaderText = "参数名称";
            dataGridView1.Columns["paramname_cn"].HeaderText = "参数中文名称";
            dataGridView1.Columns["paramsetval"].HeaderText = "参数设置值";
            dataGridView1.Columns["paramdefaultval"].HeaderText = "参数默认值";
            dataGridView1.Columns["parammaxval"].HeaderText = "参数最大值";
            dataGridView1.Columns["paramminval"].HeaderText = "参数最小值";
            dataGridView1.Columns["paramstatus1"].HeaderText = "参数状态1";
            dataGridView1.Columns["paramstatus2"].HeaderText = "参数状态2";
            dataGridView1.Columns["paramstatus3"].HeaderText = "参数状态3";
            dataGridView1.Columns["description"].HeaderText = "描述";
            dataGridView1.Columns["remark"].Visible = false;
            dataGridView1.Columns["axis_cn"].Visible = false;
            dataGridView1.Columns["reserve1"].Visible = false;
            dataGridView1.Columns["reserve2"].Visible = false;
            dataGridView1.Columns["reserve3"].Visible = false;
            dataGridView1.Columns["reserve4"].Visible = false;
            dataGridView1.Columns["reserve5"].Visible = false;
        }
    }
}
