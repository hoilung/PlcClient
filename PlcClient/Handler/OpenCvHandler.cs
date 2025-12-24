using NewLife.Log;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace PlcClient.Handler
{
    internal class OpenCvHandler
    {
        private VideoCapture _videoCapture;
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
            }
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int _sleepTime = (int)(1000 / _videoCapture.Get(VideoCaptureProperties.Fps));
            using (Mat _frame = new Mat())
            {
                while (!this._backgroundWorker.CancellationPending)
                {
                    if (this._videoCapture.Read(_frame))
                    {
                        this._backgroundWorker.ReportProgress(0, _frame);
                    }
                    Thread.Sleep(_sleepTime);
                }
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
}
