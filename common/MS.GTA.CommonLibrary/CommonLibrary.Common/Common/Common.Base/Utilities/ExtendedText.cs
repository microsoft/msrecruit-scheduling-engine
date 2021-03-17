//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace CommonLibrary.Common.Base.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using CommonDataService.Common.Internal;

    /// <summary>
    /// ExtendedText class to simplify string related operations
    /// </summary>
    public static class ExtendedText
    {
        /// <summary>
        /// Array.Join without the array
        /// </summary>
        /// <typeparam name="T">Generic object type</typeparam>
        /// <param name="sequence">The sequence for join.</param>
        /// <param name="separator">The separator, to separate items in the sequence.</param>
        /// <param name="converter">The converter to apply to the items.</param>
        /// <returns>Joint string</returns>
        public static string StringJoin<T>(
            this IEnumerable<T> sequence,
            string separator,
            Func<T, string> converter)
        {
            Contract.CheckValue(sequence, nameof(sequence));

            return string.Join(separator, sequence.Select(converter));
        }

        /// <summary>
        /// Join a sequence of objects into a string, separated by a separator
        /// </summary>
        /// <typeparam name="T">Generic object type</typeparam>
        /// <param name="sequence">Sequence of objects of type T</param>
        /// <param name="separator">Separator to use when joining the sequence of objects</param>
        /// <returns>Joint string</returns>
        public static string StringJoin<T>(this IEnumerable<T> sequence, string separator)
        {
            Contract.CheckValue(sequence, nameof(sequence));

            return sequence.StringJoin(separator, item => item.ToString());
        }

        /// <summary>
        /// Replaces the format item in a specified string with the string representation
        /// of a corresponding object in a specified array with an invariant culture.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>A copy of format in which the format items have been replaced by the string
        /// representation of the corresponding objects in args.</returns>
        /// <exception cref="System.ArgumentNullException">format or args is null.</exception>
        /// <exception cref="System.FormatException">format is invalid.-or- The index of a format item is less than zero, or greater
        /// than or equal to the length of the args array.</exception>
        public static string FormatWithInvariantCulture(this string format, params object[] args)
        {
            return FormatWith(format, CultureInfo.InvariantCulture, args);
        }

        /// <summary>
        /// Replaces the format item in a specified string with the string representation
        /// of a corresponding object in a specified array. A specified parameter supplies
        /// culture-specific formatting information.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>A copy of format in which the format items have been replaced by the string
        /// representation of the corresponding objects in args.</returns>
        /// <exception cref="System.ArgumentNullException">format or args is null.</exception>
        /// <exception cref="System.FormatException">format is invalid.-or- The index of a format item is less than zero, or greater
        /// than or equal to the length of the args array.</exception>        
        public static string FormatWith(this string format, IFormatProvider formatProvider, params object[] args)
        {
            return string.Format(formatProvider, format, args);
        }

        /// <summary>
        /// Can be used to replace a substring with the additional comparison Type input.
        /// </summary>
        /// <param name="originalString">string to replace the substring from.</param>
        /// <param name="oldValue">old value of the substring.</param>
        /// <param name="newValue">New value of the substring.</param>
        /// <param name="comparisonType">Comparison type enum.</param>
        /// <returns>A new string.</returns>
        public static string Replace(this string originalString, string oldValue, string newValue, StringComparison comparisonType)
        {
            Contract.CheckValue(oldValue, nameof(oldValue));
            Contract.CheckValue(newValue, nameof(newValue));

            int startIndex = 0;
            while (true)
            {
                startIndex = originalString.IndexOf(oldValue, startIndex, comparisonType);
                if (startIndex == -1)
                {
                    break;
                }

                originalString = originalString.Substring(0, startIndex) + newValue + originalString.Substring(startIndex + oldValue.Length);

                startIndex += newValue.Length;
            }

            return originalString;
        }
    }
}
