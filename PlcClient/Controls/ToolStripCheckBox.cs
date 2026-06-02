using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace PlcClient.Controls
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
    [DefaultProperty("Items")]
    public partial class ToolStripCheckBox : ToolStripControlHost
    {
        private CheckBox _checkBox;
        public CheckBox CheckBox => _checkBox;
        public ToolStripCheckBox() : base(CreateControlInstance())
        {
            _checkBox = Control as CheckBox;
            _checkBox.CheckedChanged += (sender, args) =>
            {
                OnCheckedChanged(EventArgs.Empty);
            };

        }
        public ToolStripCheckBox(string text) : base(CreateControlInstance())
        {
            _checkBox = Control as CheckBox;
            _checkBox.Text = text;
            _checkBox.CheckedChanged += CheckBox_CheckedChanged;
        }

        private static Control CreateControlInstance()
        {
            var checkBox = new CheckBox
            {
                //Appearance = Appearance.Normal,
                AutoSize = true,
                //TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(1, 2, 1, 0),
                Text = "CheckBox"
            };
            return checkBox;
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OnCheckedChanged(EventArgs.Empty);
        }

        public bool Checked
        {
            get { return _checkBox.Checked; }
            set { _checkBox.Checked = value; }
        }

        public string Text
        {
            get { return _checkBox.Text; }
            set { _checkBox.Text = value; }
        }

        public event EventHandler CheckedChanged;

        protected virtual void OnCheckedChanged(EventArgs e)
        {
            CheckedChanged?.Invoke(this, e);
        }

        // 重写 Size 属性以适应内容
        public override Size Size
        {
            get
            {
                return base.Size;
            }
            set
            {
                base.Size = value;
            }
        }

    }
}
