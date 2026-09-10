using NewLife;
using NewLife.Data;
using NewLife.Log;
using NewLife.Net;
using PlcClient.Handler;
using PlcClient.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlcClient.Controls
{
    public partial class ENIPScan : BaseControl
    {
        ListViewHandler<ENIPDeviceVM> listViewHandler;
        public ENIPScan()
        {
            InitializeComponent();
            this.Dock = groupBox1.Dock = tableLayoutPanel1.Dock = listViewEx1.Dock = DockStyle.Fill;
            listViewHandler = new ListViewHandler<ENIPDeviceVM>(this.listViewEx1);
            listViewHandler.SetupVirtualMode();
            listViewHandler.listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            this.cbx_ips.Items.Add("0.0.0.0");
            cbx_ips.SelectedIndex = 0;
            this.cbx_ips.Items.AddRange(GetLocalAllIP());


        }
        
        //0x63,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
        //0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
        //0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
        private byte[] identity = new byte[] {
                                0x63,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                                0x00,0x00,0x00,0x00,0xfa,0x00,0x48,0x69,
                                0x4d,0x6f,0x6d,0x00,0x00,0x00,0x00,0x00
                            };

        private CancellationTokenSource cancellationTokenSource;
        private HashSet<string> _cacheDevice = new HashSet<string>();
        private void btn_start_Click(object sender, EventArgs e)
        {
            try
            {
                if (cancellationTokenSource != null && !cancellationTokenSource.IsCancellationRequested)
                {
                    cancellationTokenSource.Cancel();
                    return;
                }
                cancellationTokenSource = new CancellationTokenSource();
                cancellationTokenSource.Token.Register(() =>
                {
                    this.Invoke(() =>
                    {
                        btn_start.Text = "开始查找";
                    });
                });
                _cacheDevice.Clear();
                btn_start.Text = "取消查找";
                var ip = cbx_ips.Text;
                Task.Run(async () =>
                {
                    var allip = GetLocalAllIP();
                    string url = $"udp://255.255.255.255:44818";
                    NetUri net = new NetUri(url);
                    var client = net.CreateRemote();
                    client.Received += Client_Received;
                    client.Local.Address = IPAddress.Parse(ip);
                    int num = 1;
                    while (!cancellationTokenSource.IsCancellationRequested)
                    {
                        client.Send(identity);
                        await Task.Delay(10 * 1000, cancellationTokenSource.Token);
                    }
                    client.Close("");
                    cancellationTokenSource.Cancel();
                }, cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
        }

        private void Client_Received(object sender, ReceivedEventArgs e)
        {
            var address = e.Remote.Address.ToString();
            var data = e.Packet.ReadBytes();
            if (_cacheDevice.Contains(address) || data.Length < 24)
            {
                return;
            }
            long content = BitConverter.ToInt64(data, 14);
            if (content == 0x006d6f4d6948)
            {
                var vm = ENIPDeviceVM.Parse(data, address);
                if (vm == null)
                    return;
                _cacheDevice.Add(address);
                this.Invoke(() =>
                {
                    this.OnMsg($"发现设备 {address},总计{_cacheDevice.Count}个");
                    vm.ID = listViewHandler.DataCount + 1;
                    listViewHandler.Add(vm);
                });
            }
        }

        private void btn_export_Click(object sender, EventArgs e)
        {
            listViewHandler.ExportExcel("enip");
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            listViewHandler.Clear();
        }
    }
}
