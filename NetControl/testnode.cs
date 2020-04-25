using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NetControl
{
    public class testnode:TreeViewItem
    {
        public testnode()
        {
            this.NodeId = Guid.NewGuid().ToString();
        }
        public string NodeId { get; set; }
        public string NodeName { get; set; }
        public string NodeContent { get; set; }
        public NodeType NodeType { get; set; }
        public string parentID { get; set; }
    }
    public enum NodeType
    {
        RootNode,//根节点
        LeafNode,//叶子节点
        StructureNode//结构节点，仅起到组织配置文件结构的作用，不参与修改
    }
}
