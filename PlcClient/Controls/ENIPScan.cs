using NewLife;
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
            this.Dock = groupBox1.Dock = listViewEx1.Dock = DockStyle.Fill;
            listViewHandler = new ListViewHandler<ENIPDeviceVM>(this.listViewEx1);
            listViewHandler.SetupVirtualMode();
            listViewHandler.listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

            this.cbx_ips.Items.AddRange(GetLocalAllIP());
            if (cbx_ips.Items.Count > 0)
                cbx_ips.SelectedIndex = 0;

        }
        private byte[] identity = new byte[] {
                                0x63,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                                0x00,0x00,0x00,0x00,0xfa,0x00,0x48,0x69,
                                0x4d,0x6f,0x6d,0x00,0x00,0x00,0x00,0x00
                            };

        private CancellationTokenSource cancellationTokenSource;
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
                btn_start.Text = "取消查找";

                Task.Run(async () =>
                {
                    var allip = GetLocalAllIP();
                    string url = $"udp://255.255.255.255:44818";
                    NetUri net = new NetUri(url);
                    var list = new List<ISocketClient>();
                    foreach (var ip in allip)
                    {
                        var client = net.CreateRemote();
                        client.Received += Client_Received;
                        client.Local.Address = IPAddress.Parse(ip);
                        if (client.Open())
                        {
                            list.Add(client);
                        }
                    }
                    
                    while (!cancellationTokenSource.IsCancellationRequested)
                    {                        
                        foreach (var client in list)
                        {
                            client.Send(identity);
                        }
                        await Task.Delay(10 * 1000, cancellationTokenSource.Token);
                        break;
                    }
                    foreach (var client in list)
                    {
                        client.Close("");
                    }
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
            var data = e.Packet.GetSpan();
            long content = BitConverter.ToInt64(data.ToArray(), 14);
            if (content == 0x006d6f4d6948)
            {
                var vm = ENIPDeviceVM.Parse(data.ToArray(), e.Remote.Address.ToString());
                this.Invoke(() =>
                {
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
