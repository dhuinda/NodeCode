﻿using System.Runtime.InteropServices;
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

            var context = LLVM.ContextCreate();
            var module = LLVM.ModuleCreateWithNameInContext("program", context);
            var builder = LLVM.CreateBuilderInContext(context);

            var namedValues = new Dictionary<string, LLVMValueRef>();
            var data = new CodegenData(builder, context, null, namedValues, module, "default");
            
            foreach (var node in ast)
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