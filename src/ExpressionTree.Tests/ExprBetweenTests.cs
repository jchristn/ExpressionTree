using System;
using System.Collections.Generic;
using ExpressionTree;

namespace ExpressionTree.Tests
{
    public class ExprBetweenTests
    {
        #region Positive

        [Fact]
        public void Between_WithTwoValues_BuildsAndClause()
        {
            Expr e = Expr.Between("age", new List<object> { 18, 65 });

            Assert.Equal(OperatorEnum.And, e.Operator);
            Assert.Equal("((age GreaterThanOrEqualTo 18) And (age LessThanOrEqualTo 65))", e.ToString());
        }

        #endregion

        #region Negative

        [Fact]
        public void Between_WithNullRight_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Expr.Between("age", null));
        }

        [Fact]
        public void Between_WithSingleValue_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => Expr.Between("age", new List<object> { 18 }));
        }

        [Fact]
        public void Between_WithThreeValues_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => Expr.Between("age", new List<object> { 18, 40, 65 }));
        }

        [Fact]
        public void Between_WithEmptyList_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => Expr.Between("age", new List<object>()));
        }

        #endregion
    }
}
