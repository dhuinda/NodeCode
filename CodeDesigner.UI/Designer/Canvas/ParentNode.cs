﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeDesigner.UI.Resources.Controls;

namespace CodeDesigner.UI.Designer.Canvas
{
    public partial class ParentNode : Node
    {
        public List<Node> Children;


        public ParentNode()
        {
            Children = new List<Node>();
        }

        public override bool CanHaveChildren()
        {
            return true;
        }

        public override void MoveNodes(MouseEventArgs e, Point lastPoint)
        {
            base.MoveNodes(e, lastPoint);
            foreach (Node c in Children)
            {
                c.MoveNodes(e, lastPoint);
            }
        }

        public void SetNodeAsChild(Node node)
        {
            Children.Add(node);

            if (NodeHasParent)
            {
                int index = Parent.Children.IndexOf(this);

                if (index == Parent.Children.Count)
                    return;

                Parent.SetChildIndex(index, node);
            }


            node.Parent = this;
            node.NodeHasParent = true;

            FormatNodes();
        }

        public void RemoveChildNode(Node node)
        {
            Children.Remove(node);
            node.Parent = null;
            node.NodeHasParent = false;
        }

        public void SetChildIndex(int index, Node child)
        {
            if (Children.Count < 2)
                return;

            Node[] tempNode = new Node[Children.Count + 1];

            bool offset = false;
            for (int i = 0; i < Children.Count; i++)
            {
                if (i == index)
                {
                    tempNode[i] = child;
                    offset = true;
                }

                if (offset)
                    tempNode[i + 1] = Children[i];
                else
                    tempNode[i] = Children[i];
            }

            Children.Clear();
            Children.AddRange(tempNode);

            FormatNodes();
        }


    public override void FormatNodes()
    {
        int heightFactor = 1;
        int heightOffset = 0;

        foreach (Node child in Children.ToArray())
        {
            child.Top = Top + heightFactor * 40;
            child.Left = Left + 30;

            if (heightOffset != 0)
                child.Top += (heightOffset * 40) - 40;

            heightOffset += child.HeightFactor;

            heightFactor++;

            if (child.CanHaveChildren())
            {
                var parentChild = (ParentNode) child;
                for (int i = 1; i < parentChild.Children.Count; i++)
                    heightFactor++;
            }
        }

        HeightFactor = heightFactor;
    }


        #region UI LOGIC

        private void DeleteButtonOnClick(object? sender, EventArgs e)
        {
            foreach (Node n in Children.ToArray())
            {
                RemoveChildNode(n);
            }
            Dispose();
        }


        #endregion
    }
}
