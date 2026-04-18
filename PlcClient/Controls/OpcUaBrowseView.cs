using HL.OpcUa;
using NewLife.Log;
using Opc.Ua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;
using static OpenCvSharp.ML.DTrees;

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
            //tv_nodes.NodeMouseDoubleClick += Tv_nodes_NodeMouseDoubleClick;
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
            var selectNode = tv_nodes.SelectedNode;
            if (selectNode.Nodes.Count == 1 && selectNode.Nodes[0].Text == "loading...")
                return;
            var item = selectNode.Tag as UaNode;
            if (item == null)
                return;
            if (e.Button == MouseButtons.Right)
            {
                addToolStripMenuItem.Enabled = item.NodeClass == NodeClass.Variable.ToString();
                addAllToolStripMenuItem.Enabled = !addToolStripMenuItem.Enabled | selectNode.Nodes.Count > 0;

                addToolStripMenuItem.ToolTipText = item.Tag;
                addAllToolStripMenuItem.ToolTipText = item.Tag;

                contextMenuStrip1.Show(tv_nodes, e.Location);
                return;
            }
            if (selectNode.Nodes.Count > 0)
                toolStripStatusLabel1.Text = $"查看节点：{item.Name} 编号：{item.NodeId} 类型：{item.NodeClass} 节点数：{selectNode.Nodes.Count}";

            Task.Factory.StartNew(() =>
            {
                var nodeid = new NodeId(item.NodeId);
                var node = driver.Session.ReadNode(nodeid);
                if (node is Opc.Ua.VariableNode valnode)
                {
                    if ((valnode.AccessLevel & Opc.Ua.AccessLevels.CurrentRead) != 0)
                    {
                        valnode.Value = driver.Session.ReadValue(nodeid);
                        this.Invoke(() =>
                        {
                            propertyGrid1.SelectedObject = valnode;
                            toolStripStatusLabel1.Text = $"预览节点：{valnode.DisplayName} 编号：{valnode.NodeId} 类型：{valnode.NodeClass} 值：{valnode.Value}";
                        });
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
            tv_nodes.Nodes.Add(rootNode);
            rootNode.Expand();
            tv_nodes.EndUpdate();

        }

        private void GetTreeNode(TreeNode pNode, string tag)
        {
            if (driver.Session == null || !driver.Session.Connected)
                return;
            Task.Factory.StartNew(() =>
            {
                if (driver.Status == OpcStatus.NotConnected)
                {
                    driver.ReConnect();
                }
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
        private void AddView(List<UaNode> items)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (UaNode item in items)
                {
                    try
                    {
                        var find = driver.FindNode(item.Tag);
                        var nodeid = new NodeId(find.NodeId);// (NodeId)item.NodeId;
                        var readnode = driver.Session.ReadNode(nodeid);
                        if (readnode is Opc.Ua.VariableNode valnode && (valnode.AccessLevel & Opc.Ua.AccessLevels.CurrentRead) != 0)
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
            });
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var select_node = tv_nodes.SelectedNode;
            var node = select_node.Tag as UaNode;

            var nodes = new List<UaNode>();
            if (node != null && node.NodeClass == NodeClass.Variable.ToString())
            {
                nodes.Add(node);
            }
            this.AddView(nodes);
        }

        private void addAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var select_node = tv_nodes.SelectedNode;
            var nodes = new List<UaNode>();

            foreach (TreeNode node2 in select_node.Nodes)
            {
                var item = node2.Tag as UaNode;
                if (item != null && item.NodeClass == NodeClass.Variable.ToString())
                    nodes.Add(item);
            }
            this.AddView(nodes);
        }
    }
}
