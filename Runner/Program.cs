// See https://aka.ms/new-console-template for more information

using CodeDesigner.Core;
using CodeDesigner.Core.ast;

List<ASTNode> ast = new List<ASTNode>();
ast.Add(new ASTFunctionDefinition(
    "ENTRY_FUNC", 
    new List<ASTVariableDefinition>(),
    new List<ASTNode>
    {
        new ASTReturn(new ASTBinaryExpression(
            BinaryOperator.GT,
            new ASTNumberExpression("2.5", PrimitiveVariableType.DOUBLE),
            new ASTNumberExpression("2", PrimitiveVariableType.DOUBLE)
        ))
    },
    new VariableType(true, PrimitiveVariableType.BOOLEAN, null)
));

CodeGenerator.Run(ast);
