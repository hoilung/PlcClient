using HL.OpcUa;
using NewLife.Log;
using Opc.Ua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlcClient.Controls
{
    public partial class OpcUaBrowseView : BaseControl
    {
        private readonly HL.OpcUa.UaClient driver;
        public List<ReferenceDescription> data = new List<ReferenceDescription>();

        public event Action<VariableNode, string> DataRefresh;
        public OpcUaBrowseView(UaClient driver)
        {
            InitializeComponent();
            this.tableLayoutPanel1.Dock = this.propertyGrid1.Dock = this.statusStrip1.Dock = this.tv_nodes.Dock = DockStyle.Fill;
            this.driver = driver;
            tv_nodes.NodeMouseDoubleClick += Tv_nodes_NodeMouseDoubleClick;
            tv_nodes.BeforeExpand += Tv_nodes_BeforeExpand;
            tv_nodes.NodeMouseClick += Tv_nodes_NodeMouseClick;

        }


        private void Tv_nodes_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Text == "loading...")
            {
                var item = e.Node.Tag as UaNode;
                if (item != null)
                {
                    this.GetTreeNode(e.Node, item.Tag);
                }
            }
        }
        private void Tv_nodes_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //if(!e.Node.IsExpanded)
            //    Tv_nodes_BeforeExpand(null, new TreeViewCancelEventArgs(e.Node, false,TreeViewAction.ByMouse));

            var item = e.Node.Tag as UaNode;
            if (item != null)
            {
                var nodeid = new NodeId(item.NodeId);
                var node = driver.Session.ReadNode(nodeid);
                if (node is Opc.Ua.VariableNode valnode)
                {
                    if ((valnode.AccessLevel & Opc.Ua.AccessLevels.CurrentRead) != 0)
                    {
                        valnode.Value = driver.Session.ReadValue(nodeid);
                        propertyGrid1.SelectedObject = valnode;
                        toolStripStatusLabel1.Text = $"预览节点：{valnode.DisplayName} 编号：{valnode.NodeId} 类型：{valnode.NodeClass} 值：{valnode.Value}";
                    }
                }
            }
        }
        private void Tv_nodes_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var node = e.Node.Tag as UaNode;
            if (node == null) return;
            if (node != null)
            {
                toolStripStatusLabel1.Text = $"查看节点：{node.Name} 编号：{node.NodeId} 类型：{node.NodeClass} 节点数：{e.Node.Nodes.Count}";
            }
            var nodes = new List<UaNode>();
            if (e.Node.Nodes.Count == 0 && node.NodeClass == NodeClass.Variable.ToString())
            {
                nodes.Add(node);
            }
            else if (!e.Node.IsExpanded)
            {
                foreach (TreeNode node2 in e.Node.Nodes)
                {
                    var item = node2.Tag as UaNode;
                    if (item != null)
                        nodes.Add(item);
                }
            }
            this.AddView(nodes);
        }

        private void AddView(List<UaNode> items)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (UaNode item in items)
                {
                    if (item != null && item.NodeClass == NodeClass.Variable.ToString())
                    {
                        try
                        {
                            var find = driver.FindNode(item.Tag);
                            var nodeid = new NodeId(find.NodeId);// (NodeId)item.NodeId;
                            var readnode = driver.Session.ReadNode(nodeid);
                            if (readnode is Opc.Ua.VariableNode valnode)
                            {
                                valnode.Value = driver.Session.ReadValue(nodeid);
                                this.Invoke(() =>
                                {
                                    DataRefresh?.Invoke(valnode, item.Tag);
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            XTrace.WriteException(ex);
                        }
                    }
                }
            });
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadData();
        }

        private void LoadData()
        {
            var rootNode = new TreeNode("根目录");
            try
            {
                GetTreeNode(rootNode, string.Empty);
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

        private void GetTreeNode(TreeNode pNode, string tag)
        {
            if (driver.Session == null || !driver.Session.Connected)
                return;
            pNode.ToolTipText = "双击添加子项或当前项到列表";
            Task.Factory.StartNew(() =>
            {
                var list = driver.ExploreFolder(tag);
                if (list != null)
                {
                    tv_nodes.Invoke(() =>
                    {
                        tv_nodes.BeginUpdate();
                        foreach (var item in list)
                        {
                            var node = pNode.Nodes.Add(item.Name);
                            node.Tag = item;
                            node.Nodes.Add(new TreeNode("loading..."));
                        }
                        if (pNode.Nodes.Count > 0 && pNode.Nodes[0].Text == "loading...")
                        {
                            pNode.Nodes.RemoveAt(0);
                        }
                        tv_nodes.EndUpdate();
                    });
                }
            });
        }
    }
}
