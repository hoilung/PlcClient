using NewLife;
using NewLife.Log;
using PlcClient.Handler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace PlcClient.Controls
{
    public partial class Netstat : BaseControl
    {
        private ListViewHandler<ProcessPortInfo> viewHandler;
        public Netstat()
        {
            InitializeComponent();
            this.Dock = listViewEx1.Dock = tableLayoutPanel1.Dock = DockStyle.Fill;
            viewHandler = new ListViewHandler<ProcessPortInfo>(listViewEx1);
            viewHandler.SetupVirtualMode();
            viewHandler.ColuminSort();
            listViewEx1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

            //var list=Enum.GetNames(typeof(NetStatEnum)).ToDictionary();
            //获取枚举的值和attr表述
            var list = Enum.GetValues(typeof(NetStatEnum)).Cast<NetStatEnum>().ToDictionary(e => e, e => e.GetDescription());
            ts_cbx_type.ComboBox.DataSource = new BindingSource(list, null);
            ts_cbx_type.ComboBox.DisplayMember = "Value";
            ts_cbx_type.ComboBox.ValueMember = "Key";
            ts_cbx_type.ComboBox.SelectedIndex = 1;
            listViewEx1.MouseClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    contextMenuStrip1.Show(listViewEx1, e.X, e.Y);
                }
            };

        }

        private void ts_btn_search_Click(object sender, EventArgs e)
        {
            viewHandler.Clear();
            if (ts_cbx_type.ComboBox.SelectedValue is NetStatEnum _type)
            {
                Task.Factory.StartNew(() =>
                {
                    var list = Handler.ProcessHandler.Instance.GetListenPort(_type);
                    this.Invoke(() =>
                    {
                        viewHandler.LoadAdd(list);
                        viewHandler.NotifyAllChanged();
                        this.OnMsg($"查询完成,共计 {list.Count} 记录");
                    });
                });
            }


        }

        private void ts_btn_clear_Click(object sender, EventArgs e)
        {
            viewHandler.Clear();
        }

        private void ts_btn_export_Click(object sender, EventArgs e)
        {
            viewHandler.ExportExcel(ts_cbx_type.Text);
        }

        private void copyselectrowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewEx1.SelectedIndices.Count > 0)
            {
                try
                {
                    var sb = new StringBuilder();
                    for (int i = 0; i < listViewEx1.SelectedIndices.Count; i++)
                    {
                        var item = viewHandler[listViewEx1.SelectedIndices[i]];
                        sb.AppendLine(item.ToString());

                    }
                    Clipboard.SetText(sb.ToString());
                    this.OnMsg("复制成功");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "复制失败");
                }
            }
        }

        private void openselectrowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewEx1.SelectedIndices.Count > 0)
            {
                try
                {
                    int id = listViewEx1.SelectedIndices[0];
                    if (id >= 0 && (id < viewHandler.ItemCount))
                    {
                        var item = viewHandler[id].ProcessPath;
                        if (!item.IsNullOrEmpty() && File.Exists(item))
                        {
                            Process.Start("explorer.exe",$"/select,{item}");                           

                        }
                    }

                }
                catch (Exception ex)
                {
                    XTrace.WriteException(ex);
                }
            }
        }
    }
}
