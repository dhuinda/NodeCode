using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDesigner.UI.Designer.Canvas.Nodes
{
    public class Node
    {
        public NodeCollection Collection { get; set; }
        public Node Parent { get; set; }
        public Canvas.Node BindedControl { get; set; }
        public LinkedList<Node> Children { get; set; }
        public bool HasParent { get; set; }
        public bool IsRoot { get; set; }

        public Node(Canvas.Node binded)
        {
            BindedControl = binded;
        }

        public Node(Canvas.Node binded, NodeCollection collection)
        {
            BindedControl = binded;
            Collection = collection;
            Children = new LinkedList<Node>();
        }

        public Node(Canvas.Node binded, NodeCollection collection, bool root)
        {
            BindedControl = binded;
            Collection = collection;
            Children = new LinkedList<Node>();
            IsRoot = root;
        }

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

        public void AddChild(Node node)
        {
            if (Children.Last == null)
            {
                Children.AddFirst(node);
                return;
            }
            else
            {
                Children.AddLast(node);
            }
        }

        public void AddChildAfter(Node sibling, Node child)
        {
            Children.AddAfter(GetLinkedNode(Children, sibling), child);
        }

        public void AddChildBefore(Node sibling, Node child)
        {
            Children.AddBefore(GetLinkedNode(Children, sibling), child);
        }

        public void RemoveChild(Node child)
        {
            Children.Remove(GetLinkedNode(Children, child));
        }

        public LinkedListNode<Node> GetLinkedNode(LinkedList<Node> list, Node n)
        {
            for(LinkedListNode<Node> node=list.First; node != null; node=node.Next)
            {
                if (node.Value == n)
                    return node;
            }

            return null;
        }
    }
}
