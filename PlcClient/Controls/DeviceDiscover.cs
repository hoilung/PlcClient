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
    public partial class DeviceDiscover : BaseControl
    {

        public DeviceDiscover()
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
                lv_data.Columns.Add(attr[i].DataMember, attr[i].Name);
            }
            listViewHandler = new Handler.ListViewHandler(this.lv_data);
            listViewHandler.ColuminSort();
            this.lv_data.MouseClick += Lv_data_MouseClick;
        }

        private void Lv_data_MouseClick(object sender, MouseEventArgs e)
        {
            if (lv_data.FullRowSelect && e.Button == MouseButtons.Right)
                contextMenuStrip1.Show(lv_data, e.X, e.Y);
        }

        Handler.ListViewHandler listViewHandler;//扩展排序和导出
        Handler.DeviceHandler deviceHandler;//设备搜索
        private Dictionary<string, HKProbeMatch> hKProbeMatches = new Dictionary<string, HKProbeMatch>();
        private void btn_find_Click(object sender, EventArgs e)
        {
            if (deviceHandler != null)
            {
                deviceHandler.Stop();
                deviceHandler = null;
                btn_find.Text = "开始搜索";
                return;
            }
            btn_find.Text = "停止搜索";

            try
            {
                deviceHandler = new Handler.DeviceHandler(tbx_ip.Text);
                switch (cbx_deviceType.Text)
                {
                    case "海康":
                        deviceHandler.DeviceReceice += HK_DeviceReceice;
                        deviceHandler.Start();
                        deviceHandler.HKDeviceFind();
                        break;
                    case "大华":
                        //deviceHandler = new Handler.DeviceHandler(tbx_ip.Text);
                        deviceHandler.DeviceReceice += DH_DeviceReceice;
                        deviceHandler.Start();
                        deviceHandler.DaHuaDeviceFind();
                        break;
                    case "ONVIF":
                        //deviceHandler = new Handler.DeviceHandler(tbx_ip.Text);
                        deviceHandler.DeviceReceice += Onvif_DeviceReceice;
                        deviceHandler.Start();
                        deviceHandler.OnvifDeviceFind();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("请勿开启同类型软件避免端口占用\r\n" + ex.Message, "查找设备错误");

            }
        }

        private void DH_DeviceReceice(object sender, DeviceEventArgs e)
        {
            try
            {
                var hk = deviceHandler.DaHuaUnpack(e.Message);
                AddDeviceData(hk);
            }
            catch (Exception ex)
            {
                XTrace.Log.Error("大华查找设备错误 {0}", ex);
                deviceHandler.DeviceReceice -= DH_DeviceReceice;
                MessageBox.Show(ex.Message, "大华查找设备错误");
            }
        }

        private void Onvif_DeviceReceice(object sender, DeviceEventArgs e)
        {
            try
            {
                var hk = deviceHandler.OnvifUnpack(e.Message);

                //var xml_info = await deviceHandler.GetDeviceInformation(hk.OnvifAddress, "admin", "xxct111111");
                //var info = deviceHandler.XmlUnpack<Envelope>(xml_info);
                //var xml_net = await deviceHandler.GetNetworkInterfaces(hk.OnvifAddress, "admin", "xxct111111");
                //var net = deviceHandler.XmlUnpack<Envelope>(xml_net);   

                AddDeviceData(hk);
            }
            catch (Exception ex)
            {
                XTrace.Log.Error("Onvif查找设备错误 {0}", ex);
                deviceHandler.DeviceReceice -= Onvif_DeviceReceice;
                MessageBox.Show(ex.Message, "Onvif查找设备错误");
            }
        }


        private void HK_DeviceReceice(object sender, Handler.DeviceEventArgs e)
        {
            if (sender is Handler.DeviceHandler hander && e.Message != null)
            {
                try
                {
                    if (!e.Message.Contains("ProbeMatch"))
                        return;

                    var hk = hander.XmlUnpack<HKProbeMatch>(e.Message);
                    AddDeviceData(hk);
                }
                catch (Exception ex)
                {
                    XTrace.Log.Error("海康查找设备错误 {0}, {1}", ex,e.Message);
                    deviceHandler.DeviceReceice -= HK_DeviceReceice;
                    MessageBox.Show(ex.Message, "海康查找设备错误");
                }
            }
        }

        private void AddDeviceData(HKProbeMatch hk)
        {
            if (hk == null)
            {
                return;
            }
            else if (hKProbeMatches.ContainsKey(hk.IPv4Address))
            {
                return;
            }

            hKProbeMatches.Add(hk.IPv4Address,hk);
            lv_data.Invoke(() =>
            {
                var row = lv_data.Items.Add(lv_data.Items.Count.ToString());
                row.Tag = hk;
                var item = hk.GetObjectMap();
                for (int j = 1; j < lv_data.Columns.Count; j++)
                {                    
                    if (j % 2 == 0)
                        row.BackColor = Color.AliceBlue;

                    if (item.TryGetValue(lv_data.Columns[j].Name, out var value))
                    {
                        value = value ?? "";
                        row.SubItems.Add(value.ToString());
                    }
                }
                row.SubItems[0].Tag = lv_data.Items.Count;
                //lv_data.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
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
                var url =string.Format("http://{0}:{1}",hk.IPv4Address, hk.HttpPort) ;
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

                var list =new List<string>();
                foreach(ListViewItem item in lv_data.SelectedItems)
                {
                    if (item.Tag is HKProbeMatch hk && hk.DeviceType!=string.Empty)
                    {
                        list.Add(hk.IPv4Address);
                    }                    
                }
                if(!list.Any())
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

                var cameraDeviceInfo=new CameraDeviceInfo();
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
