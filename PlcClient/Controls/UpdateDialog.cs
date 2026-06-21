using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpCompress.Archives;
using NewLife.Log;

namespace PlcClient.Controls
{
    public partial class UpdateDialog : UserControl
    {

        public UpdateDialog()
        {
            InitializeComponent();
            this.tableLayoutPanel1.Dock = rtb_body.Dock = DockStyle.Fill;

        }
        private readonly string api = "https://api.github.com/repos/hoilung/plcclient/releases/latest";

        public async Task<GithubRelease> GetReleaseAsync()
        {
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("PlcClient", "1.0.0")); //设置请求头
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")); //设置请求头                
                    HttpResponseMessage response = await client.GetAsync(api);
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<GithubRelease>(json);
                        return data;
                    }
                }
            }
            return null;
        }
        public bool IsUpdateAvailable(GithubRelease latestRelease)
        {
            var current = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            return Version.Parse(latestRelease.tag_name.TrimStart('v')) > current;
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            if (btn_update.Text == "下载更新" && btn_update.Tag != null && btn_update.Tag is GithubReleaseAsset asset)
            {
                // 禁用按钮以防止多次点击
                btn_update.Enabled = false; // 禁用按钮以防止多次点击
                rtb_body.Text = "正在下载更新..."; // 更新UI线程
                // 在后台线程中下载文件
                Task.Run(async () =>
                {
                    // 下载文件
                    string filePath = System.IO.Path.Combine(Application.StartupPath, "download", asset.name);
                    string downloadDir = System.IO.Path.Combine(Application.StartupPath, "download");
                    // 创建文件夹
                    if (!System.IO.Directory.Exists(downloadDir))
                    {
                        System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Application.StartupPath, "download"));
                    }
                    //清空文件夹中的旧文件
                    if (System.IO.Directory.Exists(downloadDir))
                    {
                        foreach (var file in System.IO.Directory.GetFiles(downloadDir))
                        {
                            System.IO.File.Delete(file);
                        }
                    }
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("PlcClient", "1.0.0")); //设置请求头
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/octet-stream")); //设置请求头
                        //设置超时（连接超时，不影响大文件下载）
                        client.Timeout = TimeSpan.FromMinutes(5); // 下载超时时间设为5分钟
                        var cancelToken = new System.Threading.CancellationTokenSource();
                        try
                        {
                            // 如果下载失败，尝试使用ghproxy.net代理               
                            var requestMsg = new HttpRequestMessage(HttpMethod.Get, string.Format("https://ghproxy.net/{0}", asset.browser_download_url)); 
                            var response = await client.SendAsync(requestMsg, HttpCompletionOption.ResponseHeadersRead, cancelToken.Token);
                            if (!response.IsSuccessStatusCode)
                            {

                                requestMsg = new HttpRequestMessage(HttpMethod.Get, asset.browser_download_url);
                                response = await client.SendAsync(requestMsg, HttpCompletionOption.ResponseHeadersRead, cancelToken.Token);
                            }
                            if (response.IsSuccessStatusCode)
                            {
                                var totalBytes = response.Content.Headers.ContentLength ?? 0;
                                byte[] buffer = new byte[8192];
                                long downloadedBytes = 0;
                                int bytesRead;
                                int lastPercent = -1; // 节流：只有百分比变化时才更新UI

                                using (var fs = new System.IO.FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                                using (var stream = await response.Content.ReadAsStreamAsync())
                                {
                                    while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancelToken.Token)) > 0)
                                    {
                                        await fs.WriteAsync(buffer, 0, bytesRead, cancelToken.Token);
                                        downloadedBytes += bytesRead;

                                        if (totalBytes > 0)
                                        {
                                            int percent = (int)((downloadedBytes * 100) / totalBytes);
                                            if (percent != lastPercent)
                                            {
                                                lastPercent = percent;
                                                this.Invoke(() =>
                                                {
                                                    rtb_body.Text = $"下载中... {percent}% ({FormatBytes(downloadedBytes)}/{FormatBytes(totalBytes)})";
                                                });
                                            }
                                        }
                                        else
                                        {
                                            this.Invoke(() =>
                                            {
                                                rtb_body.Text = $"下载中... {FormatBytes(downloadedBytes)}";
                                            });
                                        }
                                    }
                                }
                            }
                            else
                            {
                                this.Invoke(() =>
                                {
                                    rtb_body.Text = "下载失败";
                                    rtb_body.AppendText(asset.browser_download_url);
                                    btn_update.Enabled = true;
                                });
                                return;
                            }
                        }
                       
                        catch (Exception ex)
                        {
                            XTrace.WriteException(ex);
                            this.Invoke(() =>
                            {
                                rtb_body.Text = "下载失败: " + ex.Message;
                                rtb_body.AppendText(asset.browser_download_url);
                                btn_update.Enabled = true;
                            });
                            return;
                        }
                    }
                    // 更新UI线程
                    this.Invoke(() =>
                    {
                        rtb_body.Text = "下载完成，正在解压安装...";
                    });
                    try
                    {
                        //// 生成安装脚本bat文件，结束当前进程，替换文件，启动软件
                        System.IO.File.WriteAllText(downloadDir + "\\install.bat", $"taskkill /f /im {System.Diagnostics.Process.GetCurrentProcess().ProcessName}.exe\n" +
                            //$"del /f /q {Application.StartupPath}\\PlcClient.exe\n" +
                            $"ren {Application.StartupPath}\\PlcClient.exe PlcClient.exe.bak\n" +
                            $"copy {downloadDir}\\PlcClient.exe {Application.StartupPath}\\PlcClient.exe\n" +
                            $"start {Application.StartupPath}\\PlcClient.exe\n" +
                            $"exit\n");

                        using (var archive=ArchiveFactory.OpenArchive(filePath))
                        {
                            foreach (var entry in archive.Entries)
                            {
                                if(!entry.IsDirectory)
                                {
                                    entry.WriteToDirectory(downloadDir, new SharpCompress.Common.ExtractionOptions()
                                    {
                                        ExtractFullPath = true,
                                        Overwrite = true
                                    });
                                }
                            }
                        }
                        // 以管理员身份运行安装脚本
                        System.Diagnostics.Process.Start(downloadDir + "\\install.bat", "/runas");
                    }
                    catch (Exception ex)
                    {
                        XTrace.WriteException(ex);
                        this.Invoke(() =>
                        {
                            rtb_body.Text = "操作失败，请手动解压覆盖文件";
                            rtb_body.AppendText(filePath);
                        });
                        return;

                    }                                      
                });
                return;
            }

            btn_update.Enabled = false; // 禁用按钮以防止多次点击
            rtb_body.Text = "正在检查更新...";
            // 在后台线程中获取最新版本信息
            Task.Run(async () =>
            {
                GithubRelease latestRelease = await GetReleaseAsync();
                // 更新UI线程
                this.Invoke(() =>
                {
                    if (latestRelease != null)
                    {
                        if (!IsUpdateAvailable(latestRelease) && latestRelease.assets.Length > 0 && !string.IsNullOrEmpty(latestRelease.assets[0].browser_download_url))
                        {
                            rtb_body.Text = latestRelease.body;
                            btn_update.Enabled = true;
                            btn_update.Text = "下载更新"; // 更改按钮文本为“更新”以提示用户点击更新
                            btn_update.Tag = latestRelease.assets[0];
                        }
                        else
                        {
                            rtb_body.Text = "当前已经是最新版本";
                            btn_update.Enabled = false;
                        }
                    }
                    else
                    {
                        rtb_body.Text = "无法获取最新版本信息";
                        btn_update.Enabled = false;
                    }
                });
            });
        }

        /// <summary>
        /// 将字节数格式化为人类可读的字符串
        /// </summary>
        private string FormatBytes(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double size = bytes;
            int order = 0;
            while (size >= 1024 && order < sizes.Length - 1)
            {
                size /= 1024;
                order++;
            }
            return $"{size:F1} {sizes[order]}";
        }
    }

    public class GithubRelease
    {
        public string tag_name { get; set; } = "1.0.0";
        public string published_at { get; set; }
        public GithubReleaseAsset[] assets { get; set; }

        public string body { get; set; }

    }
    public class GithubReleaseAsset
    {
        public string name { get; set; }
        public string browser_download_url { get; set; } = string.Empty;
    }
}
