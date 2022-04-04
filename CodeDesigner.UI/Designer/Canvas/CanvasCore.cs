using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeDesigner.UI.Designer.Canvas.ast;
using CodeDesigner.UI.Designer.Canvas.NodeObject;
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
                if (tNode.NodeType == NodeType.FUNCTION_DEFINITION)
                {
                    node = new FunctionDefinitionNode();
                }
                else
                {
                    node = new ParentNode();
                }
            }
            else
            {
                if (tNode.NodeType == NodeType.VARIABLE_DECLARATION)
                {
                    node = new VariableDeclarationNode();
                }
                else if (tNode.NodeType == NodeType.NUMBER_EXPRESSION)
                {
                    node = new NumberExpressionNode();
                }
                else if (tNode.NodeType == NodeType.FUNCTION_INVOCATION)
                {
                    node = new FunctionInvocationNode();
                } else if (tNode.NodeType == NodeType.STRING_EXPRESSION)
                {
                    node = new StringExpressionNode();
                }
                else
                {
                    node = new Node();
                }
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

        public void ReleaseOverlapping(Node node, MouseEventArgs e)
        {
            int index = Nodes.IndexOf(node);

            for (int i = 0; i < Nodes.Count; i++)
            {
                if (i == index || (node.NodeHasParent && node.Parent == Nodes[i]) || (Nodes[i].NodeHasParent && Nodes[i].Parent == node))
                    continue;

                node.Intersecting = false;

                if (node.Bounds.IntersectsWith(Nodes[i].Bounds))
                {
                    Nodes[i].Intersecting = false;
                    
                    if (Nodes[i].NodeObjects.Count > 0)
                    {
                        foreach (NodeObject.NodeObject obj in Nodes[i].NodeObjects)
                        {
                            if (obj.GetType() == typeof(InputObject))
                            {
                                InputObject input = (InputObject) obj;
                                Size size = input.DropPanel.Bounds.Size;
                                Point point = new Point(Nodes[i].Left + input.DropPanel.Left,
                                    Nodes[i].Top + input.DropPanel.Top);
                                Rectangle rect = new Rectangle(point, size);
                                Point mouseLoc = new Point(Nodes[i].Left + e.X, Nodes[i].Top + e.Y);
                                
                                if (node.Bounds.IntersectsWith(rect))
                                {
                                    node.NodeHasParent = true;
                                    if (input.DropPanel.BackColor == Color.DimGray)
                                        continue;
                                    input.AttachedNode = node;
                                    input.DropPanel.BackColor = Color.Transparent;
                                    input.CreatePreviewString();
                                    
                                    var difference = TextRenderer.MeasureText(node.NodeToString(), input.DropLabel.Font).Width - input.DropPanel.Width;
                                    input.DropPanel.Width += difference; 
                                    input.DropLabel.Width += difference;
                                    Nodes[i].Width += difference;

                                    node.Hide();
                                    node.Location = new Point(0, 0);
                                    if (Nodes[i].NodeType is NodeType.FUNCTION_DEFINITION
                                        or NodeType.FUNCTION_INVOCATION) 
                                    {
                                        var io = new InputObject(50);
                                        Nodes[i].NodeObjects.Add(io);
                                        var w = Nodes[i].Width;
                                        Nodes[i].Width += 60;
                                        Nodes[i].Controls.Add(io.DropPanel);
                                        io.DropPanel.Left = w + 5;
                                        io.DropPanel.Top = 6;
                                    }
                                    return;
                                }
                            }
                        }
                    }
                    
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
