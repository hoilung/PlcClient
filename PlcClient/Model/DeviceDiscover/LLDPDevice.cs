using System.Net;
using System.Net.NetworkInformation;

namespace PlcClient.Model.DeviceDiscover
{
    public class LLDPDevice
    {
        public PhysicalAddress MacAddress { get; set; }
        public string ChassisId { get; set; } = "N/A";
        public string PortId { get; set; } = "N/A";
        
        public ushort TimeToLive { get; set; }
        public string PortDescription { get; internal set; }
        public string SystemName { get; set; } = "N/A";        
        public string SystemDescription { get;  set; }
        public IPAddress ManagementAddress { get; set; }
        

        public override string ToString()
        {
            string ip = ManagementAddress?.ToString() ?? "N/A";
            return $"[LLDP 设备] 系统名称: {SystemName}, IP: {ip}, 机箱ID: {ChassisId}, 端口ID: {PortId}";
        }
    }
}
