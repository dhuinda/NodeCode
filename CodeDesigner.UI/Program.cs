using System.Runtime.InteropServices;
using CodeDesigner.UI.Node.Blocks;
using CodeDesigner.UI.Node.Blocks.Nodes;
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
            var printfCall = new FunctionInvocation("extern.printf");
            printfCall.Parameters.Add(new Parameter()
            {
                Type = Parameter.ParameterType.String,
                RawValue = "Hello, world!\n"
            });
            mainDef.NextBlock = printfCall;
            printfCall.NextBlock = new ReturnExpression();
            blocks.Add(mainDef);
            NodeConverter.CompileNodes(blocks);
        }
    }
}