using NewLife;
using NewLife.Http;
using NewLife.Net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using NewLife.Serialization;
using NewLife.Data;
using System.Linq;
using HL.Object.Extensions;
namespace PlcClient.Controls
{
    public partial class WebSocketServer : BaseControl
    {
        public string ServerMode { get; set; } = "WebSocket";
        public string ServerIP { get; set; } = "0.0.0.0";
        public int ServerPort { get; set; } = 7000;
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

        private void OnReceive(string remote, string msg)
        {
            tbx_received.Invoke(() =>
            {
                tbx_received.AppendText(string.Format("{0} [{1}]{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), remote, Environment.NewLine));
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
                    Server.Map(ServerUrl, HttpHandler);
                }
                Server.Start();
                cbx_mode.Enabled = cbx_ip.Enabled = tbx_port.Enabled = tbx_path.Enabled = btn_start.Enabled = false;
                btn_stop.Enabled = true;
                ReceiveMessage = $"{ServerMode.ToLower()}://{ServerIP};{ServerPort}{ServerUrl}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("启动Web服务失败！", ex.Message);
            }


        }
        public void HttpHandler(IHttpContext content)
        {
            content.Response.SetResult(this.SendMessage);
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"http://{content.Request.Host}{content.Request.RequestUri}\n");
            sb.AppendLine("[HTTP Headers]");
            sb.AppendLine("Path:" + content.Path);
            sb.AppendLine("Method:" + content.Request.Method);
            foreach (var header in content.Request.Headers)
            {
                sb.AppendLine(header.Key + ":" + header.Value);
            }
            var querykv = content.Request.RequestUri.ToString().Split("?")[1]?.Split('&').ToDictionary(kv => kv.Split('=')[0], kv => kv.Split('=')[1] ?? "");
            sb.AppendLine();
            sb.AppendLine("[HTTP QueryString]");
            foreach (var kv in querykv)
            {
                sb.AppendLine(kv.Key + " = " + kv.Value);
            }
            sb.AppendLine();
            sb.AppendLine("[HTTP Body]");
            if (content.Request.ContentType.Contains("x-www-form-urlencoded") || content.Request.ContentType.Contains("form-data"))
            {
                foreach (var kv in content.Parameters)
                {
                    if (querykv.ContainsKey(kv.Key))
                        continue;
                    var val = kv.Value;
                    if (val is FormFile file)
                    {
                        val = file.FileName;
                    }
                    sb.AppendLine(kv.Key + " = " + val);
                }
            }
            else
            {
                sb.AppendLine(content.Request.Body.ToStr());
            }

            if (content.Request.Files != null && content.Request.Files.Count() > 0)
            {
                sb.AppendLine();
                sb.AppendLine("[HTTP Files]");
                foreach (var file in content.Request.Files)
                {
                    var filename = "tmp/upload/" + file.FileName;
                    if (!System.IO.Directory.Exists("tmp/upload")) System.IO.Directory.CreateDirectory("tmp/upload");
                    file.SaveToFile(filename);
                    sb.AppendLine($"文件：{file.FileName} 大小：{file.Length} 类型：{file.ContentType} 路径：{filename}");
                }
            }
            this.Invoke(() =>
            {
                this.ReceiveMessage = sb.ToString();
            });
        }

        private IDictionary<string, WebSocket> _clients = new Dictionary<string, WebSocket>();
        public void WebSocketHandler(IHttpContext content)
        {
            var ws = content.WebSocket;
            ws.Handler = ProcessMessage;
            OnReceive(ws.Context.Connection.Remote.ToString(), "WebSocket连接建立");
            // 加入客户端列表
            _clients.Add(ws.Context.Connection.Remote.ToString(), ws);
            if (cbx_reply.Checked)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(500);
                    ws.Send("Welcome to WebSocket Server");
                });
            }
        }

        public void ProcessMessage(WebSocket socket, WebSocketMessage message)
        {
            var remote = socket.Context.Connection.Remote.ToString();
            var msg = message.Payload.GetSpan().ToStr();
            switch (message.Type)
            {
                case WebSocketMessageType.Text:
                    OnReceive(remote, msg);
                    // 群发所有客户端
                    //socket.SendAll($"[{remote}]说，{msg}");
                    if (cbx_reply.Checked)
                        socket.Send(msg);
                    //socket.SendAll(msg, (s) => s.Session.Remote == remote);
                    break;
                case WebSocketMessageType.Close:
                    OnReceive(remote, string.Format("关闭连接 [{0}] {1}", message.CloseStatus, message.StatusDescription));
                    break;
                case WebSocketMessageType.Ping:
                case WebSocketMessageType.Pong:
                    OnMsg($"{remote} {message.Type}");
                    break;
                default:
                    OnReceive(remote, msg);
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
