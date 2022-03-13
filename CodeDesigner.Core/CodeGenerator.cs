using System.Text;

//todo: next is ASTVariableExpression
namespace CodeDesigner.Core
{
    
    using LLVMSharp.Interop;
    
    public class CodeGenerator
    {
        public static unsafe sbyte* StringToSBytes(string s)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(s);
            sbyte* sp;
            fixed (byte* p = bytes)
            {
                sp = (sbyte*) p;
            }

            return sp;
        }
        public static unsafe void Run(List<ASTNode> ast)
        {
            sbyte* error;
            
            LLVM.InitializeCore(LLVM.GetGlobalPassRegistry());
            LLVM.InitializeNativeAsmPrinter();
            LLVM.InitializeNativeAsmParser();
            LLVM.InitializeNativeDisassembler();
            LLVM.InitializeNativeTarget();

            LLVMContextRef context = LLVM.ContextCreate();
            LLVMModuleRef module = LLVM.ModuleCreateWithNameInContext(StringToSBytes("program"), context);
            LLVMBuilderRef builder = LLVM.CreateBuilderInContext(context);

            Dictionary<String, LLVMValueRef> namedValues = new Dictionary<string, LLVMValueRef>();
            CodegenData data = new CodegenData(builder, context, null, namedValues, module);
            
            foreach (ASTNode node in ast)
            {
                node.codegen(data);
            }
            
            LLVM.DumpModule(module);
            
            if (LLVM.VerifyModule(module, LLVMVerifierFailureAction.LLVMPrintMessageAction, &error) != 0)
            {
                Console.WriteLine("Failed to validate module: " + error->ToString());
                return;
            }

            if (LLVM.WriteBitcodeToFile(module, StringToSBytes("./output.bc")) != 0)
            {
                Console.Error.WriteLine("Failed to write bitcode to file!");
                return;
            }

            sbyte* triple = LLVM.GetDefaultTargetTriple();
            var target = new LLVMTarget();
            var targetPtr = &target;

            if (LLVM.GetTargetFromTriple(triple, &targetPtr, &error) != 0)
            {
                Console.Error.WriteLine("Failed to get target from triple: " + error->ToString());
                return;
            }

            var cpu = "generic";
            var cpuFeatures = "";
            LLVMTargetMachineRef tm = LLVM.CreateTargetMachine(targetPtr, triple, StringToSBytes(cpu),
                StringToSBytes(cpuFeatures), LLVMCodeGenOptLevel.LLVMCodeGenLevelNone, LLVMRelocMode.LLVMRelocDefault,
                LLVMCodeModel.LLVMCodeModelDefault);
            if (LLVM.TargetMachineEmitToFile(tm, module, StringToSBytes("./output.o"), LLVMCodeGenFileType.LLVMObjectFile,
                    &error) != 0)
            {
                Console.Error.WriteLine("Failed to emit relocatable object file: " + error->ToString());
                return;
            }
            
            LLVM.DisposeBuilder(builder);
            LLVM.DisposeModule(module);
            LLVM.ContextDispose(context);
        }
    }
}