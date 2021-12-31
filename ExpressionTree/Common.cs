using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTree
{
    internal class Common
    {
        /// <summary>
        /// Determines if an object is of an array type (excluding String objects).
        /// </summary>
        /// <param name="o">Object.</param>
        /// <returns>True if the object is of an Array type.</returns>
        internal static bool IsArray(object o)
        {
            if (o == null) return false;
            if (o is string) return false;
            return o.GetType().IsArray;
        }

        /// <summary>
        /// Determines if an object is of a List type.
        /// </summary>
        /// <param name="o">Object.</param>
        /// <returns>True if the object is of a List type.</returns>
        internal static bool IsList(object o)
        {
            if (o == null) return false;
            return o is IList &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }
    }
}
