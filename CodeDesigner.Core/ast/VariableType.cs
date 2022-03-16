using System.Diagnostics.Tracing;
using LLVMSharp;

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

    public VariableType(PrimitiveVariableType primitiveType)
    {
        IsPrimitive = true;
        PrimitiveType = primitiveType;
    }

    public VariableType(string className)
    {
        IsPrimitive = false;
        ClassName = className;
    }

    public LLVMTypeRef GetLLVMType(CodegenData data)
    {
        if (IsPrimitive)
        {
            if (!PrimitiveType.HasValue)
            {
                throw new Exception("expected primitive type to have value");
            }
            return PrimitiveType.Value switch
            {
                PrimitiveVariableType.INTEGER => LLVM.Int64TypeInContext(data.Context),
                PrimitiveVariableType.DOUBLE => LLVM.DoubleTypeInContext(data.Context),
                PrimitiveVariableType.BOOLEAN => LLVM.Int1TypeInContext(data.Context),
                PrimitiveVariableType.VOID => LLVM.VoidTypeInContext(data.Context),
                PrimitiveVariableType.STRING => LLVM.PointerType(LLVM.Int8TypeInContext(data.Context), 0),
                _ => throw new Exception("unimplemented primitive type")
            };
        }

        if (ClassName == null)
        {
            throw new Exception("expected object type to have a class name");
        }

        var fullClassName = ClassName.Contains('.') ? ClassName! : $"default.{ClassName}";
        if (!data.Classes.ContainsKey(fullClassName))
        {
            throw new InvalidCodeException("unknown class " + fullClassName);
        }

        return LLVM.PointerType(data.Classes[fullClassName].Type, 0);
    }

    public static string GetClassNameOfObject(LLVMValueRef obj)
    {
        var className = LLVM.TypeOf(obj).ToString();
        if (className.StartsWith('%'))
        {
            className = className[1..];
        }

        if (className.EndsWith('*'))
        {
            className = className[..^1];
        }

        return className;
    }
    
}