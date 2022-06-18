using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeDesigner.UI.Node.Blocks.Types;

namespace CodeDesigner.UI.Node.Blocks.Nodes
{
    public class BinaryExpression : BlockBase
    {
        public BinaryOperator Operator;

        public BinaryExpression(BinaryOperator binaryOperator)
        {
            Operator = binaryOperator;

            BlockProperties properties = new BlockProperties
            {
                BorderColor = Color.FromArgb(69, 69, 69),
                FillColor = Color.FromArgb(85, 85, 85),
                SecondaryColor = Color.FromArgb(69, 69, 69),
                Height = 50,
                Width = 70,
                TextColor = Color.FromArgb(255, 255, 255),
                Name = "Binary Expression",
                OutputType = Parameter.ParameterType.Object
            };

            Parameters.Add(new Parameter()
            {
                Name = "Input 1",
                Type = Parameter.ParameterType.Object
            });

            Parameters.Add(new Parameter()
            {
                Name = "Input 2",
                Type = Parameter.ParameterType.Object
            });

            Properties = properties;
        }
    }
}
