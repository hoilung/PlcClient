using NewLife;
using NewLife.Http;
using NewLife.Net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
namespace PlcClient.Controls
{
    public partial class WebSocketServer : BaseControl
    {
        public string ServerMode { get; set; } = "WebSocket";
        public string ServerIP { get; set; } = "0.0.0.0";
        public int ServerPort { get; set; } = 8080;
        public string ServerUrl { get; set; } = "/ws";

        public string SendMessage { get; set; } = "Welcome";
        public string ReceiveMessage { get; set; } = "";

        public int SendCount { get; set; } = 1;
        public int SendInterval { get; set; } = 1000;

        public HttpServer Server { get; set; }

        public WebSocketServer()
        {
            InitializeComponent();
            btn_stop.Enabled = false;
            Dock = tableLayoutPanel1.Dock = DockStyle.Fill;
            cbx_ip.Items.AddRange(GetLocalAllIP());
            cbx_ip.SelectedIndex = 0;
            cbx_mode.ComboBox.DataBindings.Add("SelectedItem", this, "ServerMode");
            cbx_ip.ComboBox.DataBindings.Add("Text", this, "ServerIP");
            tbx_port.TextBox.DataBindings.Add("Text", this, "ServerPort");
            tbx_path.TextBox.DataBindings.Add("Text", this, "ServerUrl");
            tbx_send.DataBindings.Add("Text", this, "SendMessage");
            tbx_received.DataBindings.Add("Text", this, "ReceiveMessage");

            nd_num.DataBindings.Add("Value", this, "SendCount");
            nd_step.DataBindings.Add("Value", this, "SendInterval");
        }

        private void OnReceive(string msg)
        {
            tbx_received.Invoke(() =>
            {
                tbx_received.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff "));
                tbx_received.AppendText(msg);
                tbx_received.Select(tbx_received.TextLength - msg.Length, msg.Length);
                tbx_received.SelectionColor = Color.FromArgb(255, new Random().Next(0, 100), new Random().Next(100, 255), new Random().Next(1, 128));
                tbx_received.AppendText(Environment.NewLine);
                tbx_received.Scroll();
            });

        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(ServerUrl, "^/\\w+"))
            {
                MessageBox.Show("路径必须以/开头，且只能包含字母、数字、下划线");
                return;
            }

            try
            {
                this.Server = new HttpServer
                {
                    Port = ServerPort,
                };
                if (ServerMode == "WebSocket")
                {
                    Server.Map(ServerUrl, WebSocketHandler);
                    Server.NewSession += Server_NewSession;
                }
                else if (ServerMode == "HTTP")
                {
                    Server.Map(ServerUrl, () => SendMessage);
                }
                Server.Start();
                cbx_mode.Enabled=cbx_ip.Enabled=tbx_port.Enabled=tbx_path.Enabled= btn_start.Enabled = false;
                btn_stop.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("启动Web服务失败！", ex.Message);
            }


        }
        private IDictionary<string, WebSocket> _clients = new Dictionary<string, WebSocket>();
        public void WebSocketHandler(IHttpContext content)
        {
            var ws = content.WebSocket;
            ws.Handler = ProcessMessage;
            OnReceive(string.Format("[{0}]->WebSocket连接建立", ws.Context.Connection.Remote));
            _clients.Add(ws.Context.Connection.Remote.ToString(), ws);
        }

        public void ProcessMessage(WebSocket socket, WebSocketMessage message)
        {
            var remote = socket.Context.Connection.Remote;
            var msg = message.Payload.GetSpan().ToStr();
            switch (message.Type)
            {
                case WebSocketMessageType.Text:
                    OnReceive(string.Format("[{0}]->{1}", remote, msg));
                    // 群发所有客户端
                    //socket.SendAll($"[{remote}]说，{msg}");
                    if (cbx_reply.Checked)
                        socket.Send(msg);
                    //socket.SendAll(msg, (s) => s.Session.Remote == remote);
                    break;
                case WebSocketMessageType.Close:
                    OnReceive(string.Format("[{0}]->关闭连接 [{1}] {2}", remote, message.CloseStatus, message.StatusDescription));
                    break;
                case WebSocketMessageType.Ping:
                case WebSocketMessageType.Pong:
                    OnReceive(string.Format("[{0}]->{1}", remote, msg));
                    break;
                default:
                    OnReceive(string.Format("[{0}]->{1}", remote, msg));
                    break;
            }
        }

        private void Server_NewSession(object sender, NetSessionEventArgs e)
        {
            cbx_remote.Invoke(() =>
            {
                cbx_remote.Items.Add(e.Session.Remote.ToString());
            });
            e.Session.Disconnected += (s, e2) =>
            {
                cbx_remote.Invoke(() =>
                {
                    cbx_remote.Items.Remove(e.Session.Remote.ToString());
                    _clients.Remove(e.Session.Remote.ToString());
                });
            };

        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            if (Server != null)
            {
                Server.Stop("关闭WebSocket服务");
                Server = null;
            }
            cbx_mode.Enabled = cbx_ip.Enabled = tbx_port.Enabled = tbx_path.Enabled = btn_start.Enabled = true;
            btn_stop.Enabled = false;
        }

        private void cbx_all_CheckedChanged(object sender, EventArgs e)
        {
            if (cbx_remote.Items.Count < 1) return;
            for (int i = 0; i < cbx_remote.Items.Count; i++)
            {
                cbx_remote.SetItemChecked(i, cbx_all.Checked);
            }
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            if (Server == null) return;
            if (cbx_remote.CheckedItems.Count < 1) return;
            if (!Server.Active) return;

            btn_send.Enabled = false;
            Task.Run(async () =>
            {
                var msg = new WebSocketMessage();
                msg.Type = WebSocketMessageType.Text;
                msg.Payload = new NewLife.Data.Packet(SendMessage.GetBytes());
                for (int i = 0; i < SendCount; i++)
                {
                    foreach (var item in cbx_remote.CheckedItems)
                    {
                        if (_clients.TryGetValue(item.ToString(), out var client))
                        {
                            client.Send(SendMessage);
                        }
                    }
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

        private void button1_Click(object sender, EventArgs e)
        {
            tbx_received.Clear();
        }
    }
}
