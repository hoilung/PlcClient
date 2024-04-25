using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NewLife;
using System.Net.WebSockets;
using System.Windows.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using NewLife.Remoting;
using Newtonsoft.Json;
using System.Xml;
using System.Web.UI.WebControls;
using System.Threading;
using NewLife.Messaging;

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

    public class DeviceHandler
    {

        private CancellationTokenSource CancellationTokenSource;
        private IPEndPoint endPoint;
        private UdpClient udpClient;
        private string localIP;
        private int port;
        private string broadcastAddress;

        public event EventHandler<DeviceEventArgs> DeviceReceice;

        public DeviceHandler(string localIP, int port, string broadcastAddress)
        {
            this.localIP = localIP;
            this.port = port;
            this.broadcastAddress = broadcastAddress;
        }

        protected virtual void OnBroadcastReceice(DeviceEventArgs e)
        {
            if (DeviceReceice != null)
            {
                DeviceReceice(this, e);
            }
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
            var multicast = new IPEndPoint(IPAddress.Parse(broadcastAddress), port);
            SendMsg(message, multicast);
            SendMsg(message, multicast);
        }
        /// <summary>
        /// 海康设备发现解包
        /// </summary>
        /// <param name="message"></param>
        public T HKDeviceUnpack<T>(string message)
        {
            //读取message 序列化为 对象
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            var obj = (T)xmlSerializer.Deserialize(new StringReader(message));
            return obj;
        }

        #endregion

        #region 宇视设备查找
        /// <summary>
        /// 宇视网络设备查找
        /// 广播地址：239.255.255.250
        /// 广播地址：239.255.255.255
        /// 端口：3705->3702
        /// </summary>
        public void UniViewDeviceFind()
        {
            string message = Properties.Resources.hikvision.Replace("{uuid}", Guid.NewGuid().ToString());
            var multicast = new IPEndPoint(IPAddress.Parse(broadcastAddress), port);
            SendMsg(message, multicast);
            SendMsg(message, multicast);
        }

        #endregion

        public void Start()
        {
            this.CancellationTokenSource = new CancellationTokenSource();
            Task.Run(() =>
            {
                endPoint = new IPEndPoint(IPAddress.Parse(localIP), port);
                udpClient = new UdpClient(endPoint);
                udpClient.JoinMulticastGroup(IPAddress.Parse(broadcastAddress));
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
            this.CancellationTokenSource.Cancel();
            udpClient.Close();
        }

        private void SendMsg(string message, IPEndPoint ep)
        {
            //byte[] data = Encoding.UTF8.GetBytes(message);
            //udpClient.Send(data, data.Length, ep);
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
