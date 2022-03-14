using LLVMSharp;

namespace CodeDesigner.Core.ast;

public class ASTPrototypeDeclaration : ASTNode
{
    public string Name;
    public List<VariableType> ParamTypes;
    public VariableType ReturnType;
    public bool IsVarArgs;
    public string? NamespaceName; // this needs to be filled in during code analysis (which is also when ASTPrototypeDeclarations are generated)

    public ASTPrototypeDeclaration(string name, List<VariableType> paramTypes, VariableType returnType,
        bool isVarArgs, string? namespaceName)
    {
        Name = name;
        ParamTypes = paramTypes;
        ReturnType = returnType;
        IsVarArgs = isVarArgs;
        NamespaceName = namespaceName;
    }

    public override LLVMValueRef codegen(CodegenData data)
    {
        string fullName;
        if (NamespaceName != null)
        {
            fullName = NamespaceName.Equals("extern") ? Name : $"{NamespaceName}.{Name}";
        }
        else
        {
            fullName = $"default.{Name}";
        }
        Console.WriteLine("prototype for: " + fullName);
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
        return LLVM.AddFunction(data.Module, fullName, funcType);
    }
}