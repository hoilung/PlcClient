using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlcClient.Model
{
    public partial class AppConfig
    {
        public readonly static string IPVerdify = @"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";//ip地址验证
        public readonly static string NumVerdify = @"^\d+$";//纯数组
    }

    public partial class AppConfig
    {
        public static AppConfig Instance { get; set; } = new AppConfig();
        private AppConfig()
        {

        }

        /// <summary>
        /// 安全码
        /// </summary>
        public readonly string SafeCode = "woyaoshiyong";
        /// <summary>
        /// 是否确认安全码
        /// </summary>
        public bool SafeConfirm { get; set; }
    }
}
