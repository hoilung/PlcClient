using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace PlcClient.Controls
{
    public partial class BaseControl : UserControl, INotifyPropertyChanged
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
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, eventArgs);
            }
        }

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
        /// <summary>
        /// 十进制转2进制
        /// 十进制转十六进制
        /// </summary>
        /// <param name="val">值类型数字</param>
        /// <param name="format">2/16</param>
        /// <returns></returns>
        public string DEC2BIN2HEX(object val, int format)
        {
            if (val == null)
                return string.Empty;
            if (val.GetType().IsValueType)
            {
                var type_code = Type.GetTypeCode(val.GetType());
                byte[] binaryBytes = null;
                switch (type_code)
                {
                    case TypeCode.Boolean:
                        binaryBytes = BitConverter.GetBytes((bool)val);
                        break;
                    case TypeCode.Char:
                        binaryBytes = BitConverter.GetBytes((char)val);
                        break;
                    case TypeCode.SByte:
                        binaryBytes = BitConverter.GetBytes((sbyte)val);
                        break;
                    case TypeCode.Byte:
                        binaryBytes = BitConverter.GetBytes((byte)val);
                        break;
                    case TypeCode.Int16:
                        binaryBytes = BitConverter.GetBytes((Int16)val);
                        break;
                    case TypeCode.UInt16:
                        binaryBytes = BitConverter.GetBytes((UInt16)val);
                        break;
                    case TypeCode.Int32:
                        binaryBytes = BitConverter.GetBytes((Int32)val);
                        break;
                    case TypeCode.UInt32:
                        binaryBytes = BitConverter.GetBytes((UInt32)val);
                        break;
                    case TypeCode.Int64:
                        binaryBytes = BitConverter.GetBytes((Int64)val);
                        break;
                    case TypeCode.UInt64:
                        binaryBytes = BitConverter.GetBytes((UInt32)val);
                        break;
                    case TypeCode.Single:
                        binaryBytes = BitConverter.GetBytes((float)val);
                        break;
                    case TypeCode.Double:
                        binaryBytes = BitConverter.GetBytes((double)val);
                        break;
                }
                if (binaryBytes == null)
                    return string.Empty;
                if (format == 2)
                {
                    return string.Join(" ", binaryBytes.Reverse().Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
                }
                else if (format == 16)
                {
                    return string.Join(" ", binaryBytes.Reverse().Select(b => b.ToString("X2")));
                }

            }
            return string.Empty;
        }
    }
}
