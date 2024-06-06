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
            this.components = new System.ComponentModel.Container();
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
            this.gb_act = new System.Windows.Forms.GroupBox();
            this.cbx_type = new System.Windows.Forms.ComboBox();
            this.chk_enablewrite = new System.Windows.Forms.CheckBox();
            this.btn_clearTbx = new System.Windows.Forms.Button();
            this.btn_write = new System.Windows.Forms.Button();
            this.tbx_value = new System.Windows.Forms.TextBox();
            this.btn_read = new System.Windows.Forms.Button();
            this.tbx_adr = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbx_msg = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label7 = new System.Windows.Forms.Label();
            this.tbx_time = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.tbx_num = new System.Windows.Forms.NumericUpDown();
            this.btn_readAll = new System.Windows.Forms.Button();
            this.lv_data = new PlcClient.Controls.ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Menu_lv = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItem_ExcelExport = new System.Windows.Forms.ToolStripMenuItem();
            this.clearAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tbx_addressAll = new System.Windows.Forms.TextBox();
            this.btn_add = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.gb_set.SuspendLayout();
            this.gb_act.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbx_time)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbx_num)).BeginInit();
            this.Menu_lv.SuspendLayout();
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
            this.gb_set.Dock = System.Windows.Forms.DockStyle.Top;
            this.gb_set.Location = new System.Drawing.Point(0, 0);
            this.gb_set.Name = "gb_set";
            this.gb_set.Size = new System.Drawing.Size(909, 60);
            this.gb_set.TabIndex = 0;
            this.gb_set.TabStop = false;
            this.gb_set.Text = "设置";
            // 
            // tbx_port
            // 
            this.tbx_port.Location = new System.Drawing.Point(170, 24);
            this.tbx_port.Mask = "9999";
            this.tbx_port.Name = "tbx_port";
            this.tbx_port.Size = new System.Drawing.Size(40, 21);
            this.tbx_port.TabIndex = 2;
            this.tbx_port.Text = "102";
            this.tbx_port.ValidatingType = typeof(int);
            // 
            // tbx_rack
            // 
            this.tbx_rack.Location = new System.Drawing.Point(263, 24);
            this.tbx_rack.Mask = "9";
            this.tbx_rack.Name = "tbx_rack";
            this.tbx_rack.Size = new System.Drawing.Size(40, 21);
            this.tbx_rack.TabIndex = 3;
            this.tbx_rack.Text = "0";
            this.tbx_rack.ValidatingType = typeof(int);
            // 
            // tbx_slot
            // 
            this.tbx_slot.Location = new System.Drawing.Point(344, 24);
            this.tbx_slot.Mask = "9";
            this.tbx_slot.Name = "tbx_slot";
            this.tbx_slot.Size = new System.Drawing.Size(40, 21);
            this.tbx_slot.TabIndex = 4;
            this.tbx_slot.Text = "0";
            this.tbx_slot.ValidatingType = typeof(int);
            // 
            // btn_close
            // 
            this.btn_close.Location = new System.Drawing.Point(480, 22);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(75, 23);
            this.btn_close.TabIndex = 9;
            this.btn_close.Text = "关闭";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // btn_open
            // 
            this.btn_open.Location = new System.Drawing.Point(399, 23);
            this.btn_open.Name = "btn_open";
            this.btn_open.Size = new System.Drawing.Size(75, 23);
            this.btn_open.TabIndex = 5;
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
            this.tbx_ip.Text = "127.0.0.1";
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
            // gb_act
            // 
            this.gb_act.Controls.Add(this.cbx_type);
            this.gb_act.Controls.Add(this.chk_enablewrite);
            this.gb_act.Controls.Add(this.btn_clearTbx);
            this.gb_act.Controls.Add(this.btn_write);
            this.gb_act.Controls.Add(this.tbx_value);
            this.gb_act.Controls.Add(this.btn_read);
            this.gb_act.Controls.Add(this.tbx_adr);
            this.gb_act.Controls.Add(this.label5);
            this.gb_act.Dock = System.Windows.Forms.DockStyle.Top;
            this.gb_act.Location = new System.Drawing.Point(0, 60);
            this.gb_act.Name = "gb_act";
            this.gb_act.Size = new System.Drawing.Size(909, 60);
            this.gb_act.TabIndex = 2;
            this.gb_act.TabStop = false;
            this.gb_act.Text = "单个读取/写入";
            // 
            // cbx_type
            // 
            this.cbx_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_type.FormattingEnabled = true;
            this.cbx_type.Location = new System.Drawing.Point(135, 26);
            this.cbx_type.Name = "cbx_type";
            this.cbx_type.Size = new System.Drawing.Size(100, 20);
            this.cbx_type.TabIndex = 7;
            // 
            // chk_enablewrite
            // 
            this.chk_enablewrite.AutoSize = true;
            this.chk_enablewrite.Location = new System.Drawing.Point(414, 28);
            this.chk_enablewrite.Name = "chk_enablewrite";
            this.chk_enablewrite.Size = new System.Drawing.Size(60, 16);
            this.chk_enablewrite.TabIndex = 7;
            this.chk_enablewrite.Text = "开启写";
            this.chk_enablewrite.UseVisualStyleBackColor = true;
            this.chk_enablewrite.CheckedChanged += new System.EventHandler(this.cbx_enablewrite_CheckedChanged);
            // 
            // btn_clearTbx
            // 
            this.btn_clearTbx.Location = new System.Drawing.Point(561, 25);
            this.btn_clearTbx.Name = "btn_clearTbx";
            this.btn_clearTbx.Size = new System.Drawing.Size(75, 23);
            this.btn_clearTbx.TabIndex = 22;
            this.btn_clearTbx.Text = "清空输出";
            this.btn_clearTbx.UseVisualStyleBackColor = true;
            this.btn_clearTbx.Click += new System.EventHandler(this.btn_clearTbx_Click);
            // 
            // btn_write
            // 
            this.btn_write.Location = new System.Drawing.Point(480, 25);
            this.btn_write.Name = "btn_write";
            this.btn_write.Size = new System.Drawing.Size(75, 23);
            this.btn_write.TabIndex = 21;
            this.btn_write.Text = "写入";
            this.btn_write.UseVisualStyleBackColor = true;
            this.btn_write.Click += new System.EventHandler(this.btn_write_Click);
            // 
            // tbx_value
            // 
            this.tbx_value.Location = new System.Drawing.Point(322, 26);
            this.tbx_value.Name = "tbx_value";
            this.tbx_value.Size = new System.Drawing.Size(86, 21);
            this.tbx_value.TabIndex = 20;
            this.tbx_value.Text = "0";
            // 
            // btn_read
            // 
            this.btn_read.Location = new System.Drawing.Point(241, 25);
            this.btn_read.Name = "btn_read";
            this.btn_read.Size = new System.Drawing.Size(75, 23);
            this.btn_read.TabIndex = 8;
            this.btn_read.Text = "读取";
            this.btn_read.UseVisualStyleBackColor = true;
            this.btn_read.Click += new System.EventHandler(this.btn_read_Click);
            // 
            // tbx_adr
            // 
            this.tbx_adr.Location = new System.Drawing.Point(43, 26);
            this.tbx_adr.Name = "tbx_adr";
            this.tbx_adr.Size = new System.Drawing.Size(86, 21);
            this.tbx_adr.TabIndex = 6;
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
            this.tbx_msg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbx_msg.Location = new System.Drawing.Point(3, 3);
            this.tbx_msg.Multiline = true;
            this.tbx_msg.Name = "tbx_msg";
            this.tbx_msg.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbx_msg.Size = new System.Drawing.Size(895, 392);
            this.tbx_msg.TabIndex = 3;
            this.tbx_msg.WordWrap = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 120);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(909, 424);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tbx_msg);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(901, 398);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "输出";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tableLayoutPanel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(901, 398);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "批量读取";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lv_data, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbx_addressAll, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btn_add, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.progressBar1, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 19);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(784, 339);
            this.tableLayoutPanel1.TabIndex = 43;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.Controls.Add(this.label7);
            this.flowLayoutPanel1.Controls.Add(this.tbx_time);
            this.flowLayoutPanel1.Controls.Add(this.label6);
            this.flowLayoutPanel1.Controls.Add(this.tbx_num);
            this.flowLayoutPanel1.Controls.Add(this.btn_readAll);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(574, 30);
            this.flowLayoutPanel1.TabIndex = 44;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 2;
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
            this.tbx_time.TabIndex = 3;
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
            this.label6.TabIndex = 0;
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
            this.tbx_num.TabIndex = 1;
            this.tbx_num.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btn_readAll
            // 
            this.btn_readAll.Location = new System.Drawing.Point(283, 3);
            this.btn_readAll.Name = "btn_readAll";
            this.btn_readAll.Size = new System.Drawing.Size(75, 23);
            this.btn_readAll.TabIndex = 42;
            this.btn_readAll.Text = "读取";
            this.btn_readAll.UseVisualStyleBackColor = true;
            this.btn_readAll.Click += new System.EventHandler(this.btn_readAll_Click);
            // 
            // lv_data
            // 
            this.lv_data.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader2,
            this.columnHeader6});
            this.lv_data.ContextMenuStrip = this.Menu_lv;
            this.lv_data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lv_data.FullRowSelect = true;
            this.lv_data.GridLines = true;
            this.lv_data.HideSelection = false;
            this.lv_data.Location = new System.Drawing.Point(3, 39);
            this.lv_data.MultiSelect = false;
            this.lv_data.Name = "lv_data";
            this.lv_data.Size = new System.Drawing.Size(574, 277);
            this.lv_data.TabIndex = 0;
            this.lv_data.UseCompatibleStateImageBehavior = false;
            this.lv_data.View = System.Windows.Forms.View.Details;
            this.lv_data.SelectedIndexChanged += new System.EventHandler(this.lv_data_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "序号";
            this.columnHeader1.Width = 50;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "地址";
            this.columnHeader3.Width = 90;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "数据类型";
            this.columnHeader4.Width = 75;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "数值";
            this.columnHeader5.Width = 100;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "数值(BIN)";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "数值(HEX)";
            this.columnHeader6.Width = 100;
            // 
            // Menu_lv
            // 
            this.Menu_lv.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_ExcelExport,
            this.clearAllToolStripMenuItem});
            this.Menu_lv.Name = "Menu_lv";
            this.Menu_lv.Size = new System.Drawing.Size(130, 48);
            // 
            // ToolStripMenuItem_ExcelExport
            // 
            this.ToolStripMenuItem_ExcelExport.Name = "ToolStripMenuItem_ExcelExport";
            this.ToolStripMenuItem_ExcelExport.Size = new System.Drawing.Size(129, 22);
            this.ToolStripMenuItem_ExcelExport.Text = "导出Excel";
            this.ToolStripMenuItem_ExcelExport.Click += new System.EventHandler(this.ToolStripMenuItem_ExcelExport_Click);
            // 
            // clearAllToolStripMenuItem
            // 
            this.clearAllToolStripMenuItem.Name = "clearAllToolStripMenuItem";
            this.clearAllToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.clearAllToolStripMenuItem.Text = "清空列表";
            this.clearAllToolStripMenuItem.Click += new System.EventHandler(this.clearAllToolStripMenuItem_Click);
            // 
            // tbx_addressAll
            // 
            this.tbx_addressAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbx_addressAll.Location = new System.Drawing.Point(583, 39);
            this.tbx_addressAll.Multiline = true;
            this.tbx_addressAll.Name = "tbx_addressAll";
            this.tbx_addressAll.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbx_addressAll.Size = new System.Drawing.Size(198, 277);
            this.tbx_addressAll.TabIndex = 40;
            this.tbx_addressAll.WordWrap = false;
            // 
            // btn_add
            // 
            this.btn_add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_add.Location = new System.Drawing.Point(706, 6);
            this.btn_add.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(75, 23);
            this.btn_add.TabIndex = 41;
            this.btn_add.Text = "添加";
            this.btn_add.UseVisualStyleBackColor = true;
            this.btn_add.Click += new System.EventHandler(this.btn_add_Click);
            // 
            // progressBar1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.progressBar1, 2);
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar1.Location = new System.Drawing.Point(3, 322);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(778, 14);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 45;
            // 
            // SiemensBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.gb_act);
            this.Controls.Add(this.gb_set);
            this.Name = "SiemensBase";
            this.Size = new System.Drawing.Size(909, 544);
            this.gb_set.ResumeLayout(false);
            this.gb_set.PerformLayout();
            this.gb_act.ResumeLayout(false);
            this.gb_act.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbx_time)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbx_num)).EndInit();
            this.Menu_lv.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gb_set;
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
        private System.Windows.Forms.Button btn_clearTbx;
        private System.Windows.Forms.MaskedTextBox tbx_slot;
        private System.Windows.Forms.MaskedTextBox tbx_rack;
        private System.Windows.Forms.MaskedTextBox tbx_port;
        private System.Windows.Forms.CheckBox chk_enablewrite;
        private System.Windows.Forms.ComboBox cbx_type;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private ListViewEx lv_data;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.TextBox tbx_addressAll;
        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.Button btn_readAll;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ContextMenuStrip Menu_lv;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_ExcelExport;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStripMenuItem clearAllToolStripMenuItem;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown tbx_num;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown tbx_time;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}
