using PlcClient.Controls;
using PlcClient.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            tabs.SuspendLayout();
            tabs.Dock = DockStyle.Fill;
            tabs.ImageList = imageList1;
            tabs.SelectedIndexChanged += Tabs_SelectedIndexChanged;

            var tab_siemens = new TabPage();
            tab_siemens.Text = "SIEMENS 西门子";
            tab_siemens.ImageIndex = 0;

            var siplc = new Controls.SiemensPLC();
            siplc.Msg += call_Msg;
            tab_siemens.Controls.Add(siplc);

            var tab_ge = new TabPage();
            tab_ge.Text = "GE 通用电气";
            tab_ge.ImageIndex = 1;
            var ge = new Controls.GePLC();
            ge.Msg += call_Msg;
            tab_ge.Controls.Add(ge);

            var tab_ab = new TabPage();
            tab_ab.Text = "AB 罗克韦尔";
            tab_ab.ImageIndex = 2;
            var ab = new Controls.AllenBradley();
            ab.Msg += call_Msg;
            tab_ab.Controls.Add(ab);


            var tab_opcda = new TabPage();
            tab_opcda.Text = "OPC DA";
            tab_opcda.ImageIndex = 3;
            var opcda = new OpcDa();
            opcda.Msg += call_Msg;
            tab_opcda.Controls.Add(opcda);

            var tab_opcua = new TabPage();
            tab_opcua.Text = "OPC UA";
            tab_opcua.ImageIndex = 3;
            var opcua = new OpcUa();
            opcua.Msg += call_Msg;
            tab_opcua.Controls.Add(opcua);


            tabs.TabPages.Add(tab_siemens);
            tabs.TabPages.Add(tab_ge);
            tabs.TabPages.Add(tab_ab);
            tabs.TabPages.Add(tab_opcda);
            tabs.TabPages.Add(tab_opcua);
            this.Controls.Add(tabs);
            tabs.ResumeLayout(false);
        }

        private void Tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is TabControl tabs)
            {
                if (tabs.SelectedTab.Controls != null && tabs.SelectedTab.Controls.Count > 0 && tabs.SelectedTab.Controls[0] is BaseControl baseControl)
                {
                    var arry = baseControl.FindControls<TextBox>(tabs.SelectedTab, true);
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
            }
        }

        private void call_Msg(string obj)
        {
            this.toolStrip_msg.Text = obj;
        }

    }
}
