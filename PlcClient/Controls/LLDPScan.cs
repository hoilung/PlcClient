using NewLife.Log;
using PlcClient.Handler;
using PlcClient.Model;
using SharpPcap.LibPcap;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlcClient.Controls
{

    public partial class LLDPScan : BaseControl
    {

        ListViewHandler<LLDPDeviceVM> listViewHandler;
        public LLDPScan()
        {
            InitializeComponent();
            this.listViewEx1.Dock = this.tableLayoutPanel1.Dock = groupBox1.Dock = DockStyle.Fill;
            listViewHandler = new ListViewHandler<LLDPDeviceVM>(this.listViewEx1);
            listViewHandler.SetupVirtualMode();
            listViewHandler.listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            this.cbx_ip.Items.AddRange(this.GetDeviceList());
            if (cbx_ip.Items.Count > 0)
                cbx_ip.SelectedIndex = 0;
        }
        private string[] GetDeviceList()
        {
            try
            {
                this.cbx_ip.Tag = "ip";
                var devlist = LibPcapLiveDeviceList.Instance.Select(m => m.Description);
                this.cbx_ip.ComboBox.Width = 300;
                this.cbx_ip.ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                this.cbx_ip.Tag = "device";
                return devlist.ToArray();
            }
            catch (System.Exception ex)
            {
                NewLife.Log.XTrace.WriteException(ex);
            }

            return GetLocalAllIP();
        }
        private CancellationTokenSource cancellationTokenSource;
        private void btn_start_Click(object sender, EventArgs e)
        {
            if (cbx_ip.Tag.ToString() == "ip")
            {
                try
                {
                    Clipboard.SetText(DeviceDiscovery.DOWNLOAD_NPCAP_PATH);
                    if (MessageBox.Show($"1. 无法加载网卡缺少必要的组件，请先下载并且安装后使用\r\n2. 已经复制下载地址到剪贴板，是否自动下载？否则请手动浏览器下载", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        Task.Run(async () =>
                        {                            
                            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "tmp", $"pncap_{DateTimeOffset.Now.ToUnixTimeSeconds()}.exe");
                            if (!File.Exists(savePath))
                            {
                                await new OpenCvHandler().DownloadAsync(DeviceDiscovery.DOWNLOAD_NPCAP_PATH, savePath, (progress) =>
                                {
                                    this.Invoke(() =>
                                    {
                                        this.OnMsg($"下载网络组件 {progress}%");
                                    });
                                });
                            }
                            this.Invoke(() =>
                            {
                                this.OnMsg("下载网络组件完成，尝试安装");
                            });
                            //#管理员方式执行软件
                            if (File.Exists(savePath))
                            {
                                var process = new Process();
                                process.StartInfo.FileName = savePath;
                                //process.StartInfo.Arguments = "/S"; // 静默安装参数
                                process.StartInfo.Verb = "runas";   // 请求管理员权限！

                                process.Start();
                                process.WaitForExit(); // 等待安装完成

                                this.OnMsg("Npcap 安装完成。为了使驱动生效，请重新启动本程序。");                                
                            }                            
                        });
                    }
                }
                catch (Exception ex)
                {
                    XTrace.WriteException(ex);
                    MessageBox.Show("下载安装或运行网络组件失败，请手动下载或安装!");
                }
                return;
            }
            if (cancellationTokenSource != null && !cancellationTokenSource.IsCancellationRequested)
            {
                cancellationTokenSource.Cancel();
                return;
            }
            cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Token.Register(() =>
            {
                this.Invoke(() =>
                {
                    btn_start.Text = "开始查找";
                });
            });
            var deviceName = cbx_ip.Text;
            btn_start.Text = "取消查找";
            Task.Run(async () =>
            {

                await DeviceDiscovery.FindDevicesLLDP(deviceName, (LLDPDevice) =>
                {
                    var vm = LLDPDeviceVM.Form(LLDPDevice);
                    this.Invoke(() =>
                    {
                        vm.ID = listViewHandler.DataCount;
                        listViewHandler.Add(vm);
                    });
                }, cancellationTokenSource.Token);

            });
        }

        private void btn_export_Click(object sender, EventArgs e)
        {
            listViewHandler.ExportExcel("LLDP");
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            listViewHandler.Clear();
        }
    }
}
