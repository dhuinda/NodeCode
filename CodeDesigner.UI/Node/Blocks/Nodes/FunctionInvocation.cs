using CodeDesigner.UI.Designer.Toolbox;

namespace CodeDesigner.UI.Node.Blocks.Nodes;

public class FunctionInvocation : BlockBase
{
    public string Name;

    public FunctionInvocation(string name)
    {
        Name = name;

        BlockProperties properties = new BlockProperties
        {
            BorderColor = Color.FromArgb(69, 69, 69),
            FillColor = Color.FromArgb(85, 85, 85),
            SecondaryColor = Color.FromArgb(69, 69, 69),
            Height = 50,
            Width = 70,
            TextColor = Color.FromArgb(255, 255, 255),
            Name = "Function Invocation",
            OutputType = Parameter.ParameterType.Object
        };

        // todo need a way to add/remove parameters in the UI

        Properties = properties;
        NodeType = NodeType.FUNCTION_INVOCATION;
    }
}