using CodeDesigner.UI.Designer.Canvas.NodeObject;
using CodeDesigner.UI.Designer.Toolbox;

namespace CodeDesigner.UI.Designer.Canvas.ast;

public class StringExpressionNode : Node
{
    public StringExpressionNode()
    {
        NodeType = NodeType.STRING_EXPRESSION;
        NodeObjects.Add(new LabelObject("string"));
        NodeObjects.Add(new TextboxObject(150));
        DrawObjects();
    }

    public override string NodeToString()
    {
        return ((TextboxObject) NodeObjects[1]).GetText();
    }
}