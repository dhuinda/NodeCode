using CodeDesigner.Core;
using CodeDesigner.UI.Designer.Toolbox;

namespace CodeDesigner.UI.Node.Blocks.Nodes;

public class NumberExpression : BlockBase
{
    public string Value;
    public bool IsFloatingPoint;
    
    public NumberExpression(string value, bool isFloatingPoint)
    {
        Value = value;
        IsFloatingPoint = isFloatingPoint;

        BlockProperties properties = new BlockProperties
        {
            BorderColor = Color.FromArgb(69, 69, 69),
            FillColor = Color.FromArgb(85, 85, 85),
            SecondaryColor = Color.FromArgb(69, 69, 69),
            Height = 50,
            Width = 70,
            TextColor = Color.FromArgb(255, 255, 255),
            Name = "Number Expression",
            OutputType = Parameter.ParameterType.Object
        };

        Properties = properties;
        NodeType = NodeType.NUMBER_EXPRESSION;
    }
}