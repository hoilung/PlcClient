using HL.Object.Extensions;
using HL.S7netplus.Extensions;
using S7.Net;
using S7.Net.Types;
using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GEClient.Controls
{
    public partial class SiemensBase : BaseControl
    {
        private const string _addressVerdify = @"^(DB|AI|AQ|VB|VD|VW|M|I|Q|V)\d+[DBXWD\d\.]+[0-7]$";//地址验证
        private const string _ipVerdify = @"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";//ip地址验证

        public CpuType CpuType { get; set; }
        private TypeCode _typeCode;

        private Plc Plc { get; set; }

        private TypeCode[] TypeCodes { get; set; } = new[] {
            TypeCode.Boolean,
            TypeCode.Int16,
            TypeCode.Int32,
            //TypeCode.Int64,
            TypeCode.Single,
            TypeCode.Double,
            TypeCode.UInt16,
            TypeCode.UInt32,
            //TypeCode.UInt64
        };
        public SiemensBase()
        {
            InitializeComponent();
            this.Load += SiemensBase_Load;
            this.Disposed += SiemensBase_Disposed;
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
            foreach (var typeCode in TypeCodes)
            {
                RadioButton radio = new RadioButton();
                radio.Text = typeCode.ToString();
                radio.Checked = typeCode == TypeCode.Boolean;
                radio.AutoSize = true;
                radio.Tag = typeCode;
                radio.CheckedChanged += Radio_CheckedChanged;
                flowLayoutPanel1.Controls.Add(radio);
            }
            _typeCode = TypeCode.Boolean;//默认选择
            gb_set.Text = this.CpuType.ToString().Replace("S7", "S7-") + " " + gb_set.Text;
            tbx_adr.Text = "M2.3";

            Msg2Text($"适用于西门子PLC {this.CpuType.ToString().Replace("S7", "S7-")}");
            Msg2Text("\r\n" + Properties.Resources.tip);
        }

        private void Radio_CheckedChanged(object sender, EventArgs e)
        {
            var rbtn = (sender as RadioButton);
            if (rbtn != null && rbtn.Checked && rbtn.Tag != null && rbtn.Tag is TypeCode tc)
            {
                this._typeCode = tc;
            }
        }

        private void ChangeState(bool state)
        {
            this.btn_read.Enabled = this.btn_write.Enabled = this.btn_close.Enabled = state;

            this.btn_open.Enabled = !state;

            flowLayoutPanel1.Enabled = state;

        }

        private void btn_clearTbx_Click(object sender, EventArgs e)
        {
            this.tbx_msg.Clear();
        }

        private void btn_open_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Regex.IsMatch(tbx_ip.Text, _ipVerdify) || !Regex.IsMatch(tbx_port.Text, @"^\d+$") || !Regex.IsMatch(tbx_rack.Text, @"^\d+$") || !Regex.IsMatch(tbx_slot.Text, @"^\d+$"))
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
                    if (MessageBox.Show($"{ip}\r\n网络IP疑似不通,是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }

                Plc = new Plc(CpuType, ip, port, rack, slot);
                Plc.Open();

                ChangeState(Plc.IsConnected);
                tbx_msg.ResetText();
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
                Plc = null;
                OnMsg("连接关闭 " + Plc.IP);
                Msg2Text("连接关闭 " + Plc.IP);
            }
            catch (Exception ex)
            {
                OnMsg("关闭错误");
                Msg2Text(ex.Message);
            }
        }

        private Stopwatch stopwatch = Stopwatch.StartNew();
        private Ping ping = new Ping();
        private void btn_read_Click(object sender, EventArgs e)
        {
            var address = tbx_adr.Text.Trim().ToUpper();
            if (string.IsNullOrWhiteSpace(address) || !Regex.IsMatch(address, _addressVerdify))
            {
                MessageBox.Show($"{address} 无效的PLC地址");
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
                var dt = DataItem2.FromAddressByTypeCode(address, _typeCode);
                stopwatch.Restart();
                var obj = this.Plc.Read(dt);
                stopwatch.Stop();
                //转换类型

                var obj2 = DataItem2.ValueChangeType(obj, _typeCode);

                //var obj2 = ChageType(obj);

                OnMsg($"用时：{stopwatch.Elapsed.TotalMilliseconds.ToString("0.000ms")}");
                Msg2Text($"{address}\t{obj2?.ToString()}\t用时：{stopwatch.Elapsed.TotalMilliseconds.ToString("0.000ms")}");
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

                Plc.Write(dt);
            }
            catch (Exception ex)
            {
                Msg2Text(adr + " " + ex.Message);
                MessageBox.Show(ex.Message, "提示");
            }
        }



    }
}
