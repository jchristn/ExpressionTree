using System;
using System.Collections.Generic;
using ExpressionTree;

namespace ExpressionTree.Tests
{
    public class ExprConstructorTests
    {
        #region Positive

        [Fact]
        public void Constructor_WithValidTerms_SetsProperties()
        {
            Expr e = new Expr("id", OperatorEnum.Equals, 50);

            Assert.Equal("id", e.Left);
            Assert.Equal(OperatorEnum.Equals, e.Operator);
            Assert.Equal(50, e.Right);
        }

        [Fact]
        public void Constructor_Default_HasEqualsOperatorAndNullTerms()
        {
            Expr e = new Expr();

            Assert.Null(e.Left);
            Assert.Equal(OperatorEnum.Equals, e.Operator);
            Assert.Null(e.Right);
        }

        [Theory]
        [InlineData(OperatorEnum.IsNull)]
        [InlineData(OperatorEnum.IsNotNull)]
        [InlineData(OperatorEnum.StartsWithNot)]
        [InlineData(OperatorEnum.EndsWithNot)]
        public void Constructor_WithNullRightForNonRequiringOperator_Succeeds(OperatorEnum oper)
        {
            Expr e = new Expr("lastlogin", oper, null);

            Assert.Equal("lastlogin", e.Left);
            Assert.Equal(oper, e.Operator);
            Assert.Null(e.Right);
        }

        [Fact]
        public void Constructor_WithNestedExpression_Succeeds()
        {
            Expr inner = new Expr("id", OperatorEnum.Equals, 1);
            Expr outer = new Expr(inner, OperatorEnum.And, new Expr("active", OperatorEnum.Equals, true));

            Assert.Same(inner, outer.Left);
            Assert.Equal(OperatorEnum.And, outer.Operator);
        }

        #endregion

        #region Negative

        [Fact]
        public void Constructor_WithNullLeft_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Expr(null, OperatorEnum.Equals, 1));
        }

        [Theory]
        [InlineData(OperatorEnum.Equals)]
        [InlineData(OperatorEnum.NotEquals)]
        [InlineData(OperatorEnum.GreaterThan)]
        [InlineData(OperatorEnum.GreaterThanOrEqualTo)]
        [InlineData(OperatorEnum.LessThan)]
        [InlineData(OperatorEnum.LessThanOrEqualTo)]
        [InlineData(OperatorEnum.Contains)]
        [InlineData(OperatorEnum.ContainsNot)]
        [InlineData(OperatorEnum.StartsWith)]
        [InlineData(OperatorEnum.EndsWith)]
        [InlineData(OperatorEnum.In)]
        [InlineData(OperatorEnum.NotIn)]
        [InlineData(OperatorEnum.And)]
        [InlineData(OperatorEnum.Or)]
        public void Constructor_WithNullRightForRequiringOperator_ThrowsArgumentException(OperatorEnum oper)
        {
            Assert.Throws<ArgumentException>(() => new Expr("id", oper, null));
        }

        #endregion
    }
}
