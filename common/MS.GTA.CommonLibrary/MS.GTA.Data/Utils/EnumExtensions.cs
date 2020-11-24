//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="EnumExtensions.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.Data.Utils
{
    using System;

    public static partial class EnumExtensions
    {
        public static T? TryParseAsEnum<T>(this string value)
            where T : struct
        {
            if (!string.IsNullOrEmpty(value)
                && Enum.TryParse(value, true, out T enumValue)
                && Enum.IsDefined(typeof(T), enumValue))
            {
                return (T?)enumValue;
            }

            return null;
        }

        public static Guid? ToGuid(this string value)
        {
            if (!string.IsNullOrEmpty(value)
                && Guid.TryParse(value, out var guid)
                && guid != Guid.Empty)
            {
                return guid;
            }

            return null;
        }

        public static Guid CheckGuid(this string value)
        {
            if (!string.IsNullOrEmpty(value)
                && Guid.TryParse(value, out var guid)
                && guid != Guid.Empty)
            {
                return guid;
            }

            throw new ArgumentException($"Value {value} cannot be turned into a Guid", nameof(value));
        }
    }
}
