namespace Test.Shared.Suites
{
    using System;
    using System.Collections.Generic;
    using ExpressionTree;
    using Touchstone.Core;
    using static Test.Shared.TestFactory;

    /// <summary>
    /// Coverage for the static <see cref="Expr.PrependAndClause(Expr, Expr)"/> and
    /// <see cref="Expr.PrependOrClause(Expr, Expr)"/> helpers.
    /// </summary>
    public static class PrependClauseSuite
    {
        private const string Id = "prepend_clause";

        /// <summary>
        /// Build the static prepend-clause test suite.
        /// </summary>
        /// <returns>Suite descriptor.</returns>
        public static TestSuiteDescriptor Build()
        {
            List<TestCaseDescriptor> cases = new List<TestCaseDescriptor>();

            // --- Positive ---
            cases.Add(Case(Id, "and_clause_builds", "PrependAndClause builds an And expression with the given operands", () =>
            {
                Expr a = new Expr("a", OperatorEnum.Equals, 1);
                Expr b = new Expr("b", OperatorEnum.Equals, 2);
                Expr result = Expr.PrependAndClause(a, b);
                Check.Same(a, result.Left);
                Check.Equal(OperatorEnum.And, result.Operator);
                Check.Same(b, result.Right);
            }));

            cases.Add(Case(Id, "or_clause_builds", "PrependOrClause builds an Or expression with the given operands", () =>
            {
                Expr a = new Expr("a", OperatorEnum.Equals, 1);
                Expr b = new Expr("b", OperatorEnum.Equals, 2);
                Expr result = Expr.PrependOrClause(a, b);
                Check.Same(a, result.Left);
                Check.Equal(OperatorEnum.Or, result.Operator);
                Check.Same(b, result.Right);
            }));

            // --- Negative ---
            cases.Add(Case(Id, "and_null_prepend_throws", "PrependAndClause throws ArgumentNullException when prepend is null", () =>
            {
                Expr b = new Expr("b", OperatorEnum.Equals, 2);
                Check.Throws<ArgumentNullException>(() => Expr.PrependAndClause(null, b));
            }));

            cases.Add(Case(Id, "and_null_original_throws", "PrependAndClause throws ArgumentNullException when original is null", () =>
            {
                Expr a = new Expr("a", OperatorEnum.Equals, 1);
                Check.Throws<ArgumentNullException>(() => Expr.PrependAndClause(a, null));
            }));

            cases.Add(Case(Id, "or_null_prepend_throws", "PrependOrClause throws ArgumentNullException when prepend is null", () =>
            {
                Expr b = new Expr("b", OperatorEnum.Equals, 2);
                Check.Throws<ArgumentNullException>(() => Expr.PrependOrClause(null, b));
            }));

            cases.Add(Case(Id, "or_null_original_throws", "PrependOrClause throws ArgumentNullException when original is null", () =>
            {
                Expr a = new Expr("a", OperatorEnum.Equals, 1);
                Check.Throws<ArgumentNullException>(() => Expr.PrependOrClause(a, null));
            }));

            return new TestSuiteDescriptor(Id, "Expr prepend clause (static)", cases);
        }
    }
}
