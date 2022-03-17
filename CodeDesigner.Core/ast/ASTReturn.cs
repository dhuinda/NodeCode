using LLVMSharp;

namespace CodeDesigner.Core.ast;

public class ASTReturn : ASTNode
{
    public ASTNode? Expression; // null means void

    public ASTReturn(ASTNode? expression)
    {
        Expression = expression;
    }
    
    public override LLVMValueRef Codegen(CodegenData data)
    {
        if (Expression == null)
        {
            return LLVM.BuildRetVoid(data.Builder);
        }
        LLVMValueRef val = Expression.Codegen(data);
        return LLVM.BuildRet(data.Builder, val);
    }
}