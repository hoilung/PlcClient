using HL.GESRTP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GEClient.Controls
{
    public partial class GePLC : BaseControl
    {
       

        private SRTP SRTP = null;
        public GePLC()
        {
            InitializeComponent();
            //this.Text += " v" + this.ProductVersion;// + " by:" + this.CompanyName;

            changeState(false);
            cbx_type.SelectedIndex = 0;
        }
        void changeState(bool state)
        {
            btn_open.Enabled = !state;
            btn_readOne.Enabled = btn_add.Enabled = this.btn_close.Enabled = this.btn_read.Enabled = state;

            lb_address.Visible = cbx_changetype.Visible = btn_changetype.Visible = state && lv_data.SelectedItems.Count > 0;
        }
        private void btn_open_Click(object sender, EventArgs e)
        {
            SRTP = new SRTP(this.tbx_ip.Text);
            try
            {
                var result = SRTP.Open() == 1;
                changeState(result);

                this.OnMsg(result ? "连接成功" : "连接失败");
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
                //tssl_tip.Text = "连接已关闭";
                this.OnMsg("连接已关闭");
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            var addrArry = tbx_address.Text.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            var err = addrArry.Where(m => !Regex.IsMatch(m, "^(AI|AQ|SA|SB|SC|R|I|M|Q|T|G)\\d{1,5}$")).ToList();
            if (err.Count > 0)
            {
                MessageBox.Show("无效的地址：" + err.FirstOrDefault(), "提示");
                return;
            }
            var wordArea = new[] { "R", "AI", "AQ" };
            var array = addrArry.Select(m => GEDataItem.ParseFrom(m, wordArea.Count(w => m.StartsWith(w)) == 0)).ToList();
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
            if (array == null)
                return;
            var st = Stopwatch.StartNew();
            st.Start();

            if ((int)numericUpDown1.Value > 0)
                SRTP.ReadMultipleVars(array.ToArray(), (int)numericUpDown1.Value);
            else
                SRTP.ReadMultipleVars(array.ToArray());
            st.Stop();
            //tssl_tip.Text = $"用时：{st.ElapsedMilliseconds}ms";
            this.OnMsg($"用时：{st.ElapsedMilliseconds}ms");

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
                        array[i].Value = (short)0;
                    }
                }
                item.SubItems.Add(array[i].Address);
                item.SubItems.Add(array[i].Value.GetType().Name);
                item.SubItems.Add(array[i].Value == null ? string.Empty : array[i].Value.ToString());
                lv_data.Items.Add(item);
            }
            lv_data.EndUpdate();
            lb_address.Visible = cbx_changetype.Visible = btn_changetype.Visible = lv_data.SelectedItems.Count > 0;
        }

        private void btn_readOne_Click(object sender, EventArgs e)
        {

            var address = tbx_addressOne.Text.ToUpper();
            if (!Regex.IsMatch(address, "^(AI|AQ|SA|SB|SC|R|I|M|Q|T|G)\\d{1,5}$"))
            {
                MessageBox.Show("无效的地址：" + address, "提示");
                return;
            }

            this.tbx_value.Text = string.Empty;
            object result = string.Empty;
            var st = Stopwatch.StartNew();
            st.Start();
            switch (cbx_type.Text.ToLower())
            {
                case "boolean":
                    result = SRTP.ReadBoolean(address);
                    break;
                case "int16":
                    result = SRTP.ReadInt16(address);
                    break;
                case "int32":
                    result = SRTP.ReadInt32(address);
                    break;
                case "int64":
                    result = SRTP.ReadInt64(address);
                    break;
                case "single":
                    result = SRTP.ReadSingle(address);
                    break;
                case "double":
                    result = SRTP.ReadDouble(address);
                    break;
                case "uint16":
                    result = SRTP.ReadUInt16(address);
                    break;
                case "uint32":
                    result = SRTP.ReadUInt32(address);
                    break;
                case "uint64":
                    result = SRTP.ReadUInt64(address);
                    break;
                default:
                    MessageBox.Show("尚未支持的类型");
                    break;
            }
            st.Stop();
            this.OnMsg($"用时：{st.ElapsedMilliseconds}ms");
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
                this.OnMsg($"保存文件：{fileDialog.FileName}");
                MessageBox.Show("保存文件成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void lv_data_SelectedIndexChanged(object sender, EventArgs e)
        {
            lb_address.Visible = cbx_changetype.Visible = btn_changetype.Visible = lv_data.SelectedItems.Count > 0;
            if (lv_data.SelectedItems.Count > 0)
            {
                var index = int.Parse(lv_data.SelectedItems[0].Text);
                cbx_changetype.Tag = index;

                tbx_addressOne.Text = lb_address.Text = lv_data.SelectedItems[0].SubItems[1].Text;
                cbx_type.Text = cbx_changetype.Text = lv_data.SelectedItems[0].SubItems[2].Text;
                tbx_value.Text = string.Empty;
            }

        }

        private void btn_changetype_Click(object sender, EventArgs e)
        {
            if (cbx_changetype.Tag == null)
            {
                MessageBox.Show("当前列表内容未选中", "修改类型失败");
                return;
            }
            int index = int.Parse(cbx_changetype.Tag.ToString());
            var list = lv_data.Tag as List<GEDataItem>;
            if (list.Any())
            {
                var item = list[index];
                switch (cbx_changetype.Text.ToLower())
                {
                    case "boolean":
                        item.Value = false;
                        item.IsBit = true;
                        break;
                    case "int16":
                        item.Value = (Int16)0;
                        item.IsBit = false;
                        break;
                    case "int32":
                        item.Value = (Int32)0;
                        item.IsBit = false;
                        break;
                    case "int64":
                        item.Value = (Int64)0;
                        item.IsBit = false;
                        break;
                    case "single":
                        item.Value = (Single)0;
                        item.IsBit = false;
                        break;
                    case "double":
                        item.Value = (Double)0;
                        item.IsBit = false;
                        break;
                    case "uint16":
                        item.Value = (UInt16)0;
                        item.IsBit = false;
                        break;
                    case "uint32":
                        item.Value = (UInt32)0;
                        item.IsBit = false;
                        break;
                    case "uint64":
                        item.Value = (UInt64)0;
                        item.IsBit = false;
                        break;
                }
                NewMethod(list);
                cbx_changetype.Tag = null;
                lv_data.TopItem = lv_data.Items[index];
            }
        }


    }


}
