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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlcClient.Controls
{
    public partial class WebClient : BaseControl
    {
        public string Address { get; set; } = "ws://127.0.0.1:7000/ws";
        public int SendCount { get; set; } = 1;
        public int SendInterval { get; set; } = 1000; //ms

        public string SendData { get; set; } = "Hello!";

        private ClientWebSocket _webSocket;
        private CancellationTokenSource _cts;
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
                tbx_received.SelectionColor = Color.FromArgb(255, 0, new Random().Next(100, 255), new Random().Next(1, 128));
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
            try
            {
                _webSocket?.Dispose();
                _cts?.Cancel();
                _cts = new CancellationTokenSource();
                _webSocket = new ClientWebSocket();
                Task.Run(async () =>
                {
                    await _webSocket.ConnectAsync(new Uri(Address), _cts.Token);
                    await ReceiveMessage(_webSocket);
                }, _cts.Token);
            }
            catch (Exception ex)
            {
                MessageBox.Show("WebSocket 连接出错", ex.Message);
            }

        }


        private async Task ReceiveMessage(ClientWebSocket webSocket)
        {
            try
            {
                OnMsg($"{Address} 连接成功！");
                this.Invoke(() =>
                {
                    tbx_addr.Enabled = btn_open.Enabled = false;
                    btn_close.Enabled = true;
                });
                var buffer = new byte[1024 * 8];
                while (webSocket.State == WebSocketState.Open)
                {
                    if (_cts.IsCancellationRequested)
                        break;
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), _cts.Token);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        OnMsg($"{Address} 连接断开！");
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "关闭", CancellationToken.None);
                        break;
                    }
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var msg = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        OnReceive(msg);
                    }
                    else
                    {
                        OnReceive($"收到非文本消息：{result.MessageType}");
                    }
                }
                buffer = null;
            }
            catch (WebSocketException ex)
            {
                OnMsg("Connection closed unexpectedly: " + ex.Message);
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "关闭", CancellationToken.None);
            }
            catch (Exception ex)
            {
                OnMsg(ex.Message);
            }
            finally
            {                
                btn_close_Click(null, null);
            }            
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            _cts?.Cancel();
            this.Invoke(() =>
            {
                tbx_addr.Enabled = btn_open.Enabled = true;
                btn_close.Enabled = false;
            });
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            if (_webSocket != null && _webSocket.State == WebSocketState.Open)
            {
                btn_send.Enabled = false;
                Task.Run(async () =>
                {
                    for (int i = 0; i < SendCount; i++)
                    {
                        await _webSocket.SendAsync(new ArraySegment<byte>(SendData.GetBytes()), WebSocketMessageType.Text, true, default);
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
