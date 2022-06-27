using CodeDesigner.UI.Designer.Toolbox;

namespace CodeDesigner.UI.Node.Blocks.Nodes;

public class StringExpression : BlockBase
{
    public string Value;
    
    public StringExpression(string value) : base(new BlockProperties
    {
        BorderColor = Color.FromArgb(69, 69, 69),
        FillColor = Color.FromArgb(85, 85, 85),
        SecondaryColor = Color.FromArgb(69, 69, 69),
        Height = 70,
        Width = 140,
        TextColor = Color.FromArgb(255, 255, 255),
        Name = "String Expression",
        OutputType = Parameter.ParameterType.Object
    })
    {
        Value = value;
        NodeType = NodeType.STRING_EXPRESSION;
        UseNext = false;
    }
}