using CodeDesigner.UI.Designer.Canvas.NodeObject;
using CodeDesigner.UI.Designer.Toolbox;

namespace CodeDesigner.UI.Designer.Canvas.ast;

public class VariableDeclarationNode : Node
{
    public VariableDeclarationNode()
    {
        NodeType = NodeType.VARIABLE_DECLARATION;
        NodeObjects.Add(new LabelObject("set"));
        NodeObjects.Add(new TextboxObject(50));
        NodeObjects.Add(new LabelObject("equal to"));
        NodeObjects.Add(new InputObject(50));
        DrawObjects();
    }

    protected override string NodeToString()
    {
        return (TextboxObject)NodeObjects[1].GetText() + " = " + (TextboxObject)NodeObjects[3].GetText();
    }
}