using CodeDesigner.UI.Designer.Canvas.NodeObject;
using CodeDesigner.UI.Designer.Toolbox;

namespace CodeDesigner.UI.Designer.Canvas.ast
{
    public class BinaryExpressionNode : Node
    {
        public FunctionDefinitionNode()
        {
            NodeType = NodeType.BINARY_EXPRESSION;
            NodeObjects.Add(new InputObject(50));
            
            NodeObjects.Add(new ComboObject(new [] {
                "<", 
                ">",
                "==",
                "!=",
                "<=",
                ">=",
                "&&",
                "||",
                "+",
                "-",
                "*",
                "/",
                "%"
            }));

            NodeObjects.Add(new InputObject(50));
            
            DrawObjects();
        }

        public override string NodeToString()
        {
            return "... " + ((ComboObject)NodeObjects[1]).Box.SelectedText + " ...";
        }
    }
}