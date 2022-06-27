using CodeDesigner.UI.Designer.Toolbox;
using CodeDesigner.UI.Node.Canvas;
using CodeDesigner.UI.Node.Interaction;
using CodeDesigner.UI.Node.Interaction.Elements;

namespace CodeDesigner.UI.Node.Blocks.Nodes;

public class FunctionInvocation : BlockBase
{
    public string Name => ((TextBoxElement)Elements[0]).Text;

    public FunctionInvocation() : base(new BlockProperties()
    {
        BorderColor = Color.FromArgb(69, 69, 69),
        FillColor = Color.FromArgb(85, 85, 85),
        SecondaryColor = Color.FromArgb(69, 69, 69),
        Height = 90,
        Width = 160,
        TextColor = Color.FromArgb(255, 255, 255),
        Name = "Function Invocation",
        OutputType = Parameter.ParameterType.Object
    })
    {
        // todo need a way to add/remove parameters in the UI

        TextBoxElement element = new TextBoxElement(new ElementProperties
        {
            BlockCoordinates = new PointF(27, 38),
            Size = new SizeF(100, 30)
        }, "Edit", Color.Gray, Color.DarkGray, Color.Beige, null);
        
        Elements.Add(element);
        NodeType = NodeType.FUNCTION_INVOCATION;
    }
}