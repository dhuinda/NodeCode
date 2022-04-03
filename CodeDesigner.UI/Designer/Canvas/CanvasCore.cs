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
            if (tNode.NodeType == NodeTypes.FUNCTION_DEFINITION)
            {
                node = new ParentNode();
            }
            else
            {
                node = new Node();
            }
            node.SetCanvas(this);
            node.Width = 80;

            Nodes.Add(node);

            Core.Form.DesignerCanvas.Controls.Add(node);
        }

        public void CheckOverlapping(Node node)
        {
            int index = Nodes.IndexOf(node);

            for (int i = 0; i < Nodes.Count; i++)
            {
                if (i == index)
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
                if (i == index)
                    continue;

                node.Intersecting = false;

                if (node.Bounds.IntersectsWith(Nodes[i].Bounds))
                {
                    Nodes[i].Intersecting = false;
                    node.Intersecting = false;

                    if (Nodes[i].CanHaveChildren())
                    {
                        var parentNode = (ParentNode) Nodes[i];
                        parentNode.SetNodeAsChild(node);
                    }
                    break;
                }
            }
        }
    }
}
