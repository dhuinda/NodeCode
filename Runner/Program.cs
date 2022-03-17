using CodeDesigner.Core;
using CodeDesigner.Core.ast;

var ast = new List<ASTNode>();

ast.Add(new ASTPrototypeDeclaration("printf", new List<VariableType>
{
    new(PrimitiveVariableType.STRING)
}, new VariableType(PrimitiveVariableType.VOID), true));

ast.Add(new ASTClassDefinition("MyClass", new List<ASTVariableDefinition>
{
    new("a", new VariableType(PrimitiveVariableType.INTEGER)),
    new("b", new VariableType("MyClass"))
}, new List<ASTFunctionDefinition>
{
    new("test", new List<ASTVariableDefinition>(), new List<ASTNode>
    {
        new ASTFunctionInvocation("extern.printf", new List<ASTNode>
        {
            new ASTStringExpression("this.a: %d\n"),
            new ASTClassFieldAccess(new ASTVariableExpression("this"), "a")
        })
    }, new VariableType(PrimitiveVariableType.VOID))
}));
ast.Add(new ASTFunctionDefinition("main", new List<ASTVariableDefinition>(), new List<ASTNode>
{
    new ASTClassInstantiation("myClassInst", "MyClass", new List<ASTNode>(), new List<VariableType>()),
    new ASTClassFieldStore(new ASTVariableExpression("myClassInst"), "a", new ASTNumberExpression("2", PrimitiveVariableType.INTEGER)),
    new ASTMethodInvocation(new ASTVariableExpression("myClassInst"), "test", new List<ASTNode>()),
    new ASTClassInstantiation("myNestedClassInst", "MyClass", new List<ASTNode>(), new List<VariableType>()),
    new ASTClassFieldStore(new ASTVariableExpression("myClassInst"), "b", new ASTVariableExpression("myNestedClassInst")),
    new ASTClassFieldStore(new ASTClassFieldAccess(new ASTVariableExpression("myClassInst"), "b"), "a", new ASTNumberExpression("1337", PrimitiveVariableType.INTEGER)),
    new ASTMethodInvocation(new ASTClassFieldAccess(new ASTVariableExpression("myClassInst"), "b"), "test", new List<ASTNode>())
}, new(PrimitiveVariableType.VOID)));

CodeGenerator.Run(ast);
