using HL.Object.Extensions;
using NewLife;
using NewLife.Log;
using NewLife.Reflection;
using NewLife.Xml;
using PlcClient.Handler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace PlcClient.Controls
{
    public partial class CameraDeviceInfo : BaseControl
    {
        private readonly string FFMPEG = Path.Combine(CURRENT_PATH, "ffmpeg.exe");
        private readonly string FFPLAY = Path.Combine(CURRENT_PATH, "ffplay.exe");
        private readonly string PLAY2 = Path.Combine(CURRENT_PATH, "VideoForm.exe");

        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        //private readonly List<CameraDeviceInfoModel> _cameraDeviceInfoModels = new List<CameraDeviceInfoModel>();
        private Handler.ListViewHandler<CameraDeviceInfoModel> listViewHandler;//扩展排序和导出
        public CameraDeviceInfo()
        {
            InitializeComponent();
            listViewHandler = new Handler.ListViewHandler<CameraDeviceInfoModel>(this.listView1);
            listViewHandler.ColuminSort();
            listViewHandler.SetupVirtualMode();

            tbx_type.SelectedIndex = 0;
            this.Dock = this.tableLayoutPanel1.Dock = this.listView1.Dock = DockStyle.Fill;

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.tbx_password.Focus();

        }

        public void LoadData(List<string> ipaddress)
        {
            var _cameraDeviceInfoModels = new List<CameraDeviceInfoModel>();
            for (int i = 0; i < ipaddress.Count; i++)
            {
                _cameraDeviceInfoModels.Add(new CameraDeviceInfoModel() { ID = i + 1, IPAddress = ipaddress[i] });
            }
            //this.listView1.VirtualListSize = this._cameraDeviceInfoModels.Count;
            this.listViewHandler.LoadAdd(_cameraDeviceInfoModels);
        }

        private void btn_execute_Click(object sender, EventArgs e)
        {


            var _type = tbx_type.SelectedItem.ToString();
            var username = tbx_username.Text;
            var password = tbx_password.Text;
            if (username.IsNullOrEmpty() || password.IsNullOrEmpty())
            {

                MessageBox.Show("请输入账号或密码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            cancellationTokenSource.Cancel();
            cancellationTokenSource = new CancellationTokenSource();

            //查询设备名称
            //执行设备截图
            this.progressBar1.Maximum = this.listView1.Items.Count;
            this.progressBar1.Value = 0;
            if (_type == "查询设备名称")
            {
                ShowDeviceInfo(username, password, () =>
                {
                    this.progressBar1.PerformStep();
                });

            }
            else if (_type == "执行设备截图")
            {
                VideoScreen(username, password, () =>
                {
                    this.progressBar1.PerformStep();
                });
            }
            else if (_type == "预览播放视频")
            {

                ShowVideoView(username, password);
            }

        }

        private void DownloadPackage()
        {

            MessageBox.Show("缺少必要运行组件，请保持网络正常，等待下载完成后再使用", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            this.progressBar1.Value = 0;
            this.progressBar1.Maximum = 100;
            this.Parent.Tag = this.Parent.Text;
            var ochandler = new OpenCvHandler();
            Task.Factory.StartNew(async () =>
            {
                #region 下载运行时组件
                string savePath = Path.Combine(Directory.GetCurrentDirectory(), "tmp", "OpenCvSharp4.runtime.win.zip");

                await ochandler.DownloadAsync(ochandler.DOWNLOAD_OPENCV_PATH, savePath, (progress) =>
                {
                    this.Invoke(() =>
                    {
                        this.Parent.Text = $"正在下载运行时组件 {progress}%";
                        this.progressBar1.Value = progress;
                    });
                }, cancellationTokenSource.Token);

                this.Invoke(() =>
                {
                    this.Parent.Text = $"正在解压文件";
                });
                var tarDir = Path.Combine(Directory.GetCurrentDirectory(), "tmp", "OpenCvSharp4.runtime.win");
                ochandler.Unzip(savePath, tarDir);
                this.Invoke(() =>
                {
                    this.Parent.Text = $"正在创建目录";
                });
                var sourceDir = Path.Combine(tarDir, "runtimes");
                if (Directory.Exists(sourceDir))
                {
                    //复制目录到运行目录
                    var runtimeDir = Path.Combine(Directory.GetCurrentDirectory(), "runtimes");
                    DirectoryHelper.CopyDirectory(sourceDir, runtimeDir);
                    //删除临时文件
                    tarDir.AsDirectory().Delete(true);
                    savePath.AsFile().Delete();
                }

                this.Invoke(() =>
                {
                    this.Parent.Text = this.Parent.Tag.ToString();
                });
                #endregion


            }, cancellationTokenSource.Token);

        }

        private void ShowVideoView(string username, string password)
        {
            if (this.listView1.SelectedIndices.Count == 0)
            {
                MessageBox.Show("请在列表选中要预览的设备", "提示");
                return;
            }
            var index = this.listView1.SelectedIndices[0];
            if (index <= this.listViewHandler.DataCount)
            {
                var device = this.listViewHandler[index];
                var rtsp = $"rtsp://{username}:{password}@{device.IPAddress}/Streaming/Channels/102";
                if (device.UserName != string.Empty && device.Password != string.Empty)
                {
                    rtsp = $"rtsp://{device.UserName}:{device.Password}@{device.IPAddress}/Streaming/Channels/102";
                }

                if (File.Exists(FFPLAY))
                {
                    FFPLAY.ShellExecute($"-window_title \"{device.IPAddress}\" {rtsp}");
                }
                else if (File.Exists(PLAY2))
                {
                    PLAY2.ShellExecute(rtsp);
                }
                else
                {
                    if (!OpenCvHandler.CheckOpenCvRuntime())
                    {
                        this.DownloadPackage();
                        return;
                    }
                    var frm = new Form();
                    frm.StartPosition = FormStartPosition.CenterParent;
                    frm.Text = "视频预览 " + device.IPAddress;
                    frm.ShowIcon = false;
                    frm.Size = new Size(650, 490);
                    //frm_about.MaximizeBox = false;
                    frm.MinimizeBox = false;
                    //frm.FormBorderStyle = FormBorderStyle.FixedSingle;
                    var view = new VideoView();
                    frm.Controls.Add(view);
                    frm.Show();
                    view.ShowView(rtsp);
                }


            }

        }

        private void VideoScreen(string username, string password, Action action = null)
        {
            var dir = Path.Combine(Directory.GetCurrentDirectory(), "tmp", DateTime.Now.ToString("yyyyMMdd_hhmm"));
            Directory.CreateDirectory(dir);
            if (!File.Exists(FFMPEG))
            {
                if (!OpenCvHandler.CheckOpenCvRuntime())
                {
                    this.DownloadPackage();
                    return;
                }
            }
            Task.Factory.StartNew(() =>
            {
                foreach (var device in this.listViewHandler.Data)
                {
                    if (this.cancellationTokenSource.IsCancellationRequested) break;
                    if (action != null)
                    {
                        this.Invoke(action);
                    }
                    if (device.VideoScreen != string.Empty)
                        continue;
                    var rtsp = $"rtsp://{username}:{password}@{device.IPAddress}/Streaming/Channels/101";
                    if (device.UserName != string.Empty && device.Password != string.Empty)
                    {
                        rtsp = $"rtsp://{device.UserName}:{device.Password}@{device.IPAddress}/Streaming/Channels/101";
                    }
                    var savePath = Path.Combine(dir, $"{device.IPAddress}.png");

                    var result = string.Empty;
                    if (File.Exists(FFMPEG))
                    {
                        result = FFMPEG.Execute($"-i {rtsp} -vframes 1 {savePath}", 3000);
                        if (result != null)
                        {
                            result = savePath;
                        }
                    }
                    else
                    {
                        result = OpenCvHandler.Screenshot(rtsp, savePath);
                    }
                    if (result.Length > 0)
                    {
                        device.VideoScreen = savePath;
                        device.State = "截图完成";
                    }
                    else
                    {
                        device.State = "截图失败";
                    }

                }
                this.Invoke(() =>
                {
                    this.listView1.Invalidate();
                });
            }, cancellationTokenSource.Token);
        }

        private void ShowDeviceInfo(string username, string password, Action action = null)
        {

            Task.Factory.StartNew(async () =>
            {
                var credCache = new CredentialCache();
                var netCred = new NetworkCredential(username, password);

                var clienthandler = new HttpClientHandler()
                {
                    PreAuthenticate = true,
                    Credentials = credCache
                };
                using (var http = new HttpClient(clienthandler))
                {
                    http.Timeout = TimeSpan.FromSeconds(5);
                    foreach (var device in this.listViewHandler.Data)
                    {
                        if (this.cancellationTokenSource.IsCancellationRequested)
                        {
                            break;
                        }
                        if (action != null)
                        {
                            this.Invoke(action);
                        }
                        if (device.State == HttpStatusCode.OK.ToString()) continue;


                        var request = new Uri($"http://{device.IPAddress}");
                        credCache.Add(request, "Digest", netCred);
                        credCache.Add(request, "Basic", netCred);

                        //http.BaseAddress = request;
                        try
                        {
                            var resp = await http.GetAsync($"{request}/ISAPI/System/deviceInfo");
                            device.State = resp.StatusCode.ToString();
                            if (resp.IsSuccessStatusCode)
                            {
                                var text = await resp.Content.ReadAsStringAsync();
                                var info = text.ToXmlDictionary();
                                //TODO:解析xml获取设备信息
                                if (info.TryGetValue("deviceName", out string val))
                                {
                                    device.DeviceName = val;
                                    device.UserName = username;
                                    device.Password = password;
                                }
                                if (info.TryGetValue("deviceType", out string val2))
                                {
                                    device.DeviceType = val2;
                                }
                            }

                        }
                        catch (TaskCanceledException)
                        {
                            device.State = "请求超时";
                        }
                        catch (Exception ex)
                        {
                            XTrace.WriteException(ex);
                            device.State = ex.Message;
                        }
                    }
                }
                this.Invoke(() =>
                {
                    this.listView1.Invalidate();
                });
            }, cancellationTokenSource.Token);
        }

        private void btn_export_Click(object sender, EventArgs e)
        {
            var dt = this.listViewHandler.ToDataTable();
            listViewHandler.Export(dt, "DeviceInfo");
        }
    }


    public class CameraDeviceInfoModel
    {
        [DisplayName("#")]
        public int ID { get; set; }
        [DisplayName("设备类型")]
        public string DeviceType { get; set; } = string.Empty;
        [DisplayName("IPv4")]
        public string IPAddress { get; set; }
        [DisplayName("账号")]
        public string UserName { get; set; } = string.Empty;

        [DisplayName("密码")]
        public string Password { get; set; } = string.Empty;
        [DisplayName("状态")]
        public string State { get; set; } = "待处理";
        [DisplayName("设备名称")]
        public string DeviceName { get; set; } = string.Empty;
        [DisplayName("视频截图")]
        public string VideoScreen { get; set; } = string.Empty;
    }


}
