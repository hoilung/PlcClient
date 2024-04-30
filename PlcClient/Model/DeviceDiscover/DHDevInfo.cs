using Newtonsoft.Json;

namespace PlcClient.Model.DeviceDiscover
{
    public class DHDevInfoEntity
    {
        [JsonProperty("mac")]
        public string Mac { get; set; }
        [JsonProperty("method")]
        public string Method { get; set; }
        [JsonProperty("params")]
        public ParamsEntity Params { get; set; }


        public HKProbeMatch ToHKProbeMatch()
        {
            var hk = new HKProbeMatch();
            hk.Activated = "True";
            hk.MAC = this.Mac;
            hk.DeviceType = this.Params.DeviceInfo.DeviceClass;
            hk.DeviceDescription = this.Params.DeviceInfo.DeviceType;
            hk.HttpPort = this.Params.DeviceInfo.HttpPort.ToString();
            hk.CommandPort = this.Params.DeviceInfo.Port.ToString();

            hk.DHCP = this.Params.DeviceInfo.IPv4Address.DhcpEnable.ToString();
            hk.IPv4Address = this.Params.DeviceInfo.IPv4Address.IPAddress;
            hk.IPv4SubnetMask = this.Params.DeviceInfo.IPv4Address.SubnetMask;
            hk.IPv4Gateway = this.Params.DeviceInfo.IPv4Address.DefaultGateway;

            hk.Version = this.Params.DeviceInfo.Version;
            hk.DeviceSN = this.Params.DeviceInfo.SerialNo;

            return hk;
        }

    }

    public class ParamsEntity
    {
        [JsonProperty("deviceInfo")]
        public DeviceInfoEntity DeviceInfo { get; set; }

    }

    public class DeviceInfoEntity
    {
        [JsonProperty("AlarmInputChannels")]
        public int AlarmInputChannels { get; set; }
        [JsonProperty("AlarmOutputChannels")]
        public int AlarmOutputChannels { get; set; }
        [JsonProperty("DeviceClass")]
        public string DeviceClass { get; set; }
        [JsonProperty("DeviceType")]
        public string DeviceType { get; set; }
        [JsonProperty("HttpPort")]
        public int HttpPort { get; set; }
        [JsonProperty("Port3")]
        public int Port3 { get; set; }
        [JsonProperty("Port")]
        public int Port { get; set; }
        [JsonProperty("IPv4Address")]
        public IPv4AddressEntity IPv4Address { get; set; }
        [JsonProperty("MachineName")]
        public string MachineName { get; set; }
        [JsonProperty("Manufacturer")]
        public string Manufacturer { get; set; }
        [JsonProperty("RemoteVideoInputChannels")]
        public int RemoteVideoInputChannels { get; set; }
        [JsonProperty("SerialNo")]
        public string SerialNo { get; set; }
        [JsonProperty("DeviceID")]
        public string DeviceID { get; set; }
        [JsonProperty("Vendor")]
        public string Vendor { get; set; }
        [JsonProperty("Version")]
        public string Version { get; set; }
        [JsonProperty("VideoInputChannels")]
        public int VideoInputChannels { get; set; }
        [JsonProperty("VideoOutputChannels")]
        public int VideoOutputChannels { get; set; }
        [JsonProperty("Init")]
        public int Init { get; set; }
        [JsonProperty("Find")]
        public string Find { get; set; }
        [JsonProperty("FindVersion")]
        public int FindVersion { get; set; }

    }

    public class IPv4AddressEntity
    {
        [JsonProperty("DefaultGateway")]
        public string DefaultGateway { get; set; }
        [JsonProperty("DhcpEnable")]
        public bool DhcpEnable { get; set; }
        [JsonProperty("IPAddress")]
        public string IPAddress { get; set; }
        [JsonProperty("SubnetMask")]
        public string SubnetMask { get; set; }

    }
}
