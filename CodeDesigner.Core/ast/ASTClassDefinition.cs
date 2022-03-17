using LLVMSharp;

namespace CodeDesigner.Core.ast;

public class ASTClassDefinition : ASTNode
{
    public string Name;
    public List<ASTVariableDefinition> Fields;
    public List<ASTFunctionDefinition> Methods;
    public Dictionary<string, MethodAttributes> MethodAttributesMap;

    public ASTClassDefinition(string name, List<ASTVariableDefinition> fields, List<ASTFunctionDefinition> methods, Dictionary<string, MethodAttributes> methodAttributesMap)
    {
        Name = name;
        Fields = fields;
        Methods = methods;
        MethodAttributesMap = methodAttributesMap;
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

        var methodOrder = new List<string>();
        var vtableTypeBody = new List<LLVMTypeRef>();
        var vtableValues = new List<LLVMValueRef>();
        foreach (var method in Methods)
        {
            methodOrder.Add(method.Name);
            var shortName = method.Name;
            method.Name = $"{fullName}__{method.Name}";
            method.Params.Add(new ASTVariableDefinition("this", new VariableType(fullName)));
            var paramTypes = new List<LLVMTypeRef>();
            foreach (var param in method.Params)
            {
                paramTypes.Add(param.Type.GetLLVMType(data));
            }

            var methodType = LLVM.FunctionType(method.ReturnType.GetLLVMType(data), paramTypes.ToArray(), false);
            var func = LLVM.AddFunction(data.Module, method.Name, methodType);
            if (MethodAttributesMap.ContainsKey(shortName) && MethodAttributesMap[shortName].IsVirtual)
            {
                vtableTypeBody.Add(LLVM.PointerType(methodType, 0));
                vtableValues.Add(func);
            }
        }
        var vtableType = LLVM.StructTypeInContext(data.Context, vtableTypeBody.ToArray(), false);
        fieldLlvmTypes.Insert(0, LLVM.PointerType(vtableType, 0));
        
        LLVM.StructSetBody(classType, fieldLlvmTypes.ToArray(), false);
        data.Classes.Remove(fullName); // todo: see if this remove is needed, or if Dictionaries automatically replace old values
        data.Classes.Add(fullName, new ClassData(classType, classFieldTypes, null, methodOrder, MethodAttributesMap));
        LLVM.StructSetBody(vtableType, vtableTypeBody.ToArray(), false);
        
        // todo: only add vtable to classes with 1+ virtual methods
        var vtableGlobal = LLVM.AddGlobal(data.Module, vtableType, $"{fullName}___vtable");
        LLVM.SetLinkage(vtableGlobal, LLVMLinkage.LLVMInternalLinkage);
        LLVM.SetInitializer(vtableGlobal, LLVM.ConstStructInContext(data.Context, vtableValues.ToArray(), false));

        // Initialize vtable
        LLVM.PositionBuilderAtEnd(data.Builder, LLVM.GetLastBasicBlock(LLVM.GetNamedFunction(data.Module, "__main")));
        for (var i = 0; i < vtableValues.Count; i++)
        {
            var gep = LLVM.BuildStructGEP(data.Builder, vtableGlobal, (uint) i, "");
            LLVM.BuildStore(data.Builder, vtableValues[i], gep);
        }

        data.Classes.Remove(fullName);
        data.Classes.Add(fullName, new ClassData(classType, classFieldTypes, vtableGlobal, methodOrder, MethodAttributesMap));

        foreach (var method in Methods)
        {
            method.Codegen(data);
        }
        
        return new LLVMValueRef();
    }
}