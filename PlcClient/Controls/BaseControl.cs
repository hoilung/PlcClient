using System;
using System.Diagnostics;
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
    }
}
