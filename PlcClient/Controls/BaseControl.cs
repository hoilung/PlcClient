using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlcClient.Controls
{
    public partial class BaseControl : UserControl
    {
        public BaseControl()
        {
            InitializeComponent();
        }

        #region event
        public event Action<string> Msg;

        protected virtual void OnMsg(string msg)
        {
            this.Msg?.Invoke(msg);
        }
        #endregion
    }
}
