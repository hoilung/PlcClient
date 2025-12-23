using HL.Object.Extensions;
using NewLife;
using NewLife.Http;
using NewLife.Log;
using NewLife.Serialization;
using NewLife.Xml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static NewLife.Remoting.ApiHttpClient;
namespace PlcClient.Controls
{
    public partial class CameraDeviceInfo : BaseControl
    {
        private readonly string FFMPEG = Path.Combine(CURRENT_PATH, "ffmpeg.exe");
        private readonly string FFPLAY = Path.Combine(CURRENT_PATH, "ffplay.exe");

        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly List<CameraDeviceInfoModel> _cameraDeviceInfoModels = new List<CameraDeviceInfoModel>();
        private Handler.ListViewHandler listViewHandler;//扩展排序和导出
        public CameraDeviceInfo()
        {
            InitializeComponent();
            listViewHandler = new Handler.ListViewHandler(this.listView1);
            var ps = typeof(CameraDeviceInfoModel).GetProperties();
            foreach (var p in ps)
            {
                var d = p.GetDisplayName();
                this.listView1.Columns.Add(new ColumnHeader() { Text = d, Width = d == "#" ? 35 : 100, Tag = p.Name });
            }
            tbx_type.SelectedIndex = 0;
            this.Dock = this.tableLayoutPanel1.Dock = this.listView1.Dock = DockStyle.Fill;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.listView1.VirtualMode = true;
            this.listView1.RetrieveVirtualItem += ListView1_RetrieveVirtualItem;
        }

        public void LoadData(List<string> ipaddress)
        {
            for (int i = 0; i < ipaddress.Count; i++)
            {
                this._cameraDeviceInfoModels.Add(new CameraDeviceInfoModel() { ID = i + 1, IPAddress = ipaddress[i] });
            }
            this.listView1.VirtualListSize = this._cameraDeviceInfoModels.Count;

        }

        private void ListView1_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (this._cameraDeviceInfoModels.Any() && e.ItemIndex <= this._cameraDeviceInfoModels.Count)
            {
                var item = new ListViewItem();
                if (e.ItemIndex % 2 == 0)
                {
                    item.BackColor = Color.AliceBlue;
                }
                var model = this._cameraDeviceInfoModels[e.ItemIndex];

                item.Tag = item;
                item.Text = model.ID.ToString();
                item.SubItems.Add(model.IPAddress);
                item.SubItems.Add(model.DeviceName);
                item.SubItems.Add(model.VideoScreen);
                item.SubItems.Add(model.State);
                item.SubItems.Add(model.UserName);
                item.SubItems.Add(model.Password);
                e.Item = item;
            }
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
            this.progressBar1.Maximum = this._cameraDeviceInfoModels.Count;
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
                if (!File.Exists(FFMPEG))
                {
                    MessageBox.Show("缺少工具组件，请先下载");
                    return;
                }
                VideoScreen(username, password, () =>
                {
                    this.progressBar1.PerformStep();
                });
            }
            else if (_type == "预览播放视频")
            {
                if (!File.Exists(FFPLAY))
                {
                    MessageBox.Show("缺少工具组件，请先下载");
                    return;
                }

                VideoView(username, password);
            }

        }
        private void VideoView(string username, string password)
        {
            if (this.listView1.SelectedIndices.Count > 0)
            {
                var index = this.listView1.SelectedIndices[0];
                if (index <= this._cameraDeviceInfoModels.Count)
                {
                    var device = this._cameraDeviceInfoModels[index];
                    var rtsp = $"rtsp://{username}:{password}@{device.IPAddress}/Streaming/Channels/101";
                    FFPLAY.ShellExecute($"-window_title \"{device.IPAddress}\" {rtsp}");
                }
            }
        }

        private void VideoScreen(string username, string password, Action action = null)
        {
            var dir = Path.Combine(Directory.GetCurrentDirectory(), "tmp", DateTime.Now.ToString("yyyyMMdd_hhmm"));
            Directory.CreateDirectory(dir);
            Task.Factory.StartNew(() =>
            {
                foreach (var device in this._cameraDeviceInfoModels)
                {
                    if (this.cancellationTokenSource.IsCancellationRequested) break;
                    if (action != null)
                    {
                        this.Invoke(action);
                    }
                    if (device.VideoScreen != string.Empty)
                        continue;
                    var rtsp = $"rtsp://{username}:{password}@{device.IPAddress}/Streaming/Channels/101";
                    var img = Path.Combine(dir, $"{device.IPAddress}.png");
                    var result = FFMPEG.Execute($"-i {rtsp} -vframes 1 {img}", 3000);
                    if (result == "")
                    {
                        device.VideoScreen = img;
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
                foreach (var device in this._cameraDeviceInfoModels)
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
                    var credCache = new CredentialCache();

                    var clienthandler = new HttpClientHandler();
                    clienthandler.PreAuthenticate = true;
                    clienthandler.Credentials = credCache;
                    credCache.Add(request, "Digest", new NetworkCredential(username, password));

                    var http = new HttpClient(clienthandler);
                    http.BaseAddress = request;
                    try
                    {
                        var resp = await http.GetAsync("/ISAPI/System/deviceInfo");
                        device.State = resp.StatusCode.ToString();
                        if (resp.IsSuccessStatusCode)
                        {
                            var text = await resp.Content.ReadAsStringAsync();
                            //TODO:解析xml获取设备信息
                            if (text.ToXmlDictionary().TryGetValue("deviceName", out string val))
                            {
                                device.DeviceName = val;
                                device.UserName = username;
                                device.Password = password;
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        XTrace.WriteException(ex);
                        device.State = ex.Message;
                    }
                    finally
                    {
                        http.Dispose();
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
            var dt = this._cameraDeviceInfoModels.ToDataTable();
            listViewHandler.Export(dt, "DeviceInfo");
        }
    }


    public class CameraDeviceInfoModel
    {
        [DisplayName("#")]
        public int ID { get; set; }
        [DisplayName("IPv4")]
        public string IPAddress { get; set; }
        [DisplayName("设备名称")]
        public string DeviceName { get; set; } = string.Empty;
        [DisplayName("视频截图")]
        public string VideoScreen { get; set; } = string.Empty;

        [DisplayName("状态")]
        public string State { get; set; } = "待处理";

        [DisplayName("账号")]
        public string UserName { get; set; } = string.Empty;

        [DisplayName("密码")]
        public string Password { get; set; } = string.Empty;
    }


}
