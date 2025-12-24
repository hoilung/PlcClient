using PlcClient.Handler;
using System;
using System.Windows.Forms;

namespace PlcClient.Controls
{
    public partial class VideoView : BaseControl
    {
        private OpenCvHandler handler;
        public VideoView()
        {
            InitializeComponent();
            handler = new OpenCvHandler();
            this.pictureBox1.Dock = this.Dock = DockStyle.Fill;
            this.Disposed += VideoView_Disposed;
        }

        private void VideoView_Disposed(object sender, EventArgs e)
        {
            handler.Logout();
        }

        public void ShowView(string videoPath)
        {
            handler.Logout();
            handler.Login(videoPath);
            handler.RealPlay(pictureBox1);
        }


    }
}
