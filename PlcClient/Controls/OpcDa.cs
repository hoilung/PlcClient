using HL.OpcDa;
using Opc;
using PlcClient.Handler;
using PlcClient.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PlcClient.Controls
{
    public partial class OpcDa : BaseControl
    {
        private OpcCom.ServerEnumerator m_discovery = new OpcCom.ServerEnumerator();

        private OpcDaDriver Opc;
        private ListViewHandler lvwHandler;
        public OpcDa()
        {
            InitializeComponent();
            tbx_ip.Text = GetLocalIP();
            lvwHandler = new ListViewHandler(lv_data);
            lvwHandler.ColuminSort();
            this.DoubleBuffered = true;
            Opc = new OpcDaDriver();
            ChangeState(false);
        }


        private void btn_open_Click(object sender, EventArgs e)
        {
            var ip = tbx_ip.Text.Trim();
            var name = cbx_servername.Text.Trim();
            if (!Regex.IsMatch(ip, Config.IPVerdify) || string.IsNullOrEmpty(name))
            {
                MessageBox.Show("非有效的IP地址或服务名称", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var result = ping.Send(ip, 500);
            if (result.Status != IPStatus.Success)
            {
                if (MessageBox.Show($"{ip}\r\n网络PING疑似不通,是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
            }
            string opcAddress = $"opcda://{ip}/{name}";
            try
            {
                Opc.Open(opcAddress);
                ChangeState(Opc.Server.IsConnected);
                OnMsg($"OpcDa连接{(Opc.Server.IsConnected ? "成功" : "失败")} {opcAddress}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "连接服务器失败");
            }
        }

        private void ChangeState(bool state)
        {
            this.btn_BrowseView.Enabled = btn_close.Enabled = state;
            this.btn_open.Enabled = !state;
            this.btn_read.Enabled = this.btn_sub.Enabled = state;
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            if (btn_sub.Text == "取消订阅")
            {
                btn_sub_Click(sender, e);
            }
            Opc.Close();
            ChangeState(false);
            OnMsg("OpcDa关闭连接");
        }
        private void btn_getname_Click(object sender, EventArgs e)
        {
            try
            {
                var ip = tbx_ip.Text.Trim();
                if (!Regex.IsMatch(ip, Config.IPVerdify))
                {
                    MessageBox.Show("非有效的IP地址", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var result = ping.Send(ip, 500);
                if (result.Status != IPStatus.Success)
                {
                    if (MessageBox.Show($"{ip}\r\n网络PING疑似不通,是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }
                var server = m_discovery.GetAvailableServers(Specification.COM_DA_20, ip, new ConnectData(new System.Net.NetworkCredential()));
                cbx_servername.Items.Clear();
                if (server != null)
                {
                    for (int i = 0; i < server.Length; i++)
                    {
                        cbx_servername.Items.Add(server[i].Name.Replace(ip + ".", string.Empty));
                    }
                    cbx_servername.SelectedIndex = 0;
                    OnMsg($"OpcDa 2.0 服务器名称获取成功,{ip} 数量 {server.Length} 个");
                }
                else
                {
                    OnMsg($"OpcDa 2.0 服务器名称获取失败,{ip}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("提示：非本地服务器请尝试添加或同步windows凭证后重试\r\n" + ex.Message, "获取服务器名称失败");
            }

        }


        private void btn_BrowseView_Click(object sender, EventArgs e)
        {

            var tagForm = new Form()
            {
                Height = 450,
                Width = 600,
                Text = "OPC DA 浏览节点",
                Icon = Properties.Resources.opc,
                StartPosition = FormStartPosition.CenterParent
            };
            tagForm.MinimumSize = new System.Drawing.Size(700, 450);
            OpcDaBrowseView browseView = new OpcDaBrowseView(Opc);
            browseView.Dock = DockStyle.Fill;
            browseView.DataRefresh += BrowseView_DataRefresh;
            tagForm.Controls.Add(browseView);
            tagForm.ShowDialog();

        }

        private void BrowseView_DataRefresh(OPCDAItem opcdaItem)
        {
            if (lv_data.Items.Count > 0)
            {
                var find = lv_data.FindItemWithText(opcdaItem.Address, true, 0);
                if (find != null)
                {
                    find.SubItems[3].Text = opcdaItem.Value.ToString();
                    find.SubItems[4].Text = opcdaItem.Quality;
                    find.SubItems[5].Text = opcdaItem.Time;
                    return;
                }
            }


            lv_data.BeginUpdate();
            var subItem = new ListViewItem(lv_data.Items.Count.ToString());
            subItem.Tag = opcdaItem;
            if (lv_data.Items.Count % 2 == 0)
            {
                subItem.BackColor = Color.AliceBlue;
            }
            subItem.SubItems[0].Tag = lv_data.Items.Count;
            subItem.SubItems.Add(opcdaItem.Address);
            subItem.SubItems.Add(opcdaItem.ValueType.Name);
            subItem.SubItems.Add(opcdaItem.Value.ToString());
            subItem.SubItems.Add(opcdaItem.Quality);
            subItem.SubItems.Add(opcdaItem.Time);
            lv_data.Items.Add(subItem);
            lv_data.EndUpdate();

        }
        private void btn_read_Click(object sender, EventArgs e)
        {
            var tagName = tbx_tag.Text;
            if (string.IsNullOrEmpty(tagName))
            {
                OnMsg("请输入要读取的节点名称");
                MessageBox.Show("请输入要读取的节点名称", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            tbx_tag_value.ResetText();
            var items = Opc.Server.Read(new[] { new Opc.Da.Item { ItemName = tagName } });
            if (items != null)
            {
                tbx_tag_value.Text = items[0].Value.ToString();
                OnMsg($"OpcDa读取：{tagName} 值：{items[0].Value} 质量：{items[0].Quality} 时间：{items[0].Timestamp}");
            }
        }

        private void btn_sub_Click(object sender, EventArgs e)
        {
            try
            {
                if (btn_sub.Text == "取消订阅")
                {
                    btn_sub.Text = "开启订阅";
                    Opc.SubDataChange -= Opc_SubDataChange;
                    Opc.CancelSubscription("default");

                    OnMsg("已取消数据订阅，变更数据停止刷新");
                    return;
                }
                else
                {
                    if (lv_data.Items.Count == 0)
                    {
                        MessageBox.Show("数据列表无内容，请先添加节点，再开启订阅", "提示");
                        return;
                    }
                    btn_sub.Text = "取消订阅";
                    Opc.CreateSubscription("default", 1000);
                    Opc.SubDataChange += Opc_SubDataChange;
                    OnMsg("已开启数据订阅，变更数据自动刷新");
                    var subList = new List<PointItem>();
                    for (int i = 0; i < lv_data.Items.Count; i++)
                    {
                        if (lv_data.Items[i].Tag is PointItem item)
                        {
                            subList.Add(item);
                        }

                    }
                    Opc.AddItemSubscription("default", subList.ToArray());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "订阅操作失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Opc_SubDataChange(object sender, SubDataChangeEventArgs e)
        {
            lv_data.Invoke(new Action(() =>
            {
                try
                {
                    lv_data.BeginUpdate();
                    for (int i = 0; i < e.Results.Length; i++)
                    {
                        var item = e.Results[i];
                        var subItem = lv_data.FindItemWithText(item.ItemName, true, 0);
                        if (subItem != null)
                        {
                            subItem.SubItems[3].Text = item.Value.ToString();
                            subItem.SubItems[4].Text = item.Quality.ToString();
                            subItem.SubItems[5].Text = item.Timestamp.ToString();
                        }
                    }
                    lv_data.EndUpdate();
                }
                catch (Exception)
                {

                }

            }));

        }

        private void lv_data_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                refreshToolStripMenuItem.Enabled = true;
                if (Opc.Server == null || !Opc.Server.IsConnected)
                {
                    refreshToolStripMenuItem.Enabled = false;
                }
                MenuStrip_lv.Show(lv_data, e.X, e.Y);
                return;
            }
            tbx_tag.Text = lv_data.SelectedItems[0].SubItems[1].Text;

        }

        private void export_ExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvwHandler.ExportExcel("OpcDa");
        }


        private void clearlistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (btn_sub.Text == "取消订阅")
            {
                btn_sub_Click(sender, e);
            }
            lv_data.Items.Clear();
            OnMsg("清空列表");
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Opc.Server == null || !Opc.Server.IsConnected)
            {
                OnMsg("Opc Da 连接无效或断开");
                return;
            }

            var list = new List<Opc.Da.Item>();
            for (int i = 0; i < lv_data.Items.Count; i++)
            {
                if (lv_data.Items[i].Tag is OPCDAItem item)
                {
                    list.Add(item.ParseItem());
                }
            }
            stopwatch.Restart();
            var items = Opc.Server.Read(list.ToArray());
            stopwatch.Stop();
            Opc_SubDataChange(null, new SubDataChangeEventArgs { Results = items });
            OnMsg($"刷新列表数据 {list.Count}个，用时：{stopwatch.Elapsed.TotalMilliseconds.ToString("0.000ms")}");
        }
    }
}
