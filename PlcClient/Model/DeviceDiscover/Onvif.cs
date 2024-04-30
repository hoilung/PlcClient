using NewLife;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PlcClient.Model.DeviceDiscover
{

    public class Onvif
    {
        public static string GetNonce()
        {
            byte[] nonce = new byte[16];
            new Random().NextBytes(nonce);
            return nonce.ToBase64();
        }

        public static string GetCreateString(DateTime onvifUTCDateTime)
        {
            return onvifUTCDateTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
        }

        public static string GetPasswordDigest(string nonce, string createdString, string password)
        {
            //nonce = "5WKi7zoekmBBA2AjYOHROA==";
            //created = "2024-04-30T06:08:41Z";
            //password = "xxct111111";

            var data = nonce.ToBase64().Concat(createdString.GetBytes());
            var passByte = password.GetBytes();
            if (passByte != null)
            {
                data = data.Concat(passByte);
            }
            return HashAlgorithm.Create("SHA1").ComputeHash(data.ToArray()).ToBase64();
        }

        private HttpClient client = new HttpClient();
        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<string> GetDeviceInformation(string requestUri, string username, string password)
        {
            var nonce = Onvif.GetNonce();
            var create = Onvif.GetCreateString(DateTime.UtcNow);
            var passwordDigest = Onvif.GetPasswordDigest(nonce, create, password);

            var body = Properties.Resources.onvif_device_service
                .Replace("{Username}", username)
                .Replace("{PasswordDigest}", passwordDigest)
                .Replace("{Nonce}", nonce)
                .Replace("{Created}", create);
            //.Replace("", "GetDeviceInformation");
            var content = new StringContent(body);
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// 获取设备网络信息
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<string> GetNetworkInterfaces(string requestUri, string username, string password)
        {
            var nonce = Onvif.GetNonce();
            var create = Onvif.GetCreateString(DateTime.UtcNow);
            var passwordDigest = Onvif.GetPasswordDigest(nonce, create, password);

            var body = Properties.Resources.onvif_device_service
                .Replace("{Username}", username)
                .Replace("{PasswordDigest}", passwordDigest)
                .Replace("{Nonce}", nonce)
                .Replace("{Created}", create)
                .Replace("GetDeviceInformation", "GetNetworkInterfaces");
            var content = new StringContent(body);

            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

    }

    #region 设备发现

    public class ProbeMatch
    {
        [XmlElement(ElementName = "Types", Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery")]
        public string Types { get; set; }

        [XmlElement(ElementName = "Scopes", Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery")]
        public string Scopes { get; set; }

        [XmlElement(ElementName = "XAddrs", Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery")]
        public string XAddrs { get; set; }

        [XmlElement(ElementName = "MetadataVersion", Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery")]
        public int MetadataVersion { get; set; }

        public HKProbeMatch ToHKProbeMatch()
        {
            HKProbeMatch hKProbe = new HKProbeMatch();

            return hKProbe;
        }
    }
    public class ProbeMatches
    {

        [XmlElement(ElementName = "ProbeMatch", Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery")]
        public ProbeMatch[] ProbeMatch { get; set; }
    }

    #endregion

    #region DeviceInformationResponse

    public class DeviceInformationResponse
    {

        [XmlElement(ElementName = "Manufacturer", Namespace = "http://www.onvif.org/ver10/device/wsdl")]
        public string Manufacturer { get; set; }

        [XmlElement(ElementName = "Model", Namespace = "http://www.onvif.org/ver10/device/wsdl")]
        public string Model { get; set; }

        [XmlElement(ElementName = "FirmwareVersion", Namespace = "http://www.onvif.org/ver10/device/wsdl")]
        public string FirmwareVersion { get; set; }

        [XmlElement(ElementName = "SerialNumber", Namespace = "http://www.onvif.org/ver10/device/wsdl")]
        public string SerialNumber { get; set; }

        [XmlElement(ElementName = "HardwareId", Namespace = "http://www.onvif.org/ver10/device/wsdl")]
        public double HardwareId { get; set; }
    }

    #endregion

    #region NetworkInterfacesResponse
    public class Manual
    {

        [XmlElement(ElementName = "Address", Namespace = "http://www.onvif.org/ver10/schema")]
        public string Address { get; set; }

        [XmlElement(ElementName = "PrefixLength", Namespace = "http://www.onvif.org/ver10/schema")]
        public int PrefixLength { get; set; }
    }
    public class IPv4Config
    {

        [XmlElement(ElementName = "Manual", Namespace = "http://www.onvif.org/ver10/schema")]
        public Manual Manual { get; set; }

        [XmlElement(ElementName = "DHCP", Namespace = "http://www.onvif.org/ver10/schema")]
        public bool DHCP { get; set; }
    }
    public class IPv4
    {

        [XmlElement(ElementName = "Enabled", Namespace = "http://www.onvif.org/ver10/schema")]
        public bool Enabled { get; set; }

        [XmlElement(ElementName = "Config", Namespace = "http://www.onvif.org/ver10/schema")]
        public IPv4Config Config { get; set; }
    }
    public class Info
    {

        //[XmlElement(ElementName = "Name", Namespace = "http://www.onvif.org/ver10/schema")]
        //public string Name { get; set; }

        [XmlElement(ElementName = "HwAddress", Namespace = "http://www.onvif.org/ver10/schema")]
        public string HwAddress { get; set; }

        //[XmlElement(ElementName = "MTU", Namespace = "http://www.onvif.org/ver10/schema")]
        //public int MTU { get; set; }
    }
    public class NetworkInterfaces
    {
        [XmlElement(ElementName = "Info", Namespace = "http://www.onvif.org/ver10/schema")]
        public Info Info { get; set; }

        [XmlElement(ElementName = "IPv4", Namespace = "http://www.onvif.org/ver10/schema")]
        public IPv4 IPv4 { get; set; }
    }


    public class NetworkInterfacesResponse
    {

        [XmlElement(ElementName = "NetworkInterfaces", Namespace = "http://www.onvif.org/ver10/device/wsdl")]
        public NetworkInterfaces NetworkInterfaces { get; set; }
    }



    #endregion

    #region MyRegion
    
    public class NetworkGateway
    {

        [XmlElement(ElementName = "IPv4Address", Namespace = "http://www.onvif.org/ver10/schema")]
        public string IPv4Address { get; set; }
    }
    
    public class NetworkDefaultGatewayResponse
    {

        [XmlElement(ElementName = "NetworkGateway", Namespace = "http://www.onvif.org/ver10/device/wsdl")]
        public NetworkGateway NetworkGateway { get; set; }
    }
    #endregion

    public class Body
    {
        [XmlElement(ElementName = "ProbeMatches", Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery")]
        public ProbeMatches ProbeMatches { get; set; }

        [XmlElement(ElementName = "GetDeviceInformationResponse", Namespace = "http://www.onvif.org/ver10/device/wsdl")]
        public DeviceInformationResponse DeviceInformationResponse { get; set; }

        [XmlElement(ElementName = "GetNetworkInterfacesResponse", Namespace = "http://www.onvif.org/ver10/device/wsdl")]
        public NetworkInterfacesResponse NetworkInterfacesResponse { get; set; }

        [XmlElement(ElementName = "GetNetworkDefaultGatewayResponse", Namespace = "http://www.onvif.org/ver10/device/wsdl")]
        public NetworkDefaultGatewayResponse NetworkDefaultGatewayResponse { get; set; }
    }

    [XmlRoot(ElementName = "Envelope", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
    public class Envelope
    {
        [XmlElement(ElementName = "Body", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
        public Body Body { get; set; }
      
    }
}
