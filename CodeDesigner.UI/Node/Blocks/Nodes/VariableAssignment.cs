using CodeDesigner.UI.Designer.Toolbox;

namespace CodeDesigner.UI.Node.Blocks.Nodes;

public class VariableAssignment : BlockBase
{
    public string Name;
    
    public VariableAssignment(string name)
    {
        Name = name;
        // todo probably want a way to assign variables in-line (same with VariableDefinition)
        BlockProperties properties = new BlockProperties
        {
            BorderColor = Color.FromArgb(69, 69, 69),
            FillColor = Color.FromArgb(85, 85, 85),
            SecondaryColor = Color.FromArgb(69, 69, 69),
            Height = 50,
            Width = 70,
            TextColor = Color.FromArgb(255, 255, 255),
            Name = "Variable Assignment",
            OutputType = Parameter.ParameterType.Object
        };

        Parameters.Add(new Parameter
        {
            Name = "Value",
            Type = Parameter.ParameterType.Object // todo: need a way to change this when the type is actually set
        });
        
        Properties = properties;
        NodeType = NodeType.VARIABLE_ASSIGNMENT;
    }
}