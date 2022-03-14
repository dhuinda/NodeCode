using CodeDesigner.Core;
using CodeDesigner.Core.ast;

var ast = new List<ASTNode>();
ast.Add(
    new ASTPrototypeDeclaration("printf", new List<VariableType>
    {
        new(PrimitiveVariableType.STRING)
    }, new VariableType(PrimitiveVariableType.VOID), true)
);
ast.Add(new ASTFunctionDefinition(
    "main", 
    new List<ASTVariableDefinition>(),
    new List<ASTNode>
    {
        new ASTFunctionInvocation("extern.printf", new List<ASTNode>
        {
            new ASTStringExpression("1-2: %d\n"),
            new ASTFunctionInvocation("calculator.subtract", new List<ASTNode>
            {
                new ASTNumberExpression("1", PrimitiveVariableType.INTEGER),
                new ASTNumberExpression("2", PrimitiveVariableType.INTEGER)
            })
        })
    },
    new VariableType(PrimitiveVariableType.VOID)
));
ast.Add(new ASTNamespace("calculator", new List<ASTNode>
{
    new ASTFunctionDefinition(
         "add",
         new List<ASTVariableDefinition>
         {
             new("a", new VariableType(PrimitiveVariableType.INTEGER)),
             new("b", new VariableType(PrimitiveVariableType.INTEGER))
         },
         new List<ASTNode>
         {
             new ASTReturn(new ASTBinaryExpression(BinaryOperator.PLUS, new ASTVariableExpression("a"),
                 new ASTVariableExpression("b")))
         },
         new VariableType(PrimitiveVariableType.INTEGER)),
    new ASTFunctionDefinition("subtract", new List<ASTVariableDefinition>
    {
        new("a", new VariableType(PrimitiveVariableType.INTEGER)),
        new("b", new VariableType(PrimitiveVariableType.INTEGER))
    },
    new List<ASTNode>
        {
            new ASTReturn(new ASTFunctionInvocation("add", new List<ASTNode>
            {
                new ASTVariableExpression("a"),
                new ASTBinaryExpression(BinaryOperator.TIMES, new ASTNumberExpression("-1", PrimitiveVariableType.INTEGER), new ASTVariableExpression("b"))
            }))
        },
    new VariableType(PrimitiveVariableType.INTEGER))
}
));

CodeGenerator.Run(ast);
