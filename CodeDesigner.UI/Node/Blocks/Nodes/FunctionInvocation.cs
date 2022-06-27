using CodeDesigner.UI.Designer.Toolbox;
using CodeDesigner.UI.Node.Interaction;

namespace CodeDesigner.UI.Node.Blocks.Nodes;

public class FunctionInvocation : BlockBase
{
    public string Name;

    public FunctionInvocation(string name) : base( new BlockProperties {
        BorderColor = Color.FromArgb(69, 69, 69),
        FillColor = Color.FromArgb(85, 85, 85),
        SecondaryColor = Color.FromArgb(69, 69, 69),
        Height = 70,
        Width = 140,
        TextColor = Color.FromArgb(255, 255, 255),
        Name = "Function Invocation",
        OutputType = Parameter.ParameterType.Object})
    {

        // todo need a way to add/remove parameters in the UI

        //Properties = properties;
        //NodeType = NodeType.FUNCTION_INVOCATION;
    }
}