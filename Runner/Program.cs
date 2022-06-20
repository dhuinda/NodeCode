﻿using CodeDesigner.Core;
using CodeDesigner.Core.ast;

var ast = new List<ASTNode>();

ast.Add(new ASTPrototypeDeclaration("printf", new List<VariableType>
{
    new(PrimitiveVariableType.STRING)
}, new VariableType(PrimitiveVariableType.VOID), true));

ast.Add(new ASTFunctionDefinition("main", new List<ASTVariableDefinition>(), new List<ASTNode>
{
    new ASTForLoop(
        new List<ASTNode>
        {
            new ASTFunctionInvocation("extern.printf", new List<ASTNode>
            {
                new ASTStringExpression("Hello, world!\n")
            })
        }, 
        new ASTVariableDeclaration("i", new VariableType(PrimitiveVariableType.INTEGER), new ASTNumberExpression("0", PrimitiveVariableType.INTEGER)),
        new ASTBinaryExpression(BinaryOperator.LT,
            new ASTVariableExpression("i"),
            new ASTNumberExpression("5", PrimitiveVariableType.INTEGER)),
        new ASTVariableAssignment("i",
            new ASTBinaryExpression(BinaryOperator.PLUS,
                new ASTVariableExpression("i"),
                new ASTNumberExpression("1", PrimitiveVariableType.INTEGER)))
        )
}, new(PrimitiveVariableType.VOID)));

CodeGenerator.Run(ast);
