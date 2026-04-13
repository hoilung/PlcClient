using HL.OpcUa;
using NewLife.Reflection;
using Newtonsoft.Json;
using Opc.Ua;
using Opc.Ua.Client;
using PlcClient.Handler;
using PlcClient.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Input;

namespace PlcClient.Controls
{
    public partial class OpcUa : BaseControl
    {
        private readonly OpcUaDriver OpcUaDriver;
        private ListViewHandler<OpcUaVM> lvwHandler;
        public OpcUa()
        {
            InitializeComponent();
            OpcUaDriver = new OpcUaDriver();
            cbx_verfly.SelectedIndex = 0;
            tbx_ip.Text =tbx_ip.Text.Replace("127.0.0.1",GetLocalIP());
            lv_data.Columns.Clear();
            lvwHandler = new ListViewHandler<OpcUaVM>(lv_data);
            lvwHandler.SetupVirtualMode();            
            //lvwHandler.ColuminSort();
            ChangeState(false);
        }

        private void ChangeState(bool state)
        {
            btn_open.Enabled = !state;
            btn_sub.Enabled = btn_close.Enabled = btn_view.Enabled = state;
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
        private Dictionary<string, int> _cacheData = new Dictionary<string, int>();
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

                //var find = lv_data.FindItemWithText(node.NodeId.ToString(), true, 0);
                //if (find != null)
                //{
                //    find.SubItems[2].Text = node.Value.ToString();
                //    return;
                //}
                if (_cacheData.TryGetValue(node.NodeId.ToString(), out var index) && index < lvwHandler.ItemCount)
                {
                    lvwHandler[index].Value = node.Value.ToString();
                    return;
                }

            }


            //lv_data.BeginUpdate();
            //var lvItem = new ListViewItem(lv_data.Items.Count.ToString());
            //lvItem.Tag = node;
            //if (lv_data.Items.Count % 2 == 0)
            //{
            //    lvItem.BackColor = Color.AliceBlue;
            //}
            //lvItem.SubItems[0].Tag = lv_data.Items.Count;
            //lvItem.SubItems.Add(node.DisplayName?.ToString());
            //lvItem.SubItems.Add(node.Value.ToString());
            //lvItem.SubItems.Add(node.NodeId.ToString());
            //lvItem.SubItems.Add(_cacheType[node.DataType]);
            //if (node.Value.Value is Opc.Ua.DataValue dataval)
            //{
            //    lvItem.SubItems.Add(dataval.StatusCode.ToString());
            //    lvItem.SubItems.Add(dataval.ServerTimestamp.ToString());
            //}
            //lvItem.SubItems.Add(_cacheAccessLevel[node.AccessLevel]);
            //lvItem.SubItems.Add(node.Description?.ToString());            

            //lv_data.Items.Add(lvItem);
            //lv_data.EndUpdate();
            var vm = new OpcUaVM();
            vm.ID = lv_data.Items.Count;
            vm.DisplayName = node.DisplayName?.ToString();
            vm.Value = node.Value.ToString();
            vm.NodeId = node.NodeId.ToString();
            vm.DataType = _cacheType[node.DataType];
            if (node.Value.Value is Opc.Ua.DataValue dataval)
            {
                vm.StatusCode = dataval.StatusCode.ToString();
                vm.ServerTimestamp = dataval.ServerTimestamp.ToString();
            }
            vm.AccessLevel = _cacheAccessLevel[node.AccessLevel];
            vm.Description = node.Description?.ToString();
            lvwHandler.Add(vm);
            _cacheData[vm.NodeId] = vm.ID;

        }

        private void btn_open_Click(object sender, EventArgs e)
        {
            var opcAddress = tbx_ip.Text.Trim(); //$"opc.tcp://{ip}:{port}";
            try
            {
                var url = new Uri(opcAddress);
                var ip = url.Host;
                var port = url.Port.ToString();

                if (!Regex.IsMatch(ip, AppConfig.IPVerdify) || !Regex.IsMatch(port, AppConfig.NumVerdify))
                {
                    MessageBox.Show("无效的IP地址或端口", "提示");
                    return;
                }
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
                this.dic_subscriptions.Clear();
            }
        }

        private void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lv_data.Items.Count == 0)
            {
                MessageBox.Show("请先浏览节点", "提示");
                return;
            }
            ;
            if (!OpcUaDriver.Session.Connected)
            {
                MessageBox.Show("连接已经断开，请重新连接", "提示");
                return;
            }
            try
            {
                stopwatch.Restart();
                var dic = OpcUaDriver.ReadNodes(lvwHandler.Data.Select(m => m.NodeId).ToArray());
                stopwatch.Stop();
                OnMsg($"刷新节点数量：{lvwHandler.DataCount} 个，耗时：{stopwatch.ElapsedMilliseconds} ms");

                if (lvwHandler.DataCount != dic.Length)
                    return;
                for (int i = 0; i < lvwHandler.DataCount; i++)
                {
                    var nodeid = lvwHandler[i].NodeId;
                    var val = dic[i];
                    if (this._cacheData.TryGetValue(nodeid, out var index) && index < lvwHandler.DataCount)
                    {
                        this.lvwHandler[index].Value = val.ToString();
                        this.lvwHandler[index].StatusCode = val.StatusCode.ToString();
                        this.lvwHandler[index].ServerTimestamp = val.ServerTimestamp.ToString();
                    }
                }

                lv_data.Invalidate();
              
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
            lvwHandler.Clear();
            _cacheData.Clear();
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

        private Dictionary<string, Subscription> dic_subscriptions = new Dictionary<string, Subscription>();

        private void sub(Session m_session, string key, string[] tags, Action<string, MonitoredItem, IEncodeable> callback)
        {
            Subscription m_subscription = new Subscription(m_session.DefaultSubscription);

            m_subscription.PublishingEnabled = true;
            m_subscription.PublishingInterval = 250;
            m_subscription.KeepAliveCount = uint.MaxValue;
            m_subscription.LifetimeCount = uint.MaxValue;
            m_subscription.MaxNotificationsPerPublish = uint.MaxValue;
            m_subscription.Priority = 255;
            m_subscription.DisplayName = key;


            for (int i = 0; i < tags.Length; i++)
            {
                var item = new MonitoredItem
                {
                    StartNodeId = new NodeId(tags[i]),
                    AttributeId = Attributes.Value,
                    DisplayName = tags[i],
                    SamplingInterval = 100,
                };
                item.Notification += (MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs args) =>
                {
                    callback?.Invoke(key, monitoredItem, args.NotificationValue);
                };
                m_subscription.AddItem(item);
            }

            m_session.AddSubscription(m_subscription);
            m_subscription.Create();

            lock (dic_subscriptions)
            {
                if (dic_subscriptions.ContainsKey(key))
                {
                    // remove
                    dic_subscriptions[key].Delete(true);
                    m_session.RemoveSubscription(dic_subscriptions[key]);
                    dic_subscriptions[key].Dispose();
                    dic_subscriptions[key] = m_subscription;
                }
                else
                {
                    dic_subscriptions.Add(key, m_subscription);
                }
            }
        }

        private void btn_sub_Click(object sender, EventArgs e)
        {
            if (!OpcUaDriver.Session.Connected)
            {
                MessageBox.Show("请先连接OPCUA服务器", "提示");
            }
            if (dic_subscriptions.Count > 0)
            {
                foreach (var dic in dic_subscriptions)
                {
                    OpcUaDriver.Session.RemoveSubscription(dic.Value);
                }
                btn_sub.Text = "订阅数据";
                dic_subscriptions.Clear();
                return;
            }
            btn_sub.Text = "取消订阅";

            var lst = lvwHandler.Data.Select(m => m.NodeId).ToArray();
            this.sub(OpcUaDriver.Session, "default", lst, (key, monitoredItem, notificationValue) =>
            {
                if (notificationValue is Opc.Ua.MonitoredItemNotification monitoredItemNotification)
                {
                    var val = monitoredItemNotification.Value;
                    var nodeid = monitoredItem.DisplayName;
                    this.lv_data.Invoke(() =>
                    {
                        if (this._cacheData.TryGetValue(nodeid, out var index) && index < lvwHandler.DataCount)
                        {
                            this.lvwHandler[index].Value = val.ToString();
                            this.lvwHandler[index].StatusCode = val.StatusCode.ToString();
                            this.lvwHandler[index].ServerTimestamp = val.ServerTimestamp.ToString();
                        }
                        this.lv_data.Invalidate();

                    });
                }

            });

        }
    }
}
