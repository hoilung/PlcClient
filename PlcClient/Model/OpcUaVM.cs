using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlcClient.Model
{
    public class OpcUaVM
    {
        public int ID { get; set; }

        public string DisplayName { get; set; }        

        public string NodeId { get; set; }        

        public string DataType { get; set; }

        public string Value { get; set; }

        public string StatusCode { get; set; }

        public string ServerTimestamp { get; set; }

        public string AccessLevel { get; set; }

        public string Description { get; set; }
        
        public string Tag { get;  set; }
    }
}
