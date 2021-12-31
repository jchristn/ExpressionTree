using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ExpressionTree
{
    /// <summary>
    /// Expression.
    /// </summary>
    [Serializable]
    public class Expr
    {
        #region Public-Members

        /// <summary>
        /// Left term.
        /// </summary>
        public object Left { get; set; } = null;

        /// <summary>
        /// Operator.
        /// </summary>
        public OperatorEnum Operator { get; set; } = OperatorEnum.Equals;

        /// <summary>
        /// Right term.
        /// </summary>
        public object Right { get; set; } = null;

        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        /// <summary>
        /// Instantiate.
        /// </summary>
        public Expr()
        {

        }

        /// <summary>
        /// A structure in the form of term-operator-term that defines a Boolean evaluation.
        /// A term can be a literal value, an embedded Expr object, or a list.
        /// List and Array objects can only be supplied on the right side of an expression.
        /// </summary>
        /// <param name="left">The left term of the expression.</param>
        /// <param name="oper">The operator.</param>
        /// <param name="right">The right term of the expression.</param>
        public Expr(object left, OperatorEnum oper, object right)
        {
            if (left == null) throw new ArgumentNullException(nameof(left));
            if (_RightRequired.Contains(oper) && right == null)
                throw new ArgumentException("The specified operator '" + oper.ToString() + "' requires a term on the 'Right' property.");

            Left = left;
            Operator = oper;
            Right = right;
        }

        #endregion

        #region Public-Methods

        /// <summary>
        /// String representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string ret = "(";

            Type leftType = Left.GetType();
            Type operType = Operator.GetType();
            Type rightType = Right.GetType();

            if (_LiteralTypes.Contains(leftType))
            {
                // Console.WriteLine("Appending left literal: " + Left.ToString());
                ret += Left.ToString() + " ";
            }
            else if (Common.IsArray(Left))
            {
                // Console.WriteLine("Appending left array");
                ret += "(array) ";
            }
            else if (Common.IsList(Left))
            {
                // Console.WriteLine("Appending left list");
                ret += "(list) ";
            }
            else
            {
                // Console.WriteLine("Appending left expression: " + Left.ToString());
                ret += ((Expr)Left).ToString() + " ";
            }

            ret += Operator.ToString();

            if (Right != null)
            {
                if (_LiteralTypes.Contains(rightType))
                {
                    // Console.WriteLine("Appending right literal: " + Right.ToString());
                    ret += " " + Right.ToString();
                }
                else if (Common.IsArray(Right))
                {
                    // Console.WriteLine("Appending right array");
                    ret += " (array)";
                }
                else if (Common.IsList(Right))
                {
                    // Console.WriteLine("Appending right list");
                    ret += " (list)";
                }
                else
                {
                    // Console.WriteLine("Appending right expression: " + Right.ToString());
                    ret += " " + ((Expr)Right).ToString();
                }
            }

            ret += ")";
            return ret;
        }

        #endregion

        #region Internal-Methods

        #endregion

        #region Private-Methods

        private List<OperatorEnum> _RightRequired = new List<OperatorEnum>
        {
            OperatorEnum.And,
            OperatorEnum.Contains,
            OperatorEnum.ContainsNot,
            OperatorEnum.EndsWith,
            OperatorEnum.Equals,
            OperatorEnum.GreaterThan,
            OperatorEnum.GreaterThanOrEqualTo,
            OperatorEnum.In,
            OperatorEnum.LessThan,
            OperatorEnum.LessThanOrEqualTo,
            OperatorEnum.NotEquals,
            OperatorEnum.NotIn,
            OperatorEnum.Or,
            OperatorEnum.StartsWith
        };

        private List<Type> _LiteralTypes = new List<Type>
        {
            typeof(string),
            typeof(bool),
            typeof(bool?),
            typeof(int),
            typeof(int?),
            typeof(DateTime),
            typeof(DateTime?),
            typeof(DateTimeOffset),
            typeof(DateTimeOffset?),
            typeof(decimal),
            typeof(decimal?),
            typeof(double),
            typeof(double?),
            typeof(long),
            typeof(long?)
        };

        #endregion
    }
}
