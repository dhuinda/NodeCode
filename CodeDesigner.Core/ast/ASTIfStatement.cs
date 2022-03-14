using LLVMSharp;

namespace CodeDesigner.Core.ast;

public class ASTIfStatement : ASTNode
{
    public ASTNode Condition;
    public List<ASTNode> IfBody;
    public List<ASTNode> ElseBody;

    public ASTIfStatement(ASTNode condition, List<ASTNode> ifBody, List<ASTNode> elseBody)
    {
        Condition = condition;
        IfBody = ifBody;
        ElseBody = elseBody;
    }
    
    public override LLVMValueRef codegen(CodegenData data)
    {
        var conditionValue = Condition.codegen(data);
        if (!data.Func.HasValue)
        {
            throw new Exception("expected to be inside a function");
        }
        var ifBlock = LLVM.AppendBasicBlockInContext(data.Context, data.Func.Value, "ifbody");
        LLVMBasicBlockRef? elseBlock = null;
        if (ElseBody.Count != 0)
        {
            elseBlock = LLVM.AppendBasicBlockInContext(data.Context, data.Func.Value, "elsebody");
        }
        var mergeBlock = LLVM.AppendBasicBlockInContext(data.Context, data.Func.Value, "mergeif");
        // todo: if empty ElseBody, skip straight to mergeif (and don't add elsebody BasicBlock)
        LLVM.BuildCondBr(data.Builder, conditionValue, ifBlock, elseBlock.HasValue ? elseBlock.Value : mergeBlock);

        LLVM.PositionBuilderAtEnd(data.Builder, ifBlock);
        foreach (var node in IfBody)
        {
            node.codegen(data);
        }
        LLVM.BuildBr(data.Builder, mergeBlock);

        if (elseBlock.HasValue)
        {
            LLVM.PositionBuilderAtEnd(data.Builder, elseBlock.Value);
            foreach (var node in ElseBody)
            {
                node.codegen(data);
            }

            LLVM.BuildBr(data.Builder, mergeBlock);
        }
        LLVM.PositionBuilderAtEnd(data.Builder, mergeBlock);
        return mergeBlock;
    }
}