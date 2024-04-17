using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NewLife;
using System.Net.WebSockets;

namespace PlcClient.Handler
{
    internal class DeviceHandler
    {
        public void Test()
        {
            IPAddress broadcastAddress = IPAddress.Broadcast;// IPAddress.Parse("239.255.255.250"); // 你的局域网广播地址//239.255.255.251 大华 37810
            int broadcastPort = 12344; // 选择一个空闲的端口

            // 创建UDP客户端
            UdpClient udpClient = new UdpClient();

            try
            {
                

                // 构造要发送的消息
                string message = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Probe><Uuid>3C467D7F-9996-473F-A541-26702C4DCD51</Uuid><Types>inquiry</Types></Probe>";
                byte[] bytes = Encoding.ASCII.GetBytes(message);

                // 发送广播消息
                udpClient.Send(bytes, bytes.Length, new IPEndPoint(broadcastAddress, broadcastPort));

                // 接收来自设备的响应
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                byte[] responseBytes = udpClient.Receive(ref remoteEP);
                string responseMessage = Encoding.ASCII.GetString(responseBytes);

                // 显示响应
                Console.WriteLine("Received response from: " + remoteEP.Address.ToString());
                Console.WriteLine("Response message: " + responseMessage);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            finally
            {
                udpClient.Close();
            }
        }
    }
}
