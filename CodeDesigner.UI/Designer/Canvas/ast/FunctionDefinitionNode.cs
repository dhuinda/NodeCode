using CodeDesigner.UI.Designer.Canvas.NodeObject;
using CodeDesigner.UI.Designer.Toolbox;

namespace CodeDesigner.UI.Designer.Canvas.ast;

public class FunctionDefinitionNode : ParentNode
{
    public FunctionDefinitionNode()
    {
        NodeType = NodeType.FUNCTION_DEFINITION;
        NodeObjects.Add(new LabelObject("define function"));
        NodeObjects.Add(new TextboxObject(50));
        NodeObjects.Add(new LabelObject("with return type"));
        NodeObjects.Add(new TextboxObject(50));
        NodeObjects.Add(new LabelObject("with parameters"));
        NodeObjects.Add(new InputObject(50));
        DrawObjects();
    }
}