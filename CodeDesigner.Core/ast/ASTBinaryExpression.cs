using LLVMSharp;

namespace CodeDesigner.Core.ast;

using Core;

public class ASTBinaryExpression : ASTNode
{
    private BinaryOperator Op;
    private ASTNode Lhs;
    private ASTNode Rhs;

    public ASTBinaryExpression(BinaryOperator op, ASTNode lhs, ASTNode rhs)
    {
        Op = op;
        Lhs = lhs;
        Rhs = rhs;
    }
    
    public override LLVMValueRef codegen(CodegenData data)
    {
        if (!data.Func.HasValue)
        {
            throw new Exception("expected to be inside a function");
        }
        if (Op is BinaryOperator.AND or BinaryOperator.OR) {
            LLVMValueRef lhsValue = Lhs.codegen(data);
            LLVMBasicBlockRef ifBB = LLVM.AppendBasicBlockInContext(data.Context, data.Func.Value, "condif");
            LLVMBasicBlockRef mergeBB = LLVM.AppendBasicBlockInContext(data.Context, data.Func.Value, "mergeif");
            LLVMValueRef resultAlloca = LLVM.BuildAlloca(data.Builder, LLVM.Int1TypeInContext(data.Context), "condtmp");
            LLVM.BuildStore(data.Builder, lhsValue, resultAlloca);
            if (Op == BinaryOperator.AND) {
                LLVM.BuildCondBr(data.Builder, lhsValue, ifBB, mergeBB);
            } else {
                LLVM.BuildCondBr(data.Builder, lhsValue, mergeBB, ifBB);
            }
            LLVM.PositionBuilderAtEnd(data.Builder, ifBB);
            LLVMValueRef rhsValue = Rhs.codegen(data);
            LLVM.BuildStore(data.Builder, rhsValue, resultAlloca);
            LLVM.BuildBr(data.Builder, mergeBB);
            LLVM.PositionBuilderAtEnd(data.Builder, mergeBB);
            return LLVM.BuildLoad(data.Builder, resultAlloca, "");
        }
        var l = Lhs.codegen(data);
        var r = Rhs.codegen(data);
        switch (Op) {
            case BinaryOperator.PLUS:
                if (!LLVM.IsAConstantInt(l).IsNull()) {
                    return LLVM.BuildAdd(data.Builder, l, r, "addtmp");
                }
                if (!LLVM.IsAConstantFP(l).IsNull()) {
                    return LLVM.BuildFAdd(data.Builder, l, r, "addtmp");
                }
                throw new Exception("Error: unknown add type");
            case BinaryOperator.MINUS:
                if (!LLVM.IsAConstantInt(l).IsNull()) {
                    return LLVM.BuildSub(data.Builder, l, r, "subtmp");
                }
                if (!LLVM.IsAConstantFP(l).IsNull()) {
                    return LLVM.BuildFSub(data.Builder, l, r, "subtmp");
                }
                throw new Exception("Error: unknown subtract type");
            case BinaryOperator.TIMES:
                if (!LLVM.IsAConstantInt(l).IsNull()) {
                    return LLVM.BuildMul(data.Builder, l, r, "multmp");
                }
                if (!LLVM.IsAConstantFP(l).IsNull()) {
                    return LLVM.BuildFMul(data.Builder, l, r, "multmp");
                }
                throw new Exception("Error: unknown multiplication type");
            case BinaryOperator.DIVIDE:
                if (!LLVM.IsAConstantInt(l).IsNull()) {
                    return LLVM.BuildSDiv(data.Builder, l, r, "divtmp");
                }
                if (!LLVM.IsAConstantFP(l).IsNull()) {
                    return LLVM.BuildFDiv(data.Builder, l, r, "divtmp");
                }
                throw new Exception("Error: unknown division type");
            case BinaryOperator.LT:
                if (!LLVM.IsAConstantInt(l).IsNull()) {
                    return LLVM.BuildICmp(data.Builder, LLVMIntPredicate.LLVMIntSLT, l, r, "cmptmp");
                }
                if (!LLVM.IsAConstantFP(l).IsNull()) {
                    return LLVM.BuildFCmp(data.Builder, LLVMRealPredicate.LLVMRealOLT, l, r, "cmptmp");
                }
                throw new Exception("Error: unknown comparison type");
            case BinaryOperator.GT:
                if (!LLVM.IsAConstantInt(l).IsNull()) {
                    return LLVM.BuildICmp(data.Builder, LLVMIntPredicate.LLVMIntSGT, l, r, "cmptmp");
                }
                if (!LLVM.IsAConstantFP(l).IsNull()) {
                    return LLVM.BuildFCmp(data.Builder, LLVMRealPredicate.LLVMRealOGT, l, r, "cmptmp");
                }
                throw new Exception("Error: unknown comparison type");
            case BinaryOperator.LE:
                if (!LLVM.IsAConstantInt(l).IsNull()) {
                    return LLVM.BuildICmp(data.Builder, LLVMIntPredicate.LLVMIntSLE, l, r, "cmptmp");
                }
                if (!LLVM.IsAConstantFP(l).IsNull()) {
                    return LLVM.BuildFCmp(data.Builder, LLVMRealPredicate.LLVMRealOLT, l, r, "cmptmp");
                }
                throw new Exception("Error: unknown comparison type");
            case BinaryOperator.GE:
                if (!LLVM.IsAConstantInt(l).IsNull()) {
                    return LLVM.BuildICmp(data.Builder, LLVMIntPredicate.LLVMIntSGT, l, r, "cmptmp");
                }
                if (!LLVM.IsAConstantFP(l).IsNull()) {
                    return LLVM.BuildFCmp(data.Builder, LLVMRealPredicate.LLVMRealOGT, l, r, "cmptmp");
                }
                throw new Exception("Error: unknown comparison type");
            case BinaryOperator.EQ:
                if (!LLVM.IsAConstantInt(l).IsNull()) {
                    return LLVM.BuildICmp(data.Builder, LLVMIntPredicate.LLVMIntEQ, l, r, "cmptmp");
                }
                if (!LLVM.IsAConstantFP(l).IsNull()) {
                    return LLVM.BuildFCmp(data.Builder, LLVMRealPredicate.LLVMRealOEQ, l, r, "cmptmp");
                }
                throw new Exception("Error: unknown comparison type");
            case BinaryOperator.NE:
                if (!LLVM.IsAConstantInt(l).IsNull()) {
                    return LLVM.BuildICmp(data.Builder, LLVMIntPredicate.LLVMIntNE, l, r, "cmptmp");
                }
                if (!LLVM.IsAConstantFP(l).IsNull()) {
                    return LLVM.BuildFCmp(data.Builder, LLVMRealPredicate.LLVMRealONE, l, r, "cmptmp");
                }
                throw new Exception("Error: unknown comparison type");
            case BinaryOperator.MODULO:
                if (!LLVM.IsAConstantInt(l).IsNull()) {
                    return LLVM.BuildSRem(data.Builder, l, r, "modtmp");
                }
                if (!LLVM.IsAConstantFP(l).IsNull()) {
                    return LLVM.BuildFRem(data.Builder, l, r, "modtmp");
                }
                throw new Exception("Error: unknown modulo type");
            default:
                throw new Exception("Error: unimplemented binary expression");
        }

    }
}