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
            tableLayoutPanel1.Dock = this.propertyGrid1.Dock = this.statusStrip1.Dock = this.tv_nodes.Dock = DockStyle.Fill;
            tv_nodes.NodeMouseDoubleClick += Tv_nodes_NodeMouseDoubleClick;
            tv_nodes.BeforeExpand += Tv_nodes_BeforeExpand;
            tv_nodes.NodeMouseClick += Tv_nodes_NodeMouseClick;
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

        private void Tv_nodes_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Text == "loading...")
            {
                var itemid = e.Node.Tag as Opc.Da.BrowseElement;
                if (itemid != null && itemid.HasChildren)
                {
                    GetTreeNode(e.Node, opc.Server);
                }

            }
        }

        private void Tv_nodes_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var itemid = e.Node.Tag as Opc.Da.BrowseElement;
            if (itemid != null)
            {
                this.propertyGrid1.SelectedObject = itemid;
                var readItem = new Item { ItemName = itemid.ItemName };
                var result = this.opc.Server.Read(new[] { readItem })[0];
                if (result != null)
                {
                    this.propertyGrid1.SelectedObject = result;
                    toolStripStatusLabel1.Text = $"添加节点：{readItem.ItemName} 节点值：{result.Value}";
                }

            }
        }
        private void Tv_nodes_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var nodes = new List<BrowseElement>();
            if (e.Node.Nodes.Count > 0 && !e.Node.IsExpanded)
            {
                foreach (TreeNode node in e.Node.Nodes)
                {
                    var itemid = node.Tag as Opc.Da.BrowseElement;
                    if (itemid != null && itemid.IsItem)
                        nodes.Add(itemid);
                }
            }
            else
            {
                var itemid = e.Node.Tag as Opc.Da.BrowseElement;
                if (itemid != null && itemid.IsItem)
                {
                    nodes.Add(itemid);
                }
            }
            AddView(nodes);

        }

        private void AddView(List<BrowseElement> items)
        {
            if (items.Count == 0) return;
            var readItems = items.Select(m => new Item { ItemName = m.ItemName }).ToArray();
            Task.Factory.StartNew(() =>
            {
                var result = opc.Server.Read(readItems);
                foreach (var itemid in result)
                {
                    var item = new OPCDAItem();
                    item.Name = itemid.ItemName;
                    item.Address = itemid.ItemName;                    
                    item.Value = itemid.Value;
                    item.ValueType=itemid.Value.GetType();
                    item.Quality = itemid.Quality.ToString();
                    item.Time = itemid.Timestamp.ToString();
                    DataRefresh?.Invoke(item);
                }


            });
            //foreach (var itemid in items)
            //{
            //    var item = new OPCDAItem();
            //    item.Name = itemid.Name;
            //    item.Address = itemid.ItemName;

            //    item.ValueType = itemid.Properties.FirstOrDefault(m => m.ID == new Opc.Da.PropertyID(1)).Value as Type;
            //    item.Value = itemid.Properties.FirstOrDefault(m => m.ID == new Opc.Da.PropertyID(2)).Value;
            //    item.Quality = itemid.Properties.FirstOrDefault(m => m.ID == new Opc.Da.PropertyID(3)).Value.ToString();
            //    item.Time = itemid.Properties.FirstOrDefault(m => m.ID == new Opc.Da.PropertyID(4)).Value.ToString();
            //    DataRefresh?.Invoke(item);

            //}
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
            //BrowseFilters.ReturnPropertyValues = true;
            //BrowseFilters.ReturnAllProperties = true;

            Task.Factory.StartNew(() =>
            {
                var nodes = server.Browse(itemID, new BrowseFilters(), out Opc.Da.BrowsePosition position);

                tv_nodes.Invoke(() =>
                {
                    tv_nodes.BeginUpdate();
                    if (nodes != null)
                    {
                        for (var i = 0; i < nodes.Length; i++)
                        {
                            var node = nodes[i];
                            var treeNode = pNode.Nodes.Add(node.Name);
                            treeNode.Tag = node;// new Opc.ItemIdentifier { ItemName = node.ItemName, ItemPath = node.ItemPath };                                                                
                            treeNode.ToolTipText = "双击添加子项或当前项到列表";
                            if (node.HasChildren)
                                treeNode.Nodes.Add(new TreeNode("loading..."));

                        }
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
