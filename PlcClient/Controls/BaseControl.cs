using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace PlcClient.Controls
{
    public partial class BaseControl : UserControl
    {
        public BaseControl()
        {
            InitializeComponent();
        }
        public Ping ping = new Ping();
        public Stopwatch stopwatch = Stopwatch.StartNew();
        public virtual TypeCode[] TypeCodes { get; set; } = new[] {
            TypeCode.Boolean,
            TypeCode.Byte,
            TypeCode.Int16,
            TypeCode.Int32,
            //TypeCode.Int64,
            TypeCode.Single,
            TypeCode.Double,
            TypeCode.UInt16,
            TypeCode.UInt32,
            //TypeCode.UInt64
        };
        #region event
        public event Action<string> Msg;

        protected virtual void OnMsg(string msg)
        {
            this.Msg?.Invoke(msg);
        }
        #endregion

        public TControl[] FindControls<TControl>(Control baseControl, bool searchAllChildren) where TControl : Control
        {
            var list = new List<TControl>();
            if (baseControl == null)
                return list.ToArray();

            for (int i = 0; i < baseControl.Controls.Count; i++)
            {
                var item = baseControl.Controls[i];
                if (item.GetType() == typeof(TControl))
                {
                    list.Add(item as TControl);
                }
                if (searchAllChildren && item.Controls != null)
                {
                    list.AddRange(FindControls<TControl>(item, searchAllChildren));
                }
            }
            return list.ToArray();
        }
        private static IPAddress[] IPS = null;
        public string[] GetLocalAllIP()
        {
            if (IPS == null)
            {
                IPS = Dns.GetHostAddresses(Dns.GetHostName()).ToArray();
            }
            return IPS.Where(m => m.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).Select(m => m.ToString()).OrderByDescending(m => m).ToArray();

        }
        public string GetLocalIP()
        {
            return GetLocalAllIP().FirstOrDefault();
        }

        public static void Delay(int milliSecond)
        {
            int start = Environment.TickCount;
            while (Math.Abs(Environment.TickCount - start) < milliSecond)//毫秒
            {
                Application.DoEvents();//可执行某无聊的操作
            }
        }

        public DialogResult ShowAbout()
        {
            var frm_about = new Form();
            frm_about.StartPosition = FormStartPosition.CenterParent;
            frm_about.Text = "关于";
            frm_about.ShowIcon = false;
            frm_about.Size = new Size(470, 300);
            frm_about.MaximizeBox = false;
            frm_about.MinimizeBox = false;
            frm_about.FormBorderStyle = FormBorderStyle.FixedSingle;

            PLCSafeConfirm pLCSafeConfirm = new PLCSafeConfirm();
            pLCSafeConfirm.Dock = DockStyle.Fill;

            frm_about.Controls.Add(pLCSafeConfirm);
            return frm_about.ShowDialog(this);
        }
    }
}
