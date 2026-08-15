using System;
using System.Collections.Generic;
using ExpressionTree;

namespace ExpressionTree.Tests
{
    public class ExprToStringTests
    {
        #region Positive

        [Fact]
        public void ToString_SimpleLiteralExpression_RendersTerms()
        {
            Expr e = new Expr("id", OperatorEnum.Equals, 50);

            Assert.Equal("(id Equals 50)", e.ToString());
        }

        [Fact]
        public void ToString_NullRight_RendersNullMarker()
        {
            Expr e = new Expr("lastlogin", OperatorEnum.IsNull, null);

            Assert.Equal("(lastlogin IsNull (null))", e.ToString());
        }

        [Fact]
        public void ToString_NestedExpression_RendersRecursively()
        {
            Expr e = new Expr(
                new Expr("id", OperatorEnum.Equals, 50),
                OperatorEnum.And,
                new Expr("active", OperatorEnum.Equals, false));

            Assert.Equal("((id Equals 50) And (active Equals False))", e.ToString());
        }

        [Fact]
        public void ToString_RightSideList_RendersListMarker()
        {
            Expr e = new Expr("id", OperatorEnum.In, new List<object> { 1, 2, 3 });

            Assert.Equal("(id In (list))", e.ToString());
        }

        [Fact]
        public void ToString_RightSideArray_RendersArrayMarker()
        {
            Expr e = new Expr("id", OperatorEnum.In, new int[] { 1, 2, 3 });

            Assert.Equal("(id In (array))", e.ToString());
        }

        [Fact]
        public void ToString_GuidLiteral_IsTreatedAsLiteral()
        {
            Guid g = Guid.Parse("11111111-1111-1111-1111-111111111111");
            Expr e = new Expr("id", OperatorEnum.Equals, g);

            Assert.Equal("(id Equals " + g.ToString() + ")", e.ToString());
        }

        #endregion
    }
}
