using HL.GESRTP;
using HL.Object.Extensions;
using PlcClient.Handler;
using PlcClient.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PlcClient.Controls
{
    public partial class GePLC : BaseControl
    {
        #region field

        private SRTP SRTP = null;
        private TypeCode _typeCode;
        private ListViewHandler lvwHandler;
        public override TypeCode[] TypeCodes => new[] {
            TypeCode.Boolean,
            //TypeCode.Byte,
            TypeCode.Int16,
            TypeCode.Int32,
            TypeCode.Int64,
            TypeCode.Single,
            TypeCode.Double,
            TypeCode.UInt16,
            TypeCode.UInt32,
            TypeCode.UInt64,
        };
        #endregion
        public GePLC()
        {
            InitializeComponent();
            tableLayoutPanel1.Dock = DockStyle.Fill;
            Init();
            changeState(false);


        }

        private void Init()
        {
            lvwHandler = new ListViewHandler(this.lv_data);
            lvwHandler.ColuminSort();
            tbx_ip.Text = GetLocalIP();

            var typeArry = TypeCodes.Select(m => new { Name = m, Value = m.ToString() }).ToList();
            cbx_type.DisplayMember = "Value";
            cbx_type.ValueMember = "Name";
            cbx_type.DataSource = typeArry;
            _typeCode = (TypeCode)cbx_type.SelectedValue;
            cbx_type.SelectedIndexChanged += cbx_type_SelectedIndexChanged;

            //cbx_changetype.DisplayMember = "Value";
            //cbx_changetype.ValueMember = "Name";
            //cbx_changetype.DataSource = typeArry;

            btn_write.Enabled = false;
            tbx_value.ReadOnly = !chk_enablewrite.Checked;

            tbx_address.Text = Properties.Resources.ge_tip;
        }
        private void Radio_CheckedChanged(object sender, EventArgs e)
        {
            var rbtn = (sender as RadioButton);
            if (rbtn != null && rbtn.Checked && rbtn.Tag != null && rbtn.Tag is TypeCode tc)
            {
                this._typeCode = tc;
            }
        }

        void changeState(bool state)//连接断开时候的ui状态
        {
            btn_open.Enabled = !state;
            if (!state)
                this.chk_enablewrite.Checked = state;
            chk_enablewrite.Enabled = btn_readOne.Enabled = btn_add.Enabled = this.btn_close.Enabled = this.btn_read.Enabled = state;
            //lb_address.Visible = cbx_changetype.Visible = btn_changetype.Visible = state && lv_data.SelectedItems.Count > 0;

            tbx_ip.ReadOnly = tbx_port.ReadOnly = state;
        }

        private void btn_open_Click(object sender, EventArgs e)
        {

            try
            {
                var ip = this.tbx_ip.Text;
                if (!Regex.IsMatch(ip, AppConfig.IPVerdify))
                {
                    MessageBox.Show($"{ip} 无效的IP地址");
                    tbx_ip.Focus();
                    return;
                }
                var ipState = ping.Send(ip, 500);
                if (ipState.Status != IPStatus.Success)
                {
                    if (MessageBox.Show($"{ip}\r\n网络PING疑似不通,是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }
                SRTP = new SRTP(ip);
                var result = SRTP.Open() == 1;
                changeState(result);
                this.OnMsg(result ? "连接成功 " + ip : "连接失败 " + ip);
                if (!result)
                    MessageBox.Show(ip + "\r\n连接失败,请检查IP或网络是否正常", "连接失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                tbx_address.ResetText();
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
                changeState(false);
                //tssl_tip.Text = "连接已关闭";
                this.OnMsg("连接关闭 " + this.tbx_ip.Text);
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            var addrArry = tbx_address.Lines.Select(m => m.ToUpper().Split(new[] { "\t", " ", "|" }, StringSplitOptions.RemoveEmptyEntries)).ToList();
            var err = addrArry.Where(m => !Regex.IsMatch(m[0], "^(AI|AQ|SA|SB|SC|R|I|M|Q|T|G)\\d{1,5}$")).ToList();
            if (err.Count > 0)
            {
                MessageBox.Show("无效的地址：" + err.FirstOrDefault(), "提示");
                return;
            }
            var wordArea = new[] { "R", "AI", "AQ" };
            //var typeArry = new[] { "BOOL", "BYTE", "WORD", "DWORD"};
            List<GEDataItem> list = new List<GEDataItem>();
            for (int i = 0; i < addrArry.Count; i++)
            {
                var item = addrArry[i];

                var geitem = GEDataItem.ParseFrom(item[0], wordArea.Count(w => item[0].StartsWith(w)) == 0);//默认 R/AI/AQ 读取长度是字节，实际单位是word ；其他区是位和字节，属于不同地址
                if (item.Length == 2 && item[1] != "BOOL")
                {
                    if (geitem.IsBit)
                    {
                        geitem = GEDataItem.ParseFrom(item[0], false);
                    }
                    switch (item[1])
                    {
                        case "INT"://int16
                            geitem.Value = (short)0;//word区,默认类型
                            break;
                        case "WORD"://uint16
                            geitem.Value = (ushort)0;
                            break;
                        case "DINT"://int32,
                            geitem.Value = (int)0;
                            break;
                        case "DWORD"://uint32,
                            geitem.Value = (uint)0;
                            break;
                        case "REAL"://float
                            geitem.Value = (float)0;
                            break;
                        case "LREAL"://double
                            geitem.Value = (double)0;
                            break;
                    }
                }
                list.Add(geitem);
            }
            AddListView(list);
            lv_data.Tag = list;

        }

        private void btn_read_Click(object sender, EventArgs e)
        {

            var array = lv_data.Tag as List<GEDataItem>;
            if (array == null)
                return;

            progressBar1.Maximum = (int)tbx_num.Value;
            progressBar1.Value = 0;
            for (int j = 0; j < tbx_num.Value; j++)
            {
                stopwatch.Restart();
                if ((int)numericUpDown1.Value > 0)
                    SRTP.ReadMultipleVars(array.ToArray(), (int)numericUpDown1.Value);
                else
                    SRTP.ReadMultipleVars(array.ToArray());
                stopwatch.Stop();
                this.OnMsg($"批量读取 {array.Count}个，用时：{stopwatch.Elapsed.TotalMilliseconds.ToString("0.000ms")}");
                //刷新ui
                for (int i = 0; i < lv_data.Items.Count; i++)
                {
                    var item = lv_data.Items[i].Tag as GEDataItem;
                    if (item == null) continue;
                    lv_data.Items[i].SubItems[3].Text = item.Value?.ToString();
                    lv_data.Items[i].SubItems[4].Text = DEC2BIN2HEX(array[i].Value, 2);
                    lv_data.Items[i].SubItems[5].Text = DEC2BIN2HEX(array[i].Value, 16);
                }
                progressBar1.PerformStep();
                Delay((int)tbx_time.Value);
            }

        }


        private void AddListView(List<GEDataItem> array)
        {
            lv_data.BeginUpdate();
            lv_data.Items.Clear();
            for (int i = 0; i < array.Count(); i++)
            {
                var item = new ListViewItem($"{i}");
                item.Tag = array[i];
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
                item.SubItems.Add(string.Empty);//item.SubItems.Add(array[i].Value == null ? string.Empty : array[i].Value.ToString());
                item.SubItems.Add(string.Empty);
                item.SubItems.Add(string.Empty);

                if (i % 2 == 0)
                {
                    item.BackColor = System.Drawing.Color.AliceBlue;
                }
                lv_data.Items.Add(item);
            }
            lv_data.EndUpdate();
            //lb_address.Visible = cbx_changetype.Visible = btn_changetype.Visible = lv_data.SelectedItems.Count > 0;
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

            stopwatch.Restart();
            switch (_typeCode)
            {
                case TypeCode.Boolean:
                    result = SRTP.ReadBoolean(address);
                    break;
                case TypeCode.Int16:
                    result = SRTP.ReadInt16(address);
                    break;
                case TypeCode.Int32:
                    result = SRTP.ReadInt32(address);
                    break;
                case TypeCode.Int64:
                    result = SRTP.ReadInt64(address);
                    break;
                case TypeCode.Single:
                    result = SRTP.ReadSingle(address);
                    break;
                case TypeCode.Double:
                    result = SRTP.ReadDouble(address);
                    break;
                case TypeCode.UInt16:
                    result = SRTP.ReadUInt16(address);
                    break;
                case TypeCode.UInt32:
                    result = SRTP.ReadUInt32(address);
                    break;
                case TypeCode.UInt64:
                    result = SRTP.ReadUInt64(address);
                    break;
            }
            stopwatch.Stop();
            this.OnMsg($"{address} 读取 {result}，用时：{stopwatch.Elapsed.TotalMilliseconds.ToString("0.000ms")}");
            tbx_value.Text = result.ToString();
        }



        //菜单导出
        private void tm_exportExcel_Click(object sender, EventArgs e)
        {
            var filename = lvwHandler.ExportSCV(tbx_ip.Text);
            if (string.IsNullOrEmpty(filename))
                return;
            this.OnMsg($"保存文件：{filename}");
            MessageBox.Show("保存文件成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        //列表选中
        private void lv_data_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lv_data.SelectedItems.Count > 0)
            {
                //var index = int.Parse(lv_data.SelectedItems[0].Text);                
                tbx_addressOne.Text = lv_data.SelectedItems[0].ToString();
                tbx_addressOne.Text = lv_data.SelectedItems[0].SubItems[1].Text;
                cbx_type.Text = lv_data.SelectedItems[0].SubItems[2].Text;
                tbx_value.Text = string.Empty;
            }

        }


        //是否可写
        private void ckb_enablewrite_CheckedChanged(object sender, EventArgs e)
        {
            btn_write.Enabled = chk_enablewrite.Checked;
            tbx_value.ReadOnly = !chk_enablewrite.Checked;
        }

        private void btn_write_Click(object sender, EventArgs e)
        {
            var adr = tbx_addressOne.Text.Trim();
            var val = tbx_value.Text.Trim();
            try
            {
                var item = GEDataItem.ParseFrom(adr, _typeCode == TypeCode.Boolean);
                var typecode = (TypeCode)cbx_type.SelectedValue;
                item.Value = val.ConvertToValueType(typecode);
                item.IsBit = typecode == TypeCode.Boolean;
                var state = false;
                stopwatch.Restart();
                switch (typecode)
                {
                    case TypeCode.Boolean:
                        state = SRTP.WriteBoolean(item.Address, (bool)item.Value);
                        break;
                    case TypeCode.Int16:
                        state = SRTP.WriteInt16(item.Address, (Int16)item.Value);
                        break;
                    case TypeCode.Int32:
                        state = SRTP.WriteInt32(item.Address, (Int32)item.Value);
                        break;
                    case TypeCode.Int64:
                        state = SRTP.WriteInt64(item.Address, (Int64)item.Value);
                        break;
                    case TypeCode.Single:
                        state = SRTP.WriteSingle(item.Address, (Single)item.Value);
                        break;
                    case TypeCode.Double:
                        state = SRTP.WriteDouble(item.Address, (Double)item.Value);
                        break;
                    case TypeCode.UInt16:
                        state = SRTP.WriteUInt16(item.Address, (UInt16)item.Value);
                        break;
                    case TypeCode.UInt32:
                        state = SRTP.WriteUInt32(item.Address, (UInt32)item.Value);
                        break;
                    case TypeCode.UInt64:
                        state = SRTP.WriteUInt64(item.Address, (UInt64)item.Value);
                        break;
                }
                stopwatch.Stop();
                OnMsg($"{adr} 写入 {item.Value} {(state ? "成功" : "失败")}，用时：" + stopwatch.Elapsed.TotalMilliseconds.ToString("0.000ms"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                OnMsg(ex.Message);
            }

        }
        private void cbx_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            _typeCode = (TypeCode)cbx_type.SelectedValue;
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.lv_data.Clear();
        }

        private void chk_enablewrite_Click(object sender, EventArgs e)
        {
            if (!AppConfig.Instance.SafeConfirm)
            {
                AppConfig.Instance.SafeConfirm = ShowAbout() == DialogResult.OK;
            }
            if (!AppConfig.Instance.SafeConfirm)
            {
                (sender as CheckBox).Checked = AppConfig.Instance.SafeConfirm;
            }
        }
    }


}
