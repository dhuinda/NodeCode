using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeDesigner.UI.Node.Blocks;
using CodeDesigner.UI.Node.Blocks.Nodes;
using CodeDesigner.UI.Windows.Interaction.Functions;

namespace CodeDesigner.UI.Node.Canvas
{
    public static class InteractionHelper
    {
        public static FunctionDefinitionConfiguration FunctionConfigForm = new ();

        public static void LoadFunctionConfig(FunctionDefinition function)
        {
            FunctionConfigForm.LoadFunction(function);
        }
    }
}
