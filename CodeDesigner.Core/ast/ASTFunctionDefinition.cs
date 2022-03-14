using System.Runtime.InteropServices;
using LLVMSharp;

namespace CodeDesigner.Core.ast;
using Core;

public class ASTFunctionDefinition : ASTNode
{

    public string Name;
    public List<ASTVariableDefinition> Params;
    public List<ASTNode> Body;
    public VariableType ReturnType;

    public ASTFunctionDefinition(string name, List<ASTVariableDefinition> @params, List<ASTNode> body,
        VariableType returnType)
    {
        Name = name;
        Params = @params;
        Body = body;
        ReturnType = returnType;
    }

    public override LLVMValueRef codegen(CodegenData data)
    {
        Console.WriteLine("codegen for " + Name);
        LLVMValueRef func = LLVM.GetNamedFunction(data.Module, Name);
        var paramTypes = new LLVMTypeRef[Params.Count];

        for (var i = 0; i < Params.Count; i++)
        {
            LLVMTypeRef llvmType;
            if (Params[i].VariableType.IsPrimitive)
            {
                llvmType = VariableType.GetLLVMType(Params[i].VariableType.PrimitiveType.GetValueOrDefault(PrimitiveVariableType.VOID), data.Context);
            }
            else
            {
                throw new Exception("classes aren't implemented yet");
            }

            paramTypes[i] = llvmType;
        }

        LLVMTypeRef llvmReturnType;
        if (ReturnType.IsPrimitive)
        {
            llvmReturnType =
                VariableType.GetLLVMType(ReturnType.PrimitiveType.GetValueOrDefault(PrimitiveVariableType.VOID),
                    data.Context);
        }
        else
        {
            throw new Exception("classes aren't implemented yet");
        }

        var functionType = LLVM.FunctionType(llvmReturnType, paramTypes, false);
        if (func.Pointer.ToInt64() == 0)
        {
            func = LLVM.AddFunction(data.Module, Name, functionType);
        }

        var funcEntryBlock = LLVM.AppendBasicBlockInContext(data.Context, func, "entry");
        LLVM.PositionBuilderAtEnd(data.Builder, funcEntryBlock);
        data.NamedValues.Clear();
        data.Func = func;
        
        for (var i = 0; i < Params.Count; i++)
        {
            var arg = LLVM.GetParam(func, (uint) i);
            var alloca = LLVM.BuildAlloca(data.Builder, paramTypes[i], Params[i].Name);
            LLVM.BuildStore(data.Builder, arg, alloca);
            data.NamedValues.Add(Params[i].Name, alloca);
            // todo: check if object
        }
        
        foreach (var node in Body)
        {
            node.codegen(data);
        }
        
        if (ReturnType.IsPrimitive && ReturnType.PrimitiveType == PrimitiveVariableType.VOID)
        {
            LLVM.BuildRetVoid(data.Builder);
        }
        data.NamedValues.Clear();
        return func;
    }
}