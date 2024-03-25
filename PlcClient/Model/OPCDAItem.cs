using HL.OpcDa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlcClient.Model
{
    public class OPCDAItem : PointItem
    {
        public string Quality { get; set; }
        public string Time { get; set; }
    }
}
