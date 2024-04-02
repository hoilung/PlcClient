using HL.OpcUa;
using Opc.Ua;
using PlcClient.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlcClient.Controls
{
    public partial class OpcUa : BaseControl
    {
        private readonly OpcUaDriver OpcUaDriver;
        public OpcUa()
        {
            InitializeComponent();
            OpcUaDriver = new OpcUaDriver();
            cbx_verfly.SelectedIndex = 0;
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
            lvItem.SubItems.Add(node.NodeId.ToString());
            lvItem.SubItems.Add(node.Value.ToString());
            lvItem.SubItems.Add(_cacheType[node.DataType]);
            lvItem.SubItems.Add(_cacheAccessLevel[node.AccessLevel]);
            lvItem.SubItems.Add(node.Description.ToString());
            lv_data.Items.Add(lvItem);
            lv_data.EndUpdate();
        }

        private void btn_open_Click(object sender, EventArgs e)
        {
            var ip = tbx_ip.Text.Trim();
            var port = tbx_port.Text.Trim();
            if (!Regex.IsMatch(ip, Config.IPVerdify) || !Regex.IsMatch(port, Config.NumVerdify))
            {
                MessageBox.Show("无效的IP地址或端口", "提示");
                return;
            }
            var opcAddress = $"opc.tcp://{ip}:{port}";
            try
            {
                OpcUaDriver.Open(opcAddress);
                OnMsg("连接成功 " + opcAddress);
                ChangeState(true);
            }
            catch (Exception ex)
            {
                OnMsg("连接失败 " + opcAddress);
                MessageBox.Show(ex.Message, "连接失败");
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
    }
}
