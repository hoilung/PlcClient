using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlcClient.Model
{
    public partial class Config
    {
        public readonly static string IPVerdify = @"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";//ip地址验证
        public readonly static string NumVerdify = @"^\d+$";//纯数组
    }

    public partial class Config
    {

    }
}
