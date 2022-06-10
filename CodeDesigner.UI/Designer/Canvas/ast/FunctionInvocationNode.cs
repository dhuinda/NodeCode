using CodeDesigner.UI.Designer.Canvas.NodeObject;
using CodeDesigner.UI.Designer.Toolbox;

namespace CodeDesigner.UI.Designer.Canvas.ast;

public class FunctionInvocationNode : Node
{
    public FunctionInvocationNode()
    {
        NodeType = NodeType.FUNCTION_INVOCATION;
        NodeObjects.Add(new LabelObject("call function"));
        NodeObjects.Add(new TextboxObject(50));
        NodeObjects.Add(new LabelObject("with arguments"));
        NodeObjects.Add(new InputObject(50));
        DrawObjects();
    }

    public override string NodeToString()
    {
        return "Call " + ((TextboxObject) NodeObjects[1]).GetText();
    }
}