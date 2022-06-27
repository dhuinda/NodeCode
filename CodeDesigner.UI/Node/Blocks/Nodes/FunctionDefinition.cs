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

    public FunctionDefinition(string name, Parameter.ParameterType returnType, string? objectReturnType = null) : base(BlockProperties properties = new BlockProperties
    {
        BorderColor = Color.FromArgb(69, 69, 69),
        FillColor = Color.FromArgb(85, 85, 85),
        SecondaryColor = Color.FromArgb(69, 69, 69),
        Height = 70,
        Width = 140,
        TextColor = Color.FromArgb(255, 255, 255),
        Name = "Function Definition",
        OutputType = Parameter.ParameterType.Object
    })
    {
        Name = name;
        ReturnType = returnType;
        ObjectReturnType = objectReturnType;

        ElementProperties configureBtnProperties = new ElementProperties()
        {
            BlockCoordinates = new PointF(70, 30),
            Size = new SizeF(50, 30)
        };

        UseNext = false;
        NodeType = NodeType.FUNCTION_DEFINITION;

        Element btnElement =
            new ButtonElement(configureBtnProperties, "Configure", Color.SlateGray, Color.Gray, Color.White, () =>
            {
                InteractionHelper.LoadFunctionConfig(this);
                InteractionHelper.FunctionConfigForm.Show();
            });

        Elements.Add(btnElement);
    }
}
