using CodeDesigner.UI.Designer.Toolbox;
using CodeDesigner.UI.Node.Canvas;
using CodeDesigner.UI.Node.Interaction;
using CodeDesigner.UI.Node.Interaction.Elements;

namespace CodeDesigner.UI.Node.Blocks.Nodes;

public class FunctionDefinition : BlockBase
{
    public string Name;
    public Parameter.ParameterType ReturnType;
    public string? ObjectReturnType;

    public FunctionDefinition() : base(new BlockProperties
    {
        BorderColor = Color.FromArgb(69, 69, 69),
        FillColor = Color.FromArgb(85, 85, 85),
        SecondaryColor = Color.FromArgb(69, 69, 69),
        Height = 70,
        Width = 140,
        TextColor = Color.FromArgb(255, 255, 255),
        Name = "Function main",
        OutputType = Parameter.ParameterType.Void
    })
    {
        Name = "main";
        ReturnType = Parameter.ParameterType.Void;
        ObjectReturnType = null;

        ElementProperties configureBtnProperties = new ElementProperties()
        {
            BlockCoordinates = new PointF(70, 25),
            Size = new SizeF(30, 30)
        };

        NodeType = NodeType.FUNCTION_DEFINITION;

        Element btnElement =
            new IconButtonElement(configureBtnProperties, CodeDesigner.UI.Properties.Resources.Configure_64px, Color.SlateGray, Color.Gray, Color.White, () =>
            {
                InteractionHelper.LoadFunctionConfig(this);
                InteractionHelper.FunctionConfigForm.Show();
            });

        UseOutput = false;

        Elements.Add(btnElement);
        // CheckNext();
    }
}
