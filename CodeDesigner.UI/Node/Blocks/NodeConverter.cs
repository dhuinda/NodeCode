using CodeDesigner.Core;
using CodeDesigner.Core.ast;
using CodeDesigner.UI.Designer.Toolbox;
using CodeDesigner.UI.Node.Blocks.Nodes;

namespace CodeDesigner.UI.Node.Blocks;

public class NodeConverter
{
    public static void CompileNodes(List<BlockBase> nodes)
    {
        Console.WriteLine("compiling");
        var ast = new List<ASTNode>();
        ConvertToAST(nodes, ast);
        ast.Insert(0, new ASTPrototypeDeclaration("printf", new List<VariableType>
        {
            new(PrimitiveVariableType.STRING)
        }, new VariableType(PrimitiveVariableType.VOID), true));
        CodeGenerator.Run(ast);
    }

    private static void ConvertToAST(List<BlockBase> nodes, List<ASTNode> parentChildren)
    {
        foreach (var node in nodes)
        {
            AnalyzeNode(node, parentChildren);
        }
    }

    private static void AnalyzeNode(BlockBase node, List<ASTNode> pc)
    {
        switch (node.NodeType)
        {
            case NodeType.FUNCTION_DEFINITION:
            {
                Console.WriteLine("bruhhuh");
                var funDefNode = (FunctionDefinition) node;
                var parameters = funDefNode.Parameters;
                var astParams = new List<ASTVariableDefinition>();
                foreach (var p in parameters)
                {
                    astParams.Add(new (p.Name, GetVariableType(p.Type, p.ObjectType)));
                }

                var body = new List<ASTNode>();
                if (funDefNode.NextBlock != null)
                {
                    AnalyzeNode(funDefNode.NextBlock, body);
                }

                var returnType = GetVariableType(funDefNode.ReturnType, funDefNode.ObjectReturnType);
                pc.Add(new ASTFunctionDefinition(funDefNode.Name, astParams, body, returnType));
                break;
            }
            case NodeType.FUNCTION_INVOCATION:
            {
                var funInvNode = (FunctionInvocation) node;
                var args = funInvNode.Parameters;
                var astArgs = new List<ASTNode>();
                foreach (var arg in args)
                {
                    astArgs.Add(GetASTNodeFromParam(arg));
                }
                pc.Add(new ASTFunctionInvocation(funInvNode.Name, astArgs));
                if (funInvNode.NextBlock != null)
                {
                    Console.WriteLine("printf next block not null");
                    AnalyzeNode(funInvNode.NextBlock, pc);
                }
                break;
            }
            case NodeType.STRING_EXPRESSION:
            {
                var stringExpNode = (StringExpression) node;
                pc.Add(new ASTStringExpression(stringExpNode.Value));
                break;
            }
            case NodeType.RETURN:
            {
                var returnExp = (ReturnExpression) node;
                if (returnExp.Parameters.Count == 0)
                {
                    Console.WriteLine("return null");
                    pc.Add(new ASTReturn());
                } else if (returnExp.Parameters.Count == 1)
                {
                    var val = returnExp.Parameters[0];
                    pc.Add(new ASTReturn(GetASTNodeFromParam(val)));
                }
                else
                {
                    throw new Exception("Return statements should have at most one parameter!");
                }

                break;
            }
        }
    }

    private static ASTNode GetASTNodeFromParam(Parameter param)
    {
        if (param.RawValue != null)
        {
            return param.Type switch
            {
                Parameter.ParameterType.Bool => new ASTBooleanExpression(param.RawValue.ToLower() == "true"),
                Parameter.ParameterType.Double => new ASTNumberExpression(param.RawValue, PrimitiveVariableType.DOUBLE),
                Parameter.ParameterType.Int => new ASTNumberExpression(param.RawValue, PrimitiveVariableType.INTEGER),
                Parameter.ParameterType.String => new ASTStringExpression(param.RawValue),
                Parameter.ParameterType.Object => new ASTVariableExpression(param.RawValue), // should probably add a way to use primitive variables as raw values but whatevs!
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        if (param.ReferenceValue == null) throw new Exception("RawValue and ReferenceValue can't both be null!");
        
        var l = new List<ASTNode>();
        AnalyzeNode(param.ReferenceValue, l);
        if (l.Count != 1)
        {
            throw new Exception("Expected exactly one expression for one parameter; found " +
                                l.Count);
        }
        return l[0];
    }
    
    private static VariableType GetVariableType(Parameter.ParameterType pt, string? objectType)
    {
        if (pt == Parameter.ParameterType.Object)
        {
            return new VariableType(new ClassType(objectType ?? throw new Exception("Parameter type is Object, but ParameterObjectType is null!")));
        }

        return pt switch
        {
            Parameter.ParameterType.Bool => new VariableType(PrimitiveVariableType.BOOLEAN),
            Parameter.ParameterType.Double => new VariableType(PrimitiveVariableType.DOUBLE),
            Parameter.ParameterType.String => new VariableType(PrimitiveVariableType.STRING),
            Parameter.ParameterType.Int => new VariableType(PrimitiveVariableType.INTEGER),
            Parameter.ParameterType.Void => new VariableType(PrimitiveVariableType.VOID),
            _ => throw new ArgumentOutOfRangeException(pt.ToString())
        };
    }
    
}