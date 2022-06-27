using CodeDesigner.UI.Designer.Toolbox;
using CodeDesigner.UI.Node.Interaction;
using CodeDesigner.UI.Node.Interaction.Elements;

namespace CodeDesigner.UI.Node.Blocks.Nodes;

public class FunctionInvocation : BlockBase
{
    public FunctionInvocation() : base(new BlockProperties
    {
        BorderColor = Color.FromArgb(69, 69, 69),
        FillColor = Color.FromArgb(85, 85, 85),
        SecondaryColor = Color.FromArgb(69, 69, 69),
        Height = 110,
        Width = 215,
        TextColor = Color.FromArgb(255, 255, 255),
        Name = "Function Invocation",
        OutputType = Parameter.ParameterType.Object
    })
    {
        // todo need a way to add/remove parameters in the UI

        TextBoxElement element = new TextBoxElement(new ElementProperties
        {
            BlockCoordinates = new PointF(60, 38),
            Size = new SizeF(120, 30)
        }, "Edit", Color.Gray, Color.DarkGray, Color.Beige, null);
        element.Text = "extern.printf";
        
        Elements.Add(element);
        NodeType = NodeType.FUNCTION_INVOCATION;
        UpdateFunction();
        CheckNext();
    }

    public void UpdateFunction()
    {
        Parameters.Clear();
        var textBox = (TextBoxElement) Elements[0];
        var name = textBox.Text;
        if (Canvas.Canvas.FunctionParameters.ContainsKey(name))
        {
            var fun = Canvas.Canvas.FunctionParameters[name];
            foreach (var parameter in fun.Parameters)
            {
                Parameters.Add(new Parameter
                {
                    Type = parameter.Type
                });
            }

            UseOutput = fun.ReturnType != Parameter.ParameterType.Void;
        } else if (name == "extern.printf")
        {
            Parameters.Add(new Parameter
            {
                Type = Parameter.ParameterType.String
            });
            UseOutput = false;
        }
    }
    
}