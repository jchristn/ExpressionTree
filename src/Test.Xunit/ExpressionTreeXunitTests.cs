namespace Test.Xunit
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using global::Xunit;
    using Test.Shared;
    using Touchstone.Core;
    using Touchstone.XunitAdapter;

    /// <summary>
    /// Theory-driven xUnit host: every shared Touchstone case is projected into a theory row
    /// and executed individually, labelled by its display name.
    /// </summary>
    public sealed class ExpressionTreeTheoryTests
    {
        /// <summary>
        /// All shared test cases, one theory row each.
        /// </summary>
        public static TouchstoneTheoryData TestCases
        {
            get { return new TouchstoneTheoryData(ExprTestSuites.All); }
        }

        /// <summary>
        /// Execute a single shared test case.
        /// </summary>
        /// <param name="testCase">The Touchstone test case to run.</param>
        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task Run(TestCaseDescriptor testCase)
        {
            await testCase.ExecuteAsync(CancellationToken.None);
        }
    }

    /// <summary>
    /// Fact-style xUnit host: executes every shared suite in a single aggregate fact via the
    /// Touchstone base class.
    /// </summary>
    public sealed class ExpressionTreeFactTests : TouchstoneFactBase
    {
        /// <summary>
        /// The shared suites under test.
        /// </summary>
        protected override IReadOnlyList<TestSuiteDescriptor> Suites
        {
            get { return ExprTestSuites.All; }
        }

        /// <summary>
        /// Run all shared suites and fail if any case fails.
        /// </summary>
        [Fact]
        public async Task RunAll()
        {
            await RunAllAsync();
        }
    }
}
