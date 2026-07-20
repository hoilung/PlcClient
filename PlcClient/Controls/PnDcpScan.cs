using PlcClient.Handler;
using PlcClient.Model;
using SharpPcap.LibPcap;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlcClient.Controls
{

    public partial class PnDcpScan : BaseControl
    {
        ListViewHandler<PnDcpDeviceVM> listViewHandler;
        public PnDcpScan()
        {
            InitializeComponent();
            this.listViewEx1.Dock = this.tableLayoutPanel1.Dock = groupBox1.Dock = DockStyle.Fill;
            this.listViewHandler = new ListViewHandler<PnDcpDeviceVM>(this.listViewEx1);
            //this.listViewHandler.ColuminSort();
            this.listViewHandler.SetupVirtualMode();
            this.listViewHandler.listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            this.cbx_ip.Items.AddRange(GetDeviceList());
            if (this.cbx_ip.Items.Count > 0)
                this.cbx_ip.SelectedIndex = 0;
        }
        private string[] GetDeviceList()
        {
            try
            {
                this.cbx_ip.Tag = "ip";
                var devlist = LibPcapLiveDeviceList.Instance.Select(m => m.Description);
                this.cbx_ip.ComboBox.Width = 300;
                this.cbx_ip.ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                this.cbx_ip.Tag = "device";
                return devlist.ToArray();
            }
            catch (System.Exception ex)
            {
                NewLife.Log.XTrace.WriteException(ex);
            }

            return GetLocalAllIP();
        }
        private CancellationTokenSource cancellationTokenSource;

        private void btn_start_Click(object sender, System.EventArgs e)
        {
            var ip = cbx_ip.Text;
            if (cbx_ip.Tag.ToString() == "device")
            {
                var device = LibPcapLiveDeviceList.Instance.FirstOrDefault(m => m.Description == ip);
                if (device != null)
                {
                    ip = device.Addresses.First(m => m.Addr != null && m.Addr.ipAddress != null).Addr.ipAddress.ToString();
                }
            }
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
                    this.btn_start.Text = "开始查找";
                });
            });
            this.btn_start.Text = "取消查找";            
            Task.Run(async () =>
            {
                await Handler.DeviceDiscovery.FindDevices(ip, (info) =>
                {
                    this.Invoke(() =>
                    {
                        var vm = PnDcpDeviceVM.Form(info);
                        vm.ID = listViewHandler.DataCount;
                        listViewHandler.Add(vm);
                    });
                },cancellationTokenSource.Token);
            });
        }

        private void btn_export_Click(object sender, System.EventArgs e)
        {
            listViewHandler.ExportExcel("PNDCP");
        }

        private void btn_clear_Click(object sender, System.EventArgs e)
        {
            listViewHandler.Clear();
        }
    }
}
