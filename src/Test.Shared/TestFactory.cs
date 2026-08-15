namespace Test.Shared
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Touchstone.Core;

    /// <summary>
    /// Convenience factory for building Touchstone test-case descriptors from synchronous test bodies.
    /// </summary>
    public static class TestFactory
    {
        /// <summary>
        /// Build a test case descriptor that runs a synchronous test body.
        /// </summary>
        /// <param name="suiteId">Identifier of the parent suite.</param>
        /// <param name="caseId">Identifier of this case within the suite.</param>
        /// <param name="displayName">Human-readable case name.</param>
        /// <param name="body">Synchronous test body; throw to fail.</param>
        /// <returns>Test case descriptor.</returns>
        public static TestCaseDescriptor Case(string suiteId, string caseId, string displayName, Action body)
        {
            if (body == null) throw new ArgumentNullException(nameof(body));

            Func<CancellationToken, Task> exec = token =>
            {
                body();
                return Task.CompletedTask;
            };

            return new TestCaseDescriptor(suiteId, caseId, displayName, exec);
        }
    }
}
