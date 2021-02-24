//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace MS.GTA.CommonDataService.Common.Internal
{
    /// <summary>
    /// String manipulation utility methods.
    /// </summary>
    /// <remarks>
    /// These APIs support the SDK infrastructure and are not intended to be used
    /// directly from your code. The APIs may change or be removed in future releases.
    /// </remarks>
    public static class StringUtil
    {
        private const string ContinuationPostfix = "...";

        /// <summary>
        /// Formats a string using the <see cref="CultureInfo.InvariantCulture"/>.
        /// </summary>
        /// <param name="format">Format string.</param>
        /// <param name="args">Format arguments.</param>
        /// <returns>A string formatted using the <see cref="CultureInfo.InvariantCulture"/>.</returns>
        [DebuggerStepThrough]
        public static string FormatInvariant(string format, params object[] args)
        {
            return string.Format(CultureInfo.InvariantCulture, format, args);
        }

        /// <summary>
        /// Formats a string.
        /// </summary>
        /// <param name="culture">Culture to use to format the string.</param>
        /// <param name="format">Format string.</param>
        /// <param name="args">Format arguments.</param>
        /// <returns>A string formatted using the <see cref="CultureInfo.InvariantCulture"/>.</returns>
        [DebuggerStepThrough]
        public static string Format(CultureInfo culture, string format, params object[] args)
        {
            return string.Format(culture, format, args);
        }

        /// <summary>
        /// Determines whether two specified String objects have the same value using the <see cref="StringComparison.OrdinalIgnoreCase"/> comparison.
        /// </summary>
        [DebuggerStepThrough]
        public static bool EqualsOrdinalIgnoreCase(string arg0, string arg1)
        {
            return string.Equals(arg0, arg1, StringComparison.OrdinalIgnoreCase);
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "NotImplemented")]
        public static bool EqualsOrdinal(string propName, object entitySet)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines whether two specified String objects have the same value using the <see cref="StringComparison.Ordinal"/> comparison.
        /// </summary>
        [DebuggerStepThrough]
        public static bool EqualsOrdinal(string arg0, string arg1)
        {
            return string.Equals(arg0, arg1, StringComparison.Ordinal);
        }

        /// <summary>
        /// Determines if the prefix string is a prefix of the original string using <see cref="StringCmparison.OrginalIgnoreCase"/> comparison
        /// </summary>
        [DebuggerStepThrough]
        public static bool StartsWithOrdinalIgnoreCase(string originalString, string prefixString)
        {
            return originalString?.StartsWith(prefixString, StringComparison.OrdinalIgnoreCase) ?? false;
        }

        /// <summary>
        /// Determines if the prefix string is a prefix of the original string using <see cref="StringCmparison.Orginal"/> comparison
        /// </summary>
        [DebuggerStepThrough]
        public static bool StartsWithOrdinal(string originalString, string prefixString)
        {
            return originalString?.StartsWith(prefixString, StringComparison.Ordinal) ?? false;
        }

        /// <summary>
        /// Determines whether the specified string is a CLS-compliant identifier.
        /// </summary>
        /// <param name="s">A string which may be a CLS-compliant identifier.</param>
        /// <returns>
        ///    <c>true</c> if the specified string is a CLS compliant identifier; otherwise, <c>false</c>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsClsCompliantIdentifier(string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;

            if (!IsClsCompliantIdentifierChar(s[0], true))
                return false;

            for (int i = 1; i < s.Length; i++)
            {
                if (!IsClsCompliantIdentifierChar(s[i], false))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether the specified character is a valid CLS identifier character.
        /// </summary>
        /// <param name="c">A character value.</param>
        /// <param name="firstChar">Indicates whether the character is the first character in an identifier.
        ///     Different rules govern CLS-compliance for the initial character in an identifier.</param>
        /// <returns>
        ///     <c>true</c> if the specified character is a CLS-compliant identifier character; otherwise, <c>false</c>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsClsCompliantIdentifierChar(char c, bool firstChar)
        {
            // CLS-compliant language compilers must follow the rules of Annex 7 of Technical Report 15 of the 
            // Unicode Standard 3.0, which governs the set of characters that can start and be included in identifiers.
            // This standard is available at http://www.unicode.org/unicode/reports/tr15/tr15-18.html.
            // <identifier> ::= <identifier_start> ( <identifier_start> | <identifier_extend> )*

            UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(c);
            switch (uc)
            {
                // These characters can occur in any position
                // <identifier_start> ::= [{Lu}{Ll}{Lt}{Lm}{Lo}{Nl}]
                case UnicodeCategory.UppercaseLetter:        // Lu
                case UnicodeCategory.LowercaseLetter:        // Ll
                case UnicodeCategory.TitlecaseLetter:        // Lt
                case UnicodeCategory.ModifierLetter:         // Lm
                case UnicodeCategory.OtherLetter:            // Lo
                case UnicodeCategory.LetterNumber:           // Nl
                    return true;

                // These characters can occur in any position but the first
                // <identifier_extend> ::= [{Mn}{Mc}{Nd}{Pc}{Cf}]
                case UnicodeCategory.NonSpacingMark:         // Mn
                case UnicodeCategory.SpacingCombiningMark:   // Mc
                case UnicodeCategory.DecimalDigitNumber:     // Nd
                case UnicodeCategory.ConnectorPunctuation:   // Pc
                case UnicodeCategory.Format:                 // Cf
                    return !firstChar;

                // Other characters are invalid everywhere
                default:
                    return false;
            }
        }

        /// <summary>
        /// Derives a CLS-compliant name from the specified string.
        /// </summary>
        /// <param name="s">Input string which may include any characters.</param>
        /// <param name="fallbackName">CLS-compliant fallback name to use if no allowed characters present in <paramref name="s"/>.</param>
        [DebuggerStepThrough]
        public static string DeriveClsCompliantName(string s, string fallbackName)
        {
            Contract.AssertNonEmpty(s, "s");
            Contract.AssertNonEmpty(fallbackName, "fallbackName");
            Contract.Assert(IsClsCompliantIdentifier(fallbackName), "StringUtil.IsClsCompliantIdentifier(fallbackName)");

            var builder = new StringBuilder(s.Length);

            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (builder.Length > 0 && (char.IsSeparator(c) || c == '.' || c == '/'))
                {
                    builder.Append('_');
                }
                else if (IsClsCompliantIdentifierChar(c, builder.Length == 0))
                {
                    builder.Append(c);
                }
                else if (builder.Length == 0 && IsClsCompliantIdentifierChar(c, firstChar: false))
                {
                    // Prefix with fallbackName if first char would be valid as non-first char
                    builder.Append(fallbackName);
                    builder.Append(c);
                }
            }

            return builder.Length > 0 ? builder.ToString() : fallbackName;
        }

        /// <summary>
        /// Makes a unique name based on the <paramref name="candidateName"/> by appending a numeric suffix if necessary.
        /// </summary>
        /// <param name="candidateName">Candidate name to use.</param>
        /// <param name="namesInUse">List of all the names that are used now.</param>
        /// <returns>Candidate name or a derived name which is not currently in use.</returns>
        [DebuggerStepThrough]
        public static string MakeUniqueName(string candidateName, ISet<string> namesInUse)
        {
            Contract.AssertValue(namesInUse, nameof(namesInUse));

            return MakeUniqueName(candidateName, namesInUse.Contains);
        }

        /// <summary>
        /// Makes a unique name based on the <paramref name="candidateName"/> by appending a numeric suffix if necessary.
        /// </summary>
        /// <param name="candidateName">Candidate name to use.</param>
        /// <param name="isUsedPredicate">A predicate indicating if the name is in use.</param>
        /// <returns>Candidate name or a derived name which is not currently in use.</returns>
        [DebuggerStepThrough]
        public static string MakeUniqueName(string candidateName, Func<string, bool> isUsedPredicate)
        {
            Contract.Assert(isUsedPredicate != null, nameof(isUsedPredicate));

            string baseName = candidateName;
            int i = 1;
            while (isUsedPredicate(candidateName))
            {
                candidateName = string.Concat(baseName, (i++).ToString(CultureInfo.InvariantCulture));
            }
            return candidateName;
        }

        [DebuggerStepThrough]
        public static StringBuilder AppendFormatInvariant(this StringBuilder builder, string format, params object[] args)
        {
            Contract.AssertValue(builder, "builder");

            builder.AppendFormat(CultureInfo.InvariantCulture, format, args);
            return builder;
        }

        [DebuggerStepThrough]
        public static StringBuilder AppendMany(this StringBuilder builder, IEnumerable<object> items, string delimiter)
        {
            Contract.AssertValue(builder, "builder");
            Contract.AssertNonEmpty(delimiter, "delimiter");
            Contract.AssertValue(items, "items");

            bool needsDelimiter = false;
            foreach (var item in items)
            {
                if (needsDelimiter)
                    builder.Append(delimiter);
                else
                    needsDelimiter = true;

                builder.Append(item);
            }
            return builder;
        }

        [DebuggerStepThrough]
        public static string ToCommaDelimitedText<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, string pairsDelimiter = null, string keyValueDelimiter = null)
        {
            Contract.AssertValue(dictionary, "dictionary");

            var builder = new StringBuilder();
            foreach (var pair in dictionary)
            {
                if (builder.Length > 0)
                {
                    if (pairsDelimiter == null)
                        builder.AppendLine();
                    else
                        builder.Append(pairsDelimiter);
                }

                builder.Append(pair.Key);
                builder.Append(keyValueDelimiter ?? ", ");
                builder.Append(pair.Value);
            }

            return builder.ToString();
        }

        [DebuggerStepThrough]
        public static string Trim(this string text, ref int startIndex, ref int endIndex)
        {
            Contract.AssertValue(text, "text");
            Contract.Assert(startIndex >= 0 && startIndex <= endIndex && endIndex < text.Length, "Invalid startIndex and/or endIndex");

            // trim left spaces
            while (startIndex < endIndex
                && char.IsWhiteSpace(text[startIndex]))
            {
                startIndex++;
            }

            // trim right spaces
            while (endIndex >= startIndex
                && char.IsWhiteSpace(text[endIndex]))
            {
                endIndex--;
            }

            return text.Substring(startIndex, endIndex - startIndex + 1);
        }

        [DebuggerStepThrough]
        public static string Chop(this string text, int maxLength)
        {
            Contract.CheckRange(maxLength > 0, nameof(maxLength));

            if (text.Length <= maxLength)
                return text;

            bool addPostfix = true;
            int substringLength = maxLength - ContinuationPostfix.Length;

            // add a postfix only if it's shorter than length
            if (ContinuationPostfix.Length >= maxLength)
            {
                addPostfix = false;
                substringLength = maxLength;
            }

            var fixedMessage = new StringBuilder(text, 0, substringLength, maxLength);
            if (addPostfix)
            {
                fixedMessage.Append(ContinuationPostfix);
            }

            Contract.Assert(fixedMessage.Length <= maxLength, nameof(fixedMessage));
            return fixedMessage.ToString();
        }
    }
}
