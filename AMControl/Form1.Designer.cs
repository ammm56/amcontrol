
namespace AMControl
{
    partial class Form1
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
            this.btn_gsna = new System.Windows.Forms.Button();
            this.btn_loadcfg = new System.Windows.Forms.Button();
            this.btn_querydbaxistable = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_gsna
            // 
            this.btn_gsna.Location = new System.Drawing.Point(12, 12);
            this.btn_gsna.Name = "btn_gsna";
            this.btn_gsna.Size = new System.Drawing.Size(135, 50);
            this.btn_gsna.TabIndex = 0;
            this.btn_gsna.Text = "GSNA";
            this.btn_gsna.UseVisualStyleBackColor = true;
            this.btn_gsna.Click += new System.EventHandler(this.btn_gsna_Click);
            // 
            // btn_loadcfg
            // 
            this.btn_loadcfg.Location = new System.Drawing.Point(153, 12);
            this.btn_loadcfg.Name = "btn_loadcfg";
            this.btn_loadcfg.Size = new System.Drawing.Size(135, 50);
            this.btn_loadcfg.TabIndex = 0;
            this.btn_loadcfg.Text = "载入配置";
            this.btn_loadcfg.UseVisualStyleBackColor = true;
            this.btn_loadcfg.Click += new System.EventHandler(this.btn_loadcfg_Click);
            // 
            // btn_querydbaxistable
            // 
            this.btn_querydbaxistable.Location = new System.Drawing.Point(294, 12);
            this.btn_querydbaxistable.Name = "btn_querydbaxistable";
            this.btn_querydbaxistable.Size = new System.Drawing.Size(135, 50);
            this.btn_querydbaxistable.TabIndex = 0;
            this.btn_querydbaxistable.Text = "读取轴表记录";
            this.btn_querydbaxistable.UseVisualStyleBackColor = true;
            this.btn_querydbaxistable.Click += new System.EventHandler(this.btn_querydbaxistable_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_querydbaxistable);
            this.Controls.Add(this.btn_loadcfg);
            this.Controls.Add(this.btn_gsna);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_gsna;
        private System.Windows.Forms.Button btn_loadcfg;
        private System.Windows.Forms.Button btn_querydbaxistable;
    }
}

