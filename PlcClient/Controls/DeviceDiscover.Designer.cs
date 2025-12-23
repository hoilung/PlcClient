namespace PlcClient.Controls
{
    partial class DeviceDiscover
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.tbx_ip = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.cbx_deviceType = new System.Windows.Forms.ToolStripComboBox();
            this.btn_find = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btn_exportExcel = new System.Windows.Forms.ToolStripButton();
            this.btn_clear = new System.Windows.Forms.ToolStripButton();
            this.lv_data = new PlcClient.Controls.ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openWebBrowserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.copyRTSPaddressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showDeviceNameToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.toolStrip1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lv_data, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(19, 54);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(648, 300);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2,
            this.tbx_ip,
            this.toolStripSeparator2,
            this.toolStripLabel1,
            this.cbx_deviceType,
            this.btn_find,
            this.toolStripSeparator1,
            this.btn_exportExcel,
            this.btn_clear});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(648, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(43, 22);
            this.toolStripLabel2.Text = "本机IP";
            // 
            // tbx_ip
            // 
            this.tbx_ip.AutoToolTip = true;
            this.tbx_ip.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tbx_ip.Name = "tbx_ip";
            this.tbx_ip.Size = new System.Drawing.Size(120, 25);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(56, 22);
            this.toolStripLabel1.Text = "设备厂家";
            // 
            // cbx_deviceType
            // 
            this.cbx_deviceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_deviceType.DropDownWidth = 100;
            this.cbx_deviceType.Items.AddRange(new object[] {
            "海康",
            "大华",
            "ONVIF"});
            this.cbx_deviceType.Name = "cbx_deviceType";
            this.cbx_deviceType.Size = new System.Drawing.Size(100, 25);
            // 
            // btn_find
            // 
            this.btn_find.Image = global::PlcClient.Properties.Resources.Search_in_List;
            this.btn_find.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_find.Name = "btn_find";
            this.btn_find.Size = new System.Drawing.Size(76, 22);
            this.btn_find.Text = "开始搜索";
            this.btn_find.Click += new System.EventHandler(this.btn_find_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btn_exportExcel
            // 
            this.btn_exportExcel.Image = global::PlcClient.Properties.Resources.XLS;
            this.btn_exportExcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_exportExcel.Name = "btn_exportExcel";
            this.btn_exportExcel.Size = new System.Drawing.Size(76, 22);
            this.btn_exportExcel.Text = "导出表格";
            this.btn_exportExcel.Click += new System.EventHandler(this.btn_exportExcel_Click);
            // 
            // btn_clear
            // 
            this.btn_clear.Image = global::PlcClient.Properties.Resources.Trash_Can;
            this.btn_clear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_clear.Name = "btn_clear";
            this.btn_clear.Size = new System.Drawing.Size(76, 22);
            this.btn_clear.Text = "清空列表";
            this.btn_clear.Click += new System.EventHandler(this.btn_clear_Click);
            // 
            // lv_data
            // 
            this.lv_data.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lv_data.AllowColumnReorder = true;
            this.lv_data.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lv_data.FullRowSelect = true;
            this.lv_data.GridLines = true;
            this.lv_data.HideSelection = false;
            this.lv_data.Location = new System.Drawing.Point(3, 28);
            this.lv_data.Name = "lv_data";
            this.lv_data.Size = new System.Drawing.Size(584, 247);
            this.lv_data.TabIndex = 0;
            this.lv_data.UseCompatibleStateImageBehavior = false;
            this.lv_data.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "序号";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openWebBrowserToolStripMenuItem,
            this.toolStripSeparator3,
            this.copyRTSPaddressToolStripMenuItem,
            this.showDeviceNameToolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(185, 110);
            // 
            // openWebBrowserToolStripMenuItem
            // 
            this.openWebBrowserToolStripMenuItem.Image = global::PlcClient.Properties.Resources.Internet_Explorer;
            this.openWebBrowserToolStripMenuItem.Name = "openWebBrowserToolStripMenuItem";
            this.openWebBrowserToolStripMenuItem.Size = new System.Drawing.Size(184, 26);
            this.openWebBrowserToolStripMenuItem.Text = "打开浏览器";
            this.openWebBrowserToolStripMenuItem.Click += new System.EventHandler(this.openWebBrowserToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(181, 6);
            // 
            // copyRTSPaddressToolStripMenuItem
            // 
            this.copyRTSPaddressToolStripMenuItem.Image = global::PlcClient.Properties.Resources.Copy;
            this.copyRTSPaddressToolStripMenuItem.Name = "copyRTSPaddressToolStripMenuItem";
            this.copyRTSPaddressToolStripMenuItem.Size = new System.Drawing.Size(184, 26);
            this.copyRTSPaddressToolStripMenuItem.Text = "复制RTSP地址";
            this.copyRTSPaddressToolStripMenuItem.Click += new System.EventHandler(this.copyRTSPaddressToolStripMenuItem_Click);
            // 
            // showDeviceNameToolStripMenuItem1
            // 
            this.showDeviceNameToolStripMenuItem1.Image = global::PlcClient.Properties.Resources.Video_Call;
            this.showDeviceNameToolStripMenuItem1.Name = "showDeviceNameToolStripMenuItem1";
            this.showDeviceNameToolStripMenuItem1.Size = new System.Drawing.Size(184, 26);
            this.showDeviceNameToolStripMenuItem1.Text = "查看设备信息";
            this.showDeviceNameToolStripMenuItem1.Click += new System.EventHandler(this.showDeviceNameToolStripMenuItem1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Location = new System.Drawing.Point(13, 17);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(673, 369);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "网络监控设备搜索";
            // 
            // DeviceDiscover
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "DeviceDiscover";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private ListViewEx lv_data;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox cbx_deviceType;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripButton btn_find;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStripComboBox tbx_ip;
        private System.Windows.Forms.ToolStripButton btn_exportExcel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btn_clear;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openWebBrowserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyRTSPaddressToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem showDeviceNameToolStripMenuItem1;
    }
}
