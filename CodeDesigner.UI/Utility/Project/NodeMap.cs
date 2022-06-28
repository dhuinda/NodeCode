using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeDesigner.UI.Designer.Toolbox;
using CodeDesigner.UI.Node.Blocks;
using CodeDesigner.UI.Node.Blocks.Nodes;
using CodeDesigner.UI.Node.Canvas;

namespace CodeDesigner.UI.Utility.Project
{
    [Serializable]
    public class NodeMap
    {
        public NodeMap()
        {
            Dependencies = new List<NodeMap>();
        }
        
        public string Name { get; set; }
        public System.Drawing.Image Thumbnail { get; set; }
        public List<BlockBase?> Blocks { get; set; }
        public List<NodeMap> Dependencies { get; set; }

        public void ScanForFunctions()
        {
            Console.WriteLine("scanning for functions in " + Name);
            foreach (var block in Blocks)
            {
                if (block.NodeType == NodeType.FUNCTION_DEFINITION)
                {
                    var function = (FunctionDefinition) block;
                    Console.WriteLine(function.Parameters.Count);
                    Console.WriteLine("found function " + function.Name);
                    Canvas.FunctionData[function.Name] = new FunctionInformation(function.Parameters, function.ReturnType);
                }
            }

            foreach (var dependency in Dependencies)
            {
                dependency.ScanForFunctions();
            }
        }
    }
}
