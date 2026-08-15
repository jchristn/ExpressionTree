namespace Test.Shared.Suites
{
    using System;
    using System.Collections.Generic;
    using ExpressionTree;
    using Touchstone.Core;
    using static Test.Shared.TestFactory;

    /// <summary>
    /// Coverage for the configurable <see cref="Expr.LiteralTypes"/> and <see cref="Expr.RightRequired"/>
    /// properties, including their null-guard setters.
    /// </summary>
    public static class PropertiesSuite
    {
        private const string Id = "properties";

        /// <summary>
        /// Build the properties test suite.
        /// </summary>
        /// <returns>Suite descriptor.</returns>
        public static TestSuiteDescriptor Build()
        {
            List<TestCaseDescriptor> cases = new List<TestCaseDescriptor>();

            // --- LiteralTypes ---
            cases.Add(Case(Id, "literaltypes_default_common", "LiteralTypes default contains common value types", () =>
            {
                Expr e = new Expr();
                Check.Contains(typeof(string), e.LiteralTypes);
                Check.Contains(typeof(int), e.LiteralTypes);
                Check.Contains(typeof(int?), e.LiteralTypes);
                Check.Contains(typeof(long), e.LiteralTypes);
                Check.Contains(typeof(double), e.LiteralTypes);
                Check.Contains(typeof(decimal), e.LiteralTypes);
                Check.Contains(typeof(bool), e.LiteralTypes);
                Check.Contains(typeof(DateTime), e.LiteralTypes);
                Check.Contains(typeof(DateTimeOffset), e.LiteralTypes);
                Check.Contains(typeof(Guid), e.LiteralTypes);
                Check.Contains(typeof(Guid?), e.LiteralTypes);
            }));

            cases.Add(Case(Id, "literaltypes_set_null_throws", "Setting LiteralTypes to null throws ArgumentNullException", () =>
            {
                Expr e = new Expr();
                Check.Throws<ArgumentNullException>(() => e.LiteralTypes = null);
            }));

            cases.Add(Case(Id, "literaltypes_can_be_replaced", "LiteralTypes can be replaced with a custom list", () =>
            {
                Expr e = new Expr();
                List<Type> custom = new List<Type> { typeof(string) };
                e.LiteralTypes = custom;
                Check.Same(custom, e.LiteralTypes);
            }));

            // --- RightRequired ---
            cases.Add(Case(Id, "rightrequired_default_contents", "RightRequired default contains the expected operators", () =>
            {
                Expr e = new Expr();
                Check.Contains(OperatorEnum.Equals, e.RightRequired);
                Check.Contains(OperatorEnum.In, e.RightRequired);
                Check.Contains(OperatorEnum.And, e.RightRequired);
                Check.Contains(OperatorEnum.Or, e.RightRequired);
                Check.DoesNotContain(OperatorEnum.IsNull, e.RightRequired);
                Check.DoesNotContain(OperatorEnum.IsNotNull, e.RightRequired);
                Check.DoesNotContain(OperatorEnum.StartsWithNot, e.RightRequired);
                Check.DoesNotContain(OperatorEnum.EndsWithNot, e.RightRequired);
            }));

            cases.Add(Case(Id, "rightrequired_set_null_throws", "Setting RightRequired to null throws ArgumentNullException", () =>
            {
                Expr e = new Expr();
                Check.Throws<ArgumentNullException>(() => e.RightRequired = null);
            }));

            cases.Add(Case(Id, "rightrequired_can_be_replaced", "RightRequired can be replaced with a custom list", () =>
            {
                Expr e = new Expr();
                List<OperatorEnum> custom = new List<OperatorEnum> { OperatorEnum.Equals };
                e.RightRequired = custom;
                Check.Same(custom, e.RightRequired);
            }));

            return new TestSuiteDescriptor(Id, "Expr configurable properties", cases);
        }
    }
}
