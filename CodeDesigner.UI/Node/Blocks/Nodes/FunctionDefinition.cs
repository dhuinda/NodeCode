using CodeDesigner.UI.Designer.Toolbox;

namespace CodeDesigner.UI.Node.Blocks.Nodes;

public class FunctionDefinition : BlockBase
{
    public string Name;
    public Parameter.ParameterType ReturnType;
    public string? ObjectReturnType;

    public FunctionDefinition(string name, Parameter.ParameterType returnType, string? objectReturnType = null)
    {
        Name = name;
        ReturnType = returnType;
        ObjectReturnType = objectReturnType;

        BlockProperties properties = new BlockProperties
        {
            BorderColor = Color.FromArgb(69, 69, 69),
            FillColor = Color.FromArgb(85, 85, 85),
            SecondaryColor = Color.FromArgb(69, 69, 69),
            Height = 70,
            Width = 140,
            TextColor = Color.FromArgb(255, 255, 255),
            Name = "Function Definition",
            OutputType = Parameter.ParameterType.Object
        };

        // todo need a way to add/remove parameters in the UI

        Properties = properties;
        NodeType = NodeType.FUNCTION_DEFINITION;
    }
}
