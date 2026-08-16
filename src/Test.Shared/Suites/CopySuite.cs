namespace Test.Shared.Suites
{
    using System;
    using System.Collections.Generic;
    using ExpressionTree;
    using Touchstone.Core;
    using static Test.Shared.TestFactory;

    /// <summary>
    /// Coverage for <see cref="Expr.Copy"/>.
    /// </summary>
    public static class CopySuite
    {
        private const string Id = "copy";

        /// <summary>
        /// Build the copy test suite.
        /// </summary>
        /// <returns>Suite descriptor.</returns>
        public static TestSuiteDescriptor Build()
        {
            List<TestCaseDescriptor> cases = new List<TestCaseDescriptor>();

            cases.Add(Case(Id, "equivalent_distinct", "Copy produces an equivalent but distinct instance", () =>
            {
                Expr original = new Expr("id", OperatorEnum.Equals, 50);
                Expr copy = original.Copy();
                Check.NotSame(original, copy);
                Check.Equal(original.ToString(), copy.ToString());
            }));

            cases.Add(Case(Id, "mutation_isolated", "Mutating a copy does not affect the original", () =>
            {
                Expr original = new Expr("id", OperatorEnum.Equals, 50);
                Expr copy = original.Copy();
                copy.Operator = OperatorEnum.NotEquals;
                copy.Right = 99;
                Check.Equal(OperatorEnum.Equals, original.Operator);
                Check.Equal(50, (int)original.Right);
            }));

            cases.Add(Case(Id, "copy_of_nested", "Copy of a nested expression preserves rendering", () =>
            {
                Expr original = new Expr(
                    new Expr("id", OperatorEnum.Equals, 1),
                    OperatorEnum.And,
                    new Expr("active", OperatorEnum.Equals, true));
                Expr copy = original.Copy();
                Check.NotSame(original, copy);
                Check.Equal(original.ToString(), copy.ToString());
            }));

            // Copy is a shallow copy: it re-uses the same nested child references rather than
            // cloning them recursively. Mutating a scalar on the copy is isolated (see above), but
            // the nested Expr instances themselves are shared with the original.
            cases.Add(Case(Id, "copy_is_shallow_shares_children", "Copy is shallow: nested child expressions are shared with the original", () =>
            {
                Expr child = new Expr("id", OperatorEnum.Equals, 1);
                Expr original = new Expr(child, OperatorEnum.And, new Expr("active", OperatorEnum.Equals, true));
                Expr copy = original.Copy();
                Check.NotSame(original, copy);
                Check.Same(original.Left, copy.Left);
                Check.Same(original.Right, copy.Right);
            }));

            // Copy is implemented as new Expr(Left, Operator, Right); a default Expr has a null Left,
            // so copying it surfaces the constructor's null-left guard.
            cases.Add(Case(Id, "copy_of_default_throws", "Copy of a default (null-left) expression throws ArgumentNullException", () =>
            {
                Expr original = new Expr();
                Check.Throws<ArgumentNullException>(() => original.Copy());
            }));

            return new TestSuiteDescriptor(Id, "Expr.Copy", cases);
        }
    }
}
