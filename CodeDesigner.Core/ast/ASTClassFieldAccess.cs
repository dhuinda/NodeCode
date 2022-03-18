using LLVMSharp;

namespace CodeDesigner.Core.ast;

public class ASTClassFieldAccess : ASTNode
{
    public ASTNode Object;
    public string FieldName;

    public ASTClassFieldAccess(ASTNode obj, string fieldName)
    {
        Object = obj;
        FieldName = fieldName;
    }

    public override LLVMValueRef Codegen(CodegenData data)
    {
        var obj = Object.Codegen(data);
        if (LLVM.GetTypeKind(LLVM.TypeOf(obj)) != LLVMTypeKind.LLVMPointerTypeKind ||
            LLVM.GetTypeKind(LLVM.GetElementType(LLVM.TypeOf(obj))) != LLVMTypeKind.LLVMStructTypeKind)
        {
            throw new InvalidCodeException("unexpected field access of non-object variable");
        }

        var className = VariableType.GetClassNameOfObject(obj);
        if (!data.Classes.ContainsKey(className))
        {
            throw new Exception("unknown class " + className);
        }

        var fields = data.Classes[className].Fields;
        var fieldNumber = -1;
        for (var i = 0; i < fields.Count; i++)
        {
            if (fields[i].Name == FieldName)
            {
                fieldNumber = i;
                break;
            }
        }

        if (fieldNumber == -1)
        {
            throw new InvalidCodeException("unknown field of class " + className + ": " + FieldName);
        }

        var gep = LLVM.BuildStructGEP(data.Builder, obj, (uint) fieldNumber + 1, "gep");
        return LLVM.BuildLoad(data.Builder, gep, "");

    }
}