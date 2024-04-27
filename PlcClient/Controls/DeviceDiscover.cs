using HL.Object.Extensions;
using NewLife;
using PlcClient.Model.DeviceDiscover;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            var attr = Model.DeviceDiscover.HKProbeMatch.GetDisplayCustoms().Where(m => m.Order > 0).OrderBy(m => m.Order).ToArray();

            for (int i = 0; i < attr.Length; i++)
            {
                lv_data.Columns.Add(attr[i].DataMember, attr[i].Name);
                //lv_data.DataBindings.Add("Text", list, attr[i].DataMember);

            }
            listViewHandler = new Handler.ListViewHandler(this.lv_data);
            listViewHandler.ColuminSort();
        }
        Handler.ListViewHandler listViewHandler;
        Handler.DeviceHandler deviceHandler;
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
            deviceHandler = new Handler.DeviceHandler(tbx_ip.Text, 37020, "239.255.255.250");
            deviceHandler.DeviceReceice += DeviceHandler_DeviceReceice;
            try
            {

                deviceHandler.Start();
                deviceHandler.HKDeviceFind();
            }
            catch (Exception ex)
            {
                MessageBox.Show("请勿开启同类型软件避免端口占用\r\n" + ex.Message, "查找设备错误");

            }

            #region test


            //var file = File.ReadAllText("C:\\Users\\user\\Desktop\\hk.txt").Replace("flase", "false");
            //var hk = deviceHandler.HKDeviceUnpack<HKProbeMatch>(file);
            //list.Add(hk);
            //list.Add(hk);
            //list.Add(hk);
            //list.Add(hk);            
            //for (int i = 0; i < list.Count; i++)
            //{
            //    var row = lv_data.Items.Add(lv_data.Items.Count.ToString());
            //    row.Tag = list[i];
            //    for (int j = 1; j < lv_data.Columns.Count; j++)
            //    {
            //        var item = list[i].GetObjectMap();
            //        row.SubItems.Add(item[lv_data.Columns[j].Name].ToString());
            //    }

            //}
            #endregion

        }

        private List<HKProbeMatch> hKProbeMatches = new List<HKProbeMatch>();
        private void DeviceHandler_DeviceReceice(object sender, Handler.DeviceEventArgs e)
        {
            if (sender is Handler.DeviceHandler hander && e.Message != null)
            {
                try
                {
                    if (!e.Message.Contains("ProbeMatch"))
                        return;

                    var hk = hander.HKDeviceUnpack<HKProbeMatch>(e.Message);
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
                            row.SubItems.Add(item[lv_data.Columns[j].Name].ToString());
                        }
                        row.SubItems[0].Tag = lv_data.Items.Count;
                        lv_data.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                    });
                }
                catch (Exception ex)
                {
                    deviceHandler.DeviceReceice -= DeviceHandler_DeviceReceice;
                    MessageBox.Show(ex.Message, "查找设备错误");
                }
            }
        }

        private void btn_exportExcel_Click(object sender, EventArgs e)
        {
            var filename = this.listViewHandler.ExportExcel("设备");
            if (string.IsNullOrEmpty(filename)) { return; }
            var msg = $"保存文件 {filename}";
            OnMsg(msg);
            MessageBox.Show(filename, "保存文件");
        }
    }
}
