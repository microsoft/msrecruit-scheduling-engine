//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

namespace MS.GTA.CommonDataService.Common.Internal
{
    /// <summary>
    /// Modernize the enumeration accessors
    /// </summary>
    /// <remarks>
    /// These APIs support the SDK infrastructure and are not intended to be used
    /// directly from your code. The APIs may change or be removed in future releases.
    /// </remarks>
    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = "'Enum' is intented to be part of the class. Approved.")]
    public static class EnumUtil
    {
        /// <summary>
        /// Options for <see cref="Parse{T}(String)"/>
        /// </summary>
        [Flags]
        public enum ParseOptions
        {
            /// <summary>
            /// None
            /// </summary>
            None = 0x00,

            /// <summary>
            /// Ignore case
            /// </summary>
            IgnoreCase = 0x01,
        }

        /// <summary>
        /// Parses the specified value into an enumeration type.
        /// </summary>
        /// <param name="value">The value.</param>
        public static T Parse<T>(string value)
        {
            return Parse<T>(value, ParseOptions.None);
        }

        /// <summary>
        /// Parses the specified value into an enumeration type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="options">The options.</param>
        public static T Parse<T>(string value, ParseOptions options)
        {
            return (T)Enum.Parse(typeof(T), value, options.HasFlag(ParseOptions.IgnoreCase));
        }
    }
}
