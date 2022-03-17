using LLVMSharp;

namespace CodeDesigner.Core.ast;

public class ASTClassDefinition : ASTNode
{
    public string Name;
    public List<ASTVariableDefinition> Fields;
    public List<ASTFunctionDefinition> Methods;

    public ASTClassDefinition(string name, List<ASTVariableDefinition> fields, List<ASTFunctionDefinition> methods)
    {
        Name = name;
        Fields = fields;
        Methods = methods;
    }

    public override LLVMValueRef Codegen(CodegenData data)
    {
        var fullName = $"{data.NamespaceName}.{Name}";
        Console.WriteLine("codegen for class def " + fullName);
        var fieldLlvmTypes = new List<LLVMTypeRef>();
        var classFieldTypes = new List<ClassFieldType>();
        var classType = LLVM.StructCreateNamed(data.Context, fullName);
        data.Classes.Add(fullName, new ClassData(classType, null));
        foreach (var field in Fields)
        {
            var fieldType = field.Type.GetLLVMType(data);
            fieldLlvmTypes.Add(fieldType);
            classFieldTypes.Add(new ClassFieldType(field.Name, fieldType));
        }

        LLVM.StructSetBody(classType, fieldLlvmTypes.ToArray(), false);
        data.Classes.Remove(fullName); // todo: see if this remove is needed, or if Dictionaries automatically replace old values
        data.Classes.Add(fullName, new ClassData(classType, classFieldTypes, null));
        
        var vtableBody = new List<LLVMTypeRef>();
        var methodOrder = new List<string>();
        foreach (var method in Methods)
        {
            methodOrder.Add(method.Name);
            method.Name = $"{fullName}__{method.Name}";
            method.Params.Add(new ASTVariableDefinition("this", new VariableType(fullName)));
            var paramTypes = new List<LLVMTypeRef>();
            foreach (var param in method.Params)
            {
                paramTypes.Add(param.Type.GetLLVMType(data));
            }

            var methodType = LLVM.FunctionType(method.ReturnType.GetLLVMType(data), paramTypes.ToArray(), false);
            var func = LLVM.AddFunction(data.Module, method.Name, methodType);
            method.Codegen(data);
        }

        return new LLVMValueRef();
        // var vtable = LLVM.StructTypeInContext(data.Context, )
    }
}