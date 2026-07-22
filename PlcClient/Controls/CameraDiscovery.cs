using HL.Object.Extensions;
using NewLife.Log;
using PlcClient.Handler;
using PlcClient.Model.DeviceDiscover;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PlcClient.Controls
{
    public partial class CameraDiscovery : BaseControl
    {
        private Dictionary<string, string> hk_device_type = new Dictionary<string, string>();
        public CameraDiscovery()
        {
            InitializeComponent();
            groupBox1.Dock = lv_data.Dock = tableLayoutPanel1.Dock = DockStyle.Fill;
            cbx_deviceType.SelectedIndex = 0;
            this.tbx_ip.Items.AddRange(GetLocalAllIP());
            tbx_ip.SelectedIndex = 0;

            //创建列
            var attr = Model.DeviceDiscover.HKProbeMatch.GetDisplayCustoms().Where(m => m.Order > 0).OrderBy(m => m.Order).ToArray();
            for (int i = 0; i < attr.Length; i++)
            {
                lv_data.Columns.Add(attr[i].DataMember, attr[i].Name, 80);
            }
            listViewHandler = new Handler.ListViewHandler(this.lv_data);
            listViewHandler.ColuminSort();
            this.lv_data.MouseClick += Lv_data_MouseClick;
            hk_device_type = Properties.Resources.hk_device_type.Split(new[] { Environment.NewLine }, options: StringSplitOptions.RemoveEmptyEntries).Select(m => m.Split(',')).Where(m => m.Length == 4).Skip(1).ToDictionary(m => m[1], m => m[2]);
        }

        private void Lv_data_MouseClick(object sender, MouseEventArgs e)
        {
            if (lv_data.FullRowSelect && e.Button == MouseButtons.Right)
                contextMenuStrip1.Show(lv_data, e.X, e.Y);
        }

        Handler.ListViewHandler listViewHandler;//扩展排序和导出
        Handler.DeviceHandler deviceHandler = new DeviceHandler();//设备搜索
        private Dictionary<string, HKProbeMatch> hKProbeMatches = new Dictionary<string, HKProbeMatch>();

        private void btn_find_Click(object sender, EventArgs e)
        {
            if (deviceHandler.IsStart)
            {
                deviceHandler.Stop();
                btn_find.Text = "开始搜索";
                return;
            }
            btn_find.Text = "停止搜索";
            try
            {
                switch (cbx_deviceType.Text)
                {
                    case "海康":
                        deviceHandler.CameraProtocol = CameraProtocol.HK;
                        break;
                    case "大华":
                        deviceHandler.CameraProtocol = CameraProtocol.DH;
                        break;
                    case "ONVIF":
                        deviceHandler.CameraProtocol = CameraProtocol.ONVIF;
                        break;
                }
                deviceHandler.DeviceReceice += DeviceHandler_DeviceReceice;
                deviceHandler.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("请勿开启同类型软件避免端口占用\r\n" + ex.Message, "查找设备错误");
            }
        }
        private void DeviceHandler_DeviceReceice(object sender, DeviceEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(e.Message))
                    return;
                if (deviceHandler.CameraProtocol == CameraProtocol.HK)
                {
                    if (!e.Message.Contains("</ProbeMatch>"))
                        return;
                    var hk = deviceHandler.HKUnpack(e.Message);
                    AddDeviceData(hk);
                }
                if (deviceHandler.CameraProtocol == CameraProtocol.DH)
                {
                    var dh = deviceHandler.DaHuaUnpack(e.Message);
                    AddDeviceData(dh);
                }
                if (deviceHandler.CameraProtocol == CameraProtocol.ONVIF)
                {
                    var onvif = deviceHandler.OnvifUnpack(e.Message);
                    AddDeviceData(onvif);
                }

            }
            catch (Exception ex)
            {
                XTrace.Log.Error("查找摄像头设备错误 {0},{1}", ex,e.Message);
                OnMsg("查找摄像头设备错误," + ex.Message);
            }
        }
        private void AddDeviceData(HKProbeMatch hk)
        {
            if (hk == null)
            {
                return;
            }
            if (hKProbeMatches.ContainsKey(hk.MAC))
            {
                return;
            }
            var info = hk_device_type.Keys.FirstOrDefault(m => hk.DeviceDescription.StartsWith(m));
            if (info != null)
            {
                hk.DeviceType = $"{hk_device_type[info]}";
            }
            hk.DeviceSN = hk.DeviceSN.Length > 9 ? hk.DeviceSN.Substring(hk.DeviceSN.Length - 9) : hk.DeviceSN;

            hKProbeMatches.Add(hk.MAC, hk);
            lv_data.Invoke(() =>
            {
                var row = lv_data.Items.Add(lv_data.Items.Count.ToString());
                row.Tag = hk;
                if (lv_data.Items.Count % 2 == 0)
                    row.BackColor = Color.AliceBlue;
                var item = hk.GetObjectMap();
                for (int j = 1; j < lv_data.Columns.Count; j++)
                {

                    if (item.TryGetValue(lv_data.Columns[j].Name, out var value))
                    {
                        value = value ?? "";
                        row.SubItems.Add(value.ToString());
                        if (lv_data.Columns[j].Name == nameof(HKProbeMatch.IPv4Address))
                        {
                            row.SubItems[j].Tag = listViewHandler.IPv4ToLong(value.ToString());
                        }
                    }
                }
                row.SubItems[0].Tag = lv_data.Items.Count;
            });
        }

        private void btn_exportExcel_Click(object sender, EventArgs e)
        {
            var filename = this.listViewHandler.ExportExcel("监控设备");
            if (string.IsNullOrEmpty(filename)) { return; }
            var msg = $"保存文件 {filename}";
            OnMsg(msg);
            MessageBox.Show(filename, "保存文件");
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            lv_data.Items.Clear();
            hKProbeMatches.Clear();
        }

        private void openWebBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var hk = lv_data.SelectedItems[0].Tag as HKProbeMatch;
                if (hk == null) { return; }
                var url = string.Format("http://{0}:{1}", hk.IPv4Address, hk.HttpPort);
                if (string.IsNullOrEmpty(url)) { return; }
                System.Diagnostics.Process.Start(url);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                MessageBox.Show(ex.Message, "打开网页错误");
            }
        }

        private void copyRTSPaddressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var hk = lv_data.SelectedItems[0].Tag as HKProbeMatch;
                if (hk == null) { return; }
                var text = PlcClient.Properties.Resources.RSTP_TPL.Replace("IP", hk.IPv4Address);
                if (string.IsNullOrEmpty(text)) { return; }
                Clipboard.SetText(text);
                var msg = $"RTSP参考地址已复制到剪贴板";
                OnMsg(msg);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                MessageBox.Show(ex.Message, "复制RTSP地址错误");
            }
        }

        private void showDeviceNameToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {

                var list = new List<string>();
                foreach (ListViewItem item in lv_data.SelectedItems)
                {
                    if (item.Tag is HKProbeMatch hk && hk.DeviceType != string.Empty)
                    {
                        list.Add(hk.IPv4Address);
                    }
                }
                if (!list.Any())
                {
                    return;
                }

                var frm_about = new Form();
                frm_about.StartPosition = FormStartPosition.CenterParent;
                frm_about.Text = "查看海康设备信息";
                frm_about.ShowIcon = false;
                frm_about.Size = new Size(650, 350);
                //frm_about.MaximizeBox = false;
                frm_about.MinimizeBox = false;
                frm_about.FormBorderStyle = FormBorderStyle.FixedSingle;

                var cameraDeviceInfo = new CameraDeviceInfo();
                cameraDeviceInfo.Dock = DockStyle.Fill;
                cameraDeviceInfo.LoadData(list);
                frm_about.Controls.Add(cameraDeviceInfo);
                frm_about.ShowDialog(this);

            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                MessageBox.Show(ex.Message, "查看设备信息错误");
            }
        }
    }
}
