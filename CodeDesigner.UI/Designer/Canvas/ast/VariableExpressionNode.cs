using CodeDesigner.UI.Designer.Canvas.NodeObject;
using CodeDesigner.UI.Designer.Toolbox;

namespace CodeDesigner.UI.Designer.Canvas.ast;

public class VariableExpressionNode : Node
{
    public VariableExpressionNode()
    {
        NodeType = NodeType.VARIABLE_EXPRESSION;
        NodeObjects.Add(new TextboxObject(50));
        DrawObjects();
    }

    public override string NodeToString()
    {
        return ((TextboxObject)NodeObjects[0]).GetText();
    }
}