namespace Test.Shared
{
    using System.Collections.Generic;
    using Test.Shared.Suites;
    using Touchstone.Core;

    /// <summary>
    /// Central source of truth for the ExpressionTree test suite. Every runner
    /// (Test.Automated CLI, Test.Xunit, Test.Nunit) consumes <see cref="All"/> so the
    /// same descriptors execute identically across hosts.
    /// </summary>
    public static class ExprTestSuites
    {
        /// <summary>
        /// The complete, ordered set of test suites covering the ExpressionTree library.
        /// </summary>
        public static IReadOnlyList<TestSuiteDescriptor> All
        {
            get
            {
                return new List<TestSuiteDescriptor>
                {
                    ConstructorSuite.Build(),
                    BetweenSuite.Build(),
                    PrependSuite.Build(),
                    PrependClauseSuite.Build(),
                    ListConversionSuite.Build(),
                    ToStringSuite.Build(),
                    CopySuite.Build(),
                    PropertiesSuite.Build(),
                    OperatorEnumSuite.Build()
                };
            }
        }
    }
}
