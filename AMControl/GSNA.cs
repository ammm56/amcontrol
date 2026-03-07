using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GTN;

namespace AMControl
{
    public partial class GSNA : Form
    {
        public Timer timer1 = new Timer();

        public GSNA()
        {
            InitializeComponent();
        }

        public void Error(string message,short rtn)
        {
            if(rtn != 0)
            {
                string msg = $"Error {message} : {rtn}";
                Task.Run( () =>
                {
                    MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } );
                
            }
        }

        private void btn_init_Click(object sender, EventArgs e)
        {
            // 0 控制卡索引号,只插了一块控制卡，该值通常固定为 0。如果系统中有多个控制卡，则通过该索引（0, 1, 2...）来指定操作哪一块卡。
            // 1 配置模式, 1：表示使用配置文件初始化,按照文件里的参数（轴数、脉冲极性、限位开关极性等）直接配置硬件。
            // 取值为 0（可选）：通常表示不加载配置，仅打开控制卡。这种模式下，需要通过后续代码手动设置每一个硬件参数。
            short rtn = mc.GTN_Open(0, 1);
            if (rtn != 0)
            {
                Error("卡打开失败", rtn);
                return;
            }
            // 核心 1 (Core 1)：主要负责运动控制逻辑、指令解析和轨迹规划。
            // 核心 2(Core 2)：通常负责高速数据采集、特殊的比较输出（Compare）或逻辑控制。
            rtn = mc.GTN_Reset(1);
            rtn = mc.GTN_Reset(2);
            Error("卡复位", rtn);
            rtn = mc.GTN_LoadConfig(1, "gtn_core1.cfg");
            Error("加载配置", rtn);
        }

        private void btn_clearstatus_Click(object sender, EventArgs e)
        {
            short rtn = mc.GTN_ClrSts(1, 1, 4);
            Error("清除状态", rtn);
        }

        private void btn_enable_Click(object sender, EventArgs e)
        {
            short rtn = mc.GTN_AxisOn(1, 1);
            Error("轴使能", rtn);
        }

        private void btn_pointclearzero_Click(object sender, EventArgs e)
        {
            // 清除实际位置和规划位置
            short rtn = mc.GTN_ZeroPos(1, 1, 4);    
            Error("清除位置", rtn);
        }

        private void btn_runstaart_Click(object sender, EventArgs e)
        {
            short rtn;
            short axis = 1;
            double prfpos;
            int pos = 0;
            uint clock;
            mc.TTrapPrm trap;
            // 设置为点位运动模式
            rtn = mc.GTN_PrfTrap(1, axis);      
            Error("设置运动模式", rtn);

            // 获得点位运动参数
            rtn = mc.GTN_GetTrapPrm(1, axis, out trap);     
            Error("获得点位运动参数", rtn);

            trap.acc = 1;
            trap.dec = 1;
            trap.smoothTime = 25;
            // 设置点位运动参数
            rtn = mc.GTN_SetTrapPrm(1, axis, ref trap);     
            Error("设置点位运动参数", rtn);

            // 设置点位运动速度
            rtn = mc.GTN_SetVel(1,axis, double.Parse(tb_sudu.Text.Trim()));     
            Error("设置点位运动速度", rtn);

            // 获得规划位置
            rtn = mc.GTN_GetPrfPos(1, axis, out prfpos, 1, out clock);     
            Error("获得规划位置", rtn);
            pos = int.Parse(tb_juli.Text.Trim()) + Convert.ToInt32(prfpos);
            // 设置点位运动目标位置
            rtn = mc.GTN_SetPos(1, axis, pos);     
            Error("设置点位运动目标位置", rtn);

            // 开始运动
            rtn = mc.GTN_Update(1,1<<(axis-1));     
            Error("开始运动", rtn);


        }

        private void btn_runstop_Click(object sender, EventArgs e)
        {
            short rtn = mc.GTN_Stop(1, 0xff, 0);    // 平滑停止所有轴
            Error("停止运动", rtn);
        }

        private void GSNA_Load(object sender, EventArgs e)
        {
            tb_juli.Text = "1000";
            tb_sudu.Text = "1";

            timer1.Interval = 1000;
            timer1.Tick += new EventHandler(timer1_Tick);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            short rtn;
            double encPos, prfPos, encVel, prfVel;
            uint clock;

            rtn = mc.GTN_GetEncPos(1, 1, out encPos, 1, out clock);     // 获得实际位置
            Error("获得实际位置", rtn);
            //rtn = mc.GTN_GetPrfPos(1, 1, out prfPos, 1, out clock);     // 获得规划位置
            //Error("获得规划位置", rtn);
            //rtn = mc.GTN_GetEncVel(1, 1, out encVel, 1, out clock);     // 获得实际速度
            //Error("获得实际速度", rtn);
            //rtn = mc.GTN_GetPrfVel(1, 1, out prfVel, 1, out clock);     // 获得规划速度
            //Error("获得规划速度", rtn);

            //lb_weizhi.Text = $"{prfPos:F0}";
            //lb_ghsudu.Text = $"{prfVel:F0}";
            //lb_shijiweizhi.Text = $"{encPos:F0}";
            //lb_shijisudu.Text = $"{encVel:F0}";

        }

        private void btn_timer1add_Click(object sender, EventArgs e)
        {
            if (!timer1.Enabled)
            {
                timer1.Start();
            }
            
        }

        private void btn_timer1del_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }
    }
}
