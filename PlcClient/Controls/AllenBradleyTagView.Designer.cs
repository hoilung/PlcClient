namespace PlcClient.Controls
{
    partial class AllenBradleyTagView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AllenBradleyTagView));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tv_tag = new System.Windows.Forms.TreeView();
            this.lv_data = new ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lv_Menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.export_ExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.readSelect_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.copySelect_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            this.lv_Menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.tv_tag, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lv_data, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(25, 35);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(744, 377);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tv_tag
            // 
            this.tv_tag.Location = new System.Drawing.Point(3, 3);
            this.tv_tag.Name = "tv_tag";
            this.tv_tag.Size = new System.Drawing.Size(241, 310);
            this.tv_tag.TabIndex = 0;
            // 
            // lv_data
            // 
            this.lv_data.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.lv_data.ContextMenuStrip = this.lv_Menu;
            this.lv_data.FullRowSelect = true;
            this.lv_data.GridLines = true;
            this.lv_data.HideSelection = false;
            this.lv_data.Location = new System.Drawing.Point(250, 3);
            this.lv_data.MultiSelect = false;
            this.lv_data.Name = "lv_data";
            this.lv_data.Size = new System.Drawing.Size(437, 310);
            this.lv_data.TabIndex = 1;
            this.lv_data.UseCompatibleStateImageBehavior = false;
            this.lv_data.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "序号";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "标签名称";
            this.columnHeader2.Width = 125;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "标签类型";
            this.columnHeader3.Width = 133;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "数值";
            // 
            // lv_Menu
            // 
            this.lv_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.readSelect_ToolStripMenuItem,
            this.copySelect_ToolStripMenuItem,
            this.export_ExcelToolStripMenuItem});
            this.lv_Menu.Name = "lv_Menu";
            this.lv_Menu.Size = new System.Drawing.Size(181, 92);
            // 
            // export_ExcelToolStripMenuItem
            // 
            this.export_ExcelToolStripMenuItem.Name = "export_ExcelToolStripMenuItem";
            this.export_ExcelToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.export_ExcelToolStripMenuItem.Text = "导出Excel";
            this.export_ExcelToolStripMenuItem.Click += new System.EventHandler(this.export_ExcelToolStripMenuItem_Click);
            // 
            // readSelect_ToolStripMenuItem
            // 
            this.readSelect_ToolStripMenuItem.Name = "readSelect_ToolStripMenuItem";
            this.readSelect_ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.readSelect_ToolStripMenuItem.Text = "读取选中标签数值";
            this.readSelect_ToolStripMenuItem.Click += new System.EventHandler(this.readSelect_ToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "tag");
            this.imageList1.Images.SetKeyName(1, "array");
            this.imageList1.Images.SetKeyName(2, "struct");
            // 
            // copySelect_ToolStripMenuItem
            // 
            this.copySelect_ToolStripMenuItem.Name = "copySelect_ToolStripMenuItem";
            this.copySelect_ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.copySelect_ToolStripMenuItem.Text = "复制选中标签内容";
            this.copySelect_ToolStripMenuItem.Click += new System.EventHandler(this.copySelect_ToolStripMenuItem_Click);
            // 
            // AllenBradleyTagView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "AllenBradleyTagView";
            this.Size = new System.Drawing.Size(788, 456);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.lv_Menu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TreeView tv_tag;
        private ListViewEx lv_data;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip lv_Menu;
        private System.Windows.Forms.ToolStripMenuItem export_ExcelToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ToolStripMenuItem readSelect_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copySelect_ToolStripMenuItem;
    }
}
