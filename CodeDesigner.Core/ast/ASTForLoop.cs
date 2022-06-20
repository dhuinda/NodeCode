using LLVMSharp;

namespace CodeDesigner.Core.ast;

public class ASTForLoop : ASTNode
{
    public ASTNode? Initializer;
    public ASTNode? Condition;
    public ASTNode? Action;
    public List<ASTNode> Body;

    public ASTForLoop(List<ASTNode> body, ASTNode? initializer = null, ASTNode? condition = null, ASTNode? action = null)
    {
        Initializer = initializer;
        Condition = condition;
        Action = action;
        Body = body;
    }
    
    public override LLVMValueRef Codegen(CodegenData data)
    {
        if (!data.Func.HasValue)
        {
            throw new Exception("for loop expected to be inside a function");
        }

        Initializer?.Codegen(data);
        var loopBlock = LLVM.AppendBasicBlockInContext(data.Context, data.Func.Value, "forbody");
        var mergeBlock = LLVM.AppendBasicBlockInContext(data.Context, data.Func.Value, "mergewhile");
        if (Condition != null)
        {
            var initialCondition = Condition.Codegen(data);
            LLVM.BuildCondBr(data.Builder, initialCondition, loopBlock, mergeBlock);
        }
        else
        {
            LLVM.BuildBr(data.Builder, loopBlock);
        }
        
        LLVM.PositionBuilderAtEnd(data.Builder, loopBlock);
        foreach (var node in Body)
        {
            node.Codegen(data);
        }

        Action?.Codegen(data);
        if (Condition != null)
        {
            var terminationVal = Condition.Codegen(data);
            LLVM.BuildCondBr(data.Builder, terminationVal, loopBlock, mergeBlock);
        }
        else
        {
            LLVM.BuildBr(data.Builder, loopBlock);
        }
        LLVM.PositionBuilderAtEnd(data.Builder, mergeBlock);
        return mergeBlock;
    }
}