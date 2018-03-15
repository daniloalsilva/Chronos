namespace Chronos
{
    class ExecutionResult {
        public ExecutionResult(int exitCode, string resultContent)
        {
            this.ExitCode = exitCode;
            this.ResultContent = resultContent;
        }

        public int ExitCode { get; private set; }
        public string ResultContent { get; private set; }
    }
}