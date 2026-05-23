using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlcClient.Handler
{
    public partial class ArpHandler
    {
        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        public static extern int SendARP(int DestIP, int SrcIP, byte[] pMacAddr, ref uint PhyAddrLen);

    }

    public partial class ArpHandler
    {
        public static ArpHandler Instance { get; } = new ArpHandler();
        private readonly Dictionary<string, string> _deviceOUI;
        private const string un_mac = "00-00-00-00-00-00";
        private const string un_device = "unknown";

        private ArpHandler()
        {
            _deviceOUI = GetDeviceOUI();
        }
        private CancellationTokenSource CancellationTokenSource { get; set; }

        public void Cancel()
        {
            CancellationTokenSource?.Cancel();
        }
        public void ScanIP(IEnumerable<IPAddress> list, Action<string[]> actionProcess, Action actionEnd)
        {
            try
            {
                CancellationTokenSource = new CancellationTokenSource();

                //var list = IPAddressRange.Parse(ip_and_mask).AsEnumerable();
                Task.Run(() =>
                {
                    Parallel.ForEach(list, ip =>
                    {
                        if (this.CancellationTokenSource.IsCancellationRequested)
                            return;
                        var mac = ResolveMac(ip.ToString());
                        if (mac == "00-00-00-00-00-00")
                            return;
                        var deviceInfo = GetDeviceInfoForMac(mac);
                        //XTrace.WriteLine($"{ip} {mac} {deviceInfo}");
                        actionProcess?.Invoke(new string[] { ip.ToString(), mac, deviceInfo });
                    });
                    actionEnd?.Invoke();
                }, CancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                MessageBox.Show(ex.Message, "扫描错误");
            }
        }

        bool IsPrivateNetwork3(IPAddress ipv4Address)
        {
            if (ipv4Address == null)
                return false;

            // 如果是 IPv6，检查是否为私有地址
            if (ipv4Address.AddressFamily == AddressFamily.InterNetworkV6)
            {
                return ipv4Address.IsIPv6LinkLocal || ipv4Address.IsIPv6SiteLocal;
            }
            if (ipv4Address.AddressFamily != AddressFamily.InterNetwork)
                return false;
            byte[] ipBytes = ipv4Address.GetAddressBytes();
            // 10.0.0.0/8 - RFC 1918
            if (ipBytes[0] == 10) return true;

            // 172.16.0.0/12 - RFC 1918
            if (ipBytes[0] == 172 && ipBytes[1] >= 16 && ipBytes[1] <= 31) return true;

            // 192.168.0.0/16 - RFC 1918
            if (ipBytes[0] == 192 && ipBytes[1] == 168) return true;

            // 127.0.0.0/8 - Loopback
            if (ipBytes[0] == 127) return true;

            // 169.254.0.0/16 - Link-local
            if (ipBytes[0] == 169 && ipBytes[1] == 254) return true;

            return false;
        }

        public void PingIP(IEnumerable<IPAddress> list, int ipPort, Action<string[]> actionProcess, Action actionEnd)
        {
            try
            {
                async Task PingHostAsync(IPAddress ip, int port = 0)
                {
                    try
                    {
                        var result = IPStatus.Unknown;
                        var mac = string.Empty;
                        var deviceInfo = string.Empty;
                        bool area_local = false;
                        if (port == 0)
                        {
                            using (var ping = new Ping())
                            {
                                var num = 1;
                                while (result != IPStatus.Success)
                                {
                                    if (num > 3) break;
                                    var pingTask = await ping.SendPingAsync(ip, 1000);
                                    result = pingTask.Status == IPStatus.Success ? IPStatus.Success : IPStatus.TimedOut;
                                    num++;
                                }
                            }
                        }
                        else if (port > 0 && port < 65535)
                        {
                            var client = new TcpClient();

                            var connectTask = client.ConnectAsync(ip, port);
                            var timeoutTask = Task.Delay(1000);
                            var complateTask = await Task.WhenAny(connectTask, timeoutTask);

                            try
                            {
                                if (timeoutTask.IsCompleted)
                                {
                                    result = IPStatus.TimedOut;
                                }
                                else
                                {
                                    result = client.Connected ? IPStatus.Success : IPStatus.TimedOut;
                                }
                                await complateTask;
                            }
                            catch (SocketException ex)
                            {
                                result = IPStatus.TimedOut;
                            }
                            catch (Exception ex)
                            {
                                result = IPStatus.TimedOut;
                            }
                            finally
                            {
                                if (client.Client.Connected)
                                {
                                    client.Close();
                                }
                            }
                        }
                        if (IsPrivateNetwork3(ip))
                        {
                            area_local = true;
                            if (result == IPStatus.Success)
                            {
                                mac = ResolveMac(ip.ToString());
                                deviceInfo = GetDeviceInfoForMac(mac);
                            }
                        }
                        actionProcess?.Invoke(new string[] { ip.ToString(), result.ToString(), mac == un_mac ? string.Empty : mac, deviceInfo == un_device ? string.Empty : deviceInfo, area_local ? "本地" : "远程" });
                    }
                    catch (Exception ex)
                    {
                    }

                }


                CancellationTokenSource = new CancellationTokenSource();
                Task.Factory.StartNew(async () =>
                {
                    var tasks = new List<Task>();
                    foreach (var ip in list)
                    {
                        tasks.Add(PingHostAsync(ip, ipPort));
                        //await PingHostAsync(ip, ipPort);
                    }
                    await Task.WhenAll(tasks);
                    actionEnd?.Invoke();
                }, CancellationTokenSource.Token);

            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                MessageBox.Show(ex.Message, "ping错误");
            }
        }
        public string ResolveMac(string destIp)
        {
            byte[] macAddr = new byte[6];
            uint macAddrLen = (uint)macAddr.Length;
            var destIP = IPAddress.Parse(destIp);
            var by_destIP = BitConverter.ToInt32(destIP.GetAddressBytes(), 0);
            SendARP(by_destIP, 0, macAddr, ref macAddrLen);
            return BitConverter.ToString(macAddr);//.Replace("-", ":");
        }

        private string GetDeviceInfoForMac(string mac)
        {
            if (mac == un_mac)
            {
                return un_device;
            }
            var mac2 = mac.Replace("-", ":");
            foreach (var len in lenSet)
            {
                //var key_int = MacToInt(mac.Substring(0, len));
                if (_deviceOUI.TryGetValue(mac2.Substring(0, len), out var result))
                    return result;
            }
            return un_device;
        }



        /// <summary>
        /// 将MAC地址字符串转换为ulong整数
        /// </summary>
        /// <param name="macStr">MAC地址字符串，支持冒号、连字符分隔</param>
        /// <returns>MAC地址对应的64位整数</returns>
        public static ulong MacToInt(string macStr)
        {
            // 移除所有分隔符（冒号、连字符）并转为大写
            string macClean = Regex.Replace(macStr, "[:-]", "").ToUpper();

            // 将十六进制字符串转换为ulong
            return Convert.ToUInt64(macClean, 16);
        }

        private HashSet<int> lenSet = new HashSet<int>();
        private Dictionary<string, string> GetDeviceOUI()
        {
            var kv = new Dictionary<string, string>();
            var lines = Properties.Resources.oui.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Skip(1).ToArray();
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                var parts = line.Split(new[] { ',' }, count: 2);
                var key = parts[0].Split('/')[0];
                var value = parts[1].Trim('"');                
                try
                {
                    kv.Add(key, value);                    
                }
                catch (Exception)
                {

                }
            }
            lenSet = kv.Keys.GroupBy(m => m.Length).OrderByDescending(m => m.Count()).Select(m => m.Key).ToHashSet();
            return kv;
        }
    }
}
