using NewLife;
using NewLife.Data;
using NewLife.Net;
using NewLife.Xml;
using Newtonsoft.Json;
using Opc.Da;
using PlcClient.Model.DeviceDiscover;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Xml.Serialization;

namespace PlcClient.Handler
{
    public class DeviceEventArgs : System.EventArgs
    {
        public DeviceEventArgs(IPEndPoint from, string message)
        {
            this.From = from;
            this.Message = message;
        }
        public IPEndPoint From { get; private set; }
        public string Message { get; private set; }
    }
    public enum CameraProtocol
    {
        HK,
        DH,
        ONVIF,
    }

    public class DeviceHandler : Onvif
    {
        public bool IsStart { get; private set; }
        public CameraProtocol CameraProtocol { get; set; }
        private CancellationTokenSource CancellationTokenSource;
        public event EventHandler<DeviceEventArgs> DeviceReceice;


        protected virtual void OnBroadcastReceice(DeviceEventArgs e)
        {
            if (DeviceReceice != null)
            {
                DeviceReceice(this, e);
            }
        }
        /// <summary>
        /// xml反序列化设备发现解包
        /// </summary>
        /// <param name="message"></param>
        public T XmlUnpack<T>(string message)
        {
            //读取message 序列化为 对象
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            var obj = (T)xmlSerializer.Deserialize(new StringReader(message));            
            return obj;
        }
        public HKProbeMatch HKUnpack(string message)
        {
           return message.ToXmlEntity<HKProbeMatch>();
        }

        #region Onvif设备解包


        /// <summary>
        /// 宇视设备查找，数据解包
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public HKProbeMatch OnvifUnpack(string message)
        {
            var probeMatch = new HKProbeMatch();
            var obj = this.XmlUnpack<Envelope>(message);
            if (obj != null && obj.Body != null && obj.Body.ProbeMatches != null && obj.Body.ProbeMatches.ProbeMatch != null)
            {
                if (obj.Body.ProbeMatches.ProbeMatch.Length > 0)
                {
                    var item = obj.Body.ProbeMatches.ProbeMatch.FirstOrDefault();

                    probeMatch.OnvifAddress = item.XAddrs;

                    var ips = item.XAddrs.Split(' ').Select(m => new Uri(m)).Select(m => m.Host).ToArray();
                    var scopes = item.Scopes.Split(' ').Select(m => new Uri(m)).Select(m => m.AbsolutePath).ToArray();

                    probeMatch.IPv4Address = ips.FirstOrDefault(m => !m.Contains(":")) ?? string.Empty;
                    probeMatch.MAC = scopes.FirstOrDefault(m => m.StartsWith("/MAC/"))?.Replace("/MAC/", "") ?? string.Empty;
                    // probeMatch.DeviceDescription = scopes.FirstOrDefault(m => m.StartsWith("/name/"))?.Replace("/name/", "");
                    probeMatch.DeviceDescription = scopes.FirstOrDefault(m => m.StartsWith("/hardware/"))?.Replace("/hardware/", "") ?? string.Empty;
                }
            }
            return probeMatch;
        }


        #endregion

        #region 大华设备解包

        public HKProbeMatch DaHuaUnpack(string message)
        {
            var jsonStr = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(message).Skip(32).ToArray());

            DHDevInfoEntity devInfoEntity = JsonConvert.DeserializeObject<DHDevInfoEntity>(jsonStr);

            return devInfoEntity.ToHKProbeMatch();
        }

        #endregion

        public void Start()
        {
            this.IsStart = true;
            this.CancellationTokenSource = new CancellationTokenSource();
            this.CancellationTokenSource.Token.Register(() =>
            {
                this.IsStart = false;
            });
            var ips = new List<string>();
            var msg_tpl = string.Empty;
            switch (this.CameraProtocol)
            {
                case CameraProtocol.HK:
                    /// 海康设备查找
                    /// 广播地址：239.255.255.250
                    /// 广播地址：239.255.255.250
                    /// 端口：37020
                    ips.Add("udp://239.255.255.250:37020");
                    if (System.Net.Sockets.Socket.OSSupportsIPv6)
                        ips.Add("udp://[ff02::c]:37020");
                    msg_tpl = Properties.Resources.hikvision;
                    break;
                case CameraProtocol.DH:
                    /// 大华网络设备发现
                    /// 广播地址：239.255.255.251
                    /// 端口：37810，已激活的
                    /// 端口：5050，可能是未激活的，暂无测试
                    ips.Add("udp://239.255.255.251:37810");
                    if (System.Net.Sockets.Socket.OSSupportsIPv6)
                        ips.Add("udp://[ff02::c]:37810");
                    var base64 = Properties.Resources.DaHua.ToBase64();
                    msg_tpl = Encoding.UTF8.GetString(base64);
                    break;
                case CameraProtocol.ONVIF:
                    /// 网络设备查找(基于onvif协议)
                    /// 广播地址：239.255.255.250
                    /// 广播地址：239.255.255.255
                    /// 端口：3702
                    ips.Add("udp://239.255.255.250:3702");
                    ips.Add("udp://239.255.255.255:3702");
                    msg_tpl = Properties.Resources.onvif.Replace("{uuid}", Guid.NewGuid().ToString());
                    break;
            }
            this.IsStart = true;
            Task.Run(async () =>
            {
                var clients = new List<ISocketClient>();
                foreach (string adr in ips)
                {
                    var uri = new NewLife.Net.NetUri(adr);
                    var udp = uri.CreateRemote();
#if DEBUG
                    udp.Port = uri.Port;
#endif
                    udp.Received += Client_Received;
                    clients.Add(udp);
                }
                while (!CancellationTokenSource.IsCancellationRequested && msg_tpl != string.Empty)
                {
                    var message = msg_tpl.Replace("{uuid}", Guid.NewGuid().ToString());
                    foreach (var client in clients)
                    {
                        client.Send(message);
                    }
                    await Task.Delay(30*1000, CancellationTokenSource.Token);
                }
            }, CancellationTokenSource.Token);
        }

        private void Client_Received(object sender, ReceivedEventArgs e)
        {
            var message = e.Packet.ToStr();
            OnBroadcastReceice(new DeviceEventArgs(e.Remote, message));
        }
        public void Stop()
        {
            this.CancellationTokenSource.Cancel();
        }
    }
}
