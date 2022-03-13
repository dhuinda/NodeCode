using System.Runtime.InteropServices;
using LLVMSharp.Interop;

namespace CodeDesigner.Core.ast;
using Core;

public class ASTFunctionDefinition : ASTNode
{

    private string Name;
    private List<ASTVariableDefinition> Params;
    private List<ASTNode> Body;
    private VariableType ReturnType;

    public ASTFunctionDefinition(string name, List<ASTVariableDefinition> @params, List<ASTNode> body,
        VariableType returnType)
    {
        Name = name;
        Params = @params;
        Body = body;
        ReturnType = returnType;
    }

    public override unsafe LLVMValueRef codegen(CodegenData data)
    {
        Console.WriteLine("codegen for " + Name);
        LLVMValueRef func = LLVM.GetNamedFunction(data.Module, CodeGenerator.StringToSBytes(Name));
        var paramTypes = new LLVMTypeRef[Params.Count];

        if (func == null)
        {
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

            Console.WriteLine("Creating function type");
            var functionType = LLVMTypeRef.CreateFunction(llvmReturnType, paramTypes, false);
            func = LLVM.AddFunction(data.Module, CodeGenerator.StringToSBytes(Name), functionType);
        }

        LLVMBasicBlockRef funcEntryBlock =
            LLVM.AppendBasicBlockInContext(data.Context, func, CodeGenerator.StringToSBytes("entry"));
        LLVM.PositionBuilderAtEnd(data.Builder, funcEntryBlock);
        data.NamedValues.Clear();
        // todo: init args
        data.Func = func;
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