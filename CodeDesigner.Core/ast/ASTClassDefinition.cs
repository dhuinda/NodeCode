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
        // todo: figure out a way to have a ClassType<Integer> have a field that is a ClassType<Double> if the former is defined first in GenericUsages
        foreach (var genericUsage in GenericUsages)
        {
            var genericUsageClass = new List<ClassType>();
            foreach (var vt in genericUsage)
            {
                if (vt.IsPrimitive)
                {
                    if (vt.PrimitiveType == null)
                    {
                        throw new Exception("expected primitive to contain primitive type");
                    }

                    if (vt.PrimitiveType == PrimitiveVariableType.INTEGER)
                    {
                        genericUsageClass.Add(new ClassType("Integer"));
                    } else if (vt.PrimitiveType == PrimitiveVariableType.DOUBLE)
                    {
                        genericUsageClass.Add(new ClassType("Double"));
                    } else if (vt.PrimitiveType == PrimitiveVariableType.STRING)
                    {
                        genericUsageClass.Add(new ClassType("String"));
                    } else if (vt.PrimitiveType == PrimitiveVariableType.BOOLEAN)
                    {
                        genericUsageClass.Add(new ClassType("Boolean"));
                    } else
                    {
                        throw new InvalidCodeException("invalid primitive as generic usage");
                    }
                }
                else if (vt.ClassType != null)
                {
                    genericUsageClass.Add(vt.ClassType);
                }
                else
                {
                    throw new Exception("expected object type to have a ClassType");
                }
            }

            var genericMap = new Dictionary<string, string>();
            for (var i = 0; i < ClassType.GenericTypes.Count; i++)
            {
                genericMap.Add(ClassType.GenericTypes[i].GetGenericName(), genericUsageClass[i].GetGenericName());
            }

            data.Generics = genericMap;
            var fullName = $"{data.NamespaceName}.{new ClassType(ClassType.Name, genericUsageClass).GetGenericName()}";
            Console.WriteLine("codegen for class def " + fullName);
            var fieldLlvmTypes = new List<LLVMTypeRef>();
            var classFieldTypes = new List<ClassFieldType>();
            var classType = LLVM.StructCreateNamed(data.Context, fullName);
            Console.WriteLine("add");
            data.Classes.Add(fullName, new ClassData(classType, null));
            foreach (var field in Fields)
            {
                var fieldType = field.Type.GetLLVMType(data);
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
                var shortName = method.Name;
                method.Name = $"{fullName}__{method.Name}";
                method.Params.Add(new ASTVariableDefinition("this", new VariableType(new ClassType(fullName))));
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

            data.Generics = new Dictionary<string, string>();
        }
        return new LLVMValueRef();
    }
}