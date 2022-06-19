using CodeDesigner.UI.Designer.Toolbox;

namespace CodeDesigner.UI.Node.Blocks.Nodes;

public class ReturnExpression : BlockBase
{
    public ReturnExpression()
    {
        BlockProperties properties = new BlockProperties
        {
            BorderColor = Color.FromArgb(69, 69, 69),
            FillColor = Color.FromArgb(85, 85, 85),
            SecondaryColor = Color.FromArgb(69, 69, 69),
            Height = 50,
            Width = 70,
            TextColor = Color.FromArgb(255, 255, 255),
            Name = "Return Expression",
            OutputType = Parameter.ParameterType.Object
        };
        
        // Optional parameter that needs to be added in the UI

        Properties = properties;
        NodeType = NodeType.RETURN;
    }
}