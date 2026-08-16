namespace Test.Shared.Suites
{
    using System;
    using System.Collections.Generic;
    using ExpressionTree;
    using Touchstone.Core;
    using static Test.Shared.TestFactory;

    /// <summary>
    /// Coverage for the instance-level <see cref="Expr.PrependAnd(object, OperatorEnum, object)"/> and
    /// <see cref="Expr.PrependOr(object, OperatorEnum, object)"/> methods (and their Expr overloads).
    /// </summary>
    public static class PrependSuite
    {
        private const string Id = "prepend";

        /// <summary>
        /// Build the prepend test suite.
        /// </summary>
        /// <returns>Suite descriptor.</returns>
        public static TestSuiteDescriptor Build()
        {
            List<TestCaseDescriptor> cases = new List<TestCaseDescriptor>();

            // --- PrependAnd positive ---
            cases.Add(Case(Id, "and_terms_wraps", "PrependAnd(terms) wraps existing expression in an And clause", () =>
            {
                Expr e = new Expr("id", OperatorEnum.GreaterThan, 0);
                e.PrependAnd("name", OperatorEnum.Equals, "joel");
                Check.Equal(OperatorEnum.And, e.Operator);
                Check.Equal("((name Equals joel) And (id GreaterThan 0))", e.ToString());
            }));

            cases.Add(Case(Id, "and_returns_same_instance", "PrependAnd returns the same instance for chaining", () =>
            {
                Expr e = new Expr("id", OperatorEnum.GreaterThan, 0);
                Expr result = e.PrependAnd("name", OperatorEnum.Equals, "joel");
                Check.Same(e, result);
            }));

            cases.Add(Case(Id, "and_expression_wraps", "PrependAnd(Expr) wraps existing expression in an And clause", () =>
            {
                Expr e = new Expr("id", OperatorEnum.GreaterThan, 0);
                e.PrependAnd(new Expr("name", OperatorEnum.Equals, "joel"));
                Check.Equal("((name Equals joel) And (id GreaterThan 0))", e.ToString());
            }));

            // --- PrependOr positive ---
            cases.Add(Case(Id, "or_terms_wraps", "PrependOr(terms) wraps existing expression in an Or clause", () =>
            {
                Expr e = new Expr("id", OperatorEnum.GreaterThan, 0);
                e.PrependOr("name", OperatorEnum.Equals, "joel");
                Check.Equal(OperatorEnum.Or, e.Operator);
                Check.Equal("((name Equals joel) Or (id GreaterThan 0))", e.ToString());
            }));

            cases.Add(Case(Id, "or_returns_same_instance", "PrependOr returns the same instance for chaining", () =>
            {
                Expr e = new Expr("id", OperatorEnum.GreaterThan, 0);
                Expr result = e.PrependOr("name", OperatorEnum.Equals, "joel");
                Check.Same(e, result);
            }));

            cases.Add(Case(Id, "or_expression_wraps", "PrependOr(Expr) wraps existing expression in an Or clause", () =>
            {
                Expr e = new Expr("id", OperatorEnum.GreaterThan, 0);
                e.PrependOr(new Expr("name", OperatorEnum.Equals, "joel"));
                Check.Equal("((name Equals joel) Or (id GreaterThan 0))", e.ToString());
            }));

            // --- Chaining ---
            cases.Add(Case(Id, "chain_multiple", "Multiple prepends chain in nesting order", () =>
            {
                Expr e = new Expr("hello", OperatorEnum.Equals, "world")
                    .PrependAnd("id", OperatorEnum.GreaterThan, 0)
                    .PrependAnd("state", OperatorEnum.Equals, "active");
                Check.Equal("((state Equals active) And ((id GreaterThan 0) And (hello Equals world)))", e.ToString());
            }));

            cases.Add(Case(Id, "and_allows_null_right_for_optional_operator", "PrependAnd allows null Right for a right-optional operator", () =>
            {
                Expr e = new Expr("id", OperatorEnum.GreaterThan, 0);
                e.PrependAnd("lastlogin", OperatorEnum.IsNull, null);
                Check.Equal("((lastlogin IsNull (null)) And (id GreaterThan 0))", e.ToString());
            }));

            cases.Add(Case(Id, "or_allows_null_right_for_optional_operator", "PrependOr allows null Right for a right-optional operator", () =>
            {
                Expr e = new Expr("id", OperatorEnum.GreaterThan, 0);
                e.PrependOr("lastlogin", OperatorEnum.IsNull, null);
                Check.Equal("((lastlogin IsNull (null)) Or (id GreaterThan 0))", e.ToString());
            }));

            // --- Negative ---
            cases.Add(Case(Id, "and_null_expression_throws", "PrependAnd(null Expr) throws ArgumentNullException", () =>
            {
                Expr e = new Expr("id", OperatorEnum.GreaterThan, 0);
                Check.Throws<ArgumentNullException>(() => e.PrependAnd((Expr)null));
            }));

            cases.Add(Case(Id, "or_null_expression_throws", "PrependOr(null Expr) throws ArgumentNullException", () =>
            {
                Expr e = new Expr("id", OperatorEnum.GreaterThan, 0);
                Check.Throws<ArgumentNullException>(() => e.PrependOr((Expr)null));
            }));

            cases.Add(Case(Id, "and_null_right_required_throws", "PrependAnd throws ArgumentException for null Right with a right-required operator", () =>
            {
                Expr e = new Expr("id", OperatorEnum.GreaterThan, 0);
                Check.Throws<ArgumentException>(() => e.PrependAnd("name", OperatorEnum.Equals, null));
            }));

            cases.Add(Case(Id, "or_null_right_required_throws", "PrependOr throws ArgumentException for null Right with a right-required operator", () =>
            {
                Expr e = new Expr("id", OperatorEnum.GreaterThan, 0);
                Check.Throws<ArgumentException>(() => e.PrependOr("name", OperatorEnum.Equals, null));
            }));

            return new TestSuiteDescriptor(Id, "Expr prepend (instance)", cases);
        }
    }
}
