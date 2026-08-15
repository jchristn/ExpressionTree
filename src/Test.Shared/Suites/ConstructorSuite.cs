namespace Test.Shared.Suites
{
    using System;
    using System.Collections.Generic;
    using ExpressionTree;
    using Touchstone.Core;
    using static Test.Shared.TestFactory;

    /// <summary>
    /// Coverage for the <see cref="Expr"/> constructors and their argument validation.
    /// </summary>
    public static class ConstructorSuite
    {
        private const string Id = "constructor";

        /// <summary>
        /// Operators that require a value on the Right term.
        /// </summary>
        private static readonly OperatorEnum[] RightRequiredOperators = new[]
        {
            OperatorEnum.And,
            OperatorEnum.Contains,
            OperatorEnum.ContainsNot,
            OperatorEnum.EndsWith,
            OperatorEnum.Equals,
            OperatorEnum.GreaterThan,
            OperatorEnum.GreaterThanOrEqualTo,
            OperatorEnum.In,
            OperatorEnum.LessThan,
            OperatorEnum.LessThanOrEqualTo,
            OperatorEnum.NotEquals,
            OperatorEnum.NotIn,
            OperatorEnum.Or,
            OperatorEnum.StartsWith
        };

        /// <summary>
        /// Operators that do NOT require a value on the Right term.
        /// </summary>
        private static readonly OperatorEnum[] RightOptionalOperators = new[]
        {
            OperatorEnum.IsNull,
            OperatorEnum.IsNotNull,
            OperatorEnum.StartsWithNot,
            OperatorEnum.EndsWithNot
        };

        /// <summary>
        /// Build the constructor test suite.
        /// </summary>
        /// <returns>Suite descriptor.</returns>
        public static TestSuiteDescriptor Build()
        {
            List<TestCaseDescriptor> cases = new List<TestCaseDescriptor>();

            // --- Positive ---
            cases.Add(Case(Id, "valid_terms_sets_properties", "Constructor with valid terms sets Left/Operator/Right", () =>
            {
                Expr e = new Expr("id", OperatorEnum.Equals, 50);
                Check.Equal("id", (string)e.Left);
                Check.Equal(OperatorEnum.Equals, e.Operator);
                Check.Equal(50, (int)e.Right);
            }));

            cases.Add(Case(Id, "default_ctor", "Default constructor yields Equals operator and null terms", () =>
            {
                Expr e = new Expr();
                Check.Null(e.Left);
                Check.Equal(OperatorEnum.Equals, e.Operator);
                Check.Null(e.Right);
            }));

            cases.Add(Case(Id, "nested_expression", "Constructor accepts nested expressions as terms", () =>
            {
                Expr inner = new Expr("id", OperatorEnum.Equals, 1);
                Expr outer = new Expr(inner, OperatorEnum.And, new Expr("active", OperatorEnum.Equals, true));
                Check.Same(inner, outer.Left);
                Check.Equal(OperatorEnum.And, outer.Operator);
                Check.NotNull(outer.Right);
            }));

            // Right-optional operators accept a null Right.
            foreach (OperatorEnum oper in RightOptionalOperators)
            {
                OperatorEnum captured = oper;
                cases.Add(Case(Id, "null_right_ok_" + captured, "Constructor allows null Right for " + captured, () =>
                {
                    Expr e = new Expr("lastlogin", captured, null);
                    Check.Equal("lastlogin", (string)e.Left);
                    Check.Equal(captured, e.Operator);
                    Check.Null(e.Right);
                }));
            }

            // Right-required operators accept a non-null Right.
            foreach (OperatorEnum oper in RightRequiredOperators)
            {
                OperatorEnum captured = oper;
                cases.Add(Case(Id, "value_right_ok_" + captured, "Constructor allows non-null Right for " + captured, () =>
                {
                    object right = (captured == OperatorEnum.In || captured == OperatorEnum.NotIn)
                        ? (object)new List<object> { 1, 2 }
                        : (captured == OperatorEnum.And || captured == OperatorEnum.Or)
                            ? (object)new Expr("x", OperatorEnum.Equals, 1)
                            : (object)1;
                    Expr e = new Expr("field", captured, right);
                    Check.Equal(captured, e.Operator);
                    Check.NotNull(e.Right);
                }));
            }

            // --- Negative ---
            cases.Add(Case(Id, "null_left_throws", "Constructor throws ArgumentNullException when Left is null", () =>
            {
                Check.Throws<ArgumentNullException>(() => new Expr(null, OperatorEnum.Equals, 1));
            }));

            foreach (OperatorEnum oper in RightRequiredOperators)
            {
                OperatorEnum captured = oper;
                cases.Add(Case(Id, "null_right_throws_" + captured, "Constructor throws ArgumentException for null Right with " + captured, () =>
                {
                    Check.Throws<ArgumentException>(() => new Expr("id", captured, null));
                }));
            }

            return new TestSuiteDescriptor(Id, "Expr constructors", cases);
        }
    }
}
