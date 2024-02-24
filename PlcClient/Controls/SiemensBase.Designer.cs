namespace PlcClient.Controls
{
    partial class SiemensBase
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.gb_set = new System.Windows.Forms.GroupBox();
            this.tbx_port = new System.Windows.Forms.MaskedTextBox();
            this.tbx_rack = new System.Windows.Forms.MaskedTextBox();
            this.tbx_slot = new System.Windows.Forms.MaskedTextBox();
            this.btn_close = new System.Windows.Forms.Button();
            this.btn_open = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbx_ip = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gb_datatype = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.gb_act = new System.Windows.Forms.GroupBox();
            this.chk_enablewrite = new System.Windows.Forms.CheckBox();
            this.btn_clearTbx = new System.Windows.Forms.Button();
            this.btn_write = new System.Windows.Forms.Button();
            this.tbx_value = new System.Windows.Forms.TextBox();
            this.btn_read = new System.Windows.Forms.Button();
            this.tbx_adr = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbx_msg = new System.Windows.Forms.TextBox();
            this.gb_set.SuspendLayout();
            this.gb_datatype.SuspendLayout();
            this.gb_act.SuspendLayout();
            this.SuspendLayout();
            // 
            // gb_set
            // 
            this.gb_set.Controls.Add(this.tbx_port);
            this.gb_set.Controls.Add(this.tbx_rack);
            this.gb_set.Controls.Add(this.tbx_slot);
            this.gb_set.Controls.Add(this.btn_close);
            this.gb_set.Controls.Add(this.btn_open);
            this.gb_set.Controls.Add(this.label4);
            this.gb_set.Controls.Add(this.label3);
            this.gb_set.Controls.Add(this.label2);
            this.gb_set.Controls.Add(this.tbx_ip);
            this.gb_set.Controls.Add(this.label1);
            this.gb_set.Location = new System.Drawing.Point(4, 4);
            this.gb_set.Name = "gb_set";
            this.gb_set.Size = new System.Drawing.Size(703, 60);
            this.gb_set.TabIndex = 0;
            this.gb_set.TabStop = false;
            this.gb_set.Text = "设置";
            // 
            // tbx_port
            // 
            this.tbx_port.Location = new System.Drawing.Point(170, 24);
            this.tbx_port.Mask = "99999";
            this.tbx_port.Name = "tbx_port";
            this.tbx_port.Size = new System.Drawing.Size(40, 21);
            this.tbx_port.TabIndex = 12;
            this.tbx_port.Text = "102";
            this.tbx_port.ValidatingType = typeof(int);
            // 
            // tbx_rack
            // 
            this.tbx_rack.Location = new System.Drawing.Point(263, 24);
            this.tbx_rack.Mask = "99";
            this.tbx_rack.Name = "tbx_rack";
            this.tbx_rack.Size = new System.Drawing.Size(40, 21);
            this.tbx_rack.TabIndex = 11;
            this.tbx_rack.Text = "0";
            this.tbx_rack.ValidatingType = typeof(int);
            // 
            // tbx_slot
            // 
            this.tbx_slot.Location = new System.Drawing.Point(344, 24);
            this.tbx_slot.Mask = "99";
            this.tbx_slot.Name = "tbx_slot";
            this.tbx_slot.Size = new System.Drawing.Size(40, 21);
            this.tbx_slot.TabIndex = 10;
            this.tbx_slot.Text = "0";
            this.tbx_slot.ValidatingType = typeof(int);
            // 
            // btn_close
            // 
            this.btn_close.Location = new System.Drawing.Point(471, 22);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(75, 23);
            this.btn_close.TabIndex = 9;
            this.btn_close.Text = "关闭";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // btn_open
            // 
            this.btn_open.Location = new System.Drawing.Point(390, 23);
            this.btn_open.Name = "btn_open";
            this.btn_open.Size = new System.Drawing.Size(75, 23);
            this.btn_open.TabIndex = 8;
            this.btn_open.Text = "连接";
            this.btn_open.UseVisualStyleBackColor = true;
            this.btn_open.Click += new System.EventHandler(this.btn_open_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(309, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "槽号";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(216, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "机架号";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(135, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "端口";
            // 
            // tbx_ip
            // 
            this.tbx_ip.Location = new System.Drawing.Point(29, 24);
            this.tbx_ip.Name = "tbx_ip";
            this.tbx_ip.Size = new System.Drawing.Size(100, 21);
            this.tbx_ip.TabIndex = 1;
            this.tbx_ip.Text = "192.168.0.100";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP";
            // 
            // gb_datatype
            // 
            this.gb_datatype.Controls.Add(this.flowLayoutPanel1);
            this.gb_datatype.Location = new System.Drawing.Point(4, 69);
            this.gb_datatype.Name = "gb_datatype";
            this.gb_datatype.Size = new System.Drawing.Size(703, 60);
            this.gb_datatype.TabIndex = 1;
            this.gb_datatype.TabStop = false;
            this.gb_datatype.Text = "数据类型";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Location = new System.Drawing.Point(6, 23);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(691, 22);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // gb_act
            // 
            this.gb_act.Controls.Add(this.chk_enablewrite);
            this.gb_act.Controls.Add(this.btn_clearTbx);
            this.gb_act.Controls.Add(this.btn_write);
            this.gb_act.Controls.Add(this.tbx_value);
            this.gb_act.Controls.Add(this.btn_read);
            this.gb_act.Controls.Add(this.tbx_adr);
            this.gb_act.Controls.Add(this.label5);
            this.gb_act.Location = new System.Drawing.Point(4, 135);
            this.gb_act.Name = "gb_act";
            this.gb_act.Size = new System.Drawing.Size(697, 60);
            this.gb_act.TabIndex = 2;
            this.gb_act.TabStop = false;
            this.gb_act.Text = "读/写";
            // 
            // chk_enablewrite
            // 
            this.chk_enablewrite.AutoSize = true;
            this.chk_enablewrite.Location = new System.Drawing.Point(311, 28);
            this.chk_enablewrite.Name = "chk_enablewrite";
            this.chk_enablewrite.Size = new System.Drawing.Size(60, 16);
            this.chk_enablewrite.TabIndex = 7;
            this.chk_enablewrite.Text = "开启写";
            this.chk_enablewrite.UseVisualStyleBackColor = true;
            this.chk_enablewrite.CheckedChanged += new System.EventHandler(this.cbx_enablewrite_CheckedChanged);
            // 
            // btn_clearTbx
            // 
            this.btn_clearTbx.Location = new System.Drawing.Point(471, 25);
            this.btn_clearTbx.Name = "btn_clearTbx";
            this.btn_clearTbx.Size = new System.Drawing.Size(75, 23);
            this.btn_clearTbx.TabIndex = 6;
            this.btn_clearTbx.Text = "清空输出";
            this.btn_clearTbx.UseVisualStyleBackColor = true;
            this.btn_clearTbx.Click += new System.EventHandler(this.btn_clearTbx_Click);
            // 
            // btn_write
            // 
            this.btn_write.Location = new System.Drawing.Point(390, 25);
            this.btn_write.Name = "btn_write";
            this.btn_write.Size = new System.Drawing.Size(75, 23);
            this.btn_write.TabIndex = 5;
            this.btn_write.Text = "写入";
            this.btn_write.UseVisualStyleBackColor = true;
            this.btn_write.Click += new System.EventHandler(this.btn_write_Click);
            // 
            // tbx_value
            // 
            this.tbx_value.Location = new System.Drawing.Point(218, 26);
            this.tbx_value.Name = "tbx_value";
            this.tbx_value.Size = new System.Drawing.Size(86, 21);
            this.tbx_value.TabIndex = 4;
            this.tbx_value.Text = "0";
            // 
            // btn_read
            // 
            this.btn_read.Location = new System.Drawing.Point(137, 25);
            this.btn_read.Name = "btn_read";
            this.btn_read.Size = new System.Drawing.Size(75, 23);
            this.btn_read.TabIndex = 2;
            this.btn_read.Text = "读取";
            this.btn_read.UseVisualStyleBackColor = true;
            this.btn_read.Click += new System.EventHandler(this.btn_read_Click);
            // 
            // tbx_adr
            // 
            this.tbx_adr.Location = new System.Drawing.Point(43, 26);
            this.tbx_adr.Name = "tbx_adr";
            this.tbx_adr.Size = new System.Drawing.Size(86, 21);
            this.tbx_adr.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "地址";
            // 
            // tbx_msg
            // 
            this.tbx_msg.Location = new System.Drawing.Point(4, 201);
            this.tbx_msg.Multiline = true;
            this.tbx_msg.Name = "tbx_msg";
            this.tbx_msg.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbx_msg.Size = new System.Drawing.Size(697, 213);
            this.tbx_msg.TabIndex = 3;
            // 
            // SiemensBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.tbx_msg);
            this.Controls.Add(this.gb_act);
            this.Controls.Add(this.gb_datatype);
            this.Controls.Add(this.gb_set);
            this.Name = "SiemensBase";
            this.gb_set.ResumeLayout(false);
            this.gb_set.PerformLayout();
            this.gb_datatype.ResumeLayout(false);
            this.gb_act.ResumeLayout(false);
            this.gb_act.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gb_set;
        private System.Windows.Forms.GroupBox gb_datatype;
        private System.Windows.Forms.GroupBox gb_act;
        private System.Windows.Forms.TextBox tbx_msg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbx_ip;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Button btn_open;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbx_adr;
        private System.Windows.Forms.Button btn_read;
        private System.Windows.Forms.TextBox tbx_value;
        private System.Windows.Forms.Button btn_write;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btn_clearTbx;
        private System.Windows.Forms.MaskedTextBox tbx_slot;
        private System.Windows.Forms.MaskedTextBox tbx_rack;
        private System.Windows.Forms.MaskedTextBox tbx_port;
        private System.Windows.Forms.CheckBox chk_enablewrite;
    }
}
