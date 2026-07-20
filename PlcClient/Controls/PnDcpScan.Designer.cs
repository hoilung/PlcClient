namespace PlcClient.Controls
{
    partial class PnDcpScan
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
            this.listViewEx1 = new PlcClient.Controls.ListViewEx();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.cbx_ip = new System.Windows.Forms.ToolStripComboBox();
            this.btn_start = new System.Windows.Forms.ToolStripButton();
            this.btn_export = new System.Windows.Forms.ToolStripButton();
            this.btn_clear = new System.Windows.Forms.ToolStripButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.listViewEx1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.toolStrip1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(15, 38);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(600, 343);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // listViewEx1
            // 
            this.listViewEx1.FullRowSelect = true;
            this.listViewEx1.GridLines = true;
            this.listViewEx1.HideSelection = false;
            this.listViewEx1.Location = new System.Drawing.Point(3, 28);
            this.listViewEx1.Name = "listViewEx1";
            this.listViewEx1.Size = new System.Drawing.Size(549, 298);
            this.listViewEx1.TabIndex = 1;
            this.listViewEx1.UseCompatibleStateImageBehavior = false;
            this.listViewEx1.View = System.Windows.Forms.View.Details;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.cbx_ip,
            this.btn_start,
            this.btn_export,
            this.btn_clear});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(600, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(43, 22);
            this.toolStripLabel1.Text = "网卡IP";
            // 
            // cbx_ip
            // 
            this.cbx_ip.Name = "cbx_ip";
            this.cbx_ip.Size = new System.Drawing.Size(121, 25);
            // 
            // btn_start
            // 
            this.btn_start.Image = global::PlcClient.Properties.Resources.Search_in_List;
            this.btn_start.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(76, 22);
            this.btn_start.Text = "开始查找";
            this.btn_start.ToolTipText = "Profinet DCP";
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // btn_export
            // 
            this.btn_export.Image = global::PlcClient.Properties.Resources.XLS;
            this.btn_export.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_export.Name = "btn_export";
            this.btn_export.Size = new System.Drawing.Size(81, 22);
            this.btn_export.Text = "导出Excel";
            this.btn_export.Click += new System.EventHandler(this.btn_export_Click);
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Location = new System.Drawing.Point(28, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(665, 416);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设备发现 Profinet DCP";
            // 
            // PnDcpScan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "PnDcpScan";
            this.Size = new System.Drawing.Size(749, 459);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox cbx_ip;
        private System.Windows.Forms.ToolStripButton btn_start;
        private System.Windows.Forms.ToolStripButton btn_export;
        private System.Windows.Forms.ToolStripButton btn_clear;
        private ListViewEx listViewEx1;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}
