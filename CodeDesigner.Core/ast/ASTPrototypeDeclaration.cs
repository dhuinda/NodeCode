using LLVMSharp;

namespace CodeDesigner.Core.ast;

public class ASTPrototypeDeclaration : ASTNode
{
    public string Name;
    public List<VariableType> ParamTypes;
    public VariableType ReturnType;
    public bool IsVarArgs;

    public ASTPrototypeDeclaration(string name, List<VariableType> paramTypes, VariableType returnType,
        bool isVarArgs)
    {
        Name = name;
        ParamTypes = paramTypes;
        ReturnType = returnType;
        IsVarArgs = isVarArgs;
    }

    public override LLVMValueRef codegen(CodegenData data)
    {
        var llvmParamTypes = new List<LLVMTypeRef>();
        foreach (var argType in ParamTypes)
        {
            if (argType.IsPrimitive)
            {
                llvmParamTypes.Add(VariableType.GetLLVMType(argType.PrimitiveType!.Value, data.Context));
            }
            else
            {
                Console.WriteLine("classes aren't implemented yet");
            }
        }

        if (!ReturnType.IsPrimitive)
        {
            throw new Exception("classes aren't implemented yet");
        }
        var funcType = LLVM.FunctionType(VariableType.GetLLVMType(ReturnType.PrimitiveType!.Value, data.Context), llvmParamTypes.ToArray(), IsVarArgs);
        return LLVM.AddFunction(data.Module, Name, funcType);
    }
}