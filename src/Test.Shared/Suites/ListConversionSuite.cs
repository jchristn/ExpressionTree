namespace Test.Shared.Suites
{
    using System;
    using System.Collections.Generic;
    using ExpressionTree;
    using Touchstone.Core;
    using static Test.Shared.TestFactory;

    /// <summary>
    /// Coverage for <see cref="Expr.ListToNestedAndExpression(List{Expr})"/> and
    /// <see cref="Expr.ListToNestedOrExpression(List{Expr})"/>.
    /// </summary>
    public static class ListConversionSuite
    {
        private const string Id = "list_conversion";

        /// <summary>
        /// Build the list-conversion test suite.
        /// </summary>
        /// <returns>Suite descriptor.</returns>
        public static TestSuiteDescriptor Build()
        {
            List<TestCaseDescriptor> cases = new List<TestCaseDescriptor>();

            // --- And positive ---
            cases.Add(Case(Id, "and_single_returns_same", "ListToNestedAndExpression returns the same instance for a single item", () =>
            {
                Expr only = new Expr("x", OperatorEnum.Equals, 1);
                Expr result = Expr.ListToNestedAndExpression(new List<Expr> { only });
                Check.Same(only, result);
            }));

            cases.Add(Case(Id, "and_two_items", "ListToNestedAndExpression nests two items with And", () =>
            {
                List<Expr> list = new List<Expr>
                {
                    new Expr("x", OperatorEnum.Equals, 1),
                    new Expr("y", OperatorEnum.Equals, 2)
                };
                Expr result = Expr.ListToNestedAndExpression(list);
                Check.Equal("((x Equals 1) And (y Equals 2))", result.ToString());
            }));

            cases.Add(Case(Id, "and_multiple_items", "ListToNestedAndExpression nests multiple items right-recursively with And", () =>
            {
                List<Expr> list = new List<Expr>
                {
                    new Expr("x", OperatorEnum.Equals, 1),
                    new Expr("y", OperatorEnum.Equals, 2),
                    new Expr("z", OperatorEnum.Equals, 3)
                };
                Expr result = Expr.ListToNestedAndExpression(list);
                Check.Equal("((x Equals 1) And ((y Equals 2) And (z Equals 3)))", result.ToString());
            }));

            // --- Or positive ---
            cases.Add(Case(Id, "or_single_returns_same", "ListToNestedOrExpression returns the same instance for a single item", () =>
            {
                Expr only = new Expr("x", OperatorEnum.Equals, 1);
                Expr result = Expr.ListToNestedOrExpression(new List<Expr> { only });
                Check.Same(only, result);
            }));

            cases.Add(Case(Id, "or_two_items", "ListToNestedOrExpression nests two items with Or", () =>
            {
                List<Expr> list = new List<Expr>
                {
                    new Expr("x", OperatorEnum.Equals, 1),
                    new Expr("y", OperatorEnum.Equals, 2)
                };
                Expr result = Expr.ListToNestedOrExpression(list);
                Check.Equal("((x Equals 1) Or (y Equals 2))", result.ToString());
            }));

            cases.Add(Case(Id, "or_multiple_items", "ListToNestedOrExpression nests multiple items right-recursively with Or", () =>
            {
                List<Expr> list = new List<Expr>
                {
                    new Expr("x", OperatorEnum.Equals, 1),
                    new Expr("y", OperatorEnum.Equals, 2),
                    new Expr("z", OperatorEnum.Equals, 3)
                };
                Expr result = Expr.ListToNestedOrExpression(list);
                Check.Equal("((x Equals 1) Or ((y Equals 2) Or (z Equals 3)))", result.ToString());
            }));

            // --- Negative / edge ---
            cases.Add(Case(Id, "and_empty_returns_null", "ListToNestedAndExpression returns null for an empty list", () =>
            {
                Check.Null(Expr.ListToNestedAndExpression(new List<Expr>()));
            }));

            cases.Add(Case(Id, "or_empty_returns_null", "ListToNestedOrExpression returns null for an empty list", () =>
            {
                Check.Null(Expr.ListToNestedOrExpression(new List<Expr>()));
            }));

            cases.Add(Case(Id, "and_null_throws", "ListToNestedAndExpression throws ArgumentNullException for a null list", () =>
            {
                Check.Throws<ArgumentNullException>(() => Expr.ListToNestedAndExpression(null));
            }));

            cases.Add(Case(Id, "or_null_throws", "ListToNestedOrExpression throws ArgumentNullException for a null list", () =>
            {
                Check.Throws<ArgumentNullException>(() => Expr.ListToNestedOrExpression(null));
            }));

            return new TestSuiteDescriptor(Id, "Expr list-to-nested conversion", cases);
        }
    }
}
