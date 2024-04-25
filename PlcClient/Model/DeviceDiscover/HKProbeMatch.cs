using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace PlcClient.Model.DeviceDiscover
{
    [Serializable]
    [XmlRoot("ProbeMatch")]
    public class HKProbeMatch
    {
        [XmlElement("Uuid")]
        public string Uuid { get; set; }

        [XmlElement("Types")]
        public string Types { get; set; }

        [DisplayCustom("设备类型")]
        [XmlElement("DeviceType")]
        public int DeviceType { get; set; }

        [DisplayCustom("设备描述", Order = 1)]
        [XmlElement("DeviceDescription")]
        public string DeviceDescription { get; set; }

        [DisplayCustom("设备序列号", Order = 13)]
        [XmlElement("DeviceSN")]
        public string DeviceSN { get; set; }

        [DisplayCustom("端口", Order = 7)]
        [XmlElement("CommandPort")]
        public int CommandPort { get; set; }

        [DisplayCustom("Http端口", Order = 9)]
        [XmlElement("HttpPort")]
        public int HttpPort { get; set; }

        [DisplayCustom("物理地址", Order = 6)]
        [XmlElement("MAC")]
        public string MAC { get; set; }

        [DisplayCustom("IPv4地址", Order = 3)]
        [XmlElement("IPv4Address")]
        public string IPv4Address { get; set; }

        [DisplayCustom("子网掩码", Order = 4)]
        [XmlElement("IPv4SubnetMask")]
        public string IPv4SubnetMask { get; set; }

        [DisplayCustom("IPv4网关", Order = 5)]
        [XmlElement("IPv4Gateway")]
        public string IPv4Gateway { get; set; }

        [DisplayCustom("IPv6地址")]
        [XmlElement("IPv6Address")]
        public string IPv6Address { get; set; }

        [DisplayCustom("IPv6网关")]
        [XmlElement("IPv6Gateway")]
        public string IPv6Gateway { get; set; }

        [DisplayCustom("IPv6子网前缀长度")]
        [XmlElement("IPv6MaskLen")]
        public int IPv6MaskLen { get; set; }

        [DisplayCustom("DHCP状态")]
        [XmlElement("DHCP")]
        public bool DHCP { get; set; }

        [DisplayCustom("模拟通道数")]
        [XmlElement("AnalogChannelNum")]
        public int AnalogChannelNum { get; set; }

        [DisplayCustom("编码通道数", Order = 12)]
        [XmlElement("DigitalChannelNum")]
        public int DigitalChannelNum { get; set; }

        [DisplayCustom("软件版本", Order = 10)]
        [XmlElement("SoftwareVersion")]
        public string SoftwareVersion { get; set; }

        [DisplayCustom("DSP版本", Order = 11)]
        [XmlElement("DSPVersion")]
        public string DSPVersion { get; set; }

        //[DisplayCustom("启动时间")]
        [XmlElement("BootTime")]
        public string BootTime { get; set; }

        [XmlElement("Encrypt")]
        public bool Encrypt { get; set; }

        [XmlElement("ResetAbility")]
        public bool ResetAbility { get; set; }

        [XmlElement("DiskNumber")]
        public int DiskNumber { get; set; }

        [DisplayCustom("激活状态", Order = 2)]
        [XmlElement("Activated")]
        public bool Activated { get; set; }

        [DisplayCustom("密码重置能力")]
        [XmlElement("PasswordResetAbility")]
        public bool PasswordResetAbility { get; set; }

        [XmlElement("PasswordResetModeSecond")]
        public bool PasswordResetModeSecond { get; set; }

        [XmlElement("DetailOEMCode")]
        public int DetailOEMCode { get; set; }

        [XmlElement("SupportSecurityQuestion")]
        public bool SupportSecurityQuestion { get; set; }

        [XmlElement("SupportHCPlatform")]
        public bool SupportHCPlatform { get; set; }

        [XmlElement("HCPlatformEnable")]
        public bool HCPlatformEnable { get; set; }

        [XmlElement("IsModifyVerificationCode")]
        public bool IsModifyVerificationCode { get; set; }

        [XmlElement("Salt")]
        public string Salt { get; set; }

        [XmlElement("DeviceLock")]
        public bool DeviceLock { get; set; }

        [XmlElement("SDKServerStatus")]
        public bool SDKServerStatus { get; set; }

        [XmlElement("SDKOverTLSServerStatus")]
        public bool SDKOverTLSServerStatus { get; set; }

        [DisplayCustom("服务增强型端口", Order = 8)]
        [XmlElement("SDKOverTLSPort")]
        public int SDKOverTLSPort { get; set; }

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
