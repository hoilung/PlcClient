using PlcClient.Controls;
using PlcClient.Handler;
using PlcClient.Model.DeviceDiscover;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlcClient.Model
{
    public class PnDcpDeviceVM
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
        public static PnDcpDeviceVM Form(ProfinetDcpDevice vm)
        {
            var item = new PnDcpDeviceVM
            {
                MacAddress = BitConverter.ToString(vm.MacAddress.GetAddressBytes()),
                NameOfStation = vm.NameOfStation,
                IpAddress = vm.IpAddress.ToString(),
                SubnetMask = vm.SubnetMask.ToString(),
                DefaultGateway = vm.DefaultGateway.ToString()
            };
            item.Description = ArpHandler.Instance.GetDeviceInfoForMac(item.MacAddress);

            return item;

        }
    }
}
