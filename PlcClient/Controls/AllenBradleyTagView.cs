using HL.AllenBradley;
using PlcClient.Handler;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PlcClient.Controls
{
    public partial class AllenBradleyTagView : BaseControl
    {
        private ListViewHandler lvwHandler;
        public AllenBradleyTagView()
        {
            InitializeComponent();
            this.tableLayoutPanel1.Dock = DockStyle.Fill;
            tv_tag.Dock = DockStyle.Fill;
            lv_data.Dock = DockStyle.Fill;

            lvwHandler = new ListViewHandler(lv_data);
            lvwHandler.ColuminSort();
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            OnRefreshData();
        }
        /// <summary>
        /// 刷新数据，初始化树结构
        /// </summary>
        public event Func<AbTagItem[]> RefreshDataEvent;
        /// <summary>
        /// 列表选中，读取地址对应的数值
        /// </summary>
        public event Action<AbDataItem> AddressReadEvent;

        public event Action<AbTagItem> AddTreeNode;

        public virtual void OnRefreshData()
        {
            var list = RefreshDataEvent?.Invoke();
            if (list != null)
            {
                tv_tag.StateImageList = imageList1;
                tv_tag.ImageList = imageList1;
                tv_tag.AfterSelect += Tv_tag_AfterSelect;
                tv_tag.DoubleClick += Tv_tag_DoubleClick;
                tv_tag.SuspendLayout();

                var root = tv_tag.Nodes.Add("全局标签");
                root.SelectedImageIndex = root.ImageIndex = 0;
                TreeNodeAdd(root, list);

                tv_tag.ResumeLayout();
                root.Toggle();
            }
        }

        private void Tv_tag_DoubleClick(object sender, EventArgs e)
        {
            if (tv_tag.SelectedNode.Nodes.Count == 0 && tv_tag.SelectedNode.Tag is AbTagItem abTag && abTag.IsStruct)
            {
                AddTreeNode?.Invoke(abTag);
                if (abTag.Members != null)
                {
                    TreeNodeAdd(tv_tag.SelectedNode, abTag.Members);
                }

            }
        }

        public virtual void OnAddressRead(AbDataItem address)
        {
            this.AddressReadEvent?.Invoke(address);
        }

        private void Tv_tag_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (tv_tag.SelectedNode != null && tv_tag.SelectedNode.Parent != null)
            {
                if (lv_data.Tag != null && lv_data.Tag == tv_tag.SelectedNode.Parent)//树层级一样的，定位选择 listview
                {
                    var filterText = tv_tag.SelectedNode.Text;
                    var findItem = lv_data.FindItemWithText(filterText);
                    if (findItem != null)
                    {
                        findItem.Selected = true;
                        lv_data.TopItem = findItem;
                    }
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
                        lvItem.SubItems[0].Tag = i;//后期用于数字排序
                        lvItem.SubItems.Add(abTagItem.Name);
                        lvItem.SubItems.Add(abTagItem.GetTypeText());
                        lvItem.SubItems.Add(string.Empty);
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
                // p.ToolTipText = string.IsNullOrEmpty(pnode.ToolTipText) ? item.Name : string.Format("{0}.{1}", pnode.ToolTipText, item.Name);
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
            var filename = lvwHandler.ExportExcel("AbTag");
            if (string.IsNullOrEmpty(filename))
                return;
            this.OnMsg($"保存文件：{filename}");
            MessageBox.Show("保存文件成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void readSelect_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lv_data.SelectedItems.Count > 0)
            {
                var text = lv_data.SelectedItems[0].SubItems[1].Text;
                var address = string.Format("{0}.{1}", tv_tag.SelectedNode.Parent.FullPath.Replace("\\", "."), text).Replace("全局标签.", "");
                var dataType = lv_data.SelectedItems[0].SubItems[2].Text;

                ushort len = 1;
                var mc = Regex.Match(dataType, @"(?<=\[)\d+(?=\])");
                if (mc.Success)
                {
                    len = ushort.Parse(mc.Value);
                    dataType = dataType.Replace($"[{len}]", "");
                }
                dataType = dataType.Replace("Array", string.Empty);

                if (!Enum.GetNames(typeof(ValType)).Contains(dataType))
                {
                    MessageBox.Show($"{dataType} 数据类型尚未支持直接查询", "提示");
                    return;
                }

                var varType = (ValType)Enum.Parse(typeof(ValType), dataType);
                lv_data.SelectedItems[0].SubItems[3].Text = string.Empty;
                var abDataItem = new AbDataItem()
                {
                    Address = address,
                    ValType = varType,
                    Length = varType == ValType.Boolean ? (ushort)1 : len,
                };
                OnAddressRead(abDataItem);
                lv_data.SelectedItems[0].SubItems[3].Text = abDataItem.ToString();

            }
        }

        private void copySelect_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lv_data.SelectedItems.Count == 0)
                return;
            try
            {
                Clipboard.SetText($"{lv_data.SelectedItems[0].SubItems[1].Text}\t{lv_data.SelectedItems[0].SubItems[2].Text}\t{lv_data.SelectedItems[0].SubItems[3].Text}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "复制失败");
            }
        }
    }
}
