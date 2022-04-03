using CodeDesigner.Core;
using CodeDesigner.Core.ast;
using CodeDesigner.UI.Designer.Canvas.ast;
using CodeDesigner.UI.Designer.Canvas.NodeObject;
using CodeDesigner.UI.Designer.Toolbox;

namespace CodeDesigner.UI.Designer.Canvas;

public class NodeConverter
{
    public static void CompileNodes(List<Node> nodes)
    {
        var ast = ConvertToAST(nodes);
        CodeGenerator.Run(ast);
    }
    
    private static List<ASTNode> ConvertToAST(List<Node> nodes)
    {
        var ast = new List<ASTNode>();
        foreach (var node in nodes)
        {
            if (node.NodeType == NodeType.FUNCTION_DEFINITION)
            {
                var funDefNode = (FunctionDefinitionNode) node;
                var nameObject = (TextboxObject) funDefNode.NodeObjects[1];
                var funDef = new ASTFunctionDefinition(nameObject.GetText());
            }
        }
    }
}