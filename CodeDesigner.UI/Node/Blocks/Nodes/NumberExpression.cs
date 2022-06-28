using CodeDesigner.Core;
using CodeDesigner.UI.Designer.Toolbox;
using CodeDesigner.UI.Node.Interaction;
using CodeDesigner.UI.Node.Interaction.Elements;

namespace CodeDesigner.UI.Node.Blocks.Nodes;

public class NumberExpression : BlockBase
{
    public string Value;
    
    public NumberExpression() : base (new BlockProperties
    {
        BorderColor = Color.FromArgb(69, 69, 69),
        FillColor = Color.FromArgb(85, 85, 85),
        SecondaryColor = Color.FromArgb(69, 69, 69),
        Height = 60,
        Width = 90,
        TextColor = Color.FromArgb(255, 255, 255),
        Name = "Number",
        OutputType = Parameter.ParameterType.Object
    })
    {
        Value = "0";
        TextBoxElement element = new TextBoxElement(new ElementProperties
        {
            BlockCoordinates = new PointF(10, 30),
            Size = new SizeF(60, 25)
        }, Color.Gray, Color.DarkGray, Color.Beige, () =>
        {
            Value = ((TextBoxElement) Elements[0]).Text;
        });
        Elements.Add(element);
        NodeType = NodeType.NUMBER_EXPRESSION;
    }
}