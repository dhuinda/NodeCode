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
        new ASTFunctionInvocation("printf", new List<ASTNode>
        {
            new ASTStringExpression("Hello, world!\n")
        })
    },
    new VariableType(PrimitiveVariableType.VOID)
));

CodeGenerator.Run(ast);
