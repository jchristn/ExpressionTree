namespace Test.Shared.Suites
{
    using System;
    using System.Collections.Generic;
    using ExpressionTree;
    using Touchstone.Core;
    using static Test.Shared.TestFactory;

    /// <summary>
    /// Coverage for the static <see cref="Expr.Between(object, List{object})"/> factory.
    /// </summary>
    public static class BetweenSuite
    {
        private const string Id = "between";

        /// <summary>
        /// Build the Between test suite.
        /// </summary>
        /// <returns>Suite descriptor.</returns>
        public static TestSuiteDescriptor Build()
        {
            List<TestCaseDescriptor> cases = new List<TestCaseDescriptor>();

            // --- Positive ---
            cases.Add(Case(Id, "two_values_builds_and", "Between with two values builds an And clause", () =>
            {
                Expr e = Expr.Between("age", new List<object> { 18, 65 });
                Check.Equal(OperatorEnum.And, e.Operator);
            }));

            cases.Add(Case(Id, "two_values_tostring", "Between renders lower/upper bounds via ToString", () =>
            {
                Expr e = Expr.Between("age", new List<object> { 18, 65 });
                Check.Equal("((age GreaterThanOrEqualTo 18) And (age LessThanOrEqualTo 65))", e.ToString());
            }));

            cases.Add(Case(Id, "nested_left", "Between accepts a nested expression as the left term", () =>
            {
                Expr left = new Expr("a", OperatorEnum.Equals, 1);
                Expr e = Expr.Between(left, new List<object> { 10, 20 });
                Check.Equal(OperatorEnum.And, e.Operator);
                Check.NotNull(e.Left);
                Check.NotNull(e.Right);
            }));

            cases.Add(Case(Id, "string_bounds", "Between supports string bounds", () =>
            {
                Expr e = Expr.Between("name", new List<object> { "a", "m" });
                Check.Equal("((name GreaterThanOrEqualTo a) And (name LessThanOrEqualTo m))", e.ToString());
            }));

            // --- Negative ---
            cases.Add(Case(Id, "null_right_throws", "Between throws ArgumentNullException when the bounds list is null", () =>
            {
                Check.Throws<ArgumentNullException>(() => Expr.Between("age", null));
            }));

            cases.Add(Case(Id, "single_value_throws", "Between throws ArgumentException with a single bound", () =>
            {
                Check.Throws<ArgumentException>(() => Expr.Between("age", new List<object> { 18 }));
            }));

            cases.Add(Case(Id, "three_values_throws", "Between throws ArgumentException with three bounds", () =>
            {
                Check.Throws<ArgumentException>(() => Expr.Between("age", new List<object> { 18, 40, 65 }));
            }));

            cases.Add(Case(Id, "empty_list_throws", "Between throws ArgumentException with an empty bounds list", () =>
            {
                Check.Throws<ArgumentException>(() => Expr.Between("age", new List<object>()));
            }));

            return new TestSuiteDescriptor(Id, "Expr.Between factory", cases);
        }
    }
}
