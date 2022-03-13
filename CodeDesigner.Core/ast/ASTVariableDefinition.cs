using LLVMSharp.Interop;

namespace CodeDesigner.Core.ast;
using Core;

public class ASTVariableDefinition : ASTNode
{

    public string Name;

    public VariableType VariableType;

    public ASTVariableDefinition(string name, VariableType variableType)
    {
        Name = name;
        VariableType = variableType;
    }

    public override unsafe LLVMValueRef codegen(CodegenData data)
    {
        LLVMTypeRef llvmType;
        if (VariableType.IsPrimitive)
        {
            // the OrDefault part should never happen
            llvmType = VariableType.GetLLVMType(VariableType.PrimitiveType.GetValueOrDefault(PrimitiveVariableType.VOID), data.Context);
        }
        else
        {
            throw new Exception("classes are not implemented yet");
        }

        LLVMValueRef alloca = LLVM.BuildAlloca(data.Builder, llvmType, CodeGenerator.StringToSBytes(Name));
        data.NamedValues.Add(Name, alloca);
        return alloca;
    }
}