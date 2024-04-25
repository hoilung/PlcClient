using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlcClient.Model
{
    public class DisplayCustomAttribute : Attribute
    {
        public DisplayCustomAttribute(string name)
        {
            this.Name = name;
        }

        public string DataMember { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = "N/A";
        public int Order { get; set; }

        public string Category { get; set; } = string.Empty;
    }
}
