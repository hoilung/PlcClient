using System.Net;
using System.Net.NetworkInformation;

namespace PlcClient.Model.DeviceDiscover
{
    public class ProfinetDcpDevice
    {
        public PhysicalAddress MacAddress { get; set; }
        public string NameOfStation { get; set; } = "N/A";
        public IPAddress IpAddress { get; set; }
        public IPAddress SubnetMask { get; set; }
        public IPAddress DefaultGateway { get; set; }
        
        public override string ToString()
        {
            string ip = IpAddress?.ToString() ?? "N/A";
            // 如果设备名称为空，给一个提示
            string name = string.IsNullOrEmpty(NameOfStation) ? "(无名称)" : NameOfStation;
            return $"MAC: {MacAddress}, 名称: {name}, IP: {ip}, 子网掩码: {SubnetMask}, 默认网关: {DefaultGateway}";
        }
    }
}
