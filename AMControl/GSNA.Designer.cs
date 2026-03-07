
namespace AMControl
{
    partial class GSNA
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_init = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_runstop = new System.Windows.Forms.Button();
            this.btn_runstaart = new System.Windows.Forms.Button();
            this.btn_pointclearzero = new System.Windows.Forms.Button();
            this.btn_enable = new System.Windows.Forms.Button();
            this.btn_clearstatus = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tb_shijisudu = new System.Windows.Forms.TextBox();
            this.tb_shijiweizhi = new System.Windows.Forms.TextBox();
            this.tb_ghsudu = new System.Windows.Forms.TextBox();
            this.tb_weizhi = new System.Windows.Forms.TextBox();
            this.tb_sudu = new System.Windows.Forms.TextBox();
            this.tb_juli = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lb_shijisudu = new System.Windows.Forms.Label();
            this.lb_shijiweizhi = new System.Windows.Forms.Label();
            this.lb_ghsudu = new System.Windows.Forms.Label();
            this.lb_weizhi = new System.Windows.Forms.Label();
            this.lb_sudu = new System.Windows.Forms.Label();
            this.lb_juli = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btn_timer1del = new System.Windows.Forms.Button();
            this.btn_timer1add = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_init
            // 
            this.btn_init.Location = new System.Drawing.Point(18, 33);
            this.btn_init.Name = "btn_init";
            this.btn_init.Size = new System.Drawing.Size(143, 48);
            this.btn_init.TabIndex = 0;
            this.btn_init.Text = "初始化";
            this.btn_init.UseVisualStyleBackColor = true;
            this.btn_init.Click += new System.EventHandler(this.btn_init_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.btn_runstop);
            this.groupBox1.Controls.Add(this.btn_runstaart);
            this.groupBox1.Controls.Add(this.btn_pointclearzero);
            this.groupBox1.Controls.Add(this.btn_enable);
            this.groupBox1.Controls.Add(this.btn_clearstatus);
            this.groupBox1.Controls.Add(this.btn_init);
            this.groupBox1.Location = new System.Drawing.Point(15, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(187, 503);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "按钮";
            // 
            // btn_runstop
            // 
            this.btn_runstop.Location = new System.Drawing.Point(18, 303);
            this.btn_runstop.Name = "btn_runstop";
            this.btn_runstop.Size = new System.Drawing.Size(143, 48);
            this.btn_runstop.TabIndex = 0;
            this.btn_runstop.Text = "停止运动";
            this.btn_runstop.UseVisualStyleBackColor = true;
            this.btn_runstop.Click += new System.EventHandler(this.btn_runstop_Click);
            // 
            // btn_runstaart
            // 
            this.btn_runstaart.Location = new System.Drawing.Point(18, 249);
            this.btn_runstaart.Name = "btn_runstaart";
            this.btn_runstaart.Size = new System.Drawing.Size(143, 48);
            this.btn_runstaart.TabIndex = 0;
            this.btn_runstaart.Text = "开始运动";
            this.btn_runstaart.UseVisualStyleBackColor = true;
            this.btn_runstaart.Click += new System.EventHandler(this.btn_runstaart_Click);
            // 
            // btn_pointclearzero
            // 
            this.btn_pointclearzero.Location = new System.Drawing.Point(18, 195);
            this.btn_pointclearzero.Name = "btn_pointclearzero";
            this.btn_pointclearzero.Size = new System.Drawing.Size(143, 48);
            this.btn_pointclearzero.TabIndex = 0;
            this.btn_pointclearzero.Text = "位置清零";
            this.btn_pointclearzero.UseVisualStyleBackColor = true;
            this.btn_pointclearzero.Click += new System.EventHandler(this.btn_pointclearzero_Click);
            // 
            // btn_enable
            // 
            this.btn_enable.Location = new System.Drawing.Point(18, 141);
            this.btn_enable.Name = "btn_enable";
            this.btn_enable.Size = new System.Drawing.Size(143, 48);
            this.btn_enable.TabIndex = 0;
            this.btn_enable.Text = "使能";
            this.btn_enable.UseVisualStyleBackColor = true;
            this.btn_enable.Click += new System.EventHandler(this.btn_enable_Click);
            // 
            // btn_clearstatus
            // 
            this.btn_clearstatus.Location = new System.Drawing.Point(18, 87);
            this.btn_clearstatus.Name = "btn_clearstatus";
            this.btn_clearstatus.Size = new System.Drawing.Size(143, 48);
            this.btn_clearstatus.TabIndex = 0;
            this.btn_clearstatus.Text = "清除状态";
            this.btn_clearstatus.UseVisualStyleBackColor = true;
            this.btn_clearstatus.Click += new System.EventHandler(this.btn_clearstatus_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(893, 556);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(885, 530);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "测试1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox3.Controls.Add(this.tb_shijisudu);
            this.groupBox3.Controls.Add(this.tb_shijiweizhi);
            this.groupBox3.Controls.Add(this.tb_ghsudu);
            this.groupBox3.Controls.Add(this.tb_weizhi);
            this.groupBox3.Controls.Add(this.tb_sudu);
            this.groupBox3.Controls.Add(this.tb_juli);
            this.groupBox3.Location = new System.Drawing.Point(401, 21);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(224, 503);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "数值";
            // 
            // tb_shijisudu
            // 
            this.tb_shijisudu.Font = new System.Drawing.Font("宋体", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb_shijisudu.Location = new System.Drawing.Point(23, 303);
            this.tb_shijisudu.Multiline = true;
            this.tb_shijisudu.Name = "tb_shijisudu";
            this.tb_shijisudu.Size = new System.Drawing.Size(181, 47);
            this.tb_shijisudu.TabIndex = 0;
            // 
            // tb_shijiweizhi
            // 
            this.tb_shijiweizhi.Font = new System.Drawing.Font("宋体", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb_shijiweizhi.Location = new System.Drawing.Point(23, 249);
            this.tb_shijiweizhi.Multiline = true;
            this.tb_shijiweizhi.Name = "tb_shijiweizhi";
            this.tb_shijiweizhi.Size = new System.Drawing.Size(181, 47);
            this.tb_shijiweizhi.TabIndex = 0;
            // 
            // tb_ghsudu
            // 
            this.tb_ghsudu.Font = new System.Drawing.Font("宋体", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb_ghsudu.Location = new System.Drawing.Point(23, 195);
            this.tb_ghsudu.Multiline = true;
            this.tb_ghsudu.Name = "tb_ghsudu";
            this.tb_ghsudu.Size = new System.Drawing.Size(181, 47);
            this.tb_ghsudu.TabIndex = 0;
            // 
            // tb_weizhi
            // 
            this.tb_weizhi.Font = new System.Drawing.Font("宋体", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb_weizhi.Location = new System.Drawing.Point(23, 140);
            this.tb_weizhi.Multiline = true;
            this.tb_weizhi.Name = "tb_weizhi";
            this.tb_weizhi.Size = new System.Drawing.Size(181, 47);
            this.tb_weizhi.TabIndex = 0;
            // 
            // tb_sudu
            // 
            this.tb_sudu.Font = new System.Drawing.Font("宋体", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb_sudu.Location = new System.Drawing.Point(23, 87);
            this.tb_sudu.Multiline = true;
            this.tb_sudu.Name = "tb_sudu";
            this.tb_sudu.Size = new System.Drawing.Size(181, 47);
            this.tb_sudu.TabIndex = 0;
            // 
            // tb_juli
            // 
            this.tb_juli.Font = new System.Drawing.Font("宋体", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb_juli.Location = new System.Drawing.Point(23, 34);
            this.tb_juli.Multiline = true;
            this.tb_juli.Name = "tb_juli";
            this.tb_juli.Size = new System.Drawing.Size(181, 47);
            this.tb_juli.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.lb_shijisudu);
            this.groupBox2.Controls.Add(this.lb_shijiweizhi);
            this.groupBox2.Controls.Add(this.lb_ghsudu);
            this.groupBox2.Controls.Add(this.lb_weizhi);
            this.groupBox2.Controls.Add(this.lb_sudu);
            this.groupBox2.Controls.Add(this.lb_juli);
            this.groupBox2.Location = new System.Drawing.Point(208, 21);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(187, 503);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "标签";
            // 
            // lb_shijisudu
            // 
            this.lb_shijisudu.Location = new System.Drawing.Point(18, 303);
            this.lb_shijisudu.Name = "lb_shijisudu";
            this.lb_shijisudu.Size = new System.Drawing.Size(153, 48);
            this.lb_shijisudu.TabIndex = 0;
            this.lb_shijisudu.Text = "实际速度";
            this.lb_shijisudu.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_shijiweizhi
            // 
            this.lb_shijiweizhi.Location = new System.Drawing.Point(18, 249);
            this.lb_shijiweizhi.Name = "lb_shijiweizhi";
            this.lb_shijiweizhi.Size = new System.Drawing.Size(153, 48);
            this.lb_shijiweizhi.TabIndex = 0;
            this.lb_shijiweizhi.Text = "实际位置";
            this.lb_shijiweizhi.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_ghsudu
            // 
            this.lb_ghsudu.Location = new System.Drawing.Point(18, 195);
            this.lb_ghsudu.Name = "lb_ghsudu";
            this.lb_ghsudu.Size = new System.Drawing.Size(153, 48);
            this.lb_ghsudu.TabIndex = 0;
            this.lb_ghsudu.Text = "规划速度";
            this.lb_ghsudu.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_weizhi
            // 
            this.lb_weizhi.Location = new System.Drawing.Point(18, 141);
            this.lb_weizhi.Name = "lb_weizhi";
            this.lb_weizhi.Size = new System.Drawing.Size(153, 48);
            this.lb_weizhi.TabIndex = 0;
            this.lb_weizhi.Text = "规划位置";
            this.lb_weizhi.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_sudu
            // 
            this.lb_sudu.Location = new System.Drawing.Point(18, 87);
            this.lb_sudu.Name = "lb_sudu";
            this.lb_sudu.Size = new System.Drawing.Size(153, 48);
            this.lb_sudu.TabIndex = 0;
            this.lb_sudu.Text = "点位速度";
            this.lb_sudu.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_juli
            // 
            this.lb_juli.Location = new System.Drawing.Point(18, 34);
            this.lb_juli.Name = "lb_juli";
            this.lb_juli.Size = new System.Drawing.Size(153, 48);
            this.lb_juli.TabIndex = 0;
            this.lb_juli.Text = "点位距离";
            this.lb_juli.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox4.Controls.Add(this.btn_timer1del);
            this.groupBox4.Controls.Add(this.btn_timer1add);
            this.groupBox4.Location = new System.Drawing.Point(631, 21);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(187, 503);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "按钮";
            // 
            // btn_timer1del
            // 
            this.btn_timer1del.Location = new System.Drawing.Point(18, 87);
            this.btn_timer1del.Name = "btn_timer1del";
            this.btn_timer1del.Size = new System.Drawing.Size(143, 48);
            this.btn_timer1del.TabIndex = 0;
            this.btn_timer1del.Text = "撤销定时读取值";
            this.btn_timer1del.UseVisualStyleBackColor = true;
            this.btn_timer1del.Click += new System.EventHandler(this.btn_timer1del_Click);
            // 
            // btn_timer1add
            // 
            this.btn_timer1add.Location = new System.Drawing.Point(18, 33);
            this.btn_timer1add.Name = "btn_timer1add";
            this.btn_timer1add.Size = new System.Drawing.Size(143, 48);
            this.btn_timer1add.TabIndex = 0;
            this.btn_timer1add.Text = "开启定时读取值";
            this.btn_timer1add.UseVisualStyleBackColor = true;
            this.btn_timer1add.Click += new System.EventHandler(this.btn_timer1add_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(885, 530);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "测试2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // GSNA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(917, 580);
            this.Controls.Add(this.tabControl1);
            this.Name = "GSNA";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GSNA";
            this.Load += new System.EventHandler(this.GSNA_Load);
            this.groupBox1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_init;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btn_runstop;
        private System.Windows.Forms.Button btn_runstaart;
        private System.Windows.Forms.Button btn_pointclearzero;
        private System.Windows.Forms.Button btn_enable;
        private System.Windows.Forms.Button btn_clearstatus;
        private System.Windows.Forms.Label lb_juli;
        private System.Windows.Forms.Label lb_shijisudu;
        private System.Windows.Forms.Label lb_shijiweizhi;
        private System.Windows.Forms.Label lb_ghsudu;
        private System.Windows.Forms.Label lb_weizhi;
        private System.Windows.Forms.Label lb_sudu;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox tb_shijisudu;
        private System.Windows.Forms.TextBox tb_shijiweizhi;
        private System.Windows.Forms.TextBox tb_ghsudu;
        private System.Windows.Forms.TextBox tb_weizhi;
        private System.Windows.Forms.TextBox tb_sudu;
        private System.Windows.Forms.TextBox tb_juli;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btn_timer1del;
        private System.Windows.Forms.Button btn_timer1add;
    }
}

