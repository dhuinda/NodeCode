using System.Runtime.InteropServices;
using System.Text;

namespace CodeDesigner.Core
{
    
    using LLVMSharp;
    
    public class CodeGenerator
    {
        public static void Run(List<ASTNode> ast)
        {
            string error;
            
            LLVM.InitializeCore(LLVM.GetGlobalPassRegistry());
            LLVM.InitializeX86AsmPrinter();
            LLVM.InitializeX86AsmParser();
            LLVM.InitializeX86Disassembler();
            LLVM.InitializeX86Target();
            LLVM.InitializeX86TargetInfo();
            LLVM.InitializeX86TargetMC();

            LLVMContextRef context = LLVM.ContextCreate();
            LLVMModuleRef module = LLVM.ModuleCreateWithNameInContext("program", context);
            LLVMBuilderRef builder = LLVM.CreateBuilderInContext(context);

            Dictionary<String, LLVMValueRef> namedValues = new Dictionary<string, LLVMValueRef>();
            CodegenData data = new CodegenData(builder, context, null, namedValues, module);
            
            foreach (ASTNode node in ast)
            {
                node.codegen(data);
            }
            
            LLVM.DumpModule(module);
            
            if (LLVM.VerifyModule(module, LLVMVerifierFailureAction.LLVMPrintMessageAction, out error).Value != 0)
            {
                Console.WriteLine("Failed to validate module: " + error);
                return;
            }

            if (LLVM.WriteBitcodeToFile(module, "./output.bc") != 0)
            {
                Console.Error.WriteLine("Failed to write bitcode to file!");
                return;
            }

            string triple = Marshal.PtrToStringAnsi(LLVM.GetDefaultTargetTriple());
            Console.WriteLine("triple: " + triple);
            var target = new LLVMTargetRef();

            if (LLVM.GetTargetFromTriple(triple, out target, out error).Value != 0)
            {
                Console.Error.WriteLine("Failed to get target from triple: " + error);
                return;
            }

            var cpu = "generic";
            var cpuFeatures = "";
            LLVMTargetMachineRef tm = LLVM.CreateTargetMachine(target, triple, cpu,
                cpuFeatures, LLVMCodeGenOptLevel.LLVMCodeGenLevelNone, LLVMRelocMode.LLVMRelocDefault,
                LLVMCodeModel.LLVMCodeModelDefault);
            if (LLVM.TargetMachineEmitToFile(tm, module, Marshal.StringToHGlobalAnsi("./output.o"), LLVMCodeGenFileType.LLVMObjectFile,
                    out error).Value != 0)
            {
                Console.Error.WriteLine("Failed to emit relocatable object file: " + error);
                return;
            }
            
            LLVM.DisposeBuilder(builder);
            LLVM.DisposeModule(module);
            LLVM.ContextDispose(context);
        }
    }
}