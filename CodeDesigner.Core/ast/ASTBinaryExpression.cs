using LLVMSharp.Interop;

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
    
    public override unsafe LLVMValueRef codegen(CodegenData data)
    {
        if (Op is BinaryOperator.AND or BinaryOperator.OR) {
            LLVMValueRef lhsValue = Lhs.codegen(data);
            LLVMBasicBlockRef ifBB = LLVM.AppendBasicBlockInContext(data.Context, data.Func, CodeGenerator.StringToSBytes("condif"));
            LLVMBasicBlockRef mergeBB = LLVM.AppendBasicBlockInContext(data.Context, data.Func, CodeGenerator.StringToSBytes("mergeif"));
            LLVMValueRef resultAlloca = LLVM.BuildAlloca(data.Builder, LLVM.Int1TypeInContext(data.Context), CodeGenerator.StringToSBytes("condtmp"));
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
            return LLVM.BuildLoad(data.Builder, resultAlloca, CodeGenerator.StringToSBytes(""));
        }
        var l = Lhs.codegen(data);
        var r = Rhs.codegen(data);
        switch (Op) {
            case BinaryOperator.PLUS:
                if (LLVM.IsAConstantInt(l) != null) {
                    return LLVM.BuildAdd(data.Builder, l, r, CodeGenerator.StringToSBytes("addtmp"));
                }
                if (LLVM.IsAConstantFP(l) != null) {
                    return LLVM.BuildFAdd(data.Builder, l, r, CodeGenerator.StringToSBytes("addtmp"));
                }
                throw new Exception("Error: unknown add type");
            case BinaryOperator.MINUS:
                if (LLVM.IsAConstantInt(l) != null) {
                    return LLVM.BuildSub(data.Builder, l, r, CodeGenerator.StringToSBytes("subtmp"));
                }
                if (LLVM.IsAConstantFP(l) != null) {
                    return LLVM.BuildFSub(data.Builder, l, r, CodeGenerator.StringToSBytes("subtmp"));
                }
                throw new Exception("Error: unknown subtract type");
            case BinaryOperator.TIMES:
                if (LLVM.IsAConstantInt(l) != null) {
                    return LLVM.BuildMul(data.Builder, l, r, CodeGenerator.StringToSBytes("multmp"));
                }
                if (LLVM.IsAConstantFP(l) != null) {
                    return LLVM.BuildFMul(data.Builder, l, r, CodeGenerator.StringToSBytes("multmp"));
                }
                throw new Exception("Error: unknown multiplication type");
            case BinaryOperator.DIVIDE:
                if (LLVM.IsAConstantInt(l) != null) {
                    return LLVM.BuildSDiv(data.Builder, l, r, CodeGenerator.StringToSBytes("divtmp"));
                }
                if (LLVM.IsAConstantFP(l) != null) {
                    return LLVM.BuildFDiv(data.Builder, l, r, CodeGenerator.StringToSBytes("divtmp"));
                }
                throw new Exception("Error: unknown division type");
            case BinaryOperator.LT:
                if (LLVM.IsAConstantInt(l) != null) {
                    return LLVM.BuildICmp(data.Builder, LLVMIntPredicate.LLVMIntSLT, l, r, CodeGenerator.StringToSBytes("cmptmp"));
                }
                if (LLVM.IsAConstantFP(l) != null) {
                    return LLVM.BuildFCmp(data.Builder, LLVMRealPredicate.LLVMRealOLT, l, r, CodeGenerator.StringToSBytes("cmptmp"));
                }
                throw new Exception("Error: unknown comparison type");
            case BinaryOperator.GT:
                if (LLVM.IsAConstantInt(l) != null) {
                    return LLVM.BuildICmp(data.Builder, LLVMIntPredicate.LLVMIntSGT, l, r, CodeGenerator.StringToSBytes("cmptmp"));
                }
                if (LLVM.IsAConstantFP(l) != null) {
                    return LLVM.BuildFCmp(data.Builder, LLVMRealPredicate.LLVMRealOGT, l, r, CodeGenerator.StringToSBytes("cmptmp"));
                }
                throw new Exception("Error: unknown comparison type");
            case BinaryOperator.LE:
                if (LLVM.IsAConstantInt(l) != null) {
                    return LLVM.BuildICmp(data.Builder, LLVMIntPredicate.LLVMIntSLE, l, r, CodeGenerator.StringToSBytes("cmptmp"));
                }
                if (LLVM.IsAConstantFP(l) != null) {
                    return LLVM.BuildFCmp(data.Builder, LLVMRealPredicate.LLVMRealOLT, l, r, CodeGenerator.StringToSBytes("cmptmp"));
                }
                throw new Exception("Error: unknown comparison type");
            case BinaryOperator.GE:
                if (LLVM.IsAConstantInt(l) != null) {
                    return LLVM.BuildICmp(data.Builder, LLVMIntPredicate.LLVMIntSGT, l, r, CodeGenerator.StringToSBytes("cmptmp"));
                }
                if (LLVM.IsAConstantFP(l) != null) {
                    return LLVM.BuildFCmp(data.Builder, LLVMRealPredicate.LLVMRealOGT, l, r, CodeGenerator.StringToSBytes("cmptmp"));
                }
                throw new Exception("Error: unknown comparison type");
            case BinaryOperator.EQ:
                if (LLVM.IsAConstantInt(l) != null) {
                    return LLVM.BuildICmp(data.Builder, LLVMIntPredicate.LLVMIntEQ, l, r, CodeGenerator.StringToSBytes("cmptmp"));
                }
                if (LLVM.IsAConstantFP(l) != null) {
                    return LLVM.BuildFCmp(data.Builder, LLVMRealPredicate.LLVMRealOEQ, l, r, CodeGenerator.StringToSBytes("cmptmp"));
                }
                throw new Exception("Error: unknown comparison type");
            case BinaryOperator.NE:
                if (LLVM.IsAConstantInt(l) != null) {
                    return LLVM.BuildICmp(data.Builder, LLVMIntPredicate.LLVMIntNE, l, r, CodeGenerator.StringToSBytes("cmptmp"));
                }
                if (LLVM.IsAConstantFP(l) != null) {
                    return LLVM.BuildFCmp(data.Builder, LLVMRealPredicate.LLVMRealONE, l, r, CodeGenerator.StringToSBytes("cmptmp"));
                }
                throw new Exception("Error: unknown comparison type");
            case BinaryOperator.MODULO:
                if (LLVM.IsAConstantInt(l) != null) {
                    return LLVM.BuildSRem(data.Builder, l, r, CodeGenerator.StringToSBytes("modtmp"));
                }
                if (LLVM.IsAConstantFP(l) != null) {
                    return LLVM.BuildFRem(data.Builder, l, r, CodeGenerator.StringToSBytes("modtmp"));
                }
                throw new Exception("Error: unknown modulo type");
            default:
                throw new Exception("Error: unimplemented binary expression");
        }

    }
}