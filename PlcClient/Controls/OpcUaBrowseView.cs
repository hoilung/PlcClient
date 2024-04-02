using HL.OpcUa;
using Opc.Ua;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PlcClient.Controls
{
    public partial class OpcUaBrowseView : BaseControl
    {
        private readonly OpcUaDriver driver;
        public List<ReferenceDescription> data = new List<ReferenceDescription>();

        public event Action<VariableNode> DataRefresh;
        public OpcUaBrowseView(OpcUaDriver driver)
        {
            InitializeComponent();
            this.tableLayoutPanel1.Dock = this.statusStrip1.Dock = this.tv_nodes.Dock = DockStyle.Fill;
            this.driver = driver;
            tv_nodes.NodeMouseDoubleClick += Tv_nodes_NodeMouseDoubleClick;
        }

        private void Tv_nodes_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (tv_nodes.SelectedNode == null || tv_nodes.SelectedNode.Nodes.Count > 0)
            {
                return;
            }

            var item = e.Node.Tag as ReferenceDescription;
            if (item == null)
            {
                return;
            }
            if (item.NodeClass != NodeClass.Variable || e.Node.Nodes.Count > 0)
            {
                toolStripStatusLabel1.Text = $"{item.NodeId} 节点类型：{item.NodeClass}，不支持读取";
                return;
            }
            try
            {
                var nodeid = (NodeId)item.NodeId;
                var node = driver.Session.ReadNode(nodeid);                
                if (node is Opc.Ua.VariableNode valnode)
                {
                    valnode.Value = driver.Session.ReadValue(nodeid);
                    toolStripStatusLabel1.Text = $"添加节点：{valnode.NodeId} 节点值：{valnode.Value}";
                    DataRefresh?.Invoke(valnode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "添加节点，读取值失败");
            }
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
                GetTreeNode(rootNode, ObjectIds.ObjectsFolder);
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

        private void GetTreeNode(TreeNode pNode, NodeId nodeId)
        {
            if (driver.Session == null || !driver.Session.Connected)
                return;

            var list = driver.GetReferenceDescriptionCollection(nodeId);
            if (list == null || list.Count == 0)
                return;
            foreach (var item in list)
            {
                var node = pNode.Nodes.Add(item.DisplayName.Text);
                node.Tag = item;
                GetTreeNode(node, ExpandedNodeId.ToNodeId(item.NodeId, null));
            }
        }
    }
}
