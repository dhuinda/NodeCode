using LLVMSharp;

namespace CodeDesigner.Core.ast;

public class ASTBooleanExpression : ASTNode
{
    private bool Value;

    public ASTBooleanExpression(bool value)
    {
        Value = value;
    }

    public override LLVMValueRef codegen(CodegenData data)
    {
        return LLVM.ConstInt(LLVM.Int1TypeInContext(data.Context), (ulong) (Value ? 1 : 0), false);
    }
}