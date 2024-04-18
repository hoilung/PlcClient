namespace PlcClient.Controls
{
    partial class Net2Server
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
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btn_start = new System.Windows.Forms.ToolStripButton();
            this.btn_stop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.cbx_code = new System.Windows.Forms.ToolStripComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbx_remoteAll = new System.Windows.Forms.CheckBox();
            this.cbx_remote = new System.Windows.Forms.CheckedListBox();
            this.cbx_hexSend = new System.Windows.Forms.CheckBox();
            this.btn_clearSend = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tbx_send = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_clearCallback = new System.Windows.Forms.Button();
            this.cbx_time = new System.Windows.Forms.CheckBox();
            this.cbx_hex = new System.Windows.Forms.CheckBox();
            this.cbx_string = new System.Windows.Forms.CheckBox();
            this.tbx_received = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.cbx_mode,
            this.toolStripLabel2,
            this.cbx_ip,
            this.toolStripLabel3,
            this.tbx_port,
            this.toolStripSeparator2,
            this.btn_start,
            this.btn_stop,
            this.toolStripSeparator1,
            this.toolStripLabel4,
            this.cbx_code});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(710, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(32, 22);
            this.toolStripLabel1.Text = "模式";
            // 
            // cbx_mode
            // 
            this.cbx_mode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_mode.Items.AddRange(new object[] {
            "TCP",
            "UDP",
            "TCP&UDP"});
            this.cbx_mode.Name = "cbx_mode";
            this.cbx_mode.Size = new System.Drawing.Size(100, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(43, 22);
            this.toolStripLabel2.Text = "本地IP";
            // 
            // cbx_ip
            // 
            this.cbx_ip.Name = "cbx_ip";
            this.cbx_ip.Size = new System.Drawing.Size(121, 25);
            this.cbx_ip.Text = "127.0.0.1";
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(56, 22);
            this.toolStripLabel3.Text = "本地端口";
            // 
            // tbx_port
            // 
            this.tbx_port.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.tbx_port.MaxLength = 5;
            this.tbx_port.Name = "tbx_port";
            this.tbx_port.Size = new System.Drawing.Size(50, 25);
            this.tbx_port.Text = "8000";
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
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(56, 22);
            this.toolStripLabel4.Text = "字符编码";
            // 
            // cbx_code
            // 
            this.cbx_code.Items.AddRange(new object[] {
            "Default",
            "UTF-8",
            "GBK",
            "Unicode",
            "ASCII"});
            this.cbx_code.Name = "cbx_code";
            this.cbx_code.Size = new System.Drawing.Size(80, 25);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbx_send);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(259, 175);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据发送";
            // 
            // cbx_remoteAll
            // 
            this.cbx_remoteAll.AutoSize = true;
            this.cbx_remoteAll.Location = new System.Drawing.Point(3, 46);
            this.cbx_remoteAll.Name = "cbx_remoteAll";
            this.cbx_remoteAll.Size = new System.Drawing.Size(72, 16);
            this.cbx_remoteAll.TabIndex = 13;
            this.cbx_remoteAll.Text = "全选列表";
            this.cbx_remoteAll.UseVisualStyleBackColor = true;
            this.cbx_remoteAll.CheckedChanged += new System.EventHandler(this.cbx_remoteAll_CheckedChanged);
            // 
            // cbx_remote
            // 
            this.cbx_remote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbx_remote.FormattingEnabled = true;
            this.cbx_remote.Location = new System.Drawing.Point(3, 17);
            this.cbx_remote.Name = "cbx_remote";
            this.cbx_remote.ScrollAlwaysVisible = true;
            this.cbx_remote.Size = new System.Drawing.Size(194, 155);
            this.cbx_remote.TabIndex = 12;
            // 
            // cbx_hexSend
            // 
            this.cbx_hexSend.AutoSize = true;
            this.cbx_hexSend.Location = new System.Drawing.Point(84, 19);
            this.cbx_hexSend.Name = "cbx_hexSend";
            this.cbx_hexSend.Size = new System.Drawing.Size(72, 16);
            this.cbx_hexSend.TabIndex = 11;
            this.cbx_hexSend.Text = "发送 HEX";
            this.cbx_hexSend.UseVisualStyleBackColor = true;
            this.cbx_hexSend.CheckedChanged += new System.EventHandler(this.cbx_hexSend_CheckedChanged);
            // 
            // btn_clearSend
            // 
            this.btn_clearSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_clearSend.Location = new System.Drawing.Point(3, 149);
            this.btn_clearSend.Name = "btn_clearSend";
            this.btn_clearSend.Size = new System.Drawing.Size(75, 23);
            this.btn_clearSend.TabIndex = 10;
            this.btn_clearSend.Text = "清空";
            this.btn_clearSend.UseVisualStyleBackColor = true;
            this.btn_clearSend.Click += new System.EventHandler(this.btn_clearSend_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 17);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "发送数据";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbx_send
            // 
            this.tbx_send.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbx_send.Location = new System.Drawing.Point(3, 17);
            this.tbx_send.Multiline = true;
            this.tbx_send.Name = "tbx_send";
            this.tbx_send.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbx_send.Size = new System.Drawing.Size(253, 155);
            this.tbx_send.TabIndex = 4;
            this.tbx_send.Text = "Hello world";
            this.tbx_send.WordWrap = false;
            this.tbx_send.TextChanged += new System.EventHandler(this.tbx_send_TextChanged);
            this.tbx_send.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbx_send_KeyPress);
            // 
            // groupBox2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox2, 2);
            this.groupBox2.Controls.Add(this.tbx_received);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 184);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(465, 175);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "数据接收";
            // 
            // btn_clearCallback
            // 
            this.btn_clearCallback.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_clearCallback.Location = new System.Drawing.Point(3, 149);
            this.btn_clearCallback.Name = "btn_clearCallback";
            this.btn_clearCallback.Size = new System.Drawing.Size(75, 23);
            this.btn_clearCallback.TabIndex = 12;
            this.btn_clearCallback.Text = "清空";
            this.btn_clearCallback.UseVisualStyleBackColor = true;
            this.btn_clearCallback.Click += new System.EventHandler(this.btn_clearCallback_Click);
            // 
            // cbx_time
            // 
            this.cbx_time.AutoSize = true;
            this.cbx_time.Checked = true;
            this.cbx_time.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbx_time.Location = new System.Drawing.Point(3, 17);
            this.cbx_time.Name = "cbx_time";
            this.cbx_time.Size = new System.Drawing.Size(72, 16);
            this.cbx_time.TabIndex = 9;
            this.cbx_time.Text = "显示时间";
            this.cbx_time.UseVisualStyleBackColor = true;
            // 
            // cbx_hex
            // 
            this.cbx_hex.AutoSize = true;
            this.cbx_hex.Location = new System.Drawing.Point(3, 61);
            this.cbx_hex.Name = "cbx_hex";
            this.cbx_hex.Size = new System.Drawing.Size(72, 16);
            this.cbx_hex.TabIndex = 11;
            this.cbx_hex.Text = "显示 HEX";
            this.cbx_hex.UseVisualStyleBackColor = true;
            // 
            // cbx_string
            // 
            this.cbx_string.AutoSize = true;
            this.cbx_string.Checked = true;
            this.cbx_string.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbx_string.Location = new System.Drawing.Point(3, 39);
            this.cbx_string.Name = "cbx_string";
            this.cbx_string.Size = new System.Drawing.Size(72, 16);
            this.cbx_string.TabIndex = 10;
            this.cbx_string.Text = "显示字符";
            this.cbx_string.UseVisualStyleBackColor = true;
            // 
            // tbx_received
            // 
            this.tbx_received.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbx_received.Location = new System.Drawing.Point(3, 17);
            this.tbx_received.Name = "tbx_received";
            this.tbx_received.ReadOnly = true;
            this.tbx_received.Size = new System.Drawing.Size(459, 155);
            this.tbx_received.TabIndex = 0;
            this.tbx_received.Text = "";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(17, 46);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(653, 362);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbx_remoteAll);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.btn_clearSend);
            this.panel1.Controls.Add(this.cbx_hexSend);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(474, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(176, 175);
            this.panel1.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btn_clearCallback);
            this.panel2.Controls.Add(this.cbx_time);
            this.panel2.Controls.Add(this.cbx_string);
            this.panel2.Controls.Add(this.cbx_hex);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(474, 184);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(176, 175);
            this.panel2.TabIndex = 5;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbx_remote);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(268, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 175);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "在线客户端";
            // 
            // Net2Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "Net2Server";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox cbx_mode;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox cbx_ip;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripTextBox tbx_port;
        private System.Windows.Forms.ToolStripButton btn_start;
        private System.Windows.Forms.ToolStripButton btn_stop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStripComboBox cbx_code;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tbx_send;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox tbx_received;
        private System.Windows.Forms.CheckBox cbx_time;
        private System.Windows.Forms.CheckBox cbx_hex;
        private System.Windows.Forms.CheckBox cbx_string;
        private System.Windows.Forms.Button btn_clearCallback;
        private System.Windows.Forms.Button btn_clearSend;
        private System.Windows.Forms.CheckBox cbx_hexSend;
        private System.Windows.Forms.CheckedListBox cbx_remote;
        private System.Windows.Forms.CheckBox cbx_remoteAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox3;
    }
}
