using HL.OpcDa;
using Opc.Da;
using PlcClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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
            opc = Opc;
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
            if (tv_nodes.SelectedNode.Nodes.Count > 0)
            {
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
            if(itemid.Properties==null)
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
            var nodes = server.Browse(itemID, BrowseFilters, out Opc.Da.BrowsePosition position);

            if (nodes == null)
            {
                return;
            }
            for (var i = 0; i < nodes.Length; i++)
            {
                var node = nodes[i];
                var treeNode = pNode.Nodes.Add(node.Name);
                treeNode.Tag = node;// new Opc.ItemIdentifier { ItemName = node.ItemName, ItemPath = node.ItemPath };
                if (node.HasChildren)
                {
                    GetTreeNode(treeNode, server);
                }
            }

        }
    }
}
