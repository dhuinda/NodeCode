using LLVMSharp;

namespace CodeDesigner.Core;

public class CodegenData
{
    public LLVMBuilderRef Builder;
    public LLVMContextRef Context;
    public LLVMValueRef? Func;
    public Dictionary<string, LLVMValueRef> NamedValues;
    public LLVMModuleRef Module;
    public string NamespaceName;

    public CodegenData(LLVMBuilderRef builder, LLVMContextRef context, LLVMValueRef? func,
        Dictionary<string, LLVMValueRef> namedValues, LLVMModuleRef module, string namespaceName)
    {
        Builder = builder;
        Context = context;
        Func = func;
        NamedValues = namedValues;
        Module = module;
        NamespaceName = namespaceName;
    }
    
}