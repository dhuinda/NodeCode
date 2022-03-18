using LLVMSharp;

namespace CodeDesigner.Core.ast;

public class ASTClassDefinition : ASTNode
{
    public ClassType ClassType;
    public List<ASTVariableDefinition> Fields;
    public List<ASTFunctionDefinition> Methods;
    public Dictionary<string, MethodAttributes> MethodAttributesMap;
    public List<List<VariableType>> GenericUsages;// = new(); // todo: auto generate using an analysis step

    public ASTClassDefinition(ClassType classType, List<ASTVariableDefinition> fields, List<ASTFunctionDefinition> methods, Dictionary<string, MethodAttributes> methodAttributesMap, List<List<VariableType>> genericUsages)
    {
        ClassType = classType;
        Fields = fields;
        Methods = methods;
        MethodAttributesMap = methodAttributesMap;
        GenericUsages = genericUsages;
    }

    public override LLVMValueRef Codegen(CodegenData data)
    {
        foreach (var genericUsage in GenericUsages)
        {
            var genericUsageClass = ClassType.ConvertGenericUsage(genericUsage);
            var genericMap = new Dictionary<string, string>();
            for (var i = 0; i < ClassType.GenericTypes.Count; i++)
            {
                genericMap.Add(ClassType.GenericTypes[i].GetGenericName(), genericUsageClass[i].GetGenericName());
            }
            data.Generics = genericMap;

            var fullName = $"{data.NamespaceName}.{ClassType.Of(ClassType.Name, genericUsageClass).GetGenericName()}";
            Console.WriteLine("codegen for class def " + fullName);
            var fieldLlvmTypes = new List<LLVMTypeRef>();
            var classFieldTypes = new List<ClassFieldType>();

            if (!data.Classes.ContainsKey(fullName))
            {
                throw new Exception("expected class " + fullName + "to have a struct already generated during analysis");
            }
            
            var classDef = data.Classes[fullName].Type;
            foreach (var field in Fields)
            {
                if (!field.Type.IsPrimitive)
                {
                    Console.WriteLine(field.Type.ClassType!.Name);
                }
                var fieldType = field.Type.GetLLVMType(data);
                Console.WriteLine(field.Name + ": " + fieldType);
                fieldLlvmTypes.Add(fieldType);
                classFieldTypes.Add(new ClassFieldType(field.Name, fieldType));
            }

            Console.WriteLine("methods");
            var methodOrder = new List<string>();
            var vtableTypeBody = new List<LLVMTypeRef>();
            var vtableValues = new List<LLVMValueRef>();
            foreach (var method in Methods)
            {
                methodOrder.Add(method.Name);
                var fullMethodName = $"{fullName}__{method.Name}";
                method.Params.Add(new ASTVariableDefinition("this", new VariableType(new ClassType(fullName))));
                var paramTypes = new List<LLVMTypeRef>();
                foreach (var param in method.Params)
                {
                    paramTypes.Add(param.Type.GetLLVMType(data));
                }

                var methodType = LLVM.FunctionType(method.ReturnType.GetLLVMType(data), paramTypes.ToArray(), false);
                var func = LLVM.AddFunction(data.Module, fullMethodName, methodType);
                if (MethodAttributesMap.ContainsKey(method.Name) && MethodAttributesMap[method.Name].IsVirtual)
                {
                    vtableTypeBody.Add(LLVM.PointerType(methodType, 0));
                    vtableValues.Add(func);
                }
            }
            var vtableType = LLVM.StructTypeInContext(data.Context, vtableTypeBody.ToArray(), false);
            fieldLlvmTypes.Insert(0, LLVM.PointerType(vtableType, 0));
            
            LLVM.StructSetBody(classDef, fieldLlvmTypes.ToArray(), false);
            data.Classes.Remove(fullName); // todo: see if this remove is needed, or if Dictionaries automatically replace old values
            data.Classes.Add(fullName, new ClassData(classDef, classFieldTypes, null, methodOrder, MethodAttributesMap));
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
            data.Classes.Add(fullName, new ClassData(classDef, classFieldTypes, vtableGlobal, methodOrder, MethodAttributesMap));

            foreach (var method in Methods)
            {
                var originalMethodName = method.Name;
                method.Name = $"{fullName}__{method.Name}";
                method.Codegen(data);
                method.Name = originalMethodName;
                method.Params.RemoveAt(method.Params.Count - 1);
            }

            data.Generics = new Dictionary<string, string>();
        }
        return new LLVMValueRef();
    }
}