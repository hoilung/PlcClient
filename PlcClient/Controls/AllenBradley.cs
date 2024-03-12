using HL.AllenBradley;
using HL.Object.Extensions;
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
    public partial class AllenBradley : BaseControl
    {
        private const string _ipVerdify = @"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";//ip地址验证
        private AllenBradleyNet plc;
        private TypeCode _typeCode = TypeCode.Empty;
        public AllenBradley()
        {
            InitializeComponent();
            Init();
        }
        public override TypeCode[] TypeCodes => new[] {
            TypeCode.Boolean,
            TypeCode.Byte,
            TypeCode.Int16,
            TypeCode.Int32,
            TypeCode.Int64,
            TypeCode.Single,
            TypeCode.Double,
            TypeCode.UInt16,
            TypeCode.UInt32,
            TypeCode.UInt64,
            TypeCode.String,
        };

        private void Init()
        {
            tbx_ip.Text = GetLocalIP();

            ChangeState(false);
            tbx_value.ReadOnly = true;
            chk_enablewrite.Checked = false;

            var typeArry = TypeCodes.Select(m => new { Name = m, Value = m.ToString() }).ToList();
            cbx_type.DisplayMember = "Value";
            cbx_type.ValueMember = "Name";
            cbx_type.DataSource = typeArry;
            _typeCode = TypeCode.Boolean;//默认选择
            cbx_type.SelectedIndexChanged += Cbx_type_SelectedIndexChanged;

            tbx_address.Text = Properties.Resources.ab_tip;
        }

        private void Cbx_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            _typeCode = (TypeCode)cbx_type.SelectedValue;
        }

        private void ChangeState(bool state)
        {
            btn_read.Enabled = btn_add.Enabled = btn_readOne.Enabled = chk_enablewrite.Enabled = btn_write.Enabled = btn_close.Enabled = state;
            btn_open.Enabled = !state;
            btn_tagview.Enabled = state;
            tbx_value.ResetText();
            if (!state)
            {
                chk_enablewrite.Checked = false;
            }

        }
        private void btn_open_Click(object sender, EventArgs e)
        {
            var ip = tbx_ip.Text;
            var port = ushort.Parse(tbx_port.Text);
            var slot = byte.Parse(tbx_slot.Text);
            if (!Regex.IsMatch(ip, _ipVerdify))
            {
                MessageBox.Show($"{ip} 无效的IP地址");
                tbx_ip.Focus();
                return;
            }

            var result = ping.Send(ip, 500);
            if (result.Status != IPStatus.Success)
            {
                if (MessageBox.Show($"{ip}\r\n网络PING疑似不通,是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
            }
            plc = new AllenBradleyNet(ip, port, slot);
            plc.Open();
            if (plc.Connected)
            {
                ChangeState(true);
                tbx_address.ResetText();
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            plc?.Close();
            ChangeState(false);

        }

        private void chk_enablewrite_CheckedChanged(object sender, EventArgs e)
        {
            btn_write.Enabled = chk_enablewrite.Checked;
            tbx_value.ReadOnly = !chk_enablewrite.Checked;
        }

        private void btn_readOne_Click(object sender, EventArgs e)
        {
            var adr = tbx_addressOne.Text;
            try
            {
                tbx_value.ResetText();
                object obj = null;
                stopwatch.Restart();
                if (_typeCode == TypeCode.Boolean)
                {
                    obj = plc.ReadBoolean(adr);
                }
                else
                {
                    var by = plc.Read(adr, 1);
                    if (by != null)
                    {
                        obj = ConvertValueType(by, _typeCode, 0);
                    }
                }
                stopwatch.Stop();
                OnMsg($"{adr} 读取 {obj}，用时：" + stopwatch.Elapsed.TotalMilliseconds.ToString("0.000ms"));
                tbx_value.Text = obj.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "失败");
            }

        }

        public static object ConvertValueType(byte[] bytes, TypeCode typeCode, int offset = 0)
        {
            switch (typeCode)
            {
                case TypeCode.Boolean: return BitConverter.ToBoolean(bytes, offset);
                case TypeCode.Byte: return bytes[0];
                case TypeCode.Char: return BitConverter.ToChar(bytes, offset);
                case TypeCode.Int16: return BitConverter.ToInt16(bytes, offset);
                case TypeCode.Int32: return BitConverter.ToInt32(bytes, offset);
                case TypeCode.Int64: return BitConverter.ToInt64(bytes, offset);
                case TypeCode.Single: return BitConverter.ToSingle(bytes, offset);
                case TypeCode.Double: return BitConverter.ToDouble(bytes, offset);
                case TypeCode.SByte: return Convert.ToSByte(bytes[0]);
                case TypeCode.UInt16: return BitConverter.ToUInt16(bytes, offset);
                case TypeCode.UInt32: return BitConverter.ToUInt32(bytes, offset);
                case TypeCode.UInt64: return BitConverter.ToUInt64(bytes, offset);
                case TypeCode.String:
                    int count = BitConverter.ToUInt16(bytes, 2);
                    return Encoding.UTF8.GetString(bytes, 6, count);
                default: return "";
            }

        }

        private void btn_write_Click(object sender, EventArgs e)
        {
            var adr = tbx_addressOne.Text.Trim();

            try
            {
                var typecode = (TypeCode)cbx_type.SelectedValue;
                var val = tbx_value.Text.Trim().ConvertToValueType(typecode);
                stopwatch.Restart();
                switch (typecode)
                {

                    case TypeCode.Boolean:
                        plc.WriteBoolean(adr, (bool)val);
                        break;
                    case TypeCode.Byte:
                        plc.WriteByte(adr, (byte)val);
                        break;
                    case TypeCode.Int16:
                        plc.WriteInt16(adr, (Int16)val);
                        break;
                    case TypeCode.UInt16:
                        plc.WriteUInt16(adr, (UInt16)val);
                        break;
                    case TypeCode.Int32:
                        plc.WriteInt32(adr, (Int32)val);
                        break;
                    case TypeCode.UInt32:
                        plc.WriteUInt32(adr, (UInt32)val);
                        break;
                    case TypeCode.Int64:
                        plc.WriteInt64(adr, (Int64)val);
                        break;
                    case TypeCode.UInt64:
                        plc.WriteUInt64(adr, (UInt64)val);
                        break;
                    case TypeCode.Single:
                        plc.WriteSingle(adr, (Single)val);
                        break;
                    case TypeCode.Double:
                        plc.WriteDouble(adr, (Double)val);
                        break;
                    case TypeCode.String:
                        plc.WriteString(adr, tbx_value.Text.Trim());
                        break;
                    default:
                        MessageBox.Show("尚未支持的类型");
                        break;
                }
                stopwatch.Stop();
                OnMsg($"{adr} 写入 {val}，用时：" + stopwatch.Elapsed.TotalMilliseconds.ToString("0.000ms"));
            }
            catch (Exception ex)
            {
                OnMsg($"{adr} 写入失败，用时：" + stopwatch.Elapsed.TotalMilliseconds.ToString("0.000ms"));
                MessageBox.Show(ex.Message, "失败");
                //OnMsg(ex.Message);
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            var line = tbx_address.Lines.Where(m => !string.IsNullOrWhiteSpace(m));
            var adrErr = line.FirstOrDefault(m => !Regex.IsMatch(m.Split(new[] { "\t", " ", "|" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(), @"^[\w\[\]]+"));

            var list = line.Distinct().ToList();

            var arr = new[] { "BOOL", "BYTE", "INT", "DINT", "WORD", "DWORD", "REAL", "LREAL" };
            var dataItems = new List<AbDataItem>();
            for (int i = 0; i < list.Count; i++)
            {
                var li = list[i].ToUpper().Split(new[] { "\t", " ", "|" }, StringSplitOptions.RemoveEmptyEntries);

                if (li.Length == 2 && arr.Contains(li[1]))
                {
                    AbDataItem item = new AbDataItem();
                    item.Address = li[0];
                    switch (li[1])
                    {
                        case "BOOL":
                            item.ValType = ValType.Boolean;
                            break;
                        case "BYTE":
                            item.ValType = ValType.Byte;
                            break;
                        case "INT":
                            item.ValType = ValType.Int16;
                            break;
                        case "DINT":
                            item.ValType = ValType.Int32;
                            break;
                        case "WORD":
                            item.ValType = ValType.UInt16;
                            break;
                        case "DWORD":
                            item.ValType = ValType.UInt32;
                            break;
                        case "REAL":
                            item.ValType = ValType.Single;
                            break;
                        case "LREAL":
                            item.ValType = ValType.Double;
                            break;
                    }
                    dataItems.Add(item);
                }
            }

            lv_data.BeginUpdate();
            lv_data.Items.Clear();
            for (int i = 0; i < dataItems.Count; i++)
            {
                AbDataItem data = dataItems[i];
                var item = new ListViewItem(i.ToString());
                item.Tag = data;
                item.SubItems.Add(data.Address.ToString());
                item.SubItems.Add(data.ValType.ToString());
                item.SubItems.Add(data.Value?.ToString());

                lv_data.Items.Add(item);
            }
            lv_data.EndUpdate();
            lv_data.Tag = dataItems;



        }

        private void lv_data_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lv_data.SelectedItems.Count > 0)
            {
                var item = lv_data.SelectedItems[0].Tag as AbDataItem;
                if (item != null)
                {
                    tbx_addressOne.Text = item.Address.ToString();
                    cbx_type.Text = item.ValType.ToString();
                }
            }
        }

        private void btn_read_Click(object sender, EventArgs e)
        {
            if (plc.Connected && lv_data.Tag is List<AbDataItem> dataItems)
            {
                try
                {
                    stopwatch.Restart();
                    //plc.ReadMultiple(dataItems.ToArray());
                    for (int i = 0; i < dataItems.Count; i += 20)
                    {
                        plc.ReadMultiple(dataItems.Skip(i).Take(20).ToArray());
                    }
                    stopwatch.Stop();
                    OnMsg($"批量读取 {dataItems.Count}个,用时：" + stopwatch.Elapsed.TotalMilliseconds.ToString("0.000ms"));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "读取失败");
                    this.btn_close_Click(null, null);
                }
                //connectedCipNet.Read(dataItems.ToArray());
                foreach (ListViewItem item in lv_data.Items)
                {
                    if (item.Tag is AbDataItem data)
                    {
                        item.SubItems[3].Text = data.ToString();
                    }

                }

            }
        }

        private void tm_exportExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "Save File(*.csv)|*.csv";
            fileDialog.Title = "保存文件";
            fileDialog.RestoreDirectory = true;
            fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            fileDialog.DefaultExt = "csv";
            fileDialog.FileName = tbx_ip.Text + DateTime.Now.ToString("_yyyy-MM-dd_ffff");
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("序号,地址,数据类型,数值");
                //var array = lv_data.Tag as List<GEDataItem>;
                for (int i = 0; i < lv_data.Items.Count; i++)
                {
                    var item = lv_data.Items[i];
                    if (item.Tag is AbDataItem dataItem)
                    {
                        stringBuilder.AppendLine($"{item.Text},{dataItem.Address},{dataItem.ValType},{dataItem.ToString()}");
                    }
                }
                File.WriteAllText(fileDialog.FileName, stringBuilder.ToString(), Encoding.Default);
                this.OnMsg($"保存文件：{fileDialog.FileName}");
                MessageBox.Show("保存文件成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_tagview_Click(object sender, EventArgs e)
        {

            var tagForm = new Form()
            {
                Height = 450,
                Width = 600,
                Text = "查看标签",
                Icon = Properties.Resources.ab,
                StartPosition = FormStartPosition.CenterParent
            };
            tagForm.MinimumSize = new System.Drawing.Size(700, 450);
            var abTagView = new AllenBradleyTagView();
            abTagView.RefreshDataEvent += AbTagView_RefreshDataEvent;
            abTagView.AddressReadEvent += AbTagView_AddressReadEvent;
            abTagView.AddTreeNode += AbTagView_AddTreeNode;
            abTagView.Dock = DockStyle.Fill;
            tagForm.Controls.Add(abTagView);
            tagForm.ShowDialog();

        }

        private void AbTagView_AddTreeNode(AbTagItem arg1)
        {
            arg1.Members = plc.StructTagEnumeator(arg1);
            //if (arg1.Members != null)
            //{
            //    foreach (var member in arg1.Members)
            //    {
            //        arg2.Nodes.Add(member.Name).Tag = member;
            //    }
            //}
        }

        private void AbTagView_AddressReadEvent(AbDataItem AbDataItem)
        {
            plc.Read(AbDataItem);//读取列表选中的地址
            //return "sss";
        }

        private AbTagItem[] AbTagView_RefreshDataEvent()//首次加载树结构
        {
            var list = plc.TagEnumerator();
            var structList = list.Where(m => m.IsStruct).ToList();
            for (int i = 0; i < structList.Count; i++)
            {
                structList[i].Members = plc.StructTagEnumeator(structList[i]);
            }
            return list;
        }
    }


}
