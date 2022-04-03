using CodeDesigner.UI.Designer.Canvas.NodeObject;
using CodeDesigner.UI.Designer.Toolbox;

namespace CodeDesigner.UI.Designer.Canvas.ast;

public class NumberExpressionNode : Node
{
    public NumberExpressionNode()
    {
        NodeType = NodeType.NUMBER_EXPRESSION;
        NodeObjects.Add(new LabelObject("number"));
        NodeObjects.Add(new TextboxObject(50));
        DrawObjects();
    }

    public string NodeToString()
    {
        return (TextboxObject)NodeObjects[1].GetText();
    }
}