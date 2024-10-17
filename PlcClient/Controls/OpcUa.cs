using HL.OpcUa;
using Newtonsoft.Json;
using Opc.Ua;
using PlcClient.Handler;
using PlcClient.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PlcClient.Controls
{
    public partial class OpcUa : BaseControl
    {
        private readonly OpcUaDriver OpcUaDriver;
        private ListViewHandler lvwHandler;
        public OpcUa()
        {
            InitializeComponent();
            OpcUaDriver = new OpcUaDriver();
            cbx_verfly.SelectedIndex = 0;
            tbx_ip.Text = GetLocalIP();
            lvwHandler = new ListViewHandler(lv_data);
            lvwHandler.ColuminSort();
            ChangeState(false);
        }

        private void ChangeState(bool state)
        {
            btn_open.Enabled = !state;
            btn_close.Enabled = btn_view.Enabled = state;
        }

        private void cbx_verfly_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbx_user.Enabled = tbx_pass.Enabled = cbx_verfly.SelectedIndex != 0;
        }

        private void btn_view_Click(object sender, EventArgs e)
        {
            var tagForm = new Form()
            {
                Height = 450,
                Width = 600,
                Text = "OPC UA 浏览节点",
                Icon = Properties.Resources.opc,
                StartPosition = FormStartPosition.CenterParent
            };
            tagForm.MinimumSize = new System.Drawing.Size(700, 450);
            OpcUaBrowseView browseView = new OpcUaBrowseView(OpcUaDriver);
            browseView.Dock = DockStyle.Fill;
            browseView.DataRefresh += BrowseView_DataRefresh;
            tagForm.Controls.Add(browseView);
            tagForm.ShowDialog();
        }

        private Dictionary<NodeId, string> _cacheType = null;
        private Dictionary<byte, string> _cacheAccessLevel = null;
        private void BrowseView_DataRefresh(VariableNode node)
        {

            if (_cacheType == null)
            {
                var fs = typeof(DataTypeIds).GetFields();
                _cacheType = fs.ToDictionary(m => (m.GetValue(null) as NodeId), m => m.Name);
            }
            if (_cacheAccessLevel == null)
            {
                var al = typeof(AccessLevels).GetFields();
                _cacheAccessLevel = al.ToDictionary(m => (byte)m.GetValue(null), m => m.Name);
            }


            if (lv_data.Items.Count > 0)
            {
                var find = lv_data.FindItemWithText(node.NodeId.ToString(), true, 0);
                if (find != null)
                {
                    find.SubItems[2].Text = node.Value.ToString();
                    return;
                }
            }
            lv_data.BeginUpdate();
            var lvItem = new ListViewItem(lv_data.Items.Count.ToString());
            lvItem.Tag = node;
            if (lv_data.Items.Count % 2 == 0)
            {
                lvItem.BackColor = Color.AliceBlue;
            }
            lvItem.SubItems[0].Tag = lv_data.Items.Count;
            lvItem.SubItems.Add(node.DisplayName?.ToString());
            lvItem.SubItems.Add(node.Value.ToString());
            lvItem.SubItems.Add(node.NodeId.ToString());
            lvItem.SubItems.Add(_cacheType[node.DataType]);
            lvItem.SubItems.Add(_cacheAccessLevel[node.AccessLevel]);
            lvItem.SubItems.Add(node.Description?.ToString());
            lv_data.Items.Add(lvItem);
            lv_data.EndUpdate();
        }

        private void btn_open_Click(object sender, EventArgs e)
        {
            var ip = tbx_ip.Text.Trim();
            var port = tbx_port.Text.Trim();


            if (!Regex.IsMatch(ip, AppConfig.IPVerdify) || !Regex.IsMatch(port, AppConfig.NumVerdify))
            {
                MessageBox.Show("无效的IP地址或端口", "提示");
                return;
            }
            var opcAddress = $"opc.tcp://{ip}:{port}";
            try
            {
                if (cbx_verfly.SelectedIndex == 1)
                {
                    var user = tbx_user.Text.Trim();
                    var pass = tbx_pass.Text.Trim();
                    if (string.IsNullOrEmpty(user))
                    {
                        MessageBox.Show("请输入有效的OPCUA用户或密码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    OpcUaDriver.UserIdentity = new UserIdentity(user, pass);
                }

                OpcUaDriver.Open(opcAddress);
                OnMsg("连接成功 " + opcAddress);
                ChangeState(true);
            }
            catch (Exception ex)
            {
                OnMsg("连接失败 " + opcAddress);
                MessageBox.Show(ex.Message, "连接失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            if (OpcUaDriver.Session != null && OpcUaDriver.Session.Connected)
            {
                OpcUaDriver.Dispose();
                ChangeState(false);
            }
        }

        private void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lv_data.Items.Count == 0)
            {
                MessageBox.Show("请先浏览节点", "提示");
                return;
            };
            if(!OpcUaDriver.Session.Connected)
            {
                MessageBox.Show("连接已经断开，请重新连接", "提示");
                return;            
            }
            var list = new List<PointItem>();
            for (int i = 0; i < lv_data.Items.Count; i++)
            {
                if (lv_data.Items[i].Tag is VariableNode node)
                {
                    list.Add(new PointItem() { Name = lv_data.Items[i].Text, Address = node.NodeId.ToString() });
                }
            }
            try
            {
                stopwatch.Restart();
                var dic = OpcUaDriver.Read(list.ToArray());
                stopwatch.Stop();
                OnMsg($"刷新节点数量：{list.Count} 个，耗时：{stopwatch.ElapsedMilliseconds} ms");
                lv_data.BeginUpdate();
                for (int i = 0; i < lv_data.Items.Count; i++)
                {

                    var item = lv_data.Items[i];
                    if (dic.TryGetValue(item.Text, out var value))
                    {
                        if (value == null)
                        {
                            value = "null";
                        }
                        if (value.GetType().IsArray)
                        {
                            item.SubItems[2].Text = JsonConvert.SerializeObject(value);
                            continue;
                        }
                        item.SubItems[2].Text = value.ToString();
                    }

                }
                lv_data.EndUpdate();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "刷新节点值错误");
            }

        }

        private void excelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvwHandler.ExportSCV("OpcUa");
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lv_data.Items.Clear();
        }

        private void lv_data_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                RefreshToolStripMenuItem.Enabled = true;
                if (OpcUaDriver == null || !OpcUaDriver.Session.Connected)
                {
                    RefreshToolStripMenuItem.Enabled = false;
                }
                contextMenuStrip_lv.Show(lv_data, e.X, e.Y);
                return;
            }
        }
    }
}
