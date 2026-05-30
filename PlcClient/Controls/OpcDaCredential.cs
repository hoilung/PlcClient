using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlcClient.Controls
{
    public partial class OpcDaCredential : UserControl
    {
        private OpcDaCredentialModel Model;
        public OpcDaCredential()
        {
            InitializeComponent();
            this.Dock=DockStyle.Fill;
            this.Model = OpcDaCredentialModel.GetDefault();            
            tbx_username.DataBindings.Add("Text", this.Model, nameof(this.Model.UserName));
            tbx_password.DataBindings.Add("Text", this.Model, nameof(this.Model.Password));
            tbx_domain.DataBindings.Add("Text", this.Model, nameof(this.Model.Domain));
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
        public NetworkCredential GetNetworkCredential()
        {
            return new NetworkCredential(this.Model.UserName, this.Model.Password, this.Model.Domain);
        }
        public OpcDaCredentialModel GetModel() { return this.Model;}

        private void btn_ok_Click(object sender, EventArgs e)
        {
            this.FindForm().DialogResult = DialogResult.Yes;
        }
    }

    public class OpcDaCredentialModel: INotifyPropertyChanged
    {
        [DisplayName("用户名")]
        public string UserName { get; set; } = string.Empty;

        [DisplayName("用户名")]
        public string Password { get; set; } = string.Empty;

        [DisplayName("域")]
        public string Domain { get; set; } = string.Empty;

        public event PropertyChangedEventHandler PropertyChanged;

        public static OpcDaCredentialModel GetDefault()
        {
            return new OpcDaCredentialModel()
            {
                UserName = "",
                Password = "",
                Domain = ""
            };
        }
    }


}
