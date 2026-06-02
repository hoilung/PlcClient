namespace PlcClient.Controls
{
    partial class Netstat
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.ts_cbx_type = new System.Windows.Forms.ToolStripComboBox();
            this.ts_btn_search = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ts_btn_export = new System.Windows.Forms.ToolStripButton();
            this.ts_btn_clear = new System.Windows.Forms.ToolStripButton();
            this.toolStripCheckBox1 = new PlcClient.Controls.ToolStripCheckBox();
            this.listViewEx1 = new PlcClient.Controls.ListViewEx();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyselectrowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openselectrowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.ts_cbx_type,
            this.ts_btn_search,
            this.toolStripSeparator1,
            this.toolStripCheckBox1,
            this.ts_btn_export,
            this.ts_btn_clear});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(618, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(56, 22);
            this.toolStripLabel1.Text = "查询类型";
            // 
            // ts_cbx_type
            // 
            this.ts_cbx_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ts_cbx_type.Name = "ts_cbx_type";
            this.ts_cbx_type.Size = new System.Drawing.Size(121, 25);
            // 
            // ts_btn_search
            // 
            this.ts_btn_search.Image = global::PlcClient.Properties.Resources.Search_in_List;
            this.ts_btn_search.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ts_btn_search.Name = "ts_btn_search";
            this.ts_btn_search.Size = new System.Drawing.Size(76, 22);
            this.ts_btn_search.Text = "开始查询";
            this.ts_btn_search.ToolTipText = "非管理员权限查询不显示进程路径";
            this.ts_btn_search.Click += new System.EventHandler(this.ts_btn_search_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // ts_btn_export
            // 
            this.ts_btn_export.Image = global::PlcClient.Properties.Resources.XLS;
            this.ts_btn_export.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ts_btn_export.Name = "ts_btn_export";
            this.ts_btn_export.Size = new System.Drawing.Size(76, 22);
            this.ts_btn_export.Text = "导出列表";
            this.ts_btn_export.Click += new System.EventHandler(this.ts_btn_export_Click);
            // 
            // ts_btn_clear
            // 
            this.ts_btn_clear.Image = global::PlcClient.Properties.Resources.Trash_Can;
            this.ts_btn_clear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ts_btn_clear.Name = "ts_btn_clear";
            this.ts_btn_clear.Size = new System.Drawing.Size(76, 22);
            this.ts_btn_clear.Text = "清空列表";
            this.ts_btn_clear.Click += new System.EventHandler(this.ts_btn_clear_Click);
            // 
            // toolStripCheckBox1
            // 
            this.toolStripCheckBox1.Checked = false;
            this.toolStripCheckBox1.Name = "toolStripCheckBox1";
            this.toolStripCheckBox1.Size = new System.Drawing.Size(79, 22);
            this.toolStripCheckBox1.Text = "显示进程";
            // 
            // listViewEx1
            // 
            this.listViewEx1.FullRowSelect = true;
            this.listViewEx1.GridLines = true;
            this.listViewEx1.HideSelection = false;
            this.listViewEx1.Location = new System.Drawing.Point(3, 3);
            this.listViewEx1.Name = "listViewEx1";
            this.listViewEx1.Size = new System.Drawing.Size(347, 147);
            this.listViewEx1.TabIndex = 0;
            this.listViewEx1.UseCompatibleStateImageBehavior = false;
            this.listViewEx1.View = System.Windows.Forms.View.Details;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyselectrowToolStripMenuItem,
            this.openselectrowToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(173, 48);
            // 
            // copyselectrowToolStripMenuItem
            // 
            this.copyselectrowToolStripMenuItem.Image = global::PlcClient.Properties.Resources.Copy;
            this.copyselectrowToolStripMenuItem.Name = "copyselectrowToolStripMenuItem";
            this.copyselectrowToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.copyselectrowToolStripMenuItem.Text = "复制选中行";
            this.copyselectrowToolStripMenuItem.Click += new System.EventHandler(this.copyselectrowToolStripMenuItem_Click);
            // 
            // openselectrowToolStripMenuItem
            // 
            this.openselectrowToolStripMenuItem.Image = global::PlcClient.Properties.Resources.Opened_Folder;
            this.openselectrowToolStripMenuItem.Name = "openselectrowToolStripMenuItem";
            this.openselectrowToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.openselectrowToolStripMenuItem.Text = "打开选中进程路径";
            this.openselectrowToolStripMenuItem.Click += new System.EventHandler(this.openselectrowToolStripMenuItem_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.listViewEx1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(21, 55);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(407, 179);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // Netstat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "Netstat";
            this.Size = new System.Drawing.Size(618, 347);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox ts_cbx_type;
        private System.Windows.Forms.ToolStripButton ts_btn_search;
        private ListViewEx listViewEx1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton ts_btn_export;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStripButton ts_btn_clear;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copyselectrowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openselectrowToolStripMenuItem;
        private ToolStripCheckBox toolStripCheckBox1;
    }
}
