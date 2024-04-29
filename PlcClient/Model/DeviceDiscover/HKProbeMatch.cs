using Opc.Ua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace PlcClient.Model.DeviceDiscover
{

    public class Probe
    {
        [XmlElement("Uuid")]
        public string Uuid { get; set; } = Guid.Empty.ToString();

        [XmlElement("Types")]
        public string Types { get; set; } = "inquiry";
    }


    [Serializable]
    [XmlRoot("ProbeMatch")]
    public partial class HKProbeMatch : Probe
    {

        [DisplayCustom("设备类型", Order = 1)]
        [XmlElement("DeviceDescription")]
        public string DeviceDescription { get; set; } = string.Empty;

        [DisplayCustom("激活状态", Order = 2)]
        [XmlElement("Activated")]
        public string Activated { get; set; } = string.Empty;

        [DisplayCustom("IPv4地址", Order = 3)]
        [XmlElement("IPv4Address")]
        public string IPv4Address { get; set; } = string.Empty;

        [DisplayCustom("端口", Order = 4)]
        [XmlElement("CommandPort")]
        public string CommandPort { get; set; } = string.Empty;

        [DisplayCustom("服务增强型端口", Order = 5)]
        [XmlElement("SDKOverTLSPort")]
        public string SDKOverTLSPort { get; set; } = string.Empty;
        [DisplayCustom("软件版本", Order = 6)]
        [XmlElement("SoftwareVersion")]
        public string SoftwareVersion { get; set; } = string.Empty;


        [DisplayCustom("IPv4网关", Order = 7)]
        [XmlElement("IPv4Gateway")]
        public string IPv4Gateway { get; set; } = string.Empty;


        [DisplayCustom("Http端口", Order = 8)]
        [XmlElement("HttpPort")]
        public string HttpPort { get; set; } = string.Empty;


        [DisplayCustom("设备序列号", Order = 9)]
        [XmlElement("DeviceSN")]
        public string DeviceSN { get; set; } = string.Empty;

        [DisplayCustom("子网掩码", Order = 10)]
        [XmlElement("IPv4SubnetMask")]
        public string IPv4SubnetMask { get; set; }= string.Empty;

        [DisplayCustom("物理地址", Order = 11)]
        [XmlElement("MAC")]
        public string MAC { get; set; } = string.Empty;


        [DisplayCustom("编码通道数", Order = 12)]
        [XmlElement("DigitalChannelNum")]
        public string DigitalChannelNum { get; set; } = string.Empty;

        [DisplayCustom("DSP版本", Order = 13)]
        [XmlElement("DSPVersion")]
        public string DSPVersion { get; set; } = string.Empty;

        [DisplayCustom("IPv4 DHCP状态", Order = 14)]
        [XmlElement("DHCP")]
        public string DHCP { get; set; } = string.Empty;


    }

    public partial class HKProbeMatch
    {
        public static DisplayCustomAttribute[] GetDisplayCustoms()
        {
            var ps = HL.Object.Extensions.ReflectionCache.GetProperties(typeof(HKProbeMatch));
            return ps.Where(m => m.IsDefined(typeof(DisplayCustomAttribute), false)).Select(m =>
            {
                var attr = m.GetCustomAttribute<DisplayCustomAttribute>();
                attr.DataMember = m.Name;
                return attr;

            }).ToArray();
        }
    }
}
