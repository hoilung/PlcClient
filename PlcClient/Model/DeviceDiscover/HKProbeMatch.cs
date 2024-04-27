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
        public string Uuid { get; set; }

        [XmlElement("Types")]
        public string Types { get; set; } = "inquiry";
    }


    [Serializable]
    [XmlRoot("ProbeMatch")]
    public partial class HKProbeMatch : Probe
    {

        [DisplayCustom("设备类型", Order = 1)]
        [XmlElement("DeviceDescription")]
        public string DeviceDescription { get; set; }

        [DisplayCustom("激活状态", Order = 2)]
        [XmlElement("Activated")]
        public bool Activated { get; set; }

        [DisplayCustom("IPv4地址", Order = 3)]
        [XmlElement("IPv4Address")]
        public string IPv4Address { get; set; }
        [DisplayCustom("端口", Order = 4)]
        [XmlElement("CommandPort")]
        public int CommandPort { get; set; }

        [DisplayCustom("服务增强型端口", Order = 5)]
        [XmlElement("SDKOverTLSPort")]
        public int SDKOverTLSPort { get; set; }
        [DisplayCustom("软件版本", Order = 6)]
        [XmlElement("SoftwareVersion")]
        public string SoftwareVersion { get; set; }


        [DisplayCustom("IPv4网关", Order = 7)]
        [XmlElement("IPv4Gateway")]
        public string IPv4Gateway { get; set; }


        [DisplayCustom("Http端口", Order = 8)]
        [XmlElement("HttpPort")]
        public int HttpPort { get; set; }


        [DisplayCustom("设备序列号", Order = 9)]
        [XmlElement("DeviceSN")]
        public string DeviceSN { get; set; }

        [DisplayCustom("子网掩码", Order = 10)]
        [XmlElement("IPv4SubnetMask")]
        public string IPv4SubnetMask { get; set; }
        [DisplayCustom("物理地址", Order = 11)]
        [XmlElement("MAC")]
        public string MAC { get; set; }


        [DisplayCustom("编码通道数", Order = 12)]
        [XmlElement("DigitalChannelNum")]
        public int DigitalChannelNum { get; set; }

        [DisplayCustom("DSP版本", Order = 13)]
        [XmlElement("DSPVersion")]
        public string DSPVersion { get; set; }

        [DisplayCustom("IPv4 DHCP状态", Order = 14)]
        [XmlElement("DHCP")]
        public bool DHCP { get; set; }


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
