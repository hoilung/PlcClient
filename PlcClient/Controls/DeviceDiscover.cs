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




        }
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
            deviceHandler.Start();
            deviceHandler.HKDeviceFind();

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

        private void DeviceHandler_DeviceReceice(object sender, Handler.DeviceEventArgs e)
        {
            if (sender is Handler.DeviceHandler hander && e.Message != null)
            {
                try
                {
                    var hk = hander.HKDeviceUnpack<HKProbeMatch>(e.Message);

                    lv_data.Invoke(() =>
                    {
                        var row = lv_data.Items.Add(lv_data.Items.Count.ToString());
                        row.Tag = hk;
                        for (int j = 1; j < lv_data.Columns.Count; j++)
                        {
                            var item = hk.GetObjectMap();
                            row.SubItems.Add(item[lv_data.Columns[j].Name].ToString());
                        }
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("查找设备错误", ex.Message);
                }
            }
        }
    }
}
