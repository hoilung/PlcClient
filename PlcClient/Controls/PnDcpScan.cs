using PlcClient.Handler;
using PlcClient.Model.DeviceDiscover;
using SharpPcap.LibPcap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlcClient.Controls
{
    public class ProfinetDcpDeviceVM
    {
        [DisplayName("#")]
        public int ID { get; set; }
        
        public string MacAddress { get; set; }
        
        public string NameOfStation { get; set; }
        
        public string IpAddress { get; set; }
        
        public string SubnetMask { get; set; }
        
        public string DefaultGateway { get; set; }

        [DisplayName("OUI")]
        public string Description { get; set; }
        public static ProfinetDcpDeviceVM Form(ProfinetDcpDevice vm)
        {
            var item= new ProfinetDcpDeviceVM
            {
                MacAddress = BitConverter.ToString(vm.MacAddress.GetAddressBytes()),
                NameOfStation = vm.NameOfStation,
                IpAddress = vm.IpAddress.ToString(),
                SubnetMask = vm.SubnetMask.ToString(),
                DefaultGateway = vm.DefaultGateway.ToString()
            };
            item.Description= ArpHandler.Instance.GetDeviceInfoForMac(item.MacAddress);

            return item;

        }
    }
    public partial class PnDcpScan : BaseControl
    {
        ListViewHandler<ProfinetDcpDeviceVM> listViewHandler;
        public PnDcpScan()
        {
            InitializeComponent();
            this.listViewEx1.Dock = this.tableLayoutPanel1.Dock = groupBox1.Dock = DockStyle.Fill;
            this.listViewHandler = new ListViewHandler<ProfinetDcpDeviceVM>(this.listViewEx1);
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
            Task.Run(() =>
            {
                this.Invoke(() =>
                {
                    this.btn_start.Enabled = !this.btn_start.Enabled;
                });
                Handler.DeviceDiscovery.FindDevices(ip, 5, (info) =>
                {
                    this.Invoke(() => {
                        var vm = ProfinetDcpDeviceVM.Form(info);
                        vm.ID = listViewHandler.DataCount;                        
                        listViewHandler.Add(vm);
                    });
                });
                this.Invoke(() =>
                {
                    this.btn_start.Enabled = !this.btn_start.Enabled;                    
                });
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
