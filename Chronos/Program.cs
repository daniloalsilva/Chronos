using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chronos
{
    public class Program
    {

        public static int Main(string[] args)
        {
            //args = new string[] { "30", "5", @"C:\Users\danilo.silva\Documents\Visual Studio 2010\Projects\Chronos\Chronos\bin\Debug\test.ps1" };
            //args = new string[] { "30", "5", @"C:\Program Files (x86)\check_mk\Chronos\CheckWindowsBackup.ps1" };
            //args = new string[] { "clearcache" };
            //args = new string[] { "30", "10", @"C:\nagios\IIsMonitor\IIsMonitor.exe" };
            //args = new string[] { "30", "10", @"C:\Windows\System32\cmd.exe", "/c", "dir"};

            ExecutionResult exitObject = new ExecutionResult(3, "Execution result is Empty");

            if (Tools.ValidateParams(args))
            {
                exitObject = ExecutionManager.ExecutePlugin(args);
                Console.WriteLine(exitObject.ResultContent);
            }

            return exitObject.ExitCode;
        }
    }
}
