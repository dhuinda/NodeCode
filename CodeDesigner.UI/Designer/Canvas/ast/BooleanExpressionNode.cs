using CodeDesigner.UI.Designer.Canvas.NodeObject;
using CodeDesigner.UI.Designer.Toolbox;

namespace CodeDesigner.UI.Designer.Canvas.ast
{
    public class BooleanExpressionNode : Node
    {
        public BooleanExpressionNode()
        {
            NodeType = NodeType.BOOLEAN_EXPRESSION;
            
            NodeObjects.Add(new ComboObject(new [] {
                "True", 
                "False",
            }));
            
            DrawObjects();
        }

        public override string NodeToString()
        {
            return ((ComboObject)NodeObjects[0]).Box.SelectedText;
        }
    }
}