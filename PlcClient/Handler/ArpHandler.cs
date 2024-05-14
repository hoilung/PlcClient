using NetTools;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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


        private ArpHandler()
        {
            _deviceOUI = GetDeviceOUI();
        }
        private CancellationTokenSource CancellationTokenSource { get; set; }
        public void ScanIP(string ip_and_mask, Action<string[]> actionProcess, Action actionEnd)
        {
            try
            {
                CancellationTokenSource = new CancellationTokenSource();

                var list = IPAddressRange.Parse(ip_and_mask).AsEnumerable();
                Task.Run(() =>
                {
                    Parallel.ForEach(list, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount }, ip =>
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
            if (mac == "00-00-00-00-00-00")
            {
                return "unknown";
            }
            if (_deviceOUI.TryGetValue(mac.Substring(0, 8), out var result))
                return result;
            return "unknown";
        }

        private Dictionary<string, string> GetDeviceOUI()
        {
            // var sss = Properties.Resources.oui.Split('\r').Select(m => m.Split('|')).GroupBy(m => m[0]).Where(m => m.Count() > 1).ToList();


            return Properties.Resources.oui.Split('\r').Select(m => m.Split('|')).ToDictionary(m => m[0].Trim(), m => m[1].Trim(), StringComparer.OrdinalIgnoreCase);
        }
    }
}
