using CodeDesigner.UI.Designer.Canvas.NodeObject;
using CodeDesigner.UI.Designer.Toolbox;

namespace CodeDesigner.UI.Designer.Canvas.ast;

public class VariableDefinitionNode : Node
{
    public VariableDefinitionNode()
    {
        NodeType = NodeType.VARIABLE_DEFINITION;
        NodeObjects.Add(new LabelObject("declare variable"));
        NodeObjects.Add(new TextboxObject(50));
        NodeObjects.Add(new LabelObject("of type"));
        NodeObjects.Add(new TextboxObject(50));
        DrawObjects();
    }

    public override string NodeToString()
    {
        return "Declare " + ((TextboxObject) NodeObjects[1]).GetText();
    }
}