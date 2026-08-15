namespace Test.Nunit
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using global::NUnit.Framework;
    using Test.Shared;
    using Touchstone.Core;
    using Touchstone.NunitAdapter;

    /// <summary>
    /// TestCaseSource-driven NUnit host: every shared Touchstone case becomes an individual
    /// NUnit test case, labelled by its display name.
    /// </summary>
    [TestFixture]
    public sealed class ExpressionTreeTestCaseSourceTests
    {
        /// <summary>
        /// All shared test cases as NUnit test-case-source data.
        /// </summary>
        /// <returns>Enumerable of shared test cases.</returns>
        public static IEnumerable TestCases()
        {
            return new TouchstoneTestCaseSource(ExprTestSuites.All);
        }

        /// <summary>
        /// Execute a single shared test case.
        /// </summary>
        /// <param name="testCase">The Touchstone test case to run.</param>
        [Test]
        [TestCaseSource(nameof(TestCases))]
        public async Task Run(TestCaseDescriptor testCase)
        {
            await testCase.ExecuteAsync(CancellationToken.None);
        }
    }

    /// <summary>
    /// Single-test NUnit host: executes every shared suite in one aggregate test via the
    /// Touchstone base class.
    /// </summary>
    [TestFixture]
    public sealed class ExpressionTreeNunitAggregateTests : TouchstoneNunitBase
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
        [Test]
        public async Task RunAll()
        {
            await RunAllAsync();
        }
    }
}
