using LLVMSharp.Interop;

namespace CodeDesigner.Core.ast;

public class VariableType
{

    public readonly bool IsPrimitive;

    public readonly PrimitiveVariableType? PrimitiveType;

    public readonly string? ClassName;

    public VariableType(bool isPrimitive, PrimitiveVariableType? primitiveType, string? className)
    {
        IsPrimitive = isPrimitive;
        PrimitiveType = primitiveType;
        ClassName = className;
    }

    public static unsafe LLVMTypeRef GetLLVMType(PrimitiveVariableType type, LLVMContextRef context)
    {
        return type switch
        {
            PrimitiveVariableType.INTEGER => LLVM.Int64TypeInContext(context),
            PrimitiveVariableType.DOUBLE => LLVM.DoubleTypeInContext(context),
            PrimitiveVariableType.BOOLEAN => LLVM.Int1TypeInContext(context),
            PrimitiveVariableType.VOID => LLVM.VoidTypeInContext(context),
            _ => throw new Exception("unimplemented primitive type")
        };
    }
    
}