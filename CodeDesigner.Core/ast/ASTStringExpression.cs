using LLVMSharp.Interop;

namespace CodeDesigner.Core.ast;

using Core;

public class ASTStringExpression : ASTNode
{
    private string Value;

    public ASTStringExpression(string value)
    {
        Value = value;
    }
    
    public override unsafe LLVMValueRef codegen(CodegenData data)
    {
        return LLVM.BuildGlobalStringPtr(data.Builder, CodeGenerator.StringToSBytes(Value), CodeGenerator.StringToSBytes("string"));
    }
}