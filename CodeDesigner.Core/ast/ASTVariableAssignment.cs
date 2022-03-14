using LLVMSharp;

namespace CodeDesigner.Core.ast;

public class ASTVariableAssignment : ASTNode
{
    public string Name;
    public ASTNode Value;

    public ASTVariableAssignment(string name, ASTNode value)
    {
        Name = name;
        Value = value;
    }

    public override LLVMValueRef codegen(CodegenData data)
    {
        if (!data.NamedValues.ContainsKey(Name))
        {
            throw new InvalidCodeException("unknown variable " + Name);
        }

        var llvmVar = data.NamedValues[Name];
        LLVM.BuildStore(data.Builder, Value.codegen(data), llvmVar);
        return llvmVar;
    }
}