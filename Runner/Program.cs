// See https://aka.ms/new-console-template for more information

using CodeDesigner.Core;
using CodeDesigner.Core.ast;

var ast = new List<ASTNode>();
ast.Add(new ASTFunctionDefinition(
    "ENTRY_FUNC", 
    new List<ASTVariableDefinition>(),
    new List<ASTNode>
    {
        new ASTVariableDeclaration("myVar", new VariableType(PrimitiveVariableType.STRING), new ASTStringExpression("Hello, world!")),
        new ASTReturn(new ASTVariableExpression("myVar"))
    },
    new VariableType(PrimitiveVariableType.STRING)
));

CodeGenerator.Run(ast);
