namespace Test.Automated
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Test.Shared;
    using Touchstone.Cli;

    /// <summary>
    /// Touchstone CLI runner for the ExpressionTree test suite. Executes every shared
    /// descriptor and returns 0 when all tests pass, non-zero when any test fails.
    /// </summary>
    internal static class Program
    {
        private static async Task<int> Main(string[] args)
        {
            string resultsPath = null;
            for (int i = 0; i < args.Length; i++)
            {
                if (string.Equals(args[i], "--results", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
                {
                    resultsPath = args[i + 1];
                    i++;
                }
            }

            return await ConsoleRunner.RunAsync(
                ExprTestSuites.All,
                sink: null,
                resultsPath: resultsPath,
                cancellationToken: CancellationToken.None);
        }
    }
}
