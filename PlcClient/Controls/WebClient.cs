using NewLife;
using NewLife.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlcClient.Controls
{
    public partial class WebClient : BaseControl
    {
        public string Address { get; set; } = "ws://127.0.0.1:8080/ws";
        public int SendCount { get; set; } = 1;
        public int SendInterval { get; set; } = 1000; //ms

        public string SendData { get; set; } = "Hello!";

        public WebSocketClient Client { get; set; }
        public event EventHandler<string> ReceiveEvent;

        public virtual void OnReceive(string msg)
        {
            ReceiveEvent?.Invoke(this, msg);
        }

        public WebClient()
        {
            InitializeComponent();
            Dock = tableLayoutPanel1.Dock = DockStyle.Fill;
            btn_close.Enabled = false;
            tbx_addr.TextBox.DataBindings.Add("Text", this, nameof(Address));
            nd_num.DataBindings.Add("Value", this, "SendCount");
            nd_step.DataBindings.Add("Value", this, "SendInterval");
            tbx_send.DataBindings.Add("Text", this, "SendData");
            this.ReceiveEvent += webClient_ReceiveEvent;
        }

        private void webClient_ReceiveEvent(object sender, string e)
        {
            tbx_received.Invoke(() =>
            {   
                tbx_received.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff "));
                tbx_received.AppendText(e);
                //设置最新一行数据颜色为随机颜色
                tbx_received.Select(tbx_received.TextLength - e.Length, e.Length);
                tbx_received.SelectionColor = Color.FromArgb(255,0, new Random().Next(100, 255), new Random().Next(1, 128));
                tbx_received.AppendText(Environment.NewLine);
                tbx_received.Scroll();
            });
        }

        private void btn_open_Click(object sender, EventArgs e)
        {
            //验证地址有效性，或 ws:// 或 wss://
            if (Address == null || Address.Length == 0 || !Regex.IsMatch(Address, "^(ws|wss)://"))
            {
                MessageBox.Show("地址格式不正确，请检查！");
                return;
            }
            Client = new WebSocketClient(new Uri(Address));
            Client.Closed += Client_Closed;
            Client.Error += Client_Error;
            Client.Received += Client_Received;
            Client.Opened += Client_Opened;
            Client.Open();
        }

        private void Client_Closed(object sender, EventArgs e)
        {
            btn_close_Click(null, null);
        }

        private void Client_Error(object sender, NewLife.ExceptionEventArgs e)
        {
            OnMsg("连接出错：" + e.Exception.Message);
            btn_close_Click(null, null);
        }

        private void Client_Received(object sender, ReceivedEventArgs e)
        {
            OnReceive(string.Format("[{0}]->{1}",e.Remote,e.Packet.GetSpan().ToStr()));
        }

        private void Client_Opened(object sender, EventArgs e)
        {
            OnMsg($"{Address} 连接成功！");
            this.Invoke(() =>
            {
                btn_open.Enabled = false;
                btn_close.Enabled = true;
            });

        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            if (Client != null)
            {
                Client.Close("关闭");
            }
            this.Invoke(() =>
            {
                btn_open.Enabled = true;
                btn_close.Enabled = false;
            });
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            if (Client != null && Client.Active)
            {
                btn_send.Enabled = false;
                Task.Run(async () =>
                {
                    for (int i = 0; i < SendCount; i++)
                    {
                        await Client.SendTextAsync(new NewLife.Data.Packet(SendData.GetBytes()));
                        await Task.Delay(SendInterval);
                    }
                }).ContinueWith(t =>
                {
                    btn_send.Invoke(() =>
                    {
                        btn_send.Enabled = true;
                    });
                });
            }
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            tbx_received.Clear();
        }
    }
}
