using LLVMSharp;

namespace CodeDesigner.Core.ast;

using Core;

public class ASTVariableExpression : ASTNode
{
    private readonly string Name;

    public ASTVariableExpression(string name)
    {
        Name = name;
    }

    public override LLVMValueRef codegen(CodegenData data)
    {
        if (!data.NamedValues.ContainsKey(Name))
        {
            // todo: it would be nice to report back exactly which block the error is at (if possible, which it would be for errors like this)
            throw new InvalidCodeException("unknown variable identifier " + Name + ". Maybe the variable isn't in the correct context?");
        }
        return LLVM.BuildLoad(data.Builder, data.NamedValues[Name], Name);
    }
}