using HL.OpcUa;
using Microsoft.VisualBasic.ApplicationServices;
using NewLife.Reflection;
using Opc.Ua;
using Opc.Ua.Client;
using PlcClient.Handler;
using PlcClient.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace PlcClient.Controls
{
    public partial class OpcUa : BaseControl
    {
        private UaClient OpcUaDriver;
        private ListViewHandler<OpcUaVM> lvwHandler;
        public OpcUa()
        {
            InitializeComponent();
            pl_cert.Location = new System.Drawing.Point(6, 43);
            cbx_verfly.SelectedIndex = 0;
            tbx_ip.Text = tbx_ip.Text.Replace("127.0.0.1", GetLocalIP());
            lv_data.Columns.Clear();
            lvwHandler = new ListViewHandler<OpcUaVM>(lv_data);
            lvwHandler.SetupVirtualMode();
            //lvwHandler.ColuminSort();
            ChangeState(false);
        }

        private void ChangeState(bool state)
        {
            btn_open.Enabled = !state;
            btn_read.Enabled = btn_sub.Enabled = btn_close.Enabled = btn_view.Enabled = state;
        }

        private void cbx_verfly_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbx_user.Enabled = tbx_pass.Enabled = cbx_verfly.SelectedIndex != 0;
            pl_cert.Visible = cbx_verfly.SelectedIndex == 2;
            if (pl_cert.Visible)
            {
                tbx_cert_path_MouseClick(null, null);
            }
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

        private Dictionary<uint, string> _cacheType = null;
        private Dictionary<byte, string> _cacheAccessLevel = null;
        private Dictionary<string, int> _cacheData = new Dictionary<string, int>();
        private void BrowseView_DataRefresh(VariableNode node, string tag)
        {

            if (_cacheType == null)
            {
                var fs = typeof(Opc.Ua.DataTypes).GetFields(); //DataTypes  DataTypeIds
                _cacheType = fs.ToDictionary(m => (uint)m.GetValue(null), m => m.Name);
            }
            if (_cacheAccessLevel == null)
            {
                var al = typeof(AccessLevels).GetFields();
                _cacheAccessLevel = al.ToDictionary(m => (byte)m.GetValue(null), m => m.Name);
            }


            if (lv_data.Items.Count > 0)
            {
                if (_cacheData.TryGetValue(node.NodeId.ToString(), out var index) && index < lvwHandler.ItemCount)
                {
                    lvwHandler[index].Value = node.Value.ToString();
                    return;
                }

            }

            var vm = new OpcUaVM();
            vm.ID = lv_data.Items.Count;
            vm.Tag = tag;
            vm.DisplayName = node.DisplayName?.ToString();
            vm.Value = node.Value.ToString();
            vm.NodeId = node.NodeId.ToString();
            if (_cacheType.ContainsKey((uint)node.DataType.Identifier))
            {
                vm.DataType = _cacheType[(uint)node.DataType.Identifier];
            }
            else
            {
                vm.DataType = node.DataType.Identifier.ToString();
            }
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

                OpcUaDriver = new UaClient(url);                
                OpcUaDriver.Options.DefaultMonitorInterval = 250;
                OpcUaDriver.Options.SubscriptionKeepAliveCount = 10;
                OpcUaDriver.Options.SubscriptionLifetimeCount = 1000;
                if (cbx_verfly.SelectedIndex == 1)
                {
                    var user = tbx_user.Text.Trim();
                    var pass = tbx_pass.Text.Trim();
                    if (string.IsNullOrEmpty(user))
                    {
                        MessageBox.Show("请输入有效的OPCUA用户或密码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    OpcUaDriver.Options.UserIdentity = new UserIdentity(user, pass);
                }
                else if (cbx_verfly.SelectedIndex == 2)
                {
                    string certPath = tbx_cert_path.Text.Trim();
                    if (string.IsNullOrEmpty(certPath) || !File.Exists(certPath))
                    {
                        MessageBox.Show("非有效的签名证书文件", "无效的签名证书", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }                    
                    OpcUaDriver.Options.ApplicationCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(certPath);
                }

                OpcUaDriver.Open();

                if (OpcUaDriver.Session.Connected)
                {
                    OnMsg("连接成功 " + opcAddress);
                    ChangeState(true);
                    return;
                }
                ChangeState(false);
            }
            catch (Exception ex)
            {
                OnMsg("连接失败 " + opcAddress);
                MessageBox.Show(ex.Message+"\r\n"+ex.InnerException.Message, "连接失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btn_close_Click(object sender, EventArgs e)
        {

            OpcUaDriver.Dispose();
            ChangeState(false);

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
                var dic = OpcUaDriver.ReadNodes(lvwHandler.Data.Where(m => !string.IsNullOrEmpty(m.Tag)).Select(m => m.NodeId).ToArray());
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
            if (lv_data.SelectedIndices.Count > 0 && lvwHandler.DataCount > 0)
            {
                var index = lv_data.SelectedIndices[0];
                tbx_tag.Text = lvwHandler[index].Tag;
                tbx_tag.Tag = lvwHandler[index];
            }

        }
        public void Monitor<T>(Session session, string[] tags, Action<string, ReadEvent<T>, Action> callback)
        {

            Subscription sub = new Subscription
            {
                PublishingInterval = OpcUaDriver.Options.DefaultMonitorInterval,
                PublishingEnabled = true,
                LifetimeCount = OpcUaDriver.Options.SubscriptionLifetimeCount,
                KeepAliveCount = OpcUaDriver.Options.SubscriptionKeepAliveCount,
                DisplayName = "default",
                Priority = byte.MaxValue
            };
            foreach (var tag in tags)
            {
                UaNode uaNode = OpcUaDriver.FindNode(tag);
                MonitoredItem monitoredItem = new MonitoredItem
                {
                    StartNodeId = uaNode.NodeId,
                    AttributeId = 13u,
                    DisplayName = tag,
                    SamplingInterval = OpcUaDriver.Options.DefaultMonitorInterval
                };
                sub.AddItem(monitoredItem);
                monitoredItem.Notification += delegate (MonitoredItem s, MonitoredItemNotificationEventArgs e)
                {
                    MonitoredItemNotification monitoredItemNotification = (MonitoredItemNotification)e.NotificationValue;
                    object value = monitoredItemNotification.Value.WrappedValue.Value;
                    Action arg = delegate
                    {
                        sub.RemoveItems(sub.MonitoredItems);
                        sub.Delete(silent: true);
                        session.RemoveSubscription(sub);
                        sub.Dispose();
                    };
                    ReadEvent<T> readEvent = new ReadEvent<T>
                    {
                        Value = (T)value,
                        SourceTimestamp = monitoredItemNotification.Value.SourceTimestamp,
                        ServerTimestamp = monitoredItemNotification.Value.ServerTimestamp
                    };
                    if (StatusCode.IsGood(monitoredItemNotification.Value.StatusCode))
                    {
                        readEvent.Quality = Quality.Good;
                    }

                    if (StatusCode.IsBad(monitoredItemNotification.Value.StatusCode))
                    {
                        readEvent.Quality = Quality.Bad;
                    }

                    callback(uaNode.NodeId, readEvent, arg);
                };
            }
            session.AddSubscription(sub);
            sub.Create();
            sub.ApplyChanges();

        }

        private CancellationTokenSource _cancelTokenSourceSub = new CancellationTokenSource();
        private void btn_sub_Click(object sender, EventArgs e)
        {
            if (OpcUaDriver.Status == OpcStatus.NotConnected)
            {
                MessageBox.Show("请先连接OPCUA服务器", "提示");
                return;
            }
            if (btn_sub.Text == "取消订阅")
            {
                _cancelTokenSourceSub.Cancel();
                return;
            }
            _cancelTokenSourceSub = new CancellationTokenSource();
            _cancelTokenSourceSub.Token.Register(() =>
            {
                btn_sub.Text = "订阅数据";
            });

            btn_sub.Text = "取消订阅";

            var lst = lvwHandler.Data.Where(m => !string.IsNullOrEmpty(m.Tag)).Select(m => m.Tag).ToArray();

            this.Monitor<object>(OpcUaDriver.Session, lst, (nodeId, result, unsub) =>
            {
                if (_cancelTokenSourceSub.IsCancellationRequested)
                {
                    unsub.Invoke();
                    return;
                }
                this.lv_data.Invoke(() =>
                {
                    if (this._cacheData.TryGetValue(nodeId, out var index) && index < lvwHandler.DataCount)
                    {
                        this.lvwHandler[index].Value = result.Value?.ToString();
                        this.lvwHandler[index].StatusCode = result.Quality.ToString();
                        this.lvwHandler[index].ServerTimestamp = result.ServerTimestamp.ToString();
                    }
                    this.lv_data.Invalidate();
                });
            });
        }

        private void tbx_cert_path_MouseClick(object sender, MouseEventArgs e)
        {
            //打开文件选择器
            var openDiaglog = new OpenFileDialog();
            openDiaglog.Filter = "证书文件(*.der;*.cer)|*.der;*.cer";
            openDiaglog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openDiaglog.Title = "选择证书文件";
            openDiaglog.RestoreDirectory = true;
            openDiaglog.CheckFileExists = true;
            openDiaglog.CheckPathExists = true;
            openDiaglog.Multiselect = false;
            tbx_cert_path.ResetText();
            if (openDiaglog.ShowDialog() == DialogResult.OK)
            {
                tbx_cert_path.Text = openDiaglog.FileName;
            }

        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvwHandler.ItemCount > 0)
            {
                var one = lvwHandler.Items.Where(m => m.Selected).FirstOrDefault();
                if (one != null && one.Tag != null)
                {
                    Clipboard.SetText(one.Tag.ToString());
                    this.OnMsg("复制成功");
                }
            }
        }

        private void btn_read_Click(object sender, EventArgs e)
        {
            var tag = tbx_tag.Text.Trim();
            if (string.IsNullOrEmpty(tag))
            {
                return;
            }
            if (this.OpcUaDriver != null && this.OpcUaDriver.Status == OpcStatus.Connected)
            {
                try
                {
                    var sw = new Stopwatch();
                    var nodeid = this.OpcUaDriver.FindNode(tag);
                    sw.Start();
                    var nodeval = this.OpcUaDriver.ReadNode(nodeid.NodeId);                    
                    sw.Stop();
                    if (nodeval != null)
                    {
                        tbx_val.Text = nodeval.Value.ToString();
                    }
                    this.OnMsg($"OPC UA Tag:{nodeid.Tag} Value: {tbx_val.Text} StatusCode: {nodeval.StatusCode} 耗时: {sw.ElapsedMilliseconds}ms");
                    

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "读取节点失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
