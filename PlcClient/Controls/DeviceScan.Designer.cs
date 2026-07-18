namespace PlcClient.Controls
{
    partial class DeviceScan
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.arpScanner1 = new PlcClient.Controls.ArpScanner();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.pnDcpScan1 = new PlcClient.Controls.PnDcpScan();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.lldpScan1 = new PlcClient.Controls.LLDPScan();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.ItemSize = new System.Drawing.Size(90, 20);
            this.tabControl1.Location = new System.Drawing.Point(22, 13);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(791, 443);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.arpScanner1);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(783, 415);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "TCP/ARP";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // arpScanner1
            // 
            this.arpScanner1.Location = new System.Drawing.Point(6, 21);
            this.arpScanner1.Name = "arpScanner1";
            this.arpScanner1.Size = new System.Drawing.Size(676, 328);
            this.arpScanner1.TabIndex = 0;
            this.arpScanner1.TypeCodes = new System.TypeCode[] {
        System.TypeCode.Boolean,
        System.TypeCode.Byte,
        System.TypeCode.Int16,
        System.TypeCode.Int32,
        System.TypeCode.Single,
        System.TypeCode.Double,
        System.TypeCode.UInt16,
        System.TypeCode.UInt32};
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.pnDcpScan1);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(783, 415);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "PN-DCP";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // pnDcpScan1
            // 
            this.pnDcpScan1.Location = new System.Drawing.Point(6, 17);
            this.pnDcpScan1.Name = "pnDcpScan1";
            this.pnDcpScan1.Size = new System.Drawing.Size(686, 368);
            this.pnDcpScan1.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.lldpScan1);
            this.tabPage3.Location = new System.Drawing.Point(4, 24);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(783, 415);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "LLDP";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // lldpScan1
            // 
            this.lldpScan1.Location = new System.Drawing.Point(6, 27);
            this.lldpScan1.Name = "lldpScan1";
            this.lldpScan1.Size = new System.Drawing.Size(624, 325);
            this.lldpScan1.TabIndex = 0;
            // 
            // DeviceScan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "DeviceScan";
            this.Size = new System.Drawing.Size(878, 528);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private ArpScanner arpScanner1;
        private PnDcpScan pnDcpScan1;
        private LLDPScan lldpScan1;
    }
}
