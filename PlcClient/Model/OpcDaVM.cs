using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlcClient.Model
{
    public class OpcDaVM
    {
        public int ID { get; set; }

        public string Tag { get; set; }

        public string DataType { get; set; }
        public string Value { get; set; }
        public string Quality { get; set; }
        public string Time { get; set; }
    }
}
