
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Microsoft.CommonDataService.Common.Internal
{
    public static class StringUtil
    {
        /*   [DebuggerStepThrough]
           public static StringBuilder AppendFormatInvariant(this StringBuilder builder, string format, params object[] args);
           [DebuggerStepThrough]
           public static StringBuilder AppendMany(this StringBuilder builder, IEnumerable<object> items, string delimiter);
           */
        [DebuggerStepThrough]
        public static string Chop(this string text, int maxLength)
        {
            return text?.Substring(0, Math.Min(text.Length, maxLength));
        }
          /* [DebuggerStepThrough]
           public static string DeriveClsCompliantName(string s, string fallbackName);
           public static bool EqualsOrdinal(string propName, object entitySet);
           [DebuggerStepThrough]
           public static bool EqualsOrdinal(string arg0, string arg1);
           [DebuggerStepThrough]
           public static bool EqualsOrdinalIgnoreCase(string arg0, string arg1);
           [DebuggerStepThrough]
           public static string Format(CultureInfo culture, string format, params object[] args);
           */
        [DebuggerStepThrough]
        public static string FormatInvariant(string format, params object[] args)
        {
            return string.Format(format, args);
        }
        /*[DebuggerStepThrough]
        public static bool IsClsCompliantIdentifier(string s);
        [DebuggerStepThrough]
        public static bool IsClsCompliantIdentifierChar(char c, bool firstChar);
        [DebuggerStepThrough]
        public static string MakeUniqueName(string candidateName, ISet<string> namesInUse);
        [DebuggerStepThrough]
        public static string MakeUniqueName(string candidateName, Func<string, bool> isUsedPredicate);
        [DebuggerStepThrough]
        public static bool StartsWithOrdinal(string originalString, string prefixString);
        [DebuggerStepThrough]
        public static bool StartsWithOrdinalIgnoreCase(string originalString, string prefixString);
        [DebuggerStepThrough]
        public static string ToCommaDelimitedText<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, string pairsDelimiter = null, string keyValueDelimiter = null);
        [DebuggerStepThrough]
        public static string Trim(this string text, ref int startIndex, ref int endIndex);*/
    }
}
