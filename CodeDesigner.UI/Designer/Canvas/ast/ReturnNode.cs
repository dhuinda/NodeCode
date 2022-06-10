using CodeDesigner.UI.Designer.Canvas.NodeObject;
using CodeDesigner.UI.Designer.Toolbox;

namespace CodeDesigner.UI.Designer.Canvas.ast;

public class ReturnNode : Node
{
    public ReturnNode()
    {
        NodeType = NodeType.RETURN;
        NodeObjects.Add(new LabelObject("return"));
        NodeObjects.Add(new InputObject(50));
        DrawObjects();
    }

    public override string NodeToString()
    {
        return ((TextboxObject) NodeObjects[1]).GetText();
    }
}
