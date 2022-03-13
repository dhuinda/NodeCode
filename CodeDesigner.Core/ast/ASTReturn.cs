using LLVMSharp.Interop;

namespace CodeDesigner.Core.ast;

public class ASTReturn : ASTNode
{
    private ASTNode? Expression // null means void
    {
        get;
        set;
    }

    public ASTReturn(ASTNode expression)
    {
        Expression = expression;
    }
    
    public override unsafe LLVMValueRef codegen(CodegenData data)
    {
        if (Expression != null)
        {
            LLVMValueRef val = Expression.codegen(data);
            return LLVM.BuildRet(data.Builder, val);
        }
        else
        {
            return LLVM.BuildRetVoid(data.Builder);
        }
    }
}