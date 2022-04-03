using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public namespace CodeDesigner.UI.Designer.Console
{
    public static class ConsoleHelper {
        public static void StartProcess(string path)
        {
            Process process = new ();
            ProcessStartInfo psi = new ();
            psi.FileName = "cmd.exe";
            psi.Arguments = "g++ linker.cpp CodeDesigner.UI\\bin\\Debug\\net6.0\\output.o -static";
            process.StartInfo = psi;
            process.Start();
        }
    }
}
