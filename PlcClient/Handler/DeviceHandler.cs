using NewLife;
using Newtonsoft.Json;
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

    public class DeviceHandler : Onvif
    {

        private CancellationTokenSource CancellationTokenSource;
        private IPEndPoint endPoint;
        private UdpClient udpClient;
        private string localIP;
        

        public event EventHandler<DeviceEventArgs> DeviceReceice;

        public DeviceHandler(string localIP)
        {
            this.localIP = localIP;
        }

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

        #region 海康设备发现

        /// <summary>
        /// 海康设备查找
        /// 广播地址：239.255.255.250
        /// 广播地址：239.255.255.250
        /// 端口：37020
        /// </summary>
        /// <param name="localIp">当前网段ip</param>
        public void HKDeviceFind()
        {
            // 发送广播消息
            string message = Properties.Resources.hikvision.Replace("{uuid}", Guid.NewGuid().ToString());
            var multicast = new IPEndPoint(IPAddress.Parse("239.255.255.250"), 37020);//自有协议
            SendMsg(message, multicast);
            SendMsg(message, multicast);
        }

        #endregion

        #region Onvif设备查找
        /// <summary>
        /// 网络设备查找(基于onvif协议)
        /// 广播地址：239.255.255.250
        /// 广播地址：239.255.255.255
        /// 端口：3702
        /// </summary>
        public void OnvifDeviceFind()
        {
            string message = Properties.Resources.onvif.Replace("{uuid}", Guid.NewGuid().ToString());
            var multicast = new IPEndPoint(IPAddress.Parse("239.255.255.250"), 3702);//Onvif协议
            var multicast2 = new IPEndPoint(IPAddress.Parse("239.255.255.255"), 3702);
            SendMsg(message, multicast);
            SendMsg(message, multicast2);
        }

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

        #region 大华设备发现
        /// <summary>
        /// 大华网络设备发现
        /// 广播地址：239.255.255.251
        /// 端口：37810，已激活的
        /// 端口：5050，可能是未激活的，暂无测试
        /// </summary>
        public void DaHuaDeviceFind()
        {
            // hex
            //20000000444849500000000000000000490000000000000049000000000000007b20226d6574686f6422203a20224448446973636f7665722e736561726368222c2022706172616d7322203a207b20226d616322203a2022222c2022756e6922203a2031207d207d0a
            //    DHIP        I       I       { "method" : "DHDiscover.search", "params" : { "mac" : "", "uni" : 1 } }
            //
            //var hex = Properties.Resources.DaHua
            //var msg_by = new List<byte>();
            //for (int i = 0; i < hex.Length; i += 2)
            //{
            //    var b = Convert.ToByte(hex[i].ToString() + hex[i + 1].ToString(), 16);
            //    msg_by.Add(b);
            //}
            var base64 = Properties.Resources.DaHua.ToBase64();
            var message = Encoding.UTF8.GetString(base64);
            var multicast = new IPEndPoint(IPAddress.Parse("239.255.255.251"), 37810);//自有协议
            SendMsg(message, multicast);
            SendMsg(message, multicast);
        }

        public HKProbeMatch DaHuaUnpack(string message)
        {
            var jsonStr = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(message).Skip(32).ToArray());

            DHDevInfoEntity devInfoEntity = JsonConvert.DeserializeObject<DHDevInfoEntity>(jsonStr);

            return devInfoEntity.ToHKProbeMatch();
        }

        #endregion

        public void Start()
        {
            this.CancellationTokenSource = new CancellationTokenSource();
            Task.Run(() =>
            {
                endPoint = new IPEndPoint(IPAddress.Parse(localIP), 0);
                udpClient = new UdpClient(endPoint);
                //udpClient.JoinMulticastGroup(IPAddress.Parse("239.255.255.250"));
                //udpClient.JoinMulticastGroup(IPAddress.Parse("239.255.255.251"));
                //udpClient.JoinMulticastGroup(IPAddress.Parse("239.255.255.255"));
                while (!CancellationTokenSource.IsCancellationRequested)
                {
                    var ar = udpClient.BeginReceive(new AsyncCallback(ReceiveBroadcast), null);
                    ar.AsyncWaitHandle.WaitOne();
                }
            }, CancellationTokenSource.Token);
            OnSendProcess();
        }

        private void ReceiveBroadcast(IAsyncResult ar)
        {
            if (CancellationTokenSource.IsCancellationRequested)
                return;
            //var udpClient = (UdpClient)ar.AsyncState;
            //if (udpClient != null)
            //{
            var remoteEP = new IPEndPoint(IPAddress.Any, 0);
            // 获取接收到的数据包
            var receivedData = udpClient.EndReceive(ar, ref remoteEP);
            // 解码数据包以提取消息内容
            string message = Encoding.UTF8.GetString(receivedData);
            // 处理消息内容
            OnBroadcastReceice(new DeviceEventArgs(remoteEP, message));
            //}
        }

        public void Stop()
        {
            _sendQueue.Clear();
            this.CancellationTokenSource.Cancel();
            udpClient.Close();
        }

        private void SendMsg(string message, IPEndPoint ep)
        {
            _sendQueue.Enqueue(new KeyValuePair<IPEndPoint, string>(ep, message));//将消息加入队列(发送消息))
        }

        private Queue<KeyValuePair<IPEndPoint, string>> _sendQueue = new Queue<KeyValuePair<IPEndPoint, string>>();
        protected virtual void OnSendProcess()
        {
            Task.Run(async () =>
            {
                while (!this.CancellationTokenSource.IsCancellationRequested)
                {
                    if (this._sendQueue.Count == 0 || this.udpClient == null)
                    {
                        await Task.Delay(100);
                        continue;
                    }

                    var kv = this._sendQueue.Dequeue();
                    udpClient.Send(Encoding.UTF8.GetBytes(kv.Value), kv.Value.Length, kv.Key);

                }
            }, this.CancellationTokenSource.Token);
        }




    }
}
