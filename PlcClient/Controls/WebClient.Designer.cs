namespace PlcClient.Controls
{
    partial class WebClient
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbx_received = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbx_send = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.nd_num = new System.Windows.Forms.NumericUpDown();
            this.nd_step = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_send = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btn_clear = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tbx_addr = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btn_open = new System.Windows.Forms.ToolStripButton();
            this.btn_close = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nd_num)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nd_step)).BeginInit();
            this.panel2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 40);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(679, 335);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbx_received);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 170);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(497, 162);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "接收数据";
            // 
            // tbx_received
            // 
            this.tbx_received.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbx_received.Location = new System.Drawing.Point(3, 17);
            this.tbx_received.Name = "tbx_received";
            this.tbx_received.ReadOnly = true;
            this.tbx_received.Size = new System.Drawing.Size(491, 142);
            this.tbx_received.TabIndex = 0;
            this.tbx_received.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbx_send);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(497, 161);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "发送数据";
            // 
            // tbx_send
            // 
            this.tbx_send.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbx_send.Location = new System.Drawing.Point(3, 17);
            this.tbx_send.Multiline = true;
            this.tbx_send.Name = "tbx_send";
            this.tbx_send.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbx_send.Size = new System.Drawing.Size(491, 141);
            this.tbx_send.TabIndex = 5;
            this.tbx_send.WordWrap = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.nd_num);
            this.panel1.Controls.Add(this.nd_step);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btn_send);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(506, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(170, 161);
            this.panel1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(84, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 13;
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
            this.nd_num.TabIndex = 10;
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
            500,
            0,
            0,
            0});
            this.nd_step.Name = "nd_step";
            this.nd_step.Size = new System.Drawing.Size(75, 21);
            this.nd_step.TabIndex = 11;
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
            this.label1.TabIndex = 12;
            this.label1.Text = "ms/次间隔";
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
            this.panel2.Controls.Add(this.btn_clear);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(506, 170);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(170, 162);
            this.panel2.TabIndex = 4;
            // 
            // btn_clear
            // 
            this.btn_clear.Location = new System.Drawing.Point(3, 17);
            this.btn_clear.Name = "btn_clear";
            this.btn_clear.Size = new System.Drawing.Size(75, 23);
            this.btn_clear.TabIndex = 0;
            this.btn_clear.Text = "清空";
            this.btn_clear.UseVisualStyleBackColor = true;
            this.btn_clear.Click += new System.EventHandler(this.btn_clear_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.tbx_addr,
            this.toolStripSeparator1,
            this.btn_open,
            this.btn_close});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(763, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(114, 22);
            this.toolStripLabel1.Text = "WebSocket 地址：";
            // 
            // tbx_addr
            // 
            this.tbx_addr.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.tbx_addr.Name = "tbx_addr";
            this.tbx_addr.Size = new System.Drawing.Size(300, 25);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btn_open
            // 
            this.btn_open.Image = global::PlcClient.Properties.Resources.Play;
            this.btn_open.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_open.Name = "btn_open";
            this.btn_open.Size = new System.Drawing.Size(52, 22);
            this.btn_open.Text = "连接";
            this.btn_open.Click += new System.EventHandler(this.btn_open_Click);
            // 
            // btn_close
            // 
            this.btn_close.Image = global::PlcClient.Properties.Resources.Stop;
            this.btn_close.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(52, 22);
            this.btn_close.Text = "断开";
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // WebClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;            
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.toolStrip1);

            this.Name = "WebClient";
            this.Size = new System.Drawing.Size(763, 415);            
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nd_num)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nd_step)).EndInit();
            this.panel2.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripTextBox tbx_addr;
        private System.Windows.Forms.ToolStripButton btn_open;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_send;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nd_num;
        private System.Windows.Forms.NumericUpDown nd_step;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_clear;
        private System.Windows.Forms.TextBox tbx_send;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btn_close;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RichTextBox tbx_received;
    }
}
