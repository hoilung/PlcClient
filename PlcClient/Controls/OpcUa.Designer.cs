﻿namespace PlcClient.Controls
{
    partial class OpcUa
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_view = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbx_user = new System.Windows.Forms.TextBox();
            this.tbx_pass = new System.Windows.Forms.TextBox();
            this.cbx_verfly = new System.Windows.Forms.ComboBox();
            this.btn_close = new System.Windows.Forms.Button();
            this.btn_open = new System.Windows.Forms.Button();
            this.tbx_port = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbx_ip = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lv_data = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_view);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tbx_user);
            this.groupBox1.Controls.Add(this.tbx_pass);
            this.groupBox1.Controls.Add(this.cbx_verfly);
            this.groupBox1.Controls.Add(this.btn_close);
            this.groupBox1.Controls.Add(this.btn_open);
            this.groupBox1.Controls.Add(this.tbx_port);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbx_ip);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(701, 79);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "OPC UA 设置";
            // 
            // btn_view
            // 
            this.btn_view.Location = new System.Drawing.Point(474, 45);
            this.btn_view.Name = "btn_view";
            this.btn_view.Size = new System.Drawing.Size(75, 23);
            this.btn_view.TabIndex = 12;
            this.btn_view.Text = "浏览节点";
            this.btn_view.UseVisualStyleBackColor = true;
            this.btn_view.Click += new System.EventHandler(this.btn_view_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(171, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 11;
            this.label5.Text = "密码";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "账号";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(334, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "连接方式";
            // 
            // tbx_user
            // 
            this.tbx_user.Location = new System.Drawing.Point(65, 46);
            this.tbx_user.Name = "tbx_user";
            this.tbx_user.Size = new System.Drawing.Size(100, 21);
            this.tbx_user.TabIndex = 1;
            // 
            // tbx_pass
            // 
            this.tbx_pass.Location = new System.Drawing.Point(206, 46);
            this.tbx_pass.Name = "tbx_pass";
            this.tbx_pass.Size = new System.Drawing.Size(100, 21);
            this.tbx_pass.TabIndex = 2;
            // 
            // cbx_verfly
            // 
            this.cbx_verfly.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_verfly.FormattingEnabled = true;
            this.cbx_verfly.Items.AddRange(new object[] {
            "匿名登录",
            "账号验证"});
            this.cbx_verfly.Location = new System.Drawing.Point(393, 19);
            this.cbx_verfly.Name = "cbx_verfly";
            this.cbx_verfly.Size = new System.Drawing.Size(75, 20);
            this.cbx_verfly.TabIndex = 8;
            this.cbx_verfly.SelectedIndexChanged += new System.EventHandler(this.cbx_verfly_SelectedIndexChanged);
            // 
            // btn_close
            // 
            this.btn_close.Location = new System.Drawing.Point(393, 45);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(75, 23);
            this.btn_close.TabIndex = 7;
            this.btn_close.Text = "关闭";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // btn_open
            // 
            this.btn_open.Location = new System.Drawing.Point(312, 45);
            this.btn_open.Name = "btn_open";
            this.btn_open.Size = new System.Drawing.Size(75, 23);
            this.btn_open.TabIndex = 6;
            this.btn_open.Text = "连接";
            this.btn_open.UseVisualStyleBackColor = true;
            this.btn_open.Click += new System.EventHandler(this.btn_open_Click);
            // 
            // tbx_port
            // 
            this.tbx_port.Location = new System.Drawing.Point(206, 19);
            this.tbx_port.MaxLength = 5;
            this.tbx_port.Name = "tbx_port";
            this.tbx_port.Size = new System.Drawing.Size(100, 21);
            this.tbx_port.TabIndex = 5;
            this.tbx_port.Text = "49320";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(171, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "端口";
            // 
            // tbx_ip
            // 
            this.tbx_ip.Location = new System.Drawing.Point(65, 19);
            this.tbx_ip.Name = "tbx_ip";
            this.tbx_ip.Size = new System.Drawing.Size(100, 21);
            this.tbx_ip.TabIndex = 3;
            this.tbx_ip.Text = "127.0.0.1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "IP地址";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lv_data);
            this.groupBox2.Location = new System.Drawing.Point(3, 88);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(701, 339);
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
            this.lv_data.Name = "lv_data";
            this.lv_data.Size = new System.Drawing.Size(689, 313);
            this.lv_data.TabIndex = 0;
            this.lv_data.UseCompatibleStateImageBehavior = false;
            this.lv_data.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ID";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 200;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Value";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Type";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "AccessLevel";
            this.columnHeader5.Width = 120;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Description";
            this.columnHeader6.Width = 90;
            // 
            // OpcUa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "OpcUa";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbx_ip;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbx_port;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Button btn_open;
        private System.Windows.Forms.ComboBox cbx_verfly;
        private System.Windows.Forms.TextBox tbx_user;
        private System.Windows.Forms.TextBox tbx_pass;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_view;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView lv_data;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
    }
}
