using System;
using System.Collections.Generic;
using ExpressionTree;

namespace ExpressionTree.Tests
{
    public class ExprListConversionTests
    {
        #region NestedAnd-Positive

        [Fact]
        public void ListToNestedAndExpression_SingleItem_ReturnsSameExpression()
        {
            Expr only = new Expr("x", OperatorEnum.Equals, 1);
            Expr result = Expr.ListToNestedAndExpression(new List<Expr> { only });

            Assert.Same(only, result);
        }

        [Fact]
        public void ListToNestedAndExpression_MultipleItems_NestsRightWithAnd()
        {
            List<Expr> list = new List<Expr>
            {
                new Expr("x", OperatorEnum.Equals, 1),
                new Expr("y", OperatorEnum.Equals, 2),
                new Expr("z", OperatorEnum.Equals, 3)
            };

            Expr result = Expr.ListToNestedAndExpression(list);

            Assert.Equal("((x Equals 1) And ((y Equals 2) And (z Equals 3)))", result.ToString());
        }

        #endregion

        #region NestedOr-Positive

        [Fact]
        public void ListToNestedOrExpression_SingleItem_ReturnsSameExpression()
        {
            Expr only = new Expr("x", OperatorEnum.Equals, 1);
            Expr result = Expr.ListToNestedOrExpression(new List<Expr> { only });

            Assert.Same(only, result);
        }

        [Fact]
        public void ListToNestedOrExpression_MultipleItems_NestsRightWithOr()
        {
            List<Expr> list = new List<Expr>
            {
                new Expr("x", OperatorEnum.Equals, 1),
                new Expr("y", OperatorEnum.Equals, 2),
                new Expr("z", OperatorEnum.Equals, 3)
            };

            Expr result = Expr.ListToNestedOrExpression(list);

            Assert.Equal("((x Equals 1) Or ((y Equals 2) Or (z Equals 3)))", result.ToString());
        }

        #endregion

        #region Negative-And-Empty

        [Fact]
        public void ListToNestedAndExpression_EmptyList_ReturnsNull()
        {
            Assert.Null(Expr.ListToNestedAndExpression(new List<Expr>()));
        }

        [Fact]
        public void ListToNestedOrExpression_EmptyList_ReturnsNull()
        {
            Assert.Null(Expr.ListToNestedOrExpression(new List<Expr>()));
        }

        [Fact]
        public void ListToNestedAndExpression_NullList_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Expr.ListToNestedAndExpression(null));
        }

        [Fact]
        public void ListToNestedOrExpression_NullList_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Expr.ListToNestedOrExpression(null));
        }

        #endregion
    }
}
