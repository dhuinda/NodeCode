using LLVMSharp;

namespace CodeDesigner.Core.ast;

public class ASTNotExpression : ASTNode
{
    public ASTNode Value;

    public ASTNotExpression(ASTNode value)
    {
        Value = value;
    }
    
    public override LLVMValueRef Codegen(CodegenData data)
    {
        return LLVM.BuildNot(data.Builder, Value.Codegen(data), "not");
    }
}