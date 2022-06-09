using CodeDesigner.UI.Windows;

namespace CodeRunner.UI
{
    public static class Program
    {
        public static Dashboard dash;
        public static ProjectManager pm;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(pm = new ProjectManager());
        }
    }
}