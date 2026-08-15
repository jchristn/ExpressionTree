using System;
using System.Collections.Generic;
using ExpressionTree;

namespace ExpressionTree.Tests
{
    public class ExprPrependTests
    {
        #region PrependAnd-Positive

        [Fact]
        public void PrependAnd_WithTerms_WrapsInAndClause()
        {
            Expr e = new Expr("id", OperatorEnum.GreaterThan, 0);
            e.PrependAnd("name", OperatorEnum.Equals, "joel");

            Assert.Equal(OperatorEnum.And, e.Operator);
            Assert.Equal("((name Equals joel) And (id GreaterThan 0))", e.ToString());
        }

        [Fact]
        public void PrependAnd_ReturnsSameInstanceForChaining()
        {
            Expr e = new Expr("id", OperatorEnum.GreaterThan, 0);
            Expr result = e.PrependAnd("name", OperatorEnum.Equals, "joel");

            Assert.Same(e, result);
        }

        [Fact]
        public void PrependAnd_WithExpression_WrapsInAndClause()
        {
            Expr e = new Expr("id", OperatorEnum.GreaterThan, 0);
            e.PrependAnd(new Expr("name", OperatorEnum.Equals, "joel"));

            Assert.Equal("((name Equals joel) And (id GreaterThan 0))", e.ToString());
        }

        #endregion

        #region PrependOr-Positive

        [Fact]
        public void PrependOr_WithTerms_WrapsInOrClause()
        {
            Expr e = new Expr("id", OperatorEnum.GreaterThan, 0);
            e.PrependOr("name", OperatorEnum.Equals, "joel");

            Assert.Equal(OperatorEnum.Or, e.Operator);
            Assert.Equal("((name Equals joel) Or (id GreaterThan 0))", e.ToString());
        }

        [Fact]
        public void PrependOr_WithExpression_WrapsInOrClause()
        {
            Expr e = new Expr("id", OperatorEnum.GreaterThan, 0);
            e.PrependOr(new Expr("name", OperatorEnum.Equals, "joel"));

            Assert.Equal("((name Equals joel) Or (id GreaterThan 0))", e.ToString());
        }

        #endregion

        #region Prepend-Negative

        [Fact]
        public void PrependAnd_WithNullExpression_ThrowsArgumentNullException()
        {
            Expr e = new Expr("id", OperatorEnum.GreaterThan, 0);
            Assert.Throws<ArgumentNullException>(() => e.PrependAnd((Expr)null!));
        }

        [Fact]
        public void PrependOr_WithNullExpression_ThrowsArgumentNullException()
        {
            Expr e = new Expr("id", OperatorEnum.GreaterThan, 0);
            Assert.Throws<ArgumentNullException>(() => e.PrependOr((Expr)null!));
        }

        [Fact]
        public void PrependAnd_WithNullRightForRequiringOperator_ThrowsArgumentException()
        {
            Expr e = new Expr("id", OperatorEnum.GreaterThan, 0);
            Assert.Throws<ArgumentException>(() => e.PrependAnd("name", OperatorEnum.Equals, null!));
        }

        [Fact]
        public void PrependOr_WithNullRightForRequiringOperator_ThrowsArgumentException()
        {
            Expr e = new Expr("id", OperatorEnum.GreaterThan, 0);
            Assert.Throws<ArgumentException>(() => e.PrependOr("name", OperatorEnum.Equals, null!));
        }

        #endregion

        #region PrependClause-Static

        [Fact]
        public void PrependAndClause_BuildsAndExpression()
        {
            Expr a = new Expr("a", OperatorEnum.Equals, 1);
            Expr b = new Expr("b", OperatorEnum.Equals, 2);
            Expr result = Expr.PrependAndClause(a, b);

            Assert.Same(a, result.Left);
            Assert.Equal(OperatorEnum.And, result.Operator);
            Assert.Same(b, result.Right);
        }

        [Fact]
        public void PrependOrClause_BuildsOrExpression()
        {
            Expr a = new Expr("a", OperatorEnum.Equals, 1);
            Expr b = new Expr("b", OperatorEnum.Equals, 2);
            Expr result = Expr.PrependOrClause(a, b);

            Assert.Same(a, result.Left);
            Assert.Equal(OperatorEnum.Or, result.Operator);
            Assert.Same(b, result.Right);
        }

        [Fact]
        public void PrependAndClause_WithNullPrepend_ThrowsArgumentNullException()
        {
            Expr b = new Expr("b", OperatorEnum.Equals, 2);
            Assert.Throws<ArgumentNullException>(() => Expr.PrependAndClause(null, b));
        }

        [Fact]
        public void PrependAndClause_WithNullOriginal_ThrowsArgumentNullException()
        {
            Expr a = new Expr("a", OperatorEnum.Equals, 1);
            Assert.Throws<ArgumentNullException>(() => Expr.PrependAndClause(a, null));
        }

        [Fact]
        public void PrependOrClause_WithNullPrepend_ThrowsArgumentNullException()
        {
            Expr b = new Expr("b", OperatorEnum.Equals, 2);
            Assert.Throws<ArgumentNullException>(() => Expr.PrependOrClause(null, b));
        }

        [Fact]
        public void PrependOrClause_WithNullOriginal_ThrowsArgumentNullException()
        {
            Expr a = new Expr("a", OperatorEnum.Equals, 1);
            Assert.Throws<ArgumentNullException>(() => Expr.PrependOrClause(a, null));
        }

        #endregion
    }
}
