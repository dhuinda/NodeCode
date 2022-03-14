using LLVMSharp;

namespace CodeDesigner.Core.ast;

public class ASTExternDeclaration : ASTNode
{
    public string Name;
    public List<VariableType> ArgTypes;
    public PrimitiveVariableType ReturnType;
    public bool IsVarArgs;

    public ASTExternDeclaration(string name, List<VariableType> argTypes, PrimitiveVariableType returnType,
        bool isVarArgs)
    {
        Name = name;
        ArgTypes = argTypes;
        ReturnType = returnType;
        IsVarArgs = isVarArgs;
    }

    public override LLVMValueRef codegen(CodegenData data)
    {
        var llvmArgTypes = new List<LLVMTypeRef>();
        foreach (var argType in ArgTypes)
        {
            if (argType.IsPrimitive)
            {
                llvmArgTypes.Add(VariableType.GetLLVMType(argType.PrimitiveType!.Value, data.Context));
            }
            else
            {
                Console.WriteLine("classes aren't implemented yet");
            }
        }

        var funcType = LLVM.FunctionType(VariableType.GetLLVMType(ReturnType, data.Context), llvmArgTypes.ToArray(), IsVarArgs);
        return LLVM.AddFunction(data.Module, Name, funcType);
    }
}