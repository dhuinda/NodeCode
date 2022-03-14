// See https://aka.ms/new-console-template for more information

using CodeDesigner.Core;
using CodeDesigner.Core.ast;

var ast = new List<ASTNode>();
ast.Add(new ASTFunctionDefinition(
    "ENTRY_FUNC", 
    new List<ASTVariableDefinition>(),
    new List<ASTNode>
    {
        new ASTExternDeclaration("printf", new List<VariableType>
        {
            new(PrimitiveVariableType.STRING)
        }, PrimitiveVariableType.VOID, true),
        new ASTIfStatement(new ASTBinaryExpression(BinaryOperator.GT, new ASTNumberExpression("2", PrimitiveVariableType.INTEGER), new ASTNumberExpression("1", PrimitiveVariableType.INTEGER)),
                new List<ASTNode>()
                {
                    new ASTFunctionInvocation("printf", new List<ASTNode>
                    {
                        new ASTStringExpression("true\n")
                    })
                },
                new List<ASTNode>()
            )
    },
    new VariableType(PrimitiveVariableType.VOID)
));

CodeGenerator.Run(ast);
