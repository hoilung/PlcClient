namespace PlcClient.Controls
{
    partial class OpcDa
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_getname = new System.Windows.Forms.Button();
            this.cbx_servername = new System.Windows.Forms.ComboBox();
            this.btn_close = new System.Windows.Forms.Button();
            this.btn_open = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tbx_ip = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_BrowseView = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lv_data = new ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tbx_tag_value = new System.Windows.Forms.TextBox();
            this.btn_sub = new System.Windows.Forms.Button();
            this.btn_read = new System.Windows.Forms.Button();
            this.tbx_tag = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.MenuStrip_lv = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.export_ExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearlistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.MenuStrip_lv.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_getname);
            this.groupBox1.Controls.Add(this.cbx_servername);
            this.groupBox1.Controls.Add(this.btn_close);
            this.groupBox1.Controls.Add(this.btn_open);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbx_ip);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(701, 54);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "OPC DA 设置";
            // 
            // btn_getname
            // 
            this.btn_getname.Location = new System.Drawing.Point(369, 18);
            this.btn_getname.Name = "btn_getname";
            this.btn_getname.Size = new System.Drawing.Size(75, 23);
            this.btn_getname.TabIndex = 8;
            this.btn_getname.Text = "刷新";
            this.btn_getname.UseVisualStyleBackColor = true;
            this.btn_getname.Click += new System.EventHandler(this.btn_getname_Click);
            // 
            // cbx_servername
            // 
            this.cbx_servername.FormattingEnabled = true;
            this.cbx_servername.Location = new System.Drawing.Point(208, 19);
            this.cbx_servername.Name = "cbx_servername";
            this.cbx_servername.Size = new System.Drawing.Size(161, 20);
            this.cbx_servername.TabIndex = 7;
            // 
            // btn_close
            // 
            this.btn_close.Location = new System.Drawing.Point(533, 17);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(75, 23);
            this.btn_close.TabIndex = 5;
            this.btn_close.Text = "关闭";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // btn_open
            // 
            this.btn_open.Location = new System.Drawing.Point(452, 18);
            this.btn_open.Name = "btn_open";
            this.btn_open.Size = new System.Drawing.Size(75, 23);
            this.btn_open.TabIndex = 4;
            this.btn_open.Text = "连接";
            this.btn_open.UseVisualStyleBackColor = true;
            this.btn_open.Click += new System.EventHandler(this.btn_open_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(149, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "服务器名";
            // 
            // tbx_ip
            // 
            this.tbx_ip.Location = new System.Drawing.Point(43, 19);
            this.tbx_ip.Name = "tbx_ip";
            this.tbx_ip.Size = new System.Drawing.Size(100, 21);
            this.tbx_ip.TabIndex = 1;
            this.tbx_ip.Text = "127.0.0.1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP";
            // 
            // btn_BrowseView
            // 
            this.btn_BrowseView.Location = new System.Drawing.Point(452, 25);
            this.btn_BrowseView.Name = "btn_BrowseView";
            this.btn_BrowseView.Size = new System.Drawing.Size(75, 23);
            this.btn_BrowseView.TabIndex = 6;
            this.btn_BrowseView.Text = "浏览节点";
            this.btn_BrowseView.UseVisualStyleBackColor = true;
            this.btn_BrowseView.Click += new System.EventHandler(this.btn_BrowseView_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lv_data);
            this.groupBox2.Location = new System.Drawing.Point(3, 131);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(701, 316);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "数据列表";
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
            this.lv_data.FullRowSelect = true;
            this.lv_data.GridLines = true;
            this.lv_data.HideSelection = false;
            this.lv_data.Location = new System.Drawing.Point(6, 20);
            this.lv_data.MultiSelect = false;
            this.lv_data.Name = "lv_data";
            this.lv_data.Size = new System.Drawing.Size(689, 276);
            this.lv_data.TabIndex = 1;
            this.lv_data.UseCompatibleStateImageBehavior = false;
            this.lv_data.View = System.Windows.Forms.View.Details;
            this.lv_data.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lv_data_MouseClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ID";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Tag";
            this.columnHeader2.Width = 220;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "DataType";
            this.columnHeader3.Width = 100;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Value";
            this.columnHeader4.Width = 100;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Quality";
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Time";
            this.columnHeader6.Width = 120;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tbx_tag_value);
            this.groupBox3.Controls.Add(this.btn_sub);
            this.groupBox3.Controls.Add(this.btn_BrowseView);
            this.groupBox3.Controls.Add(this.btn_read);
            this.groupBox3.Controls.Add(this.tbx_tag);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(3, 63);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(701, 62);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "数据查询";
            // 
            // tbx_tag_value
            // 
            this.tbx_tag_value.Location = new System.Drawing.Point(289, 26);
            this.tbx_tag_value.Name = "tbx_tag_value";
            this.tbx_tag_value.Size = new System.Drawing.Size(155, 21);
            this.tbx_tag_value.TabIndex = 4;
            // 
            // btn_sub
            // 
            this.btn_sub.Location = new System.Drawing.Point(533, 25);
            this.btn_sub.Name = "btn_sub";
            this.btn_sub.Size = new System.Drawing.Size(75, 23);
            this.btn_sub.TabIndex = 3;
            this.btn_sub.Text = "订阅数据";
            this.btn_sub.UseVisualStyleBackColor = true;
            this.btn_sub.Click += new System.EventHandler(this.btn_sub_Click);
            // 
            // btn_read
            // 
            this.btn_read.Location = new System.Drawing.Point(208, 25);
            this.btn_read.Name = "btn_read";
            this.btn_read.Size = new System.Drawing.Size(75, 23);
            this.btn_read.TabIndex = 2;
            this.btn_read.Text = "读取";
            this.btn_read.UseVisualStyleBackColor = true;
            this.btn_read.Click += new System.EventHandler(this.btn_read_Click);
            // 
            // tbx_tag
            // 
            this.tbx_tag.Location = new System.Drawing.Point(43, 26);
            this.tbx_tag.Name = "tbx_tag";
            this.tbx_tag.Size = new System.Drawing.Size(159, 21);
            this.tbx_tag.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "Tag";
            // 
            // MenuStrip_lv
            // 
            this.MenuStrip_lv.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolStripMenuItem,
            this.export_ExcelToolStripMenuItem,
            this.clearlistToolStripMenuItem});
            this.MenuStrip_lv.Name = "MenuStrip_lv";
            this.MenuStrip_lv.Size = new System.Drawing.Size(146, 70);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.refreshToolStripMenuItem.Text = "刷新数据";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // export_ExcelToolStripMenuItem
            // 
            this.export_ExcelToolStripMenuItem.Name = "export_ExcelToolStripMenuItem";
            this.export_ExcelToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.export_ExcelToolStripMenuItem.Text = "导出Excel";
            this.export_ExcelToolStripMenuItem.Click += new System.EventHandler(this.export_ExcelToolStripMenuItem_Click);
            // 
            // clearlistToolStripMenuItem
            // 
            this.clearlistToolStripMenuItem.Name = "clearlistToolStripMenuItem";
            this.clearlistToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.clearlistToolStripMenuItem.Text = "清空列表";
            this.clearlistToolStripMenuItem.Click += new System.EventHandler(this.clearlistToolStripMenuItem_Click);
            // 
            // OpcDa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "OpcDa";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.MenuStrip_lv.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbx_ip;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Button btn_open;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private ListViewEx lv_data;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.Button btn_BrowseView;
        private System.Windows.Forms.ComboBox cbx_servername;
        private System.Windows.Forms.Button btn_getname;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbx_tag;
        private System.Windows.Forms.Button btn_read;
        private System.Windows.Forms.Button btn_sub;
        private System.Windows.Forms.TextBox tbx_tag_value;
        private System.Windows.Forms.ContextMenuStrip MenuStrip_lv;
        private System.Windows.Forms.ToolStripMenuItem export_ExcelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearlistToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
    }
}
