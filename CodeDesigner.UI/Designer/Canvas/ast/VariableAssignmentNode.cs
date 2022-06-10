using CodeDesigner.UI.Designer.Canvas.NodeObject;
using CodeDesigner.UI.Designer.Toolbox;

namespace CodeDesigner.UI.Designer.Canvas.ast;

public class VariableAssignmentNode : Node
{
    public VariableAssignmentNode()
    {
        NodeType = NodeType.VARIABLE_ASSIGNMENT;
        NodeObjects.Add(new LabelObject("let"));
        NodeObjects.Add(new TextboxObject(50));
        NodeObjects.Add(new LabelObject("be equal to"));
        NodeObjects.Add(new InputObject(50));
        DrawObjects();
    }

    public override string NodeToString()
    {
        return $"Let {((TextboxObject)NodeObjects[1]).GetText()} = {((InputObject)NodeObjects[1]).AttachedNode.NodeToString()}";
    }
}