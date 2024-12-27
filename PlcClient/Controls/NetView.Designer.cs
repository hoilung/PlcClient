namespace PlcClient.Controls
{
    partial class NetView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NetView));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.netClient1 = new PlcClient.Controls.Net2Client();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.net2Server1 = new PlcClient.Controls.Net2Server();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.webClient1 = new PlcClient.Controls.WebClient();
            this.webSocketServer1 = new PlcClient.Controls.WebSocketServer();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ImageList = this.imageList1;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(710, 430);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.netClient1);
            this.tabPage1.ImageIndex = 0;
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(702, 403);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "网络客户端";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // netClient1
            // 
            this.netClient1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.netClient1.Location = new System.Drawing.Point(3, 3);
            this.netClient1.Name = "netClient1";
            this.netClient1.Size = new System.Drawing.Size(696, 397);
            this.netClient1.TabIndex = 0;
            this.netClient1.TypeCodes = new System.TypeCode[] {
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
            this.tabPage2.Controls.Add(this.net2Server1);
            this.tabPage2.ImageIndex = 1;
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(702, 403);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "网络服务端";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // net2Server1
            // 
            this.net2Server1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.net2Server1.Location = new System.Drawing.Point(3, 3);
            this.net2Server1.Name = "net2Server1";
            this.net2Server1.Size = new System.Drawing.Size(696, 397);
            this.net2Server1.TabIndex = 0;
            this.net2Server1.TypeCodes = new System.TypeCode[] {
        System.TypeCode.Boolean,
        System.TypeCode.Byte,
        System.TypeCode.Int16,
        System.TypeCode.Int32,
        System.TypeCode.Single,
        System.TypeCode.Double,
        System.TypeCode.UInt16,
        System.TypeCode.UInt32};
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.webClient1);
            this.tabPage3.ImageIndex = 0;
            this.tabPage3.Location = new System.Drawing.Point(4, 23);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(702, 403);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Web客户端";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.webSocketServer1);
            this.tabPage4.ImageIndex = 1;
            this.tabPage4.Location = new System.Drawing.Point(4, 23);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(702, 403);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Web服务端";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Thin Client.png");
            this.imageList1.Images.SetKeyName(1, "Root Server.png");
            // 
            // webClient1
            // 
            this.webClient1.Address = "ws://127.0.0.1:8080/ws";
            this.webClient1.Client = null;
            this.webClient1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webClient1.Location = new System.Drawing.Point(3, 3);
            this.webClient1.Name = "webClient1";
            this.webClient1.SendCount = 1;
            this.webClient1.SendData = "Hello,World!";
            this.webClient1.SendInterval = 1000;
            this.webClient1.Size = new System.Drawing.Size(696, 397);
            this.webClient1.TabIndex = 0;
            this.webClient1.TypeCodes = new System.TypeCode[] {
        System.TypeCode.Boolean,
        System.TypeCode.Byte,
        System.TypeCode.Int16,
        System.TypeCode.Int32,
        System.TypeCode.Single,
        System.TypeCode.Double,
        System.TypeCode.UInt16,
        System.TypeCode.UInt32};
            // 
            // webSocketServer1
            // 
            this.webSocketServer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webSocketServer1.Location = new System.Drawing.Point(3, 3);
            this.webSocketServer1.Name = "webSocketServer1";
            this.webSocketServer1.ReceiveMessage = "";
            this.webSocketServer1.SendCount = 1;
            this.webSocketServer1.SendInterval = 1000;
            this.webSocketServer1.SendMessage = "Hello,WebSocket!";
            this.webSocketServer1.Server = null;
            this.webSocketServer1.ServerIP = "0.0.0.0";
            this.webSocketServer1.ServerMode = "WebSocket";
            this.webSocketServer1.ServerPort = 8080;
            this.webSocketServer1.ServerUrl = "/ws";
            this.webSocketServer1.Size = new System.Drawing.Size(696, 397);
            this.webSocketServer1.TabIndex = 0;
            this.webSocketServer1.TypeCodes = new System.TypeCode[] {
        System.TypeCode.Boolean,
        System.TypeCode.Byte,
        System.TypeCode.Int16,
        System.TypeCode.Int32,
        System.TypeCode.Single,
        System.TypeCode.Double,
        System.TypeCode.UInt16,
        System.TypeCode.UInt32};
            // 
            // NetView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "NetView";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private Net2Client netClient1;
        private System.Windows.Forms.ImageList imageList1;
        private Net2Server net2Server1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private WebClient webClient1;
        private WebSocketServer webSocketServer1;
    }
}
