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
    public partial class DeviceScan : BaseControl
    {
        public DeviceScan()
        {
            InitializeComponent();
            this.tabControl1.Dock = DockStyle.Fill;
            arpScanner1.Dock = pnDcpScan1.Dock = lldpScan1.Dock = DockStyle.Fill;
            this.arpScanner1.Msg += OnMsg;
            this.pnDcpScan1.Msg += OnMsg;
            this.lldpScan1.Msg += OnMsg;
        }

    }
}
