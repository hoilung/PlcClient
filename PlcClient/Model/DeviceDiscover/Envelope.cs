using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PlcClient.Model.DeviceDiscover
{

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


    public class Body
    {
        [XmlElement(ElementName = "ProbeMatches", Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery")]
        public ProbeMatches ProbeMatches { get; set; }
    }

    [XmlRoot(ElementName = "Envelope", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
    public class Envelope
    {
        [XmlElement(ElementName = "Body", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
        public Body Body { get; set; }
    }
}
