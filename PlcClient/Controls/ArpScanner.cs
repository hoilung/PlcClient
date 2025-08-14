using NetTools;
using NewLife.Reflection;
using PlcClient.Handler;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace PlcClient.Controls
{
    public partial class ArpScanner : BaseControl
    {

        ListViewHandler listViewHandler;
        public ArpScanner()
        {
            InitializeComponent();

            groupBox1.Dock = tableLayoutPanel1.Dock = lv_data.Dock = System.Windows.Forms.DockStyle.Fill;
            listViewHandler = new ListViewHandler(this.lv_data);
            listViewHandler.ColuminSort();

        }

        protected override void OnLoad(EventArgs e)
        {
            var list = new List<string>();
            var ni = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface ni2 in ni)
            {
                if (ni2.NetworkInterfaceType == NetworkInterfaceType.Ethernet && ni2.OperationalStatus == OperationalStatus.Up)
                {
                    var ip = ni2.GetIPProperties();
                    if (ip.UnicastAddresses.Count > 0)
                    {
                        var ipv4 = ip.UnicastAddresses.FirstOrDefault(m => m.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                        if (ipv4 != null)
                        {
                            list.Add($"{ipv4.Address}/{ipv4.IPv4Mask}");
                        }
                    }
                }
            }
            this.cbx_ip.Items.AddRange(list.ToArray());
            if (list.Count > 0)
                cbx_ip.SelectedIndex = 0;
            base.OnLoad(e);
        }

        private void btn_scan_Click(object sender, System.EventArgs e)
        {
            if (btn_scan.Text == "取消扫描")
            {
                ArpHandler.Instance.Cancel();
                btn_scan.Text = "开始扫描";
                return;
            }
            var list = IPAddressRange.Parse(cbx_ip.Text).AsEnumerable();
            if (list.Count() > 256 && MessageBox.Show("当前扫描IP范围较大，共计" + list.Count() + "个，请确认是否扫描全部网段", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }
            btn_scan.Text = "取消扫描";
            OnMsg("设备扫描开始");

            var localIP = GetLocalIP();// cbx_ip.Text.Split('/')[0];
            progressBar1.Maximum = list.Count();
            progressBar1.Value = 0;
            int port = tbx_port.Text.ToInt(0);
            if (port < 0 || port > 65535)
            {
                port = 0;
            }          
            ArpHandler.Instance.PingIP(list, port, (pe) =>
            {
                lv_data.Invoke(new MethodInvoker(() =>
                {
                    var row = lv_data.Items.Add(lv_data.Items.Count.ToString());
                    row.SubItems[0].Tag = lv_data.Items.Count;
                    if (lv_data.Items.Count % 2 == 0)
                    {
                        row.BackColor = Color.AliceBlue;
                    }

                    //var area = pe[pe.Length - 1];
                    if (localIP.Equals(pe[0]))
                    {
                        pe[pe.Length - 1] = "本机IP";
                        row.BackColor = Color.LightGreen;
                    }
                    row.SubItems.AddRange(pe);
                    //if (lv_data.Items.Count % 10 == 0)
                    //{
                    //    lv_data.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                    //}
                    progressBar1.PerformStep();
                }));
            }, () =>
            {

                lv_data.Invoke(new MethodInvoker(() =>
                {
                    // lv_data.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                    btn_scan.Text = "开始扫描";
                }));
                OnMsg("设备扫描结束");
                //MessageBox.Show("设备扫描结束", "提示");
            });

        }

        private void btn_export_Click(object sender, EventArgs e)
        {
            var filename = listViewHandler.ExportExcel("设备扫描");
            if (string.IsNullOrEmpty(filename)) { return; }
            var msg = $"保存文件 {filename}";
            OnMsg(msg);
            MessageBox.Show(filename, "保存文件");
        }

        private void btn_clearall_Click(object sender, EventArgs e)
        {
            lv_data.Items.Clear();
        }


        private void toolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
