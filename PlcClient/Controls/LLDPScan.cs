using PlcClient.Handler;
using PlcClient.Model;
using SharpPcap.LibPcap;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlcClient.Controls
{

    public partial class LLDPScan : BaseControl
    {

        ListViewHandler<LLDPDeviceVM> listViewHandler;
        public LLDPScan()
        {
            InitializeComponent();
            this.listViewEx1.Dock = this.tableLayoutPanel1.Dock = groupBox1.Dock = DockStyle.Fill;
            listViewHandler = new ListViewHandler<LLDPDeviceVM>(this.listViewEx1);
            listViewHandler.SetupVirtualMode();
            listViewHandler.listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            this.cbx_ip.Items.AddRange(this.GetDeviceList());
            if (cbx_ip.Items.Count > 0)
                cbx_ip.SelectedIndex = 0;
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
        private void btn_start_Click(object sender, EventArgs e)
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
            var deviceName = cbx_ip.Text;
            btn_start.Text = "取消查找";
            Task.Run(async () =>
            {

                await DeviceDiscovery.FindDevicesLLDP(deviceName, (LLDPDevice) =>
                {
                    var vm = LLDPDeviceVM.Form(LLDPDevice);
                    this.Invoke(() =>
                    {
                        vm.ID = listViewHandler.DataCount;
                        listViewHandler.Add(vm);
                    });
                }, cancellationTokenSource.Token);

            });
        }

        private void btn_export_Click(object sender, EventArgs e)
        {
            listViewHandler.ExportExcel("LLDP");
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            listViewHandler.Clear();
        }
    }
}
