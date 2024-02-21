using S7.Net;
using System;
using System.Windows.Forms;

namespace PlcClient.Controls
{
    public partial class SiemensPLC : BaseControl
    {
        public SiemensPLC()
        {
            InitializeComponent();

            this.Load += SiemensPLC_Load;
        }

        private void SiemensPLC_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void Init()
        {
            tab_siemens.TabPages.Clear();
            tab_siemens.Dock = DockStyle.Fill;

            var cputype = Enum.GetNames(typeof(CpuType));
            foreach (var c in cputype)
            {
                var ucBase = new SiemensBase();
                ucBase.Dock = DockStyle.Fill;
                ucBase.Msg += UcBase_Msg;
                ucBase.CpuType = (CpuType)Enum.Parse(typeof(CpuType), c);

                var tabpage = new TabPage();
                tabpage.Text = c.Replace("S7", "S7-");
                tabpage.Controls.Add(ucBase);
                tab_siemens.TabPages.Add(tabpage);
            }
        }

        private void UcBase_Msg(string obj)
        {
            OnMsg(obj);
        }
    }
}
