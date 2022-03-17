using System.Runtime.InteropServices;
using System.Text;
using CodeDesigner.Core.ast;

namespace CodeDesigner.Core
{
    
    using LLVMSharp;
    
    public class CodeGenerator
    {
        public static void Run(List<ASTNode> ast)
        {
            var prototypeAnalyzer = new PrototypeAnalyzer(ast);
            prototypeAnalyzer.Run();

            LLVM.InitializeCore(LLVM.GetGlobalPassRegistry());
            LLVM.InitializeX86AsmPrinter();
            LLVM.InitializeX86AsmParser();
            LLVM.InitializeX86Disassembler();
            LLVM.InitializeX86Target();
            LLVM.InitializeX86TargetInfo();
            LLVM.InitializeX86TargetMC();

            var context = LLVM.ContextCreate();
            var module = LLVM.ModuleCreateWithNameInContext("program", context);
            var builder = LLVM.CreateBuilderInContext(context);

            var data = new CodegenData(builder, context, null, module, "default");
            
            ast.Insert(0, new ASTFunctionDefinition("main", new List<ASTVariableDefinition>(), new List<ASTNode>(), new VariableType(PrimitiveVariableType.VOID)));
            foreach (var node in ast)
            {
                node.Codegen(data);
            }
            
            LLVM.DumpModule(module);
            
            if (LLVM.VerifyModule(module, LLVMVerifierFailureAction.LLVMPrintMessageAction, out var error).Value != 0)
            {
                Console.WriteLine("Failed to validate module: " + error);
                return;
            }

            if (LLVM.WriteBitcodeToFile(module, "./output.bc") != 0)
            {
                Console.Error.WriteLine("Failed to write bitcode to file!");
                return;
            }

            var triple = Marshal.PtrToStringAnsi(LLVM.GetDefaultTargetTriple()) ?? throw new InvalidOperationException();
            Console.WriteLine("triple: " + triple);

            if (LLVM.GetTargetFromTriple(triple, out var target, out error).Value != 0)
            {
                Console.Error.WriteLine("Failed to get target from triple: " + error);
                return;
            }

            var cpu = "generic";
            var cpuFeatures = "";
            var tm = LLVM.CreateTargetMachine(target, triple, cpu,
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