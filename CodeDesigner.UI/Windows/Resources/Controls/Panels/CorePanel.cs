using CodeDesigner.UI.Node.Blocks;
using CodeDesigner.UI.Node.Blocks.Nodes;
using CodeDesigner.UI.Node.Blocks.Types;
using CodeDesigner.UI.Node.Canvas;

namespace CodeDesigner.UI.Windows.Resources.Controls.Panels
{
    public partial class CorePanel : UserControl
    {
        public CorePanel()
        {
            InitializeComponent();
        }

        private void CorePanel_Load(object sender, EventArgs e)
        {

        }

        private void NewFunction_Click(object sender, EventArgs e)
        {
            Canvas.AddNode(new FunctionDefinition("Test Function", Parameter.ParameterType.Object));
        }

        private void InvokeFunction_Click(object sender, EventArgs e)
        {
            Canvas.AddNode(new FunctionInvocation("Test Function"));
        }

        private void TestExpression_Click(object sender, EventArgs e)
        {
            BlockProperties properties = new BlockProperties();
            properties.Height = 100;
            properties.OutputType = Parameter.ParameterType.Object;
            properties.Width = 300;
            properties.TextColor = Color.White;
            properties.Name = "Test Node";
            properties.FillColor = Color.FromArgb(85,85,85);
            properties.SecondaryColor = Color.FromArgb(69,69,69);
            properties.BorderColor = Color.FromArgb(69,69,69);
            BlockBase? blockbase = new (properties);
            blockbase.Parameters.Add(new Parameter()
            {
                Type = Parameter.ParameterType.Object,
                Connected = false,
                Name = "Random Variable 1"
            });
            blockbase.Parameters.Add(new Parameter()
            {
                Type = Parameter.ParameterType.String,
                Connected = false,
                Name = "Random Variable 2"
            });
            Canvas.AddNode(blockbase);
        }

        private void BinaryExpression_Click(object sender, EventArgs e)
        {
            Canvas.AddNode(new BinaryExpression(BinaryOperator.Add));
        }

        private void StringExpression_Click(object sender, EventArgs e)
        {
            Canvas.AddNode(new StringExpression("Test Value"));
        }

        private void ReturnExpression_Click(object sender, EventArgs e)
        {
            Canvas.AddNode(new ReturnExpression());
        }
    }
}
