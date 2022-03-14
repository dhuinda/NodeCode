// See https://aka.ms/new-console-template for more information

using CodeDesigner.Core;
using CodeDesigner.Core.ast;

var ast = new List<ASTNode>();
ast.Add(
    new ASTPrototypeDeclaration("printf", new List<VariableType>
    {
        new(PrimitiveVariableType.STRING)
    }, new VariableType(PrimitiveVariableType.VOID), true)
);
ast.Add(new ASTPrototypeDeclaration("LibrarySumFunc",
    new List<VariableType>
    {
    new(PrimitiveVariableType.INTEGER),
    new(PrimitiveVariableType.INTEGER)
    },
    new VariableType(PrimitiveVariableType.INTEGER),
    false
));
ast.Add(new ASTFunctionDefinition(
    "ENTRY_FUNC", 
    new List<ASTVariableDefinition>(),
    new List<ASTNode>
    {
        new ASTFunctionInvocation("printf", new List<ASTNode>
        {
            new ASTStringExpression("1+2: %d\n"),
            new ASTFunctionInvocation("LibrarySumFunc", new List<ASTNode>
            {
                new ASTNumberExpression("1", PrimitiveVariableType.INTEGER),
                new ASTNumberExpression("2", PrimitiveVariableType.INTEGER)
            })
        })
    },
    new VariableType(PrimitiveVariableType.VOID)
));
ast.Add(new ASTFunctionDefinition(
     "LibrarySumFunc",
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
     new VariableType(PrimitiveVariableType.INTEGER)
));

CodeGenerator.Run(ast);
