//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

namespace MS.GTA.CommonDataService.Common
{
    /// <summary>
    /// Modernize the enumeration accessors
    /// </summary>
    [Obsolete("These APIs are obsolete and will be removed in a future release. Use an implementation under the MS.GTA.CommonDataService.Common.Internal namespace instead.")]
    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = "'Enum' is intented to be part of the class. Approved.")]
    internal static class EnumUtil
    {
        /// <summary>
        /// Options for <see cref="Parse{T}(String)"/>
        /// </summary>
        [Flags]
        internal enum ParseOptions
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
        internal static T Parse<T>(string value)
        {
            return Parse<T>(value, ParseOptions.None);
        }

        /// <summary>
        /// Parses the specified value into an enumeration type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="options">The options.</param>
        internal static T Parse<T>(string value, ParseOptions options)
        {
            return (T)Enum.Parse(typeof(T), value, options.HasFlag(ParseOptions.IgnoreCase));
        }
    }
}
