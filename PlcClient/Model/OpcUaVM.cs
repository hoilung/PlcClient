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
        public string Tag { get; set; }

        public string DisplayName { get; set; }

        public string NodeId { get; set; }

        public string DataType { get; set; }

        public string Value { get; set; }

        public string StatusCode { get; set; }

        public string ServerTimestamp { get; set; }

        public string AccessLevel { get; set; }

        public string Description { get; set; }



        public override string ToString()
        {
            var head = $"{nameof(ID)},{nameof(Tag)},{nameof(DisplayName)},{nameof(NodeId)},{nameof(DataType)},{nameof(Value)},{nameof(StatusCode)},{nameof(ServerTimestamp)},{nameof(AccessLevel)},{nameof(Description)}";
            var val = $"{ID},{Tag},{DisplayName},{NodeId},{DataType},{Value},{StatusCode},{ServerTimestamp},{AccessLevel},{Description}";
            return head + Environment.NewLine + val;
        }
    }
}
