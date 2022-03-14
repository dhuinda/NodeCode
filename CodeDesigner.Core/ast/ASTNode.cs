using LLVMSharp;

namespace CodeDesigner.Core;

public abstract class ASTNode
{
    public abstract LLVMValueRef codegen(CodegenData data);
}