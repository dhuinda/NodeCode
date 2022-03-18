using LLVMSharp;

namespace CodeDesigner.Core.ast;

using Core;

public class ASTVariableDeclaration : ASTNode
{

    public string Name;
    public VariableType Type;
    public ASTNode Value;

    public ASTVariableDeclaration(string name, VariableType type, ASTNode value)
    {
        Name = name;
        Type = type;
        Value = value;
    }

    public override LLVMValueRef Codegen(CodegenData data)
    {
        if (!Type.IsPrimitive)
        {
            throw new Exception("classes aren't implemented yet");
        }

        var llvmType = Type.GetLLVMType(data);
        var alloca = LLVM.BuildAlloca(data.Builder, llvmType, Name);
        LLVM.BuildStore(data.Builder, Value.Codegen(data), alloca);
        data.NamedValues.Add(Name, alloca);
        return alloca;
    }
}