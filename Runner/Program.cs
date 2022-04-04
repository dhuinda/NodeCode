using CodeDesigner.Core;
using CodeDesigner.Core.ast;

var ast = new List<ASTNode>();

ast.Add(new ASTPrototypeDeclaration("printf", new List<VariableType>
{
    new(PrimitiveVariableType.STRING)
}, new VariableType(PrimitiveVariableType.VOID), true));
// ast.Add(new ASTClassDefinition(new ClassType("ListNode", new List<ClassType>() { new("T") }), new List<ASTVariableDefinition>
// {
//     new("next", new VariableType(new ClassType("ListNode", new List<ClassType>() { new("T") }))),
//     new("len", new VariableType(PrimitiveVariableType.INTEGER)),
//     new("val", new VariableType(new ClassType("T", new List<ClassType>())))
// }, new List<ASTFunctionDefinition>
// {
//     new ASTFunctionDefinition("append", new List<ASTVariableDefinition> {
//         new ASTVariableDefinition("val", new VariableType(new ClassType("T")))
//     },
//     new List<ASTNode>
//     {
//         new ASTIfStatement(new ASTBinaryExpression(BinaryOperator.EQ, new ASTClassFieldAccess(new ASTVariableExpression("this"), "len"), new ASTNumberExpression("1", PrimitiveVariableType.INTEGER)),
//             new List<ASTNode>
//             {
//                 new ASTClassInstantiation("node", new ClassType("ListNode", new List<ClassType>
//                 {
//                     new("T")
//                 })),
//                 new ASTClassFieldStore(new ASTVariableExpression("node"), "len", new ASTNumberExpression("1", PrimitiveVariableType.INTEGER)),
//                 new ASTClassFieldStore(new ASTVariableExpression("node"), "val", new ASTVariableExpression("val")),
//                 new ASTClassFieldStore(new ASTVariableExpression("this"), "next", new ASTVariableExpression("node"))
//             }, new List<ASTNode>
//             {
//                 new ASTMethodInvocation(new ASTClassFieldAccess(new ASTVariableExpression("this"), "next"), "append", new List<ASTNode> { new ASTVariableExpression("val") })
//             }),
//         new ASTClassFieldStore(new ASTVariableExpression("this"), "len", new ASTBinaryExpression(BinaryOperator.PLUS, new ASTClassFieldAccess(new ASTVariableExpression("this"), "len"), new ASTNumberExpression("1", PrimitiveVariableType.INTEGER)))
//     }
//     , new VariableType(PrimitiveVariableType.VOID)),
//     new ASTFunctionDefinition("at", new List<ASTVariableDefinition>
//     {
//         new("index", new VariableType(PrimitiveVariableType.INTEGER))
//     },
//         new List<ASTNode>
//         {
//             new ASTIfStatement(new ASTBinaryExpression(BinaryOperator.EQ, new ASTVariableExpression("index"), new ASTNumberExpression("0", PrimitiveVariableType.INTEGER)), new List<ASTNode>
//             {
//                 new ASTReturn(new ASTClassFieldAccess(new ASTVariableExpression("this"), "val"))
//             }, new List<ASTNode>
//             {
//                 new ASTReturn(new ASTMethodInvocation(new ASTClassFieldAccess(new ASTVariableExpression("this"), "next"), "at", new List<ASTNode>
//                 {
//                     new ASTBinaryExpression(BinaryOperator.MINUS, new ASTVariableExpression("index"), new ASTNumberExpression("1", PrimitiveVariableType.INTEGER))
//                 }))
//             }),
//             new ASTReturn(new ASTNumberExpression("0", PrimitiveVariableType.INTEGER))
//         }, new VariableType(new ClassType("T"))),
//     new ASTFunctionDefinition("print", new List<ASTVariableDefinition>(), new List<ASTNode>
//     {
//         new ASTIfStatement(new ASTBinaryExpression(BinaryOperator.EQ, new ASTClassFieldAccess(new ASTVariableExpression("this"), "len"), new ASTNumberExpression("1", PrimitiveVariableType.INTEGER)),
//             new List<ASTNode>
//             {
//                 new ASTFunctionInvocation("extern.printf", new List<ASTNode>
//                 {
//                     new ASTStringExpression("%d\n"),
//                     new ASTClassFieldAccess(new ASTVariableExpression("this"), "val")
//                 })
//             }, new List<ASTNode>
//             {
//                 new ASTFunctionInvocation("extern.printf", new List<ASTNode>
//                 {
//                     new ASTStringExpression("%d->"),
//                     new ASTClassFieldAccess(new ASTVariableExpression("this"), "val")
//                 }),
//                 new ASTMethodInvocation(new ASTClassFieldAccess(new ASTVariableExpression("this"), "next"), "print", new List<ASTNode>())
//             })
//     }, new VariableType(PrimitiveVariableType.VOID))
// }));

ast.Add(new ASTFunctionDefinition("main", new List<ASTVariableDefinition>(), new List<ASTNode>
{
    new ASTFunctionInvocation("extern.printf", new List<ASTNode>
    {
        new ASTStringExpression("Hello, world!\n"),
    }),
    new ASTFunctionInvocation("extern.printf", new List<ASTNode>
        {
            new ASTStringExpression("Hello, world1!\n"),
        })
    // new ASTClassInstantiation("list", new ClassType("ListNode", new List<ClassType>
    // {
    //     new("Integer")
    // })),
    // new ASTClassFieldStore(new ASTVariableExpression("list"), "len", new ASTNumberExpression("1", PrimitiveVariableType.INTEGER)),
    // new ASTMethodInvocation(new ASTVariableExpression("list"), "append", new List<ASTNode> { new ASTNumberExpression("1", PrimitiveVariableType.INTEGER) }),
    // new ASTMethodInvocation(new ASTVariableExpression("list"), "append", new List<ASTNode> { new ASTNumberExpression("2", PrimitiveVariableType.INTEGER) }),
    // new ASTMethodInvocation(new ASTVariableExpression("list"), "append", new List<ASTNode> { new ASTNumberExpression("3", PrimitiveVariableType.INTEGER) }),
    // new ASTMethodInvocation(new ASTVariableExpression("list"), "append", new List<ASTNode> { new ASTNumberExpression("4", PrimitiveVariableType.INTEGER) }),
    // new ASTMethodInvocation(new ASTVariableExpression("list"), "append", new List<ASTNode> { new ASTNumberExpression("5", PrimitiveVariableType.INTEGER) }),
    // new ASTMethodInvocation(new ASTVariableExpression("list"), "print", new List<ASTNode>())
}, new(PrimitiveVariableType.VOID)));

CodeGenerator.Run(ast);
