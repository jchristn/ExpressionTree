namespace Test.Shared.Suites
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.Serialization;
    using ExpressionTree;
    using Touchstone.Core;
    using static Test.Shared.TestFactory;

    /// <summary>
    /// Coverage for the <see cref="OperatorEnum"/> surface: expected members and their
    /// <see cref="EnumMemberAttribute"/> serialization values.
    /// </summary>
    public static class OperatorEnumSuite
    {
        private const string Id = "operator_enum";

        private static readonly string[] Expected = new[]
        {
            "Equals", "NotEquals", "GreaterThan", "GreaterThanOrEqualTo", "LessThan",
            "LessThanOrEqualTo", "IsNull", "IsNotNull", "Contains", "ContainsNot",
            "StartsWith", "StartsWithNot", "EndsWith", "EndsWithNot", "And", "Or", "In", "NotIn"
        };

        /// <summary>
        /// Build the operator-enum test suite.
        /// </summary>
        /// <returns>Suite descriptor.</returns>
        public static TestSuiteDescriptor Build()
        {
            List<TestCaseDescriptor> cases = new List<TestCaseDescriptor>();

            cases.Add(Case(Id, "member_count", "OperatorEnum defines exactly the expected number of members", () =>
            {
                string[] names = Enum.GetNames(typeof(OperatorEnum));
                Check.Equal(Expected.Length, names.Length);
            }));

            cases.Add(Case(Id, "all_expected_members_present", "OperatorEnum contains every expected member", () =>
            {
                foreach (string name in Expected)
                {
                    Check.True(Enum.IsDefined(typeof(OperatorEnum), name), "Missing OperatorEnum member: " + name);
                }
            }));

            cases.Add(Case(Id, "enum_member_values_match_names", "Each OperatorEnum member's EnumMember value matches its name", () =>
            {
                foreach (string name in Expected)
                {
                    FieldInfo field = typeof(OperatorEnum).GetField(name);
                    Check.NotNull(field);
                    EnumMemberAttribute attr = (EnumMemberAttribute)Attribute.GetCustomAttribute(field, typeof(EnumMemberAttribute));
                    Check.NotNull(attr);
                    Check.Equal(name, attr.Value);
                }
            }));

            return new TestSuiteDescriptor(Id, "OperatorEnum surface", cases);
        }
    }
}
