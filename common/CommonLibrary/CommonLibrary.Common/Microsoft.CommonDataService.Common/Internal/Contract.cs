//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace CommonLibrary.CommonDataService.Common.Internal
{
    /// <summary>
    /// Execution contract assertions with two sets of contract assertions:
    /// 
    ///  - Assert*()
    ///  - Check*()
    /// 
    /// Assert*() methods are meant for argument validation of internal API calls. These methods are 
    /// annotated with the [Conditional("DEBUG")] attribute which means they are only called into if 
    /// the consuming code is being compiled as DEBUG.
    /// 
    /// Check*() methods are executed under all scenarios, even in retail builds and are meant for 
    /// external facing library functions.
    /// 
    /// Failing either Check or Assert is considered an application (programming) error so these 
    /// methods should never be used to validate end-user input.
    /// </summary>
    /// <remarks>
    /// These APIs support the SDK infrastructure and are not intended to be used
    /// directly from your code. The APIs may change or be removed in future releases.
    /// </remarks>
    public static class Contract
    {
        /// <summary>
        /// Action to call when an Assert fails
        /// </summary>
        private static volatile Action<string> _failAction;

        /// <summary>
        /// General purpose assert.
        /// </summary>
        /// <param name="f">The value/condition to test.</param>
        /// <param name="msg">The assertion failure message.</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void Assert(bool f, string msg)
        {
            if (!f)
                DbgFail(msg);
        }

        /// <summary>
        /// General purpose assert.
        /// </summary>
        /// <param name="f">The value/condition to test.</param>
        /// <param name="msg">The assertion failure message.</param>
        /// <param name="args">The arguments.</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void Assert(bool f, string msg, params object[] args)
        {
            if (!f)
                DbgFail(GetMsg(msg, args));
        }

        /// <summary>
        /// Used to validate that a string is non-null and non-empty.
        /// </summary>
        /// <param name="s">The string to test.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertNonEmpty(string s, string paramName)
        {
            if (string.IsNullOrEmpty(s))
                DbgFailEmpty(paramName);
        }

        /// <summary>
        /// Used to validate that a string is non-null and non-empty.
        /// </summary>
        /// <param name="s">The string to test.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        /// <param name="msg">The assertion failure message.</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertNonEmpty(string s, string paramName, string msg)
        {
            if (string.IsNullOrEmpty(s))
                DbgFailEmpty(paramName, msg);
        }

        /// <summary>
        /// Used to validate that a Guid is non-empty.
        /// </summary>
        /// <param name="guid">The Guid to test.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertNonEmpty(Guid guid, string paramName)
        {
            if (guid == Guid.Empty)
                DbgFailEmpty(paramName);
        }

        /// <summary>
        /// Used to validate that a Guid is non-empty.
        /// </summary>
        /// <param name="guid">The Guid to test.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        /// <param name="msg">The assertion failure message.</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertNonEmpty(Guid guid, string paramName, string msg)
        {
            if (guid == Guid.Empty)
                DbgFailEmpty(paramName, msg);
        }

        /// <summary>
        /// Used to validate that the value is non-null.
        /// </summary>
        /// <typeparam name="T">The type of the value to test.</typeparam>
        /// <param name="val">The value to test.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertValue<T>(T val, string paramName) where T : class
        {
            if (object.ReferenceEquals(val, null))
                DbgFailValue(paramName);
        }

        /// <summary>
        /// Used to validate that the value is valid according to its implementation of <see cref="ICheckable"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value to test.</typeparam>
        /// <param name="val">The value to test.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        /// <remarks>
        /// It is important that this method is generic to avoid boxing structs. If <paramref name="val"/> was simply defined as 
        /// <see cref="ICheckable"/> then a struct implementing the interface would be boxed when it was passed to this method.
        /// </remarks>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertValid<T>(T val, string paramName) where T : ICheckable
        {
            if (val == null || !val.IsValid)
                DbgFailValue(paramName);
        }

        /// <summary>
        /// Used to validate the value of a generic parameter T without knowing if it is value type or reference type
        /// </summary>
        /// <remarks>
        /// If T is a reference type, it checks if the input value is null
        /// If T implements ICheckable, it verifies if the input value is valid
        /// otherwise, it is a no-op
        /// </remarks>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertValueGeneric<T>(T val, string paramName)
        {
            if (!typeof(T).GetTypeInfo().IsValueType && object.ReferenceEquals(val, null))
                DbgFailValue(paramName);

            var checkableVal = val as ICheckable;
            if (checkableVal != null)
                AssertValid(checkableVal, paramName);
        }

        /// <summary>
        /// Used to validate that the <see cref="IntPtr"/> value is non-null pointer.
        /// </summary>
        /// <param name="val">The <see cref="IntPtr"/> value to test.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertValue(IntPtr val, string paramName)
        {
            if (val == IntPtr.Zero)
                DbgFailValue(paramName);
        }

        /// <summary>
        /// Used to validate that the value is non-null.
        /// </summary>
        /// <typeparam name="T">The type of the value to test.</typeparam>
        /// <param name="val">The value to test.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        /// <param name="msg">The assertion failure message.</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertValue<T>(T val, string paramName, string msg) where T : class
        {
            if (object.ReferenceEquals(val, null))
                DbgFailValue(paramName, msg);
        }

        /// <summary>
        /// Used to validate that all the strings in a collection are non-null and non-empty.
        /// </summary>
        /// <param name="args">The collection to test. May be null or empty.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        [Conditional("DEBUG")]
        public static void AssertAllNonEmpty(IList<string> args, string paramName)
        {
            for (int i = 0; i < Size(args); i++)
            {
                if (string.IsNullOrEmpty(args[i]))
                    DbgFailEmpty(paramName);
            }
        }

        /// <summary>
        /// Used to validate that all the strings in a collection are non-null and non-empty.
        /// </summary>
        /// <param name="args">The collection to test. May be null or empty.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        /// <param name="msg">The assertion failure message.</param>
        [Conditional("DEBUG")]
        public static void AssertAllNonEmpty(IList<string> args, string paramName, string msg)
        {
            for (int i = 0; i < Size(args); i++)
            {
                if (string.IsNullOrEmpty(args[i]))
                    DbgFailEmpty(paramName, msg);
            }
        }

        /// <summary>
        /// Used to validate that a dictionary is non-null and non-empty.
        /// </summary>
        /// <typeparam name="K">The dictionary key type.</typeparam>
        /// <typeparam name="T">The dictionary value type.</typeparam>
        /// <param name="args">The dictionary being tested.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertNonEmptyReadOnly<K, T>(IReadOnlyDictionary<K, T> args, string paramName)
        {
            if (Size(args) == 0)
                DbgFailEmpty(paramName);
        }

        /// <summary>
        /// Used to validate that a dictionary is non-null and non-empty.
        /// </summary>
        /// <typeparam name="K">The dictionary key type.</typeparam>
        /// <typeparam name="T">The dictionary value type.</typeparam>
        /// <param name="args">The dictionary being tested.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertNonEmpty<K, T>(IDictionary<K, T> args, string paramName)
        {
            if (Size(args) == 0)
                DbgFailEmpty(paramName);
        }

        /// <summary>
        /// Used to validate that a collection is non-null and non-empty.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="args">The collection being tested.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertNonEmpty<T>(ICollection<T> args, string paramName)
        {
            if (Size(args) == 0)
                DbgFailEmpty(paramName);
        }

        /// <summary>
        /// Used to validate that a collection is non-null and non-empty.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="args">The collection being tested.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        /// <param name="msg">The assertion failure message.</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertNonEmptyReadOnly<T>(IReadOnlyCollection<T> args, string paramName, string msg)
        {
            if (Size(args) == 0)
                DbgFailEmpty(paramName, msg);
        }

        /// <summary>
        /// Used to validate that a collection is non-null and non-empty.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="args">The collection being tested.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertNonEmptyReadOnly<T>(IReadOnlyCollection<T> args, string paramName)
        {
            if (Size(args) == 0)
                DbgFailEmpty(paramName);
        }

        /// <summary>
        /// Used to validate that a collection is non-null and non-empty.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="args">The collection being tested.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        /// <param name="msg">The assertion failure message.</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertNonEmpty<T>(ICollection<T> args, string paramName, string msg)
        {
            if (Size(args) == 0)
                DbgFailEmpty(paramName, msg);
        }

        /// <summary>
        /// Used to validate that a collection is non-null, non-empty and that none of the child items are null.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="args">The collection being tested.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertNonEmptyAndAllValues<T>(IList<T> args, string paramName) where T : class
        {
            if (Size(args) == 0)
                DbgFailEmpty(paramName);
            else
                AssertAllValues(args, paramName);
        }

        /// <summary>
        /// Used to validate that a collection is non-null, non-empty and that none of the child items are null.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="args">The collection being tested.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        /// <param name="msg">The assertion failure message.</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertNonEmptyAndAllValues<T>(IList<T> args, string paramName, string msg) where T : class
        {
            if (Size(args) == 0)
                DbgFailEmpty(paramName, msg);
            else
                AssertAllValues(args, paramName, msg);
        }

        /// <summary>
        /// Used to validate that a collection contains a specific item.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="items">The collection to find the item in.</param>
        /// <param name="item">The item that should be located.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertContainsOrNull<T>(IEnumerable<T> items, T item, string paramName)
        {
            if (item != null && !items.Contains(item))
                DbgFailContains(paramName);
        }

        /// <summary>
        /// Used to validate that a collection contains a specific item.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="items">The collection to find the item in.</param>
        /// <param name="item">The item that should be located.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        /// <param name="msg">The assertion failure message.</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertContainsOrNull<T>(IList<T> items, T item, string paramName, string msg)
        {
            if (item != null && !items.Contains(item))
                DbgFailContains(paramName, msg);
        }

        /// <summary>
        /// Used to validate that all the items in a collection are non-null.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="args">The collection being tested. May be null or empty.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        [Conditional("DEBUG")]
        public static void AssertAllValues<T>(IList<T> args, string paramName) where T : class
        {
            for (int i = 0; i < Size(args); i++)
            {
                if (object.ReferenceEquals(args[i], null))
                    DbgFailAllValue(paramName);
            }
        }

        /// <summary>
        /// Used to validate that all the items in a nested collection are non-null.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="args">The collection being tested. May be null or empty.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        [Conditional("DEBUG")]
        public static void AssertAllValuesNested<T>(IList<IList<T>> args, string paramName) where T : class
        {
            for (int i = 0; i < Size(args); i++)
            {
                var inner = args[i];
                if (object.ReferenceEquals(inner, null))
                    DbgFailAllValue(paramName);

                for (int j = 0; j < inner.Count; j++)
                {
                    if (object.ReferenceEquals(inner[j], null))
                        DbgFailAllValue(paramName);
                }
            }
        }

        /// <summary>
        /// Used to validate that all the items in a collection are non-null.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="args">The collection being tested. May be null or empty.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        /// <param name="msg">The assertion failure message.</param>
        [Conditional("DEBUG")]
        public static void AssertAllValues<T>(IList<T> args, string paramName, string msg) where T : class
        {
            for (int i = 0; i < Size(args); i++)
            {
                if (object.ReferenceEquals(args[i], null))
                    DbgFailAllValue(paramName, msg);
            }
        }

        /// <summary>
        /// Used to validate that all values in a dictionary are non-null.
        /// </summary>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TValue">The value type.</typeparam>
        /// <param name="args">The dictionary being tested. May be null or empty.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        [Conditional("DEBUG")]
        public static void AssertAllValues<TKey, TValue>(IDictionary<TKey, TValue> args, string paramName) where TValue : class
        {
            if (!object.ReferenceEquals(args, null))
            {
                foreach (var arg in args.Values)
                {
                    if (object.ReferenceEquals(arg, null))
                        DbgFailAllValue(paramName);
                }
            }
        }

        /// <summary>
        /// Used to validate that all values in a dictionary are non-null.
        /// </summary>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TValue">The value type.</typeparam>
        /// <param name="args">The dictionary being tested. May be null or empty.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        /// <param name="msg">The assertion failure message.</param>
        [Conditional("DEBUG")]
        public static void AssertAllValues<TKey, TValue>(IDictionary<TKey, TValue> args, string paramName, string msg) where TValue : class
        {
            if (!object.ReferenceEquals(args, null))
            {
                foreach (var arg in args.Values)
                {
                    if (object.ReferenceEquals(arg, null))
                        DbgFailAllValue(paramName, msg);
                }
            }
        }

        /// <summary>
        /// Used to validate that all items in a collection are "valid" according to
        /// their implementation of ICheckable.IsValid.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="args">The collection being tested. May be null or empty.</param>
        /// <param name="msg">The error message</param>
        [Conditional("DEBUG")]
        public static void AssertAll<T>(IList<T> args, string msg) where T : struct, ICheckable
        {
            for (int i = 0; i < Size(args); i++)
            {
                if (!args[i].IsValid)
                    DbgFail(msg);
            }
        }

        /// <summary>
        /// Used to validate that all items in a collection satisfy a condition
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="args">The collection being tested. May be null or empty.</param>
        /// <param name="condition">The condition</param>
        /// <param name="msg">The error message</param>
        [Conditional("DEBUG")]
        public static void AssertAll<T>(IList<T> args, Func<T, bool> condition, string msg)
        {
            for (int i = 0; i < Size(args); i++)
            {
                if (!condition(args[i]))
                    DbgFail(msg);
            }
        }

        /// <summary>
        /// General purpose assert. Assert if all two consecutive items in a collection always satisfy a condition
        /// </summary>
        /// <param name="args">The item collection to test.</param>
        /// <param name="condition">The test condition</param>
        /// <param name="msg">The assertion failure message.</param>
        [Conditional("DEBUG")]
        public static void AssertAll<T>(IList<T> args, Func<T, T, bool> condition, string msg)
        {
            for (int i = 0; i < Size(args) - 1; i++)
            {
                if (!condition(args[i], args[i + 1]))
                    DbgFail(msg);
            }
        }

        /// <summary>
        /// Used to validate that all values in a dictionary are "valid" according to
        /// their implementation of ICheckable.IsValid.
        /// </summary>
        /// <typeparam name="K">The key type.</typeparam>
        /// <typeparam name="V">The value type.</typeparam>
        /// <param name="args">The dictionary being tested. May be null or empty.</param>
        /// <param name="msg">The error message</param>
        [Conditional("DEBUG")]
        public static void AssertAll<K, V>(IDictionary<K, V> args, string msg) where V : struct, ICheckable
        {
            if (!object.ReferenceEquals(args, null))
            {
                foreach (var arg in args.Values)
                {
                    if (!arg.IsValid)
                        DbgFail(msg);
                }
            }
        }

        /// <summary>
        /// Used to validate that each value in a list is of a given type or inherits/implements that type
        /// </summary>
        /// <typeparam name="S">The known base type for the values in the list.</typeparam>
        /// <typeparam name="T">The subtype of S for which to test the values in the list.</typeparam>
        /// <param name="values">The list of values to test.</param>
        /// <param name="paramName">parameter name.</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertAllAreOfType<S, T>(IList<S> values, string paramName)
            where S : class
            where T : S
        {
            AssertValue(values, paramName);
            foreach (var val in values)
            {
                AssertValue(val, paramName);
                if (!(val is T))
                    DbgFailType(paramName);
            }
        }

        /// <summary>
        /// Used to validate that the value of a parameter is of a given type or inherits/implements that type
        /// </summary>
        /// <typeparam name="T">The type of the value to test.</typeparam>
        /// <param name="val">The value to test.</param>
        /// <param name="paramName">parameter name</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertIsOfType<T>(object val, string paramName) where T : class
        {
            AssertValue(val, paramName);
            if (!(val is T))
                DbgFailType(paramName);
        }

        /// <summary>
        /// Used to validate that the value of a parameter is of a given type or inherits/implements that type
        /// </summary>
        /// <typeparam name="T">The type of the value to test.</typeparam>
        /// <param name="val">The value to test.</param>
        /// <param name="paramName">parameter name</param>
        /// /// <param name="msg">The assertion failure message.</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertIsOfType<T>(object val, string paramName, string msg) where T : class
        {
            AssertValue(val, paramName, msg);
            if (!(val is T))
                DbgFailType(paramName, msg);
        }

        /// <summary>
        /// Used to validate that the value of a parameter is of a given type or inherits/implements that type, or null
        /// </summary>
        /// <typeparam name="T">The type of the value to test.</typeparam>
        /// <param name="val">The value to test.</param>
        /// <param name="paramName">parameter name</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertIsOfTypeOrNull<T>(object val, string paramName) where T : class
        {
            if (!ReferenceEquals(val, null))
                AssertIsOfType<T>(val, paramName);
        }

        /// <summary>
        /// Used to validate that the value of a parameter is of an exact type
        /// </summary>
        /// <typeparam name="T">The type of the value to test.</typeparam>
        /// <param name="val">The value to test.</param>
        /// <param name="paramName">parameter name</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertIsExactType<T>(object val, string paramName) where T : class
        {
            AssertValue(val, paramName);
            if (val.GetType() != typeof(T))
                DbgFailType(paramName);
        }

        /// <summary>
        /// Used to validate that the value of a parameter is of an exact type
        /// </summary>
        /// <typeparam name="T">The type of the value to test.</typeparam>
        /// <param name="val">The value to test.</param>
        /// <param name="paramName">parameter name</param>
        /// <param name="msg">The assertion failure message.</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertIsExactType<T>(object val, string paramName, string msg) where T : class
        {
            AssertValue(val, paramName, msg);
            if (val.GetType() != typeof(T))
                DbgFailType(paramName, msg);
        }

        /// <summary>
        /// Used to validate that the value of a parameter is of an exact type, or null
        /// </summary>
        /// <typeparam name="T">The type of the value to test.</typeparam>
        /// <param name="val">The value to test.</param>
        /// <param name="paramName">parameter name</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertIsExactTypeOrNull<T>(object val, string paramName) where T : class
        {
            if (!ReferenceEquals(val, null))
                AssertIsExactType<T>(val, paramName);
        }

        /// <summary>
        /// Used to validate that the value is within a range starting from 0 but less than <paramref name="exclusiveUpperBound"/>
        /// </summary>
        /// <param name="value">The value to test</param>
        /// <param name="exclusiveUpperBound">The exclusive upper bound of the range</param>
        /// <param name="paramName">parameter name</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertValueInRange(int value, int exclusiveUpperBound, string paramName)
        {
            if (value < 0 || value >= exclusiveUpperBound)
                DbgFailValueInRange(paramName);
        }

        /// <summary>
        /// Used to validate that the value is within a range starting from <paramref name="inclusiveLowerBound"/> inclusive but less than <paramref name="exclusiveUpperBound"/>
        /// </summary>
        /// <param name="value">The value to test</param>
        /// <param name="inclusiveLowerBound">The inclusive lower bound of the range</param>
        /// <param name="exclusiveUpperBound">The exclusive upper bound of the range</param>
        /// <param name="paramName">parameter name</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertValueInRange(int value, int inclusiveLowerBound, int exclusiveUpperBound, string paramName)
        {
            if (value < inclusiveLowerBound || value >= exclusiveUpperBound)
                DbgFailValueInRange(paramName);
        }

        /// <summary>
        /// Used to validate that the value is within a range starting from 0 but less than <paramref name="exclusiveUpperBound"/>
        /// </summary>
        /// <param name="value">The value to test</param>
        /// <param name="exclusiveUpperBound">The exclusive upper bound of the range</param>
        /// <param name="paramName">parameter name</param>
        /// <param name="msg">The assertion failure message.</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertValueInRange(int value, int exclusiveUpperBound, string paramName, string msg)
        {
            if (value < 0 || value >= exclusiveUpperBound)
                DbgFailValueInRange(paramName, msg);
        }

        /// <summary>
        /// Used to validate that the value is within a range starting from <paramref name="inclusiveLowerBound"/> inclusive but less than <paramref name="exclusiveUpperBound"/>
        /// </summary>
        /// <param name="value">The value to test</param>
        /// <param name="inclusiveLowerBound">The inclusive lower bound of the range</param>
        /// <param name="exclusiveUpperBound">The exclusive upper bound of the range</param>
        /// <param name="paramName">parameter name</param>
        /// <param name="msg">The assertion failure message.</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertValueInRange(int value, int inclusiveLowerBound, int exclusiveUpperBound, string paramName, string msg)
        {
            if (value < inclusiveLowerBound || value >= exclusiveUpperBound)
                DbgFailValueInRange(paramName, msg);
        }

        /// <summary>
        /// Used to validate that a double value is within the allowable ranges for weights.
        /// Value should be greater than 0 exclusive but less than one inclusive.
        /// </summary>
        /// <param name="value">The value to test</param>
        /// <param name="paramName">parameter name</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertWeightValue(double value, string paramName)
        {
            if (value < 0 || value > 1)
                DbgFailValueInRange(paramName, "Value is not in allowable range for weights.");
        }

        /// <summary>
        /// Used to fail an assertion for an unrecognized switch value
        /// </summary>
        /// <param name="value">The invalid value.</param>
        /// <param name="paramName">parameter name</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertInvalidSwitchValue<T>(T value, string paramName)
            where T : struct
        {
            Assert(false, "Invalid switch/case: Value '{0}' of type '{1}'", value.ToString(), typeof(T).FullName);
        }

        /// <summary>
        /// Currently compiles to a no-op but is very useful to document that the value
        /// can be null (or not) and must be checked appropriately before all uses.
        /// </summary>
        /// <typeparam name="T">The type of the value being tested.</typeparam>
        /// <param name="val">The value being tested.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        [Conditional("INVARIANT_CHECKS")]
        public static void AssertValueOrNull<T>(T val, string paramName)
        {
        }

        /// <summary>
        /// Currently compiles to a no-op but is very useful to document that the value
        /// can be null (or not) and must be checked appropriately before all uses.
        /// </summary>
        /// <typeparam name="T">The type of the value being tested.</typeparam>
        /// <param name="val">The value being tested.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        /// <param name="msg">The assertion failure message.</param>
        [Conditional("INVARIANT_CHECKS")]
        public static void AssertValueOrNull<T>(T val, string paramName, string msg) where T : class
        {
        }

        /// <summary>
        /// General purpose check. Throws InvalidOperationException on failure.
        /// </summary>
        /// <param name="f">The value/condition to check.</param>
        /// <param name="msg">The exception message.</param>
        [DebuggerStepThrough]
        public static void Check(bool f, string msg)
        {
            if (!f)
                throw Except(msg);
        }

        /// <summary>
        /// Used to validate that a string is non-null and non-empty.
        /// Throws ArgumentException on failure.
        /// </summary>
        /// <param name="s">The string to check.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        [DebuggerStepThrough]
        public static void CheckNonEmpty(string s, string paramName)
        {
            if (string.IsNullOrEmpty(s))
                throw ExceptEmpty(paramName);
        }

        /// <summary>
        /// Used to validate that a string is non-null and non-empty.
        /// Throws ArgumentException on failure.
        /// </summary>
        /// <param name="s">The string to check.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        /// <param name="msg">The exception message.</param>
        [DebuggerStepThrough]
        public static void CheckNonEmpty(string s, string paramName, string msg)
        {
            if (string.IsNullOrEmpty(s))
                throw ExceptEmpty(paramName, msg);
        }

        /// <summary>
        /// Used to validate that a string is non-null, non-empty, and non-whitespace.
        /// Throws ArgumentException on failure.
        /// </summary>
        /// <param name="s">The string to check.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        [DebuggerStepThrough]
        public static void CheckNonWhitespace(string s, string paramName)
        {
            if (string.IsNullOrWhiteSpace(s))
                throw ExceptEmpty(paramName);
        }

        /// <summary>
        /// Used to validate that a string is non-null, non-empty, and non-whitespace.
        /// Throws ArgumentException on failure.
        /// </summary>
        /// <param name="s">The string to check.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        /// <param name="msg">The exception message.</param>
        [DebuggerStepThrough]
        public static void CheckNonWhitespace(string s, string paramName, string msg)
        {
            if (string.IsNullOrWhiteSpace(s))
                throw ExceptEmpty(paramName, msg);
        }

        /// <summary>
        /// Used to validate numeric ranges or relationships (like that an array index is within
        /// the bounds of the array). It is sometimes valid to check multiple related ranges in the
        /// same CheckRange as long as it is for the same parameter since the CheckRange method
        /// requires the name of the parameter being tested.
        /// Throws ArgumentOutOfRangeException on failure.
        /// </summary>
        /// <param name="f">The range condition to check.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        [DebuggerStepThrough]
        public static void CheckRange(bool f, string paramName)
        {
            if (!f)
                throw ExceptRange(paramName);
        }

        /// <summary>
        /// Used to validate numeric ranges or relationships (like that an array index is within
        /// the bounds of the array). It is sometimes valid to check multiple related ranges in the
        /// same CheckRange as long as it is for the same parameter since the CheckRange method
        /// requires the name of the parameter being tested.
        /// Throws ArgumentOutOfRangeException on failure.
        /// </summary>
        /// <param name="f">The range condition to check.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        /// <param name="msg">The exception message.</param>
        [DebuggerStepThrough]
        public static void CheckRange(bool f, string paramName, string msg)
        {
            if (!f)
                throw ExceptRange(paramName, msg);
        }

        /// <summary>
        /// Used for general argument validation not covered by CheckValue or CheckRange.
        /// Throws ArgumentException on failure.
        /// </summary>
        /// <param name="f">The value/condition to check.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        [DebuggerStepThrough]
        public static void CheckParam(bool f, string paramName)
        {
            if (!f)
                throw ExceptParam(paramName);
        }

        /// <summary>
        /// Used for general argument validation not covered by CheckValue or CheckRange.
        /// Throws ArgumentException on failure.
        /// </summary>
        /// <param name="f">The value/condition to check.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        /// <param name="msg">The exception message.</param>
        [DebuggerStepThrough]
        public static void CheckParam(bool f, string paramName, string msg)
        {
            if (!f)
                throw ExceptParam(paramName, msg);
        }

        /// <summary>
        /// Used to validate that the value is non-null.
        /// Throws ArgumentNullException on failure.
        /// </summary>
        /// <typeparam name="T">The type of the value being tested.</typeparam>
        /// <param name="val">The value being tested.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        [DebuggerStepThrough]
        public static void CheckValue<T>(T val, string paramName) where T : class
        {
            if (object.ReferenceEquals(val, null))
                throw ExceptValue(paramName);
        }

        /// <summary>
        /// Used to validate that the value is non-null.
        /// Throws ArgumentNullException on failure.
        /// </summary>
        /// <typeparam name="T">The type of the value being tested.</typeparam>
        /// <param name="val">The value being tested.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        /// <param name="msg">The exception message.</param>
        [DebuggerStepThrough]
        public static void CheckValue<T>(T val, string paramName, string msg) where T : class
        {
            if (object.ReferenceEquals(val, null))
                throw ExceptValue(paramName, msg);
        }

        /// <summary>
        /// Used to validate that all the strings in a specified collection are
        /// non-null and non-empty.
        /// Throws ArgumentException on failure.
        /// </summary>
        /// <param name="args">The collection being tested. May be null or empty.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        public static void CheckAllNonEmpty(IList<string> args, string paramName)
        {
            for (int i = 0; i < Size(args); i++)
            {
                if (string.IsNullOrEmpty(args[i]))
                    throw ExceptEmpty(paramName);
            }
        }

        /// <summary>
        /// Used to validate that a collection is non-null and non-empty.
        /// Throws ArgumentException on failure.
        /// </summary>
        /// <typeparam name="T">The type of the items being tested.</typeparam>
        /// <param name="args">The collection being tested.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        [DebuggerStepThrough]
        public static void CheckNonEmpty<T>(ICollection<T> args, string paramName)
        {
            if (Size(args) == 0)
                throw ExceptEmpty(paramName);
        }

        /// <summary>
        /// Used to validate that a dictionary is non-null and non-empty.
        /// Throws ArgumentException on failure.
        /// </summary>
        /// <typeparam name="K">The type of dictionary entry key being tested.</typeparam>
        /// <typeparam name="T">The type of dictionary entry value being tested.</typeparam>
        /// <param name="args">The dictionary being tested.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        [DebuggerStepThrough]
        public static void CheckNonEmpty<K, T>(IDictionary<K, T> args, string paramName)
        {
            if (Size(args) == 0)
                throw ExceptEmpty(paramName);
        }

        /// <summary>
        /// Used to validate that a readonly dictionary is non-null and non-empty.
        /// Throws ArgumentException on failure.
        /// </summary>
        /// <typeparam name="K">The type of dictionary entry key being tested.</typeparam>
        /// <typeparam name="T">The type of dictionary entry value being tested.</typeparam>
        /// <param name="args">The dictionary being tested.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        [DebuggerStepThrough]
        public static void CheckNonEmpty<K, T>(IReadOnlyDictionary<K, T> args, string paramName)
        {
            if (Size(args) == 0)
                throw ExceptEmpty(paramName);
        }

        /// <summary>
        /// Used to validate that all the items in a collection are non-null.
        /// Throws ArgumentNullException on failure.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="args">The collection being tested. May be null or empty.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        public static void CheckAllValues<T>(IList<T> args, string paramName) where T : class
        {
            for (int i = 0; i < Size(args); i++)
            {
                if (object.ReferenceEquals(args[i], null))
                    throw ExceptParam(paramName);
            }
        }

        /// <summary>
        /// Used to validate that all the items in a collection are non-null.
        /// Throws ArgumentNullException on failure.
        /// </summary>
        /// <param name="args">The collection being tested. May be null or empty.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        public static void CheckAllValues<TKey, TValue>(IDictionary<TKey, TValue> args, string paramName) where TValue : class
        {
            if (!object.ReferenceEquals(args, null))
            {
                foreach (var arg in args.Values)
                {
                    if (object.ReferenceEquals(arg, null))
                        throw ExceptParam(paramName);
                }
            }
        }

        /// <summary>
        /// Used to validate that all the items in a collection are "valid", according to
        /// their type-specific implementation of ICheckable.IsValid.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="args">The collection being tested. May be null or empty.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        public static void CheckAll<T>(IList<T> args, string paramName) where T : struct, ICheckable
        {
            for (int i = 0; i < Size(args); i++)
            {
                if (!args[i].IsValid)
                    throw ExceptValid(paramName);
            }
        }

        /// <summary>
        /// Used to validate that the value of a parameter is of a given type or inherits/implements that type
        /// </summary>
        /// <typeparam name="T">The type of the value to test.</typeparam>
        /// <param name="val">The value to test.</param>
        /// <param name="paramName">parameter name</param>
        public static void CheckIsOfType<T>(object val, string paramName) where T : class
        {
            CheckValue(val, paramName);
            if (!(val is T))
                throw ExceptParam(paramName);
        }

        /// <summary>
        /// Used to validate that the value of a parameter is of a given type or inherits/implements that type
        /// </summary>
        /// <typeparam name="T">The type of the value to test.</typeparam>
        /// <param name="val">The value to test.</param>
        /// <param name="paramName">parameter name</param>
        /// <param name="msg">The assertion failure message.</param>
        public static void CheckIsOfType<T>(object val, string paramName, string msg) where T : class
        {
            CheckValue(val, paramName, msg);
            if (!(val is T))
                throw ExceptParam(paramName, msg);
        }

        /// <summary>
        /// Used to validate that the value of a parameter is not of a given type or inherits/implements that type
        /// </summary>
        /// <typeparam name="T">The type of the value to test.</typeparam>
        /// <param name="val">The value to test.</param>
        /// <param name="paramName">parameter name</param>
        public static void CheckIsNotOfType<T>(object val, string paramName) where T : class
        {
            CheckValue(val, paramName);
            if (val is T)
                throw ExceptParam(paramName);
        }

        /// <summary>
        /// Used to validate that the value of a parameter is not of a given type or inherits/implements that type
        /// </summary>
        /// <typeparam name="T">The type of the value to test.</typeparam>
        /// <param name="val">The value to test.</param>
        /// <param name="paramName">parameter name</param>
        /// <param name="msg">The assertion failure message.</param>
        public static void CheckIsNotOfType<T>(object val, string paramName, string msg) where T : class
        {
            CheckValue(val, paramName, msg);
            if (val is T)
                throw ExceptParam(paramName, msg);
        }

        /// <summary>
        /// Used to validate that the value of a parameter is of a given type or inherits/implements that type, or null
        /// </summary>
        /// <typeparam name="T">The type of the value to test.</typeparam>
        /// <param name="val">The value to test.</param>
        /// <param name="paramName">parameter name</param>
        public static void CheckIsOfTypeOrNull<T>(object val, string paramName) where T : class
        {
            if (!ReferenceEquals(val, null))
                CheckIsOfType<T>(val, paramName);
        }

        /// <summary>
        /// Used to validate that the value of a parameter is of a given type or inherits/implements that type, or null
        /// </summary>
        /// <typeparam name="T">The type of the value to test.</typeparam>
        /// <param name="val">The value to test.</param>
        /// <param name="paramName">parameter name</param>
        /// <param name="msg">The assertion failure message.</param>
        public static void CheckIsOfTypeOrNull<T>(object val, string paramName, string msg) where T : class
        {
            if (!ReferenceEquals(val, null))
                CheckIsOfType<T>(val, paramName, msg);
        }

        /// <summary>
        /// Used to validate that the value of a parameter is not of a given type or inherits/implements that type, or null
        /// </summary>
        /// <typeparam name="T">The type of the value to test.</typeparam>
        /// <param name="val">The value to test.</param>
        /// <param name="paramName">parameter name</param>
        public static void CheckIsNotOfTypeOrNull<T>(object val, string paramName) where T : class
        {
            if (!ReferenceEquals(val, null))
                CheckIsNotOfType<T>(val, paramName);
        }

        /// <summary>
        /// Used to validate that the value of a parameter is not of a given type or inherits/implements that type, or null
        /// </summary>
        /// <typeparam name="T">The type of the value to test.</typeparam>
        /// <param name="val">The value to test.</param>
        /// <param name="paramName">parameter name</param>
        /// <param name="msg">The assertion failure message.</param>
        public static void CheckIsNotOfTypeOrNull<T>(object val, string paramName, string msg) where T : class
        {
            if (!ReferenceEquals(val, null))
                CheckIsNotOfType<T>(val, paramName, msg);
        }

        /// <summary>
        /// Used to validate that the value of a parameter is of an exact type
        /// </summary>
        /// <typeparam name="T">The type of the value to test.</typeparam>
        /// <param name="val">The value to test.</param>
        /// <param name="paramName">parameter name</param>
        public static void CheckIsExactType<T>(object val, string paramName) where T : class
        {
            CheckValue(val, paramName);
            if (val.GetType() != typeof(T))
                throw ExceptParam(paramName);
        }

        /// <summary>
        /// Used to validate that the value of a parameter is of an exact type
        /// </summary>
        /// <typeparam name="T">The type of the value to test.</typeparam>
        /// <param name="val">The value to test.</param>
        /// <param name="paramName">parameter name</param>
        /// <param name="msg">The assertion failure message.</param>
        public static void CheckIsExactType<T>(object val, string paramName, string msg) where T : class
        {
            CheckValue(val, paramName, msg);
            if (val.GetType() != typeof(T))
                throw ExceptParam(paramName, msg);
        }

        /// <summary>
        /// Used to validate that the value of a parameter is of an exact type, or null
        /// </summary>
        /// <typeparam name="T">The type of the value to test.</typeparam>
        /// <param name="val">The value to test.</param>
        /// <param name="paramName">parameter name</param>
        public static void CheckIsExactTypeOrNull<T>(object val, string paramName) where T : class
        {
            if (!ReferenceEquals(val, null))
                CheckIsExactType<T>(val, paramName);
        }

        /// <summary>
        /// Currently compiles to a no-op but is very useful to document that the value
        /// can be null (or not) and should be checked appropriately before all uses.
        /// Doesn't throw.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="val">The value being tested.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        [Conditional("INVARIANT_CHECKS")]
        public static void CheckValueOrNull<T>(T val, string paramName) where T : class
        {
        }

        /// <summary>
        /// Currently compiles to a no-op but is very useful to document that the value
        /// can be null (or not) and should be checked appropriately before all uses.
        /// Doesn't throw.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="val">The value being tested.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        /// <param name="msg">The exception message.</param>
        [Conditional("INVARIANT_CHECKS")]
        public static void CheckValueOrNull<T>(T val, string paramName, string msg) where T : class
        {
        }

        /// <summary>
        /// Fail immediately in debug mode.
        /// </summary>
        /// <param name="msg">The assertion failure message.</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void DebugFail(string msg)
        {
            DbgFail(msg);
        }

        /// <summary>
        /// Fail immediately in debug mode.
        /// </summary>
        /// <param name="msg">The assertion failure message.</param>
        /// <param name="args">The arguments.</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void DebugFail(string msg, params object[] args)
        {
            DbgFail(GetMsg(msg, args));
        }

        /// <summary>
        /// Fail immediately in debug mode and trace in all builds.
        /// </summary>
        /// <param name="tracer">Tracer to use if possible.</param>
        /// <param name="msg">The assertion failure message.</param>
        [DebuggerStepThrough]
        public static void TraceFail(ITracer tracer, string msg)
        {
            DbgFail(msg);
            if (tracer != null)
                tracer.TraceError(msg);
        }

        /// <summary>
        /// Fail immediately in debug mode and trace in all builds.
        /// </summary>
        /// <param name="tracer">Tracer to use if possible.</param>
        /// <param name="msg">The assertion failure message.</param>
        /// <param name="args">The arguments.</param>
        [DebuggerStepThrough]
        public static void TraceFail(ITracer tracer, string msg, params object[] args)
        {
            DbgFail(GetMsg(msg, args));
            if (tracer != null)
                tracer.TraceError(GetMsg(msg, args));
        }

        public static Exception Except(string msg)
        {
            return new InvalidOperationException(msg);
        }

        public static Exception Except(string msg, params object[] args)
        {
            return new InvalidOperationException(GetMsg(msg, args));
        }

        public static Exception Except(string msg, Exception innerException)
        {
            return new InvalidOperationException(msg, innerException);
        }

        public static Exception Except(string msg, Exception innerException, params object[] args)
        {
            return new InvalidOperationException(GetMsg(msg, args), innerException);
        }

        public static Exception ExceptRange(string paramName)
        {
            return new ArgumentOutOfRangeException(paramName);
        }

        public static Exception ExceptRange(string paramName, string msg)
        {
            return new ArgumentOutOfRangeException(paramName, msg);
        }

        public static Exception ExceptParam(string paramName)
        {
            return new ArgumentException(paramName);
        }

        public static Exception ExceptParam(string paramName, string msg)
        {
            return new ArgumentException(msg, paramName);
        }

        public static Exception ExceptParam<T>(string paramName, string msg, params object[] args)
        {
            return new ArgumentException(GetMsg(msg, args), paramName);
        }

        public static Exception ExceptValue(string paramName)
        {
            return new ArgumentNullException(paramName);
        }

        public static Exception ExceptValue(string paramName, string msg)
        {
            return new ArgumentNullException(paramName, msg);
        }

        public static Exception ExceptEmpty(string paramName)
        {
            return new ArgumentException(paramName);
        }

        public static Exception ExceptEmpty(string paramName, string msg)
        {
            return new ArgumentException(msg, paramName);
        }

        public static Exception ExceptValid(string paramName)
        {
            return new ArgumentException(paramName);
        }

        public static Exception ExceptValid(string paramName, string msg)
        {
            return new ArgumentException(msg, paramName);
        }

        public static Exception ExceptNotSupported(string msg)
        {
            return new NotSupportedException(msg);
        }

        /// <summary>
        /// Delegate called when an Assert fails.
        /// Set this to be called when an Assert fails.
        /// The default implementation calls Debug.Assert
        /// </summary>
        /// <returns>True when the failAction is set successfully; false otherwise.</returns>
        /// <remarks>Only the first call returns true.  Subsequent calls return false.</remarks>
        [DebuggerStepThrough]
        public static bool TrySetDebugFail(Action<string> failAction)
        {
#pragma warning disable 0420
            Interlocked.CompareExchange(ref _failAction, failAction, null);
#pragma warning restore 0420
            return _failAction == failAction;
        }

        private static string GetMsg(string msg, params object[] args)
        {
            try
            {
                msg = string.Format(CultureInfo.CurrentCulture, msg, args);
            }
            catch (FormatException ex)
            {
                Contract.DebugFail("Resource format string arg mismatch: " + ex.Message);
            }
            return msg;
        }

        private static int Size<T>(ICollection<T> list)
        {
            return list == null ? 0 : list.Count;
        }

        private static int Size<T>(IReadOnlyCollection<T> list)
        {
            return list == null ? 0 : list.Count;
        }

        private static int Size<K, T>(IDictionary<K, T> dictionary)
        {
            return dictionary == null ? 0 : dictionary.Count;
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private static void DbgFailedAssert(string msg)
        {
            Debug.Assert(false, msg);
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private static void DbgFailCore(string msg)
        {
            if (_failAction == null)
            {
                DbgFailedAssert(msg);
            }
            else
            {
                _failAction(msg);
            }
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private static void DbgFail()
        {
            DbgFailCore("Assertion Failed");
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private static void DbgFail(string msg)
        {
            DbgFailCore(msg);
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private static void DbgFailValue()
        {
            DbgFailCore("Non-null assertion failure");
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private static void DbgFailValue(string paramName)
        {
            DbgFailCore(GetMsg("Non-null assertion failure: {0}", paramName));
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private static void DbgFailValue(string paramName, string msg)
        {
            DbgFailCore(GetMsg("Non-null assertion failure: {0}: {1}", paramName, msg));
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private static void DbgFailEmpty()
        {
            DbgFailCore("Non-empty assertion failure");
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private static void DbgFailEmpty(string msg)
        {
            DbgFailCore(GetMsg("Non-empty assertion failure: {0}", msg));
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private static void DbgFailEmpty(string paramName, string msg)
        {
            DbgFailCore(GetMsg("Non-empty assertion failure: {0}: {1}", paramName, msg));
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private static void DbgFailAllValue()
        {
            DbgFailCore("All non-null assertion failure");
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private static void DbgFailAllValue(string msg)
        {
            DbgFailCore(GetMsg("All non-null assertion failure: {0}", msg));
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private static void DbgFailAllValue(string paramName, string msg)
        {
            DbgFailCore(GetMsg("All non-null assertion failure: {0}: {1}", paramName, msg));
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private static void DbgFailType(string paramName)
        {
            DbgFailCore(GetMsg("Of-type assertion failure: {0}", paramName));
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private static void DbgFailType(string paramName, string msg)
        {
            DbgFailCore(GetMsg("Of-type assertion failure: {0}: {1}", paramName, msg));
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private static void DbgFailContains(string paramName)
        {
            DbgFailCore(GetMsg("Contains assertion failure: {0}", paramName));
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private static void DbgFailContains(string paramName, string msg)
        {
            DbgFailCore(GetMsg("Contains assertion failure: {0}: {1}", paramName, msg));
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private static void DbgFailValueInRange(string paramName)
        {
            DbgFailCore(GetMsg("Value-in-range assertion failure: {0}", paramName));
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private static void DbgFailValueInRange(string paramName, string msg)
        {
            DbgFailCore(GetMsg("Value-in-range assertion failure: {0}: {1}", paramName, msg));
        }
    }
}
