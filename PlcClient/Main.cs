using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlcClient
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            this.Text = this.ProductName + " v" + this.ProductVersion;
            this.Load += Form1_Load;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void Init()
        {
            var tabs = new TabControl();
            tabs.Dock = DockStyle.Fill;

            var tab_siemens = new TabPage();
            tab_siemens.Text = "SIEMENS 西门子";
            var siplc = new Controls.SiemensPLC();
            siplc.Msg += call_Msg;
            tab_siemens.Controls.Add(siplc);

            var tab_ge = new TabPage();
            tab_ge.Text = "GE 通用电气";
            var ge = new Controls.GePLC();
            ge.Msg += call_Msg;
            tab_ge.Controls.Add(ge);

            tabs.TabPages.Add(tab_siemens);
            tabs.TabPages.Add(tab_ge);

            this.Controls.Add(tabs);
        }



        private void call_Msg(string obj)
        {
            this.toolStrip_msg.Text = obj;
        }
    }
}
