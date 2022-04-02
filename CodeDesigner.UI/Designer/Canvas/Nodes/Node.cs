using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDesigner.UI.Designer.Canvas.Nodes
{
    public class Node
    {
        public Node Parent { get; set; }
        public LinkedList<Node> Children { get; set; }
        public LinkedList<Node> Siblings { get; set; }
        public bool HasParent { get; set; }
        public bool IsRoot { get; set; }

        public int CalculateDepth()
        {
            int depth = 1;

            Node node = this;
                              while (node.HasParent)
            {
                depth++;
                node = node.Parent;
            }

            return depth;
        }
    }
}
