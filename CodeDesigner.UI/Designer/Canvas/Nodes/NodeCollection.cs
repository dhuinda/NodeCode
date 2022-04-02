using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDesigner.UI.Designer.Canvas.Nodes
{
    public class NodeCollection
    {
        public Node RootNode { get; set; }

        public void AddRootNode(Node node)
        {
            RootNode = node;
        }

        public int GetIndexOf(Node node)
        {
            if (!node.HasParent)
                return 0;

            int index = 0;

            for (LinkedListNode<Node> n = node.Parent.Children.First; n != null; n = n.Next, index++)
            {
                if (node.Equals(n.Value))
                    return index;
            }

            return -1;
        }
    }
}
