namespace PlcClient.Controls
{
    partial class GePLC
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbx_port = new System.Windows.Forms.MaskedTextBox();
            this.btn_close = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbx_ip = new System.Windows.Forms.TextBox();
            this.btn_open = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.btn_add = new System.Windows.Forms.Button();
            this.btn_read = new System.Windows.Forms.Button();
            this.lv_data = new PlcClient.Controls.ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menu_lv = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tm_exportExcel = new System.Windows.Forms.ToolStripMenuItem();
            this.clearAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tbx_address = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbx_type = new System.Windows.Forms.ComboBox();
            this.chk_enablewrite = new System.Windows.Forms.CheckBox();
            this.btn_write = new System.Windows.Forms.Button();
            this.btn_readOne = new System.Windows.Forms.Button();
            this.tbx_value = new System.Windows.Forms.TextBox();
            this.tbx_addressOne = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label7 = new System.Windows.Forms.Label();
            this.tbx_time = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.tbx_num = new System.Windows.Forms.NumericUpDown();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.menu_lv.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbx_time)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbx_num)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbx_port);
            this.groupBox1.Controls.Add(this.btn_close);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tbx_ip);
            this.groupBox1.Controls.Add(this.btn_open);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(935, 54);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "GE 设置";
            // 
            // tbx_port
            // 
            this.tbx_port.Location = new System.Drawing.Point(201, 22);
            this.tbx_port.Mask = "99999";
            this.tbx_port.Name = "tbx_port";
            this.tbx_port.Size = new System.Drawing.Size(43, 21);
            this.tbx_port.TabIndex = 2;
            this.tbx_port.Text = "18245";
            this.tbx_port.ValidatingType = typeof(int);
            // 
            // btn_close
            // 
            this.btn_close.Location = new System.Drawing.Point(349, 21);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(75, 23);
            this.btn_close.TabIndex = 4;
            this.btn_close.Text = "关闭";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(166, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "端口";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "IP";
            // 
            // tbx_ip
            // 
            this.tbx_ip.Location = new System.Drawing.Point(54, 22);
            this.tbx_ip.Name = "tbx_ip";
            this.tbx_ip.Size = new System.Drawing.Size(100, 21);
            this.tbx_ip.TabIndex = 1;
            this.tbx_ip.Text = "127.0.0.1";
            // 
            // btn_open
            // 
            this.btn_open.Location = new System.Drawing.Point(268, 21);
            this.btn_open.Name = "btn_open";
            this.btn_open.Size = new System.Drawing.Size(75, 23);
            this.btn_open.TabIndex = 3;
            this.btn_open.Text = "连接";
            this.btn_open.UseVisualStyleBackColor = true;
            this.btn_open.Click += new System.EventHandler(this.btn_open_Click);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(283, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "最大字节:";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDown1.Location = new System.Drawing.Point(348, 4);
            this.numericUpDown1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 3);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(42, 21);
            this.numericUpDown1.TabIndex = 30;
            this.numericUpDown1.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // btn_add
            // 
            this.btn_add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_add.Location = new System.Drawing.Point(820, 6);
            this.btn_add.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(75, 23);
            this.btn_add.TabIndex = 31;
            this.btn_add.Text = "添加";
            this.btn_add.UseVisualStyleBackColor = true;
            this.btn_add.Click += new System.EventHandler(this.btn_add_Click);
            // 
            // btn_read
            // 
            this.btn_read.Location = new System.Drawing.Point(396, 3);
            this.btn_read.Name = "btn_read";
            this.btn_read.Size = new System.Drawing.Size(75, 23);
            this.btn_read.TabIndex = 32;
            this.btn_read.Text = "读取";
            this.btn_read.UseVisualStyleBackColor = true;
            this.btn_read.Click += new System.EventHandler(this.btn_read_Click);
            // 
            // lv_data
            // 
            this.lv_data.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.lv_data.ContextMenuStrip = this.menu_lv;
            this.lv_data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lv_data.FullRowSelect = true;
            this.lv_data.GridLines = true;
            this.lv_data.HideSelection = false;
            this.lv_data.Location = new System.Drawing.Point(3, 39);
            this.lv_data.MultiSelect = false;
            this.lv_data.Name = "lv_data";
            this.lv_data.Size = new System.Drawing.Size(601, 181);
            this.lv_data.TabIndex = 6;
            this.lv_data.UseCompatibleStateImageBehavior = false;
            this.lv_data.View = System.Windows.Forms.View.Details;
            this.lv_data.SelectedIndexChanged += new System.EventHandler(this.lv_data_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "序号";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "地址";
            this.columnHeader2.Width = 90;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "数据类型";
            this.columnHeader3.Width = 100;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "数值";
            this.columnHeader4.Width = 100;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "数值(BIN)";
            this.columnHeader5.Width = 100;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "数值(HEX)";
            this.columnHeader6.Width = 100;
            // 
            // menu_lv
            // 
            this.menu_lv.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tm_exportExcel,
            this.clearAllToolStripMenuItem});
            this.menu_lv.Name = "muen_lv";
            this.menu_lv.Size = new System.Drawing.Size(130, 48);
            // 
            // tm_exportExcel
            // 
            this.tm_exportExcel.Name = "tm_exportExcel";
            this.tm_exportExcel.Size = new System.Drawing.Size(129, 22);
            this.tm_exportExcel.Text = "导出Excel";
            this.tm_exportExcel.Click += new System.EventHandler(this.tm_exportExcel_Click);
            // 
            // clearAllToolStripMenuItem
            // 
            this.clearAllToolStripMenuItem.Name = "clearAllToolStripMenuItem";
            this.clearAllToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.clearAllToolStripMenuItem.Text = "清空列表";
            this.clearAllToolStripMenuItem.Click += new System.EventHandler(this.clearAllToolStripMenuItem_Click);
            // 
            // tbx_address
            // 
            this.tbx_address.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbx_address.Location = new System.Drawing.Point(610, 39);
            this.tbx_address.Multiline = true;
            this.tbx_address.Name = "tbx_address";
            this.tbx_address.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbx_address.Size = new System.Drawing.Size(285, 181);
            this.tbx_address.TabIndex = 30;
            this.tbx_address.WordWrap = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbx_type);
            this.groupBox2.Controls.Add(this.chk_enablewrite);
            this.groupBox2.Controls.Add(this.btn_write);
            this.groupBox2.Controls.Add(this.btn_readOne);
            this.groupBox2.Controls.Add(this.tbx_value);
            this.groupBox2.Controls.Add(this.tbx_addressOne);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 54);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(935, 54);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "单个读取/写入";
            // 
            // cbx_type
            // 
            this.cbx_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_type.FormattingEnabled = true;
            this.cbx_type.Location = new System.Drawing.Point(160, 23);
            this.cbx_type.Name = "cbx_type";
            this.cbx_type.Size = new System.Drawing.Size(100, 20);
            this.cbx_type.TabIndex = 5;
            // 
            // chk_enablewrite
            // 
            this.chk_enablewrite.AutoSize = true;
            this.chk_enablewrite.Location = new System.Drawing.Point(465, 25);
            this.chk_enablewrite.Name = "chk_enablewrite";
            this.chk_enablewrite.Size = new System.Drawing.Size(60, 16);
            this.chk_enablewrite.TabIndex = 25;
            this.chk_enablewrite.Text = "开启写";
            this.chk_enablewrite.UseVisualStyleBackColor = true;
            this.chk_enablewrite.CheckedChanged += new System.EventHandler(this.ckb_enablewrite_CheckedChanged);
            this.chk_enablewrite.Click += new System.EventHandler(this.chk_enablewrite_Click);
            // 
            // btn_write
            // 
            this.btn_write.Location = new System.Drawing.Point(529, 22);
            this.btn_write.Name = "btn_write";
            this.btn_write.Size = new System.Drawing.Size(75, 23);
            this.btn_write.TabIndex = 21;
            this.btn_write.Text = "写入";
            this.btn_write.UseVisualStyleBackColor = true;
            this.btn_write.Click += new System.EventHandler(this.btn_write_Click);
            // 
            // btn_readOne
            // 
            this.btn_readOne.Location = new System.Drawing.Point(268, 22);
            this.btn_readOne.Name = "btn_readOne";
            this.btn_readOne.Size = new System.Drawing.Size(75, 23);
            this.btn_readOne.TabIndex = 6;
            this.btn_readOne.Text = "读取";
            this.btn_readOne.UseVisualStyleBackColor = true;
            this.btn_readOne.Click += new System.EventHandler(this.btn_readOne_Click);
            // 
            // tbx_value
            // 
            this.tbx_value.Location = new System.Drawing.Point(349, 23);
            this.tbx_value.Name = "tbx_value";
            this.tbx_value.Size = new System.Drawing.Size(110, 21);
            this.tbx_value.TabIndex = 20;
            this.tbx_value.Text = "0";
            // 
            // tbx_addressOne
            // 
            this.tbx_addressOne.Location = new System.Drawing.Point(54, 23);
            this.tbx_addressOne.Name = "tbx_addressOne";
            this.tbx_addressOne.Size = new System.Drawing.Size(100, 21);
            this.tbx_addressOne.TabIndex = 4;
            this.tbx_addressOne.Text = "M100";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "地址";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tableLayoutPanel1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 108);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(935, 383);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "批量读取";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.btn_add, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tbx_address, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lv_data, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.progressBar1, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 20);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(898, 243);
            this.tableLayoutPanel1.TabIndex = 35;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.label7);
            this.flowLayoutPanel1.Controls.Add(this.tbx_time);
            this.flowLayoutPanel1.Controls.Add(this.label6);
            this.flowLayoutPanel1.Controls.Add(this.tbx_num);
            this.flowLayoutPanel1.Controls.Add(this.label3);
            this.flowLayoutPanel1.Controls.Add(this.numericUpDown1);
            this.flowLayoutPanel1.Controls.Add(this.btn_read);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(601, 30);
            this.flowLayoutPanel1.TabIndex = 36;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 6;
            this.label7.Text = "读取间隔(ms)";
            // 
            // tbx_time
            // 
            this.tbx_time.Location = new System.Drawing.Point(86, 4);
            this.tbx_time.Margin = new System.Windows.Forms.Padding(3, 4, 3, 3);
            this.tbx_time.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.tbx_time.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.tbx_time.Name = "tbx_time";
            this.tbx_time.Size = new System.Drawing.Size(63, 21);
            this.tbx_time.TabIndex = 7;
            this.tbx_time.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(155, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 4;
            this.label6.Text = "读取次数";
            // 
            // tbx_num
            // 
            this.tbx_num.Location = new System.Drawing.Point(214, 4);
            this.tbx_num.Margin = new System.Windows.Forms.Padding(3, 4, 3, 3);
            this.tbx_num.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.tbx_num.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.tbx_num.Name = "tbx_num";
            this.tbx_num.Size = new System.Drawing.Size(63, 21);
            this.tbx_num.TabIndex = 5;
            this.tbx_num.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // progressBar1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.progressBar1, 2);
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar1.Location = new System.Drawing.Point(3, 226);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(892, 14);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 35;
            // 
            // GePLC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "GePLC";
            this.Size = new System.Drawing.Size(935, 491);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.menu_lv.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbx_time)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbx_num)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbx_ip;
        private System.Windows.Forms.Button btn_open;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.MaskedTextBox tbx_port;
        private ListViewEx lv_data;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.TextBox tbx_address;
        private System.Windows.Forms.Button btn_read;
        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox tbx_addressOne;
        private System.Windows.Forms.TextBox tbx_value;
        private System.Windows.Forms.Button btn_readOne;
        private System.Windows.Forms.ContextMenuStrip menu_lv;
        private System.Windows.Forms.ToolStripMenuItem tm_exportExcel;
        private System.Windows.Forms.Button btn_write;
        private System.Windows.Forms.CheckBox chk_enablewrite;
        private System.Windows.Forms.ComboBox cbx_type;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStripMenuItem clearAllToolStripMenuItem;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown tbx_time;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown tbx_num;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
    }
}

