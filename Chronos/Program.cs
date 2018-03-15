using System;

namespace Chronos
{
    public class Program
    {

        public static int Main(string[] args)
        {
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
