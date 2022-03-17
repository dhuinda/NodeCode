using LLVMSharp;

namespace CodeDesigner.Core.ast;

// todo: ASTNewExpression for class instantiations without identifiers
public class ASTClassInstantiation : ASTNode
{
    public string Identifier;
    public string ClassName;
    public List<ASTNode> Args;
    public List<VariableType> GenericTypes;

    public ASTClassInstantiation(string identifier, string className, List<ASTNode> args,
        List<VariableType> genericTypes)
    {
        Identifier = identifier;
        ClassName = className;
        Args = args;
        GenericTypes = genericTypes;
    }

    public override LLVMValueRef Codegen(CodegenData data)
    {
        var fullName = ClassName.Contains('.') ? ClassName : $"default.{ClassName}";
        if (!data.Classes.ContainsKey(fullName))
        {
            throw new InvalidCodeException("cannot find class " + fullName + " to instantiate");
        }

        var classData = data.Classes[fullName];
        var alloca = LLVM.BuildAlloca(data.Builder, LLVM.PointerType(classData.Type, 0), Identifier);
        var inst = LLVM.BuildMalloc(data.Builder, classData.Type, Identifier);
        LLVM.BuildStore(data.Builder, inst, alloca);
        data.NamedValues.Add(Identifier, alloca);
        data.ObjectTypes.Add(Identifier, fullName);
        // todo: constructors
        if (classData.VtableGlobal.HasValue)
        {
            var gep = LLVM.BuildStructGEP(data.Builder, LLVM.BuildLoad(data.Builder, alloca, ""), 0, "");
            LLVM.BuildStore(data.Builder, classData.VtableGlobal.Value, gep);
        }
        return alloca;
    }
}