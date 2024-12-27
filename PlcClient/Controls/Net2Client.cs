using NewLife;
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

namespace PlcClient.Controls
{
    public partial class Net2Client : BaseControl
    {
        private ISocketClient _client;

        public Net2Client()
        {
            InitializeComponent();
            tableLayoutPanel1.Dock = DockStyle.Fill;

            this.Disposed += NetClient_Disposed;

            this.cbx_code.Tag = Encoding.Default;
            this.cbx_code.SelectedIndex = 0;
            this.cbx_code.SelectedIndexChanged += Cbx_code_SelectedIndexChanged;
            this.Dock = DockStyle.Fill;
            cbx_mode.SelectedIndex = 0;
            cbx_remoteIp.Items.AddRange(GetLocalAllIP());
            cbx_localip.Items.AddRange(NetHelper.GetIPs().Where(m => m.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).Select(m => m.ToString()).ToArray());
            cbx_remoteIp.SelectedIndex = 0;
            cbx_localip.SelectedIndex = 0;

            this.btn_close.Enabled = false;
        }

        private void Cbx_code_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbx_code.Text.ToLower() == "default")
                {
                    cbx_code.Tag = Encoding.Default;
                    return;
                }
                cbx_code.Tag = Encoding.GetEncoding(cbx_code.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("获取指定编码错误，采用系统默认编码", "编码设置错误");
            }
        }

        private void NetClient_Disposed(object sender, EventArgs e)
        {
            btn_close_Click(null, null);
        }

        private void btn_conn_Click(object sender, EventArgs e)
        {
            var address = $"{cbx_mode.Text}://{cbx_remoteIp.Text}:{tbx_remotePort.Text}";
            try
            {
                NetUri net = new NetUri(address);
                _client = net.CreateRemote();
                //_client.Log = null;
                _client.Received += _client_Received;
                _client.Opened += _client_Opened;
                _client.Closed += _client_Closed;
                _client.Open();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "连接失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void _client_Closed(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                btn_conn.Enabled = true;
                btn_close.Enabled = false;
            }));
            OnMsg($"连接断开 本地：{_client.Local}=>远程：{_client.Remote}");
        }

        private void _client_Opened(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                btn_conn.Enabled = false;
                btn_close.Enabled = true;
            }));
            OnMsg($"连接成功 本地：{_client.Local}=>远程：{_client.Remote}");
        }
        private StringBuilder sb = new StringBuilder();


        private void _client_Received(object sender, ReceivedEventArgs e)
        {
            sb.Clear();
            tbx_received.Invoke(new Action(() =>
            {
                if (cbx_time.Checked)
                {
                    tbx_received.AppendText(DateTime.Now.ToString("[HH:mm:ss.fff] ") + e.Remote + Environment.NewLine);
                }
                int start = tbx_received.Text.Length;
                if (cbx_string.Checked)
                {
                    sb.AppendLine(e.Packet.ToStr(cbx_code.Tag as Encoding));
                }
                if (cbx_hex.Checked)
                {
                    sb.AppendLine(e.Packet.ToHex(-1, " "));
                }
                if (sb.Length > 0)
                {
                    tbx_received.AppendText(sb.ToString());
                    tbx_received.SelectionStart = start;
                    tbx_received.SelectionLength = tbx_received.Text.Length;
                    var rgb = Enumerable.Range(1, 254).OrderBy(m => Guid.NewGuid()).Take(3).ToArray();
                    tbx_received.SelectionColor = Color.FromArgb(0, rgb[1], rgb[2]);
                    tbx_received.Scroll();
                }
            }));
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            if (_client == null || !_client.Active)
                return;
            var nd_num = this.nd_num.Value;
            var step = this.nd_step.Value;
            var text = tbx_send.Text.Trim();
            var code = cbx_code.Tag as Encoding;
            byte[] data = null;
            if (cbx_hexSend.Checked)
            {   //hex string to byte                    
                data = text.Split(' ').Select(hex => (byte)Convert.ToInt32(hex, 16)).ToArray();
            }
            else
            {
                data = code.GetBytes(text);
            }
            if (data == null || data.Length == 0)
            {
                MessageBox.Show("请输入要发送的数据", "提示");
            }
            Task.Run(async () =>
            {
                for (int i = 0; i < nd_num; i++)
                {
                    var len = _client.Send(data);
                    OnMsg($"{DateTime.Now.ToString("[HH:mm:ss.fff]")} 发送数据：{_client.Remote} 长度:{len}");
                    await Task.Delay(step.ToInt());
                }
            });

        }

        private void btn_clearSend_Click(object sender, EventArgs e)
        {
            tbx_send.ResetText();
        }

        private void btn_clearCallback_Click(object sender, EventArgs e)
        {
            tbx_received.ResetText();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            if (_client != null && _client.Active)
                _client.Close("");
        }

        private void tbx_send_TextChanged(object sender, EventArgs e)
        {
            if (!cbx_hexSend.Checked)
            {
                return;
            }
            if (tbx_send.TextLength % 3 == 2)
            {
                tbx_send.Append(" ");
            }
        }

        private void tbx_send_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!cbx_hexSend.Checked)
            {
                return;
            }
            if (!char.IsControl(e.KeyChar) && !Regex.IsMatch(e.KeyChar.ToString(), "[0-9A-Fa-f]"))
            {
                e.Handled = true; // 拒绝输入
            }
        }

        private void cbx_hexSend_CheckedChanged(object sender, EventArgs e)
        {
            if (cbx_code.Tag is Encoding code)
            {
                if (cbx_hexSend.Checked)
                {
                    tbx_send.Text = code.GetBytes(tbx_send.Text).ToHex(" ");
                }
                else
                {
                    tbx_send.Text = tbx_send.Text.Split(" ").Select(hex => (byte)Convert.ToInt32(hex, 16)).ToArray().ToStr(code);
                }
            }
        }
    }
}
