using LLVMSharp;

namespace CodeDesigner.Core.ast;

public class ASTMethodInvocation : ASTNode
{
    public ASTNode Object;
    public string MethodName;
    public List<ASTNode> Args;

    public ASTMethodInvocation(ASTNode obj, string methodName, List<ASTNode> args)
    {
        Object = obj;
        MethodName = methodName;
        Args = args;
    }
    
    
    public override LLVMValueRef Codegen(CodegenData data)
    {
        var obj = Object.Codegen(data);
        if (LLVM.GetTypeKind(LLVM.TypeOf(obj)) != LLVMTypeKind.LLVMPointerTypeKind ||
            LLVM.GetTypeKind(LLVM.GetElementType(LLVM.TypeOf(obj))) != LLVMTypeKind.LLVMStructTypeKind)
        {
            throw new InvalidCodeException("unexpected method call on non-object variable");
        }

        var className = VariableType.GetClassNameOfObject(obj);
        if (!data.Classes.ContainsKey(className))
        {
            throw new Exception("unknown class " + className);
        }

        var method = LLVM.GetNamedFunction(data.Module, $"{className}__{MethodName}");
        if (method.Pointer.ToInt64() == 0)
        {
            throw new InvalidCodeException("unknown method " + MethodName + " of class " + className);
        }

        var llvmArgs = new List<LLVMValueRef>();
        foreach (var arg in Args)
        {
            llvmArgs.Add(arg.Codegen(data));
        }
        llvmArgs.Add(obj); // Add reference to object ("this")
        return LLVM.BuildCall(data.Builder, method, llvmArgs.ToArray(), "");
    }
}