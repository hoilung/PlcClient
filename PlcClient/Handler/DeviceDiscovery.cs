using PacketDotNet;
using PacketDotNet.Lldp;
using PlcClient.Model.DeviceDiscover;
using SharpPcap;
using SharpPcap.LibPcap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PlcClient.Handler
{
    public partial class DeviceDiscovery
    {
        public static string DOWNLOAD_NPCAP_PATH = "https://npcap.com/dist/npcap-1.88.exe";

        public static ProfinetDcpDevice ParseIdentifyResponse(byte[] pnioPacket)
        {
            var device = new ProfinetDcpDevice();

            // DCP 头部固定为 12 字节，数据块在此之后开始
            ushort dcpDataLength = (ushort)((pnioPacket[10] << 8) | pnioPacket[11]);
            int offset = 12;
            int endOfBlocks = offset + dcpDataLength;

            // 循环遍历所有的数据块
            while (offset < endOfBlocks && offset + 4 <= pnioPacket.Length)
            {
                byte option = pnioPacket[offset];
                byte suboption = pnioPacket[offset + 1];
                // BlockLength 字段包含 Status(2B) 和 Payload 的长度
                ushort blockHeaderLengthField = (ushort)((pnioPacket[offset + 2] << 8) | pnioPacket[offset + 3]);

                // 计算这个块在网络传输中的实际总长度（包含可能的填充位）
                int totalBlockWireLength = 4 + blockHeaderLengthField; // 4 是 Opt,Sub,Len 字段的长度
                if (totalBlockWireLength % 2 != 0)
                {
                    totalBlockWireLength++; // 块总是偶数对齐
                }

                // 安全检查，防止越界
                if (offset + totalBlockWireLength > pnioPacket.Length) break;

                // 确保块长度至少包含 Status 字段
                if (blockHeaderLengthField >= 2)
                {
                    int payloadLength = blockHeaderLengthField - 2; // 减去 Status 字段的长度
                    int payloadOffset = offset + 6; // Opt(1)+Sub(1)+Len(2)+Status(2) = 6

                    if (payloadLength > 0 && payloadOffset + payloadLength <= pnioPacket.Length)
                    {
                        var payload = new byte[payloadLength];
                        Array.Copy(pnioPacket, payloadOffset, payload, 0, payloadLength);

                        // 根据 Option/Suboption 判断数据类型
                        // Option 2, Suboption 2 -> NameOfStation
                        if (option == 2 && suboption == 2)
                        {
                            device.NameOfStation = Encoding.ASCII.GetString(payload).TrimEnd('\0');
                        }
                        // Option 1, Suboption 2 -> IPAddress, SubnetMask, DefaultGateway
                        else if (option == 1 && suboption == 2)
                        {
                            if (payload.Length >= 4) device.IpAddress = new IPAddress(new byte[] { payload[0], payload[1], payload[2], payload[3] });
                            if (payload.Length >= 8) device.SubnetMask = new IPAddress(new byte[] { payload[4], payload[5], payload[6], payload[7] });
                            if (payload.Length >= 12) device.DefaultGateway = new IPAddress(new byte[] { payload[8], payload[9], payload[10], payload[11] });
                        }
                    }
                }

                // 移动到下一个块的起始位置
                offset += totalBlockWireLength;
            }

            return device;
        }

        public static async Task FindDevices(string networkName, Action<ProfinetDcpDevice> onDeviceFound = null, CancellationToken token = default)
        {


            Console.WriteLine("### PROFINET DCP Device Scanner (SharpPcap v5+ Version) ###");

            var device = LibPcapLiveDeviceList.Instance.FirstOrDefault(d => d.Description == networkName);

            if (device == null)
            {
                Console.WriteLine($"[错误] 找不到 IP 地址为 {networkName} 的网络接口。");
                return;
            }

            Console.WriteLine($"使用接口: {device.Description}");
            Console.WriteLine($"本地 MAC: {device.MacAddress}");

            var discoveredMacs = new HashSet<string>();

            // 修正: OnPacketArrival 事件处理
            device.OnPacketArrival += (sender, e) =>
            {
                // 使用 PacketDotNet.Packet.ParsePacket 来解析原始数据
                try
                {
                    var packet = Packet.ParsePacket(e.GetPacket().LinkLayerType, e.GetPacket().Data);
                    var ethernetPacket = packet.Extract<EthernetPacket>();

                    // 检查是否是 PROFINET 协议 (EtherType == 0x8892)
                    if (ethernetPacket != null && ethernetPacket.Type == EthernetType.Profinet)
                    {
                        var pnioPacket = ethernetPacket.PayloadData;

                        // 确保 pnioPacket 至少有 DCP 头部那么长 (12 字节)
                        if (pnioPacket.Length >= 12)
                        {
                            // 从 PROFINET 负载中提取 FrameID
                            ushort frameId = (ushort)((pnioPacket[0] << 8) | pnioPacket[1]);

                            // --- 最终核心修正 ---
                            // Identify Response 的 FrameID 是 0xFEFF
                            if (frameId == 0xFEFF) // <-- 正确的值是 0xFEFF!
                            {
                                //string mac = ethernetPacket.SourceHardwareAddress.ToString();
                                //if (discoveredMacs.Add(mac))
                                //{
                                //    Console.WriteLine($"[发现设备] MAC: {mac}");
                                //}
                                string macString = ethernetPacket.SourceHardwareAddress.ToString();
                                if (discoveredMacs.Add(macString))
                                {
                                    var dcpDevice = ParseIdentifyResponse(pnioPacket);
                                    dcpDevice.MacAddress = ethernetPacket.SourceHardwareAddress;
                                    onDeviceFound?.Invoke(dcpDevice);
                                    Console.WriteLine(dcpDevice.ToString());
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"处理数据包时出错: {ex.Message}");
                }
            };

            device.Open(DeviceModes.Promiscuous, 1000);
            //过滤profinet
            device.Filter = "ether proto 0x8892"; // <-- 正确的过滤条件是 ether proto 0x8892
            device.StartCapture();

            Console.WriteLine("\n发送 DCP Identify Request 广播...");
            byte[] dcpRequestFrame = BuildDcpIdentifyRequestFrame(device.MacAddress);
            device.SendPacket(dcpRequestFrame);

            if (token == default)
            {
                token = new CancellationTokenSource(TimeSpan.FromSeconds(60)).Token;
            }
            int waitSecond = 0;
            while (!token.IsCancellationRequested)
            {
                await Task.Delay(1000, token);
                waitSecond++;
                if (waitSecond % 30 == 0)
                {
                    //长时间情况下，每隔30s发一次
                    device.SendPacket(dcpRequestFrame);
                }
            }

            device.StopCapture();
            device.Close();
            Console.WriteLine("扫描结束。\n");
        }
        private static byte[] BuildDcpIdentifyRequestFrame(PhysicalAddress sourceMac)
        {
            var payload = new List<byte>();

            // --- DCP Packet Payload (内部的 PROFINET 数据) ---

            // FrameID (Identify Request: 0xFEFE)
            payload.AddRange(new byte[] { 0xFE, 0xFE });
            // ServiceID (Identify: 0x05)
            payload.Add(0x05);
            // ServiceType (Request: 0x00)
            payload.Add(0x00);
            // XID (事务 ID, 随机数或递增)
            payload.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x01 });
            // ResponseDelay (0x0080 -> 128)
            payload.AddRange(new byte[] { 0x00, 0x80 });
            // DCP Data Length (后面 DCP 块的长度)
            payload.AddRange(new byte[] { 0x00, 0x04 }); // <-- 核心修正: 长度必须是 4

            // --- DCP Block (总共 4 字节) ---
            // Option=All (0xFF), Sub-option=AllSelector (0xFF)
            payload.AddRange(new byte[] { 0xFF, 0xFF });
            // Block Length (块内数据的长度，这里为 0)
            payload.AddRange(new byte[] { 0x00, 0x00 }); // <-- 核心修正: 补上块自身的长度字段

            // --- Ethernet Frame (外部的以太网信封) ---
            var profinetMulticastMac = PhysicalAddress.Parse("01-0E-CF-00-00-00");

            var ethernetPacket = new EthernetPacket(sourceMac, profinetMulticastMac, EthernetType.Profinet)
            {
                PayloadData = payload.ToArray()
            };

            return ethernetPacket.Bytes;
        }


    }

    public partial class DeviceDiscovery
    {

        public static async Task FindDevicesLLDP(string networkName, Action<LLDPDevice> action, CancellationToken token = default)
        {
            var device = LibPcapLiveDeviceList.Instance.FirstOrDefault(m => m.Description == networkName);
            if (device == null)
            {
                return;
            }
            var discoveredMacs = new HashSet<string>();

            device.OnPacketArrival += (s, e) =>
            {
                var packet = Packet.ParsePacket(e.GetPacket().LinkLayerType, e.GetPacket().Data);
                //var aaa=ParseLLDP(packet.PayloadData);                
                var ethernetPacket = packet.Extract<EthernetPacket>();
                if (ethernetPacket != null && ethernetPacket.Type == EthernetType.Lldp)
                {
                    var mac = BitConverter.ToString(ethernetPacket.SourceHardwareAddress.GetAddressBytes());
                    if (!discoveredMacs.Add(mac))
                        return;

                    var lldpPacket = packet.Extract<LldpPacket>();
                    if (lldpPacket != null)
                    {
                        var lldpDevice = new LLDPDevice();
                        lldpDevice.MacAddress = ethernetPacket.SourceHardwareAddress;
                        foreach (var tlv in lldpPacket.TlvCollection)
                        {
                            switch (tlv.Type)
                            {
                                case TlvType.ChassisId:
                                    var chassisIdTlv = (ChassisIdTlv)tlv;
                                    string _value_id = null;

                                    if (chassisIdTlv.SubType == ChassisSubType.MacAddress)
                                    {
                                        _value_id = BitConverter.ToString(chassisIdTlv.MACAddress.GetAddressBytes());
                                    }
                                    else if (chassisIdTlv.SubType == ChassisSubType.NetworkAddress)
                                    {
                                        _value_id = chassisIdTlv.NetworkAddress.Address.ToString();
                                    }
                                    else if (chassisIdTlv.SubTypeValue is byte[])
                                    {
                                        _value_id = Encoding.ASCII.GetString(chassisIdTlv.SubTypeValue as byte[]);
                                    }
                                    else
                                    {
                                        _value_id = chassisIdTlv.ToString();
                                    }

                                    lldpDevice.ChassisId = $"SubType = {chassisIdTlv.SubType}, Id: {_value_id}";
                                    break;
                                case TlvType.PortId:
                                    var portIdTlv = ((PortIdTlv)tlv);
                                    string _value_port = null;
                                    if (portIdTlv.SubType == PortSubType.MacAddress && portIdTlv.SubTypeValue is PhysicalAddress)
                                    {
                                        _value_port = BitConverter.ToString(tlv.Bytes);
                                    }
                                    else if (portIdTlv.SubType == PortSubType.NetworkAddress && portIdTlv.SubTypeValue is NetworkAddress)
                                    {
                                        _value_port = (portIdTlv.SubTypeValue as NetworkAddress).Address.ToString();
                                    }
                                    else if (portIdTlv.SubTypeValue is byte[])
                                    {
                                        _value_port = Encoding.ASCII.GetString(portIdTlv.SubTypeValue as byte[]);
                                    }
                                    else
                                    {
                                        _value_port = portIdTlv.ToString();
                                    }
                                    lldpDevice.PortId = $"SubType = {portIdTlv.SubType}, Id: {_value_port}";
                                    break;
                                case TlvType.TimeToLive:
                                    lldpDevice.TimeToLive = ((TimeToLiveTlv)tlv).Seconds;
                                    break;
                                case TlvType.PortDescription:
                                    lldpDevice.PortDescription = ((PortDescriptionTlv)tlv).Value;
                                    break;
                                case TlvType.SystemName:
                                    lldpDevice.SystemName = ((SystemNameTlv)tlv).Name;
                                    break;
                                case TlvType.SystemDescription:
                                    lldpDevice.SystemDescription = ((SystemDescriptionTlv)tlv).Description;
                                    break;
                                case TlvType.ManagementAddress:
                                    lldpDevice.ManagementAddress = ((ManagementAddressTlv)tlv).Address.Address;
                                    break;
                            }
                        }
                        action?.Invoke(lldpDevice);
                    }
                }
            };
            device.Open(DeviceModes.Promiscuous, 1000);
            //过滤lldp协议的类型            
            device.Filter = "ether proto 0x88cc";
            device.StartCapture();
            if (token == default)
            {
                token = new CancellationTokenSource(TimeSpan.FromSeconds(60)).Token;
            }
            while (!token.IsCancellationRequested)
            {
                await Task.Delay(1000, token);
            }
            device.StopCapture();
            device.Close();
        }

    }
}
