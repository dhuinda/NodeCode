using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeDesigner.UI.Designer.Toolbox;
using CodeDesigner.UI.Windows.Resources.Controls.Node;

namespace CodeDesigner.UI.Designer.Canvas
{
    public class CanvasCore
    {
        public List<Node> Nodes;

        public DesignerCore Core;

        public CanvasCore(DesignerCore core)
        {
            Core = core;

            Nodes = new List<Node>();
        }

        public void GenerateNode(ToolboxNode tNode)
        {
            Node node;
            if (tNode.NodeType is NodeType.CLASS_DEFINITION or NodeType.FUNCTION_DEFINITION or NodeType.IF_STATEMENT)
            {
                node = new ParentNode();
            }
            else
            {
                node = new Node();
            }
            node.SetCanvas(this);

            Nodes.Add(node);

            Core.Form.DesignerCanvas.Controls.Add(node);
        }

        public void CheckOverlapping(Node node)
        {
            int index = Nodes.IndexOf(node);

            for (int i = 0; i < Nodes.Count; i++)
            {
                if (i == index || (node.NodeHasParent && node.Parent == Nodes[i]) || (Nodes[i].NodeHasParent && Nodes[i].Parent == node))
                    continue;

                Nodes[i].Intersecting = false;
                Nodes[i].SetColor(Color.FromArgb(24, 29, 39));

                node.Intersecting = false;

                if (node.Bounds.IntersectsWith(Nodes[i].Bounds))
                {
                    Nodes[i].SetColor(Color.FromArgb(14, 19, 29));
                    Nodes[i].Intersecting = true;
                    node.Intersecting = true;
                }
            }
        }

        public void ReleaseOverlapping(Node node)
        {
            int index = Nodes.IndexOf(node);

            for (int i = 0; i < Nodes.Count; i++)
            {
                if (i == index || (node.NodeHasParent && node.Parent == Nodes[i]) || (Nodes[i].NodeHasParent && Nodes[i].Parent == node))
                    continue;

                node.Intersecting = false;

                if (node.Bounds.IntersectsWith(Nodes[i].Bounds))
                {
                    if (Nodes[i].NodeObjects.Count > 0)
                    {
                        foreach (Nodes.NodeObject obj in Nodes[i].NodeObjects)
                        {
                            if (obj.GetType() == typeof(InputObject))
                            {
                                
                            }
                        }
                    }

                    Nodes[i].Intersecting = false;
                    node.Intersecting = false;

                    if (Nodes[i].NodeHasParent)
                    {
                        Nodes[i].Parent.FormatNodes();
                    }
                    if (Nodes[i].CanHaveChildren())
                    {
                        var parentNode = (ParentNode) Nodes[i];
                        parentNode.SetNodeAsChild(node);
                    }
                    else if (Nodes[i].NodeHasParent)
                    {
                        var idx = Nodes[i].Parent.Children.IndexOf(Nodes[i]);
                        Nodes[i].Parent.Children.Insert(idx + 1, node);
                        
                        node.Parent = Nodes[i].Parent;
                        node.NodeHasParent = true;
                        Nodes[i].Parent.FormatNodes();
                    }
                    break;
                }
            }
        }
    }
}
