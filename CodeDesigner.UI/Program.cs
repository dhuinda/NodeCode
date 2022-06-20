using System.Runtime.InteropServices;
using CodeDesigner.UI.Node.Blocks;
using CodeDesigner.UI.Node.Blocks.Nodes;
using CodeDesigner.UI.Node.Blocks.Types;
using CodeDesigner.UI.Windows;

namespace CodeRunner.UI
{
    public static class Program
    {
        [DllImport( "kernel32.dll" )]
        static extern bool AttachConsole( int dwProcessId );
        private const int ATTACH_PARENT_PROCESS = -1;

        public static Dashboard dash;
        public static ProjectManager pm;
        
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AttachConsole(ATTACH_PARENT_PROCESS);
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(pm = new ProjectManager());

            var blocks = new List<BlockBase>();
            var mainDef = new FunctionDefinition("main", Parameter.ParameterType.Void);
            var binOp = new BinaryExpression(BinOp.Add);
            binOp.Left = "1";
            binOp.Parameters[1].ReferenceValue = new NumberExpression("2");
            var printfCall = new FunctionInvocation("extern.printf");
            printfCall.Parameters.Add(new Parameter
            {
                Type = Parameter.ParameterType.String,
                RawValue = "\"1 + 2: %d\n\""
            });
            printfCall.Parameters.Add(new Parameter
            {
                ReferenceValue = binOp
            });
            mainDef.NextBlock = printfCall;
            printfCall.NextBlock = new ReturnExpression();
            blocks.Add(mainDef);
            NodeConverter.CompileNodes(blocks);
        }
    }
}