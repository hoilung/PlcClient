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
            //tv_nodes.NodeMouseDoubleClick += Tv_nodes_NodeMouseDoubleClick;
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
            tv_nodes.SelectedNode = e.Node;
            var select_node = tv_nodes.SelectedNode;
            if (select_node.Nodes.Count == 1 && select_node.Nodes[0].Text == "loading...")
                return;

            var itemid = select_node.Tag as Opc.Da.BrowseElement;
            if (itemid == null)
            {
                return;
            }
            if (e.Button == MouseButtons.Right) //菜单
            {
                addToolStripMenuItem.Enabled = itemid.IsItem;
                addAllToolStripMenuItem.Enabled = !itemid.IsItem;

                addAllToolStripMenuItem.ToolTipText = itemid.ItemName;
                addToolStripMenuItem.ToolTipText = itemid.ItemName;

                contextMenuStrip1.Show(tv_nodes, e.Location);
                return;
            }
            if (!itemid.IsItem)
            {
                toolStripStatusLabel1.Text = $"标签：{itemid.ItemName} 节点数：{select_node.Nodes.Count}";
                return;
            }


            Task.Factory.StartNew(() =>
            {
                var readItem = new Item { ItemName = itemid.ItemName };
                var result = this.opc.Server.Read(new[] { readItem })[0];
                if (result != null)
                {
                    this.Invoke(() =>
                    {
                        this.propertyGrid1.SelectedObject = result;
                        var msg = $"标签：{readItem.ItemName} ";
                        if (select_node.Nodes.Count == 0 && itemid.IsItem)
                        {
                            msg += $"节点值：{result.Value} ";
                        }
                        if (select_node.Nodes.Count > 0)
                        {
                            msg += $"节点数：{select_node.Nodes.Count}";
                        }
                        toolStripStatusLabel1.Text = msg;
                    });
                }
            });


        }


        private void AddView(List<BrowseElement> items)
        {
            if (items.Count == 0) return;
            var lst = items.Select(m => new Item { ItemName = m.ItemName, ItemPath = m.ItemPath }).ToList();
            Task.Factory.StartNew(() =>
            {
                int pageSize = 100;
                int page = 1;
                while (page <= lst.Count / pageSize + 1)
                {
                    var readItems = lst.Skip((page - 1) * pageSize).Take(pageSize).ToArray();
                    var result = opc.Server.Read(readItems);
                    page++;
                    if (result != null)
                    {
                        foreach (var itemid in result)
                        {
                            var item = new OPCDAItem();
                            item.Name = itemid.ItemName;
                            item.Address = itemid.ItemName;
                            item.Value = itemid.Value;
                            item.ValueType = itemid.Value.GetType();
                            item.Quality = itemid.Quality.ToString();
                            item.Time = itemid.Timestamp.ToString();
                            this.Invoke(() =>
                            {
                                DataRefresh?.Invoke(item);
                            });
                        }
                    }
                }
            });
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
            BrowseFilters.MaxElementsReturned = 100;
            Task.Factory.StartNew(() =>
            {
                var nodes = server.Browse(itemID, BrowseFilters, out Opc.Da.BrowsePosition position);
                do
                {
                    if (!this.IsHandleCreated)
                        break;
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
                                                    //treeNode.ToolTipText = "双击添加子项或当前项到列表";
                                if (node.HasChildren)
                                    treeNode.Nodes.Add(new TreeNode("loading..."));
                            }
                        }
                        if (pNode.Nodes.Count > 0 && pNode.Nodes[0].Text == "loading...")
                        {
                            pNode.Nodes.RemoveAt(0);
                        }

                        pNode.ToolTipText = "节点已加载 " + pNode.Nodes.Count + " 个 \r\n" + pNode.FullPath;
                        if (position == null)
                        {
                            pNode.ToolTipText = $"节点已加载完成 共计：{pNode.Nodes.Count} 个\r\n" + pNode.FullPath;
                        }

                        tv_nodes.EndUpdate();

                    });
                    if (position != null)
                        nodes = server.BrowseNext(ref position);
                } while (position != null);
            });
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var select_node = tv_nodes.SelectedNode;
            var nodes = new List<BrowseElement>();
            var itemid = select_node.Tag as Opc.Da.BrowseElement;
            if (itemid != null && itemid.IsItem)
            {
                nodes.Add(itemid);
            }

            AddView(nodes);
        }

        private void addAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var select_node = tv_nodes.SelectedNode;
            var nodes = new List<BrowseElement>();

            foreach (TreeNode node in select_node.Nodes)
            {
                if (node.Nodes.Count > 0)
                    continue;
                var itemid = node.Tag as Opc.Da.BrowseElement;
                if (itemid != null && itemid.IsItem)
                    nodes.Add(itemid);
            }
            if (nodes.Count == 0)
            {
                toolStripStatusLabel1.Text = "没有可添加的项";
                return;
            }
            AddView(nodes);
        }
    }
}
