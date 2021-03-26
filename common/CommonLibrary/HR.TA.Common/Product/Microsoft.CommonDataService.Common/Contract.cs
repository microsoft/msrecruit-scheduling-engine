using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.CommonDataService.Common.Internal
{
    public static class Contract
    {
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void Assert(bool f, string msg) {}
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void Assert(bool f, string msg, params object[] args) {}
        [Conditional("DEBUG")]
        public static void AssertAll<T>(IList<T> args, Func<T, T, bool> condition, string msg) {}
        [Conditional("DEBUG")]
        public static void AssertAll<T>(IList<T> args, Func<T, bool> condition, string msg) {}
        /*[Conditional("DEBUG")]
        public static void AssertAll<T>(IList<T> args, string msg) where T : struct, ICheckable;
        [Conditional("DEBUG")]
        public static void AssertAll<K, V>(IDictionary<K, V> args, string msg) where V : struct, ICheckable;
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertAllAreOfType<S, T>(IList<S> values, string paramName)
            where S : class
            where T : S;*/
        [Conditional("DEBUG")]
        public static void AssertAllNonEmpty(IList<string> args, string paramName) {}
        [Conditional("DEBUG")]
        public static void AssertAllNonEmpty(IList<string> args, string paramName, string msg) {}
        [Conditional("DEBUG")]
        public static void AssertAllValues<TKey, TValue>(IDictionary<TKey, TValue> args, string paramName, string msg) where TValue : class {}
        [Conditional("DEBUG")]
        public static void AssertAllValues<TKey, TValue>(IDictionary<TKey, TValue> args, string paramName) where TValue : class {}
        [Conditional("DEBUG")]
        public static void AssertAllValues<T>(IList<T> args, string paramName, string msg) where T : class {}
        [Conditional("DEBUG")]
        public static void AssertAllValues<T>(IList<T> args, string paramName) where T : class {}
        [Conditional("DEBUG")]
        public static void AssertAllValuesNested<T>(IList<IList<T>> args, string paramName) where T : class {}
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertContainsOrNull<T>(IList<T> items, T item, string paramName, string msg) {}
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertContainsOrNull<T>(IEnumerable<T> items, T item, string paramName) {}
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertInvalidSwitchValue<T>(T value, string paramName) where T : struct { }
        /*[Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertIsExactType<T>(object val, string paramName, string msg) where T : class {}
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertIsExactType<T>(object val, string paramName) where T : class {}
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertIsExactTypeOrNull<T>(object val, string paramName) where T : class {}
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertIsOfType<T>(object val, string paramName, string msg) where T : class {}
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertIsOfType<T>(object val, string paramName) where T : class {}
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertIsOfTypeOrNull<T>(object val, string paramName) where T : class {}
        */[Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertNonEmpty<T>(ICollection<T> args, string paramName, string msg) {}
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertNonEmpty<T>(ICollection<T> args, string paramName) {}
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertNonEmpty(Guid guid, string paramName) {}
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertNonEmpty(string s, string paramName, string msg) {}
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertNonEmpty(string s, string paramName) {}
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertNonEmpty<K, T>(IDictionary<K, T> args, string paramName) {}
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertNonEmpty(Guid guid, string paramName, string msg) {}
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertNonEmptyAndAllValues<T>(IList<T> args, string paramName) where T : class {}
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertNonEmptyAndAllValues<T>(IList<T> args, string paramName, string msg) where T : class {}
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertNonEmptyReadOnly<T>(IReadOnlyCollection<T> args, string paramName) {}
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertNonEmptyReadOnly<T>(IReadOnlyCollection<T> args, string paramName, string msg) {}
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertNonEmptyReadOnly<K, T>(IReadOnlyDictionary<K, T> args, string paramName) {}
        /*[Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertValid<T>(T val, string paramName) where T : ICheckable;*/
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertValue<T>(T val, string paramName) where T : class {}
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertValue<T>(T val, string paramName, string msg) where T : class {}
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertValue(IntPtr val, string paramName) {}
        /*[Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertValueGeneric<T>(T val, string paramName) {}
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertValueInRange(int value, int exclusiveUpperBound, string paramName) {}
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertValueInRange(int value, int inclusiveLowerBound, int exclusiveUpperBound, string paramName) {}
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertValueInRange(int value, int exclusiveUpperBound, string paramName, string msg) {}
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertValueInRange(int value, int inclusiveLowerBound, int exclusiveUpperBound, string paramName, string msg) {}
        */
        [Conditional("INVARIANT_CHECKS")]
        public static void AssertValueOrNull<T>(T val, string paramName) {}
        [Conditional("INVARIANT_CHECKS")]
        public static void AssertValueOrNull<T>(T val, string paramName, string msg) where T : class {}
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void AssertWeightValue(double value, string paramName) {}
        [DebuggerStepThrough]
        public static void Check(bool f, string msg) {}
        // public static void CheckAll<T>(IList<T> args, string paramName) where T : struct, ICheckable { }
        public static void CheckAllNonEmpty(IList<string> args, string paramName) {}
        public static void CheckAllValues<T>(IList<T> args, string paramName) where T : class {}
        public static void CheckAllValues<TKey, TValue>(IDictionary<TKey, TValue> args, string paramName) where TValue : class {}
        public static void CheckIsExactType<T>(object val, string paramName) where T : class {}
        public static void CheckIsExactType<T>(object val, string paramName, string msg) where T : class {}
        public static void CheckIsExactTypeOrNull<T>(object val, string paramName) where T : class {}
        public static void CheckIsNotOfType<T>(object val, string paramName, string msg) where T : class {}
        public static void CheckIsNotOfType<T>(object val, string paramName) where T : class {}
        public static void CheckIsNotOfTypeOrNull<T>(object val, string paramName, string msg) where T : class {}
        public static void CheckIsNotOfTypeOrNull<T>(object val, string paramName) where T : class {}
        public static void CheckIsOfType<T>(object val, string paramName, string msg) where T : class {}
        public static void CheckIsOfType<T>(object val, string paramName) where T : class {}
        public static void CheckIsOfTypeOrNull<T>(object val, string paramName, string msg) where T : class {}
        public static void CheckIsOfTypeOrNull<T>(object val, string paramName) where T : class {}
        [DebuggerStepThrough]
        public static void CheckNonEmpty(string s, string paramName, string msg) {}
        [DebuggerStepThrough]
        public static void CheckNonEmpty<K, T>(IDictionary<K, T> args, string paramName) {}
        [DebuggerStepThrough]
        public static void CheckNonEmpty<T>(ICollection<T> args, string paramName) {}
        [DebuggerStepThrough]
        public static void CheckNonEmpty<K, T>(IReadOnlyDictionary<K, T> args, string paramName) {}
        [DebuggerStepThrough]
        public static void CheckNonEmpty(string s, string paramName) {}
        [DebuggerStepThrough]
        public static void CheckNonWhitespace(string s, string paramName, string msg) {}
        [DebuggerStepThrough]
        public static void CheckNonWhitespace(string s, string paramName) {}
        [DebuggerStepThrough]
        public static void CheckParam(bool f, string paramName, string msg) {}
        [DebuggerStepThrough]
        public static void CheckParam(bool f, string paramName) {}
        [DebuggerStepThrough]
        public static void CheckRange(bool f, string paramName, string msg) {}
        [DebuggerStepThrough]
        public static void CheckRange(bool f, string paramName) {}
        [DebuggerStepThrough]
        public static void CheckValue<T>(T val, string paramName, string msg) where T : class {}
        [DebuggerStepThrough]
        public static void CheckValue<T>(T val, string paramName) where T : class {}
        [Conditional("INVARIANT_CHECKS")]
        public static void CheckValueOrNull<T>(T val, string paramName) where T : class {}
        [Conditional("INVARIANT_CHECKS")]
        public static void CheckValueOrNull<T>(T val, string paramName, string msg) where T : class {}
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void DebugFail(string msg, params object[] args) {}
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void DebugFail(string msg) {}
        /*
        public static Exception Except(string msg) {}
        public static Exception Except(string msg, params object[] args) {}
        public static Exception Except(string msg, Exception innerException) {}
        public static Exception Except(string msg, Exception innerException, params object[] args) {}
        public static Exception ExceptEmpty(string paramName, string msg) {}
        public static Exception ExceptEmpty(string paramName) {}
        public static Exception ExceptNotSupported(string msg) {}
        public static Exception ExceptParam<T>(string paramName, string msg, params object[] args) {}
        public static Exception ExceptParam(string paramName) {}
        public static Exception ExceptParam(string paramName, string msg) {}
        public static Exception ExceptRange(string paramName, string msg) {}
        public static Exception ExceptRange(string paramName) {}
        public static Exception ExceptValid(string paramName, string msg) {}
        public static Exception ExceptValid(string paramName) {}
        public static Exception ExceptValue(string paramName) {}
        public static Exception ExceptValue(string paramName, string msg) {}
        [DebuggerStepThrough]
        public static void TraceFail(ITracer tracer, string msg, params object[] args) {}
        [DebuggerStepThrough]
        public static void TraceFail(ITracer tracer, string msg) {}
        [DebuggerStepThrough]
        public static bool TrySetDebugFail(Action<string> failAction) {}*/
    }
}
