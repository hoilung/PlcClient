using S7.Net;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PlcClient.Controls
{
    public partial class SiemensPLC : BaseControl
    {
        public SiemensPLC()
        {
            InitializeComponent();           

            this.Load += SiemensPLC_Load;
            this.tab_siemens.SelectedIndexChanged += Tab_siemens_SelectedIndexChanged;
        }

        private void Tab_siemens_SelectedIndexChanged(object sender, EventArgs e)
        {
            var arry = FindControls<TextBox>(this.tab_siemens.SelectedTab, true);
            if (arry != null)
            {
                var tbx_ip = arry.Where(tbx => Regex.IsMatch(tbx.Text, @"^\d+\.\d+\.\d+\.\d+")).FirstOrDefault();
                if (tbx_ip != null)
                {
                    tbx_ip.Focus();
                    var lastIndex = tbx_ip.Text.LastIndexOf('.') + 1;
                    tbx_ip.Select(lastIndex, tbx_ip.Text.Length - lastIndex);
                }
            }
        }

        private void SiemensPLC_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void Init()
        {
            this.tab_siemens.SuspendLayout();
            tab_siemens.TabPages.Clear();
            tab_siemens.Dock = DockStyle.Fill;
            

            var cputype = Enum.GetNames(typeof(CpuType)).OrderBy(m => m);
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
            this.tab_siemens.ResumeLayout();
        }

        private void UcBase_Msg(string obj)
        {
            OnMsg(obj);
        }
    }
}
