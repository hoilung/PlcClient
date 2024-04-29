using HL.Object.Extensions;
using PlcClient.Handler;
using PlcClient.Model.DeviceDiscover;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

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

            var attr = Model.DeviceDiscover.HKProbeMatch.GetDisplayCustoms().Where(m => m.Order > 0).OrderBy(m => m.Order).ToArray();

            for (int i = 0; i < attr.Length; i++)
            {
                lv_data.Columns.Add(attr[i].DataMember, attr[i].Name);
            }
            listViewHandler = new Handler.ListViewHandler(this.lv_data);
            listViewHandler.ColuminSort();
        }
        Handler.ListViewHandler listViewHandler;
        Handler.DeviceHandler deviceHandler;
        private List<HKProbeMatch> hKProbeMatches = new List<HKProbeMatch>();
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
                switch (cbx_deviceType.Text)
                {
                    case "海康":
                        deviceHandler = new Handler.DeviceHandler(tbx_ip.Text, 37020, "239.255.255.250");
                        deviceHandler.DeviceReceice += HK_DeviceReceice;
                        deviceHandler.Start();
                        deviceHandler.HKDeviceFind();
                        break;
                    case "大华":
                        deviceHandler = new Handler.DeviceHandler(tbx_ip.Text, 37801, "239.255.255.251");
                        deviceHandler.DeviceReceice += DH_DeviceReceice;
                        deviceHandler.Start();
                        deviceHandler.DaHuaDeviceFind();
                        break;
                    case "宇视":
                        deviceHandler = new Handler.DeviceHandler(tbx_ip.Text, 3705, "239.255.255.250");
                        deviceHandler.DeviceReceice += YS_DeviceReceice;
                        deviceHandler.Start();
                        deviceHandler.UniViewDeviceFind();
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

        }

        private void YS_DeviceReceice(object sender, DeviceEventArgs e)
        {
            try
            {
                var hk = deviceHandler.UniViewUnpack(e.Message);
                AddDeviceData(hk);
            }
            catch (Exception ex)
            {
                deviceHandler.DeviceReceice -= YS_DeviceReceice;
                MessageBox.Show(ex.Message, "宇视查找设备错误"); 
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
            else if (hKProbeMatches.Exists(m => m.IPv4Address == hk.IPv4Address))
            {
                return;
            }

            hKProbeMatches.Add(hk);
            lv_data.Invoke(() =>
            {
                var row = lv_data.Items.Add(lv_data.Items.Count.ToString());
                row.Tag = hk;
                for (int j = 1; j < lv_data.Columns.Count; j++)
                {
                    var item = hk.GetObjectMap();
                    if (j % 2 == 0)
                        row.BackColor = Color.AliceBlue;
                    row.SubItems.Add(item[lv_data.Columns[j].Name].ToString());
                }
                row.SubItems[0].Tag = lv_data.Items.Count;
                lv_data.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            });
        }

        private void btn_exportExcel_Click(object sender, EventArgs e)
        {
            var filename = this.listViewHandler.ExportExcel("设备");
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
    }
}
