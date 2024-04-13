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
    public partial class NetView : BaseControl
    {
        public NetView()
        {
            InitializeComponent();
            this.netClient1.Msg += NetClient1_Msg;
        }

        private void NetClient1_Msg(string obj)
        {
            OnMsg(obj);
        }
    }
}
