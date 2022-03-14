using LLVMSharp;

namespace CodeDesigner.Core.ast;

public class ASTFunctionInvocation : ASTNode
{
    public string Name;
    public List<ASTNode> Args;

    public ASTFunctionInvocation(string name, List<ASTNode> args)
    {
        Name = name;
        Args = args;
    }

    public override LLVMValueRef codegen(CodegenData data)
    {
        var func = LLVM.GetNamedFunction(data.Module, Name);
        var argsV = new List<LLVMValueRef>();
        foreach (var arg in Args)
        {
            argsV.Add(arg.codegen(data));
        }

        return LLVM.BuildCall(data.Builder, func, argsV.ToArray(), "");
    }
}