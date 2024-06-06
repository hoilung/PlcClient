using HL.Object.Extensions;
using HL.S7netplus.Extensions;
using NewLife.Log;
using Opc.Ua;
using PlcClient.Handler;
using PlcClient.Model;
using S7.Net;
using S7.Net.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PlcClient.Controls
{
    public partial class SiemensBase : BaseControl
    {
        private const string _addressVerdify = @"(^DB\d+\.DB[BWD]\d+$)|(^DB\d+\.DBX\d+\.[0-7]$)|(^(VB|VW|VD|MB|MW|MD|IB|IW|ID|EB|EW|ED|QB|QW|QD|AB|AW|AD|OB|OW|OD)\d+$)|(^(A|V|M|I|E|Q|O|T|Z|C)\d+\.[0-7]$)";//西门子plc地址验证


        public CpuType CpuType { get; set; }
        private TypeCode _typeCode;

        private Plc Plc { get; set; }

        private ListViewHandler lvwHandler;
        public SiemensBase()
        {
            InitializeComponent();
            tableLayoutPanel1.Dock = DockStyle.Fill;
            this.Load += SiemensBase_Load;
            this.Disposed += SiemensBase_Disposed;

            btn_write.Enabled = false;
            tbx_value.ReadOnly = true;
            lvwHandler = new ListViewHandler(this.lv_data);
            lvwHandler.ColuminSort();
        }

        private void SiemensBase_Disposed(object sender, EventArgs e)
        {
            if (Plc != null && Plc.IsConnected)
            {
                Plc.Close();
            }
        }

        private void SiemensBase_Load(object sender, EventArgs e)
        {
            Init();
            ChangeState(false);
        }

        private void Init()
        {
            tbx_ip.Text = GetLocalIP();

            var typeArry = TypeCodes.Select(m => new { Name = m, Value = m.ToString() }).ToList();
            cbx_type.DisplayMember = "Value";
            cbx_type.ValueMember = "Name";
            cbx_type.DataSource = typeArry;
            _typeCode = TypeCode.Boolean;//默认选择
            cbx_type.SelectedIndexChanged += Cbx_type_SelectedIndexChanged;


            gb_set.Text = this.CpuType.ToString().Replace("S7", "S7-") + " " + gb_set.Text;
            tbx_adr.Text = "M2.3";

            tbx_msg.Dock = DockStyle.Fill;

            Msg2Text($"适用于西门子PLC {this.CpuType.ToString().Replace("S7", "S7-")}");
            Msg2Text("\r\n" + Properties.Resources.smz_tip);
            Msg2Text("\r\n");

            tbx_addressAll.Text = Properties.Resources.pl_siemens;
        }

        private void Cbx_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            _typeCode = (TypeCode)cbx_type.SelectedValue;
        }
        private void ChangeState(bool state)
        {
            if (!state)
                this.chk_enablewrite.Checked = state;
            this.chk_enablewrite.Enabled = this.btn_read.Enabled = this.btn_write.Enabled = this.btn_close.Enabled = state;

            this.btn_open.Enabled = !state;
            //cbx_type.Enabled = state;

            btn_add.Enabled = btn_readAll.Enabled = state;


        }

        private void btn_clearTbx_Click(object sender, EventArgs e)
        {
            this.tbx_msg.Clear();
        }

        private void btn_open_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Regex.IsMatch(tbx_ip.Text, AppConfig.IPVerdify) || !Regex.IsMatch(tbx_port.Text, @"^\d+$") || !Regex.IsMatch(tbx_rack.Text, @"^\d+$") || !Regex.IsMatch(tbx_slot.Text, @"^\d+$"))
                {
                    MessageBox.Show("设置错误，请检测IP、端口等是否填写正常", "提示");
                    return;
                }

                var ip = tbx_ip.Text;
                var port = int.Parse(tbx_port.Text);
                var rack = short.Parse(tbx_rack.Text);
                var slot = short.Parse(tbx_slot.Text);


                var result = ping.Send(ip, 500);
                if (result.Status != IPStatus.Success)
                {
                    if (MessageBox.Show($"{ip}\r\n网络PING疑似不通,是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }

                Plc = new Plc(CpuType, ip, port, rack, slot);
                Plc.Open();

                ChangeState(Plc.IsConnected);
                tbx_msg.ResetText();
                tbx_addressAll.ResetText();
                var msg = $"{(Plc.IsConnected ? "连接成功" : "连接失败")} {CpuType.ToString().Replace("S7", "S7-")} {ip}";
                OnMsg(msg);
                Msg2Text(msg);
            }
            catch (Exception ex)
            {
                OnMsg("连接错误");
                Msg2Text("连接错误：" + ex.Message, true, true);
            }
        }

        private void Msg2Text(string msg, bool addtime = false, bool reset = false)
        {
            if (reset)
            {
                this.tbx_msg.ResetText();
            }
            if (addtime)
            {
                this.tbx_msg.AppendText("\r\n" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\t" + msg);
            }
            else
                this.tbx_msg.AppendText("\r\n" + msg);
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            try
            {
                Plc.Close();
                ChangeState(Plc.IsConnected);
                OnMsg("连接关闭 " + Plc.IP);
                Msg2Text("连接关闭 " + Plc.IP);
            }
            catch (Exception ex)
            {
                OnMsg("关闭错误");
                Msg2Text(ex.Message);
            }
        }


        private void btn_read_Click(object sender, EventArgs e)
        {
            var address = tbx_adr.Text.Trim().ToUpper();
            if (string.IsNullOrWhiteSpace(address) || !Regex.IsMatch(address, _addressVerdify))
            {
                MessageBox.Show($"{address} 无效的地址");
                tbx_adr.Focus();
                return;
            }

            if (Plc == null || !Plc.IsConnected)
            {
                MessageBox.Show("读取失败！请先连接plc", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                tbx_value.ResetText();
                var dt = DataItem2.FromAddressByTypeCode(address, _typeCode);
                stopwatch.Restart();
                var obj = this.Plc.Read(dt);
                stopwatch.Stop();
                //转换类型
                var obj2 = DataItem2.ValueChangeType(obj, _typeCode);
                tbx_value.Text = obj2.ToString();
                Msg2Text($"{address}\t{obj2?.ToString()}\t用时：{stopwatch.Elapsed.TotalMilliseconds.ToString("0.000ms")}");

                OnMsg($"{address} 读取 {obj2.ToString()}，用时：{stopwatch.Elapsed.TotalMilliseconds.ToString("0.000ms")}");
            }
            catch (Exception ex)
            {
                Msg2Text(ex.Message, true);
                MessageBox.Show(ex.Message, "错误");
            }


        }

        private void btn_write_Click(object sender, EventArgs e)
        {
            if (Plc == null || !Plc.IsConnected)
            {
                MessageBox.Show("读取失败！请先连接plc", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var val = tbx_value.Text.Trim().ToLower();
            var adr = tbx_adr.Text;
            try
            {

                var dt = DataItem2.FromAddressByTypeCode(adr, _typeCode);

                if (dt.VarType == VarType.LReal)
                {
                    dt.Value = val.ConvertToValueType<double>();
                }
                else
                {
                    dt.Value = val.ConvertToValueType(_typeCode);
                }
                stopwatch.Restart();
                Plc.Write(dt);
                stopwatch.Stop();
                Msg2Text($"{adr}\t{val}\t用时：{stopwatch.Elapsed.TotalMilliseconds.ToString("0.000ms")}");

                OnMsg($"{adr} 写入 {val} 成功，用时：{stopwatch.Elapsed.TotalMilliseconds.ToString("0.000ms")}");

            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                Msg2Text(adr + " " + ex.Message);
                MessageBox.Show(ex.Message, "写入失败");
            }
        }

        private void cbx_enablewrite_CheckedChanged(object sender, EventArgs e)
        {
            btn_write.Enabled = chk_enablewrite.Checked;
            tbx_value.ReadOnly = !chk_enablewrite.Checked;
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            var line = tbx_addressAll.Lines.Where(m => !string.IsNullOrWhiteSpace(m));
            var adrErr = line.FirstOrDefault(m => !Regex.IsMatch(m.Split(new[] { "\t", " ", "|" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(), _addressVerdify));
            if (adrErr != null)
            {
                MessageBox.Show($"{adrErr} 无效的PLC地址");
                tbx_addressAll.Focus();
                tbx_addressAll.Select(tbx_addressAll.Text.IndexOf(adrErr), adrErr.Length);
                return;
            }
            var list = line.Distinct().ToList();//.Select(m => (address: m, item: DataItem2.FromAddress2(m))).ToList();


            lv_data.BeginUpdate();
            lv_data.Items.Clear();
            var arr = new[] { "BOOL", "INT", "DINT", "REAL", "LREAL" };
            for (int i = 0; i < list.Count; i++)
            {

                var li = list[i].ToUpper().Split(new[] { "\t", " ", "|" }, StringSplitOptions.RemoveEmptyEntries);
                var dataItem = DataItem2.FromAddress2(li[0]);
                if (li.Length == 2 && arr.Contains(li[1]))
                {
                    switch (li[1])
                    {
                        case "BOOL":
                            dataItem.VarType = VarType.Bit;
                            break;
                        case "INT":
                            dataItem.VarType = VarType.Int;
                            break;
                        case "DINT":
                            dataItem.VarType = VarType.DInt;
                            break;
                        case "REAL":
                            dataItem.VarType = VarType.Real;
                            break;
                        case "LReal":
                            dataItem.VarType = VarType.LReal;
                            break;
                    }
                }
                var item = new ListViewItem($"{i}");
                item.Tag = dataItem;
                item.SubItems[0].Tag = i;
                //item.SubItems.Add(dataItem.DataType.ToString());
                item.SubItems.Add(li[0]);
                TypeCode typeCode = TypeCode.Empty;
                switch (dataItem.VarType)
                {
                    case VarType.Bit:
                        typeCode = TypeCode.Boolean;
                        break;
                    case VarType.Byte:
                        typeCode = TypeCode.Byte;
                        break;
                    case VarType.Word:
                        typeCode = TypeCode.UInt16;
                        break;
                    case VarType.DWord:
                        typeCode = TypeCode.UInt32;
                        break;
                    case VarType.Int:
                        typeCode = TypeCode.Int16;
                        break;
                    case VarType.DInt:
                        typeCode = TypeCode.Int32;
                        break;
                    case VarType.Real:
                        typeCode = TypeCode.Single;
                        break;
                    case VarType.LReal:
                        typeCode = TypeCode.Double;
                        break;
                    default:
                        break;
                }
                item.SubItems.Add(typeCode.ToString());
                item.SubItems.Add(dataItem.Value?.ToString());
                item.SubItems.Add(string.Empty);//BIN
                item.SubItems.Add(string.Empty);//HEX

                if(i%2 == 0)
                {
                    item.BackColor = Color.AliceBlue;
                }
                lv_data.Items.Add(item);
            }
            lv_data.EndUpdate();

        }

        private void lv_data_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lv_data.SelectedItems.Count > 0)
            {
                var address = lv_data.SelectedItems[0].SubItems[1].Text;
                tbx_adr.Text = address;

                var typeCode = (TypeCode)Enum.Parse(typeof(TypeCode), lv_data.SelectedItems[0].SubItems[2].Text);
                cbx_type.SelectedValue = typeCode;
            }
        }

        private void btn_readAll_Click(object sender, EventArgs e)
        {
            progressBar1.Maximum = (int)tbx_num.Value;
            progressBar1.Value = 0;
            for (int i = 0; i < tbx_num.Value; i++)
            {
                mutli_select();
                progressBar1.PerformStep();
                Delay((int)this.tbx_time.Value);
            }
        }


        private void mutli_select()
        {
            var dataItemList = new List<DataItem>();

            foreach (ListViewItem item in lv_data.Items)
            {
                if (item.Tag is DataItem dataItem)
                {
                    dataItemList.Add(dataItem);
                }
            }
            var dataItemGroup = GetDataItemGroup(dataItemList);//合并bit位分组 230831

            stopwatch.Restart();
            int num = (this.Plc.MaxPDUSize - 19) / 12;
            for (int j = 0; j < dataItemGroup.Count; j += num)
            {
                this.Plc.ReadMultipleVars(dataItemGroup.Skip(j).Take(num).ToList());
            }
            stopwatch.Stop();
            var bitList = dataItemGroup.Where(m => m.VarType == VarType.Bit).ToList();

            foreach (var item in dataItemList)
            {
                try
                {
                    if (item.VarType == VarType.Bit)
                    {
                        var bitItem = bitList.Where(m => m.DB == item.DB && m.DataType == item.DataType && m.StartByteAdr == item.StartByteAdr).FirstOrDefault();
                        if (bitItem != null && bitItem.Value is BitArray bitarry && bitarry.Count == 8)
                        {
                            item.Value = bitarry.Get(item.BitAdr);
                        }
                    }
                }
                catch (Exception ex)
                {
                    XTrace.WriteException(ex);
                    throw;
                }
            }

            //ui 
            foreach (ListViewItem item in lv_data.Items)
            {
                if (item.Tag is DataItem dataItem)
                {
                    item.SubItems[3].Text = dataItem.Value?.ToString();

                    item.SubItems[4].Text = dec2bin2hex(dataItem.Value, 2);

                    item.SubItems[5].Text = dec2bin2hex(dataItem.Value, 16);

                }
            }
            OnMsg($"批量读取 {lv_data.Items.Count}个,用时：{stopwatch.Elapsed.TotalMilliseconds.ToString("0.000ms")}");
        }

        private string dec2bin2hex(object val, int format)
        {
            if (val == null)
                return string.Empty;
            if (val.GetType().IsValueType)
            {
                var type_code = Type.GetTypeCode(val.GetType());
                byte[] binaryBytes = null;
                switch (type_code)
                {
                    case TypeCode.Boolean:
                        binaryBytes = BitConverter.GetBytes((bool)val);
                        break;
                    case TypeCode.Char:
                        binaryBytes = BitConverter.GetBytes((char)val);
                        break;
                    case TypeCode.SByte:
                        binaryBytes = BitConverter.GetBytes((sbyte)val);
                        break;
                    case TypeCode.Byte:
                        binaryBytes = BitConverter.GetBytes((byte)val);
                        break;
                    case TypeCode.Int16:
                        binaryBytes = BitConverter.GetBytes((Int16)val);
                        break;
                    case TypeCode.UInt16:
                        binaryBytes = BitConverter.GetBytes((UInt16)val);
                        break;
                    case TypeCode.Int32:
                        binaryBytes = BitConverter.GetBytes((Int32)val);
                        break;
                    case TypeCode.UInt32:
                        binaryBytes = BitConverter.GetBytes((UInt32)val);
                        break;
                    case TypeCode.Int64:
                        binaryBytes = BitConverter.GetBytes((Int64)val);
                        break;
                    case TypeCode.UInt64:
                        binaryBytes = BitConverter.GetBytes((UInt32)val);
                        break;
                    case TypeCode.Single:
                        binaryBytes = BitConverter.GetBytes((float)val);
                        break;
                    case TypeCode.Double:
                        binaryBytes = BitConverter.GetBytes((double)val);
                        break;
                }
                if (binaryBytes == null)
                    return string.Empty;
                if (format == 2)
                {
                    return string.Join(" ", binaryBytes.Reverse().Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
                }
                else if (format == 16)
                {
                    return string.Join(" ", binaryBytes.Reverse().Select(b => b.ToString("X2")));
                }

            }
            return string.Empty;
        }



        private static List<DataItem> GetDataItemGroup(List<DataItem> dataItems)
        {
            var list = new List<DataItem>();
            var bitList = dataItems.Where(m => m.VarType == VarType.Bit).ToList();
            var otherList = dataItems.Where(m => m.VarType != VarType.Bit).ToList();
            foreach (var item in bitList)
            {
                if (!list.Exists(m => m.DB == item.DB && m.DataType == item.DataType && m.StartByteAdr == item.StartByteAdr))
                {
                    var newDataItem = new DataItem()
                    {
                        StartByteAdr = item.StartByteAdr,
                        DB = item.DB,
                        BitAdr = 0,
                        Count = 8,
                        DataType = item.DataType,
                        Value = item.Value,
                        VarType = item.VarType,
                    };
                    list.Add(newDataItem);
                }
            }
            list.AddRange(otherList);
            return list;
        }

        private void ToolStripMenuItem_ExcelExport_Click(object sender, EventArgs e)
        {
            var filename = lvwHandler.ExportSCV(tbx_ip.Text);
            if (string.IsNullOrEmpty(filename))
                return;
            this.OnMsg($"保存文件：{filename}");
            MessageBox.Show("保存文件成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.lv_data.Items.Clear();
        }
    }
}
