using HL.OpcDa;
using Opc.Da;
using PlcClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlcClient.Controls
{
    public partial class OpcDaBrowseView : BaseControl
    {
        private Opc.Da.BrowseFilters BrowseFilters = new Opc.Da.BrowseFilters();

        private Opc.ItemIdentifier itemID = new Opc.ItemIdentifier();
        private readonly OpcDaDriver opc;

        public event Action<OPCDAItem> DataRefresh;
        public OpcDaBrowseView(OpcDaDriver Opc)
        {
            InitializeComponent();
            tableLayoutPanel1.Dock = this.statusStrip1.Dock = this.tv_nodes.Dock = DockStyle.Fill;
            tv_nodes.NodeMouseDoubleClick += Tv_nodes_NodeMouseDoubleClick;
            tv_nodes.BeforeExpand += Tv_nodes_BeforeExpand;
            opc = Opc;
        }

        private void findChliditem(TreeNode treeNode)
        {
            if (treeNode.Nodes.Count == 0) { return; }
            foreach (TreeNode node in treeNode.Nodes)
            {
                var itemid = node.Tag as Opc.Da.BrowseElement;
                if (itemid == null || itemid.HasChildren)
                    continue;
                var item = new OPCDAItem();
                item.Name = itemid.Name;
                item.Address = itemid.ItemName;

                item.ValueType = itemid.Properties.FirstOrDefault(m => m.ID == new Opc.Da.PropertyID(1)).Value as Type;
                item.Value = itemid.Properties.FirstOrDefault(m => m.ID == new Opc.Da.PropertyID(2)).Value;
                item.Quality = itemid.Properties.FirstOrDefault(m => m.ID == new Opc.Da.PropertyID(3)).Value.ToString();
                item.Time = itemid.Properties.FirstOrDefault(m => m.ID == new Opc.Da.PropertyID(4)).Value.ToString();

                //toolStripStatusLabel1.Text = $"添加节点：{item.Address} 节点值：{item.Value}";
                DataRefresh?.Invoke(item);
                //}
            }
        }

        private void Tv_nodes_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Text == "loading...")
            {

                var itemid = e.Node.Tag as Opc.Da.BrowseElement;
                if (itemid != null && itemid.HasChildren)
                {
                    GetTreeNode(e.Node, opc.Server);
                    return;
                }

            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadData();
        }


        public void LoadData()
        {
            var rootNode = new TreeNode("根目录");
            try
            {
                GetTreeNode(rootNode, opc.Server);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "获取服务器目录失败");
            }
            tv_nodes.BeginUpdate();
            tv_nodes.Nodes.Clear();
            tv_nodes.Nodes.Add(rootNode);
            rootNode.Expand();
            tv_nodes.EndUpdate();
        }

        private void Tv_nodes_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Nodes.Count > 0)
            {
                findChliditem(e.Node);
                return;
            }
            var itemid = e.Node.Tag as Opc.Da.BrowseElement;
            if (itemid == null)
                return;
            if (itemid.Properties == null && !itemid.IsItem)
            {
                toolStripStatusLabel1.Text = itemid.ItemName + " 当前节点不支持读取";
                return;
            }
            if (itemid.Properties == null)
            {
                toolStripStatusLabel1.Text = itemid.ItemName + " 当前节点读取失败";
                return;
            }


            var item = new OPCDAItem();
            item.Name = itemid.Name;
            item.Address = itemid.ItemName;

            item.ValueType = itemid.Properties.FirstOrDefault(m => m.ID == new Opc.Da.PropertyID(1)).Value as Type;
            item.Value = itemid.Properties.FirstOrDefault(m => m.ID == new Opc.Da.PropertyID(2)).Value;
            item.Quality = itemid.Properties.FirstOrDefault(m => m.ID == new Opc.Da.PropertyID(3)).Value.ToString();
            item.Time = itemid.Properties.FirstOrDefault(m => m.ID == new Opc.Da.PropertyID(4)).Value.ToString();

            toolStripStatusLabel1.Text = $"添加节点：{item.Address} 节点值：{item.Value}";
            DataRefresh?.Invoke(item);

        }




        private void GetTreeNode(TreeNode pNode, Opc.Da.Server server)
        {
            if (server == null)
                return;
            if (!server.IsConnected)
                return;

            var item = pNode.Tag as Opc.Da.BrowseElement;
            if (item != null)
            {
                itemID.ItemName = item.ItemName;
                itemID.ItemPath = item.ItemPath;
            }
            BrowseFilters.ReturnPropertyValues = true;
            BrowseFilters.ReturnAllProperties = true;

            Task.Factory.StartNew(() =>
            {
                var nodes = server.Browse(itemID, BrowseFilters, out Opc.Da.BrowsePosition position);
                tv_nodes.Invoke(() =>
                {
                    tv_nodes.BeginUpdate();
                    for (var i = 0; i < nodes.Length; i++)
                    {
                        var node = nodes[i];
                        var treeNode = pNode.Nodes.Add(node.Name);
                        treeNode.Tag = node;// new Opc.ItemIdentifier { ItemName = node.ItemName, ItemPath = node.ItemPath };                                                                
                        treeNode.ToolTipText = node.ItemPath;
                        if (node.HasChildren)
                            treeNode.Nodes.Add(new TreeNode("loading..."));

                    }
                    if (pNode.Nodes.Count > 0 && pNode.Nodes[0].Text == "loading...")
                    {
                        pNode.Nodes.RemoveAt(0);
                    }
                    tv_nodes.EndUpdate();
                });
            });
        }
    }
}
