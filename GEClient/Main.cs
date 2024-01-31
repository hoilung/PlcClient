using HL.GESRTP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GEClient
{
    public partial class Main : Form
    {
        private SRTP SRTP = null;
        public Main()
        {
            InitializeComponent();
            this.Text += " v" + this.ProductVersion;
            changeState(false);
            cbx_type.SelectedIndex = 0;
        }
        void changeState(bool state)
        {
            btn_open.Enabled = !state;
            btn_readOne.Enabled = btn_add.Enabled = this.btn_close.Enabled = this.btn_read.Enabled = state;
        }
        private void btn_open_Click(object sender, EventArgs e)
        {
            SRTP = new SRTP(this.tbx_ip.Text);
            try
            {
                var result = SRTP.Open() == 1;
                changeState(result);
                tssl_tip.Text = "连接成功";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            if (SRTP.Connected)
            {
                SRTP.Close();
                SRTP = null;
                changeState(false);
                tssl_tip.Text = "连接已关闭";
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            var addrArry = tbx_address.Text.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            var array = addrArry.Select(m => GEDataItem.ParseFrom(m, !m.StartsWith("A"))).ToList();
            NewMethod(array);
            lv_data.Tag = array;
        }

        private void btn_read_Click(object sender, EventArgs e)
        {
            var index = 0;
            if (lv_data.SelectedItems.Count > 0)
            {
                index = int.Parse(lv_data.SelectedItems[0].Text);
            }

            var array = lv_data.Tag as List<GEDataItem>;
            var st = Stopwatch.StartNew();
            st.Start();
            SRTP.ReadMultipleVars(array.ToArray(), (int)numericUpDown1.Value);
            st.Stop();
            tssl_tip.Text = $"用时：{st.ElapsedMilliseconds}ms";

            NewMethod(array);

            if (index > 0)
            {
                lv_data.TopItem = lv_data.Items[index];
            }

        }

        private void NewMethod(List<GEDataItem> array)
        {
            lv_data.BeginUpdate();
            lv_data.Items.Clear();
            for (int i = 0; i < array.Count(); i++)
            {
                var item = new ListViewItem(i.ToString());
                if (array[i].Value == null)
                {
                    if (array[i].IsBit)
                    {
                        array[i].Value = false;
                    }
                    else
                    {
                        array[i].Value = 0;
                    }
                }
                item.SubItems.Add(array[i].Address);
                item.SubItems.Add(array[i].IsBit ? "bool" : "int");
                item.SubItems.Add(array[i].Value == null ? string.Empty : array[i].Value.ToString());
                lv_data.Items.Add(item);
            }
            lv_data.EndUpdate();
        }

        private void btn_readOne_Click(object sender, EventArgs e)
        {
            this.tbx_value.Text = string.Empty;
            var address = tbx_addressOne.Text.ToUpper();
            object result = null;
            var st = Stopwatch.StartNew();
            st.Start();
            switch (cbx_type.Text.ToLower())
            {
                case "bool":
                    result = SRTP.ReadBoolean(address);
                    break;
                case "int16":
                    result = SRTP.ReadInt16(address);
                    break;
                case "int32":
                    result = SRTP.ReadInt32(address);
                    break;
                case "float":
                    result = SRTP.ReadFloat(address);
                    break;
                default:
                    MessageBox.Show("尚未支持的类型");
                    break;
            }
            st.Stop();
            tssl_tip.Text = $"用时：{st.ElapsedMilliseconds}ms";
            tbx_value.Text = result.ToString();
        }

        private void lv_data_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                muen_lv.Show(lv_data, e.Location);
            }
        }

        private void tm_exportExcel_Click(object sender, EventArgs e)
        {

            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "Save File(*.csv)|*.csv";
            fileDialog.Title = "保存文件";
            fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            fileDialog.DefaultExt = "csv";
            fileDialog.FileName = DateTime.Now.ToString("yyyyMMdd-HHmmssffff");// DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                StringBuilder stringBuilder = new StringBuilder();
                var array = lv_data.Tag as List<GEDataItem>;
                for (int i = 0; i < array.Count; i++)
                {
                    stringBuilder.AppendLine($"{i},{array[i].Address},{array[i].DataType},{array[i].Value}");
                }

                File.WriteAllText(fileDialog.FileName, stringBuilder.ToString());
                tssl_tip.Text = $"保存文件：{fileDialog.FileName}";
                MessageBox.Show("保存文件成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }
    }


}
