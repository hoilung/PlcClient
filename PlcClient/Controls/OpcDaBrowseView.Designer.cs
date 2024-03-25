namespace PlcClient.Controls
{
    partial class OpcDaBrowseView
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tv_nodes = new System.Windows.Forms.TreeView();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // tv_nodes
            // 
            this.tv_nodes.Location = new System.Drawing.Point(34, 37);
            this.tv_nodes.Name = "tv_nodes";
            this.tv_nodes.Size = new System.Drawing.Size(319, 243);
            this.tv_nodes.TabIndex = 0;
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 1000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            this.toolTip1.ToolTipTitle = "提示";
            // 
            // OpcDaBrowseView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tv_nodes);
            this.Name = "OpcDaBrowseView";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tv_nodes;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
