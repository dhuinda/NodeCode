using LLVMSharp;

namespace CodeDesigner.Core;

public class ClassData
{
    public LLVMTypeRef Type;
    public List<ClassFieldType> Fields;
    public List<string> MethodOrder;
    public Dictionary<string, LLVMValueRef> Methods;
    public LLVMValueRef? VtableGlobal;

    public ClassData(LLVMTypeRef type, LLVMValueRef? vtableGlobal)
    {
        Type = type;
        Fields = new List<ClassFieldType>();
        MethodOrder = new List<string>();
        Methods = new Dictionary<string, LLVMValueRef>();
        VtableGlobal = vtableGlobal;
    }
    
    public ClassData(LLVMTypeRef type, List<ClassFieldType> fields, LLVMValueRef? vtableGlobal)
    {
        Type = type;
        Fields = fields;
        MethodOrder = new List<string>();
        Methods = new Dictionary<string, LLVMValueRef>();
        VtableGlobal = vtableGlobal;
    }
    
    public ClassData(LLVMTypeRef type, List<ClassFieldType> fields, List<string> methodOrder, Dictionary<string, LLVMValueRef> methods, LLVMValueRef? vtableGlobal)
    {
        Type = type;
        Fields = fields;
        MethodOrder = methodOrder;
        Methods = methods;
        VtableGlobal = vtableGlobal;
    }
}