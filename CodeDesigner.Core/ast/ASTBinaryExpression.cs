using LLVMSharp;

namespace CodeDesigner.Core.ast;

using Core;

public class ASTBinaryExpression : ASTNode
{
    public BinaryOperator Op;
    public ASTNode Lhs;
    public ASTNode Rhs;

    public ASTBinaryExpression(BinaryOperator op, ASTNode lhs, ASTNode rhs)
    {
        Op = op;
        Lhs = lhs;
        Rhs = rhs;
    }
    
    public override LLVMValueRef Codegen(CodegenData data)
    {
        if (!data.Func.HasValue)
        {
            throw new Exception("expected to be inside a function");
        }
        if (Op is BinaryOperator.AND or BinaryOperator.OR) {
            LLVMValueRef lhsValue = Lhs.Codegen(data);
            LLVMBasicBlockRef ifBlock = LLVM.AppendBasicBlockInContext(data.Context, data.Func.Value, "condif");
            LLVMBasicBlockRef mergeBlock = LLVM.AppendBasicBlockInContext(data.Context, data.Func.Value, "mergeif");
            LLVMValueRef resultAlloca = LLVM.BuildAlloca(data.Builder, LLVM.Int1TypeInContext(data.Context), "condtmp");
            LLVM.BuildStore(data.Builder, lhsValue, resultAlloca);
            if (Op == BinaryOperator.AND) {
                LLVM.BuildCondBr(data.Builder, lhsValue, ifBlock, mergeBlock);
            } else {
                LLVM.BuildCondBr(data.Builder, lhsValue, mergeBlock, ifBlock);
            }
            LLVM.PositionBuilderAtEnd(data.Builder, ifBlock);
            LLVMValueRef rhsValue = Rhs.Codegen(data);
            LLVM.BuildStore(data.Builder, rhsValue, resultAlloca);
            LLVM.BuildBr(data.Builder, mergeBlock);
            LLVM.PositionBuilderAtEnd(data.Builder, mergeBlock);
            return LLVM.BuildLoad(data.Builder, resultAlloca, "");
        }
        var l = Lhs.Codegen(data);
        var r = Rhs.Codegen(data);
        var typeKind = LLVM.GetTypeKind(LLVM.TypeOf(l));
        switch (Op) {
            case BinaryOperator.PLUS:
                if (typeKind == LLVMTypeKind.LLVMIntegerTypeKind) {
                    var a = LLVM.BuildAdd(data.Builder, l, r, "addtmp");
                    return a;
                }
                if (typeKind == LLVMTypeKind.LLVMDoubleTypeKind) {
                    return LLVM.BuildFAdd(data.Builder, l, r, "addtmp");
                }
                throw new Exception("Error: unknown add type");
            case BinaryOperator.MINUS:
                if (typeKind == LLVMTypeKind.LLVMIntegerTypeKind) {
                    return LLVM.BuildSub(data.Builder, l, r, "subtmp");
                }
                if (typeKind == LLVMTypeKind.LLVMDoubleTypeKind) {
                    return LLVM.BuildFSub(data.Builder, l, r, "subtmp");
                }
                throw new Exception("Error: unknown subtract type");
            case BinaryOperator.TIMES:
                if (typeKind == LLVMTypeKind.LLVMIntegerTypeKind) {
                    return LLVM.BuildMul(data.Builder, l, r, "multmp");
                }
                if (typeKind == LLVMTypeKind.LLVMDoubleTypeKind) {
                    return LLVM.BuildFMul(data.Builder, l, r, "multmp");
                }
                throw new Exception("Error: unknown multiplication type");
            case BinaryOperator.DIVIDE:
                if (typeKind == LLVMTypeKind.LLVMIntegerTypeKind) {
                    return LLVM.BuildSDiv(data.Builder, l, r, "divtmp");
                }
                if (typeKind == LLVMTypeKind.LLVMDoubleTypeKind) {
                    return LLVM.BuildFDiv(data.Builder, l, r, "divtmp");
                }
                throw new Exception("Error: unknown division type");
            case BinaryOperator.LT:
                if (typeKind == LLVMTypeKind.LLVMIntegerTypeKind) {
                    return LLVM.BuildICmp(data.Builder, LLVMIntPredicate.LLVMIntSLT, l, r, "cmptmp");
                }
                if (typeKind == LLVMTypeKind.LLVMDoubleTypeKind) {
                    return LLVM.BuildFCmp(data.Builder, LLVMRealPredicate.LLVMRealOLT, l, r, "cmptmp");
                }
                throw new Exception("Error: unknown comparison type");
            case BinaryOperator.GT:
                if (typeKind == LLVMTypeKind.LLVMIntegerTypeKind) {
                    return LLVM.BuildICmp(data.Builder, LLVMIntPredicate.LLVMIntSGT, l, r, "cmptmp");
                }
                if (typeKind == LLVMTypeKind.LLVMDoubleTypeKind) {
                    return LLVM.BuildFCmp(data.Builder, LLVMRealPredicate.LLVMRealOGT, l, r, "cmptmp");
                }
                throw new Exception("Error: unknown comparison type");
            case BinaryOperator.LE:
                if (typeKind == LLVMTypeKind.LLVMIntegerTypeKind) {
                    return LLVM.BuildICmp(data.Builder, LLVMIntPredicate.LLVMIntSLE, l, r, "cmptmp");
                }
                if (typeKind == LLVMTypeKind.LLVMDoubleTypeKind) {
                    return LLVM.BuildFCmp(data.Builder, LLVMRealPredicate.LLVMRealOLT, l, r, "cmptmp");
                }
                throw new Exception("Error: unknown comparison type");
            case BinaryOperator.GE:
                if (typeKind == LLVMTypeKind.LLVMIntegerTypeKind) {
                    return LLVM.BuildICmp(data.Builder, LLVMIntPredicate.LLVMIntSGT, l, r, "cmptmp");
                }
                if (typeKind == LLVMTypeKind.LLVMDoubleTypeKind) {
                    return LLVM.BuildFCmp(data.Builder, LLVMRealPredicate.LLVMRealOGT, l, r, "cmptmp");
                }
                throw new Exception("Error: unknown comparison type");
            case BinaryOperator.EQ:
                if (typeKind == LLVMTypeKind.LLVMIntegerTypeKind) {
                    return LLVM.BuildICmp(data.Builder, LLVMIntPredicate.LLVMIntEQ, l, r, "cmptmp");
                }
                if (typeKind == LLVMTypeKind.LLVMDoubleTypeKind) {
                    return LLVM.BuildFCmp(data.Builder, LLVMRealPredicate.LLVMRealOEQ, l, r, "cmptmp");
                }
                throw new Exception("Error: unknown comparison type");
            case BinaryOperator.NE:
                if (typeKind == LLVMTypeKind.LLVMIntegerTypeKind) {
                    return LLVM.BuildICmp(data.Builder, LLVMIntPredicate.LLVMIntNE, l, r, "cmptmp");
                }
                if (typeKind == LLVMTypeKind.LLVMDoubleTypeKind) {
                    return LLVM.BuildFCmp(data.Builder, LLVMRealPredicate.LLVMRealONE, l, r, "cmptmp");
                }
                throw new Exception("Error: unknown comparison type");
            case BinaryOperator.MODULO:
                if (typeKind == LLVMTypeKind.LLVMIntegerTypeKind) {
                    return LLVM.BuildSRem(data.Builder, l, r, "modtmp");
                }
                if (typeKind == LLVMTypeKind.LLVMDoubleTypeKind) {
                    return LLVM.BuildFRem(data.Builder, l, r, "modtmp");
                }
                throw new Exception("Error: unknown modulo type");
            default:
                throw new Exception("Error: unimplemented binary expression");
        }

    }
}