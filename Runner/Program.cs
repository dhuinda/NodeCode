using CodeDesigner.Core;
using CodeDesigner.Core.ast;

var ast = new List<ASTNode>();

ast.Add(new ASTPrototypeDeclaration("printf", new List<VariableType>
{
    new(PrimitiveVariableType.STRING)
}, new VariableType(PrimitiveVariableType.VOID), true));

ast.Add(new ASTClassDefinition(new ClassType("MyClass", new List<ClassType>
    {
        new("T")
    }), new List<ASTVariableDefinition>
{
    new("a", new VariableType(PrimitiveVariableType.INTEGER)),
    new("b", new VariableType(new ClassType("MyClass", new List<ClassType>
    {
        new ClassType("Integer")
    }))),
    new("c", new VariableType(new ClassType("T")))
}, new List<ASTFunctionDefinition>
{
    new("test", new List<ASTVariableDefinition>(), new List<ASTNode>
    {
        new ASTFunctionInvocation("extern.printf", new List<ASTNode>
        {
            new ASTStringExpression("this.a: %d\n"),
            new ASTClassFieldAccess(new ASTVariableExpression("this"), "a")
        })
    }, new VariableType(PrimitiveVariableType.VOID)),
    new("getC", new List<ASTVariableDefinition>(), new List<ASTNode>
    {
        new ASTReturn(new ASTClassFieldAccess(new ASTVariableExpression("this"), "c"))
    }, new VariableType(new ClassType("T")))
},
    new Dictionary<string, MethodAttributes>
    {
        {"test", new MethodAttributes(true)}
    },
    new List<List<VariableType>>
    {
        new()
        {
            new VariableType(PrimitiveVariableType.DOUBLE)
        },
        new()
        {
            new(PrimitiveVariableType.INTEGER)
        }
    }));
ast.Add(new ASTFunctionDefinition("main", new List<ASTVariableDefinition>(), new List<ASTNode>
{
    new ASTClassInstantiation("myClassInst", new("MyClass", new  List<ClassType>() { new("Double") }), new List<ASTNode>(), new List<VariableType>()), // todo: allow primitives as generic type in API and then wrap them in the constructor
    new ASTClassFieldStore(new ASTVariableExpression("myClassInst"), "a", new ASTNumberExpression("2", PrimitiveVariableType.INTEGER)),
    new ASTMethodInvocation(new ASTVariableExpression("myClassInst"), "test", new List<ASTNode>()),
    new ASTClassInstantiation("myNestedClassInst", new("MyClass", new List<ClassType>() { new("Integer") }), new List<ASTNode>(), new List<VariableType>()),
    new ASTClassFieldStore(new ASTVariableExpression("myNestedClassInst"), "c", new ASTNumberExpression("7331", PrimitiveVariableType.INTEGER)),
    new ASTClassFieldStore(new ASTVariableExpression("myClassInst"), "b", new ASTVariableExpression("myNestedClassInst")),
    new ASTFunctionInvocation("extern.printf", new List<ASTNode>()
    {
        new ASTStringExpression("myClassInst.b.c: %d\n"),
        new ASTMethodInvocation(new ASTClassFieldAccess(new ASTVariableExpression("myClassInst"), "b"), "getC", new List<ASTNode>())
    }),
    new ASTClassFieldStore(new ASTClassFieldAccess(new ASTVariableExpression("myClassInst"), "b"), "a", new ASTNumberExpression("1337", PrimitiveVariableType.INTEGER)),
    new ASTMethodInvocation(new ASTClassFieldAccess(new ASTVariableExpression("myClassInst"), "b"), "test", new List<ASTNode>()),
    new ASTClassFieldStore(new ASTVariableExpression("myClassInst"), "c", new ASTNumberExpression("420", PrimitiveVariableType.DOUBLE)),
    new ASTVariableDeclaration("c", new VariableType(PrimitiveVariableType.DOUBLE), new ASTMethodInvocation(new ASTVariableExpression("myClassInst"), "getC", new List<ASTNode>())),
    new ASTFunctionInvocation("extern.printf", new List<ASTNode>
    {
        new ASTStringExpression("this.c: %.6f\n"),
        new ASTVariableExpression("c")
    })
}, new(PrimitiveVariableType.VOID)));

CodeGenerator.Run(ast);
