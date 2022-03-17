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

        var classData = data.Classes[className];

        LLVMValueRef method;
        if (classData.Methods.ContainsKey(MethodName) && classData.Methods[MethodName].IsVirtual)
        {
            uint methodIndex = 0;
            foreach (var m in classData.MethodOrder)
            {
                if (m == MethodName)
                {
                    break;
                }
                methodIndex++;
            }

            if (methodIndex == classData.MethodOrder.Count)
            {
                throw new Exception("method registered in data as a method, but not included in method order");
            }

            var vtable = LLVM.BuildLoad(data.Builder,
                LLVM.BuildStructGEP(data.Builder, obj, 0, ""), "");
            var gep = LLVM.BuildStructGEP(data.Builder, vtable, methodIndex, "");
            method = LLVM.BuildLoad(data.Builder, gep, "");
        }
        else
        {
            method = LLVM.GetNamedFunction(data.Module, $"{className}__{MethodName}");
            if (method.Pointer.ToInt64() == 0)
            {
                throw new InvalidCodeException("unknown method " + MethodName + " of class " + className);
            }
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