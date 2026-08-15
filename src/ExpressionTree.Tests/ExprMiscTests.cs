using System;
using System.Collections.Generic;
using ExpressionTree;

namespace ExpressionTree.Tests
{
    public class ExprMiscTests
    {
        #region Copy-Positive

        [Fact]
        public void Copy_ProducesEquivalentButDistinctInstance()
        {
            Expr original = new Expr("id", OperatorEnum.Equals, 50);
            Expr copy = original.Copy();

            Assert.NotSame(original, copy);
            Assert.Equal(original.ToString(), copy.ToString());
        }

        [Fact]
        public void Copy_MutatingCopyDoesNotAffectOriginal()
        {
            Expr original = new Expr("id", OperatorEnum.Equals, 50);
            Expr copy = original.Copy();
            copy.Operator = OperatorEnum.NotEquals;
            copy.Right = 99;

            Assert.Equal(OperatorEnum.Equals, original.Operator);
            Assert.Equal(50, original.Right);
        }

        #endregion

        #region LiteralTypes

        [Fact]
        public void LiteralTypes_DefaultContainsCommonTypes()
        {
            Expr e = new Expr();

            Assert.Contains(typeof(string), e.LiteralTypes);
            Assert.Contains(typeof(int), e.LiteralTypes);
            Assert.Contains(typeof(Guid), e.LiteralTypes);
            Assert.Contains(typeof(Guid?), e.LiteralTypes);
            Assert.Contains(typeof(DateTime), e.LiteralTypes);
        }

        [Fact]
        public void LiteralTypes_SetNull_ThrowsArgumentNullException()
        {
            Expr e = new Expr();
            Assert.Throws<ArgumentNullException>(() => e.LiteralTypes = null);
        }

        [Fact]
        public void LiteralTypes_CanBeReplaced()
        {
            Expr e = new Expr();
            List<Type> custom = new List<Type> { typeof(string) };
            e.LiteralTypes = custom;

            Assert.Same(custom, e.LiteralTypes);
        }

        #endregion

        #region RightRequired

        [Fact]
        public void RightRequired_DefaultContainsExpectedOperators()
        {
            Expr e = new Expr();

            Assert.Contains(OperatorEnum.Equals, e.RightRequired);
            Assert.Contains(OperatorEnum.In, e.RightRequired);
            Assert.DoesNotContain(OperatorEnum.IsNull, e.RightRequired);
            Assert.DoesNotContain(OperatorEnum.IsNotNull, e.RightRequired);
        }

        [Fact]
        public void RightRequired_SetNull_ThrowsArgumentNullException()
        {
            Expr e = new Expr();
            Assert.Throws<ArgumentNullException>(() => e.RightRequired = null);
        }

        [Fact]
        public void RightRequired_CanBeReplaced()
        {
            Expr e = new Expr();
            List<OperatorEnum> custom = new List<OperatorEnum> { OperatorEnum.Equals };
            e.RightRequired = custom;

            Assert.Same(custom, e.RightRequired);
        }

        #endregion
    }
}
