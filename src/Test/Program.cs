using System;
using System.IO;
using ExpressionTree;

namespace Test.SystemTextJson
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Expr e = new Expr(
                new Expr(
                    "id",
                    OperatorEnum.Equals,
                    "50"),
                OperatorEnum.And,
                new Expr(
                    "active",
                    OperatorEnum.Equals,
                    false)
            );
            e.PrependAnd("lastlogin", OperatorEnum.IsNull, null);
            Console.WriteLine(e.ToString());
        }
    }
}
