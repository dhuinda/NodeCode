using LLVMSharp;

namespace CodeDesigner.Core.ast;

using Core;

public class ASTVariableDeclaration : ASTNode
{

    private string Name;
    private VariableType Type;
    private ASTNode Value;

    public ASTVariableDeclaration(string name, VariableType type, ASTNode value)
    {
        Name = name;
        Type = type;
        Value = value;
    }

    public override LLVMValueRef codegen(CodegenData data)
    {
        if (!Type.IsPrimitive)
        {
            throw new Exception("classes aren't implemented yet");
        }

        var llvmType = Type.GetLLVMType(data);
        var alloca = LLVM.BuildAlloca(data.Builder, llvmType, Name);
        LLVM.BuildStore(data.Builder, Value.codegen(data), alloca);
        data.NamedValues.Add(Name, alloca);
        return alloca;
    }
}