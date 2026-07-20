using PlcClient.Model.DeviceDiscover;
using System;
using System.ComponentModel;

namespace PlcClient.Model
{
    public class LLDPDeviceVM
    {
        [DisplayName("#")]
        public int ID { get; set; }        
        public string MacAddress { get; set; } = "N/A";        
        public string ChassisId { get; set; } = "N/A";        
        public string PortId { get; set; } = "N/A";        
        public ushort TimeToLive { get; set; }        
        public string PortDescription { get;  set; }        
        public string SystemName { get; set; } = "N/A";        
        public string SystemDescription { get; set; }        
        public string ManagementAddress { get; set; }

        [DisplayName("OUI")]
        public string Description { get;  set; }
        public static LLDPDeviceVM Form(LLDPDevice item)
        {
            var vm = new LLDPDeviceVM
            {
                MacAddress =BitConverter.ToString(item.MacAddress.GetAddressBytes()),
                ChassisId = item.ChassisId,
                PortId = item.PortId,
                TimeToLive = item.TimeToLive,
                PortDescription = item.PortDescription,
                SystemName = item.SystemName,
                SystemDescription = item.SystemDescription,
                ManagementAddress = item.ManagementAddress?.ToString()
            };
            vm.Description = Handler.ArpHandler.Instance.GetDeviceInfoForMac(vm.MacAddress);

            return vm;

        }
    }
}
