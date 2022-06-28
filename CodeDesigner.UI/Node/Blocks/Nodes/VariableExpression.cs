using CodeDesigner.UI.Designer.Toolbox;

namespace CodeDesigner.UI.Node.Blocks.Nodes;

public class VariableExpression : BlockBase
{
    
    public string Name;
    
    public VariableExpression(string name) : base ( new BlockProperties
    {
        BorderColor = Color.FromArgb(69, 69, 69),
        FillColor = Color.FromArgb(85, 85, 85),
        SecondaryColor = Color.FromArgb(69, 69, 69),
        Height = 50,
        Width = 70,
        TextColor = Color.FromArgb(255, 255, 255),
        Name = "Variable Expression",
        OutputType = Parameter.ParameterType.Object
    })
    {
        Name = name;
        NodeType = NodeType.VARIABLE_EXPRESSION;
        CanHavePrevious = false;
        CheckNext();
    }
}