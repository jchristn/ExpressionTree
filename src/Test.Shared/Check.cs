namespace Test.Shared
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Minimal, framework-agnostic assertion helpers used by Touchstone test descriptors.
    /// Every failure throws <see cref="AssertException"/> so it surfaces identically through
    /// the CLI runner, the xUnit adapter, and the NUnit adapter.
    /// </summary>
    public static class Check
    {
        /// <summary>
        /// Assert that a condition is true.
        /// </summary>
        /// <param name="condition">Condition expected to be true.</param>
        /// <param name="message">Optional failure message.</param>
        public static void True(bool condition, string message = null)
        {
            if (!condition) throw new AssertException(message ?? "Expected condition to be true.");
        }

        /// <summary>
        /// Assert that a condition is false.
        /// </summary>
        /// <param name="condition">Condition expected to be false.</param>
        /// <param name="message">Optional failure message.</param>
        public static void False(bool condition, string message = null)
        {
            if (condition) throw new AssertException(message ?? "Expected condition to be false.");
        }

        /// <summary>
        /// Assert that two values are equal using the default equality comparer.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="expected">Expected value.</param>
        /// <param name="actual">Actual value.</param>
        public static void Equal<T>(T expected, T actual)
        {
            if (!EqualityComparer<T>.Default.Equals(expected, actual))
                throw new AssertException("Expected [" + Format(expected) + "] but was [" + Format(actual) + "].");
        }

        /// <summary>
        /// Assert that two values are not equal.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="notExpected">Value the actual must not equal.</param>
        /// <param name="actual">Actual value.</param>
        public static void NotEqual<T>(T notExpected, T actual)
        {
            if (EqualityComparer<T>.Default.Equals(notExpected, actual))
                throw new AssertException("Expected value to differ from [" + Format(notExpected) + "] but it was equal.");
        }

        /// <summary>
        /// Assert that a value is null.
        /// </summary>
        /// <param name="value">Value expected to be null.</param>
        public static void Null(object value)
        {
            if (value != null) throw new AssertException("Expected null but was [" + Format(value) + "].");
        }

        /// <summary>
        /// Assert that a value is not null.
        /// </summary>
        /// <param name="value">Value expected to be non-null.</param>
        public static void NotNull(object value)
        {
            if (value == null) throw new AssertException("Expected non-null but was null.");
        }

        /// <summary>
        /// Assert that two references point to the same instance.
        /// </summary>
        /// <param name="expected">Expected instance.</param>
        /// <param name="actual">Actual instance.</param>
        public static void Same(object expected, object actual)
        {
            if (!ReferenceEquals(expected, actual))
                throw new AssertException("Expected the same instance but references differed.");
        }

        /// <summary>
        /// Assert that two references do not point to the same instance.
        /// </summary>
        /// <param name="notExpected">Instance the actual must not be.</param>
        /// <param name="actual">Actual instance.</param>
        public static void NotSame(object notExpected, object actual)
        {
            if (ReferenceEquals(notExpected, actual))
                throw new AssertException("Expected different instances but references were the same.");
        }

        /// <summary>
        /// Assert that a sequence contains an item.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="expected">Item expected to be present.</param>
        /// <param name="collection">Collection to inspect.</param>
        public static void Contains<T>(T expected, IEnumerable<T> collection)
        {
            if (collection == null || !collection.Contains(expected))
                throw new AssertException("Expected collection to contain [" + Format(expected) + "].");
        }

        /// <summary>
        /// Assert that a sequence does not contain an item.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="notExpected">Item expected to be absent.</param>
        /// <param name="collection">Collection to inspect.</param>
        public static void DoesNotContain<T>(T notExpected, IEnumerable<T> collection)
        {
            if (collection != null && collection.Contains(notExpected))
                throw new AssertException("Expected collection to not contain [" + Format(notExpected) + "].");
        }

        /// <summary>
        /// Assert that the supplied action throws an exception of the specified type (or a subtype).
        /// </summary>
        /// <typeparam name="TException">Expected exception type.</typeparam>
        /// <param name="action">Action expected to throw.</param>
        /// <returns>The caught exception.</returns>
        public static TException Throws<TException>(Action action) where TException : Exception
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            try
            {
                action();
            }
            catch (TException ex)
            {
                return ex;
            }
            catch (Exception ex)
            {
                throw new AssertException("Expected exception of type " + typeof(TException).Name +
                    " but caught " + ex.GetType().Name + ": " + ex.Message);
            }

            throw new AssertException("Expected exception of type " + typeof(TException).Name + " but none was thrown.");
        }

        private static string Format(object value)
        {
            if (value == null) return "null";
            if (value is string s) return s;
            if (value is IEnumerable en && !(value is string))
            {
                List<string> parts = new List<string>();
                foreach (object o in en) parts.Add(o == null ? "null" : o.ToString());
                return "[" + string.Join(", ", parts) + "]";
            }
            return value.ToString();
        }
    }

    /// <summary>
    /// Exception raised when a <see cref="Check"/> assertion fails.
    /// </summary>
    public sealed class AssertException : Exception
    {
        /// <summary>
        /// Initialize a new assertion failure exception.
        /// </summary>
        /// <param name="message">Failure message.</param>
        public AssertException(string message) : base(message)
        {
        }
    }
}
