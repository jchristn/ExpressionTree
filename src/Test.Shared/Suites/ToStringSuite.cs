namespace Test.Shared.Suites
{
    using System;
    using System.Collections.Generic;
    using ExpressionTree;
    using Touchstone.Core;
    using static Test.Shared.TestFactory;

    /// <summary>
    /// Coverage for <see cref="Expr.ToString"/> rendering across literal, list, array,
    /// nested, and null terms on both the left and right sides.
    /// </summary>
    public static class ToStringSuite
    {
        private const string Id = "tostring";

        /// <summary>
        /// Build the ToString rendering test suite.
        /// </summary>
        /// <returns>Suite descriptor.</returns>
        public static TestSuiteDescriptor Build()
        {
            List<TestCaseDescriptor> cases = new List<TestCaseDescriptor>();

            cases.Add(Case(Id, "simple_literal", "ToString renders a simple literal expression", () =>
            {
                Expr e = new Expr("id", OperatorEnum.Equals, 50);
                Check.Equal("(id Equals 50)", e.ToString());
            }));

            cases.Add(Case(Id, "null_right_marker", "ToString renders a (null) marker for a null right term", () =>
            {
                Expr e = new Expr("lastlogin", OperatorEnum.IsNull, null);
                Check.Equal("(lastlogin IsNull (null))", e.ToString());
            }));

            cases.Add(Case(Id, "nested_both_sides", "ToString renders nested expressions on both sides recursively", () =>
            {
                Expr e = new Expr(
                    new Expr("id", OperatorEnum.Equals, 50),
                    OperatorEnum.And,
                    new Expr("active", OperatorEnum.Equals, false));
                Check.Equal("((id Equals 50) And (active Equals False))", e.ToString());
            }));

            cases.Add(Case(Id, "nested_right_only", "ToString renders a literal left with a nested right expression", () =>
            {
                Expr e = new Expr("a", OperatorEnum.And, new Expr("b", OperatorEnum.Equals, 1));
                Check.Equal("(a And (b Equals 1))", e.ToString());
            }));

            cases.Add(Case(Id, "right_list_marker", "ToString renders a (list) marker for a list right term", () =>
            {
                Expr e = new Expr("id", OperatorEnum.In, new List<object> { 1, 2, 3 });
                Check.Equal("(id In (list))", e.ToString());
            }));

            cases.Add(Case(Id, "right_array_marker", "ToString renders an (array) marker for an array right term", () =>
            {
                Expr e = new Expr("id", OperatorEnum.In, new int[] { 1, 2, 3 });
                Check.Equal("(id In (array))", e.ToString());
            }));

            cases.Add(Case(Id, "left_list_marker", "ToString renders a (list) marker for a list left term", () =>
            {
                Expr e = new Expr(new List<object> { 1, 2, 3 }, OperatorEnum.IsNotNull, null);
                Check.Equal("((list) IsNotNull (null))", e.ToString());
            }));

            cases.Add(Case(Id, "left_array_marker", "ToString renders an (array) marker for an array left term", () =>
            {
                Expr e = new Expr(new int[] { 1, 2, 3 }, OperatorEnum.IsNotNull, null);
                Check.Equal("((array) IsNotNull (null))", e.ToString());
            }));

            // --- Literal types ---
            cases.Add(Case(Id, "guid_literal", "ToString treats a Guid as a literal", () =>
            {
                Guid g = Guid.Parse("11111111-1111-1111-1111-111111111111");
                Expr e = new Expr("id", OperatorEnum.Equals, g);
                Check.Equal("(id Equals " + g.ToString() + ")", e.ToString());
            }));

            cases.Add(Case(Id, "nullable_guid_literal", "ToString treats a nullable Guid as a literal", () =>
            {
                Guid? g = Guid.Parse("22222222-2222-2222-2222-222222222222");
                Expr e = new Expr("id", OperatorEnum.Equals, g);
                Check.Equal("(id Equals " + g.Value.ToString() + ")", e.ToString());
            }));

            cases.Add(Case(Id, "bool_true_literal", "ToString renders a true boolean literal", () =>
            {
                Expr e = new Expr("active", OperatorEnum.Equals, true);
                Check.Equal("(active Equals True)", e.ToString());
            }));

            cases.Add(Case(Id, "bool_false_literal", "ToString renders a false boolean literal", () =>
            {
                Expr e = new Expr("active", OperatorEnum.Equals, false);
                Check.Equal("(active Equals False)", e.ToString());
            }));

            cases.Add(Case(Id, "long_literal", "ToString renders a long literal", () =>
            {
                Expr e = new Expr("id", OperatorEnum.Equals, 9000000000L);
                Check.Equal("(id Equals 9000000000)", e.ToString());
            }));

            cases.Add(Case(Id, "double_literal", "ToString renders a double literal", () =>
            {
                double d = 1.5;
                Expr e = new Expr("ratio", OperatorEnum.Equals, d);
                Check.Equal("(ratio Equals " + d.ToString() + ")", e.ToString());
            }));

            cases.Add(Case(Id, "decimal_literal", "ToString renders a decimal literal", () =>
            {
                decimal m = 2.50m;
                Expr e = new Expr("price", OperatorEnum.Equals, m);
                Check.Equal("(price Equals " + m.ToString() + ")", e.ToString());
            }));

            cases.Add(Case(Id, "nullable_int_literal", "ToString renders a nullable int literal", () =>
            {
                int? n = 42;
                Expr e = new Expr("count", OperatorEnum.Equals, n);
                Check.Equal("(count Equals 42)", e.ToString());
            }));

            cases.Add(Case(Id, "datetime_literal", "ToString treats a DateTime as a literal", () =>
            {
                DateTime dt = new DateTime(2020, 1, 2, 3, 4, 5, DateTimeKind.Utc);
                Expr e = new Expr("created", OperatorEnum.GreaterThan, dt);
                Check.Equal("(created GreaterThan " + dt.ToString() + ")", e.ToString());
            }));

            cases.Add(Case(Id, "datetimeoffset_literal", "ToString treats a DateTimeOffset as a literal", () =>
            {
                DateTimeOffset dto = new DateTimeOffset(2020, 1, 2, 3, 4, 5, TimeSpan.Zero);
                Expr e = new Expr("created", OperatorEnum.GreaterThan, dto);
                Check.Equal("(created GreaterThan " + dto.ToString() + ")", e.ToString());
            }));

            return new TestSuiteDescriptor(Id, "Expr.ToString rendering", cases);
        }
    }
}
