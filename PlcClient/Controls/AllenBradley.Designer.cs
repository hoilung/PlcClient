namespace PlcClient.Controls
{
    partial class AllenBradley
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbx_type = new System.Windows.Forms.ComboBox();
            this.chk_enablewrite = new System.Windows.Forms.CheckBox();
            this.btn_write = new System.Windows.Forms.Button();
            this.btn_readOne = new System.Windows.Forms.Button();
            this.tbx_value = new System.Windows.Forms.TextBox();
            this.tbx_addressOne = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbx_slot = new System.Windows.Forms.MaskedTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbx_port = new System.Windows.Forms.MaskedTextBox();
            this.btn_close = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbx_ip = new System.Windows.Forms.TextBox();
            this.btn_open = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btn_read = new System.Windows.Forms.Button();
            this.btn_add = new System.Windows.Forms.Button();
            this.tbx_address = new System.Windows.Forms.TextBox();
            this.lv_data = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lv_menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tm_exportExcel = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.lv_menu.SuspendLayout();
            this.SuspendLayout();
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
            this.groupBox2.Location = new System.Drawing.Point(7, 69);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(695, 54);
            this.groupBox2.TabIndex = 9;
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
            this.cbx_type.TabIndex = 26;
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
            this.chk_enablewrite.CheckedChanged += new System.EventHandler(this.chk_enablewrite_CheckedChanged);
            // 
            // btn_write
            // 
            this.btn_write.Location = new System.Drawing.Point(529, 22);
            this.btn_write.Name = "btn_write";
            this.btn_write.Size = new System.Drawing.Size(75, 23);
            this.btn_write.TabIndex = 24;
            this.btn_write.Text = "写入";
            this.btn_write.UseVisualStyleBackColor = true;
            this.btn_write.Click += new System.EventHandler(this.btn_write_Click);
            // 
            // btn_readOne
            // 
            this.btn_readOne.Location = new System.Drawing.Point(268, 22);
            this.btn_readOne.Name = "btn_readOne";
            this.btn_readOne.Size = new System.Drawing.Size(75, 23);
            this.btn_readOne.TabIndex = 22;
            this.btn_readOne.Text = "读取";
            this.btn_readOne.UseVisualStyleBackColor = true;
            this.btn_readOne.Click += new System.EventHandler(this.btn_readOne_Click);
            // 
            // tbx_value
            // 
            this.tbx_value.Location = new System.Drawing.Point(349, 23);
            this.tbx_value.Name = "tbx_value";
            this.tbx_value.Size = new System.Drawing.Size(110, 21);
            this.tbx_value.TabIndex = 4;
            this.tbx_value.Text = "0";
            // 
            // tbx_addressOne
            // 
            this.tbx_addressOne.Location = new System.Drawing.Point(54, 23);
            this.tbx_addressOne.Name = "tbx_addressOne";
            this.tbx_addressOne.Size = new System.Drawing.Size(100, 21);
            this.tbx_addressOne.TabIndex = 20;
            this.tbx_addressOne.Text = "A1";
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbx_slot);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tbx_port);
            this.groupBox1.Controls.Add(this.btn_close);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tbx_ip);
            this.groupBox1.Controls.Add(this.btn_open);
            this.groupBox1.Location = new System.Drawing.Point(7, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(695, 54);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "AllenBradley 设置";
            // 
            // tbx_slot
            // 
            this.tbx_slot.Location = new System.Drawing.Point(300, 22);
            this.tbx_slot.Mask = "99";
            this.tbx_slot.Name = "tbx_slot";
            this.tbx_slot.Size = new System.Drawing.Size(43, 21);
            this.tbx_slot.TabIndex = 5;
            this.tbx_slot.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(265, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "槽号";
            // 
            // tbx_port
            // 
            this.tbx_port.Location = new System.Drawing.Point(201, 22);
            this.tbx_port.Mask = "99999";
            this.tbx_port.Name = "tbx_port";
            this.tbx_port.Size = new System.Drawing.Size(43, 21);
            this.tbx_port.TabIndex = 2;
            this.tbx_port.Text = "44818";
            this.tbx_port.ValidatingType = typeof(int);
            // 
            // btn_close
            // 
            this.btn_close.Location = new System.Drawing.Point(431, 21);
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
            this.tbx_ip.Text = "192.168.1.100";
            // 
            // btn_open
            // 
            this.btn_open.Location = new System.Drawing.Point(349, 21);
            this.btn_open.Name = "btn_open";
            this.btn_open.Size = new System.Drawing.Size(75, 23);
            this.btn_open.TabIndex = 3;
            this.btn_open.Text = "连接";
            this.btn_open.UseVisualStyleBackColor = true;
            this.btn_open.Click += new System.EventHandler(this.btn_open_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btn_read);
            this.groupBox3.Controls.Add(this.btn_add);
            this.groupBox3.Controls.Add(this.tbx_address);
            this.groupBox3.Controls.Add(this.lv_data);
            this.groupBox3.Location = new System.Drawing.Point(7, 129);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(695, 318);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "批量读取";
            // 
            // btn_read
            // 
            this.btn_read.Location = new System.Drawing.Point(610, 17);
            this.btn_read.Name = "btn_read";
            this.btn_read.Size = new System.Drawing.Size(75, 23);
            this.btn_read.TabIndex = 7;
            this.btn_read.Text = "读取";
            this.btn_read.UseVisualStyleBackColor = true;
            this.btn_read.Click += new System.EventHandler(this.btn_read_Click);
            // 
            // btn_add
            // 
            this.btn_add.Location = new System.Drawing.Point(529, 17);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(75, 23);
            this.btn_add.TabIndex = 8;
            this.btn_add.Text = "添加";
            this.btn_add.UseVisualStyleBackColor = true;
            this.btn_add.Click += new System.EventHandler(this.btn_add_Click);
            // 
            // tbx_address
            // 
            this.tbx_address.Location = new System.Drawing.Point(416, 49);
            this.tbx_address.Multiline = true;
            this.tbx_address.Name = "tbx_address";
            this.tbx_address.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbx_address.Size = new System.Drawing.Size(267, 263);
            this.tbx_address.TabIndex = 6;
            this.tbx_address.WordWrap = false;
            // 
            // lv_data
            // 
            this.lv_data.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.lv_data.ContextMenuStrip = this.lv_menu;
            this.lv_data.FullRowSelect = true;
            this.lv_data.GridLines = true;
            this.lv_data.HideSelection = false;
            this.lv_data.Location = new System.Drawing.Point(0, 49);
            this.lv_data.MultiSelect = false;
            this.lv_data.Name = "lv_data";
            this.lv_data.Size = new System.Drawing.Size(410, 263);
            this.lv_data.TabIndex = 0;
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
            // lv_menu
            // 
            this.lv_menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tm_exportExcel});
            this.lv_menu.Name = "lv_menu";
            this.lv_menu.Size = new System.Drawing.Size(181, 48);
            // 
            // tm_exportExcel
            // 
            this.tm_exportExcel.Name = "tm_exportExcel";
            this.tm_exportExcel.Size = new System.Drawing.Size(180, 22);
            this.tm_exportExcel.Text = "导出Excel";
            this.tm_exportExcel.Click += new System.EventHandler(this.tm_exportExcel_Click);
            // 
            // AllenBradley
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "AllenBradley";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.lv_menu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cbx_type;
        private System.Windows.Forms.CheckBox chk_enablewrite;
        private System.Windows.Forms.Button btn_write;
        private System.Windows.Forms.Button btn_readOne;
        private System.Windows.Forms.TextBox tbx_value;
        private System.Windows.Forms.TextBox tbx_addressOne;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.MaskedTextBox tbx_port;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbx_ip;
        private System.Windows.Forms.Button btn_open;
        private System.Windows.Forms.MaskedTextBox tbx_slot;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListView lv_data;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.TextBox tbx_address;
        private System.Windows.Forms.Button btn_read;
        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.ContextMenuStrip lv_menu;
        private System.Windows.Forms.ToolStripMenuItem tm_exportExcel;
    }
}
