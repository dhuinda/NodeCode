using CodeDesigner.Core;
using CodeDesigner.Core.ast;
using CodeDesigner.UI.Designer.Toolbox;
using CodeDesigner.UI.Node.Blocks.Nodes;
using CodeDesigner.UI.Node.Blocks.Types;

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
            case NodeType.BINARY_EXPRESSION:
            {
                var binExpNode = (BinaryExpression) node;
                ASTNode left;
                if (binExpNode.Left != null)
                {
                    left = GetASTNodeFromString(binExpNode.Left);
                } else if (binExpNode.Parameters.Count > 0 && binExpNode.Parameters[0] != null)
                {
                    left = GetASTNodeFromParam(binExpNode.Parameters[0] ?? throw new Exception());
                }
                else
                {
                    throw new Exception(
                        "Error in binary expression: the left value must either be inline or passed as a parameter");
                }

                ASTNode right;
                if (binExpNode.Right != null)
                {
                    right = GetASTNodeFromString(binExpNode.Right);
                } else if (binExpNode.Parameters.Count > 1 && binExpNode.Parameters[1] != null)
                {
                    right = GetASTNodeFromParam(binExpNode.Parameters[1] ?? throw new Exception());
                }
                else
                {
                    throw new Exception(
                        "Error in binary expression: the right value must either be inline or passed as a parameter");
                }
                pc.Add(new ASTBinaryExpression(ConvertBinOp(binExpNode.Operator), left, right));
                break;
            }
            case NodeType.BOOLEAN_EXPRESSION:
            {
                var boolExpNode = (BooleanExpression) node;
                pc.Add(new ASTBooleanExpression(boolExpNode.Value));
                break;
            }
            case NodeType.FUNCTION_DEFINITION:
            {
                Console.WriteLine("bruhhuh");
                var funDefNode = (FunctionDefinition) node;
                var astParams = new List<ASTVariableDefinition>();
                foreach (var p in funDefNode.Parameters)
                {
                    if (p == null)
                    {
                        throw new Exception("Unexpected null parameter in definition of function " + funDefNode.Name +
                                            ": either remove the parameter or assign it a value");
                    }
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
                var astArgs = new List<ASTNode>();
                foreach (var arg in funInvNode.Parameters)
                {
                    if (arg == null)
                    {
                        throw new Exception("Unexpected null parameter in invocation of function " + funInvNode.Name +
                                        ": either remove the parameter or assign it a value");
                    }
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
            case NodeType.NUMBER_EXPRESSION:
            {
                var numExpNode = (NumberExpression) node;
                pc.Add(new ASTNumberExpression(numExpNode.Value, numExpNode.Value.Contains('.') ? PrimitiveVariableType.DOUBLE : PrimitiveVariableType.INTEGER));
                break;
            }
            case NodeType.PROTOTYPE_DECLARATION:
            {
                var protoNode = (PrototypeDeclaration) node;
                var astParams = new List<VariableType>();
                foreach (var p in protoNode.Parameters)
                {
                    if (p == null)
                    {
                        throw new Exception("Unexpected null parameter in definition of prototype " + protoNode.Name +
                                            ": either remove the parameter or assign it a value");
                    }
                    astParams.Add(GetVariableType(p.Type, p.ObjectType));
                }

                var returnType = GetVariableType(protoNode.ReturnType, protoNode.ObjectReturnType);
                pc.Add(new ASTPrototypeDeclaration(protoNode.Name, astParams, returnType, false));
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
                    if (val == null)
                    {
                        throw new Exception(
                            "Unexpected null parameter of return: either remove the parameter or assign it a value.");
                    }
                    pc.Add(new ASTReturn(GetASTNodeFromParam(val)));
                }
                else
                {
                    throw new Exception("Return statements should have at most one parameter!");
                }

                break;
            }
            case NodeType.VARIABLE_ASSIGNMENT:
            {
                var varAssignNode = (VariableAssignment) node;
                if (varAssignNode.Parameters.Count != 1)
                {
                    throw new Exception("Expected variable assignment to have one parameter but instead it has " + varAssignNode.Parameters.Count);
                }

                pc.Add(new ASTVariableAssignment(varAssignNode.Name, GetASTNodeFromParam(varAssignNode.Parameters[0] ?? throw new Exception("Unexpected null parameter in variable assignment"))));
                break;
            }
            case NodeType.VARIABLE_DECLARATION:
            {
                var varDeclNode = (VariableDeclaration) node;
                if (varDeclNode.Parameters.Count != 1)
                {
                    throw new Exception("Expected variable assignment to have one parameter but instead it has " + varDeclNode.Parameters.Count);
                }
                pc.Add(new ASTVariableDeclaration(varDeclNode.Name, GetVariableType(varDeclNode.Type, varDeclNode.ObjectType), GetASTNodeFromParam(varDeclNode.Parameters[0] ?? throw new Exception("Unexpected null parameter in variable assignment"))));
                break;
            }
            case NodeType.VARIABLE_DEFINITION:
            {
                var varDefNode = (VariableDefinition) node;
                pc.Add(new ASTVariableDefinition(varDefNode.Name,
                    GetVariableType(varDefNode.Type, varDefNode.ObjectType)));
                break;
            }
            case NodeType.VARIABLE_EXPRESSION:
            {
                var varExpNode = (VariableExpression) node;
                pc.Add(new ASTVariableExpression(varExpNode.Name));
                break;
            }
        }
    }

    private static ASTNode GetASTNodeFromParam(Parameter param)
    {
        if (param.RawValue != null)
        {
            return GetASTNodeFromString(param.RawValue);
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

    private static ASTNode GetASTNodeFromString(string s)
    {
        if (s.StartsWith('"'))
        {
            if (!s.EndsWith('"'))
            {
                throw new Exception(
                    "A quotation mark at the beginning of an inline value must be closed by another quotation mark at the end of the value: " + s);
            }

            return new ASTStringExpression(s.Substring(1, s.Length - 2));
        }

        if (s.Contains('.'))
        {
            return new ASTNumberExpression(s, PrimitiveVariableType.DOUBLE);
        }

        var chars = s.ToCharArray();
        if (chars[0] == '-' || char.IsDigit(chars[0]))
        {
            return new ASTNumberExpression(s, PrimitiveVariableType.INTEGER);
        }

        if (s.Equals("true") || s.Equals("false"))
        {
            return new ASTBooleanExpression(s.Equals("true"));
        }

        return new ASTVariableExpression(s);
    }

    private static BinaryOperator ConvertBinOp(BinOp binOp)
    {
        return binOp switch
        {
            BinOp.Add => BinaryOperator.PLUS,
            BinOp.Subtract => BinaryOperator.MINUS,
            BinOp.Multiply => BinaryOperator.TIMES,
            BinOp.Divide => BinaryOperator.DIVIDE,
            BinOp.Modulo => BinaryOperator.MODULO,
            BinOp.LogicalAnd => BinaryOperator.AND,
            BinOp.LogicalOr => BinaryOperator.OR,
            BinOp.Equal => BinaryOperator.EQ,
            BinOp.NotEqual => BinaryOperator.NE,
            BinOp.GreaterThan => BinaryOperator.GT,
            BinOp.LessThan => BinaryOperator.LT,
            BinOp.GreaterThanOrEqual => BinaryOperator.GE,
            BinOp.LessThanOrEqual => BinaryOperator.LE,
            _ => throw new ArgumentOutOfRangeException(binOp.ToString())
        };
    }
    
}