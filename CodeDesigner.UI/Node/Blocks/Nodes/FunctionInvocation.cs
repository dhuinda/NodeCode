using CodeDesigner.UI.Designer.Toolbox;
using CodeDesigner.UI.Node.Interaction;

namespace CodeDesigner.UI.Node.Blocks.Nodes;

public class FunctionInvocation : BlockBase
{
    public string Name;

    public FunctionInvocation(string name)
    {
        Name = name;
        Name = "extern.printf";
        UpdateParameters();

        BlockProperties properties = new BlockProperties
        {
            BorderColor = Color.FromArgb(69, 69, 69),
            FillColor = Color.FromArgb(85, 85, 85),
            SecondaryColor = Color.FromArgb(69, 69, 69),
            Height = 70,
            Width = 140,
            TextColor = Color.FromArgb(255, 255, 255),
            Name = "Function Invocation",
            OutputType = Parameter.ParameterType.Object
        };

        // todo need a way to add/remove parameters in the UI

        Properties = properties;
        NodeType = NodeType.FUNCTION_INVOCATION;
        CheckNext();
    }

    public void UpdateParameters()
    {
        Parameters.Clear();
        if (Canvas.Canvas.FunctionParameters.ContainsKey(Name))
        {
            var parameters = Canvas.Canvas.FunctionParameters[Name];
            foreach (var parameter in parameters)
            {
                Parameters.Add(new Parameter
                {
                    Type = parameter.Type
                });
            }
        } else if (Name == "extern.printf")
        {
            Parameters.Add(new Parameter
            {
                Type = Parameter.ParameterType.String
            });
        }
    }
    
}