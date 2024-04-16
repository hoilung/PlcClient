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
            this.netClient1.Msg += Net_Msg;
            this.net2Server1.Msg += Net_Msg;
        }


        private void Net_Msg(string obj)
        {
            OnMsg(obj);
        }
    }
}
