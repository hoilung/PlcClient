﻿using NewLife;
using NewLife.Data;
using NewLife.Net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace PlcClient.Controls
{
    public partial class Net2Server : BaseControl
    {
        private NetServer netServer;
        public Net2Server()
        {
            InitializeComponent();
            tableLayoutPanel1.Dock = DockStyle.Fill;
            this.Disposed += Net2Server_Disposed;

            cbx_code.Tag = Encoding.Default;
            cbx_code.SelectedIndex = 0;
            cbx_code.SelectedIndexChanged += Cbx_code_SelectedIndexChanged;

            cbx_ip.Items.AddRange(NetHelper.GetIPs().Where(m => m.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToArray());
            cbx_ip.SelectedIndex = 0;
            cbx_mode.SelectedIndex = 0;
            btn_stop.Enabled = false;

            this.ShowToolTip("自动回复接收到的消息", cbx_reply);
        }

        private void Net2Server_Disposed(object sender, EventArgs e)
        {
            btn_stop_Click(null, null);
        }

        private void Cbx_code_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbx_code.Text.ToLower() != "default")
                {
                    cbx_code.Tag = Encoding.GetEncoding(cbx_code.Text);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("获取指定编码错误，采用系统默认编码", "编码设置错误");
            }
        }

        private void NetServer_NewSession(object sender, NetSessionEventArgs e)
        {
            e.Session.Disconnected += Session_Disconnected;
            e.Session.Send("Welcome " + e.Session.Remote.ToString());

            OnMsg($"{DateTime.Now.ToString("[HH:mm:ss.fff]")} 客户端连接：{e.Session}");
            cbx_remote.Invoke(new Action(() =>
            {
                cbx_remote.Items.Add(e.Session.Remote);
                groupBox3.Text = string.Format("在线客户端 {0}", cbx_remote.Items.Count);
            }));
        }

        private void Session_Disconnected(object sender, EventArgs e)
        {
            if (sender is NetSession session)
            {
                OnMsg($"{DateTime.Now.ToString("[HH:mm:ss.fff]")} 客户端离线 {session}");
                cbx_remote.Invoke(() =>
                {
                    cbx_remote.Items.Remove(session.Session.Remote);
                    groupBox3.Text = groupBox3.Tag.ToString();
                });
            }

        }

        private void NetServer_Received(object sender, ReceivedEventArgs e)
        {
            if (e.Packet.Total == 0)
            {
                return;
            }
            if (cbx_reply.Checked && sender is NetSession session)
            {
                session.Send(e.Packet);
            }
            tbx_received.Invoke(new Action(() =>
            {
                //StringBuilder sb = new StringBuilder();
                if (cbx_time.Checked)
                {
                    tbx_received.AppendText($"{DateTime.Now.ToString("[HH:mm:ss.fff] ")}{e.Remote}{Environment.NewLine}");
                }
                int start = tbx_received.Text.Length;
                if (cbx_string.Checked)
                {
                    tbx_received.AppendText(e.Packet.ToStr(cbx_code.Tag as Encoding));
                }
                if (cbx_hex.Checked)
                {
                    if (cbx_string.Checked)
                        tbx_received.AppendText(Environment.NewLine);
                    tbx_received.AppendText(e.Packet.ToHex(-1, " "));
                }
                if (e.Packet.Length > 0)
                {
                    tbx_received.SelectionStart = start;
                    tbx_received.SelectionLength = tbx_received.Text.Length;
                    var rgb = Enumerable.Range(100, 156).OrderBy(m => Guid.NewGuid()).Take(3).ToArray();
                    tbx_received.SelectionColor = Color.FromArgb(50, rgb[1], rgb[2]);
                    tbx_received.Scroll();
                    tbx_received.AppendText(Environment.NewLine);
                }
            }));
        }

        private void NetServer_Error(object sender, NewLife.ExceptionEventArgs e)
        {
            cbx_remote.Invoke(new Action(() =>
            {
                cbx_remote.Items.Remove(sender);
            }));
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            try
            {
                var address = cbx_ip.Text.ParseAddress();
                var port = tbx_port.Text.ToInt();
                NetType netType = NetType.Unknown;
                switch (cbx_mode.Text.ToLower())
                {
                    case "tcp":
                        netType = NetType.Tcp;
                        break;
                    case "udp":
                        netType = NetType.Udp;
                        break;
                        //case "http":
                        //    netType = NetType.Http;
                        //    break;
                        //case "https":
                        //    netType = NetType.Https;
                        //    break;
                        //case "websocket":
                        //    netType = NetType.WebSocket;
                        //    break;
                }
                netServer = new NetServer(address, port, netType);
                netServer.Error += NetServer_Error;
                netServer.Received += NetServer_Received;
                netServer.NewSession += NetServer_NewSession;
                netServer.Start();
                if (netServer.Active)
                {
                    btn_start.Enabled = false;
                    btn_stop.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "启动服务失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            if (netServer != null && netServer.Active)
            {
                netServer.Stop("");

                btn_stop.Enabled = false;
                btn_start.Enabled = true;

                cbx_remote.Items.Clear();
                cbx_remote.ResetText();
            }
        }

        private void btn_clearSend_Click(object sender, EventArgs e)
        {
            this.tbx_send.ResetText();
        }

        private void btn_clearCallback_Click(object sender, EventArgs e)
        {
            this.tbx_received.ResetText();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var clientIP = cbx_remote.CheckedItems;
            var code = cbx_code.Tag as Encoding;
            var nd_num = this.nd_num.Value;
            var step = (int)this.nd_step.Value;
            if (clientIP.Count > 0 && netServer != null && netServer.Active)
            {
                byte[] data = null;
                if (cbx_hexSend.Checked)
                {
                    data = tbx_send.Text.Split(" ").Select(hex => (byte)Convert.ToInt32(hex, 16)).ToArray();
                }
                else
                {
                    data = code.GetBytes(tbx_send.Text);
                }
                if (data == null || data.Length == 0)
                {
                    MessageBox.Show("请输入要发送的数据", "提示");
                }
                Task.Run(async () =>
                {
                    for (int i = 0; i < nd_num; i++)
                    {
                        var result = await netServer.SendAllAsync(new NewLife.Data.Packet(data), client =>
                          {
                              return clientIP.Contains(client.Remote);                    //return false;
                          });
                        OnMsg($"{DateTime.Now.ToString("[HH:mm:ss.fff] ")} 服务端发送数据=>{result}个客户端");
                        await Task.Delay(step);
                    }
                });
            }
            else
            {
                MessageBox.Show("请选择要发送数据的客户端", "提示");
            }

        }


        private void cbx_remoteAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < cbx_remote.Items.Count; i++)
            {
                cbx_remote.SetItemChecked(i, cbx_remoteAll.Checked);
            }
        }
        //切换16 进制 或字符串
        private void cbx_hexSend_CheckedChanged(object sender, EventArgs e)
        {
            if (cbx_code.Tag is Encoding code)
            {
                tbx_send.ReadOnly = cbx_hexSend.Checked;
                if (cbx_hexSend.Checked)
                {
                    tbx_send.Tag = tbx_send.Text;
                    //判断是否是16进制字符串
                    var tmp = tbx_send.Text.Replace(" ", "");
                    if (Regex.IsMatch(tmp, "^[0-9A-Fa-f]+$"))
                    {
                        var sb = new StringBuilder();
                        for (int i = 0; i < tmp.Length; i += 2)
                        {
                            sb.Append(tmp.Substring(i, 2));
                            sb.Append(" ");
                        }
                        tbx_send.Text = sb.ToString().TrimEnd();
                        OnMsg("当前为16进制字符，未转换，已格式化内容");
                        return;
                    }
                    tbx_send.Text = code.GetBytes(tbx_send.Text).ToHex(" ");
                    OnMsg("当前非16进制字符 ，已转换，已格式化内容");
                }
                else
                {
                    tbx_send.Text = tbx_send.Tag.ToString();// tbx_send.Text.Split(" ").Select(hex => (byte)Convert.ToInt32(hex, 16)).ToArray().ToStr(code);
                }
            }
        }


    }
}
