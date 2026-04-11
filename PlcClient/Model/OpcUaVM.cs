using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlcClient.Model
{
    public class OpcUaVM
    {
        public int ID { get; set; }

        public string DisplayName { get; set; }

        public string Value {  get; set; }
    }
}
