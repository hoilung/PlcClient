using NewLife;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
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
                    else if (processName== "System")
                    {
                        nci.Description = "核心系统进程";
                    }
                    list.Add(nci);
                }
            }

            return list;
        }
        public List<ProcessPortInfo> GetTcpConnectionsUsingNetworkInfo(NetStatEnum netStatEnum = NetStatEnum.TCPConnect)
        {
            var connections = new List<ProcessPortInfo>();
            int id = 0;

            try
            {
                var properties = IPGlobalProperties.GetIPGlobalProperties();

                if (netStatEnum == NetStatEnum.TCPConnect)
                {
                    // 获取活动的 TCP 连接
                    var tcpConnections = properties.GetActiveTcpConnections();

                    foreach (var conn in tcpConnections)
                    {
                        var connectionInfo = new ProcessPortInfo
                        {
                            ID = ++id,
                            Protocol = "TCP",
                            LocalAddress = $"{conn.LocalEndPoint.Address}:{conn.LocalEndPoint.Port}",
                            RemoteAddress = $"{conn.RemoteEndPoint?.Address?.ToString() ?? "0.0.0.0"}:{conn.RemoteEndPoint?.Port ?? 0}",
                            State = conn.State.ToString(),
                            ProcessId = 0, // NetworkInformation 不提供 PID 信息
                            ProcessName = "N/A", // 不提供进程信息
                            ProcessPath = "N/A", // 不提供进程路径
                            CreatedTime = "N/A", // 不提供创建时间
                            Description = "非管理员运行显示进程信息"
                        };

                        connections.Add(connectionInfo);
                    }
                }

                if (netStatEnum == NetStatEnum.TCPListen || netStatEnum == NetStatEnum.TCPConnect)
                {
                    // 获取监听的 TCP 端口
                    var listeners = properties.GetActiveTcpListeners();

                    foreach (var listener in listeners)
                    {
                        var listenerInfo = new ProcessPortInfo
                        {
                            ID = ++id,
                            Protocol = "TCP",
                            LocalAddress = $"{listener.Address}:{listener.Port}",
                            RemoteAddress = "0.0.0.0:0", // 监听端口没有远程地址
                            State = "LISTENING",
                            ProcessId = 0, // NetworkInformation 不提供 PID 信息
                            ProcessName = "N/A", // 不提供进程信息
                            ProcessPath = "N/A", // 不提供进程路径
                            CreatedTime = "N/A", // 不提供创建时间
                            Description = "非管理员运行不显示进程信息"
                        };

                        connections.Add(listenerInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                // 返回空列表或记录错误信息
                var errorInfo = new ProcessPortInfo
                {
                    ID = ++id,
                    Protocol = "ERROR",
                    LocalAddress = "N/A",
                    RemoteAddress = "N/A",
                    State = "ERROR",
                    ProcessId = 0,
                    ProcessName = "API Error",
                    ProcessPath = "N/A",
                    CreatedTime = "N/A",
                    Description = $"获取网络连接信息失败: {ex.Message}"
                };

                connections.Add(errorInfo);
            }

            return connections;
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
