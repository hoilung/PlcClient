using NetTools;
using NewLife;
using NewLife.Log;
using Opc.Hda;
using OpcRcw.Da;
using PlcClient.Model.DeviceDiscover;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
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
        bool IsPrivateNetwork3(string ipv4Address)
        {
            if (IPAddress.TryParse(ipv4Address, out var ip))
            {
                byte[] ipBytes = ip.GetAddressBytes();
                if (ipBytes[0] == 10) return true;
                if (ipBytes[0] == 172 && ipBytes[1] >= 16 && ipBytes[1] <= 31) return true;
                if (ipBytes[0] == 192 && ipBytes[1] == 168) return true;
            }
            return false;
        }
        bool IsPrivateNetwork3(IPAddress ipv4Address)
        {
            if (ipv4Address != null)
            {
                byte[] ipBytes = ipv4Address.GetAddressBytes();
                if (ipBytes[0] == 10) return true;
                if (ipBytes[0] == 172 && ipBytes[1] >= 16 && ipBytes[1] <= 31) return true;
                if (ipBytes[0] == 192 && ipBytes[1] == 168) return true;
            }
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
                                var connectTask = ping.SendPingAsync(ip);
                                var timeoutTask = Task.Delay(500);
                                var complateTask = await Task.WhenAny(connectTask, timeoutTask);
                                result = complateTask == timeoutTask ? IPStatus.TimedOut : IPStatus.Success;
                                await complateTask;
                            }
                        }
                        else if (port > 0 && port < 65535)
                        {
                            using (var client = new TcpClient())
                            {
                                var connectTask = client.ConnectAsync(ip, port);
                                var timeoutTask = Task.Delay(500);
                                var complateTask = await Task.WhenAny(connectTask, timeoutTask);
                                result = complateTask == timeoutTask ? IPStatus.TimedOut : IPStatus.Success;
                                await complateTask;
                            }
                        }
                        if (IsPrivateNetwork3(ip))
                        {
                            mac = ResolveMac(ip.ToString());
                            deviceInfo = GetDeviceInfoForMac(mac);
                            area_local = true;
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
            if (_deviceOUI.TryGetValue(mac.Substring(0, 8), out var result))
                return result;
            return un_device;
        }

        private Dictionary<string, string> GetDeviceOUI()
        {
            // var sss = Properties.Resources.oui.Split('\r').Select(m => m.Split('|')).GroupBy(m => m[0]).Where(m => m.Count() > 1).ToList();


            return Properties.Resources.oui.Split('\r').Select(m => m.Split('|')).ToDictionary(m => m[0].Trim(), m => m[1].Trim(), StringComparer.OrdinalIgnoreCase);
        }
    }
}
