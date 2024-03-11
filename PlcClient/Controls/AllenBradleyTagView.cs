using HL.AllenBradley;
using S7.Net.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlcClient.Controls
{
    public partial class AllenBradleyTagView : BaseControl
    {
        public AllenBradleyTagView()
        {
            InitializeComponent();
            this.tableLayoutPanel1.Dock = DockStyle.Fill;
            tv_tag.Dock = DockStyle.Fill;
            lv_data.Dock = DockStyle.Fill;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            OnRefreshData();
        }

        public event Func<AbTagItem[]> RefreshDataEvent;

        public virtual void OnRefreshData()
        {
            var list = RefreshDataEvent?.Invoke();
            if (list != null)
            {
                AddTree(list);
            }
        }

        public void AddTree(AbTagItem[] dataItems)
        {

            tv_tag.StateImageList = imageList1;
            tv_tag.ImageList = imageList1;
            tv_tag.AfterSelect += Tv_tag_AfterSelect;

            tv_tag.SuspendLayout();
            var root = tv_tag.Nodes.Add("全局标签");
            root.SelectedImageIndex = root.ImageIndex = 0;
            TreeNodeAdd(root, dataItems);
            tv_tag.ResumeLayout();
            root.Toggle();
        }

        private void Tv_tag_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (tv_tag.SelectedNode != null && tv_tag.SelectedNode.Parent != null)
            {
                if (lv_data.Tag != null && lv_data.Tag == tv_tag.SelectedNode.Parent)//一样的定位选择
                {
                    var filterText = tv_tag.SelectedNode.Text;
                    var findItem = lv_data.FindItemWithText(filterText);
                    if (findItem != null)
                    {
                        //lv_data.Focus();
                        findItem.Selected = true;
                        lv_data.TopItem = findItem;
                    }
                    //for (int i = 0; i < lv_data.Items.Count; i++)
                    //{
                    //    if (lv_data.Items[i].SubItems[1].Text == filterText)
                    //    {
                    //        lv_data.Focus();
                    //        lv_data.TopItem = lv_data.Items[i];
                    //        break;
                    //    }
                    //}
                    return;
                }
                lv_data.Tag = tv_tag.SelectedNode.Parent;//用于替换刷新
                lv_data.BeginUpdate();
                lv_data.Items.Clear();
                var list = tv_tag.SelectedNode.Parent.Nodes;
                for (int i = 0; i < list.Count; i++)
                {
                    TreeNode item = list[i];
                    if (item.Tag is AbTagItem abTagItem)
                    {
                        var lvItem = lv_data.Items.Add($"{i}");
                        //lvItem.Tag = abTagItem;
                        lvItem.SubItems.Add(abTagItem.Name);
                        lvItem.SubItems.Add(abTagItem.GetTypeText());
                    }
                }
                lv_data.EndUpdate();
            }
        }


        private void TreeNodeAdd(TreeNode pnode, AbTagItem[] tagItems)
        {
            for (int i = 0; i < tagItems.Length; i++)
            {
                var item = tagItems[i];
                var p = pnode.Nodes.Add(item.Name);
                p.Tag = item;
                p.SelectedImageIndex = p.ImageIndex = item.IsStruct ? 2 : 0;
                if (item.ArrayDimension > 0)
                {
                    p.SelectedImageIndex = p.ImageIndex = 1;
                }
                if (item.Members != null && item.Members.Length > 0)//递归子标签
                {
                    TreeNodeAdd(p, tagItems[i].Members);
                }
            }
        }

        private void export_ExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "Save File(*.csv)|*.csv";
            fileDialog.Title = "保存文件";
            fileDialog.RestoreDirectory = true;
            fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            fileDialog.DefaultExt = "csv";
            fileDialog.FileName = "AbTag" + System.DateTime.Now.ToString("_yyyy-MM-dd_ffff");
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("序号,标签名称,标签类型");
                for (int i = 0; i < lv_data.Items.Count; i++)
                {
                    var item = lv_data.Items[i];
                    stringBuilder.AppendLine($"{item.SubItems[0].Text},{item.SubItems[1].Text},{item.SubItems[2].Text}");
                }

                File.WriteAllText(fileDialog.FileName, stringBuilder.ToString(), Encoding.Default);
                this.OnMsg($"保存文件：{fileDialog.FileName}");
                MessageBox.Show("保存文件成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
