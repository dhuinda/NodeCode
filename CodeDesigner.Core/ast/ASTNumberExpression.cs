using LLVMSharp;

namespace CodeDesigner.Core.ast;

using Core;

public class ASTNumberExpression : ASTNode
{

    private string Value;
    private PrimitiveVariableType Type;

    public ASTNumberExpression(string value, PrimitiveVariableType type)
    {
        Value = value;
        Type = type;
    }

    public override unsafe LLVMValueRef Codegen(CodegenData data)
    {
        if (Type == PrimitiveVariableType.INTEGER)
        {
            return LLVM.ConstIntOfString(LLVM.Int64TypeInContext(data.Context), Value, 10);
        }

        if (Type == PrimitiveVariableType.DOUBLE)
        {
            return LLVM.ConstRealOfString(LLVM.DoubleTypeInContext(data.Context), Value);
        }

        throw new Exception("unable to use PrimitiveVariableType " + Type + " as a number");
    }
}