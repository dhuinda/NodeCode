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

    private static VariableType ConvertStringToVariableType(string s)
    {
        return s.ToLower() switch
        {
            "integer" => new VariableType(PrimitiveVariableType.INTEGER),
            "float" or "double" => new VariableType(PrimitiveVariableType.DOUBLE),
            "string" => new VariableType(PrimitiveVariableType.STRING),
            "boolean" or "bool" => new VariableType(PrimitiveVariableType.BOOLEAN),
            "void" => new VariableType(PrimitiveVariableType.VOID),
            _ => new VariableType(new ClassType(s))
        };
    }
    
    private static List<ASTNode> ConvertToAST(List<Node> nodes)
    {
        var ast = new List<ASTNode>();
        foreach (var node in nodes)
        {
            switch (node.NodeType)
            {
                case NodeType.FUNCTION_DEFINITION:
                {
                    var funDefNode = (FunctionDefinitionNode) node;
                    var nameObject = (TextboxObject) funDefNode.NodeObjects[1];
                    var parameters = new List<ASTVariableDefinition>();
                    if (funDefNode.Controls.Count >= 7)
                    {
                        for (var i = 5; i < funDefNode.NodeObjects.Count - 1; i++)
                        {
                            if (((InputObject) funDefNode.NodeObjects[i]).AttachedNode.NodeType !=
                                NodeType.VARIABLE_DEFINITION)
                            {
                                continue;
                            }
                            var varDefNode = (VariableDefinitionNode) ((InputObject) funDefNode.NodeObjects[i]).AttachedNode;
                            var name = ((TextboxObject) varDefNode.NodeObjects[1]).GetText();
                            var type = ConvertStringToVariableType(((TextboxObject) varDefNode.NodeObjects[3]).GetText());
                            parameters.Add(new ASTVariableDefinition(name, type));
                        }
                    }

                    var returnTypeObject = (TextboxObject) funDefNode.NodeObjects[3];
                    var returnType = ConvertStringToVariableType(returnTypeObject.GetText());
                    var funDef = new ASTFunctionDefinition(nameObject.GetText(), parameters, ConvertToAST(funDefNode.Children), returnType);
                    ast.Add(funDef);
                    break;
                }
                case NodeType.FUNCTION_INVOCATION:
                {
                    var funInvNode = (FunctionInvocationNode) node;
                    var funName = ((TextboxObject) funInvNode.NodeObjects[1]).GetText();
                    var args = new List<ASTNode>();
                    if (funInvNode.Controls.Count >= 5)
                    {
                        
                    }
                }
            }
        }

        return ast;
    }
}