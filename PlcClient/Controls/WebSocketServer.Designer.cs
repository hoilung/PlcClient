namespace PlcClient.Controls
{
    partial class WebSocketServer
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.cbx_mode = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.cbx_ip = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.tbx_port = new System.Windows.Forms.ToolStripTextBox();
            this.tslb_path = new System.Windows.Forms.ToolStripLabel();
            this.tbx_path = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btn_start = new System.Windows.Forms.ToolStripButton();
            this.btn_stop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbx_received = new System.Windows.Forms.RichTextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbx_remote = new System.Windows.Forms.CheckedListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbx_send = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbx_reply = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.nd_num = new System.Windows.Forms.NumericUpDown();
            this.nd_step = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.cbx_all = new System.Windows.Forms.CheckBox();
            this.btn_send = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.toolStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nd_num)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nd_step)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.toolStrip1, 3);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.cbx_mode,
            this.toolStripLabel2,
            this.cbx_ip,
            this.toolStripLabel3,
            this.tbx_port,
            this.tslb_path,
            this.tbx_path,
            this.toolStripSeparator2,
            this.btn_start,
            this.btn_stop,
            this.toolStripSeparator1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(726, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(56, 22);
            this.toolStripLabel1.Text = "服务模式";
            // 
            // cbx_mode
            // 
            this.cbx_mode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_mode.Items.AddRange(new object[] {
            "WebSocket",
            "HTTP"});
            this.cbx_mode.Name = "cbx_mode";
            this.cbx_mode.Size = new System.Drawing.Size(100, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(43, 22);
            this.toolStripLabel2.Text = "服务IP";
            // 
            // cbx_ip
            // 
            this.cbx_ip.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_ip.Items.AddRange(new object[] {
            "0.0.0.0"});
            this.cbx_ip.Name = "cbx_ip";
            this.cbx_ip.Size = new System.Drawing.Size(121, 25);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(56, 22);
            this.toolStripLabel3.Text = "服务端口";
            // 
            // tbx_port
            // 
            this.tbx_port.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.tbx_port.MaxLength = 5;
            this.tbx_port.Name = "tbx_port";
            this.tbx_port.Size = new System.Drawing.Size(50, 25);
            this.tbx_port.Text = "7000";
            // 
            // tslb_path
            // 
            this.tslb_path.Name = "tslb_path";
            this.tslb_path.Size = new System.Drawing.Size(32, 22);
            this.tslb_path.Text = "路径";
            // 
            // tbx_path
            // 
            this.tbx_path.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.tbx_path.Name = "tbx_path";
            this.tbx_path.Size = new System.Drawing.Size(100, 25);
            this.tbx_path.Text = "/";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btn_start
            // 
            this.btn_start.Image = global::PlcClient.Properties.Resources.Play;
            this.btn_start.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(52, 22);
            this.btn_start.Text = "启动";
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // btn_stop
            // 
            this.btn_stop.Image = global::PlcClient.Properties.Resources.Stop;
            this.btn_stop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(52, 22);
            this.btn_stop.Text = "停止";
            this.btn_stop.Click += new System.EventHandler(this.btn_stop_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.toolStrip1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 2, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(726, 335);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox2, 2);
            this.groupBox2.Controls.Add(this.tbx_received);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 198);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(560, 134);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "数据接收";
            // 
            // tbx_received
            // 
            this.tbx_received.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbx_received.Location = new System.Drawing.Point(3, 17);
            this.tbx_received.Name = "tbx_received";
            this.tbx_received.ReadOnly = true;
            this.tbx_received.Size = new System.Drawing.Size(554, 114);
            this.tbx_received.TabIndex = 1;
            this.tbx_received.Text = "";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbx_remote);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(319, 28);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(244, 167);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "在线客户端";
            // 
            // cbx_remote
            // 
            this.cbx_remote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbx_remote.Location = new System.Drawing.Point(3, 17);
            this.cbx_remote.Margin = new System.Windows.Forms.Padding(0);
            this.cbx_remote.Name = "cbx_remote";
            this.cbx_remote.ScrollAlwaysVisible = true;
            this.cbx_remote.Size = new System.Drawing.Size(238, 147);
            this.cbx_remote.TabIndex = 12;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbx_send);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(310, 164);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据发送";
            // 
            // tbx_send
            // 
            this.tbx_send.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbx_send.Location = new System.Drawing.Point(3, 17);
            this.tbx_send.Multiline = true;
            this.tbx_send.Name = "tbx_send";
            this.tbx_send.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbx_send.Size = new System.Drawing.Size(304, 144);
            this.tbx_send.TabIndex = 4;
            this.tbx_send.Text = "Hello world";
            this.tbx_send.WordWrap = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.cbx_reply);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.nd_num);
            this.panel1.Controls.Add(this.nd_step);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cbx_all);
            this.panel1.Controls.Add(this.btn_send);
            this.panel1.Location = new System.Drawing.Point(569, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(154, 161);
            this.panel1.TabIndex = 7;
            // 
            // cbx_reply
            // 
            this.cbx_reply.AutoSize = true;
            this.cbx_reply.Location = new System.Drawing.Point(3, 100);
            this.cbx_reply.Name = "cbx_reply";
            this.cbx_reply.Size = new System.Drawing.Size(72, 16);
            this.cbx_reply.TabIndex = 19;
            this.cbx_reply.Text = "自动回复";
            this.cbx_reply.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(84, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 18;
            this.label2.Text = "次数";
            // 
            // nd_num
            // 
            this.nd_num.Location = new System.Drawing.Point(3, 46);
            this.nd_num.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nd_num.Name = "nd_num";
            this.nd_num.Size = new System.Drawing.Size(75, 21);
            this.nd_num.TabIndex = 15;
            this.nd_num.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nd_step
            // 
            this.nd_step.Location = new System.Drawing.Point(3, 73);
            this.nd_step.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.nd_step.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nd_step.Name = "nd_step";
            this.nd_step.Size = new System.Drawing.Size(75, 21);
            this.nd_step.TabIndex = 16;
            this.nd_step.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(84, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 17;
            this.label1.Text = "ms/次间隔";
            // 
            // cbx_all
            // 
            this.cbx_all.AutoSize = true;
            this.cbx_all.Location = new System.Drawing.Point(84, 20);
            this.cbx_all.Name = "cbx_all";
            this.cbx_all.Size = new System.Drawing.Size(72, 16);
            this.cbx_all.TabIndex = 14;
            this.cbx_all.Text = "全选列表";
            this.cbx_all.UseVisualStyleBackColor = true;
            this.cbx_all.CheckedChanged += new System.EventHandler(this.cbx_all_CheckedChanged);
            // 
            // btn_send
            // 
            this.btn_send.Location = new System.Drawing.Point(3, 17);
            this.btn_send.Name = "btn_send";
            this.btn_send.Size = new System.Drawing.Size(75, 23);
            this.btn_send.TabIndex = 0;
            this.btn_send.Text = "发送数据";
            this.btn_send.UseVisualStyleBackColor = true;
            this.btn_send.Click += new System.EventHandler(this.btn_send_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button1);
            this.panel2.Location = new System.Drawing.Point(569, 198);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(154, 100);
            this.panel2.TabIndex = 8;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 17);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "清空";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // WebSocketServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "WebSocketServer";
            this.Size = new System.Drawing.Size(859, 408);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nd_num)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nd_step)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox cbx_mode;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox cbx_ip;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripTextBox tbx_port;
        private System.Windows.Forms.ToolStripLabel tslb_path;
        private System.Windows.Forms.ToolStripTextBox tbx_path;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btn_start;
        private System.Windows.Forms.ToolStripButton btn_stop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbx_send;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckedListBox cbx_remote;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_send;
        private System.Windows.Forms.CheckBox cbx_all;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nd_num;
        private System.Windows.Forms.NumericUpDown nd_step;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox tbx_received;
        private System.Windows.Forms.CheckBox cbx_reply;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button1;
    }
}
