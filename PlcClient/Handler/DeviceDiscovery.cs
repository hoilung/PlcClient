using PacketDotNet;
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
    public class DeviceDiscovery
    {
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

        public static void FindDevices(string localIpAddress, int timeoutSeconds = 5,Action<ProfinetDcpDevice> onDeviceFound = null)
        {
            Console.WriteLine("### PROFINET DCP Device Scanner (SharpPcap v5+ Version) ###");

            var device = LibPcapLiveDeviceList.Instance.FirstOrDefault(d =>
                d.Addresses.Any(a => a.Addr?.ipAddress?.ToString() == localIpAddress));

            if (device == null)
            {
                Console.WriteLine($"[错误] 找不到 IP 地址为 {localIpAddress} 的网络接口。");
                return;
            }

            Console.WriteLine($"使用接口: {device.Description}");
            Console.WriteLine($"本地 MAC: {device.MacAddress}");

            var discoveredMacs = new HashSet<string>();

            // 修正: OnPacketArrival 事件处理
            device.OnPacketArrival += (sender, e) => {
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
            device.StartCapture();

            Console.WriteLine("\n发送 DCP Identify Request 广播...");
            byte[] dcpRequestFrame = BuildDcpIdentifyRequestFrame(device.MacAddress);
            device.SendPacket(dcpRequestFrame);

            Console.WriteLine($"等待 {timeoutSeconds} 秒以接收响应...");
            Thread.Sleep(timeoutSeconds * 1000);

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
}
