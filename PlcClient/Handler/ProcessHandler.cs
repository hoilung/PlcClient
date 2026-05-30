using NewLife;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace PlcClient.Handler
{
    public enum NetStatEnum
    {
        //[Description("所有")]
        //All,        
        [Description("TCP连接")]
        TCPConnect,
        [Description("TCP监听")]
        TCPListen,

    }

    internal class ProcessHandler
    {
        private static Lazy<ProcessHandler> instance = new Lazy<ProcessHandler>(() => new ProcessHandler());

        public static ProcessHandler Instance
        {
            get
            {
                return instance.Value;
            }
        }
        public bool IsAdministrator { get; set; }

        private string GetCmd(NetStatEnum netStatEnum)
        {
            switch (netStatEnum)
            {
                case NetStatEnum.TCPListen:
                    return "/c netstat -a -n -o | findstr \"LISTENING\"";
                case NetStatEnum.TCPConnect:
                    return "/c netstat -a -n -o | findstr \"TCP\"";
                default:
                    return "/c netstat -a -n -o | findstr \"LISTENING\"";
            }

        }

        private ProcessHandler()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            this.IsAdministrator = principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        //获取监听的端口号
        public IList<ProcessPortInfo> GetListenPort(NetStatEnum netStatEnum = NetStatEnum.TCPConnect)
        {
            var list = new List<ProcessPortInfo>();
            var lines = ProcessHelper.Execute("cmd", this.GetCmd(netStatEnum));

            int row = 0;            
            var pskv = new Dictionary<int, Process>();
            foreach (var line in lines.Split(Environment.NewLine))
            {
                var items = line.Split(' ').Where(e => !e.IsNullOrEmpty()).ToArray();
                if (items.Length > 4)
                {

                    var port = items[1].Split(':')[1];
                    var pid = Int32.Parse(items[4]);
                    Process process = null;

                    try
                    {
                        if (pid > 0)
                        {
                            process = Process.GetProcessById(pid);

                        }
                    }
                    catch (Exception)
                    {

                    }

                    if (process == null)
                        continue;
                    var processName = process.ProcessName;

                    var nci = new ProcessPortInfo()
                    {
                        Protocol = items[0],
                        LocalAddress = items[1],
                        RemoteAddress = items[2],
                        State = items[3],
                        ProcessId = pid,
                        ProcessName = processName,
                    };
                    row += 1;
                    nci.ID = row;
                    if (this.IsAdministrator && processName != "System")
                    {
                        try
                        {
                            nci.ProcessPath = process.MainModule.FileName;
                            nci.CreatedTime = process.StartTime.ToString();
                        }
                        catch (Win32Exception ex)
                        {
                            switch (ex.NativeErrorCode)
                            {
                                case 5:
                                    nci.Description = "权限不足 - 受保护的系统进程";
                                    break;
                                case 299:
                                    nci.Description = "无法完全访问 - 进程受保护";
                                    break;
                                default:
                                    nci.Description = $"访问路径失败: {ex.Message}";
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            nci.Description = $"访问路径失败: {ex.Message}";
                            XTrace.WriteException(ex);

                        }
                    }
                    else
                    {
                        nci.Description = "核心系统进程";
                    }
                    list.Add(nci);
                }
            }

            return list;
        }


    }
    /// <summary>
    /// 网络连接信息类，用于存储 netstat 命令的输出数据
    /// </summary>
    public class ProcessPortInfo
    {
        [DisplayName("#")]
        public int ID { get; set; }
        /// <summary>
        /// 协议类型 (TCP/UDP)
        /// </summary>
        [DisplayName("协议")]
        public string Protocol { get; set; }

        /// <summary>
        /// 本地地址和端口 (格式: IP:Port)
        /// </summary>
        [DisplayName("本地地址和端口")]
        public string LocalAddress { get; set; }

        /// <summary>
        /// 本地IP地址
        /// </summary>
        [DisplayName("本地IP地址")]
        public string LocalIP => RemoteAddress.Contains("]:") ? RemoteAddress.Split("]:")[0] + "]" : RemoteAddress.Split(':').FirstOrDefault();

        /// <summary>
        /// 本地端口号
        /// </summary>
        [DisplayName("本地端口号")]
        public int LocalPort => LocalAddress.Split(':').LastOrDefault().ToInt();

        /// <summary>
        /// 远程地址和端口 (格式: IP:Port)
        /// </summary>
        [DisplayName("远程地址和端口")]
        public string RemoteAddress { get; set; }

        /// <summary>
        /// 远程IP地址
        /// </summary>
        [DisplayName("远程IP地址")]
        public string RemoteIP => RemoteAddress.Contains("]:") ? RemoteAddress.Split("]:")[0] + "]" : RemoteAddress.Split(':').FirstOrDefault();

        /// <summary>
        /// 远程端口号
        /// </summary>
        [DisplayName("远程端口号")]
        public int RemotePort => RemoteAddress.Split(':').LastOrDefault().ToInt();

        /// <summary>
        /// 连接状态 (如: LISTENING, ESTABLISHED 等)
        /// </summary>
        [DisplayName("连接状态")]
        public string State { get; set; }

        /// <summary>
        /// 进程ID (PID)
        /// </summary>
        [DisplayName("进程ID")]
        public int ProcessId { get; set; }

        /// <summary>
        /// 进程名称
        /// </summary>
        [DisplayName("进程名称")]
        public string ProcessName { get; set; }

        /// <summary>
        /// 进程路径
        /// </summary>
        [DisplayName("进程路径")]
        public string ProcessPath { get; set; } = string.Empty;

        /// <summary>
        /// 创建时间
        /// </summary>
        [DisplayName("创建时间")]
        public string CreatedTime { get; set; } = string.Empty;

        [DisplayName("描述消息")]
        public string Description { get; set; } = string.Empty;
        public override string ToString()
        {
            return $"{Protocol} {LocalAddress} {RemoteAddress} {State} {ProcessId}";
        }
    }
}
