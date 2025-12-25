using NewLife.Log;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlcClient.Handler
{
    internal partial class OpenCvHandler
    {
        public readonly string DOWNLOAD_OPENCV_PATH = "https://www.nuget.org/api/v2/package/OpenCvSharp4.runtime.win/4.11.0.20250507";
        private VideoCapture _videoCapture;
        private Mat _currentFrame;
        private string _filename;
        private PictureBox _pictureBox;
        private BackgroundWorker _backgroundWorker;
        public OpenCvHandler()
        {
            this._backgroundWorker = new BackgroundWorker();
            this._backgroundWorker.WorkerSupportsCancellation = true;
            this._backgroundWorker.WorkerReportsProgress = true;
            this._backgroundWorker.DoWork += this.BackgroundWorker_DoWork;
            this._backgroundWorker.RunWorkerCompleted += this.BackgroundWorker_RunWorkerCompleted;
            this._backgroundWorker.ProgressChanged += _backgroundWorker_ProgressChanged;

        }

        private void _backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState is Mat mat)
            {
                if (this._pictureBox.Image != null)
                    this._pictureBox.Image.Dispose();
                this._pictureBox.Image = mat.ToBitmap();
            }
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (this._videoCapture != null)
            {
                this._videoCapture.Release();
                this._currentFrame.Release();
            }
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int _sleepTime = (int)(1000 / _videoCapture.Get(VideoCaptureProperties.Fps));
            _currentFrame = new Mat();

            while (!this._backgroundWorker.CancellationPending)
            {
                if (this._videoCapture.Read(_currentFrame))
                {
                    this._backgroundWorker.ReportProgress(0, _currentFrame);
                }
                Thread.Sleep(_sleepTime);
            }
            e.Cancel = true;
        }

        public void Login(string filename)
        {
            this._filename = filename;

        }
        public void Logout()
        {
            this.StopRealPlay();
        }
        public void RealPlay(PictureBox pictureBox)
        {
            if (this._backgroundWorker.IsBusy)
            {
                this.StopRealPlay();
            }
            this._videoCapture = new VideoCapture();
            if (!this._videoCapture.Open(this._filename))
            {
                return;
            }
            this._pictureBox = pictureBox;
            this._pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            //this._videoCapture.Set(VideoCaptureProperties.FrameWidth,pictureBox.Width);
            //this._videoCapture.Set(VideoCaptureProperties.FrameHeight, pictureBox.Height);
            this._backgroundWorker.RunWorkerAsync();

        }
        public void StopRealPlay()
        {
            this._backgroundWorker.CancelAsync();
        }

        public static string Screenshot(string videoPath, string savePath)
        {
            var rst = string.Empty;
            try
            {
                using (var capture = new VideoCapture(videoPath))
                {
                    using (Mat frame = new Mat())
                    {
                        if (capture.Read(frame))
                        {
                            Cv2.ImWrite(savePath, frame);
                            capture.Release();
                            rst = savePath;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                XTrace.WriteException(ex);
            }
            return rst;
        }


    }

    internal partial class OpenCvHandler
    {
        public static bool CheckOpenCvRuntime()
        {
            var filename = Path.Combine(Directory.GetCurrentDirectory(), "runtimes", "win-x86", "native", "OpenCvSharpExtern.dll");
            var filename2 = Path.Combine(Directory.GetCurrentDirectory(), "runtimes", "win-x86", "native", "opencv_videoio_ffmpeg4110.dll");
            if (Environment.Is64BitProcess)
            {
                filename = Path.Combine(Directory.GetCurrentDirectory(), "runtimes", "win-x64", "native", "OpenCvSharpExtern.dll");
                filename2 = Path.Combine(Directory.GetCurrentDirectory(), "runtimes", "win-x64", "native", "opencv_videoio_ffmpeg4110_64.dll");
            }            
            if (File.Exists(filename) && File.Exists(filename2))
            {
                var dlldir = Path.GetDirectoryName(filename);
                if (!OpenCvSharp.Internal.WindowsLibraryLoader.Instance.AdditionalPaths.Contains(dlldir))
                {
                    OpenCvSharp.Internal.WindowsLibraryLoader.Instance.AdditionalPaths.Add(dlldir);
                    
                }
                //if(!OpenCvSharp.Internal.WindowsLibraryLoader.Instance.IsLibraryLoaded("OpenCvSharpExtern"))
                //{
                //    OpenCvSharp.Internal.WindowsLibraryLoader.Instance.LoadLibrary("OpenCvSharpExtern");
                //}
                return true;
            }
            return false;
        }


        //下载zip文件
        public async Task<string> DownloadAsync(string url, string savePath, Action<int> progress = null, CancellationToken cancellationToken = default)
        {
            var tardir = Path.GetDirectoryName(savePath);
            if (!Directory.Exists(tardir))
            {
                Directory.CreateDirectory(tardir);
            }
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    using (HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
                    {
                        response.EnsureSuccessStatusCode();

                        var totalBytes = response.Content.Headers.ContentLength ?? 0;

                        using (FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                        {
                            var buffer = new byte[8192];
                            var bytesRead = 0;
                            long totalBytesRead = 0;

                            using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                            {
                                while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
                                {
                                    await fs.WriteAsync(buffer, 0, bytesRead, cancellationToken);
                                    totalBytesRead += bytesRead;
                                    int progressPercentage = (int)((totalBytesRead * 100) / totalBytes);
                                    progress?.Invoke(progressPercentage);
                                }
                            }
                        }

                        Console.WriteLine("文件下载成功，保存路径：" + savePath);
                        return savePath;
                    }
                }
                catch (OperationCanceledException)
                {
                    XTrace.WriteException(new Exception("下载操作被取消"));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("文件下载失败：" + ex.Message);
                    XTrace.WriteException(ex);
                }
            }
            return string.Empty;
        }

        //解压zip压缩包
        public void Unzip(string zipFile, string targetDir)
        {
            try
            {
                if (File.Exists(zipFile))
                {
                    if (!Directory.Exists(targetDir))
                    {
                        Directory.CreateDirectory(targetDir);
                    }

                    System.IO.Compression.ZipFile.ExtractToDirectory(zipFile, targetDir);
                    Console.WriteLine("文件解压成功，目标路径：" + targetDir);
                }
                else
                {
                    Console.WriteLine("指定的ZIP文件不存在：" + zipFile);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("文件解压失败：" + ex.Message);
            }
        }
    }
}
