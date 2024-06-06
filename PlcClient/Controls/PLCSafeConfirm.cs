using NewLife.Log;
using PlcClient.Model;
using System;
using System.Windows.Forms;

namespace PlcClient.Controls
{
    public partial class PLCSafeConfirm : BaseControl
    {
        public PLCSafeConfirm()
        {
            InitializeComponent();

            cbx_isread.Text += $"（安全码：{AppConfig.Instance.SafeCode}）";
            this.tbx_pwd.Enabled = btn_ok.Enabled = false;
            this.cbx_isread.CheckedChanged += Cbx_isread_CheckedChanged;

            this.btn_ok.Click += Btn_ok_Click;
            this.btn_cancel.Click += Btn_cancel_Click;
        }

        private void Btn_cancel_Click(object sender, EventArgs e)
        {
            this.ParentForm.DialogResult = DialogResult.Cancel;
            this.ParentForm.Close();
        }

        private void Btn_ok_Click(object sender, EventArgs e)
        {
            if (tbx_pwd.Text.Equals(AppConfig.Instance.SafeCode))
            {
                XTrace.WriteLine("确认安全码" + tbx_pwd.Text);
                this.ParentForm.DialogResult = DialogResult.OK;
                AppConfig.Instance.SafeConfirm = true;
                return;
            }
            MessageBox.Show("安全码输入错误", "提示");
        }

        private void Cbx_isread_CheckedChanged(object sender, EventArgs e)
        {
            this.tbx_pwd.Enabled = btn_ok.Enabled = cbx_isread.Checked;
        }
    }
}
